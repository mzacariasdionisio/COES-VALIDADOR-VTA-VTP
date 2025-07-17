var controlador = siteRoot + 'PotenciaFirmeRemunerable/Configuracion/';

var NUEVO = 1;
var EDITAR = 2;

var ANCHO_TABLA = 1200;

var listadoPeriodos = ""; 

$(function () {
    listadoPeriodos = $("#hfListadoPeriodos").val();  
    $('#cntMenu').css("display", "none");

    $("#new-congestion-periodo").Zebra_DatePicker({
        format: 'd-m-Y',
    });

    /** GUARDAR CONGESTION */
    $("#frmNuevaCongestion").submit(function (event) {
        event.preventDefault();
        var data = getFormData($(this));
        guardarCongestion(data, NUEVO);
    });

    /** EDITAR CONGESTION */
    $("#frmEditarCongestion").submit(function (event) {
        event.preventDefault();
        var data2 = getFormData($(this));
        guardarCongestion(data2, EDITAR);
    });

    /** BAJA CONGESTION */
    $("#frmBajaCongestion").submit(function (event) {
        event.preventDefault();

        if (confirm("¿Esta seguro que desea dar de baja a la congestión?")) {
            var data = getFormData($(this));
            darBajaCongestion(data);

            $(`#popupBajaCongestion`).bPopup().close();
        }

    });


    // COMBOS LINEAS
    $('#cbLinea1').change(function () {
        var idLineaEscogido = $("#cbLinea1").val();
        if (idLineaEscogido) {
            mostrarLineaNumero("2");
        } else {
            ocultarLineas([2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12]);
        }
    });
    $('#cbLinea2').change(function () {
        var idLineaEscogido = $("#cbLinea2").val();
        if (idLineaEscogido) {
            mostrarLineaNumero("3");
        } else {
            ocultarLineas([3, 4, 5, 6, 7, 8, 9, 10, 11, 12]);
        }
    });
    $('#cbLinea3').change(function () {
        var idLineaEscogido = $("#cbLinea3").val();
        if (idLineaEscogido) {
            mostrarLineaNumero("4");
        } else {
            ocultarLineas([4, 5, 6, 7, 8, 9, 10, 11, 12]);
        }
    });
    $('#cbLinea4').change(function () {
        var idLineaEscogido = $("#cbLinea4").val();
        if (idLineaEscogido) {
            mostrarLineaNumero("5");
        } else {
            ocultarLineas([5, 6, 7, 8, 9, 10, 11, 12]);
        }
    });
    $('#cbLinea5').change(function () {
        var idLineaEscogido = $("#cbLinea5").val();
        if (idLineaEscogido) {
            mostrarLineaNumero("6");
        } else {
            ocultarLineas([6, 7, 8, 9, 10, 11, 12]);
        }
    });
    $('#cbLinea6').change(function () {
        var idLineaEscogido = $("#cbLinea6").val();
        if (idLineaEscogido) {
            mostrarLineaNumero("7");
        } else {
            ocultarLineas([7, 8, 9, 10, 11, 12]);
        }
    });
    $('#cbLinea7').change(function () {
        var idLineaEscogido = $("#cbLinea7").val();
        if (idLineaEscogido) {
            mostrarLineaNumero("8");
        } else {
            ocultarLineas([8, 9, 10, 11, 12]);
        }
    });
    $('#cbLinea8').change(function () {
        var idLineaEscogido = $("#cbLinea8").val();
        if (idLineaEscogido) {
            mostrarLineaNumero("9");
        } else {
            ocultarLineas([9, 10, 11, 12]);
        }
    });

    $('#cbLinea9').change(function () {
        var idLineaEscogido = $("#cbLinea9").val();
        if (idLineaEscogido) {
            mostrarLineaNumero("10");
        } else {
            ocultarLineas([10, 11, 12]);
        }
    });

    $('#cbLinea10').change(function () {
        var idLineaEscogido = $("#cbLinea10").val();
        if (idLineaEscogido) {
            mostrarLineaNumero("11");
        } else {
            ocultarLineas([11, 12]);
        }
    });

    $('#cbLinea11').change(function () {
        var idLineaEscogido = $("#cbLinea11").val();
        if (idLineaEscogido) {
            mostrarLineaNumero("12");
        } else {
            ocultarLineas([12]);
        }
    });

    
    /** BOTONES NUEVA CONGESTION */
    $("#btnNuevaCongestion").click(function () {
        $("#frmNuevaCongestion").trigger("reset");
        
        abrirPopupNuevaCongestion();
    });


    // COMBOS AÑOS
    $('#cbAnioCongestion').change(function () {
        listadoPeriodo();
    });


    // COMBOS PERIODOS   
    $('#cbPeriodoCongestion').change(function () {
        fechaSegunComboEquipo("cbPeriodoCongestion", "hf_new-equipo-vigencia-congestion");
        cargarListadoCongestion();
    });

    ANCHO_TABLA = ($('#mainLayout').width() - 30) + "px";

    // COMBOS ESTADOS
    $('#cbEstadoCongestion').change(function () {
        cargarListadoCongestion();
    });

    /** CARGAR LISTADO DE CONGESTIONES */
    cargarListadoCongestion();

});

