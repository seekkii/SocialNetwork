<template>
    <div>
        <h1>Post</h1>
        <post-card :myId="myId" :jwtToken="jwtToken" :postId="postId"/>
        
        <b-container fluid id="comments">
            <h2>Comments</h2>
            <comment-form 
                         :myId="myId" 
                         :jwtToken="jwtToken" 
                         :postId="postId"/>
            <comment-card 
                         :myId="myId" :jwtToken="jwtToken"
                         v-for="commentId in commentIds"
                         :key="commentId"
                         :commentId="commentId"/>
        </b-container>
    </div>
</template>

<script>
    export default {
        name: 'PostLayout',
        data() {
            return {
                myId: '',
                jwtToken: '',
                postId: '',

                getCommentsForPostUrl: "https://localhost:6868/Posts/postId/comments",
                commentIds: [],
            }
        },
        async created() {
            this.myId = this.$route.params.myId;
            if (this.myId === undefined) {
                this.myId = this.$cookies.get('sn-user-id');
            }
            this.jwtToken = this.$route.params.jwtToken;
            if (this.jwtToken === undefined) {
                this.jwtToken = this.$cookies.get('sn-auth-token');
            }
            this.$http.defaults.headers.common["Authorization"] = this.jwtToken;
            this.postId = this.$route.params.postId;

            await this.$bus.$emit("getCreds", { jwtToken: this.jwtToken, myId: this.myId });

            /**
             * Hub listener
             * whenever an event named "post-commented-on" triggered
             * run the method named "postCommentedOn"
             */
            await this.$postHub.postOpened(this.postId);
            await this.$postHub.$on('post-commented-on', this.postCommentedOn);
            await this.$postHub.$on('comment-deleted', this.commentDeleted);
        },
        async mounted() {
            /**
             * Get how many comments there are
             */
            await this.getComments();
        },
        async beforeDestroy() {
            /**
             * Destroy hub when user is not viewing the post anymore
             * */
            await this.$postHub.postClosed(this.postId);
            await this.$postHub.$off('post-commented-on', this.postCommentedOn);
            await this.$postHub.$off('comment-deleted', this.commentDeleted);
        },
        methods: {
            /**
             * get how many comments there are
             * */
            async getComments() {
                this.$http.get(
                    this.getCommentsForPostUrl.replace("postId", this.postId)
                ).then(res => {
                    this.commentIds = res.data;
                    //console.log(this.commentIds);
                }).catch(res => {
                    console.log(res.response);
                })
            },

            /**
             * Hub listener
             * whenever an event named "post-commented-on" triggered
             * run this
             * @param comment equivalent to parameter named "comment" sent from Backend in Class PostHub, Task Comment()
             */
            async postCommentedOn(comment) {
                console.log(comment);
                this.commentIds.unshift(comment.id);
            },

            async commentDeleted(commentId) {
                console.log(commentId)
                this.commentIds.splice(this.commentIds.indexOf(commentId), 1);
                console.log(this.commentIds)
            },
        },
    }
</script>

<style scoped>
    #comments {
        display: flex;
        flex-direction: column;
        gap: 20px;
    }
</style>