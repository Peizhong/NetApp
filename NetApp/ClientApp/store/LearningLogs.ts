import { fetch, addTask } from "domain-task";
import { Action, Reducer, ActionCreator } from "redux";
import { AppThunkAction } from "./";

// -----------------
// STATE - This defines the type of data maintained in the Redux store.
export interface UserInfo {
  id: string;
  userName: string;
}

export interface Entry {
  id: number;
  title: string;
  text: string;
  link: string;
  topicId: number;
}

export interface Topic {
  id: number;
  name: string;
  ownerId: string;
}

export interface LearningLogsState {
  isLoading: boolean;
  message: string;
  ownerId: number;
  topics: Topic[];
  entries: Entry[];
  topicId: number;
  entryId: number;
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

interface RequestUserInfoAtion {
  type: "REQUEST_USER_INFO";
}

interface ReciveUserInfoAction {
  type: "RECEIVE_USER_INFO";
  user: UserInfo;
}

interface RequestTopicsAction {
  type: "REQUEST_LEARNING_LOG_TOPICS";
}

interface ReceiveTopicsAction {
  type: "RECEIVE_LEARNING_LOG_TOPICS";
  topics: Topic[];
  message: string;
}

interface SelectTopicAction {
  type: "SELECT_LEARNING_LOG_TOPIC";
  topicId: number;
}

interface RequestTopicEntriesAction {
  type: "REQUEST_TOPIC_ENTRIES";
  topicId: number;
}

interface ReciveTopicEntriesAction {
  type: "RECEIVE_TOPIC_ENTRIES";
  topicId: number;
  entrise: Entry[];
  message: string;
}

interface SelectEntryAction {
  type: "SELECT_TOPIC_ENTRY";
  entryId: number;
}

interface EditEntryDetailAction {
  type: "EDIT_ENTRY_DETAIL";
  entryId: number;
  field: string;
  value: string;
}

interface PostEntryDetailAction {
  type: "POST_ENTRY_DETAIL";
  entryId: number;
}

interface RecivePostEntryDetailAction {
  type: "RECEIVE_POST_ENTRIE_DETAIL";
  message: string;
  entries: Entry[];
}

interface EditTopicDetailAction {
  type: "EDIT_TOPIC_DETAIL";
  topicId: number;
  field: string;
  value: string;
}

interface PostTopicDetailAction {
  type: "POST_TOPIC_DETAIL";
  topicId: number;
}

interface RecivePostTopicDetailAction {
  type: "RECEIVE_POST_TOPIC_DETAIL";
  topics: Topic[];
  message: string;
}

interface DeleteTopicAction {
  type: "DELETE_TOPIC";
  topicId: number;
}

interface DeleteEntryAction {
  type: "DELETE_ENTRY";
  entryId: number;
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction =
  //load topics list
  | RequestTopicsAction
  | ReceiveTopicsAction
  //click on topic, load topic entries
  | SelectTopicAction
  | RequestTopicEntriesAction
  | ReciveTopicEntriesAction
  //click on entry, show entry form
  | SelectEntryAction
  | EditEntryDetailAction
  | PostEntryDetailAction
  | RecivePostEntryDetailAction
  | EditTopicDetailAction
  | PostTopicDetailAction
  | RecivePostTopicDetailAction


function AddNewTopic(topics: Topic[]) {
  let currentTopic = [...topics]
  if (!currentTopic.find(t => t.id === 0)) {
    const newTopic: Topic = {
      id: 0,
      name: "新建主题",
      ownerId: "0"
    };
    currentTopic.push(newTopic);
  }
  return currentTopic;
}

function AddNewEntry(entry: Entry[], topicId: number) {
  let currentEntry = [...entry];
  if (!currentEntry.find(e => e.id === 0)) {
    const newEntry: Entry = {
      id: 0,
      title: "新建文章",
      text: '',
      link: '',
      topicId
    };
    currentEntry.push(newEntry);
  }
  return currentEntry;
}

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
  //when mount compoment, or user changed
  requestTopics: (): AppThunkAction<KnownAction> => (dispatch, getState) => {
    let fetchTask = fetch("api/LearningLog/Topics", {
      method: "GET",
      credentials: "include"
    })
      .then(response => response.json() as Promise<Topic[]>)
      .then(data => {
        dispatch({
          type: "RECEIVE_LEARNING_LOG_TOPICS",
          topics: data,
          message: ""
        });
      })
      .catch(err => {
        dispatch({
          type: "RECEIVE_LEARNING_LOG_TOPICS",
          topics: [],
          message: err.message
        });
      });
    addTask(fetchTask);
    dispatch({ type: "REQUEST_LEARNING_LOG_TOPICS" });
  },
  //when click on topic header, load topic's entries or collaps topic
  selectTopic: (topicId: number): AppThunkAction<KnownAction> => (
    dispatch,
    getState
  ) => {
    if (topicId !== getState().learningLogs.topicId) {
      let fetchTask = fetch(`api/LearningLog/TopicEntries/${topicId}`, {
        method: "GET",
        credentials: "include"
      })
        .then(response => response.json() as Promise<Entry[]>)
        .then(data => {
          dispatch({
            type: "RECEIVE_TOPIC_ENTRIES",
            topicId,
            entrise: data,
            message: ""
          });
        })
        .catch(err => console.log(err.message));
      addTask(fetchTask);
      dispatch({ type: "REQUEST_TOPIC_ENTRIES", topicId });
    } else {
      dispatch({ type: "SELECT_LEARNING_LOG_TOPIC", topicId });
    }
  },
  selectEntry: (entryId: number): AppThunkAction<KnownAction> => (
    dispatch,
    getState
  ) => {
    const currentEntryId = getState().learningLogs.entryId;
    dispatch({
      type: 'SELECT_TOPIC_ENTRY',
      entryId: currentEntryId === entryId ? -1 : entryId
    });
  },
  editedEntry: (entryId: number, field: string, value: string) =>
    <EditEntryDetailAction>{ type: "EDIT_ENTRY_DETAIL", entryId, field, value },
  saveEntry: (actionId: number): AppThunkAction<KnownAction> => (
    dispatch,
    getState
  ) => {
    const { entries, entryId } = getState().learningLogs;
    if (actionId !== entryId)
      return;
    const currentEntry = entries.find(e => e.id === entryId);
    if (!currentEntry)
      return;
    let postTask = fetch("api/LearningLog/Entry", {
      method: "POST",
      credentials: "include",
      headers: new Headers({
        "Content-Type": "application/json"
      }),
      body: JSON.stringify(currentEntry)
    })
      .then(response => response.json() as Promise<Entry[]>)
      .then(data =>
        dispatch({
          type: "RECEIVE_POST_ENTRIE_DETAIL",
          message: "保存成功",
          entries: data
        })
      )
      .catch(err => console.log(err.message));
    addTask(postTask);
    dispatch({ type: "POST_ENTRY_DETAIL", entryId });
  },
  editedTopic: (topicId: number, field: string, value: string) =>
    <EditTopicDetailAction>{ type: "EDIT_TOPIC_DETAIL", topicId, field, value },
  saveTopic: (actionId: number): AppThunkAction<KnownAction> => (
    dispatch,
    getState
  ) => {
    const { topics, topicId } = getState().learningLogs;
    if (actionId !== topicId)
      return;
    const currentTopic = topics.find(t => t.id === topicId);
    if (!currentTopic)
      return;
    let postTask = fetch("api/LearningLog/Topic", {
      method: "POST",
      credentials: "include",
      headers: new Headers({
        "Content-Type": "application/json"
      }),
      body: JSON.stringify(currentTopic)
    })
      .then(response => response.json() as Promise<Topic[]>)
      .then(data =>
        dispatch({
          type: "RECEIVE_POST_TOPIC_DETAIL",
          message: "保存成功",
          topics: data
        })
      )
      .catch(err => console.log(err.message));
    addTask(postTask);
    dispatch({ type: "POST_TOPIC_DETAIL", topicId });
  },
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: LearningLogsState = {
  ownerId: -1,
  isLoading: false,
  message: "",
  topics: [],
  entries: [],
  topicId: -1,
  entryId: -1
};

export const reducer: Reducer<LearningLogsState> = (
  state: LearningLogsState,
  incomingAction: Action
) => {
  const action = incomingAction as KnownAction;
  switch (action.type) {
    case "REQUEST_LEARNING_LOG_TOPICS":
      return {
        isLoading: true,
        message: "loading...",

        //loading new owner info, clear old data
        ownerId: state.ownerId,
        topics: [],
        entries: [],
        topicId: -1,
        entryId: -1
      };
    case "RECEIVE_LEARNING_LOG_TOPICS":
      // Only accept the incoming data if it matches the most recent request. This ensures we correctly
      // handle out-of-order responses.
      let fixTopics = AddNewTopic(action.topics)
      return {
        isLoading: false,
        message: action.message,
        topics: fixTopics,

        ownerId: state.ownerId,
        entries: [],
        topicId: -1,
        entryId: -1,
        //when recived new topics, selected topic and entry still unset
        //selectedtopic: state.selectedtopic,
        //selectedentry: state.selectedentry,
      };
    case "SELECT_LEARNING_LOG_TOPIC":
      //wait for recive_topic_detail to update, or just hide
      return {
        //if click topic twice, collaps topic
        topicId: state.topicId === action.topicId ? -1 : action.topicId,

        ownerId: state.ownerId,
        isLoading: state.isLoading,
        message: state.message,
        topics: state.topics,
        entries: [],
        //when update topic state, change entry info anyway
        entryId: -1
      };
    case "REQUEST_TOPIC_ENTRIES":
      return {
        isLoading: true,
        message: "loading...",
        topicId: action.topicId,

        ownerId: state.ownerId,
        topics: state.topics,
        entryId: -1,
        entries: [],
      }

    case "RECEIVE_TOPIC_ENTRIES":
      if (action.topicId == state.topicId) {
        return {
          isLoading: false,
          message: '',
          entries: AddNewEntry(action.entrise, action.topicId),
          topicId: action.topicId,

          ownerId: state.ownerId,
          topics: state.topics,
          entryId: state.entryId,
        }
      }
      break;
    case "SELECT_TOPIC_ENTRY":
      //no local entry data, wait for recive_entry to update, or just hide
      return {
        entryId: action.entryId,

        ownerId: state.ownerId,
        isLoading: state.isLoading,
        message: state.message,
        topics: state.topics,
        topicId: state.topicId,
        entries: state.entries,
      };
    case "EDIT_ENTRY_DETAIL":
      if (state.entryId != action.entryId)
        break;
      const currentEntry = state.entries.find(e => e.id === action.entryId);
      if (!currentEntry)
        break;
      const editedEntry = <Entry>{
        id: currentEntry.id,
        title: currentEntry.title,
        text: currentEntry.text,
        link: currentEntry.link,
        topicId: currentEntry.topicId
      };
      switch (action.field) {
        case "title":
          editedEntry.title = action.value;
          break;
        case "text":
          editedEntry.text = action.value;
          break;
        case "link":
          editedEntry.link = action.value;
          break;
        default:
          break;
      }
      return {
        isLoading: false,
        entryId: action.entryId,
        entries: state.entries.map(e => e.id === action.entryId ? editedEntry : e),

        message: state.message,
        ownerId: state.ownerId,
        topics: state.topics,
        topicId: state.topicId,
      };
    case "POST_ENTRY_DETAIL":
      return {
        isLoading: true,
        message: "正在保存",
        ownerId: state.ownerId,
        topics: state.topics,
        entries: state.entries,
        topicId: state.topicId,
        entryId: state.entryId,
      };
    case "RECEIVE_POST_ENTRIE_DETAIL":
      return {
        isLoading: false,
        message: "",
        ownerId: state.ownerId,
        topicId: state.topicId,
        entryId: -1,
        topics: state.topics,
        entries: AddNewEntry(action.entries, state.topicId),
      };
    case "EDIT_TOPIC_DETAIL":
      if (state.topicId !== action.topicId)
        break;
      const currentTopic = state.topics.find(t => t.id === action.topicId);
      if (!currentTopic)
        break;
      const editedTopic = <Topic>{
        id: currentTopic.id,
        name: currentTopic.name,
        ownerId: currentTopic.ownerId
      };
      switch (action.field) {
        case "name":
          editedTopic.name = action.value;
          break;
        default:
          break;
      }
      return {
        topicId: action.topicId,
        topics: state.topics.map(t => t.id === action.topicId ? editedTopic : t),

        isLoading: false,
        message: state.message,
        ownerId: state.ownerId,
        entries: state.entries,
        entryId: state.entryId,
      };
    case "POST_TOPIC_DETAIL":
      return {
        isLoading: true,
        message: "正在保存",
        ownerId: state.ownerId,
        topics: state.topics,
        entries: state.entries,
        topicId: state.topicId,
        entryId: state.entryId,
      };
    case "RECEIVE_POST_TOPIC_DETAIL":
      return {
        isLoading: false,
        message: "",
        topics: AddNewTopic(action.topics),
        topicId: -1,

        ownerId: state.ownerId,
        entryId: state.entryId,
        entries: state.entries,
      };
    default:
      // The following line guarantees that every action in the KnownAction union has been covered by a case above
      const exhaustiveCheck: never = action;
      break;
  }

  return state || unloadedState;
};
