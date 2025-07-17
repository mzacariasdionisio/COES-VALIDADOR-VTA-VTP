var controler = siteRoot + "CPPA/AjustePresupuestal/";
const imageRoot = siteRoot + "Content/Images/";

$(document).ready(function () {
    $('#cbAnio').change(function () {
        cargarAjustes($(this).val());
    });

    $('#cbAjuste').change(function () {
        cargarRevisiones($('#cbAnio').val(), $(this).val());
    })

    $('#btnCopiar').on('click', function () {
        copiarDatos();
    });
});

function copiarDatos() {

    var tabla = document.getElementById('tabla');
    //Hasta
    var anioHasta = tabla.rows[1].cells[1].innerText;
    var ajusteHasta = tabla.rows[1].cells[2].innerText;
    var revisionHasta = $('#IdRevision').val();
    //Desde
    var anioDesde = $('#cbAnio').val();
    var ajusteDesde = $('#cbAjuste').val();
    var revisionDesde = $('#cbRevision').val();

    if (anioDesde && ajusteDesde && revisionDesde) {
        $.ajax({
            type: 'POST',
            url: controler + 'CopiarDatos',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({
                revisionHasta: revisionHasta,
                anioHasta: anioHasta,
                ajusteHasta: ajusteHasta,
                revisionDesde: revisionDesde,
                anioDesde: anioDesde,
                ajusteDesde: ajusteDesde
            }),
            datatype: 'json',
            traditional: true,
            success: function (result) {
                if (result.sResultado === '-1') {
                    SetMessage('#message',
                        result.sMensaje,
                        'success', true);
                } else {
                    SetMessage('#message',
                        result.sMensaje,
                        result.sTipo, true);
                }
            },
            error: function () {
                alert("Ha ocurrido un problema...");
            }
        });
    }
    else {
        SetMessage('#message',
            "En Desde se debe seleccionar un Año, Ajuste y Revision....",
            'error', true);

    }


}

function cargarAjustes(year) {
    console.log(year, 'year');
    console.log(listRevision, 'listRevision');
    if (year) {
        $('#cbAjuste').prop('disabled', false).empty().append('<option value="">Seleccione un ajuste</option>');
        $('#cbRevision').prop('disabled', true).empty().append('<option value="">Seleccione una revisión</option>');

        listRevision
            .filter(x => x.Cpaapanio === parseInt(year))
            .map(x => x.Cpaapajuste)
            .filter((value, index, self) => self.indexOf(value) === index) // Eliminar duplicados
            .forEach((cpaapajuste) => { $('#cbAjuste').append('<option value="' + cpaapajuste + '">' + cpaapajuste + '</option>'); });
    }
    else {
        $('#cbAjuste').prop('disabled', true).empty().append('<option value="">Seleccione un ajuste</option>');
        $('#cbRevision').prop('disabled', true).empty().append('<option value="">Seleccione una revisión</option>');
    }
}

function cargarRevisiones(year, fit) {
    if (year && fit) {
        $('#cbRevision').prop('disabled', false).empty().append('<option value="">Seleccione una revisión</option>');

        listRevision
            .filter(x => x.Cpaapanio === parseInt(year) && x.Cpaapajuste === fit)
            .forEach((revision) => {
                let estado = revision.Cparestado == 'A' ? '' : ' [' + revision.Cparestado + ']';
                $('#cbRevision').append('<option value="' + revision.Cparcodi + '">' + revision.Cparrevision + estado + '</option>');
            });
    }
    else {
        $('#cbRevision').prop('disabled', true).empty().append('<option value="">Seleccione una revisión</option>');
    }
}

function SetMessage(container, msg, tpo, del) {//{Contenedor, mensaje(string), tipoMensaje(string), delay(bool)}
    var new_class = "msg-" + tpo;//Identifica la nueva clase css
    $(container).removeClass($(container).attr('class'));//Quita la clase css previa
    $(container).addClass(new_class);
    $(container).html(msg);//Carga el mensaje
    //$(container).show();

    //Focus to message
    $('html, body').animate({ scrollTop: 0 }, 5);

    //Mensaje con delay o no
    if (del) $(container).show(0).delay(5000).hide(0);//5 Segundos
    else $(container).show();
}