var controlador = siteRoot + 'combustibles/envio/';
const ESTADO_CANCELADO = 8;

$(function () {
    $('#FechaDesde').Zebra_DatePicker({
        pair: $('#FechaHasta'),
    });
    $('#FechaHasta').Zebra_DatePicker({
    });

    $('#cbEmpresa').multipleSelect({
        width: '200px',
        filter: true,
        onClose: function (view) {
            cargarCentrales();
        }
    });
    $('#cbEmpresa').multipleSelect('checkAll');

    $('#cbCentral').multipleSelect({
        width: '200px',
        filter: true,
        onClose: function (view) {
            buscarEnvio(1);
        }
    });
    $('#cbCentral').multipleSelect('checkAll');

    //
    $('#btnNuevoIngreso').click(function () {
        $('#popupGen').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown'
        });
    });
    $('#btnExpotar').click(function () {
        exportar();
    });

    $('#btnBuscar').click(function () {
        buscarEnvio(1);
    });

    $(window).resize(function () {
        $('#listado').css("width", $('#mainLayout').width() + "px");
    });

    buscarEnvio(); // muestra el listado de todos los envios de todas las empresas del tipo de combustible

    //popup
    $('#cbEmpresaGen').change(function () {
        cargarCentralesFiltro($('#cbEmpresaGen').val());
    });

    $('#cbCentralGen').change(function () {
        cargarCombustible($('#cbCentralGen').val());
    });

    $('#btnElegir').click(function () {
        var empresa = $('#cbEmpresaGen').val();
        var central = $('#cbCentralGen').val();
        var combustible = parseInt($('#cbCombustible').val()) || 0;

        if (combustible > 0) {
            abrirFormulario(empresa, central, combustible);
        } else {
            alert("Debe seleccionar un tipo de combustible...");
        }
    });

    //cancelacion
    $('#btnAceptarCancelar').click(function () {
        realizarCancelarEnvio();
    });
    $('#btnCerrarCancelar').click(function () {
        $('#popupCancelar').bPopup().close();
    });
});

