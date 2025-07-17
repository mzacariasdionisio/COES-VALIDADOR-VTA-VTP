///Ejemplo de uso:
//var objConf = {
//    idPopup: '#popupSeleccionarFecha',
//    idFechaInicio: '#Entidad_Interfechaini',
//    idFechaFin: '#Entidad_Interfechafin',
//    titulo: 'Seleccionar fechas'
//}
//selectDate_mostrarPopup(objConf);

/////////////////////////////////////////////////////////////////////////////////////////////////
/// Funcionalidad para seleccionar días, semanas, meses 
/// <m: Mes anterior
/// m>: Mes siguiente
/// [m]: Mes completo
/// <s: Semana anterior
/// s>: Semana siguiente 
/// [s]: Semana completo
/// <<: siete días antes
/// <: 1 día antes
/// >>: siete días después
/// >: 1 día después

function selectDate_mostrarPopup(objConf) {
    var idpopup = objConf.idPopup, idFechaIniMain = objConf.idFechaInicio, idFechaFinMain = objConf.idFechaFin, titulo = objConf.Titulo;

    $(idpopup).html(selectDate_HtmlPopup());

    //Cambiar titulo
    var idTitulo = idpopup + ' .popup-title span';
    $(idTitulo).html(titulo);

    //guardar los ids del formulario principal
    var idselectDate_fecha_inicio_Main = idpopup + ' .' + 'selectDate_fecha_inicio_Main';
    var idselectDate_fecha_fin_Main = idpopup + ' .' + 'selectDate_fecha_fin_Main';
    $(idselectDate_fecha_inicio_Main).val(idFechaIniMain);
    $(idselectDate_fecha_fin_Main).val(idFechaFinMain);

    //
    var valorInputFechaIni = $(idFechaIniMain).val();
    var valorInputFechaFin = $(idFechaFinMain).val();
    var valorFechaIni = selectDate_validarFecha(valorInputFechaIni) ? valorInputFechaIni : selectDate_obtenerFechaHoy();
    var valorFechaFin = selectDate_validarFecha(valorInputFechaFin) ? valorInputFechaFin : selectDate_obtenerFechaHoy();

    var idselectDate_fecha_inicio = idpopup + ' .' + 'selectDate_fecha_inicio';
    var idselectDate_fecha_fin = idpopup + ' .' + 'selectDate_fecha_fin';

    $(idselectDate_fecha_inicio).val(valorFechaIni);
    $(idselectDate_fecha_fin).val(valorFechaFin);

    //zebra fecha inicio
    $(idselectDate_fecha_inicio).unbind();
    $(idselectDate_fecha_inicio).Zebra_DatePicker({
        pair: $(idselectDate_fecha_fin),
        onSelect: function (date) {
            //$(idselectDate_fecha_fin).val(date);

            var date1 = getFecha(date);
            var date2 = getFecha($(idselectDate_fecha_fin).val());
            if (date1 > date2) {
                $(idselectDate_fecha_fin).val(date);
            }
        }
    });

    //zebra fecha fin
    $(idselectDate_fecha_fin).unbind();
    $(idselectDate_fecha_fin).Zebra_DatePicker({
        direction: true
    });

    var idbtnSFMesAnterior = idpopup + " ." + 'btnSFMesAnterior';
    var idbtnSFSemAnterior = idpopup + " ." + 'btnSFSemAnterior';
    var idbtnSFSemCompleto = idpopup + " ." + 'btnSFSemCompleto';
    var idbtnSFMesCompleto = idpopup + " ." + 'btnSFMesCompleto';
    var idbtnSFSemSiguiente = idpopup + " ." + 'btnSFSemSiguiente';
    var idbtnSFMesSiguiente = idpopup + " ." + 'btnSFMesSiguiente';

    var idbtnSF7dAntes = idpopup + " ." + 'btnSF7dAntes';
    var idbtnSF1dAntes = idpopup + " ." + 'btnSF1dAntes';
    var idbtnSF1dDespues = idpopup + " ." + 'btnSF1dDespues';
    var idbtnSF7dDespues = idpopup + " ." + 'btnSF7dDespues';

    var idbtnSF1dAntesFechaIni = idpopup + " ." + 'btnSF1dAntesFechaIni';
    var idbtnSF1dDespuesFechaIni = idpopup + " ." + 'btnSF1dDespuesFechaIni';
    var idbtnSF1dAntesFechaFin = idpopup + " ." + 'btnSF1dAntesFechaFin';
    var idbtnSF1dDespuesFechaFin = idpopup + " ." + 'btnSF1dDespuesFechaFin';

    //Funciones Mes, Semana
    $(idbtnSFMesAnterior).unbind();
    $(idbtnSFMesAnterior).click(function () {
        selectDate_SFMesAnterior(idselectDate_fecha_inicio, idselectDate_fecha_fin);
    });
    $(idbtnSFSemAnterior).unbind();
    $(idbtnSFSemAnterior).click(function () {
        selectDate_SFSemAnterior(idselectDate_fecha_inicio, idselectDate_fecha_fin);
    });
    $(idbtnSFSemCompleto).unbind();
    $(idbtnSFSemCompleto).click(function () {
        selectDate_SFSemCompleto(idselectDate_fecha_inicio, idselectDate_fecha_fin);
    });
    $(idbtnSFMesCompleto).unbind();
    $(idbtnSFMesCompleto).click(function () {
        selectDate_SFMesCompleto(idselectDate_fecha_inicio, idselectDate_fecha_fin);
    });
    $(idbtnSFSemSiguiente).unbind();
    $(idbtnSFSemSiguiente).click(function () {
        selectDate_SFSemSiguiente(idselectDate_fecha_inicio, idselectDate_fecha_fin);
    });
    $(idbtnSFMesSiguiente).unbind();
    $(idbtnSFMesSiguiente).click(function () {
        selectDate_SFMesSiguiente(idselectDate_fecha_inicio, idselectDate_fecha_fin);
    });

    //Funciones días para ambos dates
    $(idbtnSF7dAntes).unbind();
    $(idbtnSF7dAntes).click(function () {
        selectDate_SF7dAntes(idselectDate_fecha_inicio, idselectDate_fecha_fin);
    });
    $(idbtnSF1dAntes).unbind();
    $(idbtnSF1dAntes).click(function () {
        selectDate_SF1dAntes(idselectDate_fecha_inicio, idselectDate_fecha_fin);
    });
    $(idbtnSF1dDespues).unbind();
    $(idbtnSF1dDespues).click(function () {
        selectDate_SF1dDespues(idselectDate_fecha_inicio, idselectDate_fecha_fin);
    });
    $(idbtnSF7dDespues).unbind();
    $(idbtnSF7dDespues).click(function () {
        selectDate_SF7dDespues(idselectDate_fecha_inicio, idselectDate_fecha_fin);
    });

    //Funciones días para un date
    $(idbtnSF1dAntesFechaIni).unbind();
    $(idbtnSF1dAntesFechaIni).click(function () {
        selectDate_SF1dAntesFechaIni(idselectDate_fecha_inicio, idselectDate_fecha_fin);
    });
    $(idbtnSF1dDespuesFechaIni).unbind();
    $(idbtnSF1dDespuesFechaIni).click(function () {
        selectDate_SF1dDespuesFechaIni(idselectDate_fecha_inicio, idselectDate_fecha_fin);
    });
    $(idbtnSF1dAntesFechaFin).unbind();
    $(idbtnSF1dAntesFechaFin).click(function () {
        selectDate_SF1dAntesFechaFin(idselectDate_fecha_inicio, idselectDate_fecha_fin);
    });
    $(idbtnSF1dDespuesFechaFin).unbind();
    $(idbtnSF1dDespuesFechaFin).click(function () {
        selectDate_SF1dDespuesFechaFin(idselectDate_fecha_inicio, idselectDate_fecha_fin);
    });

    //botones de acciones del popup
    var idbtnSelectDatePopup = idpopup + " ." + 'btnSelectDatePopup';
    var idbtnCancelarSelectDatePopup = idpopup + " ." + 'btnCancelarSelectDatePopup';

    $(idbtnSelectDatePopup).unbind();
    $(idbtnSelectDatePopup).click(function () {
        var idFecIniMain = $(idselectDate_fecha_inicio_Main).val();
        var idFecFinMain = $(idselectDate_fecha_fin_Main).val();

        $(idFecIniMain).val($(idselectDate_fecha_inicio).val());
        $(idFecFinMain).val($(idselectDate_fecha_fin).val());

        $(idpopup).bPopup().close();
    });

    $(idbtnCancelarSelectDatePopup).unbind();
    $(idbtnCancelarSelectDatePopup).click(function () {
        $(idpopup).bPopup().close();
    });

    //mostrar popup
    setTimeout(function () {
        $(idpopup).bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown'
        });
    }, 50);
}

