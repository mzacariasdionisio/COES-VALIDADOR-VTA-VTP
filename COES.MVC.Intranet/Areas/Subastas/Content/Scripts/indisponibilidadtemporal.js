var controlador = siteRoot + 'Subastas/ActivacionOferta/';

var msgSinIndisponibilidad = "No existen datos registrados para la fecha de oferta ingresada.";
var listaDeErroresIndisponibilidadGeneral = [];
var listaUrsConIndisponibilidad = [];

const Indisponibilidad_temporal = "Indisponibilidad temporal";
const Banda_Disponible = "Banda Disponible (MW)";
const Motivo_Indisponibilidad = 'Motivo de indisponibilidad';
var  parametroUrsMinAuto;
var DATA;

var ventanaIndispSoloLectura = true;
var existeIndisponibilidadParaFechaOferta;

var containerTablaIndisponibilidad;
var tblHndIndisponibilidad;

$(function () {

    $('#btnErroresIndisp').click(function () {
        mostrarListadoErroresIndisponibilidad();
    });

    $('#btnGrabarIndisponibilidad').click(function () {
        grabarIndisponibilidadTemporal();
    });

    //anchoTablaActivacion = $("#mainLayout").width();
    //$('#contenedorTablaSubir').css("width", anchoTablaActivacion - 70);
    //$('#contenedorTablaBajar').css("width", anchoTablaActivacion - 70);

});

function cargarDatosIndisponibilidad() {

    var fechaOferta = $("#fechaOferta").val();
    var maniana = diaManiana();

    //Verifico la edicion de datos
    if (maniana === fechaOferta) {
        ventanaIndispSoloLectura = false;
    } else {
        ventanaIndispSoloLectura = true;
    }

    limpiarBarraMensaje("mensaje_indisponibilidad");

    $.ajax({
        type: 'POST',
        url: controlador + "CargarDatosIndisponibilidad",
        data: {
            fechaOferta: fechaOferta
        },
        success: function (evt) {

            if (evt.Resultado == "1") {

                existeIndisponibilidadParaFechaOferta = evt.IndisponibilidadCab != null ? true : false;

                parametroUrsMinAuto = parseFloat($("#hdparamPotUrsMinAuto").val());

                var data = evt.ListaIndisponibilidadTemporal;
                var muestraTablaEnWeb = evt.MostrarTablaIndisponibilidad;

                inicializarIndisponibilidad();

                if (muestraTablaEnWeb) {
                    $("#contenedorBotonesIndisp").css("display", "block");
                    if (evt.IndisponibilidadCab != null) {
                        $('#auditoriaIndis').html(mostrarDatosAuditoriaIndisp(evt.IndisponibilidadCab));
                    } else {
                        $('#auditoriaIndis').html("");
                    }

                    iniciarHandsontableIndisponibilidad();
                    cargarHansonTablaIndisp(data);
                } else {
                    containerTablaIndisponibilidad = null;
                    tblHndIndisponibilidad = null;
                    $("#contenedorBotonesIndisp").css("display", "none");
                    $('#tblIndispTemp').html("");
                    
                    

                    mostrarMensaje('mensaje_indisponibilidad', 'alert', msgSinIndisponibilidad);
                }                

            } else {
                mostrarMensaje('mensaje_indisponibilidad', 'error', evt.Mensaje);
            }
        },
        error: function (xhr, status) {
            mostrarMensaje('mensaje_indisponibilidad', 'error', 'Se ha producido un error.');
        }
    });

}

function inicializarIndisponibilidad() {
    
    listaDeErroresIndisponibilidadGeneral = [];

    //Activo y desactivo notones
    if (ventanaIndispSoloLectura) {
        $('#btnErroresIndisp').css("display", "none");
        $('#btnGrabarIndisponibilidad').css("display", "none");
    } else {
        $('#btnErroresIndisp').css("display", "block");
        $('#btnGrabarIndisponibilidad').css("display", "block");
    }
}


