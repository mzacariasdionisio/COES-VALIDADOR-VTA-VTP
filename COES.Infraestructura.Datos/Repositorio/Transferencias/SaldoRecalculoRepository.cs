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
    /// Clase que contiene las operaciones con la base de datos
    /// </summary>
    public class SaldoRecalculoRepository : RepositoryBase, ISaldoRecalculoRepository
    {
        public SaldoRecalculoRepository(string strConn) : base(strConn)
        {
        }

        SaldoRecalculoHelper helper = new SaldoRecalculoHelper();

        public int Save(SaldoRecalculoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.SALRECCODI, DbType.Int32, GetCodigoGenerado());
            dbProvider.AddInParameter(command, helper.EMPCODI, DbType.Int32, entity.EmpCodi);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, entity.PeriCodi);
            dbProvider.AddInParameter(command, helper.RECACODI, DbType.Int32, entity.RecaCodi);
            dbProvider.AddInParameter(command, helper.SALRECSALDO, DbType.Decimal, entity.SalRecSaldo);
            dbProvider.AddInParameter(command, helper.PERICODIDESTINO, DbType.Int32, entity.PeriCodiDestino);
            dbProvider.AddInParameter(command, helper.SALRECUSERNAME, DbType.String, entity.SalRecUserName);
            dbProvider.AddInParameter(command, helper.SALRECFECINS, DbType.DateTime, DateTime.Now);
            
            return dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int iPeriCodi, int iRecaCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, iPeriCodi);
            dbProvider.AddInParameter(command, helper.RECACODI, DbType.Int32, iRecaCodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public int GetCodigoGenerado()
        {
            int newId = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlCodigoGenerado);
            newId = Int32.Parse(dbProvider.ExecuteScalar(command).ToString());

            return newId;
        }

        public int GetByPericodiDestino(int iPeriCodi, int iRecaCodi)
        {
            int newId = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByPericodiDestino);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, iPeriCodi);
            dbProvider.AddInParameter(command, helper.RECACODI, DbType.Int32, iRecaCodi);
            try
            {
                newId = Int32.Parse(dbProvider.ExecuteScalar(command).ToString());
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                newId = 0;
            }
            return newId;
        }

        public void UpdatePericodiDestino(int iPeriCodi, int iRecaCodi, int iPeriCodiDestino)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdatePericodiDestino);
            dbProvider.AddInParameter(command, helper.PERICODIDESTINO, DbType.Int32, iPeriCodiDestino);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, iPeriCodi);
            dbProvider.AddInParameter(command, helper.RECACODI, DbType.Int32, iRecaCodi);

            dbProvider.ExecuteNonQuery(command);
        }
    }
}
