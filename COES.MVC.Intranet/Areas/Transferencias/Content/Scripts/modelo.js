// ASSETEC 2019-11
var controlador = siteRoot + "transferencias/modelo/";

$(document).ready(function () {

    // ------------------------------------- Funcionalidades de Modelos ------------------------------------------
    // Procedimiento que carga la grilla de modelo en el formulario principal
    ListarModelos();

    // Evento click del boton btnAgregarModelo que carga el formulario popup de registro de modelos
    $('#btnAgregarModelo').on('click', function () {
        PopupModelo(0, "", 0)
    });

    // Evento click del boton btnGrabarPopupModelo del formulario popup registro de modelos que
    // graba el registro de modelos
    $('#btnGrabarPopupModelo').on('click', function () {
        GrabarModelo()
    });

    // Evento keyup de la caja de texto txtNomModeloPopup del formulario popup registro de modelos que
    // valida si esta caja de texto esta vacio o no
    $("#txtNomModeloPopup").keyup(function () {
        if ($("#txtNomModeloPopup").val() == "") {

            $('#txtNomModeloPopup').css("border-color", "red");

            $('#msjErrNomModeloPopup').css("color", "red");
            $('#msjErrNomModeloPopup').html("Ingrese el nombre del modelo");

        } else {

            $('#txtNomModeloPopup').css("border-color", "");
            $('#msjErrNomModeloPopup').css("color", "red");
            $('#msjErrNomModeloPopup').html("");
        }
    });

    // Evento change del combo cboEmpresasPopup del formulario popup registro de modelos que
    // valida si este combo esta vacia o no
    $('#cboEmpresasPopup').on("change", function () {
        if ($('#cboEmpresasPopup').val() == "0") {

            $('#cboEmpresasPopup').css("border-color", "red");

            $('#msjErrEmpresasPopup').css("color", "red");
            $('#msjErrEmpresasPopup').html("Seleccione la empresa");

            rspta = false;
        }
        else {
            $('#cboEmpresasPopup').css("border-color", "");
            $('#msjErrEmpresasPopup').css("color", "red");
            $('#msjErrEmpresasPopup').html("");
        }
    });

    // -------------------------------- Funcionalidades de Códigos de Retiro -------------------------------------
    // Procedimiento que carga el combo de modelos en el formulario principal
    CargarComboModelos();

    // Evento click del boton btnConsultarCodigosRetiro que carga la grilla de códigos de retiro del formulario
    // principal
    $('#btnConsultarCodigosRetiro').on('click', function () {
        ConsultarCodigosRetiro()
    });

    // Evento click del boton btnAgregarCodigoRetiro que carga el formulario popup de registro de códigos de retiro
    $('#btnAgregarCodigoRetiro').on('click', function () {
        PopupCodigoModeloRetiro(0, 0, 0, 0, "")
    });

    // Procedimiento que carga el combo de modelos en el formulario popup de registro de códigos de retiro
    CargarComboModelosPopup();

    // Procedimiento que carga el combo de barras en el formulario popup de registro de códigos de retiro
    CargarComboBarrasPopup();

    // Evento change del combo cboModeloPopup del formulario popup registro de códigos de retiro que
    // valida si este combo esta vacio o no
    $('#cboModeloPopup').on("change", function () {
        if ($('#cboModeloPopup').val() == "0") {

            $('#cboModeloPopup').css("border-color", "red");

            $('#msjErrModeloPopup').css("color", "red");
            $('#msjErrModeloPopup').html("Seleccione el modelo");

            rspta = false;
        }
        else {
            $('#cboModeloPopup').css("border-color", "");
            $('#msjErrModeloPopup').css("color", "red");
            $('#msjErrModeloPopup').html("");
        }
    });

    // Evento change del combo cboBarraPopup del formulario popup registro de códigos de retiro que
    // ejecuta el procedimiento CargarComboCodigosRetiroPopup y ademas valida si este combo esta vacio o no
    $('#cboBarraPopup').on("change", function () {
        CargarComboCodigosRetiroPopup();

        if ($('#cboBarraPopup').val() == "0") {

            $('#cboBarraPopup').css("border-color", "red");

            $('#msjErrBarraPopup').css("color", "red");
            $('#msjErrBarraPopup').html("Seleccione la barra");

            rspta = false;
        }
        else {
            $('#cboBarraPopup').css("border-color", "");
            $('#msjErrBarraPopup').css("color", "red");
            $('#msjErrBarraPopup').html("");
        }
    });

    // Evento change del combo cboCodigosRetiroPopup del formulario popup registro de códigos de retiro que
    // valida si este combo esta vacio o no
    $('#cboCodigosRetiroPopup').on("change", function () {
        if ($('#cboCodigosRetiroPopup').val() == "0") {

            $('#cboCodigosRetiroPopup').css("border-color", "red");

            $('#msjErrCodigosRetiroPopup').css("color", "red");
            $('#msjErrCodigosRetiroPopup').html("Seleccione el código de retiro");

            rspta = false;
        }
        else {
            $('#cboCodigosRetiroPopup').css("border-color", "");
            $('#msjErrCodigosRetiroPopup').css("color", "red");
            $('#msjErrCodigosRetiroPopup').html("");
        }
    });

    // Evento click del boton btnGrabarPopupCodigoRetiro del formulario popup registro de códigos de retiro que
    // graba el registro de códigos de retiro
    $('#btnGrabarPopupCodigoRetiro').on('click', function () {
        GrabarCodigoRetiro()
    });
});

