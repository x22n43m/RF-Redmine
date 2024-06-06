document.addEventListener('DOMContentLoaded', function() {
    loadProjectTypes();
    loadDevelopers();
    loadProjects();
    loadDeadlineAlerts();
    establishSignalRConnection();
    
    // Set the default time to 23:59
    setDefaultTaskDeadline();
});

document.getElementById('loadManagerTasksButton').addEventListener('click', function() {
    loadManagerTasks();
});

function establishSignalRConnection() {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/notificationHub")
        .build();

    connection.on("NotifyTasksApproachingDeadline", function(tasks) {
        loadDeadlineAlerts(); 
    });

    connection.start().then(function() {
        console.log("SignalR connected.");
    }).catch(function (err) {
        console.error("SignalR connection error: " + err.toString());
    });
}

function displayDeadlineAlerts(tasks) {
    const deadlineAlerts = document.getElementById('deadlineAlerts');
    deadlineAlerts.innerHTML = '';
    tasks.forEach(task => {
        const li = document.createElement('li');
        li.textContent = `Task "${task.name}" is nearing its deadline: ${new Date(task.deadline).toLocaleString()}`;
        deadlineAlerts.appendChild(li);
    });
}

function loadDeadlineAlerts() {
    const jwt = sessionStorage.getItem('jwt');
    if (!jwt) {
        console.error('JWT not found in sessionStorage');
        const feedbackElement = document.getElementById('feedback');
        if (feedbackElement) {
            feedbackElement.textContent = 'Authentication error. Please log in again.';
        }
        return;
    }

    const days = 14;
    fetch(`/api/tasks/approaching-deadline?days=${days}`, {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${jwt}`,
            'Content-Type': 'application/json'
        }
    })
    .then(response => {
        if (!response.ok) {
            throw new Error(`Failed to fetch approaching deadline tasks, status: ${response.status}`);
        }
        return response.json();
    })
    .then(tasks => {
        console.log('Approaching deadline tasks:', tasks);
        if (!Array.isArray(tasks)) {
            throw new Error('Expected an array of tasks');
        }
        displayDeadlineAlerts(tasks);
    })
    .catch(error => {
        console.error('Error loading approaching deadline tasks:', error);
        const feedbackElement = document.getElementById('feedback');
        if (feedbackElement) {
            feedbackElement.textContent = 'Error loading approaching deadline tasks: ' + error.message;
        }
    });
}

function loadManagerTasks() {
    const jwt = sessionStorage.getItem('jwt');
    const managerId = sessionStorage.getItem('managerId');
    if (!jwt) {
        console.error('JWT not found in sessionStorage');
        const feedbackElement = document.getElementById('feedback');
        if (feedbackElement) {
            feedbackElement.textContent = 'Authentication error. Please log in again.';
        }
        return;
    }

    fetch('/api/tasks', {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${jwt}`,
            'Content-Type': 'application/json'
        }
    })
    .then(response => {
        if (!response.ok) {
            throw new Error(`Failed to fetch tasks, status: ${response.status}`);
        }
        return response.json();
    })
    .then(tasks => {
        console.log('Manager tasks:', tasks);
        if (!Array.isArray(tasks)) {
            throw new Error('Expected an array of tasks');
        }
        const managerTasksList = document.getElementById('managerTasks');
        managerTasksList.innerHTML = '';
        tasks.forEach(task => {
            const li = document.createElement('li');
            li.textContent = `${task.name}: ${task.description}`;
            managerTasksList.appendChild(li);
        });
    })
    .catch(error => {
        console.error('Error loading tasks:', error);
        const feedbackElement = document.getElementById('feedback');
        if (feedbackElement) {
            feedbackElement.textContent = 'Error loading tasks: ' + error.message;
        }
    });
}

function loadProjectTypes() {
    fetch('/api/ProjectTypes')
        .then(response => {
            if (!response.ok) {
                throw new Error(`Failed to fetch project types, status: ${response.status}`);
            }
            return response.json();
        })
        .then(types => {
            console.log('Project types:', types);
            if (!Array.isArray(types)) {
                throw new Error('Expected an array of project types');
            }
            const typeSelect = document.getElementById('projectType');
            types.forEach(type => {
                const option = new Option(type.name, type.id);
                typeSelect.add(option);
            });
        })
        .catch(error => {
            console.error('Error loading project types:', error);
        });
}

