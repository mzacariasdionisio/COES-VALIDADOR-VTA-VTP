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
    /// Clase de acceso a datos de la tabla CM_VERSIONPROGRAMA
    /// </summary>
    public class CmVersionprogramaRepository: RepositoryBase, ICmVersionprogramaRepository
    {
        public CmVersionprogramaRepository(string strConn): base(strConn)
        {
        }

        CmVersionprogramaHelper helper = new CmVersionprogramaHelper();

        public int Save(CmVersionprogramaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cmveprcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cmgncorrelativo, DbType.Int32, entity.Cmgncorrelativo);
            dbProvider.AddInParameter(command, helper.Cmveprvalor, DbType.String, entity.Cmveprvalor);
            dbProvider.AddInParameter(command, helper.Cmveprtipoprograma, DbType.String, entity.Cmveprtipoprograma);
            dbProvider.AddInParameter(command, helper.Cmveprtipoestimador, DbType.String, entity.Cmveprtipoestimador);
            dbProvider.AddInParameter(command, helper.Cmveprtipocorrida, DbType.String, entity.Cmveprtipocorrida);
            dbProvider.AddInParameter(command, helper.Topcodi, DbType.Int32, entity.Topcodi);
            dbProvider.AddInParameter(command, helper.Cmveprversion, DbType.Int32, entity.Cmveprversion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CmVersionprogramaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cmgncorrelativo, DbType.Int32, entity.Cmgncorrelativo);
            dbProvider.AddInParameter(command, helper.Cmveprvalor, DbType.String, entity.Cmveprvalor);
            dbProvider.AddInParameter(command, helper.Cmveprtipoprograma, DbType.String, entity.Cmveprtipoprograma);
            dbProvider.AddInParameter(command, helper.Cmveprtipoestimador, DbType.String, entity.Cmveprtipoestimador);
            dbProvider.AddInParameter(command, helper.Cmveprtipocorrida, DbType.String, entity.Cmveprtipocorrida);
            dbProvider.AddInParameter(command, helper.Topcodi, DbType.Int32, entity.Topcodi);
            dbProvider.AddInParameter(command, helper.Cmveprversion, DbType.Int32, entity.Cmveprversion);
            dbProvider.AddInParameter(command, helper.Cmveprcodi, DbType.Int32, entity.Cmveprcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cmvepr)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cmveprcodi, DbType.Int32, cmvepr);

            dbProvider.ExecuteNonQuery(command);
        }

        public CmVersionprogramaDTO GetById(int cmvepr)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cmgncorrelativo, DbType.Int32, cmvepr);
            CmVersionprogramaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CmVersionprogramaDTO> List()
        {
            List<CmVersionprogramaDTO> entitys = new List<CmVersionprogramaDTO>();
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

        public List<CmVersionprogramaDTO> GetByCriteria()
        {
            List<CmVersionprogramaDTO> entitys = new List<CmVersionprogramaDTO>();
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
