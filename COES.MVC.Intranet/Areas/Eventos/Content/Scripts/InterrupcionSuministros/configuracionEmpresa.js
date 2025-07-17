var controlador = siteRoot + 'Eventos/AnalisisFallas/'
$(function () {
    $('#cbTipoEmpresaLista').val($('#hfcbTipoEmpresaLista').val());

    $("#btnNuevaConfiguracion").click(function () {
        NuevaConfiguracionEmpresa();
    });

    $("#btnRegresar").click(function () {        
        window.location.href = controlador + 'InterrupcionSuministros';
    });
      

    DimensionarScrollListado();

    $('#cbTipoEmpresaLista').change(function () {
        MostrarListadoConfiguracion($('#cbTipoEmpresaLista').val().trim());
    });

    MostrarListadoConfiguracion($('#cbTipoEmpresaLista').val().trim());
});

function NuevaConfiguracionEmpresa() {

    $.ajax({
        type: 'POST',
        url: controlador + 'NuevaConfiguracionEmpresa',
        success: function (evt) {
            var excep_resultado = $("#hdResultado_ED").val();
            var excep_mensaje = $("#hdMensaje_ED").val();
            var excep_detalle = $("#hdDetalle_ED").val();

            $('#contenidoDetalle').html(evt);

            setTimeout(function () {
                $('#popupNuevaConfiguracion').bPopup({
                    autoClose: false,
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown',
                    onClose: function () {
                        $('#popup').empty();
                    }
                });
            }, 500);
            

            //Cuando cambiamos de tipoempresa debera cambiar las empresas
            $('#cbTipoEmpresa').change(function () {
                cargarEmpresas();
            });

            //Cancela la nueva configuracion, cierra popup
            $("#btnCancelarNewConfiguracion").click(function () {
                $('#popupNuevaConfiguracion').bPopup().close();
            });

            // Graba los datos de un nuevo ensayo...
            $('#btnGrabarNewConfiguracion').click(function () {
                
                guardarConfiguracion();
                 
            });
        },
        error: function (err) {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}



//para guardar el ensayo
function guardarConfiguracion() {
    var emprcodi = $('#cbEmpresa').val().trim();
    var codEracmf = $('#txtEmprActERACMF').val().trim();
    var codOsinergmin = $("#txtOsinergcodi").val().trim();
    var afalerta = "N";
    var chk = document.getElementById("chkAlertReporte").checked;
    if (chk) {
        afalerta = "S";
    }

    if (emprcodi != "") {
        if (codEracmf != null && codEracmf != "") {

            $.ajax({
                type: 'POST',
                url: controlador + 'GuardarNuevaConfiguracion',
                data: {
                    emprcodi: emprcodi,
                    codEracmf: codEracmf,
                    codOsinergmin: codOsinergmin,
                    afalerta: afalerta
                },
                success: function (evt) {
                    //var excep_resultado = $("#hdResultado_ED").val();
                    //var excep_mensaje = $("#hdMensaje_ED").val();

                    if (evt.Resultado == "1") {
                        mostrarMensaje('mensajeEdit', 'exito', "Configuración guardada con éxito.");
                        setTimeout("$('#popupNuevaConfiguracion').bPopup().close()", 1000);
                    
                        MostrarListadoConfiguracion($('#cbTipoEmpresaLista').val().trim());
                    }

                    if (evt.Resultado == "2") {
                        mostrarMensaje('mensajeEdit', 'alert', "Existe una configuración con los datos ingresados.");
                    }

                    if (evt.Resultado == "3") {
                        mostrarMensaje('mensajeEdit', 'alert', "Existe una configuración con el mismo codigo Osinergmin.");
                    }                    

                },
                error: function (err) {
                    
                    mostrarMensaje('mensajeEdit', 'alert', "Ha ocurrido un error en guardar la nueva configuración.");
                    
                }
            });
        } else {
            mostrarMensaje('mensajeEdit', 'alert', "Ingrese el nombre empresa archivo ERACMF.");
            
        }
    } else {
        mostrarMensaje('mensajeEdit', 'alert', "Escoja una empresa.");
        
    }
   
} 

//para guardar el ensayo
function guardarEdicionConfiguracion(afemprcodi) {
    var afalerta = "N";
    var codEracmf = $('#txtEditCodEracmf').val().trim();
    var codOsinergmin = $("#txtEditCodOsinergmin").val().trim();
    var chk = document.getElementById("chkAlertReporte").checked;
    if (chk) {
        afalerta = "S";
    }
    
        if (codEracmf != null && codEracmf != "") {

            $.ajax({
                type: 'POST',
                url: controlador + 'GuardarEdicionConfiguracion',
                data: {
                    afemprcodi: afemprcodi,
                    codEracmf: codEracmf,
                    codOsinergmin: codOsinergmin,
                    afalerta: afalerta
                },
                success: function (evt) {
                    //var excep_resultado = $("#hdResultado_ED").val();
                    //var excep_mensaje = $("#hdMensaje_ED").val();

                    if (evt.Resultado == "1") {
                        mostrarMensaje('mensajeEdit', 'exito', "Configuración editada con éxito.");
                        setTimeout("$('#popupEditarConfiguracion').bPopup().close()", 1000);
                        
                        MostrarListadoConfiguracion($('#cbTipoEmpresaLista').val().trim());
                    }

                    if (evt.Resultado == "2") {
                        mostrarMensaje('mensajeEdit', 'alert', "Existe una configuración con los datos ingresados.");
                    }

                    if (evt.Resultado == "3") {
                        mostrarMensaje('mensajeEdit', 'alert', "Existe una configuración con el mismo código Osinergmin.");
                    }


                },
                error: function (err) {

                    mostrarMensaje('mensajeEdit', 'alert', "Ha ocurrido un error en guardar la nueva configuración.");

                }
            });
        } else {
            mostrarMensaje('mensajeEdit', 'alert', "Ingrese el nombre empresa archivo ERACMF.");

        }
    

} 

function eliminarConfiguracion(afemprcodi) {
    if (confirm("¿Desea eliminar la configuracion de la empresa?")) {
        $.ajax({
            type: 'POST',
            url: controlador + "EliminarConfiguracion",
            data: {
                afemprcodi: afemprcodi
            },
            success: function (evt) {
                if (evt.Resultado == "1") {                    
                    MostrarListadoConfiguracion($('#cbTipoEmpresaLista').val().trim());
                }
                else {
                    alert(evt.StrMensaje);
                }                
            },
            error: function (err) {
                alert("Error al eliminar Configuración de la emrpesa");
            }
        });
    }
}

function editarEquipo(afemprcodi) {      

    $.ajax({
        type: 'POST',
        url: controlador + 'EditarConfiguracionEmpresa',
        data: {
            afemprcodi: afemprcodi
        },
        success: function (evt) {
            $('#popupEditarConfiguracion').html(evt);
            var excep_resultado = $("#hdResultado_ED").val();
            var excep_mensaje = $("#hdMensaje_ED").val();

            if (excep_resultado != "-1") {

                $('#contenidoDetalleEditar').html(evt);

                setTimeout(function () {
                    $('#popupEditarConfiguracion').bPopup({
                        autoClose: false,
                        easing: 'easeOutBack',
                        speed: 450,
                        transition: 'slideDown',
                        onClose: function () {
                            $('#popup').empty();
                        }
                    });
                }, 500);


                //Cancela la edicion de configuracion, cierra popup
                $("#btnCancelarEditConfiguracion").click(function () {
                    $('#popupEditarConfiguracion').bPopup().close();
                });

                // Graba los datos de ensayo...
                $('#btnGrabarEditConfiguracion').click(function () {
                    guardarEdicionConfiguracion(afemprcodi);
                });
            }
            else {
                //$('#popupEditarConfiguracion').html('');
                //mostrarMensaje('mensaje', 'alert', excep_mensaje);
                alert(excep_mensaje);
            }
        },
        error: function (err) {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });

}

function cargarEmpresas() {


    var idTipoEmpresa = $('#cbTipoEmpresa').val();
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarEmpresasNuevaConfiguracion',
        dataType: 'json',
        async: true,
        data: {
            idTipoEmpresa: idTipoEmpresa
        },
        success: function (evt) {
            $('#cbEmpresa').empty();

            var listaEmpresas = evt.LstEmpresasT;
            if (listaEmpresas != null && listaEmpresas.length > 0) {
                for (var i = 0; i < listaEmpresas.length; i++) {
                    $('#cbEmpresa').append('<option value=' + listaEmpresas[i].Emprcodi + '>' + listaEmpresas[i].Emprnomb + '</option>');
                }
            } else {
                
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error al cargar empresas");

        }
    });
}


function MostrarListadoConfiguracion(idTipoEmpresa) {    

    $.ajax({
        type: 'POST',
        url: controlador + 'ListarConfiguracionEmpresa',
        data: { idTipoEmpresa: idTipoEmpresa},
        success: function (result) {

            $('#listado').css("width", $('#mainLayout').width() + "px");
            $('#listado').html(result.Resultado);  
            $('#tabla').dataTable({
                bJQueryUI: true,
                "scrollY": 500, // tamaño listado height
                "scrollX": true,
                "sDom": 'ft',
                "ordering": true,
                "iDisplayLength": -1
            });

        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });

}

function mostrarMensaje(id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
};



//Dimensiona el listado con un scroll interior
function DimensionarScrollListado() {
    var nuevoTamanioTabla = getHeightTablaListado();

    $(window).resize(function () {
        $('#listado').css("width", $('#mainLayout').width() + "px");
    });
    $('#listado').css("width", $('#mainLayout').width() + "px");
    $('#tabla').dataTable({
        bJQueryUI: true,
        //"scrollY": 540,
        "scrollY": $('#listado').height() > 200 ? nuevoTamanioTabla + "px" : "100%", //tamaño del contenedor
        "scrollX": false,
        "sDom": 'ft',
        "ordering": false,
        "iDisplayLength": 10000
    });
}

//funcion que calcula el alto disponible para la tabla reporte
function getHeightTablaListado() {
    /*return $("#mainLayout").height() //todo
        - 15 //padding-top
        - 15 //padding-bottom
        - $("#Contenido").parent().height() //Filtros
        - 14 //<br>
        - 30 //TablaConsultaIntervencion_info
        ;*/
    return $(window).height()
        - $("header").height()
        - $("#cntTitulo").height() - 2
        - $("#Reemplazable .form-title").height()
        - 15
        - $("#Contenido").parent().height() //Filtros
        - 14 //<br>
        - $(".dataTables_filter").height()
        - $(".dataTables_scrollHead").height()
        - 30 //TablaConsultaIntervencion_info
        - 220
        - 61 //- $(".footer").height() - 10

        - 5 //propio de la tabla
        ;
}