var controler = siteRoot + "transfpotencia/rangovaldatos/";
var error = [];
var arrayEmpr = [];
var arrayHistory = [];
var arrayHistoryVariacionCodigo = [];
var arrayVariacionCodigo = [];
var globalEmprCodi = 0;
var globalTipoComp = 2;
var globalEmprCodiVtp = 0;

$(document).ready(function () {
    $('#tab-container').easytabs({
        animate: false
    });

    $('#btnGrabarPorcentajeDefectoHistorico').click(function () {
        registrarDescuentoPorDefecto(1);
    });

    $('#btnGrabarPorcentajeDefectoVtp').click(function () {
        registrarDescuentoPorDefecto(2);
    });

    $('#btnGrabarPorcentajePorEmpresa').click(function () {
        registrarDescuentoPorEmpresa(1);
    });

    $('#btnGrabarPorcentajePorEmpresa2').click(function () {
        registrarDescuentoPorEmpresa(2);
    });

    $('#btnGrabarPorcentajePorCodigo').click(function () {
        registrarDescuentoPorCodigo();
    });

    $('#btnGrabarPorcentajePorCodigo2').click(function () {
        registrarDescuentoPorCodigo();
    });
   

    $("#mensaje").hide();

    $("#aPaso1").click();

});

function Fn_Change_Lista(tipo) {
    arrayEmpr = [];
    globalTipoComp = tipo == 2 ? 1 : 2;
    pintarPaginado(1);
    mostrarListado(1);
    mostrarCodigosVariacion(0, 1);
    arrayVariacionCodigo = [];
}

function buscarPorEmpresa() {
    pintarPaginado();
    mostrarListado(1);
    mostrarCodigosVariacion(0, 1);
}

function buscarPorCodigo() {
    if (globalEmprCodiVtp > 0) {
        mostrarCodigosVariacion(globalEmprCodiVtp, 1);
        pintarPaginadoVariacion(globalEmprCodiVtp);
    }
}


function buscarCodigosVariacion(emprCodi) {
    globalEmprCodiVtp = emprCodi;
    mostrarCodigosVariacion(emprCodi, 1);
    pintarPaginadoVariacion(emprCodi);
}

function Fn_Change_Amount(idEmp, idFila) {
    let valorPorcentaje = 0;
    let existe = false;
    if (globalTipoComp == 2) {
        if ($(".listadoVtp #txtPercent_" + idFila).val() == '') {
            arrayEmpr = arrayEmpr.filter(x => x.idEmp != idEmp);
            mostrarMensajeVacios();
            return;
        } else {
            valorPorcentaje = parseFloat($(".listadoVtp #txtPercent_" + idFila).val());
            if (valorPorcentaje > 100) {
                arrayEmpr = arrayEmpr.filter(x => x.idEmp != idEmp);
                mostrarMensajeVacios();
                return;
            }
        }
    } else {
        if ($(".listadoVtea #txtPercent_" + idFila).val() == '') {
            arrayEmpr = arrayEmpr.filter(x => x.idEmp != idEmp);
            mostrarMensajeVacios();
            return;
        } else {
            valorPorcentaje = parseFloat($(".listadoVtea #txtPercent_" + idFila).val());
            if (valorPorcentaje > 100) {
                arrayEmpr = arrayEmpr.filter(x => x.idEmp != idEmp);
                mostrarMensajeVacios();
                return;
            }
        }
    }
    
    for (let x = 0; x < arrayEmpr.length; x++) {
        if (arrayEmpr[x].idEmp == idEmp) {
            existe = true;
            if (valorPorcentaje == 0) {
                arrayEmpr.splice(x, 1);
                break;
            }
            if (arrayEmpr[x].desc != valorPorcentaje && valorPorcentaje !== '') {
                arrayEmpr[x].desc = valorPorcentaje;
            }
        }
    }

    if (!existe) {
        var objEmp = {
            idEmp : idEmp,
            desc : valorPorcentaje
        }
        arrayEmpr.push(objEmp);
    }
}

