var controlador = siteRoot + 'Combustibles/reporteGas/';

const CUADRO_1 = 1;
const CUADRO_2 = 2;
const CUADRO_3 = 3;
const CUADRO_4 = 4;

var dataC1, dataC2, dataC3, dataC4;
var dataNota;
var tblC1, tblC2, tblC3, tblNota;
var listaColoreadosC3;
var timeEspera = 2000;

$(function () {
    $('#tab-container').easytabs({
        animate: false
    });
    $('#tab-container').easytabs('select', '#tabC1');

    $('#mesVigencia').Zebra_DatePicker({
        format: "m-Y",        
    });

    //Muestra para el mes presente
    $("#hdPestania").val(CUADRO_1);
    mostrarReportesGuardados();

    $('#btnConsultar').click(function () {
        $('#tab-container').easytabs('select', '#tabC1');
        $("#hdPestania").val(CUADRO_1);
        mostrarReportesGuardados();
    });

    
    $('#tab-container').bind('easytabs:before', function (id, val, t) {
        if (t.selector == "#tabC1") {
            $("#hdPestania").val(CUADRO_1);
        }
        if (t.selector == "#tabC2") {
            $("#hdPestania").val(CUADRO_2);
        }
        if (t.selector == "#tabC3") {
            $("#hdPestania").val(CUADRO_3);
        }
        if (t.selector == "#tabCvc") {
            $("#hdPestania").val(CUADRO_4);
        }
    });

    /************* BOTONES CUADRO 1 *************/

    $('#btnProcesarC1').click(function () {
        reprocesarReporteC1();
    });

    $('#btnNotasC1').click(function () {
        mostrarReporteNota(CUADRO_1);
        $("#hdPestania").val(CUADRO_1);        
    });

    $('#btnEditarC1').click(function () {
        ponerAModoEdicion(CUADRO_1);
    });

    $('#btnMostrarErroresC1').click(function () {
        mostrarErrores(CUADRO_1);
    });

    $('#btnVerHistorialC1').click(function () {
        mostrarHistorial(CUADRO_1);
    });
    
    $('#btnGuardarC1').click(function () {
        guardarDataReporte(CUADRO_1, true);
    });


    /************* BOTONES CUADRO 2 *************/

    $('#btnProcesarC2').click(function () {
        reprocesarReporteC2();
    });

    $('#btnNotasC2').click(function () {
        mostrarReporteNota(CUADRO_2);
        $("#hdPestania").val(CUADRO_2);
    });

    $('#btnEditarC2').click(function () {
        ponerAModoEdicion(CUADRO_2);
    });

    $('#btnMostrarErroresC2').click(function () {
        mostrarErrores(CUADRO_2);
    });

    $('#btnVerHistorialC2').click(function () {
        mostrarHistorial(CUADRO_2);
    });

    $('#btnGuardarC2').click(function () {
        guardarDataReporte(CUADRO_2, true);
    });


    /************* BOTONES CUADRO 3 *************/

    $('#btnProcesarC3').click(function () {
        reprocesarReporteC3();
    });

    $('#btnNotasC3').click(function () {
        mostrarReporteNota(CUADRO_3);
        $("#hdPestania").val(CUADRO_3);
    });

    $('#btnEditarC3').click(function () {
        ponerAModoEdicion(CUADRO_3);
    });

    $('#btnMostrarErroresC3').click(function () {
        mostrarErrores(CUADRO_3);
    });

    $('#btnVerHistorialC3').click(function () {
        mostrarHistorial(CUADRO_3);
    });

    $('#btnGuardarC3').click(function () {
        guardarDataReporte(CUADRO_3, true);
    });


    /************* BOTONES CUADRO 4 *************/
    
    $('#btnEditarC4').click(function () {
        ponerAModoEdicion(CUADRO_4);
    });

    $('#btnVerHistorialC4').click(function () {
        mostrarHistorial(CUADRO_4);
    });

    $('#btnGuardarC4').click(function () {
        guardarDataReporte(CUADRO_4, true);
    });



    $('#btnAceptarNota').click(function () {
        mostrarPieNota();        
    });

    $('#btnExportarReportes').click(function () {
        exportarReporte();
    });
    
    $('#btnMostrarCarga').click(function () {
        mostrarCargarBD();
    });

    $('#btnCargarBD').click(function () {
        cargarABD();
    });

    $('#btnDescargarF3IS').click(function () {
        setTimeout(function () {
            $("#popupDescargarF3InfSust").bPopup({
                easing: 'easeOutBack',
                speed: 450,
                transition: 'slideDown',
                modalClose: false
            });
        }, 50);

        $('#cbEmpresa').multipleSelect({
            width: '250px',
            filter: true,
        });

        $('#cbEmpresa').multipleSelect('checkAll');

        $('#FechaDesde').Zebra_DatePicker({
            format: "m-Y",
            //pair: $('#FechaHasta')
            //direction: -1,
        });

        $('#FechaHasta').Zebra_DatePicker({
            format: "m-Y",
            //pair: $('#FechaDesde')
            //direction: -1,
        });

        setFechas();
    });

    $('#btnCancelarDescarga').click(function () {
        $('#popupDescargarF3InfSust').bPopup().close();
    });

    $('#btnDescargarF3InfSust').click(function () {
        var empresa = $('#cbEmpresa').multipleSelect('getSelects');
        if (empresa == "[object Object]") empresa = "-1";
        if (empresa == "") empresa = "-1";
        $('#hfEmpresaF3IS').val(empresa);

        if ($('#FechaHasta').val() >= $('#FechaDesde').val()) {
            DescargarF3InfSustXEmpresas();
        }
        else {
            alert("Fecha Hasta: debe ser mayor a la fecha inicio.");
        }
    });
});

