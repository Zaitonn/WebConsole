import { createApp } from 'vue'
import { createWebHistory, createRouter } from 'vue-router'
import App from './App.vue'
import ElementPlus from 'element-plus'
import * as ElementPlusIconsVue from '@element-plus/icons-vue'

import 'element-plus/dist/index.css'
import 'element-plus/theme-chalk/dark/css-vars.css'
import './assets/main.css'
import './assets/custom.css'

import Intro from './views/Intro.vue'
import Overview from './views/service/Overview.vue'
import Login from './views/Login.vue'
import NotFound from './views/NotFound.vue'

const app = createApp(App);
app.config.productionTip = false;

const routes = [
    { path: '/', component: Intro },
    { path: '/intro', component: Intro },
    { path: '/overview', component: Overview },
    { path: '/login', component: Login },
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
