<template>
    <b-alert
        v-if="notification"
        :show="dismissNotification"
        dismissible
        class="position-fixed fixed-bottom"
    >
        <notification-card :notification="notification" :jwtToken="jwtToken"/>
    </b-alert>
</template>

<script>
    export default {
        name: "NotificationAlert",
        props: ["notification", "jwtToken", "dismissNotification"],
    }
</script>

<style scoped>
    .alert {
        color: var(--white);
        background-color: #111;
        border: 1px solid var(--white);
        max-width: 30%;
        margin-left: 20px;
    }
</style>