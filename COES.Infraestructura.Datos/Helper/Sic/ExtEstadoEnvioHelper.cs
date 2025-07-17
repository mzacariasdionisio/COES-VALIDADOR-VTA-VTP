using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class ExtEstadoEnvioHelper : HelperBase
    {
        public ExtEstadoEnvioHelper()
            : base(Consultas.ExtEstadoEnvioSql)
        {
        }

        public ExtEstadoEnvioDTO Create(IDataReader dr)
        {
            ExtEstadoEnvioDTO entity = new ExtEstadoEnvioDTO();
            int iEstenvcodi = dr.GetOrdinal(this.Estenvcodi);
            if (!dr.IsDBNull(iEstenvcodi)) entity.Estenvcodi = Convert.ToInt32(dr.GetValue(iEstenvcodi));

            int iEstenvabrev = dr.GetOrdinal(this.Estenvabrev);
            if (!dr.IsDBNull(iEstenvabrev)) entity.Estenvabrev = dr.GetString(iEstenvabrev);

            int iEstenvnomb = dr.GetOrdinal(this.Estenvnomb);
            if (!dr.IsDBNull(iEstenvnomb)) entity.Estenvnomb = dr.GetString(iEstenvnomb);

            int iEstenvactivo = dr.GetOrdinal(this.Estenvactivo);
            if (!dr.IsDBNull(iEstenvactivo)) entity.Estenvactivo = dr.GetString(iEstenvactivo);

            return entity;
        }
        #region Mapeo de Campos

        public string Estenvcodi = "ESTENVCODI";
        public string Estenvabrev = "ESTENVABREV";
        public string Estenvnomb = "ESTENVNOMB";
        public string Estenvactivo = "ESTENVACTIVO";

        #endregion
    }
}
