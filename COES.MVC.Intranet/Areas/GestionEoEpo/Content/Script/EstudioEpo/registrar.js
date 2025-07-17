var controlador = siteRoot + 'gestioneoepo/estudioepo/';
var listaConf;

$(document).ready(function () {
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
    
    //$('#Estepofechaini').Zebra_DatePicker({});
    //$('#Estepofechaini').Zebra_DatePicker({});
    //$('#Estepofechafin').Zebra_DatePicker({});
    //$('#Estepoalcancefechaini').Zebra_DatePicker({});
    //$('#Estepoalcancefechafin').Zebra_DatePicker({});
    //$('#Estepoverifechaini').Zebra_DatePicker({});
    //$('#Estepoverifechafin').Zebra_DatePicker({});

    $('#Estepofechaini').inputmask({
        mask: "1/2/y",
        placeholder: "dd/mm/yyyy",
        alias: "datetime"
    });

    $('#Estepofechaini').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#Estepofechaini').val(date);
        }
    }); 

    $('#Estepofechafin').inputmask({
        mask: "1/2/y",
        placeholder: "dd/mm/yyyy",
        alias: "datetime"
    });

    $('#Estepofechafin').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#Estepofechafin').val(date);
        }
    });

    $('#Estepoalcancefechaini').inputmask({
        mask: "1/2/y",
        placeholder: "dd/mm/yyyy",
        alias: "datetime"
    });

    $('#Estepoalcancefechaini').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#Estepoalcancefechaini').val(date);
        }
    });

    $('#Estepoalcancefechafin').inputmask({
        mask: "1/2/y",
        placeholder: "dd/mm/yyyy",
        alias: "datetime"
    });

    $('#Estepoalcancefechafin').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#Estepoalcancefechafin').val(date);
        }
    });

    $('#Estepoverifechaini').inputmask({
        mask: "1/2/y",
        placeholder: "dd/mm/yyyy",
        alias: "datetime"
    });

    $('#Estepoverifechaini').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#Estepoverifechaini').val(date);
        }
    });

    $('#Estepoverifechafin').inputmask({
        mask: "1/2/y",
        placeholder: "dd/mm/yyyy",
        alias: "datetime"
    });

    $('#Estepoverifechafin').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#Estepoverifechafin').val(date);
        }
    });

    $('#EstepoAbsFFin').inputmask({
        mask: "1/2/y",
        placeholder: "dd/mm/yyyy",
        alias: "datetime"
    });

    $('#EstepoAbsFFin').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#EstepoAbsFFin').val(date);
        }
    });

    var estepoterinvcodi = [];
    var estepoterinvcodis = $("#__Estepoterinvcodi").val().split(",");
    
    for (var i = 0; i < estepoterinvcodis.length; i++) {
        estepoterinvcodi.push(estepoterinvcodis[i]);
    }

    $('#Emprcoditi').multipleSelect('setSelects', estepoterinvcodi);

    $("#btnGuardar").click(function () {
        mostrarConfirmacion("¿Desea guardar la información ingresada?", Guardar, "c");
    });

    /*Inicio Mejoras EO - EPO*/

    if ($("#Estacodi").val() == 10 || $("#Estacodi").val() == 11) {
        $("#chkNoAprobado").prop('checked', true);
        $('#Estepofechafin').prop('disabled', true);
        $('#Estepocertconformidadtit').prop('disabled', true);
        $('#Estepocertconformidadenl').prop('disabled', true);
        $('#Esteporesumenejecutivotit').prop('disabled', true);
        $('#Esteporesumenejecutivoenl').prop('disabled', true);
        $('#btnGuardar').prop('disabled', true);
        document.getElementById("btnGuardar").style.display = "none";
    }
    else {
        $("#chkNoAprobado").prop('checked', false);
        $('#Estacodi').prop('disabled', true);
        $('#Estepofechafin').prop('disabled', false);
        $('#Estepocertconformidadtit').prop('disabled', false);
        $('#Estepocertconformidadenl').prop('disabled', false);
        $('#Esteporesumenejecutivotit').prop('disabled', false);
        $('#Esteporesumenejecutivoenl').prop('disabled', false);
        $('#btnGuardar').prop('disabled', false);
        document.getElementById("btnGuardar").style.display = "block";
        
    }

    var anioservicio = $("#vigencia").val();
    var anioActual = new Date().getFullYear();

    if ((anioActual - anioservicio < 2) && $("#EstacodiEpo").val() == 3) {
        $("#EstacodiVigencia").prop('disabled', false);
    }
    else {
        $("#EstacodiVigencia").prop('disabled', true);
    }

    /*Fin Mejoras EO - EPO*/
        
});

