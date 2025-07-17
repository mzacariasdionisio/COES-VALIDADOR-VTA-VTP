$(document).ready(function () {

    obtenerHora();
    setInterval(function () {
        obtenerHora();
    }, 1000);

    $('#cbSelectAll').click(function (e) {
        var table = $(e.target).closest('table');
        $('td input:checkbox', table).prop('checked', this.checked);
    });

    $('#btnGrabar').click(function () {
        grabar();
    });

    $('#btnAceptar').click(function () {
        document.location.href = siteRoot;
    })

    $('#btnCancelar').click(function () {
        document.location.href = siteRoot;
    });
});

function grabar() {
    var validacion = validarRegistro();
    if (validacion == "") {

        $.ajax({
            type: 'POST',
            url: siteRoot + 'account/registro/save',
            data: $('#frmRegistro').serialize(),
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    mostrarResultado();
                }
                else if (result == 2) {
                    $('#mensaje').removeClass();
                    $('#mensaje').html("El correo ingresado ya se encuentra registrado.");
                    $('#mensaje').addClass('action-alert');
                }
                else {
                    mostrarError();
                }
            },
            error: function () {
                mostrarError();
            }
        });
    }
    else {
        $('#mensaje').removeClass();
        $('#mensaje').html(validacion);
        $('#mensaje').addClass('action-alert');
    }
}

function mostrarResultado() {
    $.ajax({
        type: 'POST',
        url: siteRoot + 'account/registro/datosenviados',
        data: $('#frmRegistro').serialize(),
        success: function (evt) {
            $('#divResultado').css('display', 'block');
            $('#divRegistro').css('display', 'none');
            $('#divDatosEnviados').html(evt);
        },
        error: function () {
            mostrarError();
        }
    });
}

function validarRegistro()
{
    var modulos = "";
    var countModulo = 0;
    $('#tbModulos tbody input:checked').each(function () {
        modulos = modulos + $(this).val() + ",";
        countModulo++;
    });

    var mensaje = "<ul>";
    var flag = true;

    if ($('#txtNombre').val() == "") {
        mensaje = mensaje + "<li>Ingrese nombre</li>";
        flag = false;
    }
    else {
        if (IsText($('#txtNombre').val()) == false) {
            mensaje = mensaje + "<li>Ingrese letras en el nombre</li>";
        }
    }
    if($('#txtApellido').val() == ""){
        mensaje = mensaje + "<li>Ingrese apellidos</li>";
        flag = false;
    }
    else {
        if (IsText($('#txtApellido').val()) == false) {
            mensaje = mensaje + "<li>Ingrese letras en el apellido</li>";
        }
    }
    if ($('#cbEmpresa').val() == "") {
        mensaje = mensaje + "<li>Seleccione empresa</li>";
        flag = false;
    }
    else {       
        $('#hfEmpresa').val($('#cbEmpresa option:selected').text())
    }
    if ($('#txtCorreo').val() == "") {
        mensaje = mensaje + "<li>Ingrese correo electrónico</li>";
        flag = false;
    }
    else {
        if (IsEmail($('#txtCorreo').val()) == false) {
            mensaje = mensaje + "<li>Ingrese un correo válido.</li>";
            flag = false;
        }
    }
    if($('#txtTelefono').val() == ""){
        mensaje = mensaje + "<li>Ingrese teléfono</li>";
        flag = false;
    }
    else {
        if (IsOnlyDigit($('#txtTelefono').val()) == false) {
            mensaje = mensaje + "<li>Ingrese un número telefónico válido</li>";
        }
    }
    if($('#txtArea').val() == ""){
        mensaje = mensaje + "<li>Ingrese área laboral</li>";
        flag = false;
    }
    if($('#txtCargo').val() == ""){
        mensaje = mensaje + "<li>Ingrese cargo</li>";
        flag = false;
    }
   
    if (countModulo == 0) {
        mensaje = mensaje + "<li>Seleccione los módulos a los que desea acceder.</li>";
        flag = false;
    }
    else{
        $('#hfModulos').val(modulos);
    }    

    mensaje = mensaje + "</ul>";

    if (flag) {
        mensaje = "";
    }

    return mensaje;
}

function mostrarError() {
    $('#mensaje').removeClass();
    $('#mensaje').html("Ha ocurrido un error, por favor intente nuevamente.");
    $('#mensaje').addClass('action-error');
}

function obtenerHora() {
    var d = new Date();
    var dia = ("0" + d.getDate()).slice(-2);
    var mes = ("0" + (d.getMonth() + 1)).slice(-2);
    var anio = ("0" + d.getFullYear()).slice(-4);
    var h = ("0" + d.getHours()).slice(-2);
    var m = ("0" + d.getMinutes()).slice(-2);
    var s = ("0" + d.getSeconds()).slice(-2);

    $('#clock').html(dia + "/" + mes + "/" + anio + "  " + h + ":" + m + ":" + s);
}

function IsEmail(email) {
    var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return regex.test(email);
}

function IsText(texto) {    
    var regex = /^[A-Za-z]+$/;
    return regex.test(texto);
}

function IsOnlyDigit(texto) {

    for (var i = 0; i < texto.length; i++) {
        if (!(IsDigito(texto[i]) || texto[i] == ' ' || texto[i] == '+' || texto[i] == '(' || texto[i] == ')' || texto[i] == '-'))
        {
            return false;
        }
    }
    return true;
}

function IsDigito(caracter) {

    if( caracter == '0' || 
        caracter == '1' || 
        caracter == '2' || 
        caracter == '3' || 
        caracter == '4' || 
        caracter == '5' || 
        caracter == '6' || 
        caracter == '7' || 
        caracter == '8' ||
        caracter == '9')
        return true;
    
        return false;    
}




