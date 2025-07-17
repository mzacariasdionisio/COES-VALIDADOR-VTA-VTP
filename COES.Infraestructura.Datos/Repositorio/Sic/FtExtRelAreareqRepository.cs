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
    /// Clase de acceso a datos de la tabla FT_EXT_REL_AREAREQ
    /// </summary>
    public class FtExtRelAreareqRepository: RepositoryBase, IFtExtRelAreareqRepository
    {
        public FtExtRelAreareqRepository(string strConn): base(strConn)
        {
        }

        FtExtRelAreareqHelper helper = new FtExtRelAreareqHelper();

        public int Save(FtExtRelAreareqDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Frracodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Faremcodi, DbType.Int32, entity.Faremcodi);
            dbProvider.AddInParameter(command, helper.Fevrqcodi, DbType.Int32, entity.Fevrqcodi);
            dbProvider.AddInParameter(command, helper.Frraestado, DbType.String, entity.Frraestado);
            dbProvider.AddInParameter(command, helper.Frrafeccreacion, DbType.DateTime, entity.Frrafeccreacion);
            dbProvider.AddInParameter(command, helper.Frrausucreacion, DbType.String, entity.Frrausucreacion);
            dbProvider.AddInParameter(command, helper.Frrafecmodificacion, DbType.DateTime, entity.Frrafecmodificacion);
            dbProvider.AddInParameter(command, helper.Frrausumodificacion, DbType.String, entity.Frrausumodificacion);
            dbProvider.AddInParameter(command, helper.Frraflaghidro, DbType.String, entity.Frraflaghidro);
            dbProvider.AddInParameter(command, helper.Frraflagtermo, DbType.String, entity.Frraflagtermo);
            dbProvider.AddInParameter(command, helper.Frraflagsolar, DbType.String, entity.Frraflagsolar);
            dbProvider.AddInParameter(command, helper.Frraflageolico, DbType.String, entity.Frraflageolico);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(FtExtRelAreareqDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Faremcodi, DbType.Int32, entity.Faremcodi);
            dbProvider.AddInParameter(command, helper.Fevrqcodi, DbType.Int32, entity.Fevrqcodi);
            dbProvider.AddInParameter(command, helper.Frraestado, DbType.String, entity.Frraestado);
            dbProvider.AddInParameter(command, helper.Frrafeccreacion, DbType.DateTime, entity.Frrafeccreacion);
            dbProvider.AddInParameter(command, helper.Frrausucreacion, DbType.String, entity.Frrausucreacion);
            dbProvider.AddInParameter(command, helper.Frrafecmodificacion, DbType.DateTime, entity.Frrafecmodificacion);
            dbProvider.AddInParameter(command, helper.Frrausumodificacion, DbType.String, entity.Frrausumodificacion);
            dbProvider.AddInParameter(command, helper.Frraflaghidro, DbType.String, entity.Frraflaghidro);
            dbProvider.AddInParameter(command, helper.Frraflagtermo, DbType.String, entity.Frraflagtermo);
            dbProvider.AddInParameter(command, helper.Frraflagsolar, DbType.String, entity.Frraflagsolar);
            dbProvider.AddInParameter(command, helper.Frraflageolico, DbType.String, entity.Frraflageolico);
            dbProvider.AddInParameter(command, helper.Frracodi, DbType.Int32, entity.Frracodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int frracodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Frracodi, DbType.Int32, frracodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public FtExtRelAreareqDTO GetById(int frracodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Frracodi, DbType.Int32, frracodi);
            FtExtRelAreareqDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<FtExtRelAreareqDTO> List()
        {
            List<FtExtRelAreareqDTO> entitys = new List<FtExtRelAreareqDTO>();
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

        public List<FtExtRelAreareqDTO> GetByCriteria()
        {
            List<FtExtRelAreareqDTO> entitys = new List<FtExtRelAreareqDTO>();
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

        public List<FtExtRelAreareqDTO> ListarPorAreas(string estado, string famercodis)
        {
            List<FtExtRelAreareqDTO> entitys = new List<FtExtRelAreareqDTO>();
            string sql = string.Format(helper.SqlListarPorAreas, estado, famercodis);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FtExtRelAreareqDTO entity = helper.Create(dr);

                    int iFaremnombre = dr.GetOrdinal(helper.Faremnombre);
                    if (!dr.IsDBNull(iFaremnombre)) entity.Faremnombre = dr.GetString(iFaremnombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
