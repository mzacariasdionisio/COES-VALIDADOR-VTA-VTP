using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CPA_EMPRESA
    /// </summary>
    public class CpaEmpresaHelper : HelperBase
    {
        public CpaEmpresaHelper() : base(Consultas.CpaEmpresaSql)
        {
        }

        public CpaEmpresaDTO Create(IDataReader dr)
        {
            CpaEmpresaDTO entity = new CpaEmpresaDTO();

            int iCpaempcodi = dr.GetOrdinal(Cpaempcodi);
            if (!dr.IsDBNull(iCpaempcodi)) entity.Cpaempcodi = dr.GetInt32(iCpaempcodi);

            int iCparcodi = dr.GetOrdinal(Cparcodi);
            if (!dr.IsDBNull(iCparcodi)) entity.Cparcodi = dr.GetInt32(iCparcodi);

            int iEmprcodi = dr.GetOrdinal(Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);

            int iCpaemptipo = dr.GetOrdinal(Cpaemptipo);
            if (!dr.IsDBNull(iCpaemptipo)) entity.Cpaemptipo = dr.GetString(iCpaemptipo);

            int iCpaempestado = dr.GetOrdinal(Cpaempestado);
            if (!dr.IsDBNull(iCpaempestado)) entity.Cpaempestado = dr.GetString(iCpaempestado);

            int iCpaempusucreacion = dr.GetOrdinal(Cpaempusucreacion);
            if (!dr.IsDBNull(iCpaempusucreacion)) entity.Cpaempusucreacion = dr.GetString(iCpaempusucreacion);

            int iCpaempfeccreacion = dr.GetOrdinal(Cpaempfeccreacion);
            if (!dr.IsDBNull(iCpaempfeccreacion)) entity.Cpaempfeccreacion = dr.GetDateTime(iCpaempfeccreacion);

            int iCpaempusumodificacion = dr.GetOrdinal(Cpaempusumodificacion);
            if (!dr.IsDBNull(iCpaempusumodificacion)) entity.Cpaempusumodificacion = dr.GetString(iCpaempusumodificacion);

            int iCpaempfecmodificacion = dr.GetOrdinal(Cpaempfecmodificacion);
            if (!dr.IsDBNull(iCpaempfecmodificacion)) entity.Cpaempfecmodificacion = dr.GetDateTime(iCpaempfecmodificacion);

            return entity;
        }

        #region Mapeo de Campos
        public string Cpaempcodi = "CPAEMPCODI";
        public string Cparcodi = "CPARCODI";
        public string Emprcodi = "EMPRCODI";
        public string Cpaemptipo = "CPAEMPTIPO";
        public string Cpaempestado = "CPAEMPESTADO";
        public string Cpaempusucreacion = "CPAEMPUSUCREACION";
        public string Cpaempfeccreacion = "CPAEMPFECCREACION";
        public string Cpaempusumodificacion = "CPAEMPUSUMODIFICACION";
        public string Cpaempfecmodificacion = "CPAEMPFECMODIFICACION";
        //Agregado para pasar el nombre de la empresa
        public string Emprnomb = "EMPRNOMB";
        public string Emprnombconcatenado = "EMPRNOMBCONCATENADO";

        /* CU17: INICIO */
        public string Tipoemprcodi = "TIPOEMPRCODI";
        /* CU17: FIN */
        #endregion

        #region CU03
        public string SqlListaEmpresasIntegrantes
        {
            get { return base.GetSqlXml("ListaEmpresasIntegrantes"); }
        }
        public string SqlUpdateEstadoEmpresaGeneradora
        {
            get { return base.GetSqlXml("UpdateEstadoEmpresaGeneradora"); }
        }
        public string SqlUpdateAuditoriaEmpresaIntegrante
        {
            get { return base.GetSqlXml("UpdateAuditoriaEmpresaIntegrante"); }
        }
        public string SqlFiltroEmpresasIntegrantes
        {
            get { return base.GetSqlXml("FiltroEmpresasIntegrantes"); }
        }
        public string SqlListaEmpresaPorRevisionTipo
        {
            get { return base.GetSqlXml("ListaEmpresaPorRevisionTipo"); }
        }
        #endregion

        #region CU17
        public string SqlListEmpresasUnicasByRevisionByEstado
        {
            get { return base.GetSqlXml("ListEmpresasUnicasByRevisionByEstado"); }
        }
        #endregion
    }
}
