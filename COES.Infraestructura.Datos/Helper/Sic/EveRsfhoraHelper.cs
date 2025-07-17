using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EVE_RSFHORA
    /// </summary>
    public class EveRsfhoraHelper : HelperBase
    {
        public EveRsfhoraHelper(): base(Consultas.EveRsfhoraSql)
        {
        }

        public EveRsfhoraDTO Create(IDataReader dr)
        {
            EveRsfhoraDTO entity = new EveRsfhoraDTO();

            int iRsfhorcodi = dr.GetOrdinal(this.Rsfhorcodi);
            if (!dr.IsDBNull(iRsfhorcodi)) entity.Rsfhorcodi = Convert.ToInt32(dr.GetValue(iRsfhorcodi));

            int iRsfhorindman = dr.GetOrdinal(this.Rsfhorindman);
            if (!dr.IsDBNull(iRsfhorindman)) entity.Rsfhorindman = dr.GetString(iRsfhorindman);

            int iRsfhorindaut = dr.GetOrdinal(this.Rsfhorindaut);
            if (!dr.IsDBNull(iRsfhorindaut)) entity.Rsfhorindaut = dr.GetString(iRsfhorindaut);

            int iRsfhorcomentario = dr.GetOrdinal(this.Rsfhorcomentario);
            if (!dr.IsDBNull(iRsfhorcomentario)) entity.Rsfhorcomentario = dr.GetString(iRsfhorcomentario);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            int iRsfhorfecha = dr.GetOrdinal(this.Rsfhorfecha);
            if (!dr.IsDBNull(iRsfhorfecha)) entity.Rsfhorfecha = dr.GetDateTime(iRsfhorfecha);

            int iRsfhorinicio = dr.GetOrdinal(this.Rsfhorinicio);
            if (!dr.IsDBNull(iRsfhorinicio)) entity.Rsfhorinicio = dr.GetDateTime(iRsfhorinicio);

            int iRsfhorfin = dr.GetOrdinal(this.Rsfhorfin);
            if (!dr.IsDBNull(iRsfhorfin)) entity.Rsfhorfin = dr.GetDateTime(iRsfhorfin);

            int iRsfhormaximo = dr.GetOrdinal(this.Rsfhormaximo);
            if (!dr.IsDBNull(iRsfhormaximo)) entity.Rsfhormaximo = dr.GetDecimal(iRsfhormaximo);

            int iRsfhorindxml = dr.GetOrdinal(this.Rsfhorindxml);
            if (!dr.IsDBNull(iRsfhorindxml)) entity.Rsfhorindxml = dr.GetString(iRsfhorindxml);

            int iRsfAdd = dr.GetOrdinal(this.RsfAdd);
            if (!dr.IsDBNull(iRsfAdd)) entity.rsfadd = Convert.ToInt32(dr.GetValue(iRsfAdd));

            int iRsfDel = dr.GetOrdinal(this.RsfDel);
            if (!dr.IsDBNull(iRsfDel)) entity.rsfdel = Convert.ToInt32(dr.GetValue(iRsfDel));

            return entity;
        }


        #region Mapeo de Campos

        public string Rsfhorcodi = "RSFHORCODI";
        public string Rsfhorindman = "RSFHORINDMAN";
        public string Rsfhorindaut = "RSFHORINDAUT";
        public string Rsfhorcomentario = "RSFHORCOMENTARIO";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";
        public string Rsfhorfecha = "RSFHORFECHA";
        public string Rsfhorinicio = "RSFHORINICIO";
        public string Rsfhorfin = "RSFHORFIN";
        public string Rsfhormaximo = "RSFHORMAXIMO";
        public string Ursnomb = "GRUPONOMB";
        public string ValorAutomatico = "RSFDETVALAUT";
        public string Rsfhorindxml = "RSFHORINDXML";
        public string RsfAdd = "RSFADD";
        public string RsfDel = "RSFDEL";


        public string SqlValidarExistencia
        {
            get { return base.GetSqlXml("ValidarExistencia"); }
        }

        public string SqlObtenerReporte
        {
            get { return base.GetSqlXml("ObtenerReporte"); }
        }

        public string SqlDeletePorId
        {
            get { return base.GetSqlXml("DeletePorId"); }
        }

        public string SqlActualizarXML
        {
            get { return base.GetSqlXml("ActualizarXML"); }
        }

        public string SqlGetCriteria
        {
            get { return GetSqlXml("GetCriteria2"); }
        }

        public string SqlUpdate2
        {
            get { return GetSqlXml("Update2"); }
        }

        #endregion

        #region Modificación_RSF_05012021
        public string SqlObtenerDatosXML
        {
            get { return base.GetSqlXml("ObtenerDatosXML"); }
        }
        #endregion
    }
}
