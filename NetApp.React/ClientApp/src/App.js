import React, { Component } from "react";

import { Layout, Menu } from "antd";
import coldplay from "./Coldplay.gif";
import "./App.css";

const { Header, Content } = Layout;

class App extends Component {
  render() {
    return (
      <Layout className="layout">
        <Header style={{ position: "fixed", zIndex: 1, width: "100%" }}>
          <img src={coldplay} className="App-logo" alt="logo" />
          <Menu theme="dark" mode="horizontal" style={{ lineHeight: "64px" }} />
        </Header>
      </Layout>
    );
  }
}

export default App;
