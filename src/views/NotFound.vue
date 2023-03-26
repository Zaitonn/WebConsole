<script setup>
import CommonLayout from '/src/components/layout/CommonLayout.vue'
</script>

<template>
    <CommonLayout>
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
    </CommonLayout>
</template>

<style scoped>
div#content-flex div:first-child {
    background-image: url('/src/assets/imgs/404.png');
    background-position: center;
    background-size: contain;
    background-repeat: no-repeat;
    width: 100%;
    height: 400px;
    max-height: 70%;
}

div#content-flex div {
    margin: 10px;
}
</style>

