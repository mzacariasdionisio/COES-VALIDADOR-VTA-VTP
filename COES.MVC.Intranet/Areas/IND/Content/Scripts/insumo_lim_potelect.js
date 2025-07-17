var controlador = siteRoot + 'IND/LimitePotElect/';
var LISTA_UNIDAD = [];

$(function () {
    $('#cntMenu').css("display", "none");

    $('#cbAnio').change(function () {
        listadoPeriodo();
    });
    $('#cbPeriodo').change(function () {
        mostrarListado();
    });

    $('#btnNuevo').click(function () {
        visualizarPopup();
    });

    $('#btnBuscar').click(function () {
        mostrarListado();
    });

    mostrarListado();
});

function mostrarListado() {
    $('#listado').html('');

    $.ajax({
        type: 'POST',
        url: controlador + "LimPotListado",
        data: {
            pericodi: $("#cbPeriodo").val()
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                $("#hdFechaIni").val(evt.FechaIni);
                $("#hdFechaFin").val(evt.FechaFin);

                $('#listado').css("width", $('#mainLayout').width() + "px");

                $('#listado').html(evt.Resultado);

            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function listadoPeriodo() {

    var anio = parseInt($("#cbAnio").val()) || 0;

    $("#cbPeriodo").empty();

    $.ajax({
        type: 'POST',
        url: controlador + "PeriodoListado",
        data: {
            anio: anio,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                if (evt.ListaPeriodo.length > 0) {
                    $.each(evt.ListaPeriodo, function (i, item) {
                        $('#cbPeriodo').get(0).options[$('#cbPeriodo').get(0).options.length] = new Option(item.Iperinombre, item.Ipericodi);
                    });
                } else {
                    $('#cbPeriodo').get(0).options[0] = new Option("--", "0");
                }

                mostrarListado();
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function cargarListaCentral() {
    $("#cboCentral").empty();
    $('#cboCentral').get(0).options[$('#cboCentral').get(0).options.length] = new Option("--SELECCIONE--", 0);
    $("#cboUnidad").empty();
    $('#cboUnidad').get(0).options[$('#cboUnidad').get(0).options.length] = new Option("--SELECCIONE--", 0);

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarFiltros',
        dataType: 'json',
        data: {
            pericodi: $("#cbPeriodo").val(),
            emprcodi: parseInt($("#cboEmpresa").val()) || 0,
        },
        cache: false,
        success: function (data) {
            if (data.ListaCentral.length > 0) {
                $.each(data.ListaCentral, function (i, item) {
                    $('#cboCentral').get(0).options[$('#cboCentral').get(0).options.length] = new Option(item.Central, item.Equipadre);
                });
            }

            $('#cboCentral').unbind();
            $('#cboCentral').change(function () {
                cargarListaUnidad();
            });

        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function cargarListaUnidad() {
    $("#cboUnidad").empty();
    $('#cboUnidad').get(0).options[$('#cboUnidad').get(0).options.length] = new Option("--SELECCIONE--", 0);

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarModo',
        dataType: 'json',
        data: {
            pericodi: $("#cbPeriodo").val(),
            emprcodi: parseInt($("#cboEmpresa").val()) || 0,
            equipadre: parseInt($("#cboCentral").val()) || 0,
        },
        cache: false,
        success: function (data) {
            if (data.ListaModoOperacion.length > 0) {
                $.each(data.ListaModoOperacion, function (i, item) {
                    $('#cboUnidad').get(0).options[$('#cboUnidad').get(0).options.length] = new Option(item.text, item.codigo);
                });
            }

        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function mostrarError(msj) {
    alert("Ha ocurrido un error");
    console.log(msj);
}

////////////////////////////////////////////////////////////////////////////////////
/// Crud
////////////////////////////////////////////////////////////////////////////////////

function visualizarPopup() {
    var fechaIni = $("#hdFechaIni").val();
    var fechaFin = $("#hdFechaFin").val();

    $('#txt_fecha_ini_reg').val(fechaIni);
    $('#txt_fecha_fin_reg').val(fechaFin);

    $('#txt_fecha_ini_reg').Zebra_DatePicker({
        direction: [fechaIni, fechaFin],
        pair: $('#txt_fecha_fin_reg'),
        onSelect: function (date) {
            $('#txt_fecha_fin_reg').val(date);
        }
    });
    $('#txt_fecha_fin_reg').Zebra_DatePicker({
        direction: [fechaIni, fechaFin]
    });

    $("#txt_sistransm").val('');
    $("#txt_pl").val('');

    $('#cboEmpresa').unbind();
    $('#cboEmpresa').change(function () {
        cargarListaCentral();
    });

    $("#btnAgregarUnidad").unbind();
    $("#btnAgregarUnidad").click(function () {
        unid_agregarForm();
    });

    $("#btnGuardarForm").unbind();
    $("#btnGuardarForm").click(function () {
        unid_Guardar();
    });

    LISTA_UNIDAD = [];
    unid_generarTablaUnidad(LISTA_UNIDAD);

    setTimeout(function () {
        $('#popupRegistro').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: true
        });
    }, 50);
}

function unid_generarTablaUnidad(listaUnidad) {

    //Generar Tabla html
    var html = '';
    html += `
        <table class='pretty tabla-icono tabla_unidad' id='tabla_unidad' style='width: 510px; margin-top: 5px;    margin-bottom: 5px;'>
            <thead>
                <tr>
                    <th>Empresa</th>
                    <th>Central</th>
                    <th>Unidad</th>
                    <th>Potencia Efectiva (MW)</th>
                </tr>
            </thead>
            <tbody>
        `;

    var pos_row = 0;
    listaUnidad.forEach(function (obj) {
        var idFila = 'tr_unidad_' + pos_row;
        html += `
                <tr id="${idFila}">
                    <td>${obj.Emprnomb}</td>
                    <td>${obj.Central}</td>
                    <td>${obj.NombreUnidad}</td>
                    <td>${obj.Equlimpotefectiva}</td>
                    <td style='text-align: center'>
                        <a href='JavaScript:unid_quitarFila(${pos_row})'>
                            <img src='${siteRoot}Content/Images/btn-cancel.png' title='Eliminar fila' />
                        </a>
                    </td>
                </tr>
            `;

        pos_row++;
    });

    html += `
            </tbody>
        </table>
        `;

    $("#div_tabla_unidad").html(html);
}

function unid_quitarFila(pos_row) {
    $('#tabla_unidad #tr_unidad_' + pos_row).remove();

    var listaTmp = [];
    if (LISTA_UNIDAD != null) {
        for (var i = 0; i < LISTA_UNIDAD.length; i++) {
            if (i != pos_row)
                listaTmp.push(LISTA_UNIDAD[i]);
            //LISTA_UNIDAD = LISTA_UNIDAD.splice(pos_row + 1, 1);
        }
    }
    LISTA_UNIDAD = listaTmp;

    unid_generarTablaUnidad(LISTA_UNIDAD);
}

function unid_agregarForm() {
    var pos_row = LISTA_UNIDAD.length || 0;

    var empresa = $("#cboEmpresa option:selected").text();
    var emprcodi = $("#cboEmpresa").val();

    var central = $("#cboCentral option:selected").text();
    var equipadre = $("#cboCentral").val();

    var unidad = $("#cboUnidad option:selected").text();
    var grupocodiyPe = $("#cboUnidad").val();

    var grupocodi = null;
    var pe = 0;
    var equicodi = null;
    if (grupocodiyPe != null) {
        var params = grupocodiyPe.split(',');
        grupocodi = params[0];
        pe = params[1];
        equicodi = params[2];
    }

    var obj =
    {
        Emprcodi: emprcodi,
        Emprnomb: empresa,
        Central: central,
        Equipadre: equipadre,
        Grupocodi: grupocodi,
        Equicodi: equicodi,
        NombreUnidad: unidad,
        Equlimpotefectiva: pe,
        Fila: pos_row
    };

    LISTA_UNIDAD.push(obj);

    unid_generarTablaUnidad(LISTA_UNIDAD);
}

function unid_Guardar() {

    var mw = $("#txt_pl").val();
    var nombre = $("#txt_sistransm").val();

    var msj = obtenerValidacion();

    if (msj == '') {
        $.ajax({
            type: "POST",
            url: controlador + "LimPotGuardar",
            data: {
                strFechaIni: $("#txt_fecha_ini_reg").val(),
                strFechaFin: $("#txt_fecha_fin_reg").val(),
                nombre: nombre,
                mw: mw,
                strJsonData: JSON.stringify(LISTA_UNIDAD)
            }
        }).done(function (result) {
            if (result.Resultado != "-1") {
                $('#popupRegistro').bPopup().close();
                mostrarListado();
            } else {
                alert("Ha ocurrido un error: " + result.Mensaje);
            }

        }).fail(function (jqXHR, textStatus, errorThrown) {
            alert("Ha ocurrido un error");
        });
    } else {
        alert(msj);
    }
}

function obtenerValidacion() {
    var mw = $("#txt_pl").val();
    var nombre = $("#txt_sistransm").val();
    var msj = '';

    if (mw == null || mw == '') {
        msj += 'Debe ingresar Capacidad(MW)';
    }
    if (nombre == null || nombre == '') {
        msj += 'Debe ingresar nombre';
    }

    return msj;
}

function eliminarRegistro(id) {
    if (confirm("¿Desea eliminar el registro?")) {
        $.ajax({
            type: 'POST',
            url: controlador + 'LimPotDarBaja',
            data: {
                id: id
            },
            cache: false,
            success: function (result) {
                if (result.Resultado == '-1') {
                    alert('Ha ocurrido un error:' + result.Mensaje);
                } else {
                    alert("Se eliminó correctamente el registro.");
                    mostrarListado();
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error.');
            }
        });
    }
}