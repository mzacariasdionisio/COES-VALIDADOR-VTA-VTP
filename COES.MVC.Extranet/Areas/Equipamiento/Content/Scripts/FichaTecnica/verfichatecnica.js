var controlador = siteRoot + 'Equipamiento/fichatecnica/';

$(function () {
    $('#cbFichaTecnica').change(function () {
        listarEmpresas();
    });

    $('#btnConsultar').on('click', function () {
        buscarElementos();
    });

    setTimeout(function () { //tiempo usado al presionar botón Regresar, toma valor correcto
        listarEmpresas();
    }, 1000);
});


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
                if (result.Resultado != "-1") {
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
                    }

                    if (result.Resultado == "2") {
                        alert("El módulo extranet no cuenta con ningún equipo asignado a su empresa.");
                        window.location.href = siteRoot + 'Equipamiento/Envio/' + "Index";
                    }
                } else {
                    mostrarError();
                }
            },
            error: function () {
                mostrarError();
            }
        });
    } else {
        alert("El módulo extranet no cuenta con ningún equipo asignado a su empresa.");
        window.location.href = siteRoot + 'Equipamiento/Envio/' + "Index";
    }
}

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
}

function mostrarError() {
    $('#textoMensaje').css("display", "block");
    $('#textoMensaje').removeClass();
    $('#textoMensaje').addClass('action-alert');
    $('#textoMensaje').text("Ha ocurrido un error");
}


function pintarBusqueda() {
    $.ajax({
        type: 'POST',
        url: controlador + "ElementosListado",
        data: {
            idFicha: $("#cbFichaTecnica").val(),
            iEmpresa: $("#cbEmpresa").val(),
            nombre: $("#nombre").val(),
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                $('#listado').css("width", $('#mainLayout').width() + "px");

                $('#listado').html(evt);
                var html = _dibujarTablaListado(evt);
                $('#listado').html(html);

                $('#tabla').dataTable({
                    "scrollY": 500,
                    scrollCollapse: true,
                    "scrollX": false,
                    //"sDom": 'ft',
                    "ordering": true,
                    order: [[1, 'asc']],
                    "paging": true,
                    lengthMenu: [
                        [15, 25, 50, 100, 200, -1],
                        [15, 25, 50, 100, 200, 'TODOS'],
                    ],
                });

                //eventoContextMenu();
                

            } else {
                alert(evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarError();
        }
    });
}

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

        //var tdOpciones = _tdAcciones(model, item);
        cadena += `

            <tr id="fila_${item.Codigo}" class="context-menu-fila">
                <td class="ft_item" style='text-align: center !important;'>${item.Codigo}</td>
                <td class="ft_item">${item.Nombre}</td>
                <td class="ft_item" style='text-align: center !important;'>${item.Abreviatura}</td>
                ${htmlTdEmpresa}
                <td class="ft_item">${item.Ubicacion}</td>               
                <td>
                    <a href="javascript:verDetalle('${model.IdFicha}', '${item.Codigo}');">Ver Detalle</a>
                </td>
            </tr>
        `;
    }
    cadena += "</tbody></table>";

    return cadena;
}


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

function verDetalle(idFicha, idElemento) {
    window.location.href = controlador + "IndexDetalle?idFicha=" + idFicha + "&idElemento=" + idElemento;
}