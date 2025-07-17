$.message = "";
var controler = siteRoot + "resarcimientos/puntoentrega/";

$(document).ready(function () {
    $('.ui-datapicker').inputmask({
        mask: "1/2/y h:s:s",
        placeholder: "dd/mm/yyyy hh:mm:ss",
        alias: "datetime",
        hourFormat: "24"
    });
    $('#dtpFechaIni').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#dtpFechaIni').val(date + " 00:00:00");
            if (validarFechaMayorbyMenor()) {
                
            } else {
                mensajeOperacion("Ingrese Fecha Inicial Menor o Igual a la Feha Fin");
            }
        }
    });
    $('#dtpFechaFin').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#dtpFechaFin').val(date + " 00:00:00");
            if (validarFechaMayorbyMenor()) {
                
            } else {
                mensajeOperacion("Ingrese Fecha Fin Mayor o Igual a la Feha Inicial");
            }
        }
    });    

    function validarFechaMayorbyMenor() {
        var fechaInicial = $('#dtpFechaIni').val();
        var fechaFinal = $('#dtpFechaFin').val();

        cadStart = fechaInicial.split(" ");
        cadEnd = fechaFinal.split(" ");
        valuesStart = cadStart[0].split("/");
        valuesEnd = cadEnd[0].split("/");
        // Verificamos que la fecha no sea posterior a la actual
        var dateStart = new Date(valuesStart[2], (valuesStart[1] - 1), valuesStart[0], parseInt(cadStart[1].split(':')[0]), parseInt(cadStart[1].split(':')[1]), parseInt(cadStart[1].split(':')[2]), 0);
        var dateEnd = new Date(valuesEnd[2], (valuesEnd[1] - 1), valuesEnd[0], parseInt(cadEnd[1].split(':')[0]), parseInt(cadEnd[1].split(':')[1]), parseInt(cadEnd[1].split(':')[2]), 0);
        if (dateEnd >= dateStart) {
            return true;
        } else {
            return false;
        }
    }
	function validar(){
        var mensaje="";

        if ($('#RCboCliente').val() == "0"){    
            mensaje = mensaje + "Falta seleccionar Cliente <br/>";
            }
        if ($('#RCboPuntoEntrega').val() == "0") {
            mensaje = mensaje + "Falta seleccionar Punto de Entrega <br/>";
            }
        if ($('#RCboTension').val() == "0" ){
            mensaje = mensaje + "Falta seleccionar Tensión <br/>";
            }
        if ($('#CboTipInterrupcion').val() == "0"){
            mensaje = mensaje + "Falta seleccionar Tipo de Interrupción <br/>";
            }
        if ($('#txtCompensacion').val() == "") {
            mensaje = mensaje + "Falta ingresar datos en el campo Compensación <br/>";
        }
        if (parseFloat(($('#txtCompensacion').val())) > parseFloat(99999999.99)) {
            mensaje = mensaje + "Ingresar datos en el campo Compensación menor a 99,999,999.99 <br/>";
        }
        if ($('#txtCRInterurpcion').val() == "") {
            mensaje = mensaje + "Falta ingresar datos en el campo Interrupción <br/>";
        }
        if ($('#dtpFechaIni').val() == "") {
            mensaje = mensaje + "Falta ingresar datos en el campo Fecha Inicio <br/>";
        }
        if ($('#dtpFechaFin').val() == "") {
            mensaje = mensaje + "Falta ingresar datos en el campo Fecha Fin <br/>";
        }
        if ($('#CboPeriodo').val() == "0") {
            mensaje = mensaje + "Selecione un un Periodo antes de Registrar... <br/>";
        }
            return mensaje;
	};
	function CargarOptionselect() {
	    $.ajax({
	        type: "POST",
	        url: controler + "Optionselect",
	        //dataType: 'json',
	        data: {
	            empresa: $('#CboEmpresasGeneradoras').val(),
	            periodo: $('#CboPeriodo').val(),
	            cliente: $('#RCboCliente').val(),
	            pentrega: $('#RCboPuntoEntrega').val(),
	            ntension: $('#RCboTension').val()
	        },
	        cache: false,
	        success: function (resultado) {
	            $('#content_selection').html(resultado);
	        },
	        error: function (req, status, error) {
	            mensajeOperacion(error);
	            validaErrorOperation(req.status);
	        }
	    });
	}
    //Eliminar Registro Punto entrega
    $('#btnEliminarRegistroEntrega').click(function () {
        var id = $(this).attr('data-registro');
        $.ajax({
            type: "POST",
            url: controler + "eliminarpuntoentrega",
            //dataType: 'json',
            data: {
                registro: id
            },
            cache: false,
            success: function (resultado) {
                mensajeOperacion(resultado);
                var contnum = 0;
                $('#tblTramites tr').each(function (index) {
                    contnum++;
                });
                if (contnum == 1) {
                    $.ajax({
                        type: "POST",
                        url: controler + "Optionselect",
                        //dataType: 'json',
                        data: {
                            empresa: $('#CboEmpresasGeneradoras').val(),
                            periodo: $('#CboPeriodo').val(),
                            cliente: $('#CboCliente').val(),
                            pentrega: $('#CboPuntoEntrega').val(),
                            ntension: $('#CboTension').val()
                        },
                        cache: false,
                        success: function (resultado) {
                            $('#content_selection').html(resultado);
                            buscarReporte();
                        },
                        error: function (req, status, error) {
                            mensajeOperacion(error);
                            validaErrorOperation(req.status);
                        }
                    });
                } else {
                    buscarReporte();
                }
            },
            error: function (req, status, error) {
                mensajeOperacion(error);
                validaErrorOperation(req.status);
            }
        });
    });

    //Agregar a Grilla Empresa Generadora
    $('#CboERInstall').change(function () {
        $('#txtPorcentaje').focus();
    });
    $('#txtPorcentaje').keypress(function (e) {
        if (e.which == 13) {
            if (parseFloat($('#txtPorcentaje').val()) < 0) {
                mensajeOperacion('No puede ingresar valores negativos...');
                $('#txtPorcentaje').val('');
            } else {
                $('#btnAdd').click();
            }
        }        
    });
    $('#btnAdd').click(function () {
        if (parseFloat($('#txtPorcentaje').val()) < 0) {
            mensajeOperacion('No puede ingresar valores negativos...');
            $('#txtPorcentaje').val('');
        } else {
            procentajeDetermina();
        }
    });
});
