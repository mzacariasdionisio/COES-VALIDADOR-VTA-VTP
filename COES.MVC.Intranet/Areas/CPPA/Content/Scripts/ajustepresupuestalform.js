const controler = siteRoot + "CPPA/AjustePresupuestal/";
const imageRoot = siteRoot + "Content/Images/";

$(document).ready(function () {

    $('#mensaje').removeClass();
    $('#mensaje').html("");
    let oldAnio = $('#anioForm').val();

    $('#anioForm').Zebra_DatePicker({
        format: 'Y',
        view: 'years',
        direction: ['2025', false],
        onSelect: function (anio) {
            if (anio < 2025) {
                alert("El 'Año' no puede ser menor a 2025.");
                $('#anioForm').val(oldAnio);
                return;
            }
            oldAnio = anio;
            let ajuste = $('select[id=ajusteForm]').val();
            obtenerRevision(anio, ajuste);
        }
    });

    $('#ajusteForm').change(function () {
        let anio = $('#anioForm').val();
        let ajuste = $('select[id=ajusteForm]').val();
        obtenerRevision(anio, ajuste);
    });

    $('#btnGuardarNew').click(function (event) {
        event.preventDefault();
        $('#mensaje').removeClass();
        $('#mensaje').html("");
        createRevision();
    });

    $('#btnGuardarUpdate').click(function (event) {
        event.preventDefault();
        $('#mensaje').removeClass();
        $('#mensaje').html("");
        updateRevision();
    });

});

function obtenerRevision(anio, ajuste) {
    $.ajax({
        type: 'POST',
        url: controler + 'GetNewRevision',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            cpaapanio: anio, 
            cpaapajuste: ajuste,
        }),
        dataType: 'json',
        cache: false,
        success: function (model) {
            if (model.sResultado == "1") {
                $('#anioForm').val(model.Revision.Cpaapanio);
                $('#ajusteForm').val(model.Revision.Cpaapajuste);
                $('#revisionForm').val(model.Revision.Cparrevision);
                $('select[id=estadoForm]').val([model.Revision.Cparestado]);
                $('#cmpmpoForm').val(model.Revision.Cparcmpmpo);
            } else {
                alert("No se pudo obtener la nueva Revisión.");
                mostrarErrorConMensaje("No se pudo obtener la nueva Revisión.");
            }
        },
        error: function () {
            alert("Lo sentimos, ha ocurrido un error.");
            mostrarErrorConMensaje("Lo sentimos, ha ocurrido un error.");
        }
    });
}

function createRevision() {
    $.ajax({
        type: 'POST',
        url: controler + 'Save',
        //contentType: 'application/json; charset=utf-8', //Cuando se usa serialize() del form., esto no debe estar habilitado
        data: $('#frmAjustePresupuestal').serialize(), 
        dataType: 'json',
        cache: false,
        success: function (model) {
            if (model.sResultado == "1") {
                alert("Se guardó la revisión satisfactoriamente.");
                window.location.href = controler + "Index";
            } else {
                alert(model.sMensaje);
                mostrarErrorConMensaje(model.sMensaje);
            }
        },
        error: function () {
            alert("Lo sentimos, ha ocurrido un error.");
            mostrarErrorConMensaje("Lo sentimos, ha ocurrido un error.");
        }
    });
}

function updateRevision() {
    $.ajax({
        type: 'POST',
        url: controler + 'Update',
        //contentType: 'application/json; charset=utf-8', //Cuando se usa serialize() del form., esto no debe estar habilitado
        data: $('#frmAjustePresupuestal').serialize(),
        dataType: 'json',
        cache: false,
        success: function (model) {
            if (model.sResultado == "1") {
                alert("Se guardó la revisión satisfactoriamente.");
                window.location.href = controler + "Index";
            } else {
                alert(model.sMensaje);
                mostrarErrorConMensaje(model.sMensaje);
            }
        },
        error: function () {
            alert("Lo sentimos, ha ocurrido un error.");
            mostrarErrorConMensaje("Lo sentimos, ha ocurrido un error.");
        }
    });
}

mostrarExitoOperacion = function () {
    //$('#mensaje').addClass("action-exito"); //verde
    $('#mensaje').removeClass("");
    $('#mensaje').addClass("action-message"); //azul
    $('#mensaje').text("La operación se realizó con exito...!");
    $('#mensaje').css("display", "block");
};

mostrarErrorConMensaje = function (msg) {
    $('#mensaje').removeClass();
    $('#mensaje').html(msg);
    $('#mensaje').addClass("action-error"); //rojo
};

