var eventoInforme = false;
var UrlAbsoluta = "";
var controlador = siteRoot + 'Eventos/AnalisisFallas/';
var controladorCriterio = siteRoot + 'eventos/CriteriosEvento/';
var columnaChecks = 5;
var HEIGHT_MINIMO = 300;

iniLista = function () {
    
    $('#dtInicio').Zebra_DatePicker({
    });

    $('#dtFin').Zebra_DatePicker({
    });

    $('#dtInicioCriterio').Zebra_DatePicker({
    });

    $('#dtFinCriterio').Zebra_DatePicker({
    });

    $("#btnConsultar").click(function () {
        Consultar();
    });

    $("#btnManualUsuario").click(function () {
        window.location = controlador + 'DescargarManualUsuario';
    });

    $("#btnConsultarCriterio").click(function () {
        ConsultarCriterio();
    });

    $("#btnConfiguracionResponsables").click(function () {
        window.open(controlador + "ListaResponsables/", '_blank');
    });
}


iniUpdate = function () {

    $("#flEventoInforme").change(function () {
        if (this.files.length == 0)
            eventoInforme = false;
        else
            eventoInforme = true;
    });

    $("#cboReunionTipoReunion").change(function () {

        var data = $(this).val();

        if (data == "PRESENCIAL") {
            $("#btnReservarSala").show();
            $("#lblSala").css("display", "block");
            $("#txtSala").show();
            $("#AsistenteResponsable_Presencial").css("display", "block");
            $("#HPresencial").css("display", "contents");
            AsistenteResponsable();
        }
        else {
            $("#btnReservarSala").hide();
            $("#lblSala").css("display", "none");
            $("#txtSala").hide();
            $("#AsistenteResponsable_Presencial").css("display", "none");
            $("#HPresencial").css("display", "none");
        }
    });

    $("#btnReservarSala").click(function () {
        window.open("http://intranet.coes.org.pe/sgc/visorSalas.html", '_blank');
    });

    $("#cboEventoVersion").val("PRELIMINAR");

    $(function () {
        $('#tab-container').easytabs();
    });

    $('#txtEventoFechaInforme').Zebra_DatePicker({});
    $('#dtCitacionNominal').Zebra_DatePicker({});
    $('#dtCitacionElaboracion').Zebra_DatePicker({});
    $('#dtReunionNominal').Zebra_DatePicker({});
    $('#dtReunionProgramada').Zebra_DatePicker({});
    $('#dtAFEITFECHANOMINAL').Zebra_DatePicker({});
    $('#dtAFEITFECHAELAB').Zebra_DatePicker({});
    $('#dtAFEITDECFECHANOMINAL').Zebra_DatePicker({});
    $('#dtAFEITDECFECHAELAB').Zebra_DatePicker({});
    $('#dtAFEITRDJRFECHAENVIO').Zebra_DatePicker({});
    $('#dtAFEITRDJRFECHARECEP').Zebra_DatePicker({});
    $('#dtAFEITRDOFECHAENVIO').Zebra_DatePicker({});
    $('#dtAFEITRDOFECHARECEP').Zebra_DatePicker({});
    $('#dtInformeTecnicoAnual').Zebra_DatePicker({});
    $('#dtFuerzaMayor').Zebra_DatePicker({});
    $('#dtFechaInformeCompensacion').Zebra_DatePicker({});
    $('#dtReconsideracion').Zebra_DatePicker({});
    $('#dtReconsideracionCoes').Zebra_DatePicker({});
    $('#dtApelacion').Zebra_DatePicker({});
    $('#dtApelacionCoes').Zebra_DatePicker({});
    $('#dtArbitraje').Zebra_DatePicker({});
    $('#dtArbitrajeCoes').Zebra_DatePicker({});
    $('#txtFechaMedidaAdoptada').Zebra_DatePicker({});
    $('#dtFechaLimite').Zebra_DatePicker({});
    $("#btnSubirDesicionEvento").click(function () {
        SubirDesicionEvento();
    });

    $("#btnSubirFuerzaMayor").click(function () {
        SubirFuerzaMayor();
    });

    $("#btnSubirFuerzaMayorCoes").click(function () {
        SubirFuerzaMayorCoes();
    });

    $("#btnSubirReconsideracion").click(function () {
        SubirReconsideracion();
    });

    $("#btnSubirReconsideracionCoes").click(function () {
        SubirReconsideracionCoes();
    });

    $("#btnSubirApelacion").click(function () {
        SubirApelacion()
    });

    $("#btnSubirApelacionCoes").click(function () {
        SubirApelacionCoes()
    });

    $("#btnSubirArbitraje").click(function () {
        SubirArbitraje()
    });

    $("#btnSubirArbitrajeCoes").click(function () {
        SubirArbitrajeCoes()
    });

    $("#btnSubirActaReunion").click(function () {
        SubirActaReunion()
    });

    $("#btnSubirInformeReunion").click(function () {
        SubirInformeReunion()
    });

    $("#btnSubirInformeTecnicoAnual").click(function () {
        SubirInformeTecnicoAnual()
    });

    $("#btnSubirCartaRecomendacionCOES").click(function () {
        SubirCartaRecomendacionCOES()
    });

    $("#btnSubirCartadeRespuesta").click(function () {
        SubirCartadeRespuesta()
    });

    $("#btnSubirCitacionReunion").click(function () {
        SubirCitacionReunion()
    });

    $("#btnSubirInformeCompensacion").click(function () {
        SubirInformeCompensacion()
    });

    $("#btnLimpiar").click(function () {
        $("#flEventoInforme").val('');
    });

    $("#btngrabargeneral").click(function () {

        if (IsOnlyNumber($('#txtAFECORRCtaf').val()) == false) {
            alert('Código de evento no es válido.\r\nNo se puede continuar');
            return;
        }

        ActualizarEvento();
    });

    $("#btnEventoAgregar").click(function () {
        var stage = $(this).attr("data-stage");
        var target = $(this).attr("data-target");
        EventoAgregar(stage, target);
    });

    $("#btnReunionAgregar").click(function () {
        ReunionAgregar();
    });

    $("#btnReunionResponsableAgregar").click(function () {
        ReunionResponsableAgregar();
    });

    $("#btnAsistenteResponsableAgregar").click(function () {
        AsistenteResponsableAgregar();
    });

    $("#btnInformeTecnicoAgregarEmpresaRecomendacion").click(function () {
        InsertarRecomendacionInformeTecnico();

    });

    $("#btnInformeTecnicoAgregarEmpresaObservacion").click(function () {
        InsertarObservacionInformeTecnico();
    });

    $("#btnAgregarEmpresaResponsableCompensacion").click(function () {
        AgregarEmpresaResponsableCompensacion();
    });

    $("#btnCompensacionAgregarEmpresaCompensada").click(function () {
        AgregarEmpresaCompensadaCompensacion();
    });

    $("#btnBuscarEmpresaPropietaria").click(function () {
        BuscarEmpresaPropietaria();
    });

    $("#btnBuscarEmpresa").click(function () {
        BuscarEmpresa();
    });

    $("#btnEnviarCorreo").click(function () {
        EnviarCorreo();
    });

    $("#btnEnviarCorreoActa").click(function () {
        EnviarCorreoActa();
    });

    
    
    var chkInformeTecnicoImpugnacion = document.getElementById("chkInformeTecnicoImpugnacion").checked;

    if (chkInformeTecnicoImpugnacion) {
        $("#aReconsideraciones").text("Reconsideraciones/Apelaciones");
    } else {
        $("#aReconsideraciones").text("");
    }

    $("#chkInformeTecnicoImpugnacion").change(function () {
        if (this.checked) {
            $("#aReconsideraciones").text("Reconsideraciones/Apelaciones");
        } else {
            $("#aReconsideraciones").text("");
        }
    })

    $(".aPopupEmpresa").each(function () {
        $(this).click(function () {
            var target = $(this).attr("data-target");
            $("#btnBuscarEmpresa").attr("data-target", target);
            PopupEmpresa();
        });
    });

    var ReunionTipoReunion = $("#hfReunionTipoReunion").val();
    if (ReunionTipoReunion == "P") {
        $("#cboReunionTipoReunion").val("PRESENCIAL");
        $("#btnReservarSala").show();
        $("#lblSala").css("display", "block");
        $("#txtSala").show();
        $("#AsistenteResponsable_Presencial").css("display", "block");
        $("#HPresencial").css("display", "contents");
        AsistenteResponsable();
    } else {
        $("#cboReunionTipoReunion").val("NO PRESENCIAL");
        $("#btnReservarSala").hide();
        $("#lblSala").css("display", "none");
        $("#txtSala").hide();
        $("#AsistenteResponsable_Presencial").css("display", "none");
        $("#HPresencial").css("display", "none");
    }

    var AFECONVCITACIONFECHA = $("#hfReunionAFECONVCITACIONFECHA").val();
    var AFERCTAEINFORMEFECHA = $("#hfReunionAFERCTAEINFORMEFECHA").val();
    var AFERCTAEACTAFECHA = $("#hfReunionAFERCTAEACTAFECHA").val();

    if (AFECONVCITACIONFECHA != "") {
        $("#btnVerCitacionReunion").show();
        $("#btnEliminarCitacionReunion").show();
    } else {
        $("#btnVerCitacionReunion").hide();
        $("#btnEliminarCitacionReunion").hide();
    }
    $("#btnSubirCitacionReunion").show();
    $("#flCitacionReunion").show();

    if (AFERCTAEACTAFECHA !== "") {
        $("#btnVerActaReunion").show();
        $("#btnEliminarActaReunion").show();
    } else {
        $("#btnVerActaReunion").hide();
        $("#btnEliminarActaReunion").hide();
    }
    $("#btnSubirActaReunion").show();
    $("#flActaReunion").show();

    if (AFERCTAEINFORMEFECHA != "") {
        $("#btnVerInformeReunion").show();
        $("#btnEliminarInformeReunion").show();
    } else {

        $("#btnVerInformeReunion").hide();
        $("#btnEliminarInformeReunion").hide();
    }
    $("#btnSubirInformeReunion").show();
    $("#flInformeReunion").show();

    // Informe Tecnico Archivos

    var AFEITPITFFECHA = $("#hfReunionAFEITPITFFECHA").val();
    var AFEITPITFFECHASIST = $("#hfReunionAFEITPITFFECHASIST").val();
    var AFEITPDECISFFECHASIST = $("#hfReunionAFEITPDECISFFECHASIST").val();

    if (AFEITPITFFECHASIST != "") {
        $("#btnVerInformeTecnicoAnual").show();
        $("#btnEliminarInformeTecnicoAnual").show();

    } else {
        $("#btnVerInformeTecnicoAnual").hide();
        $("#btnEliminarInformeTecnicoAnual").hide();
    }
    $("#btnSubirInformeTecnicoAnual").show();
    $("#flInformeTecnicoAnual").show();

    if (AFEITPDECISFFECHASIST != "") {
        $("#btnVerDesicionEvento").show();
        $("#btnEliminarDesicionEvento").show();
    } else {
        $("#btnVerDesicionEvento").hide();
        $("#btnEliminarDesicionEvento").hide();
    }
    $("#btnSubirDesicionEvento").show();
    $("#flDesicionEvento").show();

    // Compensacion
    var AFECOMPFECHA = $("#hfCompensacionAFECOMPFECHA").val();
    if (AFECOMPFECHA != "") {
        $("#btnVerInformeCompensacion").show();
        $("#btnEliminarInformeCompensacion").show();
    } else {
        $("#btnVerInformeCompensacion").hide();
        $("#btnEliminarInformeCompensacion").hide();
    }
    $("#btnSubirInformeCompensacion").show();
    $("#flInformeCompensacion").show();

    ObtenerEmpresas();
}


Consultar = function () {

    var cboEmpresaPropietaria = $("#cboEmpresaPropietaria").val();
    var cboEmpresaInvolucrada = $("#cboEmpresaInvolucrada").val();
    var cboTipoEquipo = $("#cboTipoEquipo").val();
    var cboEstado = $("#cboEstado").val();
    var cboImpugnacion = $("#cboImpugnacion").val();
    var cboTipoReunion = $("#cboTipoReunion").val();
    var dtInicio = $("#dtInicio").val();
    var dtFin = $("#dtFin").val();

    // checks
    var chkRNC = $("#chkRNC").is(':checked');
    var chkERACMF = $("#chkERACMF").is(':checked');
    var chkERACMT = $("#chkERACMT").is(':checked');
    var chkEDAGSF = $("#chkEDAGSF").is(':checked');

    var Estado = "N";
    var FuerzaMayor = "N";
    var Anulado = "N";
    var Impugnacion = "N";
    var TipoReunion = "T";

    if (cboEstado != "TODOS") {
        if (cboEstado == "FUERZA MAYOR") {
            FuerzaMayor = "S";
            Estado = cboEstado;
        } else if (cboEstado == "ANULADO") {
            Anulado = "A";
            Estado = cboEstado;
        } else if (cboEstado == "ARCHIVADO") {
            Anulado = "X";
            Estado = cboEstado;
        } else {
            Estado = cboEstado;
        }
    }

    if (cboImpugnacion != "TODOS") {
        Impugnacion = cboImpugnacion;
    }

    if (cboTipoReunion != "TODOS") {
        TipoReunion = cboTipoReunion;
    }


    if (chkRNC) {
        chkRNC = "S";
    } else {
        chkRNC = "N";
    }

    if (chkERACMF) {
        chkERACMF = "S";
    } else {
        chkERACMF = "N";
    }

    if (chkERACMT) {
        chkERACMT = "S";
    } else {
        chkERACMT = "N";
    }

    if (chkEDAGSF) {
        chkEDAGSF = "S";
    } else {
        chkEDAGSF = "N";
    }

    var obj = {
        EmpresaPropietaria: cboEmpresaPropietaria,
        EmpresaInvolucrada: cboEmpresaInvolucrada,
        TipoEquipo: cboTipoEquipo,
        Estado: Estado,
        Impugnacion: Impugnacion,
        TipoReunion: TipoReunion,
        RNC: chkRNC,
        ERACMF: chkERACMF,
        ERACMT: chkERACMT,
        EDAGSF: chkEDAGSF,
        DI: dtInicio,
        DF: dtFin,
        Anulado: Anulado,
        FuerzaMayor: FuerzaMayor
    }

    $.ajax({
        type: 'POST',
        url: controlador + "Buscar",
        data: obj,
        success: function (json) {

            if (json != "") {
                $("#tbodyLista").empty();
                var njson = json.length;
                var html = "";
                for (var i = 0; i < njson; i++) {
                    html += "<tr>";
                    html += "<td><a href='" + siteRoot + "Eventos/AnalisisFallas/Update/" + json[i].AFECODI + "' onclick='Editar(1)' id='btnEditar' target='_blank'><img src='" + siteRoot + "Content/Images/btn-edit.png' style='margin-top: 5px;' /></a>";
                    html += "<a href= 'JavaScript:eliminarEventoCtaf(" + json[i].EVENCODI + ")' id='btnEliminar'><img src='" + siteRoot + "Content/Images/btn-cancel.png' style='margin-top: 5px;' /></a></td>"
                    html += "<td>" + json[i].CODIGO + "</td>";
                    html += "<td>" + json[i].NOMBRE_EVENTO + "</td>";
                    html += "<td>" + json[i].INTERRUMPIDO + "</td>";
                    html += "<td>" + json[i].FECHA_EVENTO + "</td>";
                    html += "<td>" + json[i].FECHA_REUNION + "</td>";
                    html += "<td>" + json[i].FECHA_INFORME + "</td>";
                    html += "<td>" + json[i].REVISADO_DJR + "</td>";
                    html += "<td>" + json[i].REVISADO_DO + "</td>";
                    html += "<td>" + json[i].PUBLICADO + "</td>";
                    html += "<td>" + json[i].ESTADO + "</td>";
                    html += "<td>" + json[i].AFEFZAMAYOR + "</td>";
                    html += "<td>" + json[i].IMPUG + "</td>";
                    html += "<td>" + json[i].RESPONSABLE + "</td>";
                    html += "<td>" + json[i].INF_TECNICO + "</td>";
                    html += "</tr>";
                }
                html = html.replace(/null/g, '-');
                $("#tbodyLista").append(html);

            } else {
                //alert("No existe resultados para la consulta");
                $("#tbodyLista").empty();
            }
        },
        error: function () {
            //mostrarError();
        }
    });

}

PopupEmpresa = function () {
    $('#divBuscarEmpresa').bPopup({
        follow: [true, true],
        position: ['auto', 'auto'],
        positionStyle: 'fixed',
    });

}

PopupEmpresaPropietaria = function () {
    $('#divBuscarEmpresaPropietaria').bPopup({
        follow: [true, true],
        position: ['auto', 'auto'],
        positionStyle: 'fixed',
    });
}

BuscarEmpresa = function () {
    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var nombreempresa = $("#txtNombreEmpresaBuscar").val();
    $.ajax({
        type: 'POST',
        url: controlador + "BuscarEmpresa?nombreempresa=" + nombreempresa,
        success: function (json) {

            $("#tblbodyEmpresa").empty();
            var html = "<tr><td colspan='2' stlye='color:red'>Sin resultado</td></tr>";
            if (json != "") {


                var dbClick = "";
                var njson = json.length;
                html = "";
                for (var i = 0; i < njson; i++) {
                    html += "<tr data-id='" + json[i].Emprcodi + "' data-name='" + json[i].Emprnomb + "' ondblclick='CargarEmpresa(this)'>";
                    html += "<td>" + json[i].Emprcodi + "</td>";
                    html += "<td>" + json[i].Emprnomb + "</td>";
                    html += "</tr>";
                }
                $("#tblbodyEmpresa").append(html);

            } else {
                $("#tblbodyEmpresa").append(html);
            }
        },
        error: function () {
            //mostrarError();
        }
    });
}

