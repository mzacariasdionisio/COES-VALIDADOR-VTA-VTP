//Declarando las variables 
let controlador = siteRoot + 'actualizacionmasiva/';


$(function () {

    

    $('#btnBuscar').click(function () {
        consultar();       
    });

    let today = new Date();
    let yyyy = today.getFullYear();
    let mm = today.getMonth() + 1;
    let dd = day_of_the_month(today);
    let maxFecha = dd + "/" + mm + "/" + yyyy;

    $('#fechaInicio').Zebra_DatePicker({
        show_clear_date: 0,
        direction: [false, maxFecha]
    });

    $('#fechaFin').Zebra_DatePicker({
        show_clear_date: 0,
        direction: [false, maxFecha]

    });

    $('#btnNuevo').click(function () {
        nuevaCargaMasiva();
    })


    consultar();
});

let consultar = function () {

    let fechaInicio = this.convertirCadenaFecha($('#fechaInicio').val());
    let fechaFin = this.convertirCadenaFecha($('#fechaFin').val());

    if (fechaFin < fechaInicio) {
        alert("La fecha Realizado desde no debe ser mayor que Realizado hasta");
        return;
    }
    setTimeout(function () {
        $.ajax({
            type: 'POST',
            url: controlador + 'ListaActualizacionMasivaLog',
            data: {               
                tipoUsoId: $('#cbTipoUso').val(),
                usuario: $('#nombreUsuario').val(),
                fechaInicio: $('#fechaInicio').val(),
                fechaFin: $('#fechaFin').val()
            },
            success: function (evt) {
                $('#lista').html(evt);
                $('#tablaListado').dataTable({
                    "iDisplayLength": 25,
                    language: {
                        info: 'Mostrando página _PAGE_ de _PAGES_',
                        infoEmpty: 'No hay registros disponibles',
                        infoFiltered: '(filtrado de _MAX_ registros totales)',
                        lengthMenu: 'Mostrar _MENU_ registros por página',
                        zeroRecords: 'No se encontró nada'
                    },
                });

                $('.dataTables_filter input').attr('maxLength', 50);
            },
            error: function () {
                mostrarMensaje('Ha ocurrido un error.');
            }
        });
    }, 100);
};


function mostrarMensaje(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-message');
}

let nuevaCargaMasiva = function () {
    window.location.href = controlador + "cargamasiva";
}

function convertirCadenaFecha(dateString) {
    if (dateString != '') {
        let d = dateString.split("/");
        let dat = new Date(d[2] + '/' + d[1] + '/' + d[0]);
        return dat;
    } else {
        return null;
    }
};

function day_of_the_month(d) {
    return (d.getDate() < 10 ? '0' : '') + d.getDate();
}


