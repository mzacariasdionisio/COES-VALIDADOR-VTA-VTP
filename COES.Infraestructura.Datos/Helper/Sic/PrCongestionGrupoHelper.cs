using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PR_CONGESTION_GRUPO
    /// </summary>
    public class PrCongestionGrupoHelper : HelperBase
    {
        public PrCongestionGrupoHelper(): base(Consultas.PrCongestionGrupoSql)
        {
        }

        public PrCongestionGrupoDTO Create(IDataReader dr)
        {
            PrCongestionGrupoDTO entity = new PrCongestionGrupoDTO();

            int iCongrpcodi = dr.GetOrdinal(this.Congrpcodi);
            if (!dr.IsDBNull(iCongrpcodi)) entity.Congrpcodi = Convert.ToInt32(dr.GetValue(iCongrpcodi));

            int iCongescodi = dr.GetOrdinal(this.Congescodi);
            if (!dr.IsDBNull(iCongescodi)) entity.Congescodi = Convert.ToInt32(dr.GetValue(iCongescodi));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            return entity;
        }


        #region Mapeo de Campos

        public string Congrpcodi = "CONGRPCODI";
        public string Congescodi = "CONGESCODI";
        public string Grupocodi = "GRUPOCODI";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";

        #endregion
    }
}
