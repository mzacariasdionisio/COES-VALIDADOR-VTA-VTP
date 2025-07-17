var controlador = siteRoot + 'ConsumoCombustible/VCOM/';

var IMG_EDITAR = '<img src="' + siteRoot + 'Content/Images/btn-edit.png" alt="" width="19" height="19" style="">';
var IMG_ELIMINAR = '<img src="' + siteRoot + 'Content/Images/Trash.png" alt="" width="19" height="19" style="">';
var IMG_AGREGAR = '<img src="' + siteRoot + 'Content/Images/btn-open.png" alt="" width="19" height="19" style="">';

var LISTA_MODO_OP = [];
var LISTA_COMBUSTIBLE = [];
var LISTA_DETALLE_VCOM = [];

var TIPO_OBJETO_MODO = 1;
var TIPO_OBJETO_COMB = 2;
var TIPO_OBJETO_DETALLE = 3;

var LISTA_CAMBIO_PENDIENTE = [];

var TIPO_CAMBIO_NUEVO = 1;
var TIPO_CAMBIO_MODIFICAR = 2;
var TIPO_CAMBIO_ELIMINAR = 3;

$(function () {

    $('#desc_fecha_ini').Zebra_DatePicker({
    });

    $("#btnExportarExcel").click(function () {
        exportarExcel($("#hfVersion").val());
    });

    $("#btnDescargaLog").click(function () {
        var archivo = $("#hfNombreArchivoLogCambio").val();
        window.location = controlador + "DescargarArchivoLog?fileName=" + archivo;
    });

    $("#btnRegresar").click(function () {
        regresarPantallaPrincipal();
    });

    //formulario
    $("#btnNuevo").click(function () {
        inicializarFormulario_(0, false);
    });
    $("#btnGuardarCambio").click(function () {
        guardarListaCambio();
    });

    listado();
    listarCambioPendiente();
});

///////////////////////////
/// Listar filtros
///////////////////////////

