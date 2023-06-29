$(document).ready(function () {
    requestToServer('Users/IsLogin', 'get', '', (islogin) => {
        if (islogin) {
            window.location.href += './home'
        } else {
            window.location.href += './welcome'
        }
    })
});