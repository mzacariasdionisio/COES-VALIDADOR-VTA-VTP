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
    /// Clase de acceso a datos de la tabla CB_FICHA
    /// </summary>
    public class CbFichaRepository : RepositoryBase, ICbFichaRepository
    {
        public CbFichaRepository(string strConn) : base(strConn)
        {
        }

        CbFichaHelper helper = new CbFichaHelper();

        public int Save(CbFichaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cbftcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cbftnombre, DbType.String, entity.Cbftnombre);
            dbProvider.AddInParameter(command, helper.Cbftfechavigencia, DbType.DateTime, entity.Cbftfechavigencia);
            dbProvider.AddInParameter(command, helper.Cbftusucreacion, DbType.String, entity.Cbftusucreacion);
            dbProvider.AddInParameter(command, helper.Cbftfeccreacion, DbType.DateTime, entity.Cbftfeccreacion);
            dbProvider.AddInParameter(command, helper.Cbftusumodificacion, DbType.String, entity.Cbftusumodificacion);
            dbProvider.AddInParameter(command, helper.Cbftfecmodificacion, DbType.DateTime, entity.Cbftfecmodificacion);
            dbProvider.AddInParameter(command, helper.Cbftactivo, DbType.Int32, entity.Cbftactivo);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CbFichaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cbftnombre, DbType.String, entity.Cbftnombre);
            dbProvider.AddInParameter(command, helper.Cbftfechavigencia, DbType.DateTime, entity.Cbftfechavigencia);
            dbProvider.AddInParameter(command, helper.Cbftusucreacion, DbType.String, entity.Cbftusucreacion);
            dbProvider.AddInParameter(command, helper.Cbftfeccreacion, DbType.DateTime, entity.Cbftfeccreacion);
            dbProvider.AddInParameter(command, helper.Cbftusumodificacion, DbType.String, entity.Cbftusumodificacion);
            dbProvider.AddInParameter(command, helper.Cbftfecmodificacion, DbType.DateTime, entity.Cbftfecmodificacion);
            dbProvider.AddInParameter(command, helper.Cbftactivo, DbType.Int32, entity.Cbftactivo);

            dbProvider.AddInParameter(command, helper.Cbftcodi, DbType.Int32, entity.Cbftcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CbFichaDTO GetById(int cbftcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cbftcodi, DbType.Int32, cbftcodi);
            CbFichaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CbFichaDTO> List()
        {
            List<CbFichaDTO> entitys = new List<CbFichaDTO>();
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

        public List<CbFichaDTO> GetByCriteria()
        {
            List<CbFichaDTO> entitys = new List<CbFichaDTO>();
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
