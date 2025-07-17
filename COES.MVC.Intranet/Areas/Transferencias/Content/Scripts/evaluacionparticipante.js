var controlador = siteRoot + 'Transferencias/EvaluacionParticipante/'
let tipoRegistro = 0;
$(function () {

    $('#FechaDesde').Zebra_DatePicker({
    });

    $('#FechaHasta').Zebra_DatePicker({
    });
    $('#FechaVigencia').Zebra_DatePicker({
    });

    $("#btnNuevo").click(function () {
        NuevaConfiguracionPtoMME();
    });
    $('#btnBuscar').click(function () {
        buscarConfiguracion();
    });
    $(window).resize(function () {
        $('#listado').css("width", $('#mainLayout').width() + "px");
    });

    buscarConfiguracion();
});

function NuevaConfiguracionPtoMME() {
    var confconcodi = 0;
    tipoRegistro = 1;
    $('#hdtipoGuardado').val(tipoRegistro);
    $.ajax({
        type: 'POST',
        url: controlador + 'ConfigurarPtoMME',
        data: {
            confconcodi: confconcodi
        },
        success: function (evt) {
            $('#contenidoDetalle').html(evt);
            //var idEmpresa = $('#cbEmpresaConfg').val();
            cargarPtosMedicion();
            setTimeout(function () {
                $('#popupNuevaConfiguracion').bPopup({
                    autoClose: false,
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown',
                    onClose: function () {
                        $('#popup').empty();
                    }
                });
            }, 500);

            $("#cbEmpresaConfg").change(function () {
                
                cargarPtosMedicion();
            });

            $("#btnCancelarNewConfiguracion").click(function () {
                $('#popupNuevaConfiguracion').bPopup().close();
            });

            $('#btnGrabarNewConfiguracion').click(function () {

                guardarConfiguracionPtoMME();

            });
        },
        error: function (err) {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

function cargarPtosMedicion() {
    debugger;
    var idEmpresa = $('#cbEmpresaConfg').val();
    var ptomedicodi = $('#hdPtoMedicodi').val(); 
    //alert('Empresa:' + idEmpresa);
    //alert('Ptomedicodi:' + ptomedicodi);
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarPtosMedicion',
        dataType: 'json',
        async: true,
        data: {
            idEmpresa: idEmpresa
        },
        success: function (evt) {
            $('#cbPtoMedicion').empty();
            var ListaHojaPto = evt.ListaHojaPto;
            if (ListaHojaPto != null && ListaHojaPto.length > 0) {

                for (var i = 0; i < ListaHojaPto.length; i++) {
                    if (ptomedicodi > 0 && ListaHojaPto[i].Ptomedicodi == ptomedicodi) {
                        $("#cbPtoMedicion option[value=" + ListaHojaPto[i].Ptomedicodi + "]").attr("selected", true);
                        $('#cbPtoMedicion').append('<option value=' + ListaHojaPto[i].Ptomedicodi + '>' + ListaHojaPto[i].PuntoConexion + '-' + ListaHojaPto[i].Clientenomb + '-' + ListaHojaPto[i].Ptomedicodi + '</option>');
                        $("#cbPtoMedicion option[value=" + ListaHojaPto[i].Ptomedicodi + "]").attr("selected", true);
                        //alert('<option ' + seleccionado + ''+' value=' + ListaHojaPto[i].Ptomedicodi + '>' + ListaHojaPto[i].PuntoConexion + '-' + ListaHojaPto[i].Clientenomb + '-' + ListaHojaPto[i].Ptomedicodi + '</option>');
                    }
                    else {
                        $('#cbPtoMedicion').append('<option value=' + ListaHojaPto[i].Ptomedicodi + '>' + ListaHojaPto[i].PuntoConexion + '-' + ListaHojaPto[i].Clientenomb + '-' + ListaHojaPto[i].Ptomedicodi + '</option>');
                    }
                }
            } 
        },
        error: function (err) {
            alert("Ha ocurrido un error al cargar empresas");

        }
    });
}

function guardarConfiguracionPtoMME() {
    debugger;
    var emprcodi = $('#cbEmpresaConfg').val().trim();
    var ptomedicodi = $('#cbPtoMedicion').val().trim();
    var vigenciacodi = $('#cbVigencia').val().trim();
    var fechavigencia = $('#FechaVigencia').val().trim();
    var tipo = $('#hdtipoGuardado').val();
    var confconcodi = $('#hdConfconcodi').val();
    $.ajax({
        type: 'POST',
        url: controlador + 'GuardarConfPtoMME',
        data: {
            emprcodi: emprcodi,
            ptomedicodi: ptomedicodi,
            vigenciacodi: vigenciacodi,
            fechavigencia: fechavigencia,
            tipo: tipo,
            confconcodi: confconcodi
        },
        success: function (evt) {

            if (evt.Resultado == "1") {
                mostrarMensajeEval('mensajeEdit', 'exito', "Configuración guardada con éxito.");
                setTimeout("$('#popupNuevaConfiguracion').bPopup().close()", 1000);
            }

            if (evt.Resultado == "2") {
                mostrarMensajeEval('mensajeEdit', 'alert', "Debe existir un registro con fecha Vigente para registrar un No Vigente.");
            }

            if (evt.Resultado == "3") {
                mostrarMensaje('mensajeEdit', 'alert', "Fecha de No vigencia debe ser mayor a fecha de Vigencia activa.");
            }

        },
        error: function (err) {

            mostrarMensajeEval('mensajeEdit', 'alert', "Ha ocurrido un error en guardar la nueva configuración.");

        }
    });
    
}
buscarConfiguracion = function () {
    mostrarListado(1);
}
mostrarListado = function (nroPagina) {
    $.ajax({
        type: 'POST',
        url: controlador + "lista",
        data: $('#frmBusqueda').serialize(),
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");

            $('#listado').html(evt);
            $('#tabla').dataTable({
                "scrollY": 430,
                "scrollX": true,
                "sDom": 't',
                "ordering": false,
                "iDisplayLength": 50
            });
        },
        error: function () {
        }
    });
}
function mostrarMensajeEval(id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
};

function EditarConfiguracionPtoMME(confconcodi) {
    debugger;
    tipoRegistro = 2
    $('#hdtipoGuardado').val(tipoRegistro);
    
    $.ajax({
        type: 'POST',
        url: controlador + 'ConfigurarPtoMME',
        data: {
            confconcodi: confconcodi
        },
        success: function (evt) {
            $('#contenidoDetalle').html(evt);
            //var idEmpresa = $('#hdEmpresaPtoMME').val();
            
            setTimeout(function () {
                $('#popupNuevaConfiguracion').bPopup({
                    autoClose: false,
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown',
                    onClose: function () {
                        $('#popup').empty();
                    }
                });
            }, 500);

            cargarPtosMedicion();

            $("#cbEmpresaConfg").change(function () {
                //var idEmpresa_ = $('#cbEmpresaConfg').val();
                cargarPtosMedicion();
            });

            $("#btnCancelarNewConfiguracion").click(function () {
                $('#popupNuevaConfiguracion').bPopup().close();
            });

            $('#btnGrabarNewConfiguracion').click(function () {

                guardarConfiguracionPtoMME();

            });
        },
        error: function (err) {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

function EliminarConfiguracionPtoMME(confconcodi) {
    debugger;

    if (confirm("Esta seguro que desea eliminar el registro?")) {
        $.ajax({
            type: 'POST',
            url: controlador + 'EliminarConfiguracionPtoMME',
            data: {
                confconcodi: confconcodi
            },
            success: function (evt) {
                if (evt.result == '1') {
                    alert('Se elimino correctamente');
                    buscarConfiguracion();
                }
            },
            error: function (err) {
                alert("Lo sentimos, ha ocurrido un error inesperado");
            }
        });
    }

    
}