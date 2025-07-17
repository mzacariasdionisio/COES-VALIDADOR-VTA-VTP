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
    /// Clase de acceso a datos de la tabla WB_CONVOCATORIAS
    /// </summary>
    public class WbConvocatoriasRepository : RepositoryBase, IWbConvocatoriasRepository
    {
        public WbConvocatoriasRepository(string strConn) : base(strConn)
        {
        }

        WbConvocatoriasHelper helper = new WbConvocatoriasHelper();

        public int Save(WbConvocatoriasDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Convcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Convabrev, DbType.String, entity.Convabrev);
            dbProvider.AddInParameter(command, helper.Convnomb, DbType.String, entity.Convnomb);
            dbProvider.AddInParameter(command, helper.Convdesc, DbType.String, entity.Convdesc);
            dbProvider.AddInParameter(command, helper.Convlink, DbType.String, entity.Convlink);
            dbProvider.AddInParameter(command, helper.Convfechaini, DbType.DateTime, entity.Convfechaini);
            dbProvider.AddInParameter(command, helper.Convfechafin, DbType.DateTime, entity.Convfechafin);
            dbProvider.AddInParameter(command, helper.Convestado, DbType.String, entity.Convestado);
            dbProvider.AddInParameter(command, helper.Usercreacion, DbType.String, entity.Usercreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(WbConvocatoriasDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Convabrev, DbType.String, entity.Convabrev);
            dbProvider.AddInParameter(command, helper.Convnomb, DbType.String, entity.Convnomb);
            dbProvider.AddInParameter(command, helper.Convdesc, DbType.String, entity.Convdesc);
            dbProvider.AddInParameter(command, helper.Convlink, DbType.String, entity.Convlink);
            dbProvider.AddInParameter(command, helper.Convfechaini, DbType.DateTime, entity.Convfechaini);
            dbProvider.AddInParameter(command, helper.Convfechafin, DbType.DateTime, entity.Convfechafin);
            dbProvider.AddInParameter(command, helper.Convestado, DbType.String, entity.Convestado);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);

            dbProvider.AddInParameter(command, helper.Convcodi, DbType.Int32, entity.Convcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int convcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Convcodi, DbType.Int32, convcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public WbConvocatoriasDTO GetById(int convcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Convcodi, DbType.Int32, convcodi);
            WbConvocatoriasDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<WbConvocatoriasDTO> List()
        {
            List<WbConvocatoriasDTO> entitys = new List<WbConvocatoriasDTO>();
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

        public List<WbConvocatoriasDTO> GetByCriteria()
        {
            List<WbConvocatoriasDTO> entitys = new List<WbConvocatoriasDTO>();
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
