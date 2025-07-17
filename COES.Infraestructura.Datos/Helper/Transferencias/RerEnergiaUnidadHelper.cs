using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RER_ENERGIAUNIDAD
    /// </summary>
    public class RerEnergiaUnidadHelper : HelperBase
    {
        public RerEnergiaUnidadHelper() : base(Consultas.RerEnergiaUnidadSql)
        {
        }

        public RerEnergiaUnidadDTO Create(IDataReader dr)
        {
            RerEnergiaUnidadDTO entity = new RerEnergiaUnidadDTO();

            int iRereucodi = dr.GetOrdinal(this.Rereucodi);
            if (!dr.IsDBNull(iRereucodi)) entity.Rereucodi = Convert.ToInt32(dr.GetValue(iRereucodi));

            int iRersedcodi = dr.GetOrdinal(this.Rersedcodi);
            if (!dr.IsDBNull(iRersedcodi)) entity.Rersedcodi = Convert.ToInt32(dr.GetValue(iRersedcodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iRereuenergiaunidad = dr.GetOrdinal(this.Rereuenergiaunidad);
            if (!dr.IsDBNull(iRereuenergiaunidad)) entity.Rereuenergiaunidad = dr.GetString(iRereuenergiaunidad);

            int iRereutotenergia = dr.GetOrdinal(this.Rereutotenergia);
            if (!dr.IsDBNull(iRereutotenergia)) entity.Rereutotenergia = dr.GetDecimal(iRereutotenergia);

            int iRereuusucreacion = dr.GetOrdinal(this.Rereuusucreacion);
            if (!dr.IsDBNull(iRereuusucreacion)) entity.Rereuusucreacion = dr.GetString(iRereuusucreacion);

            int iRereufeccreacion = dr.GetOrdinal(this.Rereufeccreacion);
            if (!dr.IsDBNull(iRereufeccreacion)) entity.Rereufeccreacion = dr.GetDateTime(iRereufeccreacion);

            return entity;
        }

        public string SqlListByPeriodo
        {
            get { return base.GetSqlXml("ListByPeriodo"); }
        }

        #region Mapeo de Campos

        public string Rereucodi = "REREUCODI";
        public string Rersedcodi = "RERSEDCODI";
        public string Equicodi = "EQUICODI";
        public string Rereuenergiaunidad = "REREUENERGIAUNIDAD";
        public string Rereutotenergia = "REREUTOTENERGIA";
        public string Rereuusucreacion = "REREUUSUCREACION";
        public string Rereufeccreacion = "REREUFECCREACION";

        #endregion

    }
}