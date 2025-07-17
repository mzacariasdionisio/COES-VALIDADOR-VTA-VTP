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
    /// Clase de acceso a datos de la tabla VTP_SALDO_EMPRESA
    /// </summary>
    public class VtpSaldoEmpresaRepository : RepositoryBase, IVtpSaldoEmpresaRepository
    {
        public VtpSaldoEmpresaRepository(string strConn)
            : base(strConn)
        {
        }

        VtpSaldoEmpresaHelper helper = new VtpSaldoEmpresaHelper();

        public int Save(VtpSaldoEmpresaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Potsecodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, entity.Recpotcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Potseingreso, DbType.Decimal, entity.Potseingreso);
            dbProvider.AddInParameter(command, helper.Potseegreso, DbType.Decimal, entity.Potseegreso);
            dbProvider.AddInParameter(command, helper.Potsesaldoanterior, DbType.Decimal, entity.Potsesaldoanterior);
            dbProvider.AddInParameter(command, helper.Potsesaldo, DbType.Decimal, entity.Potsesaldo);
            dbProvider.AddInParameter(command, helper.Potseajuste, DbType.Decimal, entity.Potseajuste);
            dbProvider.AddInParameter(command, helper.Potseusucreacion, DbType.String, entity.Potseusucreacion);
            dbProvider.AddInParameter(command, helper.Potsefeccreacion, DbType.DateTime, entity.Potsefeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(VtpSaldoEmpresaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Potsesaldoreca, DbType.Decimal, entity.Potsesaldoreca);
            dbProvider.AddInParameter(command, helper.Potsepericodidest, DbType.Int32, entity.Potsepericodidest);

            dbProvider.AddInParameter(command, helper.Potsecodi, DbType.Int32, entity.Potsecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void UpdatePeriodoDestino(int pericodi, int recpotcodi, int potsepericodidest)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdatePeriodoDestino);

            dbProvider.AddInParameter(command, helper.Potsepericodidest, DbType.Int32, potsepericodidest);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int potsecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Potsecodi, DbType.Int32, potsecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteByCriteria(int pericodi, int recpotcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteByCriteria);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public VtpSaldoEmpresaDTO GetById(int potsecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Potsecodi, DbType.Int32, potsecodi);
            VtpSaldoEmpresaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public VtpSaldoEmpresaDTO GetByIdSaldo(int pericodi, int recpotcodi, int emprcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdSaldo);
            
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);

            VtpSaldoEmpresaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new VtpSaldoEmpresaDTO();

                    int iPericodi = dr.GetOrdinal(this.helper.Pericodi);
                    if (!dr.IsDBNull(iPericodi)) entity.Pericodi = Convert.ToInt32(dr.GetValue(iPericodi));

                    int iRecpotcodi = dr.GetOrdinal(this.helper.Recpotcodi);
                    if (!dr.IsDBNull(iRecpotcodi)) entity.Recpotcodi = Convert.ToInt32(dr.GetValue(iRecpotcodi));

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iIngreso = dr.GetOrdinal(this.helper.Potseingreso);
                    if (!dr.IsDBNull(iIngreso)) entity.Potseingreso = Convert.ToDecimal(dr.GetValue(iIngreso));

                    int iEgreso = dr.GetOrdinal(this.helper.Potseegreso);
                    if (!dr.IsDBNull(iEgreso)) entity.Potseegreso = Convert.ToDecimal(dr.GetValue(iEgreso));

                    int iSaldoAnt = dr.GetOrdinal(this.helper.Potsesaldoanterior);
                    if (!dr.IsDBNull(iSaldoAnt)) entity.Potsesaldoanterior = Convert.ToDecimal(dr.GetValue(iSaldoAnt));

                    int iSaldo = dr.GetOrdinal(this.helper.Potsesaldo);
                    if (!dr.IsDBNull(iSaldo)) entity.Potsesaldo = Convert.ToDecimal(dr.GetValue(iSaldo));

                    int iSaldoReca = dr.GetOrdinal(this.helper.Potsesaldoreca);
                    if (!dr.IsDBNull(iSaldoReca)) entity.Potsesaldoreca = Convert.ToDecimal(dr.GetValue(iSaldoReca));
                }
            }

            return entity;
        }

        public List<VtpSaldoEmpresaDTO> GetByIdSaldoGeneral(int pericodi, int recpotcodi, int emprcodi)
        {
            List<VtpSaldoEmpresaDTO> entitys = new List<VtpSaldoEmpresaDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdSaldoGeneral);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public decimal GetSaldoAnterior(int potsepericodidest, int emprcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetSaldoAnterior);
            dbProvider.AddInParameter(command, helper.Potsepericodidest, DbType.Int32, potsepericodidest);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);

            object result = dbProvider.ExecuteScalar(command);
            decimal dSaldo = 0;
            if (result != null) dSaldo = Convert.ToDecimal(result);
            return dSaldo;
        }

        public List<VtpSaldoEmpresaDTO> List(int pericodi, int recpotcodi)
        {
            List<VtpSaldoEmpresaDTO> entitys = new List<VtpSaldoEmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpSaldoEmpresaDTO entity = helper.Create(dr);

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = Convert.ToString(dr.GetValue(iEmprnomb));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<VtpSaldoEmpresaDTO> ListCalculaSaldo(int pericodi, int recpotcodi)
        {
            List<VtpSaldoEmpresaDTO> entitys = new List<VtpSaldoEmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListCalculaSaldo);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpSaldoEmpresaDTO entity = new VtpSaldoEmpresaDTO();

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = Convert.ToString(dr.GetValue(iEmprnomb));

                    int iPotseingreso = dr.GetOrdinal(this.helper.Potseingreso);
                    if (!dr.IsDBNull(iPotseingreso)) entity.Potseingreso = dr.GetDecimal(iPotseingreso);

                    int iPotseegreso = dr.GetOrdinal(this.helper.Potseegreso);
                    if (!dr.IsDBNull(iPotseegreso)) entity.Potseegreso = dr.GetDecimal(iPotseegreso);

                    int iPotsesaldo = dr.GetOrdinal(this.helper.Potsesaldo);
                    if (!dr.IsDBNull(iPotsesaldo)) entity.Potsesaldo = dr.GetDecimal(iPotsesaldo);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<VtpSaldoEmpresaDTO> ListPositiva(int pericodi, int recpotcodi)
        {
            List<VtpSaldoEmpresaDTO> entitys = new List<VtpSaldoEmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListPositiva);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpSaldoEmpresaDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = Convert.ToString(dr.GetValue(iEmprnomb));

                    int iPotsetotalsaldopositivo = dr.GetOrdinal(this.helper.Potsetotalsaldopositivo);
                    if (!dr.IsDBNull(iPotsetotalsaldopositivo)) entity.Potsetotalsaldopositivo = dr.GetDecimal(iPotsetotalsaldopositivo);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<VtpSaldoEmpresaDTO> ListNegativa(int pericodi, int recpotcodi)
        {
            List<VtpSaldoEmpresaDTO> entitys = new List<VtpSaldoEmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListNegativa);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpSaldoEmpresaDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = Convert.ToString(dr.GetValue(iEmprnomb));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<VtpSaldoEmpresaDTO> GetByCriteria()
        {
            List<VtpSaldoEmpresaDTO> entitys = new List<VtpSaldoEmpresaDTO>();
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

        public List<VtpSaldoEmpresaDTO> ListPeriodosDestino(int potsepericodidest)
        {
            List<VtpSaldoEmpresaDTO> entitys = new List<VtpSaldoEmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListPeriodosDestino);
            dbProvider.AddInParameter(command, helper.Potsepericodidest, DbType.Int32, potsepericodidest);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpSaldoEmpresaDTO entity = new VtpSaldoEmpresaDTO();

                    int iPericodi = dr.GetOrdinal(this.helper.Pericodi);
                    if (!dr.IsDBNull(iPericodi)) entity.Pericodi = Convert.ToInt32(dr.GetValue(iPericodi));

                    int iPerinombre = dr.GetOrdinal(this.helper.Perinombre);
                    if (!dr.IsDBNull(iPerinombre)) entity.Perinombre = Convert.ToString(dr.GetValue(iPerinombre));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public VtpSaldoEmpresaDTO GetSaldoEmpresaPeriodo(int emprcodi, int pericodi, int potsepericodidest)
        {
            List<VtpSaldoEmpresaDTO> entitys = new List<VtpSaldoEmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetSaldoEmpresaPeriodo);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Potsepericodidest, DbType.Int32, potsepericodidest);

            VtpSaldoEmpresaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new VtpSaldoEmpresaDTO();

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = Convert.ToString(dr.GetValue(iEmprnomb));

                    int iPotsepericodidest = dr.GetOrdinal(this.helper.Potsepericodidest);
                    if (!dr.IsDBNull(iPotsepericodidest)) entity.Potsepericodidest = Convert.ToInt32(dr.GetValue(iPotsepericodidest));

                    int iPerinombre = dr.GetOrdinal(this.helper.Perinombre);
                    if (!dr.IsDBNull(iPerinombre)) entity.Perinombre = Convert.ToString(dr.GetValue(iPerinombre));

                    int iPotsesaldoreca = dr.GetOrdinal(this.helper.Potsesaldoreca);
                    if (!dr.IsDBNull(iPotsesaldoreca)) entity.Potsesaldoreca = dr.GetDecimal(iPotsesaldoreca);

                }
            }

            return entity;
        }
    }
}
