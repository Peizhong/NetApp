import { CHECK_LOGIN, REVC_LOGIN, SEND_LOGIN } from "../actionTypes";

const initialState = {
  isLoading: false,
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
    default:
      return state;
  }
}
