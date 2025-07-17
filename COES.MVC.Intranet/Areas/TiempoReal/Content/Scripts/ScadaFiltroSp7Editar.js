var controlador = siteRoot + 'tiemporeal/scadafiltrosp7/';


$(function() {

    $('#btnCancelar').click(function() {
        document.location.href = controlador;
    });

    $('#btnCancelar2').click(function() {
        document.location.href = controlador;
    });

    $(document).ready(function() {


        if ($('#hfAccion').val() == 0) {
            $('#btnGrabar').hide();
            $('#btnGrabar2').hide();
            $('#btnCancelar').hide();
            $('#btnCancelar2').hide();
            $('#btnAgregar').hide();

        }

    });

    $('#btnGrabar').click(function() {
        grabar();
    });

    $('#btnGrabar2').click(function() {
        grabar();
    });

    $('#cbZona').val(0);

    $('#cbZona').change(function() {
        cargarCanalPorZona($('#cbZona').val());
    });

    $('#btnAgregar').click(function() {
        agregarPunto();
    });


});


agregarPunto = function() {

    mostrarExito();

    var punto = $('#cbCanal').val();
        
    if (punto != "" && punto != null) {

        //valida si punto ya fue ingresado

        var items = "";
        var count = 0;
        var valida = true;

        $('#tbItems>tbody tr').each(function (i) {
            $punto = $(this).find('#hfCanal');
            var item = $punto.val();

            if (punto == item) {
                mostrarAlerta('No se pueden repetir Canales...');
                valida = false;
            }

            items = items +","+ item;
            
            count++;
        });

        if (!valida)
            return;

        //ingresa datos

        var descripcion = $('#cbCanal option:selected').text();
        var longitud = $('#tbItems> tbody tr').length + 1;

        $('#tbItems> tbody').append(
            '<tr>' +
            '   <td>' +
            '        <span>' + descripcion + '</span>' +
            '        <input type="hidden" id="hfCanal" value="' + punto + '" />' +
            '   </td>' +
            '   <td style="text-align:center">' +
            '       <img src="' + siteRoot + 'Content/Images/btn-cancel.png" alt="" onclick="$(this).parent().parent().remove();" style="cursor:pointer" />' +
            '   </td>' +
            '</tr>'
        );


    }
    else {
        mostrarAlerta('Seleccione Canal...');
    }
}


mostrarAlerta = function(mensaje) {

    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-alert');
    $('#mensaje').html(mensaje);
}


mostrarExito = function() {

    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-exito');
    $('#mensaje').html('La operación se realizó con éxito.');
}


cargarCanalPorZona = function(zonacodi) {

    $.ajax({
        type: 'POST',
        url: controlador + "listacanalporzona",
        data: {
            zonaCodi: $('#cbZona').val()
        },
        dataType: 'json',
        cache: false,
        success: function(evt) {

            var _len = evt.length;
            var cad1 = _len + "\r\n";

            $('#cbCanal').empty();

            for (var i = 0; i < _len; i++) {
                post = evt[i];
                var select = document.getElementById("cbCanal");
                select.options[select.options.length] = new Option(post.Canalnomb, post.Canalcodi);
            }
            
        },
        error: function(xhr, textStatus, exceptionThrown) {
            mostrarError();
        }
    });

}


validarRegistro = function() {

    var mensaje = "<ul>";
    var flag = true;

    //completa codigos configurados
    var items = "";
    var count = 0;

    var nombreFiltro = $('#txtFiltroNomb').val();

    if (nombreFiltro == "") {
        mensaje = mensaje + "Debe ingresar nombre de filtro...";
        flag = false;
    }

    $('#tbItems>tbody tr').each(function(i) {
        $punto = $(this).find('#hfCanal');
        var item = $punto.val();

        items = items + "," + item;
        count++;
    });


    if (count == 0) {
        mensaje = mensaje + "Debe seleccionar Canales...";
        flag = false;
    } else {
        $('#hfFiltroCanal').val(items);

    }


    if (flag) mensaje = "";
    return mensaje;

}


grabar = function () {

    var mensaje = validarRegistro();

    if (mensaje == "") {

        $.ajax({
            type: 'POST',
            url: controlador + "grabar",
            dataType: 'json',
            data: $('#form').serialize(),
            success: function(result) {
                if (result != "-1") {

                    mostrarExito();
                    $('#hfFiltroCodi').val(result);
                    document.location.href = controlador;

                } else {
                    mostrarError();
                }
            },
            error: function() {
                mostrarError();
            }
        });
    } else {
        mostrarAlerta(mensaje);
    }
}

mostrarError = function () {

    alert("Ha ocurrido un error");

}

