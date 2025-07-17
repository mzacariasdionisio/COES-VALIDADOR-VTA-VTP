using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PMO_QN_CAMBIOENVIO
    /// </summary>
    public class PmoQnCambioenvioHelper : HelperBase
    {
        public PmoQnCambioenvioHelper() : base(Consultas.PmoQnCambioenvioSql)
        {
        }

        public PmoQnCambioenvioDTO Create(IDataReader dr)
        {
            PmoQnCambioenvioDTO entity = new PmoQnCambioenvioDTO();

            int iQncmbecodi = dr.GetOrdinal(this.Qncmbecodi);
            if (!dr.IsDBNull(iQncmbecodi)) entity.Qncmbecodi = Convert.ToInt32(dr.GetValue(iQncmbecodi));

            int iQnbenvcodi = dr.GetOrdinal(this.Qnbenvcodi);
            if (!dr.IsDBNull(iQnbenvcodi)) entity.Qnbenvcodi = Convert.ToInt32(dr.GetValue(iQnbenvcodi));

            int iSddpcodi = dr.GetOrdinal(this.Sddpcodi);
            if (!dr.IsDBNull(iSddpcodi)) entity.Sddpcodi = Convert.ToInt32(dr.GetValue(iSddpcodi));

            int iQncmbefecha = dr.GetOrdinal(this.Qncmbefecha);
            if (!dr.IsDBNull(iQncmbefecha)) entity.Qncmbefecha = dr.GetDateTime(iQncmbefecha);

            int iQncmbedatos = dr.GetOrdinal(this.Qncmbedatos);
            if (!dr.IsDBNull(iQncmbedatos)) entity.Qncmbedatos = dr.GetString(iQncmbedatos);

            int iQncmbecolvar = dr.GetOrdinal(this.Qncmbecolvar);
            if (!dr.IsDBNull(iQncmbecolvar)) entity.Qncmbecolvar = dr.GetString(iQncmbecolvar);

            int iQncmbeusucreacion = dr.GetOrdinal(this.Qncmbeusucreacion);
            if (!dr.IsDBNull(iQncmbeusucreacion)) entity.Qncmbeusucreacion = dr.GetString(iQncmbeusucreacion);

            int iQncmbefeccreacion = dr.GetOrdinal(this.Qncmbefeccreacion);
            if (!dr.IsDBNull(iQncmbefeccreacion)) entity.Qncmbefeccreacion = dr.GetDateTime(iQncmbefeccreacion);

            int iQncmbeorigen = dr.GetOrdinal(this.Qncmbeorigen);
            if (!dr.IsDBNull(iQncmbeorigen)) entity.Qncmbeorigen = dr.GetString(iQncmbeorigen);

            return entity;
        }


        #region Mapeo de Campos

        public string Qncmbecodi = "QNCMBECODI";
        public string Qnbenvcodi = "QNBENVCODI";
        public string Sddpcodi = "SDDPCODI";
        public string Qncmbefecha = "QNCMBEFECHA";
        public string Qncmbedatos = "QNCMBEDATOS";
        public string Qncmbecolvar = "QNCMBECOLVAR";
        public string Qncmbeusucreacion = "QNCMBEUSUCREACION";
        public string Qncmbefeccreacion = "QNCMBEFECCREACION";
        public string Qncmbeorigen = "QNCMBEORIGEN";

        #endregion

        public string SqlListByEnvio
        {
            get { return base.GetSqlXml("ListByEnvio"); }
        }
    }
}
