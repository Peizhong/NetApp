const requestChildTreeType = "REQUEST_CHILD_TREE";
const receiveChildTreeType = "RECEIVE_CHILD_TREE";

const initialState = { trees: [], isLoading: false };

export const actionCreators = {
  requestChildTree: selectedId => async (dispatch, getState) => {
    if (selectedId === getState().categoryTree.selectedId) {
      // Don't issue a duplicate request (we already have or are loading the requested data)
      return;
    }

    dispatch({ type: requestChildTreeType, selectedId });

    const url = `Categories/${selectedId}/children/lite`;
    const response = await fetch(url);
    const raw = await response.json();
    const trees = raw.items;

    dispatch({ type: receiveChildTreeType, selectedId, trees });
  }
};

export const reducer = (state, action) => {
  state = state || initialState;

  if (action.type === requestChildTreeType) {
    return {
      ...state,
      selectedId: action.selectedId,
      isLoading: true
    };
  }

  if (action.type === receiveChildTreeType) {
    if (state.selectedId === action.selectedId) {
      let newTree = state.trees || [];
      if (newTree.length > 0) {
        const findSelectNode = (nodes, id) => {
          const len = nodes.length;
          for (let i = 0; i < len; i += 1) {
            if (nodes[i].id === id) {
              return nodes[i];
            }
            if (nodes[i].children && nodes[i].children.length > 0) {
              const child = findSelectNode(nodes[i].children, id);
              if (child) return child;
            }
          }
          return null;
        };
        const selectedNode = findSelectNode(newTree, action.selectedId);
        if (selectedNode) {
          selectedNode.children = action.trees;
        }
      } else {
        newTree = action.trees;
      }
      return {
        ...state,
        selectedId: action.selectedId,
        trees: newTree,
        isLoading: false
      };
    }
  }

  return state;
};
