var controlador = siteRoot + 'Intervenciones/FactorF1F2/';

var HOJA_EJECUTADO = 1;
var HOJA_PROGRAMADO_MENSUAL = 2;

var LISTA_ITEMS_MMAYOR = [];
var POS_ITEM = 0;

$(document).ready(function () {
    $("#btnConsultar").click(function () {
        mostrarListado();
    });
    $('#btnRegresar2').click(function () {
        var fecha = $("#infverfechaperiodo").val();
        window.location.href = siteRoot + "Intervenciones/FactorF1F2/Index?fechaPeriodo=" + fecha;
    });
    $("#btnExportarExcel").click(function () {
        exportarReporte();
    });
    $("#btnGuardarCambio").click(function () {
        actualizarVersionPreliminar();
    });

    $('#cbEmpresaF1F2').change(function () {
        cargarListaUbicacionF1F2();
    });
    $('#cbUbicacionF1F2').change(function () {
        cargarListaEquipoF1F2();
    });
    $('#cbEquipoF1F2').change(function () {
        mostrarListado();
    });

    var esPreliminar = $("#hfVersionFinal").val() == "N";
    if (esPreliminar) $("#div_cambio").show();

    mostrarListado();
});

///////////////////////////
/// Versión preliminar
///////////////////////////
function actualizarVersionPreliminar() {
    var listaMayor = obtenerListaMmayor();

    if (listaMayor.length > 0) {
        if (confirm('¿Desea actualizar el/los registro(s)?')) {
            var form = {
                infvercodi: getVersionF1F2(),
                infmmhoja: getHojaF1F2(),
                listaMmayor: listaMayor
            };

            $.ajax(controlador + "ActualizarVersionPreliminar", {
                method: 'post',
                contentType: 'application/json',
                dataType: 'json',
                data: JSON.stringify(form),
                traditional: true,
                success: function (result) {
                    if (result.Resultado != '-1') {
                        alert("Se guardó correctamente");
                        mostrarListado();
                    } else {
                        alert(result.Mensaje);
                    }
                },
                error: function (err) {
                    alert('Ha ocurrido un error.');
                }
            });
        }
    } else {
        alert("No existen cambios.");
    }
}

function obtenerListaMmayor() {
    var lista = [];

    $(`textarea[id^="deschoja_"]`).each(function (i, obj) {
        var idHtml = $(obj).attr('id');

        var codigo = idHtml.split("_")[1];
        var texto = $(obj).val();

        var obj = {
            Entero1: codigo,
            String1: texto,
        };

        lista.push(obj);
    });

    return lista;
}

