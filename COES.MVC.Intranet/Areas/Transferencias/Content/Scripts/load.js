abrirPopup = function () {
    $('#popup').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
}

generarExcel = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'generarexcel',
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                window.location = controlador + 'abrirexcel';
            }
            if (result == -1) {
                mostrarError();
            }
        },
        error: function () {
            mostrarError();
        }
    });
}

generarWord = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'generarword',
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                window.location = controlador + 'abrirword';
            }
            if (result == -1) {
                mostrarError();
            }
        },
        error: function () {
            mostrarError();
        }
    });
}

generarPdf = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'generarpdf',
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                window.location = controlador + 'abrirpdf';
            }
            if (result == -1) {
                mostrarError();
            }
        },
        error: function () {
            mostrarError();
        }
    });
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

mostrarError = function () {
    alert("Ha ocurrido un error.");
}

mostrarMensaje = function (mensaje) {
    alert(mensaje);
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
                console.log("A:" + eval('frmBrowse.chkItem[' + i + '].value'));
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