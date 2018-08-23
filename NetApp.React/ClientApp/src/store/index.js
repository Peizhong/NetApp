import { combineReducers } from "redux";
import * as Counter from "./Counter";
import * as WeatherForecasts from "./WeatherForecasts";

export default combineReducers({
  Counter,
  WeatherForecasts
});
