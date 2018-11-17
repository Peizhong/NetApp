import * as React from "react";
import { CookiesProvider } from "react-cookie";
import * as ReactDOM from "react-dom";
import { Provider } from "react-redux";
import App from "./App";
import "./index.css";
import store from "./redux/store";
import registerServiceWorker from "./registerServiceWorker";

ReactDOM.render(
  <CookiesProvider>
    <Provider store={store}>
      <App />
    </Provider>
  </CookiesProvider>,
  document.getElementById("root") as HTMLElement
);

registerServiceWorker();
