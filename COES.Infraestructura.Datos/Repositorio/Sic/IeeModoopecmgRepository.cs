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
    /// Clase de acceso a datos de la tabla IEE_MODOOPECMG
    /// </summary>
    public class IeeModoopecmgRepository : RepositoryBase, IIeeModoopecmgRepository
    {
        public IeeModoopecmgRepository(string strConn) : base(strConn)
        {
        }

        IeeModoopecmgHelper helper = new IeeModoopecmgHelper();

        public int Save(IeeModoopecmgDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Mocmcodigo, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Mocmtipocomb, DbType.Int32, entity.Mocmtipocomb);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(IeeModoopecmgDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Mocmcodigo, DbType.Int32, entity.Mocmcodigo);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Mocmtipocomb, DbType.Int32, entity.Mocmtipocomb);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int mocmcodigo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Mocmcodigo, DbType.Int32, mocmcodigo);

            dbProvider.ExecuteNonQuery(command);
        }

        public IeeModoopecmgDTO GetById(int mocmcodigo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Mocmcodigo, DbType.Int32, mocmcodigo);
            IeeModoopecmgDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<IeeModoopecmgDTO> List()
        {
            List<IeeModoopecmgDTO> entitys = new List<IeeModoopecmgDTO>();
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

        public List<IeeModoopecmgDTO> GetByCriteria()
        {
            List<IeeModoopecmgDTO> entitys = new List<IeeModoopecmgDTO>();
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
