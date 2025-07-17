var controlador = siteRoot + 'migraciones/parametro/';
var OPCION_VER = 1;
var OPCION_EDITAR = 2;
var OPCION_NUEVO = 3;
var ancho = 900;

var LISTA_AGRUPACION_CONCEPTO = null;

var AGRUP_FUENTE = 1;

$(function () {
    $('#btnConsultar').on('click', function () {
        listadoAgrupacion
    });

    ancho = $('#mainLayout').width() - 50;

    listadoAgrupacion();

    $('#btnCrearAgrup').on('click', function () {
        nuevaAgrupacion();
    });
});

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Agrupaciones
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function listadoAgrupacion() {
    $.ajax({
        type: 'POST',
        url: controlador + "AgrupacionLista",
        data: {
            agrupfuente: 1
        },
        success: function (evt) {
            $('#listadoAgrupacion').css("width", ancho + "px");
            $('#listadoAgrupacion').html(evt);

            $('#tabla_agrupacion').dataTable({
                "sDom": 'ft',
                "ordering": false,
                "iDisplayLength": -1
            });
        },
        error: function (err) {
            mostrarError();
        }
    });
};

function eliminarAgrupacion(id) {
    if (confirm('¿Desea eliminar la Agrupación?')) {
        $.ajax({
            type: 'POST',
            dataType: 'json',
            traditional: true,
            url: controlador + 'AgrupacionEliminar',
            data: {
                id: id
            },
            cache: false,
            success: function (result) {
                if (result.Resultado == '-1') {
                    alert('Ha ocurrido un error:' + result.Mensaje);
                } else {
                    alert("Se eliminó correctamente la Agrupación");
                    listadoAgrupacion();
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error.');
            }
        });
    }
}

function nuevaAgrupacion() {
    formularioAgrupacion(0, OPCION_NUEVO);
}

function verAgrupacion(id) {
    formularioAgrupacion(id, OPCION_VER);
}

function editarAgrupacion(id) {
    formularioAgrupacion(id, OPCION_EDITAR);
}

function formularioAgrupacion(id, opcion) {
    LISTA_AGRUPACION_CONCEPTO = null;

    $.ajax({
        type: 'POST',
        dataType: 'json',
        traditional: true,
        url: controlador + "AgrupacionObjeto",
        data: {
            id: id
        },
        success: function (obj) {
            $('#mensaje').css("display", "none");

            inicializarFormulario(opcion, id, obj);

            setTimeout(function () {
                $('#popupAgrupacion').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);

        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function inicializarFormulario(opcion, id, obj) {
    $("#frmAgrpNomb").prop('disabled', 'disabled');
    $("#btnAgregarConcepto").hide();

    $("#hfCodigoAgrupacion").val(id);
    $("#frmAgrpNomb").val(obj.Agrupacion.Agrupnombre);

    LISTA_AGRUPACION_CONCEPTO = obj.ListaAgrupacionConcepto;
    generarTablaAgrupacionConcepto(opcion);

    switch (opcion) {
        case OPCION_VER:
            break;
        case OPCION_EDITAR:
            $("#frmAgrpNomb").removeAttr('disabled');
            $("#btnAgregarConcepto").show();
            break;
        case OPCION_NUEVO:
            $("#frmAgrpNomb").removeAttr('disabled');

            $("#btnAgregarConcepto").show();

            $("#tdGuardar").show();
            $("#tdCancelar").show();
            break;
    }

    $("#btnAgregarConcepto").unbind();
    $('#btnAgregarConcepto').click(function () {
        abrirPopupConcepto();
    });

    $("#btnAgrpGuardar").unbind();
    $('#btnAgrpGuardar').click(function () {
        guardarAgrupacion();
    });

    $("#btnAgrpCancelar").unbind();
    $('#btnAgrpCancelar').click(function () {
        cerrarPopupAgrupacion();
    });
}

function abrirPopupConcepto() {
    $("#hfCodigoPropiedad").val(0);
    $("#hfDescPropiedad").val("");
    $("#descripcionPropiedad").val("");
    $("#hfItemUnidad").val("");
    $("#hfItemTipo").val("");

    $("#frmPropNomb").val("");

    cargarTableConcepto();

    $("#btnCnpGuardar").unbind();
    $('#btnCnpGuardar').click(function () {
        agregarConceptoToParametro();
    });

    $("#cbAgrpCategoria").unbind();
    $("#cbAgrpCategoria").change(function () {
        cargarTableConcepto();
    });

    $("#btnCnpCancelar").unbind();
    $('#btnCnpCancelar').click(function () {
        cerrarpopupConcepto();
    });

    setTimeout(function () {
        $('#popupConcepto').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown'
        });
    }, 500);
}

function cerrarpopupConcepto() {
    $('#popupConcepto').bPopup().close();
}

function cargarTableConcepto() {
    $("#hfCodigoPropiedad").val(0);
    $("#hfDescPropiedad").val("");
    $("#descripcionPropiedad").val("");
    $("#hfItemUnidad").val("");
    $("#hfItemTipo").val("");

    $("#frmPropNomb").val("");

    $("#div_tabla_pr_concepto").hide();
    $('#div_tabla_pr_concepto').html('');

    $.ajax({
        type: "POST",
        url: controlador + "ListaPrConcepto",
        data: {
            catecodi: $("#cbAgrpCategoria").val()
        },
        global: false,
        success: function (result) {
            if (result.Resultado == '-1') {
                alert('Ha ocurrido un error:' + result.Mensaje);
            } else {
                var htmlTable = "<table id='tabla_pr_concepto' class='pretty tabla-icono'><thead><tr><th width='12%'>Código</th><th width='18%'>Categoría de Grupo</th><th width='40%'>Nombre</th><th width='18%'>Abreviatura</th><th width='6%'>Unidad</th><th width='6%'>Tipo</th></tr></thead>";

                var tbody = '';
                for (var i = 0; i < result.ListaConcepto.length; i++) {
                    var item = result.ListaConcepto[i];
                    item.Catenomb = convertirStringToHtml(item.Catenomb);
                    item.Concepabrev = convertirStringToHtml(item.Concepabrev);
                    item.Concepdesc = convertirStringToHtml(item.Concepdesc);
                    item.Concepunid = convertirStringToHtml(item.Concepunid);
                    item.Conceptipo = convertirStringToHtml(item.Conceptipo);


                    var fila = "<tr onclick=\"seleccionarPrConcepto(" + item.Concepcodi + ",'" + item.Catenomb + "','" + item.Concepabrev
                        + "','" + item.Concepdesc + "','" + item.Concepunid + "', '" + item.Conceptipo + "');\" style='cursor:pointer'>";
                    fila += "<td style='text-align: center; padding-top: 7px; padding-bottom: 7px;'>" + item.Concepcodi + "</td>";
                    fila += "<td style='text-align: center'>" + item.Catenomb + "</td>";
                    fila += "<td style='text-align: left'>" + item.Concepdesc + "</td>";
                    fila += "<td style='text-align: left'>" + item.Concepabrev + "</td>";
                    fila += "<td style='text-align: center'>" + item.Concepunid + "</td>";
                    fila += "<td style='text-align: center'>" + item.Conceptipo + "</td>";
                    fila + "</tr>";
                    tbody += fila;
                }

                htmlTable += tbody + "<tbody></tbody></table>";
                $('#div_tabla_pr_concepto').html(htmlTable);
                $('#tabla_pr_concepto').dataTable({
                    "sPaginationType": "full_numbers",
                    "destroy": "true",
                    "bInfo": false,
                    "bLengthChange": false,
                    "sDom": 'fpt',
                    "ordering": true,
                    "order": [[2, "asc"]],
                    "iDisplayLength": 15
                });

                $("#div_tabla_pr_concepto").show();

            }
        },
        error: function (req, status, error) {
            alert("Ha ocurrido un error.");
        }
    });
}

function guardarAgrupacion() {

    if ($("#frmAgrpNomb").val().length > 200) {
        alert("El nombre de agrupación contiene más de 200 caracteres");
        return false;
    }

    var entity = getObjetoJsonAgrupacion();
    if (confirm('¿Desea guardar la agrupación?')) {
        var msj = validarAgrupacion(entity);

        if (msj == "") {
            $.ajax({
                type: 'POST',
                dataType: 'json',
                traditional: true,
                url: controlador + 'AgrupacionGuardar',
                data: {
                    id: entity.id,
                    nombre: entity.Nombre,
                    listaSelec: entity.ListaConcepcodi,
                    agrupfuente: AGRUP_FUENTE
                },
                cache: false,
                success: function (result) {
                    if (result.Resultado == '-1') {
                        alert('Ha ocurrido un error: ' + result.Mensaje);
                    } else {
                        alert("Se guardó correctamente la Agrupación");
                        cerrarPopupAgrupacion();
                        listadoAgrupacion();
                    }
                },
                error: function (err) {
                    alert('Ha ocurrido un error.');
                }
            });
        } else {
            alert(msj);
        }
    }
}

function getObjetoJsonAgrupacion() {
    var obj = {};

    obj.id = parseInt($("#hfCodigoAgrupacion").val()) || 0;
    obj.Nombre = $("#frmAgrpNomb").val();

    var listaConcepcodi = [];

    if (LISTA_AGRUPACION_CONCEPTO != null) {
        for (var i = 0; i < LISTA_AGRUPACION_CONCEPTO.length; i++) {
            listaConcepcodi.push(LISTA_AGRUPACION_CONCEPTO[i].Concepcodi);
        }
    }

    obj.ListaConcepcodi = listaConcepcodi;

    return obj;
}

function validarAgrupacion(obj) {
    var msj = "";

    if (obj.Nombre == null || obj.Nombre.trim() == "") {
        msj += "Debe ingresar nombre." + "\n";
    }

    if (obj.ListaConcepcodi == null || obj.ListaConcepcodi.length == 0) {
        msj += "Debe selecionar al menos un Concepto de Grupo." + "\n";
    }

    return msj;
}

function cerrarPopupAgrupacion() {
    $('#popupAgrupacion').bPopup().close();
}

function seleccionarPrConcepto(Concepcodi, Catenomb, Concepabrev, Concepdesc, Concepunid, Conceptipo) {
    $("#hfCodigoPropiedad").val(Concepcodi);
    $("#hfDescPropiedad").val(convertirStringToHtml(Concepdesc));
    $("#hfAbrevPropiedad").val(convertirStringToHtml(Concepabrev));
    $("#descripcionPropiedad").val(Concepcodi + " - " + convertirStringToHtml(Concepdesc));
    $("#hfItemUnidad").val(convertirStringToHtml(Concepunid));
    $("#hfItemTipo").val(convertirStringToHtml(Conceptipo));

    $("#frmPropNomb").val(convertirStringToHtml(Concepdesc));
}

function agregarConceptoToParametro() {
    var codigoNewConcepto = parseInt($("#hfCodigoPropiedad").val()) || 0;
    if (codigoNewConcepto > 0) {
        var concepto = buscarConcepcodi(codigoNewConcepto);

        if (concepto == null) {
            var objConcepto = {};
            objConcepto.Concepcodi = codigoNewConcepto;
            objConcepto.Concepdesc = $("#hfDescPropiedad").val();
            objConcepto.Concepabrev = $("#hfAbrevPropiedad").val();
            objConcepto.Concepunid = $("#hfItemUnidad").val();
            objConcepto.Conceptipo = $("#hfItemTipo").val();

            if (LISTA_AGRUPACION_CONCEPTO == null) {
                LISTA_AGRUPACION_CONCEPTO = [];
            }

            LISTA_AGRUPACION_CONCEPTO.push(objConcepto);
        }
    }

    generarTablaAgrupacionConcepto();
    cerrarpopupConcepto();
}

function buscarConcepcodi(codigo) {
    if (LISTA_AGRUPACION_CONCEPTO != null) {
        for (var i = 0; i < LISTA_AGRUPACION_CONCEPTO.length; i++) {
            if (LISTA_AGRUPACION_CONCEPTO[i].Concepcodi == codigo) {
                return LISTA_AGRUPACION_CONCEPTO[i];
            }
        }
    }

    return null;
}

function generarTablaAgrupacionConcepto(opcion) {
    $("#tabla_concepcodi").html("");
    var strHtml = '';

    if (LISTA_AGRUPACION_CONCEPTO == null || LISTA_AGRUPACION_CONCEPTO.length == 0) {
        LISTA_AGRUPACION_CONCEPTO = [];
        $("#tabla_concepcodi").hide();
    } else {
        $("#tabla_concepcodi").show();
    }

    strHtml += '<thead>';
    strHtml += '<tr>';
    strHtml += '<th class="tbform-control">Código</th>';
    strHtml += '<th class="tbform-control">Nombre</th>';
    strHtml += '<th class="tbform-control">Abreviatura</th>';
    strHtml += '<th class="tbform-control">Unidad</th>';
    strHtml += '<th class="tbform-control">Tipo</th>';

    if (opcion == OPCION_VER) {
    }
    else {
        strHtml += '<th class="tbform-control"></th>';
        strHtml += '</tr>';
    }

    strHtml += '</thead>';

    strHtml += '<tbody>';
    for (var i = 0; i < LISTA_AGRUPACION_CONCEPTO.length; i++) {
        strHtml += '<tr>';
        strHtml += '<td class="tbform-control">' + LISTA_AGRUPACION_CONCEPTO[i].Concepcodi + '</td>';
        strHtml += '<td class="tbform-control">' + LISTA_AGRUPACION_CONCEPTO[i].Concepdesc + '</td>';
        strHtml += '<td class="tbform-control">' + LISTA_AGRUPACION_CONCEPTO[i].Concepabrev + '</td>';
        strHtml += '<td class="tbform-control">' + LISTA_AGRUPACION_CONCEPTO[i].Concepunid + '</td>';
        strHtml += '<td class="tbform-control">' + LISTA_AGRUPACION_CONCEPTO[i].Conceptipo + '</td>';
        if (opcion == OPCION_VER) {
            //strHtml += '<td class="tbform-control">';
            //strHtml += '</td>';
        } else {
            strHtml += '<td class="tbform-control">';
            strHtml += '<input type="hidden" id="fila_concepto' + LISTA_AGRUPACION_CONCEPTO[i].Concepcodi + '" value="' + LISTA_AGRUPACION_CONCEPTO[i].Concepcodi + '" />';
            strHtml += '<input type="button" value="Quitar" onclick="quitarConcepto(' + LISTA_AGRUPACION_CONCEPTO[i].Concepcodi + ')" >';
            strHtml += '</td>';
        }
        strHtml += '</tr>';
    }
    strHtml += '</tbody>';

    $("#tabla_concepcodi").html(strHtml);
}

function quitarConcepto(concepcodi) {
    //generar nueva lista sin el elemento
    var listaNueva = [];
    var objElimi = {};
    for (var i = 0; i < LISTA_AGRUPACION_CONCEPTO.length; i++) {
        if (LISTA_AGRUPACION_CONCEPTO[i].Concepcodi == concepcodi) {
            objElimi = LISTA_AGRUPACION_CONCEPTO[i];
        } else {
            listaNueva.push(LISTA_AGRUPACION_CONCEPTO[i]);
        }
    }

    LISTA_AGRUPACION_CONCEPTO = listaNueva;

    generarTablaAgrupacionConcepto();
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Util
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function mostrarError() {
    $('#textoMensaje').css("display", "block");
    $('#textoMensaje').removeClass();
    $('#textoMensaje').addClass('action-alert');
    $('#textoMensaje').text("Ha ocurrido un error");
}

function getCodigoEmpresa() {
    return parseInt($("#cbEmpresa").val()) || 0;
}

function getCodigoCategoria() {
    return parseInt($("#cbCategoria").val()) || 0;
}

function getEstado() {
    return "S";
}

function convertirStringToHtml(str) {
    if (str != undefined && str != null) {
        //return str.replace(/&/g, "&amp;").replace(/>/g, "&gt;").replace(/</g, "&lt;").replace(/"/g, "&quot;");
        return str.replace(/>/g, "&gt;").replace(/</g, "&lt;").replace(/"/g, "&quot;");
    }

    return "";
}