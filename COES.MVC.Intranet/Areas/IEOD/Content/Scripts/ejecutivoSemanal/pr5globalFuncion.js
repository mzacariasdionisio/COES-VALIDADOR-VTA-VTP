////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Funciones de validación

//variables globales
var resultFiltro = false;

//validar filtros por cadenas con formart "," y "/"
function valida1(str) {
    var filtro = filtro || [];
    str = str.split("|");

    $.each(str, function (i, l) {
        var str2 = l.split(";");

        filtro.push({ id: str2[0], mensaje: str2[1] })
    })

    for (var i in filtro) {

        if (filtro[i].id.trim() != "") {
            resultFiltro = true;
        }
        else {
            alert(filtro[i].mensaje);
            resultFiltro = false;
            break;
        }
    }
    filtro.push();
}

//validar filtros por Arrays
function validarFiltros(filtro) {
    for (var i in filtro) {
        if (filtro[i].id != "") {
            resultFiltro = true;
        }
        else {
            alert(filtro[i].mensaje);
            resultFiltro = false;

            break;
        }
    }
    filtro = [];
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Funciones para obtener el valor de los inputs
function getCodigoVersion() {
    var valor = $('#cboVersion').val();
    return valor || 0;
}


//Obtener formato html
function getHtmlSaltoLinea(str) {
    return str.replace(/(?:\r\n|\r|\n)/g, '<br>');
}