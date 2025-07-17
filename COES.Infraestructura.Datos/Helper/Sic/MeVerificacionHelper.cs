using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ME_VERIFICACION
    /// </summary>
    public class MeVerificacionHelper : HelperBase
    {
        public MeVerificacionHelper()
            : base(Consultas.MeVerificacionSql)
        {
        }

        public MeVerificacionDTO Create(IDataReader dr)
        {
            MeVerificacionDTO entity = new MeVerificacionDTO();

            int iVerifcodi = dr.GetOrdinal(this.Verifcodi);
            if (!dr.IsDBNull(iVerifcodi)) entity.Verifcodi = Convert.ToInt32(dr.GetValue(iVerifcodi));

            int iVerifnomb = dr.GetOrdinal(this.Verifnomb);
            if (!dr.IsDBNull(iVerifnomb)) entity.Verifnomb = dr.GetString(iVerifnomb);

            return entity;
        }


        #region Mapeo de Campos

        public string Verifcodi = "VERIFCODI";
        public string Verifnomb = "VERIFNOMB";

        #endregion
    }
}
