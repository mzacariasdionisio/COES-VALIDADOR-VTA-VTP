var controlador = siteRoot + 'tiemporeal/control/';
var linksXMLDespacho = [
  controlador + "descargarxmldespachohidro",
  controlador + "descargarxmldespachotermo"
];

var linksXMLCostoVariable = [
  controlador + "descargarxmlcvccomb",
  controlador + "descargarxmlcvcvc",
  controlador + "descargarxmlcvcvnc"
];


var linksXMLMantenimiento = [
  controlador + "descargarxmlmantoHydro",
  controlador + "descargarxmlmantoThermo",
  controlador + "descargarxmlmantoBanco",
  controlador + "descargarxmlmantoReactor",
  controlador + "descargarxmlmantoCS",
  controlador + "descargarxmlmantoSVC"
];


var uploaderVagua;
var uploaderPActiva;

$(function () {

    cargaDeArchivo(uploaderVagua,
        'btnVaguaImportar',
        '#fileInfoVagua',
        'containerVagua',
        '#nombrearchivoVagua',
        '#hfArchivoVagua',
        '#ValoraguaDatos',
        'tablaVagua',
        'rbVaguaOpcPdiario',
        'rbVaguaOpcReprograma',
        'VaguaOpcReprograma',
        'Vagua');

    cargaDeArchivo(uploaderPActiva,
        'btnPotenciaImportar',
        '#fileInfoPActiva',
        'containerPActiva',
        '#nombrearchivoPActiva',
        '#hfArchivoPActiva',
        '#PotenciaDatos',
        'tablaPActiva',
        'rbPotenciaOpcPdiario',
        'rbPotenciaOpcReprograma',
        'PotenciaOpcReprograma',
        'PActiva');


    $('#tab-container').easytabs({
        animate: false
    });

    $('#tab-containerDespacho').easytabs({
        animate: false
    });

    $('#tab-containerValoragua').easytabs({
        animate: false
    });

    $('#tab-containerCostovariable').easytabs({
        animate: false
    });

    $('#tab-containerMantenimiento').easytabs({
        animate: false
    });

    $('#tab-containerRegulacionasignada').easytabs({
        animate: false
    });

    $('#tab-RegulacionasignadaConfiguracionTab').easytabs({
        animate: false
    });


    $('#txtFecha').inputmask({
        mask: "1/2/y",
        placeholder: "dd/mm/yyyy",
        alias: "datetime"
    });

    $('#txtFecha').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#txtFecha').val(date);
        }
    });


    $('#txtCvarExpFechaIni').inputmask({
        mask: "1/2/y",
        placeholder: "dd/mm/yyyy",
        alias: "datetime"
    });

    $('#txtCvarExpFechaIni').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#txtCvarExpFechaIni').val(date);
        }
    });


    $('#txtCvarExpFechaFin').inputmask({
        mask: "1/2/y",
        placeholder: "dd/mm/yyyy",
        alias: "datetime"
    });

    $('#txtCvarExpFechaFin').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#txtCvarExpFechaFin').val(date);
        }
    });

    $('#btnCvarExpRecuperar').click(function () {
        mostrarCostoVariable();
    });


    $('#btnCvarExpExportar').click(function () {
        exportarCostoVariable();
    });


    $('#btnMantExpExportar').click(function () {
        exportarMantenimiento();
    });


    $('#rbDespExpOpcReprograma').prop('checked', true);
    $('#rbDespExpMantPdiario').prop('checked', true);
    $('#rbVaguaOpcReprograma').prop('checked', true);
    $('#rbMantExpOpcPdiario').prop('checked', true);
    $('#rbPotenciaOpcReprograma').prop('checked', true);
    $('#rbPotenciaDatosTermo').prop('checked', true);


    $('#rbControlActivo').prop('checked', true);


    $('#DespExpOpcReprograma').val("1");
    $('#VaguaOpcReprograma').val("1");
    $('#PotenciaOpcReprograma').val("1");


    $('#btnVaguaExportar').hide();
    $('#btnVaguaRecuperar').hide();
    $('#btnPotenciaExportar').hide();
    $('#btnPotenciaRecuperar').hide();


    $('#btnPotenciaRecuperar').prop("disabled", true);

    //exportar Costo Variable
    $('#btnDespExpExportar').click(function () {
        exportarDespacho();
    });

    //recuperar control AGC
    $('#btnControlRecuperar').click(function () {
        pintarBusqueda(1);
    });


    $('#btnControlNuevo').click(function () {
        editarControl(0, 1);
    });


    buscar();

});



