using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EVE_INFORME_FILE
    /// </summary>
    public class EveInformeFileHelper : HelperBase
    {
        public EveInformeFileHelper(): base(Consultas.EveInformeFileSql)
        {
        }

        public EveInformeFileDTO Create(IDataReader dr)
        {
            EveInformeFileDTO entity = new EveInformeFileDTO();

            int iEveninfcodi = dr.GetOrdinal(this.Eveninfcodi);
            if (!dr.IsDBNull(iEveninfcodi)) entity.Eveninfcodi = Convert.ToInt32(dr.GetValue(iEveninfcodi));

            int iEveninffilecodi = dr.GetOrdinal(this.Eveninffilecodi);
            if (!dr.IsDBNull(iEveninffilecodi)) entity.Eveninffilecodi = Convert.ToInt32(dr.GetValue(iEveninffilecodi));

            int iDesfile = dr.GetOrdinal(this.Desfile);
            if (!dr.IsDBNull(iDesfile)) entity.Desfile = dr.GetString(iDesfile);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            int iExtfile = dr.GetOrdinal(this.Extfile);
            if (!dr.IsDBNull(iExtfile)) entity.Extfile = dr.GetString(iExtfile);

            return entity;
        }


        #region Mapeo de Campos

        public string Eveninfcodi = "EVENINFCODI";
        public string Eveninffilecodi = "EVENINFFILECODI";
        public string Desfile = "DESFILE";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";
        public string Extfile = "EXTFILE";

        public string SqlObtenerFileInforme
        {
            get { return base.GetSqlXml("ObtenerFileInforme"); }
        }

        #endregion
    }
}

