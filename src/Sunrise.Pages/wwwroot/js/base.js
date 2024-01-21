function loopVideoPreview(time){
    
}
function getCookie(name) {
    let matches = document.cookie.match(new RegExp(
      "(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"
    ));
    return matches ? decodeURIComponent(matches[1]) : undefined;
}
function grabQueryString(){
    if(window.location.search===''){
        return '?';
    }
    return window.location.search;
}
function getRandomInt(max) {
    return Math.floor(Math.random() * max);
}
function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
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
