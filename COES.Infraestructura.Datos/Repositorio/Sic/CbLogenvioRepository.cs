using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla CB_LOGENVIO
    /// </summary>
    public class CbLogenvioRepository : RepositoryBase, ICbLogenvioRepository
    {
        public CbLogenvioRepository(string strConn) : base(strConn)
        {
        }

        readonly CbLogenvioHelper helper = new CbLogenvioHelper();

        public int Save(CbLogenvioDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Logenvcodi, DbType.Int32, id));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Estenvcodi, DbType.Int32, entity.Estenvcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cbenvcodi, DbType.Int32, entity.Cbenvcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Logenvusucreacion, DbType.String, entity.Logenvusucreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Logenvfeccreacion, DbType.DateTime, entity.Logenvfeccreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Logenvobservacion, DbType.String, entity.Logenvobservacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Logenvfecrecepcion, DbType.DateTime, entity.Logenvfecrecepcion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Logenvfeclectura, DbType.DateTime, entity.Logenvfeclectura));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Logenvusurecepcion, DbType.String, entity.Logenvusurecepcion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Logenvusulectura, DbType.String, entity.Logenvusulectura));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Logenvplazo, DbType.Int32, entity.Logenvplazo));

            command.ExecuteNonQuery();
            return id;
        }

        public void Update(CbLogenvioDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            
            dbProvider.AddInParameter(command, helper.Estenvcodi, DbType.Int32, entity.Estenvcodi);
            dbProvider.AddInParameter(command, helper.Cbenvcodi, DbType.Int32, entity.Cbenvcodi);
            dbProvider.AddInParameter(command, helper.Logenvusucreacion, DbType.String, entity.Logenvusucreacion);
            dbProvider.AddInParameter(command, helper.Logenvfeccreacion, DbType.DateTime, entity.Logenvfeccreacion);
            dbProvider.AddInParameter(command, helper.Logenvobservacion, DbType.String, entity.Logenvobservacion);
            dbProvider.AddInParameter(command, helper.Logenvfecrecepcion, DbType.DateTime, entity.Logenvfecrecepcion);
            dbProvider.AddInParameter(command, helper.Logenvfeclectura, DbType.DateTime, entity.Logenvfeclectura);
            dbProvider.AddInParameter(command, helper.Logenvusurecepcion, DbType.String, entity.Logenvusurecepcion);
            dbProvider.AddInParameter(command, helper.Logenvusulectura, DbType.String, entity.Logenvusulectura);
            dbProvider.AddInParameter(command, helper.Logenvplazo, DbType.Int32, entity.Logenvplazo);
            
            dbProvider.AddInParameter(command, helper.Logenvcodi, DbType.Int32, entity.Logenvcodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int logenvcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Logenvcodi, DbType.Int32, logenvcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CbLogenvioDTO GetById(int logenvcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Logenvcodi, DbType.Int32, logenvcodi);
            CbLogenvioDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CbLogenvioDTO> List()
        {
            List<CbLogenvioDTO> entitys = new List<CbLogenvioDTO>();
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

        public List<CbLogenvioDTO> GetByCriteria(int cbenvcodi)
        {
            List<CbLogenvioDTO> entitys = new List<CbLogenvioDTO>();
            string sql = string.Format(helper.SqlGetByCriteria, cbenvcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CbLogenvioDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CbLogenvioDTO> ListarPorEnviosYEstado(string envioscodis, string estados)
        {
            List<CbLogenvioDTO> entitys = new List<CbLogenvioDTO>();
            string sql = string.Format(helper.SqlGetByEnviosYEstado, envioscodis, estados);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<CbLogenvioDTO> ListarLogGaseosoPorEmpresaYRango(string empresas, string fecIni, string fecFin)
        {
            List<CbLogenvioDTO> entitys = new List<CbLogenvioDTO>();
            string sql = string.Format(helper.SqlGetLogGaseososPorEmpresaYRango, empresas, fecIni, fecFin);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CbLogenvioDTO entity = helper.Create(dr);

                    int iEstenvnomb = dr.GetOrdinal(helper.Estenvnomb);
                    if (!dr.IsDBNull(iEstenvnomb)) entity.Estenvnomb = dr.GetString(iEstenvnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
