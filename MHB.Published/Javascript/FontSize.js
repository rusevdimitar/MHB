var elems = new Array('div', 'td', 'tr');

var sizes = new Array('xx-small', 'x-small', 'small', 'medium', 'large', 'x-large', 'xx-large');
var startSize = 2;

function resize(target, inc) {
    if (!document.getElementById) return
    var d = document, cEl = null, sz = startSize, i, j, cTags;

    sz += inc;
    if (sz < 0) sz = 0;
    if (sz > 6) sz = 6;
    startSize = sz;

    if (!(cEl = d.getElementById(target))) cEl = d.getElementsByTagName(target)[0];

    cEl.style.fontSize = sizes[sz];

    for (i = 0; i < elems.length; i++) {
        cTags = cEl.getElementsByTagName(elems[i]);
        for (j = 0; j < cTags.length; j++) cTags[j].style.fontSize = sizes[sz];
    }
}

//<a href="javascript:resize('body',1)">+ Larger Font</a> | <a
//href="javascript:resize('body',-1)">+ Smaller Font</a>