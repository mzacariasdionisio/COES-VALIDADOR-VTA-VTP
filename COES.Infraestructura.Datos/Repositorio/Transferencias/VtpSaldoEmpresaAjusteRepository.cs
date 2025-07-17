using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Infraestructura.Datos.Helper.Transferencias;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase de acceso a datos de la tabla VTP_SALDO_EMPRESA_AJUSTE
    /// </summary>
    public class VtpSaldoEmpresaAjusteRepository: RepositoryBase, IVtpSaldoEmpresaAjusteRepository
    {
        public VtpSaldoEmpresaAjusteRepository(string strConn): base(strConn)
        {
        }

        VtpSaldoEmpresaAjusteHelper helper = new VtpSaldoEmpresaAjusteHelper();

        public int Save(VtpSaldoEmpresaAjusteDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Potseacodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Potseaajuste, DbType.Decimal, entity.Potseaajuste);
            dbProvider.AddInParameter(command, helper.Potseausucreacion, DbType.String, entity.Potseausucreacion);
            dbProvider.AddInParameter(command, helper.Potseafeccreacion, DbType.DateTime, DateTime.Now);
            
            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(VtpSaldoEmpresaAjusteDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Potseaajuste, DbType.Decimal, entity.Potseaajuste);
            
            dbProvider.AddInParameter(command, helper.Potseacodi, DbType.Int32, entity.Potseacodi);
            
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int Potseacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Potseacodi, DbType.Int32, Potseacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteByCriteria(int pericodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteByCriteria);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public VtpSaldoEmpresaAjusteDTO GetById(int Potseacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Potseacodi, DbType.Int32, Potseacodi);
            VtpSaldoEmpresaAjusteDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VtpSaldoEmpresaAjusteDTO> List()
        {
            List<VtpSaldoEmpresaAjusteDTO> entitys = new List<VtpSaldoEmpresaAjusteDTO>();
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

        public List<VtpSaldoEmpresaAjusteDTO> GetByCriteria(int pericodi)
        {
            List<VtpSaldoEmpresaAjusteDTO> entitys = new List<VtpSaldoEmpresaAjusteDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpSaldoEmpresaAjusteDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public decimal GetAjuste(int pericodi, int emprcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetAjuste);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);

            object result = dbProvider.ExecuteScalar(command);
            decimal dSaldo = 0;
            if (result != null) dSaldo = Convert.ToDecimal(result);
            return dSaldo;
        }
    }
}
