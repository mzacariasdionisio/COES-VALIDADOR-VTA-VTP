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
    /// Clase de acceso a datos de la tabla VTP_INGRESO_TARIFARIO_AJUSTE
    /// </summary>
    public class VtpIngresoTarifarioAjusteRepository: RepositoryBase, IVtpIngresoTarifarioAjusteRepository
    {
        public VtpIngresoTarifarioAjusteRepository(string strConn): base(strConn)
        {
        }

        VtpIngresoTarifarioAjusteHelper helper = new VtpIngresoTarifarioAjusteHelper();

        public int Save(VtpIngresoTarifarioAjusteDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Ingtajcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Emprcodiping, DbType.Int32, entity.Emprcodiping);
            dbProvider.AddInParameter(command, helper.Pingcodi, DbType.Int32, entity.Pingcodi);
            dbProvider.AddInParameter(command, helper.Emprcodingpot, DbType.Int32, entity.Emprcodingpot);
            dbProvider.AddInParameter(command, helper.Ingtajajuste, DbType.Decimal, entity.Ingtajajuste);
            dbProvider.AddInParameter(command, helper.Ingtajusucreacion, DbType.String, entity.Ingtajusucreacion);
            dbProvider.AddInParameter(command, helper.Ingtajfeccreacion, DbType.DateTime, DateTime.Now);
            
            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(VtpIngresoTarifarioAjusteDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Ingtajajuste, DbType.Decimal, entity.Ingtajajuste);
            dbProvider.AddInParameter(command, helper.Ingtajcodi, DbType.Int32, entity.Ingtajcodi);
            
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int Ingtajcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Ingtajcodi, DbType.Int32, Ingtajcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteByCriteria(int pericodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteByCriteria);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public VtpIngresoTarifarioAjusteDTO GetById(int Ingtajcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Ingtajcodi, DbType.Int32, Ingtajcodi);
            VtpIngresoTarifarioAjusteDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VtpIngresoTarifarioAjusteDTO> List()
        {
            List<VtpIngresoTarifarioAjusteDTO> entitys = new List<VtpIngresoTarifarioAjusteDTO>();
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

        public List<VtpIngresoTarifarioAjusteDTO> GetByCriteria(int pericodi)
        {
            List<VtpIngresoTarifarioAjusteDTO> entitys = new List<VtpIngresoTarifarioAjusteDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpIngresoTarifarioAjusteDTO entity = helper.Create(dr);

                    int iEmprnombping = dr.GetOrdinal(this.helper.Emprnombping);
                    if (!dr.IsDBNull(iEmprnombping)) entity.Emprnombping = dr.GetString(iEmprnombping);

                    int iPingnombre = dr.GetOrdinal(this.helper.Pingnombre);
                    if (!dr.IsDBNull(iPingnombre)) entity.Pingnombre = dr.GetString(iPingnombre);

                    int iEmprnombingpot = dr.GetOrdinal(this.helper.Emprnombingpot);
                    if (!dr.IsDBNull(iEmprnombingpot)) entity.Emprnombingpot = dr.GetString(iEmprnombingpot);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public decimal GetAjuste(int pericodi, int emprnombping, int pingcodi, int emprnombingpot)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetAjuste);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Emprcodiping, DbType.Int32, emprnombping);
            dbProvider.AddInParameter(command, helper.Pingcodi, DbType.Int32, pingcodi);
            dbProvider.AddInParameter(command, helper.Emprcodingpot, DbType.Int32, emprnombingpot);
            
            object result = dbProvider.ExecuteScalar(command);
            decimal dSaldo = 0;
            if (result != null) dSaldo = Convert.ToDecimal(result);
            return dSaldo;
        }
    }
}
