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
    /// Clase de acceso a datos de la tabla WB_CMVSTARIFA
    /// </summary>
    public class WbCmvstarifaRepository : RepositoryBase, IWbCmvstarifaRepository
    {
        public WbCmvstarifaRepository(string strConn) : base(strConn)
        {
        }

        WbCmvstarifaHelper helper = new WbCmvstarifaHelper();

        public int Save(WbCmvstarifaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cmtarcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cmtarcmprom, DbType.Decimal, entity.Cmtarcmprom);
            dbProvider.AddInParameter(command, helper.Cmtartarifabarra, DbType.Decimal, entity.Cmtartarifabarra);
            dbProvider.AddInParameter(command, helper.Cmtarprommovil, DbType.Decimal, entity.Cmtarprommovil);
            dbProvider.AddInParameter(command, helper.Cmtarfecha, DbType.DateTime, entity.Cmtarfecha);
            dbProvider.AddInParameter(command, helper.Cmtarusucreacion, DbType.String, entity.Cmtarusucreacion);
            dbProvider.AddInParameter(command, helper.Cmtarusumodificacion, DbType.String, entity.Cmtarusumodificacion);
            dbProvider.AddInParameter(command, helper.Cmtarfeccreacion, DbType.DateTime, entity.Cmtarfeccreacion);
            dbProvider.AddInParameter(command, helper.Cmtarfecmodificacion, DbType.DateTime, entity.Cmtarfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(WbCmvstarifaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cmtarcodi, DbType.Int32, entity.Cmtarcodi);
            dbProvider.AddInParameter(command, helper.Cmtarcmprom, DbType.Decimal, entity.Cmtarcmprom);
            dbProvider.AddInParameter(command, helper.Cmtartarifabarra, DbType.Decimal, entity.Cmtartarifabarra);
            dbProvider.AddInParameter(command, helper.Cmtarprommovil, DbType.Decimal, entity.Cmtarprommovil);
            dbProvider.AddInParameter(command, helper.Cmtarfecha, DbType.DateTime, entity.Cmtarfecha);
            dbProvider.AddInParameter(command, helper.Cmtarusucreacion, DbType.String, entity.Cmtarusucreacion);
            dbProvider.AddInParameter(command, helper.Cmtarusumodificacion, DbType.String, entity.Cmtarusumodificacion);
            dbProvider.AddInParameter(command, helper.Cmtarfeccreacion, DbType.DateTime, entity.Cmtarfeccreacion);
            dbProvider.AddInParameter(command, helper.Cmtarfecmodificacion, DbType.DateTime, entity.Cmtarfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cmtarcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cmtarcodi, DbType.Int32, cmtarcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public WbCmvstarifaDTO GetById(int cmtarcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cmtarcodi, DbType.Int32, cmtarcodi);
            WbCmvstarifaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<WbCmvstarifaDTO> List()
        {
            List<WbCmvstarifaDTO> entitys = new List<WbCmvstarifaDTO>();
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

        public List<WbCmvstarifaDTO> GetByCriteria(DateTime fechaInicio, DateTime fechaFin)
        {
            List<WbCmvstarifaDTO> entitys = new List<WbCmvstarifaDTO>();
            var query = string.Format(helper.SqlGetByCriteria, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));
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