function mostrarMensajeVacios() {
    $("#mensaje").show();
    mostrarError("El porcentaje de variación no puede estar vacío, negativo, mayor a 100, ni contener caractéres especiales");
    setTimeout(() => {
        $("#mensaje").hide();
    }, 2000);
}

function Fn_Change_Amount_CodigoVariacion(idEmp,idCli,idBarra,codigVtp, idFila, varTipoComp) {
    let valorPorcentaje = 0;
    let existe = false;
    if (globalTipoComp == 2) {
        if ($(".listadoCodVtp #txtPercentVariacion_" + idFila).val() == '') {
            arrayVariacionCodigo = arrayVariacionCodigo.filter(x => x.codVtp != codigVtp);
            mostrarMensajeVacios();
            return;
        } else {
            valorPorcentaje = parseFloat($(".listadoCodVtp #txtPercentVariacion_" + idFila).val());
            if (valorPorcentaje > 100) {
                arrayVariacionCodigo = arrayVariacionCodigo.filter(x => x.codVtp != codigVtp);
                mostrarMensajeVacios();
                return;
            }
        }
    } else {
        if ($(".listadoCodVtea #txtPercentVariacion_" + idFila).val() == '') {
            arrayVariacionCodigo = arrayVariacionCodigo.filter(x => x.codVtp != codigVtp);
            mostrarMensajeVacios();
            return;
        } else {
            valorPorcentaje = parseFloat($(".listadoCodVtea #txtPercentVariacion_" + idFila).val());
            if (valorPorcentaje > 100) {
                arrayVariacionCodigo = arrayVariacionCodigo.filter(x => x.codVtp != codigVtp);
                mostrarMensajeVacios();
                return;
            }
        }
    }
    
    for (let x = 0; x < arrayVariacionCodigo.length; x++) {
        if (arrayVariacionCodigo[x].codVtp == codigVtp) {
            existe = true;
            if (valorPorcentaje == 0) {
                arrayVariacionCodigo.splice(x, 1);
                break;
            }
            if (arrayVariacionCodigo[x].desc != valorPorcentaje && valorPorcentaje !== '') {
                arrayVariacionCodigo[x].desc = valorPorcentaje;
            }
        }
    }

    if (!existe) {
        var objEmp = {
            idEmp: idEmp,
            idCli: idCli,
            idBarra: idBarra,
            codVtp: codigVtp,
            desc: valorPorcentaje,
            idTipoComp: varTipoComp
        }
        arrayVariacionCodigo.push(objEmp);
    }
}


function buscar() {

    pintarPaginado();
    mostrarListado(1);
}