function iniciarHandsontableIndisponibilidad() {
    $('#tblIndispTemp').html("");

    // Inicio datos
    Handsontable.renderers.registerRenderer('datosIngresadosIndisponibilidadRenderer', datosIngresadosIndisponibilidadRenderer); 
    Handsontable.renderers.registerRenderer('datosIngresadosCombosIndisponibilidadRenderer', datosIngresadosCombosIndisponibilidadRenderer);

    const OPTIONS = new Map([
        ['Si', () => ['Total', 'Parcial']],
        ['No', () => []],
    ]);

    // #region Handsontable SEMANA/MES
    containerTablaIndisponibilidad = document.getElementById('tblIndispTemp');

    tblHndIndisponibilidad = new Handsontable(containerTablaIndisponibilidad, {        
        
        dataSchema: {
            Ursnomb: null,
            Emprnomb: null,
            Centralnomb: null,
            BandaUrsCalificada: null,
            IntdetindexisteDesc: null,
            IntdettipoDesc: null,
            Intdetbanda: null,
            Intdetmotivo: null,
            Urscodi: null
        },
        colHeaders: ['URS', 'Empresa', 'Central', 'Banda URS Calificada (MW)', Indisponibilidad_temporal, 'Tipo de Indisponibilidad', Banda_Disponible, Motivo_Indisponibilidad],
        columns: [
            { data: 'Ursnomb', editor: false, className: 'fondo_soloLectura_indisponibilidad htCenter', readOnly: true },
            { data: 'Emprnomb', editor: false, className: 'fondo_soloLectura_indisponibilidad htCenter', readOnly: true },
            { data: 'Centralnomb', editor: false, className: 'fondo_soloLectura_indisponibilidad htLeft', readOnly: true },
            { data: 'BandaUrsCalificada', editor: false, className: 'fondo_soloLectura_indisponibilidad htCenter', readOnly: true },
            //{ data: 'IntdetindexisteDesc', type: 'dropdown', "allowInvalid": false, renderer: customDropDownRender },
            { data: 'IntdetindexisteDesc', type: 'dropdown', "allowInvalid": false, source: ['Si', 'No'] },
            { data: 'IntdettipoDesc', type: 'dropdown', "allowInvalid": false, source: ['Total', 'Parcial'] },
            { data: 'Intdetbanda', },
            { data: 'Intdetmotivo', },
            { data: 'Urscodi', },
        ],
        hiddenColumns: {
            columns: [8],
            indicators: false,
        },
        height: '580px',
        width: '100%',

        //Tamaño maximo de columna
        modifyColWidth: function (width, col) {
            
            if (col == 7) {
                return 450
            }
            
        },

        afterChange(changes, source) {            

            //Al escoger Si debo activar siguiente celda
            if (source === 'loadData' || source === 'internal' || changes.length > 1) {
                return;
            }
            const [row, prop, oldValue, newValue] = changes[0];

            if (prop !== 'IntdetindexisteDesc') {
                return;
            }
            if (!OPTIONS.has(newValue)) {
                return;
            }

            const option = OPTIONS.get(newValue)();

            this.setCellMeta(row, this.propToCol('IntdettipoDesc'), 'source', option);
            this.setDataAtRowProp(row, 'IntdettipoDesc', option[0]);

        },

        beforeChange: function (changes, source) {
            var colBandaUrs = 3;
            var colIndisponibilidad = 4;
            var colTipo = 5;
            var colBandaDisp = 6;
            var colMotivo = 7;
            var colUrscodi = 8;
            if (source === 'loadData' || source === 'internal') {
                return;
            }
            const [[row, prop, oldVal, newVal]] = changes;
            

            if (prop === 'IntdetindexisteDesc') {
                if (newVal == "No") {                    
                    var valorBandaUrs = tblHndIndisponibilidad.getDataAtCell(row, colBandaUrs);
                    this.setDataAtCell(row, colBandaDisp, valorBandaUrs, 'internal');
                    this.setDataAtCell(row, colMotivo, '', 'internal');
                } else {
                    if (newVal == "Si") {
                        this.setDataAtCell(row, colBandaDisp, '', 'internal');
                    }
                }


                //actualizo la lista que tienen indisponibilidad
                var valorUrscodi = tblHndIndisponibilidad.getDataAtCell(row, colUrscodi);
                if (newVal == "No") {
                    quitoUrsDeListaDeIndisponibles(valorUrscodi);
                } else {
                    if (newVal == "Si") {
                        agregoUrsAListaDeIndisponibles(valorUrscodi);
                    }
                }                                

            }

            if (prop === 'IntdettipoDesc') {
                var valorIndisp1 = tblHndIndisponibilidad.getDataAtCell(row, colIndisponibilidad);
                if (valorIndisp1 == "Si") {
                    if (newVal == "Total") {
                        this.setDataAtCell(row, colBandaDisp, 0, 'internal');
                    } else {
                        if (newVal == "Parcial") {
                            this.setDataAtCell(row, colBandaDisp, '', 'internal');
                        }
                    }
                }

            }

        }

    });


    precargarDetallesOtraPestania();
    DATA = tblHndIndisponibilidad;    

    tblHndIndisponibilidad.addHook('beforeChange', function (changes, [source]) {
        listaDeErroresIndisponibilidadGeneral = [];
    });
}

