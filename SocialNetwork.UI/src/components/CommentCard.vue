<template>
    <b-card v-if="!isEditing">
        <b-media>
            <template #aside>
                <b-avatar 
                    v-if="avatar"
                    :src="avatar" 
                    size="4rem"
                    button
                    @click="openUserProfile"
                    :id="commentId" 
                ></b-avatar>
            </template>

            <b-popover 
                :target="commentId" 
                triggers="hover" 
                placement="top"
                custom-class="wide-popover"
            >
                <user-card
                    :jwtToken="jwtToken"
                    :myId="myId"
                    :userId="comment.AuthorId"
                    :size="userCardSize"/>
            </b-popover>

            <b-container class="d-inline-flex author-date-opts" fluid>
                <div class="author-date">
                    <b class="mt-0 author"><a :href="userProfileUrl" @click="$(this).stopPropagation();">{{ name }}</a></b>
                    <div>・</div>
                    <div class="date" :title="toReadableTime(comment.Timestamp)">{{ calcTimeTillNow(comment.Timestamp) }}</div>
                </div>
                <div class="opts" v-if="myId===comment.AuthorId">
                    <b-dropdown size="md" variant="link" toggle-class="text-decoration-none" no-caret>
                        <template #button-content><b-icon icon="three-dots"></b-icon></template>
                        <b-dropdown-item @click="onEditClick"><b-icon icon="pen-fill"></b-icon>&nbsp;Edit</b-dropdown-item>
                        <b-dropdown-item @click="onDeleteClick"><b-icon icon="trash-fill"></b-icon>&nbsp;Delete</b-dropdown-item>
                    </b-dropdown>
                </div>
            </b-container>
            
            <p class="mb-0 text">
              {{ comment.Text }}
            </p>

            <b-carousel
                id="carousel-comment"
                :interval="4000"
                controls
                indicators
                img-width="1024"
                img-height="480"
                v-if="commentMedia.length > 0"
            >
                <b-carousel-slide 
                    v-for="media in commentMedia"
                    :key="media"
                    :img-src="media">
                </b-carousel-slide>
            </b-carousel>
        </b-media>

        <b-button-group class="d-flex like-cmt-share">
            <div class="cmt">
                <b-button variant="outline"><b-icon icon="chat" variant="light"></b-icon></b-button>
                <div class="count">{{comments}}</div>
            </div>
            <div class="like">
                <b-button variant="outline" v-if="iLiked" @click="unlikeComment"><b-icon icon="heart-fill" variant="danger"></b-icon></b-button>
                <b-button variant="outline" v-else @click="likeComment"><b-icon icon="heart" variant="light"></b-icon></b-button>
                <div class="count" :class="{'liked': iLiked}">{{likes}}</div>
            </div>
        </b-button-group>
    </b-card>

    <comment-form 
        v-else
        :comment="comment"
        :myId="myId"
        :jwtToken="jwtToken"/>
</template>

