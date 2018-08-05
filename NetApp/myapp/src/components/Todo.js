import React from 'react'
import PropTypes from 'prop-types'
import { List, Checkbox } from 'antd'

const Todo = ({ onClick, completed, text }) => (
    <List.Item
        onClick={onClick}
        style={{
            textDecoration: completed ? 'line-through' : 'none'
        }}>
        <div>
            <Checkbox checked={completed} style={{ marginRight: '3px' }} />
            {text}
        </div>
    </List.Item>
)

Todo.propTypes = {
    onClick: PropTypes.func.isRequired,
    completed: PropTypes.bool.isRequired,
    text: PropTypes.string.isRequired
}

export default Todo