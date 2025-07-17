var controlador = siteRoot + 'CalculoResarcimiento/PuntoEntrega/';

const NUEVO = 1;
const EDITAR = 2;
const IMG_EDITAR = '<img src="' + siteRoot + 'Content/Images/btn-edit.png" alt="Editar Envío" width="19" height="19" style="">';
const IMG_VER = '<img src="' + siteRoot + 'Content/Images/btn-open.png" alt="Ver Envío" width="19" height="19" style="">';
const IMG_ELIMINAR = '<img src="' + siteRoot + 'Content/Images/btn-cancel.png" alt="Ver Envío" width="19" height="19" style="">';

$(function () {        
    $('#btnNuevo').click(function () {
        limpiarPopupPE();
        abrirPopup("popupPEAgregar");        
    });

    $('#GuardarPE').click(function () {
        guardarPtoEntrega(NUEVO, null);
    });

    $('#btnExportar').click(function () {
        exportarPE();
    });

    mostrarListadoPuntoEntrega();
});

function mostrarListadoPuntoEntrega() {
    limpiarBarraMensaje("mensaje");  

    $.ajax({
        type: 'POST',
        url: controlador + "listarPuntosEntrega",
        data: { },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                var htmlPE = dibujarTablaPuntosEntrega(evt.ListadoPuntoEntrega, evt.Grabar);
                $("#listado").html(htmlPE);

                $('#tablaPlantillas').dataTable({
                    "scrollY": 480,
                    "scrollX": false,
                    "sDom": 't',
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


function dibujarTablaPuntosEntrega(listaPuntosEntrega, opcionGrabar) {        

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono" id="tablaPlantillas" >
       <thead>
           <tr style="height: 22px;">
               <th style='width:90px;'>Acciones</th>
               <th>Punto de Entrega</th>
               <th>Nivel de Tensión</th>
               <th>Estado</th>
               <th>Usuario Creación / Modificación</th>
               <th>Fecha de Creación / Modificación</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in listaPuntosEntrega) {
        var item = listaPuntosEntrega[key];
        var fondoBaja = "#FFD6D6";
        var fondoActivo = "#FFFFFF";
        var enBaja = item.Repentestado == "B" ? true : false;
        var fondo = enBaja ? fondoBaja : fondoActivo;
        cadena += `
            <tr>
                <td style='width:90px; background: ${fondo}'>
                    <a href="JavaScript:editarPuntoEntrega(${item.Repentcodi});">${IMG_EDITAR}</a>
        `;
        if (!enBaja && opcionGrabar ) {
            cadena += `
                    <a href="JavaScript:eliminarPuntoEntrega(${item.Repentcodi});">${IMG_ELIMINAR}</a>
            `;
        }
        cadena += `
                </td>
                <td style='background: ${fondo}'>${item.Repentnombre}</td>
                <td style='background: ${fondo}'>${item.Rentabrev}</td>
                <td style='background: ${fondo}'>${item.EstadoDesc}</td>
                <td style='background: ${fondo}'>${item.UsuarioCreaciónModificacion}</td>
                <td style='background: ${fondo}'>${item.FechaCreaciónModificacionDesc}</td>
           </tr >           
        `;
    }

    cadena += `                   
        </tbody >
    </table >
    `;

    return cadena;
}

function limpiarPopupPE() {
    limpiarBarraMensaje("mensaje_popupAgregar");
    $("#txtPtoEntergaN").val("");
    $("#cbNivelTensionN").val("-1");
    $("#cbEstadoN").val("A");
}

function editarPuntoEntrega(repentcodi) {
    $.ajax({
        type: 'POST',
        url: controlador + "abrirPuntosEntrega",
        data: { repentcodi: repentcodi },
        success: function (evt) {
            if (evt.Resultado != "-1") {

                $("#txtPtoEntergaE").val(evt.PuntoEntrega.Repentnombre);
                $("#cbNivelTensionE").val(evt.PuntoEntrega.Rentcodi);
                $("#cbEstadoE").val(evt.PuntoEntrega.Repentestado);                
                abrirPopup("popupPEEditar");

                $('#ActualizarPE').click(function () {
                    guardarPtoEntrega(EDITAR, repentcodi);
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

function guardarPtoEntrega(accion, repentcodi) {
    limpiarBarraMensaje("mensaje_popupAgregar");
    limpiarBarraMensaje("mensaje_popupEditar");

    var datosPE = {};
    datosPE = getPuntoEntrega(accion);

    var msg = validarCamposPEAGuardar(datosPE);

    if (msg == "") {
        $.ajax({
            type: 'POST',
            url: controlador + "guardarPuntosEntrega",
            data: {
                nombre: datosPE.PENombre,
                niveltension: datosPE.PENivel,
                estado: datosPE.PEEstado,
                repentcodi: repentcodi,
                accion: accion
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    var txt = "";
                    if (accion == NUEVO) {
                        cerrarPopup("popupPEAgregar");
                        txt = "registrado";
                    }
                    if (accion == EDITAR) {
                        cerrarPopup("popupPEEditar");
                        txt = "actualizado";
                    }
                    
                    mostrarListadoPuntoEntrega();
                    mostrarMensaje('mensaje', 'exito', "El punto de entrega fue " + txt + " con éxito.");
                } else {
                    if (accion == NUEVO)
                        mostrarMensaje('mensaje_popupAgregar', 'error', evt.Mensaje);
                    if (accion == EDITAR)
                        mostrarMensaje('mensaje_popupEditar', 'error', evt.Mensaje);
                    
                }
            },
            error: function (err) {
                
                if (accion == NUEVO)
                    mostrarMensaje('mensaje_popupAgregar', 'error', 'Ha ocurrido un error.');
                if (accion == EDITAR)
                    mostrarMensaje('mensaje_popupEditar', 'error', 'Ha ocurrido un error.');
            }
        });
    } else {
        if (accion == NUEVO)
            mostrarMensaje('mensaje_popupAgregar', 'error', msg);
        if (accion == EDITAR)
            mostrarMensaje('mensaje_popupEditar', 'error', msg);        
    }
}

function getPuntoEntrega(accion) {
    var obj = {};

    if (accion == NUEVO) {
        obj.PENombre = $("#txtPtoEntergaN").val();
        obj.PENivel = parseInt($("#cbNivelTensionN").val());
        obj.PEEstado = "A";
    } else {
        if (accion == EDITAR) {
            obj.PENombre = $("#txtPtoEntergaE").val();
            obj.PENivel = parseInt($("#cbNivelTensionE").val());
            obj.PEEstado = $("#cbEstadoE").val();
        }
    }

    return obj;
}


function validarCamposPEAGuardar(puntoEntrega) {
    var msj = "";
    
    if (puntoEntrega.PENombre == "") {
        msj += "<p>Debe ingresar un nombre correcto.</p>";
    } 

    if (puntoEntrega.PENivel == -1) {
        msj += "<p>Debe escoger un nivel de tensión correcto.</p>";
    }

    if (puntoEntrega.PENivel == "") {
        msj += "<p>Debe escoger un estado correcto.</p>";
    }
        
    return msj;
}

function exportarPE() {
    limpiarBarraMensaje("mensaje");    

    $.ajax({
        type: 'POST',
        url: controlador + 'GenerarArchivoPE',
        data: { },
        dataType: 'json',
        success: function (evt) {
            if (evt.Resultado != "-1") {
                window.location = controlador + "Exportar?file_name=" + evt.Resultado;
            } else {
                mostrarMensaje('mensaje', 'error', 'Error: ' + evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function eliminarPuntoEntrega(repentcodi) {
    if (confirm('¿Desea eliminar el Punto de Entrega?')) {

        $.ajax({
            type: 'POST',
            url: controlador + 'EliminarPE',
            data: {
                repentcodi: repentcodi
            },
            dataType: 'json',
            success: function (evt) {
                if (evt.Resultado == "1") {
                    mostrarListadoPuntoEntrega();
                    mostrarMensaje('mensaje', 'exito', 'El registro se eliminó correctamente.');
                }
                else {
                    mostrarMensaje('mensaje', 'error', 'Error: ' + evt.Mensaje);
                }
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}

function cerrarPopup(id) {
    $("#" + id).bPopup().close()
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