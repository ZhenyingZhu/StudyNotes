const projectUri = 'api/projects';
const todoUri = 'api/todoitems';
let projects = [];
let todos = [];
let selectedProjectId = -1;

// Projects
function _displayProjectsCount(projectCount) {
    const name = (projectCount === 1) ? 'project' : 'projects';
    document.getElementById('projectCounter').innerText = `${projectCount} ${name}`;
}

function getProjects() {
    fetch(projectUri)
        .then(response => response.json())
        .then(data => _displayProjects(data))
        .catch(error => console.error('Unable to get projects.', error));
}

function _displayProjects(data) {
    const tBody = document.getElementById('projects');
    tBody.innerHTML = '';

    _displayProjectsCount(data.length);

    const button = document.createElement('button');

    data.forEach(project => {
        let tr = tBody.insertRow();

        let textNode = document.createTextNode(project.name);
        let td1 = tr.insertCell(0);
        td1.appendChild(textNode);

        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayProjectEditForm(${project.id})`);
        let td2 = tr.insertCell(1);
        td2.appendChild(editButton);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteProject(${project.id})`);
        let td3 = tr.insertCell(2);
        td3.appendChild(deleteButton);

        let checkButton = button.cloneNode(false);
        checkButton.innerText = 'Check';
        checkButton.setAttribute('onclick', `_displayTodoViewForm(${project.id})`);
        let td4 = tr.insertCell(3);
        td4.appendChild(checkButton);
    });

    projects = data;
}

function displayProjectEditForm(id) {
    const project = projects.find(project => project.id === id);

    document.getElementById('editProjectId').value = project.id;
    document.getElementById('editProjectName').value = project.name;
    document.getElementById('editProjectForm').style.display = 'block';
}

function closeProjectInput() {
    document.getElementById('editProjectForm').style.display = 'none';
}

function updateProject() {
    const projectId = document.getElementById('editProjectId').value;
    const project = {
        id: parseInt(projectId, 10),
        name: document.getElementById('editProjectName').value.trim()
    };

    fetch(`${projectUri}/${projectId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(project)
    })
        .then(() => getProjects())
        .catch(error => console.error('Unable to update project.', error));

    closeProjectInput();

    return false;
}

function deleteProject(id) {
    fetch(`${projectUri}/${id}`, {
        method: 'DELETE'
    })
        .then(() => getProjects())
        .catch(error => console.error('Unable to delete project.', error));
}

function _displayTodoViewForm(projectId) {
    selectedProjectId = projectId;
    project = projects.find(project => project.id === projectId);
    console.log(JSON.stringify(project));
    _displayTodosCount(project.todoItems.length);
    _displayTodoItems(project.todoItems);
}

// Todos
function _displayTodosCount(itemCount) {
    const name = (itemCount === 1) ? 'to-do' : 'to-dos';

    document.getElementById('todoCounter').innerText = `${itemCount} ${name}`;
}


function addTodoItemToProject() {
    // TODO: check if selectedProject is not -1.
    const createTodoNameTextbox = document.getElementById('createTodoName');
    const createTodoSecretTextbox = document.getElementById('createTodoSecret');

    const item = {
        isComplete: false,
        name: createTodoNameTextbox.value.trim(),
        secret: createTodoSecretTextbox.value.trim()
    };

    fetch(`${projectUri}/${selectedProjectId}/TodoItems`, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(response => response.json())
        .then(() => {
            // This should be Project
            getItems();
            createTodoNameTextbox.value = '';
        })
        .catch(error => console.error('Unable to add item.', error));
}




function _displayTodoItems(data) {
    const tBody = document.getElementById('todos');
    tBody.innerHTML = '';

    _displayTodosCount(data.length);

    const button = document.createElement('button');

    data.forEach(item => {
        let tr = tBody.insertRow();

        let isCompleteCheckbox = document.createElement('input');
        isCompleteCheckbox.type = 'checkbox';
        isCompleteCheckbox.disabled = true;
        isCompleteCheckbox.checked = item.isComplete;

        let td1 = tr.insertCell(0);
        td1.appendChild(isCompleteCheckbox);

        let textNode = document.createTextNode(item.name);

        let td2 = tr.insertCell(1);
        td2.appendChild(textNode);

        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${item.id})`);

        let td3 = tr.insertCell(2);
        td3.appendChild(editButton);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteItem(${item.id})`);

        let td4 = tr.insertCell(3);
        td4.appendChild(deleteButton);
    });

    todos = data;
}

function getItems() {
    fetch(todoUri)
    .then(response => response.json())
    .then(data => _displayTodoItems(data))
    .catch(error => console.error('Unable to get items.', error));
}

function deleteItem(id) {
    fetch(`${todoUri}/${id}`, {
        method: 'DELETE'
    })
    .then(() => getItems())
    .catch(error => console.error('Unable to delete item.', error));
}

function displayEditForm(id) {
    const item = todos.find(item => item.id === id);

    document.getElementById('edit-name').value = item.name;
    document.getElementById('edit-id').value = item.id;
    document.getElementById('edit-isComplete').checked = item.isComplete;
    document.getElementById('editForm').style.display = 'block';
}

function updateItem() {
    const itemId = document.getElementById('edit-id').value;
    const item = {
        id: parseInt(itemId, 10),
        isComplete: document.getElementById('edit-isComplete').checked,
        name: document.getElementById('edit-name').value.trim()
    };

    fetch(`${todoUri}/${itemId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
    .then(() => getItems())
    .catch(error => console.error('Unable to update item.', error));

    closeInput();

    return false;
}

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}