var _pop;

const moduloAjusteSein = 6;
const exeObtenerDatosxModulo = {
    '1': 'obtenerDatosVeg()',
    '2': 'obtenerDatosUL()',
    '3': 'obtenerDatosTotalBarras()',
    '4': 'obtenerDatos()',
    '5': 'obtenerDatos()',
    '6': 'obtenerDatos()',
};

$(document).ready(function () {
    $('.nt-header-option.suboptions').click(function () {
        const id = this.id;
        $(this).addClass('active')
            .siblings()
            .removeClass('active');
        displaySelModule(id);
    });
    
    $('#repSelFecha').Zebra_DatePicker({
        onSelect: function () {
            updateVersionPorFecha();            
        }
    });
    $('#repSelFechaAnte').Zebra_DatePicker({
        onSelect: function () {
            updateVersionPorFecAnterior();  
        }
    });
    $('#repSelFechaComp').Zebra_DatePicker({
        onSelect: function () {
            updateVersionComparacion();
            updateEscenarioYupana();
        }
    });
    $('#repSelVersion').multipleSelect({
        filter: true,
        single: true,
        placeholder: 'Seleccione Versión...',
        onClose: function () {
            updateStateOptGuardar();           
        }
    });
    $('#repSelVersionAnte').multipleSelect({
        filter: true,
        single: true,
        placeholder: 'Seleccione Versión...',
    });
    $('#repSelVersionComp').multipleSelect({
        filter: true,
        single: true,
        placeholder: 'Seleccione Versión...',        
    });

    $('#btnNuevo').click(function () {
        _pop = $('#repPopNuevo').bPopup({
            easing: 'easeOutBack',
            speed: 350,
            transition: 'fadeIn',
            modalClose: false,
        });
    });
    $('#btnImportar').click(function () {
        const [_version] = $('#repSelVersion')
            .multipleSelect('getSelects');
        if (!_version) {
            alert('Debe seleccionar una versión para realizar una importación');
            return false;
        }

        _pop = $('#repPopImportar').bPopup({
            easing: 'easeOutBack',
            speed: 350,
            transition: 'fadeIn',
            modalClose: false,
        });
    });
    $('#btnConsVersion').click(function () {
        const mdl = $('#contentModulo').data('modulo');
        eval(exeObtenerDatosxModulo[mdl]);
    });
    $('#btnConsComparacion').click(function () {
        const mdl = $('#contentModulo').data('modulo');
        eval(exeObtenerDatosxModulo[mdl]);
    });

    $('#btnDuplicar').click(function () {
        _pop = $('#repPopDuplicar').bPopup({
            easing: 'easeOutBack',
            speed: 350,
            transition: 'fadeIn',
            modalClose: false,
        });
    });

    //Controles del popup "Nuevo"
    $('#rpnInpFecha').Zebra_DatePicker();
    $('#rpnBtnNuevo').click(function () {
        const _fecha = $('#rpnInpFecha').val();
        const _nombre = $('#rpnInpNombre').val();
        const _modulo = $('#contentModulo').data('modulo');
        createVersion(_modulo, -1, _fecha, _nombre);
    });
    //---------------------------------------------

    //Controles del popup "Importar"
    $('#rpiInpFechaVersion').Zebra_DatePicker({
        onSelect: function () {
            updateVersionImportar();
        },
    });    
    $('#rpiSelVersion').multipleSelect({
        filter: true,
        single: true,
        placeholder: 'Seleccione Versión...',        
    });
    $('#rpiBtnImportar').click(function () {     
        const _modulo = $('#contentModulo').data('modulo');

        const [_version] = $('#repSelVersion')
            .multipleSelect('getSelects');        
        const [_verImportar] = $('#rpiSelVersion')
            .multipleSelect('getSelects');        
        
        const _fecha = $('#repSelFecha').val();
        const _fecImportar = $('#rpiInpFechaVersion').val();

        const [nomVersion] = $('#repSelVersion')
            .multipleSelect('getSelects', 'text');

        const msg = 'Se reemplazaran y guardarán los datos de la versión '
            + `${nomVersion} con la seleccionada`;

        if (confirm(msg)) {
            importVersion(_modulo, _verImportar,
                _fecImportar, _version, _fecha);
        }
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
            updateStateOptVersion();
        },
        error: function () {
            SetMessage('#message', 'Ha ocurrido un problema...', 'error');
            $('#module').html('');
        }
    });
}

