var controlador = siteRoot + 'calculoresarcimiento/calidadproducto/';

$(function () {

    $('#btnNuevo').on('click', function () {
        editarEvento(0);
    });

    $('#btnExportar').on('click', function () {
        exportarEventos();
    });

    $('#btnConsultar').on('click', function () {
        consultar();
    });

    $('#cbAnio').val($('#hfAnio').val());
    $('#cbMes').val($('#hfMes').val());

    consultar();

});

function consultar() {
    limpiarMensaje('mensaje');
    if ($('#cbAnio').val() != "0" && $('#cbMes').val() != "0") {
        $.ajax({
            type: 'POST',
            url: controlador + 'eventolista',
            data: {
                anio: $('#cbAnio').val(),
                mes: $('#cbMes').val()                
            },
            success: function (evt) {
                $('#listado').html(evt);
                
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    }
    else {
        $('#listado').html("");
        if ($('#cbAnio').val() == "0" && $('#cbMes').val() == "0")
            mostrarMensaje('mensaje', 'alert', 'Seleccione año y mes.');
        else if ($('#cbAnio').val() == "0" && $('#cbMes').val() != "0")
            mostrarMensaje('mensaje', 'alert', 'Seleccione año.');
        else if ($('#cbAnio').val() != "0" && $('#cbMes').val() == "0")
            mostrarMensaje('mensaje', 'alert', 'Seleccione mes.');
    }
}

function editarEvento(id) {
    $.ajax({
        type: 'POST',
        url: controlador + 'EventoEditar',
        data: {
            id: id           
        },
        global: false,
        success: function (evt) {
            $('#contenidoEvento').html(evt);
            setTimeout(function () {
                $('#popupEvento').bPopup({
                    autoClose: false,
                    modalClose: false,
                    escClose: false,
                    follow: [false, false]
                });
            }, 50);

            $('#txtFechaInicial').inputmask({
                mask: "1/2/y h:s",
                placeholder: "dd/mm/yyyy hh:mm",
                alias: "datetime",
                hourFormat: "24"
            });

            $('#txtFechaInicial').Zebra_DatePicker({
                readonly_element: false,
                onSelect: function (date) {
                    $('#txtFechaInicial').val(date + " 00:00");
                }
            });

            $('#txtFechaFinal').inputmask({
                mask: "1/2/y h:s",
                placeholder: "dd/mm/yyyy hh:mm",
                alias: "datetime",
                hourFormat: "24"
            });

            $('#txtFechaFinal').Zebra_DatePicker({
                readonly_element: false,
                onSelect: function (date) {
                    $('#txtFechaFinal').val(date + " 00:00");
                }
            });

            $('#btnGrabarEvento').on("click", function () {
                grabarEvento();
            });

            $('#btnCancelarEvento').on("click", function () {
                $('#popupEvento').bPopup().close();
            });

            $('#btnAgregarEmpresa').on("click", function () {
                agregarEmpresa();
            });

            $('#cbEmpresa1').val($('#hfEmpresa1').val());
            $('#cbEmpresa2').val($('#hfEmpresa2').val());
            $('#cbEmpresa3').val($('#hfEmpresa3').val());

            if ($('#hfAcceso').val() == "S") {
                $('#cbAcceso').prop('checked', true);
            }

            $('#cbAnioEvento').val($('#hfAnioEvento').val());
            $('#cbsMesEvento').val($('#hfMesEvento').val());
            
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}

function grabarEvento() {
    var validacion = validarEvento();

    if (validacion == "") {
        $('#hfAcceso').val('N');
        if ($('#cbAcceso').is(':checked')) $('#hfAcceso').val('S');

        var count = 0;
        var items = "";
        $('#tbEmpresa>tbody tr').each(function (i) {
            $punto = $(this).find('#hfItemEmpresa');
            var constante = (count > 0) ? "," : "";
            items = items + constante + $punto.val();
            count++;
        });

        $('#hfEmpresas').val(items);

        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarEvento',
            data: $('#frmRegistroEvento').serialize(),
            dataType: 'json',
            global: false,
            success: function (result) {
                if (result == 1) {
                    mostrarMensaje('mensaje', 'exito', 'Los datos se grabaron correctamente.');
                    $('#popupEvento').bPopup().close();
                    consultar();
                }
                else if (result == 2) {
                    mostrarMensaje('mensajeEvento', 'alert', 'La fecha debe estar dentro de las fechas del periodo.');
                }
                else {
                    mostrarMensaje('mensajeEvento', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensajeEvento', 'error', 'Se ha producido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensajeEvento', 'alert', validacion);
    }
}

function validarEvento() {
    var html = "<ul>";
    var flag = true;
    var porcentaje = 0;
    html = html + "</ul>";

    if ($('#cbAnioEvento').val() == "") {
        html = html + "<li>Seleccione año.</li>";
        flag = false;
    }

    if ($('#cbsMesEvento').val() == "") {
        html = html + "<li>Seleccione mes.</li>";
        flag = false;
    }

    if ($('#txtFechaInicial').val() == '') {
        html = html + "<li>Ingrese la hora inicial.</li>";
        flag = false;
    }
    else {
        if (!$('#txtFechaInicial').inputmask("isComplete")) {
            html = html + "<li>Ingrese hora inicial.</li>";
            flag = false;
        }
    }

    if ($('#txtFechaFinal').val() == '') {
        html = html + "<li>Ingrese la hora final.</li>";
        flag = false;
    }
    else {
        if (!$('#txtFechaFinal').inputmask("isComplete")) {
            html = html + "<li>Ingrese hora final.</li>";
            flag = false;
        }
    }

    if ($('#txtFechaInicial').inputmask("isComplete") && $('#txtFechaFinal').inputmask("isComplete")) {
        if (getDateTime($('#txtFechaInicial').val()) > getDateTime($('#txtFechaFinal').val())) {
            html = html + "<li>La fecha final debe ser mayor a la fecha inicial.</li>";
            flag = false;
        }
    }

    if ($('#txtPuntoEntrega').val() == "") {
        html = html + "<li>Ingrese punto de entrega.</li>";
        flag = false;
    }

    if ($('#txtTension').val() == "") {
        html = html + "<li>Ingrese nivel de tensión.</li>";
        flag = false;
    }
    else {
        if (!validarNumero($('#txtTension').val())) {
            html = html + "<li>La tensión debe ser un dato numérico.</li>";
            flag = false;
        }        
    }


    if ($('#cbEmpresa1').val() == "") {
        html = html + "<li>Seleccione responsable 1.</li>";
        flag = false;
    }

    if ($('#txtPorcentaje1').val() == "") {
        html = html + "<li>Ingrese el Porcentaje del Responsable 1</li>";
        flag = false;
    }
    else {
        if (!validarNumero($('#txtPorcentaje1').val())) {
            html = html + "<li>El Porcentaje de Responsable 1 debe ser numérico.</li>";
            flag = false;
        }
        else {
            porcentaje = porcentaje + parseFloat($('#txtPorcentaje1').val());
        }
    }

    if (($('#cbEmpresa2').val() != "" && $('#txtPorcentaje2').val() == "") || ($('#cbEmpresa2').val() == "" && $('#txtPorcentaje2').val() != "")) {
        html = html + "<li>Para el Responsable 2, debe seleccionar empresa e ingresar porcentaje.</li>";
        flag = false;
    }
    else {
        if ($('#cbEmpresa2').val() != "" && $('#txtPorcentaje2').val() != "") {
            if (!validarNumero($('#txtPorcentaje2').val())) {
                html = html + "<li>El Porcentaje de Responsable 2 debe ser numérico.</li>";
                flag = false;
            }
            else {
                porcentaje = porcentaje + parseFloat($('#txtPorcentaje2').val());
            }
        }
    }
    if (($('#cbEmpresa3').val() != "" && $('#txtPorcentaje3').val() == "") || ($('#cbEmpresa3').val() == "" && $('#txtPorcentaje3').val() != "")) {
        html = html + "<li>Para el Responsable 3, debe seleccionar empresa e ingresar porcentaje.</li>";
        flag = false;
    }
    else {
        if ($('#cbEmpresa3').val() != "" && $('#txtPorcentaje3').val() != "") {
            if (!validarNumero($('#txtPorcentaje3').val())) {
                html = html + "<li>El Porcentaje de Responsable 3 debe ser numérico.</li>";
                flag = false;
            }
            else {
                porcentaje = porcentaje + parseFloat($('#txtPorcentaje3').val());
            }
        }
    }


    if ($('#txtComentario').val() == "") {
        html = html + "<li>Debe ingresar el comentario del Evento.</li>";
        flag = false;
    }

    if (porcentaje != 100 && flag) {
        html = html + "<li>Los porcentajes deben sumar 100%.</li>";
        flag = false;
    }

    var count = $('#tbEmpresa >tbody >tr').length;

    if (count == 0) {
        html = html + "<li>Debe agregar al menos un Suministrador involucrado.</li>";
        flag = false;
    }

    if (flag) {
        html = "";
    }
    return html;
}

function eliminarEvento(id) {
    if (confirm('¿Está seguro de realizar esta operación?')) {

        $.ajax({
            type: 'POST',
            url: controlador + 'EliminarEvento',
            data: {
                id: id
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    mostrarMensaje('mensaje', 'exito', 'El registro se eliminó correctamente.');
                    consultar();
                }
                else {
                    mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}

function agregarEmpresa() {
    if ($('#cbInvolucrado').val() != "") {               
        var flag = true;
        $('#tbEmpresa>tbody tr').each(function (i) {
            $punto = $(this).find('#hfItemEmpresa');
            if ($punto.val() == $('#cbInvolucrado').val()) {
                flag = false;
            }
        });

        if (flag) {
            $('#tbEmpresa> tbody').append(
                '<tr>' +
                '   <td style="text-align:center">' +
                '       <input type="hidden" id="hfItemEmpresa" value="' + $('#cbInvolucrado').val() + '" /> ' +
                '       <img src="' + siteRoot + 'Content/Images/btn-cancel.png" alt="" onclick="$(this).parent().parent().remove();" style="cursor:pointer" />' +
                '   </td>' +
                '   <td>' + $('#cbInvolucrado option:selected').text() + '</td>' +
                '</tr>'
            );
        }
        else {
            mostrarMensaje('mensajeEvento', 'alert', 'La empresa ya se encuentra agregada.');
        }
    }
    else {
        mostrarMensaje('mensajeEvento', 'alert', 'Seleccione un involucrado.');
    }
}

function exportarEventos() {
    limpiarMensaje('mensaje');
    if ($('#cbAnio').val() != "" && $('#cbMes').val() != "") {
    $.ajax({
        type: 'POST',
        url: controlador + "ExportarEventos",
        data: {
            anio: $('#cbAnio').val(),
            mes: $('#cbMes').val()
        },
        dataType: 'json',
        cache: false,
        success: function (resultado) {
            if (resultado == 1) {
                location.href = controlador + "DescargarEventos";
            }
            else {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
    }
    else {
        mostrarMensaje('mensaje', 'alert', 'Seleccione año y mes.');
    }
}


function cargarMedicion(id) {
    document.location.href = siteRoot + "calculoresarcimiento/medicion/index/?id=" + id;
}


function getDateTime(date) {
    var parts = date.split(' ');
    var fecha = parts[0].split("/");;
    var hora = parseInt(parts[1].split(":")[0]);
    var minuto = parseInt(parts[1].split(":")[1]);

    var date1 = new Date(fecha[1] + "/" + fecha[0] + "/" + fecha[2]);
    return date1.getTime() + (minuto * 60 + 60 * 60 * hora) * 1000;
}


/////////////////////////////////////////////////////////////////////////////
///                              Reporte Word                              //
/////////////////////////////////////////////////////////////////////////////

function generarReporte(idEvento) {
    
    limpiarMensaje("mensaje");
    
    $.ajax({
        type: 'POST',
        url: controlador + 'GenerarReporteInformeCompensacionMalaCalidad',
        data: {
            reevprcodi: idEvento,            
        },
        dataType: 'json',
        success: function (evt) {
            if (evt.Resultado != "-1") {
                window.location = controlador + "Exportar?file_name=" + evt.Resultado;
            } else {
                mostrarMensaje('mensaje', 'error', 'Error: ' + evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
    
}

/////////////////////////////////////////////////////////////////////////////
///                           Envío de correo                              //
/////////////////////////////////////////////////////////////////////////////

function iniciarEnvioCorreo(idEvento) {
    limpiarMensaje("mensaje");
    limpiarMensaje("mensaje_Notificacion");
    $.ajax({
        type: 'POST',
        url: controlador + 'cargarDatosCorreo',
        data: {
            reevprcodi: idEvento,
        },
        dataType: 'json',
        success: function (evt) {
            if (evt.Resultado != "-1") {
                var htmlFiltro = dibujarSeccionFiltro(evt.Entidad);
                $("#div_filtro").html(htmlFiltro);

                var htmlPlantilla = dibujarPlantilla(evt.Correo);
                $("#div_detalle").html(htmlPlantilla);

                $.ajax({
                    success: function () {
                        readonly = 1;

                        //muestro editor de contenido
                        iniciarControlTexto('Contenido', readonly);
                    }
                });


                $('#btnEnviarCorreo').click(function () {                    
                    realizarEnvioMensaje(idEvento);
                });

                abrirPopup("popupCorreoNotificacion");
            } else {
                mostrarMensaje('mensaje', 'error', 'Error: ' + evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });

    
}


function dibujarSeccionFiltro(evento) {

    var mesAnio = obtenerMes(evento.Reevprmes) + " " + evento.Reevpranio;
    var ptoEntrega = evento.Reevprptoentrega;
    var tension = evento.Reevprtension;

    var cadena = '';

   

    cadena += `
    `;
    cadena += `           
            <div class="search-content">
                    <table class="content-tabla-search" style="width:auto">
                        <tr>
                            <td>Mes/Año:</td>
                            <td>
                                <input type="text" id="txtAnio" style="width: 120px; text-align: center;" value="${mesAnio}" disabled />
                            </td>

                            <td style="padding: 3px 10px 3px 12px;">Punto Entrega:</td>
                            <td>
                                <input type="text" id="txtAnio" style="width: 170px; text-align: center;" value="${ptoEntrega}" disabled />
                            </td>

                            <td style="padding: 3px 10px 3px 12px;">Tensión:</td>
                            <td>
                                <input type="text" id="txtAnio" style="width: 60px;" value="${tension}" disabled />
                            </td>

                        </tr>
                    </table>
                </div>
    `;

    return cadena;
}

function dibujarPlantilla(objCorreo) {

    var esEditable = false;
    var habilitacion = "disabled";

    //Formato    
    var val_To = objCorreo.Corrto != null ? objCorreo.Corrto : "";
    var val_CC = objCorreo.Corrcc != null ? objCorreo.Corrcc : "";
    var val_BCC = objCorreo.Corrbcc != null ? objCorreo.Corrbcc : "";
    var val_Asunto = objCorreo.Corrasunto != null ? objCorreo.Corrasunto : "";
    var val_Contenido = objCorreo.Corrcontenido != null ? objCorreo.Corrcontenido : "";
   
    
    var cadena = '';

    cadena += `
            <table style="width:100%">    
                                
                <tr>
                    <td class="tbform-label">Para(*):</td>
                    
                    <td class="registro-control" style="width:850px;">
                        <textarea maxlength="450" name="To" id="To" value=""  cols="" rows="2" style="resize: none;width:840px;"  ${habilitacion}>${val_To}</textarea>
                    </td>
                </tr>                 
            
                <tr>
                    <td class="tbform-label">CC:</td>
                    <td class="registro-control" style="width:1100px;"><input name="CC" id="CC" type="text" value="${val_CC}" maxlength="120" style="width:840px;"   /></td>
                    
                </tr>
                <tr>
                    <td class="tbform-label"> BCC:</td>
                    <td class="registro-control" style="width:1100px;"><input name="BCC" id="BCC" type="text" value="${val_BCC}" maxlength="120" style="width:840px;" /></td>
                </tr>
                <tr>
                    <td class="tbform-label">Asunto(*):</td>
                    <td class="registro-control" style="width:850px;">
                        <textarea maxlength="450" name="Asunto" id="Asuntos" value=""  cols="" rows="3" style="resize: none;width:840px;"  ${habilitacion}>${val_Asunto}</textarea>
                    </td>
                    
                </tr>
                <tr>
                    <td class="tbform-label"> Contenido(*):</td>
                    <td class="registro-control" style="width:850px;">
                        <textarea name="Contenido" id="Contenido" maxlength="2000" cols="137" rows="16"  ${habilitacion}>
                            ${val_Contenido}
                        </textarea>
                        (*): Campos obligatorios.
                    </td>
                    <td class="registro-label">
                    
                    </td>
                </tr>
            </table>
    `;


    cadena += `
        <table style="width:100%">
            <tr>
                
                <td colspan="3" style="padding-top: 10px; text-align: center;">
                    <input type="button" id="btnEnviarCorreo" value="Enviar" />
                    <input type="button" id="btnCancelarEnviarCorreo" value="Cancelar" onclick="cerrarPopup('popupCorreoNotificacion')" />
                </td>
            </tr>
        </table>
    `;  


    return cadena;
}

function iniciarControlTexto(id, sololectura) {

    tinymce.remove();

    tinymce.init({
        selector: '#' + id,
        plugins: [
            //'paste textcolor colorpicker textpattern link table image imagetools preview'
            'wordcount advlist anchor autolink codesample colorpicker contextmenu fullscreen image imagetools lists link media noneditable preview  searchreplace table template textcolor visualblocks wordcount'
        ],
        toolbar1:
            //'insertfile undo redo | fontsizeselect bold italic underline strikethrough | forecolor backcolor | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link | table | image | mybutton | preview',
            'insertfile undo redo | styleselect fontsizeselect | forecolor backcolor | bullist numlist outdent indent | link | table | image | preview',

        menubar: false,
        readonly: sololectura,
        language: 'es',
        statusbar: false,
        convert_urls: false,
        plugin_preview_width: 1000,
        setup: function (editor) {
            editor.on('change',
                function () {
                    editor.save();
                });            
        },
       
    });

}

function realizarEnvioMensaje(idEvento) {
    limpiarMensaje("mensaje_Notificacion");

    var correo = {};
    correo = getCorreo();

    var msg = validarCamposCorreoAGuardar(correo);

    if (msg == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'validarCorreoEmpresas',
            contentType: "application/json",
            dataType: 'json',
            data: JSON.stringify({                
                reevprcodi: idEvento
            }
            ),
            cache: false,
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    if (evt.Resultado == "2") {
                        if (confirm('Algunas empresas suministradoras del evento no tienen registrados correos electrónicos, tales como: ' + evt.Mensaje + '. ¿Desea enviar la notificacion solo a las que si tengan correos registrados?')) {
                            enviarMensaje(idEvento);                           
                        }                  
                        
                    } else {
                        if (evt.Resultado == "3") {
                            mostrarMensaje('mensaje_Notificacion', 'alert', "Ningún suministrador del evento tienen registrado correos electrónicos.");
                        } else {
                            enviarMensaje(idEvento);                            
                        }                        
                    }
                } else {
                    mostrarMensaje('mensaje_Notificacion', 'error', evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje('mensaje_Notificacion', 'error', 'Ha ocurrido un error.' + evt.Mensaje);
            }
        });
    } else {
        mostrarMensaje('mensaje_Notificacion', 'error', msg);
    }
}


function enviarMensaje(idEvento) {
    
    var correo = {};
    correo = getCorreo();

    $.ajax({
        type: 'POST',
        url: controlador + 'EnviarNotificacionMalaCalidad',
        contentType: "application/json",
        dataType: 'json',
        data: JSON.stringify({
            correo: correo,
            reevprcodi: idEvento
        }
        ),
        cache: false,
        success: function (evt) {
            if (evt.Resultado != "-1") {
                if (evt.Resultado == "2") {
                    mostrarMensaje('mensaje', 'exito', "Las notificaciones fueron enviadas exitosamente...");
                } else {

                    cerrarPopup('popupCorreoNotificacion');

                    mostrarMensaje('mensaje', 'exito', "Las notificaciones fueron enviadas exitosamente...");
                }


            } else {
                mostrarMensaje('mensaje', 'error', evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.' + evt.Mensaje);
        }
    });
   
}


function getCorreo() {
    var obj = {};

    obj.Corrcc = $("#CC").val();
    obj.Corrbcc = $("#BCC").val();


    return obj;
}

function validarCamposCorreoAGuardar(correo) {
    var msj = "";

    /*********************************************** validacion del campo CC ***********************************************/
    var validaCc = validarCorreo($('#CC').val(), 0, -1);
    if (validaCc < 0) {
        msj += "<p>Error encontrado en el campo 'CC'. Revisar que el/los correo(s) sea(n) válido(s) y separados por punto y coma (;).</p>";
    }



    /*********************************************** validacion del campo BCC ***********************************************/
    var validaBcc = validarCorreo($('#BCC').val(), 0, -1);
    if (validaBcc < 0) {
        msj += "<p>Error encontrado en el campo 'BCC'. Revisar que el/los correo(s) sea(n) válido(s) y separados por punto y coma (;).</p>";
    }

    return msj;
}

function validarCorreo(valCadena, minimo, maximo) {
    var cadena = valCadena;

    var arreglo = cadena.split(';');
    var nroCorreo = 0;
    var longitud = arreglo.length;

    if (longitud == 0) {
        arreglo = cadena;
        longitud = 1;
    }

    for (var i = 0; i < longitud; i++) {

        var email = arreglo[i].trim();
        var validacion = validarDirecccionCorreo(email);

        if (validacion) {
            nroCorreo++;
        } else {
            if (email != "")
                return -1;
        }

    }

    if (minimo > nroCorreo)
        return -1;

    if (maximo > 0 && nroCorreo > maximo)
        return -2;

    return 1;
}

function validarDirecccionCorreo(email) {
    var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
}

function obtenerMes(numMes) {
    switch (numMes) {
        case 1: return "Enero"; break;
        case 2: return "Febrero"; break;
        case 3: return "Marzo"; break;
        case 4: return "Abril"; break;
        case 5: return "Mayo"; break;
        case 6: return "Junio"; break;
        case 7: return "Julio"; break;
        case 8: return "Agosto"; break;
        case 9: return "Setiembre"; break;
        case 10: return "Octubre"; break;
        case 11: return "Noviembre"; break;
        case 12: return "Diciembre"; break;
    }
}

function cerrarPopup(id) {
    $("#" + id).bPopup().close()
}

function abrirPopup(contentPopup) {
    setTimeout(function () {
        $("#" + contentPopup).bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    }, 50);
}