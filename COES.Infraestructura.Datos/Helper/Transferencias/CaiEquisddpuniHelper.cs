using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CAI_EQUISDDPUNI
    /// </summary>
    public class CaiEquisddpuniHelper : HelperBase
    {
        public CaiEquisddpuniHelper(): base(Consultas.CaiEquisddpuniSql)
        {
        }

        public CaiEquisddpuniDTO Create(IDataReader dr)
        {
            CaiEquisddpuniDTO entity = new CaiEquisddpuniDTO();

            int iCasdducodi = dr.GetOrdinal(this.Casdducodi);
            if (!dr.IsDBNull(iCasdducodi)) entity.Casdducodi = Convert.ToInt32(dr.GetValue(iCasdducodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iCasdduunidad = dr.GetOrdinal(this.Casdduunidad);
            if (!dr.IsDBNull(iCasdduunidad)) entity.Casdduunidad = dr.GetString(iCasdduunidad);

            int iCasddufecvigencia = dr.GetOrdinal(this.Casddufecvigencia);
            if (!dr.IsDBNull(iCasddufecvigencia)) entity.Casddufecvigencia = dr.GetDateTime(iCasddufecvigencia);

            int iCasdduusucreacion = dr.GetOrdinal(this.Casdduusucreacion);
            if (!dr.IsDBNull(iCasdduusucreacion)) entity.Casdduusucreacion = dr.GetString(iCasdduusucreacion);

            int iCasddufeccreacion = dr.GetOrdinal(this.Casddufeccreacion);
            if (!dr.IsDBNull(iCasddufeccreacion)) entity.Casddufeccreacion = dr.GetDateTime(iCasddufeccreacion);

            int iCasdduusumodificacion = dr.GetOrdinal(this.Casdduusumodificacion);
            if (!dr.IsDBNull(iCasdduusumodificacion)) entity.Casdduusumodificacion = dr.GetString(iCasdduusumodificacion);

            int iCasddufecmodificacion = dr.GetOrdinal(this.Casddufecmodificacion);
            if (!dr.IsDBNull(iCasddufecmodificacion)) entity.Casddufecmodificacion = dr.GetDateTime(iCasddufecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Casdducodi = "CASDDUCODI";
        public string Equicodi = "EQUICODI";
        public string Casdduunidad = "CASDDUUNIDAD";
        public string Casddufecvigencia = "CASDDUFECVIGENCIA";
        public string Casdduusucreacion = "CASDDUUSUCREACION";
        public string Casddufeccreacion = "CASDDUFECCREACION";
        public string Casdduusumodificacion = "CASDDUUSUMODIFICACION";
        public string Casddufecmodificacion = "CASDDUFECMODIFICACION";

        public string Equinomb = "EQUINOMB";

        #endregion


        public string SqlListEquisddpuni
        {
            get { return GetSqlXml("ListCaiEquisddpuni"); }
        }

        public string GetByIdCaiEquisddpuni
        {
            get { return GetSqlXml("GetByIdCaiEquisddpuni"); }
        }

        public string GetByNombreEquipoSddp
        {
            get { return GetSqlXml("GetByNombreEquipoSddp"); }
        }

        public string GetByCriteriaCaiEquiunidbarrsNoIns
        {
            get { return GetSqlXml("GetByCriteriaCaiEquiunidbarrsNoIns"); }
        }

        public string SqlUnidad
        {
            get { return GetSqlXml("Unidad"); }
        }
    }
}
