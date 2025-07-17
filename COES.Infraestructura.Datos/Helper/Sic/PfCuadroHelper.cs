using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PF_CUADRO
    /// </summary>
    public class PfCuadroHelper : HelperBase
    {
        public PfCuadroHelper(): base(Consultas.PfCuadroSql)
        {
        }

        public PfCuadroDTO Create(IDataReader dr)
        {
            PfCuadroDTO entity = new PfCuadroDTO();

            int iPfcuacodi = dr.GetOrdinal(this.Pfcuacodi);
            if (!dr.IsDBNull(iPfcuacodi)) entity.Pfcuacodi = Convert.ToInt32(dr.GetValue(iPfcuacodi));

            int iPfcuanombre = dr.GetOrdinal(this.Pfcuanombre);
            if (!dr.IsDBNull(iPfcuanombre)) entity.Pfcuanombre = dr.GetString(iPfcuanombre);

            int iPfcuatitulo = dr.GetOrdinal(this.Pfcuatitulo);
            if (!dr.IsDBNull(iPfcuatitulo)) entity.Pfcuatitulo = dr.GetString(iPfcuatitulo);

            int iPfcuasubtitulo = dr.GetOrdinal(this.Pfcuasubtitulo);
            if (!dr.IsDBNull(iPfcuasubtitulo)) entity.Pfcuasubtitulo = dr.GetString(iPfcuasubtitulo);


            return entity;
        }


        #region Mapeo de Campos

        public string Pfcuacodi = "PFCUACODI";
        public string Pfcuanombre = "PFCUANOMBRE";
        public string Pfcuatitulo = "PFCUATITULO";
        public string Pfcuasubtitulo = "PFCUASUBTITULO";

        #endregion
    }
}