exportarCostoVariable = function () {
    var repCodi = obtenerReporte();

    if (repCodi == -1) {
        return;
    }


    $.ajax({
        type: 'POST',
        url: controlador + "crearxmlcostovariable",
        data: {
            repCodi: repCodi,
            fecha: $('#txtFecha').val(),
        },
        success: function (resultado) {

            $("#txtLog").val("");

            if (resultado == "-1") {
                mostrarError();
            } else {

                if (resultado != "") {

                    $("#txtLog").val(resultado);
                    $('#tab-container').easytabs('select', '#Log');

                } else {

                }

                descargaMultiple(window.linksXMLCostoVariable)

            }

        },
        error: function () {
            mostrarError();
        }
    });


}


exportarDespacho = function () {
    var lectCodi = 5;
    var bloqueIni = 1;
    var fecha = "";
    var evenClase = 1;

    if ($("#rbDespExpOpcPdiario").is(':checked')) {
        lectCodi = 4;
        bloqueIni = 1;
        fecha = $('#txtFecha').val() + " 00:30";
    }
    else {
        var hora = $("#DespExpOpcReprograma option:selected").text();


        lectCodi = 5;
        bloqueIni = $("#DespExpOpcReprograma").val(); //cambia por hora inicial
        fecha = $('#txtFecha').val() + " " + hora;

    }


    if ($("#rbDespExpMantEjecutado").is(':checked')) {
        evenClase = 1;
    } else {
        evenClase = 2;
    }


    $.ajax({
        type: 'POST',
        url: controlador + "crearxmldespacho",
        data: {
            lectCodi: lectCodi,
            bloqueIni: bloqueIni,
            fecha: fecha,
            evenClase: evenClase
        },
        success: function (resultado) {

            $("#txtLog").val("");

            if (resultado == "-1") {
                mostrarError();
            }
            else {

                if (resultado != "") {
                    $("#txtLog").val(resultado);
                    $('#tab-container').easytabs('select', '#Log');
                }
                else {

                }

                descargaMultiple(window.linksXMLDespacho);

            }

        },
        error: function () {
            mostrarError();
        }
    });

}


exportarMantenimiento = function () {
    var evenClaseCodi = 2;

    if ($("#rbMantExpOpcPdiario").is(':checked')) {
        evenClaseCodi = 2;
    }
    else {
        evenClaseCodi = 1;
    }

    $.ajax({
        type: 'POST',
        url: controlador + "crearxmlmantenimiento",
        data: {
            evenClaseCodi: evenClaseCodi,
            fecha: $('#txtFecha').val()
        },
        success: function (resultado) {

            $("#txtLog").val("");

            if (resultado == "-1") {
                mostrarError();
            }
            else {

                if (resultado != "") {
                    $("#txtLog").val(resultado);
                    $('#tab-container').easytabs('select', '#Log');
                }
                else {

                }


                descargaMultiple(window.linksXMLMantenimiento);

            }

        },
        error: function () {
            mostrarError();
        }
    });


}


descargaMultiple = function (urls) {

    var link = document.createElement('a');

    link.setAttribute('download', null);
    link.style.display = 'none';

    document.body.appendChild(link);

    for (var i = 0; i < urls.length; i++) {
        link.setAttribute('href', urls[i]);
        link.click();
    }
}


obtenerReporte = function () {

    //obtener check
    var repcodi = "";

    $('#CostovariableEscenario input:checked').each(function () {
        repcodi = repcodi + $(this).val() + ",";
    });

    var count = (repcodi.match(/,/g) || []).length;

    if (count == 0) {
        alert("Debe seleccionar un reporte de costo variable...");
        return -1;
    }

    if (count > 1) {
        alert("Debe seleccionar solo un reporte de costo variable...");
        return -1;
    }


    repcodi = repcodi.substring(0, repcodi.length - 1);


    return repcodi;

}


buscar = function () {
    pintarPaginado();
    mostrarListado(1);
    mostrarPotencia("2,4", 20, '#ValoraguaConfiguracion');
    mostrarPotencia("2,4", 20, '#RegulacionasignadaConfiguracionTabHidro');

    mostrarPotencia("3,5", 20, '#RegulacionasignadaConfiguracionTabTermo');
    mostrarCostoVariableAGC();

    mostrarEquipoPropiedad();
    mostrarCostoVariable();


}

