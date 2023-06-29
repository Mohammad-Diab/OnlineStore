$(document).ready(function () {
    requestToServer('Users/IsLogin', 'get', '', (userFullName) => {
        if (userFullName) {
            window.location.href += './home'
        }
    });
});