var controlador = siteRoot + 'campanias/periodo/';
var dataCheck = [];
var catalogoEstado = "1";
var idEliminacion = 0;
let isLoading = false;

$(function () {

    $('#btnConsultar').on('click', function () {
        limpiarMensaje('mensaje');
        consultar();
    });

    $('#btnNuevo').on('click', function () {
        limpiarMensaje('mensaje');
        registrar();

    });

    $("#btnManualUsuario").click(function () {
        window.location = controlador + 'DescargarManualUsuario';
    });

    cargarAnios();

    cargarPeriodos();

    cargarCatalogoEstado(catalogoEstado, '#cbEstado');
    cargarEliminar();
});

cargarAnios = function () {

    var d = new Date();
    var n = parseInt(d.getFullYear());
    var anios = n + 20;

    for (var i = n; i <= anios; i++)
        $("#cbAnioConsulta").append("<option value='"+i+"'>" + i + "</option>"); 
  
}


cargarPeriodos = function () {
    $.ajax({
        type: 'GET',
        url: controlador + 'listar',
        success: function (evt) {
            $('#listado').html(evt);

        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}



consultar = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'buscar',
        data: {
            anio: $('#cbAnioConsulta').val(),
            estado: $('#cbEstado').val()
        },
        success: function (evt) {
            $('#listado').html(evt);
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}


registrar = function () {

    $.ajax({
        type: 'GET',
        url: controlador + 'registrar',
        success: function (evt) {
            $('#contenidoEdicion').html(evt);

            $('#popupEdicion').bPopup({
                autoClose: false,
            });

            $('#txtFechaInicial').Zebra_DatePicker({
                format: 'd/m/Y',
                onSelect: function () {
                    calcularFechasAtras();
                    calcularFechasAdelante();
                }
            });

            $('#txtFechaFinal').Zebra_DatePicker({
                format: 'd/m/Y',
            });

            $('#txtHora').Zebra_DatePicker({
                format: "H:i",
            });

            $('#txtAnioAtras').on("keyup", function () { calcularFechasAtras(); });

            $('#txtAnioAdelante').on("keyup", function () { calcularFechasAdelante(); });

            $('#btnGrabar').on("click", function () {
                grabar();
            });
            $('#btnCancelarPeriodo').on("click", function () {
                $('#popupEdicion').bPopup().close();
            });

            $('#tab-container').easytabs({
                animate: false,
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}

editar = function (id) {
    limpiarMensaje('mensaje');
    $.ajax({
        type: 'POST',
        url: controlador + 'editar',
        data: {
            pericodi:id,
        } ,
        success: function (evt) {
            $('#contenidoEdicion').html(evt);
            console.log(fichasSeleccionadas);
            $('#popupEdicion').bPopup({
                autoClose: false,
            });

            $('#txtFechaInicial').Zebra_DatePicker({
                format: 'd/m/Y',
                onSelect: function () {
                    calcularFechasAtras();
                    calcularFechasAdelante();
                }
            });

            $('#txtFechaFinal').Zebra_DatePicker({
                format: 'd/m/Y',
            });

            $('#txtHora').Zebra_DatePicker({
                format: "H:i",
            });

            $('#txtFechaInicial').on("change", function () {
                calcularFechasAtras();
                calcularFechasAdelante();
            });
            $('#txtAnioAtras').on("keyup", function () { calcularFechasAtras(); });

            $('#txtAnioAdelante').on("keyup", function () { calcularFechasAdelante(); });

            $('#btnActualizar').on("click", function () {
                actualizar();
            });

            $('#btnCancelarPeriodo').on("click", function () {
                $('#popupEdicion').bPopup().close();
            });

            $('#tab-container').easytabs({
                animate: false,
            });

            dataCheck = [];
            fichasSeleccionadas.forEach(function (fichaId) {
                $("input[type='checkbox'][value='" + fichaId.HojaCodigo + "']").prop('checked', true);
            });

            calcularFechasAtras();

            calcularFechasAdelante();
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });


}

calcularFechasAtras = function () { 

    var fecInicial = $('#txtFechaInicial').val();
    var aniosAtras = $('#txtAnioAtras').val();  
    console.log('fech inii', fecInicial);
    console.log('anios', aniosAtras)  

    if ($("#txtFechaInicial").val().length < 1) {
        $("#fechaAtrasResultado").empty();
        $("#fechaAtrasResultado").html("Debe seleccionar una fecha inicial para calcular el 'Fecha Atras'");
    } else {
        var partes = fecInicial.split("/");
        var resultFecInical = new Date(partes[2], partes[1] - 1, partes[0]); 
        var resultAnioInical = parseInt(resultFecInical.getFullYear()) - parseInt(aniosAtras == '' ? 0 : aniosAtras);
        $("#fechaAtrasResultado").empty();
        if(aniosAtras!="")$("#fechaAtrasResultado").html("Año calculado: " + resultAnioInical)
    }
}

calcularFechasAdelante = function () { 

    var fecFinal = $('#txtFechaInicial').val();
    var anioAdelante = $('#txtAnioAdelante').val();

    if ($("#txtFechaInicial").val().length < 1) {
        $("#fechaAdelanteResultado").empty();
        $("#fechaAdelanteResultado").html("Debe seleccionar una fecha inicial para calcular el 'Fecha Adelante'");
    } else {
        var partes = fecFinal.split("/");
        var resultFecFinal = new Date(partes[2], partes[1] - 1, partes[0]); 
        var resultAnioFinal = parseInt(resultFecFinal.getFullYear()) + parseInt(anioAdelante == '' ? 0 : anioAdelante);
        $("#fechaAdelanteResultado").empty();
        if(anioAdelante!="")$("#fechaAdelanteResultado").html("Año calculado: " + resultAnioFinal)
    }

 
}

eliminarPopup = function (id, periodo) {
    limpiarMensaje('mensaje');
    popupEliminarPeriodo(id, periodo);
    //if (confirm('¿Está seguro de realizar esta operación?')) {

    //    $.ajax({
    //        type: 'POST',
    //        url: controlador + 'eliminar',
    //        data: {
    //            idPeriodo: id
    //        },
    //        dataType: 'json',
    //        success: function (result) {
    //            if (result == 1) {
    //                mostrarMensaje('mensaje', 'exito', 'El período ha sido eliminado correctamente.');
    //                cargarPeriodos();
    //            }
    //            else {
    //                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
    //            }
    //        },
    //        error: function () {
    //            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
    //        }
    //    });
    //}
}

grabar = function () {
    var validacion = validar();

    var param = getDataPeriodo();

    if (validacion == "") { 

        $.ajax({
            type: 'POST',
            url: controlador + 'grabar',
            data: param,
            success: function (result) {
                if (result == 1) {
                    mostrarMensaje('mensaje', 'exito', 'Los datos se grabaron correctamente.');
                    $('#popupEdicion').bPopup().close();
                    cargarPeriodos();
                }
                else {
                    if(result == 2) mostrarMensaje('mensajeEdicion', 'error', 'Las fechas (inicial y final) del periodo se traslapan o cruzan con las de otro periodo ya existente.');
                    else mostrarMensaje('mensajeEdicion', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensajeEdicion', 'error', 'Se ha producido un error.');
            }
        }); 
    }
    else {
        mostrarMensaje('mensajeEdicion', 'alert', validacion);
    }
}


actualizar = function () {
    var validacion = validar();

    var param = getDataPeriodo();

    if (validacion == "") {
        
        $.ajax({
            type: 'POST',
            url: controlador + 'actualizar',
            data: param,
            success: function (result) {
                if (result == 1) {
                    mostrarMensaje('mensaje', 'exito', 'Los datos se grabaron correctamente.');
                    $('#popupEdicion').bPopup().close();
                    cargarPeriodos();
                }
                else {
                    if(result == 2) mostrarMensaje('mensajeEdicion', 'error', 'Las fechas (inicial y final) del periodo se traslapan o cruzan con las de otro periodo ya existente.');
                    else mostrarMensaje('mensajeEdicion', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensajeEdicion', 'error', 'Se ha producido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensajeEdicion', 'alert', validacion);
    }
}

getDataPeriodo = function () {

    var param = {};
    dataCheck = [];
    param.PeriCodi = $("#txtCodigo").val();
    param.PeriNombre = $("#txtPeriodo").val();
    param.PeriFechaInicio = moment($("#txtFechaInicial").val(), "DD-MM-YYYY").format("MM-DD-YYYY hh:mm:ss A");
    param.PeriFechaFin = moment($("#txtFechaFinal").val(), "DD-MM-YYYY").format("MM-DD-YYYY hh:mm:ss A");
    param.PeriHoraFin = $("#txtHora").val();
    param.PeriEstado = $("#cbEstadoRegistrar").val();
    param.PeriHorizonteAtras = $("#txtAnioAtras").val();
    param.PeriHorizonteAdelante = $("#txtAnioAdelante").val();
    param.PeriComentario = $("#txtComentario").val();
    param.PeriHojaProyecto = {};
    seleccionarChekeados();
    dataCheck.forEach(function (value, index) {
        param.PeriHojaProyecto[index] = value;
    });

    return param;
}

validar = function () {
    var mensaje = "<ul>";
    var flag = true;

    if ($("#txtFechaInicial").val().length < 1) {
        mensaje = mensaje + "<li>Seleccione una fecha inicial</li>";
        flag = false;
    }

    if ($("#txtFechaFinal").val().length < 1) {
        mensaje = mensaje + "<li>Seleccione una fecha final</li>";
        flag = false;
    }

    if ($("#txtHora").val().length < 1) {
        mensaje = mensaje + "<li>El campo Hora es obligatorio</li>";
        flag = false;
    }
    
    if ($('#cbEstadoRegistrar').val() == "S") {
        mensaje = mensaje + "<li>El campo 'Estado' es obligatorio, seleccione una opcion</li>";
        flag = false;
    }

     
    if ($('#txtAnioAtras').val() == "") {
        mensaje = mensaje + "<li>El campo 'Años Atras' es obligatorio</li>";
        flag = false;
    }

    if ($('#txtAnioAdelante').val() == "") {
        mensaje = mensaje + "<li>El campo 'Años Adelante' es obligatorio</li>";
        flag = false;
    }

    var fechaFinal = $("#txtFechaFinal").val();
    var fechaInicial = $("#txtFechaInicial").val(); 

    // Convertir las fechas en objetos Date (formato esperado: "dd-mm-yyyy")
    var partesFechaFinal = fechaFinal.split("/");
    var partesFechaInicial = fechaInicial.split("/");

    var fechaFinalDate = new Date(partesFechaFinal[2], partesFechaFinal[1] - 1, partesFechaFinal[0]);
    var fechaInicialDate = new Date(partesFechaInicial[2], partesFechaInicial[1] - 1, partesFechaInicial[0]);

    if (fechaFinalDate < fechaInicialDate) {
        mensaje = mensaje + "<li>La fecha inicial debe ser menor a la fecha final</li>";
        flag = false;
    }

    var tiposProyecto = document.querySelectorAll('input[class*="dataCheck_"]');
    var proyectos = {};

// Agrupar los checkboxes por el valor dinámico de la clase (dataCheck_TipoCodigo)
tiposProyecto.forEach(function(checkbox) {
    var projectClass = Array.from(checkbox.classList).find(function(className) {
        return className.startsWith('dataCheck_');
    });
    
    if (projectClass) {
        if (!proyectos[projectClass]) {
            proyectos[projectClass] = [];
        }
        proyectos[projectClass].push(checkbox);
    }
});

for (var projectClass in proyectos) {
    var algunoSeleccionado = proyectos[projectClass].some(function(checkbox) {
        return checkbox.checked;  
    });

    if (!algunoSeleccionado) {
        mensaje += "<li>Se tiene que seleccionar al menos una ficha para cada tipo de proyecto</li>";
        flag = false;
    }
}

    mensaje = mensaje + "</ul>";
     
    if (flag) mensaje = "";
    
    return mensaje;
}


eliminar = function (id) {
    if (confirm('¿Está seguro de realizar esta operación?')) {

        $.ajax({
            type: 'POST',
            url: controlador + 'eliminar',
            data: {
                pericodi: id
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    mostrarMensaje('mensaje', 'exito', 'El registro se eliminó correctamente.');
                    cargarPeriodos();
                }
                else {
                    mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}
mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).attr('class', '');
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}

limpiarMensaje = function (id) {
    $('#' + id).attr('class', '');
    $('#' + id).html('');
}



seleccionarChekeados = function () {
    var checkboxes = document.querySelectorAll(".chkFicha");

    checkboxes.forEach(function (checkbox) {
        if (checkbox.checked) {
            dataCheck.push(checkbox.value);
        }
    });

}

cargarAccordion = function () {
    var acc = document.getElementsByClassName("accordion");
    var i;

    for (i = 0; i < acc.length; i++) {
        acc[i].addEventListener("click", function () {
            this.classList.toggle("active");
            var panel = this.nextElementSibling;
            if (panel.style.maxHeight == 0+"px") {
                panel.style.maxHeight = panel.scrollHeight + "px";
            } else {
                panel.style.maxHeight = 0+ "px";
            }
        });
    }
}


function cargarCatalogoEstado(id, selectHtml) {
    $.ajax({
        type: 'POST',
        url: controlador + 'ListarCatalogo',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: id }),
        success: function (eData) {
            cargarListaParametros(eData, selectHtml);
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarListaParametros(listaValores, selectHtml) {
    var selectData = $(selectHtml);
    $.each(listaValores, function (index, catalogo) {
        // Crear la opción
        var option = $('<option>', {
            value: catalogo.Valor,
            text: catalogo.DescortaDatacat
        });

        // Agregar la opción al select
        selectData.append(option);
    });

}

function cargarCatalogoEstado(id, selectHtml) {
    $.ajax({
        type: 'POST',
        url: controlador + 'ListarCatalogo',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: id }),
        success: function (eData) {
            console.log(eData);
            cargarListaParametros(eData, selectHtml);
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarListaParametros(listaValores, selectHtml) {
    var selectData = $(selectHtml);
    $.each(listaValores, function (index, catalogo) {
        // Crear la opción
        var option = $('<option>', {
            value: catalogo.Valor,
            text: catalogo.DescortaDatacat
        });

        // Agregar la opción al select
        selectData.append(option);
    });

}


function cargarCatalogoEstadoEditar(id, selectHtml, valueDefault) {
    $.ajax({
        type: 'POST',
        url: controlador + 'ListarCatalogo',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: id }),
        success: function (eData) {
            cargarListaParametrosEditar(eData, selectHtml, valueDefault);
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarListaParametrosEditar(listaValores, selectHtml, valueDefault) {
    var selectData = $(selectHtml);
    selectData.empty();  // Limpiar el select antes de agregar nuevas opciones

    $.each(listaValores, function (index, catalogo) {
        // Crear la opción
        var option = $('<option>', {
            value: catalogo.Valor,
            text: catalogo.DescortaDatacat
        });

        // Agregar la opción al select
        selectData.append(option);
    });

    // Establecer el valor por defecto
    selectData.val(valueDefault);
}

function popupEliminarPeriodo(id, periodo) {
    idEliminacion = id;
    document.getElementById("namePeriodo").textContent = periodo;
    $('#popupEliminarPeriodo').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
}

function popupRegistroEliminarPeriodo() {
    $('#popupRegistroEliminarPeriodo').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
}

function popupClose(id) {
    $(`#${id}`).bPopup().close();
}

function cargarEliminar() {
    $("#frmEliminarPeriodo").submit(function (event) {
        event.preventDefault();
        $.ajax({
            type: 'POST',
            url: controlador + 'eliminar',
            data: {
                idPeriodo: idEliminacion
            },
            dataType: 'json',
            success: function (result) {
                idEliminacion = 0;
                if (result == 1) {
                    mostrarMensaje('mensaje', 'exito', 'El período ha sido eliminado correctamente.');
                    cargarPeriodos();
                } else {
                    if(result == 2) mostrarMensaje('mensaje', 'error', 'El periodo ya cuenta con registros, dicho periodo no se podrá eliminar.');
                    else mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                idEliminacion = 0;
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });

        $(this).trigger("reset");
        $(`#popupEliminarPeriodo`).bPopup().close();

    });
}

function cargarLoading() {
    if (!isLoading) {
        isLoading = true;
        $('#loadingProyecto').bPopup({
            fadeSpeed: 'fast',
            opacity: 0.4,
            followSpeed: 500, // can be a string ('slow'/'fast') or int
            modalColor: '#000000',
            onClose: function () {
                isLoading = false;
            }
        });
    }
}

function stopLoading() {
    if (isLoading) {
        $('#loadingProyecto').bPopup().close();
        isLoading = false;
    }
}
