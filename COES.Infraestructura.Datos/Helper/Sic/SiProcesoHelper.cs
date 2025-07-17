using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_PROCESO
    /// </summary>
    public class SiProcesoHelper : HelperBase
    {
        public SiProcesoHelper(): base(Consultas.SiProcesoSql)
        {
        }

        public SiProcesoDTO Create(IDataReader dr)
        {
            SiProcesoDTO entity = new SiProcesoDTO();

            int iPrcscodi = dr.GetOrdinal(this.Prcscodi);
            if (!dr.IsDBNull(iPrcscodi)) entity.Prcscodi = Convert.ToInt32(dr.GetValue(iPrcscodi));

            int iPrcsnomb = dr.GetOrdinal(this.Prcsnomb);
            if (!dr.IsDBNull(iPrcsnomb)) entity.Prcsnomb = dr.GetString(iPrcsnomb);

            int iPrcsestado = dr.GetOrdinal(this.Prcsestado);
            if (!dr.IsDBNull(iPrcsestado)) entity.Prcsestado = dr.GetString(iPrcsestado);

            int iPrcsmetodo = dr.GetOrdinal(this.Prcsmetodo);
            if (!dr.IsDBNull(iPrcsmetodo)) entity.Prcsmetodo = dr.GetString(iPrcsmetodo);

            int iPrscfrecuencia = dr.GetOrdinal(this.Prscfrecuencia);
            if (!dr.IsDBNull(iPrscfrecuencia)) entity.Prscfrecuencia = dr.GetString(iPrscfrecuencia);

            int iPrschorainicio = dr.GetOrdinal(this.Prschorainicio);
            if (!dr.IsDBNull(iPrschorainicio)) entity.Prschorainicio = Convert.ToInt32(dr.GetValue(iPrschorainicio));

            int iPrscminutoinicio = dr.GetOrdinal(this.Prscminutoinicio);
            if (!dr.IsDBNull(iPrscminutoinicio)) entity.Prscminutoinicio = Convert.ToInt32(dr.GetValue(iPrscminutoinicio));

            int iModcodi = dr.GetOrdinal(this.Modcodi);
            if (!dr.IsDBNull(iModcodi)) entity.Modcodi = Convert.ToInt32(dr.GetValue(iModcodi));

            int iPrscbloque = dr.GetOrdinal(this.Prscbloque);
            if (!dr.IsDBNull(iPrscbloque)) entity.Prscbloque = Convert.ToInt32(dr.GetValue(iPrscbloque));

            return entity;
        }


        #region Mapeo de Campos

        public string Prcscodi = "PRCSCODI";
        public string Prcsnomb = "PRCSNOMB";
        public string Prcsestado = "PRCSESTADO";
        public string Prcsmetodo = "PRCSMETODO";
        public string Prscfrecuencia = "PRSCFRECUENCIA";
        public string Prschorainicio = "PRSCHORAINICIO";
        public string Prscminutoinicio = "PRSCMINUTOINICIO";
        public string Modcodi = "MODCODI";
        public string Prscbloque = "PRSCBLOQUE";
        public string PathFile = "PATHFILE";
        public string pGPS = "pGPS";
        public string pFecha = "pFecha";

        public string SqlObtenerParametros
        {
            get { return base.GetSqlXml("ObtenerParametros"); }
        }

        public string SqlActualizarEstado
        {
            get { return base.GetSqlXml("ActualizarEstado"); }
        }

        #endregion
    }
}
