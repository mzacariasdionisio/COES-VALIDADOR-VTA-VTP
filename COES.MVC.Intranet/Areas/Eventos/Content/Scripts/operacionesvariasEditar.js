var controller = siteRoot + 'eventos/operacionesvarias/';

var modoIngreso = ''; //modo ingreso de datos: 
// unidad (se elige un equipo y sus propiedades van a texto), 
// grupo  (se elige un equipo y sus propiedades van a una grilla).
var iniciado = false;
var famcodi = "0";
var listaGps = [];

$(function () {

    $('#valhoriniGrupo').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#valhoriniGrupo').val(date + " 00:00:00");
        }
    });

    $('#valhorFinGrupo').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#valhorFinGrupo').val(date + " 00:00:00");
        }
    });

    $('#valhorini').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#valhorini').val(date + " 00:00:00");
        }
    });

    $('#valhorinicarga').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#valhorinicarga').val(date + " 00:00:00");
        }
    });

    $('#valhorfin').inputmask({
        mask: "1/2/y h:s:s",
        placeholder: "dd/mm/yyyy hh:mm:ss",
        alias: "datetime",
        hourFormat: "24"
    });

    $('#valhorfin').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#valhorfin').val(date + " 00:00:00");
        }
    });

    $("#valhorini").change(function () {
        $("#valhoriniGrupo").val($("#valhorini").val());
    });
    $("#valhorfin").change(function () {
        $("#valhorFinGrupo").val($("#valhorfin").val());
    });

    $('#txthorcoord').inputmask({
        mask: "1/2/y h:s:s",
        placeholder: "dd/mm/yyyy hh:mm:ss",
        alias: "datetime",
        hourFormat: "24"
    });

    $('#txthorcoord').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#txthorcoord').val(date + " 00:00:00");
        }
    });

    $('#txthorcoord2').inputmask({
        mask: "1/2/y h:s:s",
        placeholder: "dd/mm/yyyy hh:mm:ss",
        alias: "datetime",
        hourFormat: "24"
    });

    $('#txthorcoord2').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#txthorcoord2').val(date + " 00:00:00");
        }
    });

    $('#btnCancelar').click(function () {
        var equicodiCancel = $('#hfEquicodi').val();
        var codOp = $('#hficcodi').val();

        var regNuevo = (codOp == 0);
        var regEqInvolucrado = numeroEquipoInvolucrado();

        if (regNuevo && ((equicodiCancel != '0') || (regEqInvolucrado > 1))) {
            if (confirm('Se eliminará lo registrado. Desea continuar?')) {

                document.location.href = controller;
            }
        } else {
            document.location.href = controller;
        }




    });

    $('#btnCancelar2').click(function () {
        document.location.href = controller;
    });

    $('#cbTipoOpera').change(function () {
        cargarTipoOperacion($('#cbTipoOpera').val());
    });

    $('#cbHorizonte').change(function () {
        $('#hfEvenclasecodi').val($('#cbHorizonte').val());
    });

    setTextoInvisible();

    $(document).ready(function () {
        //console.log('fin cargado: ' + $('#cbTipoOpera').val());
        cargarTipoOperacion($('#cbTipoOpera').val());

        //Trae la tabla de los equipos Involucrados
        cargardetalle($('#hficcodi').val());

        //Trae la tabla de los grupos Involucrados (CONGESTION)
        cargardetalleGrupos($('#hficcodi').val());

        //Trae la tabla de los grupos Involucrados (SISTEMAS AISLADOS)
        cargardetalleGps($('#hficcodi').val());
    });


    $('#btnSeleccionar').click(function () {

        modoIngreso = 'unidad';
        //famcodi = "4,8";
        //openBusquedaEquipoFamcodi(famcodi);


        $.ajax({
            type: "POST",
            url: siteRoot + 'eventos/subcausafamilia/listaevesubcausafamilias',
            data: { subcausacodi: $('#cbTipoOpera').val() },
            success: function (result) {
                famcodi = result;
                openBusquedaEquipoFamcodi(result);
            },
            error: function () {
                mostrarError();
            }
        });
    });

    $('#btnSeleccionar2').click(function () {
        agregarEquipo();
    });

    $('#btnSeleccionarGps').click(function () {
        popupGps();
    });

    $('#btnGrabar').click(function () {
        grabar();
    });

    $('#btnGrabar2').click(function () {
        grabar();
    });

    $('#cbxgenevenini').click(function () {
        var tpoope = $('#cbTipoOpera').val();
        if (tpoope == 203) {
            grabar();
        }
    });

    $('#cbxgenevenfin').click(function () {
        var tpoope = $('#cbTipoOpera').val();
        if (tpoope == 203) {
            grabar();
        }
    });

    //otras funciones
    $('#cbHorizonte').val($('#hfEvenclasecodi').val());
    $('#cbTipoOpera').val($('#hfSubcausacodi').val());

    if ($('#hfIccheck1').val() == "S") { $('#cbxIccheck1').prop('checked', true); }
    if ($('#hfIccheck2').val() == "S") { $('#cbxIccheck2').prop('checked', true); }
    if ($('#hfIccheck3').val() == "S") { $('#cbxindisp').prop('checked', true); }
    if ($('#hfIccheck4').val() == "S") { $('#cbxindparc').prop('checked', true); }

    if ($('#hfIccheckCuadro').val() == 1) { $('#cbxIccheckCuadro1').prop('checked', true); }
    if ($('#hfIccheckCuadro').val() == 2) { $('#cbxIccheckCuadro2').prop('checked', true); }
    if ($('#hfIccheckCuadro').val() == 3) { $('#cbxIccheckCuadro3').prop('checked', true); }
    if ($('#hfIccheckCuadro').val() == 0) { $('#cbxIccheckCuadro4').prop('checked', true); }

    if ($('#hfAccionCuadro').val() == 0) { $('#cbxIccheckCuadro1').prop('disabled', true); }
    if ($('#hfAccionCuadro').val() == 0) { $('#cbxIccheckCuadro2').prop('disabled', true); }
    if ($('#hfAccionCuadro').val() == 0) { $('#cbxIccheckCuadro3').prop('disabled', true); }
    if ($('#hfAccionCuadro').val() == 0) { $('#cbxIccheckCuadro4').prop('disabled', true); }

    if ($('#hfAccion').val() == 0) {
        $('#btnGrabar').hide();
        $('#btnSeleccionar').hide();

    }

    cargarEquipos();
    cargarBusquedaEquipo();
    cargarPrevio();

    $('#btnDespacho').click(function () {

        ListGrupoDespacho();
    });
});

