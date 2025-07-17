var controlador = siteRoot + 'Monitoreo/Parametro/';
var ancho = 1200;
$(function () {
    $('#btnNuevo').click(function () {
        mostrarView();
    });

    $('#txtFecha').Zebra_DatePicker({
        format: 'm Y '
    });

    $('#btnSave').click(function () {

        saveParametro();

    });

    consultarPivotal();
});

function valida(e) {
    tecla = (document.all) ? e.keyCode : e.which;

    //Tecla de retroceso para borrar, siempre la permite
    if (tecla == 8) {
        return true;
    }

    // Patron de entrada, en este caso solo acepta numeros y punto
    patron = /[0-9-.]/;
    tecla_final = String.fromCharCode(tecla);
    return patron.test(tecla_final);
}

function mostrarView() {
    setTimeout(function () {
        $('#idPopupNew').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    }, 250);
}

function saveParametro() {
    var fecha = $('#txtFecha').val();
    var pivotal = $('#txtPivotal').val();

    if (fecha != "" && pivotal != "") {
        $.ajax({
            type: 'POST',
            async: true,
            url: controlador + 'SaveParametrosPivotal',
            data: {
                fecha: fecha,
                pivotal: pivotal,

            },
            success: function (result) {

                if (result == 1) {
                    alert("Se creo la generación correctamente ");
                    $('#txtPivotal').val('');
                    cerrarPopupNew();
                    consultarPivotal();

                } else {
                    alert("Este periodo ya esta registrado");
                }
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    } else {
        alert("No puede dejar valores en blanco");
    }
}

function consultarPivotal() {

    $.ajax({
        type: 'POST',
        async: true,
        url: controlador + 'ConsultarOfertaPivotal',
        success: function (aData) {
            $('#listado').html(aData.Resultado);
            var anchoReporte = $('#reporte').width();
            $("#resultado").css("width", (anchoReporte > ancho ? ancho : anchoReporte) + "px");
            $('#reporte').dataTable({
                "scrollX": true,
                "scrollY": "780px",
                "scrollCollapse": true,
                "sDom": 't',
                "ordering": false,
                paging: false,
                fixedColumns: {
                    leftColumns: 1
                }
            });
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function cerrarPopupNew() {

    $('#idPopupNew').bPopup().close();
}
