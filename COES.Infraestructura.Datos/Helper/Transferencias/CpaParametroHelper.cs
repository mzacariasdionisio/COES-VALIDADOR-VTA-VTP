using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CPA_PARAMETRO
    /// </summary>
    public class CpaParametroHelper : HelperBase
    {
        public CpaParametroHelper() : base(Consultas.CpaParametroSql)
        {
        }

        public CpaParametroDTO Create(IDataReader dr)
        {
            CpaParametroDTO entity = new CpaParametroDTO();

            int iCpaprmcodi = dr.GetOrdinal(Cpaprmcodi);
            if (!dr.IsDBNull(iCpaprmcodi)) entity.Cpaprmcodi = dr.GetInt32(iCpaprmcodi);

            int iCparcodi = dr.GetOrdinal(Cparcodi);
            if (!dr.IsDBNull(iCparcodi)) entity.Cparcodi = dr.GetInt32(iCparcodi);

            int iCpaprmanio = dr.GetOrdinal(Cpaprmanio);
            if (!dr.IsDBNull(iCpaprmanio)) entity.Cpaprmanio = dr.GetInt32(iCpaprmanio);

            int iCpaprmmes = dr.GetOrdinal(Cpaprmmes);
            if (!dr.IsDBNull(iCpaprmmes)) entity.Cpaprmmes = dr.GetInt32(iCpaprmmes);

            int iCpaprmtipomd = dr.GetOrdinal(Cpaprmtipomd);
            if (!dr.IsDBNull(iCpaprmtipomd)) entity.Cpaprmtipomd = dr.GetString(iCpaprmtipomd);

            int iCpaprmfechamd = dr.GetOrdinal(Cpaprmfechamd);
            if (!dr.IsDBNull(iCpaprmfechamd)) entity.Cpaprmfechamd = dr.GetDateTime(iCpaprmfechamd);

            int iCpaprmcambio = dr.GetOrdinal(Cpaprmcambio);
            if (!dr.IsDBNull(iCpaprmcambio)) entity.Cpaprmcambio = dr.GetDecimal(iCpaprmcambio);

            int iCpaprmprecio = dr.GetOrdinal(Cpaprmprecio);
            if (!dr.IsDBNull(iCpaprmprecio)) entity.Cpaprmprecio = dr.GetDecimal(iCpaprmprecio);

            int iCpaprmestado = dr.GetOrdinal(Cpaprmestado);
            if (!dr.IsDBNull(iCpaprmestado)) entity.Cpaprmestado = dr.GetString(iCpaprmestado);

            int iCpaprmcorrelativo = dr.GetOrdinal(Cpaprmcorrelativo);
            if (!dr.IsDBNull(iCpaprmcorrelativo)) entity.Cpaprmcorrelativo = dr.GetInt32(iCpaprmcorrelativo);

            int iCpaprmusucreacion = dr.GetOrdinal(Cpaprmusucreacion);
            if (!dr.IsDBNull(iCpaprmusucreacion)) entity.Cpaprmusucreacion = dr.GetString(iCpaprmusucreacion);

            int iCpaprmfeccreacion = dr.GetOrdinal(Cpaprmfeccreacion);
            if (!dr.IsDBNull(iCpaprmfeccreacion)) entity.Cpaprmfeccreacion = dr.GetDateTime(iCpaprmfeccreacion);

            int iCpaprmusumodificacion = dr.GetOrdinal(Cpaprmusumodificacion);
            if (!dr.IsDBNull(iCpaprmusumodificacion)) entity.Cpaprmusumodificacion = dr.GetString(iCpaprmusumodificacion);

            int iCpaprmfecmodificacion = dr.GetOrdinal(Cpaprmfecmodificacion);
            if (!dr.IsDBNull(iCpaprmfecmodificacion)) entity.Cpaprmfecmodificacion = dr.GetDateTime(iCpaprmfecmodificacion);

            return entity;
        }

        #region Mapeo de Campos
        public string Cpaprmcodi = "CPAPRMCODI";
        public string Cparcodi = "CPARCODI";
        public string Cpaprmanio = "CPAPRMANIO";
        public string Cpaprmmes = "CPAPRMMES";
        public string Cpaprmtipomd = "CPAPRMTIPOMD";
        public string Cpaprmfechamd = "CPAPRMFECHAMD";
        public string Cpaprmcambio = "CPAPRMCAMBIO";
        public string Cpaprmprecio = "CPAPRMPRECIO";
        public string Cpaprmestado = "CPAPRMESTADO";
        public string Cpaprmcorrelativo = "CPAPRMCORRELATIVO";
        public string Cpaprmusucreacion = "CPAPRMUSUCREACION";
        public string Cpaprmfeccreacion = "CPAPRMFECCREACION";
        public string Cpaprmusumodificacion = "CPAPRMUSUMODIFICACION";
        public string Cpaprmfecmodificacion = "CPAPRMFECMODIFICACION";
        //CU05
        public string Aniomes = "ANIOMES";
        #endregion

        public string SqlGetMaxIdParametro
        {
            get { return base.GetSqlXml("GetMaxIdParametro"); }
        }
        public string SqlListaParametrosRegistrados
        {
            get { return base.GetSqlXml("ListaParametrosRegistrados"); }
        }
        public string SqlUpdateCpaParametroTipoYCambio
        {
            get { return base.GetSqlXml("UpdateCpaParametroTipoYCambio"); }
        }
        public string SqlUpdateCpaParametroEstado
        {
            get { return base.GetSqlXml("UpdateCpaParametroEstado"); }
        }

        public string SqlGetByRevisionByEstado
        {
            get { return base.GetSqlXml("GetByRevisionByEstado"); }
        }

        public string SqlListaParametrosByRevisionAnioMesEstado
        {
            get { return base.GetSqlXml("ListaParametrosByRevisionAnioMesEstado"); }
        }
        public string SqlGetByRevisionMes
        {
            get { return base.GetSqlXml("GetByRevisionMes"); }
        }
    }
}
