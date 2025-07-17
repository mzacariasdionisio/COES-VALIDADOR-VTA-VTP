var controlador = siteRoot + 'InformeEjecutivoMen/reporteejecutivo/';
var controladorBarra = siteRoot + 'InformeEjecutivoMen/Barra/';

const NUEVO = 1;
const EDITAR = 2;
const IMG_EDITAR = '<img src="' + siteRoot + 'Content/Images/btn-edit.png" alt="Editar Evento" width="19" height="19" style="">';
const IMG_ELIMINAR = '<img src="' + siteRoot + 'Content/Images/btn-cancel.png" alt="Eliminar Evento" width="19" height="19" style="">';

$(function () {
  
    $('#btnNuevo').on('click', function () {
        $("#hdAccion").val(NUEVO);
        abrirPopup("popupBarra", NUEVO);
        limpiarPopup();
        document.getElementById('cbBarra').disabled = false;
    });
    
    $('#btnCancelar').on('click', function () {
        cerrarPopup("popupBarra");
    });

    $('#tab-container').easytabs({
        animate: false
    });

    $('#btnGrabar').on('click', function () {
        guardarBarra();
    });      

    $('#btnRegresar').on('click', function () {
        document.location.href = controlador + "index";
    });

    $('#btnAgregarAgrupacion').hide();

    $('#btnConsultarAgrupacion').on('click', function () {
        consultarAgrupaciones();
    });

    $('#cbAreaOperativa').on('change', function () {
        consultarAgrupaciones();
    });

    $('#btnAgregarAgrupacion').on('click', function () {
        editarAgrupacion("");
    });

    //$('#btnGrabar').hide();
    mostrarListado();
    
});

