var controlador = siteRoot + 'Monitoreo/Indicadores/';
var ancho = 900;
var idGlobal = 0;

$(function () {
    var fecha = $('#hfMes').val();

    $('#txtMesPeriodo').Zebra_DatePicker({
        format: 'm Y',
        direction: fecha
    });

   
    $('#txtMes').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () {
            consultarGenerador();
        },
        direction: fecha
    });

    ancho = $("#mainLayout").width() > 900 ? $("#mainLayout").width() - 30 : 900;
    $('#btnGenerar').click(function () {
        mostrarView();
    });
    $('#btnSave').click(function () {
        saveGenerador();
    });
    $('#btnConsultar').click(function () {
        consultarGenerador();
    });
    $('#btnUpdate').click(function () {
        var motivo = $('#txtMotivoEdit').val();
        if (motivo != "") {
            var estado = $('#cboEstado').val();
            var resultado;
            var portal = document.getElementById("chkPortal");
            var motivo = $('#txtMotivoEdit').val();
            if (portal.checked == true) {
                resultado = 1;
            } else {
                resultado = 0;
            }
            editGeneracion(idGlobal, estado, resultado, motivo)
        }
        else {
            alert("No puede dejar la descripcion del motivo vacia");
        }
    });
    setInterval(function () { consultarGenerador(); }, 30000);

    consultarGenerador();
});

function mostrarView() {
    $("#txtMotivo").val('');
    $("#txtMesPeriodo").val($("#txtMes").val());
    setTimeout(function () {
        $('#idPopupNew').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    }, 50);
}

function saveGenerador() {
    var valorMotivo = $('#txtMotivo').val();
    var mes = $('#txtMesPeriodo').val();
    if (valorMotivo != "") {
        $.ajax({
            type: 'POST',
            async: true,
            url: controlador + 'SaveGenerador',
            data: {
                motivo: valorMotivo,
                mes: mes
            },
            success: function (result) {
                if (result.Resultado4 == 0) {
                    alert("No se creo la  version, Existe una generacion en proceso....");
                    cerrarPopupNew();
                    consultarGenerador();
                    $("#btnGenerar").css("display", "none");
                } else if (result.Resultado4 === -1) {
                    alert("Ocurrio un error en la generación de la versión");
                    cerrarPopupNew()
                }
                else {
                    cerrarPopupNew();
                    consultarGenerador();
                }
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    } else {
        alert("Debe escribir un motivo para poder hacer la generación");
    }
}

function consultarGenerador() {

    var periodo = $('#txtMes').val();
    $.ajax({
        type: 'POST',
        async: true,
        url: controlador + 'ConsultarGenerador',
        data: {
            fecha: periodo,
        },
        success: function (aData) {
            if (aData.Resultado == '-1') {
                alert("Ha ocurrido un error");
            }
            else {
                $('#listado').html(aData.Resultado);
                var anchoReporte = $('#listado').width();
                $("#listado").css("width", (anchoReporte > ancho ? ancho : anchoReporte) + "px");
                $('#reporte').dataTable({
                    "scrollX": true,
                    "scrollY": "400px",
                    "sDom": 't',
                    "ordering": false,
                    paging: false
                });
            }

            if (aData.Resultado4 == '0') {
                $("#btnGenerar").css("display", "none");
            }
            else {
                $("#btnGenerar").css("display", "block");
            }
        }
            ,
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function editGeneracion(id, estado, portal, motivo) {
    var periodo = $('#txtMotivo').val();
    $.ajax({
        type: 'POST',
        async: true,
        url: controlador + 'EditGeneracion',
        data: {
            id: id,
            estado: estado,
            portal: portal,
            motivo: motivo
        },
        success: function (result) {
            if (result.Resultado == '-1') {
                alert(result.Mensaje);
            } else {
                alert("Se guardo correctamente los cambios");
                cerrarPopupEdit();
                consultarGenerador();
            }
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function editarGeneracion(id) {
    formularioGeneracion(id);
}

function visualizarGeneracion(dato) {
    window.location.href = controlador + "reporte?id=" + dato;
}

function formularioGeneracion(id) {
    idGlobal = id;
    $.ajax({
        type: 'POST',
        url: controlador + "PopupEditarGenerador",
        data: {
            id: id
        },
        success: function (evt) {
            if (evt.Generador.Vermmestado == 1) {
                $('#cboEstado').val(evt.Generador.Vermmestado);
            }
            else if (evt.Generador.Vermmestado == 2) {
                $('#cboEstado').val(evt.Generador.Vermmestado);
            }
            else {
                $('#cboEstado').val(evt.Generador.Vermmestado);
            }
            if (evt.Generador.Vermmmotivoportal == 1) {

            } else {
                $('#chkPortal').attr('checked', false);
            }
            $("#txtMotivoEdit").val(evt.Generador.Vermmmotivo);
            setTimeout(function () {
                $('#EditPopupGenerador').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function cerrarPopupNew() {
    $('#idPopupNew').bPopup().close();
}

function cerrarPopupEdit() {
    $('#EditPopupGenerador').bPopup().close();
}

function DescargarExcelVersion(mesPeriodo) {
    window.location = controlador + "ExportarReporteIndicadores?nombreArchivo=" + mesPeriodo;
}