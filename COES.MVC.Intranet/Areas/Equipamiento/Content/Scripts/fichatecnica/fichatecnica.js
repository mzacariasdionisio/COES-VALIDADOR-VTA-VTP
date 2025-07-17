var controlador = siteRoot + 'Equipamiento/fichatecnica/';
var ancho = 900;
var TIPO_SUBESTACION = -2;

var IMG_OCULTO = `<img src="${siteRoot}Content/Images/ContextMenu/menuhidden.png" title="Oculto para el Portal"/>`;
var IMG_OCULTO_EXTRANET = `<img src="${siteRoot}Content/Images/ContextMenu/menuhidden.png" title="Oculto para Extranet"/>`;

$(function () {
    $('#cbFichaMaestra').change(function () {
        listarFichaTecnica();
    });

    $('#cbFichaTecnica').change(function () {
        listarEmpresas();
    });

    $('#cbEmpresa').change(function () {
        buscarElementos();
    });

    $('#btnConsultar').on('click', function () {
        buscarElementos();
    });

    $('#btnExportarMasivo').on('click', function () {
        exportarMasivoFichaTecnica();
    });
    $('#btnExportarReporte').click(function () {
        setTimeout(function () {
            $("#popupExportarReporte").bPopup({
                easing: 'easeOutBack',
                speed: 450,
                transition: 'slideDown',
                modalClose: false
            });
        }, 50);
    });
    $('#btnCancelarExportacion').click(function () {
        $('#popupExportarReporte').bPopup().close();
    });

    ancho = $('#mainLayout').width() - 30;

    setTimeout(function () { //tiempo usado al presionar botón Regresar, toma valor correcto del cbFichaMaestra
        listarFichaTecnica();
    }, 1000);

    
});

///////////////////////////
/// Lista de Ficha Tecnica
///////////////////////////

