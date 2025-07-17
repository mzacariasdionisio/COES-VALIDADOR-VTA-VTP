var levantamiento = "levantamiento/";
var controler = siteRoot + "transferencias/" + levantamiento;

//Funciones de busqueda
$(document).ready(function () {
    preload();
    $("#VersionB").prop("disabled", true);
    $("#Pericodi").change(function () {
        if ($("#Pericodi").val() != "") {
            var options = {};
            options.url = controler + "GetVersion";
            options.type = "POST";
            options.data = JSON.stringify({ pericodi: $("#Pericodi").val() });
            options.dataType = "json";
            options.contentType = "application/json";
            options.success = function (states) {
                //console.log(states);
                $("#VersionB").empty();
                $("#VersionB").removeAttr("disabled");
                $.each(states, function (k, v) {
                    var a = '<option value=' + v.RecaCodi + '>' + v.RecaNombre + '</option>';
                    $('#VersionB').append(a);
                    //console.log(a);
                });


            };
            options.error = function () { alert("Lo sentimos, no se encuentran registrada ninguna revisión"); };
            $.ajax(options);
        }
        else {
            $("#VersionB").empty();
            $("#VersionB").prop("disabled", true);
        }
    });

    $('#txtFecha').Zebra_DatePicker({
    });

    $('#btnBuscar').click(function () {
        buscar();
    });                                         

    buscar();
});

function preload() {
    if ($("#Pericodi").val() != "") {
        var options = {};
        options.url = controler + "GetVersion";
        options.type = "POST";
        options.data = JSON.stringify({ pericodi: $("#Pericodi").val() });
        options.dataType = "json";
        options.contentType = "application/json";
        options.success = function (states) {
            //console.log(states);
            $("#VersionB").empty();
            $("#VersionB").removeAttr("disabled");
            $.each(states, function (k, v) {
                var a = '<option value=' + v.RecaCodi + '>' + v.RecaNombre + '</option>';
                $('#VersionB').append(a);
                //console.log(a);
            });
        };
        options.error = function () { alert("Lo sentimos, no se encuentran registrada ninguna revisión"); };
        $.ajax(options);
    }
    else {
        $("#VersionB").empty();
        $("#VersionB").prop("disabled", true);
    }
}

function buscar() {
    var c;
    var vers;
    if ($("#Pericodi option:selected").val() == "") {
        iPeriCodi = 0;
    }
    else {
        c = $("#Pericodi option:selected").val();
        vers = $("#VersionB option:selected").val();
        $.ajax({
            type: 'POST',
            url: controler + "lista",
            data: { fecha: $('#txtFecha').val(), corrinf: $('[name="radio"]:radio:checked').val(), pericodi: c, vers: vers },
            success: function (evt) {
                $('#listado').html(evt);
                //addDeleteEvent();
                viewEvent();
                oTable = $('#tabla').dataTable({
                    "sPaginationType": "full_numbers",
                    "destroy": "true",
                    "aaSorting": [[0, "desc"]]
                });
            },
            error: function () {
                mostrarError();
            }
        });
    }
};

mostrarError = function () {
    alert("Lo sentimos, ha ocurrido un error inesperado");
}

addAntiForgeryToken = function (data) {
    data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
    return data;
};

//Funciones de vista detalle
var controler2 = siteRoot + "transferencias/" + levantamiento;

function viewEvent() {
    $('.view').click(function () {
        id = $(this).attr("id").split("_")[1];
        abrirPopup(id);
    });
};

abrirPopup = function (id) {

    $.ajax({
        type: 'POST',
        url: controler2 + "View/" + id,
        success: function (evt) {
            $('#popup').html(evt);

            setTimeout(function () {
                $('#popup').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);
        },
        error: function () {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

