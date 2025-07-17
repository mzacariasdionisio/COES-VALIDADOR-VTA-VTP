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
    /// Clase de acceso a datos de la tabla ME_INFORME_INTERCONEXION
    /// </summary>
    public class MeInformeInterconexionRepository: RepositoryBase, IMeInformeInterconexionRepository
    {
        public MeInformeInterconexionRepository(string strConn): base(strConn)
        {
        }

        MeInformeInterconexionHelper helper = new MeInformeInterconexionHelper();

        public int Save(MeInformeInterconexionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Infintcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Infintanio, DbType.Int32, entity.Infintanio);
            dbProvider.AddInParameter(command, helper.Infintnrosemana, DbType.Int32, entity.Infintnrosemana);
            dbProvider.AddInParameter(command, helper.Infintversion, DbType.Int32, entity.Infintversion);
            dbProvider.AddInParameter(command, helper.Infintestado, DbType.String, entity.Infintestado);
            dbProvider.AddInParameter(command, helper.Infintusucreacion, DbType.String, entity.Infintusucreacion);
            dbProvider.AddInParameter(command, helper.Infintfeccreacion, DbType.DateTime, entity.Infintfeccreacion);
            dbProvider.AddInParameter(command, helper.Infintusumodificacion, DbType.String, entity.Infintusumodificacion);
            dbProvider.AddInParameter(command, helper.Infintfecmodificacion, DbType.DateTime, entity.Infintfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(MeInformeInterconexionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Infintanio, DbType.Int32, entity.Infintanio);
            dbProvider.AddInParameter(command, helper.Infintnrosemana, DbType.Int32, entity.Infintnrosemana);
            dbProvider.AddInParameter(command, helper.Infintversion, DbType.Int32, entity.Infintversion);
            dbProvider.AddInParameter(command, helper.Infintestado, DbType.String, entity.Infintestado);
            dbProvider.AddInParameter(command, helper.Infintusucreacion, DbType.String, entity.Infintusucreacion);
            dbProvider.AddInParameter(command, helper.Infintfeccreacion, DbType.DateTime, entity.Infintfeccreacion);
            dbProvider.AddInParameter(command, helper.Infintusumodificacion, DbType.String, entity.Infintusumodificacion);
            dbProvider.AddInParameter(command, helper.Infintfecmodificacion, DbType.DateTime, entity.Infintfecmodificacion);
            dbProvider.AddInParameter(command, helper.Infintcodi, DbType.Int32, entity.Infintcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int infintcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Infintcodi, DbType.Int32, infintcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public MeInformeInterconexionDTO GetById(int infintcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Infintcodi, DbType.Int32, infintcodi);
            MeInformeInterconexionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MeInformeInterconexionDTO> List()
        {
            List<MeInformeInterconexionDTO> entitys = new List<MeInformeInterconexionDTO>();
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

        public List<MeInformeInterconexionDTO> GetByCriteria(int anio, int semana)
        {
            List<MeInformeInterconexionDTO> entitys = new List<MeInformeInterconexionDTO>();
            string sql = string.Format(helper.SqlGetByCriteria, anio, semana);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeInformeInterconexionDTO entity = helper.Create(dr);
                    entity.NombreSemana = string.Format("Sem{0}-{1}", entity.Infintnrosemana, entity.Infintanio);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
