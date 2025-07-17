var controlador = siteRoot + 'RegistroIntegrante/Solicitud/'
var RowRL = null;
$(function () {

    $("input[type=text]").keyup(function () {
        $("#" + this.id).val($("#" + this.id).val().toUpperCase());
    });


    $("#val500").prop('disabled', true);
    $("#val200").prop('disabled', true);
    $("#val138").prop('disabled', true);

    $('#chk500').change(function () {
        if (this.checked) {
            $("#val500").prop('disabled', false);
            $("#val500").removeClass("Deshabilitado");
            $("#val500").addClass("Habilitado");
        } else {
            $('#val500').val("");
            $("#val500").prop('disabled', true);
            $("#val500").removeClass("Habilitado");
            $("#val500").addClass("Deshabilitado");
            SumarLineasTransmision();
        }
    });

    $('#chk200').change(function () {
        if (this.checked) {
            $("#val200").prop('disabled', false);
            $("#val200").removeClass("Deshabilitado");
            $("#val200").addClass("Habilitado");
        } else {
            $('#val200').val("");
            $("#val200").prop('disabled', true);
            $("#val200").removeClass("Habilitado");
            $("#val200").addClass("Deshabilitado");
            SumarLineasTransmision();
        }
    });

    $('#chk138').change(function () {
        if (this.checked) {
            $("#val138").prop('disabled', false);
            $("#val138").removeClass("Deshabilitado");
            $("#val138").addClass("Habilitado");
        } else {
            $('#val138').val("");
            $("#val138").prop('disabled', true);
            $("#val138").removeClass("Habilitado");
            $("#val138").addClass("Deshabilitado");
            SumarLineasTransmision();
        }
    });

    cargarRL();
    OcultarTipoAgente();
    $("#TipoEmpresa").change(function () {
        MostrarCamposPorTipoAgente(this.value);
    })

    $('#btnAgregarRL').click(function () {
        AgregarRL();
    });  

    var estado = $("#txtESTADO").val();
    
    if (estado == "") //NUEVO
    {     
        $('#divEstado').hide();

        $('#divObservacion').hide();
        $('#divObservacionTitulo').hide();

        $('#divDocumentoSustentatorio').hide();
        $('#divDocumentoSustentatorioTitulo').hide();
    }
    else if (estado == "PENDIENTE" || estado == "APROBADO_FISICA" || estado == "APROBADO_DIGITAL") {
        
        $('#btnAgregarRL').hide();
        $('#btnGrabarNuevo').hide();
        
        $('#divObservacion').hide();
        $('#divObservacionTitulo').hide();

        $('#divSolicitar').hide();
        $('#divUpload').hide();

    } else if (estado == "DENEGADO") {
        $('#btnAgregarRL').hide();
        $('#btnGrabarNuevo').hide();

        $('#divSolicitar').hide();
        $('#divUpload').hide();
    }

    var solicitudencurso = $("#hdfSolicitudenCurso").val();
    if (solicitudencurso == "SI") {
        $('#mensaje').html("Ya existe una solicitud en estado PENDIENTE, no es posible iniciar una nueva.");
        $("#mensaje").show();
        deshabilitaControles();
    }

    

    $('#btnVer').click(function () {
        window.open(controlador + 'ver?url=' + $('#hdfAdjunto').val(), "_blank", 'fullscreen=yes');
    });

    $('#btnDescargar').click(function (e) {
        document.location.href = controlador + 'Download?url=' + $('#hdfAdjunto').val() + "&nombre=" + $('#hdfNombreArchivo').val();
    });

    $('#btnCancelarNuevo').click(function () {
        $('#popupAgregar').bPopup().close();
        LimpiarRepresentanteLegalPopUp();
    });


    $(".SumTrans").keyup(function () {
        SumarLineasTransmision();
    });

    $("#txtPotenciaInstaladaTI").keyup(function () {
        var variante = parseInt($("#hfEmpresaCondicionVarianteGenerador").val());
        if ($(this).val() != "") {
            if (parseInt($(this).val()) >= variante) {
                $("#spnIntegranteGenerador").text("Integrante Obligatorio");
            } else {
                $("#spnIntegranteGenerador").text("Integrante Voluntario");
            }
        }
    });

    $("#txtMaximaDemandaCoincidenteAnual").keyup(function () {
        var variante = parseInt($("#hfEmpresaCondicionVarianteDistribuidor").val());
        if ($(this).val() != "") {
            if (parseInt($(this).val()) >= variante) {
                $("#spnIntegranteDistribuidor").text("Integrante Obligatorio");
            } else {
                $("#spnIntegranteDistribuidor").text("Integrante Voluntario");
            }
        }
    });

    $("#txtMaximaDemandaContratada").keyup(function () {
        var variante = parseInt($("#hfEmpresaCondicionVarianteUsuarioLibre").val());
        if ($(this).val() != "") {
            if (parseInt($(this).val()) >= variante) {
                $("#spnIntegranteUsuarioLibre").text("Integrante Obligatorio");
            } else {
                $("#spnIntegranteUsuarioLibre").text("Integrante Voluntario");
            }
        }
    });

});


