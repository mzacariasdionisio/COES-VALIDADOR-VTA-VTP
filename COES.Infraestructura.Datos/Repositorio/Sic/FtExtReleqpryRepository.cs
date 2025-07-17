using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla FT_EXT_RELEQPRY
    /// </summary>
    public class FtExtReleqpryRepository : RepositoryBase, IFtExtReleqpryRepository
    {
        public FtExtReleqpryRepository(string strConn) : base(strConn)
        {
        }

        FtExtReleqpryHelper helper = new FtExtReleqpryHelper();

        public int Save(FtExtReleqpryDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Ftreqpcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Ftprycodi, DbType.Int32, entity.Ftprycodi);
            dbProvider.AddInParameter(command, helper.Ftreqpestado, DbType.Int32, entity.Ftreqpestado);
            dbProvider.AddInParameter(command, helper.Ftreqpusucreacion, DbType.String, entity.Ftreqpusucreacion);
            dbProvider.AddInParameter(command, helper.Ftreqpfeccreacion, DbType.DateTime, entity.Ftreqpfeccreacion);
            dbProvider.AddInParameter(command, helper.Ftreqpusumodificacion, DbType.String, entity.Ftreqpusumodificacion);
            dbProvider.AddInParameter(command, helper.Ftreqpfecmodificacion, DbType.DateTime, entity.Ftreqpfecmodificacion);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public int Save(FtExtReleqpryDTO entity, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;
                dbCommand.CommandText = helper.SqlGetMaxId;

                object result = dbCommand.ExecuteScalar();
                int id = result == null ? 1 : Convert.ToInt32(result);

                dbCommand.CommandText = helper.SqlSave;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Ftreqpcodi, DbType.Int32, id));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Equicodi, DbType.Int32, entity.Equicodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Ftprycodi, DbType.Int32, entity.Ftprycodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Ftreqpestado, DbType.Int32, entity.Ftreqpestado));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Ftreqpusucreacion, DbType.String, entity.Ftreqpusucreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Ftreqpfeccreacion, DbType.DateTime, entity.Ftreqpfeccreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Ftreqpusumodificacion, DbType.String, entity.Ftreqpusumodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Ftreqpfecmodificacion, DbType.DateTime, entity.Ftreqpfecmodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Grupocodi, DbType.Int32, entity.Grupocodi));


                dbCommand.ExecuteNonQuery();
                return id;
            }
        }

        public void Update(FtExtReleqpryDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);


            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Ftprycodi, DbType.Int32, entity.Ftprycodi);
            dbProvider.AddInParameter(command, helper.Ftreqpestado, DbType.Int32, entity.Ftreqpestado);
            dbProvider.AddInParameter(command, helper.Ftreqpusucreacion, DbType.String, entity.Ftreqpusucreacion);
            dbProvider.AddInParameter(command, helper.Ftreqpfeccreacion, DbType.DateTime, entity.Ftreqpfeccreacion);
            dbProvider.AddInParameter(command, helper.Ftreqpusumodificacion, DbType.String, entity.Ftreqpusumodificacion);
            dbProvider.AddInParameter(command, helper.Ftreqpfecmodificacion, DbType.DateTime, entity.Ftreqpfecmodificacion);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);

            dbProvider.AddInParameter(command, helper.Ftreqpcodi, DbType.Int32, entity.Ftreqpcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(FtExtReleqpryDTO entity, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;
                dbCommand.CommandText = helper.SqlUpdate;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Equicodi, DbType.Int32, entity.Equicodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Ftprycodi, DbType.Int32, entity.Ftprycodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Ftreqpestado, DbType.Int32, entity.Ftreqpestado));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Ftreqpusucreacion, DbType.String, entity.Ftreqpusucreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Ftreqpfeccreacion, DbType.DateTime, entity.Ftreqpfeccreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Ftreqpusumodificacion, DbType.String, entity.Ftreqpusumodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Ftreqpfecmodificacion, DbType.DateTime, entity.Ftreqpfecmodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Grupocodi, DbType.Int32, entity.Grupocodi));

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Ftreqpcodi, DbType.Int32, entity.Ftreqpcodi));


                dbCommand.ExecuteNonQuery();

            }
        }

        public void Delete(int ftreqpcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Ftreqpcodi, DbType.Int32, ftreqpcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public FtExtReleqpryDTO GetById(int ftreqpcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Ftreqpcodi, DbType.Int32, ftreqpcodi);
            FtExtReleqpryDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<FtExtReleqpryDTO> List()
        {
            List<FtExtReleqpryDTO> entitys = new List<FtExtReleqpryDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<FtExtReleqpryDTO> GetByCriteria()
        {
            List<FtExtReleqpryDTO> entitys = new List<FtExtReleqpryDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<FtExtReleqpryDTO> ListarPorEquipo(int equicodi)
        {
            List<FtExtReleqpryDTO> entitys = new List<FtExtReleqpryDTO>();

            string query = string.Format(helper.SqlListarPorEquipo, equicodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FtExtReleqpryDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iFtpryeocodigo = dr.GetOrdinal(helper.Ftpryeocodigo);
                    if (!dr.IsDBNull(iFtpryeocodigo)) entity.Ftpryeocodigo = dr.GetString(iFtpryeocodigo);

                    int iFtpryeonombre = dr.GetOrdinal(helper.Ftpryeonombre);
                    if (!dr.IsDBNull(iFtpryeonombre)) entity.Ftpryeonombre = dr.GetString(iFtpryeonombre);

                    int iFtprynombre = dr.GetOrdinal(helper.Ftprynombre);
                    if (!dr.IsDBNull(iFtprynombre)) entity.Ftprynombre = dr.GetString(iFtprynombre);


                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<FtExtReleqpryDTO> ListarPorGrupo(int grupocodi)
        {
            List<FtExtReleqpryDTO> entitys = new List<FtExtReleqpryDTO>();

            string query = string.Format(helper.SqlListarPorGrupo, grupocodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FtExtReleqpryDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iFtpryeocodigo = dr.GetOrdinal(helper.Ftpryeocodigo);
                    if (!dr.IsDBNull(iFtpryeocodigo)) entity.Ftpryeocodigo = dr.GetString(iFtpryeocodigo);

                    int iFtpryeonombre = dr.GetOrdinal(helper.Ftpryeonombre);
                    if (!dr.IsDBNull(iFtpryeonombre)) entity.Ftpryeonombre = dr.GetString(iFtpryeonombre);

                    int iFtprynombre = dr.GetOrdinal(helper.Ftprynombre);
                    if (!dr.IsDBNull(iFtprynombre)) entity.Ftprynombre = dr.GetString(iFtprynombre);


                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<FtExtReleqpryDTO> ListarSoloEquipos()
        {
            List<FtExtReleqpryDTO> entitys = new List<FtExtReleqpryDTO>();

            string query = string.Format(helper.SqlListarSoloEquipos);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<FtExtReleqpryDTO> ListarSoloGrupos()
        {
            List<FtExtReleqpryDTO> entitys = new List<FtExtReleqpryDTO>();

            string query = string.Format(helper.SqlListarSoloGrupos);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<FtExtReleqpryDTO> ListarPorEmpresaPropietariaYProyecto(int ftprycodi, int emprcodi)
        {
            List<FtExtReleqpryDTO> entitys = new List<FtExtReleqpryDTO>();

            string query = string.Format(helper.SqlListarPorEmpresaPropYProyecto, ftprycodi, emprcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FtExtReleqpryDTO entity = helper.Create(dr);

                    int iNombempresaelemento = dr.GetOrdinal(helper.Nombempresaelemento);
                    if (!dr.IsDBNull(iNombempresaelemento)) entity.Nombempresaelemento = dr.GetString(iNombempresaelemento);

                    int iIdempresaelemento = dr.GetOrdinal(helper.Idempresaelemento);
                    if (!dr.IsDBNull(iIdempresaelemento)) entity.Idempresaelemento = Convert.ToInt32(dr.GetValue(iIdempresaelemento));

                    int iNombreelemento = dr.GetOrdinal(helper.Nombreelemento);
                    if (!dr.IsDBNull(iNombreelemento)) entity.Nombreelemento = dr.GetString(iNombreelemento);

                    int iTipoelemento = dr.GetOrdinal(helper.Tipoelemento);
                    if (!dr.IsDBNull(iTipoelemento)) entity.Tipoelemento = dr.GetString(iTipoelemento);

                    int iAreaelemento = dr.GetOrdinal(helper.Areaelemento);
                    if (!dr.IsDBNull(iAreaelemento)) entity.Areaelemento = dr.GetString(iAreaelemento);

                    int iEstadoelemento = dr.GetOrdinal(helper.Estadoelemento);
                    if (!dr.IsDBNull(iEstadoelemento)) entity.Estadoelemento = dr.GetString(iEstadoelemento);

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iCatecodi = dr.GetOrdinal(helper.Catecodi);
                    if (!dr.IsDBNull(iCatecodi)) entity.Catecodi = Convert.ToInt32(dr.GetValue(iCatecodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<FtExtReleqpryDTO> ListarPorEmpresaCopropietariaYProyecto(int ftprycodi, int emprcodi)
        {
            List<FtExtReleqpryDTO> entitys = new List<FtExtReleqpryDTO>();

            string query = string.Format(helper.SqlListarPorEmpresaCopropYProyecto, ftprycodi, emprcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FtExtReleqpryDTO entity = helper.Create(dr);

                    int iIdempresaelemento = dr.GetOrdinal(helper.Idempresaelemento);
                    if (!dr.IsDBNull(iIdempresaelemento)) entity.Idempresaelemento = Convert.ToInt32(dr.GetValue(iIdempresaelemento));

                    int iNombempresaelemento = dr.GetOrdinal(helper.Nombempresaelemento);
                    if (!dr.IsDBNull(iNombempresaelemento)) entity.Nombempresaelemento = dr.GetString(iNombempresaelemento);

                    int iIdempresacopelemento = dr.GetOrdinal(helper.Idempresacopelemento);
                    if (!dr.IsDBNull(iIdempresacopelemento)) entity.Idempresacopelemento = Convert.ToInt32(dr.GetValue(iIdempresacopelemento));

                    int iNombempresacopelemento = dr.GetOrdinal(helper.Nombempresacopelemento);
                    if (!dr.IsDBNull(iNombempresacopelemento)) entity.Nombempresacopelemento = dr.GetString(iNombempresacopelemento);

                    int iNombreelemento = dr.GetOrdinal(helper.Nombreelemento);
                    if (!dr.IsDBNull(iNombreelemento)) entity.Nombreelemento = dr.GetString(iNombreelemento);

                    int iTipoelemento = dr.GetOrdinal(helper.Tipoelemento);
                    if (!dr.IsDBNull(iTipoelemento)) entity.Tipoelemento = dr.GetString(iTipoelemento);

                    int iAreaelemento = dr.GetOrdinal(helper.Areaelemento);
                    if (!dr.IsDBNull(iAreaelemento)) entity.Areaelemento = dr.GetString(iAreaelemento);

                    int iEstadoelemento = dr.GetOrdinal(helper.Estadoelemento);
                    if (!dr.IsDBNull(iEstadoelemento)) entity.Estadoelemento = dr.GetString(iEstadoelemento);

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }




    }
}
