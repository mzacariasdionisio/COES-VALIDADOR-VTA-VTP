var controlador = siteRoot + 'PMPO/GeneracionArchivosDAT/';
var ANCHO_REPORTE = 1000;
var TSDDPCODI_TERMO = 2;
var actualizar = false;

$(document).ready(function () {
    $('#cntMenu').css("display", "none");

    $("#cbEmpresaFiltro").change(function () {
        consultarCorrelaciones();
    });
    $("#cbCategoriaFiltro").change(function () {
        consultarCorrelaciones();
    });
    $("#cbFamiliaFiltro").change(function () {
        consultarCorrelaciones();
    });
    $("#btnConsultar").click(function () {
        consultarCorrelaciones();
    });

    ANCHO_REPORTE = $("#mainLayout").width() > 1100 ? $("#mainLayout").width() - 30 : 1100;

    consultarCorrelaciones();

    //formulario
    $("#cbTipoGenerador").change(function () {
        listarCodigoSddp();
    });
    $("#cbTipoEquipo").change(function () {
        selectEquipo();
    });

    $("input[name=cbFuenteRelacion]").click(function () {
        _mostrarDivFuente();
    });

});

function _mostrarDivFuente() {
    var fuente = $('input[name=cbFuenteRelacion]:checked').val();

    $("#tbl_equipo").hide();
    $("#tblModo").hide();
    if (fuente == "1") { //equipo
        $("#tbl_equipo").show();
    } else {
        $("#tblModo").show();
    }
}