function regresar() {
    //window.location = controlador + "index";
    window.history.go(-1);
}

function Guardar() {
    $("#btnGuardar").submit();
}

$('#frmEstudioEpo').submit(function (e) {
    debugger;

    var data = $('#frmEstudioEpo').serializeArray();
    elemento = JSON.stringify(getFormData(data));
    console.log(elemento);
    if (!$(this).valid()) {
        $("#mensaje").show();
        $('#popupConfirmarOperacion').bPopup().close();
        return false;
    }

    $.ajax({
        type: "POST",
        url: controlador + "RegistrarEstudioEpo",
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

  
    indexed_array["Estepoterinvcodi"] = $('#Emprcoditi').multipleSelect('getSelects');
    indexed_array["Estepofechaini"] = getFechaYYYYMMDD($('#Estepofechaini').val(), "-");
    indexed_array["Estepofechafin"] = getFechaYYYYMMDD($('#Estepofechafin').val(), "-");
    indexed_array["Estepoalcancefechaini"] = getFechaYYYYMMDD($('#Estepoalcancefechaini').val(), "-");
    indexed_array["Estepoalcancefechafin"] = getFechaYYYYMMDD($('#Estepoalcancefechafin').val(), "-");
    indexed_array["Estepoverifechaini"] = getFechaYYYYMMDD($('#Estepoverifechaini').val(), "-");
    indexed_array["Estepoverifechafin"] = getFechaYYYYMMDD($('#Estepoverifechafin').val(), "-");
    indexed_array["EstepoAbsFFin"] = getFechaYYYYMMDD($('#EstepoAbsFFin').val(), "-");

    /*Inicio Mejoras EO - EPO*/
    indexed_array["EstacodiVigencia"] = $("#EstacodiVigencia").val();
    /*Fin Mejoras EO - EPO*/


    //var descripcionProyecto = $("#cboDescripcionProyecto").val();
    //switch (descripcionProyecto) {
    //    case "Generación Convencional":
    //        indexed_array["Estepopotencia"] = $("#txtDescripcionProyecto").val();
    //        break;
    //    case "Generación No Convencional":
    //        indexed_array["Estepocapacidad"] = $("#txtDescripcionProyecto").val();
    //        break;
    //    case "Transmisión":
    //        indexed_array["Estepocarga"] = $("#txtDescripcionProyecto").val();
    //        break;
    //    case "Demanda":
    //        indexed_array["Estepocarga"] = $("#txtDescripcionProyecto").val();
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
                if (objListaConfi.length > 0)
                {
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
        $('#Estepofechafin').prop('disabled', true);
        $('#Estepocertconformidadtit').prop('disabled', true);
        $('#Estepocertconformidadenl').prop('disabled', true);
        $('#Esteporesumenejecutivotit').prop('disabled', true);
        $('#Esteporesumenejecutivoenl').prop('disabled', true);
        $("#Estepofechafin").val("");

        $('#EstacodiVigencia').prop('disabled', true);
        
        $("#Estepocertconformidadtit").val("");
        $("#Estepocertconformidadenl").val("");
        $("#Esteporesumenejecutivotit").val("");
        $("#Esteporesumenejecutivoenl").val("");

    } else {

        $('#Estacodi').prop('disabled', true);
        $('#Estepofechafin').prop('disabled', false);
        $('#Estepocertconformidadtit').prop('disabled', false);
        $('#Estepocertconformidadenl').prop('disabled', false);
        $('#Esteporesumenejecutivotit').prop('disabled', false);
        $('#Esteporesumenejecutivoenl').prop('disabled', false);        
        $("#Estacodi").val("0");

        var anioservicio = $("#vigencia").val();
        var anioActual = new Date().getFullYear();

        if ((anioActual - anioservicio < 2) && $("#EstacodiEpo").val() == 3) {
            $("#EstacodiVigencia").prop('disabled', false);
        }
        else {
            $("#EstacodiVigencia").prop('disabled', true);
        }
    }
});
/*Fin Mejoras EO - EPO*/

/*Inicio Mejoras EO-EPO-II*/
function MaximoCaracteres(maximoCaracteres) {
    var elemento = document.getElementById("txt_Esteponomb");
    if (elemento.value.length > maximoCaracteres) {
        mostrarMensajePopup("El máximo de caracteres es 200", 1);
        $("#txt_Esteponomb").val("");
    }
}
/*Fin Mejoras EO-EPO-II*/