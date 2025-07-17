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
    public class RcgCostoMarginalCab : RepositoryBase, IRcgCostoMarginalCabRepository
    {
        public RcgCostoMarginalCab(string strConn)
            : base(strConn) 
        {
        }

        RcgCostoMarginalCabHelper helper = new RcgCostoMarginalCabHelper();    

        public int Save(RcgCostoMarginalCabDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            var codigoGenerado = GetCodigoGenerado();
            
            dbProvider.AddInParameter(command, helper.RCCMGCCODI, DbType.Int32, codigoGenerado);            
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.RECACODI, DbType.Int32, entity.Recacodi);

            dbProvider.AddInParameter(command, helper.RCCMGCUSUCREACION, DbType.String, entity.Rccmgcusucreacion);
            //dbProvider.AddInParameter(command, helper.EMPPAGOFECINS, DbType.DateTime, DateTime.Now);
            dbProvider.ExecuteNonQuery(command);

            return codigoGenerado;
        }

        public int Update(RcgCostoMarginalCabDTO entity)
        {
            //var sqlQuery = string.Format(helper.SqlUpdate, entity.Rccmgcusumodificacion);
            //DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
            //var codigoGenerado = GetCodigoGenerado();
            //dbProvider.AddInParameter(command, helper.RCCMGCCODI, DbType.Int32, entity.Rccmgccodi);            
            //dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, entity.PeriCodi);
            //dbProvider.AddInParameter(command, helper.RECACODI, DbType.Int32, entity.Recacodi);

            dbProvider.AddInParameter(command, helper.RCCMGCUSUMODIFICACION, DbType.String, entity.Rccmgcusumodificacion);
            //dbProvider.AddInParameter(command, helper.RCCMGCFECMODIFICACION, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.RCCMGCCODI, DbType.Int32, entity.Rccmgccodi); 
            

            var res = dbProvider.ExecuteNonQuery(command);

            return res;
        }           

        public int GetCodigoGenerado()
        {
            int newId = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            newId = Int32.Parse(dbProvider.ExecuteScalar(command).ToString());

            return newId;
        }


        public List<RcgCostoMarginalCabDTO> ListCostoMarginalCab(int pericodi, int recacodi)
        {
            List<RcgCostoMarginalCabDTO> entitys = new List<RcgCostoMarginalCabDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListCostoMarginalCab);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.RECACODI, DbType.Int32, recacodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        
    }
}
