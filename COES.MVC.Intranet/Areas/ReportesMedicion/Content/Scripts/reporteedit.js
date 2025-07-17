var controlador = siteRoot + 'hidrologia/'
var altoFila = 25;
var oTableHead;
$(function () {
    buscarHoja();
    $('#btnGrabar').click(function () {
        actualizarFormatoHoja();
    });
    $('#btnCancelar').click(function () {
        cancelar();
    });


    $(window).resize(function () {
        $('#listado').css("width", $('#mainLayout').width() + "px");
    });
});

function cancelar() {
    document.location.href = controlador + "formatomedicion/index";
}

function buscarHoja() {
    mostrarDetalleGeneral();
    mostrarListado();
}

function mostrarDetalleGeneral()
{
    var formato = $('#hfId').val();
    $.ajax({
        type: 'POST',
        url: controlador + "formatomedicion/DetalleGeneralFormato",
        data: {
            formato: formato
        },
        success: function (evt) {

            $('#detalle_master').html(evt);

        },
        error: function () {
            alert("Ha ocurrido un error en detalle master");
        }
    });
}

function mostrarListado() {
    var formato = $('#hfId').val();
    $.ajax({
        type: 'POST',
        url: controlador + "formatomedicion/ListaHoja",
        data: {
            formato: formato
        },
        success: function (evt) {

            $('#detalle').html(evt);

        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function mostrarListaPto(empresa, formato, hoja) {
    $.ajax({
        type: 'POST',
        url: controlador + "formatomedicion/ListaPtoMedicion",
        data: {
            empresa: empresa,
            formato: formato,
            hoja: hoja
        },
        success: function (evt) {

            $('#listpto').html(evt);
            oTable = $('#tablaPtos').dataTable({
                "bJQueryUI": true,
                "scrollY": 400,
                "scrollX": true,
                "sDom": 't',
                "ordering": true,
                "iDisplayLength": 200,
                "sPaginationType": "full_numbers"
            });
            oTable.rowReordering({ sURL: controlador + "/formatomedicion/UpdateOrder?formato=" + formato + "&hoja=" + hoja + "&empresa=" + empresa });
            oTableHead = $('#tablaHead').dataTable({
                "bJQueryUI": true,
                "scrollY": 400,
                "scrollX": true,
                "sDom": 't',
                "ordering": false,
                "iDisplayLength": 200,
                "sPaginationType": "full_numbers"
            });

        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function actualizarFormatoHoja()
{
    area = $('#cbArea').val();
    resolucion = $("#cbResolucion").val();
    horizonte = $("#cbHorizonte").val();
    periodo = $("#cbPeriodo").val();
    nombre = $('#txtFormato').val();
    lectura = $('#idLecturas').val();
    tituloHoja = $('#txtTitulo').val();
    formato = $('#hfId').val();
    diaplazo = $('#txtDiaPlazo').val();
    minPlazo = $('#txtMinPlazo').val();
    descripcion = $('#txtDescripcion').val();

    if (nombre == "") {
        alert("Ingresar Nombre del Formato");
        return;
    }
    if (area == 0) {
        alert("Seleccionar el area");
        return;
    }
    if (resolucion == 0) {
        alert("Seleccionar Resolución");
        return;
    }
    if (horizonte == 0) {
        alert("Seleccionar Horizonte");
        return;
    }
    if (periodo == 0) {
        alert("Seleccionar Periodo");
        return;
    }
    if (tituloHoja == "") {
        alert("Ingresar nombre del Titulo");
        return;
    }
    if (lectura == 0) {
        alert("Seleccionar Lectura");
        return;
    }

    $.ajax({

        type: 'POST',
        url: controlador + 'formatomedicion/ActualizarFormato',
        dataType: 'json',
        data: {
            idFormato: formato, idHoja: 1, nombre: nombre, area: area, resolucion: resolucion, horizonte: horizonte,
            periodo: periodo, lectura: lectura, tituloHoja: tituloHoja, diaPlazo: diaplazo, minPlazo: minPlazo, descripcion: descripcion
        },
        cache: false,
        success: function (resultado) {
            if (resultado == 1) {
                document.location.href = controlador + "formatomedicion/index";
            }
            else {
                alert("Error al grabar formato");
            }

        },
        error: function () {
            alert("Error al grabar formato");
        }

    });

}

function nuevoPto(empresa, formato, hoja) {
    $.ajax({
        type: 'POST',
        url: controlador + "formatomedicion/MostrarAgregarPto",
        data: {
            empresa: empresa, formato: formato,
            hoja: hoja
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
            listarequipo(empresa, -2);
            listarpto(-2);
        },
        error: function () {
            alert("Error al mostrar Ventana para agregar puntos");
        }
    });
}

function nuevoTitulo(empresa, formato, hoja) {
    $.ajax({
        type: 'POST',
        url: controlador + "formatomedicion/MostrarAgregarTitulo",
        data: {
            empresa: empresa, formato: formato,
            hoja: hoja, titulo: '', pos: 0
        },
        success: function (evt) {
            $('#agregarHead').html(evt);
            setTimeout(function () {
                $('#popuphead').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);

        },
        error: function () {
            alert("Error al mostrar Ventana para agregar puntos");
        }
    });
}

function agregarNuevoPto() {
    var punto = $('#idPunto').val();
    var medida = $('#idMedidas').val();
    var limsup = $('#idLimSup').val();
    var formato = $('#hfFormat').val();
    var hoja = $('#hfHoja').val();
    var empresa = $('#hfEmpresa').val();
    var signo = 1;
    if ($('#idSigno').attr('checked')) {
        signo = -1;
    }
    if (punto == -2) {
        alert("Seleccionar punto de medición");
    }
    else {
        if (medida == 0) {
            alert("Seleccionar medida")
        }
        else {

            $('#busquedaEquipo').bPopup().close();
            $.ajax({
                type: 'POST',
                url: controlador + 'formatomedicion/AgregarPto',
                dataType: 'json',
                data: { empresa: empresa, formato: formato, hoja: hoja, punto: punto, medida: medida, limsup: limsup, signo: signo },
                cache: false,
                success: function (res) {
                    switch (res.Resultado) {
                        case 1:
                            if (res.Resultado == 1) {
                                var idestado = "id" + res.HojaPto.Hojaptoorden;
                                var activo = "Activo";
                                if (res.HojaPto.Hojaptoactivo != 1) {
                                    activo = "Desactivo";
                                }

                                var newRow = '<tr id="' + res.HojaPto.Hojaptoorden + '"><td>' + res.HojaPto.Hojaptoorden + '</td><td>' + punto + "</td><td>" + res.HojaPto.Emprabrev + "</td><td>" + res.HojaPto.Equinomb + "</td>" +
                                    "<td>" + res.HojaPto.Tipoptomedinomb + " (" + res.HojaPto.Tipoinfoabrev +  ") </td><td>" + limsup + '</td><td>' + activo + '</td>' +
                                    '<td><a class="edit" href="">Edit</a></td><td><a class="delete" href="">Delete</a></td></tr>';
                                var tabla = $('#tablaPtos').DataTable();
                                tabla.row.add($(newRow)).draw();
                            }
                            break;
                        case -1:
                            alert("Error en BD ptos de medicion");
                            break;
                        case 0:
                            alert("Ya existe la información ingresada");
                            break;
                    }

                },
                error: function () {
                    alert("Error al grabar punto de medición");
                }
            });
        }
    }

}

function eliminarPto(idEmpresa,idFormato,idHoja,idOrden,idPto)
{
    $.ajax({
        type: 'POST',
        url: controlador + "formatomedicion/EliminarPto",
        data: {
            idEmpresa: idEmpresa, idFormato: idFormato, idHoja: idHoja, idOrden: idOrden, idPunto: idPto
        },
        success: function (evt) {
            mostrarListaPto(idEmpresa, idFormato, idHoja);

        },
        error: function () {
            alert("Error al mostrar Ventana para agregar puntos");
        }
    });
}

function agregarTitulo() {
    var formato = $('#hfFormat').val();
    var hoja = $('#hfHoja').val();
    var empresa = $('#hfEmpresa').val();
    var ancho = $('#idAncho').val();
    var titulo = $('#titulo').val();
    var pos = $('#hfPos').val();
    var idRowEditHead = pos + "#" + empresa + "#" + formato + "#" + hoja + "#" + titulo ;

    var jsevento = "<script type='text/javascript'> $(document).ready(function () { $('a.edithead').on('click', function (e) {";
    jsevento += "e.preventDefault();";
    jsevento += "var argumento = $(this).attr('id');";
    jsevento += 'var param = argumento.split("#");';
    jsevento += "modificarTitulo(param[1], param[2], param[3], param[4], param[0]);";
    jsevento += "}); }); </script>";

    $('#popuphead').bPopup().close();
    if (titulo != "") {
        var len = titulo.length;
        var titulocorto = titulo;
        if (len > 10) {
            titulocorto = titulocorto.substring(0, len - 1) + "...";
        }
        $.ajax({
            type: 'POST',
            url: controlador + 'formatomedicion/AgregarTitulo',
            dataType: 'json',
            data: { empresa: empresa, formato: formato, hoja: hoja, pos: pos, titulo: titulo, ancho: ancho },
            cache: false,
            success: function (res) {
                switch (res.Resultado) {
                    case -1:
                        alert("Error Modificando Titulo");
                        break;
                    case 0:
                        $("#rwHead" + pos).height(ancho * altoFila);
                        $("#rwHead" + pos)[0].innerHTML = "<td>" + titulocorto + "</td><td><a id="+ idRowEditHead + " href='#' class='edithead' >[E]</a></td>";
                        break;
                    case 1:
                        var newRow = "<tr style='height:" + parseInt(ancho) * altoFila + "px;'><td>" + titulocorto + "</td><td><a href='#' id=" + idRowEditHead + " class='edithead'>[E]</a></td></tr>";
                        var tabla = $('#tablaHead').DataTable();
                        tabla.row.add($(newRow)).draw();
                        break;
                }
            },
            error: function () {
                alert("Error al grabar punto de medición");
            }
        });
    }
    else {
        alert("Ingresar el titulo");
    }
}

function eliminarTitulo(empresa, formato, hoja) {
    $.ajax({
        type: 'POST',
        url: controlador + 'formatomedicion/EliminarTitulo',
        dataType: 'json',
        data: { empresa: empresa, formato: formato, hoja: hoja},
        cache: false,
        success: function (res) {
            if (res > 0) {
                var nRow = $('#tablaHead tr:last');
                oTableHead.fnDeleteRow(nRow);
            }
        },
        error: function () {
            alert("Error al grabar punto de medición");
        }
    });
}

function modificarTitulo(empresa, formato, hoja, titulo, pos) {
    //var hgt = $("#rwHead" + indice).height();
    //$("#rwHead" + indice).height(32);
    //$("#rwHead" + indice)[0].innerHTML = '<td>Central</td><td><a href="#" onclick="modificarTitulo(1)">[E]</a></td><td><a href="#">[X]</a></td>';

    $.ajax({
        type: 'POST',
        url: controlador + "formatomedicion/MostrarAgregarTitulo",
        data: {
            empresa: empresa, formato: formato,
            hoja: hoja, titulo: titulo, pos: pos
        },
        success: function (evt) {
            $('#agregarHead').html(evt);
            setTimeout(function () {
                $('#popuphead').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);

        },
        error: function () {
            alert("Error al mostrar Ventana para agregar título");
        }
    });

}

function listarequipo(empresa, familia) {

    $.ajax({
        type: 'POST',
        url: controlador + "formatomedicion/listarequipo",
        data: {
            empresa: empresa, familia: familia
        },
        success: function (evt) {
            $('#listaEquipo').html(evt);
        },
        error: function () {
            alert("Error al mostrar lista de equipos");
        }
    });

}

function listarpto(equipo) {
    $.ajax({
        type: 'POST',
        global:false,
        url: controlador + "formatomedicion/listarpto",
        data: {
            equipo: equipo
        },
        success: function (evt) {
            $('#listaPto').html(evt);
        },
        error: function () {
            alert("Error al mostrar lista de ptos");
        }
    });
}

