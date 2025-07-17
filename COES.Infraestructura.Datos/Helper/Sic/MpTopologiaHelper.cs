using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla MP_TOPOLOGIA
    /// </summary>
    public class MpTopologiaHelper : HelperBase
    {
        public MpTopologiaHelper(): base(Consultas.MpTopologiaSql)
        {
        }

        public MpTopologiaDTO Create(IDataReader dr)
        {
            MpTopologiaDTO entity = new MpTopologiaDTO();

            int iMtopcodi = dr.GetOrdinal(this.Mtopcodi);
            if (!dr.IsDBNull(iMtopcodi)) entity.Mtopcodi = Convert.ToInt32(dr.GetValue(iMtopcodi));

            int iMtopnomb = dr.GetOrdinal(this.Mtopnomb);
            if (!dr.IsDBNull(iMtopnomb)) entity.Mtopnomb = dr.GetString(iMtopnomb);

            int iMtopversion = dr.GetOrdinal(this.Mtopversion);
            if (!dr.IsDBNull(iMtopversion)) entity.Mtopversion = Convert.ToInt32(dr.GetValue(iMtopversion));

            int iMtopestado = dr.GetOrdinal(this.Mtopestado);
            if (!dr.IsDBNull(iMtopestado)) entity.Mtopestado = Convert.ToInt32(dr.GetValue(iMtopestado));

            int iMtopfecha = dr.GetOrdinal(this.Mtopfecha);
            if (!dr.IsDBNull(iMtopfecha)) entity.Mtopfecha = dr.GetDateTime(iMtopfecha);

            int iMtopfechafutura = dr.GetOrdinal(this.Mtopfechafutura);
            if (!dr.IsDBNull(iMtopfechafutura)) entity.Mtopfechafutura = dr.GetDateTime(iMtopfechafutura);

            int iMtopresolucion = dr.GetOrdinal(this.Mtopresolucion);
            if (!dr.IsDBNull(iMtopresolucion)) entity.Mtopresolucion = dr.GetString(iMtopresolucion);

            int iMtopoficial = dr.GetOrdinal(this.Mtopoficial);
            if (!dr.IsDBNull(iMtopoficial)) entity.Mtopoficial = Convert.ToInt32(dr.GetValue(iMtopoficial));

            int iMtopusuregistro = dr.GetOrdinal(this.Mtopusuregistro);
            if (!dr.IsDBNull(iMtopusuregistro)) entity.Mtopusuregistro = dr.GetString(iMtopusuregistro);

            int iMtopfeccreacion = dr.GetOrdinal(this.Mtopfeccreacion);
            if (!dr.IsDBNull(iMtopfeccreacion)) entity.Mtopfeccreacion = dr.GetDateTime(iMtopfeccreacion);

            int iMtopusumodificacion = dr.GetOrdinal(this.Mtopusumodificacion);
            if (!dr.IsDBNull(iMtopusumodificacion)) entity.Mtopusumodificacion = dr.GetString(iMtopusumodificacion);

            int iMtopfecmodificacion = dr.GetOrdinal(this.Mtopfecmodificacion);
            if (!dr.IsDBNull(iMtopfecmodificacion)) entity.Mtopfecmodificacion = dr.GetDateTime(iMtopfecmodificacion);

            int iMtopcodipadre = dr.GetOrdinal(this.Mtopcodipadre);
            if (!dr.IsDBNull(iMtopcodipadre)) entity.Mtopcodipadre = Convert.ToInt32(dr.GetValue(iMtopcodipadre));

            return entity;
        }


        #region Mapeo de Campos

        public string Mtopcodi = "MTOPCODI";
        public string Mtopnomb = "MTOPNOMB";
        public string Mtopversion = "MTOPVERSION";
        public string Mtopestado = "MTOPESTADO";
        public string Mtopfecha = "MTOPFECHA";
        public string Mtopfechafutura = "MTOPFECHAFUTURA";
        public string Mtopresolucion = "MTOPRESOLUCION";
        public string Mtopoficial = "MTOPOFICIAL";
        public string Mtopusuregistro = "MTOPUSUREGISTRO";
        public string Mtopfeccreacion = "MTOPFECCREACION";
        public string Mtopusumodificacion = "MTOPUSUMODIFICACION";
        public string Mtopfecmodificacion = "MTOPFECMODIFICACION";
        public string Mtopcodipadre = "MTOPCODIPADRE";
        

        #endregion

        public string SqlListarEscenariosSddp
        {
            get { return base.GetSqlXml("ListarEscenariosSddp"); }
        }
    }
}
