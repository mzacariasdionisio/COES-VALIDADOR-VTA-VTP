using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla WB_SUBSCRIPCIONITEM
    /// </summary>
    public class WbSubscripcionitemHelper : HelperBase
    {
        public WbSubscripcionitemHelper(): base(Consultas.WbSubscripcionitemSql)
        {
        }

        public WbSubscripcionitemDTO Create(IDataReader dr)
        {
            WbSubscripcionitemDTO entity = new WbSubscripcionitemDTO();

            int iSubscripcodi = dr.GetOrdinal(this.Subscripcodi);
            if (!dr.IsDBNull(iSubscripcodi)) entity.Subscripcodi = Convert.ToInt32(dr.GetValue(iSubscripcodi));

            int iPubliccodi = dr.GetOrdinal(this.Publiccodi);
            if (!dr.IsDBNull(iPubliccodi)) entity.Publiccodi = Convert.ToInt32(dr.GetValue(iPubliccodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Subscripcodi = "SUBSCRIPCODI";
        public string Publiccodi = "PUBLICCODI";
        public string Despublicacion = "DESPUBLICACION";
        public string Indicador = "INDICADOR";

        #endregion
    }
}
