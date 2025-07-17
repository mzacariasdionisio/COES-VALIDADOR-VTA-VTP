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
    /// Clase de acceso a datos de la tabla CP_YUPCON_CFGDET
    /// </summary>
    public class CpYupconCfgdetRepository : RepositoryBase, ICpYupconCfgdetRepository
    {
        public CpYupconCfgdetRepository(string strConn) : base(strConn)
        {
        }

        CpYupconCfgdetHelper helper = new CpYupconCfgdetHelper();

        public int Save(CpYupconCfgdetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Recurcodi, DbType.Int32, entity.Recurcodi);
            dbProvider.AddInParameter(command, helper.Topcodi, DbType.Int32, entity.Topcodi);
            dbProvider.AddInParameter(command, helper.Yupcfgcodi, DbType.Int32, entity.Yupcfgcodi);
            dbProvider.AddInParameter(command, helper.Ycdetcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Ycdetfactor, DbType.Decimal, entity.Ycdetfactor);
            dbProvider.AddInParameter(command, helper.Ycdetactivo, DbType.Int32, entity.Ycdetactivo);
            dbProvider.AddInParameter(command, helper.Ycdetusuregistro, DbType.String, entity.Ycdetusuregistro);
            dbProvider.AddInParameter(command, helper.Ycdetusumodificacion, DbType.String, entity.Ycdetusumodificacion);
            dbProvider.AddInParameter(command, helper.Ycdetfecregistro, DbType.DateTime, entity.Ycdetfecregistro);
            dbProvider.AddInParameter(command, helper.Ycdetfecmodificacion, DbType.DateTime, entity.Ycdetfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CpYupconCfgdetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Ycdetactivo, DbType.Int32, entity.Ycdetactivo);
            dbProvider.AddInParameter(command, helper.Ycdetusumodificacion, DbType.String, entity.Ycdetusumodificacion);
            dbProvider.AddInParameter(command, helper.Ycdetfecmodificacion, DbType.DateTime, entity.Ycdetfecmodificacion);

            dbProvider.AddInParameter(command, helper.Ycdetcodi, DbType.Int32, entity.Ycdetcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int ycdetcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Ycdetcodi, DbType.Int32, ycdetcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CpYupconCfgdetDTO GetById(int ycdetcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Ycdetcodi, DbType.Int32, ycdetcodi);
            CpYupconCfgdetDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CpYupconCfgdetDTO> List()
        {
            List<CpYupconCfgdetDTO> entitys = new List<CpYupconCfgdetDTO>();
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

        public List<CpYupconCfgdetDTO> GetByCriteria(int yupcfgcodi, int recurcodi)
        {
            List<CpYupconCfgdetDTO> entitys = new List<CpYupconCfgdetDTO>();

            string sql = string.Format(helper.SqlGetByCriteria, yupcfgcodi, recurcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iRecurnombre = dr.GetOrdinal(helper.Recurnombre);
                    if (!dr.IsDBNull(iRecurnombre)) entity.Recurnombre = dr.GetString(iRecurnombre);

                    int iPtomedielenomb = dr.GetOrdinal(helper.Ptomedielenomb);
                    if (!dr.IsDBNull(iPtomedielenomb)) entity.Ptomedielenomb = dr.GetString(iPtomedielenomb);

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
