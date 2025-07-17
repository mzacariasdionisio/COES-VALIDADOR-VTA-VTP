var controlador = siteRoot + 'ReportesMedicion/formatoreporte/';
var oTable;

$(function () {
    $("#btnOcultarMenu").click();

    $('#btnBuscar').click(function () {
        mostrarListaPto();
    });
    $('#btnExportar').click(function () {
        exportarReporte();
    });
    $('#btnPuntoCalculado').click(function () {
        nuevoPtoCalculado();
    });
    $('#btnActualizar').click(function () {
        editarPtoCal();
    });

    mostrarListaPto();
});

function mostrarListaPto() {
    var reporte = -1;
    $.ajax({
        type: 'POST',
        url: controlador + "ListaPtoMedicionCal",
        data: {
            reporte: reporte
        },
        success: function (evt) {
            $('#listpto').html(evt);
            oTable = $('#tablaPtos').dataTable({
                "bJQueryUI": true,
                "scrollY": 550,
                "scrollX": true,
                "sDom": 'ftp',
                "bPaginate": true,
                "ordering": true,
                "order": [[0, "desc"]],
                "iDisplayLength": 20,
                "sPaginationType": "full_numbers"
            });
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function exportarReporte() {
    $.ajax({
        type: 'POST',
        url: controlador + 'ExportarExcelReportePtoCalculado',
        data: {
        },
        success: function (evt) {
            if (evt.Error == undefined) {
                window.location.href = controlador + 'DescargarExcelReporte?archivo=' + evt[0] + '&nombre=' + evt[1];
            }
            else {
                alert("Error:" + evt.Descripcion);
            }
        },
        error: function (result) {
            alert('ha ocurrido un error al descargar el archivo excel. ' + result.status + ' - ' + result.statusText + '.');
        }
    });
}

function modificarPto(reporcodi, tipoinfocodi, ptomedicodi, activo) {
    $('#hfPunto').val(ptomedicodi);
    $('#hfTipoinfo').val(tipoinfocodi);
    if (activo == 1) {
        $('#idActivo').prop('checked', true);
    } else {
        $('#idActivo').prop('checked', false);
    }

    setTimeout(function () {
        $('#popupmpto').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown'
        });
    }, 50);
}

function modificarPtoCal(ptomedicodi, activo, barranomb, elenomb, descripcion) {
    $("#txtModifPtomedicodi").html(ptomedicodi);
    $("#txtModifPtomedibarranomb").val(barranomb);
    $("#txtModifPtomedielenomb").val(elenomb);
    $("#txtModifDescripcion").val(descripcion);

    $('#hfPunto').val(ptomedicodi);
    if (activo == 'A') {
        $('#idActivo').prop('checked', true);
    } else {
        $('#idActivo').prop('checked', false);
    }

    setTimeout(function () {
        $('#popupmpto').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown'
        });
    }, 50);
}

