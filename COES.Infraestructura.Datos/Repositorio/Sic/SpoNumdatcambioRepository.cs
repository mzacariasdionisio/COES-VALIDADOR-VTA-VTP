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
    /// Clase de acceso a datos de la tabla SPO_NumdcbCAMBIO
    /// </summary>
    public class SpoNumdatcambioRepository: RepositoryBase, ISpoNumdatcambioRepository
    {
        public SpoNumdatcambioRepository(string strConn): base(strConn)
        {
        }

        SpoNumdatcambioHelper helper = new SpoNumdatcambioHelper();

        public int Save(SpoNumdatcambioDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Numdcbcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Verncodi, DbType.Int32, entity.Verncodi);
            dbProvider.AddInParameter(command, helper.Sconcodi, DbType.Int32, entity.Sconcodi);
            dbProvider.AddInParameter(command, helper.Clasicodi, DbType.Int32, entity.Clasicodi);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, entity.Tipoinfocodi);
            dbProvider.AddInParameter(command, helper.Numdcbvalor, DbType.Decimal, entity.Numdcbvalor);
            dbProvider.AddInParameter(command, helper.Numdcbfechainicio, DbType.DateTime, entity.Numdcbfechainicio);
            dbProvider.AddInParameter(command, helper.Numdcbfechafin, DbType.DateTime, entity.Numdcbfechafin);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SpoNumdatcambioDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Numdcbcodi, DbType.Int32, entity.Numdcbcodi);
            dbProvider.AddInParameter(command, helper.Verncodi, DbType.Int32, entity.Verncodi);
            dbProvider.AddInParameter(command, helper.Sconcodi, DbType.Int32, entity.Sconcodi);
            dbProvider.AddInParameter(command, helper.Clasicodi, DbType.Int32, entity.Clasicodi);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, entity.Tipoinfocodi);
            dbProvider.AddInParameter(command, helper.Numdcbvalor, DbType.Decimal, entity.Numdcbvalor);
            dbProvider.AddInParameter(command, helper.Numdcbfechainicio, DbType.DateTime, entity.Numdcbfechainicio);
            dbProvider.AddInParameter(command, helper.Numdcbfechafin, DbType.DateTime, entity.Numdcbfechafin);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int numdcbcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Numdcbcodi, DbType.Int32, numdcbcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SpoNumdatcambioDTO GetById(int numdcbcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Numdcbcodi, DbType.Int32, numdcbcodi);
            SpoNumdatcambioDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SpoNumdatcambioDTO> List()
        {
            List<SpoNumdatcambioDTO> entitys = new List<SpoNumdatcambioDTO>();
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

        public List<SpoNumdatcambioDTO> GetByCriteria(int version,int numecodi, DateTime periodo)
        {
            string sqlQuery = string.Format(helper.SqlGetByCriteria, version,numecodi,periodo.ToString(ConstantesBase.FormatoFecha));
            List<SpoNumdatcambioDTO> entitys = new List<SpoNumdatcambioDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

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