pintarPaginado = function () {

    var estado = "S";

    if ($('#rbControlActivo').is(':checked')) {
        estado = 'S';
    } else {
        if ($('#rbControlInactivo').is(':checked')) {
            estado = 'N';
        } else {

            if ($('#rbControlTodos').is(':checked')) {
                estado = 'T';
            } else {
                estado = 'S';
            }

        }
    }


    $.ajax({
        type: 'POST',
        url: controlador + "paginado",
        data: {
            estado: estado
        },
        success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado();
        },
        error: function () {
            mostrarError();
        }
    });
}


pintarBusqueda = function (nroPagina) {

    pintarPaginado();
    mostrarListado(nroPagina);
}

mostrarListado = function (nroPagina) {

    var estado = "S";


    if ($('#rbControlActivo').is(':checked')) {
        estado = 'S';
    } else {
        if ($('#rbControlInactivo').is(':checked')) {
            estado = 'N';
        } else {

            if ($('#rbControlTodos').is(':checked')) {
                estado = 'T';
            } else {
                estado = 'S';
            }
        }
    }


    $('#hfNroPagina').val(nroPagina);


    $.ajax({
        type: 'POST',
        url: controlador + "listadespachoconfiguracion",
        data: {
            estado: estado,
            nroPage: nroPagina
        },
        success: function (evt) {
            $('#DespachoConfiguracion').html(evt);
            /*$('#tabla').dataTable({
                "scrollY": 430,
                "scrollX": true,
                "sDom": 't',
                "ordering": false,
                "iDisplayLength": 50
            });*/

        },
        error: function () {
            mostrarError();
        }
    });
}

mostrarPotencia = function (familia, idOriglectura, control) {

    $.ajax({
        type: 'POST',
        url: controlador + "listapotenciaconfiguracion",
        data: {
            familia: familia,
            idOriglectura: idOriglectura,
            control: control
        },
        success: function (evt) {
            $(control).html(evt);
            /*$('#tabla').dataTable({
                "scrollY": 430,
                "scrollX": true,
                "sDom": 't',
                "ordering": false,
                "iDisplayLength": 50,
                "bDestroy": true
            });*/

        },
        error: function () {
            mostrarError();
        }
    });
}


mostrarCostoVariableAGC = function () {

    $.ajax({
        type: 'POST',
        url: controlador + "listacostovariableagc",
        data: {

        },
        success: function (evt) {
            $('#CostovariableConfiguracion').css("width", $('#mainLayout').width() + "px");
            $('#CostovariableConfiguracion').html(evt);
            /*$('#tablacvagc').dataTable({
                "scrollY": 430,
                "scrollX": true,
                "sDom": 't',
                "ordering": false,
                "iDisplayLength": 50,
                "bDestroy": true
            });*/

        },
        error: function () {
            mostrarError();
        }
    });
}


mostrarEquipoPropiedad = function () {

    $.ajax({
        type: 'POST',
        url: controlador + "listaequipopropiedad",
        data: {
            fecha: $('#txtFecha').val()
        },
        success: function (evt) {
            $('#MantenimientoUnidades').css("width", $('#mainLayout').width() + "px");
            $('#MantenimientoUnidades').html(evt);
            /*$('#tabla').dataTable({
                "scrollY": 430,
                "scrollX": true,
                "sDom": 't',
                "ordering": false,
                "iDisplayLength": 50,
                "bDestroy": true
            });*/

        },
        error: function () {
            mostrarError();
        }
    });
}


mostrarCostoVariable = function () {

    $.ajax({
        type: 'POST',
        url: controlador + "listacostovariable",
        data: {
            fechaIni: $('#txtCvarExpFechaIni').val(),
            fechaFin: $('#txtCvarExpFechaFin').val()
        },
        success: function (evt) {
            $('#CostovariableEscenario').html(evt);
            /* $('#tabla').dataTable({
                 "scrollY": 430,
                 "scrollX": true,
                 "sDom": 't',
                 "ordering": false,
                 "iDisplayLength": 50,
                 "bDestroy": true
             });*/

        },
        error: function () {
            mostrarError();
        }
    });
}


