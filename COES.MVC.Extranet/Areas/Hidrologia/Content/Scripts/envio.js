var controlador = siteRoot + 'hidrologia/envio/';
var listFormatCodi = [];
var listFormatPeriodo = [];
var listFormatDescrip = [];
var uploader;
var totNoNumero = 0;
var listErrores = [];
var validaInicial = true;

$(function () {


    $('#txtFecha').Zebra_DatePicker({
        direction: 0,
        onSelect: function () {
            //validarFecha();
            //limpiarMensaje();
        }
    });

    $('#txtMes').Zebra_DatePicker({
        format: 'm Y'
    });

    $('#cbArea').change(function () {
        cargarLectura();
    });

    $('#cbLectura').change(function () {
        cargarFormatos();
        //limpiarMensaje();
    });

    $('#cbEmpresa').change(function () {
        //cargarFormatos();
        cargarLectura();
        //limpiarMensaje();
    });

    $('#cbFormato').change(function () {
        horizonte();
        //pintaDescripcion($('#cbFormato').val())
    });

    $('#formEnvio').submit(function () {
        var mensaje = validarEnvio();
        if (mensaje == "") {
            return true;
        }
        else {
            alert(mensaje);
            return false;
        }
    });
    
    //validarFecha();
    //validacion();

    CargarPrevio();

});

function inicializaVarGlobales()
{
    totNoNumero = 0;
    validaInicial = true;
    listErrores.splice(0, listErrores.length);
}

CargarPrevio = function () {
    $("#cbEmpresa").val($("#hfEmpresa").val());
    $("#cbLectura").val($("#hfLectura").val());
    $("#txtMes").val($("#hfMes").val());
    $("#cbSemana").val($("#hfSemana").val());
    $("#txtFecha").val($("#hfFecha").val());
    var strFormatCodi = $('#hfFormatCodi').val();
    var strFormatPeriodo = $('#hfFormatPeriodo').val();
    var strFormatDescrip = $('#hfFormatDescrip').val();
    listFormatCodi = strFormatCodi.split(',');
    listFormatPeriodo = strFormatPeriodo.split(',');
    listFormatDescrip = strFormatDescrip.split(',');
    //listarFormato(-1);
}

validarFecha = function () { 
    var itipoinformacion = $('#cbTipoInformacion').val();
    if (itipoinformacion == null) itipoinformacion = 0;
    $.ajax({
        type: 'POST',
        url: controlador + 'ValidarPlazo',
        dataType: 'json',
        data: {
            fecha: $('#txtFecha').val(), idFormato: itipoinformacion, idEmpresa: $('#cbEmpresa').val(),
            semana: $('#cbSemana').val(), idLectura: $('#cbLectura').val(), mes: $('#txtMes').val()
        },
        
        success: function (result) {
            if (result.EnPlazo) {
                $('#idcarga').css("display", "block");
            }
            else {
                $('#idcarga').css("display", "none");
            }

            $('#contentPlazo').text(result.ValidacionPlazo);
            $('#cntFecha').css("display", "block");

        },
        error: function () {
            mostrarError("Error en Validar Fecha");
        }
    });
}

horizonte = function () {

    var opcion = buscarPeriodo($('#cbFormato').val());    
    switch (parseInt(opcion))
    {        
        case 1: //dia
            $('#cntFecha').css("display", "block");
            $('#cntSemana').css("display", "none");
            $('#fechasSemana').css("display", "none");
            $('#cntMes').css("display", "none");
            break;
        case 2: //semanal
            $('#cntFecha').css("display", "none");
            $('#cntSemana').css("display", "block");
            $('#fechasSemana').css("display", "block");
            $('#cntMes').css("display", "none");
            break;
         //mensual

            //break;
        case 3: case 5:
            $('#cntFecha').css("display", "none");
            $('#cntSemana').css("display", "none");
            $('#fechasSemana').css("display", "none");
            $('#cntMes').css("display", "block");
            break;
    }
}

