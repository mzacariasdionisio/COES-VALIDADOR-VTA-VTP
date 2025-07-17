var controlador = siteRoot + 'Equipamiento/ParametrosTecnicos/';
var controladorEq = siteRoot + 'Equipamiento/Equipo/';

$(function () {
    $("#btnConsultar").click(function () {
        Consultar();
    });

    $('#cbTipoEmpresa').change(function () {
        $('#cbEmpresa').val("-1");
        cargarEmpresas();
        CargarFamilias();
    });

    $("#cbEmpresa").change(function () {
        CargarFamilias();
    });

    //ObtenerListado();
});

cargarEmpresas = function () {
    $.ajax({
        type: 'POST',
        url: controlador + '/CargarEmpresas',
        dataType: 'json',
        data: { idTipoEmpresa: $('#cbTipoEmpresa').val() },
        cache: false,
        success: function (aData) {
            $('#cbEmpresa').get(0).options.length = 0;
            $('#cbEmpresa').get(0).options[0] = new Option("TODOS", "-1");
            $.each(aData, function (i, item) {
                $('#cbEmpresa').get(0).options[$('#cbEmpresa').get(0).options.length] = new Option(item.Text, item.Value);
            });
        },
        error: function () {
            mostrarError();
        }
    });
};

CargarFamilias = function () {
    $.ajax({
        type: 'POST',
        url: controlador + '/CargarFamilias',
        dataType: 'json',
        data: { idEmpresa: $('#cbEmpresa').val() },
        cache: false,
        success: function (aData) {
            $('#cbFamilia').get(0).options.length = 0;
            $('#cbFamilia').get(0).options[0] = new Option("TODOS", "-1");
            $.each(aData, function (i, item) {
                $('#cbFamilia').get(0).options[$('#cbFamilia').get(0).options.length] = new Option(item.Text, item.Value);
            });
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
};

function ObtenerListado() {
    var miDataM = getObjFiltro();
    $("#listado").html('');

    $.ajax({
        type: 'POST',
        url: controlador + 'ListadoEqPropSinValor',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(miDataM),
        success: function (eData) {
            $('#listado').css("width", $('.form-main').width() + "px");

            $('#listado').html(eData);
            var t =$('#tabla').DataTable({
                bJQueryUI: true,
                "scrollY": 500, // tamaño listado height
                "scrollX": true,
                "sDom": 'ft',
                "ordering": true,
                "iDisplayLength": -1,
                "targets": 0
            });

            t.on('order.dt search.dt', function () {
                t.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                    cell.innerHTML = i + 1;
                });
            }).draw();
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function getObjFiltro() {
    var cbTipoEmpresa = $("#cbTipoEmpresa").val();
    var cboEmpresa = $("#cbEmpresa").val();
    var cboFamilia = $("#cbFamilia").val();
    var cboEstado = $("#cbEstado").val();

    var miDataM = {
        TipoEmpresa: cbTipoEmpresa,
        Emprcodi: cboEmpresa,
        Famcodi: cboFamilia,
        Equiestado: cboEstado
    };
    return miDataM;
}

function Consultar() {
    ObtenerListado();
}

function VerPropiedadesValidas(emprCodi, equicodi, famCodi) {
    abrirPopupPropiedades(emprCodi, equicodi, famCodi);
}

function abrirPopupPropiedades(emprCodi, equicodi, famCodi) {
    $.ajax({
        type: 'POST',
        url: controlador + "VerPropiedadesValidas",
        data: {
            codEmpresa: emprCodi,
            codEquipo: equicodi,
            codFamilia: famCodi
        },
        success: function (evt) {
            $('#contenidoDetalle').html(evt);

            var excep_resultado = $("#hdResultado_ED").val();
            var excep_mensaje = $("#hdMensaje_ED").val();

            if (excep_resultado !== "-1") {

                $('#tablaListadoDetalle').dataTable({
                    "destroy": "true",
                    "sPaginationType": "full_numbers",
                    "ordering": true,
                    "searching": true,
                    "paging": true,
                    "info": true,
                    "iDisplayLength": 15
                });

                setTimeout(function () {
                    $('#popupPropiedadesValidas').bPopup({
                        autoClose: false,
                        easing: 'easeOutBack',
                        speed: 450,
                        transition: 'slideDown'
                    });
                }, 500);
            } else {
                $('#contenidoDetalle').html('');
                alert(excep_mensaje);
            }
        },
        error: function (err) {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

function VerPropiedadesEquip(idEquipo) {
    var DireccionEquipProp = controladorEq + 'EquipoPropiedadesSheet?iEquipo=' + idEquipo;
    window.open(DireccionEquipProp, '_blank');
}