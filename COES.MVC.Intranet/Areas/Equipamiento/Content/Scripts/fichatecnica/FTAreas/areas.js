var controlador = siteRoot + 'Equipamiento/FTAreas/';

const NUEVO = 1;
const EDITAR = 2;
const VER = 3;
const IMG_EDITAR = '<img src="' + siteRoot + 'Content/Images/btn-edit.png" alt="Editar Área" width="19" height="19" title="Editar" style="">';
const IMG_VER = '<img src="' + siteRoot + 'Content/Images/btn-open.png" alt="Ver Área" width="19" height="19" title="Ver" style="">';
const IMG_ELIMINAR = '<img src="' + siteRoot + 'Content/Images/btn-cancel.png" alt="Eliminar Área" width="19" title="Eliminar" height="19" style="">';
const IMG_ACTIVAR = '<img src="' + siteRoot + 'Content/Images/btn-ok.png" alt="Activar Área" width="19" height="19" title="Activar área" style="">';

const COLOR_BAJA = "#FFDDDD";
const REGEX_EMAIL = "([a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@" + "(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)";

var LISTA_CORREO_BD = [];
var ES_INICIALIZADO = false;

$(function () {

    $('#btnNuevo').click(function () {
        $("#txtArea").css("background", "white");
        limpiarPopupAreaCorreos();
        $("#hdAccion").val(NUEVO);
        //accion = NUEVO;
        abrirPopup("popupEdicion");
    });

    $('#btnGrabarIngreso').click(function () {
        guardarAreaCorreo();
    });    
    
    $('#btnExportar').click(function () {
        exportarListado();
    });

    $('#btnCorreosAdminFT').click(function () {
        mostrarVentanaCorreosAdmin();
    });

    $('#btnGuardarCFT').click(function () {
        guardarAreaCorreoAdminFT();
    });

    mostrarListadoAreas();
});

function inicializarPlugin() {
    var itemss = LISTA_CORREO_BD.map(function (x) { return { email: x.Correo, name: x.Nombre, imageUrl: x.Imagen }; });

    $("#txtEmail2").selectize({
        plugins: ["remove_button"],
        persist: false,
        maxItems: null,
        valueField: "email",
        labelField: "name",
        searchField: ["name", "email"],
        options: itemss,
        render: {
            item: function (item, escape) {
                return (
                    "<div>" +
                    (item.email
                        ? '<span class="email">' + escape(item.email) + "</span>"
                        : "") +
                    "</div>"
                );
            },
            option: function (item, escape) {
                var label = item.name || item.email;
                var caption = item.name ? item.email : null;
                return (
                    "<div>" +
                    '<div class="image">' +
                    '<img class="avatar" src="' + item.imageUrl + '" />' +
                    '</div>' +
                    '<span class="label">' +
                    escape(label) +
                    "</span>" +
                    (caption
                        ? '<span class="caption">' + escape(caption) + "</span>"
                        : "") +
                    "</div>"
                );
            },
        },
        createFilter: function (input) {
            var match, regex;

            // email@address.com
            regex = /^[a-z0-9._-]+@[a-z0-9._-]+\.[a-z]{2,6}$/;
            match = input.match(regex);
            if (match) return !this.options.hasOwnProperty(match[0]);

            // name <email@address.com>
            //regex = new RegExp("^([^<]*)<" + REGEX_EMAIL + ">$", "i");
            //match = input.match(regex);
            //if (match) return !this.options.hasOwnProperty(match[2]);

            return false;
        },
        create: function (input) {
            if (/^[a-z0-9._-]+@[a-z0-9._-]+\.[a-z]{2,6}$/.test(input)) {
                return { email: input };
            }
            var match = input.match(
                new RegExp("^([^<]*)<" + REGEX_EMAIL + ">$", "i")
            );
            if (match) {
                return {
                    email: match[2],
                    name: $.trim(match[1]),
                };
            }
            alert("dirección de correo inválido.");
            return false;
        },
    });

}

