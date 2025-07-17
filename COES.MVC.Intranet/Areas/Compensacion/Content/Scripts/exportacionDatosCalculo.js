var controlador = siteRoot + "compensacion/general/";

$( /**
   * Llamadas iniciales
   * @returns {} 
   */
   $(document).ready(function () {
       mostrarMensaje("Selecionar el periodo y la versión");

       $('#btnExportar').click(function () {
           if (ValidarVersion("Exportar")) {
               exportar('grupo1');
           }
       });

       $('#btnExportar2').click(function () {
           if (ValidarVersion("Exportar")) {
               exportar('grupo2');
           }
       });

       $("#pericodi").change(function () { ObtenerPeriodoCalculo(this.value, ''); });
       $("#checkTodos").change(function () {$("input:checkbox.grupo1").prop('checked', $(this).prop("checked"));
       });

       //Inicializamos la pantalla
       ObtenerPeriodoCalculo($("#pericodi").val(), '');
   }));

function mostrarExito(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-exito');
}

function mostrarError(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-error');
}

function mostrarAlerta(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-alert');
}

function mostrarMensaje(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-message');
}

function exportar(grupo) {
    var lista =[];
    var i = 0;
    
    $('input[type=checkbox]:checked').each(function () {
        if($(this).prop("class") == grupo) {
            i++;
             lista.push ($(this).prop("id"));
             //alert(lista);
        }
    })
   
    if (i >= 1) {
        $.ajax({
            type: 'POST',
            url: controlador + 'exportarDatosCalculo',
            data: {
                pecacodi: $("#pecacodi").val(),
                grupo: grupo,
                lista:  lista.join()
            },
            dataType: 'json',
            success: function (result) {
                if (result != -1) {
                    document.location.href = controlador + 'descargar?formato=' + 1 + '&file=' + result
                    mostrarMensaje("Exportación realizada");
                }
                else {
                    mostrarError('Ha ocurrido un error');
                }
            },
            error: function () {
                mostrarError('Ha ocurrido un error');
            }
        });
    }
   
}

function ObtenerPeriodoCalculo(valor, selected) {

    if (valor != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'obtenerPeriodoCalculo',
            data: {
                pericodi: valor
            },
            dataType: 'json',
            success: function (data) {
                var jsonData = JSON.parse(data);
                dwr.util.removeAllOptions("pecacodi");
                dwr.util.addOptions("pecacodi", jsonData, 'id', 'name');
                dwr.util.setValue("pecacodi", selected);
            },
            error: function () {
                mostrarError('Ha ocurrido un error');
            }
        });
    }
    else {
        dwr.util.removeAllOptions("pecacodi");
    }
}
function ValidarVersion(titulo_opcion) {
    if ($("#pecacodi").val() == "" || $("#pecacodi").val() == null) {
        mostrarAlerta('Opcion ' + titulo_opcion + ': Verificar la selección del periodo y la versión');
        
        return false;
    }
    else {
        return true;
    }
}
