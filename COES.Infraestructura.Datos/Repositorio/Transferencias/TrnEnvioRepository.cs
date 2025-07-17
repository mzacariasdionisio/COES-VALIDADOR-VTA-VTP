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
    // ASSETEC 2019-11
    /// <summary>
    /// Clase que contiene las operaciones con la tabla TRN_ENVIO
    /// </summary>
    public class TrnEnvioRepository : RepositoryBase, ITrnEnvioRepository
    {
        public TrnEnvioRepository(string strConn) : base(strConn)
        {

        }

        TrnEnvioHelper helper = new TrnEnvioHelper();

        #region Metodos Basicos Trn_Envio
        public int Save(TrnEnvioDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            DateTime dFechaSistema = DateTime.Now;
            dbProvider.AddInParameter(command, helper.TrnEnvCodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, entity.PeriCodi);
            dbProvider.AddInParameter(command, helper.RecaCodi, DbType.Int32, entity.RecaCodi);
            dbProvider.AddInParameter(command, helper.EmprCodi, DbType.Int32, entity.EmprCodi);
            dbProvider.AddInParameter(command, helper.TrnModCodi, DbType.Int32, entity.TrnModCodi);
            dbProvider.AddInParameter(command, helper.TrnEnvTipInf, DbType.String, entity.TrnEnvTipInf);
            dbProvider.AddInParameter(command, helper.TrnEnvPlazo, DbType.String, entity.TrnEnvPlazo);
            dbProvider.AddInParameter(command, helper.TrnEnvLiqVt, DbType.String, entity.TrnEnvLiqVt);
            dbProvider.AddInParameter(command, helper.TrnEnvUseIns, DbType.String, entity.TrnEnvUseIns);
            dbProvider.AddInParameter(command, helper.TrnEnvFecIns, DbType.DateTime, dFechaSistema);
            dbProvider.AddInParameter(command, helper.TrnEnvUseAct, DbType.String, entity.TrnEnvUseAct);
            dbProvider.AddInParameter(command, helper.TrnEnvFecAct, DbType.DateTime, dFechaSistema);
            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public int Update(TrnEnvioDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, entity.PeriCodi);
            dbProvider.AddInParameter(command, helper.RecaCodi, DbType.Int32, entity.RecaCodi);
            dbProvider.AddInParameter(command, helper.EmprCodi, DbType.Int32, entity.EmprCodi);
            dbProvider.AddInParameter(command, helper.TrnEnvTipInf, DbType.String, entity.TrnEnvTipInf);
            dbProvider.AddInParameter(command, helper.TrnEnvPlazo, DbType.String, entity.TrnEnvPlazo);
            dbProvider.AddInParameter(command, helper.TrnEnvLiqVt, DbType.String, entity.TrnEnvLiqVt);
            dbProvider.AddInParameter(command, helper.TrnEnvUseAct, DbType.String, entity.TrnEnvUseAct);
            dbProvider.AddInParameter(command, helper.TrnEnvFecAct, DbType.DateTime, entity.TrnEnvFecAct);
            dbProvider.AddInParameter(command, helper.TrnEnvCodi, DbType.Int32, entity.TrnEnvCodi);

            return dbProvider.ExecuteNonQuery(command);
        }

        public int Delete(int id)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.TrnEnvCodi, DbType.Int32, id);

            return dbProvider.ExecuteNonQuery(command);
        }

        public TrnEnvioDTO GetById(int id)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.TrnEnvCodi, DbType.Int32, id);
            TrnEnvioDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    // EMPRNOMB
                    int iEmprNomb = dr.GetOrdinal(this.helper.EmprNomb);
                    if (!dr.IsDBNull(iEmprNomb)) entity.EmprNomb = dr.GetString(iEmprNomb);

                    // PERINOMBRE
                    int iPeriNombre = dr.GetOrdinal(this.helper.PeriNombre);
                    if (!dr.IsDBNull(iPeriNombre)) entity.PeriNombre = dr.GetString(iPeriNombre);

                    // trnmodnombre
                    int iTrnModNombre = dr.GetOrdinal(this.helper.TrnModNombre);
                    if (!dr.IsDBNull(iTrnModNombre)) entity.TrnModNombre = dr.GetString(iTrnModNombre);
                }
            }

            return entity;
        }

        public List<TrnEnvioDTO> List(int PeriCodi, int RecaCodi, int EmprCodi, string TrnEnvTipInf, int trnmodcodi)
        {
            List<TrnEnvioDTO> entitys = new List<TrnEnvioDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, PeriCodi);
            dbProvider.AddInParameter(command, helper.RecaCodi, DbType.Int32, RecaCodi);
            dbProvider.AddInParameter(command, helper.EmprCodi, DbType.Int32, EmprCodi);
            dbProvider.AddInParameter(command, helper.TrnEnvTipInf, DbType.String, TrnEnvTipInf);
            dbProvider.AddInParameter(command, helper.TrnModCodi, DbType.Int32, trnmodcodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<TrnEnvioDTO> ListIntranet(int pericodi, int recacodi, int emprcodi, string tipoinfocodi, string trnenvplazo, string trnenvliqvt)
        {
            List<TrnEnvioDTO> entitys = new List<TrnEnvioDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListIntranet);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.RecaCodi, DbType.Int32, recacodi);

            dbProvider.AddInParameter(command, helper.EmprCodi, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.EmprCodi, DbType.Int32, emprcodi);

            dbProvider.AddInParameter(command, helper.TrnEnvTipInf, DbType.String, tipoinfocodi);
            dbProvider.AddInParameter(command, helper.TrnEnvTipInf, DbType.String, tipoinfocodi);

            dbProvider.AddInParameter(command, helper.TrnEnvPlazo, DbType.String, trnenvplazo);
            dbProvider.AddInParameter(command, helper.TrnEnvPlazo, DbType.String, trnenvplazo);

            dbProvider.AddInParameter(command, helper.TrnEnvLiqVt, DbType.String, trnenvliqvt);
            dbProvider.AddInParameter(command, helper.TrnEnvLiqVt, DbType.String, trnenvliqvt);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TrnEnvioDTO entity = helper.Create(dr);

                    // EMPRNOMB
                    int iEmprNomb = dr.GetOrdinal(this.helper.EmprNomb);
                    if (!dr.IsDBNull(iEmprNomb)) entity.EmprNomb = dr.GetString(iEmprNomb);

                    // PERINOMBRE
                    int iPeriNombre = dr.GetOrdinal(this.helper.PeriNombre);
                    if (!dr.IsDBNull(iPeriNombre)) entity.PeriNombre = dr.GetString(iPeriNombre);

                    // trnmodnombre
                    int iTrnModNombre = dr.GetOrdinal(this.helper.TrnModNombre);
                    if (!dr.IsDBNull(iTrnModNombre)) entity.TrnModNombre = dr.GetString(iTrnModNombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<TrnEnvioDTO> GetByCriteria(int idEmpresa, int idPeriodo)
        {
            List<TrnEnvioDTO> entitys = new List<TrnEnvioDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            dbProvider.AddInParameter(command, helper.EmprCodi, DbType.Int32, idEmpresa);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, idPeriodo);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TrnEnvioDTO entity = helper.Create(dr);

                    // EMPRNOMB
                    int iEmprNomb = dr.GetOrdinal(this.helper.EmprNomb);
                    if (!dr.IsDBNull(iEmprNomb)) entity.EmprNomb = dr.GetString(iEmprNomb);

                    // PERINOMBRE
                    int iPeriNombre = dr.GetOrdinal(this.helper.PeriNombre);
                    if (!dr.IsDBNull(iPeriNombre)) entity.PeriNombre = dr.GetString(iPeriNombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion

        #region Metodos Adicionales Trn_Envio
        private int GetMaxIdTrnEnvio()
        {
            int newId = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            newId = Int32.Parse(dbProvider.ExecuteScalar(command).ToString());

            return newId;
        }

        public TrnEnvioDTO GetByIdPeriodoEmpresa(int pericodi, int recacodi, int emprcodi, string trnenvtipinf, int trnmodcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdPeriodoEmpresa);

            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.RecaCodi, DbType.Int32, recacodi);
            dbProvider.AddInParameter(command, helper.EmprCodi, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.TrnEnvTipInf, DbType.String, trnenvtipinf);
            dbProvider.AddInParameter(command, helper.TrnModCodi, DbType.Int32, trnmodcodi);
            TrnEnvioDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public int UpdateByCriteriaTrnEnvio(int pericodi, int recacodi, int emprcodi, int trnmodcodi, string trnenvtipinf, string suser)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateByCriteriaTrnEnvio);

            dbProvider.AddInParameter(command, helper.TrnEnvUseAct, DbType.String, suser);
            dbProvider.AddInParameter(command, helper.TrnEnvFecAct, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.RecaCodi, DbType.Int32, recacodi);
            dbProvider.AddInParameter(command, helper.EmprCodi, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.TrnModCodi, DbType.Int32, trnmodcodi);
            dbProvider.AddInParameter(command, helper.TrnEnvTipInf, DbType.String, trnenvtipinf);

            return dbProvider.ExecuteNonQuery(command);
        }

        public void UpdateTrnEnvioLiquidacion(TrnEnvioDTO entity)
        {
            //Actualizamos todos las Entregas/Retiros (todos los envios) a desactualizados tentestado = 'INA' / tretestado = 'INA'
            //Las Entregas y Retiros relacionados a trnenvcodi, tentestado = 'ACT' / tretestado = 'ACT'
            string sqlUpdate = string.Format(helper.SqlUpdateTrnEnvioLiquidacion, entity.PeriCodi, entity.RecaCodi, entity.EmprCodi, entity.TrnEnvCodi, entity.TrnEnvUseAct); //TRN_TRANS_ENTREGA/RETIRO
            DbCommand command = dbProvider.GetSqlStringCommand(sqlUpdate);
            command.CommandTimeout = 0;
            dbProvider.ExecuteNonQuery(command);
            command.Parameters.Clear();
        }

        public void UpdateEntregaLiquidacion(ExportExcelDTO entity)
        {
            //Actualizamos todos las Entregas (todos los envios ER/DM) a desactualizados tentestado = 'INA' 
            //La Entrega tentcodi, tentestado = 'ACT' 
            string sqlUpdate = string.Format(helper.SqlUpdateEntregaLiquidacion, entity.PeriCodi, entity.VtranVersion, entity.EmprCodi, entity.CodiEntreRetiCodi); //TRN_TRANS_ENTREGA
            DbCommand command = dbProvider.GetSqlStringCommand(sqlUpdate);
            command.CommandTimeout = 0;
            dbProvider.ExecuteNonQuery(command);
            command.Parameters.Clear();
        }

        public void UpdateRetiroLiquidacion(ExportExcelDTO entity, int trnenvcodi, int trnmodcodi, string suser)
        {
            //Actualizamos todos los Retiros (todos los envios ER/DM) a desactualizados tretestado = 'INA' 
            //El Retiro tretcodi, tretestado = 'ACT' 
            string sqlUpdate = string.Format(helper.SqlUpdateRetiroLiquidacion, entity.PeriCodi, entity.VtranVersion, entity.CodiEntreRetiCodi, trnenvcodi, trnmodcodi, suser); //TRN_TRANS_RETIRO codentcodi
            DbCommand command = dbProvider.GetSqlStringCommand(sqlUpdate);
            command.CommandTimeout = 0;
            dbProvider.ExecuteNonQuery(command);
            command.Parameters.Clear();
        }
        #endregion

    }
}