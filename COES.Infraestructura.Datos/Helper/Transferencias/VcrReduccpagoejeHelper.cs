using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla VCR_REDUCCPAGOEJE
    /// </summary>
    public class VcrReduccpagoejeHelper : HelperBase
    {
        public VcrReduccpagoejeHelper(): base(Consultas.VcrReduccpagoejeSql)
        {
        }

        public VcrReduccpagoejeDTO Create(IDataReader dr)
        {
            VcrReduccpagoejeDTO entity = new VcrReduccpagoejeDTO();

            int iVcrpecodi = dr.GetOrdinal(this.Vcrpecodi);
            if (!dr.IsDBNull(iVcrpecodi)) entity.Vcrpecodi = Convert.ToInt32(dr.GetValue(iVcrpecodi));

            int iVcrecacodi = dr.GetOrdinal(this.Vcrecacodi);
            if (!dr.IsDBNull(iVcrecacodi)) entity.Vcrecacodi = Convert.ToInt32(dr.GetValue(iVcrecacodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iVcrpecumplmes = dr.GetOrdinal(this.Vcrpecumplmes);
            if (!dr.IsDBNull(iVcrpecumplmes)) entity.Vcrpecumplmes = dr.GetDecimal(iVcrpecumplmes);

            int iVcrpereduccpagomax = dr.GetOrdinal(this.Vcrpereduccpagomax);
            if (!dr.IsDBNull(iVcrpereduccpagomax)) entity.Vcrpereduccpagomax = dr.GetDecimal(iVcrpereduccpagomax);

            int iVcrpereduccpagoeje = dr.GetOrdinal(this.Vcrpereduccpagoeje);
            if (!dr.IsDBNull(iVcrpereduccpagoeje)) entity.Vcrpereduccpagoeje = dr.GetDecimal(iVcrpereduccpagoeje);

            int iVcrpeusucreacion = dr.GetOrdinal(this.Vcrpeusucreacion);
            if (!dr.IsDBNull(iVcrpeusucreacion)) entity.Vcrpeusucreacion = dr.GetString(iVcrpeusucreacion);

            int iVcrpefeccreacion = dr.GetOrdinal(this.Vcrpefeccreacion);
            if (!dr.IsDBNull(iVcrpefeccreacion)) entity.Vcrpefeccreacion = dr.GetDateTime(iVcrpefeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Vcrpecodi = "VCRPECODI";
        public string Vcrecacodi = "VCRECACODI";
        public string Equicodi = "EQUICODI";
        public string Vcrpecumplmes = "VCRPECUMPLMES";
        public string Vcrpereduccpagomax = "VCRPEREDUCCPAGOMAX";
        public string Vcrpereduccpagoeje = "VCRPEREDUCCPAGOEJE";
        public string Vcrpeusucreacion = "VCRPEUSUCREACION";
        public string Vcrpefeccreacion = "VCRPEFECCREACION";

        #endregion
    }
}
