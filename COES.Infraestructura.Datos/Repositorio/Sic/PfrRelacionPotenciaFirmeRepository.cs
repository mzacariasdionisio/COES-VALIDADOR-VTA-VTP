using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla PFR_RELACION_POTENCIA_FIRME
    /// </summary>
    public class PfrRelacionPotenciaFirmeRepository: RepositoryBase, IPfrRelacionPotenciaFirmeRepository
    {
        public PfrRelacionPotenciaFirmeRepository(string strConn): base(strConn)
        {
        }

        PfrRelacionPotenciaFirmeHelper helper = new PfrRelacionPotenciaFirmeHelper();

        //public int Save(PfrRelacionPotenciaFirmeDTO entity)
        //{
        //    DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
        //    object result = dbProvider.ExecuteScalar(command);
        //    int id = 1;
        //    if (result != null)id = Convert.ToInt32(result);

        //    command = dbProvider.GetSqlStringCommand(helper.SqlSave);

        //    dbProvider.AddInParameter(command, helper.Pfrrpfcodi, DbType.Int32, id);
        //    dbProvider.AddInParameter(command, helper.Pfrrptcodi, DbType.Int32, entity.Pfrrptcodi);
        //    dbProvider.AddInParameter(command, helper.Pfrptcodi, DbType.Int32, entity.Pfrptcodi);

        //    dbProvider.ExecuteNonQuery(command);
        //    return id;
        //}

        public int Save(PfrRelacionPotenciaFirmeDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfrrpfcodi, DbType.Int32, id));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfrrptcodi, DbType.Int32, entity.Pfrrptcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfrptcodi, DbType.Int32, entity.Pfrptcodi));

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PfrRelacionPotenciaFirmeDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Pfrrpfcodi, DbType.Int32, entity.Pfrrpfcodi);
            dbProvider.AddInParameter(command, helper.Pfrrptcodi, DbType.Int32, entity.Pfrrptcodi);
            dbProvider.AddInParameter(command, helper.Pfrptcodi, DbType.Int32, entity.Pfrptcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int pfrrpfcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Pfrrpfcodi, DbType.Int32, pfrrpfcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PfrRelacionPotenciaFirmeDTO GetById(int pfrrpfcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Pfrrpfcodi, DbType.Int32, pfrrpfcodi);
            PfrRelacionPotenciaFirmeDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PfrRelacionPotenciaFirmeDTO> List()
        {
            List<PfrRelacionPotenciaFirmeDTO> entitys = new List<PfrRelacionPotenciaFirmeDTO>();
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

        public List<PfrRelacionPotenciaFirmeDTO> GetByCriteria(int pfrrptcodi)
        {
            List<PfrRelacionPotenciaFirmeDTO> entitys = new List<PfrRelacionPotenciaFirmeDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Pfrrptcodi, DbType.Int32, pfrrptcodi);

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
