var controlador = siteRoot + 'cortoplazo/configuracion/';

$(function () {

    $('#btnConsultar').on('click', function () {
        consultar();
    });

    $('#btnNuevo').on('click', function () {
        editarLinea(0);
    });

    $('#btnNuevoGrupo').on('click', function () {
        editarGrupo(0, '1');
    });

    $('#btnNuevoGrupoMinimo').on('click', function () {
        editarGrupo(0, '2');
    });

    $('#btnNuevoLinea').on('click', function () {
        editarLinea(0);
    });

    $('#tab-container').easytabs({
        animate: false
    });

    $('#btnLineaTrafo').on('click', function () {
        verListado(); 
    });

    //- Agreagado por región de seguridad
    $('#btnNuevoRegionSeguridad').on('click', function () {
        editarRegionSeguridad(0);
    });
    

    consultar();
    consultarGrupo('1');

    //- Agreagado por región de seguridad
    consultarRegionSeguridad();

    consultarGrupo('2');

});

/**** METODOS DE LINEAS ****/

verListado = function (columna) {

    document.location.href = controlador + "LineaListGlobal";
}

consultar = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'linealist',
        data: {
            idEmpresa: $('#cbEmpresa').val(),
            estado: $('#cbEstado').val(),
            idGrupo: $('#cbGrupoFiltro').val(),
            idFamilia: $('#cbFamiliaList').val()
        },
        success: function (evt) {
            $('#listado').html(evt);
            $('#tablaLinea').dataTable({
            });
        },
        error: function () {
            mostrarMensaje('mensajeGrupo', 'error', 'Se ha producido un error');
        }
    });
}

editarLinea = function (id) {
    $.ajax({
        type: 'POST',
        url: controlador + 'lineaedit',
        data: {
            idLinea: id
        },
        global: false,
        success: function (evt) {
            $('#contenidoEdicion').html(evt);
            setTimeout(function () {
                $('#popupEdicion').bPopup({
                    autoClose: false
                });
            }, 50);
                        
            $('#cbEstadoLinea').val($('#hfEstadoLinea').val());
            $('#cbEmpresaLinea').val($('#hfEmpresaLinea').val());
            $('#cbEquipoLinea').val($('#hfEquipoLinea').val());
            $('#cbGrupoLineaEdit').val($('#hfGrupoLineaEdit').val());
            $('#cbFamilia').val($('#hfTipoLinea').val());

            if ($('#cbFamilia').val() == "10") {
                $('#trNodo3').show();
                $('#trNombretna2').show();
                $('#trNombretna3').show();
            }

            $('#btnGrabarLinea').on("click", function () {
                grabarLinea();
            });

            $('#btnCancelarLinea').on("click", function () {
                $('#popupEdicion').bPopup().close();
            });

            $('#cbEmpresaLinea').on("change", function () {
                cargarLineas();
            });

            $('#cbFamilia').on("change", function () {
                $('#trNodo3').hide();
                $('#trNombretna2').hide();
                $('#trNombretna3').hide();
                if ($('#cbFamilia').val() == "10") {
                    $('#trNodo3').show();
                    $('#trNombretna2').show();
                    $('#trNombretna3').show();
                }
                cargarLineas();
            });

        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error');
        }
    });
}

cargarLineas = function () {

    $('option', '#cbEquipoLinea').remove();
   

    $.ajax({
        type: 'POST',
        url: controlador + 'cargarlineas',
        dataType: 'json',
        data: {
            emprcodi: $('#cbEmpresaLinea').val(),
            famcodi: $('#cbFamilia').val()
        },
        cache: false,
        global: false,
        success: function (aData) {
            $('#cbEquipoLinea').get(0).options.length = 0;
            $('#cbEquipoLinea').get(0).options[0] = new Option("--SELECCIONE--", "-1");
            $.each(aData, function (i, item) {
                $('#cbEquipoLinea').get(0).options[$('#cbEquipoLinea').get(0).options.length] = new Option(item.Text, item.Value);
            });
        },
        error: function () {
            mostrarMensaje('mensajeLinea', 'error', 'Se ha producido un error.');
        }
    });


}

