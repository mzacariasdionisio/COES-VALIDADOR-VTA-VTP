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
    /// Clase de acceso a datos de la tabla REP_VCOM
    /// </summary>
    public class RepVcomRepository : RepositoryBase, IRepVcomRepository
    {
        public RepVcomRepository(string strConn) : base(strConn)
        {
        }

        RepVcomHelper helper = new RepVcomHelper();

        public void Save(RepVcomDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Periodo, DbType.Int32, entity.Periodo);
            dbProvider.AddInParameter(command, helper.Codigomodooperacion, DbType.String, entity.Codigomodooperacion);
            dbProvider.AddInParameter(command, helper.Codigotipocombustible, DbType.String, entity.Codigotipocombustible);
            dbProvider.AddInParameter(command, helper.Valor, DbType.Decimal, entity.Valor);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(RepVcomDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Periodo, DbType.Int32, entity.Periodo);
            dbProvider.AddInParameter(command, helper.Codigomodooperacion, DbType.String, entity.Codigomodooperacion);
            dbProvider.AddInParameter(command, helper.Codigotipocombustible, DbType.String, entity.Codigotipocombustible);
            dbProvider.AddInParameter(command, helper.Valor, DbType.Decimal, entity.Valor);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int periodo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Periodo, DbType.Int32, periodo);

            dbProvider.ExecuteNonQuery(command);
        }

        public RepVcomDTO GetById(int periodo, string codigomodooperacion, string codigotipocombustible)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Periodo, DbType.Int32, periodo);
            dbProvider.AddInParameter(command, helper.Codigomodooperacion, DbType.String, codigomodooperacion);
            dbProvider.AddInParameter(command, helper.Codigotipocombustible, DbType.String, codigotipocombustible);
            RepVcomDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<RepVcomDTO> List()
        {
            List<RepVcomDTO> entitys = new List<RepVcomDTO>();
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

        public List<RepVcomDTO> GetByCriteria()
        {
            List<RepVcomDTO> entitys = new List<RepVcomDTO>();
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
