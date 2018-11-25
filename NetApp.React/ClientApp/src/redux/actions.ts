import { createBrowserHistory } from "history";
import { UserManager } from "oidc-client";
import {
  CALL_API,
  CALL_IDENTITY_API,
  CHECK_LOGIN,
  RECV_API,
  RECV_IDENTITY_API,
  REVC_LOGIN,
  SEND_LOGIN
} from "./actionTypes";

const config = {
  authority: "http://localhost:5050",
  client_id: "js",
  post_logout_redirect_uri: "http://localhost:3000/logout",
  redirect_uri: "http://localhost:3000/callback",
  response_type: "id_token token",
  scope: "openid profile api1"
};

const history = createBrowserHistory();
const mgr = new UserManager(config);

export const checkLogin = () => {
  return (dispatch: any) => {
    dispatch({
      payload: null,
      type: CHECK_LOGIN
    });
    const location = history.location;
    if (location.pathname.includes("callback")) {
      mgr
        .signinRedirectCallback()
        .then(() => {
          mgr.getUser().then(user2 => {
            dispatch(
              recvLogin({
                profile: user2.profile
              })
            );
            history.push("/");
          });
        })
        .catch(error => {
          dispatch(recvLogin(error));
        });
    } else {
      mgr.getUser().then(user => {
        if (user) {
          dispatch(
            recvLogin({
              profile: user.profile
            })
          );
        }
      });
    }
  };
};

export const sendLogin = (content: any) => {
  return (dispatch: any) => {
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
    setTimeout(() => dispatch(recvLogin("ok")), 4000);
  };
};

export const checkIdentity = () => {
  mgr.signinRedirect();
};

export const recvIdentity = () => {
  return (dispatch: any) => {
    mgr.signinRedirectCallback().then(() => {
      mgr.getUser().then(user => {
        dispatch(
          recvLogin({
            profile: user
          })
        );
      });
    });
  };
};

export const recvLogin = (content: any) => ({
  payload: {
    permissions: ["admin"],
    profile: content && content.profile
  },
  type: REVC_LOGIN
});

export const callIdentityApi = () => {
  return (dispatch: any) => {
    dispatch({
      payload: null,
      type: CALL_IDENTITY_API
    });
    mgr.getUser().then(user => {
      const url = "http://localhost:5100/api/home/SecretService";
      fetch(url, {
        headers: {
          Authorization: "Bearer " + user.access_token,
          method: "GET"
        }
      })
        .then(response => response.json())
        .then(data =>
          dispatch({
            payload: { data },
            type: RECV_IDENTITY_API
          })
        )
        .catch(err => {
          dispatch({
            payload: { message: err.message },
            type: RECV_IDENTITY_API
          });
        });
    });
  };
};

export const callCategoriesApi = () => {
  return (dispatch: any) => {
    dispatch({
      payload: null,
      type: CALL_API
    });
    const url = "http://192.168.1.100:5100/api/Categories/1/children/lite";
    fetch(url)
      .then(response => response.json())
      .then(data =>
        dispatch({
          payload: { data: data.items },
          type: RECV_API
        })
      )
      .catch(err => {
        dispatch({
          payload: { message: err.message },
          type: RECV_API
        });
      });
  };
};

export const callGatewayCategoriesApi = () => {
  return (dispatch: any) => {
    dispatch({
      payload: null,
      type: CALL_API
    });
    const url =
      "http://192.168.1.100:5010/clientservice/Categories/1/children/lite";
    fetch(url)
      .then(response => response.json())
      .then(data =>
        dispatch({
          payload: { data: data.items },
          type: RECV_API
        })
      )
      .catch(err => {
        dispatch({
          payload: { message: err.message },
          type: RECV_API
        });
      });
  };
};