numeroEquipoInvolucrado = function () {

    var numregEqInv = 0;

    var tbl = document.getElementById("tablaEquipo");
    if (tbl != null) {
        for (var i = 0; i < tbl.rows.length; i++) {

            numregEqInv++;

        }
    }

    return numregEqInv;

}

activarControlCondicional = function () {
    var tipoOperacion = $('#cbTipoOpera').val();
    //activacion condicional para ENERGIA DEJADA DE ENTREGAR
    if (cbxIccheck2.checked && (tipoOperacion == 314)) {
        setTextoVisible('#ctnhorcoord', true);
        setTextoVisible('#ctnhorcoord2', true);
    } else {
        if ((tipoOperacion != 218)) {
            setTextoVisible('#ctnhorcoord', false);
            setTextoVisible('#ctnhorcoord2', false);
        }
    }

    //activacion condicional para RESTRICCIONES OPERATIVAS
    if (cbxindisp.checked && (tipoOperacion == 205)) {
        setTexto('#cbxindparc', 'Indisp. Parcial');
        setTexto('#lblindparc', 'Indisp. Parcial');
    } else {
        setTexto('#cbxindparc', '');
        setTexto('#lblindparc', '');
    }


    if ($('#cbxindparc').is(':checked') && $('#cbxindisp').is(':checked') && ($('#cbTipoOpera').val() == 205)) {
        setTexto('#txtgendisp', 'Gen. Disponible (MW)');
        setTexto('#lblgendisp', 'Gen. Disponible (MW)');
    } else {
        setTexto('#txtgendisp', '');
        setTexto('#lblgendisp', '');
    }
}


cargarPrevio = function () {

}


agregarEquipo = function () {
    famcodi = "0";
    modoIngreso = 'grupo';
    cargarBusquedaEquipo();
    openBusquedaEquipo();
    return;
}


openBusquedaEquipo = function () {
    //cargarBusquedaEquipo();
    $('#busquedaEquipo').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
    $('#txtFiltro').focus();
}
/*
openBusquedaEquipoFamilia = function () {

    $('#busquedaEquipoFamilia').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
    $('#txtFiltro').focus();

}*/

openBusquedaEquipoFamcodi = function (famcodi) {


    $.ajax({
        type: "POST",
        url: siteRoot + 'eventos/busqueda/index',
        data: { famcodi: famcodi },
        success: function (evt) {

            $('#busquedaEquipo').html(evt);
            openBusquedaEquipo();
        },
        error: function () {
            mostrarError();
        }
    });


}

cargarEquipos = function () {
    $.ajax({
        type: "POST",
        url: siteRoot + 'eventos/registro/equipos',
        success: function (evt) {
            $('#equiposInvolucrados').html(evt);
        },
        error: function () {
            mostrarError();
        }
    });
}


cargarBusquedaEquipo = function () {
    $.ajax({
        type: "POST",
        url: siteRoot + "eventos/busqueda/index",
        data: {},
        success: function (evt) {
            $('#busquedaEquipo').html(evt);

        },
        error: function (req, status, error) {
            mostrarError();
        }
    });
}


seleccionarEquipo = function (idEquipo, substacion, equipo, empresa, idEmpresa) {

    var tipoEquipo = '';

    $.ajax({
        type: 'POST',
        url: controller + 'seleccEquipo',
        dataType: 'json',
        data: { idEquipo: idEquipo },
        cache: false,
        success: function (result) {
            escribirFamilia(idEquipo, substacion, equipo, empresa, idEmpresa, result);

        },
        error: function () {
            mostrarError();
        }
    });

    $('#busquedaEquipo').bPopup().close();
    //$('#busquedaEquipoFamilia').bPopup().close();


}

escribirFamilia = function (idEquipo, substacion, equipo, empresa, idEmpresa, tipoEquipoDemHPDemFP) {

    var familiaDemHPDemFP = tipoEquipoDemHPDemFP.split(',');

    if (modoIngreso == 'unidad') {
        $('#hfEquicodi').val(idEquipo);
        $('#txtEmpresa').val(empresa);
        $('#txtUbicacion').val(substacion);
        $('#txtEquipo').val(equipo);

        var familiaDemHPDemFP = tipoEquipoDemHPDemFP.split(',');

        $('#txtTipoEquipo').val(familiaDemHPDemFP[0]);
        $('#txthp').val(familiaDemHPDemFP[1]);
        $('#txtfp').val(familiaDemHPDemFP[2]);

    } else {

        if (modoIngreso == 'grupo') {

            var tbl = document.getElementById("tablaEquipo");
            if (tbl != null) {
                for (var i = 0; i < tbl.rows.length; i++) {

                    if (tbl.rows[i].cells[0].innerHTML == idEquipo) {
                        return;
                    }

                }

                var a = "<tr id='tr" + idEquipo + "'>";
                a += "<td >" + idEquipo + "</td>";
                a += "<td>" + empresa + "</td>";
                a += "<td>" + substacion + "</td>";
                a += "<td>" + equipo + "</td>";
                a += "<td>" + familiaDemHPDemFP[0] + "</td>";

                a += "<td><a href='JavaScript:cambiarOpcion(" + idEquipo + ")'><span id='det'>S</span></a></td>";
                a += "<td style='text-align:center'>";
                a += "<a href='JavaScript:eliminarEquipo(" + idEquipo + ")'>";
                //a += "<img src='../../../../Content/Images/btn-cancel.png' />";
                //a += "<img src='" + siteRoot + "/Content/Images/btn-cancel.png' />";
                //a += "<img src='/Content/Images/btn-cancel.png' />";                
                a += "<img src='" + siteRoot + "/Content/Images/btn-cancel.png' />";
                a += "</a>";
                a += "</td>";
                a += "</tr>";

                $('#tablaEquipo').append(a);

            }
        }
    }
}

mostrarError = function () {
    alert("Ha ocurrido un error");
}


//detalle de operaciones varias.
cargardetalle = function (iccodi) {

    $.ajax({
        type: "POST",
        url: controller + "Equipos",
        data: { iccodi: iccodi },
        cache: false,
        success: function (evt) {
            $('#tblCentral2').html(evt);
        },
        error: function () {
            mostrarError();
        }
    });
}


