using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla IND_CUADRO
    /// </summary>
    public class IndCuadroHelper : HelperBase
    {
        public IndCuadroHelper() : base(Consultas.IndCuadroSql)
        {
        }

        public IndCuadroDTO Create(IDataReader dr)
        {
            IndCuadroDTO entity = new IndCuadroDTO();

            int iIcuacodi = dr.GetOrdinal(this.Icuacodi);
            if (!dr.IsDBNull(iIcuacodi)) entity.Icuacodi = Convert.ToInt32(dr.GetValue(iIcuacodi));

            int iIcuatitulo = dr.GetOrdinal(this.Icuatitulo);
            if (!dr.IsDBNull(iIcuatitulo)) entity.Icuatitulo = dr.GetString(iIcuatitulo);

            int iIcuanombre = dr.GetOrdinal(this.Icuanombre);
            if (!dr.IsDBNull(iIcuanombre)) entity.Icuanombre = dr.GetString(iIcuanombre);

            int iIcuasubtitulo = dr.GetOrdinal(this.Icuasubtitulo);
            if (!dr.IsDBNull(iIcuasubtitulo)) entity.Icuasubtitulo = dr.GetString(iIcuasubtitulo);

            return entity;
        }

        #region Mapeo de Campos

        public string Icuacodi = "ICUACODI";
        public string Icuatitulo = "ICUATITULO";
        public string Icuanombre = "ICUANOMBRE";
        public string Icuasubtitulo = "ICUASUBTITULO";

        #endregion
    }
}
