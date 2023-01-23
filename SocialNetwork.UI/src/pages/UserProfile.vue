<template>
    <div id="user-profile">
        <user-card 
            v-if="userId&&myId"
            :jwtToken="jwtToken"
            :myId="myId"
            :userId="userId"
            :size="userCardSize"/>

        <b-card no-body class="border-0">
            <b-tabs 
                card 
                active-nav-item-class="text-light bg-dark border-0 rounded-0" 
                active-tab-class="bg-dark border-0 rounded-0"
                nav-wrapper-class="bg-dark py-0 border-0 rounded-0"
                nav-class="bg-dark border-0"
                align="center" 
                justified
            > 
                <b-tab title="Personal information">
                    <personal-information 
                        :userId="userId" />
                </b-tab>
                <b-tab title="Posts" active>
                    <feed 
                        v-if="userId"
                        :userId="userId"
                        :myId="myId"
                        :jwtToken="jwtToken"
                        :feedType="'posts'"/>
                </b-tab>
                <b-tab title="Media">
                    <b-card-text>Tab contents 2</b-card-text>
                </b-tab>
                <b-tab title="Follow">
                    <follow-list 
                        v-if="userId"
                        :myId="myId"
                        :userId="userId"
                        :jwtToken="jwtToken"/>
                </b-tab>
            </b-tabs>
        </b-card>
    </div>
</template>

<script>
    export default {
        name: 'UserProfile',
        data() {
            return {
                jwtToken: '',
                myId: null,
                userId: null,

                userCardSize: 'L',
            };
        },
        async created() {
            this.myId = this.$route.params.myId;
            if (this.myId === undefined || this.myId === null) {
                this.myId = this.$cookies.get('sn-user-id');
            }

            this.userId = this.$route.params.userId;
            if (this.userId === undefined || this.userId === null) {
                this.$router.go(-1);
            }

            this.jwtToken = this.$route.params.jwtToken;
            if (this.jwtToken === undefined) {
                this.jwtToken = this.$cookies.get('sn-auth-token');
            }
            this.$http.defaults.headers.common["Authorization"] = this.jwtToken;
            await this.$bus.$emit("getCreds", { jwtToken: this.jwtToken, myId: this.myId });
        },
    }
</script>

<style scoped>
    #user-profile .card {
        border: 0;
    }
</style>