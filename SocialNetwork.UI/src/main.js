import Vue from 'vue';
import App from './App.vue';

Vue.config.productionTip = true;

/** Event Bus **/
Vue.prototype.$bus = new Vue();
/** End Event Bus **/

/** Axios **/
import axios from 'axios';
Vue.prototype.$http = axios;
/** End Axios **/

/** Post Socket **/
import PostSocket from './plugins/hubs/postHub';
Vue.use(PostSocket);
/** End Post Socket **/

/** Comment Socket **/
import CommentSocket from './plugins/hubs/commentHub';
Vue.use(CommentSocket);
/** End Comment Socket **/

/** Chat Socket **/
import ChatSocket from './plugins/hubs/chatHub';
Vue.use(ChatSocket);
/** End Comment Socket **/

/** Notification Socket **/
import NotificationSocket from './plugins/hubs/notificationHub';
Vue.use(NotificationSocket);
/** End Notification Socket **/

/** Vue router **/
import router from './plugins/router';
import VueRouter from 'vue-router';
Vue.use(VueRouter);
/** End Vue router **/

/** Vue cookies **/
import VueCookies from 'vue-cookies';
Vue.use(VueCookies);
/** End Vue cookies **/

import { BootstrapVue, BootstrapVueIcons } from 'bootstrap-vue';
import 'bootstrap/dist/css/bootstrap.css';
import 'bootstrap-vue/dist/bootstrap-vue.css';
Vue.use(BootstrapVue);
Vue.use(BootstrapVueIcons);

Vue.mixin({
    methods: {
        /**
             * Display how much time from when the comment is posted till now
             * Still ugly coded
             * @param time somment's timestamp
             */
        calcTimeTillNow(time) {
            time = Date.now() - Date.parse(time);
            var years = Math.floor(time / 31556952000);
            if (years > 0) {
                return years + " years ago";
            } else {
                var months = Math.floor(time / 2629746000);
                if (months > 0) {
                    return months + " months ago";
                } else {
                    var weeks = Math.floor(time / 604800000);
                    if (weeks > 0) {
                        return weeks + " weeks ago";
                    } else {
                        var days = Math.floor(time / 86400000);
                        if (days > 0) {
                            return days + " days ago";
                        } else {
                            var hours = Math.floor(time / 3600000);
                            if (hours > 0) {
                                return hours + " hours ago";
                            } else {
                                var minutes = Math.floor(time / 60000);
                                if (minutes > 0) {
                                    return minutes + " minutes ago";
                                } else {
                                    var seconds = Math.floor(time / 1000);
                                    return seconds + " seconds ago";
                                }
                            }
                        }
                    }
                }
            }
        },

        /**
             * Convert comment's timestamp to easily readable time
             * @param time comment's timestamp
             */
        toReadableTime(time) {
            time = new Date(time);
            return (time.getDate() < 10 ? "0" + time.getDate() : time.getDate()) + "/" +
                ((time.getMonth() + 1) < 10 ? "0" + (time.getMonth() + 1) : (time.getMonth() + 1)) + "/" +
                time.getFullYear() + " " +
                (time.getHours() < 10 ? "0" + time.getHours() : time.getHours()) + ":" +
                (time.getMinutes() < 10 ? "0" + time.getMinutes() : time.getMinutes()) + ":" +
                (time.getSeconds() < 10 ? "0" + time.getSeconds() : time.getSeconds());
        },
    },
});

/** Global components **/
import Feed from "@/layouts/Feed.vue";
Vue.component("feed", Feed);

import NotificationAlert from "@/components/NotificationAlert.vue";
import NotificationCard from "@/components/NotificationCard.vue";
Vue.component("notification-alert", NotificationAlert);
Vue.component("notification-card", NotificationCard);

import PostForm from "@/components/PostForm.vue";
import PostCard from "@/components/PostCard.vue";
Vue.component("post-form", PostForm);
Vue.component("post-card", PostCard);

import CommentForm from "@/components/CommentForm.vue";
import CommentCard from "@/components/CommentCard.vue";
Vue.component("comment-form", CommentForm);
Vue.component("comment-card", CommentCard);

import UserCard from "@/components/UserCard.vue";
import PersonalInformation from "@/layouts/PersonalInformation.vue";
import FollowList from "@/layouts/FollowList.vue";
Vue.component("user-card", UserCard);
Vue.component("personal-information", PersonalInformation);
Vue.component("follow-list", FollowList);

import ChatMessage from "@/components/ChatMessage.vue";
import ChatHistory from "@/components/ChatHistory.vue";
import ChatForm from "@/components/ChatForm.vue";
import Chat from "@/components/Chat.vue";
Vue.component("chat-message", ChatMessage);
Vue.component("chat-history", ChatHistory);
Vue.component("chat-form", ChatForm);
Vue.component("chat", Chat);
/** End global components **/

new Vue({
    router,
    render: h => h(App)
}).$mount('#app');
