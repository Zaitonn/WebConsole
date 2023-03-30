<script setup>
import Header from '../Header.vue'
import StatusBar from '../StatusBar.vue'
import { defineProps } from 'vue'

const address = '127.0.0.1';
</script>

<template>
    <Header>
        <StatusBar :address="address" status="" />
    </Header>
    <div class="flex detailLayout">
        <slot></slot>
    </div>
</template>