//Declarando las variables 
let controlador = siteRoot + 'ProyectoActualizacion/';
let hot;
let hotOptions;
let evtHot;
let mensajeError;

$(function () {
    $("#fArea").val("0");
    buscarListaProyectos();

    $("#popUpEditar").addClass("general-popup");

    $("#popUpEqModificacion").hide();

    $('#btnBuscar').click(function () {
        consultarListaProyectos();
    });
    $('#btnRegresar').click(function () {
        regresar();
    });
    $('#btnExportarEqModificacion').click(function () {
        exportarListado();
    });

    $('#popfFechaCreacion').Zebra_DatePicker({
        show_clear_date: 0
    });

    $('#popfFechaUltActualizacion').Zebra_DatePicker({
        show_clear_date: 0
    });

    $('#btnNuevoProyecto').click(function () {
        editarProyecto(0, 1);
    });

    let today = new Date();
    let yyyy = today.getFullYear();
    let mm = today.getMonth() + 1; 
    let dd = day_of_the_month(today);
    let maxFecha = dd + "/" + mm + "/" + yyyy;

    $('#fecha').Zebra_DatePicker({
        show_clear_date: 0,
        direction: [false, maxFecha]
    });


    $('#fFechaInicio').Zebra_DatePicker({
        direction: [false, maxFecha],
        show_clear_date: 0
    });

    $('#fFechaFin').Zebra_DatePicker({
        direction: [false, maxFecha],
        show_clear_date: 0
    });

    $('#btnGrabar').click(function () {
        guardar();
    });

    $('#btnBuscarEO').click(function () {
        limpiarBarraMensaje("mensaje_popupProyecto");
        limpiarPopupBusquedaConEO();
        abrirPopup("popupBusquedaEO");
        cargarEmpresasSegunTipoConEO();
    });

    $('input[type=radio][name=CtgFlagExcluyente]').on('change', function () {
        if ($(this).val() == "S") {
            $("#btnBuscarEO").show();
        } else if ($(this).val() == "N") {
            $("#btnBuscarEO").hide();
        }
    });

    $('#cbPBEOTipoEmpresa').change(function () {
        cargarEmpresasSegunTipoConEO();
    });

    $('#btnConfirmarEO').unbind();
    $('#btnConfirmarEO').click(function () {
        confirmarEstudioEO();
    });


    $('#btnExportarExcel').click(function () {
        exportarListado();
    });
    
});

let regresar = function () {
    $("#popUpEqModificacion").hide();
    $("#divPrincipal").show();
}


let limpiarArray = function (arrayControles) {
    for (let control of arrayControles) {
        $('#' + control).val('');
    }
};

$.fn.dataTable.ext.type.order['date-dd-mm-yyyy-pre'] = function (date) {
    let parts = date.split('/');
    return new Date(parts[2], parts[1] - 1, parts[0]).getTime();
};

let consultarListaProyectos = function () {
    let fechaStr = $('#fFechaInicio').val()
    let fechaInicio = this.convertirCadenaFecha(fechaStr);
    let fechaFin = this.convertirCadenaFecha($('#fFechaFin').val());

    if (fechaFin < fechaInicio) {
        alert("La fecha Realizado desde no debe ser mayor que Realizado hasta");
        return;
    }

    let datos = {
        iArea: $('#fArea').val(),
        sNombre: $('#fNombre').val(),
        sFechaInicio: $('#fFechaInicio').val(),
        sFechaFin: $('#fFechaFin').val()
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
                    columnDefs: [
                        { type: 'date-dd-mm-yyyy', targets: 4 }
                    ],
                    language: {
                        info: 'Mostrando página _PAGE_ de _PAGES_',
                        infoEmpty: 'No hay registros disponibles',
                        infoFiltered: '(filtrado de _MAX_ registros totales)',
                        lengthMenu: 'Mostrar _MENU_ registros por página',
                        zeroRecords: 'No se encontró nada'
                    },
                    order: [[4, 'desc'],[1, 'asc']]
                });

                $('.dataTables_filter input').attr('maxLength', 50);


            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    }, 100);
};

