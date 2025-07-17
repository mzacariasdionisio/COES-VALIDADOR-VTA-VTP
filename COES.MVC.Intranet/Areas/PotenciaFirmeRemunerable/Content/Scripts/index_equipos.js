var controlador = siteRoot + 'PotenciaFirmeRemunerable/Configuracion/';

var NUEVO = 1;
var EDITAR = 2;

var ANCHO_TABLA_EQ = 1200;
var listadoPeriodos = "";

$(function () {
    listadoPeriodos = $("#hfListadoPeriodos").val();  
    $('#cntMenu').css("display", "none");


    $('#tab-container').easytabs({
        animate: false
    });
    $('#tab-container').bind('easytabs:after', function () {
        refrehDatatable();
    });

    $('#tab-container').easytabs('select', '#vistaBarra');

    $("#new-equipo-periodo").Zebra_DatePicker({
        //format: 'm-Y',
        format: 'd-m-Y',
    });

    $("input[name=Pfreqpvigenciafin]").Zebra_DatePicker({
        format: 'd-m-Y',
        onSelect: function (date, dateFull, dateObject) {
            $("#NotaVigencia").text(`(*) El equipo estará de baja desde el ${date} 00:00`);
        }
    });

    /** GUARDAR EQUIPOS */
    $("#frmNuevoEquipo").submit(function (event) {
        event.preventDefault();
        var data = getFormData($(this));
        guardarEquipoGeneral(data, NUEVO);        
    });

    /** EDITAR EQUIPOS */
    $("#frmEditarEquipo").submit(function (event) {
        event.preventDefault();
        var data2 = getFormData($(this));
        guardarEquipoGeneral(data2, EDITAR);
    });

    /** BAJA EQUIPOS */
    $("#frmBajaEquipo").submit(function (event) {
        event.preventDefault();

        if (confirm("¿Esta seguro que desea dar de baja el equipo?")) {

            var data = getFormData($(this));
            darBajaEquipo(data);

            $(`#popupBajaEquipo`).bPopup().close();
        }

    });

    /** BOTONES NUEVOS EQUIPOS */
    $("#btnNuevaBarra").click(function () {
        $("#frmNuevoEquipo").trigger("reset");      
        abrirPopupNuevoEquipo(TIPO_BARRA);
    });
    $("#btnNuevaLinea").click(function () {
        var pericodiLinea = parseInt($("#cbPeriodoLinea").val()) || 0; 
        
        $("#frmNuevoEquipo").trigger("reset"); 
        abrirPopupNuevoEquipo(TIPO_LINEA); 
        verListadoBarras(pericodiLinea);  //al abrir popup muestra barras actualizadas
    });
    $("#btnNuevaTrafo2").click(function () {
        var pericodiTrafo2 = parseInt($("#cbPeriodoTrafo2").val()) || 0; 
        
        $("#frmNuevoEquipo").trigger("reset"); 
        abrirPopupNuevoEquipo(TIPO_TRAFO2); 
        verListadoBarras(pericodiTrafo2); //al abrir popup muestra barras actualizadas
    });
    $("#btnNuevaTrafo3").click(function () {
        var pericodiTrafo3 = parseInt($("#cbPeriodoTrafo3").val()) || 0; 
        
        $("#frmNuevoEquipo").trigger("reset"); 
        abrirPopupNuevoEquipo(TIPO_TRAFO3); 
        verListadoBarras(pericodiTrafo3);  //al abrir popup muestra barras actualizadas
    });
    $("#btnNuevaCompDinamica").click(function () {
        var pericodipd = parseInt($("#cbPeriodoCompDinamica").val()) || 0; 
        $("#frmNuevoEquipo").trigger("reset");
        abrirPopupNuevoEquipo(TIPO_COMPDINAMICA);
        verListadoBarras(pericodipd);  //al abrir popup muestra barras actualizadas
    });



    // COMBOS AÑOS
    $('#cbAnioBarra').change(function () {
        listadoPeriodo(TIPO_BARRA);
    });
    $('#cbAnioLinea').change(function () {
        listadoPeriodo(TIPO_LINEA);
    });
    $('#cbAnioTrafo2').change(function () {
        listadoPeriodo(TIPO_TRAFO2);
    });
    $('#cbAnioTrafo3').change(function () {
        listadoPeriodo(TIPO_TRAFO3);
    });
    $('#cbAnioCompDinamica').change(function () {
        listadoPeriodo(TIPO_COMPDINAMICA);
    });


    // COMBOS PERIODOS
    $('#cbPeriodoBarra').change(function () {
        fechaSegunComboEquipo("cbPeriodoBarra", "hf_new-equipo-vigencia-barra");        
        cargarListadoEquipos(TIPO_BARRA);
    });
    $('#cbPeriodoLinea').change(function () {
        fechaSegunComboEquipo("cbPeriodoLinea", "hf_new-equipo-vigencia-linea");        
        cargarListadoEquipos(TIPO_LINEA);
    });
    $('#cbPeriodoTrafo2').change(function () {
        fechaSegunComboEquipo("cbPeriodoTrafo2", "hf_new-equipo-vigencia-trafo2"); 
        cargarListadoEquipos(TIPO_TRAFO2);
    });
    $('#cbPeriodoTrafo3').change(function () {
        fechaSegunComboEquipo("cbPeriodoTrafo3", "hf_new-equipo-vigencia-trafo3");  
        cargarListadoEquipos(TIPO_TRAFO3);
    });
    $('#cbPeriodoCompDinamica').change(function () {
        fechaSegunComboEquipo("cbPeriodoCompDinamica", "hf_new-equipo-vigencia-compDinamica"); 
        cargarListadoEquipos(TIPO_COMPDINAMICA);
    });



    // COMBOS ESTADOS
    $('#cbEstadoBarra').change(function () {
        cargarListadoEquipos(TIPO_BARRA);
    });
    $('#cbEstadoLinea').change(function () {

        cargarListadoEquipos(TIPO_LINEA);
    });
    $('#cbEstadoTrafo2').change(function () {

        cargarListadoEquipos(TIPO_TRAFO2);
    });
    $('#cbEstadoTrafo3').change(function () {

        cargarListadoEquipos(TIPO_TRAFO3);
    });
    $('#cbEstadoCompDinamica').change(function () {

        cargarListadoEquipos(TIPO_COMPDINAMICA);
    });
    $('#cbBarraNomb').change(function () {
        var nombre = $("#cbBarraNomb").val();

        var select = document.getElementById('cbBarraNomb');
        var selectedOption = this.options[select.selectedIndex];

        $("#idnombreBarra").val(selectedOption.text);
    });

    $('#cbEBarraNomb').change(function () {
        var nombre = $("#cbEBarraNomb").val();

        var select = document.getElementById('cbEBarraNomb');
        var selectedOption = this.options[select.selectedIndex];

        $("#edit_equipo_nombre").val(selectedOption.text);
    });

    ANCHO_TABLA_EQ = ($('#mainLayout').width() - 30) + "px";

    /** CARGAR LISTADO DE EQUIPOS */
    cargarListadoEquipos(TIPO_BARRA);    

    /** CARGAR LISTADO DE EQUIPOS CADA VEZ QUE SE ELIJA UNA PESTAÑA (Si se agregan equipos, deben aparecer en las siguientes pestañas)*/
    $("#mi_tab_barra").on("click", function () {
        cargarListadoEquipos(TIPO_BARRA);
    });

    $("#mi_tab_linea").on("click", function () {
        cargarListadoEquipos(TIPO_LINEA);
    });

    $("#mi_tab_trafo2").on("click", function () {
        cargarListadoEquipos(TIPO_TRAFO2);
    });

    $("#mi_tab_trafo3").on("click", function () {
        cargarListadoEquipos(TIPO_TRAFO3);
    });

    $("#mi_tab_compDinamica").on("click", function () {
        cargarListadoEquipos(TIPO_COMPDINAMICA);
    });
});

