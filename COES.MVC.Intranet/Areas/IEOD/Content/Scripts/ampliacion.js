var controlador = siteRoot + 'IEOD/ampliacion/';

var listFormatCodi = [];
var listFormatPeriodo = [];

$(function () {
    $('#cbEmpresa').multipleSelect({
        width: '200px',
        filter: true
    });
    $('#cbFormato').change(function () {
        cargarEmpresas();
    });

    $('#idFecha').Zebra_DatePicker({
    });
    $('#btnBuscar').click(function () {
        mostrarListado();
    });
    $('#btnAmpliar').click(function () {
        agregarAmpliacion();
    });

    valoresIniciales();
    mostrarListado();
});

function valoresIniciales() {
    var strFormatCodi = $('#hfFormatCodi').val();
    var strFormatPeriodo = $('#hfFormatPeriodo').val();
    listFormatCodi = strFormatCodi.split(',');
    listFormatPeriodo = strFormatPeriodo.split(',');
    //$('#cbEmpresa').multipleSelect('checkAll');  
    //$('#cbFormato').val(57);//CONSUMO COMBUSTIBLES
    cargarEmpresas();
    
}

function mostrarListado() {

    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    var formato = $('#cbFormato').val();
    if (empresa == "[object Object]") empresa = "-1";
    if (empresa == "") empresa = "-1";   
    $('#hfEmpresa').val(empresa);
    $('#hfFormato').val(formato);
    $.ajax({
        type: 'POST',
        url: controlador + "lista",
        data: {
            sEmpresa: $('#hfEmpresa').val(), fecha: $('#idFecha').val(), sFormato: $('#hfFormato').val()
        },

        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");
            $('#listado').html(evt);
            $('#tabla').dataTable({
                "scrollY": 430,
                "scrollX": true,
                "sDom": 't',
                "ordering": true,
                "iDisplayLength": 50
            });
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function agregarAmpliacion() {
    var codigoApp = parseInt($("#hdCodigoApp").val()) || 0;

    $.ajax({
        type: 'POST',
        url: controlador + "AgregarAmpliacion",
        data: {
            app: codigoApp
        },
        success: function (evt) {

            $('#detalleAmpliacion').html(evt);

            $('#txtFecha').unbind();
            $('#txtFecha').Zebra_DatePicker({
                onSelect: function () {
                }
            });
            $('#cbAnio').unbind();
            $('#cbAnio').Zebra_DatePicker({
                format: 'Y',
                onSelect: function () {
                    cargarSemanaAnho($('#cbAnio').val());
                }
            });

            $('#cbSemana').change(function () {
                
            });

            $('#idFechaAmp').unbind();
            $('#idFechaAmp').Zebra_DatePicker({
            });

            $("#idAgregar").unbind();
            $("#idAgregar").click(function () {
                grabarAmpliacion();
            });
            $("#idCancelar").click(function () {
                $('#validaciones').bPopup().close();
            });

            $('#cbFormato2').unbind();
            $('#cbFormato2').change(function () {
                horizonte($('#cbFormato2').val());
                cargarEmpresas2();
            });

            cargarEmpresas2();

            horizonte($('#cbFormato2').val());

            setTimeout(function () {
                $('#validaciones').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown',
                    modalClose: false
                });
            }, 50);
        },
        error: function () {
            alert("Error en mostrar periodo");
        }
    });
}

function grabarAmpliacion() {
    var error = false;
    var hora = $('#cbHora').val();
    var formato = $('#cbFormato2').val();
    if (formato == 0) {
        alert("Ingresar Formato");
    }
    else {
       
        $('#validaciones').bPopup().close();

        $.ajax({
            type: 'POST',
            url: controlador + "grabarValidacion",
            dataType: 'json',
            data: {
                anio: $('#cbAnio').val(),
                fecha: $('#txtFecha').val(), nroSemana: $('#cbSemana').val(),
                hora: hora, empresa: $('#cbEmpresa2').val(),
                idFormato: formato
            },
            success: function (evt) {
                if (evt == 1) {

                    $("#cbFormato").val($('#cbFormato2').val());
                    mostrarListado();
                }
                else {
                    alert("Error en Grabar Ampliación");
                }
            },
            error: function () {
                alert("Error en Grabar Ampliación");
            }
        });
    }
}

function cargarEmpresas() {
    var formato = $('#cbFormato').val();
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarEmpresas',
        data: { idFormato: formato },

        success: function (aData) {
            $('#empresas').html(aData);
            mostrarListado();
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarEmpresas2() {
    var formato = $('#cbFormato2').val();
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarEmpresas2',
        data: { idFormato: formato },

        success: function (aData) {
            $('#empresas2').html(aData);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function listarFormato2(idLectura) {
    $.ajax({
        type: 'POST',
        url: controlador + "ListarFormatosXLectura2",
        data: {
            idLectura: idLectura, idEmpresa: $('#cbEmpresa2').val()
        },
        success: function (evt) {
            $('#listTipoInformacion2').html(evt);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });

}

function buscarFormatPeriodo(valor) {
    for (var i = 0 ; i < listFormatCodi.length; i++)
        if (listFormatCodi[i] == valor) return listFormatPeriodo[i];
}

function horizonte(valor) {
    var opcion = buscarFormatPeriodo(valor);
    
    if (opcion == 1) {
        $('.cntFecha').css("display", "table-row");
        $('.cntSemana').css("display", "none");
        $('#fechasSemana').css("display", "none");
    } else {
        $('.cntFecha').css("display", "none");
        $('.cntSemana').css("display", "table-row");
        $('#fechasSemana').css("display", "table-row");
    }
}


function cargarSemanaAnho(anho) {
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarSemanas',
        async: false,
        data: {
            idAnho: anho
        },
        success: function (aData) {
            $("#divSemana").html(aData);

            $('#cbSemana').unbind('change');
            $('#cbSemana').change(function () {
                mostrarFechas();
            });
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}