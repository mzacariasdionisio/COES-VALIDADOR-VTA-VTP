using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class EveCondPreviaHelper : HelperBase
    {
        public EveCondPreviaHelper() : base(Consultas.EveCondPreviaSql)
        {
        }

        public EveCondPreviaDTO Create(IDataReader dr)
        {
            EveCondPreviaDTO entity = new EveCondPreviaDTO();

            int iEvecondprcodi = dr.GetOrdinal(this.Evecondprcodi);
            if (!dr.IsDBNull(iEvecondprcodi)) entity.EVECONDPRCODI = dr.GetInt32(iEvecondprcodi);

            int iEvencodi = dr.GetOrdinal(this.Evencodi);
            if (!dr.IsDBNull(iEvencodi)) entity.EVENCODI = dr.GetInt32(iEvencodi);

            int iEvecondprtipo = dr.GetOrdinal(this.Evecondprtipo);
            if (!dr.IsDBNull(iEvecondprtipo)) entity.EVECONDPRTIPO = dr.GetString(iEvecondprtipo);

            int iEvecondprcodigounidad = dr.GetOrdinal(this.Evecondprcodigounidad);
            if (!dr.IsDBNull(iEvecondprcodigounidad)) entity.EVECONDPRCODIGOUNIDAD = dr.GetString(iEvecondprcodigounidad);

            int iEvecondprsubestaciona = dr.GetOrdinal(this.Evecondprsubestaciona);
            if (!dr.IsDBNull(iEvecondprsubestaciona)) entity.EVECONDPRSUBESTACIONA = dr.GetString(iEvecondprsubestaciona);

            int iEvecondprsubestacioncent = dr.GetOrdinal(this.Evecondprsubestacioncent);
            if (!dr.IsDBNull(iEvecondprsubestacioncent)) entity.EVECONDPRSUBESTACIONCENT = dr.GetInt32(iEvecondprsubestacioncent);

            int iEvecondprtension = dr.GetOrdinal(this.Evecondprtension);
            if (!dr.IsDBNull(iEvecondprtension)) entity.EVECONDPRTENSION = dr.GetDecimal(iEvecondprtension);

            int iEvecondprpotenciamw = dr.GetOrdinal(this.Evecondprpotenciamw);
            if (!dr.IsDBNull(iEvecondprpotenciamw)) entity.EVECONDPRPOTENCIAMW = dr.GetString(iEvecondprpotenciamw);

            int iEvecondprpotenciamvar = dr.GetOrdinal(this.Evecondprpotenciamvar);
            if (!dr.IsDBNull(iEvecondprpotenciamvar)) entity.EVECONDPRPOTENCIAMVAR = dr.GetString(iEvecondprpotenciamvar);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.LASTDATE = dr.GetDateTime(iLastdate);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.LASTUSER = dr.GetString(iLastuser);

            int iEvecondprcentralde = dr.GetOrdinal(this.Evecondprcentralde);
            if (!dr.IsDBNull(iEvecondprcentralde)) entity.EVECONDPRCENTRALDE = dr.GetString(iEvecondprcentralde);

            return entity;
        }

        #region Mapeo de Campos

        public string Evecondprcodi = "EVECONDPRCODI";
        public string Evencodi = "EVENCODI";
        public string Evecondprtipo = "EVECONDPRTIPO";
        public string Evecondprcodigounidad = "EVECONDPRCODIGOUNIDAD";
        public string Evecondprsubestaciona = "EVECONDPRSUBESTACIONA";
        public string Evecondprsubestacioncent = "EVECONDPRSUBESTACIONCENT";
        public string Evecondprtension = "EVECONDPRTENSION";
        public string Evecondprpotenciamw = "EVECONDPRPOTENCIAMW";
        public string Evecondprpotenciamvar = "EVECONDPRPOTENCIAMVAR";
        public string Lastdate = "LASTDATE";
        public string Lastuser = "LASTUSER";
        public string Evecondprcentralde = "EVECONDPRCENTRALDE";

        #endregion

        public string GetByIdCanalZona
        {
            get { return GetSqlXml("GetByIdCanalZona"); }
        }
    }
}
