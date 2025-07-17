var controlador = siteRoot + 'admin/empresa/';

$(function(){

    $('#btnConsultar').click(function () {
        buscar();
    })

    $('#cntBusqueda').keypress(function (e) {
        if (e.keyCode == '13') {
            $('#btnConsultar').click();
        }
    });

    $('#btnNuevo').click(function () {
        editarEmpresa(0);
    });

    $('#btnExportar').click(function () {
        exportar();
    });

    buscar();
});

buscar = function () {
    pintarPaginado();
    pintarBusqueda(1);
}

pintarPaginado = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "paginado",
        data: {
            nombre: $('#txtNombreFiltro').val(),
            ruc: $('#txtRucFiltro').val(),
            idTipoEmpresa: $('#cbTipoEmpresaFiltro').val(),
            estado: $('#cbEstadoFiltro').val(),
            empresaSein: $('#cbSeinFiltro').val()
        },
        success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado();
        },
        error: function () {
           mostrarError('mensaje');
        }
    });
}

pintarBusqueda = function (nroPagina) {
    $('#hfNroPagina').val(nroPagina);
    $.ajax({
        type: 'POST',
        url: controlador + "listado",
        data: {
            nombre: $('#txtNombreFiltro').val(),
            ruc: $('#txtRucFiltro').val(),
            idTipoEmpresa: $('#cbTipoEmpresaFiltro').val(),
            estado: $('#cbEstadoFiltro').val(),
            empresaSein: $('#cbSeinFiltro').val(),
            nroPagina: nroPagina,
            indicadorGrabar: $('#hfIndicadorGrabar').val()
        },
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");
            $('#listado').html(evt);
            $('#tabla').dataTable({
                "scrollY": 457,
                "scrollX": false,
                "sDom": 't',
                "ordering": false,
                "iDisplayLength": 50
            });
        },
        error: function () {
          mostrarError('mensaje');
        }
    });
}

editarEmpresa = function (id) {
    $.ajax({
        type: 'POST',
        url: controlador + 'editar',
        data: {
            idEmpresa: id,
            indicadorGrabar: $('#hfIndicadorGrabar').val()
        },
        success: function (evt) {
            $('#contenidoDetalle').html(evt);

            $('#cbTipoEmpresa').val($('#hfTipoEmpresa').val());
            $('#cbEmpresaSein').val($('#hfEmpresaSein').val());
            $('#cbProveedor').val($('#hfProveedor').val());
            $('#cbEstado').val($('#hfEstado').val());
            $('#cbIntegrante').val($('#hfIntegrante').val());
            $('#trDemanda').hide();            

            if ($('#hfIndDemanda').val() == "S") {
                $('#cbIndDemanda').prop('checked', true);
                $('#trDemanda').show();
            }

            var tipoEmpresa = $('#hfTipoEmpresa').val();
            var listaEmpresa = $('#hfListaEmpresa').val();
            var array = listaEmpresa.split(',');

            $('#tbTipoEmpresa>tbody tr').each(function (i) {              
                $checkbox = $(this).find('input:checkbox');
                $radio = $(this).find('input:radio');

                if ($radio.val() === tipoEmpresa) {
                    $radio.prop('checked', true);
                }

                for (var j in array) {
                    if ($checkbox.val() === array[j]) {
                        $checkbox.prop('checked', true);
                    }
                }                
            });

            //$('.cb-tipoempresa').on('click', function () {              
               
            //});


            $('#btnGrabar').click(function () {
                grabar();
            })

            $('#btnCancelar').click(function () {
                cancelar();
            });

            $('#cbTipoEmpresa').change(function () {
                varificarDemanda();
            });

            $('#txtRuc').change(function () {
                obtenerDatos();
            });

            $('#btnBuscarRuc').click(function () {
                window.open('http://www.sunat.gob.pe/cl-ti-itmrconsruc/jcrS03Alias',
                    "_blank", "toolbar=no,scrollbars=no,resizable=yes,status=no,top=400,left=500,width=700,height=400");
            });

            $('#rdBienes').change(function () {
                var option = $('#rdBienes').prop('checked');

                if (option) {
                    $('#hddRubro').val(1);

                }
            })

            $('#rdServicios').change(function () {
                var option = $('#rdServicios').prop('checked');

                if (option) {
                    $('#hddRubro').val(2);

                }
            })

            $('#rdOtros').change(function () {
                var option = $('#rdOtros').prop('checked');

                if (option) {
                    $('#hddRubro').val(3);  //Modificado por sts 17 oct 2017

                }
            })

            $('#ddl-options').change(function () {
                var option = $('#ddl-options').val();

                if (option == "S") {

                    //$('#cbEmpresaSein').prop('disabled', false);
                    $('#cbEmpresaSein').val(0);                     //Agregado por sts 17 oct 2017
                    //$('#cbTipoEmpresa').prop('disabled', false);

                    $('#trSein').show();
                    $('#trTipoAgente').show();
                    $('#trRubro').hide();

                }
                else if (option == "N") {
                    //$('#cbEmpresaSein').prop('disabled', true);
                    //$('#cbTipoEmpresa').prop('disabled', true);
                    $('#hfEmpresaSein').val("N");
                    $('#cbTipoEmpresa').val("-1");
                    $('#trSein').hide();
                    $('#trTipoAgente').hide();
                    $('#trRubro').show();
                    
                }
                else if (option == "NA") {
                    //$('#cbEmpresaSein').prop('disabled', true);
                    //$('#cbTipoEmpresa').prop('disabled', true);

                    $('#trSein').show();
                    $('#trTipoAgente').show();
                    $('#trEmpPrivada').hide();
                    $('#trRubro').hide();
                }
            })

            $('#cbEmpresaSein').change(function () {
                var option = $('#cbEmpresaSein').val();
                
                if (option == "S") {

                    $('#hfEmpresaSein').val("S");

                }
                else if (option == "N") {

                    $('#hfEmpresaSein').val("N");

                }
                else if (option == "0") {

                    $('#hfEmpresaSein').val("0");

                }
            })
            
            $('#cbProveedorf').val("N");
            if ($('#hfProveedor').val() == "S") {
                $('#cbProveedor').val("S");
            }

            $('#ddl-domicilioda').change(function () {

                if ($('#ddl-domicilioda').val() == 'N') {

                    $('#trRuc').show();
                    $('#ch-opciones').prop('disabled', false);

                    $('#trSein').show();
                    $('#trTipoAgente').show();
                    $('#trRubro').hide();
                }
                else {
                    $('#ch-opciones').prop('disabled', true);

                    $('#ch-opciones').prop('checked', false);
                    $('#ddl-options').val("NA");

                    $('#trSein').hide();
                    $('#trTipoAgente').hide();

                    $('#trDemanda').hide();
                    $('#trRubro').show();
                    if ($('#ddl-domicilioda').val() == 'E') {
                        $('#trRuc').hide();
                        $('#cbTipoEmpresa').val("-1");
                    }
                }

            })

            $('#ch-opciones').change(function () {
                var option = $('#ch-opciones').prop('checked');

                if (option == true) {
                    $('#ddl-options').prop('disabled', false);
                }
                else if (option == false) {
                    $('#ddl-options').prop('disabled', true);
                }

            })


            CargarParamEmpr(id);
            CargarRubro();

            setTimeout(function () {
                $('#popupDetalle').bPopup({
                    autoClose: false
                });
            }, 500);
        },
        error: function () {
          mostrarError('mensaje');
        }
    });
};
var flag = false;

