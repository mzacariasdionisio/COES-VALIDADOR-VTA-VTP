using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RCA_REGISTRO_SVRM
    /// </summary>
    public class RcaRegistroSvrmHelper : HelperBase
    {
        public RcaRegistroSvrmHelper(): base(Consultas.RcaRegistroSvrmSql)
        {
        }

        public RcaRegistroSvrmDTO Create(IDataReader dr)
        {
            RcaRegistroSvrmDTO entity = new RcaRegistroSvrmDTO();

            int iRcsvrmcodi = dr.GetOrdinal(this.Rcsvrmcodi);
            if (!dr.IsDBNull(iRcsvrmcodi)) entity.Rcsvrmcodi = Convert.ToInt32(dr.GetValue(iRcsvrmcodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iRcsvrmhperacmf = dr.GetOrdinal(this.Rcsvrmhperacmf);
            if (!dr.IsDBNull(iRcsvrmhperacmf)) entity.Rcsvrmhperacmf = dr.GetDecimal(iRcsvrmhperacmf);

            int iRcsvrmhperacmt = dr.GetOrdinal(this.Rcsvrmhperacmt);
            if (!dr.IsDBNull(iRcsvrmhperacmt)) entity.Rcsvrmhperacmt = dr.GetDecimal(iRcsvrmhperacmt);

            int iRcsvrmhfperacmf = dr.GetOrdinal(this.Rcsvrmhfperacmf);
            if (!dr.IsDBNull(iRcsvrmhfperacmf)) entity.Rcsvrmhfperacmf = dr.GetDecimal(iRcsvrmhfperacmf);

            int iRcsvrmhfperacmt = dr.GetOrdinal(this.Rcsvrmhfperacmt);
            if (!dr.IsDBNull(iRcsvrmhfperacmt)) entity.Rcsvrmhfperacmt = dr.GetDecimal(iRcsvrmhfperacmt);

            int iRcsvrmmaxdemcont = dr.GetOrdinal(this.Rcsvrmmaxdemcont);
            if (!dr.IsDBNull(iRcsvrmmaxdemcont)) entity.Rcsvrmmaxdemcont = dr.GetDecimal(iRcsvrmmaxdemcont);

            int iRcsvrmmaxdemdisp = dr.GetOrdinal(this.Rcsvrmmaxdemdisp);
            if (!dr.IsDBNull(iRcsvrmmaxdemdisp)) entity.Rcsvrmmaxdemdisp = dr.GetDecimal(iRcsvrmmaxdemdisp);

            int iRcsvrmmaxdemcomp = dr.GetOrdinal(this.Rcsvrmmaxdemcomp);
            if (!dr.IsDBNull(iRcsvrmmaxdemcomp)) entity.Rcsvrmmaxdemcomp = dr.GetDecimal(iRcsvrmmaxdemcomp);

            int iRcsvrmdocumento = dr.GetOrdinal(this.Rcsvrmdocumento);
            if (!dr.IsDBNull(iRcsvrmdocumento)) entity.Rcsvrmdocumento = dr.GetString(iRcsvrmdocumento);

            int iRcsvrmfechavigencia = dr.GetOrdinal(this.Rcsvrmfechavigencia);
            if (!dr.IsDBNull(iRcsvrmfechavigencia)) entity.Rcsvrmfechavigencia = dr.GetDateTime(iRcsvrmfechavigencia);

            int iRcsvrmestado = dr.GetOrdinal(this.Rcsvrmestado);
            if (!dr.IsDBNull(iRcsvrmestado)) entity.Rcsvrmestado = dr.GetString(iRcsvrmestado);

            int iRcsvrmestregistro = dr.GetOrdinal(this.Rcsvrmestregistro);
            if (!dr.IsDBNull(iRcsvrmestregistro)) entity.Rcsvrmestregistro = dr.GetString(iRcsvrmestregistro);

            int iRcsvrmusucreacion = dr.GetOrdinal(this.Rcsvrmusucreacion);
            if (!dr.IsDBNull(iRcsvrmusucreacion)) entity.Rcsvrmusucreacion = dr.GetString(iRcsvrmusucreacion);

            int iRcsvrmfeccreacion = dr.GetOrdinal(this.Rcsvrmfeccreacion);
            if (!dr.IsDBNull(iRcsvrmfeccreacion)) entity.Rcsvrmfeccreacion = dr.GetDateTime(iRcsvrmfeccreacion);

            int iRcsvrmusumodificacion = dr.GetOrdinal(this.Rcsvrmusumodificacion);
            if (!dr.IsDBNull(iRcsvrmusumodificacion)) entity.Rcsvrmusumodificacion = dr.GetString(iRcsvrmusumodificacion);

            int iRcsvrmfecmodificacion = dr.GetOrdinal(this.Rcsvrmfecmodificacion);
            if (!dr.IsDBNull(iRcsvrmfecmodificacion)) entity.Rcsvrmfecmodificacion = dr.GetDateTime(iRcsvrmfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Rcsvrmcodi = "RCSVRMCODI";
        public string Equicodi = "EQUICODI";
        public string Rcsvrmhperacmf = "RCSVRMHPERACMF";
        public string Rcsvrmhperacmt = "RCSVRMHPERACMT";
        public string Rcsvrmhfperacmf = "RCSVRMHFPERACMF";
        public string Rcsvrmhfperacmt = "RCSVRMHFPERACMT";
        public string Rcsvrmmaxdemcont = "RCSVRMMAXDEMCONT";
        public string Rcsvrmmaxdemdisp = "RCSVRMMAXDEMDISP";
        public string Rcsvrmmaxdemcomp = "RCSVRMMAXDEMCOMP";
        public string Rcsvrmdocumento = "RCSVRMDOCUMENTO";
        public string Rcsvrmfechavigencia = "RCSVRMFECHAVIGENCIA";
        public string Rcsvrmestado = "RCSVRMESTADO";
        public string Rcsvrmestregistro = "RCSVRMESTREGISTRO";
        public string Rcsvrmusucreacion = "RCSVRMUSUCREACION";
        public string Rcsvrmfeccreacion = "RCSVRMFECCREACION";
        public string Rcsvrmusumodificacion = "RCSVRMUSUMODIFICACION";
        public string Rcsvrmfecmodificacion = "RCSVRMFECMODIFICACION";

        public string Emprrazsocial = "EMPRRAZSOCIAL";
        public string Tipoemprcodi = "TIPOEMPRCODI";
        public string Equinomb = "EQUINOMB";
        public string Areaabrev = "AREAABREV";
        public string Areanomb = "AREANOMB";

        public string Osinergcodi = "OSINERGCODI";

        public string Emprsum = "EMPRSUM";

        public string Qregistros = "Q_REGISTROS";

        #endregion

        public string SqlListFiltro
        {
            get { return base.GetSqlXml("ListFiltro"); }
        }

        public string SqlObtenerPorCodigo
        {
            get { return base.GetSqlXml("ObtenerPorCodigo"); }
        }

        public string SqlListFiltroExcel
        {
            get { return base.GetSqlXml("ListFiltroExcel"); }
        }

        public string SqlListFiltroCount
        {
            get { return base.GetSqlXml("ListFiltroCount"); }
        }
    }
}