function mostrarLineaNumero(strNumero) {
    $("#bloque_add_congestion_linea" + strNumero).css("display", "block");
}

function ocultarLineaNumero(strNumero) {
    $("#cbLinea" + strNumero).val("-2");
    $("#bloque_add_congestion_linea" + strNumero).css("display", "none");
}

function ocultarLineas(array) {
    $.each(array, function (index, value) {
        $("#cbLinea" + value).val("");
        $("#bloque_add_congestion_linea" + value).css("display", "none");
    });
}

function mostrarELineaNumero(strNumero) {
    //$("#cbELinea" + strNumero).val("-2");
    $("#bloque_edit_congestion_linea" + strNumero).css("display", "block");
}

function ocultarELineaNumero(strNumero) {
    $("#cbELinea" + strNumero).val("-2");
    $("#bloque_edit_congestion_linea" + strNumero).css("display", "none");
}

/** Carga lista de Congestion **/
function cargarListadoCongestion() {

    obj = ObtenerFiltroElegido();

    $.ajax({
        type: 'POST',
        url: controlador + "ListarCongestion",
        data: {
            pericodi: obj.pericodiEscogido,
            estado: obj.estadoEscogido
        },
        success: function (evt) {
            var anchoDiv = ANCHO_TABLA;

            if (evt.Resultado != "-1") {
                var altotabla;
                var idTabla;

                $('#listadoDeCongestiones').html(evt.Resultado);
                

                $("#listadoDeCongestiones").css("width", (anchoDiv) + "px");
                altotabla = parseInt($('#listadoDeCongestiones').height()) || 0;
                idTabla = "#tabla_lstCongestiones";

                $(idTabla).dataTable({
                    "sPaginationType": "full_numbers",
                    "destroy": "true",
                    "ordering": false,
                    "searching": true,
                    "iDisplayLength": 15,
                    "info": false,
                    "paging": false,
                    "scrollX": true,
                    "scrollY": altotabla > 355 || altotabla == 0 ? 355 + "px" : "100%"
                });


            } else {
                alert('Ha ocurrido un error: ' + evt.Mensaje);
            }

        },
        error: function (err) {
            alert('Ha ocurrido un error');
        }
    });
};

