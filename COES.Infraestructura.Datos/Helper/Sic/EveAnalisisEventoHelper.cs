using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Security.Policy;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class EveAnalisisEventoHelper : HelperBase
    {
        public EveAnalisisEventoHelper() : base(Consultas.EveAnalisisEventoSql)
        {
        }

        public EveAnalisisEventoDTO Create(IDataReader dr)
        {
            EveAnalisisEventoDTO entity = new EveAnalisisEventoDTO();

            int iEveanaevecodi = dr.GetOrdinal(this.Eveanaevecodi);
            if (!dr.IsDBNull(iEveanaevecodi)) entity.EVEANAEVECODI = dr.GetInt32(iEveanaevecodi);

            int iEvencodi = dr.GetOrdinal(this.Evencodi);
            if (!dr.IsDBNull(iEvencodi)) entity.EVENCODI = dr.GetInt32(iEvencodi);

            int iEveanaevedescnumeral = dr.GetOrdinal(this.Eveanaevedescnumeral);
            if (!dr.IsDBNull(iEveanaevedescnumeral)) entity.EVEANAEVEDESCNUMERAL = dr.GetString(iEveanaevedescnumeral);

            int iEveanaevedescfigura = dr.GetOrdinal(this.Eveanaevedescfigura);
            if (!dr.IsDBNull(iEveanaevedescfigura)) entity.EVEANAEVEDESCFIGURA = dr.GetString(iEveanaevedescfigura);

            int iEveanaeverutafigura = dr.GetOrdinal(this.Eveanaeverutafigura);
            if (!dr.IsDBNull(iEveanaeverutafigura)) entity.EVEANAEVERUTAFIGURA = dr.GetString(iEveanaeverutafigura);

            int iEvenumcodi = dr.GetOrdinal(this.Evenumcodi);
            if (!dr.IsDBNull(iEvenumcodi)) entity.EVENUMCODI = dr.GetInt32(iEvenumcodi);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.LASTDATE = dr.GetDateTime(iLastdate);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.LASTUSER = dr.GetString(iLastuser);

            int iEveanatipo = dr.GetOrdinal(this.Eveanatipo);
            if (!dr.IsDBNull(iEveanatipo)) entity.EVEANATIPO = dr.GetInt32(iEveanatipo);

            int iEveanaHora = dr.GetOrdinal(this.EveanaHora);
            if (!dr.IsDBNull(iEveanaHora)) entity.EVEANAHORA = dr.GetString(iEveanaHora);

            return entity;
        }

        #region Mapeo de Campos

        public string Eveanaevecodi = "EVEANAEVECODI";
        public string Evencodi = "EVENCODI";
        public string Eveanaevedescnumeral = "EVEANAEVEDESCNUMERAL";
        public string Eveanaevedescfigura = "EVEANAEVEDESCFIGURA";
        public string Eveanaeverutafigura = "EVEANAEVERUTAFIGURA";
        public string Evenumcodi = "EVENUMCODI";
        public string Lastdate = "LASTDATE";
        public string Lastuser = "LASTUSER";
        public string Eveanatipo = "EVEANATIPO";
        public string EveanaHora = "EVEANAHORA";
        public string EveanaPosicion = "EVEANAPOSICION";
        #endregion
        #region Campos Adicionales
        public string Evetipnumdescripcion = "EVETIPNUMDESCRIPCION";
        #endregion

        public string SqlUpdateDescripcion
        {
            get { return GetSqlXml("UpdateDescripcion"); }
        }

        public string SqlValidarTipoNumeralxAnalisisEvento
        {
            get { return base.GetSqlXml("ValidarTipoNumeralxAnalisisEvento"); }
        }

        public string SqlDeleteAnalisis
        {
            get { return base.GetSqlXml("DeleteAnalisisxEvento"); }
        }
    }
}
