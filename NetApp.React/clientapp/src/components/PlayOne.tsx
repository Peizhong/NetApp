import { Button, Spin } from "antd";
import * as React from "react";
import { connect } from "react-redux";
import { callCategoriesApi, callGatewayCategoriesApi } from "../redux/actions";

export interface IProps {
  isLoading: boolean;
  callCategoriesApi: () => void;
  callGatewayCategoriesApi: () => void;
  data: any[];
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

class PlayOne extends React.Component<IProps> {
  public render() {
    let keyIndex = 0;
    return (
      <div>
        <Button.Group>
          <Button type="primary" value="direct" onClick={this.handleCallApi}>
            Direct
          </Button>
          <Button type="primary" value="gateway" onClick={this.handleCallApi}>
            Gateway
          </Button>
        </Button.Group>
        <Spin spinning={this.props.isLoading}>
          <div style={exampleStyle}>
            {this.props.message ||
              (this.props.data &&
                this.props.data.map(r => (
                  <div key={keyIndex++}>
                    {r.id}: {r.name}
                  </div>
                )))}
          </div>
        </Spin>
      </div>
    );
  }
  private handleCallApi = (e: any) => {
    const {value} = e.target;
    if (value === "direct") {
      this.props.callCategoriesApi();
    } else if (value === "gateway") {
      this.props.callGatewayCategoriesApi();
    }
  };
}

const mapStateToProps = (state: any) => ({
  data: state.playone.data,
  isLoading: state.playone.isLoading,
  message: state.playone.message
});

export default connect(
  mapStateToProps,
  { callCategoriesApi, callGatewayCategoriesApi }
)(PlayOne);