<script>
    import _ from 'lodash';
    export default {
        name: 'CommentCard',
        props: ["commentId", "myId", "jwtToken"],
        data() {
            return {
                getAvatarNameUrl: "https://localhost:6868/Users/userId/profile/avatarname",
                avatar: null,
                name: null,

                getCommentUrl: "https://localhost:6868/Comments/commentId",
                getMediaUrl: "https://localhost:6868/Comments/commentId/media",
                comment: {},
                commentMedia: [],

                getLikesUrl: "https://localhost:6868/Comments/commentId/likes",
                checkLikedUrl: "https://localhost:6868/Comments/commentId/likes/userId",
                likeCommentUrl: "https://localhost:6868/Comments/commentId/likes/userId/like",
                unlikeCommentUrl: "https://localhost:6868/Comments/commentId/likes/userId/unlike",

                iLiked: false,
                whoLiked: [],
                likes: 0,

                getChildCountUrl: "https://localhost:6868/Comments/commentId/comments/count",
                comments: 0,
                shares: 0,

                isEditing: false,
                userCardSize: 'M',
            }
        },
        async created() {
            /**
             * Hub listener
             * whenever an event named "comment-react" triggered
             * run the method named "commentReacted"
             */
            await this.$commentHub.$on('comment-react', this.commentReacted);

            await this.$postHub.$on('comment-edited', this.commentEdited);
        },
        beforeDestroy() {
            this.$commentHub.$off('comment-react', this.commentReacted);
            this.$postHub.$off('comment-edited', this.commentEdited);
        },
        async mounted() {
            /**
             * What to do when component is mounted:
             * 1. get the comment to display (text, time, id, ...)
             * 2. get comment's media
             * 3. get author to display (name, avatar, ...)
             * 
             * 4. check if viewing user has liked the comment ?
             * 5. list all who liked the comment
             * 
             * 6. get how many children comment there are
             */
            await this.getComment();
            await this.getMedia();

            await this.getAvatarName();

            await this.haveILiked();
            await this.getWhoLiked();

            await this.getChildCount();
        },
        methods: {
            async getAvatarName() {
                await this.$http.get(
                    this.getAvatarNameUrl.replace("userId", this.comment.AuthorId)
                ).then(res => {
                    this.avatar = require(`@/assets/${res.data.Avatar}`);
                    this.name = res.data.Name
                }).catch(err => {
                    console.log(err.response);
                });
            },

            /**
             * get the comment to display (text, time, id, ...)
             * */
            async getComment() {
                await this.$http.get(
                    this.getCommentUrl.replace("commentId", this.commentId)
                ).then((res) => {
                    this.comment = res.data;
                    //console.log(this.comment);
                }).catch((res) => {
                    console.log(res);
                });
            },

            /**
             * get post's media
             * */
            async getMedia() {
                await this.$http.get(
                    this.getMediaUrl.replace("commentId", this.commentId)
                ).then(res => {
                    res.data.forEach((val, idx) => {
                        //console.log(idx);
                        var media = require(`@/assets/${val}`);
                        this.commentMedia.push(media);
                    });
                }).catch(res => {
                    console.log(res.response);
                });
            },

            openUserProfile(e) {
                this.$router.push({
                    name: 'user',
                    params: {
                        userId: this.comment.AuthorId,
                        myId: this.myId,
                        jwtToken: this.jwtToken
                    }
                }).catch(err => {
                    this.$router.go();
                });
                e.stopPropagation();
            },

            /**
             * list all who liked the comment
             * */
            async getWhoLiked() {
                await this.$http.get(
                    this.getLikesUrl.replace("commentId", this.commentId)
                ).then((res) => {
                    this.likes = res.data.length;
                }).catch((res) => {
                    console.log(res.response);
                });
            },

            /**
             * check if viewing user has liked the comment ?
             * */
            async haveILiked() {
                await this.$http.get(
                    this.checkLikedUrl.replace("commentId", this.commentId).replace("userId", this.myId)
                ).then((res) => {
                    this.iLiked = res.data;
                    //console.log(this.liked);
                }).catch((res) => {
                    console.log(res.response);
                });
            },

            /**
             * Run when user click like
             * @param e like event
             */
            async likeComment(e) {
                //console.log("Like...");
                await this.$http.post(
                    this.likeCommentUrl.replace("commentId", this.commentId).replace("userId", this.myId),
                    null
                ).then((res) => {
                    console.log(res);
                }).catch((res) => {
                    console.log(res.response);
                });
                e.stopPropagation();
            },

            /**
             * Run when user click unlike
             * @param e unlike event
             */
            async unlikeComment(e) {
                await this.$http.post(
                    this.unlikeCommentUrl.replace("commentId", this.commentId).replace("userId", this.myId),
                    null
                ).then((res) => {
                    console.log(res);
                }).catch((res) => {
                    console.log(res.response);
                });
                e.stopPropagation();
            },

            /**
             * Hub listener
             * whenever an event named "comment-react" triggered
             * run this
             * @param params equivalent to parameter named "comment" sent from Backend in Class CommentHub, Task Reaction()
             */
            async commentReacted(params) {
                //console.log(params);
                if (this.commentId !== params.commentId) return;
                if (params.like) {
                    this.likes += 1;
                    if (this.myId === params.userId) {
                        this.iLiked = true;
                    }
                } else {
                    this.likes -= 1;
                    if (this.myId === params.userId) {
                        this.iLiked = false;
                    }
                }
            },

            /**
             * Count how many replies this comment has
             * */
            async getChildCount() {
                await this.$http.get(
                    this.getChildCountUrl.replace("commentId", this.commentId)
                ).then(res => {
                    this.comments = res.data;
                }).catch(res => {
                    console.log(res.response);
                });
            },

            onEditClick(e) {
                this.isEditing = true;
                e.stopPropagation();
            },

            async commentEdited(cmt) {
                cmt = _.mapKeys(cmt, function (v, k) {
                    return _.upperFirst(k);
                });

                if (this.comment.Id === cmt.Id) {
                    this.isEditing = false;
                    this.comment = cmt;
                }
            },

            onDeleteClick(e) {
                e.stopPropagation();
                this.$http.delete(
                    this.getCommentUrl.replace("commentId", this.commentId)
                ).then((res) => {
                    console.log(res);
                }).catch((res) => {
                    console.log(res.response);
                });
            }
        },
        watch: {
            //comment: {
            //    immediate: true,
            //    deep: true,
            //    handler: function () {
            //        console.log(this.comment);
            //    }
            //},
        },
        computed: {
            userProfileUrl() {
                return `http://localhost:8080/user/${this.comment.AuthorId}/profile`;
            }
        }
    }
</script>

<style scoped>
    .card, .card-body {
        background-color: #111;
        border-radius: 10px;
    }

    .author-date-opts {
        display: flex;
        justify-content: space-between;
        padding: 0;
    }

    .author-date {
        display: flex;
        gap: 10px;
        align-items: center;
    }

    .like-cmt-share {
        margin-top: 8px;
        justify-content: space-around;
    }

    .like-cmt-share .like,
    .like-cmt-share .cmt,
    .like-cmt-share .share {
        display: inline-flex;
    }

    .like-cmt-share .btn {
        padding-right: 6px;
    }

    .like-cmt-share .count {
        line-height: 36px;
    }

    .count.liked {
        color: #dc3545;
    }

    .carousel {
        margin-top: 10px;
    }

    .wide-popover {
        min-width: max-content;
        background-color: #111;
        box-shadow: 0 0 10px #9ecaed;
    }
</style>