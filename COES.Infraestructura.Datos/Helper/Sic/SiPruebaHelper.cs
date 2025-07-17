using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_PRUEBA
    /// </summary>
    public class SiPruebaHelper : HelperBase
    {
        public SiPruebaHelper(): base(Consultas.SiPruebaSql)
        {
        }

        public SiPruebaDTO Create(IDataReader dr)
        {
            SiPruebaDTO entity = new SiPruebaDTO();

            int iPruebacodi = dr.GetOrdinal(this.Pruebacodi);
            if (!dr.IsDBNull(iPruebacodi)) entity.Pruebacodi = dr.GetString(iPruebacodi);

            int iPruebanomb = dr.GetOrdinal(this.Pruebanomb);
            if (!dr.IsDBNull(iPruebanomb)) entity.Pruebanomb = dr.GetString(iPruebanomb);

            int iPruebaest = dr.GetOrdinal(this.Pruebaest);
            if (!dr.IsDBNull(iPruebaest)) entity.Pruebaest = dr.GetString(iPruebaest);

            return entity;
        }


        #region Mapeo de Campos

        public string Pruebacodi = "PRUEBACODI";
        public string Pruebanomb = "PRUEBANOMB";
        public string Pruebaest = "PRUEBAEST";

        #endregion

        public string SqlBuscarPorNombre
        {
            get { return base.GetSqlXml("BuscarPorNombre"); }
        }

    }
}