eliminarEquipo = function (id) {
    $('#tr' + id).remove();
    return;
}


cambiarOpcion = function (idEquipo) {

    var tbl = document.getElementById("tablaEquipo");
    if (tbl != null) {
        for (var i = 0; i < tbl.rows.length; i++) {
            if (tbl.rows[i].cells[0].innerHTML == idEquipo) {
                var row = document.getElementById('tr' + idEquipo);
                var spans = row.getElementsByTagName("span");
                var span1 = spans[0].innerHTML;
                //cambia texto
                if (span1 == 'S') {
                    spans[0].innerHTML = 'N';
                } else {
                    spans[0].innerHTML = 'S';
                }

                return;
            }
        }
    }

    return;
}


mostrarAlerta = function (mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-alert');
    $('#mensaje').html(mensaje);
}


mostrarExito = function () {
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-exito');
    $('#mensaje').html('La operación se realizó con éxito.');
}


grabar = function () {
    var mensaje = validarRegistro();

    if (mensaje == "") {

        var eqinvol = obtenerTablaEquipo();

        var eqinvol2 = obtenerTablaGrupo();

        var gpsinvol = obtenerTablaGPS();

        $('#hfEquipoInvolucrado').val(eqinvol);

        $('#hfGrupoInvolucrado').val(eqinvol2);

        $('#hfStrAislado').val(gpsinvol[0]);
        $('#hfStrAisladoFlagPpal').val(gpsinvol[1]);

        $('#hfEvenclasecodi').val($('#cbHorizonte').val());

        $('#hfSubcausacodi').val($('#cbTipoOpera').val());

        $('#hfIccheck1').val('N');
        if ($('#cbxIccheck1').is(':checked')) $('#hfIccheck1').val('S');

        $('#hfIccheck2').val('N')
        if ($('#cbxIccheck2').is(':checked')) $('#hfIccheck2').val('S');

        $('#hfIccheck3').val('N')
        if ($('#cbxindisp').is(':checked')) $('#hfIccheck3').val('S');

        $('#hfIccheck4').val('N')
        if ($('#cbxindparc').is(':checked')) $('#hfIccheck4').val('S');

        $('#hfIccheckCuadro').val($('input[name="cbxIccheckCuadro"]:checked').val());

        $.ajax({
            type: 'POST',
            url: controller + "grabar",
            dataType: 'json',
            data: $('#formEvento').serialize(),
            success: function (result) {
                if (result > 1) {
                    mostrarExito();
                    $('#txthp').val("");
                    $('#txtfp').val("");

                    $('#hficcodi').val(result);

                    grabarEvento1(result);

                }
                if (result == -1) {
                    mostrarError();
                }
            },
            error: function () {
                mostrarError();
            }
        });
    } else {
        mostrarAlerta(mensaje);
    }
}


grabarEvento1 = function (id) {

    var genIni = $('#cbxgenevenini').is(':checked');
    var genFin = $('#cbxgenevenfin').is(':checked');

    $.ajax({
        type: 'POST',
        url: controller + "grabarevento",
        dataType: 'json',
        data: { id: id, genIni: genIni, genFin: genFin },
        success: function (result) {
            if (result >= 1) {
                mostrarExito();
                document.location.href = controller;
            }
            if (result == -1) {
                mostrarError();
            }
        }
    });
}


validarNumero = function (valor) {
    return !isNaN(parseFloat(valor)) && isFinite(valor);
}

validarRegistro = function () {

    var mensaje = "<ul>";
    var flag = true;

    if ($('#hfEvenclasecodi').val() == '') {
        mensaje = mensaje + "<li>Seleccione Horizonte.";
        flag = false;
    }

    if ($('#hfSubcausacodi').val() == '') {
        mensaje = mensaje + "<li>Seleccione Tipo de Operación.";
        flag = false;
    }

    if ($('#hfEquicodi').val() == '') {
        mensaje = mensaje + "<li>Seleccione el equipo.</li>";
        flag = false;
    }


    if ($('#valhorini').val() == '') {
        mensaje = mensaje + "<li>Ingrese la hora inicial.</li>";
        flag = false;
    }
    else {
        if (!$('#valhorini').inputmask("isComplete")) {
            mensaje = mensaje + "<li>Ingrese hora inicial.</li>";
            flag = false;
        }
    }

    if ($('#valhorfin').val() == '') {
        mensaje = mensaje + "<li>Ingrese la hora final.</li>";
        flag = false;
    }
    else {
        if (!$('#valhorfin').inputmask("isComplete")) {
            mensaje = mensaje + "<li>Ingrese hora final.</li>";
            flag = false;
        }
    }

    var valor1 = $('#txtvalor1').val();

    if (valor1 != "") {
        if (!validarNumero(valor1)) {
            mensaje = mensaje + "<li>valor ingresado no es numérico</li>";
            flag = false;
        }
    }

    var fhIni = $('#valhorini').val();
    var fhFin = $('#valhorfin').val();

    var ldtIini = convertDateTime(fhIni);
    var ldtFin = convertDateTime(fhFin);

    if (ldtFin < ldtIini) {
        mensaje = mensaje + "<li>corregir rango de fechas debe ser Inicio menor Final</li>";
        flag = false;
    }

    if (obtenerFecha(ldtIini) + " " + obtenerHora(ldtIini) == obtenerFecha(ldtFin) + " " + obtenerHora(ldtFin)) {
        mensaje = mensaje + "<li>Fecha-hora inicial y final no pueden ser iguales.</li>";
        flag = false;
    }

    var diff = (Math.abs(ldtFin - ldtIini)) / 1000;

    if (diff > 86400) {
        mensaje = mensaje + "<li>Diferencia de fechas debe ser máximo de un día</li>";
        flag = false;
    }

    if (obtenerFecha(ldtIini) != obtenerFecha(ldtFin) && obtenerHora(ldtFin) != "00:00:00") {
        mensaje = mensaje + "<li>" + "Fecha máxima permitida: " + obtenerFecha(addDays(ldtIini, 1)) + " 00:00:00" + "</li>";
        flag = false;
    }

    if ($('#hfEquicodi').val() == '' || $('#hfEquicodi').val() == '-1' || $('#hfEquicodi').val() == '0') {
        mensaje = mensaje + "<li>" + "Debe seleccionar equipo" + "</li>";
        flag = false;
    }

    if (flag) mensaje = "";
    return mensaje;
}


