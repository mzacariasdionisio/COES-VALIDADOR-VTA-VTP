using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla IN_FACTOR_VERSION_MMAYOR
    /// </summary>
    public class InFactorVersionMmayorRepository : RepositoryBase, IInFactorVersionMmayorRepository
    {
        private string strConexion;

        public InFactorVersionMmayorRepository(string strConn) : base(strConn)
        {
            strConexion = strConn;
        }

        InFactorVersionMmayorHelper helper = new InFactorVersionMmayorHelper();

        public IDbConnection BeginConnection()
        {
            Database db = DatabaseFactory.CreateDatabase(strConexion);
            IDbConnection conn = db.CreateConnection();
            conn.Open();
            return conn;
        }

        public DbTransaction StartTransaction(IDbConnection conn)
        {
            return (DbTransaction)conn.BeginTransaction();
        }

        public int GetMaxId()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(InFactorVersionMmayorDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Infmmcodi, DbType.Int32, entity.Infmmcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Infvercodi, DbType.Int32, entity.Infvercodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Infmmdescrip, DbType.String, entity.Infmmdescrip));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Infmmfechaini, DbType.DateTime, entity.Infmmfechaini));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Infmmfechafin, DbType.DateTime, entity.Infmmfechafin));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Infmmduracion, DbType.Decimal, entity.Infmmduracion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Claprocodi, DbType.Int32, entity.Claprocodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Tipoevencodi, DbType.Int32, entity.Tipoevencodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Infmmhoja, DbType.Int32, entity.Infmmhoja));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Infmmobspm, DbType.String, entity.Infmmobspm));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Infmmorigen, DbType.String, entity.Infmmorigen));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Infmmjustif, DbType.String, entity.Infmmjustif));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Infmmobsps, DbType.String, entity.Infmmobsps));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Infmmobspd, DbType.String, entity.Infmmobspd));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Infmmobse, DbType.String, entity.Infmmobse));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Infmmusumodificacion, DbType.String, entity.Infmmusumodificacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Infmmfecmodificacion, DbType.DateTime, entity.Infmmfecmodificacion));

            command.ExecuteNonQuery();
            return entity.Infmmcodi;
        }

        public void Update(InFactorVersionMmayorDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlUpdate;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Infvercodi, DbType.Int32, entity.Infvercodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Infmmdescrip, DbType.String, entity.Infmmdescrip));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Infmmfechaini, DbType.DateTime, entity.Infmmfechaini));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Infmmfechafin, DbType.DateTime, entity.Infmmfechafin));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Infmmduracion, DbType.Decimal, entity.Infmmduracion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Claprocodi, DbType.Int32, entity.Claprocodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Tipoevencodi, DbType.Int32, entity.Tipoevencodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Infmmhoja, DbType.Int32, entity.Infmmhoja));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Infmmobspm, DbType.String, entity.Infmmobspm));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Infmmorigen, DbType.String, entity.Infmmorigen));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Infmmjustif, DbType.String, entity.Infmmjustif));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Infmmobsps, DbType.String, entity.Infmmobsps));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Infmmobspd, DbType.String, entity.Infmmobspd));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Infmmobse, DbType.String, entity.Infmmobse));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Infmmusumodificacion, DbType.String, entity.Infmmusumodificacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Infmmfecmodificacion, DbType.DateTime, entity.Infmmfecmodificacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Infmmcodi, DbType.Int32, entity.Infmmcodi));

            command.ExecuteNonQuery();
        }

        public void Delete(int infmmcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Infmmcodi, DbType.Int32, infmmcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public InFactorVersionMmayorDTO GetById(int infmmcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Infmmcodi, DbType.Int32, infmmcodi);
            InFactorVersionMmayorDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    entity.Infmmjustif = entity.Infmmjustif ?? string.Empty;
                }
            }

            return entity;
        }

        public List<InFactorVersionMmayorDTO> List()
        {
            List<InFactorVersionMmayorDTO> entitys = new List<InFactorVersionMmayorDTO>();
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

        public List<InFactorVersionMmayorDTO> GetByCriteria(int infvercodi, string infmmhoja)
        {
            List<InFactorVersionMmayorDTO> entitys = new List<InFactorVersionMmayorDTO>();

            string sql = string.Format(helper.SqlGetByCriteria, infvercodi, infmmhoja);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    InFactorVersionMmayorDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);
                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iFamabrev = dr.GetOrdinal(helper.Famabrev);
                    if (!dr.IsDBNull(iFamabrev)) entity.Famabrev = dr.GetString(iFamabrev);

                    int iTipoevendesc = dr.GetOrdinal(helper.Tipoevendesc);
                    if (!dr.IsDBNull(iTipoevendesc)) entity.Tipoevendesc = dr.GetString(iTipoevendesc);

                    int iClapronombre = dr.GetOrdinal(helper.Clapronombre);
                    if (!dr.IsDBNull(iClapronombre)) entity.Clapronombre = dr.GetString(iClapronombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<SiEmpresaDTO> GetEmpresaByID(int infvercodi)
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetEmpresasByInfvercodi);

            dbProvider.AddInParameter(command, helper.Infvercodi, DbType.Int32, infvercodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaDTO entity = new SiEmpresaDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
