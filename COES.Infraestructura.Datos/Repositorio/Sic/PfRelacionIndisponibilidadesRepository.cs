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
    /// Clase de acceso a datos de la tabla PF_RELACION_INDISPONIBILIDADES
    /// </summary>
    public class PfRelacionIndisponibilidadesRepository : RepositoryBase, IPfRelacionIndisponibilidadesRepository
    {
        public PfRelacionIndisponibilidadesRepository(string strConn) : base(strConn)
        {
        }

        PfRelacionIndisponibilidadesHelper helper = new PfRelacionIndisponibilidadesHelper();

        public int Save(PfRelacionIndisponibilidadesDTO entity, IDbConnection conn, DbTransaction tran)
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
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Irptcodi, DbType.Int32, entity.Irptcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfrindcodi, DbType.Int32, id));

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PfRelacionIndisponibilidadesDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Pfrptcodi, DbType.Int32, entity.Pfrptcodi);
            dbProvider.AddInParameter(command, helper.Irptcodi, DbType.Int32, entity.Irptcodi);
            dbProvider.AddInParameter(command, helper.Pfrindcodi, DbType.Int32, entity.Pfrindcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int pfrindcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Pfrindcodi, DbType.Int32, pfrindcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PfRelacionIndisponibilidadesDTO GetById(int pfrindcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Pfrindcodi, DbType.Int32, pfrindcodi);
            PfRelacionIndisponibilidadesDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PfRelacionIndisponibilidadesDTO> List()
        {
            List<PfRelacionIndisponibilidadesDTO> entitys = new List<PfRelacionIndisponibilidadesDTO>();
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

        public List<PfRelacionIndisponibilidadesDTO> GetByCriteria(int pfrptcodi)
        {
            List<PfRelacionIndisponibilidadesDTO> entitys = new List<PfRelacionIndisponibilidadesDTO>();

            string sql = string.Format(helper.SqlGetByCriteria, pfrptcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iIcuacodi = dr.GetOrdinal(helper.Icuacodi);
                    if (!dr.IsDBNull(iIcuacodi)) entity.Icuacodi = Convert.ToInt32(dr.GetValue(iIcuacodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
