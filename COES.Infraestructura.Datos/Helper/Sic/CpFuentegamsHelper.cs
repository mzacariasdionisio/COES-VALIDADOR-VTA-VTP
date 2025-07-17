using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CP_FUENTEGAMS
    /// </summary>
    public class CpFuentegamsHelper : HelperBase
    {
        public CpFuentegamsHelper(): base(Consultas.CpFuentegamsSql)
        {
        }

        public CpFuentegamsDTO Create(IDataReader dr)
        {
            CpFuentegamsDTO entity = new CpFuentegamsDTO();

            int iFtegcodi = dr.GetOrdinal(this.Ftegcodi);
            if (!dr.IsDBNull(iFtegcodi)) entity.Ftegcodi = Convert.ToInt32(dr.GetValue(iFtegcodi));

            int iFtegnombre = dr.GetOrdinal(this.Ftegnombre);
            if (!dr.IsDBNull(iFtegnombre)) entity.Ftegnombre = dr.GetString(iFtegnombre);

            int iFtgedefault = dr.GetOrdinal(this.Ftgedefault);
            if (!dr.IsDBNull(iFtgedefault)) entity.Ftegdefault = Convert.ToInt32(dr.GetValue(iFtgedefault));

            int iFtegestado = dr.GetOrdinal(this.Ftegestado);
            if (!dr.IsDBNull(iFtegestado)) entity.Ftegestado = Convert.ToInt32(dr.GetValue(iFtegestado));

            int iFtemetodo = dr.GetOrdinal(this.Ftemetodo);
            if (!dr.IsDBNull(iFtemetodo)) entity.Ftemetodo = Convert.ToInt32(dr.GetValue(iFtemetodo));

            int iFtegusumodificacion = dr.GetOrdinal(this.Ftegusumodificacion);
            if (!dr.IsDBNull(iFtegusumodificacion)) entity.Ftegusumodificacion = dr.GetString(iFtegusumodificacion);

            int iFtegfecmodificacion = dr.GetOrdinal(this.Ftegfecmodificacion);
            if (!dr.IsDBNull(iFtegfecmodificacion)) entity.Ftegfecmodificacion = dr.GetDateTime(iFtegfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Ftegcodi = "FTEGCODI";
        public string Ftegnombre = "FTEGNOMBRE";
        public string Ftegcodigoencrip = "FTEGCODIGOENCRIP";
        public string Ftgedefault = "FTEGDEFAULT";
        public string Ftegestado = "FTEGESTADO";
        public string Ftemetodo = "FTEMETODO";
        public string Ftegusumodificacion = "FTEGUSUMODIFICACION";
        public string Ftegfecmodificacion = "FTEGFECMODIFICACION";
        public string Fteginputdata = "FTEGINPUTDATA";
        public string Ftegruncase = "FTEGRUNCASE";

        public string Fverscodi = "FVERSCODI";
        public string Fversnum = "FVERSNUM";
        public string Fversdescrip = "FVERSDESCRIP";
        public string Fversusumodificacion = "FVERSUSUMODIFICACION";
        public string Fversfecmodificacion = "FVERSFECMODIFICACION";
        public string Fversestado = "FVERSESTADO";
        public string Fversinputdata = "FVERSINPUTDATA";
        public string Fversruncase = "FVERSRUNCASE";
        public string Fverscodigoencrip = "FVERSCODIGOENCRIP";

        #endregion

        public string SqlResetOficial
        {
            get { return base.GetSqlXml("ResetOficial"); }
        }

        public string SqlOficial
        {
            get { return base.GetSqlXml("Oficial"); }
        }

        public string SqlGetByIdVersion
        {
            get { return base.GetSqlXml("GetByIdVersion"); }
        }
    }
}
