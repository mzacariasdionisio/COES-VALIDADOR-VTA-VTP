var controlador = siteRoot + "GMM/garantias/";

$(document).ready(function () {

    $('#tab-container').easytabs({
        animate: false
    });

    $('#btnRespaldar').click(function () {
        respaldar();
    });

    $('#btnCalcular').click(function () {
        calcular();
    });

    $('#btnPublicar').click(function () {
        publicar();
    });

    $('#mes').change(function () {
        ActualizarTabProcesamiento();
    });

    $('#rpt1').click(function () {
        exportarReporte('rpt1');
    });

    $('#rpt2').click(function () {
        exportarReporte('rpt2');
    });

    $('#rpt3').click(function () {
        exportarReporte('rpt3');
    });

    $('#rpt4').click(function () {
        exportarReporte('rpt4');
    });

    $('#rpt5').click(function () {
        exportarReporte('rpt5');
    });

    $('#rpt6').click(function () {
        exportarReporte('rpt6');
    });

    $('#rptInsumos').click(function () {
        exportarInsumos('rptInsumos');
    });

    setAnio("anho");
    setAnio("anhorpt");

    var vd = new Date();
    var vanho = vd.getFullYear();
   
    var vmes = vd.getMonth() +1;

    $('#anho').val(vanho);
    $('#mes').val(vmes);
    
    $('#anhorpt').val(vanho);
    $('#mesrpt').val(vmes);

    ActualizarTabProcesamiento();

});

function setAnio(aselect) {

    var d = new Date();
    var n = d.getFullYear();
    var select = document.getElementById(aselect);
    for (var i = 2019; i <= n + 1; i++) {
        var opc = document.createElement("option");
        opc.text = i;
        opc.value = i;
        select.add(opc);
    }
}

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

function ActualizarTabProcesamiento() {
    var panio = $('#anho').val();
    var pmes = $('#mes').val();
    
    $.ajax({
        type: 'POST',
        //url: controlador + 'MensajeProcesamiento',
        url: controlador + 'ListarGarantias',
        data: {
            anio: panio,
            mes: pmes
        },
        dataType: 'json',
        success: function (model) {

            $("#tbodyLista").empty();
            var total = model.listaGarantias.length;
            var data = model.listaGarantias;
            var html = "";
            for (var i = 0; i < total; i++) {
                html += "<tr>";
                
                html += "<td>" + data[i].EMPRESA + "</td>";
                html += "<td>" + data[i].RENERGIA + "</td>";
                html += "<td>" + data[i].RCAPACIDAD + "</td>";
                html += "<td>" + data[i].RPEAJE + "</td>";
                html += "<td>" + data[i].RSCOMPLE + "</td>";
                html += "<td>" + data[i].RINFLEXOP + "</td>";
                html += "<td>" + data[i].REREACTIVA + "</td>";
                html += "<td>" + data[i].TOTALGARANTIA + "</td>";
              
                html += "</tr>";
            }
            html = html.replace(/null/g, '-');
            $("#tbodyLista").append(html);

            //var jsonData = JSON.parse(data);
            //$('#EMPRESA').val(jsonData.EMPRESA);
            //$('#RENERGIA').val(jsonData.RENERGIA);
            //$('#RCAPACIDAD').val(jsonData.RCAPACIDAD);
            ////$('#RPEAJE').val(parseFloat(jsonData.RPEAJE));
            //$('#RPEAJE').val(jsonData.RPEAJE);
            //$('#RSCOMPLE').val(jsonData.RSCOMPLE);
            //$('#RINFLEXOP').val(jsonData.RINFLEXOP);
            //$('#REREACTIVA').val(jsonData.REREACTIVA);
            //$('#TOTALGARANTIA').val(jsonData.TOTALGARANTIA);
            //$('#Mensaje3').val(jsonData.Mensaje3);

            mostrarExito('Se copiaron los insumos para el procesamiento.');
        },
        error: function () {
            mostrarError('Ocurrió un problema al momento de actualizar la sección de procesamiento.');
        }
    });
}

