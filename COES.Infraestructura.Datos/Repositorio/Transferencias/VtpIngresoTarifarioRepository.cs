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
    /// Clase de acceso a datos de la tabla VTP_INGRESO_TARIFARIO
    /// </summary>
    public class VtpIngresoTarifarioRepository : RepositoryBase, IVtpIngresoTarifarioRepository
    {
        public VtpIngresoTarifarioRepository(string strConn)
            : base(strConn)
        {
        }

        VtpIngresoTarifarioHelper helper = new VtpIngresoTarifarioHelper();

        public int Save(VtpIngresoTarifarioDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Ingtarcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, entity.Recpotcodi);
            dbProvider.AddInParameter(command, helper.Pingcodi, DbType.Int32, entity.Pingcodi);
            dbProvider.AddInParameter(command, helper.Emprcodiping, DbType.Int32, entity.Emprcodiping);
            dbProvider.AddInParameter(command, helper.Ingtartarimensual, DbType.Decimal, entity.Ingtartarimensual);
            dbProvider.AddInParameter(command, helper.Emprcodingpot, DbType.Int32, entity.Emprcodingpot);
            dbProvider.AddInParameter(command, helper.Ingtarporcentaje, DbType.Decimal, entity.Ingtarporcentaje);
            dbProvider.AddInParameter(command, helper.Ingtarimporte, DbType.Decimal, entity.Ingtarimporte);
            dbProvider.AddInParameter(command, helper.Ingtarsaldoanterior, DbType.Decimal, entity.Ingtarsaldoanterior);
            dbProvider.AddInParameter(command, helper.Ingtarajuste, DbType.Decimal, entity.Ingtarajuste);
            dbProvider.AddInParameter(command, helper.Ingtarusucreacion, DbType.String, entity.Ingtarusucreacion);
            dbProvider.AddInParameter(command, helper.Ingtarfeccreacion, DbType.DateTime, entity.Ingtarfeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(VtpIngresoTarifarioDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Ingtarsaldo, DbType.Decimal, entity.Ingtarsaldo);
            dbProvider.AddInParameter(command, helper.Ingtarpericodidest, DbType.Int32, entity.Ingtarpericodidest);

            dbProvider.AddInParameter(command, helper.Ingtarcodi, DbType.Int32, entity.Ingtarcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void UpdatePeriodoDestino(int pericodi, int recpotcodi, int ingtarpericodidest)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdatePeriodoDestino);

            dbProvider.AddInParameter(command, helper.Ingtarpericodidest, DbType.Int32, ingtarpericodidest);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);
            

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int ingtarcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Ingtarcodi, DbType.Int32, ingtarcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteByCriteria(int pericodi, int recpotcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteByCriteria);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public VtpIngresoTarifarioDTO GetById(int ingtarcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Ingtarcodi, DbType.Int32, ingtarcodi);
            VtpIngresoTarifarioDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public VtpIngresoTarifarioDTO GetByIdSaldo(int pericodi, int recpotcodi, int pingcodi, int emprcodiping, int emprcodingpot)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdSaldo);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);
            dbProvider.AddInParameter(command, helper.Pingcodi, DbType.Int32, pingcodi);
            dbProvider.AddInParameter(command, helper.Emprcodiping, DbType.Int32, emprcodiping);
            dbProvider.AddInParameter(command, helper.Emprcodingpot, DbType.Int32, emprcodingpot);
            VtpIngresoTarifarioDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VtpIngresoTarifarioDTO> GetByCriteriaIngresoTarifarioSaldo(int pericodi, int recpotcodi, int pingcodi, int emprcodiping, int emprcodingpot)
        {
            List<VtpIngresoTarifarioDTO> entitys = new List<VtpIngresoTarifarioDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteriaIngresoTarifarioSaldo);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);
            dbProvider.AddInParameter(command, helper.Pingcodi, DbType.Int32, pingcodi);
            dbProvider.AddInParameter(command, helper.Emprcodiping, DbType.Int32, emprcodiping);
            dbProvider.AddInParameter(command, helper.Emprcodingpot, DbType.Int32, emprcodingpot);
            dbProvider.AddInParameter(command, helper.Emprcodingpot, DbType.Int32, emprcodingpot);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpIngresoTarifarioDTO entity = helper.Create(dr);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public decimal GetSaldoAnterior(int ingtarpericodidest, int pingcodi, int emprcodiping, int emprcodingpot)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetSaldoAnterior);
            dbProvider.AddInParameter(command, helper.Ingtarpericodidest, DbType.Int32, ingtarpericodidest);
            dbProvider.AddInParameter(command, helper.Pingcodi, DbType.Int32, pingcodi);
            dbProvider.AddInParameter(command, helper.Emprcodiping, DbType.Int32, emprcodiping);
            dbProvider.AddInParameter(command, helper.Emprcodiping, DbType.Int32, emprcodiping);
            dbProvider.AddInParameter(command, helper.Emprcodingpot, DbType.Int32, emprcodingpot);
            dbProvider.AddInParameter(command, helper.Emprcodingpot, DbType.Int32, emprcodingpot);
            object result = dbProvider.ExecuteScalar(command);
            decimal dSaldo = 0;
            if (result != null) dSaldo = Convert.ToDecimal(result);
            return dSaldo;
        }

        public List<VtpIngresoTarifarioDTO> List()
        {
            List<VtpIngresoTarifarioDTO> entitys = new List<VtpIngresoTarifarioDTO>();
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

        public List<VtpIngresoTarifarioDTO> ListEmpresaPago(int pericodi, int recpotcodi)
        {
            List<VtpIngresoTarifarioDTO> entitys = new List<VtpIngresoTarifarioDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListEmpresaPago);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpIngresoTarifarioDTO entity = new VtpIngresoTarifarioDTO();

                    int iEmprcodingpot = dr.GetOrdinal(this.helper.Emprcodingpot);
                    if (!dr.IsDBNull(iEmprcodingpot)) entity.Emprcodingpot = Convert.ToInt32(dr.GetValue(iEmprcodingpot));

                    int iEmprnombingpot = dr.GetOrdinal(this.helper.Emprnombingpot);
                    if (!dr.IsDBNull(iEmprnombingpot)) entity.Emprnombingpot = Convert.ToString(dr.GetValue(iEmprnombingpot));

                    int iEmprruc = dr.GetOrdinal(this.helper.Emprruc);
                    if (!dr.IsDBNull(iEmprruc)) entity.Emprruc = Convert.ToString(dr.GetValue(iEmprruc));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<VtpIngresoTarifarioDTO> ListEmpresaCobroParaMultEmprcodingpot(string emprcodingpot, int pericodi, int recpotcodi)
        {
            List<VtpIngresoTarifarioDTO> entitys = new List<VtpIngresoTarifarioDTO>();
            var query = string.Format(helper.SqlListEmpresaCobroParaMultEmprcodingpot, emprcodingpot, pericodi, recpotcodi);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpIngresoTarifarioDTO entity = new VtpIngresoTarifarioDTO();

                    int iEmprcodingpot = dr.GetOrdinal(this.helper.Emprcodingpot);
                    if (!dr.IsDBNull(iEmprcodingpot)) entity.Emprcodingpot = Convert.ToInt32(dr.GetValue(iEmprcodingpot));

                    int iEmprnombingpot = dr.GetOrdinal(this.helper.Emprnombingpot);
                    if (!dr.IsDBNull(iEmprnombingpot)) entity.Emprnombingpot = Convert.ToString(dr.GetValue(iEmprnombingpot));

                    int iEmprcodiping = dr.GetOrdinal(this.helper.Emprcodiping);
                    if (!dr.IsDBNull(iEmprcodiping)) entity.Emprcodiping = Convert.ToInt32(dr.GetValue(iEmprcodiping));

                    int iEmprnombping = dr.GetOrdinal(this.helper.Emprnombping);
                    if (!dr.IsDBNull(iEmprnombping)) entity.Emprnombping = Convert.ToString(dr.GetValue(iEmprnombping));

                    int iPingcodi = dr.GetOrdinal(this.helper.Pingcodi);
                    if (!dr.IsDBNull(iPingcodi)) entity.Pingcodi = Convert.ToInt32(dr.GetValue(iPingcodi));

                    int iIngtarimporte = dr.GetOrdinal(this.helper.Ingtarimporte);
                    if (!dr.IsDBNull(iIngtarimporte)) entity.Ingtarimporte = Convert.ToDecimal(dr.GetValue(iIngtarimporte));

                    int iIngtarsaldoanterior = dr.GetOrdinal(this.helper.Ingtarsaldoanterior);
                    if (!dr.IsDBNull(iIngtarsaldoanterior)) entity.Ingtarsaldoanterior = Convert.ToDecimal(dr.GetValue(iIngtarsaldoanterior));

                    int iIngtarsaldo = dr.GetOrdinal(this.helper.Ingtarsaldo);
                    if (!dr.IsDBNull(iIngtarsaldo)) entity.Ingtarsaldo = Convert.ToDecimal(dr.GetValue(iIngtarsaldo));

                    int iIngtarajuste = dr.GetOrdinal(this.helper.Ingtarajuste);
                    if (!dr.IsDBNull(iIngtarajuste)) entity.Ingtarajuste = Convert.ToDecimal(dr.GetValue(iIngtarajuste));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<VtpIngresoTarifarioDTO> ListEmpresaCobro(int emprcodingpot, int pericodi, int recpotcodi)
        {
            List<VtpIngresoTarifarioDTO> entitys = new List<VtpIngresoTarifarioDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListEmpresaCobro);
            dbProvider.AddInParameter(command, helper.Emprcodingpot, DbType.Int32, emprcodingpot);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpIngresoTarifarioDTO entity = new VtpIngresoTarifarioDTO();

                    int iEmprcodingpot = dr.GetOrdinal(this.helper.Emprcodingpot);
                    if (!dr.IsDBNull(iEmprcodingpot)) entity.Emprcodingpot = Convert.ToInt32(dr.GetValue(iEmprcodingpot));

                    int iEmprnombingpot = dr.GetOrdinal(this.helper.Emprnombingpot);
                    if (!dr.IsDBNull(iEmprnombingpot)) entity.Emprnombingpot = Convert.ToString(dr.GetValue(iEmprnombingpot));

                    int iEmprcodiping = dr.GetOrdinal(this.helper.Emprcodiping);
                    if (!dr.IsDBNull(iEmprcodiping)) entity.Emprcodiping = Convert.ToInt32(dr.GetValue(iEmprcodiping));

                    int iEmprruc = dr.GetOrdinal(this.helper.Emprruc);
                    if (!dr.IsDBNull(iEmprruc)) entity.Emprruc = Convert.ToString(dr.GetValue(iEmprruc));

                    int iEmprnombping = dr.GetOrdinal(this.helper.Emprnombping);
                    if (!dr.IsDBNull(iEmprnombping)) entity.Emprnombping = Convert.ToString(dr.GetValue(iEmprnombping));

                    int iPingcodi = dr.GetOrdinal(this.helper.Pingcodi);
                    if (!dr.IsDBNull(iPingcodi)) entity.Pingcodi = Convert.ToInt32(dr.GetValue(iPingcodi));

                    int iIngtarimporte = dr.GetOrdinal(this.helper.Ingtarimporte);
                    if (!dr.IsDBNull(iIngtarimporte)) entity.Ingtarimporte = Convert.ToDecimal(dr.GetValue(iIngtarimporte));

                    int iIngtarsaldoanterior = dr.GetOrdinal(this.helper.Ingtarsaldoanterior);
                    if (!dr.IsDBNull(iIngtarsaldoanterior)) entity.Ingtarsaldoanterior = Convert.ToDecimal(dr.GetValue(iIngtarsaldoanterior));

                    int iIngtarsaldo = dr.GetOrdinal(this.helper.Ingtarsaldo);
                    if (!dr.IsDBNull(iIngtarsaldo)) entity.Ingtarsaldo = Convert.ToDecimal(dr.GetValue(iIngtarsaldo));

                    int iIngtarajuste = dr.GetOrdinal(this.helper.Ingtarajuste);
                    if (!dr.IsDBNull(iIngtarajuste)) entity.Ingtarajuste = Convert.ToDecimal(dr.GetValue(iIngtarajuste));

                    int iEmprcodosinergminingpot = dr.GetOrdinal(this.helper.Emprcodosinergminingpot);
                    if (!dr.IsDBNull(iEmprcodosinergminingpot)) entity.Emprcodosinergminingpot = dr.GetString(iEmprcodosinergminingpot);
                    int iEmprcodosinergminping = dr.GetOrdinal(this.helper.Emprcodosinergminping);
                    if (!dr.IsDBNull(iEmprcodosinergminping)) entity.Emprcodosinergminping = dr.GetString(iEmprcodosinergminping);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<VtpIngresoTarifarioDTO> GetByCriteria()
        {
            List<VtpIngresoTarifarioDTO> entitys = new List<VtpIngresoTarifarioDTO>();
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
