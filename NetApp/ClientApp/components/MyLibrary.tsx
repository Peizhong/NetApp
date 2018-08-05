import * as React from "react";
import { Link, RouteComponentProps } from "react-router-dom";
import { connect } from "react-redux";
import { ApplicationState } from "../store";
import * as LearningLogsState from "../store/LearningLogs";
import TopicForm from "./TopicForm";
import EntryForm from "./EntryForm";

type MyLibarayProps = LearningLogsState.LearningLogsState &
  typeof LearningLogsState.actionCreators &
  RouteComponentProps<{ ownerid: number }>;

class MyLibaray extends React.Component<MyLibarayProps, {}> {
  componentWillMount() {
    this.props.requestTopics();
  }

  public render() {
    return (
      <div>
        <nav className="navbar" role="search">
          <form className="navbar-form navbar-right">
            <div className="form-group">
              <input
                type="text"
                className="form-control"
                placeholder="Search"
              />
            </div>
            <button type="button" className="btn btn-default">
              Submit
            </button>
          </form>
        </nav>
        {this.props.isLoading && (
          <div className="progress">
            <div
              className="progress-bar progress-bar-info progress-bar-striped active"
              style={{ width: "100%" }}
            />
          </div>
            )}
        请退出默认账号，使用账号test@test.com test1234登陆
        {this.renderTopics()}
        <button className="btn btn-primary pull-left">Look Good</button>
      </div>
    );
  }

  private renderTopics() {
    return (
      <div className="list-group">
        {this.props.topics.map(
          topic =>
            topic.id === this.props.topicId ? (
              this.expandTopic(topic)
            ) : (
                <button
                  className="list-group-item"
                  key={topic.id}
                  onClick={() => {
                    this.props.selectTopic(topic.id);
                  }}
                >
                  <h5>{topic.name}</h5>
                </button>
              )
        )}
      </div>
    );
  }

  private expandTopic(topic: LearningLogsState.Topic) {
    const { entries, editedTopic, saveTopic, entryId } = this.props;
    return (
      <div className="panel panel-primary">
        <div
          className="panel-heading"
          onClick={() => this.props.selectTopic(topic.id)}
        >
          <h3 className="panel-title">
            {topic.name}
          </h3>
        </div>
        <TopicForm
          selectedTopic={topic}
          editedTopic={editedTopic}
          saveTopic={saveTopic}
        />
        <div className="well well-sm">
          {topic.id > 0 && entries.map(
            entry =>
              entry.id === entryId ? (
                this.showEntry(entry)
              ) : (
                  <button
                    className="list-group-item"
                    key={entry.id}
                    onClick={() => {
                      this.props.selectEntry(entry.id);
                    }}
                  >
                    {entry.title}
                  </button>
                )
          )}
        </div>
      </div>
    );
  }

  private showEntry(entry: LearningLogsState.Entry) {
    const { entryId, editedEntry, saveEntry } = this.props;
    return (
      <div className="panel panel-info">
        <div
          className="panel-heading"
          onClick={() => this.props.selectEntry(entry.id)}
        >
          <h4 className="panel-title">
            {entry.title}
          </h4>
        </div>
        {entry.id == entryId && (
          <EntryForm
            selectedEntry={entry}
            editedEntry={editedEntry}
            saveEntry={saveEntry}
          />
        )}
      </div>
    );
  }
}

export default connect(
  (state: ApplicationState) => state.learningLogs,
  LearningLogsState.actionCreators
)(MyLibaray) as typeof MyLibaray;