function loadDevelopers() {
    fetch('/api/developers')
        .then(response => {
            if (!response.ok) {
                throw new Error('Failed to fetch developers');
            }
            return response.json();
        })
        .then(developers => {
            console.log('Developers:', developers);
            if (!Array.isArray(developers)) {
                throw new Error('Expected an array of developers');
            }
            const developerSelect = document.getElementById('developerList');
            developers.forEach(developer => {
                const option = document.createElement('option');
                option.value = developer.id;
                option.textContent = developer.name;
                developerSelect.appendChild(option);
            });
        })
        .catch(error => console.error('Error loading developers:', error));
}

document.getElementById('newTaskForm').onsubmit = function(event) {
    event.preventDefault();
    addNewTask();
};

function addNewTask() {
    const name = document.getElementById('taskName').value;
    const description = document.getElementById('taskDescription').value;
    const developerId = document.getElementById('developerList').value;
    const deadline = document.getElementById('taskDeadline').value;
    const projectId = document.getElementById('projectSelect').value;

    const taskData = {
        name: name,
        description: description,
        projectId: projectId,
        deadline: deadline,
        developerId: developerId
    };

    const jwt = sessionStorage.getItem('jwt');

    fetch('/api/tasks/add', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${jwt}`
        },
        body: JSON.stringify(taskData)
    })
    .then(response => {
        if (!response.ok) {
            throw new Error('Failed to create task');
        }
        return response.json();
    })
    .then(() => {
        alert('Task added successfully');
        loadDeadlineAlerts();
    })
    .catch(error => {
        alert(error.message);
    });
}

function loadProjects(typeId = 0) {
    const url = typeId ? `/api/projects?typeId=${typeId}` : '/api/projects';
    fetch(url)
        .then(response => {
            if (!response.ok) {
                throw new Error(`Failed to fetch projects, status: ${response.status}`);
            }
            return response.json();
        })
        .then(projects => {
            console.log('Projects:', projects);
            if (!Array.isArray(projects)) {
                throw new Error('Expected an array of projects');
            }
            const projectList = document.getElementById('projectList');
            const projectSelect = document.getElementById('projectSelect');
            projectList.innerHTML = '';
            projectSelect.innerHTML = '';
            projects.forEach(project => {
                const li = document.createElement('li');
                li.textContent = project.name;
                li.onclick = () => loadTasksForProject(project.id);
                projectList.appendChild(li);

                const option = document.createElement('option');
                option.value = project.id;
                option.textContent = project.name;
                projectSelect.appendChild(option);
            });
        })
        .catch(error => {
            console.error('Error loading projects:', error);
        });
}

function filterProjects() {
    const typeId = document.getElementById('projectType').value;
    loadProjects(typeId);
}

function loadTasksForProject(projectId) {
    fetch(`/api/projects/${projectId}/tasks`)
        .then(response => {
            if (!response.ok) {
                throw new Error(`Failed to fetch tasks for project, status: ${response.status}`);
            }
            return response.json();
        })
        .then(tasks => {
            console.log('Tasks for project:', tasks);
            if (!Array.isArray(tasks)) {
                throw new Error('Expected an array of tasks');
            }
            const taskList = document.getElementById('taskList');
            taskList.innerHTML = '';
            tasks.forEach(task => {
                const li = document.createElement('li');
                li.textContent = task.name;
                taskList.appendChild(li);
            });
        })
        .catch(error => {
            console.error('Error loading tasks for project:', error);
        });
}

function setDefaultTaskDeadline() {
    const taskDeadlineInput = document.getElementById('taskDeadline');
    if (taskDeadlineInput) {
        const now = new Date();
        const year = now.getFullYear();
        const month = String(now.getMonth() + 1).padStart(2, '0');
        const day = String(now.getDate()).padStart(2, '0');
        const hours = '23';
        const minutes = '59';
        
        const defaultDate = `${year}-${month}-${day}T${hours}:${minutes}`;
        taskDeadlineInput.value = defaultDate;
    }
}