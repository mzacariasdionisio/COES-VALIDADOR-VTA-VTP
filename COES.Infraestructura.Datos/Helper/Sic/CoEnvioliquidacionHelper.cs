using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CO_ENVIOLIQUIDACION
    /// </summary>
    public class CoEnvioliquidacionHelper : HelperBase
    {
        public CoEnvioliquidacionHelper(): base(Consultas.CoEnvioliquidacionSql)
        {
        }

        public CoEnvioliquidacionDTO Create(IDataReader dr)
        {
            CoEnvioliquidacionDTO entity = new CoEnvioliquidacionDTO();

            int iCoenlicodi = dr.GetOrdinal(this.Coenlicodi);
            if (!dr.IsDBNull(iCoenlicodi)) entity.Coenlicodi = Convert.ToInt32(dr.GetValue(iCoenlicodi));

            int iCoenlifecha = dr.GetOrdinal(this.Coenlifecha);
            if (!dr.IsDBNull(iCoenlifecha)) entity.Coenlifecha = dr.GetDateTime(iCoenlifecha);

            int iCoenliusuario = dr.GetOrdinal(this.Coenliusuario);
            if (!dr.IsDBNull(iCoenliusuario)) entity.Coenliusuario = dr.GetString(iCoenliusuario);

            int iPericodi = dr.GetOrdinal(this.Pericodi);
            if (!dr.IsDBNull(iPericodi)) entity.Pericodi = Convert.ToInt32(dr.GetValue(iPericodi));

            int iVcrecacodi = dr.GetOrdinal(this.Vcrecacodi);
            if (!dr.IsDBNull(iVcrecacodi)) entity.Vcrecacodi = Convert.ToInt32(dr.GetValue(iVcrecacodi));

            int iCovercodi = dr.GetOrdinal(this.Covercodi);
            if (!dr.IsDBNull(iCovercodi)) entity.Covercodi = Convert.ToInt32(dr.GetValue(iCovercodi));

            int iCopercodi = dr.GetOrdinal(this.Copercodi);
            if (!dr.IsDBNull(iCopercodi)) entity.Copercodi = Convert.ToInt32(dr.GetValue(iCopercodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Coenlicodi = "COENLICODI";
        public string Coenlifecha = "COENLIFECHA";
        public string Coenliusuario = "COENLIUSUARIO";
        public string Pericodi = "PERICODI";
        public string Vcrecacodi = "VCRECACODI";
        public string Covercodi = "COVERCODI";
        public string Copercodi = "COPERCODI";
        public string Periodonomb = "PERIODONOMB";
        public string Versionnomb = "VERSIONNOMB";
        public string Periododesc = "PERIODODESC";
        public string Versiondesc = "VERSIONDESC";

        public string SqlObtenerEnviosPorPeriodo
        {
            get { return base.GetSqlXml("ObtenerEnviosPorPeriodo"); }
        }

        #endregion
    }
}
