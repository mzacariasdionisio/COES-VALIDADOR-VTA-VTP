using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla IEE_MODOOPECMG
    /// </summary>
    public class IeeModoopecmgHelper : HelperBase
    {
        public IeeModoopecmgHelper(): base(Consultas.IeeModoopecmgSql)
        {
        }

        public IeeModoopecmgDTO Create(IDataReader dr)
        {
            IeeModoopecmgDTO entity = new IeeModoopecmgDTO();

            int iMocmcodigo = dr.GetOrdinal(this.Mocmcodigo);
            if (!dr.IsDBNull(iMocmcodigo)) entity.Mocmcodigo = Convert.ToInt32(dr.GetValue(iMocmcodigo));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = dr.GetInt32(iGrupocodi);

            int iMocmtipocomb = dr.GetOrdinal(this.Mocmtipocomb);
            if (!dr.IsDBNull(iMocmtipocomb)) entity.Mocmtipocomb = Convert.ToInt32(dr.GetValue(iMocmtipocomb));

            return entity;
        }


        #region Mapeo de Campos

        public string Mocmcodigo = "MOCMCODIGO";
        public string Grupocodi = "GRUPOCODI";
        public string Mocmtipocomb = "MOCMTIPOCOMB";

        #endregion
    }
}
