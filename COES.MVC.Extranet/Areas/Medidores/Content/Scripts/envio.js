
var controlador = siteRoot + 'Medidores/'
$(function () {
    
    $('#cbEmpresa').val("0");

    $("#CodigoEmpresa").change(function () {
        $('#mensajeFormato').html('');
        $.ajax({
            type: "POST",
            url: controlador + "Envio/EnlaceFormatoEmpresa",
            data: {
                empresa: $("#CodigoEmpresa").val()
            },
            beforeSend: beforeFunc,
            success: successFunc,
            error: errorFunc
        });
        function beforeFunc() {

        }

        function successFunc(data, status) {
            var mensajeError;
            var ubicacionArchivo = data.direccionUrl;
            //ubicacionArchivo += data.direccionUrl;

            if (data.direccionUrl == "") {
                $('#mensajeFormato').html('<div class="alert alert-danger"><strong>¡Error!</strong> Formato no disponible, favor contactarse con el administrador</p> ');
                $("#file").attr("disabled", "disabled");
            }
            else {

                if (data.habilitaCargaFormato) {
                    $("#file").attr("disabled", null);
                }
                else {
                    $("#file").attr("disabled", "disabled");
                }
            }
            if (data.FueraDePlazo) {
                $("#mensajePlazo").html('<div class="alert alert-danger"><strong>¡Formato Fuera de Plazo.!</strong></div>');
            }
            else {
                $("#mensajePlazo").html('<div class="alert alert-success"><strong>¡Formato en Plazo.!</strong> </div>');
            }
        }

        function errorFunc() {

            $('#mensajeFormato').html(
                '<div class="alert alert-danger"><strong>¡Error!</strong> Formato no disponible, favor contactarse con el administrador</p> '

            );

        }

    });

    $("#descarga").click(function () {
        $.ajax({
            type: "POST",
            url: controlador + "Envio/EnlaceFormatoEmpresa",
            data: {
                empresa: $("#CodigoEmpresa").val()
            },
            beforeSend: beforeFunc,
            success: successFunc,
            error: errorFunc
        });
        function beforeFunc() {
        }

        function successFunc(data, status) {
            var mensajeError;
            var ubicacionArchivo = data.direccionUrl;
            if (data.direccionUrl == "")

                $('#mensajeFormato').html('<div class="alert alert-danger"><strong>¡Error!</strong> Formato no disponible, favor contactarse con el administrador</p> ');
            else

                window.location.href = siteRoot + 'Publicacion/' + ubicacionArchivo;
        }

        function errorFunc() {

            $('#mensajeFormato').html(
                '<div class="alert alert-danger"><strong>¡Error!</strong> Formato no disponible, favor contactarse con el administrador</p> '

            );

        }



    });

    $("#btn_continuar").click(function () {
        limpiar_paso2();
    });

    $("#btn_regresar").click(function () {
        limpiar_paso1();
    });

    $("#subir").click(function () {
        $("#panel-carga").css("display", "block");

    });

    $("#file").change(function () {
        var extension = ($("#file").val().substring($("#file").val().lastIndexOf("."))).toLowerCase();
        limpiar_paso2();
        if (extension == '.xls' || extension == '.xlsx')
            $("#subir").attr("disabled", null);
        else {
            if (extension != "") {
                $("#file").attr("value", null);
                $("#subir").attr("disabled", "disabled");
                alert("¡Error! seleccionar archivo válido");
            }
        }
    });

    $('#file-uploader').ajaxForm(

        {
            beforeSend: function () {
                var fecha_actual = new Date();
                var gifURL = '@Url.Content("~/Images/ajax_load.gif")'

                $('#pasarVariable').attr('value', fecha_actual);
                $("#subir").attr("disabled", "disabled");
                $("#resultado").html('<span class="badge">1</span>Enviando Archivo ...<img src="' + gifURL + '" />');
                $("#resultado0").html('<p></p> ');
                $("#resultado1").html('<p></p> ');
                $("#resultado2").html('<p></p> ');
                $('#file').attr('value', null);
            },
            success: function (res) {

                // Si se está usando IE9 o inferior, el resultado se recibe como texto plano, es decir, como string.
                // Por lo tanto, hay que convertirlo a objeto JSON, usando la función parse.
                var fecha_inicial = new Date($('#pasarVariable').val());
                var fecha_final = new Date();
                var diferencia_fecha = fecha_final - fecha_inicial;
                diferencia_fecha = diferencia_fecha / 1000;

                if (typeof (res) == 'string') {
                    res = JSON.parse(res);
                }
                if (res.errorUpload == 0) {
                    $("#resultado").html('<span class="badge">1</span>Archivo enviado en <strong>' + diferencia_fecha + ' segundos</strong>');
                    $("#resultado0").html('<span class="badge">2</span><strong>Código de Envío: ' + res.codigo + '</strong>');
                    validaArchivo(res.ruta, res.archivo, res.empresa, res.codigo);
                }
                else {
                    $("#resultado").html('<span class="badge">1</span>Error en el envío de archivo:' + res.mensaje);
                }

            },

            error: function (ex) {
                //*alert(JSON.stringify(ex.responseText))
                $("#resultado").html('<p></p> ');
                $("#resultado0").html('<p></p> ');
                $("#resultado1").html('<p></p> ');
                $('#resultado2').html(ex.responseText);
            }
        });

    $("#btnConsolidado").live("click", function () {
        var codigo = $('#CodigoEmpresa').val();
        //var url = '@Url.Action("ConsolidadoEnvioXEmpresa", "Reportes")?codigoEmpresa=' + codigo;
        var url = controlador + "Reportes/ConsolidadoEnvioXEmpresa?=" + codigo;
        window.location.href = url;
    });
});

