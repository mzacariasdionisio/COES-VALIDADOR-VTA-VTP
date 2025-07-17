using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RCA_CUADRO_PROG_DISTRIB
    /// </summary>
    public class RcaCuadroProgDistribHelper : HelperBase
    {
        public RcaCuadroProgDistribHelper(): base(Consultas.RcaCuadroProgDistribSql)
        {
        }

        public RcaCuadroProgDistribDTO Create(IDataReader dr)
        {
            RcaCuadroProgDistribDTO entity = new RcaCuadroProgDistribDTO();

            int iRcprouusumodificacion = dr.GetOrdinal(this.Rcprodusumodificacion);
            if (!dr.IsDBNull(iRcprouusumodificacion)) entity.Rcprodusumodificacion = dr.GetString(iRcprouusumodificacion);

            int iRcproufecmodificacion = dr.GetOrdinal(this.Rcprodfecmodificacion);
            if (!dr.IsDBNull(iRcproufecmodificacion)) entity.Rcprodfecmodificacion = dr.GetDateTime(iRcproufecmodificacion);

            int iRcproucodi = dr.GetOrdinal(this.Rcprodcodi);
            if (!dr.IsDBNull(iRcproucodi)) entity.Rcprodcodi = Convert.ToInt32(dr.GetValue(iRcproucodi));

            int iRccuadcodi = dr.GetOrdinal(this.Rccuadcodi);
            if (!dr.IsDBNull(iRccuadcodi)) entity.Rccuadcodi = Convert.ToInt32(dr.GetValue(iRccuadcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iRcprodsubestacion = dr.GetOrdinal(this.Rcprodsubestacion);
            if (!dr.IsDBNull(iRcprodsubestacion)) entity.Rcprodsubestacion = dr.GetString(iRcprodsubestacion);

            int iRcproudemanda = dr.GetOrdinal(this.Rcproddemanda);
            if (!dr.IsDBNull(iRcproudemanda)) entity.Rcproddemanda = dr.GetDecimal(iRcproudemanda);

            int iRcprodcargarechazar = dr.GetOrdinal(this.Rcprodcargarechazar);
            if (!dr.IsDBNull(iRcprodcargarechazar)) entity.Rcprodcargarechazar = dr.GetDecimal(iRcprodcargarechazar);                      

            int iRcprouestregistro = dr.GetOrdinal(this.Rcprodestregistro);
            if (!dr.IsDBNull(iRcprouestregistro)) entity.Rcprodestregistro = dr.GetString(iRcprouestregistro);

            int iRcprouusucreacion = dr.GetOrdinal(this.Rcprodusucreacion);
            if (!dr.IsDBNull(iRcprouusucreacion)) entity.Rcprodusucreacion = dr.GetString(iRcprouusucreacion);

            int iRcproufeccreacion = dr.GetOrdinal(this.Rcprodfeccreacion);
            if (!dr.IsDBNull(iRcproufeccreacion)) entity.Rcprodfeccreacion = dr.GetDateTime(iRcproufeccreacion);

            int iEmpresa = dr.GetOrdinal(this.Empresa);
            if (!dr.IsDBNull(iEmpresa)) entity.Empresa = dr.GetString(iEmpresa);

            return entity;
        }


        #region Mapeo de Campos

        public string Rcprodusumodificacion = "RCPRODUSUMODIFICACION";
        public string Rcprodfecmodificacion = "RCPRODFECMODIFICACION";
        public string Rcprodcodi = "RCPRODCODI";
        public string Rccuadcodi = "RCCUADCODI";
        public string Emprcodi = "EMPRCODI";
        
        public string Rcprodsubestacion = "RCPRODSUBESTACION";
        public string Rcproddemanda = "RCPRODDEMANDA";
        public string Rcprodcargarechazar = "RCPRODCARGARECHAZAR";
        public string Rcprodestregistro = "RCPRODESTREGISTRO";
        public string Rcprodusucreacion = "RCPRODUSUCREACION";
        public string Rcprodfeccreacion = "RCPRODFECCREACION";       

        public string Empresa = "EMPRESA";        
        public string Subestacion = "SUBESTACION";
       

        #endregion

        public string SqlListCuadroProgDistrib
        {
            get { return base.GetSqlXml("ListCuadroProgDistrib"); }
        }      

       
    }
}
