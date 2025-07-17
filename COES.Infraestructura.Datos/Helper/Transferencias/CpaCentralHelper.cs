using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CPA_CENTRAL
    /// </summary>
    public class CpaCentralHelper : HelperBase
    {
        public CpaCentralHelper() : base(Consultas.CpaCentralSql)
        {
        }

        public CpaCentralDTO Create(IDataReader dr)
        {
            CpaCentralDTO entity = new CpaCentralDTO();

            int iCpacntcodi = dr.GetOrdinal(Cpacntcodi);
            if (!dr.IsDBNull(iCpacntcodi)) entity.Cpacntcodi = dr.GetInt32(iCpacntcodi);

            int iCpaempcodi = dr.GetOrdinal(Cpaempcodi);
            if (!dr.IsDBNull(iCpaempcodi)) entity.Cpaempcodi = dr.GetInt32(iCpaempcodi);

            int iCparcodi = dr.GetOrdinal(Cparcodi);
            if (!dr.IsDBNull(iCparcodi)) entity.Cparcodi = dr.GetInt32(iCparcodi);

            int iEquicodi = dr.GetOrdinal(Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = dr.GetInt32(iEquicodi);

            int iBarrcodi = dr.GetOrdinal(Barrcodi);
            if (!dr.IsDBNull(iBarrcodi)) entity.Barrcodi = dr.GetInt32(iBarrcodi);

            int iPtomedicodi = dr.GetOrdinal(Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = dr.GetInt32(iPtomedicodi);

            int iCpacntestado = dr.GetOrdinal(Cpacntestado);
            if (!dr.IsDBNull(iCpacntestado)) entity.Cpacntestado = dr.GetString(iCpacntestado);

            int iCpacnttipo = dr.GetOrdinal(Cpacnttipo);
            if (!dr.IsDBNull(iCpacnttipo)) entity.Cpacnttipo = dr.GetString(iCpacnttipo);

            int iCpacntfecejecinicio = dr.GetOrdinal(Cpacntfecejecinicio);
            if (!dr.IsDBNull(iCpacntfecejecinicio)) entity.Cpacntfecejecinicio = dr.GetDateTime(iCpacntfecejecinicio);

            int iCpacntfecejecfin = dr.GetOrdinal(Cpacntfecejecfin);
            if (!dr.IsDBNull(iCpacntfecejecfin)) entity.Cpacntfecejecfin = dr.GetDateTime(iCpacntfecejecfin);

            int iCpacntfecproginicio = dr.GetOrdinal(Cpacntfecproginicio);
            if (!dr.IsDBNull(iCpacntfecproginicio)) entity.Cpacntfecproginicio = dr.GetDateTime(iCpacntfecproginicio);

            int iCpacntfecprogfin = dr.GetOrdinal(Cpacntfecprogfin);
            if (!dr.IsDBNull(iCpacntfecprogfin)) entity.Cpacntfecprogfin = dr.GetDateTime(iCpacntfecprogfin);

            int iCpacntusucreacion = dr.GetOrdinal(Cpacntusucreacion);
            if (!dr.IsDBNull(iCpacntusucreacion)) entity.Cpacntusucreacion = dr.GetString(iCpacntusucreacion);

            int iCpacntfeccreacion = dr.GetOrdinal(Cpacntfeccreacion);
            if (!dr.IsDBNull(iCpacntfeccreacion)) entity.Cpacntfeccreacion = dr.GetDateTime(iCpacntfeccreacion);

            int iCpacntusumodificacion = dr.GetOrdinal(Cpacntusumodificacion);
            if (!dr.IsDBNull(iCpacntusumodificacion)) entity.Cpacntusumodificacion = dr.GetString(iCpacntusumodificacion);

            int iCpacntfecmodificacion = dr.GetOrdinal(Cpacntfecmodificacion);
            if (!dr.IsDBNull(iCpacntfecmodificacion)) entity.Cpacntfecmodificacion = dr.GetDateTime(iCpacntfecmodificacion);

            return entity;
        }

        #region Mapeo de Campos
        public string Cpacntcodi = "CPACNTCODI";
        public string Cpaempcodi = "CPAEMPCODI";
        public string Cparcodi = "CPARCODI";
        public string Equicodi = "EQUICODI";
        public string Barrcodi = "BARRCODI";
        public string Ptomedicodi = "PTOMEDICODI";
        public string Cpacntestado = "CPACNTESTADO";
        public string Cpacnttipo = "CPACNTTIPO";
        public string Cpacntfecejecinicio = "CPACNTFECEJECINICIO";
        public string Cpacntfecejecfin = "CPACNTFECEJECFIN";
        public string Cpacntfecproginicio = "CPACNTFECPROGINICIO";
        public string Cpacntfecprogfin = "CPACNTFECPROGFIN";
        public string Cpacntusucreacion = "CPACNTUSUCREACION";
        public string Cpacntfeccreacion = "CPACNTFECCREACION";
        public string Cpacntusumodificacion = "CPACNTUSUMODIFICACION";
        public string Cpacntfecmodificacion = "CPACNTFECMODIFICACION";
        //CU03
        public string Equinomb = "EQUINOMB";
        public string Equinombconcatenado = "EQUINOMBCONCATENADO";
        //CU04
        public string Emprcodi = "EMPRCODI";
        public string Emprnomb = "EMPRNOMB";
        public string Barrnombre = "BARRNOMBRE";
        public string Barrbarratransferencia = "BARRBARRATRANSFERENCIA";
        public string Equifechiniopcom = "EQUIFECHINIOPCOM";
        public string Equifechfinopcom = "EQUIFECHFINOPCOM";
        public string Ptomedidesc = "PTOMEDIDESC";
        public string Centralespmpo = "CENTRALESPMPO";

        //Additionals
        public string Cpaemptipo = "CPAEMPTIPO";
        public string Cpaempestado = "CPAEMPESTADO";
        //Agregado para registrar varias veces una central
        public string Cpacntcorrelativo = "CPACNTCORRELATIVO";
        #endregion

        #region CU03
        public string SqlListaCentralesIntegrantes
        {
            get { return base.GetSqlXml("ListaCentralesIntegrantes"); }
        }

        public string SqlUpdateEstadoCentralGeneradora
        {
            get { return base.GetSqlXml("UpdateEstadoCentralGeneradora"); }
        }

        public string SqlFiltroCentralesIntegrantes
        {
            get { return base.GetSqlXml("FiltroCentralesIntegrantes"); }
        }

        public string SqlListaCentralesEmpresasParticipantes
        {
            get { return base.GetSqlXml("ListaCentralesEmpresasParticipantes"); }
        }
        #endregion

        public string SqlListByRevision
        {
            get { return base.GetSqlXml("ListByRevision"); }
        }

        //CU04
        public string SqlUpdateCentralPMPO
        {
            get { return base.GetSqlXml("UpdateCentralPMPO"); }
        }
        public string SqlGetByRevisionByTipoEmpresaByEstadoEmpresaByEstadoCentral
        {
            get { return base.GetSqlXml("GetByRevisionByTipoEmpresaByEstadoEmpresaByEstadoCentral"); }
        }
        public string SqlGetMaxIdCentral
        {
            get { return base.GetSqlXml("GetMaxIdCentral"); }
        }
        public string SqlListaCentralesPorEmpresaRevison
        {
            get { return base.GetSqlXml("ListaCentralesPorEmpresaRevision"); }
        }
        public string SqlListaCentralesPorRevison
        {
            get { return base.GetSqlXml("ListaCentralesPorRevision"); }
        }
        public string SqlListaCentralesByEmpresa
        {
            get { return base.GetSqlXml("ListaCentralesByEmpresa"); }
        }
    }
}
