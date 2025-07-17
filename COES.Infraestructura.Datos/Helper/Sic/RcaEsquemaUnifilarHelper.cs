using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RCA_ESQUEMA_UNIFILAR
    /// </summary>
    public class RcaEsquemaUnifilarHelper : HelperBase
    {
        public RcaEsquemaUnifilarHelper(): base(Consultas.RcaEsquemaUnifilarSql)
        {
        }

        public RcaEsquemaUnifilarDTO Create(IDataReader dr)
        {
            RcaEsquemaUnifilarDTO entity = new RcaEsquemaUnifilarDTO();

            int iRcesqucodi = dr.GetOrdinal(this.Rcesqucodi);
            if (!dr.IsDBNull(iRcesqucodi)) entity.Rcesqucodi = Convert.ToInt32(dr.GetValue(iRcesqucodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iRcesqudocumento = dr.GetOrdinal(this.Rcesqudocumento);
            if (!dr.IsDBNull(iRcesqudocumento)) entity.Rcesqudocumento = dr.GetString(iRcesqudocumento);

            int iRcesqufecharecepcion = dr.GetOrdinal(this.Rcesqufecharecepcion);
            if (!dr.IsDBNull(iRcesqufecharecepcion)) entity.Rcesqufecharecepcion = dr.GetDateTime(iRcesqufecharecepcion);

            int iRcesquestado = dr.GetOrdinal(this.Rcesquestado);
            if (!dr.IsDBNull(iRcesquestado)) entity.Rcesquestado = dr.GetString(iRcesquestado);

            int iRcesqunombarchivo = dr.GetOrdinal(this.Rcesqunombarchivo);
            if (!dr.IsDBNull(iRcesqunombarchivo)) entity.Rcesqunombarchivo = dr.GetString(iRcesqunombarchivo);

            int iRcesquestregistro = dr.GetOrdinal(this.Rcesquestregistro);
            if (!dr.IsDBNull(iRcesquestregistro)) entity.Rcesquestregistro = dr.GetString(iRcesquestregistro);

            int iRcesquusucreacion = dr.GetOrdinal(this.Rcesquusucreacion);
            if (!dr.IsDBNull(iRcesquusucreacion)) entity.Rcesquusucreacion = dr.GetString(iRcesquusucreacion);

            int iRcesqufeccreacion = dr.GetOrdinal(this.Rcesqufeccreacion);
            if (!dr.IsDBNull(iRcesqufeccreacion)) entity.Rcesqufeccreacion = dr.GetDateTime(iRcesqufeccreacion);

            int iRcesquusumodificacion = dr.GetOrdinal(this.Rcesquusumodificacion);
            if (!dr.IsDBNull(iRcesquusumodificacion)) entity.Rcesquusumodificacion = dr.GetString(iRcesquusumodificacion);

            int iRcesqufecmodificacion = dr.GetOrdinal(this.Rcesqufecmodificacion);
            if (!dr.IsDBNull(iRcesqufecmodificacion)) entity.Rcesqufecmodificacion = dr.GetDateTime(iRcesqufecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Rcesqucodi = "RCESQUCODI";
        public string Emprcodi = "EMPRCODI";
        public string Equicodi = "EQUICODI";
        public string Rcesqudocumento = "RCESQUDOCUMENTO";
        public string Rcesqufecharecepcion = "RCESQUFECHARECEPCION";
        public string Rcesquestado = "RCESQUESTADO";
        public string Rcesqunombarchivo = "RCESQUNOMBARCHIVO";
        public string Rcesquestregistro = "RCESQUESTREGISTRO";
        public string Rcesquusucreacion = "RCESQUUSUCREACION";
        public string Rcesqufeccreacion = "RCESQUFECCREACION";
        public string Rcesquusumodificacion = "RCESQUUSUMODIFICACION";
        public string Rcesqufecmodificacion = "RCESQUFECMODIFICACION";

        public string Emprrazsocial = "EMPRRAZSOCIAL";
        public string Tipoemprcodi = "TIPOEMPRCODI";
        public string Equinomb = "EQUINOMB";
        public string Areaabrev = "AREAABREV";
        public string Areanomb = "AREANOMB";

        public string Osinergcodi = "OSINERGCODI";
        public string Rcesquorigen = "RCESQUORIGEN";

        public string Rcesqunombarchivodatos = "RCESQUNOMBARCHIVODATOS";
        public string Qregistros = "Q_REGISTROS";

        #endregion

        public string SqlListFiltro
        {
            get { return base.GetSqlXml("ListFiltro"); }
        }

        public string SqlListHistorial
        {
            get { return base.GetSqlXml("ListHistorial"); }
        }
        public string SqlObtenerPorCodigo
        {
            get { return base.GetSqlXml("ObtenerPorCodigo"); }
        }

        public string SqlListFiltroExcel
        {
            get { return base.GetSqlXml("ListFiltroExcel"); }
        }

        public string SqlListFiltroCount
        {
            get { return base.GetSqlXml("ListFiltroCount"); }
        }
    }
}
