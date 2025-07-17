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
    /// Clase de acceso a datos de la tabla HT_CENTRAL_CFGDET
    /// </summary>
    public class HtCentralCfgdetRepository: RepositoryBase, IHtCentralCfgdetRepository
    {
        public HtCentralCfgdetRepository(string strConn): base(strConn)
        {
        }

        HtCentralCfgdetHelper helper = new HtCentralCfgdetHelper();

        public int Save(HtCentralCfgdetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Htcdetcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Htcentcodi, DbType.Int32, entity.Htcentcodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Canalcodi, DbType.Int32, entity.Canalcodi);
            dbProvider.AddInParameter(command, helper.Htcdetfactor, DbType.Decimal, entity.Htcdetfactor);
            dbProvider.AddInParameter(command, helper.Htcdetactivo, DbType.Int32, entity.Htcdetactivo);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(HtCentralCfgdetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Htcentcodi, DbType.Int32, entity.Htcentcodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Canalcodi, DbType.Int32, entity.Canalcodi);
            dbProvider.AddInParameter(command, helper.Htcdetfactor, DbType.Decimal, entity.Htcdetfactor);
            dbProvider.AddInParameter(command, helper.Htcdetactivo, DbType.Int32, entity.Htcdetactivo);
            dbProvider.AddInParameter(command, helper.Htcdetcodi, DbType.Int32, entity.Htcdetcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);


            dbProvider.ExecuteNonQuery(command);
        }

        public HtCentralCfgdetDTO GetById()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            HtCentralCfgdetDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<HtCentralCfgdetDTO> List()
        {
            List<HtCentralCfgdetDTO> entitys = new List<HtCentralCfgdetDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iPtomedielenomb = dr.GetOrdinal(helper.Ptomedielenomb);
                    if (!dr.IsDBNull(iPtomedielenomb)) entity.Ptomedielenomb = dr.GetString(iPtomedielenomb);

                    int iCanalnomb = dr.GetOrdinal(helper.Canalnomb);
                    if (!dr.IsDBNull(iCanalnomb)) entity.Canalnomb = dr.GetString(iCanalnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<HtCentralCfgdetDTO> GetByCriteria(int htcentcodi)
        {
            List<HtCentralCfgdetDTO> entitys = new List<HtCentralCfgdetDTO>();
            
            string query = string.Format(helper.SqlGetByCriteria, htcentcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iPtomedielenomb = dr.GetOrdinal(helper.Ptomedielenomb);
                    if (!dr.IsDBNull(iPtomedielenomb)) entity.Ptomedielenomb = dr.GetString(iPtomedielenomb);

                    int iCanalnomb = dr.GetOrdinal(helper.Canalnomb);
                    if (!dr.IsDBNull(iCanalnomb)) entity.Canalnomb = dr.GetString(iCanalnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
