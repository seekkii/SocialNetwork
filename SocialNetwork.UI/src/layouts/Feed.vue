<template>
    <div id="feed">
        <post-form v-if="userId===myId" :myId="myId" :jwtToken="jwtToken"/>
        <post-card 
            :myId="myId"
            :jwtToken="jwtToken"
            v-for="postId in postIds"
            :key="postId"
            :postId="postId"/>
    </div>
</template>

<script>
    export default {
        name: 'Feed',
        props: ["userId", "jwtToken", "feedType", "myId"],
        data() {
            return {
                getFeedUrl: "https://localhost:6868/Users/userId/feedType",
                postIds: [],
            }
        },
        async created() {
            this.$http.defaults.headers.common["Authorization"] = this.jwtToken;
        },
        async mounted() {
            //console.log("Into Feed...");
            //console.log(this.userId);
            if (this.userId) {
                //console.log("Found userId: ", this.userId);
                await this.getFeed();
            }
        },
        methods: {
            async getFeed() {
                await this.$http.get(
                    this.getFeedUrl.replace("userId", this.userId).replace("feedType", this.feedType)
                ).then((res) => {
                    this.postIds = res.data;
                    //console.log(this.postIds);
                }).catch((res) => {
                    console.log(res);
                });
            }
        }
    }
</script>

<style scoped>
    #feed {
        display: flex;
        flex-direction: column;
        gap: 20px;
    }    
</style>