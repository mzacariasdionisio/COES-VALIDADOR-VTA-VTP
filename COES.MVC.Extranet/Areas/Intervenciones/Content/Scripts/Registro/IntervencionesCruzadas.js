var controler = siteRoot + "Intervenciones/Registro/";
var EVENCLASECODI_ANUAL = 5;

$(document).ready(function ($) {
    // Acciones barra de herramientas
    $('#btnExportarExcel').click(function () {
        exportarExcelIntervencionesCruzadas(1);
    });

    //acciones de consulta
    $('.txtFecha').inputmask({
        mask: "1/2/y",
        placeholder: "dd/mm/yyyy",
        alias: "datetime"
    });

    $('.txtFecha').Zebra_DatePicker({
        readonly_element: false
    });

    $('#cboTipoEventoFiltro, #cboCausaFiltro, #cboDisponibilidad').multipleSelect({
        selectAll: true,
        filter: true,
        placeholder: "SELECCIONE"
    });

    $('#cboClaseProgramacionFiltro, #cboManiobras').multipleSelect({
        selectAll: true,
        filter: true,
        placeholder: "SELECCIONE"
    });

    $('#cboEmpresaFiltro').multipleSelect({
        filter: true,
        filter: true,
        placeholder: "SELECCIONE"
    });

    $('#cboTipoEventoFiltro, #cboCausaFiltro, #cboDisponibilidad').multipleSelect('checkAll');
    $('#cboClaseProgramacionFiltro, #cboManiobras').multipleSelect('checkAll');
    $('#cboEmpresaFiltro').multipleSelect('checkAll');

    $('#btnConsultar').click(function () {
        mostrarGrillaExcel();
    });

    $(".check_mostrarAdjuntos").on("click", function () {
        mostrarGrillaExcel();
    });

    $('#btnContraer').click(function (e) {
        $('#Contenido').toggle();
        $(this).css("display", "none");
        $('#btnDescontraer').css("display", "block");
        //asignar tamaño de handson
        ocultar = 1;

        $("#listado").hide();
        var nuevoTamanioTabla = getHeightTablaListado();
        document.getElementById('grillaExcel').style.height = nuevoTamanioTabla + "px";
        $("#listado").show();
        updateDimensionHandson(hot);
    });

    $('#btnDescontraer').click(function (e) {
        $('#Contenido').slideDown();
        $(this).css("display", "none");
        $('#btnContraer').css("display", "block");
        ocultar = 0;

        $("#grillaExcel").hide();
        var nuevoTamanioTabla = getHeightTablaListado();
        document.getElementById('grillaExcel').style.height = nuevoTamanioTabla + 'px';
        $("#grillaExcel").show();
        updateDimensionHandson(hot);
    });

    // Valida HorasIndispo
    _mostrarH48Indisp();
    $('#cboFamilias').on("change", function () {
        _mostrarH48Indisp();
    });
    $("#btnCerrarNotif").on("click", function () {
        CerrarNotificacion();
    });
});

function mostrarGrillaExcel() {

    var objData = getObjetoFiltro();
    var msj = validarConsulta(objData);

    if (msj == "") {
        $.ajax({
            type: 'POST',
            url: controler + "GrillaExcel",
            data: objData,
            dataType: 'json',
            success: function (result) {
                if (result.Resultado != "-1") {
                    //completar información
                    ObtenerDataGridCruzada(result.GridExcel.ListaFecha, result.GridExcel.ListaEq);
                    result.GridExcel.Data = data;
                    result.GridExcel.DataCodigo = dataCodigo;

                    MODELO_GRID = result.GridExcel;
                    generarHoTweb();
                    if (result.GridExcel.ListaNotificaciones !== undefined && result.GridExcel.ListaNotificaciones.length > 0)
                        mostrarPopupNotificacion(result.GridExcel);
                } else {
                    alert(result.StrMensaje);
                }
            },
            error: function (err) {
                alert('Lo sentimos no se puede mostrar la consulta . *Revise que el rango de fechas no debe de sobrepasar el año')
            }
        });
    } else {
        alert(msj);
    }
}