function cargarCentrales() {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "-1";
    $('#hfEmpresa').val(empresa);


    $.ajax({
        type: 'POST',
        url: controlador + 'CargarFiltroCentralXEmpresa',
        dataType: 'json',
        data: { idEmpresa: $('#hfEmpresa').val() },
        success: function (evt) {
            $("#div_central_filtro").html('');
            $("#div_central_filtro").html('<select id="cbCentral" name="cbCentral"></select>');

            if (evt.Resultado != "-1") {
                if (evt.ListaCentral.length > 0) {
                    $.each(evt.ListaCentral, function (i, item) {
                        $('#cbCentral').get(0).options[$('#cbCentral').get(0).options.length] = new Option(item.Equinomb, item.Equicodi);
                    });
                }
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }

            $('#cbCentral').multipleSelect({
                width: '200px',
                filter: true,
                onClose: function (view) {
                    buscarEnvio();
                }
            });
            $('#cbCentral').multipleSelect('checkAll');
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarCentralesFiltro(idEmpresa) {
    $('#cbCentralGen').empty();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarFiltroCentralXEmpresa',
        dataType: 'json',
        data: { idEmpresa: parseInt(idEmpresa) || 0 },
        cache: false,
        success: function (evt) {
            if (evt.Resultado != "-1") {
                if (evt.ListaCentral.length > 0) {
                    $.each(evt.ListaCentral, function (i, item) {
                        $('#cbCentralGen').get(0).options[$('#cbCentralGen').get(0).options.length] = new Option(item.Equinomb, item.Codigogrupo);
                    });

                    cargarCombustible($('#cbCentralGen').val());
                }
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error...");
        }
    });
}

function cargarCombustible(codigogrupo) {
    $('#cbCombustible').empty();
    var arrEq = codigogrupo.split("|");
    var idCentral = arrEq[0];

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarFiltroCombustibleXCentral',
        dataType: 'json',
        data: { idCentral: parseInt(idCentral) || 0 },
        cache: false,
        success: function (evt) {
            if (evt.Resultado != "-1") {
                if (evt.ListaCombustible.length > 0) {
                    $.each(evt.ListaCombustible, function (i, item) {
                        $('#cbCombustible').get(0).options[$('#cbCombustible').get(0).options.length] = new Option(item.Fenergnomb, item.Fenergcodi);
                    });
                }
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error...");
        }
    });
}

//////////////////////////////////////////////////////////////////////////////

function exportar() {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    var central = $('#cbCentral').multipleSelect('getSelects');
    var finicio = $('#FechaDesde').val();
    var ffin = $('#FechaHasta').val();
    if (empresa == "[object Object]") empresa = "-1";
    $('#hfEmpresa').val(empresa);
    $('#hfCentral').val(central);

    $.ajax({
        type: 'POST',
        url: controlador + 'GenerarArchivoReporte',
        data: {
            empresas: $('#hfEmpresa').val(), idEstado: $('#hfEstado').val(),
            centrales: $('#hfCentral').val(),
            finicios: finicio,
            ffins: ffin,
        },
        dataType: 'json',
        success: function (evt) {
            if (evt.Resultado != "-1") {
                window.location = controlador + "ExportarReporte";
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error ");
        }
    });
}

function buscarEnvio(idEstado) {
    idEstado = parseInt(idEstado) || 0;
    if (idEstado > 0) {
        $("#hdIdEstado").val(idEstado);
    }
    idEstado = $("#hdIdEstado").val();

    pintarPaginado(idEstado)
    mostrarListado(1, idEstado);
}

function pintarPaginado(idEstado) {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    var estado = idEstado;
    var central = $('#cbCentral').multipleSelect('getSelects');
    var finicio = $('#FechaDesde').val();
    var ffin = $('#FechaHasta').val();
    if (empresa == "[object Object]") empresa = "-1";
    if (empresa == "") empresa = "-1";
    if (central == "[object Object]") central = "-1";
    $('#hfEmpresa').val(empresa);
    $('#hfEstado').val(estado);
    $('#hfCentral').val(central);

    $.ajax({
        type: 'POST',
        url: controlador + "paginado",
        data: {
            empresas: $('#hfEmpresa').val(), estado: $('#hfEstado').val(),
            centrales: $('#hfCentral').val(), finicios: finicio,
            ffins: ffin
        },
        success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado();
        },
        error: function (err) {
        }
    });
}

function pintarBusqueda(nroPagina) {
    var estado = $('#hfEstado').val();
    mostrarListado(nroPagina, estado);
}

function mostrarListado(nroPagina, idEstado) {
    $('#hfNroPagina').val(nroPagina);
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    var estado = idEstado;//$('#cbEstado').multipleSelect('getSelects');
    var central = $('#cbCentral').multipleSelect('getSelects');
    var finicio = $('#FechaDesde').val();
    var ffin = $('#FechaHasta').val();
    if (empresa == "[object Object]") empresa = "-1";
    if (empresa == "") empresa = "-1";
    if (central == "[object Object]") central = "-1";
    if (central == "") central = "-1";
    $('#hfEmpresa').val(empresa);
    $('#hfEstado').val(estado);
    $('#hfCentral').val(central);


    $.ajax({
        type: 'POST',
        url: controlador + "lista",
        dataType: 'json',
        data: {
            empresas: $('#hfEmpresa').val(), idEstado: idEstado,
            centrales: $('#hfCentral').val(), finicios: finicio,
            ffins: ffin, nroPaginas: nroPagina,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                var html = "<h3>Carpetas</h3>";
                html += evt.HtmlCarpeta;
                $("#div_carpetas").html(html);

                $("#reporte").html(evt.HtmlListado);

                $('#listado').css("width", $('#mainLayout').width() + "px");
                $('#tabla').dataTable({
                    "scrollY": 430,
                    "scrollX": true,
                    "sDom": 't',
                    "ordering": false,
                    "iDisplayLength": 50
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

function mostrarDetalle(id) {
    var form_url = controlador + "EnvioCombustible";

    $("#frmEnvio").attr("action", form_url);
    $("#hdIdEnvio").val(id);
    $("#hdIdEmpresa").val(0);
    $("#hdIdEquipo").val(0);
    $("#hdIdGrupo").val(0);
    $("#hdIdFenerg").val(0);

    $("#frmEnvio").submit();
}

function abrirFormulario(empresa, codigogrupo, fenerg) {
    var form_url = controlador + "EnvioCombustible";

    var arrEq = codigogrupo.split("|");
    var idCentral = arrEq[0];
    var idGrupo = arrEq[1];

    $("#frmEnvio").attr("action", form_url);
    $("#hdIdEnvio").val(0);
    $("#hdIdEmpresa").val(empresa);
    $("#hdIdEquipo").val(idCentral);
    $("#hdIdGrupo").val(idGrupo);
    $("#hdIdFenerg").val(fenerg);

    $("#frmEnvio").submit();
}

////////////////////////////////////////////////////////////////
//Cancelación de envio
function cancelarEnvio(idEnvio) {
    $('#txtMotivo').val('');
    $("#hfIdCancelarEnvio").val(idEnvio);

    setTimeout(function () {
        $('#popupCancelar').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown'
        });
    }, 50);
}

function realizarCancelarEnvio() {
    var idEnvio = parseInt($("#hfIdCancelarEnvio").val()) || 0;
    var motivo = ($('#txtMotivo').val()).trim();
    if (motivo == '')
        alert("Debe ingresar motivo");
    else {
        if (confirm('¿Desea cancelar el envío de costo de combustible?')) {
            $.ajax({
                type: 'POST',
                url: controlador + 'CancelarEnvio',
                dataType: 'json',
                data: {
                    idEnvio: idEnvio,
                    motivo: motivo
                },
                cache: false,
                success: function (evt) {
                    if (evt.Resultado != "-1") {
                        alert("Se efectuó la cancelación del envío.");
                        $('#popupCancelar').bPopup().close();

                        buscarEnvio(ESTADO_CANCELADO);
                    } else {
                        alert('Error al cancelar el envío: ' + evt.Mensaje);
                    }

                },
                error: function (err) {
                    alert('Error al cancelar el envío');
                }
            });
        }
    }
}