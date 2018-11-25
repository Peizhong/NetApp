import { Button, Spin } from "antd";
import * as React from "react";
import { connect } from "react-redux";
import { callApi } from "../redux/actions";

export interface IProps {
  isLoading: boolean;
  callApi: () => void;
  response: any[];
}

const exampleStyle = {
  background: "rgba(0,0,0,0.05)",
  borderradius: "4px",
  margin: "20px 0",
  marginbottom: "20px",
  padding: "30px 50px",
  textalign: "center"
};

class PlayOne extends React.Component<IProps> {
  public render() {
    let keyIndex = 0;
    return (
      <div>
        <Button type="primary" onClick={this.handleCallApi}>
          Primary
        </Button>
        <Spin spinning={this.props.isLoading}>
          <div style={exampleStyle}>
            {this.props.response &&
              this.props.response.map(r => (
                <div key={keyIndex++}>
                  {r.type}: {r.value}
                </div>
              ))}
          </div>
        </Spin>
      </div>
    );
  }
  private handleCallApi = (e: any) => {
    this.props.callApi();
  };
}

const mapStateToProps = (state: any) => ({
  isLoading: state.playone.isLoading,
  response: state.playone.data
});

export default connect(
  mapStateToProps,
  { callApi }
)(PlayOne);
