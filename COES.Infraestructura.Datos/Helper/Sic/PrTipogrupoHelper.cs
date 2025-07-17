using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PR_TIPOGRUPO
    /// </summary>
    public class PrTipogrupoHelper : HelperBase
    {
        public PrTipogrupoHelper(): base(Consultas.PrTipogrupoSql)
        {
        }

        public PrTipogrupoDTO Create(IDataReader dr)
        {
            PrTipogrupoDTO entity = new PrTipogrupoDTO();

            int iTipogrupocodi = dr.GetOrdinal(this.Tipogrupocodi);
            if (!dr.IsDBNull(iTipogrupocodi)) entity.Tipogrupocodi = Convert.ToInt32(dr.GetValue(iTipogrupocodi));

            int iTipogruponomb = dr.GetOrdinal(this.Tipogruponomb);
            if (!dr.IsDBNull(iTipogruponomb)) entity.Tipogruponomb = dr.GetString(iTipogruponomb);

            int iTipogrupoabrev = dr.GetOrdinal(this.Tipogrupoabrev);
            if (!dr.IsDBNull(iTipogrupoabrev)) entity.Tipogrupoabrev = dr.GetString(iTipogrupoabrev);

            return entity;
        }


        #region Mapeo de Campos

        public string Tipogrupocodi = "TIPOGRUPOCODI";
        public string Tipogruponomb = "TIPOGRUPONOMB";
        public string Tipogrupoabrev = "TIPOGRUPOABREV";

        #endregion
    }
}

