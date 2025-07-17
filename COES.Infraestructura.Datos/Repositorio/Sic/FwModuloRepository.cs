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
    /// Clase de acceso a datos de la tabla FW_MODULO
    /// </summary>
    public class FwModuloRepository : RepositoryBase, IFwModuloRepository
    {
        public FwModuloRepository(string strConn)
            : base(strConn)
        {
        }

        FwModuloHelper helper = new FwModuloHelper();

        public int Save(FwModuloDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Modcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Modnombre, DbType.String, entity.Modnombre);
            dbProvider.AddInParameter(command, helper.Modestado, DbType.String, entity.Modestado);
            dbProvider.AddInParameter(command, helper.Rolcode, DbType.Int32, entity.Rolcode);
            dbProvider.AddInParameter(command, helper.Pathfile, DbType.String, entity.Pathfile);
            dbProvider.AddInParameter(command, helper.Fuenterepositorio, DbType.String, entity.Fuenterepositorio);
            dbProvider.AddInParameter(command, helper.Usermanual, DbType.String, entity.Usermanual);
            dbProvider.AddInParameter(command, helper.Inddefecto, DbType.String, entity.Inddefecto);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(FwModuloDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Modnombre, DbType.String, entity.Modnombre);
            dbProvider.AddInParameter(command, helper.Modestado, DbType.String, entity.Modestado);
            dbProvider.AddInParameter(command, helper.Rolcode, DbType.Int32, entity.Rolcode);
            dbProvider.AddInParameter(command, helper.Pathfile, DbType.String, entity.Pathfile);
            dbProvider.AddInParameter(command, helper.Fuenterepositorio, DbType.String, entity.Fuenterepositorio);
            dbProvider.AddInParameter(command, helper.Usermanual, DbType.String, entity.Usermanual);
            dbProvider.AddInParameter(command, helper.Inddefecto, DbType.String, entity.Inddefecto);
            dbProvider.AddInParameter(command, helper.Modcodi, DbType.Int32, entity.Modcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int Modcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Modcodi, DbType.Int32, Modcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public FwModuloDTO GetById(int Modcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Modcodi, DbType.Int32, Modcodi);
            FwModuloDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<FwModuloDTO> List()
        {
            List<FwModuloDTO> entitys = new List<FwModuloDTO>();
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

        public List<FwModuloDTO> GetByCriteria()
        {
            List<FwModuloDTO> entitys = new List<FwModuloDTO>();
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
