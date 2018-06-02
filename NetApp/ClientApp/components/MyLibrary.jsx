import React from "react";
import { connect } from "react-redux";

class MyLibaray extends React.Component {
  constructor(props) {
    super(props);
  }

  render() {
    return (
      <div>
        <h1>Hello, World!</h1>
        <p>提交新的代码，然后会发生什么?</p>
        <div className="progress">
          <div
            className="progress-bar progress-bar-success progress-bar-striped active"
            role="progressbar"
            aria-valuenow="60"
            aria-valuemin="0"
            aria-valuemax="100"
            style={{ width: "43%" }}
          >
            43%
          </div>
        </div>
        <div className="list-group">
          <a href="#" className="list-group-item">
            Instagram
          </a>
          <a href="#" className="list-group-item">
            WhatsApp
          </a>
          <a href="#" className="list-group-item">
            Oculus
          </a>
        </div>
        <button className="btn btn-primary pull-left">Look Good</button>
      </div>
    );
  }
}

export default connect()(MyLibaray);
