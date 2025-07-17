var controlador = siteRoot + 'equipamiento/fichatecnica/';
var ancho = 900;
var TIPO_SUBESTACION = -2;

$(function () {
    $('#cbFichaTecnica').change(function () {
        listarEmpresas();
    });

    $('#cbEmpresa').change(function () {
        buscarElementos();
    });

    $('#btnConsultar').on('click', function () {
        buscarElementos();
    });

    listarEmpresas();
});

///////////////////////////
/// Lista de Empresas
///////////////////////////

function listarEmpresas() {
    $(".td_empresa").hide();
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
    $("#listado").html('');

    var ficha = parseInt($("#cbFichaTecnica").val()) || 0;
    var msj = "";
    if (ficha == "0") {
        $("#cbEmpresa").val("-2");
        msj += "Debe seleccionar una Ficha Técnica";
    }

    if (msj == "") {
        mostrarListado();
    } else {
        $('#textoMensaje').css("display", "block");
        $('#textoMensaje').removeClass();
        $('#textoMensaje').addClass('action-alert');
        $('#textoMensaje').text(msj);
    }
};

function mostrarListado() {
    $.ajax({
        type: 'POST',
        url: controlador + "ElementosListado",
        data: {
            idFicha: $("#cbFichaTecnica").val(),
            iEmpresa: $("#cbEmpresa").val(),
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                $('#listado').css("width", $('#mainLayout').width() + "px");

                $('#listado').html(evt);
                var html = _dibujarTablaListado(evt);
                $('#listado').html(html);

                $('#tabla').dataTable({
                    scrollCollapse: true,
                    //"sDom": 'ft',
                    "ordering": true,
                    order: [[1, 'asc']],
                    lengthMenu: [
                        [15, 25, 50, 100],
                        [15, 25, 50, 100],
                    ],
                });
            }
            else {
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
    <table border="0" class="tabla-icono table table-hover" id="tabla" cellspacing="0" width="100%" >
        <thead>
            <tr>
            <th>Código</th>
            <th>Nombre </th>
            <th>Abreviatura</th>
            ${htmlThEmpresa}
            <th>Ubicación</th>
            <th style="width:120px">Ver Detalle</th>
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

            <tr id="fila_${item.Codigo}">
                <td class="ft_item" style='text-align: center !important;'>${item.Codigo}</td>
                <td class="ft_item">${item.Nombre}</td>
                <td class="ft_item" style='text-align: center !important;'>${item.Abreviatura}</td>
                ${htmlTdEmpresa}
                <td class="ft_item">${item.Ubicacion}</td>
                <td>
                    <a class="coes-button coes-button--plain-text" href="javascript:verDetalle('${model.IdFicha}', '${item.Codigo}');">Ver Detalle</a>
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