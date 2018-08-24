const requestChildTreeType = 'REQUEST_CHILD_TREE';
const receiveChildTreeType = 'RECEIVE_CHILD_TREE';

const initialState = { trees: [], isLoading: false };

export const actionCreators = {
  requestWeatherForecasts: startDateIndex => async (dispatch, getState) => {
    if (startDateIndex === getState().weatherForecasts.startDateIndex) {
      // Don't issue a duplicate request (we already have or are loading the requested data)
      return;
    }

    dispatch({ type: requestChildTreeType, startDateIndex });

    const url = `api/SampleData/WeatherForecasts?startDateIndex=${startDateIndex}`;
    const response = await fetch(url);
    const trees = await response.json();

    dispatch({ type: receiveChildTreeType, startDateIndex, trees });
  }
};

export const reducer = (state, action) => {
  state = state || initialState;

  if (action.type === requestChildTreeType) {
    return {
      ...state,
      startDateIndex: action.startDateIndex,
      isLoading: true
    };
  }

  if (action.type === receiveChildTreeType) {
    return {
      ...state,
      startDateIndex: action.startDateIndex,
      trees: action.trees,
      isLoading: false
    };
  }

  return state;
};
