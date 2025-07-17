using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SPO_CLASIFICACION
    /// </summary>
    public class SpoClasificacionHelper : HelperBase
    {
        public SpoClasificacionHelper(): base(Consultas.SpoClasificacionSql)
        {
        }

        public SpoClasificacionDTO Create(IDataReader dr)
        {
            SpoClasificacionDTO entity = new SpoClasificacionDTO();

            int iClasicodi = dr.GetOrdinal(this.Clasicodi);
            if (!dr.IsDBNull(iClasicodi)) entity.Clasicodi = Convert.ToInt32(dr.GetValue(iClasicodi));

            int iClasinombre = dr.GetOrdinal(this.Clasinombre);
            if (!dr.IsDBNull(iClasinombre)) entity.Clasinombre = dr.GetString(iClasinombre);

            return entity;
        }


        #region Mapeo de Campos

        public string Clasicodi = "CLASICODI";
        public string Clasinombre = "CLASINOMBRE";

        #endregion
    }
}