function obtenerFecha(fechahora) {
    var l1 = fechahora.getDate() + "/" + (fechahora.getMonth() + 1) + "/" + fechahora.getFullYear();
    return l1;
}


function addDays(fechahora, days) {
    var fh1 = fechahora;
    fh1.setSeconds(fh1.getSeconds() + 86400 * days);
    return fh1;
}


function obtenerHora(fechahora) {
    var l1 = (fechahora.getHours() < 10 ? '0' : '') + fechahora.getHours() + ":" + (fechahora.getMinutes() < 10 ? '0' : '') + fechahora.getMinutes() + ":" + (fechahora.getSeconds() < 10 ? '0' : '') + fechahora.getSeconds();
    l1 = l1.toString();
    return l1;
}


function convertDateTime(fechahora) {

    var fh = fechahora.toString().split(" ");
    var fecha = fh[0].split("/");
    var hora = fh[1].split(":");
    var dd = fecha[0];
    var mm = fecha[1] - 1;
    var yyyy = fecha[2];
    var h = hora[0];
    var m = hora[1];
    var s = parseInt(hora[2]);

    return new Date(yyyy, mm, dd, h, m, s);
}


function convertDate(fechahora) {
    //fecha hora en formato dd/mm/yyyy hh:mm:ss
    var fh = fechahora.toString().split(" ");
    var fecha = fh[0].split("/");
    var dd = fecha[0];
    var mm = fecha[1] - 1;
    var yyyy = fecha[2];

    return new Date(yyyy, mm, dd, 0, 0, 0);
}


obtenerTablaEquipo = function () {

    var rows = document.getElementById("tablaEquipo").getElementsByTagName("tr").length;
    var cad = "";

    var tbl = document.getElementById("tablaEquipo");
    if (tbl != null) {
        for (var i = 1; i < tbl.rows.length; i++) {
            var ideq = tbl.rows[i].cells[0].innerHTML;
            var row = document.getElementById('tr' + ideq);
            var spans = row.getElementsByTagName("span");
            var span1 = spans[0].innerHTML;
            cad += ideq + "," + span1 + ",";
        }
    }

    return cad;
}


