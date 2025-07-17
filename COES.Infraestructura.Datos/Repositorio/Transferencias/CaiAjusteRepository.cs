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
    /// Clase de acceso a datos de la tabla CAI_AJUSTE
    /// </summary>
    public class CaiAjusteRepository: RepositoryBase, ICaiAjusteRepository
    {
        public CaiAjusteRepository(string strConn): base(strConn)
        {
        }

        CaiAjusteHelper helper = new CaiAjusteHelper();

        public int Save(CaiAjusteDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Caiajcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Caiprscodi, DbType.Int32, entity.Caiprscodi);
            dbProvider.AddInParameter(command, helper.Caiajanio, DbType.Int32, entity.Caiajanio);
            dbProvider.AddInParameter(command, helper.Caiajmes, DbType.Int32, entity.Caiajmes);
            dbProvider.AddInParameter(command, helper.Caiajnombre, DbType.String, entity.Caiajnombre);
            dbProvider.AddInParameter(command, helper.Caiajusucreacion, DbType.String, entity.Caiajusucreacion);
            dbProvider.AddInParameter(command, helper.Caiajfeccreacion, DbType.DateTime, entity.Caiajfeccreacion);
            dbProvider.AddInParameter(command, helper.Caiajusumodificacion, DbType.String, entity.Caiajusumodificacion);
            dbProvider.AddInParameter(command, helper.Caiajfecmodificacion, DbType.DateTime, entity.Caiajfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CaiAjusteDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Caiprscodi, DbType.Int32, entity.Caiprscodi);
            dbProvider.AddInParameter(command, helper.Caiajanio, DbType.Int32, entity.Caiajanio);
            dbProvider.AddInParameter(command, helper.Caiajmes, DbType.Int32, entity.Caiajmes);
            dbProvider.AddInParameter(command, helper.Caiajnombre, DbType.String, entity.Caiajnombre);
            dbProvider.AddInParameter(command, helper.Caiajusumodificacion, DbType.String, entity.Caiajusumodificacion);
            dbProvider.AddInParameter(command, helper.Caiajfecmodificacion, DbType.DateTime, entity.Caiajfecmodificacion);
            dbProvider.AddInParameter(command, helper.Caiajcodi, DbType.Int32, entity.Caiajcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int caiajcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Caiajcodi, DbType.Int32, caiajcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CaiAjusteDTO GetById(int caiajcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Caiajcodi, DbType.Int32, caiajcodi);
            CaiAjusteDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public CaiAjusteDTO GetByIdAnterior(int caiajcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdAnterior);

            dbProvider.AddInParameter(command, helper.Caiajcodi, DbType.Int32, caiajcodi);
            CaiAjusteDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CaiAjusteDTO> List(int caiprscodi)
        {
            List<CaiAjusteDTO> entitys = new List<CaiAjusteDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, helper.Caiprscodi, DbType.Int32, caiprscodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<CaiAjusteDTO> GetByCriteria()
        {
            List<CaiAjusteDTO> entitys = new List<CaiAjusteDTO>();
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
        /*-------------------------------------------------------------------------------------*/
        public List<CaiAjusteDTO> ListByCaiPrscodi(int caiprscodi)
        {
            List<CaiAjusteDTO> entitys = new List<CaiAjusteDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListByCaiPrscodi);
            dbProvider.AddInParameter(command,helper.Caiprscodi,DbType.Int32,caiprscodi);

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
