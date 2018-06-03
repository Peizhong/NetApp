import { fetch, addTask } from 'domain-task';
import { Action, Reducer, ActionCreator } from 'redux';
import { AppThunkAction } from './';
import { stat } from 'fs';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface Entry {
  id: number;
  title: string;
  text: string;
  link: string;
  topicid: number;
}

export interface Topic {
  id: number;
  name: string;
  ownerid: number;
}

export interface LearningLogsState {
  isLoading: boolean;
  message: string;
  ownerid: number;
  topics: Topic[];
  selectedtopic: number;
  entries: Entry[];
  selectedentry: number;
  entrydetail: Entry;
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

interface RequestTopicsAction {
  type: 'REQUEST_LEARNING_LOG_TOPICS';
  ownerid: number;
}

interface ReceiveTopicsAction {
  type: 'RECEIVE_LEARNING_LOG_TOPICS';
  ownerid: number;
  topics: Topic[];
  message: string;
}

interface SelectTopicAction {
  type: 'SELECT_LEARNING_LOG_TOPIC';
  selectedtopic: number;
}

interface RequestEntriesAction {
  type: 'REQUEST_TOPIC_ENTRIES';
  selectedtopic: number;
}

interface ReciveEntriesAction {
  type: 'RECEIVE_TOPIC_ENTRIES';
  topicid: number;
  entries: Entry[];
  message: string;
}

interface SelectEntryAction {
  type: 'SELECT_TOPIC_ENTRY';
  selectedentry: number;
}

interface RequestEntryDetailAction {
  type: 'REQUEST_ENTRIY_DETAIL';
  selectedentry: number;
}

interface ReciveEntryDetailAction {
  type: 'RECEIVE_ENTRIE_DETAIL';
  entrydetail: Entry;
  message: string;
}

interface PostEntryDetailAction {
  type: 'POST_ENTRY_DETAIL';
  entrydetail: Entry;
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
  | RequestEntriesAction
  | ReciveEntriesAction
  | SelectEntryAction
  | RequestEntryDetailAction
  | ReciveEntryDetailAction
  | PostEntryDetailAction
  | RecivePostEntryDetailAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
  requestTopics: (ownerid: number): AppThunkAction<KnownAction> => (dispatch, getState) => {
    if (ownerid !== getState().learningLogs.ownerid) {
      let fetchTask = fetch(`api/SampleData/UserTopics?userId=${ownerid}`)
        .then(response => response.json() as Promise<Topic[]>)
        .then(data => {
          dispatch({
            type: 'RECEIVE_LEARNING_LOG_TOPICS',
            topics: data,
            ownerid,
            message: '',
          });
        })
        .catch(err => console.log(err.message));
      addTask(fetchTask);
      dispatch({ type: 'REQUEST_LEARNING_LOG_TOPICS', ownerid: ownerid });
    }
  },
  requestEntries: (topicid: number): AppThunkAction<KnownAction> => (dispatch, getState) => {
    if (topicid !== getState().learningLogs.selectedtopic) {
      let fetchTask = fetch(`api/SampleData/UserTopicEnries?topicId=${topicid}`)
        .then(response => response.json() as Promise<Entry[]>)
        .then(data => {
          dispatch({
            type: 'RECEIVE_TOPIC_ENTRIES',
            entries: data,
            topicid,
            message: '',
          });
        })
        .catch(err => console.log(err.message));
      addTask(fetchTask);
      dispatch({ type: 'REQUEST_TOPIC_ENTRIES', selectedtopic: topicid });
    }
  },
  collapsTopic: (topicid: number) =>
    <SelectTopicAction>{ type: 'SELECT_LEARNING_LOG_TOPIC', selectedtopic: topicid },
  selectEntry: (entryid: number) =>
    <SelectEntryAction>{ type: 'SELECT_TOPIC_ENTRY', selectedentry: entryid },
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: LearningLogsState = {
  ownerid: -1,
  isLoading: false,
  message: '',
  topics: [],
  entries: [],
  selectedentry: 0,
  selectedtopic: 0,
  entrydetail: {
    id: 0,
    title: '',
    text: '',
    link: '',
    topicid: 0,
  },
};

export const reducer: Reducer<LearningLogsState> = (
  state: LearningLogsState,
  incomingAction: Action,
) => {
  const action = incomingAction as KnownAction;
  switch (action.type) {
    case 'REQUEST_LEARNING_LOG_TOPICS':
      return {
        ownerid: action.ownerid,
        isLoading: true,
        message: 'loading...',

        selectedtopic: state.selectedtopic,
        selectedentry: state.selectedentry,
        topics: state.topics,
        entries: state.entries,
        entrydetail: state.entrydetail,
      };
    case 'RECEIVE_LEARNING_LOG_TOPICS':
      // Only accept the incoming data if it matches the most recent request. This ensures we correctly
      // handle out-of-order responses.
      if (action.ownerid === state.ownerid) {
        return {
          isLoading: false,
          topics: action.topics,
          message: action.message,

          ownerid: state.ownerid,
          selectedtopic: state.selectedtopic,
          selectedentry: state.selectedentry,
          entries: state.entries,
          entrydetail: state.entrydetail,
        };
      }
      break;
    case 'SELECT_TOPIC_ENTRY':
      if (action.selectedentry !== state.selectedentry) {
        return {
          ownerid: state.ownerid,
          topics: state.topics,
          isLoading: state.isLoading,
          message: state.message,
          selectedtopic: state.selectedtopic,
          selectedentry: action.selectedentry,
          entries: state.entries,
          entrydetail: state.entries.find(e => e.id == action.selectedentry) || {
            id: 0,
            title: '',
            text: '',
            link: '',
            topicid: 0,
          },
        };
      }
      return {
        ownerid: state.ownerid,
        topics: state.topics,
        isLoading: state.isLoading,
        message: state.message,
        selectedtopic: state.selectedtopic,
        selectedentry: -1,
        entries: state.entries,
        entrydetail: {
          id: 0,
          title: '',
          text: '',
          link: '',
          topicid: 0,
        },
      };
    case 'REQUEST_TOPIC_ENTRIES':
      return {
        selectedtopic: action.selectedtopic,
        isLoading: true,
        message: 'loading...',

        ownerid: state.ownerid,
        selectedentry: state.selectedentry,
        topics: state.topics,
        entries: state.entries,
        entrydetail: state.entrydetail,
      };
    case 'RECEIVE_TOPIC_ENTRIES':
      if (action.topicid === state.selectedtopic) {
        return {
          isLoading: false,
          entries: action.entries,
          message: action.message,

          ownerid: state.ownerid,
          topics: state.topics,
          selectedtopic: state.selectedtopic,
          selectedentry: state.selectedentry,
          entrydetail: state.entrydetail,
        };
      }
      break;
    case 'SELECT_LEARNING_LOG_TOPIC':
      if (action.selectedtopic == state.selectedtopic) {
        return {
          isLoading: false,
          ownerid: state.ownerid,
          topics: state.topics,
          entries: [],
          message: state.message,
          selectedtopic: -1,
          selectedentry: state.selectedentry,
          entrydetail: state.entrydetail,
        };
      }
      break;
    case 'REQUEST_ENTRIY_DETAIL':
    case 'RECEIVE_ENTRIE_DETAIL':
    case 'POST_ENTRY_DETAIL':
    case 'RECEIVE_POST_ENTRIE_DETAIL':
      return {
        isLoading: false,
        message: '',

        ownerid: state.ownerid,
        topics: state.topics,
        entries: state.entries,
        selectedtopic: state.selectedtopic,
        selectedentry: state.selectedentry,
        entrydetail: state.entrydetail,
      };
    default:
      // The following line guarantees that every action in the KnownAction union has been covered by a case above
      const exhaustiveCheck: never = action;
      break;
  }

  return state || unloadedState;
};
