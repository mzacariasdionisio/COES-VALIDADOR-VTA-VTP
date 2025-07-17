var controlador = siteRoot + 'RegistroIntegrante/Integrante/';
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

        if (!(this.id == "txtCorreoElectronicoRT" || this.id == "txtCorreoElectronicoRepetirRT" || this.id == "txtCorreoRL" || this.id == "txtCorreoRepetirRL" || this.id == "txtCorreoElectronicoPC" || this.id == "txtCorreoElectronicoRepetirPC"))
            $("#" + this.id).val($("#" + this.id).val().toUpperCase());
    });

    $("#val500").prop('disabled', true);
    $("#val200").prop('disabled', true);
    $("#val138").prop('disabled', true);
    $("#valMenor138").prop('disabled', true);

    $('#chk500').change(function () {
        if (this.checked) {
            $("#val500").prop('disabled', false);
        } else {
            $('#val500').val("");
            $("#val500").prop('disabled', true);
            SumarLineasTransmision();
        }
    });

    $('#chk200').change(function () {
        if (this.checked) {
            $("#val200").prop('disabled', false);
        } else {
            $('#val200').val("");
            $("#val200").prop('disabled', true);
            SumarLineasTransmision();
        }
    });

    $('#chk138').change(function () {
        if (this.checked) {
            $("#val138").prop('disabled', false);
        } else {
            $('#val138').val("");
            $("#val138").prop('disabled', true);
            SumarLineasTransmision();
        }
    });

    $('#chkMenor138').change(function () {
        if (this.checked) {
            $("#valMenor138").prop('disabled', false);
        } else {
            $("#valMenor138").val("");
            $("#valMenor138").prop('disabled', true);
            SumarLineasTransmision();
        }
    });

    $('#lnkayuda').click(function (event) {
        event.preventDefault();
        Ayuda();
    });

    Ayuda = function () {
        $('#divAyuda').bPopup({
            follow: [true, true],
            position: ['auto', 'auto'],
            positionStyle: 'fixed',
        });

    }

    $('#txtNumeroRucDE').change(function () {
        obtenerDatos();
    });

    //$('#tab-container').easytabs();

    OcultarTipoAgente();

    $('#btnAgregarRepresentanteLegal').click(function () {

        $("#hfRepresentanteLegalNuevo").val("1");

        var contador = $("#hfRepresentanteLegalContador").val();
        contador++;
        $("#hfRepresentanteLegalContador").val(contador);

        var FileInputDNI = document.getElementById("flDNIRL").cloneNode();
        FileInputDNI.setAttribute("id", "flDNIRL" + contador);
        FileInputDNI.setAttribute("class", "DNIRL");

        $(FileInputDNI).hide();
        document.getElementById("divDocumentoIdentidad").appendChild(FileInputDNI);

        var FileInputVigenciaPoder = document.getElementById("flVigenciaPoderRL").cloneNode(true);
        FileInputVigenciaPoder.setAttribute("id", "flVigenciaPoderRL" + contador);
        FileInputVigenciaPoder.setAttribute("class", "VigenciaPoderRL");

        $(FileInputVigenciaPoder).hide();
        document.getElementById("divVigenciaPoder").appendChild(FileInputVigenciaPoder);

        RowRL = null;

        $("MensajeRL").hide();

        $(".DNIRL").each(function () {
            $(this).hide();
        });
        $(".VigenciaPoderRL").each(function () {
            $(this).hide();
        });

        $("#flDNIRL" + contador).show();
        $("#flVigenciaPoderRL" + contador).show();

        $('#divAgregarRepresentanteLegal').bPopup({
            modalClose: false,
            follow: [true, true],
            position: ['auto', 'auto'],
            positionStyle: 'fixed',
            onClose: function () {
                LimpiarRepresentanteLegalPopUp();
            }
        });
    });

    $("#btnCancelarRL").click(function () {
        var nuevo = $("#hfRepresentanteLegalNuevo").val();
        if (nuevo == "1") {
            var contador = $("#hfRepresentanteLegalContador").val();

            $("#flDNIRL" + contador).remove();

            contador--;
            $("#hfRepresentanteLegalContador").val(contador);
        }
        var bPopup = $('#divAgregarRepresentanteLegal').bPopup();
        bPopup.close();
        LimpiarRepresentanteLegalPopUp();
        RowRL = null;
    });
    //$("#btnSiguiente").click(function () {
    //    $('#tab-container').easytabs('select', 'tabs3-css');
    //})
    $("#TipoEmpresa").change(function () {
        MostrarCamposPorTipoAgente(this.value);
        MostrarDescargarModeloCarta();
    })
    $("#TipoDocumentoSustentario").change(function () {
        MostrarDescargarModeloCarta();
    })

    $("#btnGrabarRL").click(function () {
        AgregarRepresentanteLegal();
    });
    $("#btnEnviar").click(function () {
        var Emprcodi = $("#hfEmprcodi").val();
        if (Emprcodi == 0) {
            EnviarDatos();
        } else {
            Reenviar();
        }
    });


    $("#chkTitulosAdicionales").on("change", function () {
        if ($(this).prop("checked")) {
            $("#btnAddTA").show();

        } else {
            $("#btnAddTA").hide();
            $("#lblObservacionTA").hide();
            $("#txtobstitulosadicionales").val("");
            $("#txtobstitulosadicionales").hide();

            $("#tblTitulosAdicionales tbody").empty();
        }
    });
    $("#btnAddTA").click(function () {
        AgregarTitulosAdicionales();
    });
    $("#btnAddTA").hide();
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

    var IdTipoEmpresa = $("#TipoEmpresa").val();
    if (IdTipoEmpresa != "" || IdTipoEmpresa != "0") {
        MostrarCamposPorTipoAgente(IdTipoEmpresa);
    }


    $("#chkterminosycondiciones").on("change", function () {
        if ($(this).prop("checked")) {
            $("#btnSiguiente").show();
            $("#btnEnviar").show();
            $("#btnTab1ToTab2").show();
        } else {
            $("#btnSiguiente").hide();
            $("#btnEnviar").hide();
            $("#btnTab1ToTab2").hide();
        }
    });

    $('#btnBuscarRuc').click(function () {
        window.open('http://www.sunat.gob.pe/cl-ti-itmrconsruc/jcrS03Alias',
            "_blank", "toolbar=no,scrollbars=no,resizable=yes,status=no,top=400,left=500,width=700,height=400");
    });
});


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

ValidarSize = function (obj) {
    if (getBrowserInfo().lastIndexOf("IE") == 0)
        return;
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

obtenerDatos = function () {

    var ruc = $('#txtNumeroRucDE').val();
    var isNumber = /^\d+$/.test(ruc);
    $('#txtNombreComercialDE').val("");
    $('#txtDenominacionRazonSocialDE').val("");
    $('#txtDomicilioLegalDE').val("");

    if (ruc.length == 11 && isNumber) {
        $('#loadEmpresa').show();
        $.ajax({
            type: 'POST',
            url: controlador + 'obtenerdatos',
            data: {
                ruc: ruc
            },
            dataType: 'json',
            global: false,
            success: function (result) {
                if (result == -2) {
                    mostrarAlerta('El RUC ingresado no existe.', 'mensajeEMP');
                }
                else if (result == -1) {
                    mostrarError('mensajeEMP');
                }
                if (result == -3) {
                    mostrarAlerta('El RUC ingresado se encuentra de BAJA.', 'mensajeEMP');
                }
                else {
                    $('#txtNombreComercialDE').val(result.NombreComercial);
                    $('#txtDenominacionRazonSocialDE').val(result.RazonSocial);
                    $('#txtDomicilioLegalDE').val(result.DomicilioLegal);
                    $("#mensajeEMP").hide();
                }
            },
            error: function () {
                mostrarAlerta('Error al consultar a SUNAT', 'mensajeEMP');
            }
        });
        $('#loadEmpresa').hide();
    }
    else {
        mostrarAlerta('El RUC debe contener 11 dígitos.', 'mensajeEMP');
    }
}

mostrarAlerta = function (mensaje, id) {
    //$('#' + id).removeClass();
    $('#' + id).show();
    //$('#' + id).addClass('action-alert');
    $('#' + id).html(mensaje);
}

mostrarError = function (id) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-error');
    $('#' + id).text('Ha ocurrido un error.');
}

