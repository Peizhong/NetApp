import { CALL_API, RECV_API } from "../actionTypes";

const initialState = {
  data: null,
  isLoading: false
};

export default function(state = initialState, action: any) {
  switch (action.type) {
    case CALL_API:
      return {
        ...state,
        data: [],
        isLoading: true
      };
    case RECV_API:
      return {
        ...state,
        data: action.payload,
        isLoading: false
      };
    default:
      return state;
  }
}
