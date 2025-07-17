using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla FT_EXT_ETAPA
    /// </summary>
    public class FtExtEtapaHelper : HelperBase
    {
        public FtExtEtapaHelper() : base(Consultas.FtExtEtapaSql)
        {
        }

        public FtExtEtapaDTO Create(IDataReader dr)
        {
            FtExtEtapaDTO entity = new FtExtEtapaDTO();

            int iFtetcodi = dr.GetOrdinal(this.Ftetcodi);
            if (!dr.IsDBNull(iFtetcodi)) entity.Ftetcodi = Convert.ToInt32(dr.GetValue(iFtetcodi));

            int iFtetnombre = dr.GetOrdinal(this.Ftetnombre);
            if (!dr.IsDBNull(iFtetnombre)) entity.Ftetnombre = dr.GetString(iFtetnombre);

            return entity;
        }

        #region Mapeo de Campos

        public string Ftetcodi = "FTETCODI";
        public string Ftetnombre = "FTETNOMBRE";

        #endregion
    }
}
