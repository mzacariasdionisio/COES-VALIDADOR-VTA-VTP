using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CM_GENERACION
    /// </summary>
    public class CmGeneracionHelper : HelperBase
    {
        public CmGeneracionHelper() : base(Consultas.CmGeneracionSql)
        {
        }

        public CmGeneracionDTO Create(IDataReader dr)
        {
            CmGeneracionDTO entity = new CmGeneracionDTO();

            int iGenecodi = dr.GetOrdinal(this.Genecodi);
            if (!dr.IsDBNull(iGenecodi)) entity.Genecodi = Convert.ToInt32(dr.GetValue(iGenecodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iGenefecha = dr.GetOrdinal(this.Genefecha);
            if (!dr.IsDBNull(iGenefecha)) entity.Genefecha = dr.GetDateTime(iGenefecha);

            int iGeneintervalo = dr.GetOrdinal(this.Geneintervalo);
            if (!dr.IsDBNull(iGeneintervalo)) entity.Geneintervalo = Convert.ToInt32(dr.GetValue(iGeneintervalo));

            int iGenevalor = dr.GetOrdinal(this.Genevalor);
            if (!dr.IsDBNull(iGenevalor)) entity.Genevalor = dr.GetDecimal(iGenevalor);

            int iGenesucreacion = dr.GetOrdinal(this.Genesucreacion);
            if (!dr.IsDBNull(iGenesucreacion)) entity.Genesucreacion = dr.GetString(iGenesucreacion);

            int iGenefeccreacion = dr.GetOrdinal(this.Genefeccreacion);
            if (!dr.IsDBNull(iGenefeccreacion)) entity.Genefeccreacion = dr.GetDateTime(iGenefeccreacion);

            int iGeneusumodificacion = dr.GetOrdinal(this.Geneusumodificacion);
            if (!dr.IsDBNull(iGeneusumodificacion)) entity.Geneusumodificacion = dr.GetString(iGeneusumodificacion);

            int iGenefecmodificacion = dr.GetOrdinal(this.Genefecmodificacion);
            if (!dr.IsDBNull(iGenefecmodificacion)) entity.Genefecmodificacion = dr.GetDateTime(iGenefecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Genecodi = "GENECODI";
        public string Equicodi = "EQUICODI";
        public string Genefecha = "GENEFECHA";
        public string Geneintervalo = "GENEINTERVALO";
        public string Genevalor = "GENEVALOR";
        public string Genesucreacion = "GENESUCREACION";
        public string Genefeccreacion = "GENEFECCREACION";
        public string Geneusumodificacion = "GENEUSUMODIFICACION";
        public string Genefecmodificacion = "GENEFECMODIFICACION";

        #endregion

        public string SqlDeleteByCriteria
        {
            get { return GetSqlXml("DeleteByCriteria"); }
        }

        public string SqlListByEmpresa
        {
            get { return GetSqlXml("ListByEmpresa"); }
        }

        public string SqlProducionEnergiaByDate
        {
            get { return GetSqlXml("ProducionEnergiaByDate"); }
        }
    }
}
