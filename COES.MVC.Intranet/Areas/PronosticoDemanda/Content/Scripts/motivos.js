var rutaImagenes = siteRoot + "Content/Images/";
var popupid;

$(document).ready(function () {

    $('#btn-guardar').on('click', function () {
        var nombre = $('#id-subcausadesc').val();
        var abrev = $('#id-subcausaabrev').val()
        if (!nombre || !abrev) {
            SetMessage('#message', "Necesita ingresar una Descripcion y su Abreviatura", 'error');
        }
        else {
            MotivoSave();
        }
        //MotivoSave();
    });

    $('#btn-pop-guardar').on('click', function () {
        popupMotivoSave();
    });

    List();

});

function MotivoSave() {

    $.ajax({
        type: 'POST',
        url: controller + 'MotivoSave',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            nombre: $('#id-subcausadesc').val(),
            abrev: $('#id-subcausaabrev').val(),
        }),
        success: function (e) {
            SetMessage('#message', 'Registro exitoso...', 'success');
            List();
        }, 
        error: function () {
            SetMessage('#message', 'Ha ocurrido un problema...', 'error');
        }
    });
}

function popupMotivoSave() {

    $.ajax({
        type: 'POST',
        url: controller + 'MotivoUpdate',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            id: popupid,
            nombre: $('#id-subcausadescpop').val(),
            abrev: $('#id-subcausaabrevpop').val(),
        }),
        success: function (e) {
            SetMessage('#message', 'Registro exitoso...', 'success');
            $("#popupEditar").bPopup().close();
            List();
        },
        error: function () {
            SetMessage('#message', 'Ha ocurrido un problema...', 'error');
        }
    });
}

function List() {

    $('#dtgridMotivos').html("");

    $.ajax({
        type: 'POST',
        url: controller + 'MotivoList',
        contentType: 'application/json; charset=utf-8',
        datatype: 'json',
        success: function (result) {
            console.log(result);
            dt = $('#dtgridMotivos').DataTable({
                data: result,
                columns: [
                    { data: "Subcausacodi", title: "ID", visible: false },
                    { data:"Subcausadesc", title: "NOMBRE" },
                    { data: "Subcausaabrev", title: "ABREVIATURA" },
                    { data: null, title:''}
                ],
                initComplete: function () {
                    $('#dtgridMotivos').css('width', '100%');
                    $('.dataTables_scrollHeadInner').css('width', '100%');
                    $('.dataTables_scrollHeadInner table').css('width', '98.84%');
                },
                columnDefs:
                    [
                        
                        {
                            targets: [3], width: 70,
                            defaultContent: '<img class="clsEditar" src="' + rutaImagenes + 'btn-edit.png" style="cursor: pointer;" title="Editar"/> '
                                
                        }
                    ],    
                searching: false,
                bLengthChange: false,
                bSort: true,
                destroy: true,
                paging: true,
                pageLength: 25,
                info: false,
            });
        }
    });
}

$(document).on('click', '#dtgridMotivos tr td .clsEditar', function (e) {
    var row = $(this).closest('tr');
    var r = dt.row(row).data();
    //var r = row.data("td");
    console.log(r);
    $("#popupEditar").bPopup({
        modalClose: false
    });
    $("#id-subcausadescpop").val(r.Subcausadesc);
    $("#id-subcausaabrevpop").val(r.Subcausaabrev);
    popupid = r.Subcausacodi;

});
