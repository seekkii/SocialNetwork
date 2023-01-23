<template>
    <!--TODO: Add shared by wrapped by b-card-->
    <b-card v-if="!isEditing" @click="postCardOnClick">
        <b-media>
            <template #aside>
                <b-avatar 
                    v-if="avatar"
                    :src="avatar" 
                    size="4rem"
                    button
                    @click="openUserProfile"
                    :id="postId" 
                ></b-avatar>
            </template>

            <b-popover 
                :target="postId" 
                triggers="hover" 
                placement="top"
                custom-class="wide-popover"
            >
                <user-card
                    :jwtToken="jwtToken"
                    :myId="myId"
                    :userId="post.AuthorId"
                    :size="userCardSize"/>
            </b-popover>

            <b-container class="d-inline-flex author-date-opts" fluid>
                <div class="author-date">
                    <b class="mt-0 author"><a @click="openUserProfile">{{ name }}</a></b>
                    <div>・</div>
                    <div class="date" :title="toReadableTime(post.Timestamp)">{{ calcTimeTillNow(post.Timestamp) }}</div>
                </div>

                <div class="opts">
                    <b-dropdown size="md" variant="link" toggle-class="text-decoration-none" no-caret>
                        <template #button-content><b-icon icon="three-dots"></b-icon></template>

                        <div v-if="myId===post.AuthorId">
                            <b-dropdown-item @click="onEditClick"><b-icon icon="pen-fill"></b-icon>&nbsp;Edit</b-dropdown-item>
                            <b-dropdown-item @click="onDeleteClick"><b-icon icon="trash-fill"></b-icon>&nbsp;Delete</b-dropdown-item>
                        </div>
                        <div v-else>
                            <b-dropdown-item @click="onSaveClick"><b-icon icon="bookmark-heart-fill"></b-icon>&nbsp;Save for later</b-dropdown-item>
                        </div>
                    </b-dropdown>
                </div>
            </b-container>
            
            <p class="mb-0 text">{{ post.Text }}</p>

            <b-carousel
                id="carousel"
                controls
                indicators
                img-width="370"
                img-height="200"
                v-if="postMedia.length > 0"
            >
                <!--<b-carousel-slide 
                    v-for="media in postMedia"
                    :key="media"
                    :img-src="media">
                </b-carousel-slide>-->
                <b-embed
                    v-for="media in postMedia"
                    type="iframe"
                    aspect="16by9"
                    :key="media"
                    :src="media"
                    allowfullscreen
                ></b-embed>
            </b-carousel>
        </b-media>

        <b-button-group class="d-flex like-cmt-share">
            <div class="share">
                <b-button variant="outline"><b-icon icon="share" variant="light"></b-icon></b-button>
                <div class="count">{{shares}}</div>
            </div>
            <div class="cmt">
                <b-button variant="outline"><b-icon icon="chat" variant="light"></b-icon></b-button>
                <div class="count">{{comments}}</div>
            </div>
            <div class="like">
                <b-button variant="outline" v-if="iLiked" @click="unlikePost"><b-icon icon="heart-fill" variant="danger"></b-icon></b-button>
                <b-button variant="outline" v-else @click="likePost"><b-icon icon="heart" variant="light"></b-icon></b-button>
                <div class="count" :class="{'liked': iLiked}">{{likes}}</div>
            </div>
        </b-button-group>
    </b-card>

    <post-form
        v-else
        :post="post"
        :myId="myId"
        :jwtToken="jwtToken"
        @cancelEditing="cancelEditing"/>
</template>

