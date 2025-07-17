var controlador = siteRoot + 'Migraciones/LectorConfigsp7/'

$(function () {

    $('#cbEmpresa').change(function (e) {
        cargarZonas();
    });

    $('#cbZona').on('change', function () {
        cargarCanales();
    });

    $('#btnProcesar').on('click', function () {
        procesarDatominutosp7();
    });
    window.history.forward();
    function noBack() {
        window.history.forward();
    }
});

function checkAll(ele) {
    var checkboxes = document.getElementsByTagName('input');
    if (ele.checked) {
        for (var i = 0; i < checkboxes.length; i++) {
            if (checkboxes[i].type == 'checkbox') {
                checkboxes[i].checked = true;
            }
        }
    } else {
        for (var i = 0; i < checkboxes.length; i++) {
            if (checkboxes[i].type == 'checkbox') {
                checkboxes[i].checked = false;
            }
        }
    }
}

function cargarZonas() {
    $("#cbZona").empty();
    $('#cbZona').get(0).options[0] = new Option("--SELECCIONE ZONA--", "-1");
    $("#listado").html('');
    $("#tb_").hide();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarZonas',
        dataType: 'json',
        data: {
            emprcodi: $('#cbEmpresa').val(),
            fecha: $('#txtFecha').val()
        },
        cache: false,
        success: function (data) {
            if (data.ListaTrzonas.length > 0) {
                $.each(data.ListaTrzonas, function (i, item) {
                    $('#cbZona').get(0).options[$('#cbZona').get(0).options.length] = new Option(item.Zonanomb, item.Zonacodi);
                });
            }
        },
        error: function () {
            mostrarError();
        }
    });
}

function cargarCanales() {
    if ($('#cbZona').val() != "-1") {
        $.ajax({
            type: 'POST',
            url: controlador + "CargarCanales",
            dataType: 'json',
            data: { zonacodi: $('#cbZona').val() },
            success: function (evt) {
                $("#listado").html(evt.Resultado);
                if (evt.nRegistros > 0) {
                    $("#tb_info").dataTable({
                        "scrollY": "200px",
                        "scrollCollapse": true,
                        "paging": false,
                        "ordering": false
                    }); $("#tb_").show();
                } else { $("#tb_").hide(); }
            },
            error: function (err) { alert("Error al cargar Excel Web"); }
        });
    } else { $("#listado").html(''); $("#tb_").hide(); }
}

function procesarDatominutosp7() {
    var cadena = "";
    var checkboxes = document.getElementById('tbSeleccionados').getElementsByTagName('input');
    for (var i = 0; i < checkboxes.length; i++) {
        if (checkboxes[i].type == 'checkbox' && checkboxes[i].checked == true) {
            var valor = checkboxes[i].id;
            if (cadena == "") {
                cadena = valor;
            }
            else {
                cadena = cadena + "," + valor;
            }
        }
    }

    if (cadena != "") {
        $.ajax({
            type: "POST",
            url: controlador + "LoadViewScada",
            data: {
                fecha: convertirFecha($("#txtFecha").val()), canalcodi: cadena
            },
            success: function (data) {
                if (data == 1) { window.location.href = controlador + "ViewScada"; }
            },
            error: function () {
                alert("Error");
            }
        });
    } else { alert("Debe seleccionar al menos un Codigo"); }
}

function convertirFecha(fecha) {
    const partes = fecha.split('-');
    const año = partes[0];
    const mes = partes[1];
    const día = partes[2];
    return `${día}/${mes}/${año}`;
}
