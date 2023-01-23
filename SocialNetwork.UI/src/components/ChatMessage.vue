<template>
    <div>
        <div
            v-if="msg.UserId === myId"
            class="chat__mymessage"
            :class="[isSame ? '' : 'chat__first']">
            <b-dropdown size="md" variant="link" toggle-class="text-decoration-none" no-caret>
                <template #button-content><b-icon icon="three-dots"></b-icon></template>
                <b-dropdown-item @click="onDeleteClick"><b-icon icon="trash-fill"></b-icon>&nbsp;Delete</b-dropdown-item>
            </b-dropdown>
            <p class="chat__mymessage__paragraph">{{msg.Text}}</p>
        </div>
        <div
            v-else
            class="chat__yourmessage"
            :class="[isSame ? '' : 'chat__first']">
            <div class="chat__yourmessage__avartar">
                <b-avatar v-if="avatar" :src="avatar" size="2rem"></b-avatar>
            </div>
            <div>
                <p class="chat__yourmessage__user" v-if="!isSame">
                    {{name}}
                </p>
                <div class="chat__yourmessage__p">
                    <p class="chat__yourmessage__paragraph">
                        {{msg.Text}}
                    </p>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    export default {
        props: ["msg", "prev", "myId"],
        data() {
            return {
                getNameAvatarUrl: "https://localhost:6868/Users/userId/profile/avatarname",
                avatar: null,
                name: null,
                isSame: false,
            };
        },
        async created() {
            this.isSame = this.isSamePerson(this.msg, this.prev);
            if (this.msg.UserId !== this.myId) {
                await this.getAvatarName();
            }
            if (this.msg.UserId !== this.myId && !this.msg.Read) {
                this.$bus.$emit("newMsg", true);
            }
        },
        methods: {
            async getAvatarName() {
                await this.$http.get(
                    this.getNameAvatarUrl.replace("userId", this.msg.UserId)
                ).then(res => {
                    this.avatar = require(`@/assets/${res.data.Avatar}`);
                    this.name = res.data.Name;
                }).catch(err => {
                    console.log(err);
                });
            },

            isSamePerson(msg, prev) {
                if (prev === null) {
                    return false;
                } else if (prev[0]?.UserId == msg?.UserId) {
                    return true;
                } else {
                    return false;
                }
            },

            onDeleteClick(e) {
                e.stopPropagation();
                //this.$http.delete(
                //    this.getPostUrl.replace("postId", this.postId)
                //).then((res) => {
                //    console.log(res);
                //    this.$router.go();
                //}).catch((res) => {
                //    console.log(res.response)
                //});
            }
        },
    };
</script>

<style scoped>
    .chat__mymessage {
        display: flex;
        justify-content: right;
        align-items: center;
        margin: 0;
        min-height: 40px;
        line-break: anywhere;
    }

    .chat__mymessage__paragraph {
        margin: 0;
        border-radius: 20px 20px 0px 20px;
        max-width: 180px;
        background-color: #bbc4ef;
        color: #000;
        padding: 0.8rem;
        font-size: 14px;
    }

    .chat__first {
        margin-top: 10px;
    }

    .chat__yourmessage {
        display: flex;
    }

    .chat__yourmessage__avartar {
        width: 40px;
        margin-right: 10px;
    }

    .chat__yourmessage__img {
        width: 20px;
        height: 20px;
        border-radius: 50%;
        object-fit: cover;
    }

    .chat__yourmessage__user {
        font-size: 14px;
        font-weight: 700;
        color: #292929;
        margin-top: 0;
        margin-block-end: 0rem;
    }

    .chat__yourmessage__p {
        display: flex;
        align-items: flex-end;
        line-break: anywhere;
    }

    .chat__yourmessage__paragraph {
        margin: 0.4rem 1rem 0 0;
        border-radius: 0px 20px 20px 20px;
        background-color: #343a40;
        max-width: 180px;
        color: var(--white);
        padding: 0.8rem;
        font-size: 14px;
    }

    .chat__yourmessage__time {
        margin: 0;
        font-size: 12px;
        color: #9c9c9c;
    }
</style>