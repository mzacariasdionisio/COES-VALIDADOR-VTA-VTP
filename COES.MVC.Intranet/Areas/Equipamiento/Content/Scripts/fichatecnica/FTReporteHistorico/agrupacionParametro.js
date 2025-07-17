var controlador = siteRoot + 'Equipamiento/FTReporteHistorico/';

var AGRUP_FUENTE = 2;

var OPCION_VER = 1;
var OPCION_EDITAR = 2;
var OPCION_NUEVO = 3;
var ancho = 900;

var ORIGEN_TIPO_GRUPO = 1;
var ORIGEN_TIPO_EQUIPO = 2;

var LISTA_EQ_PROPIEDAD = null;
var LISTA_AGRUPACION_CONCEPTO = null;

$(document).ready(function () {
    $('#btnConsultar').on('click', function () {
        listadoAgrupacion
    });

    ancho = $('#mainLayout').width() - 50;

    listadoAgrupacion();

    $('#btnCrearAgrup').on('click', function () {
        nuevaAgrupacion();
    });

    $('input[type=radio][name=origen]').unbind();
    $('input[type=radio][name=origen]').change(function () {
        $("#div_tabla_eq_propiedad").hide();
        $("#div_tabla_pr_concepto").hide();

        mostrarOrigenDatos();
    });

    $("#cbFamiliaEquipo").unbind();
    $("#cbFamiliaEquipo").change(function () {
        cargarTablePropiedad();
        //mostrarOrigenDatos();
        //mostrarFichaPadre();
    });

    $("#cbAgrpCategoria").unbind();
    $("#cbAgrpCategoria").change(function () {
        cargarTableConcepto();
    });
});

function asignarEventosCheckBox() {
    $('.checkAll-concept').click(function () {
        if (this.checked) {
            $(".checkboxes-concept").prop("checked", true);
        } else {
            $(".checkboxes-concept").prop("checked", false);
        }
    });

    $(".checkboxes-concept").click(function () {
        var numberOfCheckboxes = $(".checkboxes-concept").length;
        var numberOfCheckboxesChecked = $('.checkboxes-concept:checked').length;
        if (numberOfCheckboxes == numberOfCheckboxesChecked) {
            $(".checkAll-concept").prop("checked", true);
        } else {
            $(".checkAll-concept").prop("checked", false);
        }
    });

    $('.checkAll-equip').click(function () {

        if (this.checked) {
            $(".checkboxes-equip").prop("checked", true);
        } else {
            $(".checkboxes-equip").prop("checked", false);
        }
    });

    $(".checkboxes-equip").click(function () {
        var numberOfCheckboxes = $(".checkboxes-equip").length;
        var numberOfCheckboxesChecked = $('.checkboxes-equip:checked').length;
        if (numberOfCheckboxes == numberOfCheckboxesChecked) {
            $(".checkAll-equip").prop("checked", true);
        } else {
            $(".checkAll-equip").prop("checked", false);
        }
    });
}

