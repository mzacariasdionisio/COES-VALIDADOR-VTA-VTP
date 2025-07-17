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
    /// Clase de acceso a datos de la tabla ST_COMPMENSUALELE
    /// </summary>
    public class StCompmensualeleRepository : RepositoryBase, IStCompmensualeleRepository
    {
        public StCompmensualeleRepository(string strConn)
            : base(strConn)
        {
        }

        StCompmensualeleHelper helper = new StCompmensualeleHelper();

        public int Save(StCompmensualeleDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cmpmelcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cmpmencodi, DbType.Int32, entity.Cmpmencodi);
            dbProvider.AddInParameter(command, helper.Stcompcodi, DbType.Int32, entity.Stcompcodi);
            dbProvider.AddInParameter(command, helper.Cmpmelcodelemento, DbType.String, entity.Cmpmelcodelemento);
            dbProvider.AddInParameter(command, helper.Cmpmelvalor, DbType.Decimal, entity.Cmpmelvalor);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(StCompmensualeleDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cmpmencodi, DbType.Int32, entity.Cmpmencodi);
            dbProvider.AddInParameter(command, helper.Stcompcodi, DbType.Int32, entity.Stcompcodi);
            dbProvider.AddInParameter(command, helper.Cmpmelcodelemento, DbType.String, entity.Cmpmelcodelemento);
            dbProvider.AddInParameter(command, helper.Cmpmelvalor, DbType.Decimal, entity.Cmpmelvalor);
            dbProvider.AddInParameter(command, helper.Cmpmelcodi, DbType.Int32, entity.Cmpmelcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int strecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, strecacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteStCompmensualEleVersion(int id)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteStCompmensualEleVersion);

            dbProvider.AddInParameter(command, helper.Cmpmencodi, DbType.Int32, id);

            dbProvider.ExecuteNonQuery(command);
        }

        public StCompmensualeleDTO GetById(int cmpmencodi, int stcompcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cmpmencodi, DbType.Int32, cmpmencodi);
            dbProvider.AddInParameter(command, helper.Cmpmelcodi, DbType.Int32, stcompcodi);
            StCompmensualeleDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<StCompmensualeleDTO> List()
        {
            List<StCompmensualeleDTO> entitys = new List<StCompmensualeleDTO>();
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

        public List<StCompmensualeleDTO> GetByCriteria(int strecacodi)
        {
            List<StCompmensualeleDTO> entitys = new List<StCompmensualeleDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, strecacodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {

                    StCompmensualeleDTO entity = helper.Create(dr);

                    int iEquinombre = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinombre)) entity.Equinomb = dr.GetString(iEquinombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public StCompmensualeleDTO GetByIdStCompMensualEle(int strecacodi, int stcompcodi, int stcntgcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdStCompMensualEle);

            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, strecacodi);
            dbProvider.AddInParameter(command, helper.Stcompcodi, DbType.Int32, stcompcodi);
            dbProvider.AddInParameter(command, helper.Stcntgcodi, DbType.Int32, stcntgcodi);
            StCompmensualeleDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<StCompmensualeleDTO> ListStCompMenElePorID(int Cmpmencodi)
        {
            List<StCompmensualeleDTO> entitys = new List<StCompmensualeleDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListStCompMenElePorID);
            dbProvider.AddInParameter(command, helper.Cmpmencodi, DbType.Int32, Cmpmencodi);

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
