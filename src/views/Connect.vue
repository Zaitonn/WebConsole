<script setup>
import { ElMessage } from 'element-plus'
import { ref } from 'vue'
import { useRoute } from 'vue-router'

import CommonLayout from '/src/components/layout/CommonLayout.vue'
import { connectTo, setListener } from '/src/service'
import { setCookie, getCookies, deleteCookie } from '/src/utils/cookie.js'

const route = useRoute();

const currentCookie = getCookies();

const addr = ref(route.query['addr'] || currentCookie['addr']);
const account = ref(route.query['account'] || currentCookie['account']);
const pwd = ref(route.query['pwd'] || currentCookie['pwd']);
const savePwd = ref(Boolean(pwd.value));

const addrInput = ref();
const accountInput = ref();
const pwdInput = ref();

const isConnecting = ref(false);

const connect = () => {
    isConnecting.value = true;

    setCookie('addr', addr.value);
    setCookie('account', account.value);

    if (savePwd.value)
        setCookie('pwd', pwd.value);
    else
        deleteCookie('pwd');

    const connectResult = connectTo(addr.value, account.value, pwd.value);
    if (!connectResult) {
        isConnecting.value = false;

        if (!pwd.value)
            pwdInput.value.focus();

        if (!account.value)
            accountInput.value.focus();

        if (!addr.value)
            addrInput.value.focus();

    }
}

const onClickSavePwd = () => {
    if (savePwd.value)
        ElMessage({
            showClose: true,
            message: '请不要在非个人电脑上勾选此项 :D',
            grouping: true
        });
}

setListener('onclose', () => isConnecting.value = false);

</script>

<template>
    <CommonLayout>
        <h1 class="no-select">
            <el-icon size="large">
                <Connection />
            </el-icon>
            连接
        </h1>
        <div class="no-select">
            美好，从此处相遇
            <br>
            故事，从这里开始
        </div>
        <div id="form">
            <el-input v-model="addr" ref="addrInput" placeholder="ws://" prefix-icon="Link" />
            <el-input v-model="account" ref="accountInput" placeholder="账号" prefix-icon="User" />
            <el-input v-model="pwd" ref="pwdInput" placeholder="密码" type="password" prefix-icon="Lock" />
        </div>
        <div class="no-select">
            <el-checkbox v-model="savePwd" label="保存密码" @change="onClickSavePwd" />
        </div>
        <el-button class="no-select" type="primary" :loading="isConnecting" @click="connect">连接</el-button>
    </CommonLayout>
</template>

<style scoped>
div#form {
    width: 70%;
    max-width: 300px;
}

div#form>* {
    margin: 10px;
}
</style>