function ObtenerDataGridCruzada(listaAllFecha, listaAllEq) {
    // Se arma la matriz de datos
    var listaFila = [];
    var listaFilaCodigo = [];

    var row = 0;
    for (var i = 0; i < listaAllEq.length; i++) {
        var regEq = listaAllEq[i];

        var listaCelda = [];
        var listaCeldaCodigo = [];

        listaCelda.push(regEq.EmprNomb);
        listaCelda.push(regEq.AreaNomb);
        listaCelda.push(regEq.FamAbrev);
        listaCelda.push(regEq.EquiNomb);
        listaCeldaCodigo.push(regEq.Emprcodi.toString());
        listaCeldaCodigo.push(regEq.Areacodi.toString());
        listaCeldaCodigo.push(regEq.Equicodi.toString());

        var col = listaCelda.length;
        for (var j = 0; j < listaAllFecha.length; j++) {
            var regDia = listaAllFecha[j];

            var htmlDiaEq = "";

            var objDiaXEq = regEq.ListaEqDia.find((x) => x.Dia == regDia.Dia);
            if (objDiaXEq != null) {

                var listadoFormat = objDiaXEq.ListaDato.map((x) => format("{5} <a href='#' content='{8}, {9}, {10}, {11}, {12}, {13}, {14}, {15}, {16}, {17}, {18}, {19}, {20}' class='intervencion intercodi_{1} {0}' title='{2}'>{3}-{4}  {7}</a>{6}{21}{22}"
                    , x.CeldaClase, x.Intercodi, x.Title, x.CeldaHoraIni, x.CeldaHoraFin, x.CeldaHorizonte, (x.TieneArchivo ? "<span class='cruz_file'>*</span>" : ""),
                    x.VistoBueno, x.Tipoevenabrev, x.EstadoRegistro, x.Operadornomb, x.Famabrev, x.InterfechainiDesc, x.InterfechafinDesc, x.InterindispoDesc
                    , x.InterinterrupDesc, x.InterconexionprovDesc, x.IntersistemaaisladoDesc, x.UltimaModificacionUsuarioDesc, x.UltimaModificacionFechaDesc, x.Interdescrip, (x.TieneNota ? "<span class='cruz_file'>!</span>" : ""), (x.Interflagsustento != 1 ? "" : "<span class='cruz_file'>#</span>")));

                htmlDiaEq = listadoFormat.join(" <br/> ");
            }

            listaCelda.push(htmlDiaEq);
            col++;
        }

        listaCelda.push(regEq.HorasIndispXEq.toString());
        row++;

        listaFila.push(listaCelda);
        listaFilaCodigo.push(listaCeldaCodigo);
    }

    data = listaFila;
    dataCodigo = listaFilaCodigo;
}

function format(fmt, ...args) {
    if (!fmt.match(/^(?:(?:(?:[^{}]|(?:\{\{)|(?:\}\}))+)|(?:\{[0-9]+\}))+$/)) {
        throw new Error('invalid format string.');
    }
    return fmt.replace(/((?:[^{}]|(?:\{\{)|(?:\}\}))+)|(?:\{([0-9]+)\})/g, (m, str, index) => {
        if (str) {
            return str.replace(/(?:{{)|(?:}})/g, m => m[0]);
        } else {
            if (index >= args.length) {
                throw new Error('argument index is out of range in format');
            }
            return args[index];
        }
    });
}

function exportarExcelIntervencionesCruzadas(tipoReporte) {
    var objData = getObjetoFiltro();
    objData.TipoReporte = tipoReporte;

    var msj = validarConsulta(objData);

    if (msj == "") {
        $.ajax({
            type: 'POST',
            url: controler + 'ExportarIntervencionesCruzadas',
            dataType: 'json',
            data: objData,
            success: function (result) {
                if (result.Resultado != "-1") {
                    var paramList = [
                        { tipo: 'input', nombre: 'file', value: result.NombreArchivo }
                    ];
                    var form = CreateForm(controler + 'AbrirArchivo', paramList);
                    document.body.appendChild(form);
                    form.submit();
                } else {
                    alert(result.Mensaje);
                }
            },
            error: function (err) {
                alert('Lo sentimos no se puede mostrar la consulta . *Revise que el rango de fechas no debe de sobrepasar el año')
            }
        });
    } else {
        alert(msj);
    }
}

function programaciones() {
    var tipoProgramacion = parseInt($('#TipoProgramacion').val()) || 0;

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
                        option += '<option value =' + v.Progrcodi + ' selected>' + v.ProgrnombYPlazo + '</option>';
                    } else {
                        option += '<option value =' + v.Progrcodi + '>' + v.ProgrnombYPlazo + '</option>';
                    }
                })
                $('#Programacion').append(option);
                $('#Programacion').trigger("change");
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

function causasFiltro() {
    var tipoEvenCodi = $('#cboTipoEventoFiltro').val();

    if (tipoEvenCodi != "" || tipoEvenCodi == null) {
        $.ajax({
            type: 'POST',
            url: controler + "ListarCboCausa",
            datatype: 'json',
            data: JSON.stringify({ claProCodi: $('#cboTipoEventoFiltro').val() }),
            contentType: "application/json",
            success: function (modelo) {
                $('#cboCausaFiltro').empty();
                //var option = '<option value="0">----- Seleccione  ----- </option>';
                $.each(modelo.ListaCausas, function (k, v) {
                    var option = '<option value =' + v.Subcausacodi + '>' + v.Subcausadesc + '</option>';
                    $('#cboCausaFiltro').append(option);
                })
                $('#cboCausaFiltro').multipleSelect({
                    filter: true,
                    placeholder: "SELECCIONE"
                });
                $("#cboCausaFiltro").multipleSelect("refresh");
                $('#cboCausaFiltro').multipleSelect('checkAll');
            }
        });
    } else {
        $('#cboCausaFiltro').empty();
        $('#cboCausaFiltro').multipleSelect({
            filter: true,
            placeholder: "SELECCIONE"
        });
        $("#cboCausaFiltro").multipleSelect("refresh");
        $('#cboCausaFiltro').multipleSelect('checkAll');
    }
}

