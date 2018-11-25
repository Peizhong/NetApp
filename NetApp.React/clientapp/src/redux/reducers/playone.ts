import { CALL_API, RECV_API } from "../actionTypes";

const initialState = {
  data: null,
  isLoading: false,
  message: null
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
        data: action.payload.data,
        isLoading: false,
        message: action.payload.message
      };
    default:
      return state;
  }
}