function mostrarListado() {
    $.ajax({
        type: 'POST',
        url: controladorBarra + "listarBarras",
        data: {
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                var htmlList = dibujarTablaListado(evt.ListadoBarras);
                $("#listado").html(htmlList);
                $('#tablaBarras').dataTable({
                    "scrollY": 480,
                    "scrollX": false,
                    "sDom": 'ft',
                    "ordering": false,
                    "iDisplayLength": -1
                });
            } else {
                mostrarMensaje('mensaje', 'error', evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function dibujarTablaListado(listado) {
    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono" id="tablaBarras" >
       <thead>
           <tr style="height: 22px;">
               <th style='width:90px;'>Acciones</th>               
               <th>Barra Transferencia</th>
               <th>Área Operativa</th>
                <th>Es barra ejecutiva mensual</th>               
               <th>Usuario</th>
               <th>Fecha</th>                
            </tr>
        </thead>
        <tbody>
    `;


    for (key in listado) {
        var item = listado[key];       
        var botonEliminar = '';
        var botonEditar = '';        
        botonEditar += `
                <a href="JavaScript:editarBarra(${item.Bararecodi},${EDITAR});">${IMG_EDITAR}</a>
            `;
        
        botonEliminar += `
                <a href="JavaScript:eliminarBarra(${item.Bararecodi});">${IMG_ELIMINAR}</a>
            `;
        
        if (item.EstadoActual == "En Proyecto") {
           
        }

        cadena += `
            <tr>
                <td style='width:90px;'>                    
                    ${botonEditar}
                    ${botonEliminar}
        `;
        cadena += `                    
                </td>
                <td>${item.Barrnombre}</td>
                <td>${item.Bararearea}</td>
                <td>${item.Barareejecutiva}</td>
                <td>${item.Barareusucreacion}</td>
                <td>${item.sFechaCreacion}</td>                
           </tr >           
        `;
    }

    cadena += `                   
        </tbody >
    </table >
    `;

    return cadena;
}

function guardarBarra() {
    //$("#hdAccion").val();
    //$('#hdBararecodi').val(id);

    limpiarBarraMensaje("mensaje_popup");

    var filtro = datosRequisitos();
    var msg = validarDatos(filtro);

    if (msg == "") {        
        $.ajax({
            type: 'POST',
            url: controladorBarra + "GuardarBarra",
            data: {
                bararecodi: filtro.Idbarraareacodi,
                barrcodi: filtro.Codibarra,
                bararearea: filtro.Areabarra,
                barareejecutiva: filtro.Barraejecutiva,
                accion: filtro.Accion                
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {

                    mostrarListado();
                    mostrarMensaje('mensaje_popup', 'exito', "Registro exitoso.");
                    //cerrarPopup("popupEvento");
                    //posFilaNueva = -1;

                } else {
                    mostrarMensaje('mensaje_popup', 'error', evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje('mensaje_popup', 'error', 'Ha ocurrido un error al guardar.');
            }
        });
    } else {
        mostrarMensaje('mensaje_popup', 'alert', msg);
    }
    
}

function datosRequisitos() {
    var filtro = {};
    filtro.Codibarra = $("#cbBarra").val();
    filtro.Areabarra = $("#cbBarraArea").val();
    filtro.Barraejecutiva = $('#cbEsBarraEjec').val();
    filtro.Accion = $("#hdAccion").val();
    filtro.Idbarraareacodi = parseInt($("#hdBararecodi").val()) || "0";
    return filtro;
}

function validarDatos(datos) {
    var msj = "";

    if (datos.codibarra == "-1") {
        msj += "<p>Debe seleccionar barra'.</p>";
    }

    if (datos.arearbarra == "-1") {
        msj += "<p>Debe seleccionar un Área operativa.</p>";
    }

    if (datos.barraejecutiva== "-1") {
        msj += "<p>Seleccione si es barra ejecutiva.</p>";
    }    
    return msj;
}

function eliminarBarra(id) {
    limpiarBarraMensaje("mensaje_popup");
    if (confirm('¿Desea eliminar el registro?')) {

        $.ajax({
            type: 'POST',
            url: controladorBarra + 'EliminarBarra',
            data: {
                bararecodi: id
            },
            dataType: 'json',
            success: function (evt) {
                if (evt.Resultado == "1") {
                    mostrarListado();
                    alert('Eliminación exitosa.');
                }
                else {
                    alert(evt.Mensaje);
                }
            },
            error: function () {
                alert('Se ha producido un error.');
            }
        });
    }
}

function editarBarra(id, accion) {
    $('#hdBararecodi').val(id);
    $("#hdAccion").val(accion);
    
    $.ajax({
        type: 'POST',
        url: controladorBarra + "DetallarBarra",
        data: { bararecodi: id },
        success: function (evt) {
            if (evt.Resultado != "-1") {                
                setearInfoBarra(evt.Barra);                
                abrirPopup('popupBarra', accion);

            } else {
                mostrarMensaje('mensaje', 'error', evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });


}


function setearInfoBarra(barra) {
    limpiarBarraMensaje("mensaje_popup");
    $('#cbBarra').val(barra.Barrcodi);
    $('#cbBarraArea').val(barra.Bararearea);
    $('#cbEsBarraEjec').val(barra.Barareejecutiva);
    $('#cbBarra').prop('disabled', false);
    document.getElementById('cbBarra').disabled = true;
}
///UTIL////
function abrirPopup(contentPopup, accion) {

    if (accion == EDITAR) {    
        $("#tituloPopup").html("<span>[Editar] Relación de barras de transferencia y área operativa</span>");
    }
    else {
        $("#tituloPopup").html("<span>[Registro] Relación de barras de transferencia y área operativa</span>");
    }

    setTimeout(function () {
        $("#" + contentPopup).bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    }, 50);
}


function consultarAgrupaciones() {
    $('#detalleAgrupacion').html("");
    if ($('#cbAreaOperativa').val() != "") {
        $.ajax({
            type: 'POST',
            url: controladorBarra + "agrupacion",
            data: {
                zona: $('#cbAreaOperativa').val()
            },
            success: function (evt) {
                $('#listaAgrupacion').html(evt);
                $('#tablaAgrupacion').dataTable({
                    "iDisplayLength": 25
                });
            },
            error: function (err) {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
        limpiarBarraMensaje("mensaje");
        $('#btnAgregarAgrupacion').show();
    }
    else {
        $('#listaAgrupacion').html("");        
        $('#btnAgregarAgrupacion').hide();
        mostrarMensaje('mensaje', 'alert', 'Seleccione área operativa.');
    }
}

function editarAgrupacion(agrupacion) {
    limpiarBarraMensaje("mensaje");
    $.ajax({
        type: 'POST',
        url: controladorBarra + 'DetalleAgrupacion',
        data: {
            zona: $('#cbAreaOperativa').val(),
            agrupacion: agrupacion
        },      
        success: function (evt) {
            $('#detalleAgrupacion').html(evt);

            $('#btnAgregarBarra').on('click', function () {
                agregarAgrupacionBarra();
            });

            $('#btnGrabarAgrupacion').on('click', function () {
                grabarAgrupacionBarra();
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}

function eliminarAgrupacion(agrupacion) {
    if (confirm('¿Está seguro de eliminar esta agrupación?')) {
        $.ajax({
            type: 'POST',
            url: controladorBarra + 'EliminarAgrupacion',
            data: {
                zona: $('#cbAreaOperativa').val(),
                agrupacion: agrupacion
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {                    
                    consultarAgrupaciones();
                    mostrarMensaje('mensaje', 'exito', 'Eliminación exitosa.');
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

function agregarAgrupacionBarra() {

    if ($('#cbBarraAgrupacion').val() != "") {
        var barraCodi = $('#cbBarraAgrupacion').val();

        var count = 0;
        var flag = true;
        $('#tablaBarraAgrupacion>tbody tr').each(function (i) {
            $punto = $(this).find('#hfItemBarraAgrupacion');
            if ($punto.val() == barraCodi) {
                flag = false;
            }
        });

        if (flag) {
            $('#tablaBarraAgrupacion> tbody').append(
                '<tr>' +              
                '   <td style="text-align:center">' +               
                '       <img src="' + siteRoot + 'Content/Images/btn-cancel.png" alt="" onclick="$(this).parent().parent().remove();" style="cursor:pointer" />' +
                '       <input type="hidden" id="hfItemBarraAgrupacion" value="' + barraCodi + '" /> ' +
                '   </td>' +
                '   <td>' + $("#cbBarraAgrupacion option:selected").text()+ '</td>' +
                '</tr>'
            );
            limpiarBarraMensaje("mensaje");
        }
        else {
            mostrarMensaje('mensaje', 'alert', 'La barra de transferencia ya ha sido agregada previamente.');            
        }
    }
    else {
        mostrarMensaje('mensaje', 'alert', 'Seleccione una barra de transferencia.');
    }
}

function grabarAgrupacionBarra() {
    var count = 0;
    var items = "";
    $('#tablaBarraAgrupacion>tbody tr').each(function (i) {
        $punto = $(this).find('#hfItemBarraAgrupacion');
        var constante = (count > 0) ? "," : "";
        items = items + constante + $punto.val();
        count++;
    });

    if (count != 0) {
        if ($('#txtNombreAgrupacion').val() != "") {
            $.ajax({
                type: 'POST',
                url: controladorBarra + 'GrabarAgrupacion',
                data: {
                    zona: $('#cbAreaOperativa').val(),
                    agrupacion: $('#hfNombreAgrupacion').val(),
                    nombre: $('#txtNombreAgrupacion').val(),
                    barras: items
                },
                dataType: 'json',
                success: function (result) {
                    if (result == 1) {                        
                        consultarAgrupaciones();
                        mostrarMensaje('mensaje', 'exito', 'Los datos de la agrupación se grabaron correctamente.');
                    }
                    else if(result == 2) {
                        mostrarMensaje('mensaje', 'alert', 'Debe cambiar el nombre de la agrupación porque ya está siendo utilizado.');
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
        else {
            mostrarMensaje('mensaje', 'alert', 'Debe ingresar un nombre para la agrupación.');
        }
    }
    else {
        mostrarMensaje('mensaje', 'alert', 'Debe agregar al menos una barra de transferencia a la agrupación.');
    }
}

function cerrarPopup(id) {
    $("#" + id).bPopup().close()
}

function limpiarPopup() {
    limpiarBarraMensaje("mensaje_popup");
    $('#cbBarra').val("-1");
    $('#cbBarraArea').val("-1");
    $('#cbEsBarraEjec').val("-1");
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