function agregoUrsAListaDeIndisponibles(urscodi) {
    listaUrsConIndisponibilidad = [...new Set(listaUrsConIndisponibilidad)];
    //verifico existencia
    const found = listaUrsConIndisponibilidad.find(x => x == urscodi);

    if (found == undefined || found == null) { //si no esta en lista: agrego
        listaUrsConIndisponibilidad.push(urscodi)
    }
}

function quitoUrsDeListaDeIndisponibles(urscodi) {
    listaUrsConIndisponibilidad = [...new Set(listaUrsConIndisponibilidad)];

    const index = listaUrsConIndisponibilidad.map(x => x).indexOf(urscodi); 
    const filaEliminada = listaUrsConIndisponibilidad.splice(index, 1);

}

function datosIngresadosCombosIndisponibilidadRenderer(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.AutocompleteRenderer.apply(this, arguments);
    var colIndisponibilidad = 4;

    //validaciones campo indisponibilidad (SI / No)
    if (col == colIndisponibilidad) {
        //si no existe reg Indisponibilidad para la fecha en BD
        
        if (!existeIndisponibilidadParaFechaOferta && listaUrsConIndisponibilidad.length == 0) {
            agregarErrorIndisponibilidad("", Indisponibilidad_temporal, "", "Se debe escoger mínimamente una opción 'Si' en el campo 'Indisponibilidad Temporal'");
        }
    }
}


function datosIngresadosIndisponibilidadRenderer(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    var colBandaUrs = 3;
    var colIndisponibilidad = 4;
    var colTipo = 5;
    var colBandaDisp = 6;
    var colMotivo = 7;
    var valUrs = DATA.getDataAtRowProp(row, 'Ursnomb');

    //validaciones campo Indisponibilidad
    if (col == colBandaDisp) {
        var valTipo = DATA.getDataAtRowProp(row, 'IntdettipoDesc');
        var valBandaUrsCalif = parseFloat(DATA.getDataAtRowProp(row, 'BandaUrsCalificada'));

        if (valTipo == "Parcial") {

            if (value != "" && value != null) {
            //if (value != null) {

                //Valido si es NO numerico    
                if (isNaN(value)) {
                    td.className = 'celda_error_roja htCenter';
                    agregarErrorIndisponibilidad(valUrs, Banda_Disponible, value, "Se debe ingresar valor mayor a " + parametroUrsMinAuto );

                } else {
                    if (value < parametroUrsMinAuto || value > valBandaUrsCalif) {
                        if (value < parametroUrsMinAuto) {
                            td.className = 'celda_error_roja htCenter';
                            agregarErrorIndisponibilidad(valUrs, Banda_Disponible, value, "El valor mínimo es " + parametroUrsMinAuto + " por lo tanto se debe seleccionar la opción “Total” en el campo 'Tipo de Indisponibilidad'.");

                        }
                        if (value > valBandaUrsCalif) {
                            td.className = 'celda_error_roja htCenter';
                            agregarErrorIndisponibilidad(valUrs, Banda_Disponible, value, "El valor máximo es  " + valBandaUrsCalif);

                        }
                        
                    } else {
                        //valido si son positivos pero con mas de 3 cifras enteras o mas de 1 cifra decimal
                        //var valor = new Decimal(value);
                        var parteEntera = Math.trunc(value);
                        //var parteDecimal = valor.sub(parteEntera);
                        var parteDecimal;
                        var arrNum = (value + "").split(".");
                        if (arrNum.length > 1) {
                            parteDecimal = (value + "").replace((parteEntera + ""), "0");
                        }

                        var pe = parteEntera + "";
                        var pd = parteDecimal != undefined ? (parteDecimal + "").substring(2) : "";
                        var numPE = pe.length;
                        var numPD = pd.length;

                        //Solo es aceptado si tiene hasta 3 cifras enteras y 2 cifra2 decimales
                        if (numPE > 3 || numPD > 2) {
                            td.className = 'celda_error_roja htCenter';
                            agregarErrorIndisponibilidad(valUrs, Banda_Disponible, value, "Solo está permitido el ingreso de valores con máximo 3 cifras enteras y/o como máximo 2 cifras decimales.");
                            if (row == 3) {
                                if (tipoInfo == SUBIR) datosEnDeficitSubir.push(value);
                                if (tipoInfo == BAJAR) datosEnDeficitBajar.push(value);
                            }
                        } 
                    }
                }
            } else {
                td.className = 'celda_error_roja htCenter';
                agregarErrorIndisponibilidad(valUrs, Banda_Disponible, "", "Se debe ingresar valor mayor a " + parametroUrsMinAuto );
            }
        }
    }

    //validaciones campo Motivo
    if (col == colMotivo) {
        var valIndisponibilidad = DATA.getDataAtRowProp(row, 'IntdetindexisteDesc');

        if (valIndisponibilidad == "Si") {

            if (value != null) {
                var numCaracteres = value.length;

                if (numCaracteres < 3 || numCaracteres > 2000) {
                    td.className = 'celda_error_roja htCenter';
                    agregarErrorIndisponibilidad(valUrs, Motivo_Indisponibilidad, value, "Se debe ingresar valores con más de 3 caracteres, pero menos de 2000 caracteres");
                }
            } else {
                td.className = 'celda_error_roja htCenter';
                agregarErrorIndisponibilidad(valUrs, Motivo_Indisponibilidad, "", "Se debe ingresar valores con más de 3 caracteres, pero menos de 2000 caracteres");
            }
           
        }
    }

}


