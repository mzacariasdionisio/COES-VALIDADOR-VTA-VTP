var controlador = siteRoot + 'PMPO/codigosddp/';
var ANCHO_REPORTE = 1000;
var validarCambioDePestaña = true;

$(document).ready(function () {
    $('#cntMenu').css("display", "none");

    $('#tab-container').easytabs({
        animate: false
    });
    $('#tab-container').easytabs('select', '#tabListado');

    $('#tab-container').bind('easytabs:before', function () {
        var esTabDetalle = $("#tab-container .tab.active").html().toLowerCase().includes('detalle');
        var esVisible = $("#divDetalle").is(":visible");

        if (esTabDetalle && esVisible && validarCambioDePestaña) {
            if (confirm('¿Desea cambiar de pestaña? Si selecciona "Aceptar" se perderán los cambios. Si selecciona "Cancelar" se mantendrá en la misma pestaña')) {
                $("#divDetalle").hide();//ocultar detalle
                //limpiarBarraMensaje('mensaje');
            } else {
                return false;
            }
        }
        validarCambioDePestaña = true;
    });

    $("#tipo_sddp").change(function () {
        consultarListado();
    });
    $("#btnConsultar").click(function () {
        consultarListado();
    });

    ANCHO_REPORTE = $("#mainLayout").width() > 1100 ? $("#mainLayout").width() - 40 : 1100;

    //formulario
    $("#btnNuevo").click(function () {
        formularioNuevo();
    });
    $("#btnGuardar").click(function () {
        guardarCodigo();
    });

    $("#btnCancelar").click(function () {
        consultarListado();
    });

    consultarListado();
});

function consultarListado() {
    //$('#tab-container').easytabs('select', '#tabListado');
    $("#divDetalle").hide();//ocultar detalle
    $('#tab-container').easytabs('select', '#tabListado');

    var tsddpcodi = parseInt($("#tipo_sddp").val()) || 0;
    $("#mensaje").hide();

    $.ajax({
        type: 'POST',
        url: controlador + "ListaCodigo",
        dataType: 'json',
        data: {
            tsddpcodi: tsddpcodi,
        },
        success: function (model) {
            $('#divTablaListado').html('');

            if (model.Resultado != "-1") {
                $('#divTablaListado').html(dibujarTablaCodigo(model.ListaCodigoSDDP));

                $('#divListado').css("width", ANCHO_REPORTE + "px");

                $('#reporteCodigo').dataTable({
                    "scrollY": 530,
                    "scrollX": true,
                    "destroy": "true",
                    "sDom": 'ft',
                    "ordering": false,
                    "iDisplayLength": -1,
                    "stripeClasses": [],
                });

            } else {
                $("#mensaje").show();
                mostrarMensaje("mensaje", model.Mensaje, $tipoMensajeError, $modoMensajeCuadro);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function dibujarTablaCodigo(lista) {
    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono dataTable" cellspacing="0" width="auto" id="reporteCodigo">
        <thead>
            <tr>
                <th>Opción</th>
                <th>Mensaje</th>
                <th>Tipo</th>
                <th>Num SDDP</th>
                <th>Nombre SDDP</th>

                <th></th>
                <th>Punto de medición PMPO</th>

                <th>Usuario mod.</th>
                <th>Fecha mod.</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in lista) {
        var obj = lista[key];

        var htmlTd = '<td></td>';
        if (obj.Ptomedicodi > 0) {
            htmlTd = `<td onclick="editarPtomedicodi(${obj.Ptomedicodi}, '${obj.Ptomedielenomb}');"><img src="${siteRoot}Content/Images/btn-open.png" style="height: 20px;"></td>`;
        }

        cadena += `
            <tr>
                <td onclick="editarCodigo(${obj.Sddpcodi});"><img src="${siteRoot}Content/Images/btn-edit.png" style="height: 20px;"></td>

                <td>${obj.MensajeValidacion}</td>
                <td>${obj.Tsddpnomb}</td>
                <td>${obj.Sddpnum}</td>
                <td>${obj.Sddpnomb}</td>

                ${htmlTd}
                <td>${obj.PtoPmpo}</td>

                <td>${obj.UltimaModificacionUsuarioDesc}</td>
                <td>${obj.UltimaModificacionFechaDesc}</td>
            </tr>
        `;
    }
    cadena += "</tbody></table>";

    return cadena;
}

function editarPtomedicodi(idPto, elenomb) {
    var tipo = elenomb == "EM" ? 1: 2;
    var url = siteRoot + 'Hidrologia/PtoMedicion/' + "Index?codigo=" + idPto + "&tipoFuente=" + tipo;
    window.open(url, '_blank').focus();
}

//////////////////////////////////////////////////////////////////////////
// Formulario
//////////////////////////////////////////////////////////////////////////
function formularioNuevo() {
    $('#tab-container').easytabs('select', '#tabDetalle');

    $("#hfSddpcodi").val("0");
    $("#cbTipoForm").val("1");
    $("#txtnombreForm").val("");
    $("#txtcodigoForm").val("");
    $("#txtPtoPmpoForm").val("");

    $("#divDetalle").show();// mostrar detalle
}

function guardarCodigo() {
    var sddpcodi = parseInt($("#hfSddpcodi").val()) || 0;
    var tsddpcodi = parseInt($("#cbTipoForm").val()) || 0;
    var sddpnomb = $("#txtnombreForm").val();
    var sddpnum = parseInt($("#txtcodigoForm").val()) || 0;
    var ptomedicodi = parseInt($("#txtPtoPmpoForm").val()) || 0;

    $.ajax({
        type: 'POST',
        url: controlador + "GuardarCodigo",
        dataType: 'json',
        data: {
            Tsddpcodi: tsddpcodi,
            Sddpcodi: sddpcodi,
            Sddpnum: sddpnum,
            Sddpnomb: sddpnomb,
            Ptomedicodi: ptomedicodi,
        },
        cache: false,
        success: function (model) {
            if (model.Resultado != "-1") {
                validarCambioDePestaña = false;
                mostrarMensaje("mensaje", "El registro se guardó correctamente.", $tipoMensajeExito, $modoMensajeCuadro);
                consultarListado();
            } else {
                $("#mensaje").show();
                mostrarMensaje("mensaje", model.Mensaje, $tipoMensajeError, $modoMensajeCuadro);
            }
        },
        error: function () {
        }
    });
}

function editarCodigo(sddpcodi) {
    $("#mensaje").hide();

    $.ajax({
        type: 'POST',
        url: controlador + "ObtenerCodigo",
        dataType: 'json',
        data: {
            sddpcodi: sddpcodi,
        },
        success: function (model) {
            if (model.Resultado != "-1") {
                var obj = model.CodigoSDDP;

                $("#hfSddpcodi").val(obj.Sddpcodi);
                $("#cbTipoForm").val(obj.Tsddpcodi);
                $("#txtnombreForm").val(obj.Sddpnomb);
                $("#txtcodigoForm").val(obj.Sddpnum);
                $("#txtPtoPmpoForm").val(obj.Ptomedicodi);

                $('#tab-container').easytabs('select', '#tabDetalle');
                $("#divDetalle").show();// mostrar detalle
            } else {
                $("#mensaje").show();
                mostrarMensaje("mensaje", model.Mensaje, $tipoMensajeError, $modoMensajeCuadro);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}