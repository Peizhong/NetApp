import { SEND_LOGIN, REVC_LOGIN } from "../actionTypes";

const initialState = {
  profile: null,
  permissions: [],
  isLoading: false
};

export default function(state = initialState, action) {
  switch (action.type) {
    case SEND_LOGIN: {
      return {
        ...state,
        isLoading: true
      };
    }
    case REVC_LOGIN: {
      const load = action.payload;
      return {
        ...state,
        profile: {
          name: "wpz",
          sex: "male"
        },
        isLoading: false
      };
    }
    default:
      return state;
  }
}