/** Carga lista de periodos por año */
function listadoPeriodo() {

    var annio = -1;

    annio = parseInt($("#cbAnioCongestion").val()) || 0;
    $("#cbPeriodoCongestion").empty();

    $.ajax({
        type: 'POST',
        url: controlador + "PeriodoListado",
        data: {
            anio: annio,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                if (evt.ListaPeriodo.length > 0) {
                    $.each(evt.ListaPeriodo, function (i, item) {
                        $('#cbPeriodoCongestion').get(0).options[$('#cbPeriodoCongestion').get(0).options.length] = new Option(item.Pfrpernombre, item.Pfrpercodi);
                    });
                    fechaSegunComboEquipo("cbPeriodoCongestion", "hf_new-equipo-vigencia-congestion");
                    cargarListadoCongestion();
                } else {
                    $('#cbPeriodoCongestion').get(0).options[0] = new Option("--", "0");
                }

            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}
function refrehDatatable() {

    var altotablaL = parseInt($('#listadoDeCongestiones').height()) || 0;
    $("#listadoDeCongestiones").css("width", (1450) + "px");
    $('#tabla_lstCongestiones').dataTable({
        "sPaginationType": "full_numbers",
        "destroy": "true",
        "ordering": false,
        "searching": true,
        "iDisplayLength": 15,
        "info": false,
        "paging": false,
        "scrollX": true,
        "scrollY": altotablaL > 355 || altotablaL == 0 ? 355 + "px" : "100%"
    });

}

function ObtenerFiltroElegido() {
    var filtro = {};

    filtro.estadoEscogido = $("#cbEstadoCongestion").val();
    filtro.pericodiEscogido = parseInt($("#cbPeriodoCongestion").val()) || 0;

    return filtro;
}


function abrirPopupNuevaCongestion() {

    resetearPopupAgregarCongestion();

    var fecha = ""; 
    fecha = $("#hf_new-equipo-vigencia-congestion").val();
    $("#new-congestion-periodo").val(fecha); 

    var codigoDisponibleEnCongestion = $("#hfCodDisponibleCongestion").val();

    $("#new-congestion-id").val(codigoDisponibleEnCongestion);
    $("#bloque_add_congestion_periodo").css("display", "block");
    $("#bloque_add_congestion_id").css("display", "block");
    $("#bloque_add_congestion_enlace").css("display", "block");
    $("#bloque_add_congestion_pmax").css("display", "block");
    $("#bloque_add_congestion_pmin").css("display", "block");

    $("#bloque_add_congestion_linea1").css("display", "block");
    $("#bloque_add_congestion_linea2").css("display", "none");
    $("#bloque_add_congestion_linea3").css("display", "none");
    $("#bloque_add_congestion_linea4").css("display", "none");
    $("#bloque_add_congestion_linea5").css("display", "none");
    $("#bloque_add_congestion_linea6").css("display", "none");
    $("#bloque_add_congestion_linea7").css("display", "none");
    $("#bloque_add_congestion_linea8").css("display", "none");
    $("#bloque_add_congestion_linea9").css("display", "none");



    setTimeout(function () {
        $('#popupNuevaCongestion').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown'
        });
    });
}


function cerrarPopup(id) {
    $(id).bPopup().close()
}


function validarFormularioCongestion(objEquipo, periodoElegido, accion) {
    var msj = "";

    var validarId = true;
    var validarEnlace = true;
    var validarPmax = Boolean(objEquipo.Pfrcgtpotenciamax);
    var validarPmin = Boolean(objEquipo.Pfrcgtpotenciamin);
    
    var validarVigenciaIni = true;

    var esNuevo = false;

    if (accion == NUEVO) {
        esNuevo = true;
    } 


    if (validarId &&  esNuevo) {
        if (objEquipo.Pfrentid == null || objEquipo.Pfrentid == '') {
            msj += "Debe ingresar ID" + "\n";
        }
    }

    if (validarEnlace && esNuevo) {
        if (objEquipo.Pfrentnomb == null || objEquipo.Pfrentnomb == '') {
            msj += "Debe ingresar Enlace" + "\n";
        }
    }

    if (validarPmax) {
        if (isNaN(objEquipo.Pfrcgtpotenciamax)) {
            msj += "Dato Potencia Máxima incorrecto, debe ser de tipo numérico" + "\n";
        }
        if (+objEquipo.Pfrcgtpotenciamax < +objEquipo.Pfrcgtpotenciamin) {
            msj += "Dato Potencia Máxima incorrecto, debe ser mayor que Potencia Mínima" + "\n";
        }
    }

    if (validarPmin) {

        if (isNaN(objEquipo.Pfrcgtpotenciamin)) {
            msj += "Dato Potencia Mínima incorrecto, debe ser de tipo numérico" + "\n";
        }
        if (+objEquipo.Pfrcgtpotenciamax < +objEquipo.Pfrcgtpotenciamin) {
            msj += "Dato Potencia Mínima incorrecto, debe ser menor que Potencia Máxima" + "\n";
        }
    }

    if (validarVigenciaIni) {
        if (periodoElegido == null || periodoElegido == '') {
            msj += "Debe seleccionar un Periodo" + "\n";
        }
    }

    return msj;
}



function getFormData($form) {
    var disabled = $form.find(':input:disabled').removeAttr('disabled');
    var unindexed_array = $form.serializeArray();
    disabled.attr('disabled', 'disabled');

    var indexed_array = {};

    $.map(unindexed_array, function (n, i) {
        indexed_array[n['name']] = n['value'];
    });

    return indexed_array;
}

function validarCongestionAGuardar(dataForm, accion1, obj) {

    $.ajax({
        type: 'POST',
        url: controlador + 'ValidarCongestion',
        contentType: "application/json",
        dataType: 'json',
        data: JSON.stringify({
            pfrCongestion: dataForm,
            accion: accion1
        }
        ),
        cache: false,
        success: function (data) {
            if (data.Resultado == "1") {
                guardarCGT(dataForm, accion1, obj);
            } else {
                alert('Ha ocurrido un error : ' + data.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
    
}


function guardarCongestion(dataForm, accion1) {
    var obj = ObtenerFiltroElegido();
    var periodoVigenciaEscogido = $("#new-congestion-periodo").val();    

    var mensajeErrores = validarFormularioCongestion(dataForm, periodoVigenciaEscogido, accion1);
    dataForm.Pfrcatcodi = TIPO_CONGESTION;
    AgregarEntidadDat(dataForm);

    if (mensajeErrores == "") {
        validarCongestionAGuardar(dataForm, accion1, obj);
    } else {
        alert(mensajeErrores);
    }
}

function guardarCGT(dataForm, accion1, obj) {
    $.ajax({
        type: 'POST',
        url: controlador + 'GuardarCongestion',
        contentType: "application/json",
        dataType: 'json',
        data: JSON.stringify({
            pfrCongestion: dataForm,
            accion: accion1,
            pericodi: obj.pericodiEscogido,
        }
        ),
        cache: false,
        success: function (data) {
            if (data.Resultado == "1") {

                if (accion1 == NUEVO) {

                    $("#popupNuevaCongestion").bPopup().close()
                    $("#frmNuevaCongestion").trigger("reset");

                    alert("Se registró correctamente la Congestión");

                    //Actualizar proximo codigo disponible
                    $("#hfCodDisponibleCongestion").val(data.CodigoDisponibleCongestion);

                    //Listar Congestiones
                    cargarListadoCongestion();

                } else {
                    if (accion1 == EDITAR) {

                        $("#popupEditarCongestion").bPopup().close();
                        $("#frmEditarCongestion").trigger("reset");

                        alert("Se editó correctamente la Congestión");

                        //Listar Congestiones
                        cargarListadoCongestion();
                    }
                }

            } else {
                alert('Ha ocurrido un error : ' + data.Mensaje);                
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function resetearPopupAgregarCongestion() {
    $("#bloque_add_congestion_periodo").css("display", "none");
    $("#bloque_add_congestion_id").css("display", "none");
    $("#bloque_add_congestion_enlace").css("display", "none");
    $("#bloque_add_congestion_pmax").css("display", "none");
    $("#bloque_add_congestion_pmin").css("display", "none");

    $("#bloque_add_congestion_linea1").css("display", "none");
    $("#bloque_add_congestion_linea2").css("display", "none");
    $("#bloque_add_congestion_linea3").css("display", "none");
    $("#bloque_add_congestion_linea4").css("display", "none");
    $("#bloque_add_congestion_linea5").css("display", "none");
    $("#bloque_add_congestion_linea6").css("display", "none");
    $("#bloque_add_congestion_linea7").css("display", "none");
    $("#bloque_add_congestion_linea8").css("display", "none");
    $("#bloque_add_congestion_linea9").css("display", "none");
    $("#bloque_add_congestion_linea10").css("display", "none");
    $("#bloque_add_congestion_linea11").css("display", "none");
    $("#bloque_add_congestion_linea12").css("display", "none");


}


function fechaSegunComboEquipo(comboCongestion, hfCongestion) {
    var periodoCongestion = parseInt($("#" + comboCongestion).val()) || 0;
    var pCongestion = obtenerFechaIniNuevo(periodoCongestion);
    $("#" + hfCongestion).val(pCongestion);
}

function obtenerFechaIniNuevo(pericodi) {
    var arrayDeGrupos = listadoPeriodos.split(",");
    for (var i = 0; i < arrayDeGrupos.length; i++) {
        var grupo = arrayDeGrupos[i];
        var arraySubGrupo = grupo.split("/");
        var strPericodi = arraySubGrupo[0];
        if (strPericodi == pericodi) {
            return arraySubGrupo[1];
        }

    }
    return "";
}