Tabs = function (tab) {

    if (tab == "tabs1-css") {
        SelecionarTab("tabs1-css");
    }

    if (tab == "tabs2-css") {
        SelecionarTab("tabs2-css");
    }

    if (tab == "tabs3-css") {

        if (ResponsableTramite() > 0) {
            $("#mensajeRT").show();
            return;
        }

        if (IsOnlyDigit($('#txtTelefonoRT').val()) == false) {
            $("#spnTelefonoRT").addClass("Error");
            $("#mensajeRT").show();
            $("#mensajeRT").text("Ingrese un teléfono válido.");
            return;
        }
        if (IsEmail($('#txtCorreoElectronicoRT').val()) == false) {
            $("#spnCorreoElectronicoRT").addClass("Error");
            $("#mensajeRT").show();
            $("#mensajeRT").text("Ingrese un correo válido.");
            return;
        }
        if (IsEmail($('#txtCorreoElectronicoRepetirRT').val()) == false) {
            $("#spnCorreoElectronicoRepetirRT").addClass("Error");
            $("#mensajeRT").show();
            $("#mensajeRT").text("Ingrese un correo válido.");
            return;
        }
        if ($("#txtCorreoElectronicoRT").val() != $("#txtCorreoElectronicoRepetirRT").val()) {
            $("#spnCorreoElectronicoRT").addClass("Error");
            $("#spnCorreoElectronicoRepetirRT").addClass("Error");

            $("#mensajeRT").show();
            $("#mensajeRT").text("Los correos no son iguales.");
            return;
        }
        $("#mensajeRT").hide();

        SelecionarTab("tabs3-css");
    }


    if (tab == "tabs4-css") {

        if (DatosEmpresa() > 0) {
            return;
        }

        if (IsOnlyNumber($('#txtNumeroRucDE').val()) == false) {
            $("#spntxtNumeroRucDE").addClass("Error");
            $("#mensajeEMP").show();
            $("#mensajeEMP").text("Ingrese un RUC válido.");
            return;
        }
        if (IsOnlyDigit($('#txtTelefonoDE').val()) == false) {
            $("#spnTelefonoDE").addClass("Error");
            $("#mensajeEMP").show();
            $("#mensajeEMP").text("Ingrese un teléfono válido.");
            return;
        }
        $("#mensajeEMP").hide();

        SelecionarTab("tabs4-css");
    }

    if (tab == "tabs5-css") {

        if (ValidarTipoAgente() > 0) {
            return;
        }
        $("#mensajeTipo").hide();

        SelecionarTab("tabs5-css");
    }

    if (tab == "tabs6-css") {

        var LenRL = $(".tblRLDetalle").length;
        if (LenRL == 0) {
            $("#mensajeRPTE").show();
            $("#mensajeRPTE").text("Agrege al menos un representante legal");
            return;
        }
        if (RepresentanteLegal() > 0) {
            return;
        }
        $("#mensajeRPTE").hide();

        SelecionarTab("tabs6-css");

    }
    //$('#tab-container').easytabs('select', tab);
};

switchToTab = function (tab) {

    if (tab == "tabs1-css") {
        SelecionarTab("tabs1-css");
    }

    if (tab == "tabs2-css") {
        SelecionarTab("tabs2-css");
    }

    if (tab == "tabs3-css") {
        SelecionarTab("tabs3-css");
    }

    if (tab == "tabs4-css") {
        SelecionarTab("tabs4-css");
    }

    if (tab == "tabs5-css") {
        SelecionarTab("tabs5-css");
    }

    if (tab == "tabs6-css") {
        SelecionarTab("tabs6-css");
    }
};


openCondiciones = function () {
    $('#popupDisclaimer').bPopup({
    });
}

ValidarContactoDiferenteRpteLegal = function (nombreContacto, apellidosContacto) {

    var NRepresentanteLegal = 0;

    var tblRL = document.getElementById("tablaRepresentanteLegalbody");
    NRepresentanteLegal = tblRL.rows.length;
    var strRepresentateLegal = "";
    var Index = 0;
    for (var i = 0; i < NRepresentanteLegal; i++) {
        strNombreRepresentateLegal = $(tblRL.rows[i].cells[2]).text();
        strApellidosRepresentateLegal = $(tblRL.rows[i].cells[3]).text();

        if (strNombreRepresentateLegal == nombreContacto && strApellidosRepresentateLegal == apellidosContacto)
            return false;
    }

}

ValidarTitularRpteLegal = function (tipo) {

    var NRepresentanteLegal = 0;
    var Titulares = 0;
    var tblRL = document.getElementById("tablaRepresentanteLegalbody");
    NRepresentanteLegal = tblRL.rows.length;
    var strRepresentateLegal = "";

    for (var i = 0; i < NRepresentanteLegal; i++) {
        strtipoRepresentateLegal = $(tblRL.rows[i].cells[0]).text();
        if (tipoRepresentanteTitular_Descripcion == strtipoRepresentateLegal)
            Titulares += 1;
    }

    if (tipoRepresentanteTitular_Descripcion == tipo) {
        Titulares += 1;
    }

    var limite = 0;
    var nuevo = $("#hfRepresentanteLegalNuevo").val();
    if (nuevo == "1")
        limite = 1;
    else
        limite = 2;

    if (Titulares > limite)
        return true;
    else
        return false;
}

OcultarTipoAgente = function () {
    $("#Generador1").hide();
    $("#Generador2").hide();
    $("#Transmisor1").hide();
    $("#Transmisor2").hide();
    $("#Distribuidor1").hide();
    $("#UsuarioLibre1").hide();
    $("#UsuarioLibre2").hide();
};

MostrarDescargarModeloCarta = function (codigo) {

    var USUARIOLIBRE = $("#hfTipoComportamientoUsuarioLibre_codigo").val();
    var tipoIntegrante = $("#TipoEmpresa").val();
    var TipoDoc = $("#TipoDocumentoSustentario").val();

    if (tipoIntegrante == USUARIOLIBRE && TipoDoc == "3") {
        $("#lnModeloCarta").show();
    } else {
        $("#lnModeloCarta").hide();
    }

}

