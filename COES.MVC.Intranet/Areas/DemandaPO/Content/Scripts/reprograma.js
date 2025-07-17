var pop_importar;
var ht_vegtativa;

const _datos = [
    ["05/08/2022", 0, 0, 0, 0, 0, 0],
    ["05/08/2022", 0, 0, 0, 0, 0, 0],
    ["05/08/2022", 0, 0, 0, 0, 0, 0],
    ["05/08/2022", 0, 0, 0, 0, 0, 0],
    ["05/08/2022", 0, 0, 0, 0, 0, 0],
    ["05/08/2022", 0, 0, 0, 0, 0, 0],
    ["05/08/2022", 0, 0, 0, 0, 0, 0],
];

$(document).ready(function () {
    $('.nt-header-option').click(function () {
        $(this).addClass('active')
            .siblings()
            .removeClass('active');
    });

    $('#selVersion').multipleSelect({
        filter: true,
        single: true,
        placeholder: 'Seleccione Versión...',
    });

    $('#pop-imp-version').multipleSelect({
        filter: true,
        single: true,
        placeholder: 'Seleccione Versión...',
    });

    $('#pop-dup-version').multipleSelect({
        filter: true,
        single: true,
        placeholder: 'Seleccione Versión...',
    });

    $('#selFecha').Zebra_DatePicker();
    $('#pop-nue-fecha').Zebra_DatePicker();
    $('#pop-imp-fecha').Zebra_DatePicker();
    $('#pop-dup-fecha').Zebra_DatePicker();

    $('#btnNuevo').on('click', function () {
        pop_importar = $('#pop-nuevo').bPopup({
            easing: 'easeOutBack',
            speed: 350,
            transition: 'fadeIn',
            modalClose: false,
        });
    });

    $('#btnImportar').on('click', function () {
        pop_importar = $('#pop-importar').bPopup({
            easing: 'easeOutBack',
            speed: 350,
            transition: 'fadeIn',
            modalClose: false,
        });
    });

    $('#btnDuplicar').on('click', function () {
        pop_importar = $('#pop-duplicar').bPopup({
            easing: 'easeOutBack',
            speed: 350,
            transition: 'fadeIn',
            modalClose: false,
        });
    });

    iniHandson();
})

function iniHandson() {
    const container = document.getElementById('ht_vegetativa');
    ht_vegtativa = new Handsontable(container, {
        data: _datos,
        fillHandle: true,
        stretchH: 'all',
        minSpareCols: 0,
        minSpareRows: 0,  
        colHeaders: [
            'Fecha Hora', 'Barra 1', 'Barra 2',
            'Barra 3', 'Barra 4', 'Barra 5',
            'Barra 6'],
    });
}

