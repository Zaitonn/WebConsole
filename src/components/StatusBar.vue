<template>
    <div id="status-container" :class="`status-${status || 'unknown'}`">
        <el-tooltip :content="{
            opened: '已连接',
            closed: '已断开'
        }[status] || '未知'">
            <i></i>
        </el-tooltip>
        <span id="addr">
            {{ address || 'unknown' }}
        </span>
    </div>
</template>

<script setup>
import { defineProps } from 'vue'

defineProps({
    address: String,
    status: String
});
</script>

<style scoped>
#status-container i {
    width: 8px;
    height: 8px;
    display: inline-block;
    border-radius: 50%;
    background: var(--current-status);
}

#addr {
    font-size: 14px;
}

#status-container>* {
    margin: 3px;
}

#status-container {
    height: 100%;
    display: flex;
    align-items: center;
    --connect-status-opened: #2fe998;
    --connect-status-unknown: #a3a3a3;
    --connect-status-closed: #b82525;
    --current-status: var(--connect-status-unknown);
}

div#status-container.status-unknown {
    --current-status: var(--connect-status-unknown) !important;
}

div#status-container.status-opened {
    --current-status: var(--connect-status-opened) !important;
}

div#status-container.status-closed {
    --current-status: var(--connect-status-closed) !important;
}
</style>