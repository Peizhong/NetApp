import { Button, Spin } from "antd";
import * as React from "react";
import { connect } from "react-redux";
import { callIdentityApi } from "../redux/actions";

export interface IProps {
    isLoading: boolean;
    callIdentityApi: () => void;
    permissions: any[];
    message: string;
}

const exampleStyle = {
  background: "rgba(0,0,0,0.05)",
  borderradius: "4px",
  margin: "20px 0",
  marginbottom: "20px",
  padding: "30px 50px",
  textalign: "center"
};

class UserInfo extends React.Component<IProps> {
  public componentWillMount() {
    this.props.callIdentityApi();
  }

  public render() {
    let keyIndex = 0;
    return (
      <div>
        <Button type="primary" onClick={this.handleCallApi}>
          Primary
        </Button>
        <Spin spinning={this.props.isLoading}>
        <div style={exampleStyle}>
            {this.props.message ||
              (this.props.permissions &&
                this.props.permissions.map(r => (
                  <div key={keyIndex++}>
                    {r.type}: {r.value}
                  </div>
                )))}
          </div>
        </Spin>
      </div>
    );
  }
  private handleCallApi = (e: any) => {
    this.props.callIdentityApi();
  };
}

const mapStateToProps = (state: any) => ({
    isLoading: state.account.isLoading,
    message: state.account.message,
    permissions: state.account.permissions
});

export default connect(
  mapStateToProps,
  { callIdentityApi }
)(UserInfo);