function obtenerFechaOperacion() {
    var tipoProgramacion = parseInt($('#TipoProgramacion').val()) || 0;
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
            $("#Entidad_Interfechaini").val(model.Progrfechaini);
            $("#Entidad_Interfechafin").val(model.Progrfechafin);
        },
        error: function (err) {
            alert("Lo sentimos, se ha producido un error al obtener las fechas de operacion");
        }
    });
}

//#region Metodos Generales

function _mostrarH48Indisp() {
    var id = parseInt($("#cboFamilias").val()) || 0;
    if (id == 1) {
        $("#cboHrasIndispp").parent().parent().show()
    } else {
        $("#cboHrasIndispp").parent().parent().hide()
        $("#cboHrasIndispp").val(0);
    }
}

//funcion que calcula el ancho disponible para la tabla reporte
function getHeightTablaListado() {
    return $(window).height()
        - $("header").height()
        - $("#cntTitulo").height() - 2
        - $("#Reemplazable .form-title").height()
        - 15
        - $("#Contenido").parent().height() //Filtros
        - 20 //leyenda
        - 15
        - 61 //- $(".footer").height() - 10

        - 18 //propio de la tabla
        ;
}

function validarConsulta(objFiltro) {
    var listaMsj = [];

    if (objFiltro.TipoProgramacion <= 0)
        listaMsj.push("Seleccione un Tipo de Programación");

    //tipo evento
    if (objFiltro.TipoEvenCodi == "")
        listaMsj.push("Tipo evento: Seleccione una opción.");
    //causa
    if (objFiltro.Subcausa == "")
        listaMsj.push("Causa: Seleccione una opción.");
    //Disponibilidad
    if (objFiltro.InterIndispo == "")
        listaMsj.push("Disponibilidad: Seleccione una opción.");
    //clase programación
    if (objFiltro.ClaseProgramacion == "")
        listaMsj.push("Clase programación: Seleccione una opción.");
    //Empresa
    if (objFiltro.Emprcodi == "")
        listaMsj.push("Empresa: Seleccione una opción.");
    //maniobras
    if (objFiltro.Maniobras == "")
        listaMsj.push("Maniobras: Seleccione una opción.");

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
    var progrCodi = parseInt($('#Programacion').val()) || 0;
    var tipoProgramacion = parseInt($('#TipoProgramacion').val()) || 0;

    var interFechaIni = $('#Entidad_Interfechaini').val();
    var interFechaFin = $('#Entidad_Interfechafin').val();

    var emprCodi = $('#cboEmpresaFiltro').multipleSelect('getSelects').join(',');
    var tipoEvenCodi = $('#cboTipoEventoFiltro').multipleSelect('getSelects').join(',');

    var interIndispo = $('#cboDisponibilidad').multipleSelect('getSelects').join(',');
    //var interIndispo = $('#cboDisponibilidad').val();

    var vCausa = $('#cboCausaFiltro').multipleSelect('getSelects').join(',');
    //var vCausa = $('#cboCausaFiltro').val();

    var vClaseProgramacion = $('#cboClaseProgramacionFiltro').multipleSelect('getSelects').join(',');
    //var vClaseProgramacion = $('#cboClaseProgramacionFiltro').val();

    var vFamilia = $('#cboFamilias').val();

    var vManiobras = $('#cboManiobras').val() != null && $('#cboManiobras')[0].length == $('#cboManiobras').val().length ? "0" : $('#cboManiobras').multipleSelect('getSelects').join(',');
    //var vManiobras = $('#cboManiobras').val();
    var vHorasIndispo = $('#cboHrasIndispp').val();

    var estadoFiles = getEstadoFiles();

    var obj = {
        Progrcodi: progrCodi,
        TipoProgramacion: tipoProgramacion,

        InterFechaIni: interFechaIni,
        InterFechaFin: interFechaFin,

        Emprcodi: emprCodi,
        TipoEvenCodi: tipoEvenCodi,

        InterIndispo: interIndispo,

        Subcausa: vCausa,
        ClaseProgramacion: vClaseProgramacion,

        TipoGrupoEquipo: vFamilia,
        Maniobras: vManiobras,
        HorasIndispo: vHorasIndispo,

        EstadoFiles: estadoFiles,
    };

    return obj;
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

//check Mostrar Archivos
function getEstadoFiles() {
    var estado = "0";
    if ($('#check_mostrarAdjuntos').is(':checked')) {
        estado = '1';
    }
    return estado;
}

// solo para formatos dd/mm/yyy
function toDate(dateStr) {
    var parts = dateStr.split("/");
    return new Date(parts[2], parts[1] - 1, parts[0]);
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

function validarFormatoFech(obj) {
    var patron = /^\d{1,2}\/\d{2}\/\d{4}$/
    if (!patron.test(obj)) {
        return false;
    }
    return true;
}

//#endregion

function mostrarPopupNotificacion(model) {
    var intercodis = "";
    var lista = model.ListaNotificaciones;
    var sStyle = "";
    var styleFraccionado = "";
    var styleConnsecutivoXHora = "";
    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono" id="TablaConsultaIntervencion" style="overflow: auto; height:auto; width: 1000px !important; white-space: nowrap">
        <thead>
            <tr>
                <th style="text-align:center;width:20%">Empresa</th>
                <th style="text-align:center;width:12%">Ubicacion</th>
                <th style="text-align:center;width:8%">Tipo</th>
                <th style="text-align:center;width:6%">Equipo</th>
                <th style="text-align:center;width:8%">Fecha<br> inicio</th>
                <th style="text-align:center;width:8%">Fecha<br> fin</th>
                <th style="text-align:center;width:22%">Descripción</th>
                <th style="text-align:center;width:8%">Usuario Mod.</th>
                <th style="text-align:center;width:8%">Fec. Mod.</th>
                <th style="text-align:center;width:8%">Estado Antiguo.</th>
                <th style="text-align:center;width:8%">Estado Nuevo.</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in lista) {
        var item = lista[key];
        cadena += `
            
            <tr id="fila_${item.Intercodi}">
                <td style="text-align:left; ${sStyle}">${item.EmprNomb}</td>
                <td style="text-align:left; ${sStyle}">${item.AreaNomb}</td>
                <td style="text-align:left; ${sStyle}">${item.Famabrev}</td>
                <td style="text-align:left; ${sStyle}">${item.Equiabrev}</td>
                <td style="text-align:center; ${sStyle} ${styleFraccionado} ${styleConnsecutivoXHora}">${item.InterfechainiDesc}</td>
                <td style="text-align:center; ${sStyle} ${styleFraccionado} ${styleConnsecutivoXHora}">${item.InterfechafinDesc}</td>
                <td style="text-align:left; ${sStyle}">${item.Interdescrip}</td>
                <td style="text-align:left; ${sStyle}">${item.UltimaModificacionUsuarioDesc}</td>
                <td style="text-align:left; ${sStyle}">${item.UltimaModificacionFechaDesc}</td>
                <td style="text-align:left; ${sStyle}">${item.EstadoRegistroPadre}</td>
                <td style="text-align:left; ${sStyle}">${item.EstadoRegistro}</td>
            </tr>
        `;
        if (intercodis == "")
            intercodis = item.Intercodi;
        else
            intercodis += "," + item.Intercodi;
    }
    $('#hfIntercodisNotif').val(intercodis);
    cadena += "</tbody></table>";
    $('#idtitulo').text("");
    //alert(idtitulo);
    $('#tablaNotificacion').html(cadena);
    switch (model.IdTipoProgramacion) {
        case 2: $('#idtitulo').text("Cambios de Estado - Programado Diario");
            break;
        case 3: $('#idtitulo').text("Cambios de Estado - Programado Semanal");
            break;
        case 4: $('#idtitulo').text("Cambios de Estado - Programado Mensual");
            break;
        case 5: $('#idtitulo').text("Cambios de Estado - Programado Anaul");
            break;
    }
    setTimeout(function () {
        $('#popupFormNotificacion').bPopup({
            modalClose: false,
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown'
        });
    }, 50);


    $('#btnCerrarNotif').prop('disabled', true);
    $('#btnCerrarNotif').css({ opacity: 0.3 });
    setTimeout(function () {
        $('#btnCerrarNotif').prop('disabled', false);
        $('#btnCerrarNotif').css({ opacity: 1 });
    }, 10000);

}

function CerrarNotificacion() {
    var codigos = $("#hfIntercodisNotif").val();
    $.ajax({
        type: "POST",
        url: controler + "ActualizarLeidoAgente",
        traditional: true,
        data: {
            intercodis: codigos
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                $('#popupFormNotificacion').bPopup().close();
                //_mostrarMensajeAlertaTemporal(true, evt.StrMensaje);

            } else {
                alert(evt.StrMensaje);
            }
        },
        error: function (err) {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}