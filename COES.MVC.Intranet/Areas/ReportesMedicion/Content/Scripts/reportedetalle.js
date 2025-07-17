var controlador = siteRoot + 'ReportesMedicion/formatoreporte/';
var oTable;
const IMG_FILE = '<img src="' + siteRoot + 'Content/Images/file.png" title="Ver puntos" alt="Ver puntos">';
const IMG_EDITAR = '<img src="' + siteRoot + 'Content/Images/btn-edit.png" title="Editar Punto" alt="Editar Punto">';
const IMG_CANCELAR = '<img src="' + siteRoot + 'Content/Images/btn-cancel.png" title="Eliminar punto" alt="Eliminar Punto">';

var LISTA_PTOS = [];

$(function () {
    $("#btnOcultarMenu").click();

    $('#btnRegresar').click(function () {
        recargar();
    });
    $('#btnBuscar').click(function () {
        mostrarListaPto();
    });
    $('#btnPuntoCalculado').click(function () {
        window.location.href = controlador + 'IndexPtoCal';
    });
    $('#btnActualizar').click(function () {
        editarPto();
    });
    $('#btnPunto').click(function () {
        nuevoPto();
    });

    $("#btnPtoCal").click(function () { window.location.href = siteRoot + 'ReportesMedicion/formatoreporte/IndexPtoCal'; })

    $('#listpto').css("width", ($('#mainLayout').width() - 40) + "px");

    console.log($('#hfIndCopiado').val());
    if ($('#hfIndCopiado').val() == "S") {
        $('.idTrCopiarDatos').show();
    }
    else {
        $('.idTrCopiarDatos').hide();
    }

    mostrarListaPto();
});

