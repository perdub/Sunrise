function getRawTags(){
    return tagInput.value.trim();
}

function getTagToComplete(){
    return getRawTags().split(' ').pop();
}

function editTags(newTag){
    var raw = getRawTags().split(' ');
    raw.pop();
    let str = (raw.join(' ')+' '+newTag).trim();
    tagInput.value = str;
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
    let list = tagsSuggections;
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
    tagsSuggections.style.display = "none";
}
function showSuggections(){
    tagsSuggections.style.removeProperty('display');
}

function eventSubscribe(){
    tagInput = document.getElementById('find');
    tagsSuggections = document.getElementById('autocomplete');

    tagInput.oninput = getSuggections;
    tagInput.onfocus = getSuggections;
    tagInput.onblur = hideSuggections;

    tagsSuggections.style.width = tagInput.offsetWidth+'px';
}

var tagInput=0;
var tagsSuggections=0;