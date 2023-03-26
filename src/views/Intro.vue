<script setup>
import Header from '../components/Header.vue'
import { useRouter } from 'vue-router'
const router = useRouter();
</script>

<template>
    <Header></Header>
    <div id="content">
        <img alt="logo" src="../assets/favicon.png">
        <div id="title-container">
            <span>WebConsole</span>
            <span>for</span>
            <span>Serein</span>
        </div>
        <div id="description">
            一个专门为
            <a href="http://serein.cc" target="_blank">Serein</a>
            打造的多功能网页控制台
        </div>
        <div>
            <el-tooltip content="若是第一次使用请看看这里👇" placement="top">
                <el-button size="large" icon="Document">查看文档</el-button>
            </el-tooltip>
            <el-tooltip content="点此打开登录界面" placement="top">
                <el-button type="primary" size="large" @click="router.push('/login')" icon="Position">登录</el-button>
            </el-tooltip>
        </div>
    </div>
</template>

<style scoped>
div#content>* {
    margin: 10px;
}

div#content>img {
    image-rendering: pixelated;
    width: 100px;
    display: inline-block;
}


div#title-container {
    font-size: 40px;
    margin: 5px;
}

div#title-container span {
    margin: 7px;
    font-weight: bolder;
}

div#title-container span:last-child {
    color: var(--c-brand);
    background-image: linear-gradient(to top, #48c6ef 0%, #6f86d6 100%);
    background-clip: text;
    -webkit-background-clip: text;
    -webkit-text-fill-color: transparent;
}

div#content :deep(.el-button) {
    margin: 0 15px;
}
</style>