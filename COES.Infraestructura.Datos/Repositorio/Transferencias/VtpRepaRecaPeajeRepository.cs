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
    /// Clase de acceso a datos de la tabla VTP_REPA_RECA_PEAJE
    /// </summary>
    public class VtpRepaRecaPeajeRepository: RepositoryBase, IVtpRepaRecaPeajeRepository
    {
        public VtpRepaRecaPeajeRepository(string strConn): base(strConn)
        {
        }

        VtpRepaRecaPeajeHelper helper = new VtpRepaRecaPeajeHelper();

        public int Save(VtpRepaRecaPeajeDTO entity)
        {
            if (entity.Rrpecodi == 0)
            {
                DbCommand cmd = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
                dbProvider.AddInParameter(cmd, helper.Pericodi, DbType.Int32, entity.Pericodi);
                dbProvider.AddInParameter(cmd, helper.Recpotcodi, DbType.Int32, entity.Recpotcodi);
                object result = dbProvider.ExecuteScalar(cmd);
                if (result != null) entity.Rrpecodi = Convert.ToInt32(result);
            }
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Rrpecodi, DbType.Int32, entity.Rrpecodi);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, entity.Recpotcodi);
            dbProvider.AddInParameter(command, helper.Rrpenombre, DbType.String, entity.Rrpenombre);
            dbProvider.AddInParameter(command, helper.Rrpeusucreacion, DbType.String, entity.Rrpeusucreacion);
            dbProvider.AddInParameter(command, helper.Rrpefeccreacion, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.Rrpeusumodificacion, DbType.String, entity.Rrpeusumodificacion);
            dbProvider.AddInParameter(command, helper.Rrpefecmodificacion, DbType.DateTime, DateTime.Now);

            dbProvider.ExecuteNonQuery(command);
            return entity.Rrpecodi; 
        }

        public void Update(VtpRepaRecaPeajeDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

           
            dbProvider.AddInParameter(command, helper.Rrpenombre, DbType.String, entity.Rrpenombre);
            dbProvider.AddInParameter(command, helper.Rrpeusucreacion, DbType.String, entity.Rrpeusucreacion);
            dbProvider.AddInParameter(command, helper.Rrpefeccreacion, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.Rrpeusumodificacion, DbType.String, entity.Rrpeusumodificacion);
            dbProvider.AddInParameter(command, helper.Rrpefecmodificacion, DbType.DateTime, DateTime.Now);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, entity.Recpotcodi);
            dbProvider.AddInParameter(command, helper.Rrpecodi, DbType.Int32, entity.Rrpecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int pericodi, int recpotcodi, int rrpecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);
            dbProvider.AddInParameter(command, helper.Rrpecodi, DbType.Int32, rrpecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteByCriteria(int pericodi, int recpotcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteByCriteria);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public VtpRepaRecaPeajeDTO GetById(int pericodi, int recpotcodi, int rrpecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);
            dbProvider.AddInParameter(command, helper.Rrpecodi, DbType.Int32, rrpecodi);
            VtpRepaRecaPeajeDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public VtpRepaRecaPeajeDTO GetByNombre(int pericodi, int recpotcodi, string rrpenombre)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByNombre);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);
            dbProvider.AddInParameter(command, helper.Rrpenombre, DbType.String, rrpenombre);
            VtpRepaRecaPeajeDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VtpRepaRecaPeajeDTO> List()
        {
            List<VtpRepaRecaPeajeDTO> entitys = new List<VtpRepaRecaPeajeDTO>();
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
        
        public List<VtpRepaRecaPeajeDTO> GetByCriteria(int pericodi,int recpotcodi)
        {
            List<VtpRepaRecaPeajeDTO> entitys = new List<VtpRepaRecaPeajeDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);

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
