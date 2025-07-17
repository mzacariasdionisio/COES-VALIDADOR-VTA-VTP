var controlador = siteRoot + "GMM/ServiciosCompl/";
var controladorAgente = siteRoot + "GMM/Agentes/";
$( /**
   * Llamadas iniciales
   * @returns {} 
   */
    $(document).ready(function () {

        $('#tab-container').easytabs({
            animate: false
        });

        $('#anho').change(function () {
            pintarTodo();
        });

        $('#mes').change(function () {
            pintarTodo();
        });

        $('#btnGrabar').click(function () {
            fnGrabarPrimerMes();
        });

        $('#btnGrabarSegundoMes').click(function () {
            fnGrabarSegundoMes();
        });
        $('#btnCancelar').click(function () {
            fnCancelar();
        });

        $("#grabarCelda").change(function () {
            document.getElementById("btnGrabar").disabled = this.checked;
            document.getElementById("btnCancelar").disabled = this.checked;
        });

        setAnio("anho");

        var vd = new Date();
        var vanho = vd.getFullYear();
        var vmes = vd.getMonth() + 1;

        $('#anho').val(vanho);
        $('#mes').val(vmes);
        //pintarInsumos();
        pintarTodo();
        cambioPrimerMes();
        cambioSegundoMes();


    }));

function pintarTodo() {
    //pintarEmpresas();
    //pintarEntregas();
    //pintarInflexibilidades();
    //pintarRecaudaciones();
    pintarInsumos();
    pintarPrimerMes();
    // ActualizarCamposInsumosGen();
};

var gevent;

var pintarInsumos =
    function () {

        $.ajax({
            type: "POST",
            url: controlador + 'ListarInsumos',
            data: {
                anho: $("#anho").val(),
                mes: $("#mes").val()
            },
            success: function (evt) {
                // $('#listadoInsumos').css("width", $('#mainLayout').width() + "px");
                $('#listadoInsumos').html(evt);
                $('#tablaInsumos').dataTable({
                    //"retrieve": true,
                    //"ordering": true,
                    "paging": false,
                    "scrollY": 200,
                    "scrollX": true,
                    //"scrollCollapse": true,
                    "fixedColumns": true
                });
                cambioSegundoMes();
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                mostrarError('No se puedo ejecutar la carga de empresas.');
            }
        });

    };
var pintarPrimerMes =
    function () {

        $.ajax({
            type: "POST",
            url: controlador + 'ListaPrimerMes',
            data: {
                anho: $("#anho").val(),
                mes: $("#mes").val()
            },
            success: function (evt) {
                //  $('#divTablaInsumesGen').css("width", $('#mainLayout').width() + "px");
                $('#divTablaInsumesGen').html(evt);
                $('#tablaInsumosGen').dataTable({
                    //"retrieve": true,
                    //"ordering": true,
                    "paging": false,
                    "scrollY": 150,
                    "scrollX": true,
                    //"scrollCollapse": true,
                    "fixedColumns": true
                });
                cambioPrimerMes();
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                mostrarError('No se puedo ejecutar la carga de empresas.');
            }
        });

    };

function ActualizarCamposInsumosGen() {
    var panio = $('#anho').val();
    var pmes = $('#mes').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CamposInsumosGen',
        data: {
            anio: panio,
            mes: pmes
        },
        dataType: 'json',
        success: function (data) {
            var jsonData = JSON.parse(data);
            $('#mreserva').val(parseFloat(jsonData.MRESERVA).toFixed(2));
            $('#sscc').val(parseFloat(jsonData.SSCC).toFixed(2));
            $('#tinflex').val(parseFloat(jsonData.TINFLEX).toFixed(2));
            $('#texceso').val(parseFloat(jsonData.TEXCESO).toFixed(2));

        },
        error: function () {
            mostrarError('Ocurrió un problema al momento de actualizar la sección de procesamiento.');
        }
    });
}

var pintarEmpresas =
    function () {

        $.ajax({
            type: "POST",
            url: controlador + 'ListarComplementarios',
            data: {
                anho: $("#anho").val(),
                mes: $("#mes").val()
            },
            success: function (evt) {

                $('#listadoComplementarios').css("width", $('#mainLayout').width() + "px");
                $('#listadoComplementarios').html(evt);
                $('#tablaComplementarios').dataTable({
                    "ordering": true,
                    "paging": false,
                    "scrollY": 110,
                    "scrollX": true,
                    "scrollCollapse": true,
                    "fixedColumns": true
                });

                mostrarMensaje("Empresas cargadas.");
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                mostrarError('No se puedo ejecutar la carga de empresas.');
            }
        });

    };