function buscarListaProyectos() {
    
    consultarListaProyectos();
}
function editarProyecto(e, accion) {

    estilosProyecto(accion);
    
    $.ajax({
        type: 'POST',
        url: controlador + "Editar",
        data: {
            id: e
        },
        success: function (resultado) {
            
            if (resultado != -1) {

                limpiarProyecto();

                $("#hCodigo").val(resultado.Codigo);
                $("#area").val(resultado.idArea);
                $("#titular").val(resultado.idFamilia);
                $("#codigoNemoTecnico").val(resultado.CodigoNemoTecnico);
                $("#nombres").val(resultado.Nombre);
                $("#fecha").val(resultado.FechaRegistro);
                $("#descripcion").val(resultado.Descripcion);

                if (resultado.Eppproyflgtieneeo == "0") {
                    $("#optRadioN").prop("checked", true);
                    $("#optRadioS").prop("checked", false);
                    $("#btnBuscarEO").hide();
                } else if (resultado.Eppproyflgtieneeo == "1") {
                    $("#optRadioN").prop("checked", false);
                    $("#optRadioS").prop("checked", true);

                    if (accion == "3") {
                        $("#btnBuscarEO").hide();
                    } else {
                        $("#btnBuscarEO").show();
                    }

                    
                    
                }

                setTimeout(function () {
                    abrirPopup("popupProyecto");
                }, 50);

            }

        },
        error: function (err) {
            mostrarError();
        }
    });
};

function limpiarProyecto() {
    $("#hCodigo").val("");
    $("#area").val("");
    $("#titular").val("");
    $("#codigoNemoTecnico").val("");
    $("#nombres").val("");
    $("#fecha").val("");
    $("#descripcion").val("");
}

function validacionProyecto() {

    let msj = "";

    if ($('#area').val() <= 0) {
        msj = "Seleccione Área COES\n";
    }

    if ($('#titular').val() <= 0) {
        msj = msj + "Seleccione Titular\n";
    }

    if ($("#codigoNemoTecnico").val() == "" || $('#codigoNemoTecnico').val().trim().length == 0) {
        msj = msj + "Ingrese Código\n";
    }

    if ($("#nombres").val() == "" || $('#nombres').val().trim().length == 0) {
        msj = msj + "Ingrese Nombre\n";
    }

    if ($("#fecha").val() == "") {
        msj = msj + "Ingrese Fecha\n";
    }

    if (msj != null && msj != "") {
        alert(msj);
        return false;
    } else {
        return true;
    }

}

function estilosProyecto(accion) {
    //1:NUEVO, 2:EDITAR, 3:CONSULTAR
    if (accion == "1") {

        $("#tituloPopup").text("Crear datos generales del proyecto");

        habilitarInput("area");
        habilitarInput("titular");
        habilitarInput("codigoNemoTecnico");
        habilitarInput("nombres");
        habilitarInput("fecha");

        habilitarInput("descripcion");

        $("input[type=radio]").attr('disabled', false);
        $("#optRadioN").prop("checked", true);

        $("#btnBuscarEO").hide();
        $("#btnGrabar").show();

    } else if (accion == "3") {

        $("#tituloPopup").text("Consultar datos generales del proyecto");

        deshabilitarInput("area");
        deshabilitarInput("titular");
        deshabilitarInput("codigoNemoTecnico");
        deshabilitarInput("nombres");
        deshabilitarInput("fecha");

        deshabilitarInput("descripcion");

        $("input[type=radio]").attr('disabled', true);
        $("#optRadioN").prop("checked", true);

        $("#btnBuscarEO").hide();
        $("#btnGrabar").hide();

    } else if (accion == "2") {

        $("#tituloPopup").text("Editar datos generales del proyecto");

        habilitarInput("area");
        habilitarInput("titular");
        habilitarInput("codigoNemoTecnico");
        habilitarInput("nombres");
        habilitarInput("fecha");
        habilitarInput("descripcion");

        $("input[type=radio]").attr('disabled', false);
        $("#optRadioN").prop("checked", true);

        $("#btnBuscarEO").hide();
        $("#btnGrabar").show();

    }
}

