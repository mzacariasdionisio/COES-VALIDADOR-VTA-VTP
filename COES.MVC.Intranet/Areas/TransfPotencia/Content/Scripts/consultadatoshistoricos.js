var controler = siteRoot + "transfpotencia/consultadatoshistoricos/";

var error = [];

$(document).ready(function () {
    $('#tab-container').easytabs({
        animate: false
    });

    $('#emprcodi').multipleSelect({
        width: '220px',
        filter: true,
        selectAll: false,
        single: true,
        onClick: function (view) {
            var emprcodi = $("#emprcodi").multipleSelect('getSelects');
            $('#hfComboEmpresa').val(emprcodi);
            if ($('#pegrtipoinformacion option:selected').val() == 2) {
                obtenerCargosEmpresasGeneradoras();
            }
        },
        onClose: function (view) {
            var emprcodi = $("#emprcodi").multipleSelect('getSelects');
            $('#hfComboEmpresa').val(emprcodi);
        }
    });

    $('#emprcodi2').multipleSelect({
        width: '220px',
        filter: true,
        selectAll: false,
        single: true,
        onClick: function (view) {
            var emprcodi = $("#emprcodi2").multipleSelect('getSelects');
            $('#hfComboEmpresa2').val(emprcodi);
        },
        onClose: function (view) {
            var emprcodi = $("#emprcodi2").multipleSelect('getSelects');
            $('#hfComboEmpresa2').val(emprcodi);
        }
    });

    //$("#cargo").multipleSelect({
    //    width: '220px',
    //    filter: true
    //});

    //$('#cargo').multipleSelect('checkAll');

    $("#pegrtipoinformacion").on('change', function () {
        
        obtenerEmpresas(1);
        if ($('#pegrtipoinformacion option:selected').val() == 0) {
            $("#tdCentral").css('display', 'table-cell');
            $("#tdCentral1").css('display', 'table-cell');
            $("#tdCargo").css('display', 'none');
            $("#tdCargo1").css('display', 'none');
        } else if ($('#pegrtipoinformacion option:selected').val() == 1) {
            $("#tdCentral").css('display', 'none');
            $("#tdCentral1").css('display', 'none');
            $("#tdCargo").css('display', 'none');
            $("#tdCargo1").css('display', 'none');
        } else {
            obtenerEmpresasGeneradoras(1);
            $("#tdCentral").css('display', 'none');
            $("#tdCentral1").css('display', 'none');
            $("#tdCargo").css('display', 'table-cell');
            $("#tdCargo1").css('display', 'table-cell');
        }
    });

    $("#pegrtipoinformacion2").on('change', function () {
        
        if ($('#pegrtipoinformacion2 option:selected').val() == 0 || $('#pegrtipoinformacion2 option:selected').val() == 1) {
            obtenerEmpresas(2);
        }
        else {
            obtenerEmpresasGeneradoras(2);
        }
    });

    $("#periinicio").on('change', function () {
        if ($('#pegrtipoinformacion option:selected').val() == 2) {
            obtenerEmpresasGeneradoras(1);
        }
    });

    $("#periinicio1").on('change', function () {
        if ($('#pegrtipoinformacion option:selected').val() == 2) {
            obtenerEmpresasGeneradoras(2);
        }
    });

    $("#emprcodi").on('change', function () {
        
    });

    $('#btnConsultarVista').click(function () {
        mostrarListado();
    });

    $('#btnConsultarComparacion').click(function () {
        mostrarComparacion();
    });

    $('#btnDescargarExcelConsulta').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivo(1);
    });

    $('#btnDescargarComparacion').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarComparacion(1);
    });
    

    init();
    

});

initPluginEmpresas = function (option) {
    if (option == 1) {
        $('#emprcodi').multipleSelect({
            width: '220px',
            filter: true,
            selectAll: false,
            single: true,
            onClick: function (view) {
                var emprcodi = $("#emprcodi").multipleSelect('getSelects');
                $('#hfComboEmpresa').val(emprcodi);
            },
            onClose: function (view) {
                var emprcodi = $("#emprcodi").multipleSelect('getSelects');
                $('#hfComboEmpresa').val(emprcodi);
            }
        });
    } else {
        $('#emprcodi2').multipleSelect({
            width: '220px',
            filter: true,
            selectAll: false,
            single: true,
            onClick: function (view) {
                var emprcodi = $("#emprcodi2").multipleSelect('getSelects');
                $('#hfComboEmpresa2').val(emprcodi);
            },
            onClose: function (view) {
                var emprcodi = $("#emprcodi2").multipleSelect('getSelects');
                $('#hfComboEmpresa2').val(emprcodi);
            }
        });
    }
    
}

