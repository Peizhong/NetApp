import { Icon, Layout, Menu, Modal, Skeleton } from "antd";
import { instanceOf } from "prop-types";
import * as React from "react";
import { Cookies, withCookies } from "react-cookie";
import { connect } from "react-redux";
import { BrowserRouter as Router, Link, Route } from "react-router-dom";
import "./App.css";
import coldplay from "./Coldplay.gif";
import NormalLoginForm from "./components/NormalLoginForm";
import { checkLogin } from "./redux/actions";

const { Content, Sider } = Layout;

const Index = () => <h2>Hello</h2>;
const About = () => <h2>About</h2>;
const Users = () => <h2>Users</h2>;

interface IProps {
  checkLogin: () => void;
  cookies: Cookies;
  profile: any;
}

class App extends React.Component<IProps> {
  public static propTypes = {
    cookies: instanceOf(Cookies).isRequired
  };

  constructor(props: any) {
    super(props);
  }

  public componentWillMount() {
    this.props.checkLogin();
  }

  public render() {
    const offlined = !this.props.profile;
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
                <Route path="/users/" component={Users} />
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

export default withCookies(
  connect(
    mapStateToProps,
    { checkLogin }
  )(App)
);
