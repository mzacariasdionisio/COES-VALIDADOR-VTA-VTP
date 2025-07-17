using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ST_DSTELE_BARRA
    /// </summary>
    public class StDsteleBarraHelper : HelperBase
    {
        public StDsteleBarraHelper(): base(Consultas.StDsteleBarraSql)
        {
        }

        public StDsteleBarraDTO Create(IDataReader dr)
        {
            StDsteleBarraDTO entity = new StDsteleBarraDTO();

            int iDstelecodi = dr.GetOrdinal(this.Dstelecodi);
            if (!dr.IsDBNull(iDstelecodi)) entity.Dstelecodi = Convert.ToInt32(dr.GetValue(iDstelecodi));

            int iBarrcodi = dr.GetOrdinal(this.Barrcodi);
            if (!dr.IsDBNull(iBarrcodi)) entity.Barrcodi = Convert.ToInt32(dr.GetValue(iBarrcodi));

            int iDelbarrpu = dr.GetOrdinal(this.Delbarrpu);
            if (!dr.IsDBNull(iDelbarrpu)) entity.Delbarrpu = dr.GetDecimal(iDelbarrpu);

            int iDelbarxpu = dr.GetOrdinal(this.Delbarxpu);
            if (!dr.IsDBNull(iDelbarxpu)) entity.Delbarxpu = dr.GetDecimal(iDelbarxpu);

            return entity;
        }

        #region Mapeo de Campos

        public string Dstelecodi = "DSTELECODI";
        public string Barrcodi = "BARRCODI";
        public string Delbarrpu = "DELBARRPU";
        public string Delbarxpu = "DELBARXPU";
        //atributos de consultas
        public string Strecacodi = "STRECACODI";
        public string Barra1 = "GWBARRCODI";
        public string Barra2 = "LMBARRCODI";
        #endregion

        public string SqlGetByCriterios
        {
            get { return base.GetSqlXml("GetByCriterios"); }
        }

        public string SqlListStDistEleBarrPorID
        {
            get { return base.GetSqlXml("ListStDistEleBarrPorID"); }
        }
        
    }
}