ObtenerEmpresas = function () {
    var controlador = siteRoot + 'Eventos/AnalisisFallas/';

    $.ajax({
        type: 'POST',
        url: controlador + "ObtenerEmpresa",
        success: function (json) {


            var html = "";
            if (json != "") {

                var njson = json.length;
                html = "";
                for (var i = 0; i < njson; i++) {
                    html += "<option value='" + json[i].Emprcodi + "'>" + json[i].Emprnomb + "</option>";
                }
                $("#txtEventoEmpresaInvolucrada").append(html);
                $("#txtReunionEmpresaInvolucrada").append(html);
                $("#txtInformeTecnicoEmpresaRecomendacion").append(html);
                $("#txtInformeTecnicoEmpresaObservacion").append(html);
                $("#txtFuerzaMayorEmpresa").append(html);
                $("#txtCompensacionEmpresaResponsable").append(html);
                $("#txtReunionEmpresaResponsable").append(html);
                $("#txtCompensacionEmpresaCompensada").append(html);
                $("#txtEmpresaReconsideracion").append(html);
                $("#txtEmpresaApelacion").append(html);
                $("#txtEmpresaArbitraje").append(html);

                $('#txtEventoEmpresaInvolucrada').selectToAutocomplete();
                $('#txtReunionEmpresaInvolucrada').selectToAutocomplete();
                $('#txtInformeTecnicoEmpresaRecomendacion').selectToAutocomplete();
                $('#txtInformeTecnicoEmpresaObservacion').selectToAutocomplete();
                $('#txtFuerzaMayorEmpresa').selectToAutocomplete();
                $('#txtCompensacionEmpresaResponsable').selectToAutocomplete();
                $('#txtReunionEmpresaResponsable').selectToAutocomplete();
                $('#txtCompensacionEmpresaCompensada').selectToAutocomplete();
                $('#txtEmpresaReconsideracion').selectToAutocomplete();
                $('#txtEmpresaApelacion').selectToAutocomplete();
                $('#txtEmpresaArbitraje').selectToAutocomplete();

            }
        },
        error: function () {
            //mostrarError();
        }
    });
}

BuscarEmpresaPropietaria = function () {
    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var nombreempresa = $("#txtNombreEmpresaBuscar").val();
    $.ajax({
        type: 'POST',
        url: controlador + "BuscarEmpresaPropietaria?nombreempresa=" + nombreempresa,
        success: function (json) {
            if (json != "") {
                $("#tblbodyEmpresaPropietaria").empty();
                var njson = json.length;
                var html = "";
                for (var i = 0; i < njson; i++) {
                    html += "<tr>";
                    html += "<td>" + json[i].Emprcodi + "</td>";
                    html += "<td>" + json[i].Emprnomb + "</td>";
                    html += "</tr>";
                }
                $("#tblbodyEmpresaPropietaria").append(html);

                $('#tblbodyEmpresaPropietaria').hover(
                    function () {
                        $(this).addClass('highlight');
                    }, function () {
                        $(this).removeClass('highlight');
                    }
                )
            }
        },
        error: function () {
            //mostrarError();
        }
    });
}

CargarEmpresa = function (obj) {

    var txtEmpresa = $("#btnBuscarEmpresa").attr("data-target");
    var CodigoEmpresa = $(obj).attr("data-id");
    var NombreEmpresa = $(obj).attr("data-name");

    $("#" + txtEmpresa).val(NombreEmpresa);
    $("#" + txtEmpresa).attr("data-id", CodigoEmpresa);
    $("#" + txtEmpresa).attr("data-name", NombreEmpresa);

    var bPopup = $('#divBuscarEmpresa').bPopup();
    bPopup.close();

}

EventoAgregar = function (stage, target) {

    var CodigoEmpresa = $("#txtEventoEmpresaInvolucrada").val();
    var txtEventoFechaInforme = $("#txtEventoFechaInforme").val();
    var cboEventoVersion = $("#cboEventoVersion").val();
    var cboEventoCumplimiento = $("#cboEventoCumplimiento").val();
    var txtEventoMensaje = $("#txtEventoMensaje").val();


    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var frmValidacion = new FormData();
    frmValidacion.append("AFECODI", $("#hfCodigo").val());
    frmValidacion.append("EMPRCODI", CodigoEmpresa);
    frmValidacion.append("AFIVERSION", cboEventoVersion.substring(0, 1));

    $.ajax({
        url: controlador + "ExisteIEI",
        data: frmValidacion,
        type: 'POST',
        processData: false,
        contentType: false,
        success: function (result) {

            if (result) {

                var validacion = true;

                $("#tbodyEventoEmpresaInvolucrada tr").each(function (index) {
                    var cumplimiento = $(this).find("td").eq(4).html();

                    if (cumplimiento == "NO EXISTEN INFORMES") {
                        validacion = false;
                    }

                })

                // Validar 
                var Mensaje = "";
                if (validacion) {

                    if (txtEventoFechaInforme == "") {
                        Mensaje += "<li>Por favor seleccione fecha de informe</li>";
                    }

                    if (cboEventoCumplimiento == "") {

                        if (eventoInforme == false) {
                            Mensaje += "<li>Por favor seleccione informe</li>";
                        }

                        if (CodigoEmpresa == "" || CodigoEmpresa == "-1") {
                            Mensaje += "<li>Por favor busque una empresa</li>";
                        }


                    } else {

                        if (cboEventoCumplimiento == "FUERA DE PLAZO") {

                            if (eventoInforme == false) {
                                Mensaje += "<li>Por favor seleccione informe</li>";
                            }

                            if (CodigoEmpresa == "" || CodigoEmpresa == "-1") {
                                Mensaje += "<li>Por favor busque una empresa</li>";
                            }
                        }


                        if (cboEventoCumplimiento == "NO ENVIADO") {

                            if (CodigoEmpresa == "" || CodigoEmpresa == "-1") {
                                Mensaje += "<li>Por favor busque una empresa</li>";
                            }

                        }

                        if (cboEventoCumplimiento == "NO EXISTEN INFORMES") {

                            if (CodigoEmpresa != "" && CodigoEmpresa != "-1") {
                                Mensaje += "<li>No debe seleccionar una empresa, seleccione ( SINAC) NO DEFINIDO </li>";
                            }

                            if (txtEventoMensaje == "") {
                                Mensaje += "<li>Por favor ingrese un Mensaje</li>";
                            }
                        }

                    }


                } else {
                    Mensaje += "<li>Error. Existe registro\r\nNo se puede continuar...</li>";
                }

                if (Mensaje != "") {
                    $("#ulMensaje").empty();
                    $("#ulMensaje").append(Mensaje);

                    $('#divMensaje').bPopup({
                        follow: [true, true],
                        position: ['auto', 'auto'],
                        positionStyle: 'fixed',
                    });
                    return;
                }

                // Fechas    
                var dtFechaInforme = txtEventoFechaInforme;
                // Fin Fechas

                if (stage == "Agregar") {

                    var Cumplimiento = "";
                    if (cboEventoCumplimiento == "FUERA DE PLAZO") { Cumplimiento = "F"; }
                    if (cboEventoCumplimiento == "NO ENVIADO") { Cumplimiento = "E"; }
                    if (cboEventoCumplimiento == "NO EXISTEN INFORMES") { Cumplimiento = "N"; }

                    //var controlador = siteRoot + 'Eventos/AnalisisFallas/';
                    var frm = new FormData();
                    frm.append("AFECODI", $("#hfCodigo").val());
                    frm.append("EMPRCODI", CodigoEmpresa);
                    frm.append("AFIVERSION", cboEventoVersion.substring(0, 1));
                    frm.append("CUMPLIMIENTO", Cumplimiento);
                    frm.append("AFIMENSAJE", txtEventoMensaje);
                    frm.append("AFIFECHAINFEVE", dtFechaInforme);
                    frm.append("AFIPUBLICA", "S");//Default S
                    frm.append("ANIO", $("#txtEventoAnioCtaf").val());//Default S
                    frm.append("file", $("#flEventoInforme")[0].files[0]);

                    $.ajax({
                        url: controlador + "InsertarEmpresaInvolucradaEvento",
                        data: frm,
                        type: 'POST',
                        processData: false,
                        contentType: false,
                        success: function (result) {

                            if (result) {
                                // Cleaning
                                $("#txtEventoEmpresaInvolucrada").val("");
                                $("#txtEventoFechaInforme").val("");
                                $("#flEventoInforme").val("");
                                $("#cboEventoVersion").val("PRELIMINAR");
                                $("#cboEventoCumplimiento").val("");
                                $("#txtEventoMensaje").val("");
                                TraerEmpresaInvolucrada();

                            }
                        }
                    });
                }





            } else {

                $("#ulMensaje").empty();
                $("#ulMensaje").append("Error.Existe registro\r\nNo se puede continuar...");

                $('#divMensaje').bPopup({
                    follow: [true, true],
                    position: ['auto', 'auto'],
                    positionStyle: 'fixed',
                });
                return;
            }
        }
    });

}



EventoEliminarEmpresaInvolucrada = function (obj, id) {
    id = parseInt(id);
    if (id > 0) {
        //Push into array of deletes
        ///
    }
    $(obj).closest("tr").remove();
}

ReunionAgregar = function () {

    //var txtReunionEmpresaInvolucrada = $("#txtReunionEmpresaInvolucrada").val();
    var CodigoEmpresa = $("#txtReunionEmpresaInvolucrada").val();
    //var NombreEmpresa = $("#txtReunionEmpresaInvolucrada").attr("data-name");

    // Validar 
    var Mensaje = "";
    if (CodigoEmpresa == "") {
        Mensaje += "<li>Por favor busque una empresa</li>";
    }

    if (Mensaje != "") {
        $("#ulMensaje").empty();
        $("#ulMensaje").append(Mensaje);

        $('#divMensaje').bPopup({
            follow: [true, true],
            position: ['auto', 'auto'],
            positionStyle: 'fixed',
        });
        return;
    }


    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1; //January is 0!

    var yyyy = today.getFullYear();
    if (dd < 10) {
        dd = '0' + dd;
    }
    if (mm < 10) {
        mm = '0' + mm;
    }
    var today = dd + '/' + mm + '/' + yyyy;

    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var frm = new FormData();
    frm.append("AFECODI", $("#hfCodigo").val());
    frm.append("EMPRCODI", CodigoEmpresa);
    frm.append("AFIVERSION", "X");
    frm.append("AFIFECHAINFEVE", today);
    frm.append("AFIPUBLICA", "N");


    $.ajax({
        url: controlador + "InsertarEmpresaInvolucradaReunion",
        data: frm,
        type: 'POST',
        processData: false,
        contentType: false,
        success: function (result) {
            if (result != "")
                alert(result);

            $("#txtReunionEmpresaInvolucrada").val("");
            $("#txtReunionEmpresaInvolucrada").attr("data-id", "");
            $("#txtReunionEmpresaInvolucrada").attr("data-name", "");
            TraerEmpresaInvolucradaReunion();

        },
        error: function () {
            //mostrarError();
        }
    });



}



ReunionResponsableAgregar = function () {

    //var txtReunionEmpresaInvolucrada = $("#txtReunionEmpresaInvolucrada").val();
    var CodigoResponsable = $("#cboReunionResponsable").val();
    //var NombreEmpresa = $("#txtReunionEmpresaInvolucrada").attr("data-name");

    // Validar 
    var Mensaje = "";
    if (CodigoResponsable == "") {
        Mensaje += "<li>Por favor seleccione un responsable</li>";
    }

    if (Mensaje != "") {
        $("#ulMensaje").empty();
        $("#ulMensaje").append(Mensaje);

        $('#divMensaje').bPopup({
            follow: [true, true],
            position: ['auto', 'auto'],
            positionStyle: 'fixed',
        });
        return;
    }


    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var frm = new FormData();

    frm.append("EVENCODI", $("#hfCodigoEvento").val());
    frm.append("RESPCOD", CodigoResponsable);

    $.ajax({
        url: controlador + "InsertarReunionResponsable",
        data: frm,
        type: 'POST',
        processData: false,
        contentType: false,
        success: function (result) {
            if (result != "")
                alert(result);

            $("#cboReunionResponsable").val("");
            TraerReunionResponsable();

        },
        error: function () {
            //mostrarError();
        }
    });



}

AsistenteResponsableAgregar = function () {

    var CodigoEmpresa = $("#cboAsistenteResponsable").val();
    var txtAsistente = $("#txtAsistenteResponsable").val();

    // Validar 
    var Mensaje = "";
    if (CodigoEmpresa == "") {
        Mensaje += "<li>Por favor seleccione una empresa</li>";
    } else if(txtAsistente == "") {
        Mensaje += "<li>Por favor ingrese un asistente</li>";
    }

    if (Mensaje != "") {
        $("#ulMensaje").empty();
        $("#ulMensaje").append(Mensaje);

        $('#divMensaje').bPopup({
            follow: [true, true],
            position: ['auto', 'auto'],
            positionStyle: 'fixed',
        });
        return;
    }


    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var frm = new FormData();

    frm.append("EVENCODI", $("#hfCodigoEvento").val());
    frm.append("RESPCOD", CodigoEmpresa);
    frm.append("EVEPARTICIPANTE", txtAsistente);

    $.ajax({
        url: controlador + "InsertarAsistenteResponsable",
        data: frm,
        type: 'POST',
        processData: false,
        contentType: false,
        success: function (result) {
            if (result != "")
                alert(result);

            $("#cboAsistenteResponsable").val("");
            $("#txtAsistenteResponsable").val("");
            AsistenteResponsable();

        },
        error: function () {
            //mostrarError();
        }
    });

}

ReunionEliminarEmpresaInvolucrada = function (obj, id) {
    id = parseInt(id);
    if (id > 0) {
        //Push into array of deletes
        ///
    }
    $(obj).closest("tr").remove();
}

InsertarRecomendacionInformeTecnico = function () {

    //var CodigoEmpresa = $("#txtInformeTecnicoEmpresaRecomendacion").val();
    var Numero = $("#txtNumeroRecomendacion").val();
    var txtInformeTecnicoPublicarRecomendacion = $("#txtInformeTecnicoPublicarRecomendacion").val();

    var IdEquipo = $('#hfEquiCodi').val();
    var CodigoEmpresa = $('#hfIdEmpresa').val();
    var IdSubestacion = $('#hfIdSubestacion').val();

    // Validar 
    var Mensaje = "";
    if (CodigoEmpresa == "") {
        Mensaje += "<li>Por favor busque una empresa</li>";
    }

    if (txtInformeTecnicoPublicarRecomendacion == "") {
        Mensaje += "<li>Por favor ingrese una recomendacion</li>";
    }

    if (isNaN(Numero) || parseInt(Numero) <= 0 || Numero == "") {
        Mensaje += "<li>Por favor ingrese un numero mayor a 0</li>"
    }


    if (Mensaje != "") {
        $("#ulMensaje").empty();
        $("#ulMensaje").append(Mensaje);

        $('#divMensaje').bPopup({
            follow: [true, true],
            position: ['auto', 'auto'],
            positionStyle: 'fixed',
        });
        return;
    }

    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var frm = new FormData();
    frm.append("AFECODI", $("#hfCodigo").val());
    frm.append("EMPRCODI", CodigoEmpresa);
    frm.append("AFRRECOMEND", txtInformeTecnicoPublicarRecomendacion);
    frm.append("AFRCORR", Numero);
    frm.append("IDEQUIPO", IdEquipo);
    frm.append("IDSUBESTACION", IdSubestacion);

    $.ajax({
        url: controlador + "InsertarRecomendacionInformeTecnico",
        data: frm,
        type: 'POST',
        processData: false,
        contentType: false,
        success: function (result) {
            alert(result);
            // Cleaning
            $("#txtInformeTecnicoEmpresaRecomendacion").val("");
            $("#txtInformeTecnicoPublicarRecomendacion").val("");
            $("#txtNumeroRecomendacion").val("1");
            TraerEmpresaRecomendacionInformeTecnico();

        }
    });

}

EliminarRecomendacionInformeTecnico = function (emprcodi, afrcorr) {

    if (confirm("Esta seguro que desea eliminar el registro?")) {
        var controlador = siteRoot + 'Eventos/AnalisisFallas/';
        var frm = new FormData();
        frm.append("AFECODI", $("#hfCodigo").val());
        frm.append("EMPRCODI", emprcodi);
        frm.append("AFRCORR", afrcorr);

        $.ajax({
            url: controlador + "EliminarRecomendacionInformeTecnico",
            data: frm,
            type: 'POST',
            processData: false,
            contentType: false,
            success: function (result) {
                if (result) {
                    alert('Se elimino correctamente');
                    TraerEmpresaRecomendacionInformeTecnico();
                }
            }
        });
    }

}

TraerEmpresaRecomendacionInformeTecnico = function () {
    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var id = $("#hfCodigo").val();
    var caracter = "'''";
    $.ajax({
        type: 'POST',
        url: controlador + "TraerEmpresaRecomendacionInformeTecnico/" + id,
        success: function (json) {


            var html = "";
            if (json != "") {
                var njson = json.length;
                for (var i = 0; i < njson; i++) {
                    html += "<tr>";                    
                    //html += "<td><a href='#' onclick='EliminarRecomendacionInformeTecnico(" + json[i].EMPRCODI + "," + json[i].AFRCORR + ")' ><img src='" + siteRoot + "Content/Images/btn-cancel.png' style='margin-top: 2px;'/></a></td>";

                    //html += "<td><table style='border: hidden; width: 55px'><tr><td style='border:hidden'>";

                    if (json[i].EVENRCMCTAF == 'S') {
                        html += "<td><a href='#'  ><img src='" + siteRoot + "Content/Images/btn-cancel-disabled.png' style='margin-top: 2px;'/></a></td>";

                        html += "<td><table style='border: hidden; width: 55px'><tr><td style='border:hidden'>";
                        html += "<input type='checkbox' class='chEventoAO' checked='checked' disabled='disabled' value=" + json[i].AFRREC + ">";
                    }
                    else {
                        html += "<td><a href='#' onclick='EliminarRecomendacionInformeTecnico(" + json[i].EMPRCODI + "," + json[i].AFRCORR + ")' ><img src='" + siteRoot + "Content/Images/btn-cancel.png' style='margin-top: 2px;'/></a></td>";

                        html += "<td><table style='border: hidden; width: 55px'><tr><td style='border:hidden'>";

                        html += "<input type='checkbox' class='chEventoAO' value=" + json[i].AFRREC + ">";
                    }
                    html += "<td style='border: hidden'><a href='#' onclick='EliminarEventoCtafxRecomendacion(" + json[i].AFECODI + "," + json[i].AFRREC + ")' id='btnEliminarAO'><img src='" + siteRoot + "Content/Images/btn-cancel.png' style='margin-top: 2px;'/></a></td></tr></table></td>";

                    html += "<td><a href='#' onclick='VerMedidasAdoptadas(" + json[i].AFRREC + ")'><img src='" + siteRoot + "Content/Images/btn-properties.png' style='margin-top: 5px;'/></a></td>";
                    html += "<td>" + json[i].EMPRNOMB + "</td>";
                    html += "<td>" + json[i].AFRRECOMEND + "</td>";
                    html += "<td>" + json[i].LASTUSER + "</td>";
                    html += "<td>" + json[i].LASTDATEstr + "</td>";
                    html += "</tr>";


                }
                $("#tbodyInformeTecnicoEmpresaRecomendacion").empty();
                $("#tbodyInformeTecnicoEmpresaRecomendacion").append(html);

            } else {
                $("#tbodyInformeTecnicoEmpresaRecomendacion").empty();
            }
        }
    });
}

// Aquiii