<script>
    export default {
        name: 'PostCard',
        props: ["postId", "jwtToken", "myId"],
        data() {
            return {
                getAvatarNameUrl: "https://localhost:6868/Users/userId/profile/avatarname",
                avatar: null,
                name: null,

                getPostUrl: "https://localhost:6868/Posts/postId",
                getMediaUrl: "https://localhost:6868/Posts/postId/media",
                post: {},
                postMedia: [],

                getLikesUrl: "https://localhost:6868/Posts/postId/likes",
                checkLikedUrl: "https://localhost:6868/Posts/postId/likes/userId",
                likePostUrl: "https://localhost:6868/Posts/postId/likes/userId/like",
                unlikePostUrl: "https://localhost:6868/Posts/postId/likes/userId/unlike",

                savePostUrl: "https://localhost:6868/Posts/postId/save/userId",

                iLiked: false,
                whoLiked: [],
                likes: 0,

                getCommentCountUrl: "https://localhost:6868/Posts/postId/comments/count",
                comments: 0,
                shares: 0,

                isEditing: false,
                userCardSize: 'M',
            }
        },
        async created() {
            /**
             * Hub listener
             * whenever an event named "post-react" triggered
             * run the method named "postReacted"
             */
            await this.$postHub.$on('post-react', this.postReacted);
        },
        beforeDestroy() {
            this.$postHub.$off('post-react', this.postReacted);
        },
        async mounted() {
            /**
             * What to do when component is mounted:
             * 1. get the post to display (text, time, id, ...)
             * 2. get post's media
             * 
             * 3. get author to display (name, avatar, ...)
             *
             * 4. check if viewing user has liked the post ?
             * 5. list all who liked the post
             *
             * 6. get how many comments there are
             */
            await this.getPost();
            await this.getMedia();

            await this.getAvatarName();

            await this.haveILiked();
            await this.getWhoLiked();

            await this.getCommentCount();
        },
        methods: {
            async getAvatarName() {
                await this.$http.get(
                    this.getAvatarNameUrl.replace("userId", this.post.AuthorId)
                ).then(res => {
                    this.avatar = require(`@/assets/${res.data.Avatar}`);
                    this.name = res.data.Name;
                }).catch(err => {
                    console.log(err.response);
                });
            },

            /**
             * get the post to display (text, time, id, ...)
             * */
            async getPost() {
                await this.$http.get(
                    this.getPostUrl.replace("postId", this.postId)
                ).then((res) => {
                    this.post = res.data;
                }).catch((res) => {
                    console.log(res);
                });
            },

            /**
             * get post's media
             * */
            async getMedia() {
                await this.$http.get(
                    this.getMediaUrl.replace("postId", this.postId)
                ).then(res => {
                    res.data.forEach((val, _) => {
                        var media = require(`@/assets/${val}`);
                        this.postMedia.push(media);
                    });
                }).catch(res => {
                    console.log(res);
                });
            },

            openUserProfile(e) {
                this.$router.push({
                    name: 'user',
                    params: {
                        userId: this.post.AuthorId,
                        myId: this.myId,
                        jwtToken: this.jwtToken
                    }
                }).catch(err => {
                    this.$router.go();
                });
                e.stopPropagation();
            },

            /**
             * list all who liked the post
             * */
            async getWhoLiked() {
                await this.$http.get(
                    this.getLikesUrl.replace("postId", this.postId)
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
                    this.checkLikedUrl.replace("postId", this.postId).replace("userId", this.myId)
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
            async likePost(e) {
                e.stopPropagation();
                await this.$http.post(
                    this.likePostUrl.replace("postId", this.postId).replace("userId", this.myId),
                    null
                ).then((res) => {
                    console.log(res.data);
                }).catch((res) => {
                    console.log(res.response);
                });
            },

            /**
             * Run when user click unlike
             * @param e unlike event
             */
            async unlikePost(e) {
                e.stopPropagation();
                await this.$http.post(
                    this.unlikePostUrl.replace("postId", this.postId).replace("userId", this.myId),
                    null
                ).then((res) => {
                    console.log(res.data);
                }).catch((res) => {
                    console.log(res.response);
                });
            },

            /**
             * Hub listener
             * whenever an event named "post-react" triggered
             * run this
             * @param params equivalent to parameters {postId, userId, like} sent from Backend in Class PostHub, Task Reaction()
             */
            async postReacted(params) {
                if (this.postId !== params.postId) return;
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
            async getCommentCount() {
                await this.$http.get(
                    this.getCommentCountUrl.replace("postId", this.postId)
                ).then(res => {
                    this.comments = res.data;
                }).catch(res => {
                    console.log(res.response);
                });
            },  

            /**
             * Redirect to PostLayout single page -> Full view of the post
             * */
            postCardOnClick() {
                if (this.$route.path !== `/post/${this.postId}`) {
                    this.$router.push({
                        name: 'post',
                        params: {
                            postId: this.postId,
                            myId: this.myId,
                            jwtToken: this.jwtToken
                        }
                    })
                }
            },

            onEditClick(e) {
                this.isEditing = true;
                e.stopPropagation();
            },

            cancelEditing() {
                this.isEditing = false;
            },

            onDeleteClick(e) {
                e.stopPropagation();
                this.$http.delete(
                    this.getPostUrl.replace("postId", this.postId)
                ).then((res) => {
                    console.log(res);
                    this.$router.go();
                }).catch((res) => {
                    console.log(res.response)
                });
            },

            onSaveClick(e) {
                e.stopPropagation();
                this.$http.post(
                    this.savePostUrl.replace("postId", this.postId).replace("userId", this.myId),
                    null
                ).then((res) => {
                    console.log(res);
                }).catch((res) => {
                    console.log(res.response)
                });
            },
        },
        watch: {
            post: {
                immediate: true,
                deep: true,
                handler: function () {
                    //console.log(this.post);
                }
            },
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

    .author {
        cursor: pointer;
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
        color: var(--red);
    }

    .carousel {
        margin-top: 10px;
    }
    .carousel-inner img {
        width: 100% !important;
        max-height: 200px !important;
        min-height: 200px !important;
        background-size: cover;
    }

    .dropdown-toggle {
        padding: 0;
    }

    .wide-popover {
        min-width: max-content;
        background-color: #111;
        box-shadow: 0 0 10px #9ecaed;
    }
</style>