eliminarLinea = function (id) {
    if (confirm('¿Está seguro de eliminar este registro?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'lineadelete',
            data: {
                idLinea: id
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    consultar();
                    mostrarMensaje('mensaje', 'exito', 'La operación se realizó correctamente.');
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

function grabarLinea () {
    var validacion = validarLinea();

    if (validacion == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'lineasave',
            data: $('#frmRegistro').serialize(),
            dataType: 'json',
            global: false,
            success: function (result) {
                if (result.Resultado == "1") {
                    mostrarMensaje('mensaje', 'exito', 'La operación se realizó correctamente');
                    $('#popupEdicion').bPopup().close();
                    consultar();

                }
                else if (result.Resultado == "2") {
                    mostrarMensaje('mensajeLinea', 'alert', 'La equivalencia de la línea ya se encuentra registrada.');
                }
                else {
                    mostrarMensaje('mensajeLinea', 'error', 'Se ha producido un error: ' + result.Mensaje);
                }
            },
            error: function () {
                mostrarMensaje('mensajeLinea', 'error', 'Se ha producido un error: ' + result.Mensaje);
            }
        });
    }
    else {
        mostrarMensaje('mensajeLinea', 'alert', validacion);
    }
}

function validarLinea () {
    var mensaje = "<ul>";
    var flag = true;

    if ($('#cbEquipoLinea').val() == "") {
        mensaje = mensaje + "<li>Seleccione el codigo Línea SGOCOOES.</li>";
        flag = false;
    }

    if ($('#txtNodoBarra1').val() == "") {
        mensaje = mensaje + "<li>Ingrese nombre nodo barra 1</li>";
        flag = false;
    }      

    if ($('#txtNodoBarra2').val() == "") {
        mensaje = mensaje + "<li>Ingrese nombre nodo barra 2</li>";
        flag = false;
    }  

    if ($('#txtNombretna1').val() == "") {
        mensaje = mensaje + "<li>Ingrese el identificar del TNA</li>";
        flag = false;            
    }

    if ($('#cbEstadoLinea').val() == "") {
        mensaje = mensaje + "<li>Seleccione estado.</li>";
        flag = false;
    }

    if ($('#txtCodigoScada').val().trim() != "") {

        var msj = "<li>El código SCADA ingresado no existe, por favor ingrese un código correcto.</li>";

        if (isNaN($('#txtCodigoScada').val().trim())) { //no es numerico, es texto
            mensaje = mensaje + msj;
            flag = false;
        } else {
            var valor = parseFloat($('#txtCodigoScada').val().trim());

            if (!Number.isInteger(valor) || valor <= 0) { //si es decimal ó <= 0
                mensaje = mensaje + msj;
                flag = false;
            }            
        }
                
    }
    
    mensaje = mensaje + "</ul>";

    if (flag) {
        mensaje = "";
    }

    return mensaje;
}


/**** METODOS PARA GRUPOS ****/

consultarGrupo = function (tipo) {
    $.ajax({
        type: 'POST',
        url: controlador + 'grupolinealist',
        data: {
            tipo: tipo
        },
        success: function (evt) {

            if (tipo == '1') {
                $('#listadoGrupo').html(evt);
                $('#tablaGrupoLinea').dataTable({
                });
            }
            else if (tipo == '2') {
                $('#listadoGrupoMinimo').html(evt);
                $('#tablaGrupoLineaMinimo').dataTable({
                });
            }

        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error');
        }
    });
}

editarGrupo = function (id, tipo) {
    $.ajax({
        type: 'POST',
        url: controlador + 'grupolineaedit',
        data: {
            idGrupo: id,
            tipo: tipo
        },
        global: false,
        success: function (evt) {
            $('#contenidoGrupoLinea').html(evt);
            setTimeout(function () {
                $('#popupGrupoLinea').bPopup({
                    autoClose: false
                });
            }, 50);

            if (tipo == '1') {
                $('#tituloGrupo').text('Configuración de Grupo de Líneas');
            }
            else if (tipo == '2') {
                $('#tituloGrupo').text('Configuración de Grupo de Líneas Flujo Mínimo');
            }
                      
            $('#cbEstadoGrupo').val($('#hfEstadoGrupo').val());

            $('#cbEquipo').val($('#hfEquipo').val());

            $('#btnGrabarGrupo').on("click", function () {
                grabarGrupo(tipo);
            });

            $('#btnCancelarGrupo').on("click", function () {
                $('#popupGrupoLinea').bPopup().close();
            });

        },
        error: function () {
            mostrarMensaje('mensajeGrupo', 'error', 'Se ha producido un error');
        }
    });
}

grabarGrupo = function (tipo) {
    var validacion = validarGrupo();

    if (validacion == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'grupolineasave',
            data: $('#frmRegistroGrupo').serialize(),
            dataType: 'json',
            global: false,
            success: function (result) {
                if (result == 1) {
                    $('#popupGrupoLinea').bPopup().close();

                    if (tipo == '1') {
                        mostrarMensaje('mensajeGrupo', 'exito', 'Los datos se grabaron correctamente.');
                        consultarGrupo('1');
                    }
                    else if (tipo == '2') {
                        mostrarMensaje('mensajeGrupoMinimo', 'exito', 'Los datos se grabaron correctamente.');
                        consultarGrupo('2');
                    }
                }               
                else {
                    mostrarMensaje('mensajeEditGrupo', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensajeEditGrupo', 'error', 'Se ha producido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensajeEditGrupo', 'alert', validacion);
    }
}

validarGrupo = function () {
    var mensaje = "<ul>";
    var flag = true;
        
    if ($('#txtNombreGrupo').val() == '') {
        mensaje = mensaje + "<li>Ingrese el nombre.</li>";
        flag = false;
    }
    /*
    if ($('#txtLimiteGrupo').val() == '') {
        mensaje = mensaje + "<li>Ingrese el flujo límite.</li>";
        flag = false;
    }
    else
    {
        if (!validarDecimal($('#txtLimiteGrupo').val())) {
            mensaje = mensaje + "<li>Ingrese un flujo límite válido.</li>";
            flag = false;
        }
    }
    if ($('#txtPorcentajeGrupo').val() == '') {
        mensaje = mensaje + "<li>Ingrese el porcentaje para determinar congestión.</li>";
        flag = false;
    }
    else {
        if (!validarDecimal($('#txtPorcentajeGrupo').val())) {
            mensaje = mensaje + "<li>Ingrese un porcentaje válido.</li>";
            flag = false;
        }
    }
    
    if ($('#txtCodigoNCPGrupo').val() == '') {
        mensaje = mensaje + "<li>Ingrese el código NCP.</li>";
        flag = false;
    }
    if ($('#txtNombreNCPGrupo').val() == '') {
        mensaje = mensaje + "<li>Ingrese nombre NCP</li>";
        flag = false;
    }
    */
    if ($('#cbEstadoGrupo').val() == '') {
        mensaje = mensaje + "<li>Seleccione el estado.</li>";
        flag = false;
    }

    if ($('#cbEquipo').val() == "") {
        mensaje = mensaje + "<li>Seleccione equipo.</li>";
        flag = false;
    }
    
    mensaje = mensaje + "</ul>";

    if (flag) {
        mensaje = "";
    }

    return mensaje;
}

eliminarGrupo = function (id, tipo) {
    if (confirm('¿Está seguro de eliminar este registro?')) {
        var idMsg = 'mensajeGrupo';
        if (tipo == '2') idMsg = 'mensajeGrupoMinimo';

        $.ajax({
            type:'POST',
            url: controlador + 'grupolineadelete',
            data: {
                idGrupo: id
            },
            dataType: 'json',
            success: function (result) {

                if (result == 1) {
                    mostrarMensaje(idMsg, 'exito', 'La operación se realizó correctamente');
                    consultarGrupo(tipo);
                }
                else if (result == 2)
                {
                    mostrarMensaje(idMsg, 'alert', 'No se puede eliminar ya que existen lineas relacionadas.');
                }
                else {
                    mostrarMensaje(idMsg, 'error', 'Se ha producido un error');
                }
            },
            error: function () {
                mostrarMensaje('mensajeGrupo', 'error', 'Se ha producido un error');
            }
        })
    }
}


/*****METODOS PARA ENLACES DE LINEAS*****/

enlacesGrupo = function (id, tipo) {
    $.ajax({
        type: 'POST',
        url: controlador + 'enlaceindex',
        data: {
            idGrupo: id
        },
        global: false,
        success: function (evt) {
            $('#contenidoGrupoLinea').html(evt);
            setTimeout(function () {
                $('#popupGrupoLinea').bPopup({
                    autoClose: false
                });
            }, 50);

            if (tipo == '1') {
                $('#tituloGrupo').text('Configuración de Grupo de Líneas');
            }
            else if (tipo == '2') {
                $('#tituloGrupo').text('Configuración de Grupo de Líneas Flujo Mínimo');
            }

            enlaceList(id);

            $('#btnAgregarEnlace').on('click', function () {
                enlaceSave()                
            });

            $('#btnCancelarEnlace').on('click', function () {
                $('#popupGrupoLinea').bPopup().close();
            });

            $('#hfIdGrupoEnlace').val(id);
        },
        error: function () {
            mostrarMensaje('mensajeGrupo', 'error', 'Se ha producido un error');
        }
    });
}

enlaceList = function (idGrupo)
{
    $.ajax({
        type: 'POST',
        url: controlador + 'enlacelist',
        data: {
            idGrupo: idGrupo
        },
        global: false,
        success: function (evt) {
            $('#contentEnlace').html(evt);
            $('#tablaEnlace').dataTable({
                "sDom": 't',
            });
        },
        error: function () {
            mostrarMensaje('mensajeEnlace', 'error', 'Se ha producido un error');
        }
    });
}


enlaceSave = function () {

    if ($('#cbEnlace').val() != "") {
        var idLinea = $('#cbEnlace').val();
        var idGrupo = $('#hfIdGrupoEnlace').val();

        $.ajax({
            type: 'POST',
            url: controlador + 'enlacesave',
            data: {
                idGrupo: idGrupo,
                idLinea: idLinea
            },
            dataType: 'json',
            global:false,
            success: function (result) {

                if (result == 1) {
                    mostrarMensaje('mensajeEnlace', 'exito', 'La operación se realizó correctamente.');
                    enlaceList(idGrupo);
                }
                else if (result == 2) {
                    mostrarMensaje('mensajeEnlace', 'alert', 'La línea ya se encuentra agregada.');
                }
                else {
                    mostrarMensaje('mensajeEnlace', 'error', 'Se ha producido un error');
                }
            },
            error: function () {
                mostrarMensaje('mensajeEnlace', 'error', 'Se ha producido un error');
            }
        });
    }
    else {
        mostrarMensaje('mensajeEnlace', 'alert', 'Por favor seleccione una línea.');
    }
}

enlaceDelete = function (idLinea) {
    if (confirm('¿Está seguro de eliminar este registro?'))
    {        
        var idGrupo = $('#hfIdGrupoEnlace').val();
        
        $.ajax({
            type: 'POST',
            url: controlador + 'enlacedelete',
            data: {
                idGrupo: idGrupo,
                idLinea: idLinea
            },
            dataType: 'json',
            global: false,
            success: function (result)
            {
                if (result == 1) {
                    mostrarMensaje('mensajeEnlace', 'exito', 'La operación se realizó correctamente.');
                    enlaceList(idGrupo);
                }
                else {
                    mostrarMensaje('mensajeEnlace', 'error', 'Se ha producido un error');
                }
            },
            error: function () {
                mostrarMensaje('mensajeEnlace', 'error', 'Se ha producido un error');
            }
        });
    }
}


/*NUEVOS MÉTODOS PARA REGION SEGURIDAD*/

/**** METODOS PARA GRUPOS ****/

consultarRegionSeguridad = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'regionseguridadlist',
        success: function (evt) {
            $('#listadoRegionSeguridad').html(evt);
            $('#tablaRegionSeguridad').dataTable({
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error');
        }
    });
};

editarRegionSeguridad = function (id) {
    $.ajax({
        type: 'POST',
        url: controlador + 'regionseguridadedit',
        data: {
            idRegion: id
        },
        global: false,
        success: function (evt) {
            $('#contenidoRegionSeguridad').html(evt);
            setTimeout(function () {
                $('#popupRegionSeguridad').bPopup({
                    autoClose: false
                });
            }, 50);

            $('#cbEstadoRegionSeguridad').val($('#hfEstadoRegionSeguridad').val());

            $('#cbDireccion').val($('#hfDireccion').val());


            $('#btnGrabarRegionSeguridad').on("click", function () {
                grabarRegionSeguridad();
            });

            $('#btnCancelarRegionSeguridad').on("click", function () {
                $('#popupRegionSeguridad').bPopup().close();
            });

        },
        error: function () {
            mostrarMensaje('mensajeRegionSeguridad', 'error', 'Se ha producido un error');
        }
    });
};

grabarRegionSeguridad = function () {
    var validacion = validarRegionSeguridad();

    if (validacion == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'regionseguridadsave',
            data: $('#frmRegistroRegionSeguridad').serialize(),
            dataType: 'json',
            global: false,
            success: function (result) {
                if (result == 1) {
                    mostrarMensaje('mensajeRegionSeguridad', 'exito', 'Los datos se grabaron correctamente.');
                    $('#popupRegionSeguridad').bPopup().close();
                    consultarRegionSeguridad();
                }
                else {
                    mostrarMensaje('mensajeEditRegionSeguridad', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensajeEditRegionSeguridad', 'error', 'Se ha producido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensajeEditRegionSeguridad', 'alert', validacion);
    }
};

validarRegionSeguridad = function () {
    var mensaje = "<ul>";
    var flag = true;
           
    if ($('#txtNombreRegionSeguridad').val() == '') {
        mensaje = mensaje + "<li>Ingrese el nombre del conjunto.</li>";
        flag = false;
    }
    
    if (!validarDecimal($('#txtValorM').val())) {
        mensaje = mensaje + "<li>Ingrese un valor de M válido.</li>";
        flag = false;
    }
    
    if ($('#cbDireccion').val() == '') {
        mensaje = mensaje + "<li>Seleccione una dirección.</li>";
        flag = false;
    }
    
    mensaje = mensaje + "</ul>";

    if (flag) {
        mensaje = "";
    }

    return mensaje;
};

eliminarRegionSeguridad = function (id) {
    if (confirm('¿Está seguro de eliminar este registro?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'regionseguridaddelete',
            data: {
                idRegion: id
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    mostrarMensaje('mensajeRegionSeguridad', 'exito', 'La operación se realizó correctamente');
                    consultarRegionSeguridad();
                }
                else if (result == 2) {
                    mostrarMensaje('mensajeRegionSeguridad', 'alert', 'No se puede eliminar ya que existen lineas relacionadas.');
                }
                else {
                    mostrarMensaje('mensajeRegionSeguridad', 'error', 'Se ha producido un error');
                }
            },
            error: function () {
                mostrarMensaje('mensajeRegionSeguridad', 'error', 'Se ha producido un error');
            }
        });
    }
};


/*****METODOS PARA ENLACES DE LINEAS*****/

configuracionRegionSeguridad = function (id) {
    $.ajax({
        type: 'POST',
        url: controlador + 'enlaceregionseguridadindex',
        data: {
            idRegion: id
        },
        global: false,
        success: function (evt) {
            $('#contenidoRegionSeguridad').html(evt);
            setTimeout(function () {
                $('#popupRegionSeguridad').bPopup({
                    autoClose: false
                });
            }, 50);

            enlaceRegionSeguridadList(id);

            $('#btnAgregarEnlaceRegionSeguridad').on('click', function () {
                enlaceRegionSeguridadSave();
            });

            $('#btnCancelarEnlaceRegionSeguridad').on('click', function () {
                $('#popupRegionSeguridad').bPopup().close();
            });

            $('#cbTipoRegionSeguridad').on('change', function () {
                cargarEquiposRegionSeguridad();
            });

            $('#hfCodigoRegionSeguridad').val(id);
        },
        error: function () {
            mostrarMensaje('mensajeRegionSeguridad', 'error', 'Se ha producido un error');
        }
    });
};

cargarEquiposRegionSeguridad = function () {
    $('#cbEnlaceRegionSeguridad').get(0).options.length = 0;

    if ($('#cbTipoRegionSeguridad').val() != "") {

        mostrarMensaje('mensajeEnlaceRegionSeguridad', 'message', 'Complete los datos por favor.');
        $.ajax({
            type: 'POST',
            url: controlador + 'obtenerequiposregionseguridad',
            data: {
                tipo: $('#cbTipoRegionSeguridad').val() 
            },
            dataType: 'json',
            global: false,
            success: function (result) {
                if (result != -1) {
                                        
                    $('#cbEnlaceRegionSeguridad').get(0).options[0] = new Option("--SELECCIONE--", "0");
                    $.each(result, function (i, item) {
                        $('#cbEnlaceRegionSeguridad').get(0).options[$('#cbEnlaceRegionSeguridad').get(0).options.length] = new Option(item.Nombretna, item.Equicodi);
                    });
                }
                else {
                    mostrarMensaje('mensajeEnlaceRegionSeguridad', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensajeEnlaceRegionSeguridad', 'error', 'Se ha producido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensajeEnlaceRegionSeguridad', 'alert', 'Seleccione tipo de equipo.');
    }
};

enlaceRegionSeguridadList = function (idRegion) {
    $.ajax({
        type: 'POST',
        url: controlador + 'enlaceregionseguridadlist',
        data: {
            idRegion: idRegion
        },
        global: false,
        success: function (evt) {
            $('#contentEnlaceRegionSeguridad').html(evt);
            $('#tablaEnlaceRegionSeguridad').dataTable({
                //"sDom": 't',
            });
        },
        error: function () {
            mostrarMensaje('mensajeEnlaceRegionSeguridad', 'error', 'Se ha producido un error');
        }
    });
};


enlaceRegionSeguridadSave = function () {

    if ($('#cbEnlaceRegionSeguridad').val() != "") {
        var idEquipo = $('#cbEnlaceRegionSeguridad').val();
        var idRegion = $('#hfCodigoRegionSeguridad').val();

        $.ajax({
            type: 'POST',
            url: controlador + 'enlaceregionseguridadsave',
            data: {
                idRegion: idRegion,
                idEquipo: idEquipo
            },
            dataType: 'json',
            global: false,
            success: function (result) {

                if (result == 1) {
                    mostrarMensaje('mensajeEnlaceRegionSeguridad', 'exito', 'La operación se realizó correctamente.');
                    enlaceRegionSeguridadList(idRegion);
                }
                else if (result == 2) {
                    mostrarMensaje('mensajeEnlaceRegionSeguridad', 'alert', 'El equipo ya se encuentra agregado.');
                }
                else {
                    mostrarMensaje('mensajeEnlaceRegionSeguridad', 'error', 'Se ha producido un error');
                }
            },
            error: function () {
                mostrarMensaje('mensajeEnlaceRegionSeguridad', 'error', 'Se ha producido un error');
            }
        });
    }
    else {
        mostrarMensaje('mensajeEnlaceRegionSeguridad', 'alert', 'Por favor seleccione un equipo.');
    }
};

enlaceRegionSeguridadDelete = function (idEquipo) {
    if (confirm('¿Está seguro de eliminar este registro?')) {
        var idRegion = $('#hfCodigoRegionSeguridad').val();

        $.ajax({
            type: 'POST',
            url: controlador + 'enlaceregionseguridaddelete',
            data: {
                idRegion: idRegion,
                idEquipo: idEquipo
            },
            dataType: 'json',
            global: false,
            success: function (result) {
                if (result == 1) {
                    mostrarMensaje('mensajeEnlaceRegionSeguridad', 'exito', 'La operación se realizó correctamente.');
                    enlaceRegionSeguridadList(idRegion);
                }
                else {
                    mostrarMensaje('mensajeEnlaceRegionSeguridad', 'error', 'Se ha producido un error');
                }
            },
            error: function () {
                mostrarMensaje('mensajeEnlaceRegionSeguridad', 'error', 'Se ha producido un error');
            }
        });
    }
};


/*FIN DE METODOS DE REGIONES DE SEGURIDAD*/




/**** METODOS COMUNES ****/

mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}

validarNumero = function (item, evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;

    if (charCode == 46) {
        var regex = new RegExp(/\./g)
        var count = $(item).val().match(regex).length;
        if (count > 1) {
            return false;
        }
    }

    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }

    return true;
}

validarNumeroEntero = function (item, evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;

    if (charCode == 46) {        
        return false;        
    }

    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }

    return true;
}

validarDecimal = function (n) {
    if (n == "")
        return false;

    var count = 0;
    var strCheck = "-0123456789.";
    var i;

    for (i in n) {
        if (strCheck.indexOf(n[i]) == -1)
            return false;
        else {
            if (n[i] == '.') {
                count = count + 1;
            }
        }
    }
    if (count > 1) return false;
    return true;
}