MostrarCamposPorTipoAgente = function (codigo) {
    OcultarTipoAgente();


    var GENERADOR = $("#hfTipoComportamientoGenerador_codigo").val();
    var TRANSMISOR = $("#hfTipoComportamientoTrasmisor_codigo").val();
    var DISTRIBUIDOR = $("#hfTipoComportamientoDistribuidor_codigo").val();
    var USUARIOLIBRE = $("#hfTipoComportamientoUsuarioLibre_codigo").val();

    switch (codigo) {
        case GENERADOR:
            $("#Generador1").show(200);
            $("#Generador2").show(200);

            if ($("#hfEditar").val() != "1") {
                var htmlDS = "<option value=''>Seleccione</option><option value='1'>AUTORIZACIÓN</option><option value='2'>CONCESIÓN</option>";
                $("#TipoDocumentoSustentario").empty();
                $("#TipoDocumentoSustentario").append(htmlDS);
            }

            break;
        case TRANSMISOR:
            $("#Transmisor1").show(200);
            $("#Transmisor2").show(200);
            if ($("#hfEditar").val() != "1") {
                var htmlDS = "<option value=''>Seleccione</option><option value='1'>AUTORIZACIÓN</option><option value='2'>CONCESIÓN</option>";
                $("#TipoDocumentoSustentario").empty();
                $("#TipoDocumentoSustentario").append(htmlDS);
            }

            break;
        case DISTRIBUIDOR:
            $("#Distribuidor1").show(200);
            if ($("#hfEditar").val() != "1") {
                var htmlDS = "<option value=''>Seleccione</option><option value='1'>AUTORIZACIÓN</option>";
                $("#TipoDocumentoSustentario").empty();
                $("#TipoDocumentoSustentario").append(htmlDS);
            }

            break;
        case USUARIOLIBRE:
            $("#UsuarioLibre1").show(200);
            $("#UsuarioLibre2").show(200);
            if ($("#hfEditar").val() != "1") {
                var htmlDS = "<option value=''>Seleccione</option><option value='3'>DECLARACIÓN JURADA</option>";
                $("#TipoDocumentoSustentario").empty();
                $("#TipoDocumentoSustentario").append(htmlDS);
            }
            break;
    }
}

AgregarRepresentanteLegal = function () {

    if (!ValidarRL()) {
        return false;
    }
    var contador = $("#hfRepresentanteLegalContador").val();

    if (RowRL == null) {

        if (!ValidarArchivosRL(contador)) {
            return false;
        }
        $("#mensajeRL").hide();

        var tipo = $("#tiporepresentante option:selected").text();
        if (ValidarTitularRpteLegal(tipo) == true) {
            $("#spnTIPORL").addClass("Error");
            $("#mensajeRL").show();
            $("#mensajeRL").text("Solo se puede registrar un solo Representantes Legal TITULAR.");
            return false;
        }

        $("#mensajeRL").hide();
        var btnEliminar = "";

        Representante = $("#tiporepresentante option:selected").text();
        btnEliminar = "<td><input type='button' class='coes-form-item--submit mb-3' onclick='EliminarRL(this);' data-flag='" + Representante + "' value='Eliminar'/></td>";


        var html = "";
        html += "<tr class='tblRLDetalle'>";

        html += "<td>" + Representante + "</td>";
        html += "<td>" + $("#txtDNIRL").val() + "</td>";
        html += "<td>" + $("#txtNombresRL").val() + "</td>";
        html += "<td>" + $("#txtApellidosRL").val() + "</td>";
        html += "<td>" + $("#txtCargoEmpresaRL").val() + "</td>";
        html += "<td>" + $("#txtTelefonoRL").val() + "</td>";
        html += "<td>" + $("#txtCorreoRL").val() + "</td>";
        html += "<td><input type='button' class='coes-form-item--submit mb-3' onclick='EditarRepresentanteLegal(this);' value='Editar'/></td>";
        html += btnEliminar;
        html += "<td style='display:none;'>" + $("#txtMovilRL").val() + "</td>"
        html += "<td style='display:none;'>" + contador + "</td>";
        html += "</tr>";

        $("#tablaRepresentanteLegal tbody").append(html);
        $("#hfRepresentanteLegalContador").val(contador);

    } else {
        var Index = RowRL.rowIndex;
        var ID = RowRL.cells[10].innerText;
        if (!ValidarArchivosRL(ID)) {
            return false;
        }

        var tipo = $("#tiporepresentante option:selected").text();
        if (ValidarTitularRpteLegal(tipo) == true) {
            $("#spnTIPORL").addClass("Error");
            $("#mensajeRL").show();
            $("#mensajeRL").text("Solo se puede registrar un solo Representantes Legal TITULAR.");
            return false;
        }

        $("#mensajeRL").hide();

        RowRL.cells[0].innerText = $("#tiporepresentante option:selected").text();
        RowRL.cells[1].innerText = $("#txtDNIRL").val();
        RowRL.cells[2].innerText = $("#txtNombresRL").val();
        RowRL.cells[3].innerText = $("#txtApellidosRL").val();
        RowRL.cells[4].innerText = $("#txtCargoEmpresaRL").val();
        RowRL.cells[5].innerText = $("#txtTelefonoRL").val();
        RowRL.cells[6].innerText = $("#txtCorreoRL").val();
        RowRL.cells[8].innerHTML = "<input type='button' class='coes-form-item--submit mb-3' onclick='EliminarRL(this);' data-flag='" + $("#tiporepresentante option:selected").text() + "' value='Eliminar'/>";
        RowRL.cells[9].innerText = $("#txtMovilRL").val();
    }

    RowRL = null;
    var bPopup = $('#divAgregarRepresentanteLegal').bPopup();
    bPopup.close();

    LimpiarRepresentanteLegalPopUp();
};

EditarRepresentanteLegal = function (obj) {

    $("#hfRepresentanteLegalNuevo").val("0");

    $("#mensajeRL").hide();

    var Row = $(obj).closest("tr")[0];
    RowRL = Row;

    //var Index = Row.rowIndex;
    var tipo = Row.cells[0].innerText;
    if (tipo == tipoRepresentanteTitular_Descripcion)
        $("#tiporepresentante").val(tipoRepresentanteTitular_codigo);
    else
        $("#tiporepresentante").val(tipoRepresentanteAlternativo_codigo);


    $("#txtDNIRL").val(Row.cells[1].innerText);
    $("#txtNombresRL").val(Row.cells[2].innerText);
    $("#txtApellidosRL").val(Row.cells[3].innerText);
    $("#txtCargoEmpresaRL").val(Row.cells[4].innerText);
    $("#txtTelefonoRL").val(Row.cells[5].innerText);
    $("#txtCorreoRL").val(Row.cells[6].innerText);
    $("#txtCorreoRepetirRL").val(Row.cells[6].innerText);
    $("#txtMovilRL").val(Row.cells[9].innerText);

    var Index = Row.cells[10].innerText;
    Index = Index.trim();
    $(".DNIRL").each(function () {
        $(this).hide();
    });
    $(".VigenciaPoderRL").each(function () {
        $(this).hide();
    });
    $(".FileRL").each(function () {
        $(this).hide();
    });


    $("#flDNIRL" + Index).show();
    $("#flVigenciaPoderRL" + Index).show();

    $("#aVP" + Index).show();
    $("#aDNI" + Index).show();

    $('#divAgregarRepresentanteLegal').bPopup({
        modalClose: false,
        follow: [true, true],
        position: ['auto', 'auto'],
        positionStyle: 'fixed',
        onClose: function () {
            LimpiarRepresentanteLegalPopUp();
        }
    });
};

EliminarRL = function (obj) {
    var flag = $(obj).attr("data-flag");
    if (flag == tipoRepresentanteTitular_Descripcion) {
        alert("No se puede eliminar el representate tipo Principal");
        return;
    }
    $(obj).closest('tr').remove();
}

