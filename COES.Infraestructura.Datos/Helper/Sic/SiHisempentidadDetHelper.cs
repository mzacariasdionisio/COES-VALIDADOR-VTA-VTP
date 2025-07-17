using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_HISEMPENTIDAD_DET
    /// </summary>
    public class SiHisempentidadDetHelper : HelperBase
    {
        public SiHisempentidadDetHelper() : base(Consultas.SiHisempentidadDetSql)
        {
        }

        public SiHisempentidadDetDTO Create(IDataReader dr)
        {
            SiHisempentidadDetDTO entity = new SiHisempentidadDetDTO();

            int iHempedcodi = dr.GetOrdinal(this.Hempedcodi);
            if (!dr.IsDBNull(iHempedcodi)) entity.Hempedcodi = Convert.ToInt32(dr.GetValue(iHempedcodi));

            int iHempencodi = dr.GetOrdinal(this.Hempencodi);
            if (!dr.IsDBNull(iHempencodi)) entity.Hempencodi = Convert.ToInt32(dr.GetValue(iHempencodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iHempedfecha = dr.GetOrdinal(this.Hempedfecha);
            if (!dr.IsDBNull(iHempedfecha)) entity.Hempedfecha = dr.GetDateTime(iHempedfecha);

            int iHempedvalorid = dr.GetOrdinal(this.Hempedvalorid);
            if (!dr.IsDBNull(iHempedvalorid)) entity.Hempedvalorid = Convert.ToInt32(dr.GetValue(iHempedvalorid));

            int iHempedvalorestado = dr.GetOrdinal(this.Hempedvalorestado);
            if (!dr.IsDBNull(iHempedvalorestado)) entity.Hempedvalorestado = dr.GetString(iHempedvalorestado);

            return entity;
        }

        #region Mapeo de Campos

        public string Hempedcodi = "HEMPEDCODI";
        public string Hempencodi = "HEMPENCODI";
        public string Emprcodi = "EMPRCODI";
        public string Hempedfecha = "HEMPEDFECHA";
        public string Hempedvalorid = "HEMPEDVALORID";
        public string Hempedvalorestado = "HEMPEDVALORESTADO";

        public string EmprnombOrigen = "EmprnombOrigen";
        public string Nombre = "Nombre";
        public string Nombre2 = "Nombre2";
        public string EstadoActual = "EstadoActual";

        #endregion

        public string SqlGetByCriteriaXTabla
        {
            get { return base.GetSqlXml("GetByCriteriaXTabla"); }
        }
    }
}
