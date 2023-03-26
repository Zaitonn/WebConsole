<script setup>
import Header from '../Header.vue'
</script>

<template>
    <Header></Header>
    <div id="content-flex">
        <slot></slot>
    </div>
</template>