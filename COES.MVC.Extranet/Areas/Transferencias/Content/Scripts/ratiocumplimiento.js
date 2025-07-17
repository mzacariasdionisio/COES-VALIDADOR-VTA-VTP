var ratiocumplimiento = "ratiocumplimiento/";
var controler = siteRoot + "transferencias/" + ratiocumplimiento;

//Funciones de busqueda
$(document).ready(function () {


    $("#Version").prop("disabled", true);
    $("#Pericodi").change(function () {
        if ($("#Pericodi").val() != "") {
            var options = {};
            options.url = controler + "GetVersion";
            options.type = "POST";
            options.data = JSON.stringify({ pericodi: $("#Pericodi").val() });
            options.dataType = "json";
            options.contentType = "application/json";
            options.success = function (states) {
                console.log(states);
                $("#Version").empty();
                $("#Version").removeAttr("disabled");
                $.each(states, function (k, v) {
                    var a = '<option value=' + v.RecaCodi + '>' + v.RecaNombre + '</option>';
                    $('#Version').append(a);
                    console.log(a);
                });


            };
            options.error = function () { alert("Lo sentimos, no se encuentran registrada ninguna revisión"); };
            $.ajax(options);
        }
        else {
            $("#Version").empty();
            $("#Version").prop("disabled", true);
        }
    });


    $('#btnBuscar').click(function () {

        buscar();
    });

    $('#btnGenerarExcel').click(function () {
        generarExcel();
    });

});


function buscar() {
    var c;
    var p;

    if ($("#Pericodi option:selected").val() == "") {

        alert("Seleccione Perido")
    }
    else {
        if ($("#Tipoemprcodi option:selected").val() == "")
        {
            alert("Seleccione Tipo Empresa")
        }
        else
            {
        

        p = $("#Pericodi option:selected").val();

       
            c = $("#Tipoemprcodi option:selected").val();
    

        $.ajax({
            type: 'POST',
            url: controler + "lista",
            data: { tipoemprcodi: c, pericodi: p, vers: $("#Version").val() },
            success: function (evt) {
                $('#listado').html(evt);
                //add_deleteEvent();
                ViewEvent();
                oTable = $('#tabla').dataTable({
                    "sPaginationType": "full_numbers",
                    "destroy": "true"
                });
            },
            error: function () {
                mostrarError();
            }
        });

        }

    }






};

mostrarError = function () {
    alert("Lo sentimos, ha ocurrido un error inesperado");
}

generarExcel = function () {

    if ($("#Pericodi option:selected").val() == "") {

        alert("Seleccione Perido")
    }
    else {


        $.ajax({
            type: 'POST',
            url: controler + 'generarexcel',
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    window.location = controler + 'abrirexcel';
                }
                if (result == -1) {
                    alert("Lo sentimos, se ha producido un error");
                }
            },
            error: function () {
                alert("Lo sentimos, se ha producido un error");
            }
        });

    }
}

AddAntiForgeryToken = function (data) {
    data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
    return data;
};

//Funciones de vista detalle
var controler2 = siteRoot + "transferencias/" + ratiocumplimiento;

function ViewEvent() {
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


