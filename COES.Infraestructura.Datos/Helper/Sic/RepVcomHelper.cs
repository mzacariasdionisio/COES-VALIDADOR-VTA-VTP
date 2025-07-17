using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla REP_VCOM
    /// </summary>
    public class RepVcomHelper : HelperBase
    {
        public RepVcomHelper() : base(Consultas.RepVcomSql)
        {
        }

        public RepVcomDTO Create(IDataReader dr)
        {
            RepVcomDTO entity = new RepVcomDTO();

            int iPeriodo = dr.GetOrdinal(this.Periodo);
            if (!dr.IsDBNull(iPeriodo)) entity.Periodo = Convert.ToInt32(dr.GetValue(iPeriodo));

            int iCodigomodooperacion = dr.GetOrdinal(this.Codigomodooperacion);
            if (!dr.IsDBNull(iCodigomodooperacion)) entity.Codigomodooperacion = dr.GetString(iCodigomodooperacion);

            int iCodigotipocombustible = dr.GetOrdinal(this.Codigotipocombustible);
            if (!dr.IsDBNull(iCodigotipocombustible)) entity.Codigotipocombustible = dr.GetString(iCodigotipocombustible);

            int iValor = dr.GetOrdinal(this.Valor);
            if (!dr.IsDBNull(iValor)) entity.Valor = dr.GetDecimal(iValor);

            return entity;
        }


        #region Mapeo de Campos

        public string Periodo = "PERIODO";
        public string Codigomodooperacion = "CODIGOMODOOPERACION";
        public string Codigotipocombustible = "CODIGOTIPOCOMBUSTIBLE";
        public string Valor = "VALOR";

        #endregion
    }
}
