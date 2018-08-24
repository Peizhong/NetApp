import React from 'react';
import { BrowserRouter as Router, Route, NavLink } from 'react-router-dom';

import { Layout, Menu } from 'antd';

import Home from './components/Home';
import CatalogTree from './components/CatalogTree';

const { Header, Content } = Layout;
const SubMenu = Menu.SubMenu;
const MenuItemGroup = Menu.ItemGroup;

const About = () => (
  <div style={{ padding: 24, background: '#fff', textAlign: 'center' }}>
    <p>about me and you</p>
  </div>
);

export default () => (
  <Layout className="layout">
    <Header style={{ position: 'fixed', zIndex: 1, width: '100%' }}>
      <div className="logo" />
      <Menu theme="dark" mode="horizontal" style={{ lineHeight: '64px' }}>
        <Menu.Item key="home">
          <NavLink to="/" exact activeClassName="selected">
            Home
          </NavLink>
        </Menu.Item>
        <SubMenu title="My Showcase">
          <MenuItemGroup title="Item 1">
            <Menu.Item key="setting:1">Option 1</Menu.Item>
            <Menu.Item key="setting:2">Option 2</Menu.Item>
          </MenuItemGroup>
          <MenuItemGroup title="Item 2">
            <Menu.Item key="setting:3">Option 3</Menu.Item>
            <Menu.Item key="setting:4">Option 4</Menu.Item>
          </MenuItemGroup>
        </SubMenu>
        <Menu.Item key="showcase">
          <NavLink to="/showcase" activeClassName="selected">
            Showcase
          </NavLink>
        </Menu.Item>
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
    </Content>
  </Layout>
);
