using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla AGC_CONTROL
    /// </summary>
    public class AgcControlHelper : HelperBase
    {
        public AgcControlHelper(): base(Consultas.AgcControlSql)
        {
        }

        public AgcControlDTO Create(IDataReader dr)
        {
            AgcControlDTO entity = new AgcControlDTO();

            int iAgcccodi = dr.GetOrdinal(this.Agcccodi);
            if (!dr.IsDBNull(iAgcccodi)) entity.Agcccodi = Convert.ToInt32(dr.GetValue(iAgcccodi));

            int iAgcctipo = dr.GetOrdinal(this.Agcctipo);
            if (!dr.IsDBNull(iAgcctipo)) entity.Agcctipo = dr.GetString(iAgcctipo);

            int iAgccdescrip = dr.GetOrdinal(this.Agccdescrip);
            if (!dr.IsDBNull(iAgccdescrip)) entity.Agccdescrip = dr.GetString(iAgccdescrip);

            int iPtomedicodi = dr.GetOrdinal(this.Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

            int iAgccb2 = dr.GetOrdinal(this.Agccb2);
            if (!dr.IsDBNull(iAgccb2)) entity.Agccb2 = dr.GetString(iAgccb2);

            int iAgccb3 = dr.GetOrdinal(this.Agccb3);
            if (!dr.IsDBNull(iAgccb3)) entity.Agccb3 = dr.GetString(iAgccb3);

            int iAgccvalido = dr.GetOrdinal(this.Agccvalido);
            if (!dr.IsDBNull(iAgccvalido)) entity.Agccvalido = dr.GetString(iAgccvalido);

            int iAgccusucreacion = dr.GetOrdinal(this.Agccusucreacion);
            if (!dr.IsDBNull(iAgccusucreacion)) entity.Agccusucreacion = dr.GetString(iAgccusucreacion);

            int iAgccfeccreacion = dr.GetOrdinal(this.Agccfeccreacion);
            if (!dr.IsDBNull(iAgccfeccreacion)) entity.Agccfeccreacion = dr.GetDateTime(iAgccfeccreacion);

            int iAgccusumodificacion = dr.GetOrdinal(this.Agccusumodificacion);
            if (!dr.IsDBNull(iAgccusumodificacion)) entity.Agccusumodificacion = dr.GetString(iAgccusumodificacion);

            int iAgccfecmodificacion = dr.GetOrdinal(this.Agccfecmodificacion);
            if (!dr.IsDBNull(iAgccfecmodificacion)) entity.Agccfecmodificacion = dr.GetDateTime(iAgccfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Agcccodi = "AGCCCODI";
        public string Agcctipo = "AGCCTIPO";
        public string Agccdescrip = "AGCCDESCRIP";
        public string Ptomedicodi = "PTOMEDICODI";
        public string Agccb2 = "AGCCB2";
        public string Agccb3 = "AGCCB3";
        public string Agccvalido = "AGCCVALIDO";
        public string Agccusucreacion = "AGCCUSUCREACION";
        public string Agccfeccreacion = "AGCCFECCREACION";
        public string Agccusumodificacion = "AGCCUSUMODIFICACION";
        public string Agccfecmodificacion = "AGCCFECMODIFICACION";
        public string Ptomedibarranomb = "PTOMEDIBARRANOMB";

        public string Ptomedielenomb = "PTOMEDIELENOMB";        
        public string Ptomedidesc = "PTOMEDIDESC";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";

        public string Grupocodi = "GRUPOCODI";


        public string ObtenerListado
        {
            get { return base.GetSqlXml("ObtenerListado"); }
        }

        public string TotalRegistros
        {
            get { return base.GetSqlXml("TotalRegistros"); }
        }

        public string ObtenerListadoPotencia
        {
            get { return base.GetSqlXml("ObtenerListadoPotencia"); }
        }

        /*
        public string SqlUpdateMePtomedicion
        {
            get { return base.GetSqlXml("UpdateMePtomedicion"); }
        }

        public string SqlUpdateMePtomedicionCVariable
        {
            get { return base.GetSqlXml("UpdateMePtomedicionCVariable"); }
        }*/

        

        

        #endregion
    }
}
