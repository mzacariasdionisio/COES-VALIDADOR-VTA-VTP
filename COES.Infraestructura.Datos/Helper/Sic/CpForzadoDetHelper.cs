using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CP_FORZADO_DET
    /// </summary>
    public class CpForzadoDetHelper : HelperBase
    {
        public CpForzadoDetHelper() : base(Consultas.CpForzadoDetSql)
        {
        }

        public CpForzadoDetDTO Create(IDataReader dr)
        {
            CpForzadoDetDTO entity = new CpForzadoDetDTO();

            int iCpfzdtcodi = dr.GetOrdinal(this.Cpfzdtcodi);
            if (!dr.IsDBNull(iCpfzdtcodi)) entity.Cpfzdtcodi = Convert.ToInt32(dr.GetValue(iCpfzdtcodi));

            int iCpfzcodi = dr.GetOrdinal(this.Cpfzcodi);
            if (!dr.IsDBNull(iCpfzcodi)) entity.Cpfzcodi = Convert.ToInt32(dr.GetValue(iCpfzcodi));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iCpfzdtperiodoini = dr.GetOrdinal(this.Cpfzdtperiodoini);
            if (!dr.IsDBNull(iCpfzdtperiodoini)) entity.Cpfzdtperiodoini = Convert.ToInt32(dr.GetValue(iCpfzdtperiodoini));

            int iCpfzdtperiodofin = dr.GetOrdinal(this.Cpfzdtperiodofin);
            if (!dr.IsDBNull(iCpfzdtperiodofin)) entity.Cpfzdtperiodofin = Convert.ToInt32(dr.GetValue(iCpfzdtperiodofin));

            int iCpfzdtflagcreacion = dr.GetOrdinal(this.Cpfzdtflagcreacion);
            if (!dr.IsDBNull(iCpfzdtflagcreacion)) entity.Cpfzdtflagcreacion = dr.GetString(iCpfzdtflagcreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Cpfzdtcodi = "CPFZDTCODI";
        public string Cpfzcodi = "CPFZCODI";
        public string Grupocodi = "GRUPOCODI";
        public string Equicodi = "EQUICODI";
        public string Emprcodi = "EMPRCODI";
        public string Cpfzdtperiodoini = "CPFZDTPERIODOINI";
        public string Cpfzdtperiodofin = "CPFZDTPERIODOFIN";
        public string Cpfzdtflagcreacion = "CPFZDTFLAGCREACION";

        public string Emprnomb = "EMPRNOMB";
        public string Equinomb = "EQUINOMB";
        public string Gruponomb = "GRUPONOMB";


        #endregion
        public string SqlGetBycpfzcodi => GetSqlXml("GetBycpfzcodi");


    }
}