cargarTipoOperacion = function (tipo) {

    setTextoInvisible();

    var tipo1 = parseInt(tipo);

    //var ancho = (tipo1 == 205 ? 200 : 416);
    //window.document.getElementById('formHoras').width = ancho;

    //alert(ancho + "*" + window.document.getElementById('formHoras').width);
    switch (tipo1) {

        case 217: //CONGESTION EN GASODUCTO DE CAMISEA
            setTextoVisible('#txtDetalleAdicional1', true);
            setTexto('#lblDetalleAdicional1', 'Descripción');
            break;

        case 343: //SOBRECARGA
            setTextoVisible('#txtDetalleAdicional1', true);
            setTexto('#lblDetalleAdicional1', 'Observación');
            break;

        case 201: //CONGESTION
            setTextoVisible('#txtDetalleAdicional2', true);
            setTexto('#lblDetalleAdicional2', 'Observaciones');
            setTextoVisible('#tblCentral', true);
            setTextoVisible('#tblCentral2', true);
            setTextoVisible('#btnSeleccionar2', true);
            setTextoVisible('#btnAgregarGru', true);

            break;

        case 40105: //SITUACIÓN EXCEPCIONAL
            setTextoVisible('#txtDetalleAdicional2', true);
            setTexto('#lblDetalleAdicional2', 'Observaciones');
            setTextoVisible('#tblCentral', true);
            setTextoVisible('#tblCentral2', true);
            setTextoVisible('#btnSeleccionar2', true);
            setTextoVisible('#btnAgregarGru', true);

            break;

        case 218: //DESCARGA LAGUNAS
            setTextoVisible('#txtDetalleAdicional1', true);
            setTexto('#lblDetalleAdicional1', 'Observaciones');
            setTexto('#cbxIccheck1', 'Cierre/Disminución');
            setTexto('#lblIccheck1', 'Cierre/Disminución');
            setTexto('#txtvalor1', 'Caudal (m3/s)');
            setTexto('#lblvalor1', 'Caudal (m3/s)');
            setTexto('#txthorcoord', 'Hora Coordinación');

            setTexto('#lblhorcoord', 'Hora Coordinación');
            setTextoVisible('#ctnhorcoord', true);
            break;

        case 222: //DISPONIBILIDAD DE GAS
            setTextoVisible('#txtDetalleAdicional1', true);
            setTexto('#lblDetalleAdicional1', 'Observaciones');
            setTexto('#txtvalor1', 'Mm3');
            setTexto('#lblvalor1', 'Mm3');
            break;

        case 314: //ENERGIA DEJADA DE INYECTAR RER

            setTexto('#cbxIccheck2', 'Aprobación');
            setTexto('#lbliccheck2', 'Aprobación');
            setTexto('#txthorcoord', 'Aprob. Hora Inicio');
            setTexto('#lblhorcoord', 'Aprob. Hora Inicio');
            setTexto('#txthorcoord2', 'Aprob. Hora Fin');
            setTexto('#lblhorcoord2', 'Aprob. Hora Fin');
            setTextoVisible('#ctnhorcoord', true);
            break;

        case 219: //INTERCONEXIONES INTERNACIONALES
            setTextoVisible('#txtDetalleAdicional1', true);
            setTexto('#lblDetalleAdicional1', 'País importador');
            setTextoVisible('#txtDetalleAdicional2', true);
            setTexto('#lblDetalleAdicional2', 'País exportador');
            setTextoVisible('#txtDetalleAdicional3', true);
            setTexto('#lblDetalleAdicional3', 'Observación');
            break;

        case 202: //OPERACION DE CALDEROS
            setTextoVisible('#txtDetalleAdicional1', true);
            setTexto('#lblDetalleAdicional1', 'Descripción');
            setTexto('#cbxgenevenini', 'Generar Evento');
            setTexto('#lblgenevenini', 'Generar Evento');
            setTexto('#cbxgenevenfin', 'Generar Evento');
            setTexto('#lblgenevenfin', 'Generar Evento');
            break;

        case 208: //POR PRUEBAS (no termoelectrico)
            setTextoVisible('#txtDetalleAdicional1', true);
            setTexto('#lblDetalleAdicional1', 'Descripción');
            setTexto('#cbxgenevenini', 'Generar Evento');
            setTexto('#lblgenevenini', 'Generar Evento');
            setTexto('#cbxgenevenfin', 'Generar Evento');
            setTexto('#lblgenevenfin', 'Generar Evento');
            break;

        case 221: //REDUCCION DE CARGA
            setTextoVisible('#txtDetalleAdicional1', true);
            setTexto('#lblDetalleAdicional1', 'Observación');
            setTexto('#txtvalor1', 'MW');
            setTexto('#lblvalor1', 'MW');
            break;


        case 203: //REGULACION DE TENSION
            setTextoVisible('#txtDetalleAdicional1', true);
            setTexto('#lblDetalleAdicional1', 'Motivo');
            setTexto('#cbxgenevenini', 'Generar Evento');
            setTexto('#lblgenevenini', 'Generar Evento');
            setTexto('#cbxgenevenfin', 'Generar Evento');
            setTexto('#lblgenevenfin', 'Generar Evento');
            break;

        case 220: //REGULACION ESPECIAL DE FRECUENCIA
            setTextoVisible('#txtDetalleAdicional1', true);
            setTexto('#lblDetalleAdicional1', 'Observaciones');
            setTexto('#txtvalor1', 'Potencia asignada (MW)');
            setTexto('#lblvalor1', 'Potencia asignada (MW)');
            setTexto('#cbxIccheck1', 'Sist. Aislado?');
            setTexto('#lblIccheck1', 'Sist. Aislado?');
            break;

        case 204: //REGULACION PRIMARIA DE FRECUENCIA
            setTextoVisible('#txtDetalleAdicional1', true);
            setTexto('#lblDetalleAdicional1', 'Observaciones');
            setTexto('#txtvalor1', 'Potencia asignada (MW)');
            setTexto('#lblvalor1', 'Potencia asignada (MW)');
            setTexto('#cbxIccheck1', 'Sist. Aislado?');
            setTexto('#lblIccheck1', 'Sist. Aislado?');
            break;

        case 209: //REGULACION SECUNDARIA DE FRECUENCIA
            setTextoVisible('#txtDetalleAdicional1', true);
            setTexto('#lblDetalleAdicional1', 'Observaciones');
            setTexto('#txtvalor1', 'Potencia asignada (MW)');
            setTexto('#lblvalor1', 'Potencia asignada (MW)');
            setTexto('#cbxIccheck1', 'Sist. Aislado?');
            setTexto('#lblIccheck1', 'Sist. Aislado?');
            setTexto('#cbxIccheck2', 'Unid. Regulación');
            setTexto('#lbliccheck2', 'Unid. Regulación');
            break;

        case 215: //RESERVA MAP-COES
            setTexto('#txtvalor1', 'MW');
            setTexto('#lblvalor1', 'MW');
            break;

        case 213: //RESTRICCION DE SUMINISTROS
            setTextoVisible('#txtDetalleAdicional1', true);
            setTexto('#lblDetalleAdicional1', 'Observación');
            setTexto('#txtvalor1', 'MW');
            setTexto('#lblvalor1', 'MW');
            setTexto('#txthp', 'Hora Punta (MW)');
            setTexto('#lblhp', 'Hora Punta (MW)');
            setTexto('#txtfp', 'Fuera de Punta (MW)');
            setTexto('#lblfp', 'Fuera de Punta (MW)');
            setTextoVisible('#ctnhp', true);
            setTextoVisible('#ctnhp2', true);
            setTextoVisible('#ctnfp', true);
            setTextoVisible('#ctnfp2', true);
            break;


        case 205: //RESTRICCIONES OPERATIVAS

            setTextoVisible('#txtDetalleAdicional1', true);
            setTexto('#lblDetalleAdicional1', 'Descripción');
            setTexto('#txthorcoord', 'Inicio restricción');
            setTexto('#lblhorcoord', 'Inicio restricción');
            setTexto('#cbxindisp', 'Indisponibilidad');
            setTexto('#lblindisp', 'Indisponibilidad');
            setTexto('#cbxgenevenini', 'Generar Evento');
            setTexto('#lblgenevenini', 'Generar Evento');
            setTexto('#cbxgenevenfin', 'Generar Evento');
            setTexto('#lblgenevenfin', 'Generar Evento');
            setTexto('#cbxIccheck1', 'No restringe en Pruebas Aleatorias');
            setTexto('#lblIccheck1', 'No restringe en Pruebas Aleatorias');
            setTextoVisible('#ctnhorcoord', true);
            break;

        case 206: //SISTEMAS AISLADOS

            setTextoVisible('#txtDetalleAdicional2', true);
            setTexto('#lblDetalleAdicional2', 'Motivo');
            setTextoVisible('#txtDetalleAdicional3', true);
            setTexto('#lblDetalleAdicional3', 'Subsistema aislado');
            setTexto('#cbxgenevenini', 'Generar Evento');
            setTexto('#lblgenevenini', 'Generar Evento');
            setTexto('#cbxgeneveninicarga', 'Generar Evento');
            setTexto('#lblgeneveninicarga', 'Generar Evento');
            setTexto('#cbxgenevenfin', 'Generar Evento');
            setTexto('#lblgenevenfin', 'Generar Evento');
            setTextoVisible('#tblCentral', true);
            setTextoVisible('#tblCentral2', true);
            setTextoVisible('#btnSeleccionar2', true);
            setTextoVisible('#btnAgregarGps', true);
            setTextoVisible('#btnGps', true);
            setTextoVisible('#ctnhoraisla', true);
            setTextoVisible('#ctnhoraisla2', true);
            setTextoVisible('#cbxIccheckCuadro1', true);
            setTextoVisible('#cbxIccheckCuadro2', true);
            setTextoVisible('#cbxIccheckCuadro3', true);
            setTextoVisible('#cbxIccheckCuadro4', true);
            setTexto('#lblIccheckCuadro1', 'Cuadro N° 1. Según literal j) del numeral 5.1.4. de la BMNTCSE');
            setTexto('#lblIccheckCuadro2', 'Cuadro N° 2. No incluidos en el cumplimiento del literal j) numeral 5.1.4. de la BMNTCSE');
            setTexto('#lblIccheckCuadro3', 'Cuadro N° 3. No incluidos en el literal j) del numeral 5.1.4 de la BMNTCSE < 10 min');
            setTexto('#lblIccheckCuadro4', 'Ninguno');
            break;

        case 216: //SISTEMAS AISLADOS F/S

            setTextoVisible('#txtDetalleAdicional2', true);
            setTexto('#lblDetalleAdicional2', 'Motivo');
            setTextoVisible('#txtDetalleAdicional3', true);
            setTexto('#lblDetalleAdicional3', 'Área operativa fuera de servicio');
            break;

        case 212: //VARIACION DE DEMANDA CLIENTE LIBRE >10 MW
            setTextoVisible('#txtDetalleAdicional1', true);
            setTexto('#lblDetalleAdicional1', 'Motivo');
            setTexto('#txtvalor1', 'Potencia (MW)');
            setTexto('#lblvalor1', 'Potencia (MW)');
            setTexto('#txthp', 'Hora Punta (MW)');
            setTexto('#lblhp', 'Hora Punta (MW)');
            setTexto('#txtfp', 'Fuera de Punta (MW)');
            setTexto('#lblfp', 'Fuera de Punta (MW)');
            setTextoVisible('#ctnhp', true);
            setTextoVisible('#ctnhp2', true);
            setTextoVisible('#ctnfp', true);
            setTextoVisible('#ctnfp2', true);
            break;

        case 210: //VENTEO DE GAS
            setTexto('#txtvalor1', 'MMPCD');
            setTexto('#lblvalor1', 'MMPCD');
            break;

        case 207: //VERTIMIENTO DE EMBALSES Y PRESAS
            setTexto('#txtvalor1', 'Caudal promedio (m3/s)');
            setTexto('#lblvalor1', 'Caudal promedio (m3/s)');
            break;

        case 349: //SOBRECARGA
            setTextoVisible('#txtDetalleAdicional1', true);
            setTexto('#lblDetalleAdicional1', 'Observación');
            setTexto('#txtvalor1', 'MW');
            setTexto('#lblvalor1', 'MW');
            break;

        default:

    }


    activarControlCondicional();
    $('#txthp').val("");
    $('#txtfp').val("");

    //var ancho = (tipo1 == 205 ? 200 : 416);
    //window.document.getElementById('formHoras').width = ancho;
}