InsertarObservacionInformeTecnico = function () {

    var CodigoEmpresa = $("#txtInformeTecnicoEmpresaObservacion").val();
    var txtInformeTecnicoPublicarObservacion = $("#txtInformeTecnicoPublicarObservacion").val();

    // Validar 
    var Mensaje = "";
    if (CodigoEmpresa == "") {
        Mensaje += "<li>Por favor busque una empresa</li>";
    }

    if (txtInformeTecnicoPublicarObservacion == "") {
        Mensaje += "<li>Por favor ingrese una observacion</li>";
    }

    if (Mensaje != "") {
        $("#ulMensaje").empty();
        $("#ulMensaje").append(Mensaje);

        $('#divMensaje').bPopup({
            follow: [true, true],
            position: ['auto', 'auto'],
            positionStyle: 'fixed',
        });
        return;
    }

    var Numero = $("#tbodyInformeTecnicoEmpresaObservacion").length + 1;

    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var frm = new FormData();
    frm.append("AFECODI", $("#hfCodigo").val());
    frm.append("EMPRCODI", CodigoEmpresa);
    frm.append("AFOOBSERVAC", txtInformeTecnicoPublicarObservacion);
    frm.append("AFOCORR", Numero);


    $.ajax({
        url: controlador + "InsertarObservacionInformeTecnico",
        data: frm,
        type: 'POST',
        processData: false,
        contentType: false,
        success: function (result) {
            if (result) {
                // Cleaning
                $("#txtInformeTecnicoEmpresaObservacion").val("");
                $("#txtInformeTecnicoPublicarRecomendacion").val("");
                TraerEmpresaObservacionInformeTecnico();
            }


        }
    });

}

EliminarObservacionInformeTecnico = function (AFOOBS) {

    if (confirm("Esta seguro que desea eliminar el registro?")) {
        var controlador = siteRoot + 'Eventos/AnalisisFallas/';
        var frm = new FormData();
        frm.append("AFOOBS", AFOOBS);


        $.ajax({
            url: controlador + "EliminarObservacionInformeTecnico",
            data: frm,
            type: 'POST',
            processData: false,
            contentType: false,
            success: function (result) {
                if (result) {
                    alert('Se elimino correctamente');
                    TraerEmpresaObservacionInformeTecnico();
                }
            }
        });
    }

}

TraerEmpresaObservacionInformeTecnico = function () {
    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var id = $("#hfCodigo").val();

    $.ajax({
        type: 'POST',
        url: controlador + "TraerEmpresaObservacionInformeTecnico/" + id,
        success: function (json) {


            var html = "";
            if (json != "") {
                var njson = json.length;

                for (var i = 0; i < njson; i++) {
                    html += "<tr>";
                    html += "<td><a href='#' onclick='EliminarObservacionInformeTecnico(" + json[i].AFOOBS + ")'><img src='" + siteRoot + "Content/Images/btn-cancel.png' style='margin-top: 5px;'/></a></td>";
                    html += "<td>" + json[i].EMPRNOMB + "</td>";
                    html += "<td>" + json[i].AFOOBSERVAC + "</td>";
                    html += "<td>" + json[i].LASTUSER + "</td>";
                    html += "<td>" + json[i].LASTDATEstr + "</td>";
                    html += "</tr>";


                }
                $("#tbodyInformeTecnicoEmpresaObservacion").empty();
                $("#tbodyInformeTecnicoEmpresaObservacion").append(html);

            } else {
                $("#tbodyInformeTecnicoEmpresaObservacion").empty();
            }
        }
    });
}


InformeTecnicoEliminarEmpresaRecomendacion = function (obj, id) {
    id = parseInt(id);
    if (id > 0) {
        //Push into array of deletes
        ///
    }
    $(obj).closest("tr").remove();
}

InformeTecnicoEliminarEmpresaObservacion = function (obj, id) {
    id = parseInt(id);
    if (id > 0) {
        //Push into array of deletes
        ///
    }
    $(obj).closest("tr").remove();
}
InformeTecnicoEliminarEmpresaFuerzaMayor = function (obj, id) {
    id = parseInt(id);
    if (id > 0) {
        //Push into array of deletes
        ///
    }
    $(obj).closest("tr").remove();
}

VerMedidasAdoptadas = function (id) {

    var controlador = siteRoot + 'Eventos/AnalisisFallas/';

    $.ajax({
        url: controlador + "ObtenerMedidasAdoptadas/" + id,
        type: 'POST',
        processData: false,
        contentType: false,
        success: function (json) {
            if (json != "") {

                $("#hfMedidaAdoptadaSelected").val(id);

                $("#txtMedidaAdoptadaAFECORR").val(json.AFECORR);
                $("#txtMedidaAdoptadaAFEANIO").val(json.AFEANIO);
                $("#txtMedidaAdoptadaEVENASUNTO").val(json.EVENASUNTO);
                $("#txtMedidaAdoptadaEMPRNOMB").val(json.EMPRNOMB);
                $("#txtMedidaAdoptadaAFRRECOMEND").val(json.AFRRECOMEND);

                if (json.INDIMPORTANTE == "S") {
                    document.getElementById("chkMedidaAdoptadaImportante").checked = true;
                }

                $("#txtMedidaAdoptadaNroRegistroRespuesta").val(json.NROREGRESPUESTA);
                $("#txtFechaMedidaAdoptada").val(json.AFRMEDADOPFECHAstr);
                $("#txtMedidaAdoptada").val(json.AFRMEDADOPMEDIDA);


                if (json.AFRPUBLICAFECHA != null) {
                    document.getElementById("chkCartaRecomendacion").checked = true;
                }

                if (json.AFRPUBLICAFECHA != null) {
                    $("#btnVerCartaRecomendacionCOES").show();
                } else {
                    $("#btnSubirCartaRecomendacionCOES").show();
                    $("#flCartaRecomendacionCOES").show();
                }


                if (json.LASTDATERPTA != null) {
                    $("#btnVerCartaRespuesta").show();
                    $("#btnEliminarVerCartaRespuesta").show();
                } else {
                    $("#btnSubirCartadeRespuesta").show();
                    $("#flCartaRespuesta").show();
                }




                var Cumplimiento = "";

                //switch (json.AFRMEDADOPNIVCUMP) {
                //    case "E":
                //        Cumplimiento = "EN PROCESO";
                //        break;
                //    case "P":
                //        Cumplimiento = "PARCIAL";
                //        break;
                //    case "C":
                //        Cumplimiento = "CUMPLIO";
                //        break;
                //    case "S":
                //        Cumplimiento = "SIN CUMPLIR";
                //        break;
                //    case "R":
                //        Cumplimiento = "PENDIENTE DE RESPUESTA";
                //        break;
                //}

                //valor por defecto
                $('#cboNiveldeCumplimiento').val("R");


                $("#cboNiveldeCumplimiento").val(json.AFRMEDADOPNIVCUMP);



                $('#divMedidasAdoptadas').bPopup({
                    follow: [true, true],
                    position: ['auto', 'auto'],
                    positionStyle: 'fixed',
                });

            }
        }
    });


}


ActualizarRecomendacionMA = function () {

    var controlador = siteRoot + 'Eventos/AnalisisFallas/';

    var Importante = "N";
    var chk = document.getElementById("chkMedidaAdoptadaImportante").checked;
    if (chk) {
        Importante = "S";
    }

    var frm = new FormData();
    frm.append("AFRREC", $("#hfMedidaAdoptadaSelected").val());
    frm.append("NROREGRESPUESTA", $("#txtMedidaAdoptadaNroRegistroRespuesta").val());
    frm.append("INDIMPORTANTE", Importante);

    $.ajax({
        url: controlador + "ActualizarRecomendacionMA",
        data: frm,
        type: 'POST',
        processData: false,
        contentType: false,
        success: function (result) {
            if (result) {
                alert("Se actualizo correctamente");
            } else {
                alert("No se pudo actualizar");
            }
        }
    });

}

ActualizarRecomendacionMAG = function () {

    var controlador = siteRoot + 'Eventos/AnalisisFallas/';


    var frm = new FormData();
    frm.append("AFRREC", $("#hfMedidaAdoptadaSelected").val());

    if ($("#cboNiveldeCumplimiento").val() != null)
        frm.append("AFRMEDADOPNIVCUMP", $("#cboNiveldeCumplimiento").val());
    else
        frm.append("AFRMEDADOPNIVCUMP", "R");

    frm.append("AFRMEDADOPMEDIDA", $("#txtMedidaAdoptada").val());
    frm.append("AFRMEDADOPFECHAU", $("#txtFechaMedidaAdoptada").val());

    $.ajax({
        url: controlador + "ActualizarRecomendacionMAG",
        data: frm,
        type: 'POST',
        processData: false,
        contentType: false,
        success: function (result) {
            if (result) {
                alert("Se actualizo correctamente");
            } else {
                alert("No se pudo actualizar");
            }
        }
    });

}

SubirCartaRecomendacionCOES = function () {

    var file = $("#flCartaRecomendacionCOES")[0].files[0];
    if (file != null) {

        var controlador = siteRoot + 'Eventos/AnalisisFallas/';
        var frm = new FormData();
        frm.append("AFRREC", $("#hfMedidaAdoptadaSelected").val());
        frm.append("ANIO", $("#txtEventoAnioCtaf").val());
        frm.append("file", file);

        $.ajax({
            url: controlador + "ActualizarCartaRecomendacionCOES",
            data: frm,
            type: 'POST',
            processData: false,
            contentType: false,
            success: function (result) {
                if (result) {

                    $("#btnSubirCartaRecomendacionCOES").show();
                    $("#btnVerCartaRecomendacionCOES").show();
                    $("#flCartaRecomendacionCOES").val("");

                    document.getElementById("chkCartaRecomendacion").checked = true;

                    alert('Se cargo correctamente');
                }
            }
        });


    } else {
        alert('Seleccione un archivo');
    }

}

EliminarCartaRespuesta = function () {


    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var frm = new FormData();
    frm.append("AFRREC", $("#hfMedidaAdoptadaSelected").val());
    frm.append("ANIO", $("#txtEventoAnioCtaf").val());

    $.ajax({
        url: controlador + "EliminarCartaRespuesta",
        data: frm,
        type: 'POST',
        processData: false,
        contentType: false,
        success: function (result) {
            if (result) {
                $("#flCartaRecomendacionCOES").val("");

                $("#btnVerCartaRespuesta").hide();
                $("#btnEliminarVerCartaRespuesta").hide();


                $("#btnSubirCartadeRespuesta").show();
                $("#flCartaRespuesta").show();

                alert('Se elimino correctamente');
            }
        }
    });




}


SubirCartadeRespuesta = function () {

    var file = $("#flCartaRespuesta")[0].files[0];
    if (file != null) {

        var controlador = siteRoot + 'Eventos/AnalisisFallas/';
        var frm = new FormData();
        frm.append("AFRREC", $("#hfMedidaAdoptadaSelected").val());
        frm.append("ANIO", $("#txtEventoAnioCtaf").val());
        frm.append("AFRMEDADOPNIVCUMP", $("#cboNiveldeCumplimiento").val());
        frm.append("AFRMEDADOPMEDIDA", $("#txtMedidaAdoptada").val());
        frm.append("AFRMEDADOPFECHAU", $("#txtFechaMedidaAdoptada").val());
        frm.append("file", file);

        $.ajax({
            url: controlador + "ActualizarCartaRespuesta",
            data: frm,
            type: 'POST',
            processData: false,
            contentType: false,
            success: function (result) {
                if (result) {
                    $("#flCartaRespuesta").val("");

                    $("#btnVerCartaRespuesta").show();
                    $("#btnEliminarVerCartaRespuesta").show();
                    alert('Se cargo correctamente');
                }
            }
        });


    } else {
        alert('Seleccione un archivo');
    }

}

CompensacionEliminarEmpresaResponsable = function (obj, id) {
    id = parseInt(id);
    if (id > 0) {
        //Push into array of deletes
        ///
    }
    $(obj).closest("tr").remove();
}

CompensacionEliminarEmpresaCompensada = function (obj, id) {
    id = parseInt(id);
    if (id > 0) {
        //Push into array of deletes
        ///
    }
    $(obj).closest("tr").remove();
}

ReconsideracionVerReclamoReconsideracion = function (obj, id) {

}

ReconsideracionEliminarReclamoReconsideracion = function (obj, id) {
    id = parseInt(id);
    if (id > 0) {
        //Push into array of deletes
        ///
    }
    $(obj).closest("tr").remove();
}

TraerEmpresaInvolucrada = function () {
    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var id = $("#hfCodigo").val();

    $.ajax({
        type: 'POST',
        url: controlador + "TraerEmpresaInvolucrada/" + id,
        success: function (json) {

            var html = "";
            if (json != "") {
                var njson = json.length;
                html = "";
                for (var i = 0; i < njson; i++) {
                    var EmpresaInvolucradaMaxId = (i + 1);
                    html += "<tr id='rowEventoEmpresaInvolucrada" + EmpresaInvolucradaMaxId + "' data-id='0'>";
                    html += "<td><a href='#' onclick=VerEmpresaInvolucrada(" + json[i].EMPRCODI + ",'" + json[i].AFIVERSION + "') id='btnReunionEliminar'><img src='" + siteRoot + "Content/Images/btn-open.png' style='margin-top: 5px;'/></a></td>";

                    html += "<td><a href='#' onclick=EliminarEmpresaInvolucrada(" + json[i].EMPRCODI + ",'" + json[i].AFIVERSION + "')><img src='" + siteRoot + "Content/Images/btn-cancel.png' style='margin-top: 5px;'/></a></td>";
                    html += "<td>" + json[i].EMPRNOMB + "</td>";
                    html += "<td>" + json[i].VERSION + "</td>";
                    html += "<td>" + json[i].CUMPLIMIENTO + "</td>";
                    html += "<td>" + json[i].AFIEXTENSION + " </td>";
                    html += "<td>" + json[i].AFIFECHAINFstr + "</td>";
                    html += "<td>" + json[i].AFIMENSAJE + "</td>";
                    html += "<td>" + json[i].LASTUSER + "</td>";
                    html += "<td>" + json[i].LASTDATEstr + "</td>";
                    html += "</tr>";
                }
                $("#tbodyEventoEmpresaInvolucrada").empty();
                $("#tbodyEventoEmpresaInvolucrada").append(html);

            } else {
                $("#tbodyEventoEmpresaInvolucrada").empty();
            }
        }
    });
}

TraerEmpresaInvolucradaReunion = function () {
    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var id = $("#hfCodigo").val();

    $.ajax({
        type: 'POST',
        url: controlador + "TraerEmpresaInvolucradaReunion/" + id,
        success: function (json) {


            var html = "";
            if (json != "") {
                var njson = json.length;

                for (var i = 0; i < njson; i++) {
                    var EmpresaInvolucradaMaxId = (i + 1);
                    html += "<tr id='rowReunionEmpresaInvolucrada" + EmpresaInvolucradaMaxId + "' data-id='0'>";
                    html += "<td><a href='#' onclick='EliminarEmpresaInvolucradaReunion(" + json[i].EMPRCODI + ")' id='btnReunionEliminar'><img src='" + siteRoot + "Content/Images/btn-cancel.png' style='margin-top: 5px;'/></a></td>";
                    html += "<td>" + json[i].EMPRNOMB + "</td>";
                    html += "<td>" + json[i].LASTUSER + "</td>";
                    html += "<td>" + json[i].LASTDATEstr + "</td>";
                    html += "</tr>";

                }
                $("#tbodyReunionEmpresaInvolucrada").empty();
                $("#tbodyReunionEmpresaInvolucrada").append(html);

            } else {
                $("#tbodyReunionEmpresaInvolucrada").empty();
            }
        }
    });
}

TraerReunionResponsable = function () {
    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var id = $("#hfCodigoEvento").val();

    $.ajax({
        type: 'POST',
        url: controlador + "TraerReunionResponsable/" + id,
        success: function (json) {


            var html = "";
            if (json != "") {
                var njson = json.length;

                for (var i = 0; i < njson; i++) {
                    var ReunionResponsableMaxId = (i + 1);
                    html += "<tr id='rowReunionResponsable" + json[i].RESEVENCODI + "' data-id='0'>";
                    html += "<td><a href='#' onclick='EliminarReunionResponsable(" + json[i].RESEVENCODI + ")' id='btnReunionEliminar'><img src='" + siteRoot + "Content/Images/btn-cancel.png' style='margin-top: 5px;'/></a></td>";
                    html += "<td>" + json[i].RESPNAME + "</td>";
                    html += "</tr>";

                }
                $("#tbodyReunionEmpresaCTAF").empty();
                $("#tbodyReunionEmpresaCTAF").append(html);

            } else {
                $("#tbodyReunionEmpresaCTAF").empty();
            }
        }
    });
}

EliminarEmpresaInvolucrada = function (CodigoEmpresa, Version) {

    if (confirm("Esta seguro de Eliminar?")) {
        var controlador = siteRoot + 'Eventos/AnalisisFallas/';

        var frm = new FormData();
        frm.append("AFECODI", $("#hfCodigo").val());
        frm.append("EMPRCODI", CodigoEmpresa);
        frm.append("AFIVERSION", Version);
        frm.append("ANIO", $("#txtEventoAnioCtaf").val());

        $.ajax({
            url: controlador + "EliminarEmpresaInvolucrada",
            data: frm,
            type: 'POST',
            processData: false,
            contentType: false,
            success: function (result) {

                if (result) {
                    TraerEmpresaInvolucrada();
                }
            }
        });
    }
}

EliminarEmpresaInvolucradaReunion = function (CodigoEmpresa) {

    if (confirm("Esta seguro de Eliminar?")) {
        var controlador = siteRoot + 'Eventos/AnalisisFallas/';

        var frm = new FormData();
        frm.append("AFECODI", $("#hfCodigo").val());
        frm.append("EMPRCODI", CodigoEmpresa);

        $.ajax({
            url: controlador + "EliminarEmpresaInvolucradaReunion",
            data: frm,
            type: 'POST',
            processData: false,
            contentType: false,
            success: function (result) {

                if (result) {
                    TraerEmpresaInvolucradaReunion();
                }
            }
        });
    }
}

EliminarReunionResponsable = function (CodigoResponsable) {

    if (confirm("Esta seguro de Eliminar?")) {
        var controlador = siteRoot + 'Eventos/AnalisisFallas/';

        var frm = new FormData();
        frm.append("EVENCODI", $("#hfCodigoEvento").val());
        frm.append("RESEVENCODI", CodigoResponsable);

        $.ajax({
            url: controlador + "EliminarReunionResponsable",
            data: frm,
            type: 'POST',
            processData: false,
            contentType: false,
            success: function (result) {

                if (result) {
                    TraerReunionResponsable();
                }
            }
        });
    }
}

