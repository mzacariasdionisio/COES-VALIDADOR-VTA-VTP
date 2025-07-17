using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla WB_COMITE_CONTACTO
    /// </summary>
    public class WbComiteContactoHelper : HelperBase
    {
        public WbComiteContactoHelper(): base(Consultas.WbComiteContactoSql)
        {
        }

        public WbComiteContactoDTO Create(IDataReader dr)
        {
            WbComiteContactoDTO entity = new WbComiteContactoDTO();

            int iContaccodi = dr.GetOrdinal(this.Contaccodi);
            if (!dr.IsDBNull(iContaccodi)) entity.Contaccodi = Convert.ToInt32(dr.GetValue(iContaccodi));

            int iComitecodi = dr.GetOrdinal(this.Comitecodi);
            if (!dr.IsDBNull(iComitecodi)) entity.Comitecodi = Convert.ToInt32(dr.GetValue(iComitecodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Contaccodi = "CONTACCODI";
        public string Comitecodi = "COMITECODI";
        public string Descomite = "DESCOMITE";
        public string Indicador = "INDICADOR";

        #endregion
    }
}