function mostrarListadoAreas() {
    limpiarBarraMensaje("mensaje");

    $.ajax({
        type: 'POST',
        url: controlador + "ListarAreas",
        data: {},
        success: function (evt) {
            if (evt.Resultado != "-1") {
                LISTA_CORREO_BD = evt.ListaCorreos;
                if (!ES_INICIALIZADO) {
                    inicializarPlugin();
                    inicializarPluginAdminFT();
                }                    

                ES_INICIALIZADO = true;

                var htmlArea = dibujarListadoAreaCorreo(evt.ListadoAreas);
                $("#listado").html(htmlArea);

                $('#tablaAreas').dataTable({
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

function dibujarListadoAreaCorreo(listaAreas) {

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono" id="tablaAreas" >
       <thead>
           <tr style="height: 22px;">
               <th style='width:90px;'>Acciones</th>
               <th>Nombre Área COES</th>
               <th>Usuario Creación</th>
               <th>Fecha de Creación</th>
               <th>Usuario Modificación</th>
               <th>Fecha de Modificación</th>
               <th>Correos asociados</th>
               <th>Estado</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (var key in listaAreas) {
        var item = listaAreas[key];
        var colorFila = "";

        if (item.Faremestado == "X")
            colorFila = COLOR_BAJA;

        cadena += `
            <tr>
                <td style='width:90px; background: ${colorFila}'>
                    <a href="JavaScript:mostrarAreaCorreo(${item.Faremcodi},${VER} );">${IMG_VER}</a>
        `;
        if (item.Faremestado == "X") {
            cadena += `
                    <a href="JavaScript:activarArea(${item.Faremcodi});">${IMG_ACTIVAR}</a>
            `;
        } else {
            cadena += `
                    <a href="JavaScript:mostrarAreaCorreo(${item.Faremcodi},${EDITAR});">${IMG_EDITAR}</a>
                    <a href="JavaScript:eliminarArea(${item.Faremcodi});">${IMG_ELIMINAR}</a>
            `;
        }
        cadena += `                    
                </td>
                <td style="background: ${colorFila}">${item.Faremnombre}</td>
                <td style="background: ${colorFila}">${item.Faremusucreacion}</td>
                <td style="background: ${colorFila}">${item.FechaCreacionDesc}</td>
                <td style="background: ${colorFila}">${item.Faremusumodificacion}</td>
                <td style="background: ${colorFila}">${item.FechaModificacionDesc}</td>
                <td style="background: ${colorFila}">${item.CantidadCorreos}</td>
                <td style="background: ${colorFila}">${item.FaremEstadoDesc}</td>
           </tr >           
        `;
    }

    cadena += `                   
        </tbody >
    </table >
    `;

    return cadena;
}

function limpiarPopupAreaCorreos() {
    limpiarBarraMensaje("mensaje_popupCorreo");
    $("#txtArea").val("");

    $('#txtEmail2')[0].selectize.setValue("");

    document.getElementById("txtArea").disabled = false;
    $('#txtEmail2')[0].selectize.enable();
    $("#bloqueBotones").css("display", "block");
    $("#tituloPopup").html("<span>Nueva Área</span>");
}

function mostrarAreaCorreo(faremcodi, accion) {
    $.ajax({
        type: 'POST',
        url: controlador + "ObtenerArea",
        data: { faremcodi: faremcodi },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                prepararDetallesPopup(evt.Areas, accion);
                setearInfoProyecto(evt, accion);

                abrirPopup("popupEdicion");

            } else {
                mostrarMensaje('mensaje', 'error', evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function prepararDetallesPopup(objArea, accion) {
    limpiarBarraMensaje("mensaje");
    limpiarBarraMensaje("mensaje_popupCorreo");


    if (accion == VER) {
        $("#txtArea").css("background", "#F2F4F3");        
        $("#tituloPopup").html("<span>Detalle Área de Correos</span>");

        $('#txtArea').val(objArea.Faremnombre);
        document.getElementById("txtArea").disabled = true;
        $('#txtEmail2')[0].selectize.disable();

        $("#bloqueBotones").css("display", "none");
    } else {
        if (accion == EDITAR) {
            $("#txtArea").css("background", "white");
            $("#tituloPopup").html("<span>Edición Área de Correos</span>");

            $('#txtArea').val(objArea.Faremnombre);

            document.getElementById("txtArea").disabled = false;
            $('#txtEmail2')[0].selectize.enable();
            $("#bloqueBotones").css("display", "block");
        }
    }
}

function setearInfoProyecto(objArea, accion) {

    let lstCorreos = objArea.ListaCorreosPorArea;
    var datos = objArea.ListaCorreos;

    for (var i = 0; i < datos.length; i++) {
        $('#txtEmail2')[0].selectize.addOption({ email: datos[i].Correo, name: datos[i].Nombre, imageUrl: datos[i].Imagen});
    }

    //$('#txtEmail2')[0].selectize.refreshOptions();
    $('#txtEmail2')[0].selectize.setValue(lstCorreos);
    $("#hdAccion").val(accion);
    $("#hdIdArea").val(objArea.Areas.Faremcodi);
}

function eliminarArea(faremcodi) {
    limpiarBarraMensaje("mensaje");
    if (confirm('¿Desea eliminar el grupo?')) {

        $.ajax({
            type: 'POST',
            url: controlador + 'EliminarArea',
            data: {
                faremcodi: faremcodi
            },
            dataType: 'json',
            success: function (evt) {
                if (evt.Resultado == "1") {
                    mostrarListadoAreas();
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

function verRoles() {
    var accion = $("#hdAccion").val();

    if (accion == EDITAR || accion == NUEVO) {
        limpiarBarraMensaje("mensaje_popupCorreo");
        $("#correosPT").html("");
        $("#correosNSC").html("");

        var data = getDataGuardar();
        var msg = validarCamposVerRol(data);

        if (msg == "") {

            $.ajax({
                type: 'POST',
                url: controlador + 'VerRolesUsuariosArea',
                dataType: 'json',
                contentType: "application/json",
                data: JSON.stringify({
                    correos: data.ListaCorreos
                }),
                success: function (evt) {
                    if (evt.Resultado != "-1") {

                        $("#correosPT").html(evt.correosRolPermisoTotal);
                        $("#correosNSC").html(evt.correosRolSoloNoConfidenciales);

                        abrirPopup("popupDescRolesAreas");
                        

                    } else {
                        mostrarMensaje('mensaje_popupCorreo', 'error', evt.Mensaje);
                    }
                },
                error: function (err) {
                    mostrarMensaje('mensaje_popupCorreo', 'error', 'Ha ocurrido un error.');
                }
            });
        } else {
            mostrarMensaje('mensaje_popupCorreo', 'alert', msg);
        }
    } else
        mostrarMensaje('mensaje_popupCorreo', 'alert', "No tiene permiso para realizar cambios.");
}


function validarCamposVerRol(data) {
    var msj = "";

    

    if ($('#txtEmail2')[0].selectize.items.length <= 0) {
        msj += "<p>Debe ingresar mínimamente un correo asociado.</p>";
    }

    var listaCorreos = data.ListaCorreos;
    var flag = false;
    for (key in listaCorreos) {
        if (!flag) {
            var correo = listaCorreos[key];
            let esCorreoIntranet = correo.includes("@coes.org.pe");

            if (!esCorreoIntranet) {
                msj += "<p>Debe ingresar solo correos asociados a COES, es decir, que terminen en '@coes.org.pe'.</p>";
                flag = true;
            }
        }
    }


    return msj;
}

function guardarAreaCorreo() {
    var accion = $("#hdAccion").val();

    if (accion == EDITAR || accion == NUEVO) {
        limpiarBarraMensaje("mensaje_popupCorreo");

        var data = getDataGuardar();
        var msg = validarCampos(data);

        if (msg == "") {

            $.ajax({
                type: 'POST',
                url: controlador + 'GuardarAreaCorreos',
                dataType: 'json',
                contentType: "application/json",
                data: JSON.stringify({
                    accion: accion,
                    area: data.Area,
                    correos: data.ListaCorreos,
                    faremcodi: data.Faremcodi
                }),
                success: function (evt) {
                    if (evt.Resultado != "-1") {

                        mostrarListadoAreas();
                        mostrarMensaje('mensaje', 'exito', "La operacion se realizó satisfactoriamente.");
                        cerrarPopup("popupEdicion");

                    } else {
                        mostrarMensaje('mensaje_popupCorreo', 'error', evt.Mensaje);
                    }
                },
                error: function (err) {
                    mostrarMensaje('mensaje_popupCorreo', 'error', 'Ha ocurrido un error.');
                }
            });
        } else {
            mostrarMensaje('mensaje_popupCorreo', 'alert', msg);
        }
    } else
        mostrarMensaje('mensaje_popupCorreo', 'alert', "No tiene permiso para realizar cambios.");
}

function getDataGuardar() {
    var obj = {};

    obj.Area = $("#txtArea").val();
    obj.ListaCorreos = $('#txtEmail2')[0].selectize.items
    obj.Faremcodi = parseInt($("#hdIdArea").val()) || 0;

    return obj;
}

function validarCampos(data) {
    var msj = "";

    if (data.Area.trim() == "") {
        msj += "<p>Debe ingresar el 'Nombre Área COES'.</p>";
    } else {
        if (data.Area.trim().length > 200) {
            msj += "<p>El campo 'Nombre Área COES' no debe exceder los 200 caracteres.</p>";
        }
    }

    if ($('#txtEmail2')[0].selectize.items.length <= 0) {
        msj += "<p>Debe ingresar mínimamente un correo asociado.</p>";
    }

    var listaCorreos = data.ListaCorreos;
    var flag = false;
    for (key in listaCorreos) {
        if (!flag) {
            var correo = listaCorreos[key];
            let esCorreoIntranet = correo.includes("@coes.org.pe");

            if (!esCorreoIntranet) {
                msj += "<p>Debe ingresar solo correos asociados a COES, es decir, que terminen en '@coes.org.pe'.</p>";
                flag = true;
            }
        }
    }


    return msj;
}

function exportarListado() {
    limpiarBarraMensaje("mensaje");

    $.ajax({
        type: 'POST',
        url: controlador + 'ExportarArea',
        data: {},
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

function activarArea(faremcodi) {
    limpiarBarraMensaje("mensaje");
    if (confirm('¿Desea activar el registro?')) {

        $.ajax({
            type: 'POST',
            url: controlador + 'ActivarArea',
            data: {
                faremcodi: faremcodi
            },
            dataType: 'json',
            success: function (evt) {
                if (evt.Resultado == "1") {
                    mostrarListadoAreas();
                    mostrarMensaje('mensaje', 'exito', 'Se activó el registro exitosamente.');
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


/********* CORREOS ADMIN FT ***********/
function mostrarVentanaCorreosAdmin() {
    limpiarBarraMensaje("mensaje"); 
    limpiarBarraMensaje("mensaje_popupAdminFT");

    $.ajax({
        type: 'POST',
        url: controlador + "ObtenerArea",
        data: { faremcodi: 1 },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                $("#usuarioCFT").html(evt.Areas.Faremusucreacion);
                $("#fechaCFT").html(evt.Areas.FechaCreacionDesc);
                $("#usuarioMFT").html(evt.Areas.Faremusumodificacion);
                $("#fechaMFT").html(evt.Areas.FechaModificacionDesc);
                $("#hdExisteCorreoAdmin").val(evt.ExisteCorreosAdminFT);
                

                setearInfoProyectoFT(evt);

                abrirPopup("popupCorreosAdminFT");

            } else {
                mostrarMensaje('mensaje', 'error', evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });

     
}


function setearInfoProyectoFT(objArea) {

    let lstCorreos = objArea.ListaCorreosPorArea;
    var datos = objArea.ListaCorreos;

    for (var i = 0; i < datos.length; i++) {
        $('#txtEmailFT')[0].selectize.addOption({ email: datos[i].Correo, name: datos[i].Nombre, imageUrl: datos[i].Imagen });
    }

    $('#txtEmailFT')[0].selectize.setValue(lstCorreos);

}

function inicializarPluginAdminFT() {
    var itemss = LISTA_CORREO_BD.map(function (x) { return { email: x.Correo, name: x.Nombre, imageUrl: x.Imagen }; });

    $("#txtEmailFT").selectize({
        plugins: ["remove_button"],
        persist: false,
        maxItems: null,
        valueField: "email",
        labelField: "name",
        searchField: ["name", "email"],
        options: itemss,
        render: {
            item: function (item, escape) {
                return (
                    "<div>" +
                    (item.email
                        ? '<span class="email">' + escape(item.email) + "</span>"
                        : "") +
                    "</div>"
                );
            },
            option: function (item, escape) {
                var label = item.name || item.email;
                var caption = item.name ? item.email : null;
                return (
                    "<div>" +
                    '<div class="image">' +
                    '<img class="avatar" src="' + item.imageUrl + '" />' +
                    '</div>' +
                    '<span class="label">' +
                    escape(label) +
                    "</span>" +
                    (caption
                        ? '<span class="caption">' + escape(caption) + "</span>"
                        : "") +
                    "</div>"
                );
            },
        },
        createFilter: function (input) {
            var match, regex;
            regex = /^[a-z0-9._-]+@[a-z0-9._-]+\.[a-z]{2,6}$/;
            match = input.match(regex);
            if (match) return !this.options.hasOwnProperty(match[0]);

            return false;
        },
        create: function (input) {
            if (/^[a-z0-9._-]+@[a-z0-9._-]+\.[a-z]{2,6}$/.test(input)) {
                return { email: input };
            }
            var match = input.match(
                new RegExp("^([^<]*)<" + REGEX_EMAIL + ">$", "i")
            );
            if (match) {
                return {
                    email: match[2],
                    name: $.trim(match[1]),
                };
            }
            alert("dirección de correo inválido.");
            return false;
        },
    });

}


function guardarAreaCorreoAdminFT() {
    var tienePermiso = $("#hdPermisoAdministracion").val();

    if (tienePermiso) {
        limpiarBarraMensaje("mensaje_popupAdminFT");

        var data = getDataGuardarFT();
        var msg = validarCamposFT(data);

        if (msg == "") {

            $.ajax({
                type: 'POST',
                url: controlador + 'GuardarAreaCorreos',
                dataType: 'json',
                contentType: "application/json",
                data: JSON.stringify({
                    accion: EDITAR,
                    area: data.Area,
                    correos: data.ListaCorreos,
                    faremcodi: data.Faremcodi
                }),
                success: function (evt) {
                    if (evt.Resultado != "-1") {

                        mostrarListadoAreas();
                        mostrarMensaje('mensaje', 'exito', "Se actualizó los correos de la administración de ficha técnica de forma correcta.");
                        cerrarPopup("popupCorreosAdminFT");

                    } else {
                        mostrarMensaje('mensaje_popupAdminFT', 'error', evt.Mensaje);
                    }
                },
                error: function (err) {
                    mostrarMensaje('mensaje_popupAdminFT', 'error', 'Ha ocurrido un error.');
                }
            });
        } else {
            mostrarMensaje('mensaje_popupAdminFT', 'alert', msg);
        }
    } else
        mostrarMensaje('mensaje_popupAdminFT', 'alert', "No tiene permiso para editar los correos de la administración de ficha técnica.");
}

function getDataGuardarFT() {
    var obj = {};

    obj.Area = "Administración F.T.";
    obj.ListaCorreos = $('#txtEmailFT')[0].selectize.items
    obj.Faremcodi = 1;

    return obj;
}

function validarCamposFT(data) {
    var msj = "";
    

    if ($('#txtEmailFT')[0].selectize.items.length <= 0) {
        msj += "<p>Debe ingresar mínimamente un correo asociado a la administración de ficha técnica.</p>";
    }

    var listaCorreos = data.ListaCorreos;
    var flag = false;
    for (key in listaCorreos) {
        if (!flag) {
            var correo = listaCorreos[key];
            let esCorreoIntranet = correo.includes("@coes.org.pe");

            if (!esCorreoIntranet) {
                msj += "<p>Debe ingresar solo correos asociados a COES, es decir, que terminen en '@coes.org.pe'.</p>";
                flag = true;
            }
        }
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