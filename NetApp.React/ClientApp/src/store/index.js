import { combineReducers } from 'redux';
import * as Counter from './Counter';
import * as WeatherForecasts from './WeatherForecasts';
import * as MyTree from './MyTree';

export default combineReducers({
  counter: Counter.reducer,
  weatherForecasts: WeatherForecasts.reducer,
  myTree: MyTree.reducer,
});
