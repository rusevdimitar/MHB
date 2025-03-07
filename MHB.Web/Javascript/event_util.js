/*
EventUtil is cross browser event handling object
source is taken from book "Pro Javascript for Web Developers, 2nd Edition"
*/
var EventUtil = {
    //DOMReady is not taken from the book
    //it's written by iFadey.com
    DOMReady: function (f) {
        if (document.addEventListener) {
            document.addEventListener("DOMContentLoaded", f, false);
        } else {
            window.setTimeout(f, 0);
        }
    },

    addHandler: function (element, type, handler) {
        if (element.addEventListener) {
            element.addEventListener(type, handler, false);
        } else if (element.addEvent) {
            element.addEvent('on' + type, handler);
        } else {
            element['on' + type] = handler;
        }
    },

    removeHandler: function (element, type, handler) {
        if (element.removeEventListener) {
            element.removeEventListener(type, handler);
        } else if (element.detachEvent) {
            element.detachEvent('on' + type, handler);
        } else {
            element['on' + type] = null;
        }
    },

    getEventObj: function (e) {
        return e ? e : window.event;
    },

    getEventTarget: function (e) {
        return e.target ? e.target : e.srcElement;
    },

    preventDefault: function (e) {
        e.preventDefault ? e.preventDefault() : e.returnValue = false;
    }
};