var controlador = siteRoot + 'combustibles/gestion/';
const ESTADO_OBSERVADO = 6;
const ESTADO_APROBADO = 3;
const ESTADO_LEVANTAMIENTO_OBS = 7;

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
            buscarEnvio();
        }
    });
    $('#cbCentral').multipleSelect('checkAll');

    //
    $('#btnExpotar').click(function () {
        exportar();
    });

    $('#btnBuscar').click(function () {
        buscarEnvio();
    });

    $('#btnParametros').click(function () {
        window.location = siteRoot + 'combustibles/Configuracion/';
    });

    //ampliación
    $('#idFechaAmp').Zebra_DatePicker({
    });

    $("#idAgregarAmpl").click(function () {
        grabarAmpliacion();
    });
    $("#idCancelarAmpl").click(function () {
        $('#popupAmpl').bPopup().close();
    });

    //
    $(window).resize(function () {
        $('#listado').css("width", $('#mainLayout').width() + "px");
    });

    buscarEnvio(); // muestra el listado de todos los envios de todas las empresas del tipo de combustible
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
            //alert("Ha ocurrido un error");
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

//////////////////////////////////////////////////////////////////////////////

function mostrarDetalle(id) {
    window.location = controlador + "EnvioCombustible?idEnvio=" + id;
}

function exportarFormularioEnvio(idEnvio) {
    $.ajax({
        type: 'POST',
        url: controlador + "ExportarFormularioEnvio",
        dataType: 'json',
        data: {
            cbenvcodi: idEnvio
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                window.location = controlador + "ExportarReporteFormulario";
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }

        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}


//////////////////////////////////////////////////////////////////////////////
function mostrarAmpliarPlazo(id) {

    $("#hfEnvio").val(0);
    $("#ampl_enviocodi").html("");
    $("#ampl_fec_sol").html("");
    $("#ampl_central").html("");
    $("#ampl_comb").html("");

    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerDatosEnvio',
        dataType: 'json',
        data: {
            idEnvio: id,
        },
        cache: false,
        success: function (evt) {
            if (evt.Resultado != "-1") {
                $("#hfEnvio").val(evt.Envio.Cbenvcodi);
                $("#ampl_enviocodi").html(evt.Envio.Cbenvcodi);
                $("#ampl_fec_sol").html(evt.Envio.CbenvfecsolicitudDesc);
                $("#ampl_central").html(evt.Envio.Equinomb);
                $("#ampl_comb").html(evt.Envio.Fenergnomb);

                $("#idFechaAmp").val(evt.FechaPlazo);
                $("#cbHora").html(generarHtmlHora(evt.HoraPlazo));

                setTimeout(function () {
                    $('#popupAmpl').bPopup({
                        easing: 'easeOutBack',
                        speed: 150,
                        transition: 'slideDown'
                    });
                }, 50);

            } else {
                alert('Ha ocurrido un error: ' + evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function generarHtmlHora(horaActual) {
    var str = ``;

    for (var i = 0; i < 48; i++) {
        var hora = "0" + Math.floor( (i + 1) / 2);
        hora = hora.substr((hora.length > 2) ? 1 : 0, 2);
        var minuto = "0" + (((i + 1) % 2) * 30);
        minuto = minuto.substr((minuto.length > 2) ? 1 : 0, 2);
        var horarmin = hora + ":" + minuto;

        var selected = i == horaActual ? "selected": "";
        str += `<option value="${(i + 1)}" ${selected}>${horarmin}</option>`;
    }

    return str;
}

function grabarAmpliacion() {

    var id = parseInt($("#hfEnvio").val()) || 0;
    var fecha = $("#idFechaAmp").val();
    var hora = parseInt($("#cbHora").val()) || 0;

    if (confirm("¿Desea habilitar el plazo?")) {
        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarAmpliacion',
            dataType: 'json',
            data: {
                idEnvio: id,
                fecha: fecha,
                hora: hora
            },
            cache: false,
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    alert("Se habilitó el plazo para levantar observaciones.");

                    $('#popupAmpl').bPopup().close();

                    buscarEnvio(ESTADO_OBSERVADO);
                } else {
                    alert('Ha ocurrido un error: ' + evt.Mensaje);
                }
            },
            error: function (err) {
                alert("Ha ocurrido un error");
            }
        });
    }
}


//////////////////////////////////////////////////////////////////////////////

function habilitarItem106(id) {

    if (confirm("¿Desea habilitar el ítem 1.06?")) {
        $.ajax({
            type: 'POST',
            url: controlador + 'HabilitarItem106',
            dataType: 'json',
            data: {
                idEnvio: id,
            },
            cache: false,
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    alert("Se habilitó el ítem 1.06.");

                    buscarEnvio(ESTADO_OBSERVADO);
                } else {
                    alert('Ha ocurrido un error: ' + evt.Mensaje);
                }
            },
            error: function (err) {
                alert("Ha ocurrido un error");
            }
        });
    }
}

