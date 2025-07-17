var eventoInforme = false;
var UrlAbsoluta = "";
var controlador = siteRoot + 'eventos/CriteriosEvento/';

$(function () {

    $('#btnNuevoCasosEspeciales').on('click', function () {
        editarCasosEspeciales(0);
    });   
    consultarCasosEspeciales();

    $('#btnNuevoCriterios').on('click', function () {
        editarCriterios(0);
    });

    $("#btnExportar").click(function () {
        exportar();
    });

    $("#btnAgregarResponsableDesicion").click(function () {
        InsertarResponsable(1);
    });

    $("#btnImportarResponsableDesicion").click(function () {
        ImportarResponsable(1);
    });

    $("#btnAgregarResponsableReconsideracion").click(function () {
        InsertarResponsable(2);
    });
    
    $("#btnImportarResponsableReconsideracion").click(function () {
        ImportarResponsable(2);
    });

    $("#btnAgregarResponsableApelacion").click(function () {
        InsertarResponsable(3);
    });
    
    $("#btnImportarResponsableApelacion").click(function () {
        ImportarResponsable(3);
    });
    
    $("#btnAgregarResponsableArbitraje").click(function () {
        InsertarResponsable(4);
    });

    $("#btnImportarResponsableArbitraje").click(function () {
        ImportarResponsable(4);
    });

    $("#btnAgregarSolicitanteReconsideracion").click(function () {
        InsertarSolicitante(2);
    });

    $("#btnImportarSolicitanteReconsideracion").click(function () {
        ImportarSolicitante(2);
    });

    $("#btnAgregarSolicitanteApelacion").click(function () {
        InsertarSolicitante(3);
    });

    $("#btnImportarSolicitanteApelacion").click(function () {
        ImportarSolicitante(3);
    });

    $("#btnAgregarSolicitanteArbitraje").click(function () {
        InsertarSolicitante(4);
    });

    $("#btnImportarSolicitanteArbitraje").click(function () {
        ImportarSolicitante(4);
    });

    $("#btnGrabar").click(function () {
        GrabarCrEventoEtapas(4);
    });

    $("#btnGrabarSolicitante").click(function () {
        ActualizarEmpresaSolicitante();
    });
    
    consultarCriterios(1);
});

iniLista = function () {

    $('#btnAgregarCasosEspeciales').click(function () {
        window.open(controlador + "CasosEspecialesIndex/", '_blank');
    });

    $('#btnAgregarCriterios').click(function () {
        window.open(controlador + "CriteriosIndex/", '_blank');
    });

    $('#dtInicioCriterio').Zebra_DatePicker({
    });

    $('#dtFinCriterio').Zebra_DatePicker({
    });

    $("#btnConsultarCriteriosEvento").click(function () {
        ConsultarCriteriosEvento(1);
    });
}

iniDetalleEventoCriterio = function () {
    var seleccionDesicion = new SlimSelect({
        select: '#cbCrCriterioDesicion'
    });

    var seleccionReconsideracion = new SlimSelect({
        select: '#cbCrCriterioReconsideracion'
    });

    var seleccionApelacion = new SlimSelect({
        select: '#cbCrCriterioApelacion'
    });

    var seleccionArbitraje = new SlimSelect({
        select: '#cbCrCriterioArbitraje'
    });
    $('#cbCasoEspecial').val($('#hfCasoEspecial').val());
    var arregloDecision = $('#hfCrCriterioDesicion').val().split(',');
    var criteriosDecision = []
    for (x = 0; x < arregloDecision.length; x++) {
        if (arregloDecision[x] != "")
            criteriosDecision.push(arregloDecision[x])
    }
    seleccionDesicion.setSelected(criteriosDecision);
    
    var arregloReconsideracion = $('#hfCrCriterioReconsideracion').val().split(',');
    var criteriosReconsideracion = []
    for (x = 0; x < arregloReconsideracion.length; x++) {
        if (arregloReconsideracion[x] != "")
            criteriosReconsideracion.push(arregloReconsideracion[x])
    }
    seleccionReconsideracion.setSelected(criteriosReconsideracion);

    var arregloApelacion = $('#hfCrCriterioApelacion').val().split(',');
    var criteriosApelacion = []
    for (x = 0; x < arregloApelacion.length; x++) {
        if (arregloApelacion[x] != "")
            criteriosApelacion.push(arregloApelacion[x])
    }
    seleccionApelacion.setSelected(criteriosApelacion);

    var arregloArbitraje = $('#hfCrCriterioArbitraje').val().split(',');
    var criteriosArbitraje = []
    for (x = 0; x < arregloArbitraje.length; x++) {
        if (arregloArbitraje[x] != "")
            criteriosArbitraje.push(arregloArbitraje[x])
    }
    seleccionArbitraje.setSelected(criteriosArbitraje);
    
    $(function () {
        $('#tab-container').easytabs();
    });

    ObtenerEmpresas();
}

