using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class CrEmpresaSolicitanteHelper : HelperBase
    {
        public CrEmpresaSolicitanteHelper() : base(Consultas.CrEmpresaSolicitanteSql)
        {
        }

        public CrEmpresaSolicitanteDTO Create(IDataReader dr)
        {
            CrEmpresaSolicitanteDTO entity = new CrEmpresaSolicitanteDTO();

            int iCrsolemprcodi = dr.GetOrdinal(this.Crsolemprcodi);
            if (!dr.IsDBNull(iCrsolemprcodi)) entity.CRSOLEMPRCODI = dr.GetInt32(iCrsolemprcodi);

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.EMPRCODI = dr.GetInt32(iEmprcodi);

            int iCretapacodi = dr.GetOrdinal(this.Cretapacodi);
            if (!dr.IsDBNull(iCretapacodi)) entity.CRETAPACODI = dr.GetInt32(iCretapacodi);

            int iEmprnomb = dr.GetOrdinal(this.Emprnomb);
            if (!dr.IsDBNull(iEmprnomb)) entity.EMPRNOMB = dr.GetString(iEmprnomb);

            int iCrargumento = dr.GetOrdinal(this.Crargumento);
            if (!dr.IsDBNull(iCrargumento)) entity.CRARGUMENTO = dr.GetString(iCrargumento);

            int iCrdecision = dr.GetOrdinal(this.Crdecision);
            if (!dr.IsDBNull(iCrdecision)) entity.CRDECISION = dr.GetString(iCrdecision);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.LASTDATE = dr.GetDateTime(iLastdate);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.LASTUSER = dr.GetString(iLastuser);

            return entity;
        }

        #region Mapeo de Campos

        public string Crsolemprcodi = "CRSOLEMPRCODI";
        public string Emprcodi = "EMPRCODI";
        public string Cretapacodi = "CRETAPACODI";
        public string Crargumento = "CRARGUMENTO";
        public string Crdecision = "CRDECISION";
        public string Lastdate = "LASTDATE";
        public string Lastuser = "LASTUSER";

        #endregion

        #region Mapeo de Campos adicionales

        public string Emprnomb = "EMPRNOMB";

        #endregion

        public string ObtenerSolicitanteEtapa
        {
            get { return base.GetSqlXml("ObtenerSolicitanteEtapa"); }
        }
        public string ValidarEmpresaSolicitante
        {
            get { return base.GetSqlXml("ValidarEmpresaSolicitante"); }
        }
        public string SqlDeleteSolicitantexEtapa
        {
            get { return base.GetSqlXml("DeleteSolicitantexEtapa"); }
        }
        public string SqlObtenerEmpresaSolicitantexEvento
        {
            get { return base.GetSqlXml("SqlObtenerEmpresaSolicitantexEvento"); }
        }
    }
}
