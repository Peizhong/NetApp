import React, { Component } from 'react';
import { connect } from 'react-redux';
import {
  Form, Icon, Input, Button, Checkbox,
} from 'antd';

const FormItem = Form.Item;

class LoginForm extends Component {
  handleSubmit = (e)=> {
    e.preventDefault();
    this.props.form.validateFields((err, values) => {
      if (!err) {
        console.log('Received values of form: ', values);
        const url = 'http://localhost:5050/'
      }
    });
  }

  render() {
    const { getFieldDecorator } = this.props.form;
    return (
      <Form onSubmit={this.handleSubmit} className="login-form">
        {'这个是客户端, 用IdentityServer验证?'}
        <FormItem>
          {getFieldDecorator('userName', {
            rules: [{ required: true, message: 'Please input your username!' }],
          })(
            <Input
              prefix={<Icon type="user" style={{ fontSize: 13 }} />}
              placeholder="Username"
            />,
          )}
        </FormItem>
        <FormItem>
          {getFieldDecorator('password', {
            rules: [{ required: true, message: 'Please input your Password!' }],
          })(
            <Input
              prefix={<Icon type="lock" style={{ fontSize: 13 }} />}
              type="password"
              placeholder="Password"
            />,
          )}
        </FormItem>
        <FormItem>
          {getFieldDecorator('remember', {
            valuePropName: 'checked',
            initialValue: true,
          })(<Checkbox>Remember me</Checkbox>)}
          <a className="login-form-forgot" style={{ float: 'right' }}>
            Forgot password
          </a>
          <Button
            type="primary"
            htmlType="submit"
            className="login-form-button"
            style={{ width: '100%' }}
          >
            Log in
          </Button>
          Or
          {' '}
          <a>register now!</a>
        </FormItem>
      </Form>
    );
  }
}

const LF = Form.create()(LoginForm);


class Login extends Component{
  render(){
    const isLoading = this.props;
    console.log(isLoading)
    return (
      <div>
        <LF/>
        {isLoading}
      </div>
    )
  }
}

export default connect(
  state => state.Login
)(Login);
