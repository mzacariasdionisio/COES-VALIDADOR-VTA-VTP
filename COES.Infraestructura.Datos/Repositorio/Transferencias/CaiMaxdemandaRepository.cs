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
    /// Clase de acceso a datos de la tabla CAI_MAXDEMANDA
    /// </summary>
    public class CaiMaxdemandaRepository: RepositoryBase, ICaiMaxdemandaRepository
    {
        public CaiMaxdemandaRepository(string strConn): base(strConn)
        {
        }

        CaiMaxdemandaHelper helper = new CaiMaxdemandaHelper();

        public int Save(CaiMaxdemandaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Caimdecodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Caiajcodi, DbType.Int32, entity.Caiajcodi);
            dbProvider.AddInParameter(command, helper.Caimdeaniomes, DbType.Int32, entity.Caimdeaniomes);
            dbProvider.AddInParameter(command, helper.Caimdefechor, DbType.DateTime, entity.Caimdefechor);
            dbProvider.AddInParameter(command, helper.Caimdetipoinfo, DbType.String, entity.Caimdetipoinfo);
            dbProvider.AddInParameter(command, helper.Caimdemaxdemanda, DbType.Decimal, entity.Caimdemaxdemanda);
            dbProvider.AddInParameter(command, helper.Caimdeusucreacion, DbType.String, entity.Caimdeusucreacion);
            dbProvider.AddInParameter(command, helper.Caimdefeccreacion, DbType.DateTime, entity.Caimdefeccreacion);
            dbProvider.AddInParameter(command, helper.Caimdeusumodificacion, DbType.String, entity.Caimdeusumodificacion);
            dbProvider.AddInParameter(command, helper.Caimdefecmodificacion, DbType.DateTime, entity.Caimdefecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CaiMaxdemandaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Caiajcodi, DbType.Int32, entity.Caiajcodi);
            dbProvider.AddInParameter(command, helper.Caimdeaniomes, DbType.Int32, entity.Caimdeaniomes);
            dbProvider.AddInParameter(command, helper.Caimdefechor, DbType.DateTime, entity.Caimdefechor);
            dbProvider.AddInParameter(command, helper.Caimdetipoinfo, DbType.String, entity.Caimdetipoinfo);
            dbProvider.AddInParameter(command, helper.Caimdemaxdemanda, DbType.Decimal, entity.Caimdemaxdemanda);
            dbProvider.AddInParameter(command, helper.Caimdeusumodificacion, DbType.String, entity.Caimdeusumodificacion);
            dbProvider.AddInParameter(command, helper.Caimdefecmodificacion, DbType.DateTime, entity.Caimdefecmodificacion);
            dbProvider.AddInParameter(command, helper.Caimdecodi, DbType.Int32, entity.Caimdecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int caimdecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Caimdecodi, DbType.Int32, caimdecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteEjecutada(int caiajcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteEjecutada);

            dbProvider.AddInParameter(command, helper.Caiajcodi, DbType.Int32, caiajcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CaiMaxdemandaDTO GetById(int caimdecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Caimdecodi, DbType.Int32, caimdecodi);
            CaiMaxdemandaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CaiMaxdemandaDTO> List(int caiajcodi)
        {
            List<CaiMaxdemandaDTO> entitys = new List<CaiMaxdemandaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, helper.Caiajcodi, DbType.Int32, caiajcodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<CaiMaxdemandaDTO> GetByCriteria()
        {
            List<CaiMaxdemandaDTO> entitys = new List<CaiMaxdemandaDTO>();
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

        public CaiMaxdemandaDTO GetByCaimdeAnioMes(int caiajcodi, int caimdeaniomes)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCaimdeAnioMes);
            dbProvider.AddInParameter(command, helper.Caiajcodi, DbType.Int32, caiajcodi);
            dbProvider.AddInParameter(command, helper.Caimdeaniomes, DbType.Int32, caimdeaniomes);
            CaiMaxdemandaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }
    }
}