grabarPtoMedicion = function (control) {
    $.ajax({
        type: 'POST',
        url: controlador + "grabarpmedicion",
        dataType: 'json',
        data: $('#frmEditarPMedicion').serialize(),
        success: function (result) {
            if (result >= 1) {
                $('#popUpEditarPMedicion').bPopup().close();

                var n = control.indexOf("Termo");

                if (n >= 0) {
                    mostrarPotencia("3,5", 20, '#RegulacionasignadaConfiguracionTabTermo');
                }
                else {
                    mostrarPotencia("2,4", 20, '#ValoraguaConfiguracion');
                    mostrarPotencia("2,4", 20, '#RegulacionasignadaConfiguracionTabHidro');
                }
            }
            if (result == -1) {
                mostrarError();
            }
        },
        error: function () {
            mostrarError();
        }
    });


}



grabarCVariable = function () {

    $('#hfGrupoCodi').val($('#cbGrupoCodi').val());

    $.ajax({
        type: 'POST',
        url: controlador + "grabarcvariable",
        dataType: 'json',
        data: $('#frmEditarCostoVariableAGC').serialize(),
        success: function (result) {
            if (result >= 1) {
                $('#popUpEditarCVariable').bPopup().close();
                mostrarCostoVariableAGC();
            }
            if (result == -1) {
                mostrarError();
            }
        },
        error: function () {
            mostrarError();
        }
    });

}


validarControl = function () {
    var mensaje = "";

    if ($("#rbAgccTipoC").is(':checked')) {
        //revisar B2, B3
        var b2 = $("#txtAgccB2").val().trim();
        var b3 = $("#txtAgccB3").val().trim();

        if (b2 + 'X' == 'X') {
            mensaje = mensaje + "Debe ingresar B2\n";
        }

        if (b3 + 'X' == 'X') {
            mensaje = mensaje + "Debe ingresar B3\n";
        }

        $("#hfAgccTipo").val('C');

    } else {
        $("#hfAgccTipo").val('P');
    }

    //puntomedicodi
    $("#hfPtomediCodi").val($("#cbPtomediCodi").val());

    //estado
    if ($("#rbAgccValidoS").is(':checked')) {
        $("#hfAgccValido").val('S');
    } else {
        $("#hfAgccValido").val('N');
    }

    //descripción
    var descrip = $("#txtAgccDescrip").val().trim();

    if (descrip + 'X' == 'X') {
        mensaje = mensaje + "Debe ingresar Descripción\n";
    }


    return mensaje;

}


validarControlPunto = function () {
    var mensaje = "";

    var codigos = "";
    var datos = "";

    if ($("#rbAgccTipoC").is(':checked')) {
        //control centralizado
        var cad = "";
        var tbl = document.getElementById("tablaControlPunto");
        if (tbl != null) {

            if (tbl.rows.length == 1) {
                mensaje += "Debe seleccionar elementos." + "\n" + "No se puede continuar" + "\n";

            } else {

                for (var i = 1; i < tbl.rows.length; i++) {
                    var id = tbl.rows[i].cells[0].innerHTML;

                    var codigo = $("#cbPtomedidesc" + id).val();

                    if (codigos.indexOf(codigo + ",") >= 0) {

                        mensaje += "Punto no puede repetirse." + "\n" + "No se puede continuar" + "\n";
                        break;

                    } else {

                        if ($("#cbPtomedidesc" + id).val() == "" || $("#cbPtomedidesc" + id).val() == null) {
                            mensaje += "Debe seleccionar Punto." + "\n" + "No se puede continuar" + "\n";
                            break;
                        }

                    }


                    var b2 = $("#txtAgccB2").val().trim();
                    var b3 = $("#txtAgccB3").val().trim();


                    if (b2 + 'X' == 'X') {
                        mensaje = mensaje + "Debe ingresar B2\n" + "No se puede continuar" + "\n";
                        break;
                    }

                    if (b3 + 'X' == 'X') {
                        mensaje = mensaje + "Debe ingresar B3\n" + "No se puede continuar" + "\n";
                        break;
                    }


                    codigos += codigo + ",";

                }

            }
        }


    } else {
        //control proporcional

        var cad = "";
        var tbl = document.getElementById("tablaControlPunto");
        if (tbl != null) {

            if (tbl.rows.length == 1) {
                mensaje += "Debe seleccionar elementos." + "\n" + "No se puede continuar" + "\n";

            } else {


                for (var i = 1; i < tbl.rows.length; i++) {
                    var id = tbl.rows[i].cells[0].innerHTML;

                    var codigo = $("#cbEquiabrev" + id).val();

                    if (codigos.indexOf(codigo + ",") >= 0) {

                        mensaje += "Equipo no puede repetirse." + "\n" + "No se puede continuar" + "\n";
                        break;

                    } else {

                        if ($("#cbEquiabrev" + id).val() == "" || $("#cbEquiabrev" + id).val() == null) {
                            mensaje += "Debe seleccionar Equipo." + "\n" + "No se puede continuar" + "\n";
                            break;
                        }

                    }

                    var b2 = $('#txtAgccB2_' + id).val().trim();
                    var b3 = $('#txtAgccB3_' + id).val().trim();

                    if (b2 + 'X' == 'X') {
                        mensaje = mensaje + "Debe ingresar B2\n" + "No se puede continuar" + "\n";
                        break;
                    }

                    if (b3 + 'X' == 'X') {
                        mensaje = mensaje + "Debe ingresar B3\n" + "No se puede continuar" + "\n";
                        break;
                    }


                    datos += codigo + ",[" + b2 + "]," + b3 + "\n";


                    codigos += codigo + ",";

                }

            }
        }

    }

    return mensaje;
}


