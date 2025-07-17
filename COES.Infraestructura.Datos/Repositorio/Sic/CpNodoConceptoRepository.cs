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
    /// Clase de acceso a datos de la tabla CP_NODO_CONCEPTO
    /// </summary>
    public class CpNodoConceptoRepository: RepositoryBase, ICpNodoConceptoRepository
    {
        public CpNodoConceptoRepository(string strConn): base(strConn)
        {
        }

        CpNodoConceptoHelper helper = new CpNodoConceptoHelper();

        public int Save(CpNodoConceptoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cpnconcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cpncontipo, DbType.Int32, entity.Cpncontipo);
            dbProvider.AddInParameter(command, helper.Cpnconnombre, DbType.String, entity.Cpnconnombre);
            dbProvider.AddInParameter(command, helper.Cpnconunidad, DbType.String, entity.Cpnconunidad);
            dbProvider.AddInParameter(command, helper.Cpnconestado, DbType.Int32, entity.Cpnconestado);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CpNodoConceptoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cpnconcodi, DbType.Int32, entity.Cpnconcodi);
            dbProvider.AddInParameter(command, helper.Cpncontipo, DbType.Int32, entity.Cpncontipo);
            dbProvider.AddInParameter(command, helper.Cpnconnombre, DbType.String, entity.Cpnconnombre);
            dbProvider.AddInParameter(command, helper.Cpnconunidad, DbType.String, entity.Cpnconunidad);
            dbProvider.AddInParameter(command, helper.Cpnconestado, DbType.Int32, entity.Cpnconestado);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cpnconcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cpnconcodi, DbType.Int32, cpnconcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CpNodoConceptoDTO GetById(int cpnconcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cpnconcodi, DbType.Int32, cpnconcodi);
            CpNodoConceptoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CpNodoConceptoDTO> List()
        {
            List<CpNodoConceptoDTO> entitys = new List<CpNodoConceptoDTO>();
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

        public List<CpNodoConceptoDTO> GetByCriteria()
        {
            List<CpNodoConceptoDTO> entitys = new List<CpNodoConceptoDTO>();
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
