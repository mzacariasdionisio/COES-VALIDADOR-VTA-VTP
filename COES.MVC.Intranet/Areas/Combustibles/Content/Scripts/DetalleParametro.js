var controlador = siteRoot + 'Combustibles/configuracion/';

$(function () {

    $('#btnInicio').click(function () {
        regresarPrincipal();
    });
    $('#btnGuardar').click(function () {
        guardarDatosCentralesxFenerg();
    });

    $('#btnGrupomop').click(function () {
        var idGrupo = $("#hdIdGrupo").val();
        var idAgrup = $("#hdIdAgrup").val();
        var fechaConsulta = $("#hdFechaConsulta").val();
        var url = siteRoot + 'Migraciones/Parametro/' + "Index?grupocodi=" + idGrupo + "&agrupcodi=" + idAgrup + "&fechaConsulta=" + fechaConsulta;
        window.open(url, '_blank').focus();
    });

    ObtenerListadoConceptocombsResultado();
});

function regresarPrincipal() {
    document.location.href = controlador + "Index";
}

function ObtenerListadoConceptocombsResultado() {
    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerListadoConceptocombsResultado',
        dataType: 'json',
        data: {
            Cbcxfecodi: $("#txtCbcxfecodi").val()
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {

                $("#listado").html(evt.HtmlListado);

            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error...");
        }
    });
}

function obtenerDatosTablaConcepto() {
    var listaDatos = [];
    var cbcxfecodi = $("#txtCbcxfecodi").val();

    $('#tabla > tbody  > tr').each(function (index, tr) {
        var ccombcodi = $(this).find("input[name='txtCcombcodi']").val();
        var valor1 = $(this).find("input[name='txtValor1']").val();
        var valor2 = $(this).find("input[name='txtValor2']").val();

        var dato = {
            Cbcxfecodi: cbcxfecodi,
            Ccombcodi: ccombcodi,
            Cbdatvalor1: valor1,
            Cbdatvalor2: valor2
        }

        listaDatos.push(dato);
    })

    return listaDatos;
}

function guardarDatosCentralesxFenerg() {

    var lstDato = obtenerDatosTablaConcepto();

    $.ajax({
        type: 'POST',
        url: controlador + 'GuardarDatoCentralXFenerg',
        dataType: 'json',
        data: JSON.stringify(lstDato),
        contentType: "application/json",
        success: function (evt) {
            if (evt.Resultado != "-1") {
                alert("La información se guardó correctamente");
                ObtenerListadoConceptocombsResultado();
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error...");
        }
    });
}

function validaNum(evt, id) {
    try {
        var charCode = (evt.which) ? evt.which : event.keyCode;
        if (charCode == 86) { return false; }
        if (charCode == 46) {

            var txt = document.getElementById(id).value;
            txt += ".";
            var count = txt.split(".").length - 1;

            if ((txt.indexOf(".") > 0) && count <= 1) { return true; }
        }
        if (charCode > 31 && (charCode < 48 || charCode > 57)) { return false; }
        return true;
    } catch (w) { alert(w); }
}