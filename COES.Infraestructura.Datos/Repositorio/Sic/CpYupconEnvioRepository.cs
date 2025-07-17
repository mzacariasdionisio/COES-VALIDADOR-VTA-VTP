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
    /// Clase de acceso a datos de la tabla CP_YUPCON_ENVIO
    /// </summary>
    public class CpYupconEnvioRepository : RepositoryBase, ICpYupconEnvioRepository
    {
        public CpYupconEnvioRepository(string strConn) : base(strConn)
        {
        }

        CpYupconEnvioHelper helper = new CpYupconEnvioHelper();

        public int Save(CpYupconEnvioDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cyupcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cyupfecha, DbType.DateTime, entity.Cyupfecha);
            dbProvider.AddInParameter(command, helper.Cyupbloquehorario, DbType.Int32, entity.Cyupbloquehorario);
            dbProvider.AddInParameter(command, helper.Cyupusuregistro, DbType.String, entity.Cyupusuregistro);
            dbProvider.AddInParameter(command, helper.Cyupfecregistro, DbType.DateTime, entity.Cyupfecregistro);
            dbProvider.AddInParameter(command, helper.Topcodi, DbType.Int32, entity.Topcodi);
            dbProvider.AddInParameter(command, helper.Tyupcodi, DbType.Int32, entity.Tyupcodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CpYupconEnvioDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cyupcodi, DbType.Int32, entity.Cyupcodi);
            dbProvider.AddInParameter(command, helper.Cyupfecha, DbType.DateTime, entity.Cyupfecha);
            dbProvider.AddInParameter(command, helper.Cyupbloquehorario, DbType.Int32, entity.Cyupbloquehorario);
            dbProvider.AddInParameter(command, helper.Cyupusuregistro, DbType.String, entity.Cyupusuregistro);
            dbProvider.AddInParameter(command, helper.Cyupfecregistro, DbType.DateTime, entity.Cyupfecregistro);
            dbProvider.AddInParameter(command, helper.Topcodi, DbType.Int32, entity.Topcodi);
            dbProvider.AddInParameter(command, helper.Tyupcodi, DbType.Int32, entity.Tyupcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cyupcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cyupcodi, DbType.Int32, cyupcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CpYupconEnvioDTO GetById(int cyupcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cyupcodi, DbType.Int32, cyupcodi);
            CpYupconEnvioDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CpYupconEnvioDTO> List()
        {
            List<CpYupconEnvioDTO> entitys = new List<CpYupconEnvioDTO>();
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

        public List<CpYupconEnvioDTO> GetByCriteria(int tyupcodi, DateTime fecha, int hora)
        {
            List<CpYupconEnvioDTO> entitys = new List<CpYupconEnvioDTO>();
            string sql = string.Format(helper.SqlGetByCriteria, tyupcodi, fecha.ToString(ConstantesBase.FormatoFecha), hora);
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
