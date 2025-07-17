var controlador = siteRoot + 'Subastas/Oferta/';
/*var HEIGHT_MINIMO = 500;*/

$(function () {

    $('#tab-container').easytabs({
        animate: false
    });
    $('#tab-container').easytabs('select', '#tabSubir');

    $('#tab-container').bind('easytabs:after', function () {
        refrehDatatable();
    });

    $('#cbEmpresa').multipleSelect({
        width: '200px',
        filter: true,
    });
    $('#cbEmpresa').multipleSelect('checkAll');

    $('#cbUrs').multipleSelect({
        width: '200px',
        filter: true,
    });
    $('#cbUrs').multipleSelect('checkAll');

    $('#cbOferta').multipleSelect({
        width: '200px',
        filter: true,
    });
    $('#cbOferta').multipleSelect('checkAll');

    $('#FechaDesde').Zebra_DatePicker({
        format: "d/m/Y",
        pair: $('#FechaHasta'),
        onSelect: function (date) {
            $('#FechaHasta').val(date);
        }
        //direction: false,
    });

    $('#FechaHasta').Zebra_DatePicker({
        format: "d/m/Y",
        direction: true,
    });

    $('#btnConsultar').click(function () {
        mostrarListado();
    });

    $('#btnExpotar').click(function () {
        exportarExcel();
    });

    mostrarListado();

   
});

function mostrarListado() {

    var objData = getObjetoFiltro();
    var msj = validarConsulta(objData);

    if (msj == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'ListarOfertasDefectoActivadas',
            data: {
                fechaInicial: objData.FechaInicio,
                fechaFinal: objData.FechaFin,
                emprCodi: objData.Emprcodi,
                urs: objData.Urs,
                fuente: objData.Fuente
            },
            dataType: 'json',
            success: function (evt) {

                if (evt.Resultado != -1) {

                    if (!evt.FlagTieneDatos) // si no tiene datos
                        alert("No existen datos (subir y bajar) para los filtros consultados.");

                    var htmlLeyenda = _dibujarLeyenda(objData.Fuente);
                    $("#leyendaSubir").html(htmlLeyenda);
                    $("#leyendaBajar").html(htmlLeyenda);

                    $("#hst-subasta-ingreso-subir").html('');
                    $("#hst-subasta-ingreso-bajar").html('');

                    var htmlSubir = _dibujarTablaListado(evt.ListaOfertaSubir, "reporte-subir");
                    $("#hst-subasta-ingreso-subir").html(htmlSubir);
                    var htmlBajar = _dibujarTablaListado(evt.ListaOfertaBajar, "reporte-bajar");
                    $('#hst-subasta-ingreso-bajar').html(htmlBajar);

                    refrehDatatable();

                } else {
                    alert("Ha ocurrido un error: " + evt.Mensaje);
                }
            },
            error: function (err) {
                alert("Ha ocurrido un error");
            }
        });
    } else {
        alert(msj);
    }
}

function _dibujarLeyenda(fuente) {

    var lstFuente = fuente.split(',');
    var scolor = "";
    var nombre = "";

    var cadena = '';
    cadena += `
             <p>LEYENDA:</p>
             <table border="0" style="width: 567px;">
                 <tbody>`;

    for (var key in lstFuente) {
        var item = lstFuente[key];

        cadena += `
            <tr>
                `;

        scolor = item == "E" ? "white" : "#75A2FD";
        nombre = item == "E" ? "Mercado de ajuste" : "Activación de Oferta por defecto";

        cadena += `
                <td style="width: 50px; border: 1px solid black; background-color: ${scolor} !important; color: white;"></td>
                <td style="padding-right: 20px;">${nombre}</td>
                 `;
    }
    cadena += "</tbody></table>";

    return cadena;
}

