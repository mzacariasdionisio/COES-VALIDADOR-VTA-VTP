using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla ABI_FACTORPLANTA
    /// </summary>
    public class AbiFactorplantaRepository : RepositoryBase, IAbiFactorplantaRepository
    {
        public AbiFactorplantaRepository(string strConn) : base(strConn)
        {
        }

        AbiFactorplantaHelper helper = new AbiFactorplantaHelper();

        public int Save(AbiFactorplantaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Fpfecmodificacion, DbType.DateTime, entity.Fpfecmodificacion);
            dbProvider.AddInParameter(command, helper.Fpusumodificacion, DbType.String, entity.Fpusumodificacion);
            dbProvider.AddInParameter(command, helper.Fptipogenerrer, DbType.String, entity.Fptipogenerrer);
            dbProvider.AddInParameter(command, helper.Fpintegrante, DbType.String, entity.Fpintegrante);
            dbProvider.AddInParameter(command, helper.Fpenergia, DbType.Decimal, entity.Fpenergia);
            dbProvider.AddInParameter(command, helper.Fppe, DbType.Decimal, entity.Fppe);
            dbProvider.AddInParameter(command, helper.Fpvalormes, DbType.Decimal, entity.Fpvalormes);
            dbProvider.AddInParameter(command, helper.Fpvalor, DbType.Decimal, entity.Fpvalor);
            dbProvider.AddInParameter(command, helper.Fpfechaperiodo, DbType.DateTime, entity.Fpfechaperiodo);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Tgenercodi, DbType.Int32, entity.Tgenercodi);

            dbProvider.AddInParameter(command, helper.Fpcodi, DbType.Int32, id);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(AbiFactorplantaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Fpfecmodificacion, DbType.DateTime, entity.Fpfecmodificacion);
            dbProvider.AddInParameter(command, helper.Fpusumodificacion, DbType.String, entity.Fpusumodificacion);
            dbProvider.AddInParameter(command, helper.Fptipogenerrer, DbType.String, entity.Fptipogenerrer);
            dbProvider.AddInParameter(command, helper.Fpintegrante, DbType.String, entity.Fpintegrante);
            dbProvider.AddInParameter(command, helper.Fpenergia, DbType.Decimal, entity.Fpenergia);
            dbProvider.AddInParameter(command, helper.Fppe, DbType.Decimal, entity.Fppe);
            dbProvider.AddInParameter(command, helper.Fpvalormes, DbType.Decimal, entity.Fpvalormes);
            dbProvider.AddInParameter(command, helper.Fpvalor, DbType.Decimal, entity.Fpvalor);
            dbProvider.AddInParameter(command, helper.Fpfechaperiodo, DbType.DateTime, entity.Fpfechaperiodo);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Tgenercodi, DbType.Int32, entity.Tgenercodi);
            dbProvider.AddInParameter(command, helper.Fpcodi, DbType.Int32, entity.Fpcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int fpcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Fpcodi, DbType.Int32, fpcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteByMes(DateTime fechaPeriodo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteByMes);

            dbProvider.AddInParameter(command, helper.Fpfechaperiodo, DbType.DateTime, fechaPeriodo);

            dbProvider.ExecuteNonQuery(command);
        }

        public AbiFactorplantaDTO GetById(int fpcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Fpcodi, DbType.Int32, fpcodi);
            AbiFactorplantaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<AbiFactorplantaDTO> List()
        {
            List<AbiFactorplantaDTO> entitys = new List<AbiFactorplantaDTO>();
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

        public List<AbiFactorplantaDTO> GetByCriteria()
        {
            List<AbiFactorplantaDTO> entitys = new List<AbiFactorplantaDTO>();
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
