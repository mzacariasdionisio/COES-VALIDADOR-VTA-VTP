var name = "Seguimiento/";
var controller = siteRoot + "DemandaPO/" + name;

$(document).ready(function () {
    $('#txtFecha').Zebra_DatePicker({
        format: 'm Y',
    });

    $('#selPunto').multipleSelect({
        filter: true,
        single: true,
        placeholder: 'Seleccione',
    });
    $('#selPuntoEjecutar').multipleSelect({
        filter: true,
        single: true,
        placeholder: 'Seleccione',
    });
    $('#selFechaConsulta').Zebra_DatePicker();

    $('#selFechaEjecutar').Zebra_DatePicker({
        format: 'm Y',
    });

    $('#btnConsulta').on('click', function () {
        consultarEstado();
    });

    $('#btnConsultarFiltrado').on('click', function () {
        pop = $('#popupConsultar').bPopup({
            easing: 'easeOutBack',
            speed: 350,
            transition: 'fadeIn',
            modalClose: false,            
        });
    });

    $('#btnEjecutarFiltrado').on('click', function () {
        pop = $('#popupEjecutar').bPopup({
            easing: 'easeOutBack',
            speed: 350,
            transition: 'fadeIn',
            modalClose: false,
        });
    });

    $('#popConsultar').on('click', function () {
        consultarFiltrado();
    });

    $('#popEjecutar').on('click', function () {
        ejecutarFiltrado();
    });

    $('.popExeTipo').on('click', function () {
        const e = $(this).val();
        var datepicker = $('#selFechaEjecutar').data('Zebra_DatePicker');

        if (e === 'dia') {
            datepicker.update({ format: 'd/m/Y', });
            datepicker.clear_date();
        }
        if (e === 'mes') {
            datepicker.update({ format: 'm Y', });
            datepicker.clear_date();
        }
    });
});

function consultarEstado() {
    $.ajax({
        type: 'POST',
        url: controller + 'ConsultarEstado',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            fecha: $('#txtFecha').val(),
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            const data = [
                {
                    tipo: 1,
                    id: "statusFiltro",
                    intervalos: result,
                },
            ];
            crearBarraEstados(data);            
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function consultarFiltrado() {
    $.ajax({
        type: 'POST',
        url: controller + 'ConsultarFiltrado',
        contentType: 'application/json; charset=utf-8',   
        data: JSON.stringify({
            punto: $('#selPunto').val(),
            fecha: $('#selFechaConsulta').val(),
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            console.log(result, 'result');
            if (result.valid === 1)
                window.location = controller + 'AbrirArchivo?formato=' + 3 + '&file=' + result.response;
            if (result.valid === -1)
                alert(result.response);
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function ejecutarFiltrado() {
    const selExeFecha = $('#selFechaEjecutar').val();
    const rdoTipoFecha = $('.popExeTipo:checked').val();
    const selPuntoEjecutar = $('#selPuntoEjecutar').val();

    if (!selExeFecha) {
        alert('Debe seleccionar una fecha para la ejecución');
        return false;
    }

    $.ajax({
        type: 'POST',
        url: controller + 'EjecutarFiltrado',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            tipo: rdoTipoFecha,
            fecha: selExeFecha,
            idPunto: selPuntoEjecutar,
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            alert(result);
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function crearBarraEstados(data) {
    data.forEach(e => {
        const elem = $(`#${e.id}`);
        elem.html("");
        e.intervalos
            .forEach((item, index) => {
                let className = 'status-bar-item-filter';

                if (item === 0)
                    className += ' status-pending';
                if (item === 1)
                    className += ' status-done';
                if (item === 2)
                    className += ' status-error';

                const str = `<div id="${e.tipo}_${(index + 1)}"`
                    + `class="${className}"`
                    + `></div>`;
                elem.append(str);
            });
    });
}