<template>
    <b-card>
        <b-media>
            <template #aside>
                <b-avatar v-if="avatar" :src="avatar" size="4rem"></b-avatar>
            </template>

            <b-form @submit="onSubmit">
                <b-form-textarea
                    id="textarea"
                    v-model="form.Text"
                    placeholder="Enter something..."
                    rows="3"
                    max-rows="6"
                ></b-form-textarea>

                <div id="media-privacy-submit">
                    <b-form-select v-model="form.Privacy" :options="privacyOptions" required></b-form-select>
                    <b-form-file
                        v-model="media"
                        accept="image/*"
                        placeholder="Add an image..."
                        @change="addMedia"
                        multiple
                    ></b-form-file>

                    <b-button v-if="isEditing" variant="secondary" @click="cancelEditing">Cancel</b-button>
                    <b-button type="submit" variant="primary">Submit</b-button>
                </div>
                
                <div id="preview" v-if="mediaUrls.length > 0">
                    <img v-for="url in mediaUrls" :key="url" :src="url" @contextmenu="removeImg"/>
                    <video controls v-for="url in mediaUrls" :key="url" >
                        <source :src="url" />
                    </video>
                </div>
            </b-form>
        </b-media>
    </b-card>
</template>

<script>
    export default {
        name: 'PostForm',
        props: ["myId", "jwtToken", "post"],
        data() {
            return {
                getAvatarUrl: "https://localhost:6868/Users/userId/profile/avatar",
                avatar: null,

                postUrl: "https://localhost:6868/Posts/",
                uploadUrl: "https://localhost:6868/Posts/upload",
                unuploadUrl: "https://localhost:6868/Posts/unupload",
                form: {
                    MediaPaths: [],
                },
                media: null,
                mediaUrls: [],
                privacyOptions: [
                    { value: 0, text: 'Public' },
                    { value: 1, text: 'Private' }
                ],

                isEditing: false,
            }
        },
        created() {
            this.$http.defaults.headers.common["Authorization"] = this.jwtToken;
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
                        this.postUrl + this.post.Id,
                        this.form
                    ).then((res) => {
                        console.log(res);
                        this.$router.go();
                    }).catch((res) => {
                        console.log(res.response)
                    });
                } else {
                    this.form.AuthorId = this.myId;

                    this.$http.post(
                        this.postUrl,
                        this.form
                    ).then((res) => {
                        console.log(res);
                        this.$router.go();
                    }).catch((res) => {
                        console.log(res.response)
                    });
                }
            },

            cancelEditing() {
                this.$emit('cancelEditing');
            },

            addMedia(e) {
                const file = e.target.files[0];
                this.mediaUrls.push(URL.createObjectURL(file));

                var mediaForm = new FormData();
                mediaForm.append('media', file);
                this.$http.post(
                    this.uploadUrl,
                    mediaForm,
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
                console.log(e.target.src);
                var unloadForm = new FormData();
                unloadForm.append(e.target.src.replace("blob:http://localhost:8080/", ""), "id");
                this.$http.post(
                    this.unuploadUrl,
                    unloadForm,
                    {
                        headers: {
                            'Content-Type': 'multipart/form-data'
                        }
                    }
                ).then(res => {
                    console.log(res);
                    this.mediaUrls.splice(this.mediaUrls.indexOf(e.target.src), 1);
                }).catch(res => {
                    console.log(res.response);
                });
                e.preventDefault();
            }
        },
        watch: {
            post: {
                deep: true,
                immediate: true,
                handler: function () {
                    if (this.post) {
                        this.isEditing = true;
                        this.form = this.post;
                    }
                }
            }
        },
    }
</script>

<style scoped>
    .card, .card-body {
        background-color: #111;
        color: var(--white);
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

    #media-privacy-submit {
        display: flex;
        align-items: center;
        justify-content: space-around;
        gap: 20px;
        margin: 5px 0;
    }
    .custom-select {
        margin-top: 0;
        background-color: #111;
        color: var(--white);
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