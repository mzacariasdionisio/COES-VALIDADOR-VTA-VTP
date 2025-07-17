var controler = siteRoot + "transferencias/barra/";

//Funciones de busqueda
$(document).ready(function () {
    //buscar();
    quitarRelacion();
    $('#BARRCODISUM').multipleSelect({
        width: '200px',
        filter: true,
        selectAll: false,
        single: true,
        placeholder: '--Seleccione--'
    });

    $('#btnAgregar').click(function () {

        var idsum = $('select[id="BARRCODISUM"]').val();
        var nombreBarraTransferencia = $('#hfBARRNOMBTRA').val();
        var t = document.getElementById("BARRCODISUM");
        var nombreBarraSum = t.options[t.selectedIndex].text;

        if (idsum == null) {
            alert("Seleccione la barra de suministro");
            return;
        }
        if (confirm("¿Está seguro agregar la barra de suministro?")) {

            $.ajax({
                type: 'POST',
                url: controler + "RegistrarRelacion",
                data: { barraSum: idsum, barraTrans: nombreBarraTransferencia, barraSumText: nombreBarraSum },
                success: function (evt) {
                    if (evt.EsCorrecto == 1) {
                        listarSuministros(evt.Data);
                        quitarRelacion();
                    } else {
                        alert(evt.Mensaje)
                    }
                },
                error: function () {
                    alert("Lo sentimos, se ha producido un error");
                }
            });

        }

    });


});

function listarSuministros(barrtra) {
    $.ajax({
        type: 'POST',
        url: controler + "ListaSuministro",
        data: { barrtTra: barrtra },
        success: function (evt) {
            $('#listaSuministro').html(evt);
        },
        error: function () {
            alert("Lo sentimos, se ha producido un error");
        }
    });
};



//Funciones de eliminado de registro
function quitarRelacion() {
    $(".quitar").click(function (e) {
        e.preventDefault();
        id = $(this).attr("id").split("_")[1];
        if (id == null) {
            return;
        }
        if (confirm("¿Desea eliminar la información seleccionada?")) {

            $.ajax({
                type: "POST",
                url: controler + "EliminarRelacionBarra",
                data: { id: id },
                success: function (evt) {
                    if (evt.EsCorrecto == 1) {
                        listarSuministros(evt.Data);
                    } else {
                        alert(evt.Mensaje)
                    }
                }
            });
        }
    });
};

addAntiForgeryToken = function (data) {
    data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
    return data;
};


