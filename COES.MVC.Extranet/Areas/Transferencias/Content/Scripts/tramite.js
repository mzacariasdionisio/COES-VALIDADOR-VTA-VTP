var controler = siteRoot + "transferencias/tramite/";

//Funciones de busqueda
$(document).ready(function () {
  
    preload();
    $("#Version").prop("disabled", true);
    $("#Pericodi").change(function () {
        if ($("#Pericodi").val() != "") {
            var options = {};
            options.url = controler + "GetVersion";
            options.type = "POST";
            options.data = JSON.stringify({ iPeriCodi: $("#Pericodi").val() });
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

    buscar();
  
});


function preload() {
    if ($("#Pericodi").val() != "") {
        var options = {};
        options.url = controler + "GetVersion";
        options.type = "POST";
        options.data = JSON.stringify({ iPeriCodi: $("#Pericodi").val() });
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
}

function buscar() {
    var iPeriCodi;

    if ($("#Pericodi option:selected").val() == "") {
        iPeriCodi = 0;
    } else {
        iPeriCodi = $("#Pericodi option:selected").val();
        $.ajax({
            type: 'POST',
            url: controler + "lista",
            data: { iPeriCodi: iPeriCodi, iTrmVersion: $("#Version option:selected").val() },
            success: function (evt) {
                $('#listado').html(evt);
                oTable = $('#tabla').dataTable({
                    "sPaginationType": "full_numbers",
                    "destroy": true,
                    "scrollY": 430,
                    "scrollX": true,
                    "aaSorting": [[0, "desc"]]
                });

                $('#btnNuevo').click(function () {
                    nuevo();
                });
                ViewEvent();
            },
            error: function () {
                mostrarError();
            }
        });
    }
};


function nuevo() {
    $.ajax({
        type: 'POST',
        url: controler + "new",
        data: { iPeriCodi: $("#Pericodi option:selected").val(), iTrmVersion: $("#Version").val() },
        success: function (evt){
            $('#ast').html(evt);
        },
        error: function () {
            mostrarError();
        }
    });
}

mostrarError = function () {
    alert("Lo sentimos, se ha producido un error interno");
}


AddAntiForgeryToken = function (data) {
    data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
    return data;
};

//Funciones de vista detalle
function ViewEvent() {
    $('.view').click(function () {
        id = $(this).attr("id").split("_")[1];
        abrirPopup(id);
    });
};

abrirPopup = function (id) {

    $.ajax({
        type: 'POST',
        url: controler + "View/" + id,
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

seleccionarEmpresa = function () {
    $.ajax({
        type: 'POST',
        url: controler + "EscogerEmpresa",
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