initPluginCargo = function () {
    $('#cargo').multipleSelect({
        width: '220px',
        filter: true,
        onClick: function (view) {
            var emprcodi = $("#cargo").multipleSelect('getSelects');
            $('#hfComboCargo').val(emprcodi);
        },
        onClose: function (view) {
            var emprcodi = $("#cargo").multipleSelect('getSelects');
            $('#hfComboCargo').val(emprcodi);
        }
    });
    $('#cargo').multipleSelect('checkAll');

}

obtenerEmpresas = function (option) {
    $.ajax({
        type: 'POST',
        url: controler + "ObtenerEmpresas",
        dataType: 'json',
        success: function (evt) {
            setEmpresas(evt, option);
        },
        error: function (err) {
            console.log(err);
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

obtenerEmpresasGeneradoras = function (option) {
    $.ajax({
        type: 'POST',
        url: controler + "ObtenerEmpresasGeneradoras",
        data: {
            periini: option == 1 ? $("#periinicio").val() : $("#periinicio1").val()
        },
        dataType: 'json',
        success: function (evt) {
            setEmpresasGeneradoras(evt, option);
        },
        error: function (err) {
            console.log(err);
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

obtenerCargosEmpresasGeneradoras = function () {
    $.ajax({
        type: 'POST',
        url: controler + "ObtenerCargos",
        data: {
            periini: $("#periinicio").val(),
            emprcodi: $('#hfComboEmpresa').val()
        },
        dataType: 'json',
        success: function (evt) {
            setCargosEmpresas(evt);
        },
        error: function (err) {
            console.log(err);
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

function setEmpresas(evt, option) {
    if (option == 1) {
        $("#emprcodi").html('');
        var html = "";
        for (var i = 0; i < evt.length; i++) {
            html += "<option value=" + evt[i].EmprCodi + ">" + evt[i].EmprNombre + "</option>";
        }
        $("#emprcodi").append(html);
        initPluginEmpresas(1);
    } else {
        $("#emprcodi2").html('');
        var html = "";
        for (var i = 0; i < evt.length; i++) {
            html += "<option value=" + evt[i].EmprCodi + ">" + evt[i].EmprNombre + "</option>";
        }
        $("#emprcodi2").append(html);
        initPluginEmpresas(2);
    }
    
}

function setEmpresasGeneradoras(evt, option) {
    if (option == 1) {
        $("#emprcodi").html('');
        var html = "<option value='0'>TODOS</option>";
        for (var i = 0; i < evt.length; i++) {
            html += "<option value=" + evt[i].Emprcodicargo + ">" + evt[i].Emprnombcargo + "</option>";
        }
        $("#emprcodi").append(html);
        initPluginEmpresas(1);
    } else {
        $("#emprcodi2").html('');
        var html = "<option value='0'>TODOS</option>";
        for (var i = 0; i < evt.length; i++) {
            html += "<option value=" + evt[i].Emprcodicargo + ">" + evt[i].Emprnombcargo + "</option>";
        }
        $("#emprcodi2").append(html);
        initPluginEmpresas(2);
    }
}

function setCargosEmpresas(evt) {
    $("#cargo").html('');
    var html = "";
    for (var i = 0; i < evt.length; i++) {
        html += "<option value=" + evt[i].Pingcodi + ">" + evt[i].Pingnombre + "</option>";
    }
    $("#cargo").append(html);
    initPluginCargo();
}

init = function () {
    var x = localStorage.getItem("tab");
    if (x != undefined) {
        if (x == 1) {
            $("#tab2").click();
        }
    }
}

change_tab = function (tab) {
    if (tab == 2) {
        localStorage.setItem('tab', 1);
    } else {
        localStorage.setItem('tab', 0);
    }
}

recargar = function () {
    var cmbPericodi = document.getElementById('periinicio1');
    var cmbPericodi2 = document.getElementById('perifin1');
    window.location.href = controler + "index?pericodi=" + cmbPericodi.value + "&pericodi2=" + cmbPericodi2.value;
}

descargarArchivo = function (formato) {
    var periini = $('#periinicio').val();
    var perifin = $('#perifin').val();
    var emprcodi = $('#hfComboEmpresa').val();
    var equicodi = $('#pegrtipoinformacion option:selected').val() == 0 ? $('#central').val() : $('#cargo').val();
    var pegrtipinfo = $("#pegrtipoinformacion").val();
    var recpotcodiConsulta = $("#recpotcodiConsulta").val();

    $.ajax({
        type: 'POST',
        url: controler + 'exportardata',
        data: { 
            periini: periini,
            recpotini: 0,
            perifin: perifin,
            recpotfin: 0,
            emprcodi: emprcodi,
            equicodi: equicodi,
            formato: formato,
            opt: 1,
            pegrtipinfo: pegrtipinfo,
            recpotcodiConsulta: recpotcodiConsulta,
            datos: $('#hfComboCargo').val() == 0 ? JSON.stringify([]) : JSON.stringify($('#hfComboCargo').val())
        },
        dataType: 'json',
        success: function (result) {
            if (result != -1) {
                window.location = controler + 'abrirarchivo?formato=' + formato + '&file=' + result;
                mostrarExito("Felicidades, el archivo se descargo correctamente...!");
            }
            else {
                mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
        }
    });
}

descargarComparacion = function (formato) {
    var periini = $('#periinicio1').val();
    var recpotini = $('#recpotcodi1').val();
    var perifin = $('#perifin1').val();
    var recpotfin = $('#recpotcodi2').val();
    var emprcodi = $('#hfComboEmpresa2').val();
    var pegrtipinfo = $("#pegrtipoinformacion2").val();

    $.ajax({
        type: 'POST',
        url: controler + 'exportardata',
        data: {
            periini: periini,
            recpotini: recpotini,
            perifin: perifin,
            recpotfin: recpotfin,
            emprcodi: emprcodi,
            equicodi: 0,
            formato: formato,
            opt: 2,
            pegrtipinfo: pegrtipinfo
        },
        dataType: 'json',
        success: function (result) {
            if (result != -1) {
                window.location = controler + 'abrirarchivo?formato=' + formato + '&file=' + result;
                mostrarExito("Felicidades, el archivo se descargo correctamente...!");
            }
            else {
                mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
        }
    });
}


function mostrarListado() {
    var periini = $('#periinicio').val();
    var perifin = $('#perifin').val();
    var emprcodi = $('#hfComboEmpresa').val();
    var equicodi = $('#pegrtipoinformacion option:selected').val() == 0 ? $('#central').val() : 0;
    equicodi = equicodi == null ? 0 : equicodi;
    var pegrtipinfo = $("#pegrtipoinformacion").val();
    var recpotcodiConsulta = $("#recpotcodiConsulta").val();

    $.ajax({
        type: 'POST',
        url: controler + "listaconsulta",
        data: {
            periini: periini,
            perifin: perifin,
            emprcodi: emprcodi,
            equicodi: equicodi,
            pegrtipinfo: pegrtipinfo,
            recpotcodiConsulta: recpotcodiConsulta,
            datos: $('#hfComboCargo').val() == 0 ? JSON.stringify([]) : JSON.stringify($('#hfComboCargo').val())
        },
        success: function (evt) {
            $('#listadoConsulta').html(evt);
        },
        error: function () {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

function mostrarComparacion() {
    var periini = $('#periinicio1').val();
    var recpotini = $('#recpotcodi1').val();
    var perifin = $('#perifin1').val();
    var recpotfin = $('#recpotcodi2').val();
    var emprcodi = $('#hfComboEmpresa2').val();
    var pegrtipinfo = $("#pegrtipoinformacion2").val();

    $.ajax({
        type: 'POST',
        url: controler + "listacomparacion",
        data: {
            periini: periini,
            recpotini: recpotini,
            perifin: perifin,
            recpotfin: recpotfin,
            emprcodi: emprcodi,
            pegrtipinfo: pegrtipinfo
        },
        success: function (evt) {
            $('#listadoComparacion').html(evt);
        },
        error: function () {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}