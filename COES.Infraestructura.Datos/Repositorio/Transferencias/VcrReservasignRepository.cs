using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Transferencias;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase de acceso a datos de la tabla VCR_RESERVASIGN
    /// </summary>
    public class VcrReservasignRepository: RepositoryBase, IVcrReservasignRepository
    {
        public VcrReservasignRepository(string strConn): base(strConn)
        {
        }

        VcrReservasignHelper helper = new VcrReservasignHelper();

        public int Save(VcrReservasignDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Vcrasgcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, entity.Vcrecacodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Gruponomb, DbType.String, entity.Gruponomb);
            dbProvider.AddInParameter(command, helper.Vcrasgfecha, DbType.DateTime, entity.Vcrasgfecha);
            dbProvider.AddInParameter(command, helper.Vcrasghorinicio, DbType.DateTime, entity.Vcrasghorinicio);
            dbProvider.AddInParameter(command, helper.Vcrasghorfinal, DbType.DateTime, entity.Vcrasghorfinal);
            dbProvider.AddInParameter(command, helper.Vcrasgreservasign, DbType.Decimal, entity.Vcrasgreservasign);
            dbProvider.AddInParameter(command, helper.Vcrasgtipo, DbType.String, entity.Vcrasgtipo);
            dbProvider.AddInParameter(command, helper.Vcrasgusucreacion, DbType.String, entity.Vcrasgusucreacion);
            dbProvider.AddInParameter(command, helper.Vcrasgfeccreacion, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.Vcrasgreservasignb, DbType.Decimal, entity.Vcrasgreservasignb); 

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(VcrReservasignDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, entity.Vcrecacodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Gruponomb, DbType.String, entity.Gruponomb);
            dbProvider.AddInParameter(command, helper.Vcrasgfecha, DbType.DateTime, entity.Vcrasgfecha);
            dbProvider.AddInParameter(command, helper.Vcrasghorinicio, DbType.DateTime, entity.Vcrasghorinicio);
            dbProvider.AddInParameter(command, helper.Vcrasghorfinal, DbType.DateTime, entity.Vcrasghorfinal);
            dbProvider.AddInParameter(command, helper.Vcrasgreservasign, DbType.Decimal, entity.Vcrasgreservasign);
            dbProvider.AddInParameter(command, helper.Vcrasgtipo, DbType.String, entity.Vcrasgtipo);
            dbProvider.AddInParameter(command, helper.Vcrasgusucreacion, DbType.String, entity.Vcrasgusucreacion);
            dbProvider.AddInParameter(command, helper.Vcrasgfeccreacion, DbType.DateTime, entity.Vcrasgfeccreacion);
            dbProvider.AddInParameter(command, helper.Vcrasgreservasignb, DbType.Decimal, entity.Vcrasgreservasignb);
            dbProvider.AddInParameter(command, helper.Vcrasgcodi, DbType.Int32, entity.Vcrasgcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int vcrecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public VcrReservasignDTO GetById(int vcrasgcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Vcrasgcodi, DbType.Int32, vcrasgcodi);
            VcrReservasignDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VcrReservasignDTO> List()
        {
            List<VcrReservasignDTO> entitys = new List<VcrReservasignDTO>();
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

        public List<VcrReservasignDTO> GetByCriteria()
        {
            List<VcrReservasignDTO> entitys = new List<VcrReservasignDTO>();
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

        public List<VcrReservasignDTO> GetByCriteriaURSDia(int vcrecacodi, int grupocodi, DateTime dFecha)
        {
            List<VcrReservasignDTO> entitys = new List<VcrReservasignDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteriaURSDia);

            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);
            dbProvider.AddInParameter(command, helper.Vcrasgfecha, DbType.DateTime, dFecha);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<VcrReservasignDTO> GetByCriteriaDia(int vcrecacodi, DateTime dFecha)
        {
            List<VcrReservasignDTO> entitys = new List<VcrReservasignDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteriaDia);

            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);
            dbProvider.AddInParameter(command, helper.Vcrasgfecha, DbType.DateTime, dFecha);

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
