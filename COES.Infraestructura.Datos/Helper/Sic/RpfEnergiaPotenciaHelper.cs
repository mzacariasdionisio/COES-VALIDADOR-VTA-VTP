using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    
    public class RpfEnergiaPotenciaHelper : HelperBase
    {
        public RpfEnergiaPotenciaHelper()
            : base(Consultas.RpfEnergiaPotenciaSql)
        {
        }

        public RpfEnergiaPotenciaDTO Create(IDataReader dr)
        {
            RpfEnergiaPotenciaDTO entity = new RpfEnergiaPotenciaDTO();
            Debug.WriteLine("ini");
            int iRpfenetotal = dr.GetOrdinal(this.Rpfenetotal);
            if (!dr.IsDBNull(iRpfenetotal)) entity.Rpfenetotal = Convert.ToDecimal(dr.GetValue(iRpfenetotal));


            int iRpfpotmedia = dr.GetOrdinal(this.Rpfpotmedia);
            if (!dr.IsDBNull(iRpfpotmedia)) entity.Rpfpotmedia = Convert.ToDecimal(dr.GetValue(iRpfpotmedia));

            int iEneindhidra = dr.GetOrdinal(this.Eneindhidra);
            if (!dr.IsDBNull(iEneindhidra)) entity.Eneindhidra = Convert.ToDecimal(dr.GetValue(iEneindhidra));

            int iPotindhidra = dr.GetOrdinal(this.Potindhidra);
            if (!dr.IsDBNull(iPotindhidra)) entity.Potindhidra = Convert.ToDecimal(dr.GetValue(iPotindhidra));

            int iRpfhidfecha = dr.GetOrdinal(this.Rpfhidfecha);
            if (!dr.IsDBNull(iRpfhidfecha)) entity.Rpfhidfecha = dr.GetDateTime(iRpfhidfecha);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            Debug.WriteLine("fin");




            return entity;
        }


        #region Mapeo de Campos



        public string Rpfenetotal = "RPFENETOTAL";
        public string Rpfhidfecha = "RPFHIDFECHA";
        public string RpfhidfechaIni = "RPFHIDFECHAINI";
        public string RpfhidfechaFin = "RPFHIDFECHAFIN";
        public string Rpfpotmedia = "RPFPOTMEDIA";
        public string Eneindhidra = "ENEINDHIDRA";
        public string Potindhidra = "POTINDHIDRA";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";

        #endregion


    }
}

