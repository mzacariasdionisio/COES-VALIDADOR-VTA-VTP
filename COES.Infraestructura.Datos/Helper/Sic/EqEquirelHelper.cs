using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EQ_EQUIREL
    /// </summary>
    public class EqEquirelHelper : HelperBase
    {
        public EqEquirelHelper()
            : base(Consultas.EqEquirelSql)
        {
        }

        public EqEquirelDTO Create(IDataReader dr)
        {
            EqEquirelDTO entity = new EqEquirelDTO();

            int iEquicodi1 = dr.GetOrdinal(this.Equicodi1);
            if (!dr.IsDBNull(iEquicodi1)) entity.Equicodi1 = Convert.ToInt32(dr.GetValue(iEquicodi1));

            int iTiporelcodi = dr.GetOrdinal(this.Tiporelcodi);
            if (!dr.IsDBNull(iTiporelcodi)) entity.Tiporelcodi = Convert.ToInt32(dr.GetValue(iTiporelcodi));

            int iEquicodi2 = dr.GetOrdinal(this.Equicodi2);
            if (!dr.IsDBNull(iEquicodi2)) entity.Equicodi2 = Convert.ToInt32(dr.GetValue(iEquicodi2));

            int iEquirelagrup = dr.GetOrdinal(this.Equirelagrup);
            if (!dr.IsDBNull(iEquirelagrup)) entity.Equirelagrup = Convert.ToInt32(dr.GetValue(iEquirelagrup));

            int iEquirelfecmodificacion = dr.GetOrdinal(this.Equirelfecmodificacion);
            if (!dr.IsDBNull(iEquirelfecmodificacion)) entity.Equirelfecmodificacion = dr.GetDateTime(iEquirelfecmodificacion);

            int iEquirelusumodificacion = dr.GetOrdinal(this.Equirelusumodificacion);
            if (!dr.IsDBNull(iEquirelusumodificacion)) entity.Equirelusumodificacion = dr.GetString(iEquirelusumodificacion);

            int iEquirelexcep = dr.GetOrdinal(this.Equirelexcep);
            if (!dr.IsDBNull(iEquirelexcep)) entity.Equirelexcep = Convert.ToInt32(dr.GetValue(iEquirelexcep));

            return entity;
        }

        #region Mapeo de Campos
        public string Equicodi1 = "EQUICODI1";
        public string Tiporelcodi = "TIPORELCODI";
        public string Equicodi2 = "EQUICODI2";
        public string Equirelagrup = "EQUIRELAGRUP";
        public string Equirelfecmodificacion = "EQUIRELFECMODIFICACION";
        public string Equirelusumodificacion = "EQUIRELUSUMODIFICACION";
        public string Equirelexcep = "EQUIRELEXCEP";
        public string Emprnomb = "EMPRNOMB";
        public string Equinomb = "EQUINOMB";
        public string Empresatopologia = "EMPRESATOPOLOGIA";
        public string Equipotopologia = "EQUIPOTOPOLOGIA";
        public string Emprcodi = "EMPRCODI";
        public string Famnomb = "FAMNOMB";

        public string Equinomb1 = "EQUINOMB1";
        public string Equinomb2 = "EQUINOMB2";
        public string Famcodi1 = "FAMCODI1";
        public string Famcodi2 = "FAMCODI2";
        public string Emprnomb1 = "EMPRNOMB1";
        public string Emprnomb2 = "EMPRNOMB2";
        public string Famnomb1 = "FAMNOMB1";
        public string Famnomb2 = "FAMNOMB2";
        #endregion

        public string SqlGetByCriteriaTopologia
        {
            get { return base.GetSqlXml("GetByCriteriaTopologia"); }
        }

        #region INTERVENCIONES
        // Metodos agregados para Procedimiento Maniobra
        public string SqlListarRelacionesByIdsEquicodi
        {
            get { return base.GetSqlXml("ListarRelacionesByIdsEquicodi"); }
        }

        public string SqlListarRelacionesBarraByIdsEquicodi
        {
            get { return base.GetSqlXml("ListarRelacionesBarraByIdsEquicodi"); }
        }
        #endregion
    }
}
