var controlador = siteRoot + 'Intervenciones/FactorF1F2/';
var ANCHO_LISTADO_EMS = 1200;

var IMG_VER_NUMERAL = '<img src="' + siteRoot + 'Content/Images/btn-open.png" alt="" width="19" height="19" style="">';
var IMG_PROGRAMADO = `<img src="${siteRoot}Content/Images/tipo_programado.png" alt="" width="19" height="19" style="">`;
var IMG_EJECUTADO = `<img src="${siteRoot}Content/Images/tipo_ejecutado.png" alt="" width="19" height="19" style="">`;
var IMG_DESCARGA_EXCEL = '<img src="' + siteRoot + 'Content/Images/ExportExcel.png" alt="" width="19" height="19" style="">';
var IMG_DESCARGA_ZIP = '<img src="' + siteRoot + 'Content/Images/Document/raricon.png" alt="" width="19" height="19" style="">';
var IMG_AGRUPAR = '<img src="' + siteRoot + 'Content/Images/Agrupar.png" alt="" width="19" height="19" style="">';

$(document).ready(function () {
    $('#mes').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () {
            buscarCambios();
        }
    });

    buscarCambios();
});

$(function () {
    $('#btnConsultar').click(function () {
        buscarCambios();
    });

    $('#btnGuardarVersion').click(function () {
        versionGuardar();
    });

    $('#btnCancelar').click(function () {
        $('#idPopupVersion').bPopup().close();
    });

    $('#btnPopupNuevaVersion').click(function () {
        $('#periodo').val($('#mes').val());
        setTimeout(function () {
            $('#idPopupVersion').bPopup({
                easing: 'easeOutBack',
                speed: 450,
                transition: 'slideDown',
                modalClose: false
            });
        }, 50);
    });

    ANCHO_LISTADO_EMS = ANCHO_LISTADO_EMS > 1200 ? ANCHO_LISTADO_EMS : 1200;
    $(window).resize(function () {
        $('#listado').css("width", ANCHO_LISTADO_EMS + "px");
    });
});

buscarCambios = function () {
    $('#mensaje').css("display", "none");
    versionListado()
};
mostrarExitoOperacion = function () {
    $('#mensaje').removeClass("action-error");
    $('#mensaje').addClass("action-exito");
    $('#mensaje').text("La operación se realizó con exito...!");
    $('#mensaje').css("display", "block");
};
mostrarError = function () {
    $('#mensaje').removeClass("action-exito");
    $('#mensaje').addClass("action-error");
    $('#mensaje').text("Ha ocurrido un error");
    $('#mensaje').css("display", "block");
};
ocultarMensaje = function () {
    $('#mensaje').css("display", "none");
};
mostrarListado = function (nroPagina) {
    $('#hfNroPagina').val(nroPagina);
    $.ajax({
        type: 'POST',
        url: controlador + "ListadoVersion",
        data: $('#frmBusquedaCambios').serialize(),
        success: function (evt) {
            //$('#listado').css("width", $('#mainLayout').width() + "px");

            $('#listado').html(evt);
            $('#tabla').dataTable({
                "scrollY": 430,
                "scrollX": false,
                "sDom": 'ft',
                "ordering": false,
                "iDisplayLength": -1
            });
        },
        error: function () {
            mostrarError();
        }
    });
};

