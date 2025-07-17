using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EXT_LOGENVIO
    /// </summary>
    public class ExtLogenvioHelper : HelperBase
    {
        public ExtLogenvioHelper()
            : base(Consultas.ExtLogenvioSql)
        {
        }

        public ExtLogenvioDTO Create(IDataReader dr)
        {
            ExtLogenvioDTO entity = new ExtLogenvioDTO();

            int iLogcodi = dr.GetOrdinal(this.Logcodi);
            if (!dr.IsDBNull(iLogcodi)) entity.Logcodi = Convert.ToInt32(dr.GetValue(iLogcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iFilenomb = dr.GetOrdinal(this.Filenomb);
            if (!dr.IsDBNull(iFilenomb)) entity.Filenomb = dr.GetString(iFilenomb);

            int iOriglectcodi = dr.GetOrdinal(this.Origlectcodi);
            if (!dr.IsDBNull(iOriglectcodi)) entity.Origlectcodi = Convert.ToInt32(dr.GetValue(iOriglectcodi));

            int iLectcodi = dr.GetOrdinal(this.Lectcodi);
            if (!dr.IsDBNull(iLectcodi)) entity.Lectcodi = Convert.ToInt32(dr.GetValue(iLectcodi));

            int iEstenvcodi = dr.GetOrdinal(this.Estenvcodi);
            if (!dr.IsDBNull(iEstenvcodi)) entity.Estenvcodi = Convert.ToInt32(dr.GetValue(iEstenvcodi));

            int iFeccarga = dr.GetOrdinal(this.Feccarga);
            if (!dr.IsDBNull(iFeccarga)) entity.Feccarga = dr.GetDateTime(iFeccarga);

            int iNrosemana = dr.GetOrdinal(this.Nrosemana);
            if (!dr.IsDBNull(iNrosemana)) entity.Nrosemana = Convert.ToInt32(dr.GetValue(iNrosemana));

            int iFecproceso = dr.GetOrdinal(this.Fecproceso);
            if (!dr.IsDBNull(iFecproceso)) entity.Fecproceso = dr.GetDateTime(iFecproceso);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            int iNroAnio = dr.GetOrdinal(this.NroAnio);
            if (!dr.IsDBNull(iNroAnio)) entity.NroAnio = Convert.ToInt32(dr.GetValue(iNroAnio));

            return entity;
        }


        #region Mapeo de Campos

        public string Logcodi = "LOGCODI";
        public string Emprcodi = "EMPRCODI";
        public string Filenomb = "FILENOMB";
        public string Origlectcodi = "ORIGLECTCODI";
        public string Lectcodi = "LECTCODI";
        public string Estenvcodi = "ESTENVCODI";
        public string Feccarga = "FECCARGA";
        public string Nrosemana = "NROSEMANA";
        public string Fecproceso = "FECPROCESO";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";
        public string EmprNomb = "EMPRNOMB";
        public string EstEnvNomb = "ESTENVNOMB";
        public string NroAnio = "NROANIO";

        #endregion
    }
}

