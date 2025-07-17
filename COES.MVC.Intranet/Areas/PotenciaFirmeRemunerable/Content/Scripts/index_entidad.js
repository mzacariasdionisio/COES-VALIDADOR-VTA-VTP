var TIPO_BARRA = 1;
var TIPO_LINEA = 2;
var TIPO_TRAFO2 = 3;
var TIPO_TRAFO3 = 4;
var TIPO_COMPDINAMICA = 5;
var TIPO_GAMS_VTP = 6;
var TIPO_GAMS_SSAA = 7;
var TIPO_GAMS_EQUIPOS = 8;
var TIPO_CONGESTION = 9;
var TIPO_PENALIDAD = 10;


const CONCEPTO = Object.freeze(
    {
        VIGENCIA: 1,
        TENSION: 2,
        VMAX: 3,
        VMIN: 4,
        COMPREACTIVA: 5,
        RESISTENCIA: 6,
        REACTANCIA: 7,
        CONDUCTANCIA: 8,
        ADMITANCIA: 9,
        POTENCIAMAXIMA: 10,
        TAP1: 11,
        TAP2: 12,
        QMAX: 13,
        QMIN: 14,
        NUMUNIDAD: 15,
        REF: 16,
        PMAX: 17,
        PMIN: 18,
        LINEA1: 19,
        LINEA2: 20,
        LINEA3: 21,
        LINEA4: 22,
        LINEA5: 23,
        LINEA6: 24,
        LINEA7: 25,
        LINEA8: 26,
        LINEA9: 27,
        LINEA10: 28,
        LINEA11: 29,
        LINEA12: 30,
        PENALIDAD: 31,
        DESCRIPCION: 32
    }
);


const DELETED = Object.freeze(
    {
        ACTIVO: 0
    }
);


