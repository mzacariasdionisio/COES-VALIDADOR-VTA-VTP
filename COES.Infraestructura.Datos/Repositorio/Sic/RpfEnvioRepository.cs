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
    /// Clase de acceso a datos de la tabla RPF_ENVIO
    /// </summary>
    public class RpfEnvioRepository: RepositoryBase, IRpfEnvioRepository
    {
        public RpfEnvioRepository(string strConn): base(strConn)
        {
        }

        RpfEnvioHelper helper = new RpfEnvioHelper();

        public int Save(RpfEnvioDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Rpfenvcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Rpfenvfecha, DbType.DateTime, entity.Rpfenvfecha);
            dbProvider.AddInParameter(command, helper.Rpfenvestado, DbType.String, entity.Rpfenvestado);
            dbProvider.AddInParameter(command, helper.Rpfenvusucreacion, DbType.String, entity.Rpfenvusucreacion);
            dbProvider.AddInParameter(command, helper.Rpfenvfeccreacion, DbType.DateTime, entity.Rpfenvfeccreacion);
            dbProvider.AddInParameter(command, helper.Rpfenvusumodificacion, DbType.String, entity.Rpfenvusumodificacion);
            dbProvider.AddInParameter(command, helper.Rpfenvfecmodificacion, DbType.DateTime, entity.Rpfenvfecmodificacion);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(RpfEnvioDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Rpfenvfecha, DbType.DateTime, entity.Rpfenvfecha);
            dbProvider.AddInParameter(command, helper.Rpfenvestado, DbType.String, entity.Rpfenvestado);
            dbProvider.AddInParameter(command, helper.Rpfenvusucreacion, DbType.String, entity.Rpfenvusucreacion);
            dbProvider.AddInParameter(command, helper.Rpfenvfeccreacion, DbType.DateTime, entity.Rpfenvfeccreacion);
            dbProvider.AddInParameter(command, helper.Rpfenvusumodificacion, DbType.String, entity.Rpfenvusumodificacion);
            dbProvider.AddInParameter(command, helper.Rpfenvfecmodificacion, DbType.DateTime, entity.Rpfenvfecmodificacion);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Rpfenvcodi, DbType.Int32, entity.Rpfenvcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int rpfenvcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Rpfenvcodi, DbType.Int32, rpfenvcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public RpfEnvioDTO GetById(int rpfenvcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Rpfenvcodi, DbType.Int32, rpfenvcodi);
            RpfEnvioDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<RpfEnvioDTO> List()
        {
            List<RpfEnvioDTO> entitys = new List<RpfEnvioDTO>();
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

        public List<RpfEnvioDTO> GetByCriteria()
        {
            List<RpfEnvioDTO> entitys = new List<RpfEnvioDTO>();
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

        public RpfEnvioDTO ObtenerPorFecha(DateTime fecha, int idEmpresa)
        {
            string query = string.Format(helper.SqlObtenerPorFecha, fecha.ToString(ConstantesBase.FormatoFecha), idEmpresa);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            RpfEnvioDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<RpfEnvioDTO> ObtenerEnviosPorFecha(DateTime fecha)
        {
            List<RpfEnvioDTO> entitys = new List<RpfEnvioDTO>();
            string query = string.Format(helper.SqlObtenerEnviosPorFecha, fecha.ToString(ConstantesBase.FormatoFecha));
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