function mostrarListaPto() {
    var reporte = $('#hfReporte').val();
    var esEditable = parseInt($("#hfReporteEditable").val());
    var esEditableReporte = esEditable == 1;

    $.ajax({
        type: 'POST',
        url: controlador + "ListaPtoMedicion",
        data: {
            reporte: reporte,
            esEditable: esEditable
        },
        success: function (evt) {
            LISTA_PTOS = [];
            if (evt.StrResultado != "-1") {
                LISTA_PTOS = evt.ListaReportPto;
                var htmlTabla = dibujarTablaListadoPtos(evt.ListaReportPto, esEditableReporte);
                $("#listpto").html(htmlTabla);

                oTable = $('#tablaPtos').dataTable({
                    "bJQueryUI": true,
                    "scrollY": 650,
                    "scrollX": true,
                    "sDom": 'ft',
                    "ordering": true,
                    "iDisplayLength": 400,
                    "sPaginationType": "full_numbers"
                });

                if (esEditableReporte) {
                    oTable.rowReordering({ sURL: controlador + "UpdateOrderDetalleReporte?reporcodi=" + reporte });
                }

            } else {
                alert("Ha ocurrido un error" + evt.StrMensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function dibujarTablaListadoPtos(listaPtos, esEditableReporte) {
    var thCodPto = '';
    var idRpt = parseInt($("#hfReporte").val());
    if (idRpt == 71) {
        thCodPto = `<th style="background-color: #42b12e;">Pto equivalente Flujo de potencia IEOD</th>`;
    }

    var cadena = '';
    cadena += `
    <table class="pretty tabla-icono" cellspacing="0" width="100%" id="tablaPtos">
       <thead>
           <tr>
                
                <th>Orden</th>
                <th>Pto</th>
                <th>Origen Lectura</th>
                <th>Empresa</th>
                <th>Ubicación</th>
                <th>Tipo</th>
                <th>Equipo</th>
                <th style="background-color: #42b12e;">Nombre en reporte</th>
                ${thCodPto}
                <th>Lectura</th>
                <th>Medida</th>
                <th>Calculado</th>
                <th>Estado</th>
                <th>Resolución</th>
                <th style="width: 80px;">ACCIÓN</th>
            
            </tr>
        </thead>
        <tbody>
    `;

    for (var key in listaPtos) {
        var item = listaPtos[key];

        var title = `Datos del punto de medición\n Código: ${item.Ptomedicodi}\n Nombre: ${item.Ptomedibarranomb}\n Abreviatura: ${item.Ptomedielenomb}\n Descripción: ${item.Ptomedidesc}.`;
        var tdCodPto = '';
        if (idRpt == 71) {
            tdCodPto = `<td style='text-align: left; '>${item.RepptoequivptoDesc}</td>`;
        }

        cadena += `
            <tr id="${item.Repptoorden}">
                <td>${item.Repptoorden}</td>
                <td>${item.Ptomedicodi}</td>
                <td>${item.Origlectnombre}</td>
                <td>${item.Emprnomb}</td>
                <td>${item.Areanomb}</td>
                <td>${item.Famabrev}</td>
                <td>${item.Equiabrev}</td>
                <td style="font-weight: bold; text-align: left; padding-left: 5px; word-break: break-all;" title="${title}">${item.DescPto}</td>
                ${tdCodPto}
                <td>${item.Lectnomb}</td>
                <td>${item.Tipoptomedinomb} (${item.Tipoinfoabrev})</td>
                <td>
        `;
        if (item.PtomediCalculado == "S") {
            cadena += `
                    ${item.PtomediCalculadoDescrip}
            `;
        }
        cadena += `
                </td>
                <td>${item.RepptoestadoDescrip}</td>
                <td>${item.RepptotabmedDesc}</td>
                <td style="text-align: right; padding-right: 8px;">
        `;
        if (item.PtomediCalculado == "S") {
            cadena += `                    
                    <a href="JavaScript:verPuntoCalculado(${item.Reporcodi}, ${item.Ptomedicodi})"> ${IMG_FILE} </a>
            `;
        }
        if (esEditableReporte) {
            cadena += `
                    <a href="JavaScript:modificarPto(${item.Repptocodi})"> ${IMG_EDITAR} </a>
                    <a href="JavaScript:eliminarPto(${item.Repptocodi})"> ${IMG_CANCELAR} </a>
            `;
        }

        cadena += `
                </td>
           </tr >           
        `;
    }

    cadena += `                   
        </tbody >
    </table >
    `;

    return cadena;
}

function recargar() {
    document.location.href = controlador + "index";
}

function modificarPto(repptocodi) {

    var objPto = LISTA_PTOS.find(x => x.Repptocodi === repptocodi);

    var tipoinfocodi = objPto.Tipoinfocodi;
    var ptomedicodi = objPto.Ptomedicodi;
    var activo = objPto.Repptoestado;
    var nombrePto = objPto.Ptomedibarranomb;
    var repptonomb = objPto.Repptonomb;
    var Origlectcodi = objPto.Origlectcodi;
    var Lectcodi = objPto.Lectcodi;
    var tipoptomedicodi = objPto.Tipoptomedicodi;
    var resolucion = objPto.Repptotabmed;
    var color = objPto.Repptocolorcelda;
    var repptoequivpto = objPto.Repptoequivpto;
    var indicadorcopiado = objPto.Repptoindcopiado;

    $('#hfRepptocodi').val(repptocodi);
    $('#hfPunto').val(ptomedicodi);
    $('#hfTipoinfo').val(tipoinfocodi);
    if (activo == 1) {
        $('#idActivo').prop('checked', true);
    } else {
        $('#idActivo').prop('checked', false);
    }

    if (indicadorcopiado == "S") {
        $('#cbCopiarDatosEdit').prop('checked', true);
    } else {
        $('#cbCopiarDatosEdit').prop('checked', false);
    }


    $("#modifPto").html(ptomedicodi + " - " + nombrePto);
    $("#modifNomb").val(repptonomb);
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

    $('#modifColor').val(color);
    $('#modifColor').colorpicker({
        showOn: 'focus',
        displayIndicator: false,
        history: false
    });

    $("#modifPtomedequiv").val(repptoequivpto);
    //solo visible para el reporte "Reporte principal - Demanda por área"
    $(".tr_fila_pto_equiv").hide();
    var idRpt = parseInt($("#hfReporte").val());
    if (idRpt == 71) {
        $(".tr_fila_pto_equiv").show();
    }

    setTimeout(function () {
        $('#popupmpto').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown'
        });
    }, 50);
}

function editarPto() {
    var repptocodi = parseInt($('#hfRepptocodi').val()) || 0;
    var estado = $('#idActivo').is(':checked') ? 1 : 0;
    var newLectcodi = parseInt($("#modifidLectura").val()) || -1;
    var newTipoinfocodi = parseInt($("#modifidMedida").val()) || -1;
    var newTipoptomedicodi = parseInt($("#modifidTipoMedida").val()) || -1;
    var newFrecuencia = parseInt($("#modifidFrecuencia").val());
    var nombre = $("#modifNomb").val();
    var color = $('#modifColor').val();
    var ptomedequiv = parseInt($('#modifPtomedequiv').val()) || 0;
    var indcopiado = $('#cbCopiarDatosEdit').is(':checked') ? "S" : "N";

    $.ajax({
        type: 'POST',
        url: controlador + "EditarPto",
        dataType: 'json',
        data: {
            repptocodi: repptocodi,
            estado: estado,
            newLectcodi: newLectcodi,
            newTipoinfocodi: newTipoinfocodi,
            newTipoptomedicodi: newTipoptomedicodi,
            newFrecuencia: newFrecuencia,
            repptonomb: nombre,
            color: color,
            ptomedequiv: ptomedequiv,
            indcopiado: indcopiado
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

function eliminarPto(repptocodi) {
    if (confirm("¿Desea eliminar el punto de medición del reporte?")) {
        $.ajax({
            type: 'POST',
            url: controlador + "EliminarPtoFromReporte",
            data: {
                repptocodi
            },
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
                alert("Error al mostrar Ventana para agregar puntos");
            }
        });
    }
}

/////////////////////////////////////////////////
/// Nuevo Punto
/////////////////////////////////////////////////

function nuevoPto() {
    $('#agregarPtoCalculado').html('');

    inicializarValoresPopupNuevoPto();

    setTimeout(function () {
        $('#popupAgregarPto').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown'
        });
    }, 50);

    inicializarInterfazAgregarPto();
}

function inicializarValoresPopupNuevoPto() {
    $("#cbTipoPto").val("N");
    var tipoPto = $('#cbTipoPto').val();
    if (tipoPto == "N") { viewTr(true); } else { viewTr(false); }

    if (tipoPto == "N") {
        var tipEmpresa = parseInt($('#cbTipoEmpresa').val()) || 0;
        var origlectcodi = parseInt($('#idorigenlectura').val()) || 0;

        listarEmpresa(tipEmpresa, origlectcodi);
    } else {
        $("#cbEmpresa").val(0);
        $("#idFamilia").val(0);
        $("#idequipo").val(0);
    }
    $("#idorigenlectura").val("0");
    $("#txtFiltroPo").val("");
    $("#cbTipoEmpresa").val("-2");
    $("#cbEmpresa").val("0");
    $("#idFamilia").val("0");
    $("#idequipo").val("0");
    $("#idLectura").val("0");
    $("#idPunto").val("0");
    $("#idPtosCal").val("0");
    $("#idNombre").val("");
    $("#idColorcelda").val("");
    $("#idMedida").val("0");
    $("#idTipoMedida").val("-1");
    $("#idFrecuencia").val("0");

    $("#idCodpuntoequiv").val("");
    //solo visible para el reporte "Reporte principal - Demanda por área"
    $(".tr_fila_pto_equiv").hide();
    var idRpt = parseInt($("#hfReporte").val());
    if (idRpt == 71) {
        $(".tr_fila_pto_equiv").show();
    }
}

function inicializarInterfazAgregarPto() {
    var codigoOrigLec = parseInt($("#hfOriglectcodi").val()) || 0;
    $("#idorigenlectura").val(codigoOrigLec > 0 ? codigoOrigLec : 0);
    $("#idLectura").val(parseInt($("#hfIdLectura").val()));

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
        } else {
            $("#cbEmpresa").val(0);
            $("#idFamilia").val(0);
            $("#idequipo").val(0);
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

    $('#btnFiltroPto').unbind();
    $('#btnFiltroPto').click(function () {
        mostrarFiltrosSegunPtomedicodi();
    });


    $('#idColorcelda').colorpicker({
        showOn: 'focus',
        displayIndicator: false,
        history: false
    });

    $("#fila_tipo_empresa").hide();
}

function viewTr(x) {
    for (z = 1; z < 5; z++) {
        if (x) { $("#id" + z).show(); }
        else { $("#id" + z).hide(); }
    }

    if (x) {
        $("#id5").hide();
        $("#fila_empresa").show();
        $("#fila_tipo_equipo").show();
        $("#fila_equipo").show();
        $("#id6").show();
    }
    else {
        $("#id5").show();
        $("#fila_empresa").hide();
        $("#fila_tipo_equipo").hide();
        $("#fila_equipo").hide();
        $("#id6").hide();
    }
}

function agregarNuevoPto() {
    var medida_ = "0";
    var punto = $('#idPunto').val();
    if ($('#cbTipoPto').val() == "S") { punto = $('#idPtosCal').val(); }
    var reporte = $('#hfReporte').val();
    var empresa = $('#cbEmpresa').val();
    var lectcodi_ = parseInt($('#idLectura').val()) || 0;
    if ($('#cbTipoPto').val() != "S") { medida_ = parseInt($("#idMedida").val()) || 0; }
    var punto_ = punto.split('/')[0];
    var frecuencia_ = $('#idFrecuencia').val();
    var tipoPtomedi = parseInt($("#idTipoMedida").val()) || - 1;
    var nombre_ = $('#idNombre').val();
    var color_ = $('#idColorcelda').val();
    var codptoequiv_ = parseInt($('#idCodpuntoequiv').val()) || 0;
    var indicadorcopiado_ = $('#cbCopiarDatos').is(':checked') ? "S" : "N";

    if (punto == 0) {
        alert("Seleccionar punto de medición");
    }
    else {
        if ($('#cbTipoPto').val() != "S" && frecuencia_ == 0) {
            alert("Seleccionar una frecuencia");
        } else {
            $.ajax({
                type: 'POST',
                url: controlador + 'AgregarPto',
                dataType: 'json',
                data: {
                    empresa: empresa,
                    reporte: reporte,
                    punto: punto_,
                    lectcodi: lectcodi_,
                    medida: medida_,
                    calculado: $('#cbTipoPto').val(),
                    frecuencia: frecuencia_,
                    repptonomb: nombre_,
                    tipoptomedicodi: tipoPtomedi,
                    color: color_,
                    codptoequiv: codptoequiv_,
                    indicadorcopiado: indicadorcopiado_
                },
                cache: false,
                success: function (model) {
                    if (model.StrResultado != '-1') {
                        switch (model.Resultado) {
                            case 1:
                                $('#popupAgregarPto').bPopup().close();
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
        }
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
                    $("#idorigenlectura").val(model.Origlectcodi);

                    $("#cbTipoEmpresa").val(model.IdTipoempresa);

                    cargarComboEmpresa(model.ListaEmpresa, model.IdEmpresa);

                    cargarComboFamilia(model.ListaFamilia, model.IdFamilia);

                    cargarComboEquipo(model.ListaEquipo, model.IdEquipo);

                    cargarComboLectura('idLectura', model.ListaLectura, model.IdLectura);

                    cargarComboPto(model.ListaPtos, model.Ptomedicodi, $("#cbTipoPto").val());

                    var opcion = $('#idPunto option:selected').val();
                    setearMedidaSegunPto(opcion);
                } else {
                    $("#cbTipoEmpresa").val(model.IdTipoempresa);
                    $("#cbEmpresa").val(model.IdEmpresa);
                    $("#idFamilia").val(model.IdFamilia);
                    $("#idequipo").val(model.IdEquipo);
                    $("#idLectura").val(model.IdLectura);
                    $("#idPunto").val(model.Ptomedicodi);
                    $("#idPtosCal").val(model.Ptomedicodi);
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

function verPuntoCalculado(reporte, ptomedicodi) {
    location.href = controlador + "IndexDetalleCalculado?reporte=" + reporte + "&pto=" + ptomedicodi;
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
                var tipoPto = $('#cbTipoPto').val();

                var origlectcodi = parseInt($('#idorigenlectura').val()) || 0;
                var empresa = parseInt($('#cbEmpresa').val()) || 0;
                var familia = parseInt($("#idFamilia").val()) || 0;

                listarequipo(empresa, familia, origlectcodi);

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
            var familia = parseInt($("#idFamilia").val()) || 0;

            listarequipo(empresa, familia, origlectcodi);

            if (tipoPto == "S") {
                var emprcodi = parseInt($('#cbEmpresa').val()) || 0;
                var equicodi = parseInt($('#idequipo').val()) || 0;
                var origlectcod = parseInt($('#idorigenlectura').val()) || 0;

                listarptoCal(emprcodi, equicodi, origlectcod);
            }
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
                var tipoPto = $('#cbTipoPto').val();

                var origlectcodi = parseInt($('#idorigenlectura').val()) || 0;
                var lectcodi = parseInt($('#idLectura').val()) || 0;
                var idEquipo = parseInt($('#idequipo').val()) || 0;

                listarpto(idEquipo, origlectcodi, lectcodi);

                if (tipoPto == "S") {
                    var emprcodi = parseInt($('#cbEmpresa').val()) || 0;
                    var equicodi = parseInt($('#idequipo').val()) || 0;
                    var origlectcod = parseInt($('#idorigenlectura').val()) || 0;

                    listarptoCal(emprcodi, equicodi, origlectcod);
                }
            });

            var tipoPto = $('#cbTipoPto').val();

            var origlectcodi = parseInt($('#idorigenlectura').val()) || 0;
            var lectcodi = parseInt($('#idLectura').val()) || 0;
            var idEquipo = parseInt($('#idequipo').val()) || 0;

            listarpto(idEquipo, origlectcodi, lectcodi);

            if (tipoPto == "S") {
                var emprcodi = parseInt($('#cbEmpresa').val()) || 0;
                var equicodi = parseInt($('#idequipo').val()) || 0;
                var origlectcod = parseInt($('#idorigenlectura').val()) || 0;

                listarptoCal(emprcodi, equicodi, origlectcod);
            }
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
            cargarComboLectura(cb_, aData, lectura);
        },
        error: function (err) {
            alert("Error al mostrar lista de equipos");
        }
    });

}

function cargarComboLectura(cb_, aData, id) {
    $('#' + cb_).get(0).options.length = 0;
    $('#' + cb_).get(0).options[0] = new Option(" [Seleccione Lectura] ", "0");
    $.each(aData, function (i, item) {
        $('#' + cb_).get(0).options[$('#' + cb_).get(0).options.length] = new Option(item.Lectnomb, item.Lectcodi);
    });
    if (id != undefined && id != null) {
        $('#' + cb_).val(id);
    }
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
            var ptomedicodi = $("#txtFiltroPo").val() || 0;
            cargarComboPto(model.ListaPtos, ptomedicodi);

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

function cargarComboPto(lista, id, tipo) {
    if (tipo !== undefined && tipo == "S") {
        $("#idPtosCal").empty();

        if (id <= 0 && lista.length != 1)
            $("#idPtosCal").append('<option value="0" selected="selected"> [Seleccionar Pto] </option>');

        for (var i = 0; i < lista.length; i++) {
            var reg = lista[i];
            var selPto = id == reg.Ptomedicodi ? 'selected="selected"' : '';
            var nombrePunto = reg.Ptomedicodi + " / " + reg.Ptomedielenomb;
            var codigoPunto = reg.Ptomedicodi;
            $("#idPtosCal").append('<option value="' + codigoPunto + '" ' + selPto + '  >' + nombrePunto + '</option>');
        }
    } else {
        $("#idPunto").empty();

        if (id <= 0 && lista.length != 1)
            $("#idPunto").append('<option value="0" selected="selected"> [Seleccionar Pto] </option>');

        for (var i = 0; i < lista.length; i++) {
            var reg = lista[i];
            var selPto = id == reg.Ptomedicodi ? 'selected="selected"' : '';
            var nombrePunto = reg.Ptomedicodi + " / " + reg.Ptomedielenomb + " - " + reg.Tipoptomedinomb + " - " + reg.Tipoinfoabrev;
            var codigoPunto = reg.Ptomedicodi + "/" + reg.Tipoinfocodi + "/" + reg.Tipoptomedicodi + "/" + reg.Tipoinfoabrev + "/" + reg.Tipoptomedinomb;
            $("#idPunto").append('<option value="' + codigoPunto + '" ' + selPto + '  >' + nombrePunto + '</option>');
        }
    }
}

function setearMedidaSegunPto(opcion) {
    if (opcion != undefined && opcion != null && opcion != '0') {
        var param = opcion.split("/");
        var tipoinfocodi = parseInt(param[1]) || 0;
        var tipoptomedicion = parseInt(param[2]) || 0;

        $('#idMedida').val(tipoinfocodi);
        //$('#idTipoMedida').val(param[4]);

        listarTptomedicodi('idTipoMedida', tipoinfocodi, tipoptomedicion);
    } else {
        $('#idTipoMedida').val(-1);
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
    $("#idPtosCal").empty();

    $.ajax({
        type: 'POST',

        url: controlador + "listarptoCal",
        data: {
            emprcodi: emprcod,
            equicodi: equicod,
            origlectcodi: origlectcod,
            tpto: "S"
        },

        success: function (model) {
            cargarComboPto(model, -1, "S");

            $('#idPtosCal').unbind();
            $('#idPtosCal').on('change', function () {
                var opcion = $('#idPtosCal option:selected').val();
                setearMedidaSegunPto(opcion);
            });

            var opcion = $('#idPtosCal option:selected').val();
            setearMedidaSegunPto(opcion);
        },
        error: function (err) {
            alert("Error al mostrar lista de ptos");
        }
    });
}