var pintarEntregas =
    function () {

        $.ajax({
            type: "POST",
            url: controlador + 'ListarEntregas',
            data: {
                anho: $("#anho").val(),
                mes: $("#mes").val()
            },
            success: function (evt) {

                $('#listadoEntregas').css("width", $('#mainLayout').width() + "px");
                $('#listadoEntregas').html(evt);
                $('#tablaEntregas').dataTable({
                    "ordering": true,
                    "paging": false,
                    "scrollY": 110,
                    "scrollX": true,
                    "scrollCollapse": true,
                    "fixedColumns": true
                });

                mostrarMensaje("Empresas cargadas.");
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                mostrarError('No se puedo ejecutar la carga de empresas.');
            }
        });

    };

var pintarInflexibilidades =
    function () {

        $.ajax({
            type: "POST",
            url: controlador + 'ListarInflexibilidades',
            data: {
                anho: $("#anho").val(),
                mes: $("#mes").val()
            },
            success: function (evt) {

                $('#listadoInflexibilidades').css("width", $('#mainLayout').width() + "px");
                $('#listadoInflexibilidades').html(evt);
                $('#tablaInflexibilidades').dataTable({
                    "ordering": true,
                    "paging": false,
                    "scrollY": 110,
                    "scrollX": true,
                    "scrollCollapse": true,
                    "fixedColumns": true
                });

                mostrarMensaje("Empresas cargadas.");
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                mostrarError('No se puedo ejecutar la carga de empresas.');
            }
        });

    };

var pintarRecaudaciones =
    function () {

        $.ajax({
            type: "POST",
            url: controlador + 'ListarRecaudaciones',
            data: {
                anho: $("#anho").val(),
                mes: $("#mes").val()
            },
            success: function (evt) {

                $('#listadoRecaudaciones').css("width", $('#mainLayout').width() + "px");
                $('#listadoRecaudaciones').html(evt);
                $('#tablaRecaudaciones').dataTable({
                    "ordering": true,
                    "paging": false,
                    "scrollY": 110,
                    "scrollX": true,
                    "scrollCollapse": true,
                    "fixedColumns": true
                });

                mostrarMensaje("Empresas cargadas.");
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                mostrarError('No se puedo ejecutar la carga de empresas.');
            }
        });

    };


var isNumber =
    function (o) {
        //return typeof o === 'number' && isFinite(o);

        var regexp = /^\d+\.\d{0,2}$/;
        regexp.test(o);
    };

function setAnio(aselect) {

    var d = new Date();
    var n = d.getFullYear();
    var select = document.getElementById(aselect);
    for (var i = 2019; i <= n + 1; i++) {
        var opc = document.createElement("option");
        opc.text = i;
        opc.value = i;
        select.add(opc)
    }
}

var fnGrabarTodosDat =
    function () {

        $('#tablaComplementarios input[id=LISTDINSVALOR]').each(function (e) {
            fnGrabar(this, 1);
        });
        $('#tablaEntregas input[id=LISTDINSVALOR]').each(function (e) {
            fnGrabar(this, 2);
        });
        $('#tablaInflexibilidades input[id=LISTDINSVALOR]').each(function (e) {
            fnGrabar(this, 3);
        });
        $('#tablaRecaudaciones input[id=LISTDINSVALOR]').each(function (e) {
            fnGrabar(this, 4);
        });
    };

function fnGrabarPrimerMes() {

    $("#tablaInsumosGen > tbody > tr").each(function () {

        fnGrabarInsumo($("#" + $(this).data('code') + "_mreserva"), 5, $(this).data('code'), $(this).data('codepart'));
        fnGrabarInsumo($("#" + $(this).data('code') + "_sscc"), 6, $(this).data('code'), $(this).data('codepart'));
        fnGrabarInsumo($("#" + $(this).data('code') + "_tinflex"), 7, $(this).data('code'), $(this).data('codepart'));
        fnGrabarInsumo($("#" + $(this).data('code') + "_texceso"), 8, $(this).data('code'), $(this).data('codepart'));

    });



    // mostrarMensaje("La lista de insumos fueron guardados correctamente.");
};
function fnGrabarSegundoMes() {
    $('#tablaInsumos input[id=ENTREGAS]').each(function (e) {
        fnGrabar(this, 2);
    });

    $('#tablaInsumos input[id=SC]').each(function (e) {
        fnGrabar(this, 1);
    });

    $('#tablaInsumos input[id=INFLEX]').each(function (e) {
        fnGrabar(this, 3);
    });

    $('#tablaInsumos input[id=RECAU]').each(function (e) {
        fnGrabar(this, 4);
    });
};
var fnGrabarCelda =
    function (thiss, tipo) {

        if ($("#grabarCelda:checked").length == 0)
            return false;

        fnGrabar(thiss, tipo);
    };

