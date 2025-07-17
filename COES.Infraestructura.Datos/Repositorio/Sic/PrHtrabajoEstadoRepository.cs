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
    /// Clase de acceso a datos de la tabla PR_HTRABAJO_ESTADO
    /// </summary>
    public class PrHtrabajoEstadoRepository: RepositoryBase, IPrHtrabajoEstadoRepository
    {
        public PrHtrabajoEstadoRepository(string strConn): base(strConn)
        {
        }

        PrHtrabajoEstadoHelper helper = new PrHtrabajoEstadoHelper();

        public int Save(PrHtrabajoEstadoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Htestcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Htestcolor, DbType.String, entity.Htestcolor);
            dbProvider.AddInParameter(command, helper.Htestdesc, DbType.String, entity.Htestdesc);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PrHtrabajoEstadoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Htestcodi, DbType.Int32, entity.Htestcodi);
            dbProvider.AddInParameter(command, helper.Htestcolor, DbType.String, entity.Htestcolor);
            dbProvider.AddInParameter(command, helper.Htestdesc, DbType.String, entity.Htestdesc);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int htestcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Htestcodi, DbType.Int32, htestcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PrHtrabajoEstadoDTO GetById(int htestcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Htestcodi, DbType.Int32, htestcodi);
            PrHtrabajoEstadoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PrHtrabajoEstadoDTO> List()
        {
            List<PrHtrabajoEstadoDTO> entitys = new List<PrHtrabajoEstadoDTO>();
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

        public List<PrHtrabajoEstadoDTO> GetByCriteria()
        {
            List<PrHtrabajoEstadoDTO> entitys = new List<PrHtrabajoEstadoDTO>();
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
