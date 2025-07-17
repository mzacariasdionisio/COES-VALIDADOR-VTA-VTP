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
    /// Clase de acceso a datos de la tabla IN_SECCION
    /// </summary>
    public class InSeccionRepository : RepositoryBase, IInSeccionRepository
    {
        public InSeccionRepository(string strConn) : base(strConn)
        {
        }

        InSeccionHelper helper = new InSeccionHelper();

        public void Save(InSeccionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            dbProvider.AddInParameter(command, helper.Inseccodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Insecnombre, DbType.String, entity.Insecnombre);
            dbProvider.AddInParameter(command, helper.Inseccontenido, DbType.String, entity.Inseccontenido);
            dbProvider.AddInParameter(command, helper.Insecusumodificacion, DbType.String, entity.Insecusumodificacion);
            dbProvider.AddInParameter(command, helper.Insecfeccracion, DbType.DateTime, entity.Insecfeccracion);
            dbProvider.AddInParameter(command, helper.Insecusucreacion, DbType.String, entity.Insecusucreacion);
            dbProvider.AddInParameter(command, helper.Insecfeccreacion, DbType.DateTime, entity.Insecfeccreacion);
            dbProvider.AddInParameter(command, helper.Inrepcodi, DbType.Int32, entity.Inrepcodi);
            dbProvider.ExecuteNonQuery(command);
        }


        public void Update(InSeccionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Insecnombre, DbType.String, entity.Insecnombre);
            dbProvider.AddInParameter(command, helper.Inseccontenido, DbType.String, entity.Inseccontenido);
            dbProvider.AddInParameter(command, helper.Insecusumodificacion, DbType.String, entity.Insecusumodificacion);
            dbProvider.AddInParameter(command, helper.Insecfeccracion, DbType.DateTime, entity.Insecfeccracion);
            dbProvider.AddInParameter(command, helper.Insecusucreacion, DbType.String, entity.Insecusucreacion);
            dbProvider.AddInParameter(command, helper.Insecfeccreacion, DbType.DateTime, entity.Insecfeccreacion);
            dbProvider.AddInParameter(command, helper.Inrepcodi, DbType.Int32, entity.Inrepcodi);
            dbProvider.AddInParameter(command, helper.Inseccodi, DbType.Int32, entity.Inseccodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void UpdateSeccion(InSeccionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateSeccion);

            dbProvider.AddInParameter(command, helper.Inseccontenido, DbType.String, entity.Inseccontenido);
            dbProvider.AddInParameter(command, helper.Insecusumodificacion, DbType.String, entity.Insecusumodificacion);
            dbProvider.AddInParameter(command, helper.Insecfeccracion, DbType.DateTime, entity.Insecfeccracion);
            dbProvider.AddInParameter(command, helper.Inrepcodi, DbType.Int32, entity.Inrepcodi);
            dbProvider.AddInParameter(command, helper.Inseccodi, DbType.Int32, entity.Inseccodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int Inseccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Inseccodi, DbType.Int32, Inseccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public InSeccionDTO GetById(int Inseccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Inseccodi, DbType.Int32, Inseccodi);
            InSeccionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<InSeccionDTO> List()
        {
            List<InSeccionDTO> entitys = new List<InSeccionDTO>();
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

        public List<InSeccionDTO> GetByCriteria(int repcodi)
        {
            List<InSeccionDTO> entitys = new List<InSeccionDTO>();

            string query = string.Format(helper.SqlGetByCriteria, repcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    InSeccionDTO entity = helper.Create(dr);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
