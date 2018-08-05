import React, { Component } from 'react';
import {
  Router,
  Route,
  Link
} from 'react-router-dom';
import './App.css';
import 'antd/dist/antd.css';
import Home from './components/Home'
import Tab2 from './components/Tab2'
import { Menu, Icon } from 'antd';

import createHistory from 'history/createBrowserHistory'
const history = createHistory()

class App extends Component {
  render() {
    return (
      <div className="App">
        <Router history={history}>
          <Menu mode="horizontal">
            <Menu.Item><Link to="/">首页</Link></Menu.Item>
            <Menu.Item><Link to="/tab2">计划表</Link></Menu.Item>
            <div className="content">
              <Route exact path="/" component={Home} />
              <Route exact path="/tab2" component={Tab2} />
            </div>
          </Menu>
        </Router>
      </div>
    );
  }
}

export default App;
