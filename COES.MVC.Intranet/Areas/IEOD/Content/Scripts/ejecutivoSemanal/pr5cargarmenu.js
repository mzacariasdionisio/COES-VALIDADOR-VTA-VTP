var controlador = siteRoot + 'IEOD/EjecutivoSemanal/';
var parametro1 = '';

$(function () {
    cargarMenuInfo(); //cargar menu flotante

    $('#openMenuPR5').click(function () {
        $('#contenedorMenuPR5').slideToggle("slow");
    });

    $('#closeMenuPR5').click(function () {
        $('#contenedorMenuPR5').css("display", "none");
    });

    //funciones
    $('#AnhoIni').Zebra_DatePicker({
        format: 'Y',
        onSelect: function () {
        }
    });
    $('#cboVersion').change(function () {
        mostrarReporteByFiltros();
    });
    $('#btnSearch').click(function () {
        mostrarReporteByFiltros();
    });

    $('#btnExportExcel').click(function () {
        exportarExcel();
    });

    $('#btnRegresar').click(function () {
        var fecha = $("#hdSemanaIni").val().replaceAll("/", '-');
        document.location.href = siteRoot + 'IEOD/EjecutivoSemanal/MenuEjecutivoSemanal?fecha=' + fecha;
    });

});

//menu
function fnClick(x) {
    var codigoVersion = getCodigoVersion();
    document.location.href = controlador + x + '?codigoVersion=' + codigoVersion;
}

