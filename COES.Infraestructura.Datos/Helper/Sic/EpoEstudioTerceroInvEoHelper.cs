using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EPO_ESTUDIO_EO
    /// </summary>
    public class EpoEstudioTerceroInvEoHelper : HelperBase
    {
        public EpoEstudioTerceroInvEoHelper(): base(Consultas.EpoEstudioTerceroInvEoSql)
        {
        }

        public EpoEstudioTerceroInvEoDTO Create(IDataReader dr)
        {
            EpoEstudioTerceroInvEoDTO entity = new EpoEstudioTerceroInvEoDTO();

            int iEsteoterinvcodi = dr.GetOrdinal(this.Inveocodi);
            if (!dr.IsDBNull(iEsteoterinvcodi)) entity.Inveocodi = Convert.ToInt32(dr.GetValue(iEsteoterinvcodi));

            int iEsteocodi = dr.GetOrdinal(this.Esteocodi);
            if (!dr.IsDBNull(iEsteocodi)) entity.Esteocodi = Convert.ToInt32(dr.GetValue(iEsteocodi));

            int iEsteoemprcodi = dr.GetOrdinal(this.Esteoemprcodi);
            if (!dr.IsDBNull(iEsteoemprcodi)) entity.Esteoemprcodi = Convert.ToInt32(dr.GetValue(iEsteoemprcodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Inveocodi = "INVEOCODI";
        public string Esteocodi = "esteocodi";
        public string Esteoemprcodi = "Esteoemprcodi";
        
        public string Lastdate = "LASTDATE";
        public string Lastuser = "LASTUSER";

        #endregion

    }
}
