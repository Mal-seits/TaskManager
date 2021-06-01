import React, { Component } from 'react';
import { Route } from 'react-router';
import Layout from './Components/Layout';
import Login from './Pages/Login';
import Signup from './Pages/Signup';
import { AuthContextComponent, useAuthContext } from './AuthContext';

export default class App extends Component {

  render() {
    return (
      <AuthContextComponent>
      <Layout>
          <Route exact path='/signup' component={Signup} />
          <Route exact path='/login' component={Login} />

      </Layout>
  </AuthContextComponent>

    );
  }
}
