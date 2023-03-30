import { createWebHistory, createRouter } from 'vue-router'

import Intro from '/src/views/Intro.vue'
import Overview from '/src/views/Overview.vue'
import Connect from '/src/views/Connect.vue'
import NotFound from '/src/views/NotFound.vue'


const router = createRouter({
    history: createWebHistory(),
    routes: [
        { path: '/', component: Intro },
        { path: '/intro', component: Intro },
        { path: '/overview', component: Overview },
        { path: '/connect', component: Connect },
        { path: '/:notFoundPath(.*)*', component: NotFound },
    ]
});

export default router;
