using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ST_FACTOR
    /// </summary>
    public class StFactorHelper : HelperBase
    {
        public StFactorHelper(): base(Consultas.StFactorSql)
        {
        }

        public StFactorDTO Create(IDataReader dr)
        {
            StFactorDTO entity = new StFactorDTO();

            int iStfactcodi = dr.GetOrdinal(this.Stfactcodi);
            if (!dr.IsDBNull(iStfactcodi)) entity.Stfactcodi = Convert.ToInt32(dr.GetValue(iStfactcodi));

            int iStrecacodi = dr.GetOrdinal(this.Strecacodi);
            if (!dr.IsDBNull(iStrecacodi)) entity.Strecacodi = Convert.ToInt32(dr.GetValue(iStrecacodi));

            int iSistrncodi = dr.GetOrdinal(this.Sistrncodi);
            if (!dr.IsDBNull(iSistrncodi)) entity.Sistrncodi = Convert.ToInt32(dr.GetValue(iSistrncodi));

            int iStfacttor = dr.GetOrdinal(this.Stfacttor);
            if (!dr.IsDBNull(iStfacttor)) entity.Stfacttor = dr.GetDecimal(iStfacttor);

            int iStfactusucreacion = dr.GetOrdinal(this.Stfactusucreacion);
            if (!dr.IsDBNull(iStfactusucreacion)) entity.Stfactusucreacion = dr.GetString(iStfactusucreacion);

            int iStfactfeccreacion = dr.GetOrdinal(this.Stfactfeccreacion);
            if (!dr.IsDBNull(iStfactfeccreacion)) entity.Stfactfeccreacion = dr.GetDateTime(iStfactfeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Stfactcodi = "STFACTCODI";
        public string Strecacodi = "STRECACODI";
        public string Sistrncodi = "SISTRNCODI";
        public string Stfacttor = "STFACTTOR";
        public string Stfactusucreacion = "STFACTUSUCREACION";
        public string Stfactfeccreacion = "STFACTFECCREACION";
        //atributos de consultas
        public string SisTrnnombre = "SISTRNNOMBRE";
        #endregion


        public string SqlGetBySisTrans
        {
            get { return base.GetSqlXml("GetBySisTrans"); }
        }

        public string SqlDeleteVersion
        {
            get { return base.GetSqlXml("DeleteVersion"); }
        }

        public string SqlListByStFactorVersion
        {
            get { return base.GetSqlXml("ListByStFactorVersion"); }
        }
    }
}
