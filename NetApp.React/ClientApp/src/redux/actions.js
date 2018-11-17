import { SEND_LOGIN, REVC_LOGIN } from "./actionTypes";

export const sendLogin = content => {
  return function(dispatch) {
    dispatch({
      type: SEND_LOGIN,
      payload: {
        username: "hi",
        password: "password"
      }
    });
    /*
    fetch("http://www.google.com").then(
      response => {
        console.log(response);
        dispatch(recvLogin("ok"));
      },
      reject => console.log("erro")
    );*/
    setTimeout(() => dispatch(recvLogin("ok")), 1000);
  };
};

export const recvLogin = content => ({
  type: REVC_LOGIN,
  payload: {
    profile: content,
    permissions: ["admin"]
  }
});
