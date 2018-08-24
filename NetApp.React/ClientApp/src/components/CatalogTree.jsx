import React from 'react';
import { connect } from 'react-redux';
import { Tree } from 'antd';

const TreeNode = Tree.TreeNode;

const mapStateToProps = state => ({
  todos: state.trees,
  isLoading: state.isLoading
});

const toggleTodo = x => x + 1;

const mapDispatchToProps = dispatch => ({
  onTodoClick: id => {
    dispatch(toggleTodo(id));
  }
});

const treeData = [
  {
    title: 'Node1',
    value: '0-0',
    key: '0-0',
    children: [
      {
        title: 'Child Node1',
        value: '0-0-1',
        key: '0-0-1'
      },
      {
        title: 'Child Node2',
        value: '0-0-2',
        key: '0-0-2'
      }
    ]
  },
  {
    title: 'Node2',
    value: '0-1',
    key: '0-1'
  }
];

class CatalogTree extends React.Component {
  renderTreeNodes = data => {
    return data.map(item => {
      if (item.children) {
        return (
          <TreeNode title={item.title} key={item.key} dataRef={item}>
            {this.renderTreeNodes(item.children)}
          </TreeNode>
        );
      }
      return <TreeNode {...item} dataRef={item} />;
    });
  };

  render() {
    return (
      <Tree
        defaultExpandedKeys={['0-0-0', '0-0-1']}
        defaultSelectedKeys={['0-0-0', '0-0-1']}
        defaultCheckedKeys={['0-0-0', '0-0-1']}
        onSelect={this.onSelect}
        onCheck={this.onCheck}
      >
        {this.renderTreeNodes(treeData)}
      </Tree>
    );
  }
}

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(CatalogTree);
