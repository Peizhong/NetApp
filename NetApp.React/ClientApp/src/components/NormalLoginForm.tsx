import { Button, Checkbox, Form, Icon, Input } from "antd";
import { FormComponentProps } from "antd/lib/form/Form";
import * as React from "react";
import { connect } from "react-redux";
import { sendLogin } from "../redux/actions";
import "./components.css";

const FormItem = Form.Item;

export interface IProps extends FormComponentProps {
  isLoading: boolean;
  sendLogin: (content: string) => void;
}

export interface IState {
  rememberMe: boolean;
}

class NormalLoginForm extends React.Component<IProps, IState> {
  public constructor(props: any) {
    super(props);
    this.state = {
      rememberMe: false
    };
  }

  public render() {
    const { getFieldDecorator } = this.props.form;
    return (
      <div>
        <Form onSubmit={this.handleSubmit} className="login-form">
          <FormItem>
            {getFieldDecorator("userName", {
              rules: [
                { required: true, message: "Please input your username!" }
              ]
            })(
              <Input
                prefix={
                  <Icon type="user" style={{ color: "rgba(0,0,0,.25)" }} />
                }
                placeholder="Username"
              />
            )}
          </FormItem>
          <FormItem>
            {getFieldDecorator("password", {
              rules: [
                { required: true, message: "Please input your Password!" }
              ]
            })(
              <Input
                prefix={
                  <Icon type="lock" style={{ color: "rgba(0,0,0,.25)" }} />
                }
                type="password"
                placeholder="Password"
              />
            )}
          </FormItem>
          <FormItem>
            {getFieldDecorator("remember", {
              initialValue: this.state.rememberMe,
              valuePropName: "checked"
            })(<Checkbox>Remember me</Checkbox>)}
            <a className="login-form-forgot" href="">
              Forgot password
            </a>
            <Button
              type="primary"
              htmlType="submit"
              className="login-form-button"
              loading={this.props.isLoading}
            >
              Log in
            </Button>
            Or <a href="">register now!</a>
          </FormItem>
        </Form>
      </div>
    );
  }
  private handleSubmit = (e: any) => {
    e.preventDefault();
    this.props.form.validateFields(err => {
      if (!err) {
        // const remember = this.state.rememberMe;
        this.props.sendLogin("ok");
      }
    });
  };
}

const WrappedNormalLoginForm = Form.create()(NormalLoginForm);

const mapStateToProps = (state: any) => ({
  isLoading: state.account.isLoading
});

export default connect(
  mapStateToProps,
  { sendLogin }
)(WrappedNormalLoginForm);