function agregarErrorIndisponibilidad(ursnomb, columnaDato, valor, mensajeError) {
    //Agrega al array de tipo de informacion
    if (validarErrorIndisp(ursnomb, columnaDato, valor, mensajeError)) {
        var regError = {
            UrsNombre: ursnomb,
            Columna: columnaDato,
            Valor: valor,
            Mensaje: mensajeError
        };

        listaDeErroresIndisponibilidadGeneral.push(regError);
    }
}


function validarErrorIndisp(ursnombre, columna, valor, mensajeError) {
    var arrayData = [];


    arrayData = listaDeErroresIndisponibilidadGeneral.slice();



    for (var j in arrayData) {
        if (arrayData[j]['UrsNombre'] == ursnombre && arrayData[j]['Columna'] == columna && arrayData[j]['Valor'] == valor && arrayData[j]['Mensaje'] == mensajeError) {
            return false;
        }
    }
    return true;
}


function mostrarListadoErroresIndisponibilidad() {
    limpiarBarraMensaje("mensaje_indisponibilidad");

    if (!ventanaIndispSoloLectura) {
        setTimeout(function () {
            $('#tablaErroresIndisponibilidad').html(dibujarTablaErroresIndisponibilidad());

            $('#contenedorErroresIndisponibilidad').bPopup({
                easing: 'easeOutBack',
                speed: 450,
                transition: 'slideDown',
                modalClose: false
            });
            $('#tablaErrorInd').dataTable({
                "scrollY": 330,
                "scrollX": true,
                "sDom": 't',
                "ordering": false,
                "bPaginate": false,
                "iDisplayLength": -1
            });

        }, 200);
    } else {
        mostrarMensaje('mensaje_indisponibilidad', 'error', MSG_ERROR_LECTURA);
    }


}

function dibujarTablaErroresIndisponibilidad() {
    var cadena = `
        <div style='clear:both; height:5px'></div>
            <table id='tablaErrorInd' border='1' class='pretty tabla-adicional' cellspacing='0' style='width: 850px;'>
                <thead>
                    <tr>
                        <th style='width: 80px;'>URS</th>
                        <th style='width: 120px;'>Dato</th>
                        <th style='width: 250px;'>Valor</th>
                        <th style='width: 250px;'>Tipo Error</th>
                    </tr>
                </thead>
                <tbody>
    `;

    //ordeno el listado
    listaDeErroresIndisponibilidadGeneral.sort(function (a, b) {
        return a.UrsNombre.localeCompare(b.UrsNombre) || a.Columna.localeCompare(b.Columna);
    });


    for (var i = 0; i < listaDeErroresIndisponibilidadGeneral.length; i++) {
        var item = listaDeErroresIndisponibilidadGeneral[i];
        cadena += `
                    <tr>
                        <td style='width:  80px; text-align: center; white-space: break-spaces;'>${item.UrsNombre}</td>
                        <td style='width: 120px; text-align: left; white-space: break-spaces;'>${item.Columna}</td>
                        <td style='width: 250px; text-align: left; white-space: break-spaces;'>${item.Valor}</td>
                        <td style='width: 250px; text-align: left; white-space: break-spaces;'>${item.Mensaje}</td>
                    </tr>
        `;
    }

    cadena += `
                </tbody>
            </table>
        </div>
    `;

    return cadena;
}