SubirCitacionReunion = function () {

    var file = $("#flCitacionReunion")[0].files[0];
    if (file != null) {

        var controlador = siteRoot + 'Eventos/AnalisisFallas/';
        var frm = new FormData();
        frm.append("afecodi", $("#hfCodigo").val());
        frm.append("anio", $("#txtEventoAnioCtaf").val());
        frm.append("file", file);

        $.ajax({
            url: controlador + "ActualizarFechaConvocatoriaCitacionReunion",
            data: frm,
            type: 'POST',
            processData: false,
            contentType: false,
            success: function (result) {
                if (result) {
                    $("#flCitacionReunion").val("");

                    $("#btnVerCitacionReunion").show();
                    $("#btnEliminarCitacionReunion").show();

                    $("#lblCitacionReunion").text(obtenerFechayhora());

                    alert('Se cargo correctamente');
                }
            }
        });


    } else {
        //alert('Seleccione un archivo');
        //GenerarCitacionPDF();
        GenerarPDF(1);
    }
}

EliminarCitacionReunion = function () {

    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var frm = new FormData();
    frm.append("afecodi", $("#hfCodigo").val());
    frm.append("anio", $("#txtEventoAnioCtaf").val());

    $.ajax({
        url: controlador + "EliminarFechaConvocatoriaCitacionReunion",
        data: frm,
        type: 'POST',
        processData: false,
        contentType: false,
        success: function (result) {
            if (result) {
                $("#btnVerCitacionReunion").hide();
                $("#btnEliminarCitacionReunion").hide();

                $("#lblCitacionReunion").text("");

                alert('Se elimino correctamente');
            }
        }
    });

}

SubirActaReunion = function () {

    var file = $("#flActaReunion")[0].files[0];
    if (file != null) {

        var controlador = siteRoot + 'Eventos/AnalisisFallas/';
        var frm = new FormData();
        frm.append("afecodi", $("#hfCodigo").val());
        frm.append("anio", $("#txtEventoAnioCtaf").val());
        frm.append("file", file);

        $.ajax({
            url: controlador + "ActualizarFechaActaReunion",
            data: frm,
            type: 'POST',
            processData: false,
            contentType: false,
            success: function (result) {
                if (result) {
                    $("#flActaReunion").val("");

                    $("#btnVerActaReunion").show();
                    $("#btnEliminarActaReunion").show();

                    $("#lblActaReunion").text(obtenerFechayhora());

                    alert('Se cargo correctamente');
                }
            }
        });


    } else {
        //alert('Seleccione un archivo');
        GenerarPDF(2);
    }

}

EliminarActaReunion = function () {

    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var frm = new FormData();
    frm.append("afecodi", $("#hfCodigo").val());
    frm.append("anio", $("#txtEventoAnioCtaf").val());

    $.ajax({
        url: controlador + "EliminarFechaActaReunion",
        data: frm,
        type: 'POST',
        processData: false,
        contentType: false,
        success: function (result) {
            if (result) {

                $("#btnSubirActaReunion").show();
                $("#flActaReunion").show();
                $("#btnVerActaReunion").hide();
                $("#btnEliminarActaReunion").hide();

                $("#lblActaReunion").text("");

                alert('Se elimino correctamente');
            }
        }
    });

}

SubirInformeReunion = function () {

    var file = $("#flInformeReunion")[0].files[0];

    if (file != null) {

        var controlador = siteRoot + 'Eventos/AnalisisFallas/';
        var frm = new FormData();
        frm.append("afecodi", $("#hfCodigo").val());
        frm.append("anio", $("#txtEventoAnioCtaf").val());
        frm.append("file", file);

        $.ajax({
            url: controlador + "ActualizarFechaInformeCTAFReunion",
            data: frm,
            type: 'POST',
            processData: false,
            contentType: false,
            beforeSend: function () {
                $(".modal-loading-background").css("visibility", "visible");
            },
            success: function (result) {
                if (result) {
                    $("#flInformeReunion").val("");

                    $("#btnVerInformeReunion").show();
                    $("#btnEliminarInformeReunion").show();

                    $("#lblInformeReunion").text(obtenerFechayhora());

                    alert('Se cargo correctamente');
                }
            },
            complete: function () {
                $(".modal-loading-background").css("visibility", "hidden");
            }
        });
    } else {
        //alert('Seleccione un archivo');
        GenerarPDF(3);
    }

}

EliminarInformeReunion = function () {

    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var frm = new FormData();
    frm.append("afecodi", $("#hfCodigo").val());
    frm.append("anio", $("#txtEventoAnioCtaf").val());

    $.ajax({
        url: controlador + "EliminarFechaInformeCTAFReunion",
        data: frm,
        type: 'POST',
        processData: false,
        contentType: false,
        success: function (result) {
            if (result) {

                $("#btnSubirInformeReunion").show();
                $("#flInformeReunion").show();
                $("#btnVerInformeReunion").hide();
                $("#btnEliminarInformeReunion").hide();

                $("#lblInformeReunion").text("");

                alert('Se elimino correctamente');
            }
        }
    });

}

SubirInformeTecnicoAnual = function () {

    var file = $("#flInformeTecnicoAnual")[0].files[0];
    if (file != null) {

        var dtInformeTecnicoAnual = $("#dtInformeTecnicoAnual").val();

        if (dtInformeTecnicoAnual == "") {
            alert("Seleccione una fecha de Informe Tecnico");
            return false;
        }

        var dtFechaInforme = dtInformeTecnicoAnual;

        var controlador = siteRoot + 'Eventos/AnalisisFallas/';
        var frm = new FormData();
        frm.append("afecodi", $("#hfCodigo").val());
        frm.append("anio", $("#txtEventoAnioCtaf").val());
        frm.append("AFEITPITFFECHA", dtFechaInforme);
        frm.append("file", file);

        $.ajax({
            url: controlador + "ActualizarPublicacionInformeTecnicoAnualInformeTecnico",
            data: frm,
            type: 'POST',
            processData: false,
            contentType: false,
            success: function (result) {
                if (result) {
                    $("#flInformeTecnicoAnual").val("");

                    $("#btnVerInformeTecnicoAnual").show();
                    $("#btnEliminarInformeTecnicoAnual").show();

                    $("#lblInformeTecnicoAnual").text(obtenerFechayhora());

                    alert('Se cargo correctamente');
                }
            }
        });


    } else {
        alert('Seleccione un archivo');
    }
}

EliminarInformeTecnicoAnual = function () {

    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var frm = new FormData();
    frm.append("afecodi", $("#hfCodigo").val());
    frm.append("anio", $("#txtEventoAnioCtaf").val());

    $.ajax({
        url: controlador + "EliminarPublicacionInformeTecnicoAnualInformeTecnico",
        data: frm,
        type: 'POST',
        processData: false,
        contentType: false,
        success: function (result) {
            if (result) {

                $("#btnSubirInformeTecnicoAnual").show();
                $("#flInformeTecnicoAnual").show();
                $("#btnVerInformeTecnicoAnual").hide();
                $("#btnEliminarInformeTecnicoAnual").hide();

                $("#lblInformeTecnicoAnual").text("");

                alert('Se elimino correctamente');
            }
        }
    });

}

SubirDesicionEvento = function () {

    var file = $("#flDesicionEvento")[0].files[0];
    if (file != null) {

        var controlador = siteRoot + 'Eventos/AnalisisFallas/';
        var frm = new FormData();
        frm.append("afecodi", $("#hfCodigo").val());
        frm.append("anio", $("#txtEventoAnioCtaf").val());

        frm.append("file", file);

        $.ajax({
            url: controlador + "ActualizarPublicacionDesicionEventoInformeTecnico",
            data: frm,
            type: 'POST',
            processData: false,
            contentType: false,
            success: function (result) {
                if (result) {
                    $("#flDesicionEvento").val("");

                    $("#btnVerDesicionEvento").show();
                    $("#btnEliminarDesicionEvento").show();


                    $("#lblInformeDesicionEvento").text(obtenerFechayhora());

                    alert('Se cargo correctamente');
                }
            }
        });


    } else {
        alert('Seleccione un archivo');
    }
}

EliminarDesicionEvento = function () {

    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var frm = new FormData();
    frm.append("afecodi", $("#hfCodigo").val());
    frm.append("anio", $("#txtEventoAnioCtaf").val());

    $.ajax({
        url: controlador + "EliminarPublicacionDesicionEventoInformeTecnico",
        data: frm,
        type: 'POST',
        processData: false,
        contentType: false,
        success: function (result) {
            if (result) {

                $("#btnSubirDesicionEvento").show();
                $("#flDesicionEvento").show();
                $("#btnVerDesicionEvento").hide();
                $("#btnEliminarDesicionEvento").hide();

                $("#lblInformeDesicionEvento").text("");

                alert('Se elimino correctamente');
            }
        }
    });

}

VerPublicacionDesicionEventoInformeTecnico = function () {
    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var filename = "EV-DEC-" + $("#txtEventoAnioCtaf").val() + "-" + $("#hfCodigo").val() + ".pdf";
    window.open(controlador + 'verArchivo?filename=' + filename + '&anio=' + $("#txtEventoAnioCtaf").val() + '&id=' + $("#hfCodigo").val(), "_blank", 'fullscreen=yes');
}


VerInformeTecnicoAnual = function () {
    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var filename = "EV-ITF-" + $("#txtEventoAnioCtaf").val() + "-" + $("#hfCodigo").val() + ".pdf";
    window.open(controlador + 'verArchivo?filename=' + filename + '&anio=' + $("#txtEventoAnioCtaf").val() + '&id=' + $("#hfCodigo").val(), "_blank", 'fullscreen=yes');
}


VerCitacionReunion = function () {
    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var filename = "EV-COC-" + $("#txtEventoAnioCtaf").val() + "-" + $("#hfCodigo").val() + ".pdf";
    window.open(controlador + 'verArchivo?filename=' + filename + '&anio=' + $("#txtEventoAnioCtaf").val() + '&id=' + $("#hfCodigo").val(), "_blank", 'fullscreen=yes');
}

VerActaReunion = function () {
    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var filename = "EV-CTA-" + $("#txtEventoAnioCtaf").val() + "-" + $("#hfCodigo").val() + ".pdf";
    window.open(controlador + 'verArchivo?filename=' + filename + '&anio=' + $("#txtEventoAnioCtaf").val() + '&id=' + $("#hfCodigo").val(), "_blank", 'fullscreen=yes');
}

VerActaReunion = function () {
    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var filename = "EV-CTA-" + $("#txtEventoAnioCtaf").val() + "-" + $("#hfCodigo").val() + ".pdf";
    window.open(controlador + 'verArchivo?filename=' + filename + '&anio=' + $("#txtEventoAnioCtaf").val() + '&id=' + $("#hfCodigo").val(), "_blank", 'fullscreen=yes');
}

VerInformeReunion = function () {
    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var filename = "EV-CTI-" + $("#txtEventoAnioCtaf").val() + "-" + $("#hfCodigo").val() + ".pdf";
    window.open(controlador + 'verArchivo?filename=' + filename + '&anio=' + $("#txtEventoAnioCtaf").val() + '&id=' + $("#hfCodigo").val(), "_blank", 'fullscreen=yes');
}

VerInformeCompensacion = function () {
    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var filename = "EV-CMP-" + $("#txtEventoAnioCtaf").val() + "-" + $("#hfCodigo").val() + ".pdf";
    window.open(controlador + 'verArchivo?filename=' + filename + '&anio=' + $("#txtEventoAnioCtaf").val() + '&id=' + $("#hfCodigo").val(), "_blank", 'fullscreen=yes');
}

VerFuerzaMayor = function (empresacodi, reclamocodi) {
    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var filename = "EV-FMY-" + $("#txtEventoAnioCtaf").val() + "-" + $("#hfCodigo").val() + "-" + empresacodi + "-" + reclamocodi + ".pdf";
    window.open(controlador + 'verArchivo?filename=' + filename + '&anio=' + $("#txtEventoAnioCtaf").val() + '&id=' + $("#hfCodigo").val(), "_blank", 'fullscreen=yes');
}

VerFuerzaMayorCoes = function (empresacodi, rptacodi) {
    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var filename = "EV-FMC-" + $("#txtEventoAnioCtaf").val() + "-" + $("#hfCodigo").val() + "-" + empresacodi + "-" + rptacodi + ".pdf";
    window.open(controlador + 'verArchivo?filename=' + filename + '&anio=' + $("#txtEventoAnioCtaf").val() + '&id=' + $("#hfCodigo").val(), "_blank", 'fullscreen=yes');
}

VerEmpresaInvolucrada = function (empresaid, afiversion) {
    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var filename = "EV-IEI-" + $("#txtEventoAnioCtaf").val() + "-" + $("#hfCodigo").val() + "-" + empresaid + "-" + afiversion + ".pdf";
    window.open(controlador + 'verArchivo?filename=' + filename + '&anio=' + $("#txtEventoAnioCtaf").val() + '&id=' + $("#hfCodigo").val(), "_blank", 'fullscreen=yes');
}


VerReconsideracion = function (empresacodi, id) {
    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var filename = "EV-REE-" + $("#txtEventoAnioCtaf").val() + "-" + $("#hfCodigo").val() + "-" + empresacodi + "-" + id + ".pdf";
    window.open(controlador + 'verArchivo?filename=' + filename + '&anio=' + $("#txtEventoAnioCtaf").val() + '&id=' + $("#hfCodigo").val(), "_blank", 'fullscreen=yes');
}

VerReconsideracionCOES = function (empresacodi, id) {
    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var filename = "EV-REC-" + $("#txtEventoAnioCtaf").val() + "-" + $("#hfCodigo").val() + "-" + empresacodi + "-" + id + ".pdf";
    window.open(controlador + 'verArchivo?filename=' + filename + '&anio=' + $("#txtEventoAnioCtaf").val() + '&id=' + $("#hfCodigo").val(), "_blank", 'fullscreen=yes');
}



VerApelacion = function (empresacodi, id) {
    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var filename = "EV-APE-" + $("#txtEventoAnioCtaf").val() + "-" + $("#hfCodigo").val() + "-" + empresacodi + "-" + id + ".pdf";
    window.open(controlador + 'verArchivo?filename=' + filename + '&anio=' + $("#txtEventoAnioCtaf").val() + '&id=' + $("#hfCodigo").val(), "_blank", 'fullscreen=yes');
}

VerApelacionCOES = function (empresacodi, id) {
    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var filename = "EV-APC-" + $("#txtEventoAnioCtaf").val() + "-" + $("#hfCodigo").val() + "-" + empresacodi + "-" + id + ".pdf";
    window.open(controlador + 'verArchivo?filename=' + filename + '&anio=' + $("#txtEventoAnioCtaf").val() + '&id=' + $("#hfCodigo").val(), "_blank", 'fullscreen=yes');
}

VerArbitraje = function (empresacodi, id) {
    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var filename = "EV-ARE-" + $("#txtEventoAnioCtaf").val() + "-" + $("#hfCodigo").val() + "-" + empresacodi + "-" + id + ".pdf";
    window.open(controlador + 'verArchivo?filename=' + filename + '&anio=' + $("#txtEventoAnioCtaf").val() + '&id=' + $("#hfCodigo").val(), "_blank", 'fullscreen=yes');
}

VerArbitrajeCOES = function (empresacodi, id) {
    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var filename = "EV-ARC-" + $("#txtEventoAnioCtaf").val() + "-" + $("#hfCodigo").val() + "-" + empresacodi + "-" + id + ".pdf";
    window.open(controlador + 'verArchivo?filename=' + filename + '&anio=' + $("#txtEventoAnioCtaf").val() + '&id=' + $("#hfCodigo").val(), "_blank", 'fullscreen=yes');
}




SubirFuerzaMayor = function () {

    var file = $("#flFuerzaMayor")[0].files[0];
    if (file != null) {


        var CodigoEmpresa = $("#txtFuerzaMayorEmpresa").val();


        if (CodigoEmpresa == "") {
            alert('Por favor seleccione una empresa');
            return;
        }

        var dtFuerzaMayor = $("#dtFuerzaMayor").val();

        if (dtFuerzaMayor == "") {
            alert("Seleccione una fecha de Fuerza Mayor");
            return false;
        }

        var dtFecha = dtFuerzaMayor;

        var controlador = siteRoot + 'Eventos/AnalisisFallas/';
        var frm = new FormData();
        frm.append("AFECODI", $("#hfCodigo").val());
        frm.append("EMPRCODI", CodigoEmpresa);
        frm.append("ANIO", $("#txtEventoAnioCtaf").val());
        frm.append("AFREMPFECHA", dtFecha);
        frm.append("file", file);

        $.ajax({
            url: controlador + "InsertarFuerzaMayorInformeTecnico",
            data: frm,
            type: 'POST',
            processData: false,
            contentType: false,
            success: function (result) {
                if (result) {
                    $("#flFuerzaMayor").val("");
                    $("#dtFuerzaMayor").val("");
                    alert('Se cargo correctamente');
                    TraerFuerzaMayorInformeTecnico();
                }
            }
        });


    } else {
        alert('Seleccione un archivo');
    }
}

SubirFuerzaMayorCoes = function () {

    var file = $("#flFuerzaMayorDesicion")[0].files[0];
    if (file != null) {

        var RECLAMOCODI = "";
        var EMPRCODI = "";
        $("#tbodyFuerzaMayor tr").each(function () {
            var checked = $(this.cells[0]).find("input")[0].checked;
            if (checked) {
                RECLAMOCODI = $(this.cells[0]).find("input").attr("data-id");
                EMPRCODI = $(this.cells[0]).find("input").attr("data-empresa");
                return false;
            }

        });

        if (RECLAMOCODI == "" || EMPRCODI == "") {
            alert("Por favor seleccione un registro de Fuerza Mayor");
            return false;
        }

        var dtFuerzaMayor = $("#dtFuerzaMayor").val();

        if (dtFuerzaMayor == "") {
            alert("Seleccione una fecha de Fuerza Mayor");
            return false;
        }

        var dtFecha = dtFuerzaMayor;

        var controlador = siteRoot + 'Eventos/AnalisisFallas/';
        var frm = new FormData();
        frm.append("RECLAMOCODI", RECLAMOCODI);
        frm.append("AFECODI", $("#hfCodigo").val());
        frm.append("EMPRCODI", EMPRCODI);
        frm.append("ANIO", $("#txtEventoAnioCtaf").val());
        frm.append("FECHA_COES", dtFecha);
        frm.append("file", file);

        $.ajax({
            url: controlador + "ActualizarFuerzaMayorInformeTecnico",
            data: frm,
            type: 'POST',
            processData: false,
            contentType: false,
            success: function (result) {
                if (result) {
                    $("#flFuerzaMayorDesicion").val("");
                    $("#dtFuerzaMayor").val("");
                    alert('Se cargo correctamente');
                    TraerFuerzaMayorInformeTecnico();
                }
            }
        });


    } else {
        alert('Seleccione un archivo');
    }
}

