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
    /// Clase de acceso a datos de la tabla VTP_PEAJE_CARGO_AJUSTE
    /// </summary>
    public class VtpPeajeCargoAjusteRepository: RepositoryBase, IVtpPeajeCargoAjusteRepository
    {
        public VtpPeajeCargoAjusteRepository(string strConn): base(strConn)
        {
        }

        VtpPeajeCargoAjusteHelper helper = new VtpPeajeCargoAjusteHelper();

        public int Save(VtpPeajeCargoAjusteDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Pecajcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Pingcodi, DbType.Int32, entity.Pingcodi);
            dbProvider.AddInParameter(command, helper.Pecajajuste, DbType.Decimal, entity.Pecajajuste);
            dbProvider.AddInParameter(command, helper.Pecajusucreacion, DbType.String, entity.Pecajusucreacion);
            dbProvider.AddInParameter(command, helper.Pecajfeccreacion, DbType.DateTime, DateTime.Now);
            
            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(VtpPeajeCargoAjusteDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Pecajajuste, DbType.Decimal, entity.Pecajajuste);
            
            dbProvider.AddInParameter(command, helper.Pecajcodi, DbType.Int32, entity.Pecajcodi);
            
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int Pecajcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Pecajcodi, DbType.Int32, Pecajcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteByCriteria(int pericodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteByCriteria);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public VtpPeajeCargoAjusteDTO GetById(int Pecajcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Pecajcodi, DbType.Int32, Pecajcodi);
            VtpPeajeCargoAjusteDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VtpPeajeCargoAjusteDTO> List()
        {
            List<VtpPeajeCargoAjusteDTO> entitys = new List<VtpPeajeCargoAjusteDTO>();
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

        public List<VtpPeajeCargoAjusteDTO> GetByCriteria(int pericodi)
        {
            List<VtpPeajeCargoAjusteDTO> entitys = new List<VtpPeajeCargoAjusteDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpPeajeCargoAjusteDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iPingnombre = dr.GetOrdinal(this.helper.Pingnombre);
                    if (!dr.IsDBNull(iPingnombre)) entity.Pingnombre = dr.GetString(iPingnombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public decimal GetAjuste(int pericodi, int emprcodi, int pingcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetAjuste);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Pingcodi, DbType.Int32, pingcodi);

            object result = dbProvider.ExecuteScalar(command);
            decimal dSaldo = 0;
            if (result != null) dSaldo = Convert.ToDecimal(result);
            return dSaldo;
        }
    }
}
