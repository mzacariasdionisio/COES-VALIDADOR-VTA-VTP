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
    /// Clase de acceso a datos de la tabla PMO_QN_LECTURA
    /// </summary>
    public class PmoQnLecturaRepository : RepositoryBase, IPmoQnLecturaRepository
    {
        public PmoQnLecturaRepository(string strConn) : base(strConn)
        {
        }

        PmoQnLecturaHelper helper = new PmoQnLecturaHelper();

        public int Save(PmoQnLecturaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Qnlectcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Qnlectnomb, DbType.String, entity.Qnlectnomb);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PmoQnLecturaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Qnlectnomb, DbType.String, entity.Qnlectnomb);
            dbProvider.AddInParameter(command, helper.Qnlectcodi, DbType.Int32, entity.Qnlectcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int qnlectcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Qnlectcodi, DbType.Int32, qnlectcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PmoQnLecturaDTO GetById(int qnlectcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Qnlectcodi, DbType.Int32, qnlectcodi);
            PmoQnLecturaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PmoQnLecturaDTO> List()
        {
            List<PmoQnLecturaDTO> entitys = new List<PmoQnLecturaDTO>();
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

        public List<PmoQnLecturaDTO> GetByCriteria()
        {
            List<PmoQnLecturaDTO> entitys = new List<PmoQnLecturaDTO>();
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
    }
}