TraerFuerzaMayorInformeTecnico = function () {
    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var id = $("#hfCodigo").val();

    $.ajax({
        type: 'POST',
        url: controlador + "TraerFuerzaMayorInformeTecnico/" + id,
        success: function (json) {


            var html = "";
            if (json != "") {
                var njson = json.length;

                for (var i = 0; i < njson; i++) {

                    html += "<tr ondblclick='CargarDatosFuerzaMayor(this," + json[i].RECLAMOCODI + ")' >";
                    html += "<td><input type='radio' name='rdFuerzaMayor' data-id='" + json[i].RECLAMOCODI + "' data-empresa='" + json[i].EMPRCODI + "' /></td>"
                    html += "<td><a href='#' onclick='VerFuerzaMayor(" + json[i].EMPRCODI + "," + json[i].RECLAMOCODI + ")' id='btnReunionEliminar'><img src='" + siteRoot + "Content/Images/btn-open.png' style='margin-top: 5px;'/></a></td>";
                    html += "<td><a href='#' onclick='EliminarFuerzaMayor(" + json[i].RECLAMOCODI + "," + json[i].EMPRCODI + "," + json[i].RPTACODI + ")' id='btnReunionEliminar'><img src='" + siteRoot + "Content/Images/btn-cancel.png' style='margin-top: 5px;'/></a></td>";
                    html += "<td>" + json[i].EMPRNOMB + "</td>";
                    html += "<td>" + json[i].FECHA + "</td>";
                    html += "<td>" + json[i].LASTUSER + "</td>";
                    html += "<td>" + json[i].LASTDATE + "</td>";
                    html += "<td><a href='#' onclick='VerFuerzaMayorCoes(" + json[i].EMPRCODI + "," + json[i].RPTACODI + ")' id='btnReunionEliminar'><img src='" + siteRoot + "Content/Images/btn-open.png' style='margin-top: 5px;'/></a></td>";
                    html += "<td><a href='#' onclick='EliminarFuerzaMayorCOES(" + json[i].RECLAMOCODI + "," + json[i].EMPRCODI + "," + json[i].RPTACODI + ")' id='btnReunionEliminar'><img src='" + siteRoot + "Content/Images/btn-cancel.png' style='margin-top: 5px;'/></a></td>";
                    html += "<td>" + json[i].FECHA_COES + "</td>";
                    html += "<td>" + json[i].LASTUSER_COES + "</td>";
                    html += "<td>" + json[i].LASTDATE_COES + "</td>";
                    html += "</tr>";

                }
                $("#tbodyFuerzaMayor").empty();
                $("#tbodyFuerzaMayor").append(html);

            } else {
                $("#tbodyFuerzaMayor").empty();
            }
        }
    });
}


EliminarFuerzaMayor = function (RECLAMOCODI, EMPRCODI, RPTACODI) {

    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var frm = new FormData();
    frm.append("afecodi", $("#hfCodigo").val());
    frm.append("RECLAMOCODI", RECLAMOCODI);
    frm.append("anio", $("#txtEventoAnioCtaf").val());
    frm.append("emprcodi", EMPRCODI);
    frm.append("RPTACODI", RPTACODI);

    $.ajax({
        url: controlador + "EliminarFuerzaMayorInformeTecnico",
        data: frm,
        type: 'POST',
        processData: false,
        contentType: false,
        success: function (result) {
            if (result) {

                alert('Se elimino correctamente');
                TraerFuerzaMayorInformeTecnico();
            }
        }
    });

}
EliminarFuerzaMayorCOES = function (RECLAMOCODI, EMPRCODI, RPTACODI) {

    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var frm = new FormData();
    frm.append("afecodi", $("#hfCodigo").val());
    frm.append("RECLAMOCODI", RECLAMOCODI);
    frm.append("anio", $("#txtEventoAnioCtaf").val());
    frm.append("emprcodi", EMPRCODI);
    frm.append("RPTACODI", RPTACODI);

    $.ajax({
        url: controlador + "EliminarrFuerzaMayorCOESInformeTecnico",
        data: frm,
        type: 'POST',
        processData: false,
        contentType: false,
        success: function (result) {
            if (result) {

                alert('Se elimino correctamente');
                TraerFuerzaMayorInformeTecnico();
            }
        }
    });

}
CargarDatosFuerzaMayor = function (obj, RECLAMOCODI) {
    alert(RECLAMOCODI);
    $(obj).attr("bgcolor", " #ffd800");
    $("#hfFuerzaMayorRECLAMOCODI").val(RECLAMOCODI);
}

AgregarEmpresaResponsableCompensacion = function () {


    var CodigoEmpresa = $("#txtCompensacionEmpresaResponsable").val();

    var txtCompensacionPorcentaje = $("#txtCompensacionPorcentaje").val();
    var txtCompensacionDecide = $("#txtCompensacionDecide").val();
    var txtCompensacionObservacion = $("#txtCompensacionObservacion").val();

    // Validar 
    var Mensaje = "";
    if (CodigoEmpresa == "") {
        Mensaje += "<li>Por favor busque una empresa</li>";
    }

    if (txtCompensacionPorcentaje == "") {
        Mensaje += "<li>Por favor ingrese un porcentaje</li>";
    }

    if (txtCompensacionDecide == "") {
        Mensaje += "<li>Por favor ingrese un valor para el campo Decide</li>";
    }

    if (txtCompensacionObservacion == "") {
        Mensaje += "<li>Por favor ingrese una observacion</li>";
    }

    if (Mensaje != "") {
        $("#ulMensaje").empty();
        $("#ulMensaje").append(Mensaje);

        $('#divMensaje').bPopup({
            follow: [true, true],
            position: ['auto', 'auto'],
            positionStyle: 'fixed',
        });
        return;
    }


    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var frm = new FormData();
    frm.append("afecodi", $("#hfCodigo").val());
    frm.append("EMPRCODI", CodigoEmpresa);
    frm.append("AFIVERSION", "R");
    frm.append("AFIPUBLICA", "N");
    frm.append("AFIPORCENTAJE", txtCompensacionPorcentaje);
    frm.append("AFIDECIDE", txtCompensacionDecide);
    frm.append("AFIMENSAJE", txtCompensacionObservacion);

    $.ajax({
        url: controlador + "InsertarEmpresaResponsableCompensacion",
        data: frm,
        type: 'POST',
        processData: false,
        contentType: false,
        success: function (result) {

            alert(result);
            $("#txtCompensacionEmpresaResponsable").val("");

            $("#txtCompensacionPorcentaje").val("");
            $("#txtCompensacionDecide").val("");
            $("#txtCompensacionObservacion").val("");
            TraerEmpresaResponsableCompensacion();

        }
    });

}


TraerEmpresaResponsableCompensacion = function () {
    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var id = $("#hfCodigo").val();

    $.ajax({
        type: 'POST',
        url: controlador + "TraerEmpresaResponsableCompensacion/" + id,
        success: function (json) {


            var html = "";
            if (json != "") {
                var njson = json.length;

                for (var i = 0; i < njson; i++) {

                    html += "<tr>";
                    html += "<td><a href='#' onclick='EliminarEmpresaResponsableCompensacion(" + json[i].EMPRCODI + "," + json[i].AFIDECIDE + ")'><img src='" + siteRoot + "Content/Images/btn-cancel.png' style='margin-top: 5px;'/></a></td>";
                    html += "<td>" + json[i].EMPRNOMB + "</td>";
                    html += "<td>" + json[i].AFIPORCENTAJE + "</td>";
                    html += "<td>" + json[i].AFIDECIDE + "</td>";
                    html += "<td>" + json[i].AFIMENSAJE + "</td>";
                    html += "<td>" + json[i].LASTUSER + "</td>";
                    html += "<td>" + json[i].LASTDATEstr + "</td>";
                    html += "</tr>";

                }
                $("#tbodyCompensacionEmpresaResponsable").empty();
                $("#tbodyCompensacionEmpresaResponsable").append(html);

            } else {
                $("#tbodyCompensacionEmpresaResponsable").empty();
            }
        }
    });
}

EliminarEmpresaResponsableCompensacion = function (EMPRCODI, AFIDECIDE) {

    if (confirm("Esta seguro que desea eliminar el registro?")) {
        var controlador = siteRoot + 'Eventos/AnalisisFallas/';
        var frm = new FormData();
        frm.append("afecodi", $("#hfCodigo").val());
        frm.append("EMPRCODI", EMPRCODI);
        frm.append("AFIVERSION", "R");
        frm.append("AFIDECIDE", AFIDECIDE);

        $.ajax({
            url: controlador + "EliminarEmpresaResponsableCompensacion",
            data: frm,
            type: 'POST',
            processData: false,
            contentType: false,
            success: function (result) {
                if (result) {
                    TraerEmpresaResponsableCompensacion();
                }
            }
        });
    }
}
// here we are 

AgregarEmpresaCompensadaCompensacion = function () {


    var CodigoEmpresa = $("#txtCompensacionEmpresaCompensada").val();

    // Validar 
    var Mensaje = "";
    if (CodigoEmpresa == "") {
        Mensaje += "<li>Por favor busque una empresa</li>";
    }

    if (Mensaje != "") {
        $("#ulMensaje").empty();
        $("#ulMensaje").append(Mensaje);

        $('#divMensaje').bPopup({
            follow: [true, true],
            position: ['auto', 'auto'],
            positionStyle: 'fixed',
        });
        return;
    }

    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var frm = new FormData();
    frm.append("afecodi", $("#hfCodigo").val());
    frm.append("EMPRCODI", CodigoEmpresa);
    frm.append("AFIVERSION", "C");
    frm.append("AFIPUBLICA", "N");

    $.ajax({
        url: controlador + "InsertarEmpresaCompensadaCompensacion",
        data: frm,
        type: 'POST',
        processData: false,
        contentType: false,
        success: function (result) {

            alert(result);
            $("#txtCompensacionEmpresaCompensada").val("");
            TraerEmpresaCompensadaCompensacion();

        }
    });

}


TraerEmpresaCompensadaCompensacion = function () {
    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var id = $("#hfCodigo").val();

    $.ajax({
        type: 'POST',
        url: controlador + "TraerEmpresaCompensadaCompensacion/" + id,
        success: function (json) {


            var html = "";
            if (json != "") {
                var njson = json.length;

                for (var i = 0; i < njson; i++) {

                    html += "<tr>";
                    html += "<td><a href='#' onclick='EliminarEmpresaCompensadaCompensacion(" + json[i].EMPRCODI + ")'><img src='" + siteRoot + "Content/Images/btn-cancel.png' style='margin-top: 5px;'/></a></td>";
                    html += "<td>" + json[i].EMPRNOMB + "</td>";
                    html += "<td>" + json[i].LASTUSER + "</td>";
                    html += "<td>" + json[i].LASTDATEstr + "</td>";
                    html += "</tr>";

                }
                $("#tbodyCompensacionEmpresaCompensada").empty();
                $("#tbodyCompensacionEmpresaCompensada").append(html);

            } else {
                $("#tbodyCompensacionEmpresaCompensada").empty();
            }
        }
    });
}

EliminarEmpresaCompensadaCompensacion = function (EMPRCODI) {

    if (confirm("Esta seguro que desea eliminar el registro?")) {
        var controlador = siteRoot + 'Eventos/AnalisisFallas/';
        var frm = new FormData();
        frm.append("afecodi", $("#hfCodigo").val());
        frm.append("EMPRCODI", EMPRCODI);
        frm.append("AFIVERSION", "C");

        $.ajax({
            url: controlador + "EliminarEmpresaCompensadaCompensacion",
            data: frm,
            type: 'POST',
            processData: false,
            contentType: false,
            success: function (result) {
                if (result) {
                    TraerEmpresaCompensadaCompensacion();
                }
            }
        });
    }
}

SubirInformeCompensacion = function () {
    var file = $("#flInformeCompensacion")[0].files[0];
    var dtFechaInformeCompensacion = $("#dtFechaInformeCompensacion").val();
    if (dtFechaInformeCompensacion == "") {
        alert("Seleccione una fecha de Informe");
        return;
    }

    if (file != null) {

        var dtFechaInforme = dtFechaInformeCompensacion;

        var controlador = siteRoot + 'Eventos/AnalisisFallas/';
        var frm = new FormData();
        frm.append("afecodi", $("#hfCodigo").val());
        frm.append("anio", $("#txtEventoAnioCtaf").val());
        frm.append("AFECOMPFECHA", dtFechaInforme);
        frm.append("file", file);

        $.ajax({
            url: controlador + "ActualizarInformeCompensaciones",
            data: frm,
            type: 'POST',
            processData: false,
            contentType: false,
            success: function (result) {
                if (result) {
                    $("#flInformeCompensacion").val("");

                    //$("#btnSubirInformeCompensacion").hide();
                    //$("#flInformeCompensacion").hide();
                    $("#btnVerInformeCompensacion").show();
                    $("#btnEliminarInformeCompensacion").show();

                    $("#lblInformeCompensacion").text(obtenerFechayhora());

                    alert('Se cargo correctamente');
                }
            }
        });


    } else {
        alert('Seleccione un archivo');
    }
}

EliminarInformeCompensacion = function () {

    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var frm = new FormData();
    frm.append("afecodi", $("#hfCodigo").val());
    frm.append("anio", $("#txtEventoAnioCtaf").val());

    $.ajax({
        url: controlador + "EliminarInformeCompensaciones",
        data: frm,
        type: 'POST',
        processData: false,
        contentType: false,
        success: function (result) {
            if (result) {

                $("#btnSubirInformeCompensacion").show();
                $("#flInformeCompensacion").show();
                $("#btnVerInformeCompensacion").hide();
                $("#btnEliminarInformeCompensacion").hide();

                $("#lblInformeCompensacion").text("");

                alert('Se elimino correctamente');
            }
        }
    });
}

SubirReconsideracion = function () {
    var file = $("#flReconsideracion")[0].files[0];
    if (file != null) {

        var CodigoEmpresa = $("#txtEmpresaReconsideracion").val();

        if (CodigoEmpresa == "") {
            alert('Por favor seleccione una empresa');
            return;
        }

        var dtReconsideracion = $("#dtReconsideracion").val();

        if (dtReconsideracion == "") {
            alert("Seleccione una fecha de Reconsideracion");
            return false;
        }

        var dtFecha = dtReconsideracion;

        var controlador = siteRoot + 'Eventos/AnalisisFallas/';
        var frm = new FormData();
        frm.append("AFECODI", $("#hfCodigo").val());
        frm.append("EMPRCODI", CodigoEmpresa);
        frm.append("ANIO", $("#txtEventoAnioCtaf").val());
        frm.append("AFREMPFECHA", dtFecha);
        frm.append("file", file);

        $.ajax({
            url: controlador + "InsertarReconsideracion",
            data: frm,
            type: 'POST',
            processData: false,
            contentType: false,
            success: function (result) {
                if (result) {

                    $("#txtEmpresaReconsideracion").val("");
                    $("#dtReconsideracion").val("");
                    $("#flReconsideracion").val("");
                    alert('Se cargo correctamente');
                    TraerReconsideracion();
                }
            }
        });


    } else {
        alert('Seleccione un archivo');
    }
}

SubirReconsideracionCoes = function () {

    var file = $("#flReconsideracionCoes")[0].files[0];
    if (file != null) {

        var RECLAMOCODI = "";
        var EMPRCODI = "";
        $("#tbodyReconsideracionReclamoReconsideracion tr").each(function () {
            var checked = $(this.cells[0]).find("input")[0].checked;
            if (checked) {
                RECLAMOCODI = $(this.cells[0]).find("input").attr("data-id");
                EMPRCODI = $(this.cells[0]).find("input").attr("data-empresa");
                return false;
            }

        });

        if (RECLAMOCODI == "" || EMPRCODI == "") {
            alert("Por favor seleccione un registro de Reconsideracion");
            return false;
        }

        var dtFuerzaMayor = $("#dtReconsideracionCoes").val();

        if (dtFuerzaMayor == "") {
            alert("Seleccione una fecha Coes");
            return false;
        }

        var dtFecha = dtFuerzaMayor;

        var controlador = siteRoot + 'Eventos/AnalisisFallas/';
        var frm = new FormData();
        frm.append("RECLAMOCODI", RECLAMOCODI);
        frm.append("AFECODI", $("#hfCodigo").val());
        frm.append("EMPRCODI", EMPRCODI);
        frm.append("ANIO", $("#txtEventoAnioCtaf").val());
        frm.append("FECHA_COES", dtFecha);
        frm.append("file", file);

        $.ajax({
            url: controlador + "ActualizarReconsideracionCOES",
            data: frm,
            type: 'POST',
            processData: false,
            contentType: false,
            success: function (data) {
                if (data.result == 1) {
                    $("#flReconsideracionCoes").val("");
                    $("#dtReconsideracionCoes").val("");
                    alert('Se cargo correctamente');
                    TraerReconsideracion();
                } else {
                    alert(data.responseText);
                }
            }
        });


    } else {
        alert('Seleccione un archivo');
    }
}

EliminarReconsideracionCOES = function (RECLAMOCODI, EMPRCODI, RPTACODI) {

    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var frm = new FormData();
    frm.append("afecodi", $("#hfCodigo").val());
    frm.append("reclamocodi", RECLAMOCODI);
    frm.append("anio", $("#txtEventoAnioCtaf").val());
    frm.append("emprcodi", EMPRCODI);
    frm.append("rptacodi", RPTACODI);

    $.ajax({
        url: controlador + "EliminarReconsideracionCOES",
        data: frm,
        type: 'POST',
        processData: false,
        contentType: false,
        success: function (result) {
            if (result) {

                alert('Se elimino correctamente');
                TraerReconsideracion();
            }
        }
    });

}

SubirApelacion = function () {
    var file = $("#flApelacion")[0].files[0];
    if (file != null) {

        var CodigoEmpresa = $("#txtEmpresaApelacion").val();

        if (CodigoEmpresa == "") {
            alert('Por favor seleccione una empresa');
            return;
        }

        var dtApelacion = $("#dtApelacion").val();

        if (dtApelacion == "") {
            alert("Seleccione una fecha de Apelacion");
            return false;
        }

        var dtFecha = dtApelacion;

        var controlador = siteRoot + 'Eventos/AnalisisFallas/';
        var frm = new FormData();
        frm.append("AFECODI", $("#hfCodigo").val());
        frm.append("EMPRCODI", CodigoEmpresa);
        frm.append("ANIO", $("#txtEventoAnioCtaf").val());
        frm.append("AFREMPFECHA", dtFecha);
        frm.append("file", file);

        $.ajax({
            url: controlador + "InsertarApelacion",
            data: frm,
            type: 'POST',
            processData: false,
            contentType: false,
            success: function (result) {
                if (result) {
                    $("#txtEmpresaApelacion").val("");
                    $("#dtApelacion").val("");
                    $("#flApelacion").val("");
                    alert('Se cargo correctamente');
                    TraerApelacion();
                }
            }
        });


    } else {
        alert('Seleccione un archivo');
    }
}

