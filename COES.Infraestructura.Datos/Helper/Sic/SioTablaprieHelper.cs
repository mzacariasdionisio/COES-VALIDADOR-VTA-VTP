using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SIO_TABLAPRIE
    /// </summary>
    public class SioTablaprieHelper : HelperBase
    {
        public SioTablaprieHelper()
            : base(Consultas.SioTablaprieSql)
        {
        }

        public SioTablaprieDTO Create(IDataReader dr)
        {
            SioTablaprieDTO entity = new SioTablaprieDTO();

            int iTpriecodi = dr.GetOrdinal(this.Tpriecodi);
            if (!dr.IsDBNull(iTpriecodi)) entity.Tpriecodi = Convert.ToInt32(dr.GetValue(iTpriecodi));

            int iTpriedscripcion = dr.GetOrdinal(this.Tpriedscripcion);
            if (!dr.IsDBNull(iTpriedscripcion)) entity.Tpriedscripcion = dr.GetString(iTpriedscripcion);

            int iTpriefechaplazo = dr.GetOrdinal(this.Tpriefechaplazo);
            if (!dr.IsDBNull(iTpriefechaplazo)) entity.Tpriefechaplazo = dr.GetDateTime(iTpriefechaplazo);

            int iAreacodi = dr.GetOrdinal(this.Areacodi);
            if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

            int iTprieabrev = dr.GetOrdinal(this.Tprieabrev);
            if (!dr.IsDBNull(iTprieabrev)) entity.Tprieabrev = dr.GetString(iTprieabrev);

            int iTprieusumodificacion = dr.GetOrdinal(this.Tprieusumodificacion);
            if (!dr.IsDBNull(iTprieusumodificacion)) entity.Tprieusumodificacion = dr.GetString(iTprieusumodificacion);

            int iTpriefecmodificacion = dr.GetOrdinal(this.Tpriefecmodificacion);
            if (!dr.IsDBNull(iTpriefecmodificacion)) entity.Tpriefecmodificacion = dr.GetDateTime(iTpriefecmodificacion);

            int iTprieusucreacion = dr.GetOrdinal(this.Tprieusucreacion);
            if (!dr.IsDBNull(iTprieusucreacion)) entity.Tprieusucreacion = dr.GetString(iTprieusucreacion);

            int iTpriequery = dr.GetOrdinal(this.Tpriequery);
            if (!dr.IsDBNull(iTpriequery)) entity.Tpriequery = dr.GetString(iTpriequery);

            int iTprieffeccreacion = dr.GetOrdinal(this.Tprieffeccreacion);
            if (!dr.IsDBNull(iTprieffeccreacion)) entity.Tprieffeccreacion = dr.GetDateTime(iTprieffeccreacion);

            int iTprieresolucion = dr.GetOrdinal(this.Tprieresolucion);
            if (!dr.IsDBNull(iTprieresolucion)) entity.Tprieresolucion = Convert.ToInt32(dr.GetValue(iTprieresolucion));

            int iTpriefechacierre = dr.GetOrdinal(this.Tpriefechacierre);
            if (!dr.IsDBNull(iTpriefechacierre)) entity.Tpriefechacierre = dr.GetDateTime(iTpriefechacierre);

            //int iTprieusutabla = dr.GetOrdinal(this.Tprieusutabla);
            //if (!dr.IsDBNull(iTprieusutabla)) entity.Tprieusutabla = dr.GetString(iTprieusutabla);
            //int iTpriecodtablaosig = dr.GetOrdinal(this.Tpriecodtablaosig);
            //if (!dr.IsDBNull(iTpriecodtablaosig)) entity.Tpriecodtablaosig = dr.GetString(iTpriecodtablaosig);

            return entity;
        }


        #region Mapeo de Campos

        public string Tpriecodi = "TPRIECODI";
        public string Tpriedscripcion = "TPRIEDESCRIPCION";
        public string Tpriefechaplazo = "TPRIEFECHAPLAZO";
        public string Areacodi = "AREACODE";
        public string Tprieabrev = "TPRIEABREV";
        public string Tprieusumodificacion = "TPRIEUSUMODIFICACION";
        public string Tpriefecmodificacion = "TPRIEFECMODIFICACION";
        public string Tprieusucreacion = "TPRIEUSUCREACION";
        public string Tpriequery = "TPRIEQUERY";
        public string Tprieffeccreacion = "TPRIEFECCREACION";
        public string Tprieresolucion = "TPRIERESOLUCION";
        public string Tpriefechacierre = "TPRIEFECHACIERRE";
        //public string Tprieusutabla = "TPRIEUSUTABLA";
        public string Tpriecodtablaosig = "Tpriecodtablaosig";

        #region SIOSEIN
        public string CantidadVersion = "CANTIDADVERSION";
        public string Cabpritieneregistros = "CABPRITIENEREGISTROS";
        #endregion

        #endregion

        #region SIOSEIN
        public string SqlGetByPeriodo
        {
            get { return GetSqlXml("SqlGetByPeriodo"); }
        }
        #endregion
    }
}