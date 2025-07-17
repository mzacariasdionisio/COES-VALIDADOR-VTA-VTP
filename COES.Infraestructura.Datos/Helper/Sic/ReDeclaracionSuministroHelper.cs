using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RE_DECLARACION_SUMINISTRO
    /// </summary>
    public class ReDeclaracionSuministroHelper : HelperBase
    {
        public ReDeclaracionSuministroHelper(): base(Consultas.ReDeclaracionSuministroSql)
        {
        }

        public ReDeclaracionSuministroDTO Create(IDataReader dr)
        {
            ReDeclaracionSuministroDTO entity = new ReDeclaracionSuministroDTO();

            int iRedeccodi = dr.GetOrdinal(this.Redeccodi);
            if (!dr.IsDBNull(iRedeccodi)) entity.Redeccodi = Convert.ToInt32(dr.GetValue(iRedeccodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iReenvcodi = dr.GetOrdinal(this.Reenvcodi);
            if (!dr.IsDBNull(iReenvcodi)) entity.Reenvcodi = Convert.ToInt32(dr.GetValue(iReenvcodi));

            int iRedeindicador = dr.GetOrdinal(this.Redeindicador);
            if (!dr.IsDBNull(iRedeindicador)) entity.Redeindicador = dr.GetString(iRedeindicador);

            return entity;
        }


        #region Mapeo de Campos

        public string Redeccodi = "REDECCODI";
        public string Emprcodi = "EMPRCODI";
        public string Reenvcodi = "REENVCODI";
        public string Redeindicador = "REDEINDICADOR";

        #endregion
    }
}
