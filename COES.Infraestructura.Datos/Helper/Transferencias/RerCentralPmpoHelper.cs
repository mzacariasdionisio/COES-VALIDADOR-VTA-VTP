using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RER_CENTRAL_PMPO
    /// </summary>
    public class RerCentralPmpoHelper : HelperBase
    {
        public RerCentralPmpoHelper() : base(Consultas.RerCentralPmpoSql)
        {
        }

        public RerCentralPmpoDTO Create(IDataReader dr)
        {
            RerCentralPmpoDTO entity = new RerCentralPmpoDTO();

            int iRercpmcodi = dr.GetOrdinal(this.Rercpmcodi);
            if (!dr.IsDBNull(iRercpmcodi)) entity.Rercpmcodi = Convert.ToInt32(dr.GetValue(iRercpmcodi));

            int iRercencodi = dr.GetOrdinal(this.Rercencodi);
            if (!dr.IsDBNull(iRercencodi)) entity.Rercencodi = Convert.ToInt32(dr.GetValue(iRercencodi));

            int iPtomedicodi = dr.GetOrdinal(this.Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

            int iRercpmusucreacion = dr.GetOrdinal(this.Rercpmusucreacion);
            if (!dr.IsDBNull(iRercpmusucreacion)) entity.Rercpmusucreacion = dr.GetString(iRercpmusucreacion);

            int iRercpmfeccreacion = dr.GetOrdinal(this.Rercpmfeccreacion);
            if (!dr.IsDBNull(iRercpmfeccreacion)) entity.Rercpmfeccreacion = dr.GetDateTime(iRercpmfeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Rercpmcodi = "RERCPMCODI";
        public string Rercencodi = "RERCENCODI";
        public string Ptomedicodi = "PTOMEDICODI";
        public string Rercpmusucreacion = "RERCPMUSUCREACION";
        public string Rercpmfeccreacion = "RERCPMFECCREACION";

        public string Ptomedidesc = "PTOMEDIDESC";
        public string Fenergcodi = "FENERGCODI";
        public string Tgenercodi = "TGENERCODI";
        public string Grupotipocogen = "GRUPOTIPOCOGEN";
        #endregion

        public string SqlListByRercencodi
        {
            get { return base.GetSqlXml("ListByRercencodi"); }
        }
        public string SqlDeleteAllByRercencodi
        {
            get { return base.GetSqlXml("DeleteAllByRercencodi"); }
        }

    }
}