const controler = siteRoot + "CPPA/AjustePresupuestal/";
const imageRoot = siteRoot + "Content/Images/";

$(document).ready(function () {

    $('#btnConsultar').click(function () {
        consultar();
    });
    $("#btnManualUsuario").click(function () {
        window.location = controler + 'DescargarManualUsuario';
    });

    let anio = new Date().getFullYear(); 

    $('#txtFechaIni').Zebra_DatePicker({
        format: 'Y',
        view: 'years',
        direction: ['2023', false],
        default_position: anio.toString(),
        onSelect: function (view, elements) {
            let anio = $(this).val();
            $('#txtFechaFin').val(anio);
        }
    });

    $('#txtFechaFin').Zebra_DatePicker({
        format: 'Y',
        view: 'years',
        direction: ['2023', false],
        default_position: anio.toString(),
    });

    $('#txtFechaIni').val(anio);
    $('#txtFechaFin').val(anio);
    $('#chbAbierto').prop('checked', true);
    $('#chbCerrado').prop('checked', true);
    $('#chbAnualdo').prop('checked', false);

    buscar(anio, anio, 1, 1, 0);
});

function buscar(anioFrom, anioUntil, estadoA, estadoC, estadoX) {
    let estados = getEstado(estadoA, estadoC, estadoX);

    $.ajax({
        type: 'POST',
        url: controler + "list",
        data: {
            anioFrom: anioFrom,
            anioUntil: anioUntil,
            estados: estados
        },
        success: function (evt) {
            $('#listado').html(evt);
            $('#tabla').dataTable({
                "sPaginationType": "full_numbers",
                "destroy": "true",
                "ordering": false
            });
        },
        error: function () {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
};

function consultar() {
    let anioInicio = $("#txtFechaIni").val();
    let anioFin = $("#txtFechaFin").val();
    let estadoA = $('#chbAbierto').is(':checked') ? 1 : 0;
    let estadoC = $('#chbCerrado').is(':checked') ? 1 : 0;
    let estadoX = $('#chbAnualdo').is(':checked') ? 1 : 0;

    if (anioInicio < 2023) {
        alert("La 'Fecha desde' no puede ser menor a 2023");
        return;
    }

    if (anioFin < 2023) {
        alert("La 'Fecha hasta' no puede ser menor a 2023");
        return;
    }

    if (anioFin < anioInicio) {
        alert("La 'Fecha hasta' no puede ser menor a la 'Fecha desde'");
        return;
    }

    if (estadoA == 0 && estadoC == 0 && estadoX == 0) {
        alert('Debe seleccionar al menos un estado');
        return;
    }

    buscar(anioInicio, anioFin, estadoA, estadoC, estadoX);
};

function Edit(cparcodi) {
    window.location.href = controler + "Edit?cparcodi=" + cparcodi;
}
function Register(cparcodi, cpaapanio, cpaapajuste, cpaaprevision) {
    let url = siteRoot + `CPPA/RegistroParametrosAjuste/index/?fecha=${cpaapanio}&ajuste=${cpaapajuste}&revision=${cpaaprevision}&idRevision=${cparcodi}`;
    window.location.href = url
}
function CopyParameter(cparcodi, cpaapanio, cpaapajuste, cparrevision) {
    window.location.href = controler + "CopyParameter?anio=" + cpaapanio + "&ajuste=" + cpaapajuste + "&revision=" + cparrevision + "&idRevision=" + cparcodi;
}
function ViewLog(cparcodi) {
    window.location.href = controler + "ViewLog?cparcodi=" + cparcodi;
}

function Annul(cparcodi, cpaapanio, cpaapajuste, cparrevision) {

    if (!confirm("¿Está seguro que desea anular la Revisión '" + cpaapanio + " - " + cpaapajuste + " - " + cparrevision + "'?")) {
        return;
    }

    $.ajax({
        type: 'POST',
        url: controler + 'Annul',
        data: {
            cparcodi: cparcodi
        },
        success: function (model) {
            if (model.sResultado == "1") {
                alert("Se cambió el estado de la Revisión a ‘Anulado’ satisfactoriamente.");
                window.location.href = controler + "Index";
            } else {
                alert("No se pudo anular la Revisión.");
            }
        },
        error: function () {
            alert("Lo sentimos, ha ocurrido un error.");
        }
    });
}

function getEstado(estadoA, estadoC, estadoX) {
    let strEstado = "";
    if (estadoA == 1) {
        strEstado = "'A'";
    }
    if (estadoC == 1) {
        if (strEstado.length > 0) {
            strEstado = strEstado + ",'C'";
        }
        else {
            strEstado = "'C'";
        }
    }
    if (estadoX == 1) {
        if (strEstado.length > 0) {
            strEstado = strEstado + ",'X'";
        }
        else {
            strEstado = "'X'";
        }
    }  
    return strEstado;
}


