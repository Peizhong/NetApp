import React from 'react'
import { connect } from 'react-redux'
import { addTodo } from '../actions'
import { Input, Button } from 'antd'

let AddTodo = ({ dispatch }) => {
    let input = ''

    return (
        <div>
            <form
                onSubmit={e => {
                    e.preventDefault()
                    if (!input.value.trim()) {
                        return
                    }
                    dispatch(addTodo(input.value))
                    input.value = ''
                }}>
                <input ref={node => { input = node }} className='todoInput' />
                <Button type="primary" htmlType='submit'>
                    Add Todo
                </Button>
            </form>
        </div>
    )
}
AddTodo = connect()(AddTodo)

export default AddTodo