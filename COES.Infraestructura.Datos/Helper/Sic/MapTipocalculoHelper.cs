using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla MAP_TIPOCALCULO
    /// </summary>
    public class MapTipocalculoHelper : HelperBase
    {
        public MapTipocalculoHelper(): base(Consultas.MapTipocalculoSql)
        {
        }

        public MapTipocalculoDTO Create(IDataReader dr)
        {
            MapTipocalculoDTO entity = new MapTipocalculoDTO();

            int iTipoccodi = dr.GetOrdinal(this.Tipoccodi);
            if (!dr.IsDBNull(iTipoccodi)) entity.Tipoccodi = Convert.ToInt32(dr.GetValue(iTipoccodi));

            int iTipocdesc = dr.GetOrdinal(this.Tipocdesc);
            if (!dr.IsDBNull(iTipocdesc)) entity.Tipocdesc = dr.GetString(iTipocdesc);

            int iTipocabrev = dr.GetOrdinal(this.Tipocabrev);
            if (!dr.IsDBNull(iTipocabrev)) entity.Tipocabrev = dr.GetString(iTipocabrev);

            return entity;
        }


        #region Mapeo de Campos

        public string Tipoccodi = "TIPOCCODI";
        public string Tipocdesc = "TIPOCDESC";
        public string Tipocabrev = "TIPOCABREV";

        #endregion
    }
}
