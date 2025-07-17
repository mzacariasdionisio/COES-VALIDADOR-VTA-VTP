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
    /// Clase de acceso a datos de la tabla IND_CUADRO3
    /// </summary>
    public class IndCuadro3Repository : RepositoryBase, IIndCuadro3Repository
    {
        public IndCuadro3Repository(string strConn)
            : base(strConn)
        {
        }

        IndCuadro3Helper helper = new IndCuadro3Helper();

        public int Save(IndCuadro3DTO entity)
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            dbProvider.AddInParameter(command, helper.Cuadr3codi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cuadr3potlimite, DbType.Decimal, entity.Cuadr3potlimite);
            dbProvider.AddInParameter(command, helper.Cuadr3despotlimite, DbType.String, entity.Cuadr3despotlimite);
            dbProvider.AddInParameter(command, helper.Cuadr3usumodificacion, DbType.String, entity.Cuadr3usumodificacion);
            dbProvider.AddInParameter(command, helper.Cuadr3fecmodificacion, DbType.DateTime, entity.Cuadr3fecmodificacion);
            dbProvider.AddInParameter(command, helper.Cuadr3Electrico, DbType.String, entity.Cuadr3Electrico);
            dbProvider.ExecuteNonQuery(command);
            return id;

        }

        public void Update(IndCuadro3DTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cuadr3potlimite, DbType.Decimal, entity.Cuadr3potlimite);
            dbProvider.AddInParameter(command, helper.Cuadr3despotlimite, DbType.String, entity.Cuadr3despotlimite);
            dbProvider.AddInParameter(command, helper.Cuadr3usumodificacion, DbType.String, entity.Cuadr3usumodificacion);
            dbProvider.AddInParameter(command, helper.Cuadr3fecmodificacion, DbType.DateTime, entity.Cuadr3fecmodificacion);
            dbProvider.AddInParameter(command, helper.Cuadr3Electrico, DbType.String, entity.Cuadr3Electrico);
            dbProvider.AddInParameter(command, helper.Cuadr3codi, DbType.Int32, entity.Cuadr3codi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cuadr3codi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cuadr3codi, DbType.Int32, cuadr3codi);

            dbProvider.ExecuteNonQuery(command);
        }

        public IndCuadro3DTO GetById(int cuadr3codi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cuadr3codi, DbType.Int32, cuadr3codi);
            IndCuadro3DTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<IndCuadro3DTO> List()
        {
            List<IndCuadro3DTO> entitys = new List<IndCuadro3DTO>();
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

        public List<IndCuadro3DTO> GetByCriteria()
        {
            List<IndCuadro3DTO> entitys = new List<IndCuadro3DTO>();
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
