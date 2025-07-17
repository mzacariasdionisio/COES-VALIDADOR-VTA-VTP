var controlador = siteRoot + 'interconexiones/'

$(function () {
    //$('#tab-container').easytabs({
    //    animate: false
    //});

    SelecionarTab('tabs1-css');

    $('#cbConttipoper').val(1);
    $('#cbHoraIni').val($('#hfcbHoraIni').val());
    $('#cbHoraFin').val($('#hfcbHoraFin').val());
    $('#btnAceptarContrato').click(function () {
        cerrarpopupContNuevoOK();
    });
    //$('#printArea').hide();

    $("#frmContrato").validate({
        submitHandler: function () {
            pasarDatos();
            imprSelec('printArea');
        },
        rules: {
            Contagautnom: "required",
            Contagautdirec: "required",
            Contagauttipoact: "required",
            Contagautreplegal: "required",
            Contagautemail: {
                required: true

            },
            Contagauttelef: {
                required: true,
                minlength: 7
            },
            Contagautrepaut: "required",
            Contagautrepautemail: {
                required: true
            },
            Contagautrepauttel: {
                required: true,
                minlength: 7
            },
            Contaghabnom: "required",
            Contaghabdirec: "required",
            Contaghabreplegal: "required",
            Contaghabemail: {
                required: true
            },
            Contaghabtelef: {
                required: true,
                minlength: 7
            },
            Contfechaini: "required",
            Contfechafin: "required",
            Contpotmax: "required",
            Contcopcontrato: "required",
            Contacuinter: "required"
        },
        messages: {
            Contagautnom: "Ingrese denominación o razón social",
            Contagautdirec: "Ingrese domicilio legal",
            Contagauttipoact: "Ingrese tipo de actividad",
            Contagautreplegal: "Ingrese representante legal",
            Contagautemail: "Ingrese email válido",

            Contagauttelef: {
                required: "Ingrese teléfono",
                minlength: "Mínino 7 caracteres"
            },
            Contagautrepaut: "Ingrese representante autorizado",
            Contagautrepautemail: "Ingrese email válido",
            Contagautrepauttel: {
                required: "Ingrese teléfono",
                minlength: "Mínino 7 caracteres"
            },
            Contaghabnom: "Ingrese denominación o razón social",
            Contaghabdirec: "Ingrese domicilio legal",
            Contaghabreplegal: "Ingrese representante legal",
            Contaghabemail: "Ingrese email válido",
            Contaghabtelef: {
                required: "Ingrese teléfono",
                minlength: "Mínino 7 caracteres"
            },
            Contfechaini: "Ingrese fecha inicio de contrato",
            Contfechafin: "Ingrese fecha fin de contrato",
            Contpotmax: "Ingrese potencia máxima contratada",
            Contcopcontrato: "Ingrese contrato",
            Contacuinter: "Ingrese acuerdo de intercambio"
        }
    });

    //$('#txtContfechaini').Zebra_DatePicker({

    //});

    //$('#txtContfechafin').Zebra_DatePicker({
    //    //format: 'm/Y'
    //});


    $('#cbTipoCopia').multipleSelect({
        width: '200px',
        filter: true
    });

    $('#btnImprimir').click(function () {
        var resultado = validarContrato();
            if (resultado === "") {
                pasarDatos();
                imprSelec('printArea');
            }

    });

    $('#btnCancelar').click(function () {
        cerrar();
    });
    cargarPrevio();
});// end function

downloadBlob = function (url) {
    document.location.href = controlador + "reportes/download?url=" + url;
}

function cargarPrevio() {
    if ($('#hfIsnuevo').val() == 0) { //si el contrato existe        
        deshab();
        $('#btnAceptar').hide();
    }
    else {
        //$('#btnImprimir').hide();
    }

}

cerrar = function () {
    document.location.href = controlador + "reportes/Contratos";
}

function validarContrato() {
    var resultado = "";

    var tipoOper = $('#cbConttipoper').val();
    if (tipoOper == 0) {
        resultado = "Seleccionar tipo de operación";
        return resultado;
    }

    if ($('#txtContagautnom').val() == "") {
        resultado = "Ingrese Denominacion o razón social";
        return resultado;
    }

    if ($('#Contagautdirec').val() == "") {
        resultado = "Ingrese Domicilio legal";
        return resultado;
    }

    return resultado;
}

function guardarContrato() {
    
    $.ajax({
        type: 'POST',
        url: controlador + "GrabarContrato",
        dataType: 'json',
        data: $('#frmContrato').serialize(),        
        success: function (evt) {
            alert("Formulario enviado correctamente,Imprimirlo");
        },
        error: function () {
            alert("Ha ocurrido un error al enviar el formaulario.");
        }
    });

}

function cerrarpopupContNuevoOK() {
    //$('#popupContNuevoOK').bPopup().close();
    //cerrar();

}

function deshab() {
    frm = document.forms['frmContrato'];
    for (i = 0; ele = frm.elements[i]; i++)
        ele.disabled = true;
}

