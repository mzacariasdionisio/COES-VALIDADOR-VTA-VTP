var controlador = siteRoot + 'formulas/configuracion/'

$(function () {
       
    $('#btnBuscar').click(function () {
        buscar();
    });

    $('#btnNuevo').click(function () {
        nuevo();
    });
  
    inicio();   
});

inicio = function (){
    buscar();
}

buscar = function () {
    $.ajax({
        type: "POST",
        url: controlador + "lista",
        data:{
             areaOperativa: $('#cbAreaOperativa').val()
        },
        success: function (evt) {
            $('#listado').html(evt);
            oTable = $('#tabla').dataTable({
                "sPaginationType": "full_numbers",
                "aaSorting": [[0, "desc"]],
                "destroy": "true"
            });
        },
        error: function () {
            mostrarError();
        }
    });
}

nuevo = function (){  
    document.location.href = controlador + "nuevo?id=0";
}

editar = function (id){
    document.location.href = controlador + "nuevo?id=" + id;
}

eliminar = function (id) {
    if (confirm('¿Está seguro de eliminar este registro?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'eliminar',
            data: {
                id: id
            },
            dataType: 'json',
            cache: false,
            success: function (resultado) {
                if (resultado == 1) {
                    buscar();
                }
                else {
                    mostrarError();
                }
            },
            error: function () {
                mostrarError();
            }
        });
    }
}

mostrarMensaje = function (mensaje){
    alert(mensaje);
}

mostrarError = function (){
    alert('Ha ocurrido un error.');
}

