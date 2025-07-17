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
    /// Clase de acceso a datos de la tabla CM_EQUIPOBARRA_DET
    /// </summary>
    public class CmEquipobarraDetRepository: RepositoryBase, ICmEquipobarraDetRepository
    {
        public CmEquipobarraDetRepository(string strConn): base(strConn)
        {
        }

        CmEquipobarraDetHelper helper = new CmEquipobarraDetHelper();

        public int Save(CmEquipobarraDetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cmebdecodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cmeqbacodi, DbType.Int32, entity.Cmeqbacodi);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, entity.Barrcodi);
            dbProvider.AddInParameter(command, helper.Cmebdeusucreacion, DbType.String, entity.Cmebdeusucreacion);
            dbProvider.AddInParameter(command, helper.Cmebdefeccreacion, DbType.DateTime, entity.Cmebdefeccreacion);
            dbProvider.AddInParameter(command, helper.Cmebdeusumodificacion, DbType.String, entity.Cmebdeusumodificacion);
            dbProvider.AddInParameter(command, helper.Cmebdefecmodificacion, DbType.DateTime, entity.Cmebdefecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CmEquipobarraDetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cmeqbacodi, DbType.Int32, entity.Cmeqbacodi);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, entity.Barrcodi);
            dbProvider.AddInParameter(command, helper.Cmebdeusucreacion, DbType.String, entity.Cmebdeusucreacion);
            dbProvider.AddInParameter(command, helper.Cmebdefeccreacion, DbType.DateTime, entity.Cmebdefeccreacion);
            dbProvider.AddInParameter(command, helper.Cmebdeusumodificacion, DbType.String, entity.Cmebdeusumodificacion);
            dbProvider.AddInParameter(command, helper.Cmebdefecmodificacion, DbType.DateTime, entity.Cmebdefecmodificacion);
            dbProvider.AddInParameter(command, helper.Cmebdecodi, DbType.Int32, entity.Cmebdecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cmebdecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cmeqbacodi, DbType.Int32, cmebdecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CmEquipobarraDetDTO GetById(int cmebdecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cmebdecodi, DbType.Int32, cmebdecodi);
            CmEquipobarraDetDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CmEquipobarraDetDTO> List()
        {
            List<CmEquipobarraDetDTO> entitys = new List<CmEquipobarraDetDTO>();
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

        public List<CmEquipobarraDetDTO> GetByCriteria(int idPadre)
        {
            List<CmEquipobarraDetDTO> entitys = new List<CmEquipobarraDetDTO>();
            string sql = string.Format(helper.SqlGetByCriteria, idPadre);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CmEquipobarraDetDTO entity = helper.Create(dr);

                    int iBarrnombre = dr.GetOrdinal(helper.Barrnombre);
                    if (!dr.IsDBNull(iBarrnombre)) entity.Barrnombre = dr.GetString(iBarrnombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
