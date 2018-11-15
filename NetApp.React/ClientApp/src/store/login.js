const postLoginType = "POST_LOGIN";
const reciveLoginType = "RECIVE_LOGIN";

const initialState = { account: {}, isLoading: false };

export const reducer = (state, action) => {
  state = state || initialState;

  if (action.type === postLoginType) {
    return {
      ...state,
      isLoading: true
    };
  }
  if (action === reciveLoginType) {
    return {
      ...state,
      isLoading: false
    };
  }
  return state;
};