grabarControl = function () {
    //cabecera
    var mensaje = validarControl();
    var mensajeCP = validarControlPunto();

    if ((mensaje == "") && (mensajeCP == "")) {

        //grabar control
        $.ajax({
            type: 'POST',
            url: controlador + "grabarcontrol",
            dataType: 'json',
            data: $('#frmControl').serialize(),
            success: function (result) {
                if (result != -1) {

                    //mostrarExito();
                    $('#hfAgccCodi').val(result);

                    //grabar detalle
                    grabarControlPunto(result);

                    //cerrar popup
                    $('#popUpEditarControl').bPopup().close();

                    //actualizar grid
                    mostrarListado(1);

                } else {
                    mostrarError();
                }
            },
            error: function () {
                mostrarError();
            }
        });
    } else {
        alert(mensaje + "\n" + mensajeCP);
    }

}

grabarControlPunto = function (result) {

    var nroError = 0;
    var idAgccCodi = result;

    $.ajax({
        type: 'POST',
        url: controlador + "eliminarcontrolpunto",
        dataType: 'json',
        cache: false,
        data: {
            idAgccCodi: idAgccCodi
        },
        success: function (result) {

            if (result != -1) {


                var codigos = "";
                var datos = "";

                if ($("#rbAgccTipoC").is(':checked')) {
                    //control centralizado
                    var cad = "";
                    var tbl = document.getElementById("tablaControlPunto");
                    if (tbl != null) {

                        for (var i = 1; i < tbl.rows.length; i++) {
                            var id = tbl.rows[i].cells[0].innerHTML;
                            var ptomediCodi = $("#cbPtomedidesc" + id).val();
                            
                            datos = datos + "," + "C" + "," + ptomediCodi + "," + "-1" + "," + "" + "," + "";//+ ",";
                            
                            /*
                            $.ajax({
                                type: 'POST',
                                url: controlador + "grabarcontrolpunto1",
                                dataType: 'json',
                                cache: false,
                                data: {
                                    tipoControl: "C",
                                    idControl: idAgccCodi,
                                    ptomediCodi: ptomediCodi,
                                    equiCodi: -1,
                                    b2: "",
                                    b3: ""
                                },
                                success: function (resultado) {
                                    if (resultado == 1) {
                                        nroError++;
                                    } else {
                                        nroError++;
                                    }

                                },
                                error: function () {
                                    nroError++;
                                }
                            });
                            */

                        }
                    }

                } else {

                    //control proporcional
                    var cad = "";
                    var tbl = document.getElementById("tablaControlPunto");
                    if (tbl != null) {


                        for (var i = 1; i < tbl.rows.length; i++) {
                            var id = tbl.rows[i].cells[0].innerHTML;
                            var equiCodi = $("#cbEquiabrev" + id).val();
                            var b2 = $('#txtAgccB2_' + id).val().trim();
                            var b3 = $('#txtAgccB3_' + id).val().trim();

                            datos = datos + "," + "P" + "," + "-1" + "," + equiCodi + "," + b2 + "," + b3;//+ ",";
                            /*
                            $.ajax({
                                type: 'POST',
                                url: controlador + "grabarcontrolpunto1",
                                dataType: 'json',
                                cache: false,
                                data: {
                                    tipoControl: "P",
                                    idControl: idAgccCodi,
                                    ptomediCodi: -1,
                                    equiCodi: equiCodi,
                                    b2: b2,
                                    b3: b3
                                },
                                success: function (resultado) {
                                    if (resultado == 1) {
                                        nroError++;
                                    } else {
                                        nroError++;
                                    }

                                },
                                error: function () {
                                    nroError++;
                                }
                            });
                            */

                        }


                    }


                }

                $.ajax({
                    type: 'POST',
                    url: controlador + "grabarcontrolpunto",
                    dataType: 'json',
                    cache: false,
                    data: {
                        data: datos,
                        idControl: idAgccCodi
                    },
                    success: function (resultado) {
                        if (resultado == 1) {
                            nroError++;
                        } else {
                            nroError++;
                        }

                    },
                    error: function () {
                        nroError++;
                    }
                });

                
            } else {
                mostrarError();
            }
        },
        error: function () {
            mostrarError();
        }
    });


    if (nroError > 0)
        mostrarError();

}


