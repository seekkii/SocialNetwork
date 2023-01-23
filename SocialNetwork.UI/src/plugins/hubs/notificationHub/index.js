import {
    HubConnectionBuilder,
    LogLevel,
} from '@microsoft/signalr';
import Vue from 'vue';

export default {
    install() {
        // init connection to backend hub ('notificationsocket')
        const connection = new HubConnectionBuilder()
            .withUrl('https://localhost:6868/notificationsocket')
            .configureLogging(LogLevel.Information)
            .build();

        // init vue shortcut
        const notificationHub = new Vue();
        Vue.prototype.$notificationHub = notificationHub;

        // handle notify when event happens
        connection.on('Notify', (notification) => {
            notificationHub.$emit('notify', notification)
        });

        // handle tracking users online
        notificationHub.online = userId => {
            return startedPromise
                .then(() => connection.invoke('Online', userId))
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