function AgregarEntidadDat(dataForm) {

    dataForm.ListEntidadDat = [];
    var obj = { Prfdatfechavigdesc: dataForm.Pfrentvigenciaini, Pfrdatdeleted: DELETED.ACTIVO };

    if (dataForm.Pfrentvigenciaini)
        dataForm.ListEntidadDat.push({ Pfrcnpcodi: CONCEPTO.VIGENCIA, Pfrdatvalor: "S", ...obj });

    switch (dataForm.Pfrcatcodi) {
        case TIPO_BARRA:

            if (dataForm.Pfreqptension) dataForm.ListEntidadDat.push({ Pfrcnpcodi: CONCEPTO.TENSION, Pfrdatvalor: dataForm.Pfreqptension, ...obj });
            if (dataForm.Pfreqpvmax) dataForm.ListEntidadDat.push({ Pfrcnpcodi: CONCEPTO.VMAX, Pfrdatvalor: dataForm.Pfreqpvmax, ...obj });
            if (dataForm.Pfreqpvmin) dataForm.ListEntidadDat.push({ Pfrcnpcodi: CONCEPTO.VMIN, Pfrdatvalor: dataForm.Pfreqpvmin, ...obj });
            if (dataForm.Pfreqpvmin) dataForm.ListEntidadDat.push({ Pfrcnpcodi: CONCEPTO.COMPREACTIVA, Pfrdatvalor: dataForm.Pfreqpcompreactiva, ...obj });

            break;

        case TIPO_LINEA:

            if (dataForm.Pfreqpresistencia) dataForm.ListEntidadDat.push({ Pfrcnpcodi: CONCEPTO.RESISTENCIA, Pfrdatvalor: dataForm.Pfreqpresistencia, ...obj });
            if (dataForm.Pfreqpreactancia) dataForm.ListEntidadDat.push({ Pfrcnpcodi: CONCEPTO.REACTANCIA, Pfrdatvalor: dataForm.Pfreqpreactancia, ...obj });
            if (dataForm.Pfreqpconductancia) dataForm.ListEntidadDat.push({ Pfrcnpcodi: CONCEPTO.CONDUCTANCIA, Pfrdatvalor: dataForm.Pfreqpconductancia, ...obj });
            if (dataForm.Pfreqpadmitancia) dataForm.ListEntidadDat.push({ Pfrcnpcodi: CONCEPTO.ADMITANCIA, Pfrdatvalor: dataForm.Pfreqpadmitancia, ...obj });
            if (dataForm.Pfreqppotenciamax) dataForm.ListEntidadDat.push({ Pfrcnpcodi: CONCEPTO.POTENCIAMAXIMA, Pfrdatvalor: dataForm.Pfreqppotenciamax, ...obj });

            break;

        case TIPO_TRAFO2:
        case TIPO_TRAFO3:

            if (dataForm.Pfreqpresistencia) dataForm.ListEntidadDat.push({ Pfrcnpcodi: CONCEPTO.RESISTENCIA, Pfrdatvalor: dataForm.Pfreqpresistencia, ...obj });
            if (dataForm.Pfreqpreactancia) dataForm.ListEntidadDat.push({ Pfrcnpcodi: CONCEPTO.REACTANCIA, Pfrdatvalor: dataForm.Pfreqpreactancia, ...obj });
            if (dataForm.Pfreqpconductancia) dataForm.ListEntidadDat.push({ Pfrcnpcodi: CONCEPTO.CONDUCTANCIA, Pfrdatvalor: dataForm.Pfreqpconductancia, ...obj });
            if (dataForm.Pfreqpadmitancia) dataForm.ListEntidadDat.push({ Pfrcnpcodi: CONCEPTO.ADMITANCIA, Pfrdatvalor: dataForm.Pfreqpadmitancia, ...obj });
            if (dataForm.Pfreqppotenciamax) dataForm.ListEntidadDat.push({ Pfrcnpcodi: CONCEPTO.POTENCIAMAXIMA, Pfrdatvalor: dataForm.Pfreqppotenciamax, ...obj });

            if (dataForm.Pfreqptap1) dataForm.ListEntidadDat.push({ Pfrcnpcodi: CONCEPTO.TAP1, Pfrdatvalor: dataForm.Pfreqptap1, ...obj });
            if (dataForm.Pfreqptap2) dataForm.ListEntidadDat.push({ Pfrcnpcodi: CONCEPTO.TAP2, Pfrdatvalor: dataForm.Pfreqptap2, ...obj });

            break;

        case TIPO_COMPDINAMICA:

            if (dataForm.Pfreqpqmax) dataForm.ListEntidadDat.push({ Pfrcnpcodi: CONCEPTO.QMAX, Pfrdatvalor: dataForm.Pfreqpqmax, ...obj });
            if (dataForm.Pfreqpqmin) dataForm.ListEntidadDat.push({ Pfrcnpcodi: CONCEPTO.QMIN, Pfrdatvalor: dataForm.Pfreqpqmin, ...obj });

            break;

        case TIPO_GAMS_EQUIPOS:

            if (dataForm.Pfrrgeunidad) dataForm.ListEntidadDat.push({ Pfrcnpcodi: CONCEPTO.NUMUNIDAD, Pfrdatvalor: dataForm.Pfrrgeunidad, ...obj });
            if (dataForm.Pfrrgeqmax) dataForm.ListEntidadDat.push({ Pfrcnpcodi: CONCEPTO.QMAX, Pfrdatvalor: dataForm.Pfrrgeqmax, ...obj });
            if (dataForm.Pfrrgeqmin) dataForm.ListEntidadDat.push({ Pfrcnpcodi: CONCEPTO.QMIN, Pfrdatvalor: dataForm.Pfrrgeqmin, ...obj });
            if (dataForm.Pfrrgeref) dataForm.ListEntidadDat.push({ Pfrcnpcodi: CONCEPTO.REF, Pfrdatvalor: dataForm.Pfrrgeref, ...obj });

            break;

        case TIPO_CONGESTION:

            if (dataForm.Pfrcgtpotenciamax) dataForm.ListEntidadDat.push({ Pfrcnpcodi: CONCEPTO.PMAX, Pfrdatvalor: dataForm.Pfrcgtpotenciamax, ...obj });
            if (dataForm.Pfrcgtpotenciamin) dataForm.ListEntidadDat.push({ Pfrcnpcodi: CONCEPTO.PMIN, Pfrdatvalor: dataForm.Pfrcgtpotenciamin, ...obj });
            if (dataForm.Pfrcgtidlinea1) dataForm.ListEntidadDat.push({ Pfrcnpcodi: CONCEPTO.LINEA1, Pfrdatvalor: dataForm.Pfrcgtidlinea1, ...obj });
            if (dataForm.Pfrcgtidlinea2) dataForm.ListEntidadDat.push({ Pfrcnpcodi: CONCEPTO.LINEA2, Pfrdatvalor: dataForm.Pfrcgtidlinea2, ...obj });
            if (dataForm.Pfrcgtidlinea3) dataForm.ListEntidadDat.push({ Pfrcnpcodi: CONCEPTO.LINEA3, Pfrdatvalor: dataForm.Pfrcgtidlinea3, ...obj });
            if (dataForm.Pfrcgtidlinea4) dataForm.ListEntidadDat.push({ Pfrcnpcodi: CONCEPTO.LINEA4, Pfrdatvalor: dataForm.Pfrcgtidlinea4, ...obj });
            if (dataForm.Pfrcgtidlinea5) dataForm.ListEntidadDat.push({ Pfrcnpcodi: CONCEPTO.LINEA5, Pfrdatvalor: dataForm.Pfrcgtidlinea5, ...obj });
            if (dataForm.Pfrcgtidlinea6) dataForm.ListEntidadDat.push({ Pfrcnpcodi: CONCEPTO.LINEA6, Pfrdatvalor: dataForm.Pfrcgtidlinea6, ...obj });
            if (dataForm.Pfrcgtidlinea7) dataForm.ListEntidadDat.push({ Pfrcnpcodi: CONCEPTO.LINEA7, Pfrdatvalor: dataForm.Pfrcgtidlinea7, ...obj });
            if (dataForm.Pfrcgtidlinea8) dataForm.ListEntidadDat.push({ Pfrcnpcodi: CONCEPTO.LINEA8, Pfrdatvalor: dataForm.Pfrcgtidlinea8, ...obj });
            if (dataForm.Pfrcgtidlinea9) dataForm.ListEntidadDat.push({ Pfrcnpcodi: CONCEPTO.LINEA9, Pfrdatvalor: dataForm.Pfrcgtidlinea9, ...obj });
            if (dataForm.Pfrcgtidlinea10) dataForm.ListEntidadDat.push({ Pfrcnpcodi: CONCEPTO.LINEA10, Pfrdatvalor: dataForm.Pfrcgtidlinea10, ...obj });
            if (dataForm.Pfrcgtidlinea11) dataForm.ListEntidadDat.push({ Pfrcnpcodi: CONCEPTO.LINEA11, Pfrdatvalor: dataForm.Pfrcgtidlinea11, ...obj });
            if (dataForm.Pfrcgtidlinea12) dataForm.ListEntidadDat.push({ Pfrcnpcodi: CONCEPTO.LINEA12, Pfrdatvalor: dataForm.Pfrcgtidlinea13, ...obj });

            break;

        case TIPO_PENALIDAD:

            if (dataForm.Pfrpenvalor) dataForm.ListEntidadDat.push({ Pfrcnpcodi: CONCEPTO.PENALIDAD, Pfrdatvalor: dataForm.Pfrpenvalor, ...obj });
            if (dataForm.Pfrpendescripcion) dataForm.ListEntidadDat.push({ Pfrcnpcodi: CONCEPTO.DESCRIPCION, Pfrdatvalor: dataForm.Pfrpendescripcion, ...obj });

            break;
    }
}

