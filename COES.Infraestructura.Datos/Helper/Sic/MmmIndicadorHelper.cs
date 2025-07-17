using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla MMM_INDICADOR
    /// </summary>
    public class MmmIndicadorHelper : HelperBase
    {
        public MmmIndicadorHelper()
            : base(Consultas.MmmIndicadorSql)
        {
        }

        public MmmIndicadorDTO Create(IDataReader dr)
        {
            MmmIndicadorDTO entity = new MmmIndicadorDTO();

            int iImmecodi = dr.GetOrdinal(this.Immecodi);
            if (!dr.IsDBNull(iImmecodi)) entity.Immecodi = Convert.ToInt32(dr.GetValue(iImmecodi));

            int iImmeabrev = dr.GetOrdinal(this.Immeabrev);
            if (!dr.IsDBNull(iImmeabrev)) entity.Immeabrev = dr.GetString(iImmeabrev);

            int iImmenombre = dr.GetOrdinal(this.Immenombre);
            if (!dr.IsDBNull(iImmenombre)) entity.Immenombre = dr.GetString(iImmenombre);

            int iImmedescripcion = dr.GetOrdinal(this.Immedescripcion);
            if (!dr.IsDBNull(iImmedescripcion)) entity.Immedescripcion = dr.GetString(iImmedescripcion);

            int iImmecodigo = dr.GetOrdinal(this.Immecodigo);
            if (!dr.IsDBNull(iImmecodigo)) entity.Immecodigo = dr.GetString(iImmecodigo);

            return entity;
        }


        #region Mapeo de Campos

        public string Immecodi = "IMMECODI";
        public string Immeabrev = "IMMEABREV";
        public string Immenombre = "IMMENOMBRE";
        public string Immedescripcion = "IMMEDESCRIPCION";
        public string Immecodigo = "IMMECODIGO";

        #endregion
    }
}
