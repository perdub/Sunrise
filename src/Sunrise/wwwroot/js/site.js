// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
async function login(login, password, remember) {
    var a = new Object();
    a.name = login.trim();
    a.password = password.trim();
    a.rememberMe = remember;
    var res = await fetch("/api/login/", {
        method: "POST",
        headers: {
            "Access-Control-Allow-Origin": "*",
            "mode": "no-cors",
            "Content-Type": "application/json"
        },
        body: JSON.stringify(a)
    });
    return await res.json();
}

async function registry(username, password, email)
{
    var a = new Object();
    a.username = username.trim();
    a.password = password.trim();
    a.email=email.trim();
    var res = await fetch("/auth/registry/", {
        method: "POST",
        headers: {
            "Access-Control-Allow-Origin": "*",
            "mode": "no-cors",
            "Content-Type": "application/json"
        },
        body: JSON.stringify(a)
    });
    return await res.json();
}

function openpage(page) {
    var ngp = "";
    window.location.search.substring(1).split('&').forEach((x) => {
        if(x==""){
            return;
        }

        if (x.split('=')[0] == 'page') {
            ngp += "page=" + page;
        }
        else {
            ngp += x;
        }
        ngp += '&';
    });
    window.open('/?'+ngp, '_self');
}