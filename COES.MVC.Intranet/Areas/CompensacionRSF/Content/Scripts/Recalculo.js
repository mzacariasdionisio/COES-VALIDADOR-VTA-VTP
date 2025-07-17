var sControlador = siteRoot + "CompensacionRSF/recalculo/";


$(document).ready(function () {
    buscar();
    $('#btnNuevo').click(function () {
        nuevo();
    });

    $("#EntidadRecalculo_Pericodi").change(function () {
        console.log("3333");
        if ($("#EntidadRecalculo_Pericodi").val() != "") {
            var options = {};
            options.url = sControlador + "GetVersion";
            options.type = "POST";
            options.data = JSON.stringify({ pericodi: $("#EntidadRecalculo_Pericodi").val() });
            options.dataType = "json";
            options.contentType = "application/json";
            options.success = function (modelo) {
                //console.log(modelo);
                $("#EntidadRecalculo_Recacodi").empty();
                $("#EntidadRecalculo_Recacodi").removeAttr("disabled");

                $("#EntidadRecalculo_Vcrdsrcodi").empty();
                $("#EntidadRecalculo_Vcrdsrcodi").removeAttr("disabled");

                $("#EntidadRecalculo_Vcrinccodi").empty();
                $("#EntidadRecalculo_Vcrinccodi").removeAttr("disabled");

                $.each(modelo.ListaTrnRecalculo, function (k, v) {
                    var option = '<option value=' + v.RecaCodi + '>' + v.RecaNombre + '</option>';
                    $('#EntidadRecalculo_Recacodi').append(option);
                    console.log(option);
                })

                $.each(modelo.ListaVcrSuDeRns, function (k, v) {
                    var option = '<option value=' + v.Vcrdsrcodi + '>' + v.Vcrdsrnombre + '</option>';
                    $('#EntidadRecalculo_Vcrdsrcodi').append(option);
                    //console.log(option);
                })

                $.each(modelo.ListaIncumplimiento, function (k, v) {
                    var option = '<option value=' + v.Vcrinccodi + '>' + v.Vcrincnombre + '</option>';
                    $('#EntidadRecalculo_Vcrinccodi').append(option);
                    //console.log(option);
                })

            };
            options.error = function () { alert("Lo sentimos, no se encuentran registrada ninguna revisión"); };
            $.ajax(options);
        }
    });
});

buscar = function () {
    $.ajax({
        type: 'POST',
        url: sControlador + "lista",
        success: function (evt) {
            $('#listado').html(evt);
            addDeleteEvent();
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
};

nuevo = function () {

    window.location.href = sControlador + "new";

}

//Funciones de eliminado de registro
addDeleteEvent = function () {
    $(".delete").click(function (e) {
        e.preventDefault();
        pericodi = $(this).attr("id").split("_")[1];
        vcrecacodi = $(this).attr("id").split("_")[2];
        estado = $(this).attr("id").split("_")[3];
        if (estado !== "Cerrado") {
            if (confirm("¿Desea eliminar la información seleccionada? Tener en consideración que se eliminará los registros asociados a esta!")) {
                $.ajax({
                    type: "post",
                    dataType: "text",
                    url: sControlador + "Delete",
                    data: addAntiForgeryToken({ pericodi: pericodi, vcrecacodi: vcrecacodi }),
                    success: function (resultado) {
                        console.log(resultado);
                        if (resultado == "true") {
                            $("#fila_" + pericodi + "_" + vcrecacodi + "_" + estado).remove();
                            mostrarExito("Se ha eliminado correctamente el registro");
                        }
                        else {
                            mostrarError("No se ha logrado eliminar el registro: ");
                        }
                    }
                });
            }
        } else {
            alert("Registro en estado CERRADO, no se puede ser eliminado.");
        }
        
    });
};

//Funciones de vista detalle
viewEvent = function () {
    $('.view').click(function () {
        pericodi = $(this).attr("id").split("_")[1];
        vcrecacodi = $(this).attr("id").split("_")[2];
        abrirPopup(pericodi, vcrecacodi);
    });
};

abrirPopup = function (pericodi, vcrecacodi) {
    $.ajax({
        type: 'POST',
        url: sControlador + "View",
        data: { pericodi: pericodi, vcrecacodi: vcrecacodi },
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
            mostrarError("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}