function mostrarReportesGuardados() {
    limpiarBarraMensaje("mensaje");
    limpiarBarraMensaje("mensaje_C1");
    limpiarBarraMensaje("mensaje_C2");
    limpiarBarraMensaje("mensaje_C3");
    limpiarBarraMensaje("mensaje_C4");

    $("#div_listado_C1").html("");
    $("#div_listado_C2").html("");
    $("#div_listado_C3").html("");
    $("#div_listado_C4").html("");

    $("#nombReporteC1").css("display", "none");
    $("#Notas_C1").css("display", "none");

    $("#nombReporteC2").css("display", "none");
    $("#Notas_C2").css("display", "none");

    $("#nombReporteC3").css("display", "none");
    $("#Notas_C3").css("display", "none");

    $("#nombReporteC4").css("display", "none");

    var mesVigencia = $("#mesVigencia").val();

    $.ajax({
        type: 'POST',
        url: controlador + "ObtenerReportesGuardados",
        data: {
            mesVigencia: mesVigencia
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {

                //************* CUADRO 1 *************/
                if (evt.Data != null) { //tiene guardados                    
                    dataC1 = evt.Data;
                    $("#hdreportecodiC1").val(evt.ReporteCodiC1);
                    mostrarReporte(CUADRO_1, evt.NombreReporteC1, evt.NotaRC1, true, null);
                    btnEnModoLectura(CUADRO_1);
                    
                } else { // no tiene guardados
                    mostrarMensaje('mensaje_C1', 'alert', "No se encontró datos generados para el Cuadro 1 en el mes de vigencia ingresado. Procese información y Guarde.");
                    btnEnModoProcesar(CUADRO_1)
                }

                //************* CUADRO 2 *************/
                if (evt.Data2 != null) { //tiene guardados                    
                    dataC2 = evt.Data2;
                    $("#hdreportecodiC2").val(evt.ReporteCodiC2);
                    mostrarReporte(CUADRO_2, evt.NombreReporteC2, evt.NotaRC2, true, null);
                    btnEnModoLectura(CUADRO_2);

                } else { // no tiene guardados
                    mostrarMensaje('mensaje_C2', 'alert', "No se encontró datos generados para el Cuadro 2 en el mes de vigencia ingresado. Procese información y Guarde.");
                    btnEnModoProcesar(CUADRO_2)
                }

                ///************* CUADRO 3 *************/
                if (evt.Data3 != null) { //tiene guardados                    
                    dataC3 = evt.Data3;
                    $("#hdreportecodiC3").val(evt.ReporteCodiC3);
                    mostrarReporte(CUADRO_3, evt.NombreReporteC3, evt.NotaRC3, true, evt.ListaFilasPintar);
                    btnEnModoLectura(CUADRO_3);
                    $("#hdlistaColoreados").val(evt.ListaFilasPintar);                    

                } else { // no tiene guardados
                    mostrarMensaje('mensaje_C3', 'alert', "No se encontró datos generados para el Cuadro 3 en el mes de vigencia ingresado. Procese información y Guarde.");
                    btnEnModoProcesar(CUADRO_3)
                }

                ///************* CUADRO 4 *************/                  
                    
                $("#hdreportecodiC4").val(evt.ReporteCodiC4);
                mostrarReporteC4(evt.NombreReporteC4, evt.MesAnio);
                btnEnModoLecturaC4(CUADRO_4);

                

            } else {
                mostrarMensaje('mensaje', 'error', evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function mostrarReporteC4(nombreReporteC4, mesAnio) {
    var cadena = '';

    cadena += `
             
             <div style=" padding-top: 8px; padding-bottom: 12px; font-size: 14px; font-weight: bold;" > ${mesAnio}  </div>
             <div style=" padding-top: 8px; padding-bottom: 8px;" > Disponible en el siguiente enlace del portal web COES.</div>
             <div style=" padding-top: 8px; padding-bottom: 12px;" >
                <a href="https://www.coes.org.pe/Portal/Operacion/CaractSEIN/CostoVariables/" target="_blank">https://www.coes.org.pe/Portal/Operacion/CaractSEIN/CostoVariables</a>
             </div>
    `;

    $("#div_listado_C4").html(cadena);

    $("#txtNombreReporteC4").val(nombreReporteC4);

    $("#nombReporteC4").css("display", "block");

    document.getElementById('txtNombreReporteC4').readOnly = true;
}


function mostrarReporte(tipoReporte, nombre, notas, modoLectura, listaFilasAPintar) {
    document.getElementById('txtNombreReporteC' + tipoReporte).readOnly = modoLectura;

    var notaArrBr = notas.replace(/@/gi, '<br>');


    //****************** cuadro X ********************/
    //seteamos valores de nombre y notas
    $("#txtNombreReporteC" + tipoReporte).val(nombre);
    const regex = /\n/g;
    var nuevoHtml = notaArrBr.replace(regex, '<br>');
    $("#Notas_C" + tipoReporte).html(nuevoHtml);
    
    $("#hddataNotaC" + tipoReporte).val(notas);
    
    //mostramos  nombre y notas
    $("#nombReporteC" + tipoReporte).css("display", "block");
    $("#Notas_C" + tipoReporte).css("display", "block");

    switch (tipoReporte) {
        case CUADRO_1: mostrarHandsonC1(modoLectura); break;
        case CUADRO_2: mostrarHandsonC2(modoLectura); break;
        case CUADRO_3: mostrarHandsonC3(modoLectura, listaFilasAPintar); break;
    }
    
}

function mostrarHandsonC1(esSoloLectura) {    
    if (typeof tblC1 != 'undefined' && tblC1 != null) {
        tblC1.destroy();
    }
    // #region Handsontable SEMANA/MES
    var containerC1 = document.getElementById('div_listado_C1');

    tblC1 = new Handsontable(containerC1, {
        data: getData(CUADRO_1),
        mergeCells: celdasMerge(CUADRO_1),
        cell: propiedadCeldas(CUADRO_1, null),
        colWidths: [350, 120, 120, 120],
        contextMenu: esSoloLectura ? false : contexMenuConfigC1, //sirve para cuando muestra reprocesos
        //contextMenu: false,
        columnSorting: false, // no puede ordenarse las columnas
        minSpareRows: 0,
        showWeekNumber: false,
        rowHeaders: false, //quita numeración
        colHeaders: false,
        autoWrapRow: true,
        startRows: 0,
        //width: 500, //ancho de la tabla
        //height: 350, //alto de la tabla  //para que las notas esten cerca
        manualColumnResize: true,
        readOnly: esSoloLectura,
        fillHandle: { //Evita agregar mas filas de las deseadas
            direction: 'vertical',
            autoInsertRow: false
        },        
        hiddenColumns: {
            columns: [4]
        }
    });
    tblC1.updateSettings({

        beforeKeyDown(e) {
            var ffil = this.getSelectedRange().from.row;
            var ccol = this.getSelectedRange().from.col;

            if (ffil >= 3 && ccol >= 1) {
                const selection = tblC1.getSelected()[0];
                //si se presiona ciertos caracteres
                if (e.keyCode === 188) { //char: ','
                    //no los muestra
                    e.stopImmediatePropagation();
                    e.preventDefault();
                }
            }
        }
    });
}

function mostrarHandsonC2(esSoloLectura) {
    if (typeof tblC2 != 'undefined' && tblC2 != null) {
        tblC2.destroy();
    }
    // #region Handsontable SEMANA/MES
    var containerC2 = document.getElementById('div_listado_C2');

    tblC2 = new Handsontable(containerC2, {
        data: getData(CUADRO_2),
        mergeCells: celdasMerge(CUADRO_2),
        cell: propiedadCeldas(CUADRO_2, null),
        colWidths: [350, 120, 120, 120, 160],
        contextMenu: esSoloLectura? false: contexMenuConfigC2, //sirve para cuando muestra reprocesos
        //contextMenu: false,
        columnSorting: false, // no puede ordenarse las columnas
        minSpareRows: 0,
        showWeekNumber: false,
        rowHeaders: false, //quita numeración
        colHeaders: false,
        autoWrapRow: true,
        startRows: 0,
        //width: 500, //ancho de la tabla
        //height: 350, //alto de la tabla  //para que las notas esten cerca
        manualColumnResize: true,
        readOnly: esSoloLectura,
        fillHandle: { //Evita agregar mas filas de las deseadas
            direction: 'vertical',
            autoInsertRow: false
        },
        hiddenColumns: {
            columns: [5]
        }
    });
    tblC2.updateSettings({

        beforeKeyDown(e) {            
            var ffil = this.getSelectedRange().from.row;
            var ccol = this.getSelectedRange().from.col;

            if (ffil >= 3 && ccol >= 1) {
                const selection = tblC2.getSelected()[0];
                //si se presiona ciertos caracteres
                if (e.keyCode === 188) { //char: ','
                    //no los muestra
                    e.stopImmediatePropagation();                    
                    e.preventDefault();
                }
            }
        }       
    });

}

function mostrarHandsonC3(esSoloLectura, listaFilasAPintar) {
    if (typeof tblC3 != 'undefined' && tblC3 != null) {
        tblC3.destroy();
    }
    // #region Handsontable
    var containerC3 = document.getElementById('div_listado_C3');

    tblC3 = new Handsontable(containerC3, {
        data: getData(CUADRO_3),
        mergeCells: celdasMerge(CUADRO_3),
        cell: propiedadCeldas(CUADRO_3, listaFilasAPintar),
        colWidths: [350, 120, 120, 120, 120],
        contextMenu: esSoloLectura ? false : contexMenuConfigC3, //sirve para cuando muestra reprocesos
        //contextMenu: false,
        columnSorting: false, // no puede ordenarse las columnas
        minSpareRows: 0,
        showWeekNumber: false,
        rowHeaders: false, //quita numeración
        colHeaders: false,
        autoWrapRow: true,
        startRows: 0,
        //width: 500, //ancho de la tabla
        //height: 350, //alto de la tabla  //para que las notas esten cerca
        manualColumnResize: true,
        readOnly: esSoloLectura,
        fillHandle: { //Evita agregar mas filas de las deseadas
            direction: 'vertical',
            autoInsertRow: false
        },
        hiddenColumns: {
          columns: [5,6]
        }
    });
    tblC3.updateSettings({

        beforeKeyDown(e) {
            var ffil = this.getSelectedRange().from.row;
            var ccol = this.getSelectedRange().from.col;

            if (ffil >= 3 && ccol >= 1) {
                const selection = tblC3.getSelected()[0];
                //si se presiona ciertos caracteres
                if (e.keyCode === 188) { //char: ','
                    //no los muestra
                    e.stopImmediatePropagation();
                    e.preventDefault();
                }
            }
        }
    });
}

function getData(cuadro) {
    var miData;

    switch (cuadro) {
        case CUADRO_1: miData = dataC1; break;
        case CUADRO_2: miData = dataC2; break;
        case CUADRO_3: miData = dataC3; break;
        
    }

    return miData;
}

function celdasMerge(cuadro) {
    var miMerge;

    switch (cuadro) {
        case CUADRO_1:
            miMerge = [
                { row: 0, col: 0, rowspan: 3, colspan: 1 },
                { row: 0, col: 1, rowspan: 1, colspan: 3 }
            ];
            break;

        case CUADRO_2:
            miMerge = [
                { row: 0, col: 0, rowspan: 3, colspan: 1 },
                { row: 0, col: 1, rowspan: 1, colspan: 3 }
            ];
            break;

        case CUADRO_3:
            miMerge = [
                { row: 0, col: 0, rowspan: 3, colspan: 1 },
                { row: 0, col: 1, rowspan: 1, colspan: 3 },
                { row: 0, col: 4, rowspan: 2, colspan: 1 }
            ];
            break;
    }    

    return miMerge;
}

function propiedadCeldas(cuadro, strListaFilasAPintar) {
    var propiedad;

    if (cuadro == CUADRO_1) {
        propiedad =
            [
                { row: 0, col: 0, className: 'htCenter htMiddle htColorCabecera' },
                { row: 0, col: 1, className: 'htCenter htMiddle htColorCabecera' },
                { row: 1, col: 1, className: 'htCenter htMiddle htColorCabecera' },
                { row: 1, col: 2, className: 'htCenter htMiddle htColorCabecera' },
                { row: 1, col: 3, className: 'htCenter htMiddle htColorCabecera' },
                { row: 2, col: 1, className: 'htCenter htMiddle htColorCabecera' },
                { row: 2, col: 2, className: 'htCenter htMiddle htColorCabecera' },
                { row: 2, col: 3, className: 'htCenter htMiddle htColorCabecera' }
            ]
    }

    if (cuadro == CUADRO_2) {
        propiedad =
            [
                { row: 0, col: 0, className: 'htCenter htMiddle htColorCabecera' },
                { row: 0, col: 1, className: 'htCenter htMiddle htColorCabecera' },
                { row: 0, col: 4, className: 'htCenter htMiddle htColorCabecera' },
                { row: 1, col: 1, className: 'htCenter htMiddle htColorCabecera' },
                { row: 1, col: 2, className: 'htCenter htMiddle htColorCabecera' },
                { row: 1, col: 3, className: 'htCenter htMiddle htColorCabecera' },
                { row: 1, col: 4, className: 'htCenter htMiddle htColorCabecera' },
                { row: 2, col: 1, className: 'htCenter htMiddle htColorCabecera' },
                { row: 2, col: 2, className: 'htCenter htMiddle htColorCabecera' },
                { row: 2, col: 3, className: 'htCenter htMiddle htColorCabecera' },
                { row: 2, col: 4, className: 'htCenter htMiddle htColorCabecera' }
            ]
    }

    if (cuadro == CUADRO_3) {

        var lstFilas = [];
        var numFilas = 0;
        if (strListaFilasAPintar != "") {
            lstFilas = strListaFilasAPintar.split(",");
            numFilas = lstFilas.length;
        }

        propiedad =
            [
                { row: 0, col: 0, className: 'htCenter htMiddle htColorCabecera' },
                { row: 0, col: 1, className: 'htCenter htMiddle htColorCabecera' },
                { row: 0, col: 4, className: 'htCenter htMiddle htColorCabecera' },
                { row: 1, col: 1, className: 'htCenter htMiddle htColorCabecera' },
                { row: 1, col: 2, className: 'htCenter htMiddle htColorCabecera' },
                { row: 1, col: 3, className: 'htCenter htMiddle htColorCabecera' },
                //{ row: 1, col: 4, className: 'htCenter htMiddle htColorCabecera' },
                { row: 2, col: 1, className: 'htCenter htMiddle htColorCabecera' },
                { row: 2, col: 2, className: 'htCenter htMiddle htColorCabecera' },
                { row: 2, col: 3, className: 'htCenter htMiddle htColorCabecera' },
                { row: 2, col: 4, className: 'htCenter htMiddle htColorCabecera' },
            ];

        for (var i = 0; i < numFilas; i++) {
            var fila = lstFilas[i];
            var dato = {
                "row": fila,
                "col": 0,
                "className": 'htColorCelda'
            };
            propiedad.push(dato);

            var dato = {
                "row": fila,
                "col": 1,
                "className": 'htColorCelda'
            };
            propiedad.push(dato);

            var dato = {
                "row": fila,
                "col": 2,
                "className": 'htColorCelda'
            };
            propiedad.push(dato);

            var dato = {
                "row": fila,
                "col": 3,
                "className": 'htColorCelda'
            };
            propiedad.push(dato);

            var dato = {
                "row": fila,
                "col": 4,
                "className": 'htColorCelda'
            };
            propiedad.push(dato);
        }

    }
    return propiedad;
}


var contexMenuConfigC1 = {
    items: {               
        'remove_row': {
            name: "Eliminar fila",
            hidden: function () {
                // if first row, disable this option
                return tblC1.getSelectedRange().to.row <= 2;
            }
        },
    }   
};

var contexMenuConfigC2 = {
    items: {        
        'remove_row': {
            name: "Eliminar fila",
            hidden: function () {
                // if first row, disable this option
                return tblC2.getSelectedRange().to.row <= 2;
            }
        },
    }
};

var contexMenuConfigC3 = {
    items: {        
        'remove_row': {
            name: "Eliminar fila",
            hidden: function () {
                // if first row, disable this option
                return tblC3.getSelectedRange().to.row <= 2;
            }
        },
    }
};


function reprocesarReporteC1() {
    //Limpiar informacion
    limpiarBarraMensaje("mensaje");
    limpiarBarraMensaje("mensaje_C1");
    $("#div_listado_C1").html("");
    $("#nombReporteC1").css("display", "none");
    $("#Notas_C1").css("display", "none");
        
    var mesVigencia = $("#mesVigencia").val();

    $.ajax({
        type: 'POST',
        url: controlador + "procesarReporteC1",
        data: {
            mesVigencia: mesVigencia
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                if (evt.Resultado == "1") { //si hay centrales a procesar
                    dataC1 = evt.Data;
                    $("#hdreportecodiC1").val(0);
                    mostrarReporte(CUADRO_1, evt.NombreReporteC1, evt.NotaRC1, false, null);
                    btnEnModoEdicion(CUADRO_1);

                    //si es nuevo guardo primera version
                    if (evt.ExisteVersion1 == 0) {
                        setTimeout(function () {
                            guardarDataReporte(CUADRO_1, false);
                        }, timeEspera);
                    }

                } else { //si no hay centrales a procesar
                    btnEnModoProcesar(CUADRO_1);
                    mostrarMensaje('mensaje', 'error', "No existe centrales aprobadas o parcialmente aprobadas para dicho mes de  vigencia.");
                }

            } else {
                mostrarMensaje('mensaje', 'error', evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function reprocesarReporteC2() {
    //Limpiar informacion
    limpiarBarraMensaje("mensaje");
    limpiarBarraMensaje("mensaje_C2");
    $("#div_listado_C2").html("");
    $("#nombReporteC2").css("display", "none");
    $("#Notas_C2").css("display", "none");

    var mesVigencia = $("#mesVigencia").val();

    $.ajax({
        type: 'POST',
        url: controlador + "procesarReporteC2",
        data: {
            mesVigencia: mesVigencia
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                if (evt.Resultado == "1") { //si hay centrales a procesar
                    dataC2 = evt.Data2;
                    $("#hdreportecodiC2").val(0);
                    mostrarReporte(CUADRO_2, evt.NombreReporteC2, evt.NotaRC2, false, null);
                    btnEnModoEdicion(CUADRO_2);

                    //si es nuevo guardo primera version
                    if (evt.ExisteVersion1 == 0) {
                        setTimeout(function () {
                            guardarDataReporte(CUADRO_2, false);
                        }, timeEspera);
                    }
                } else { //si no hay centrales a procesar
                    btnEnModoProcesar(CUADRO_2);
                    mostrarMensaje('mensaje', 'error', "No existe centrales aprobadas o parcialmente aprobadas para dicho mes de  vigencia.");
                }

            } else {
                mostrarMensaje('mensaje', 'error', evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function reprocesarReporteC3() {
    //Limpiar informacion
    limpiarBarraMensaje("mensaje");
    limpiarBarraMensaje("mensaje_C3");
    $("#div_listado_C3").html("");
    $("#nombReporteC3").css("display", "none");
    $("#Notas_C3").css("display", "none");

    var mesVigencia = $("#mesVigencia").val();

    $.ajax({
        type: 'POST',
        url: controlador + "procesarReporteC3",
        data: {
            mesVigencia: mesVigencia
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                if (evt.Resultado == "1") { //si hay centrales a procesar
                    dataC3 = evt.Data3;
                    $("#hdreportecodiC3").val(0);
                    mostrarReporte(CUADRO_3, evt.NombreReporteC3, evt.NotaRC3, false, evt.ListaFilasPintar);
                    btnEnModoEdicion(CUADRO_3);
                    $("#hdlistaColoreados").val(evt.ListaFilasPintar);

                    //si es nuevo guardo primera version
                    if (evt.ExisteVersion1 == 0) {
                        setTimeout(function () {
                            guardarDataReporte(CUADRO_3, false);
                        }, timeEspera);
                    }

                } else { //si no hay centrales a procesar
                    btnEnModoProcesar(CUADRO_3);
                    mostrarMensaje('mensaje', 'error', "No existe centrales aprobadas o parcialmente aprobadas para dicho mes de  vigencia.");
                }

            } else {
                mostrarMensaje('mensaje', 'error', evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function mostrarReporteNota(tipoCuadro) {
    limpiarBarraMensaje("mensaje_popupNota");
    $("#div_nota").html("");

    var lstNota = [][2];

    switch (tipoCuadro) {
        case CUADRO_1: lstNota = $("#hddataNotaC1").val(); break;
        case CUADRO_2: lstNota = $("#hddataNotaC2").val(); break;
        case CUADRO_3: lstNota = $("#hddataNotaC3").val(); break;
    }

    dataNota = obtenerArray(lstNota);
    mostrarNota();
    abrirPopup("popupNota");

    setTimeout(function () {
        document.querySelector('#seccionClick').click();
    }, 1000);
    
}

function obtenerArray(lstNotas) {

    var lst = lstNotas.split("@");
    var num = lst.length;

    var salida = new Array(num);
    for (var i = 0; i < num; i++) {
        salida[i] = new Array(1);
        salida[i][0] = lst[i];
    }

    return salida;
}

function mostrarNota() {

    if (typeof tblNota != 'undefined' && tblNota != null) {
        tblNota.destroy();
    }

    var containerNota = document.getElementById('div_nota');

    tblNota = new Handsontable(containerNota, {
        data: dataNota,
        //mergeCells: celdasMerge(CUADRO_1),
        //cell: propiedadCeldas(CUADRO_1),
        colWidths: [540],
        contextMenu: contexMenuNota,
        columnSorting: false, // no puede ordenarse las columnas
        minSpareRows: 0,
        showWeekNumber: false,
        rowHeaders: false, //quita numeración
        colHeaders: false,
        autoWrapRow: true,
        startRows: 0,
        //width: 500, //ancho de la tabla
        height: 250, //alto de la tabla
        manualColumnResize: true,
        //readOnly: true
        fillHandle: { //Evita agregar mas filas de las deseadas
            direction: 'vertical',
            autoInsertRow: false
        }
    });


}

var contexMenuNota = {
    items: {
        'row_above': { name: "Insertar fila arriba" },
        'row_below': {  name: "Insertar fila abajo" },        
        'remove_row': { name: "Eliminar fila" },
    }
};

function mostrarPieNota() {
    cerrarPopup('popupNota');

    var idPie = "";
    var tablaNota = tblNota;
    var cuadroPestania = $("#hdPestania").val();
    var dataNota = tablaNota.getData();

    var htmlNota = "";
    var numNotas = dataNota.length;
    for (var i = 0; i < numNotas; i++) {
        htmlNota += dataNota[i] + "<br>"
    }

    let strLstDataNota = dataNota.join("@");
    if (cuadroPestania == CUADRO_1) {
        idPie = "Notas_C1";
        $("#hddataNotaC1").val(strLstDataNota);
    }
    if (cuadroPestania == CUADRO_2) {
        idPie = "Notas_C2";
        $("#hddataNotaC2").val(strLstDataNota);
    }
    if (cuadroPestania == CUADRO_3) {
        idPie = "Notas_C3";
        $("#hddataNotaC3").val(strLstDataNota);
    }

    const regex = /\n/g;
    var nuevoHtml = htmlNota.replace(regex, '<br>');

    $("#" + idPie).html(nuevoHtml);
}


function guardarDataReporte(tipoReporte, debeValidar) {

    limpiarBarraMensaje("mensaje");
    limpiarBarraMensaje("mensaje_C1");
    limpiarBarraMensaje("mensaje_C2");
    limpiarBarraMensaje("mensaje_C3");
    limpiarBarraMensaje("mensaje_C4");
    
    var mesVigencia = $("#mesVigencia").val();
    var notaData = "";

    var dataNota, dataHandson, nombreReporte;
    switch (tipoReporte) {
        case CUADRO_1:
            dataHandson = tblC1.getData();
            dataNota = $("#hddataNotaC1").val();
            nombreReporte = $("#txtNombreReporteC1").val();
            break;
        case CUADRO_2:
            dataHandson = tblC2.getData();
            dataNota = $("#hddataNotaC2").val();
            nombreReporte = $("#txtNombreReporteC2").val();
            break;
        case CUADRO_3:
            dataHandson = tblC3.getData();
            dataNota = $("#hddataNotaC3").val();
            nombreReporte = $("#txtNombreReporteC3").val();
            break;
        case CUADRO_4:
            var matriz = [2];
            dataHandson = matriz; //envio cualquiera, no es usado
            dataNota = $("#hddataNotaC4").val();
            nombreReporte = $("#txtNombreReporteC4").val();
            break;
    }

    var listaErrores = [];

    //para la version 1 no valido
    if (debeValidar) { 
        //verifico si existen errores 
        var listaErrores = obtenerErroresDeHandson(dataHandson, tipoReporte);
    } 
    if (listaErrores.length == 0) {

        if (debeValidar) {
            if (confirm('¿Desea guardar la información?')) {
                guardarReporte(dataHandson, dataNota, nombreReporte, tipoReporte, mesVigencia, 1);
            }
        } else {
            guardarReporte(dataHandson, dataNota, nombreReporte, tipoReporte, mesVigencia, 2);
        }
        
    } else {
        mostrarErrores(tipoReporte);
    }
}

function guardarReporte(dataHandson, dataNota, nombreReporte, tipoReporte, mesVigencia, tipoGuardado) {
    $.ajax({
        type: "POST",
        url: controlador + 'guardarReporte',
        dataType: "json",
        contentType: 'application/json',
        traditional: true,
        data: JSON.stringify({
            datos: dataHandson,
            notas: dataNota,
            nombReporte: nombreReporte,
            tipoReporte: tipoReporte,
            strMesVigencia: mesVigencia
        }),

        success: function (evt) {
            if (evt.Resultado != "-1") {
                verReporteCVPorVersion(evt.IdReporte);
                if (tipoGuardado == 1)
                    mostrarMensaje('mensaje', 'exito', "Reporte guardado con éxito.");
                if (tipoGuardado == 2)
                    mostrarMensaje('mensaje', 'exito', "Se cargó de base de datos y se guardó la primera versión.");
                ponerAModoLectura(tipoReporte);
                
            } else {
                mostrarMensaje('mensaje', 'error', evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}


function ponerAModoEdicion(tipoReporte) {
    switch (tipoReporte) {
        case CUADRO_1:
            btnEnModoEdicion(tipoReporte);

            document.getElementById('txtNombreReporteC1').readOnly = false;

            tblC1.updateSettings({
                cells: function (row, col, prop) {
                    var cellProperties = {};
                    cellProperties.readOnly = false;

                    if (row == 0 && col == 0) cellProperties.className = 'htCenter htMiddle htColorCabecera';
                    if (row == 0 && col == 1) cellProperties.className = 'htCenter htMiddle htColorCabecera';
                    if (row == 1 && col == 1) cellProperties.className = 'htCenter htMiddle htColorCabecera';
                    if (row == 1 && col == 2) cellProperties.className = 'htCenter htMiddle htColorCabecera';
                    if (row == 1 && col == 3) cellProperties.className = 'htCenter htMiddle htColorCabecera';
                    if (row == 2 && col == 1) cellProperties.className = 'htCenter htMiddle htColorCabecera';
                    if (row == 2 && col == 2) cellProperties.className = 'htCenter htMiddle htColorCabecera';
                    if (row == 2 && col == 3) cellProperties.className = 'htCenter htMiddle htColorCabecera';
    
                    return cellProperties;
                },
                contextMenu: contexMenuConfigC1,
            })
            
            break;

        case CUADRO_2:
            btnEnModoEdicion(tipoReporte);

            document.getElementById('txtNombreReporteC2').readOnly = false;

            tblC2.updateSettings({
                cells: function (row, col, prop) {
                    var cellProperties = {};
                    cellProperties.readOnly = false;

                    if (row == 0 && col == 0) cellProperties.className = 'htCenter htMiddle htColorCabecera';
                    if (row == 0 && col == 1) cellProperties.className = 'htCenter htMiddle htColorCabecera';
                    if (row == 0 && col == 4) cellProperties.className = 'htCenter htMiddle htColorCabecera';
                    if (row == 1 && col == 1) cellProperties.className = 'htCenter htMiddle htColorCabecera';
                    if (row == 1 && col == 2) cellProperties.className = 'htCenter htMiddle htColorCabecera';
                    if (row == 1 && col == 3) cellProperties.className = 'htCenter htMiddle htColorCabecera';
                    if (row == 1 && col == 4) cellProperties.className = 'htCenter htMiddle htColorCabecera';
                    if (row == 2 && col == 1) cellProperties.className = 'htCenter htMiddle htColorCabecera';
                    if (row == 2 && col == 2) cellProperties.className = 'htCenter htMiddle htColorCabecera';
                    if (row == 2 && col == 3) cellProperties.className = 'htCenter htMiddle htColorCabecera';
                    if (row == 2 && col == 4) cellProperties.className = 'htCenter htMiddle htColorCabecera';

                    return cellProperties;
                },
                contextMenu: contexMenuConfigC2,
            })

            break;


        case CUADRO_3:

            var strListaFilasAPintar = $("#hdlistaColoreados").val();
            var lstFilas = [];
            var numFilas = 0;
            if (strListaFilasAPintar != "") {
                lstFilas = strListaFilasAPintar.split(",");
                numFilas = lstFilas.length;
            }

            btnEnModoEdicion(tipoReporte);

            document.getElementById('txtNombreReporteC3').readOnly = false;

            tblC3.updateSettings({
                cells: function (row, col, prop) {
                    var cellProperties = {};
                    cellProperties.readOnly = false;

                    if (row == 0 && col == 0) cellProperties.className = 'htCenter htMiddle htColorCabecera';
                    if (row == 0 && col == 1) cellProperties.className = 'htCenter htMiddle htColorCabecera';
                    if (row == 0 && col == 4) cellProperties.className = 'htCenter htMiddle htColorCabecera';
                    if (row == 1 && col == 1) cellProperties.className = 'htCenter htMiddle htColorCabecera';
                    if (row == 1 && col == 2) cellProperties.className = 'htCenter htMiddle htColorCabecera';
                    if (row == 1 && col == 3) cellProperties.className = 'htCenter htMiddle htColorCabecera';
                    //if (row== 1&& col== 4) cellProperties.className= 'htCenter htMiddle htColorCabecera' ;
                    if (row == 2 && col == 1) cellProperties.className = 'htCenter htMiddle htColorCabecera';
                    if (row == 2 && col == 2) cellProperties.className = 'htCenter htMiddle htColorCabecera';
                    if (row == 2 && col == 3) cellProperties.className = 'htCenter htMiddle htColorCabecera';
                    if (row == 2 && col == 4) cellProperties.className = 'htCenter htMiddle htColorCabecera';

                    
                    for (var i = 0; i < numFilas; i++) {
                        var fila = lstFilas[i];

                        if (row == fila && col == 0) cellProperties.className = 'htColorCelda';
                        if (row == fila && col == 1) cellProperties.className = 'htColorCelda';
                        if (row == fila && col == 2) cellProperties.className = 'htColorCelda';
                        if (row == fila && col == 3) cellProperties.className = 'htColorCelda';
                        if (row == fila && col == 4) cellProperties.className = 'htColorCelda';                        
                    }
                    
                    return cellProperties;
                },
                contextMenu: contexMenuConfigC3, 
            })

            break;

        case CUADRO_4:
            btnEnModoEdicionC4(tipoReporte);

            document.getElementById('txtNombreReporteC4').readOnly = false;
            
            break;

            
    }
}

function ponerAModoLectura(tipoReporte) {
    switch (tipoReporte) {
        case CUADRO_1:
            btnEnModoLectura(tipoReporte);

            document.getElementById('txtNombreReporteC1').readOnly = true;

            tblC1.updateSettings({
                cells: function (row, col, prop) {
                    var cellProperties = {};
                    cellProperties.readOnly = true;

                    if (row == 0 && col == 0) cellProperties.className = 'htCenter htMiddle htColorCabecera';
                    if (row == 0 && col == 1) cellProperties.className = 'htCenter htMiddle htColorCabecera';
                    if (row == 1 && col == 1) cellProperties.className = 'htCenter htMiddle htColorCabecera';
                    if (row == 1 && col == 2) cellProperties.className = 'htCenter htMiddle htColorCabecera';
                    if (row == 1 && col == 3) cellProperties.className = 'htCenter htMiddle htColorCabecera';
                    if (row == 2 && col == 1) cellProperties.className = 'htCenter htMiddle htColorCabecera';
                    if (row == 2 && col == 2) cellProperties.className = 'htCenter htMiddle htColorCabecera';
                    if (row == 2 && col == 3) cellProperties.className = 'htCenter htMiddle htColorCabecera';

                    return cellProperties;
                },
                contextMenu: false, // disable context menu to change things
            })

            break;

        case CUADRO_2:
            btnEnModoLectura(tipoReporte);

            document.getElementById('txtNombreReporteC2').readOnly = true;

            tblC2.updateSettings({
                cells: function (row, col, prop) {
                    var cellProperties = {};
                    cellProperties.readOnly = true;

                    if (row == 0 && col == 0) cellProperties.className = 'htCenter htMiddle htColorCabecera';
                    if (row == 0 && col == 1) cellProperties.className = 'htCenter htMiddle htColorCabecera';
                    if (row == 0 && col == 4) cellProperties.className = 'htCenter htMiddle htColorCabecera';
                    if (row == 1 && col == 1) cellProperties.className = 'htCenter htMiddle htColorCabecera';
                    if (row == 1 && col == 2) cellProperties.className = 'htCenter htMiddle htColorCabecera';
                    if (row == 1 && col == 3) cellProperties.className = 'htCenter htMiddle htColorCabecera';
                    if (row == 1 && col == 4) cellProperties.className = 'htCenter htMiddle htColorCabecera';
                    if (row == 2 && col == 1) cellProperties.className = 'htCenter htMiddle htColorCabecera';
                    if (row == 2 && col == 2) cellProperties.className = 'htCenter htMiddle htColorCabecera';
                    if (row == 2 && col == 3) cellProperties.className = 'htCenter htMiddle htColorCabecera';
                    if (row == 2 && col == 4) cellProperties.className = 'htCenter htMiddle htColorCabecera';

                    return cellProperties;
                },
                contextMenu: false, // disable context menu to change things
            })

            break;

        case CUADRO_3:

            var strListaFilasAPintar = $("#hdlistaColoreados").val();
            var lstFilas = [];
            var numFilas = 0;
            if (strListaFilasAPintar != "") {
                lstFilas = strListaFilasAPintar.split(",");
                numFilas = lstFilas.length;
            }

            btnEnModoLectura(tipoReporte);

            document.getElementById('txtNombreReporteC3').readOnly = true;

            tblC3.updateSettings({
                cells: function (row, col, prop) {
                    var cellProperties = {};
                    cellProperties.readOnly = true;

                    if (row == 0 && col == 0) cellProperties.className = 'htCenter htMiddle htColorCabecera';
                    if (row == 0 && col == 1) cellProperties.className = 'htCenter htMiddle htColorCabecera';
                    if (row == 0 && col == 4) cellProperties.className = 'htCenter htMiddle htColorCabecera';
                    if (row == 1 && col == 1) cellProperties.className = 'htCenter htMiddle htColorCabecera';
                    if (row == 1 && col == 2) cellProperties.className = 'htCenter htMiddle htColorCabecera';
                    if (row == 1 && col == 3) cellProperties.className = 'htCenter htMiddle htColorCabecera';
                    //if (row== 1&& col== 4) cellProperties.className= 'htCenter htMiddle htColorCabecera' ;
                    if (row == 2 && col == 1) cellProperties.className = 'htCenter htMiddle htColorCabecera';
                    if (row == 2 && col == 2) cellProperties.className = 'htCenter htMiddle htColorCabecera';
                    if (row == 2 && col == 3) cellProperties.className = 'htCenter htMiddle htColorCabecera';
                    if (row == 2 && col == 4) cellProperties.className = 'htCenter htMiddle htColorCabecera';

                    for (var i = 0; i < numFilas; i++) {
                        var fila = lstFilas[i];

                        if (row == fila && col == 0) cellProperties.className = 'htColorCelda';
                        if (row == fila && col == 1) cellProperties.className = 'htColorCelda';
                        if (row == fila && col == 2) cellProperties.className = 'htColorCelda';
                        if (row == fila && col == 3) cellProperties.className = 'htColorCelda';
                        if (row == fila && col == 4) cellProperties.className = 'htColorCelda';

                        
                    }

                    return cellProperties;
                },
                contextMenu: false, // disable context menu to change things
            })

            break;

        case CUADRO_4:
            btnEnModoLecturaC4(tipoReporte);

            document.getElementById('txtNombreReporteC4').readOnly = true;

            break;
    }
}

function btnEnModoLecturaC4(cuadro) {    
    $("#btnEditarC" + cuadro).css("display", "block");
    $("#btnGuardarC" + cuadro).css("display", "none");
    $("#btnVerHistorialC" + cuadro).css("display", "block");
}

function btnEnModoEdicionC4(cuadro) {
    $("#btnEditarC" + cuadro).css("display", "none");
    $("#btnGuardarC" + cuadro).css("display", "block");
    $("#btnVerHistorialC" + cuadro).css("display", "none");
}

function btnEnModoLectura(cuadro) {
    $("#btnProcesarC" + cuadro).css("display", "none");
    $("#btnNotasC" + cuadro).css("display", "none");
    $("#btnEditarC" + cuadro).css("display", "block");
    $("#btnGuardarC" + cuadro).css("display", "none");
    $("#btnVerHistorialC" + cuadro).css("display", "block");
    $("#btnMostrarErroresC" + cuadro).css("display", "none");
}

function btnEnModoEdicion(cuadro) {
    $("#btnProcesarC" + cuadro).css("display", "block");
    $("#btnNotasC" + cuadro).css("display", "block");
    $("#btnEditarC" + cuadro).css("display", "none");
    $("#btnGuardarC" + cuadro).css("display", "block");
    $("#btnVerHistorialC" + cuadro).css("display", "none");
    $("#btnMostrarErroresC" + cuadro).css("display", "block");
}

function btnEnModoProcesar(cuadro) {
    $("#btnProcesarC" + cuadro).css("display", "block");
    $("#btnNotasC" + cuadro).css("display", "none");
    $("#btnEditarC" + cuadro).css("display", "none");
    $("#btnGuardarC" + cuadro).css("display", "none");
    $("#btnVerHistorialC" + cuadro).css("display", "none");
    $("#btnMostrarErroresC" + cuadro).css("display", "none");
}


function mostrarErrores(tipoReporte) {
    var dataDelHandson;
    var listaErrores = [];
    var htmlErrores;
    switch (tipoReporte) {
        case CUADRO_1:
            dataDelHandson = tblC1.getData();
            listaErrores = obtenerErroresDeHandson(dataDelHandson, tipoReporte);
            htmlErrores = obtenerHtmlErrores(listaErrores);
            $('#div_errores').html(htmlErrores);

            break;

        case CUADRO_2:
            dataDelHandson = tblC2.getData();
            listaErrores = obtenerErroresDeHandson(dataDelHandson, tipoReporte);
            htmlErrores = obtenerHtmlErrores(listaErrores);
            $('#div_errores').html(htmlErrores);

            break;

        case CUADRO_3:
            dataDelHandson = tblC3.getData();
            listaErrores = obtenerErroresDeHandson(dataDelHandson, tipoReporte);
            htmlErrores = obtenerHtmlErrores(listaErrores);
            $('#div_errores').html(htmlErrores);

            break;
    }
    $('#tablaErrores').dataTable({
        "sDom": 't',
        "ordering": false,
    });
    abrirPopup("popupError");
}

function obtenerErroresDeHandson(handson, tipoReporte) {
    let errores = [];

    var numFilas = handson.length;
    var numColumnas;

    switch (tipoReporte) {
        case CUADRO_1: numColumnas = 4; break;
        case CUADRO_2: numColumnas = 5; break;
        case CUADRO_3: numColumnas = 5; break;
        case CUADRO_4: return errores; break;
    }

    for (var fila = 0; fila < numFilas; fila++) {
        for (var col = 0; col < numColumnas; col++) {
            var val = handson[fila][col];

            //Relleno las celdas vacias de la cabecera
            switch (tipoReporte) {
                case CUADRO_1:
                    if ((fila == 0 && col == 2) || (fila == 0 && col == 3) || (fila == 1 && col == 0) || (fila == 2 && col == 0))
                        val = "X";
                    break;
                case CUADRO_2:
                    if ((fila == 0 && col == 2) || (fila == 0 && col == 3) || (fila == 1 && col == 0) || (fila == 2 && col == 0))
                        val = "X";
                    break;
                case CUADRO_3:
                    if ((fila == 0 && col == 2) || (fila == 0 && col == 3) || (fila == 1 && col == 0) || (fila == 1 && col == 4) || (fila == 2 && col == 0))
                        val = "X";
                    break;
            }

            if (val == "") { //si es vacio
                let error = [
                    {
                        "Fila": fila + 1,
                        "Columna": col + 1,
                        "Descripcion": "Celda vacía."
                    }];
                errores.push(error);
            } else { // si no es vacio

                //solo para celdas donde son datos numericos o NO APLICA 
                if (fila > 2 && col > 0) {
                    if (!isNumber(val)) { //Si no son numeros
                        if (val.toUpperCase() != "NO APLICA") { //si son diferentes a NO APLICA
                            if (tipoReporte != CUADRO_3) {

                                let error = [
                                    {
                                        "Fila": fila + 1,
                                        "Columna": col + 1,
                                        "Descripcion": "Celda con texto desconocido. (Texto admitido: 'No Aplica')."
                                    }];
                                errores.push(error);
                            } else {
                                if (val.toUpperCase() != "---") {
                                    let error = [
                                        {
                                            "Fila": fila + 1,
                                            "Columna": col + 1,
                                            "Descripcion": "Celda con texto desconocido. (Textos admitidos: 'No Aplica','---')."
                                        }];
                                    errores.push(error);
                                }
                            }
                        }
                    } else { //si toma a # separados por comas  o a varios puntos como números
                        if (val.includes(',')) {
                            let error = [
                                {
                                    "Fila": fila + 1,
                                    "Columna": col + 1,
                                    "Descripcion": "Celda con separador numérico incorrecto. (el punto '.' es el separador númerico correcto)."
                                }];
                            errores.push(error);
                        }
                        
                    }
                }
            }
        }
    }

    return errores;
}

function isNumber(n) {
    'use strict';    
    n = n.replace(',', '.');
    return !isNaN(parseFloat(n)) && isFinite(n);
}

function obtenerHtmlErrores(listaErrores) {
    var cadena = '';

    cadena += `
            <table id='tablaErrores' class='pretty tabla-adicional'>
                <thead>
                    <tr>
                        <th style=''>Número de registro</th>
                        <th style=''>Fila</th>
                        <th style=''>Columna</th>
                        
                        <th style=''>Mensaje de Validación</th>
                    </tr>
                </thead>
                <tbody>
            `;
    for (var numError = 0; numError < listaErrores.length; numError++) {
        let regError = listaErrores[numError];
        cadena += `
                    <tr>
                        <td style='text-align: center;'>${numError + 1} </td>
                        <td>${regError[0]['Fila']} </td>
                        <td>${regError[0]['Columna']} </td>

                        <td>${regError[0]['Descripcion']} </td>
                    </tr>
                
            `;
    }
    cadena += `                    
                </tbody>
            </table>
     `;
    return cadena;
}


function mostrarHistorial(tipoReporte) {
    limpiarBarraMensaje("mensaje");
    limpiarBarraMensaje("mensaje_C1");
    limpiarBarraMensaje("mensaje_C2");
    limpiarBarraMensaje("mensaje_C3");
    limpiarBarraMensaje("mensaje_C4");
    limpiarBarraMensaje("mensaje_popupHistorial");
    
    var mesVigencia = $("#mesVigencia").val();
    $.ajax({
        type: 'POST',
        url: controlador + "ObtenerHistorial",
        data: {
            tipoReporte: tipoReporte,
            mesVigencia: mesVigencia
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                $('#div_historial').html(evt.HtmlHistorial);
                $('#tablaHistorial').dataTable({
                    "scrollY": 250,
                    "scrollX": false,
                    "sDom": 't',
                    "ordering": false,
                });
                abrirPopup("popupHistorial");

            } else {
                mostrarMensaje('mensaje_popupHistorial', 'error', evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            /*alert('Request Status: ' + err.status + ' Status Text: ' + err.statusText + ' DDD:' + err.responseText);*/
            /*alert(' Status Text: ' + err.statusText + ' DDD:' + err.responseText);*/
            //alert(' DDD:' + err.responseText);
        }
    });
}

function exportarReporte() {
    limpiarBarraMensaje("mensaje");

    $.ajax({
        type: 'POST',
        url: controlador + 'ExportarReporteCV',
        data: {
            mesVigencia : $("#mesVigencia").val()
        },
        dataType: 'json',
        success: function (evt) {
            if (evt.Resultado != "-1") {
                //limpiarBarraMensaje("mensaje");
                window.location = controlador + "Exportar?file_name=" + evt.Resultado;
            } else {
                mostrarMensaje('mensaje', 'error', evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}


function verReporteCVPorVersion(codigoReporte) {
    limpiarBarraMensaje("mensaje");
    limpiarBarraMensaje("mensaje_popupHistorial");
    
    var pestania = parseInt($("#hdPestania").val());
    $.ajax({
        type: 'POST',
        url: controlador + 'obtenerReportePorCodigo',
        data: {
            cbrepcodi: codigoReporte,
            tipoReporte: pestania
        },
        dataType: 'json',
        success: function (evt) {
            if (evt.Resultado != "-1") { 
                
                switch (pestania) {
                    case CUADRO_1:
                        dataC1 = evt.Data;
                        $("#hdreportecodiC1").val(evt.ReporteCodiC1);
                        mostrarReporte(CUADRO_1, evt.NombreReporteC1, evt.NotaRC1, true, null);
                        
                        break;
                    case CUADRO_2:
                        dataC2 = evt.Data;
                        $("#hdreportecodiC2").val(evt.ReporteCodiC1);
                        mostrarReporte(CUADRO_2, evt.NombreReporteC1, evt.NotaRC1, true, null);
                        
                        break;
                    case CUADRO_3:
                        dataC3 = evt.Data;
                        $("#hdreportecodiC3").val(evt.ReporteCodiC1);
                        mostrarReporte(CUADRO_3, evt.NombreReporteC1, evt.NotaRC1, true, evt.ListaFilasPintar);
                        
                        break;
                    case CUADRO_4:                        
                        $("#hdreportecodiC4").val(evt.ReporteCodiC1);
                        mostrarReporteC4(evt.NombreReporteC1, evt.MesAnio);
                        

                        break;
                    
                }
                cerrarPopup('popupHistorial');

            } else {
                mostrarMensaje('mensaje_popupHistorial', 'error', evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function mostrarCargarBD() {
    limpiarBarraMensaje("mensaje");
    limpiarBarraMensaje("mensaje_popupCargarBD");

    var mesVigencia = $("#mesVigencia").val();
    $.ajax({
        type: 'POST',
        url: controlador + "ObtenerDatosCarga",
        data: {
            mesVigencia: mesVigencia
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                $('#div_CargarBD').html(evt.HtmlCentrales);
                $('#tablaCargaBD').dataTable({
                    "sDom": 't',
                    "ordering": false,
                });
                $('#div_CVBD').html(evt.HtmlListado);
                $('#tabla').dataTable({
                    "sDom": 't',
                    "ordering": false,
                });
                abrirPopup("popupCargarBD");

            } else {
                mostrarMensaje('mensaje', 'error', evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');            
        }
    });
}


function cargarABD() {
    limpiarBarraMensaje("mensaje");
    limpiarBarraMensaje("mensaje_popupCargarBD");

    var lstCodigocv = ObtenerListaCodigocv();
    var lstCentrales = ObtenerListaCentralesMarcadas();

    if (lstCodigocv.length == 0) {
        alert("Debe seleccionar al menos un registro de Costos Variables");
        return;
    }

    if (confirm('¿Desea cargar la informacion a la base de datos?')) {

        var mesVigencia = $("#mesVigencia").val();

        var data = {

            mesVigencia: mesVigencia,
            listaCodicocv: lstCodigocv,
            lstCentrales: lstCentrales
        };

        $.ajax({
            type: 'POST',
            url: controlador + 'cargarABaseDeDatos',
            dataType: 'json',
            data: JSON.stringify(data),
            contentType: "application/json",
            cache: false,
            success: function (evt) {
                if (evt.Resultado == "1") {                    
                    cerrarPopup('popupCargarBD');
                    mostrarMensaje('mensaje', 'exito', "la información fue cargada a la base de datos correctamente.");
                }
                else {
                    mostrarMensaje('mensaje_popupCargarBD', 'error', evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    }
    
}

function ObtenerListaCodigocv() {
    var lstCodigocv = [];
    $("input:radio[name=chkCvariable]:checked").each(function () {
        lstCodigocv.push($(this).val());
    });
    return lstCodigocv;
}

function ObtenerListaCentralesMarcadas() {
    var lstCodigocv = [];
    $("input:checkbox[name=ckbCentral]:checked").each(function () {
        lstCodigocv.push($(this).val());
    });
    return lstCodigocv;
}

function cerrarPopup(id) {
    $("#" + id).bPopup().close()
}

function abrirPopup(contentPopup) {
    setTimeout(function () {
        $("#" + contentPopup).bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    }, 50);        
}


function mostrarMensaje(id, tipo, mensaje) {
    $("#" + id).css("display", "block");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);

}

function limpiarBarraMensaje(id) {
    $('#' + id).css("display", "none");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-message');
    $('#' + id).html('');
}



function DescargarF3InfSustXEmpresas() {

    $.ajax({
        type: 'POST',
        url: controlador + 'ExportarF3InfSustXEmpresas',
        data: {
            empresas: $('#hfEmpresaF3IS').val(),
            finicios: $('#FechaDesde').val(),
            ffins: $('#FechaHasta').val()
        },
        dataType: 'json',
        success: function (evt) {
            if (evt.Resultado != "-1") {
                limpiarBarraMensaje("mensaje");
                window.location = controlador + "ExportarZip?file_name=" + evt.Resultado;
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function setFechas() {
    var fecha = new Date();
    var fechafin = new Date();
    var stFecha = obtenerFechaMes(fecha, 0);
    var stFechaFin = obtenerFechaMes(fechafin, 1);
    $('#FechaDesde').val(stFecha);
    $('#FechaHasta').val(stFecha);

    $('#FechaDesde').Zebra_DatePicker({
        format: "m-Y",
        pair: $('#FechaHasta'),
        direction: ["01-2000", stFechaFin]
    });

    $('#FechaHasta').Zebra_DatePicker({
        format: "m-Y",
        //pair: $('#FechaDesde'),
        direction: [true, stFechaFin]
    });
}

function obtenerFechaMes(fecha, numero) {
    fecha = new Date(fecha.setMonth(fecha.getMonth() + numero));
    var mesfin = "0" + (fecha.getMonth() + 1).toString();
    mesfin = mesfin.substr(mesfin.length - 2, mesfin.length);
    var stFecha = mesfin + "-" + fecha.getFullYear();

    return stFecha;
}