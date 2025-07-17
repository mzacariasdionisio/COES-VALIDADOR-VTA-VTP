using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RCA_CUADRO_EJEC_USU_DET
    /// </summary>
    public class RcaCuadroEjecUsuarioDetHelper : HelperBase
    {
        public RcaCuadroEjecUsuarioDetHelper(): base(Consultas.RcaCuadroEjecUsuarioDetSql)
        {
        }

        public RcaCuadroEjecUsuarioDetDTO Create(IDataReader dr)
        {
            RcaCuadroEjecUsuarioDetDTO entity = new RcaCuadroEjecUsuarioDetDTO();

            int iRcejeucodi = dr.GetOrdinal(this.Rcejeucodi);
            if (!dr.IsDBNull(iRcejeucodi)) entity.Rcejeucodi = Convert.ToInt32(dr.GetValue(iRcejeucodi));

            int iRcejedcodi = dr.GetOrdinal(this.Rcejedcodi);
            if (!dr.IsDBNull(iRcejedcodi)) entity.Rcejedcodi = Convert.ToInt32(dr.GetValue(iRcejedcodi));

            //int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            //if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));           

            int iRcejedpotencia = dr.GetOrdinal(this.Rcejedpotencia);
            if (!dr.IsDBNull(iRcejedpotencia)) entity.Rcejedpotencia = dr.GetDecimal(iRcejedpotencia);          

            int iRcejedfechor = dr.GetOrdinal(this.Rcejedfechor);
            if (!dr.IsDBNull(iRcejedfechor)) entity.Rcejedfechor = dr.GetDateTime(iRcejedfechor);
           
            int iRcejedusucreacion = dr.GetOrdinal(this.Rcejedusucreacion);
            if (!dr.IsDBNull(iRcejedusucreacion)) entity.Rcejedusucreacion = dr.GetString(iRcejedusucreacion);

            int iRcejedfeccreacion = dr.GetOrdinal(this.Rcejedfeccreacion);
            if (!dr.IsDBNull(iRcejedfeccreacion)) entity.Rcejedfeccreacion = dr.GetDateTime(iRcejedfeccreacion);

            int iRcejedusumodificacion = dr.GetOrdinal(this.Rcejedusumodificacion);
            if (!dr.IsDBNull(iRcejedusumodificacion)) entity.Rcejedusumodificacion = dr.GetString(iRcejedusumodificacion);

            int iRcejedfecmodificacion = dr.GetOrdinal(this.Rcejedfecmodificacion);
            if (!dr.IsDBNull(iRcejedfecmodificacion)) entity.Rcejedfecmodificacion = dr.GetDateTime(iRcejedfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Rcejeucodi = "RCEJEUCODI";
        public string Rcejedcodi = "RCEJEDCODI";
        public string Emprcodi = "EMPRCODI";
       
        public string Rcejedpotencia = "RCEJEDPOTENCIA"; 
       
        public string Rcejedfechor = "RCEJEDFECHOR";
       
        public string Rcejedusucreacion = "RCEJEDUSUCREACION";
        public string Rcejedfeccreacion = "RCEJEDFECCREACION";
        public string Rcejedusumodificacion = "RCEJEDUSUMODIFICACION";
        public string Rcejedfecmodificacion = "RCEJEDFECMODIFICACION";
      
        public string Empresa = "EMPRESA";
        //public string Subestacion = "SUBESTACION";
        //public string Puntomedicion = "PUNTOMEDICION";
        //public string Suministrador = "SUMINISTRADOR";      

        #endregion

        public string SqlListFiltro
        {
            get { return GetSqlXml("ListFiltro"); }
        }
        public string SqlDeletePorCliente
        {
            get { return GetSqlXml("DeletePorCliente"); }
        }
    }
}