editarControl = function (id, accion) {

    $.ajax({
        type: 'POST',
        url: controlador + "editarcontrol",
        cache: false,
        data: {
            id: id,
            accion: accion
        },
        success: function (evt) {
            $('#editarControl').html(evt);
            $('#mensaje').css("display", "none");
            setTimeout(function () {
                $('#popUpEditarControl').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            },
                50);


            configurarControl();


            $('#btnGrabarControl').click(function () {
                grabarControl();
            });

            $('#btnAgregar').click(function () {
                agregarFila();
            });


        },
        error: function () {
            mostrarError();
        }
    });

}


desactivarControl = function (id) {

    if (confirm('¿Desea cambiar el estado a "No activo"?')) {

        $.ajax({
            type: 'POST',
            url: controlador + "desactivarControl",
            dataType: 'json',
            cache: false,
            data: { id: id },
            success: function (resultado) {
                if (resultado == 1) {
                    mostrarListado(1);
                } else {
                    mostrarError();
                }


            },
            error: function () {
                mostrarError();
            }
        });

    }
}


configurarControl = function () {


    if ($('#hfAgccTipo').val() == 'P')
        $("#rbAgccTipoP").prop('checked', true);
    else
        $("#rbAgccTipoC").prop('checked', true);



    if ($('#hfAgccValido').val() == 'N')
        $("#rbAgccValidoN").prop('checked', true);
    else
        $("#rbAgccValidoS").prop('checked', true);


    $("#rbDespExpOpcPdiario").is(':checked');

    if ($('#hfAgccTipo').val() == 'P')
        configuraControlElegido('P');
    else
        configuraControlElegido('C');

    if ($('#hfPtomediCodi').val() + "*" != "*") {
        $('#cbPtomediCodi').val($('#hfPtomediCodi').val());
    }


    //control punto    
    var cad = "";
    var tbl = document.getElementById("tablaControlPunto");
    if (tbl != null) {
        for (var i = 1; i < tbl.rows.length; i++) {
            var id = tbl.rows[i].cells[0].innerHTML;
            var idCodigoPunto = tbl.rows[i].cells[1].innerHTML;
            var idPtomedicodi = tbl.rows[i].cells[2].innerHTML;
            var idEquicodi = tbl.rows[i].cells[3].innerHTML;

            cad += idCodigoPunto + ",*" + idPtomedicodi + "*," + idEquicodi + "," + $('#cbPtomedidesc' + (i - 1)).val() + "\r\n";

            $('#cbPtomedidesc' + (i - 1)).val(idPtomedicodi);
            $('#cbEquiabrev' + (i - 1)).val(idEquicodi);


        }
    }

    ocultarColumna();
}


ocultarColumna = function () {


    var tipoControl = $('#rbAgccTipoC').is(':checked') ? "C" : "P";
    var tbl = document.getElementById("tablaControlPunto");

    if (tbl != null) {
        for (var i = -1; i < tbl.rows.length; i++) {

            if (tipoControl == "C") {

                //ocultar columna Equicodi, B2, B3                                
                $('#equipo' + i).hide();
                $('#txtAgccB2' + i).hide();
                $('#txtAgccB3' + i).hide();
                //mostrar columna Ptomedicodi
                $('#punto' + i).show();

            } else {

                //mostrar columna Equicodi, B2, B3
                $('#equipo' + i).show();
                $('#txtAgccB2' + i).show();
                $('#txtAgccB3' + i).show();
                //ocultar columna Ptomedicodi
                $('#punto' + i).hide();

            }

        }

    }


}


