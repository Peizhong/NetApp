import { UserManager } from "oidc-client";
import { CHECK_LOGIN, REVC_LOGIN, SEND_LOGIN } from "./actionTypes";

const config = {
  authority: "http://localhost:5050",
  client_id: "js",
  post_logout_redirect_uri: "http://localhost:3000/logout",
  redirect_uri: "http://localhost:3000",
  response_type: "id_token token",
  scope: "openid profile api1"
};
const mgr = new UserManager(config);

export const checkLogin = () => {
  return (dispatch: any) => {
    dispatch({
      payload: null,
      type: CHECK_LOGIN
    });
    mgr.getUser().then(user => {
      dispatch(recvLogin(user));
    });
  };
};

export const sendLogin = (content: any) => {
  return (dispatch: any) => {
    dispatch({
      payload: null,
      type: SEND_LOGIN
    });
    mgr.signinRedirect();
    /*
    fetch("http://www.google.com").then(
      response => {
        console.log(response);
        dispatch(recvLogin("ok"));
      },
      reject => console.log("erro")
    );*/
    // setTimeout(() => dispatch(recvLogin("ok")), 4000);
  };
};

export const recvLogin = (content: any) => ({
  payload: {
    permissions: ["admin"],
    profile: content && content.profile
  },
  type: REVC_LOGIN
});