function cargarMenuInfo() {
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarMenu',
        data: {},
        dataType: 'json',
        success: function (e) {
            $('#MenuID').html(e.Menu);
            $('#myTable').DataTable({
                "paging": false,
                "lengthChange": false,
                "pagingType": false,
                "ordering": false,
                "info": false
            });

            //sombrear el item seleccionado
            $(".item_pr5 a").css("font-weight", "normal");
            var codigoItem = $("#hdReporcodi").val();
            $("#repor_" + codigoItem + ".item_pr5 a").css("font-weight", "bold");

            $('#CodiMenu').val(1);
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function exportarExcel() {
    var codigoVersion = getCodigoVersion();
    var reporcodi = parseInt($("#hdReporcodi").val());

    $.ajax({
        type: 'POST',
        url: controlador + 'GenerarEjecutivoSemanal',
        data: {
            reporcodi: reporcodi,
            codigoVersion: codigoVersion
        },
        dataType: 'json',
        success: function (e) {

            switch (e.Total) {
                case 1: window.location = controlador + "ExportarReporteXls?nameFile=" + e.Resultado; break;// si hay elementos
                case -1: alert(e.Mensaje);
                    //alert(e.Resultado2);
                    break;// Error en C#
            }
        },
        error: function () {
            alert("Error en reporte");;
        }
    });
}

/**
 * Tabla Resumen
 * */
function verTablaResumenProd(sfechaInicial, sfechaFinal) {
    $("#div_tabla_registro").html('');
    $("#div_tabla_inter").html('');

    $("#hdTxtFechaIniResumen").val(sfechaInicial);
    $("#hdTxtFechaFinResumen").val(sfechaFinal);
    var filtroRER = $("#hdFlagRER").val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarDetalleTablaResumen',
        dataType: 'json',
        data: {
            sfechaInicial: sfechaInicial,
            sfechaFinal: sfechaFinal,
            filtroRER: filtroRER
        },
        cache: false,
        success: function (data) {
            if (data.Resultado != "-1") {
                $("#txtFechaIniResumen").html(sfechaInicial);
                $("#txtFechaFinResumen").html(sfechaFinal);

                var htmlExcel = dibujarTablaResumen(data.ListaDetalleProduccion);
                $("#div_tabla_registro").html(htmlExcel);

                //primero generar datatable
                setTimeout(function () {
                    $('#tbl_resumen').dataTable({
                        "destroy": "true",
                        "scrollX": true,
                        scrollY: 450,
                        "sDom": 'ft',
                        "ordering": false,
                        "bPaginate": false,
                        "iDisplayLength": -1,
                        "language": {
                            "emptyTable": "¡No existen hojas!"
                        },
                    });
                }, 150);

                //luego abrir popup
                setTimeout(function () {
                    $('#idPopupTablaResumen').bPopup({
                        easing: 'easeOutBack',
                        speed: 450,
                        transition: 'slideDown',
                        modalClose: false
                    });
                }, 50);

            } else {
                alert("Ha ocurrido un error: " + data.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function dibujarTablaResumen(lista) {

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono dataTable" cellspacing="0" width="100%" id="tbl_resumen">
        <thead>
            <tr>
                <th style='width: 100px'>Empresa</th>
                <th style='width: 100px'>Central</th>
                <th style='width: 100px'>Grupo <br> Despacho</th>
                <th style='width: 100px'>Tipo de  <br> Combustible</th>
                <th style='width: 100px'>E. Hidráulica  <br> MWh</th>
                <th style='width: 100px'>E. Térmica  <br> MWh</th>
                <th style='width: 100px'>E. Eolica  <br> MWh</th>
                <th style='width: 100px'>E. Solar  <br> MWh</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in lista) {
        var item = lista[key];

        cadena += `
            <tr>
                <td style="height: 24px;text-align: center; ">${item.Emprnomb}</td>
                <td style="height: 24px;text-align: center; ">${item.Central}</td>
                <td style="height: 24px;text-align: center; ">${item.Gruponomb}</td>
                <td style="height: 24px;text-align: center; ">${item.Fenergnomb}</td>
                <td style="height: 24px;text-align: right; ">${item.EnergiaH}</td>
                <td style="height: 24px;text-align: right; ">${item.EnergiaT}</td>
                <td style="height: 24px;text-align: right; ">${item.EnergiaE}</td>
                <td style="height: 24px;text-align: right; ">${item.EnergiaS}</td>
            </tr>
        `;
    }
    cadena += "</tbody></table>";

    return cadena;
}

function exportarTablaResumenProd() {
    var sfechaInicial = $("#hdTxtFechaIniResumen").val();
    var sfechaFinal = $("#hdTxtFechaFinResumen").val();
    var filtroRER = $("#hdFlagRER").val();

    $.ajax({
        type: 'POST',
        url: controlador + "GenerarArchivoExcelDetalleTablaResumen",
        data: {
            sfechaInicial: sfechaInicial,
            sfechaFinal: sfechaFinal,
            filtroRER: filtroRER
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                window.location = controlador + "ExportarReporteXls?nameFile=" + evt.Resultado;
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

/**
 * Tabla Interconexiones
 * */
function verTablaInterconexion(sfechaInicial, sfechaFinal) {
    $("#div_tabla_registro").html('');
    $("#div_tabla_inter").html('');

    $("#hdTxtFechaIniInter").val(sfechaInicial);
    $("#hdTxtFechaFinInter").val(sfechaFinal);

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarDetalleTablaInterconexion',
        dataType: 'json',
        data: {
            sfechaInicial: sfechaInicial,
            sfechaFinal: sfechaFinal,
        },
        cache: false,
        success: function (data) {
            if (data.Resultado != "-1") {
                $("#txtFechaIniInter").html(sfechaInicial);
                $("#txtFechaFinInter").html(sfechaFinal);

                var htmlExcel = dibujarTablaInterconexion(data.ListaDetalleInterconexion);
                $("#div_tabla_inter").html(htmlExcel);

                //primero generar datatable
                setTimeout(function () {
                    $('#tbl_resumen').dataTable({
                        "destroy": "true",
                        "scrollX": true,
                        scrollY: 450,
                        "sDom": 'ft',
                        "ordering": false,
                        "bPaginate": false,
                        "iDisplayLength": -1,
                        "language": {
                            "emptyTable": "¡No existen hojas!"
                        },
                    });
                }, 150);

                //luego abrir popup
                setTimeout(function () {
                    $('#idPopupTablaInterconexion').bPopup({
                        easing: 'easeOutBack',
                        speed: 450,
                        transition: 'slideDown',
                        modalClose: false
                    });
                }, 50);

            } else {
                alert("Ha ocurrido un error: " + data.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function dibujarTablaInterconexion(lista) {

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono dataTable" cellspacing="0" width="100%" id="tbl_resumen">
        <thead>
            <tr>
                <th rowspan='3'style='width: 100px'>Fecha Hora</th>
                <th style='width: 100px'>Exportación</th>
                <th style='width: 100px'>Importación</th>
                <th style='width: 100px'>Exportación</th>
                <th style='width: 100px'>Importación</th>
            </tr>
            <tr>
                <th style='width: 100px'>L-2280<br/> (Zorritos)</th>
                <th style='width: 100px'>L-2280<br/> (Zorritos)</th>
                <th style='width: 100px'>L-2280<br/> (Zorritos)</th>
                <th style='width: 100px'>L-2280<br/> (Zorritos)</th>
            </tr>
            <tr>
                <th style='width: 100px'>MWh</th>
                <th style='width: 100px'>MWh</th>
                <th style='width: 100px'>MVarh</th>
                <th style='width: 100px'>MVarh</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in lista) {
        var item = lista[key];

        cadena += `
            <tr>
                <td style="height: 24px;text-align: center; ">${item.FechaHoraDesc}</td>
                <td style="height: 24px;text-align: center; ">${item.EnergiaExp}</td>
                <td style="height: 24px;text-align: center; ">${item.EnergiaImp}</td>
                <td style="height: 24px;text-align: center; ">${item.ReactivaExp}</td>
                <td style="height: 24px;text-align: center; ">${item.ReactivaImp}</td>
            </tr>
        `;
    }
    cadena += "</tbody></table>";

    return cadena;
}

function exportarTablaInterconexion() {
    var sfechaInicial = $("#hdTxtFechaIniInter").val();
    var sfechaFinal = $("#hdTxtFechaFinInter").val();

    $.ajax({
        type: 'POST',
        url: controlador + "GenerarArchivoExcelDetalleInterconexion",
        data: {
            sfechaInicial: sfechaInicial,
            sfechaFinal: sfechaFinal,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                window.location = controlador + "ExportarReporteXls?nameFile=" + evt.Resultado;
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

/**
 * Tabla Resumen Maxima Demanda
 * */
function verTablaMaximaDemanda(sfechaInicial, sfechaFinal, sFechaMaximaDemanda) {
    $("#div_tabla_registro_md").html('');
    $("#div_tabla_inter_md").html('');

    $("#hdTxtFechaIniResumenMD").val(sfechaInicial);
    $("#hdTxtFechaFinResumenMD").val(sfechaFinal);
    $("#hdTxtFechaHoraMD").val(sFechaMaximaDemanda);

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarDetalleTablaResumenMD',
        dataType: 'json',
        data: {
            sfechaInicial: sfechaInicial,
            sfechaFinal: sfechaFinal,
            sFechaMaximaDemanda: sFechaMaximaDemanda
        },
        cache: false,
        success: function (data) {
            if (data.Resultado != "-1") {
                $("#txtFechaIniResumenMD").html(sfechaInicial);
                $("#txtFechaFinResumenMD").html(sfechaFinal);
                $("#txtFechaHoraResumenMD").html(sFechaMaximaDemanda);

                var htmlExcel = dibujarTablaResumenMD(data.ListaDetalleProduccion);
                $("#div_tabla_registro_md").html(htmlExcel);

                //primero generar datatable
                setTimeout(function () {
                    $('#tbl_resumen').dataTable({
                        "destroy": "true",
                        "scrollX": true,
                        scrollY: 450,
                        "sDom": 'ft',
                        "ordering": false,
                        "bPaginate": false,
                        "iDisplayLength": -1,
                        "language": {
                            "emptyTable": "¡No existen hojas!"
                        },
                    });
                }, 150);

                //luego abrir popup
                setTimeout(function () {
                    $('#idPopupTablaResumenMD').bPopup({
                        easing: 'easeOutBack',
                        speed: 450,
                        transition: 'slideDown',
                        modalClose: false
                    });
                }, 50);

            } else {
                alert("Ha ocurrido un error: " + data.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function dibujarTablaResumenMD(lista) {

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono dataTable" cellspacing="0" width="100%" id="tbl_resumen">
        <thead>
            <tr>
                <th style='width: 100px'>Empresa</th>
                <th style='width: 100px'>Central</th>
                <th style='width: 100px'>Grupo <br> Despacho</th>
                <th style='width: 100px'>Tipo de  <br> Combustible</th>
                <th style='width: 100px'>E. Hidráulica  <br> MWh</th>
                <th style='width: 100px'>E. Térmica  <br> MWh</th>
                <th style='width: 100px'>E. Eolica  <br> MWh</th>
                <th style='width: 100px'>E. Solar  <br> MWh</th>
                <th style='width: 100px'>MW</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in lista) {
        var item = lista[key];

        cadena += `
            <tr>
                <td style="height: 24px;text-align: center; ">${item.Emprnomb}</td>
                <td style="height: 24px;text-align: center; ">${item.Central}</td>
                <td style="height: 24px;text-align: center; ">${item.Gruponomb}</td>
                <td style="height: 24px;text-align: center; ">${item.Fenergnomb}</td>
                <td style="height: 24px;text-align: right; ">${item.EnergiaH}</td>
                <td style="height: 24px;text-align: right; ">${item.EnergiaT}</td>
                <td style="height: 24px;text-align: right; ">${item.EnergiaE}</td>
                <td style="height: 24px;text-align: right; ">${item.EnergiaS}</td>
                <td style="height: 24px;text-align: right; ">${item.PotenciaMD}</td>
            </tr>
        `;
    }
    cadena += "</tbody></table>";

    return cadena;
}

function exportarTablaResumenMD() {
    var sfechaInicial = $("#hdTxtFechaIniResumenMD").val();
    var sfechaFinal = $("#hdTxtFechaFinResumenMD").val();
    var sFechaMaximaDemanda = $("#hdTxtFechaHoraMD").val();

    $.ajax({
        type: 'POST',
        url: controlador + "GenerarArchivoExcelDetalleTablaResumenMD",
        data: {
            sfechaInicial: sfechaInicial,
            sfechaFinal: sfechaFinal,
            sFechaMaximaDemanda: sFechaMaximaDemanda
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                window.location = controlador + "ExportarReporteXls?nameFile=" + evt.Resultado;
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

/**
 * Tabla Interconexiones Maxima Demanda
 * */
function verTablaInterconexionMaximaDemanda(sfechaInicial, sfechaFinal, sFechaMaximaDemanda) {
    $("#div_tabla_registro_md").html('');
    $("#div_tabla_inter_md").html('');

    $("#hdTxtFechaIniInterMD").val(sfechaInicial);
    $("#hdTxtFechaFinInterMD").val(sfechaFinal);
    $("#hdTxtFechaHoraInterMD").val(sFechaMaximaDemanda);

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarDetalleTablaInterconexionMD',
        dataType: 'json',
        data: {
            sfechaInicial: sfechaInicial,
            sfechaFinal: sfechaFinal,
            sFechaMaximaDemanda: sFechaMaximaDemanda
        },
        cache: false,
        success: function (data) {
            if (data.Resultado != "-1") {
                $("#txtFechaIniInterMD").html(sfechaInicial);
                $("#txtFechaFinInterMD").html(sfechaFinal);
                $("#txtFechaHoraInterMD").html(sFechaMaximaDemanda);

                var htmlExcel = dibujarTablaInterconexionMD(data.ListaDetalleInterconexion);
                $("#div_tabla_inter_md").html(htmlExcel);

                //primero generar datatable
                setTimeout(function () {
                    $('#tbl_resumen').dataTable({
                        "destroy": "true",
                        "scrollX": true,
                        scrollY: 450,
                        "sDom": 'ft',
                        "ordering": false,
                        "bPaginate": false,
                        "iDisplayLength": -1,
                        "language": {
                            "emptyTable": "¡No existen hojas!"
                        },
                    });
                }, 150);

                //luego abrir popup
                setTimeout(function () {
                    $('#idPopupTablaInterconexionMD').bPopup({
                        easing: 'easeOutBack',
                        speed: 450,
                        transition: 'slideDown',
                        modalClose: false
                    });
                }, 50);

            } else {
                alert("Ha ocurrido un error: " + data.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function dibujarTablaInterconexionMD(lista) {

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono dataTable" cellspacing="0" width="100%" id="tbl_resumen">
        <thead>
            <tr>
                <th rowspan='3'style='width: 100px'>Fecha Hora</th>
                <th style='width: 100px'>Exportación</th>
                <th style='width: 100px'>Importación</th>
                <th style='width: 100px'>Exportación</th>
                <th style='width: 100px'>Importación</th>
            </tr>
            <tr>
                <th style='width: 100px'>L-2280<br/> (Zorritos)</th>
                <th style='width: 100px'>L-2280<br/> (Zorritos)</th>
                <th style='width: 100px'>L-2280<br/> (Zorritos)</th>
                <th style='width: 100px'>L-2280<br/> (Zorritos)</th>
            </tr>
            <tr>
                <th style='width: 100px'>MWh</th>
                <th style='width: 100px'>MWh</th>
                <th style='width: 100px'>MVarh</th>
                <th style='width: 100px'>MVarh</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in lista) {
        var item = lista[key];

        var textoColor = item.TieneMD ? "background-color: #93c4ff;" : "";

        cadena += `
            <tr>
                <td style="height: 24px;text-align: center; ${textoColor}">${item.FechaHoraDesc}</td>
                <td style="height: 24px;text-align: center; ${textoColor}">${item.EnergiaExp}</td>
                <td style="height: 24px;text-align: center; ${textoColor}">${item.EnergiaImp}</td>
                <td style="height: 24px;text-align: center; ${textoColor}">${item.ReactivaExp}</td>
                <td style="height: 24px;text-align: center; ${textoColor}">${item.ReactivaImp}</td>
            </tr>
        `;
    }
    cadena += "</tbody></table>";

    return cadena;
}

function exportarTablaInterconexionMD() {
    var sfechaInicial = $("#hdTxtFechaIniInterMD").val();
    var sfechaFinal = $("#hdTxtFechaFinInterMD").val();
    var sFechaMaximaDemanda = $("#hdTxtFechaHoraInterMD").val();

    $.ajax({
        type: 'POST',
        url: controlador + "GenerarArchivoExcelDetalleInterconexionMD",
        data: {
            sfechaInicial: sfechaInicial,
            sfechaFinal: sfechaFinal,
            sFechaMaximaDemanda: sFechaMaximaDemanda
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                window.location = controlador + "ExportarReporteXls?nameFile=" + evt.Resultado;
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}
