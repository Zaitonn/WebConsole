import { createApp } from 'vue'
import App from '/src/App.vue'
import ElementPlus from 'element-plus'
import * as ElementPlusIconsVue from '@element-plus/icons-vue'

import router from './route'

import 'element-plus/dist/index.css'
import 'element-plus/theme-chalk/dark/css-vars.css'
import '/src/assets/main.css'
import '/src/assets/custom.css'


const app = createApp(App);
app.config.productionTip = false;


for (const [key, component] of Object.entries(ElementPlusIconsVue)) {
    app.component(key, component);
}

app.use(ElementPlus);
app.use(router);
app.mount('#app');
