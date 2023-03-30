<script setup>
import { ref } from 'vue';
import DetailLayout from '/src/components/layout/DetailLayout.vue'

const count = 0;
const hasInstances = ref(count != 0);
const noInstances = ref(count === 0);
</script>

<template>
    <DetailLayout>
        <div id="search-container">
            <el-input prefix-icon="Search" placeholder="在此搜索" :disabled="noInstances" />
            <el-tooltip content="点此刷新实例列表">
                <el-button icon="Refresh" />
            </el-tooltip>
        </div>
        <div v-show="hasInstances">
            <span>
                当前在线{{ typeof (count) === 'number' ? count : 'null' }}个实例
            </span>
        </div>
        <div class="flex-grow" v-show="noInstances"></div>
        <el-empty description="一个实例都没有呢..." v-show="noInstances" />
        <div>
            找不到你的实例？点击这里查看<a href="#">解决方案</a>
        </div>
        <div class="flex-grow" v-show="noInstances"></div>
    </DetailLayout>
</template>

<style scoped>
div#search-container {
    width: 80%;
    display: flex;
    justify-items: center;
}

div#search-container>* {
    margin: 5px;
}
</style>