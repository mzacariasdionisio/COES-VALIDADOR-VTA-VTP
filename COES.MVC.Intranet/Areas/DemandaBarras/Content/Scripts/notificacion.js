/*-------------------------------------------------------------------
Clase: notificacion.js
Descripción: Contiene la funcionalidad de la pantalla de notificacion
Fecha creación: 19-08-2016
Autor: Raúl Castro
Versión: 1.0
-------------------------------------------------------------------*/

var controlador = siteRoot + 'demandabarras/notificacion/';

$(function () {

    $('#tab-container').easytabs({
        animate: false
    });

    $('#btnCargarCuenta').on("click", function () {
        cargarCuentas();
    });

    $('#btnNuevaCuenta').on("click", function () {
        editarCuenta(0);
    });

    $('#cbTipoEmpresa').on("change", function () {
        cargarEmpresas();
    });

    $('#cbEmpresa').on("change", function () {
        cargarCuentas();
    });

    cargarCuentas();
    configurarEmpresas();

    tinymce.init({
        selector: '#txtPlantilla',
        plugins: [      
        'paste textcolor colorpicker textpattern'
        ],
        toolbar1: 'insertfile undo redo | bold italic | alignleft aligncenter alignright alignjustify |  bullist numlist outdent indent| forecolor backcolor',
        menubar: false,
        language: 'es',
        statusbar: false,
        setup: function (editor) {
            editor.on('change', function () {
                editor.save();
            });
        }
    });

    $('#btnMostrarPlantilla').on("click", function () {
        cargarPlantilla();
    });

    $('#cbPlantillaCorreo').on("change", function () {
        cargarPlantilla();
    });

    $('#btnGrabarPlantilla').on("click", function(){
        grabarPlantilla();
    });
    
    $('#txtFechaLog').Zebra_DatePicker({
        onSelect: function (date) {
            consultarLog();
        }
    });

    $('#btnConsultarLog').on("click", function () {
        consultarLog();
    });

    consultarLog();

});

cargarEmpresas = function () {
    $('option', '#cbEmpresa').remove();
    
    $.ajax({
        type: 'POST',
        url: controlador + 'cargarempresas',
        dataType: 'json',
        data: {
            tipoEmpresa: $('#cbTipoEmpresa').val()
        },
        cache: false,
        success: function (aData) {
            $('#cbEmpresa').get(0).options.length = 0;
            $('#cbEmpresa').get(0).options[0] = new Option("--SELECCIONE--", "-1");
            $.each(aData, function (i, item) {
                $('#cbEmpresa').get(0).options[$('#cbEmpresa').get(0).options.length] = new Option(item.Text, item.Value);
            });

            cargarCuentas();
        },
        error: function () {
            mostrarError('mensaje');
        }
    });    
}

cargarCuentas = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'listacuentas',
        data: {
            tipoEmpresa: $('#cbTipoEmpresa').val(),
            empresa: $('#cbEmpresa').val()
        },
        success: function (evt) {
            $('#listaCuenta').html(evt);
            $('#tablaCuenta').dataTable({
            });        
        },
        error: function () {
            mostrarError('mensaje');
        }
    });
}

editarCuenta = function (id) {
    $.ajax({
        type: 'POST',
        url: controlador + 'addcuenta',
        data: {
            idCuenta: id
        },
        success: function (evt) {
            $('#contenidoCuenta').html(evt);
            setTimeout(function () {
                $('#popupCuenta').bPopup({
                    autoClose: false
                });
            }, 50);

            $('#cbEmpresaCuenta').val($('#hfEmpresaCuenta').val());
            $('#cbEstadoCuenta').val($('#hfEstadoCuenta').val());

            $('#btnGrabarCuenta').on("click", function () {
                grabarCuenta();
            });
            $('#btnCancelarCuenta').on("click", function(){
                $('#popupCuenta').bPopup().close();
            });
        },
        error: function () {
            mostrarError('mensaje');
        }
    });
}

eliminarCuenta = function (id) {
    if (confirm('¿Está seguro de eliminar esta cuenta?')){
        
        $.ajax({
            type: 'POST',
            url: controlador + 'eliminarcuenta',
            data: {
                idCuenta: id
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    cargarCuentas();                    
                    mostrarExito('La cuenta ha sido eliminada...!', 'mensajeListCuenta');
                }
                else {
                    mostrarError('mensajeListCuenta');
                }
            },
            error: function () {
                mostrarError('mensajeListCuenta');
            }
        });
    }
}

grabarCuenta = function () {
    var mensaje = validarCuenta();

    if (mensaje == "") {
        
        $.ajax({
            type: 'POST',
            url: controlador + 'grabarcuenta',
            data: {
                idCuenta : $('#hfIdCuenta').val(),
                emprcodi : $('#cbEmpresaCuenta').val(), 
                nombre : $('#txtNombreCuenta').val(), 
                email : $('#txtCorreoCuenta').val(), 
                descripcion : $('#txtComentarioCuenta').val(), 
                estado: $('#cbEstadoCuenta').val()
            },
            dataType: 'json',
            success: function (result) {
                if (result > 0) {
                    cargarCuentas();
                    $('#popupCuenta').bPopup().close();
                    mostrarExito('Los datos se grabaron correctamente.', 'mensajeListCuenta');
                }
                else {
                    mostrarError('mensajeCuenta');
                }
            },
            error: function () {
                mostrarError('mensajeCuenta');
            }
        });
    }
    else {
        mostrarAlerta(mensaje, 'mensajeCuenta')
    }    
}

