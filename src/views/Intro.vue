<script setup>
import CommonLayout from '/src/components/layout/CommonLayout.vue'
import { useRouter } from 'vue-router'
const router = useRouter();
</script>

<template>
    <CommonLayout>
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
        <div id="button-container">
            <el-tooltip content="若是第一次使用请看看这里👇" placement="top-end">
                <el-button size="large" icon="Document">查看文档</el-button>
            </el-tooltip>
            <el-tooltip content="点此打开连接界面" placement="top-start">
                <el-button type="primary" size="large" @click="router.push('/connect')" icon="Position">连接</el-button>
            </el-tooltip>
        </div>
    </CommonLayout>
    <div style="height: 100vh;">11</div>
</template>

<style scoped>
div#content-flex>* {
    margin: 10px;
}

div#content-flex>img {
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
    background-image: linear-gradient(30deg, #48c6ef 0%, #6f86d6 100%);
    background-clip: text;
    -webkit-background-clip: text;
    -webkit-text-fill-color: transparent;
}

div#description {
    text-align: center;
}

div#content-flex :deep(.el-button) {
    margin: 5px;
}

div#button-container {
    display: flex;
    justify-content: center;
    flex-wrap: wrap;
    align-content: center;
    align-items: center;
}

@media screen and (max-width: 500px) {
    div#title-container {
        font-size: 20px;
    }

    div#title-container span {
        margin: 3px;
    }

    div#content-flex>img {
        image-rendering: pixelated;
        width: 25vw;
        max-width: 75px;
        display: inline-block;
    }
}
</style>