function validarEnvio() {
    var resultado = "";
    var empresa = $('#cbEmpresa').val();
    var lectura = $('#cbLectura').val();
    var formato = $('#cbFormato').val();
    if (empresa == 0) {
        resultado = "Seleccionar Empresa";
        return resultado;
    }
    if (lectura == 0) {
        resultado = "Seleccionar tipo de lectura";
        return resultado;
    }
    if (formato == 0) {
        resultado = "Seleccionar formato";
        return resultado;
    }


    return resultado;
}

    
    mostrarIngresoEnvio = function(){
        window.location.href = controlador + "envio/mostrarhojaexcelweb?id=0";
    }

    mostrarAlerta = function (alerta) {
        $('#mensaje').removeClass("action-message");
        $('#mensaje').removeClass("action-exito");
        $('#mensaje').addClass("action-alert");
        $('#mensaje').html(alerta);
        $('#mensaje').css("display", "block");
    }

    mostrarExito = function (mensaje) {
        $('#mensaje').removeClass("action-alert");
        $('#mensaje').removeClass("action-message");
        $('#mensaje').addClass("action-exito");
        $('#mensaje').html(mensaje);
    }

    mostrarError = function (alerta)
    {
        $('#mensaje').removeClass("action-message");
        $('#mensaje').removeClass("action-exito");
        $('#mensaje').addClass("action-alert");
        $('#mensaje').html("alerta");
        $('#mensaje').css("display", "block");
    }

    function listarFormato(idLectura)
    {                
        $.ajax({
            type: 'POST',
            url: controlador + "ListarFormatosXLectura",
            data: {
                idLectura: idLectura, idEmpresa: $('#cbEmpresa').val(), idModulo: $('#hfIdModulo').val()
            },
            success: function (evt) {               
                $('#listTipoInformacion').html(evt);
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    }

    function cargarFormatos() {
        idLectura = $('#cbLectura').val();
        $.ajax({
            type: 'POST',
            url: controlador + 'cargaFormatosXLectura',
            dataType: 'json',
            data: {
                idLectura: idLectura, idEmpresa: $('#cbEmpresa').val(), idModulo: $('#hfIdModulo').val()
            },
            cache: false,
            success: function (aData) {
                
                $('#cbFormato').get(0).options.length = 0;
                //$('#cbFormato').get(0).options[0] = new Option("--SELECCIONE--", "");
                $.each(aData, function (i, item) {
                    $('#cbFormato').get(0).options[$('#cbFormato').get(0).options.length] = new Option(item.Text, item.Value);
                    horizonte();
                });

                if ($('#cbFormato').get(0).options.length == 0)
                {
                    $('#cbFormato').get(0).options[0] = new Option("--No hay registros--", "");
                }
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    }

    function cargarLectura() {
        var idArea = $("#cbArea").val();
        $.ajax({
            type: 'POST',
            url: controlador + 'cargaLecturaXArea',
            dataType: 'json',
            data: {
                idArea: idArea, idModulo: $('#hfIdModulo').val()
            },
            cache: false,
            success: function (aData) {
                $('#cbLectura').get(0).options.length = 0;
                //$('#cbLectura').get(0).options[0] = new Option("--SELECCIONE--", "0");
                $.each(aData, function (i, item) {
                    $('#cbLectura').get(0).options[$('#cbLectura').get(0).options.length] = new Option(item.Text, item.Value);
                });
                if ($('#cbLectura').get(0).options.length == 0) {
                    $('#cbLectura').get(0).options[0] = new Option("--No hay registros--", "");
                }
                else {
                    cargarFormatos();
                }
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    }

    function buscarPeriodo(valor) {// valor: formatCodi

        for (var i = 0 ; i < listFormatCodi.length; i++)
            if (listFormatCodi[i] == valor) return listFormatPeriodo[i];

    }

    function pintaDescripcion(valor){
        $('#descripcion').html("<strong>Descripción del Formato</strong><br />" + buscarDescrip(valor));
    }

    function buscarDescrip(valor) {// valor: formatCodi

        for (var i = 0 ; i < listFormatCodi.length; i++)
            if (listFormatCodi[i] == valor) return listFormatDescrip[i];

    }

    function abrirFormatoExcelWeb(idEnvio) {
        window.location.href = controlador + "envio/mostrarhojaexcelweb?id=0";
    }
    
    function mostrarErrores() {
        $('#idTerrores').html(dibujarTablaError());
        setTimeout(function () {
            $('#validaciones').bPopup({
                easing: 'easeOutBack',
                speed: 450,
                transition: 'slideDown',
                modalClose: false
            });
            $('#tablaError').dataTable({
                "scrollY": 330,
                "scrollX": true,
                "sDom": 't',
                "ordering": false,
                "bPaginate": false,
                "iDisplayLength": -1
            });

        }, 50);
    }