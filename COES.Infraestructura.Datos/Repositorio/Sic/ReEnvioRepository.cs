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
    /// Clase de acceso a datos de la tabla RE_ENVIO
    /// </summary>
    public class ReEnvioRepository: RepositoryBase, IReEnvioRepository
    {
        public ReEnvioRepository(string strConn): base(strConn)
        {
        }

        ReEnvioHelper helper = new ReEnvioHelper();

        public int Save(ReEnvioDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Reenvcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Repercodi, DbType.Int32, entity.Repercodi);
            dbProvider.AddInParameter(command, helper.Reenvtipo, DbType.String, entity.Reenvtipo);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Reenvfecha, DbType.DateTime, entity.Reenvfecha);
            dbProvider.AddInParameter(command, helper.Reenvplazo, DbType.String, entity.Reenvplazo);
            dbProvider.AddInParameter(command, helper.Reenvestado, DbType.String, entity.Reenvestado);
            dbProvider.AddInParameter(command, helper.Reenvindicador, DbType.String, entity.Reenvindicador);
            dbProvider.AddInParameter(command, helper.Reenvcomentario, DbType.String, entity.Reenvcomentario);
            dbProvider.AddInParameter(command, helper.Reenvusucreacion, DbType.String, entity.Reenvusucreacion);
            dbProvider.AddInParameter(command, helper.Reenvfeccreacion, DbType.DateTime, entity.Reenvfeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(ReEnvioDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Repercodi, DbType.Int32, entity.Repercodi);
            dbProvider.AddInParameter(command, helper.Reenvtipo, DbType.String, entity.Reenvtipo);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Reenvfecha, DbType.DateTime, entity.Reenvfecha);
            dbProvider.AddInParameter(command, helper.Reenvplazo, DbType.String, entity.Reenvplazo);
            dbProvider.AddInParameter(command, helper.Reenvestado, DbType.String, entity.Reenvestado);
            dbProvider.AddInParameter(command, helper.Reenvindicador, DbType.String, entity.Reenvindicador);
            dbProvider.AddInParameter(command, helper.Reenvcomentario, DbType.String, entity.Reenvcomentario);
            dbProvider.AddInParameter(command, helper.Reenvusucreacion, DbType.String, entity.Reenvusucreacion);
            dbProvider.AddInParameter(command, helper.Reenvfeccreacion, DbType.DateTime, entity.Reenvfeccreacion);
            dbProvider.AddInParameter(command, helper.Reenvcodi, DbType.Int32, entity.Reenvcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int reenvcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Reenvcodi, DbType.Int32, reenvcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public ReEnvioDTO GetById(int reenvcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Reenvcodi, DbType.Int32, reenvcodi);
            ReEnvioDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<ReEnvioDTO> List()
        {
            List<ReEnvioDTO> entitys = new List<ReEnvioDTO>();
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

        public List<ReEnvioDTO> GetByCriteria(int idEmpresa, int idPeriodo, string tipo)
        {
            List<ReEnvioDTO> entitys = new List<ReEnvioDTO>();
            string sql = string.Format(helper.SqlGetByCriteria, idEmpresa, idPeriodo, tipo);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<ReEnvioDTO> GetByPeriodoYEmpresa(int emprcodi, int idperiodo, string tipo)
        {
            List<ReEnvioDTO> entitys = new List<ReEnvioDTO>();
            
            string sql = string.Format(helper.SqlListarPorPeriodoYEmpresa, emprcodi, idperiodo, tipo);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            
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
