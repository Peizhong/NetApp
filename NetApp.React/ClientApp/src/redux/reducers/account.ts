import {
  CALL_IDENTITY_API,
  CHECK_LOGIN,
  RECV_IDENTITY_API,
  REVC_LOGIN,
  SEND_LOGIN
} from "../actionTypes";

const initialState = {
  isLoading: false,
  message: null,
  permissions: [],
  profile: null
};

export default function(state = initialState, action: any) {
  switch (action.type) {
    case CHECK_LOGIN: {
      return {
        ...state,
        isLoading: true
      };
    }
    case SEND_LOGIN: {
      return {
        ...state,
        isLoading: true
      };
    }
    case REVC_LOGIN: {
      // const load = action.payload;
      return {
        ...state,
        isLoading: false,
        permissions: action.payload.permissions,
        profile: action.payload.profile
      };
    }
    case CALL_IDENTITY_API:
      return {
        ...state,
        isLoading: true,
        permissions: []
      };
    case RECV_IDENTITY_API:
      return {
        ...state,
        isLoading: false,
        message: action.payload.message,
        permissions: action.payload.data
      };
    default:
      return state;
  }
}
