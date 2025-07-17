var controlador = siteRoot + "PotenciaFirmeRemunerable/Configuracion/";
var NUEVO = 1;
var EDITAR = 2;

$(document).ready(function () {


    $('#cbAnio').change(function () {
        listadoPeriodo();
        cargarListaGamsEquipos();
    });

    $('#cbPeriodo,#chbxMostarTodos').change(function () {
        cargarListaGamsEquipos();
    });


    $("#btnNuevo").click(function () {
        $("#frmNuevo").trigger("reset");
        cargarBarrasGams();
        cargarUnidades();

        abrirPopupNuevoEquipo();

        $('#popupNuevo').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            follow: [false, false], //x, y
            position: ["38%", "15%"] //x, y
        });
    });

    
    $("#new-equipo-periodo").Zebra_DatePicker({
        format: 'd-m-Y',
    });

    $("#frmNuevo").submit(function (event) {
        event.preventDefault();
        var data = getFormData($(this));
        //data.Idunidad = data.selectItemIdunidad;

        guardarRelacionGamsEquipos(data, NUEVO);
    });

    /** EDITAR GAMS-EQUIPO */
    $("#frmEditarEquipo").submit(function (event) {
        event.preventDefault();
        var data2 = getFormData($(this));

        guardarRelacionGamsEquipos(data2, EDITAR);
    });
    $('#cbEstado').change(function () {
        cargarListaGamsEquipos();
    });

    cargarListaGamsEquipos();
});


/** Carga lista de Relacion Gams y Central */
function cargarListaGamsEquipos() {
    var pericodi = parseInt($("#cbPeriodo").val()) || 0;
    var estado = $("#cbEstado").val();

    $("#listadoGamsEquipo").html('');

    var ANCHO_TABLA_EQ = ($('#mainLayout').width() - 30) + "px";

    $.ajax({
        type: 'POST',
        url: controlador + "ListarRelacionGamsEquipos",
        data: {
            pfrpercodi: pericodi,
            estado: estado
        },
        success: function (evt) {

            if (evt.Resultado != "-1") {
                $('#listadoGamsEquipo').html(evt.Resultado);
                $('#listadoGamsEquipo').css("width", ANCHO_TABLA_EQ);

                var altotabla = parseInt($('#listadoGamsEquipo').height()) || 0;
                $('#lstGamsEquipo').dataTable({
                    "ordering": false,
                    "searching": true,
                    "iDisplayLength": 15,
                    "info": false,
                    "paging": false,
                    scrollCollapse: true,
                    "scrollX": true,
                    "scrollY": altotabla > 355 || altotabla == 0 ? 355 + "px" : "100%"
                });

            } else {
                alert('Ha ocurrido un error: ' + evt.Mensaje);
            }

        },
        error: function (err) {
            alert('Ha ocurrido un error: ' + err);
        }
    });
};

