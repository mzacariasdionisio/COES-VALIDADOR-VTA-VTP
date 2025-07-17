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
    /// Clase de acceso a datos de la tabla CO_PERIODO_PROG
    /// </summary>
    public class CoPeriodoProgRepository: RepositoryBase, ICoPeriodoProgRepository
    {
        public CoPeriodoProgRepository(string strConn): base(strConn)
        {
        }

        CoPeriodoProgHelper helper = new CoPeriodoProgHelper();

        public int Save(CoPeriodoProgDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Perprgcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Perprgvigencia, DbType.DateTime, entity.Perprgvigencia);
            dbProvider.AddInParameter(command, helper.Perprgvalor, DbType.Decimal, entity.Perprgvalor);
            dbProvider.AddInParameter(command, helper.Perprgestado, DbType.String, entity.Perprgestado);
            dbProvider.AddInParameter(command, helper.Perprgusucreacion, DbType.String, entity.Perprgusucreacion);
            dbProvider.AddInParameter(command, helper.Perprgfeccreacion, DbType.DateTime, entity.Perprgfeccreacion);
            dbProvider.AddInParameter(command, helper.Perprgusumodificacion, DbType.String, entity.Perprgusumodificacion);
            dbProvider.AddInParameter(command, helper.Perprgfecmodificacion, DbType.DateTime, entity.Perprgfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CoPeriodoProgDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            
            dbProvider.AddInParameter(command, helper.Perprgvigencia, DbType.DateTime, entity.Perprgvigencia);
            dbProvider.AddInParameter(command, helper.Perprgvalor, DbType.Decimal, entity.Perprgvalor);
            dbProvider.AddInParameter(command, helper.Perprgestado, DbType.String, entity.Perprgestado);
            dbProvider.AddInParameter(command, helper.Perprgusucreacion, DbType.String, entity.Perprgusucreacion);
            dbProvider.AddInParameter(command, helper.Perprgfeccreacion, DbType.DateTime, entity.Perprgfeccreacion);
            dbProvider.AddInParameter(command, helper.Perprgusumodificacion, DbType.String, entity.Perprgusumodificacion);
            dbProvider.AddInParameter(command, helper.Perprgfecmodificacion, DbType.DateTime, entity.Perprgfecmodificacion);
            dbProvider.AddInParameter(command, helper.Perprgcodi, DbType.Int32, entity.Perprgcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int perprgcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Perprgcodi, DbType.Int32, perprgcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CoPeriodoProgDTO GetById(int perprgcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Perprgcodi, DbType.Int32, perprgcodi);
            CoPeriodoProgDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CoPeriodoProgDTO> List()
        {
            List<CoPeriodoProgDTO> entitys = new List<CoPeriodoProgDTO>();
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

        public List<CoPeriodoProgDTO> GetByCriteria()
        {
            List<CoPeriodoProgDTO> entitys = new List<CoPeriodoProgDTO>();
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

        public CoPeriodoProgDTO ObtenerPeriodoProgVigente(DateTime fecha)
        {
            string sql = string.Format(helper.SqlObtenerPeriodoVigente, fecha.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            CoPeriodoProgDTO entity = null;

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