function setTextoInvisible() {

    setTexto('#txtDetalleAdicional1', '');
    setTexto('#lblDetalleAdicional1', '');
    setTexto('#txtDetalleAdicional2', '');
    setTexto('#lblDetalleAdicional2', '');
    setTexto('#txtDetalleAdicional3', '');
    setTexto('#lblDetalleAdicional3', '');
    setTexto('#cbxIccheck1', '');
    setTexto('#lblIccheck1', '');
    setTexto('#cbxIccheck2', '');
    setTexto('#lbliccheck2', '');
    setTexto('#txtvalor1', '');
    setTexto('#lblvalor1', '');
    setTexto('#txthorcoord', '');
    setTexto('#lblhorcoord', '');
    setTexto('#cbxgenevenini', '');
    setTexto('#lblgenevenini', '');
    setTexto('#cbxgeneveninicarga', '');
    setTexto('#lblgeneveninicarga', '');
    setTexto('#cbxgenevenfin', '');
    setTexto('#lblgenevenfin', '');
    setTexto('#cbxindisp', '');
    setTexto('#lblindisp', '');
    setTexto('#cbxindparc', '');
    setTexto('#lblindparc', '');
    setTexto('#txtgendisp', '');
    setTexto('#lblgendisp', '');
    setTexto('#txthp', '');
    setTexto('#lblhp', '');
    setTexto('#txtfp', '');
    setTexto('#lblfp', '');
    setTexto('#tblCentral', '');
    setTexto('#tblCentral2', '');
    setTexto('#txthorcoord2', '');
    setTexto('#lblhorcoord2', '');
    setTextoVisible('#ctnhorcoord', false);
    setTextoVisible('#ctnhp', false);
    setTextoVisible('#ctnhp2', false);
    setTextoVisible('#ctnfp', false);
    setTextoVisible('#ctnfp2', false);
    setTextoVisible('#ctnhorcoord2', false);
    setTextoVisible('#ctnhoraisla', false);
    setTextoVisible('#ctnhoraisla2', false);
    setTextoVisible('#btnSeleccionar2', false);
    setTextoVisible('#btnAgregarGru', false);
    setTextoVisible('#cbxIccheckCuadro1', false);
    setTextoVisible('#cbxIccheckCuadro2', false);
    setTextoVisible('#cbxIccheckCuadro3', false);
    setTextoVisible('#cbxIccheckCuadro4', false);
    setTexto('#lblIccheckCuadro1', '');
    setTexto('#lblIccheckCuadro2', '');
    setTexto('#lblIccheckCuadro3', '');
    setTexto('#lblIccheckCuadro4', '');

}


function setTextoVisible(psId, pbVisible) {
    var str1 = "";
    if (pbVisible) {
        str1 = "inline";
    }
    else {
        str1 = "none";
    }

    $(psId).css("display", str1);

}


function setTexto(psId, psTexto) {
    if (psTexto != '') {

        $(psId).text(psTexto);
        str1 = "inline";
    } else {
        str1 = "none";
    }

    $(psId).css("display", str1);

}

cargarBusquedaEquipo = function () {
    $.ajax({
        type: "POST",
        url: siteRoot + "eventos/busqueda/index",
        data: {},
        success: function (evt) {
            $('#busquedaEquipo').html(evt);

        },
        error: function (req, status, error) {
            mostrarError();
        }
    });
}

