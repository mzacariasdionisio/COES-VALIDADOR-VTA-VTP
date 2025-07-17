var controlador = siteRoot + "PotenciaFirmeRemunerable/Configuracion/";

$(document).ready(function () {


    $('#cbAnio').change(function () {
        listadoPeriodo();
        cargarListaGamsSsaa();
    });

    $("#new-equipo-periodo").Zebra_DatePicker({
        format: 'd-m-Y',
    });

    $('#cbPeriodo,#chbxMostarTodos').change(function () {
        cargarListaGamsSsaa();
    });


    $("#btnNuevo").click(function () {
        $("#frmNuevo").trigger("reset");
        cargarBarrasGams();

        $('#popupNuevo').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            follow: [false, false], //x, y
            position: ["38%", "15%"] //x, y
        });
    });

    $("#frmNuevo").submit(function (event) {
        event.preventDefault();
        var data = getFormData($(this));
        //data.Idunidad = data.selectItemIdunidad;

        guardarRelacionGamsSsaa(data);
    });
    $('#cbEstado').change(function () {
        cargarListaGamsSsaa();
    });

    cargarListaGamsSsaa();
});


/** Carga lista de Relacion Gams y Ssaa */
function cargarListaGamsSsaa() {
    var pericodi = parseInt($("#cbPeriodo").val()) || 0;
    var estado = $("#cbEstado").val();

    $("#listadoGamsEquipo").html('');

    var ANCHO_TABLA_EQ = ($('#mainLayout').width() - 30) + "px";

    $.ajax({
        type: 'POST',
        url: controlador + "ListarRelacionGamsSsaa",
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
        async: false,
        data: {
            anio: anio,
        },
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

function guardarRelacionGamsSsaa(dataForm) {

    dataForm.Pfrcatcodi = TIPO_GAMS_SSAA;
    AgregarEntidadDat(dataForm);


    $.ajax({
        type: 'POST',
        url: controlador + 'GuardarRelacionGamsSsaa',
        contentType: "application/json",
        dataType: 'json',
        data: JSON.stringify({ pfrRelacion: dataForm }),
        cache: false,
        success: function (data) {
            if (data.Resultado == "1") {
                alert("Se registro correctamente");
                $(`#popupNuevo`).bPopup().close()
                $("#frmNuevo").trigger("reset");

                cargarListaGamsSsaa();
            } else {
                alert('Ha ocurrido un error : ' + data.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
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

function popupClose(id) {
    $(`#${id}`).bPopup().close();
}

function obtenerBarraGams(id) {

    $.ajax({
        type: 'POST',
        url: controlador + "EquipoObjeto",
        async: false,
        data: {
            pfreqpcodi: id,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {

                var iniVigencia = evt.PfrEquipo.PfreqpvigenciainiDesc;
                var finVigencia = evt.PfrEquipo.PfreqpvigenciafinDesc;

                $("#vigenciaDesc").text(`(*) Vigente desde el ${iniVigencia}`);
                $("#vigenciaDescFin").text(``);
                if (finVigencia) {
                    $("#vigenciaDescFin").text(`(*) La relación estará de baja desde el ${finVigencia}`);
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
