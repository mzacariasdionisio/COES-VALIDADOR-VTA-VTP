using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Transferencias;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase de acceso a datos de la tabla ST_RESPAGOELE
    /// </summary>
    public class StRespagoeleRepository : RepositoryBase, IStRespagoeleRepository
    {
        public StRespagoeleRepository(string strConn)
            : base(strConn)
        {
        }

        StRespagoeleHelper helper = new StRespagoeleHelper();

        public int Save(StRespagoeleDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Respaecodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Respagcodi, DbType.Int32, entity.Respagcodi);
            dbProvider.AddInParameter(command, helper.Stcompcodi, DbType.Int32, entity.Stcompcodi);
            dbProvider.AddInParameter(command, helper.Respaecodelemento, DbType.String, entity.Respaecodelemento);
            dbProvider.AddInParameter(command, helper.Respaevalor, DbType.Int32, entity.Respaevalor);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(StRespagoeleDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Respagcodi, DbType.Int32, entity.Respagcodi);
            dbProvider.AddInParameter(command, helper.Stcompcodi, DbType.Int32, entity.Stcompcodi);
            dbProvider.AddInParameter(command, helper.Respaecodelemento, DbType.String, entity.Respaecodelemento);
            dbProvider.AddInParameter(command, helper.Respaevalor, DbType.Int32, entity.Respaevalor);
            dbProvider.AddInParameter(command, helper.Respaecodi, DbType.Int32, entity.Respaecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int strecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, strecacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteStRespagoEleVersion(int strecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteStRespagoEleVersion);
            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, strecacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public StRespagoeleDTO GetById(int respagcodi, int stcompcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Respagcodi, DbType.Int32, respagcodi);
            dbProvider.AddInParameter(command, helper.Stcompcodi, DbType.Int32, stcompcodi);
            StRespagoeleDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<StRespagoeleDTO> List()
        {
            List<StRespagoeleDTO> entitys = new List<StRespagoeleDTO>();
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

        public List<StRespagoeleDTO> GetByCriteria()
        {
            List<StRespagoeleDTO> entitys = new List<StRespagoeleDTO>();
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

        public List<StRespagoeleDTO> ListStRespagElePorID(int id)
        {
            List<StRespagoeleDTO> entitys = new List<StRespagoeleDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListStRespagElePorID);
            dbProvider.AddInParameter(command, helper.Respagcodi, DbType.Int32, id);

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
