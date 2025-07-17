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
    public class EpoEstudioTerceroInvEpoHelper : HelperBase
    {
        public EpoEstudioTerceroInvEpoHelper(): base(Consultas.EpoEstudioTerceroInvEpoSql)
        {
        }

        public EpoEstudioTerceroInvEpoDTO Create(IDataReader dr)
        {
            EpoEstudioTerceroInvEpoDTO entity = new EpoEstudioTerceroInvEpoDTO();

            int iEstepoterinvcodi = dr.GetOrdinal(this.Invepocodi);
            if (!dr.IsDBNull(iEstepoterinvcodi)) entity.Invepocodi = Convert.ToInt32(dr.GetValue(iEstepoterinvcodi));

            int iEstepocodi = dr.GetOrdinal(this.Estepocodi);
            if (!dr.IsDBNull(iEstepocodi)) entity.Estepocodi = Convert.ToInt32(dr.GetValue(iEstepocodi));

            int iEstepoemprcodi = dr.GetOrdinal(this.Estepoemprcodi);
            if (!dr.IsDBNull(iEstepoemprcodi)) entity.Estepoemprcodi = Convert.ToInt32(dr.GetValue(iEstepoemprcodi));
            
            return entity;
        }


        #region Mapeo de Campos

        public string Invepocodi = "Invepocodi";
        public string Estepocodi = "estepocodi";
        public string Estepoemprcodi = "Estepoemprcodi";
        
        public string Lastdate = "LASTDATE";
        public string Lastuser = "LASTUSER";

        #endregion

    }
}