function selectDate_validarFecha(strFecha) {
    if (strFecha !== undefined && strFecha != null && strFecha.length == 10) {
        return strFecha.match(/^([0-2][0-9]|(3)[0-1])(\/)(((0)[0-9])|((1)[0-2]))(\/)\d{4}$/i) != null;
    }

    return false;
}

function selectDate_obtenerFechaHoy() {
    return selectDate_obtenerFechaByDate(new Date());
}

function selectDate_obtenerFechaByDate(objDateTime) {
    var dd = String(objDateTime.getDate()).padStart(2, '0');
    var mm = String(objDateTime.getMonth() + 1).padStart(2, '0'); //January is 0!
    var yyyy = objDateTime.getFullYear();

    var fechaStr = today = dd + '/' + mm + '/' + yyyy;

    return fechaStr;
}

//Funciones Mes, Semana
function selectDate_SFMesAnterior(idFechaIni, idFechaFin) {
    var valorFechaIni = selectDate_GetValorFechaValido(idFechaIni);

    var date1 = selectDate_GetInicioMes(-1, getFechaDate(valorFechaIni));
    var date2 = selectDate_GetInicioMes(1, date1);
    date2.setDate(date2.getDate() - 1);

    selectDate_SetValorFechaFromDate(idFechaIni, date1, idFechaFin, date2);
}
function selectDate_SFSemAnterior(idFechaIni, idFechaFin) {
    var valorFechaIni = selectDate_GetValorFechaValido(idFechaIni);
    var valorFechaFin = selectDate_GetValorFechaValido(idFechaFin);

    var date1 = selectDate_GetInicioSemana(getFechaDate(valorFechaIni));
    date1.setDate(date1.getDate() - 7);
    var date2 = new Date(date1);
    date2.setDate(date2.getDate() + 6);

    selectDate_SetValorFechaFromDate(idFechaIni, date1, idFechaFin, date2);
}
function selectDate_SFSemCompleto(idFechaIni, idFechaFin) { //semana actual
    var valorFechaIni = selectDate_GetValorFechaValido(idFechaIni);

    var date1 = selectDate_GetInicioSemana(getFechaDate(valorFechaIni));
    var date2 = new Date(date1);
    date2.setDate(date2.getDate() + 6);

    selectDate_SetValorFechaFromDate(idFechaIni, date1, idFechaFin, date2);
}
function selectDate_SFMesCompleto(idFechaIni, idFechaFin) {//mes actual
    var valorFechaIni = selectDate_GetValorFechaValido(idFechaIni);

    var date1 = selectDate_GetInicioMes(0, getFechaDate(valorFechaIni));
    var date2 = selectDate_GetInicioMes(1, date1);
    date2.setDate(date2.getDate() - 1);

    selectDate_SetValorFechaFromDate(idFechaIni, date1, idFechaFin, date2);
}
function selectDate_SFSemSiguiente(idFechaIni, idFechaFin) {
    var valorFechaIni = selectDate_GetValorFechaValido(idFechaIni);

    var date1 = selectDate_GetInicioSemana(getFechaDate(valorFechaIni));
    date1.setDate(date1.getDate() + 7);
    var date2 = new Date(date1);
    date2.setDate(date2.getDate() + 6);

    selectDate_SetValorFechaFromDate(idFechaIni, date1, idFechaFin, date2);
}
function selectDate_SFMesSiguiente(idFechaIni, idFechaFin) {
    var valorFechaIni = selectDate_GetValorFechaValido(idFechaIni);

    var date1 = selectDate_GetInicioMes(1, getFechaDate(valorFechaIni));
    var date2 = selectDate_GetInicioMes(1, date1);
    date2.setDate(date2.getDate() - 1);

    selectDate_SetValorFechaFromDate(idFechaIni, date1, idFechaFin, date2);
}

