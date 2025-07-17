var controlador = siteRoot + 'formulas/configuracion/'

$(function () {
    //#Assetec.PRODEM.E3 - 20211125# Inicio
    $('#cbPuntoSco').multipleSelect({
        filter: true,
        single: true,
        placeholder: 'Seleccione'
    });

    $('#cbPuntoIeod').multipleSelect({
        filter: true,
        single: true,
        placeholder: 'Seleccione'
    });

    $('#cbPuntoDpo').multipleSelect({
        filter: true,
        single: true,
        placeholder: 'Seleccione'
    });

    $('.filtroIeod').hide();
    $('.filtroSco').hide();
    //#Assetec.PRODEM.E3 - 20211125# Fin

    ////#Assetec.Demanda DPO - 20230515# Inicio
    //$('#cbPuntoSco').multipleSelect({
    //    filter: true,
    //    single: true,
    //    placeholder: 'Seleccione'
    //});
    ////#Assetec.Demanda DPO - 20230515# Inicio

    $('#btnGrabar').click(function () {
        grabar();
    });

    $('#btnNuevo').click(function () {
        document.location.href = controlador + "nuevo";
    });

    $('#btnAddEdit').click(function () {
        cargarFuenteEdicion();
    });

    $('#btnCancelar').click(function () {
        cancelar();
    });

    $('#cbOrigen').change(function () {
        //#Assetec.PRODEM.E3 - 20211125# Inicio
        const id = $(this).val();
        switch (id) {
            case 'X': {
                $('.filtrosBase').hide();
                $('.filtroIeod').hide();
                $('.filtroDpo').hide();
                $('.filtroSirpit').hide();
                $('.filtroSicli').hide();
                $('.filtroSco').show();
                break;
            }
            case 'Y': {
                $('.filtrosBase').hide();
                $('.filtroSco').hide();
                $('.filtroDpo').hide();
                $('.filtroSirpit').hide();
                $('.filtroSicli').hide();
                $('.filtroIeod').show();
                break;
            }
            //#Assetec.DEMANDADPO - 20230419# Inicio
            case 'F': {
                $('.filtrosBase').hide();
                $('.filtroSco').hide();
                $('.filtroSirpit').hide();
                $('.filtroSicli').hide();
                $('.filtroDpo').show();
                break;
            }
            //#Assetec.DEMANDADPO - 20230419# Inicio
            //#Assetec.DEMANDADPO - 20230419# Inicio
            case 'I': {
                $('.filtrosBase').hide();
                $('.filtroSco').hide();
                $('.filtroDpo').hide();
                $('.filtroSicli').hide();
                $('.filtroSirpit').show();
                cargarSubestacionSirpit();
                break;
            }
            //#Assetec.DEMANDADPO - 20230419# Inicio
            //#Assetec.DEMANDADPO - 20230515# Inicio
            case 'G': {
                $('.filtrosBase').hide();
                $('.filtroSco').hide();
                $('.filtroDpo').hide();
                $('.filtroSirpit').hide();
                $('.filtroSicli').show();
                cargarEmpresaSicli();
                break;
            }
            //#Assetec.DEMANDADPO - 20230515# Inicio
            default: {
                $('.filtroIeod').hide();
                $('.filtroSco').hide();
                $('.filtroDpo').hide();
                $('.filtroSirpit').hide();
                $('.filtroSicli').hide();
                $('.filtrosBase').show();
                cargarEmpresas();
            }
        }
        //#Assetec.PRODEM.E3 - 20211125# Fin
    });

    $('#cbEmpresa').change(function () {
        cargarSubEstacion();
    });

    $('#scEmpresa').change(function () {
        //cargarEquipoSicli();
        cargarPuntoSicli();
    });

    $('#scEquipo').change(function () {
        cargarPuntoSicli();
    });

    $('#spSubestacion').change(function () {
        cargarTransformador();
    });

    $('#spTransformador').change(function () {
        cargarBarra();
    });

    $('#cbSubEstacion').change(function () {
        cargarPuntos();
    });

    $('#btnAgregar').click(function () {
        addPunto();
    });

    //#Assetec.PRODEM.E3 - 20211125# Inicio
    $('#btnAgregarSco').click(function () {
        const punto = $('#cbPuntoSco').val();
        if (!punto) mostrarMensaje('Seleccione un punto');
        const descripcion = $('#cbPuntoSco option:selected').text();
        addPuntoTna("X", "Estimador TNA (SCO)", punto, descripcion);
    });
    $('#btnAgregarIeod').click(function () {
        const punto = $('#cbPuntoIeod').val();
        if (!punto) mostrarMensaje('Seleccione un punto');
        const descripcion = $('#cbPuntoIeod option:selected').text();
        addPuntoTna("Y", "Estimador TNA (IEOD)", punto, descripcion);
    });
    //#Assetec.PRODEM.E3 - 20211125# Fin

    //#Assetec.DEMANDADPO - 20230419# Inicio
    $('#btnAgregarDpo').click(function () {
        const punto = $('#cbPuntoDpo').val();
        if (!punto) mostrarMensaje('Seleccione un punto');
        const descripcion = $('#cbPuntoDpo option:selected').text();
        addPuntoTna("F", "Informacion TNA(DPO)", punto, descripcion);
    });
    //#Assetec.DEMANDADPO - 20230419# Fin

    //#Assetec.DEMANDADPO - 20230502# Inicio
    $('#btnAgregarSirpit').click(function () {
        const trafoBarra = $('#spBarra').val();
        if (!trafoBarra) mostrarMensaje('Seleccione una Barra');
        const descripcionBarra = $('#spBarra option:selected').text();
        const descripcionTrafo = $('#spTransformador option:selected').text();
        addPuntoTna("I", "Informacion SIRPIT", trafoBarra, descripcionTrafo + " - " + descripcionBarra);
    });
    //#Assetec.DEMANDADPO - 20230502# Fin

    //#Assetec.DEMANDADPO - 20230502# Inicio
    $('#btnAgregarSicli').click(function () {
        const punto = $('#scPunto').val();
        if (!punto) mostrarMensaje('Seleccione un punto');
        const descripcion = $('#scPunto option:selected').text();
        addPuntoTna("G", "Informacion SICLI", punto, descripcion);
    });
    //#Assetec.DEMANDADPO - 20230502# Fin

    $('#cbAreaOperativa').val($('#hfAreaOperativa').val());

});

