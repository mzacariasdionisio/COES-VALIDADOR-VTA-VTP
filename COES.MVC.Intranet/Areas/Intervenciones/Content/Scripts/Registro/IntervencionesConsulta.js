var controler = siteRoot + "Intervenciones/Registro/";
var ANCHO_LISTADO = 900;
var EVENCLASECODI_ANUAL = 5;

$(document).ready(function ($) {
    $.fn.dataTable.moment('DD/MM/YYYY HH:mm');
    $("#btnOcultarMenu").click();

    ANCHO_LISTADO = $("#mainLayout").width() > 900 ? $("#mainLayout").width() - 30 : 900;

    $('.txtFecha').inputmask({
        mask: "1/2/y",
        placeholder: "dd/mm/yyyy",
        alias: "datetime"
    });
    $('.txtFecha').Zebra_DatePicker({
        readonly_element: false
    });

    $('#cboTipoIntervencion, #InterDispo, #estadocodi').multipleSelect({
        selectAll: true,
        filter: true,
        placeholder: "SELECCIONE",
    });

    $('#cboEmpresa, #cboUbicacion,#cboFamilia').multipleSelect({
        filter: true,
        placeholder: "SELECCIONE",
    });

    $('#cboEquipo').multipleSelect({
        filter: true,
        placeholder: "SELECCIONE",
    });

    $('#cboTipoIntervencion, #InterDispo, #estadocodi').multipleSelect('checkAll');
    $('#cboEmpresa, #cboUbicacion, #cboFamilia').multipleSelect('checkAll');
    $('#cboEquipo').multipleSelect('checkAll');


    $('#cboUbicacion').on("change", function () {
        equipo();
    });
    $('#cboFamilia').on("change", function () {
        equipo();
    });

    $('#cboTipoProgramacion').on("change", function () {
        programaciones();
        equipo();
    });

    $('#Programacion').on("change", function () {
        obtenerFechaOperacion();
    });

    $('#btnConsultar').click(function () {
        mostrarLista();
    });

    $('#btnExportarExcel').click(function () {
        ExportaAExcelConsultaIntervenciones();
    });

    $('#btnManttoConsulta').click(function () {
        generarManttoConsultaConsulta();
    });

    $('#btnManttoPlantilla').click(function () {
        descargarManttoPlantilla();
    });

    $('#btnContraer').click(
        function (e) {
            $('#Contenido').toggle();
            $('#tablaTabular2').toggle();
            $(this).css("display", "none");
            $('#btnDescontraer').css("display", "block");
            //asignar tamaño de handson
            ocultar = 1;

            $("#listado").hide();
            var nuevoTamanioTabla = getHeightTablaListado();
            $(".dataTables_scrollBody").css('height', nuevoTamanioTabla + "px");
            $("#listado").show();
        }
    );

    $('#btnDescontraer').click(
        function (e) {
            $('#Contenido').slideDown();
            $('#tablaTabular2').slideDown();
            $(this).css("display", "none");
            $('#btnContraer').css("display", "block");
            ocultar = 0;
            $("#listado").hide();
            var nuevoTamanioTabla = getHeightTablaListado();
            $(".dataTables_scrollBody").css('height', nuevoTamanioTabla + 'px');
            $("#listado").show();
        }
    );
});

//funcion que calcula el ancho disponible para la tabla reporte
function getHeightTablaListado() {
    /*return $("#mainLayout").height() //todo
        - 15 //padding-top
        - 15 //padding-bottom
        - $("#Contenido").parent().height() //Filtros
        - 14 //<br>
        - 30 //TablaConsultaIntervencion_info
        ;*/
    return $(window).height()
        - $("header").height()
        - $("#cntTitulo").height() - 2
        //- $("#Reemplazable .form-title").height()
        - 55
        - $("#Contenido").parent().height() //Filtros
        - 14 //<br>
        - $(".dataTables_filter").height()
        - $(".dataTables_scrollHead").height()
        - 30 //TablaConsultaIntervencion_info
        - 15
        - 61 //- $(".footer").height() - 10

        - 5 //propio de la tabla
        ;
}

