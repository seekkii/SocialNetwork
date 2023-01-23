<template>
    <b-media @click="notificationCardOnClick">
        <template #aside>
            <b-avatar variant="info" src="https://placekitten.com/300/300" size="4rem"></b-avatar>
        </template>
        
        <div class="noti-body">
            <p class="mb-0 text">{{notification.verb}}</p>
            <p class="time">{{calcTimeTillNow(notification.timestamp)}}</p>
        </div>
    </b-media>
</template>

<script>
    export default {
        name: 'NotificationCard',
        props: ["notification", "jwtToken"],
        data() {
            return {
                getProfileUrl: "https://localhost:6868/Users/userId/profile",
                getEntityTypeUrl: "https://localhost:6868/Entity/entityId",
                from: null,
            };
        },
        async mounted() {
            await this.getProfile();
        },
        methods: {
            /**
             * get user to display (name, avatar, ...)
             * */
            async getProfile() {
                await this.$http.get(
                    this.getProfileUrl.replace("userId", this.notification.fromId)
                ).then((res) => {
                    this.from = res.data;
                }).catch((res) => {
                    console.log(res);
                });
            },

            /**
             * redirect to event's object if notification is clicked on
             * */
            async notificationCardOnClick() {
                await this.$http.get(
                    this.getEntityTypeUrl.replace("entityId", this.notification.entityId)
                ).then(res => {
                    this.$router.push(`/${res.data}/${this.notification.entityId}`);
                }).catch(res => {
                    console.log(res.response);
                })
            },
        },
    }
</script>

<style scoped>
    .noti-body .time {
        margin: 20px 0 0 0;
    }
</style>