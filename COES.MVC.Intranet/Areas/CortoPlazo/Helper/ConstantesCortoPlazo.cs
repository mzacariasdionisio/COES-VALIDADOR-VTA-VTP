using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.CortoPlazo.Helper
{
    public class ConstantesCortoPlazo
    {
        public const string EstadoActivo = "ACTIVO";
        public const string RutaCarga = "Areas\\CortoPlazo\\Documentos\\";
        public const string ArchivoResultado = "{0}.gen";        
        // se puede unificar con HelperCortoPlazo
        public const int IdLineaTransmision = 8;
        public const int IdTrafo2D = 9;
        public const int IdTrafo3D = 10;
        public const int IdGeneradorHidro = 2;
        public const int IdCentralHidro = 4;
        public const int IdGeneradorTermico = 3;
        public const int IdCentralTermico = 5;
        public const int IdFuenteGrupoLinea = 1;
        public const int IdFuenteLinea = 2;
        public const int IdFuenteTrafo2D = 3;
        public const int IdFuenteTrafo3D = 4;
        public const string TxtFuenteGrupoLinea = "Grupo de Líneas";
        public const string TxtFuenteGrupoLineaMinimo = "Grupo de Líneas de Flujo Mínimo";
        public const string TxtFuenteLinea = "Línea";
        public const string TxtFuenteTrafo2D = "Trafo 2D";
        public const string TxtFuenteTrafo3D = "Trafo 3D";
        public const int ConstanteDesfaseGrupoLinea = 1000000;
        public const int TipoEmpresaGeneracion = 3;
        public const int PropcodiCapacidadRegulacion = 1830;
        public const string CongestionSimple = "S";
        public const string CongestionCompuesta = "C";
        public const string ReporteResultado = "ResultadoCMgN.xlsx";
        public const string ReporteResultadoMasivo = "ResultadosCMgN.xlsx";
        public const string ReporteResultadoMasivoPublicado = "ResultadosCMPublicado.xlsx";
        public const int HidroVelocidadTomaCarga = 1835;
        public const int HidroVelocidadReduccionCarga = 1836;
        public const int TermoVelocidadTomaCarga = 504;
        public const int TermoVelocidadReduccionCarga = 505;
        public const string UltimoPeriodo = "23:59";
        public const string PrimerPeriodo = "00:00";
        public const string ReporteConsultaDatos = "ConsultaDatosCP.xlsx";

        public const string TipoMDCOES = "Y";
        public const string TipoNCP = "N";
        public const string EstimadorTNA = "T";
        public const string EstimadorPSS = "P";

        #region Regiones_seguridad

        public const int ConstanteDesfaseRegionSeguridad = 5000000;
        public const string CongestionRegionSeguridad = "R";
        public const string TxtFuenteRegionSeguridad = "Región de Seguridad";

        #endregion

        #region CMgCP_PR07
        public const int DesfaceGrupoLineaFlujoMuinimo = 6000000;
        public const string FechaVigenciaPR07 = "FechaVigenciaPR07";
        #endregion

        #region Informes SGI
        public const int IdFamiliaGrupo = 52;
        #endregion

    }
}