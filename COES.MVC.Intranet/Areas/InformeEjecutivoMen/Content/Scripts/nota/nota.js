var controladorN = siteRoot + 'InformeEjecutivoMen/Nota/';

var TipoEstado = Object.freeze(
    {
        OK: 1,
        ERROR: 0
    }
);
var tablaNota;
var validator;
$(function () {

    validator = $("#frmNota").validate({
        rules: {
            Sinotadesc: {
                required: true,
                maxlength: 1000
            }
        }, messages: {
            Sinotadesc: {
                required: "Ingrese descripción de nota",
                maxlength: "Máximo 500 caracteres"
            }
        },
        errorContainer: "#contenedorMensaje",
        errorLabelContainer: "#contenedorMensaje ul",
        wrapper: "li"
    });

    $('#txtMesAnioN').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () {
            validator.resetForm();
            cargarNota();
        }
    });


    $("#cboMenuEjecutivo").on("change", function (event) {
        validator.resetForm();
        cargarNota();
    });

    $('#btnNuevaNota').bind('click', function (e) {
        if ($('#cboVersiones').val() === null) {
            alert("Seleccionar versión del Reporte Ejecutivo");
            return;
        }
        initModalNota();
        seterCabeceraNota();

        e.preventDefault();
        openPopupCrear();
    });

    // #region TABLA NOTA

    tablaNota = $('#tablaNota').DataTable({
        data: [],
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.10.19/i18n/Spanish.json"
        },
        columns: [
            { data: "Sinotacodi", "visible": false },
            { data: "Sinotaorden", width: "10%" },
            { data: "Sinotadesc", width: "80%", className: "text" },
            {
                mRender: function (data, type, row) {
                    var strEdit = "<a id='btnEdit'><img src='" + siteRoot + "Content/Images/btn-edit.png' alt='Editar Nota'></a>";
                    var strDelete = " <a id='btnDelete'><img src='" + siteRoot + "Content/Images/btn-cancel.png' alt='Eliminar Nota'></a>";
                    return strEdit + strDelete;
                },
                searchable: false,
                orderable: false
            }
        ],
        rowCallback: function (row, data) { },
        filter: false,
        info: true,
        ordering: false,
        processing: true,
        retrieve: true,
        rowReorder: {
            snapX: 10,
            dataSrc: 'Sinotaorden'

        }
    });


    tablaNota.on('row-reorder', function (e, diff, edit) {

        var data = tablaNota.rows().data();

        var dataCambio = [];

        for (var index in diff) {
            var diff_ = diff[index];
            var itemCambio = data.filter(x => x.Sinotaorden === diff_.oldData)[0];

            var itemCambio_ = $.extend({}, itemCambio);
            itemCambio_.Sinotaorden = diff_.newData;

            dataCambio.push({
                Sinotacodi: itemCambio_.Sinotacodi,
                Sinotaorden: itemCambio_.Sinotaorden
            });
        }

        if (dataCambio.length > 0) {

            $.ajax({
                type: "POST",
                url: controladorN + "UpdateListaNota",
                data: JSON.stringify(dataCambio),
                contentType: "application/json"
            }).done(function (result) {
                if (result.Estado === TipoEstado.OK) {
                    cargarNota();
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                alert("Ha ocurrido un error");
            });
        }

    });


    $('#tablaNota tbody').on('click', '#btnEdit', function () {
        limpiarFormularioNota();
        validator.resetForm();
        var data = tablaNota.row($(this).parents('tr')).data();

        $("#txtSinotacodi").val(data.Sinotacodi);
        $("#txtSinotadesc").val(data.Sinotadesc);
        $("#cboMenuEjecutivo").val(data.Mrepcodi);
        //$("txtMesAnioN").val(moment.utc(data.Sinotaperiodo).format("MM YYYY"));

    });

    $('#tablaNota tbody').on('click', '#btnDelete', function () {
        limpiarFormularioNota();
        validator.resetForm();
        var data = tablaNota.row($(this).parents('tr')).data();

        $.ajax({
            type: "DELETE",
            url: controladorN + "DeleteNota",
            data: { Sinotacodi: data.Sinotacodi }
        }).done(function (result) {
            if (result.Estado === TipoEstado.OK) {
                cargarNota();
            }
            alert(result.Resultado);
        }).fail(function (jqXHR, textStatus, errorThrown) {
            alert("Ha ocurrido un error");
        });

    });

    // #endregion

});

// #region Metodos

function initModalNota() {
    $("#tableContent").hide();
    tablaNota.clear().draw();
    limpiarCabeceraNota();
    limpiarFormularioNota();
    validator.resetForm();
}

function openPopupCrear() {

    $('#popupDetalle').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown',
        transitionClose: 'slideUp',
        modalClose: false
    });
}

function guardarNota() {
    event.preventDefault();
    if ($("#frmNota").valid()) {

        var periodoNota = $('#txtMesAnioN').val();

        var nota = {
            Sinotacodi: $("#txtSinotacodi").val(),
            Sinotadesc: $("#txtSinotadesc").val(),
            Sinotaperiodo: moment(periodoNota, "MM YYYY").format('YYYY-MM-DD'),
            Mrepcodi: $("#cboMenuEjecutivo").val(),
            verscodi: $('#txtVerscodi').val()
        };

        $.ajax({
            type: "POST",
            url: controladorN + "GuardarNota",
            data: nota
        }).done(function (result) {
            if (result.Estado === TipoEstado.OK) {
                limpiarFormularioNota();
                cargarNota();
            }
            alert(result.Resultado);
        }).fail(function (jqXHR, textStatus, errorThrown) {
            alert("Ha ocurrido un error");
        });
    }
}

function cargarNota() {
    $("#tableContent").show("slow");

    var repcodi = $("#cboMenuEjecutivo").val();
    var verscodi = $('#txtVerscodi').val();
    $.ajax({
        type: "POST",
        async: false,
        url: controladorN + "CargarListaNota",
        data: {
            periodo: $('#txtMesAnioN').val(),
            mrepcodi: repcodi,
            verscodi: verscodi
        }
    }).done(function (result) {
        tablaNota.clear().draw();
        tablaNota.rows.add(result).draw();
    }).fail(function (jqXHR, textStatus, errorThrown) {
        alert("Ha ocurrido un error");
    });
}

function limpiarFormularioNota() {
    $("#txtSinotacodi").val(0);
    $("#frmNota").trigger("reset");
}

function limpiarCabeceraNota() {
    $("#cboMenuEjecutivo").prop('selectedIndex', 0);
    $('#txtMesAnioN').val('');
    $('#txtVersion').val('');
    $('#txtVerscodi').val('');
}

function seterCabeceraNota() {
    $('#txtMesAnioN').val($('#txtMesAnio').val());
    $('#txtVersion').val($('#cboVersiones option:selected').text());
    $('#txtVerscodi').val($('#cboVersiones').val());
}

// #endregion