/** Carga lista de periodos por año */
function listadoPeriodo() {

    var anio = parseInt($("#cbAnio").val()) || 0;

    $("#cbPeriodo").empty();

    $.ajax({
        type: 'POST',
        url: controlador + "PeriodoListado",
        data: {
            anio: anio,
        },
        async: false,
        success: function (evt) {
            if (evt.Resultado != "-1") {
                if (evt.ListaPeriodo.length > 0) {
                    $.each(evt.ListaPeriodo, function (i, item) {
                        $('#cbPeriodo').get(0).options[$('#cbPeriodo').get(0).options.length] = new Option(item.Pfrpernombre, item.Pfrpercodi);
                    });
                } else {
                    $('#cbPeriodo').get(0).options[0] = new Option("--", "0");
                }

            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function getFormData($form) {

    var disabled = $form.find(':input:disabled').removeAttr('disabled');
    var unindexed_array = $form.serializeArray();
    disabled.attr('disabled', 'disabled');

    var indexed_array = {};

    $.map(unindexed_array, function (n, i) {
        indexed_array[n['name']] = n['value'];
    });

    return indexed_array;
}

function guardarRelacionGamsEquipos(dataForm, accion1) {

    dataForm.Pfrcatcodi = TIPO_GAMS_EQUIPOS;
    AgregarEntidadDat(dataForm);

    var idunidad = dataForm.Idunidad.split('|');
    dataForm.Equipadre = +idunidad[0];
    dataForm.Grupocodi = +idunidad[1];
    dataForm.Equicodi = +idunidad[2];
    dataForm.Pfrrgeunidadnomb = idunidad[3];

    $.ajax({
        type: 'POST',
        url: controlador + 'GuardarRelacionGamsEquipos',
        contentType: "application/json",
        dataType: 'json',
        data: JSON.stringify({ pfrRelacion: dataForm, accion: accion1 }),
        cache: false,
        success: function (data) {
            if (data.Resultado == "1") {
                if (accion1 == NUEVO) {
                    alert("Se registro correctamente");
                    $(`#popupNuevo`).bPopup().close()
                    $("#frmNuevo").trigger("reset");
                    $('#cboUnidad').multipleSelect('uncheckAll');

                    //Actualizar proximo codigo dispoible
                    $("#hfCodDisponibleGamsequipo").val(data.CodigoDisponibleGamsequipo);

                    cargarListaGamsEquipos();
                }
                else {
                    if (accion1 == EDITAR) {

                        $("#popupEditarGamsEquipo").bPopup().close()
                        $("#frmEditarEquipo").trigger("reset");

                        alert("Se editó correctamente");

                        cargarListaGamsEquipos();
                    }
                }
            } else {
                alert('Ha ocurrido un error : ' + data.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function popupClose(id) {
    $(`#${id}`).bPopup().close();
}

function abrirPopupNuevoEquipo() {

    var codigoDisponibleGamsequi = $("#hfCodDisponibleGamsequipo").val();

    $("#new-gamsequipo-id").val(codigoDisponibleGamsequi);
}

function cargarBarrasGams() {
    var pericodi = parseInt($("#cbPeriodo").val()) || 0;

    $.ajax({
        type: 'POST',
        url: controlador + "ListarBarrasGams",
        async: false,
        data: {
            pericodi: pericodi
        },
        success: function (evt) {

            if (evt.Resultado == "1") {
                $("#cboBarraGams option").remove();
                $("#cboBarraGams").append(new Option("-- SELECCIONAR --", ""));
                $("#cboBarraGams option:first").prop('disabled', true);
                $.each(evt.ListaBarra, function (i, val) {
                    $("#cboBarraGams").append(new Option(`${val.Pfrentid} | ${val.Pfrentnomb}`, val.Pfrentcodi));
                });

            } else {
                alert('Ha ocurrido un error: ' + evt.Mensaje);
            }

        },
        error: function (err) {
            alert('Ha ocurrido un error: ' + err);
        }
    });
}

function cargarUnidades() {
    var pericodi = parseInt($("#cbPeriodo").val()) || 0;

    $.ajax({
        type: 'POST',
        url: controlador + "ListarUnidades",
        async: false,
        data: {
            pericodi: pericodi
        },
        success: function (evt) {

            if (evt.Resultado == "1") {
                $("#cboUnidad option").remove();
                $.each(evt.ListaUnidad, function (i, val) {

                    var equipo = "";
                    if (val.Equinomb) {
                        equipo = `| ${val.Equinomb}`;
                    }

                    var sUnidad = `${val.Emprnomb} | ${val.Central} ${equipo}`;

                    $("#cboUnidad").append(new Option(`${sUnidad}`, `${val.Equipadre}|${val.Grupocodi}|${val.Equicodi}|${val.Equinomb}`));
                });

                $('#cboUnidad').multipleSelect({
                    width: '350px',
                    filter: true,
                    single: true,
                    onClose: function () {
                    },
                    placeholder: '-- SELECCIONAR --'
                });

            } else {
                alert('Ha ocurrido un error: ' + evt.Mensaje);
            }

        },
        error: function (err) {
            alert('Ha ocurrido un error: ' + err);
        }
    });
}