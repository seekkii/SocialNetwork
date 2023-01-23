<template>
    <b-container fluid id="follow-list">
        <b-card no-body v-if="followers && followees">
            <b-tabs card active-nav-item-class="text-light bg-dark border-0" justified>
                <b-tab title="Followers" active>
                    <b-list-group v-if="followers.length > 0">
                        <b-list-group-item 
                            class="d-flex align-items-center border border-dark m-0"
                            v-for="follower in followers"
                            :key="follower"
                        >
                            <user-card
                                :jwtToken="jwtToken"
                                :myId="myId"
                                :userId="follower"
                                :size="userCardSize"/>
                        </b-list-group-item>
                    </b-list-group>
                    <div v-else>No followers yet.</div>
                </b-tab>
                <b-tab title="Following">
                    <b-list-group v-if="followees.length > 0">
                        <b-list-group-item 
                            class="d-flex align-items-center border border-dark m-0"
                            v-for="followee in followees"
                            :key="followee"
                        >
                            <user-card
                                :jwtToken="jwtToken"
                                :myId="myId"
                                :userId="followee"
                                :size="userCardSize"/>
                        </b-list-group-item>
                    </b-list-group>
                    <div v-else>Not following anyone yet.</div>
                </b-tab>
            </b-tabs>
        </b-card>
    </b-container>
</template>

<script>
    export default {
        name: 'FollowList',
        props: ["myId", "userId", "jwtToken"],
        data() {
            return {
                getFollowersUrl: "https://localhost:6868/Users/userId/followers",
                getFolloweesUrl: "https://localhost:6868/Users/userId/followees",

                followers: null,
                followees: null,

                userCardSize: 'S',
            };
        },
        async mounted() {
            await this.getFollowers();
            await this.getFollowees();
        },
        methods: {
            async getFollowers() {
                await this.$http.get(
                    this.getFollowersUrl.replace("userId", this.userId)
                ).then(res => {
                    this.followers = res.data;
                }).catch(res => {
                    console.log(res.response);
                });
            },

            async getFollowees() {
                await this.$http.get(
                    this.getFolloweesUrl.replace("userId", this.userId)
                ).then(res => {
                    this.followees = res.data;
                }).catch(res => {
                    console.log(res.response);
                });
            },
        },
    }
</script>

<style scoped>
    #follow-list {
        padding: 0 50px;
    }

    .tab-pane {
        padding: 10px;
    }

    .list-group {
        gap: 10px;
    }
    .card,
    .list-group-item {
        background-color: #111 !important;
        color: var(--white) !important;
        padding: 0;
        width: 100%;
    }
    .list-group .card-body {
        height: 30px;
    }
</style>