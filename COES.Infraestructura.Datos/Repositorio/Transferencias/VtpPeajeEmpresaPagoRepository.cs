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
    /// Clase de acceso a datos de la tabla VTP_PEAJE_EMPRESA_PAGO
    /// </summary>
    public class VtpPeajeEmpresaPagoRepository : RepositoryBase, IVtpPeajeEmpresaPagoRepository
    {
        public VtpPeajeEmpresaPagoRepository(string strConn)
            : base(strConn)
        {
        }

        VtpPeajeEmpresaPagoHelper helper = new VtpPeajeEmpresaPagoHelper();

        public int Save(VtpPeajeEmpresaPagoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Pempagcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, entity.Recpotcodi);
            dbProvider.AddInParameter(command, helper.Emprcodipeaje, DbType.Int32, entity.Emprcodipeaje);
            dbProvider.AddInParameter(command, helper.Pingcodi, DbType.Int32, entity.Pingcodi);
            dbProvider.AddInParameter(command, helper.Emprcodicargo, DbType.Int32, entity.Emprcodicargo);
            dbProvider.AddInParameter(command, helper.Pempagtransmision, DbType.String, entity.Pempagtransmision);
            dbProvider.AddInParameter(command, helper.Pempagpeajepago, DbType.Decimal, entity.Pempagpeajepago);
            dbProvider.AddInParameter(command, helper.Pempagsaldoanterior, DbType.Decimal, entity.Pempagsaldoanterior);
            dbProvider.AddInParameter(command, helper.Pempagajuste, DbType.Decimal, entity.Pempagajuste);
            dbProvider.AddInParameter(command, helper.Pempagpericodidest, DbType.Int32, entity.Pempagpericodidest);
            dbProvider.AddInParameter(command, helper.Pempagusucreacion, DbType.String, entity.Pempagusucreacion);
            dbProvider.AddInParameter(command, helper.Pempagfeccreacion, DbType.DateTime, DateTime.Now);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(VtpPeajeEmpresaPagoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Pempagsaldo, DbType.Decimal, entity.Pempagsaldo);
            dbProvider.AddInParameter(command, helper.Pempagpericodidest, DbType.Int32, entity.Pempagpericodidest);

            dbProvider.AddInParameter(command, helper.Pempagcodi, DbType.Int32, entity.Pempagcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void UpdatePeriodoDestino(int pericodi, int recpotcodi, int pempagpericodidest)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdatePeriodoDestino);

            dbProvider.AddInParameter(command, helper.Pempagpericodidest, DbType.Int32, pempagpericodidest);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int Pempagcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Pempagcodi, DbType.Int32, Pempagcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteByCriteria(int pericodi, int recpotcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteByCriteria);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public VtpPeajeEmpresaPagoDTO GetById(int Pempagcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Pempagcodi, DbType.Int32, Pempagcodi);
            VtpPeajeEmpresaPagoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VtpPeajeEmpresaPagoDTO> List()
        {
            List<VtpPeajeEmpresaPagoDTO> entitys = new List<VtpPeajeEmpresaPagoDTO>();
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

        public List<VtpPeajeEmpresaPagoDTO> ListPeajePago(int pericodi, int recpotcodi)
        {
            List<VtpPeajeEmpresaPagoDTO> entitys = new List<VtpPeajeEmpresaPagoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListPeajePago);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpPeajeEmpresaPagoDTO entity = new VtpPeajeEmpresaPagoDTO();

                    int iEmprcodipeaje = dr.GetOrdinal(this.helper.Emprcodipeaje);
                    if (!dr.IsDBNull(iEmprcodipeaje)) entity.Emprcodipeaje = Convert.ToInt32(dr.GetValue(iEmprcodipeaje));

                    int iEmprnombpeaje = dr.GetOrdinal(this.helper.Emprnombpeaje);
                    if (!dr.IsDBNull(iEmprnombpeaje)) entity.Emprnombpeaje = Convert.ToString(dr.GetValue(iEmprnombpeaje));

                    int iEmprruc = dr.GetOrdinal(this.helper.Emprruc);
                    if (!dr.IsDBNull(iEmprruc)) entity.Emprruc = Convert.ToString(dr.GetValue(iEmprruc));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<VtpPeajeEmpresaPagoDTO> ListPeajeCobro(int emprcodipeaje, int pericodi, int recpotcodi)
        {
            List<VtpPeajeEmpresaPagoDTO> entitys = new List<VtpPeajeEmpresaPagoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListPeajeCobro);
            dbProvider.AddInParameter(command, helper.Emprcodipeaje, DbType.Int32, emprcodipeaje);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpPeajeEmpresaPagoDTO entity = new VtpPeajeEmpresaPagoDTO();

                    int iEmprcodipeaje = dr.GetOrdinal(this.helper.Emprcodipeaje);
                    if (!dr.IsDBNull(iEmprcodipeaje)) entity.Emprcodipeaje = Convert.ToInt32(dr.GetValue(iEmprcodipeaje));

                    int iEmprnombpeaje = dr.GetOrdinal(this.helper.Emprnombpeaje);
                    if (!dr.IsDBNull(iEmprnombpeaje)) entity.Emprnombpeaje = Convert.ToString(dr.GetValue(iEmprnombpeaje));

                    int iEmprcodicargo = dr.GetOrdinal(this.helper.Emprcodicargo);
                    if (!dr.IsDBNull(iEmprcodicargo)) entity.Emprcodicargo = Convert.ToInt32(dr.GetValue(iEmprcodicargo));

                    int iEmprruc = dr.GetOrdinal(this.helper.Emprruc);
                    if (!dr.IsDBNull(iEmprruc)) entity.Emprruc = Convert.ToString(dr.GetValue(iEmprruc));

                    int iEmprnombcargo = dr.GetOrdinal(this.helper.Emprnombcargo);
                    if (!dr.IsDBNull(iEmprnombcargo)) entity.Emprnombcargo = Convert.ToString(dr.GetValue(iEmprnombcargo));

                    int iPingcodi = dr.GetOrdinal(this.helper.Pingcodi);
                    if (!dr.IsDBNull(iPingcodi)) entity.Pingcodi = Convert.ToInt32(dr.GetValue(iPingcodi));

                    int iPempagpeajepago = dr.GetOrdinal(this.helper.Pempagpeajepago);
                    if (!dr.IsDBNull(iPempagpeajepago)) entity.Pempagpeajepago = Convert.ToDecimal(dr.GetValue(iPempagpeajepago));

                    int iPempagsaldoanterior = dr.GetOrdinal(this.helper.Pempagsaldoanterior);
                    if (!dr.IsDBNull(iPempagsaldoanterior)) entity.Pempagsaldoanterior = Convert.ToDecimal(dr.GetValue(iPempagsaldoanterior));

                    int iPempagsaldo = dr.GetOrdinal(this.helper.Pempagsaldo);
                    if (!dr.IsDBNull(iPempagsaldo)) entity.Pempagsaldo = Convert.ToDecimal(dr.GetValue(iPempagsaldo));

                    int iPempagajuste = dr.GetOrdinal(this.helper.Pempagajuste);
                    if (!dr.IsDBNull(iPempagajuste)) entity.Pempagajuste = Convert.ToDecimal(dr.GetValue(iPempagajuste));

                    int iEmprcodosinergminpeaje = dr.GetOrdinal(this.helper.Emprcodosinergminpeaje);
                    if (!dr.IsDBNull(iEmprcodosinergminpeaje)) entity.Emprcodosinergminpeaje = dr.GetString(iEmprcodosinergminpeaje);
                    int iEmprcodosinergmincargo = dr.GetOrdinal(this.helper.Emprcodosinergmincargo);
                    if (!dr.IsDBNull(iEmprcodosinergmincargo)) entity.Emprcodosinergmincargo = dr.GetString(iEmprcodosinergmincargo);

                    int iPingtipo = dr.GetOrdinal(helper.Pingtipo);
                    if (!dr.IsDBNull(iPingtipo)) entity.Pingtipo = dr.GetString(iPingtipo);
                    int iPingnombre = dr.GetOrdinal(helper.Pingnombre);
                    if (!dr.IsDBNull(iPingnombre)) entity.Pingnombre = dr.GetString(iPingnombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<VtpPeajeEmpresaPagoDTO> ListPeajeCobroHistoricos(int emprcodipeaje, int pericodi, int recpotcodi)
        {
            List<VtpPeajeEmpresaPagoDTO> entitys = new List<VtpPeajeEmpresaPagoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListPeajeCobroHistoricos);
            dbProvider.AddInParameter(command, helper.Emprcodipeaje, DbType.Int32, emprcodipeaje);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpPeajeEmpresaPagoDTO entity = new VtpPeajeEmpresaPagoDTO();

                    int iEmprcodipeaje = dr.GetOrdinal(this.helper.Emprcodipeaje);
                    if (!dr.IsDBNull(iEmprcodipeaje)) entity.Emprcodipeaje = Convert.ToInt32(dr.GetValue(iEmprcodipeaje));

                    int iEmprnombpeaje = dr.GetOrdinal(this.helper.Emprnombpeaje);
                    if (!dr.IsDBNull(iEmprnombpeaje)) entity.Emprnombpeaje = Convert.ToString(dr.GetValue(iEmprnombpeaje));

                    int iEmprcodicargo = dr.GetOrdinal(this.helper.Emprcodicargo);
                    if (!dr.IsDBNull(iEmprcodicargo)) entity.Emprcodicargo = Convert.ToInt32(dr.GetValue(iEmprcodicargo));

                    int iEmprnombcargo = dr.GetOrdinal(this.helper.Emprnombcargo);
                    if (!dr.IsDBNull(iEmprnombcargo)) entity.Emprnombcargo = Convert.ToString(dr.GetValue(iEmprnombcargo));

                    int iPingcodi = dr.GetOrdinal(this.helper.Pingcodi);
                    if (!dr.IsDBNull(iPingcodi)) entity.Pingcodi = Convert.ToInt32(dr.GetValue(iPingcodi));

                    int iPempagpeajepago = dr.GetOrdinal(this.helper.Pempagpeajepago);
                    if (!dr.IsDBNull(iPempagpeajepago)) entity.Pempagpeajepago = Convert.ToDecimal(dr.GetValue(iPempagpeajepago));

                    int iPempagsaldoanterior = dr.GetOrdinal(this.helper.Pempagsaldoanterior);
                    if (!dr.IsDBNull(iPempagsaldoanterior)) entity.Pempagsaldoanterior = Convert.ToDecimal(dr.GetValue(iPempagsaldoanterior));

                    int iPempagsaldo = dr.GetOrdinal(this.helper.Pempagsaldo);
                    if (!dr.IsDBNull(iPempagsaldo)) entity.Pempagsaldo = Convert.ToDecimal(dr.GetValue(iPempagsaldo));

                    int iPempagajuste = dr.GetOrdinal(this.helper.Pempagajuste);
                    if (!dr.IsDBNull(iPempagajuste)) entity.Pempagajuste = Convert.ToDecimal(dr.GetValue(iPempagajuste));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }


        


        public List<VtpPeajeEmpresaPagoDTO> ListPeajeCobroSelect(int pericodi, int recpotcodi)
        {
            List<VtpPeajeEmpresaPagoDTO> entitys = new List<VtpPeajeEmpresaPagoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListPeajeCobroSelect);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpPeajeEmpresaPagoDTO entity = new VtpPeajeEmpresaPagoDTO();

                    int iEmprcodicargo = dr.GetOrdinal(this.helper.Emprcodicargo);
                    if (!dr.IsDBNull(iEmprcodicargo)) entity.Emprcodicargo = Convert.ToInt32(dr.GetValue(iEmprcodicargo));

                    int iEmprnombcargo = dr.GetOrdinal(this.helper.Emprnombcargo);
                    if (!dr.IsDBNull(iEmprnombcargo)) entity.Emprnombcargo = Convert.ToString(dr.GetValue(iEmprnombcargo));

                    int iPingcodi = dr.GetOrdinal(this.helper.Pingcodi);
                    if (!dr.IsDBNull(iPingcodi)) entity.Pingcodi = Convert.ToInt32(dr.GetValue(iPingcodi));

                    int iPempagpeajepago = dr.GetOrdinal(this.helper.Pempagpeajepago);
                    if (!dr.IsDBNull(iPempagpeajepago)) entity.Pempagpeajepago = Convert.ToDecimal(dr.GetValue(iPempagpeajepago));

                    int iPempagsaldoanterior = dr.GetOrdinal(this.helper.Pempagsaldoanterior);
                    if (!dr.IsDBNull(iPempagsaldoanterior)) entity.Pempagsaldoanterior = Convert.ToDecimal(dr.GetValue(iPempagsaldoanterior));

                    int iPempagsaldo = dr.GetOrdinal(this.helper.Pempagsaldo);
                    if (!dr.IsDBNull(iPempagsaldo)) entity.Pempagsaldo = Convert.ToDecimal(dr.GetValue(iPempagsaldo));

                    int iPempagajuste = dr.GetOrdinal(this.helper.Pempagajuste);
                    if (!dr.IsDBNull(iPempagajuste)) entity.Pempagajuste = Convert.ToDecimal(dr.GetValue(iPempagajuste));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<VtpPeajeEmpresaPagoDTO> ListPeajeCobroNoTransm(int emprcodipeaje, int pericodi, int recpotcodi)
        {
            List<VtpPeajeEmpresaPagoDTO> entitys = new List<VtpPeajeEmpresaPagoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListPeajeCobroNoTransm);
            dbProvider.AddInParameter(command, helper.Emprcodipeaje, DbType.Int32, emprcodipeaje);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpPeajeEmpresaPagoDTO entity = new VtpPeajeEmpresaPagoDTO();

                    int iEmprcodipeaje = dr.GetOrdinal(this.helper.Emprcodipeaje);
                    if (!dr.IsDBNull(iEmprcodipeaje)) entity.Emprcodipeaje = Convert.ToInt32(dr.GetValue(iEmprcodipeaje));

                    int iEmprnombpeaje = dr.GetOrdinal(this.helper.Emprnombpeaje);
                    if (!dr.IsDBNull(iEmprnombpeaje)) entity.Emprnombpeaje = Convert.ToString(dr.GetValue(iEmprnombpeaje));

                    int iEmprcodicargo = dr.GetOrdinal(this.helper.Emprcodicargo);
                    if (!dr.IsDBNull(iEmprcodicargo)) entity.Emprcodicargo = Convert.ToInt32(dr.GetValue(iEmprcodicargo));

                    int iEmprruc = dr.GetOrdinal(this.helper.Emprruc);
                    if (!dr.IsDBNull(iEmprruc)) entity.Emprruc = Convert.ToString(dr.GetValue(iEmprruc));

                    int iEmprnombcargo = dr.GetOrdinal(this.helper.Emprnombcargo);
                    if (!dr.IsDBNull(iEmprnombcargo)) entity.Emprnombcargo = Convert.ToString(dr.GetValue(iEmprnombcargo));

                    int iPingcodi = dr.GetOrdinal(this.helper.Pingcodi);
                    if (!dr.IsDBNull(iPingcodi)) entity.Pingcodi = Convert.ToInt32(dr.GetValue(iPingcodi));

                    int iPempagpeajepago = dr.GetOrdinal(this.helper.Pempagpeajepago);
                    if (!dr.IsDBNull(iPempagpeajepago)) entity.Pempagpeajepago = Convert.ToDecimal(dr.GetValue(iPempagpeajepago));

                    int iPempagsaldoanterior = dr.GetOrdinal(this.helper.Pempagsaldoanterior);
                    if (!dr.IsDBNull(iPempagsaldoanterior)) entity.Pempagsaldoanterior = Convert.ToDecimal(dr.GetValue(iPempagsaldoanterior));

                    int iPempagsaldo = dr.GetOrdinal(this.helper.Pempagsaldo);
                    if (!dr.IsDBNull(iPempagsaldo)) entity.Pempagsaldo = Convert.ToDecimal(dr.GetValue(iPempagsaldo));

                    int iPempagajuste = dr.GetOrdinal(this.helper.Pempagajuste);
                    if (!dr.IsDBNull(iPempagajuste)) entity.Pempagajuste = Convert.ToDecimal(dr.GetValue(iPempagajuste));

                    int iEmprcodosinergminpeaje = dr.GetOrdinal(this.helper.Emprcodosinergminpeaje);
                    if (!dr.IsDBNull(iEmprcodosinergminpeaje)) entity.Emprcodosinergminpeaje = dr.GetString(iEmprcodosinergminpeaje);
                    int iEmprcodosinergmincargo = dr.GetOrdinal(this.helper.Emprcodosinergmincargo);
                    if (!dr.IsDBNull(iEmprcodosinergmincargo)) entity.Emprcodosinergmincargo = dr.GetString(iEmprcodosinergmincargo);

                    int iPingtipo = dr.GetOrdinal(helper.Pingtipo);
                    if (!dr.IsDBNull(iPingtipo)) entity.Pingtipo = dr.GetString(iPingtipo);
                    int iPingnombre = dr.GetOrdinal(helper.Pingnombre);
                    if (!dr.IsDBNull(iPingnombre)) entity.Pingnombre = dr.GetString(iPingnombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<VtpPeajeEmpresaPagoDTO> ListPeajeCobroReparto(int rrpecodi, int emprcodipeaje, int pericodi, int recpotcodi)
        {
            List<VtpPeajeEmpresaPagoDTO> entitys = new List<VtpPeajeEmpresaPagoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListPeajeCobroReparto);
            dbProvider.AddInParameter(command, helper.Rrpecodi, DbType.Int32, rrpecodi);
            dbProvider.AddInParameter(command, helper.Emprcodipeaje, DbType.Int32, emprcodipeaje);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpPeajeEmpresaPagoDTO entity = new VtpPeajeEmpresaPagoDTO();

                    int iEmprcodipeaje = dr.GetOrdinal(this.helper.Emprcodipeaje);
                    if (!dr.IsDBNull(iEmprcodipeaje)) entity.Emprcodipeaje = Convert.ToInt32(dr.GetValue(iEmprcodipeaje));

                    int iEmprnombpeaje = dr.GetOrdinal(this.helper.Emprnombpeaje);
                    if (!dr.IsDBNull(iEmprnombpeaje)) entity.Emprnombpeaje = Convert.ToString(dr.GetValue(iEmprnombpeaje));

                    int iEmprcodicargo = dr.GetOrdinal(this.helper.Emprcodicargo);
                    if (!dr.IsDBNull(iEmprcodicargo)) entity.Emprcodicargo = Convert.ToInt32(dr.GetValue(iEmprcodicargo));

                    int iEmprnombcargo = dr.GetOrdinal(this.helper.Emprnombcargo);
                    if (!dr.IsDBNull(iEmprnombcargo)) entity.Emprnombcargo = Convert.ToString(dr.GetValue(iEmprnombcargo));

                    int iPingcodi = dr.GetOrdinal(this.helper.Pingcodi);
                    if (!dr.IsDBNull(iPingcodi)) entity.Pingcodi = Convert.ToInt32(dr.GetValue(iPingcodi));

                    int iPempagpeajepago = dr.GetOrdinal(this.helper.Pempagpeajepago);
                    if (!dr.IsDBNull(iPempagpeajepago)) entity.Pempagpeajepago = Convert.ToDecimal(dr.GetValue(iPempagpeajepago));

                    int iPempagsaldoanterior = dr.GetOrdinal(this.helper.Pempagsaldoanterior);
                    if (!dr.IsDBNull(iPempagsaldoanterior)) entity.Pempagsaldoanterior = Convert.ToDecimal(dr.GetValue(iPempagsaldoanterior));

                    int iPempagsaldo = dr.GetOrdinal(this.helper.Pempagsaldo);
                    if (!dr.IsDBNull(iPempagsaldo)) entity.Pempagsaldo = Convert.ToDecimal(dr.GetValue(iPempagsaldo));

                    int iPempagajuste = dr.GetOrdinal(this.helper.Pempagajuste);
                    if (!dr.IsDBNull(iPempagajuste)) entity.Pempagajuste = Convert.ToDecimal(dr.GetValue(iPempagajuste));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public decimal GetSaldoAnterior(int pempagpericodidest, int pingcodi, int emprcodipeaje, int emprcodicargo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetSaldoAnterior);
            dbProvider.AddInParameter(command, helper.Pempagpericodidest, DbType.Int32, pempagpericodidest);
            dbProvider.AddInParameter(command, helper.Pingcodi, DbType.Int32, pingcodi);
            dbProvider.AddInParameter(command, helper.Emprcodipeaje, DbType.Int32, emprcodipeaje);
            dbProvider.AddInParameter(command, helper.Emprcodipeaje, DbType.Int32, emprcodipeaje);
            dbProvider.AddInParameter(command, helper.Emprcodicargo, DbType.Int32, emprcodicargo);
            dbProvider.AddInParameter(command, helper.Emprcodicargo, DbType.Int32, emprcodicargo);
            object result = dbProvider.ExecuteScalar(command);
            decimal dSaldo = 0;
            if (result != null) dSaldo = Convert.ToDecimal(result);
            return dSaldo;
        }

        public List<VtpPeajeEmpresaPagoDTO> GetByIdSaldo(int pericodi, int recpotcodi, int pingcodi, int emprcodipeaje, int emprcodicargo)
        {
            List<VtpPeajeEmpresaPagoDTO> entitys = new List<VtpPeajeEmpresaPagoDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdSaldo);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);
            dbProvider.AddInParameter(command, helper.Pingcodi, DbType.Int32, pingcodi);
            dbProvider.AddInParameter(command, helper.Emprcodipeaje, DbType.Int32, emprcodipeaje);
            dbProvider.AddInParameter(command, helper.Emprcodipeaje, DbType.Int32, emprcodipeaje);
            dbProvider.AddInParameter(command, helper.Emprcodipeaje, DbType.Int32, emprcodipeaje);
            dbProvider.AddInParameter(command, helper.Emprcodicargo, DbType.Int32, emprcodicargo);
            dbProvider.AddInParameter(command, helper.Emprcodicargo, DbType.Int32, emprcodicargo);
            dbProvider.AddInParameter(command, helper.Emprcodicargo, DbType.Int32, emprcodicargo);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<VtpPeajeEmpresaPagoDTO> GetByCriteria()
        {
            List<VtpPeajeEmpresaPagoDTO> entitys = new List<VtpPeajeEmpresaPagoDTO>();
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

        public List<VtpPeajeEmpresaPagoDTO> ObtenerListVtpPeajeEmpresaPago(int pericodi, int recpotcodi, int? emprcodipeaje, string cargotransmision)
        {
            List<VtpPeajeEmpresaPagoDTO> entitys = new List<VtpPeajeEmpresaPagoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerListPeajeEmpresaPago);
            dbProvider.AddInParameter(command, helper.Pempagtransmision, DbType.String, cargotransmision);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);
            dbProvider.AddInParameter(command, helper.Emprcodipeaje, DbType.Int32, emprcodipeaje);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iEmprnombpeaje = dr.GetOrdinal(this.helper.Emprnombpeaje);
                    if (!dr.IsDBNull(iEmprnombpeaje)) entity.Emprnombpeaje = dr.GetString(iEmprnombpeaje);

                    int iEmprnombcargo = dr.GetOrdinal(this.helper.Emprnombcargo);
                    if (!dr.IsDBNull(iEmprnombcargo)) entity.Emprnombcargo = dr.GetString(iEmprnombcargo);

                    int iEmprcodosinergminpeaje = dr.GetOrdinal(this.helper.Emprcodosinergminpeaje);
                    if (!dr.IsDBNull(iEmprcodosinergminpeaje)) entity.Emprcodosinergminpeaje = dr.GetString(iEmprcodosinergminpeaje);

                    int iEmprcodosinergmincargo = dr.GetOrdinal(this.helper.Emprcodosinergmincargo);
                    if (!dr.IsDBNull(iEmprcodosinergmincargo)) entity.Emprcodosinergmincargo = dr.GetString(iEmprcodosinergmincargo);

                    int iPingtipo = dr.GetOrdinal(this.helper.Pingtipo);
                    if (!dr.IsDBNull(iPingtipo)) entity.Pingtipo = dr.GetString(iPingtipo);

                    int iPingnombre = dr.GetOrdinal(this.helper.Pingnombre);
                    if (!dr.IsDBNull(iPingnombre)) entity.Pingnombre = dr.GetString(iPingnombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }


        public List<VtpPeajeEmpresaPagoDTO> GetPeajeEmpresaPagoByComparative(int pericodiini, int pericodifin, int emprcodi, int emprcodicargo)
        {
            List<VtpPeajeEmpresaPagoDTO> entitys = new List<VtpPeajeEmpresaPagoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetPeajeEmpresaPagoByComparative);
            dbProvider.AddInParameter(command, helper.Pericodiini, DbType.Int32, pericodiini);
            dbProvider.AddInParameter(command, helper.Pericodifin, DbType.Int32, pericodifin);
            dbProvider.AddInParameter(command, helper.Emprcodipeaje, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Emprcodipeaje, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Emprcodicargo, DbType.Int32, emprcodicargo);
            dbProvider.AddInParameter(command, helper.Emprcodicargo, DbType.Int32, emprcodicargo);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                entitys = GetListIngresoPotByComparative(dr);
            }

            return entitys;
        }

        public List<VtpPeajeEmpresaPagoDTO> GetPeajeEmpresaPagoByComparativeUnique(int pericodiini, int pericodifin, int emprcodi, int emprcodicargo)
        {
            List<VtpPeajeEmpresaPagoDTO> entitys = new List<VtpPeajeEmpresaPagoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetPeajeEmpresaPagoByComparativeUnique);
            dbProvider.AddInParameter(command, helper.Pericodiini, DbType.Int32, pericodiini);
            dbProvider.AddInParameter(command, helper.Pericodifin, DbType.Int32, pericodifin);
            dbProvider.AddInParameter(command, helper.Emprcodipeaje, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Emprcodipeaje, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Emprcodicargo, DbType.Int32, emprcodicargo);
            dbProvider.AddInParameter(command, helper.Emprcodicargo, DbType.Int32, emprcodicargo);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                entitys = GetListIngresoPotByComparative(dr);
            }

            return entitys;
        }

        public List<VtpPeajeEmpresaPagoDTO> GetPeajeEmpresaPagoByCompHist(int pericodiini, int recpotcodiini, int pericodifin, int recpotcodifin, int emprcodi)
        {
            List<VtpPeajeEmpresaPagoDTO> entitys = new List<VtpPeajeEmpresaPagoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetPeajeEmpresaPagoByCompHist);
            dbProvider.AddInParameter(command, helper.Pericodiini, DbType.Int32, pericodiini);
            dbProvider.AddInParameter(command, helper.Recpotini, DbType.Int32, recpotcodiini);
            dbProvider.AddInParameter(command, helper.Pericodifin, DbType.Int32, pericodifin);
            dbProvider.AddInParameter(command, helper.Recpotfin, DbType.Int32, recpotcodifin);
            dbProvider.AddInParameter(command, helper.Emprcodipeaje, DbType.Int32, emprcodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                entitys = GetListIngresoPotByComparative(dr);
            }

            return entitys;
        }

        public List<VtpPeajeEmpresaPagoDTO> GetPeajeEmpresaPagoByCompHistUnique(int pericodiini, int recpotcodiini, int pericodifin, int recpotcodifin, int emprcodi)
        {
            List<VtpPeajeEmpresaPagoDTO> entitys = new List<VtpPeajeEmpresaPagoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetPeajeEmpresaPagoByCompHistUnique);
            dbProvider.AddInParameter(command, helper.Pericodiini, DbType.Int32, pericodiini);
            dbProvider.AddInParameter(command, helper.Recpotini, DbType.Int32, recpotcodiini);
            dbProvider.AddInParameter(command, helper.Pericodifin, DbType.Int32, pericodifin);
            dbProvider.AddInParameter(command, helper.Recpotfin, DbType.Int32, recpotcodifin);
            dbProvider.AddInParameter(command, helper.Emprcodipeaje, DbType.Int32, emprcodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                entitys = GetListIngresoPotByComparative(dr);
            }

            return entitys;
        }


        private List<VtpPeajeEmpresaPagoDTO> GetListIngresoPotByComparative(IDataReader dr)
        {
            List<VtpPeajeEmpresaPagoDTO> entitys = new List<VtpPeajeEmpresaPagoDTO>();
            while (dr.Read())
            {
                VtpPeajeEmpresaPagoDTO entity = new VtpPeajeEmpresaPagoDTO();

                int iPempagcodi = dr.GetOrdinal(this.helper.Pempagcodi);
                if (!dr.IsDBNull(iPempagcodi)) entity.Pempagcodi = Convert.ToInt32(dr.GetValue(iPempagcodi));

                int iPericodi = dr.GetOrdinal(this.helper.Pericodi);
                if (!dr.IsDBNull(iPericodi)) entity.Pericodi = Convert.ToInt32(dr.GetValue(iPericodi));

                int iRecpotcodi = dr.GetOrdinal(this.helper.Recpotcodi);
                if (!dr.IsDBNull(iRecpotcodi)) entity.Recpotcodi = Convert.ToInt32(dr.GetValue(iRecpotcodi));

                int iEmprcodipeaje = dr.GetOrdinal(this.helper.Emprcodipeaje);
                if (!dr.IsDBNull(iEmprcodipeaje)) entity.Emprcodipeaje = Convert.ToInt32(dr.GetValue(iEmprcodipeaje));

                int iEmprcodicargo = dr.GetOrdinal(this.helper.Emprcodicargo);
                if (!dr.IsDBNull(iEmprcodicargo)) entity.Emprcodicargo = Convert.ToInt32(dr.GetValue(iEmprcodicargo));

                int iPempagpeajepago = dr.GetOrdinal(this.helper.Pempagpeajepago);
                if (!dr.IsDBNull(iPempagpeajepago)) entity.Pempagpeajepago = dr.GetDecimal(iPempagpeajepago);

                int iRecpotnombre = dr.GetOrdinal(this.helper.Recpotnombre);
                if (!dr.IsDBNull(iRecpotnombre)) entity.Recpotnombre = dr.GetString(iRecpotnombre);

                int iPerinombre = dr.GetOrdinal(this.helper.Perinombre);
                if (!dr.IsDBNull(iPerinombre)) entity.Perinombre = dr.GetString(iPerinombre);

                int iPerianio = dr.GetOrdinal(this.helper.Perianio);
                if (!dr.IsDBNull(iPerianio)) entity.Perianio = Convert.ToInt32(dr.GetValue(iPerianio));

                int iPerimes = dr.GetOrdinal(this.helper.Perimes);
                if (!dr.IsDBNull(iPerimes)) entity.Perimes = Convert.ToInt32(dr.GetValue(iPerimes));

                int iPerianiomes = dr.GetOrdinal(this.helper.Perianiomes);
                if (!dr.IsDBNull(iPerianiomes)) entity.Perianiomes = Convert.ToInt32(dr.GetValue(iPerianiomes));

                int iRecanombre = dr.GetOrdinal(this.helper.Recanombre);
                if (!dr.IsDBNull(iRecanombre)) entity.Recanombre = dr.GetString(iRecanombre);

                int iEmprnombpeaje = dr.GetOrdinal(this.helper.Emprnombpeaje);
                if (!dr.IsDBNull(iEmprnombpeaje)) entity.Emprnombpeaje = dr.GetString(iEmprnombpeaje);

                int iEmprnombcargo = dr.GetOrdinal(this.helper.Emprnombcargo);
                if (!dr.IsDBNull(iEmprnombcargo)) entity.Emprnombcargo = dr.GetString(iEmprnombcargo);

                entitys.Add(entity);
            }
            return entitys;
        }

        public List<VtpPeajeEmpresaPagoDTO> GetPeajeEmpresaPagoByEmprCodiAndRecPotCodi(int emprcodicargo, int pericodi, int recpotcodi, int pingcodi)
        {
            List<VtpPeajeEmpresaPagoDTO> entitys = new List<VtpPeajeEmpresaPagoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetPeajeEmpresaPagoByEmprCodiAndRecPotCodi);
            dbProvider.AddInParameter(command, helper.Emprcodicargo, DbType.Int32, emprcodicargo);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);
            dbProvider.AddInParameter(command, helper.Pingcodi, DbType.Int32, pingcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpPeajeEmpresaPagoDTO entity = new VtpPeajeEmpresaPagoDTO();

                    int iEmprcodipeaje = dr.GetOrdinal(this.helper.Emprcodipeaje);
                    if (!dr.IsDBNull(iEmprcodipeaje)) entity.Emprcodipeaje = Convert.ToInt32(dr.GetValue(iEmprcodipeaje));

                    int iEmprnombpeaje = dr.GetOrdinal(this.helper.Emprnombpeaje);
                    if (!dr.IsDBNull(iEmprnombpeaje)) entity.Emprnombpeaje = Convert.ToString(dr.GetValue(iEmprnombpeaje));

                    int iEmprcodicargo = dr.GetOrdinal(this.helper.Emprcodicargo);
                    if (!dr.IsDBNull(iEmprcodicargo)) entity.Emprcodicargo = Convert.ToInt32(dr.GetValue(iEmprcodicargo));

                    int iEmprnombcargo = dr.GetOrdinal(this.helper.Emprnombcargo);
                    if (!dr.IsDBNull(iEmprnombcargo)) entity.Emprnombcargo = Convert.ToString(dr.GetValue(iEmprnombcargo));

                    int iPingcodi = dr.GetOrdinal(this.helper.Pingcodi);
                    if (!dr.IsDBNull(iPingcodi)) entity.Pingcodi = Convert.ToInt32(dr.GetValue(iPingcodi));

                    int iPempagpeajepago = dr.GetOrdinal(this.helper.Pempagpeajepago);
                    if (!dr.IsDBNull(iPempagpeajepago)) entity.Pempagpeajepago = Convert.ToDecimal(dr.GetValue(iPempagpeajepago));

                    int iPempagsaldoanterior = dr.GetOrdinal(this.helper.Pempagsaldoanterior);
                    if (!dr.IsDBNull(iPempagsaldoanterior)) entity.Pempagsaldoanterior = Convert.ToDecimal(dr.GetValue(iPempagsaldoanterior));

                    int iPempagsaldo = dr.GetOrdinal(this.helper.Pempagsaldo);
                    if (!dr.IsDBNull(iPempagsaldo)) entity.Pempagsaldo = Convert.ToDecimal(dr.GetValue(iPempagsaldo));

                    int iPempagajuste = dr.GetOrdinal(this.helper.Pempagajuste);
                    if (!dr.IsDBNull(iPempagajuste)) entity.Pempagajuste = Convert.ToDecimal(dr.GetValue(iPempagajuste));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        //CU21
        public VtpPeajeEmpresaPagoDTO GetByCargoPrima(int pericodi, int recpotcodi, string pingnombre)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCargoPrima);

            dbProvider.AddInParameter(command, helper.Pingcodi, DbType.String, pingnombre);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);
            VtpPeajeEmpresaPagoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new VtpPeajeEmpresaPagoDTO();

                    int iEmprcodicargo = dr.GetOrdinal(this.helper.Emprcodicargo);
                    if (!dr.IsDBNull(iEmprcodicargo)) entity.Emprcodicargo = Convert.ToInt32(dr.GetValue(iEmprcodicargo));

                    int iEmprnombcargo = dr.GetOrdinal(this.helper.Emprnombcargo);
                    if (!dr.IsDBNull(iEmprnombcargo)) entity.Emprnombcargo = Convert.ToString(dr.GetValue(iEmprnombcargo));

                    int iPingnombre = dr.GetOrdinal(this.helper.Pingnombre);
                    if (!dr.IsDBNull(iPingnombre)) entity.Pingnombre = dr.GetString(iPingnombre);

                    int iPempagsaldo = dr.GetOrdinal(this.helper.Pempagsaldo);
                    if (!dr.IsDBNull(iPempagsaldo)) entity.Pempagsaldo = Convert.ToDecimal(dr.GetValue(iPempagsaldo));

                }
            }

            return entity;
        }
    }
}
