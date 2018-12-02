import { Icon, Layout, Menu, Modal, Skeleton } from "antd";
import * as React from "react";
import { connect } from "react-redux";
import { BrowserRouter as Router, Link, Route } from "react-router-dom";
import "./App.css";
import coldplay from "./Coldplay.gif";
import NormalLoginForm from "./components/NormalLoginForm";
import PlayOne from "./components/PlayOne";
import UserInfo from "./components/UserInfo";
import { checkLogin } from "./redux/actions";

const { Content, Sider } = Layout;

const Index = () => <h2>Hello</h2>;
const About = () => <h2>About</h2>;

interface IProps {
  checkLogin: () => void;
  profile: any;
}

class App extends React.Component<IProps> {

  constructor(props: any) {
    super(props);
  }

  public componentWillMount() {
    this.props.checkLogin();
  }

  public render() {
    const {profile} = this.props;
    const offlined = !profile;
    const userName = profile?profile.name:'nobody'
    return (
      <Router>
        <Layout>
          <Sider
            style={{
              height: "100vh",
              left: 0,
              overflow: "auto",
              position: "fixed"
            }}
          >
            <div className="logo">
              <img src={coldplay} className="App-logo" alt="logo" />
            </div>
            {!offlined && (
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
                    <span className="nav-text">{userName}</span>
                  </Link>
                </Menu.Item>
                <Menu.Item key="3">
                  <Link to="/playone/">
                    <Icon type="star" />
                    <span className="nav-text">Play One</span>
                  </Link>
                </Menu.Item>
                <Menu.Item key="4">
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
              visible={offlined}
              closable={false}
              footer={null}
            >
              <NormalLoginForm />
            </Modal>
            <Content style={{ margin: "24px 16px 0", overflow: "initial" }}>
              <Skeleton
                loading={offlined}
                active={true}
                paragraph={{ rows: 16 }}
              >
                <Route path="/" exact={true} component={Index} />
                <Route path="/about/" component={About} />
                <Route path="/playone/" component={PlayOne} />
                <Route path="/users/" component={UserInfo} />
              </Skeleton>
            </Content>
          </Layout>
        </Layout>
      </Router>
    );
  }
}

const mapStateToProps = (state: any) => ({
  profile: state.account.profile
});

export default connect(
    mapStateToProps,
    { checkLogin }
  )(App);