function mostrarOrigenDatos() {
    var origen = $('input[name="origen"]:checked').val();
    if (origen == ORIGEN_TIPO_EQUIPO) {
        $("#cbAgrpCategoria").hide();
        $("#cbFamiliaEquipo").show();

        cargarTablePropiedad();
    } else {
        $("#cbFamiliaEquipo").hide();
        $("#cbAgrpCategoria").show();

        cargarTableConcepto();
    }
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Agrupaciones
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function listadoAgrupacion() {
    $.ajax({
        type: 'POST',
        url: controlador + "AgrupacionLista",
        data: {
            agrupfuente: AGRUP_FUENTE
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
            $("#tdAgrpGuardar").hide();
            break;
        case OPCION_EDITAR:
            $("#tdAgrpGuardar").show();
            $("#frmAgrpNomb").removeAttr('disabled');
            $("#btnAgregarConcepto").show();
            break;
        case OPCION_NUEVO:
            $("#tdAgrpGuardar").show();
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

//detalle
function abrirPopupConcepto() {
    $("#hfCodigoPropiedad").val(0);
    $("#hfDescPropiedad").val("");
    $("#descripcionPropiedad").val("");
    $("#hfItemUnidad").val("");
    $("#hfItemTipo").val("");

    $("#frmPropNomb").val("");

    mostrarOrigenDatos();

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
            speed: 20,
            transition: 'slideDown',
        });
    }, 10);
}

function cerrarpopupConcepto() {
    $('#popupConcepto').bPopup().close();
}

function cargarTablePropiedad() {

    $("#div_tabla_eq_propiedad").hide();
    $("#div_tabla_pr_concepto").hide();

    var famcodi = $("#cbFamiliaEquipo").val();

    if (famcodi != -2) {
        $.ajax({
            type: "POST",
            url: controlador + "ListaEqPropiedad",
            data: {
                famcodi: famcodi
            },
            global: false,
            success: function (result) {
                if (result.Resultado == '-1') {
                    alert('Ha ocurrido un error:' + result.Mensaje);
                } else {
                    var htmlTable = `<table id='tabla_eq_propiedad' style='width: 800px;' class='pretty tabla-icono'>
                                        <thead>
                                            <tr>
                                                <th style='width:50px'>Sel.<input type="checkbox" name="checkAll-equip" class="checkAll-equip"></th>
                                                <th style='width:50px'>Código</th>
                                                <th style='width:150px'>Tipo de Equipo</th>
                                                <th style='width:150px'>Nombre</th>
                                                <th style='width:150px'>Nombre Ficha Técnica</th>
                                                <th style='width:80px'>Abreviatura</th>
                                                <th style='width:50px'>Unidad</th>
                                                <th style='width:50px'>Tipo</th>
                                            </tr>
                                        </thead>`;

                    var tbody = `<tbody>`;

                    for (var i = 0; i < result.ListaPropiedad.length; i++) {
                        var item = result.ListaPropiedad[i];
                        item.NombreFamilia = convertirStringToHtml(item.NombreFamilia);
                        //item.Famnomb = convertirStringToHtml(item.Famnomb);
                        item.Propabrev = convertirStringToHtml(item.Propabrev);
                        item.Propnomb = convertirStringToHtml(item.Propnomb);
                        item.Propnombficha = convertirStringToHtml(item.Propnombficha);
                        item.Propunidad = convertirStringToHtml(item.Propunidad);
                        item.Propdefinicion = convertirStringToHtml(item.Propdefinicion);
                        item.Proptipo = convertirStringToHtml(item.Proptipo);

                        var htmlChecked = '';
                        if (tieneCheckFilaListaConcepto(item.Propcodi, ORIGEN_TIPO_EQUIPO)) htmlChecked = ' checked ';

                        var fila = "<tr>";
                        fila += `<td style='text-align: center'><input type="checkbox" id="chkItemPropiedad" class="checkboxes-equip" ${htmlChecked}></td>`;

                        fila += "<td style='text-align: center; padding-top: 7px; padding-bottom: 7px;'>" + item.Propcodi + "</td>";
                        fila += "<td style='text-align: left'>" + item.NombreFamilia + "</td>";
                        fila += "<td style='text-align: left'>" + item.Propnomb + "</td>";
                        fila += "<td style='text-align: left'>" + item.Propnombficha + "</td>";
                        fila += "<td style='text-align: left'>" + item.Propabrev + "</td>";
                        fila += "<td style='text-align: center'>" + item.Propunidad + "</td>";
                        fila += "<td style='text-align: center'>" + item.Proptipo + "</td>";
                        fila += "</tr>";
                        tbody += fila;
                    }
                    htmlTable += tbody + "</tbody></table>";
                    $('#div_tabla_eq_propiedad').html(htmlTable);
                    $("#div_tabla_pr_concepto").html("");

                    $("#div_tabla_eq_propiedad").show();

                    setTimeout(function () {
                        $('#tabla_eq_propiedad').dataTable({
                            "scrollY": 480,
                            "scrollX": true,
                            "sDom": 'ft',
                            "ordering": true,
                            "iDisplayLength": -1
                        });
                    }, 250);

                    asignarEventosCheckBox();
                }
            },
            error: function (req, status, error) {
                alert("Ha ocurrido un error.");
            }
        });
    } else {
        $('#div_tabla_eq_propiedad').html('');
        $("#div_tabla_eq_propiedad").show();
    }
}

function cargarTableConcepto() {
    $("#hfCodigoPropiedad").val(0);
    $("#hfDescPropiedad").val("");
    $("#descripcionPropiedad").val("");
    $("#hfItemUnidad").val("");
    $("#hfItemTipo").val("");

    $("#frmPropNomb").val("");

    $("#div_tabla_eq_propiedad").hide();
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
                var htmlTable = `<table id='tabla_pr_concepto' style='width: 800px;' class='pretty tabla-icono' >
                                    <thead>
                                        <tr>
                                            <th style='width: 50px'>Sel.<input type="checkbox" name="checkAll-concept" class="checkAll-concept"></th>
                                            <th style='width: 50px'>Código</th>
                                            <th style='width: 150px'>Categoría de Grupo</th>
                                            <th style='width: 150px'>Nombre</th>
                                            <th style='width: 150px'>Nombre Ficha Técnica</th>
                                            <th style='width: 80px'>Abreviatura</th>
                                            <th style='width: 50px'>Unidad</th>
                                            <th style='width: 50px'>Tipo</th>
                                        </tr>
                                     </thead>`;

                var tbody = `<tbody>`;
                for (var i = 0; i < result.ListaConcepto.length; i++) {
                    var item = result.ListaConcepto[i];
                    item.Catenomb = convertirStringToHtml(item.Catenomb);
                    item.Concepdesc = convertirStringToHtml(item.Concepdesc);
                    item.Concepnombficha = convertirStringToHtml(item.Concepnombficha);
                    item.Concepabrev = convertirStringToHtml(item.Concepabrev);
                    item.Concepunid = convertirStringToHtml(item.Concepunid);
                    item.Conceptipo = convertirStringToHtml(item.Conceptipo);

                    var htmlChecked = '';
                    if (tieneCheckFilaListaConcepto(item.Concepcodi, ORIGEN_TIPO_GRUPO)) htmlChecked = ' checked ';

                    var fila = "<tr>";
                    fila += `<td style='text-align: center'><input type="checkbox" id="chkItemConcepto" class="checkboxes-concept" ${htmlChecked}></td>`;
                    fila += "<td style='text-align: center; padding-top: 7px; padding-bottom: 7px;'>" + item.Concepcodi + "</td>";
                    fila += "<td style='text-align: center'>" + item.Catenomb + "</td>";
                    fila += "<td style='text-align: left'>" + item.Concepdesc + "</td>";
                    fila += "<td style='text-align: center'>" + item.Concepnombficha + "</td>";
                    fila += "<td style='text-align: left'>" + item.Concepabrev + "</td>";
                    fila += "<td style='text-align: center'>" + item.Concepunid + "</td>";
                    fila += "<td style='text-align: center'>" + item.Conceptipo + "</td>";
                    fila + "</tr>";
                    tbody += fila;
                }

                htmlTable += tbody + `</tbody></table>`;
                $('#div_tabla_pr_concepto').html(htmlTable);
                $('#div_tabla_eq_propiedad').html("");

                $("#div_tabla_pr_concepto").show();

                setTimeout(function () {
                    $('#tabla_pr_concepto').dataTable({
                        "scrollY": 480,
                        "scrollX": true,
                        "sDom": 'ft',
                        "ordering": true,
                        "iDisplayLength": -1
                    });
                }, 250);

                asignarEventosCheckBox();
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
                    listaOrigen: entity.ListaConceporigen,
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
    var listaConceporigen = [];

    if (LISTA_AGRUPACION_CONCEPTO != null) {
        for (var i = 0; i < LISTA_AGRUPACION_CONCEPTO.length; i++) {
            if (LISTA_AGRUPACION_CONCEPTO[i].Conceporigen == 2)
                listaConcepcodi.push(LISTA_AGRUPACION_CONCEPTO[i].Propcodi);
            else
                listaConcepcodi.push(LISTA_AGRUPACION_CONCEPTO[i].Concepcodi);
            listaConceporigen.push(LISTA_AGRUPACION_CONCEPTO[i].Conceporigen);
        }
    }

    obj.ListaConcepcodi = listaConcepcodi;
    obj.ListaConceporigen = listaConceporigen;

    return obj;
}

function validarAgrupacion(obj) {
    var msj = "";

    if (obj.Nombre == null || obj.Nombre.trim() == "") {
        msj += "Debe ingresar nombre." + "\n";
    }

    if (obj.ListaConcepcodi == null || obj.ListaConcepcodi.length == 0) {
        //msj += "Debe seleccionar al menos un Concepto de Grupo." + "\n";
        msj += "Debe seleccionar al menos un Concepto / Propiedad." + "\n";
    }

    return msj;
}

function cerrarPopupAgrupacion() {
    $('#popupAgrupacion').bPopup().close();
}

function agregarConceptoToParametro() {

    $('#tabla_pr_concepto').find('input.checkboxes-concept[type="checkbox"]:checked').each(function () {

        var fila = $(this).parent().parent();

        var objConcepto = {};
        objConcepto.Propcodi = null;
        objConcepto.Concepcodi = fila.find(':nth-child(2)').text();
        objConcepto.Catenomb = fila.find(':nth-child(3)').text();
        objConcepto.Concepdesc = fila.find(':nth-child(4)').text();
        objConcepto.Concepnombficha = fila.find(':nth-child(5)').text();
        objConcepto.Concepabrev = fila.find(':nth-child(6)').text();
        objConcepto.Concepunid = fila.find(':nth-child(7)').text();
        objConcepto.Conceptipo = fila.find(':nth-child(8)').text();
        objConcepto.Conceporigen = $("input[name='origen']:checked").val();

        if (LISTA_AGRUPACION_CONCEPTO == null) {
            LISTA_AGRUPACION_CONCEPTO = [];
        }

        LISTA_AGRUPACION_CONCEPTO.push(objConcepto);
    });

    $('#tabla_eq_propiedad').find('input.checkboxes-equip[type="checkbox"]:checked').each(function () {

        var fila = $(this).parent().parent();

        var objConcepto = {};
        objConcepto.Propcodi = fila.find(':nth-child(2)').text();
        objConcepto.Concepcodi = null;
        objConcepto.Catenomb = fila.find(':nth-child(3)').text();
        objConcepto.Concepdesc = fila.find(':nth-child(4)').text();
        objConcepto.Concepnombficha = fila.find(':nth-child(5)').text();
        objConcepto.Concepabrev = fila.find(':nth-child(6)').text();
        objConcepto.Concepunid = fila.find(':nth-child(7)').text();
        objConcepto.Conceptipo = fila.find(':nth-child(8)').text();
        objConcepto.Conceporigen = $("input[name='origen']:checked").val();

        if (LISTA_AGRUPACION_CONCEPTO == null) {
            LISTA_AGRUPACION_CONCEPTO = [];
        }

        LISTA_AGRUPACION_CONCEPTO.push(objConcepto);
    });

    generarTablaAgrupacionConcepto();
    cerrarpopupConcepto();
}

function buscarConcepcodi(codigo, origen) {
    if (LISTA_AGRUPACION_CONCEPTO != null) {
        for (var i = 0; i < LISTA_AGRUPACION_CONCEPTO.length; i++) {
            if (ORIGEN_TIPO_EQUIPO != origen && LISTA_AGRUPACION_CONCEPTO[i].Concepcodi == codigo) {
                return LISTA_AGRUPACION_CONCEPTO[i];
            }
            if (ORIGEN_TIPO_EQUIPO == origen && LISTA_AGRUPACION_CONCEPTO[i].Propcodi == codigo) {
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
    strHtml += '<th style="width: 70px" class="tbform-control">Código</th>';
    strHtml += '<th style="width: 150px" class="tbform-control">Nombre</th>';
    strHtml += '<th style="width: 150px" class="tbform-control">Nombre Ficha Técnica</th>';
    strHtml += '<th style="width: 70px" class="tbform-control">Abreviatura</th>';
    strHtml += '<th style="width: 50px" class="tbform-control">Unidad</th>';
    strHtml += '<th style="width: 50px" class="tbform-control">Tipo</th>';

    if (opcion == OPCION_VER) { }
    else {
        strHtml += '<th style="width: 50px"  class="tbform-control"></th>';
    }
    strHtml += '</tr>';
    strHtml += '</thead>';

    strHtml += '<tbody>';
    for (var i = 0; i < LISTA_AGRUPACION_CONCEPTO.length; i++) {
        strHtml += '<tr>';

        if (LISTA_AGRUPACION_CONCEPTO[i].Concepcodi == null) {
            strHtml += '<td class="tbform-control">' + LISTA_AGRUPACION_CONCEPTO[i].Propcodi + '</td>';
        }
        else {
            strHtml += '<td class="tbform-control">' + LISTA_AGRUPACION_CONCEPTO[i].Concepcodi + '</td>';
        }

        strHtml += '<td class="tbform-control">' + LISTA_AGRUPACION_CONCEPTO[i].Concepdesc + '</td>';
        strHtml += '<td class="tbform-control">' + LISTA_AGRUPACION_CONCEPTO[i].Concepnombficha+ '</td>';
        strHtml += '<td class="tbform-control">' + LISTA_AGRUPACION_CONCEPTO[i].Concepabrev + '</td>';
        strHtml += '<td class="tbform-control">' + LISTA_AGRUPACION_CONCEPTO[i].Concepunid + '</td>';
        strHtml += '<td class="tbform-control">' + LISTA_AGRUPACION_CONCEPTO[i].Conceptipo + '</td>';
        if (opcion == OPCION_VER) {
        } else {
            strHtml += '<td class="tbform-control">';
            strHtml += '<input type="hidden" id="fila_concepto' + LISTA_AGRUPACION_CONCEPTO[i].Concepcodi + '" value="' + LISTA_AGRUPACION_CONCEPTO[i].Concepcodi + '" />';
            strHtml += `<input type="button" value="Quitar" onclick="quitarConcepto(${LISTA_AGRUPACION_CONCEPTO[i].Concepcodi},${LISTA_AGRUPACION_CONCEPTO[i].Propcodi})" >`;
            strHtml += '</td>';
        }
        strHtml += '</tr>';
    }
    strHtml += '</tbody>';

    $("#tabla_concepcodi").html(strHtml);

}

function quitarConcepto(concepcodi, propcodi) {
    //generar nueva lista sin el elemento
    var listaNueva = [];
    var objElimi = {};
    for (var i = 0; i < LISTA_AGRUPACION_CONCEPTO.length; i++) {
        if ((concepcodi > 0 && LISTA_AGRUPACION_CONCEPTO[i].Concepcodi == concepcodi)
            || (propcodi > 0 && LISTA_AGRUPACION_CONCEPTO[i].Propcodi == propcodi)) {
            objElimi = LISTA_AGRUPACION_CONCEPTO[i];
        } else {
            listaNueva.push(LISTA_AGRUPACION_CONCEPTO[i]);
        }
    }

    LISTA_AGRUPACION_CONCEPTO = listaNueva;

    generarTablaAgrupacionConcepto();
}

function tieneCheckFilaListaConcepto(codigo, origen) {
    var objFila = buscarConcepcodi(codigo, origen);
    return objFila != null;
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