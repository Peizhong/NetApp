import "antd/dist/antd.css";
import "./index.css";
import React from "react";
import ReactDOM from "react-dom";
import { Provider } from "react-redux";
import { createBrowserHistory } from "history";
import { applyMiddleware, compose, createStore } from "redux";
import {
  connectRouter,
  routerMiddleware,
  ConnectedRouter
} from "connected-react-router";
import reducer from "./store/index";
import App from "./App";
import registerServiceWorker from "./registerServiceWorker";

// Create browser history to use in the Redux store
const baseUrl = document.getElementsByTagName("base")[0].getAttribute("href");
const history = createBrowserHistory({ basename: baseUrl });

// Get the application-wide store instance, prepopulating with state from the server where available.
const initialState = window.initialReduxState;
//const store = configureStore(history, initialState);
//const store = createStore(reducer);
const store = createStore(
  connectRouter(history)(reducer), // new root reducer with router state
  initialState,
  compose(
    applyMiddleware(
      routerMiddleware(history) // for dispatching history actions
      // ... other middlewares ...
    )
  )
);

const rootElement = document.getElementById("root");

ReactDOM.render(
  <Provider store={store}>
    <ConnectedRouter history={history}>
      <App />
    </ConnectedRouter>
  </Provider>,
  rootElement
);

registerServiceWorker();
