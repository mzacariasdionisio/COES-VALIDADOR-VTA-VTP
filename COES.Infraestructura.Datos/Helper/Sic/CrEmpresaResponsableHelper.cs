using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class CrEmpresaResponsableHelper : HelperBase
    {
        public CrEmpresaResponsableHelper() : base(Consultas.CrEmpresaResponsableSql)
        {
        }

        public CrEmpresaResponsableDTO Create(IDataReader dr)
        {
            CrEmpresaResponsableDTO entity = new CrEmpresaResponsableDTO();

            int iCrrespemprcodi = dr.GetOrdinal(this.Crrespemprcodi);
            if (!dr.IsDBNull(iCrrespemprcodi)) entity.CRRESPEMPRCODI = dr.GetInt32(iCrrespemprcodi);

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.EMPRCODI = dr.GetInt32(iEmprcodi);

            int iEmprnomb = dr.GetOrdinal(this.Emprnomb);
            if (!dr.IsDBNull(iEmprnomb)) entity.EMPRNOMB = dr.GetString(iEmprnomb);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.LASTDATE = dr.GetDateTime(iLastdate);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.LASTUSER = dr.GetString(iLastuser);

            return entity;
        }

        #region Mapeo de Campos

        public string Crrespemprcodi = "CRRESPEMPRCODI";
        public string Emprcodi = "EMPRCODI";
        public string Cretapacodi = "CRETAPACODI";      
        public string Lastdate = "LASTDATE";
        public string Lastuser = "LASTUSER";

        #endregion

        #region Mapeo de Campos adicionales

        public string Emprnomb = "EMPRNOMB";

        #endregion

        public string ObtenerResponsableEtapa
        {
            get { return base.GetSqlXml("ObtenerResponsableEtapa"); }
        }

        public string ValidarEmpresaResponsable
        {
            get { return base.GetSqlXml("ValidarEmpresaResponsable"); }
        }
        public string SqlDeleteEtapa
        {
            get { return base.GetSqlXml("DeleteEtapa"); }
        }
        public string SqlObtenerEmpresaResponsablexevento
        {
            get { return base.GetSqlXml("SqlObtenerEmpresaResponsablexevento"); }
        }
    }
}
