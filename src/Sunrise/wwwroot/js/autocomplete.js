function getRawTags(){
    return document.getElementById('find').value.trim();
}

function getTagToComplete(){
    return getRawTags().split(' ').pop();
}

function editTags(newTag){
    var raw = getRawTags().split(' ');
    raw.pop();
    let str = (raw.join(' ')+' '+newTag).trim();
    document.getElementById('find').value = str;
}

async function complete(){
    var tag = getTagToComplete();
    var res = await fetch("/tags/complete?tag="+tag);
    var item = await res.json();
    return item;
}

function clickHandler(){
    editTags(this.innerText);
}

async function getSuggections(){
    showSuggections();
    var a = await complete();
    let list = document.getElementById('autocomplete');
    list.innerHTML = '';
    for(let idx = 0; idx<a.length;idx++)
    {
        var ns = document.createElement('p');
        ns.className = "tag_item";
        ns.onmousedown = clickHandler;
        ns.innerText = a[idx].searchText;
        list.appendChild(ns);
    }
}

function hideSuggections(){
    document.getElementById('autocomplete').style.display = "none";
}
function showSuggections(){
    document.getElementById('autocomplete').style.removeProperty('display');
}

function eventSubscribe(){
    var f = document.getElementById('find');
    var list = document.getElementById('autocomplete');

    f.oninput = getSuggections;
    f.onfocus = getSuggections;
    f.onblur = hideSuggections;

    list.style.width = f.offsetWidth+'px';
}