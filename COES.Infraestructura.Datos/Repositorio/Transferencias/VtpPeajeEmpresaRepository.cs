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
    /// Clase de acceso a datos de la tabla VTP_PEAJE_EMPRESA
    /// </summary>
    public class VtpPeajeEmpresaRepository: RepositoryBase, IVtpPeajeEmpresaRepository
    {
        public VtpPeajeEmpresaRepository(string strConn): base(strConn)
        {
        }

        VtpPeajeEmpresaHelper helper = new VtpPeajeEmpresaHelper();

        public int Save(VtpPeajeEmpresaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Pempcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, entity.Recpotcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Pemptotalrecaudacion, DbType.Decimal, entity.Pemptotalrecaudacion);
            dbProvider.AddInParameter(command, helper.Pempporctrecaudacion, DbType.Decimal, entity.Pempporctrecaudacion);
            dbProvider.AddInParameter(command, helper.Pempusucreacion, DbType.String, entity.Pempusucreacion);
            dbProvider.AddInParameter(command, helper.Pempfeccreacion, DbType.DateTime, DateTime.Now);
            
            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(VtpPeajeEmpresaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, entity.Recpotcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Pemptotalrecaudacion, DbType.Decimal, entity.Pemptotalrecaudacion);
            dbProvider.AddInParameter(command, helper.Pempporctrecaudacion, DbType.Decimal, entity.Pempporctrecaudacion);
            dbProvider.AddInParameter(command, helper.Pempusucreacion, DbType.String, entity.Pempusucreacion);
            dbProvider.AddInParameter(command, helper.Pempfeccreacion, DbType.DateTime, entity.Pempfeccreacion);

            dbProvider.AddInParameter(command, helper.Pempcodi, DbType.Int32, entity.Pempcodi);
            
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int Pempcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Pempcodi, DbType.Int32, Pempcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteByCriteria(int pericodi, int recpotcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteByCriteria);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public VtpPeajeEmpresaDTO GetById(int Pempcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Pempcodi, DbType.Int32, Pempcodi);
            VtpPeajeEmpresaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VtpPeajeEmpresaDTO> List()
        {
            List<VtpPeajeEmpresaDTO> entitys = new List<VtpPeajeEmpresaDTO>();
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

        public List<VtpPeajeEmpresaDTO> GetByCriteria()
        {
            List<VtpPeajeEmpresaDTO> entitys = new List<VtpPeajeEmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

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
