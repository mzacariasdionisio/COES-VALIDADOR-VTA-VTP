var controlador = siteRoot + 'gestioneoepo/estudioeo/';

$(document).ready(function () {
    $('#mapa').html("asas");

    ListarConfiguraciones();
    $("#cboPR").change(function () {
        LLenarInputsConfig();
    });

    $('#Emprcoditi').multipleSelect({
        filter: true,
        selectAll: false
    })



    $('#PuntCodi').multipleSelect({
        filter: true,
        selectAll: false
    })


    $('#tab-container').easytabs();
    //$('#Esteofechaini').Zebra_DatePicker({});
    //$('#Esteofechafin').Zebra_DatePicker({});
    //$('#Esteoalcancefechaini').Zebra_DatePicker({});
    //$('#Esteoalcancefechafin').Zebra_DatePicker({});
    //$('#Esteoverifechaini').Zebra_DatePicker({});
    //$('#Esteoverifechafin').Zebra_DatePicker({});

    $('#Esteofechaini').inputmask({
        mask: "1/2/y",
        placeholder: "dd/mm/yyyy",
        alias: "datetime"
    });

    $('#Esteofechaini').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#Esteofechaini').val(date);
        }
    });

    $('#Esteofechafin').inputmask({
        mask: "1/2/y",
        placeholder: "dd/mm/yyyy",
        alias: "datetime"
    });

    $('#Esteofechafin').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#Esteofechafin').val(date);
        }
    });

    $('#Esteoalcancefechaini').inputmask({
        mask: "1/2/y",
        placeholder: "dd/mm/yyyy",
        alias: "datetime"
    });

    $('#Esteoalcancefechaini').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#Esteoalcancefechaini').val(date);
        }
    });

    $('#Esteoalcancefechafin').inputmask({
        mask: "1/2/y",
        placeholder: "dd/mm/yyyy",
        alias: "datetime"
    });

    $('#Esteoalcancefechafin').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#Esteoalcancefechafin').val(date);
        }
    });

    $('#Esteoverifechaini').inputmask({
        mask: "1/2/y",
        placeholder: "dd/mm/yyyy",
        alias: "datetime"
    });

    $('#Esteoverifechaini').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#Esteoverifechaini').val(date);
        }
    });

    $('#Esteoverifechafin').inputmask({
        mask: "1/2/y",
        placeholder: "dd/mm/yyyy",
        alias: "datetime"
    });

    $('#Esteoverifechafin').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#Esteoverifechafin').val(date);
        }
    });

    $('#Esteofechaconexion').inputmask({
        mask: "1/2/y",
        placeholder: "dd/mm/yyyy",
        alias: "datetime"
    });

    $('#Esteofechaconexion').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#Esteofechaconexion').val(date);
        }
    });

    $('#Esteofechaopecomercial').inputmask({
        mask: "1/2/y",
        placeholder: "dd/mm/yyyy",
        alias: "datetime"
    });

    $('#Esteofechaopecomercial').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#Esteofechaopecomercial').val(date);
        }
    });

    $('#Esteofechaintegracion').inputmask({
        mask: "1/2/y",
        placeholder: "dd/mm/yyyy",
        alias: "datetime"
    });

    $('#Esteofechaintegracion').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#Esteofechaintegracion').val(date);
        }
    });


    $('#EsteoAbsFFin').inputmask({
        mask: "1/2/y",
        placeholder: "dd/mm/yyyy",
        alias: "datetime"
    });

    $('#EsteoAbsFFin').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#EsteoAbsFFin').val(date);
        }
    });

    var esteoterinvcodi = [];
    var esteoterinvcodis = $("#__Esteoterinvcodi").val().split(",");
    
    for (var i = 0; i < esteoterinvcodis.length; i++) {
        esteoterinvcodi.push(esteoterinvcodis[i]);
    }

    $('#Emprcoditi').multipleSelect('setSelects', esteoterinvcodi);

    $("#btnGuardar").click(function () {
        mostrarConfirmacion("¿Desea guardar la información ingresada?", Guardar, "c");
    });

    /*Inicio Mejoras EO - EPO*/
    if ($("#Estacodi").val() == 10 || $("#Estacodi").val() == 11) {
        $("#chkNoAprobado").prop('checked', true);
        $('#Esteofechafin').prop('disabled', true);
        $('#Esteocertconformidadtit').prop('disabled', true);
        $('#Esteocertconformidadenl').prop('disabled', true);
        $('#Esteoresumenejecutivotit').prop('disabled', true);
        $('#Esteoresumenejecutivoenl').prop('disabled', true);
        $('#btnGuardar').prop('disabled', true);
        
    }
    else {
        $("#chkNoAprobado").prop('checked', false);
        $('#Estacodi').prop('disabled', true);
        $('#Esteofechafin').prop('disabled', false);
        $('#Esteocertconformidadtit').prop('disabled', false);
        $('#Esteocertconformidadenl').prop('disabled', false);
        $('#Esteoresumenejecutivotit').prop('disabled', false);
        $('#Esteoresumenejecutivoenl').prop('disabled', false);
        $('#btnGuardar').prop('disabled', false);
        document.getElementById("btnGuardar").style.display = "block";
    }
    /*Fin Mejoras EO-EPO*/
    

});