//#Assetec.DEMANDADPO - 20230502# Inicio
cargarSubestacionSirpit = function () {

    $('option', '#spSubestacion').remove();
    $('option', '#spTransformador').remove();
    $('option', '#spBarra').remove();

    if ($('#cbOrigen').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'cargarSubestacionSirpit',
            dataType: 'json',
            cache: false,
            success: function (result) {
                $('#spSubestacion').get(0).options.length = 0;
                //RefillDropDowList($('#spSubestacion'), result, 'Dposubcodi', 'Dposubnombre');
                $('#spSubestacion').get(0).options[0] = new Option("--SELECCIONE--", "");
                $.each(result, function (i, item) {
                    $('#spSubestacion').get(0).options[$('#spSubestacion').get(0).options.length] = new Option(item.Dposubnombre, item.Dposubcodi);
                });
            },
            error: function () {
                mostrarError();
            }
        });
    }
    else {
        $('option', '#spSubestacion').remove();
    }
}

cargarTransformador = function () {

    $('option', '#spBarra').remove();

    if ($('#spSubestacion').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'cargarTransformadorSirpit',
            dataType: 'json',
            data: { idSubestacion: $('#spSubestacion').val() },
            cache: false,
            success: function (result) {
                $('#spTransformador').get(0).options.length = 0;
                $('#spTransformador').get(0).options[0] = new Option("--SELECCIONE--", "");
                $.each(result, function (i, item) {
                    $('#spTransformador').get(0).options[$('#spTransformador').get(0).options.length] = new Option(item.Dpotnfcodiexcel, item.Dpotnfcodiexcel);
                });
            },
            error: function () {
                mostrarError();
            }
        });
    }
    else {
        $('option', '#spTransformador').remove();
    }
}