function limpiar_paso1() {
    $("#panel-descarga").css("display", "block");
    $("#panel-envio").css("display", "none");
    $("#panel-mensaje-error").css("display", "none");
    $("#resultado_detalle").html("");
    $("#divDetalle").css("display", "none");
    $("#resultado_obs").html("");
    $("#divEnvio").css("display", "none");
}

function limpiar_paso2() {
    $("#panel-descarga").css("display", "none");
    $("#panel-mensaje-error").css("display", "none");
    $("#panel-envio").css("display", "block");
    $("#resultado_detalle").html("");
    $("#divDetalle").css("display", "none");
    $("#resultado").html("");
    $("#resultado_obs").html("");
    $("#divEnvio").css("display", "none");
}

function validaArchivo(ruta, nombre, empresa, codigo) {
    $.ajax({
        type: "POST",
        url: controlador + "Envio/ValidarArchivo",
        dataType: "json",
        data: {
            pPpathArchivo: ruta,
            pNombreArchivo: nombre,
            pEmpresa: empresa,
            pCodigoEnvio: codigo

        },

        cache: false,
        beforeSend: beforeFunc(),
        success: successFunc,
        error: errorFunc
    });
    function beforeFunc() {
        var gifURL = '@Url.Content("~/Images/ajax_load.gif")';
        var endu = (new Date()).getTime();
        $("#resultado").html('<span class="badge">3</span>Confirmando Recepción ...<img src="' + gifURL + '" />');
    }

    function errorFunc() {
        $('#resultado2').html(
        '<div class="alert alert-danger"><strong>¡Error!</strong> Error </p> '
        );
    }
    function successFunc(res) {
        var mensajeError;
        mensajeError = res.mensaje + "<br>";

        /// Arma Tabla resumen de errores
        var mensaje = "";
        var cadenaResumen = "";//"<ul><li>Resumen de las observaciones encontradas en el archivo</li></ul>";
        cadenaResumen += '<table class="table table-bordered table-condensed" style="background-color: #ffffff;"><tr><td><strong>Observación</strong></td><td><strong>Cantidad</strong></td></tr>';
        var totalObservaciones = 0;


        $.each(res.ListaTipoError, function (posicion, tipoError) {
            if (tipoError.TotalObservacion > 0) {
                totalObservaciones++;
                mensaje = tipoError.Mensaje;
                cadenaResumen += '<tr><td>' + mensaje + '</td><td>' + tipoError.TotalObservacion + '</td></tr>';
            }
        }
        );
        if (totalObservaciones == 0)
            cadenaResumen = "";
        else
            cadenaResumen += "</table>";

        switch (res.tipoError) {
            case 1:
                $("#resultado").html('<div class="alert alert-danger">Error en Confirmación de Recepción </div> ');
                mensajeError += '<div class="alert alert-danger"><strong>¡Error!</strong>Existen errores en el archivo, corregir y vuelva a cargarlo</div>';
                $("#divEnvio").css("display", "block");
                $('#resultado_obs').html(cadenaResumen);
                $("#panel-mensaje-error").css("display", "block");
                break;
            case 2:


                mensajeError += '<div class="alert alert-warning"><strong>¡Sugerencia!</strong>¿Desea reemplazar los valores <strong>BLANCOS</strong> por <strong>CEROS</strong>? <button  id="btnSubmit" type="button" class="btn btn-primary btn-mini">SI</button>&nbsp;&nbsp;&nbsp;<button  id="btncancelar" type="button" class="btn-primary btn btn-mini">NO</button></div>';
                $("#resultado").html('Falló Confirmación de Recepción. ' + mensajeError);
                $('#resultado_obs').html(cadenaResumen);
                break;
            case 3:
                $("#resultado").html('Confirmación de Recepción Exitoso.');
                GrabarBDArchivo(res.nombre, res.empresa, res.codigo);

        }



        var cadena = '<table class="table table-bordered table-condensed" style="background-color: #ffffff;"> <thead><tr >';
        cadena = cadena + '<th align="CENTER"  width="50"><label class="control-label" ><strong>Celda</strong></label></th>';
        cadena = cadena + '<th align="CENTER"  width="120"><label class="control-label"><strong>Fecha</strong></label></th>';
        cadena = cadena + '<Th align="CENTER" width="100"><label class="control-label" ><strong>Registro</strong></label></Th>';
        cadena = cadena + '<Th align="CENTER" width="150"><label class="control-label" ><strong>Observaciones</strong></label></Th>';
        cadena = cadena + '<Th align="CENTER" width="150"><label class="control-label" ><strong>Central</strong></label></Th>';
        cadena = cadena + '<Th align="CENTER" width="150"><label class="control-label" ><strong>Grupo/SSAA</strong></label></Th>';
        cadena = cadena + '<Th align="CENTER" width="100"><label class="control-label" ><strong>Variable<br />Operativa</label> </Th></tr></thead><tbody >';
        var color = '';
        var clase = '';
        var totalErrores = 0;
        if (res.tipoError < 3) {
            $.each(res.rows, function (posicion, errores) {
                totalErrores++;
                if (errores.tipoObservacion == 'VALOR NEGATIVO') {
                    //color = 'style="color: red;"';
                    clase = 'danger';
                }

                cadena = cadena + '<tr class="' + clase + '" ' + color + '><td>' + errores.Celda + '</td>';
                cadena = cadena + '<td style="font-size:9px;">' + errores.fechaError + '</td>';
                cadena = cadena + '<td style="font-size:9px;">' + errores.valor + '</td>';
                cadena = cadena + '<td style="font-size:9px;">' + errores.tipoObservacion + '</td>';
                cadena = cadena + '<td style="font-size:9px;">' + errores.central + '</td>';
                cadena = cadena + '<td style="font-size:9px;">' + errores.grupo + '</td>';
                if (errores.tipoInfo == "1") {
                    cadena = cadena + '<td style="font-size:9px;">MW</td></tr>';
                }
                else {
                    cadena = cadena + '<td>MVAR</td></tr>';
                }
                clase = '';
            }
            );

            cadena = cadena + '</tbody></table>';
            if (totalErrores > 0) {
                $("#divDetalle").css("display", "block");
                $('#resultado_detalle').html(cadena);
            }
        }
    }
}

