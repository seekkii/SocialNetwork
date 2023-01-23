<template>
    <div v-if="msgs" class="chat">
        <div class="chat__header" :class="[unreadMsg ? 'new__msg' : '']">
            <span class="chat__header__greetings">
                <b-avatar v-if="avatar" :src="avatar" size="2rem"></b-avatar>
                {{chatName}}
                <b-icon v-if="unreadMsg" variant="danger" icon="circle-fill"></b-icon>
            </span>
            <b-icon icon="x" class="close-chat-btn" @click="closeChat"></b-icon>
        </div>

        <chat-history :msgs="msgs" :myId="myId"/>
        <chat-form @sendMessage="sendMessage"/>
    </div>
</template>

<script>
    import _ from 'lodash';
    export default {
        props: ["myId", "jwtToken", "chatId"],
        data() {
            return {
                getChatNameUrl: "https://localhost:6868/Chat/chatId/name",
                getOneToOneChatBuddyId: "https://localhost:6868/Chat/chatId/userId/buddy/",
                getBuddyInfo: "https://localhost:6868/Users/userId/profile/avatarname",
                getChatHistoryUrl: "https://localhost:6868/Chat/chatId/history",
                sendMessageUrl: "https://localhost:6868/Chat/messages",
                markAsReadMessagesUrl: "https://localhost:6868/Chat/chatId/messages/read/userId",

                chatName: null,
                avatar: null,
                msgs: null,

                unreadMsg: false,
            };
        },
        async created() {
            this.$http.defaults.headers.common["Authorization"] = this.jwtToken;

            await this.$chatHub.$on('message-received', this.messageReceived);

            await this.$bus.$on("newMsg", val => {
                if (this.unreadMsg === true && val === false) {
                    this.$http.post(
                        this.markAsReadMessagesUrl.replace("chatId", this.chatId).replace("userId", this.myId),
                        null
                    ).then(res => {
                        console.log(res);
                    }).catch(err => {
                        console.log(err);
                    });
                }
                this.unreadMsg = val;
            });
        },
        async mounted() {
            await this.$chatHub.chatOpened(this.chatId);
            await this.getChat();
            await this.getChatHistory();
        },
        beforeDestroy() {
            this.$chatHub.$off('message-received', this.messageReceived);
        },
        methods: {
            async getChat() {
                await this.$http.get(
                    this.getChatNameUrl.replace("chatId", this.chatId)
                ).then(res => {
                    if (res.data === "") {
                        this.$http.get(
                            this.getOneToOneChatBuddyId.replace("chatId", this.chatId).replace("userId", this.myId)
                        ).then(res => {
                            let buddyId = res.data;
                            this.$http.get(
                                this.getBuddyInfo.replace("userId", buddyId)
                            ).then(res => {
                                this.chatName = res.data.Name;
                                this.avatar = require(`@/assets/${res.data.Avatar}`);
                            }).catch(err => {
                                console.log(err.response);
                            });
                        }).catch(err => {
                            console.log(err);
                        });
                    } else {
                        this.chatName = res.data;
                    }
                }).catch(err => {
                    console.log(err);
                });
            },

            async getChatHistory() {
                await this.$http.get(
                    this.getChatHistoryUrl.replace("chatId", this.chatId)
                ).then(res => {
                    this.msgs = res.data;
                }).catch(res => {
                    console.log(res.response);
                });
            },

            async sendMessage(msg) {
                msg.UserId = this.myId;
                msg.ChatId = this.chatId;
                //console.log(msg)
                await this.$http.post(
                    this.sendMessageUrl,
                    msg
                ).then(res => {
                    console.log(res);
                }).catch(res => {
                    console.log(res.response);
                });
            },

            messageReceived(message) {
                message = _.mapKeys(message, function (v, k) {
                    return _.upperFirst(k);
                });
                console.log(message);
                this.msgs.push(message);
            },

            closeChat() {
                this.$emit("closeChat", this.chatId);
            },
        },
    };
</script>

<style scoped>
    .chat {
        display: flex;
        flex-direction: column;
        justify-content: space-between;
        box-shadow: 0 0 6px #9ecaed;
        border-radius: 4px;
        position: fixed;
        bottom: 0;
        right: 20px;
        z-index: 1;
    }

    .chat__header {
        background-color: #343a40;
        box-shadow: 0px 3px 10px rgba(0, 0, 0, 0.05);
        border-radius: 4px 4px 0 0;
        padding: 10px;
        font-size: 16px;
        font-weight: 700;
        display: flex;
        justify-content: space-between;
        align-items: center;
    }
    .chat__header.new__msg {
        background-color: var(--primary);
    }

    .chat__header__greetings {
        color: var(--white);
    }

    .close-chat-btn {
        cursor: pointer;
    }
</style>