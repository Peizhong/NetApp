import React from 'react'
import PropTypes from 'prop-types'
import Todo from './Todo'
import { List } from 'antd'

const TodoList = ({ todos, onTodoClick }) => (
    <List
        header={<div>Header</div>}
        bordered>
        {todos.map((todo, index) => (
            <Todo key={index} {...todo} onClick={() => onTodoClick(todo.id)} />
        ))}
    </List>
)

TodoList.propTypes = {
    todos: PropTypes.arrayOf(
        PropTypes.shape({
            id: PropTypes.number.isRequired,
            completed: PropTypes.bool.isRequired,
            text: PropTypes.string.isRequired
        }).isRequired
    ).isRequired,
    onTodoClick: PropTypes.func.isRequired
}

export default TodoList