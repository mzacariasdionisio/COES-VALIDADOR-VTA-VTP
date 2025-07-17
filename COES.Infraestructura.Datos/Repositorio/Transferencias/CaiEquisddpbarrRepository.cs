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
    /// Clase de acceso a datos de la tabla CAI_EQUISDDPBARR
    /// </summary>
    public class CaiEquisddpbarrRepository: RepositoryBase, ICaiEquisddpbarrRepository
    {
        public CaiEquisddpbarrRepository(string strConn): base(strConn)
        {
        }

        CaiEquisddpbarrHelper helper = new CaiEquisddpbarrHelper();

        public int Save(CaiEquisddpbarrDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Casddbcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, entity.Barrcodi);
            dbProvider.AddInParameter(command, helper.Casddbbarra, DbType.String, entity.Casddbbarra);
            dbProvider.AddInParameter(command, helper.Casddbfecvigencia, DbType.DateTime, entity.Casddbfecvigencia);
            dbProvider.AddInParameter(command, helper.Casddbusucreacion, DbType.String, entity.Casddbusucreacion);
            dbProvider.AddInParameter(command, helper.Casddbfeccreacion, DbType.DateTime, entity.Casddbfeccreacion);
            dbProvider.AddInParameter(command, helper.Casddbusumodificacion, DbType.String, entity.Casddbusumodificacion);
            dbProvider.AddInParameter(command, helper.Casddbfecmodificacion, DbType.DateTime, entity.Casddbfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CaiEquisddpbarrDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, entity.Barrcodi);
            dbProvider.AddInParameter(command, helper.Casddbbarra, DbType.String, entity.Casddbbarra);
            dbProvider.AddInParameter(command, helper.Casddbfecvigencia, DbType.DateTime, entity.Casddbfecvigencia);
            dbProvider.AddInParameter(command, helper.Casddbusumodificacion, DbType.String, entity.Casddbusumodificacion);
            dbProvider.AddInParameter(command, helper.Casddbfecmodificacion, DbType.DateTime, entity.Casddbfecmodificacion);
            dbProvider.AddInParameter(command, helper.Casddbcodi, DbType.Int32, entity.Casddbcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int casddbcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Casddbcodi, DbType.Int32, casddbcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CaiEquisddpbarrDTO GetById(int casddbcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Casddbcodi, DbType.Int32, casddbcodi);
            CaiEquisddpbarrDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public CaiEquisddpbarrDTO GetByIdCaiEquisddpbarr(int casddbcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.GetByIdCaiEquisddpbarr);

            dbProvider.AddInParameter(command, helper.Casddbcodi, DbType.Int32, casddbcodi);
            CaiEquisddpbarrDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                    int iBarrNombre = dr.GetOrdinal(this.helper.Barrnombre);
                    if (!dr.IsDBNull(iBarrNombre)) entity.Barrnombre = dr.GetString(iBarrNombre);
                }
            }

            return entity;
        }

        public CaiEquisddpbarrDTO GetByNombreBarraSddp(string sddpgmnombre)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.GetByNombreBarraSddp);

            dbProvider.AddInParameter(command, helper.Casddbbarra, DbType.String, sddpgmnombre);
            CaiEquisddpbarrDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CaiEquisddpbarrDTO> List()
        {
            List<CaiEquisddpbarrDTO> entitys = new List<CaiEquisddpbarrDTO>();
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

        public List<CaiEquisddpbarrDTO> ListCaiEquisddpbarr()
        {
            List<CaiEquisddpbarrDTO> entitys = new List<CaiEquisddpbarrDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListEquisddpbarr);

            CaiEquisddpbarrDTO entity;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iBarrNombre = dr.GetOrdinal(this.helper.Barrnombre);
                    if (!dr.IsDBNull(iBarrNombre)) entity.Barrnombre = dr.GetString(iBarrNombre);

                    entitys.Add(entity);

                }
            }

            return entitys;
        }

        public List<CaiEquisddpbarrDTO> GetByCriteria()
        {
            List<CaiEquisddpbarrDTO> entitys = new List<CaiEquisddpbarrDTO>();
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

        public List<CaiEquisddpbarrDTO> GetByCriteriaCaiEquiunidbarrsNoIns()
        {
            List<CaiEquisddpbarrDTO> entitys = new List<CaiEquisddpbarrDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.GetByCriteriaCaiEquiunidbarrsNoIns);

            CaiEquisddpbarrDTO entity;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new CaiEquisddpbarrDTO();
                    entity = helper.Create(dr);

                    int iBarrNombre = dr.GetOrdinal(this.helper.Barrnombre);
                    if (!dr.IsDBNull(iBarrNombre)) entity.Barrnombre = dr.GetString(iBarrNombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public CaiEquisddpbarrDTO GetByBarraNombreSddp(int barrcodi, string sddpgmnombre)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.GetByBarraNombreSddp);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, barrcodi);
            dbProvider.AddInParameter(command, helper.Casddbbarra, DbType.String, sddpgmnombre);
            CaiEquisddpbarrDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }
    }
}
