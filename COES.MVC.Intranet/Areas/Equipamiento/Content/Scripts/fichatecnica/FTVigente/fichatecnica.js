var controlador = siteRoot + 'Equipamiento/FTVigente/';
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

    $('#btnExportarReporte').click(function () {

        var mostrarComent = $("#hflagChekComentario").val();
        var mostrarCheckSustento = $("#hflagChekSustento").val();
        var existeComent = $("#hflagExisteComentario").val();

        if (mostrarCheckSustento == 1 || (existeComent == 1 && mostrarComent == 1)) {
            setTimeout(function () {
                $("#popupExportarReporte").bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown',
                    modalClose: false
                });
            }, 50);
        }
        else {
            exportarMasivoFichaTecnica();
        }
    });
    //$('#btnCancelarExportacion').click(function () {
    //    $('#popupExportarReporte').bPopup().close();
    //});

    ancho = $('#mainLayout').width() - 30;

    setTimeout(function () { //tiempo usado al presionar botón Regresar
        listarEmpresas();
    }, 1000);


});


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

                    $("#hflagChekComentario").val(result.FlagCheckComent);
                    $("#hflagChekSustento").val(result.FlagCheckSust);

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
        listarEmpresas();
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

                $("#hflagExisteComentario").val(evt.FlagExisteComentario);
                //DIBUJAR POPPUT
                var htmlPoput = _dibujarContenidoPopput();
                $('#vistaExportar').html(htmlPoput);

                //evento descargar popput
                $('#btnExportarMasivo').on('click', function () {
                    exportarMasivoFichaTecnica();
                });
                $('#btnCancelarExportacion').click(function () {
                    $('#popupExportarReporte').bPopup().close();
                });


                $('#listado').css("width", ancho + "px");

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

            } else {
                alert(evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarError();
        }
    });
};

function _dibujarContenidoPopput() {

    var cadena = '';
    var htmlComentario = "";
    var htmlSustento = "";

    var existeComent = $("#hflagExisteComentario").val();

    var mostrarComent = $("#hflagChekComentario").val();
    var mostrarCheckSustento = $("#hflagChekSustento").val();

    if (existeComent == 1 && mostrarComent == 1)
        htmlComentario = ` <input type="checkbox" id="chkComent" name="chkComent" value="" checked> <span>Incluir comentario</span> <br />`;

    if (mostrarCheckSustento == 1)
        htmlSustento =  ` <input type="checkbox" id="chkSust" name="chkSust" value=""> <span>Incluir sustento</span> <br />`;

    cadena += `
            <div class="content-tabla">

                <div style="width:250px; padding-left: 30px;">
                    <div style="width:250px; padding-top:30px;">
                        <div style=" padding-bottom: 10px;">
                            Tipo Exportación:
                        </div>
                        <div>
                                ${htmlComentario}
                                ${htmlSustento}
                        </div>
                    </div>
                </div>
                <div style="width:250px; text-align:center; padding:30px;">
                    <div style="width:250px;">
                        <input type="button" id="btnExportarMasivo" value="Exportar" />
                        <input type="button" id="btnCancelarExportacion" value="Cancelar" />
                    </div>
                </div>
            </div>
    `;

    return cadena;
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

            <tr id="fila_${item.Codigo}" class=${claseFila}>
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

    //var strEstado = "A,P,F" // TODOS LOS ESTADOS VALIDOS
    //if ($("#cbEstado").val() != "-2") {
    //    strEstado = $("#cbEstado").val();
    //}

    var opcionComent = $('#chkComent').is(":checked")? true : false ;
    var opcionSust = $('#chkSust').is(":checked")? true : false ;

    if (idFT > 0) {
        $.ajax({
            type: 'POST',
            url: controlador + 'ExportarMasivoFichaTecnica',
            data: {
                idFT: idFT,
                opcionComentario: opcionComent,
                opcionSustento: opcionSust,
                iEmpresa: $("#cbEmpresa").val()
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