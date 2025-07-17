using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CO_PROCESOCALCULO
    /// </summary>
    public class CoProcesocalculoHelper : HelperBase
    {
        public CoProcesocalculoHelper(): base(Consultas.CoProcesocalculoSql)
        {
        }

        public CoProcesocalculoDTO Create(IDataReader dr)
        {
            CoProcesocalculoDTO entity = new CoProcesocalculoDTO();

            int iCoprcacodi = dr.GetOrdinal(this.Coprcacodi);
            if (!dr.IsDBNull(iCoprcacodi)) entity.Coprcacodi = Convert.ToInt32(dr.GetValue(iCoprcacodi));

            int iCoprcafecproceso = dr.GetOrdinal(this.Coprcafecproceso);
            if (!dr.IsDBNull(iCoprcafecproceso)) entity.Coprcafecproceso = dr.GetDateTime(iCoprcafecproceso);

            int iCoprcausuproceso = dr.GetOrdinal(this.Coprcausuproceso);
            if (!dr.IsDBNull(iCoprcausuproceso)) entity.Coprcausuproceso = dr.GetString(iCoprcausuproceso);

            int iCoprcaestado = dr.GetOrdinal(this.Coprcaestado);
            if (!dr.IsDBNull(iCoprcaestado)) entity.Coprcaestado = dr.GetString(iCoprcaestado);

            int iCoprcafecinicio = dr.GetOrdinal(this.Coprcafecinicio);
            if (!dr.IsDBNull(iCoprcafecinicio)) entity.Coprcafecinicio = dr.GetDateTime(iCoprcafecinicio);

            int iCoprcafecfin = dr.GetOrdinal(this.Coprcafecfin);
            if (!dr.IsDBNull(iCoprcafecfin)) entity.Coprcafecfin = dr.GetDateTime(iCoprcafecfin);

            int iPericodi = dr.GetOrdinal(this.Pericodi);
            if (!dr.IsDBNull(iPericodi)) entity.Pericodi = Convert.ToInt32(dr.GetValue(iPericodi));

            int iVcrecaversion = dr.GetOrdinal(this.Vcrecaversion);
            if (!dr.IsDBNull(iVcrecaversion)) entity.Vcrecaversion = Convert.ToInt32(dr.GetValue(iVcrecaversion));

            int iCopercodi = dr.GetOrdinal(this.Copercodi);
            if (!dr.IsDBNull(iCopercodi)) entity.Copercodi = Convert.ToInt32(dr.GetValue(iCopercodi));

            int iCovercodi = dr.GetOrdinal(this.Covercodi);
            if (!dr.IsDBNull(iCovercodi)) entity.Covercodi = Convert.ToInt32(dr.GetValue(iCovercodi));

            int iCoprcafuentedato = dr.GetOrdinal(this.Coprcafuentedato);
            if (!dr.IsDBNull(iCoprcafuentedato)) entity.Coprcafuentedato = dr.GetString(iCoprcafuentedato);

            return entity;
        }


        #region Mapeo de Campos

        public string Coprcacodi = "COPRCACODI";
        public string Coprcafecproceso = "COPRCAFECPROCESO";
        public string Coprcausuproceso = "COPRCAUSUPROCESO";
        public string Coprcaestado = "COPRCAESTADO";
        public string Coprcafecinicio = "COPRCAFECINICIO";
        public string Coprcafecfin = "COPRCAFECFIN";
        public string Pericodi = "PERICODI";
        public string Vcrecaversion = "VCRECAVERSION";
        public string Copercodi = "COPERCODI";
        public string Covercodi = "COVERCODI";
        public string Coprcafuentedato = "COPRCAFUENTEDATO";

        public string SqlValidarExistencia
        {
            get { return base.GetSqlXml("ValidarExistencia"); }
        }

        #endregion
    }
}