function regresar() {
    //window.location = controlador + "index";
    window.history.go(-1);
}

function Guardar() {
    $("#btnGuardar").submit();
}

$('#frmEstudioEo').submit(function (e) {

    var data = $('#frmEstudioEo').serializeArray();
    elemento = JSON.stringify(getFormData(data));
    
    if (!$(this).valid()) {
        $("#mensaje").show();
        $('#popupConfirmarOperacion').bPopup().close();
        return false;
    }

    $.ajax({
        type: "POST",
        url: controlador + "RegistrarEstudioEo",
        contentType: 'application/json',
        //dataType: "json",
        data: elemento,
        success: function (resultado) {
            if (resultado == 1) {
                $('#popupZ').bPopup().close();
                $("#popupConfirmarOperacion .b-close").click();

                mostrarMensajePopup("La información se actualizó correctamente!.", 1);

                window.location = controlador + "index";
            }
        },
        error: function (req, status, error) {

        }
    });
});

function getFechaYYYYMMDD(fecha, separador) {
    if (fecha == "") return fecha;
    fecha = this.cambiarSeparador(fecha, "");
    return fecha.substr(4, 4) + separador + fecha.substr(2, 2) + separador + fecha.substr(0, 2);
}

function cambiarSeparador(fecha, separador) {
    return fecha.split('/').join(separador);
}

function getFormData(data) {
    var unindexed_array = data;
    var indexed_array = {};

    $.map(unindexed_array, function (n, i) {
        indexed_array[n['name']] = n['value'];
    });

    indexed_array["Esteoterinvcodi"] = $('#Emprcoditi').multipleSelect('getSelects');
    indexed_array["Esteofechaini"] = getFechaYYYYMMDD($('#Esteofechaini').val(), "-");
    indexed_array["Esteofechafin"] = getFechaYYYYMMDD($('#Esteofechafin').val(), "-");
    indexed_array["Esteoalcancefechaini"] = getFechaYYYYMMDD($('#Esteoalcancefechaini').val(), "-");
    indexed_array["Esteoalcancefechafin"] = getFechaYYYYMMDD($('#Esteoalcancefechafin').val(), "-");
    indexed_array["Esteoverifechaini"] = getFechaYYYYMMDD($('#Esteoverifechaini').val(), "-");
    indexed_array["Esteoverifechafin"] = getFechaYYYYMMDD($('#Esteoverifechafin').val(), "-");
    indexed_array["EsteoAbsFFin"] = getFechaYYYYMMDD($('#EsteoAbsFFin').val(), "-");

    /*Inicio Mejoras EO - EPO*/

    //indexed_array["Esteofechaconexion"] = getFechaYYYYMMDD($('#Esteofechaconexion').val(), "-");
    //indexed_array["Esteofechaopecomercial"] = getFechaYYYYMMDD($('#Esteofechaopecomercial').val(), "-");
    //indexed_array["Esteofechaintegracion"] = getFechaYYYYMMDD($('#Esteofechaintegracion').val(), "-");

    /*Fin Mejoras EO - EPO*/

    //var descripcionProyecto = $("#cboDescripcionProyecto").val();
    //switch (descripcionProyecto) {
    //    case "Potencia de Generación":
    //        indexed_array["Esteopotencia"] = $("#txtDescripcionProyecto").val();
    //        break;
    //    case "Potencia de Demanda":
    //        indexed_array["Esteocapacidad"] = $("#txtDescripcionProyecto").val();
    //        break;
    //    case "Transmisión":
    //        indexed_array["Esteocarga"] = $("#txtDescripcionProyecto").val();
    //        break;
    //    case "Potencia de Generación RER":
    //        indexed_array["Esteopotenciarer"] = $("#txtDescripcionProyecto").val();
    //        break;
    //    default:
        
    //}
    
    return indexed_array;
}

function MostrarZonaXPunto(obj) {
    var idZonaPuntoConex = obj.value;
    $.ajax({
        url: controlador + 'MostrarZonaXPunto',
        type: 'POST',
        data: { PuntCodi: idZonaPuntoConex },
        success: function (repsuesta) {
            $('#txtZona').val(repsuesta);
        }
    });
}


