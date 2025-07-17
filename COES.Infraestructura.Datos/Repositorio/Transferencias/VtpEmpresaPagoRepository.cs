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
    /// Clase de acceso a datos de la tabla VTP_EMPRESA_PAGO
    /// </summary>
    public class VtpEmpresaPagoRepository: RepositoryBase, IVtpEmpresaPagoRepository
    {
        public VtpEmpresaPagoRepository(string strConn): base(strConn)
        {
        }

        VtpEmpresaPagoHelper helper = new VtpEmpresaPagoHelper();

        public int Save(VtpEmpresaPagoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Potepcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, entity.Recpotcodi);
            dbProvider.AddInParameter(command, helper.Potsecodi, DbType.Int32, entity.Potsecodi);
            dbProvider.AddInParameter(command, helper.Emprcodipago, DbType.Int32, entity.Emprcodipago);
            dbProvider.AddInParameter(command, helper.Emprcodicobro, DbType.Int32, entity.Emprcodicobro);
            dbProvider.AddInParameter(command, helper.Potepmonto, DbType.Decimal, entity.Potepmonto);
            dbProvider.AddInParameter(command, helper.Potepusucreacion, DbType.String, entity.Potepusucreacion);
            dbProvider.AddInParameter(command, helper.Potepfeccreacion, DbType.DateTime, DateTime.Now);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(VtpEmpresaPagoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, entity.Recpotcodi);
            dbProvider.AddInParameter(command, helper.Potsecodi, DbType.Int32, entity.Potsecodi);
            dbProvider.AddInParameter(command, helper.Emprcodipago, DbType.Int32, entity.Emprcodipago);
            dbProvider.AddInParameter(command, helper.Emprcodicobro, DbType.Int32, entity.Emprcodicobro);
            dbProvider.AddInParameter(command, helper.Potepmonto, DbType.Decimal, entity.Potepmonto);
            dbProvider.AddInParameter(command, helper.Potepusucreacion, DbType.String, entity.Potepusucreacion);
            dbProvider.AddInParameter(command, helper.Potepfeccreacion, DbType.DateTime, DateTime.Now);

            dbProvider.AddInParameter(command, helper.Potepcodi, DbType.Int32, entity.Potepcodi);
            
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int potepcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Potepcodi, DbType.Int32, potepcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteByCriteria(int pericodi, int recpotcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteByCriteria);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public VtpEmpresaPagoDTO GetById(int potepcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Potepcodi, DbType.Int32, potepcodi);
            VtpEmpresaPagoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VtpEmpresaPagoDTO> List()
        {
            List<VtpEmpresaPagoDTO> entitys = new List<VtpEmpresaPagoDTO>();
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

        public List<VtpEmpresaPagoDTO> ListPago(int pericodi, int recpotcodi)
        {
            List<VtpEmpresaPagoDTO> entitys = new List<VtpEmpresaPagoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListPago);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpEmpresaPagoDTO entity = new VtpEmpresaPagoDTO();

                    int iEmprcodipago = dr.GetOrdinal(this.helper.Emprcodipago);
                    if (!dr.IsDBNull(iEmprcodipago)) entity.Emprcodipago = Convert.ToInt32(dr.GetValue(iEmprcodipago));

                    int iEmprnombpago = dr.GetOrdinal(this.helper.Emprnombpago);
                    if (!dr.IsDBNull(iEmprnombpago)) entity.Emprnombpago = Convert.ToString(dr.GetValue(iEmprnombpago));

                    int iEmprcodosinergminpago = dr.GetOrdinal(this.helper.Emprcodosinergminpago);
                    if (!dr.IsDBNull(iEmprcodosinergminpago)) entity.Emprcodosinergminpago = dr.GetString(iEmprcodosinergminpago);

                    int iEmprruc = dr.GetOrdinal(this.helper.Emprruc);
                    if (!dr.IsDBNull(iEmprruc)) entity.EmprRuc = dr.GetString(iEmprruc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<VtpEmpresaPagoDTO> ListCobro(int emprcodipago, int pericodi, int recpotcodi)
        {
            List<VtpEmpresaPagoDTO> entitys = new List<VtpEmpresaPagoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListCobro);
            dbProvider.AddInParameter(command, helper.Emprcodipago, DbType.Int32, emprcodipago);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);
            
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpEmpresaPagoDTO entity = new VtpEmpresaPagoDTO();

                    int iEmprcodipago = dr.GetOrdinal(this.helper.Emprcodipago);
                    if (!dr.IsDBNull(iEmprcodipago)) entity.Emprcodipago = Convert.ToInt32(dr.GetValue(iEmprcodipago));

                    int iEmprnombpago = dr.GetOrdinal(this.helper.Emprnombpago);
                    if (!dr.IsDBNull(iEmprnombpago)) entity.Emprnombpago = Convert.ToString(dr.GetValue(iEmprnombpago));

                    int iEmprcodicobro = dr.GetOrdinal(this.helper.Emprcodicobro);
                    if (!dr.IsDBNull(iEmprcodicobro)) entity.Emprcodicobro = Convert.ToInt32(dr.GetValue(iEmprcodicobro));

                    int iEmprruc = dr.GetOrdinal(this.helper.Emprruc);
                    if (!dr.IsDBNull(iEmprruc)) entity.EmprRuc = Convert.ToString(dr.GetValue(iEmprruc));

                    int iEmprnombcobro = dr.GetOrdinal(this.helper.Emprnombcobro);
                    if (!dr.IsDBNull(iEmprnombcobro)) entity.Emprnombcobro = Convert.ToString(dr.GetValue(iEmprnombcobro));

                    int iPotepmonto = dr.GetOrdinal(this.helper.Potepmonto);
                    if (!dr.IsDBNull(iPotepmonto)) entity.Potepmonto = Convert.ToDecimal(dr.GetValue(iPotepmonto));

                    int iEmprcodosinergminpago = dr.GetOrdinal(this.helper.Emprcodosinergminpago);
                    if (!dr.IsDBNull(iEmprcodosinergminpago)) entity.Emprcodosinergminpago = dr.GetString(iEmprcodosinergminpago);

                    int iEmprcodosinergmincobro = dr.GetOrdinal(this.helper.Emprcodosinergmincobro);
                    if (!dr.IsDBNull(iEmprcodosinergmincobro)) entity.Emprcodosinergmincobro = dr.GetString(iEmprcodosinergmincobro);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<VtpEmpresaPagoDTO> ListCobroConsultaHistoricos(int emprcodipago, int pericodi, int recpotcodi)
        {
            List<VtpEmpresaPagoDTO> entitys = new List<VtpEmpresaPagoDTO>();
            string sqlQuery = string.Format(this.helper.SqlListCobroConsultaHistoricos, pericodi, recpotcodi, emprcodipago);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                entitys = GetListEmpresaPago(dr);
            }

            return entitys;
        }

        public List<VtpEmpresaPagoDTO> GetByCriteria()
        {
            List<VtpEmpresaPagoDTO> entitys = new List<VtpEmpresaPagoDTO>();
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

        public List<VtpEmpresaPagoDTO> ObtenerListaEmpresaPago(int pericodi, int recpotcodi, int? emprcodipago, int? emprcodicobro)
        {

            List<VtpEmpresaPagoDTO> entitys = new List<VtpEmpresaPagoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerListaEmpresaPago);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);
            dbProvider.AddInParameter(command, helper.Emprcodipago, DbType.Int32, emprcodipago);
            dbProvider.AddInParameter(command, helper.Emprcodipago, DbType.Int32, emprcodipago);
            dbProvider.AddInParameter(command, helper.Emprcodicobro, DbType.Int32, emprcodicobro);
            dbProvider.AddInParameter(command, helper.Emprcodicobro, DbType.Int32, emprcodicobro);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iEmprnombpago = dr.GetOrdinal(this.helper.Emprnombpago);
                    if (!dr.IsDBNull(iEmprnombpago)) entity.Emprnombpago = Convert.ToString(dr.GetValue(iEmprnombpago));

                    int iEmprcodosinergminpago = dr.GetOrdinal(this.helper.Emprcodosinergminpago);
                    if (!dr.IsDBNull(iEmprcodosinergminpago)) entity.Emprcodosinergminpago = dr.GetString(iEmprcodosinergminpago);

                    int iEmprnombcobro = dr.GetOrdinal(this.helper.Emprnombcobro);
                    if (!dr.IsDBNull(iEmprnombcobro)) entity.Emprnombcobro = Convert.ToString(dr.GetValue(iEmprnombcobro));

                    int iEmprcodosinergmincobro = dr.GetOrdinal(this.helper.Emprcodosinergmincobro);
                    if (!dr.IsDBNull(iEmprcodosinergmincobro)) entity.Emprcodosinergmincobro = dr.GetString(iEmprcodosinergmincobro);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }


        public List<VtpEmpresaPagoDTO> GetEmpresaPagoByComparative(int pericodiini, int pericodifin, int emprcodi)
        {
            List<VtpEmpresaPagoDTO> entitys = new List<VtpEmpresaPagoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetEmpresaPagoByComparative);
            dbProvider.AddInParameter(command, helper.Pericodiini, DbType.Int32, pericodiini);
            dbProvider.AddInParameter(command, helper.Pericodifin, DbType.Int32, pericodifin);
            dbProvider.AddInParameter(command, helper.Emprcodipago, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Emprcodipago, DbType.Int32, emprcodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                entitys = GetListEmpresaPago(dr);
            }

            return entitys;
        }

        public List<VtpEmpresaPagoDTO> GetEmpresaPagoByComparativeHistorico(int pericodiini, int pericodifin, int emprcodi)
        {
            List<VtpEmpresaPagoDTO> entitys = new List<VtpEmpresaPagoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetEmpresaPagoHistoricoByComparative);
            dbProvider.AddInParameter(command, helper.Pericodiini, DbType.Int32, pericodiini);
            dbProvider.AddInParameter(command, helper.Pericodifin, DbType.Int32, pericodifin);
            dbProvider.AddInParameter(command, helper.Emprcodipago, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Emprcodipago, DbType.Int32, emprcodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                entitys = GetListEmpresaPago(dr);
            }

            return entitys;
        }
        public List<VtpEmpresaPagoDTO> GetEmpresaPagoByComparativeHistorico2(int pericodiini, int recpotcodiini, int pericodifin, int recpotcodifin, int emprcodi)
        {
            List<VtpEmpresaPagoDTO> entitys = new List<VtpEmpresaPagoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetEmpresaPagoHistoricoByComparative2);
            dbProvider.AddInParameter(command, helper.Pericodiini, DbType.Int32, pericodiini);
            dbProvider.AddInParameter(command, helper.Recpotini, DbType.Int32, recpotcodiini);
            dbProvider.AddInParameter(command, helper.Pericodifin, DbType.Int32, pericodifin);
            dbProvider.AddInParameter(command, helper.Recpotfin, DbType.Int32, recpotcodifin);
            dbProvider.AddInParameter(command, helper.Emprcodipago, DbType.Int32, emprcodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                entitys = GetListEmpresaPago(dr);
            }

            return entitys;
        }

        public List<VtpEmpresaPagoDTO> GetEmpresaPagoByComparativeUnique(int pericodiini, int pericodifin, int emprcodi)
        {
            List<VtpEmpresaPagoDTO> entitys = new List<VtpEmpresaPagoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetEmpresaPagoByComparativeUnique);
            dbProvider.AddInParameter(command, helper.Pericodiini, DbType.Int32, pericodiini);
            dbProvider.AddInParameter(command, helper.Pericodifin, DbType.Int32, pericodifin);
            dbProvider.AddInParameter(command, helper.Emprcodipago, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Emprcodipago, DbType.Int32, emprcodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                entitys = GetListEmpresaPago(dr);
            }

            return entitys;
        }

        public List<VtpEmpresaPagoDTO> GetEmpresaPagoByHist(int pericodiini, int recpotcodiini, int pericodifin, int recpotcodifin, int emprcodi)
        {
            List<VtpEmpresaPagoDTO> entitys = new List<VtpEmpresaPagoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetEmpresaPagoByHist);
            dbProvider.AddInParameter(command, helper.Pericodiini, DbType.Int32, pericodiini);
            dbProvider.AddInParameter(command, helper.Recpotini, DbType.Int32, recpotcodiini);
            dbProvider.AddInParameter(command, helper.Pericodifin, DbType.Int32, pericodifin);
            dbProvider.AddInParameter(command, helper.Recpotfin, DbType.Int32, recpotcodifin);
            dbProvider.AddInParameter(command, helper.Emprcodipago, DbType.Int32, emprcodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                entitys = GetListEmpresaPago(dr);
            }

            return entitys;
        }

        public List<VtpEmpresaPagoDTO> GetEmpresaPagoByHistUnique(int pericodiini, int recpotcodiini, int pericodifin, int recpotcodifin, int emprcodi)
        {
            List<VtpEmpresaPagoDTO> entitys = new List<VtpEmpresaPagoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetEmpresaPagoByHistUnique);
            dbProvider.AddInParameter(command, helper.Pericodiini, DbType.Int32, pericodiini);
            dbProvider.AddInParameter(command, helper.Recpotini, DbType.Int32, recpotcodiini);
            dbProvider.AddInParameter(command, helper.Pericodifin, DbType.Int32, pericodifin);
            dbProvider.AddInParameter(command, helper.Recpotfin, DbType.Int32, recpotcodifin);
            dbProvider.AddInParameter(command, helper.Emprcodipago, DbType.Int32, emprcodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                entitys = GetListEmpresaPago(dr);
            }

            return entitys;
        }


        private List<VtpEmpresaPagoDTO> GetListEmpresaPago(IDataReader dr)
        {
            List<VtpEmpresaPagoDTO> entitys = new List<VtpEmpresaPagoDTO>();
            while (dr.Read())
            {
                VtpEmpresaPagoDTO entity = new VtpEmpresaPagoDTO();

                int iPotepcodi = dr.GetOrdinal(this.helper.Potepcodi);
                if (!dr.IsDBNull(iPotepcodi)) entity.Potepcodi = Convert.ToInt32(dr.GetValue(iPotepcodi));

                int iPericodi = dr.GetOrdinal(this.helper.Pericodi);
                if (!dr.IsDBNull(iPericodi)) entity.Pericodi = Convert.ToInt32(dr.GetValue(iPericodi));

                int iRecpotcodi = dr.GetOrdinal(this.helper.Recpotcodi);
                if (!dr.IsDBNull(iRecpotcodi)) entity.Recpotcodi = Convert.ToInt32(dr.GetValue(iRecpotcodi));

                int iPotsecodi = dr.GetOrdinal(this.helper.Potsecodi);
                if (!dr.IsDBNull(iPotsecodi)) entity.Potsecodi = Convert.ToInt32(dr.GetValue(iPotsecodi));

                int iEmprcodipago = dr.GetOrdinal(this.helper.Emprcodipago);
                if (!dr.IsDBNull(iEmprcodipago)) entity.Emprcodipago = Convert.ToInt32(dr.GetValue(iEmprcodipago));

                int iEmprcodicobro = dr.GetOrdinal(this.helper.Emprcodicobro);
                if (!dr.IsDBNull(iEmprcodicobro)) entity.Emprcodicobro = Convert.ToInt32(dr.GetValue(iEmprcodicobro));

                int iPotepmonto = dr.GetOrdinal(this.helper.Potepmonto);
                if (!dr.IsDBNull(iPotepmonto)) entity.Potepmonto = dr.GetDecimal(iPotepmonto);

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

                int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                if (!dr.IsDBNull(iEmprnomb)) entity.Emprnombpago = dr.GetString(iEmprnomb);

                entitys.Add(entity);
            }
            return entitys;
        }
    }
}
