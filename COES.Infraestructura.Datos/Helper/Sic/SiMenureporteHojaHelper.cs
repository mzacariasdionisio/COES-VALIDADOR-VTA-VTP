using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_MENUREPORTE_HOJA
    /// </summary>
    public class SiMenureporteHojaHelper : HelperBase
    {
        public SiMenureporteHojaHelper() : base(Consultas.SiMenureporteHojaSql)
        {
        }

        public SiMenureporteHojaDTO Create(IDataReader dr)
        {
            SiMenureporteHojaDTO entity = new SiMenureporteHojaDTO();

            int iMrephcodi = dr.GetOrdinal(this.Mrephcodi);
            if (!dr.IsDBNull(iMrephcodi)) entity.Mrephcodi = Convert.ToInt32(dr.GetValue(iMrephcodi));

            int iMrephnombre = dr.GetOrdinal(this.Mrephnombre);
            if (!dr.IsDBNull(iMrephnombre)) entity.Mrephnombre = dr.GetString(iMrephnombre);

            int iMrephestado = dr.GetOrdinal(this.Mrephestado);
            if (!dr.IsDBNull(iMrephestado)) entity.Mrephestado = Convert.ToInt32(dr.GetValue(iMrephestado));

            int iMrephvisible = dr.GetOrdinal(this.Mrephvisible);
            if (!dr.IsDBNull(iMrephvisible)) entity.Mrephvisible = Convert.ToInt32(dr.GetValue(iMrephvisible));

            int iMrephorden = dr.GetOrdinal(this.Mrephorden);
            if (!dr.IsDBNull(iMrephorden)) entity.Mrephorden = Convert.ToInt32(dr.GetValue(iMrephorden));

            int iMrepcodi = dr.GetOrdinal(this.Mrepcodi);
            if (!dr.IsDBNull(iMrepcodi)) entity.Mrepcodi = Convert.ToInt32(dr.GetValue(iMrepcodi));

            return entity;
        }

        #region Mapeo de Campos

        public string Mrephcodi = "MREPHCODI";
        public string Mrephnombre = "MREPHNOMBRE";
        public string Mrephestado = "MREPHESTADO";
        public string Mrephvisible = "MREPHVISIBLE";
        public string Mrephorden = "MREPHORDEN";
        public string Mrepcodi = "MREPCODI";

        public string Repdescripcion = "MREPDESCRIPCION";

        #endregion
    }
}
