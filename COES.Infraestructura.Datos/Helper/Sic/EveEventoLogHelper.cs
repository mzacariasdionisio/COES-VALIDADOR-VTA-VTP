using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EVE_EVENTO_LOG
    /// </summary>
    public class EveEventoLogHelper : HelperBase
    {
        public EveEventoLogHelper(): base(Consultas.EveEventoLogSql)
        {
        }

        public EveEventoLogDTO Create(IDataReader dr)
        {
            EveEventoLogDTO entity = new EveEventoLogDTO();

            int iEvelogcodi = dr.GetOrdinal(this.Evelogcodi);
            if (!dr.IsDBNull(iEvelogcodi)) entity.Evelogcodi = Convert.ToInt32(dr.GetValue(iEvelogcodi));

            int iEvencodi = dr.GetOrdinal(this.Evencodi);
            if (!dr.IsDBNull(iEvencodi)) entity.Evencodi = Convert.ToInt32(dr.GetValue(iEvencodi));

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            int iDesoperacion = dr.GetOrdinal(this.Desoperacion);
            if (!dr.IsDBNull(iDesoperacion)) entity.Desoperacion = dr.GetString(iDesoperacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Evelogcodi = "EVELOGCODI";
        public string Evencodi = "EVENCODI";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";
        public string Desoperacion = "DESOPERACION";

        #endregion
    }
}
