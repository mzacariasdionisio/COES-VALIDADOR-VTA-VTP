var controlador = siteRoot + 'IND/restriccionoperativa/';
var APP_MOSTRAR_MODAL_EQUIPO_YA_CARGADO = 0;

var TIPO_ACCION_NUEVO = 1;
var TIPO_ACCION_VER = 2;
var TIPO_ACCION_EDITAR = 3;

$(function () {
    $('#cntMenu').css("display", "none");

    $('#cbAnio').change(function () {
        listadoPeriodo();
    });
    $('#cbPeriodo').change(function () {
        mostrarListado();
    });

    $('#cbTipoEmpresa').multipleSelect({
        width: '100%',
        filter: true,
        onClose: function (view) {
            cargarEmpresas();
        }
    });

    $('#cbTipoEmpresa').multipleSelect('checkAll');

    $('#cbFamilia').multipleSelect({
        width: '100%',
        filter: true
    });

    $('#cbFamilia').multipleSelect('checkAll');

    $('#cbEmpresa').multipleSelect({
        width: '250px',
        filter: true
    });

    $('#cbEmpresa').multipleSelect('checkAll');

    $('#btnBuscar').click(function () {
        buscarEvento();
    });

    $('#btnGuardar').click(function () {
        guardarClasificacion();
    });

    buscarEvento();
});

function buscarEvento() {
    mostrarListado();
}

