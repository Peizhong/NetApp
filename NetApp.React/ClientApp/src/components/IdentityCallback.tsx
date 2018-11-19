import * as React from "react";
import { connect } from "react-redux";

export interface IProps {
  recvIdentity: () => void;
  profile: any;
}

class IdentityCallback extends React.Component<IProps> {
  public render() {
    const {profile} = this.props
    let name = 'jon doe'
    if(profile){
      name = profile.name
    }
    return <div>Tell me about {name}</div>;
  }
}

const mapStateToProps = (state: any) => ({
  profile: state.account.profile
});

export default connect(mapStateToProps)(IdentityCallback);
