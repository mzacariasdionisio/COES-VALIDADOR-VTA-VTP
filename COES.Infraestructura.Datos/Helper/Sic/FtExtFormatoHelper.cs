using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla FT_EXT_FORMATO
    /// </summary>
    public class FtExtFormatoHelper : HelperBase
    {
        public FtExtFormatoHelper() : base(Consultas.FtExtFormatoSql)
        {
        }

        public FtExtFormatoDTO Create(IDataReader dr)
        {
            FtExtFormatoDTO entity = new FtExtFormatoDTO();

            int iFteqcodi = dr.GetOrdinal(this.Fteqcodi);
            if (!dr.IsDBNull(iFteqcodi)) entity.Fteqcodi = Convert.ToInt32(dr.GetValue(iFteqcodi));

            int iFtetcodi = dr.GetOrdinal(this.Ftetcodi);
            if (!dr.IsDBNull(iFtetcodi)) entity.Ftetcodi = Convert.ToInt32(dr.GetValue(iFtetcodi));

            int iFtfmtcodi = dr.GetOrdinal(this.Ftfmtcodi);
            if (!dr.IsDBNull(iFtfmtcodi)) entity.Ftfmtcodi = Convert.ToInt32(dr.GetValue(iFtfmtcodi));

            return entity;
        }

        #region Mapeo de Campos

        public string Fteqcodi = "FTEQCODI";
        public string Ftetcodi = "FTETCODI";
        public string Ftfmtcodi = "FTFMTCODI";

        public string Famcodi = "FAMCODI";
        public string Catecodi = "CATECODI";

        #endregion

        public string SqlListarPorEtapaYTipoEquipo
        {
            get { return base.GetSqlXml("ListarPorEtapaYTipoEquipo"); }
        }
    }
}
