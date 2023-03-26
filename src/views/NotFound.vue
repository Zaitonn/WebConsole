<script setup>
import Header from '../components/Header.vue'
</script>

<template>
    <Header></Header>
    <div id="content">
        <div></div>
        <div>你访问的链接
            <a :href=this.$route.fullPath> {{ this.$route.fullPath }} </a>
            不存在
        </div>
        <div>
            <el-button @click="$router.push('/')">返回首页</el-button>
            <el-button type="primary" @click="$router.go(-1)">
                上一页
            </el-button>
        </div>
    </div>
</template>

<style scoped>


div#content div:first-child {
    background-image: url('/src/assets/imgs/404.png');
    background-position: center;
    background-size: contain;
    background-repeat: no-repeat;
    width: 100%;
    height: 400px;
    max-height: 70%;
}

div#content div {
    margin: 10px;
}
</style>

