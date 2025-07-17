using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PSU_RPFHID
    /// </summary>
    public class PsuRpfhidHelper : HelperBase
    {
        public PsuRpfhidHelper(): base(Consultas.PsuRpfhidSql)
        {
        }

        public PsuRpfhidDTO Create(IDataReader dr)
        {
            PsuRpfhidDTO entity = new PsuRpfhidDTO();

            int iRpfhidfecha = dr.GetOrdinal(this.Rpfhidfecha);
            if (!dr.IsDBNull(iRpfhidfecha)) entity.Rpfhidfecha = dr.GetDateTime(iRpfhidfecha);

            int iRpfenetotal = dr.GetOrdinal(this.Rpfenetotal);
            if (!dr.IsDBNull(iRpfenetotal)) entity.Rpfenetotal = dr.GetDecimal(iRpfenetotal);

            int iRpfpotmedia = dr.GetOrdinal(this.Rpfpotmedia);
            if (!dr.IsDBNull(iRpfpotmedia)) entity.Rpfpotmedia = dr.GetDecimal(iRpfpotmedia);

            int iEneindhidra = dr.GetOrdinal(this.Eneindhidra);
            if (!dr.IsDBNull(iEneindhidra)) entity.Eneindhidra = dr.GetDecimal(iEneindhidra);

            int iPotindhidra = dr.GetOrdinal(this.Potindhidra);
            if (!dr.IsDBNull(iPotindhidra)) entity.Potindhidra = dr.GetDecimal(iPotindhidra);

            int iLastUser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastUser)) entity.Lastuser = dr.GetString(iLastUser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);


            return entity;
        }


        #region Mapeo de Campos

        public string Rpfhidfecha = "RPFHIDFECHA";
        public string Rpfenetotal = "RPFENETOTAL";
        public string Rpfpotmedia = "RPFPOTMEDIA";
        public string Eneindhidra = "ENEINDHIDRA";
        public string Potindhidra = "POTINDHIDRA";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";

        #endregion
    }
}
