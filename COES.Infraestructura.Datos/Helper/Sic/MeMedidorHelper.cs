using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ME_MEDIDOR
    /// </summary>
    public class MeMedidorHelper : HelperBase
    {
        public MeMedidorHelper(): base(Consultas.MeMedidorSql)
        {
        }

        public MeMedidorDTO Create(IDataReader dr)
        {
            MeMedidorDTO entity = new MeMedidorDTO();

            int iMedicodi = dr.GetOrdinal(this.Medicodi);
            if (!dr.IsDBNull(iMedicodi)) entity.Medicodi = Convert.ToInt32(dr.GetValue(iMedicodi));

            int iMedinombre = dr.GetOrdinal(this.Medinombre);
            if (!dr.IsDBNull(iMedinombre)) entity.Medinombre = dr.GetString(iMedinombre);           

            int iPtomedicodi = dr.GetOrdinal(this.Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

            int iMeditipo = dr.GetOrdinal(this.Meditipo);
            if (!dr.IsDBNull(iMeditipo)) entity.Meditipo = dr.GetString(iMeditipo);

            return entity;
        }


        #region Mapeo de Campos

        public string Medicodi = "MEDICODI";
        public string Medinombre = "MEDINOMBRE";        
        public string Ptomedicodi = "PTOMEDICODI";
        public string Meditipo = "MEDITIPO";

        #endregion
    }
}