ValidarSize = function (obj) {
    var fileSize = obj.files[0].size;
    var sizekiloByte = parseInt(fileSize / 1024);
    var sizeMegaByte = parseInt(sizekiloByte / 1024);
    if (sizeMegaByte > 8) {
        obj.value = "";
        alert('Ingrese un archivo menor a 8mb');
    }

    extension = (obj.value.substring(obj.value.lastIndexOf("."))).toLowerCase();
    if (extension != '.pdf') {
        obj.value = "";
        alert('Ingrese un archivo en formato PDF');
    }

}

SumarLineasTransmision = function () {
    var total = 0;
    var variante = parseInt($("#hfEmpresaCondicionVarianteTransmisor").val());

    $(".SumTrans").each(function () {
        if ($(this).val() != "") {
            total += parseInt($(this).val());
        }
    });
    if (total >= variante) {
        $("#spnIntegranteTransmisor").text("Integrante Obligatorio");
    } else {
        $("#spnIntegranteTransmisor").text("Integrante Voluntario");
    }
    $("#txtTotalTransmision").val(total);
}

cargarRL = function () {

    $.ajax({
        type: 'POST',        
        url: controlador + 'ListadoTipo',
        data: {
            solicodi: $('#hdfSolicodi').val()
        },
        success: function (evt) {
            $('#listadoRepresentante').html(evt);
        },
        error: function () {
            mostrarErrorAgregar();
        }
    });
}


BajaRL = function (obj) {

    var Row = $(obj).closest("tr")[0];
    RowRL = Row;
    if (Row.cells[1].innerText == "Principal") {
        alert("No se puede dar de baja al tipo Principal");
        return;
    }

    Row.cells[2].innerText = "BAJA";
}


EliminarRL = function (obj) {
    $(obj).closest('tr').remove();
}


