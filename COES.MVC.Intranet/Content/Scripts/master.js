$(document).ready(function () {

    jQuery('ul.sf-menu').superfish({
        animation: { height: 'show' }
    });
    
    $('#CerrarSesion').click(function () {
        confirmOperacion();
    });

    $('#CambiarClave').click(function () {
        openCambioClave();
    });

    $('#ConfiguracionFavorito').click(function () {
        $('#contenedorAccesosDirectos').slideToggle(100);
        openConfiguracionFavorito();        
    });

    $('#openAccesoHeader').click(function () {
        $('#contenedorAccesosDirectos').slideToggle("slow");
    });

    $('#openNotification').click(function () {
        $('#contenedorNotificacion').slideToggle("slow");
    });

    $('#closeAccesoHeader').click(function () {
        $('#contenedorAccesosDirectos').slideToggle("slow");
    });


    $('#closeNotificacion').click(function () {
        $('#contenedorNotificacion').slideToggle("slow");
    });

    $('#btnOkCerrarSesion').click(function ()
    {
        $.ajax({
            type: "POST",
            url: siteRoot + "home/CerrarSesion",
            cache: false,
            dataType: 'json',
            success: function (evt) {
                location.href = siteRoot + 'home/login';
            },
            error: function (req, status, error) {
                alert("Ha ocurrido un error.");
            }
        });
    });

    $('#btnCancelCerrarSesion').click(function ()
    {
        $('#confirmarClose').bPopup().close();
    });

    $('#btnOcultarMenu').click(function ()
    {
        if ($('#cntMenu').css("display") == "none" || $('#cntMenu').css("display") == "hide") {
            $('#cntMenu').css("display", "block");
        }
        else {
            $('#cntMenu').css("display", "none");
        }
    });

    $('#btnOcultarTitulo').click(function ()
    {
        if ($('#cntTitulo').css("display") == "none") {
            $('#cntTitulo').css("display", "block");
            $('#cntLogo').css("display", "block");
        }
        else {
            $('#cntTitulo').css("display", "none");
            $('#cntLogo').css("display", "none");
        }
    });

    cargarMapa();

    setInterval(function () {
        validarSession();
    }, 600000);
    

});

function confirmOperacion()
{
    $('#confirmarClose').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
}

$(document).ajaxStart(function (event, jqxhr, settings) {
    if (!$('#loading').hasClass('cancel')) {
    $('#loading').bPopup({
        fadeSpeed: 'fast',
        opacity: 0.4,
        followSpeed: 500,
        modalColor: '#000000',        
        modalClose: false
    });
    }
});

$(document).ajaxStop(function () {
    if (!$('#loading').hasClass('cancel')) {
    $('#loading').bPopup().close();
    } else {
        $('#loading').removeClass('cancel')
    }
});

function cargarMapa()
{
    $.ajax({
        type: 'POST',
        url: siteRoot + "home/mostrarmapa",
        dataType: 'json',      
        cache: false,
        success: function (resultado) {
            $('#mapa').html(resultado.replace("Periodo Potencia Contratada","Periodo declaración Gestión de Códigos"));
        }
     });
}

function openCambioClave()
{
    $.ajax({
        type: 'POST',
        url: siteRoot + "home/cambioclave",        
        cache: false,
        success: function (evt) {
            $('#cambiarClave').html(evt);            
            setTimeout(function () {
                $('#cambiarClave').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });                
            }, 50);            
        }
    });    
}

function cambiarClave()
{
    var validacion = validarCambioClave();
    var elemento = $('#mensajeCambioClave');
    elemento.removeClass();

    if (validacion == "") {        
        $.ajax({
            type: 'POST',
            url: siteRoot + 'home/cambiarclave',
            dataType: 'json',
            data: {
                claveActual: $('#txtPasswordActual').val(),
                claveNueva: $('#txtPasswordNuevo').val()
            },
            cache: false,
            success: function (result) {
                if (result == 1) {
                    $('#contenidoCambioClave').css('display', 'none');
                    $('#resultadoCambioClave').css('display', 'block');
                    elemento.text("La operación se realizó correctamente.");                    
                    elemento.addClass('action-exito');
                }
                else if (result == 2) {
                    elemento.text("La clave actual no es correcta.");
                    elemento.addClass('action-alert');
                }
                else {
                    elemento.html("Ha ocurrido un error.");
                    elemento.addClass('action-error');
                }
            },
            error: function () {
                elemento.html("Ha ocurrido un error.");
                elemento.addClass('action-error');
            }
        });
    }
    else {
        elemento.html(validacion);        
        elemento.addClass('action-alert');
    }
}

