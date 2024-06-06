document.getElementById('loginForm').onsubmit = function(event) {
    event.preventDefault();

    var username = document.getElementsByName('uname')[0].value;
    var password = document.getElementsByName('psw')[0].value;

    var data = JSON.stringify({
        Username: username,
        Password: password
    });

    fetch('/auth/login', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: data
    })
    .then(response => response.json())
    .then(data => {
        if (data.token && data.managerId) {
            sessionStorage.setItem('jwt', data.token);
            sessionStorage.setItem('managerId', data.managerId);
            
            establishWebSocketConnection(); 

            window.location.href = data.redirectUrl; 
        } else {
            throw new Error('Login failed: Token or manager ID missing');
        }
    })
    .catch(error => {
        document.getElementById('feedback').textContent = error.message;
    });
};

function establishWebSocketConnection() {
    const socket = new WebSocket('ws://localhost:5000');
    
    socket.onopen = function(event) {
        console.log("WebSocket is open now.");
    };

    socket.onmessage = function(event) {
        console.log("WebSocket message received:", event.data);
    };

    socket.onclose = function(event) {
        console.log("WebSocket is closed now.");
    };

    socket.onerror = function(error) {
        console.log("WebSocket error:", error);
    };

    window.webSocket = socket;
}