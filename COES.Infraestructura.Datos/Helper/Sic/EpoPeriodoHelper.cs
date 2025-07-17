using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EPO_PERIODO
    /// </summary>
    public class EpoPeriodoHelper : HelperBase
    {
        public EpoPeriodoHelper(): base(Consultas.EpoPeriodoSql)
        {
        }

        public EpoPeriodoDTO Create(IDataReader dr)
        {
            EpoPeriodoDTO entity = new EpoPeriodoDTO();

            int iPercodi = dr.GetOrdinal(this.Percodi);
            if (!dr.IsDBNull(iPercodi)) entity.Percodi = Convert.ToInt32(dr.GetValue(iPercodi));

            int iPeranio = dr.GetOrdinal(this.Peranio);
            if (!dr.IsDBNull(iPeranio)) entity.Peranio = Convert.ToInt32(dr.GetValue(iPeranio));

            int iPermes = dr.GetOrdinal(this.Permes);
            if (!dr.IsDBNull(iPermes)) entity.Permes = Convert.ToInt32(dr.GetValue(iPermes));

            int iPerestado = dr.GetOrdinal(this.Perestado);
            if (!dr.IsDBNull(iPerestado)) entity.Perestado = dr.GetString(iPerestado);

            return entity;
        }


        #region Mapeo de Campos

        public string Percodi = "PERCODI";
        public string Peranio = "PERANIO";
        public string Permes = "PERMES";
        public string Perestado = "PERESTADO";

        #endregion
    }
}
