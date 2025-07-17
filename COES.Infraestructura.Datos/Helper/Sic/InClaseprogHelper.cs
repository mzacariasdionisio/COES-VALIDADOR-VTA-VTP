using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla IN_CLASEPROG
    /// </summary>
    public class InClaseprogHelper : HelperBase
    {
        public InClaseprogHelper(): base(Consultas.InClaseprogSql)
        {

        }

        public InClaseProgDTO Create(IDataReader dr)
        {
            InClaseProgDTO entity = new InClaseProgDTO();

            int iClaprocodi = dr.GetOrdinal(this.Claprocodi);
            if (!dr.IsDBNull(iClaprocodi)) entity.Claprocodi = Convert.ToInt32(dr.GetValue(iClaprocodi));

            int iClapronombre = dr.GetOrdinal(this.Clapronombre);
            if (!dr.IsDBNull(iClapronombre)) entity.Clapronombre = dr.GetString(iClapronombre);

            int iClaprousucreacion = dr.GetOrdinal(this.Claprousucreacion);
            if (!dr.IsDBNull(iClaprousucreacion)) entity.Claprousucreacion = dr.GetString(iClaprousucreacion);

            int iClaprofeccreacion = dr.GetOrdinal(this.Claprofeccreacion);
            if (!dr.IsDBNull(iClaprofeccreacion)) entity.Claprofeccreacion = dr.GetDateTime(iClaprofeccreacion);

            int iClaprousumodificacion = dr.GetOrdinal(this.Claprousumodificacion);
            if (!dr.IsDBNull(iClaprousumodificacion)) entity.Claprousumodificacion = dr.GetString(iClaprousumodificacion);

            int iClaprofecmodificacion = dr.GetOrdinal(this.Claprofecmodificacion);
            if (!dr.IsDBNull(iClaprofecmodificacion)) entity.Claprofecmodificacion = dr.GetDateTime(iClaprofecmodificacion);

            return entity;
        }

        #region Mapeo de Campos
        public string Claprocodi = "CLAPROCODI";
        public string Clapronombre = "CLAPRONOMBRE";
        public string Claprousucreacion = "CLAPROUSUCREACION";
        public string Claprofeccreacion = "CLAPROFECCREACION";
        public string Claprousumodificacion = "CLAPROUSUMODIFICACION";
        public string Claprofecmodificacion = "CLAPROFECMODIFICACION";
        #endregion

        #region Querys SQL
        public string SqlListarComboClasesProgramacion
        {
            get { return base.GetSqlXml("ListarComboClasesProgramacion"); }
        }
        #endregion

    }
}
