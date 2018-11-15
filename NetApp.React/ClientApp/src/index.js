import "./index.css";
import React from "react";
import ReactDOM from "react-dom";
import { Provider } from "react-redux";
import store from "./redux/store";

import App from "./App";
import registerServiceWorker from "./registerServiceWorker";

// Get the application-wide store instance, prepopulating with state from the server where available.
const initialState = window.initialReduxState;

const rootElement = document.getElementById("root");

ReactDOM.render(
  <Provider store={store}>
    <App />
  </Provider>,
  rootElement
);

registerServiceWorker();
