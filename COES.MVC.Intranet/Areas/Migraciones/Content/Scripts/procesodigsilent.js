var controlador = siteRoot + 'Migraciones/DigSilent/';
var ancho = 900;
var DATA_REPORTE = '';

$(function () {
    $("#btnOcultarMenu").click();

    $('#tab-container').easytabs({ animate: false });
    $('#tab-container').bind('easytabs:after', function () {
        var id = $("#tab-container1 li.tab.active").attr("id");
        if (id == 'tab_3')
            refreshDatatable();
    });

    $('.txtFecha').inputmask({
        mask: "1/2/y",
        placeholder: "dd/mm/yyyy",
        alias: "datetime"
    });

    $('.txtFecha').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function () {
            cargarInformacionYupana();
        }
    });

    $('.txtFecha').change(function () {
        cargarInformacionYupana();
    });
    //$('#txtFecha').Zebra_DatePicker();

    $('#btnSelectFecha').click(function () {
        var objConf = {
            idPopup: '#popupSeleccionarFecha',
            idFechaInicio: '#txtFecha',
            idFechaFin: '#txtFechaFin',
            titulo: 'Seleccionar fechas'
        }
        selectDate_mostrarPopup(objConf);
    });

    $("#btnMoveRight").on("click", function (e) {
        var selectedOptions = $('#lbTeamA > option:selected');
        if (selectedOptions.length == 0) {
            alert("Seleccionar al menos un numero y mover");
            return false;
        }

        $('#lbTeamA > option:selected').appendTo('#lbTeamB');
        e.preventDefault();

        var opt = $("#lbTeamB option").sort(function (a, b) { return a.value.toUpperCase().localeCompare(b.value.toUpperCase()) });
        $("#lbTeamB").append(opt);
        AllRight();
    });

    $("#btnMoveAllRight").on("click", function (e) {
        $('#lbTeamA > option').appendTo('#lbTeamB');
        e.preventDefault();

        var opt = $("#lbTeamB option").sort(function (a, b) { return a.value.toUpperCase().localeCompare(b.value.toUpperCase()) });
        $("#lbTeamB").append(opt);
        AllRight();
    });

    $("#btnMoveLeft").on("click", function (e) {
        var selectedOptions = $('#lbTeamB > option:selected');
        if (selectedOptions.length == 0) {
            alert("Seleccionar al menos un numero y mover");
            return false;
        }

        $('#lbTeamB > option:selected').appendTo('#lbTeamA');
        e.preventDefault();

        var opt = $("#lbTeamA option").sort(function (a, b) { return a.value.toUpperCase().localeCompare(b.value.toUpperCase()) });
        $("#lbTeamA").append(opt);
        noAllLeft();
    });

    $("#btnMoveAllLeft").on("click", function (e) {
        $('#lbTeamB > option').appendTo('#lbTeamA');
        e.preventDefault();

        var opt = $("#lbTeamA option").sort(function (a, b) { return a.value.toUpperCase().localeCompare(b.value.toUpperCase()) });
        $("#lbTeamA").append(opt);
        noAllLeft();
    });

    $("#btnProcesar").click(function () {
        AllRight();
        if (validarCampos()) { ProcesarDatos(); }
    });

    $("#btnGrabarDig").click(function () {
        SaveDigSilent();
    });

    $('#btnIrEquivalencia').on('click', function () {
        document.location.href = siteRoot + 'IEOD/Configuracion/EquivalenciaEquipoScada/';
    });

    $("#cbProg").val("4");

    mostrarTrFuenteReprog();
    $("input[name=cbDespachoSICOES]").click(function () {
        mostrarTrFuenteReprog();
    });
    $('#cbProg1').change(function () {
        mostrarTrFuenteReprog();
    });
    $("input[name=cbFuenteReprog]").click(function () {
        cargarInformacionYupana();
    });

    ancho = $("#mainLayout").width() > 900 ? $("#mainLayout").width() - 30 : 900;

});

