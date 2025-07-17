using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ME_MODULO
    /// </summary>
    public class MeModuloHelper : HelperBase
    {
        public MeModuloHelper(): base(Consultas.MeModuloSql)
        {
        }

        public MeModuloDTO Create(IDataReader dr)
        {
            MeModuloDTO entity = new MeModuloDTO();

            int iModcodi = dr.GetOrdinal(this.Modcodi);
            if (!dr.IsDBNull(iModcodi)) entity.Modcodi = Convert.ToInt32(dr.GetValue(iModcodi));

            int iModnomb = dr.GetOrdinal(this.Modnomb);
            if (!dr.IsDBNull(iModnomb)) entity.Modnomb = dr.GetString(iModnomb);

            int iOriglectcodi = dr.GetOrdinal(this.Origlectcodi);
            if (!dr.IsDBNull(iOriglectcodi)) entity.Origlectcodi = Convert.ToInt32(dr.GetValue(iOriglectcodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Modcodi = "MODCODI";
        public string Modnomb = "MODNOMB";
        public string Origlectcodi = "ORIGLECTCODI";

        #endregion
    }
}