function ActualizarTabProcesamientoCalculo() {
    var panio = $('#anho').val();
    var pmes = $('#mes').val();

    $.ajax({
        type: 'POST',
        //url: controlador + 'MensajeProcesamiento',
        url: controlador + 'ListarGarantias',
        data: {
            anio: panio,
            mes: pmes
        },
        dataType: 'json',
        success: function (model) {

            $("#tbodyLista").empty();
            var total = model.listaGarantias.length;
            var data = model.listaGarantias;
            var html = "";
            for (var i = 0; i < total; i++) {
                html += "<tr>";

                html += "<td>" + data[i].EMPRESA + "</td>";
                html += "<td>" + data[i].RENERGIA + "</td>";
                html += "<td>" + data[i].RCAPACIDAD + "</td>";
                html += "<td>" + data[i].RPEAJE + "</td>";
                html += "<td>" + data[i].RSCOMPLE + "</td>";
                html += "<td>" + data[i].RINFLEXOP + "</td>";
                html += "<td>" + data[i].REREACTIVA + "</td>";
                html += "<td>" + data[i].TOTALGARANTIA + "</td>";

                html += "</tr>";
            }
            html = html.replace(/null/g, '-');
            $("#tbodyLista").append(html);

            //var jsonData = JSON.parse(data);
            //$('#EMPRESA').val(jsonData.EMPRESA);
            //$('#RENERGIA').val(jsonData.RENERGIA);
            //$('#RCAPACIDAD').val(jsonData.RCAPACIDAD);
            ////$('#RPEAJE').val(parseFloat(jsonData.RPEAJE));
            //$('#RPEAJE').val(jsonData.RPEAJE);
            //$('#RSCOMPLE').val(jsonData.RSCOMPLE);
            //$('#RINFLEXOP').val(jsonData.RINFLEXOP);
            //$('#REREACTIVA').val(jsonData.REREACTIVA);
            //$('#TOTALGARANTIA').val(jsonData.TOTALGARANTIA); 
            //$('#Mensaje3').val(jsonData.Mensaje3);
            mostrarExito('El proceso de cálculo de garantías se ejecutó correctamente.');
        },
        error: function () {
            mostrarError('Ocurrió un problema al momento de actualizar la sección de procesamiento.');
        }
    });
}

function ActualizarTabProcesamientoPaso3() {
    var panio = $('#anho').val();
    var pmes = $('#mes').val();

    $.ajax({
        type: 'POST',
        //url: controlador + 'MensajeProcesamiento',
        url: controlador + 'ListarGarantias',
        data: {
            anio: panio,
            mes: pmes
        },
        dataType: 'json',
        success: function (model) {

            $("#tbodyLista").empty();
            var total = model.listaGarantias.length;
            var data = model.listaGarantias;
            var html = "";
            for (var i = 0; i < total; i++) {
                html += "<tr>";

                html += "<td>" + data[i].EMPRESA + "</td>";
                html += "<td>" + data[i].RENERGIA + "</td>";
                html += "<td>" + data[i].RCAPACIDAD + "</td>";
                html += "<td>" + data[i].RPEAJE + "</td>";
                html += "<td>" + data[i].RSCOMPLE + "</td>";
                html += "<td>" + data[i].RINFLEXOP + "</td>";
                html += "<td>" + data[i].REREACTIVA + "</td>";
                html += "<td>" + data[i].TOTALGARANTIA + "</td>";

                html += "</tr>";
            }
            html = html.replace(/null/g, '-');
            $("#tbodyLista").append(html);

            //var jsonData = JSON.parse(data);
            //$('#EMPRESA').val(jsonData.EMPRESA);
            //$('#RENERGIA').val(jsonData.RENERGIA);
            //$('#RCAPACIDAD').val(jsonData.RCAPACIDAD);
            ////$('#RPEAJE').val(parseFloat(jsonData.RPEAJE));
            //$('#RPEAJE').val(jsonData.RPEAJE);
            //$('#RSCOMPLE').val(jsonData.RSCOMPLE);
            //$('#RINFLEXOP').val(jsonData.RINFLEXOP);
            //$('#REREACTIVA').val(jsonData.REREACTIVA);
            //$('#TOTALGARANTIA').val(jsonData.TOTALGARANTIA);
            //$('#Mensaje3').val(jsonData.Mensaje3);
            mostrarExito('El estado del periodo se actualizó correctamente.');
        },
        error: function () {
            mostrarError('Ocurrió un problema al ejecutar el paso 3.');
        }
    });
}