EditarRepresentanteLegal = function (obj) {

    var Row = $(obj).closest("tr")[0];
    RowRL = Row;

    $("#TipoEmpresa").val(Row.cells[3].innerText); $("#TipoEmpresa").change();
    $("#TipoDocumentoSustentario").val(Row.cells[5].innerText); $("#TipoDocumentoSustentario").change();
    
    //7-baja, 8-editar, 9-quitar
    //10-Tipoarcdigitalizado, 11-Tipoarcdigitalizadofilename
    $("#txtPotenciaInstaladaTI").val(Row.cells[12].innerText);
    $("#txtNumeroCentralesTI").val(Row.cells[13].innerText);

    if (Row.cells[14].innerText == "true")
        $("#chk500").prop('checked', true);
    else
        $("#chk500").prop('checked', false);

    if (Row.cells[15].innerText == "true")
        $("#chk200").prop('checked', true);
    else
        $("#chk200").prop('checked', false);

    if (Row.cells[16].innerText == "true")
        $("#chk138").prop('checked', true);
    else
        $("#chk138").prop('checked', false);


    $("#val500").val(Row.cells[17].innerText);
    $("#val200").val(Row.cells[18].innerText);
    $("#val138").val(Row.cells[19].innerText);

    $("#txtTotalTransmision").val(Row.cells[20].innerText);

    $("#txtMaximaDemandaCoincidenteAnual").val(Row.cells[21].innerText);
    $("#txtMaximaDemandaContratada").val(Row.cells[22].innerText);
    $("#txtNumeroSuministrador").val(Row.cells[23].innerText);
    

    var Index = Row.cells[24].innerText;


    $(".ArchivoDigitalizado").each(function () {
        $(this).hide();
    });

    $("#TipoEmpresa").show();
    $("#TipoDocumentoSustentario").show();
    
    var estado = $("#txtESTADO").val();
    if (estado == "") {
        $("#flArchivoDigitalizadoTI" + Index).show();

    } else //Si no es nuevo es solo lectura
    {
        disabledFields();
        //Enlaces

        $('#hdfNombreArchivoDigitalizado').val(Row.cells[11].innerText);
        $('#verDigitalizado').show();

        $('#hdfAdjuntoDigitalizado').val(Row.cells[10].innerText);
        $('#descargarDigitalizado').show();
    }

    $('#popupAgregar').bPopup({
        modalClose: false,
        follow: [true, true],
        position: ['auto', 'auto'],
        positionStyle: 'fixed',
        onClose: function () {
            LimpiarRepresentanteLegalPopUp();
        }
    });
};

verDigitalizado = function () {
    window.open(controlador + 'ver?url=' + $('#hdfNombreArchivoDigitalizado').val(), "_blank", 'fullscreen=yes');
}

descargarDigitalizado = function () {
    document.location.href = controlador + 'Download?url=' + $('#hdfNombreArchivoDigitalizado').val() + '&nombre=' + $('#hdfAdjuntoDigitalizado').val();
}