agregarFila = function () {

    var filas = 0;

    var tbl = document.getElementById("tablaControlPunto");
    if (tbl != null) {
        filas = tbl.rows.length - 1;

        var fila1 = tbl.rows[filas].cells[0].innerHTML;



        if (isNaN(parseInt(fila1)))
            fila1 = -1;

        fila1++;


        var comboPunto = "cbPtomedidesc" + fila1;
        var comboEquipo = "cbEquiabrev" + fila1;
        var punto = "punto" + fila1;
        var equipo = "equipo" + fila1;
        var b2 = "txtAgccB2" + fila1;
        var b3 = "txtAgccB3" + fila1;



        var a = "";
        a += "                        <tr id='" + fila1 + "'>";
        a += "                            <td style='display: none'>" + fila1 + "</td>";
        a += "                            <td style='display: none'>-1</td>";
        a += "                            <td style='display: none'></td>";
        a += "                            <td style='display: none'></td>";
        a += "                            <td id='" + punto + "'>";
        a += "                                <select id='" + comboPunto + "' name='PtomediCodi' style='width:350px'>";
        a += "                                    @foreach (var item in Model.ListaMePtomedicion)";
        a += "                                    {";
        a += "                                        <option value='@item.Ptomedicodi'></option>";
        a += "                                    }";
        a += "                                </select>";
        a += "                            </td>";
        a += "                            <td id='" + equipo + "'>";
        a += "                                <select id='" + comboEquipo + "' name='EquiCodi' style='width:250px'>";
        a += "                                    @foreach (var item in Model.ListaEqEquipo)";
        a += "                                    {";
        a += "                                        <option value='@item.Equicodi'></option>";
        a += "                                    }";
        a += "                                </select>";
        a += "                            </td>";
        a += "                            <td id='" + b2 + "'>";
        a += "                                <input type='text' id='txtAgccB2_" + fila1 + "' maxlength='30' name='AgccB2' value='' />";
        a += "                            </td>";
        a += "                            <td id='" + b3 + "'>";
        a += "                                <input type='text' id='txtAgccB3_" + fila1 + "' maxlength='30' name='AgccB3' value='' />";
        a += "                            </td>";
        a += "                                <td>";
        a += "                                    <a href='JavaScript:eliminarPuntoControl(" + fila1 + ")' title='Eliminar...'>";
        a += "                                          <img src='+ siteRoot + 'Content/Images/Trash.png' />"
        a += "                                    </a>";
        a += "                                </td>";
        a += "                        </tr>";


        $('#tablaControlPunto').append(a);



        poblarCombo('cbPuntoBase', comboPunto);
        poblarCombo('cbEquipoBase', comboEquipo);

        ocultarColumna();

    }

}


poblarCombo = function (controlOrigen, controlDestino) {


    var idOrigen = document.getElementById(controlOrigen);
    var opciones = idOrigen.innerHTML;

    var idDestino = document.getElementById(controlDestino);

    idDestino.innerHTML = opciones;

}

eliminarPuntoControl = function (id) {
    var row = document.getElementById(id);
    row.parentNode.removeChild(row);

}


configuraControlElegido = function (tipoControl) {


    if (tipoControl == "C") {
        $("#ptonomb").hide();
        $("#cbPtomediCodi").hide();

        $("#lblAgccB2").show();
        $("#txtAgccB2").show();

        $("#lblAgccB3").show();
        $("#txtAgccB3").show();
    } else {
        $("#ptonomb").show();
        $("#cbPtomediCodi").show();

        $("#lblAgccB2").hide();
        $("#txtAgccB2").hide();

        $("#lblAgccB3").hide();
        $("#txtAgccB3").hide();

    }

    ocultarColumna();

}


editarPMedicion = function (id, accion, control) {


    $.ajax({
        type: 'POST',
        url: controlador + "editarpmedicion",
        cache: false,
        data: {
            id: id,
            accion: accion
        },
        success: function (evt) {
            $('#editarPMedicion').html(evt);
            $('#mensaje').css("display", "none");
            setTimeout(function () {
                $('#popUpEditarPMedicion').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            },
                50);


            $('#btnGrabarPtoMedicion').click(function () {
                grabarPtoMedicion(control);
            });


        },
        error: function () {
            mostrarError();
        }
    });

}