function cargarEmpresas() {
    $("#empresas").html('');

    var tipoEmpresa = $('#cbTipoEmpresa').multipleSelect('getSelects');
    $('#hfTipoEmpresa').val(tipoEmpresa);
    $.ajax({
        type: 'POST',
        url: controlador + 'EmpresaListado',

        data: { tiposEmpresa: $('#hfTipoEmpresa').val() },

        success: function (data) {
            if (data.Resultado != '-1') {
                $("#empresas").html("<select id='cbEmpresa' name='IdEmpresa' multiple='multiple'></select>");

                if (data.ListaEmpresa.length > 0) {
                    $.each(data.ListaEmpresa, function (i, item) {
                        $('#cbEmpresa').get(0).options[$('#cbEmpresa').get(0).options.length] = new Option(item.Emprnomb, item.Emprcodi);
                    });
                }

                $('#cbEmpresa').multipleSelect({
                    width: '250px',
                    filter: true
                });
                $('#cbEmpresa').multipleSelect('checkAll');
            } else {
                alert("Ha ocurrido un error: " + data.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error ");
        }
    });
}

function mostrarListado() {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    var tipoEmpresa = $('#cbTipoEmpresa').multipleSelect('getSelects');
    var tipoEquipo = $('#cbFamilia').multipleSelect('getSelects');
    var pericodi = parseInt($("#cbPeriodo").val()) || 0;

    if (empresa == "[object Object]") empresa = "-1";

    $('#hfEmpresa').val(empresa);
    $('#hfTipoEmpresa').val(tipoEmpresa);
    $('#hfTipoEquipo').val(tipoEquipo);

    $('#reporte').css("display", "block");
    $('#listado').html('');

    $.ajax({
        type: 'POST',
        url: controlador + "RestriccionListado",
        data: {
            pericodi: pericodi,
            tiposEmpresa: $('#hfTipoEmpresa').val(),
            empresas: $('#hfEmpresa').val(),
            tiposEquipo: $('#hfTipoEquipo').val(),
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                $('#listado').css("width", $('#mainLayout').width() + "px");

                $('#listado').html(evt.Resultado);

                $(document).ready(function () {
                    // Setup - add a text input to each footer cell

                    /*$('#tabla tfoot th').each(function (i) {
                        var title = $(this).text();
                        //var title = $('#example thead th').eq($(this).index()).text();
                        //$(this).html('<input type="text" placeholder="Search ' + title + '" />');
                        $(this).html('<input type="text" placeholder="Search ' + title + '" data-index="' + 4 + '" />');
                    });*/
                    $('#foot_1').html('<input type="text" placeholder="Buscar Ubicación" data-index="8" style="width: 105px;" />');
                    $('#foot_2').html('<input type="text" placeholder="Equipo" data-index="9" style="width: 50px;" />');

                    // DataTable
                    var table = $('#miTabla').DataTable({
                        scrollY: "300px",
                        scrollX: true,
                        scrollCollapse: true,
                        paging: false,
                        "sDom": 'ft',
                        ordering: false,
                        "stripeClasses": [],
                        columnDefs: [{
                            targets: 7, //empresa
                            render: $.fn.dataTable.render.ellipsis(30, true)
                        },
                        {
                            targets: 8, //ubicacion
                            render: $.fn.dataTable.render.ellipsis(20, true)
                        },
                            //{
                            //    targets: 12, //descripcion
                            //    render: $.fn.dataTable.render.ellipsis(90, true)
                            //}
                        ]
                    });

                    // Filter event handler
                    $(table.table().container()).on('keyup', 'tfoot input', function () {
                        table
                            .column($(this).data('index'))
                            .search(this.value)
                            .draw();
                    });

                    //https://stackoverflow.com/questions/56838899/datatable-first-radio-button-of-list-never-showing-checked
                    var htmlRow = $("#hdFirstRowRadio").val();
                    if (htmlRow != null) {
                        var params = htmlRow.split(',');
                        $("input[name=rbtnIndisponibilidad_" + params[0] + "]").prop('checked', false).removeAttr('checked');

                        if (params[1] != null && params[1].length > 0)
                            $("input[name=rbtnIndisponibilidad_" + params[0] + "][value=" + params[1] + "]").prop('checked', true).attr('checked', 'checked');
                        else
                            $("input[name=rbtnIndisponibilidad_" + params[0] + "][value='-1']").prop('checked', true).attr('checked', 'checked');
                    }

                    $('input[type=radio][name^="rbtnIndisponibilidad"]').unbind();
                    $('input[type=radio][name^="rbtnIndisponibilidad"]').change(function () {

                        var idFila = $(this).parent().parent().attr('id') + '';
                        var codigo = idFila.substring(3, idFila.length);

                        var tipoindisp = $('input[name="rbtnIndisponibilidad_' + codigo + '"]:checked').val();

                        if (tipoindisp == "PP" || tipoindisp == "FP") {
                            $("#txt_prmw_" + codigo).show();
                        } else {
                            $("#txt_prmw_" + codigo).val("");
                            $("#txt_prmw_" + codigo).hide();
                        }
                    });
                });
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function mostrarError(msj) {
    alert("Ha ocurrido un error");
    console.log(msj);
}

////////////////////////////////////////////////////////////////////////////////////
/// Crud de mantenimiento
////////////////////////////////////////////////////////////////////////////////////

function guardarClasificacion() {
    var listaData = listarCheckDeUsuario();

    if (confirm("¿Desea guardar los cambios efectuados?")) {
        var empresa = $('#cbEmpresa').multipleSelect('getSelects');
        var tipoEquipo = $('#cbFamilia').multipleSelect('getSelects');
        var pericodi = parseInt($("#cbPeriodo").val()) || 0;

        if (empresa == "[object Object]") empresa = "-1";

        $('#hfEmpresa').val(empresa);
        $('#hfTipoEquipo').val(tipoEquipo);

        var dataJson = {
            lstIndRestriccion: listaData,
            pericodi: pericodi,
            empresas: $('#hfEmpresa').val(),
            tiposEquipo: $('#hfTipoEquipo').val(),
        };

        $.ajax({
            type: 'POST',
            contentType: 'application/json; charset=UTF-8',
            dataType: 'json',
            url: controlador + "RestriccionGuardar",
            data: JSON.stringify(dataJson),
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    $('#mensaje').hide();
                    mostrarListado();
                } else {
                    mostrarError("Error al Grabar: " + evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarError('Ha ocurrido un error');
            }
        });
    }
}

function listarCheckDeUsuario() {
    var listaData = [];
    $("tr.fila_dato").each(function () {

        var idFila = $(this).attr('id') + '';
        var codigo = idFila.substring(3, idFila.length);

        var tipoindisp = $('input[name="rbtnIndisponibilidad_' + codigo + '"]:checked').val();
        var pr = parseFloat($('#txt_prmw_' + codigo).val()) || 0;

        if (tipoindisp != "PP" && tipoindisp != "FP")
            pr = null;

        var entity = {
            Iccodi: codigo,
            Iiccotipoindisp: tipoindisp,
            Iiccopr: pr > 0 ? pr : null,
        };

        listaData.push(entity);
    });

    return listaData;
}

function consultarRestriccion(iccodi) {
    $("#popupEvento div.popup-title span").html('Ver detalle de la restricción');
    $("#idPopupEvento").html('');

    $.ajax({
        type: 'POST',
        url: controlador + "VerDetalleRestriccion",
        data: {
            iccodi: iccodi
        },
        success: function (dataHtml) {
            $('#idPopupEvento').html(dataHtml);

            setTimeout(function () {
                $('#popupEvento').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown',
                    modalClose: true
                });
            }, 50);
        },
        error: function (err) {
            alert('Ha ocurrido un error');
        }
    });

}

function verHistorialRestriccion(iccodi) {
    $("#popupEvento div.popup-title span").html('Historial de Cambios');
    $("#idPopupEvento").html('');

    $.ajax({
        type: 'POST',
        url: controlador + "VerHistorialCambio",
        data: {
            iccodi: iccodi
        },
        success: function (dataHtml) {
            $('#idPopupEvento').html(dataHtml);

            setTimeout(function () {
                $('#popupEvento').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown',
                    modalClose: true
                });

                $('#tabla_hist').dataTable({
                    scrollX: false,
                    scrollCollapse: false,
                    paging: false,
                    "sDom": 't',
                    ordering: false,
                });

            }, 50);

        },
        error: function (err) {
            alert('Ha ocurrido un error');
        }
    });

}

function listadoPeriodo() {

    var anio = parseInt($("#cbAnio").val()) || 0;

    $("#cbPeriodo").empty();

    $.ajax({
        type: 'POST',
        url: controlador + "PeriodoListado",
        data: {
            anio: anio,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                if (evt.ListaPeriodo.length > 0) {
                    $.each(evt.ListaPeriodo, function (i, item) {
                        $('#cbPeriodo').get(0).options[$('#cbPeriodo').get(0).options.length] = new Option(item.Iperinombre, item.Ipericodi);
                    });
                } else {
                    $('#cbPeriodo').get(0).options[0] = new Option("--", "0");
                }

                mostrarListado();
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}