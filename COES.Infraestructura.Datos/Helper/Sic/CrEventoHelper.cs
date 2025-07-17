using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class CrEventoHelper :HelperBase
    {
        public CrEventoHelper() : base(Consultas.CrEventoSql)
        {
        }

        public CrEventoDTO Create(IDataReader dr)
        {
            CrEventoDTO entity = new CrEventoDTO();

            int iCrevencodi = dr.GetOrdinal(this.Crevencodi);
            if (!dr.IsDBNull(iCrevencodi)) entity.CREVENCODI = dr.GetInt32(iCrevencodi);

            int iAfecodi = dr.GetOrdinal(this.Afecodi);
            if (!dr.IsDBNull(iAfecodi)) entity.AFECODI = dr.GetInt32(iAfecodi);

            int iCrespecialcodi = dr.GetOrdinal(this.Crespecialcodi);
            if (!dr.IsDBNull(iCrespecialcodi)) entity.CRESPECIALCODI = dr.GetInt32(iCrespecialcodi);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.LASTDATE = dr.GetDateTime(iLastdate);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.LASTUSER = dr.GetString(iLastuser);

            return entity;
        }

        #region Mapeo de Campos

        public string Crevencodi = "CREVENCODI";
        public string Afecodi = "AFECODI";
        public string Crespecialcodi = "CRESPECIALCODI";
        public string Lastdate = "LASTDATE";
        public string Lastuser = "LASTUSER";

        #endregion

        #region Mapeo de Campos Adicionales

        public string CodigoEvento = "CODIGOEVENTO";
        public string Fechaevento = "FECHAEVENTO";
        public string Nombreevento = "NOMBREEVENTO";
        public string CodigoImpugnacion = "CODIGOIMPUGNACION";
        public string Impugnacion = "IMPUGNACION";
        public string Afeitdecfechaelab = "AFEITDECFECHAELAB";
        public string Evendescctaf = "EVENDESCCTAF";

        #endregion

        public string SqlGetByAfecodi
        {
            get { return base.GetSqlXml("GetByAfecodi"); }
        }
    }
}
