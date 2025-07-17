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
    /// Clase de acceso a datos de la tabla SI_CONCEPTO
    /// </summary>
    public class SiConceptoRepository : RepositoryBase, ISiConceptoRepository
    {
        public SiConceptoRepository(string strConn)
            : base(strConn)
        {
        }

        SiConceptoHelper helper = new SiConceptoHelper();

        public int Save(SiConceptoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Consiscodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Consisabrev, DbType.String, entity.Consisabrev);
            dbProvider.AddInParameter(command, helper.Consisdesc, DbType.String, entity.Consisdesc);
            dbProvider.AddInParameter(command, helper.Consisactivo, DbType.String, entity.Consisactivo);
            dbProvider.AddInParameter(command, helper.Consisorden, DbType.Int32, entity.Consisorden);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SiConceptoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Consiscodi, DbType.Int32, entity.Consiscodi);
            dbProvider.AddInParameter(command, helper.Consisabrev, DbType.String, entity.Consisabrev);
            dbProvider.AddInParameter(command, helper.Consisdesc, DbType.String, entity.Consisdesc);
            dbProvider.AddInParameter(command, helper.Consisactivo, DbType.String, entity.Consisactivo);
            dbProvider.AddInParameter(command, helper.Consisorden, DbType.Int32, entity.Consisorden);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int consiscodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Consiscodi, DbType.Int32, consiscodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiConceptoDTO GetById(int consiscodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Consiscodi, DbType.Int32, consiscodi);
            SiConceptoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiConceptoDTO> List()
        {
            List<SiConceptoDTO> entitys = new List<SiConceptoDTO>();
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

        public List<SiConceptoDTO> GetByCriteria()
        {
            List<SiConceptoDTO> entitys = new List<SiConceptoDTO>();
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
