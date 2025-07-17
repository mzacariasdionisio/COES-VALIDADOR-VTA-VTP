using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Infraestructura.Datos.Helper.Transferencias;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase que contiene las operaciones con la base de datos
    /// </summary>
    public class FactorPerdidaRepository : RepositoryBase, IFactorPerdidaRepository
    {
        public FactorPerdidaRepository(string strConn) : base(strConn)
        {
        }

        FactorPerdidaHelper helper = new FactorPerdidaHelper();

        public int Save(FactorPerdidaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            int IdFactorPerdida = GetCodigoGenerado();
            dbProvider.AddInParameter(command, helper.FacPerCodi, DbType.Int32, IdFactorPerdida);
            dbProvider.AddInParameter(command, helper.BarrCodi, DbType.Int32, entity.BarrCodi);
            dbProvider.AddInParameter(command, helper.FacPerBarrNombre, DbType.String, entity.FacPerBarrNombre);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, entity.PeriCodi);
            dbProvider.AddInParameter(command, helper.FacPerBase, DbType.String, entity.FacPerBase);
            dbProvider.AddInParameter(command, helper.FacPerVersion, DbType.Int32, entity.FacPerVersion);
            dbProvider.AddInParameter(command, helper.FacPerDia, DbType.Int32, entity.FacPerDia);
            dbProvider.AddInParameter(command, helper.FacPer1, DbType.Double, entity.FacPer1);
            dbProvider.AddInParameter(command, helper.FacPer2, DbType.Double, entity.FacPer2);
            dbProvider.AddInParameter(command, helper.FacPer3, DbType.Double, entity.FacPer3);
            dbProvider.AddInParameter(command, helper.FacPer4, DbType.Double, entity.FacPer4);
            dbProvider.AddInParameter(command, helper.FacPer5, DbType.Double, entity.FacPer5);
            dbProvider.AddInParameter(command, helper.FacPer6, DbType.Double, entity.FacPer6);
            dbProvider.AddInParameter(command, helper.FacPer7, DbType.Double, entity.FacPer7);
            dbProvider.AddInParameter(command, helper.FacPer8, DbType.Double, entity.FacPer8);
            dbProvider.AddInParameter(command, helper.FacPer9, DbType.Double, entity.FacPer9);
            dbProvider.AddInParameter(command, helper.FacPer10, DbType.Double, entity.FacPer10);
            dbProvider.AddInParameter(command, helper.FacPer11, DbType.Double, entity.FacPer11);
            dbProvider.AddInParameter(command, helper.FacPer12, DbType.Double, entity.FacPer12);
            dbProvider.AddInParameter(command, helper.FacPer13, DbType.Double, entity.FacPer13);
            dbProvider.AddInParameter(command, helper.FacPer14, DbType.Double, entity.FacPer14);
            dbProvider.AddInParameter(command, helper.FacPer15, DbType.Double, entity.FacPer15);
            dbProvider.AddInParameter(command, helper.FacPer16, DbType.Double, entity.FacPer16);
            dbProvider.AddInParameter(command, helper.FacPer17, DbType.Double, entity.FacPer17);
            dbProvider.AddInParameter(command, helper.FacPer18, DbType.Double, entity.FacPer18);
            dbProvider.AddInParameter(command, helper.FacPer19, DbType.Double, entity.FacPer19);
            dbProvider.AddInParameter(command, helper.FacPer20, DbType.Double, entity.FacPer20);
            dbProvider.AddInParameter(command, helper.FacPer21, DbType.Double, entity.FacPer21);
            dbProvider.AddInParameter(command, helper.FacPer22, DbType.Double, entity.FacPer22);
            dbProvider.AddInParameter(command, helper.FacPer23, DbType.Double, entity.FacPer23);
            dbProvider.AddInParameter(command, helper.FacPer24, DbType.Double, entity.FacPer24);
            dbProvider.AddInParameter(command, helper.FacPer25, DbType.Double, entity.FacPer25);
            dbProvider.AddInParameter(command, helper.FacPer26, DbType.Double, entity.FacPer26);
            dbProvider.AddInParameter(command, helper.FacPer27, DbType.Double, entity.FacPer27);
            dbProvider.AddInParameter(command, helper.FacPer28, DbType.Double, entity.FacPer28);
            dbProvider.AddInParameter(command, helper.FacPer29, DbType.Double, entity.FacPer29);
            dbProvider.AddInParameter(command, helper.FacPer30, DbType.Double, entity.FacPer30);
            dbProvider.AddInParameter(command, helper.FacPer31, DbType.Double, entity.FacPer31);
            dbProvider.AddInParameter(command, helper.FacPer32, DbType.Double, entity.FacPer32);
            dbProvider.AddInParameter(command, helper.FacPer33, DbType.Double, entity.FacPer33);
            dbProvider.AddInParameter(command, helper.FacPer34, DbType.Double, entity.FacPer34);
            dbProvider.AddInParameter(command, helper.FacPer35, DbType.Double, entity.FacPer35);
            dbProvider.AddInParameter(command, helper.FacPer36, DbType.Double, entity.FacPer36);
            dbProvider.AddInParameter(command, helper.FacPer37, DbType.Double, entity.FacPer37);
            dbProvider.AddInParameter(command, helper.FacPer38, DbType.Double, entity.FacPer38);
            dbProvider.AddInParameter(command, helper.FacPer39, DbType.Double, entity.FacPer39);
            dbProvider.AddInParameter(command, helper.FacPer40, DbType.Double, entity.FacPer40);
            dbProvider.AddInParameter(command, helper.FacPer41, DbType.Double, entity.FacPer41);
            dbProvider.AddInParameter(command, helper.FacPer42, DbType.Double, entity.FacPer42);
            dbProvider.AddInParameter(command, helper.FacPer43, DbType.Double, entity.FacPer43);
            dbProvider.AddInParameter(command, helper.FacPer44, DbType.Double, entity.FacPer44);
            dbProvider.AddInParameter(command, helper.FacPer45, DbType.Double, entity.FacPer45);
            dbProvider.AddInParameter(command, helper.FacPer46, DbType.Double, entity.FacPer46);
            dbProvider.AddInParameter(command, helper.FacPer47, DbType.Double, entity.FacPer47);
            dbProvider.AddInParameter(command, helper.FacPer48, DbType.Double, entity.FacPer48);
            dbProvider.AddInParameter(command, helper.FacPerUserName, DbType.String, entity.FacPerUserName);
            dbProvider.AddInParameter(command, helper.FacPerFecIns, DbType.DateTime, DateTime.Now);

            dbProvider.ExecuteNonQuery(command);
            return IdFactorPerdida;
        }

        public void Delete(System.Int32 PeriCod, System.Int32 FacPerVersion)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, PeriCod);
            dbProvider.AddInParameter(command, helper.FacPerVersion, DbType.Int32, FacPerVersion);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<FactorPerdidaDTO> ListByPeriodoVersion(int IPeriCodi, int IFacPerVersion)
        {
            List<FactorPerdidaDTO> entitys = new List<FactorPerdidaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListByPeriodoVersion);

            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, IPeriCodi);
            dbProvider.AddInParameter(command, helper.FacPerVersion, DbType.Int32, IFacPerVersion);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));

                }
            }
            return entitys;
        }

        public int GetCodigoGenerado()
        {
            int newId = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlCodigoGenerado);
            newId = Int32.Parse(dbProvider.ExecuteScalar(command).ToString());

            return newId;
        }

        public int GetCodigoGeneradoDec()
        {
            int newId = -1;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlCodigoGeneradoDec);
            newId = Int32.Parse(dbProvider.ExecuteScalar(command).ToString());

            return newId;
        }

        public void BulkInsert(List<TrnFactorPerdidaBullkDTO> entitys)
        {
            dbProvider.AddColumnMapping(helper.FacPerCodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.PeriCodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.BarrCodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.FacPerVersion, DbType.Int32);
            dbProvider.AddColumnMapping(helper.FacPerDia, DbType.Int32);
            dbProvider.AddColumnMapping(helper.FacPerBase, DbType.String);
            dbProvider.AddColumnMapping(helper.FacPerBarrNombre, DbType.String);
            dbProvider.AddColumnMapping(helper.FacPer1, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.FacPer2, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.FacPer3, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.FacPer4, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.FacPer5, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.FacPer6, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.FacPer7, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.FacPer8, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.FacPer9, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.FacPer10, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.FacPer11, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.FacPer12, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.FacPer13, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.FacPer14, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.FacPer15, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.FacPer16, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.FacPer17, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.FacPer18, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.FacPer19, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.FacPer20, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.FacPer21, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.FacPer22, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.FacPer23, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.FacPer24, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.FacPer25, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.FacPer26, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.FacPer27, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.FacPer28, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.FacPer29, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.FacPer30, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.FacPer31, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.FacPer32, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.FacPer33, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.FacPer34, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.FacPer35, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.FacPer36, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.FacPer37, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.FacPer38, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.FacPer39, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.FacPer40, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.FacPer41, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.FacPer42, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.FacPer43, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.FacPer44, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.FacPer45, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.FacPer46, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.FacPer47, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.FacPer48, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.FacPerUserName, DbType.String);
            dbProvider.AddColumnMapping(helper.FacPerFecIns, DbType.DateTime);

            dbProvider.BulkInsert<TrnFactorPerdidaBullkDTO>(entitys, helper.TableName);
        }

        public void CopiarFactorPerdidaCostoMarginal(int iPeriCodi, int iVersionOld, int iVersionNew, int iFacPerCodi, int iCostMargCodi, IDbConnection conn, DbTransaction tran)
        {
            //==========================================================================
            //-- FACTOR DE PERDIDA
            //==========================================================================

            DbCommand commandFP = (DbCommand)conn.CreateCommand();
            commandFP.Transaction = tran;
            commandFP.Connection = (DbConnection)conn;
            commandFP.CommandText = helper.SqlCopiarFactorPerdida;
            IDbDataParameter paramFP = commandFP.CreateParameter();
            paramFP.ParameterName = helper.FacPerCodi;
            paramFP.Value = iFacPerCodi;
            commandFP.Parameters.Add(paramFP);

            paramFP = commandFP.CreateParameter();
            paramFP.ParameterName = helper.PeriCodi;
            paramFP.Value = iPeriCodi;
            commandFP.Parameters.Add(paramFP);

            paramFP = commandFP.CreateParameter();
            paramFP.ParameterName = helper.FacPerVersion;
            paramFP.Value = iVersionNew;
            commandFP.Parameters.Add(paramFP);

            paramFP = commandFP.CreateParameter();
            paramFP.ParameterName = helper.PeriCodi;
            paramFP.Value = iPeriCodi;
            commandFP.Parameters.Add(paramFP);

            paramFP = commandFP.CreateParameter();
            paramFP.ParameterName = helper.BarrCodi;
            paramFP.Value = iVersionOld;
            commandFP.Parameters.Add(paramFP);

            commandFP.ExecuteNonQuery();

            //==========================================================================
            //-- COSTO MARGINAL - CONGESTION/ENERGIA
            //==========================================================================
            DbCommand commandCG = (DbCommand)conn.CreateCommand();
            commandCG.Transaction = tran;
            commandCG.Connection = (DbConnection)conn;
            commandCG.CommandText = helper.SqlCopiarFactorPerdidaCongestion;
            IDbDataParameter paramCG = commandCG.CreateParameter();
            paramCG.ParameterName = helper.CongeneCodi;
            paramCG.Value = iFacPerCodi;
            commandCG.Parameters.Add(paramCG);

            paramCG = commandCG.CreateParameter();
            paramCG.ParameterName = helper.PeriCodi;
            paramCG.Value = iPeriCodi;
            commandCG.Parameters.Add(paramCG);

            paramCG = commandCG.CreateParameter();
            paramCG.ParameterName = helper.CongeneVersion;
            paramCG.Value = iVersionNew;
            commandCG.Parameters.Add(paramCG);

            paramCG = commandCG.CreateParameter();
            paramCG.ParameterName = helper.PeriCodi;
            paramCG.Value = iPeriCodi;
            commandCG.Parameters.Add(paramCG);

            paramCG = commandCG.CreateParameter();
            paramCG.ParameterName = helper.BarrCodi;
            paramCG.Value = iVersionOld;
            commandCG.Parameters.Add(paramCG);

            commandCG.ExecuteNonQuery();


            //--------------------------------------------------------------------------------------------------
            DbCommand commandTMP = (DbCommand)conn.CreateCommand();
            commandTMP.Transaction = tran;
            commandTMP.Connection = (DbConnection)conn;

            //Limpiamos tabla temporal
            commandTMP.CommandText = "delete from trn_tmp_equiv";
            commandTMP.ExecuteNonQuery();

            //insertamos en la tabla temporal
            commandTMP.CommandText = helper.SqlCopiarTemporal;
            IDbDataParameter paramTMP = commandTMP.CreateParameter();
            paramTMP = commandTMP.CreateParameter();
            paramTMP.ParameterName = helper.FacPerCodi;
            paramTMP.Value = iFacPerCodi;
            commandTMP.Parameters.Add(paramTMP);

            paramTMP = commandTMP.CreateParameter();
            paramTMP.ParameterName = helper.PeriCodi;
            paramTMP.Value = iPeriCodi;
            commandTMP.Parameters.Add(paramTMP);

            paramTMP = commandTMP.CreateParameter();
            paramTMP.ParameterName = helper.BarrCodi;
            paramTMP.Value = iVersionOld;
            commandTMP.Parameters.Add(paramTMP);

            commandTMP.ExecuteNonQuery();
            //-----------------------------------------------------------------------------------------------------
            //COSTO MARGINAL --- trn_costo_marginal 
            DbCommand commandCM = (DbCommand)conn.CreateCommand();
            commandCM.Transaction = tran;
            commandCM.Connection = (DbConnection)conn;
            commandCM.CommandText = helper.SqlCopiarCostoMarginal;
            IDbDataParameter paramCM = commandCM.CreateParameter();
            paramCM = commandCM.CreateParameter();
            paramCM.ParameterName = helper.CosMarCodi;
            paramCM.Value = iCostMargCodi;
            commandCM.Parameters.Add(paramCM);

            paramCM = commandCM.CreateParameter();
            paramCM.ParameterName = helper.PeriCodi;
            paramCM.Value = iPeriCodi;
            commandCM.Parameters.Add(paramCM);

            paramCM = commandCM.CreateParameter();
            paramCM.ParameterName = helper.FacPerVersion;
            paramCM.Value = iVersionNew;
            commandCM.Parameters.Add(paramCM);

            commandCM.ExecuteNonQuery();
        }

        public void CopiarSGOCOES(int iPeriCodi, int iVersion, int iFacPerCodi, int iCostMargCodi, string suser, string sAnioMes, IDbConnection conn, DbTransaction tran)
        {
            //FACTOR DE PERDIDA
            //Si_CostoMarginal  --- AQUI
            DbCommand commandFP = (DbCommand)conn.CreateCommand();
            commandFP.Transaction = tran;
            commandFP.Connection = (DbConnection)conn;
            commandFP.CommandText = helper.SqlCopiarSGOCOES;
            IDbDataParameter paramFP = commandFP.CreateParameter();
            paramFP.ParameterName = helper.FacPerCodi;
            paramFP.Value = iFacPerCodi;
            commandFP.Parameters.Add(paramFP);

            paramFP = commandFP.CreateParameter();
            paramFP.ParameterName = helper.PeriCodi;
            paramFP.Value = iPeriCodi;
            commandFP.Parameters.Add(paramFP);

            paramFP = commandFP.CreateParameter();
            paramFP.ParameterName = helper.FacPerVersion;
            paramFP.Value = iVersion;
            commandFP.Parameters.Add(paramFP);

            paramFP = commandFP.CreateParameter();
            paramFP.ParameterName = helper.FacPerUserName;
            paramFP.Value = suser;
            commandFP.Parameters.Add(paramFP);

            for (int i = 0; i < 48; i++)
            {
                paramFP = commandFP.CreateParameter();
                paramFP.ParameterName = helper.FacPerBase;
                paramFP.Value = sAnioMes;
                commandFP.Parameters.Add(paramFP);
            }
            commandFP.ExecuteNonQuery();
            //--------------------------------------------------------------------------------------------------
            //COSTO MARGINAL - CONGESTION
            //--------------------------------------------------------------------------------------------------

            DbCommand commandCG = (DbCommand)conn.CreateCommand();
            commandCG.Transaction = tran;
            commandCG.Connection = (DbConnection)conn;
            commandCG.CommandText = helper.SqlCopiarSGOCOESCongestion;
            IDbDataParameter paramCG = commandCG.CreateParameter();
            paramCG.ParameterName = helper.CongeneCodi;
            paramCG.Value = iFacPerCodi;
            commandCG.Parameters.Add(paramCG);

            paramCG = commandCG.CreateParameter();
            paramCG.ParameterName = helper.PeriCodi;
            paramCG.Value = iPeriCodi;
            commandCG.Parameters.Add(paramCG);

            paramCG = commandCG.CreateParameter();
            paramCG.ParameterName = helper.CongeneVersion;
            paramCG.Value = iVersion;
            commandCG.Parameters.Add(paramCG);

            paramCG = commandCG.CreateParameter();
            paramCG.ParameterName = helper.CongeneUserName;
            paramCG.Value = suser;
            commandCG.Parameters.Add(paramCG);

            for (int i = 0; i < 48; i++)
            {
                paramCG = commandCG.CreateParameter();
                paramCG.ParameterName = helper.CongeneBase;
                paramCG.Value = sAnioMes;
                commandCG.Parameters.Add(paramCG);
            }
            commandCG.ExecuteNonQuery();


            //--------------------------------------------------------------------------------------------------
            //COSTO MARGINAL - ENERGIA
            //--------------------------------------------------------------------------------------------------


            DbCommand commandCG2 = (DbCommand)conn.CreateCommand();
            commandCG2.Transaction = tran;
            commandCG2.Connection = (DbConnection)conn;
            commandCG2.CommandText = helper.SqlCopiarSGOCOESEnergia;
            IDbDataParameter paramCG2 = commandCG2.CreateParameter();
            paramCG2.ParameterName = helper.CongeneCodi;
            paramCG2.Value = iFacPerCodi;
            commandCG2.Parameters.Add(paramCG2);

            paramCG2 = commandCG2.CreateParameter();
            paramCG2.ParameterName = helper.PeriCodi;
            paramCG2.Value = iPeriCodi;
            commandCG2.Parameters.Add(paramCG2);

            paramCG2 = commandCG2.CreateParameter();
            paramCG2.ParameterName = helper.CongeneVersion;
            paramCG2.Value = iVersion;
            commandCG2.Parameters.Add(paramCG2);

            paramCG2 = commandCG2.CreateParameter();
            paramCG2.ParameterName = helper.CongeneUserName;
            paramCG2.Value = suser;
            commandCG2.Parameters.Add(paramCG2);

            for (int i = 0; i < 48; i++)
            {
                paramCG2 = commandCG2.CreateParameter();
                paramCG2.ParameterName = helper.CongeneBase;
                paramCG2.Value = sAnioMes;
                commandCG2.Parameters.Add(paramCG2);
            }
            commandCG2.ExecuteNonQuery();

            //--------------------------------------------------------------------------------------------------




            //COSTO MARGINAL

            // trn_costo_marginal
            DbCommand commandCM = (DbCommand)conn.CreateCommand();
            commandCM.Transaction = tran;
            commandCM.Connection = (DbConnection)conn;
            commandCM.CommandText = helper.SqlCopiarSGOCOESCM;
            IDbDataParameter paramCM = commandCM.CreateParameter();
            paramCM = commandCM.CreateParameter();
            paramCM.ParameterName = helper.CosMarCodi;
            paramCM.Value = iCostMargCodi;
            commandCM.Parameters.Add(paramCM);

            paramCM = commandCM.CreateParameter();
            paramCM.ParameterName = helper.PeriCodi;
            paramCM.Value = iPeriCodi;
            commandCM.Parameters.Add(paramCM);

            paramCM = commandCM.CreateParameter();
            paramCM.ParameterName = helper.FacPerVersion;
            paramCM.Value = iVersion;
            commandCM.Parameters.Add(paramCM);

            commandCM.ExecuteNonQuery();
        }

        //ASSETEC 202002
        public void DeleteCMTMP()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteCMTMP);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<FactorPerdidaDTO> ListBarrasSiCostMarg(string sAnioMes)
        {
            List<FactorPerdidaDTO> entitys = new List<FactorPerdidaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListBarrasSiCostMarg);

            dbProvider.AddInParameter(command, helper.FacPerBase, DbType.String, sAnioMes);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FactorPerdidaDTO entity = new FactorPerdidaDTO();
                    int iBarrCodi = dr.GetOrdinal(this.helper.BarrCodi);
                    if (!dr.IsDBNull(iBarrCodi)) entity.BarrCodi = dr.GetInt32(iBarrCodi);

                    int iFacPerBarrNombre = dr.GetOrdinal(this.helper.FacPerBarrNombre);
                    if (!dr.IsDBNull(iFacPerBarrNombre)) entity.FacPerBarrNombre = dr.GetString(iFacPerBarrNombre);
                    entitys.Add(entity);

                }
            }
            return entitys;
        }

        public void SaveCostMargTmp(int barrcodi, int iDiasMes, string sAnioMes)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSaveCostMargTmp);

            for (int i = 0; i < 48; i++)
            {
                dbProvider.AddInParameter(command, helper.BarrCodi, DbType.Int32, barrcodi);
                dbProvider.AddInParameter(command, helper.FacPerBase, DbType.String, sAnioMes);
                dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, iDiasMes);
            }


            dbProvider.ExecuteNonQuery(command);
        }

        public List<string> ListFechaXBarraSiCostMarg(string sAnioMes, int barrcodi)
        {
            List<string> entitys = new List<string>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListFechaXBarraSiCostMarg);

            dbProvider.AddInParameter(command, helper.BarrCodi, DbType.Int32, barrcodi);
            dbProvider.AddInParameter(command, helper.BarrCodi, DbType.Int32, barrcodi);
            dbProvider.AddInParameter(command, helper.FacPerBase, DbType.String, sAnioMes);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DateTime dDiaIntervalo = DateTime.Now;
                    int iFacPerFecIns = dr.GetOrdinal(this.helper.FacPerFecIns);
                    if (!dr.IsDBNull(iFacPerFecIns)) dDiaIntervalo = dr.GetDateTime(iFacPerFecIns);
                    string entity = dDiaIntervalo.Day.ToString() + "[" + dDiaIntervalo.Minute + "]";
                    entitys.Add(entity);
                }
            }
            return entitys;
        }
    }
}