function listarFichaTecnica() {
    $("#paginado").html('');
    $("#listado").html('');

    $('#cbFichaTecnica').empty();
    $('#cbFichaTecnica').append('<option value="0">--SELECCIONE--</option>');

    $('#cbEmpresa').empty();
    $('#cbEmpresa').append('<option value="-2">--TODOS--</option>');
    var idFTmaestra = parseInt($("#cbFichaMaestra").val()) || 0;

    if (idFTmaestra > 0) {
        $.ajax({
            type: 'POST',
            url: controlador + 'ListaFichaTecnicaXMaestra',
            dataType: 'json',
            async: true,
            data: {
                idFTmaestra: idFTmaestra
            },
            success: function (result) {
                if (result.Resultado == "1") {
                    var listaFT = result.ListaFichaTecnicaSelec;
                    for (var i = 0; i < listaFT.length; i++) {
                        $('#cbFichaTecnica').append('<option value=' + listaFT[i].Fteqcodi + '>' + listaFT[i].Fteqnombre + '</option>');
                    }
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


///////////////////////////
/// Lista de Empresas
///////////////////////////

function listarEmpresas() {
    $("#paginado").html('');
    $("#listado").html('');

    $('#cbEmpresa').empty();
    $('#cbEmpresa').append('<option value="-2">--TODOS--</option>');
    var idFT = parseInt($("#cbFichaTecnica").val()) || 0;

    if (idFT > 0) {
        $.ajax({
            type: 'POST',
            url: controlador + 'ListaEmpresaXFichaTecnica',
            dataType: 'json',
            async: true,
            data: {
                idFT: idFT
            },
            success: function (result) {
                if (result.Resultado == "1") {
                    TIPO_SUBESTACION = result.TipoElementoId;

                    if (TIPO_SUBESTACION != 1) {
                        // mostrar combo
                        $(".td_empresa").show();

                        var listaEmp = result.ListaEmpresa;
                        for (var i = 0; i < listaEmp.length; i++) {
                            $('#cbEmpresa').append('<option value=' + listaEmp[i].Emprcodi + '>' + listaEmp[i].Emprnomb + '</option>');
                        }
                    }
                    else {
                        //ocultar combo
                        $(".td_empresa").hide();
                    }

                    buscarElementos();
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

///////////////////////////
/// Lista de Equipos
///////////////////////////

function buscarElementos() {
    $('#textoMensaje').css("display", "none");
    $("#paginado").html('');
    $("#listado").html('');

    var ficha = parseInt($("#cbFichaTecnica").val()) || 0;
    var msj = "";
    if (ficha <= 0) {
        $("#cbFichaTecnica").val('0');
        listarFichaTecnica();
        msj += "Debe seleccionar una Ficha Técnica";
    }

    if (msj == "") {
        pintarBusqueda();
    } else {
        $('#textoMensaje').css("display", "block");
        $('#textoMensaje').removeClass();
        $('#textoMensaje').addClass('action-alert');
        $('#textoMensaje').text(msj);
    }
};

function pintarBusqueda() {

    var strEstado = "A,P,F" // TODOS LOS ESTADOS VALIDOS
    if ($("#cbEstado").val() != "-2") {
        strEstado = $("#cbEstado").val();
    }

    $.ajax({
        type: 'POST',
        url: controlador + "ElementosListado",
        data: {
            idFicha: $("#cbFichaTecnica").val(),
            iEmpresa: $("#cbEmpresa").val(),
            iEstado: strEstado,
            nombre: $("#nombre").val(),
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                $('#listado').css("width", ancho + "px");

                $('#listado').html(evt);
                var html = _dibujarTablaListado(evt);
                $('#listado').html(html);

                $('#tabla').dataTable({
                    "scrollY": 500,
                    scrollCollapse: true,
                    //"scrollX": false,
                    //"sDom": 'ft',
                    "ordering": true,
                    order: [[1, 'asc']],
                    "paging": true,
                    lengthMenu: [
                        [15, 25, 50, 100, 200, -1],
                        [15, 25, 50, 100, 200, 'TODOS'],
                    ],
                });

                eventoContextMenu(evt);
                eventoContextMenuFilaNoActivo(evt);
                //viewEvent();

            } else {
                alert(evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarError();
        }
    });
};

function _dibujarTablaListado(model) {
    var lista = model.ListaElemento;

    var htmlThEmpresa = "";
    if (TIPO_SUBESTACION != 1) {
        htmlThEmpresa = `<th>Empresa</th>`;
    }

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono" id="tabla" cellspacing="0" width="100%" >
        <thead>
            <tr>
            <th>Código</th>
            <th>Nombre </th>
            <th>Abreviatura</th>
            ${htmlThEmpresa}
            <th>Ubicación</th>
            <th>Estado</th>
            <th>oculto Extranet</th>
            <th>oculto Portal web</th>
            <th>Ver Detalle</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in lista) {
        var item = lista[key];

        var claseFila = "";
        var htmlTdEmpresa = "";
        if (TIPO_SUBESTACION != 1) {
            htmlTdEmpresa = `<td class="ft_item">${item.Empresa}</td>`;
        }

        var htmlTdOcultoPortal = `<td class=""><span id="oculto_${item.Codigo} "></span></td>`;

        if (item.Estado == "Activo") {
            if (item.Ftveroculto == "S") {
                htmlTdOcultoPortal = `<td class=""><span id="oculto_${item.Codigo}"> ${IMG_OCULTO}</span></td>`;
            }
            claseFila = "context-menu-fila-activo";
        }
        else {
            htmlTdOcultoPortal = `<td class=""><span id="oculto_${item.Codigo} ">Oculto</span></td>`;
            claseFila = "context-menu-extranet";
        }

        var htmlTdOcultoExtranet = `<td class=""><span id="ocultoExt_${item.Codigo} "></span></td>`;
        if (item.FtverocultoExtranet == "S") {
            htmlTdOcultoExtranet = `<td class=""><span id="ocultoExt_${item.Codigo}"> ${IMG_OCULTO_EXTRANET}</span></td>`;
        }

        //var tdOpciones = _tdAcciones(model, item);
        cadena += `

            <tr id="fila_${item.Codigo}" class=${claseFila}>
                <td class="ft_item" style='text-align: center !important;'>${item.Codigo}</td>
                <td class="ft_item">${item.Nombre}</td>
                <td class="ft_item" style='text-align: center !important;'>${item.Abreviatura}</td>
                ${htmlTdEmpresa}
                <td class="ft_item">${item.Ubicacion}</td>
                <td class="ft_item">${item.Estado}</td>
                ${htmlTdOcultoExtranet}
                ${htmlTdOcultoPortal}
                <td>
                    <a href="javascript:verDetalle('${model.IdFicha}', '${item.Codigo}');">Ver Detalle</a>
                </td>
            </tr>
        `;
    }
    cadena += "</tbody></table>";

    return cadena;
}

function mostrarError() {
    $('#textoMensaje').css("display", "block");
    $('#textoMensaje').removeClass();
    $('#textoMensaje').addClass('action-alert');
    $('#textoMensaje').text("Ha ocurrido un error");
}

///////////////////////////
/// Ver detalle
///////////////////////////
function verDetalle(idFicha, idElemento) {
    window.location.href = controlador + "IndexDetalle?idFicha=" + idFicha + "&idElemento=" + idElemento;
}

///////////////////////////
/// Exportación masiva
///////////////////////////
function exportarMasivoFichaTecnica() {
    var idFT = parseInt($("#cbFichaTecnica").val()) || 0;
    var existe = $("#hfExisteElemento").val();

    var strEstado = "A,P,F" // TODOS LOS ESTADOS VALIDOS
    if ($("#cbEstado").val() != "-2") {
        strEstado = $("#cbEstado").val();
    }

    var opcion = parseInt($('input[name="tipoExportar"]:checked').val()) || 0;

    if (idFT > 0 && opcion > 0) {
        $.ajax({
            type: 'POST',
            url: controlador + 'ExportarMasivoFichaTecnica',
            data: {
                idFT: idFT,
                opcionComentario: opcion,
                iEmpresa: $("#cbEmpresa").val(),
                iEstado: strEstado
            },
            success: function (evt) {
                if (evt.Error == undefined) {
                    if (evt[0] == null || evt[0] == "" || evt[1] == null || evt[1] == "") {
                        alert("No existen elementos a exportar.");
                    } else {
                        window.location.href = controlador + 'DescargarExcel?archivo=' + evt[0] + '&nombre=' + evt[1];
                    }
                }
                else {
                    alert("Error:" + evt.Descripcion);
                }
            },
            error: function (result) {
                alert('Ha ocurrido un error al descargar el archivo excel. ' + result.status + ' - ' + result.statusText + '.');
            }
        });
    } else {
        alert("Debe seleccionar una ficha técnica");
    }
}

function eventoContextMenu(model) {
    var lista = model.ListaElemento;

    var objItems = {};

    objItems = {
        "ocultarPort": { name: "Ocultar [Portal Web]. " },
        "visualizarPort": { name: "Visualizar [Portal Web]. " },
        "ocultarExt": { name: "Ocultar [Extranet]. " },
        "visualizarExt": { name: "Visualizar [Extranet]. " }
    };

    $('#tabla').unbind();
    $('#tabla').contextMenu({
        selector: '.context-menu-fila-activo',
        callback: function (key, options) {

            idElement = $(this).attr("id").split("_")[1];
            var equipo = lista.find((eq) => eq.Codigo == idElement);

            //Actualiza los equipos ocultos
            if (key == "ocultarPort" && equipo.Ftveroculto != "S") {
                modificarVisibilidadEquipoModos(idElement, "S", 1)
            }
            if (key == "visualizarPort" && equipo.Ftveroculto != "N") {
                modificarVisibilidadEquipoModos(idElement, "N", 1)
            }
            if (key == "ocultarExt" && equipo.FtverocultoExtranet != "S") {
                modificarVisibilidadEquipoModos(idElement, "S", 2)
            }
            if (key == "visualizarExt" && equipo.FtverocultoExtranet != "N") {
                modificarVisibilidadEquipoModos(idElement, "N", 2)
            }
        },
        items: objItems
    });

}

function eventoContextMenuFilaNoActivo(model) {
    var lista = model.ListaElemento;
    var objItems = {};

    objItems = {
        "ocultarExt": { name: "Ocultar [Extranet]. " },
        "visualizarExt": { name: "Visualizar [Extranet]. " }
    };

    //$('#tabla').unbind();
    $('#tabla').contextMenu({
        selector: '.context-menu-extranet',
        callback: function (key, options) {

            idElement = $(this).attr("id").split("_")[1];
            var equipo = lista.find((eq) => eq.Codigo == idElement);

            //Actualiza los equipos ocultos
            if (key == "ocultarExt" && equipo.FtverocultoExtranet != "S") {
                modificarVisibilidadEquipoModos(idElement, "S", 2)
            }
            if (key == "visualizarExt" && equipo.FtverocultoExtranet != "N") {
                modificarVisibilidadEquipoModos(idElement, "N", 2)
            }
        },
        items: objItems
    });

}

function modificarVisibilidadEquipoModos(idElement, opcion, tipoOculto) {

    var codFT = parseInt($("#cbFichaTecnica").val()) || 0;

    $.ajax({
        type: 'POST',
        url: controlador + "UpdateVisibilidadEquiposModos",
        data: {
            idElemento: idElement,
            idFT: codFT,
            opcion: opcion,
            tipoOculto: tipoOculto
        },
        success: function (evt) {
            pintarBusqueda();
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}