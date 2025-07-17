//var controlador = siteRoot + 'ensayo/'
var controlador = siteRoot + 'Ensayo/';

var ListaUnidades = [];

$(function () {

    $("#btnCancelar3").click(function () {
        $('#miPopupRegistrarNuevoEnsayo').bPopup().close();
    });

    // Graba los datos de un nuevo ensayo...
    $('#btnAceptar').click(function () {
        var resultado = validarEnsayo();
        if (resultado == "") {
            guardarEnsayo();
        }
        else {
            alert("Error:" + resultado);
        }
    });

    $('#cbEmpresa').change(function () {
        cargarCentrales();
    });

    $('#btnCancelar').click(function () {
        cerrar();
    });
    $(window).resize(function () {
        $('#listado').css("width", $('#mainLayout').width() + "px");
    });

    $('#FechaDesde').Zebra_DatePicker({
    });
    $('#FechaAceptacion').Zebra_DatePicker({
    });
    //valoresIniciales()
    valoresIniciales();

});

/*
cerrar = function () {
    document.location.href = controlador + "genera/index/";
}
*/
function cerrar() {

}

function cerrarPopupUnidad() {
    $('#popupUnidad').bPopup().close();
}

function cerrarpopupEnsNuevoOK() {
    $('#popupEnsNuevoOK').bPopup().close();
    cerrar();
}

function validarEnsayo() {
    var resultado = "";
    var empresa = $('#cbEmpresa').val();
    var central = $('#cbCentral').val();

    if (empresa == 0) {
        resultado = "Seleccionar empresa";
        return resultado;
    }
    if (central == 0) {
        resultado = "Seleccionar Central";
        return resultado;
    }
    if (ListaUnidades.length == 0) {
        resultado = "Debe ingresar una unidad";
        return resultado;
    }

    return resultado;
}

function valoresIniciales() {
    var flagNuevo = $('#hEnsayoCodi').val();
    $('#txtModoOp').val($('#hftxtModoOp').val());
    //if (flagNuevo != "0") {
    //    $('#cbEmpresa').prop('disabled', true);
    //    $('#cbCentral').prop('disabled', true);
    //}

    if (flagNuevo != "0") {
        //deshab();
        $('#cbEmpresa').prop('disabled', true);
        $('#cbCentral').prop('disabled', true);
        $('#txtModoOp').prop('disabled', true);
        $('#available-fields').hide();
        document.getElementbyId('listaUnidades').disabled = true;
    }

    $("#listaUnidades tbody tr").each(function (index) {
        var codequipo;
        var nomequi;
        var modoper;
        $(this).children("td").each(function (index2) {
            switch (index2) {
                case 0: codequipo = $(this).text();
                    break;
                case 1: nomequi = $(this).text();
                    break;
            }
        });
        var unidad = {
            codEqui: codequipo,
            nomEqui: nomequi,
        };
        ListaUnidades.push(unidad);
    });
}

function guardarEnsayo() {
    $('#cbEmpresa').prop('disabled', false);
    $('#cbCentral').prop('disabled', false);
    $('#hstrVectorUnidad').val(vectorUnidad());
    $.ajax({
        type: 'POST',
        url: controlador + "genera/GrabarEnsayo",
        dataType: 'json',
        data: $('#frmRegistro').serialize(),
        success: function (evt) {
            $('#popupEnsNuevoOK').bPopup({
                easing: 'easeOutBack',
                speed: 450,
                transition: 'slideDown'
            });
        },
        error: function () {
            alert("Ha ocurrido un error en guardar ensayo");
        }
    });
}

function cargarCentrales() {
    var empresa = $('#cbEmpresa').val();
    $.ajax({
        type: 'POST',
        url: controlador + "genera/CargarCentralesDetalle",
        dataType: 'json',
        data: { idEmpresa: empresa },
        cache: false,
        success: function (aData) {
            $('#cbCentral').get(0).options.length = 0;
            $('#cbCentral').get(0).options[0] = new Option("--SELECCIONE--", "");
            $.each(aData, function (i, item) {
                $('#cbCentral').get(0).options[$('#cbCentral').get(0).options.length] = new Option(item.Text, item.Value);
            });
        },
        error: function () {
            alert("Ha ocurrido un error en cargar centrales...");
        }
    });
}

function ingresarUnidadesEnsayo() {
    var central = $('#cbCentral').val();
    if (central == 0) {
        alert("Error:Seleccionar la Central");
    }
    else {
        $('#hfcbCentral').val(central);
        $.ajax({
            type: 'POST',
            url: controlador + "genera/agregarUnidadesEnsayo",
            data: { iequicodi: $('#hfcbCentral').val() },
            success: function (evt) {
                $('#edicionEnsayo').html(evt);
                setTimeout(function () {
                    $('#popupUnidad').bPopup({
                        easing: 'easeOutBack',
                        speed: 450,
                        transition: 'slideDown'
                    });
                }, 50);
                $('#alerta').css("display", "none");
                $('#mensaje').css("display", "none");
            },
            error: function () {
                alert("Ha ocurrido un error en ingresar Unidades de Ensayo");
            }
        });
    }

}

function agregarUnidad(codequipo, nomequi) {
    var unidad = {
        codEqui: codequipo,
        nomEqui: nomequi,
    };
    if (!existeUnidad(codequipo)) {
        ListaUnidades.push(unidad);
        $('#popupUnidad').bPopup().close();
        $('#idTunidad').html(dibujarTablaUnidades());
        $('#cbEmpresa').prop('disabled', true);
        $('#cbCentral').prop('disabled', true);
    }
    else {
        alert("Unidad ya ingresada");
    }

}

function existeUnidad(codequipo) {
    var len = ListaUnidades.length;
    for (var i = 0; i < len; i++) {
        if (ListaUnidades[i].codEqui == codequipo)
            return 1;
    }
    return 0;
}

function eliminarUnidad(index) {

    if (ListaUnidades.length > 0) {
        if (confirm('¿Está seguro de eliminar la unidad?')) {
            ListaUnidades.splice(index, 1);
            $('#idTunidad').html(dibujarTablaUnidades());
        }
    }
}

function dibujarTablaUnidades() {
    var cadena = "<table border='1' class='pretty tabla-adicional' cellspacing='0'>";
    cadena += "<thead><tr><th>Código</th><th>Unidad</th><th></th></tr></thead>";
    cadena += "<tbody>";
    var len = ListaUnidades.length;
    for (var i = 0; i < len; i++) {
        cadena += "<tr><td>" + ListaUnidades[i].codEqui + "</td>";
        cadena += "<td>" + ListaUnidades[i].nomEqui + "</td>";
        cadena +=
            "<td  style=" + '"width: 18px"' + " > <a onclick=" + '"return eliminarUnidad(' + i + ");" + '" href="javascript:;"' +
            "><img  src=" + '"' + siteRoot + 'areas/ensayo/Content/Images/delete.png"' + "/></a></td></tr>";
    }
    cadena += "</tbody></table>";
    return cadena;
}

function vectorUnidad() {
    var cadena = "";
    var len = ListaUnidades.length;
    for (var i = 0; i < len; i++) {
        if (i == 0) {
            cadena += ListaUnidades[i].codEqui;
        }
        else {
            cadena += "," + ListaUnidades[i].codEqui;
        }
    }
    return cadena;
}