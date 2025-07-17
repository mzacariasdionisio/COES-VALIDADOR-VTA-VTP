using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla IIO_SICLI_OSIG_FACTURA
    /// </summary>
    public class IioSicliOsigFacturaRepository : RepositoryBase, IIioSicliOsigFacturaRepository
    {
        public IioSicliOsigFacturaRepository(string strConn)
            : base(strConn)
        {
        }

        IioSicliOsigFacturaHelper helper = new IioSicliOsigFacturaHelper();

        public int Save(IioSicliOsigFacturaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            //object result = dbProvider.ExecuteScalar(command);
            //int id = 1;
            //if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Clofaccodi, DbType.Int32, entity.Clofaccodi);
            dbProvider.AddInParameter(command, helper.Clofacanhiomes, DbType.String, entity.Clofacanhiomes);
            dbProvider.AddInParameter(command, helper.Clofaccodempresa, DbType.String, entity.Clofaccodempresa);
            dbProvider.AddInParameter(command, helper.Clofacnomempresa, DbType.String, entity.Clofacnomempresa);
            dbProvider.AddInParameter(command, helper.Clofacruc, DbType.String, entity.Clofacruc);
            dbProvider.AddInParameter(command, helper.Clofaccodcliente, DbType.String, entity.Clofaccodcliente);
            dbProvider.AddInParameter(command, helper.Clofacnomcliente, DbType.String, entity.Clofacnomcliente);
            dbProvider.AddInParameter(command, helper.Clofaccodbarrasumin, DbType.String, entity.Clofaccodbarrasumin);
            dbProvider.AddInParameter(command, helper.Clofacnombarrasumin, DbType.String, entity.Clofacnombarrasumin);
            dbProvider.AddInParameter(command, helper.Clofactensionentrega, DbType.Decimal, entity.Clofactensionentrega);
            dbProvider.AddInParameter(command, helper.Clofaccodbrg, DbType.String, entity.Clofaccodbrg);
            dbProvider.AddInParameter(command, helper.Clofacnombrg, DbType.String, entity.Clofacnombrg);
            dbProvider.AddInParameter(command, helper.Clofactensionbrg, DbType.Decimal, entity.Clofactensionbrg);
            dbProvider.AddInParameter(command, helper.Clofacphpbe, DbType.Decimal, entity.Clofacphpbe);
            dbProvider.AddInParameter(command, helper.Clofacpfpbe, DbType.Decimal, entity.Clofacpfpbe);
            dbProvider.AddInParameter(command, helper.Clofacehpbe, DbType.Decimal, entity.Clofacehpbe);
            dbProvider.AddInParameter(command, helper.Clofacefpbe, DbType.Decimal, entity.Clofacefpbe);

            dbProvider.AddInParameter(command, helper.Clofacusucreacion, DbType.String, entity.Clofacusucreacion);
            dbProvider.AddInParameter(command, helper.Clofacfeccreacion, DbType.DateTime, entity.Clofacfeccreacion);


            dbProvider.ExecuteNonQuery(command);
            return entity.Clofaccodi;
        }
                

        public void Delete(string periodo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Clofacanhiomes, DbType.String, periodo);

            dbProvider.ExecuteNonQuery(command);
        }

        public int GetMaxId()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        //Reportes

        public int GetCountTotal(string periodo)
        {
            var queryString = string.Format(helper.SqlGetCountTotal, periodo);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            object result = dbProvider.ExecuteScalar(command);
            int id = 0;
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int GetCountTotalFactura(string periodo)
        {
            var queryString = string.Format(helper.SqlGetCountTotalFactura, periodo);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            object result = dbProvider.ExecuteScalar(command);
            int id = 0;
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int GetCountTotalRuc(string periodo)
        {
            var queryString = string.Format(helper.SqlGetCountTotalRuc, periodo);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            object result = dbProvider.ExecuteScalar(command);
            int id = 0;
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int GetCountTotalFacturaRuc(string periodo)
        {
            var queryString = string.Format(helper.SqlGetCountTotalFacturaRuc, periodo);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            object result = dbProvider.ExecuteScalar(command);
            int id = 0;
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }
        public IDataReader ListRepCompCliente(string periodo)
        {
            
            var queryString = string.Format(helper.SqlListRepCompCliente, periodo);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            
            IDataReader reader = dbProvider.ExecuteReader(command);

            return reader;
        }

        public IDataReader ListRepCompEmpresa(string periodo)
        {

            var queryString = string.Format(helper.SqlListRepCompEmpresa, periodo);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            IDataReader reader = dbProvider.ExecuteReader(command);

            return reader;
        }

        public IDataReader ListRepCompHistorico(DateTime periodoInicio, DateTime periodoFin)
        {

            var queryString = helper.SqlListRepCompHistorico;
            var selectString = "";
            for (int i = 1; i <= 12; i++)
            {
                selectString = selectString + string.Format(",SUM(CASE WHEN UFACMESFACTURADO = '{0}' THEN CANTIDAD END) AS PS_{0} ", 
                    periodoInicio.AddMonths(i-1).ToString("yyyyMM"));
                selectString = selectString + string.Format(",SUM(CASE WHEN UFACMESFACTURADO = '{0}' THEN UFACMAXDEMHPPS END) AS MD_{0} ",
                    periodoInicio.AddMonths(i - 1).ToString("yyyyMM")); 
            }

            queryString = string.Format(queryString, selectString, periodoInicio.ToString("yyyyMM"), periodoFin.ToString("yyyyMM"));

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            IDataReader reader = dbProvider.ExecuteReader(command);

            return reader;
        }
       
       
    }
}
