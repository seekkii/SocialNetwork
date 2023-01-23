<template>
    <div>
        <h1>Update profile</h1>
        <b-form @submit="onSubmit" v-if="show">
            <!--<b-form-group>
                <b-form-file
                    placeholder="Add avatar"
                    @change="onFileChange"
                ></b-form-file>
                <div id="preview">
                    <b-img thumbnail center v-if="avatarUrl" :src="avatarUrl" />
                </div>
            </b-form-group>-->

            <b-form-group>
                <b-form-input
                    id="input-name"
                    v-model="form.Name"
                    placeholder="Name"
                    required
                ></b-form-input>
            </b-form-group>

            <b-form-group>
                <b-form-datepicker id="input-dob" v-model="form.DateOfBirth" class="mb-2"></b-form-datepicker>
            </b-form-group>

            <b-form-group>
                <b-form-textarea
                    id="textarea"
                    v-model="form.SelfIntroduction"
                    placeholder="About yourself..."
                    rows="3"
                    max-rows="6"
                ></b-form-textarea>
            </b-form-group>

            <b-form-group>
                <b-form-radio-group
                    v-model="form.Gender"
                    :options="options"
                    class="mb-3"
                    value-field="value"
                    text-field="name"
                ></b-form-radio-group>
            </b-form-group>

            <b-form-group>
                <b-form-input
                    id="input-location"
                    v-model="form.CurrentLocation"
                    placeholder="Address"
                ></b-form-input>
            </b-form-group>

            <b-button type="submit" variant="primary">Submit</b-button>
        </b-form>
        <b-card class="mt-3" header="Form Data Result">
            <pre class="m-0">{{ form }}</pre>
        </b-card>
    </div>
</template>

<script>
    import axios from 'axios';
    export default {
        data() {
            return {
                profileUrl: "https://localhost:6868/Users/userId/profile",
                show: true,
                form: {},
                options: [
                    { value: 0, name: 'Male' },
                    { value: 1, name: 'Female' },
                    { value: null, name: 'Prefer not to share'},
                ],
                avatarUrl: "",
            }
        },
        created() {
            this.originalform = this.$route.params.userProfile;
            this.form = this.$route.params.userProfile;
        },
        methods: {
            onSubmit(event) {
                event.preventDefault();
                this.form.Timestamp = new Date();
                axios.patch(
                    this.profileUrl.replace("userId", this.$route.params.myId),
                    JSON.parse(JSON.stringify(this.form)),
                    {
                        headers: {
                            Authorization: `Bearer ${this.$route.params.jwtToken}`
                        }
                    }
                ).then((res) => {
                    console.log(res);
                    this.$router.push({
                        name: 'home',
                        params: {
                            jwtToken: this.$route.params.jwtToken,
                            myId: this.$route.params.myId,
                        }
                    });
                }).catch((res) => {
                    console.log(res.response);
                })
            },
            onFileChange(e) {
                const file = e.target.files[0];
                this.avatarUrl = URL.createObjectURL(file);
            }
        }
    }
</script>

<style scoped>
    #preview img {
        width: 200px;
        height: 200px;
        object-fit: cover;
    }
</style>