using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;
using Microsoft.SqlServer.Server;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla PR_RESTRIC_CFG
    /// </summary>
    public class PrRestricCfgRepository: RepositoryBase, IPrRestricCfgRepository
    {
        public PrRestricCfgRepository(string strConn): base(strConn)
        {
        }

        PrRestricCfgHelper helper = new PrRestricCfgHelper();

        public int Save(PrRestricCfgDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Rescfgcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Rescfgnombre, DbType.String, entity.Rescfgnombre);
            dbProvider.AddInParameter(command, helper.Rescfgusucreacion, DbType.String, entity.Rescfgusucreacion);
            dbProvider.AddInParameter(command, helper.Rescfgfeccreacion, DbType.DateTime, entity.Rescfgfeccreacion);
            dbProvider.AddInParameter(command, helper.Rescfgusumodificacion, DbType.String, entity.Rescfgusumodificacion);
            dbProvider.AddInParameter(command, helper.Rescfgestado, DbType.String, entity.Rescfgestado);
            dbProvider.AddInParameter(command, helper.Restipcodi, DbType.Int32, entity.Restipcodi);
            dbProvider.AddInParameter(command, helper.Rescfgfecmodificacion, DbType.DateTime, entity.Rescfgfecmodificacion);
            dbProvider.AddInParameter(command, helper.Rescfgtipoecuacion, DbType.String, entity.Rescfgtipoecuacion);
            dbProvider.AddInParameter(command, helper.Rescfges, DbType.Decimal, entity.Rescfges);
            dbProvider.AddInParameter(command, helper.Rescfgfs, DbType.Decimal, entity.Rescfgfs);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PrRestricCfgDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Rescfgnombre, DbType.String, entity.Rescfgnombre);
            dbProvider.AddInParameter(command, helper.Rescfgusucreacion, DbType.String, entity.Rescfgusucreacion);
            dbProvider.AddInParameter(command, helper.Rescfgfeccreacion, DbType.DateTime, entity.Rescfgfeccreacion);
            dbProvider.AddInParameter(command, helper.Rescfgusumodificacion, DbType.String, entity.Rescfgusumodificacion);
            dbProvider.AddInParameter(command, helper.Rescfgestado, DbType.String, entity.Rescfgestado);
            dbProvider.AddInParameter(command, helper.Restipcodi, DbType.Int32, entity.Restipcodi);
            dbProvider.AddInParameter(command, helper.Rescfgfecmodificacion, DbType.DateTime, entity.Rescfgfecmodificacion);
            dbProvider.AddInParameter(command, helper.Rescfgtipoecuacion, DbType.String, entity.Rescfgtipoecuacion);
            dbProvider.AddInParameter(command, helper.Rescfges, DbType.Decimal, entity.Rescfges);
            dbProvider.AddInParameter(command, helper.Rescfgfs, DbType.Decimal, entity.Rescfgfs);
            dbProvider.AddInParameter(command, helper.Rescfgcodi, DbType.Int32, entity.Rescfgcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int rescfgcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Rescfgcodi, DbType.Int32, rescfgcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PrRestricCfgDTO GetById(int rescfgcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Rescfgcodi, DbType.Int32, rescfgcodi);
            PrRestricCfgDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PrRestricCfgDTO> List()
        {
            List<PrRestricCfgDTO> entitys = new List<PrRestricCfgDTO>();
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

        public List<PrRestricCfgDTO> GetByCriteria(string tipos)
        {
            List<PrRestricCfgDTO> entitys = new List<PrRestricCfgDTO>();
            string query = string.Format(helper.SqlGetByCriteria, tipos);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public void ActualizarEstadoRegistro(PrRestricCfgDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlActualizarEstadoRegistro);

            dbProvider.AddInParameter(command, helper.Rescfgestado, DbType.String, entity.Rescfgestado);
            dbProvider.AddInParameter(command, helper.Rescfgusumodificacion, DbType.String, entity.Rescfgusumodificacion);
            dbProvider.AddInParameter(command, helper.Rescfgfecmodificacion, DbType.DateTime, entity.Rescfgfecmodificacion);
            dbProvider.AddInParameter(command, helper.Rescfgcodi, DbType.Int32, entity.Rescfgcodi);

            dbProvider.ExecuteNonQuery(command);
        }

    }
}