validarTipoEmpresa = function (e) {

    $('input:radio[name="rbtipoempresa"]').prop('checked', false);

    if ($(e).is(':checked')) {
        $('input:radio[name="rbtipoempresa"][value="' + $(e).val() + '"]').prop('checked', true);
    }  
    $('#trDemanda').hide();   
    $('#hfTipoEmpresa').val("");
    
    if ($(e).val() == "4" && $(e).is(':checked') ) {      
        flag = true;
        $('#hfTipoEmpresa').val($(e).val());        
    }
    if ($(e).val() == "4" && $(e).is(':not(:checked)')) {
        flag = false;
    }
    if (flag) {
        $('#trDemanda').show();
    }  
};


validarTipoPrincipal = function (e) {
    if ($(e).is(':checked')) {

        $('#tbTipoEmpresa>tbody tr').each(function (i) {
            $checkbox = $(this).find('input:checkbox');         

            if ($checkbox.val() === $(e).val()) {
                $checkbox.prop('checked', true);
            }
        });
    }
};

darBajaEmpresa = function (id) {
    if (confirm('¿Está seguro de realizar esta operación?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'darbaja',
            data: {
                idEmpresa: id
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    mostrarExito('La operación se realizó correctamente.', 'mensaje');
                    buscar();
                }
                else {
                    mostrarError('mensaje');
                }
            },
            error: function () {
               mostrarError('mensaje');
            }
        });
    }
}

eliminarEmpresa = function (id) {
    if (confirm('¿Está seguro de realizar esta operación?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'eliminar',
            data: {
                idEmpresa: id
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    mostrarExito('La operación se realizó correctamente.', 'mensaje');
                    buscar();
                }
                else {
                   mostrarError('mensaje');
                }
            },
            error: function () {
               mostrarError('mensaje');
            }
        });
    }
}

