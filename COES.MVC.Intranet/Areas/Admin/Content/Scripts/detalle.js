var controlador = siteRoot + 'admin/usuarios/'

$(function () {  
    
    $('#btnGrabarEmpresa').click(function () {
        grabarEmpresas();
    });

    $('#btnCancelar').click(function () {
        document.location.href = controlador + 'index';
    });

    $('#btnGrabarTotal').click(function () {
        grabarTotal();
    });

    cargarDatos();
    cargarEmpresas();
    cargarSolicitudes();
});

function cargarDatos() {
    $.ajax({
        type: "POST",
        url: controlador + "datos",
        data: {
            idUsuario: $('#hfUsuario').val()
        },
        success: function (evt) {
            $('#datos').html(evt);

            $("#btnDarBajaUsuario").click(function () {
                darBajaUsuario();
            })

            $("#btnActivarUsuarioPendiente").click(function () {
                activarUsuario();
            });

            $("#btnActivarUsuarioBaja").click(function () {
                activarUsuario();
            });
        },
        error: function () {
            mostrarError();
        }
    });
}

function cargarEmpresas() {
    $.ajax({
        type: "POST",
        url: controlador + "empresa",
        success: function (evt) {
            $('#empresa-selected').html(evt);           
        },
        error: function () {
            mostrarError();
        }
    });
}

function cargarSolicitudes() {
    $.ajax({
        type: "POST",
        url: controlador + "solicitudes",
        data: {
            id: $('#hfUsuario').val()
        },
        success: function (evt) {
            $('#solicitudes').html(evt);

            $('#cbSelectAll').click(function (e) {
                var table = $(e.target).closest('table');
                $('td input:checkbox', table).prop('checked', this.checked);
            });
                       
            if ($('#hfEstadoUsuario').val() == "P" && $('#hfIndicadorSolicitud').val() == "S") {
               
                $('#btnGrabarTotal').css('display', 'block');
            }
            else {
                $('#btnGrabarTotal').css('display', 'none');
            }
        },
        error: function () {
            mostrarError();
        }
    });
}

function agregarEmpresa(idEmpresa) {
    if (idEmpresa != "") {
        $.ajax({
            type: "POST",
            url: controlador + "addempresa",
            data: {
                idEmpresa: idEmpresa
            },
            success: function (evt) {
                $('#empresa-selected').html(evt);
            },
            error: function () {
                mostrarError();
            }
        });
    }
}

function removeEmpresa(idEmpresa) {
    if (confirm("¿Está seguro de realizar esta operación?")) {
        $.ajax({
            type: "POST",
            url: controlador + "removeempresa",
            data: {
                idEmpresa: idEmpresa
            },
            success: function (evt) {
                $('#empresa-selected').html(evt);
            },
            error: function () {
                mostrarError();
            }
        });
    }
}


darBajaUsuario = function () {
    if (confirm('¿Está seguro de realizar esta operación?')) {
        $.ajax({
            type: "POST",
            url: controlador + "darbajausuario",
            data: {
                idUsuario: $("#hfUsuario").val()
            },
            datatype: "json",
            success: function (result) {
                if (result == 1) {
                    cargarDatos();
                    mostrarExitoOperacion();
                }
                else {
                    mostrarError();
                }
            },
            error: function () {
                mostrarError();
            }
        });
    }
}

activarUsuario = function () {
    if (confirm('¿Está seguro de realizar esta operación?')) {
        $.ajax({
            type: "POST",
            url: controlador + "activarusuario",
            data: {
                idUsuario: $("#hfUsuario").val()
            },
            datatype: "json",
            success: function (result) {
                if (result == 1) {
                    cargarDatos();
                    mostrarExitoOperacion();
                }
                else {
                    mostrarError();
                }
            },
            error: function () {
                mostrarError();
            }
        });
    }
}

darAcceso = function (idModulo) {
    if (confirm('¿Está seguro de aceptar esta solicitud?'))
    {
        $.ajax({
            type: "POST",
            url: controlador + "daracceso",
            data: {
                idUsuario: $('#hfUsuario').val(),
                idModulo: idModulo
            },
            datatype: "json",
            success: function (result) {
                if (result == 1) {
                    mostrarExitoOperacion();
                    cargarSolicitudes();
                }
                else {
                    mostrarError();
                }
            },
            error: function () {
                mostrarError();
            }
        });
    }
}

grabarEmpresas = function () 
{
    if (confirm('¿Está seguro de agregar las empresas')) {
        $.ajax({
            type: "POST",
            url: controlador + "grabarempresausuario",
            data: {
                idUsuario: $('#hfUsuario').val()
            },
            datatype: "json",
            success: function (result) {
                if (result == 1) {
                    mostrarExitoOperacion();                    
                }
                else {
                    mostrarError();
                }
            },
            error: function () {
                mostrarError();
            }
        });
    }
}

grabarTotal = function () {
    if (confirm('¿Está seguro de realizar esta operación?')) {

        var modulos = "";
        var countModulo = 0;
        $('#tbSeleccionados tbody input:checked').each(function () {
            modulos = modulos + $(this).val() + ",";
            countModulo++;
        });

        if (countModulo > 0) {

            $.ajax({
                type: "POST",
                url: controlador + "grabarusuario",
                data: {
                    idUsuario: $("#hfUsuario").val(),
                    modulos: modulos
                },
                datatype: "json",
                success: function (result) {
                    if (result == 1) {
                        cargarDatos();
                        cargarSolicitudes();
                        mostrarExitoOperacion();
                    }
                    else {
                        mostrarError();
                    }
                },
                error: function () {
                    mostrarError();
                }
            });
        }
        else {
            mostrarAlerta('Seleccione al menos una solicitud de acceso al módulo');
        }
    }
}

jQuery.browser = {};
(function () {
    jQuery.browser.msie = false;
    jQuery.browser.version = 0;
    if (navigator.userAgent.match(/MSIE ([0-9]+)\./)) {
        jQuery.browser.msie = true;
        jQuery.browser.version = RegExp.$1;
    }
})();

function mostrarError()
{
    $('#mensaje').removeClass();
    $('#mensaje').addClass("action-error");
    $('#mensaje').text("Ha ocurrido un error");
}

function mostrarExitoOperacion()
{
    $('#mensaje').removeClass();
    $('#mensaje').addClass("action-exito");
    $('#mensaje').text("Operación ejecutada correctamente.");
}

function mostrarAlerta(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').addClass("action-alert");
    $('#mensaje').text(mensaje);
}