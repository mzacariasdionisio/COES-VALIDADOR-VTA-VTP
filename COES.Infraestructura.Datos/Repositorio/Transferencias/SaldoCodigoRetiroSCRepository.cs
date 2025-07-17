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
    public class SaldoCodigoRetiroSCRepository : RepositoryBase, ISaldoCodigoRetiroSCRepository
    {
        public SaldoCodigoRetiroSCRepository(string strConn)
            : base(strConn)
        {
        }

        SaldoCodigoRetiroSCHelper helper = new SaldoCodigoRetiroSCHelper();

        public int Save(SaldoCodigoRetiroscDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.SALRSCCODI, DbType.Int32, GetCodigoGenerado());
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, entity.PeriCodi);
            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, entity.EmprCodi);
            dbProvider.AddInParameter(command, helper.SALRSCVERSION, DbType.Int32, entity.SalrscVersion);
            dbProvider.AddInParameter(command, helper.SALRSCSALDO, DbType.Decimal, entity.SalrscsSaldo);
            dbProvider.AddInParameter(command, helper.SALRSCFECINS, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.SALRSCUSERNAME, DbType.String, entity.SalrscUserName);

            return dbProvider.ExecuteNonQuery(command);
        }

        public void Update(SaldoCodigoRetiroscDTO entity)
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

        public void Delete(int pericodi, int version)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
           
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.SALRSCVERSION, DbType.Int32, version);

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
