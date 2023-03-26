import { createApp } from 'vue'
import { createWebHistory, createRouter } from 'vue-router'
import App from '/src/App.vue'
import ElementPlus from 'element-plus'
import * as ElementPlusIconsVue from '@element-plus/icons-vue'

import 'element-plus/dist/index.css'
import 'element-plus/theme-chalk/dark/css-vars.css'
import '/src/assets/main.css'
import '/src/assets/custom.css'

import Intro from '/src/views/Intro.vue'
import Overview from '/src/views/service/Overview.vue'
import Connect from '/src/views/service/Connect.vue'
import NotFound from '/src/views/NotFound.vue'

const app = createApp(App);
app.config.productionTip = false;

const routes = [
    { path: '/', component: Intro },
    { path: '/intro', component: Intro },
    { path: '/overview', component: Overview },
    { path: '/connect', component: Connect },
    { path: '/:notFoundPath(.*)*', component: NotFound },
];

const router = createRouter({
    history: createWebHistory(),
    routes
});

for (const [key, component] of Object.entries(ElementPlusIconsVue)) {
    app.component(key, component);
}

app.use(ElementPlus);
app.use(router);
app.mount('#app');
