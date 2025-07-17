var controlador = siteRoot + 'YupanaContinuo/CondicionTermica/';

var UTILIZA_X_SI = 1;
var EMPRESA_DEFAULT_SELECT = -3;
var ID_CENTRAL_DEFAULT = -2;
var ID_CENTRAL_DEFAULT_SELECT = -3;
var TIPO_HO_MODO = 100;

var TIPO_CENTRAL_HIDROELECTRICA = 4;
var TIPO_CENTRAL_TERMOELECTRICA = 5;
var TIPO_CENTRAL_SOLAR = 37;
var TIPO_CENTRAL_EOLICA = 39;

var APP_OPCION = -1;
var OPCION_COPIAR = 1;
var OPCION_EDITAR = 2;
var OPCION_NUEVO = 3;
var OPCION_ELIMINAR = 4;
var OPCION_VER = 5;

var OBJ_DATA_CONDICION_TERMICA = {
    TipoVistaCoordinador: parseInt(UTILIZA_X_SI),

    IdPos: -1,

    IdEmpresa: EMPRESA_DEFAULT_SELECT,
    IdTipoCentral: 0,
    IdCentralSelect: -3,
    IdEquipoOrIdModo: -1,
    IdEquipo: 0,
    TipoModOp: TIPO_HO_MODO,

    FechaIni: '',
    HoraIni: '',
    FechaFin: '',
    HoraFin: ''
};

$(function () {
    $('#tab-container').easytabs({
        animate: false
    });

    //$('#tab-container').easytabs('select', '#vistaGrafico');

    $('#txtFecha').Zebra_DatePicker({
        format: 'd/m/Y',
        onSelect: function () {
            cargarVista();
        }
    });
    $('#cbHoras').change(function () {
        cargarVista();
    });
    $("#btnConsultar").click(function () {
        cargarVista();
    });

    $("#btnActualizar").click(function () {
        actualizarAutomatico();
    });

    $('#btnVerEnvios').click(function () {
        popUpListaEnvios();
    });
    $("#btnAgregarCondicionTermica").click(function () {
        vistaDetalle();
    });

    cargarVista();
});

function padDigits(number, digits) {
    return Array(Math.max(digits - String(number).length + 1, 0)).join(0) + number;
}

function vistaDetalle() {
    $('#tab-container').easytabs('select', '#vistaDetalle');

    var strObjForm = JSON.stringify(OBJ_DATA_CONDICION_TERMICA);
    var objForm = JSON.parse(strObjForm);
    view_Formulario(objForm);
}

function view_Formulario(obj) {
    obj.TipoVistaCoordinador = parseInt(UTILIZA_X_SI);
    obj.IdTipoOperSelect = obj.IdTipoOperSelect == null ? -1 : obj.IdTipoOperSelect;

    obj.IdEmpresa = obj.IdEmpresa != undefined ? obj.IdEmpresa : $('#cbEmpresa').val();
    obj.IdEmpresa = parseInt(obj.IdEmpresa) > 0 ? parseInt(obj.IdEmpresa) : EMPRESA_DEFAULT_SELECT;

    obj.IdTipoCentral = parseInt($('#cbTipoCentral').val()) || 0;

    obj.IdCentralSelect = obj.IdCentralSelect != undefined ? obj.IdCentralSelect : $('#cbCentral').val();
    if (obj.IdCentralSelect == null)
        obj.IdCentralSelect = -1;

    obj.Fecha = $('#txtFecha').val();

    obj.IdMotOpForzadaSelect = obj.IdMotOpForzadaSelect != undefined ? (parseInt(obj.IdMotOpForzadaSelect) || -1) : -1;

    obj.UsuarioModificacion = obj.UsuarioModificacion != undefined ? obj.UsuarioModificacion : "";
    obj.FechaModificacion = obj.FechaModificacion != undefined ? obj.FechaModificacion : "";


    $.ajax({
        type: 'POST',
        traditional: true,
        async: false,
        url: controlador + 'ViewIngresoPeriodoForzado',
        data: {
            objJsonForm: JSON.stringify(obj)
        },
        success: function (result) {
            view_HtmlInterfaz(obj, result);
        },
        error: function (err) {
            alert('Ha ocurrido un error al generar vista');
        }
    });
}

