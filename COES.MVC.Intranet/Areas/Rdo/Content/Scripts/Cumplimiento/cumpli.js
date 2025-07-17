var controlador = siteRoot + 'rdo/Cumplimiento/';

var CantidadFilas = 20;

var formatoSeleccionado = 0;

$(document).ready(function () {

    $('#filaInformeFinal').hide(); 
    $('#listadoNoFalla').hide();
    $('#listadoFalla').hide();
    $('#filaNoInformeFinal').hide();
    $('#btnExportar').hide();
    $('#txtFchPresentacion').inputmask({
        mask: "1/2/y",
        placeholder: "dd/mm/yyyy",
        alias: "datetime"
    });

    $('#txtFchPresentacion').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#txtFchPresentacion').val(date);
        }
    });


    $('#txtFchFalla').inputmask({
        mask: "1/y",
        placeholder: "mm/yyyy",
        alias: "datetime"
    });


    $('#txtFchFalla').Zebra_DatePicker({
        readonly_element: false,
        format: 'm/Y',
        onSelect: function (date) {
            $('#txtFchFalla').val(date);
        }
    });


    $("#btnBuscar").click(function () {
        var formato = $('#cboEstado').val();

        if (formato == "0") {
            return;
        }

        if (formato == "130") {
            buscarFallas();
        } else {
            buscar()
        }
    })

    $('#btnExportar').click(function () {
        exportarExcel();
    });

    $("#cboEstado").change(function () {
        var formato = $('#cboEstado').val();

        $('#listadoNoFalla').hide();
        $('#listado').hide();
        $('#listadoFallas').hide();
        $('#listadoFalla').hide();

        if (formato == "130") {
            $("#filaInformeFinal").show();  
            $('#filaNoInformeFinal').hide();
            $('#cboTipoInforme').val("0");
            $('#cboEtapaInforme').val("0");
            $('#txtFchPresentacion').val("");
          //  $('#btnExportar').hide();
            $('#btnExportar').show();


        } else {
            $("#filaInformeFinal").hide(); 
            $('#cboTipoInforme').val("0");
            $('#cboEtapaInforme').val("0");
            $('#txtFchFalla').val("");
            $('#filaNoInformeFinal').show();
            $('#txtFchPresentacion').val("");
            $('#btnExportar').show();

        }
        if (formato == "0") {
            $('#filaNoInformeFinal').hide()
            $('#btnExportar').hide();
        }

    });

    //setTimeout(function () { buscar() }, 500);

    //buscar();
});

function buscar() {

    //pintarPaginado();
    pintarBusqueda(1);
}

function buscarFallas() {

    //pintarPaginadoFallas();
    pintarBusquedaFallas(1)
}

function pintarBusqueda(nroPagina) {

    $('#hfNroPagina').val(nroPagina);

    var dataFiltros = filtros();

    $.ajax({
        type: 'POST',
        url: controlador + "Listado",
        data: dataFiltros,
        success: function (evt) {
            $('#listadoNoFalla').show();
            $('#listado').show();
            $('#listado').css("width", $('#mainLayout').width() + "px");

            $('#listado').html(evt);
            $('#tabla').dataTable({
                "scrollY": 430,
                "scrollX": false,
                'searching': false,
                "ordering": false,
                "paging": false,
                "pagingType": "full_numbers",
                "iDisplayLength": -1,
                "stripeClasses": []
            });
          
        },
        error: function () {
            //mostrarError();
        }
    });
}

function pintarBusquedaFallas(nroPagina) {

    $('#hfNroPagina').val(nroPagina);

    var dataFiltros = filtros(nroPagina);

    $.ajax({
        type: 'POST',
        url: controlador + "ListadoFallas",
        data: dataFiltros,
        success: function (evt) {
            $('#listadoFallas').show();
            $('#listadoFalla').show();
            $('#listadoFallas').css("width", $('#mainLayout').width() + "px");
            console.log(evt)
            if (evt.HtmlReporte != "") {
                $('#listadoFallas').html(evt);
                $('#tablaFallas').dataTable({
                    "scrollY": 430,
                    "scrollX": false,
                    'searching': false,
                    "ordering": false,
                    "paging": false,
                    "pagingType": "full_numbers",
                    "iDisplayLength": 50
                });
            }

           
       
        },
        error: function () {
            //mostrarError();
        }
    });
}

