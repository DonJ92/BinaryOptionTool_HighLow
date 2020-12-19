// Coded by H(S 2018.12.25

Object.size = function(obj) {
    var size = 0, key;
    for (key in obj) {
        if (obj.hasOwnProperty(key)) size++;
    }
    return size;
};

function autocomplete(inp, minLength = 10) {
  /*the autocomplete function takes two arguments,
  the text field element and an array of possible autocompleted values:*/
  var currentFocus;
  /*execute a function when someone writes in the text field:*/
  inp.addEventListener("input", function(e) {
      var a, b, i, val = this.value;
      /*close any already open lists of autocompleted values*/
      closeAllLists();
      if (!val) {
          searchByTraderName(val);
          return false;
      }
      currentFocus = -1;
      /*create a DIV element that will contain the items (values):*/
      a = document.createElement("DIV");
      a.setAttribute("id", this.id + "autocomplete-list");
      a.setAttribute("class", "autocomplete-items");
      /*append the DIV element as a child of the autocomplete container:*/
      this.parentNode.appendChild(a);

      $.ajax({
        url: BASE_URL + 'ajax/users/getAll',
        type: 'POST',
        data: {
          'key':      val,
          'length' :  minLength,
        },
        success: function(arr) {
          /*for each item in the array...*/
          var length = Math.min(minLength, arr.length);
          for (i = 0; i < length; i++) {
            var value = arr[i]['email'];

            /*check if the item starts with the same letters as the text field value:*/
            //if (value.substr(0, val.length).toUpperCase() == val.toUpperCase()) {
              /*create a DIV element for each matching element:*/
              b = document.createElement("DIV");
              /*make the matching letters bold:*/
              let pos = value.indexOf(val);
              b.innerHTML = value.substr(0, pos);
              b.innerHTML += "<strong>" + value.substr(pos, val.length) + "</strong>";
              b.innerHTML += value.substr(pos + val.length);
              /*insert a input field that will hold the current array item's value:*/
              b.innerHTML += "<input type='hidden' value='" + value + "'>";
              /*execute a function when someone clicks on the item value (DIV element):*/
              b.addEventListener("click", function(e) {
                  var val = this.getElementsByTagName("input")[0].value;
                  if (val == '...') {
                      return;
                  }
                  /*insert the value for the autocomplete text field:*/
                  inp.value = val;
                  searchByTraderName(inp.value);
                  /*close the list of autocompleted values,
                  (or any other open lists of autocompleted values:*/
                  closeAllLists();
              });
              a.appendChild(b);
            //}
          }

          // Added by H(S
          if (length + 1 <= arr.length) {
            // Add ... item
            var value = '...';

            b = document.createElement("DIV");
            /*make the matching letters bold:*/
            b.innerHTML = "<strong>" + value.substr(0, val.length) + "</strong>";
            b.innerHTML += value.substr(val.length);
            /*insert a input field that will hold the current array item's value:*/
            b.innerHTML += "<input type='hidden' value='" + value + "'>";
            /*execute a function when someone clicks on the item value (DIV element):*/
            b.addEventListener("click", function(e) {
                var val = this.getElementsByTagName("input")[0].value;
                if (val == '...') {
                    return;
                }
                /*insert the value for the autocomplete text field:*/
                inp.value = val;
                searchByTraderName(inp.value);
                /*close the list of autocompleted values,
                (or any other open lists of autocompleted values:*/
                closeAllLists();
            });
            a.appendChild(b);
          }
        },
        error: function(err) {
          console.log('Autocomplete failed. Error :', err);
        }
      })


  });
  /*execute a function presses a key on the keyboard:*/
  inp.addEventListener("keydown", function(e) {
      var x = document.getElementById(this.id + "autocomplete-list");
      if (x) x = x.getElementsByTagName("div");
      if (e.keyCode == 40) {
        /*If the arrow DOWN key is pressed,
        increase the currentFocus variable:*/
        currentFocus++;
        if (x) {
          var nextValue = x[currentFocus >= x.length ? 0 : currentFocus].getElementsByTagName("input")[0].value;
          if (nextValue == '...') {
            // Add to next item
            currentFocus ++;
          }
        }
        /*and and make the current item more visible:*/
        addActive(x);
      } else if (e.keyCode == 38) { //up
        /*If the arrow UP key is pressed,
        decrease the currentFocus variable:*/
        currentFocus--;
        if (x) {
          var nextValue = x[currentFocus < 0 ? x.length + currentFocus : currentFocus].getElementsByTagName("input")[0].value;
          if (nextValue == '...') {
            // Add to prev item
            currentFocus --;
          }
        }
        /*and and make the current item more visible:*/
        addActive(x);
      } else if (e.keyCode == 13) {
        /*If the ENTER key is pressed, prevent the form from being submitted,*/
        e.preventDefault();
        if (currentFocus > -1) {
          /*and simulate a click on the "active" item:*/
          if (x) x[currentFocus].click();
        }
      }
      else if (e.keyCode == 27) {
        var x = document.getElementsByClassName("autocomplete-items");
        if (!x.length) return;
        inp.value = '';
        searchByTraderName(inp.value);
        closeAllLists();
      }
  });
  function addActive(x) {
    /*a function to classify an item as "active":*/
    if (!x) return false;
    /*start by removing the "active" class on all items:*/
    removeActive(x);
    if (currentFocus >= x.length) currentFocus = 0;
    if (currentFocus < 0) currentFocus = x.length + currentFocus;
    /*add class "autocomplete-active":*/
    x[currentFocus].classList.add("autocomplete-active");
  }
  function removeActive(x) {
    /*a function to remove the "active" class from all autocomplete items:*/
    for (var i = 0; i < x.length; i++) {
      x[i].classList.remove("autocomplete-active");
    }
  }
  function closeAllLists(elmnt) {
    /*close all autocomplete lists in the document,
    except the one passed as an argument:*/
    var x = document.getElementsByClassName("autocomplete-items");
    for (var i = 0; i < x.length; i++) {
      if (elmnt != x[i] && elmnt != inp) {
      x[i].parentNode.removeChild(x[i]);
    }
  }
}
/*execute a function when someone clicks in the document:*/
document.addEventListener("click", function (e) {
    closeAllLists(e.target);
  });
}
