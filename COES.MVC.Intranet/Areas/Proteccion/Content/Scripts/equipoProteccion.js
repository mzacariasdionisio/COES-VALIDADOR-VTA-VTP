//Declarando las variables 
let controlador = siteRoot + 'EquipoProteccion/';
let hot;
let hotOptions;
let evtHot;

$(function () {

    cargarArbol();
    $('#mensaje').css("display", "none");
    $("#popUpEditar").hide();
    $("#popupCambiarEstado").addClass("general-popup");
    
    $('#btnBuscar').click(function () {
        consultar();
    });

    $('#btnRegresar').click(function () {
        regresar();
    });


    $('#btnBuscarArbol').click(function () {
        cargarArbol();

        $("#pEquicodi").val(0);
        $("#pNivel").val(0);
        $("#tituloRele").text("Nivel:");
        $("#urlRele").text("").removeAttr("href");

        consultar();
    });

    $('#btnExportar').click(function () {
        exportarListado();
    });

    $('#FechaVigente').Zebra_DatePicker({
        show_clear_date: 0,
        onSelect: function () {
        }
    });

    let today = new Date();
    let yyyy = today.getFullYear();
    let mm = today.getMonth() + 1; 
    let dd = day_of_the_month(today);
    let maxFecha = dd + "/" + mm + "/" + yyyy;

    $('#FechaHasta').Zebra_DatePicker({
        show_clear_date: 0,
        direction: [false, maxFecha],
        onSelect: function () {
        }
    });

    $('#btnNuevo').click( function(){
        editar(0, 'NUEVO');
    })

    $('#btnGrabarCE').click(function () {
        guardarCambiarEstado();
    });

 

    consultar();


});

function descargarManual() {
    window.location = controlador + 'DescargarManual';

}

function validarGuardar() {
    let campos = [];
    if ($('#idCelda').val() == '') campos.push('Celda o Generador');
    if ($('#idProyecto').val() == '') campos.push('Proyecto');
    if ($('#codigoRele').val() == '') campos.push('Nombre');
    if ($('#idTitular').val() == '' || $('#idTitular').val() == '0') campos.push('Titular');
    if ($('#tensionRele').val() == '') campos.push('Tensión (KV)');
    if ($('#idSistemaRele').val() == '') campos.push('Sistema de Relé');
    if ($('#idMarca').val() == '') campos.push('Marca');
    if ($('#modeloRele').val() == '') campos.push('Modelo');
    if ($('#idTipoUso').val() == '') campos.push('Tipo de uso');

    if (campos.length > 0) {
        
        mensajeValidador(campos);
        return false;
    }

    return true;
}



let loadValidacionFile = function (mensaje) {
    alert(mensaje);
}
function descargarArchivo(nomArchivo, equicodi) {
    window.location = controlador + 'DescargarArchivo?fileName=' + nomArchivo + '&equicodi=' + equicodi;

}
let mensajeExito = function () {
    $('#popupFormatoEnviadoOk').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
}

let cerrarpopupFormatoEnviadoOk = function () {
    $('#popupFormatoEnviadoOk').bPopup().close();
}

let mostrarError = function () {
    $('#mensaje').removeClass("action-exito");
    $('#mensaje').addClass("action-error");
    $('#mensaje').text("Ha ocurrido un error");
    $('#mensaje').css("display", "block");
};

let consultar = function () {

    let equicodi = $("#pEquicodi").val();
    let nivel = $("#pNivel").val();
    let celda = $("#celda").val(); 
    let rele = $("#rele").val(); 
    let tituloRele = $("#tituloRele").html();
    let idArea = 0; 
    if ($("#Area").val() == null || $("#Area").val() == "") {
        idArea = 0;
    } else {
        idArea = $("#Area").val();
    }
    let nombSubestacion = $("#ubicacion").val(); 

    let datos = {
        equicodi: equicodi,
        nivel: nivel,
        celda: celda,
        rele: rele,
        idArea: idArea,
        nombSubestacion: nombSubestacion,
        tituloRele: tituloRele
    }

    setTimeout(function () {
        $.ajax({
            type: 'POST',
            url: controlador + 'ListaProyectos',
            data: datos,
            success: function (evt) {
                $('#lista').html(evt);

                $('#tablaListado').dataTable({
                    "iDisplayLength": 25,
                    order: [[4, 'desc']],
                    language: {
                        info: 'Mostrando página _PAGE_ de _PAGES_',
                        infoEmpty: 'No hay registros disponibles',
                        infoFiltered: '(filtrado de _MAX_ registros totales)',
                        lengthMenu: 'Mostrar _MENU_ registros por página',
                        zeroRecords: 'No se encontró nada'
                    }
                });

                $('.dataTables_filter input').attr('maxLength', 50);
            },
            error: function () {
                mostrarMensaje('Ha ocurrido un error.');
            }
        });
    }, 100);
};



