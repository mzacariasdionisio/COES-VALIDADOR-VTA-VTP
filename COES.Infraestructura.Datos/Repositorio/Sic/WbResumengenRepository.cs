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
    /// Clase de acceso a datos de la tabla WB_RESUMENGEN
    /// </summary>
    public class WbResumengenRepository: RepositoryBase, IWbResumengenRepository
    {
        public WbResumengenRepository(string strConn): base(strConn)
        {
        }

        WbResumengenHelper helper = new WbResumengenHelper();

        public int Save(WbResumengenDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Resgencodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Resgenactual, DbType.Decimal, entity.Resgenactual);
            dbProvider.AddInParameter(command, helper.Resgenanterior, DbType.Decimal, entity.Resgenanterior);
            dbProvider.AddInParameter(command, helper.Resgenvariacion, DbType.Decimal, entity.Resgenvariacion);
            dbProvider.AddInParameter(command, helper.Resgenfecha, DbType.DateTime, entity.Resgenfecha);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(WbResumengenDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Resgenactual, DbType.Decimal, entity.Resgenactual);
            dbProvider.AddInParameter(command, helper.Resgenanterior, DbType.Decimal, entity.Resgenanterior);
            dbProvider.AddInParameter(command, helper.Resgenvariacion, DbType.Decimal, entity.Resgenvariacion);
            dbProvider.AddInParameter(command, helper.Resgenfecha, DbType.DateTime, entity.Resgenfecha);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Resgencodi, DbType.Int32, entity.Resgencodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int resgencodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Resgencodi, DbType.Int32, resgencodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public WbResumengenDTO GetById(int resgencodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Resgencodi, DbType.Int32, resgencodi);
            WbResumengenDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<WbResumengenDTO> List()
        {
            List<WbResumengenDTO> entitys = new List<WbResumengenDTO>();
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

        public WbResumengenDTO GetByCriteria(DateTime fecha)
        {
            WbResumengenDTO entity = null;
            string query = string.Format(helper.SqlGetByCriteria, fecha.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

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
