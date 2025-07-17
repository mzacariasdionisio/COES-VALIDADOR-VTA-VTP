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
   public  class CodigoRetiroSinContratoRespository : RepositoryBase, ICodigoRetiroSinContratoRepository
    {     
        public CodigoRetiroSinContratoRespository(string strConn)   : base(strConn)
        {
        }

        CodigoRetiroSinContratoHelper helper = new CodigoRetiroSinContratoHelper();

        public int Save(CodigoRetiroSinContratoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.CODRETISINCONCODI, DbType.Int32, GetCodigoGenerado());
            dbProvider.AddInParameter(command, helper.CLICODI, DbType.Int32, entity.CliCodi);
            dbProvider.AddInParameter(command, helper.BARRCODI, DbType.Int32, entity.BarrCodi);         
            dbProvider.AddInParameter(command, helper.CODRETISINCONCODIGO, DbType.String, entity.CodRetiSinConCodigo);
            dbProvider.AddInParameter(command, helper.CODRETISINCONIFECHAINICIO, DbType.DateTime, entity.CodRetiSinConFechaInicio);
            dbProvider.AddInParameter(command, helper.CODRETISINCONFECHAFIN, DbType.DateTime, entity.CodRetiSinConFechaFin);
            dbProvider.AddInParameter(command, helper.CODRETISINCONESTADO, DbType.String, entity.CodRetiSinConEstado);
            dbProvider.AddInParameter(command, helper.CODRETISINCONUSERNAME, DbType.String, entity.CodRetiSinConUserName);
            dbProvider.AddInParameter(command, helper.CODRETISINCONFECINS, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.GENEMPRCODI, DbType.Int32, entity.GenEmprCodi);
            dbProvider.AddInParameter(command, helper.TIPUSUCODI, DbType.Int32, entity.TipUsuCodi);

            return dbProvider.ExecuteNonQuery(command);
        }

        public void Update(CodigoRetiroSinContratoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.CLICODI, DbType.Int32, entity.CliCodi);
            dbProvider.AddInParameter(command, helper.BARRCODI, DbType.Int32, entity.BarrCodi);
            dbProvider.AddInParameter(command, helper.CODRETISINCONCODIGO, DbType.String, entity.CodRetiSinConCodigo);      
            dbProvider.AddInParameter(command, helper.CODRETISINCONIFECHAINICIO, DbType.DateTime, entity.CodRetiSinConFechaInicio);
            dbProvider.AddInParameter(command, helper.CODRETISINCONFECHAFIN, DbType.DateTime, entity.CodRetiSinConFechaFin);
            dbProvider.AddInParameter(command, helper.CODRETISINCONESTADO, DbType.String, entity.CodRetiSinConEstado);
            dbProvider.AddInParameter(command, helper.CODRETISINCONFECACT, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.TIPUSUCODI, DbType.Int32, entity.TipUsuCodi);
            //dbProvider.AddInParameter(command, helper.GENEMPRCODI, DbType.Int32, entity.Genemprcodi);
            dbProvider.AddInParameter(command, helper.CODRETISINCONCODI, DbType.Int32, entity.CodRetiSinConCodi);
            

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(System.Int32 id)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.CODRETISINCONCODI, DbType.Int32, id);

            dbProvider.ExecuteNonQuery(command);
        }

        public CodigoRetiroSinContratoDTO GetById(System.Int32 id)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.CODRETISINCONCODI, DbType.Int32, id);
            CodigoRetiroSinContratoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CodigoRetiroSinContratoDTO> List()
        {
            List<CodigoRetiroSinContratoDTO> entitys = new List<CodigoRetiroSinContratoDTO>();
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

        public List<CodigoRetiroSinContratoDTO> GetByCriteria(string nombreCli, string nombreBarra, DateTime? fechaini, DateTime? fechafin, string estado,string codretirosc, int NroPagina, int PageSize)
        {
            List<CodigoRetiroSinContratoDTO> entitys = new List<CodigoRetiroSinContratoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            dbProvider.AddInParameter(command, helper.CLINOMBRE, DbType.String, nombreCli);
            dbProvider.AddInParameter(command, helper.CLINOMBRE, DbType.String, nombreCli);
            dbProvider.AddInParameter(command, helper.BARRNOMBBARRTRAN, DbType.String, nombreBarra);
            dbProvider.AddInParameter(command, helper.BARRNOMBBARRTRAN, DbType.String, nombreBarra);
            dbProvider.AddInParameter(command, helper.CODRETISINCONIFECHAINICIO, DbType.DateTime, fechaini);
            dbProvider.AddInParameter(command, helper.CODRETISINCONIFECHAINICIO, DbType.DateTime, fechaini);
            dbProvider.AddInParameter(command, helper.CODRETISINCONFECHAFIN, DbType.DateTime, fechafin);
            dbProvider.AddInParameter(command, helper.CODRETISINCONFECHAFIN, DbType.DateTime, fechafin);
            dbProvider.AddInParameter(command, helper.CODRETISINCONESTADO, DbType.String, estado);
            dbProvider.AddInParameter(command, helper.CODRETISINCONESTADO, DbType.String, estado);
            dbProvider.AddInParameter(command, helper.CODRETISINCONCODIGO, DbType.String, codretirosc);
            dbProvider.AddInParameter(command, helper.CODRETISINCONCODIGO, DbType.String, codretirosc);
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

        public int ObtenerNroRegistros(string nombreCli, string nombreBarra, DateTime? fechaini, DateTime? fechafin, string estado,string codretirosc)
        {
            int NroRegistros = 0; 
            List<CodigoRetiroSinContratoDTO> entitys = new List<CodigoRetiroSinContratoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlTotalRecords);

            dbProvider.AddInParameter(command, helper.CLINOMBRE, DbType.String, nombreCli);
            dbProvider.AddInParameter(command, helper.CLINOMBRE, DbType.String, nombreCli);
            dbProvider.AddInParameter(command, helper.BARRNOMBBARRTRAN, DbType.String, nombreBarra);
            dbProvider.AddInParameter(command, helper.BARRNOMBBARRTRAN, DbType.String, nombreBarra);
            dbProvider.AddInParameter(command, helper.CODRETISINCONIFECHAINICIO, DbType.DateTime, fechaini);
            dbProvider.AddInParameter(command, helper.CODRETISINCONIFECHAINICIO, DbType.DateTime, fechaini);
            dbProvider.AddInParameter(command, helper.CODRETISINCONFECHAFIN, DbType.DateTime, fechafin);
            dbProvider.AddInParameter(command, helper.CODRETISINCONFECHAFIN, DbType.DateTime, fechafin);
            dbProvider.AddInParameter(command, helper.CODRETISINCONESTADO, DbType.String, estado);
            dbProvider.AddInParameter(command, helper.CODRETISINCONESTADO, DbType.String, estado);
            dbProvider.AddInParameter(command, helper.CODRETISINCONCODIGO, DbType.String, codretirosc);
            dbProvider.AddInParameter(command, helper.CODRETISINCONCODIGO, DbType.String, codretirosc);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    NroRegistros = Convert.ToInt32(dr["NroRegistros"].ToString().Trim());
                }
            }

            return NroRegistros;
        }

        public CodigoRetiroSinContratoDTO GetByCodigoRetiroSinContratoCodigo(System.String Codretisinconcodigo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlCodigoRetiroSinContratoCodigo);
            dbProvider.AddInParameter(command, helper.CODRETISINCONCODIGO, DbType.String, Codretisinconcodigo);
            CodigoRetiroSinContratoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
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
