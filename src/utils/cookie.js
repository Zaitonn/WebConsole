/**
 * 设置Cookie
 * @param {String} key 键
 * @param {String} value 值
 */
export function setCookie(key, value) {
    if (!key || !value)
        return;

    document.cookie = `${encodeURI(key)}=${encodeURI(value)}; `;
}

/**
 * 获取当前的所有Cookies
 * @returns Cookies字典
 */
export function getCookies() {
    const cookie = document.cookie;
    let cookies = {};
    for (const item of cookie.split(';')) {
        if (item.indexOf('=') < 0)
            continue;

        const kv = item.trim().split('=');
        cookies[decodeURI(kv[0])] = decodeURI(kv[1]);
    }
    return cookies;
}

export function getCookie(key) {
    return getCookies()[key];
}

export function deleteCookie(key) {
    document.cookie = `${encodeURI(key)}=; expires=Thu, 01 Jan 1970 00:00:00 GMT`;
}