function updateVersionPorFecha() {
    $.ajax({
        type: 'POST',
        url: controlador + 'ActualizarVersionPO',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            fecha: $('#repSelFecha').val(),
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            RefillDropDowList($('#repSelVersion'),
                result, 'Vergrpcodi', 'Vergrpnomb');
            updateStateOptGuardar();
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function updateVersionPorFecAnterior() {
    $.ajax({
        type: 'POST',
        url: controlador + 'ActualizarVersion',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            fecha: $('#repSelFechaAnte').val(),
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            RefillDropDowList($('#repSelVersionAnte'),
                result, 'Vergrpcodi', 'Vergrpnomb');
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function createVersion(
    modulo, version, fecha, nombre) {
    $.ajax({
        type: 'POST',
        url: controlador + 'CrearVersion',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            idModulo: modulo,
            idVersion: version,
            selFecha: fecha,
            selNombre: nombre,
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            alert(result);
            updateVersionPorFecha();
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        },
        complete: function () {
            _pop.close();
        },
    });
}

function importVersion(modulo,
    verImportar, fecImportar,
    version, fecha) {    
    $.ajax({
        type: 'POST',
        url: controlador + 'ImportarVersion',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            idModulo: modulo,
            idVersionImportar: verImportar,
            selFechaImportar: fecImportar,
            idVersion: version,
            selFecha: fecha,
        }),
        datatype: 'json',
        traditional: true,
        success: function (model) {
            eval(exeObtenerDatosxModulo[modulo]);
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        },
        complete: function () {
            _pop.close();
        },
    });
}

function updateVersionImportar() {
    $.ajax({
        type: 'POST',
        url: controlador + 'ActualizarVersion',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            fecha: $('#rpiInpFechaVersion').val(),
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            RefillDropDowList($('#rpiSelVersion'),
                result, 'Vergrpcodi', 'Vergrpnomb');            
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function updateVersionComparacion() {
    $.ajax({
        type: 'POST',
        url: controlador + 'ActualizarVersion',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            fecha: $('#repSelFechaComp').val(),
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            RefillDropDowList($('#repSelVersionComp'),
                result, 'Vergrpcodi', 'Vergrpnomb');
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function updateStateOptGuardar() {
    const [_id] = $('#repSelVersion')
        .multipleSelect('getSelects');
    console.log(_id);
    if (_id)
        $('#btnGuardar').parent()
        .removeClass('disabled');
    else
        $('#btnGuardar').parent()
        .addClass('disabled');
}

function updateStateOptVersion() {
    const modulosValidos = [1];
    const selModulo = $('#contentModulo').data('modulo');

    if (modulosValidos.includes(selModulo)) {
        $('#btnNuevo').parent()
            .removeClass('disabled');
        $('#btnImportar').parent()
            .removeClass('disabled');
    }

    else {
        $('#btnNuevo').parent()
            .addClass('disabled');
        $('#btnImportar').parent()
            .addClass('disabled');
    }
        
}

function updateEscenarioYupana() {
    const _modulo = $('#contentModulo').data('modulo');
    if (_modulo == moduloAjusteSein)
        eval("obtenerEscenarios()");
}

function RefillDropDowList(element, data, data_id, data_name) {
    element.empty();
    $.each(data, function (i, item) {
        let n_value = i, n_html = item;
        if (data_id) n_value = item[data_id];
        if (data_name) n_html = item[data_name];        
        element.append($('<option></option>').val(n_value).html(n_html));    
    });
    element.multipleSelect('refresh');
}

function HoraColumnRenderer(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.color = 'MidnightBlue';
    td.style.background = '#E8F6FF';    
}

function HeaderCellRenderer(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.fontSize = '10px';
    td.style.fontWeight = 'bold';
    td.style.color = '#fff';
    td.style.background = '#2980B9';
    td.style.height = '45px';
}