function view_HtmlInterfaz(obj, result) {

    $('#detalle').html(result);

    view_ConfInicialRegistro();

    if (obj.IdPos != -1) { //modificacion

        $('#listado_grupos_modo_view').show();
    } else {
        //pantalla nuevo registro
        $('#cbEmpresa2').removeAttr("disabled");
        $('#cbCentral2').removeAttr("disabled");

    }

    $('#cbEmpresa2').val(obj.IdEmpresa);
    $("#hfCodigo").val("");
    //}
}

function view_ConfInicialRegistro() {
    var tipoCentral = $('#hfTipoCentral').val();

    $('#cbCentral2').val($('#hfCentral').val());
    $('#cbTipoOp').val($('#hfTipoOerac').val());
    $('#cbMotOpForzada').val($('#hfMotOpForzada').val());

    $("#btnAceptar").click(function () {

        var obj = {
            Cpfzdtcodi: $("#hfCodigo").val(),
            Emprcodi: $("#cbEmpresa2").val(),
            Equicodi: $("#cbCentral2").val(),
            Grupocodi: $("#cbModoOpGrupo").val(),
            Cpfzdtperiodoini: $("#cbPeriodoinicio").val(),
            Cpfzdtperiodofin: $("#cbPeriodofin").val(),
        };
        var msj = "";

        if (obj.Emprcodi == '-3') msj += " Seleccionar Empresa";
        if (obj.Equicodi == '-3') msj += ", Seleccionar Central";
        if (obj.Grupocodi == '-3') msj += ", Seleccionar Modo";

        if (+obj.Cpfzdtperiodoini > +obj.Cpfzdtperiodofin) {
            if (msj !== "") msj += ", ";
            msj += "El periodo inicio debe ser menor a periodo fin";
        }

        if (msj != "") {
            alert(msj);
            return;
        }

        var fecha = $("#txtFecha").val();
        var hora = parseInt($("#cbHoras").val()) || 0;

        $.ajax({
            type: 'POST',
            url: controlador + 'MantenerCondicionTermico',
            traditional: true,
            contentType: "application/json",
            data: JSON.stringify({
                cpForzadoDet: obj,
                fecha: fecha,
                hora: hora,
            }),
            dataType: "json",
            success: function (result) {
                if (result.Resultado == "1") {
                    alert('El registro se guardó correctamente');
                    cargarVista();
                }
                else {

                    alert('Error : ' + result.Mensaje);
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error al generar vista');
            }
        });

    });



    $('#cbEmpresa2').change(function () {
        //view_EnabledOrDisabledUIHoraOperacion();
        cargarCentral2();
    });

    $('#cbCentral2').change(function () {
        cargarListaModo_Grupo($('#hfTipoCentral').val(), $('#hfIdModoGrupo').val(), -1);
    });

    cargarListaModo_Grupo($('#hfTipoCentral').val(), $('#hfIdModoGrupo').val(), $('#hfIdPos').val());
};

function cargarCentral2() {
    $('.unidades_modo').hide();
    $('#unidades_especiales').html('');

    $('#cbCentral2').empty();
    $('#cbCentral2').append('<option value="' + $("#hfCentral").val() + '">--SELECCIONE--</option>');

    $('#cbModoOpGrupo').empty();
    $('#cbModoOpGrupo').append('<option value="' + $("#hfCentral").val() + '">--SELECCIONE--</option>');

    var idTipoCentral = $('#cbTipoCentral').val();
    var idEmpresa = $('#cbEmpresa2').val();
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarCentral2',
        dataType: 'json',
        async: false,
        data: {
            tipoCentral: idTipoCentral,
            idEmpresa: idEmpresa
        },
        success: function (evt) {
            $('#cbCentral2').empty();
            $('#cbCentral2').append('<option value="' + $("#hfCentral").val() + '">--SELECCIONE--</option>');
            var listaCentral = evt.ListaCentrales;
            for (var i = 0; i < listaCentral.length; i++) {
                $('#cbCentral2').append('<option value=' + listaCentral[i].Equicodi + '>' + listaCentral[i].Equinomb + '</option>');
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarListaModo_Grupo(tipoCentral, modoGrupoSelect, pos) {
    var idCentral = $('#cbCentral2').val();
    var idEmpresa = $("#cbEmpresa2").val() != undefined ? $("#cbEmpresa2").val() : $('#cbEmpresa').val();
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaGrupoModo',
        async: false,
        data: {
            idCentral: idCentral,
            idEmpresa: idEmpresa,
            idTipoCentral: tipoCentral,
            modoGrupoSelect: modoGrupoSelect,
            pos: pos,
            viewCoordinador: 1
        },

        success: function (aData) {
            $('#listaModoGrupo').html(aData);

            $(".unidades_modo").hide();

            $('#cbModoOpGrupo').val($('#hfIdModGrup').val());

        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarVista() {

    $('#tab-container').easytabs('select', '#vistaGrafico');

    var fecha = $("#txtFecha").val();
    var hora = parseInt($("#cbHoras").val()) || 0;

    $('#resultado').html('');
    $("#grafico").hide();
    $("#tblInfo,#tblInfoEnvio").hide();
    $("#barraHerramientaYC").hide();

    var anchoTabPrincipal = $("#tab-container").width() - 50;

    $.ajax({
        type: 'POST',
        url: controlador + 'GraficarCondicionTermica',
        data: {
            fecha: fecha,
            hora: hora,
        },
        success: function (result) {
            if (result.Resultado === "-1") {
                alert("Error: " + result.Mensaje);

            } else {
                if (result.ForzadoCab) {
                    $("#barraHerramientaYC").show();
                    $("#tblInfoEnvio").show();
                    $("#txtCodigoEnvio").text(result.ForzadoCab.Cpfzcodi);
                    $("#txtFechaEnvio").text(result.ForzadoCab.CpfzfecregistroDesc);
                } else {
                    $("#tblInfo").show();
                }

                $("#grafico").css('width', anchoTabPrincipal + 'px');
                $('#resultado').html(result.HtmlList);
                $("#grafico").show();

                tablaContextMenu();

                $("#tblCondicionTermico").dataTable({
                    scrollY: "100%",
                    scrollX: true,
                    scrollCollapse: false,
                    sDom: 't',
                    ordering: false,
                    paging: false,
                    "bAutoWidth": false,
                    "destroy": "true",
                    fixedColumns: {
                        leftColumns: 3
                    },
                    stripeClasses: []
                });
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error al generar vista');
        }
    });
}

function tablaContextMenu() {

    var objItems = {
        "modificar": { name: "MODIFICAR" },
        "eliminar": { name: "ELIMINAR" }
    };
    $('#tblCondicionTermico').contextMenu({
        selector: '.context-menu-one',
        callback: function (key, options) {
            var listaVariable = ($(this).attr('id')).split('_');

            var cpfzdtcodi = listaVariable[1];
            var iEmprcodi = listaVariable[2];
            var iEquicodi = listaVariable[3];
            var iGrupocodi = listaVariable[4];
            var iBloqueIni = listaVariable[5];
            var iBloqueFin = listaVariable[6];

            if (key == "modificar") {
                vistaDetalle();

                $("#hfCodigo").val(cpfzdtcodi);

                $("#cbEmpresa2").val(iEmprcodi);
                $("#cbEmpresa2").trigger("change");

                $("#cbCentral2").val(iEquicodi);
                $("#cbCentral2").trigger("change");

                $("#cbModoOpGrupo").val(iGrupocodi);

                $('#cbEmpresa2,#cbCentral2,#cbModoOpGrupo').prop('disabled', true);

                $("#cbPeriodoinicio").val(iBloqueIni);
                $("#cbPeriodofin").val(iBloqueFin);

            }
            if (key == "eliminar") {
                if (confirm('¿Realmente desea eliminar?')) {

                    eliminarCondicionTermica(cpfzdtcodi);
                }
            }
        },
        items: objItems
    });

    var objItems2 = {
        "nuevo": { name: "NUEVO" },
    };
    $('#tblCondicionTermico').contextMenu({
        selector: '.context-menu-one-nuevo',
        callback: function (key, options) {
            var listaVariable = ($(this).attr('id')).split('_');

            var cpfzdtcodi = listaVariable[1];
            var iEmprcodi = listaVariable[2];
            var iEquicodi = listaVariable[3];
            var iGrupocodi = listaVariable[4];
            var iBloqueIni = listaVariable[5];
            var iBloqueFin = listaVariable[6];

            if (key == "nuevo") {
                vistaDetalle();

                $("#hfCodigo").val(cpfzdtcodi);

                $("#cbEmpresa2").val(iEmprcodi);
                $("#cbEmpresa2").trigger("change");

                $("#cbCentral2").val(iEquicodi);
                $("#cbCentral2").trigger("change");

                $("#cbModoOpGrupo").val(iGrupocodi);

                $('#cbEmpresa2,#cbCentral2,#cbModoOpGrupo').prop('disabled', true);

                $("#cbPeriodoinicio").val(iBloqueIni);
                $("#cbPeriodofin").val(iBloqueFin);

            }
        },
        items: objItems2
    });
}

function eliminarCondicionTermica(id) {

    var obj = {
        Cpfzdtcodi: id
    };

    var fecha = $("#txtFecha").val();
    var hora = parseInt($("#cbHoras").val()) || 0;

    $.ajax({
        type: 'POST',
        traditional: true,
        url: controlador + 'EliminarCondicionTermica',
        contentType: "application/json",
        data: JSON.stringify({
            cpForzadoDet: obj,
            fecha: fecha,
            hora: hora,
        }),
        dataType: "json",
        success: function (result) {
            if (result.Resultado == "1") {
                alert('El registro se eliminó correctamente');
                cargarVista();
            }
            else {

                alert('Error : ' + result.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error al generar vista');
        }
    });
}

function obtenerDetallePorId(cpfzdtcodi) {
    var obj;
    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerDetallePorId',
        dataType: 'json',
        async: false,
        data: {
            cpfzdtcodi: cpfzdtcodi
        },
        success: function (evt) {
            obj = evt.ForzadoDet;
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
    return obj;
}

function popUpListaEnvios() {

    var fecha = $("#txtFecha").val();
    var hora = parseInt($("#cbHoras").val()) || 0;

    $.ajax({
        type: 'POST',
        traditional: true,
        url: controlador + 'ReporteHtmlEnvios',
        data: {
            fecha: fecha,
            hora: hora,
        },
        success: function (result) {
            $('#idEnviosAnteriores').html(result.HtmlList);

            setTimeout(function () {
                $('#enviosanteriores').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown',
                    modalClose: false
                });

                $('#tablaenvio').dataTable({
                    "scrollY": 330,
                    "scrollX": true,
                    "sDom": 't',
                    "ordering": false,
                    "bPaginate": false,
                    "iDisplayLength": -1
                });
            }, 50);

        },
        error: function (err) {
            alert('Ha ocurrido un error al generar vista');
        }
    });
}

function visualizarCondicionTermica(codigo, fecha) {

    $('#resultado').html('');
    $("#grafico").hide();
    $("#tblInfo,#tblInfoEnvio").hide();
    var anchoTabPrincipal = $("#tab-container").width() - 50;

    $.ajax({
        type: 'POST',
        url: controlador + 'GraficarCondicionTermicaPorCodigo',
        data: {
            cpfzcodi: codigo
        },
        success: function (result) {
            $("#grafico").css('width', anchoTabPrincipal + 'px');
            $('#resultado').html(result.HtmlList);
            $("#grafico").show();

            tablaContextMenu();

            $("#tblCondicionTermico").dataTable({
                scrollY: "100%",
                scrollX: true,
                scrollCollapse: false,
                sDom: 't',
                ordering: false,
                paging: false,
                "bAutoWidth": false,
                "destroy": "true",
                fixedColumns: {
                    leftColumns: 3
                },
                stripeClasses: []
            });

            $('#enviosanteriores').bPopup().close();

            $("#tblInfoEnvio").show();
            $("#txtCodigoEnvio").text(codigo);
            $("#txtFechaEnvio").text(fecha);

        },
        error: function (err) {
            alert('Ha ocurrido un error al generar vista');
        }
    });
}


function actualizarAutomatico() {

    var fecha = $("#txtFecha").val();
    var hora = parseInt($("#cbHoras").val()) || 0;

    $.ajax({
        type: 'POST',
        traditional: true,
        url: controlador + 'CondicionTermicaAutomatizado',
        data: {
            fecha: fecha,
            hora: hora,
        },
        success: function (result) {
            if (result.Resultado == "1") {
                alert('El proceso se ejecutó correctamente');
                cargarVista();
            }
            else {

                alert('Error : ' + result.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error al generar vista');
        }
    });

}