//Funciones días para ambos dates
function selectDate_SF7dAntes(idFechaIni, idFechaFin) {
    var valorFechaIni = selectDate_GetValorFechaValido(idFechaIni);
    var valorFechaFin = selectDate_GetValorFechaValido(idFechaFin);

    var date1 = getFechaDate(valorFechaIni);
    date1.setDate(date1.getDate() - 7);
    var date2 = getFechaDate(valorFechaFin);
    date2.setDate(date2.getDate() - 7);
    if (date1 > date2) {
        date2 = date1;
    }

    selectDate_SetValorFechaFromDate(idFechaIni, date1, idFechaFin, date2);
}
function selectDate_SF1dAntes(idFechaIni, idFechaFin) {
    var valorFechaIni = selectDate_GetValorFechaValido(idFechaIni);
    var valorFechaFin = selectDate_GetValorFechaValido(idFechaFin);

    var date1 = getFechaDate(valorFechaIni);
    date1.setDate(date1.getDate() - 1);
    var date2 = getFechaDate(valorFechaFin);
    date2.setDate(date2.getDate() - 1);
    if (date1 > date2) {
        date2 = date1;
    }

    selectDate_SetValorFechaFromDate(idFechaIni, date1, idFechaFin, date2);
}
function selectDate_SF1dDespues(idFechaIni, idFechaFin) {
    var valorFechaIni = selectDate_GetValorFechaValido(idFechaIni);
    var valorFechaFin = selectDate_GetValorFechaValido(idFechaFin);

    var date1 = getFechaDate(valorFechaIni);
    date1.setDate(date1.getDate() + 1);
    var date2 = getFechaDate(valorFechaFin);
    date2.setDate(date2.getDate() + 1);
    if (date1 > date2) {
        date2 = date1;
    }

    selectDate_SetValorFechaFromDate(idFechaIni, date1, idFechaFin, date2);
}
function selectDate_SF7dDespues(idFechaIni, idFechaFin) {
    var valorFechaIni = selectDate_GetValorFechaValido(idFechaIni);
    var valorFechaFin = selectDate_GetValorFechaValido(idFechaFin);

    var date1 = getFechaDate(valorFechaIni);
    date1.setDate(date1.getDate() + 7);
    var date2 = getFechaDate(valorFechaFin);
    date2.setDate(date2.getDate() + 7);
    if (date1 > date2) {
        date2 = date1;
    }

    selectDate_SetValorFechaFromDate(idFechaIni, date1, idFechaFin, date2);
}

