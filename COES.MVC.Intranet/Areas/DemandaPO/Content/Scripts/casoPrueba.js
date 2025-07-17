// Constantes generales para el flujo de datos del modulo
const controller = siteRoot + "DemandaPO/CasoPrueba/";
const imageRoot = siteRoot + "Content/Images/";

// Variables para las grillas
var dt, dtFuncDm, dtFuncDp;
var dtDiasTipicosR1, dtDiasTipicosR2, dtDiasTipicosA1, dtDiasTipicosA2;
var dtDiasTipicosDpR1, dtDiasTipicosDpR2, dtDiasTipicosDpA1, dtDiasTipicosDpA2;

// Variables para los popup
var popCaso, popFiltro, popCopiar, popEliminar;
var popParametrosR1, popParametrosR2, popParametrosF1, popParametrosF2, popParametrosA1, popParametrosA2;
var popParametrosDpR1, popParametrosDpR2, popParametrosDpF1, popParametrosDpF2, popParametrosDpA1, popParametrosDpA2;

// Varables para controlar la modificación de parametros del caso de condiguración
var tempDataR1;
var tempDataR2;
var tempDataF1;
var tempDataF2;
var tempDataA1;
var tempDataA2;

var tempDataDpR1;
var tempDataDpR2;
var tempDataDpF1;
var tempDataDpF2;
var tempDataDpA1;
var tempDataDpA2;


// Varable registro o modificación de registro de caso de configuración
var idCaso;

const datosDm = [
    {
        Dposecuencma: null,
        Dpofnccodima: '1',
        Dpodesfundm: 'R1: Autocompletar datos del rango de fechas, tomando como referencia meses pasados',
    },
    {
        Dposecuencma: null,
        Dpofnccodima: '3',
        Dpodesfundm: 'F1: Filtrado por máxima variación por rampa',
    },
    {
        Dposecuencma: null,
        Dpofnccodima: '4',
        Dpodesfundm: 'F2: Exclusión de datos fuera del promedio +/- K* desviación estándar',
    },
    {
        Dposecuencma: null,
        Dpofnccodima: '5',
        Dpodesfundm: 'A1: De una serie de tiempo completa, obtener una semana típica',
    },
    {
        Dposecuencma: null,
        Dpofnccodima: '6',
        Dpodesfundm: 'A2: De una serie de tiempo completa, obtener los días típicos definidos en el caso, con su promedio y desviación estándar',
    },
];

const datosDp = [
    {
        Dposecuencpr: null,
        Dpofnccodipr: '1',
        Dpodesfunpr: 'R1: Autocompletar datos del rango de fechas, tomando como referencia meses pasados',
    },
    {
        Dposecuencpr: null,
        Dpofnccodipr: '2',
        Dpodesfunpr: 'R2: Reconstruir una serie de tiempo en base a otra serie de tiempo completa',
    },
    {
        Dposecuencpr: null,
        Dpofnccodipr: '3',
        Dpodesfunpr: 'F1: Filtrado por máxima variación por rampa',
    },
    {
        Dposecuencpr: null,
        Dpofnccodipr: '4',
        Dpodesfunpr: 'F2: Exclusión de datos fuera del promedio +/- K* desviación estándar',
    },
    {
        Dposecuencpr: null,
        Dpofnccodipr: '5',
        Dpodesfunpr: 'A1: De una serie de tiempo completa, obtener una semana típica',
    },
    {
        Dposecuencpr: null,
        Dpofnccodipr: '6',
        Dpodesfunpr: 'A2: De una serie de tiempo completa, obtener los días típicos definidos en el caso, con su promedio y desviación estándar',
    },
];


const diasTipicos = [
    { value: '0', text: '-' },
];


$(document).ready(function () {

    $('.pop-caso-calendar').Zebra_DatePicker();

    $('#pop-txtDesde-r1').Zebra_DatePicker({
        format: 'm Y',
    });

    $('#pop-txtHasta-r1').Zebra_DatePicker({
        format: 'm Y',
    });

    // Llamada al metodo que inicializa la grilla de Casos de prueba
    obtenerDtCasos();

    // Llamada al metodo que lista y llena la grilla con los casos de prueba
    listarCasos();

    $('#btnNuevo').on('click', function () {
        // Reemplaza el título del popup
        $('#pop-caso-titulo').html('Crear un nuevo caso de prueba');

        // Desbloquea los controles
        $('.popup-text').removeClass('disabled');
        $('#pop-caso-guardar').prop('disabled', false);

        idCaso = 0;

        // Establece en CANCELAR a la modificación de parametros del caso de condiguración
        tempDataR1 = "CANCELAR";
        tempDataR2 = "CANCELAR";
        tempDataF1 = "CANCELAR";
        tempDataF2 = "CANCELAR";
        tempDataA1 = "CANCELAR";
        tempDataA2 = "CANCELAR";

        popCaso = $('#pop-caso').bPopup({
            easing: 'easeOutBack',
            speed: 350,
            transition: 'fadeIn',
            modalClose: false,
            onOpen: function () {
                idCaso = 0;

                document.getElementById("pop-caso-nombre").value = "";
                document.getElementById("pop-caso-area").value = "";

                obtenerDtFuncionesDataMaestra(datosDm);
                obtenerDtFuncionesDataProcesar(datosDp);
            }
        });
    });

    $('#pop-caso-guardar').on('click', function () {
        nombre = $('#pop-caso-nombre').val();
        area = $('#pop-caso-area').val();

        formScadaDatMae = $('#pop-caso-maes-data').val();
        fecIniDatMae = $('#pop-caso-maes-fecini').val();
        fecFinDatMae = $('#pop-caso-maes-fecfin').val();

        formScadaDatPro = $('#pop-caso-proc-data').val();
        fecIniDatPro = $('#pop-caso-proc-fecini').val();
        fecFinDatPro = $('#pop-caso-proc-fecfin').val();

        if (!nombre) {
            alert('No coloco el nombre');
            return false;
        }

        if (!area) {
            alert('No selecciono el area operativa');
            return false;
        }


        if (!formScadaDatMae) {
            alert('No selecciono la formula SCADA de la data maestra');
            return false;
        }

        if (!fecIniDatMae) {
            alert('No selecciono la fecha de inicio de la formula SCADA de la data maestra');
            return false;
        }

        if (!fecFinDatMae) {
            alert('No selecciono la fecha de fin de la formula SCADA de la data maestra');
            return false;
        }

        // Valida consistencia del rango de fechas
        if (CompararFechas(fecIniDatMae, fecFinDatMae) == false) {
            alert("La fecha de inicio no puede ser mayor o igual que la fecha de fin de la formula SCADA de la data maestra");
            return;
        }


        if (!formScadaDatPro) {
            alert('No selecciono la formula SCADA de la data a procesar');
            return false;
        }

        if (!fecIniDatPro) {
            alert('No selecciono la fecha de inicio de la formula SCADA de la data a procesar');
            return false;
        }

        if (!fecFinDatPro) {
            alert('No selecciono la fecha de fin de la formula SCADA de la data a procesar');
            return false;
        }

        // Valida consistencia del rango de fechas
        if (CompararFechas(fecIniDatPro, fecFinDatPro) == false) {
            alert("La fecha de inicio no puede ser mayor o igual que la fecha de fin de la formula SCADA de la data a procesar");
            return;
        }


        if (idCaso == 0) {
            agregarCaso();
        }
        else {
            editarCaso(idCaso)
        }
    });

    $('#pop-caso-ejecutar').on('click', function () {
        ejecutarCaso();
    });

    $('#btnFiltro').on('click', function () {
        popFiltro = $('#pop-filtro').bPopup({
            easing: 'easeOutBack',
            speed: 350,
            transition: 'fadeIn',
            modalClose: false,
            onOpen: function () {
                document.getElementById("pop-filtro-nombre").value = "";
                document.getElementById("pop-filtro-area").value = "";
                document.getElementById("pop-filtro-usuario").value = "";
            }
        });
    });

    $('#pop-filtro-aceptar').on('click', function () {
        filtrarCasos();
    });

    $('#pop-eliminar-aceptar').on('click', function () {
        eliminarCaso(idCaso)
    });

    $('#pop-copiar-aceptar').on('click', function () {
        copiarCaso(idCaso)
    });

    $('#pop-caso-area').on('change', function () {
        listarFormulas();
    });



    // Botones Aceptar y Cancelar del popup de parametros R1 - Data Maestra
    $('#pop-aceptar-r1').click(function () {
        // Establece en MODIFICAR a la modificación de parametros del caso de condiguración
        tempDataR1 = "MODIFICAR"
        $('#pop-parametros-r1').bPopup().close();
    });
    $('#pop-cancelar-r1').click(function () {
        // Establece en CANCELAR a la modificación de parametros del caso de condiguración
        tempDataR1 = "CANCELAR"
        $('#pop-parametros-r1').bPopup().close();
    });

    // Botones Aceptar y Cancelar del popup de parametros R2 - Data Maestra
    $('#pop-aceptar-r2').click(function () {
        // Establece en MODIFICAR a la modificación de parametros del caso de condiguración
        tempDataR2 = "MODIFICAR"
        $('#pop-parametros-r2').bPopup().close();
    });
    $('#pop-cancelar-r2').click(function () {
        // Establece en CANCELAR a la modificación de parametros del caso de condiguración
        tempDataR2 = "CANCELAR"
        $('#pop-parametros-r2').bPopup().close();
    });

    // Botones Aceptar y Cancelar del popup de parametros F1 - Data Maestra
    $('#pop-aceptar-f1').click(function () {
        // Establece en MODIFICAR a la modificación de parametros del caso de condiguración
        tempDataF1 = "MODIFICAR"
        $('#pop-parametros-f1').bPopup().close();
    });
    $('#pop-cancelar-f1').click(function () {
        // Establece en CANCELAR a la modificación de parametros del caso de condiguración
        tempDataF1 = "CANCELAR"
        $('#pop-parametros-f1').bPopup().close();
    });

    // Botones Aceptar y Cancelar del popup de parametros F2 - Data Maestra
    $('#pop-aceptar-f2').click(function () {
        // Establece en MODIFICAR a la modificación de parametros del caso de condiguración
        tempDataF2 = "MODIFICAR"
        $('#pop-parametros-f2').bPopup().close();
    });
    $('#pop-cancelar-f2').click(function () {
        // Establece en CANCELAR a la modificación de parametros del caso de condiguración
        tempDataF2 = "CANCELAR"
        $('#pop-parametros-f2').bPopup().close();
    });

    // Botones Aceptar y Cancelar del popup de parametros A1 - Data Maestra
    $('#pop-aceptar-a1').click(function () {
        // Establece en MODIFICAR a la modificación de parametros del caso de condiguración
        tempDataA1 = "MODIFICAR"
        $('#pop-parametros-a1').bPopup().close();
    });
    $('#pop-cancelar-a1').click(function () {
        // Establece en CANCELAR a la modificación de parametros del caso de condiguración
        tempDataA1 = "CANCELAR"
        $('#pop-parametros-a1').bPopup().close();
    });

    // Botones Aceptar y Cancelar del popup de parametros A2 - Data Maestra
    $('#pop-aceptar-a2').click(function () {
        // Establece en MODIFICAR a la modificación de parametros del caso de condiguración
        tempDataA2 = "MODIFICAR"
        $('#pop-parametros-a2').bPopup().close();
    });
    $('#pop-cancelar-a2').click(function () {
        // Establece en CANCELAR a la modificación de parametros del caso de condiguración
        tempDataA2 = "CANCELAR"
        $('#pop-parametros-a2').bPopup().close();
    });



    // Botones Aceptar y Cancelar del popup de parametros R1 - Data a Procesar
    $('#pop-aceptar-dp-r1').click(function () {
        // Establece en MODIFICAR a la modificación de parametros del caso de condiguración
        tempDataDpR1 = "MODIFICAR"
        $('#pop-parametros-dp-r1').bPopup().close();
    });
    $('#pop-cancelar-dp-r1').click(function () {
        // Establece en CANCELAR a la modificación de parametros del caso de condiguración
        tempDataDpR1 = "CANCELAR"
        $('#pop-parametros-dp-r1').bPopup().close();
    });

    // Botones Aceptar y Cancelar del popup de parametros R2 - Data a Procesar
    $('#pop-aceptar-dp-r2').click(function () {
        // Establece en MODIFICAR a la modificación de parametros del caso de condiguración
        tempDataDpR2 = "MODIFICAR"
        $('#pop-parametros-dp-r2').bPopup().close();
    });
    $('#pop-cancelar-dp-r2').click(function () {
        // Establece en CANCELAR a la modificación de parametros del caso de condiguración
        tempDataDpR2 = "CANCELAR"
        $('#pop-parametros-dp-r2').bPopup().close();
    });

    // Botones Aceptar y Cancelar del popup de parametros F1 - Data a Procesar
    $('#pop-aceptar-dp-f1').click(function () {
        // Establece en MODIFICAR a la modificación de parametros del caso de condiguración
        tempDataDpF1 = "MODIFICAR"
        $('#pop-parametros-dp-f1').bPopup().close();
    });
    $('#pop-cancelar-dp-f1').click(function () {
        // Establece en CANCELAR a la modificación de parametros del caso de condiguración
        tempDataDpF1 = "CANCELAR"
        $('#pop-parametros-dp-f1').bPopup().close();
    });

    // Botones Aceptar y Cancelar del popup de parametros F2 - Data a Procesar
    $('#pop-aceptar-dp-f2').click(function () {
        // Establece en MODIFICAR a la modificación de parametros del caso de condiguración
        tempDataDpF2 = "MODIFICAR"
        $('#pop-parametros-dp-f2').bPopup().close();
    });
    $('#pop-cancelar-dp-f2').click(function () {
        // Establece en CANCELAR a la modificación de parametros del caso de condiguración
        tempDataDpF2 = "CANCELAR"
        $('#pop-parametros-dp-f2').bPopup().close();
    });

    // Botones Aceptar y Cancelar del popup de parametros A1 - Data a Procesar
    $('#pop-aceptar-dp-a1').click(function () {
        // Establece en MODIFICAR a la modificación de parametros del caso de condiguración
        tempDataDpA1 = "MODIFICAR"
        $('#pop-parametros-dp-a1').bPopup().close();
    });
    $('#pop-cancelar-dp-a1').click(function () {
        // Establece en CANCELAR a la modificación de parametros del caso de condiguración
        tempDataDpA1 = "CANCELAR"
        $('#pop-parametros-dp-a1').bPopup().close();
    });

    // Botones Aceptar y Cancelar del popup de parametros A2 - Data a Procesar
    $('#pop-aceptar-dp-a2').click(function () {
        // Establece en MODIFICAR a la modificación de parametros del caso de condiguración
        tempDataDpA2 = "MODIFICAR"
        $('#pop-parametros-dp-a2').bPopup().close();
    });
    $('#pop-cancelar-dp-a2').click(function () {
        // Establece en CANCELAR a la modificación de parametros del caso de condiguración
        tempDataDpA2 = "CANCELAR"
        $('#pop-parametros-dp-a2').bPopup().close();
    });



    // Llamada al metodo que lista y llena la grilla de funciones para la Data maestra
    obtenerDtFuncionesDataMaestra();

    // Escuchar el evento 'click' en la celda especifica de la grilla para la Data maestra
    $('#dtFuncDm tbody').on('click', 'tr td .dt-ico-editar', function () {
        var row = $(this).closest('tr');
        var r = dtFuncDm.row(row).data();
        var fila = dtFuncDm.row(row).index();

        if (fila === 0) {
            popParametrosR1 = $('#pop-parametros-r1').bPopup({
                easing: 'easeOutBack',
                speed: 350,
                transition: 'fadeIn',
                modalClose: false,
                onOpen: function () {
                    if (idCaso != 0) {
                        if (tempDataR1 == "CANCELAR") {
                            listarParametrosR1(idCaso, r.Dpocasdetcodi);
                        }
                    }
                }
            });
        }
        else if (fila === 1) {
            popParametrosF1 = $('#pop-parametros-f1').bPopup({
                easing: 'easeOutBack',
                speed: 350,
                transition: 'fadeIn',
                modalClose: false,
                onOpen: function () {
                    if (idCaso != 0) {
                        if (tempDataF1 == "CANCELAR") {
                            listarParametrosF1(idCaso, r.Dpocasdetcodi);
                        }
                    }
                }
            });
        }
        else if (fila === 2) {
            popParametrosF2 = $('#pop-parametros-f2').bPopup({
                easing: 'easeOutBack',
                speed: 350,
                transition: 'fadeIn',
                modalClose: false,
                onOpen: function () {
                    if (idCaso != 0) {
                        if (tempDataF2 == "CANCELAR") {
                            listarParametrosF2(idCaso, r.Dpocasdetcodi);
                        }
                    }
                }
            });
        }
        else if (fila === 3) {
            popParametrosA1 = $('#pop-parametros-a1').bPopup({
                easing: 'easeOutBack',
                speed: 350,
                transition: 'fadeIn',
                modalClose: false,
                onOpen: function () {
                    if (idCaso != 0) {
                        if (tempDataA1 == "CANCELAR") {
                            listarParametrosA1(idCaso, r.Dpocasdetcodi);
                        }
                    }
                }
            });
        }
        else if (fila === 4) {
            popParametrosA2 = $('#pop-parametros-a2').bPopup({
                easing: 'easeOutBack',
                speed: 350,
                transition: 'fadeIn',
                modalClose: false,
                onOpen: function () {
                    if (idCaso != 0) {
                        if (tempDataA2 == "CANCELAR") {
                            listarParametrosA2(idCaso, r.Dpocasdetcodi);
                        }
                    }
                }
            });
        }

    });

    // Llamada al metodo que lista y llena la grilla de funciones para la Data a procesar
    obtenerDtFuncionesDataProcesar();

    // Escuchar el evento 'click' en la celda especifica de la grilla para la Data a procesar
    $('#dtFuncDp tbody').on('click', 'tr td .dt-ico-editar', function () {
        var row = $(this).closest('tr');
        var r = dtFuncDp.row(row).data();
        var fila = dtFuncDp.row(row).index();

        if (fila === 0) {
            popParametrosDpR1 = $('#pop-parametros-dp-r1').bPopup({
                easing: 'easeOutBack',
                speed: 350,
                transition: 'fadeIn',
                modalClose: false,
                onOpen: function () {
                    if (idCaso != 0) {
                        if (tempDataDpR1 == "CANCELAR") {
                            listarParametrosDpR1(idCaso, r.Dpocasdetcodi);
                        }
                    }
                }
            });
        }
        else if (fila === 1) {
            popParametrosDpR2 = $('#pop-parametros-dp-r2').bPopup({
                easing: 'easeOutBack',
                speed: 350,
                transition: 'fadeIn',
                modalClose: false,
                onOpen: function () {
                    if (idCaso != 0) {
                        if (tempDataDpR2 == "CANCELAR") {
                            listarParametrosDpR2(idCaso, r.Dpocasdetcodi);
                        }
                    }
                }
            });
        }
        else if (fila === 2) {
            popParametrosDpF1 = $('#pop-parametros-dp-f1').bPopup({
                easing: 'easeOutBack',
                speed: 350,
                transition: 'fadeIn',
                modalClose: false,
                onOpen: function () {
                    if (idCaso != 0) {
                        if (tempDataDpF1 == "CANCELAR") {
                            listarParametrosDpF1(idCaso, r.Dpocasdetcodi);
                        }
                    }
                }
            });
        }
        else if (fila === 3) {
            popParametrosDpF2 = $('#pop-parametros-dp-f2').bPopup({
                easing: 'easeOutBack',
                speed: 350,
                transition: 'fadeIn',
                modalClose: false,
                onOpen: function () {
                    if (idCaso != 0) {
                        if (tempDataDpF2 == "CANCELAR") {
                            listarParametrosDpF2(idCaso, r.Dpocasdetcodi);
                        }
                    }
                }
            });
        }
        else if (fila === 4) {
            popParametrosDpA1 = $('#pop-parametros-dp-a1').bPopup({
                easing: 'easeOutBack',
                speed: 350,
                transition: 'fadeIn',
                modalClose: false,
                onOpen: function () {
                    if (idCaso != 0) {
                        if (tempDataDpA1 == "CANCELAR") {
                            listarParametrosDpA1(idCaso, r.Dpocasdetcodi);
                        }
                    }
                }
            });
        }
        else if (fila === 5) {
            popParametrosDpA2 = $('#pop-parametros-dp-a2').bPopup({
                easing: 'easeOutBack',
                speed: 350,
                transition: 'fadeIn',
                modalClose: false,
                onOpen: function () {
                    if (idCaso != 0) {
                        if (tempDataDpA2 == "CANCELAR") {
                            listarParametrosDpA2(idCaso, r.Dpocasdetcodi);
                        }
                    }
                }
            });
        }
    });


    // Llamada al metodo que lista y llena la grilla de dias tipicos para el popup de los parametros de la Formula R1 - Data maestra
    obtenerDtDiasTipicosR1();

    // Llamada al metodo que lista y llena la grilla de dias tipicos para el popup de los parametros de la Formula R2 - Data maestra
    obtenerDtDiasTipicosR2();

    // Llamada al metodo que lista y llena la grilla de dias tipicos para el popup de los parametros de la Formula A1 - Data maestra
    obtenerDtDiasTipicosA1();

    // Llamada al metodo que lista y llena la grilla de dias tipicos para el popup de los parametros de la Formula A2 - Data maestra
    obtenerDtDiasTipicosA2();



    // Llamada al metodo que lista y llena la grilla de dias tipicos para el popup de los parametros de la Formula R1 - Data a Procesar
    obtenerDtDiasTipicosDpR1();

    // Llamada al metodo que lista y llena la grilla de dias tipicos para el popup de los parametros de la Formula R2 - Data a Procesar
    obtenerDtDiasTipicosDpR2();

    // Llamada al metodo que lista y llena la grilla de dias tipicos para el popup de los parametros de la Formula A1 - Data a Procesar
    obtenerDtDiasTipicosDpA1();

    // Llamada al metodo que lista y llena la grilla de dias tipicos para el popup de los parametros de la Formula A2 - Data a Procesar
    obtenerDtDiasTipicosDpA2();
});

