<script setup>
import Header from '../Header.vue'
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
            router.push('/connect');
            break;
        case '2-2':
            window.open('https://github.com/Zaitonn/WebConsole', 'blank');
        default:
            router.push('/');
            break;
    }
};

const activeIndex = (() => {
    switch (useRoute().path.split('/')[1]) {
        case 'connect':
            return '1';
        default:
            return '-1';
    }
})();

</script>

<template>
    <Header>
        <el-menu :default-active="activeIndex" class="no-select no-border-bottom " mode="horizontal" :ellipsis="false"
            @select="handleSelect">
            <el-menu-item index="1">连接</el-menu-item>
            <el-sub-menu index="2">
                <template #title>其他</template>
                <el-menu-item index="2-1">文档</el-menu-item>
                <el-menu-item index="2-2">GitHub</el-menu-item>
                <el-menu-item index="2-3">退出</el-menu-item>
            </el-sub-menu>
        </el-menu>
        <el-switch v-model="value" size="small" @change="toggleDark" active-value="true" active-icon="Moon"
            inactive-icon="Sunny" inline-prompt />
    </Header>
    <div class="flex">
        <slot></slot>
    </div>
</template>

<style scoped>
html div#header :deep(.el-switch) {
    height: 100%;
}

html.dark div#header :deep(.el-switch) {
    --el-switch-on-color: var(--el-bg-color-overlay);
}

div.flex {
    justify-content: center;
}
</style>