function openConfiguracionFavorito()
{    
    $.ajax({
        type: 'POST',
        url: siteRoot + "home/favoritos",
        cache: false,
        success: function (evt) {
            $('#configurarFavorito').html(evt);
            setTimeout(function () {
                $('#configurarFavorito').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);
        }
    });
}

function grabarFavoritos() {
    var elemento = $('#mensajeFavoritos');
    elemento.removeClass();

    if ($('#hfNodosSeleccionados').val() != undefined && $('#hfNodosSeleccionados').val() != "") {
        $.ajax({
            type: 'POST',
            url: siteRoot + 'home/grabarfavoritos',
            dataType: 'json',
            data: {
                nodos: $('#hfNodosSeleccionados').val()
            },
            cache: false,
            success: function (result) {
                if (result == 1) {
                    cargarAccesosDirectos();
                    cargarAccesosHome();
                    $('#configurarFavorito').bPopup().close();
                }                
                else {
                    elemento.html("Ha ocurrido un error.");
                    elemento.addClass('action-error');
                }
            },
            error: function () {
                elemento.html("Ha ocurrido un error.");
                elemento.addClass('action-error');
            }
        });
    }
    else {
        elemento.addClass('action-alert');
        elemento.text('No ha seleccionado ninguna opción.');
    }
}

function cargarAccesosDirectos() {
    $.ajax({
        type: 'POST',
        url: siteRoot + "home/accesoheader",
        cache: false,
        success: function (evt) {
            $('#accesoDirectoHeader').html(evt);
        }
    });
}

function cargarAccesosHome()
{    
    $.ajax({
        type: 'POST',
        url: siteRoot + "home/accesohome",
        cache: false,
        success: function (evt) {
            $('#listaAccesoDirectos').html(evt);
        }
    });
}

function validarCambioClave()
{
    var mensaje = "<ul>";
    var flag = true;
    var claveActual = $('#txtPasswordActual').val();
    var claveNueva = $('#txtPasswordNuevo').val();
    var claveConfirmar = $('#txtPasswordConfirmar').val();

    if (claveActual == "") {
        mensaje = mensaje + "<li>Ingrese su clave actual.</li>";
        flag = false;
    }

    if (claveNueva == "") {
        mensaje = mensaje + "<li>Ingrese una nueva clave.</li>";
        flag = false;
    }

    if (claveNueva != "" || claveConfirmar != "") {
        if (claveNueva != claveConfirmar) {
            mensaje = mensaje + "<li>La nueva clave y la confirmación no coinciden.</li>";
            flag = false;
        }
    }

    mensaje = mensaje + "</ul>";

    if (flag == true) mensaje = "";

    return mensaje;
}

function validarSession() {
    $.ajax({
        type: 'POST',
        url: siteRoot + "home/validarSession",
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        cache: false,
        success: function (resultado) {
            if (resultado == 0) {
                alert("Sesión expirada!");
                location.href = siteRoot + 'home/login';
            }
        }
    });
}

function OpenUrl(enlace, tipo, id, controlador, accion)
{      
    if (tipo == "I")
    {
        $('#divGeneral').css("display", "none");
        $('#frameContenido').attr("src", enlace);
        $('#divTemporal').css("display", 'block');
        $('#frameContenido').load(function () {
            setTimeout(iResize, 2000);
        });

        function iResize() {            
            $('#frameContenido').height($('#frameContenido').contents().find("html").height() + 20);
        }

        $.ajax({
            type: 'POST',
            url: siteRoot + "home/setearopcion",
            dataType: 'json',
            data: {
                idOpcion: id
            },
            cache: false,
            success: function (resultado) {
                cargarMapa();
            }
        });
    }
    if (tipo == "F")
    {
        $.ajax({
            type: 'POST',
            url: siteRoot + "home/setearopcion",
            dataType: 'json',
            data: {
                idOpcion: id
            },
            cache: false,
            success: function (resultado) {
                var enlace = "";
                if (controlador != "" && accion != "") {
                    enlace = siteRoot + controlador + "/" + accion + (accion.indexOf("?") !== -1 ? "" : "/");
                }
                else if (controlador != "" && accion == "") {
                    enlace = siteRoot + controlador + "/";
                }
                else {
                    enlace = siteRoot + "#";
                }
                document.location.href = enlace;
            }
        });        
    }   
}
   
