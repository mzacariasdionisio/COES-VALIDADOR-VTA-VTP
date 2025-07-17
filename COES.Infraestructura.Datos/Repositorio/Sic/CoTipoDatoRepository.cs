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
    /// Clase de acceso a datos de la tabla CO_TIPO_DATO
    /// </summary>
    public class CoTipoDatoRepository: RepositoryBase, ICoTipoDatoRepository
    {
        public CoTipoDatoRepository(string strConn): base(strConn)
        {
        }

        CoTipoDatoHelper helper = new CoTipoDatoHelper();

        public int Save(CoTipoDatoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cotidacodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cotidausumodificacion, DbType.String, entity.Cotidausumodificacion);
            dbProvider.AddInParameter(command, helper.Cotidafecmodificacion, DbType.DateTime, entity.Cotidafecmodificacion);
            dbProvider.AddInParameter(command, helper.Cotidaindicador, DbType.String, entity.Cotidaindicador);
            dbProvider.AddInParameter(command, helper.Cotidausucreacion, DbType.String, entity.Cotidausucreacion);
            dbProvider.AddInParameter(command, helper.Cotidafeccreacion, DbType.DateTime, entity.Cotidafeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CoTipoDatoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cotidausumodificacion, DbType.String, entity.Cotidausumodificacion);
            dbProvider.AddInParameter(command, helper.Cotidafecmodificacion, DbType.DateTime, entity.Cotidafecmodificacion);
            dbProvider.AddInParameter(command, helper.Cotidacodi, DbType.Int32, entity.Cotidacodi);
            dbProvider.AddInParameter(command, helper.Cotidaindicador, DbType.String, entity.Cotidaindicador);
            dbProvider.AddInParameter(command, helper.Cotidausucreacion, DbType.String, entity.Cotidausucreacion);
            dbProvider.AddInParameter(command, helper.Cotidafeccreacion, DbType.DateTime, entity.Cotidafeccreacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cotidacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cotidacodi, DbType.Int32, cotidacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CoTipoDatoDTO GetById(int cotidacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cotidacodi, DbType.Int32, cotidacodi);
            CoTipoDatoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CoTipoDatoDTO> List()
        {
            List<CoTipoDatoDTO> entitys = new List<CoTipoDatoDTO>();
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

        public List<CoTipoDatoDTO> GetByCriteria(string tipoDato)
        {
            List<CoTipoDatoDTO> entitys = new List<CoTipoDatoDTO>();
            string sql = string.Format(helper.SqlGetByCriteria, tipoDato);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

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