///////////////////////////
/// Versión final
///////////////////////////
function actualizarSustento(infmmcodi) {

    var id = infmmcodi;
    var sustento = $("#justificacion").val();

    if (confirm('¿Desea actualizar el registro?')) {
        var form = {
            version: $("#VersionDesc").val(),
            infmmcodi: id,
            sustento: sustento,
            listaArhchivoWeb: LISTA_ARCHIVOS_MMAYOR
        };

        console.log(form);

        $.ajax(controlador + "ActualizarSustento", {
            method: 'post',
            contentType: 'application/json',
            dataType: 'json',
            data: JSON.stringify(form),
            traditional: true,
            success: function (result) {
                if (result.Resultado != '-1') {
                    alert("Se guardó correctamente el registro");
                    $("#bSalir").click();
                    mostrarListado();
                } else {
                    alert(result.Mensaje);
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error.');
            }
        });

    }
}

function asignarEventos() {

    $('.btnAdjuntos').click(function () {

        var infmmcodi = $(this).parent().parent().attr("id").replace("fila-", "");
        var posItem = $(this).parent().parent().attr("posItem")

        mostrarAdjuntos(infmmcodi, posItem);
    });
}

function asignarEventosPopup(infmmcodi) {

    $('#bActualizar').click(function () {
        actualizarSustento(infmmcodi);
    });
}

///////////////////////////
/// Tabla web
///////////////////////////

function mostrarListado() {
    $("#mensaje").hide();

    $.ajax({
        type: 'POST',
        url: controlador + 'ListarMmayorXFiltro',
        dataType: 'json',
        data: {
            infvercodi: getVersionF1F2(),
            infmmhoja: getHojaF1F2(),
            empresa: getEmpresaF1F2(),
            ubicacion: getUbicacionF1F2(),
            equipo: getEquipoF1F2(),
            similitud: getSimilitud()
        },
        cache: false,
        success: function (data) {
            if (data.Resultado != "-1") {

                LISTA_ITEMS_MMAYOR = data.ListInFactorVersionMmayor;
                var esPreliminar = $("#hfVersionFinal").val() == "N";

                var html = "";
                if (esPreliminar)
                    html = dibujarTablaListadoPreliminar(data.ListInFactorVersionMmayor);
                else
                    html = dibujarTablaListadoFinal(data.ListInFactorVersionMmayor);
                $("#listado").html(html);

                $('#tblListado').dataTable({
                    "destroy": "true",
                    "scrollY": 550,
                    "scrollX": true,
                    "sDom": 't',
                    "ordering": false,
                    "bPaginate": false,
                    "iDisplayLength": -1
                });

                //adjuntarArchivo();
                asignarEventos();

            } else {
                alert("Ha ocurrido un error: " + data.StrMensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function dibujarTablaListadoPreliminar(lista) {

    var tHOrigen = "";
    if (getHojaF1F2() == HOJA_EJECUTADO) {
        tHOrigen = `<th style='width: 70px'>Origen</th>`;
    }

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono dataTable" cellspacing="0" width="100%" id="tblListado">
        <thead>
            <th>Fila</th>
            <th>Empresa</th>
            <th>Central</th>
            <th>Unidad</th>
            <th>Descripción</th>
            <th style='width: 70px'>Fila Similar</th>
            <th style='width: 70px'>Fecha Inicio</th>
            <th style='width: 70px'>Fecha Fin</th>
            <th style='width: 70px'>Duración</th>
            <th style='width: 70px'>Tipo de evento</th>
            <th style='width: 70px'>Programa</th>
            ${tHOrigen}
            <th style='width: 70px'>Observaciones<br>Incumplimiento</th>
            <th style='width: 70px'>Usuario<br>Modificación</th>
            <th style='width: 70px'>Fecha<br>Modificación</th>
            </tr>
        </thead>
        <tbody>
    `;

    var posItem = 0;

    for (key in lista) {
        var item = lista[key];

        obsClass = "";
        cadenaArchivos = "";

        if (item.Infmmobse == "NE") {
            obsClass = "background-color:yellow";
        }
        if (item.Infmmobspm == "NP") {
            obsClass = "background-color:#00ff00";
        }
        var tdOrigen = "";
        if (getHojaF1F2() == HOJA_EJECUTADO) {
            tdOrigen = `<td style="height: 24px;">${item.InfmmorigenDesc}</td>`;
        }
        var tdSimiliar = '<td></td>';
        if (item.NroFilaSimilarCon > 0) {
            tdSimiliar = `
                <td style='background-color: #F9AF07;'>${item.NroFilaSimilarCon}</td>
            `;
        }

        cadena += `
            <tr id="fila-${item.Infmmcodi}" version="${$("#mes").val()}" posItem="${posItem}">
                <td style="text-align:center;">${item.NroFila}</td>
                <td style="text-align:left;">${item.Emprnomb}</td>
                <td style="text-align:left;">${item.Areanomb}</td>
                <td style="text-align:center;">${item.Equiabrev}</td>
                <td style="height: 24px;text-align:left;">
                <textarea id="deschoja_${item.Infmmcodi}" style='width: 450px; height: 60px;background-color: white; resize: none;'>${item.Infmmdescrip}</textarea>
                </td>
                ${tdSimiliar}
                <td style="height: 24px;">${item.InfmmfechainiDesc}</td>
                <td style="height: 24px;">${item.InfmmfechafinDesc}</td>
                <td style="height: 24px;text-align:center;">${item.Infmmduracion}</td>
                <td style="height: 24px;text-align:center;">${item.Tipoevendesc}</td>
                <td style="height: 24px;">${item.Clapronombre}</td>
                ${tdOrigen}
                <td style="height: 24px;${obsClass}">${item.ObsIncumpl}</td>
                <td style="height: 24px;">${item.Infmmusumodificacion}</td>
                <td style="height: 24px;">${item.InfmmfecmodificacionDesc}</td>
            </tr>
        `;

        posItem++;
    }
    cadena += "</tbody></table>";

    return cadena;
}

function dibujarTablaListadoFinal(lista) {

    var tHOrigen = "";
    if (getHojaF1F2() == HOJA_EJECUTADO) {
        tHOrigen = `<th style='width: 70px'>Origen</th>`;
    }

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono dataTable" cellspacing="0" width="100%" id="tblListado">
        <thead>
            <th>Fila</th>
            <th>Empresa</th>
            <th>Central</th>
            <th>Unidad</th>
            <th>Descripción</th>
            <th style='width: 70px'>Fila Similar</th>
            <th style='width: 70px'>Fecha Inicio</th>
            <th style='width: 70px'>Fecha Fin</th>
            <th style='width: 70px'>Duración</th>
            <th style='width: 70px'>Tipo de evento</th>
            <th style='width: 70px'>Programa</th>
            ${tHOrigen}
            <th style='width: 70px'>Observaciones<br>Incumplimiento</th>
            <th style='width: 70px'>Sustento</th>
            <th>Justificación</th>
            <th style='width: 70px'>Usuario<br>Modificación</th>
            <th style='width: 70px'>Fecha<br>Modificación</th>
            <th style='width: 70px'>Acción</th>
            </tr>
        </thead>
        <tbody>
    `;

    var cadenaArchivos = "";
    var posItem = 0;
    var posArchivo = 0;

    for (key in lista) {
        var item = lista[key];

        obsClass = "";
        cadenaArchivos = "";

        if (item.Infmmobse == "NE") {
            obsClass = "background-color:yellow";
        }
        if (item.Infmmobspm == "NP") {
            obsClass = "background-color:#00ff00";
        }

        var listaArchivos = item.ListaArchivos;
        posArchivo = 0;
        for (var archivo in listaArchivos) {
            cadenaArchivos += `<a href="javascript:descargarArchivo(${item.Infmmcodi}, ${posItem}, ${posArchivo});" title="${listaArchivos[archivo].Inarchnombreoriginal}"><div class="int-file-name"></div></a><br>`;
            posArchivo++;
        }
        var tdOrigen = "";
        if (getHojaF1F2() == HOJA_EJECUTADO) {
            tdOrigen = `<td style="height: 24px;">${item.InfmmorigenDesc}</td>`;
        }
        var tdSimiliar = '<td></td>';
        if (item.NroFilaSimilarCon > 0) {
            tdSimiliar = `
                <td style='background-color: #F9AF07;'>${item.NroFilaSimilarCon}</td>
            `;
        }

        var tdEditar = '';
        if (item.AccionEditar) tdEditar = `
            <a class="btnAdjuntos" style='cursor: pointer;'><img src="${siteRoot}Content/Images/btn-edit.png" alt="Editar Sustento" title="Editar Sustento"></a>
        `;

        cadena += `
            <tr id="fila-${item.Infmmcodi}" version="${$("#mes").val()}" posItem="${posItem}">
                <td style="text-align:center;">${item.NroFila}</td>
                <td style="text-align:left;">${item.Emprnomb}</td>
                <td style="text-align:left;">${item.Areanomb}</td>
                <td style="text-align:center;">${item.Equiabrev}</td>
                <td style="height: 24px;text-align:left;">${item.Infmmdescrip}</td>
                ${tdSimiliar}
                <td style="height: 24px;">${item.InfmmfechainiDesc}</td>
                <td style="height: 24px;">${item.InfmmfechafinDesc}</td>
                <td style="height: 24px;text-align:center;">${item.Infmmduracion}</td>
                <td style="height: 24px;text-align:center;">${item.Tipoevendesc}</td>
                <td style="height: 24px;">${item.Clapronombre}</td>
                ${tdOrigen}
                <td style="height: 24px;${obsClass}">${item.ObsIncumpl}</td>
                <td style="text-align:center;">${cadenaArchivos}</div>
                <td style="text-align:left;">${item.Infmmjustif}</td>
                <td style="height: 24px;">${item.Infmmusumodificacion}</td>
                <td style="height: 24px;">${item.InfmmfecmodificacionDesc}</td>
                <td style="height: 24px;">${tdEditar}</td>
            </tr>
        `;

        posItem++;
    }
    cadena += "</tbody></table>";

    return cadena;
}

function exportarReporte() {
    $.ajax({
        type: 'POST',
        url: controlador + "GenerarArchivoExcelReporteHojaIndividual",
        data: {
            infvercodi: getVersionF1F2(),
            infmmhoja: getHojaF1F2(),
            empresa: getEmpresaF1F2(),
            ubicacion: getUbicacionF1F2(),
            equipo: getEquipoF1F2()
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                window.location = controlador + "ExportarReporte?nameFile=" + evt.Resultado;
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

///////////////////////////
/// Listar filtros 
///////////////////////////

function cargarListaUbicacionF1F2() {
    $("#div_ubicacion_filtro").html('');

    $.ajax({
        type: 'POST',
        url: controlador + 'ViewCargarFiltros',
        dataType: 'json',
        data: {
            infvercodi: getVersionF1F2(),
            infmmhoja: getHojaF1F2(),
            empresa: getEmpresaF1F2(),
            ubicacion: getUbicacionF1F2(),
        },
        cache: false,
        success: function (data) {
            //ubicacion
            $("#div_ubicacion_filtroF1F2").html('<select id="cbUbicacionF1F2"><option value="-1">--TODOS--</option></select>');

            if (data.ListaUbicacion.length > 0) {
                $.each(data.ListaUbicacion, function (i, item) {
                    $('#cbUbicacionF1F2').get(0).options[$('#cbUbicacionF1F2').get(0).options.length] = new Option(item.Areanomb, item.Areacodi);
                });
            }

            $('#cbUbicacionF1F2').unbind();
            $('#cbUbicacionF1F2').change(function () {
                cargarListaEquipoF1F2();
            });

            //equipo
            $("#div_equipo_filtroF1F2").html('<select id="cbEquipoF1F2"><option value="-1">--TODOS--</option></select>');

            if (data.ListaEquipo.length > 0) {
                $.each(data.ListaEquipo, function (i, item) {
                    $('#cbEquipoF1F2').get(0).options[$('#cbEquipoF1F2').get(0).options.length] = new Option(item.Equiabrev, item.Equicodi);
                });
            }

            $('#cbEquipoF1F2').unbind();
            $('#cbEquipoF1F2').change(function () {
                mostrarListado();
            });

            mostrarListado();
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function cargarListaEquipoF1F2() {
    $("#div_equipo_filtroF1F2").html('');

    $.ajax({
        type: 'POST',
        url: controlador + 'ViewCargarFiltros',
        dataType: 'json',
        data: {
            infvercodi: getVersionF1F2(),
            infmmhoja: getHojaF1F2(),
            empresa: getEmpresaF1F2(),
            ubicacion: getUbicacionF1F2(),
        },
        cache: false,
        success: function (data) {
            //equipo
            $("#div_equipo_filtroF1F2").html('<select id="cbEquipoF1F2"><option value="-1">--TODOS--</option></select>');

            if (data.ListaEquipo.length > 0) {
                $.each(data.ListaEquipo, function (i, item) {
                    $('#cbEquipoF1F2').get(0).options[$('#cbEquipoF1F2').get(0).options.length] = new Option(item.Equiabrev, item.Equicodi);
                });
            }

            $('#cbEquipoF1F2').unbind();
            $('#cbEquipoF1F2').change(function () {
                mostrarListado();
            });

            mostrarListado();
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function getEmpresaF1F2() {
    var item = $('#cbEmpresaF1F2').val();

    return item;
}

function getUbicacionF1F2() {
    var item = $('#cbUbicacionF1F2').val();

    return item;
}

function getEquipoF1F2() {
    var item = $('#cbEquipoF1F2').val();

    return item;
}

function getVersionF1F2() {
    return $("#hfVersionF1F2").val();
}

function getHojaF1F2() {
    return $("#hfHojaF1F2").val();
}

function getSimilitud() {
    var texto = $("#textSimilitud").val();

    var valor = parseFloat(texto) || 0;
    if (valor < 0) valor = 70;
    if (valor > 100) valor = 99.9;

    $("#textSimilitud").val(valor);

    return valor;
}