function consultarCorrelaciones() {
    var emprcodi = parseInt($("#cbEmpresaFiltro").val()) || 0;
    var catecodi = parseInt($("#cbCategoriaFiltro").val()) || 0;
    var famcodi = parseInt($("#cbFamiliaFiltro").val()) || 0;
    var tipoReporteMantto = $('input[name="rbMantto"]:checked').val();

    $("#mensaje").hide();

    $.ajax({
        type: 'POST',
        url: controlador + "ListaCorrelaciones",
        dataType: 'json',
        data: {
            emprcodi: emprcodi,
            tsppcodi: catecodi,
            famcodi: famcodi,
            tipoReporteMantto: tipoReporteMantto
        },
        success: function (model) {
            $('#divTablaCorrelacion').html('');

            if (model.Resultado != "-1") {
                if (model.TieneAlerta) {
                    $("#mensaje").show();
                    mostrarMensaje("mensaje", "Existen mensajes de validación.", $tipoMensajeAlerta, $modoMensajeCuadro); //<ul>" + htmlL + "</ul>
                }

                $('#divTablaCorrelacion').html(dibujarTablaCorrelacion(model.Correlaciones));

                $('#listado').css("width", ANCHO_REPORTE + "px");

                $('#reporteCorrelacion').dataTable({
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

function dibujarTablaCorrelacion(lista) {
    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono dataTable" cellspacing="0" width="auto" id="reporteCorrelacion">
        <thead>
            <tr>
                <th rowspan="2">Mensaje</th>
                <th colspan="3">Código SDDP</th>
                <th colspan="3" style="background-color: #008000;">Grupo</th>
                <th colspan="4">Equipo</th>
                <th colspan="5">Datos de correlación</th>
            </tr>
            <tr>
                <th>Tipo</th>
                <th>Num SDDP</th>
                <th>Nombre SDDP</th>

                <th style="background-color: #008000;">Empresa</th>
                <th style="background-color: #008000;">Código</th>
                <th style="background-color: #008000;">Grupo</th>

                <th>Ubicación</th>
                <th>T.eq</th>
                <th>Equipo</th>
                <th>Código</th>

                <th>Opción</th>
                <th>% Indisp</th>
                <th>Agrupación</th>
                <th>Usuario Mod.</th>
                <th>Fecha Mod.</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in lista) {
        var correlacion = lista[key];

        if (correlacion.Grupocodimodo <= 0) {
            cadena += `
                <tr>
                    <td>${correlacion.MensajeAlerta}</td>

                    <td>${correlacion.Grupotipo}</td>
                    <td>${correlacion.Sddpnum}</td>
                    <td>${correlacion.Sddpnomb}</td>

                    <td>${correlacion.Emprnomb}</td>
                    <td>${correlacion.GrupoCodi}</td>
                    <td>${correlacion.GrupoNomb}</td>

                    <td>${correlacion.AreaNomb}</td>
                    <td>${correlacion.Famabrev}</td>
                    <td>${correlacion.EquiAbrev}</td>
                    <td>${correlacion.EquiCodi}</td>

                    <td>
                        <a onclick="popupEdit('${correlacion.PmCindCodi}');">
                            <img src="${siteRoot}Content/Images/btn-edit.png" st style="cursor:pointer" title="Editar el registro" alt="Editar el registro" />
                        </a>
                        <a onclick="eliminarCorrelacion('${correlacion.PmCindCodi}');">
                            <img src="${siteRoot}Content/Images/btn-cancel.png" st style="cursor:pointer" title="Eliminar el registro" alt="Eliminar el registro" />
                        </a>
                    </td>
                    <td>${correlacion.PmCindPorcentaje}</td>
                    <td>${correlacion.PmCindConJuntoEqp}</td>
                    <td>${correlacion.UsuarioMod}</td>
                    <td>${correlacion.FechaModStr}</td>
                </tr>
            `;
        } else {
            cadena += `
                <tr>
                    <td>${correlacion.MensajeAlerta}</td>

                    <td>${correlacion.Grupotipo}</td>
                    <td>${correlacion.Sddpnum}</td>
                    <td>${correlacion.Sddpnomb}</td>

                    <td>${correlacion.Emprnomb}</td>
                    <td>${correlacion.GrupoCodi}</td>
                    <td>${correlacion.GrupoNomb}</td>

                    <td>${correlacion.Central}</td>
                    <td>${correlacion.Famabrev}</td>
                    <td>${correlacion.SListaEquiabrev}</td>
                    <td>${correlacion.SListaEquicodi}</td>

                    <td>
                        <a onclick="popupEdit('${correlacion.PmCindCodi}');">
                            <img src="${siteRoot}Content/Images/btn-edit.png" st style="cursor:pointer" title="Editar el registro" alt="Editar el registro" />
                        </a>
                        <a onclick="eliminarCorrelacion('${correlacion.PmCindCodi}');">
                            <img src="${siteRoot}Content/Images/btn-cancel.png" st style="cursor:pointer" title="Eliminar el registro" alt="Eliminar el registro" />
                        </a>
                    </td>
                    <td>${correlacion.PmCindPorcentaje}</td>
                    <td>${correlacion.PmCindConJuntoEqp}</td>
                    <td>${correlacion.UsuarioMod}</td>
                    <td>${correlacion.FechaModStr}</td>
                </tr>
            `;
        }
    }
    cadena += "</tbody></table>";

    return cadena;
}

//////////////////////////////////////////////////////////////////////////
// Formulario
//////////////////////////////////////////////////////////////////////////
function listarCodigoSddp() {
    var tsppcodi = $("#cbTipoGenerador").val();
    $("#mensaje").hide();

    $.ajax({
        type: 'POST',
        url: controlador + "ListaCodigoSddp",
        data: {
            tsppcodi: tsppcodi,
        },
        cache: false,
        async: true,
        success: function (model) {
            $('#cbGenerador').empty();
            var option = '<option value="" >----- Seleccione  ----- </option>';

            if (model.Resultado != "-1") {
                $.each(model.ListaCodigoSDDP, function (k, v) {
                    option += `<option value ='${v.Sddpcodi}'> ${v.Sddpnum} ${v.Sddpnomb}</option>`;
                })
            } else {
                $("#mensaje").show();
                mostrarMensaje("mensaje", model.Mensaje, $tipoMensajeError, $modoMensajeCuadro);
            }

            $('#cbGenerador').append(option);
        },
        error: function (err) {
            alert("Ha ocurrido un error.");
        }
    });
}

function selectEquipo() {
    var cbTipoEquipo = $("#cbTipoEquipo").val();
    $("#mensaje").hide();

    $.ajax({
        type: 'POST',
        url: controlador + "ListaEquipo",
        data: {
            famcodi: cbTipoEquipo,
        },
        cache: false,
        async: true,
        success: function (model) {
            $('#cbEquipo').empty();
            var option = '<option value="" >----- Seleccione  ----- </option>';

            if (model.Resultado != "-1") {
                $.each(model.ListaEquipo, function (k, v) {
                    option += `<option value ='${v.Equicodi}'>${v.Tareaabrev} ${v.Areanomb} - ${v.Famabrev} - ${v.Equiabrev}</option>`;
                })
            } else {
                $("#mensaje").show();
                mostrarMensaje("mensaje", model.Mensaje, $tipoMensajeError, $modoMensajeCuadro);
            }

            $('#cbEquipo').append(option);
        },
        error: function (err) {
            alert("Ha ocurrido un error.");
        }
    });
}

function grabarCorrelacion() {

    if (!porcentajeValido())
        return;

    var sddpcodi = $("#cbGenerador").val();

    var indisponibilidad = parseFloat($("#txtIndisponibilidad").val()) || 0;
    var equiCodi = $("#cbEquipo").val();

    var txtPmCindCodi = $("#txtPmCindCodi").val();
    var txtAgrupacion = $("#txtAgrupacion").val();

    var PmCindRelInversa = '';
    if ($('#rbPmcindRelinversaS').is(':checked')) {
        PmCindRelInversa = '1';
    }
    if ($('#rbPmcindRelinversaN').is(':checked')) {
        PmCindRelInversa = '0';
    }
    var Grupocodimodo = parseInt($("#cbModoForm").val()) || 0;

    var msjVal = '';
    var fuente = $('input[name=cbFuenteRelacion]:checked').val();
    if (fuente == "1") { //equipo
        Grupocodimodo = -1;
        if (equiCodi <= 0) {
            msjVal +="Debe seleccionar equipo";
        }
    } else {
        equiCodi = -1;
        indisponibilidad = 100;
        if (Grupocodimodo <= 0) {
            msjVal += "Debe seleccionar modo de operación";
        }
    }

    if (msjVal == '') {
        $.ajax({
            type: 'POST',
            url: controlador + "GuardarCorrelacion",
            dataType: 'json',
            data: {
                Sddpcodi: sddpcodi,
                EquiCodi: equiCodi,
                Porcentaje: indisponibilidad,
                Actualizar: actualizar,
                PmCindCodi: txtPmCindCodi,
                PmCindConJuntoEqp: txtAgrupacion,
                PmCindRelInversa: PmCindRelInversa,
                Grupocodimodo: Grupocodimodo
            },
            cache: false,
            async: true,
            success: function (model) {

                if (model.Resultado != "-1") {
                    alert("El registro se guardó correctamente.");
                    cerrarPopUp();
                    //consultarCorrelaciones();
                } else {
                    $("#mensaje").show();
                    mostrarMensaje("mensaje", model.Mensaje, $tipoMensajeError, $modoMensajeCuadro);
                }
            },
            error: function (err) {
                alert("Ha ocurrido un error");
            }
        });
    } else {
        alert(msjVal);
    }
}

function popupEdit(id) {
    actualizar = true;
    $("#mensaje").hide();

    $.ajax({
        type: 'POST',
        url: controlador + "ObtenerCorrelacion",
        data: {
            codigo: id,
        },
        cache: false,
        async: true,
        success: function (model) {

            if (model.Resultado != "-1") {
                var obj = model.Correlacion;

                $("#cbTipoGenerador").val(obj.Tsddpcodi);
                $("#cbTipoEquipo").val(obj.FamCodi);
                $("#txtIndisponibilidad").val(obj.PmCindPorcentaje);
                $("#txtAgrupacion").val(obj.PmCindConJuntoEqp);
                $("#txtPmCindCodi").val(id);

                if (obj.PmCindRelInversa == '1') { $('#rbPmcindRelinversaS').prop('checked', true); $('#rbPmcindRelinversaN').prop('checked', false); }
                if (obj.PmCindRelInversa == '0') { $('#rbPmcindRelinversaS').prop('checked', false); $('#rbPmcindRelinversaN').prop('checked', true); }

                //
                $('#cbGenerador').empty();
                var option = '<option value="" >----- Seleccione  ----- </option>';
                $.each(model.ListaCodigoSDDP, function (k, v) {
                    option += `<option value ='${v.Sddpcodi}'> ${v.Sddpnum} ${v.Sddpnomb}</option>`;
                });
                $('#cbGenerador').append(option);

                //
                $('#cbEquipo').empty();
                var option = '<option value="" >----- Seleccione  ----- </option>';
                $.each(model.ListaEquipo, function (k, v) {
                    option += `<option value ='${v.Equicodi}'>${v.Tareaabrev} ${v.Areanomb} - ${v.Famabrev} - ${v.Equiabrev}</option>`;
                });
                $('#cbEquipo').append(option);

                //
                $("#cbGenerador").val(obj.Sddpcodi);
                $("#cbEquipo").val(obj.EquiCodi);

                //
                $("#cbModoForm").val(obj.Grupocodimodo);

                //
                var tipoFuenteEdit = obj.Grupocodimodo > 0 ? "2": "1";
                $('input:radio[name=cbFuenteRelacion]').val([tipoFuenteEdit]);
                _mostrarDivFuente();

                abrirPopUp();
            } else {
                $("#mensaje").show();
                mostrarMensaje("mensaje", model.Mensaje, $tipoMensajeError, $modoMensajeCuadro);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error.");
        }
    });
}

function eliminarCorrelacion(id) {
    if (confirm("¿Está seguro que desea eliminar la correlación seleccionada?")) {
        $.ajax({
            type: 'POST',
            url: controlador + "EliminarCorrelacion",
            dataType: 'json',
            data: {
                pmCindCodi: id
            },
            cache: false,
            async: true,
            success: function (model) {
                if (model.Resultado != "-1") {
                    alert("El registro se eliminó correctamente.");
                    consultarCorrelaciones();
                } else {
                    $("#mensaje").show();
                    mostrarMensaje("mensaje", model.Mensaje, $tipoMensajeError, $modoMensajeCuadro);
                }
            },
            error: function (err) {
                alert("Ha ocurrido un error.");
            }
        });
    }
}

function abrirPopUpNuevo() {
    $("#cbTipoGenerador").val("-1");
    $("#cbTipoEquipo").val("-1");
    $("#txtIndisponibilidad").val("");
    $("#cbGenerador").val("-1");
    $("#cbEquipo").val("-1");
    $("#txtAgrupacion").val("1");
    $("#rbPmcindRelinversaS").prop('checked', false);
    $("#rbPmcindRelinversaN").prop('checked', true);
    $("#cbModoForm").val("1");
    actualizar = false;

    abrirPopUp();
}

function abrirPopUp() {
    $('#popupPeriodo').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
}

function cerrarPopUp() {
    $('#popupPeriodo').bPopup().close();
}

function porcentajeValido() {
    var value = $("#txtIndisponibilidad").val();
    var x = parseFloat(value);
    if (isNaN(x) || x >= 0 && x <= 100) {
        return true;
    } else {
        document.getElementById("lblText").style.display = 'block';
        document.getElementById("txtIndisponibilidad").setAttribute("style", "border-color: red");
        return false;
    }

}