function VerEquipo(pfreqpcodi, tipoEquipo) {
    cargarInterfazPropiedadXTipo(pfreqpcodi);
}


/** Carga lista de Equipos **/
function cargarListadoEquipos(tipo) {

    obj = ObtenerFiltroElegido(tipo);

    $.ajax({
        type: 'POST',
        url: controlador + "ListarEntidadXTipo",
        data: {
            pfrcatcodi: tipo,
            pericodi: obj.pericodiEscogido,
            estado: obj.estadoEscogido
        },
        success: function (evt) {
            var anchoDiv = ANCHO_TABLA_EQ;

            if (evt.Resultado != "-1") {
                var altotabla;
                var idTabla;
                switch (tipo) {
                    case TIPO_BARRA:
                        $('#listadoDeBarras').html('');
                        $('#listadoDeBarras').html(evt.Resultado);

                        $('#listadoDeBarras').css("width", anchoDiv);

                        altotabla = parseInt($('#listadoDeBarras').height()) || 0;
                        idTabla = "#tabla_lstBarras";
                        break;

                    case TIPO_LINEA:
                        $('#listadoDeLineas').html('');
                        $('#listadoDeLineas').html(evt.Resultado);

                        $('#listadoDeLineas').css("width", anchoDiv);

                        altotabla = parseInt($('#listadoDeLineas').height()) || 0;
                        idTabla = "#tabla_lstLineas";
                        break;

                    case TIPO_TRAFO2:
                        $('#listadoDeTrafo2').html('');
                        $('#listadoDeTrafo2').css("width", anchoDiv);

                        $('#listadoDeTrafo2').html(evt.Resultado);

                        altotabla = parseInt($('#listadoDeTrafo2').height()) || 0;
                        idTabla = "#tabla_lstTrafos2";
                        break;

                    case TIPO_TRAFO3:
                        $('#listadoDeTrafo3').html('');
                        $('#listadoDeTrafo3').css("width", anchoDiv);

                        $('#listadoDeTrafo3').html(evt.Resultado);

                        altotabla = parseInt($('#listadoDeTrafo3').height()) || 0;
                        idTabla = "#tabla_lstTrafos3";
                        break;

                    case TIPO_COMPDINAMICA:
                        $('#listadoDeCompDinamica').html('');
                        $('#listadoDeCompDinamica').css("width", anchoDiv);

                        $('#listadoDeCompDinamica').html(evt.Resultado);

                        altotabla = parseInt($('#listadoDeCompDinamica').height()) || 0;
                        idTabla = "#tabla_lstCompDinamica";
                        break;
                }

                $(document).ready(function () {
                    $(idTabla).dataTable({
                        "ordering": false,
                        "searching": true,
                        "iDisplayLength": 15,
                        "info": false,
                        "paging": false,
                        scrollCollapse: true,
                        "scrollX": true,
                        "scrollY": altotabla > 355 || altotabla == 0 ? 355 + "px" : "100%"
                    });
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
function listadoPeriodo(familiaEquipo) {

    var annio = -1;
    switch (familiaEquipo) {
        case TIPO_BARRA:
            annio = parseInt($("#cbAnioBarra").val()) || 0;
            $("#cbPeriodoBarra").empty();
            break;

        case TIPO_LINEA:
            annio = parseInt($("#cbAnioLinea").val()) || 0;
            $("#cbPeriodoLinea").empty();
            break;

        case TIPO_TRAFO2:
            annio = parseInt($("#cbAnioTrafo2").val()) || 0;
            $("#cbPeriodoTrafo2").empty();
            break;

        case TIPO_TRAFO3:
            annio = parseInt($("#cbAnioTrafo3").val()) || 0;
            $("#cbPeriodoTrafo3").empty();
            break;

        case TIPO_COMPDINAMICA:
            annio = parseInt($("#cbAnioCompDinamica").val()) || 0;
            $("#cbPeriodoCompDinamica").empty();
            break;
    }




    $.ajax({
        type: 'POST',
        url: controlador + "PeriodoListado",
        data: {
            anio: annio,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                if (evt.ListaPeriodo.length > 0) {
                    switch (familiaEquipo) {
                        case TIPO_BARRA:
                            $.each(evt.ListaPeriodo, function (i, item) {
                                $('#cbPeriodoBarra').get(0).options[$('#cbPeriodoBarra').get(0).options.length] = new Option(item.Pfrpernombre, item.Pfrpercodi);
                            });
                            fechaSegunComboEquipo("cbPeriodoBarra", "hf_new-equipo-vigencia-barra");                            
                            cargarListadoEquipos(TIPO_BARRA);
                            break;

                        case TIPO_LINEA:
                            $.each(evt.ListaPeriodo, function (i, item) {
                                $('#cbPeriodoLinea').get(0).options[$('#cbPeriodoLinea').get(0).options.length] = new Option(item.Pfrpernombre, item.Pfrpercodi);
                            });
                            fechaSegunComboEquipo("cbPeriodoLinea", "hf_new-equipo-vigencia-linea");                             
                            cargarListadoEquipos(TIPO_LINEA);
                            break;

                        case TIPO_TRAFO2:
                            $.each(evt.ListaPeriodo, function (i, item) {
                                $('#cbPeriodoTrafo2').get(0).options[$('#cbPeriodoTrafo2').get(0).options.length] = new Option(item.Pfrpernombre, item.Pfrpercodi);
                            });
                            fechaSegunComboEquipo("cbPeriodoTrafo2", "hf_new-equipo-vigencia-trafo2");  
                            cargarListadoEquipos(TIPO_TRAFO2);
                            break;

                        case TIPO_TRAFO3:
                            $.each(evt.ListaPeriodo, function (i, item) {
                                $('#cbPeriodoTrafo3').get(0).options[$('#cbPeriodoTrafo3').get(0).options.length] = new Option(item.Pfrpernombre, item.Pfrpercodi);
                            });
                            fechaSegunComboEquipo("cbPeriodoTrafo3", "hf_new-equipo-vigencia-trafo3");  
                            cargarListadoEquipos(TIPO_TRAFO3);
                            break;

                        case TIPO_COMPDINAMICA:
                            $.each(evt.ListaPeriodo, function (i, item) {
                                $('#cbPeriodoCompDinamica').get(0).options[$('#cbPeriodoCompDinamica').get(0).options.length] = new Option(item.Pfrpernombre, item.Pfrpercodi);
                            });
                            fechaSegunComboEquipo("cbPeriodoCompDinamica", "hf_new-equipo-vigencia-compDinamica");  
                            cargarListadoEquipos(TIPO_COMPDINAMICA);
                            break;
                    }
                } else {
                    switch (familiaEquipo) {
                        case TIPO_BARRA:
                            $('#cbPeriodoBarra').get(0).options[0] = new Option("--", "0");
                            break;

                        case TIPO_LINEA:
                            $('#cbPeriodoLinea').get(0).options[0] = new Option("--", "0");
                            break;

                        case TIPO_TRAFO2:
                            $('#cbPeriodoTrafo2').get(0).options[0] = new Option("--", "0");
                            break;

                        case TIPO_TRAFO3:
                            $('#cbPeriodoTrafo3').get(0).options[0] = new Option("--", "0");
                            break;

                        case TIPO_COMPDINAMICA:
                            $('#cbPeriodoCompDinamica').get(0).options[0] = new Option("--", "0");
                            break;
                    }
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

    var altotablaB = parseInt($('#listadoDeBarras').height()) || 0;
    $('#tabla_lstBarras').dataTable({
        "sPaginationType": "full_numbers",
        "destroy": "true",
        "ordering": false,
        "searching": true,
        "iDisplayLength": 15,
        "info": false,
        "paging": false,
        scrollCollapse: true,
        "scrollX": true,
        "scrollY": altotablaB > 355 || altotablaB == 0 ? 355 + "px" : "100%"
    });

    var altotablaL = parseInt($('#listadoDeLineas').height()) || 0;
    $('#tabla_lstLineas').dataTable({
        "sPaginationType": "full_numbers",
        "destroy": "true",
        "ordering": false,
        "searching": true,
        "iDisplayLength": 15,
        "info": false,
        "paging": false,
        scrollCollapse: true,
        "scrollX": true,
        "scrollY": altotablaL > 355 || altotablaL == 0 ? 355 + "px" : "100%"
    });

    var altotablaT2 = parseInt($('#listadoDeTrafo2').height()) || 0;
    $('#tabla_lstTrafos2').dataTable({
        "sPaginationType": "full_numbers",
        "destroy": "true",
        "ordering": false,
        "searching": true,
        "iDisplayLength": 15,
        "info": false,
        "paging": false,
        scrollCollapse: true,
        "scrollX": true,
        "scrollY": altotablaT2 > 355 || altotablaT2 == 0 ? 355 + "px" : "100%"
    });

    var altotablaT3 = parseInt($('#listadoDeTrafo3').height()) || 0;
    $('#tabla_lstTrafos3').dataTable({
        "sPaginationType": "full_numbers",
        "destroy": "true",
        "ordering": false,
        "searching": true,
        "iDisplayLength": 15,
        "info": false,
        "paging": false,
        scrollCollapse: true,
        "scrollX": true,
        "scrollY": altotablaT3 > 355 || altotablaT3 == 0 ? 355 + "px" : "100%"
    });

    var altotablaCD = parseInt($('#listadoDeCompDinamica').height()) || 0;
    $('#tabla_lstCompDinamica').dataTable({
        "sPaginationType": "full_numbers",
        "destroy": "true",
        "ordering": false,
        "searching": true,
        "iDisplayLength": 15,
        "info": false,
        "paging": false,
        scrollCollapse: true,
        "scrollX": true,
        "scrollY": altotablaCD > 355 || altotablaCD == 0 ? 355 + "px" : "100%"
    });

}

function ObtenerFiltroElegido(tipo) {
    var filtro = {};

    switch (tipo) {
        case TIPO_BARRA:
            filtro.estadoEscogido = $("#cbEstadoBarra").val();
            filtro.pericodiEscogido = parseInt($("#cbPeriodoBarra").val()) || 0;
            break;

        case TIPO_LINEA:
            filtro.estadoEscogido = $("#cbEstadoLinea").val();
            filtro.pericodiEscogido = parseInt($("#cbPeriodoLinea").val()) || 0;
            break;

        case TIPO_TRAFO2:
            filtro.estadoEscogido = $("#cbEstadoTrafo2").val();
            filtro.pericodiEscogido = parseInt($("#cbPeriodoTrafo2").val()) || 0;
            break;

        case TIPO_TRAFO3:
            filtro.estadoEscogido = $("#cbEstadoTrafo3").val();
            filtro.pericodiEscogido = parseInt($("#cbPeriodoTrafo3").val()) || 0;
            break;

        case TIPO_COMPDINAMICA:
            filtro.estadoEscogido = $("#cbEstadoCompDinamica").val();
            filtro.pericodiEscogido = parseInt($("#cbPeriodoCompDinamica").val()) || 0;
            break;
    }

    return filtro;
}


function abrirPopupNuevoEquipo(tipoEquipo) {

    resetearPopupAgregarEquipo();

    var codigoDisponibleEnBarra = $("#hfCodDisponibleBarra").val();
    var codigoDisponibleEnLinea = $("#hfCodDisponibleLinea").val();
    var codigoDisponibleEnTrafo2 = $("#hfCodDisponibleTrafo2").val();
    var codigoDisponibleEnTrafo3 = $("#hfCodDisponibleTrafo3").val();
    var codigoDisponibleEnCompDinamica = $("#hfCodDisponibleCompDinamica").val();

    var fecha = ""; 
    

    $("#bloque_add_equipo_periodo").css("display", "block");

    switch (tipoEquipo) {
        case TIPO_BARRA:
            fecha = $("#hf_new-equipo-vigencia-barra").val();
            $("#new-barra-id").val(codigoDisponibleEnBarra);
            $("#hfAddFamcodiEquipo").val(TIPO_BARRA);
            $("#bloque_add_equipo_id").css("display", "block");
            $("#bloque_add_equipo_grupo").css("display", "block");
            $("#bloque_add_eqObligatorio").css("display", "block");
            $("#bloque_add_equipo_nombre").css("display", "block");
            $("#bloque_add_equipo_tension").css("display", "block");
            $("#bloque_add_equipo_vmax").css("display", "block");
            $("#bloque_add_equipo_vmin").css("display", "block");
            $("#bloque_add_equipo_compreactiva").css("display", "block");
            break;

        case TIPO_LINEA:
            fecha = $("#hf_new-equipo-vigencia-linea").val();
            $("#new-barra-id").val(codigoDisponibleEnLinea);
            $("#hfAddFamcodiEquipo").val(TIPO_LINEA);
            $("#bloque_add_equipo_id").css("display", "block");
            $("#bloque_add_equipo_barra1").css("display", "block");
            $("#bloque_add_equipo_barra2").css("display", "block");
            $("#bloque_add_equipo_resistencia").css("display", "block");
            $("#bloque_add_equipo_reactancia").css("display", "block");
            $("#bloque_add_equipo_conductancia").css("display", "block");
            $("#bloque_add_equipo_admitancia").css("display", "block");
            $("#bloque_add_equipo_potenciamax").css("display", "block");
            break;

        case TIPO_TRAFO2:
            fecha = $("#hf_new-equipo-vigencia-trafo2").val();
            $("#new-barra-id").val(codigoDisponibleEnTrafo2);
            $("#hfAddFamcodiEquipo").val(TIPO_TRAFO2);
            $("#bloque_add_equipo_id").css("display", "block");
            $("#bloque_add_equipo_barra1").css("display", "block");
            $("#bloque_add_equipo_barra2").css("display", "block");
            $("#bloque_add_equipo_resistencia").css("display", "block");
            $("#bloque_add_equipo_reactancia").css("display", "block");
            $("#bloque_add_equipo_conductancia").css("display", "block");
            $("#bloque_add_equipo_admitancia").css("display", "block");
            $("#bloque_add_equipo_tap1").css("display", "block");
            $("#bloque_add_equipo_tap2").css("display", "block");
            $("#bloque_add_equipo_potenciamax").css("display", "block");
            break;

        case TIPO_TRAFO3:
            fecha = $("#hf_new-equipo-vigencia-trafo3").val();
            $("#new-barra-id").val(codigoDisponibleEnTrafo3);
            $("#hfAddFamcodiEquipo").val(TIPO_TRAFO3);
            $("#bloque_add_equipo_id").css("display", "block");
            $("#bloque_add_equipo_barra1").css("display", "block");
            $("#bloque_add_equipo_barra2").css("display", "block");
            $("#bloque_add_equipo_resistencia").css("display", "block");
            $("#bloque_add_equipo_reactancia").css("display", "block");
            $("#bloque_add_equipo_conductancia").css("display", "block");
            $("#bloque_add_equipo_admitancia").css("display", "block");
            $("#bloque_add_equipo_tap1").css("display", "block");
            $("#bloque_add_equipo_tap2").css("display", "block");
            $("#bloque_add_equipo_potenciamax").css("display", "block");
            break;

        case TIPO_COMPDINAMICA:
            fecha = $("#hf_new-equipo-vigencia-compDinamica").val();
            $("#new-barra-id").val(codigoDisponibleEnCompDinamica);
            $("#hfAddFamcodiEquipo").val(TIPO_COMPDINAMICA);
            $("#bloque_add_equipo_id").css("display", "block");
            $("#bloque_add_equipo_barra").css("display", "block");
            $("#bloque_add_equipo_qmax").css("display", "block");
            $("#bloque_add_equipo_qmin").css("display", "block");
            break;
    }

    $("#new-equipo-periodo").val(fecha); 

    setTimeout(function () {
        $('#popupNuevoEquipo').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown'
        });
    });
}


function cerrarPopup(id) {
    $(id).bPopup().close()
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

function validarEquipoAGuardar(dataForm, accion1, obj) {
    
    $.ajax({
        type: 'POST',
        url: controlador + 'ValidarEntidad',
        contentType: "application/json",
        dataType: 'json',
        data: JSON.stringify({
            pfrEntidad: dataForm,
            accion: accion1
        }
        ),
        cache: false,
        success: function (data) {
            if (data.Resultado == "1") {
                guardarEQP(dataForm, accion1, obj);
            } else {
                alert('Ha ocurrido un error : ' + data.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });

}


function guardarEquipoGeneral(dataForm, accion1) {

    var tipo;
    if (accion1 == NUEVO) {
        tipo = +$("#hfAddFamcodiEquipo").val();
    } else {
        if (accion1 == EDITAR) {
            tipo = +$("#hfFamcodiEquipo").val();
        }
    }
    var obj = ObtenerFiltroElegido(tipo);

    var mensajeErrores = validarFormularioEquipo(dataForm, tipo);

    dataForm.Pfrcatcodi = tipo;
    if (tipo == TIPO_COMPDINAMICA) dataForm.Pfrentcodibarragams = dataForm.Pfrentcodibarragamsd;

    AgregarEntidadDat(dataForm);

    if (mensajeErrores == "") {
        validarEquipoAGuardar(dataForm, accion1, obj);

    } else {
        alert(mensajeErrores);
    }
}

function guardarEQP(dataForm, accion1, obj) {
    $.ajax({
        type: 'POST',
        url: controlador + 'GuardarEntidad',
        contentType: "application/json",
        dataType: 'json',
        data: JSON.stringify({
            pfrEntidad: dataForm,
            accion: accion1,
            pericodi: obj.pericodiEscogido, 
        }
        ),
        cache: false,
        success: function (data) {
            if (data.Resultado == "1") {

                if (accion1 == NUEVO) {

                    $("#popupNuevoEquipo").bPopup().close()
                    $("#frmNuevoEquipo").trigger("reset");

                    switch (data.FamiliaEquipo) {
                        case TIPO_BARRA:
                            alert("Se registró correctamente la barra");

                            //Actualizar proximo codigo dispoible
                            $("#hfCodDisponibleBarra").val(data.CodigoDisponibleEquipo);

                            //Listar Barras
                            cargarListadoEquipos(TIPO_BARRA);
                            break;

                        case TIPO_LINEA:
                            alert("Se registró correctamente la Línea");

                            //Actualizar proximo codigo disponible
                            $("#hfCodDisponibleLinea").val(data.CodigoDisponibleEquipo);

                            //Listar Lineas
                            cargarListadoEquipos(TIPO_LINEA);
                            break;

                        case TIPO_TRAFO2:
                            alert("Se registró correctamente el Trafo2");

                            //Actualizar proximo codigo disponible
                            $("#hfCodDisponibleTrafo2").val(data.CodigoDisponibleEquipo);

                            //Listar Trafo2
                            cargarListadoEquipos(TIPO_TRAFO2);
                            break;

                        case TIPO_TRAFO3:
                            alert("Se registró correctamente el Trafo3");

                            //Actualizar proximo codigo disponible
                            $("#hfCodDisponibleTrafo3").val(data.CodigoDisponibleEquipo);

                            //Listar Trafo3
                            cargarListadoEquipos(TIPO_TRAFO3);
                            break;

                        case TIPO_COMPDINAMICA:
                            alert("Se registró correctamente la Compensación Dinámica");

                            //Actualizar proximo codigo disponible
                            $("#hfCodDisponibleCompDinamica").val(data.CodigoDisponibleEquipo);

                            //Listar Comp Dinamicas
                            cargarListadoEquipos(TIPO_COMPDINAMICA);
                            break;
                    }
                } else {
                    if (accion1 == EDITAR) {

                        $("#popupEditarEquipo").bPopup().close()
                        $("#frmEditarEquipo").trigger("reset");

                        switch (data.FamiliaEquipo) {
                            case TIPO_BARRA:
                                alert("Se editó correctamente la barra");

                                //Listar Barras
                                cargarListadoEquipos(TIPO_BARRA);
                                break;

                            case TIPO_LINEA:
                                alert("Se editó correctamente la Línea");

                                //Listar Lineas
                                cargarListadoEquipos(TIPO_LINEA);
                                break;

                            case TIPO_TRAFO2:
                                alert("Se editó correctamente el Trafo2");

                                //Listar Trafo2
                                cargarListadoEquipos(TIPO_TRAFO2);
                                break;

                            case TIPO_TRAFO3:
                                alert("Se editó correctamente el Trafo3");

                                //Listar Trafo3
                                cargarListadoEquipos(TIPO_TRAFO3);
                                break;

                            case TIPO_COMPDINAMICA:
                                alert("Se editó correctamente la Compensación Dinámica");

                                //Listar Comp Dinamicas
                                cargarListadoEquipos(TIPO_COMPDINAMICA);
                                break;
                        }
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


function resetearPopupAgregarEquipo() {
    $("#bloque_add_equipo_periodo").css("display", "none");
    $("#bloque_add_equipo_id").css("display", "none");
    $("#bloque_add_equipo_grupo").css("display", "none");
    $("#bloque_add_eqObligatorio").css("display", "none");
    $("#bloque_add_equipo_nombre").css("display", "none");
    $("#bloque_add_equipo_barra").css("display", "none");
    $("#bloque_add_equipo_tension").css("display", "none");
    $("#bloque_add_equipo_barra1").css("display", "none");
    $("#bloque_add_equipo_barra2").css("display", "none");
    $("#bloque_add_equipo_resistencia").css("display", "none");
    $("#bloque_add_equipo_reactancia").css("display", "none");
    $("#bloque_add_equipo_conductancia").css("display", "none");
    $("#bloque_add_equipo_admitancia").css("display", "none");
    $("#bloque_add_equipo_vmax").css("display", "none");
    $("#bloque_add_equipo_vmin").css("display", "none");
    $("#bloque_add_equipo_tap1").css("display", "none");
    $("#bloque_add_equipo_tap2").css("display", "none");
    $("#bloque_add_equipo_qmax").css("display", "none");
    $("#bloque_add_equipo_qmin").css("display", "none");
    $("#bloque_add_equipo_potenciamax").css("display", "none");
    $("#bloque_add_equipo_compreactiva").css("display", "none");
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

function fechaSegunComboEquipo(comboEquipo, hfEquipo) {
    var periodoEquipo = parseInt($("#" + comboEquipo).val()) || 0;
    var pEquipo = obtenerFechaIniNuevo(periodoEquipo);
    $("#" + hfEquipo).val(pEquipo);
}


function verListadoBarras(pericodi) { 

    $.ajax({ 
        type: 'POST', 
        url: controlador + "ListadoActualBarras", 
        data: { 
            pericodi: pericodi 
        }, 
        async: false, 
        success: function (evt) { 
            if (evt.Resultado != "-1") { 
                                
                $("#listado_barras").html(evt.HtmlBarras);
                $("#listado_barras1").html(evt.HtmlBarras1);
                $("#listado_barras2").html(evt.HtmlBarras2);

                $("#listado_E_barras").html(evt.HtmlEBarras);
                $("#listado_E_barras1").html(evt.HtmlEBarras1);
                $("#listado_E_barras2").html(evt.HtmlEBarras2);

            } else { 
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error: ' + evt.Mensaje); 
            } 
        }, 
        error: function (err) { 
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.'); 
        } 
    }); 
};
