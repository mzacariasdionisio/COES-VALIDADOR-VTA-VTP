using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CB_FICHA
    /// </summary>
    public class CbFichaHelper : HelperBase
    {
        public CbFichaHelper() : base(Consultas.CbFichaSql)
        {
        }

        public CbFichaDTO Create(IDataReader dr)
        {
            CbFichaDTO entity = new CbFichaDTO();

            int iCbftcodi = dr.GetOrdinal(this.Cbftcodi);
            if (!dr.IsDBNull(iCbftcodi)) entity.Cbftcodi = Convert.ToInt32(dr.GetValue(iCbftcodi));

            int iCbftnombre = dr.GetOrdinal(this.Cbftnombre);
            if (!dr.IsDBNull(iCbftnombre)) entity.Cbftnombre = dr.GetString(iCbftnombre);

            int iCbftfechavigencia = dr.GetOrdinal(this.Cbftfechavigencia);
            if (!dr.IsDBNull(iCbftfechavigencia)) entity.Cbftfechavigencia = dr.GetDateTime(iCbftfechavigencia);

            int iCbftusucreacion = dr.GetOrdinal(this.Cbftusucreacion);
            if (!dr.IsDBNull(iCbftusucreacion)) entity.Cbftusucreacion = dr.GetString(iCbftusucreacion);

            int iCbftfeccreacion = dr.GetOrdinal(this.Cbftfeccreacion);
            if (!dr.IsDBNull(iCbftfeccreacion)) entity.Cbftfeccreacion = dr.GetDateTime(iCbftfeccreacion);

            int iCbftusumodificacion = dr.GetOrdinal(this.Cbftusumodificacion);
            if (!dr.IsDBNull(iCbftusumodificacion)) entity.Cbftusumodificacion = dr.GetString(iCbftusumodificacion);

            int iCbftfecmodificacion = dr.GetOrdinal(this.Cbftfecmodificacion);
            if (!dr.IsDBNull(iCbftfecmodificacion)) entity.Cbftfecmodificacion = dr.GetDateTime(iCbftfecmodificacion);

            int iCbftactivo = dr.GetOrdinal(this.Cbftactivo);
            if (!dr.IsDBNull(iCbftactivo)) entity.Cbftactivo = Convert.ToInt32(dr.GetValue(iCbftactivo));

            return entity;
        }


        #region Mapeo de Campos

        public string Cbftcodi = "CBFTCODI";
        public string Cbftnombre = "CBFTNOMBRE";
        public string Cbftfechavigencia = "CBFTFECHAVIGENCIA";
        public string Cbftusucreacion = "CBFTUSUCREACION";
        public string Cbftfeccreacion = "CBFTFECCREACION";
        public string Cbftusumodificacion = "CBFTUSUMODIFICACION";
        public string Cbftfecmodificacion = "CBFTFECMODIFICACION";
        public string Cbftactivo = "CBFTACTIVO";

        #endregion
    }
}
