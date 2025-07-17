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
    /// Clase de acceso a datos de la tabla VTP_PEAJE_EMPRESA_AJUSTE
    /// </summary>
    public class VtpPeajeEmpresaAjusteRepository: RepositoryBase, IVtpPeajeEmpresaAjusteRepository
    {
        public VtpPeajeEmpresaAjusteRepository(string strConn): base(strConn)
        {
        }

        VtpPeajeEmpresaAjusteHelper helper = new VtpPeajeEmpresaAjusteHelper();

        public int Save(VtpPeajeEmpresaAjusteDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Pempajcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Emprcodipeaje, DbType.Int32, entity.Emprcodipeaje);
            dbProvider.AddInParameter(command, helper.Pingcodi, DbType.Int32, entity.Pingcodi);
            dbProvider.AddInParameter(command, helper.Emprcodicargo, DbType.Int32, entity.Emprcodicargo);
            dbProvider.AddInParameter(command, helper.Pempajajuste, DbType.Decimal, entity.Pempajajuste);
            dbProvider.AddInParameter(command, helper.Pempajusucreacion, DbType.String, entity.Pempajusucreacion);
            dbProvider.AddInParameter(command, helper.Pempajfeccreacion, DbType.DateTime, DateTime.Now);
            
            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(VtpPeajeEmpresaAjusteDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Pempajajuste, DbType.Decimal, entity.Pempajajuste);
            dbProvider.AddInParameter(command, helper.Pempajcodi, DbType.Int32, entity.Pempajcodi);
            
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int Pempajcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Pempajcodi, DbType.Int32, Pempajcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteByCriteria(int pericodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteByCriteria);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public VtpPeajeEmpresaAjusteDTO GetById(int Pempajcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Pempajcodi, DbType.Int32, Pempajcodi);
            VtpPeajeEmpresaAjusteDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VtpPeajeEmpresaAjusteDTO> List()
        {
            List<VtpPeajeEmpresaAjusteDTO> entitys = new List<VtpPeajeEmpresaAjusteDTO>();
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

        public List<VtpPeajeEmpresaAjusteDTO> GetByCriteria(int pericodi)
        {
            List<VtpPeajeEmpresaAjusteDTO> entitys = new List<VtpPeajeEmpresaAjusteDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpPeajeEmpresaAjusteDTO entity = helper.Create(dr);

                    int iEmprnombpeaje = dr.GetOrdinal(this.helper.Emprnombpeaje);
                    if (!dr.IsDBNull(iEmprnombpeaje)) entity.Emprnombpeaje = dr.GetString(iEmprnombpeaje);

                    int iPingnombre = dr.GetOrdinal(this.helper.Pingnombre);
                    if (!dr.IsDBNull(iPingnombre)) entity.Pingnombre = dr.GetString(iPingnombre);

                    int iEmprnombcargo = dr.GetOrdinal(this.helper.Emprnombcargo);
                    if (!dr.IsDBNull(iEmprnombcargo)) entity.Emprnombcargo = dr.GetString(iEmprnombcargo);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public decimal GetAjuste(int pericodi, int emprcodipeaje, int pingcodi, int emprcodicargo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetAjuste);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Emprcodipeaje, DbType.Int32, emprcodipeaje);
            dbProvider.AddInParameter(command, helper.Pingcodi, DbType.Int32, pingcodi);
            dbProvider.AddInParameter(command, helper.Emprcodicargo, DbType.Int32, emprcodicargo);
            
            object result = dbProvider.ExecuteScalar(command);
            decimal dSaldo = 0;
            if (result != null) dSaldo = Convert.ToDecimal(result);
            return dSaldo;
        }
    }
}