validarCuenta = function () {
    var flag = true;
    var mensaje = "<ul>";
    
    if ($('#cbEmpresaCuenta').val() == '-1') {
        mensaje = mensaje + "<li>Seleccione empresa.</li>";
        flag = false;
    }

    if ($('#txtNombreCuenta').val() == '') {
        mensaje = mensaje + "<li>Ingrese nombre de la cuenta.</li>";
        flag = false;
    }

    if ($('#txtCorreoCuenta').val() == '') {
        mensaje = mensaje + "<li>Ingrese correo.</li>";
        flag = false;
    }
    else if (!validarEmail($('#txtCorreoCuenta').val())) {
        mensaje = mensaje + "<li>Ingrese un correo válido.</li>";
        flag = false;
    }

    if ($('#cbEstadoCuenta').val() == '') {
        mensaje = mensaje + "<li>Seleccione estado.</li>";
        flag = false;
    }

    mensaje = mensaje + "</ul>";
    if (flag) mensaje = "";
    return mensaje;
}

validarEmail = function (email) {
    var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return regex.test(email);
}

cargarPlantilla = function () {    
    var id = $('#cbPlantillaCorreo').val();

    if (id != '-1')
    {
        $.ajax({
            type: 'POST',
            url: controlador + 'obtenerplantilla',
            data: {
                idPlantilla: id
            },
            dataType: 'json',
            success: function (result) {
                if (result != -1) {
                    $('#txtAsuntoCorreo').val(result.Plantasunto);
                    $('#hfIdPlantilla').val(id);
                    tinyMCE.get('txtPlantilla').setContent(result.Plantcontenido);
                    $('#confPlantilla').show();
                }
                else {
                    mostrarError('mensajePlantilla');
                }
            },
            error: function () {
                mostrarError('mensajePlantilla');
            }
        })
    }
    else
    {
        mostrarAlerta('Seleccione plantilla', 'mensajePlantilla');
        $('#confPlantilla').hide();
    }
}

grabarPlantilla = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'grabarplantilla',
        data: $('#formPlantilla').serialize(),
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                mostrarExito('Los datos de la plantilla se grabaron correctamente', 'mensajePlantilla');
            }
            else {
                mostrarError('mensajePlantilla');
            }
        },
        error: function () {
            mostrarError('mensajePlantilla');
        }
    })
}

consultarLog = function () {
    var fecha = $('#txtFechaLog').val();

    if (fecha != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'consultarlog',
            data: {
                fecha: $('#txtFechaLog').val()
            },
            success: function (evt) {
                $('#listadoLog').html(evt);
                $('#tablaLog').dataTable({
                });
            },
            error: function () {
                mostrarError('mensajeLog');
            }
        });
    }
}

verContenido = function (id) {
    $.ajax({
        type: 'POST',
        url: controlador + 'vercontenido',
        data: {
            idCorreo: id
        },
        dataType: 'json',
        success: function (result) {
            if (result != -1) {
                $('#contenidoLog').html(result);
                setTimeout(function () {
                    $('#popupLog').bPopup({
                        autoClose: false
                    });
                }, 50);
            }
            else {
                mostrarError('mensajeLog');
            }
        },
        error: function () {
            mostrarError('mensajeLog');
        }
    });
}

configurarEmpresas = function () {

    $.ajax({
        type: 'POST',
        url: controlador + 'listaempresas',
        success: function (evt) {
            $('#listadoEmpresas').html(evt);
            $('#tablaEmpresas').dataTable({
                "sDom": 't',
                "iDisplayLength": 500  
            });
        },
        error: function () {
            mostrarError('mensajeEmpresas');
        }
    });
}


grabarConfiguracion = function () {

    var empresas = "";    
    $('#tablaEmpresas tbody input:checked').each(function () {
        empresas = empresas + $(this).val() + ",";        
    });
    
    $.ajax({
        type: 'POST',
        url: controlador + 'actualizarconfiguracionempresas',
        data: {
            empresas: empresas
        },
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                configurarEmpresas();
                mostrarExito('La operación se realizó correctamente.', 'mensajeEmpresas');
            }
            else {
                mostrarError('mensajeEmpresas');
            }
        },
        error: function () {
            mostrarError('mensajeEmpresas');
        }
    });

}

actualizarInfNotificacion = function (indicador, emprcodi) {
    if (confirm('¿Está seguro de realizar esta acción?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'actualizarindnotificacion',
            data: {
                emprcodi: emprcodi,
                indicador: indicador
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    configurarEmpresas();
                    mostrarExito('La operación se realizó correctamente.', 'mensajeEmpresas');
                }
                else {
                    mostrarError('mensajeEmpresas');
                }
            },
            error: function () {
                mostrarError('mensajeEmpresas');
            }
        });
    }
}

configurarProceso = function (estado) {

    if (confirm('¿Está seguro de realizar esta acción?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'configurarproceso',
            data: {
                estado: estado
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {                    
                    mostrarExito('La operación se realizó correctamente.', 'mensajeEmpresas');
                }
                else {
                    mostrarError('mensajeEmpresas');
                }
            },
            error: function () {
                mostrarError('mensajeEmpresas');
            }
        });
    }
}

mostrarExito = function (mensaje, id){
    $('#' + id).removeClass();
    $('#' + id).addClass('action-exito');
    $('#' + id).html(mensaje);
}

mostrarError = function (id) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-error');
    $('#' + id).html('Ha ocurrido un error.');
}

mostrarAlerta = function (mensaje, id) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-alert');
    $('#' + id).html(mensaje);
}