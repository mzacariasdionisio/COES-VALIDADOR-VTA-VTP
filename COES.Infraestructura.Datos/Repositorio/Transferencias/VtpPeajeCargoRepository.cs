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
    /// Clase de acceso a datos de la tabla VTP_PEAJE_CARGO
    /// </summary>
    public class VtpPeajeCargoRepository : RepositoryBase, IVtpPeajeCargoRepository
    {
        public VtpPeajeCargoRepository(string strConn)
            : base(strConn)
        {
        }

        VtpPeajeCargoHelper helper = new VtpPeajeCargoHelper();

        public int Save(VtpPeajeCargoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Pecarcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, entity.Recpotcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Pingcodi, DbType.Int32, entity.Pingcodi);
            dbProvider.AddInParameter(command, helper.Pecartransmision, DbType.String, entity.Pecartransmision);
            dbProvider.AddInParameter(command, helper.Pecarpeajecalculado, DbType.Decimal, entity.Pecarpeajecalculado);
            dbProvider.AddInParameter(command, helper.Pecarpeajedeclarado, DbType.Decimal, entity.Pecarpeajedeclarado);
            dbProvider.AddInParameter(command, helper.Pecarpeajerecaudado, DbType.Decimal, entity.Pecarpeajerecaudado);
            dbProvider.AddInParameter(command, helper.Pecarsaldoanterior, DbType.Decimal, entity.Pecarsaldoanterior);
            dbProvider.AddInParameter(command, helper.Pecarajuste, DbType.Decimal, entity.Pecarajuste);
            dbProvider.AddInParameter(command, helper.Pecarusucreacion, DbType.String, entity.Pecarusucreacion);
            dbProvider.AddInParameter(command, helper.Pecarfeccreacion, DbType.DateTime, DateTime.Now);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(VtpPeajeCargoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Pecarsaldo, DbType.Decimal, entity.Pecarsaldo);
            dbProvider.AddInParameter(command, helper.Pecarpericodidest, DbType.Int32, entity.Pecarpericodidest);

            dbProvider.AddInParameter(command, helper.Pecarcodi, DbType.Int32, entity.Pecarcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void UpdatePeriodoDestino(int pericodi, int recpotcodi, int pecarpericodidest)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdatePeriodoDestino);

            dbProvider.AddInParameter(command, helper.Pecarpericodidest, DbType.Int32, pecarpericodidest);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);
          
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int pecarcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Pecarcodi, DbType.Int32, pecarcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteByCriteria(int pericodi, int recpotcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteByCriteria);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public VtpPeajeCargoDTO GetById(int pecarcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Pecarcodi, DbType.Int32, pecarcodi);
            VtpPeajeCargoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public VtpPeajeCargoDTO GetByIdSaldo(int pericodi, int recpotcodi, int emprcodi, int pingcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdSaldo);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Pingcodi, DbType.Int32, pingcodi);
            VtpPeajeCargoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public decimal GetSaldoAnterior(int pecarpericodidest, int emprcodi, int pingcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetSaldoAnterior);
            dbProvider.AddInParameter(command, helper.Pecarpericodidest, DbType.Int32, pecarpericodidest);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Pingcodi, DbType.Int32, pingcodi);

            object result = dbProvider.ExecuteScalar(command);
            decimal dSaldo = 0;
            if (result != null) dSaldo = Convert.ToDecimal(result);
            return dSaldo;
        }

        public List<VtpPeajeCargoDTO> List()
        {
            List<VtpPeajeCargoDTO> entitys = new List<VtpPeajeCargoDTO>();
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

        public List<VtpPeajeCargoDTO> ListEmpresa(int pericodi, int recpotcodi)
        {
            List<VtpPeajeCargoDTO> entitys = new List<VtpPeajeCargoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListEmpresa);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpPeajeCargoDTO entity = new VtpPeajeCargoDTO();

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<VtpPeajeCargoDTO> ListPagoNo(string emprcodi, int pericodi, int recpotcodi)
        {
            List<VtpPeajeCargoDTO> entitys = new List<VtpPeajeCargoDTO>();
            var query = string.Format(helper.SqlListPagoNo, emprcodi, pericodi, recpotcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpPeajeCargoDTO entity = new VtpPeajeCargoDTO();

                    int iPingcodi = dr.GetOrdinal(this.helper.Pingcodi);
                    if (!dr.IsDBNull(iPingcodi)) entity.Pingcodi = Convert.ToInt32(dr.GetValue(iPingcodi));

                    int iPingnombre = dr.GetOrdinal(this.helper.Pingnombre);
                    if (!dr.IsDBNull(iPingnombre)) entity.Pingnombre = dr.GetString(iPingnombre);

                    int iPecarpeajerecaudado = dr.GetOrdinal(this.helper.Pecarpeajerecaudado);
                    if (!dr.IsDBNull(iPecarpeajerecaudado)) entity.Pecarpeajerecaudado = dr.GetDecimal(iPecarpeajerecaudado);

                    int iPecarsaldoanterior = dr.GetOrdinal(this.helper.Pecarsaldoanterior);
                    if (!dr.IsDBNull(iPecarsaldoanterior)) entity.Pecarsaldoanterior = dr.GetDecimal(iPecarsaldoanterior);

                    int iPecarsaldo = dr.GetOrdinal(this.helper.Pecarsaldo);
                    if (!dr.IsDBNull(iPecarsaldo)) entity.Pecarsaldo = dr.GetDecimal(iPecarsaldo);

                    int iPecarajuste = dr.GetOrdinal(this.helper.Pecarajuste);
                    if (!dr.IsDBNull(iPecarajuste)) entity.Pecarajuste = dr.GetDecimal(iPecarajuste);

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<VtpPeajeCargoDTO> ListPagoAdicional(int pericodi, int recpotcodi, int pingcodi)
        {
            List<VtpPeajeCargoDTO> entitys = new List<VtpPeajeCargoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListPagoAdicional);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);
            dbProvider.AddInParameter(command, helper.Pingcodi, DbType.Int32, pingcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpPeajeCargoDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<VtpPeajeCargoDTO> GetByCriteria()
        {
            List<VtpPeajeCargoDTO> entitys = new List<VtpPeajeCargoDTO>();
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
