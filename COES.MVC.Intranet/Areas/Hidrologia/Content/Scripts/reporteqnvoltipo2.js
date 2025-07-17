var controlador = siteRoot + 'hidrologia/';
$(function () {
    $('#cbEmpresa').multipleSelect({
        width: '222px',
        filter: true
    });

    $('#cbCuenca').multipleSelect({
        width: '222px',
        filter: true
    });

    $('#Fecha').Zebra_DatePicker({
        format: 'm Y'
    });

    $('#Anho').Zebra_DatePicker({
        format: 'Y',
        onSelect: function () {
            cargarSemanaAnho()
        }
    });
    $('#AnhoInicio').Zebra_DatePicker({
        format: 'Y'
    });
    $('#AnhoFin').Zebra_DatePicker({
        format: 'Y'
    });

    $('#cbUnidades').change(function () {
        listarPuntoMedicion();
    });

    $('#btnBuscar').click(function () {
        $('#listado').html("");
        $('#paginado').html("");
        //buscarDatos();
        mostrarListado();

    });
    $('#btnGrafico').click(function () {
        //var tipoInformacion = buscarTipoInformacion($('#cbTipoInformacion').val());
        //var valor = $("input[name='rbidTipo']:checked").val();

        //if ((tipoInformacion == 1) && (valor == 0)) // Diario x horas
            //pintarPaginado(0);
        generarGrafico(1);
    });
    $('#btnExpotar').click(function () {
        exportarExcel();
    });
    cargarPrevio();
    cargarSemanaAnho()
});

function cargarPrevio() {
    var fecha = new Date();
    var stFecha = fecha.getFullYear();
    $('#cbEmpresa').multipleSelect('checkAll');
    $('#cbCuenca').multipleSelect('checkAll');
    $('#Anho').val(stFecha);
    cargarSemanaAnho();
    $('#Semana select').val(1);
    $('#AnhoInicio').val(stFecha);
    $('#AnhoFin').val(stFecha);
    //*********************
    $('#idtrMes td').hide();
    $('#idTrSem td').show();
    $('#idTrYears td').hide();
    //***********************************
    $('input[name=rbidTipo][value=1]').attr('checked', true);
    var mes = "0" + (fecha.getMonth() + 1).toString();
    mes = mes.substr(mes.length - 2, mes.length);
    var stFecha = mes + " " + fecha.getFullYear();
    $('#Fecha').val(stFecha);
    $('#cbTipoInformacion').val($("#hfLectura").val());
    $('#cbUnidades').val(11);
    listarPuntoMedicion();
}

function cargarSemanaAnho() {
    var anho = $('#Anho').val();
    $('#hfAnho').val(anho);
    $.ajax({
        type: 'POST',
        url: controlador + 'reporte/CargarSemanas',

        data: { idAnho: $('#hfAnho').val() },

        success: function (aData) {

            $('#Semana').html(aData);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });

}

function mostrarListado() {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    var anho = $('#Anho').val();
    var anhoIni = $('#AnhoInicio').val();
    var anhoFin = $('#AnhoFin').val();
    var semana = $('#Semana select').val();
    var unidades = $('#cbUnidades').val();
    var cuenca = $('#cbCuenca').multipleSelect('getSelects');
    var valor = $("input[name='rbidTipo']:checked").val();
    var ptomedicion = $('#cbPtoMedicion').multipleSelect('getSelects');

    if (semana == undefined) semana = 0;
    if (empresa == "[object Object]") empresa = "-1";
    if (empresa == "") empresa = "-1";
    if (cuenca == "[object Object]") cuenca = "-1";
    if (cuenca == "") cuenca = "-1";

    $('#hfEmpresa').val(empresa);
    $('#hfAnho').val(anho);
    $('#hfAnhoInicio').val(anhoIni);
    $('#hfAnhoFin').val(anhoFin);
    $('#hfSemana').val(semana);
    $('#hfUnidad').val(unidades);
    $('#hfCuenca').val(cuenca);
    $('#hfPtoMedicion').val(ptomedicion);

    $('#hfidTipo').val(valor.toString());

    $.ajax({
        type: 'POST',
        url: controlador + "reporte/ListaQnVolTipo2",
        data: {
            idsEmpresa: $('#hfEmpresa').val(),
            idsCuenca: $('#hfCuenca').val(),
            idsPtoMedicion: $('#hfPtoMedicion').val(),
            unidad: $('#hfUnidad').val(),
            idLectura: $('#cbTipoInformacion').val(),
            fecha: $('#Fecha').val(),
            anho: $('#hfAnho').val(),
            anhoInicial: $('#hfAnhoInicio').val(),
            anhoFinal: $('#hfAnhoFin').val(),
            semana: $('#hfSemana').val(),
            rbDetalleRpte: $('#hfidTipo').val(),
            idsFamilia:"-1"
        },
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");
            $('#listado').html(evt);
            //if ($('#tabla th').length > 1) {
            //    $('#tabla').dataTable({
            //        "aoColumns": aoColumns(),
            //        "bSort": false,
            //        "scrollY": 430,
            //        "scrollX": true,
            //        "sDom": 't',
            //        "iDisplayLength": 50
            //    });
            //}
        },
        error: function () {
            alert("Ha ocurrido un error en lista");
        }
    });
}

function handleClick(myRadio) {
    currentValue = myRadio.value;
    cambiarFormatoFecha(currentValue);
}

function cambiarFormatoFecha(tipo) {
    switch (tipo) {
        case '1':// Semanal
            $('#idtrMes td').hide();
            $('#idTrSem td').show();
            $('#idTrYears td').hide();
            break;
        case '2': //Mensual
            $('#idtrMes td').show();
            $('#idTrSem td').hide();
            $('#idTrYears td').hide();

            break;
        case '3':// Anual
            $('#idtrMes td').hide();
            $('#idTrSem td').hide();
            $('#idTrYears td').show();
            break;
    }

}

function listarPuntoMedicion() {
    var unidad = $('#cbUnidades').val();
    $('#hfUnidad').val(unidad);
    $.ajax({
        type: 'POST',
        url: controlador + "reporte/ListarPuntosMedicion",
        data: {
            iUnidad: $('#hfUnidad').val()
        },
        success: function (evt) {
            $('#listPuntoMedicion').html(evt);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });

}