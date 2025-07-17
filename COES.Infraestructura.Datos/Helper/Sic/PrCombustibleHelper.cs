using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using COES.Dominio.DTO.Sic;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class PrCombustibleHelper : HelperBase
    {
        public PrCombustibleHelper()
            : base(Consultas.PrCombustible)
        {
        }
        public CombustibleDTO Create(IDataReader dr)
        {
            CombustibleDTO entity = new CombustibleDTO();

            int iCombcodi = dr.GetOrdinal(this.Combcodi);
            if (!dr.IsDBNull(iCombcodi)) entity.Combcodi = Convert.ToInt32(dr.GetValue(iCombcodi));

            int iCombabrev = dr.GetOrdinal(this.Combabrev);
            if (!dr.IsDBNull(iCombabrev)) entity.Combabrev = dr.GetString(iCombabrev);

            int iCombnomb = dr.GetOrdinal(this.Combnomb);
            if (!dr.IsDBNull(iCombnomb)) entity.Combnomb = dr.GetString(iCombnomb);

            return entity;
        }


        #region Mapeo de Campos

        public string Combcodi = "COMBCODI";
        public string Combabrev = "COMBABREV";
        public string Combnomb = "COMBNOMB";

        #endregion
    }
}

