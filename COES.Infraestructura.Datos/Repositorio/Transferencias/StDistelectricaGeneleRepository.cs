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
    /// Clase de acceso a datos de la tabla ST_DISTELECTRICA_GENELE
    /// </summary>
    public class StDistelectricaGeneleRepository: RepositoryBase, IStDistelectricaGeneleRepository
    {
        public StDistelectricaGeneleRepository(string strConn): base(strConn)
        {
        }

        StDistelectricaGeneleHelper helper = new StDistelectricaGeneleHelper();

        public int Save(StDistelectricaGeneleDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Degelecodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, entity.Strecacodi);
            dbProvider.AddInParameter(command, helper.Stcntgcodi, DbType.Int32, entity.Stcntgcodi);
            dbProvider.AddInParameter(command, helper.Barrcodigw, DbType.Int32, entity.Barrcodigw);
            dbProvider.AddInParameter(command, helper.Stcompcodi, DbType.Int32, entity.Stcompcodi);
            dbProvider.AddInParameter(command, helper.Barrcodilm, DbType.Int32, entity.Barrcodilm);
            dbProvider.AddInParameter(command, helper.Barrcodiln, DbType.Int32, entity.Barrcodiln);            
            dbProvider.AddInParameter(command, helper.Degeledistancia, DbType.Decimal, entity.Degeledistancia);
            dbProvider.AddInParameter(command, helper.Degeleusucreacion, DbType.String, entity.Degeleusucreacion);
            dbProvider.AddInParameter(command, helper.Degelefeccreacion, DbType.DateTime, entity.Degelefeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(StDistelectricaGeneleDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, entity.Strecacodi);
            dbProvider.AddInParameter(command, helper.Stcntgcodi, DbType.Int32, entity.Stcntgcodi);
            dbProvider.AddInParameter(command, helper.Barrcodigw, DbType.Int32, entity.Barrcodigw);
            dbProvider.AddInParameter(command, helper.Stcompcodi, DbType.Int32, entity.Stcompcodi);
            dbProvider.AddInParameter(command, helper.Barrcodilm, DbType.Int32, entity.Barrcodilm);
            dbProvider.AddInParameter(command, helper.Barrcodiln, DbType.Int32, entity.Barrcodiln);
            dbProvider.AddInParameter(command, helper.Degeledistancia, DbType.Decimal, entity.Degeledistancia);
            dbProvider.AddInParameter(command, helper.Degeleusucreacion, DbType.String, entity.Degeleusucreacion);
            dbProvider.AddInParameter(command, helper.Degelefeccreacion, DbType.DateTime, entity.Degelefeccreacion);
            dbProvider.AddInParameter(command, helper.Degelecodi, DbType.Int32, entity.Degelecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int strecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, strecacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public StDistelectricaGeneleDTO GetById(int degelecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Degelecodi, DbType.Int32, degelecodi);
            StDistelectricaGeneleDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<StDistelectricaGeneleDTO> List()
        {
            List<StDistelectricaGeneleDTO> entitys = new List<StDistelectricaGeneleDTO>();
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

        public List<StDistelectricaGeneleDTO> GetByCriteria(int strecacodi, int stcompcodi)
        {
            List<StDistelectricaGeneleDTO> entitys = new List<StDistelectricaGeneleDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, strecacodi);
            dbProvider.AddInParameter(command, helper.Stcompcodi, DbType.Int32, stcompcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public StDistelectricaGeneleDTO GetByIdCriteriaStDistGene(int stcntgcodi, int stcompcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdCriteriaStDistGene);

            dbProvider.AddInParameter(command, helper.Stcntgcodi, DbType.Int32, stcntgcodi);
            dbProvider.AddInParameter(command, helper.Stcompcodi, DbType.Int32, stcompcodi);
            StDistelectricaGeneleDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<StDistelectricaGeneleDTO> GetByIdCriteriaStDistGeneReporte(int strecacodi)
        {
            List<StDistelectricaGeneleDTO> entitys = new List<StDistelectricaGeneleDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdCriteriaStDistGeneReporte);
            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, strecacodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    StDistelectricaGeneleDTO entity = helper.Create(dr);

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iCmpmelcodelemento = dr.GetOrdinal(this.helper.Cmpmelcodelemento);
                    if (!dr.IsDBNull(iCmpmelcodelemento)) entity.Cmpmelcodelemento = dr.GetString(iCmpmelcodelemento);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

    }
}
