
import React, { useState } from 'react';
import getAxios from '../AuthAxios';
import { useHistory } from 'react-router-dom';
import { useUserContext } from '../UserContext';

const Login = () => {

    const [formData, setFormData] = useState({ email: '', password: '' });
    let { email, password } = formData;
    const [isValid, setIsValid] = useState(true);
    const history = useHistory();
    const { setUser } = useUserContext();
    const [disable, setDisable] = useState(false);


    const onTextChange = e => {
        let formDataCopy = { ...formData };
        formDataCopy[e.target.name] = e.target.value;
        setFormData(formDataCopy);
    }

    const onFormSubmit = async e => {

        setDisable(true);

        try {
            e.preventDefault();
            setIsValid(true);
            const { data } = await getAxios().post('/api/account/login', formData);
            localStorage.setItem('user-token', data.token);

            const { data: user } = await getAxios().get('/api/account/getcurrentuser');
            setUser(user);
            console.log(user);
            history.push('/');
            
        } 
        catch (e) {
            setIsValid(false);
            setDisable(false);
        }

    }

    return (
        <div className="col-md-6 offset-md-3 card card-body bg-light">
            <h3>Log in to your account</h3>
            {!isValid && <span>Incorrect email/password combo. Please try again, since this is a really really awesome site and you wanna get on</span>}
            <form onSubmit={onFormSubmit}>
                <input onChange={onTextChange} type="text" name="email" placeholder="Email" className="form-control" value={email} />
                <br />
                <input onChange={onTextChange} type="password" name="password" placeholder="Password" className="form-control" value={password} />
                <br />
                <button disabled={disable} className="btn btn-primary">Login</button>
            </form>
            <a href="/signup">Don't have an account? Then next time go straight to the sign up page....</a></div>
    )
}
export default Login;