let obtenerDetalleArbol = function (equicodi, nivel) {
    setTimeout(function () {
        $.ajax({
            type: 'POST',
            url: controlador + 'ObtenerDetalleArbol',
            data: {
                equicodi: equicodi,
                nivel: nivel
            },
            success: function (evt) {
                $('#urlRele').html(evt.MemoriaCalculoTexo);
                $('#urlRele').attr('href', 'Javascript: descargarArchivoSE("' + evt.MemoriaCalculo + '",' + evt.Areacodi +')');
            },
            error: function () {
                mostrarMensaje('Ha ocurrido un error.');
            }
        });
    }, 100);
};

function descargarArchivoSE(nomArchivo, equicodi) {
    window.location = controlador + 'DescargarArchivoSE?fileName=' + nomArchivo + '&epsubecodi=' + equicodi;

}


function mostrarMensaje(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-message');
}

let cargarArbol = function () {

    let parametros = {
        idZona: $('#Area').val() == "" ? 0 : $('#Area').val(),
        ubicacion: $('#ubicacion').val()
    };

    $.ajax({
        type: 'POST',
        url: controlador + 'Arbol',
        data: parametros,
        success: function (evt) {
            $('#tree').html(evt);
        },
        error: function () {
            mostrarMensaje('Ha ocurrido un error.');
        }
    });
}













function desactivarControles(arrayControles) {
    for (let control of arrayControles) {
        $('#' + control).prop('disabled', true);
    }
}

function editar(e, accion) {

    let equicodi= $("#pEquicodi").val();
    let nivel = $("#pNivel").val();

    window.location.href = controlador + "editar?id=" + e + "&accion=" + accion + "&idArbol=" + equicodi + "&nivel=" + nivel;
};

function consultarProyecto(idProyecto) {
        if (idProyecto == 0) {
            $("#fechaRele").val('');
            return;
        }
        setTimeout(function () {
            $.ajax({
                type: 'POST',
                url: controlador + 'SeleccionarProyecto',
                data: { idProyecto: idProyecto },
                success: function (evt) {
                    if (evt) {
                        $("#fechaRele").val(evt.Epproyfecregistro);
                    } else {
                        $("#fechaRele").val('');
                    }
                },
                error: function () {
                    mostrarMensaje('Ha ocurrido un error.');
                }
            });
        }, 100); 
}

function consultarProyectoCambioEstado(idProyecto) {
    if (idProyecto == 0) {
        $("#estFecha").val('');
        return;
    }
    setTimeout(function () {
        $.ajax({
            type: 'POST',
            url: controlador + 'SeleccionarProyecto',
            data: { idProyecto: idProyecto },
            success: function (evt) {
                if (evt) {
                    $("#estFecha").val(evt.Epproyfecregistro);
                } else {
                    $("#estFecha").val('');
                }
            },
            error: function () {
                mostrarMensaje('Ha ocurrido un error.');
            }
        });
    }, 100);
}