cargardetalleGrupos = function (iccodi) {

    $.ajax({
        type: "POST",
        url: controller + 'GruposByIccodi',

        data: { iccodi: iccodi },

        success: function (aData) {
            //   if (aData.LisGrupoDespacho == '-1') {
            //       alert('Ha ocurrido un error:' + aData.Mensaje);
            //  }

            //     else {

            //Grupo de despacho
            var htmlTable = "<table id='resultadoTablaGrupoInvolucrado' class='pretty tabla-icono'><thead><tr> <th width='80px'>ID</th> <th width='200px'>Empresa</th> <th width='150px'>Grupo de Despacho</th> <th width='80px'>Eliminar</th> </tr ></thead > ";
            var tbody = '';

            for (var i = 0; i < aData.ListaCongestion.length; i++) {
                var item = aData.ListaCongestion[i];

                for (var j = 0; j < aData.ListaGrupoDespachoEdit.length; j++) {

                    var item2 = aData.ListaGrupoDespachoEdit[j];
                    if (item2 != null) {
                        if (item.Grupocodi == item2.Grupocodi && item2.Grupocodi != null) {
                            var fila = "<tr id='tr" + item.Grupocodi + "'>";
                            fila += "<td style='text-align: center; padding-top: 7px; padding-bottom: 7px;'>" + item.Grupocodi + "</td>";
                            fila += "<td style='text-align: center'>" + item2.Emprnomb + "</td>";
                            fila += "<td style='text-align: center'>" + item2.Gruponomb + "</td>";

                            // fila += "<td style='text-align: center'>" + item.CongdefechainiDesc + "</td>";
                            // fila += "<td style='text-align: center'>" + item.CongdefechafinDesc + "</td>";
                            fila += "<td style='text-align: center'>";
                            fila += "<a href='JavaScript:eliminarEquipo(" + item.Grupocodi + ")'>";
                            fila += "<img src='" + siteRoot + "Content/Images/btn-cancel.png' />";
                            fila += "</a>";
                            fila += "</td>";

                            fila += "</tr>";
                            tbody += fila;
                        }
                    }
                }
            }

            htmlTable += tbody + "<tbody></tbody></table>";
            $('#div_tabla_pr_Grupo').html(htmlTable);

            $("#div_tabla_pr_Grupo").show();
        },
        error: function (req, status, error) {
            mostrarError();
        }
    });
}

seleccionarGrupo = function (codigo, empresa, central, ubicacion) {

    var fhIni = $('#valhoriniGrupo').val();
    var fhFin = $('#valhorFinGrupo').val();

    var ldtIini = convertDateTime(fhIni);
    var ldtFin = convertDateTime(fhFin);

    var diff = (Math.abs(ldtFin - ldtIini)) / 1000;

    if (diff > 86400) {
        alert('Diferencia de fechas debe ser máximo de un día');
    }

    else {
        $.ajax({
            type: 'POST',
            url: controller + 'BuscarGrupo',
            dataType: 'json',
            data: { codigo: codigo },
            cache: false,
            success: function (result) {
                escribirGrupo(codigo, empresa, central, ubicacion);
            },
            error: function () {
                mostrarError();
            }
        });
        $('#busquedaDespachoPopup').bPopup().close();
    }
}

