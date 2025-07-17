using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class EveRecomobservHelper : HelperBase
    {
        public EveRecomobservHelper() : base(Consultas.EveRecomobservSql)
        {
        }
        public EveRecomobservDTO Create(IDataReader dr)
        {
            EveRecomobservDTO entity = new EveRecomobservDTO();

            int iEverecomobservcodi = dr.GetOrdinal(this.Everecomobservcodi);
            if (!dr.IsDBNull(iEverecomobservcodi)) entity.EVERECOMOBSERVCODI = dr.GetInt32(iEverecomobservcodi);

            int iEvencodi = dr.GetOrdinal(this.Evencodi);
            if (!dr.IsDBNull(iEvencodi)) entity.EVENCODI = dr.GetInt32(iEvencodi);

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEvencodi)) entity.EMPRCODI = dr.GetInt32(iEmprcodi);

            int iEverecomobservtipo = dr.GetOrdinal(this.Everecomobservtipo);
            if (!dr.IsDBNull(iEverecomobservtipo)) entity.EVERECOMOBSERVTIPO = dr.GetInt32(iEverecomobservtipo);

            int iEverecomobservdesc = dr.GetOrdinal(this.Everecomobservdesc);
            if (!dr.IsDBNull(iEverecomobservdesc)) entity.EVERECOMOBSERVDESC = dr.GetString(iEverecomobservdesc);

            int iEverecomobservestado = dr.GetOrdinal(this.Everecomobservestado);
            if (!dr.IsDBNull(iEverecomobservestado)) entity.EVERECOMOBSERVESTADO = dr.GetString(iEverecomobservestado);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.LASTDATE = dr.GetDateTime(iLastdate);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.LASTUSER = dr.GetString(iLastuser);

            return entity;
        }

        #region Mapeo de Campos

        public string Everecomobservcodi = "EVERECOMOBSERVCODI";
        public string Evencodi = "EVENCODI";
        public string Emprcodi = "EMPRCODI";      
        public string Everecomobservtipo = "EVERECOMOBSERVTIPO";
        public string Everecomobservdesc = "EVERECOMOBSERVDESC";
        public string Everecomobservestado = "EVERECOMOBSERVESTADO";
        public string Lastdate = "LASTDATE";
        public string Lastuser = "LASTUSER";

        #endregion

        #region Campos Adicionales
        public string Emprnomb = "EMPRNOMB";
        #endregion
    }
}
