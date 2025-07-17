using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class WbComiteListaContactoHelper : HelperBase
    {
        public WbComiteListaContactoHelper() : base(Consultas.WbComiteListaContactoSql)
        {
        }
        public WbComiteListaContactoDTO Create(IDataReader dr)
        {
            WbComiteListaContactoDTO entity = new WbComiteListaContactoDTO();

            int iContaccodi = dr.GetOrdinal(this.Contaccodi);
            if (!dr.IsDBNull(iContaccodi)) entity.Contaccodi = Convert.ToInt32(dr.GetValue(iContaccodi));

            int iComitecodi = dr.GetOrdinal(this.Comitecodi);
            if (!dr.IsDBNull(iComitecodi)) entity.ComiteCodi = Convert.ToInt32(dr.GetValue(iComitecodi));

            int iComiteListacodi = dr.GetOrdinal(this.ComiteListacodi);
            if (!dr.IsDBNull(iComiteListacodi)) entity.ComiteListacodi = Convert.ToInt32(dr.GetValue(iComiteListacodi));

            return entity;
        }

        #region Mapeo de Campos

        public string Contaccodi = "CONTACCODI";
        public string ComiteListacodi = "COMITELISTACODI";
        public string Comitecodi = "COMITECODI";
        public string Descomite = "DESCOMITE";
        public string Indicador = "INDICADOR";

        #endregion
    }

}
