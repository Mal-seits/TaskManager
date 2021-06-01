import React, { Component } from 'react';
import { Route } from 'react-router';
import Layout from './Components/Layout';
import Home from './Pages/Home';
import Login from './Pages/Login';
import Logout from './Pages/Logout';
import Signup from './Pages/Signup';
import PrivateRoute from './PrivateRoute';
import { UserContextComponent } from './UserContext';

export default class App extends Component {

  render() {
    return (
      <UserContextComponent>
        <Layout>
          <Route exact path='/login' component={Login} />
          <Route exact path='/signup' component={Signup} />
          <PrivateRoute exact path ='/' component={Home} />
          <PrivateRoute exact path ='/logout' component={Logout} />
        </Layout>
      </UserContextComponent>
    );
  }
}
