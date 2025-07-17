using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RE_CAUSA_INTERRUPCION
    /// </summary>
    public class ReCausaInterrupcionHelper : HelperBase
    {
        public ReCausaInterrupcionHelper(): base(Consultas.ReCausaInterrupcionSql)
        {
        }

        public ReCausaInterrupcionDTO Create(IDataReader dr)
        {
            ReCausaInterrupcionDTO entity = new ReCausaInterrupcionDTO();

            int iRecintcodi = dr.GetOrdinal(this.Recintcodi);
            if (!dr.IsDBNull(iRecintcodi)) entity.Recintcodi = Convert.ToInt32(dr.GetValue(iRecintcodi));

            int iRetintcodi = dr.GetOrdinal(this.Retintcodi);
            if (!dr.IsDBNull(iRetintcodi)) entity.Retintcodi = Convert.ToInt32(dr.GetValue(iRetintcodi));

            int iRecintnombre = dr.GetOrdinal(this.Recintnombre);
            if (!dr.IsDBNull(iRecintnombre)) entity.Recintnombre = dr.GetString(iRecintnombre);

            int iRecintestado = dr.GetOrdinal(this.Recintestado);
            if (!dr.IsDBNull(iRecintestado)) entity.Recintestado = dr.GetString(iRecintestado);

            int iRecintusucreacion = dr.GetOrdinal(this.Recintusucreacion);
            if (!dr.IsDBNull(iRecintusucreacion)) entity.Recintusucreacion = dr.GetString(iRecintusucreacion);

            int iRecintfeccreacion = dr.GetOrdinal(this.Recintfeccreacion);
            if (!dr.IsDBNull(iRecintfeccreacion)) entity.Recintfeccreacion = dr.GetDateTime(iRecintfeccreacion);

            int iRecintusumodificacion = dr.GetOrdinal(this.Recintusumodificacion);
            if (!dr.IsDBNull(iRecintusumodificacion)) entity.Recintusumodificacion = dr.GetString(iRecintusumodificacion);

            int iRecintfecmodificacion = dr.GetOrdinal(this.Recintfecmodificacion);
            if (!dr.IsDBNull(iRecintfecmodificacion)) entity.Recintfecmodificacion = dr.GetDateTime(iRecintfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Recintcodi = "RECINTCODI";
        public string Retintcodi = "RETINTCODI";
        public string Recintnombre = "RECINTNOMBRE";
        public string Recintestado = "RECINTESTADO";
        public string Recintusucreacion = "RECINTUSUCREACION";
        public string Recintfeccreacion = "RECINTFECCREACION";
        public string Recintusumodificacion = "RECINTUSUMODIFICACION";
        public string Recintfecmodificacion = "RECINTFECMODIFICACION";
        public string Retintnombre = "RETINTNOMBRE";
        public string Reindki = "REINDKI";
        public string Reindni = "REINDNI";
        public string IndicadorEdicion = "INDICADOREDICION";

        #endregion

        public string SqlObenerConfiguracion
        {
            get { return base.GetSqlXml("ObtenerConfiguracion"); }
        }

        public string SqlObtenerCausasInterrupcionUtilizadosPorPeriodo
        {
            get { return base.GetSqlXml("ObtenerCausasInterrupcionUtilizadosPorPeriodo"); }
        }
    }
}