AgregarRL = function () {


    RowRL = null;

    $(".ArchivoDigitalizado").each(function () {
        $(this).hide();
    });

    $("#flArchivoDigitalizadoTI").show();


    $("#mensajeAgregar").hide();
    enabledFields();
    
    $('#popupAgregar').bPopup({
        modalClose: false,
        follow: [true, true],
        position: ['auto', 'auto'],
        positionStyle: 'fixed',
        onClose: function () {
            LimpiarRepresentanteLegalPopUp();
        }
    });

    $("#btnGrabarNuevo").click(function () {

        if (!ValidarRequeridoAgregar()) {
            $("#mensajeAgregar").show();
            return false;
        } else
            $("#mensajeAgregar").hide();

        var validacion = validar();
        if (validacion == "") {
            $('#mensajeAgregar').removeClass();
            $('#mensajeAgregar').html("Todos los campos estan correctos");
            $('#mensajeAgregar').addClass('action-exito');
          

            if (RowRL == null) {

                if (!ValidarArchivosRL(-1)) {
                    return false;
                }

                var contador = $("#hfRepresentanteLegalContador").val(); 
                contador++;

                var btnEliminar = "";
                btnEliminar = "<td><a href='#' onclick='EliminarRL(this)'><img src='/Content/Images/btn-cancel.png' style='margin-top: 5px;' /></a></td>";

                var html = "";
                html += "<tr class='tblRLDetalle'>";
                html += "<td style='display:none;'></td>";
                html += "<td>Secundario</td>";
                html += "<td>AGREGADO</td>";
                html += "<td style='display: none;'>" + $("#TipoEmpresa").val() + "</td>";
                html += "<td>" + $("#TipoEmpresa option:selected").text() + "</td>";                
                html += "<td style='display: none;'>" + $("#TipoDocumentoSustentario").val() + "</td>";
                html += "<td>" + $("#TipoDocumentoSustentario option:selected").text() + "</td>";
                html += "<td></td>";
                html += "<td><a href='#' onclick='EditarRepresentanteLegal(this)'><img src='/Content/Images/btn-edit.png' style='margin-top: 5px;' /></a></td>";
                html += btnEliminar; 
                html += "<td style='display: none;'></td>";
                html += "<td style='display: none;'></td>";
                html += "<td style='display: none;'>" + $("#txtPotenciaInstaladaTI").val() + "</td>";
                html += "<td style='display: none;'>" + $("#txtNumeroCentralesTI").val() + "</td>";

                html += "<td style='display: none;'>" + $("#chk500").prop('checked') + "</td>";
                html += "<td style='display: none;'>" + $("#chk200").prop('checked') + "</td>"
                html += "<td style='display: none;'>" + $("#chk138").prop('checked') + "</td>";

                html += "<td style='display: none;'>" + $("#val500").val() + "</td>";
                html += "<td style='display: none;'>" + $("#val200").val() + "</td>"
                html += "<td style='display: none;'>" + $("#val138").val() + "</td>";

                html += "<td style='display: none;'>" + $("#txtTotalTransmision").val() + "</td>";

                html += "<td style='display: none;'>" + $("#txtMaximaDemandaCoincidenteAnual").val() + "</td>";
                html += "<td style='display: none;'>" + $("#txtMaximaDemandaContratada").val() + "</td>";
                html += "<td style='display: none;'>" + $("#txtNumeroSuministrador").val() + "</td>";


                html += "<td style='display:none;'>" + contador + "</td>";
                html += "</tr>";

                var FileInputDNI = document.getElementById("flArchivoDigitalizadoTI").cloneNode(true);
                FileInputDNI.setAttribute("id", "flArchivoDigitalizadoTI" + contador);
                FileInputDNI.setAttribute("class", "ArchivoDigitalizado");
                $(FileInputDNI).hide();
                document.getElementById("divDocumentoIdentidad").appendChild(FileInputDNI);
              
                $("#tablaRL tbody").append(html);
                $("#hfRepresentanteLegalContador").val(contador);

            } else {
                var Index = RowRL.rowIndex;
                if (!ValidarArchivosRL(Index)) {
                    return false;
                }
                
                RowRL.cells[3].innerText = $("#TipoEmpresa").val();
                RowRL.cells[4].innerText = $("#TipoEmpresa option:selected").text();
                RowRL.cells[5].innerText = $("#TipoDocumentoSustentario").val();
                RowRL.cells[6].innerText = $("#TipoDocumentoSustentario option:selected").text();
                //7-baja, 8-editar, 9-quitar
                //10-Tipoarcdigitalizado, 11-Tipoarcdigitalizadofilename
                RowRL.cells[12].innerText = $("#txtPotenciaInstaladaTI").val();
                RowRL.cells[13].innerText = $("#txtNumeroCentralesTI").val();

                RowRL.cells[14].innerText = $("#chk500").prop('checked');
                RowRL.cells[15].innerText = $("#chk200").prop('checked');
                RowRL.cells[16].innerText = $("#chk138").prop('checked');
                
                RowRL.cells[17].innerText = $("#val500").val();
                RowRL.cells[18].innerText = $("#val200").val();
                RowRL.cells[19].innerText = $("#val138").val();

                RowRL.cells[20].innerText = $("#txtTotalTransmision").val();

                RowRL.cells[21].innerText = $("#txtMaximaDemandaCoincidenteAnual").val();
                RowRL.cells[22].innerText = $("#txtMaximaDemandaContratada").val();
                RowRL.cells[23].innerText = $("#txtNumeroSuministrador").val();
            }

            RowRL = null;
            var bPopup = $('#popupAgregar').bPopup();
            bPopup.close();
            LimpiarRepresentanteLegalPopUp();

        } else {
            $("#mensajeAgregar").show();
            $('#mensajeAgregar').removeClass();
            $('#mensajeAgregar').html(validacion);
            $('#mensajeAgregar').addClass('action-alert');
        }
    });
}

