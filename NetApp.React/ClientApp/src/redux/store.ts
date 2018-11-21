import { applyMiddleware, createStore } from "redux";
// Logger with default options
import logger from "redux-logger";
import thunk from "redux-thunk";
import rootReducer from "./reducers";

export default createStore(rootReducer, applyMiddleware(thunk, logger));
