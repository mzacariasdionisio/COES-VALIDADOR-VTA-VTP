var controlador = siteRoot + 'IND/gaseoducto/';
var ancho = 1100;

var TipoEstado = Object.freeze(
    {
        OK: 1,
        ERROR: 0
    }
);

var tblGaseoducto, validator;

$(function () {

    $('#cntMenu').css("display", "none");

    $('#tab-container').easytabs({
        animate: false
    });
    $('#tab-container').bind('easytabs:after', function () {
        refrehDatatable();
    });

    $('#cbAnio').change(function () {
        listadoPeriodo();
    });
    $('#cbPeriodo').change(function () {
        mostrarListado();
    });

    tblGaseoducto = $('#tblGaseoducto').DataTable({
        data: [],
        columns: [
            { data: "Gasctrcodi", "visible": false },
            { data: "Emprnomb" },
            { data: "Gaseoducto" },
            { data: "Central" },
            { data: "UltimaModificacionUsuarioDesc" },
            { data: "UltimaModificacionFechaDesc" },
            {
                mRender: function (data, type, row) {
                    return "<a id='btnDelete'><img src='" + siteRoot + "Content/Images/btn-cancel.png' alt='Eliminar Nota'></a>";
                },
                searchable: false,
                orderable: false
            }
        ],
        filter: false,
        info: false,
        ordering: false,
        processing: true,
        retrieve: true
    });

    $('#tblGaseoducto tbody').on('click', '#btnDelete', function () {

        if (confirm("¿Realmente desea eliminar?")) {

            var data = tblGaseoducto.row($(this).parents('tr')).data();

            $.ajax({
                type: "DELETE",
                url: controlador + "EliminarGaseoducto",
                data: { gasctrcodi: data.Gasctrcodi }
            }).done(function (result) {
                if (result.Estado === TipoEstado.OK) {
                    alert("Se eliminó correctamente");
                    cargarGaseoducto();
                    mostrarListado();
                }
                else {
                    alert("Ha ocurrido un error");
                }

            }).fail(function (jqXHR, textStatus, errorThrown) {
                alert("Ha ocurrido un error");
            });

        }
    });

    validator = $("#frmGaseoducto").validate({
        rules: {
            Equicodi: {
                required: true
            },
            Gaseoductoequicodi: {
                required: true
            }
        }, messages: {
            Equicodi: {
                required: "Falta seleccionar Central"
            },
            Gaseoductoequicodi: {
                required: "Falta seleccionar Gaseoducto"
            }
        },
        errorPlacement: function (error, element) {

        },
        invalidHandler: function (form, ivalidator) {
            var errors = ivalidator.numberOfInvalids();
            if (errors) {
                alert(ivalidator.errorList[0].message);  //Only show first invalid rule message!!!
                ivalidator.errorList[0].element.focus(); //Set Focus
            }
        }
    });

    $('#cboEmpresa').change(function () {
        cargarFiltro();
    });

    $('#btnNuevo').bind('click', function (e) {
        initModalNota();
        e.preventDefault();
        openPopupCrear();
    });

    $('#tab-container').easytabs('select', '#vistaRelacion');
    mostrarListado();
    cargarGaseoducto();
});

///////////////////////////////////////////////////////////////////
/// Formulario Gaseoducto

function mostrarListado() {

    var pericodi = parseInt($("#cbPeriodo").val()) || 0;

    $('#listado').html('');

    ancho = $("#mainLayout").width() > 1100 ? $("#mainLayout").width() - 30 : 1100;

    $.ajax({
        type: "POST",
        async: false,
        url: controlador + "CargarListaReporteRendimiento",
        data: {
            pericodi: pericodi,
        },
    }).done(function (evt) {

        if (evt.Resultado != "-1") {
            $('#listado').html(evt.Resultado);
            refrehDatatable();
        } else {
            alert("Ha ocurrido un error: " + evt.Mensaje);
        }

    }).fail(function (jqXHR, textStatus, errorThrown) {
        alert("Ha ocurrido un error");
    });
}



///////////////////////////////////////////////////////////////////
/// Formulario Gaseoducto

function cargarGaseoducto() {

    $.ajax({
        type: "POST",
        async: false,
        url: controlador + "CargarListaGaseoducto"
    }).done(function (result) {
        tblGaseoducto.clear().draw();
        tblGaseoducto.rows.add(result.ListadoRelacionGaseoducto).draw();
    }).fail(function (jqXHR, textStatus, errorThrown) {
        alert("Ha ocurrido un error");
    });
}

function initModalNota() {
    //tblGaseoducto.clear().draw();
    restablecerFormulario();
}

function cargarFiltro() {
    $("#cboGaseoductoequicodi").empty();
    $('#cboGaseoductoequicodi').get(0).options[$('#cboGaseoductoequicodi').get(0).options.length] = new Option("--SELECCIONE--", 0);
    $("#cboEquicodi").empty();
    $('#cboEquicodi').get(0).options[$('#cboEquicodi').get(0).options.length] = new Option("--SELECCIONE--", 0);

    var emprcodi = parseInt($("#cboEmpresa").val()) || 0;

    if (emprcodi > 0) {
        $.ajax({
            type: 'POST',
            url: controlador + 'CargarFiltros',
            dataType: 'json',
            data: {
                emprcodi: emprcodi,
            },
            cache: false,
            success: function (data) {
                if (data.ListaCentral.length > 0) {
                    $.each(data.ListaCentral, function (i, item) {
                        $('#cboEquicodi').get(0).options[$('#cboEquicodi').get(0).options.length] = new Option(item.Central, item.Equipadre);
                    });
                }

                if (data.ListaGasoducto.length > 0) {
                    $.each(data.ListaGasoducto, function (i, item) {
                        $('#cboGaseoductoequicodi').get(0).options[$('#cboGaseoductoequicodi').get(0).options.length] = new Option(item.Equinomb, item.Equicodi);
                    });
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error.');
            }
        });
    }
}

function restablecerFormulario() {
    $("#txtGasctrcodi").val(null);
    $("#frmGaseoducto").trigger("reset");
}

function openPopupCrear() {

    $('#popupFormulario').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown',
        transitionClose: 'slideUp',
        modalClose: false
    });
}

function guardarGaseoducto() {
    event.preventDefault();

    if ($("#frmGaseoducto").valid()) {
        var indGaseoductoxcentral = {
            Equicodi: $("#cboEquicodi").val(),
            Gaseoductoequicodi: $("#cboGaseoductoequicodi").val(),
        };

        $.ajax({
            type: "POST",
            url: controlador + "GuardarGaseoducto",
            data: indGaseoductoxcentral
        }).done(function (result) {
            if (result.Estado === TipoEstado.OK) {
                restablecerFormulario();
                $('#popupFormulario').bPopup().close();
                alert("Se registró correctamente");
                cargarGaseoducto();
                mostrarListado();

                $('#tab-container').easytabs('select', '#vistaRelacion');
            } else {
                alert("Ha ocurrido un error");
            }

        }).fail(function (jqXHR, textStatus, errorThrown) {
            alert("Ha ocurrido un error");
        });

    }
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

function refrehDatatable() {

    $("#miTabla").css("width", (ancho) + "px");

    $('#miTabla').dataTable({
        "sPaginationType": "full_numbers",
        "destroy": "true",
        "ordering": true,
        "searching": true,
        "iDisplayLength": 15,
        "info": false,
        "paging": false,
        "scrollX": true,
        "scrollY": "100%"
    });
}