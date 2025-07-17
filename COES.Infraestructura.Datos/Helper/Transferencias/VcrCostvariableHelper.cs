using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla VCR_COSTVARIABLE
    /// </summary>
    public class VcrCostvariableHelper : HelperBase
    {
        public VcrCostvariableHelper(): base(Consultas.VcrCostvariableSql)
        {
        }

        public VcrCostvariableDTO Create(IDataReader dr)
        {
            VcrCostvariableDTO entity = new VcrCostvariableDTO();

            int iVcvarcodi = dr.GetOrdinal(this.Vcvarcodi);
            if (!dr.IsDBNull(iVcvarcodi)) entity.Vcvarcodi = Convert.ToInt32(dr.GetValue(iVcvarcodi));

            int iVcrecacodi = dr.GetOrdinal(this.Vcrecacodi);
            if (!dr.IsDBNull(iVcrecacodi)) entity.Vcrecacodi = Convert.ToInt32(dr.GetValue(iVcrecacodi));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iGruponomb = dr.GetOrdinal(this.Gruponomb);
            if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iVcvarfecha = dr.GetOrdinal(this.Vcvarfecha);
            if (!dr.IsDBNull(iVcvarfecha)) entity.Vcvarfecha = dr.GetDateTime(iVcvarfecha);

            int iVcvarcostvar = dr.GetOrdinal(this.Vcvarcostvar);
            if (!dr.IsDBNull(iVcvarcostvar)) entity.Vcvarcostvar = dr.GetDecimal(iVcvarcostvar);

            int iVcvarusucreacion = dr.GetOrdinal(this.Vcvarusucreacion);
            if (!dr.IsDBNull(iVcvarusucreacion)) entity.Vcvarusucreacion = dr.GetString(iVcvarusucreacion);

            int iVcvarfeccreacion = dr.GetOrdinal(this.Vcvarfeccreacion);
            if (!dr.IsDBNull(iVcvarfeccreacion)) entity.Vcvarfeccreacion = dr.GetDateTime(iVcvarfeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Vcvarcodi = "VCVARCODI";
        public string Vcrecacodi = "VCRECACODI";
        public string Grupocodi = "GRUPOCODI";
        public string Gruponomb = "GRUPONOMB";
        public string Equicodi = "EQUICODI";
        public string Vcvarfecha = "VCVARFECHA";
        public string Vcvarcostvar = "VCVARCOSTVAR";
        public string Vcvarusucreacion = "VCVARUSUCREACION";
        public string Vcvarfeccreacion = "VCVARFECCREACION";

        #endregion
    }
}