function validarFormularioEquipo(objEquipo, tipoEquipo) {
    var msj = "";

    var validarId =
        validarNombre =
        validarVigenciaIni =
        validarVigenciaFin =
        validarTension =
        validarVmax =
        validarVmin =
        validarQmax =
        validarQmin =
        validarCompReactiva =
        validarIdBarra =
        validarIdBarra1 =
        validarIdBarra2 =
        validarResistencia =
        validarReactancia =
        validarConductancia =
        validarAdmitancia =
        validarTap1 =
        validarTap2 =
        validarPotenciaMaxima =
        validarTrnBarra =
        validarNUnidad = false;

    switch (tipoEquipo) {
        case TIPO_BARRA:
            validarVigenciaIni = true;
            validarId = true;
            validarNombre = true;

            validarTension = Boolean(objEquipo.Pfreqptension);
            validarVmax = Boolean(objEquipo.Pfreqpvmax);
            validarVmin = Boolean(objEquipo.Pfreqpvmin);
            validarCompReactiva = Boolean(objEquipo.Pfreqpcompreactiva)
            break;

        case TIPO_LINEA:
            validarVigenciaIni = true;
            validarId = true;
            validarIdBarra1 = true;
            validarIdBarra2 = true;

            validarResistencia = Boolean(objEquipo.Pfreqpresistencia);
            validarReactancia = Boolean(objEquipo.Pfreqpreactancia);
            validarConductancia = Boolean(objEquipo.Pfreqpconductancia);
            validarAdmitancia = Boolean(objEquipo.Pfreqpadmitancia);
            validarPotenciaMaxima = Boolean(objEquipo.Pfreqppotenciamax);
            break;

        case TIPO_TRAFO2:
        case TIPO_TRAFO3:
            validarVigenciaIni = true;
            validarId = true;
            validarIdBarra1 = true;
            validarIdBarra2 = true;
            validarResistencia = Boolean(objEquipo.Pfreqpresistencia);
            validarReactancia = Boolean(objEquipo.Pfreqpreactancia);
            validarConductancia = Boolean(objEquipo.Pfreqpconductancia);
            validarAdmitancia = Boolean(objEquipo.Pfreqpadmitancia);
            validarTap1 = Boolean(objEquipo.Pfreqptap1);
            validarTap2 = Boolean(objEquipo.Pfreqptap2);
            validarPotenciaMaxima = Boolean(objEquipo.Pfreqppotenciamax);
            break;

        case TIPO_COMPDINAMICA:
            validarVigenciaIni = true;
            validarId = true;
            validarIdBarra = true;
            validarQmax = Boolean(objEquipo.Pfreqpqmax);
            validarQmin = Boolean(objEquipo.Pfreqpqmin);
            break;

        case TIPO_COMPDINAMICA:
            validarQmax = Boolean(objEquipo.Pfreqpqmax);
            validarQmax = Boolean(objEquipo.Pfreqpqmax);
            validarNUnidad = Boolean(objEquipo.Pfrrgeunidad);
            break;
    }

    if (validarVigenciaIni) {
        if (objEquipo.Pfrentvigenciaini == null || objEquipo.Pfrentvigenciaini == '') {
            msj += "Debe seleccionar un Periodo" + "\n";
        }
    }

    if (validarId) {
        if (objEquipo.Pfrentid == null || objEquipo.Pfrentid == '') {
            msj += "Debe ingresar ID" + "\n";
        }
    }

    if (validarNombre) {
        if (objEquipo.Pfrentnomb == null || objEquipo.Pfrentnomb == '') {
            msj += "Debe ingresar Nombre" + "\n";
        }
    }

    if (validarTension) {
        if (isNaN(objEquipo.Pfreqptension)) {
            msj += "Dato Tensión incorrecto, debe ser de tipo numérico" + "\n";
        }
    }

    if (validarVmax) {
        if (isNaN(objEquipo.Pfreqpvmax)) {
            msj += "Dato VMáx incorrecto, debe ser de tipo numérico" + "\n";
        }

        if (+objEquipo.Pfreqpvmax < +objEquipo.Pfreqpvmin) {
            msj += "Dato VMáx incorrecto, debe ser mayor que VMín" + "\n";
        }
    }

    if (validarVmin) {
        if (isNaN(objEquipo.Pfreqpvmin)) {
            msj += "Dato VMín incorrecto, debe ser de tipo numérico" + "\n";
        }
        if (+objEquipo.Pfreqpvmax < +objEquipo.Pfreqpvmin) {
            msj += "Dato VMín incorrecto, debe ser menor que VMáx" + "\n";
        }
    }

    if (validarQmax) {
        if (isNaN(objEquipo.Pfreqpqmax)) {
            msj += "Dato QMáx incorrecto, debe ser de tipo numérico" + "\n";
        }
        if (+objEquipo.Pfreqpqmax < +objEquipo.Pfreqpqmin) {
            msj += "Dato QMáx incorrecto, debe ser mayor que QMín" + "\n";
        }
    }

    if (validarQmin) {
        if (isNaN(objEquipo.Pfreqpqmin)) {
            msj += "Dato QMáx incorrecto, debe ser de tipo numérico" + "\n";
        }
        if (+objEquipo.Pfreqpqmax < +objEquipo.Pfreqpqmin) {
            msj += "Dato QMín incorrecto, debe ser menor que QMáx" + "\n";
        }
    }

    if (validarNUnidad) {
        if (isNaN(objEquipo.Pfreqpqmin)) {
            msj += "Dato Num. Unidad incorrecto, debe ser de tipo numérico" + "\n";
        }
    }

    if (validarCompReactiva) {
        if (isNaN(objEquipo.Pfreqpcompreactiva)) {
            msj += "Dato Compensación Reactiva incorrecta, debe ser de tipo numérico" + "\n";
        }
    }

    if (validarIdBarra) {
        if (objEquipo.Pfrentcodibarragams == null || objEquipo.Pfrentcodibarragams == '') {
            msj += "Debe escoger una Barra" + "\n";
        } else {

        }
    }

    if (validarIdBarra1) {
        if (objEquipo.Pfrentcodibarragams == null || objEquipo.Pfrentcodibarragams == '') {
            msj += "Debe escoger una Barra1" + "\n";
        } else {
            if (objEquipo.Pfrentcodibarragams2 == null || objEquipo.Pfrentcodibarragams2 == '') {

            } else {
                if (objEquipo.Pfrentcodibarragams == objEquipo.Pfrentcodibarragams2) {
                    msj += "Barra1 incorrecto, Barra1 y Barra2 deben ser diferentes" + "\n";
                }
            }
        }
    }

    if (validarIdBarra2) {
        if (objEquipo.Pfrentcodibarragams2 == null || objEquipo.Pfrentcodibarragams2 == '') {
            msj += "Debe escoger una Barra2" + "\n";
        } else {
            if (objEquipo.Pfrentcodibarragams == null || objEquipo.Pfrentcodibarragams == '') {

            } else {
                if (objEquipo.Pfrentcodibarragams2 == objEquipo.Pfrentcodibarragams) {
                    msj += "Barra2 incorrecto, Barra1 y Barra2 deben ser diferentes" + "\n";
                }
            }
        }
    }

    if (validarResistencia) {
        if (isNaN(objEquipo.Pfreqpresistencia)) {
            msj += "Dato Resistencia incorrecta, debe ser de tipo numérico" + "\n";
        }
    }

    if (validarReactancia) {
        if (isNaN(objEquipo.Pfreqpreactancia)) {
            msj += "Dato Reactancia incorrecta, debe ser de tipo numérico" + "\n";
        }
    }

    if (validarConductancia) {
        if (isNaN(objEquipo.Pfreqpconductancia)) {
            msj += "Dato Conductancia incorrecta, debe ser de tipo numérico" + "\n";
        }
    }

    if (validarAdmitancia) {
        if (isNaN(objEquipo.Pfreqpadmitancia)) {
            msj += "Dato Admitancia incorrecta, debe ser de tipo numérico" + "\n";
        }
    }

    if (validarTap1) {
        if (isNaN(objEquipo.Pfreqptap1)) {
            msj += "Dato Tap1 incorrecto, debe ser de tipo numérico" + "\n";
        }
    }

    if (validarTap2) {
        if (isNaN(objEquipo.Pfreqptap2)) {
            msj += "Dato Tap2 incorrecto, debe ser de tipo numérico" + "\n";
        }
    }

    if (validarPotenciaMaxima) {
        if (isNaN(objEquipo.Pfreqppotenciamax)) {
            msj += "Dato Potencia Máxima incorrecta, debe ser de tipo numérico" + "\n";
        }
    }


    return msj;
}