ValidarRequeridoAgregar = function () {
    var validator = true;
    var contError = 0;
    // Validar datos

    var TipoAgente = "";
    $(".Tipo").each(function () {
        if ($(this).hasClass("Required")) {
            var spn = $(this).attr("data-validator-id");
            if ($(this).val() == "" || $(this).val() == "0" || $(this).val() == null) {
                $("#" + spn).addClass("Error");
                contError++;
            } else {
                $("#" + spn).removeClass("Error");
            }
        }
    });

    var GENERADOR = $("#hfTipoComportamientoGenerador_codigo").val();
    var TRANSMISOR = $("#hfTipoComportamientoTrasmisor_codigo").val();
    var DISTRIBUIDOR = $("#hfTipoComportamientoDistribuidor_codigo").val();
    var USUARIOLIBRE = $("#hfTipoComportamientoUsuarioLibre_codigo").val();


    var tipoIntegrante = $("#TipoEmpresa").val();
    if (tipoIntegrante == GENERADOR) {

        TipoAgente = "Generador";

        $(".GeneradorField").each(function () {
            if ($(this).hasClass("Required")) {
                var spn = $(this).attr("data-validator-id");
                if ($(this).val() == "" || $(this).val() == "0" || $(this).val() == null) {
                    $("#" + spn).addClass("Error");
                    contError++;
                } else {
                    $("#" + spn).removeClass("Error");
                }
            }
        })

        var potenciaInstaladaTI = $("#txtPotenciaInstaladaTI").val();
        if (potenciaInstaladaTI == "" || isNaN(potenciaInstaladaTI) || potenciaInstaladaTI == 0) {
            alert("Ingrese un valor válido en potencia instalada.");
            contError++;;
        }

        var numeroCentralesTI = $("#txtNumeroCentralesTI").val();
        if (numeroCentralesTI == "" || isNaN(numeroCentralesTI) || numeroCentralesTI == 0) {
            alert("Ingrese un valor válido en número de centrales.");
            contError++;;
        }

    } else if (tipoIntegrante == TRANSMISOR) {

        TipoAgente = "Transmision";

        var chk500 = $("#chk500").prop('checked');
        var val500 = $("#val500").val();

        var chk200 = $("#chk200").prop('checked');
        var val200 = $("#val200").val();

        var chk138 = $("#chk138").prop('checked');
        var val138 = $("#val138").val();

        var mensaje = "";
        var suma = val500 + val200 + val138
        if (suma == "" || isNaN(suma) || suma == 0) {
            alert("Agregue al menos una linea de transmision");
            contError++;;
        }

    } else if (tipoIntegrante == "3") {


        TipoAgente = DISTRIBUIDOR;

        $(".DistribuidorField").each(function () {
            if ($(this).hasClass("Required")) {
                var spn = $(this).attr("data-validator-id");
                if ($(this).val() == "" || $(this).val() == "0" || $(this).val() == null) {
                    $("#" + spn).addClass("Error");
                    contError++;
                } else {
                    $("#" + spn).removeClass("Error");
                }
            }
        })

        var maximaDemandaCoincidenteAnual = $("#txtMaximaDemandaCoincidenteAnual").val();
        if (maximaDemandaCoincidenteAnual == "" || isNaN(maximaDemandaCoincidenteAnual) || maximaDemandaCoincidenteAnual == 0) {
            alert("Ingrese un valor válido en Máxima demanda coincidente anual");
            contError++;;
        }

    } else if (tipoIntegrante == USUARIOLIBRE) {

        TipoAgente = "UsuarioLibre";

        $(".UsuarioLibreField").each(function () {
            if ($(this).hasClass("Required")) {
                var spn = $(this).attr("data-validator-id");
                if ($(this).val() == "" || $(this).val() == "0" || $(this).val() == null) {
                    $("#" + spn).addClass("Error");
                    contError++;
                } else {
                    $("#" + spn).removeClass("Error");
                }
            }
        })

        var maximaDemandaContratada = $("#txtMaximaDemandaContratada").val();
        if (maximaDemandaContratada == "" || isNaN(maximaDemandaContratada) || maximaDemandaContratada == 0) {
            alert("Ingrese un valor válido en Máxima demanda contratada.");
            contError++;;
        }

        var numeroSuministrador = $("#txtNumeroSuministrador").val();
        if (numeroSuministrador == "" || isNaN(numeroSuministrador) || numeroSuministrador == 0) {
            alert("Ingrese un valor válido en número de Suministrador");
            contError++;;
        }
    }

    if (contError > 0) {
        validator = false;
    }
    return validator;
}

