
$(document).ready(function () {
    $('.nt-header-option.suboptions').click(function () {
        const id = this.id;
        $(this).addClass('active')
            .siblings()
            .removeClass('active');
        displaySelModule(id);
    });
    
    iniSelModulo();
});

function iniSelModulo() {
    const arrayMenu = $('.suboptions');
    $.each(arrayMenu, function (i, item) {
        if ($(item).hasClass('active')) {
            displaySelModule(item.id);
            return false;
        }
    });
}

function displaySelModule(option) {
    const module = `${option.split("subopt-")[1]}`;//nombre PartialActionResult a mostrar
    $.ajax({
        type: 'POST',
        url: controlador + module,
        success: function (result) {
            //$('.Zebra_DatePicker').remove();//Elimina los remanentes del Zebra_DatePicker al cambiar de partialView
            $('.sub-module-popup').remove();//Elimina los remanentes del Popup al cambiar de partialView
            //SetMessage('#message', $('#main').data('msg'), $('#main').data('tpo'));
            $('#submodule-container').html(result);//Carga el html del nuevo partialView
            //updateStateOptVersion();
        },
        error: function () {
            SetMessage('#message', 'Ha ocurrido un problema...', 'error');
            $('#module').html('');
        }
    });
}

function formatDate(dateString) {
    // Extrae el número de milisegundos
    const milliseconds = parseInt(dateString.replace(/\/Date\((\d+)\)\//, '$1'));
    const date = new Date(milliseconds);

    // Formato dd/mm/yyyy
    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0'); // Meses son 0-indexados
    const year = date.getFullYear();

    return `${day}/${month}/${year}`;
}

function formatDateComplete(dateString) {
    // Extrae el número de milisegundos 
    const milliseconds = parseInt(dateString.replace(/\/Date\((\d+)\)\//, '$1'));
    const date = new Date(milliseconds);

    // Formato dd/mm/yyyy 
    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0');
    // Meses son 0-indexados 
    const year = date.getFullYear();
    // Formato hh:mm:ss 
    const hours = String(date.getHours()).padStart(2, '0');
    const minutes = String(date.getMinutes()).padStart(2, '0');
    const seconds = String(date.getSeconds()).padStart(2, '0');

    return `${day}/${month}/${year} ${hours}:${minutes}:${seconds}`;
}