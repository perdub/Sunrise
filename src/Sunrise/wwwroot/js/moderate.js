//this file contains all js code to moderation

//post moderation
function trackTags(){
    var Tags = [];
    var TagsList = document.getElementById('tag-list');
    document.getElementById('addTag').onclick = ()=>{
        var nt = document.createElement('div');
        var i = document.createElement('input');
        i.type = "text";
        i.value = "INCERT_TAG";
        nt.appendChild(i);
        var a = document.createElement('p');
        a.innerText = 'id:';
        nt.appendChild(a);
        nt.appendChild(document.createElement('br'));
        TagsList.appendChild(nt);
    };
}
function sendModerationVersion(){
    document.getElementById('check').onclick = ()=>{
        
    };
}
//