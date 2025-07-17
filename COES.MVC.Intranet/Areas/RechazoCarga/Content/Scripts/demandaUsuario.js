var controlador = siteRoot + 'rechazoCarga/demandaUsuario/';
var uploader;
var totNoNumero = 0;
var totLimSup = 0;
var totLimInf = 0;
var listErrores = [];
var listDescripErrores = ["BLANCO", "NÚMERO", "No es número", "Valor negativo", "Supera el valor máximo"];
var listValInf = [];
var listValSup = [];
var validaInicial = true;
var hot;
var hotOptions;
var evtHot;

$(function () {

    $('#btnConsultar').click(function () {
        $('#hfTipoBusqueda').val("0");
        pintarPaginado();
        pintarBusqueda(1);
    });

    $("#cbZona").change(function () {
        changeZona();       
        //changeZonaPrueba();
    });      

    $("#cbPeriodoIni").change(function () {
        changePeriodo();
    });

    $('#cbSuministrador').multipleSelect({
        width: '500px',
        filter: false,
        onClick: function (view) {
        },
        onClose: function (view) {
        }
    });

    $('#btnValidar').click(function () {
        $('#hfTipoBusqueda').val("1");
        pintarPaginado();
        pintarBusqueda(1);
    });


    $('#btnExportar').click(function () {
        exportarExcel();
    });

    //changeZona();
    //crearPupload();
    $('#cbSubestaciones').multipleSelect({
        width: '250px',
        filter: false,
        onClick: function (view) {
        },
        onClose: function (view) {
        }
    });
    $('#cbSubestaciones').multipleSelect('checkAll');
    $('#cbSuministrador').multipleSelect('checkAll');
});

pintarPaginado = function () {

    var inp1 = document.getElementById('cbPeriodoIni').value;
    var suministrador = $('#cbSuministrador').multipleSelect('getSelects');
    if (suministrador == "[object Object]") suministrador = "";
    $('#hfSuministrador').val(suministrador);

    var subestaciones = $('#cbSubestaciones').multipleSelect('getSelects');
    if (subestaciones == "[object Object]") subestaciones = "";
    $('#hfSubestaciones').val(subestaciones);

    var registros = $('#cbRegistros').val();

    if (registros == undefined || registros == null) {
        registros = 50;
    }

    if ($('#cbPeriodoIni').val() != "") {

        $.ajax({
            type: 'POST',
            url: controlador + "paginado",
            data: {
                periodo: $('#cbPeriodoIni').val(),
                codigoZona: $('#cbZona').val(),
                codigoPuntoMedicion: $('#hfSubestaciones').val(),
                empresa: $('#txtRazonSocial').val(),
                suministrador: $('#hfSuministrador').val(),
                tipoBusqueda: $('#hfTipoBusqueda').val(),
                nroRegistrosPag: registros
            },
            success: function (evt) {
                $('#paginado').html(evt);
                mostrarPaginado();
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });

    }
    else {
        mostrarAlerta("Seleccione periodo.");
    }
}

function changeZona() {
    var x = document.getElementById("cbZona").value;
    if (x != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'ListarSubestaciones',

            data: {
                codigoZona: x
            },
            success: function (aData) {
                $('#cbSubestaciones').empty().multipleSelect('refresh');
                var cbPrueba = document.getElementById("cbSubestaciones");
                for (i = 0; i < aData.Subestaciones.length; i += 1) {

                    var fila = aData.Subestaciones[i];
                    var option = document.createElement('option');
                    option.value = fila.AREACODI;
                    option.text = fila.AREANOMB;
                    cbPrueba.add(option);
                };
                $('#cbSubestaciones').multipleSelect('refresh');
                $('#cbSubestaciones').multipleSelect('checkAll');
                //$('#cbPrueba').multipleSelect('refresh');
            },
            error: function (ex) {
                mostrarError('Consultar demanda usuario: Ha ocurrido un error');
            }
        });
    }
    else {
        $('#cbPrucbSubestacioneseba').empty().multipleSelect('refresh');
    }


}

function mostrarExito(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-exito');
}

function mostrarError(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-error');
}

function mostrarAlerta(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-alert');
}

function mostrarMensaje(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-message');
}

function limpiarMensaje() {
    $('#mensaje').removeClass();
    $('#mensaje').html('');    
}

function mostrarPaginado() {
    $.ajax({
        type: 'POST',
        url: controlador + 'paginado',
        success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado();
        },
        error: function () {
            alert("Error al cargar el paginado");
        }
    });
}

