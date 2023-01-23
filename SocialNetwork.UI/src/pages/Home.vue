<template>
    <b-container fluid>
        <b-row cols="3">
            <b-col cols="3">Column</b-col>
            <b-col cols="6">
                <feed
                    v-if="myId"
                    :userId="myId"
                    :myId="myId"
                    :jwtToken="jwtToken"
                    :feedType="'feed'"/>
            </b-col>
            <b-col cols="3">Column</b-col>
        </b-row>
    </b-container>
</template>

<script>
    export default {
        name: 'Home',
        data() {
            return {
                getUserUrl: "https://localhost:6868/Users/userId",
                getUserProfileUrl: "https://localhost:6868/Users/userId/profile",
                jwtToken: "",
                myId: "",
            };
        },
        async created() {
            this.jwtToken = this.$route.params.authToken;
            if (this.jwtToken === undefined) {
                this.jwtToken = this.$cookies.get('sn-auth-token');
                if (this.jwtToken === null) {
                    this.$router.push('login');
                }
            }
            this.$http.defaults.headers.common["Authorization"] = this.jwtToken;
            this.myId = this.$cookies.get('sn-user-id');

            await this.$bus.$emit("getCreds", { jwtToken: this.jwtToken, myId: this.myId });
        },
        async mounted() {
            await this.getUser();
            await this.getUserProfile();
        },
        methods: {
            logout() {
                this.jwtToken = this.$cookies.get('sn-auth-token');
                if (this.jwtToken !== null) {
                    this.$cookies.remove('sn-auth-token');
                }
                this.$nextTick(function () {
                    this.$router.push('login');
                });
            },

            async getUser() {
                if (this.myId !== null) {
                    await this.$http.get(
                        this.getUserUrl.replace("userId", this.myId)
                    ).then((res) => {
                        this.user = res.data;
                    }).catch(err => {
                        console.log(err.response);
                    });
                }
            },
    
            async getUserProfile() {
                var self = this;
                await this.$http.get(
                    this.getUserProfileUrl.replace("userId", this.myId)
                ).then(res => {
                    console.log(res.data);
                    if (!res.data.Timestamp) {
                        this.$router.push({
                            name: 'updateprofile',
                            params: {
                                userProfile: res.data,
                                myId: self.myId,
                                jwtToken: self.jwtToken
                            }
                        });
                    }
                }).catch(err => {
                    console.log(err.response);
                });
            },
        },
    };
</script>

<style scoped>
    
</style>