cargarBarra = function () {

    if ($('#spTransformador').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'cargarBarraSirpit',
            dataType: 'json',
            data: { idTransformador: $('#spTransformador').val() },
            cache: false,
            success: function (result) {
                $('#spBarra').get(0).options.length = 0;
                $('#spBarra').get(0).options[0] = new Option("--SELECCIONE--", "");
                $.each(result, function (i, item) {
                    $('#spBarra').get(0).options[$('#spBarra')
                        .get(0).options.length] = new Option(item.Tnfbarbarnombre + " - " +
                                                             item.Tnfbarbartension,
                                                             item.Tnfbarcodi);
                });
            },
            error: function () {
                mostrarError();
            }
        });
    }
    else {
        $('option', '#spBarra').remove();
    }
}
//#Assetec.DEMANDADPO - 20230502# Fin

//#Assetec.DEMANDADPO - 20230515# Inicio
cargarEmpresaSicli = function () {

    $('option', '#scEquipo').remove();
    $('option', '#scPunto').remove();

    if ($('#cbOrigen').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'cargarEmpresaSicli',
            dataType: 'json',
            cache: false,
            success: function (result) {
                $('#scEmpresa').get(0).options.length = 0;
                $('#scEmpresa').get(0).options[0] = new Option("--SELECCIONE--", "");
                $.each(result, function (i, item) {
                    $('#scEmpresa').get(0).options[$('#scEmpresa').get(0).options.length] = new Option(item.Emprnomb, item.Emprcodi);
                });
            },
            error: function () {
                mostrarError();
            }
        });
    }
    else {
        $('option', '#scEmpresa').remove();
        $('option', '#scEquipo').remove();
        $('option', '#scPunto').remove();
    }
}

cargarEquipoSicli = function () {
    $('option', '#scPunto').remove();
    if ($('#scEmpresa').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'cargarEquipoSicli',
            dataType: 'json',
            data: { idEmpresa : $('#scEmpresa').val() },
            cache: false,
            success: function (result) {
                $('#scEquipo').get(0).options.length = 0;
                $('#scEquipo').get(0).options[0] = new Option("--SELECCIONE--", "");
                $.each(result, function (i, item) {
                    $('#scEquipo').get(0).options[$('#scEquipo').get(0).options.length] = new Option(item.Equinomb, item.Equicodi);
                });
            },
            error: function () {
                mostrarError();
            }
        });
    }
    else {
        $('option', '#scEquipo').remove();
        $('option', '#scPunto').remove();
    }
}

cargarPuntoSicli = function () {
    if ($('#scEmpresa').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'cargarPuntoSicli',
            dataType: 'json',
            data: { idEmpresa: $('#scEmpresa').val() },
            cache: false,
            success: function (result) {
                $('#scPunto').get(0).options.length = 0;
                $('#scPunto').get(0).options[0] = new Option("--SELECCIONE--", "");
                $.each(result, function (i, item) {
                    $('#scPunto').get(0).options[$('#scPunto').get(0).options.length] = new Option(item.Ptomedidesc, item.Ptomedicodi);
                });
            },
            error: function () {
                mostrarError();
            }
        });
    }
    else {
        $('option', '#scPunto').remove();
    }
}
//#Assetec.DEMANDADPO - 20230515# Fin

cargarEmpresas = function () {

    $('option', '#cbEmpresa').remove();
    $('option', '#cbPunto').remove();
    $('option', '#cbSubEstacion').remove();

    if ($('#cbOrigen').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'cargarempresas',
            dataType: 'json',
            data: {
                indicador: $('#cbOrigen').val()
            },
            cache: false,
            success: function (aData) {
                $('#cbEmpresa').get(0).options.length = 0;
                $('#cbEmpresa').get(0).options[0] = new Option("--SELECCIONE--", "");
                $.each(aData, function (i, item) {
                    $('#cbEmpresa').get(0).options[$('#cbEmpresa').get(0).options.length] = new Option(item.Text, item.Value);
                });
            },
            error: function () {
                mostrarError();
            }
        });
    }
    else {
        $('option', '#cbEmpresa').remove();
    }
}

