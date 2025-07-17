
addAntiForgeryToken = function (data) {
    data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
    return data;
};

mostrarExito = function (mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-exito');
}

mostrarError = function (mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-error');
}

mostrarAlerta = function (mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-alert');
}

mostrarMensaje = function (mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-message');
}

loadInfoFile = function (fileName, fileSize) {
    $('#fileInfo').html(fileName + " (" + fileSize + ")");
}

loadValidacionFile = function (mensaje) {
    $('#fileInfo').html(mensaje);
}

mostrarProgreso = function (porcentaje) {
    $('#progreso').text(porcentaje + "%");
}

SelectAll = function () {
    var cont = frmBrowse.Count.value;
    var xSel;
    var i;
    xSel = frmBrowse.chkAll.checked;
    if (cont > 1) {
        for (i = 0; i < cont; i++) {
            eval('frmBrowse.chkItem[' + i + '].checked=' + xSel + ';');
        }
    }
    else {
        eval('frmBrowse.chkItem.checked=' + xSel + ';');
    }
}

checkMark = function () {
    debugger;
    var cont = frmBrowse.Count.value;
    var xSel = "0";
    var i;
    if (cont > 1) {
        for (i = 0; i < cont; i++) {
            if (eval('frmBrowse.chkItem[' + i + '].checked')) {
                //console.log("A:" + eval('frmBrowse.chkItem[' + i + '].value'));
                xSel = xSel + "," + eval('frmBrowse.chkItem[' + i + '].value');
            }
        }
    }
    else if (cont == 1) {
        if (eval('frmBrowse.chkItem.checked')) {
            //console.log("B:" + eval('frmBrowse.chkItem[' + i + '].value'));
            xSel = xSel + "," + eval('frmBrowse.chkItem.value');
        }
    }
    return xSel;
}