function AllRight() {
    for (var i = 0; i < $("#lbTeamB option").length; i++) {
        $("#lbTeamB option")[i].selected = true;
    }
}

function noAllLeft() {
    for (var i = 0; i < $("#lbTeamA option").length; i++) {
        $("#lbTeamA option")[i].selected = false;
    }
}

function validarCampos() {
    var bloq_ = $('#lbTeamB').val();
    var vFecha = $('#txtFecha').val();
    $('#hdBloques').val(bloq_);
    if ($('#hdBloques').val() == "") {
        alert("Debe al menos seleccionar un bloque de Horario");
        return false;
    }
    else {
        if (validarFormatoFech(vFecha) == false) {
            alert("La fecha ingresada no tiene el formato correcto");
            return false;
        } 
    }

    var parametro1 = $('input[name=cbDespachoSICOES]:checked').val();
    var comboEscogido = parametro1 == 1 ? $('#cbProg1').val() : $("#cbProgY1").val();
    var fuenteReprog = $('input[name=cbFuenteReprog]:checked').val();
    if (comboEscogido == "5" && fuenteReprog == "2") {
        var topcodiYupana = parseInt($('input[name=cbTopcodi]:checked').val()) || 0;
        if (topcodiYupana <= 0) {
            alert("Debe seleccionar Escenario YUPANA");
            return false;
        }
    }

    return true;
};

function ProcesarDatos() {
    var nroChk;
    for (var x = 0; x <= 5; x++) {
        if ($('#rd' + x).is(":checked")) { nroChk = $('#rd' + x).val(); }
    }
    var bloq_ = $('#lbTeamB').val();
    $('#hdBloques').val(bloq_);

    var parametro1 = $('input[name=cbDespachoSICOES]:checked').val();
    var comboEscogido = parametro1 == 1 ? $('#cbProg1').val() : $("#cbProgY1").val();

    var fuenteReprog = $('input[name=cbFuenteReprog]:checked').val();

    var topcodiYupana = 0;
    if (comboEscogido == "5" && fuenteReprog == "2") {
        topcodiYupana = parseInt($('input[name=cbTopcodi]:checked').val()) || 0;
    }

    $("#tab_3").hide();
    $("#listaConfiguracionOpera").html('');
    $("#listadoDigsilent").html('');
    $("#listaObservaciones").html('');
    $("#validacionForeignKey").html();
    $("#validacionForeignKey").hide();

    $.ajax({
        type: 'POST',
        url: controlador + "ProcesarDigsilent",
        dataType: 'json',
        data: {
            program: comboEscogido, fecha: $('#txtFecha').val(), rdchk: nroChk, bloq: $('#hdBloques').val(),
            fuente: fuenteReprog,
            topcodiYupana: topcodiYupana
        },
        success: function (evt) {
            if (evt.nRegistros != -1) {
                $("#barra").show();

                var msj_resu = $("#listadoDigsilent").html();
                $("#listadoDigsilent").html(msj_resu + evt.Resultado);
                $('#listadoDigsilent').css("height", "500px");
                $("#listadoDigsilent").css('overflowX', 'hidden');

                $("#listaObservaciones").html(evt.Comentario);
                $('#listaObservaciones').css("height", "500px");
                $("#listaObservaciones").css('overflowX', 'hidden');

                DATA_REPORTE = evt.Resultado2;
                if (DATA_REPORTE != null && DATA_REPORTE != '') {
                    $('#tab-container').easytabs('select', '#configuracionOpera');

                    $("#tab_3").show();
                    refreshDatatable();

                } else {
                    $('#tab-container').easytabs('select', '#file_digsilent');
                }

                //Observaciones
                var msjError = '';
                if (evt.Comentario != null && evt.Comentario != '')
                    msjError = "Existen Observaciones. <br>";

                if (evt.Resultado3 != null && evt.Resultado3 != '') {
                    msjError += evt.Resultado3;
                }
                if (msjError != '') {
                    $("#validacionForeignKey").html(msjError);
                    $("#validacionForeignKey").show();
                }
            }
            else { alert("Ha ocurrido un error: " + evt.Mensaje); }
        },
        error: function (err) { alert("Ha ocurrido un error."); }
    });
}