function ubicacion() {

    var emprcodis = $('#cboEmpresa').val();
    if (emprcodis != null) {
        $.ajax({
            type: 'POST',
            url: controler + "ListarCboUbicacion",
            datatype: 'json',
            data: JSON.stringify({ idEmpresa: $('#cboEmpresa').val() }),
            contentType: "application/json",
            success: function (modelo) {
                $("#cboUbicacion").empty();
                $('#cboEquipo').empty();
                $.each(modelo.ListaCboUbicacion, function (k, v) {
                    var option = '<option value =' + v.Areacodi + '>' + v.Areanomb + '</option>';
                    $('#cboUbicacion').append(option);
                })
                $('#cboUbicacion').multipleSelect({
                    filter: true,
                    placeholder: "SELECCIONE"
                });
                $("#cboUbicacion").multipleSelect("refresh");
                $('#cboUbicacion').multipleSelect('checkAll');
            },
            error: function (err) {
                alert("Error inesperado", $('#cboEmpresa').val());
            }
        });
    }
    else {
        $('#cboUbicacion').empty();
        $('#cboUbicacion').multipleSelect({
            filter: true,
            placeholder: "SELECCIONE"
        });
        $("#cboUbicacion").multipleSelect("refresh");
        $('#cboUbicacion').multipleSelect('checkAll');
    }
}

function equipo() {

    var areaCodi = $('#cboUbicacion').val() != null && $('#cboUbicacion')[0].length == $('#cboUbicacion').val().length ? "0" : $('#cboUbicacion').multipleSelect('getSelects').join(',');
    var famCodi = $('#cboFamilia').val() != null && $('#cboFamilia')[0].length == $('#cboFamilia').val().length ? "0" : $('#cboFamilia').multipleSelect('getSelects').join(',');
    var idTipoPrograma = parseInt($('#cboTipoProgramacion').val()) || 0;

    $.ajax({
        type: 'POST',
        url: controler + "ListarEquiposXprograma",
        datatype: 'json',
        data: JSON.stringify({ idUbicacion: areaCodi, idFamilia: famCodi, evenclasecodi: idTipoPrograma }),
        contentType: "application/json",
        success: function (modelo) {
            $('#cboEquipo').empty();
            $.each(modelo.ListaCboEquipo, function (k, v) {
                var option = '<option value =' + v.Entero1 + '>' + v.String1 + '</option>';
                $('#cboEquipo').append(option);
            })
            $('#cboEquipo').multipleSelect({
                filter: true,
                placeholder: "SELECCIONE"
            });
            $("#cboEquipo").multipleSelect("refresh");
            $('#cboEquipo').multipleSelect('checkAll');
        }
    });
}

//obtener programaciones
function programaciones() {
    var tipoProgramacion = parseInt($('#cboTipoProgramacion').val()) || 0;

    if (tipoProgramacion > 0) {
        $.ajax({
            type: 'POST',
            url: controler + "ListarProgramaciones",
            datatype: "json",
            contentType: 'application/json',
            data: JSON.stringify({ idTipoProgramacion: tipoProgramacion }),

            success: function (evt) {
                $('#Programacion').empty();
                evt.ProgramacionRegistro;
                var option = '<option value="0">----- Seleccione ----- </option>';
                if (EVENCLASECODI_ANUAL == tipoProgramacion) option = '<option value="0">----- Todos (más reciente) ----- </option>';
                $.each(evt.ListaProgramaciones, function (k, v) {
                    if (v.Progrcodi == evt.Entidad.Progrcodi) {
                        option += '<option value =' + v.Progrcodi + ' selected>' + v.ProgrnombYPlazoCruzado + '</option>';
                    } else {
                        option += '<option value =' + v.Progrcodi + '>' + v.ProgrnombYPlazoCruzado + '</option>';
                    }
                })
                $('#Programacion').append(option);
                $('#Programacion').trigger("change");

                obtenerFechaOperacion();

            },
            error: function (err) {
                alert("Lo sentimos, se ha producido un error");
            }
        });
    } else {
        $("#Programacion").empty();
        var option = '<option value="0">----- Seleccione un tipo de programación ----- </option>';
        $('#Programacion').append(option);
    }
}

