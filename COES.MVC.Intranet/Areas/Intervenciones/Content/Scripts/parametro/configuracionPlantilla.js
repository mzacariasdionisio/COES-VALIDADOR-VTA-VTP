var controlador = siteRoot + 'Intervenciones/Parametro/';

const NUEVO = 0;
const VER = 1;
const EDITAR = 2;
const ELIMINAR = 3;
const HISTORIAL = 4;
var LISTADO_REQUISITOS = [];
var REQUISITO_WEB = null;
var IMG_EDITAR = '<img src="' + siteRoot + 'Content/Images/btn-edit.png" alt="Editar Envío" width="19" height="19" style="">';
var IMG_VER = '<img src="' + siteRoot + 'Content/Images/btn-open.png" alt="Ver Envío" width="19" height="19" style="">';
var IMG_ELIMINAR = '<img src="' + siteRoot + 'Content/Images/btn-cancel.png" alt="Eliminar" width="19" height="19" style="">';
var IMG_HISTORIAL = '<img src="' + siteRoot + 'Content/Images/btn-properties.png" alt="Ver Historial de cambios" width="19" height="19" style="">';

$(function () {
    $('#tab-container').easytabs({
        animate: false
    });
    $('#tab-container').easytabs('select', '#tabLista');

    mostrarListadoPlantilla();
});