var fnGrabar =
    function (thiss, tipo) {
        $.ajax({
            type: 'POST',
            url: controlador + 'Grabar',
            data: {
                anho: $("#anho").val(),
                mes: $("#mes").val(),
                codigoEmpresa: $(thiss).data('empgcodi'),
                codigoParticipante: $(thiss).data('empgcodi'),
                valor: $(thiss).val(),
                tipo: tipo
            },
            dataType: 'json',
            success: function (result) {

                $(thiss).parent().find('span').remove();

                if (result.success) {
                    mostrarMensaje("Insumo guardado correctamente.");
                    $("#divMensajeSegundoMes").hide();
                }
                else {
                    //alert(result.message);
                    //$(thiss).parent().append('<span style="color:red;">' + result.message + '</span>');
                    mostrarError("Error al guardas el documento.");
                }

            },
            error: function () {
                mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
            }
        });
    };


var fnGrabarInsumo =
    function (thiss, tipo, codigoEmpresa, codigoParticipante) {
        $.ajax({
            type: 'POST',
            url: controlador + 'Grabar',
            data: {
                anho: $("#anho").val(),
                mes: $("#mes").val(),
                codigoEmpresa: codigoEmpresa,
                codigoParticipante: codigoParticipante,
                valor: $(thiss).val(),
                tipo: tipo
            },
            dataType: 'json',
            success: function (result) {

                $(thiss).parent().find('span').remove();

                if (result.success) {
                    $("#divMensajePrimerMes").hide();

                    $(thiss).attr("disabled", "disabled");
                    mostrarMensaje("Registro guardado correctamente.");
                }
                else {
                    //alert(result.message);
                    //$(thiss).parent().append('<span style="color:red;">' + result.message + '</span>');
                    mostrarError("Error al guardas el documento.");
                }

            },
            error: function () {
                mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
            }
        });
    };

var fnCancelar =
    function () {

    };

function mostrarMensaje(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-message');
}

function mostrarError(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-alert');
}


/* MANTENIMIENTO PARTICIPANTE*/
function modificarAgentes(id) {

    $("#" + id + "_mreserva").removeAttr("disabled");
    $("#" + id + "_sscc").removeAttr("disabled");
    $("#" + id + "_tinflex").removeAttr("disabled");
    $("#" + id + "_texceso").removeAttr("disabled");

}
function eliminarAgentes(id) {

    if (confirm('¿Está seguro de realizar esta acción?')) {
        fnEliminarInsumo($("#" + id + "_mreserva"), 5, id);
        fnEliminarInsumo($("#" + id + "_sscc"), 6, id);
        fnEliminarInsumo($("#" + id + "_tinflex"), 7, id);
        fnEliminarInsumo($("#" + id + "_texceso"), 8, id);
    };
}
function fnEliminarInsumo(thiss, tipo, codigoEmpresa) {

    $.ajax({
        type: 'POST',
        url: controlador + 'Eliminar',
        data: {
            anho: $("#anho").val(),
            mes: $("#mes").val(),
            codigoEmpresa: codigoEmpresa,
            valor: $(thiss).val(),
            tipo: tipo
        },
        dataType: 'json',
        success: function (result) {

            //$(thiss).parent().find('span').remove();

            if (result.success) {
                //$('#popupEdicion').bPopup().close();
                //alert('Registro actualizado correctamente.');

                //pintarBusqueda();
                //limpiarDatosRegistroSvrm();
                //$(thiss).parent().append('<span style="color:blue;">Guardado</span>');
                //$(thiss).parent().closest('tr').remove();
                pintarPrimerMes();
                mostrarMensaje("Registro eliminado correctamente.");
            }
            else {
                //alert(result.message);
                //$(thiss).parent().append('<span style="color:red;">' + result.message + '</span>');
                mostrarError("Error al eliminar el registro.");
            }

        },
        error: function () {
            mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
        }
    });

};