grabar = function () {
    var mensaje = validar();

    if (mensaje == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'grabar',
            data: $('#frmRegistro').serialize(),
            dataType: 'json',
            success: function (result) {
                if (result.IdResultado == 1) {
                    mostrarExito('La operación se realizó correctamente.', 'mensaje');
                    //if ($('#hfCodigoEmpresa').val() == "0") {
                    //    grabarCyDOC(result.Id, result.Ruc);
                    //}
                    buscar();
                    cancelar();
                }
                else if (result.IdResultado == 2) {
                    mostrarValidaciones(result.Validaciones);
                }
                else {
                   mostrarError('mensajeEdit');
                }
            },
            error: function () {
               mostrarError('mensajeEdit');
            }
        });
    }
    else {
        mostrarAlerta(mensaje, 'mensajeEdit');
    }
}

grabarCyDOC = function (id, ruc) {
    var json = {
        "tipoCliente": "juridica",
        "numeroDocumento": ruc,
        "nombres": "",
        "apellidoPaterno": "",
        "apellidoMaterno": "",
        "empresaCodigo": id,
        "razonSocial": $('#txtRazonSocial').val()
    };

    console.log(json);

    $.ajax({
        url: "https://gestiondocumental.coes.org.pe/std/std-rest/empresa",
        method: 'POST',
        dataType: 'json',
        crossDomain: true,
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(json),
        cache: false,
        success: function (data) {
            console.log(data);
        },
        error: function (error) {
            console.log(error);
        }
    });
};

validar = function () {
    var mensaje = "<ul>";
    var flag = true;

    //if ($('#txtAbreviatura').val() == "") {
    //    mensaje = mensaje + "<li>Ingrese abreviatura.</li>";
    //    flag = false;
    //}
    if ($('#txtNombre').val() == "") {
        mensaje = mensaje + "<li>Ingrese nombre.</li>";
        flag = false;
    }
    //if ($('#cbTipoEmpresa').val() == "") {
    //    mensaje = mensaje + "<li>Seleccione tipo de empresa.</li>";
    //    flag = false;
    //}

    var tipoPrincipal = $('input[name=rbtipoempresa]:checked').val();
    var tiposEmpresas = "";
    var validacionTipo = false;
    var contadorTipo = 0;
    $('#tbTipoEmpresa>tbody tr').each(function (i) {
        $checkbox = $(this).find('input:checkbox');      

        if ($checkbox.is(':checked')) {
            contadorTipo++;
            tiposEmpresas = tiposEmpresas + $checkbox.val() + ",";
            if (tipoPrincipal === $checkbox.val()) {
                validacionTipo = true;
            }
        }
    });

    if (contadorTipo > 0) {
        if (validacionTipo == false) {
            mensaje = mensaje + "<li>Seleccione correctamente el tipo de empresa e indicar si es el principal.</li>";
            flag = false;
        }
        else if (validacionTipo === true) {
            $('#hfTipoEmpresa').val(tipoPrincipal);
            $('#hfListaEmpresa').val(tiposEmpresas);
        }        
    }
    if ($('#txtRuc').val() == "" && $('#ddl-domicilioda').val() == "N") {
        mensaje = mensaje + "<li>Ingrese el Nro de RUC.</li>";
        flag = false;
    }
    if ($('#cbEstado').val() == "") {
        mensaje = mensaje + "<li>Seleccione el estado.</li>";
        flag = false;
    }
    mensaje = mensaje + "</ul>";

    $('#hfIndDemanda').val('N')
    if ($('#cbIndDemanda').is(':checked')) $('#hfIndDemanda').val('S');

    if (flag) mensaje = "";

    return mensaje;
}

mostrarValidaciones = function (validaciones) {
    var mensaje = "<ul>";
    for (var j in validaciones) {
        mensaje = mensaje + "<li>" + validaciones[j] + "</li>";
    }
    mensaje = mensaje + "</ul>";

    mostrarAlerta(mensaje, 'mensajeEdit');
}



cancelar = function () {
    $('#popupDetalle').bPopup().close();
}

