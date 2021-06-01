import React from 'react';
import { useUserContext } from './UserContext';
import LoginPage from './Pages/Login';
import { Route } from 'react-router-dom';

const PrivateRoute = ({component, ...options}) => {
    const {user} = useUserContext();
    const finalComponent = !!user ? component : LoginPage;
    return <Route {...options} component={finalComponent} />;
}
export default PrivateRoute;