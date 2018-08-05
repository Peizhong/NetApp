import React from 'react'
import Footer from './Footer'
import AddTodo from '../containers/AddTodo'
import VisibleTodoList from '../containers/VisibleTodoList'

const Home = () => (
    <div>
        This is Home Page
        <AddTodo />
        <VisibleTodoList />
        <Footer />
    </div>
)

export default Home