function GrabarBDArchivo(nombre, empresa, codigo) {
    $.ajax({
        type: "POST",
        url: controlador + "Envio/GrabarBdArchivo",
        dataType: "json",
        data: {
            pNombreArchivo: nombre,
            pEmpresa: empresa,
            pCodigoEnvio: codigo
        },

        cache: false,
        beforeSend: beforeFunc(),
        success: successFunc,
        error: errorFunc
    });
    function beforeFunc() {
        var gifURL = '@Url.Content("~/Images/ajax_load.gif")';

        $("#resultado").html('<span class="badge">4</span> Grabando Información en Base de Datos ...<img src="' + gifURL);
    }
    function successFunc(res) {
        $('#pasarVariable').attr('value', res.CodigoEnvio);
        var url = '@Url.Action("ConsolidadoEnvio", "Reportes")?codEnvio=' + res.CodigoEnvio;
        // $("#resultado").html('<div class="alert alert-success"><strong>¡Exito!</strong> Archivo enviado correctamente !!! </div><button  id="btnConsolidado" type="button" class="btn btn-success btn-small">Ver Consolidado de envío</button>');

        if (res.tipoError != -1)
            $("#resultado").html('<span class="badge">4</span> Archivo Grabado en Base de Datos.</p><div class="alert alert-success"><strong>¡Exito!</strong>El archivo se cargó correctamente</div><button  id="btnConsolidado" type="button" class="btn btn-success btn-small">Ver Consolidado de envío</button>');
        else {
            $("#resultado").html('<span class="badge">4</span> Error en el envío.</p><div class="alert alert-danger"><strong>¡Error!</strong>' + res.mensaje + '</div> ');
            console.log(res.Mensaje);
        }

    }
    function errorFunc() {
        $('#resultado').html(
        '<div class="alert alert-danger"><strong>¡Error!</strong> Error </p> '
        );
    }

}

