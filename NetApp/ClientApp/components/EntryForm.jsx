import * as React from 'react';
import { connect } from 'react-redux';
import { Field, reduxForm } from 'redux-form'

let EntryForm = props => {
    const { text, link } = props.selectedEntry;
    return (<div className="well">
        <form>
            {props.selectedEntry.text}
            <br />
            {props.selectedEntry.link}
        </form>
        <form onSubmit={(v) => {
            console.log(v);
            console.log('can you see?');
        }}>
            <div>
                <label htmlFor="link">Link</label>
                <Field name="link" component="input" type="text" value={link} />
            </div>
            <div>
                <label htmlFor="content">Content</label>
                <Field name="content" component="input" type="text" value={text} />
            </div>
            <button type="submit">Submit</button>
        </form >
    </div >
    );
}

EntryForm = reduxForm({
    // a unique name for the form
    form: 'entry'
})(EntryForm)

export default EntryForm