//Funciones días para un date
function selectDate_SF1dAntesFechaIni(idFechaIni, idFechaFin) {
    var valorFechaIni = selectDate_GetValorFechaValido(idFechaIni);
    var valorFechaFin = selectDate_GetValorFechaValido(idFechaFin);

    var date1 = getFechaDate(valorFechaIni);
    date1.setDate(date1.getDate() - 1);
    var date2 = getFechaDate(valorFechaFin);
    if (date1 > date2) {
        date2 = date1;
    }

    selectDate_SetValorFechaFromDate(idFechaIni, date1, idFechaFin, date2);
}
function selectDate_SF1dDespuesFechaIni(idFechaIni, idFechaFin) {
    var valorFechaIni = selectDate_GetValorFechaValido(idFechaIni);
    var valorFechaFin = selectDate_GetValorFechaValido(idFechaFin);

    var date1 = getFechaDate(valorFechaIni);
    date1.setDate(date1.getDate() + 1);
    var date2 = getFechaDate(valorFechaFin);
    if (date1 > date2) {
        date2 = date1;
    }

    selectDate_SetValorFechaFromDate(idFechaIni, date1, idFechaFin, date2);
}

function selectDate_SF1dAntesFechaFin(idFechaIni, idFechaFin) {
    var valorFechaIni = selectDate_GetValorFechaValido(idFechaIni);
    var valorFechaFin = selectDate_GetValorFechaValido(idFechaFin);

    var date1 = getFechaDate(valorFechaIni);
    var date2 = getFechaDate(valorFechaFin);
    date2.setDate(date2.getDate() - 1);
    if (date1 > date2) {
        date1 = date2;
    }

    selectDate_SetValorFechaFromDate(idFechaIni, date1, idFechaFin, date2);
}
function selectDate_SF1dDespuesFechaFin(idFechaIni, idFechaFin) {
    var valorFechaIni = selectDate_GetValorFechaValido(idFechaIni);
    var valorFechaFin = selectDate_GetValorFechaValido(idFechaFin);

    var date1 = getFechaDate(valorFechaIni);
    var date2 = getFechaDate(valorFechaFin);
    date2.setDate(date2.getDate() + 1);
    if (date1 > date2) {
        date1 = date2;
    }

    selectDate_SetValorFechaFromDate(idFechaIni, date1, idFechaFin, date2);
}

