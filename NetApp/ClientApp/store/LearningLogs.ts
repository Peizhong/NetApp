import { fetch, addTask } from 'domain-task';
import { Action, Reducer, ActionCreator } from 'redux';
import { AppThunkAction } from './';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.
export interface UserInfo {
  id: string;
  userName: string;
}

export interface EntryHeader {
  id: number;
  title: string;
}

export interface Entry extends EntryHeader {
  text: string;
  link: string;
  updateTime: Date;
  topicId: number;
}

export interface TopicHeader {
  id: number;
  name: string;
  ownerId: number;
}

export interface Topic extends TopicHeader {
  updateTime: Date;
  entryHeaders: EntryHeader[];
}

export interface LearningLogsState {
  isLoading: boolean;
  message: string;
  ownerId: number;
  topics: TopicHeader[];
  selectedTopic?: Topic;
  topicId: number;
  selectedEntry?: Entry;
  entryId: number;
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

interface RequestUserInfoAtion {
  type: 'REQUEST_USER_INFO';
}

interface ReciveUserInfoAction {
  type: 'RECEIVE_USER_INFO';
  user: UserInfo;
}

interface RequestTopicsAction {
  type: 'REQUEST_LEARNING_LOG_TOPICS';
}

interface ReceiveTopicsAction {
  type: 'RECEIVE_LEARNING_LOG_TOPICS';
  ownerId: number;
  topics: TopicHeader[];
  message: string;
}

interface SelectTopicAction {
  type: 'SELECT_LEARNING_LOG_TOPIC';
  topicId: number;
}

interface RequestTopicDetailAction {
  type: 'REQUEST_TOPIC_DETAIL';
  topicId: number;
}

interface ReciveTopicDetailAction {
  type: 'RECEIVE_TOPIC_DETAIL';
  topic: Topic;
  message: string;
}

interface SelectEntryAction {
  type: 'SELECT_TOPIC_ENTRY';
  entryId: number;
}

interface RequestEntryDetailAction {
  type: 'REQUEST_ENTRIY_DETAIL';
  entryId: number;
}

interface ReciveEntryDetailAction {
  type: 'RECEIVE_ENTRIE_DETAIL';
  entry: Entry;
  message: string;
}

interface EditEntryDetailAction {
  type: 'EDIT_ENTRY_DETAIL';
  entryId: number;
  field: string;
  value: string;
}

interface PostEntryDetailAction {
  type: 'POST_ENTRY_DETAIL';
  entryId: number;
}

interface RecivePostEntryDetailAction {
  type: 'RECEIVE_POST_ENTRIE_DETAIL';
  message: string;
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction =
  | RequestTopicsAction
  | ReceiveTopicsAction
  | SelectTopicAction
  | RequestTopicDetailAction
  | ReciveTopicDetailAction
  | SelectEntryAction
  | RequestEntryDetailAction
  | ReciveEntryDetailAction
  | EditEntryDetailAction
  | PostEntryDetailAction
  | RecivePostEntryDetailAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
  //when mount compoment, or user changed
  requestTopics: (): AppThunkAction<KnownAction> => (dispatch, getState) => {
    let fetchTask = fetch('api/LearningLog/Topics', {
      method: 'GET',
      credentials: 'include'
    })
      .then(response => response.json() as Promise<TopicHeader[]>)
      .then(data => {
        dispatch({
          type: 'RECEIVE_LEARNING_LOG_TOPICS',
          topics: data,
          ownerId: data.length > 0 ? data[0].ownerId : -1,
          message: ''
        });
      })
      .catch(err => {
        dispatch({
          type: 'RECEIVE_LEARNING_LOG_TOPICS',
          topics: [],
          ownerId: -1,
          message: err.message
        });
      });
    addTask(fetchTask);
    dispatch({ type: 'REQUEST_LEARNING_LOG_TOPICS' });
  },
  //when click on topic header, load topic's entries or collaps topic
  selectTopic: (topicId: number): AppThunkAction<KnownAction> => (dispatch, getState) => {
    if (topicId !== getState().learningLogs.topicId) {
      let fetchTask = fetch(`api/LearningLog/Topic/${topicId}`, {
        method: 'GET',
        credentials: 'include'
      })
        .then(response => response.json() as Promise<Topic>)
        .then(data => {
          dispatch({
            type: 'RECEIVE_TOPIC_DETAIL',
            topic: data,
            message: ''
          });
        })
        .catch(err => console.log(err.message));
      addTask(fetchTask);
      dispatch({ type: 'REQUEST_TOPIC_DETAIL', topicId });
    } else {
      dispatch({ type: 'SELECT_LEARNING_LOG_TOPIC', topicId });
    }
  },
  selectEntry: (entryId: number): AppThunkAction<KnownAction> => (dispatch, getState) => {
    if (entryId !== getState().learningLogs.entryId) {
      let fetchTask = fetch(`api/LearningLog/Entry/${entryId}`, {
        method: 'GET',
        credentials: 'include'
      })
        .then(response => response.json() as Promise<Entry>)
        .then(data => {
          dispatch({
            type: 'RECEIVE_ENTRIE_DETAIL',
            entry: data,
            message: ''
          });
        })
        .catch(err => console.log(err.message));
      addTask(fetchTask);
      dispatch({ type: 'REQUEST_ENTRIY_DETAIL', entryId });
    } else {
      dispatch({ type: 'SELECT_TOPIC_ENTRY', entryId });
    }
  },
  editedEntry: (entryId: number, field: string, value: string) =>
    <EditEntryDetailAction>{ type: 'EDIT_ENTRY_DETAIL', entryId, field, value },
  saveEntry: (entryId: number): AppThunkAction<KnownAction> => (dispatch, getState) => {
    const currentEntry = getState().learningLogs.selectedEntry;
    if (currentEntry && entryId == currentEntry.id) {
      let postTask = fetch('api/LearningLog/Entry', {
        method: 'POST',
        credentials: 'include',
        headers: new Headers({
          'Content-Type': 'application/json'
        }),
        body: JSON.stringify(currentEntry)
      })
        .then(response => response.json() as Promise<EntryHeader>)
        .then(data =>
          dispatch({
            type: 'RECEIVE_POST_ENTRIE_DETAIL',
            message: data.id > 0 ? '保存成功' : '保存失败'
          })
        )
        .catch(err => console.log(err.message));
      addTask(postTask);
      dispatch({ type: 'POST_ENTRY_DETAIL', entryId });
    }
  }
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: LearningLogsState = {
  ownerId: -1,
  isLoading: false,
  message: '',
  topics: [],
  topicId: -1,
  entryId: -1
};

export const reducer: Reducer<LearningLogsState> = (
  state: LearningLogsState,
  incomingAction: Action
) => {
  const action = incomingAction as KnownAction;
  switch (action.type) {
    case 'REQUEST_LEARNING_LOG_TOPICS':
      return {
        isLoading: true,
        message: 'loading...',

        //loading new owner info, clear old data
        ownerId: -1,
        topics: [],
        topicId: -1,
        entryId: -1
        //selectedTopic: state.selectedTopic,
        //selectedEntry: state.selectedEntry,
      };
    case 'RECEIVE_LEARNING_LOG_TOPICS':
      // Only accept the incoming data if it matches the most recent request. This ensures we correctly
      // handle out-of-order responses.
      return {
        isLoading: false,
        message: action.message,
        topics: action.topics,
        ownerId: action.ownerId,

        topicId: -1,
        entryId: -1
        //when recived new topics, selected topic and entry still unset
        //selectedtopic: state.selectedtopic,
        //selectedentry: state.selectedentry,
      };
    case 'SELECT_LEARNING_LOG_TOPIC':
      //wait for recive_topic_detail to update, or just hide
      return {
        //if click topic twice, collaps topic
        topicId: state.topicId === action.topicId ? -1 : action.topicId,

        ownerId: state.ownerId,
        isLoading: state.isLoading,
        message: state.message,
        topics: state.topics,

        //when update topic state, change entry info anyway
        entryId: -1
      };
    case 'REQUEST_TOPIC_DETAIL':
      return {
        isLoading: true,
        message: 'loading...',
        topicId: action.topicId,

        ownerId: state.ownerId,
        topics: state.topics,
        //loading new topic info, clear old data
        entryId: -1
        //selectedTopic: state.selectedTopic,
        //selectedEntry: state.selectedEntry,
      };
    case 'RECEIVE_TOPIC_DETAIL':
      if (action.topic.id === state.topicId) {
        return {
          isLoading: false,
          message: action.message,
          selectedTopic: action.topic,

          ownerId: state.ownerId,
          topics: state.topics,
          topicId: state.topicId,
          entryId: -1
        };
      }
      break;
    case 'SELECT_TOPIC_ENTRY':
      //no local entry data, wait for recive_entry to update, or just hide
      return {
        entryId: state.entryId === action.entryId ? -1 : action.entryId,

        ownerId: state.ownerId,
        isLoading: state.isLoading,
        message: state.message,
        topics: state.topics,
        topicId: state.topicId,
        selectedTopic: state.selectedTopic
      };
    case 'REQUEST_ENTRIY_DETAIL': {
      return {
        isLoading: true,
        message: 'loading...',
        entryId: action.entryId,

        topicId: state.topicId,
        ownerId: state.ownerId,
        topics: state.topics,
        selectedTopic: state.selectedTopic
        //loading new topic info, clear old data
        //selectedTopic: state.selectedTopic,
        //selectedEntry: state.selectedEntry,
      };
    }
    case 'RECEIVE_ENTRIE_DETAIL': {
      if (action.entry.id === state.entryId) {
        return {
          isLoading: false,
          message: action.message,
          selectedEntry: action.entry,

          ownerId: state.ownerId,
          topics: state.topics,
          topicId: state.topicId,
          entryId: state.entryId,
          selectedTopic: state.selectedTopic
        };
      }
      break;
    }
    case 'EDIT_ENTRY_DETAIL':
      if (!state.selectedEntry || state.selectedEntry.id !== action.entryId) break;
      const editedEntry = <Entry>{
        id: state.selectedEntry.id,
        title: state.selectedEntry.title,
        text: state.selectedEntry.text,
        link: state.selectedEntry.link,
        updateTime: state.selectedEntry.updateTime,
        topicId: state.selectedEntry.topicId
      };
      switch (action.field) {
        case 'text':
          editedEntry.text = action.value;
          break;
        case 'link':
          editedEntry.link = action.value;
          break;
        default:
          break;
      }
      return {
        isLoading: false,
        selectedEntry: editedEntry,

        message: state.message,
        ownerId: state.ownerId,
        topics: state.topics,
        topicId: state.topicId,
        entryId: state.entryId,
        selectedTopic: state.selectedTopic
      };
    case 'POST_ENTRY_DETAIL':
    case 'RECEIVE_POST_ENTRIE_DETAIL':
      return {
        isLoading: false,
        message: '',
        ownerId: state.ownerId,
        topics: state.topics,
        topicId: state.topicId,
        entryId: state.entryId,
        selectedEntry: state.selectedEntry,
        selectedTopic: state.selectedTopic
      };
    default:
      // The following line guarantees that every action in the KnownAction union has been covered by a case above
      const exhaustiveCheck: never = action;
      break;
  }

  return state || unloadedState;
};