//obtener fechas de programación seleccionada
function obtenerFechaOperacion() {
    var tipoProgramacion = parseInt($('#cboTipoProgramacion').val()) || 0;
    var programacion = parseInt($('#Programacion').val()) || 0;

    if (tipoProgramacion == 0 || programacion == 0 || tipoProgramacion == "" || programacion == "") {
        return;
    }

    $.ajax({
        type: 'POST',
        url: controler + "ObtenerFechaProgramacion",
        datatype: "json",
        contentType: 'application/json',
        data: JSON.stringify({ progCodi: programacion }),
        success: function (model) {
            $("#InterfechainiD").val(model.Progrfechaini);
            $("#InterfechafinD").val(model.Progrfechafin);
        },
        error: function (err) {
            alert("Lo sentimos, se ha producido un error al obtener las fechas de operacion");
        }
    });
}

function mostrarLista() {
    $('#listado').html('');
    ANCHO_LISTADO = $("#mainLayout").width() > 900 ? $("#mainLayout").width() - 25 : 900;

    $("#listado").html('');

    var objData = getObjetoFiltro();
    var msj = validarConsulta(objData);

    if (msj == "") {
        $.ajax({
            type: 'POST',
            url: controler + "MostrarListadoConsulta",
            data: objData,
            dataType: 'json',
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    $("#listado").hide();
                    $('#listado').css("width", ANCHO_LISTADO + "px");

                    var nuevoTamanioTabla = getHeightTablaListado();
                    nuevoTamanioTabla = nuevoTamanioTabla > 250 ? nuevoTamanioTabla : 250;

                    $("#listado").show();

                    var html = _dibujarTablaListado(evt.ListaFilaWeb);
                    $('#listado').html(html);
                    $('#TablaConsultaIntervencion').dataTable({
                        "destroy": "true",
                        "ordering": true,
                        "searching": true,
                        "paging": false,
                        "info": false,
                        order: [[1, 'asc']],
                        "scrollX": true,
                        "scrollY": $('#listado').height() > 250 ? nuevoTamanioTabla + "px" : "100%"
                    });
                } else {
                    alert(evt.Mensaje);
                }
            },
            error: function (err) {
                alert("Lo sentimos, ha ocurrido un error inesperado");
            }
        });
    } else {
        alert(msj);
    }
}

function _dibujarTablaListado(lista) {

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono" id="TablaConsultaIntervencion" style=" overflow: auto; height:auto; width: 2000px !important; white-space: nowrap">
        <thead>
            <tr>
                <th style="text-align:center">Tip.<br> Interv.</th>
                <th style="text-align:center">Empresa</th>
                <th style="text-align:center">Ubicacion</th>
                <th style="text-align:center">Tipo Equipo</th>
                <th style="text-align:center">Equipo</th>
                <th style="text-align:center">Estado</th>
                <th style="text-align:center">Fecha<br> inicio</th>
                <th style="text-align:center">Fecha<br> fin</th>
                <th style="text-align:center">MW <br>Ind.</th>
                <th style="text-align:center">Disp.</th>
                <th style="text-align:center">Interrup.</th>
                <th style="text-align:center">Descripción</th>
                <th style="text-align:center">Usuario Mod.</th>
                <th style="text-align:center">Fec. Mod.</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in lista) {
        var item = lista[key];

        var checked = "";
        if (item.ChkMensaje) checked = ' checked ';

        //
        var sStyle = "";
        if (item.Estadocodi == 3) {   // Rechazado
            sStyle = "background-color:#FF2C2C; text-decoration:line-through";
        }
        else if (item.Interdeleted == 1) {   // Eliminado
            sStyle = "background-color:#E0DADA; text-decoration:line-through";
        }

        cadena += `
            
            <tr id="fila_${item.Intercodi}">
                <td style="text-align:left;height: 21px; ${sStyle}">${item.Tipoevenabrev}</td>
                <td style="text-align:left; ${sStyle}">${item.EmprNomb}</td>
                <td style="text-align:left; ${sStyle}">${item.AreaNomb}</td>
                <td style="text-align:left; ${sStyle}">${item.Famabrev}</td>
                <td style="text-align:left; ${sStyle}">${item.Equiabrev}</td>
                <td style="text-align:left; ${sStyle}">${item.EstadoRegistro}</td>
                <td style="text-align:center; ${sStyle}">${item.InterfechainiDesc}</td>
                <td style="text-align:center; ${sStyle}">${item.InterfechafinDesc}</td>
                <td style="text-align:right; ${sStyle}">${item.Intermwindispo}</td>
                <td style="text-align:center; ${sStyle}">${item.InterindispoDesc}</td>
                <td style="text-align:center; ${sStyle}">${item.InterinterrupDesc}</td>
                <td style="text-align:left; ${sStyle}">${item.Interdescrip}</td>
                <td style="text-align:left; ${sStyle}">${item.UltimaModificacionUsuarioDesc}</td>
                <td style="text-align:left; ${sStyle}">${item.UltimaModificacionFechaDesc}</td>
            </tr>
        `;
    }
    cadena += "</tbody></table>";

    return cadena;
}