function selectAll() {
    $("#lbTeamA option").attr("selected", "selected");
    $("#lbTeamB option").attr("selected", "selected");
}

function SaveDigSilent() {
    var str = $("#listadoDigsilent").html();
    $("#hdDigsilent").val(str);
    var tex_ = $("#hdDigsilent").val();
    var str_replace = tex_.replace(/<br>/gi, "~");
    $.ajax({
        type: 'POST',
        url: controlador + "SaveDigSilent",
        dataType: 'json',
        data: { texto_: str_replace, fecha: $("#txtFecha").val() },
        success: function (evt) {
            if (evt.nRegistros > 0) {
                window.location = controlador + "Exportar?fi=" + evt.Resultado;
            } else { alert("No se genero el archivo..."); }
        },
        error: function (err) { alert("Ha ocurrido un error."); }
    });
}

function refreshDatatable(id) {
    $("#listaConfiguracionOpera").html('');

    $("#listaConfiguracionOpera").html(DATA_REPORTE);

    $('#tablaConfOpera').dataTable({
        "language": {
            "emptyTable": "¡No existe configuración!"
        },
        //"order": [[0, 'asc']],
        "ordering": false,
        "destroy": "true",
        "info": false,
        "searching": true,
        "paging": false,
        "scrollY": 500 + "px"
    });
}

function getfechita(x) {
    var arr = x.split('/');
    var monthNames = ["Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Set", "Oct", "Nov", "Dic"];
    return arr[0] + monthNames[parseInt(arr[1]) - 1] + arr[2];
}

function validarFormatoFech(obj) {
    var patron = /^\d{1,2}\/\d{2}\/\d{4}$/
    if (!patron.test(obj)) {
        return false;
    }
    return true;
}

function mostrarTrFuenteReprog() {
    $("#trFuenteReprog").hide();

    var parametro1 = $('input[name=cbDespachoSICOES]:checked').val();
    if (parametro1 == 1) {
        var lectParam1 = $('#cbProg1').val();
        if (lectParam1 == 5) //REPROGRAMACIÓN DIARIA
            $("#trFuenteReprog").show();
    }

    cargarInformacionYupana();
}

var table = null;
function cargarInformacionYupana() {

    var tipoProg = $("#cbProg1").val();
    var fuente = $('input[name=cbFuenteReprog]:checked').val();

    //reprogramacion diaria y fuente preliminar
    if (tipoProg == "5" && fuente == "2") {
        $.ajax({
            type: 'POST',
            url: controlador + "CargarInformacionYupana",
            dataType: 'json',
            data: {
                fecha: $('#txtFecha').val()
            },
            success: function (evt) {
                if (evt.nRegistros != -1) {
                    $("#div_informacion_yupana").html(evt.Resultado);

                    table = $('#tabla_topologia').dataTable({
                        "language": {
                            "emptyTable": "¡No existe escenarios!"
                        },
                        //"order": [[0, 'asc']],
                        "ordering": false,
                        "destroy": "true",
                        "info": false,
                        "searching": false,
                        "paging": false,
                    });

                    $("input[name=cbTopcodi]").unbind();
                    $("input[name=cbTopcodi]").click(function () {

                        var valor = $('input[name=cbTopcodi]:checked').val();

                        var idfila = "#fila_esc_" + valor;
                        if ($(idfila).hasClass('selected')) {
                            $(idfila).removeClass('selected');
                        }
                        else {
                            table.$('tr.selected').removeClass('selected');
                            $(idfila).addClass('selected');
                        }
                    });
                }
                else { alert("Ha ocurrido un error: " + evt.Mensaje); }
            },
            error: function (err) { alert("Ha ocurrido un error."); }
        });
    } else {
        $("#div_informacion_yupana").html("");
    }
}