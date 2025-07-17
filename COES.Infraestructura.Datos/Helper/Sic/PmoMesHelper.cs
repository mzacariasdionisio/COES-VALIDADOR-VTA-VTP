using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PMO_MES
    /// </summary>
    public class PmoMesHelper : HelperBase
    {
        public PmoMesHelper(): base(Consultas.PmoMesSql)
        {
        }

        public PmoMesDTO Create(IDataReader dr)
        {
            PmoMesDTO entity = new PmoMesDTO();

            int iPmmescodi = dr.GetOrdinal(this.Pmmescodi);
            if (!dr.IsDBNull(iPmmescodi)) entity.Pmmescodi = Convert.ToInt32(dr.GetValue(iPmmescodi));

            int iPmmesaniomes = dr.GetOrdinal(this.Pmmesaniomes);
            if (!dr.IsDBNull(iPmmesaniomes)) entity.Pmmesaniomes = Convert.ToInt32(dr.GetValue(iPmmesaniomes));

            int iPmmesestado = dr.GetOrdinal(this.Pmmesestado);
            if (!dr.IsDBNull(iPmmesestado)) entity.Pmmesestado = Convert.ToInt32(dr.GetValue(iPmmesestado));

            int iPmmesprocesado = dr.GetOrdinal(this.Pmmesprocesado);
            if (!dr.IsDBNull(iPmmesprocesado)) entity.Pmmesprocesado = Convert.ToInt32(dr.GetValue(iPmmesprocesado));

            int iPmmesfecini = dr.GetOrdinal(this.Pmmesfecini);
            if (!dr.IsDBNull(iPmmesfecini)) entity.Pmmesfecini = dr.GetDateTime(iPmmesfecini);

            int iPmmesfecinimes = dr.GetOrdinal(this.Pmmesfecinimes);
            if (!dr.IsDBNull(iPmmesfecinimes)) entity.Pmmesfecinimes = dr.GetDateTime(iPmmesfecinimes);

            int iPmanopcodi = dr.GetOrdinal(this.Pmanopcodi);
            if (!dr.IsDBNull(iPmanopcodi)) entity.Pmanopcodi = Convert.ToInt32(dr.GetValue(iPmanopcodi));

            int iPmmesusucreacion = dr.GetOrdinal(this.Pmmesusucreacion);
            if (!dr.IsDBNull(iPmmesusucreacion)) entity.Pmmesusucreacion = dr.GetString(iPmmesusucreacion);

            int iPmmesfeccreacion = dr.GetOrdinal(this.Pmmesfeccreacion);
            if (!dr.IsDBNull(iPmmesfeccreacion)) entity.Pmmesfeccreacion = dr.GetDateTime(iPmmesfeccreacion);

            int iPmusumodificacion = dr.GetOrdinal(this.Pmusumodificacion);
            if (!dr.IsDBNull(iPmusumodificacion)) entity.Pmusumodificacion = dr.GetString(iPmusumodificacion);

            int iPmfecmodificacion = dr.GetOrdinal(this.Pmfecmodificacion);
            if (!dr.IsDBNull(iPmfecmodificacion)) entity.Pmfecmodificacion = dr.GetDateTime(iPmfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Pmmescodi = "PMMESCODI";
        public string Pmmesaniomes = "PMMESANIOMES";
        public string Pmmesestado = "PMMESESTADO";
        public string Pmmesprocesado = "PMMESPROCESADO";
        public string Pmmesfecini = "PMMESFECINI";
        public string Pmmesfecinimes = "PMMESFECINIMES";
        public string Pmanopcodi = "PMANOPCODI";
        public string Pmmesusucreacion = "PMMESUSUCREACION";
        public string Pmmesfeccreacion = "PMMESFECCREACION";
        public string Pmusumodificacion = "PMUSUMODIFICACION";
        public string Pmfecmodificacion = "PMFECMODIFICACION";

        public string Pmanopanio = "PMANOPANIO";
        public string Pmanopfecini = "PMANOPFECINI";

        #endregion

        public string SqlUpdateEstadoBaja
        {
            get { return GetSqlXml("UpdateEstadoBaja"); }
        }
        public string SqlUpdateAprobar
        {
            get { return base.GetSqlXml("UpdateAprobar"); }
        }
        public string SqlUpdateEstadoProcesado
        {
            get { return GetSqlXml("UpdateEstadoProcesado"); }
        }
        public string SqlGetByCriteriaXAnio
        {
            get { return GetSqlXml("GetByCriteriaXAnio"); }
        }
        public string SqlGetByCriteriaXMes
        {
            get { return GetSqlXml("GetByCriteriaXMes"); }
        }
    }
}
