import * as React from 'react';
import { NavLink, Link } from 'react-router-dom';

export class NavMenu extends React.Component<{}, {}> {
  public render() {
    return (
      <div className="main-nav">
        <div className="navbar navbar-inverse">
          <div className="navbar-header">
            <button
              type="button"
              className="navbar-toggle"
              data-toggle="collapse"
              data-target=".navbar-collapse"
            >
              <span className="sr-only">Toggle navigation</span>
              <span className="icon-bar" />
              <span className="icon-bar" />
              <span className="icon-bar" />
            </button>
            <Link className="navbar-brand" to={'/'}>
              NetApp
            </Link>
          </div>
          <div className="clearfix" />
          <div className="navbar-collapse collapse">
            <ul className="nav navbar-nav">
              <li>
                <NavLink exact to={'/'} activeClassName="active">
                  <span className="glyphicon glyphicon-home" /> Home
                </NavLink>
              </li>
                        {
              //<li>
              //  <NavLink to={'/counter'} activeClassName="active">
              //    <span className="glyphicon glyphicon-education" /> Counter
              //  </NavLink>
              //</li>
              //<li>
              //  <NavLink to={'/fetchdata'} activeClassName="active">
              //    <span className="glyphicon glyphicon-th-list" /> Fetch data
              //  </NavLink>
              //</li>
                        }
              <li>
                <NavLink to={'/mylibrary'} activeClassName="active">
                  <span className="glyphicon glyphicon-folder-open" /> My Library
                </NavLink>
              </li>
              <li className="dropdown">
                <a
                  href="#"
                  className="dropdown-toggle"
                  data-toggle="dropdown"
                  role="button"
                  aria-haspopup="true"
                  aria-expanded="false"
                >
                  <span className="glyphicon glyphicon-user" /> 王培仲 <span className="caret" />
                </a>
                <ul className="dropdown-menu">
                  <li>
                    <a href="">Info</a>
                  </li>
                  <li>
                    <a href="Identity/Account/manage">Settings</a>
                  </li>
                  <li role="separator" className="divider" />
                  <li>
                    <button
                      type="submit"
                      onClick={() => {
                        const signout = fetch('home/LogOut', {
                          method: 'POST',
                          credentials: 'include'
                        }).then(r => (window.location.href = 'Identity/Account/Login'));
                      }}
                    >
                      Sign Out
                    </button>
                  </li>
                </ul>
              </li>
            </ul>
          </div>
        </div>
      </div>
    );
  }
}