ValidarRequerido = function () {
    var validator = true;
    var contError = 0;
    // Validar datos
    $(".DatosRequeridos").each(function () {
        if ($(this).hasClass("Required")) {
            var spn = $(this).attr("data-validator-id");
            if ($(this).val() == "" || $(this).val() == "0" || $(this).val() == null) {
                $("#" + spn).addClass("Error");
                contError++;
            } else {
                $("#" + spn).removeClass("Error");
            }
        }
    });

    if (contError > 0) {
        validator = false;
    }
    return validator;
}

ValidarArchivosRL = function (index) {

    var cont = 0;
    if (index >= 0) {
        if ($("#flArchivoDigitalizadoTI" + index).val() == "") {
            $("#spnArchivoDigitalizadoTI").addClass("Error");
            cont++;
        }
    } else {
        if ($("#flArchivoDigitalizadoTI").val() == "") {
            $("#spnArchivoDigitalizadoTI").addClass("Error");
            cont++;
        }
    }

    if (cont > 0) {
        return false;
    }
    return true;

}

validar = function () {
    var mensaje = "<ul>", flag = true;

    //if ($('#txtNombresRL').val() == "") {
    //    mensaje = mensaje + "<li>Ingrese Nombres</li>";
    //    flag = false;
    //}

    //if ($('#txtApellidosRL').val() == "") {
    //    mensaje = mensaje + "<li>Ingrese Apellidos</li>";
    //    flag = false;
    //}

    //if ($('#txtCargoEmpresaRL').val() == "") {
    //    mensaje = mensaje + "<li>Ingrese Cargo</li>";
    //    flag = false;
    //}

    //if ($('#txtCorreoRL').val() == "") {
    //    mensaje = mensaje + "<li>Ingrese Correo</li>";
    //    flag = false;
    //} else {
    //    if (IsEmail($('#txtCorreoRL').val()) == false) {
    //        mensaje = mensaje + "<li>Ingrese un correo válido</li>";
    //        flag = false;
    //    }
    //}


    //if ($("#txtCorreoRL").val() != $("#txtCorreoRepetirRL").val()) {
    //    mensaje = mensaje + "<li>Los correos no son iguales.</li>";
    //    flag = false;
    //}


    //if ($('#txtTelefonoRL').val() == "") {
    //    mensaje = mensaje + "<li>Ingrese número de teléfono</li>";
    //    flag = false;
    //}
    //else {
    //    if (IsOnlyDigit($('#txtTelefonoRL').val()) == false) {
    //        mensaje = mensaje + "<li>Ingrese un número de teléfono válido</li>";
    //        flag = false;
    //    }
    //}

    //if ($('#txtMovilRL').val() == "") {
    //    mensaje = mensaje + "<li>Ingrese número de teléfono móvil</li>";
    //    flag = false;
    //}
    //else {
    //    if (IsOnlyDigit($('#txtMovilRL').val()) == false) {
    //        mensaje = mensaje + "<li>Ingrese un número de teléfono válido</li>";
    //        flag = false;
    //    }

    //}

    mensaje = mensaje + "</ul>";
    if (flag) mensaje = "";

    return mensaje;
}

function IsText(texto) {
    var regex = /^[A-Za-záéíóú ]+$/;
    return regex.test(texto);
}
function IsEmail(email) {
    var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return regex.test(email);
}
function IsOnlyDigit(texto) {
    for (var i = 0; i < texto.length; i++)
        if (!(IsDigito(texto[i]) || texto[i] == ' ' || texto[i] == '+' || texto[i] == '(' || texto[i] == ')' || texto[i] == '-'))
            return false;
    return true;
}
function IsDigito(caracter) {
    if (caracter == '0' || caracter == '1' || caracter == '2' || caracter == '3' || caracter == '4' || caracter == '5' || caracter == '6' || caracter == '7' || caracter == '8' || caracter == '9')
        return true;
    return false;
}

