using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SPO_NUMERAL
    /// </summary>
    public class SpoNumeralHelper : HelperBase
    {
        public SpoNumeralHelper(): base(Consultas.SpoNumeralSql)
        {
        }

        public SpoNumeralDTO Create(IDataReader dr)
        {
            SpoNumeralDTO entity = new SpoNumeralDTO();

            int iNumecodi = dr.GetOrdinal(this.Numecodi);
            if (!dr.IsDBNull(iNumecodi)) entity.Numecodi = Convert.ToInt32(dr.GetValue(iNumecodi));

            int iAreacodi = dr.GetOrdinal(this.Areacodi);
            if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

            int iNumediaplazo = dr.GetOrdinal(this.Numediaplazo);
            if (!dr.IsDBNull(iNumediaplazo)) entity.Numediaplazo = Convert.ToInt32(dr.GetValue(iNumediaplazo));

            int iNumeusucreacion = dr.GetOrdinal(this.Numeusucreacion);
            if (!dr.IsDBNull(iNumeusucreacion)) entity.Numeusucreacion = dr.GetString(iNumeusucreacion);

            int iNumefeccreacion = dr.GetOrdinal(this.Numefeccreacion);
            if (!dr.IsDBNull(iNumefeccreacion)) entity.Numefeccreacion = dr.GetDateTime(iNumefeccreacion);

            int iNumeactivo = dr.GetOrdinal(this.Numeactivo);
            if (!dr.IsDBNull(iNumeactivo)) entity.Numeactivo = Convert.ToInt32(dr.GetValue(iNumeactivo));

            return entity;
        }


        #region Mapeo de Campos

        public string Numecodi = "NUMECODI";
        public string Areacodi = "AREACODI";
        public string Numediaplazo = "NUMEDIAPLAZO";
        public string Numeusucreacion = "NUMEUSUCREACION";
        public string Numefeccreacion = "NUMEFECCREACION";
        public string Numeactivo = "NUMEACTIVO";
        public string Numhisabrev = "NUMHISABREV";

        #endregion
    }
}
