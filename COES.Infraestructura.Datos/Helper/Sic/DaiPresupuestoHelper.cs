using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla DAI_PRESUPUESTO
    /// </summary>
    public class DaiPresupuestoHelper : HelperBase
    {
        public DaiPresupuestoHelper(): base(Consultas.DaiPresupuestoSql)
        {
        }

        public DaiPresupuestoDTO Create(IDataReader dr)
        {
            DaiPresupuestoDTO entity = new DaiPresupuestoDTO();

            int iPrescodi = dr.GetOrdinal(this.Prescodi);
            if (!dr.IsDBNull(iPrescodi)) entity.Prescodi = Convert.ToInt32(dr.GetValue(iPrescodi));

            int iPresanio = dr.GetOrdinal(this.Presanio);
            if (!dr.IsDBNull(iPresanio)) entity.Presanio = dr.GetString(iPresanio);

            int iPresmonto = dr.GetOrdinal(this.Presmonto);
            if (!dr.IsDBNull(iPresmonto)) entity.Presmonto = dr.GetDecimal(iPresmonto);

            int iPresamortizacion = dr.GetOrdinal(this.Presamortizacion);
            if (!dr.IsDBNull(iPresamortizacion)) entity.Presamortizacion = Convert.ToInt32(dr.GetValue(iPresamortizacion));

            int iPresinteres = dr.GetOrdinal(this.Presinteres);
            if (!dr.IsDBNull(iPresinteres)) entity.Presinteres = dr.GetDecimal(iPresinteres);

            int iPresactivo = dr.GetOrdinal(this.Presactivo);
            if (!dr.IsDBNull(iPresactivo)) entity.Presactivo = dr.GetString(iPresactivo);

            int iPresusucreacion = dr.GetOrdinal(this.Presusucreacion);
            if (!dr.IsDBNull(iPresusucreacion)) entity.Presusucreacion = dr.GetString(iPresusucreacion);

            int iPresfeccreacion = dr.GetOrdinal(this.Presfeccreacion);
            if (!dr.IsDBNull(iPresfeccreacion)) entity.Presfeccreacion = dr.GetDateTime(iPresfeccreacion);

            int iPresusumodificacion = dr.GetOrdinal(this.Presusumodificacion);
            if (!dr.IsDBNull(iPresusumodificacion)) entity.Presusumodificacion = dr.GetString(iPresusumodificacion);

            int iPresfecmodificacion = dr.GetOrdinal(this.Presfecmodificacion);
            if (!dr.IsDBNull(iPresfecmodificacion)) entity.Presfecmodificacion = dr.GetDateTime(iPresfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Prescodi = "PRESCODI";
        public string Presanio = "PRESANIO";
        public string Presmonto = "PRESMONTO";
        public string Presamortizacion = "PRESAMORTIZACION";
        public string Presinteres = "PRESINTERES";
        public string Presactivo = "PRESACTIVO";
        public string Presusucreacion = "PRESUSUCREACION";
        public string Presfeccreacion = "PRESFECCREACION";
        public string Presusumodificacion = "PRESUSUMODIFICACION";
        public string Presfecmodificacion = "PRESFECMODIFICACION";

        public string Tieneaportantes = "Tieneaportantes";
        public string Presprocesada = "PRESPROCESADA";
        
        #endregion
    }
}
