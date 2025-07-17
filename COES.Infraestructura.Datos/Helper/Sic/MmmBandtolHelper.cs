using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla MMM_BANDTOL
    /// </summary>
    public class MmmBandtolHelper : HelperBase
    {
        public MmmBandtolHelper()
            : base(Consultas.MmmBandtolSql)
        {
        }

        public MmmBandtolDTO Create(IDataReader dr)
        {
            MmmBandtolDTO entity = new MmmBandtolDTO();

            int iMmmtolcodi = dr.GetOrdinal(this.Mmmtolcodi);
            if (!dr.IsDBNull(iMmmtolcodi)) entity.Mmmtolcodi = Convert.ToInt32(dr.GetValue(iMmmtolcodi));

            int iMmmtolfechavigencia = dr.GetOrdinal(this.Mmmtolfechavigencia);
            if (!dr.IsDBNull(iMmmtolfechavigencia)) entity.Mmmtolfechavigencia = dr.GetDateTime(iMmmtolfechavigencia);

            int iMmmtolusucreacion = dr.GetOrdinal(this.Mmmtolusucreacion);
            if (!dr.IsDBNull(iMmmtolusucreacion)) entity.Mmmtolusucreacion = dr.GetString(iMmmtolusucreacion);

            int iMmmtolfeccreacion = dr.GetOrdinal(this.Mmmtolfeccreacion);
            if (!dr.IsDBNull(iMmmtolfeccreacion)) entity.Mmmtolfeccreacion = dr.GetDateTime(iMmmtolfeccreacion);

            int iMmmtolnormativa = dr.GetOrdinal(this.Mmmtolnormativa);
            if (!dr.IsDBNull(iMmmtolnormativa)) entity.Mmmtolnormativa = dr.GetString(iMmmtolnormativa);

            int iMmmtolusumodificacion = dr.GetOrdinal(this.Mmmtolusumodificacion);
            if (!dr.IsDBNull(iMmmtolusumodificacion)) entity.Mmmtolusumodificacion = dr.GetString(iMmmtolusumodificacion);

            int iMmmtolfecmodificacion = dr.GetOrdinal(this.Mmmtolfecmodificacion);
            if (!dr.IsDBNull(iMmmtolfecmodificacion)) entity.Mmmtolfecmodificacion = dr.GetDateTime(iMmmtolfecmodificacion);

            int iImmecodi = dr.GetOrdinal(this.Immecodi);
            if (!dr.IsDBNull(iImmecodi)) entity.Immecodi = Convert.ToInt32(dr.GetValue(iImmecodi));

            int iMmmtolcriterio = dr.GetOrdinal(this.Mmmtolcriterio);
            if (!dr.IsDBNull(iMmmtolcriterio)) entity.Mmmtolcriterio = dr.GetString(iMmmtolcriterio);

            int iMmmtolvaloreferencia = dr.GetOrdinal(this.Mmmtolvalorreferencia);
            if (!dr.IsDBNull(iMmmtolvaloreferencia)) entity.Mmmtolvalorreferencia = dr.GetDecimal(iMmmtolvaloreferencia);

            int iMmmtolvalortolerancia = dr.GetOrdinal(this.Mmmtolvalortolerancia);
            if (!dr.IsDBNull(iMmmtolvalortolerancia)) entity.Mmmtolvalortolerancia = dr.GetDecimal(iMmmtolvalortolerancia);

            int iMmmtolestado = dr.GetOrdinal(this.Mmmtolestado);
            if (!dr.IsDBNull(iMmmtolestado)) entity.Mmmtolestado = dr.GetString(iMmmtolestado);

            return entity;
        }


        #region Mapeo de Campos

        public string Mmmtolcodi = "MMMTOLCODI";
        public string Mmmtolfechavigencia = "MMMTOLFECHAVIGENCIA";
        public string Mmmtolusucreacion = "MMMTOLUSUCREACION";
        public string Mmmtolfeccreacion = "MMMTOLFECCREACION";
        public string Mmmtolnormativa = "MMMTOLNORMATIVA";
        public string Mmmtolusumodificacion = "MMMTOLUSUMODIFICACION";
        public string Mmmtolfecmodificacion = "MMMTOLFECMODIFICACION";
        public string Immecodi = "IMMECODI";
        public string Mmmtolcriterio = "MMMTOLCRITERIO";
        public string Mmmtolvalorreferencia = "MMMTOLVALORREFERENCIA";
        public string Mmmtolvalortolerancia = "MMMTOLVALORTOLERANCIA";
        public string Mmmtolestado = "MMMTOLESTADO";
        public string Immenombre = "IMMENOMBRE";
        public string Immecodigo = "IMMECODIGO";

        #endregion

        public string SqlGetByIndicadorYPeriodo
        {
            get { return base.GetSqlXml("GetByIndicadorYPeriodo"); }
        }
    }
}
