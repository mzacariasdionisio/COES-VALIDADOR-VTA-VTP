using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla FT_EXT_RELEMPETAPA
    /// </summary>
    public class FtExtRelempetapaHelper : HelperBase
    {
        public FtExtRelempetapaHelper() : base(Consultas.FtExtRelempetapaSql)
        {
        }

        public FtExtRelempetapaDTO Create(IDataReader dr)
        {
            FtExtRelempetapaDTO entity = new FtExtRelempetapaDTO();

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iFtetcodi = dr.GetOrdinal(this.Ftetcodi);
            if (!dr.IsDBNull(iFtetcodi)) entity.Ftetcodi = Convert.ToInt32(dr.GetValue(iFtetcodi));

            int iFetempcodi = dr.GetOrdinal(this.Fetempcodi);
            if (!dr.IsDBNull(iFetempcodi)) entity.Fetempcodi = Convert.ToInt32(dr.GetValue(iFetempcodi));

            int iFetempusucreacion = dr.GetOrdinal(this.Fetempusucreacion);
            if (!dr.IsDBNull(iFetempusucreacion)) entity.Fetempusucreacion = dr.GetString(iFetempusucreacion);

            int iFetempfeccreacion = dr.GetOrdinal(this.Fetempfeccreacion);
            if (!dr.IsDBNull(iFetempfeccreacion)) entity.Fetempfeccreacion = dr.GetDateTime(iFetempfeccreacion);

            int iFetempusumodificacion = dr.GetOrdinal(this.Fetempusumodificacion);
            if (!dr.IsDBNull(iFetempusumodificacion)) entity.Fetempusumodificacion = dr.GetString(iFetempusumodificacion);

            int iFetempfecmodificacion = dr.GetOrdinal(this.Fetempfecmodificacion);
            if (!dr.IsDBNull(iFetempfecmodificacion)) entity.Fetempfecmodificacion = dr.GetDateTime(iFetempfecmodificacion);

            int iFetempestado = dr.GetOrdinal(this.Fetempestado);
            if (!dr.IsDBNull(iFetempestado)) entity.Fetempestado = dr.GetString(iFetempestado);

            return entity;
        }

        #region Mapeo de Campos

        public string Emprcodi = "EMPRCODI";
        public string Ftetcodi = "FTETCODI";
        public string Fetempcodi = "FETEMPCODI";
        public string Fetempusucreacion = "FETEMPUSUCREACION";
        public string Fetempfeccreacion = "FETEMPFECCREACION";
        public string Fetempusumodificacion = "FETEMPUSUMODIFICACION";
        public string Fetempfecmodificacion = "FETEMPFECMODIFICACION";
        public string Fetempestado = "FETEMPESTADO";

        public string Emprnomb = "EMPRNOMB";
        public string Ftetnombre = "FTETNOMBRE"; 
        public string Equicodi = "EQUICODI";
        public string Grupocodi = "GRUPOCODI";
        public string Ftrpycodi = "FTPRYCODI";

        #endregion

        public string SqlGetByCriteriaProyAsigByFiltros
        {
            get { return GetSqlXml("GetByCriteriaProyAsigByFiltros"); }
        }

        public string SqlGetByEmpresaYEtapa
        {
            get { return GetSqlXml("GetByEmpresaYEtapa"); }
        }

        public string SqlGetEtapasPorEquicodi
        {
            get { return GetSqlXml("GetEtapasPorEquicodi"); }
        }

        public string SqlGetEtapasPorGrupocodi
        {
            get { return GetSqlXml("GetEtapasPorGrupocodi"); }
        }

        public string SqlGetByProyectos
        {
            get { return GetSqlXml("GetByProyectos"); }
        }
        

    }
}
