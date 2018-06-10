import * as React from "react";
import * as LearningLogsState from "../store/LearningLogs";

interface states {
  selectedTopic?: LearningLogsState.Topic;
}

const actions = {
  editedTopic: LearningLogsState.actionCreators.editedTopic,
  saveTopic: LearningLogsState.actionCreators.saveTopic
};

type TopicFormProps = states & typeof actions;

class TopicForm extends React.Component<TopicFormProps, {}> {
  public render() {
    const { selectedTopic, editedTopic, saveTopic } = this.props;
    if (!selectedTopic) return <div />;
    const { id, name } = selectedTopic;
    return (
      <form className="form-horizontal">
        <div className="form-group">
          <label className="col-sm-2 control-label">Name</label>
          <div className="col-sm-10">
            <input
              className="form-control"
              type="text"
              value={name}
              onChange={v => editedTopic(id, "name", v.target.value)}
            />
          </div>
        </div>
        <div className="form-group">
          <div className="col-sm-offset-2 col-sm-10">
            <div className="btn-group" role="group">
              <button
                type="button"
                className="btn btn-primary"
                onClick={() => saveTopic(id)}
              >
                Update
              </button>
              <button type="submit" className="btn btn-danger">
                Delete
              </button>
            </div>
          </div>
        </div>
      </form>
    );
  }
}

export default TopicForm;
