var controlador = siteRoot + 'hidrologia/'

var ORIGENLECTURA_HIDROLOGIAFINAL = 16;
var EMPRESA_TODOS = -1;
$(function () {

    $('#btnRegresar').click(function () {
        recargar();
    });     

    $('#btnBuscar').click(function () {
        mostrarListaEquipos();
    });
    
    $('#btnVentanaAgregarEquipo').click(function () {
        agregarEquipo();
    });

    mostrarListaEquipos();

});

function agregarEquipo() {
    var formato = $('#hfFormato').val();
    $.ajax({
        type: 'POST',
        url: controlador + "formatomedicion/MostrarAgregarEquipo",
        data: {
            formato: formato
        },
        global: false,
        success: function (evt) {
            $('#agregarEquipo').html(evt);
            
            setTimeout(function () {
                $('#busquedaEquipo').bPopup({
                    easing: 'easeOutBack',
                    speed: 50,
                    transition: 'slideDown'
                });
            }, 50);
           
            inicializarInterfazAgregarPto();

        },
        error: function (err) {
            alert("Error al mostrar Ventana para agregar puntos");
        }
    });
}


function mostrarListaEquipos() {
    
    var empresa = -1;
    var formato = $('#hfFormato').val();
    var idHoja = parseInt($('#cbHoja').val()) || -1;
    var formatoOrigen = parseInt($('#idFormatoOrigen').val()) || 0;

    $.ajax({
        type: 'POST',
    
        url: controlador + "formatomedicion/ListaEquipos",
        data: {
            empresa: empresa,
            formato: formato,
            hoja: idHoja,
            formatoOrigen: formatoOrigen
        },
        success: function (evt) {

            $('#listEquipos').html(evt);
            var oTable = $('#tablaPtos').dataTable({
                "bJQueryUI": true,
                "scrollY": 650,
                "scrollX": true,
                "sDom": 't',
                "ordering": formatoOrigen <= 0,
                "iDisplayLength": 400,
                "sPaginationType": "full_numbers"
            });

        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function inicializarInterfazAgregarPto() {
    
    $('#idFamilia').unbind();
    $('#idFamilia').on('change', function () {        
        listarCentral(EMPRESA_TODOS, $(this).val());
    });
    
    var num = 0;
    $('#cbEmpresa').unbind();
    
    $('#cbEmpresa').on('click ', function () {
        if (num == 0)
            listarEmpresa(ORIGENLECTURA_HIDROLOGIAFINAL);
        num++;
    });

    $('#btnAgregarEquipo').unbind();
    $('#btnAgregarEquipo').on('click', function () {
        agregarNuevoEquipo();
    });

    listarCentral(EMPRESA_TODOS, -2);

    
}

function listarEmpresa(origlect) {
    $("#cbEmpresa").empty();
    $("#cbEmpresa").append('<option value="0" selected="selected"> [Seleccionar Empresa] </option>');

    $.ajax({
        type: 'POST',
        url: controlador + "formatomedicion/ListarEmpresas",
        data: {
            origlectcodi: origlect
        },
        success: function (model) {
            cargarComboEmpresa(model.ListaEmpresa2, 0);

            $('#cbEmpresa').unbind();
            $('#cbEmpresa').change(function () {                                                
                var empresa1 = parseInt($('#cbEmpresa').val()) || 0;
                listarFamilia(empresa1, ORIGENLECTURA_HIDROLOGIAFINAL);                
            });            
            
            var empresa = parseInt($('#cbEmpresa').val()) || 0;
            listarFamilia(empresa, ORIGENLECTURA_HIDROLOGIAFINAL);
            
        },
        error: function (err) {
            alert("Error al mostrar lista de empresas");
        }
    });
}

function cargarComboEmpresa(lista, id) {
    $("#cbEmpresa").empty();
    if (id <= 0 && lista.length != 1)
        $("#cbEmpresa").append('<option value="0" selected="selected"> [Seleccionar Empresa] </option>');
    for (var i = 0; i < lista.length; i++) {
        var regEmp = lista[i];
        var selEmp = id == regEmp.Emprcodi ? 'selected="selected"' : '';
        $("#cbEmpresa").append('<option value=' + regEmp.Emprcodi + ' ' + selEmp + '  >' + regEmp.Emprnomb + '</option>');
    }
}

function listarCentral(empresa, familia) {
    $("#idequipo").empty();
    $("#idequipo").append('<option value="0" selected="selected"> [Seleccionar Equipo] </option>');

    $.ajax({
        type: 'POST',
        url: controlador + "formatomedicion/listarequipo",
        data: {
            empresa: empresa, familia: familia
        },
        global: false,
        success: function (model) {
            cargarComboCentral(model.ListaEquipo, 0);
        },
        error: function (err) {
            alert("Error al mostrar lista de equipos");
        }
    });
}

function cargarComboCentral(lista, id) {
    $("#idequipo").empty();
    if (id <= 0 && lista.length != 1)
        $("#idequipo").append('<option value="0" selected="selected"> [Seleccionar Equipo] </option>');
    for (var i = 0; i < lista.length; i++) {
        var regEq = lista[i];
        var selEq = id == regEq.Equicodi ? 'selected="selected"' : '';
        $("#idequipo").append('<option value=' + regEq.Equicodi + ' ' + selEq + '  >' + regEq.Equinomb + '</option>');
    }
}

function listarFamilia(empresa, origlect) {
    $("#idFamilia").empty();
    $("#idFamilia").append('<option value="0" selected="selected"> [Seleccionar Tipo de Equipo] </option>');

    $.ajax({
        type: 'POST',
        url: controlador + "formatomedicion/ListarFamilia",
        data: {
            empresa: empresa,
            origlectcodi: origlect
        },
        success: function (model) {
            cargarComboFamilia(model.ListaFamilia, 0);

            $('#idFamilia').unbind();
            $('#idFamilia').change(function () {
                                
                var empresa1 = parseInt($('#cbEmpresa').val()) || 0;
                var familia1 = parseInt($("#idFamilia").val()) || 0;

                listarCentral(empresa1, familia1, ORIGENLECTURA_HIDROLOGIAFINAL);
            });
            
            var empresa = parseInt($('#cbEmpresa').val()) || 0;
            var familia = parseInt($("#idFamilia").val()) || 0;

            listarCentral(empresa, familia, ORIGENLECTURA_HIDROLOGIAFINAL);
        },
        error: function (err) {
            alert("Error al mostrar lista de Tipo de equipos");
        }
    });
}


function cargarComboFamilia(lista, id) {
    $("#idFamilia").empty();
    if (id <= 0 && lista.length != 1)
        $("#idFamilia").append('<option value="0" selected="selected"> [Seleccionar Tipo de Equipo] </option>');
    for (var i = 0; i < lista.length; i++) {
        var reg = lista[i];
        var sel = id == reg.Famcodi ? 'selected="selected"' : '';
        $("#idFamilia").append('<option value=' + reg.Famcodi + ' ' + sel + '  >' + reg.Famnomb + '</option>');
    }
}

function agregarNuevoEquipo() {
    
    var formato = $('#hfFormato').val();
    var empresa = $('#cbEmpresa').val();
    var equipo = $('#idequipo').val();

    if (equipo == 0) {
        alert("Seleccionar equipo");
    }
    else {
        
        $('#busquedaEquipo').bPopup().close();
        $.ajax({
            type: 'POST',
            url: controlador + 'formatomedicion/AgregarEquipo',
            dataType: 'json',
            data: {
                empresa: empresa,
                formato: formato,
                equipo: equipo
                
            },
            cache: false,
            success: function (res) {
                switch (res.Resultado) {
                    case 1:
                        if (res.Resultado == 1) {
                            
                            mostrarListaEquipos();
                            
                        }
                        break;
                    case -1:
                        alert("Error en BD ptos de medicion: " + res.Descripcion);
                        break;
                    case 0:
                        alert("Ya existe la información ingresada");
                        break;
                }

            },
            error: function (err) {
                alert("Error al grabar equipo");
            }
        });
        
    }

}

function recargar() {
    var codigoApp = parseInt($("#hdCodigoApp").val()) || 0;
    document.location.href = controlador + "formatomedicion/Index?app=" + codigoApp;
}

function eliminarPto(idFormato, tipoInfo, tptomedi, idOrden, idPto) {
 
    var idEmpresa = -1;
    
    if (confirm("¿Desea eliminar el equipo? \n")) {

        var idHoja = ($('#hfIndicadorHoja').val() == 'S') ? $('#cbHoja').val() : -1;

        $.ajax({
            type: 'POST',
            url: controlador + "formatomedicion/EliminarPto",
            data: {
                idEmpresa: idEmpresa,
                idFormato: idFormato,
                tipoInfo: tipoInfo,
                tptomedi: tptomedi,
                idOrden: idOrden,
                idPunto: idPto,
                hoja: idHoja
            },
            success: function (evt) {
                mostrarListaEquipos();
            },
            error: function (err) {
                alert("Error al mostrar Ventana para eliminar equipos");
            }
        });
    }
}