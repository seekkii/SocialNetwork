<template>
    <div>
        <h1>Register</h1>
        <b-form @submit="onSubmit" @reset="onReset" v-if="show">
            <b-form-group
                id="input-group-1"
                label="Email address:"
                label-for="input-1"
                description="We'll never share your email with anyone else."
            >
                <b-form-input
                    id="input-1"
                    v-model="form.Email"
                    type="email"
                    placeholder="Enter email"
                    required
                ></b-form-input>
            </b-form-group>

            <b-form-group id="input-group-2" label="Password:" label-for="input-2">
                <b-form-input
                    id="input-2"
                    v-model="form.Password"
                    placeholder="Enter password"
                    required
                ></b-form-input>
            </b-form-group>

            <b-button type="submit" variant="primary">Submit</b-button>
            <b-button type="reset" variant="danger">Reset</b-button>
        </b-form>
        <b-card class="mt-3" header="Form Data Result">
            <pre class="m-0">{{ form }}</pre>
        </b-card>
    </div>
</template>

<script>
    import axios from 'axios';

    export default {
        name: 'Register',
        data() {
            return {
                registerUrl: "https://localhost:6868/Users/register",
                form: {},
                show: true
            }
        },
        methods: {
            onSubmit(event) {
                event.preventDefault()

                axios.post(
                    this.registerUrl,
                    JSON.parse(JSON.stringify(this.form))
                ).then(res => {
                    //console.log(res);
                    this.$router.push({
                        name: 'login'
                    });
                }).catch(res => {
                    console.log(res.response);
                })
            },
            onReset(event) {
                event.preventDefault()
                // Reset our form values
                this.form = {}
                // Trick to reset/clear native browser form validation state
                this.show = false
                this.$nextTick(() => {
                    this.show = true
                })
            }
        }
    }
</script>

<style scoped>
</style>