addPunto = function () {
    var punto = $('#cbPunto').val();
    if (punto != "") {
        var descripcion = $('#cbPunto option:selected').text();
        var origen = $('#cbOrigen').val();
        var textoOrigen = "";
        if (origen == "A") textoOrigen = "SCADA";
        if (origen == "B") textoOrigen = "Despacho ejecutado diario";
        if (origen == "C") textoOrigen = "Histórico (Demanda barra - diario)";
        if (origen == "D") textoOrigen = "Medidores (Demanda en barras)";
        if (origen == "M") textoOrigen = "Medidores de Generación";
        if (origen == "E") textoOrigen = "PR5";
        if (origen == "S") textoOrigen = "SCADA SP7";
        if (origen == "U") textoOrigen = "Demanda UL y D (PR-16)";
        //Assetec.PRODEM3 - 20220401
        if (origen == "Z") textoOrigen = "Servicios Auxiliares";
        //Assetec.PRODEM3 - 20220401
        var longitud = $('#tbItems> tbody tr').length + 1;

        $('#tbItems> tbody').append(
            '<tr>' +
            '    <td>' +
            '        <input type="hidden" id="thfOrigen" value="' + origen + '"/>' +
            '        <input type="text" style="width:50px" value="1" onkeydown="mover(event, this)" onkeypress="return validarNumero(this,event)" id="constante' + longitud + '" />' +
            '   </td>' +
            '   <td>' + textoOrigen + '</td>' +
            '   <td>' +
            '        <span>' + descripcion + '</span>' +
            '        <input type="hidden" id="thfPunto" value="' + punto + '" />' +
            '   </td>' +
            '   <td style="text-align:center">' +
            '       <img src="' + siteRoot + 'Content/Images/btn-cancel.png" alt="" onclick="$(this).parent().parent().remove();" style="cursor:pointer" />' +
            '   </td>' +
            '</tr>'
        );
    }
    else {
        mostrarMensaje('Seleccione punto.');
    }
};

//#Assetec.PRODEM.E3 - 20211125# Inicio
addPuntoTna = function (origen, textoOrigen, punto, descripcion) {
    const longitud = $('#tbItems> tbody tr').length + 1;
    $('#tbItems> tbody').append(
        '<tr>' +
        '    <td>' +
        '        <input type="hidden" id="thfOrigen" value="' + origen + '"/>' +
        '        <input type="text" style="width:50px" value="1" onkeydown="mover(event, this)" onkeypress="return validarNumero(this,event)" id="constante' + longitud + '" />' +
        '   </td>' +
        '   <td>' + textoOrigen + '</td>' +
        '   <td>' +
        '        <span>' + descripcion + '</span>' +
        '        <input type="hidden" id="thfPunto" value="' + punto + '" />' +
        '   </td>' +
        '   <td style="text-align:center">' +
        '       <img src="' + siteRoot + 'Content/Images/btn-cancel.png" alt="" onclick="$(this).parent().parent().remove();" style="cursor:pointer" />' +
        '   </td>' +
        '</tr>'
    );
};
//#Assetec.PRODEM.E3 - 20211125# Fin

