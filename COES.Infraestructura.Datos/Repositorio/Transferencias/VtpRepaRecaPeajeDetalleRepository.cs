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
    /// Clase de acceso a datos de la tabla VTP_REPA_RECA_PEAJE_DETALLE
    /// </summary>
    public class VtpRepaRecaPeajeDetalleRepository: RepositoryBase, IVtpRepaRecaPeajeDetalleRepository
    {
        public VtpRepaRecaPeajeDetalleRepository(string strConn): base(strConn)
        {
        }

        VtpRepaRecaPeajeDetalleHelper helper = new VtpRepaRecaPeajeDetalleHelper();

        public int Save(VtpRepaRecaPeajeDetalleDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);
     
            dbProvider.AddInParameter(command, helper.Rrpdcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Rrpecodi, DbType.Int32, entity.Rrpecodi);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, entity.Recpotcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Rrpdporcentaje, DbType.Decimal, entity.Rrpdporcentaje);
            dbProvider.AddInParameter(command, helper.Rrpdusucreacion, DbType.String, entity.Rrpdusucreacion);
            dbProvider.AddInParameter(command, helper.Rrpdfeccreacion, DbType.DateTime, DateTime.Today);
            dbProvider.AddInParameter(command, helper.Rrpdusumodificacion, DbType.String, entity.Rrpdusumodificacion);
            dbProvider.AddInParameter(command, helper.Rrpdfecmodificacion, DbType.DateTime, DateTime.Today);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(VtpRepaRecaPeajeDetalleDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Rrpdcodi, DbType.Int32, entity.Rrpdcodi);
            dbProvider.AddInParameter(command, helper.Rrpecodi, DbType.Int32, entity.Rrpecodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Rrpdporcentaje, DbType.Decimal, entity.Rrpdporcentaje);
            dbProvider.AddInParameter(command, helper.Rrpdusucreacion, DbType.String, entity.Rrpdusucreacion);
            dbProvider.AddInParameter(command, helper.Rrpdfeccreacion, DbType.DateTime, entity.Rrpdfeccreacion);
            dbProvider.AddInParameter(command, helper.Rrpdusumodificacion, DbType.String, entity.Rrpdusumodificacion);
            dbProvider.AddInParameter(command, helper.Rrpdfecmodificacion, DbType.DateTime, entity.Rrpdfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int rrpdcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Rrpdcodi, DbType.Int32, rrpdcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public VtpRepaRecaPeajeDetalleDTO GetById(int rrpdcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Rrpdcodi, DbType.Int32, rrpdcodi);
            VtpRepaRecaPeajeDetalleDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VtpRepaRecaPeajeDetalleDTO> List(int pericodi, int recpotcodi, int rrpecodi)
        {
            List<VtpRepaRecaPeajeDetalleDTO> entitys = new List<VtpRepaRecaPeajeDetalleDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, helper.Rrpecodi, DbType.Int32, rrpecodi);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);
            
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpRepaRecaPeajeDetalleDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<VtpRepaRecaPeajeDetalleDTO> GetByCriteria(int pericodi,int recpotcodi)
        {
            List<VtpRepaRecaPeajeDetalleDTO> entitys = new List<VtpRepaRecaPeajeDetalleDTO>();
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

        public int GetMaxNumEmpresas(int pericodi, int recpotcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlNumeroEmpresas);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);

            object result= dbProvider.ExecuteScalar(command);

            return Convert.ToInt32(result);
        }

        public void DeleteByCriteria(int pericodi, int recpotcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteByCriteria);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteByCriteriaRRPE(int pericodi, int recpotcodi, int rrpecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteByCriteriaRRPE);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);
            dbProvider.AddInParameter(command, helper.Rrpecodi, DbType.Int32, rrpecodi);

            dbProvider.ExecuteNonQuery(command);
        }
    }
}