SubirApelacionCoes = function () {

    var file = $("#flApelacionCoes")[0].files[0];
    if (file != null) {

        var RECLAMOCODI = "";
        var EMPRCODI = "";
        $("#tbodyReconsideracionReclamoApelacion tr").each(function () {
            var checked = $(this.cells[0]).find("input")[0].checked;
            if (checked) {
                RECLAMOCODI = $(this.cells[0]).find("input").attr("data-id");
                EMPRCODI = $(this.cells[0]).find("input").attr("data-empresa");
                return false;
            }

        });

        if (RECLAMOCODI == "" || EMPRCODI == "") {
            alert("Por favor seleccione un registro de Apelacion");
            return false;
        }

        var dtFuerzaMayor = $("#dtApelacionCoes").val();

        if (dtFuerzaMayor == "") {
            alert("Seleccione una fecha Coes");
            return false;
        }

        var dtFecha = dtFuerzaMayor;

        var controlador = siteRoot + 'Eventos/AnalisisFallas/';
        var frm = new FormData();
        frm.append("RECLAMOCODI", RECLAMOCODI);
        frm.append("AFECODI", $("#hfCodigo").val());
        frm.append("EMPRCODI", EMPRCODI);
        frm.append("ANIO", $("#txtEventoAnioCtaf").val());
        frm.append("FECHA_COES", dtFecha);
        frm.append("file", file);

        $.ajax({
            url: controlador + "ActualizarApelacionCOES",
            data: frm,
            type: 'POST',
            processData: false,
            contentType: false,
            success: function (data) {
                if (data.result == 1) {
                    $("#flApelacionCoes").val("");
                    $("#dtApelacionCoes").val("");
                    alert('Se cargo correctamente');
                    TraerApelacion();
                } else {
                    alert(data.responseText);
                }
            }
        });


    } else {
        alert('Seleccione un archivo');
    }
}

EliminarApelacionCOES = function (RECLAMOCODI, EMPRCODI, RPTACODI) {

    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var frm = new FormData();
    frm.append("afecodi", $("#hfCodigo").val());
    frm.append("reclamocodi", RECLAMOCODI);
    frm.append("anio", $("#txtEventoAnioCtaf").val());
    frm.append("emprcodi", EMPRCODI);
    frm.append("rptacodi", RPTACODI);

    $.ajax({
        url: controlador + "EliminarApelacionCOES",
        data: frm,
        type: 'POST',
        processData: false,
        contentType: false,
        success: function (result) {
            if (result) {

                alert('Se elimino correctamente');
                TraerApelacion();
            }
        }
    });

}


SubirArbitraje = function () {
    var file = $("#flArbitraje")[0].files[0];
    if (file != null) {

        var CodigoEmpresa = $("#txtEmpresaArbitraje").val();

        if (CodigoEmpresa == "") {
            alert('Por favor seleccione una empresa');
            return;
        }

        var dtArbitraje = $("#dtArbitraje").val();

        if (dtArbitraje == "") {
            alert("Seleccione una fecha de Arbitraje");
            return false;
        }

        var dtFecha = dtArbitraje;

        var controlador = siteRoot + 'Eventos/AnalisisFallas/';
        var frm = new FormData();
        frm.append("AFECODI", $("#hfCodigo").val());
        frm.append("EMPRCODI", CodigoEmpresa);
        frm.append("ANIO", $("#txtEventoAnioCtaf").val());
        frm.append("AFREMPFECHA", dtFecha);
        frm.append("file", file);

        $.ajax({
            url: controlador + "InsertarArbitraje",
            data: frm,
            type: 'POST',
            processData: false,
            contentType: false,
            success: function (result) {
                if (result) {
                    $("#txtEmpresaArbitraje").val("");
                    $("#dtArbitraje").val("");
                    $("#flArbitraje").val("");
                    alert('Se cargo correctamente');
                    TraerArbitraje();
                }
            }
        });


    } else {
        alert('Seleccione un archivo');
    }
}

SubirArbitrajeCoes = function () {

    var file = $("#flArbitrajeCoes")[0].files[0];
    if (file != null) {

        var RECLAMOCODI = "";
        var EMPRCODI = "";
        $("#tbodyArbitraje tr").each(function () {
            var checked = $(this.cells[0]).find("input")[0].checked;
            if (checked) {
                RECLAMOCODI = $(this.cells[0]).find("input").attr("data-id");
                EMPRCODI = $(this.cells[0]).find("input").attr("data-empresa");
                return false;
            }

        });

        if (RECLAMOCODI == "" || EMPRCODI == "") {
            alert("Por favor seleccione un registro de Arbitraje");
            return false;
        }

        var dtFuerzaMayor = $("#dtArbitrajeCoes").val();

        if (dtFuerzaMayor == "") {
            alert("Seleccione una fecha Coes");
            return false;
        }

        var dtFecha = dtFuerzaMayor;

        var controlador = siteRoot + 'Eventos/AnalisisFallas/';
        var frm = new FormData();
        frm.append("RECLAMOCODI", RECLAMOCODI);
        frm.append("AFECODI", $("#hfCodigo").val());
        frm.append("EMPRCODI", EMPRCODI);
        frm.append("ANIO", $("#txtEventoAnioCtaf").val());
        frm.append("FECHA_COES", dtFecha);
        frm.append("file", file);

        $.ajax({
            url: controlador + "ActualizarArbitrajeCOES",
            data: frm,
            type: 'POST',
            processData: false,
            contentType: false,
            success: function (data) {
                if (data.result == 1) {
                    $("#flArbitrajeCoes").val("");
                    $("#dtArbitrajeCoes").val("");
                    alert('Se cargo correctamente');
                    TraerArbitraje();
                } else {
                    alert(data.responseText);
                }
            }
        });


    } else {
        alert('Seleccione un archivo');
    }
}

EliminarArbitrajeCOES = function (RECLAMOCODI, EMPRCODI, RPTACODI) {

    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var frm = new FormData();
    frm.append("afecodi", $("#hfCodigo").val());
    frm.append("reclamocodi", RECLAMOCODI);
    frm.append("anio", $("#txtEventoAnioCtaf").val());
    frm.append("emprcodi", EMPRCODI);
    frm.append("rptacodi", RPTACODI);

    $.ajax({
        url: controlador + "EliminarArbitrajeCOES",
        data: frm,
        type: 'POST',
        processData: false,
        contentType: false,
        success: function (result) {
            if (result) {
                alert('Se elimino correctamente');
                TraerArbitraje();
            }
        }
    });

}

TraerReconsideracion = function () {
    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var id = $("#hfCodigo").val();

    $.ajax({
        type: 'POST',
        url: controlador + "TraerReclamoReconsideracionReconsideracion/" + id,
        success: function (json) {


            var html = "";
            if (json != "") {
                var njson = json.length;

                for (var i = 0; i < njson; i++) {

                    html += "<tr>";
                    html += "<td><input type='radio' name='rdFuerzaMayor' data-id='" + json[i].RECLAMOCODI + "' data-empresa='" + json[i].EMPRCODI + "' /></td>"
                    html += "<td><a href='#' onclick='VerReconsideracion(" + json[i].EMPRCODI + "," + json[i].RECLAMOCODI + ")'><img src='" + siteRoot + "Content/Images/btn-open.png' style='margin-top: 5px;'/></a></td>";
                    html += "<td><a href='#' onclick='EliminarReconsideracion(" + json[i].RECLAMOCODI + "," + json[i].EMPRCODI + "," + json[i].RPTACODI + ")'><img src='" + siteRoot + "Content/Images/btn-cancel.png' style='margin-top: 5px;'/></a></td>";
                    html += "<td>" + json[i].EMPRNOMB + "</td>";
                    html += "<td>" + json[i].FECHA + "</td>";
                    html += "<td>" + json[i].LASTUSER + "</td>";
                    html += "<td>" + json[i].LASTDATE + "</td>";
                    if (json[i].RPTACODI != -1) {
                        html += "<td><a href='#' onclick='VerReconsideracionCOES(" + json[i].EMPRCODI + "," + json[i].RPTACODI + ")'><img src='" + siteRoot + "Content/Images/btn-open.png' style='margin-top: 5px;'/></a></td>";
                        html += "<td><a href='#' onclick='EliminarReconsideracionCOES(" + json[i].RECLAMOCODI + "," + json[i].EMPRCODI + "," + json[i].RPTACODI + ")'><img src='" + siteRoot + "Content/Images/btn-cancel.png' style='margin-top: 5px;'/></a></td>";
                        html += "<td>" + json[i].FECHA_COES + "</td>";
                        html += "<td>" + json[i].LASTUSER_COES + "</td>";
                        html += "<td>" + json[i].LASTDATE_COES + "</td>";
                    } else {
                        html += "<td></td><td></td><td></td><td></td><td></td>";
                    }
                    html += "</tr>";

                }
                $("#tbodyReconsideracionReclamoReconsideracion").empty();
                $("#tbodyReconsideracionReclamoReconsideracion").append(html);

            } else {
                $("#tbodyReconsideracionReclamoReconsideracion").empty();
            }
        }
    });
}

TraerApelacion = function () {
    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var id = $("#hfCodigo").val();

    $.ajax({
        type: 'POST',
        url: controlador + "TraerReclamoApelacionReconsideracion/" + id,
        success: function (json) {


            var html = "";
            if (json != "") {
                var njson = json.length;

                for (var i = 0; i < njson; i++) {

                    html += "<tr>";
                    html += "<td><input type='radio' name='rdFuerzaMayor' data-id='" + json[i].RECLAMOCODI + "' data-empresa='" + json[i].EMPRCODI + "' /></td>"
                    html += "<td><a href='#' onclick='VerApelacion(" + json[i].EMPRCODI + "," + json[i].RECLAMOCODI + ")'><img src='" + siteRoot + "Content/Images/btn-open.png' style='margin-top: 5px;'/></a></td>";
                    html += "<td><a href='#' onclick='EliminarApelacion(" + json[i].RECLAMOCODI + "," + json[i].EMPRCODI + "," + json[i].RPTACODI + ")'><img src='" + siteRoot + "Content/Images/btn-cancel.png' style='margin-top: 5px;'/></a></td>";
                    html += "<td>" + json[i].EMPRNOMB + "</td>";
                    html += "<td>" + json[i].FECHA + "</td>";
                    html += "<td>" + json[i].LASTUSER + "</td>";
                    html += "<td>" + json[i].LASTDATE + "</td>";
                    if (json[i].RPTACODI != -1) {
                        html += "<td><a href='#' onclick='VerApelacionCOES(" + json[i].EMPRCODI + "," + json[i].RPTACODI + ")'><img src='" + siteRoot + "Content/Images/btn-open.png' style='margin-top: 5px;'/></a></td>";
                        html += "<td><a href='#' onclick='EliminarApelacionCOES(" + json[i].RECLAMOCODI + "," + json[i].EMPRCODI + "," + json[i].RPTACODI + ")'><img src='" + siteRoot + "Content/Images/btn-cancel.png' style='margin-top: 5px;'/></a></td>";
                        html += "<td>" + json[i].FECHA_COES + "</td>";
                        html += "<td>" + json[i].LASTUSER_COES + "</td>";
                        html += "<td>" + json[i].LASTDATE_COES + "</td>";
                    } else {
                        html += "<td></td><td></td><td></td><td></td><td></td>";
                    }
                    html += "</tr>";

                }
                $("#tbodyReconsideracionReclamoApelacion").empty();
                $("#tbodyReconsideracionReclamoApelacion").append(html);

            } else {
                $("#tbodyReconsideracionReclamoApelacion").empty();
            }
        }
    });
}

TraerArbitraje = function () {
    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var id = $("#hfCodigo").val();

    $.ajax({
        type: 'POST',
        url: controlador + "TraerReclamoArbitrajeReconsideracion/" + id,
        success: function (json) {


            var html = "";
            if (json != "") {
                var njson = json.length;

                for (var i = 0; i < njson; i++) {

                    html += "<tr>";
                    html += "<td><input type='radio' name='rdFuerzaMayor' data-id='" + json[i].RECLAMOCODI + "' data-empresa='" + json[i].EMPRCODI + "' /></td>"
                    html += "<td><a href='#' onclick='VerArbitraje(" + json[i].EMPRCODI + "," + json[i].RECLAMOCODI + ")'><img src='" + siteRoot + "Content/Images/btn-open.png' style='margin-top: 5px;'/></a></td>";
                    html += "<td><a href='#' onclick='EliminarArbitraje(" + json[i].RECLAMOCODI + "," + json[i].EMPRCODI + "," + json[i].RPTACODI + ")'><img src='" + siteRoot + "Content/Images/btn-cancel.png' style='margin-top: 5px;'/></a></td>";
                    html += "<td>" + json[i].EMPRNOMB + "</td>";
                    html += "<td>" + json[i].FECHA + "</td>";
                    html += "<td>" + json[i].LASTUSER + "</td>";
                    html += "<td>" + json[i].LASTDATE + "</td>";
                    if (json[i].RPTACODI != -1) {
                        html += "<td><a href='#' onclick='VerArbitrajeCOES(" + json[i].EMPRCODI + "," + json[i].RPTACODI + ")'><img src='" + siteRoot + "Content/Images/btn-open.png' style='margin-top: 5px;'/></a></td>";
                        html += "<td><a href='#' onclick='EliminarArbitrajeCOES(" + json[i].RECLAMOCODI + "," + json[i].EMPRCODI + "," + json[i].RPTACODI + ")'><img src='" + siteRoot + "Content/Images/btn-cancel.png' style='margin-top: 5px;'/></a></td>";
                        html += "<td>" + json[i].FECHA_COES + "</td>";
                        html += "<td>" + json[i].LASTUSER_COES + "</td>";
                        html += "<td>" + json[i].LASTDATE_COES + "</td>";
                    } else {
                        html += "<td></td><td></td><td></td><td></td><td></td>";
                    }
                    html += "</tr>";

                }
                $("#tbodyArbitraje").empty();
                $("#tbodyArbitraje").append(html);

            } else {
                $("#tbodyArbitraje").empty();
            }
        }
    });
}

EliminarReconsideracion = function (RECLAMOCODI, EMPRCODI, RPTACODI) {
    var controlador = siteRoot + 'Eventos/AnalisisFallas/';

    var frm = new FormData();
    frm.append("afecodi", $("#hfCodigo").val());
    frm.append("anio", $("#txtEventoAnioCtaf").val());
    frm.append("reclamocodi", RECLAMOCODI);
    frm.append("emprcodi", EMPRCODI);
    frm.append("rptacodi", RPTACODI);

    $.ajax({
        url: controlador + "EliminarReconsideracion",
        data: frm,
        type: 'POST',
        processData: false,
        contentType: false,
        success: function (result) {
            if (result) {
                alert('Se elimino correctamente');
                TraerReconsideracion();
            }
        }
    });
}

EliminarApelacion = function (RECLAMOCODI, EMPRCODI, RPTACODI) {
    var controlador = siteRoot + 'Eventos/AnalisisFallas/';

    var frm = new FormData();
    frm.append("afecodi", $("#hfCodigo").val());
    frm.append("anio", $("#txtEventoAnioCtaf").val());
    frm.append("reclamocodi", RECLAMOCODI);
    frm.append("emprcodi", EMPRCODI);
    frm.append("rptacodi", RPTACODI);

    $.ajax({
        url: controlador + "EliminarApelacion",
        data: frm,
        type: 'POST',
        processData: false,
        contentType: false,
        success: function (result) {
            if (result) {
                alert('Se elimino correctamente');
                TraerApelacion();
            }
        }
    });
}

EliminarArbitraje = function (RECLAMOCODI, EMPRCODI, RPTACODI) {
    var controlador = siteRoot + 'Eventos/AnalisisFallas/';

    var frm = new FormData();
    frm.append("afecodi", $("#hfCodigo").val());
    frm.append("anio", $("#txtEventoAnioCtaf").val());
    frm.append("reclamocodi", RECLAMOCODI);
    frm.append("emprcodi", EMPRCODI);
    frm.append("rptacodi", RPTACODI);

    $.ajax({
        url: controlador + "EliminarArbitraje",
        data: frm,
        type: 'POST',
        processData: false,
        contentType: false,
        success: function (result) {
            if (result) {
                alert('Se elimino correctamente');
                TraerArbitraje();
            }
        }
    });
}



function DescargarArtworkFile(obj) {
    var filename = $(obj).attr("data-file");
    var OriginalFileName = $(obj).attr("data-fileo");

    var url = urlBase() + "GestionProducto/Estilo/Descargar?filename=" + filename + "&originalfilename=" + OriginalFileName;

    var link = document.createElement("a");
    link.href = url;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
    delete link;
}

EnviarCorreo = function () {

    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var frm = new FormData();
    frm.append("codigoCTAF", $("#hfCodigo").val());

    $.ajax({
        url: controlador + "EnviarCorreo",
        data: frm,
        type: 'POST',
        processData: false,
        contentType: false,
        beforeSend: function () {
            $(".modal-loading-background").css("visibility", "visible");
        },
        success: function (result) {
            alert(result.responseText);
        },
        complete: function () {
            $(".modal-loading-background").css("visibility", "hidden");
        }
    });

}

EnviarCorreoActa = function () {
    let fechaLimiteEnvio = $("#dtFechaLimite").val();
    let diaFechaLimite = fechaLimiteEnvio.substr(0, 2);
    let mesFechaLimite = fechaLimiteEnvio.substr(3, 2);
    let anioFechaLimite = fechaLimiteEnvio.substr(6, 4);

    let fechaActual = new Date().toJSON().slice(0, 10).replace(/-/g,'');
    const fechaLimite = anioFechaLimite + mesFechaLimite + diaFechaLimite;

    //TODO: Verificacion de la fecha limite
    //if (fechaLimite < fechaActual) {
    //    alert('No puede enviar correos después de la Fecha Límite');
    //    return;
    //}
    
    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var frm = new FormData();
    frm.append("codigoCTAF", $("#hfCodigo").val());

    $.ajax({
        url: controlador + "EnviarCorreoActa",
        data: frm,
        type: 'POST',
        processData: false,
        contentType: false,
        //beforeSend: function () {
        //    $(".modal-loading-background").css("visibility", "visible");
        //},
        success: function (result) {
            alert(result.responseText);
        }//,
        //complete: function () {
        //    $(".modal-loading-background").css("visibility", "hidden");
        //}
    });

}

