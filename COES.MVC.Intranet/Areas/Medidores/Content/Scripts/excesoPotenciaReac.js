var controlador = siteRoot + 'Medidores/reportes/'

$(function () {

    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": false,
        "progressBar": true,
        "positionClass": "toast-top-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "3000",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }

    $("#cbTipoGen").change(function () {
        consultar();
    });
   /* $("#cbCentral").change(function () {
        consultar();
    });*/
    $("#cbEmpresa").change(function () {
        consultar();
    });

    $('#mes').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () {
            consultar();
        }
    });

    $('#btnConsultar').click(function () {
        consultar();
    });

    $('#btnExportar').click(function () {
        exportar();
    });
   
    consultar();
});

function consultar() {
    $("#reporteMaxDemanda").html("");
    if (validarConsulta) {
        ReporteExcesoPotenReactiva();
        ReporteExcesoPotenReactivaDet();
    }
}

function validarConsulta() {
    return true;
}

function ReporteExcesoPotenReactiva() {
    var idEmpresa = $("#cbEmpresa").val();
    var mes = $("#mes").val();
    var tipoGeneracion = $('#cbTipoGen').val();
   /* var tipoCentral = $('#cbCentral').val();*/

    var tipoCentral = 0;
    var valida = false;
    /*alert(valida);*/

    var retorno = false;
    var idParametro = $("#parametro").val();

    $.ajax({
        type: 'POST',
        async:false,
        url: controlador + 'GetRangoAnalisis',
        data: {
            idParametro : idParametro,
            mes: mes
        },
        success: function (result) {
         /*  alert(result);*/
            if (result=="true") {
                retorno = true;
            } else {
               /* alert("No se registro un rango de analisis de potencia inductiva para el mes indicado.");*/
                toastr.warning('No se registro un rango de analisis de potencia inductiva para el mes indicado.');
            }
            
        }
    });
   
    $.ajax({
        type: 'POST',
        url: controlador + 'ReporteExcesoPotenReactiva',
        data: {
            tipoCentral: tipoCentral,
            tipoGeneracion: tipoGeneracion,
            idEmpresa: idEmpresa,
            mes: mes
        },
        success: function (data) {            
            
            $('#reporteExcesoPotenciaReactiva').html(data);

           /* $('#tablaPotenciaReactiva').dataTable({
                "scrollY": 430,
                "scrollX": true,
                "sDom": 't',
                "ordering": false,
                "bPaginate": false,
                "iDisplayLength": -1
            });
            */
          /*  $('#reportePotenciaReactiva').html(data);*/
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
    

}

function ReporteExcesoPotenReactivaDet() {
    var idEmpresa = $("#cbEmpresa").val();
    var mes = $("#mes").val();
    var tipoGeneracion = $('#cbTipoGen').val();    

    var tipoCentral = 0;
    var valida = false;  

    var retorno = false;
    var idParametro = $("#parametro").val();

    $.ajax({
        type: 'POST',
        url: controlador + 'ReporteExcesoPotenReactivaDet',
        data: {
            tipoCentral: tipoCentral,
            tipoGeneracion: tipoGeneracion,
            idEmpresa: idEmpresa,
            mes: mes
        },
        success: function (data) {           

              $('#reportePotenciaReactiva').html(data);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });


}

function exportar() {
    var idEmpresa = $("#cbEmpresa").val();
    var mes = $("#mes").val();
    var tipoGeneracion = $('#cbTipoGen').val();
    /*var tipoCentral = $('#cbCentral').val();*/
    var tipoCentral = 0;

    $.ajax({
        type: 'POST',
        url: controlador + 'ExportarExcesoPotenReactiva',
        data: {
            tipoCentral: tipoCentral,
            tipoGeneracion: tipoGeneracion,
            idEmpresa: idEmpresa,
            mes: mes
        },
        success: function (result) {
            if (result.length > 0) {
                archivo = result[0];
                nombre = result[1];
                if (archivo != '-1' && archivo !='2') {
                    window.location.href = controlador + 'DescargarArchivo?rutaArchivoTemp=' + archivo + "&nombreArchivo=" + nombre;
                } else {
                    if (archivo == '2') {
                        alert("No existen registros!!!");
                    }
                    if (archivo == '-1') {
                        alert("Error en descargar el archivo");
                    }
                    
                }
            }
            else {
                alert("Error en descargar el archivo");
            }
        },
        error: function () {
            alert('ha ocurrido un error al descargar el archivo excel.');
        }
    });

   
}
/*function ComprobarRango() {

    var mes = $("#mes").val();
    var retorno = false;

    $.ajax({
        type: 'POST',
        url: controlador + 'GetRangoAnalisis',
        data: {
            mes: mes
        },
        success: function (result) {
            retorno = true;
        }
    });

    return retorno;
}*/