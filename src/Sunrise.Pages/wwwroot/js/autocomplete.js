//function to get suggestions from server

const getTags = async () => {
  var r = await fetch("/tags/complete");
  var d = await r.json();
  var str = d.tags.split(';');
  let tags = [];
  for(let o of str){
    tags.push(o);
  }
  return tags;
}

const sortSuggestions = (a, b) => {
  let c = (a.indexOf(activeSuggestion) - b.indexOf(activeSuggestion));
  if(c === 0){
    return a.localeCompare(b);
  }
  return c;
}

const input = document.getElementById("find");

const suggestions = [];

let rawTags = [];

const awesomplete = new Awesomplete(input, {
  /*filter: () => { // We will provide a list that is already filtered ...
    return true;
  },*/
  sort: sortSuggestions,    // ... and sorted.
  list: [],
  minChars: 1,
  filter: function(text, input) {
		return Awesomplete.FILTER_CONTAINS(text, input.match(/[^ ]*$/)[0]);
	},

	item: function(text, input) {
		return Awesomplete.ITEM(text, input.match(/[^ ]*$/)[0]);
	},

	replace: function(text) {
		var before = this.input.value.match(/^.+ \s*|/)[0];
		this.input.value = before + text + " ";
	}
});

var activeSuggestion = "";

input.addEventListener("input", async (event) => {
  const inputText = event.target.value;
  const tags = inputText.split(" ");
  const lastTag = tags[tags.length - 1].trim().toLowerCase();

  activeSuggestion = lastTag;

  if(rawTags.length===0){
    rawTags = await getTags();
  }
  // Process inputText as you want, e.g. make an API request.
  awesomplete.list = rawTags;
  awesomplete.evaluate();
});