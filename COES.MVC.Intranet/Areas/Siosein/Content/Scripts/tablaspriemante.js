var controlador = siteRoot + 'Siosein/TablasPrie/'
var ListaCamposTabla = [];
$(function () {        
    $('#cbArea').val(codi);

    // #region mante

    codi = parseInt($('#hfAreaCodi').val());
    $('#cbArea').val(codi);

    $('#txtPlazoEntrega').Zebra_DatePicker({
        //direction: -1
    });


    // #endregion
});

function mostrarCampos() {
    if (validarAgregarNuevoCampo()) {
        $('#campos').show();
        limpiarPropiedadesCampo();
    }
}

function validarAgregarNuevoCampo() {
    
    descripcionTabla = $('#TxtDescripcion').val();
    AreaInvolucrada = $('#cbArea').val();
    plazoentrega = $('#txtPlazoEntrega').val();

    if (descripcionTabla != "") {
        if (AreaInvolucrada != "") {
            if (plazoentrega != "") {             
                //$('#campos').show();
            }
            else {
                alert("Debe ingresar plazo de entrega..!");
                $('#txtPlazoEntrega').focus();
                return false;
            }
        }
        else {
            alert("debe seleccionar area involucrada..!");
            $('#cbArea').focus();
            return false;
        }
    }
    else {
        alert("debe ingresar una descripcion..!");
        $('#TxtDescripcion').focus();
        return false;
    }
    return true;
}

//funcion que inicializa la lista de los campos de una tabla
function cargarListaCampos() {
    strCampos = $('#hfListaCampos').val();
    arrCampos = strCampos.split('#');
    arrCampos.splice(arrCampos.length - 1, 1);


    ///codigo para interpretar la cadena de texto e inicializar la lista
    for (i = 0; i < arrCampos.length; i++) {
        propCampo = arrCampos[i].split(',');
        var entityCampo =
        {
            Cpriecodi: propCampo[0],
            Cprienombre: propCampo[1],
            Cpriedescripcion: propCampo[2],
            Cprietipo: propCampo[3],
            Cprielong1: propCampo[4],
            Cprielong2: propCampo[5],           
            opCrud: 0 // -1: eliminar, 0:lectura, 1:crear, 2:actualizar 

        };
        ListaCamposTabla.push(entityCampo);
    }
    dibujarTablaCampos();
}

function insertarCampo() {    
    nombreCampo = $('#txtNombrecampo').val();
    descripcionCampo = $('#TxtDescripcionCampo').val();
    tipoDato = $('#cbTipoDato').val();
    longitud1 = $('#longitud1').val();
    longitud2 = $('#longitud2').val();

    if (nombreCampo != "") {
        if (descripcionCampo != "") {
            if (tipoDato != "") {

                autogeneId = (-1) * (ListaCamposTabla.length + 1); 
                entityCampo =
                 {
                     Cpriecodi: autogeneId,
                     Cprienombre: nombreCampo,
                     Cpriedescripcion: descripcionCampo,
                     Cprietipo: tipoDato,
                     Cprielong1: longitud1,
                     Cprielong2: longitud2,
                     opCrud: 1 // -1: eliminar, 0:lectura, 1:crear, 2:actualizar 

                 };
                ListaCamposTabla.push(entityCampo);


                dibujarTablaCampos();
                //$('#btnGrabar').show();
                $('#campos').hide();

            }
            else {
                alert("Debe ingresar nombre del campo..!");
                $('#txtNombrecampo').focus();
            }
        }
        else {
            alert("debe ingresar descripcción del campo..!");
            $('#TxtDescripcionCampo').focus();
        }
    }
    else {
        alert("debe seleccionar tipo de dato..!");
        $('#cbTipoDato').focus();
    }
}

