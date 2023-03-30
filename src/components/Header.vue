<script setup>
import Logo from './Logo.vue'
</script>

<template>
    <div id="header">
        <Logo @click="$router.push('/')" />
        <div class="flex-grow"></div>
        <slot></slot>
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
    background: var(--el-bg-color);
}

:deep(#logo-container span#logo) {
    transition: opacity 0.2s, transform 0.3s, width 0.5s;
}

@media screen and (max-width: 500px) {

    :deep(#logo-container span#logo) {
        opacity: 0;
        transform: translateX(-100px);
        width: 0;
    }

    html div#header :deep(.el-switch) {
        display: none;
    }
}
</style>
  