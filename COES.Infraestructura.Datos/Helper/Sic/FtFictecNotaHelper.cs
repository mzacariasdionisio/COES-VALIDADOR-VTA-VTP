using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla FT_FICTECNOTA
    /// </summary>
    public class FtFictecNotaHelper : HelperBase
    {
        public FtFictecNotaHelper()
            : base(Consultas.FtFictecNotaSql)
        {
        }

        public FtFictecNotaDTO Create(IDataReader dr)
        {
            FtFictecNotaDTO entity = new FtFictecNotaDTO();

            int iFtnotacodi = dr.GetOrdinal(this.Ftnotacodi);
            if (!dr.IsDBNull(iFtnotacodi)) entity.Ftnotacodi = Convert.ToInt32(dr.GetValue(iFtnotacodi));

            int iFtnotanum = dr.GetOrdinal(this.Ftnotanum);
            if (!dr.IsDBNull(iFtnotanum)) entity.Ftnotanum = Convert.ToInt32(dr.GetValue(iFtnotanum));

            int iFtnotdesc = dr.GetOrdinal(this.Ftnotdesc);
            if (!dr.IsDBNull(iFtnotdesc)) entity.Ftnotdesc = dr.GetString(iFtnotdesc);

            int iFtnotausucreacion = dr.GetOrdinal(this.Ftnotausucreacion);
            if (!dr.IsDBNull(iFtnotausucreacion)) entity.Ftnotausucreacion = dr.GetString(iFtnotausucreacion);

            int iFtnotafeccreacion = dr.GetOrdinal(this.Ftnotafeccreacion);
            if (!dr.IsDBNull(iFtnotafeccreacion)) entity.Ftnotafeccreacion = dr.GetDateTime(iFtnotafeccreacion);

            int iFtnotausumodificacion = dr.GetOrdinal(this.Ftnotausumodificacion);
            if (!dr.IsDBNull(iFtnotausumodificacion)) entity.Ftnotausumodificacion = dr.GetString(iFtnotausumodificacion);

            int iFtnotafecmodificacion = dr.GetOrdinal(this.Ftnotafecmodificacion);
            if (!dr.IsDBNull(iFtnotafecmodificacion)) entity.Ftnotafecmodificacion = dr.GetDateTime(iFtnotafecmodificacion);

            int iFtnotestado = dr.GetOrdinal(this.Ftnotestado);
            if (!dr.IsDBNull(iFtnotestado)) entity.Ftnotestado = Convert.ToInt32(dr.GetValue(iFtnotestado));

            int iFteqcodi = dr.GetOrdinal(this.Fteqcodi);
            if (!dr.IsDBNull(iFteqcodi)) entity.Fteqcodi = Convert.ToInt32(dr.GetValue(iFteqcodi));

            return entity;
        }
        
        #region Mapeo de Campos

        public string Ftnotacodi = "FTNOTACODI";
        public string Ftnotanum = "FTNOTANUM";
        public string Ftnotdesc = "FTNOTDESC";
        public string Ftnotausucreacion = "FTNOTAUSUCREACION";
        public string Ftnotafeccreacion = "FTNOTAFECCREACION";
        public string Ftnotausumodificacion = "FTNOTAUSUMODIFICACION";
        public string Ftnotafecmodificacion = "FTNOTAFECMODIFICACION";
        public string Ftnotestado = "FTNOTESTADO";
        public string Fteqcodi = "FTEQCODI";

        #endregion
        
        #region Mapeo de Consultas

        public string SqlListByFteqcodi
        {
            get { return base.GetSqlXml("ListByFteqcodi"); }
        }

        #endregion
        
    }
}