function editarPtoCal() {
    var ptomedicodi = $('#hfPunto').val();
    var barranomb = $("#txtModifPtomedibarranomb").val();
    var elenomb = $("#txtModifPtomedielenomb").val();
    var descripcion = $("#txtModifDescripcion").val();
    var estado = $('#idActivo').is(':checked') ? 1 : 0;

    $.ajax({
        type: 'POST',
        url: controlador + "EditarPtoCal",
        dataType: 'json',
        data: {
            ptomedicodi: ptomedicodi,
            barranomb: barranomb,
            elenomb: elenomb,
            descripcion: descripcion,
            estado: estado
        },
        success: function (model) {
            if (model.StrResultado != '-1') {
                alert("Se actualizó correctamente");
                $('#popupmpto').bPopup().close();
                mostrarListaPto();
            }
            else {
                alert("Error en actualizar: " + model.StrMensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function eliminarPtoCal(ptomedicodi) {
    if (confirm("¿Desea eliminar el punto de medición calculado?")) {
        $.ajax({
            type: 'POST',
            url: controlador + "EliminarPtoCal",
            data: {
                ptomedicodi: ptomedicodi
            },
            success: function (evt) {
                mostrarListaPto();
            },
            error: function (err) {
                alert("Error al mostrar Ventana para agregar puntos");
            }
        });
    }
}

function verPuntoCalculado(ptomedicodi) {
    location.href = controlador + "IndexDetalleCalculado?pto=" + ptomedicodi;
}

/////////////////////////////////////////////////
/// Nuevo Punto calculado
/////////////////////////////////////////////////
function nuevoPtoCalculado() {
    $('#agregarPtoCalculado').html('');

    reporte = $('#hfReporte').val();
    $.ajax({
        type: 'POST',
        url: controlador + "MostrarAgregarPtoCalculado",
        data: {
            reporte: reporte,
            lectcodi: 0
        },

        success: function (evt) {
            $('#agregarPtoCalculado').html(evt);
            $('#btnAgregarPtoCalculado').unbind();
            $('#btnAgregarPtoCalculado').on('click', function () {
                agregarNuevoPtoCalculado();
            });

            $('#cbTipoEmpresa').on('change', function () {
                listarEmpresa($(this).val());
            });

            $('#idFamilia').on('change', function () {
                var empresa = $('#cbEmpresa').val();
                listarequipo(empresa, $(this).val());
            });

            setTimeout(function () {
                $('#popupCalculado').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);
            listarEmpresa($('#cbTipoEmpresa').val());
        },
        error: function (err) {
            alert("Error al mostrar Ventana para agregar punto calculado");
        }
    });
}

function agregarNuevoPtoCalculado() {
    var medida = $('#idMedidas').val();
    var reporte = $('#hfReporte').val();

    var empresa = $('#cbEmpresa').val();
    var equipocodi = $('#idequipo').val();
    var lectura = $('#idorigenlectura').val();
    var barranomb = $('#txtPtomedibarranomb').val();
    var elenomb = $('#txtPtomedielenomb').val();
    var descripcion = $('#txtDescripcion').val();

    var mjsValidacion = '';

    //if (equipocodi == 0) {
    //    mjsValidacion += "Seleccionar punto de medición" + "\n";
    //}
    if (lectura == 0) {
        mjsValidacion += "Seleccionar origen lectura" + "\n";
    }
    if (barranomb == null || barranomb == '') {
        mjsValidacion += "Ingresar nombre" + "\n";
    }
    if (elenomb == null || elenomb == '') {
        mjsValidacion += "Ingresar abreviatura" + "\n";
    }
    if (descripcion == null || descripcion == '') {
        mjsValidacion += "Ingresar descripción" + "\n";
    }
    if (medida == 0) {
        mjsValidacion += "Seleccionar medida" + "\n";
    }

    if (mjsValidacion != '') { alert(mjsValidacion); }
    else {
        $.ajax({
            type: 'POST',
            url: controlador + 'AgregarPtoCalculado',
            dataType: 'json',
            data: {
                empresa: empresa, reporte: reporte,
                medida: medida, equipocodi: equipocodi,
                lectura: lectura, barranomb: barranomb, elenomb: elenomb, descripcion: descripcion
            },
            cache: false,
            success: function (res) {
                switch (res.Resultado) {
                    case 1:
                        alert("Registro Satisfactorio")
                        $('#popupCalculado').bPopup().close();
                        mostrarListaPto();
                        break;
                    case -1:
                        alert("Error en BD ptos de medicion");
                        break;
                    case 0:
                        alert("Ya existe la información ingresada");
                        break;
                }
            },
            error: function (err) {
                alert("Error al grabar punto de medición");
            }
        });
    }
}

/////////////////////////////////////////////////
/// Filtros
/////////////////////////////////////////////////

function listarEmpresa(idTipoEmpresa) {
    $.ajax({
        type: 'POST',
        url: controlador + "ListarEmpresas",
        data: {
            idTipoEmpresa: idTipoEmpresa,
            origlectcodi: -1
        },

        success: function (model) {
            cargarComboEmpresa(model.ListaEmpresa, 0);

            $('#cbEmpresa').unbind();
            $('#cbEmpresa').change(function () {
                var empresa = parseInt($('#cbEmpresa').val()) || 0;
                listarFamilia(empresa);
            });

            var empresa = parseInt($('#cbEmpresa').val()) || 0;
            listarFamilia(empresa);
        },
        error: function (err) {
            alert("Error al mostrar lista de empresas");
        }
    });
}

function cargarComboEmpresa(lista, id) {
    $("#cbEmpresa").empty();
    if (id <= 0 && lista.length != 1)
        $("#cbEmpresa").append('<option value="0" selected="selected"> [Seleccionar Empresa] </option>');
    for (var i = 0; i < lista.length; i++) {
        var regEmp = lista[i];
        var selEmp = id == regEmp.Emprcodi ? 'selected="selected"' : '';
        $("#cbEmpresa").append('<option value=' + regEmp.Emprcodi + ' ' + selEmp + '  >' + regEmp.Emprnomb + '</option>');
    }
}

function listarFamilia(empresa) {
    $("#idFamilia").empty();
    $("#idFamilia").append('<option value="0" selected="selected"> [Seleccionar Tipo de Equipo] </option>');

    $.ajax({
        type: 'POST',
        url: controlador + "ListarFamilia",
        data: {
            empresa: empresa,
            origlectcodi: -1
        },

        success: function (model) {
            cargarComboFamilia(model.ListaFamilia, 0);

            $('#idFamilia').unbind();
            $('#idFamilia').change(function () {
                var origlectcodi = -1;
                var empresa = parseInt($('#cbEmpresa').val()) || 0;
                var familia = parseInt($("#idFamilia").val()) || 0;

                listarequipo(empresa, familia, origlectcodi);
            });

            var origlectcodi = -1;
            var empresa = parseInt($('#cbEmpresa').val()) || 0;
            var familia = parseInt($("#idFamilia").val()) || 0;

            listarequipo(empresa, familia, origlectcodi);
        },
        error: function (err) {
            alert("Error al mostrar lista de Tipo de equipos");
        }
    });
}

function cargarComboFamilia(lista, id) {
    $("#idFamilia").empty();
    if (id <= 0 && lista.length != 1)
        $("#idFamilia").append('<option value="0" selected="selected"> [Seleccionar Tipo de Equipo] </option>');
    for (var i = 0; i < lista.length; i++) {
        var reg = lista[i];
        var sel = id == reg.Famcodi ? 'selected="selected"' : '';
        $("#idFamilia").append('<option value=' + reg.Famcodi + ' ' + sel + '  >' + reg.Famnomb + '</option>');
    }
}

function listarequipo(empresa, familia, origlectcodi) {
    $.ajax({
        type: 'POST',
        url: controlador + "listarequipo",
        data: {
            empresa: empresa,
            familia: familia,
            origlectcodi: origlectcodi
        },

        success: function (model) {
            cargarComboEquipo(model.ListaEquipo, 0);
        },
        error: function (err) {
            alert("Error al mostrar lista de equipos");
        }
    });
}

function cargarComboEquipo(lista, id) {
    $("#idequipo").empty();
    if (id <= 0 && lista.length != 1)
        $("#idequipo").append('<option value="0" selected="selected"> [Seleccionar Equipo] </option>');
    for (var i = 0; i < lista.length; i++) {
        var regEq = lista[i];
        var selEq = id == regEq.Equicodi ? 'selected="selected"' : '';
        $("#idequipo").append('<option value=' + regEq.Equicodi + ' ' + selEq + '  >' + regEq.Equinomb + '</option>');
    }
}