function dibujarTablaCampos() {
    tipoArea = 0;
    nombreArea = "";
    strHtml = "";
    strHtml += "<table border='0' class='pretty tabla-icono' cellspacing='0' width='100%' id='tablacampos'>";
    strHtml += "<thead><tr><th>Item</th><th>Campo</th><th>Descripción</th><th>Tipo</th><th>entero</th><th>Decimal</th><th></th></tr></thead><tbody>";
   
    if (ListaCamposTabla.length > 0) {
        for (i = 0; i < ListaCamposTabla.length; i++) {
            if (ListaCamposTabla[i].opCrud != -1) { // imprimimos los equipos nuevos y los que no han sido eliminados que vienen de BD              
                strHtml += "<tr>";                
                strHtml += "<td>" + ListaCamposTabla[i].Cpriecodi + "</td>";
                strHtml += "<td>" + ListaCamposTabla[i].Cprienombre + "</td>";
                strHtml += "<td>" + ListaCamposTabla[i].Cpriedescripcion + "</td>";
                tipoArear = parseInt(ListaCamposTabla[i].Cprietipo);
                switch (tipoArear) {
                    case 1:
                        nombreArea = "Carater";
                        break;
                    case 2:
                        nombreArea = "Numerico";
                        break;
                    case 3:
                        nombreArea = "Fechahora";
                        break;
                }

                strHtml += "<td>" + nombreArea + "</td>";
                strHtml += "<td>" + ListaCamposTabla[i].Cprielong1 + "</td>";
                strHtml += "<td>" + ListaCamposTabla[i].Cprielong2 + "</td>";
                
                strHtml += "<td><a href='#'><img onclick='eliminaCampo(" + ListaCamposTabla[i].Cpriecodi + ");' src='../Content/Images/ico-delete.gif' title='Eliminar Campo' alt='eliminar'/></a>";
                strHtml += "</td></tr>";                
            }
        }
    }
    else {
        strHtml += "<tr><td colspan='5'>No hay elementos...</td></tr>";
    }
    strHtml += "</tbody></table>";
    $('#listadocampos').css("width", $('#mainLayout').width() / 2 + "px");
    $('#listadocampos').html(strHtml);
}

function grabarTabla() {
    if (!validarAgregarNuevoCampo()) return;
    listaCampos = ListaCamposTabla;
    descripcionTabla = $('#TxtDescripcion').val();
    AreaInvolucrada = $('#cbArea').val();
    plazoentrega = $('#txtPlazoEntrega').val();
    abreviatura = $('#txtAbreviatura').val();



    $.ajax({
        type: 'POST',
        //async: false,
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        traditional: true,
        url: controlador + "grabar",
        data: JSON.stringify({
            data: listaCampos,
            descripcionTabla: descripcionTabla,
            AreaInvolucrada: AreaInvolucrada,
            plazoentrega: plazoentrega,
            idTabla: $('#hfTpriecodi').val(),
            abreviatura: abreviatura
        }),
        beforeSend: function () {
            //mostrarExito("Enviando Información ..");
        },
        success: function (evt) {
            if (evt == 1) {
                //mostrarExito("Los datos se enviaron correctamente");
                cancelar();
                mostrarListadoTablasPrie();
            }
            else {
                alert("Error al Grabar");
            }
        },
        error: function () {
            alert("Error");
        }

    });
}

function cancelar() {
    $('#popupTabla').bPopup().close();
}

function eliminaCampo(cpriecodi){

    if (ListaCamposTabla.length > 0) {
        if (confirm("¿confirma que desea eliminar campo..?")) {
            for (var indice in ListaCamposTabla) {
                if (ListaCamposTabla[indice].Cpriecodi == cpriecodi) {
                    if (ListaCamposTabla[indice].Cpriecodi > 0) { //si viene de base de datos
                        ListaCamposTabla[indice].opCrud = -1; // eliminado lógico 
                    }
                    else {
                        ListaCamposTabla.splice(indice, 1); //eliminado fisico de la matriz auxiliar
                    }
                }


            }
            dibujarTablaCampos();
        }
    }
}

function limpiarPropiedadesCampo() {
    $('#txtNombrecampo').val("");
    $('#TxtDescripcionCampo').val("");
    $('#cbTipoDato').val(1);
    $('#longitud1').val("");
    $('#longitud2').val("");
}