import { REVC_LOGIN,SEND_LOGIN } from "./actionTypes";

export const sendLogin = (content:any)=> {
  return (dispatch:any) => {
    dispatch({
      payload: null,
      type: SEND_LOGIN
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

export const recvLogin = (content:any) => ({
  payload: {
    permissions: ["admin"],
    profile: content,
  },
  type: REVC_LOGIN
});
