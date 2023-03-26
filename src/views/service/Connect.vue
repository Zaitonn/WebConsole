<script setup>
import CommonLayout from '/src/components/layout/CommonLayout.vue'
import { ref } from 'vue'

const addr = ref('');
const account = ref('');
const pwd = ref('');
let connecting = false;
</script>

<template>
    <CommonLayout>
        <h1>
            <el-icon size="large">
                <Connection />
            </el-icon>
            连接
        </h1>
        <div>
            美好，从此处相遇
            <br>
            故事，从这里开始
        </div>
        <div id="form">
            <el-input v-model="addr" placeholder="ws://" prefix-icon="Link" />
            <el-input v-model="account" placeholder="账号" prefix-icon="User" />
            <el-input v-model="pwd" placeholder="密码" type="password" prefix-icon="Lock" />
        </div>
        <el-button type="primary" :loading="connecting">连接</el-button>
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