exportar = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "exportar",
        data: {
            nombre: $('#txtNombreFiltro').val(),
            ruc: $('#txtRucFiltro').val(),
            idTipoEmpresa: $('#cbTipoEmpresaFiltro').val(),
            estado: $('#cbEstadoFiltro').val(),
            empresaSein: $('#cbSeinFiltro').val()
        },
        dataType: 'json',
        cache: false,
        success: function (resultado) {
            if (resultado == 1) {
                location.href = controlador + "descargar";
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

obtenerDatos = function () {

    var nacionalidad = $('#ddl-domicilioda').val();;
    var ruc = $('#txtRuc').val();
    var isNumber = /^\d+$/.test(ruc);
    $('#txtNombre').val("");
    $('#txtRazonSocial').val("");

    if (nacionalidad == "N") {
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
                        mostrarAlerta('El RUC ingresado no existe.', 'mensajeEdit');
                    }
                    else if (result == -1) {
                       mostrarError('mensajeEdit');
                    }
                    else {
                        $('#txtNombre').val(result.NombreComercial);
                        $('#txtRazonSocial').val(result.RazonSocial);

                        if (result.NombreComercial != "") {
                            if (result.NombreComercial.trim() != "") {
                                $('#txtNombre').val($('#txtRazonSocial').val());
                            }
                        }
                    }
                },
                error: function () {
                    mostrarAlerta('Error al consultar a SUNAT', 'mensajeEdit');
                }
            });
            $('#loadEmpresa').hide();
        }
        else {
            mostrarAlerta('El RUC debe contener 11 dígitos.', 'mensajeEdit');
        }
    }
    else {
        if (nacionalidad != " ") {
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
                        $('#txtNombre').val(result.NombreComercial);
                        $('#txtRazonSocial').val(result.RazonSocial);

                        if (result.NombreComercial != "") {
                            if (result.NombreComercial.trim() != "") {
                                $('#txtNombre').val($('#txtRazonSocial').val());
                            }
                        }
                    
                },
                error: function () {
                    mostrarAlerta('Error al consultar a SUNAT', 'mensajeEdit');
                }
            });
            $('#loadEmpresa').hide();
        }
        else {
            mostrarAlerta('El RUC no es correcto.', 'mensajeEdit');
        }

    }

  
}

mostrarError = function (id){
    $('#' + id).removeClass();
    $('#' + id).addClass('action-error');
    $('#' + id).text('Ha ocurrido un error.');
}

mostrarExito = function (mensaje, id) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-exito');
    $('#' + id).html(mensaje);
}

mostrarAlerta = function (mensaje, id) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-alert');
    $('#' + id).html(mensaje);
}

CargarRubro = function () {
    var rubro = $('#hddRubro').val();

    if (rubro == 1) {
        $('#rdBienes').prop('checked', true);
    }
    else if (rubro == 2) {
        $('#rdServicios').prop('checked', true);
    }
    else if (rubro == 3) {
        $('#rdOtros').prop('checked', true);
    }
}

CargarParamEmpr = function (id) { //Modificado por sts 17 oct 2017

    $.ajax({
        type: 'POST',
        url: controlador + "ObtenerEmpresa",
        data: {
            idEmpresa: id
        },
        success: function (respuesta) {
            if (respuesta.Emprdomiciliada == "N") {
                $('#ddl-domicilioda').val(respuesta.Emprdomiciliada);
                $('#trRuc').show();

                $('#trRubro').hide();
                $('#trSein').show();
                $('#trTipoAgente').show();
            }
            else if (respuesta.Emprdomiciliada == "E") {
                $('#ddl-domicilioda').val((respuesta.Emprdomiciliada));
                $('#trRuc').hide();

                $('#trRubro').show();
                $('#trSein').hide();
                $('#trTipoAgente').hide();
                $('#trDemanda').hide();
            }
            else {
                $('#ddl-domicilioda').val("-1");
                $('#trRubro').hide();
            }

            //if (respuesta.Empragente == "S" && respuesta.Emprdomiciliada == "N") {
            if (respuesta.Empragente == "S") {
                $('#ch-opciones').prop('disabled', false);
                $('#ch-opciones').prop('checked', true);
                $('#ddl-options').prop('disabled', false);

                $('#trSein').show();
                $('#trTipoAgente').show();

                $('#trRubro').hide();
            }
            //else if (respuesta.Empragente == "N" && respuesta.Emprdomiciliada == "N") {
            else if (respuesta.Empragente == "N") {
                $('#ch-opciones').prop('disabled', false);
                $('#ch-opciones').prop('checked', true);
                $('#ddl-options').prop('disabled', false);

                $('#cbTipoEmpresa').val("-1");
                $('#cbEmpresaSein').val("N");

                $('#trSein').hide();
                $('#trTipoAgente').hide();

                $('#trRubro').show();
            }

            if (respuesta.Emprsein == "S" || respuesta.Emprsein == "N") {
                $('#cbEmpresaSein').val(respuesta.Emprsein);
            }
            else {
                $('#cbEmpresaSein').val("NA");
            }

            if (respuesta.Emprindproveedor == "S" || respuesta.Emprindproveedor == "N") {
                $('#cbProveedor').val(respuesta.Emprindproveedor);
            }
            else {
                $('#cbProveedor').val("N");
        }        

            if (respuesta.Empragente == "S" || respuesta.Empragente == "N") {
                $('#ddl-options').val(respuesta.Empragente);
            }
            else {
                $('#ddl-options').val("NA");
            }

            if (respuesta.Emprambito == "Pr" || respuesta.Emprambito == "Pu") {
                $('#cbAmbito').val(respuesta.Emprambito);
            }
            else {
                $('#cbAmbito').val("0");
            }
        },
        error: function () {
           mostrarError('mensaje');
        }
    });
}