const controller = siteRoot + "DemandaPO/Feriados/";
const imageRoot = siteRoot + "Content/Images/";

var dt, popAgregar, popEditar;
var tempData;
$(document).ready(function () {

    $('#popFecha').Zebra_DatePicker();
    $('#popFechaEditar').Zebra_DatePicker();

    $('#txtFechaAnio').Zebra_DatePicker({
        view: 'years',
        format: 'Y',
        onSelect: function () {
            listarFeriados();
        }
    });

    dt = $('#dt').DataTable({
        data: [],
        columns: [
            { title: '', data: null, width: 45 },
            { title: '', data: null, width: 45 },
            { title: 'Fecha', data: 'Strfecha', width: 150},
            { title: 'Descripción', data: 'Dpoferdescripcion'},
            { title: 'SPL', data: 'Dpoferspl', width: 80},
            { title: 'SCO', data: 'Dpofersco', width: 80},
        ],
        columnDefs: [              
            {
                targets: 0,
                defaultContent: `<img src="${imageRoot}btn-cancel.png"`
                    + 'class="btnEliminar"'
                    + 'title="Eliminar registro"'
                    + '/>',
            },
            {
                targets: [1],
                defaultContent: `<img src="${imageRoot}btn-edit.png"`
                    + 'class="btnEditar"'
                    + 'title="Editar registro"'
                    + '/>',
            },
            {
                //Color de columna "SPL" segun criterio
                targets: [4],
                createdCell: function (td, cellData, rowData, row, col) {
                    if (rowData.Dpoferspl == 'S') {
                        $(td).html('Si');
                    }
                    else {
                        $(td).html('No');
                    }
                }
            },
            {
                //Color de columna "SCO" segun criterio
                targets: [5],
                createdCell: function (td, cellData, rowData, row, col) {
                    if (rowData.Dpofersco == 'S') {
                        $(td).html('Si');
                    }
                    else {
                        $(td).html('No');
                    }
                }
            },

        ],
        searching: false,
        bLengthChange: false,
        bSort: false,
        destroy: true,
        paging: false,
        info: false,
    });

    $('#btnAgregar').on('click', function () {
        popAgregar = $('#popup').bPopup({
            easing: 'easeOutBack',
            speed: 350,
            transition: 'fadeIn',
            modalClose: false,
            onOpen: function () {
                document.getElementById("popFecha").value = "";
                document.getElementById("popDescripcion").value = "";
                document.getElementById("popSPL").checked = false;
                document.getElementById("popSCO").checked = false;
            }
        });

    });

    $('#popAgregar').on('click', function () {
        agregarFeriados();

    });


    $('#popCancelar').click(function () {
        $('#popup').bPopup().close();
    });



    $('#popCancelarEditar').click(function () {
        $('#popupEditar').bPopup().close();
    });

    $('#popAceptarEditar').on('click', function () {
        editarFeriados(tempData.Dpofercodi);
    });

    listarFeriados();
});

$(document).on('click', '#dt tr td .btnEliminar', function () {
    var row = $(this).closest('tr');
    var r = dt.row(row).data();
    eliminarFeriados(r.Dpofercodi);

});

$(document).on('click', '#dt tr td .btnEditar', function () {
    var row = $(this).closest('tr');
    var r = dt.row(row).data();
    tempData = r
    popEditar = $('#popupEditar').bPopup({
        easing: 'easeOutBack',
        speed: 350,
        transition: 'fadeIn',
        modalClose: false,
        onOpen: function () {
            //document.getElementById("popFechaEditar").disabled = true;
            document.getElementById("popFechaEditar").value = r.Strfecha;
            document.getElementById("popDescripcionEditar").value = r.Dpoferdescripcion;
            document.getElementById("popSPLEditar").checked = (r.Dpoferspl == "S") ? true: false;
            document.getElementById("popSCOEditar").checked = (r.Dpofersco == "S") ? true : false;
        }
    });


    
});



function listarFeriados() {
    $.ajax({
        type: 'POST',
        url: controller + 'ListaFeriados',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            fechaAnio: $('#txtFechaAnio').val(),
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            if (result.Resultado === "-1") {
                alert("Ocurrio un error inesperado");
            } else {
                dt.clear();
                dt.rows.add(result.ListaFeriados);
                dt.draw();
            }
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function agregarFeriados() {
    if (document.getElementById("popSPL").checked == true) {
        spl = "S";
    }
    else {
        spl = "N";
    }
    if (document.getElementById("popSCO").checked == true) {
        sco = "S";
    }
    else {
        sco = "N";
    }
    $.ajax({
        type: 'POST',
        url: controller + 'AgregarFeriados',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            fechaAnio: $('#txtFechaAnio').val(),
            fecha: $('#popFecha').val(),
            descripcion: $('#popDescripcion').val(),
            spl: spl,
            sco: sco,
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            if (result.Resultado === "-1") {
                alert(result.Mensaje);
            }
            else if (result.Resultado === "-2") {
                alert(result.Mensaje);
            }
            else {
                dt.clear();
                dt.rows.add(result.ListaFeriados);
                dt.draw();
                alert(result.Mensaje);
                $("#popup").bPopup().close();
            }
            
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}


function eliminarFeriados(dpofercodi) {
    $.ajax({
        type: 'POST',
        url: controller + 'EliminarFeriados',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            dpofercodi: dpofercodi,
            fechaAnio: $('#txtFechaAnio').val(),
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            if (result.Resultado === "-1") {
                alert("Ocurrio un error inesperado");
            } else {
                dt.clear();
                dt.rows.add(result.ListaFeriados);
                dt.draw();
            }
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function editarFeriados(dpofercodi) {
    if (document.getElementById("popSPLEditar").checked == true) {
        spl = "S";
    }
    else {
        spl = "N";
    }
    if (document.getElementById("popSCOEditar").checked == true) {
        sco = "S";
    }
    else {
        sco = "N";
    }

    $.ajax({
        type: 'POST',
        url: controller + 'EditarFeriados',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            dpofercodi: dpofercodi,
            descripcion: $('#popDescripcionEditar').val(),
            spl: spl,
            sco: sco,
            fecha: $('#popFechaEditar').val(),
            fechaAnio: $('#txtFechaAnio').val(),
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            if (result.Resultado === "-1") {
                alert("Ocurrio un error inesperado");
            }
            else if (result.Resultado === "-2") {
                alert(result.Mensaje);
            }
            else {
                alert(result.Mensaje);
                dt.clear();
                dt.rows.add(result.ListaFeriados);
                dt.draw();
                $("#popupEditar").bPopup().close();
            }
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}