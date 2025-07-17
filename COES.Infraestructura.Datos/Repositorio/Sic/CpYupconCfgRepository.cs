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
    /// Clase de acceso a datos de la tabla CP_YUPCON_CFG
    /// </summary>
    public class CpYupconCfgRepository : RepositoryBase, ICpYupconCfgRepository
    {
        public CpYupconCfgRepository(string strConn) : base(strConn)
        {
        }

        CpYupconCfgHelper helper = new CpYupconCfgHelper();

        public int Save(CpYupconCfgDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Yupcfgcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Yupcfgtipo, DbType.String, entity.Yupcfgtipo);
            dbProvider.AddInParameter(command, helper.Yupcfgfecha, DbType.DateTime, entity.Yupcfgfecha);
            dbProvider.AddInParameter(command, helper.Yupcfgbloquehorario, DbType.Int32, entity.Yupcfgbloquehorario);
            dbProvider.AddInParameter(command, helper.Tyupcodi, DbType.Int32, entity.Tyupcodi);
            dbProvider.AddInParameter(command, helper.Topcodi, DbType.Int32, entity.Topcodi);
            dbProvider.AddInParameter(command, helper.Yupcfgusuregistro, DbType.String, entity.Yupcfgusuregistro);
            dbProvider.AddInParameter(command, helper.Yupcfgfecregistro, DbType.DateTime, entity.Yupcfgfecregistro);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CpYupconCfgDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Yupcfgtipo, DbType.String, entity.Yupcfgtipo);
            dbProvider.AddInParameter(command, helper.Yupcfgfecha, DbType.DateTime, entity.Yupcfgfecha);
            dbProvider.AddInParameter(command, helper.Yupcfgbloquehorario, DbType.Int32, entity.Yupcfgbloquehorario);
            dbProvider.AddInParameter(command, helper.Tyupcodi, DbType.Int32, entity.Tyupcodi);
            dbProvider.AddInParameter(command, helper.Topcodi, DbType.Int32, entity.Topcodi);
            dbProvider.AddInParameter(command, helper.Yupcfgusuregistro, DbType.String, entity.Yupcfgusuregistro);
            dbProvider.AddInParameter(command, helper.Yupcfgfecregistro, DbType.DateTime, entity.Yupcfgfecregistro);
            dbProvider.AddInParameter(command, helper.Yupcfgcodi, DbType.Int32, entity.Yupcfgcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int yupcfgcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Yupcfgcodi, DbType.Int32, yupcfgcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CpYupconCfgDTO GetById(int yupcfgcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Yupcfgcodi, DbType.Int32, yupcfgcodi);
            CpYupconCfgDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CpYupconCfgDTO> List()
        {
            List<CpYupconCfgDTO> entitys = new List<CpYupconCfgDTO>();
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

        public List<CpYupconCfgDTO> GetByCriteria(int tyupcodi, DateTime fechaConsulta, int hora)
        {
            List<CpYupconCfgDTO> entitys = new List<CpYupconCfgDTO>();
            string sql = string.Format(helper.SqlGetByCriteria, tyupcodi, fechaConsulta.ToString(ConstantesBase.FormatoFecha), hora);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

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
