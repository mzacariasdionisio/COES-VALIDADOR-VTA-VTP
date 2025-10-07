using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PR_RESTRIC_CFG
    /// </summary>
    public class PrRestricCfgHelper : HelperBase
    {
        public PrRestricCfgHelper(): base(Consultas.PrRestricCfgSql)
        {
        }

        public PrRestricCfgDTO Create(IDataReader dr)
        {
            PrRestricCfgDTO entity = new PrRestricCfgDTO();

            int iRescfgcodi = dr.GetOrdinal(this.Rescfgcodi);
            if (!dr.IsDBNull(iRescfgcodi)) entity.Rescfgcodi = Convert.ToInt32(dr.GetValue(iRescfgcodi));

            int iRescfgnombre = dr.GetOrdinal(this.Rescfgnombre);
            if (!dr.IsDBNull(iRescfgnombre)) entity.Rescfgnombre = dr.GetString(iRescfgnombre);

            int iRescfgusucreacion = dr.GetOrdinal(this.Rescfgusucreacion);
            if (!dr.IsDBNull(iRescfgusucreacion)) entity.Rescfgusucreacion = dr.GetString(iRescfgusucreacion);

            int iRescfgfeccreacion = dr.GetOrdinal(this.Rescfgfeccreacion);
            if (!dr.IsDBNull(iRescfgfeccreacion)) entity.Rescfgfeccreacion = dr.GetDateTime(iRescfgfeccreacion);

            int iRescfgusumodificacion = dr.GetOrdinal(this.Rescfgusumodificacion);
            if (!dr.IsDBNull(iRescfgusumodificacion)) entity.Rescfgusumodificacion = dr.GetString(iRescfgusumodificacion);

            int iRescfgestado = dr.GetOrdinal(this.Rescfgestado);
            if (!dr.IsDBNull(iRescfgestado)) entity.Rescfgestado = dr.GetString(iRescfgestado);

            int iRestipcodi = dr.GetOrdinal(this.Restipcodi);
            if (!dr.IsDBNull(iRestipcodi)) entity.Restipcodi = Convert.ToInt32(dr.GetValue(iRestipcodi));

            int iRescfgfecmodificacion = dr.GetOrdinal(this.Rescfgfecmodificacion);
            if (!dr.IsDBNull(iRescfgfecmodificacion)) entity.Rescfgfecmodificacion = dr.GetDateTime(iRescfgfecmodificacion);

            int iRescfgtipoecuacion = dr.GetOrdinal(this.Rescfgtipoecuacion);
            if (!dr.IsDBNull(iRescfgtipoecuacion)) entity.Rescfgtipoecuacion = dr.GetString(iRescfgtipoecuacion);

            int iRescfges = dr.GetOrdinal(this.Rescfges);
            if (!dr.IsDBNull(iRescfges)) entity.Rescfges = dr.GetDecimal(iRescfges);

            int iRescfgfs = dr.GetOrdinal(this.Rescfgfs);
            if (!dr.IsDBNull(iRescfgfs)) entity.Rescfgfs = dr.GetDecimal(iRescfgfs);

            return entity;
        }


        #region Mapeo de Campos

        public string Rescfgcodi = "RESCFGCODI";
        public string Rescfgnombre = "RESCFGNOMBRE";
        public string Rescfgusucreacion = "RESCFGUSUCREACION";
        public string Rescfgfeccreacion = "RESCFGFECCREACION";
        public string Rescfgusumodificacion = "RESCFGUSUMODIFICACION";
        public string Rescfgestado = "RESCFGESTADO";
        public string Restipcodi = "RESTIPCODI";
        public string Rescfgfecmodificacion = "RESCFGFECMODIFICACION";
        public string Rescfgtipoecuacion = "RESCFGTIPOECUACION";
        public string Rescfges = "RESCFGES";
        public string Rescfgfs = "RESCFGFS";

        #endregion

        public string SqlActualizarEstadoRegistro
        {
            get { return GetSqlXml("ActualizarEstadoRegistro"); }
        }

    }
}
