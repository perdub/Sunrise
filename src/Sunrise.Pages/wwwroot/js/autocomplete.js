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

const input = document.getElementById("find");

const suggestions = [];

let rawTags = [];

const awesomplete = new Awesomplete(input, {
  /*filter: () => { // We will provide a list that is already filtered ...
    return true;
  },*/
  sort: false,    // ... and sorted.
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

input.addEventListener("input", async (event) => {
  const inputText = event.target.value;

  if(rawTags.length===0){
    rawTags = await getTags();
  }
  // Process inputText as you want, e.g. make an API request.
  awesomplete.list = rawTags;
  awesomplete.evaluate();
});