ListGrupoDespacho = function () {
    $.ajax({
        type: 'POST',
        url: controller + 'GrupoListado',
        success: function (aData) {
            if (aData.ListaGrupoDespacho == '-1') {
                alert('Ha ocurrido un error:' + result.Mensaje);
            }

            else {
                var htmlTable = "<table id='tabla_pr_concepto' class='pretty tabla-icono' style='width:550px'><thead><tr> <th width='12%'>Codigo</th> <th width='12%'>Empresa</th> <th width='12%'>Grupo Despacho</th></tr></thead>";
                var tbody = '';

                for (var i = 0; i < aData.ListaGrupoDespacho.length; i++) {
                    var item = aData.ListaGrupoDespacho[i];

                    var claseFila = "";
                    if (item.GrupoEstado != 'S') { claseFila = "clase_eliminado"; }

                    var fila = "<tr class='" + claseFila + "' onclick=\"seleccionarGrupo(" + item.Grupocodi + ",'" + item.Emprnomb + "','" + item.Gruponomb + "');\" style='cursor:pointer' >";
                    fila += "<td style='text-align: center; padding-top: 7px; padding-bottom: 7px;'>" + item.Grupocodi + "</td>";
                    fila += "<td style='text-align: center'>" + item.Emprnomb + "</td>";
                    fila += "<td style='text-align: left'>" + item.Gruponomb + "</td>";
                    fila += "</tr>";
                    tbody += fila;
                }

                htmlTable += tbody + "<tbody></tbody></table>";
                $('#div_tabla_pr_concepto').html(htmlTable);
                $('#tabla_pr_concepto').dataTable({
                    "sPaginationType": "full_numbers",
                    "destroy": "true",
                    "bInfo": false,
                    "bLengthChange": false,
                    "sDom": 'fpt',
                    "ordering": true,
                    "order": [[2, "asc"]],
                    "iDisplayLength": 15
                });
                $("#div_tabla_pr_concepto").show();

                openBusquedaDespacho();
            }
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

openBusquedaDespacho = function () {
    $('#busquedaDespachoPopup').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown',
        async: true,
        modalClose: false
    });
}

escribirGrupo = function (codigo, empresa, generacion, ubicacion) {

    var tbl = document.getElementById("resultadoTablaGrupoInvolucrado");
    if (tbl != null) {
        for (var i = 0; i < tbl.rows.length; i++) {

            if (tbl.rows[i].cells[0].innerHTML == codigo) {
                return;
            }

        }
        var a = "<tr id='tr" + codigo + "'>";
        a += "<td >" + codigo + "</td>";
        a += "<td>" + empresa + "</td>";
        a += "<td>" + generacion + "</td>";
        a += "<td style='text-align:center'>";
        a += "<a href='JavaScript:eliminarEquipo(" + codigo + ")'>";
        a += "<img  src='" + siteRoot + "Content/Images/btn-cancel.png' />";

        a += "</a>";
        a += "</td>";
        a += "</tr>";
        $('#resultadoTablaGrupoInvolucrado').append(a);
    }
}

obtenerTablaGrupo = function () {

    var rows = document.getElementById("resultadoTablaGrupoInvolucrado").getElementsByTagName("tr").length;
    var cad = [];

    var tbl = document.getElementById("resultadoTablaGrupoInvolucrado");
    if (tbl != null) {
        for (var i = 1; i < tbl.rows.length; i++) {
            var ideq = tbl.rows[i].cells[0].innerHTML;
            cad.push(ideq);
        }
    }

    return cad.join();
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Gps
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
function cargardetalleGps(iccodi) {

    $.ajax({
        type: "POST",
        url: controller + 'GpsByIccodi',

        data: { iccodi: iccodi },

        success: function (aData) {
            //Html de los gps guardados para el iccodi
            var htmlTable = "<table id='resultadoTablaGps' class='pretty tabla-icono'><thead><tr> <th width='80px'>ID</th> <th width='200px'>Nombre</th> <th width='150px'>Código Osinerg</th> <th width='80px'>Principal</th> <th width='80px'>Eliminar</th> </tr ></thead > ";
            var tbody = '';

            for (var i = 0; i < aData.ListaGpsAislado.length; i++) {
                var item = aData.ListaGpsAislado[i];

                var fila = "<tr id='tr" + item.Gpscodi + "'>";
                fila += "<td style='text-align: center; padding-top: 7px; padding-bottom: 7px;'>" + item.Gpscodi + "</td>";
                fila += "<td style='text-align: center'>" + item.Gpsnombre + "</td>";
                fila += "<td style='text-align: center'>" + item.Gpsosinerg + "</td>";
                fila += "<td style='text-align: center'>" + (item.Gpsaisprincipal === 1 ? 'SÍ' : '') + "</td>";

                fila += "<td style='text-align: center'>";
                fila += "<a href='JavaScript:eliminarEquipo(" + item.Gpsaiscodi + ")'>";
                fila += "<img src='" + siteRoot + "Content/Images/btn-cancel.png' />";
                fila += "</a>";
                fila += "</td>";

                fila += "</tr>";
                tbody += fila;
            }

            htmlTable += tbody + "<tbody></tbody></table>";
            $('#div_tabla_Gps').html(htmlTable);

            $("#div_tabla_Gps").show();

            listaGps = aData.ListaGps;

        },
        error: function (req, status, error) {
            mostrarError();
        }
    });
}

function popupGps() {
    //data ya generado en 
    var gpsinvol = obtenerTablaGPS();
    var arrayGps = JSON.parse("[" + gpsinvol[0] + "]");
    var existePpal = gps_existePrincipal(gpsinvol[1]);
    var atributoDisabled = existePpal ? ' disabled ' : '';

    //Html de la tabla que lista todos los gps de BD
    htmlTable = "<table id='tabla_popup_gps' class='pretty tabla-icono' style='width:550px'><thead><tr> <th width='12%'>Seleccionar <br/>Principal</th> <th width='12%'>Codigo</th> <th width='12%'>Nombre</th> <th width='12%'>Código Osinerg</th></tr></thead>";
    tbody = '';

    for (i = 0; i < listaGps.length; i++) {
        item = listaGps[i];


        if (!gps_existeGps(item.Gpscodi, arrayGps)) {
            fila = "<tr class='' onclick=\"seleccionarGps(" + item.Gpscodi + ",'" + item.Nombre + "','" + item.Gpsosinerg + "');\" style='cursor:pointer' >";
            fila += "<td> <input type='radio' name='gpsppal' value='" + item.Gpscodi + "' " + atributoDisabled + "></td>";
            fila += "<td style='text-align: center; padding-top: 7px; padding-bottom: 7px;'>" + item.Gpscodi + "</td>";
            fila += "<td style='text-align: center'>" + item.Nombre + "</td>";
            fila += "<td style='text-align: center'>" + item.Gpsosinerg + "</td>";
            fila += "</tr>";
            tbody += fila;
        }
    }

    htmlTable += tbody + "<tbody></tbody></table>";
    $('#div_tabla_popup_gps').html(htmlTable);
    $('#tabla_popup_gps').dataTable({
        "sPaginationType": "full_numbers",
        "destroy": "true",
        "bInfo": false,
        "bLengthChange": false,
        "sDom": 'ft',
        "ordering": true,
        "order": [[2, "asc"]],
        "iDisplayLength": 15
    });

    $("#div_tabla_popup_gps").show();

    openBusquedaGps();
}

function openBusquedaGps() {
    $('#busquedaGpsPopup').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown',
        async: true,
        modalClose: false
    });
}

function seleccionarGps(Gpscodi, Gpsnombre, Gpsosinerg) {

    var tbl = document.getElementById("resultadoTablaGps");
    if (tbl != null) {
        for (var i = 0; i < tbl.rows.length; i++) {
            if (tbl.rows[i].cells[0].innerHTML == Gpscodi) {
                return;
            }
        }

        var idGpscodiSelec = parseInt($('input:radio[name=gpsppal]:checked').val()) || 0;

        var a = "<tr id='tr" + Gpscodi + "'>";
        a += "<td>" + Gpscodi + "</td>";
        a += "<td>" + Gpsnombre + "</td>";
        a += "<td>" + Gpsosinerg + "</td>";
        a += "<td style='text-align: center'>" + (idGpscodiSelec === Gpscodi ? 'SÍ' : '') + "</td>";
        a += "<td style='text-align:center'>";
        a += "<a href='JavaScript:eliminarEquipo(" + Gpscodi + ")'>";
        a += "<img  src='" + siteRoot + "Content/Images/btn-cancel.png' />";

        a += "</a>";
        a += "</td>";
        a += "</tr>";
        $('#resultadoTablaGps').append(a);

        $('#busquedaGpsPopup').bPopup().close();
    }
}

function obtenerTablaGPS() {

    var rows = document.getElementById("resultadoTablaGps").getElementsByTagName("tr").length;
    var cad = [];
    var flag = [];
    var matrizTabla = [];

    var tbl = document.getElementById("resultadoTablaGps");
    if (tbl != null) {
        for (var i = 1; i < tbl.rows.length; i++) {
            var ideq = tbl.rows[i].cells[0].innerHTML;
            cad.push(ideq);

            var flageq = tbl.rows[i].cells[3].innerHTML == 'SÍ' ? 1 : 0;
            flag.push(flageq);
        }
    }

    matrizTabla.push(cad);
    matrizTabla.push(flag);

    return matrizTabla;
}

function gps_existeGps(id, listaCodSelec) {
    if (id !== undefined && id != null && listaCodSelec !== undefined && listaCodSelec != null) {
        for (var i = 0; i < listaCodSelec.length; i++) {
            if (listaCodSelec[i] == id)
                return true;
        }
    }

    return false;
}

function gps_existePrincipal(listaCodSelec) {
    var suma = 0;
    if (listaCodSelec !== undefined && listaCodSelec != null) {
        for (var i = 0; i < listaCodSelec.length; i++) {
            suma += listaCodSelec[i];
        }
    }

    return suma;
}