var controlador = siteRoot + 'ReportesMedicion/formatoreporte/';
var oTable;

$(function () {
    $("#btnOcultarMenu").click();

    $("#btnPtoCal").click(function () { window.location.href = siteRoot + 'ReportesMedicion/formatoreporte/IndexPtoCal'; })

    $('#btnRegresar').click(function () {
        recargar();
    });
    $('#btnBuscar').click(function () {
        mostrarListaPto();
    });
    $('#btnPunto').click(function () {
        nuevoPto();
    });

    $('#btnActualizar').click(function () {
        editarPto();
    });
    $("#btnCancelar").click(function () { $('#popupmpto').bPopup().close(); });
    mostrarListaPto();
});

function mostrarListaPto() {
    var ptomedicodi = $('#hfPtocalculado').val();

    $.ajax({
        type: 'POST',
        url: controlador + "ListaPtoCalculado",
        data: {
            ptomedicodi: ptomedicodi
        },
        success: function (evt) {

            $('#listpto').html(evt);

            oTable = $('#tablaPtos').dataTable({
                "bJQueryUI": true,
                "scrollY": 650,
                "scrollX": true,
                "sDom": 'ft',
                "ordering": false,

                "iDisplayLength": 400,
                "sPaginationType": "full_numbers"
            });
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function recargar() {
    window.history.back();
}

/////////////////////////////////////////////////
/// Nuevo Punto al Calculado
/////////////////////////////////////////////////
function nuevoPto() {
    var pto = $('#hfPtocalculado').val();
    $.ajax({
        type: 'POST',
        url: controlador + "MostrarAgregarPtoACalculado",
        data: {
            pto: pto
        },
        success: function (evt) {
            $('#agregarPto').html(evt);
            setTimeout(function () {
                $('#busquedaEquipo').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
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

function inicializarInterfazAgregarPto() {
    $("#idorigenlectura").val(parseInt($("#hfOriglectcodi").val()) || 0);
    $("#idLectura").val(0);

    $('#btnAgregarPto').unbind();
    $('#btnAgregarPto').on('click', function () {
        agregarNuevoPto();
    });

    $('#cbTipoPto').unbind();
    $('#cbTipoPto').on('change', function () {
        var tipoPto = $('#cbTipoPto').val();
        if (tipoPto == "N") { viewTr(true); } else { viewTr(false); }

        if (tipoPto == "N") {
            var tipEmpresa = parseInt($('#cbTipoEmpresa').val()) || 0;
            var origlectcodi = parseInt($('#idorigenlectura').val()) || 0;

            listarEmpresa(tipEmpresa, origlectcodi);
        }
    });

    $('#idorigenlectura').unbind();
    $('#idorigenlectura').on('change', function () {
        var tipoPto = $('#cbTipoPto').val();
        var origlectcodi = parseInt($('#idorigenlectura').val()) || 0;

        listarLecturas(origlectcodi);

        if (tipoPto == "S") {
            var emprcodi = parseInt($('#cbEmpresa').val()) || 0;
            var equicodi = parseInt($('#idequipo').val()) || 0;
            var origlectcod = parseInt($('#idorigenlectura').val()) || 0;

            var tipoEmp = parseInt($('#cbTipoEmpresa').val()) || 0;

            listarEmpresa(tipoEmp, origlectcodi);
            listarptoCal(emprcodi, equicodi, origlectcod);
        } else {
            $('#cbTipoEmpresa').val(-2);
            var tipoEmp = parseInt($('#cbTipoEmpresa').val()) || 0;

            listarEmpresa(tipoEmp, origlectcodi);
        }
    });

    $('#cbEmpresa').unbind();
    $('#cbEmpresa').change(function () {
        var tipoPto = $('#cbTipoPto').val();

        var origlectcodi = parseInt($('#idorigenlectura').val()) || 0;
        var empresa = parseInt($('#cbEmpresa').val()) || 0;
        listarFamilia(empresa, origlectcodi);

        if (tipoPto == "S") {
            var emprcodi = parseInt($('#cbEmpresa').val()) || 0;
            var equicodi = parseInt($('#idequipo').val()) || 0;
            var origlectcod = parseInt($('#idorigenlectura').val()) || 0;

            listarptoCal(emprcodi, equicodi, origlectcod);
        }
    });

    $('#idLectura').unbind();
    $('#idLectura').on('change', function () {
        var origlectcodi = parseInt($('#idorigenlectura').val()) || 0;
        var equipo = parseInt($('#idequipo').val()) || 0;
        var lectcodi = parseInt($('#idLectura').val()) || 0;

        listarpto(equipo, origlectcodi, lectcodi);
    });

    $('#idMedida').unbind();
    $('#idMedida').on('change', function () {
        var idmedida = parseInt($('#idMedida').val()) || 0;
        listarTptomedicodi('idTipoMedida', idmedida, -1);
    });

    $('#btnFiltroPto').click(function () {
        mostrarFiltrosSegunPtomedicodi();
    });

    $("#fila_tipo_empresa").hide();
}

function viewTr(x) {
    for (z = 1; z < 5; z++) {
        if (x) { $("#id" + z).show(); } else { $("#id" + z).hide(); }
    }

    if (x) { $("#id5").hide(); } else { $("#id5").show(); }
}

function agregarNuevoPto() {
    var medida_ = "0";
    var puntoCalculado = $('#hfPtocalculado').val();
    var punto = $('#idPunto').val();
    if ($('#cbTipoPto').val() == "S") { punto = $('#idPtosCal').val(); }
    var empresa = $('#cbEmpresa').val();
    var factor = $('#factorNuevo').val();
    var lectcod = $('#idLectura').val();
    var potencia = $('#factorPotencia').val();
    if (lectcod == "") { lectcod = "-1"; }
    var punto_ = "-1", tipmedida_ = "-1", medida_ = "-1";
    if ($('#cbTipoPto').val() == "S") { punto_ = punto.split('/')[0]; }
    else {
        medida_ = parseInt($("#idMedida").val()) || 0;;
        punto_ = punto.split('/')[0];
        tipmedida_ = parseInt($("#idTipoMedida").val()) || 0;;
    }
    var frecuencia_ = $('#idFrecuencia').val();

    var msj = '';
    if (punto == 0) {
        msj += "Seleccionar punto de medición" + "\n";
    }
    if (factor.length == 0) {
        msj += "Debe ingresar factor" + "\n";
    }

    if (msj == '') {
        $.ajax({
            type: 'POST',
            url: controlador + 'AgregarPtoAPtoCalculado',
            dataType: 'json',
            data: {
                empresa: empresa,
                punto: punto_,
                puntoCalculado: puntoCalculado,
                factor: factor,
                lectcodi: lectcod,
                medida: medida_,
                tipmedida: tipmedida_,
                cal: $('#cbTipoPto').val(),
                frecuencia: frecuencia_,
                potencia: potencia,
            },
            cache: false,
            success: function (model) {
                if (model.StrResultado != '-1') {
                    switch (model.Resultado) {
                        case 1:
                            alert("Registro exitoso")
                            $('#busquedaEquipo').bPopup().close();
                            mostrarListaPto();
                            break;
                        case -1:
                            alert("Error en BD ptos de medicion");
                            break;
                        case 0:
                            alert("Ya existe la información ingresada");
                            break;
                    }
                }
                else {
                    alert("Error en eliminar: " + model.StrMensaje);
                }
            },
            error: function (err) {
                alert("Error al grabar punto de medición");
            }
        });
    } else {
        alert(msj);
    }
}

function mostrarFiltrosSegunPtomedicodi() {
    var origlectcod = $('#idorigenlectura').val() || 0;
    var ptomedicodi = $("#txtFiltroPo").val() || 0;

    $.ajax({
        type: 'POST',

        dataType: 'json',
        url: controlador + "ListarDataFiltroXPtomedicodi",
        data: {
            tpto: $("#cbTipoPto").val(),
            ptomedicodi: ptomedicodi,
            origlectcodi: origlectcod
        },
        success: function (model) {
            if (model.StrResultado != -1) {
                if (model.Ptomedicodi > 0) {
                    $("#cbTipoEmpresa").val(0);

                    cargarComboEmpresa(model.ListaEmpresa, model.IdEmpresa);

                    cargarComboFamilia(model.ListaFamilia, model.IdFamilia);

                    cargarComboEquipo(model.ListaEquipo, model.IdEquipo);

                    $("#idLectura").val(model.IdLectura);

                    cargarComboPto(model.ListaPtos, model.Ptomedicodi);

                    var opcion = $('#idPunto option:selected').val();
                    setearMedidaSegunPto(opcion);
                } else {
                    $("#cbTipoEmpresa").val(model.IdTipoempresa);
                    $("#cbEmpresa").val(model.IdEmpresa);
                    $("#idFamilia").val(model.IdFamilia);
                    $("#idequipo").val(model.IdEquipo);
                    $("#idLectura").val(model.IdLectura);
                    $("#idPunto").val(model.Ptomedicodi);
                }
            } else {
                alert(model.StrResultado);
            }
        },
        error: function (err) {
            alert("Error al mostrar lista de Tipo de medida");
        }
    });
}

/////////////////////////////////////////////////
/// Nuevo Punto de Calculado
/////////////////////////////////////////////////
function modificarPtoDeCalculado(relptocodi, factor, tipoinfocodi, ptomedicodi, nombrePto, Origlectcodi, Lectcodi, tipoptomedicodi, resolucion, potencia) {
    $('#hfRelptocodi').val(relptocodi);
    $('#factor').val(factor);
    $('#potencia').val(potencia);


    $("#modifPto").html(ptomedicodi + " - " + nombrePto);
    $("#modifidorigenlectura").unbind();
    $("#modifidorigenlectura").val(Origlectcodi);
    $('#modifidorigenlectura').on('change', function () {
        var origenlectura = $('#modifidorigenlectura').val();
        listarLecturas(origenlectura, 'modifidLectura', -1);
    });
    listarLecturas(Origlectcodi, 'modifidLectura', Lectcodi);

    $("#modifidMedida").unbind();
    $("#modifidMedida").val(tipoinfocodi);
    $('#modifidMedida').on('change', function () {
        var medida = $('#modifidMedida').val();
        listarTptomedicodi('modifidTipoMedida', medida, -1);
    });
    $("#modifidTipoMedida").unbind();
    listarTptomedicodi('modifidTipoMedida', tipoinfocodi, tipoptomedicodi);

    $("#modifidFrecuencia").val(resolucion);

    setTimeout(function () {
        $('#popupmpto').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown'
        });
    }, 50);
}

function editarPto() {
    var relptocodi = $('#hfRelptocodi').val();
    var factor = $('#factor').val();
    var potencia = $('#potencia').val();
    var newLectcodi = parseInt($("#modifidLectura").val()) || -1;
    var newTipoinfocodi = parseInt($("#modifidMedida").val()) || -1;
    var newtipmedida = parseInt($("#modifidTipoMedida").val()) || -1;
    var newFrecuencia = parseInt($("#modifidFrecuencia").val());

    $.ajax({
        type: 'POST',
        url: controlador + "EditarPtoDeCalculado",
        dataType: 'json',
        data: {
            relptocodi: relptocodi,
            factor: factor,
            potencia: potencia,
            newLectcodi: newLectcodi,
            newTipoinfocodi: newTipoinfocodi,
            newtipmedida: newtipmedida,
            newresolucion: newFrecuencia
        },
        success: function (model) {
            if (model.StrResultado != '-1') {
                alert("Se actualizó correctamente");
                $('#popupmpto').bPopup().close();
                mostrarListaPto();
            }
            else {
                alert("Error en actualizar: " + model.StrMensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function deletePtoDeCalculado(relptocodi) {
    $.ajax({
        type: 'POST',
        url: controlador + 'DeletePto',
        dataType: 'json',
        data: {
            relptocodi: relptocodi
        },
        cache: false,
        success: function (model) {
            if (model.StrResultado != '-1') {
                alert("Se eliminó correctamente");
                mostrarListaPto();
            }
            else {
                alert("Error en eliminar: " + model.StrMensaje);
            }
        },
        error: function (err) {
            alert("Error al grabar punto de medición");
        }
    });
}

/////////////////////////////////////////////////
/// Filtros
/////////////////////////////////////////////////

function listarEmpresa(idTipoEmpresa, origlect) {
    $("#cbEmpresa").empty();
    $("#cbEmpresa").append('<option value="0" selected="selected"> [Seleccionar Empresa] </option>');

    $.ajax({
        type: 'POST',
        url: controlador + "ListarEmpresas",
        data: {
            idTipoEmpresa: idTipoEmpresa,
            origlectcodi: origlect
        },

        success: function (model) {
            cargarComboEmpresa(model.ListaEmpresa, 0);

            $('#cbEmpresa').unbind();
            $('#cbEmpresa').change(function () {
                var tipoPto = $('#cbTipoPto').val();

                var origlectcodi = parseInt($('#idorigenlectura').val()) || 0;
                var empresa = parseInt($('#cbEmpresa').val()) || 0;
                listarFamilia(empresa, origlectcodi);

                if (tipoPto == "S") {
                    var emprcodi = parseInt($('#cbEmpresa').val()) || 0;
                    var equicodi = parseInt($('#idequipo').val()) || 0;
                    var origlectcod = parseInt($('#idorigenlectura').val()) || 0;

                    listarptoCal(emprcodi, equicodi, origlectcod);
                }
            });

            var tipoPto = $('#cbTipoPto').val();

            var origlectcodi = parseInt($('#idorigenlectura').val()) || 0;
            var empresa = parseInt($('#cbEmpresa').val()) || 0;
            listarFamilia(empresa, origlectcodi);

            if (tipoPto == "S") {
                var emprcodi = parseInt($('#cbEmpresa').val()) || 0;
                var equicodi = parseInt($('#idequipo').val()) || 0;
                var origlectcod = parseInt($('#idorigenlectura').val()) || 0;

                listarptoCal(emprcodi, equicodi, origlectcod);
            }
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

function listarFamilia(empresa, origlect) {
    $("#idFamilia").empty();
    $("#idFamilia").append('<option value="0" selected="selected"> [Seleccionar Tipo de Equipo] </option>');

    $.ajax({
        type: 'POST',
        url: controlador + "ListarFamilia",
        data: {
            empresa: empresa,
            origlectcodi: origlect
        },

        success: function (model) {
            cargarComboFamilia(model.ListaFamilia, 0);

            $('#idFamilia').unbind();
            $('#idFamilia').change(function () {
                var origlectcodi = parseInt($('#idorigenlectura').val()) || 0;
                var empresa = parseInt($('#cbEmpresa').val()) || 0;
                var familia = parseInt($("#idFamilia").val()) || 0;

                listarequipo(empresa, familia, origlectcodi);

                if ($("#cbTipoPto").val() == "S") {
                    var emprcodi = $('#cbEmpresa').val();
                    var equicodi = $('#idequipo').val();
                    var origlectcod = $('#idorigenlectura').val();
                    //alert(emprcodi + '/' + equicodi + '/' + origlectcod);
                    listarptoCal(emprcodi, equicodi, origlectcod);
                }
            });

            var origlectcodi = parseInt($('#idorigenlectura').val()) || 0;
            var empresa = parseInt($('#cbEmpresa').val()) || 0;
            var familia = parseInt($("#idFamilia").val()) || 0;

            listarequipo(empresa, familia, origlectcodi);
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

function listarequipo(empresa, familia, origlect) {
    $("#idequipo").empty();
    $("#idequipo").append('<option value="0" selected="selected"> [Seleccionar Equipo] </option>');

    $.ajax({
        type: 'POST',
        url: controlador + "listarequipo",
        data: {
            origlectcodi: origlect,
            empresa: empresa,
            familia: familia
        },

        success: function (model) {
            cargarComboEquipo(model.ListaEquipo, 0);

            $('#idequipo').unbind();
            $('#idequipo').change(function () {
                var origlectcodi = parseInt($('#idorigenlectura').val()) || 0;
                var lectcodi = parseInt($('#idLectura').val()) || 0;
                var idEquipo = parseInt($('#idequipo').val()) || 0;

                listarpto(idEquipo, origlectcodi, lectcodi);

                if ($("#cbTipoPto").val() == "S") {
                    var emprcodi = $('#cbEmpresa').val();
                    var equicodi = $('#idequipo').val();
                    var origlectcod = $('#idorigenlectura').val();
                    //alert(emprcodi + '/' + equicodi + '/' + origlectcod);
                    listarptoCal(emprcodi, equicodi, origlectcod);
                }
            });

            var origlectcodi = parseInt($('#idorigenlectura').val()) || 0;
            var lectcodi = parseInt($('#idLectura').val()) || 0;
            var idEquipo = parseInt($('#idequipo').val()) || 0;

            listarpto(idEquipo, origlectcodi, lectcodi);
        },
        error: function (err) {
            alert("Error al mostrar lista de equipos");
        }
    });
}

function cargarComboEquipo(lista, id) {
    $("#idequipo").empty();
    if (id <= 0 && lista.length != 1)
        $("#idequipo").append('<option value="0" selected="selected"> [Seleccionar Equipo] </option>');
    for (var i = 0; i < lista.length; i++) {
        var regEq = lista[i];
        var selEq = id == regEq.Equicodi ? 'selected="selected"' : '';
        $("#idequipo").append('<option value=' + regEq.Equicodi + ' ' + selEq + '  >' + regEq.Equinomb + '</option>');
    }
}

function listarLecturas(origlect, idElemento, lectura) {
    var cb_ = idElemento == undefined || idElemento == null ? "idLectura" : idElemento;
    $('option', '#' + cb_).remove();
    $.ajax({
        type: 'POST',
        url: controlador + "ListarLecturas",
        data: {
            origlectcodi: origlect
        },

        success: function (model) {
            var aData = model.ListaLectura;
            $('#' + cb_).get(0).options.length = 0;
            $('#' + cb_).get(0).options[0] = new Option(" [Seleccione Lectura] ", -1);
            $.each(aData, function (i, item) {
                $('#' + cb_).get(0).options[$('#' + cb_).get(0).options.length] = new Option(item.Lectnomb, item.Lectcodi);
            });
            if (lectura != undefined && lectura != null) {
                $('#' + cb_).val(lectura);
            }
        },
        error: function (err) {
            alert("Error al mostrar lista de equipos");
        }
    });

}

function listarpto(equipo, origlectcodi_, lectcodi_) {
    $("#idPunto").empty();

    $.ajax({
        type: 'POST',

        url: controlador + "listarpto",
        data: {
            equipo: equipo,
            origlectcodi: origlectcodi_,
            lectcodi: lectcodi_,
            tpto: $("#cbTipoPto").val()
        },
        success: function (model) {
            cargarComboPto(model.ListaPtos, model.Ptomedicodi);

            $('#idPunto').unbind();
            $('#idPunto').on('change', function () {
                var opcion = $('#idPunto option:selected').val();
                setearMedidaSegunPto(opcion);
            });

            var opcion = $('#idPunto option:selected').val();
            setearMedidaSegunPto(opcion);
        },
        error: function (err) {
            alert("Error al mostrar lista de ptos");
        }
    });
}

function cargarComboPto(lista, id) {
    $("#idPunto").empty();

    if (id <= 0 && lista.length != 1)
        $("#idPunto").append('<option value="0" selected="selected"> [Seleccionar Pto] </option>');
    for (var i = 0; i < lista.length; i++) {
        var reg = lista[i];
        var selPto = id == reg.Ptomedicodi ? 'selected="selected"' : '';
        var nombrePunto = reg.Ptomedicodi + " / " + reg.Ptomedielenomb + " - " + reg.Tipoptomedinomb + " - " + reg.Tipoinfoabrev;
        var codigoPunto = reg.Ptomedicodi + "/" + reg.Tipoinfocodi + "/" + reg.Tipoptomedicodi + "/" + reg.Tipoinfoabrev + "/" + reg.Tipoptomedinomb;
        $("#idPunto").append('<option value=' + codigoPunto + ' ' + selPto + '  >' + nombrePunto + '</option>');
    }
}

function setearMedidaSegunPto(opcion) {
    if (opcion != '0') {
        var param = opcion.split("/");
        var tipoinfocodi = parseInt(param[1]) || 0;
        var tipoptomedicion = parseInt(param[2]) || 0;

        $('#idMedida').val(tipoinfocodi);
        //$('#idTipoMedida').val(param[4]);

        listarTptomedicodi('idTipoMedida', tipoinfocodi, tipoptomedicion);
    }
}

function listarTptomedicodi(idelemento, tipoinfocodi, defecto) {
    $('#' + idelemento).parent().parent().hide();

    $("#" + idelemento).empty();
    $('#' + idelemento).append('<option value="-1" selected="selected"> [Seleccione Tipo de Medida] </option>');

    $.ajax({
        type: 'POST',

        dataType: 'json',
        url: controlador + "ListarTptomedicion",
        data: {
            tipoinfocodi: tipoinfocodi
        },
        success: function (model) {
            var lista = model.ListaTipoMedidas;
            if (lista.length > 0) {
                for (var i = 0; i < lista.length; i++) {
                    $('#' + idelemento).append('<option value=' + lista[i].Tipoptomedicodi + '>' + lista[i].Tipoptomedinomb + '</option>');
                }

                $('#' + idelemento).val(defecto);
                $('#' + idelemento).parent().parent().show();
            }
        },
        error: function (err) {
            alert("Error al mostrar lista de Tipo de medida");
        }
    });
}

function listarptoCal(emprcod, equicod, origlectcod) {
    var cb_ = "idPtosCal";
    $('option', '#' + cb_).remove();
    $.ajax({
        type: 'POST',

        url: controlador + "listarptoCal",
        data: {
            emprcodi: emprcod,
            equicodi: equicod,
            origlectcodi: origlectcod,
            tpto: "S"
        },

        success: function (aData) {
            $('#' + cb_).get(0).options.length = 0;
            $('#' + cb_).get(0).options[0] = new Option(" [Seleccione Pto Calculado] ", "");
            $.each(aData, function (i, item) {
                $('#' + cb_).get(0).options[$('#' + cb_).get(0).options.length] = new Option(item.Ptomedicodi + " / " + item.Ptomedidesc, item.Ptomedicodi);
            });
        },
        error: function (err) {
            alert("Error al mostrar lista de ptos");
        }
    });
}
