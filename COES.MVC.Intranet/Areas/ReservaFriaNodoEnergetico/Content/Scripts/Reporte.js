var controlador = siteRoot + 'reservafrianodoenergetico/reporte/';
var ancho = -1;

$(function() {
    
    $('#tab-container').easytabs({
        animate: false
    });

    $('#btnProcesar').click(function () {
        procesar();
    });
    
    $('#btnExportar').click(function () {
        exportar();
    });
    
    $(document).ready(function () {

        var modulo = $('#hfNrsmodCodi').val();
        $('#cbNrsmodCodi').val(modulo);

        ocultarTab(modulo);

        if (ancho == -1)
            ancho = $('#mainLayout').width();
    });

    $('#cbNrsmodCodi').change(function () {
        ocultarTab($('#cbNrsmodCodi').val());
        eliminardatos();
    });
});


ocultarTab = function (modulo) {
    
    if (modulo == 1) {
        $('#ede2').show();
        $('#edeipp2').hide();
        $('#edeipf2').hide();
        $('#edeitp2').hide();
        $('#edeitf2').hide();

        $('#hora2').hide();
        $('#sitf2').hide();
        $('#sipf2').hide();
        $('#horada2').show();
        $('#horampe2').show();
        $('#horamce2').show();

    } else {
        $('#ede2').hide();
        $('#edeipp2').show();
        $('#edeipf2').show();
        $('#edeitp2').show();
        $('#edeitf2').show();

        $('#hora2').show();
        $('#sitf2').show();
        $('#sipf2').show();
        $('#horada2').hide();
        $('#horampe2').hide();
        $('#horamce2').hide();
    }
}


eliminardatos= function() {
    $('#ederpta').html("");
    $('#edeipprpta').html("");
    $('#edeipfrpta').html("");
    $('#edeitprpta').html("");
    $('#edeitfrpta').html("");
    $('#hdarpta').html("");
    $('#hmperpta').html("");
    $('#hmcerpta').html("");
    $('#sitfrpta').html("");
    $('#sipfrpta').html("");
    $('#horarpta').html("");    
}

exportar = function () {
    
    var modulo = $('#cbNrsmodCodi').val();
    var periodo = $('#cbNrperCodi').val();
    
    if (modulo == 0) {
        alert('Debe elegir Submódulo');
        return;
    }

    if (periodo == 0) {
        alert('Debe elegir Periodo');
        return;
    }

    
    $.ajax({
        type: 'POST',
        url: controlador + "reporte",
        data: {
            nrsmodcodi: modulo,
            nrpercodi: periodo
        },
        success: function (resultado) {
           
            if (resultado != 1) {
                mostrarError();
            } else {
                if (modulo == 1) {
                    window.location = controlador + "descargarReserva";
                } else {
                    window.location = controlador + "descargarNodo";
                }
            }

        },
        error: function () {
            mostrarError();
        }
    });
}

procesar = function () {
    var modulo = $('#cbNrsmodCodi').val();
    var periodo = $('#cbNrperCodi').val();

    eliminardatos();


    if (modulo == 1) {
        //obtener EDE Reserva Fria
        mostrarEde(1,periodo , 'ederpta');
        mostrarProceso(1, periodo, 'hdarpta',true);
        mostrarProceso(2, periodo, 'hmperpta', true);
        mostrarProceso(3, periodo, 'hmcerpta', true);
        
    } else {
        //obtener EDE Nodo - IPP
        mostrarEde(2, periodo, 'edeipprpta');
        //obtener EDE Nodo - IPF
        mostrarEde(3, periodo, 'edeipfrpta');
        //obtener EDE Nodo - ITP
        mostrarEde(4, periodo, 'edeitprpta');
        //obtener EDE Nodo - ITF
        mostrarEde(5, periodo, 'edeitfrpta');

        //sobrecosto itf
        mostrarProceso(4, periodo, 'sitfrpta',false);

        //sobrecosto ipp
        mostrarProceso(5, periodo, 'sipfrpta', false);
        mostrarProcesoHoraNodo(periodo, 'horarpta');
    }    
}

mostrarEde = function (idEde, periodo, tag) {
    var tagtabla = "tabla" + tag;
    
    $.ajax({
        type: 'POST',
        url: controlador + "listaede",
        data: {
            idEde: idEde,
            periodo: periodo,
            tag: tagtabla
        },
        success: function (evt) {
            
            $('#' + tag).css("width", ancho + "px");
            $('#' + tag).html(evt);
            $('#' + tagtabla).dataTable({
                "scrollY": 230,
                "scrollX": true,
                "sDom": 't',
                "ordering": false,
                "iDisplayLength": 50,
                "bDestroy": true
            });

        },
        error: function() {
            mostrarError();
        }
    });
}

mostrarProceso = function (idProceso, periodo, tag, hora) {
    var tagtabla = "tabla" + tag;
    
    $.ajax({
        type: 'POST',
        url: controlador + "listaproceso",
        data: {
            idProceso: idProceso,
            periodo: periodo,
            tag: tagtabla,
            hora: hora
        },
        success: function (evt) {
            $('#' + tag).css("width", ancho + "px");
            $('#' + tag).html(evt);
            $('#' + tagtabla).dataTable({
                "scrollY": 130,
                "scrollX": true,
                "sDom": 't',
                "ordering": false,
                "iDisplayLength": 50,
                "bDestroy": true
            });

        },
        error: function () {
            mostrarError();
        }
    });
}

mostrarProcesoHoraNodo = function (periodo, tag) {
    var tagtabla = "tabla" + tag;
    
    $.ajax({
        type: 'POST',
        url: controlador + "listaprocesohoranodo",
        data: {
            periodo: periodo,
            tag: tagtabla
        },
        success: function (evt) {
            $('#' + tag).css("width", ancho + "px");
            $('#' + tag).html(evt);
            $('#' + tagtabla).dataTable({
                "scrollY": 130,
                "scrollX": true,
                "sDom": 't',
                "ordering": false,
                "iDisplayLength": 50,
                "bDestroy": true
            });

        },
        error: function () {
            mostrarError();
        }
    });
}

mostrarError = function () {
    alert('Ha ocurrido un error.');
}


