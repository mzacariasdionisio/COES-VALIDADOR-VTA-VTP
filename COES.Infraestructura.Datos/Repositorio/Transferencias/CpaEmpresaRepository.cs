using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Infraestructura.Datos.Helper.Transferencias;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase de acceso a datos de la tabla CPA_EMPRESA
    /// </summary>
    public class CpaEmpresaRepository : RepositoryBase, ICpaEmpresaRepository
    {
        public CpaEmpresaRepository(string strConn)
            : base(strConn)
        {
        }

        CpaEmpresaHelper helper = new CpaEmpresaHelper();

        public int Save(CpaEmpresaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cpaempcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cparcodi, DbType.Int32, entity.Cparcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Cpaemptipo, DbType.String, entity.Cpaemptipo);
            dbProvider.AddInParameter(command, helper.Cpaempestado, DbType.String, entity.Cpaempestado);
            dbProvider.AddInParameter(command, helper.Cpaempusucreacion, DbType.String, entity.Cpaempusucreacion);
            dbProvider.AddInParameter(command, helper.Cpaempfeccreacion, DbType.DateTime, entity.Cpaempfeccreacion);
            dbProvider.AddInParameter(command, helper.Cpaempusumodificacion, DbType.String, entity.Cpaempusumodificacion);
            dbProvider.AddInParameter(command, helper.Cpaempfecmodificacion, DbType.DateTime, entity.Cpaempfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CpaEmpresaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cpaempcodi, DbType.Int32, entity.Cpaempcodi);
            dbProvider.AddInParameter(command, helper.Cparcodi, DbType.Int32, entity.Cparcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Cpaemptipo, DbType.String, entity.Cpaemptipo);
            dbProvider.AddInParameter(command, helper.Cpaempestado, DbType.String, entity.Cpaempestado);
            dbProvider.AddInParameter(command, helper.Cpaempusucreacion, DbType.String, entity.Cpaempusucreacion);
            dbProvider.AddInParameter(command, helper.Cpaempfeccreacion, DbType.DateTime, entity.Cpaempfeccreacion);
            dbProvider.AddInParameter(command, helper.Cpaempusumodificacion, DbType.String, entity.Cpaempusumodificacion);
            dbProvider.AddInParameter(command, helper.Cpaempfecmodificacion, DbType.DateTime, entity.Cpaempfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cpaEmpcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cpaempcodi, DbType.Int32, cpaEmpcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CpaEmpresaDTO GetById(int cpaEmpcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cpaempcodi, DbType.Int32, cpaEmpcodi);
            CpaEmpresaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CpaEmpresaDTO> List()
        {
            List<CpaEmpresaDTO> entities = new List<CpaEmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entities.Add(helper.Create(dr));
                }
            }

            return entities;
        }

        public List<CpaEmpresaDTO> ListaEmpresasIntegrantes(int revision, string estado, string tipo)
        {
            CpaEmpresaDTO entity = new CpaEmpresaDTO();
            List<CpaEmpresaDTO> entitys = new List<CpaEmpresaDTO>();
            string query = string.Format(helper.SqlListaEmpresasIntegrantes, revision, estado, tipo);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new CpaEmpresaDTO();

                    int iCpaempcodi = dr.GetOrdinal(helper.Cpaempcodi);
                    if (!dr.IsDBNull(iCpaempcodi)) entity.Cpaempcodi = dr.GetInt32(iCpaempcodi);

                    int iCparcodi = dr.GetOrdinal(helper.Cparcodi);
                    if (!dr.IsDBNull(iCparcodi)) entity.Cparcodi = dr.GetInt32(iCparcodi);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);

                    int iEmprnomb= dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iCpaemptipo = dr.GetOrdinal(helper.Cpaemptipo);
                    if (!dr.IsDBNull(iCpaemptipo)) entity.Cpaemptipo = dr.GetString(iCpaemptipo);

                    int iCpaempestado = dr.GetOrdinal(helper.Cpaempestado);
                    if (!dr.IsDBNull(iCpaempestado)) entity.Cpaempestado = dr.GetString(iCpaempestado);

                    int iCpaempusucreacion = dr.GetOrdinal(helper.Cpaempusucreacion);
                    if (!dr.IsDBNull(iCpaempusucreacion)) entity.Cpaempusucreacion = dr.GetString(iCpaempusucreacion);

                    int iCpaempfeccreacion = dr.GetOrdinal(helper.Cpaempfeccreacion);
                    if (!dr.IsDBNull(iCpaempfeccreacion)) entity.Cpaempfeccreacion = dr.GetDateTime(iCpaempfeccreacion);

                    int iCpaempusumodificacion = dr.GetOrdinal(helper.Cpaempusumodificacion);
                    if (!dr.IsDBNull(iCpaempusumodificacion)) entity.Cpaempusumodificacion = dr.GetString(iCpaempusumodificacion);

                    int iCpaempfecmodificacion = dr.GetOrdinal(helper.Cpaempfecmodificacion);
                    if (!dr.IsDBNull(iCpaempfecmodificacion)) entity.Cpaempfecmodificacion = dr.GetDateTime(iCpaempfecmodificacion);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void UpdateEstadoEmpresaIntegrante(CpaEmpresaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateEstadoEmpresaGeneradora);

            dbProvider.AddInParameter(command, helper.Cpaempestado, DbType.String, entity.Cpaempestado);
            dbProvider.AddInParameter(command, helper.Cpaempusumodificacion, DbType.String, entity.Cpaempusumodificacion);
            dbProvider.AddInParameter(command, helper.Cpaempfecmodificacion, DbType.DateTime, entity.Cpaempfecmodificacion);
            dbProvider.AddInParameter(command, helper.Cpaempcodi, DbType.Int32, entity.Cpaempcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void UpdateAuditoriaEmpresaIntegrante(CpaEmpresaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateAuditoriaEmpresaIntegrante);

            dbProvider.AddInParameter(command, helper.Cpaempusumodificacion, DbType.String, entity.Cpaempusumodificacion);
            dbProvider.AddInParameter(command, helper.Cpaempfecmodificacion, DbType.DateTime, entity.Cpaempfecmodificacion);
            dbProvider.AddInParameter(command, helper.Cpaempcodi, DbType.Int32, entity.Cpaempcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<CpaEmpresaDTO> FiltroEmpresasIntegrantes(int revision)
        {
            CpaEmpresaDTO entity = new CpaEmpresaDTO();
            List<CpaEmpresaDTO> entitys = new List<CpaEmpresaDTO>();
            string query = string.Format(helper.SqlFiltroEmpresasIntegrantes, revision);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new CpaEmpresaDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEmprnombconcatenado = dr.GetOrdinal(helper.Emprnombconcatenado);
                    if (!dr.IsDBNull(iEmprnombconcatenado)) entity.Emprnombconcatenado = dr.GetString(iEmprnombconcatenado);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CpaEmpresaDTO> ListaEmpresaPorRevisionTipo(int revision, string tipo, int empresa)
        {
            CpaEmpresaDTO entity = new CpaEmpresaDTO();
            List<CpaEmpresaDTO> entitys = new List<CpaEmpresaDTO>();
            string query = string.Format(helper.SqlListaEmpresaPorRevisionTipo, revision, tipo, empresa);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new CpaEmpresaDTO();

                    int iCpaempcodi = dr.GetOrdinal(helper.Cpaempcodi);
                    if (!dr.IsDBNull(iCpaempcodi)) entity.Cpaempcodi = dr.GetInt32(iCpaempcodi);

                    int iCparcodi = dr.GetOrdinal(helper.Cparcodi);
                    if (!dr.IsDBNull(iCparcodi)) entity.Cparcodi = dr.GetInt32(iCparcodi);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iCpaemptipo = dr.GetOrdinal(helper.Cpaemptipo);
                    if (!dr.IsDBNull(iCpaemptipo)) entity.Cpaemptipo = dr.GetString(iCpaemptipo);

                    int iCpaempestado = dr.GetOrdinal(helper.Cpaempestado);
                    if (!dr.IsDBNull(iCpaempestado)) entity.Cpaempestado = dr.GetString(iCpaempestado);

                    int iCpaempusucreacion = dr.GetOrdinal(helper.Cpaempusucreacion);
                    if (!dr.IsDBNull(iCpaempusucreacion)) entity.Cpaempusucreacion = dr.GetString(iCpaempusucreacion);

                    int iCpaempfeccreacion = dr.GetOrdinal(helper.Cpaempfeccreacion);
                    if (!dr.IsDBNull(iCpaempfeccreacion)) entity.Cpaempfeccreacion = dr.GetDateTime(iCpaempfeccreacion);

                    int iCpaempusumodificacion = dr.GetOrdinal(helper.Cpaempusumodificacion);
                    if (!dr.IsDBNull(iCpaempusumodificacion)) entity.Cpaempusumodificacion = dr.GetString(iCpaempusumodificacion);

                    int iCpaempfecmodificacion = dr.GetOrdinal(helper.Cpaempfecmodificacion);
                    if (!dr.IsDBNull(iCpaempfecmodificacion)) entity.Cpaempfecmodificacion = dr.GetDateTime(iCpaempfecmodificacion);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        /* CU17: INICIO */
        public List<SiEmpresaDTO> ListEmpresasUnicasByRevisionByEstado(int cparcodi, string strcpaempestado)
        {
            SiEmpresaDTO entity;
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
            string query = string.Format(helper.SqlListEmpresasUnicasByRevisionByEstado, cparcodi, strcpaempestado);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new SiEmpresaDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iTipoemprcodi = dr.GetOrdinal(helper.Tipoemprcodi);
                    if (!dr.IsDBNull(iTipoemprcodi)) entity.Tipoemprcodi = dr.GetInt32(iTipoemprcodi);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        /* CU17: FIN */

    }
}