ConsultarCriteriosEvento = function (nroPagina) {
    
    debugger;
    var cboEmpresaPropietaria = $("#cboEmpresaPropietaria").val();
    var cboEmpresaInvolucrada = $("#cboEmpresaInvolucrada").val();
    var cboCriteriosD = $("#cboCriteriosD").val();
    var cboCasosEspeciales = $("#cboCasosEspeciales").val();
    var cboImpugnacion = $("#cboImpugnacion").val();
    var cboCriteriosI = $("#cboCriteriosI").val();
    var dtInicioCriterio = $("#dtInicioCriterio").val();
    var dtFinCriterio = $("#dtFinCriterio").val();

    var Impugnacion = "N";

    if (cboImpugnacion != "TODOS") {
        Impugnacion = cboImpugnacion;
    }

    var controlador = siteRoot + 'Eventos/CriteriosEvento/';
    var obj = {
        DI: dtInicioCriterio, //0
        DF: dtFinCriterio, //1
        EmpresaPropietaria: cboEmpresaPropietaria,  //2
        EmpresaInvolucrada: cboEmpresaInvolucrada, //3
        CriterioDecision: cboCriteriosD, //4
        CasosEspeciales: cboCasosEspeciales, //5
        Impugnacionc: Impugnacion, //6
        CriteriosImpugnacion: cboCriteriosI, //7
        NroPagina: nroPagina
    }
    $.ajax({
        type: 'POST',
        url: controlador + "BuscarCriterio",
        data: obj,
        success: function (evt) {
            $('#listadoBuscarCriterio').html(evt);
            $('#tablaBuscarCriterio').dataTable({
                "iDisplayLength": 50
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });

}

consultarCasosEspeciales = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'CasosEspecialesLista',
        success: function (evt) {
            $('#listadoCasosEspeciales').html(evt);
            $('#tablaCasosEspeciales').dataTable({
                "iDisplayLength": 25
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
};

editarCasosEspeciales = function (id) {
    $.ajax({
        type: 'POST',
        url: controlador + 'CasosEspecialesEdit',
        data: {
            CRESPECIALCODI: id
        },
        success: function (evt) {
            $('#contenidoEdicionCasosEspeciales').html(evt);
            setTimeout(function () {
                $('#popupEdicionCasosEspeciales').bPopup({
                    autoClose: false
                });
            }, 50);

            $('#cbEstadoCasosEspeciales').val($('#hfEstadoCasosEspeciales').val());

            $('#btnGrabarCasosEspeciales').on("click", function () {
                grabarCasosEspeciales();
            });

            $('#btnCancelar').on("click", function () {
                $('#popupEdicionCasosEspeciales').bPopup().close();
            });

        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
};

verCasosEspeciales = function (id) {
    $.ajax({
        type: 'POST',
        url: controlador + 'CasosEspecialesEdit',
        data: {
            CRESPECIALCODI: id
        },
        success: function (evt) {
            $('#contenidoEdicionCasosEspeciales').html(evt);
            setTimeout(function () {
                $('#popupEdicionCasosEspeciales').bPopup({
                    autoClose: false
                });
            }, 50);


            document.getElementById('cbEstadoCasosEspeciales').disabled = true;
            document.getElementById('txtDescripcionCasosEspeciales').disabled = true;
            $('#btnGrabarCasosEspeciales').hide();

            $('#cbEstadoCasosEspeciales').val($('#hfEstadoCasosEspeciales').val());

            $('#btnCancelar').on("click", function () {
                $('#popupEdicionCasosEspeciales').bPopup().close();
            });

        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
};

verComentario = function (idevento,etapa) {
    $.ajax({
        type: 'POST',
        url: controlador + 'CREtapaEventoVerComentario',
        data: {
            CREVENCODI: idevento,
            CRETAPA: etapa
        },
        success: function (evt) {
            $('#contenidoVerComentario').html(evt);
            setTimeout(function () {
                $('#popupVerComentario').bPopup({
                    autoClose: false
                });
            }, 50);
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
};

verCriterios = function (id) {
    $.ajax({
        type: 'POST',
        url: controlador + 'CriteriosEdit',
        data: {
            CRCRITERIOCODI: id
        },
        success: function (evt) {
            $('#contenidoEdicionCriterios').html(evt);
            setTimeout(function () {
                $('#popupEdicionCriterios').bPopup({
                    autoClose: false
                });
            }, 50);

            document.getElementById('cbEstadoCriterios').disabled = true;
            document.getElementById('txtDescripcionCriterios').disabled = true;
            $('#btnGrabarCriterios').hide();

            $('#cbEstadoCriterios').val($('#hfEstadoCriterios').val());
            $('#btnCancelar').on("click", function () {
                $('#popupEdicionCriterios').bPopup().close();
            });

        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
};

grabarCasosEspeciales = function () {
    debugger;
    var crespecialcodi = $('#txtCodigoCasosEspeciales').val();
    var crestado = $('#cbEstadoCasosEspeciales').val();
    var crdescripcion = $('#txtDescripcionCasosEspeciales').val();

    var validacion = validarCasosEspeciales();
    if (validacion == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'ValidarCasosEspeciales',
            data: { crespecialcodi: crespecialcodi, crestado: crestado },
            success: function (result) {
                if (result == 0) {
                    $.ajax({
                        type: 'POST',
                        url: controlador + 'CasosEspecialesSave',
                        data: { crespecialcodi: crespecialcodi, crdescripcion: crdescripcion, crestado: crestado },
                        success: function (result) {
                            if (result == 1) {
                                mostrarMensaje('mensaje', 'exito', 'Los datos se grabaron correctamente.');
                                $('#popupEdicionCasosEspeciales').bPopup().close();
                                consultarCasosEspeciales();
                            }
                            else {
                                mostrarMensaje('mensajeEdicion', 'error', 'Se ha producido un error.');
                            }
                        },
                        error: function () {
                            mostrarMensaje('mensajeEdicion', 'error', 'Se ha producido un error.');
                        }
                    });
                }
                else
                    mostrarMensaje('mensajeEdicion', 'error', 'Existen criterios asignados al Caso Especial.');
            },
            error: function () {
                mostrarMensaje('mensajeEdicion', 'error', 'Se ha producido un error.');
            }
        });  
    }
    else {
        mostrarMensaje('mensajeEdicion', 'alert', validacion);
    }
}

eliminarCasosEspeciales = function (id) {
    debugger;
    var crespecialcodi = id;
    var crestado = 'I';

    $.ajax({
        type: 'POST',
        url: controlador + 'ValidarCasosEspeciales',
        data: { crespecialcodi: crespecialcodi, crestado: crestado },
        success: function (result) {
            if (result == 0) {
                if (confirm('¿Está seguro de realizar esta operación?')) {
                    $.ajax({
                        type: 'POST',
                        url: controlador + 'CasosEspecialesDelete',
                        data: {
                            CRESPECIALCODI: id
                        },
                        dataType: 'json',
                        success: function (result) {
                            if (result == 1) {
                                mostrarMensaje('mensaje', 'exito', 'El registro se eliminó correctamente.');
                                consultarCasosEspeciales();
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
            else
                mostrarMensaje('mensaje', 'error', 'Existen criterios asignados al Caso Especial.');
        },
        error: function () {
            mostrarMensaje('mensajeEdicion', 'error', 'Se ha producido un error.');
        }
    });

    
};

validarCasosEspeciales = function () {
    debugger;
    var mensaje = "<ul>";
    var flag = true;
    if ($('#txtCodigoCasosEspeciales').val() == "") {
        mensaje = mensaje + "<li>Ingrese un codigo.</li>";
        flag = false;
    }
    if ($('#txtDescripcionCasosEspeciales').val() == "") {
        mensaje = mensaje + "<li>Ingrese una descripción.</li>";
        flag = false;
    }
    if ($('#cbEstadoCasosEspeciales').val() == "") {
        mensaje = mensaje + "<li>Seleccione el estado</li>";
        flag = false;
    }

    mensaje = mensaje + "</ul>";

    if (flag) mensaje = "";

    return mensaje;
}

mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}

consultarCriterios = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'CriteriosLista',
        success: function (evt) {
            $('#listadoCriterios').html(evt);
            $('#tablaCriterios').dataTable({
                "iDisplayLength": 25
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
};

editarCriterios = function (id) {
    
    $.ajax({
        type: 'POST',
        url: controlador + 'CriteriosEdit',
        data: {
            CRCRITERIOCODI: id
        },
        success: function (evt) {
            $('#contenidoEdicionCriterios').html(evt);
            setTimeout(function () {
                $('#popupEdicionCriterios').bPopup({
                    autoClose: false
                });
            }, 50);

            $('#cbEstadoCriterios').val($('#hfEstadoCriterios').val());

            $('#btnGrabarCriterios').on("click", function () {
                grabarCriterios();
            });

            $('#btnCancelar').on("click", function () {
                $('#popupEdicionCriterios').bPopup().close();
            });

        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
};

grabarCriterios = function () {
    var crcriteriocodi = $('#txtCodigoCriterios').val();
    var crestado = $('#cbEstadoCriterios').val();
    var crdescripcion = $('#txtDescripcionCriterios').val();

    var validacion = validarCriterios();
    if (validacion == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'ValidarCriterios',
            data: { crcriteriocodi: crcriteriocodi, crestado: crestado },
            success: function (result) {
                if (result == 0) {
                    $.ajax({
                        type: 'POST',
                        url: controlador + 'CriteriosSave',
                        data: { crcriteriocodi: crcriteriocodi, crdescripcion: crdescripcion, crestado: crestado },
                        success: function (result) {
                            if (result == 1) {
                                mostrarMensaje('mensaje', 'exito', 'Los datos se grabaron correctamente.');
                                $('#popupEdicionCriterios').bPopup().close();
                                consultarCriterios();
                            }
                            else {
                                mostrarMensaje('mensajeEdicion', 'error', 'Se ha producido un error.');
                            }
                        },
                        error: function () {
                            mostrarMensaje('mensajeEdicion', 'error', 'Se ha producido un error.');
                        }
                    });
                }
                else {
                    mostrarMensaje('mensajeEdicion', 'error', 'Existen etapas asignadas al criterio.');
                }
            },
            error: function () {
                mostrarMensaje('mensajeEdicion', 'error', 'Se ha producido un error.');
            }
        }); 
    }
    else {
        mostrarMensaje('mensajeEdicion', 'alert', validacion);
    }
}

eliminarCriterios = function (id) {
    var crcriteriocodi = id;
    var crestado = 'I';

    $.ajax({
        type: 'POST',
        url: controlador + 'ValidarCriterios',
        data: { crcriteriocodi: crcriteriocodi, crestado: crestado },
        success: function (result) {
            if (result == 0) {
                if (confirm('¿Está seguro de realizar esta operación?')) {
                    $.ajax({
                        type: 'POST',
                        url: controlador + 'CriteriosDelete',
                        data: {
                            CRCRITERIOCODI: id
                        },
                        dataType: 'json',
                        success: function (result) {
                            if (result == 1) {
                                mostrarMensaje('mensaje', 'exito', 'El registro se eliminó correctamente.');
                                consultarCriterios();
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
            else {
                mostrarMensaje('mensaje', 'error', 'Existen etapas asignadas al criterio.');
            }
        },
        error: function () {
            mostrarMensaje('mensajeEdicion', 'error', 'Se ha producido un error.');
        }
    });

    
};

validarCriterios = function () {

    var mensaje = "<ul>";
    var flag = true;

    if ($('#txtCodigoCriterios').val() == "") {
        mensaje = mensaje + "<li>Ingrese un codigo.</li>";
        flag = false;
    }

    if ($('#txtDescripcionCriterios').val() == "") {
        mensaje = mensaje + "<li>Ingrese una descripción.</li>";
        flag = false;
    }

    if ($('#cbEstadoCriterios').val() == "") {
        mensaje = mensaje + "<li>Seleccione el estado</li>";
        flag = false;
    }

    mensaje = mensaje + "</ul>";

    if (flag) mensaje = "";

    return mensaje;
}

verEvento = function (id) {
    window.open(controlador + 'detalleeventocriterio?id=' + id, '_blank')
}

exportar = function () {

    var cboEmpresaPropietaria = $("#cboEmpresaPropietaria").val();
    var cboEmpresaInvolucrada = $("#cboEmpresaInvolucrada").val();
    var cboCriteriosD = $("#cboCriteriosD").val();
    var cboCasosEspeciales = $("#cboCasosEspeciales").val();
    var cboImpugnacion = $("#cboImpugnacion").val();
    var cboCriteriosI = $("#cboCriteriosI").val();
    var dtInicioCriterio = $("#dtInicioCriterio").val();
    var dtFinCriterio = $("#dtFinCriterio").val();

    var Impugnacion = "N";

    if (cboImpugnacion != "TODOS") {
        Impugnacion = cboImpugnacion;
    }

    var obj = {
        DI: dtInicioCriterio, //0
        DF: dtFinCriterio, //1
        EmpresaPropietaria: cboEmpresaPropietaria,  //2
        EmpresaInvolucrada: cboEmpresaInvolucrada, //3
        CriterioDecision: cboCriteriosD, //4
        CasosEspeciales: cboCasosEspeciales, //5
        Impugnacionc: Impugnacion, //6
        CriteriosImpugnacion: cboCriteriosI, //7
        NroPagina: 1
    }
    $.ajax({
        type: 'POST',
        url: controlador + "ReporteCriterios",
        data: obj,
        success: function (resultado) {
            if (resultado.result == 1) {
                location.href = controlador + "DescargarReporteCriterios";
            }
            else {
                //mostrarError();
            }
        },
        error: function (error) {
            mostrarMensaje('mensaje', 'error', "Lo sentimos ocurrió un error.");
        }
    });
}

InsertarResponsable = function (etapa) {
    var codigoEmpresa;
    var cretapacodi;
    var crevencodi = $('#hfCrevencodi').val();
    
    if (etapa == 1) {
        codigoEmpresa = $("#txtEmpresaDesicion").val();
        cretapacodi = $("#hfIdEtapaDesicion").val();
    }
    else if (etapa == 2) {
        codigoEmpresa = $("#txtEmpresaReconsideracion").val();
        cretapacodi = $("#hfIdEtapaReconsideracion").val();
    }
    else if (etapa == 3) {
        codigoEmpresa = $("#txtEmpresaApelacion").val();
        cretapacodi = $("#hfIdEtapaApelacion").val();
    }
    else if (etapa == 4) {
        codigoEmpresa = $("#txtEmpresaArbitraje").val();
        cretapacodi = $("#hfIdEtapaArbitraje").val();
    }
        

    var Mensaje = "";
    if (codigoEmpresa == "") {
        Mensaje += "<li>Por favor busque una empresa</li>";
    }

    if (Mensaje != "") {
        $("#ulMensajeEmpresa").empty();
        $("#ulMensajeEmpresa").append(Mensaje);

        $('#divMensajeEmpresas').bPopup({
            follow: [true, true],
            position: ['auto', 'auto'],
            positionStyle: 'fixed',
        });
        return;
    }

    controlador = siteRoot + 'eventos/CriteriosEvento/';

    $.ajax({
        type: 'POST',
        url: controlador + "InsertarResponsable",
        dataType: 'json',
        data: { cretapacodi: cretapacodi, codigoEmpresa: codigoEmpresa, crevencodi: crevencodi, etapa: etapa },
        success: function (result) {
            if (etapa == 1) {
                $("#txtEmpresaDesicion").val("");
                $("#txtEmpresaDesicion").attr("data-id", "");
                $("#txtEmpresaDesicion").attr("data-name", "");
            }               
            if (etapa == 2) {
                $("#txtEmpresaReconsideracion").val("");
                $("#txtEmpresaReconsideracion").attr("data-id", "");
                $("#txtEmpresaReconsideracion").attr("data-name", "");
            }                
            if (etapa == 3) {
                $("#txtEmpresaApelacion").val("");
                $("#txtEmpresaApelacion").attr("data-id", "");
                $("#txtEmpresaApelacion").attr("data-name", "");
            }                
            if (etapa == 4) {
                $("#txtEmpresaArbitraje").val("");
                $("#txtEmpresaArbitraje").attr("data-id", "");
                $("#txtEmpresaArbitraje").attr("data-name", "");
            }

            TraerEmpresaResponsable(etapa);
        }
    });
}

TraerEmpresaResponsable = function (etapa) {
    var cretapacodi;
    if (etapa == 1)
        cretapacodi = $("#hfIdEtapaDesicion").val();
    else if (etapa == 2)
        cretapacodi = $("#hfIdEtapaReconsideracion").val();
    else if (etapa == 3)
        cretapacodi = $("#hfIdEtapaApelacion").val();
    else if (etapa == 4)
        cretapacodi = $("#hfIdEtapaArbitraje").val();

    controlador = siteRoot + 'eventos/CriteriosEvento/';

    $.ajax({
        type: 'POST',
        url: controlador + "TraerEmpresaResponsable",
        dataType: 'json',
        data: { cretapacodi: cretapacodi },
        success: function (json) {
            var html = "";
            if (json != "") {
                var njson = json.length;
                console.log(json);
                for (var i = 0; i < njson; i++) {
                    html += "<tr>";
                    html += "<td style='border: hidden'><a href='#' onclick='EliminarEmpresaResponsable(" + json[i].CRRESPEMPRCODI + "," + etapa +")'><img src='" + siteRoot + "Content/Images/btn-cancel.png' style='margin-top: 2px;'/></a></td>";

                    html += "<td>" + json[i].EMPRNOMB + "</td>";
                    html += "<td>" + json[i].LASTUSER + "</td>";
                    html += "<td>" + json[i].LASTDATEstr + "</td>";
                    html += "</tr>";
                }
                if (etapa == 1) {
                    $("#tbodyEmpresaResponsableDesicion").empty();
                    $("#tbodyEmpresaResponsableDesicion").append(html);
                }
                else if (etapa == 2) {
                    $("#tbodyEmpresaResponsableReconsideracion").empty();
                    $("#tbodyEmpresaResponsableReconsideracion").append(html);
                }
                else if (etapa == 3) {
                    $("#tbodyEmpresaResponsableApelacion").empty();
                    $("#tbodyEmpresaResponsableApelacion").append(html);
                }
                else if (etapa == 4) {
                    $("#tbodyEmpresaResponsableArbitraje").empty();
                    $("#tbodyEmpresaResponsableArbitraje").append(html);
                }

            } else {
                if (etapa == 1)
                    $("#tbodyEmpresaResponsableDesicion").empty();
                else if (etapa == 2)
                    $("#tbodyEmpresaResponsableReconsideracion").empty();
                else if (etapa == 3)
                    $("#tbodyEmpresaResponsableApelacion").empty();
                else if (etapa == 4)
                    $("#tbodyEmpresaResponsableArbitraje").empty();
            }
        }
    });
}

ImportarResponsable = function (etapa) {
    var cretapacodi;
    var afecodi = $("#hfAfecodi").val();
    if (etapa == 1)
        cretapacodi = $("#hfIdEtapaDesicion").val();
    else if (etapa == 2)
        cretapacodi = $("#hfIdEtapaReconsideracion").val();
    else if (etapa == 3)
        cretapacodi = $("#hfIdEtapaApelacion").val();
    else if (etapa == 4)
        cretapacodi = $("#hfIdEtapaArbitraje").val();

    var crevencodi = $('#hfCrevencodi').val();

    controlador = siteRoot + 'eventos/CriteriosEvento/';

    $.ajax({
        type: 'POST',
        url: controlador + "ImportarResponsable",
        dataType: 'json',
        data: { cretapacodi: cretapacodi, afecodi: afecodi, crevencodi: crevencodi, etapa: etapa },
        success: function (result) {
            TraerEmpresaResponsable(etapa);
        }
    });
}

EliminarEmpresaResponsable = function (crrespemprcodi, etapa) {
    if (confirm("Está seguro que desea eliminar el registro?")) {
        var controlador = siteRoot + 'eventos/CriteriosEvento/';

        $.ajax({
            type: 'POST',
            url: controlador + "EliminarEmpresaResponsable",
            dataType: 'json',
            data: { crrespemprcodi: crrespemprcodi },
            success: function (resultado) {
                if (resultado.result != '-1') {
                    mostrarMensaje('mensaje', 'exito', "Se eliminó correctamente");
                    TraerEmpresaResponsable(etapa);
                }
            }
        });
    }
}

InsertarSolicitante = function (etapa) {
    var codigoEmpresa;
    var crevencodi = $('#hfCrevencodi').val();
    var cretapacodi;
    var argumentos;
    var desicion;
    if (etapa == 2) {
        codigoEmpresa = $('#txtEmpresaRecSol').val();
        cretapacodi = $("#hfIdEtapaReconsideracion").val();
        argumentos = $("#txtArgumentoRec").val();
        desicion = $("#txtDesicionRec").val();       
    }
    else if (etapa == 3) {
        codigoEmpresa = $('#txtEmpresaApeSol').val();
        cretapacodi = $("#hfIdEtapaApelacion").val();
        argumentos = $("#txtArgumentoApe").val();
        desicion = $("#txtDesicionApe").val();
    }
    else if (etapa == 4) {
        codigoEmpresa = $('#txtEmpresaArbSol').val();
        cretapacodi = $("#hfIdEtapaArbitraje").val();
        argumentos = $("#txtArgumentoArb").val();
        desicion = $("#txtDesicionArb").val();
    }
        
    var Mensaje = "";
    if (codigoEmpresa == "") {
        Mensaje += "<li>Por favor busque una empresa</li>";
    }

    if (argumentos == "") {
        Mensaje += "<li>Por favor ingrese los principales argumentos.</li>";
    }

    if (desicion == "") {
        Mensaje += "<li>Por favor ingrese la desición adoptada.</li>";
    }

    if (argumentos.length > 9500) {
        Mensaje += "<li>El campo principales argumentos no debe ser mayor a 9500 caracteres.</li>";
    }
    if (desicion.length > 4000) {
        Mensaje += "<li>El campo decisión adoptada no debe ser mayor a 4000 caracteres.</li>";
    }

    if (Mensaje != "") {
        $("#ulMensajeEmpresa").empty();
        $("#ulMensajeEmpresa").append(Mensaje);

        $('#divMensajeEmpresas').bPopup({
            follow: [true, true],
            position: ['auto', 'auto'],
            positionStyle: 'fixed',
        });
        return;
    }

    controlador = siteRoot + 'eventos/CriteriosEvento/';

    $.ajax({
        type: 'POST',
        url: controlador + "InsertarSolicitante",
        dataType: 'json',
        data: { cretapacodi: cretapacodi, codigoEmpresa: codigoEmpresa, crevencodi: crevencodi, etapa: etapa, argumentos: argumentos, desicion: desicion },
        success: function (result) {
            if (etapa == 2) {
                $("#txtEmpresaRecSol").val("");
                $("#txtEmpresaRecSol").attr("data-id", "");
                $("#txtEmpresaRecSol").attr("data-name", "");
                $("#txtArgumentoRec").val("");
                $("#txtDesicionRec").val("");
            }
            else if (etapa == 3) {
                $("#txtEmpresaApeSol").val("");
                $("#txtEmpresaApeSol").attr("data-id", "");
                $("#txtEmpresaApeSol").attr("data-name", "");
                $("#txtArgumentoApe").val("");
                $("#txtDesicionApe").val("");
            }
            else if (etapa == 4) {
                $("#txtEmpresaArbSol").val("");
                $("#txtEmpresaArbSol").attr("data-id", "");
                $("#txtEmpresaArbSol").attr("data-name", "");
                $("#txtArgumentoArb").val("");
                $("#txtDesicionArb").val("");
            }
                
            TraerEmpresaSolicitante(etapa);
        }
    });
}

TraerEmpresaSolicitante = function (etapa) {
    var cretapacodi;
    if (etapa == 2)
        cretapacodi = $("#hfIdEtapaReconsideracion").val();
    else if (etapa == 3)
        cretapacodi = $("#hfIdEtapaApelacion").val();
    else if (etapa == 4)
        cretapacodi = $("#hfIdEtapaArbitraje").val();
    controlador = siteRoot + 'eventos/CriteriosEvento/';
    $.ajax({
        type: 'POST',
        url: controlador + "TraerEmpresaSolicitante",
        dataType: 'json',
        data: { cretapacodi: cretapacodi },
        success: function (json) {
            var html = "";
            if (json != "") {
                var njson = json.length;
                console.log(json);
                for (var i = 0; i < njson; i++) {
                    html += "<tr>";
                    html += "<td style='border: hidden'><a href='#' onclick='VerEmpresaSolicitante(" + json[i].CRSOLEMPRCODI + ")'><img src='" + siteRoot + "Content/Images/btn-open.png' style='margin-top: 2px;'/></a>";
                    html += "<a href='#' onclick='EditarEmpresaSolicitante(" + json[i].CRSOLEMPRCODI + "," + etapa + ")'><img src='" + siteRoot + "Content/Images/btn-edit.png' style='margin-top: 2px;'/></a>";
                    html += "<a href='#' onclick='EliminarEmpresaSolicitante(" + json[i].CRSOLEMPRCODI + "," + etapa + ")'><img src='" + siteRoot + "Content/Images/btn-cancel.png' style='margin-top: 2px;'/></a></td>";

                    html += "<td>" + json[i].EMPRNOMB + "</td>";
                    if (json[i].CRARGUMENTO != null)
                        html += "<td>" + json[i].CRARGUMENTO + "</td>";
                    else
                        html += "<td></td>";

                    if (json[i].CRDECISION != null)
                        html += "<td>" + json[i].CRDECISION + "</td>";
                    else
                        html += "<td></td>";
                    html += "<td>" + json[i].LASTUSER + "</td>";
                    html += "<td>" + json[i].LASTDATEstr + "</td>";
                    html += "</tr>";
                }
                if (etapa == 2) {
                    $("#tbodyReconsideracionEmpresaSolicitante").empty();
                    $("#tbodyReconsideracionEmpresaSolicitante").append(html);
                }
                else if (etapa == 3) {
                    $("#tbodyApelacionEmpresaSolicitante").empty();
                    $("#tbodyApelacionEmpresaSolicitante").append(html);
                }
                else if (etapa == 4) {
                    $("#tbodyArbitrajeEmpresaSolicitante").empty();
                    $("#tbodyArbitrajeEmpresaSolicitante").append(html);
                }

            } else {
                if (etapa == 2)
                    $("#tbodyReconsideracionEmpresaSolicitante").empty();
                else if (etapa == 3)
                    $("#tbodyApelacionEmpresaSolicitante").empty();
                else if (etapa == 4)
                    $("#tbodyArbitrajeEmpresaSolicitante").empty();
            }
        }
    });
}

ImportarSolicitante = function (etapa) {
    var cretapacodi;
    var afecodi = $("#hfAfecodi").val();
    if (etapa == 2)
        cretapacodi = $("#hfIdEtapaReconsideracion").val();
    else if (etapa == 3)
        cretapacodi = $("#hfIdEtapaApelacion").val();
    else if (etapa == 4)
        cretapacodi = $("#hfIdEtapaArbitraje").val();

    var crevencodi = $('#hfCrevencodi').val();
    controlador = siteRoot + 'eventos/CriteriosEvento/';

    $.ajax({
        type: 'POST',
        url: controlador + "ImportarSolicitante",
        dataType: 'json',
        data: { cretapacodi: cretapacodi, afecodi: afecodi, etapa: etapa, crevencodi: crevencodi },
        success: function (result) {
            TraerEmpresaSolicitante(etapa);
        }
    });
}

EliminarEmpresaSolicitante = function (crsolemprcodi, etapa) {
    if (confirm("Está seguro que desea eliminar el registro?")) {
        var controlador = siteRoot + 'eventos/CriteriosEvento/';

        $.ajax({
            type: 'POST',
            url: controlador + "EliminarEmpresaSolicitante",
            dataType: 'json',
            data: { crsolemprcodi: crsolemprcodi },
            success: function (resultado) {
                if (resultado.result != '-1') {
                    mostrarMensaje('mensaje', 'exito', "Se eliminó correctamente");
                    TraerEmpresaSolicitante(etapa);
                }
            }
        });
    }
}

GrabarCrEventoEtapas = function () {

    var mensaje = ValidarRegistro();
    if (mensaje != "") {
        $("#ulValidacion").empty();
        $("#ulValidacion").append(mensaje);

        $('#divMensajeValidacion').bPopup({
            follow: [true, true],
            position: ['auto', 'auto'],
            positionStyle: 'fixed',
        });
        return;
    }

    var frm = new FormData();
    frm.append("CREVENCODI", $('#hfCrevencodi').val());
    frm.append("AFECODI", $("#hfAfecodi").val());
    frm.append("CRESPECIALCODI", $("#cbCasoEspecial").val());
    frm.append("FECHA_DECISION", $("#txtFechDecision").val());
    frm.append("CRCRITERIOCODIDESICION", $("#cbCrCriterioDesicion").val());
    frm.append("DESCRIPCION_EVENTO_DECISION", $("#txtDescripcionEvento").val());
    frm.append("RESUMEN_DECISION", $("#txtResumenDecision").val());
    frm.append("COMENTARIO_EMPRESA_DECISION", $("#txtComentarioRespDecision").val());
    frm.append("CRCRITERIOCODIRECONSIDERACION", $("#cbCrCriterioReconsideracion").val());
    frm.append("COMENTARIOS_RECONCIDERACION", $("#txtComentarioRespReconsideracion").val());
    frm.append("CRCRITERIOCODIAPELACION", $("#cbCrCriterioApelacion").val());
    frm.append("COMENTARIOS_APELACION", $("#txtComentarioRespApelacion").val());
    frm.append("CRCRITERIOCODIARBITRAJE", $("#cbCrCriterioArbitraje").val());
    frm.append("COMENTARIOS_ARBITRAJE", $("#txtComentarioRespArbitraje").val());
    
    controlador = siteRoot + 'eventos/CriteriosEvento/';

    $.ajax({
        type: 'POST',
        url: controlador + "GrabarCrEventoEtapas",
        data: frm,
        processData: false,
        contentType: false,
        success: function (result) {
            var strMensaje;
            strMensaje += "Se registró la decisión \n ";
            strMensaje += "Se registró la recomendación \n ";
            mostrarMensaje('mensaje', 'exito', "Se registraron los datos en las etapas de Decisión, Recomendación, Apelación y Arbitraje.");
        }
    });
}

function EditarEmpresaSolicitante(crsolemprcodi, etapa) {
    $('#hfEtapaSolicitante').val(etapa);
    controlador = siteRoot + 'eventos/CriteriosEvento/';
    
    $.ajax({
        type: 'POST',
        url: controlador + 'EmpresaSolicitante',
        data: { crsolemprcodi: crsolemprcodi, tipoSolicitante: 1},
        success: function (evt) {
            $('#contenidoSolicitante').html(evt);
            
            setTimeout(function () {
                $('#popupSolicitante').bPopup({
                    autoClose: false,
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown',
                    onClose: function () {
                        $('#popup').empty();
                    }
                });
            }, 500);

        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', "Lo sentimos ocurrió un error.");
        }
    });
}

function ActualizarEmpresaSolicitante() {
    var crsolemprcodi = $("#hfCrsolemprcodi").val();
    var argumentos = $("#txtArgumentoSolicitante").val();
    var desicion = $("#txtDesicionSolicitante").val();
    var etapa = $("#hfEtapaSolicitante").val();

    var Mensaje = "";
    if (argumentos == "") {
        Mensaje += "<li>Por favor ingrese los principales argumentos.</li>";
    }
    if (desicion == "") {
        Mensaje += "<li>Por favor ingrese la desición adoptada.</li>";
    }
    if (argumentos.length > 9500) {
        Mensaje += "<li>El campo principales argumentos no debe ser mayor a 9500 caracteres.</li>";
    }
    if (desicion.length > 4000) {
        Mensaje += "<li>El campo decisión adoptada no debe ser mayor a 4000 caracteres.</li>";
    }

    if (Mensaje != "") {
        mostrarMensaje('mensajeSolicitante', 'alert', Mensaje);
        return;
    }
    
    controlador = siteRoot + 'eventos/CriteriosEvento/';
    $.ajax({
        type: 'POST',
        url: controlador + 'ActualizarEmpresaSolicitante',
        data: {
            crsolemprcodi: crsolemprcodi,
            argumentos: argumentos,
            desicion: desicion
        },
        success: function (resultado) {
            if (resultado.result == 1) {               
                TraerEmpresaSolicitante(etapa);
                var bPopup = $('#popupSolicitante').bPopup();
                bPopup.close();
                mostrarMensaje('mensaje', 'exito', "Se actualizó empresa solicitante");               
            }
            else {
                mostrarMensaje('mensaje', 'error', "Ocurrió un error.");
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', "Lo sentimos ocurrió un error.");
        }
    });
}

function VerEmpresaSolicitante(crsolemprcodi) {
    controlador = siteRoot + 'eventos/CriteriosEvento/';

    $.ajax({
        type: 'POST',
        url: controlador + 'EmpresaSolicitante',
        data: { crsolemprcodi: crsolemprcodi, tipoSolicitante: 2 },
        success: function (evt) {
            $('#contenidoSolicitante').html(evt);

            setTimeout(function () {
                $('#popupSolicitante').bPopup({
                    autoClose: false,
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown',
                    onClose: function () {
                        $('#popup').empty();
                    }
                });
            }, 500);

        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', "Lo sentimos ocurrió un error.");
        }
    });
}

ObtenerEmpresas = function () {
    var controlador = siteRoot + 'eventos/CriteriosEvento/';

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

                $("#txtEmpresaDesicion").append(html);
                $("#txtEmpresaRecSol").append(html);
                $("#txtEmpresaReconsideracion").append(html);
                $("#txtEmpresaApeSol").append(html);
                $("#txtEmpresaApelacion").append(html);
                $("#txtEmpresaArbSol").append(html);
                $("#txtEmpresaArbitraje").append(html);
                
                $('#txtEmpresaDesicion').selectToAutocomplete();
                $('#txtEmpresaRecSol').selectToAutocomplete();
                $('#txtEmpresaReconsideracion').selectToAutocomplete();
                $('#txtEmpresaApeSol').selectToAutocomplete();
                $('#txtEmpresaApelacion').selectToAutocomplete();
                $('#txtEmpresaArbSol').selectToAutocomplete();
                $('#txtEmpresaArbitraje').selectToAutocomplete();
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', "Lo sentimos ocurrió un error.");
        }
    });
}

function ValidarRegistro() {
    debugger;
    var mensaje = "";
    if ($("#txtDescripcionEvento").val().length > 9500) {
        mensaje += "<li>La descripción del evento no debe ser mayor a 9500 caracteres.</li>";
    }
    if ($("#txtResumenDecision").val().length > 9500) {
        mensaje += "<li>El resumen de la decisión no debe ser mayor a 9500 caracteres.</li>";
    }
    if ($("#txtComentarioRespDecision").val().length > 4000) {
        mensaje += "<li>Los Comentarios a las empresas responsables (decisión) no debe ser mayor a 4000 caracteres.</li>";
    }
    if ($("#txtComentarioRespReconsideracion").val().length > 4000) {
        mensaje += "<li>Los Comentarios a las empresas responsables (reconsideración) no debe ser mayor a 4000 caracteres.</li>";
    }
    if ($("#txtComentarioRespApelacion").val().length > 4000) {
        mensaje += "<li>Los Comentarios a las empresas responsables (apelación) no debe ser mayor a 4000 caracteres.</li>";
    }
    if ($("#txtComentarioRespArbitraje").val().length > 4000) {
        mensaje += "<li>Los Comentarios a las empresas responsables (arbitraje) no debe ser mayor a 4000 caracteres.</li>";
    }
    
    return mensaje;

}