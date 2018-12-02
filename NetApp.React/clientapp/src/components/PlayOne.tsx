import { Button, Col, Icon, Row, Spin } from "antd";
import * as React from "react";
import { connect } from "react-redux";
import { callCategoriesApi, callGatewayCategoriesApi, callMyGatewayApi } from "../redux/actions";

export interface IProps {
  isLoading: boolean;
  callCategoriesApi: () => void;
  callGatewayCategoriesApi: () => void;
  callMyGatewayApi: () => void;
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
        <Row>
          <Col span={24}>
            <Button.Group>
              <Button
                type="primary"
                value="direct"
                onClick={this.handleCallApi}
              >
                <Icon type="api" />
                Direct
              </Button>
              <Button
                type="primary"
                value="gateway"
                onClick={this.handleCallApi}
              >
                <Icon type="cluster" />
                Gateway
              </Button>              
              <Button
                type="primary"
                value="netapp"
                onClick={this.handleCallApi}
              >
                <Icon type="star" />
                NetApp
              </Button>
            </Button.Group>
          </Col>
        </Row>
        <Row>
          <Col span={24}>
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
          </Col>
        </Row>
      </div>
    );
  }
  private handleCallApi = (e: any) => {
    const { value } = e.target;
    if (value === "direct") {
      this.props.callCategoriesApi();
    } else if (value === "gateway") {
      this.props.callGatewayCategoriesApi();
    } else if(value==='netapp'){
      this.props.callMyGatewayApi();
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
  { callCategoriesApi, callGatewayCategoriesApi, callMyGatewayApi }
)(PlayOne);
