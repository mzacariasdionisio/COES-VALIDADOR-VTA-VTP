using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PF_VERSION
    /// </summary>
    public class PfVersionHelper : HelperBase
    {
        public PfVersionHelper(): base(Consultas.PfVersionSql)
        {
        }

        public PfVersionDTO Create(IDataReader dr)
        {
            PfVersionDTO entity = new PfVersionDTO();

            int iPfverscodi = dr.GetOrdinal(this.Pfverscodi);
            if (!dr.IsDBNull(iPfverscodi)) entity.Pfverscodi = Convert.ToInt32(dr.GetValue(iPfverscodi));

            int iPfrecacodi = dr.GetOrdinal(this.Pfrecacodi);
            if (!dr.IsDBNull(iPfrecacodi)) entity.Pfrecacodi = Convert.ToInt32(dr.GetValue(iPfrecacodi));

            int iPfrecucodi = dr.GetOrdinal(this.Pfrecucodi);
            if (!dr.IsDBNull(iPfrecucodi)) entity.Pfrecucodi = Convert.ToInt32(dr.GetValue(iPfrecucodi));

            int iPfversusucreacion = dr.GetOrdinal(this.Pfversusucreacion);
            if (!dr.IsDBNull(iPfversusucreacion)) entity.Pfversusucreacion = dr.GetString(iPfversusucreacion);

            int iPfversfeccreacion = dr.GetOrdinal(this.Pfversfeccreacion);
            if (!dr.IsDBNull(iPfversfeccreacion)) entity.Pfversfeccreacion = dr.GetDateTime(iPfversfeccreacion);

            int iPfversnumero = dr.GetOrdinal(this.Pfversnumero);
            if (!dr.IsDBNull(iPfversnumero)) entity.Pfversnumero = Convert.ToInt32(dr.GetValue(iPfversnumero));

            int iPfversestado = dr.GetOrdinal(this.Pfversestado);
            if (!dr.IsDBNull(iPfversestado)) entity.Pfversestado = dr.GetString(iPfversestado);

            int iIrptcodi = dr.GetOrdinal(this.Irptcodi);
            if (!dr.IsDBNull(iIrptcodi)) entity.Irptcodi = Convert.ToInt32(dr.GetValue(iIrptcodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Pfverscodi = "PFVERSCODI";
        public string Pfrecacodi = "PFRECACODI";
        public string Pfrecucodi = "PFRECUCODI";
        public string Pfversusucreacion = "PFVERSUSUCREACION";
        public string Pfversfeccreacion = "PFVERSFECCREACION";
        public string Pfversnumero = "PFVERSNUMERO";
        public string Pfversestado = "PFVERSESTADO";
        public string Irptcodi = "IRPTCODI";

        #endregion
    }
}
