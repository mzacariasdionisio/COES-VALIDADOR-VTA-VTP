using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla FT_EXT_RELAREAITEMCFG
    /// </summary>
    public class FtExtRelareaitemcfgHelper : HelperBase
    {
        public FtExtRelareaitemcfgHelper(): base(Consultas.FtExtRelareaitemcfgSql)
        {
        }

        public FtExtRelareaitemcfgDTO Create(IDataReader dr)
        {
            FtExtRelareaitemcfgDTO entity = new FtExtRelareaitemcfgDTO();

            int iFaremcodi = dr.GetOrdinal(this.Faremcodi);
            if (!dr.IsDBNull(iFaremcodi)) entity.Faremcodi = Convert.ToInt32(dr.GetValue(iFaremcodi));

            int iFtitcodi = dr.GetOrdinal(this.Ftitcodi);
            if (!dr.IsDBNull(iFtitcodi)) entity.Ftitcodi = Convert.ToInt32(dr.GetValue(iFtitcodi));

            int iFriacodi = dr.GetOrdinal(this.Friacodi);
            if (!dr.IsDBNull(iFriacodi)) entity.Friacodi = Convert.ToInt32(dr.GetValue(iFriacodi));

            int iFriaestado = dr.GetOrdinal(this.Friaestado);
            if (!dr.IsDBNull(iFriaestado)) entity.Friaestado = dr.GetString(iFriaestado);

            int iFriafeccreacion = dr.GetOrdinal(this.Friafeccreacion);
            if (!dr.IsDBNull(iFriafeccreacion)) entity.Friafeccreacion = dr.GetDateTime(iFriafeccreacion);

            int iFriausucreacion = dr.GetOrdinal(this.Friausucreacion);
            if (!dr.IsDBNull(iFriausucreacion)) entity.Friausucreacion = dr.GetString(iFriausucreacion);

            int iFriafecmodificacion = dr.GetOrdinal(this.Friafecmodificacion);
            if (!dr.IsDBNull(iFriafecmodificacion)) entity.Friafecmodificacion = dr.GetDateTime(iFriafecmodificacion);

            int iFriausumodificacion = dr.GetOrdinal(this.Friausumodificacion);
            if (!dr.IsDBNull(iFriausumodificacion)) entity.Friausumodificacion = dr.GetString(iFriausumodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Faremcodi = "FAREMCODI";
        public string Ftitcodi = "FTITCODI";
        public string Friacodi = "FRIACODI";
        public string Friaestado = "FRIAESTADO";
        public string Friafeccreacion = "FRIAFECCREACION";
        public string Friausucreacion = "FRIAUSUCREACION";
        public string Friafecmodificacion = "FRIAFECMODIFICACION";
        public string Friausumodificacion = "FRIAUSUMODIFICACION";

        public string Faremnombre = "FAREMNOMBRE";
        
        #endregion

        public string SqlListarPorAreas
        {
            get { return GetSqlXml("ListarPorAreas"); }
        }
    }
}
