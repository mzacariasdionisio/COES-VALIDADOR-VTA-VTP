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
    /// Clase de acceso a datos de la tabla FT_EXT_EVENTO_REQ
    /// </summary>
    public class FtExtEventoReqRepository : RepositoryBase, IFtExtEventoReqRepository
    {
        public FtExtEventoReqRepository(string strConn) : base(strConn)
        {
        }

        FtExtEventoReqHelper helper = new FtExtEventoReqHelper();

        public int Save(FtExtEventoReqDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Ftevcodi, DbType.Int32, entity.Ftevcodi);
            dbProvider.AddInParameter(command, helper.Fevrqcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Fevrqliteral, DbType.String, entity.Fevrqliteral);
            dbProvider.AddInParameter(command, helper.Fevrqdesc, DbType.String, entity.Fevrqdesc);
            dbProvider.AddInParameter(command, helper.Fevrqflaghidro, DbType.String, entity.Fevrqflaghidro);
            dbProvider.AddInParameter(command, helper.Fevrqflagtermo, DbType.String, entity.Fevrqflagtermo);
            dbProvider.AddInParameter(command, helper.Fevrqflagsolar, DbType.String, entity.Fevrqflagsolar);
            dbProvider.AddInParameter(command, helper.Fevrqflageolico, DbType.String, entity.Fevrqflageolico);
            dbProvider.AddInParameter(command, helper.Fevrqestado, DbType.String, entity.Fevrqestado);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(FtExtEventoReqDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Ftevcodi, DbType.Int32, entity.Ftevcodi);

            dbProvider.AddInParameter(command, helper.Fevrqliteral, DbType.String, entity.Fevrqliteral);
            dbProvider.AddInParameter(command, helper.Fevrqdesc, DbType.String, entity.Fevrqdesc);
            dbProvider.AddInParameter(command, helper.Fevrqflaghidro, DbType.String, entity.Fevrqflaghidro);
            dbProvider.AddInParameter(command, helper.Fevrqflagtermo, DbType.String, entity.Fevrqflagtermo);
            dbProvider.AddInParameter(command, helper.Fevrqflagsolar, DbType.String, entity.Fevrqflagsolar);
            dbProvider.AddInParameter(command, helper.Fevrqflageolico, DbType.String, entity.Fevrqflageolico);
            dbProvider.AddInParameter(command, helper.Fevrqestado, DbType.String, entity.Fevrqestado);
            dbProvider.AddInParameter(command, helper.Fevrqcodi, DbType.Int32, entity.Fevrqcodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int fevrqcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Fevrqcodi, DbType.Int32, fevrqcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public FtExtEventoReqDTO GetById(int fevrqcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Fevrqcodi, DbType.Int32, fevrqcodi);
            FtExtEventoReqDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<FtExtEventoReqDTO> List()
        {
            List<FtExtEventoReqDTO> entitys = new List<FtExtEventoReqDTO>();
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

        public List<FtExtEventoReqDTO> GetByCriteria()
        {
            List<FtExtEventoReqDTO> entitys = new List<FtExtEventoReqDTO>();
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