function cargarListaCentral() {
    $("#div_central_filtro").html('');

    $.ajax({
        type: 'POST',
        url: controlador + 'ViewCargarFiltros',
        dataType: 'json',
        data: {
            vercodi: $("#hfVersion").val(),
            empresa: getEmpresa(),
        },
        cache: false,
        success: function (data) {
            $("#div_central_filtro").html('<select id="cbCentral" name="cbCentral"></select>');

            if (data.ListaCentral.length > 0) {
                $.each(data.ListaCentral, function (i, item) {
                    $('#cbCentral').get(0).options[$('#cbCentral').get(0).options.length] = new Option(item.Central, item.Equipadre);
                });
            }

            $('#cbCentral').multipleSelect({
                width: '220px',
                filter: true,
                single: false,
                onClose: function () {
                    listado();
                }
            });
            $('#cbCentral').multipleSelect('checkAll');

            listado();
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function getEmpresa() {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "[object Object]" || empresa.length == 0) empresa = "-1";
    $('#hfEmpresa').val(empresa);
    var idEmpresa = $('#hfEmpresa').val();

    return idEmpresa;
}

function getCentral() {
    var central = $('#cbCentral').multipleSelect('getSelects');
    if (central == "[object Object]" || central.length == 0) central = "-1";
    $('#hfCentral').val(central);
    var idEmpresa = $('#hfCentral').val();

    return idEmpresa;
}

//////////////////////////////////////////////////////
/// tabla web  y exportacion excel
//////////////////////////////////////////////////////

function listado() {
    $('#listado').html('');

    ancho = $("#mainLayout").width() > 1100 ? $("#mainLayout").width() - 30 : 1100;

    $.ajax({
        type: 'POST',
        url: controlador + "ViewReporteWeb",
        data: {
            vercodi: $("#hfVersion").val(),
            empresa: getEmpresa(),
            central: getCentral()
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                LISTA_MODO_OP = evt.ListaModoOp;
                LISTA_COMBUSTIBLE = evt.ListaFuenteEnergia;
                LISTA_DETALLE_VCOM = evt.ListaDetalleVCOM;
                LISTA_CAMBIO_PENDIENTE = [];

                var html = '';
                html += dibujarObservaciones(evt.Version);
                html += dibujarTablaReporte(evt.ListaDetalleVCOM);
                $('#listado').html(html);

                $("#listado").css("width", (ancho) + "px");
                $('#tabla_reporte').dataTable({
                    "sPaginationType": "full_numbers",
                    "destroy": "true",
                    "ordering": false,
                    "searching": true,
                    "iDisplayLength": -1,
                    "info": false,
                    "paging": false,
                    "scrollX": true,
                    "scrollY": "400px"
                });
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function dibujarTablaReporte(lista) {

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono dataTable" cellspacing="0" width="100%" id="tabla_reporte">
        <thead>
            <tr>
                <th style='width: 70px'>Acción</th>

                <th style=''>Empresa</th>
                <th style=''>Central</th>
                <th style=''>Modo de operación</th>
                <th style=''>Combustible</th>

                <th style=''>Medida COES</th>
                <th style='background-color: #00B050;'>Consumo OSINERGMIN</th>
                <th style='background-color: #00B050;'>Medida Osinergmin</th>

                <th style=''>Código Modo de operación</th>
                <th style=''>Código Combustible</th>      
            </tr>
        </thead>
        <tbody>
    `;

    for (key in lista) {
        var reg = lista[key];

        cadena += `
            <tr>
                <td style="height: 24px">
                    <a title="Editar registro" href="JavaScript:editarRegistro(${reg.Vcomcodi});">${IMG_EDITAR} </a>
                    <a title="Eliminar registro" href="JavaScript:eliminarRegistro(${reg.Vcomcodi});">${IMG_ELIMINAR} </a>
                </td>

                <td class='emprcodi_${reg.Emprcodi}' style='text-align: center;height: 20px;'>${reg.Emprnomb}</td>
                <td class='equipadre_${reg.Equipadre}' style='text-align: center'>${reg.Central}</td>   
                <td class='grupocodi_${reg.Grupocodi}' style='text-align: center'>${reg.Gruponomb}</td>   
                <td class='fenergcodi_${reg.Fenergcodi}' style='text-align: center'>${reg.Fenergnomb}</td>  

                <td style="text-align: center;">${reg.Tinfcoesabrev}</td>
                <td style="text-align: center;">${reg.VcomvalorDesc}</td>
                <td style="text-align: center;">${reg.Tinfosiabrev}</td>

                <td style="text-align: center;">${reg.Vcomcodigomop}</td>
                <td style="text-align: center;">${reg.Vcomcodigotcomb}</td>
            </tr>
        `;
    }
    cadena += "</tbody></table>";

    return cadena;
}

function dibujarObservaciones(version) {
    var html = `<div id='mensaje_version'>`;
    if (version.ListaObs.length > 0) {
        html += `Observaciones: <br/>
            <ul>
        `;

        for (var i = 0; i < version.ListaObs.length; i++) {
            html += `<li>${version.ListaObs[i]}</li>        `;
        }

        html += `
            </ul>
        `;
    }

    html += '</div>';

    return html;
}

function exportarExcel(id) {

    $.ajax({
        type: 'POST',
        url: controlador + "GenerarArchivoExcelReporte",
        data: {
            vercodi: id,
            empresa: getEmpresa(),
            central: getCentral(),
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                window.location = controlador + "Exportar?file_name=" + evt.Resultado;
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });

}

function editarRegistro(vcomcodi) {
    $("#mensaje").hide();
    inicializarFormulario_(vcomcodi, true);
}

function eliminarRegistro(vcomcodi) {
    if (confirm("¿Desea eliminar el registro? Esta eliminación se guarda en Cambios Pendientes")) {
        var objDet = _getObjXLista(TIPO_OBJETO_DETALLE, vcomcodi);

        var objCambio = {
            Vcomcodi: objDet.Vcomcodi,
            NroCambio: LISTA_CAMBIO_PENDIENTE.length + 1,
            TipoCambioDesc: "Eliminación",
            TipoCambio: TIPO_CAMBIO_ELIMINAR,
            Grupocodi: objDet.Grupocodi,
            Gruponomb: objDet.Gruponomb,
            FenergcodiAnterior: objDet.Fenergcodi,
            FenergnombAnterior: objDet.Fenergnomb,
            FenergnombFinal: '',
            ConsumoAnterior: objDet.Vcomvalor,
            ConsumoFinal: ''
        };

        LISTA_CAMBIO_PENDIENTE.push(objCambio);
    }

    listarCambioPendiente();
}

function regresarPantallaPrincipal() {
    var fecha = $("#hfFechaPeriodo").val();
    fecha = fecha.replace("/", "-");
    fecha = fecha.replace("/", "-");

    window.location.href = siteRoot + "ConsumoCombustible/VCOM/Index?fechaConsulta=" + fecha;
}

//////////////////////////////////////////////////////
/// Formulario web
//////////////////////////////////////////////////////

function inicializarFormulario_(vcomcodi, editable) {
    $("#div_formulario").html('');

    //Inicializar Formulario
    var tituloPopup = editable ? 'Edición de registro' : 'Nuevo registro';
    $("#popup_formulario .popup-title span").html(tituloPopup);

    var htmlForm = dibujarHtmlFormulario_(vcomcodi, editable);
    $("#div_formulario").html(htmlForm);

    var btnGuardar = editable ? 'Guardar modificación en Cambios Pendientes' : 'Guardar nuevo en Cambios Pendientes';;
    $("#btnGrabarForm").val(btnGuardar);

    //inicializarVistaFormulario();
    cargarEventosFormulario(editable);

    $('#popup_formulario').bPopup({
        modalClose: false,
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown',
        onClose: function () {
            $('#popup').empty();
        }
    });

}

function dibujarHtmlFormulario_(vcomcodi, editable) {
    var idModo = 0;
    var nombreModo = '';
    var idFenerg = 0;
    var consumo = 0;
    if (editable) {
        var objDet = _getObjXLista(TIPO_OBJETO_DETALLE, vcomcodi);
        idModo = objDet.Grupocodi;
        nombreModo = objDet.Gruponomb;
        idFenerg = objDet.Fenergcodi;
        consumo = objDet.Vcomvalor;
    }

    //Modo de operación
    var htmlcbFormModo = '';
    if (!editable) {
        htmlcbFormModo += `<select id='cbFormModo'>`;
        htmlcbFormModo += `<option value='0'>--SELECCIONE--</option>`;
        for (var i = 0; i < LISTA_MODO_OP.length; i++) {
            var v = LISTA_MODO_OP[i];
            var esSelected = v.Grupocodi == idModo ? ' selected ' : '';
            var estado = v.GrupoEstado != 'A' ? `(${v.GrupoEstadoDesc})` : '';
            htmlcbFormModo += `<option value='${v.Grupocodi}' ${esSelected}>${v.Grupocodi} ${v.Gruponomb} ${estado}</option>`;
        }
        htmlcbFormModo += `</select>`;
    } else {
        htmlcbFormModo += `<input type='text' value='${nombreModo}' style="width: 250px" disabled /> `;
        htmlcbFormModo += `<input type='hidden' id='cbFormModo' value='${idModo}' /> `;
    }

    //Fuente de energía
    var htmlcbFormFenerg = '';
    htmlcbFormFenerg += `<select id='cbFormFenerg'>`;
    htmlcbFormFenerg += `<option value='0'>--SELECCIONE--</option>`;
    for (var i = 0; i < LISTA_COMBUSTIBLE.length; i++) {
        var v = LISTA_COMBUSTIBLE[i];
        var esSelected = v.Fenergcodi == idFenerg ? ' selected ' : '';
        htmlcbFormFenerg += `<option value='${v.Fenergcodi}' ${esSelected}>${v.Fenergnomb}</option>`;
    }
    htmlcbFormFenerg += `</select>`;


    //Div general
    var html = `
        <input type="hidden" id="idVcomcodi" value="${vcomcodi}" />

        <div class="content-registro" style="width:auto">

            <div style="clear:both; height:10px;"></div>

            <table cellpadding="8" style="width:auto">
                <tbody>
                    <tr>
                        <td class="tbform-label" style="width:140px;">Modo de Operación:</td>
                        <td class="registro-control" style="width:300px;">
                            ${htmlcbFormModo}
                        </td>
                    </tr>
                    <tr>
                        <td class="tbform-label" style="">Combustible:</td>
                        <td class="registro-control" style="width:300px;">
                            ${htmlcbFormFenerg}
                        </td>
                    </tr>
                    <tr>
                        <td class="tbform-label" style="">Consumo Osinergmin:</td>
                        <td class="registro-control" style="width:300px;">
                            <input type="text" id='consumo' value="${consumo}" style="width: 100px" >
                        </td>
                    </tr>
                </tbody>
            </table>

            <table class="btnAcciones" style="margin-left: 159px;">
                 <tbody><tr>
                        <td>
                            <input type="button" id="btnGrabarForm" value="Guardar">
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
`;

    return html;
}

function cargarEventosFormulario(editable) {
    $("#cbFormModo").unbind();
    $("#cbFormFenerg").unbind();

    if (!editable) {
        $('#cbFormModo').multipleSelect({
            width: '250px',
            filter: true,
            single: true,
            onClose: function () {

                var objModo = _getObjXLista(TIPO_OBJETO_MODO, $("#cbFormModo").val() || 0);
                $("#cbFormFenerg").multipleSelect("setSelects", [objModo.Fenergcodi || 0]);

            }
        });
        $("#cbFormModo").multipleSelect("setSelects", [$("#cbFormModo").val() || 0]);
    }

    $('#cbFormFenerg').multipleSelect({
        width: '250px',
        filter: true,
        single: true,
        onClose: function () {
        }
    });
    $("#cbFormFenerg").multipleSelect("setSelects", [$("#cbFormFenerg").val() || 0]);

    //input de consumo
    $('#consumo').unbind();
    $('#consumo').bind('keypress', function (e) {
        var key = window.Event ? e.which : e.keyCode
        return (key <= 13 || (key >= 48 && key <= 57) || key == 46 || key == 45);
    });
    $('#consumo').bind('paste', function (e) {
        var key = window.Event ? e.which : e.keyCode
        return (key <= 13 || (key >= 48 && key <= 57) || key == 46 || key == 45);
    });

    $("#btnGrabarForm").unbind();
    $("#btnGrabarForm").click(function () {
        guardarFormularioCambioPendiente();
    });

}

function guardarFormularioCambioPendiente() {
    var idForm = parseInt($("#idVcomcodi").val()) || 0;
    var grupocodi = parseInt($("#cbFormModo").val()) || 0;
    var fenergcodi = parseInt($("#cbFormFenerg").val()) || 0;
    var consumoTexto = $("#consumo").val().trim();
    var consumo = parseFloat($("#consumo").val()) || 0;

    //Validación
    var msj = '';
    if (grupocodi <= 0) msj += "Debe seleccionar modo de operación\n";
    if (fenergcodi <= 0) msj += "Debe seleccionar combustible\n";
    if (consumoTexto == '') msj += "Debe ingresar Consumo Osinergmin\n";
    if (consumo <= 0) msj += "El Consumo Osinergmin debe ser mayor a cero\n";

    var objDet = _getObjXLista(TIPO_OBJETO_DETALLE, idForm);
    var objGrupo = _getObjXLista(TIPO_OBJETO_MODO, grupocodi);
    var objFuente = _getObjXLista(TIPO_OBJETO_COMB, fenergcodi);

    if (msj == "") {
        var objCambio = {};
        if (idForm > 0) {
            objCambio = {
                Vcomcodi: objDet.Vcomcodi,
                NroCambio: LISTA_CAMBIO_PENDIENTE.length + 1,
                TipoCambioDesc: "Modificación",
                TipoCambio: TIPO_CAMBIO_MODIFICAR,
                Grupocodi: objDet.Grupocodi,
                Gruponomb: objDet.Gruponomb,
                FenergcodiFinal: fenergcodi,
                FenergnombAnterior: objDet.Fenergnomb,
                FenergnombFinal: objFuente.Fenergnomb,
                ConsumoAnterior: objDet.Vcomvalor,
                ConsumoFinal: consumo
            };

            //Validación
            var tieneCambio = false;
            if (objDet.Fenergnomb != objFuente.Fenergnomb) tieneCambio = true;
            if (objDet.Vcomvalor != consumo) tieneCambio = true;

            if (!tieneCambio) msj += "El registro no tiene cambio. No se permite la edición";

        } else {

            objCambio = {
                Vcomcodi:0,
                NroCambio: LISTA_CAMBIO_PENDIENTE.length + 1,
                TipoCambioDesc: "Nuevo",
                TipoCambio: TIPO_CAMBIO_NUEVO,
                Grupocodi: objGrupo.Grupocodi,
                Gruponomb: objGrupo.Gruponomb,
                FenergcodiFinal: fenergcodi,
                FenergnombAnterior: '',
                FenergnombFinal: objFuente.Fenergnomb,
                ConsumoAnterior: '',
                ConsumoFinal: consumo
            };

            //Validación
            var objExistente = _getRegistroExistenteVCOM(objGrupo.Grupocodi, fenergcodi);
            if (objExistente != null) msj += "Ya existe un registro con el mismo modo de operación y combustible. No se permite agregar.";
        }


        if (msj == "") {
            LISTA_CAMBIO_PENDIENTE.push(objCambio);
            $('#popup_formulario').bPopup().close();
        } else {
            alert(msj);
        }
    }
    else {
        alert(msj);
    }

    listarCambioPendiente();
}

function _getObjXLista(tipoObjeto, codigo) {

    switch (tipoObjeto) {
        case TIPO_OBJETO_MODO:
            for (var i = 0; i < LISTA_MODO_OP.length; i++) {
                if (LISTA_MODO_OP[i].Grupocodi == codigo)
                    return LISTA_MODO_OP[i];
            }
            break;
        case TIPO_OBJETO_COMB:
            for (var i = 0; i < LISTA_COMBUSTIBLE.length; i++) {
                if (LISTA_COMBUSTIBLE[i].Fenergcodi == codigo)
                    return LISTA_COMBUSTIBLE[i];
            }
            break;
        case TIPO_OBJETO_DETALLE:
            for (var i = 0; i < LISTA_DETALLE_VCOM.length; i++) {
                if (LISTA_DETALLE_VCOM[i].Vcomcodi == codigo)
                    return LISTA_DETALLE_VCOM[i];
            }
            break;
    }

    return null;
}

function _getRegistroExistenteVCOM(grupocodi, fenergcodi) {
    for (var i = 0; i < LISTA_DETALLE_VCOM.length; i++) {
        if (LISTA_DETALLE_VCOM[i].Grupocodi == grupocodi && LISTA_DETALLE_VCOM[i].Fenergcodi == fenergcodi)
            return LISTA_DETALLE_VCOM[i];
    }

    return null;
}

//////////////////////////////////////////////////////
/// Cambios pendientes
//////////////////////////////////////////////////////

function guardarListaCambio() {
    var model = {
        IdVersion: $("#hfVersion").val(),
        ListaCambio: LISTA_CAMBIO_PENDIENTE
    };

    if (LISTA_CAMBIO_PENDIENTE.length > 0) {
        if (confirm("¿Desea guardar los cambios? Esta acción crea una nueva versión")) {
            $.ajax({
                type: 'POST',
                url: controlador + "GuardarListaCambioVersion",
                traditional: true,
                data: {
                    smodel: JSON.stringify(model)
                },
                success: function (evt) {
                    if (evt.Resultado != "-1") {
                        alert("La nueva versión se ha generado correctamente");
                        regresarPantallaPrincipal();
                    } else {
                        alert("Ha ocurrido un error: " + evt.Mensaje);
                    }
                },
                error: function (err) {
                    alert('Ha ocurrido un error.');
                }
            });
        }
    } else {
        alert("No existen cambios. No se permite la acción Guardar");
    }
}

function listarCambioPendiente() {

    var html = '';
    html += dibujarTablaCambio(LISTA_CAMBIO_PENDIENTE);
    $('#html_listado_cambio').html(html);
}

function dibujarTablaCambio(lista) {

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono dataTable" cellspacing="0" width="100%" id="tabla_cambio">
        <thead>
            <tr>
                <th style=''>N° cambio</th>
                <th style=''>Tipo Cambio</th>
                <th style=''>Modo de<br /> operación</th>
                <th style=''>Combustible <br />Anterior</th>
                <th style=''>Combustible <br /> Final</th>
                <th style=''>Consumo<br /> OSINERGMIN <br />Anterior</th>
                <th style=''>Consumo <br />OSINERGMIN <br/> Final</th>      
            </tr>
        </thead>
        <tbody>
    `;

    if (lista.length > 0) {
        for (key in lista) {
            var reg = lista[key];

            cadena += `
            <tr>
                <td style="text-align: center;">${reg.NroCambio}</td>
                <td style="text-align: center;">${reg.TipoCambioDesc}</td>
                <td style="text-align: center;">${reg.Gruponomb}</td>

                <td style="text-align: center;">${reg.FenergnombAnterior}</td>
                <td style="text-align: center;">${reg.FenergnombFinal}</td>

                <td style="text-align: center;">${reg.ConsumoAnterior}</td>
                <td style="text-align: center;">${reg.ConsumoFinal}</td>
            </tr>
        `;
        }
    } else {
        cadena += `
            <tr>
                <td colspan="7" style="text-align: left;">No existen registros</td>
            </tr>
        `;
    }
    cadena += "</tbody></table>";

    return cadena;
}
