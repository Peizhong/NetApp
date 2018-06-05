import * as React from 'react';
import { Link, RouteComponentProps } from 'react-router-dom';
import { connect } from 'react-redux';
import { ApplicationState } from '../store';
import * as LearningLogsState from '../store/LearningLogs';

type MyLibarayProps = LearningLogsState.LearningLogsState &
  typeof LearningLogsState.actionCreators &
  RouteComponentProps<{ ownerid: number }>;

class MyLibaray extends React.Component<MyLibarayProps, {}> {
  componentWillMount() {
    let ownerid = this.props.match.params.ownerid || 1;
    this.props.requestTopics(ownerid);
  }

  componentWillReceiveProps(nextProps: MyLibarayProps) {
    let ownerid = this.props.match.params.ownerid || 1;
    this.props.requestTopics(ownerid);
  }

  public render() {
    return (
      <div>
        <nav className="navbar" role="search">
          <form className="navbar-form navbar-right">
            <div className="form-group">
              <input type="text" className="form-control" placeholder="Search" />
            </div>
            <button type="submit" className="btn btn-default">
              Submit
            </button>
          </form>
        </nav>
        {false &&
          this.props.isLoading && (
            <div className="progress">
              <div
                className="progress-bar progress-bar-info progress-bar-striped active"
                style={{ width: '100%' }}
              />
            </div>
          )}
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

  private expandTopic(topic: LearningLogsState.TopicHeader) {
    return (
      <div className="panel panel-primary">
        <div className="panel-heading" onClick={() => this.props.selectTopic(topic.id)}>
          <h3 className="panel-title">{topic.name}</h3>
        </div>
        <div className="well well-sm">
          {this.props.selectedTopic &&
            this.props.selectedTopic.id === topic.id &&
            this.props.selectedTopic.entryHeaders.map(
              entry =>
                entry.id === this.props.entryId ? (
                  this.showEntry(entry)
                ) : (
                  <button
                    className="list-group-item"
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

  private showEntry(entry: LearningLogsState.EntryHeader) {
    const selectedEntry = this.props.selectedEntry;
    return (
      <div className="panel panel-info">
        <div className="panel-heading" onClick={() => this.props.selectEntry(entry.id)}>
          <h4 className="panel-title">{entry.title}</h4>
        </div>
        {selectedEntry &&
          selectedEntry.id == entry.id && (
            <div className="well">
              <p>
                {selectedEntry.title}
                <br />
                {selectedEntry.link}
              </p>
            </div>
          )}
      </div>
    );
  }
}

export default connect(
  (state: ApplicationState) => state.learningLogs,
  LearningLogsState.actionCreators
)(MyLibaray) as typeof MyLibaray;
