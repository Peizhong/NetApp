import React, { Component } from "react";
import { connect } from "react-redux";
import { BrowserRouter as Router, Route, Link } from "react-router-dom";
import { Layout, Menu, Icon, Modal, Skeleton } from "antd";

import NormalLoginForm from "./components/NormalLoginForm";

import coldplay from "./Coldplay.gif";
import "./App.css";

const { Content, Sider } = Layout;

const Index = () => <h2>Hello</h2>;
const About = () => <h2>About</h2>;
const Users = () => <h2>Users</h2>;

class App extends React.Component {
  constructor(props) {
    super(props);
  }

  render() {
    const { showLogin } = this.props;
    return (
      <Router>
        <Layout>
          <Sider
            style={{
              overflow: "auto",
              height: "100vh",
              position: "fixed",
              left: 0
            }}
          >
            <div className="logo">
              <img src={coldplay} className="App-logo" alt="logo" />
            </div>
            {!showLogin && (
              <Menu theme="dark" mode="inline" defaultSelectedKeys={["1"]}>
                <Menu.Item key="1">
                  <Link to="/">
                    <Icon type="smile" />
                    <span className="nav-text">Index</span>
                  </Link>
                </Menu.Item>
                <Menu.Item key="2">
                  <Link to="/users/">
                    <Icon type="user" />
                    <span className="nav-text">Users</span>
                  </Link>
                </Menu.Item>
                <Menu.Item key="3">
                  <Link to="/about/">
                    <Icon type="cloud-o" />
                    <span className="nav-text">About</span>
                  </Link>
                </Menu.Item>
              </Menu>
            )}
          </Sider>
          <Layout style={{ marginLeft: "200px", height: "100vh" }}>
            <Modal
              title="Login Required"
              visible={showLogin}
              onOk={this.handleOk}
              onCancel={this.handleCancel}
              closable={false}
              footer={null}
            >
              <NormalLoginForm />
            </Modal>
            <Content style={{ margin: "24px 16px 0", overflow: "initial" }}>
              <Skeleton loading={showLogin} active>
                <Route path="/" exact component={Index} />
                <Route path="/about/" component={About} />
                <Route path="/users/" component={Users} />
              </Skeleton>
            </Content>
          </Layout>
        </Layout>
      </Router>
    );
  }
}

const mapStateToProps = state => ({
  showLogin: !state.account.profile
});

export default connect(mapStateToProps)(App);