function _dibujarTablaListado(lista, tabla) {

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono" cellspacing="0" width="100%" id="${tabla}">
        <thead>
            <tr>
                <th style='width: 70px'>Fecha de oferta </th>
                <th style='width: 70px'>URS</th>
                <th style='width: 100px'>HORA DE <br/> INICIO</th>
                <th style='width: 100px'>HORA DE <br/> FIN</th>
                <th style='width: 100px'>POTENCIA <br/> OFERTADA</th>
                <th style='width: 100px'>PRECIO <br/> (S/. / MW-Mes)</th>
                <th style='width: 100px'>EMPRESA</th>
                <th style='width: 100px'>MODO DE <br/> OPERACIÓN</th>
                <th style='width: 100px'>BANDA <br/> CALIFICADA</th>
                <th style='width: 100px'>CODIGO DE <br/> ENVÍO</th>
                <th style='width: 100px'>USUARIO</th>
                <th style='width: 100px'>FECHA DE <br/> ENVÍO</th>
            </tr>
        </thead>
        <tbody>
    `;

    var contador = 0;
    var final = 0;
    for (var key in lista) {
        var item = lista[key];

        var sStyle = "";
        var rowspan = "";
        if (item.Oferfuente == "A") sStyle = "background-color:#75A2FD;"; // azul
        if (item.Oferfuente == "E") sStyle = "background-color:#FFFFFF;"; // blanco

        if (item.Rowspan > 1 ) rowspan = item.Rowspan.toString();

        cadena += `
            <tr>
                `;

        if (item.Rowspan > 0) {
            var estilo = item.TineAgrupFuentes ? "" : sStyle;
            cadena += `                
                <td rowspan="${rowspan}" style="${estilo}">${item.OferfechainicioDesc}</td>
                <td rowspan="${rowspan}"style="${estilo}">${item.Ursnomb}</td>
                 `;
        }
        else {
            cadena += `                
                        <td style="display: none;"></td>
                        <td style="display: none;"></td>
                 `;
        }

        //agrupar hora
        var rowspan2 = "";
        if (item.Rowspan2 > 1)
        {
            rowspan2 = item.Rowspan2.toString();
            final = contador + item.Rowspan2 - 1;
        }

        if (item.Rowspan2 > 0) {
            var estilo2 = item.TineAgrupFuentes2 ? "" : sStyle;
            cadena += `
                <td rowspan="${rowspan2}" style="${estilo2}">${item.Ofdehorainicio}</td>
                <td rowspan="${rowspan2}"style="${estilo2}">${item.Ofdehorafin}</td>
                 `;
        }
        else {
            if (final > contador) {
                cadena += `                
                <td style="${sStyle}">${item.Ofdehorainicio}</td>
                <td style="${sStyle}">${item.Ofdehorafin}</td>
                 `;
            }
            else {
                cadena += `                
                        <td style="display: none;"></td>
                        <td style="display: none;"></td>
                 `;
            }
        }


        cadena += `
                <td style="${sStyle}">${item.RepopotoferDesc}</td>
                <td style="${sStyle}">${item.Repoprecio}</td>
                <td style="${sStyle}">${item.Emprnomb}</td>
                <td style="${sStyle}">${item.Gruponomb}</td>
                `;
        if (item.Rowspan > 0) {
            var estilo = item.TineAgrupFuentes ? "" : sStyle;
            cadena += `                
                <td rowspan="${rowspan}" style="${estilo}">${item.BandaCalificadaDesc}</td>
                 `;
        }
        else {
            cadena += `                
                        <td style="display: none;"></td>
                 `;
        }

        cadena += `
                <td style="${sStyle}">${item.Ofercodenvio}</td>
                <td style="${sStyle}">${item.Username}</td>
                <td style="${sStyle}">${item.OferfechaenvioDesc}</td>
            </tr>
        `;
        contador++;
    }
    cadena += "</tbody></table>";

    return cadena;
}

function getObjetoFiltro() {

    var fechaInicio = $('#FechaDesde').val();
    var fechaFin = $('#FechaHasta').val();

    var emprCodi = $('#cbEmpresa').multipleSelect('getSelects').join(',');
    var urs = $('#cbUrs').multipleSelect('getSelects').join(',');
    var fuente = $('#cbOferta').multipleSelect('getSelects').join(',');

    var obj = {
        FechaInicio: fechaInicio,
        FechaFin: fechaFin,

        Emprcodi: emprCodi,
        Urs: urs,
        Fuente: fuente,
    };

    return obj;
}

function validarConsulta(objFiltro) {
    var listaMsj = [];

    //Empresa
    if (objFiltro.Emprcodi == "")
        listaMsj.push("Empresa: Seleccione una opción.");

    //URS
    if (objFiltro.Urs == "")
        listaMsj.push("URS: Seleccione una opción.");
    //FUENTE
    if (objFiltro.Fuente == "")
        listaMsj.push("Oferta: Seleccione una opción.");


    // Valida consistencia del rango de fechas
    var fechaInicio = objFiltro.FechaInicio;
    var fechaFin = objFiltro.FechaFin;
    if (fechaInicio != "" && fechaFin != "") {
        if (CompararFechas(fechaInicio, fechaFin) == false) {
            listaMsj.push("La fecha de inicio no puede ser mayor que la fecha de fin.");
        }
    }

    var msj = listaMsj.join('\n');
    return msj;
}

function CompararFechas(fecha1, fecha2) {

    //Split de las fechas recibidas para separarlas
    var x = fecha1.split('/');
    var z = fecha2.split('/');

    //Cambiamos el orden al formato americano, de esto dd/mm/yyyy a esto mm/dd/yyyy
    fecha1 = x[1] + '/' + x[0] + '/' + x[2];
    fecha2 = z[1] + '/' + z[0] + '/' + z[2];

    //Comparamos las fechas
    if (Date.parse(fecha1) > Date.parse(fecha2)) {
        return false;
    } else {
        return true;
    }
}

function exportarExcel() {

    var objData = getObjetoFiltro();
    var msj = validarConsulta(objData);

    if (msj == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'GenerarExcelOfertasDefectoActivadas',
            data: {
                fechaInicial: objData.FechaInicio,
                fechaFinal: objData.FechaFin,
                emprCodi: objData.Emprcodi,
                urs: objData.Urs,
                fuente: objData.Fuente
            },
            dataType: 'json',
            success: function (evt) {
                if (evt.Resultado != -1) {

                    if (!evt.FlagTieneDatos) // si no tiene datos
                        alert("No existen datos (subir y bajar) para los filtros consultados.");

                    window.location = controlador + "AbrirArchivo?file=" + evt.NombreArchivo;
                }
                else {
                    alert('Ha ocurrido un error');
                }
            },
            error: function (err) {
                alert("Error en la exportación del reporte");
            }
        });
    } else {
        alert(msj);
    }

};

function refrehDatatable() {

    $('#reporte-subir').dataTable({
        "scrollY": "400px",
        "scrollX": true,
        "scrollCollapse": false,
        "sDom": 't',
        "ordering": false,
        paging: false,
        "bAutoWidth": false,
        "destroy": "true",
        "iDisplayLength": -1
    });

    $('#reporte-bajar').dataTable({
        "scrollY": "400px",
        "scrollX": true,
        "scrollCollapse": false,
        "sDom": 't',
        "ordering": false,
        paging: false,
        "bAutoWidth": false,
        "destroy": "true",
        "iDisplayLength": -1
    });
}