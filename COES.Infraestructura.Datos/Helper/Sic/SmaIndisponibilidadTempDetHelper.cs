using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SMA_INDISPONIBILIDAD_TEMP_DET
    /// </summary>
    public class SmaIndisponibilidadTempDetHelper : HelperBase
    {
        public SmaIndisponibilidadTempDetHelper(): base(Consultas.SmaIndisponibilidadTempDetSql)
        {
        }

        public SmaIndisponibilidadTempDetDTO Create(IDataReader dr)
        {
            SmaIndisponibilidadTempDetDTO entity = new SmaIndisponibilidadTempDetDTO();

            int iIntdetcodi = dr.GetOrdinal(this.Intdetcodi);
            if (!dr.IsDBNull(iIntdetcodi)) entity.Intdetcodi = Convert.ToInt32(dr.GetValue(iIntdetcodi));

            int iIntcabcodi = dr.GetOrdinal(this.Intcabcodi);
            if (!dr.IsDBNull(iIntcabcodi)) entity.Intcabcodi = Convert.ToInt32(dr.GetValue(iIntcabcodi));

            int iUrscodi = dr.GetOrdinal(this.Urscodi);
            if (!dr.IsDBNull(iUrscodi)) entity.Urscodi = Convert.ToInt32(dr.GetValue(iUrscodi));

            int iIntdetindexiste = dr.GetOrdinal(this.Intdetindexiste);
            if (!dr.IsDBNull(iIntdetindexiste)) entity.Intdetindexiste = dr.GetString(iIntdetindexiste);

            int iIntdettipo = dr.GetOrdinal(this.Intdettipo);
            if (!dr.IsDBNull(iIntdettipo)) entity.Intdettipo = dr.GetString(iIntdettipo);

            int iIntdetbanda = dr.GetOrdinal(this.Intdetbanda);
            if (!dr.IsDBNull(iIntdetbanda)) entity.Intdetbanda = dr.GetDecimal(iIntdetbanda);

            int iIntdetmotivo = dr.GetOrdinal(this.Intdetmotivo);
            if (!dr.IsDBNull(iIntdetmotivo)) entity.Intdetmotivo = dr.GetString(iIntdetmotivo);

            return entity;
        }


        #region Mapeo de Campos

        public string Intdetcodi = "INTDETCODI";
        public string Intcabcodi = "INTCABCODI";
        public string Urscodi = "URSCODI";
        public string Intdetindexiste = "INTDETINDEXISTE";
        public string Intdettipo = "INTDETTIPO";
        public string Intdetbanda = "INTDETBANDA";
        public string Intdetmotivo = "INTDETMOTIVO";

        #endregion

        public string SqlListarPorFecha
        {
            get { return base.GetSqlXml("ListarPorFecha"); }
        }

        public string SqlDeletePorIdsCab
        {
            get { return base.GetSqlXml("DeletePorIdsCab"); }
        }
    }
}