function pintarBusqueda(id) {
    var inp1 = document.getElementById('cbPeriodoIni').value;
     
    var suministrador = $('#cbSuministrador').multipleSelect('getSelects');
    if (suministrador == "[object Object]") suministrador = "";
    $('#hfSuministrador').val(suministrador);

    var subestaciones = $('#cbSubestaciones').multipleSelect('getSelects');
    if (subestaciones == "[object Object]") subestaciones = "";
    $('#hfSubestaciones').val(subestaciones);

    var zona = $('#cbZona').val();
    var tipoBusqueda = $('#hfTipoBusqueda').val();

    if (tipoBusqueda == 0) {
        if (zona > 0 && subestaciones == "") {

            mostrarAlerta(" Debe de selecceionar al menos una subestación.");
            return;
        }
    }

    var registros = $('#cbRegistros').val();
    //alert(registros);
    if (registros == undefined || registros == null) {
        registros = 50;
    }

    limpiarMensaje();

    if ($('#cbPeriodoIni').val() != "") {

        $.ajax({
            type: 'POST',
            url: controlador + 'ListarReporteInformacion',
            data: {
                periodo: $('#cbPeriodoIni').val(),
                codigoZona: zona,
                codigoPuntoMedicion: $('#hfSubestaciones').val(),
                empresa: $('#txtRazonSocial').val(),
                suministrador: $('#hfSuministrador').val(),
                nroPagina: id,
                tipoBusqueda: tipoBusqueda,
                nroRegistros: registros
            },
            success: function (evt) {
                $('#listado').css("width", $('#mainLayout').width() + "px");
                $('#listado').html(evt);
                $('#tabla').dataTable({
                    "scrollY": 480,
                    "scrollX": true,
                    "sDom": 't',
                    "ordering": false,
                    "bDestroy": true,
                    "bPaginate": false,
                    "iDisplayLength": 50
                });

                if ($("#cbRegistros").length) {
                    $('#cbRegistros').val(registros);
                }

                if (tipoBusqueda == 1) {
                    if ($("#hfCantidadErroresValidacion").val() == 0) {
                        mostrarMensaje("La información es correcta para el periodo seleccionado.");
                    } else {
                        mostrarAlerta("Existen registros con error para el periodo seleccionado. Revisar lista.");
                    }
                } else {
                    mostrarMensaje("Consulta generada.");
                }                
            },
            error: function () {
                mostrarAlerta("Error al obtener la consulta");
            }
        });
    }
    else {
        mostrarAlerta("Seleccione periodo.");
    }
}

function crearPupload() {
    uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnImportar',
        url: siteRoot + 'RechazoCarga/DemandaUsuario/Subir',
        flash_swf_url: 'Scripts/Moxie.swf',
        silverlight_xap_url: 'Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: '5mb',
            mime_types: [
                { title: "Archivos Excel .xlsx", extensions: "xlsx,xls" }
            ]
        },
        init: {
            FilesAdded: function (up, files) {
                if (uploader.files.length == 2) {
                    uploader.removeFile(uploader.files[0]);
                }
                uploader.start();
                up.refresh();
            },
            UploadProgress: function (up, file) {
                mostrarAlerta("Por favor espere ...(<strong>" + file.percent + "%</strong>)");
            },
            UploadComplete: function (up, file) {
                mostrarAlerta("Subida completada <strong>Por favor espere</strong>");
                leerExcelSubido();
                //limpiarError();
            },
            Error: function (up, err) {
                mostrarError(err.code + "-" + err.message);
            }
        }
    });
    uploader.init();
}

function leerExcelSubido() {
    var periodo = document.getElementById('cbPeriodoIni').value;
    
    var puedeImportar = true;
    var cantidadRegistros = document.getElementById('hfCantidadRegistros').value;
    
    if (cantidadRegistros > 0) {
        puedeImportar = confirm("Existen registros en el periodo seleccionado. ¿Desea continuar?");
    }
    if (puedeImportar) {
        $.ajax({
            type: 'POST',
            url: controlador + 'LeerExcelSubido',
            dataType: 'json',
            async: false,
            data: {
                periodo: periodo
            },
            success: function (respuesta) {
                if (respuesta.Exito == false) {
                    mostrarError(respuesta.Mensaje);
                } else {
                    pintarPaginado();
                    pintarBusqueda(1);
                    mostrarExito("Archivo importado");
                }
            },
            error: function () {
                mostrarError("Ha ocurrido un error");
            }
        });
    } else {
        mostrarAlerta("Operación cancelada");
    }
    
}

function changePeriodo() {
    var periodoSeleccionado = document.getElementById('cbPeriodoIni').value;
    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: controlador + "ObtenerRegistrosPeriodo",
        data: {
            periodo: periodoSeleccionado
        },
        success: function (result) {

            if (result != "-1") {                
                document.getElementById('hfCantidadRegistros').value = result;
            }
            else {
                mostrarError("Ha ocurrido un error al consultar.");
            }

        },
        error: function () {
            mostrarError("Ha ocurrido un error al consultar.");
        }
    });
}

function exportarExcel() {
   
    var suministrador = $('#cbSuministrador').multipleSelect('getSelects');
    if (suministrador == "[object Object]") suministrador = "";
    $('#hfSuministrador').val(suministrador);

    var subestaciones = $('#cbSubestaciones').multipleSelect('getSelects');
    if (subestaciones == "[object Object]") subestaciones = "";
    $('#hfSubestaciones').val(subestaciones);

    var tipoBusqueda = $('#hfTipoBusqueda').val();

    if ($('#cbPeriodoIni').val() != "") {

        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: controlador + 'GenerarReporte',
            data: {
                periodo: $('#cbPeriodoIni').val(),
                codigoZona: $('#cbZona').val(),
                codigoPuntoMedicion: $('#hfSubestaciones').val(),
                empresa: $('#txtRazonSocial').val(),
                suministrador: $('#hfSuministrador').val(),                
                tipoBusqueda: tipoBusqueda
            },
            success: function (result) {
                if (result != "-1") {
                    window.location.href = controlador + 'DescargarFormato?file=' + result + '&tipoBusqueda=' + tipoBusqueda;                   
                }
                else {
                    alert("Error al generar el archivo.");
                }
            },
            error: function () {
                mostrarAlerta("Error al generar el archivo");
            }
        });
    }
    else {
        mostrarAlerta("Seleccione periodo.");
    }
}
