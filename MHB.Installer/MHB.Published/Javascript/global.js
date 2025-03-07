var defsrchinpt = '';

function searchin(obj) {
    if (defsrchinpt == '') {
        defsrchinpt = obj.value;
    }
    if (obj.value == defsrchinpt) {
        obj.value = '';
        obj.className = 'LoginTextBox';
    }
}

function searchout(obj) {
    if (obj.value.length == 0) {
        obj.value = defsrchinpt;
        obj.className = 'TextBoxEntryNotify'
    }
    else {
        obj.className = 'LoginTextBox'
    }
}

/*

TextBoxEmail.Attributes.Add("onmouseout", "javascript:searchout(this);")
TextBoxEmail.Attributes.Add("onmouseover", "javascript:searchin(this);")

*/

(function ($) {
    EnablePopup = function () {
        $("[id$='DivAddNewRecord']").css("height", "0px");

        $("[id$='DivModal']").animate(
                    {
                        width: "3000px",
                        height: "5000px",
                        opacity: 0.5
                    }, 800);

        $("[id$='DivAddNewRecord']").animate(
                                        {
                                            width: "280px",
                                            height: "480px",
                                            opacity: 1
                                        }, 500);

        $("[id$='DivAddNewRecord']").animate(
                                        {
                                            width: "300px",
                                            height: "500px",
                                            opacity: 1
                                        }, 3000);

        $("[id$='PanelAddNewFields']").animate(
                                        {
                                            width: "300px",
                                            height: "500px",
                                            opacity: 1
                                        }, 500);
    };
})(jQuery);