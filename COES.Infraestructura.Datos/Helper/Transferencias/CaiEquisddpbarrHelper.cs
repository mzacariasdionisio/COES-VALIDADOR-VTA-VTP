using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CAI_EQUISDDPBARR
    /// </summary>
    public class CaiEquisddpbarrHelper : HelperBase
    {
        public CaiEquisddpbarrHelper(): base(Consultas.CaiEquisddpbarrSql)
        {
        }

        public CaiEquisddpbarrDTO Create(IDataReader dr)
        {
            CaiEquisddpbarrDTO entity = new CaiEquisddpbarrDTO();

            int iCasddbcodi = dr.GetOrdinal(this.Casddbcodi);
            if (!dr.IsDBNull(iCasddbcodi)) entity.Casddbcodi = Convert.ToInt32(dr.GetValue(iCasddbcodi));

            int iBarrcodi = dr.GetOrdinal(this.Barrcodi);
            if (!dr.IsDBNull(iBarrcodi)) entity.Barrcodi = Convert.ToInt32(dr.GetValue(iBarrcodi));

            int iCasddbbarra = dr.GetOrdinal(this.Casddbbarra);
            if (!dr.IsDBNull(iCasddbbarra)) entity.Casddbbarra = dr.GetString(iCasddbbarra);

            int iCasddbfecvigencia = dr.GetOrdinal(this.Casddbfecvigencia);
            if (!dr.IsDBNull(iCasddbfecvigencia)) entity.Casddbfecvigencia = dr.GetDateTime(iCasddbfecvigencia);

            int iCasddbusucreacion = dr.GetOrdinal(this.Casddbusucreacion);
            if (!dr.IsDBNull(iCasddbusucreacion)) entity.Casddbusucreacion = dr.GetString(iCasddbusucreacion);

            int iCasddbfeccreacion = dr.GetOrdinal(this.Casddbfeccreacion);
            if (!dr.IsDBNull(iCasddbfeccreacion)) entity.Casddbfeccreacion = dr.GetDateTime(iCasddbfeccreacion);

            int iCasddbusumodificacion = dr.GetOrdinal(this.Casddbusumodificacion);
            if (!dr.IsDBNull(iCasddbusumodificacion)) entity.Casddbusumodificacion = dr.GetString(iCasddbusumodificacion);

            int iCasddbfecmodificacion = dr.GetOrdinal(this.Casddbfecmodificacion);
            if (!dr.IsDBNull(iCasddbfecmodificacion)) entity.Casddbfecmodificacion = dr.GetDateTime(iCasddbfecmodificacion);

            int iCasddbfactbarrpmpo = dr.GetOrdinal(this.Casddbfactbarrpmpo);
            if (!dr.IsDBNull(iCasddbfactbarrpmpo)) entity.Casddbfactbarrpmpo = Convert.ToDecimal(dr.GetValue(iCasddbfactbarrpmpo));

            return entity;
        }


        #region Mapeo de Campos

        public string Casddbcodi = "CASDDBCODI";
        public string Barrcodi = "BARRCODI";
        public string Casddbbarra = "CASDDBBARRA";
        public string Casddbfecvigencia = "CASDDBFECVIGENCIA";
        public string Casddbusucreacion = "CASDDBUSUCREACION";
        public string Casddbfeccreacion = "CASDDBFECCREACION";
        public string Casddbusumodificacion = "CASDDBUSUMODIFICACION";
        public string Casddbfecmodificacion = "CASDDBFECMODIFICACION";
        public string Casddbfactbarrpmpo = "CASDDBFACTBARRPMPO";

        public string Barrnombre = "BARRNOMBRE";

        #endregion

        public string SqlListEquisddpbarr
        {
            get { return GetSqlXml("ListCaiEquisddpbarr"); }
        }

        public string GetByIdCaiEquisddpbarr
        {
            get { return GetSqlXml("GetByIdCaiEquisddpbarr"); }
        }

        public string GetByNombreBarraSddp
        {
            get { return GetSqlXml("GetByNombreBarraSddp"); }
        }

        public string GetByCriteriaCaiEquiunidbarrsNoIns
        {
            get { return GetSqlXml("GetByCriteriaCaiEquiunidbarrsNoIns"); }
        }

        public string GetByBarraNombreSddp
        {
            get { return GetSqlXml("GetByBarraNombreSddp"); }
        }
    }
}
