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
    public class CodigoInfoBaseRepository : RepositoryBase, ICodigoInfoBaseRepository
    {   
        public CodigoInfoBaseRepository(string strConn) : base(strConn)
        {
        }

        CodigoInfoBaseHelper helper = new CodigoInfoBaseHelper();

        public int Save(CodigoInfoBaseDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);
       
            dbProvider.AddInParameter(command, helper.COINFBCODI, DbType.Int32, GetCodigoGenerado());
            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, entity.EmprCodi);
            dbProvider.AddInParameter(command, helper.BARRCODI, DbType.Int32, entity.BarrCodi);
            dbProvider.AddInParameter(command, helper.CENTGENECODI, DbType.Int32, entity.CentGeneCodi);
            dbProvider.AddInParameter(command, helper.COINFBCODIGO, DbType.String, entity.CoInfBCodigo);
            dbProvider.AddInParameter(command, helper.COINFBFECHAINICIO, DbType.DateTime,  entity.CoInfBFechaInicio);
            dbProvider.AddInParameter(command, helper.COINFBFECHAFIN, DbType.DateTime, entity.CoInfBFechaFin);
            dbProvider.AddInParameter(command, helper.COINFBESTADO, DbType.String, entity.CoInfBEstado);
            dbProvider.AddInParameter(command, helper.COINFBUSERNAME, DbType.String, entity.CoInfBUserName);
            dbProvider.AddInParameter(command, helper.COINFBFECINS, DbType.DateTime, DateTime.Now.Date);

            return dbProvider.ExecuteNonQuery(command);
        }
  
        public void Update(CodigoInfoBaseDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, entity.EmprCodi);
            dbProvider.AddInParameter(command, helper.BARRCODI, DbType.Int32, entity.BarrCodi);
            dbProvider.AddInParameter(command, helper.CENTGENECODI, DbType.Int32, entity.CentGeneCodi);
            dbProvider.AddInParameter(command, helper.COINFBCODIGO, DbType.String, entity.CoInfBCodigo);
            dbProvider.AddInParameter(command, helper.COINFBFECHAINICIO, DbType.DateTime, entity.CoInfBFechaInicio);
            dbProvider.AddInParameter(command, helper.COINFBFECHAFIN, DbType.DateTime, entity.CoInfBFechaFin);
            dbProvider.AddInParameter(command, helper.COINFBESTADO, DbType.String, entity.CoInfBEstado);
            dbProvider.AddInParameter(command, helper.COINFBFECACT, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.COINFBCODI, DbType.Int32, entity.CoInfBCodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(System.Int32 id)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
      
            dbProvider.AddInParameter(command, helper.COINFBCODI, DbType.Int32, id);

            dbProvider.ExecuteNonQuery(command);
        }

        public CodigoInfoBaseDTO GetById(System.Int32 id)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.COINFBCODI, DbType.Int32, id);
            CodigoInfoBaseDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CodigoInfoBaseDTO> List()
        {
            List<CodigoInfoBaseDTO> entitys = new List<CodigoInfoBaseDTO>();
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

        public List<CodigoInfoBaseDTO> GetByCriteria(string nombreEmp, string centralgene, string barrTrans, DateTime? fechini, DateTime? fechafin, string estado,string codinfobase, int NroPagina, int PageSize)
        {      
            List<CodigoInfoBaseDTO> entitys = new List<CodigoInfoBaseDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            dbProvider.AddInParameter(command, helper.EMPRNOMB, DbType.String, nombreEmp);
            dbProvider.AddInParameter(command, helper.EMPRNOMB, DbType.String, nombreEmp);
            dbProvider.AddInParameter(command, helper.BARRNOMBBARRTRAN, DbType.String, barrTrans);
            dbProvider.AddInParameter(command, helper.BARRNOMBBARRTRAN, DbType.String, barrTrans);
            dbProvider.AddInParameter(command, helper.CENTGENENOMBRE, DbType.String, centralgene);
            dbProvider.AddInParameter(command, helper.CENTGENENOMBRE, DbType.String, centralgene);
            dbProvider.AddInParameter(command, helper.COINFBFECHAINICIO, DbType.DateTime, fechini);
            dbProvider.AddInParameter(command, helper.COINFBFECHAINICIO, DbType.DateTime, fechini);
            dbProvider.AddInParameter(command, helper.COINFBFECHAFIN, DbType.DateTime, fechafin);
            dbProvider.AddInParameter(command, helper.COINFBFECHAFIN, DbType.DateTime, fechafin);
            dbProvider.AddInParameter(command, helper.COINFBESTADO, DbType.String, estado);
            dbProvider.AddInParameter(command, helper.COINFBESTADO, DbType.String, estado);
            dbProvider.AddInParameter(command, helper.COINFBCODIGO, DbType.String, codinfobase);
            dbProvider.AddInParameter(command, helper.COINFBCODIGO, DbType.String, codinfobase);
            dbProvider.AddInParameter(command, helper.NROPAGINA, DbType.Int32, NroPagina);
            dbProvider.AddInParameter(command, helper.PAGESIZE, DbType.Int32, PageSize);
            dbProvider.AddInParameter(command, helper.NROPAGINA, DbType.Int32, NroPagina);
            dbProvider.AddInParameter(command, helper.PAGESIZE, DbType.Int32, PageSize);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public int ObtenerNroRegistros(string nombreEmp, string centralgene, string barrTrans, DateTime? fechini, DateTime? fechafin, string estado,string codinfobase)
        {
            int NroRegistros = 0;
            List<CodigoInfoBaseDTO> entitys = new List<CodigoInfoBaseDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlTotalRecords);

            dbProvider.AddInParameter(command, helper.EMPRNOMB, DbType.String, nombreEmp);
            dbProvider.AddInParameter(command, helper.EMPRNOMB, DbType.String, nombreEmp);
            dbProvider.AddInParameter(command, helper.BARRNOMBBARRTRAN, DbType.String, barrTrans);
            dbProvider.AddInParameter(command, helper.BARRNOMBBARRTRAN, DbType.String, barrTrans);
            dbProvider.AddInParameter(command, helper.CENTGENENOMBRE, DbType.String, centralgene);
            dbProvider.AddInParameter(command, helper.CENTGENENOMBRE, DbType.String, centralgene);
            dbProvider.AddInParameter(command, helper.COINFBFECHAINICIO, DbType.DateTime, fechini);
            dbProvider.AddInParameter(command, helper.COINFBFECHAINICIO, DbType.DateTime, fechini);
            dbProvider.AddInParameter(command, helper.COINFBFECHAFIN, DbType.DateTime, fechafin);
            dbProvider.AddInParameter(command, helper.COINFBFECHAFIN, DbType.DateTime, fechafin);
            dbProvider.AddInParameter(command, helper.COINFBESTADO, DbType.String, estado);
            dbProvider.AddInParameter(command, helper.COINFBESTADO, DbType.String, estado);
            dbProvider.AddInParameter(command, helper.COINFBCODIGO, DbType.String, codinfobase);
            dbProvider.AddInParameter(command, helper.COINFBCODIGO, DbType.String, codinfobase);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    NroRegistros = Convert.ToInt32(dr["NroRegistros"].ToString().Trim());
                }
            }

            return NroRegistros;
        }

        public int GetCodigoGenerado()
        {
            int newId = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlCodigoGenerado);
            newId = Int32.Parse(dbProvider.ExecuteScalar(command).ToString());

            return newId;
        }
 
        public CodigoInfoBaseDTO GetByCoInfBCodigo(System.String codientrCodigo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlCodigoInfoBaseCodigo);
            dbProvider.AddInParameter(command, helper.COINFBCODIGO, DbType.String, codientrCodigo);
            CodigoInfoBaseDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
            if (dr.Read())
            {
                entity = helper.Create(dr);
            }
            }

            return entity;
        }

        public CodigoInfoBaseDTO CodigoInfoBaseVigenteByPeriodo(int iPericodi, System.String sCodigo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlCodigoInfoBaseVigenteByPeriodo);
            dbProvider.AddInParameter(command, helper.COINFBCODIGO, DbType.String, sCodigo);
            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, iPericodi);
            CodigoInfoBaseDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }
    }
}
