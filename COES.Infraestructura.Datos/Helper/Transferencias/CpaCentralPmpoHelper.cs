using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CPA_CENTRAL_PMPO
    /// </summary>
    public class CpaCentralPmpoHelper : HelperBase
    {
        public CpaCentralPmpoHelper() : base(Consultas.CpaCentralPmpoSql)
        {
        }

        public CpaCentralPmpoDTO Create(IDataReader dr)
        {
            CpaCentralPmpoDTO entity = new CpaCentralPmpoDTO();

            int iCpacnpcodi = dr.GetOrdinal(Cpacnpcodi);
            if (!dr.IsDBNull(iCpacnpcodi)) entity.Cpacnpcodi = dr.GetInt32(iCpacnpcodi);

            int iCpacntcodi = dr.GetOrdinal(Cpacntcodi);
            if (!dr.IsDBNull(iCpacntcodi)) entity.Cpacntcodi = dr.GetInt32(iCpacntcodi);

            int iPtomedicodi = dr.GetOrdinal(Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = dr.GetInt32(iPtomedicodi);

            int iCpacnpusumodificacion = dr.GetOrdinal(Cpacnpusumodificacion);
            if (!dr.IsDBNull(iCpacnpusumodificacion)) entity.Cpacnpusumodificacion = dr.GetString(iCpacnpusumodificacion);

            int iCpacnpfecmodificacion = dr.GetOrdinal(Cpacnpfecmodificacion);
            if (!dr.IsDBNull(iCpacnpfecmodificacion)) entity.Cpacnpfecmodificacion = dr.GetDateTime(iCpacnpfecmodificacion);

            return entity;
        }

        #region Mapeo de Campos
        public string Cpacnpcodi = "CPACNPCODI";
        public string Cpacntcodi = "CPACNTCODI";
        public string Cparcodi = "CPARCODI";
        public string Ptomedicodi = "PTOMEDICODI";
        public string Cpacnpusumodificacion = "CPACNPUSUMODIFICACION";
        public string Cpacnpfecmodificacion = "CPACNPFECMODIFICACION";
        //CU04
        public string Ptomedidesc = "PTOMEDIDESC";
        #endregion

        //CU04
        public string SqlListCpaCentralPmpobyCentral
        {
            get { return base.GetSqlXml("ListCpaCentralPmpobyCentral"); }
        }

        //Inicio: CU011
        public string SqlGetByCentral
        {
            get { return base.GetSqlXml("GetByCentral"); }
        }

        public string SqlGetByRevision
        {
            get { return base.GetSqlXml("GetByRevision"); }
        }
        //Fin: CU011

    }
}
