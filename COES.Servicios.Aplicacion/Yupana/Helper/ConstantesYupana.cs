using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.Yupana.Helper
{
    public class ConstantesYupana
    {
        public const short Urs = 11;
        public const short UrsSicoes = 12;
        public const short SresReservaUrs = 69;
        public const short SresReservaUrsUp = 106;
        public const short SresReservaUrsDown = 107;
        public const short SresCostoReservaUrsUp = 108;
        public const short SresCostoReservaUrsDown = 109;
        public const short SresCostoDeficitReservaUrsUp = 110;
        public const short SresCostoDeficitReservaUrsDown = 111;
        public const short SresCostoDeficitRegSeguridad = 112;
        public const short SresCostoExcesoReservaUrsDown = 113;


        public const int RestricCostosOperacion = 28;
        public const int CostoTermico = 76;
        public const int CostoArranque = 77;
        public const int CostoHidro = 78;
        public const int CostoUrs = 79;
        public const int CostoRacionamiento = 81;
        public const int CostoDeficitRsf = 97;
        public const int CostoExcesoPotencia = 82;
        public const int CostoVertimientoEmbalse = 83;
        public const int CostoVertimientoCentrales = 84;
        public const int CostoFuturo = 85;
        public const int CostoTotal = 94;
        public const int TipoCambioDolar = 98;

        #region Yupana Continuo 

        public const string SeparadorCampo = ",";

        //Variables Gams
        public const string GamsCostosMarginalesBarra = "PBAL_Acoplado";
        public const string GamsCostoRacionamiento = "CostoRacionamientos";
        public const string GamsCostoOperacion = "Costo";
        public const string GamsConverge = "Status: Normal completion";
        public const string GamsDiverge = "Status: Compilation error";

        //Prefijos
        public const string DefinicionTablaPlantaH = "TABLE   TERMICAS (Ut,*)  Datos de las unidades termicas";
        public const string CmdParameter = "PARAMATER";
        public const string PrefijoPlantaH = "Uh";
        public const string PrefijoPlantaT = "Ut";
        public const string PrefijoLinea = "L";
        public const string PrefijoEmbalse = "Emb";
        public const string PrefijoNodoTopologico = "N";
        public const string PrefijoPlantaRer = "Unc";
        public const string PrefijoPlantaOtros = "Unc";
        public const string PrefijoRSF = "RSF";
        public const string PrefijoURS = "URS";
        public const string PrefijoResGen = "Res";
        public const string PrefijoSumFlujo = "sumf";
        public const string PrefijoGenMeta = "gm";
        public const string PrefijoDispComb = "DC";
        public const string PrefijoCComb = "cc";
        public const string PrefijoRegionSeg = "Reg";
        //Yupana 2022
        public const string PrefijoCaldero = "Cal";

        // SAlida Yupana Continuo
        public const string Diverge = "D";
        public const string Converge = "C";

        //Archivos Csv
        public const string NombArchivoPropiedad = "propiedades.csv";
        public const string NombArchivoSurestric = "subrestricciones.csv";
        public const string NombArchivoSurestricdat = "subrestriccionesdat.csv";
        public const string NombArchivoSurestricdatResultados = "resultadosdat.csv";
        public const string NombArchivoMedicion48 = "datosrestricciones.csv";
        public const string NombArchivoMedicion48Forzadas = "datosrestriccionesForz.csv";
        public const string NombArchivoMedicion48Rer = "datosrestriccionesRer.csv";
        public const string NombArchivoMedicion48Cc = "datosrestriccionesCc.csv";
        public const string NombArchivoMedicion48Sc = "datosrestriccionesSc.csv";
        public const string NombArchivoMedicion48CcSc = "datosrestriccionesCcSc.csv";
        public const string NombArchivoMedicion48RerSc = "datosrestriccionesRerSc.csv";
        public const string NombArchivoMedicion48RerCc = "datosrestriccionesRerCc.csv";
        public const string NombArchivoMedicion48RerCcSc = "datosrestriccionesRerCcSc.csv";
        public const string NombArchivoMedicion48RerForzadas = "datosrestriccionesRerForz.csv";
        public const string NombArchivoMedicion48CcForzadas = "datosrestriccionesCcForz.csv";
        public const string NombArchivoMedicion48ScForzadas = "datosrestriccionesScForz.csv";
        public const string NombArchivoMedicion48RerCcForzadas = "datosrestriccionesRerCcForz.csv";
        public const string NombArchivoMedicion48RerScForzadas = "datosrestriccionesRerScForz.csv";
        public const string NombArchivoMedicion48ForzadasCcSc = "datosrestriccionesForzCcSc.csv";
        public const string NombArchivoMedicion48ForzadasRerCcSc = "datosrestriccionesForzRerCcSc.csv";

        public const string NombArchivoMedicion48Resultados = "datosresultados.csv";
        public const string NombArchivoCostoURS = "costours.csv";
        public const string NombArchivoPlantaH = "plantah.csv";
        public const string NombArchivoPlantaT = "plantat.csv";
        public const string NombArchivoPlantaNoConvO = "plantanco.csv";
        public const string NombArchivoLinea = "linea.csv";
        public const string NombArchivoNodoT = "nodot.csv";
        public const string NombArchivoEmbalse = "embalse.csv";
        public const string NombArchivoUnidadT = "unidadt.csv";
        public const string NombArchivoModoT = "modot.csv";
        public const string NombArchivoTrafo2D = "trafo2D.csv";
        public const string NombArchivoTrafo3D = "trafo3D.csv";
        public const string NombArchivoURS = "urs.csv";
        public const string NombArchivoRsfCsv = "rsf.csv";
        public const string NombArchivoResGenerCsv = "rgeneracion.csv";
        public const string NombArchivoRSFurs = "rsfurs.csv";
        public const string NombArchivoEscenario = "escenario.csv";
        public const string NombArchivoDetEtapa = "detalleetapa.csv";
        public const string NombArchivoRecursoGams = "recgamsid.csv";
        public const string NombArchivoCombustible = "combustibles.csv";
        public const string NombArchivoGrupoRec = "gruporec.csv";
        public const string NombArchivoEquipoNotoT = "equiposnodot.csv";
        public const string NombArchivoRelacion = "relacion.csv";
        public const string NombArchivoGrupoPrioridad = "gprioridad.csv";
        public const string NombArchivoCicloCombinado = "ccombinado.csv";
        public const string NombArchivoDisponComb = "dcombustible.csv";
        public const string NombArchivoSumaFlujosCsv = "sumaflujo.csv";
        public const string NombArchivoGenerMetaCsv = "GenerMeta.csv";
        public const string NombArchivoCategoriaCsv = "Categoria.csv";
        public const string NombArchivoDetfcostofCsv = "detcostof.csv";
        public const string NombArchivoParametroCsv = "param.csv";
        public const string NombArchivofuenteCsv = "fuente.csv";
        public const string NombArchivoGeneralesCsv = "generales.csv";
        public const string NombreArchivoRegionSeguridadCsv = "Region_Seguridad.csv";
        public const string NombArchivoCalderoCsv = "caldero.csv";

        #endregion

    }
}
