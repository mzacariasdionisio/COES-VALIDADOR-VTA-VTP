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
    /// Clase de acceso a datos de la tabla PFR_RELACION_INDISPONIBILIDAD
    /// </summary>
    public class PfrRelacionIndisponibilidadRepository: RepositoryBase, IPfrRelacionIndisponibilidadRepository
    {
        public PfrRelacionIndisponibilidadRepository(string strConn): base(strConn)
        {
        }

        PfrRelacionIndisponibilidadHelper helper = new PfrRelacionIndisponibilidadHelper();     

        public int Save(PfrRelacionIndisponibilidadDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfrrincodi, DbType.Int32, id));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfrrptcodi, DbType.Int32, entity.Pfrrptcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Irptcodi, DbType.Int32, entity.Irptcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfrrintipo, DbType.Int32, entity.Pfrrintipo));

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PfrRelacionIndisponibilidadDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Pfrrincodi, DbType.Int32, entity.Pfrrincodi);
            dbProvider.AddInParameter(command, helper.Pfrrptcodi, DbType.Int32, entity.Pfrrptcodi);
            dbProvider.AddInParameter(command, helper.Irptcodi, DbType.Int32, entity.Irptcodi);
            dbProvider.AddInParameter(command, helper.Pfrrintipo, DbType.Int32, entity.Pfrrintipo);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int pfrrincodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Pfrrincodi, DbType.Int32, pfrrincodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PfrRelacionIndisponibilidadDTO GetById(int pfrrincodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Pfrrincodi, DbType.Int32, pfrrincodi);
            PfrRelacionIndisponibilidadDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PfrRelacionIndisponibilidadDTO> List()
        {
            List<PfrRelacionIndisponibilidadDTO> entitys = new List<PfrRelacionIndisponibilidadDTO>();
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

        public List<PfrRelacionIndisponibilidadDTO> GetByCriteria(int pfrrptcodi)
        {
            List<PfrRelacionIndisponibilidadDTO> entitys = new List<PfrRelacionIndisponibilidadDTO>();
            var sqlQuery = string.Format(helper.SqlGetByCriteria, pfrrptcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }
    }
}