function listarCasos() {
    $.ajax({
        type: 'POST',
        url: controller + 'ListarCasos',
        contentType: 'application/json; charset=utf-8',
        data: {},
        datatype: 'json',
        traditional: true,
        success: function (result) {
            if (result.Resultado === "-1") {
                alert("Ocurrio un error inesperado");
            } else {
                // Llena la grilla de casos de prueba
                dt.clear();
                dt.rows.add(result.ListaCasos);
                dt.draw();
            }
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function filtrarCasos() {
    $.ajax({
        type: 'POST',
        url: controller + 'FiltrarCasos',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            nombre: $('#pop-filtro-nombre').val(),
            areaOperativa: $('#pop-filtro-area').val(),
            usuario: $('#pop-filtro-usuario').val(),
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            if (result.Resultado === "-1") {
                alert("Ocurrio un error inesperado");
            } else {
                // Llena la grilla de casos de prueba
                dt.clear();
                dt.rows.add(result.ListaCasos);
                dt.draw();

                $("#pop-filtro").bPopup().close();
            }
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}


function listarFunciones(idCaso) {
    $.ajax({
        type: 'POST',
        url: controller + 'ListarFunciones',
        contentType: 'application/json; charset=utf-8',
        datatype: 'json',
        data: JSON.stringify({
            idCaso: idCaso
        }),
        traditional: true,
        success: function (modelo) {
            document.getElementById("pop-caso-maes-data").value = modelo.listFuncionesDataMaestra[0].Dpodetmafscada;
            document.getElementById("pop-caso-maes-fecini").value = modelo.listFuncionesDataMaestra[0].StrDpodetmatinicio;
            document.getElementById("pop-caso-maes-fecfin").value = modelo.listFuncionesDataMaestra[0].StrDpodetmatfin;

            // Llena la Grilla de funciones de data maestra
            dtFuncDm.clear();
            dtFuncDm.rows.add(modelo.listFuncionesDataMaestra);
            dtFuncDm.draw();

            document.getElementById("pop-caso-proc-data").value = modelo.listFuncionesDataProcesar[0].Dpodetprfscada;
            document.getElementById("pop-caso-proc-fecini").value = modelo.listFuncionesDataProcesar[0].StrDpodetprinicio;
            document.getElementById("pop-caso-proc-fecfin").value = modelo.listFuncionesDataProcesar[0].StrDpodetprfin;

            // Llena la Grilla de funciones de data a procesar
            dtFuncDp.clear();
            dtFuncDp.rows.add(modelo.listFuncionesDataProcesar);
            dtFuncDp.draw();
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}


function listarParametrosR1(idCaso, idDetalleCaso) {
    $.ajax({
        type: 'POST',
        url: controller + 'ListarParametrosR1',
        contentType: 'application/json; charset=utf-8',
        datatype: 'json',
        data: JSON.stringify({
            idCaso: idCaso,
            idDetalleCaso: idDetalleCaso,
        }),
        traditional: true,
        success: function (modelo) {
            dtDiasTipicosR1.clear();
            dtDiasTipicosR1.rows.add(modelo.listParametrosR1);
            dtDiasTipicosR1.draw();

            document.getElementById("pop-txtDesde-r1").value = modelo.listParametrosR1[0].Pafunr1deg7;
            document.getElementById("pop-txtHasta-r1").value = modelo.listParametrosR1[0].Pafunr1hag7;
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function listarParametrosR2(idCaso, idDetalleCaso) {
    $.ajax({
        type: 'POST',
        url: controller + 'ListarParametrosR2',
        contentType: 'application/json; charset=utf-8',
        datatype: 'json',
        data: JSON.stringify({
            idCaso: idCaso,
            idDetalleCaso: idDetalleCaso,
        }),
        traditional: true,
        success: function (modelo) {
            dtDiasTipicosR2.clear();
            dtDiasTipicosR2.rows.add(modelo.listParametrosR2);
            dtDiasTipicosR2.draw();
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function listarParametrosF1(idCaso, idDetalleCaso) {
    $.ajax({
        type: 'POST',
        url: controller + 'ListarParametrosF1',
        contentType: 'application/json; charset=utf-8',
        datatype: 'json',
        data: JSON.stringify({
            idCaso: idCaso,
            idDetalleCaso: idDetalleCaso,
        }),
        traditional: true,
        success: function (modelo) {
            document.getElementById("pop-txtToleraciaRampa-f1").value = modelo.listParametrosF1[0].Pafunf1toram;
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function listarParametrosF2(idCaso, idDetalleCaso) {
    $.ajax({
        type: 'POST',
        url: controller + 'ListarParametrosF2',
        contentType: 'application/json; charset=utf-8',
        datatype: 'json',
        data: JSON.stringify({
            idCaso: idCaso,
            idDetalleCaso: idDetalleCaso,
        }),
        traditional: true,
        success: function (modelo) {
            document.getElementById("pop-txtFactorK-f2").value = modelo.listParametrosF2[0].Pafunf2factk;
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function listarParametrosA1(idCaso, idDetalleCaso) {
    $.ajax({
        type: 'POST',
        url: controller + 'ListarParametrosA1',
        contentType: 'application/json; charset=utf-8',
        datatype: 'json',
        data: JSON.stringify({
            idCaso: idCaso,
            idDetalleCaso: idDetalleCaso,
        }),
        traditional: true,
        success: function (modelo) {
            dtDiasTipicosA1.clear();
            dtDiasTipicosA1.rows.add(modelo.listParametrosA1);
            dtDiasTipicosA1.draw();

            document.getElementById("pop-txtFechaAnio-a1").value = modelo.listParametrosA1[0].Pafuna1aniof;
            document.getElementById("pop-filtro-feriado-a1").value = modelo.listParametrosA1[0].Pafuna1idfer;
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function listarParametrosA2(idCaso, idDetalleCaso) {
    $.ajax({
        type: 'POST',
        url: controller + 'ListarParametrosA2',
        contentType: 'application/json; charset=utf-8',
        datatype: 'json',
        data: JSON.stringify({
            idCaso: idCaso,
            idDetalleCaso: idDetalleCaso,
        }),
        traditional: true,
        success: function (modelo) {
            dtDiasTipicosA2.clear();
            dtDiasTipicosA2.rows.add(modelo.listParametrosA2);
            dtDiasTipicosA2.draw();

            document.getElementById("pop-txtFechaAnio-a2").value = modelo.listParametrosA2[0].Pafuna2aniof;
            document.getElementById("pop-filtro-feriado-a2").value = modelo.listParametrosA2[0].Pafuna2idfer;
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}


function listarParametrosDpR1(idCaso, idDetalleCaso) {
    $.ajax({
        type: 'POST',
        url: controller + 'ListarParametrosR1',
        contentType: 'application/json; charset=utf-8',
        datatype: 'json',
        data: JSON.stringify({
            idCaso: idCaso,
            idDetalleCaso: idDetalleCaso,
        }),
        traditional: true,
        success: function (modelo) {
            dtDiasTipicosDpR1.clear();
            dtDiasTipicosDpR1.rows.add(modelo.listParametrosR1);
            dtDiasTipicosDpR1.draw();

            document.getElementById("pop-txtDesde-dp-r1").value = modelo.listParametrosR1[0].Pafunr1deg7;
            document.getElementById("pop-txtHasta-dp-r1").value = modelo.listParametrosR1[0].Pafunr1hag7;
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function listarParametrosDpR2(idCaso, idDetalleCaso) {
    $.ajax({
        type: 'POST',
        url: controller + 'ListarParametrosR2',
        contentType: 'application/json; charset=utf-8',
        datatype: 'json',
        data: JSON.stringify({
            idCaso: idCaso,
            idDetalleCaso: idDetalleCaso,
        }),
        traditional: true,
        success: function (modelo) {
            dtDiasTipicosDpR2.clear();
            dtDiasTipicosDpR2.rows.add(modelo.listParametrosR2);
            dtDiasTipicosDpR2.draw();
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function listarParametrosDpF1(idCaso, idDetalleCaso) {
    $.ajax({
        type: 'POST',
        url: controller + 'ListarParametrosF1',
        contentType: 'application/json; charset=utf-8',
        datatype: 'json',
        data: JSON.stringify({
            idCaso: idCaso,
            idDetalleCaso: idDetalleCaso,
        }),
        traditional: true,
        success: function (modelo) {
            document.getElementById("pop-txtToleraciaRampa-dp-f1").value = modelo.listParametrosF1[0].Pafunf1toram;
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function listarParametrosDpF2(idCaso, idDetalleCaso) {
    $.ajax({
        type: 'POST',
        url: controller + 'ListarParametrosF2',
        contentType: 'application/json; charset=utf-8',
        datatype: 'json',
        data: JSON.stringify({
            idCaso: idCaso,
            idDetalleCaso: idDetalleCaso,
        }),
        traditional: true,
        success: function (modelo) {
            document.getElementById("pop-txtFactorK-dp-f2").value = modelo.listParametrosF2[0].Pafunf2factk;
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function listarParametrosDpA1(idCaso, idDetalleCaso) {
    $.ajax({
        type: 'POST',
        url: controller + 'ListarParametrosA1',
        contentType: 'application/json; charset=utf-8',
        datatype: 'json',
        data: JSON.stringify({
            idCaso: idCaso,
            idDetalleCaso: idDetalleCaso,
        }),
        traditional: true,
        success: function (modelo) {
            dtDiasTipicosDpA1.clear();
            dtDiasTipicosDpA1.rows.add(modelo.listParametrosA1);
            dtDiasTipicosDpA1.draw();

            document.getElementById("pop-txtFechaAnio-dp-a1").value = modelo.listParametrosA1[0].Pafuna1aniof;
            document.getElementById("pop-filtro-feriado-dp-a1").value = modelo.listParametrosA1[0].Pafuna1idfer;
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function listarParametrosDpA2(idCaso, idDetalleCaso) {
    $.ajax({
        type: 'POST',
        url: controller + 'ListarParametrosA2',
        contentType: 'application/json; charset=utf-8',
        datatype: 'json',
        data: JSON.stringify({
            idCaso: idCaso,
            idDetalleCaso: idDetalleCaso,
        }),
        traditional: true,
        success: function (modelo) {
            dtDiasTipicosDpA2.clear();
            dtDiasTipicosDpA2.rows.add(modelo.listParametrosA2);
            dtDiasTipicosDpA2.draw();

            document.getElementById("pop-txtFechaAnio-dp-a2").value = modelo.listParametrosA2[0].Pafuna2aniof;
            document.getElementById("pop-filtro-feriado-dp-a2").value = modelo.listParametrosA2[0].Pafuna2idfer;
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}


function agregarCaso() {
    const datosFuncDm = dtFuncDm
        .rows()
        .data()
        .toArray();

    const datosFuncDp = dtFuncDp
        .rows()
        .data()
        .toArray();

    // -----------------------------------------------------------

    const datosDtR1 = dtDiasTipicosR1
        .rows()
        .data()
        .toArray();

    const datosDtR2 = dtDiasTipicosR2
        .rows()
        .data()
        .toArray();

    const datosDtA1 = dtDiasTipicosA1
        .rows()
        .data()
        .toArray();

    const datosDtA2 = dtDiasTipicosA2
        .rows()
        .data()
        .toArray();

    // --------------------------------------------

    const datosDtDpR1 = dtDiasTipicosDpR1
        .rows()
        .data()
        .toArray();

    const datosDtDpR2 = dtDiasTipicosDpR2
        .rows()
        .data()
        .toArray();

    const datosDtDpA1 = dtDiasTipicosDpA1
        .rows()
        .data()
        .toArray();

    const datosDtDpA2 = dtDiasTipicosDpA2
        .rows()
        .data()
        .toArray();

    $.ajax({
        type: 'POST',
        url: controller + 'AgregarCaso',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            nombre: $('#pop-caso-nombre').val(),
            areaOperativa: $('#pop-caso-area').val(),

            formScadaDatMae: $('#pop-caso-maes-data').val(),
            fecIniDatMae: $('#pop-caso-maes-fecini').val(),
            fecFinDatMae: $('#pop-caso-maes-fecfin').val(),
            listFuncionesDataMaestra: datosFuncDm,

            formScadaDatPro: $('#pop-caso-proc-data').val(),
            fecIniDatPro: $('#pop-caso-proc-fecini').val(),
            fecFinDatPro: $('#pop-caso-proc-fecfin').val(),
            listFuncionesDataProcesar: datosFuncDp,

            // --------------------------------------------------------

            listDiasTipicosR1: datosDtR1,
            fecIniR1: $('#pop-txtDesde-r1').val(),
            fecFinR1: $('#pop-txtHasta-r1').val(),

            listDiasTipicosR2: datosDtR2,

            numToleranciaRampaF1: $('#pop-txtToleraciaRampa-f1').val(),
            numFactorKF2: $('#pop-txtFactorK-f2').val(),

            /*anioFeriadoA1: $('#pop-txtFechaAnio-a1').val(),*/
            /*idFeriadoA1: $('#pop-filtro-feriado-a1').val(),*/
            anioFeriadoA1: 0,
            idFeriadoA1: 0,

            listDiasTipicosA1: datosDtA1,

            /*anioFeriadoA2: $('#pop-txtFechaAnio-a2').val(),*/
            /*idFeriadoA2: $('#pop-filtro-feriado-a2').val(),*/
            anioFeriadoA2: 0,
            idFeriadoA2: 0,

            listDiasTipicosA2: datosDtA2,

            // --------------------------------------------------------

            listDiasTipicosDpR1: datosDtDpR1,
            fecIniDpR1: $('#pop-txtDesde-dp-r1').val(),
            fecFinDpR1: $('#pop-txtHasta-dp-r1').val(),

            listDiasTipicosDpR2: datosDtDpR2,

            numToleranciaRampaDpF1: $('#pop-txtToleraciaRampa-dp-f1').val(),
            numFactorKDpF2: $('#pop-txtFactorK-dp-f2').val(),

            /*anioFeriadoDpA1: $('#pop-txtFechaAnio-dp-a1').val(),*/
            /*idFeriadoDpA1: $('#pop-filtro-feriado-dp-a1').val(),*/
            anioFeriadoDpA1: 0,
            idFeriadoDpA1: 0,

            listDiasTipicosDpA1: datosDtDpA1,

            /*anioFeriadoA2: $('#pop-txtFechaAnio-dp-a2').val(),*/
            /*idFeriadoA2: $('#pop-filtro-feriado-dp-a2').val(),*/
            anioFeriadoDpA2: 0,
            idFeriadoDpA2: 0,

            listDiasTipicosDpA2: datosDtDpA2,

        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            // Llena la grilla de casos de prueba
            dt.clear();
            dt.rows.add(result.ListaCasos);
            dt.draw();

            idCaso = result.idCaso;

            // Establece en CANCELAR a la modificación de parametros del caso de condiguración
            tempDataR1 = "CANCELAR";
            tempDataR2 = "CANCELAR";
            tempDataF1 = "CANCELAR";
            tempDataF2 = "CANCELAR";
            tempDataA1 = "CANCELAR";
            tempDataA2 = "CANCELAR";

            tempDataDpR1 = "CANCELAR";
            tempDataDpR2 = "CANCELAR";
            tempDataDpF1 = "CANCELAR";
            tempDataDpF2 = "CANCELAR";
            tempDataDpA1 = "CANCELAR";
            tempDataDpA2 = "CANCELAR";

            // Refresca grilla de funciones de data maestra
            listarFunciones(result.idCaso);

            alert(result.Mensaje);
            /*$("#pop-caso").bPopup().close();*/
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function ejecutarCaso() {
    const datosFuncDm = dtFuncDm
        .rows()
        .data()
        .toArray();

    const datosFuncDp = dtFuncDp
        .rows()
        .data()
        .toArray();

    $.ajax({
        type: 'POST',
        url: controller + 'EjecutarCaso',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            idCaso: idCaso,
            formScadaDatMae: $('#pop-caso-maes-data').val(),
            fecIniDatMae: $('#pop-caso-maes-fecini').val(),
            fecFinDatMae: $('#pop-caso-maes-fecfin').val(),
            listFuncionesDataMaestra: datosFuncDm,
            formScadaDatPro: $('#pop-caso-proc-data').val(),
            fecIniDatPro: $('#pop-caso-proc-fecini').val(),
            fecFinDatPro: $('#pop-caso-proc-fecfin').val(),
            listFuncionesDataProcesar: datosFuncDp
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            if (result.Resultado === "-1") {
                alert("Ocurrio un error inesperado");
            } else if (result.Resultado === "-2") {
                alert(result.Mensaje);
            } else {
                window.location = controller + 'AbrirArchivo?formato=' + 3 + '&file=' + result.Resultado;
                alert(result.Mensaje);
                $("#pop-caso").bPopup().close();
            }
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function editarCaso(id) {

    const datosFuncDm = dtFuncDm
        .rows()
        .data()
        .toArray();

    const datosFuncDp = dtFuncDp
        .rows()
        .data()
        .toArray();

    // -----------------------------------------------------------

    const datosDtR1 = dtDiasTipicosR1
        .rows()
        .data()
        .toArray();

    const datosDtR2 = dtDiasTipicosR2
        .rows()
        .data()
        .toArray();

    const datosDtA1 = dtDiasTipicosA1
        .rows()
        .data()
        .toArray();

    const datosDtA2 = dtDiasTipicosA2
        .rows()
        .data()
        .toArray();

    // -----------------------------------------------------------

    const datosDtDpR1 = dtDiasTipicosDpR1
        .rows()
        .data()
        .toArray();

    const datosDtDpR2 = dtDiasTipicosDpR2
        .rows()
        .data()
        .toArray();

    const datosDtDpA1 = dtDiasTipicosDpA1
        .rows()
        .data()
        .toArray();

    const datosDtDpA2 = dtDiasTipicosDpA2
        .rows()
        .data()
        .toArray();


    $.ajax({
        type: 'POST',
        url: controller + 'EditarCaso',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            id: id,
            nombre: $('#pop-caso-nombre').val(),
            areaOperativa: $('#pop-caso-area').val(),

            formScadaDatMae: $('#pop-caso-maes-data').val(),
            fecIniDatMae: $('#pop-caso-maes-fecini').val(),
            fecFinDatMae: $('#pop-caso-maes-fecfin').val(),
            listFuncionesDataMaestra: datosFuncDm,

            formScadaDatPro: $('#pop-caso-proc-data').val(),
            fecIniDatPro: $('#pop-caso-proc-fecini').val(),
            fecFinDatPro: $('#pop-caso-proc-fecfin').val(),
            listFuncionesDataProcesar: datosFuncDp,

            // -------------------------------------------------------------------------

            listDiasTipicosR1: datosDtR1,
            fecIniR1: $('#pop-txtDesde-r1').val(),
            fecFinR1: $('#pop-txtHasta-r1').val(),

            listDiasTipicosR2: datosDtR2,

            numToleranciaRampaF1: $('#pop-txtToleraciaRampa-f1').val(),
            numFactorKF2: $('#pop-txtFactorK-f2').val(),

            anioFeriadoA1: $('#pop-txtFechaAnio-a1').val(),
            idFeriadoA1: $('#pop-filtro-feriado-a1').val(),
            listDiasTipicosA1: datosDtA1,

            anioFeriadoA2: $('#pop-txtFechaAnio-a2').val(),
            idFeriadoA2: $('#pop-filtro-feriado-a2').val(),
            listDiasTipicosA2: datosDtA2,

            // -------------------------------------------------------------------------

            listDiasTipicosDpR1: datosDtDpR1,
            fecIniDpR1: $('#pop-txtDesde-dp-r1').val(),
            fecFinDpR1: $('#pop-txtHasta-dp-r1').val(),

            listDiasTipicosDpR2: datosDtDpR2,

            numToleranciaRampaDpF1: $('#pop-txtToleraciaRampa-dp-f1').val(),
            numFactorKDpF2: $('#pop-txtFactorK-dp-f2').val(),

            /*anioFeriadoDpA1: $('#pop-txtFechaAnio-dp-a1').val(),*/
            /*idFeriadoDpA1: $('#pop-filtro-feriado-dp-a1').val(),*/
            anioFeriadoDpA1: 0,
            idFeriadoDpA1: 0,

            listDiasTipicosDpA1: datosDtDpA1,

            /*anioFeriadoA2: $('#pop-txtFechaAnio-dp-a2').val(),*/
            /*idFeriadoA2: $('#pop-filtro-feriado-dp-a2').val(),*/
            anioFeriadoDpA2: 0,
            idFeriadoDpA2: 0,

            listDiasTipicosDpA2: datosDtDpA2,

        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            // Llena la grilla de casos de prueba
            dt.clear();
            dt.rows.add(result.ListaCasos);
            dt.draw();

            // Establece en CANCELAR a la modificación de parametros del caso de condiguración
            tempDataR1 = "CANCELAR";
            tempDataR2 = "CANCELAR";
            tempDataF1 = "CANCELAR";
            tempDataF2 = "CANCELAR";
            tempDataA1 = "CANCELAR";
            tempDataA2 = "CANCELAR";

            tempDataDpR1 = "CANCELAR";
            tempDataDpR2 = "CANCELAR";
            tempDataDpF1 = "CANCELAR";
            tempDataDpF2 = "CANCELAR";
            tempDataDpA1 = "CANCELAR";
            tempDataDpA2 = "CANCELAR";

            alert(result.Mensaje);
            /*$("#pop-caso").bPopup().close();*/
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function eliminarCaso(id) {
    $.ajax({
        type: 'POST',
        url: controller + 'EliminarCaso',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            id: id,
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            if (result.Resultado === "-1") {
                alert("Ocurrio un error inesperado");
            } else {
                // Llena la grilla de casos de prueba
                dt.clear();
                dt.rows.add(result.ListaCasos);
                dt.draw();

                alert(result.Mensaje);
                $("#pop-eliminar").bPopup().close();
            }
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function copiarCaso(id) {
    $.ajax({
        type: 'POST',
        url: controller + 'CopiarCaso',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            id: id,
            nombre: $('#pop-copiar-nombre').val(),
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            if (result.Resultado === "-1") {
                alert("Ocurrio un error inesperado");
            } else {
                // Llena la grilla de casos de prueba
                dt.clear();
                dt.rows.add(result.ListaCasos);
                dt.draw();

                alert(result.Mensaje);
                $("#pop-copiar").bPopup().close();
            }
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function listarFormulas() {
    $.ajax({
        type: 'POST',
        url: controller + 'ListarFormulas',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            areaOperativa: $('#pop-caso-area').val(),
        }),
        datatype: 'json',
        traditional: true,
        success: function (model) {

            $("#pop-caso-maes-data").empty();
            $('#pop-caso-maes-data').append("<option value=''>-SELECCIONAR-</option>");
            $.each(model.ListaFormulas, function (k, v) {
                var option = '<option value =' + v.Prrucodi + '>' + v.Prruabrev + '</option>';
                $('#pop-caso-maes-data').append(option);
            })

            $("#pop-caso-proc-data").empty();
            $('#pop-caso-proc-data').append("<option value=''>-SELECCIONAR-</option>");
            $.each(model.ListaFormulas, function (k, v) {
                var option = '<option value =' + v.Prrucodi + '>' + v.Prruabrev + '</option>';
                $('#pop-caso-proc-data').append(option);
            })
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}


function obtenerDtCasos() {
    dt = $('#dt').DataTable({
        data: [],
        columns: [
            { title: '', data: null },
            { title: 'Nombre', data: 'Dpocsocnombre' },
            { title: 'Area Operativa', data: 'Areaabrev' },
            { title: 'Usuario Creación', data: 'Dpocsousucreacion' },
            { title: 'Fecha Creación', data: 'StrDpocsofeccreacion' },
            { title: 'Usuario última modificación', data: 'Dpocsousumodificacion' },
            { title: 'Fecha última modificación', data: 'StrDpocsousumodificacion' },
        ],
        columnDefs: [
            {
                targets: 0,
                defaultContent:
                    '<div class="dt-col-options">' +
                    `<img src="${imageRoot}btn-open.png" class="dt-ico-detalle" title="Ver detalle" />` +
                    `<img src="${imageRoot}btn-edit.png" class="dt-ico-editar" title="Editar registro" />` +
                    `<img src="${imageRoot}eliminar.png" class="dt-ico-eliminar" title="Eliminar registro" />` +
                    `<img src="${imageRoot}copiar.png" class="dt-ico-copiar" title="Copiar registro" />` +
                    '</div>',
            }
        ],
        createdRow: function (row, data, index) {

            $(row).find('.dt-ico-detalle').on('click', function () {
                // Reemplaza el título del popup
                $('#pop-caso-titulo').html('Detalle de caso de prueba');

                // Bloquea la edición del popup  
                $('.popup-text').addClass('disabled');
                $('#pop-caso-guardar').prop('disabled', true);

                // Establece en CANCELAR a la modificación de parametros del caso de condiguración
                tempDataR1 = "CANCELAR";
                tempDataR2 = "CANCELAR";
                tempDataF1 = "CANCELAR";
                tempDataF2 = "CANCELAR";
                tempDataA1 = "CANCELAR";
                tempDataA2 = "CANCELAR";

                tempDataDpR1 = "CANCELAR";
                tempDataDpR2 = "CANCELAR";
                tempDataDpF1 = "CANCELAR";
                tempDataDpF2 = "CANCELAR";
                tempDataDpA1 = "CANCELAR";
                tempDataDpA2 = "CANCELAR";

                // Muestra la ventana emergente
                popCaso = $('#pop-caso').bPopup({
                    easing: 'easeOutBack',
                    speed: 350,
                    transition: 'fadeIn',
                    modalClose: false,
                    onOpen: function () {
                        idCaso = data.Dpocsocodi;

                        document.getElementById("pop-caso-nombre").value = data.Dpocsocnombre;
                        document.getElementById("pop-caso-area").value = data.Areaabrev;

                        // Carga grilla de funciones de data maestra
                        listarFunciones(data.Dpocsocodi);
                    }
                });
            });

            $(row).find('.dt-ico-editar').on('click', function () {
                // Reemplaza el título del popup
                $('#pop-caso-titulo').html('Editar caso de prueba');

                // Bloquea la edición del popup  
                $('.popup-text').removeClass('disabled');
                $('#pop-caso-guardar').prop('disabled', false);

                // Establece en CANCELAR a la modificación de parametros del caso de condiguración
                tempDataR1 = "CANCELAR";
                tempDataR2 = "CANCELAR";
                tempDataF1 = "CANCELAR";
                tempDataF2 = "CANCELAR";
                tempDataA1 = "CANCELAR";
                tempDataA2 = "CANCELAR";

                tempDataDpR1 = "CANCELAR";
                tempDataDpR2 = "CANCELAR";
                tempDataDpF1 = "CANCELAR";
                tempDataDpF2 = "CANCELAR";
                tempDataDpA1 = "CANCELAR";
                tempDataDpA2 = "CANCELAR";

                // Muestra la ventana emergente
                popCaso = $('#pop-caso').bPopup({
                    easing: 'easeOutBack',
                    speed: 350,
                    transition: 'fadeIn',
                    modalClose: false,
                    onOpen: function () {
                        idCaso = data.Dpocsocodi;

                        document.getElementById("pop-caso-nombre").value = data.Dpocsocnombre;
                        document.getElementById("pop-caso-area").value = data.Areaabrev;

                        // Carga grilla de funciones de data a procesar
                        listarFunciones(data.Dpocsocodi);
                    }
                });
            });

            $(row).find('.dt-ico-eliminar').on('click', function () {
                popEliminar = $('#pop-eliminar').bPopup({
                    easing: 'easeOutBack',
                    speed: 350,
                    transition: 'fadeIn',
                    modalClose: false,
                    onOpen: function () {
                        idCaso = data.Dpocsocodi;
                    }
                });
            });

            $(row).find('.dt-ico-copiar').on('click', function () {
                popCopiar = $('#pop-copiar').bPopup({
                    easing: 'easeOutBack',
                    speed: 350,
                    transition: 'fadeIn',
                    modalClose: false,
                    onOpen: function () {
                        idCaso = data.Dpocsocodi;
                    }
                });
            });
        },
        searching: false,
        bLengthChange: false,
        bSort: false,
        destroy: true,
        paging: true,
        pageLength: 20,
        info: false,
    });
}


function obtenerDtFuncionesDataMaestra(datos) {
    dtFuncDm = $('#dtFuncDm').DataTable({
        data: datos,
        columns: [
            { title: '', data: 'Dposecuencma' },
            { title: 'Id', data: 'Dpofnccodima' },
            { title: 'Nombre', data: 'Dpodesfundm' },
            { title: '', data: null },
        ],
        columnDefs: [
            {
                targets: 0,

                render: function (data, type, row) {
                    // Devuelve un elemento HTML de tipo select con opciones predefinidas
                    return '<select class="cb-combo-sec-dm"><option value="0">-</option><option value="1">1</option><option value="2">2</option><option value="3">3</option><option value="4">4</option><option value="5">5</option></select>';
                }

                //createdCell: function (td, cellData, rowData, row, col) {
                //    var str = $('<select class="cb-combo-sec-dm">');
                //    str.append('<option value="0">-</option><option value="1">1</option><option value="2">2</option><option value="3">3</option><option value="4">4</option><option value="5">5</option>')
                //    str.append('</select>')
                //    $(td).html(str);

                //    return str;
                //}
            },
            {
                targets: 1,
                visible: false,
            },
            {
                targets: 3,
                defaultContent:
                    '<div class="dt-col-options">' +
                    `<img src="${imageRoot}copiar.png" class="dt-ico-editar" title="Configurar parametros" />` +
                    '</div>',
            }
        ],
        createdRow: function (row, data, index) {
            const cboDm = $(row).find('.cb-combo-sec-dm').children();
            cboDm.each((x, item) => {
                if (item.value === data.Dposecuencma) {
                    $(item).prop('selected', true);
                }
            });

            $(row).find('.cb-combo-sec-dm').on('change', function () {
                const secuencia = $(this).val();
                data.Dposecuencma = secuencia;
            });
        },
        searching: false,
        bLengthChange: false,
        bSort: false,
        destroy: true,
        paging: false,
        pageLength: 20,
        info: false,
    });
}

function obtenerDtFuncionesDataProcesar(datos) {
    dtFuncDp = $('#dtFuncDp').DataTable({
        data: datos,
        columns: [
            { title: '', data: 'Dposecuencpr' },
            { title: 'Id', data: 'Dpofnccodipr' },
            { title: 'Nombre', data: 'Dpodesfunpr' },
            { title: '', data: null },
        ],
        columnDefs: [
            {
                targets: 0,
                render: function (data, type, row) {
                    // Devuelve un elemento HTML de tipo select con opciones predefinidas
                    return '<select class="cb-combo-sec-pr"><option value="0">-</option><option value="1">1</option><option value="2">2</option><option value="3">3</option><option value="4">4</option><option value="5">5</option><option value="6">6</option></select>';
                }
            },
            {
                targets: 1,
                visible: false,
            },
            {
                targets: 3,
                defaultContent:
                    '<div class="dt-col-options">' +
                    `<img src="${imageRoot}copiar.png" class="dt-ico-editar" title="Configurar parametros" />` +
                    '</div>',
            }
        ],
        createdRow: function (row, data, index) {
            const cboPr = $(row).find('.cb-combo-sec-pr').children();
            cboPr.each((x, item) => {
                if (item.value === data.Dposecuencpr) {
                    $(item).prop('selected', true);
                }
            });

            $(row).find('.cb-combo-sec-pr').on('click', function () {
                const secuencia = this.value;
                data.Dposecuencpr = secuencia;
            });
        },
        searching: false,
        bLengthChange: false,
        bSort: false,
        destroy: true,
        paging: false,
        pageLength: 20,
        info: false,
    });
}


function obtenerDtDiasTipicosR1() {
    dtDiasTipicosR1 = $('#dtDiasTipicosR1').DataTable({
        data: diasTipicos,
        columns: [
            { title: 'Lunes', data: 'Pafunr1dtg1' },
            { title: 'Martes', data: 'Pafunr1dtg2' },
            { title: 'Miercoles', data: 'Pafunr1dtg3' },
            { title: 'Jueves', data: 'Pafunr1dtg4' },
            { title: 'Viernes', data: 'Pafunr1dtg5' },
            { title: 'Sabado', data: 'Pafunr1dtg6' },
            { title: 'Domingo', data: 'Pafunr1dtg7' },
        ],
        columnDefs: [
            {
                targets: 0,
                render: function (data, type, row) {
                    // Devuelve un elemento HTML de tipo select con opciones predefinidas
                    return '<select class="cb-combo-parr1-dt1"><option value="0">-</option><option value="1">G1</option><option value="2">G2</option><option value="3">G3</option><option value="4">G4</option><option value="5">G5</option><option value="6">G6</option><option value="7">G7</option></select>';
                }
            },
            {
                targets: 1,
                render: function (data, type, row) {
                    // Devuelve un elemento HTML de tipo select con opciones predefinidas
                    return '<select class="cb-combo-parr1-dt2"><option value="0">-</option><option value="1">G1</option><option value="2">G2</option><option value="3">G3</option><option value="4">G4</option><option value="5">G5</option><option value="6">G6</option><option value="7">G7</option></select>';
                }
            },
            {
                targets: 2,
                render: function (data, type, row) {
                    // Devuelve un elemento HTML de tipo select con opciones predefinidas
                    return '<select class="cb-combo-parr1-dt3"><option value="0">-</option><option value="1">G1</option><option value="2">G2</option><option value="3">G3</option><option value="4">G4</option><option value="5">G5</option><option value="6">G6</option><option value="7">G7</option></select>';
                }
            },
            {
                targets: 3,
                render: function (data, type, row) {
                    // Devuelve un elemento HTML de tipo select con opciones predefinidas
                    return '<select class="cb-combo-parr1-dt4"><option value="0">-</option><option value="1">G1</option><option value="2">G2</option><option value="3">G3</option><option value="4">G4</option><option value="5">G5</option><option value="6">G6</option><option value="7">G7</option></select>';
                }
            },
            {
                targets: 4,
                render: function (data, type, row) {
                    // Devuelve un elemento HTML de tipo select con opciones predefinidas
                    return '<select class="cb-combo-parr1-dt5"><option value="0">-</option><option value="1">G1</option><option value="2">G2</option><option value="3">G3</option><option value="4">G4</option><option value="5">G5</option><option value="6">G6</option><option value="7">G7</option></select>';
                }
            },
            {
                targets: 5,
                render: function (data, type, row) {
                    // Devuelve un elemento HTML de tipo select con opciones predefinidas
                    return '<select class="cb-combo-parr1-dt6"><option value="0">-</option><option value="1">G1</option><option value="2">G2</option><option value="3">G3</option><option value="4">G4</option><option value="5">G5</option><option value="6">G6</option><option value="7">G7</option></select>';
                }
            },
            {
                targets: 6,
                render: function (data, type, row) {
                    // Devuelve un elemento HTML de tipo select con opciones predefinidas
                    return '<select class="cb-combo-parr1-dt7"><option value="0">-</option><option value="1">G1</option><option value="2">G2</option><option value="3">G3</option><option value="4">G4</option><option value="5">G5</option><option value="6">G6</option><option value="7">G7</option></select>';
                }
            }
        ],
        createdRow: function (row, data, index) {

            const cboDt1 = $(row).find('.cb-combo-parr1-dt1').children();
            cboDt1.each((x, item) => {
                if (item.value === data.Pafunr1dtg1) {
                    $(item).prop('selected', true);
                }
            });

            $(row).find('.cb-combo-parr1-dt1').on('click', function () {
                const diaTipico1 = this.value;
                data.Pafunr1dtg1 = diaTipico1;
            });

            const cboDt2 = $(row).find('.cb-combo-parr1-dt2').children();
            cboDt2.each((x, item) => {
                if (item.value === data.Pafunr1dtg2) {
                    $(item).prop('selected', true);
                }
            });

            $(row).find('.cb-combo-parr1-dt2').on('click', function () {
                const diaTipico2 = this.value;
                data.Pafunr1dtg2 = diaTipico2;
            });

            const cboDt3 = $(row).find('.cb-combo-parr1-dt3').children();
            cboDt3.each((x, item) => {
                if (item.value === data.Pafunr1dtg3) {
                    $(item).prop('selected', true);
                }
            });

            $(row).find('.cb-combo-parr1-dt3').on('click', function () {
                const diaTipico3 = this.value;
                data.Pafunr1dtg3 = diaTipico3;
            });

            const cboDt4 = $(row).find('.cb-combo-parr1-dt4').children();
            cboDt4.each((x, item) => {
                if (item.value === data.Pafunr1dtg4) {
                    $(item).prop('selected', true);
                }
            });

            $(row).find('.cb-combo-parr1-dt4').on('click', function () {
                const diaTipico4 = this.value;
                data.Pafunr1dtg4 = diaTipico4;
            });

            const cboDt5 = $(row).find('.cb-combo-parr1-dt5').children();
            cboDt5.each((x, item) => {
                if (item.value === data.Pafunr1dtg5) {
                    $(item).prop('selected', true);
                }
            });

            $(row).find('.cb-combo-parr1-dt5').on('click', function () {
                const diaTipico5 = this.value;
                data.Pafunr1dtg5 = diaTipico5;
            });

            const cboDt6 = $(row).find('.cb-combo-parr1-dt6').children();
            cboDt6.each((x, item) => {
                if (item.value === data.Pafunr1dtg6) {
                    $(item).prop('selected', true);
                }
            });

            $(row).find('.cb-combo-parr1-dt6').on('click', function () {
                const diaTipico6 = this.value;
                data.Pafunr1dtg6 = diaTipico6;
            });

            const cboDt7 = $(row).find('.cb-combo-parr1-dt7').children();
            cboDt7.each((x, item) => {
                if (item.value === data.Pafunr1dtg7) {
                    $(item).prop('selected', true);
                }
            });

            $(row).find('.cb-combo-parr1-dt7').on('click', function () {
                const diaTipico7 = this.value;
                data.Pafunr1dtg7 = diaTipico7;
            });
        },
        searching: false,
        bLengthChange: false,
        bSort: false,
        destroy: true,
        paging: false,
        pageLength: 20,
        info: false,
    });
}

function obtenerDtDiasTipicosR2() {
    dtDiasTipicosR2 = $('#dtDiasTipicosR2').DataTable({
        data: diasTipicos,
        columns: [
            { title: 'Lunes', data: 'Pafunr2dtg1' },
            { title: 'Martes', data: 'Pafunr2dtg2' },
            { title: 'Miercoles', data: 'Pafunr2dtg3' },
            { title: 'Jueves', data: 'Pafunr2dtg4' },
            { title: 'Viernes', data: 'Pafunr2dtg5' },
            { title: 'Sabado', data: 'Pafunr2dtg6' },
            { title: 'Domingo', data: 'Pafunr2dtg7' },
        ],
        columnDefs: [
            {
                targets: 0,
                render: function (data, type, row) {
                    // Devuelve un elemento HTML de tipo select con opciones predefinidas
                    return '<select class="cb-combo-parr2-dt1"><option value="0">-</option><option value="1">G1</option><option value="2">G2</option><option value="3">G3</option><option value="4">G4</option><option value="5">G5</option><option value="6">G6</option><option value="7">G7</option></select>';
                }
            },
            {
                targets: 1,
                render: function (data, type, row) {
                    // Devuelve un elemento HTML de tipo select con opciones predefinidas
                    return '<select class="cb-combo-parr2-dt2"><option value="0">-</option><option value="1">G1</option><option value="2">G2</option><option value="3">G3</option><option value="4">G4</option><option value="5">G5</option><option value="6">G6</option><option value="7">G7</option></select>';
                }
            },
            {
                targets: 2,
                render: function (data, type, row) {
                    // Devuelve un elemento HTML de tipo select con opciones predefinidas
                    return '<select class="cb-combo-parr2-dt3"><option value="0">-</option><option value="1">G1</option><option value="2">G2</option><option value="3">G3</option><option value="4">G4</option><option value="5">G5</option><option value="6">G6</option><option value="7">G7</option></select>';
                }
            },
            {
                targets: 3,
                render: function (data, type, row) {
                    // Devuelve un elemento HTML de tipo select con opciones predefinidas
                    return '<select class="cb-combo-parr2-dt4"><option value="0">-</option><option value="1">G1</option><option value="2">G2</option><option value="3">G3</option><option value="4">G4</option><option value="5">G5</option><option value="6">G6</option><option value="7">G7</option></select>';
                }
            },
            {
                targets: 4,
                render: function (data, type, row) {
                    // Devuelve un elemento HTML de tipo select con opciones predefinidas
                    return '<select class="cb-combo-parr2-dt5"><option value="0">-</option><option value="1">G1</option><option value="2">G2</option><option value="3">G3</option><option value="4">G4</option><option value="5">G5</option><option value="6">G6</option><option value="7">G7</option></select>';
                }
            },
            {
                targets: 5,
                render: function (data, type, row) {
                    // Devuelve un elemento HTML de tipo select con opciones predefinidas
                    return '<select class="cb-combo-parr2-dt6"><option value="0">-</option><option value="1">G1</option><option value="2">G2</option><option value="3">G3</option><option value="4">G4</option><option value="5">G5</option><option value="6">G6</option><option value="7">G7</option></select>';
                }
            },
            {
                targets: 6,
                render: function (data, type, row) {
                    // Devuelve un elemento HTML de tipo select con opciones predefinidas
                    return '<select class="cb-combo-parr2-dt7"><option value="0">-</option><option value="1">G1</option><option value="2">G2</option><option value="3">G3</option><option value="4">G4</option><option value="5">G5</option><option value="6">G6</option><option value="7">G7</option></select>';
                }
            }
        ],
        createdRow: function (row, data, index) {

            const cboDt1 = $(row).find('.cb-combo-parr2-dt1').children();
            cboDt1.each((x, item) => {
                if (item.value === data.Pafunr2dtg1) {
                    $(item).prop('selected', true);
                }
            });

            $(row).find('.cb-combo-parr2-dt1').on('click', function () {
                const diaTipico1 = this.value;
                data.Pafunr2dtg1 = diaTipico1;
            });

            const cboDt2 = $(row).find('.cb-combo-parr2-dt2').children();
            cboDt2.each((x, item) => {
                if (item.value === data.Pafunr2dtg2) {
                    $(item).prop('selected', true);
                }
            });

            $(row).find('.cb-combo-parr2-dt2').on('click', function () {
                const diaTipico2 = this.value;
                data.Pafunr2dtg2 = diaTipico2;
            });

            const cboDt3 = $(row).find('.cb-combo-parr2-dt3').children();
            cboDt3.each((x, item) => {
                if (item.value === data.Pafunr2dtg3) {
                    $(item).prop('selected', true);
                }
            });

            $(row).find('.cb-combo-parr2-dt3').on('click', function () {
                const diaTipico3 = this.value;
                data.Pafunr2dtg3 = diaTipico3;
            });

            const cboDt4 = $(row).find('.cb-combo-parr2-dt4').children();
            cboDt4.each((x, item) => {
                if (item.value === data.Pafunr2dtg4) {
                    $(item).prop('selected', true);
                }
            });

            $(row).find('.cb-combo-parr2-dt4').on('click', function () {
                const diaTipico4 = this.value;
                data.Pafunr2dtg4 = diaTipico4;
            });

            const cboDt5 = $(row).find('.cb-combo-parr2-dt5').children();
            cboDt5.each((x, item) => {
                if (item.value === data.Pafunr2dtg5) {
                    $(item).prop('selected', true);
                }
            });

            $(row).find('.cb-combo-parr2-dt5').on('click', function () {
                const diaTipico5 = this.value;
                data.Pafunr2dtg5 = diaTipico5;
            });

            const cboDt6 = $(row).find('.cb-combo-parr2-dt6').children();
            cboDt6.each((x, item) => {
                if (item.value === data.Pafunr2dtg6) {
                    $(item).prop('selected', true);
                }
            });

            $(row).find('.cb-combo-parr2-dt6').on('click', function () {
                const diaTipico6 = this.value;
                data.Pafunr2dtg6 = diaTipico6;
            });

            const cboDt7 = $(row).find('.cb-combo-parr2-dt7').children();
            cboDt7.each((x, item) => {
                if (item.value === data.Pafunr2dtg7) {
                    $(item).prop('selected', true);
                }
            });

            $(row).find('.cb-combo-parr2-dt7').on('click', function () {
                const diaTipico7 = this.value;
                data.Pafunr2dtg7 = diaTipico7;
            });
        },
        searching: false,
        bLengthChange: false,
        bSort: false,
        destroy: true,
        paging: false,
        pageLength: 20,
        info: false,
    });
}

function obtenerDtDiasTipicosA1() {
    dtDiasTipicosA1 = $('#dtDiasTipicosA1').DataTable({
        data: diasTipicos,
        columns: [
            { title: 'Lunes', data: 'Pafuna1dtg1' },
            { title: 'Martes', data: 'Pafuna1dtg2' },
            { title: 'Miercoles', data: 'Pafuna1dtg3' },
            { title: 'Jueves', data: 'Pafuna1dtg4' },
            { title: 'Viernes', data: 'Pafuna1dtg5' },
            { title: 'Sabado', data: 'Pafuna1dtg6' },
            { title: 'Domingo', data: 'Pafuna1dtg7' },
        ],
        columnDefs: [
            {
                targets: 0,
                render: function (data, type, row) {
                    // Devuelve un elemento HTML de tipo select con opciones predefinidas
                    return '<select class="cb-combo-para1-dt1"><option value="0">-</option><option value="1">G1</option><option value="2">G2</option><option value="3">G3</option><option value="4">G4</option><option value="5">G5</option><option value="6">G6</option><option value="7">G7</option></select>';
                }
            },
            {
                targets: 1,
                render: function (data, type, row) {
                    // Devuelve un elemento HTML de tipo select con opciones predefinidas
                    return '<select class="cb-combo-para1-dt2"><option value="0">-</option><option value="1">G1</option><option value="2">G2</option><option value="3">G3</option><option value="4">G4</option><option value="5">G5</option><option value="6">G6</option><option value="7">G7</option></select>';
                }
            },
            {
                targets: 2,
                render: function (data, type, row) {
                    // Devuelve un elemento HTML de tipo select con opciones predefinidas
                    return '<select class="cb-combo-para1-dt3"><option value="0">-</option><option value="1">G1</option><option value="2">G2</option><option value="3">G3</option><option value="4">G4</option><option value="5">G5</option><option value="6">G6</option><option value="7">G7</option></select>';
                }
            },
            {
                targets: 3,
                render: function (data, type, row) {
                    // Devuelve un elemento HTML de tipo select con opciones predefinidas
                    return '<select class="cb-combo-para1-dt4"><option value="0">-</option><option value="1">G1</option><option value="2">G2</option><option value="3">G3</option><option value="4">G4</option><option value="5">G5</option><option value="6">G6</option><option value="7">G7</option></select>';
                }
            },
            {
                targets: 4,
                render: function (data, type, row) {
                    // Devuelve un elemento HTML de tipo select con opciones predefinidas
                    return '<select class="cb-combo-para1-dt5"><option value="0">-</option><option value="1">G1</option><option value="2">G2</option><option value="3">G3</option><option value="4">G4</option><option value="5">G5</option><option value="6">G6</option><option value="7">G7</option></select>';
                }
            },
            {
                targets: 5,
                render: function (data, type, row) {
                    // Devuelve un elemento HTML de tipo select con opciones predefinidas
                    return '<select class="cb-combo-para1-dt6"><option value="0">-</option><option value="1">G1</option><option value="2">G2</option><option value="3">G3</option><option value="4">G4</option><option value="5">G5</option><option value="6">G6</option><option value="7">G7</option></select>';
                }
            },
            {
                targets: 6,
                render: function (data, type, row) {
                    // Devuelve un elemento HTML de tipo select con opciones predefinidas
                    return '<select class="cb-combo-para1-dt7"><option value="0">-</option><option value="1">G1</option><option value="2">G2</option><option value="3">G3</option><option value="4">G4</option><option value="5">G5</option><option value="6">G6</option><option value="7">G7</option></select>';
                }
            }
        ],
        createdRow: function (row, data, index) {
            const cboDt1 = $(row).find('.cb-combo-para1-dt1').children();
            cboDt1.each((x, item) => {
                if (item.value === data.Pafuna1dtg1) {
                    $(item).prop('selected', true);
                }
            });

            $(row).find('.cb-combo-para1-dt1').on('click', function () {
                const diaTipico1 = this.value;
                data.Pafuna1dtg1 = diaTipico1;
            });

            const cboDt2 = $(row).find('.cb-combo-para1-dt2').children();
            cboDt2.each((x, item) => {
                if (item.value === data.Pafuna1dtg2) {
                    $(item).prop('selected', true);
                }
            });

            $(row).find('.cb-combo-para1-dt2').on('click', function () {
                const diaTipico2 = this.value;
                data.Pafuna1dtg2 = diaTipico2;
            });

            const cboDt3 = $(row).find('.cb-combo-para1-dt3').children();
            cboDt3.each((x, item) => {
                if (item.value === data.Pafuna1dtg3) {
                    $(item).prop('selected', true);
                }
            });

            $(row).find('.cb-combo-para1-dt3').on('click', function () {
                const diaTipico3 = this.value;
                data.Pafuna1dtg3 = diaTipico3;
            });

            const cboDt4 = $(row).find('.cb-combo-para1-dt4').children();
            cboDt4.each((x, item) => {
                if (item.value === data.Pafuna1dtg4) {
                    $(item).prop('selected', true);
                }
            });

            $(row).find('.cb-combo-para1-dt4').on('click', function () {
                const diaTipico4 = this.value;
                data.Pafuna1dtg4 = diaTipico4;
            });

            const cboDt5 = $(row).find('.cb-combo-para1-dt5').children();
            cboDt5.each((x, item) => {
                if (item.value === data.Pafuna1dtg5) {
                    $(item).prop('selected', true);
                }
            });

            $(row).find('.cb-combo-para1-dt5').on('click', function () {
                const diaTipico5 = this.value;
                data.Pafuna1dtg5 = diaTipico5;
            });

            const cboDt6 = $(row).find('.cb-combo-para1-dt6').children();
            cboDt6.each((x, item) => {
                if (item.value === data.Pafuna1dtg6) {
                    $(item).prop('selected', true);
                }
            });

            $(row).find('.cb-combo-para1-dt6').on('click', function () {
                const diaTipico6 = this.value;
                data.Pafuna1dtg6 = diaTipico6;
            });

            const cboDt7 = $(row).find('.cb-combo-para1-dt7').children();
            cboDt7.each((x, item) => {
                if (item.value === data.Pafuna1dtg7) {
                    $(item).prop('selected', true);
                }
            });

            $(row).find('.cb-combo-para1-dt7').on('click', function () {
                const diaTipico7 = this.value;
                data.Pafuna1dtg7 = diaTipico7;
            });
        },
        searching: false,
        bLengthChange: false,
        bSort: false,
        destroy: true,
        paging: false,
        pageLength: 20,
        info: false,
    });
}

function obtenerDtDiasTipicosA2() {
    dtDiasTipicosA2 = $('#dtDiasTipicosA2').DataTable({
        data: diasTipicos,
        columns: [
            { title: 'Lunes', data: 'Pafuna2dtg1' },
            { title: 'Martes', data: 'Pafuna2dtg2' },
            { title: 'Miercoles', data: 'Pafuna2dtg3' },
            { title: 'Jueves', data: 'Pafuna2dtg4' },
            { title: 'Viernes', data: 'Pafuna2dtg5' },
            { title: 'Sabado', data: 'Pafuna2dtg6' },
            { title: 'Domingo', data: 'Pafuna2dtg7' },
        ],
        columnDefs: [
            {
                targets: 0,
                render: function (data, type, row) {
                    // Devuelve un elemento HTML de tipo select con opciones predefinidas
                    return '<select class="cb-combo-para2-dt1"><option value="0">-</option><option value="1">G1</option><option value="2">G2</option><option value="3">G3</option><option value="4">G4</option><option value="5">G5</option><option value="6">G6</option><option value="7">G7</option></select>';
                }
            },
            {
                targets: 1,
                render: function (data, type, row) {
                    // Devuelve un elemento HTML de tipo select con opciones predefinidas
                    return '<select class="cb-combo-para2-dt2"><option value="0">-</option><option value="1">G1</option><option value="2">G2</option><option value="3">G3</option><option value="4">G4</option><option value="5">G5</option><option value="6">G6</option><option value="7">G7</option></select>';
                }
            },
            {
                targets: 2,
                render: function (data, type, row) {
                    // Devuelve un elemento HTML de tipo select con opciones predefinidas
                    return '<select class="cb-combo-para2-dt3"><option value="0">-</option><option value="1">G1</option><option value="2">G2</option><option value="3">G3</option><option value="4">G4</option><option value="5">G5</option><option value="6">G6</option><option value="7">G7</option></select>';
                }
            },
            {
                targets: 3,
                render: function (data, type, row) {
                    // Devuelve un elemento HTML de tipo select con opciones predefinidas
                    return '<select class="cb-combo-para2-dt4"><option value="0">-</option><option value="1">G1</option><option value="2">G2</option><option value="3">G3</option><option value="4">G4</option><option value="5">G5</option><option value="6">G6</option><option value="7">G7</option></select>';
                }
            },
            {
                targets: 4,
                render: function (data, type, row) {
                    // Devuelve un elemento HTML de tipo select con opciones predefinidas
                    return '<select class="cb-combo-para2-dt5"><option value="0">-</option><option value="1">G1</option><option value="2">G2</option><option value="3">G3</option><option value="4">G4</option><option value="5">G5</option><option value="6">G6</option><option value="7">G7</option></select>';
                }
            },
            {
                targets: 5,
                render: function (data, type, row) {
                    // Devuelve un elemento HTML de tipo select con opciones predefinidas
                    return '<select class="cb-combo-para2-dt6"><option value="0">-</option><option value="1">G1</option><option value="2">G2</option><option value="3">G3</option><option value="4">G4</option><option value="5">G5</option><option value="6">G6</option><option value="7">G7</option></select>';
                }
            },
            {
                targets: 6,
                render: function (data, type, row) {
                    // Devuelve un elemento HTML de tipo select con opciones predefinidas
                    return '<select class="cb-combo-para2-dt7"><option value="0">-</option><option value="1">G1</option><option value="2">G2</option><option value="3">G3</option><option value="4">G4</option><option value="5">G5</option><option value="6">G6</option><option value="7">G7</option></select>';
                }
            }
        ],
        createdRow: function (row, data, index) {
            const cboDt1 = $(row).find('.cb-combo-para2-dt1').children();
            cboDt1.each((x, item) => {
                if (item.value === data.Pafuna2dtg1) {
                    $(item).prop('selected', true);
                }
            });

            $(row).find('.cb-combo-para2-dt1').on('click', function () {
                const diaTipico1 = this.value;
                data.Pafuna2dtg1 = diaTipico1;
            });

            const cboDt2 = $(row).find('.cb-combo-para2-dt2').children();
            cboDt2.each((x, item) => {
                if (item.value === data.Pafuna2dtg2) {
                    $(item).prop('selected', true);
                }
            });

            $(row).find('.cb-combo-para2-dt2').on('click', function () {
                const diaTipico2 = this.value;
                data.Pafuna2dtg2 = diaTipico2;
            });

            const cboDt3 = $(row).find('.cb-combo-para2-dt3').children();
            cboDt3.each((x, item) => {
                if (item.value === data.Pafuna2dtg3) {
                    $(item).prop('selected', true);
                }
            });

            $(row).find('.cb-combo-para2-dt3').on('click', function () {
                const diaTipico3 = this.value;
                data.Pafuna2dtg3 = diaTipico3;
            });

            const cboDt4 = $(row).find('.cb-combo-para2-dt4').children();
            cboDt4.each((x, item) => {
                if (item.value === data.Pafuna2dtg4) {
                    $(item).prop('selected', true);
                }
            });

            $(row).find('.cb-combo-para2-dt4').on('click', function () {
                const diaTipico4 = this.value;
                data.Pafuna2dtg4 = diaTipico4;
            });

            const cboDt5 = $(row).find('.cb-combo-para2-dt5').children();
            cboDt5.each((x, item) => {
                if (item.value === data.Pafuna2dtg5) {
                    $(item).prop('selected', true);
                }
            });

            $(row).find('.cb-combo-para2-dt5').on('click', function () {
                const diaTipico5 = this.value;
                data.Pafuna2dtg5 = diaTipico5;
            });

            const cboDt6 = $(row).find('.cb-combo-para2-dt6').children();
            cboDt6.each((x, item) => {
                if (item.value === data.Pafuna2dtg6) {
                    $(item).prop('selected', true);
                }
            });

            $(row).find('.cb-combo-para2-dt6').on('click', function () {
                const diaTipico6 = this.value;
                data.Pafuna2dtg6 = diaTipico6;
            });

            const cboDt7 = $(row).find('.cb-combo-para2-dt7').children();
            cboDt7.each((x, item) => {
                if (item.value === data.Pafuna2dtg7) {
                    $(item).prop('selected', true);
                }
            });

            $(row).find('.cb-combo-para2-dt7').on('click', function () {
                const diaTipico7 = this.value;
                data.Pafuna2dtg7 = diaTipico7;
            });
        },
        searching: false,
        bLengthChange: false,
        bSort: false,
        destroy: true,
        paging: false,
        pageLength: 20,
        info: false,
    });
}


function obtenerDtDiasTipicosDpR1() {
    dtDiasTipicosDpR1 = $('#dtDiasTipicosDpR1').DataTable({
        data: diasTipicos,
        columns: [
            { title: 'Lunes', data: 'Pafunr1dtg1' },
            { title: 'Martes', data: 'Pafunr1dtg2' },
            { title: 'Miercoles', data: 'Pafunr1dtg3' },
            { title: 'Jueves', data: 'Pafunr1dtg4' },
            { title: 'Viernes', data: 'Pafunr1dtg5' },
            { title: 'Sabado', data: 'Pafunr1dtg6' },
            { title: 'Domingo', data: 'Pafunr1dtg7' },
        ],
        columnDefs: [
            {
                targets: 0,
                render: function (data, type, row) {
                    // Devuelve un elemento HTML de tipo select con opciones predefinidas
                    return '<select class="cb-combo-parr1-dp-dt1"><option value="0">-</option><option value="1">G1</option><option value="2">G2</option><option value="3">G3</option><option value="4">G4</option><option value="5">G5</option><option value="6">G6</option><option value="7">G7</option></select>';
                }
            },
            {
                targets: 1,
                render: function (data, type, row) {
                    // Devuelve un elemento HTML de tipo select con opciones predefinidas
                    return '<select class="cb-combo-parr1-dp-dt2"><option value="0">-</option><option value="1">G1</option><option value="2">G2</option><option value="3">G3</option><option value="4">G4</option><option value="5">G5</option><option value="6">G6</option><option value="7">G7</option></select>';
                }
            },
            {
                targets: 2,
                render: function (data, type, row) {
                    // Devuelve un elemento HTML de tipo select con opciones predefinidas
                    return '<select class="cb-combo-parr1-dp-dt3"><option value="0">-</option><option value="1">G1</option><option value="2">G2</option><option value="3">G3</option><option value="4">G4</option><option value="5">G5</option><option value="6">G6</option><option value="7">G7</option></select>';
                }
            },
            {
                targets: 3,
                render: function (data, type, row) {
                    // Devuelve un elemento HTML de tipo select con opciones predefinidas
                    return '<select class="cb-combo-parr1-dp-dt4"><option value="0">-</option><option value="1">G1</option><option value="2">G2</option><option value="3">G3</option><option value="4">G4</option><option value="5">G5</option><option value="6">G6</option><option value="7">G7</option></select>';
                }
            },
            {
                targets: 4,
                render: function (data, type, row) {
                    // Devuelve un elemento HTML de tipo select con opciones predefinidas
                    return '<select class="cb-combo-parr1-dp-dt5"><option value="0">-</option><option value="1">G1</option><option value="2">G2</option><option value="3">G3</option><option value="4">G4</option><option value="5">G5</option><option value="6">G6</option><option value="7">G7</option></select>';
                }
            },
            {
                targets: 5,
                render: function (data, type, row) {
                    // Devuelve un elemento HTML de tipo select con opciones predefinidas
                    return '<select class="cb-combo-parr1-dp-dt6"><option value="0">-</option><option value="1">G1</option><option value="2">G2</option><option value="3">G3</option><option value="4">G4</option><option value="5">G5</option><option value="6">G6</option><option value="7">G7</option></select>';
                }
            },
            {
                targets: 6,
                render: function (data, type, row) {
                    // Devuelve un elemento HTML de tipo select con opciones predefinidas
                    return '<select class="cb-combo-parr1-dp-dt7"><option value="0">-</option><option value="1">G1</option><option value="2">G2</option><option value="3">G3</option><option value="4">G4</option><option value="5">G5</option><option value="6">G6</option><option value="7">G7</option></select>';
                }
            }
        ],
        createdRow: function (row, data, index) {

            const cboDt1 = $(row).find('.cb-combo-parr1-dp-dt1').children();
            cboDt1.each((x, item) => {
                if (item.value === data.Pafunr1dtg1) {
                    $(item).prop('selected', true);
                }
            });

            $(row).find('.cb-combo-parr1-dp-dt1').on('click', function () {
                const diaTipico1 = this.value;
                data.Pafunr1dtg1 = diaTipico1;
            });

            const cboDt2 = $(row).find('.cb-combo-parr1-dp-dt2').children();
            cboDt2.each((x, item) => {
                if (item.value === data.Pafunr1dtg2) {
                    $(item).prop('selected', true);
                }
            });

            $(row).find('.cb-combo-parr1-dp-dt2').on('click', function () {
                const diaTipico2 = this.value;
                data.Pafunr1dtg2 = diaTipico2;
            });

            const cboDt3 = $(row).find('.cb-combo-parr1-dp-dt3').children();
            cboDt3.each((x, item) => {
                if (item.value === data.Pafunr1dtg3) {
                    $(item).prop('selected', true);
                }
            });

            $(row).find('.cb-combo-parr1-dp-dt3').on('click', function () {
                const diaTipico3 = this.value;
                data.Pafunr1dtg3 = diaTipico3;
            });

            const cboDt4 = $(row).find('.cb-combo-parr1-dp-dt4').children();
            cboDt4.each((x, item) => {
                if (item.value === data.Pafunr1dtg4) {
                    $(item).prop('selected', true);
                }
            });

            $(row).find('.cb-combo-parr1-dp-dt4').on('click', function () {
                const diaTipico4 = this.value;
                data.Pafunr1dtg4 = diaTipico4;
            });

            const cboDt5 = $(row).find('.cb-combo-parr1-dp-dt5').children();
            cboDt5.each((x, item) => {
                if (item.value === data.Pafunr1dtg5) {
                    $(item).prop('selected', true);
                }
            });

            $(row).find('.cb-combo-parr1-dp-dt5').on('click', function () {
                const diaTipico5 = this.value;
                data.Pafunr1dtg5 = diaTipico5;
            });

            const cboDt6 = $(row).find('.cb-combo-parr1-dp-dt6').children();
            cboDt6.each((x, item) => {
                if (item.value === data.Pafunr1dtg6) {
                    $(item).prop('selected', true);
                }
            });

            $(row).find('.cb-combo-parr1-dp-dt6').on('click', function () {
                const diaTipico6 = this.value;
                data.Pafunr1dtg6 = diaTipico6;
            });

            const cboDt7 = $(row).find('.cb-combo-parr1-dp-dt7').children();
            cboDt7.each((x, item) => {
                if (item.value === data.Pafunr1dtg7) {
                    $(item).prop('selected', true);
                }
            });

            $(row).find('.cb-combo-parr1-dp-dt7').on('click', function () {
                const diaTipico7 = this.value;
                data.Pafunr1dtg7 = diaTipico7;
            });
        },
        searching: false,
        bLengthChange: false,
        bSort: false,
        destroy: true,
        paging: false,
        pageLength: 20,
        info: false,
    });
}

function obtenerDtDiasTipicosDpR2() {
    dtDiasTipicosDpR2 = $('#dtDiasTipicosDpR2').DataTable({
        data: diasTipicos,
        columns: [
            { title: 'Lunes', data: 'Pafunr2dtg1' },
            { title: 'Martes', data: 'Pafunr2dtg2' },
            { title: 'Miercoles', data: 'Pafunr2dtg3' },
            { title: 'Jueves', data: 'Pafunr2dtg4' },
            { title: 'Viernes', data: 'Pafunr2dtg5' },
            { title: 'Sabado', data: 'Pafunr2dtg6' },
            { title: 'Domingo', data: 'Pafunr2dtg7' },
        ],
        columnDefs: [
            {
                targets: 0,
                render: function (data, type, row) {
                    // Devuelve un elemento HTML de tipo select con opciones predefinidas
                    return '<select class="cb-combo-parr2-dp-dt1"><option value="0">-</option><option value="1">G1</option><option value="2">G2</option><option value="3">G3</option><option value="4">G4</option><option value="5">G5</option><option value="6">G6</option><option value="7">G7</option></select>';
                }
            },
            {
                targets: 1,
                render: function (data, type, row) {
                    // Devuelve un elemento HTML de tipo select con opciones predefinidas
                    return '<select class="cb-combo-parr2-dp-dt2"><option value="0">-</option><option value="1">G1</option><option value="2">G2</option><option value="3">G3</option><option value="4">G4</option><option value="5">G5</option><option value="6">G6</option><option value="7">G7</option></select>';
                }
            },
            {
                targets: 2,
                render: function (data, type, row) {
                    // Devuelve un elemento HTML de tipo select con opciones predefinidas
                    return '<select class="cb-combo-parr2-dp-dt3"><option value="0">-</option><option value="1">G1</option><option value="2">G2</option><option value="3">G3</option><option value="4">G4</option><option value="5">G5</option><option value="6">G6</option><option value="7">G7</option></select>';
                }
            },
            {
                targets: 3,
                render: function (data, type, row) {
                    // Devuelve un elemento HTML de tipo select con opciones predefinidas
                    return '<select class="cb-combo-parr2-dp-dt4"><option value="0">-</option><option value="1">G1</option><option value="2">G2</option><option value="3">G3</option><option value="4">G4</option><option value="5">G5</option><option value="6">G6</option><option value="7">G7</option></select>';
                }
            },
            {
                targets: 4,
                render: function (data, type, row) {
                    // Devuelve un elemento HTML de tipo select con opciones predefinidas
                    return '<select class="cb-combo-parr2-dp-dt5"><option value="0">-</option><option value="1">G1</option><option value="2">G2</option><option value="3">G3</option><option value="4">G4</option><option value="5">G5</option><option value="6">G6</option><option value="7">G7</option></select>';
                }
            },
            {
                targets: 5,
                render: function (data, type, row) {
                    // Devuelve un elemento HTML de tipo select con opciones predefinidas
                    return '<select class="cb-combo-parr2-dp-dt6"><option value="0">-</option><option value="1">G1</option><option value="2">G2</option><option value="3">G3</option><option value="4">G4</option><option value="5">G5</option><option value="6">G6</option><option value="7">G7</option></select>';
                }
            },
            {
                targets: 6,
                render: function (data, type, row) {
                    // Devuelve un elemento HTML de tipo select con opciones predefinidas
                    return '<select class="cb-combo-parr2-dp-dt7"><option value="0">-</option><option value="1">G1</option><option value="2">G2</option><option value="3">G3</option><option value="4">G4</option><option value="5">G5</option><option value="6">G6</option><option value="7">G7</option></select>';
                }
            }
        ],
        createdRow: function (row, data, index) {

            const cboDt1 = $(row).find('.cb-combo-parr2-dp-dt1').children();
            cboDt1.each((x, item) => {
                if (item.value === data.Pafunr2dtg1) {
                    $(item).prop('selected', true);
                }
            });

            $(row).find('.cb-combo-parr2-dp-dt1').on('click', function () {
                const diaTipico1 = this.value;
                data.Pafunr2dtg1 = diaTipico1;
            });

            const cboDt2 = $(row).find('.cb-combo-parr2-dp-dt2').children();
            cboDt2.each((x, item) => {
                if (item.value === data.Pafunr2dtg2) {
                    $(item).prop('selected', true);
                }
            });

            $(row).find('.cb-combo-parr2-dp-dt2').on('click', function () {
                const diaTipico2 = this.value;
                data.Pafunr2dtg2 = diaTipico2;
            });

            const cboDt3 = $(row).find('.cb-combo-parr2-dp-dt3').children();
            cboDt3.each((x, item) => {
                if (item.value === data.Pafunr2dtg3) {
                    $(item).prop('selected', true);
                }
            });

            $(row).find('.cb-combo-parr2-dp-dt3').on('click', function () {
                const diaTipico3 = this.value;
                data.Pafunr2dtg3 = diaTipico3;
            });

            const cboDt4 = $(row).find('.cb-combo-parr2-dp-dt4').children();
            cboDt4.each((x, item) => {
                if (item.value === data.Pafunr2dtg4) {
                    $(item).prop('selected', true);
                }
            });

            $(row).find('.cb-combo-parr2-dp-dt4').on('click', function () {
                const diaTipico4 = this.value;
                data.Pafunr2dtg4 = diaTipico4;
            });

            const cboDt5 = $(row).find('.cb-combo-parr2-dp-dt5').children();
            cboDt5.each((x, item) => {
                if (item.value === data.Pafunr2dtg5) {
                    $(item).prop('selected', true);
                }
            });

            $(row).find('.cb-combo-parr2-dp-dt5').on('click', function () {
                const diaTipico5 = this.value;
                data.Pafunr2dtg5 = diaTipico5;
            });

            const cboDt6 = $(row).find('.cb-combo-parr2-dp-dt6').children();
            cboDt6.each((x, item) => {
                if (item.value === data.Pafunr2dtg6) {
                    $(item).prop('selected', true);
                }
            });

            $(row).find('.cb-combo-parr2-dp-dt6').on('click', function () {
                const diaTipico6 = this.value;
                data.Pafunr2dtg6 = diaTipico6;
            });

            const cboDt7 = $(row).find('.cb-combo-parr2-dp-dt7').children();
            cboDt7.each((x, item) => {
                if (item.value === data.Pafunr2dtg7) {
                    $(item).prop('selected', true);
                }
            });

            $(row).find('.cb-combo-parr2-dp-dt7').on('click', function () {
                const diaTipico7 = this.value;
                data.Pafunr2dtg7 = diaTipico7;
            });
        },
        searching: false,
        bLengthChange: false,
        bSort: false,
        destroy: true,
        paging: false,
        pageLength: 20,
        info: false,
    });
}

function obtenerDtDiasTipicosDpA1() {
    dtDiasTipicosDpA1 = $('#dtDiasTipicosDpA1').DataTable({
        data: diasTipicos,
        columns: [
            { title: 'Lunes', data: 'Pafuna1dtg1' },
            { title: 'Martes', data: 'Pafuna1dtg2' },
            { title: 'Miercoles', data: 'Pafuna1dtg3' },
            { title: 'Jueves', data: 'Pafuna1dtg4' },
            { title: 'Viernes', data: 'Pafuna1dtg5' },
            { title: 'Sabado', data: 'Pafuna1dtg6' },
            { title: 'Domingo', data: 'Pafuna1dtg7' },
        ],
        columnDefs: [
            {
                targets: 0,
                render: function (data, type, row) {
                    // Devuelve un elemento HTML de tipo select con opciones predefinidas
                    return '<select class="cb-combo-para1-dp-dt1"><option value="0">-</option><option value="1">G1</option><option value="2">G2</option><option value="3">G3</option><option value="4">G4</option><option value="5">G5</option><option value="6">G6</option><option value="7">G7</option></select>';
                }
            },
            {
                targets: 1,
                render: function (data, type, row) {
                    // Devuelve un elemento HTML de tipo select con opciones predefinidas
                    return '<select class="cb-combo-para1-dp-dt2"><option value="0">-</option><option value="1">G1</option><option value="2">G2</option><option value="3">G3</option><option value="4">G4</option><option value="5">G5</option><option value="6">G6</option><option value="7">G7</option></select>';
                }
            },
            {
                targets: 2,
                render: function (data, type, row) {
                    // Devuelve un elemento HTML de tipo select con opciones predefinidas
                    return '<select class="cb-combo-para1-dp-dt3"><option value="0">-</option><option value="1">G1</option><option value="2">G2</option><option value="3">G3</option><option value="4">G4</option><option value="5">G5</option><option value="6">G6</option><option value="7">G7</option></select>';
                }
            },
            {
                targets: 3,
                render: function (data, type, row) {
                    // Devuelve un elemento HTML de tipo select con opciones predefinidas
                    return '<select class="cb-combo-para1-dp-dt4"><option value="0">-</option><option value="1">G1</option><option value="2">G2</option><option value="3">G3</option><option value="4">G4</option><option value="5">G5</option><option value="6">G6</option><option value="7">G7</option></select>';
                }
            },
            {
                targets: 4,
                render: function (data, type, row) {
                    // Devuelve un elemento HTML de tipo select con opciones predefinidas
                    return '<select class="cb-combo-para1-dp-dt5"><option value="0">-</option><option value="1">G1</option><option value="2">G2</option><option value="3">G3</option><option value="4">G4</option><option value="5">G5</option><option value="6">G6</option><option value="7">G7</option></select>';
                }
            },
            {
                targets: 5,
                render: function (data, type, row) {
                    // Devuelve un elemento HTML de tipo select con opciones predefinidas
                    return '<select class="cb-combo-para1-dp-dt6"><option value="0">-</option><option value="1">G1</option><option value="2">G2</option><option value="3">G3</option><option value="4">G4</option><option value="5">G5</option><option value="6">G6</option><option value="7">G7</option></select>';
                }
            },
            {
                targets: 6,
                render: function (data, type, row) {
                    // Devuelve un elemento HTML de tipo select con opciones predefinidas
                    return '<select class="cb-combo-para1-dp-dt7"><option value="0">-</option><option value="1">G1</option><option value="2">G2</option><option value="3">G3</option><option value="4">G4</option><option value="5">G5</option><option value="6">G6</option><option value="7">G7</option></select>';
                }
            }
        ],
        createdRow: function (row, data, index) {
            const cboDt1 = $(row).find('.cb-combo-para1-dp-dt1').children();
            cboDt1.each((x, item) => {
                if (item.value === data.Pafuna1dtg1) {
                    $(item).prop('selected', true);
                }
            });

            $(row).find('.cb-combo-para1-dp-dt1').on('click', function () {
                const diaTipico1 = this.value;
                data.Pafuna1dtg1 = diaTipico1;
            });

            const cboDt2 = $(row).find('.cb-combo-para1-dp-dt2').children();
            cboDt2.each((x, item) => {
                if (item.value === data.Pafuna1dtg2) {
                    $(item).prop('selected', true);
                }
            });

            $(row).find('.cb-combo-para1-dp-dt2').on('click', function () {
                const diaTipico2 = this.value;
                data.Pafuna1dtg2 = diaTipico2;
            });

            const cboDt3 = $(row).find('.cb-combo-para1-dp-dt3').children();
            cboDt3.each((x, item) => {
                if (item.value === data.Pafuna1dtg3) {
                    $(item).prop('selected', true);
                }
            });

            $(row).find('.cb-combo-para1-dp-dt3').on('click', function () {
                const diaTipico3 = this.value;
                data.Pafuna1dtg3 = diaTipico3;
            });

            const cboDt4 = $(row).find('.cb-combo-para1-dp-dt4').children();
            cboDt4.each((x, item) => {
                if (item.value === data.Pafuna1dtg4) {
                    $(item).prop('selected', true);
                }
            });

            $(row).find('.cb-combo-para1-dp-dt4').on('click', function () {
                const diaTipico4 = this.value;
                data.Pafuna1dtg4 = diaTipico4;
            });

            const cboDt5 = $(row).find('.cb-combo-para1-dp-dt5').children();
            cboDt5.each((x, item) => {
                if (item.value === data.Pafuna1dtg5) {
                    $(item).prop('selected', true);
                }
            });

            $(row).find('.cb-combo-para1-dp-dt5').on('click', function () {
                const diaTipico5 = this.value;
                data.Pafuna1dtg5 = diaTipico5;
            });

            const cboDt6 = $(row).find('.cb-combo-para1-dp-dt6').children();
            cboDt6.each((x, item) => {
                if (item.value === data.Pafuna1dtg6) {
                    $(item).prop('selected', true);
                }
            });

            $(row).find('.cb-combo-para1-dp-dt6').on('click', function () {
                const diaTipico6 = this.value;
                data.Pafuna1dtg6 = diaTipico6;
            });

            const cboDt7 = $(row).find('.cb-combo-para1-dp-dt7').children();
            cboDt7.each((x, item) => {
                if (item.value === data.Pafuna1dtg7) {
                    $(item).prop('selected', true);
                }
            });

            $(row).find('.cb-combo-para1-dp-dt7').on('click', function () {
                const diaTipico7 = this.value;
                data.Pafuna1dtg7 = diaTipico7;
            });
        },
        searching: false,
        bLengthChange: false,
        bSort: false,
        destroy: true,
        paging: false,
        pageLength: 20,
        info: false,
    });
}

function obtenerDtDiasTipicosDpA2() {
    dtDiasTipicosDpA2 = $('#dtDiasTipicosDpA2').DataTable({
        data: diasTipicos,
        columns: [
            { title: 'Lunes', data: 'Pafuna2dtg1' },
            { title: 'Martes', data: 'Pafuna2dtg2' },
            { title: 'Miercoles', data: 'Pafuna2dtg3' },
            { title: 'Jueves', data: 'Pafuna2dtg4' },
            { title: 'Viernes', data: 'Pafuna2dtg5' },
            { title: 'Sabado', data: 'Pafuna2dtg6' },
            { title: 'Domingo', data: 'Pafuna2dtg7' },
        ],
        columnDefs: [
            {
                targets: 0,
                render: function (data, type, row) {
                    // Devuelve un elemento HTML de tipo select con opciones predefinidas
                    return '<select class="cb-combo-para2-dp-dt1"><option value="0">-</option><option value="1">G1</option><option value="2">G2</option><option value="3">G3</option><option value="4">G4</option><option value="5">G5</option><option value="6">G6</option><option value="7">G7</option></select>';
                }
            },
            {
                targets: 1,
                render: function (data, type, row) {
                    // Devuelve un elemento HTML de tipo select con opciones predefinidas
                    return '<select class="cb-combo-para2-dp-dt2"><option value="0">-</option><option value="1">G1</option><option value="2">G2</option><option value="3">G3</option><option value="4">G4</option><option value="5">G5</option><option value="6">G6</option><option value="7">G7</option></select>';
                }
            },
            {
                targets: 2,
                render: function (data, type, row) {
                    // Devuelve un elemento HTML de tipo select con opciones predefinidas
                    return '<select class="cb-combo-para2-dp-dt3"><option value="0">-</option><option value="1">G1</option><option value="2">G2</option><option value="3">G3</option><option value="4">G4</option><option value="5">G5</option><option value="6">G6</option><option value="7">G7</option></select>';
                }
            },
            {
                targets: 3,
                render: function (data, type, row) {
                    // Devuelve un elemento HTML de tipo select con opciones predefinidas
                    return '<select class="cb-combo-para2-dp-dt4"><option value="0">-</option><option value="1">G1</option><option value="2">G2</option><option value="3">G3</option><option value="4">G4</option><option value="5">G5</option><option value="6">G6</option><option value="7">G7</option></select>';
                }
            },
            {
                targets: 4,
                render: function (data, type, row) {
                    // Devuelve un elemento HTML de tipo select con opciones predefinidas
                    return '<select class="cb-combo-para2-dp-dt5"><option value="0">-</option><option value="1">G1</option><option value="2">G2</option><option value="3">G3</option><option value="4">G4</option><option value="5">G5</option><option value="6">G6</option><option value="7">G7</option></select>';
                }
            },
            {
                targets: 5,
                render: function (data, type, row) {
                    // Devuelve un elemento HTML de tipo select con opciones predefinidas
                    return '<select class="cb-combo-para2-dp-dt6"><option value="0">-</option><option value="1">G1</option><option value="2">G2</option><option value="3">G3</option><option value="4">G4</option><option value="5">G5</option><option value="6">G6</option><option value="7">G7</option></select>';
                }
            },
            {
                targets: 6,
                render: function (data, type, row) {
                    // Devuelve un elemento HTML de tipo select con opciones predefinidas
                    return '<select class="cb-combo-para2-dp-dt7"><option value="0">-</option><option value="1">G1</option><option value="2">G2</option><option value="3">G3</option><option value="4">G4</option><option value="5">G5</option><option value="6">G6</option><option value="7">G7</option></select>';
                }
            }
        ],
        createdRow: function (row, data, index) {
            const cboDt1 = $(row).find('.cb-combo-para2-dp-dt1').children();
            cboDt1.each((x, item) => {
                if (item.value === data.Pafuna2dtg1) {
                    $(item).prop('selected', true);
                }
            });

            $(row).find('.cb-combo-para2-dp-dt1').on('click', function () {
                const diaTipico1 = this.value;
                data.Pafuna2dtg1 = diaTipico1;
            });

            const cboDt2 = $(row).find('.cb-combo-para2-dp-dt2').children();
            cboDt2.each((x, item) => {
                if (item.value === data.Pafuna2dtg2) {
                    $(item).prop('selected', true);
                }
            });

            $(row).find('.cb-combo-para2-dp-dt2').on('click', function () {
                const diaTipico2 = this.value;
                data.Pafuna2dtg2 = diaTipico2;
            });

            const cboDt3 = $(row).find('.cb-combo-para2-dp-dt3').children();
            cboDt3.each((x, item) => {
                if (item.value === data.Pafuna2dtg3) {
                    $(item).prop('selected', true);
                }
            });

            $(row).find('.cb-combo-para2-dp-dt3').on('click', function () {
                const diaTipico3 = this.value;
                data.Pafuna2dtg3 = diaTipico3;
            });

            const cboDt4 = $(row).find('.cb-combo-para2-dp-dt4').children();
            cboDt4.each((x, item) => {
                if (item.value === data.Pafuna2dtg4) {
                    $(item).prop('selected', true);
                }
            });

            $(row).find('.cb-combo-para2-dp-dt4').on('click', function () {
                const diaTipico4 = this.value;
                data.Pafuna2dtg4 = diaTipico4;
            });

            const cboDt5 = $(row).find('.cb-combo-para2-dp-dt5').children();
            cboDt5.each((x, item) => {
                if (item.value === data.Pafuna2dtg5) {
                    $(item).prop('selected', true);
                }
            });

            $(row).find('.cb-combo-para2-dp-dt5').on('click', function () {
                const diaTipico5 = this.value;
                data.Pafuna2dtg5 = diaTipico5;
            });

            const cboDt6 = $(row).find('.cb-combo-para2-dp-dt6').children();
            cboDt6.each((x, item) => {
                if (item.value === data.Pafuna2dtg6) {
                    $(item).prop('selected', true);
                }
            });

            $(row).find('.cb-combo-para2-dp-dt6').on('click', function () {
                const diaTipico6 = this.value;
                data.Pafuna2dtg6 = diaTipico6;
            });

            const cboDt7 = $(row).find('.cb-combo-para2-dp-dt7').children();
            cboDt7.each((x, item) => {
                if (item.value === data.Pafuna2dtg7) {
                    $(item).prop('selected', true);
                }
            });

            $(row).find('.cb-combo-para2-dp-dt7').on('click', function () {
                const diaTipico7 = this.value;
                data.Pafuna2dtg7 = diaTipico7;
            });
        },
        searching: false,
        bLengthChange: false,
        bSort: false,
        destroy: true,
        paging: false,
        pageLength: 20,
        info: false,
    });
}


function CompararFechas(fecha1, fecha2) {

    //Split de las fechas recibidas para separarlas
    var x = fecha1.split('/');
    var z = fecha2.split('/');

    //Cambiamos el orden al formato americano, de esto dd/mm/yyyy a esto mm/dd/yyyy
    fecha1 = x[1] + '/' + x[0] + '/' + x[2];
    fecha2 = z[1] + '/' + z[0] + '/' + z[2];

    //Comparamos las fechas
    if (Date.parse(fecha1) >= Date.parse(fecha2)) {
        return false;
    } else {
        return true;
    }
}