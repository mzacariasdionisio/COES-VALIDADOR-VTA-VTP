using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.Scada.Helper
{
    /// <summary>
    /// Constantes utilizadas en el módulo de fórmulas scada
    /// </summary>
    public class ConstanteFormulaScada
    {       
        public const char SeparadorFormula = '+';
        public const string CaracterComa = ",";
        public const string SI = "S";
        public const string NO = "N";
        public const string OrigenScada = "A";
        public const string OrigenEjecutado = "B";
        public const string OrigenDemandaDiaria = "C";
        public const string OrigenDemandaMensual = "D";
        public const string OrigenMedidoresGeneracion = "M";
        public const string OrigenScadaSP7 = "S";
        public const string OrigenPR16 = "U";
        public const string TextoSCADA = "SCADA";
        public const string TextoEjecutado = "Despacho ejecutado diario.";
        public const string TextoDemandaDiaria = "Histórico (Demanda barra - diario)";
        public const string TextoDemandaMensual = "Medidores (Demanda en barras)";
        public const string TextoMedidoresGeneracion = "Medidores de Generación";
        public const string TextoScadaSP7 = "SCADA SP7";
        public const string TextoPR16 = "Demanda UL y D (PR-16)";

        #region PR5
        public const string TextoPR5 = "PR5";
        public const int OriglectcodiPR5 = 21;
        public const string OrigenPR5 = "E";
        public const int TipoNodefinido = -1;
        public const int TipoAreaOperativa = 1;
        public const int TipoSubAreaOperativa = 2;

        public const string AreaOperativaAbrevNorte = "NORTE";
        public const string AreaOperativaAbrevCentro = "CENTRO";
        public const string AreaOperativaAbrevSur = "SUR";
        public const string AreaOperativaAbrevNorteMedio = "NORTE_MEDIO";
        public const string AreaOperativaAbrevLima = "LIMA";
        public const string AreaOperativaAbrevCentroNorte = "CENNOR";
        public const string AreaOperativaAbrevCentroSur = "CENSUR";
        public const string AreaOperativaAbrevElectroandes = "ELA";
        public const string AreaOperativaAbrevSurMedio = "SUR_MEDIO";
        public const string AreaOperativaAbrevSurEste = "SUR_ESTE";
        public const string AreaOperativaAbrevSurOeste = "SUR_OESTE";
        public const string AreaOperativaAbrevArequipa = "AREQUIPA";

        public const string AreaOperativaNombreNorte = "NORTE";
        public const string AreaOperativaNombreCentro = "CENTRO";
        public const string AreaOperativaNombreSur = "SUR";
        public const string AreaOperativaNombreNorteMedio = "NORTE MEDIO";
        public const string AreaOperativaNombreLima = "LIMA";
        public const string AreaOperativaNombreCentroNorte = "CENTRO - NORTE";
        public const string AreaOperativaNombreCentroSur = "CENTRO - SUR";
        public const string AreaOperativaNombreElectroandes = "ELA";
        public const string AreaOperativaNombreSurMedio = "SUR MEDIO";
        public const string AreaOperativaNombreSurEste = "SUR ESTE";
        public const string AreaOperativaNombreSurOeste = "SUR OESTE";
        public const string AreaOperativaNombreArequipa = "AREQUIPA";

        public const string FormulaAreaOperativaNorte = "PRUEBA DTI final";
        public const string FormulaAreaOperativaCentro = "PRUEBA DTI final";
        public const string FormulaAreaOperativaSur = "PRUEBA DTI final";
        public const string FormulaAreaOperativaNorteMedio = "PRUEBA DTI final";
        public const string FormulaAreaOperativaLima = "PRUEBA DTI final";
        public const string FormulaAreaOperativaElectroandes = "PRUEBA DTI final";
        public const string FormulaAreaOperativaSurMedio = "PRUEBA DTI final";
        public const string FormulaAreaOperativaSurEste = "PRUEBA DTI final";
        public const string FormulaAreaOperativaSurOeste = "PRUEBA DTI final";
        public const string FormulaAreaOperativaArequipa = "PRUEBA DTI final";
        #endregion
    }
}
