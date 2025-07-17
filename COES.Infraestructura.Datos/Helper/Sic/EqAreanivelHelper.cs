using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EQ_AREANIVEL
    /// </summary>
    public class EqAreanivelHelper : HelperBase
    {
        public EqAreanivelHelper(): base(Consultas.EqAreanivelSql)
        {
        }

        public EqAreaNivelDTO Create(IDataReader dr)
        {
            EqAreaNivelDTO entity = new EqAreaNivelDTO();

            int iAnivelcodi = dr.GetOrdinal(this.Anivelcodi);
            if (!dr.IsDBNull(iAnivelcodi)) entity.ANivelCodi = Convert.ToInt32(dr.GetValue(iAnivelcodi));

            int iAnivelnomb = dr.GetOrdinal(this.Anivelnomb);
            if (!dr.IsDBNull(iAnivelnomb)) entity.ANivelNomb = dr.GetString(iAnivelnomb);

            return entity;
        }


        #region Mapeo de Campos

        public string Anivelcodi = "ANIVELCODI";
        public string Anivelnomb = "ANIVELNOMB";

        #endregion
    }
}
