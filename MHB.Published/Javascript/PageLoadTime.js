NavName = navigator.appName.substring(0, 3);
NavVersion = navigator.appVersion.substring(0, 1);
if (NavName != "Mic" || NavVersion >= 4) {
    entree = new Date;
    entree = entree.getTime();
}

function calculateloadgingtime() {
    if (NavName != "Mic" || NavVersion >= 4) {
        fin = new Date;
        fin = fin.getTime();
        secondes = (fin - entree) / 1000;
        window.status = 'Page loaded in ' + secondes + ' second(s).';
        document.getElementById("loadgingpage").innerHTML = "Page loaded in " + secondes + " second(s).";
    }
}
//window.onload = calculateloadgingtime;