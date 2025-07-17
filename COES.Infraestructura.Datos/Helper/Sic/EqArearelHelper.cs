using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EQ_AREAREL
    /// </summary>
    public class EqArearelHelper : HelperBase
    {
        public EqArearelHelper(): base(Consultas.EqArearelSql)
        {
        }

        public EqAreaRelDTO Create(IDataReader dr)
        {
            EqAreaRelDTO entity = new EqAreaRelDTO();

            int iArearlcodi = dr.GetOrdinal(this.Arearlcodi);
            if (!dr.IsDBNull(iArearlcodi)) entity.AreaRlCodi = Convert.ToInt32(dr.GetValue(iArearlcodi));

            int iAreapadre = dr.GetOrdinal(this.Areapadre);
            if (!dr.IsDBNull(iAreapadre)) entity.AreaPadre = Convert.ToInt32(dr.GetValue(iAreapadre));

            int iAreacodi = dr.GetOrdinal(this.Areacodi);
            if (!dr.IsDBNull(iAreacodi)) entity.AreaCodi = Convert.ToInt32(dr.GetValue(iAreacodi));

            int iFechadat = dr.GetOrdinal(this.Fechadat);
            if (!dr.IsDBNull(iFechadat)) entity.FechaDat = dr.GetDateTime(iFechadat);

            int iLastcodi = dr.GetOrdinal(this.Lastcodi);
            if (!dr.IsDBNull(iLastcodi)) entity.LastCodi = Convert.ToInt32(dr.GetValue(iLastcodi));

            int iArearlusumodificacion = dr.GetOrdinal(this.Arearlusumodificacion);
            if (!dr.IsDBNull(iArearlusumodificacion)) entity.Arearlusumodificacion = dr.GetString(iArearlusumodificacion);


            int iArearlfecmodificacion = dr.GetOrdinal(this.Arearlfecmodificacion);
            if (!dr.IsDBNull(iArearlfecmodificacion)) entity.Arearlfecmodificacion = dr.GetDateTime(iArearlfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Arearlcodi = "AREARLCODI";
        public string Areapadre = "AREAPADRE";
        public string Areacodi = "AREACODI";
        public string Fechadat = "FECHADAT";
        public string Lastcodi = "LASTCODI";
        public string Arearlusumodificacion = "AREARLUSUMODIFICACION";
        public string Arearlfecmodificacion = "AREARLFECMODIFICACION";

        #endregion

        #region ZONAS
        public string SqlListarAreasxAreapadre
        {
            get { return base.GetSqlXml("ListarAreasxAreapadre"); }
        }

        public string SqlGetxAreapadrexAreacodi
        {
            get { return base.GetSqlXml("GetxAreapadrexAreacodi"); }
        }
        #endregion
    }
}