editarCVariable = function (id, accion) {

    $.ajax({
        type: 'POST',
        url: controlador + "editarcostovariableagc",
        cache: false,
        data: {
            id: id,
            accion: accion
        },
        success: function (evt) {
            $('#editarCVariable').html(evt);
            $('#mensaje').css("display", "none");
            setTimeout(function () {
                $('#popUpEditarCVariable').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);

            //asignar grupocodi hf a combo
            $('#cbGrupoCodi').val($('#hfGrupoCodi').val());

            $('#btnGrabarCVariable').click(function () {
                grabarCVariable();
            });



        },
        error: function () {
            mostrarError();
        }
    });


}


mostrarError = function () {
    alert('Ha ocurrido un error.');
}


cargaDeArchivo = function (uploader,
    btnId,
    fileInfo,
    container,
    nombrearchivo,
    hfArchivo,
    idDIV,
    idTabla,
    idRbPdiario,
    idRbReprograma,
    idLbReprog,
    idOpcionDatos) {


    uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: btnId,
        container: document.getElementById(container),
        url: controlador + 'Upload',
        flash_swf_url: '~/Content/Scripts/Moxie.swf',
        silverlight_xap_url: '~/Content/Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: '100mb',
            mime_types: [
                { title: "Archivos csv .csv", extensions: "csv" }
            ]
        },
        init: {
            PostInit: function () {

            },
            FilesAdded: function (up, files) {
                if (uploader.files.length == 2) {
                    uploader.removeFile(uploader.files[0]);
                }
                plupload.each(files,
                    function (file) {
                        loadInfoFile(fileInfo, file.name);
                        $(hfArchivo).val("S");
                    });
                up.refresh();
                uploader.settings.multipart_params = {
                    "nombreArchivo": $(fileInfo).text()
                }
                uploader.start();
            },
            UploadComplete: function (up, file) {
                $(nombrearchivo).val($(fileInfo).text());

                var fileFinal = $(fileInfo).text();

                //carga de archivo a matriz
                $.ajax({
                    type: 'POST',
                    url: controlador + "/cargarMatriz",
                    dataType: 'json',
                    data: {
                        file: fileFinal,
                        idTabla: idTabla,
                        idOpcionDatos: idOpcionDatos,
                        fecha: $('#txtFecha').val(),
                        esHidro: $('#rbPotenciaDatosHidro').is(':checked')
                    },
                    success: function (result) {

                        //carga de matriz en ID
                        if (result == "-1") {
                            mostrarError();
                        } else {

                            $(idDIV).css("width", $('#mainLayout').width() + "px") //.css("width", "20 " + "px");
                            $(idDIV).html(result);
                            $('#' + idTabla).dataTable({
                                "scrollY": 430,
                                "scrollX": true,
                                "sDom": 't',
                                "ordering": false,
                                "iDisplayLength": 50
                            });

                            var fila = document.getElementById(idTabla).rows.length - 1;

                            //ubica control
                            if (fila == 48) {
                                if ($("#" + idRbReprograma).is(':checked')) {
                                    $("#" + idLbReprog).val("1");

                                }

                            } else {

                                $("#" + idRbReprograma).prop('checked', true);

                                $("#" + idLbReprog).val(48 - fila + 1);

                            }

                            switch (idOpcionDatos) {
                                case 'Vagua':
                                    window.location = controlador + "descargarxmlvaloragua";
                                    break;
                                case 'PActiva':
                                    if ($('#rbPotenciaDatosHidro').is(':checked'))
                                        window.location = controlador + "descargarxmlpactivahidro";
                                    else
                                        window.location = controlador + "descargarxmlpactivatermo";

                                    break;

                            }


                        }

                    },
                    error: function () {
                        mostrarError();
                    }
                });


            },
            Error: function (up, err) {
                loadValidacionFile(fileInfo, err.code + "-" + err.message);
            }
        }
    });
    uploader.init();

}


function loadValidacionFile(fileInfo, mensaje) {
    $(fileInfo).html(mensaje);
}


function loadInfoFile(fileInfo, fileName) {
    $(fileInfo).html(fileName);
}