ActualizarEvento = function () {
    //$('#loadingcarga').css('display', 'inline');
    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var AFECODI = $("#hfCodigo").val();
    var _AFECODISCO = 0;
    var _EVENCODI = 0;
    var objeto = [];

    var inputsInformes = document.querySelectorAll('.chPortal');
    var data = hot.getData();

    for (var k = 0; k < inputsInformes.length; k++) {

        var parametros = inputsInformes[k].value.split('|');

        if (inputsInformes[k].checked == true) {

            for (var z = 1; z < data.length; z++) {

                var parametrosPortal = data[z][0].split('|');

                if (parametrosPortal[3] == parametros[0]) {
                    objeto.push({ Eveninfcodi: parametros[0], PortalWeb: 'S', Eveinfcodigo: data[z][6] });
                }

            }

        }
        else {

            for (var z = 1; z < data.length; z++) {

                var parametrosPortal = data[z][0].split('|');

                if (parametrosPortal[3] == parametros[0]) {
                    objeto.push({ Eveninfcodi: parametros[0], PortalWeb: 'N', Eveinfcodigo: data[z][6] });
                }

            }

        }
    }

    var inputsInformesCtaf = document.querySelectorAll('.chCtaf');
    for (var k = 0; k < inputsInformesCtaf.length; k++) {
        if (inputsInformesCtaf[k].checked == true) {
            var parametros = inputsInformesCtaf[k].value.split('|');
            _EVENCODI = parametros[0];
            _AFECODISCO = parametros[1];
        }
    }

    var objRcmAO = [];
    var inputsAO = document.querySelectorAll('.chEventoAO');
    for (var j = 0; j < inputsAO.length; j++) {
        if (inputsAO[j].checked == true)
            objRcmAO.push({ AFRREC: inputsAO[j].value, EVENRCMCTAF: 'S' });
        else
            objRcmAO.push({ AFRREC: inputsAO[j].value, EVENRCMCTAF: 'N' });
    }

    if (_AFECODISCO == 0) {
        alert("Debe seleccionar un evento Sco");
        return false;
    }
    
    var AFECORR = parseInt($("#txtAFECORRCtaf").val());
    var AFECORRORIGINAL = parseInt($("#hfAFECORR").val());

    var AFERMC = "N";
    var chkAFERMC = document.getElementById("chkAFERMC").checked;
    if (chkAFERMC) {
        AFERMC = 'S'
    }

    var AFEERACMF = "N";
    var chkAFEERACMF = document.getElementById("chkAFEERACMF").checked;
    if (chkAFEERACMF) {
        AFEERACMF = 'S'
    }

    var AFERACMT = "N";
    var chkAFERACMT = document.getElementById("chkAFERACMT").checked;
    if (chkAFERACMT) {
        AFERACMT = 'S'
    }

    var AFEEDAGSF = "N";
    var chkAFEEDAGSF = document.getElementById("chkAFEEDAGSF").checked;
    if (chkAFEEDAGSF) {
        AFEEDAGSF = 'S'
    }

    var AFERESPONSABLE = $("#cboEventoResponsable").val();

    var AFECITFECHAELABU = "";
    var dtCitacionElaboracion = $("#dtCitacionElaboracion").val();
    if (dtCitacionElaboracion == "") {
        alert("Seleccione una fecha Citacion - Elaboracion");
        return false;
    }

    AFECITFECHAELABU = dtCitacionElaboracion;

    var AFEREUFECHAPROGU = "";
    var dtReunionProgramada = $("#dtReunionProgramada").val();
    if (dtReunionProgramada == "") {
        alert("Seleccione una fecha Reunion - Programada");
        return false;
    }
    AFEREUFECHAPROGU = dtReunionProgramada;

    var AFEREUHORAPROG = $("#dtReunionHora").val();

    var AFECONVTIPOREUNION = "N";
    var cboReunionTipoReunion = $("#cboReunionTipoReunion").val();
    var AFSALA = $("#txtSala").val();
    if (cboReunionTipoReunion == "PRESENCIAL") {
        AFECONVTIPOREUNION = "P";
        if (AFSALA == "") {
            alert("Ingrese el nro. de Sala.");
            return false;
        }
        else if (IsOnlyNumber(AFSALA) == false) {
            alert("El campo Sala debe ser numérico.");
            return false;
        }
    }

    var AFERCTAEOBSERVACION = $("#txtAFERCTAEOBSERVACION").val();

    var AFEITFECHAELABU = "";
    var dtAFEITFECHAELAB = $("#dtAFEITFECHAELAB").val();
    if (dtAFEITFECHAELAB == "") {
        alert("Seleccione una fecha de Elaboracion en Informe Tecnico");
        return false;
    }
    AFEITFECHAELABU = dtAFEITFECHAELAB;

    var AFEITRDJRFECHAENVIOU = "";
    var dtAFEITRDJRFECHAENVIO = $("#dtAFEITRDJRFECHAENVIO").val();
    if (dtAFEITRDJRFECHAENVIO == "") {
        alert("Seleccione una fecha de Revision DJR-Envio");
        return false;
    }
    AFEITRDJRFECHAENVIOU = dtAFEITRDJRFECHAENVIO;

    var AFEITRDJRFECHARECEPU = "";
    var dtAFEITRDJRFECHARECEP = $("#dtAFEITRDJRFECHARECEP").val();
    if (dtAFEITRDJRFECHARECEP == "") {
        alert("Seleccione una fecha de Revision DJR-Recepcion");
        return false;
    }
    AFEITRDJRFECHARECEPU = dtAFEITRDJRFECHARECEP;

    var AFEITRDOFECHAENVIOU = "";
    var dtAFEITRDOFECHAENVIO = $("#dtAFEITRDOFECHAENVIO").val();
    if (dtAFEITRDOFECHAENVIO == "") {
        alert("Seleccione una fecha de Revision DO-Envio");
        return false;
    }
    AFEITRDOFECHAENVIOU = dtAFEITRDOFECHAENVIO;

    var AFEITRDOFECHARECEPU = "";
    var dtAFEITRDOFECHARECEP = $("#dtAFEITRDOFECHARECEP").val();
    if (dtAFEITRDOFECHARECEP == "") {
        alert("Seleccione una fecha de Revision DO-Recepcion");
        return false;
    }
    AFEITRDOFECHARECEPU = dtAFEITRDOFECHARECEP;

    var AFEITRDJRESTADO = "N"; // Oculto en el formulario
    var AFEITRDOESTADO = "N"; // Oculto en el formulario

    var AFEIMPUGNA = "N";
    var chkInformeTecnicoImpugnacion = document.getElementById("chkInformeTecnicoImpugnacion").checked;
    if (chkInformeTecnicoImpugnacion) {
        AFEIMPUGNA = "S";
    }

    var AFEFZAMAYOR = "N";
    var chkInformeTecnicoFuerzaMayor = document.getElementById("chkInformeTecnicoFuerzaMayor").checked;
    if (chkInformeTecnicoFuerzaMayor) {
        AFEFZAMAYOR = "S";
    }

    var AFEITDECFECHANOMINALU = "";
    var dtAFEITDECFECHANOMINAL = $("#dtAFEITDECFECHANOMINAL").val();
    if (dtAFEITDECFECHANOMINAL == "") {
        alert("Seleccione una fecha IT + Desicion - Nominal");
        return false;
    }
    AFEITDECFECHANOMINALU = dtAFEITDECFECHANOMINAL;

    var AFEITDECFECHAELABU = "";
    var dtAFEITDECFECHAELAB = $("#dtAFEITDECFECHAELAB").val();
    if (dtAFEITDECFECHAELAB == "") {
        alert("Seleccione una fecha IT + Desicion - Elaboracion");
        return false;
    }
    AFEITDECFECHAELABU = dtAFEITDECFECHAELAB;


    var AFEEMPRESPNINGUNA = "N";
    var chkEmpresaResponsableNinguna = document.getElementById("chkEmpresaResponsableNinguna").checked;
    if (chkEmpresaResponsableNinguna) {
        AFEEMPRESPNINGUNA = "S";
    }

    var AFEEMPCOMPNINGUNA = "N";
    var chkEmpresaCompensadaNinguna = document.getElementById("chkEmpresaCompensadaNinguna").checked;
    if (chkEmpresaCompensadaNinguna) {
        AFEEMPCOMPNINGUNA = "S";
    }

    var AFEESTADO = "V";
    var chkEstadoAnulado = document.getElementById("chkEstadoAnulado").checked;
    if (chkEstadoAnulado) {
        AFEESTADO = "A";
    }

    var chkEstadoArchivado = document.getElementById("chkEstadoArchivado").checked;
    if (chkEstadoArchivado) {
        AFEESTADO = "X";
    }

    var AFEESTADOMOTIVO = $("#txtAFEESTADOMOTIVO").val();
    var AFEITPITFFECHAstr = $('#dtInformeTecnicoAnual').val();
    //var EVENDESCCTAF = $("#txtDescripcionEvento").val();
    var AFEREUHORINI = $("#horaInicioReunion").val();
    var AFEREUHORFIN = $("#horaFinReunion").val();

    var AFELIMATENCOMENU = $("#dtFechaLimite").val();

    var frm = new FormData();
    frm.append("AFECODI", AFECODI);

    frm.append("AFECORR", AFECORR);
    frm.append("AFERMC", AFERMC);
    frm.append("AFEERACMF", AFEERACMF);
    frm.append("AFERACMT", AFERACMT);
    frm.append("AFEEDAGSF", AFEEDAGSF);

    frm.append("AFERESPONSABLE", AFERESPONSABLE);
    frm.append("AFECITFECHAELABU", AFECITFECHAELABU);
    frm.append("AFEREUFECHAPROGU", AFEREUFECHAPROGU);
    frm.append("AFEREUHORAPROG", AFEREUHORAPROG);
    frm.append("AFECONVTIPOREUNION", AFECONVTIPOREUNION);

    frm.append("AFERCTAEOBSERVACION", AFERCTAEOBSERVACION);
    frm.append("AFEITFECHAELABU", AFEITFECHAELABU);
    frm.append("AFEITRDJRFECHAENVIOU", AFEITRDJRFECHAENVIOU);
    frm.append("AFEITRDJRFECHARECEPU", AFEITRDJRFECHARECEPU);

    frm.append("AFEITRDOFECHAENVIOU", AFEITRDOFECHAENVIOU);
    frm.append("AFEITRDOFECHARECEPU", AFEITRDOFECHARECEPU);

    frm.append("AFEITRDJRESTADO", AFEITRDJRESTADO);
    frm.append("AFEITRDOESTADO", AFEITRDOESTADO);

    frm.append("AFEIMPUGNA", AFEIMPUGNA);
    frm.append("AFEFZAMAYOR", AFEFZAMAYOR);
    frm.append("AFEITDECFECHANOMINALU", AFEITDECFECHANOMINALU);
    frm.append("AFEITDECFECHAELABU", AFEITDECFECHAELABU);

    frm.append("AFEEMPRESPNINGUNA", AFEEMPRESPNINGUNA);
    frm.append("AFEEMPCOMPNINGUNA", AFEEMPCOMPNINGUNA);
    frm.append("AFEESTADO", AFEESTADO);
    frm.append("AFEESTADOMOTIVO", AFEESTADOMOTIVO);
    frm.append("AFEITPITFFECHAFecha", AFEITPITFFECHAstr);
    frm.append("AFEANIO", $("#txtEventoAnioCtaf").val());
    frm.append("AFECODISCO", _AFECODISCO);
    frm.append("EVENCODI", _EVENCODI);
    frm.append("AFSALA", AFSALA);
    //frm.append("EVENDESCCTAF", EVENDESCCTAF);
    frm.append("AFEREUHORINI", AFEREUHORINI);
    frm.append("AFEREUHORFIN", AFEREUHORFIN);
    frm.append("AFELIMATENCOMENU", AFELIMATENCOMENU);
    
    $.ajax({
        url: controlador + "ValidaCorrelativo",
        data: frm,
        type: 'POST',
        processData: false,
        contentType: false,
        success: function (result) {

            if ((result >= 1) && (AFECORR != AFECORRORIGINAL)) {

                alert('El correlativo ya existe, no se puede continuar.');

            } else {

                $.ajax({
                    url: controlador + "ActualizarEvento",
                    data: frm,
                    type: 'POST',
                    processData: false,
                    contentType: false,
                    success: function (result) {

                        if (result) {
                            $("#hfAFECORR").val($("#txtAFECORRCtaf").val());
                                $.ajax({
                                    type: 'POST',
                                    url: controlador + "ActualizarInformePortalWeb",
                                    contentType: 'application/json',
                                    dataType: 'json',
                                    traditional: true,
                                    data: JSON.stringify(objeto),
                                    success: function (resultado) {
                                        if (resultado != 1) {
                                            alert("Error portal web");
                                        }
                                     
                                    },
                                    error: function () {
                                        //mostrarError();
                                    }
                                });  
                            if (inputsAO.length > 0) {
                               
                                $.ajax({
                                    type: 'POST',
                                    url: controlador + "ActualizarEventoAO",
                                    contentType: 'application/json',
                                    dataType: 'json',
                                    traditional: true,
                                    data: JSON.stringify(objRcmAO),
                                    success: function (result) {
                                        if (result) {
                                            if (result.Resultado == "-1") {
                                                alert(result.StrMensaje);
                                                alert("Error evento AO");
                                            }
                                            else {
                                                alert("Evento AO no requiere actualización");
                                            }
                                            TraerEmpresaRecomendacionInformeTecnico();
                                        } 
                                      }
                                    });
                            }

                            
                            alert('Se actualizó evento correctamente');
                            Consultar();

                        } else {
                            alert('No se pudo actualizar');
                        }
                    }
                });
            }
        }
    });
}

VerCartaRecomendacionCOES = function () {
    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var filename = "EV-REM-" + $("#txtMedidaAdoptadaAFEANIO").val() + "-" + $("#hfMedidaAdoptadaSelected").val() + ".pdf";
    window.open(controlador + 'verArchivo?filename=' + filename + '&anio=' + $("#txtMedidaAdoptadaAFEANIO").val() + '&id=' + $("#hfMedidaAdoptadaSelected").val(), "_blank", 'fullscreen=yes');
}

VerCartaRespuesta = function () {
    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var filename = "EV-RER-" + $("#txtMedidaAdoptadaAFEANIO").val() + "-" + $("#hfMedidaAdoptadaSelected").val() + ".pdf";
    window.open(controlador + 'verArchivo?filename=' + filename + '&anio=' + $("#txtMedidaAdoptadaAFEANIO").val() + '&id=' + $("#hfMedidaAdoptadaSelected").val(), "_blank", 'fullscreen=yes');
}

function descargarArchivo(paramUrl, paramMethod) {

    $(".modal-loading-background").css("visibility", "visible");

    fetch(paramUrl, { method: paramMethod }).then(response => {

        if (!response.ok) {
            throw new Error("Ocurrió un error al descargar el archivo.");
        }
            
        return response.blob();

    }).then((blob) => {

        const nombreArchivo = "INFORME DEL CTAF.docx";

        const archivo = new File([blob], nombreArchivo, { type: "application/vnd.openxmlformats-officedocument.wordprocessingml.document" });

        const link = document.createElement('a');
        link.href = window.URL.createObjectURL(archivo);
        link.download = nombreArchivo;
        link.click();

        $(".modal-loading-background").css("visibility", "hidden");

    }).catch(error => {
        
        alert("Ocurrió un error al descargar el archivo.");

        console.log(error);

        $(".modal-loading-background").css("visibility", "hidden");
        
    });

}

BajarInformeCTAF = function () {

    var controlador = siteRoot + 'Eventos/AnalisisFallas';
    var id = $("#hfCodigo").val();
    var url = controlador + "/InformeCTAF/" + id;
    
    descargarArchivo(url, 'GET');

}

BajarCitacion = function () {
    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var id = $("#hfCodigo").val();
    var url = controlador + "/Citacion/" + id;
    var link = document.createElement("a");
    link.href = url;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
    delete link;
}

