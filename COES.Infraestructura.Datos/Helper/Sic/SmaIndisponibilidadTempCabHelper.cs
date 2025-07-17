using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SMA_INDISPONIBILIDAD_TEMP_CAB
    /// </summary>
    public class SmaIndisponibilidadTempCabHelper : HelperBase
    {
        public SmaIndisponibilidadTempCabHelper(): base(Consultas.SmaIndisponibilidadTempCabSql)
        {
        }

        public SmaIndisponibilidadTempCabDTO Create(IDataReader dr)
        {
            SmaIndisponibilidadTempCabDTO entity = new SmaIndisponibilidadTempCabDTO();

            int iIntcabcodi = dr.GetOrdinal(this.Intcabcodi);
            if (!dr.IsDBNull(iIntcabcodi)) entity.Intcabcodi = Convert.ToInt32(dr.GetValue(iIntcabcodi));

            int iIntcabfecha = dr.GetOrdinal(this.Intcabfecha);
            if (!dr.IsDBNull(iIntcabfecha)) entity.Intcabfecha = dr.GetDateTime(iIntcabfecha);

            int iIntcabusucreacion = dr.GetOrdinal(this.Intcabusucreacion);
            if (!dr.IsDBNull(iIntcabusucreacion)) entity.Intcabusucreacion = dr.GetString(iIntcabusucreacion);

            int iIntcabfeccreacion = dr.GetOrdinal(this.Intcabfeccreacion);
            if (!dr.IsDBNull(iIntcabfeccreacion)) entity.Intcabfeccreacion = dr.GetDateTime(iIntcabfeccreacion);

            int iIntcabusumodificacion = dr.GetOrdinal(this.Intcabusumodificacion);
            if (!dr.IsDBNull(iIntcabusumodificacion)) entity.Intcabusumodificacion = dr.GetString(iIntcabusumodificacion);

            int iIntcabfecmodificacion = dr.GetOrdinal(this.Intcabfecmodificacion);
            if (!dr.IsDBNull(iIntcabfecmodificacion)) entity.Intcabfecmodificacion = dr.GetDateTime(iIntcabfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Intcabcodi = "INTCABCODI";
        public string Intcabfecha = "INTCABFECHA";
        public string Intcabusucreacion = "INTCABUSUCREACION";
        public string Intcabfeccreacion = "INTCABFECCREACION";
        public string Intcabusumodificacion = "INTCABUSUMODIFICACION";
        public string Intcabfecmodificacion = "INTCABFECMODIFICACION";

        #endregion

        public string SqlObtenerPorFecha
        {
            get { return base.GetSqlXml("ObtenerPorFecha"); }
        }
    }
}