// ---------------------------------------------------------------------------------------------------------------
// Seccción de funcionalidades de Modelos
// ---------------------------------------------------------------------------------------------------------------
// Carga vista parcial de la grilla de modelos del formulario principal
function ListarModelos() {
    $.ajax({
        type: 'POST',
        url: controlador + "ListaModelo",
        data: {},
        success: function (result) {            
            $('#listaModelo').html(result);
            oTable = $('#tablaModelo').dataTable({
                "sPaginationType": "full_numbers",
                "destroy": "true",
                "aaSorting": [[1, "asc"], [2, "asc"]]
            });
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

// Carga el formulario popup de registro de modelos
function PopupModelo(id, nomModelo, idEmpresa) {
    $('#txtNomModeloPopup').css("border-color", "");
    $('#msjErrNomModeloPopup').css("color", "red");
    $('#msjErrNomModeloPopup').html("");

    $('#cboEmpresasPopup').css("border-color", "");
    $('#msjErrEmpresasPopup').css("color", "red");
    $('#msjErrEmpresasPopup').html("");

    if (id == 0) { // Registro        
        document.getElementById('idModelo').value = "0";
        document.getElementById('txtNomModeloPopup').value = "";
        document.getElementById('cboEmpresasPopup').value = 0;       
    }
    else { // Modificación
        document.getElementById('idModelo').value = id;
        document.getElementById('txtNomModeloPopup').value = nomModelo;
        document.getElementById('cboEmpresasPopup').value = idEmpresa;
    }

    $.ajax({
        type: 'POST',
        url: controlador + "PopupModelo",
        data: {},
        dataType: 'json',
        traditional: true,
        success: function (result) {

            setTimeout(function () {
                $('#popupModelo').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown',
                    onClose: function () { }
                });

            }, 100);

        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

// Graba el registro de modelo
function GrabarModelo() {
    var idModelo = document.getElementById('idModelo').value;
    var nombreModelo = document.getElementById('txtNomModeloPopup').value;
    var idCoordinador = document.getElementById('cboEmpresasPopup').value;
    
    if (ValidarRegistroModelo()) {
        $.ajax({
            type: 'POST',
            url: controlador + "GrabarModelo",
            data: {
                idModelo: idModelo,
                nombreModelo: nombreModelo,
                idCoordinador: idCoordinador
            },
            dataType: 'json',
            traditional: true,
            success: function (result) {

                if (result != "-1") {
                    ListarModelos();
                    CargarComboModelos();
                    CargarComboModelosPopup();
                    $('#popupModelo').bPopup().close();
                }
            },
            error: function () {
                alert("Ha ocurrido un problema...");
            }
        });
    }
}

// Elimina el registro de modelo
function EliminarModelo(idModelo) {
        $.ajax({
        type: 'POST',
        url: controlador + "TieneCodigosRetiroModelo",
        data: { idModelo: idModelo },
        success: function (result) {
            if (result != -1) {
                alert("Existen códigos de retiro asociado al modelo. No se puede eliminar el modelo");                
            }
            else {
                if (confirm("¿Desea eliminar el modelo?")) {
                    $.ajax({
                        type: 'POST',
                        url: controlador + "EliminarModelo",
                        data: { idModelo: idModelo },
                        dataType: 'json',
                        traditional: true,
                        success: function (result) {

                            if (result != "-1") {
                                ListarModelos();
                                CargarComboModelos();
                                CargarComboModelosPopup();
                            }
                        },
                        error: function () {
                            alert("Ha ocurrido un problema...");
                        }
                    });
                }
            }
        },
        error: function () {            
            alert("Ha ocurrido un problema...");
        }
    });    
}

// Valida los campos en blanco del formulario popup de registro de modelos
function ValidarRegistroModelo() {
    var rspta = true;

    if ($('#txtNomModeloPopup').val() == "") {

        $('#txtNomModeloPopup').css("border-color", "red");

        $('#msjErrNomModeloPopup').css("color", "red");
        $('#msjErrNomModeloPopup').html("Ingrese el nombre del modelo");

        rspta = false;
    }
    else {
        $('#txtNomModeloPopup').css("border-color", "");
        $('#msjErrNomModeloPopup').css("color", "red");
        $('#msjErrNomModeloPopup').html("");
    }

    if ($('#cboEmpresasPopup').val() == "0") {

        $('#cboEmpresasPopup').css("border-color", "red");

        $('#msjErrEmpresasPopup').css("color", "red");
        $('#msjErrEmpresasPopup').html("Seleccione la empresa");

        rspta = false;
    }
    else {
        $('#cboEmpresasPopup').css("border-color", "");
        $('#msjErrEmpresasPopup').css("color", "red");
        $('#msjErrEmpresasPopup').html("");
    }

    return rspta;
}

// ---------------------------------------------------------------------------------------------------------------
// Seccción de funcionalidades de Códigos de Retiro
// ---------------------------------------------------------------------------------------------------------------
// Carga combo de modelos del formulario principal
function CargarComboModelos() {    
    $.ajax({
        type: 'POST',
        url: controlador + "ListarComboModelos",
        datatype: 'json',
        data: { },
        contentType: "application/json",
        success: function (modelo) {
            $('#cboModelo').empty();
            var option = '<option value="0">-- Seleccionar --</option>';
            $.each(modelo.ListaModelos, function (k, v) {
                option += '<option value =' + v.TrnModCodi + '>' + ((v.TrnModNombre == null) ? "Sin nombre" : v.TrnModNombre) + '</option>';
            })
            $('#cboModelo').append(option);
        }
    });    
}

// Carga vista parcial de la grilla de códigos de retiro del formulario principal
function ConsultarCodigosRetiro() {
    $.ajax({
        type: 'POST',
        url: controlador + "ListaCodigoRetiroModelo", 
        datatype: 'json',       
        data: JSON.stringify({ idModelo: $('#cboModelo').val() }),
        contentType: "application/json",
        success: function (result) {
            $('#listado').html(result);
            oTable = $('#tabla').dataTable({
                "sPaginationType": "full_numbers",
                "destroy": "true",
                "aaSorting": [[1, "asc"], [2, "asc"]]
            });
        },
        error: function () {            
            alert("Ha ocurrido un problema...");
        }
    });
}

// Carga combo de modelos del formulario popup de registro de códigos de retiro
function CargarComboModelosPopup() {
    $.ajax({
        type: 'POST',
        url: controlador + "ListarComboModelos",
        datatype: 'json',
        data: {},
        contentType: "application/json",
        success: function (modelo) {
            $('#cboModeloPopup').empty();
            var option = '<option value="0">-- Seleccionar --</option>';
            $.each(modelo.ListaModelos, function (k, v) {
                option += '<option value =' + v.TrnModCodi + '>' + ((v.TrnModNombre == null) ? "Sin nombre" : v.TrnModNombre) + '</option>';
            })
            $('#cboModeloPopup').append(option);
        }
    });
}

// Carga combo en cascada de barras del formulario popup de registro de códigos de retiro
function CargarComboBarrasPopup() {
    $.ajax({
        type: 'POST',
        url: controlador + "ListarComboBarras",
        datatype: 'json',
        data: {},
        contentType: "application/json",
        success: function (modelo) {
            $('#cboBarraPopup').empty();
            var option = '<option value="0">-- Seleccionar --</option>';
            $.each(modelo.ListaBarras, function (k, v) {
                option += '<option value =' + v.BarrCodi + '>' + ((v.BarrNombBarrTran == null) ? "Sin nombre" : v.BarrNombBarrTran) + '</option>';
            })
            $('#cboBarraPopup').append(option);

            $('#cboCodigosRetiroPopup').empty();
            var option2 = '<option value="0">-- Seleccionar --</option>';
            $('#cboCodigosRetiroPopup').append(option2);
        }
    });
}

// Carga combo en cascada de códigos de retiro del formulario popup de registro de códigos de retiro
function CargarComboCodigosRetiroPopup() {
    idBarra = document.getElementById('cboBarraPopup').value;
    console.log(idBarra);
    if (idBarra != "0") {       

        $.ajax({
            type: 'POST',
            url: controlador + "ListarComboCodigosRetiro",
            datatype: 'json',
            data: JSON.stringify({ idBarra: idBarra }),
            contentType: "application/json",
            success: function (modelo) {
                $('#cboCodigosRetiroPopup').empty();
                var option = '<option value="0">-- Seleccionar --</option>';
                $.each(modelo.ListaCodigosRetiro, function (k, v) {
                    option += '<option value =' + v.SoliCodiRetiCodi + '>' + v.SoliCodiRetiCodigo + '</option>';                    
                })
                $('#cboCodigosRetiroPopup').append(option);
            }
        });        
    }
    else {
        $('#cboCodigosRetiroPopup').empty();
        var option2 = '<option value="0">-- Seleccionar --</option>';
        $('#cboCodigosRetiroPopup').append(option2);
    }    
}

// Carga el combo de códigos de retiro en proceso de edición que asegurara que
// aparezca dicho combo seleccionado ya con el valor que se selecciono en la grilla
function CargarComboCodigosRetiroPopupEnEdicion(idBarra, coresoCodi) {   

    $.ajax({
        type: 'POST',
        url: controlador + "ListarComboCodigosRetiro",
        datatype: 'json',
        data: JSON.stringify({ idBarra: idBarra }),
        contentType: "application/json",
        success: function (modelo) {
            $('#cboCodigosRetiroPopup').empty();
            var option = '<option value="0">-- Seleccionar --</option>';
            $.each(modelo.ListaCodigosRetiro, function (k, v) {
                option += '<option value =' + v.SoliCodiRetiCodi + '>' + v.SoliCodiRetiCodigo + '</option>';
            })
            $('#cboCodigosRetiroPopup').append(option);

            $("#cboCodigosRetiroPopup").val(coresoCodi);
        }
    }); 
}

// Carga el formulario popup de registro de códigos de retiro
function PopupCodigoModeloRetiro(id, idModelo, idBarra, coresoCodi) {
    $('#cboModeloPopup').css("border-color", "");
    $('#msjErrModeloPopup').css("color", "red");
    $('#msjErrModeloPopup').html("");

    $('#cboBarraPopup').css("border-color", "");
    $('#msjErrBarraPopup').css("color", "red");
    $('#msjErrBarraPopup').html("");

    $('#cboCodigosRetiroPopup').css("border-color", "");
    $('#msjErrCodigosRetiroPopup').css("color", "red");
    $('#msjErrCodigosRetiroPopup').html("");

    if (id == 0) { // Registro
        $("#idCodigoRetiro").val('0');
        console.log(document.getElementById('cboModelo').value);
        document.getElementById('cboModeloPopup').value = document.getElementById('cboModelo').value;
        document.getElementById('cboBarraPopup').value = 0;
        $("#CoresoCodi").val('0');
        document.getElementById('cboCodigosRetiroPopup').value = "";

        $('#cboCodigosRetiroPopup').empty();
        var option2 = '<option value="0">-- Seleccionar --</option>';
        $('#cboCodigosRetiroPopup').append(option2);
    }
    else { // Modificación
        document.getElementById('idCodigoRetiro').value = id;
        document.getElementById('cboModeloPopup').value = idModelo;
        document.getElementById('cboBarraPopup').value = idBarra;
        document.getElementById('CoresoCodi').value = coresoCodi;       
        CargarComboCodigosRetiroPopupEnEdicion(idBarra, coresoCodi)
    }

    $.ajax({
        type: 'POST',
        url: controlador + "PopupCodigoModeloRetiro",
        data: {},
        dataType: 'json',
        traditional: true,
        success: function (result) {

            setTimeout(function () {
                $('#popupCodigoModeloRetiro').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown',
                    onClose: function () { }
                });

            }, 100);           
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

// Graba el registro de código de retiro
function GrabarCodigoRetiro() {
    var idCodigoRetiro = $("#idCodigoRetiro").val();
    var idModelo = document.getElementById('cboModeloPopup').value;
    var idBarra = document.getElementById('cboBarraPopup').value;    
    var coresoCodi = $("#cboCodigosRetiroPopup").val();
    var coresoCodigo = $('#cboCodigosRetiroPopup option:selected').html()

    $("#cboModelo").val(idModelo);
    
    if (ValidarRegistroCodigoRetiro()) {
        $.ajax({
            type: 'POST',
            url: controlador + "GrabarCodigoRetiro",
            data: {
                idCodigoRetiro: idCodigoRetiro,
                idModelo: idModelo,
                idBarra: idBarra,
                coresoCodi: coresoCodi,
                coresoCodigo: coresoCodigo
            },
            dataType: 'json',
            traditional: true,
            success: function (result) {

                if (result != "-1") {                    
                    ConsultarCodigosRetiro();
                    $('#popupCodigoModeloRetiro').bPopup().close();
                }

            },
            error: function () {
                alert("Ha ocurrido un problema...");
            }
        });
    }    
}

// Elimina el registro de código de retiro
function EliminarCodigoRetiro(id, idModelo) {

    $("#cboModelo").val(idModelo);

    if (confirm("¿Desea eliminar el código de retiro?")) {
        $.ajax({
            type: 'POST',
            url: controlador + "EliminarCodigoRetiro",
            data: { id: id },
            dataType: 'json',
            traditional: true,
            success: function (result) {

                if (result != "-1") {                    
                    ConsultarCodigosRetiro();
                }
            },
            error: function () {
                alert("Ha ocurrido un problema...");
            }
        });
    }    
}

// Valida los campos en blanco del formulario popup de registro de códigos de retiro
function ValidarRegistroCodigoRetiro() {
    var rspta = true;    

    if ($('#cboModeloPopup').val() == "0") {

        $('#cboModeloPopup').css("border-color", "red");

        $('#msjErrModeloPopup').css("color", "red");
        $('#msjErrModeloPopup').html("Seleccione el modelo");

        rspta = false;
    }
    else {
        $('#cboModeloPopup').css("border-color", "");
        $('#msjErrModeloPopup').css("color", "red");
        $('#msjErrModeloPopup').html("");
    }

    if ($('#cboBarraPopup').val() == "0") {

        $('#cboBarraPopup').css("border-color", "red");

        $('#msjErrBarraPopup').css("color", "red");
        $('#msjErrBarraPopup').html("Seleccione la barra");

        rspta = false;
    }
    else {
        $('#cboBarraPopup').css("border-color", "");
        $('#msjErrBarraPopup').css("color", "red");
        $('#msjErrBarraPopup').html("");
    }

    if ($('#cboCodigosRetiroPopup').val() == "0") {

        $('#cboCodigosRetiroPopup').css("border-color", "red");

        $('#msjErrCodigosRetiroPopup').css("color", "red");
        $('#msjErrCodigosRetiroPopup').html("Seleccione el código de retiro");

        rspta = false;
    }
    else {
        $('#cboCodigosRetiroPopup').css("border-color", "");
        $('#msjErrCodigosRetiroPopup').css("color", "red");
        $('#msjErrCodigosRetiroPopup').html("");
    }

    return rspta;
}