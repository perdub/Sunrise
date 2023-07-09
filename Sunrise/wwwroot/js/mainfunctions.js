function getRandomInt(max) {
    return Math.floor(Math.random() * max);
}
function getRandomBackground() {
    fetch("/random", {
        method: "POST",
        headers: {
            "Access-Control-Allow-Origin": "*",
            "mode": "no-cors",
            "Content-Type": "application/json"
        }
    }).then(x => x.json()).then((y) => {
        var background = document.getElementById('background');
        background.style.setProperty("background-image", "url('/" + y.path + "')");
        background.style.setProperty("background-position", "center "+getRandomInt(101)+"%");
    });
}
async function setUserButton(){
    //функция которая заменяет переход на страницу логина ссылкой на пользователя
    let a = await getUser();
    if(a.ok===true){
        var b = document.getElementById('topbarloginuser').children[0];
        b.href = "/users/"+a.id;
        b.children[0].innerText  = a.name;
    }
}