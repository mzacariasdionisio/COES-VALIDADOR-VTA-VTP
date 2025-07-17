using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Transferencias;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase de acceso a datos de la tabla CPA_PARAMETRO
    /// </summary>
    public class CpaParametroRepository : RepositoryBase, ICpaParametroRepository
    {
        public CpaParametroRepository(string strConn)
            : base(strConn)
        {
        }

        CpaParametroHelper helper = new CpaParametroHelper();

        public int Save(CpaParametroDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            string query = string.Format(helper.SqlGetMaxIdParametro, entity.Cparcodi, entity.Cpaprmanio, entity.Cpaprmmes, entity.Cpaprmtipomd);
            command = dbProvider.GetSqlStringCommand(query);
            result = dbProvider.ExecuteScalar(command);
            int correlativo = 1;
            if (result != null) correlativo = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cpaprmcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cparcodi, DbType.Int32, entity.Cparcodi);
            dbProvider.AddInParameter(command, helper.Cpaprmanio, DbType.Int32, entity.Cpaprmanio);
            dbProvider.AddInParameter(command, helper.Cpaprmmes, DbType.Int32, entity.Cpaprmmes);
            dbProvider.AddInParameter(command, helper.Cpaprmtipomd, DbType.String, entity.Cpaprmtipomd);
            dbProvider.AddInParameter(command, helper.Cpaprmfechamd, DbType.DateTime, entity.Cpaprmfechamd);
            dbProvider.AddInParameter(command, helper.Cpaprmcambio, DbType.Decimal, entity.Cpaprmcambio);
            dbProvider.AddInParameter(command, helper.Cpaprmprecio, DbType.Decimal, entity.Cpaprmprecio);
            dbProvider.AddInParameter(command, helper.Cpaprmestado, DbType.String, entity.Cpaprmestado);
            dbProvider.AddInParameter(command, helper.Cpaprmcorrelativo, DbType.Int32, correlativo);
            dbProvider.AddInParameter(command, helper.Cpaprmusucreacion, DbType.String, entity.Cpaprmusucreacion);
            dbProvider.AddInParameter(command, helper.Cpaprmfeccreacion, DbType.DateTime, entity.Cpaprmfeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CpaParametroDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cparcodi, DbType.Int32, entity.Cparcodi);
            dbProvider.AddInParameter(command, helper.Cpaprmanio, DbType.Int32, entity.Cpaprmanio);
            dbProvider.AddInParameter(command, helper.Cpaprmmes, DbType.Int32, entity.Cpaprmmes);
            dbProvider.AddInParameter(command, helper.Cpaprmtipomd, DbType.String, entity.Cpaprmtipomd);
            dbProvider.AddInParameter(command, helper.Cpaprmfechamd, DbType.DateTime, entity.Cpaprmfechamd);
            dbProvider.AddInParameter(command, helper.Cpaprmcambio, DbType.Decimal, entity.Cpaprmcambio);
            dbProvider.AddInParameter(command, helper.Cpaprmprecio, DbType.Decimal, entity.Cpaprmprecio);
            dbProvider.AddInParameter(command, helper.Cpaprmestado, DbType.String, entity.Cpaprmestado);
            dbProvider.AddInParameter(command, helper.Cpaprmcorrelativo, DbType.Int32, entity.Cpaprmcorrelativo);
            dbProvider.AddInParameter(command, helper.Cpaprmusucreacion, DbType.String, entity.Cpaprmusucreacion);
            dbProvider.AddInParameter(command, helper.Cpaprmfeccreacion, DbType.DateTime, entity.Cpaprmfeccreacion);
            dbProvider.AddInParameter(command, helper.Cpaprmusumodificacion, DbType.String, entity.Cpaprmusumodificacion);
            dbProvider.AddInParameter(command, helper.Cpaprmfecmodificacion, DbType.DateTime, entity.Cpaprmfecmodificacion);
            dbProvider.AddInParameter(command, helper.Cpaprmcodi, DbType.Int32, entity.Cpaprmcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void UpdateCpaParametroTipoYCambio(CpaParametroDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateCpaParametroTipoYCambio);

            dbProvider.AddInParameter(command, helper.Cpaprmtipomd, DbType.String, entity.Cpaprmtipomd);
            dbProvider.AddInParameter(command, helper.Cpaprmfechamd, DbType.DateTime, entity.Cpaprmfechamd);
            dbProvider.AddInParameter(command, helper.Cpaprmcambio, DbType.Decimal, entity.Cpaprmcambio);
            dbProvider.AddInParameter(command, helper.Cpaprmprecio, DbType.Decimal, entity.Cpaprmprecio);
            dbProvider.AddInParameter(command, helper.Cpaprmusumodificacion, DbType.String, entity.Cpaprmusumodificacion);
            dbProvider.AddInParameter(command, helper.Cpaprmfecmodificacion, DbType.DateTime, entity.Cpaprmfecmodificacion);
            dbProvider.AddInParameter(command, helper.Cpaprmcodi, DbType.Int32, entity.Cpaprmcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void UpdateCpaParametroEstado(CpaParametroDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateCpaParametroEstado);

            dbProvider.AddInParameter(command, helper.Cpaprmestado, DbType.String, entity.Cpaprmestado);
            dbProvider.AddInParameter(command, helper.Cpaprmusumodificacion, DbType.String, entity.Cpaprmusumodificacion);
            dbProvider.AddInParameter(command, helper.Cpaprmfecmodificacion, DbType.DateTime, entity.Cpaprmfecmodificacion);
            dbProvider.AddInParameter(command, helper.Cpaprmcodi, DbType.Int32, entity.Cpaprmcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cpaPrmCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cpaprmcodi, DbType.Int32, cpaPrmCodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CpaParametroDTO GetById(int cpaPrmCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cpaprmcodi, DbType.Int32, cpaPrmCodi);
            CpaParametroDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CpaParametroDTO> List()
        {
            List<CpaParametroDTO> entities = new List<CpaParametroDTO>();
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

        public List<CpaParametroDTO> ListaParametrosRegistrados(int revision, string estado, int anio)
        {
            CpaParametroDTO entity = new CpaParametroDTO();
            List<CpaParametroDTO> entitys = new List<CpaParametroDTO>();
            string query = string.Format(helper.SqlListaParametrosRegistrados, revision, estado, anio);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new CpaParametroDTO();

                    int iCpaprmcodi = dr.GetOrdinal(helper.Cpaprmcodi);
                    if (!dr.IsDBNull(iCpaprmcodi)) entity.Cpaprmcodi = dr.GetInt32(iCpaprmcodi);

                    int iCparcodi = dr.GetOrdinal(helper.Cparcodi);
                    if (!dr.IsDBNull(iCparcodi)) entity.Cparcodi = dr.GetInt32(iCparcodi);

                    int iCpaprmanio = dr.GetOrdinal(helper.Cpaprmanio);
                    if (!dr.IsDBNull(iCpaprmanio)) entity.Cpaprmanio = dr.GetInt32(iCpaprmanio);

                    int iCpaprmmes = dr.GetOrdinal(helper.Cpaprmmes);
                    if (!dr.IsDBNull(iCpaprmmes)) entity.Cpaprmmes = dr.GetInt32(iCpaprmmes);

                    int iCpaprmtipomd = dr.GetOrdinal(helper.Cpaprmtipomd);
                    if (!dr.IsDBNull(iCpaprmtipomd)) entity.Cpaprmtipomd = dr.GetString(iCpaprmtipomd);

                    int iCpaprmfechamd = dr.GetOrdinal(helper.Cpaprmfechamd);
                    if (!dr.IsDBNull(iCpaprmfechamd)) entity.Cpaprmfechamd = dr.GetDateTime(iCpaprmfechamd);

                    int iCpaprmcambio = dr.GetOrdinal(helper.Cpaprmcambio);
                    if (!dr.IsDBNull(iCpaprmcambio)) entity.Cpaprmcambio = dr.GetDecimal(iCpaprmcambio);

                    int iCpaprmprecio = dr.GetOrdinal(helper.Cpaprmprecio);
                    if (!dr.IsDBNull(iCpaprmprecio)) entity.Cpaprmprecio = dr.GetDecimal(iCpaprmprecio);

                    int iCpaprmestado = dr.GetOrdinal(helper.Cpaprmestado);
                    if (!dr.IsDBNull(iCpaprmestado)) entity.Cpaprmestado = dr.GetString(iCpaprmestado);

                    int iCpaprmcorrelativo = dr.GetOrdinal(helper.Cpaprmcorrelativo);
                    if (!dr.IsDBNull(iCpaprmcorrelativo)) entity.Cpaprmcorrelativo = dr.GetInt32(iCpaprmcorrelativo);

                    int iCpaprmusucreacion = dr.GetOrdinal(helper.Cpaprmusucreacion);
                    if (!dr.IsDBNull(iCpaprmusucreacion)) entity.Cpaprmusucreacion = dr.GetString(iCpaprmusucreacion);

                    int iCpaprmfeccreacion = dr.GetOrdinal(helper.Cpaprmfeccreacion);
                    if (!dr.IsDBNull(iCpaprmfeccreacion)) entity.Cpaprmfeccreacion = dr.GetDateTime(iCpaprmfeccreacion);

                    int iCpaprmusumodificacion = dr.GetOrdinal(helper.Cpaprmusumodificacion);
                    if (!dr.IsDBNull(iCpaprmusumodificacion)) entity.Cpaprmusumodificacion = dr.GetString(iCpaprmusumodificacion);

                    int iCpaprmfecmodificacion = dr.GetOrdinal(helper.Cpaprmfecmodificacion);
                    if (!dr.IsDBNull(iCpaprmfecmodificacion)) entity.Cpaprmfecmodificacion = dr.GetDateTime(iCpaprmfecmodificacion);

                    int iAnioMes = dr.GetOrdinal(helper.Aniomes);
                    if (!dr.IsDBNull(iAnioMes)) entity.Aniomes = dr.GetString(iAnioMes);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CpaParametroDTO> GetByRevisionByEstado(int cparcodi, string cpaprmestado)
        {
            string query = string.Format(helper.SqlGetByRevisionByEstado, cparcodi, cpaprmestado);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<CpaParametroDTO> entitys = new List<CpaParametroDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CpaParametroDTO entity = helper.Create(dr);
                    entitys.Add(entity);
                }
            }

            return entitys;

        }

        public List<CpaParametroDTO> ListaParametrosByRevisionAnioMesEstado(int revision, int anio, int mes, string estado)
        {
            CpaParametroDTO entity = new CpaParametroDTO();
            List<CpaParametroDTO> entitys = new List<CpaParametroDTO>();
            string query = string.Format(helper.SqlListaParametrosByRevisionAnioMesEstado, revision, anio, mes, estado);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new CpaParametroDTO();

                    int iCpaprmcodi = dr.GetOrdinal(helper.Cpaprmcodi);
                    if (!dr.IsDBNull(iCpaprmcodi)) entity.Cpaprmcodi = dr.GetInt32(iCpaprmcodi);

                    int iCparcodi = dr.GetOrdinal(helper.Cparcodi);
                    if (!dr.IsDBNull(iCparcodi)) entity.Cparcodi = dr.GetInt32(iCparcodi);

                    int iCpaprmanio = dr.GetOrdinal(helper.Cpaprmanio);
                    if (!dr.IsDBNull(iCpaprmanio)) entity.Cpaprmanio = dr.GetInt32(iCpaprmanio);

                    int iCpaprmmes = dr.GetOrdinal(helper.Cpaprmmes);
                    if (!dr.IsDBNull(iCpaprmmes)) entity.Cpaprmmes = dr.GetInt32(iCpaprmmes);

                    int iCpaprmtipomd = dr.GetOrdinal(helper.Cpaprmtipomd);
                    if (!dr.IsDBNull(iCpaprmtipomd)) entity.Cpaprmtipomd = dr.GetString(iCpaprmtipomd);

                    int iCpaprmfechamd = dr.GetOrdinal(helper.Cpaprmfechamd);
                    if (!dr.IsDBNull(iCpaprmfechamd)) entity.Cpaprmfechamd = dr.GetDateTime(iCpaprmfechamd);

                    int iCpaprmcambio = dr.GetOrdinal(helper.Cpaprmcambio);
                    if (!dr.IsDBNull(iCpaprmcambio)) entity.Cpaprmcambio = dr.GetDecimal(iCpaprmcambio);

                    int iCpaprmprecio = dr.GetOrdinal(helper.Cpaprmprecio);
                    if (!dr.IsDBNull(iCpaprmprecio)) entity.Cpaprmprecio = dr.GetDecimal(iCpaprmprecio);

                    int iCpaprmestado = dr.GetOrdinal(helper.Cpaprmestado);
                    if (!dr.IsDBNull(iCpaprmestado)) entity.Cpaprmestado = dr.GetString(iCpaprmestado);

                    int iCpaprmcorrelativo = dr.GetOrdinal(helper.Cpaprmcorrelativo);
                    if (!dr.IsDBNull(iCpaprmcorrelativo)) entity.Cpaprmcorrelativo = dr.GetInt32(iCpaprmcorrelativo);

                    int iCpaprmusucreacion = dr.GetOrdinal(helper.Cpaprmusucreacion);
                    if (!dr.IsDBNull(iCpaprmusucreacion)) entity.Cpaprmusucreacion = dr.GetString(iCpaprmusucreacion);

                    int iCpaprmfeccreacion = dr.GetOrdinal(helper.Cpaprmfeccreacion);
                    if (!dr.IsDBNull(iCpaprmfeccreacion)) entity.Cpaprmfeccreacion = dr.GetDateTime(iCpaprmfeccreacion);

                    int iCpaprmusumodificacion = dr.GetOrdinal(helper.Cpaprmusumodificacion);
                    if (!dr.IsDBNull(iCpaprmusumodificacion)) entity.Cpaprmusumodificacion = dr.GetString(iCpaprmusumodificacion);

                    int iCpaprmfecmodificacion = dr.GetOrdinal(helper.Cpaprmfecmodificacion);
                    if (!dr.IsDBNull(iCpaprmfecmodificacion)) entity.Cpaprmfecmodificacion = dr.GetDateTime(iCpaprmfecmodificacion);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public CpaParametroDTO GetByRevisionMes(int Cparcodi, int Cpaprmmes)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByRevisionMes);

            dbProvider.AddInParameter(command, helper.Cparcodi, DbType.Int32, Cparcodi);
            dbProvider.AddInParameter(command, helper.Cpaprmmes, DbType.Int32, Cpaprmmes);

            CpaParametroDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }
    }
}
