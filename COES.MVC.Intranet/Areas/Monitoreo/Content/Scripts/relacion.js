var controlador = siteRoot + 'Monitoreo/Relacion/';
var ancho = 800;

$(function () {

    ancho = $('#mainLayout').width() - 30;

    $('#btnConsultar').click(function () {
        consultarRelacion();
    });

    $('#btnSave').click(function () {
        saveRelacion();
    });

    $('#cbEmpresa').multipleSelect({
        width: '200px',
        filter: true,
        onClose: function (view) {
        },
        onSelect: function () {
            consultarRelacion();
        }
    });
    $('#cbEmpresa').multipleSelect('checkAll');
    consultarRelacion();

    $('#cbCentral').change(function () {
        consultarRelacion();
    });

});

function consultarRelacion() {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');

    if (empresa == "[object Object]" || empresa.length == 0) empresa = "-1";
    var idCentral = $('#cbCentral').val();

    $.ajax({
        type: 'POST',
        async: true,
        url: controlador + 'ListadoRelacion',
        data: {
            idEmpresa: empresa.join(','),
            idTipoCentral: idCentral
        },
        success: function (aData) {
            if (aData.Resultado != "-1") {
                $('#listado').html(aData.Resultado);
                var anchoReporte = $('#reporte').width();
                $("#resultado").css("width", (anchoReporte > ancho ? anchoReporte : ancho) + "px");
                $('#reporte').dataTable({
                    "scrollX": true,
                    "scrollY": "500px",
                    "scrollCollapse": true,
                    "sDom": 'ft',
                    "ordering": false,
                    paging: false
                });
            } else {
                alert("Ha ocurrido un error: " + aData.Mensaje);
            }
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function seleccionarBarra(codigo, nombre, nombre2) {
    $("#txtCodigoBarra").val(codigo);
    $("#txtNombre1").val(nombre);
    $("#txtNombre2").val((nombre2));
}

function saveRelacion() {
    var codigoGrupo = $('#idCodigo').val();
    var codigoBarra = $('#txtCodigoBarra').val();
    if (codigoGrupo != "" && codigoBarra != "") {
        $.ajax({
            type: 'POST',
            async: true,
            url: controlador + 'SaveRelacionBarraDespacho',
            data: {
                idGrupo: codigoGrupo,
                idBarra: codigoBarra
            },
            success: function (result) {
                if (result.Resultado != '-1') {
                    if (result.Resultado == '0') {
                        alert("Error este registro ya existe");
                    } else {
                        alert("La relación se registro correctamente.");
                        $('#txtCodigoBarra').val('');
                        $('#txtNombre1').val('');
                        $('#txtNombre2').val('');
                        cerrarPopupNew();

                        consultarRelacion();
                    }
                } else {
                    alert("Ha ocurrido un error: " + result.Mensaje);
                }
            },
            error: function (err) {
                alert("Ha ocurrido un error");
            }
        });
    } else {
        alert("Debe seleccionar una Barra de Programación");
    }
}

function eliminarRelacion(id, idRelacion) {

    if (confirm('¿Desea Eliminar la relación?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'DeleteRelacion',
            data: {
                idGrupo: id,
                idRelacion: idRelacion
            },
            success: function (result) {
                if (result.Resultado != '-1') {
                    if (result.Resultado == '0') {
                        alert("No existe la relación");
                    } else {
                        alert("Se eliminó la relación correctamente");
                        consultarRelacion();
                    }
                } else {
                    alert("Ha ocurrido un error: " + result.Mensaje);
                }
            },
            error: function (err) {
                alert("Ha ocurrido un error");
            }
        });
    }
}

function agregarRelacion(id) {
    $('#txtCodigoBarra').val('');
    $('#txtNombre1').val('');
    $('#txtNombre2').val('');

    $("#div_tabla_barra_prog").hide();

    consultar(id);
}

function consultar(id) {

    $.ajax({
        type: 'POST',
        global: false,
        url: controlador + 'ListarBarraYGrupo',
        data: {
            grupocodi: id,
        },
        success: function (aData) {
            if (aData.Resultado == '-1') {
                alert('Ha ocurrido un error:' + result.Mensaje);
            }
            else {
                var htmlTable = "<table id='tabla_barra_prog' class='pretty tabla-icono'><thead><tr><th width='12%'>ID</th>  <th width='12%'>Barra NCP</th>  <th width='12%'>Barra</th>  </tr></thead>";
                var tbody = '';

                $('#idCodigo').val(aData.ObjBarr.Grupocodi);
                $('#txtEmpresa').val(aData.ObjBarr.Emprnomb);
                $('#txtCentro').val(aData.ObjBarr.Gruponomb);

                for (var i = 0; i < aData.LisConfigBarr.length; i++) {
                    var item = aData.LisConfigBarr[i];
                    if (item.Cnfbarindpublicacion == 'S') {
                        var fila = "<tr  onclick=\"seleccionarBarra(" + item.Cnfbarcodi + ",'" + item.Cnfbarnodo + "','" + item.Cnfbarnombre + "');\" style='cursor:pointer' >";
                        fila += "<td style='text-align: center; padding-top: 7px; padding-bottom: 7px;'>" + item.Cnfbarcodi + "</td>";
                        fila += "<td style='text-align: center'>" + item.Cnfbarnombncp + "</td>";
                        fila += "<td style='text-align: left'>" + item.Cnfbarnombre + "</td>";
                        fila += "</tr>";
                        tbody += fila;
                    }
                }

                htmlTable += tbody + "<tbody></tbody></table>";
                $('#div_tabla_barra_prog').html(htmlTable);
                $('#tabla_barra_prog').dataTable({
                    "sPaginationType": "full_numbers",
                    "destroy": "true",
                    "bInfo": false,
                    "bLengthChange": false,
                    "sDom": 'fpt',
                    "ordering": true,
                    "order": [[2, "asc"]],
                    "iDisplayLength": 15
                });

                $("#div_tabla_barra_prog").show();

                setTimeout(function () {
                    $('#idPopupNew').bPopup({
                        easing: 'easeOutBack',
                        speed: 450,
                        transition: 'slideDown',
                        async: true,
                        modalClose: false
                    });
                }, 100);
            }
        },
        error: function (ee) {
            alert('Ha ocurrido un error.');
        }
    });
}

function cerrarPopupNew() {

    $('#idPopupNew').bPopup().close();
}