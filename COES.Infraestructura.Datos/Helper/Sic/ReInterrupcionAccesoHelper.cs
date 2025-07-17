using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RE_INTERRUPCION_ACCESO
    /// </summary>
    public class ReInterrupcionAccesoHelper : HelperBase
    {
        public ReInterrupcionAccesoHelper(): base(Consultas.ReInterrupcionAccesoSql)
        {
        }

        public ReInterrupcionAccesoDTO Create(IDataReader dr)
        {
            ReInterrupcionAccesoDTO entity = new ReInterrupcionAccesoDTO();

            int iReinaccodi = dr.GetOrdinal(this.Reinaccodi);
            if (!dr.IsDBNull(iReinaccodi)) entity.Reinaccodi = Convert.ToInt32(dr.GetValue(iReinaccodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iRepercodi = dr.GetOrdinal(this.Repercodi);
            if (!dr.IsDBNull(iRepercodi)) entity.Repercodi = Convert.ToInt32(dr.GetValue(iRepercodi));

            int iReinacptoentrega = dr.GetOrdinal(this.Reinacptoentrega);
            if (!dr.IsDBNull(iReinacptoentrega)) entity.Reinacptoentrega = dr.GetString(iReinacptoentrega);

            int iReinacrechazocarga = dr.GetOrdinal(this.Reinacrechazocarga);
            if (!dr.IsDBNull(iReinacrechazocarga)) entity.Reinacrechazocarga = dr.GetString(iReinacrechazocarga);

            int iReinacusucreacion = dr.GetOrdinal(this.Reinacusucreacion);
            if (!dr.IsDBNull(iReinacusucreacion)) entity.Reinacusucreacion = dr.GetString(iReinacusucreacion);

            int iReinacfeccreacion = dr.GetOrdinal(this.Reinacfeccreacion);
            if (!dr.IsDBNull(iReinacfeccreacion)) entity.Reinacfeccreacion = dr.GetDateTime(iReinacfeccreacion);

            int iReinacusumodificacion = dr.GetOrdinal(this.Reinacusumodificacion);
            if (!dr.IsDBNull(iReinacusumodificacion)) entity.Reinacusumodificacion = dr.GetString(iReinacusumodificacion);

            int iReinacfecmodificacion = dr.GetOrdinal(this.Reinacfecmodificacion);
            if (!dr.IsDBNull(iReinacfecmodificacion)) entity.Reinacfecmodificacion = dr.GetDateTime(iReinacfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Reinaccodi = "REINACCODI";
        public string Emprcodi = "EMPRCODI";
        public string Repercodi = "REPERCODI";
        public string Reinacptoentrega = "REINACPTOENTREGA";
        public string Reinacrechazocarga = "REINACRECHAZOCARGA";
        public string Reinacusucreacion = "REINACUSUCREACION";
        public string Reinacfeccreacion = "REINACFECCREACION";
        public string Reinacusumodificacion = "REINACUSUMODIFICACION";
        public string Reinacfecmodificacion = "REINACFECMODIFICACION";
        public string Emprnomb = "EMPRNOMB";
        public string TableName = "RE_INTERRUPCION_ACCESO";

        #endregion

        public string SqlDeletePeriodo
        {
            get { return base.GetSqlXml("DeletePeriodo"); }
        }

        public string SqlListarPorPeriodo
        {
            get { return base.GetSqlXml("ListarPorPeriodo"); }
        }

        
    }
}
