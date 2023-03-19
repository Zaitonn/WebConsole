(function () {
    let ws = null;
    let noticeContainer = document.querySelector('#notice-container'), currentNotices = [];
    let addr, pwd;
    let connected = false;

    restoreFromUrl();

    document.querySelector('#connect-body button#connect-btn').addEventListener('click', tryConnect);
    document.querySelector('#connect-body input#connect-addr').addEventListener('keydown', (ev) => ev.keyCode === 13 && document.querySelector('#connect-body input#connect-pwd').focus());
    document.querySelector('#connect-body input#connect-pwd').addEventListener('keydown', (ev) => ev.keyCode === 13 && tryConnect());

    /**
     * 更新输入内容
     */
    function updateInput() {
        addr = document.querySelector('#connect-body input#connect-addr').value.trim();
        pwd = document.querySelector('#connect-body input#connect-pwd').value;
    }

    /**
     * 从url参数恢复地址
     */
    function restoreFromUrl() {
        let url = new URL(window.location.href);
        if (url.searchParams.has('addr'))
            document.querySelector('#connect-body input#connect-addr').value = url.searchParams.get('addr');

        updateInput();

        if (url.searchParams.has('debug'))
            showPanel();

    }

    /**
     * 尝试连接
     */
    function tryConnect() {
        if (checkConnectInput())
            if (ws === null || ws.readyState === 3) {
                try {
                    ws = new WebSocket(document.querySelector('#connect-body input#connect-addr').value.trim());
                    ws.onopen = onOpen;
                    ws.onclose = onClose;
                    ws.onmessage = onMessage;
                } catch (e) {
                    createNotice(2, `初始化ws实例失败：${e.message}`);
                }
            } else
                createNotice(2, 'ws已连接或正在连接');
    }

    /**
     * 检查输入
     * @returns 结果
     */
    function checkConnectInput() {
        updateInput();

        if (!addr) {
            createNotice(1, 'ws地址栏为空');
            document.querySelector('#connect-body input#connect-addr').focus();
            return false;
        }

        if (!/^wss?:\/\//.test(addr)) {
            createNotice(1, 'ws地址不正确');
            document.querySelector('#connect-body input#connect-addr').focus();
            return false;
        }

        if (!pwd) {
            createNotice(1, '密码为空');
            document.querySelector('#connect-body input#connect-pwd').focus();
            return false;
        }

        history.pushState({ 'page_id': 1, 'user_id': 5 }, '', '?addr=' + encodeURIComponent(addr));
        return true;

    }

    /**
     * 创建提示
     * @param {Number} level 等级
     * @param {String} text 文本
     */
    function createNotice(level = 0, text = null, close = true) {
        console.log(text);
        if (noticeContainer.childElementCount > 10 || currentNotices.includes(text))
            return;

        currentNotices.push(text);
        let div = document.createElement('div');
        div.classList.add('notice');
        div.innerHTML += `<span class='symbol-${['info', 'warn', 'error'][level]} symbol'>!</span><span>${text}</span>`;
        noticeContainer.appendChild(div);
        setTimeout(() => div.setAttribute('style', 'transform: none'), 20);

        if (close) {
            setTimeout(() => noticeContainer.removeChild(div), 5500);
            setTimeout(() => div.setAttribute('style', '') || currentNotices.shift(), 5000);
        }
    }

    function showPanel() {
        document.querySelector('div#panel-container').classList.remove('hide');
        document.querySelector('div#panel-container nav span#panel-addr').innerHTML = addr.replace(/^ws:\/\//, '');
        setTimeout(() => {
            document.querySelector('div#connect-container').classList.add('hide');
            document.querySelector('div#bg-container').classList.add('hide');
        }, 1000);
        document.querySelector('div#panel-container nav span#panel-connection-state').setAttribute('style', 'color: #78d428');
    }

    /**
     * 开启事件处理
     */
    function onOpen() {
        createNotice(0, '连接成功');
        document.title = `WebConsole - ${addr.replace(/^ws:\/\//, '')}`;
    }

    /**
     * 关闭事件处理
     */
    function onClose() {
        createNotice(connected ? 2 : 1, connected ? '连接已断开，请刷新页面重试' : '连接已断开', !connected);
        document.querySelector('div#panel-container nav span#panel-connection-state').setAttribute('style', 'color: #e20');
        document.title = 'WebConsole';
    }

    /**
     * 接收消息事件
     * @param {MessageEvent} ev 消息事件
     */
    function onMessage(ev) {
        try {
            let data = JSON.parse(ev.data);
            console.log('receive:', data);
            handle(data);
        } catch (e) {
            createNotice(2, `接收数据包时出现问题：${e.message}`);
            throw e;
        }
    }

    /**
     * 处理数据包
     * @param {*} jsonBody json主体
     */
    function handle(jsonBody) {
        switch (jsonBody.type) {
            case 'action':
                handleAction(jsonBody);
                break;

            case 'event':
                handleEvent(jsonBody);
                break;

            default:
                break;
        }
    }

    /**
     * 处理动作数据包
     * @param {*} jsonBody json主体
     */
    function handleAction(jsonBody) {
        switch (jsonBody.sub_type) {
            case 'verify_request':
                ws.send(JSON.stringify({
                    type: 'action',
                    sub_type: 'verify',
                    data: {
                        md5: md5(jsonBody.data.random_key + pwd),
                        client_type: 'console'
                    }
                }))
                break;
        }
    }

    /**
     * 处理事件数据包
     * @param {*} jsonBody json主体
     */
    function handleEvent(jsonBody) {
        switch (jsonBody.sub_type) {
            case 'verify_result':
                if (jsonBody.data.success) {
                    showPanel();
                    createNotice(0, '验证通过');
                    connected = true;
                } else {
                    createNotice(2, `验证失败：${jsonBody.data.reason}`);
                }
                break;
            case 'disconnection':
                createNotice(1, `即将断开连接：${jsonBody.data.reason}`)
                break;
        }
    }

})();