import React, { Component } from 'react';
import { Route, NavLink } from 'react-router-dom';

import { Layout, Menu } from 'antd';
import coldplay from './Coldplay.gif';
import './App.css';

import Home from './components/Home';
import CatalogTree from './components/CatalogTree';
import FetchData from './components/FetchData';

const { Header, Content } = Layout;
const SubMenu = Menu.SubMenu;
const MenuItemGroup = Menu.ItemGroup;

const About = () => (
  <div style={{ background: '#fff', textAlign: 'center' }}>
    <p>about me and you</p>
    <p>关于你和我</p>
  </div>
);

class App extends Component {
  render() {
    return (
      <Layout className="layout">
        <Header style={{ position: 'fixed', zIndex: 1, width: '100%' }}>
          <img src={coldplay} className="App-logo" alt="logo" />
          <Menu theme="dark" mode="horizontal" style={{ lineHeight: '64px' }}>
            <Menu.Item key="home">
              <NavLink to="/" exact activeClassName="selected">
                Home
              </NavLink>
            </Menu.Item>
            <SubMenu title="Showcase">
              <MenuItemGroup title="Mall">
                <Menu.Item key="setting:3">
                  <NavLink to="/showcase" activeClassName="selected">
                    Catalog Tree
                  </NavLink>
                </Menu.Item>
                <Menu.Item key="setting:4">
                  <NavLink to="/fetchData" activeClassName="selected">
                    Fetch Data
                  </NavLink>
                </Menu.Item>
              </MenuItemGroup>
            </SubMenu>
            <Menu.Item key="about">
              <NavLink to="/about" activeClassName="selected">
                About
              </NavLink>
            </Menu.Item>
          </Menu>
        </Header>
        <Content style={{ padding: '0 50px', marginTop: 64 }}>
          <Route exact path="/" component={Home} />
          <Route path="/showcase" component={CatalogTree} />
          <Route path="/about" component={About} />
          <Route path="/fetchData" component={FetchData} />
        </Content>
      </Layout>
    );
  }
}

export default App;
