using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class DpoProcesoPronosticoHelper : HelperBase
    {
        public DpoProcesoPronosticoHelper() : base(Consultas.DpoProcesoPronosticoSql)
        {
        }

        #region Mapeo de Campos
        public string Ptomedicodi = "PTOMEDICODI";
        public string Medifecha = "MEDIFECHA";

        public string Ptomedibarranomb = "PTOMEDIBARRANOMB";
        public string Nombre_P = "NOMBRE_P";
        public string Formula_P = "FORMULA_P";
        public string Nombre_S = "NOMBRE_S";
        public string Formula_S = "FORMULA_S";

        public string Ptomedicodi_Sco = "PTOMEDICODI_SCO";
        public string Ptomedicodi_Ieod = "PTOMEDICODI_IEOD";

        public string Grupocodi = "GRUPOCODI";
        public string Areacodi = "AREACODI";
        #endregion

        #region Consultas BD
        public string SqlObtenerGeneracionPorFechas
        {
            get { return base.GetSqlXml("ObtenerGeneracionPorFechas"); }
        }
        public string SqlObtenerFormulas
        {
            get { return base.GetSqlXml("ObtenerFormulas"); }
        }
        public string SqlObtenerDemandaPorFechas
        {
            get { return base.GetSqlXml("ObtenerDemandaPorFechas"); }
        }
        public string SqlRelacionScoIeod
        {
            get { return base.GetSqlXml("RelacionScoIeod"); }
        }
        public string SqlListaBarras
        {
            get { return base.GetSqlXml("ListaBarras"); }
        }
        public string SqlObtenerDemandaSRP
        {
            get { return base.GetSqlXml("ObtenerDemandaSRP"); }
        }
        public string SqlObtenerDemandaUltimaHora
        {
            get { return base.GetSqlXml("ObtenerDemandaUltimaHora"); }
        }
        #endregion
    }
}
