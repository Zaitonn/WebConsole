import { ElMessage } from 'element-plus'

let wsclient = null;
const cfg = {};

const eventListener = {};

/**
 * 检查数据
 * @param {String} addr 地址
 * @param {String} account 账号
 * @param {String} pwd 密码
 * @returns 检查结果
 */
function check(addr, account, pwd) {
    let msg;

    if (!addr || typeof (addr) != 'string')
        msg = '地址不能为空';

    else if (!/^wss?:\/\//.test(addr))
        msg = '地址格式不正确';

    else if (!account || typeof (account) != 'string')
        msg = '账号不能为空';

    else if (!pwd || typeof (pwd) != 'string')
        msg = '密码不能为空';

    else if (![-1, 0, 3].includes(getStatus()))
        msg = 'ws已连接';

    if (msg) {
        ElMessage({
            showClose: true,
            message: msg,
            type: 'warning',
            grouping: true
        });
    }

    return msg;
}

/**
 * 连接ws
 * @param {String} addr 地址
 * @param {String} account 账号
 * @param {String} pwd 密码
 * @returns 连接结果
 */
export function connectTo(addr, account, pwd) {
    const checkResult = check(addr, account, pwd);
    if (checkResult || ![-1, 0, 3].includes(getStatus()))
        return false;

    cfg.addr = addr;
    cfg.account = account;
    cfg.pwd = pwd;

    try {

        wsclient = new WebSocket(addr);
        wsclient.onopen = onopen;
        wsclient.onclose = onclose;

        return true;
    } catch (error) {
        ElMessage({
            showClose: true,
            message: error.message,
            type: 'error',
            grouping: true
        });

        return false;
    }
}

/**
 * 开启事件
 * @param {Event} ev 事件对象
 */
function onopen(ev) {
    ElMessage({
        showClose: true,
        message: '连接成功',
        type: 'success',
        grouping: true
    });

    if (eventListener['onopen'] && typeof (eventListener['onopen']) === 'function')
        eventListener['onopen'](ev);
}

/**
 * 关闭事件
 * @param {Event} ev 事件对象
 */
function onclose(ev) {
    ElMessage({
        showClose: true,
        message: 'ws连接已断开',
        type: 'warning',
        grouping: true
    });

    if (eventListener['onclose'] && typeof (eventListener['onclose']) === 'function')
        eventListener['onclose'](ev);
}

/**
 * 获取当前状态
 * @returns 状态数
 */
export default function getStatus() {
    return wsclient == null ? -1 : wsclient.readyState;
}

/**
 * 设置监听器
 * @param {String} event 事件
 * @param {Function} callback 回调函数
 */
export function setListener(event, callback) {
    event = event.toLowerCase();

    if (!['onopen', 'onclose', 'onverifysuccess'].includes(event))
        throw new Error('监听事件名称错误');

    eventListener[event] = callback;
}