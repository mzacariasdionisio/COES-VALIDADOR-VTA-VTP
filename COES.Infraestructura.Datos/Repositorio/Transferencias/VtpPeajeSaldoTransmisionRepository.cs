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
    public class VtpPeajeSaldoTransmisionRepository: RepositoryBase, IVtpPeajeSaldoTransmisionRepository
    {
        public VtpPeajeSaldoTransmisionRepository(string strConn): base(strConn)
        {
        }

        VtpPeajeSaldoTransmisionHelper helper = new VtpPeajeSaldoTransmisionHelper();

        public int Save(VtpPeajeSaldoTransmisionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Pstrnscodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, entity.Recpotcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Pstrnstotalrecaudacion, DbType.Decimal, entity.Pstrnstotalrecaudacion);
            dbProvider.AddInParameter(command, helper.Pstrnstotalpago, DbType.Decimal, entity.Pstrnstotalpago);
            dbProvider.AddInParameter(command, helper.Pstrnssaldotransmision, DbType.Decimal, entity.Pstrnssaldotransmision);
            dbProvider.AddInParameter(command, helper.Pstrnsusucreacion, DbType.String, entity.Pstrnsusucreacion);
            dbProvider.AddInParameter(command, helper.Pstrnsfeccreacion, DbType.DateTime, DateTime.Now);
            
            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(VtpPeajeSaldoTransmisionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, entity.Recpotcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Pstrnstotalrecaudacion, DbType.Decimal, entity.Pstrnstotalrecaudacion);
            dbProvider.AddInParameter(command, helper.Pstrnstotalpago, DbType.Decimal, entity.Pstrnstotalpago);
            dbProvider.AddInParameter(command, helper.Pstrnssaldotransmision, DbType.Decimal, entity.Pstrnssaldotransmision);
            dbProvider.AddInParameter(command, helper.Pstrnsusucreacion, DbType.String, entity.Pstrnsusucreacion);
            dbProvider.AddInParameter(command, helper.Pstrnsfeccreacion, DbType.DateTime, entity.Pstrnsfeccreacion);

            dbProvider.AddInParameter(command, helper.Pstrnscodi, DbType.Int32, entity.Pstrnscodi);
            
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int Pstrnscodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Pstrnscodi, DbType.Int32, Pstrnscodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteByCriteria(int pericodi, int recpotcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteByCriteria);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public VtpPeajeSaldoTransmisionDTO GetById(int Pstrnscodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Pstrnscodi, DbType.Int32, Pstrnscodi);
            VtpPeajeSaldoTransmisionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public VtpPeajeSaldoTransmisionDTO GetByIdEmpresa(int pericodi, int recpotcodi, int emprcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdEmpresa);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            VtpPeajeSaldoTransmisionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VtpPeajeSaldoTransmisionDTO> List()
        {
            List<VtpPeajeSaldoTransmisionDTO> entitys = new List<VtpPeajeSaldoTransmisionDTO>();
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

        public List<VtpPeajeSaldoTransmisionDTO> ListEmpresaEgreso(int pericodi, int recpotcodi)
        {
            List<VtpPeajeSaldoTransmisionDTO> entitys = new List<VtpPeajeSaldoTransmisionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListEmpresaEgreso);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpPeajeSaldoTransmisionDTO entity = new VtpPeajeSaldoTransmisionDTO();

                    entity.Pericodi = pericodi;

                    entity.Recpotcodi = recpotcodi;

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = Convert.ToString(dr.GetValue(iEmprnomb));

                    int iPstrnstotalrecaudacion = dr.GetOrdinal(this.helper.Pstrnstotalrecaudacion);
                    if (!dr.IsDBNull(iPstrnstotalrecaudacion)) entity.Pstrnstotalrecaudacion = dr.GetDecimal(iPstrnstotalrecaudacion);

                    int iPstrnstotalpago = dr.GetOrdinal(this.helper.Pstrnstotalpago);
                    if (!dr.IsDBNull(iPstrnstotalpago)) entity.Pstrnstotalpago = dr.GetDecimal(iPstrnstotalpago);

                    int iPstrnssaldotransmision = dr.GetOrdinal(this.helper.Pstrnssaldotransmision);
                    if (!dr.IsDBNull(iPstrnssaldotransmision)) entity.Pstrnssaldotransmision = dr.GetDecimal(iPstrnssaldotransmision);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<VtpPeajeSaldoTransmisionDTO> GetByCriteria(int pericodi, int recpotcodi)
        {
            List<VtpPeajeSaldoTransmisionDTO> entitys = new List<VtpPeajeSaldoTransmisionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpPeajeSaldoTransmisionDTO entity = new VtpPeajeSaldoTransmisionDTO();

                    entity.Pericodi = pericodi;

                    entity.Recpotcodi = recpotcodi;

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = Convert.ToString(dr.GetValue(iEmprnomb));

                    int iPstrnstotalrecaudacion = dr.GetOrdinal(this.helper.Pstrnstotalrecaudacion);
                    if (!dr.IsDBNull(iPstrnstotalrecaudacion)) entity.Pstrnstotalrecaudacion = dr.GetDecimal(iPstrnstotalrecaudacion);

                    int iPstrnstotalpago = dr.GetOrdinal(this.helper.Pstrnstotalpago);
                    if (!dr.IsDBNull(iPstrnstotalpago)) entity.Pstrnstotalpago = dr.GetDecimal(iPstrnstotalpago);

                    int iPstrnssaldotransmision = dr.GetOrdinal(this.helper.Pstrnssaldotransmision);
                    if (!dr.IsDBNull(iPstrnssaldotransmision)) entity.Pstrnssaldotransmision = dr.GetDecimal(iPstrnssaldotransmision);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
