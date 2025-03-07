<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ToolTip.ascx.vb" Inherits="MHB.Web.ToolTip" %>

<script type="text/javascript" language="javascript">
    var divObj;

    /**
    *identify which event is supported
    * Based on that collect pageX and pageY properties of the event object
    * pageX and pageY gets the X and Y cursor coordinates
    */

    $("#toolTip").mouseover(function () {
        //divObj.css("display", "none");
        divObj.fadeOut("slow");
    });

    function ShowToolTip(event, text) {
        try {

            var ev = event || window.event;

            var x = ev.pageX || ev.clientX;
            var y = ev.pageY || ev.clientY;

            //    divObj.style.posLeft  = 10 + ev.clientX;
            //    divObj.style.posTop = 10 + ev.clientY;
            //divObj.style.visibility = 'visible';

            //    divObj.position.left = 10 + ev.pageX;
            //    divObj.position.top = 10 + ev.pageY;

            //divObj.css({ top: ev.pageY, left: ev.pageX });

            divObj.css("top", 10 + y);
            divObj.css("left", 10 + x);

            divObj.fadeIn("slow");

            divObj.html(text);

        } catch (e) {

        }
    }
    function HideToolTip(event, text) {
        //divObj.style.visibility = 'hidden';
        //divObj.hide("slow");
        //divObj.css("display", "none");
        divObj.fadeOut("slow");
    }

    function loadDiv() {

        divObj = $("#toolTip");
        //divObj = document.getElementById('toolTip');
    }
</script>


<div id="toolTip" style="border: 1px dotted #FF9900; background-color: #FFFFCC; position: absolute; display: none; font-family: Arial, Helvetica, sans-serif; font-size: 12px; color: #000066; padding: 8px;">
</div>
