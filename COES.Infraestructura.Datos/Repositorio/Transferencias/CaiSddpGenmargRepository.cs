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
    /// Clase de acceso a datos de la tabla CAI_SDDP_GENMARG
    /// </summary>
    public class CaiSddpGenmargRepository: RepositoryBase, ICaiSddpGenmargRepository
    {
        public CaiSddpGenmargRepository(string strConn): base(strConn)
        {
        }

        CaiSddpGenmargHelper helper = new CaiSddpGenmargHelper();

        public int Save(CaiSddpGenmargDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Sddpgmcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Caiajcodi, DbType.Int32, entity.Caiajcodi);
            dbProvider.AddInParameter(command, helper.Sddpgmtipo, DbType.String, entity.Sddpgmtipo);
            dbProvider.AddInParameter(command, helper.Sddpgmetapa, DbType.Int32, entity.Sddpgmetapa);
            dbProvider.AddInParameter(command, helper.Sddpgmserie, DbType.Int32, entity.Sddpgmserie);
            dbProvider.AddInParameter(command, helper.Sddpgmbloque, DbType.Int32, entity.Sddpgmbloque);
            dbProvider.AddInParameter(command, helper.Sddpgmnombre, DbType.String, entity.Sddpgmnombre);
            dbProvider.AddInParameter(command, helper.Sddpgmenergia, DbType.Int32, entity.Sddpgmenergia);
            dbProvider.AddInParameter(command, helper.Sddpgmfecha, DbType.DateTime, entity.Sddpgmfecha);
            dbProvider.AddInParameter(command, helper.Sddpgmusucreacion, DbType.String, entity.Sddpgmusucreacion);
            dbProvider.AddInParameter(command, helper.Sddpgmfeccreacion, DbType.DateTime, entity.Sddpgmfeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CaiSddpGenmargDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Caiajcodi, DbType.Int32, entity.Caiajcodi);
            dbProvider.AddInParameter(command, helper.Sddpgmtipo, DbType.String, entity.Sddpgmtipo);
            dbProvider.AddInParameter(command, helper.Sddpgmetapa, DbType.Int32, entity.Sddpgmetapa);
            dbProvider.AddInParameter(command, helper.Sddpgmserie, DbType.Int32, entity.Sddpgmserie);
            dbProvider.AddInParameter(command, helper.Sddpgmbloque, DbType.Int32, entity.Sddpgmbloque);
            dbProvider.AddInParameter(command, helper.Sddpgmnombre, DbType.String, entity.Sddpgmnombre);
            dbProvider.AddInParameter(command, helper.Sddpgmenergia, DbType.Int32, entity.Sddpgmenergia);
            dbProvider.AddInParameter(command, helper.Sddpgmfecha, DbType.DateTime, entity.Sddpgmfecha);
            dbProvider.AddInParameter(command, helper.Sddpgmusucreacion, DbType.String, entity.Sddpgmusucreacion);
            dbProvider.AddInParameter(command, helper.Sddpgmfeccreacion, DbType.DateTime, entity.Sddpgmfeccreacion);
            dbProvider.AddInParameter(command, helper.Sddpgmcodi, DbType.Int32, entity.Sddpgmcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(string tipo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Sddpgmtipo, DbType.String, tipo);

            dbProvider.ExecuteNonQuery(command);
        }

        public CaiSddpGenmargDTO GetById(int sddpgmcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Sddpgmcodi, DbType.Int32, sddpgmcodi);
            CaiSddpGenmargDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CaiSddpGenmargDTO> List()
        {
            List<CaiSddpGenmargDTO> entitys = new List<CaiSddpGenmargDTO>();
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

        public List<CaiSddpGenmargDTO> GetByCriteria(string sddpgmtipo)
        {
            List<CaiSddpGenmargDTO> entitys = new List<CaiSddpGenmargDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            dbProvider.AddInParameter(command, helper.Sddpgmtipo, DbType.String, sddpgmtipo);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<CaiSddpGenmargDTO> GetByCriteriaCaiSddpGenmargsBarrNoIns(string sddpgmtipo)
        {
            List<CaiSddpGenmargDTO> entitys = new List<CaiSddpGenmargDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.GetByCriteriaCaiSddpGenmargsBarrNoIns);

            dbProvider.AddInParameter(command, helper.Sddpgmtipo, DbType.String, sddpgmtipo);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }
               

        public int GetCodigoGenerado()
        {
            int newId = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlCodigoGenerado);
            newId = Int32.Parse(dbProvider.ExecuteScalar(command).ToString());

            return newId;
        }

        public decimal GetSumSddpGenmargByEtapaB1(int sddpgmetapa, string sddpgmnombre)
        {
            decimal suma = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.GetSumSddpGenmargByEtapaB1);

            dbProvider.AddInParameter(command, helper.Sddpgmetapa, DbType.Int32, sddpgmetapa);
            dbProvider.AddInParameter(command, helper.Sddpgmnombre, DbType.String, sddpgmnombre);

            suma = Decimal.Parse(dbProvider.ExecuteScalar(command).ToString());

            return suma;
        }

        public decimal GetSumSddpGenmargByEtapaB2(int sddpgmetapa, string sddpgmnombre)
        {
            decimal suma = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.GetSumSddpGenmargByEtapaB2);

            dbProvider.AddInParameter(command, helper.Sddpgmetapa, DbType.Int32, sddpgmetapa);
            dbProvider.AddInParameter(command, helper.Sddpgmnombre, DbType.String, sddpgmnombre);

            suma = Decimal.Parse(dbProvider.ExecuteScalar(command).ToString());

            return suma;
        }

        public decimal GetSumSddpGenmargByEtapaB3(int sddpgmetapa, string sddpgmnombre)
        {
            decimal suma = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.GetSumSddpGenmargByEtapaB3);

            dbProvider.AddInParameter(command, helper.Sddpgmetapa, DbType.Int32, sddpgmetapa);
            dbProvider.AddInParameter(command, helper.Sddpgmnombre, DbType.String, sddpgmnombre);

            suma = Decimal.Parse(dbProvider.ExecuteScalar(command).ToString());

            return suma;
        }

        public decimal GetSumSddpGenmargByEtapaB4(int sddpgmetapa, string sddpgmnombre)
        {
            decimal suma = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.GetSumSddpGenmargByEtapaB4);

            dbProvider.AddInParameter(command, helper.Sddpgmetapa, DbType.Int32, sddpgmetapa);
            dbProvider.AddInParameter(command, helper.Sddpgmnombre, DbType.String, sddpgmnombre);

            suma = Decimal.Parse(dbProvider.ExecuteScalar(command).ToString());

            return suma;
        }

        public decimal GetSumSddpGenmargByEtapaB5(int sddpgmetapa, string sddpgmnombre)
        {
            decimal suma = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.GetSumSddpGenmargByEtapaB5);

            dbProvider.AddInParameter(command, helper.Sddpgmetapa, DbType.Int32, sddpgmetapa);
            dbProvider.AddInParameter(command, helper.Sddpgmnombre, DbType.String, sddpgmnombre);

            suma = Decimal.Parse(dbProvider.ExecuteScalar(command).ToString());

            return suma;
        }

        public decimal GetSumSddpGenmargByEtapa(string sddpgmtipo, int sddpgmetapa, int sddpgmbloque, string sddpgmnombre)
        {
            decimal suma = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.GetSumSddpGenmargByEtapa);

            dbProvider.AddInParameter(command, helper.Sddpgmtipo, DbType.String, sddpgmtipo);
            dbProvider.AddInParameter(command, helper.Sddpgmetapa, DbType.Int32, sddpgmetapa);
            dbProvider.AddInParameter(command, helper.Sddpgmbloque, DbType.Int32, sddpgmbloque);
            dbProvider.AddInParameter(command, helper.Sddpgmnombre, DbType.String, sddpgmnombre);


            suma = Decimal.Parse(dbProvider.ExecuteScalar(command).ToString());

            return suma;
        }

        public void BulkInsert(List<CaiSddpGenmargDTO> entitys)
        {
            dbProvider.AddColumnMapping(helper.Sddpgmcodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Sddpgmtipo, DbType.String);
            dbProvider.AddColumnMapping(helper.Caiajcodi, DbType.Int32);            
            dbProvider.AddColumnMapping(helper.Sddpgmetapa, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Sddpgmserie, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Sddpgmbloque, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Sddpgmnombre, DbType.String);
            dbProvider.AddColumnMapping(helper.Sddpgmenergia, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Sddpgmfecha, DbType.DateTime);
            dbProvider.AddColumnMapping(helper.Sddpgmusucreacion, DbType.String);
            dbProvider.AddColumnMapping(helper.Sddpgmfeccreacion, DbType.DateTime);


            dbProvider.BulkInsert<CaiSddpGenmargDTO>(entitys, helper.TableName);
        }
    }
}
