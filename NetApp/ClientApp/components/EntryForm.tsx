import * as React from "react";
import { connect } from "react-redux";
import { ApplicationState } from "../store";
import * as LearningLogsState from "../store/LearningLogs";

interface states {
  selectedEntry?: LearningLogsState.Entry;
}

const actions = {
  editedEntry: LearningLogsState.actionCreators.editedEntry,
  saveEntry: LearningLogsState.actionCreators.saveEntry
};

type EntryFormProps = states & typeof actions;

class EntryForm extends React.Component<EntryFormProps, {}> {
  public render() {
    const { selectedEntry, editedEntry, saveEntry } = this.props;
    if (!selectedEntry) return <div />;
    const { id, title, link, text } = selectedEntry;
    return (
      <form className="form-horizontal">
        <div className="form-group">
          <label className="col-sm-2 control-label">Title</label>
          <div className="col-sm-10">
            <input
              className="form-control"
              type="text"
              value={title}
              onChange={v => editedEntry(id, "title", v.target.value)}
            />
          </div>
        </div>
        <div className="form-group">
          <label className="col-sm-2 control-label">Link</label>
          <div className="col-sm-10">
            <input
              className="form-control"
              type="text"
              value={link}
              onChange={v => editedEntry(id, "link", v.target.value)}
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
              onChange={v => editedEntry(id, "text", v.target.value)}
            />
          </div>
        </div>
        <div className="form-group">
          <div className="col-sm-offset-2 col-sm-10">
            <div className="btn-group" role="group">
              <button
                type="button"
                className="btn btn-info"
                onClick={() => saveEntry(id)}
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

export default EntryForm;
