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
    /// Clase de acceso a datos de la tabla PR_RESTRIC_DATO
    /// </summary>
    public class PrRestricDatoRepository: RepositoryBase, IPrRestricDatoRepository
    {
        public PrRestricDatoRepository(string strConn): base(strConn)
        {
        }

        PrRestricDatoHelper helper = new PrRestricDatoHelper();

        public int Save(PrRestricDatoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Resdatcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Rescfgcodi, DbType.Int32, entity.Rescfgcodi);
            dbProvider.AddInParameter(command, helper.Resdatdato, DbType.String, entity.Resdatdato);
            dbProvider.AddInParameter(command, helper.Resdatflag, DbType.String, entity.Resdatflag);
            dbProvider.AddInParameter(command, helper.Resdatfechaini, DbType.DateTime, entity.Resdatfechaini);
            dbProvider.AddInParameter(command, helper.Resenvcodi, DbType.Int32, entity.Resenvcodi);
            dbProvider.AddInParameter(command, helper.Resdatfechafin, DbType.DateTime, entity.Resdatfechafin);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PrRestricDatoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Rescfgcodi, DbType.Int32, entity.Rescfgcodi);
            dbProvider.AddInParameter(command, helper.Resdatdato, DbType.String, entity.Resdatdato);
            dbProvider.AddInParameter(command, helper.Resdatflag, DbType.String, entity.Resdatflag);
            dbProvider.AddInParameter(command, helper.Resdatfechaini, DbType.DateTime, entity.Resdatfechaini);
            dbProvider.AddInParameter(command, helper.Resenvcodi, DbType.Int32, entity.Resenvcodi);
            dbProvider.AddInParameter(command, helper.Resdatfechafin, DbType.DateTime, entity.Resdatfechafin);
            dbProvider.AddInParameter(command, helper.Resdatcodi, DbType.Int32, entity.Resdatcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int resdatcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Resdatcodi, DbType.Int32, resdatcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PrRestricDatoDTO GetById(int resdatcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Resdatcodi, DbType.Int32, resdatcodi);
            PrRestricDatoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PrRestricDatoDTO> List()
        {
            List<PrRestricDatoDTO> entitys = new List<PrRestricDatoDTO>();
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

        public List<PrRestricDatoDTO> GetByCriteria()
        {
            List<PrRestricDatoDTO> entitys = new List<PrRestricDatoDTO>();
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

        public List<PrRestricDatoDTO> ObtenerDataEnvio(DateTime fechaPeriodo, string codigos, int formato)
        {
            List<PrRestricDatoDTO> entitys = new List<PrRestricDatoDTO>();
            string query = string.Format(helper.SqlObtenerDataEnvio, fechaPeriodo.ToString(ConstantesBase.FormatoFecha), codigos, formato);
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

        public List<PrRestricDatoDTO> ObtenerPorEnvio(int idEnvio)
        {
            List<PrRestricDatoDTO> entitys = new List<PrRestricDatoDTO>();
            string query = string.Format(helper.SqlObtenerPorEnvio, idEnvio);
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
