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
    /// Clase de acceso a datos de la tabla EVE_EVENTO_LOG
    /// </summary>
    public class EveEventoLogRepository: RepositoryBase, IEveEventoLogRepository
    {
        public EveEventoLogRepository(string strConn): base(strConn)
        {
        }

        EveEventoLogHelper helper = new EveEventoLogHelper();

        public int Save(EveEventoLogDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Evelogcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, entity.Evencodi);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);           
            dbProvider.AddInParameter(command, helper.Desoperacion, DbType.String, entity.Desoperacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(EveEventoLogDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Evelogcodi, DbType.Int32, entity.Evelogcodi);
            dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, entity.Evencodi);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Desoperacion, DbType.String, entity.Desoperacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int evelogcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Evelogcodi, DbType.Int32, evelogcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public EveEventoLogDTO GetById(int evelogcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Evelogcodi, DbType.Int32, evelogcodi);
            EveEventoLogDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<EveEventoLogDTO> List(int idEvento)
        {
            List<EveEventoLogDTO> entitys = new List<EveEventoLogDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, idEvento);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<EveEventoLogDTO> GetByCriteria()
        {
            List<EveEventoLogDTO> entitys = new List<EveEventoLogDTO>();
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