function imprSelec(printArea) {       
    $("#printArea").print({
        //Use Global styles
        globalStyles: false,
        //Add link with attrbute media=print
        mediaPrint: false,
        //Custom stylesheet
        stylesheet: urlRoot + "/areas/interconexiones/content/css/print.css",
        //Print in a hidden iframe
        iframe: false,
        //Don't print this
        noPrintSelector: "",
        //Add this at top
        prepend: " ",
        //Add this on bottom
        append: " "
    });
}

function pasarDatos() {

    var tipo = $('#cbConttipoper').val();

    if (tipo == 1) {
        $('#txtImport').html('de Ecuador');
    }
    if (tipo == 2) {
        $('#txtExport').html('a Ecuador');
    }

    tipo = $('#cbTipoCopia').multipleSelect('getSelects');
    $('#hfcbTipoCopia').val(tipo);
    texto = $('#hfcbTipoCopia').val();
    lista = texto.split(",");

    if (lista.indexOf("1") >= 0) {
        $('#txtContrato').html('X'); //7.1
    }
    if (lista.indexOf("2") >= 0) {
        $('#txtAnexo1').html('X'); //7.2
    }
    if (lista.indexOf("3") >= 0) {
        $('#txtAnexo2').html('X'); //7.3
    }
    if (lista.indexOf("4") >= 0) {
        $('#txtAcuerdoInter').html('X'); //7.4
    }

    $('#Contagautnom').html($('#txtContagautnom').val()); //2.1
    $('#Contagautdirec').html($('#txtContagautdirec').val()); //2.2
    $('#Contagauttipoact').html($('#txtContagauttipoact').val()); //2.3
    $('#Contagautreplegal').html($('#txtContagautreplegal').val()); //2.4
    $('#Contagautemail').html($('#txtContagautemail').val()); //2.5
    var texto = $('#txtContagautemail').val();
    if (texto.length > 30) {
        $('#Contagautemail').css("height", 30);
    }
       

    $('#Contagauttelef').html($('#txtContagauttelef').val()); //2.6
    $('#Contagautrepaut').html($('#txtContagautrepaut').val()); //2.7
    $('#Contagautrepautemail').html($('#txtContagautrepautemail').val()); //2.8
    $('#Contagautrepauttel').html($('#txtContagautrepauttel').val()); //2.9
    $('#Contaghabnom').html($('#txtContaghabnom').val()); //3.1
    $('#Contaghabdirec').html($('#txtContaghabdirec').val()); //3.2
    $('#Contaghabreplegal').html($('#txtContaghabreplegal').val()); //3.3
    $('#Contaghabemail').html($('#txtContaghabemail').val()); //3.4
    $('#Contaghabtelef').html($('#txtContaghabtelef').val()); //3.5

    $('#Contfechaini').html(GetDateNormalFormat($('#txtContfechaini').val())); //4.1
    $('#HoraIni').html($('#cbHoraIni option:selected').text()); //4.2
    $('#Contfechafin').html(GetDateNormalFormat($('#txtContfechafin').val())); //5.1
    $('#HoraFin').html($('#cbHoraFin option:selected').text()); //5.2
    $('#Contpotmax').html($('#txtContpotmax').val()); //6.1
    $('#txtFirma').html($('#txtContagautrepaut').val()); //6.1
}


switchToTab = function (tab) {
    debugger;
    if (tab == "tabs1-css") {
        SelecionarTab("tabs1-css");
    }

    if (tab == "tabs2-css") {
        SelecionarTab("tabs2-css");
    }

    if (tab == "tabs3-css") {
        SelecionarTab("tabs3-css");
    }
};

SelecionarTab = function (NroTab) {
    if (NroTab === 'tabs1-css') {
        $('#btnTab2').removeClass('coes-tab--active');
        $('#btnTab3').removeClass('coes-tab--active');
        $('#btnTab1').addClass('coes-tab--active');

        $("#tabs2-css").css("display", "none");
        $("#tabs3-css").css("display", "none");
        $("#tabs1-css").css("display", "contents");
    }
    if (NroTab === 'tabs2-css') {
        $('#btnTab1').removeClass('coes-tab--active');
        $('#btnTab3').removeClass('coes-tab--active');
        $('#btnTab2').addClass('coes-tab--active');

        $("#tabs1-css").css("display", "none");
        $("#tabs3-css").css("display", "none");
        $("#tabs2-css").css("display", "contents");
    }
    if (NroTab === 'tabs3-css') {
        $('#btnTab1').removeClass('coes-tab--active');
        $('#btnTab2').removeClass('coes-tab--active');
        $('#btnTab3').addClass('coes-tab--active');

        $("#tabs1-css").css("display", "none");
        $("#tabs2-css").css("display", "none");
        $("#tabs3-css").css("display", "contents");
    }
}

// Convierte formato yyyy-mm-dd a dd/mm/yyyy
GetDateNormalFormat = function (fechaFormatoISO) {
    let fechaNormal = fechaFormatoISO.substring(8, 10) + "/" + fechaFormatoISO.substring(5, 7) + "/" + fechaFormatoISO.substring(0, 4);
    return fechaNormal;
}




