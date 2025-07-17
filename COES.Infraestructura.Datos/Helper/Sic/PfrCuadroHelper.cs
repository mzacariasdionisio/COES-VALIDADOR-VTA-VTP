using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PFR_CUADRO
    /// </summary>
    public class PfrCuadroHelper : HelperBase
    {
        public PfrCuadroHelper(): base(Consultas.PfrCuadroSql)
        {
        }

        public PfrCuadroDTO Create(IDataReader dr)
        {
            PfrCuadroDTO entity = new PfrCuadroDTO();

            int iPfrcuacodi = dr.GetOrdinal(this.Pfrcuacodi);
            if (!dr.IsDBNull(iPfrcuacodi)) entity.Pfrcuacodi = Convert.ToInt32(dr.GetValue(iPfrcuacodi));

            int iPfrcuanombre = dr.GetOrdinal(this.Pfrcuanombre);
            if (!dr.IsDBNull(iPfrcuanombre)) entity.Pfrcuanombre = dr.GetString(iPfrcuanombre);

            int iPfrcuatitulo = dr.GetOrdinal(this.Pfrcuatitulo);
            if (!dr.IsDBNull(iPfrcuatitulo)) entity.Pfrcuatitulo = dr.GetString(iPfrcuatitulo);

            int iPfrcuasubtitulo = dr.GetOrdinal(this.Pfrcuasubtitulo);
            if (!dr.IsDBNull(iPfrcuasubtitulo)) entity.Pfrcuasubtitulo = dr.GetString(iPfrcuasubtitulo);

            return entity;
        }


        #region Mapeo de Campos

        public string Pfrcuacodi = "PFRCUACODI";
        public string Pfrcuanombre = "PFRCUANOMBRE";
        public string Pfrcuatitulo = "PFRCUATITULO";
        public string Pfrcuasubtitulo = "PFRCUASUBTITULO";

        #endregion
    }
}