function mostrarListadoPlantilla() {
    //limpiarBarraMensaje("mensaje");

    $.ajax({
        type: 'POST',
        url: controlador + "ListarPlantillas",
        success: function (evt) {
            if (evt.Resultado != "-1") {

                var htmlPlantillas = dibujarTablaPlantillas(evt.ListadoPlantillas, evt.AccionGrabar);
                $("#div_listado").html(htmlPlantillas);

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

function dibujarTablaPlantillas(listaPlantillas, esAdmin) {
    var num = 0;

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono" id="tablaPlantillas" >
       <thead>
           <tr style="height: 22px;">
               <th style='width:90px;'>Acciones</th>
               <th>Nombre de Plantilla</th>
               <th>Usuario Modificación</th>
               <th>Fecha de Modificación</th>        
            </tr>
        </thead>
        <tbody>
    `;

    for (key in listaPlantillas) {
        var item = listaPlantillas[key];
        num++;

        var htmlTdAccion = "";
        if (esAdmin) {
            htmlTdAccion = `
                    <a href="JavaScript:mostrarDetalle(${item.Inpstcodi}, '${item.Inpstnombre}', ${VER});">${IMG_VER}</a>
                    <a href="JavaScript:mostrarVersiones(${item.Inpsttipo}, ${HISTORIAL});">${IMG_HISTORIAL}</a>
                    <a href="JavaScript:mostrarDetalle(${item.Inpstcodi}, '${item.Inpstnombre}', ${EDITAR});">${IMG_EDITAR}</a>
            `;
        } else {
            htmlTdAccion = `
                    <a href="JavaScript:mostrarDetalle(${item.Inpstcodi}, '${item.Inpstnombre}', ${VER});">${IMG_VER}</a>
            `;
        }

        cadena += `
            <tr>
                <td style='width:90px;'>   
                    ${htmlTdAccion}
                </td>
                <td style="text-align:left;">${item.Inpstnombre}</td>
                <td>${item.Inpstusumodificacion}</td>
                <td>${item.InpstfecmodificacionDesc}</td>
           </tr >           
        `;
    }


    cadena += `                   
        </tbody >
    </table >
    `;

    return cadena;
}

function mostrarDetalle(inpstcodi, inpstnombre, accion) {
    $("#div_detalle").html('');
    limpiarBarraMensaje("mensaje");
    $('#tab-container').easytabs('select', '#tabDetalle');

    $.ajax({
        type: 'POST',
        url: controlador + "RequisitosListado",
        data: {
            inpstcodi: inpstcodi
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {

                cerrarPopup("historialPlantilla");



                if (accion == EDITAR)
                    $("#btnNuevoRequisito").show();
                else
                    $("#btnNuevoRequisito").hide();

                $("#titulo").html(inpstnombre);

                LISTADO_REQUISITOS = evt.PlantillaSustento.Requisitos;
                var htmlRequisitos = dibujarTablaRequisitos(inpstcodi, evt.PlantillaSustento.Requisitos, accion);
                $("#div_detalle").html(htmlRequisitos);

                $('#tablaRequisitos').dataTable({
                    "scrollY": 480,
                    "scrollX": false,
                    "sDom": 'ft',
                    "iDisplayLength": -1
                });

                if (accion == EDITAR) {
                    var oTable = $('#tablaRequisitos').dataTable();
                    $("#tablaRequisitos .ui-sortable").sortable("enable");
                    oTable.rowReordering({ sURL: controlador + "UpdateOrder?codigo=" + inpstcodi});
                }
                else {
                    $("#tablaRequisitos .ui-sortable").sortable("disable");
                }

                $("#btnNuevoRequisito").unbind();
                $("#btnGuardarRequisito").unbind();

                $('#btnNuevoRequisito').click(function (event) {
                    abrirPopupFormulario(inpstcodi, 0, 0);
                });

                $('#btnGuardarRequisito').click(function (event) {
                    guardarRequisito(inpstcodi, NUEVO);
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

function dibujarTablaRequisitos(inpstcodi, listaRquisitos, accion) {
    var num = 0;

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono" id="tablaRequisitos" >
       <thead>
           <tr style="height: 22px;">
               <th style='width:90px;'>Orden</th>
               <th style='width:90px;'>Acciones</th>
               <th>Requisitos</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in listaRquisitos) {
        var item = listaRquisitos[key];
        num++;

        var htmlTdAccion = "";
        if (accion == EDITAR) {
            htmlTdAccion = `
                    <a href="JavaScript:abrirPopupFormulario(${item.Inpstcodi}, ${item.Inpsticodi}, ${EDITAR});">${IMG_EDITAR}</a>
                    <a href="JavaScript:eliminarRequisito(${item.Inpsticodi}, ${item.Inpstcodi}, ${ELIMINAR});">${IMG_ELIMINAR}</a>
            `;
        }
        else {
            htmlTdAccion = `
                    <a href="JavaScript:abrirPopupFormulario(${item.Inpstcodi}, ${item.Inpsticodi}, ${VER});">${IMG_VER}</a>
            `;
        }

        cadena += `
            <tr id="${item.Inpstiorden}">
                <td>${item.Inpstiorden}</td>
                <td style='width:90px;'>   
                    ${htmlTdAccion}
                </td>
                <td style="text-align:left;">${item.Inpstidesc}</td>
           </tr >           
        `;
    }


    cadena += `                   
        </tbody >
    </table >
    `;

    return cadena;
}

function mostrarVersiones(inpsttipo, accion) {
    limpiarBarraMensaje("mensaje");
    $('#listadoPlantilla').html('');

    $.ajax({
        type: 'POST',
        url: controlador + "HistorialListado",
        data: {
            inpsttipo: inpsttipo
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {

                var htmlHistorial = dibujarHistorial(evt.ListadoPlantillas, evt.AccionGrabar);
                $("#listadoPlantilla").html(htmlHistorial);
                $("#listadoPlantilla").css("width", (820) + "px");

                abrirPopup("historialPlantilla");
            } else {

                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error al listar el historial :' + evt.Mensaje);
            }
        },
        error: function (err) {

            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function dibujarHistorial(historial, esAdmin) {
    var num = 0;
    var colorActivo = "";

    var cadena = '';
    cadena += `
    <table border='0' class='pretty tabla-icono' cellspacing='0' width='100%' id='tablaHistorial'>
       <thead>
           <tr>
               <th style='width:80px;'>Acciones</th>
               <th style='width: 100px;'>Estado</th>
               <th style='width: 300px;'>Nombre de plantilla</th>
               <th style='width: 180px'>Fecha modificación</th>
               <th style='width: 180px'>Usuario modificación</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in historial) {
        var item = historial[key];
        num++;

        var htmlTdAccion = "";
        htmlTdAccion = `
                    <a href="JavaScript:mostrarDetalle(${item.Inpstcodi},'${item.Inpstnombre}', ${VER});">${IMG_VER}</a>
            `;

        if (item.Inpstestado == "A")
            colorActivo = "#C6E0B4";
        else
            colorActivo = "";

        cadena += `
            <tr>
                <td style='width:90px; background-color:${colorActivo}'>
                    ${htmlTdAccion}
                </td>
                <td style='background-color:${colorActivo}'>${item.InpstestadoDesc}</td>
                <td style='background-color:${colorActivo}'>${item.Inpstnombre}</td>
                <td style='background-color:${colorActivo}'>${item.InpstfecmodificacionDesc}</td>
                <td style='background-color:${colorActivo}'>${item.Inpstusumodificacion}</td>
           </tr >           
        `;
    }


    cadena += `                   
        </tbody >
    </table >
    `;

    return cadena;
}

function abrirPopupFormulario(codPlantilla, codigoReq, accion) {
    //$('#popupRequisito').html('');

    if (codigoReq == 0) {
        REQUISITO_WEB = {};
        REQUISITO_WEB.Inpstcodi = codPlantilla;
        REQUISITO_WEB.Inpsticodi = 0;
        REQUISITO_WEB.Inpstiorden = 0;
        REQUISITO_WEB.Inpstidesc = "";
        REQUISITO_WEB.Inpstitipo = "";
        $("#txtRequisito").val("");
        $("#cbTipoResp").val(1);// texto por defecto
    }
    else {
        REQUISITO_WEB = LISTADO_REQUISITOS.find((element) => element.Inpsticodi == codigoReq);
        $("#txtRequisito").val(REQUISITO_WEB.Inpstidesc);
        $("#cbTipoResp").val(REQUISITO_WEB.Inpstitipo);
    }

    if (accion == VER) {
        document.getElementById("txtRequisito").disabled = true;
        document.getElementById("cbTipoResp").disabled = true;
        $("#btnGuardarRequisito").hide();
    }
    else {
        document.getElementById("txtRequisito").disabled = false;
        document.getElementById("cbTipoResp").disabled = false;
        $("#btnGuardarRequisito").show();
    }

    abrirPopup("popupRequisito");
}

function guardarRequisito(codPlantilla, accion) {
    limpiarBarraMensaje("mensaje");

    //actualizar
    REQUISITO_WEB.Inpsticodi = REQUISITO_WEB.Inpsticodi || 0;
    REQUISITO_WEB.Inpstiorden = REQUISITO_WEB.Inpstiorden || 0;
    REQUISITO_WEB.Inpstidesc = $("#txtRequisito").val().trim();
    REQUISITO_WEB.Inpstitipo = $("#cbTipoResp").val();

    var msg = validarRequisito(REQUISITO_WEB);

    if (msg == "") {

        var dataJson = JSON.stringify(REQUISITO_WEB).replace(/\/Date/g, "\\\/Date").replace(/\)\//g, "\)\\\/");
        $.ajax({
            type: 'POST',
            dataType: 'json',
            traditional: true,
            url: controlador + "GuardarRequisitoPlantilla",
            data: {
                dataJson: dataJson,
                codPlantilla: codPlantilla
            },
            success: function (result) {
                if (result.Resultado == "-1") {
                    alert(result.Mensaje);
                } else {
                    //alert("El registro se ha guardado correctamente.");
                    cerrarPopup("popupRequisito");
                    $("#vistaDetalle").html('');//limpiar tab Detalle
                    $('#tab-container').easytabs('select', '#tabLista');
                    mostrarListadoPlantilla();
                    mostrarMensaje('mensaje', 'exito', 'El registro se ha guardado correctamente.');
                }
            },
            error: function (err) {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.' + result.Mensaje);
            }
        });
    } else {
        alert('Error:' + msg);
    }
}

function eliminarRequisito(inpsticodi, inpstcodi) {

    var msgConfirmacion = '¿Esta seguro que desea eliminar el requisito?';

    if (confirm(msgConfirmacion)) {
        $.ajax({
            url: controlador + "EliminarRequisito",
            data: {
                inpsticodi: inpsticodi,
                inpstcodi: inpstcodi
            },
            type: 'POST',
            success: function (result) {

                if (result.Resultado === "-1") {
                    mostrarMensaje('mensaje', 'error', 'Se ha producido un error al eliminar el requisito: ' + result.Mensaje);
                } else {
                    $("#vistaDetalle").html('');//limpiar tab Detalle
                    $('#tab-container').easytabs('select', '#tabLista');
                    mostrarListadoPlantilla();
                    mostrarMensaje('mensaje', 'exito', 'Se ha eliminado el requisito con éxito.');
                }
            },
            error: function (xhr, status) {
                mostrarMensaje_('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}

function validarRequisito(obj) {
    var msj = "";

    var tamanioReq = obj.Inpstidesc.length;
    if (tamanioReq > 300) {
        msj += "Se ha superado el número máximo de caracteres.";
    }

    if (tamanioReq == 0) {
        msj += "Debe ingresar un requisito.";
    }

    if ($("#cbTipoResp").val() == null) {
        msj += "Debe seleccionar el tipo de respuesta." + "\n";
    }

    return msj;
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