function mostrarError() {
    $('#mensaje').removeClass();
    $('#mensaje').html("Ha ocurrido un error, por favor intente nuevamente.");
    $('#mensaje').addClass('action-error');
}

function mostrarErrorAgregar() {
    $('#mensajeAgregar').removeClass();
    $('#mensajeAgregar').html("Ha ocurrido un error, por favor intente nuevamente.");
    $('#mensajeAgregar').addClass('action-error');
}

mostrarMensaje = function (mensaje) {
    alert(mensaje);
}

LimpiarRepresentanteLegalPopUp = function () {
    $(".Tipo").each(function () {
        $(this).val("");
        if ($(this).hasClass("Required")) {
            var spn = $(this).attr("data-validator-id");
            $("#" + spn).removeClass("Error");
        }
    });

    $(".GeneradorField").each(function () {
        $(this).val("");
        if ($(this).hasClass("Required")) {
            var spn = $(this).attr("data-validator-id");
            $("#" + spn).removeClass("Error");
        }
    });

    $(".TransmisorField").each(function () {
        $(this).val("");
    });

    $("#chk500").prop('checked',false);
    $("#chk200").prop('checked', false);
    $("#chk138").prop('checked', false);

    $(".DistribuidorField").each(function () {
        $(this).val("");
        if ($(this).hasClass("Required")) {
            var spn = $(this).attr("data-validator-id");
            $("#" + spn).removeClass("Error");
        }
    });

    $(".UsuarioLibreField").each(function () {
        $(this).val("");
        if ($(this).hasClass("Required")) {
            var spn = $(this).attr("data-validator-id");
            $("#" + spn).removeClass("Error");
        }
    });


    //Estableer los combos a los valores predeterminados
    $("#TipoEmpresa").val(0); $("#TipoEmpresa").change();
    $("#TipoDocumentoSustentario").val(0); $("#TipoDocumentoSustentario").change();
    //Limpiar archivo digitalizado
    $("#flArchivoDigitalizadoTI").val("");
    //remover error del archivo digitalizado
    $("#spnArchivoDigitalizadoTI").removeClass("Error");    
    
}

deshabilitaControles = function () {
    $('#divSolicitar').hide();
    $('#divUpload').hide();
    $('#btnAgregarRL').hide();


    var RLDNI = new Array();
    var tblRL = document.getElementById("tablaRepresentanteLegalbody");

    NRepresentanteLegal = tblRL.rows.length;
    var strRepresentateLegal = "";
    var Index = 0;
    for (var i = 0; i < NRepresentanteLegal; i++) {
        $(tblRL.rows[i].cells[7]).text(""); //boton dar de baja
        $(tblRL.rows[i].cells[8]).text(""); //boton editar
        $(tblRL.rows[i].cells[9]).text(""); //boton eliminar
        $(tblRL.rows[i].cells[24]).text(""); //Id   
    }
}

enabledFields = function () {

    $('#TipoEmpresa').prop("disabled", false);
    $('#TipoDocumentoSustentario').prop("disabled", false);
    $('#flArchivoDigitalizadoTI').prop("disabled", false);
    $('#txtPotenciaInstaladaTI').prop("disabled", false);
    $('#txtNumeroCentralesTI').prop("disabled", false);

    $('#chk500').prop("disabled", false);
    $('#chk200').prop("disabled", false);
    $('#chk138').prop("disabled", false);

    //$('#val500').prop("disabled", false);
    //$('#val200').prop("disabled", false);
    //$('#val138').prop("disabled", false);

    $('#txtTotalTransmision').prop("disabled", false);
    $('#txtMaximaDemandaCoincidenteAnual').prop("disabled", false);
    $('#txtMaximaDemandaContratada').prop("disabled", false);
    $('#txtNumeroSuministrador').prop("disabled", false);
  

    $('#TipoEmpresa').css("backgroundColor", "white");
    $('#TipoDocumentoSustentario').css("backgroundColor", "white");
    $("#txtPotenciaInstaladaTI").css("backgroundColor", "white");
    $("#txtNumeroCentralesTI").css("backgroundColor", "white");   
    
    $("#chk500").css("backgroundColor", "white");
    $("#chk200").css("backgroundColor", "white");
    $("#chk138").css("backgroundColor", "white");

    $("#val500").css("backgroundColor", "white");
    $("#val200").css("backgroundColor", "white");
    $("#val138").css("backgroundColor", "white");

    $("#txtTotalTransmision").css("backgroundColor", "white");
    $("#txtMaximaDemandaCoincidenteAnual").css("backgroundColor", "white");
    $("#txtMaximaDemandaContratada").css("backgroundColor", "white");
    $("#txtNumeroSuministrador").css("backgroundColor", "white");
}