function fnPopUpEmpresa() {
    $("#empresaBuscar").val('');
    $("#listadoEmpresas").html("");
    $("#popupBuscarEmpresa").bPopup({
        autoClose: false
    });
}
function cambioPrimerMes() {
    $('#tablaInsumosGen').on('change', 'input', function () {
        $("#divMensajePrimerMes").show();
    });
}
function cambioSegundoMes() {
    $('#tablaInsumos').on('change', 'input', function () {
        $("#divMensajeSegundoMes").show();
    });

}
function buscarEmpresa() {

    var empresaIngresada = document.getElementById("empresaBuscar").value;
    var tipoEmpresaId = $('input:radio[name=tipoEmpresa]:checked').val(); // $('#tipoParticipanteEdit').val(); //

    $.ajax({
        type: 'POST',
        datatype: 'json',
        url: controlador + "ListarEmpresasParticipante",
        data: {
            empresa: empresaIngresada
        },
        success: function (result) {
            $('#listadoEmpresas').css("width", "90%");
            $('#listadoEmpresas').html(result);
            $('#tablaListaEmpresas').dataTable({
                "filter": false,
                "ordering": true,
                "paging": false,
                "scrollY": 150,
                "scrollX": true,
                "bDestroy": true,
                "autoWidth": false,
                "columnDefs": [
                    { "width": "20%", "targets": 0 },
                    { "width": "80%", "targets": 1 }
                ]
            });
            $('#btnSeleccionarEmpresa').click(function () {
                seleccionarEmpresa();
            });
        },
        error: function () {
            mostrarErrorpe("Ocurrió un error al obtener la Lista de Empresas.");
        }
    });
}
function seleccionarEmpresa() {
    var empresa = $('input:radio[name=codEmpresa]:checked').val();
    if (empresa == "" || empresa == null) {
        mostrarAlertape('No ha seleccionado una empresa');
        return false;
    }
    var datos = empresa.split('/');
    var codigo = datos[0];
    var nombre = datos[1];
    var participante = datos[3];
    var repetido = false;
    //document.getElementById("documentoEdit").value = datos[2];
    $("#tablaInsumosGen > tbody > tr").each(function () {
        if ($(this).data('code') == codigo) {
            repetido = true;
            alert("La empresa participante ya se encuentra agregado");
        }
    });

    if (repetido == false) {

        $.ajax({
            type: 'POST',
            url: controlador + 'ConsultaParticipanteExistente',
            data: {
                participante: participante
            },
            dataType: 'json',
            success: function (result) {


                if (result.success == false) {
                    LlenaTablaPrimerMes(codigo, participante, nombre);

                }
                else {
                    //alert(result.message);
                    //$(thiss).parent().append('<span style="color:red;">' + result.message + '</span>');
                    alert("La empresa participante ya se encuentra agregado en otro periodo");
                }

            },
            error: function () {
                mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
            }
        });


        $('#popupBuscarEmpresa').bPopup().close();
    }

}

function LlenaTablaPrimerMes(codigo, participante, nombre) {
    cambioPrimerMes();
    $("#divMensajePrimerMes").show();
    $('#tablaInsumosGen > tbody:last-child').append('<tr class="menu-contextual" data-code="' + codigo + '" data-codepart="' + participante + '">' +
        '<td style = "text-align: left; width: 30%;" >' +
        '<input type="text"  id="' + codigo + '_mcodigo" name="' + codigo + '_mcodigo" class="form-control" disabled style="width:100%" + value="' + nombre + '" />' +
        '</td>' +
        '<td style="text-align: left; width: 15%;">' +
        '<input type="text" id="' + codigo + '_mreserva" name="' + codigo + '_mreserva" class="form-control" style="width:100%"' +
        'value="0" />' +
        '</td>' +
        '<td style="text-align: left; width: 15%;">' +
        '<input type="text" id="' + codigo + '_sscc" name="' + codigo + '_sscc" class="form-control" style="width:100%" value="0" />' +
        '</td>' +
        '<td style="text-align: left; width: 15%;">' +
        '<input type="text" id="' + codigo + '_tinflex" name=' + codigo + '_"tinflex" class="form-control" style="width:100%"   value="0" />' +
        '</td>' +
        '<td style="text-align: left; width: 15%;">' +
        '<input type="text" id="' + codigo + '_texceso" name="' + codigo + '_texceso" class="form-control" style="width:100%"  value="0" />' +
        '</td>' +

        '<td style="width:40px;">' +
        '<a href="JavaScript:modificarAgentes("' + codigo + '");" title="Modificar">' +
        '<img src="~/Content/Images/btn-edit.png" title="Editar" />' +
        '</a>' +
        '<a href="JavaScript:eliminarAgentes("' + codigo + '");" title="Eliminar">' +
        '<img src="~/Content/Images/Trash.png" title="Eliminar" />' +
        '</a>' +
        '</td>' +
        '</tr>');
}
/* FIN MANTENIMIENTO PARTICIPANTE*/