import { POST_LOGIN, RECIVE_LOGIN } from "../actionTypes";

const initialState = { account: {}, isLoading: false };

export default function(state = initialState, action) {
  if (action.type === POST_LOGIN) {
    return {
      ...state,
      isLoading: true
    };
  }
  if (action === RECIVE_LOGIN) {
    return {
      ...state,
      isLoading: false
    };
  }
  return state;
}
