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
    /// Clase de acceso a datos de la tabla IEE_RECENERGETICO_HIST
    /// </summary>
    public class IeeRecenergeticoHistRepository : RepositoryBase, IIeeRecenergeticoHistRepository
    {
        public IeeRecenergeticoHistRepository(string strConn) : base(strConn)
        {
        }

        IeeRecenergeticoHistHelper helper = new IeeRecenergeticoHistHelper();

        public int Save(IeeRecenergeticoHistDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Renerhcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Renerhfecha, DbType.DateTime, entity.Renerhfecha);
            dbProvider.AddInParameter(command, helper.Renerhvalor, DbType.Decimal, entity.Renerhvalor);
            dbProvider.AddInParameter(command, helper.Renertipcodi, DbType.Int32, entity.Renertipcodi);
            dbProvider.AddInParameter(command, helper.Renerhusumodificacion, DbType.String, entity.Renerhusumodificacion);
            dbProvider.AddInParameter(command, helper.Renerhfecmodificacion, DbType.DateTime, entity.Renerhfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(IeeRecenergeticoHistDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Renerhcodi, DbType.Int32, entity.Renerhcodi);
            dbProvider.AddInParameter(command, helper.Renerhfecha, DbType.DateTime, entity.Renerhfecha);
            dbProvider.AddInParameter(command, helper.Renerhvalor, DbType.Int32, entity.Renerhvalor);
            dbProvider.AddInParameter(command, helper.Renertipcodi, DbType.Int32, entity.Renertipcodi);
            dbProvider.AddInParameter(command, helper.Renerhusumodificacion, DbType.String, entity.Renerhusumodificacion);
            dbProvider.AddInParameter(command, helper.Renerhfecmodificacion, DbType.DateTime, entity.Renerhfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int renercodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Renerhcodi, DbType.Int32, renercodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public IeeRecenergeticoHistDTO GetById(int renercodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Renerhcodi, DbType.Int32, renercodi);
            IeeRecenergeticoHistDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<IeeRecenergeticoHistDTO> List()
        {
            List<IeeRecenergeticoHistDTO> entitys = new List<IeeRecenergeticoHistDTO>();
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

        public List<IeeRecenergeticoHistDTO> GetByCriteria(DateTime fechaInicio, DateTime fechaFin)
        {
            List<IeeRecenergeticoHistDTO> entitys = new List<IeeRecenergeticoHistDTO>();
            var query = string.Format(helper.SqlGetByCriteria, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iRenertipnomb = dr.GetOrdinal(helper.Renertipnomb);
                    if (!dr.IsDBNull(iRenertipnomb)) entity.Renertipnomb = dr.GetString(iRenertipnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
