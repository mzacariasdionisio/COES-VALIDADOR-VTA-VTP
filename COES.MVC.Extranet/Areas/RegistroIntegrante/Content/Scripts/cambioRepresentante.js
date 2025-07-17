var controlador = siteRoot + 'RegistroIntegrante/Solicitud/'
var RowRL = null;

var tipoRepresentanteTitular_codigo = "";
var tipoRepresentanteTitular_Descripcion = "";
var tipoRepresentanteAlternativo_codigo = "";
var tipoRepresentanteAlternativo_Descripcion = "";

$(function () {

    tipoRepresentanteTitular_codigo = $('#hfTipoRepresentanteTitular_codigo').val();
    tipoRepresentanteTitular_Descripcion = $('#hfTipoRepresentanteTitular_Descripcion').val();
    tipoRepresentanteAlternativo_codigo = $('#hfTipoRepresentanteAlternativo_codigo').val();
    tipoRepresentanteAlternativo_Descripcion = $('#hfTipoRepresentanteAlternativo_Descripcion').val();

    $("input[type=text]").keyup(function () {

        if (!(this.id == "txtCorreoRL" || this.id == "txtCorreoRepetirRL"))
            $("#" + this.id).val($("#" + this.id).val().toUpperCase());
    });

    cargarRL();

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

verDocumentoIdentidad = function () {
    window.open(controlador + 'ver?url=' + $('#hdfNombreArchivoDocumentoIdentidad').val(), "_blank", 'fullscreen=yes');
}

descargarDocumentoIdentidad = function () {
    document.location.href = controlador + 'Download?url=' + $('#hdfNombreArchivoDocumentoIdentidad').val() + '&nombre=' + $('#hdfAdjuntoDocumentoIdentidad').val();
}

verVigenciaPoder = function () {
    window.open(controlador + 'ver?url=' + $('#hdfNombreArchivoVigenciaPoder').val(), "_blank", 'fullscreen=yes');
}

descargarVigenciaPoder = function () {
    document.location.href = controlador + 'Download?url=' + $('#hdfNombreArchivoVigenciaPoder').val() + '&nombre=' + $('#hdfAdjuntoVigenciaPoder').val();
}

cargarRL = function () {

    $.ajax({
        type: 'POST',        
        url: controlador + 'ListadoRepresentante',
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
    if (Row.cells[1].innerText == "Titular") {
        alert("No se puede dar de baja al representate Titular");
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

    var tipo = Row.cells[1].innerText;
    if (tipo == tipoRepresentanteTitular_Descripcion)
        $("#tiporepresentante").val(tipoRepresentanteTitular_codigo);
    else
        $("#tiporepresentante").val(tipoRepresentanteAlternativo_codigo);

    $("#txtDNIRL").val(Row.cells[3].innerText);
    $("#txtNombresRL").val(Row.cells[4].innerText);
    $("#txtApellidosRL").val(Row.cells[5].innerText);
    $("#txtCargoEmpresaRL").val(Row.cells[6].innerText);
    $("#txtTelefonoRL").val(Row.cells[7].innerText);
    $("#txtMovilRL").val(Row.cells[8].innerText);
    $("#txtCorreoRL").val(Row.cells[9].innerText);
    $("#txtCorreoRepetirRL").val(Row.cells[9].innerText);
   
    var Index = Row.cells[13].innerText;


    $(".DNIRL").each(function () {
        $(this).hide();
    });

    $(".VigenciaPoderRL").each(function () {
        $(this).hide();
    });

    var estado = $("#txtESTADO").val();
    if (estado == "") {
        $("#flDNIRL" + Index).show();
        $("#flVigenciaPoderRL" + Index).show();


    } else //Si no es nuevo es solo lectura
    {
        disabledFields();

        //Enlaces

        $('#hdfNombreArchivoDocumentoIdentidad').val(Row.cells[14].innerText);
        $('#verDocumentoIdentidad').show();
        $('#hdfAdjuntoDocumentoIdentidad').val(Row.cells[15].innerText);
        $('#descargarDocumentoIdentidad').show();

        $('#hdfNombreArchivoVigenciaPoder').val(Row.cells[16].innerText);
        $('#verVigenciaPoder').show();
        $('#hdfAdjuntoVigenciaPoder').val(Row.cells[17].innerText);
        $('#descargarVigenciaPoder').show();
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


AgregarRL = function () {


    RowRL = null;

    $(".DNIRL").each(function () {
        $(this).hide();
    });
    $(".VigenciaPoderRL").each(function () {
        $(this).hide();
    });

    $("#flDNIRL").show();
    $("#flVigenciaPoderRL").show();

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

                var tipo = $("#tiporepresentante option:selected").text();
                if (ValidarTitularRpteLegal(tipo) == true) {
                    $("#spnTIPORL").addClass("Error");
                    $("#mensajeAgregar").show();
                    $("#mensajeAgregar").text("Solo se puede registrar un solo Representantes Legal TITULAR.");
                    return false;
                }

                $("#mensajeAgregar").hide();

                var contador = $("#hfRepresentanteLegalContador").val(); 
                contador++;

                var btnEliminar = "";
                btnEliminar = "<td><a href='#' onclick='EliminarRL(this)'><img src='/Content/Images/btn-cancel.png' style='margin-top: 5px;' /></a></td>";

                var Representante = "";
                Representante = $("#tiporepresentante option:selected").text();

                var html = "";
                html += "<tr class='tblRLDetalle'>";
                html += "<td style='display:none;'></td>";
                html += "<td>" + Representante + "</td>";
                html += "<td>AGREGADO</td>";
                html += "<td>" + $("#txtDNIRL").val() + "</td>";
                html += "<td>" + $("#txtNombresRL").val() + "</td>";
                html += "<td>" + $("#txtApellidosRL").val() + "</td>";
                html += "<td>" + $("#txtCargoEmpresaRL").val() + "</td>";
                html += "<td>" + $("#txtTelefonoRL").val() + "</td>";
                html += "<td>" + $("#txtMovilRL").val() + "</td>"
                html += "<td>" + $("#txtCorreoRL").val() + "</td>";
                html += "<td></td>";
                html += "<td><a href='#' onclick='EditarRepresentanteLegal(this)'><img src='/Content/Images/btn-edit.png' style='margin-top: 5px;' /></a></td>";                
                html += btnEliminar;                
                html += "<td style='display:none;'>" + contador + "</td>";
                html += "</tr>";

                var FileInputDNI = document.getElementById("flDNIRL").cloneNode(true);
                FileInputDNI.setAttribute("id", "flDNIRL" + contador);
                FileInputDNI.setAttribute("class", "DNIRL");
                $(FileInputDNI).hide();
                document.getElementById("divDocumentoIdentidad").appendChild(FileInputDNI);

                var FileInputVigenciaPoder = document.getElementById("flVigenciaPoderRL").cloneNode(true);
                FileInputVigenciaPoder.setAttribute("id", "flVigenciaPoderRL" + contador);
                FileInputVigenciaPoder.setAttribute("class", "VigenciaPoderRL");
                $(FileInputVigenciaPoder).hide();
                document.getElementById("divVigenciaPoder").appendChild(FileInputVigenciaPoder);

                $("#tablaRL tbody").append(html);
                $("#hfRepresentanteLegalContador").val(contador);

            } else {
                var Index = RowRL.rowIndex;
                if (!ValidarArchivosRL(Index)) {
                    return false;
                }
                RowRL.cells[1].innerText = $("#tiporepresentante option:selected").text();
                RowRL.cells[3].innerText = $("#txtDNIRL").val();
                RowRL.cells[4].innerText = $("#txtNombresRL").val();
                RowRL.cells[5].innerText = $("#txtApellidosRL").val();
                RowRL.cells[6].innerText = $("#txtCargoEmpresaRL").val();
                RowRL.cells[7].innerText = $("#txtTelefonoRL").val();
                RowRL.cells[8].innerText = $("#txtMovilRL").val();
                RowRL.cells[9].innerText = $("#txtCorreoRL").val();              
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


ValidarTitularRpteLegal = function (tipo) {

    var NRepresentanteLegal = 0;

    var tblRL = document.getElementById("tablaRepresentanteLegalbody");
    NRepresentanteLegal = tblRL.rows.length;
    var strRepresentateLegal = "";
    var strAccion = "";
    var Index = 0;
    for (var i = 0; i < NRepresentanteLegal; i++) {
        strtipoRepresentateLegal = $(tblRL.rows[i].cells[1]).text();
        strAccion = $(tblRL.rows[i].cells[2]).text();
        if (strAccion != 'BAJA') {
            if (tipoRepresentanteTitular_Descripcion == strtipoRepresentateLegal && tipoRepresentanteTitular_Descripcion == tipo)
                return true;
        }
    }
    return false;
}

ValidarRequeridoAgregar = function () {
    var validator = true;
    var contError = 0;
    // Validar datos
    $(".DatosRequeridosAgregar").each(function () {
        if ($(this).hasClass("RequiredAgregar")) {


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
        if ($("#flDNIRL" + index).val() == "") {
            $("#spnflDNIRL").addClass("Error");
            cont++;
        }
        if ($("#flVigenciaPoderRL" + index).val() == "") {
            $("#spnVigenciaPoderRL").addClass("Error");
            cont++;
        }
    } else {
        if ($("#flDNIRL").val() == "") {
            $("#spnflDNIRL").addClass("Error");
            cont++;
        }
        if ($("#flVigenciaPoderRL").val() == "") {
            $("#spnVigenciaPoderRL").addClass("Error");
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

    if ($('#txtNombresRL').val() == "") {
        mensaje = mensaje + "<li>Ingrese Nombres</li>";
        flag = false;
    }

    if ($('#txtApellidosRL').val() == "") {
        mensaje = mensaje + "<li>Ingrese Apellidos</li>";
        flag = false;
    }

    if ($('#txtCargoEmpresaRL').val() == "") {
        mensaje = mensaje + "<li>Ingrese Cargo</li>";
        flag = false;
    }

    if ($('#txtCorreoRL').val() == "") {
        mensaje = mensaje + "<li>Ingrese Correo</li>";
        flag = false;
    } else {
        if (IsEmail($('#txtCorreoRL').val()) == false) {
            mensaje = mensaje + "<li>Ingrese un correo válido</li>";
            flag = false;
        }
    }


    if ($("#txtCorreoRL").val() != $("#txtCorreoRepetirRL").val()) {
        mensaje = mensaje + "<li>Los correos no son iguales.</li>";
        flag = false;
    }


    if ($('#txtTelefonoRL').val() == "") {
        mensaje = mensaje + "<li>Ingrese número de teléfono</li>";
        flag = false;
    }
    else {
        if (IsOnlyDigit($('#txtTelefonoRL').val()) == false) {
            mensaje = mensaje + "<li>Ingrese un número de teléfono válido</li>";
            flag = false;
        }
    }

    if ($('#txtMovilRL').val() == "") {
        mensaje = mensaje + "<li>Ingrese número de teléfono móvil</li>";
        flag = false;
    }
    else {
        if (IsOnlyDigit($('#txtMovilRL').val()) == false) {
            mensaje = mensaje + "<li>Ingrese un número de teléfono válido</li>";
            flag = false;
        }

    }

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
    $(".DatosRequeridosAgregar").each(function () {
        $(this).val("");
        if ($(this).hasClass("RequiredAgregar")) {
            var spn = $(this).attr("data-validator-id");
            $("#" + spn).removeClass("Error");
        }

    });
    $("#spnflDNIRL").removeClass("Error");
    $("#spnVigenciaPoderRL").removeClass("Error");
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
        $(tblRL.rows[i].cells[10]).text("");
        $(tblRL.rows[i].cells[11]).text("");
        $(tblRL.rows[i].cells[12]).text("");
        $(tblRL.rows[i].cells[13]).text("");      
    }
}

enabledFields = function () {

    $('#tiporepresentante').prop("disabled", false);
    $('#cbTipoDocumento').prop("disabled", false);
    $('#txtDNIRL').prop("disabled", false);    
    $('#flDNIRL').prop("disabled", false);
    $('#txtNombresRL').prop("disabled", false);
    $('#txtApellidosRL').prop("disabled", false);
    
    $('#flVigenciaPoderRL').prop("disabled", false);
    $('#txtCargoEmpresaRL').prop("disabled", false);
    $('#txtTelefonoRL').prop("disabled", false);
    $('#txtMovilRL').prop("disabled", false);
    $('#txtCorreoRL').prop("disabled", false);
    $('#txtCorreoRepetirRL').prop("disabled", false);
       
    $('#cbTipoDocumento').css("backgroundColor", "white");
    $('#txtDNIRL').css("backgroundColor", "white");
    $("#txtNombresRL").css("backgroundColor", "white");
    $("#txtApellidosRL").css("backgroundColor", "white");    
    $("#txtCargoEmpresaRL").css("backgroundColor", "white");
    $("#txtTelefonoRL").css("backgroundColor", "white");
    $("#txtMovilRL").css("backgroundColor", "white");
    $("#txtCorreoRL").css("backgroundColor", "white");
    $("#txtCorreoRepetirRL").css("backgroundColor", "white");
}

disabledFields = function () {
    $('#tiporepresentante').prop("disabled", true);
    $('#cbTipoDocumento').prop("disabled", true);
    $('#txtDNIRL').prop("disabled", true);
    $('#flDNIRL').prop("disabled", true);
    $('#txtNombresRL').prop("disabled", true);
    $('#txtApellidosRL').prop("disabled", true);
    $('#flVigenciaPoderRL').prop("disabled", true);
    $('#txtCargoEmpresaRL').prop("disabled", true);
    $('#txtTelefonoRL').prop("disabled", true);
    $('#txtMovilRL').prop("disabled", true);
    $('#txtCorreoRL').prop("disabled", true);
    $('#txtCorreoRepetirRL').prop("disabled", true);
}