LimpiarRepresentanteLegalPopUp = function () {
    $(".RepresentanteLegal").each(function () {
        $(this).val("");
        if ($(this).hasClass("Required")) {
            var spn = $(this).attr("data-validator-id");
            $("#" + spn).removeClass("Error");
        }

    });
    $("#spnflDNIRL").removeClass("Error");
    $("#spnVigenciaPoderRL").removeClass("Error");
}

ResponsableTramite = function () {
    var contError = 0;

    $(".ResponsableTramite").each(function () {
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

    return contError;
}

DatosEmpresa = function () {
    var contError = 0;

    $(".DatosEmpresa").each(function () {
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

    return contError;
}

ValidarTipoAgente = function () {
    var contError = 0;

    var TipoAgente = "";
    $(".Tipo").each(function () {
        if ($(this).hasClass("Required")) {
            var spn = $(this).attr("data-validator-id");
            if ($(this).val() === "" || $(this).val() === "0" || $(this).val() === null) {
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
            $("#mensajeTipo").show();
            $("#mensajeTipo").text("Ingrese un valor válido en potencia instalada.");
            contError++;;
        }

        var numeroCentralesTI = $("#txtNumeroCentralesTI").val();
        if (numeroCentralesTI == "" || isNaN(numeroCentralesTI) || numeroCentralesTI == 0) {
            $("#mensajeTipo").show();
            $("#mensajeTipo").text("Ingrese un valor válido en número de centrales.");
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

        var chkMenor138 = $("chkMenor138").prop('checked');
        var valMenor138 = $("valMenor138").val();

        var mensaje = "";
        var suma = v0;
        suma += (val500 && !isNaN(val500)) ? parseInt(val500) : 0;
        suma += (val200 && !isNaN(val200)) ? parseInt(val200) : 0; 
        suma += (val138 && !isNaN(val138)) ? parseInt(val138) : 0;
        suma += (valMenor138 && !isNaN(valMenor138)) ? parseInt(valMenor138) : 0;

        if (suma === 0) {
            $("#mensajeTipo").show();
            $("#mensajeTipo").text("Agregue al menos una linea de transmision.");
            contError++;;
        }

    } else if (tipoIntegrante == DISTRIBUIDOR) {


        TipoAgente = "Distribuidor";

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
            $("#mensajeTipo").show();
            $("#mensajeTipo").text("Ingrese un valor válido en Máxima demanda coincidente anual");
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
            $("#mensajeTipo").show();
            $("#mensajeTipo").text("Ingrese un valor válido en Máxima demanda contratada.");
            contError++;;
        }

        var numeroSuministrador = $("#txtNumeroSuministrador").val();
        if (numeroSuministrador == "" || isNaN(numeroSuministrador) || numeroSuministrador == 0) {
            $("#mensajeTipo").show();
            $("#mensajeTipo").text("Ingrese un valor válido en número de Suministrador");
            contError++;;
        }
    }

    var conErrorTA = 0;
    $(".TituloAdicionalFileInput").each(function () {
        if (this.value == "") {
            contError++;
            conErrorTA++;
        }
    });
    if (conErrorTA) {
        $("#mensajeTipo").show();
        $("#mensajeTipo").text("Seleccione un archivo para titulo habilitante.");
    }
    return contError;
}

RepresentanteLegal = function () {
    var contError = 0;

    $(".RepresentanteLegalFicha").each(function () {
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

    return contError;
}

PersonaContacto = function () {
    var contError = 0;

    $(".PersonaContacto").each(function () {
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

    return contError;
}

SelecionarTab = function (NroTab) {
    if (NroTab === 'tabs1-css') {
        $('#btnTab2').removeClass('coes-tab--active');
        $('#btnTab3').removeClass('coes-tab--active');
        $('#btnTab4').removeClass('coes-tab--active');
        $('#btnTab5').removeClass('coes-tab--active');
        $('#btnTab6').removeClass('coes-tab--active');
        $('#btnTab1').addClass('coes-tab--active');

        $("#tabs2-css").css("display", "none");
        $("#tabs3-css").css("display", "none");
        $("#tabs4-css").css("display", "none");
        $("#tabs5-css").css("display", "none");
        $("#tabs6-css").css("display", "none");
        $("#tabs1-css").css("display", "contents");
    }
    if (NroTab === 'tabs2-css') {
        $('#btnTab1').removeClass('coes-tab--active');
        $('#btnTab3').removeClass('coes-tab--active');
        $('#btnTab4').removeClass('coes-tab--active');
        $('#btnTab5').removeClass('coes-tab--active');
        $('#btnTab6').removeClass('coes-tab--active');
        $('#btnTab2').addClass('coes-tab--active');

        $("#tabs1-css").css("display", "none");
        $("#tabs3-css").css("display", "none");
        $("#tabs4-css").css("display", "none");
        $("#tabs5-css").css("display", "none");
        $("#tabs6-css").css("display", "none");
        $("#tabs2-css").css("display", "contents");
    }
    if (NroTab === 'tabs3-css') {
        $('#btnTab1').removeClass('coes-tab--active');
        $('#btnTab2').removeClass('coes-tab--active');
        $('#btnTab4').removeClass('coes-tab--active');
        $('#btnTab5').removeClass('coes-tab--active');
        $('#btnTab6').removeClass('coes-tab--active');
        $('#btnTab3').addClass('coes-tab--active');

        $("#tabs1-css").css("display", "none");
        $("#tabs2-css").css("display", "none");
        $("#tabs4-css").css("display", "none");
        $("#tabs5-css").css("display", "none");
        $("#tabs6-css").css("display", "none");
        $("#tabs3-css").css("display", "contents");
    }
    if (NroTab === 'tabs4-css') {
        $('#btnTab1').removeClass('coes-tab--active');
        $('#btnTab2').removeClass('coes-tab--active');
        $('#btnTab3').removeClass('coes-tab--active');
        $('#btnTab5').removeClass('coes-tab--active');
        $('#btnTab6').removeClass('coes-tab--active');
        $('#btnTab4').addClass('coes-tab--active');

        $("#tabs1-css").css("display", "none");
        $("#tabs2-css").css("display", "none");
        $("#tabs3-css").css("display", "none");
        $("#tabs5-css").css("display", "none");
        $("#tabs6-css").css("display", "none");
        $("#tabs4-css").css("display", "contents");
    }
    if (NroTab === 'tabs5-css') {
        $('#btnTab1').removeClass('coes-tab--active');
        $('#btnTab2').removeClass('coes-tab--active');
        $('#btnTab3').removeClass('coes-tab--active');
        $('#btnTab4').removeClass('coes-tab--active');
        $('#btnTab6').removeClass('coes-tab--active');
        $('#btnTab5').addClass('coes-tab--active');

        $("#tabs1-css").css("display", "none");
        $("#tabs2-css").css("display", "none");
        $("#tabs3-css").css("display", "none");
        $("#tabs4-css").css("display", "none");
        $("#tabs6-css").css("display", "none");
        $("#tabs5-css").css("display", "contents");
    }
    if (NroTab === 'tabs6-css') {
        $('#btnTab1').removeClass('coes-tab--active');
        $('#btnTab2').removeClass('coes-tab--active');
        $('#btnTab3').removeClass('coes-tab--active');
        $('#btnTab4').removeClass('coes-tab--active');
        $('#btnTab5').removeClass('coes-tab--active');
        $('#btnTab6').addClass('coes-tab--active');

        $("#tabs1-css").css("display", "none");
        $("#tabs2-css").css("display", "none");
        $("#tabs3-css").css("display", "none");
        $("#tabs4-css").css("display", "none");
        $("#tabs5-css").css("display", "none");
        $("#tabs6-css").css("display", "contents");
    }

}

EnviarDatos = function () {

    //Validar Responsable Tramite
    if (ResponsableTramite() > 0) {
        //Tabs("tabs3-css");
        //$('#tab-container').easytabs('select', "tabs2-css");.
        SelecionarTab("tabs2-css");
        return;
    }

    if (IsOnlyDigit($('#txtTelefonoRT').val()) == false) {
        $("#spnTelefonoRT").addClass("Error");
        $("#mensajeRT").show();
        $("#mensajeRT").text("Ingrese un teléfono válido.");
        //Tabs("tabs3-css");
        //$('#tab-container').easytabs('select', "tabs2-css");
        SelecionarTab("tabs2-css");
        return;
    }
    if (IsEmail($('#txtCorreoElectronicoRT').val()) == false) {
        $("#spnCorreoElectronicoRT").addClass("Error");
        $("#mensajeRT").show();
        $("#mensajeRT").text("Ingrese un correo válido.");
        //Tabs("tabs3-css");
        //$('#tab-container').easytabs('select', "tabs2-css");
        SelecionarTab("tabs2-css");
        return;
    }
    if (IsEmail($('#txtCorreoElectronicoRepetirRT').val()) == false) {
        $("#spnCorreoElectronicoRepetirRT").addClass("Error");
        $("#mensajeRT").show();
        $("#mensajeRT").text("Ingrese un correo válido.");
        //Tabs("tabs3-css");
        //$('#tab-container').easytabs('select', "tabs2-css");
        SelecionarTab("tabs2-css");
        return;
    }
    if ($("#txtCorreoElectronicoRT").val() != $("#txtCorreoElectronicoRepetirRT").val()) {
        $("#spnCorreoElectronicoRT").addClass("Error");
        $("#spnCorreoElectronicoRepetirRT").addClass("Error");

        $("#mensajeRT").show();
        $("#mensajeRT").text("Los correos no son iguales.");
        //Tabs("tabs3-css");
        //$('#tab-container').easytabs('select', "tabs2-css");
        SelecionarTab("tabs2-css");
        return;
    }

    if (!$('#cbCondicion1').is(':checked')) {
        $("#mensajeRT").show();
        $("#mensajeRT").text("Acepte las condiciones de tratamiento de datos personales.");        
        //$('#tab-container').easytabs('select', "tabs2-css");
        SelecionarTab("tabs2-css");
        return;
    }



    //Validar Empresa
    if (DatosEmpresa() > 0) {
        //Tabs("tabs4-css");
        //$('#tab-container').easytabs('select', "tabs3-css");
        SelecionarTab("tabs3-css");
        return;
    }

    if (IsOnlyNumber($('#txtNumeroRucDE').val()) == false) {
        $("#spntxtNumeroRucDE").addClass("Error");
        $("#mensajeEMP").show();
        $("#mensajeEMP").text("Ingrese un RUC válido.");
        //Tabs("tabs4-css");
        //$('#tab-container').easytabs('select', "tabs3-css");
        SelecionarTab("tabs3-css");
        return;
    }
    if (IsOnlyDigit($('#txtTelefonoDE').val()) == false) {
        $("#spnTelefonoDE").addClass("Error");
        $("#mensajeEMP").show();
        $("#mensajeEMP").text("Ingrese un teléfono válido.");
        //Tabs("tabs4-css");
        //$('#tab-container').easytabs('select', "tabs3-css");
        SelecionarTab("tabs3-css");
        return;
    }

    //Validar Tipo
    if (ValidarTipoAgente() > 0) {
        //Tabs("tabs5-css");
        //$('#tab-container').easytabs('select', "tabs4-css");
        SelecionarTab("tabs4-css");
        return;
    }

    //Validar Rpte Legal
    var LenRL = $(".tblRLDetalle").length;
    if (LenRL == 0) {
        alert("Agrege al menos un representante legal");
        //Tabs("tabs6-css");
        //$('#tab-container').easytabs('select', "tabs5-css");
        SelecionarTab("tabs5-css");
        return;
    }
    if (RepresentanteLegal() > 0) {
        //Tabs("tabs6-css");
        //$('#tab-container').easytabs('select', "tabs5-css");
        SelecionarTab("tabs5-css");
        return;
    }

    if (PersonaContacto() > 0) {
        return;
    }

    var nombreContacto = $("#txtNombresPC").val();
    var apellidosContacto = $("#txtApellidosPC").val();

    if (ValidarContactoDiferenteRpteLegal(nombreContacto, apellidosContacto) == false) {
        $("#spnNombresPC").addClass("Error");
        $("#spnApellidosPC").addClass("Error");
        $("#mensajePC").show();
        $("#mensajePC").text("La Persona de Contacto no puede ser igual a uno de los Representantes Legales.");
        return;
    }


    if (IsOnlyDigit($('#txtTelefonoPC').val()) == false) {
        $("#spnTelefonoPC").addClass("Error");
        $("#mensajePC").show();
        $("#mensajePC").text("Ingrese un teléfono válido.");
        return;
    }
    if (IsEmail($('#txtCorreoElectronicoPC').val()) == false) {
        $("#spnCorreoElectronicoPC").addClass("Error");
        $("#mensajePC").show();
        $("#mensajePC").text("Ingrese un correo válido.");
        return;
    }
    if (IsEmail($('#txtCorreoElectronicoRepetirPC').val()) == false) {
        $("#spnCorreoElectronicoRepetirPC").addClass("Error");
        $("#mensajePC").show();
        $("#mensajePC").text("Ingrese un correo válido.");
        return;
    }
    if ($("#txtCorreoElectronicoPC").val() != $("#txtCorreoElectronicoRepetirPC").val()) {
        $("#spnCorreoElectronicoPC").addClass("Error");
        $("#spnCorreoElectronicoRepetirPC").addClass("Error");

        $("#mensajePC").show();
        $("#mensajePC").text("Los correos no son iguales.");
        return;
    }

    if(!$('#cbCondicion2').is(':checked')) {
        $("#mensajePC").show();
        $("#mensajePC").text("Acepte las condiciones de tratamiento de datos personales.");        
        return;
    }

    //try {
    //    var response = grecaptcha.getResponse();
    //    if (response.length == 0) {
    //        $("#mensajePC").text('Favor de realizar la valicación [No Soy Robot].');
    //        $("#mensajePC").show();
    //        return;
    //    }
    //}
    //catch (e) {
    //    $("#mensajePC").text('Favor de realizar la valicación [No Soy Robot].');
    //    $("#mensajePC").show();
    //    return;
    //}
    $("#mensajePC").hide();

    var GENERADOR = $("#hfTipoComportamientoGenerador_codigo").val();
    var TRANSMISOR = $("#hfTipoComportamientoTrasmisor_codigo").val();
    var DISTRIBUIDOR = $("#hfTipoComportamientoDistribuidor_codigo").val();
    var USUARIOLIBRE = $("#hfTipoComportamientoUsuarioLibre_codigo").val();

    var tipoIntegrante = $("#TipoEmpresa").val();
    var TipoAgente = "";
    if (tipoIntegrante == GENERADOR) {
        TipoAgente = "Generador";
    } else if (tipoIntegrante == TRANSMISOR) {
        TipoAgente = "Transmisor";
    } else if (tipoIntegrante == DISTRIBUIDOR) {
        TipoAgente = "Distribuidor";
    } else if (tipoIntegrante == USUARIOLIBRE) {
        TipoAgente = "UsuarioLibre";
    }

    var formData = new FormData();
    //formData.append("Capcha", response);

    //Guardar Informacion
    var RLDNI = new Array();

    RLDNI.push($("#flCartaSolicitudIngresoRT")[0].files[0]);

    // Datos de Empresa 
    formData.append("EMPRRUC", $("#txtNumeroRucDE").val());
    formData.append("Tipoemprcodi", parseInt(tipoIntegrante));
    formData.append("EMPRNOMBRECOMERCIAL", $("#txtNombreComercialDE").val());

    formData.append("EMPRDIRE", $("#txtDomicilioLegalDE").val());
    formData.append("EMPRTELE", $("#txtTelefonoDE").val());
    formData.append("EMPRNUMEDOCU", $("#txtNumeroPartidaRegistralDE").val());

    formData.append("EMPRNOMB", $("#txtDenominacionRazonSocialDE").val().substr(0, 50));
    formData.append("EMPRRAZSOCIAL", $("#txtDenominacionRazonSocialDE").val());

    //formData.append("EMPRABREV", $("#txtSiglaDE").val());
    formData.append("EMPRABREV", "");

    formData.append("EMPRSIGLA", $("#txtSiglaDE").val());

    formData.append("EMPRFAX", $("#txtFaxDE").val());
    formData.append("EMPRPAGWEB", $("#txtPaginaWebDE").val());

    // Responsable Tramite

    formData.append("RTNombres", $("#txtNombresRT").val());
    formData.append("RTApellidos", $("#txtApellidosRT").val());
    formData.append("RTCargoEmpresa", $("#txtCargoEmpresaRT").val());
    formData.append("RTTelefono", $("#txtTelefonoRT").val());
    formData.append("RTTelefonoMobil", $("#txtTelefonoMovilRT").val());
    formData.append("RTCorreoElectronico", $("#txtCorreoElectronicoRT").val());
    formData.append("RTCartaSolicitudIngreso", $("#flCartaSolicitudIngresoRT")[0].files[0]); // File 

    // Persona Contacto

    formData.append("PCNombres", $("#txtNombresPC").val());
    formData.append("PCApellidos", $("#txtApellidosPC").val());
    formData.append("PCCargoEmpresa", $("#txtCargoEmpresaPC").val());
    formData.append("PCTelefono", $("#txtTelefonoPC").val());
    formData.append("PCTelefonoMobil", $("#txtTelefonoMovilPC").val());
    formData.append("PCCorreoElectronico", $("#txtCorreoElectronicoPC").val());

    //Representante Legal


    var NRepresentanteLegal = 0;

    var tblRL = document.getElementById("tablaRepresentanteLegalbody");
    NRepresentanteLegal = tblRL.rows.length;
    var strRepresentateLegal = "";
    var Index = 0;
    for (var i = 0; i < NRepresentanteLegal; i++) {
        strRepresentateLegal += $(tblRL.rows[i].cells[0]).text().replace('|', '') + "*";
        strRepresentateLegal += $(tblRL.rows[i].cells[1]).text().replace('|', '')  + "*";
        strRepresentateLegal += $(tblRL.rows[i].cells[2]).text().replace('|', '')  + "*";
        strRepresentateLegal += $(tblRL.rows[i].cells[3]).text().replace('|', '')  + "*";
        strRepresentateLegal += $(tblRL.rows[i].cells[4]).text().replace('|', '')  + "*";
        strRepresentateLegal += $(tblRL.rows[i].cells[5]).text().replace('|', '')  + "*";
        strRepresentateLegal += $(tblRL.rows[i].cells[6]).text().replace('|', '')  + "*";
        strRepresentateLegal += $(tblRL.rows[i].cells[9]).text().replace('|', '')  + "*";
        Index = $(tblRL.rows[i].cells[10]).text();
        strRepresentateLegal += Index;
        strRepresentateLegal += "|";

        formData.append("flDNIRL" + Index, $("#flDNIRL" + Index)[0].files[0]);
        formData.append("flVigenciaPoderRL" + Index, $("#flVigenciaPoderRL" + Index)[0].files[0]);


    }
    strRepresentateLegal = strRepresentateLegal.substring(0, strRepresentateLegal.length - 1);
    formData.append("strRepresentateLegal", strRepresentateLegal);


    // Tipo

    var TipoDocumentoSustentario = "";
    var TipoDoc = $("#TipoDocumentoSustentario").val();
    if (TipoDoc == "1") {
        TipoDocumentoSustentario = "AUTORIZACIÓN";
    } else if (TipoDoc == "2") {
        TipoDocumentoSustentario = "CONCESIÓN";
    } else {
        TipoDocumentoSustentario = "DECLARACIÓN JURADA";
    }

    formData.append("TIPODOCSUSTENTATORIO", TipoDocumentoSustentario);
    formData.append("TIPOARCDIGITALIZADO", $("#flArchivoDigitalizadoTI")[0].files[0]);
    formData.append("TIPOPOTENCIAINSTALADA", $("#txtPotenciaInstaladaTI").val());
    formData.append("TIPONROCENTRALES", $("#txtNumeroCentralesTI").val());
    formData.append("TIPOLINEATRANS_500KM", $("#val500").val());
    formData.append("TIPOLINEATRANS_220KM", $("#val200").val());
    formData.append("TIPOLINEATRANS_138KM", $("#val138").val());
    formData.append("TIPOLINEATRANS_MENOR138KM", $("#valMenor138").val());

    var SiNo500 = "N";
    if ($("#chk500").prop('checked')) {
        SiNo500 = "S";
    }
    formData.append("TIPOLINEATRANS_500", SiNo500);

    var SiNo200 = "N";
    if ($("#chk200").prop('checked')) {
        SiNo200 = "S";
    }
    formData.append("TIPOLINEATRANS_220", SiNo200);

    var SiNo138 = "N";
    if ($("#chk138").prop('checked')) {
        SiNo138 = "S";
    }
    formData.append("TIPOLINEATRANS_138", SiNo138);

    var SiNoMenor138 = "N";
    if ($("#hkMenor138").prop('checked')) {
        SiNo138 = "S";
    }
    formData.append("TIPOLINEATRANS_MENOR138", SiNoMenor138);

    formData.append("TIPOMAXDEMANDACOINCIDENTE", $("#txtMaximaDemandaCoincidenteAnual").val());
    formData.append("TIPOMAXDEMANDACONTRATADA", $("#txtMaximaDemandaContratada").val());

    formData.append("TIPONUMSUMINISTRADOR", $("#txtNumeroSuministrador").val());
    formData.append("TIPOTIPAGENTE", TipoAgente);

    var contador = 1;
    $(".TituloAdicionalFile").each(function () {
        if ($(this).val() != "") {
            if (contador <= 5) {
                formData.append("Tipodocname" + contador, $(this)[0].files[0]);
            }
            contador++;
        }
    });

    formData.append("Tipocomentario", $("#txtobstitulosadicionales").val());

    $.ajax({
        type: "POST",
        url: controlador + "Insertar",
        data: formData,
        dataType: 'json',
        contentType: false,
        processData: false,
        success: function (data) {
            data = data.toString();

            //if (data == -1) {
            //    alert('Error al Validar el formulario [NO SOY ROBOT].');
            //} else
            if (data == -2) {
                alert('Error al Validar los datos de la empresa: RUC y/o Nombre Existente en COES SINAC.');
            } else if (data == -3) {
                alert('El RUC de la empresa esta de BAJA.');
            } else if (data != "") {
                var para = data.split(',');
                var id = parseInt(para[0]);
                //Edición garantia 9Jul18
                var f = new Date();
                var nroconstancia = parseInt(para[1]) + "." + f.getDate() + "." + (f.getMonth() + 1) + "." + f.getFullYear();
                //fin edición
                $("#hfEmprcodi").val(id);
                $("#btnEnviar").hide();
                $("#btnExportar").show();
                $('#btnNuevoRegistro').show();
                $("#btnAnterior").hide();
                //$("#btnCapcha").hide();
                $("#spnEmpCodi").text(nroconstancia);
                //$('#divConstancia').bPopup({
                //    follow: [true, true],
                //    position: ['auto', 'auto'],
                //    positionStyle: 'fixed',
                //});
                $('#divConstancia').bPopup({
                });
            } else {
                alert('Se ha producido un error');
            }
        },
        error: function (e) {
            alert('Error');
        }
    });

    
}

Reenviar = function () {
    var contError = 0;

    var LenRL = $(".tblRLDetalle").length;
    if (LenRL == 0) {
        alert("Agrege al menos un representante legal");
        Tabs("tabs5-css");
        return;
    }


    // Val 5 tipo integrante   

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


    } else if (tipoIntegrante == TRANSMISOR) {

        TipoAgente = "Transmisor";

        var chk500 = $("#chk500").prop('checked');
        var val500 = $("#val500").val();

        var chk200 = $("#chk200").prop('checked');
        var val200 = $("#val200").val();

        var chk138 = $("#chk138").prop('checked');
        var val138 = $("#val138").val();

        var chkMenor138 = $("#chkMenor138").prop('checked');
        var valMenor138 = $("#valMenor138").val();

        var mensaje = "";
        var suma = 0;
        suma += (val500 && !isNan(val500)) ? parseInt(val500) : 0;
        suma += (val200 && !isNaN(val200)) ? parseInt(val200) : 0;
        suma += (val138 && !isNaN(val138)) ? parseiNT(val138) : 0;
        suma += (valMenor && !isNaN(valMenor138)) ? parseInt(valMenor138) : 0;

        if (suma === 0) {
            alert("Agregue al menos una linea de transmision");
            contError++;;
        }

    } else if (tipoIntegrante == DISTRIBUIDOR) {


        TipoAgente = "Distribuidor";

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

    }

    var conErrorTA = 0;
    $(".TituloAdicionalFileInput").each(function () {
        if (this.value == "") {
            contError++;
            conErrorTA++;
        }
    });
    if (conErrorTA) {
        alert("Seleccione un archivo para titulo habilitante");
    }
    if (contError > 0) {
        Tabs("tabs4-css");
        return;
    }

    var formData = new FormData();
    formData.append("Emprcodi", $("#hfEmprcodi").val());

    // Datos de Empresa 
    formData.append("Tipoemprcodi", parseInt(tipoIntegrante));

    //Representante Legal
    var NRepresentanteLegal = 0;

    var tblRL = document.getElementById("tablaRepresentanteLegalbody");
    NRepresentanteLegal = tblRL.rows.length;
    var strRepresentateLegal = "";
    var Index = 0;
    for (var i = 0; i < NRepresentanteLegal; i++) {
        strRepresentateLegal += $(tblRL.rows[i].cells[0]).text() + "*";
        strRepresentateLegal += $(tblRL.rows[i].cells[1]).text() + "*";
        strRepresentateLegal += $(tblRL.rows[i].cells[2]).text() + "*";
        strRepresentateLegal += $(tblRL.rows[i].cells[3]).text() + "*";
        strRepresentateLegal += $(tblRL.rows[i].cells[4]).text() + "*";
        strRepresentateLegal += $(tblRL.rows[i].cells[5]).text() + "*";
        strRepresentateLegal += $(tblRL.rows[i].cells[6]).text() + "*";
        strRepresentateLegal += $(tblRL.rows[i].cells[9]).text() + "*";
        Index = $(tblRL.rows[i].cells[10]).text().trim();
        strRepresentateLegal += Index + "*";
        strRepresentateLegal += $(tblRL.rows[i].cells[11]).text() + "*"; // OBS
        strRepresentateLegal += $(tblRL.rows[i].cells[12]).text() + "*"; // DNI
        strRepresentateLegal += $(tblRL.rows[i].cells[13]).text() + "*"; // DNIFN
        strRepresentateLegal += $(tblRL.rows[i].cells[14]).text() + "*"; // VP
        strRepresentateLegal += $(tblRL.rows[i].cells[15]).text(); // VPFN
        strRepresentateLegal += "|";
        if ($(tblRL.rows[i].cells[11]).text() != "") {
            if ($("#flDNIRL" + Index).val() == "") {
                contError++;
            }
            else if ($("#flVigenciaPoderRL" + Index).val() == "") {
                contError++;
            } else {
                formData.append("flDNIRL" + Index, $("#flDNIRL" + Index)[0].files[0]);
                formData.append("flVigenciaPoderRL" + Index, $("#flVigenciaPoderRL" + Index)[0].files[0]);
            }
        }
    }


    if (contError > 0) {
        alert("Ingrese informacion del representante observado");
        Tabs("tabs5-css");
        return;
    }

    strRepresentateLegal = strRepresentateLegal.substring(0, strRepresentateLegal.length - 1);
    formData.append("strRepresentateLegal", strRepresentateLegal);


    // Tipo

    var TipoDocumentoSustentario = ""; TipoDocumentoSustentario
    var TipoDoc = $("#TipoDocumentoSustentario").val();
    if (TipoDoc == "1") {
        TipoDocumentoSustentario = "AUTORIZACIÓN";
    } else if (TipoDoc == "2") {
        TipoDocumentoSustentario = "CONCESIÓN";
    } else {
        TipoDocumentoSustentario = "DECLARACIÓN JURADA";
    }

    formData.append("TIPODOCSUSTENTATORIO", TipoDocumentoSustentario);
    formData.append("TIPOARCDIGITALIZADO", $("#flArchivoDigitalizadoTI")[0].files[0]);
    formData.append("TIPOPOTENCIAINSTALADA", $("#txtPotenciaInstaladaTI").val());
    formData.append("TIPONROCENTRALES", $("#txtNumeroCentralesTI").val());
    formData.append("TIPOLINEATRANS_500KM", $("#val500").val());
    formData.append("TIPOLINEATRANS_220KM", $("#val200").val());
    formData.append("TIPOLINEATRANS_138KM", $("#val138").val());
    formData.append("TIPOLINEATRANS_MENOR138KM", $("#valMenor138").val());

    formData.append("Tipoarchivodigitalizado", $("#flArchivoDigitalizadoTI").attr("data-file"));
    formData.append("Tipoarchivodigitalizadofilename", $("#flArchivoDigitalizadoTI").attr("data-filename"));

    var SiNo500 = "N";
    if ($("#chk500").prop('checked')) {
        SiNo500 = "S";
    }
    formData.append("TIPOLINEATRANS_500", SiNo500);

    var SiNo200 = "N";
    if ($("#chk200").prop('checked')) {
        SiNo200 = "S";
    }
    formData.append("TIPOLINEATRANS_220", SiNo200);

    var SiNo138 = "N";
    if ($("#chk138").prop('checked')) {
        SiNo138 = "S";
    }
    formData.append("TIPOLINEATRANS_138", SiNo138);

    var SiNoMenor138 = "N";
    if ($("#chkMenor138").prop('checked')) {
        SiNoMenor138 = "S";
    }
    formData.append("TIPOLINEATRANS_MENOR138", SiNoMenor138);

    formData.append("TIPOMAXDEMANDACOINCIDENTE", $("#txtMaximaDemandaCoincidenteAnual").val());
    formData.append("TIPOMAXDEMANDACONTRATADA", $("#txtMaximaDemandaContratada").val());

    formData.append("TIPONUMSUMINISTRADOR", $("#txtNumeroSuministrador").val());
    formData.append("TIPOTIPAGENTE", TipoAgente);

    var contador = "";
    var TAstr = "";
    $(".TituloAdicionalFile").each(function () {

        if ($(this.cells[0]).has("input").length > 0) {
            contador = $(this.cells[0]).find("input").attr("data-number").trim();
            formData.append("Tipodocname" + contador, $(this.cells[0]).find("input")[0].files[0]);
        }
        TAstr += this.cells[2].innerText + "^" + this.cells[3].innerText + "^" + this.cells[4].innerText + "^" + this.cells[5].innerText;
        TAstr += "|";
    });
    if (TAstr != "") {
        TAstr = TAstr.substring(0, TAstr.length - 1);
    }
    formData.append("Tipotastr", TAstr);

    formData.append("ReviiteracionSGI", $("#hfReviiteracionSGI").val());
    formData.append("ReviiteracionDJR", $("#hfReviiteracionDJR").val());
    formData.append("Tipocodi", $("#hfTipoCodi").val());

    formData.append("Tipocomentario", $("#txtobstitulosadicionales").val());

    $.ajax({
        type: "POST",
        url: controlador + "Reenviar",
        data: formData,
        dataType: 'html',
        contentType: false,
        processData: false,
        success: function (data) {
            if (data > 0) {
                //Edición garantia 9Jul18
                var f = new Date();
                var nroconstancia = $("#hfNroConstancia").val() + "." + f.getDate() + "." + (f.getMonth() + 1) + "." + f.getFullYear();
                //fin edición
                $("#hfEmprcodi").val(data);
                $("#btnEnviar").hide();
                $("#btnExportar").show();
                $('#btnNuevoRegistro').show();
                $("#btnAnterior").hide();
                $("#spnEmpCodi").text(nroconstancia);
                //$('#divConstancia').bPopup({
                //    follow: [true, true],
                //    position: ['auto', 'auto'],
                //    positionStyle: 'fixed',
                //});
                $('#divConstancia').bPopup({
                });
            //} else if (data == -1) {
            //    alert('Error al Validar el formulario [NO SOY ROBOT].');
            } else {
                alert('Se ha producido un error');
            }
        },
        error: function () {
            alert('Error');
        }
    });

}

ValidarRL = function () {
    var validator = true;
    var cont = 0;
    $(".RepresentanteLegal").each(function () {

        if ($(this).hasClass("Required")) {
            var spn = $(this).attr("data-validator-id");
            if ($(this).val() == "" || $(this).val() == "0" || $(this).val() == null) {
                $("#" + spn).addClass("Error");
                cont++;
            } else {
                $("#" + spn).removeClass("Error");
            }
        }
    });

    if (IsOnlyDigit($('#txtTelefonoRL').val()) == false) {
        $("#spnTelefonoRL").addClass("Error");
        $("#mensajeRL").show();
        $("#mensajeRL").text("Ingrese un teléfono válido.");
        return;
    }

    if (IsOnlyDigit($('#txtMovilRL').val()) == false) {
        $("#spnMovilRL").addClass("Error");
        $("#mensajeRL").show();
        $("#mensajeRL").text("Ingrese un teléfono válido.");
        return;
    }

    if (IsEmail($('#txtCorreoRL').val()) == false) {
        $("#spnCorreoRL").addClass("Error");
        $("#mensajeRL").show();
        $("#mensajeRL").text("Ingrese un correo válido.");
        return;
    }
    if (IsEmail($('#txtCorreoRepetirRL').val()) == false) {
        $("#spnCorreoRepetirRL").addClass("Error");
        $("#mensajeRL").show();
        $("#mensajeRL").text("Ingrese un correo válido.");
        return;
    }

    if ($("#txtCorreoRL").val() != $("#txtCorreoRepetirRL").val()) {
        $("#spnCorreoRL").addClass("Error");
        $("#spnCorreoRepetirRL").addClass("Error");

        $("#mensajeRL").show();
        $("#mensajeRL").text("Los correos no son iguales.");

        cont++;
    }

    if (cont > 0) {
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

AgregarTitulosAdicionales = function () {

    $("#lblObservacionTA").show();
    $("#txtobstitulosadicionales").show();

    var html = "";
    html += "<tr><td style='text-align: left;'><input type='file' class='TituloAdicionalFile' accept='.pdf' onchange='ValidarSize(this)' data-validator-id='spnflCartaSolicitudIngresoRT' style='width:400px;'></td><td style='text-align: left;'><span style='padding-left:10px; padding-right:10px'>PDF (Máximo 8Mb)</span>&nbsp;&nbsp;&nbsp;<span onclick='EliminarTA(this)'>Eliminar<span></td></tr>";
    $("#tblTitulosAdicionales tbody").append(html);
}

EliminarTA = function (obj) {
    $(obj).parent().parent().remove();
}

descargarTA = function (datos) {
    document.location.href = controlador + 'Download?url=' + datos;
}

Exportar = function () {
    var id = $("#hfEmprcodi").val();
    var url = controlador + "ExportarPDF/" + id;
    var link = document.createElement("a");
    link.href = url;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
    delete link;
}

nuevoRegistro = function () {
    document.location.href = controlador + 'index';
}

Ayuda = function () {
    $('#divAyuda').bPopup({
        follow: [true, true],
        position: ['auto', 'auto'],
        positionStyle: 'fixed',
    });

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
function IsOnlyNumber(texto) {
    for (var i = 0; i < texto.length; i++)
        if (!(IsDigito(texto[i])))
            return false;
    return true;
}
function VerAlerta(obj) {
    var mensaje = $(obj).attr("data-alerta");
    $("#pMensaje").empty();
    $("#pMensaje").append(mensaje);
    $('#divAlerta').bPopup({
        follow: [true, true],
        position: ['auto', 'auto'],
        positionStyle: 'fixed',
    });
}

var getBrowserInfo = function () {
    var ua = navigator.userAgent, tem,
        M = ua.match(/(opera|chrome|safari|firefox|msie|trident(?=\/))\/?\s*(\d+)/i) || [];
    if (/trident/i.test(M[1])) {
        tem = /\brv[ :]+(\d+)/g.exec(ua) || [];
        return 'IE ' + (tem[1] || '');
    }
    if (M[1] === 'Chrome') {
        tem = ua.match(/\b(OPR|Edge)\/(\d+)/);
        if (tem != null) return tem.slice(1).join(' ').replace('OPR', 'Opera');
    }
    M = M[2] ? [M[1], M[2]] : [navigator.appName, navigator.appVersion, '-?'];
    if ((tem = ua.match(/version\/(\d+)/i)) != null) M.splice(1, 1, tem[1]);
    return M.join(' ');
};