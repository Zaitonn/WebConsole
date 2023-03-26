<script setup>
import Logo from './Logo.vue'
import { useDark, useToggle } from '@vueuse/core'
import { useRoute, useRouter } from 'vue-router'
import { ref } from 'vue'

const router = useRouter();
const isDark = useDark();
const value = ref(isDark.value);
const toggleDark = useToggle(isDark);
const handleSelect = (_, keyPath) => {
    switch (keyPath[keyPath.length - 1]) {
        case '1':
            router.push('/overview');
            break;
        case '3-2':
            window.open('https://github.com/Zaitonn/WebConsole', 'blank');
        default:
            router.push('/');
            break;
    }
};

const activeIndex = (() => {
    switch (useRoute().path.split('/')[1]) {
        case 'overview':
            return '1';
        case 'login':
            return '2';
        default:
            return '-1';
    }
})();

</script>

<template>
    <div id="header">
        <Logo @click="$router.push('/')" />
        <div id="flex-grow"></div>
        <el-menu :default-active="activeIndex" class="el-menu-demo" mode="horizontal" :ellipsis="false"
            @select="handleSelect">
            <el-menu-item index="1">总览</el-menu-item>
            <el-menu-item index="2">连接</el-menu-item>
            <el-sub-menu index="3">
                <template #title>其他</template>
                <el-menu-item index="3-1">文档</el-menu-item>
                <el-menu-item index="3-2">GitHub</el-menu-item>
                <el-menu-item index="3-3">退出</el-menu-item>
            </el-sub-menu>
        </el-menu>
        <el-switch v-model="value" size="small" @change="toggleDark" active-value="true" active-icon="Moon"
            inactive-icon="Sunny" inline-prompt />
    </div>
</template>

<style scoped>
div#header {
    width: 100%;
    height: 100%;
    max-height: 50px;
    padding: 0 10px;
    margin-bottom: 10px;
    border-bottom: var(--el-border-color) solid 1px;
    display: flex;
    justify-content: space-between;
}

div#header>div#flex-grow {
    flex-grow: 100;
}

div#header .el-menu--horizontal {
    border-bottom: none;
    -webkit-user-select: none;
    user-select: none;
}

html div#header :deep(.el-switch) {
    height: 100%;
}

html.dark div#header :deep(.el-switch) {
    --el-switch-on-color: var(--el-bg-color-overlay);
}

@media screen and (max-width: 500px) {

    :deep(#logo-container span#logo),
    html div#header :deep(.el-switch) {
        display: none;
    }
}
</style>
  