function CargarRegistros() {

    //pintarPaginado();
    pintarBusqueda(1);
}

//function pintarPaginado() {
//    var dataFiltros = filtros(1);

//    $.ajax({
//        type: 'POST',
//        url: controlador + "paginado",
//        data: dataFiltros,
//        success: function (evt) {
//            $('#paginado').html(evt);
//            mostrarPaginado();

//            $("#ddlCantidadRegistros").val(CantidadFilas);
//        },
//        error: function () {
//            //mostrarError('mensaje');
//        }
//    });
//}

//function pintarPaginadoFallas() {
//    var dataFiltros = filtros(1);

//    $.ajax({
//        type: 'POST',
//        url: controlador + "paginado",
//        data: dataFiltros,
//        success: function (evt) {
//            $('#paginadoFallas').html(evt);
//            mostrarPaginado();

//            $("#ddlCantidadRegistros").val(CantidadFilas);
//        },
//        error: function () {
//            //mostrarError('mensaje');
//        }
//    });
//}

//function nuevoEstudioEo(id) {
//    window.location = controlador + "RegistrarEstudioEo/" + id;
//}

//function revisionEstudioEo(id) {
//    window.location = controlador + "revision/" + id;
//}

//function noVigenciaEstudioEo(id) {
//    window.location = controlador + "EstablecerNoVigencia?Esteocodi=" + id;
//}

//function anular(esteocodi) {
//    mostrarConfirmacion("¿Confirma que desea anular el estudio seleccionado?", AnularEstudioEO, esteocodi);
//}

//function AnularEstudioEO(parametros) {
//    $.ajax({
//        url: controlador + 'Anular',
//        type: 'POST',
//        data: { Esteocodi: parametros[0] },
//        success: function (d) {
//            if (d.bResult) {
//                $('#popupZ').bPopup().close();
//                $("#popupConfirmarOperacion .b-close").click();

//                mostrarMensajePopup("El registro se actualizó correctamente!.", 1);

//                buscar();
//            }
//            else { alert(d.sMensaje); }
//        }
//    });
//}

function filtros(nroPagina, nroFilas) {
    var filas = 20;

    if (typeof $("#ddlCantidadRegistros").val() != "undefined") {
        filas = $("#ddlCantidadRegistros").val();
    }

    CantidadFilas = filas;

    var filtro = {
        sRdofechaini: $('#txtFchPresentacion').val(),
        codFormato: $('#cboEstado').val(),
        Emprcoditp: $('#cboTitularProyecto').val(),
        Esteonomb: $('#txtNombreEstudio').val(),
        Esteopuntoconexion: $('#txtPuntoConexion').val(),
        Esteocodiproy: $('#txtCodigioProyecto').val(),
        Esteoanospuestaservicio: $('#txtAnioPuestoServicio').val(),
        nroPagina: nroPagina,
        nroFilas: filas,
        tipoProyecto: $('#cboTipoProyecto').val(),
        Periodo: $('#txtFchFalla').val(),
        TipoFalla: $('#cboTipoInforme').val(),
        EtapaInforme: $('#cboEtapaInforme').val()

    }

    return filtro;
}


function exportarExcel() {
    //var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    //var formato = $('#cbFormato').multipleSelect('getSelects');
    //var lectura = $('#cbLectura').multipleSelect('getSelects');
    //var estado = $('#cbEstado').multipleSelect('getSelects');

    //if (empresa == "[object Object]") empresa = "-1";
    //if (empresa == "") empresa = "-1";
    //if (formato == "[object Object]") formato = "-1";
    //if (formato == "") formato = "-1";
    //if (lectura == "[object Object]") lectura = "-1";
    //if (lectura == "") lectura = "-1";
    //if (estado == "[object Object]") estado = "-1";
    //if (estado == "") estado = "-1";
    //$('#hfEmpresa').val(empresa);
    //$('#hfFormato').val(formato);
    //$('#hfLectura').val(lectura);
    //$('#hfEstado').val(estado);
    var dataFiltros = filtros(1);

    $.ajax({
        type: 'POST',
        url: controlador + 'ExcelCumplimiento',
        /*dataType: 'json',*/
        data: dataFiltros,
        success: function (result) {
            if (result == 1)
                window.location = controlador + "ExportarReporte";
            if (result == -1)
                alert("Error en exportar reporte...");
        },
        error: function () {
            alert("Error en reporte...");
        }
    });
}