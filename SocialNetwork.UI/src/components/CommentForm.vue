<template>
    <b-card>
        <b-media>
            <template #aside>
                <b-avatar v-if="avatar" :src="avatar" size="4rem"></b-avatar>
            </template>

            <b-form @submit="onSubmit" ref="form">
                <b-form-group>
                    <b-form-textarea
                        id="textarea"
                        v-model="form.Text"
                        placeholder="Enter something..."
                        rows="3"
                        max-rows="6"
                    ></b-form-textarea>
                </b-form-group>

                <div id="media-submit">
                    <b-form-file
                        v-model="media"
                        placeholder="Add an image..."
                        @change="addImg"
                        multiple
                    ></b-form-file>

                    <b-button v-if="isEditing" variant="secondary" @click="cancelEditing">Cancel</b-button>
                    <b-button type="submit" variant="primary">Submit</b-button>
                </div>

                <div id="preview">
                    <img v-for="url in imgUrls" :key="url" :src="url" @contextmenu="removeImg"/>
                </div>
            </b-form>
        </b-media>
    </b-card>
</template>

<script>
    export default {
        name: 'CommentForm',
        props: ["postId", "myId", "jwtToken", "comment"],
        data() {
            return {
                getAvatarUrl: "https://localhost:6868/Users/userId/profile/avatar",
                avatar: null,

                commentUrl: "https://localhost:6868/Comments/",
                uploadUrl: "https://localhost:6868/Posts/upload",
                unuploadUrl: "https://localhost:6868/Posts/unupload",
                form: {
                    MediaPaths: [],
                },
                media: null,
                imgUrls: [],

                isEditing: false,
            }
        },
        async mounted() {
            await this.getAvatar();
        },
        methods: {
            async getAvatar() {
                await this.$http.get(
                    this.getAvatarUrl.replace("userId", this.myId)
                ).then(res => {
                    this.avatar = require(`@/assets/${res.data}`);
                }).catch(err => {
                    console.log(err.response);
                });
            },

            async onSubmit(event) {
                event.preventDefault();

                if (this.isEditing) {
                    this.$http.patch(
                        this.commentUrl + this.comment.Id,
                        this.form
                    ).then((res) => {
                        console.log(res);
                    }).catch((res) => {
                        console.log(res.response)
                    });
                } else {
                    this.form.AuthorId = this.myId;
                    this.form.PostId = this.postId,

                    await this.$http.post(
                        this.commentUrl,
                        JSON.parse(JSON.stringify(this.form))
                    ).then((res) => {
                        console.log(res);
                        this.$refs.form.reset();
                    }).catch((res) => {
                        console.log(res.response);
                    });
                }
            },

            cancelEditing() {
                this.$emit('cancelEditing');
            },

            addImg(e) {
                const file = e.target.files[0];
                this.imgUrls.push(URL.createObjectURL(file));

                var imgForm = new FormData();
                imgForm.append('image', file);
                this.$http.post(
                    this.uploadUrl,
                    imgForm,
                    {
                        headers: {
                            'Content-Type': 'multipart/form-data'
                        }
                    }
                ).then(res => {
                    console.log(res.data);
                    this.form.MediaPaths.push(res.data);
                }).catch(res => {
                    console.log(res.response);
                })
            },
            removeImg(e) {
                this.imgUrls.splice(this.imgUrls.indexOf(e.target.src), 1);
                e.preventDefault();
            }
        },
        watch: {
            comment: {
                deep: true,
                immediate: true,
                handler: function () {
                    if (this.comment) {
                        this.isEditing = true;
                        this.form = this.comment;
                    }
                }
            }
        },
    }
</script>

<style scoped>
    .card, .card-body {
        background-color: #111;
        border-radius: 10px;
    }

    #textarea {
        background-color: #111;
        border: none;
        color: var(--white);
        overflow: hidden;
    }
    #textarea::-webkit-scrollbar {
        display: none;
    }

    #media-submit {
        display: flex;
        align-items: center;
        justify-content: space-around;
        gap: 20px;
        margin: 5px 0;
    }

    #preview {
        display: flex;
        justify-content: center;
        align-items: center;
        max-width: 500px;
        overflow-x: auto;
    }
    #preview img {
        width: 200px;
        height: 200px;
        object-fit: contain;
    }
</style>