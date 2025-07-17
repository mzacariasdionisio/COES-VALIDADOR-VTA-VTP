using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EVE_EVENEQUIPO
    /// </summary>
    public class EveEvenequipoHelper : HelperBase
    {
        public EveEvenequipoHelper(): base(Consultas.EveEvenequipoSql)
        {
        }

        public EveEvenequipoDTO Create(IDataReader dr)
        {
            EveEvenequipoDTO entity = new EveEvenequipoDTO();

            int iEvencodi = dr.GetOrdinal(this.Evencodi);
            if (!dr.IsDBNull(iEvencodi)) entity.Evencodi = Convert.ToInt32(dr.GetValue(iEvencodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            return entity;
        }


        #region Mapeo de Campos

        public string Evencodi = "EVENCODI";
        public string Equicodi = "EQUICODI";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";

        public string SqlObtenerEquiposPorEvento
        {
            get { return base.GetSqlXml("ObtenerEquiposPorEvento"); }
        }

        public string SqlDeleteEquipo
        {
            get { return base.GetSqlXml("DeleteEquipo"); }
        }

        #endregion
    }
}
