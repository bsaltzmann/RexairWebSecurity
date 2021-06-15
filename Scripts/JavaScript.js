
var ErrMsg = "Function is disabled. ";

function disableRightClick(btnClick) {
    if (navigator.appName == "Netscape" && btnClick.which == 3) // check for netscape and right click
    {
        alert(ErrMsg);
        return false;
    }
    else if (navigator.appName == "Microsoft Internet Explorer" && event.button == 2) // for IE and Right Click
    {
        alert(ErrMsg);
        return false;
    }
}
document.onmousedown = disableRightClick;

function checkCapsLock(e) {
    var myKeyCode = 0;
    var myShiftKey = false;
    var myMsg = 'Caps Lock is On.';

    // Internet Explorer 4+
    if (document.all) {
        myKeyCode = e.keyCode;
        myShiftKey = e.shiftKey;

        // Netscape 4
    } else if (document.layers) {
        myKeyCode = e.which;
        myShiftKey = (myKeyCode == 16) ? true : false;

        // Netscape 6
    } else if (document.getElementById) {
        myKeyCode = e.which;
        myShiftKey = (myKeyCode == 16) ? true : false;
    }

    // Upper case letters are seen without depressing the Shift key, therefore Caps Lock is on
    if ((myKeyCode >= 65 && myKeyCode <= 90) && !myShiftKey) {
        document.getElementById("message").style.color = "#FF0000";
        document.getElementById("message").innerHTML = "<h5>Warning, your CAPS LOCK is on!</h5>\n";
        // Lower case letters are seen while depressing the Shift key, therefore Caps Lock is on
    } else if ((myKeyCode >= 97 && myKeyCode <= 122) && myShiftKey) {
        document.getElementById("message").style.color = "#FF0000";
        document.getElementById("message").innerHTML = "<h5>Warning, your CAPS LOCK is on!</h5>\n";
        // numeric so do not change warning
    } else if (myKeyCode >= 48 && myKeyCode <= 57) {
        //document.getElementById("message").style.color = "#FF0000";
        // document.getElementById("message").innerHTML = "<h5>Warning, your CAPS LOCK is on!</h5>\n"; 
    }
    else {
        document.getElementById("message").innerHTML = "";
    }
}

function closeWindow() {
    if (navigator.appName == "Netscape") // check for netscape and right click
    {
        window.open('wsloginclose.html', '_self');
        return false;
    }
    else if (navigator.appName == "Microsoft Internet Explorer") // for IE and Right Click
    {
        window.open('', '_self', '');
        window.close();
        return false;
    }
    else {   //if browse is Chrome ,handle it with this 
        window.opener = null;
        window.open('', '_self', '');
        window.close();
    }
}

var PrintWin = null;
function PrintData(Src) {
    if (PrintWin == null) {
        PrintWin = window.open("wsreportname.aspx?src=" + Src,
            "_blank",
            "height=700, width=1045, left=10, top=10, " +
            "location=no, menubar=no, resizable=no, " +
            "scrollbars=no, titlebar=no, toolbar=no, fullscreen=no", true);
    }
    else {
        PrintWin.focus()
    }
}  

function autotab(original, destination) {
    if (original.getAttribute && original.value.length == original.getAttribute("maxlength"))
        destination.focus()
}

function WriteForm() {
    document.write('<table><tr>');
    if (window.opener) {
        document.write('<td><input type="text" name="subject" value="' + window.opener.document.title + '" size="60"></td>');
    }
    else {
        document.write('<td><input type="text" name="subject" size="60"></td>');
    }
    document.write('</tr></table>');
}

var GroupSearchWin = null;
function pickGroup(Src) {
    document.getElementById('btnGet').disabled = false;
    if (GroupSearchWin == null) {
        GroupSearchWin = window.open("wsgroupmastersearch.aspx?src=" + Src,
            "_blank",
            "height=800, width=800, left=100, top=100, " +
            "location=no, menubar=no, resizable=no, " +
            "scrollbars=yes, titlebar=no, toolbar=no, fullscreen=yes", true);
    }
    else {
        GroupSearchWin.focus()
    }
}

var UserSearchWin = null;
function pickUser(Src) {
    document.getElementById('btnGet').disabled = false;
    if (UserSearchWin == null) {
        UserSearchWin = window.open("wsusermastersearch.aspx?src=" + Src,
            "_blank",
            "height=800, width=800, left=100, top=100, " +
            "location=no, menubar=no, resizable=no, " +
            "scrollbars=yes, titlebar=no, toolbar=no, fullscreen=yes", true);
    }
    else {
        UserSearchWin.focus()
    }
}

