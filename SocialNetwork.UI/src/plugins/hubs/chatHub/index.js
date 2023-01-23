import {
    HubConnectionBuilder,
    LogLevel,
} from '@microsoft/signalr';
import Vue from 'vue';

export default {
    install() {
        // init connection to backend hub ('chatsocket')
        const connection = new HubConnectionBuilder()
            .withUrl('https://localhost:6868/chatsocket')
            .configureLogging(LogLevel.Information)
            .build();

        // init vue shortcut
        const chatHub = new Vue();
        Vue.prototype.$chatHub = chatHub;

        // handle users like chat
        connection.on('Send', (message) => {
            chatHub.$emit('message-received', message)
        });

        // handle users comment on chat
        connection.on('Delete', (messageId) => {
            chatHub.$emit('message-deleted', messageId)
        });

        // handle tracking users view chat
        chatHub.chatOpened = chatId => {
            return startedPromise
                .then(() => connection.invoke('ViewingChat', chatId))
                .catch(console.error);
        }
        chatHub.chatClosed = chatId => {
            return startedPromise
                .then(() => connection.invoke('LeavingChat', chatId))
                .catch(console.error);
        }

        // attempt to reconnect if fails
        let startedPromise = null
        function start() {
            startedPromise = connection.start().catch(err => {
                console.error('Failed to connect with hub', err)
                return new Promise((resolve, reject) =>
                    setTimeout(() => start().then(resolve).catch(reject), 5000))
            })
            return startedPromise
        }
        connection.onclose(() => start());

        start();
    }
};