using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ME_ESTADOENVIO
    /// </summary>
    public class MeEstadoenvioHelper : HelperBase
    {
        public MeEstadoenvioHelper(): base(Consultas.MeEstadoenvioSql)
        {
        }

        public MeEstadoenvioDTO Create(IDataReader dr)
        {
            MeEstadoenvioDTO entity = new MeEstadoenvioDTO();

            int iEstenvcodi = dr.GetOrdinal(this.Estenvcodi);
            if (!dr.IsDBNull(iEstenvcodi)) entity.Estenvcodi = Convert.ToInt32(dr.GetValue(iEstenvcodi));

            int iEstenvnombre = dr.GetOrdinal(this.Estenvnombre);
            if (!dr.IsDBNull(iEstenvnombre)) entity.Estenvnombre = dr.GetString(iEstenvnombre);

            int iEstenvabrev = dr.GetOrdinal(this.Estenvabrev);
            if (!dr.IsDBNull(iEstenvabrev)) entity.Estenvabrev = dr.GetString(iEstenvabrev);

            return entity;
        }


        #region Mapeo de Campos

        public string Estenvcodi = "ESTENVCODI";
        public string Estenvnombre = "ESTENVNOMBRE";
        public string Estenvabrev = "ESTENVABREV";

        #endregion
    }
}
