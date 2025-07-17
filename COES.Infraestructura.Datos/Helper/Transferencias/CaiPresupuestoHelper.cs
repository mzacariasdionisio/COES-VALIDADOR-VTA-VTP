using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CAI_PRESUPUESTO
    /// </summary>
    public class CaiPresupuestoHelper : HelperBase
    {
        public CaiPresupuestoHelper(): base(Consultas.CaiPresupuestoSql)
        {
        }

        public CaiPresupuestoDTO Create(IDataReader dr)
        {
            CaiPresupuestoDTO entity = new CaiPresupuestoDTO();

            int iCaiprscodi = dr.GetOrdinal(this.Caiprscodi);
            if (!dr.IsDBNull(iCaiprscodi)) entity.Caiprscodi = Convert.ToInt32(dr.GetValue(iCaiprscodi));

            int iCaiprsanio = dr.GetOrdinal(this.Caiprsanio);
            if (!dr.IsDBNull(iCaiprsanio)) entity.Caiprsanio = Convert.ToInt32(dr.GetValue(iCaiprsanio));

            int iCaiprsmesinicio = dr.GetOrdinal(this.Caiprsmesinicio);
            if (!dr.IsDBNull(iCaiprsmesinicio)) entity.Caiprsmesinicio = Convert.ToInt32(dr.GetValue(iCaiprsmesinicio));

            int iCaiprsnromeses = dr.GetOrdinal(this.Caiprsnromeses);
            if (!dr.IsDBNull(iCaiprsnromeses)) entity.Caiprsnromeses = Convert.ToInt32(dr.GetValue(iCaiprsnromeses));

            int iCaiprsnombre = dr.GetOrdinal(this.Caiprsnombre);
            if (!dr.IsDBNull(iCaiprsnombre)) entity.Caiprsnombre = dr.GetString(iCaiprsnombre);

            int iCaiprsusucreacion = dr.GetOrdinal(this.Caiprsusucreacion);
            if (!dr.IsDBNull(iCaiprsusucreacion)) entity.Caiprsusucreacion = dr.GetString(iCaiprsusucreacion);

            int iCaiprsfeccreacion = dr.GetOrdinal(this.Caiprsfeccreacion);
            if (!dr.IsDBNull(iCaiprsfeccreacion)) entity.Caiprsfeccreacion = dr.GetDateTime(iCaiprsfeccreacion);

            int iCaiprsusumodificacion = dr.GetOrdinal(this.Caiprsusumodificacion);
            if (!dr.IsDBNull(iCaiprsusumodificacion)) entity.Caiprsusumodificacion = dr.GetString(iCaiprsusumodificacion);

            int iCaiprsfecmodificacion = dr.GetOrdinal(this.Caiprsfecmodificacion);
            if (!dr.IsDBNull(iCaiprsfecmodificacion)) entity.Caiprsfecmodificacion = dr.GetDateTime(iCaiprsfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Caiprscodi = "CAIPRSCODI";
        public string Caiprsanio = "CAIPRSANIO";
        public string Caiprsmesinicio = "CAIPRSMESINICIO";
        public string Caiprsnromeses = "CAIPRSNROMESES";
        public string Caiprsnombre = "CAIPRSNOMBRE";
        public string Caiprsusucreacion = "CAIPRSUSUCREACION";
        public string Caiprsfeccreacion = "CAIPRSFECCREACION";
        public string Caiprsusumodificacion = "CAIPRSUSUMODIFICACION";
        public string Caiprsfecmodificacion = "CAIPRSFECMODIFICACION";

        #endregion
    }
}