var UserTSearchWin = null;
function pickTUser(Src) {
    document.getElementById('btnTGet').disabled = false;
    if (UserTSearchWin == null) {
        UserTSearchWin = window.open("wsusermastersearch.aspx?src=" + Src,
            "_blank",
            "height=800, width=800, left=100, top=100, " +
            "location=no, menubar=no, resizable=no, " +
            "scrollbars=yes, titlebar=no, toolbar=no, fullscreen=yes", true);
    }
    else {
        UserTSearchWin.focus()
    }
}

var UserSearchWinSubmit = null;
function pickUserSubmit(Src) {
    document.getElementById('btnSubmit').disabled = false;
    if (UserSearchWinSubmit == null) {
        UserSearchWinSubmit = window.open("wsusermastersearch.aspx?src=" + Src,
            "_blank",
            "height=800, width=800, left=100, top=100, " +
            "location=no, menubar=no, resizable=no, " +
            "scrollbars=yes, titlebar=no, toolbar=no, fullscreen=yes", true);
    }
    else {
        UserSearchWinSubmit.focus()
    }
}

var AppSearchWin = null;
function pickApplication(Src) {
    document.getElementById('btnGet').disabled = false;
    if (AppSearchWin == null) {
        AppSearchWin = window.open("wsapplicationmastersearch.aspx?src=" + Src,
            "_blank",
            "height=800, width=800, left=100, top=100, " +
            "location=no, menubar=no, resizable=no, " +
            "scrollbars=yes, titlebar=no, toolbar=no, fullscreen=yes", true);
    }
    else {
        AppSearchWin.focus()
    }
}

var AppSearchWinSubmit = null;
function pickApplicationAreaSubmit(Src) {
    document.getElementById('btnSubmit').disabled = false;
    if (AppSearchWinSubmit == null) {
        AppSearchWinSubmit = window.open("wsapplicationareasearch.aspx?src=" + Src,
            "_blank",
            "height=800, width=800, left=100, top=100, " +
            "location=no, menubar=no, resizable=no, " +
            "scrollbars=yes, titlebar=no, toolbar=no, fullscreen=yes", true);
    }
    else {
        AppSearchWinSubmit.focus()
    }
}

var CustSearchWin = null;
function pickCust(Src) {
    document.getElementById('btnGet').disabled = false;
    if (CustSearchWin == null) {
        CustSearchWin = window.open("rxcustomermastersearch.aspx?src=" + Src,
            "_blank",
            "height=900, width=1000, left=100, top=100, " +
            "location=no, menubar=no, resizable=no, " +
            "scrollbars=yes, titlebar=no, toolbar=no, fullscreen=yes", true);
    }
    else {
        CustSearchWin.focus()
    }
}

function msg() {
    alert("Hello world!");
}

function WindowPrint() {
    window.print();
}

var windowObjectReference = null; // global variable

function closeReportWin() {
    if (!windowObjectReference) {
        //alert("myWindow' has never been opened!");
        window.location.href = "wsmainmenu.aspx";
    } else {
        if (windowObjectReference.closed) {
            //alert("myWindow' has been closed!");
            window.location.href = "wsmainmenu.aspx";
        } else {
            //alert("myWindow' has not been closed!");
            windowObjectReference.focus();
            windowObjectReference.close();
            window.location.href = "wsmainmenu.aspx";
        }
    }
    return false;
}

function openReportWin() {
    if (windowObjectReference == null || windowObjectReference.closed)
              /* if the pointer to the window object in memory does not exist
               or if such pointer exists but the window was closed */ {
        windowObjectReference = window.open("wsusermasterrpt.aspx", "UserMasterMainReportWindowName", "_blank", "width=1200,height=700,left=100,top=100,resizable=yes", "true");
        /* then create it. The new window will be created and
           will be brought on top of any other window. */
        //alert(windowObjectReference);
    }
    else {
        windowObjectReference.focus();
        /* else the window reference must exist and the window
           is not closed; therefore, we can bring it back on top of any other
           window with the focus() method. There would be no need to re-create
           the window or to reload the referenced resource. */
    };
    return false;
}

$(document).ready(function () {
    $("ul[id*=myid] li").click(function () {
        //alert($(this).html()); // gets innerHTML of clicked li
        //alert($(this).text()); // gets text contents of clicked li
        document.getElementById("getsource").value = $(this).text();
        //$("ul[id*=myid] li").focus
        //$('#myid li:nth-child(3) a').tab('show') // Select third tab
        //var btn = window.opener.document.getElementById('btnGet');
        //btn.click();
    });
});




