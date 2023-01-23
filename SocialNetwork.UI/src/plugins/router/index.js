import Vue from 'vue'
import VueRouter from 'vue-router';
Vue.use(VueRouter);

import Login from '@/pages/Login.vue';
import Register from '@/pages/Register.vue';
import Home from '@/pages/Home.vue';
import UserProfile from '@/pages/UserProfile.vue';
import UpdateProfile from '@/pages/UpdateProfile.vue';
import PostLayout from '@/layouts/PostLayout.vue';

const routes = [
    {
        path: '/login',
        name: 'login',
        component: Login,
    },
    {
        path: '/register',
        name: 'register',
        component: Register,
    },
    {
        path: '/home',
        name: 'home',
        component: Home,
    },
    {
        path: '/update-profile',
        name: 'updateprofile',
        component: UpdateProfile,
    },
    {
        path: '/user/:userId/profile',
        name: 'user',
        component: UserProfile
    },
    {
        path: '/post/:postId',
        name: 'post',
        component: PostLayout,
    }
]
const router = new VueRouter({
    mode: 'history',
    routes
});
export default router