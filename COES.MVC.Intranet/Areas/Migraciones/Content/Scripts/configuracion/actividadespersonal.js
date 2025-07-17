var controlador = siteRoot + 'Migraciones/Configuraciones/'

$(function () {
    $('#cbArea').multipleSelect({
        width: '250px',
        filter: true,
        onClose: function (view) {
            cargarActividadesPersonal();
        }
    });

    $("#btnCrear").click(function () {
        limpiarPopupCrear();
        openPopupCrear();

    });

    $("#btnGrabar").click(function () {
        openCrear($('#event').val());
    });

    cargarActividadesPersonal();
    $('#cbArea').multipleSelect('checkAll');
});


function limpiarPopupCrear() {
    $('#actcodi').val(0);
    $('#event').val(1);
    $("#cbArea_").val($("#cbArea_ option:first").val()).prop('disabled', false);
    $("#txtActabrev").val("").attr('readonly', false);
    $("#txtActnomb").val("").attr('readonly', false);

}

function openPopupCrear() {
    setTimeout(function () {
        $('#popupTablaNuevo').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    }, 50);
}

function openCrear(x) {
    if ($('#txtActabrev').val() == "") { alert("Campo Actividad Abrev. Obligatorio..!!"); return; }
    if ($('#txtActnomb').val() == "") { alert("Campo Actividad Descripcion. Obligatorio..!!"); return; }

    $.ajax({
        type: 'POST',
        url: controlador + "SaveActividad",
        dataType: 'json',
        data: { areacodi: $('#cbArea_').val(), acti: $('#txtActabrev').val(), descrip: $('#txtActnomb').val(), evnto: x, actcodi: $('#actcodi').val() },
        success: function (evt) {
            if (evt.nRegistros != -1) {
                cargarActividadesPersonal();
				$('#popupTablaNuevo').bPopup().close();
            }
            else { alert("Ha ocurrido un error."); }
        },
        error: function (err) { alert("Error al cargar Excel Web"); }
    });
}

function cargarActividadesPersonal() {
    var areas = $('#cbArea').multipleSelect('getSelects');
    if (areas == "") areas = "-1";
    $('#hdcbArea').val(areas);

    $.ajax({
        type: 'POST',
        url: controlador + "CargarActividadesPersonal",
        dataType: 'json',
        data: {
            areacodi: $('#hdcbArea').val()
        },
        success: function (evt) {
            $('#listado').html(evt.Resultado);
            if (evt.nRegistros > 0) {
                $("#tb_Actividades").dataTable({
                    "ordering": false,
                    "bLengthChange": false,
                    "bInfo": false,
                });
            }
        },
        error: function (err) { alert("Error..!!"); }
    });
}

function delete_(x) {
    $.ajax({
        type: 'POST',
        url: controlador + "DeleteActividad",
        dataType: 'json',
        data: { actcodi: x },
        success: function (evt) {
            if (evt.nRegistros > 0) {
                cargarActividadesPersonal();
            }
            else {
                if (evt.nRegistros == -2) {
                    alert("Existen Rols de Turnos asignados con Actividad a eliminar");
                } else {
                    alert("Ha ocurrido un error.");
                }
            }
        },
        error: function (err) { alert("Error al cargar Excel Web"); }
    });
}

function edit_(x, y) {
    limpiarPopupCrear();
    $.ajax({
        type: 'POST',
        url: controlador + "ProcesoEditActividad",
        dataType: 'json',
        data: { actcodi: x, evnto: y },
        success: function (evt) {
            if (evt.nRegistros != -1) {
                $('#actcodi').val(evt.Actcodi);
                $('#cbArea_').val(evt.Areacodi);
                $('#txtActabrev').val(evt.Actabrev);
                $('#txtActnomb').val(evt.Actnomb);

                $('#txtActabrev').attr('readonly', true);
                $('#txtActnomb').attr('readonly', true);
                $('#cbArea_').prop('disabled', (y == 1 ? true : false));
                $('#event').val(y);

                if (y == 1) { $('#btnGrabar').hide(); } else { $('#btnGrabar').show(); }
                openPopupCrear();
            }
            else { alert("Ha ocurrido un error."); }
        },
        error: function (err) { alert("Error al cargar Excel Web"); }
    });
}