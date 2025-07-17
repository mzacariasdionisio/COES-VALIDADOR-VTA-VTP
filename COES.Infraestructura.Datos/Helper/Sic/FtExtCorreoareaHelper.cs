using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla FT_EXT_CORREOAREA
    /// </summary>
    public class FtExtCorreoareaHelper : HelperBase
    {
        public FtExtCorreoareaHelper(): base(Consultas.FtExtCorreoareaSql)
        {
        }

        public FtExtCorreoareaDTO Create(IDataReader dr)
        {
            FtExtCorreoareaDTO entity = new FtExtCorreoareaDTO();

            int iFaremcodi = dr.GetOrdinal(this.Faremcodi);
            if (!dr.IsDBNull(iFaremcodi)) entity.Faremcodi = Convert.ToInt32(dr.GetValue(iFaremcodi));

            int iFaremfeccreacion = dr.GetOrdinal(this.Faremfeccreacion);
            if (!dr.IsDBNull(iFaremfeccreacion)) entity.Faremfeccreacion = dr.GetDateTime(iFaremfeccreacion);

            int iFaremusucreacion = dr.GetOrdinal(this.Faremusucreacion);
            if (!dr.IsDBNull(iFaremusucreacion)) entity.Faremusucreacion = dr.GetString(iFaremusucreacion);

            int iFaremfecmodificacion = dr.GetOrdinal(this.Faremfecmodificacion);
            if (!dr.IsDBNull(iFaremfecmodificacion)) entity.Faremfecmodificacion = dr.GetDateTime(iFaremfecmodificacion);

            int iFaremusumodificacion = dr.GetOrdinal(this.Faremusumodificacion);
            if (!dr.IsDBNull(iFaremusumodificacion)) entity.Faremusumodificacion = dr.GetString(iFaremusumodificacion);

            int iFaremnombre = dr.GetOrdinal(this.Faremnombre);
            if (!dr.IsDBNull(iFaremnombre)) entity.Faremnombre = dr.GetString(iFaremnombre);

            int iFaremestado = dr.GetOrdinal(this.Faremestado);
            if (!dr.IsDBNull(iFaremestado)) entity.Faremestado = dr.GetString(iFaremestado);

            return entity;
        }


        #region Mapeo de Campos

        public string Faremcodi = "FAREMCODI";
        public string Faremfeccreacion = "FAREMFECCREACION";
        public string Faremusucreacion = "FAREMUSUCREACION";
        public string Faremfecmodificacion = "FAREMFECMODIFICACION";
        public string Faremusumodificacion = "FAREMUSUMODIFICACION";
        public string Faremnombre = "FAREMNOMBRE";
        public string Faremestado = "FAREMESTADO";
        public string Ftitcodi = "FTITCODI";
        public string Fevrqcodi = "FEVRQCODI";
        

        #endregion

        public string SqlListarPorParametros
        {
            get { return GetSqlXml("ListarPorParametros"); }
        }

        public string SqlListarPorRequisitos
        {
            get { return GetSqlXml("ListarPorRequisitos"); }
        }
        

        public string SqlListarPorIds
        {
            get { return GetSqlXml("ListarPorIds"); }
        }
        
    }
}