respaldar = function () {

    // Validación
    var val1 = /^\d+\.\d{0,2}$/.test($('#tipoCambio').val());
    var val2 = /^\d+\.\d{0,2}$/.test($('#margenReserva').val());
    var val3 = /^\d+\.\d{0,2}$/.test($('#totalInflex').val());
    var val4 = /^\d+\.\d{0,2}$/.test($('#totalExceso').val());

    //if (!val1 || !val2 || !val3 || !val4)
    //{
    //    mostrarError('No se ejecutó el respaldo, revise los valores ingresados.');
    //    return;
    //}

    var panio = $('#anho').val();
    var pmes = $('#mes').val();
    $.ajax({
        type: 'POST',
        url: controlador + 'Respaldar',
        dataType: 'json',
        data: {
            anio: panio,
            mes: pmes
        },
        success: function (result) {
            if (result == "1") {
                ActualizarTabProcesamiento();
            }
            else if (result == "2") {
                mostrarError('El perido ha sido cerrado, para ejecutar el procesamiento debe volver a abrirlo.');
            }
            else if (result == "-1") {
                mostrarError('Ocurrió un problema al momento de ejecutar el respaldo.');
            }
            else {
                mostrarError(result); //alert(result);
            }
        },
        error: function () {
            alert("Lo sentimos, se ha producido un error");
        }
    });
}

calcular = function () {

    // Validación
    var val1 = /^\d+\.\d{0,2}$/.test($('#tipoCambio').val());
    var val2 = /^\d+\.\d{0,2}$/.test($('#margenReserva').val());
    var val3 = /^\d+\.\d{0,2}$/.test($('#totalInflex').val());
    var val4 = /^\d+\.\d{0,2}$/.test($('#totalExceso').val());

    //if (!val1 || !val2 || !val3 || !val4) {
    //    mostrarError('No se ejecutó el cálculo, revise los valores ingresados.');
    //    return;
    //}

    var panio = $('#anho').val();
    var pmes = $('#mes').val();
    $.ajax({
        type: 'POST',
        url: controlador + 'calcular',
        dataType: 'json',
        data: {
            anio: panio,
            mes: pmes
        },
        success: function (result) {
            if (result == "1") {
                ActualizarTabProcesamientoCalculo();
            }
            else if (result == "2") {
                mostrarError('El perido ha sido cerrado, para ejecutar el procesamiento debe volver a abrirlo.');
            }
            else if (result == "-1") {
                mostrarError('Ocurrió un problema al momento de ejecutar el respaldo.');
            }
            else {
                alert(result);
            }
        },
        error: function () {
            alert("Lo sentimos, se ha producido un error");
        }
    });
}


publicar = function () {

    var panio = $('#anho').val();
    var pmes = $('#mes').val();
    $.ajax({
        type: 'POST',
        url: controlador + 'Publicar',
        dataType: 'json',
        data: {
            anio: panio,
            mes: pmes
        },
        success: function (result) {
            if (result == "1") {
                ActualizarTabProcesamientoPaso3();
            }
            else if (result == "-1") {
                mostrarError('Ocurrió un problema al momento de ejecutar el cierre del periodo.');
            }
            else {
                alert(result);
            }
        },
        error: function () {
            alert("Lo sentimos, se ha producido un error");
        }
    });
}
exportarReporte = function (ptipo) {
    var panio = $('#anhorpt').val();
    var pmes = $('#mesrpt').val();
        $.ajax({
            type: 'POST',
            url: controlador + 'exportarReporte',
            dataType: 'json',
            data: { anio: panio, mes: pmes, tipo: ptipo},
            success: function (result) {
                if (result == "1") {
                    window.location = controlador + ptipo;
                }
                else if (result == "-1") {
                    mostrarError();
                }
                else {
                    alert(result);
                }
            },
            error: function () {
                alert("Lo sentimos, se ha producido un error");
            }
        });
}


exportarInsumos = function (ptipo) {
    var panio = $('#anho').val();
    var pmes = $('#mes').val();
    $.ajax({
        type: 'POST',
        url: controlador + 'exportarReporte',
        dataType: 'json',
        data: { anio: panio, mes: pmes, tipo: ptipo },
        success: function (result) {
            if (result == "1") {
                window.location = controlador + ptipo;
            }
            else if (result == "-1") {
                mostrarError();
            }
            else {
                alert(result);
            }
        },
        error: function () {
            alert("Lo sentimos, se ha producido un error");
        }
    });
}