function habilitarInput(id) {
    $('#' + id).prop("disabled", false);
}

function deshabilitarInput(id) {
    $('#' + id).prop("disabled", true);
}


let guardar = function () {
    
    if (validacionProyecto()) {

        let codigo = $('#hCodigo').val();

        let textoConfirmacion = codigo > 0 
        ? '¿Está seguro de editar datos generales del proyecto?' 
        : '¿Está seguro de agregar datos generales del proyecto?';

        let codigoNemo = $('#codigoNemoTecnico').val()?.trim() || "";
        let nombre = $('#nombres').val()?.trim() || "";
        let descripcion = $('#descripcion').val()?.trim() || "";
        let estado = $('input[name="CtgFlagExcluyente"]:checked').val() === 'S' ? "1" : "0";

        if (confirm(textoConfirmacion)) {

            let datos = {
                iArea: $('#area').val(),
                iTitular: $('#titular').val(),
                sEstado: estado,
                sCodigoNemoTecnico: codigoNemo,
                sNombre: nombre,
                sDescripcion: descripcion,
                sFecha: $('#fecha').val(),
                iCodigo: $('#hCodigo').val(),
            };
        
            $.ajax({
                type: 'POST',
                url: controlador + 'GuardarProyectoActualizacion',
                dataType: 'json',
                data: datos,
                cache: false,
                success: function (resultado) {
                    if (resultado == 1) {
                        $('#mensaje').css("display", "none");
                        cerrarPopup("popupProyecto");
                        buscarListaProyectos();
                    } else if (resultado == 2) {
                        alert("Código duplicado, ingrese otro.");
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
};

let eliminar = function (e) {
    if (confirm('¿Está seguro de eliminar el proyecto de actualización?')) {

        let datos = {
            iCodigo: e,
        };

        $.ajax({
            type: 'POST',
            url: controlador + 'EliminarProyectoActualizacion',
            dataType: 'json',
            data: datos,
            cache: false,
            success: function (val) {
                if (val.Resultado == 1) {
                    $('#mensaje').css("display", "none");
                    cerrarPopup("popupProyecto");
                    buscarListaProyectos();
                } else if (val.Resultado == 2) {
                    alert(val.Mensaje);
                    $('#mensaje').css("display", "none");
                } else {
                    mostrarError();
                }
            },
            error: function () {
                mostrarError();
            }
        });
    }
};

let mostrarError = function () {
    $('#mensaje').removeClass("action-exito");
    $('#mensaje').addClass("action-error");
    $('#mensaje').text("Ha ocurrido un error");
    $('#mensaje').css("display", "block");
};

function convertirCadenaFecha(dateString) {
    if (dateString != '') { 
    let d = dateString.split("/");
        let dat = new Date(d[2] + '/' + d[1] + '/' + d[0]);
        return dat;
    } else {
        return null;
    }
    
};

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

function cargarEmpresasSegunTipoConEO() {
    limpiarBarraMensaje("mensaje_popupBusquedaEO");
    $("#listadoEstudio").html("");

    let valSel = $("#cbPBEOTipoEmpresa").val();
    let tipo;

    if (isNaN(valSel)) {
        tipo = -3;
    } else {
        tipo = parseInt($("#cbPBEOTipoEmpresa").val())
    }


    $("#cbPBEOEmpresa").empty();


    if (tipo == -3) {
        mostrarMensaje('mensaje_popupBusquedaEO', 'alert', "Seleccionar un tipo de empresa correcto.");
    } else {

        $.ajax({
            type: 'POST',
            url: controlador + 'CargarEmpresasXTipo',
            data: {
                tipoEmpresa: tipo
            },

            success: function (evt) {
                if (evt.Resultado != "-1") {
                    let numEmpresas = evt.ListaEmpresas.length;
                    if (numEmpresas > 0) {

                        //Lleno una variable con todos los datos de las empresas   
                        $('#cbPBEOEmpresa').get(0).options[0] = new Option("--  Seleccione Empresa  --", "-5");
                        $.each(evt.ListaEmpresas, function (i, item) { //item es string                           
                            $('#cbPBEOEmpresa').get(0).options[$('#cbPBEOEmpresa').get(0).options.length] = new Option(item.Emprnomb, item.Emprcodi); // listaIdentificadores es List<string>, si es objeto usar asi item.Pfrpernombre
                        });

                        $('#cbPBEOEmpresa').multipleSelect({
                            filter: true,
                            single: true,
                            onClose: function () {
                            }
                        });
                        $("#cbPBEOEmpresa").multipleSelect("setSelects", [-5]);


                        $('#cbPBEOEmpresa').change(function () {
                            mostrarListadoEstudioEO();
                        });

                        $('#btnConfirmarEO').unbind();
                        $('#btnConfirmarEO').click(function () {
                            confirmarEstudioEO();
                        });

                    } else {
                        $('#cbPBEOEmpresa').get(0).options[0] = new Option("--  Seleccione Empresa  --", "-5");
                    }
                } else {
                    mostrarMensaje('mensaje_popupBusquedaEO', 'error', "Ha ocurrido un error: " + evt.Mensaje);

                }
            },
            error: function (err) {
                mostrarMensaje('mensaje_popupBusquedaEO', 'error', 'Se ha producido un error al cargar el listado de empresas para el tipo escogido.');

            }
        });
    }
}

function mostrarListadoEstudioEO() {
    limpiarBarraMensaje("mensaje_popupBusquedaEO");

    let empresa = $('#cbPBEOEmpresa').val();
    let val = isNaN(empresa);
    if (!val) {
        $.ajax({
            type: 'POST',
            url: controlador + "listarEstudiosEo",
            data: {
                idEmpresa: empresa
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    let htmlLEO = dibujarTablaListadoEstudio(evt.ListadoEstudiosEo);
                    $("#listadoEstudio").html(htmlLEO);

                    $('#tablaEstudio').dataTable({
                        "scrollY": 150,
                        "scrollX": false,
                        "sDom": 'ft',
                        "ordering": false,
                        "iDisplayLength": -1,
                        language: {
                            info: 'Mostrando página _PAGE_ de _PAGES_',
                            infoEmpty: 'No hay registros disponibles',
                            infoFiltered: '(filtrado de _MAX_ registros totales)',
                            lengthMenu: 'Mostrar _MENU_ registros por página',
                            zeroRecords: 'No se encontró nada'
                        }
                    });

                    $('.dataTables_filter input').attr('maxLength', 20);

                } else {
                    mostrarMensaje('mensaje_popupBusquedaEO', 'error', evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje('mensaje_popupBusquedaEO', 'error', 'Ha ocurrido un error al mostrar estudios EO.');
            }
        });
    }
}

function dibujarTablaListadoEstudio(listaEstudio) {

    let cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono" id="tablaEstudio" >
       <thead>
           <tr style="height: 22px;">
               <th style='width:90px;'>Sel.</th>
               <th>Código Estudio</th>
               <th>Nombre Estudio</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (let key in listaEstudio) {
        let item = listaEstudio[key];

        cadena += `
            <tr>
                <td style='width:90px;'>        
                    <input type="radio" id="rdEstudio_${item.Esteocodi}" name="rdEstudioEO" value="${item.Esteocodi}">
                </td>
                <td>${item.Esteocodiusu}</td>
                <td>${item.Esteonomb}</td>
               
           </tr >           
        `;
    }

    cadena += `                   
        </tbody >
    </table >
    `;

    return cadena;
}

function confirmarEstudioEO() {
    limpiarBarraMensaje("mensaje_popupBusquedaEO");
    let filtro = datosConfirmar();
    let msg = validarDatosConfirmar(filtro);

    if (msg == "") {
        let idEstudioEO = parseInt(filtro.seleccionado.value) || 0;
        $.ajax({
            type: 'POST',
            url: controlador + "obtenerDatoEstudioEO",
            data: {
                esteocodi: idEstudioEO
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    limpiarDatosProyectoNuevo();
                    completarDatosEstudioEnFormulario(evt.EstudioEO);

                    cerrarPopup('popupBusquedaEO');


                } else {
                    mostrarMensaje('mensaje_popupBusquedaEO', 'error', evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje('mensaje_popupBusquedaEO', 'error', 'Ha ocurrido un error al mostrar estudios EO.');
            }
        });
    } else {
        mostrarMensaje('mensaje_popupBusquedaEO', 'alert', msg);
    }
}

function datosConfirmar() {
    let filtro = {};

    let radioSel = document.querySelector('input[name="rdEstudioEO"]:checked');

    filtro.seleccionado = radioSel;
    filtro.empresa = $("#cbPBEOEmpresa").val();

    return filtro;
}

function validarDatosConfirmar(datos) {

    let msj = "";

    if (datos.empresa == -5) {
        msj += "<p>Debe seleccionar una empresa.</p>";
    } else if (datos.seleccionado == null) {
            msj += "<p>Debe seleccionar un Código de Estudio EO.</p>";
    }

    return msj;

}

function limpiarDatosProyectoNuevo() {
    $("#titular").val("");
    $("#codigoNemoTecnico").val("");
    $("#nombres").val("");
}

function completarDatosEstudioEnFormulario(objEstudioEO) {
    $("#titular").val(objEstudioEO.Emprcoditp);
    $("#codigoNemoTecnico").val(objEstudioEO.Esteocodiusu);
    $("#nombres").val(objEstudioEO.Esteonomb);
}

function limpiarPopupBusquedaConEO() {
    limpiarBarraMensaje("mensaje_popupBusquedaEO");

    $("#cbPBEOTipoEmpresa").val("-2");
    $("#cbPBEOEmpresa").val("-5");

}

function mostrarMensaje(id, tipo, mensaje) {
    $("#" + id).css("display", "block");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);

}

function limpiarBarraMensaje(id) {
    $('#' + id).css("display", "none");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-message');
    $('#' + id).html('');
}

let consultarEqModificacion = function (e) {
    setTimeout(function () {
        $.ajax({
            type: 'POST',
            url: controlador + 'ListaEqModificado',
            data: { id: e },
            success: function (evt) {
                $('#listaEqModificado').html(evt);
                $('#tablaListadoEqModificado').dataTable({
                    "iDisplayLength": 25,
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
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    }, 100);
};

function consultarEquipamiento(e) {

    $.ajax({
        type: 'POST',
        url: controlador + "ConsultaEqupamientoModificado?id=" + e,
        data: {
            id: e
        },
        success: function (evt) {
            $('#equipamientoModificacion').html(evt);
            consultarEqModificacion(e);
            setTimeout(function () {
                $("#divPrincipal").hide();
                $("#popUpEqModificacion").show();

            }, 50);

        },
        error: function () {
            mostrarError();
        }
    });

};

function exportarListado() {
    let codigoreporte = $('#Codigo').val();
    let memotecnico = $('#memotecnico').val();
    $.ajax({
        type: 'POST',
        url: controlador + 'GenerarReporteRele',
        data: {
            epproycodi: codigoreporte,
            memotecnico: memotecnico
        },
        dataType: 'json',
        success: function (evt) {

            if (evt.Resultado != "-1") {
                window.location = controlador + "ExportarReporte?file_name=" + evt.Resultado;
            } else {
                mostrarMensaje('mensaje', 'error', 'Error: ' + evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function day_of_the_month(d) {
    return (d.getDate() < 10 ? '0' : '') + d.getDate();
}
function descargarArchivo(nomArchivo) {
    window.location = controlador + 'DescargarArchivo?fileName=' + nomArchivo;
}