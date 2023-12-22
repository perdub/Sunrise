function scrollBuild(postId, baseLink, contentType){
    var rt = document.createElement('div');
    rt.style.display = "flex";
    var a = document.createElement('a');
    rt.appendChild(a);
    a.href = '/post/'+postId;
    a.style.margin = "auto";
    document.getElementById('scroll').appendChild(rt);
    var i;
    switch(contentType){
        case 1:
            i = document.createElement('video');
            var videoSource = document.createElement('source');
            videoSource.src = baseLink;
            i.appendChild(videoSource);
            i.muted = true;
            i.play();
            break;
        case 0:
        default:
            i = document.createElement('img');
            i.src = baseLink;
            break;
    }
    i.className += "image-post";
    i.style.maxWidth = "100%";
    a.appendChild(i);
}
async function fill(){
    //проверка на то вызывалась ли функция недавно
    if(Date.now()-scrollLastInvokeTimestap<scrollInvokeTimeout){
        return;
    }
    scrollLastInvokeTimestap = Date.now();

    //увеличение сдвига
    let localScrollOffset = scrollOffset;
    scrollOffset += scrollCount;

    //запрос к серверу
    var a = await fetch('/find'+grabQueryString()+'&offset='+localScrollOffset+'&count='+scrollCount);
    var b = await a.json();
    
    //создание элементов
    b.forEach(c => {
        scrollBuild(c.postId, c.baseLink, c.contentType);
    });
}
function scroll(){
    var a = document.documentElement.getBoundingClientRect().bottom + document.getElementById('scroll').offsetHeight;
    if(a<document.documentElement.clientHeight + 4096){
        fill();
    }
}
var scrollOffset=0;
var scrollCount=15;
var scrollLastInvokeTimestap = 0;
const scrollInvokeTimeout = 5*1000;
window.onscroll = function(){
    scroll();
}
scroll();