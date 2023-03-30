<template>
    <div id="logo-container" class="no-select">
        <img alt="logo" id="icon" src="../assets/favicon.png">
        <span id="logo">WebConsole</span>
    </div>
</template>

<style scoped>
#logo-container {
    display: flex;
    align-items: center;
    height: 100%;
}

#icon {
    image-rendering: pixelated;
    width: 22px;
    display: inline-block;
}

#logo {
    font-weight: bolder;
}
</style>