grabar = function () {
    var items = "";
    var count = 0;
    var countArea = 0;

    $('#tbItems>tbody tr').each(function (i) {
        $text = $(this).find('input:text');
        $origen = $(this).find('#thfOrigen');
        $punto = $(this).find('#thfPunto');
        var constante = (count > 0) ? "+" : "";
        items = items + constante + $text.val() + $origen.val() + $punto.val();
        count++;
    });

    var roles = "";
    $('#tablaArea input:checked').each(function () {
        roles = roles + $(this).val() + ",";
        countArea++;
    });

    if (count > 0) {
        if (countArea > 0) {
            var subEstacion = $('#txtSubestacion').val();
            var areaOperativa = $('#cbAreaOperativa').val();

            if (subEstacion != "" && areaOperativa != "") {
                $.ajax({
                    type: 'POST',
                    url: controlador + 'grabar',
                    data: {
                        idFormula: $('#hfIdFormula').val(),
                        subEstacion: subEstacion,
                        areaOperativa: areaOperativa,
                        roles: roles,
                        items: items
                    },
                    dataType: 'json',
                    cache: false,
                    success: function (resultado) {
                        if (resultado > 0) {
                            $('#hfIdFormula').val(resultado);
                            $('#mensaje').removeClass();
                            $('#mensaje').addClass('action-exito');
                            $('#mensaje').text("La fórmula fué grabada correctamente.");
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
                $('#mensaje').addClass('action-alert');
                $('#mensaje').text("Ingrese la subestación y el área.");
            }
        }
        else {
            $('#mensaje').removeClass();
            $('#mensaje').addClass('action-alert');
            $('#mensaje').text("Seleccione la(s) área(s).");
        }
    }
    else {
        $('#mensaje').removeClass();
        $('#mensaje').addClass('action-alert');
        $('#mensaje').text("Agregue items a la fórmula.");
    }
}

cargarSubEstacion = function () {
    if ($('#cbEmpresa').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'cargarsubestacion',
            dataType: 'json',
            data: { idEmpresa: $('#cbEmpresa').val() },
            cache: false,
            success: function (aData) {
                $('#cbSubEstacion').get(0).options.length = 0;
                $('#cbSubEstacion').get(0).options[0] = new Option("--SELECCIONE--", "");
                $.each(aData, function (i, item) {
                    $('#cbSubEstacion').get(0).options[$('#cbSubEstacion').get(0).options.length] = new Option(item.Text, item.Value);
                });
            },
            error: function () {
                mostrarError();
            }
        });
    }
    else {
        $('option', '#cbPunto').remove();
        $('option', '#cbSubEstacion').remove();
    }
}

cargarPuntos = function () {

    if ($('#cbEmpresa').val() != "" && $('#cbSubEstacion').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'cargarpuntos',
            dataType: 'json',
            data: { idEmpresa: $('#cbEmpresa').val(), idSubEstacion: $('#cbSubEstacion').val() },
            cache: false,
            success: function (aData) {
                $('#cbPunto').get(0).options.length = 0;
                $('#cbPunto').get(0).options[0] = new Option("--SELECCIONE--", "");
                $.each(aData, function (i, item) {
                    $('#cbPunto').get(0).options[$('#cbPunto').get(0).options.length] = new Option(item.Text, item.Value);
                });
            },
            error: function () {
                mostrarError();
            }
        });
    }
    else {
        $('option', '#cbPunto').remove();
    }
}

cancelar = function () {
    document.location.href = controlador + "index";
}

mover = function (e, control) {
    var str = control.id;
    var indice = str.replace('constante', '');

    if (e.which == '13') {
        var id = '';
        if (indice != '48') {
            id = '#constante' + (parseInt(indice) + 1);
        }
    }
    if (e.which == '40') {
        if (indice != '48') {
            $('#constante' + (parseInt(indice) + 1)).focus();

        }
    }
    if (e.which == '38') {
        if (indice != '1') {
            $('#constante' + (parseInt(indice) - 1)).focus();
        }
    }
}

mostrarError = function () {
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-error');
    $('#mensaje').text("Ha ocurrido un error.");

}

mostrarMensaje = function (mensaje) {
    alert(mensaje);
}

validarNumero = function (item, evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;

    if (charCode == 46) {
        var regex = new RegExp(/\./g)
        var count = $(item).val().match(regex).length;
        if (count > 1) {
            return false;
        }
    }

    if (charCode == 45) {
        var regex = new RegExp(/\-/g)
        var count = $(item).val().match(regex).length;
        if (count > 0) {
            return false;
        }
    }

    if (charCode > 31 && charCode != 45 && (charCode < 48 || charCode > 57)) {
        return false;
    }

    return true;
}