function ListarConfiguraciones() {

    $.ajax({
        type: "POST",
        url: controlador + "ListaConfiguraciones",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            //BloquearBlockUI();
        },
        complete: function () {
            //DesbloquearBlockUI();
        },
        success: function (response) {
            //alert("Respuesta desde el codigo servidor");
            debugger;
            var objListaConfi = response.DataConfi;
            listaConf = response.DataConfi;
            if (objListaConfi != null) {
                if (objListaConfi.length > 0) {
                    /*$("#cboPR").empty();
                    var nroRegistros = objListaConfi.length;
                    
                    for (var i = 0; i < nroRegistros ; i++) {                        
                        $("<option value='" + objListaConfi[i].Confcodi + "'>" + objListaConfi[i].Confdescripcion + "</option>").appendTo("#cboPR");
                    }
                    $("#cboPR").val(2);*/
                }
            }

        },
        error: function (request, status, error) {
            //desbloqObject();
            var re = request;
        }
    });
}

function LLenarInputsConfig() {
    debugger;
    if (listaConf != null) {
        if (listaConf.length > 0) {

            if ($("#cboPR").val() == 1) {
                $("#txtPorVencer").val(listaConf[0].Confplazorevcoesporv);
                $("#txtVencido").val(listaConf[0].Confplazorevcoesvenc);
                $("#txtAbsolPorVen").val(listaConf[0].Confplazolevobsporv);
                $("#txtAbsolVencido").val(listaConf[0].Confplazolevobsvenc);
                $("#txtEnvioVencido").val(listaConf[0].Confplazoalcancesvenc);
                $("#txtVerifVencido").val(listaConf[0].Confplazoverificacionvenc);
                $("#txtReviPorVencer").val(listaConf[0].Confplazorevterceroinvporv);
                $("#txtReviVencido").val(listaConf[0].Confplazorevterceroinvvenc);
                $("#txtEnvioEstPorVen").val(listaConf[0].Confplazoenvestterceroinvporv);
                $("#txtEnvioEstVencido").val(listaConf[0].Confplazoenvestterceroinvvenc);

                $("#lblTiempoPorVencer").text("dh");
                $("#lblTiempoPorVencido").text("dh");
            }
            else {
                $("#txtPorVencer").val(listaConf[1].Confplazorevcoesporv);
                $("#txtVencido").val(listaConf[1].Confplazorevcoesvenc);
                $("#txtAbsolPorVen").val(listaConf[1].Confplazolevobsporv);
                $("#txtAbsolVencido").val(listaConf[1].Confplazolevobsvenc);
                $("#txtEnvioVencido").val(listaConf[1].Confplazoalcancesvenc);
                $("#txtVerifVencido").val(listaConf[1].Confplazoverificacionvenc);
                $("#txtReviPorVencer").val(listaConf[1].Confplazorevterceroinvporv);
                $("#txtReviVencido").val(listaConf[1].Confplazorevterceroinvvenc);
                $("#txtEnvioEstPorVen").val(listaConf[1].Confplazoenvestterceroinvporv);
                $("#txtEnvioEstVencido").val(listaConf[1].Confplazoenvestterceroinvvenc);

                $("#lblTiempoPorVencer").text("meses");
                $("#lblTiempoPorVencido").text("meses");
            }
        }
    }

}

/*Inicio Mejoras EO - EPO*/

$('#chkNoAprobado').on('click', function () {
    if ($(this).is(':checked')) {

        $('#Estacodi').prop('disabled', false);
        $('#Esteofechafin').prop('disabled', true);
        $('#Esteocertconformidadtit').prop('disabled', true);
        $('#Esteocertconformidadenl').prop('disabled', true);
        $('#Esteoresumenejecutivotit').prop('disabled', true);
        $('#Esteoresumenejecutivoenl').prop('disabled', true);
        $("#Esteofechafin").val("");

        $("#Esteocertconformidadtit").val("");
        $("#Esteocertconformidadenl").val("");
        $("#Esteoresumenejecutivotit").val("");
        $("#Esteoresumenejecutivoenl").val("");

    } else {

        $('#Estacodi').prop('disabled', true);
        $('#Esteofechafin').prop('disabled', false);
        $('#Esteocertconformidadtit').prop('disabled', false);
        $('#Esteocertconformidadenl').prop('disabled', false);
        $('#Esteoresumenejecutivotit').prop('disabled', false);
        $('#Esteoresumenejecutivoenl').prop('disabled', false);
        $("#Estacodi").val("0");
    }
});
/*Fin Mejoras EO - EPO*/

/*Inicio Mejoras EO-EPO-II*/
function MaximoCaracteres(maximoCaracteres) {
    var elemento = document.getElementById("txt_Esteonomb");
    if (elemento.value.length > maximoCaracteres) {
        mostrarMensajePopup("El máximo de caracteres es 200", 1);
        $("#txt_Esteonomb").val("");
    }
}
/*Fin Mejoras EO-EPO-II*/