function pintarPaginado(tipocomp = 2) {
    var buscaNomb = globalTipoComp == 2 ? $('#txtBuscarEmpresa1').val() : $('#txtBuscarEmpresa2').val();
    $.ajax({
        type: 'POST',
        url: controler + "paginado",
        data: {
            tipocomp: tipocomp,
            NroPagina: $('#hfNroPagina').val(),
            emprNomb: buscaNomb
        }, success: function (evt) {
            $('.paginadoTabla').html(evt);
            mostrarPaginado();
        },
        error: function () {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

function pintarPaginadoCodigo(tipocomp = 2) {
    var buscaCodigoVtp = globalTipoComp == 2 ? $('#txtBuscarCodigo').val() : $('#txtBuscarCodigo').val();
    $.ajax({
        type: 'POST',
        url: controler + "paginadovariacioncodigo",
        data: {
            emprCodi: emprCodi,
            NroPagina: $('#hfNroPaginaCodigoVariacion').val(),
            codiVtp: buscaCodigoVtp,
            tipocomp: globalTipoComp
        }, success: function (evt) {
            $('.paginadoTabla2').html(evt);
            mostrarPaginado1();
        },
        error: function () {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

function pintarBusqueda(nroPagina) {
    mostrarListado(nroPagina);
}

function pintarBusqueda1(nroPagina) {
    mostrarCodigosVariacion(globalEmprCodi, nroPagina);
}

function mostrarListado(nroPagina) {
    $('#hfNroPagina').val(nroPagina);
    var buscaNomb = globalTipoComp == 2 ? $('#txtBuscarEmpresa1').val() : $('#txtBuscarEmpresa2').val();
    $.ajax({
        type: 'POST',
        url: controler + "Lista",
        data: {
            tipocomp: globalTipoComp,
            NroPagina: $('#hfNroPagina').val(),
            emprNomb: buscaNomb
        },
        success: function (evt) {
            $('.listadoTabla').html(evt);
        },
        error: function () {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

function pintarPaginadoVariacion(emprCodi) {
    var buscaCodigoVtp = globalTipoComp == 2 ? $('#txtBuscarCodigo').val() : $('#txtBuscarCodigo2').val();
    $.ajax({
        type: 'POST',
        url: controler + "paginadovariacioncodigo",
        data: {
            emprCodi: emprCodi,
            NroPagina: $('#hfNroPaginaCodigoVariacion').val(),
            codiVtp: buscaCodigoVtp,
            tipocomp: globalTipoComp
        }, success: function (evt) {
            $('.paginadoTabla2').html(evt);
            mostrarPaginado1();
        },
        error: function () {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

function mostrarCodigosVariacion(emprCodi, nroPagina) {
    globalEmprCodi = emprCodi;
    $('#hfNroPaginaCodigoVariacion').val(nroPagina);
    var buscaCodigoVtp = globalTipoComp == 2 ? $('#txtBuscarCodigo').val() : $('#txtBuscarCodigo2').val();
        $.ajax({
            type: 'POST',
            url: controler + "ListVariacionCodigoByEmprCodi",
            data: {
                emprCodi: emprCodi,
                NroPagina: $('#hfNroPaginaCodigoVariacion').val(),
                codiVtp: buscaCodigoVtp,
                tipocomp: globalTipoComp
            },
            success: function (evt) {
                $('.listadoTabla2').html(evt);
            },
            error: function () {
                alert("Lo sentimos, ha ocurrido un error inesperado");
            }
        });
}


function registrarDescuentoPorDefecto(tipoComp) {
    const value = tipoComp == 1 ? $('#porcentajeDefectoHistorico').val() : $('#porcentajeDefectoVTEA').val();
    if (value != "" && parseInt(value) > 0 && parseInt(value) < 101) {
        var porcentaje = tipoComp == 1 ? $('#porcentajeDefectoHistorico').val() : $('#porcentajeDefectoVTEA').val();
        $.ajax({
            type: 'POST',
            url: controler + "registrardescuentopordefecto",
            data: { porcentaje: porcentaje, tipocomp: tipoComp },
            dataType: 'json',
            success: function (result) {
                if (result == "1") {
                    $("#mensaje").show();
                    mostrarExito('Registro creado correctamente');
                    setTimeout(() => {
                        $("#mensaje").hide();
                    }, 2000);
                }
                else {
                    $("#mensaje").show();
                    mostrarAlerta(result);
                    setTimeout(() => {
                        $("#mensaje").hide();
                    }, 2000);
                }
            },
            error: function () {
                $("#mensaje").show();
                mostrarError("Lo sentimos, se ha producido un error");
                setTimeout(() => {
                    $("#mensaje").hide();
                }, 2000);
            }
        });
    } else {
        $("#mensaje").show();
        mostrarError("El porcentaje no puede estar vacío, en cero, negativo, mayor a 100, ni contener caractéres especiales");
        setTimeout(() => {
            $("#mensaje").hide();
        }, 2000);
    }
    
};

function registrarDescuentoPorEmpresa(tipoComp) {
    if (!validarNegativosArray() && arrayEmpr.length > 0) {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        let arraydata = JSON.stringify(arrayEmpr);
        $.ajax({
            type: 'POST',
            url: controler + "registrardescuentoempresa",
            data: JSON.stringify({ tipocomp: tipoComp, datos: arraydata }),
            dataType: 'json',
            contentType: 'application/json',
            traditional: true,
            success: function (result) {
                if (result == "1") {
                    $("#mensaje").show();
                    mostrarExito('Registro creado correctamente');
                    arrayEmpr = [];
                    setTimeout(() => {
                        $("#mensaje").hide();
                    }, 2000);
                }
                else {
                    $("#mensaje").show();
                    mostrarAlerta(result);
                    arrayEmpr = [];
                    setTimeout(() => {
                        $("#mensaje").hide();
                    }, 2000);
                }
            },
            error: function (response) {
                $("#mensaje").show();
                mostrarError("Lo sentimos, se ha producido un error");
                arrayEmpr = [];
                setTimeout(() => {
                    $("#mensaje").hide();
                }, 2000);
            }
        });
    } else {
        $("#mensaje").show();

        if (arrayEmpr.length > 0) {
            mostrarError("La grilla de empresas tiene valores negativos o porcentaje mayor a 100, por favor corregir.");
        } else {
            mostrarAlerta("No se detectaron cambios para grabar");
        }
        
        arrayEmpr = [];
        setTimeout(() => {
            $("#mensaje").hide();
        }, 2000);
    }
    
}

function validarNegativosArray() {
    let negative = false;
    for (let x = 0; x < arrayEmpr.length; x++) {
        if (arrayEmpr[x].desc == "" || arrayEmpr[x].desc < 0 || arrayEmpr[x].desc > 100) {
            negative = true;
            break;
        }
    }
    return negative;
}
function validarNegativosArrayCodigo() {
    let negative = false;
    for (let x = 0; x < arrayVariacionCodigo.length; x++) {
        if (arrayVariacionCodigo[x].desc == "" || arrayVariacionCodigo[x].desc < 0 || arrayVariacionCodigo[x].desc > 100) {
            negative = true;
            break;
        }
    }
    return negative;
}


function registrarDescuentoPorCodigo() {
    if (!validarNegativosArray() && arrayVariacionCodigo.length > 0) {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        let arraydata = JSON.stringify(arrayVariacionCodigo);
        $.ajax({
            type: 'POST',
            url: controler + "registrardescuentocodigovariacion",
            data: JSON.stringify({ datos: arraydata }),
            dataType: 'json',
            contentType: 'application/json',
            traditional: true,
            success: function (result) {
                if (result == "1") {
                    $("#mensaje").show();
                    mostrarExito('Registro creado correctamente');
                    arrayVariacionCodigo = [];
                    setTimeout(() => {
                        $("#mensaje").hide();
                    }, 2000);
                }
                else {
                    arrayVariacionCodigo = [];
                    mostrarAlerta(result);
                }
            },
            error: function (response) {
                arrayVariacionCodigo = [];
                mostrarError("Lo sentimos, se ha producido un error");
            }
        });
    } else {
        $("#mensaje").show();

        if (arrayVariacionCodigo.length > 0) {
            mostrarError("La grilla de codigos tiene valores negativos o porcentaje mayor a 100, por favor corregir.");
        } else {
            mostrarAlerta("No se detectaron cambios para grabar");
        }
        arrayVariacionCodigo = [];
        setTimeout(() => {
            $("#mensaje").hide();
        }, 2000);
    }
    
}

function Fn_ListaHistorialPorcentaje(emprCodi) {
    $.ajax({
        type: 'POST',
        url: controler + "listahistorialvariacionempresa",
        data: {
            tipocomp: globalTipoComp == 2 ? 1 : 2,
            emprcodi: emprCodi
        },
        success: function (evt) {
            arrayHistory = JSON.parse(evt);
            Fn_MostrarHistorial();
        },
        error: function () {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

function Fn_ListaHistorialVariacionCodigo(codigovtp) {
    $.ajax({
        type: 'POST',
        url: controler + "listahistorialvariacioncodigo",
        data: {
            codigovtp: codigovtp,
            tipocomp: globalTipoComp == 2 ? 1 : 2
        },
        success: function (evt) {
            arrayHistoryVariacionCodigo = JSON.parse(evt);
            Fn_MostrarHistorialVariacionCodigo();
        },
        error: function () {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}


function Fn_MostrarHistorial() {
    
    if (arrayHistory.length > 0) {
        var html = '<span class="button b-close"><span>X</span></span>';
        html += '<p style="border-bottom: 1px solid #6ba9d0;"><b style="color: #6ba9d0;font-size: 16px;">Historial de Variacion por Empresa<div style="height: 5px;"></div></b><p>';
        html += '<table border="0" class="pretty tabla-icono" id="tablaHistorial">'
        html += '<thead>'
        html += '<tr>'
        html += '<th>Empresa</th>'
        html += '<th>Porcentaje Potencia</th>'
        html += '<th>Vigencia</th>'
        html += '<th>Estado</th>'
        html += '<th>Usuario Modificación</th>'
        html += '<th>Fecha Modificación</th>'
        html += '</tr>'
        html += '</thead>'
        html += '<tbody>'
        for (let x = 0; x < arrayHistory.length; x++) {
            html += '<tr>'
            html += '<td class="text_left">' + arrayHistory[x].Emprnomb + '</td>'
            html += '<td class="text_left">' + arrayHistory[x].Varempprocentaje + '</td>'
            html += '<td class="text_left">' + convertirFecha(arrayHistory[x].Varempvigencia) + '</td>'
            html += '<td class="text_left">' + arrayHistory[x].Varempestado + '</td>'
            html += '<td class="text_left">' + arrayHistory[x].Varempusumodificacion + '</td>'
            html += '<td class="text_left">' + convertirFecha(arrayHistory[x].Varempfeccreacion) + '</td>'
            html += '</tr>'
        }
        html += '</tbody>'
        html += '</table>';

        Fn_Mostrar_Popup(html);
    }
}

function Fn_MostrarHistorialVariacionCodigo() {

    if (arrayHistoryVariacionCodigo.length > 0) {
        var html = '<span class="button b-close"><span>X</span></span>';
        html += '<p style="border-bottom: 1px solid #6ba9d0;"><b style="color: #6ba9d0;font-size: 16px;">Historial de Variacion por Código<div style="height: 5px;"></div></b><p>';
        html += '<table border="0" class="pretty tabla-icono" id="tablaHistorialCodigo">'
        html += '<thead>'
        html += '<tr>'
        html += '<th>Empresa</th>'
        html += globalTipoComp == 2 ? '<th>CodigoVTP</th>' : '<th>CodigoVTEA</th>'
        html += globalTipoComp == 2 ? '<th>Porcentaje Potencia</th>' : '<th> Porcentaje Energía</th>'
        html += '<th>Estado</th>'
        html += '<th>Usuario Modificación</th>'
        html += '<th>Fecha Modificación</th>'
        html += '</tr>'
        html += '</thead>'
        html += '<tbody>'
        for (let x = 0; x < arrayHistoryVariacionCodigo.length; x++) {
            html += '<tr>'
            html += '<td class="text_left">' + arrayHistoryVariacionCodigo[x].EmprNomb + '</td>'
            html += '<td class="text_left">' + arrayHistoryVariacionCodigo[x].VarCodCodigoVtp + '</td>'
            html += '<td class="text_left">' + arrayHistoryVariacionCodigo[x].VarCodPorcentaje + '</td>'
            html += '<td class="text_left">' + arrayHistoryVariacionCodigo[x].VarCodEstado + '</td>'
            html += '<td class="text_left">' + arrayHistoryVariacionCodigo[x].VarCodUsuModificacion + '</td>'
            html += '<td class="text_left">' + convertirFecha(arrayHistoryVariacionCodigo[x].VarCodFecCreacion) + '</td>'
            html += '</tr>'
        }
        html += '</tbody>'
        html += '</table>';

        Fn_Mostrar_Popup(html);
    }
}

function Fn_Mostrar_Popup(html) {
    $('#popup').html(html);
    $('#popup').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
}


function convertirFecha(fecha) {
    let fec = fecha.split('T');
    console.log(fec[0] + " " + fec[1]);
    return fec[0] + " " + fec[1];
}