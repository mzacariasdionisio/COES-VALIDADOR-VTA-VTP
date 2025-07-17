using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RCA_CARGA_ESENCIAL
    /// </summary>
    public class RcaCargaEsencialHelper : HelperBase
    {
        public RcaCargaEsencialHelper(): base(Consultas.RcaCargaEsencialSql)
        {
        }

        public RcaCargaEsencialDTO Create(IDataReader dr)
        {
            RcaCargaEsencialDTO entity = new RcaCargaEsencialDTO();

            int iRccarecodi = dr.GetOrdinal(this.Rccarecodi);
            if (!dr.IsDBNull(iRccarecodi)) entity.Rccarecodi = Convert.ToInt32(dr.GetValue(iRccarecodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iRccarecarga = dr.GetOrdinal(this.Rccarecarga);
            if (!dr.IsDBNull(iRccarecarga)) entity.Rccarecarga = dr.GetDecimal(iRccarecarga);

            int iRccaredocumento = dr.GetOrdinal(this.Rccaredocumento);
            if (!dr.IsDBNull(iRccaredocumento)) entity.Rccaredocumento = dr.GetString(iRccaredocumento);

            int iRccarefecharecepcion = dr.GetOrdinal(this.Rccarefecharecepcion);
            if (!dr.IsDBNull(iRccarefecharecepcion)) entity.Rccarefecharecepcion = dr.GetDateTime(iRccarefecharecepcion);

            int iRccareestado = dr.GetOrdinal(this.Rccareestado);
            if (!dr.IsDBNull(iRccareestado)) entity.Rccareestado = dr.GetString(iRccareestado);

            int iRccarenombarchivo = dr.GetOrdinal(this.Rccarenombarchivo);
            if (!dr.IsDBNull(iRccarenombarchivo)) entity.Rccarenombarchivo = dr.GetString(iRccarenombarchivo);

            int iRccareestregistro = dr.GetOrdinal(this.Rccareestregistro);
            if (!dr.IsDBNull(iRccareestregistro)) entity.Rccareestregistro = dr.GetString(iRccareestregistro);

            int iRccareusucreacion = dr.GetOrdinal(this.Rccareusucreacion);
            if (!dr.IsDBNull(iRccareusucreacion)) entity.Rccareusucreacion = dr.GetString(iRccareusucreacion);

            int iRccarefeccreacion = dr.GetOrdinal(this.Rccarefeccreacion);
            if (!dr.IsDBNull(iRccarefeccreacion)) entity.Rccarefeccreacion = dr.GetDateTime(iRccarefeccreacion);

            int iRccareusumodificacion = dr.GetOrdinal(this.Rccareusumodificacion);
            if (!dr.IsDBNull(iRccareusumodificacion)) entity.Rccareusumodificacion = dr.GetString(iRccareusumodificacion);

            int iRccarefecmodificacion = dr.GetOrdinal(this.Rccarefecmodificacion);
            if (!dr.IsDBNull(iRccarefecmodificacion)) entity.Rccarefecmodificacion = dr.GetDateTime(iRccarefecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Rccarecodi = "RCCARECODI";
        public string Emprcodi = "EMPRCODI";
        public string Equicodi = "EQUICODI";
        public string Rccarecarga = "RCCARECARGA";
        public string Rccaredocumento = "RCCAREDOCUMENTO";
        public string Rccarefecharecepcion = "RCCAREFECHARECEPCION";
        public string Rccareestado = "RCCAREESTADO";
        public string Rccarenombarchivo = "RCCARENOMBARCHIVO";
        public string Rccareestregistro = "RCCAREESTREGISTRO";
        public string Rccareusucreacion = "RCCAREUSUCREACION";
        public string Rccarefeccreacion = "RCCAREFECCREACION";
        public string Rccareusumodificacion = "RCCAREUSUMODIFICACION";
        public string Rccarefecmodificacion = "RCCAREFECMODIFICACION";

        public string Emprrazsocial = "EMPRRAZSOCIAL";
        public string Tipoemprcodi = "TIPOEMPRCODI";
        public string Equinomb = "EQUINOMB";
        public string Areaabrev = "AREAABREV";
        public string Areanomb = "AREANOMB";
        public string Rccareorigen = "RCCAREORIGEN";

        public string Rccaretipocarga = "RCCARETIPOCARGA";

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

        public string SqlListFiltroPorPuntoMedicion
        {
            get { return base.GetSqlXml("ListFiltroPorPuntoMedicion"); }
        }

        public string SqlListFiltroCount
        {
            get { return base.GetSqlXml("ListFiltroCount"); }
        }

        public string SqlListFiltroExcel
        {
            get { return base.GetSqlXml("ListFiltroExcel"); }
        }
    }
}