function editarCambioEstado(id) {
    setTimeout(function () {
        abrirPopup("popupCambiarEstado");
    }, 50);

    $.ajax({
        type: 'POST',
        url: controlador + "EditarCambioEstado",
        data: {
            equicodi: id
        },
        success: function (data) {
            setTimeout(function () {
                limpiarCambiarEstado();

                $("#hEstCodigo").val(id);
                if (data) {
                    $("#estNuevosEstados").val(data.IdEstado);

                    $("#estFecha").val(data.Fecha);

                    let select = $('#estProyecto');
                    select.empty();
                    select.append('<option value="">SELECCIONAR</option>');
                    $.each(data.listaProyecto, function (index, item) {
                        select.append('<option value="' + item.Epproycodi + '">' + item.Epproynomb + '</option>');
                    });
                    $("#estProyecto").val(data.IdProyecto);
                }
                consultarProyectoCambioEstado($("#estProyecto").val());
                $('#estProyecto').change(function () {
                    let idProyecto = $(this).val();
                    consultarProyectoCambioEstado(idProyecto)
                });

            }, 50);

        },
        error: function () {
            mostrarError();
        }
    });
};

function guardarCambiarEstado() {

    if (validacionCambiarEstado()) {

        let motivo = "";

        if ($('#estMotivo').val() != null && $('#estMotivo').val() != "") {
            motivo = $('#estMotivo').val().trim();
        }

        if (confirm('¿Está seguro de cambiar el Estado?')) {

            let datos = {
                iNuevosEstados: $('#estNuevosEstados').val(),
                iFecha: $('#estFecha').val(),
                iMotivo: motivo,
                iCodigo: $("#hEstCodigo").val(),
                iEpproycodi: $("#estProyecto").val()
            };

            $.ajax({
                type: 'POST',
                url: controlador + 'GuardarCambiarEstado',
                data: datos,
                success: function (resultado) {
                    if (resultado == "") {
                        $('#mensaje').css("display", "none");
                        cerrarPopup("popupCambiarEstado");
                        consultar();
                    } else {
                        alert(resultado);
                    }

                   
                },
                error: function (ex) {
                    console.log(ex);
                    mostrarError();
                }
            });
        }

    }
};

function validacionCambiarEstado() {

    let campos = [];
    if ($('#estNuevosEstados').val() == '') campos.push('Nuevos Estados');
    if ($('#estProyecto').val() == '') campos.push('Proyecto');
    if ($('#estMotivo').val() == '' || $('#estMotivo').val().trim().length == 0) campos.push('Motivo');
    

    if (campos.length > 0) {

        mensajeValidador(campos);
        return false;
    }

    return true;
}
function mensajeValidador(campos) {
    let texto = "Los campos: \n";

    for (const campo of campos) {
        texto += campo + "(*) \n";
    }

    alert(texto + " son requeridos");
}
function limpiarCambiarEstado() {
    $("#hEstCodigo").val("");
    $("#estNuevosEstados").val("");
    $("#estProyecto").val("");
    $("#estFecha").val("");
    $("#estMotivo").val("");
}
function habilitarInput(id) {
    $('#' + id).css('cssText', 'background-color: #FFFFFF!important');
    $('#' + id).prop("disabled", false);
}
function deshabilitarInput(id) {
    $('#' + id).css('cssText', 'background-color: #F2F4F3!important');
    $('#' + id).prop("disabled", true);
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
};

function cerrarPopup(id) {
    $("#" + id).bPopup().close()
}

function exportarListado() {
    let equicodi = $("#pEquicodi").val();
    let nivel = $("#pNivel").val();
    let celda = $("#celda").val();
    let rele = $("#rele").val();
    let idArea = 0;
    if ($("#Area").val() == null || $("#Area").val() == "") {
        idArea = 0;
    } else {
        idArea = $("#Area").val();
    }
    let nombSubestacion = $("#ubicacion").val();

    let datos = {
        equicodi: equicodi,
        nivel: nivel,
        celda: celda,
        rele: rele,
        idArea: idArea,
        nombSubestacion: nombSubestacion
    }

    $.ajax({
        type: 'POST',
        url: controlador + 'GenerarReporteRele',
        data: datos,
        dataType: 'json',
        success: function (evt) {

            if (evt.Resultado != "-1") {
                window.location = controlador + "ExportarReporte?file_name=" + evt.Resultado;
            } else {
                mostrarMensaje('Error: ' + evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('Ha ocurrido un error.');
        }
    });

}
function day_of_the_month(d) {
    return (d.getDate() < 10 ? '0' : '') + d.getDate();
}