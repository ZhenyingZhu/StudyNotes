// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const todoUri = 'api/todoitems';
let todos = [];

function getTodos() {
    fetch(todoUri)
        .then(response => response.json())
        .then(data => _displayTodos(data))
        .catch(error => console.error('Unable to get Todos.', error));
}

function _displayTodos(data) {
    const tBody = document.getElementById('todos');
    tBody.innerHTML = '';

    data.forEach(todo => {
        let tr = tBody.insertRow();

        let todoNameTextNode = document.createTextNode(todo.name);
        let td1 = tr.insertCell(0);
        td1.appendChild(todoNameTextNode);
    });

    todos = data;
}

function createTodo() {
    const createTodoNameTextBox = document.getElementById('createTodoName');

    const todo = {
        name: createTodoNameTextBox.value.trim(),
        isComplete: false
    };

    fetch(todoUri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(todo)
    })
        .then(response => response.json())
        .then(() => {
            createTodoNameTextBox.value = '';
            getTodos();
        })
        .catch(error => console.error('Unable to create Todo.', error));
}
