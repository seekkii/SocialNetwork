<template>
    <b-card
        v-if="size==='L' && profile"
        :img-src="require(`@/assets/${profile.Cover}`)"
        img-alt="Image"
        img-top
        tag="article"
        class="mb-2"
        id="profile-header"
    >
        <b-button v-if="userId===myId" id="change-cover" @click="showAvatarOrCoverChangeModal($event, 'cover')"><b-icon icon="camera"></b-icon></b-button>
        <div id="basic-info">
            <b-avatar
                id="avatar" 
                :src="require(`@/assets/${profile.Avatar}`)" 
                size="15rem"
                button
                @click="showAvatarOrCoverChangeModal($event, 'avatar')">
            </b-avatar>
            <div id="info">
                <div id="name-follow">
                    <h1 id="name">{{profile.Name}}</h1>
                    <div class="actions" v-if="myId !== userId">
                        <b-button variant="outline-danger" v-if="iFollowed" @click="unfollow">
                            <b-icon icon="person-dash"></b-icon></b-button>
                        <b-button variant="outline-success" v-else @click="follow">
                            <b-icon icon="person-plus"></b-icon></b-button>
                        <b-button variant="outline-info" @click="startChat"><b-icon icon="chat-text"></b-icon></b-button>
                    </div>
                </div>
                <div id="about-me"><b-icon icon="info-circle"></b-icon>{{profile.SelfIntroduction}}</div>
                <div id="address"><b-icon icon="geo-alt"></b-icon>{{profile.CurrentLocation}}</div>
                <div id="follow"><b-icon icon="people"></b-icon>
                    <div>{{followerCount}} followers<div>・</div>{{followeeCount}} following</div>
                </div>
                <div id="mutual">Mo foro- sarete imasu</div>
            </div>
        </div>

        <b-modal 
            v-if="userId===myId"
            id="change-avatar-or-cover-modal" 
            :title="`Change ${modalType}`" 
            size="lg"
            @ok="saveAvatarOrCover"
        >
            <b-form-file
                accept="image/*"
                placeholder="Add an image..."
                @change="chooseImage"
            ></b-form-file>

            <div id="preview" v-if="avatarOrCoverUrl">
                <img :src="avatarOrCoverUrl" width="400" height="400"/>
            </div>
        </b-modal>
    </b-card>

    <b-card v-else-if="size==='M' && profile" class="p-0 border-0 user-card-medium">
        <b-avatar size="6rem" class="mr-3 center" :src="require(`@/assets/${profile.Avatar}`)"></b-avatar>
        <div class="medium-card-right">
            <div class="name">
                <span class="mr-auto"><b><a :href="`/user/${profile.UserId}/profile`">{{profile.Name}}</a></b></span>
            </div>
            <div id="follow"><b-icon icon="people"></b-icon>
                <div>{{followerCount}} followers<div>・</div>{{followeeCount}} following</div>
            </div>
            <div class="actions" v-if="myId !== userId">
                <b-button variant="outline-danger" v-if="iFollowed" @click="unfollow">
                    <b-icon icon="person-dash"></b-icon></b-button>
                <b-button variant="outline-success" v-else @click="follow">
                    <b-icon icon="person-plus"></b-icon></b-button>
                <b-button variant="outline-info" @click="startChat"><b-icon icon="chat-text"></b-icon></b-button>
            </div>
        </div>
    </b-card>

    <b-card v-else>
        <div class="user-card-small" v-if="profile">
            <div class="avatar-name">
                <b-avatar class="mr-3" :src="require(`@/assets/${profile.Avatar}`)"></b-avatar>
                <span class="mr-auto"><a :href="`/user/${profile.UserId}/profile`">{{profile.Name}}</a></span>
            </div>
            <div class="actions" v-if="myId !== userId">
                <b-button variant="outline-danger" v-if="iFollowed" @click="unfollow">
                    <b-icon icon="person-dash"></b-icon></b-button>
                <b-button variant="outline-success" v-else @click="follow">
                    <b-icon icon="person-plus"></b-icon></b-button>
                <b-button variant="outline-info" @click="startChat"><b-icon icon="chat-text"></b-icon></b-button>
            </div>
        </div>
    </b-card>
</template>

