import {
    HubConnectionBuilder,
    LogLevel,
} from '@microsoft/signalr';
import Vue from 'vue';

export default {
    install() {
        // init connection to backend hub ('postsocket')
        const connection = new HubConnectionBuilder()
            .withUrl('https://localhost:6868/postsocket')
            .configureLogging(LogLevel.Information)
            .build();

        // init vue shortcut
        const postHub = new Vue();
        Vue.prototype.$postHub = postHub;

        // handle users like post
        connection.on('Reaction', (postId, userId, like) => {
            postHub.$emit('post-react', { postId, userId, like })
        });

        // handle users comment on post
        connection.on('Comment', (comment) => {
            postHub.$emit('post-commented-on', comment)
        });

        // handle users edit a comment
        connection.on('Edit', (comment) => {
            postHub.$emit('comment-edited', comment)
        });

        // handle users edit a comment
        connection.on('Delete', (commentId) => {
            postHub.$emit('comment-deleted', commentId)
        });

        // handle tracking users view post
        postHub.postOpened = postId => {
            return startedPromise
                .then(() => connection.invoke('ViewingPost', postId))
                .catch(console.error);
        }
        postHub.postClosed = postId => {
            return startedPromise
                .then(() => connection.invoke('LeavingPost', postId))
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