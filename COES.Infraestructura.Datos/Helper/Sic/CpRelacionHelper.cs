using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using System.Data;
using COES.Dominio.DTO.Sic;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CP_GRUPORECURSO
    /// </summary>
    public class CpRelacionHelper : HelperBase
    {
        public CpRelacionHelper()
            : base(Consultas.CpRelacionSql)
        {
        }
        public CpRelacionDTO Create(IDataReader dr)
        {
            CpRelacionDTO entity = new CpRelacionDTO();



            int iRecurcodi1 = dr.GetOrdinal(this.Recurcodi1);
            if (!dr.IsDBNull(iRecurcodi1)) entity.Recurcodi1 = Convert.ToInt32(dr.GetValue(iRecurcodi1));

            int iRecurcodi2 = dr.GetOrdinal(this.Recurcodi2);
            if (!dr.IsDBNull(iRecurcodi2)) entity.Recurcodi2 = Convert.ToInt32(dr.GetValue(iRecurcodi2));

            int iCptrelcodi = dr.GetOrdinal(this.Cptrelcodi);
            if (!dr.IsDBNull(iCptrelcodi)) entity.Cptrelcodi = Convert.ToInt32(dr.GetValue(iCptrelcodi));

            int iTopcodi = dr.GetOrdinal(this.Topcodi);
            if (!dr.IsDBNull(iTopcodi)) entity.Topcodi = Convert.ToInt32(dr.GetValue(iTopcodi));

            int iCprelval1 = dr.GetOrdinal(this.Cprelval1);
            if (!dr.IsDBNull(iCprelval1)) entity.Cprelval1 = dr.GetDecimal(iCprelval1);

            int iCprelusucreacion = dr.GetOrdinal(this.Cprelusucreacion);
            if (!dr.IsDBNull(iCprelusucreacion)) entity.Cprelusucreacion = dr.GetString(iCprelusucreacion);

            int iCprelfeccreacion = dr.GetOrdinal(this.Cprelfeccreacion);
            if (!dr.IsDBNull(iCprelfeccreacion)) entity.Cprelfeccreacion = dr.GetDateTime(iCprelfeccreacion);

            int iCprelusumodificacion = dr.GetOrdinal(this.Cprelusumodificacion);
            if (!dr.IsDBNull(iCprelusumodificacion)) entity.Cprelusumodificacion = dr.GetString(iCprelusumodificacion);

            int iCprelfecmodificacion= dr.GetOrdinal(this.Cprelfecmodificacion);
            if (!dr.IsDBNull(iCprelfecmodificacion)) entity.Cprelfecmodificacion = dr.GetDateTime(iCprelfecmodificacion);

            int iCprelval2 = dr.GetOrdinal(this.Cprelval2);
            if (!dr.IsDBNull(iCprelval2)) entity.Cprelval2 = dr.GetDecimal(iCprelval2);

            return entity;
        }


        #region Mapeo de Campos

        public string Recurcodi1 = "RECURCODI1";
        public string Recurcodi2 = "RECURCODI2";
        public string Cprelcodi = "CPRELCODI";
        public string Cptrelcodi = "CPTRELCODI";
        public string Topcodi = "TOPCODI";
        public string Cprelval1 = "CPRELVAL1";
        public string Cprelval2 = "CPRELVAL2";
        public string Cprelusucreacion = "CPRELUSUCREACION";
        public string Cprelfeccreacion = "CPRELFECCREACION";
        public string Cprelusumodificacion = "CPRELUSUMODIFICACION";
        public string Cprelfecmodificacion = "CPRELFECMODIFICACION";
        public string Recurnombre = "RECURNOMBRE";
        public string Catcodi = "CATCODI";
        public string Catcodi1 = "CATCODI1";
        public string Recurconsideragams = "RECURCONSIDERAGAMS";

        #endregion

        public string SqlDeleteAll
        {
            get { return base.GetSqlXml("DeleteAll"); }
        }

        public string SqlDeleteAllTipo
        {
            get { return base.GetSqlXml("DeleteAllTipo"); }
        }

        public string SqlGetByCriteriaNombre
        {
            get { return base.GetSqlXml("GetByCriteriaNombre"); }
        }


        public string SqlObtenerDependencias
        {
            get { return base.GetSqlXml("ObtenerDependencias"); }
        }

        public string SqlDeleteEscenario
        {
            get { return base.GetSqlXml("DeleteEscenario"); }
        }

        public string SqlCrearCopia
        {
            get { return base.GetSqlXml("CrearCopia"); }
        }

    }
}