<script>
    export default {
        name: 'UserCard',
        props: ["jwtToken", "myId", "userId", "size"],
        data() {
            return {
                getProfileUrl: "https://localhost:6868/Users/userId/profile",
                profile: null,
                uploadUrl: "https://localhost:6868/Posts/upload",
                newAvatarOrCover: null,
                avatarOrCoverUrl: null,
                modalType: null,

                followUrl: "https://localhost:6868/Users/fromId/follow/toId",
                getFollowerCountUrl: "https://localhost:6868/Users/userId/followers/count",
                getFolloweeCountUrl: "https://localhost:6868/Users/userId/followees/count",
                iFollowed: false,
                followerCount: null,
                followeeCount: null,

                getChatIdUrl: "https://localhost:6868/Chat/fromId/toId",
            };
        },
        async created() {
            this.$http.defaults.headers.common["Authorization"] = this.jwtToken;  
        },
        async mounted() {
            await this.getProfile();
            await this.haveIFollowed();

            await this.getFollowerCount();
            await this.getFolloweeCount();
        },
        methods: {
            async getProfile() {
                await this.$http.get(
                    this.getProfileUrl.replace("userId", this.userId)
                ).then((res) => {
                    this.profile = res.data;
                    //console.log(this.profile);
                }).catch((res) => {
                    console.log(res.response);
                });
            },

            async haveIFollowed() {
                await this.$http.get(
                    this.followUrl.replace("fromId", this.myId).replace("toId", this.userId)
                ).then(res => {
                    this.iFollowed = res.data;
                }).catch(res => {
                    console.log(res.response);
                });
            },

            async getFollowerCount() {
                await this.$http.get(
                    this.getFollowerCountUrl.replace("userId", this.userId)
                ).then(res => {
                    this.followerCount = res.data;
                }).catch(res => {
                    console.log(res.response);
                });
            },

            async getFolloweeCount() {
                await this.$http.get(
                    this.getFolloweeCountUrl.replace("userId", this.userId)
                ).then(res => {
                    this.followeeCount = res.data;
                }).catch(res => {
                    console.log(res.response);
                });
            },

            async follow() {
                await this.$http.post(
                    this.followUrl.replace("fromId", this.myId).replace("toId", this.userId),
                    null
                ).then(res => {
                    console.log(res);
                    this.iFollowed = true;
                    this.followerCount += 1;
                }).catch(res => {
                    console.log(res.response);
                })
            },

            async unfollow() {
                await this.$http.post(
                    this.followUrl.replace("follow", "unfollow").replace("fromId", this.myId).replace("toId", this.userId),
                    null
                ).then(res => {
                    console.log(res);
                    this.iFollowed = false;
                    this.followerCount -= 1;
                }).catch(res => {
                    console.log(res.response);
                })
            },

            async startChat() {
                let chatId = null;
                await this.$http.get(
                    this.getChatIdUrl.replace("fromId", this.myId).replace("toId", this.userId)
                ).then(res => {
                    chatId = res.data;
                }).catch(err => {
                    console.log(err);
                });
                this.$bus.$emit('startChat', { myId: this.myId, chatId: chatId });
            },

            showAvatarOrCoverChangeModal(e, type) {
                e.stopPropagation();
                this.modalType = type;
                this.$bvModal.show('change-avatar-or-cover-modal');
            },

            chooseImage(e) {
                const file = e.target.files[0];
                this.avatarOrCoverUrl = URL.createObjectURL(file);

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
                    this.newAvatarOrCover = res.data;
                }).catch(res => {
                    console.log(res.response);
                })
            },

            async saveAvatarOrCover() {
                if (this.modalType === 'avatar') {
                    this.profile.Avatar = this.newAvatarOrCover;
                } else if (this.modalType === 'cover') {
                    this.profile.Cover = this.newAvatarOrCover;
                }
                
                await this.$http.patch(
                    this.getProfileUrl.replace("userId", this.myId),
                    this.profile
                ).then(res => { }).catch(err => {
                    console.log(err.response);
                });
            },
        }
    }
</script>

<style scoped>
    a, u {
        text-decoration: none;
        color: var(--white);
    }

    #change-cover {
        position: absolute;
        margin-top: -68px;
        right: 50px;
    }
    #change-cover .b-icon {
        margin: 0;
    }

    #profile-header {
        width: 100%;
        max-width: 100% !important;
        height: 100%;
        background-color: #111;
        color: var(--white);
        padding: 0 50px;
        margin: 0 !important;
    }

    .card-img-top {
        margin: 0 0 10px 0;
        max-height: 400px;
        border-radius: 10px;
        border-top-left-radius: 0px;
        border-top-right-radius: 0px;
    }

    #basic-info {
        display: flex;
        flex-direction: row;
        gap: 50px;
        align-items: center;
        width: 100%;
        padding: 68px 68px 10px 68px;
        margin-top: -150px;
    }

    #info {
        width: 100%;
        margin-top: 65px;
    }

    #name-follow {
        display: flex;
        justify-content: space-between;
        align-items: end;
        width: 100%;
    }

    #name-follow .actions {
        display: flex;
        gap: 5px;
        align-items: center;
        margin-bottom: 12px;
    }

    #name-follow .actions .btn {
        max-height: 50px;
        display: flex;
        align-items: center;
    }

    .actions .btn .b-icon {
        margin: 0;
    }

    #follow, #follow div {
        display: flex;
        flex-direction: row;
        gap: 10px;
    }

    #follow .b-icon {
        margin: 4px 0 0 0;
    }

    #name, #address, #about-me, #follow {
        margin-bottom: 10px;
    }

    .b-icon {
        margin-right: 10px;
    }

    .user-card-medium {

    }
    .user-card-medium .card-body {
        display: flex;
        flex-direction: row;
        align-items: center;
        color: var(--white);
        padding: 5px;
    }
    .medium-card-right {
        padding: 0 !important;
        display: flex;
        flex-direction: column;
        justify-content: space-between;
        gap: 10px;
        align-items: start;
    }
    .medium-card-right .actions {
        width: 100%;
        display: flex;
        gap: 5px;
        align-items: end;
        justify-content: space-between;
    }
    .user-card-medium .actions .btn {
        width: 100%;
    }

    .user-card-small {
        display: flex;
        flex-direction: row;
        justify-content: space-between;
        max-height: 20px;
        align-items: center;
    }
    .user-card-small .actions {
        display: flex;
        gap: 10px;
        align-items: end;
    }

    #preview {
        display: flex;
        justify-content: space-around;
    }
</style>