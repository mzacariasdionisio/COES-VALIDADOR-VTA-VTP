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
    public class SaldoEmpresaRepository : RepositoryBase, ISaldoEmpresaRepository
    {
        public SaldoEmpresaRepository(string strConn)
            : base(strConn)
        {
        }

        SaldoEmpresaHelper helper = new SaldoEmpresaHelper();

        public int Save(SaldoEmpresaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.SALEMPCODI, DbType.Int32, GetCodigoGenerado());
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, entity.PeriCodi);
            dbProvider.AddInParameter(command, helper.EMPCODI, DbType.Int32, entity.EmpCodi);
            dbProvider.AddInParameter(command, helper.SALEMPVERSION, DbType.Int32, entity.SalEmpVersion);
            dbProvider.AddInParameter(command, helper.SALEMPSALDO, DbType.Decimal, entity.SalEmpSaldo);   
            dbProvider.AddInParameter(command, helper.SALEMPFECINS, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.SALEMPUSERNAME, DbType.String, entity.SalEmpUserName);

            return dbProvider.ExecuteNonQuery(command);
        }

        public void Update(SaldoEmpresaDTO entity)
        {
            //DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            //dbProvider.AddInParameter(command, helper.Barrnombre, DbType.String, entity.Barrnombre);
            //dbProvider.AddInParameter(command, helper.Barrtension, DbType.String, entity.Barrtension);
            //dbProvider.AddInParameter(command, helper.Barrpuntosumirer, DbType.String, entity.Barrpuntosumirer);
            //dbProvider.AddInParameter(command, helper.Barrbarrabgr, DbType.String, entity.Barrbarrabgr);
            //dbProvider.AddInParameter(command, helper.Barrflagbarrtran, DbType.String, entity.Barrflagbarrtran);
            //dbProvider.AddInParameter(command, helper.Barrnombbarrtran, DbType.String, entity.Barrnombbarrtran);
            //dbProvider.AddInParameter(command, helper.Barrestado, DbType.String, entity.Barrestado);
            //dbProvider.AddInParameter(command, helper.Barrfecact, DbType.DateTime, DateTime.Now);
            //dbProvider.AddInParameter(command, helper.BarrCodi, DbType.Int32, entity.BarrCodi);

            //dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int pericodi,int version)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
         
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.SALEMPVERSION, DbType.Int32, version);

            dbProvider.ExecuteNonQuery(command);

    
        }
              
        public int GetCodigoGenerado()
        {
            int newId = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlCodigoGenerado);
            newId = Int32.Parse(dbProvider.ExecuteScalar(command).ToString());

            return newId;
        }
    }
}