disabledFields = function () {

    $('#TipoEmpresa').prop("disabled", true);
    $('#TipoDocumentoSustentario').prop("disabled", true);
    $('#flArchivoDigitalizadoTI').prop("disabled", true);
    $('#txtPotenciaInstaladaTI').prop("disabled", true);
    $('#txtNumeroCentralesTI').prop("disabled", true);

    $('#chk500').prop("disabled", true);
    $('#chk200').prop("disabled", true);
    $('#chk138').prop("disabled", true);

    $('#val500').prop("disabled", true);
    $('#val200').prop("disabled", true);
    $('#val138').prop("disabled", true);

    $('#txtTotalTransmision').prop("disabled", true);
    $('#txtMaximaDemandaCoincidenteAnual').prop("disabled", true);
    $('#txtMaximaDemandaContratada').prop("disabled", true);
    $('#txtNumeroSuministrador').prop("disabled", true);
}

OcultarTipoAgente = function () {
    $(".Generador").hide();
    $(".Transmisor").hide();
    $(".Distribuidor").hide();
    $(".UsuarioLibre").hide();
};

MostrarCamposPorTipoAgente = function (codigo) {
    OcultarTipoAgente();

    var GENERADOR = $("#hfTipoComportamientoGenerador_codigo").val();
    var TRANSMISOR = $("#hfTipoComportamientoTrasmisor_codigo").val();
    var DISTRIBUIDOR = $("#hfTipoComportamientoDistribuidor_codigo").val();
    var USUARIOLIBRE = $("#hfTipoComportamientoUsuarioLibre_codigo").val();



    switch (codigo) {
        case GENERADOR:
            $(".Generador").show(1000);
            if ($("#hfEditar").val() != "1") {
                var htmlDS = "<option value=''>Seleccione</option><option value='1'>AUTORIZACIÓN</option><option value='2'>CONCESIÓN</option>";
                $("#TipoDocumentoSustentario").empty();
                $("#TipoDocumentoSustentario").append(htmlDS);
            }

            break;
        case TRANSMISOR:
            $(".Transmisor").show(1000);
            if ($("#hfEditar").val() != "1") {
                var htmlDS = "<option value=''>Seleccione</option><option value='1'>AUTORIZACIÓN</option><option value='2'>CONCESIÓN</option>";
                $("#TipoDocumentoSustentario").empty();
                $("#TipoDocumentoSustentario").append(htmlDS);
            }

            break;
        case DISTRIBUIDOR:
            $(".Distribuidor").show(1000);
            if ($("#hfEditar").val() != "1") {
                var htmlDS = "<option value=''>Seleccione</option><option value='1'>AUTORIZACIÓN</option>";
                $("#TipoDocumentoSustentario").empty();
                $("#TipoDocumentoSustentario").append(htmlDS);
            }

            break;
        case USUARIOLIBRE:
            $(".UsuarioLibre").show(1000);
            if ($("#hfEditar").val() != "1") {
                var htmlDS = "<option value=''>Seleccione</option><option value='3'>DECLARACIÓN JURADA</option>";
                $("#TipoDocumentoSustentario").empty();
                $("#TipoDocumentoSustentario").append(htmlDS);
            }
            break;
    }
}