function ExportaAExcelConsultaIntervenciones() {

    var objData = getObjetoFiltro();
    var msj = validarConsulta(objData);

    if (msj == "") {
        $.ajax({
            type: 'POST',
            url: controler + 'ExportarIntervencionesConsulta',
            data: objData,
            dataType: 'json',
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    var paramList = [
                        { tipo: 'input', nombre: 'file', value: evt.NombreArchivo }
                    ];
                    var form = CreateForm(controler + 'AbrirArchivo', paramList);
                    document.body.appendChild(form);
                    form.submit();
                }
                else {
                    alert('Ha ocurrido un error');
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error');
            }
        });
    } else {
        alert(msj);
    }
}

function generarManttoConsultaConsulta() {

    var objData = getObjetoFiltro();
    var msj = validarConsulta(objData);

    if (msj == "") {
        $.ajax({
            type: 'POST',
            url: controler + 'GenerarManttoConsultaConsulta',
            data: objData,
            dataType: 'json',
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    var paramList = [
                        { tipo: 'input', nombre: 'file', value: evt.NombreArchivo }
                    ];
                    var form = CreateForm(controler + 'AbrirArchivo', paramList);
                    document.body.appendChild(form);
                    form.submit();
                }
                else {
                    alert('Ha ocurrido un error');
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error');
            }
        });
    } else {
        alert(msj);
    }
}

//Descargar plantilla
function descargarManttoPlantilla() {

    var objData = getObjetoFiltro();
    var msj = validarConsulta(objData);

    if (msj == "") {
        $.ajax({
            type: 'POST',
            url: controler + 'DescargarManttoPlantillaActualizada',
            data: objData,
            dataType: 'json',
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    var paramList = [
                        { tipo: 'input', nombre: 'file', value: evt.NombreArchivo }
                    ];
                    var form = CreateForm(controler + 'AbrirArchivo', paramList);
                    document.body.appendChild(form);
                    form.submit();
                }
                else {
                    alert('Ha ocurrido un error');
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error');
            }
        });
    } else {
        alert(msj);
    }
}

