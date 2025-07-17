using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Scada;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Scada
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla DATOS_SP7
    /// </summary>
    public class DatosSp7Helper : HelperBase
    {
        public DatosSp7Helper(): base(Consultas.DatosSp7Sql)
        {
        }

        public DatosSP7DTO Create(IDataReader dr)
        {
            DatosSP7DTO entity = new DatosSP7DTO();

            int iCanalcodi = dr.GetOrdinal(this.Canalcodi);
            if (!dr.IsDBNull(iCanalcodi)) entity.Canalcodi = Convert.ToInt32(dr.GetValue(iCanalcodi));

            int iFecha = dr.GetOrdinal(this.Fecha);
            if (!dr.IsDBNull(iFecha)) entity.Fecha = dr.GetDateTime(iFecha);

            int iFechasistema = dr.GetOrdinal(this.Fechasistema);
            if (!dr.IsDBNull(iFechasistema)) entity.FechaSistema = dr.GetDateTime(iFechasistema);

            int iPath = dr.GetOrdinal(this.Path);
            if (!dr.IsDBNull(iPath)) entity.Path = dr.GetString(iPath);

            int iValor = dr.GetOrdinal(this.Valor);
            if (!dr.IsDBNull(iValor)) entity.Valor = dr.GetDecimal(iValor);

            int iCalidad = dr.GetOrdinal(this.Calidad);
            if (!dr.IsDBNull(iCalidad)) entity.Calidad = Convert.ToInt32(dr.GetValue(iCalidad));

            int iCalidadtexto = dr.GetOrdinal(this.Calidadtexto);
            if (!dr.IsDBNull(iCalidadtexto)) entity.CalidadTexto = dr.GetString(iCalidadtexto);

            return entity;
        }


        #region Mapeo de Campos

        public string Canalcodi = "CANALCODI";
        public string Fecha = "TIMESTAMP";
        public string Fechasistema = "SYSTEMTIMESTAMP";
        public string Path = "PATH";
        public string Valor1 = "VALUE";
        public string Calidad = "CALIDAD";
        public string Calidadtexto = "QUALITYTEXTUSER";

        public string SqlObtenerListadoSP7
        {
            get { return base.GetSqlXml("ListadoSP7"); }
        }


        #endregion
    }
}
