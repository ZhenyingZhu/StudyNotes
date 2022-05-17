const projectUri = 'api/projects';
const todoUri = 'api/todoitems';
let projects = [];
let todos = [];
let selectedProjectId = -1;

// Projects
function getProjects() {
    // Called in
    // 1. page load
    // 2. create a project
    // 3. update a project
    // 4. delete a project
    // Don't unselect project/clear error here. After create and update a project, the
    // selected project should not change.
    fetch(projectUri)
        .then(response => response.json())
        .then(data => _displayProjects(data))
        .catch(error => console.error('Unable to get projects.', error));
}

function _displayProjects(data) {
    // Only called in getProjects()
    const tBody = document.getElementById('projects');
    tBody.innerHTML = '';

    _displayProjectsCount(data.length);

    const button = document.createElement('button');

    data.forEach(project => {
        let tr = tBody.insertRow();

        let projectNameTextNode = document.createTextNode(project.name);
        let td1 = tr.insertCell(0);
        td1.appendChild(projectNameTextNode);

        let todoCountTextNode = document.createTextNode(project.todoItems.length);
        let td2 = tr.insertCell(1);
        td2.appendChild(todoCountTextNode);

        let checkButton = button.cloneNode(false);
        checkButton.innerText = 'Check';
        checkButton.setAttribute('onclick', `_displayProjectEditForm(${project.id})`);
        let td3 = tr.insertCell(2);
        td3.appendChild(checkButton);
    });

    projects = data;

    // Don't need to unset selectedProject here because get shouldn't update anything.
    // But need to reload the selectedProject name as there might be changes.
    // Do it here because the in mem projects array is used to display.
    if (selectedProjectId !== -1) {
        _selectProjectAndDisplayTodoViewForm(selectedProjectId);
    }
}

function _displayProjectsCount(projectCount) {
    // Only called in _displayProjects
    const name = (projectCount === 1) ? 'project' : 'projects';
    document.getElementById('projectCounter').innerText = `In total ${projectCount} ${name}`;
}

function _displayProjectEditForm(id) {
    // Only called in _displayProjects()
    // This is the logic for select a project.
    if (id !== selectedProjectId) {
        _clearProjectErrorMessage();
    }

    const project = projects.find(project => project.id === id);

    _selectProjectAndDisplayTodoViewForm(project.id);

    document.getElementById('editProjectId').value = project.id;
    document.getElementById('editProjectName').value = project.name;
    document.getElementById('editProjectForm').style.display = 'block';
}

function _selectProjectAndDisplayTodoViewForm(projectId) {
    // Called in
    // 1. display projects when selectedProject is set
    // 2. display a project
    // Not clear error message here because here cannot decide wheter a new or old project is selected 
    project = projects.find(project => project.id === projectId);

    selectedProjectId = project.id;

    _displayTodoItems(project.todoItems);
}

function closeProjectEditForm() {
    // Called in when click the close button in edit form or a project get deleted.
    // This is same as unselect a project.
    _clearProjectErrorMessage();

    document.getElementById('editProjectForm').style.display = 'none';

    selectedProjectId = -1;
    _clearTodoItems([]);
}

function createProject() {
    // Only called when click the submit button on createProjectForm.
    _clearProjectErrorMessage();

    const createProjectNameTextBox = document.getElementById('createProjectName');

    const project = {
        name: createProjectNameTextBox.value.trim()
    };

    fetch(projectUri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(project)
    })
        .then(response => response.json()) // TODO: Handle 500, get project id
        .then(() => {
            createProjectNameTextBox.value = '';
            getProjects();
        })
        .catch(error => _displayProjectErrorMessage('Unable to create project.', error));
}

function updateProject() {
    // Called when click the submit button on the edit form.
    _clearProjectErrorMessage();

    const projectId = document.getElementById('editProjectId').value;
    const project = {
        id: parseInt(projectId, 10),
        name: document.getElementById('editProjectName').value.trim()
    };

    // Here don't need set the selectedProjectId as it is set during click the check button.
    fetch(`${projectUri}/${projectId}`, {
        method: 'PUT',
        headers: { // TODO: move headers to a const.
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(project)
    })
        .then(() => getProjects()) // TODO: handle 500.
        .catch(error => _displayProjectErrorMessage('Unable to update project.', error));

    // Don't need to close the edit form as the updated project should still be selected
    return false;
}

function deleteProject() {
    // Only called when click the delete button.
    // No need to clear error message because if the deletion succeed, unselect the project
    // clears it. If the deletion fails, the error message rewrites
    fetch(`${projectUri}/${selectedProjectId}`, {
        method: 'DELETE'
    })
        .then((response) => {
            if (response.status === 500) {
                _displayProjectErrorMessage('Failed to delete project.', null);
            }
            else {
                closeProjectEditForm();
                getProjects();
            }
        })
        .catch(error => _displayProjectErrorMessage('Unable to delete project.', error));
}

function _displayProjectErrorMessage(msg, error) {
    // Called in create, update, delete projects.
    console.error(msg, error);
    const errorPara = document.getElementById('projectErrorMessage');
    console.log(errorPara.innerText);
    errorPara.innerText = msg;
    errorPara.style.display = 'block';
}

function _clearProjectErrorMessage() {
    // Called when select a new project, create, update, delete projects.
    // Should be called when doing a new write of a project or select another project.
    const errorPara = document.getElementById('projectErrorMessage');
    errorPara.innerText = '';
    errorPara.style.display = 'none';
}


// Todos
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

function _displayTodosCount(itemCount) {
    const name = (itemCount === 1) ? 'to-do' : 'to-dos';

    document.getElementById('todoCounter').innerText = `In total ${itemCount} ${name}`;
}

function _clearTodoItems() {
    document.getElementById('todoCounter').innerText = '';
    document.getElementById('todos').innerHTML = '';
}

function getItems() {
    fetch(todoUri)
    .then(response => response.json())
    .then(data => _displayTodoItems(data))
    .catch(error => console.error('Unable to get items.', error));
}

function deleteItem(id) {
    // Make sure after delete it refreshes
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