///////////////////////////////////////////////////////////////////////////////////////
// Utilitario
///////////////////////////////////////////////////////////////////////////////////////
function validarConsulta(objFiltro) {
    var listaMsj = [];

    //evenclase
    if (objFiltro.TipoProgramacion <= 0)
        listaMsj.push("Seleccione Tipo de programación");

    //Tipo intervención
    if (objFiltro.TipoEvenCodi == "")
        listaMsj.push("Tipo de intervención: Seleccione una opción.");
    //Estado
    if (objFiltro.EstadoCodi == "")
        listaMsj.push("Estado: Seleccione una opción.");
    //Ubicación
    if (objFiltro.AreaCodi == "")
        listaMsj.push("Ubicación: Seleccione una opción.");
    //Tipo equipo
    if (objFiltro.FamCodi == "")
        listaMsj.push("Tipo de equipo: Seleccione una opción.");
    //Equipo
    //

    //Empresa
    if (objFiltro.Emprcodi == "")
        listaMsj.push("Empresa: Seleccione una opción.");
    //Disponibilidad
    if (objFiltro.InterIndispo == "")
        listaMsj.push("Disponibilidad: Seleccione una opción.");

    // Valida filtros de fecha cuando falta seleccionar cualquiera de ellas
    if (objFiltro.InterFechaIni == "") {
        listaMsj.push("Seleccione una fecha de inicio");
    }
    if (objFiltro.InterFechaFin == "") {
        listaMsj.push("Seleccione una fecha de fin");
    }

    // Valida consistencia del rango de fechas
    if (!validarFormatoFech(objFiltro.InterFechaIni) || !validarFormatoFech(objFiltro.InterFechaFin)) {
        listaMsj.push("La fecha ingresada no tiene el formato correcto");
    } else {
        if (objFiltro.InterFechaIni != "" && objFiltro.InterFechaFin != "") {
            if (!CompararFechas(objFiltro.InterFechaIni, objFiltro.InterFechaFin)) {
                listaMsj.push("La fecha de inicio no puede ser mayor que la fecha de fin");
            }
        }
    }

    var msj = listaMsj.join('\n');
    return msj;
}

function getObjetoFiltro() {
    var tipoProgramacion = parseInt($('#cboTipoProgramacion').val()) || 0;
    var progrCodi = parseInt($('#Programacion').val()) || 0;

    var interFechaIni = $('#InterfechainiD').val();
    var interFechaFin = $('#InterfechafinD').val();

    var emprCodi = $('#cboEmpresa').multipleSelect('getSelects').join(',');
    var tipoEvenCodi = $('#cboTipoIntervencion').multipleSelect('getSelects').join(',');
    var estadoCodi = $('#estadocodi').multipleSelect('getSelects').join(',');
    var areaCodi = $('#cboUbicacion').multipleSelect('getSelects').join(',');
    //var famCodi = $('#cboFamilia').multipleSelect('getSelects').join(',');
    var famCodi = $('#cboFamilia').val() != null && $('#cboFamilia')[0].length == $('#cboFamilia').val().length ? "0" : $('#cboFamilia').multipleSelect('getSelects').join(',');
    var equicodi = $('#cboEquipo').val() != null && $('#cboEquipo')[0].length == $('#cboEquipo').val().length ? "0" : $('#cboEquipo').multipleSelect('getSelects').join(',');
    //var equicodi = $('#cboEquipo').multipleSelect('getSelects').join(',');

    var interIndispo = $('#InterDispo').multipleSelect('getSelects').join(',');

    var descripcion = $('#txtNombreFiltro').val();

    var tipoGrupoEquipo = $('#cboConjuntoEq').val();

    var obj = {
        TipoProgramacion: tipoProgramacion,
        Progrcodi: progrCodi,

        InterFechaIni: interFechaIni,
        InterFechaFin: interFechaFin,

        Emprcodi: emprCodi,
        TipoEvenCodi: tipoEvenCodi,
        EstadoCodi: estadoCodi,
        AreaCodi: areaCodi,
        FamCodi: famCodi,
        Equicodi: equicodi,

        InterIndispo: interIndispo,

        Descripcion: descripcion,
        TipoGrupoEquipo: tipoGrupoEquipo,
    };

    return obj;
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

function CreateForm(path, params, method = 'post') {
    var form = document.createElement('form');
    form.method = method;
    form.action = path;

    $.each(params, function (index, obj) {
        var hiddenInput = document.createElement(obj.tipo);
        hiddenInput.type = 'hidden';
        hiddenInput.name = obj.nombre;
        hiddenInput.value = obj.value;
        form.appendChild(hiddenInput);
    });
    return form;
}

function toDate(dateStr) {
    var parts = dateStr.split("/");
    return new Date(parts[2], parts[1] - 1, parts[0]);
}

function validarFormatoFech(obj) {
    var patron = /^\d{1,2}\/\d{2}\/\d{4}$/
    if (!patron.test(obj)) {
        return false;
    }
    return true;
}
