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
    /// Clase de acceso a datos de la tabla PF_RELACION_INSUMOS
    /// </summary>
    public class PfRelacionInsumosRepository : RepositoryBase, IPfRelacionInsumosRepository
    {
        public PfRelacionInsumosRepository(string strConn) : base(strConn)
        {
        }

        PfRelacionInsumosHelper helper = new PfRelacionInsumosHelper();

        public int Save(PfRelacionInsumosDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfrptcodi, DbType.Int32, entity.Pfrptcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfverscodi, DbType.Int32, entity.Pfverscodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfrinscodi, DbType.Int32, id));

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PfRelacionInsumosDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Pfrptcodi, DbType.Int32, entity.Pfrptcodi);
            dbProvider.AddInParameter(command, helper.Pfverscodi, DbType.Int32, entity.Pfverscodi);
            dbProvider.AddInParameter(command, helper.Pfrinscodi, DbType.Int32, entity.Pfrinscodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int pfrinscodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Pfrinscodi, DbType.Int32, pfrinscodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PfRelacionInsumosDTO GetById(int pfrinscodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Pfrinscodi, DbType.Int32, pfrinscodi);
            PfRelacionInsumosDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PfRelacionInsumosDTO> List()
        {
            List<PfRelacionInsumosDTO> entitys = new List<PfRelacionInsumosDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iPfrecucodi = dr.GetOrdinal(helper.Pfrecucodi);
                    if (!dr.IsDBNull(iPfrecucodi)) entity.Pfrecucodi = dr.GetInt32(iPfrecucodi);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PfRelacionInsumosDTO> GetByCriteria(int pfrptcodi)
        {
            List<PfRelacionInsumosDTO> entitys = new List<PfRelacionInsumosDTO>();

            string sql = string.Format(helper.SqlGetByCriteria, pfrptcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iPfrecucodi = dr.GetOrdinal(helper.Pfrecucodi);
                    if (!dr.IsDBNull(iPfrecucodi)) entity.Pfrecucodi = dr.GetInt32(iPfrecucodi);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
