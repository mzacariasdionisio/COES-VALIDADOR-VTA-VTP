using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System.Data;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla TRN_CONFIGURACION_PMME
    /// </summary>
    public class TrnConfiguracionPmmeHelper : HelperBase
    {
        #region Constructor
        public TrnConfiguracionPmmeHelper() : base(Consultas.TrnConfiguracionPmmeSql)
        {

        }
        #endregion

        #region Helpers Trn_configuracion_pmme
        public TrnConfiguracionPmmeDTO Create(IDataReader dr)
        {
            TrnConfiguracionPmmeDTO entity = new TrnConfiguracionPmmeDTO();

            #region Campos Originales
            // CONFCONCODI
            int iConfconcodi = dr.GetOrdinal(this.Confconcodi);
            if (!dr.IsDBNull(iConfconcodi)) entity.Confconcodi = dr.GetInt32(iConfconcodi);

            // PTOMEDICODI
            int iPtomedicodi = dr.GetOrdinal(this.Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = dr.GetInt32(iPtomedicodi);

            // EMPRCODI
            int iEmprCodi = dr.GetOrdinal(this.EmprCodi);
            if (!dr.IsDBNull(iEmprCodi)) entity.Emprcodi = dr.GetInt32(iEmprCodi);

            // FECHAVIGENCIA
            int iFechavigencia = dr.GetOrdinal(this.Fechavigencia);
            if (!dr.IsDBNull(iFechavigencia)) entity.Fechavigencia = dr.GetDateTime(iFechavigencia);

            // VIGENCIA
            int iVigencia = dr.GetOrdinal(this.Vigencia);
            if (!dr.IsDBNull(iVigencia)) entity.Vigencia = dr.GetString(iVigencia);

            // LASTUSER
            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            // LASTDATE
            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            #endregion

            return entity;
        }
        #endregion

        #region Mapeo de Campos Trn_configuracion_pmme

        #region Campos Originales
        public string Confconcodi = "CONFCONCODI";
        public string Ptomedicodi = "PTOMEDICODI";
        public string EmprCodi = "EMPRCODI";
        public string Fechavigencia = "FECHAVIGENCIA";
        public string Vigencia = "VIGENCIA";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";
        public string Estado = "ESTADO";
        #endregion

        #region Campos Adicionales
        public string EmprNomb = "EMPRNOMB";
        public string PtoMediDesc = "PTOMEDIDESC";
        #endregion

        #endregion

        #region Querys SQL Trn_configuracion_pmme

        public string SqlValidarExistencia
        {
            get { return base.GetSqlXml("ValidarExistencia"); }
        }

        public string SqlListxEmpresa
        {
            get { return base.GetSqlXml("ListxEmpresa"); }
        }
        public string SqlListTrnConfiguracionxVigencia
        {
            get { return base.GetSqlXml("ListTrnConfiguracionxVigencia"); }
        }
        #endregion
    }
}