function cargarHansonTablaIndisp(listaIndisponibilidades) {
    listaUrsConIndisponibilidad = [];
    var lstInd = listaIndisponibilidades && listaIndisponibilidades.length > 0 ? listaIndisponibilidades : [];

    var lstData = [];
    tblHndIndisponibilidad.loadData(lstData);

    for (var index in lstInd) {

        var item = lstInd[index];


        var data = {
            Ursnomb: item.Ursnomb,
            Emprnomb: item.Emprnomb,
            Centralnomb: item.Centralnomb,
            BandaUrsCalificada: item.BandaUrsCalificada,
            IntdetindexisteDesc: item.IntdetindexisteDesc,
            IntdettipoDesc: item.IntdettipoDesc,
            Intdetbanda: item.Intdetbanda,
            Intdetmotivo: item.Intdetmotivo,
            Urscodi: item.Urscodi

        };

        lstData.push(data);

        //lleno tabla indisponibilidad (aquellos que tienen SI en indisponibilidad temporal)
        if (item.IntdetindexisteDesc == 'Si') {
            listaUrsConIndisponibilidad.push(item.Urscodi);
        }
    }
    tblHndIndisponibilidad.loadData(lstData);

}


function mostrarDatosAuditoriaIndisp(regIndisp) {
    var cadena = `
        <div style='clear:both; height:5px'></div>
            <table id='' border='0' class='' cellspacing='0' style='width: auto;'>
                <tr>
                    <td style='width: 140px; color: #009AD9;'>Usuario Creación:</td>
                    <td style='width: 300px;'>${regIndisp.Intcabusucreacion}</td>
                </tr>
                <tr>
                    <td style='width: 140px; color: #009AD9;'>Fecha Creación:</td>
                    <td style='width: 300px;'>${regIndisp.IntcabfeccreacionDesc}</td>
                </tr>
                <tr>
                    <td style='width: 140px; color: #009AD9;'>Usuario Modificación:</td>
                    <td style='width: 300px;'>${regIndisp.Intcabusumodificacion}</td>
                </tr>
                <tr>
                    <td style='width: 140px; color: #009AD9;'>Fecha Modificación:</td>
                    <td style='width: 300px;'>${regIndisp.IntcabfecmodificacionDesc}</td>
                </tr>
           </table>
       </div>
                
    `;

    return cadena;
}

function grabarIndisponibilidadTemporal() {
    limpiarBarraMensaje("mensaje_indisponibilidad");

    if (!ventanaIndispSoloLectura) {
        //Verifico la existencia de errores
        if (listaDeErroresIndisponibilidadGeneral.length == 0) {
            guardarDatosIndisponibilidadTemporaUrs();
        } else {
            mostrarListadoErroresIndisponibilidad();
        }
    } else {
        mostrarMensaje('mensaje_indisponibilidad', 'error', MSG_ERROR_LECTURA);
    }
}


function guardarDatosIndisponibilidadTemporaUrs() {
    limpiarBarraMensaje("mensaje_indisponibilidad");

    var dataGuardar = obtenerDataIndGuardar();
    var dataJson = {
        fechaOferta: $("#fechaOferta").val(),
        datosAGuardar: dataGuardar
    };

    $.ajax({
        url: controlador + "GuardarIndisponibilidadTemporaUrs",
        type: 'POST',
        contentType: 'application/json; charset=UTF-8',
        dataType: 'json',
        data: JSON.stringify(dataJson),
        success: function (result) {

            if (result.Resultado == "1") {
                mostrarMensaje('mensaje_indisponibilidad', 'exito', 'La información fue registrada correctamente.');
                $('#auditoriaIndis').html(mostrarDatosAuditoriaIndisp(result.IndisponibilidadCab));
                existeIndisponibilidadParaFechaOferta = true;
            } else {
                mostrarMensaje('mensaje_indisponibilidad', 'error', result.Mensaje);
            }
        },
        error: function (xhr, status) {
            mostrarMensaje('mensaje_indisponibilidad', 'error', 'Se ha producido un error.');
        }
    });

}

function obtenerDataIndGuardar() {
    var dataHandson = [];
    dataHandson = tblHndIndisponibilidad.getSourceData();

    var lstData = dataHandson;

    return lstData;
}
