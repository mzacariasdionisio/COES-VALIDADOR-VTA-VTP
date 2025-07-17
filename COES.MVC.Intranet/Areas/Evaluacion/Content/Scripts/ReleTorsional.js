//Declarando las variables 
let controlador = siteRoot + 'Rele/';
let hot;
let hotOptions;
let evtHot;

$(function () {

    $('#btnConsultar').click(function () {
        consultarReleTorsional();
    });

    $('#fSubestacion').change(function () {

        let arraySE = $('#fSubestacion').multipleSelect('getSelects');
        const subestacion = arraySE[0];

        if (subestacion != "0") {
            consultarCelda(subestacion);
        } else {
            let seleccion = $('#fCelda');
            seleccion.empty();
            seleccion.append('<option value="0">TODOS</option>');
        }
    });

    inputSoloNumeros('fCodigoId');

    $('#btnExportar').click(function () {
        exportar();
    });

    $('#fSubestacion').multipleSelect({
        width: '150px',
        filter: true,
        single: true
    });

    $('#fEmpresa').multipleSelect({
        width: '150px',
        filter: true,
        single: true
    });

    $('#fSubestacion').multipleSelect('setSelects', ['0']);
    $('#fEmpresa').multipleSelect('setSelects', ['0']);

    consultar();

});

function consultarReleTorsional() {
    consultar();
}

$.fn.dataTable.ext.type.order['date-dd-mm-yyyy-pre'] = function (date) {

    if (!date) return 0;

    let parts = date.trim().split(' ');
    let dateParts = parts[0].split('/');
    let timeParts = parts[1] ? parts[1].split(':') : ['00', '00'];

    let hour = parseInt(timeParts[0], 10);
    let minute = parseInt(timeParts[1], 10);
    console.log(new Date(dateParts[2], dateParts[1] - 1, dateParts[0], hour, minute).getTime())
    return new Date(dateParts[2], dateParts[1] - 1, dateParts[0], hour, minute).getTime();

};
let consultar = function () {

    let estado = "";

    let codigoId = $("#fCodigoId").val();
    let codigo = $("#fCodigo").val();
    let subestacion = $("#fSubestacion").val();
    let celda = $("#fCelda").val();
    let empresa = $("#fEmpresa").val();
    let area = $("#fArea").val();

    if ($("#fEstado").val() == 0) {
        estado = "";
    } else {
        estado = $("#fEstado").val();
    }
    setTimeout(function () {
        $.ajax({
            type: 'POST',
            url: controlador + 'ListaReleTorsional?codigoId=' + codigoId + "&codigo=" + codigo + "&subestacion=" + subestacion + "&celda=" + celda + "&empresa=" + empresa + "&area=" + area + "&estado=" + estado,
            success: function (evt) {
                $('#lista').html(evt);
                $('#tablaListado').dataTable({
                    "iDisplayLength": 25,
                    columnDefs: [
                        { type: 'date-dd-mm-yyyy', targets: 9 }
                    ],
                    language: {
                        info: 'Mostrando página _PAGE_ de _PAGES_',
                        infoEmpty: 'No hay registros disponibles',
                        infoFiltered: '(filtrado de _MAX_ registros totales)',
                        lengthMenu: 'Mostrar _MENU_ registros por página',
                        zeroRecords: 'No se encontró nada'
                    },
                    order: [[5, 'asc'], [2, 'asc']]
                });

                $('.dataTables_filter input').attr('maxLength', 50);
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    }, 100);
};

let mostrarError = function () {
    $('#mensaje').removeClass("action-exito");
    $('#mensaje').addClass("action-error");
    $('#mensaje').text("Ha ocurrido un error");
    $('#mensaje').css("display", "block");
};

let consultarCelda = function (idSubEstacion) {
    setTimeout(function () {
        $.ajax({
            type: 'POST',
            url: controlador + 'ListaCelda',
            data: { idSubEstacion: idSubEstacion },
            success: function (evt) {
                let seleccion = $('#fCelda');
                seleccion.empty();
                seleccion.append('<option value="0">TODOS</option>');
                $.each(evt, function (index, item) {
                    seleccion.append('<option value="' + item.Equicodi + '">' + item.Equinomb + '</option>');
                });
            },
            error: function () {
                mostrarMensaje('Ha ocurrido un error.');
            }
        });
    }, 100);
};

function consultarEquipo(codigoId) {
    window.location.href = siteRoot + "Evaluacion/Transversal/IndexConsultarEquipo?codigoId=" + codigoId + "&pathReturn=Evaluacion-Rele-IndexReleTorsional";
};

function historialCambio(codigoId) {
    window.location.href = siteRoot + "Evaluacion/Transversal/IndexHistorialCambio?codigoId=" + codigoId + "&pathReturn=Evaluacion-Rele-IndexReleTorsional";
};

let exportar = function () {

    let estado = "";

    let codigoId = $("#fCodigoId").val();
    let codigo = $("#fCodigo").val();
    let subestacion = $("#fSubestacion").val();
    let celda = $("#fCelda").val();
    let empresa = $("#fEmpresa").val();
    let area = $("#fArea").val();

    if ($("#fEstado").val() == 0) {
        estado = "";
    } else {
        estado = $("#fEstado").val();
    }

    setTimeout(function () {

        $.ajax({
            type: 'POST',
            url: controlador + 'ExportarReleTorsional?codigoId=' + codigoId + "&codigo=" + codigo + "&subestacion=" + subestacion + "&celda=" + celda + "&empresa=" + empresa + "&area=" + area + "&estado=" + estado,
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    window.location = controlador + "AbrirArchivo?file=" + evt.NombreArchivo;
                }
                else {
                    alert(evt.StrMensaje);
                }
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    }, 100);
};