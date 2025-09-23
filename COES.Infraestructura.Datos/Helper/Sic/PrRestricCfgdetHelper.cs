using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PR_RESTRIC_CFGDET
    /// </summary>
    public class PrRestricCfgdetHelper : HelperBase
    {
        public PrRestricCfgdetHelper(): base(Consultas.PrRestricCfgdetSql)
        {
        }

        public PrRestricCfgdetDTO Create(IDataReader dr)
        {
            PrRestricCfgdetDTO entity = new PrRestricCfgdetDTO();

            int iResdetcodi = dr.GetOrdinal(this.Resdetcodi);
            if (!dr.IsDBNull(iResdetcodi)) entity.Resdetcodi = Convert.ToInt32(dr.GetValue(iResdetcodi));

            int iResdetfuente = dr.GetOrdinal(this.Resdetfuente);
            if (!dr.IsDBNull(iResdetfuente)) entity.Resdetfuente = dr.GetString(iResdetfuente);

            int iResdetcodigo = dr.GetOrdinal(this.Resdetcodigo);
            if (!dr.IsDBNull(iResdetcodigo)) entity.Resdetcodigo = Convert.ToInt32(dr.GetValue(iResdetcodigo));

            int iResdetestado = dr.GetOrdinal(this.Resdetestado);
            if (!dr.IsDBNull(iResdetestado)) entity.Resdetestado = dr.GetString(iResdetestado);

            int iRescfgcodi = dr.GetOrdinal(this.Rescfgcodi);
            if (!dr.IsDBNull(iRescfgcodi)) entity.Rescfgcodi = Convert.ToInt32(dr.GetValue(iRescfgcodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Resdetcodi = "RESDETCODI";
        public string Resdetfuente = "RESDETFUENTE";
        public string Resdetcodigo = "RESDETCODIGO";
        public string Resdetestado = "RESDETESTADO";
        public string Rescfgcodi = "RESCFGCODI";

        #endregion

        public string SqlActualizarRelaciones
        {
            get { return GetSqlXml("ActualizarRelaciones"); }
        }
    }
}