function selectDate_HtmlPopup() {
    return `
    <span class="button b-close"><span>X</span></span>
    <div class="popup-title"><span>Seleccionar fecha</span></div>
    <div class="verSeleccionarFecha">
        <table style="margin-top: 8px;">
            <tr>
                <td>
                    <input type="button" class="btnSFMesAnterior" value="<m" style="width: 38px; text-align: center;" title="Mes anterior" />
                </td>
                <td>
                    <input type="button" class="btnSFSemAnterior" value="<s" style="width: 38px; text-align: center;" title="Semana anterior"/>
                </td>
                <td>
                    <input type="button" class="btnSFSemCompleto" value="[s]" style="width: 38px; text-align: center;" title="Semana completo"/>
                </td>
                <td>
                    <input type="button" class="btnSFMesCompleto" value="[m]" style="width: 38px; text-align: center;" title="Mes completo"/>
                </td>
                <td>
                    <input type="button" class="btnSFSemSiguiente" value="s>" style="width: 38px; text-align: center;" title="Semana siguiente "/>
                </td>
                <td>
                    <input type="button" class="btnSFMesSiguiente" value="m>" style="width: 38px; text-align: center;" title="Mes siguiente"/>
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <input type="button" class="btnSF7dAntes" value="<<" style="width: 38px; text-align: center;" title="siete días antes" />
                </td>
                <td>
                    <input type="button" class="btnSF1dAntes" value="<" style="width: 38px; text-align: center;" title="1 día antes" />
                </td>
                <td>
                    <input type="button" class="btnSF1dDespues" value=">" style="width: 38px; text-align: center;" title="1 día después" />
                </td>
                <td>
                    <input type="button" class="btnSF7dDespues" value=">>" style="width: 38px; text-align: center;" title="siete días después" />
                </td>
                <td></td>
            </tr>
            <tr>
                <td colspan="2" style="padding-top: 15px;text-align: right;">Fecha Inicio</td>
                <td colspan="2" style="padding-top: 15px;">
                    <input type="text" class="selectDate_fecha_inicio" value="" style="width:91px;" />
                </td>
                <td colspan="2" style="padding-top: 15px;">
                    <input type="button" class="btnSF1dAntesFechaIni" value="<" style="width: 27px; text-align: center;" title="1 día antes" />
                    <input type="button" class="btnSF1dDespuesFechaIni" value=">" style="width: 27px; text-align: center;" title="1 día después" />
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: right;">Fecha Fin</td>
                <td colspan="2">
                    <input type="text" class="selectDate_fecha_fin" value="" style="width:91px;" />
                </td>
                <td colspan="2">
                    <input type="button" class="btnSF1dAntesFechaFin" value="<" style="width: 27px; text-align: center;" title="1 día antes" />
                    <input type="button" class="btnSF1dDespuesFechaFin" value=">" style="width: 27px; text-align: center;" title="1 día después" />
                </td>
            </tr>
        </table>    
    </div>

    <div style="clear:both; text-align:center; margin:auto; margin-top:20px">
        <input type="button" value="Seleccionar" class="btnSelectDatePopup" />
        <input type="button" value="Cancelar" class="btnCancelarSelectDatePopup" />

        <input type="hidden" value="" class="selectDate_fecha_inicio_Main" />
        <input type="hidden" value="" class="selectDate_fecha_fin_Main" />
    </div>
    `;
}

//funciones auxiliares
function selectDate_GetValorFechaValido(idInputFecha) {
    var valorInputFecha = $(idInputFecha).val();
    var valorFecha = selectDate_validarFecha(valorInputFecha) ? valorInputFecha : selectDate_obtenerFechaHoy();
    return valorFecha;
}

function selectDate_SetValorFechaFromDate(idFechaIni, date1, idFechaFin, date2) {
    $(idFechaIni).val(selectDate_obtenerFechaByDate(date1));
    $(idFechaFin).val(selectDate_obtenerFechaByDate(date2));
}

function selectDate_GetInicioSemana(currentDate) {
    var curr = new Date(currentDate);
    if (curr.getDay() != 6) {// si no es sabado
        var first = curr.getDate() - curr.getDay() - 1; // First day is the day of the month - the day of the week
        var last = first + 6; // last day is the first day + 6

        return new Date(curr.setDate(first));
    }

    return currentDate;
}

function selectDate_GetInicioMes(n, objDate) {
    /*var current;
    if (objDate.getMonth() == 11) {
        current = new Date(objDate.getFullYear() + 1, 0, 1);
    } else {
        if (numMeses == 0) {
            current = new Date(objDate.getFullYear(), objDate.getMonth(), 1);
        }
        else {
            current = new Date(objDate.getFullYear(), objDate.getMonth() + 1, 1);
        }
    }
    return (numMeses <= 1) ? current : selectDate_GetInicioMes(numMeses - 1, new Date(objDate.getFullYear(), objDate.getMonth() + 1, 1))*/
    /*var dt = new Date(objDate);
    if (dt.getDay() == 1 && 0) {
        return dt;
    }*/

    var dt = new Date(objDate);
    var current = new Date(dt.setMonth(dt.getMonth() + n));
    current.setDate(1);

    return current;
}