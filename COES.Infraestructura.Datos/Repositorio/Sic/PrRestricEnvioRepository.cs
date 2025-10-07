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
    /// Clase de acceso a datos de la tabla PR_RESTRIC_ENVIO
    /// </summary>
    public class PrRestricEnvioRepository: RepositoryBase, IPrRestricEnvioRepository
    {
        public PrRestricEnvioRepository(string strConn): base(strConn)
        {
        }

        PrRestricEnvioHelper helper = new PrRestricEnvioHelper();

        public int Save(PrRestricEnvioDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Resenvcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Resenvfechaperiodo, DbType.DateTime, entity.Resenvfechaperiodo);
            dbProvider.AddInParameter(command, helper.Resenvusucreacion, DbType.String, entity.Resenvusucreacion);
            dbProvider.AddInParameter(command, helper.Resenvfeccreacion, DbType.DateTime, entity.Resenvfeccreacion);
            dbProvider.AddInParameter(command, helper.Resenvestado, DbType.String, entity.Resenvestado);
            dbProvider.AddInParameter(command, helper.Resfmtcodi, DbType.Int32, entity.Resfmtcodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PrRestricEnvioDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Resenvfechaperiodo, DbType.DateTime, entity.Resenvfechaperiodo);
            dbProvider.AddInParameter(command, helper.Resenvusucreacion, DbType.String, entity.Resenvusucreacion);
            dbProvider.AddInParameter(command, helper.Resenvfeccreacion, DbType.DateTime, entity.Resenvfeccreacion);
            dbProvider.AddInParameter(command, helper.Resenvestado, DbType.String, entity.Resenvestado);
            dbProvider.AddInParameter(command, helper.Resfmtcodi, DbType.Int32, entity.Resfmtcodi);
            dbProvider.AddInParameter(command, helper.Resenvcodi, DbType.Int32, entity.Resenvcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int resenvcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Resenvcodi, DbType.Int32, resenvcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PrRestricEnvioDTO GetById(int resenvcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Resenvcodi, DbType.Int32, resenvcodi);
            PrRestricEnvioDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PrRestricEnvioDTO> List()
        {
            List<PrRestricEnvioDTO> entitys = new List<PrRestricEnvioDTO>();
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

        public List<PrRestricEnvioDTO> GetByCriteria()
        {
            List<PrRestricEnvioDTO> entitys = new List<PrRestricEnvioDTO>();
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

        public List<PrRestricEnvioDTO> ObtenerEnvioActivo(DateTime fechaPeriodo, string codigos, int formato, string estado)
        {
            List<PrRestricEnvioDTO> entitys = new List<PrRestricEnvioDTO>();
            string query = string.Format(helper.SqlObtenerEnvioActivo, fechaPeriodo.ToString(ConstantesBase.FormatoFecha), codigos, formato, estado);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

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
