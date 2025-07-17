var controlador = siteRoot + 'CostoOportunidad/'

$(function () {
        

    $('#txtFecha').Zebra_DatePicker({       
    });
  
    $('#btnConsultar').click(function () {       
        mostrarExcelWeb();
    });
    
    $('#btnSelectArch').click(function () {

    });

    $('#btnEnviarDatos').click(function () {
        enviarExcelWeb();
    });
   
    crearPupload();
});

//function cargarPrevio() {
//    $('#cbTipoHorizonte').val(1);
//    //$('#FechaInicio').multipleSelect('checkAll');
//    $('#cbVariables').multipleSelect('checkAll');
//}

function mostrarExcelWeb() {

    if ($("#txtFecha").val() != "") {
        $('#mensajeEvento').css("display", "none");
        $("#hfIdEnvio").val(0);
        mostrarFormularioCostos();
    }
    else {
        alert("Error!.Ingresar fecha correcta");
    }
}

function mostrarFormularioCostos(accion) {
    //listErrores = [];    
    var fecha = $("#txtFecha").val();
    var idEnvio = $("#hfIdEnvio").val();
    //$('#hfFecha').val($('#txtFecha').val());       
    //var tipoCentral = $('#cbTipoCentral').val();     


    if (typeof hot != 'undefined') {
        hot.destroy();
    }
    $.ajax({
        type: 'POST',
        url: controlador + "MostrarCostosOportunidad",
        dataType: 'json',
        data: {
            fecha: fecha,
            idEnvio: idEnvio,
        },
        success: function (evt) {
            if (evt != -1) {
                crearGrillaExcelCostos(evt);
                evtHot = evt;
                switch (accion) {
                    case envioDatos:
                        var mensaje = mostrarMensajeEnvio(idEnvio);
                        mostrarExito("Los datos se grabaron correctamente. " + mensaje);
                        hideMensaje();
                        break;
                    case importarDatos:
                        mostrarExito("<strong>Los datos se cargaron correctamente, por favor presione el botón guardar para grabar.</strong>");
                        break;
                }
                if (evtHot.Handson.ReadOnly) {
                    $('#btnSelectExcel').css("display", "none");
                }
                else {
                    $('#btnSelectExcel').css("display", "block");
                }

                if (evt.Handson.ListaExcelData.length > 1) $('#barraCostos').css("display", "block");
                else $('#barraCostos').css("display", "none");
            }
            else {
                alert("La empresa no tiene puntos de medición para cargar.");
                document.location.href = controlador + 'index';
            }
        },
        error: function () {
            alert("Error al cargar Excel Web");
        }
    });
}

function enviarExcelWeb() {
    if (confirm("¿Desea enviar información a COES?")) {
         envioData();        
    }
}

function envioData() {
    var fecha = $("#txtFecha").val();
    var idEnvio = $("#hfIdEnvio").val();
    $.ajax({
        type: 'POST',
        dataType: 'json',
        //async: false,
        contentType: 'application/json',
        traditional: true,
        url: controlador + "GrabarExcelWeb",
        data: JSON.stringify({
            data: hot.getData()
            , fecha: fecha
            //,listaPtos: ListaPtos
        }),

        beforeSend: function () {
            mostrarExito("Enviando Información ..");
        },
        success: function (evt) {
            if (evt.Resultado == 1) {
                $("#hfIdEnvio").val(evt.IdEnvio);
                //mostrarFormulario(envioDatos);
                mostrarExito("Los datos se enviaron correctamente");
            }
            else {
                mostrarError("Error al Grabar:" + evt.Mensaje);
            }
        },
        error: function () {
            mostrarError();
        }
    });
}

function crearPupload() {
    //nroArchivo = 0;
    var fecha = $('#txtFecha').val();
    //var tipoCentral = $('#cbTipoCentral').val();    
    uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelectArch',
        //container: document.getElementById('container'),
        multipart_params: {
            //"tipoCentral": tipoCentral,            
            "fecha": fecha,
            "nArch": nroArchivo
        },
        url: siteRoot + 'CostoOportunidad/Carga/upload',
        flash_swf_url: 'Scripts/Moxie.swf',
        silverlight_xap_url: 'Scripts/Moxie.xap',
        multi_selection: true,
        filters: {
            max_file_size: '5mb',
            mime_types: [
                { title: "Archivos Dat .dat", extensions: "dat" }
            ]
        },
        init: {
            FilesAdded: function (up, files) {
                if (uploader.files.length != 4) {
                    alert("Se necesita 04 archivos de carga");
                    uploader.splice();
                    return;
                }
                uploader.bind('BeforeUpload', function (up, file) {
                    up.settings.multipart_params = {
                        // Actualiza los parametros de entrada para controller
                        //"tipoCentral": tipoCentral,                        
                        "fecha": fecha,
                        //"nArch": nroArchivo++
                    };
                });

                uploader.start();
                up.refresh();

            },
            UploadProgress: function (up, file) {
                showMensaje();
                mostrarAlerta("Por favor espere ...(<strong>" + file.percent + "%</strong>)");
                hideMensajeEvento();
            },
            UploadComplete: function (up, file) {

                uploader.splice();
                hideMensaje();
                alert / ("Alert mesje");
                var retorno = leerFileUpArchivo();
                if (retorno == 1 || retorno == 2) {
                    if (retorno == 2) { mostrarError("Uno de los archivos seleccionados es incorrecto!!!!"); }
                    else {
                        limpiarError();

                        $("#hfIdEnvio").val(-1);//-1 indica que el handsontable mostrara datos del archivo importado
                        mostrarFormularioCostos(importarDatos);
                    }
                }
                else { mostrarError("Error: Formato desconocido."); }
            },
            Error: function (up, err) {
                mostrarError(err.code + "-" + err.message);
            }
        }
    });
    uploader.init();
}