BajarActa = function () {
    //llama al controller ActaCTAF
    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var id = $("#hfCodigo").val();
    var url = controlador + "/ActaCTAF/" + id;
    var link = document.createElement("a");
    link.href = url;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
    delete link;
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

obtenerFechayhora = function () {
    var hoy = new Date();

    var dd = hoy.getDate();
    var mm = hoy.getMonth() + 1;
    var yyyy = hoy.getFullYear();
    var hora = hoy.getHours();
    var minuto = hoy.getMinutes();
    var segundos = hoy.getSeconds();

    if (dd < 10) {
        dd = '0' + dd
    }

    if (mm < 10) {
        mm = '0' + mm
    }

    return dd + '/' + mm + '/' + yyyy + " " + hora + ":" + minuto + ":" + segundos;
}

function cerrar() { $('#divMedidasAdoptadas').bPopup().close() }

eliminarEventoCtaf = function (id) {
    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    if (confirm('Existen Criterios registrados. ¿Desea eliminar el evento?')) {
        $.ajax({
            type: 'POST',
            url: controlador + "ObtieneCantFileEnviadosSco",
            dataType: 'json',
            cache: false,
            data: { evencodi: id },
            success: function (resultado) {
                if (resultado == 0) {
                    $.ajax({
                        type: 'POST',
                        url: controlador + "eliminarEventoCtaf",
                        dataType: 'json',
                        cache: false,
                        data: { evencodi: id },
                        success: function (resultado) {
                            if (resultado == 1) {
                                Consultar();
                            }
                        },
                        error: function () {
                            //mostrarError();
                        }
                    });
                }
                else if (resultado > 0) {
                    if (confirm('Existen Informes de Fallas en el Evento CTAF. ¿Desea eliminar el evento?')) {
                        $.ajax({
                            type: 'POST',
                            url: controlador + "eliminarEventoCtaf",
                            dataType: 'json',
                            cache: false,
                            data: { evencodi: id },
                            success: function (resultado) {
                                if (resultado == 1) {
                                    Consultar();
                                }
                            },
                            error: function () {
                                //mostrarError();
                            }
                        });
                    }
                }
            },
            error: function () {
                //mostrarError();
            }
        });
    }
    //Validar si evencodi o relaciones tienes Ipis o If enviados
    
}

function Check(checkbox) {
    
    var inputs = document.querySelectorAll('.chCtaf');

    for (var k = 0; k < inputs.length; k++) {
        if (inputs[k].id != checkbox.id)
            inputs[k].checked = false;
    }

    if (checkbox.checked == true) {

        var parametros = checkbox.value.split('|');
        var _evencodi = parametros[0];
        var _afecodi = parametros[1];
        $("#hfEvencodicheck").val(_evencodi);
        $("#listadoScoHandsontable").html('');
        
        InformacionSco(_evencodi, _afecodi);
    }
};

function InformacionSco(evencodi, afecodi) {
    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    $.ajax({
        type: 'POST',
        url: controlador + 'ListadoInformesSco',
        data: { evencodi: evencodi },
        success: function (resultado) {
            if (resultado.Resultado != '-1') {
                crearHandsonTable(resultado);
            }
            $("#DetalleEventoSel").html('');
            $.ajax({
                type: 'POST',
                url: controlador + 'DetalleEventoSel',
                data: { evencodi: evencodi, afecodi: afecodi },
                success: function (datos) {
                    $('#DetalleEventoSel').css("width", $('.form-main').width() + "px");
                    $('#DetalleEventoSel').html(datos);
                    $.ajax({
                        type: 'POST',
                        url: controlador + 'ExcelSenalizacionProtecciones',
                        data: { evencodi: evencodi },
                        success: function (resultadoProtecciones) {
                            if (resultadoProtecciones.Handson.ListaExcelData != '-1') {
                                crearHandsonTableProtecciones(resultadoProtecciones);
                            }
                        },
                        error: function (err) {
                            alert("Ingresa al error " + err);
                        }
                    });

                },
                error: function (err) {
                    alert("Ingresa al error " + err);
                }
            });
        },
        error: function (err) {
            alert("Ingresa al error " + err);
        }
    });
}

function AsistenteResponsable() {
    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var evencodi = $("#hfCodigoEvento").val();
    $.ajax({
        type: 'POST',
        url: controlador + 'ListadoAsistenteResponsableSco',
        data: { evencodi: evencodi },
        success: function (resultado) {
            $("#listadoAsistenteHandsontable").html('');
            if (resultado.Resultado != '-1') {
                crearHandsonTableAsistente(resultado);
            }
        },
        error: function (err) {
            alert("Ingresa al error " + err);
        }
    });
}

VerEmpresaInvolucradaSco = function (evencodi, env_evencodi, eveinfcodi) {

    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    //window.open(controlador + 'verArchivoSco?evencodi=' + evencodi + '&env_evencodi=' + env_evencodi + '&eveinfcodi=' + eveinfcodi, "_blank", 'fullscreen=yes');
    location.href =  controlador + 'verArchivoSco?evencodi=' + evencodi + '&env_evencodi=' + env_evencodi + '&eveinfcodi=' + eveinfcodi;
}

EliminarAsistente = function (evencodi, codempr) {

    if (confirm("Esta seguro de Eliminar?")) {
        var controlador = siteRoot + 'Eventos/AnalisisFallas/';

        var frm = new FormData();
        frm.append("EVENCODI", evencodi);
        frm.append("EMPRCODI", codempr);

        $.ajax({
            url: controlador + "EliminarAsistenteResponsable",
            data: frm,
            type: 'POST',
            processData: false,
            contentType: false,
            success: function (result) {

                if (result) {
                    AsistenteResponsable();
                }
            }
        });
    }
}

function SenializacionProteccion() {
    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    var evencodi = $("#hfCodigoEvento").val();
    $.ajax({
        type: 'POST',
        url: controlador + 'ListadoSenializacionProteccion',
        data: { evencodi: evencodi },
        success: function (resultado) {
            $("#CuadroSenalizacionProtecciones").html('');
            if (resultado.Resultado !== '-1') {
                crearHandsonTableProtecciones(resultado);
            }
        },
        error: function (err) {
            alert("Ingresa al error " + err);
        }
    });
}

function EliminarEventoCtafxRecomendacion(afecodi, afrrec) {

    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    $.ajax({
        type: 'POST',
        url: controlador + 'EliminarEventoCtafxRecomendacion',
        data: {
            afecodi: afecodi,
            afrrec: afrrec
        },
        success: function (resultado) {
            if (resultado == 1) {
                alert("Se actualizó correctamente.");
                TraerEmpresaRecomendacionInformeTecnico();
            }
                
        },
        error: function (err) {
            alert("Ingresa al error " + err);
        }
    });
}

var container;
function crearHandsonTable(evtHot) {

    function firstRowRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontWeight = 'bold';
        td.style.fontSize = '10px';
        td.style.color = 'White';
        td.style.background = '#2980B9';
        td.style.textAlign = 'center';
    }

    function firstRowRenderer2(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontWeight = 'bold';
        td.style.fontSize = '10px';
        td.style.color = 'White';
        td.style.background = '#93C47D';
    }

    function descripRowRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '10px';
        td.style.color = 'MidnightBlue';
        td.style.background = '#EAF7D9';
    }

    function descripRowRenderer2(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '10px';
        td.style.color = 'MidnightBlue';
        td.style.background = '#E8F6FF';
        td.style.textAlign = 'center';
    }

    function cambiosCellRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '12px';
        td.style.textAlign = 'right';
        td.style.background = '#FFFFD7';
        $(td).html(formatFloat(parseFloat(value), 3, '.', ','));
    }

    function negativeValueRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);

        if (parseInt(value, 10) < 0) {
            td.style.color = 'orange';
            td.style.fontStyle = 'italic';
        }
    }

    function readOnlyValueRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '12px';
        td.style.textAlign = 'right';
        td.style.color = 'DimGray';
        td.style.background = 'Gainsboro';
        if (parseFloat(value))
            $(td).html(formatFloat(parseFloat(value), 3, '.', ','));
        else {
            if (value == "0")
                $(td).html("0.000");
        }
    }

    function checkRenderer(instance, td, row, col, prop, value, cellProperties) {

        Handsontable.renderers.CheckboxRenderer.apply(this, arguments);
        td.style.fontSize = '12px';
        td.style.textAlign = 'center';
        td.style.color = 'DimGray';
        td.style.background = 'silver';
        
        var valor = value.split('|');
        var _valor = valor[1] + "|" + valor[2] + "|" + valor[3] + "|" + valor[4];
        var checktd = '';
        var idSco = "cb" + valor[1];

        

        if (value[0] == "S") {
            checktd = '<input type="checkbox" class="chPortal" checked="checked" value="' + _valor + '"' + 'id="' + idSco + '" />';
        }
        else {
            checktd = '<input type="checkbox" class="chPortal" value="' + _valor + '"' + 'id="' + idSco + '" />';
        }

        td.innerHTML = checktd;

        $(`#${idSco}`).on('change', function (event) {

            var marcado = event.target.checked ? "S" : "N";
            var nuevoValor = `${marcado}|${event.target.value}`;

            evtHot.Handson.ListaExcelData[row][5] = nuevoValor;

            var inputs = document.querySelectorAll('.chPortal');
            var parametros_check = event.target.value.split('|');

            var _Emprcodi = parametros_check[1];
            var _Afiversion = parametros_check[2];
            var _Afecodi = parametros_check[3];

            
        });
    }

    function calculateSize() {
        var offset = Handsontable.Dom.offset(container);

        if (offset.top == 222) {
            availableHeight = $(window).height() - 140 - offset.top - 20;
        }
        else {
            availableHeight = $(window).height() - 140 - offset.top - 20;
        }

        if (offset.left > 0)
            availableWidth = $(window).width() - 2 * offset.left;
        if (offset.top > 0) {
            availableHeight = availableHeight < HEIGHT_MINIMO ? HEIGHT_MINIMO : availableHeight;
            container.style.height = availableHeight + 'px';
        }
    }

    function BtnDescargarFile(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.background = '#E8F6FF';
        var valor = evtHot.Handson.ListaExcelData[row][col].split('|');
        var buttonFile = '';
        if (valor[0] == 'A')
            buttonFile = '<a href="JavaScript:VerEmpresaInvolucrada(' + valor[1] + ',' + valor[2] + ')" title="Descargar archivo">' + '<img src ="' + siteRoot + 'Content/Images/btn-open.png" style = "margin-top: 5px;width: 20px;height: 20px;" /></a >';
        else if (valor[0] == 'N')
            buttonFile = '<a href="JavaScript:VerEmpresaInvolucradaSco(' + valor[1] + ',' + valor[2] + ',' + valor[3] + ')" title="Descargar archivo">' + '<img src ="' + siteRoot + 'Content/Images/btn-open.png" style = "margin-top: 5px;width: 20px;height: 20px;" /></a >';
        td.innerHTML = buttonFile;
    }

    container = document.getElementById('listadoScoHandsontable');
    Handsontable.renderers.registerRenderer('negativeValueRenderer', negativeValueRenderer);

    hotOptions = {
        data: evtHot.Handson.ListaExcelData,
        maxRows: evtHot.Handson.ListaExcelData.length,
        maxCols: evtHot.Handson.ListaExcelData[0].length,
        colHeaders: true,
        rowHeaders: true,
        fillHandle: true,
        columnSorting: false,        
        className: "htCenter",
        readOnly: evtHot.Handson.ReadOnly,
        fixedRowsTop: 1,
        fixedColumnsLeft: evtHot.Handson.ColCabecera,
        mergeCells: evtHot.Handson.ListaMerge,
        colWidths: evtHot.Handson.ListaColWidth,
        afterLoadData: function () {
            this.render();
        },
        cells: function (row, col, prop) {
            var cellProperties = {};
            var formato = "";
            var render;
            var readOnly = true;
            var tipo;

            if (row == 0) {
                render = firstRowRenderer;
                readOnly = true;
            }
            if (row >= evtHot.Handson.FilasCabecera && row <= evtHot.Handson.ListaExcelData.length) {
                if (col == 0)
                    render = BtnDescargarFile;
                else {
                    if (col == columnaChecks)
                        render = checkRenderer;
                    else
                        render = descripRowRenderer2;
                }

                if (col > 0 && col < evtHot.Handson.ListaExcelData[0].length - 2)
                    readOnly = true;
                else
                    readOnly = false;
            }

            for (var i in evtHot.ListaCambios) {
                if ((row == evtHot.ListaCambios[i].Row) && (col == evtHot.ListaCambios[i].Col)) {
                    render = cambiosCellRenderer;
                }
            }

            cellProperties = {
                renderer: render,
                format: formato,
                type: tipo,
                readOnly: readOnly
            }

            return cellProperties;
        }
    };

    hot = new Handsontable(container, hotOptions);
    calculateSize(1);    
}

var containerAsistente;
function crearHandsonTableAsistente(evtHot) {

    function firstRowRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontWeight = 'bold';
        td.style.fontSize = '10px';
        td.style.color = 'White';
        td.style.background = '#2980B9';
        td.style.textAlign = 'center';
    }

    function descripRowRenderer2(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '10px';
        td.style.color = 'MidnightBlue';
        td.style.background = '#E8F6FF';
        td.style.textAlign = 'center';
    }

    function cambiosCellRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '12px';
        td.style.textAlign = 'right';
        td.style.background = '#FFFFD7';
        $(td).html(formatFloat(parseFloat(value), 3, '.', ','));
    }

    function negativeValueRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);

        if (parseInt(value, 10) < 0) {
            td.style.color = 'orange';
            td.style.fontStyle = 'italic';
        }
    }

    function checkRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.CheckboxRenderer.apply(this, arguments);
        td.style.fontSize = '12px';
        td.style.textAlign = 'center';
        td.style.color = 'DimGray';
        td.style.background = 'silver';
    }

    function calculateSize() {
        var offset = Handsontable.Dom.offset(containerAsistente);

        if (offset.top == 222) {
            availableHeight = $(window).height() - 140 - offset.top - 20;
        }
        else {
            availableHeight = $(window).height() - 140 - offset.top - 20;
        }

        if (offset.left > 0)
            availableWidth = $(window).width() - 2 * offset.left;
        if (offset.top > 0) {
            availableHeight = availableHeight < HEIGHT_MINIMO ? HEIGHT_MINIMO : availableHeight;
        }
    }

    function BtnEliminar(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.background = '#E8F6FF';
        var valor = evtHot.Handson.ListaExcelData[row][col].split('|');
        var buttonEliminar = '';
        buttonEliminar = '<a href="JavaScript:EliminarAsistente(' + valor[0] + ',' + valor[1] + ')" title="Eliminar">' + '<img src ="' + siteRoot + 'Content/Images/btn-cancel.png" style = "margin-top: 5px;width: 20px;height: 20px;" /></a >';
        td.innerHTML = buttonEliminar;
    }

    containerAsistente = document.getElementById('listadoAsistenteHandsontable');
    Handsontable.renderers.registerRenderer('negativeValueRenderer', negativeValueRenderer);

    hotOptions = {
        data: evtHot.Handson.ListaExcelData,
        height: 100,
        maxRows: evtHot.Handson.ListaExcelData.length,
        maxCols: evtHot.Handson.ListaExcelData[0].length,
        colHeaders: true,
        rowHeaders: true,
        fillHandle: true,
        columnSorting: false,
        className: "htCenter",
        readOnly: evtHot.Handson.ReadOnly,
        fixedRowsTop: 1,
        fixedColumnsLeft: evtHot.Handson.ColCabecera,
        mergeCells: evtHot.Handson.ListaMerge,
        colWidths: evtHot.Handson.ListaColWidth,
        afterLoadData: function () {
            this.render();
        },
        cells: function (row, col, prop) {
            var cellProperties = {};
            var formato = "";
            var render;
            var readOnly = true;
            var tipo;

            if (row == 0) {
                render = firstRowRenderer;
                readOnly = true;
            }
            if (row >= evtHot.Handson.FilasCabecera && row <= evtHot.Handson.ListaExcelData.length) {
               if (col == 0)
                   render = BtnEliminar;
               if (col != 0) {
                    if (col == columnaChecks)
                        render = checkRenderer;
                    else
                        render = descripRowRenderer2;
                }

                if (col > 0 && col < evtHot.Handson.ListaExcelData[0].length - 2)
                    readOnly = true;
                else
                    readOnly = false;
            }

            for (var i in evtHot.ListaCambios) {
                if ((row == evtHot.ListaCambios[i].Row) && (col == evtHot.ListaCambios[i].Col)) {
                    render = cambiosCellRenderer;
                }
            }

            cellProperties = {
                renderer: render,
                format: formato,
                type: tipo,
                readOnly: readOnly
            }

            return cellProperties;
        }
    };

    hotAsistente = new Handsontable(containerAsistente, hotOptions);
    calculateSize(1);
}

var containerProtecciones;
function crearHandsonTableProtecciones(evtHot) {

    function firstRowRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontWeight = 'bold';
        td.style.fontSize = '10px';
        td.style.color = 'White';
        td.style.background = '#2980B9';
        td.style.textAlign = 'center';
    }

    function descripRowRenderer2(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '10px';
        td.style.color = 'MidnightBlue';
        td.style.textAlign = 'center';
    }

    function negativeValueRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);

        if (parseInt(value, 10) < 0) {
            td.style.color = 'orange';
            td.style.fontStyle = 'italic';
        }
    }

    var contexMenuCPSenializacion = {
        items: {
            'row_below': {
                name: "Insertar fila abajo",
            },
            'remove_row': {
                name: "Eliminar fila"
            }
        }
    };
    containerProteccion = document.getElementById('CuadroSenalizacionProtecciones');
    Handsontable.renderers.registerRenderer('negativeValueRenderer', negativeValueRenderer);

    if (evtHot.Handson.ListaExcelData !== null) {
        hotOptions = {
            data: evtHot.Handson.ListaExcelData,
            height: 300,
            maxCols: evtHot.Handson.ListaExcelData[0].length,
            colHeaders: false,
            rowHeaders: false,
            fillHandle: true,
            columnSorting: false,
            contextMenu: contexMenuCPSenializacion,
            className: "htCenter",
            readOnly: evtHot.Handson.readOnly,
            fixedRowsTop: 1,
            fixedColumnsLeft: evtHot.Handson.ColCabecera,
            mergeCells: evtHot.Handson.ListaMerge,
            colWidths: evtHot.Handson.ListaColWidth,
            hiddenColumns: {
                columns: [0],
                indicators: true,
            },
            afterLoadData: function () {
                this.render();
            },
            cells: function (row, col, prop) {
                var cellProperties = {};
                var formato = "";
                var render;
                var readOnly = false;
                var tipo;

                if (row == 0) {
                    render = firstRowRenderer;
                    readOnly = true;
                }
                if (row >= evtHot.Handson.FilasCabecera && row <= evtHot.Handson.ListaExcelData.length) {
                    render = descripRowRenderer2;
                    readOnly = false;
                }

                cellProperties = {
                    renderer: render,
                    format: formato,
                    type: tipo,
                    readOnly: false
                }

                return cellProperties;
            }
        };

        hotProteccion = new Handsontable(containerProteccion, hotOptions);
    }

}

GenerarPDF = function (tipoDocumento) {
    var id = $("#hfCodigo").val();
    $.ajax({
        type: 'POST',
        url: controlador + 'GenerarPDF',
        global: false,
        dataType: 'json',
        data: {
            id: id,
            tipoDocumento: tipoDocumento
        },
        cache: false,
        beforeSend: function () {
            $(".modal-loading-background").css("visibility", "visible");
        },
        success: function (resultado) {
            if (resultado.result == '1') {
                
                alert("Se ha generado el documento en formato .pdf");
                if (tipoDocumento == 1) {
                    $("#lblCitacionReunion").text(resultado.responseText);

                    $("#btnVerCitacionReunion").show();
                    $("#btnEliminarCitacionReunion").show();

                }
                else if (tipoDocumento == 2) {
                    $("#lblActaReunion").text(resultado.responseText);
                }
                else if (tipoDocumento == 3) {
                    $("#lblInformeReunion").text(resultado.responseText);
                }
            }
            else if (resultado.result == '-1'){
                alert(resultado.responseText);
            }
            else if (resultado.result == '-2') {
                alert(resultado.responseText);
            }
        },
        error: function () {
        },
        complete: function () {
            $(".modal-loading-background").css("visibility", "hidden");
        }
    });
}

function VerImagenAnalisis(eveanaevecodi) {
    
    var controlador = siteRoot + 'Eventos/AnalisisFallas/';
    location.href = controlador + 'VerImagenAnalisis?eveanaevecodi=' + eveanaevecodi;
}

$(document).ajaxStart(function () {
    $('#loading').bPopup({
        fadeSpeed: 'fast',
        opacity: 0.4,
        followSpeed: 500,
        modalColor: '#000000',
        modalClose: false
    });
});

$(document).ajaxStop(function () {
    $('#loading').bPopup().close();
});

$(document).ready(() => {
    $('#btnGenerarInformeTecnico').on('click', (event) => {
        event.preventDefault();
        const id = $("#hfCodigo").val();
        const url = `${controlador}DescargarInformeTecnico?id=${id}`;
        const link = document.createElement("a");
        link.href = url;
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
        delete link;
    });

    $('#btnGenerarDecisionCTAF').on('click', (event) => {
        event.preventDefault();
        const id = $("#hfCodigo").val();
        const url = `${controlador}DescargarDecisionCTAF?id=${id}`;
        const link = document.createElement("a");
        link.href = url;
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
        delete link;
    });
});