using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RCA_CUADRO_EJEC_USUARIO
    /// </summary>
    public class RcaCuadroEjecUsuarioHelper : HelperBase
    {
        public RcaCuadroEjecUsuarioHelper(): base(Consultas.RcaCuadroEjecUsuarioSql)
        {
        }

        public RcaCuadroEjecUsuarioDTO Create(IDataReader dr)
        {
            RcaCuadroEjecUsuarioDTO entity = new RcaCuadroEjecUsuarioDTO();

            int iRcejeucodi = dr.GetOrdinal(this.Rcejeucodi);
            if (!dr.IsDBNull(iRcejeucodi)) entity.Rcejeucodi = Convert.ToInt32(dr.GetValue(iRcejeucodi));

            int iRcproucodi = dr.GetOrdinal(this.Rcproucodi);
            if (!dr.IsDBNull(iRcproucodi)) entity.Rcproucodi = Convert.ToInt32(dr.GetValue(iRcproucodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iRcejeucargarechazada = dr.GetOrdinal(this.Rcejeucargarechazada);
            if (!dr.IsDBNull(iRcejeucargarechazada)) entity.Rcejeucargarechazada = dr.GetDecimal(iRcejeucargarechazada);

            int iRcejeutiporeporte = dr.GetOrdinal(this.Rcejeutiporeporte);
            if (!dr.IsDBNull(iRcejeutiporeporte)) entity.Rcejeutiporeporte = dr.GetString(iRcejeutiporeporte);

            int iRcejeufechorinicio = dr.GetOrdinal(this.Rcejeufechorinicio);
            if (!dr.IsDBNull(iRcejeufechorinicio)) entity.Rcejeufechorinicio = dr.GetDateTime(iRcejeufechorinicio);

            int iRcejeufechorfin = dr.GetOrdinal(this.Rcejeufechorfin);
            if (!dr.IsDBNull(iRcejeufechorfin)) entity.Rcejeufechorfin = dr.GetDateTime(iRcejeufechorfin);

            int iRcejeuestregistro = dr.GetOrdinal(this.Rcejeuestregistro);
            if (!dr.IsDBNull(iRcejeuestregistro)) entity.Rcejeuestregistro = dr.GetString(iRcejeuestregistro);

            int iRcejeuusucreacion = dr.GetOrdinal(this.Rcejeuusucreacion);
            if (!dr.IsDBNull(iRcejeuusucreacion)) entity.Rcejeuusucreacion = dr.GetString(iRcejeuusucreacion);

            int iRcejeufeccreacion = dr.GetOrdinal(this.Rcejeufeccreacion);
            if (!dr.IsDBNull(iRcejeufeccreacion)) entity.Rcejeufeccreacion = dr.GetDateTime(iRcejeufeccreacion);

            int iRcejeuusumodificacion = dr.GetOrdinal(this.Rcejeuusumodificacion);
            if (!dr.IsDBNull(iRcejeuusumodificacion)) entity.Rcejeuusumodificacion = dr.GetString(iRcejeuusumodificacion);

            int iRcejeufecmodificacion = dr.GetOrdinal(this.Rcejeufecmodificacion);
            if (!dr.IsDBNull(iRcejeufecmodificacion)) entity.Rcejeufecmodificacion = dr.GetDateTime(iRcejeufecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Rcejeucodi = "RCEJEUCODI";
        public string Rcproucodi = "RCPROUCODI";
        public string Emprcodi = "EMPRCODI";
        public string Equicodi = "EQUICODI";
        public string Rcejeucargarechazada = "RCEJEUCARGARECHAZADA";
        public string Rcejeutiporeporte = "RCEJEUTIPOREPORTE";
        public string Rcejeufechorinicio = "RCEJEUFECHORINICIO";
        public string Rcejeufechorfin = "RCEJEUFECHORFIN";
        public string Rcejeuestregistro = "RCEJEUESTREGISTRO";
        public string Rcejeuusucreacion = "RCEJEUUSUCREACION";
        public string Rcejeufeccreacion = "RCEJEUFECCREACION";
        public string Rcejeuusumodificacion = "RCEJEUUSUMODIFICACION";
        public string Rcejeufecmodificacion = "RCEJEUFECMODIFICACION";

        public string Rcproudemanda = "RCPROUDEMANDA";
        public string Rcproufuente = "RCPROUFUENTE";
        public string Rcproudemandaracionar = "RCPROUDEMANDARACIONAR";
        public string Rcprouemprcodisuministrador = "RCPROUEMPRCODISUMINISTRADOR";
        public string Empresa = "EMPRESA";
        public string Subestacion = "SUBESTACION";
        public string Puntomedicion = "PUNTOMEDICION";
        public string Suministrador = "SUMINISTRADOR";

        public string Rcproudemandareal = "RCPROUDEMANDAREAL";

        #endregion

        public string SqlListFiltro
        {
            get { return GetSqlXml("ListFiltro"); }
        }
    }
}
