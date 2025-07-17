using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla FT_EXT_ENVIO_RELREVAREAARCHIVO
    /// </summary>
    public class FtExtEnvioRelrevareaarchivoHelper : HelperBase
    {
        public FtExtEnvioRelrevareaarchivoHelper(): base(Consultas.FtExtEnvioRelrevareaarchivoSql)
        {
        }

        public FtExtEnvioRelrevareaarchivoDTO Create(IDataReader dr)
        {
            FtExtEnvioRelrevareaarchivoDTO entity = new FtExtEnvioRelrevareaarchivoDTO();

            int iRevaacodi = dr.GetOrdinal(this.Revaacodi);
            if (!dr.IsDBNull(iRevaacodi)) entity.Revaacodi = Convert.ToInt32(dr.GetValue(iRevaacodi));

            int iRevacodi = dr.GetOrdinal(this.Revacodi);
            if (!dr.IsDBNull(iRevacodi)) entity.Revacodi = Convert.ToInt32(dr.GetValue(iRevacodi));

            int iFtearccodi = dr.GetOrdinal(this.Ftearccodi);
            if (!dr.IsDBNull(iFtearccodi)) entity.Ftearccodi = Convert.ToInt32(dr.GetValue(iFtearccodi));

            int iFtevercodi = dr.GetOrdinal(this.Ftevercodi);
            if (!dr.IsDBNull(iFtevercodi)) entity.Ftevercodi = Convert.ToInt32(dr.GetValue(iFtevercodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Revaacodi = "REVAACODI";
        public string Revacodi = "REVACODI";
        public string Ftearccodi = "FTEARCCODI";
        public string Ftevercodi = "FTEVERCODI";

        #endregion

        public string SqlDeletePorGrupo
        {
            get { return GetSqlXml("DeletePorGrupo"); }
        }

        public string SqlDeletePorVersionAreaYEquipo
        {
            get { return GetSqlXml("DeletePorVersionAreaYEquipo"); }
        }

        public string SqlListarPorVersiones
        {
            get { return GetSqlXml("ListarPorVersiones"); }
        }

        public string SqlListarRelacionesPorVersionAreaYEquipo
        {
            get { return GetSqlXml("ListarRelacionesPorVersionAreaYEquipo"); }
        }

        public string SqlDeletePorIds
        {
            get { return GetSqlXml("DeletePorIds"); }
        }

        public string SqlListarRelacionesContenidoPorVersionArea
        {
            get { return GetSqlXml("ListarRelacionesContenidoPorVersionArea"); }
        }
        
    }
}
