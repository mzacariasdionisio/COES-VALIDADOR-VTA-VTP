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
    /// Clase de acceso a datos de la tabla CO_TIPOINFORMACION
    /// </summary>
    public class CoTipoinformacionRepository: RepositoryBase, ICoTipoinformacionRepository
    {
        public CoTipoinformacionRepository(string strConn): base(strConn)
        {
        }

        CoTipoinformacionHelper helper = new CoTipoinformacionHelper();

        public int Save(CoTipoinformacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cotinfcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cotinfestado, DbType.String, entity.Cotinfestado);
            dbProvider.AddInParameter(command, helper.Cotinfusucreacion, DbType.String, entity.Cotinfusucreacion);
            dbProvider.AddInParameter(command, helper.Cotinffeccreacion, DbType.DateTime, entity.Cotinffeccreacion);
            dbProvider.AddInParameter(command, helper.Cotinfusumodificacion, DbType.String, entity.Cotinfusumodificacion);
            dbProvider.AddInParameter(command, helper.Cotinffecmodificacion, DbType.DateTime, entity.Cotinffecmodificacion);
            dbProvider.AddInParameter(command, helper.Cotinfdesc, DbType.String, entity.Cotinfdesc);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CoTipoinformacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cotinfestado, DbType.String, entity.Cotinfestado);
            dbProvider.AddInParameter(command, helper.Cotinfusucreacion, DbType.String, entity.Cotinfusucreacion);
            dbProvider.AddInParameter(command, helper.Cotinffeccreacion, DbType.DateTime, entity.Cotinffeccreacion);
            dbProvider.AddInParameter(command, helper.Cotinfusumodificacion, DbType.String, entity.Cotinfusumodificacion);
            dbProvider.AddInParameter(command, helper.Cotinffecmodificacion, DbType.DateTime, entity.Cotinffecmodificacion);
            dbProvider.AddInParameter(command, helper.Cotinfdesc, DbType.String, entity.Cotinfdesc);
            dbProvider.AddInParameter(command, helper.Cotinfcodi, DbType.Int32, entity.Cotinfcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cotinfcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cotinfcodi, DbType.Int32, cotinfcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CoTipoinformacionDTO GetById(int cotinfcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cotinfcodi, DbType.Int32, cotinfcodi);
            CoTipoinformacionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CoTipoinformacionDTO> List()
        {
            List<CoTipoinformacionDTO> entitys = new List<CoTipoinformacionDTO>();
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

        public List<CoTipoinformacionDTO> GetByCriteria()
        {
            List<CoTipoinformacionDTO> entitys = new List<CoTipoinformacionDTO>();
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
