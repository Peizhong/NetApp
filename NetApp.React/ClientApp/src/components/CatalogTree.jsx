import React, { Component } from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { Tree } from 'antd';
import { actionCreators } from '../store/categoryTree';

const TreeNode = Tree.TreeNode;

class CatalogTree extends Component {
  renderTreeNodes = data => {
    return data.map(item => {
      if (item.children) {
        return (
          <TreeNode title={item.name} key={item.id} dataRef={item}>
            {this.renderTreeNodes(item.children)}
          </TreeNode>
        );
      }
      return <TreeNode title={item.name} key={item.id} dataRef={item} />;
    });
  };

  onLoadData = treeNode => {
    return new Promise(resolve => {
      if (treeNode) {
        this.props.requestChildTree(treeNode.props.eventKey);
      }
      resolve();
      return;
    });
  };

  componentWillMount() {
    // This method runs when the component is first added to the page
    this.props.requestChildTree(-1);
  }

  render() {
    const trees = this.props.trees || [];
    return <Tree loadData={this.onLoadData}>{this.renderTreeNodes(trees)}</Tree>;
  }
}

export default connect(
  state => state.categoryTree,
  dispatch => bindActionCreators(actionCreators, dispatch),
)(CatalogTree);