function versionListado() {
    $("#mensaje").hide();

    var fechaPeriodo = $('#mes').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'ListadoVersion',
        dataType: 'json',
        data: {
            fechaPeriodo: fechaPeriodo
        },
        cache: false,
        success: function (data) {
            if (data.Resultado != "-1") {

                var html = dibujarTablaListadoVersion(data.ListInFactorVersion);
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

            } else {
                alert("Ha ocurrido un error: " + data.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function versionGuardar() {
    //datos
    var fechaPeriodo = $('#periodo').val();
    var estado = $("input[name='rbVersion']:checked").val();

    if (estado != "") {
        $.ajax({
            type: 'POST',
            async: true,
            url: controlador + 'GuardarNuevaVersion',
            data: {
                estado: estado,
                fechaPeriodo: fechaPeriodo
            },
            success: function (result) {
                if (result.Resultado == "-1") {
                    alert(result.Mensaje);
                } else {
                    versionListado();
                    $('#idPopupVersion').bPopup().close();
                }
            },
            error: function (err) {
                alert("Ha ocurrido un error");
            }
        });
    } else {
        alert("Datos incorrectos");
    }
}

function dibujarTablaListadoVersion(lista) {

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono dataTable" cellspacing="0" width="100%" id="tblListado">
        <thead>
            <th style='width: 70px'>Opciones</th>
            <th style='width: 70px'>N° Versión</th>
            <th style='width: 70px'>Fecha Periodo</th>
            <th style='width: 70px'>Tipo de Equipo</th>
            <th style='width: 70px'>Disponibilidad</th>
            <th style='width: 70px'>Estado</th>
            <th style='width: 70px'>Usuario de creación</th>
            <th style='width: 70px'>Fecha de creación</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in lista) {
        var item = lista[key];

        var tdAgrup = "";
        if (item.Infverflagfinal == "N") {
            tdAgrup = `
                    <a title="Agrupar mantenimientos" href="javascript:agruparMMayorXVersion(${item.Infvercodi});">${IMG_AGRUPAR} </a>
            `;
        }

        var tdZip = "";
        if (item.Infverflagfinal == "S") {
            tdZip = `
                    <a title="Descargar sustento" href="javascript:exportarZip(${item.Infvercodi});">${IMG_DESCARGA_ZIP} </a>
            `;
        }

        cadena += `
            <tr>
                <td style="height: 24px;">
                    <a title="Ver Detalles en Consultas Cruzadas" href="javascript:verConsultasCruzadasF1F2(${item.Infvercodi});">${IMG_VER_NUMERAL} </a>
                    <a title="Mantenimiento Programado" href="javascript:verMantProgramado(${item.Infvercodi});">${IMG_PROGRAMADO} </a>
                    <a title="Mantenimiento Ejecutado" href="javascript:verMantEjecutado(${item.Infvercodi});">${IMG_EJECUTADO} </a>
                    ${tdAgrup}
                    ${tdZip}
                    <a title="Descargar Excel" href="javascript:exportarReporte(${item.Infvercodi}, ${item.Infvernro}, ${item.Mes}, ${item.Anio});">${IMG_DESCARGA_EXCEL} </a>
                </td>
                <td style="height: 24px;text-align: center; ">${item.Infvernro}</td>
                <td style="height: 24px;text-align: center; ">${item.InfverfechaperiodoDesc}</td>
                <td style="height: 24px;">${item.InfvertipoeqDesc}</td>
                <td style="height: 24px;">${item.InfverdispDesc}</td>
                <td style="height: 24px;">${item.InfverflagfinalDesc}</td>
                <td style="height: 24px;text-align: center; ">${item.Infverusucreacion}</td>
                <td style="height: 24px;text-align: center; ">${item.InfverfeccreacionDesc}</td>
            </tr>
        `;
    }
    cadena += "</tbody></table>";

    return cadena;
}

function verConsultasCruzadasF1F2(id) {
    window.location.href = controlador + "ConsultasCruzadasF1F2?infvercodi=" + id;
}

function verMantProgramado(id) {
    window.location.href = controlador + "IndexProgramado?versionDesc=" + $("#mes").val().replace(" ", "") + "&infvercodi=" + id;
}

function verMantEjecutado(id) {
    window.location.href = controlador + "IndexEjecutado?versionDesc=" + $("#mes").val().replace(" ", "") + "&infvercodi=" + id;
}

function exportarReporte(id, idVersion, mes, anio) {
    $.ajax({
        type: 'POST',
        url: controlador + "GenerarArchivoExcelReporte",
        data: {
            infvercodi: id,
            version: $("#mes").val().replace(" ", ""),
            idVersion: idVersion,
            mes: mes,
            anio: anio
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


function exportarZip(id) {
    $.ajax({
        type: 'POST',
        url: controlador + "ExportarFormatoZip",
        data: {
            infvercodi: id,
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

function agruparMMayorXVersion(id) {
    if (confirm('¿Desea generar una nueva versión Final que agrupe los mantenimientos editados?')) {
        $.ajax({
            type: 'POST',
            url: controlador + "AgruparMmayorXVersion",
            data: {
                infvercodi: id,
            },
            success: function (evt) {
                if (evt.Resultado == "-1") {
                    alert(evt.Mensaje);
                } else {
                    alert("La operación se realizó correctamente.");
                    versionListado();
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error.');
            }
        });
    }
}