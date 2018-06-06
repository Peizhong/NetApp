import * as React from 'react';
import { connect } from 'react-redux';
import { ApplicationState } from '../store';
import * as LearningLogsState from '../store/LearningLogs';

const actions = {
  editedEntry: LearningLogsState.actionCreators.editedEntry
};

type EntryFormProps = LearningLogsState.Entry & typeof actions;

class EntryForm extends React.Component<EntryFormProps, {}> {
  public render() {
    const { id, link, text, editedEntry } = this.props;
    return (
      <form className="form-horizontal">
        <div className="form-group">
          <label className="col-sm-2 control-label">Link</label>
          <div className="col-sm-10">
            <input
              className="form-control"
              type="text"
              value={link}
              onChange={v => editedEntry(id, 'link', v.target.value)}
            />
          </div>
        </div>
        <div className="form-group">
          <label className="col-sm-2 control-label">Content</label>
          <div className="col-sm-10">
            <input
              className="form-control"
              type="text"
              value={text}
              onChange={v => editedEntry(id, 'text', v.target.value)}
            />
          </div>
        </div>
        <div className="form-group">
          <div className="col-sm-offset-2 col-sm-10">
            <div className="btn-group" role="group">
              <button type="submit" className="btn btn-info">
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

export default connect(
  (state: ApplicationState) => state.learningLogs.selectedEntry, // Selects which state properties are merged into the component's props
  LearningLogsState.actionCreators // Selects which action creators are merged into the component's props
)(EntryForm) as typeof EntryForm;
