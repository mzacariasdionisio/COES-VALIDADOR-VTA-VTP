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
    public class ValorTransferenciaRepository : RepositoryBase, IValorTransferenciaRepository
    {
        public ValorTransferenciaRepository(string strConn)
            : base(strConn)
        {
        }

        ValorTransferenciaHelper helper = new ValorTransferenciaHelper();

        public int Save(ValorTransferenciaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.ValoTranCodi, DbType.Int32, GetCodigoGenerado());
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, entity.PeriCodi);
            dbProvider.AddInParameter(command, helper.BarrCodi, DbType.Int32, entity.BarrCodi);
            dbProvider.AddInParameter(command, helper.EmpCodi, DbType.Int32, entity.EmpCodi);
            dbProvider.AddInParameter(command, helper.CostMargCodi, DbType.Int32, entity.CostMargCodi);
            dbProvider.AddInParameter(command, helper.ValoTranFlag, DbType.String, entity.ValoTranFlag);
            dbProvider.AddInParameter(command, helper.ValoTranCodEntRet, DbType.String, entity.ValoTranCodEntRet);
            dbProvider.AddInParameter(command, helper.ValoTranVersion, DbType.Int32, entity.ValoTranVersion);
            dbProvider.AddInParameter(command, helper.ValoTranDia, DbType.Int32, entity.ValoTranDia);
            dbProvider.AddInParameter(command, helper.VTTotalDia, DbType.Double, entity.VTTotalDia);
            dbProvider.AddInParameter(command, helper.VTTotalEnergia, DbType.Double, entity.VTTotalEnergia);
            dbProvider.AddInParameter(command, helper.VTTipoinformacion, DbType.String, entity.VTTipoInformacion);
            dbProvider.AddInParameter(command, helper.VT1, DbType.Double, entity.VT1);
            dbProvider.AddInParameter(command, helper.VT2, DbType.Double, entity.VT2);
            dbProvider.AddInParameter(command, helper.VT3, DbType.Double, entity.VT3);
            dbProvider.AddInParameter(command, helper.VT4, DbType.Double, entity.VT4);
            dbProvider.AddInParameter(command, helper.VT5, DbType.Double, entity.VT5);
            dbProvider.AddInParameter(command, helper.VT6, DbType.Double, entity.VT6);
            dbProvider.AddInParameter(command, helper.VT7, DbType.Double, entity.VT7);
            dbProvider.AddInParameter(command, helper.VT8, DbType.Double, entity.VT8);
            dbProvider.AddInParameter(command, helper.VT9, DbType.Double, entity.VT9);
            dbProvider.AddInParameter(command, helper.VT10, DbType.Double, entity.VT10);
            dbProvider.AddInParameter(command, helper.VT11, DbType.Double, entity.VT11);
            dbProvider.AddInParameter(command, helper.VT12, DbType.Double, entity.VT12);
            dbProvider.AddInParameter(command, helper.VT13, DbType.Double, entity.VT13);
            dbProvider.AddInParameter(command, helper.VT14, DbType.Double, entity.VT14);
            dbProvider.AddInParameter(command, helper.VT15, DbType.Double, entity.VT15);
            dbProvider.AddInParameter(command, helper.VT16, DbType.Double, entity.VT16);
            dbProvider.AddInParameter(command, helper.VT17, DbType.Double, entity.VT17);
            dbProvider.AddInParameter(command, helper.VT18, DbType.Double, entity.VT18);
            dbProvider.AddInParameter(command, helper.VT19, DbType.Double, entity.VT19);
            dbProvider.AddInParameter(command, helper.VT20, DbType.Double, entity.VT20);
            dbProvider.AddInParameter(command, helper.VT21, DbType.Double, entity.VT21);
            dbProvider.AddInParameter(command, helper.VT22, DbType.Double, entity.VT22);
            dbProvider.AddInParameter(command, helper.VT23, DbType.Double, entity.VT23);
            dbProvider.AddInParameter(command, helper.VT24, DbType.Double, entity.VT24);
            dbProvider.AddInParameter(command, helper.VT25, DbType.Double, entity.VT25);
            dbProvider.AddInParameter(command, helper.VT26, DbType.Double, entity.VT26);
            dbProvider.AddInParameter(command, helper.VT27, DbType.Double, entity.VT27);
            dbProvider.AddInParameter(command, helper.VT28, DbType.Double, entity.VT28);
            dbProvider.AddInParameter(command, helper.VT29, DbType.Double, entity.VT29);
            dbProvider.AddInParameter(command, helper.VT30, DbType.Double, entity.VT30);
            dbProvider.AddInParameter(command, helper.VT31, DbType.Double, entity.VT31);
            dbProvider.AddInParameter(command, helper.VT32, DbType.Double, entity.VT32);
            dbProvider.AddInParameter(command, helper.VT33, DbType.Double, entity.VT33);
            dbProvider.AddInParameter(command, helper.VT34, DbType.Double, entity.VT34);
            dbProvider.AddInParameter(command, helper.VT35, DbType.Double, entity.VT35);
            dbProvider.AddInParameter(command, helper.VT36, DbType.Double, entity.VT36);
            dbProvider.AddInParameter(command, helper.VT37, DbType.Double, entity.VT37);
            dbProvider.AddInParameter(command, helper.VT38, DbType.Double, entity.VT38);
            dbProvider.AddInParameter(command, helper.VT39, DbType.Double, entity.VT39);
            dbProvider.AddInParameter(command, helper.VT40, DbType.Double, entity.VT40);
            dbProvider.AddInParameter(command, helper.VT41, DbType.Double, entity.VT41);
            dbProvider.AddInParameter(command, helper.VT42, DbType.Double, entity.VT42);
            dbProvider.AddInParameter(command, helper.VT43, DbType.Double, entity.VT43);
            dbProvider.AddInParameter(command, helper.VT44, DbType.Double, entity.VT44);
            dbProvider.AddInParameter(command, helper.VT45, DbType.Double, entity.VT45);
            dbProvider.AddInParameter(command, helper.VT46, DbType.Double, entity.VT46);
            dbProvider.AddInParameter(command, helper.VT47, DbType.Double, entity.VT47);
            dbProvider.AddInParameter(command, helper.VT48, DbType.Double, entity.VT48);
            dbProvider.AddInParameter(command, helper.VT49, DbType.Double, entity.VT49);
            dbProvider.AddInParameter(command, helper.VT50, DbType.Double, entity.VT50);
            dbProvider.AddInParameter(command, helper.VT51, DbType.Double, entity.VT51);
            dbProvider.AddInParameter(command, helper.VT52, DbType.Double, entity.VT52);
            dbProvider.AddInParameter(command, helper.VT53, DbType.Double, entity.VT53);
            dbProvider.AddInParameter(command, helper.VT54, DbType.Double, entity.VT54);
            dbProvider.AddInParameter(command, helper.VT55, DbType.Double, entity.VT55);
            dbProvider.AddInParameter(command, helper.VT56, DbType.Double, entity.VT56);
            dbProvider.AddInParameter(command, helper.VT57, DbType.Double, entity.VT57);
            dbProvider.AddInParameter(command, helper.VT58, DbType.Double, entity.VT58);
            dbProvider.AddInParameter(command, helper.VT59, DbType.Double, entity.VT59);
            dbProvider.AddInParameter(command, helper.VT60, DbType.Double, entity.VT60);
            dbProvider.AddInParameter(command, helper.VT61, DbType.Double, entity.VT61);
            dbProvider.AddInParameter(command, helper.VT62, DbType.Double, entity.VT62);
            dbProvider.AddInParameter(command, helper.VT63, DbType.Double, entity.VT63);
            dbProvider.AddInParameter(command, helper.VT64, DbType.Double, entity.VT64);
            dbProvider.AddInParameter(command, helper.VT65, DbType.Double, entity.VT65);
            dbProvider.AddInParameter(command, helper.VT66, DbType.Double, entity.VT66);
            dbProvider.AddInParameter(command, helper.VT67, DbType.Double, entity.VT67);
            dbProvider.AddInParameter(command, helper.VT68, DbType.Double, entity.VT68);
            dbProvider.AddInParameter(command, helper.VT69, DbType.Double, entity.VT69);
            dbProvider.AddInParameter(command, helper.VT70, DbType.Double, entity.VT70);
            dbProvider.AddInParameter(command, helper.VT71, DbType.Double, entity.VT71);
            dbProvider.AddInParameter(command, helper.VT72, DbType.Double, entity.VT72);
            dbProvider.AddInParameter(command, helper.VT73, DbType.Double, entity.VT73);
            dbProvider.AddInParameter(command, helper.VT74, DbType.Double, entity.VT74);
            dbProvider.AddInParameter(command, helper.VT75, DbType.Double, entity.VT75);
            dbProvider.AddInParameter(command, helper.VT76, DbType.Double, entity.VT76);
            dbProvider.AddInParameter(command, helper.VT77, DbType.Double, entity.VT77);
            dbProvider.AddInParameter(command, helper.VT78, DbType.Double, entity.VT78);
            dbProvider.AddInParameter(command, helper.VT79, DbType.Double, entity.VT79);
            dbProvider.AddInParameter(command, helper.VT80, DbType.Double, entity.VT80);
            dbProvider.AddInParameter(command, helper.VT81, DbType.Double, entity.VT81);
            dbProvider.AddInParameter(command, helper.VT82, DbType.Double, entity.VT82);
            dbProvider.AddInParameter(command, helper.VT83, DbType.Double, entity.VT83);
            dbProvider.AddInParameter(command, helper.VT84, DbType.Double, entity.VT84);
            dbProvider.AddInParameter(command, helper.VT85, DbType.Double, entity.VT85);
            dbProvider.AddInParameter(command, helper.VT86, DbType.Double, entity.VT86);
            dbProvider.AddInParameter(command, helper.VT87, DbType.Double, entity.VT87);
            dbProvider.AddInParameter(command, helper.VT88, DbType.Double, entity.VT88);
            dbProvider.AddInParameter(command, helper.VT89, DbType.Double, entity.VT89);
            dbProvider.AddInParameter(command, helper.VT90, DbType.Double, entity.VT90);
            dbProvider.AddInParameter(command, helper.VT91, DbType.Double, entity.VT91);
            dbProvider.AddInParameter(command, helper.VT92, DbType.Double, entity.VT92);
            dbProvider.AddInParameter(command, helper.VT93, DbType.Double, entity.VT93);
            dbProvider.AddInParameter(command, helper.VT94, DbType.Double, entity.VT94);
            dbProvider.AddInParameter(command, helper.VT95, DbType.Double, entity.VT95);
            dbProvider.AddInParameter(command, helper.VT96, DbType.Double, entity.VT96);
            dbProvider.AddInParameter(command, helper.USERNAME, DbType.String, entity.VtranUserName);
            dbProvider.AddInParameter(command, helper.ValoTranFecIns, DbType.DateTime, DateTime.Now);

            return dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(System.Int32 PeriCod, System.Int32 ValoTranVersion)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, PeriCod);
            dbProvider.AddInParameter(command, helper.ValoTranVersion, DbType.Int32, ValoTranVersion);

            dbProvider.ExecuteNonQuery(command);
        }

        public int GetCodigoGenerado()
        {
            int newId = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlCodigoGenerado);
            newId = Int32.Parse(dbProvider.ExecuteScalar(command).ToString());

            return newId;
        }

        public List<ValorTransferenciaDTO> List(int pericodi, int version)
        {
            List<ValorTransferenciaDTO> entitys = new List<ValorTransferenciaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.ValoTranVersion, DbType.Int32, version);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.ValoTranVersion, DbType.Int32, version);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ValorTransferenciaDTO entity = new ValorTransferenciaDTO();

                    int iEmpCodi = dr.GetOrdinal(this.helper.EmpCodi);
                    if (!dr.IsDBNull(iEmpCodi)) entity.EmpCodi = dr.GetInt32(iEmpCodi);

                    int iPeriCodi = dr.GetOrdinal(this.helper.PeriCodi);
                    if (!dr.IsDBNull(iPeriCodi)) entity.PeriCodi = dr.GetInt32(iPeriCodi);

                    int iValoTranVersion = dr.GetOrdinal(this.helper.ValoTranVersion);
                    if (!dr.IsDBNull(iValoTranVersion)) entity.ValoTranVersion = dr.GetInt32(iValoTranVersion);

                    int iENTREGA = dr.GetOrdinal(this.helper.ENTREGA);
                    if (!dr.IsDBNull(iENTREGA)) entity.VTEmpresaEntrega = dr.GetDecimal(iENTREGA);

                    int iRETIRO = dr.GetOrdinal(this.helper.RETIRO);
                    if (!dr.IsDBNull(iRETIRO)) entity.VTEmpresaRetiro = dr.GetDecimal(iRETIRO);

                    entitys.Add(entity);

                }
            }

            return entitys;
        }

        public List<ValorTransferenciaDTO> GetByCriteria(int? empcodi, int? barrcodi, int? pericodi, int? tipoemprcodi, int? version, string flagEntrReti) //int pericodi, string barrcodi
        {
            List<ValorTransferenciaDTO> entitys = new List<ValorTransferenciaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.ValoTranVersion, DbType.Int32, version);
            dbProvider.AddInParameter(command, helper.EmpCodi, DbType.Int32, empcodi);
            dbProvider.AddInParameter(command, helper.EmpCodi, DbType.Int32, empcodi);
            dbProvider.AddInParameter(command, helper.TIPOEMPRCODI, DbType.Int32, tipoemprcodi);
            dbProvider.AddInParameter(command, helper.TIPOEMPRCODI, DbType.Int32, tipoemprcodi);
            dbProvider.AddInParameter(command, helper.BarrCodi, DbType.Int32, barrcodi);
            dbProvider.AddInParameter(command, helper.BarrCodi, DbType.Int32, barrcodi);
            dbProvider.AddInParameter(command, helper.ValoTranFlag, DbType.String, flagEntrReti);
            dbProvider.AddInParameter(command, helper.ValoTranFlag, DbType.String, flagEntrReti);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.ValoTranVersion, DbType.Int32, version);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ValorTransferenciaDTO entity = new ValorTransferenciaDTO();

                    int iEMPRNOMB = dr.GetOrdinal(this.helper.EMPRNOMB);
                    if (!dr.IsDBNull(iEMPRNOMB)) entity.EmprNomb = dr.GetString(iEMPRNOMB);

                    int iBARRNOMBRE = dr.GetOrdinal(this.helper.BARRNOMBRE);
                    if (!dr.IsDBNull(iBARRNOMBRE)) entity.BarrNombBarrTran = dr.GetString(iBARRNOMBRE);

                    int iValoTranCodEntRet = dr.GetOrdinal(this.helper.ValoTranCodEntRet);
                    if (!dr.IsDBNull(iValoTranCodEntRet)) entity.ValoTranCodEntRet = dr.GetString(iValoTranCodEntRet);

                    int iVALORIZACION = dr.GetOrdinal(this.helper.VALORIZACION);
                    if (!dr.IsDBNull(iVALORIZACION)) entity.VTTotalDia = dr.GetDecimal(iVALORIZACION);

                    int iENERGIA = dr.GetOrdinal(this.helper.ENERGIA);
                    if (!dr.IsDBNull(iENERGIA)) entity.VTTotalEnergia = dr.GetDecimal(iENERGIA);

                    int iVTTipoinformacion = dr.GetOrdinal(this.helper.VTTipoinformacion);
                    if (!dr.IsDBNull(iVTTipoinformacion)) entity.VTTipoInformacion = dr.GetString(iVTTipoinformacion);

                    int iNombEmpresa = dr.GetOrdinal(this.helper.NombEmpresa);
                    if (!dr.IsDBNull(iNombEmpresa)) entity.NombEmpresa = dr.GetString(iNombEmpresa);

                    int iRucEmpresa = dr.GetOrdinal(this.helper.RucEmpresa);
                    if (!dr.IsDBNull(iRucEmpresa)) entity.RucEmpresa = dr.GetString(iRucEmpresa);

                    int iENTREGA = dr.GetOrdinal(this.helper.ENTREGA);
                    if (!dr.IsDBNull(iENTREGA)) entity.CentGeneNombre = dr.GetString(iENTREGA);

                    int iRETIRO = dr.GetOrdinal(this.helper.RETIRO);
                    if (!dr.IsDBNull(iRETIRO)) entity.VtranUserName = dr.GetString(iRETIRO);

                    int iValoTranFlag = dr.GetOrdinal(this.helper.ValoTranFlag);
                    if (!dr.IsDBNull(iValoTranFlag)) entity.ValoTranFlag = dr.GetString(iValoTranFlag);

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);

                    int iBarrCodi = dr.GetOrdinal(this.helper.BarrCodi);
                    if (!dr.IsDBNull(iBarrCodi)) entity.BarrCodi = dr.GetInt32(iBarrCodi);

                    int iOsinergcodi = dr.GetOrdinal(this.helper.Osinergcodi);
                    if (!dr.IsDBNull(iOsinergcodi)) entity.Osinergcodi = dr.GetString(iOsinergcodi);

                    int iRCLICITACION = dr.GetOrdinal(this.helper.RCLICITACION);
                    if (!dr.IsDBNull(iRCLICITACION)) entity.RcLicitacion = dr.GetDecimal(iRCLICITACION);

                    int iRCBILATERAL = dr.GetOrdinal(this.helper.RCBILATERAL);
                    if (!dr.IsDBNull(iRCBILATERAL)) entity.RcBilateral = dr.GetDecimal(iRCBILATERAL);

                    int iEmprcodosinergmin = dr.GetOrdinal(helper.Emprcodosinergmin);
                    if (!dr.IsDBNull(iEmprcodosinergmin)) entity.Emprcodosinergmin = dr.GetString(iEmprcodosinergmin);

                    entitys.Add(entity);

                }
            }

            return entitys;
        }

        public List<ValorTransferenciaDTO> GetTotalByTipoFlag(int pericodi, int version)
        {
            List<ValorTransferenciaDTO> entitys = new List<ValorTransferenciaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetTotalByTipoFlag);

            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.ValoTranVersion, DbType.Int32, version);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ValorTransferenciaDTO entity = new ValorTransferenciaDTO();

                    int iPeriCodi = dr.GetOrdinal(this.helper.PeriCodi);
                    if (!dr.IsDBNull(iPeriCodi)) entity.PeriCodi = dr.GetInt32(iPeriCodi);

                    int iValoTranVersion = dr.GetOrdinal(this.helper.ValoTranVersion);
                    if (!dr.IsDBNull(iValoTranVersion)) entity.ValoTranVersion = dr.GetInt32(iValoTranVersion);

                    int iValoTranFlag = dr.GetOrdinal(this.helper.ValoTranFlag);
                    if (!dr.IsDBNull(iValoTranFlag)) entity.ValoTranFlag = dr.GetString(iValoTranFlag);

                    int iVTTotalDia = dr.GetOrdinal(this.helper.VTTotalDia);
                    if (!dr.IsDBNull(iVTTotalDia)) entity.VTTotalDia = dr.GetDecimal(iVTTotalDia);

                    entitys.Add(entity);

                }
            }

            return entitys;
        }

        public List<ValorTransferenciaDTO> GetValorEmpresa(int pericodi, int version)
        {
            List<ValorTransferenciaDTO> entitys = new List<ValorTransferenciaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetValorEmpresaByPeriVer);

            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.ValoTranVersion, DbType.Int32, version);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ValorTransferenciaDTO entity = new ValorTransferenciaDTO();

                    int iEmpCodi = dr.GetOrdinal(this.helper.EmpCodi);
                    if (!dr.IsDBNull(iEmpCodi)) entity.EmpCodi = dr.GetInt32(iEmpCodi);
                    entity.Emprcodi = entity.EmpCodi ?? 0;

                    int iEMPRNOMB = dr.GetOrdinal(this.helper.EMPRNOMB);
                    if (!dr.IsDBNull(iEMPRNOMB)) entity.EmprNomb = dr.GetString(iEMPRNOMB);

                    int iSALEMPSALDO = dr.GetOrdinal(this.helper.SALEMPSALDO);
                    if (!dr.IsDBNull(iSALEMPSALDO)) entity.SalEmpSaldo = dr.GetDecimal(iSALEMPSALDO);

                    int iCOMPENSACION = dr.GetOrdinal(this.helper.COMPENSACION);
                    if (!dr.IsDBNull(iCOMPENSACION)) entity.Compensacion = dr.GetDecimal(iCOMPENSACION);

                    int iVALORIZACION = dr.GetOrdinal(this.helper.VALORIZACION);
                    if (!dr.IsDBNull(iVALORIZACION)) entity.Valorizacion = dr.GetDecimal(iVALORIZACION);

                    int iSALRSCSALDO = dr.GetOrdinal(this.helper.SALRSCSALDO);
                    if (!dr.IsDBNull(iSALRSCSALDO)) entity.SalrscSaldo = dr.GetDecimal(iSALRSCSALDO);

                    int iSALDORECALCULO = dr.GetOrdinal(this.helper.SALDORECALCULO);
                    if (!dr.IsDBNull(iSALDORECALCULO)) entity.Salrecalculo = dr.GetDecimal(iSALDORECALCULO);

                    int iVTOTEMPRESA = dr.GetOrdinal(this.helper.VTOTEMPRESA);
                    if (!dr.IsDBNull(iVTOTEMPRESA)) entity.Vtotempresa = dr.GetDecimal(iVTOTEMPRESA);

                    int iVTOTANTERIOR = dr.GetOrdinal(this.helper.VTOTANTERIOR);
                    if (!dr.IsDBNull(iVTOTANTERIOR)) entity.Vtotanterior = dr.GetDecimal(iVTOTANTERIOR);

                    int iPeriCodi = dr.GetOrdinal(this.helper.PeriCodi);
                    if (!dr.IsDBNull(iPeriCodi)) entity.PeriCodi = dr.GetInt32(iPeriCodi);

                    int iValoTranVersion = dr.GetOrdinal(this.helper.ValoTranVersion);
                    if (!dr.IsDBNull(iValoTranVersion)) entity.ValoTranVersion = dr.GetInt32(iValoTranVersion);

                    int iRentaCongestion = dr.GetOrdinal(this.helper.ENTREGA);
                    if (!dr.IsDBNull(iRentaCongestion)) entity.Entregas = dr.GetDecimal(iRentaCongestion);

                    int iEmprcodosinergmin = dr.GetOrdinal(helper.Emprcodosinergmin);
                    if (!dr.IsDBNull(iEmprcodosinergmin)) entity.Emprcodosinergmin = dr.GetString(iEmprcodosinergmin);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public decimal GetSaldoEmpresa(int pericodi, int version, int empr)
        {
            decimal saldoEmpSaldo = 0;
            List<ValorTransferenciaDTO> entitys = new List<ValorTransferenciaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetSaldoEmpresaByPeriVer);

            dbProvider.AddInParameter(command, helper.EmpCodi, DbType.Int32, empr);
            dbProvider.AddInParameter(command, helper.EmpCodi, DbType.Int32, empr);
            dbProvider.AddInParameter(command, helper.EmpCodi, DbType.Int32, empr);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.ValoTranVersion, DbType.Int32, version);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.ValoTranVersion, DbType.Int32, version);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    ValorTransferenciaDTO entity = new ValorTransferenciaDTO();

                    int iSALEMPSALDO = dr.GetOrdinal(this.helper.SALEMPSALDO);
                    if (!dr.IsDBNull(iSALEMPSALDO)) saldoEmpSaldo = dr.GetDecimal(iSALEMPSALDO);
                }
            }

            return saldoEmpSaldo;
        }

        public List<ValorTransferenciaDTO> GetBalanceEnergia(int pericodi, int version)
        {
            List<ValorTransferenciaDTO> entitys = new List<ValorTransferenciaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetBalanceEnergia);

            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.ValoTranVersion, DbType.Int32, version);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ValorTransferenciaDTO entity = new ValorTransferenciaDTO();

                    int iEmpCodi = dr.GetOrdinal(this.helper.EmpCodi);
                    if (!dr.IsDBNull(iEmpCodi)) entity.EmpCodi = Convert.ToInt32(dr.GetDecimal(iEmpCodi));

                    int iNombEmpresa = dr.GetOrdinal(this.helper.NombEmpresa);
                    if (!dr.IsDBNull(iNombEmpresa)) entity.NombEmpresa = dr.GetString(iNombEmpresa);

                    int iEntregas = dr.GetOrdinal(this.helper.Entregas);
                    if (!dr.IsDBNull(iEntregas)) entity.Entregas = dr.GetDecimal(iEntregas);

                    int iRetiros = dr.GetOrdinal(this.helper.Retiros);
                    if (!dr.IsDBNull(iRetiros)) entity.Retiros = dr.GetDecimal(iRetiros);

                    entitys.Add(entity);

                }
            }

            return entitys;
        }

        public List<ValorTransferenciaDTO> GetDesviacionRetiros(int pericodi, int pericodianterior)
        {
            List<ValorTransferenciaDTO> entitys = new List<ValorTransferenciaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetDesviacionRetiros);

            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.ValoTranVersion, DbType.Int32, pericodianterior);
            dbProvider.AddInParameter(command, helper.ValoTranVersion, DbType.Int32, pericodianterior);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ValorTransferenciaDTO entity = new ValorTransferenciaDTO();

                    int iBarrCodi = dr.GetOrdinal(this.helper.BarrCodi);
                    if (!dr.IsDBNull(iBarrCodi)) entity.BarrCodi = Convert.ToInt32(dr.GetDecimal(iBarrCodi));

                    int iBarrNombBarrTran = dr.GetOrdinal(this.helper.BARRNOMBRE);
                    if (!dr.IsDBNull(iBarrNombBarrTran)) entity.BarrNombBarrTran = dr.GetString(iBarrNombBarrTran);

                    int iEntregas = dr.GetOrdinal(this.helper.Entregas);
                    if (!dr.IsDBNull(iEntregas)) entity.Entregas = dr.GetDecimal(iEntregas);

                    int iRetiros = dr.GetOrdinal(this.helper.Retiros);
                    if (!dr.IsDBNull(iRetiros)) entity.Retiros = dr.GetDecimal(iRetiros);

                    entitys.Add(entity);

                }
            }

            return entitys;
        }

        public List<ValorTransferenciaDTO> GetBalanceValorTransferencia(int pericodi, int version)
        {
            List<ValorTransferenciaDTO> entitys = new List<ValorTransferenciaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetBalanceValorTransferencia);

            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.ValoTranVersion, DbType.Int32, version);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ValorTransferenciaDTO entity = new ValorTransferenciaDTO();

                    int iEmpCodi = dr.GetOrdinal(this.helper.EmpCodi);
                    if (!dr.IsDBNull(iEmpCodi)) entity.EmpCodi = Convert.ToInt32(dr.GetDecimal(iEmpCodi));

                    int iNombEmpresa = dr.GetOrdinal(this.helper.NombEmpresa);
                    if (!dr.IsDBNull(iNombEmpresa)) entity.NombEmpresa = dr.GetString(iNombEmpresa);

                    int iEntregas = dr.GetOrdinal(this.helper.Entregas);
                    if (!dr.IsDBNull(iEntregas)) entity.Entregas = dr.GetDecimal(iEntregas);

                    int iRetiros = dr.GetOrdinal(this.helper.Retiros);
                    if (!dr.IsDBNull(iRetiros)) entity.Retiros = dr.GetDecimal(iRetiros);

                    entitys.Add(entity);

                }
            }

            return entitys;
        }

        public List<ValorTransferenciaDTO> GetValorTransferencia(int iPeriCodi, int iVTranVersion, int iEmpcodi, int iBarrcodi, string sTranFlag)
        {
            List<ValorTransferenciaDTO> entitys = new List<ValorTransferenciaDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetValorTransferencia);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, iPeriCodi);
            dbProvider.AddInParameter(command, helper.ValoTranVersion, DbType.Int32, iVTranVersion);
            dbProvider.AddInParameter(command, helper.EmpCodi, DbType.Int32, iEmpcodi);
            dbProvider.AddInParameter(command, helper.EmpCodi, DbType.Int32, iEmpcodi);
            dbProvider.AddInParameter(command, helper.BarrCodi, DbType.Int32, iBarrcodi);
            dbProvider.AddInParameter(command, helper.BarrCodi, DbType.Int32, iBarrcodi);
            dbProvider.AddInParameter(command, helper.ValoTranFlag, DbType.String, sTranFlag);
            dbProvider.AddInParameter(command, helper.ValoTranFlag, DbType.String, sTranFlag);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iEMPRNOMB = dr.GetOrdinal(helper.EMPRNOMB);
                    if (!dr.IsDBNull(iEMPRNOMB)) entity.EmprNomb = dr.GetString(iEMPRNOMB);

                    int iBARRNOMBRE = dr.GetOrdinal(helper.BARRNOMBRE);
                    if (!dr.IsDBNull(iBARRNOMBRE)) entity.BarrNombBarrTran = dr.GetString(iBARRNOMBRE);

                    int iNombEmpresa = dr.GetOrdinal(helper.NombEmpresa);
                    if (!dr.IsDBNull(iNombEmpresa)) entity.NombEmpresa = dr.GetString(iNombEmpresa);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void BulkInsert(List<TrnValorTransBullk> entitys)
        {
            dbProvider.AddColumnMapping(helper.ValoTranCodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.PeriCodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.BarrCodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.EmpCodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.CostMargCodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.ValoTranFlag, DbType.String);
            dbProvider.AddColumnMapping(helper.ValoTranCodEntRet, DbType.String);
            dbProvider.AddColumnMapping(helper.ValoTranVersion, DbType.Int32);
            dbProvider.AddColumnMapping(helper.ValoTranDia, DbType.Int32);
            dbProvider.AddColumnMapping(helper.VTTotalDia, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VTTotalEnergia, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VTTipoinformacion, DbType.String);
            dbProvider.AddColumnMapping(helper.VT1, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT2, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT3, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT4, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT5, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT6, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT7, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT8, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT9, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT10, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT11, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT12, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT13, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT14, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT15, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT16, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT17, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT18, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT19, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT20, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT21, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT22, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT23, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT24, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT25, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT26, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT27, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT28, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT29, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT30, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT31, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT32, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT33, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT34, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT35, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT36, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT37, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT38, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT39, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT40, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT41, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT42, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT43, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT44, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT45, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT46, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT47, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT48, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT49, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT50, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT51, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT52, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT53, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT54, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT55, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT56, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT57, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT58, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT59, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT60, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT61, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT62, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT63, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT64, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT65, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT66, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT67, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT68, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT69, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT70, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT71, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT72, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT73, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT74, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT75, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT76, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT77, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT78, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT79, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT80, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT81, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT82, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT83, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT84, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT85, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT86, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT87, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT88, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT89, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT90, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT91, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT92, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT93, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT94, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT95, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.VT96, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.USERNAME, DbType.String);
            dbProvider.AddColumnMapping(helper.ValoTranFecIns, DbType.DateTime);

            dbProvider.BulkInsert<TrnValorTransBullk>(entitys, helper.TableName);
        }

        //ASSETEC - 20181001-----------------------------------------------------------------------------------------------
        public void GrabarValorizacionEntrega(int pericodi, int version, string user, int iVtrancodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGrabarValorizacionEntrega);

            dbProvider.AddInParameter(command, helper.ValoTranCodi, DbType.Int32, iVtrancodi);
            dbProvider.AddInParameter(command, helper.USERNAME, DbType.String, user);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.ValoTranVersion, DbType.Int32, version);
            dbProvider.ExecuteNonQuery(command);
        }

        public void GrabarValorizacionRetiro(int pericodi, int version, string user, int iVtrancodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGrabarValorizacionRetiro);

            dbProvider.AddInParameter(command, helper.ValoTranCodi, DbType.Int32, iVtrancodi);
            dbProvider.AddInParameter(command, helper.USERNAME, DbType.String, user);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.ValoTranVersion, DbType.Int32, version);
            dbProvider.ExecuteNonQuery(command);
        }

        #region SIOSEIN2    

        public List<ValorTransferenciaDTO> ListarValorTransferenciaUltVersionXEmpresaYTipoflag(int pericodi, int version)
        {
            List<ValorTransferenciaDTO> entitys = new List<ValorTransferenciaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarValorTransferenciaTotalXEmpresaYTipoflag);

            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.ValoTranVersion, DbType.Int32, version);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ValorTransferenciaDTO entity = new ValorTransferenciaDTO();

                    int IEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(IEmprcodi)) entity.Emprcodi = dr.GetInt32(IEmprcodi);

                    int iEemprenomb = dr.GetOrdinal(this.helper.EMPRNOMB);
                    if (!dr.IsDBNull(iEemprenomb)) entity.EmprNomb = dr.GetString(iEemprenomb);

                    int iValoTranFlag = dr.GetOrdinal(this.helper.ValoTranFlag);
                    if (!dr.IsDBNull(iValoTranFlag)) entity.ValoTranFlag = dr.GetString(iValoTranFlag);

                    int iVTTotalDia = dr.GetOrdinal(this.helper.VTTotalDia);
                    if (!dr.IsDBNull(iVTTotalDia)) entity.VTTotalDia = dr.GetDecimal(iVTTotalDia);

                    entitys.Add(entity);

                }
            }

            return entitys;
        }

        public int? GetMaxVersion(int pericodi)
        {
            int? newId = null;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxVersion);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);
            var scalar = dbProvider.ExecuteScalar(command);
            newId = scalar == DBNull.Value ? (int?)null : int.Parse(scalar.ToString());
            return newId;
        }

        public List<ValorTransferenciaDTO> GetValorTransferenciaAgrpBarra(string barracodi, int pericodi, int vtranversion, string vtranflag)
        {
            List<ValorTransferenciaDTO> entitys = new List<ValorTransferenciaDTO>();
            var query = string.Format(helper.SqlGetValorTransferenciaAgrpBarra, pericodi, vtranversion, vtranflag, barracodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ValorTransferenciaDTO entity = new ValorTransferenciaDTO();

                    int IBarrCodi = dr.GetOrdinal(this.helper.BarrCodi);
                    if (!dr.IsDBNull(IBarrCodi)) entity.BarrCodi = dr.GetInt32(IBarrCodi);

                    int iPeriCodi = dr.GetOrdinal(this.helper.PeriCodi);
                    if (!dr.IsDBNull(iPeriCodi)) entity.PeriCodi = dr.GetInt32(iPeriCodi);

                    int iBARRNOMBRE = dr.GetOrdinal(this.helper.BARRNOMBRE);
                    if (!dr.IsDBNull(iBARRNOMBRE)) entity.BarrNombBarrTran = dr.GetString(iBARRNOMBRE);

                    int iVTTotalDia = dr.GetOrdinal(this.helper.VTTotalDia);
                    if (!dr.IsDBNull(iVTTotalDia)) entity.VTTotalDia = dr.GetDecimal(iVTTotalDia);

                    int iVTTotalEnergia = dr.GetOrdinal(this.helper.VTTotalEnergia);
                    if (!dr.IsDBNull(iVTTotalEnergia)) entity.VTTotalEnergia = dr.GetDecimal(iVTTotalEnergia);

                    entitys.Add(entity);

                }
            }

            return entitys;
        }

        #endregion

        #region SIOSEIN

        public List<ValorTransferenciaDTO> ObtenerListaValoresTransferencia(int pericodi, int vtranversion, string vtranflag, string emprcodi)
        {
            List<ValorTransferenciaDTO> entitys = new List<ValorTransferenciaDTO>();
            var query = string.Format(helper.SqlObtenerListaValoresTransferencia, pericodi, vtranversion, vtranflag, emprcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ValorTransferenciaDTO entity = new ValorTransferenciaDTO();

                    int iBarrCodi = dr.GetOrdinal(helper.BarrCodi);
                    if (!dr.IsDBNull(iBarrCodi)) entity.BarrCodi = dr.GetInt32(iBarrCodi);

                    int iEmpCodi = dr.GetOrdinal(helper.EmpCodi);
                    if (!dr.IsDBNull(iEmpCodi)) entity.EmpCodi = dr.GetInt32(iEmpCodi);

                    int iValoTranFlag = dr.GetOrdinal(helper.ValoTranFlag);
                    if (!dr.IsDBNull(iValoTranFlag)) entity.ValoTranFlag = dr.GetString(iValoTranFlag);

                    int iValoTranCodEntRet = dr.GetOrdinal(helper.ValoTranCodEntRet);
                    if (!dr.IsDBNull(iValoTranCodEntRet)) entity.ValoTranCodEntRet = dr.GetString(iValoTranCodEntRet);

                    int iVTTotalDia = dr.GetOrdinal(helper.VTTotalDia);
                    if (!dr.IsDBNull(iVTTotalDia)) entity.VTTotalDia = dr.GetDecimal(iVTTotalDia);

                    int iVTTotalEnergia = dr.GetOrdinal(helper.VTTotalEnergia);
                    if (!dr.IsDBNull(iVTTotalEnergia)) entity.VTTotalEnergia = dr.GetDecimal(iVTTotalEnergia);

                    int iOsinergcodi = dr.GetOrdinal(helper.Osinergcodi);
                    if (!dr.IsDBNull(iOsinergcodi)) entity.Osinergcodi = dr.GetString(iOsinergcodi);

                    int iBARRNOMBRE = dr.GetOrdinal(helper.BARRNOMBRE);
                    if (!dr.IsDBNull(iBARRNOMBRE)) entity.BarrNombBarrTran = dr.GetString(iBARRNOMBRE);

                    int iEMPRNOMB = dr.GetOrdinal(helper.EMPRNOMB);
                    if (!dr.IsDBNull(iEMPRNOMB)) entity.EmprNomb = dr.GetString(iEMPRNOMB);

                    int iEmprcodosinergmin = dr.GetOrdinal(helper.Emprcodosinergmin);
                    if (!dr.IsDBNull(iEmprcodosinergmin)) entity.Emprcodosinergmin = dr.GetString(iEmprcodosinergmin);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion


        public List<ValorTransferenciaDTO> ListarCodigosValorizadosTransferencia(int pericodi, int version, int? empcodi, int? barrcodi, string flagEntrReti, DateTime? fechaIni, DateTime? fechaFin) //int pericodi, string barrcodi
        {
            string query = string.Format(helper.SqlListarCodigosValorizadosTransferencia, fechaIni?.ToString(ConstantesBase.FormatoFecha), fechaFin?.ToString(ConstantesBase.FormatoFecha));


            List<ValorTransferenciaDTO> entitys = new List<ValorTransferenciaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.ValoTranVersion, DbType.Int32, version);
            dbProvider.AddInParameter(command, helper.EmpCodi, DbType.Int32, empcodi);
            dbProvider.AddInParameter(command, helper.EmpCodi, DbType.Int32, empcodi);
            dbProvider.AddInParameter(command, helper.BarrCodi, DbType.Int32, barrcodi);
            dbProvider.AddInParameter(command, helper.BarrCodi, DbType.Int32, barrcodi);
            dbProvider.AddInParameter(command, "flag", DbType.String, flagEntrReti);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ValorTransferenciaDTO entity = new ValorTransferenciaDTO();
                    entity.ValoTranCodi = 0;

                    int iNombEmpresa = dr.GetOrdinal(this.helper.NombEmpresa);
                    if (!dr.IsDBNull(iNombEmpresa)) entity.NombEmpresa = dr.GetString(iNombEmpresa);

                    int iValoTranCodEntRet = dr.GetOrdinal(this.helper.ValoTranCodEntRet);
                    if (!dr.IsDBNull(iValoTranCodEntRet)) entity.ValoTranCodEntRet = dr.GetString(iValoTranCodEntRet);



                    entitys.Add(entity);

                }
            }

            return entitys;
        }

        public List<ValorTransferenciaDTO> ListarCodigosValorizados(int pericodi, int version, int? empcodi, int? cliemprcodi, int? barrcodi, string flagEntrReti, DateTime? fechaIni, DateTime? fechaFin) //int pericodi, string barrcodi
        {
            string query = string.Format(helper.SqlListarCodigosValorizados, fechaIni?.ToString(ConstantesBase.FormatoFecha), fechaFin?.ToString(ConstantesBase.FormatoFecha), string.IsNullOrEmpty(flagEntrReti) ? "T" : flagEntrReti);


            List<ValorTransferenciaDTO> entitys = new List<ValorTransferenciaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.ValoTranVersion, DbType.Int32, version);
            dbProvider.AddInParameter(command, helper.EmpCodi, DbType.Int32, empcodi);
            dbProvider.AddInParameter(command, helper.EmpCodi, DbType.Int32, empcodi);
            dbProvider.AddInParameter(command, helper.Cliemprcodi, DbType.Int32, cliemprcodi);
            dbProvider.AddInParameter(command, helper.Cliemprcodi, DbType.Int32, cliemprcodi);
            dbProvider.AddInParameter(command, helper.BarrCodi, DbType.Int32, barrcodi);
            dbProvider.AddInParameter(command, helper.BarrCodi, DbType.Int32, barrcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ValorTransferenciaDTO entity = new ValorTransferenciaDTO();
                    entity.ValoTranCodi = 0;

                    int iNombEmpresa = dr.GetOrdinal(this.helper.NombEmpresa);
                    if (!dr.IsDBNull(iNombEmpresa)) entity.NombEmpresa = dr.GetString(iNombEmpresa);

                    int iValoTranCodEntRet = dr.GetOrdinal(this.helper.ValoTranCodEntRet);
                    if (!dr.IsDBNull(iValoTranCodEntRet)) entity.ValoTranCodEntRet = dr.GetString(iValoTranCodEntRet);



                    entitys.Add(entity);

                }
            }

            return entitys;
        }


        public List<ValorTransferenciaDTO> ListarCodigosValorizadosGrafica(int pericodi, int version, int? empcodi, string codigos) //int pericodi, string barrcodi
        {
            List<ValorTransferenciaDTO> entitys = new List<ValorTransferenciaDTO>();
            string query = helper.SqlListarCodigosValorizadosGrafica.Replace("@codigo", codigos);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.ValoTranVersion, DbType.Int32, version);
            dbProvider.AddInParameter(command, helper.EmpCodi, DbType.Int32, empcodi);
            dbProvider.AddInParameter(command, helper.EmpCodi, DbType.Int32, empcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ValorTransferenciaDTO entity = new ValorTransferenciaDTO();

                    entity = helper.Create(dr);

                    entitys.Add(entity);

                }
            }

            return entitys;
        }

        public DataTable ListarCodigosValorizadosGrafica_New(int pericodi, int version, int? empcodi, string codigos, DateTime FechaInicio, DateTime FechaFin) //int pericodi, string barrcodi
        {


            DataTable tbl = new DataTable();
            string query = helper.SqlListarCodigosValorizadosGraficaNew.Replace("@codigo", codigos);
            query = string.Format(query, FechaInicio.ToString(ConstantesBase.FormatoFecha), FechaFin.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.ValoTranVersion, DbType.Int32, version);
            dbProvider.AddInParameter(command, helper.EmpCodi, DbType.Int32, empcodi);
            dbProvider.AddInParameter(command, helper.EmpCodi, DbType.Int32, empcodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                tbl.Load(dr);
            }
            return tbl;
        }

        public DataTable ListarCodigosValorizadosGraficaTransferencia_New(int pericodi, int version, int? empcodi, string codigos, DateTime FechaInicio, DateTime FechaFin) //int pericodi, string barrcodi
        {


            DataTable tbl = new DataTable();
            string query = helper.SqlListarCodigosValorizadosTransferenciaGraficaNew.Replace("@codigo", codigos);
            query = string.Format(query, FechaInicio.ToString(ConstantesBase.FormatoFecha), FechaFin.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.ValoTranVersion, DbType.Int32, version);
            dbProvider.AddInParameter(command, helper.EmpCodi, DbType.Int32, empcodi);
            dbProvider.AddInParameter(command, helper.EmpCodi, DbType.Int32, empcodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                tbl.Load(dr);
            }
            return tbl;
        }


        public DataTable ListarCodigosValorizadosGraficaTotal(int pericodi, int version, int? empcodi, string codigos, DateTime? FechaInicio, DateTime? FechaFin) //int pericodi, string barrcodi
        {

            DataTable tbl = new DataTable();
            string query = helper.SqlListarCodigosValorizadosGraficaTotal.Replace("@codigo", codigos);
            query = string.Format(query, FechaInicio?.ToString(ConstantesBase.FormatoFecha), FechaFin?.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.ValoTranVersion, DbType.Int32, version);
            dbProvider.AddInParameter(command, helper.EmpCodi, DbType.Int32, empcodi);
            dbProvider.AddInParameter(command, helper.EmpCodi, DbType.Int32, empcodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                tbl.Load(dr);
            }
            return tbl;

        }

        public DataTable ListarCodigosValorizadosGraficaTotalTransferencia(int pericodi, int version, int? empcodi, string codigos, DateTime? FechaInicio, DateTime? FechaFin) //int pericodi, string barrcodi
        {

            DataTable tbl = new DataTable();
            string query = helper.SqlListarCodigosValorizadosGraficaTotalTransferencia.Replace("@codigo", codigos);
            query = string.Format(query, FechaInicio?.ToString(ConstantesBase.FormatoFecha), FechaFin?.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.ValoTranVersion, DbType.Int32, version);
            dbProvider.AddInParameter(command, helper.EmpCodi, DbType.Int32, empcodi);
            dbProvider.AddInParameter(command, helper.EmpCodi, DbType.Int32, empcodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                tbl.Load(dr);
            }
            return tbl;

        }
        public List<EmpresaDTO> ListarEmpresasAsociadas(ComparacionEntregaRetirosFiltroDTO parametro)
        {
            List<EmpresaDTO> entitys = new List<EmpresaDTO>();
            string query = helper.SqlListarEmpresasAsociadas;
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.AddInParameter(command, helper.PeriCodi1, DbType.Int32, parametro.PeriCodi1);
            dbProvider.AddInParameter(command, helper.PeriCodi2, DbType.Int32, parametro.PeriCodi2);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EmpresaDTO entity = new EmpresaDTO();
                    entity.EmprCodi = Convert.ToInt32(dr["EMPRCODI"].ToString());
                    entity.EmprNombre = Convert.ToString(dr["EMPRNOMB"].ToString());
                    entitys.Add(entity);

                }
            }

            return entitys;
        }
        public List<ValorTransferenciaDTO> ListarCodigos(int EmprCodi)
        {
            List<ValorTransferenciaDTO> entitys = new List<ValorTransferenciaDTO>();
            string query = helper.SqlListarCodigos;
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, EmprCodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ValorTransferenciaDTO entity = new ValorTransferenciaDTO();

                    if (!dr.IsDBNull(1)) entity.ValoTranCodEntRet = dr.GetString(1);

                    entitys.Add(entity);

                }
            }

            return entitys;
        }

        public List<ValorTransferenciaDTO> ListarComparativo(int pericodi, int version,
            int? empcodi, int? cliemprcodi, int? barrcodi, string flagEntrReti, string dias, string codigos)
        {
            List<ValorTransferenciaDTO> entitys = new List<ValorTransferenciaDTO>();

            codigos = (string.IsNullOrEmpty(codigos)) ? "vtrancodentret" : codigos;
            dias = (string.IsNullOrEmpty(dias)) ? "vtrandia" : dias;

            string query = helper.SqlListarComparativo.Replace("@codigo", codigos).Replace("@dia", dias);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.ValoTranVersion, DbType.Int32, version);
            dbProvider.AddInParameter(command, helper.EmpCodi, DbType.Int32, empcodi);
            dbProvider.AddInParameter(command, helper.EmpCodi, DbType.Int32, empcodi);
            dbProvider.AddInParameter(command, helper.Cliemprcodi, DbType.Int32, cliemprcodi);
            dbProvider.AddInParameter(command, helper.Cliemprcodi, DbType.Int32, cliemprcodi);
            dbProvider.AddInParameter(command, helper.BarrCodi, DbType.Int32, barrcodi);
            dbProvider.AddInParameter(command, helper.BarrCodi, DbType.Int32, barrcodi);
            dbProvider.AddInParameter(command, helper.ValoTranFlag, DbType.String, flagEntrReti);
            dbProvider.AddInParameter(command, helper.ValoTranFlag, DbType.String, flagEntrReti);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ValorTransferenciaDTO entity = new ValorTransferenciaDTO();

                    int iValoTranDia = dr.GetOrdinal(helper.ValoTranDia);
                    if (!dr.IsDBNull(iValoTranDia)) entity.ValoTranDia = dr.GetInt32(iValoTranDia);

                    int iVT1 = dr.GetOrdinal(helper.VT1);
                    if (!dr.IsDBNull(iVT1)) entity.VT1 = dr.GetDecimal(iVT1);

                    int iVT2 = dr.GetOrdinal(helper.VT2);
                    if (!dr.IsDBNull(iVT2)) entity.VT2 = dr.GetDecimal(iVT2);

                    int iVT3 = dr.GetOrdinal(helper.VT3);
                    if (!dr.IsDBNull(iVT3)) entity.VT3 = dr.GetDecimal(iVT3);

                    int iVT4 = dr.GetOrdinal(helper.VT4);
                    if (!dr.IsDBNull(iVT4)) entity.VT4 = dr.GetDecimal(iVT4);

                    int iVT5 = dr.GetOrdinal(helper.VT5);
                    if (!dr.IsDBNull(iVT5)) entity.VT5 = dr.GetDecimal(iVT5);

                    int iVT6 = dr.GetOrdinal(helper.VT6);
                    if (!dr.IsDBNull(iVT6)) entity.VT6 = dr.GetDecimal(iVT6);

                    int iVT7 = dr.GetOrdinal(helper.VT7);
                    if (!dr.IsDBNull(iVT7)) entity.VT7 = dr.GetDecimal(iVT7);

                    int iVT8 = dr.GetOrdinal(helper.VT8);
                    if (!dr.IsDBNull(iVT8)) entity.VT8 = dr.GetDecimal(iVT8);

                    int iVT9 = dr.GetOrdinal(helper.VT9);
                    if (!dr.IsDBNull(iVT9)) entity.VT9 = dr.GetDecimal(iVT9);

                    int iVT10 = dr.GetOrdinal(helper.VT10);
                    if (!dr.IsDBNull(iVT10)) entity.VT10 = dr.GetDecimal(iVT10);

                    int iVT11 = dr.GetOrdinal(helper.VT11);
                    if (!dr.IsDBNull(iVT11)) entity.VT11 = dr.GetDecimal(iVT11);

                    int iVT12 = dr.GetOrdinal(helper.VT12);
                    if (!dr.IsDBNull(iVT12)) entity.VT12 = dr.GetDecimal(iVT12);

                    int iVT13 = dr.GetOrdinal(helper.VT13);
                    if (!dr.IsDBNull(iVT13)) entity.VT13 = dr.GetDecimal(iVT13);

                    int iVT14 = dr.GetOrdinal(helper.VT14);
                    if (!dr.IsDBNull(iVT14)) entity.VT14 = dr.GetDecimal(iVT14);

                    int iVT15 = dr.GetOrdinal(helper.VT15);
                    if (!dr.IsDBNull(iVT15)) entity.VT15 = dr.GetDecimal(iVT15);

                    int iVT16 = dr.GetOrdinal(helper.VT16);
                    if (!dr.IsDBNull(iVT16)) entity.VT16 = dr.GetDecimal(iVT16);

                    int iVT17 = dr.GetOrdinal(helper.VT17);
                    if (!dr.IsDBNull(iVT17)) entity.VT17 = dr.GetDecimal(iVT17);

                    int iVT18 = dr.GetOrdinal(helper.VT18);
                    if (!dr.IsDBNull(iVT18)) entity.VT18 = dr.GetDecimal(iVT18);

                    int iVT19 = dr.GetOrdinal(helper.VT19);
                    if (!dr.IsDBNull(iVT19)) entity.VT19 = dr.GetDecimal(iVT19);

                    int iVT20 = dr.GetOrdinal(helper.VT20);
                    if (!dr.IsDBNull(iVT20)) entity.VT20 = dr.GetDecimal(iVT20);

                    int iVT21 = dr.GetOrdinal(helper.VT21);
                    if (!dr.IsDBNull(iVT21)) entity.VT21 = dr.GetDecimal(iVT21);

                    int iVT22 = dr.GetOrdinal(helper.VT22);
                    if (!dr.IsDBNull(iVT22)) entity.VT22 = dr.GetDecimal(iVT22);

                    int iVT23 = dr.GetOrdinal(helper.VT23);
                    if (!dr.IsDBNull(iVT23)) entity.VT23 = dr.GetDecimal(iVT23);

                    int iVT24 = dr.GetOrdinal(helper.VT24);
                    if (!dr.IsDBNull(iVT24)) entity.VT24 = dr.GetDecimal(iVT24);

                    int iVT25 = dr.GetOrdinal(helper.VT25);
                    if (!dr.IsDBNull(iVT25)) entity.VT25 = dr.GetDecimal(iVT25);

                    int iVT26 = dr.GetOrdinal(helper.VT26);
                    if (!dr.IsDBNull(iVT26)) entity.VT26 = dr.GetDecimal(iVT26);

                    int iVT27 = dr.GetOrdinal(helper.VT27);
                    if (!dr.IsDBNull(iVT27)) entity.VT27 = dr.GetDecimal(iVT27);

                    int iVT28 = dr.GetOrdinal(helper.VT28);
                    if (!dr.IsDBNull(iVT28)) entity.VT28 = dr.GetDecimal(iVT28);

                    int iVT29 = dr.GetOrdinal(helper.VT29);
                    if (!dr.IsDBNull(iVT29)) entity.VT29 = dr.GetDecimal(iVT29);

                    int iVT30 = dr.GetOrdinal(helper.VT30);
                    if (!dr.IsDBNull(iVT30)) entity.VT30 = dr.GetDecimal(iVT30);

                    int iVT31 = dr.GetOrdinal(helper.VT31);
                    if (!dr.IsDBNull(iVT31)) entity.VT31 = dr.GetDecimal(iVT31);

                    int iVT32 = dr.GetOrdinal(helper.VT32);
                    if (!dr.IsDBNull(iVT32)) entity.VT32 = dr.GetDecimal(iVT32);

                    int iVT33 = dr.GetOrdinal(helper.VT33);
                    if (!dr.IsDBNull(iVT33)) entity.VT33 = dr.GetDecimal(iVT33);

                    int iVT34 = dr.GetOrdinal(helper.VT34);
                    if (!dr.IsDBNull(iVT34)) entity.VT34 = dr.GetDecimal(iVT34);

                    int iVT35 = dr.GetOrdinal(helper.VT35);
                    if (!dr.IsDBNull(iVT35)) entity.VT35 = dr.GetDecimal(iVT35);

                    int iVT36 = dr.GetOrdinal(helper.VT36);
                    if (!dr.IsDBNull(iVT36)) entity.VT36 = dr.GetDecimal(iVT36);

                    int iVT37 = dr.GetOrdinal(helper.VT37);
                    if (!dr.IsDBNull(iVT37)) entity.VT37 = dr.GetDecimal(iVT37);

                    int iVT38 = dr.GetOrdinal(helper.VT38);
                    if (!dr.IsDBNull(iVT38)) entity.VT38 = dr.GetDecimal(iVT38);

                    int iVT39 = dr.GetOrdinal(helper.VT39);
                    if (!dr.IsDBNull(iVT39)) entity.VT39 = dr.GetDecimal(iVT39);

                    int iVT40 = dr.GetOrdinal(helper.VT40);
                    if (!dr.IsDBNull(iVT40)) entity.VT40 = dr.GetDecimal(iVT40);

                    int iVT41 = dr.GetOrdinal(helper.VT41);
                    if (!dr.IsDBNull(iVT41)) entity.VT41 = dr.GetDecimal(iVT41);

                    int iVT42 = dr.GetOrdinal(helper.VT42);
                    if (!dr.IsDBNull(iVT42)) entity.VT42 = dr.GetDecimal(iVT42);

                    int iVT43 = dr.GetOrdinal(helper.VT43);
                    if (!dr.IsDBNull(iVT43)) entity.VT43 = dr.GetDecimal(iVT43);

                    int iVT44 = dr.GetOrdinal(helper.VT44);
                    if (!dr.IsDBNull(iVT44)) entity.VT44 = dr.GetDecimal(iVT44);

                    int iVT45 = dr.GetOrdinal(helper.VT45);
                    if (!dr.IsDBNull(iVT45)) entity.VT45 = dr.GetDecimal(iVT45);

                    int iVT46 = dr.GetOrdinal(helper.VT46);
                    if (!dr.IsDBNull(iVT46)) entity.VT46 = dr.GetDecimal(iVT46);

                    int iVT47 = dr.GetOrdinal(helper.VT47);
                    if (!dr.IsDBNull(iVT47)) entity.VT47 = dr.GetDecimal(iVT47);

                    int iVT48 = dr.GetOrdinal(helper.VT48);
                    if (!dr.IsDBNull(iVT48)) entity.VT48 = dr.GetDecimal(iVT48);

                    int iVT49 = dr.GetOrdinal(helper.VT49);
                    if (!dr.IsDBNull(iVT49)) entity.VT49 = dr.GetDecimal(iVT49);

                    int iVT50 = dr.GetOrdinal(helper.VT50);
                    if (!dr.IsDBNull(iVT50)) entity.VT50 = dr.GetDecimal(iVT50);

                    int iVT51 = dr.GetOrdinal(helper.VT51);
                    if (!dr.IsDBNull(iVT51)) entity.VT51 = dr.GetDecimal(iVT51);

                    int iVT52 = dr.GetOrdinal(helper.VT52);
                    if (!dr.IsDBNull(iVT52)) entity.VT52 = dr.GetDecimal(iVT52);

                    int iVT53 = dr.GetOrdinal(helper.VT53);
                    if (!dr.IsDBNull(iVT53)) entity.VT53 = dr.GetDecimal(iVT53);

                    int iVT54 = dr.GetOrdinal(helper.VT54);
                    if (!dr.IsDBNull(iVT54)) entity.VT54 = dr.GetDecimal(iVT54);

                    int iVT55 = dr.GetOrdinal(helper.VT55);
                    if (!dr.IsDBNull(iVT55)) entity.VT55 = dr.GetDecimal(iVT55);

                    int iVT56 = dr.GetOrdinal(helper.VT56);
                    if (!dr.IsDBNull(iVT56)) entity.VT56 = dr.GetDecimal(iVT56);

                    int iVT57 = dr.GetOrdinal(helper.VT57);
                    if (!dr.IsDBNull(iVT57)) entity.VT57 = dr.GetDecimal(iVT57);

                    int iVT58 = dr.GetOrdinal(helper.VT58);
                    if (!dr.IsDBNull(iVT58)) entity.VT58 = dr.GetDecimal(iVT58);

                    int iVT59 = dr.GetOrdinal(helper.VT59);
                    if (!dr.IsDBNull(iVT59)) entity.VT59 = dr.GetDecimal(iVT59);

                    int iVT60 = dr.GetOrdinal(helper.VT60);
                    if (!dr.IsDBNull(iVT60)) entity.VT60 = dr.GetDecimal(iVT60);

                    int iVT61 = dr.GetOrdinal(helper.VT61);
                    if (!dr.IsDBNull(iVT61)) entity.VT61 = dr.GetDecimal(iVT61);

                    int iVT62 = dr.GetOrdinal(helper.VT62);
                    if (!dr.IsDBNull(iVT62)) entity.VT62 = dr.GetDecimal(iVT62);

                    int iVT63 = dr.GetOrdinal(helper.VT63);
                    if (!dr.IsDBNull(iVT63)) entity.VT63 = dr.GetDecimal(iVT63);

                    int iVT64 = dr.GetOrdinal(helper.VT64);
                    if (!dr.IsDBNull(iVT64)) entity.VT64 = dr.GetDecimal(iVT64);

                    int iVT65 = dr.GetOrdinal(helper.VT65);
                    if (!dr.IsDBNull(iVT65)) entity.VT65 = dr.GetDecimal(iVT65);

                    int iVT66 = dr.GetOrdinal(helper.VT66);
                    if (!dr.IsDBNull(iVT66)) entity.VT66 = dr.GetDecimal(iVT66);

                    int iVT67 = dr.GetOrdinal(helper.VT67);
                    if (!dr.IsDBNull(iVT67)) entity.VT67 = dr.GetDecimal(iVT67);

                    int iVT68 = dr.GetOrdinal(helper.VT68);
                    if (!dr.IsDBNull(iVT68)) entity.VT68 = dr.GetDecimal(iVT68);

                    int iVT69 = dr.GetOrdinal(helper.VT69);
                    if (!dr.IsDBNull(iVT60)) entity.VT69 = dr.GetDecimal(iVT69);

                    int iVT70 = dr.GetOrdinal(helper.VT70);
                    if (!dr.IsDBNull(iVT70)) entity.VT70 = dr.GetDecimal(iVT70);

                    int iVT71 = dr.GetOrdinal(helper.VT71);
                    if (!dr.IsDBNull(iVT71)) entity.VT71 = dr.GetDecimal(iVT71);

                    int iVT72 = dr.GetOrdinal(helper.VT72);
                    if (!dr.IsDBNull(iVT72)) entity.VT72 = dr.GetDecimal(iVT72);

                    int iVT73 = dr.GetOrdinal(helper.VT73);
                    if (!dr.IsDBNull(iVT73)) entity.VT73 = dr.GetDecimal(iVT73);

                    int iVT74 = dr.GetOrdinal(helper.VT74);
                    if (!dr.IsDBNull(iVT74)) entity.VT74 = dr.GetDecimal(iVT74);

                    int iVT75 = dr.GetOrdinal(helper.VT75);
                    if (!dr.IsDBNull(iVT75)) entity.VT75 = dr.GetDecimal(iVT75);

                    int iVT76 = dr.GetOrdinal(helper.VT76);
                    if (!dr.IsDBNull(iVT76)) entity.VT76 = dr.GetDecimal(iVT76);

                    int iVT77 = dr.GetOrdinal(helper.VT77);
                    if (!dr.IsDBNull(iVT77)) entity.VT77 = dr.GetDecimal(iVT77);

                    int iVT78 = dr.GetOrdinal(helper.VT78);
                    if (!dr.IsDBNull(iVT78)) entity.VT78 = dr.GetDecimal(iVT78);

                    int iVT79 = dr.GetOrdinal(helper.VT79);
                    if (!dr.IsDBNull(iVT79)) entity.VT79 = dr.GetDecimal(iVT79);

                    int iVT80 = dr.GetOrdinal(helper.VT80);
                    if (!dr.IsDBNull(iVT80)) entity.VT80 = dr.GetDecimal(iVT80);

                    int iVT81 = dr.GetOrdinal(helper.VT81);
                    if (!dr.IsDBNull(iVT81)) entity.VT81 = dr.GetDecimal(iVT81);

                    int iVT82 = dr.GetOrdinal(helper.VT82);
                    if (!dr.IsDBNull(iVT82)) entity.VT82 = dr.GetDecimal(iVT82);

                    int iVT83 = dr.GetOrdinal(helper.VT83);
                    if (!dr.IsDBNull(iVT83)) entity.VT83 = dr.GetDecimal(iVT83);

                    int iVT84 = dr.GetOrdinal(helper.VT84);
                    if (!dr.IsDBNull(iVT84)) entity.VT84 = dr.GetDecimal(iVT84);

                    int iVT85 = dr.GetOrdinal(helper.VT85);
                    if (!dr.IsDBNull(iVT85)) entity.VT85 = dr.GetDecimal(iVT85);

                    int iVT86 = dr.GetOrdinal(helper.VT86);
                    if (!dr.IsDBNull(iVT86)) entity.VT86 = dr.GetDecimal(iVT86);

                    int iVT87 = dr.GetOrdinal(helper.VT87);
                    if (!dr.IsDBNull(iVT87)) entity.VT87 = dr.GetDecimal(iVT87);

                    int iVT88 = dr.GetOrdinal(helper.VT88);
                    if (!dr.IsDBNull(iVT88)) entity.VT88 = dr.GetDecimal(iVT88);

                    int iVT89 = dr.GetOrdinal(helper.VT89);
                    if (!dr.IsDBNull(iVT89)) entity.VT89 = dr.GetDecimal(iVT89);

                    int iVT90 = dr.GetOrdinal(helper.VT90);
                    if (!dr.IsDBNull(iVT90)) entity.VT90 = dr.GetDecimal(iVT90);

                    int iVT91 = dr.GetOrdinal(helper.VT91);
                    if (!dr.IsDBNull(iVT91)) entity.VT91 = dr.GetDecimal(iVT91);

                    int iVT92 = dr.GetOrdinal(helper.VT92);
                    if (!dr.IsDBNull(iVT92)) entity.VT92 = dr.GetDecimal(iVT92);

                    int iVT93 = dr.GetOrdinal(helper.VT93);
                    if (!dr.IsDBNull(iVT93)) entity.VT93 = dr.GetDecimal(iVT93);

                    int iVT94 = dr.GetOrdinal(helper.VT94);
                    if (!dr.IsDBNull(iVT94)) entity.VT94 = dr.GetDecimal(iVT94);

                    int iVT95 = dr.GetOrdinal(helper.VT95);
                    if (!dr.IsDBNull(iVT95)) entity.VT95 = dr.GetDecimal(iVT95);

                    int iVT96 = dr.GetOrdinal(helper.VT96);
                    if (!dr.IsDBNull(iVT96)) entity.VT96 = dr.GetDecimal(iVT96);

                    entitys.Add(entity);

                }
            }

            return entitys;
        }



        #region CUP08-Graficas


        public DataTable ListarComparativoEntregaRetiroValo(ComparacionEntregaRetirosFiltroDTO parametro)
        {


            string filtro = "";

            if (parametro.CodigoRetiroArray != null && parametro.CodigoRetiroArray.Count < 1000)
                filtro = "AND TENTCODIGO IN(" + string.Join(",", parametro.CodigoRetiroArray) + ")";


            DataTable tbl = new DataTable();
            string query = string.Format(helper.SqlListarComparativoEntregaRetiroValor,
               string.Join(",", parametro.DiaArray),
                filtro
                );
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.AddInParameter(command, helper.PeriCodi1, DbType.Int32, parametro.PeriCodi1);
            dbProvider.AddInParameter(command, helper.VersionCodi1, DbType.Int32, parametro.VersionCodi1);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, parametro.EmprCodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, parametro.EmprCodi);
            dbProvider.AddInParameter(command, helper.CliCodi, DbType.Int32, parametro.CliCodi);
            dbProvider.AddInParameter(command, helper.CliCodi, DbType.Int32, parametro.CliCodi);
            dbProvider.AddInParameter(command, helper.BarrCodi, DbType.Int32, parametro.BarrCodi);
            dbProvider.AddInParameter(command, helper.BarrCodi, DbType.Int32, parametro.BarrCodi);
            dbProvider.AddInParameter(command, helper.TipoEntregaCodi, DbType.String, parametro.TipoEntregaCodi);
            dbProvider.AddInParameter(command, helper.TipoEntregaCodi, DbType.String, parametro.TipoEntregaCodi);


            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                tbl.Load(dr);
            }
            return tbl;

        }
        public DataTable ListarComparativoEntregaRetiroValoTransferencia(ComparacionEntregaRetirosFiltroDTO parametro)
        {


            string filtro = "";

            if (parametro.CodigoRetiroArray != null && parametro.CodigoRetiroArray.Count < 1000)
                filtro = "AND TENTCODIGO IN(" + string.Join(",", parametro.CodigoRetiroArray) + ")";


            DataTable tbl = new DataTable();
            string query = string.Format(helper.SqlListarComparativoEntregaRetiroValorTransferencia,
               string.Join(",", parametro.DiaArray),
                filtro
                );
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.AddInParameter(command, helper.PeriCodi1, DbType.Int32, parametro.PeriCodi1);
            dbProvider.AddInParameter(command, helper.VersionCodi1, DbType.Int32, parametro.VersionCodi1);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, parametro.EmprCodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, parametro.EmprCodi);
            dbProvider.AddInParameter(command, helper.CliCodi, DbType.Int32, parametro.CliCodi);
            dbProvider.AddInParameter(command, helper.CliCodi, DbType.Int32, parametro.CliCodi);
            dbProvider.AddInParameter(command, helper.BarrCodi, DbType.Int32, parametro.BarrCodi);
            dbProvider.AddInParameter(command, helper.BarrCodi, DbType.Int32, parametro.BarrCodi);
            dbProvider.AddInParameter(command, helper.TipoEntregaCodi, DbType.String, parametro.TipoEntregaCodi);
            dbProvider.AddInParameter(command, helper.TipoEntregaCodi, DbType.String, parametro.TipoEntregaCodi);


            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                tbl.Load(dr);
            }
            return tbl;

        }

        public DataTable ListarComparativoEntregaRetiroValorDET(ComparacionEntregaRetirosFiltroDTO parametro)
        {
            int cantidad = 0;

            string filtro = "";

            if (parametro.CodigoRetiroArray != null && parametro.CodigoRetiroArray.Count < 1000)
                filtro = "AND TENTCODIGO IN(" + string.Join(",", parametro.CodigoRetiroArray) + ")";



            DataTable tbl = new DataTable();
            string query = string.Format(helper.SqlListarComparativoEntregaRetiroValorDET,
               string.Join(",", parametro.DiaArray),
              filtro
                );

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.AddInParameter(command, helper.PeriCodi1, DbType.Int32, parametro.PeriCodi1);
            dbProvider.AddInParameter(command, helper.VersionCodi1, DbType.Int32, parametro.VersionCodi1);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, parametro.EmprCodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, parametro.EmprCodi);
            dbProvider.AddInParameter(command, helper.CliCodi, DbType.Int32, parametro.CliCodi);
            dbProvider.AddInParameter(command, helper.CliCodi, DbType.Int32, parametro.CliCodi);
            dbProvider.AddInParameter(command, helper.BarrCodi, DbType.Int32, parametro.BarrCodi);
            dbProvider.AddInParameter(command, helper.BarrCodi, DbType.Int32, parametro.BarrCodi);
            dbProvider.AddInParameter(command, helper.TipoEntregaCodi, DbType.String, parametro.TipoEntregaCodi);
            dbProvider.AddInParameter(command, helper.TipoEntregaCodi, DbType.String, parametro.TipoEntregaCodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                tbl.Load(dr);
            }
            return tbl;

        }

        public DataTable ListarComparativoEntregaRetiroValorDETTransferencia(ComparacionEntregaRetirosFiltroDTO parametro)
        {
            int cantidad = 0;

            string filtro = "";

            if (parametro.CodigoRetiroArray != null && parametro.CodigoRetiroArray.Count < 1000)
                filtro = "AND TENTCODIGO IN(" + string.Join(",", parametro.CodigoRetiroArray) + ")";

            DataTable tbl = new DataTable();
            string query = string.Format(helper.SqlListarComparativoEntregaRetiroValorDETTransferencia,
               string.Join(",", parametro.DiaArray),
              filtro
                );

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.AddInParameter(command, helper.PeriCodi1, DbType.Int32, parametro.PeriCodi1);
            dbProvider.AddInParameter(command, helper.VersionCodi1, DbType.Int32, parametro.VersionCodi1);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, parametro.EmprCodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, parametro.EmprCodi);
            dbProvider.AddInParameter(command, helper.CliCodi, DbType.Int32, parametro.CliCodi);
            dbProvider.AddInParameter(command, helper.CliCodi, DbType.Int32, parametro.CliCodi);
            dbProvider.AddInParameter(command, helper.BarrCodi, DbType.Int32, parametro.BarrCodi);
            dbProvider.AddInParameter(command, helper.BarrCodi, DbType.Int32, parametro.BarrCodi);
            dbProvider.AddInParameter(command, helper.TipoEntregaCodi, DbType.String, parametro.TipoEntregaCodi);
            dbProvider.AddInParameter(command, helper.TipoEntregaCodi, DbType.String, parametro.TipoEntregaCodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                tbl.Load(dr);
            }
            return tbl;

        }
        #endregion CUP08-Graficas


        //CU21
        public List<ValorTransferenciaDTO> ListarEnergiaEntregaDetalle(int iPeriCodi, int iVTranVersion, int iCodEntCodi)
        {
            List<ValorTransferenciaDTO> entitys = new List<ValorTransferenciaDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarEnergiaEntregaDetalle);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, iPeriCodi);
            dbProvider.AddInParameter(command, helper.ValoTranVersion, DbType.Int32, iVTranVersion);
            dbProvider.AddInParameter(command, helper.CodEntCodi, DbType.Int32, iCodEntCodi);
            
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ValorTransferenciaDTO entity = new ValorTransferenciaDTO();
                    
                    int iValoTranDia = dr.GetOrdinal(helper.ValoTranDia);
                    if (!dr.IsDBNull(iValoTranDia)) entity.ValoTranDia = dr.GetInt32(iValoTranDia);

                    int iVTTotalEnergia = dr.GetOrdinal(helper.VTTotalEnergia);
                    if (!dr.IsDBNull(iVTTotalEnergia)) entity.VTTotalEnergia = dr.GetDecimal(iVTTotalEnergia);

                    int iVT1 = dr.GetOrdinal(helper.VT1);
                    if (!dr.IsDBNull(iVT1)) entity.VT1 = dr.GetDecimal(iVT1);

                    int iVT2 = dr.GetOrdinal(helper.VT2);
                    if (!dr.IsDBNull(iVT2)) entity.VT2 = dr.GetDecimal(iVT2);

                    int iVT3 = dr.GetOrdinal(helper.VT3);
                    if (!dr.IsDBNull(iVT3)) entity.VT3 = dr.GetDecimal(iVT3);

                    int iVT4 = dr.GetOrdinal(helper.VT4);
                    if (!dr.IsDBNull(iVT4)) entity.VT4 = dr.GetDecimal(iVT4);

                    int iVT5 = dr.GetOrdinal(helper.VT5);
                    if (!dr.IsDBNull(iVT5)) entity.VT5 = dr.GetDecimal(iVT5);

                    int iVT6 = dr.GetOrdinal(helper.VT6);
                    if (!dr.IsDBNull(iVT6)) entity.VT6 = dr.GetDecimal(iVT6);

                    int iVT7 = dr.GetOrdinal(helper.VT7);
                    if (!dr.IsDBNull(iVT7)) entity.VT7 = dr.GetDecimal(iVT7);

                    int iVT8 = dr.GetOrdinal(helper.VT8);
                    if (!dr.IsDBNull(iVT8)) entity.VT8 = dr.GetDecimal(iVT8);

                    int iVT9 = dr.GetOrdinal(helper.VT9);
                    if (!dr.IsDBNull(iVT9)) entity.VT9 = dr.GetDecimal(iVT9);

                    int iVT10 = dr.GetOrdinal(helper.VT10);
                    if (!dr.IsDBNull(iVT10)) entity.VT10 = dr.GetDecimal(iVT10);

                    int iVT11 = dr.GetOrdinal(helper.VT11);
                    if (!dr.IsDBNull(iVT11)) entity.VT11 = dr.GetDecimal(iVT11);

                    int iVT12 = dr.GetOrdinal(helper.VT12);
                    if (!dr.IsDBNull(iVT12)) entity.VT12 = dr.GetDecimal(iVT12);

                    int iVT13 = dr.GetOrdinal(helper.VT13);
                    if (!dr.IsDBNull(iVT13)) entity.VT13 = dr.GetDecimal(iVT13);

                    int iVT14 = dr.GetOrdinal(helper.VT14);
                    if (!dr.IsDBNull(iVT14)) entity.VT14 = dr.GetDecimal(iVT14);

                    int iVT15 = dr.GetOrdinal(helper.VT15);
                    if (!dr.IsDBNull(iVT15)) entity.VT15 = dr.GetDecimal(iVT15);

                    int iVT16 = dr.GetOrdinal(helper.VT16);
                    if (!dr.IsDBNull(iVT16)) entity.VT16 = dr.GetDecimal(iVT16);

                    int iVT17 = dr.GetOrdinal(helper.VT17);
                    if (!dr.IsDBNull(iVT17)) entity.VT17 = dr.GetDecimal(iVT17);

                    int iVT18 = dr.GetOrdinal(helper.VT18);
                    if (!dr.IsDBNull(iVT18)) entity.VT18 = dr.GetDecimal(iVT18);

                    int iVT19 = dr.GetOrdinal(helper.VT19);
                    if (!dr.IsDBNull(iVT19)) entity.VT19 = dr.GetDecimal(iVT19);

                    int iVT20 = dr.GetOrdinal(helper.VT20);
                    if (!dr.IsDBNull(iVT20)) entity.VT20 = dr.GetDecimal(iVT20);

                    int iVT21 = dr.GetOrdinal(helper.VT21);
                    if (!dr.IsDBNull(iVT21)) entity.VT21 = dr.GetDecimal(iVT21);

                    int iVT22 = dr.GetOrdinal(helper.VT22);
                    if (!dr.IsDBNull(iVT22)) entity.VT22 = dr.GetDecimal(iVT22);

                    int iVT23 = dr.GetOrdinal(helper.VT23);
                    if (!dr.IsDBNull(iVT23)) entity.VT23 = dr.GetDecimal(iVT23);

                    int iVT24 = dr.GetOrdinal(helper.VT24);
                    if (!dr.IsDBNull(iVT24)) entity.VT24 = dr.GetDecimal(iVT24);

                    int iVT25 = dr.GetOrdinal(helper.VT25);
                    if (!dr.IsDBNull(iVT25)) entity.VT25 = dr.GetDecimal(iVT25);

                    int iVT26 = dr.GetOrdinal(helper.VT26);
                    if (!dr.IsDBNull(iVT26)) entity.VT26 = dr.GetDecimal(iVT26);

                    int iVT27 = dr.GetOrdinal(helper.VT27);
                    if (!dr.IsDBNull(iVT27)) entity.VT27 = dr.GetDecimal(iVT27);

                    int iVT28 = dr.GetOrdinal(helper.VT28);
                    if (!dr.IsDBNull(iVT28)) entity.VT28 = dr.GetDecimal(iVT28);

                    int iVT29 = dr.GetOrdinal(helper.VT29);
                    if (!dr.IsDBNull(iVT29)) entity.VT29 = dr.GetDecimal(iVT29);

                    int iVT30 = dr.GetOrdinal(helper.VT30);
                    if (!dr.IsDBNull(iVT30)) entity.VT30 = dr.GetDecimal(iVT30);

                    int iVT31 = dr.GetOrdinal(helper.VT31);
                    if (!dr.IsDBNull(iVT31)) entity.VT31 = dr.GetDecimal(iVT31);

                    int iVT32 = dr.GetOrdinal(helper.VT32);
                    if (!dr.IsDBNull(iVT32)) entity.VT32 = dr.GetDecimal(iVT32);

                    int iVT33 = dr.GetOrdinal(helper.VT33);
                    if (!dr.IsDBNull(iVT33)) entity.VT33 = dr.GetDecimal(iVT33);

                    int iVT34 = dr.GetOrdinal(helper.VT34);
                    if (!dr.IsDBNull(iVT34)) entity.VT34 = dr.GetDecimal(iVT34);

                    int iVT35 = dr.GetOrdinal(helper.VT35);
                    if (!dr.IsDBNull(iVT35)) entity.VT35 = dr.GetDecimal(iVT35);

                    int iVT36 = dr.GetOrdinal(helper.VT36);
                    if (!dr.IsDBNull(iVT36)) entity.VT36 = dr.GetDecimal(iVT36);

                    int iVT37 = dr.GetOrdinal(helper.VT37);
                    if (!dr.IsDBNull(iVT37)) entity.VT37 = dr.GetDecimal(iVT37);

                    int iVT38 = dr.GetOrdinal(helper.VT38);
                    if (!dr.IsDBNull(iVT38)) entity.VT38 = dr.GetDecimal(iVT38);

                    int iVT39 = dr.GetOrdinal(helper.VT39);
                    if (!dr.IsDBNull(iVT39)) entity.VT39 = dr.GetDecimal(iVT39);

                    int iVT40 = dr.GetOrdinal(helper.VT40);
                    if (!dr.IsDBNull(iVT40)) entity.VT40 = dr.GetDecimal(iVT40);

                    int iVT41 = dr.GetOrdinal(helper.VT41);
                    if (!dr.IsDBNull(iVT41)) entity.VT41 = dr.GetDecimal(iVT41);

                    int iVT42 = dr.GetOrdinal(helper.VT42);
                    if (!dr.IsDBNull(iVT42)) entity.VT42 = dr.GetDecimal(iVT42);

                    int iVT43 = dr.GetOrdinal(helper.VT43);
                    if (!dr.IsDBNull(iVT43)) entity.VT43 = dr.GetDecimal(iVT43);

                    int iVT44 = dr.GetOrdinal(helper.VT44);
                    if (!dr.IsDBNull(iVT44)) entity.VT44 = dr.GetDecimal(iVT44);

                    int iVT45 = dr.GetOrdinal(helper.VT45);
                    if (!dr.IsDBNull(iVT45)) entity.VT45 = dr.GetDecimal(iVT45);

                    int iVT46 = dr.GetOrdinal(helper.VT46);
                    if (!dr.IsDBNull(iVT46)) entity.VT46 = dr.GetDecimal(iVT46);

                    int iVT47 = dr.GetOrdinal(helper.VT47);
                    if (!dr.IsDBNull(iVT47)) entity.VT47 = dr.GetDecimal(iVT47);

                    int iVT48 = dr.GetOrdinal(helper.VT48);
                    if (!dr.IsDBNull(iVT48)) entity.VT48 = dr.GetDecimal(iVT48);

                    int iVT49 = dr.GetOrdinal(helper.VT49);
                    if (!dr.IsDBNull(iVT49)) entity.VT49 = dr.GetDecimal(iVT49);

                    int iVT50 = dr.GetOrdinal(helper.VT50);
                    if (!dr.IsDBNull(iVT50)) entity.VT50 = dr.GetDecimal(iVT50);

                    int iVT51 = dr.GetOrdinal(helper.VT51);
                    if (!dr.IsDBNull(iVT51)) entity.VT51 = dr.GetDecimal(iVT51);

                    int iVT52 = dr.GetOrdinal(helper.VT52);
                    if (!dr.IsDBNull(iVT52)) entity.VT52 = dr.GetDecimal(iVT52);

                    int iVT53 = dr.GetOrdinal(helper.VT53);
                    if (!dr.IsDBNull(iVT53)) entity.VT53 = dr.GetDecimal(iVT53);

                    int iVT54 = dr.GetOrdinal(helper.VT54);
                    if (!dr.IsDBNull(iVT54)) entity.VT54 = dr.GetDecimal(iVT54);

                    int iVT55 = dr.GetOrdinal(helper.VT55);
                    if (!dr.IsDBNull(iVT55)) entity.VT55 = dr.GetDecimal(iVT55);

                    int iVT56 = dr.GetOrdinal(helper.VT56);
                    if (!dr.IsDBNull(iVT56)) entity.VT56 = dr.GetDecimal(iVT56);

                    int iVT57 = dr.GetOrdinal(helper.VT57);
                    if (!dr.IsDBNull(iVT57)) entity.VT57 = dr.GetDecimal(iVT57);

                    int iVT58 = dr.GetOrdinal(helper.VT58);
                    if (!dr.IsDBNull(iVT58)) entity.VT58 = dr.GetDecimal(iVT58);

                    int iVT59 = dr.GetOrdinal(helper.VT59);
                    if (!dr.IsDBNull(iVT59)) entity.VT59 = dr.GetDecimal(iVT59);

                    int iVT60 = dr.GetOrdinal(helper.VT60);
                    if (!dr.IsDBNull(iVT60)) entity.VT60 = dr.GetDecimal(iVT60);

                    int iVT61 = dr.GetOrdinal(helper.VT61);
                    if (!dr.IsDBNull(iVT61)) entity.VT61 = dr.GetDecimal(iVT61);

                    int iVT62 = dr.GetOrdinal(helper.VT62);
                    if (!dr.IsDBNull(iVT62)) entity.VT62 = dr.GetDecimal(iVT62);

                    int iVT63 = dr.GetOrdinal(helper.VT63);
                    if (!dr.IsDBNull(iVT63)) entity.VT63 = dr.GetDecimal(iVT63);

                    int iVT64 = dr.GetOrdinal(helper.VT64);
                    if (!dr.IsDBNull(iVT64)) entity.VT64 = dr.GetDecimal(iVT64);

                    int iVT65 = dr.GetOrdinal(helper.VT65);
                    if (!dr.IsDBNull(iVT65)) entity.VT65 = dr.GetDecimal(iVT65);

                    int iVT66 = dr.GetOrdinal(helper.VT66);
                    if (!dr.IsDBNull(iVT66)) entity.VT66 = dr.GetDecimal(iVT66);

                    int iVT67 = dr.GetOrdinal(helper.VT67);
                    if (!dr.IsDBNull(iVT67)) entity.VT67 = dr.GetDecimal(iVT67);

                    int iVT68 = dr.GetOrdinal(helper.VT68);
                    if (!dr.IsDBNull(iVT68)) entity.VT68 = dr.GetDecimal(iVT68);

                    int iVT69 = dr.GetOrdinal(helper.VT69);
                    if (!dr.IsDBNull(iVT60)) entity.VT69 = dr.GetDecimal(iVT69);

                    int iVT70 = dr.GetOrdinal(helper.VT70);
                    if (!dr.IsDBNull(iVT70)) entity.VT70 = dr.GetDecimal(iVT70);

                    int iVT71 = dr.GetOrdinal(helper.VT71);
                    if (!dr.IsDBNull(iVT71)) entity.VT71 = dr.GetDecimal(iVT71);

                    int iVT72 = dr.GetOrdinal(helper.VT72);
                    if (!dr.IsDBNull(iVT72)) entity.VT72 = dr.GetDecimal(iVT72);

                    int iVT73 = dr.GetOrdinal(helper.VT73);
                    if (!dr.IsDBNull(iVT73)) entity.VT73 = dr.GetDecimal(iVT73);

                    int iVT74 = dr.GetOrdinal(helper.VT74);
                    if (!dr.IsDBNull(iVT74)) entity.VT74 = dr.GetDecimal(iVT74);

                    int iVT75 = dr.GetOrdinal(helper.VT75);
                    if (!dr.IsDBNull(iVT75)) entity.VT75 = dr.GetDecimal(iVT75);

                    int iVT76 = dr.GetOrdinal(helper.VT76);
                    if (!dr.IsDBNull(iVT76)) entity.VT76 = dr.GetDecimal(iVT76);

                    int iVT77 = dr.GetOrdinal(helper.VT77);
                    if (!dr.IsDBNull(iVT77)) entity.VT77 = dr.GetDecimal(iVT77);

                    int iVT78 = dr.GetOrdinal(helper.VT78);
                    if (!dr.IsDBNull(iVT78)) entity.VT78 = dr.GetDecimal(iVT78);

                    int iVT79 = dr.GetOrdinal(helper.VT79);
                    if (!dr.IsDBNull(iVT79)) entity.VT79 = dr.GetDecimal(iVT79);

                    int iVT80 = dr.GetOrdinal(helper.VT80);
                    if (!dr.IsDBNull(iVT80)) entity.VT80 = dr.GetDecimal(iVT80);

                    int iVT81 = dr.GetOrdinal(helper.VT81);
                    if (!dr.IsDBNull(iVT81)) entity.VT81 = dr.GetDecimal(iVT81);

                    int iVT82 = dr.GetOrdinal(helper.VT82);
                    if (!dr.IsDBNull(iVT82)) entity.VT82 = dr.GetDecimal(iVT82);

                    int iVT83 = dr.GetOrdinal(helper.VT83);
                    if (!dr.IsDBNull(iVT83)) entity.VT83 = dr.GetDecimal(iVT83);

                    int iVT84 = dr.GetOrdinal(helper.VT84);
                    if (!dr.IsDBNull(iVT84)) entity.VT84 = dr.GetDecimal(iVT84);

                    int iVT85 = dr.GetOrdinal(helper.VT85);
                    if (!dr.IsDBNull(iVT85)) entity.VT85 = dr.GetDecimal(iVT85);

                    int iVT86 = dr.GetOrdinal(helper.VT86);
                    if (!dr.IsDBNull(iVT86)) entity.VT86 = dr.GetDecimal(iVT86);

                    int iVT87 = dr.GetOrdinal(helper.VT87);
                    if (!dr.IsDBNull(iVT87)) entity.VT87 = dr.GetDecimal(iVT87);

                    int iVT88 = dr.GetOrdinal(helper.VT88);
                    if (!dr.IsDBNull(iVT88)) entity.VT88 = dr.GetDecimal(iVT88);

                    int iVT89 = dr.GetOrdinal(helper.VT89);
                    if (!dr.IsDBNull(iVT89)) entity.VT89 = dr.GetDecimal(iVT89);

                    int iVT90 = dr.GetOrdinal(helper.VT90);
                    if (!dr.IsDBNull(iVT90)) entity.VT90 = dr.GetDecimal(iVT90);

                    int iVT91 = dr.GetOrdinal(helper.VT91);
                    if (!dr.IsDBNull(iVT91)) entity.VT91 = dr.GetDecimal(iVT91);

                    int iVT92 = dr.GetOrdinal(helper.VT92);
                    if (!dr.IsDBNull(iVT92)) entity.VT92 = dr.GetDecimal(iVT92);

                    int iVT93 = dr.GetOrdinal(helper.VT93);
                    if (!dr.IsDBNull(iVT93)) entity.VT93 = dr.GetDecimal(iVT93);

                    int iVT94 = dr.GetOrdinal(helper.VT94);
                    if (!dr.IsDBNull(iVT94)) entity.VT94 = dr.GetDecimal(iVT94);

                    int iVT95 = dr.GetOrdinal(helper.VT95);
                    if (!dr.IsDBNull(iVT95)) entity.VT95 = dr.GetDecimal(iVT95);

                    int iVT96 = dr.GetOrdinal(helper.VT96);
                    if (!dr.IsDBNull(iVT96)) entity.VT96 = dr.GetDecimal(iVT96);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<ValorTransferenciaDTO> ListarEnergiaRetiroDetalle(int iPeriCodi, int iVTranVersion, string listaCodigosRetiro)
        {
            List<ValorTransferenciaDTO> entitys = new List<ValorTransferenciaDTO>();
           
            var queryString = string.Format(helper.SqlListarEnergiaRetiroDetalle, iPeriCodi, iVTranVersion, listaCodigosRetiro);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ValorTransferenciaDTO entity = new ValorTransferenciaDTO();

                    int iValoTranDia = dr.GetOrdinal(helper.ValoTranDia);
                    if (!dr.IsDBNull(iValoTranDia)) entity.ValoTranDia = dr.GetInt32(iValoTranDia);

                    int iVTTotalEnergia = dr.GetOrdinal(helper.VTTotalEnergia);
                    if (!dr.IsDBNull(iVTTotalEnergia)) entity.VTTotalEnergia = dr.GetDecimal(iVTTotalEnergia);

                    int iVT1 = dr.GetOrdinal(helper.VT1);
                    if (!dr.IsDBNull(iVT1)) entity.VT1 = dr.GetDecimal(iVT1);

                    int iVT2 = dr.GetOrdinal(helper.VT2);
                    if (!dr.IsDBNull(iVT2)) entity.VT2 = dr.GetDecimal(iVT2);

                    int iVT3 = dr.GetOrdinal(helper.VT3);
                    if (!dr.IsDBNull(iVT3)) entity.VT3 = dr.GetDecimal(iVT3);

                    int iVT4 = dr.GetOrdinal(helper.VT4);
                    if (!dr.IsDBNull(iVT4)) entity.VT4 = dr.GetDecimal(iVT4);

                    int iVT5 = dr.GetOrdinal(helper.VT5);
                    if (!dr.IsDBNull(iVT5)) entity.VT5 = dr.GetDecimal(iVT5);

                    int iVT6 = dr.GetOrdinal(helper.VT6);
                    if (!dr.IsDBNull(iVT6)) entity.VT6 = dr.GetDecimal(iVT6);

                    int iVT7 = dr.GetOrdinal(helper.VT7);
                    if (!dr.IsDBNull(iVT7)) entity.VT7 = dr.GetDecimal(iVT7);

                    int iVT8 = dr.GetOrdinal(helper.VT8);
                    if (!dr.IsDBNull(iVT8)) entity.VT8 = dr.GetDecimal(iVT8);

                    int iVT9 = dr.GetOrdinal(helper.VT9);
                    if (!dr.IsDBNull(iVT9)) entity.VT9 = dr.GetDecimal(iVT9);

                    int iVT10 = dr.GetOrdinal(helper.VT10);
                    if (!dr.IsDBNull(iVT10)) entity.VT10 = dr.GetDecimal(iVT10);

                    int iVT11 = dr.GetOrdinal(helper.VT11);
                    if (!dr.IsDBNull(iVT11)) entity.VT11 = dr.GetDecimal(iVT11);

                    int iVT12 = dr.GetOrdinal(helper.VT12);
                    if (!dr.IsDBNull(iVT12)) entity.VT12 = dr.GetDecimal(iVT12);

                    int iVT13 = dr.GetOrdinal(helper.VT13);
                    if (!dr.IsDBNull(iVT13)) entity.VT13 = dr.GetDecimal(iVT13);

                    int iVT14 = dr.GetOrdinal(helper.VT14);
                    if (!dr.IsDBNull(iVT14)) entity.VT14 = dr.GetDecimal(iVT14);

                    int iVT15 = dr.GetOrdinal(helper.VT15);
                    if (!dr.IsDBNull(iVT15)) entity.VT15 = dr.GetDecimal(iVT15);

                    int iVT16 = dr.GetOrdinal(helper.VT16);
                    if (!dr.IsDBNull(iVT16)) entity.VT16 = dr.GetDecimal(iVT16);

                    int iVT17 = dr.GetOrdinal(helper.VT17);
                    if (!dr.IsDBNull(iVT17)) entity.VT17 = dr.GetDecimal(iVT17);

                    int iVT18 = dr.GetOrdinal(helper.VT18);
                    if (!dr.IsDBNull(iVT18)) entity.VT18 = dr.GetDecimal(iVT18);

                    int iVT19 = dr.GetOrdinal(helper.VT19);
                    if (!dr.IsDBNull(iVT19)) entity.VT19 = dr.GetDecimal(iVT19);

                    int iVT20 = dr.GetOrdinal(helper.VT20);
                    if (!dr.IsDBNull(iVT20)) entity.VT20 = dr.GetDecimal(iVT20);

                    int iVT21 = dr.GetOrdinal(helper.VT21);
                    if (!dr.IsDBNull(iVT21)) entity.VT21 = dr.GetDecimal(iVT21);

                    int iVT22 = dr.GetOrdinal(helper.VT22);
                    if (!dr.IsDBNull(iVT22)) entity.VT22 = dr.GetDecimal(iVT22);

                    int iVT23 = dr.GetOrdinal(helper.VT23);
                    if (!dr.IsDBNull(iVT23)) entity.VT23 = dr.GetDecimal(iVT23);

                    int iVT24 = dr.GetOrdinal(helper.VT24);
                    if (!dr.IsDBNull(iVT24)) entity.VT24 = dr.GetDecimal(iVT24);

                    int iVT25 = dr.GetOrdinal(helper.VT25);
                    if (!dr.IsDBNull(iVT25)) entity.VT25 = dr.GetDecimal(iVT25);

                    int iVT26 = dr.GetOrdinal(helper.VT26);
                    if (!dr.IsDBNull(iVT26)) entity.VT26 = dr.GetDecimal(iVT26);

                    int iVT27 = dr.GetOrdinal(helper.VT27);
                    if (!dr.IsDBNull(iVT27)) entity.VT27 = dr.GetDecimal(iVT27);

                    int iVT28 = dr.GetOrdinal(helper.VT28);
                    if (!dr.IsDBNull(iVT28)) entity.VT28 = dr.GetDecimal(iVT28);

                    int iVT29 = dr.GetOrdinal(helper.VT29);
                    if (!dr.IsDBNull(iVT29)) entity.VT29 = dr.GetDecimal(iVT29);

                    int iVT30 = dr.GetOrdinal(helper.VT30);
                    if (!dr.IsDBNull(iVT30)) entity.VT30 = dr.GetDecimal(iVT30);

                    int iVT31 = dr.GetOrdinal(helper.VT31);
                    if (!dr.IsDBNull(iVT31)) entity.VT31 = dr.GetDecimal(iVT31);

                    int iVT32 = dr.GetOrdinal(helper.VT32);
                    if (!dr.IsDBNull(iVT32)) entity.VT32 = dr.GetDecimal(iVT32);

                    int iVT33 = dr.GetOrdinal(helper.VT33);
                    if (!dr.IsDBNull(iVT33)) entity.VT33 = dr.GetDecimal(iVT33);

                    int iVT34 = dr.GetOrdinal(helper.VT34);
                    if (!dr.IsDBNull(iVT34)) entity.VT34 = dr.GetDecimal(iVT34);

                    int iVT35 = dr.GetOrdinal(helper.VT35);
                    if (!dr.IsDBNull(iVT35)) entity.VT35 = dr.GetDecimal(iVT35);

                    int iVT36 = dr.GetOrdinal(helper.VT36);
                    if (!dr.IsDBNull(iVT36)) entity.VT36 = dr.GetDecimal(iVT36);

                    int iVT37 = dr.GetOrdinal(helper.VT37);
                    if (!dr.IsDBNull(iVT37)) entity.VT37 = dr.GetDecimal(iVT37);

                    int iVT38 = dr.GetOrdinal(helper.VT38);
                    if (!dr.IsDBNull(iVT38)) entity.VT38 = dr.GetDecimal(iVT38);

                    int iVT39 = dr.GetOrdinal(helper.VT39);
                    if (!dr.IsDBNull(iVT39)) entity.VT39 = dr.GetDecimal(iVT39);

                    int iVT40 = dr.GetOrdinal(helper.VT40);
                    if (!dr.IsDBNull(iVT40)) entity.VT40 = dr.GetDecimal(iVT40);

                    int iVT41 = dr.GetOrdinal(helper.VT41);
                    if (!dr.IsDBNull(iVT41)) entity.VT41 = dr.GetDecimal(iVT41);

                    int iVT42 = dr.GetOrdinal(helper.VT42);
                    if (!dr.IsDBNull(iVT42)) entity.VT42 = dr.GetDecimal(iVT42);

                    int iVT43 = dr.GetOrdinal(helper.VT43);
                    if (!dr.IsDBNull(iVT43)) entity.VT43 = dr.GetDecimal(iVT43);

                    int iVT44 = dr.GetOrdinal(helper.VT44);
                    if (!dr.IsDBNull(iVT44)) entity.VT44 = dr.GetDecimal(iVT44);

                    int iVT45 = dr.GetOrdinal(helper.VT45);
                    if (!dr.IsDBNull(iVT45)) entity.VT45 = dr.GetDecimal(iVT45);

                    int iVT46 = dr.GetOrdinal(helper.VT46);
                    if (!dr.IsDBNull(iVT46)) entity.VT46 = dr.GetDecimal(iVT46);

                    int iVT47 = dr.GetOrdinal(helper.VT47);
                    if (!dr.IsDBNull(iVT47)) entity.VT47 = dr.GetDecimal(iVT47);

                    int iVT48 = dr.GetOrdinal(helper.VT48);
                    if (!dr.IsDBNull(iVT48)) entity.VT48 = dr.GetDecimal(iVT48);

                    int iVT49 = dr.GetOrdinal(helper.VT49);
                    if (!dr.IsDBNull(iVT49)) entity.VT49 = dr.GetDecimal(iVT49);

                    int iVT50 = dr.GetOrdinal(helper.VT50);
                    if (!dr.IsDBNull(iVT50)) entity.VT50 = dr.GetDecimal(iVT50);

                    int iVT51 = dr.GetOrdinal(helper.VT51);
                    if (!dr.IsDBNull(iVT51)) entity.VT51 = dr.GetDecimal(iVT51);

                    int iVT52 = dr.GetOrdinal(helper.VT52);
                    if (!dr.IsDBNull(iVT52)) entity.VT52 = dr.GetDecimal(iVT52);

                    int iVT53 = dr.GetOrdinal(helper.VT53);
                    if (!dr.IsDBNull(iVT53)) entity.VT53 = dr.GetDecimal(iVT53);

                    int iVT54 = dr.GetOrdinal(helper.VT54);
                    if (!dr.IsDBNull(iVT54)) entity.VT54 = dr.GetDecimal(iVT54);

                    int iVT55 = dr.GetOrdinal(helper.VT55);
                    if (!dr.IsDBNull(iVT55)) entity.VT55 = dr.GetDecimal(iVT55);

                    int iVT56 = dr.GetOrdinal(helper.VT56);
                    if (!dr.IsDBNull(iVT56)) entity.VT56 = dr.GetDecimal(iVT56);

                    int iVT57 = dr.GetOrdinal(helper.VT57);
                    if (!dr.IsDBNull(iVT57)) entity.VT57 = dr.GetDecimal(iVT57);

                    int iVT58 = dr.GetOrdinal(helper.VT58);
                    if (!dr.IsDBNull(iVT58)) entity.VT58 = dr.GetDecimal(iVT58);

                    int iVT59 = dr.GetOrdinal(helper.VT59);
                    if (!dr.IsDBNull(iVT59)) entity.VT59 = dr.GetDecimal(iVT59);

                    int iVT60 = dr.GetOrdinal(helper.VT60);
                    if (!dr.IsDBNull(iVT60)) entity.VT60 = dr.GetDecimal(iVT60);

                    int iVT61 = dr.GetOrdinal(helper.VT61);
                    if (!dr.IsDBNull(iVT61)) entity.VT61 = dr.GetDecimal(iVT61);

                    int iVT62 = dr.GetOrdinal(helper.VT62);
                    if (!dr.IsDBNull(iVT62)) entity.VT62 = dr.GetDecimal(iVT62);

                    int iVT63 = dr.GetOrdinal(helper.VT63);
                    if (!dr.IsDBNull(iVT63)) entity.VT63 = dr.GetDecimal(iVT63);

                    int iVT64 = dr.GetOrdinal(helper.VT64);
                    if (!dr.IsDBNull(iVT64)) entity.VT64 = dr.GetDecimal(iVT64);

                    int iVT65 = dr.GetOrdinal(helper.VT65);
                    if (!dr.IsDBNull(iVT65)) entity.VT65 = dr.GetDecimal(iVT65);

                    int iVT66 = dr.GetOrdinal(helper.VT66);
                    if (!dr.IsDBNull(iVT66)) entity.VT66 = dr.GetDecimal(iVT66);

                    int iVT67 = dr.GetOrdinal(helper.VT67);
                    if (!dr.IsDBNull(iVT67)) entity.VT67 = dr.GetDecimal(iVT67);

                    int iVT68 = dr.GetOrdinal(helper.VT68);
                    if (!dr.IsDBNull(iVT68)) entity.VT68 = dr.GetDecimal(iVT68);

                    int iVT69 = dr.GetOrdinal(helper.VT69);
                    if (!dr.IsDBNull(iVT60)) entity.VT69 = dr.GetDecimal(iVT69);

                    int iVT70 = dr.GetOrdinal(helper.VT70);
                    if (!dr.IsDBNull(iVT70)) entity.VT70 = dr.GetDecimal(iVT70);

                    int iVT71 = dr.GetOrdinal(helper.VT71);
                    if (!dr.IsDBNull(iVT71)) entity.VT71 = dr.GetDecimal(iVT71);

                    int iVT72 = dr.GetOrdinal(helper.VT72);
                    if (!dr.IsDBNull(iVT72)) entity.VT72 = dr.GetDecimal(iVT72);

                    int iVT73 = dr.GetOrdinal(helper.VT73);
                    if (!dr.IsDBNull(iVT73)) entity.VT73 = dr.GetDecimal(iVT73);

                    int iVT74 = dr.GetOrdinal(helper.VT74);
                    if (!dr.IsDBNull(iVT74)) entity.VT74 = dr.GetDecimal(iVT74);

                    int iVT75 = dr.GetOrdinal(helper.VT75);
                    if (!dr.IsDBNull(iVT75)) entity.VT75 = dr.GetDecimal(iVT75);

                    int iVT76 = dr.GetOrdinal(helper.VT76);
                    if (!dr.IsDBNull(iVT76)) entity.VT76 = dr.GetDecimal(iVT76);

                    int iVT77 = dr.GetOrdinal(helper.VT77);
                    if (!dr.IsDBNull(iVT77)) entity.VT77 = dr.GetDecimal(iVT77);

                    int iVT78 = dr.GetOrdinal(helper.VT78);
                    if (!dr.IsDBNull(iVT78)) entity.VT78 = dr.GetDecimal(iVT78);

                    int iVT79 = dr.GetOrdinal(helper.VT79);
                    if (!dr.IsDBNull(iVT79)) entity.VT79 = dr.GetDecimal(iVT79);

                    int iVT80 = dr.GetOrdinal(helper.VT80);
                    if (!dr.IsDBNull(iVT80)) entity.VT80 = dr.GetDecimal(iVT80);

                    int iVT81 = dr.GetOrdinal(helper.VT81);
                    if (!dr.IsDBNull(iVT81)) entity.VT81 = dr.GetDecimal(iVT81);

                    int iVT82 = dr.GetOrdinal(helper.VT82);
                    if (!dr.IsDBNull(iVT82)) entity.VT82 = dr.GetDecimal(iVT82);

                    int iVT83 = dr.GetOrdinal(helper.VT83);
                    if (!dr.IsDBNull(iVT83)) entity.VT83 = dr.GetDecimal(iVT83);

                    int iVT84 = dr.GetOrdinal(helper.VT84);
                    if (!dr.IsDBNull(iVT84)) entity.VT84 = dr.GetDecimal(iVT84);

                    int iVT85 = dr.GetOrdinal(helper.VT85);
                    if (!dr.IsDBNull(iVT85)) entity.VT85 = dr.GetDecimal(iVT85);

                    int iVT86 = dr.GetOrdinal(helper.VT86);
                    if (!dr.IsDBNull(iVT86)) entity.VT86 = dr.GetDecimal(iVT86);

                    int iVT87 = dr.GetOrdinal(helper.VT87);
                    if (!dr.IsDBNull(iVT87)) entity.VT87 = dr.GetDecimal(iVT87);

                    int iVT88 = dr.GetOrdinal(helper.VT88);
                    if (!dr.IsDBNull(iVT88)) entity.VT88 = dr.GetDecimal(iVT88);

                    int iVT89 = dr.GetOrdinal(helper.VT89);
                    if (!dr.IsDBNull(iVT89)) entity.VT89 = dr.GetDecimal(iVT89);

                    int iVT90 = dr.GetOrdinal(helper.VT90);
                    if (!dr.IsDBNull(iVT90)) entity.VT90 = dr.GetDecimal(iVT90);

                    int iVT91 = dr.GetOrdinal(helper.VT91);
                    if (!dr.IsDBNull(iVT91)) entity.VT91 = dr.GetDecimal(iVT91);

                    int iVT92 = dr.GetOrdinal(helper.VT92);
                    if (!dr.IsDBNull(iVT92)) entity.VT92 = dr.GetDecimal(iVT92);

                    int iVT93 = dr.GetOrdinal(helper.VT93);
                    if (!dr.IsDBNull(iVT93)) entity.VT93 = dr.GetDecimal(iVT93);

                    int iVT94 = dr.GetOrdinal(helper.VT94);
                    if (!dr.IsDBNull(iVT94)) entity.VT94 = dr.GetDecimal(iVT94);

                    int iVT95 = dr.GetOrdinal(helper.VT95);
                    if (!dr.IsDBNull(iVT95)) entity.VT95 = dr.GetDecimal(iVT95);

                    int iVT96 = dr.GetOrdinal(helper.VT96);
                    if (!dr.IsDBNull(iVT96)) entity.VT96 = dr.GetDecimal(iVT96);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<ValorTransferenciaDTO> ListarValorEnergiaEntregaDetalle(int iPeriCodi, int iVTranVersion, int iCodEntCodi)
        {
            List<ValorTransferenciaDTO> entitys = new List<ValorTransferenciaDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarValorEnergiaEntregaDetalle);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, iPeriCodi);
            dbProvider.AddInParameter(command, helper.ValoTranVersion, DbType.Int32, iVTranVersion);
            dbProvider.AddInParameter(command, helper.CodEntCodi, DbType.Int32, iCodEntCodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ValorTransferenciaDTO entity = new ValorTransferenciaDTO();

                    int iValoTranDia = dr.GetOrdinal(helper.ValoTranDia);
                    if (!dr.IsDBNull(iValoTranDia)) entity.ValoTranDia = dr.GetInt32(iValoTranDia);

                    int iVTTotalDia = dr.GetOrdinal(helper.VTTotalDia);
                    if (!dr.IsDBNull(iVTTotalDia)) entity.VTTotalDia = dr.GetDecimal(iVTTotalDia);

                    int iVT1 = dr.GetOrdinal(helper.VT1);
                    if (!dr.IsDBNull(iVT1)) entity.VT1 = dr.GetDecimal(iVT1);

                    int iVT2 = dr.GetOrdinal(helper.VT2);
                    if (!dr.IsDBNull(iVT2)) entity.VT2 = dr.GetDecimal(iVT2);

                    int iVT3 = dr.GetOrdinal(helper.VT3);
                    if (!dr.IsDBNull(iVT3)) entity.VT3 = dr.GetDecimal(iVT3);

                    int iVT4 = dr.GetOrdinal(helper.VT4);
                    if (!dr.IsDBNull(iVT4)) entity.VT4 = dr.GetDecimal(iVT4);

                    int iVT5 = dr.GetOrdinal(helper.VT5);
                    if (!dr.IsDBNull(iVT5)) entity.VT5 = dr.GetDecimal(iVT5);

                    int iVT6 = dr.GetOrdinal(helper.VT6);
                    if (!dr.IsDBNull(iVT6)) entity.VT6 = dr.GetDecimal(iVT6);

                    int iVT7 = dr.GetOrdinal(helper.VT7);
                    if (!dr.IsDBNull(iVT7)) entity.VT7 = dr.GetDecimal(iVT7);

                    int iVT8 = dr.GetOrdinal(helper.VT8);
                    if (!dr.IsDBNull(iVT8)) entity.VT8 = dr.GetDecimal(iVT8);

                    int iVT9 = dr.GetOrdinal(helper.VT9);
                    if (!dr.IsDBNull(iVT9)) entity.VT9 = dr.GetDecimal(iVT9);

                    int iVT10 = dr.GetOrdinal(helper.VT10);
                    if (!dr.IsDBNull(iVT10)) entity.VT10 = dr.GetDecimal(iVT10);

                    int iVT11 = dr.GetOrdinal(helper.VT11);
                    if (!dr.IsDBNull(iVT11)) entity.VT11 = dr.GetDecimal(iVT11);

                    int iVT12 = dr.GetOrdinal(helper.VT12);
                    if (!dr.IsDBNull(iVT12)) entity.VT12 = dr.GetDecimal(iVT12);

                    int iVT13 = dr.GetOrdinal(helper.VT13);
                    if (!dr.IsDBNull(iVT13)) entity.VT13 = dr.GetDecimal(iVT13);

                    int iVT14 = dr.GetOrdinal(helper.VT14);
                    if (!dr.IsDBNull(iVT14)) entity.VT14 = dr.GetDecimal(iVT14);

                    int iVT15 = dr.GetOrdinal(helper.VT15);
                    if (!dr.IsDBNull(iVT15)) entity.VT15 = dr.GetDecimal(iVT15);

                    int iVT16 = dr.GetOrdinal(helper.VT16);
                    if (!dr.IsDBNull(iVT16)) entity.VT16 = dr.GetDecimal(iVT16);

                    int iVT17 = dr.GetOrdinal(helper.VT17);
                    if (!dr.IsDBNull(iVT17)) entity.VT17 = dr.GetDecimal(iVT17);

                    int iVT18 = dr.GetOrdinal(helper.VT18);
                    if (!dr.IsDBNull(iVT18)) entity.VT18 = dr.GetDecimal(iVT18);

                    int iVT19 = dr.GetOrdinal(helper.VT19);
                    if (!dr.IsDBNull(iVT19)) entity.VT19 = dr.GetDecimal(iVT19);

                    int iVT20 = dr.GetOrdinal(helper.VT20);
                    if (!dr.IsDBNull(iVT20)) entity.VT20 = dr.GetDecimal(iVT20);

                    int iVT21 = dr.GetOrdinal(helper.VT21);
                    if (!dr.IsDBNull(iVT21)) entity.VT21 = dr.GetDecimal(iVT21);

                    int iVT22 = dr.GetOrdinal(helper.VT22);
                    if (!dr.IsDBNull(iVT22)) entity.VT22 = dr.GetDecimal(iVT22);

                    int iVT23 = dr.GetOrdinal(helper.VT23);
                    if (!dr.IsDBNull(iVT23)) entity.VT23 = dr.GetDecimal(iVT23);

                    int iVT24 = dr.GetOrdinal(helper.VT24);
                    if (!dr.IsDBNull(iVT24)) entity.VT24 = dr.GetDecimal(iVT24);

                    int iVT25 = dr.GetOrdinal(helper.VT25);
                    if (!dr.IsDBNull(iVT25)) entity.VT25 = dr.GetDecimal(iVT25);

                    int iVT26 = dr.GetOrdinal(helper.VT26);
                    if (!dr.IsDBNull(iVT26)) entity.VT26 = dr.GetDecimal(iVT26);

                    int iVT27 = dr.GetOrdinal(helper.VT27);
                    if (!dr.IsDBNull(iVT27)) entity.VT27 = dr.GetDecimal(iVT27);

                    int iVT28 = dr.GetOrdinal(helper.VT28);
                    if (!dr.IsDBNull(iVT28)) entity.VT28 = dr.GetDecimal(iVT28);

                    int iVT29 = dr.GetOrdinal(helper.VT29);
                    if (!dr.IsDBNull(iVT29)) entity.VT29 = dr.GetDecimal(iVT29);

                    int iVT30 = dr.GetOrdinal(helper.VT30);
                    if (!dr.IsDBNull(iVT30)) entity.VT30 = dr.GetDecimal(iVT30);

                    int iVT31 = dr.GetOrdinal(helper.VT31);
                    if (!dr.IsDBNull(iVT31)) entity.VT31 = dr.GetDecimal(iVT31);

                    int iVT32 = dr.GetOrdinal(helper.VT32);
                    if (!dr.IsDBNull(iVT32)) entity.VT32 = dr.GetDecimal(iVT32);

                    int iVT33 = dr.GetOrdinal(helper.VT33);
                    if (!dr.IsDBNull(iVT33)) entity.VT33 = dr.GetDecimal(iVT33);

                    int iVT34 = dr.GetOrdinal(helper.VT34);
                    if (!dr.IsDBNull(iVT34)) entity.VT34 = dr.GetDecimal(iVT34);

                    int iVT35 = dr.GetOrdinal(helper.VT35);
                    if (!dr.IsDBNull(iVT35)) entity.VT35 = dr.GetDecimal(iVT35);

                    int iVT36 = dr.GetOrdinal(helper.VT36);
                    if (!dr.IsDBNull(iVT36)) entity.VT36 = dr.GetDecimal(iVT36);

                    int iVT37 = dr.GetOrdinal(helper.VT37);
                    if (!dr.IsDBNull(iVT37)) entity.VT37 = dr.GetDecimal(iVT37);

                    int iVT38 = dr.GetOrdinal(helper.VT38);
                    if (!dr.IsDBNull(iVT38)) entity.VT38 = dr.GetDecimal(iVT38);

                    int iVT39 = dr.GetOrdinal(helper.VT39);
                    if (!dr.IsDBNull(iVT39)) entity.VT39 = dr.GetDecimal(iVT39);

                    int iVT40 = dr.GetOrdinal(helper.VT40);
                    if (!dr.IsDBNull(iVT40)) entity.VT40 = dr.GetDecimal(iVT40);

                    int iVT41 = dr.GetOrdinal(helper.VT41);
                    if (!dr.IsDBNull(iVT41)) entity.VT41 = dr.GetDecimal(iVT41);

                    int iVT42 = dr.GetOrdinal(helper.VT42);
                    if (!dr.IsDBNull(iVT42)) entity.VT42 = dr.GetDecimal(iVT42);

                    int iVT43 = dr.GetOrdinal(helper.VT43);
                    if (!dr.IsDBNull(iVT43)) entity.VT43 = dr.GetDecimal(iVT43);

                    int iVT44 = dr.GetOrdinal(helper.VT44);
                    if (!dr.IsDBNull(iVT44)) entity.VT44 = dr.GetDecimal(iVT44);

                    int iVT45 = dr.GetOrdinal(helper.VT45);
                    if (!dr.IsDBNull(iVT45)) entity.VT45 = dr.GetDecimal(iVT45);

                    int iVT46 = dr.GetOrdinal(helper.VT46);
                    if (!dr.IsDBNull(iVT46)) entity.VT46 = dr.GetDecimal(iVT46);

                    int iVT47 = dr.GetOrdinal(helper.VT47);
                    if (!dr.IsDBNull(iVT47)) entity.VT47 = dr.GetDecimal(iVT47);

                    int iVT48 = dr.GetOrdinal(helper.VT48);
                    if (!dr.IsDBNull(iVT48)) entity.VT48 = dr.GetDecimal(iVT48);

                    int iVT49 = dr.GetOrdinal(helper.VT49);
                    if (!dr.IsDBNull(iVT49)) entity.VT49 = dr.GetDecimal(iVT49);

                    int iVT50 = dr.GetOrdinal(helper.VT50);
                    if (!dr.IsDBNull(iVT50)) entity.VT50 = dr.GetDecimal(iVT50);

                    int iVT51 = dr.GetOrdinal(helper.VT51);
                    if (!dr.IsDBNull(iVT51)) entity.VT51 = dr.GetDecimal(iVT51);

                    int iVT52 = dr.GetOrdinal(helper.VT52);
                    if (!dr.IsDBNull(iVT52)) entity.VT52 = dr.GetDecimal(iVT52);

                    int iVT53 = dr.GetOrdinal(helper.VT53);
                    if (!dr.IsDBNull(iVT53)) entity.VT53 = dr.GetDecimal(iVT53);

                    int iVT54 = dr.GetOrdinal(helper.VT54);
                    if (!dr.IsDBNull(iVT54)) entity.VT54 = dr.GetDecimal(iVT54);

                    int iVT55 = dr.GetOrdinal(helper.VT55);
                    if (!dr.IsDBNull(iVT55)) entity.VT55 = dr.GetDecimal(iVT55);

                    int iVT56 = dr.GetOrdinal(helper.VT56);
                    if (!dr.IsDBNull(iVT56)) entity.VT56 = dr.GetDecimal(iVT56);

                    int iVT57 = dr.GetOrdinal(helper.VT57);
                    if (!dr.IsDBNull(iVT57)) entity.VT57 = dr.GetDecimal(iVT57);

                    int iVT58 = dr.GetOrdinal(helper.VT58);
                    if (!dr.IsDBNull(iVT58)) entity.VT58 = dr.GetDecimal(iVT58);

                    int iVT59 = dr.GetOrdinal(helper.VT59);
                    if (!dr.IsDBNull(iVT59)) entity.VT59 = dr.GetDecimal(iVT59);

                    int iVT60 = dr.GetOrdinal(helper.VT60);
                    if (!dr.IsDBNull(iVT60)) entity.VT60 = dr.GetDecimal(iVT60);

                    int iVT61 = dr.GetOrdinal(helper.VT61);
                    if (!dr.IsDBNull(iVT61)) entity.VT61 = dr.GetDecimal(iVT61);

                    int iVT62 = dr.GetOrdinal(helper.VT62);
                    if (!dr.IsDBNull(iVT62)) entity.VT62 = dr.GetDecimal(iVT62);

                    int iVT63 = dr.GetOrdinal(helper.VT63);
                    if (!dr.IsDBNull(iVT63)) entity.VT63 = dr.GetDecimal(iVT63);

                    int iVT64 = dr.GetOrdinal(helper.VT64);
                    if (!dr.IsDBNull(iVT64)) entity.VT64 = dr.GetDecimal(iVT64);

                    int iVT65 = dr.GetOrdinal(helper.VT65);
                    if (!dr.IsDBNull(iVT65)) entity.VT65 = dr.GetDecimal(iVT65);

                    int iVT66 = dr.GetOrdinal(helper.VT66);
                    if (!dr.IsDBNull(iVT66)) entity.VT66 = dr.GetDecimal(iVT66);

                    int iVT67 = dr.GetOrdinal(helper.VT67);
                    if (!dr.IsDBNull(iVT67)) entity.VT67 = dr.GetDecimal(iVT67);

                    int iVT68 = dr.GetOrdinal(helper.VT68);
                    if (!dr.IsDBNull(iVT68)) entity.VT68 = dr.GetDecimal(iVT68);

                    int iVT69 = dr.GetOrdinal(helper.VT69);
                    if (!dr.IsDBNull(iVT60)) entity.VT69 = dr.GetDecimal(iVT69);

                    int iVT70 = dr.GetOrdinal(helper.VT70);
                    if (!dr.IsDBNull(iVT70)) entity.VT70 = dr.GetDecimal(iVT70);

                    int iVT71 = dr.GetOrdinal(helper.VT71);
                    if (!dr.IsDBNull(iVT71)) entity.VT71 = dr.GetDecimal(iVT71);

                    int iVT72 = dr.GetOrdinal(helper.VT72);
                    if (!dr.IsDBNull(iVT72)) entity.VT72 = dr.GetDecimal(iVT72);

                    int iVT73 = dr.GetOrdinal(helper.VT73);
                    if (!dr.IsDBNull(iVT73)) entity.VT73 = dr.GetDecimal(iVT73);

                    int iVT74 = dr.GetOrdinal(helper.VT74);
                    if (!dr.IsDBNull(iVT74)) entity.VT74 = dr.GetDecimal(iVT74);

                    int iVT75 = dr.GetOrdinal(helper.VT75);
                    if (!dr.IsDBNull(iVT75)) entity.VT75 = dr.GetDecimal(iVT75);

                    int iVT76 = dr.GetOrdinal(helper.VT76);
                    if (!dr.IsDBNull(iVT76)) entity.VT76 = dr.GetDecimal(iVT76);

                    int iVT77 = dr.GetOrdinal(helper.VT77);
                    if (!dr.IsDBNull(iVT77)) entity.VT77 = dr.GetDecimal(iVT77);

                    int iVT78 = dr.GetOrdinal(helper.VT78);
                    if (!dr.IsDBNull(iVT78)) entity.VT78 = dr.GetDecimal(iVT78);

                    int iVT79 = dr.GetOrdinal(helper.VT79);
                    if (!dr.IsDBNull(iVT79)) entity.VT79 = dr.GetDecimal(iVT79);

                    int iVT80 = dr.GetOrdinal(helper.VT80);
                    if (!dr.IsDBNull(iVT80)) entity.VT80 = dr.GetDecimal(iVT80);

                    int iVT81 = dr.GetOrdinal(helper.VT81);
                    if (!dr.IsDBNull(iVT81)) entity.VT81 = dr.GetDecimal(iVT81);

                    int iVT82 = dr.GetOrdinal(helper.VT82);
                    if (!dr.IsDBNull(iVT82)) entity.VT82 = dr.GetDecimal(iVT82);

                    int iVT83 = dr.GetOrdinal(helper.VT83);
                    if (!dr.IsDBNull(iVT83)) entity.VT83 = dr.GetDecimal(iVT83);

                    int iVT84 = dr.GetOrdinal(helper.VT84);
                    if (!dr.IsDBNull(iVT84)) entity.VT84 = dr.GetDecimal(iVT84);

                    int iVT85 = dr.GetOrdinal(helper.VT85);
                    if (!dr.IsDBNull(iVT85)) entity.VT85 = dr.GetDecimal(iVT85);

                    int iVT86 = dr.GetOrdinal(helper.VT86);
                    if (!dr.IsDBNull(iVT86)) entity.VT86 = dr.GetDecimal(iVT86);

                    int iVT87 = dr.GetOrdinal(helper.VT87);
                    if (!dr.IsDBNull(iVT87)) entity.VT87 = dr.GetDecimal(iVT87);

                    int iVT88 = dr.GetOrdinal(helper.VT88);
                    if (!dr.IsDBNull(iVT88)) entity.VT88 = dr.GetDecimal(iVT88);

                    int iVT89 = dr.GetOrdinal(helper.VT89);
                    if (!dr.IsDBNull(iVT89)) entity.VT89 = dr.GetDecimal(iVT89);

                    int iVT90 = dr.GetOrdinal(helper.VT90);
                    if (!dr.IsDBNull(iVT90)) entity.VT90 = dr.GetDecimal(iVT90);

                    int iVT91 = dr.GetOrdinal(helper.VT91);
                    if (!dr.IsDBNull(iVT91)) entity.VT91 = dr.GetDecimal(iVT91);

                    int iVT92 = dr.GetOrdinal(helper.VT92);
                    if (!dr.IsDBNull(iVT92)) entity.VT92 = dr.GetDecimal(iVT92);

                    int iVT93 = dr.GetOrdinal(helper.VT93);
                    if (!dr.IsDBNull(iVT93)) entity.VT93 = dr.GetDecimal(iVT93);

                    int iVT94 = dr.GetOrdinal(helper.VT94);
                    if (!dr.IsDBNull(iVT94)) entity.VT94 = dr.GetDecimal(iVT94);

                    int iVT95 = dr.GetOrdinal(helper.VT95);
                    if (!dr.IsDBNull(iVT95)) entity.VT95 = dr.GetDecimal(iVT95);

                    int iVT96 = dr.GetOrdinal(helper.VT96);
                    if (!dr.IsDBNull(iVT96)) entity.VT96 = dr.GetDecimal(iVT96);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<ValorTransferenciaDTO> ListarValorEnergiaRetiroDetalle(int iPeriCodi, int iVTranVersion, string listaCodigosRetiro)
        {
            List<ValorTransferenciaDTO> entitys = new List<ValorTransferenciaDTO>();

            var queryString = string.Format(helper.SqlListarValorEnergiaRetiroDetalle, iPeriCodi, iVTranVersion, listaCodigosRetiro);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ValorTransferenciaDTO entity = new ValorTransferenciaDTO();

                    int iValoTranDia = dr.GetOrdinal(helper.ValoTranDia);
                    if (!dr.IsDBNull(iValoTranDia)) entity.ValoTranDia = dr.GetInt32(iValoTranDia);

                    int iVTTotalDia = dr.GetOrdinal(helper.VTTotalDia);
                    if (!dr.IsDBNull(iVTTotalDia)) entity.VTTotalDia = dr.GetDecimal(iVTTotalDia);

                    int iVT1 = dr.GetOrdinal(helper.VT1);
                    if (!dr.IsDBNull(iVT1)) entity.VT1 = dr.GetDecimal(iVT1);

                    int iVT2 = dr.GetOrdinal(helper.VT2);
                    if (!dr.IsDBNull(iVT2)) entity.VT2 = dr.GetDecimal(iVT2);

                    int iVT3 = dr.GetOrdinal(helper.VT3);
                    if (!dr.IsDBNull(iVT3)) entity.VT3 = dr.GetDecimal(iVT3);

                    int iVT4 = dr.GetOrdinal(helper.VT4);
                    if (!dr.IsDBNull(iVT4)) entity.VT4 = dr.GetDecimal(iVT4);

                    int iVT5 = dr.GetOrdinal(helper.VT5);
                    if (!dr.IsDBNull(iVT5)) entity.VT5 = dr.GetDecimal(iVT5);

                    int iVT6 = dr.GetOrdinal(helper.VT6);
                    if (!dr.IsDBNull(iVT6)) entity.VT6 = dr.GetDecimal(iVT6);

                    int iVT7 = dr.GetOrdinal(helper.VT7);
                    if (!dr.IsDBNull(iVT7)) entity.VT7 = dr.GetDecimal(iVT7);

                    int iVT8 = dr.GetOrdinal(helper.VT8);
                    if (!dr.IsDBNull(iVT8)) entity.VT8 = dr.GetDecimal(iVT8);

                    int iVT9 = dr.GetOrdinal(helper.VT9);
                    if (!dr.IsDBNull(iVT9)) entity.VT9 = dr.GetDecimal(iVT9);

                    int iVT10 = dr.GetOrdinal(helper.VT10);
                    if (!dr.IsDBNull(iVT10)) entity.VT10 = dr.GetDecimal(iVT10);

                    int iVT11 = dr.GetOrdinal(helper.VT11);
                    if (!dr.IsDBNull(iVT11)) entity.VT11 = dr.GetDecimal(iVT11);

                    int iVT12 = dr.GetOrdinal(helper.VT12);
                    if (!dr.IsDBNull(iVT12)) entity.VT12 = dr.GetDecimal(iVT12);

                    int iVT13 = dr.GetOrdinal(helper.VT13);
                    if (!dr.IsDBNull(iVT13)) entity.VT13 = dr.GetDecimal(iVT13);

                    int iVT14 = dr.GetOrdinal(helper.VT14);
                    if (!dr.IsDBNull(iVT14)) entity.VT14 = dr.GetDecimal(iVT14);

                    int iVT15 = dr.GetOrdinal(helper.VT15);
                    if (!dr.IsDBNull(iVT15)) entity.VT15 = dr.GetDecimal(iVT15);

                    int iVT16 = dr.GetOrdinal(helper.VT16);
                    if (!dr.IsDBNull(iVT16)) entity.VT16 = dr.GetDecimal(iVT16);

                    int iVT17 = dr.GetOrdinal(helper.VT17);
                    if (!dr.IsDBNull(iVT17)) entity.VT17 = dr.GetDecimal(iVT17);

                    int iVT18 = dr.GetOrdinal(helper.VT18);
                    if (!dr.IsDBNull(iVT18)) entity.VT18 = dr.GetDecimal(iVT18);

                    int iVT19 = dr.GetOrdinal(helper.VT19);
                    if (!dr.IsDBNull(iVT19)) entity.VT19 = dr.GetDecimal(iVT19);

                    int iVT20 = dr.GetOrdinal(helper.VT20);
                    if (!dr.IsDBNull(iVT20)) entity.VT20 = dr.GetDecimal(iVT20);

                    int iVT21 = dr.GetOrdinal(helper.VT21);
                    if (!dr.IsDBNull(iVT21)) entity.VT21 = dr.GetDecimal(iVT21);

                    int iVT22 = dr.GetOrdinal(helper.VT22);
                    if (!dr.IsDBNull(iVT22)) entity.VT22 = dr.GetDecimal(iVT22);

                    int iVT23 = dr.GetOrdinal(helper.VT23);
                    if (!dr.IsDBNull(iVT23)) entity.VT23 = dr.GetDecimal(iVT23);

                    int iVT24 = dr.GetOrdinal(helper.VT24);
                    if (!dr.IsDBNull(iVT24)) entity.VT24 = dr.GetDecimal(iVT24);

                    int iVT25 = dr.GetOrdinal(helper.VT25);
                    if (!dr.IsDBNull(iVT25)) entity.VT25 = dr.GetDecimal(iVT25);

                    int iVT26 = dr.GetOrdinal(helper.VT26);
                    if (!dr.IsDBNull(iVT26)) entity.VT26 = dr.GetDecimal(iVT26);

                    int iVT27 = dr.GetOrdinal(helper.VT27);
                    if (!dr.IsDBNull(iVT27)) entity.VT27 = dr.GetDecimal(iVT27);

                    int iVT28 = dr.GetOrdinal(helper.VT28);
                    if (!dr.IsDBNull(iVT28)) entity.VT28 = dr.GetDecimal(iVT28);

                    int iVT29 = dr.GetOrdinal(helper.VT29);
                    if (!dr.IsDBNull(iVT29)) entity.VT29 = dr.GetDecimal(iVT29);

                    int iVT30 = dr.GetOrdinal(helper.VT30);
                    if (!dr.IsDBNull(iVT30)) entity.VT30 = dr.GetDecimal(iVT30);

                    int iVT31 = dr.GetOrdinal(helper.VT31);
                    if (!dr.IsDBNull(iVT31)) entity.VT31 = dr.GetDecimal(iVT31);

                    int iVT32 = dr.GetOrdinal(helper.VT32);
                    if (!dr.IsDBNull(iVT32)) entity.VT32 = dr.GetDecimal(iVT32);

                    int iVT33 = dr.GetOrdinal(helper.VT33);
                    if (!dr.IsDBNull(iVT33)) entity.VT33 = dr.GetDecimal(iVT33);

                    int iVT34 = dr.GetOrdinal(helper.VT34);
                    if (!dr.IsDBNull(iVT34)) entity.VT34 = dr.GetDecimal(iVT34);

                    int iVT35 = dr.GetOrdinal(helper.VT35);
                    if (!dr.IsDBNull(iVT35)) entity.VT35 = dr.GetDecimal(iVT35);

                    int iVT36 = dr.GetOrdinal(helper.VT36);
                    if (!dr.IsDBNull(iVT36)) entity.VT36 = dr.GetDecimal(iVT36);

                    int iVT37 = dr.GetOrdinal(helper.VT37);
                    if (!dr.IsDBNull(iVT37)) entity.VT37 = dr.GetDecimal(iVT37);

                    int iVT38 = dr.GetOrdinal(helper.VT38);
                    if (!dr.IsDBNull(iVT38)) entity.VT38 = dr.GetDecimal(iVT38);

                    int iVT39 = dr.GetOrdinal(helper.VT39);
                    if (!dr.IsDBNull(iVT39)) entity.VT39 = dr.GetDecimal(iVT39);

                    int iVT40 = dr.GetOrdinal(helper.VT40);
                    if (!dr.IsDBNull(iVT40)) entity.VT40 = dr.GetDecimal(iVT40);

                    int iVT41 = dr.GetOrdinal(helper.VT41);
                    if (!dr.IsDBNull(iVT41)) entity.VT41 = dr.GetDecimal(iVT41);

                    int iVT42 = dr.GetOrdinal(helper.VT42);
                    if (!dr.IsDBNull(iVT42)) entity.VT42 = dr.GetDecimal(iVT42);

                    int iVT43 = dr.GetOrdinal(helper.VT43);
                    if (!dr.IsDBNull(iVT43)) entity.VT43 = dr.GetDecimal(iVT43);

                    int iVT44 = dr.GetOrdinal(helper.VT44);
                    if (!dr.IsDBNull(iVT44)) entity.VT44 = dr.GetDecimal(iVT44);

                    int iVT45 = dr.GetOrdinal(helper.VT45);
                    if (!dr.IsDBNull(iVT45)) entity.VT45 = dr.GetDecimal(iVT45);

                    int iVT46 = dr.GetOrdinal(helper.VT46);
                    if (!dr.IsDBNull(iVT46)) entity.VT46 = dr.GetDecimal(iVT46);

                    int iVT47 = dr.GetOrdinal(helper.VT47);
                    if (!dr.IsDBNull(iVT47)) entity.VT47 = dr.GetDecimal(iVT47);

                    int iVT48 = dr.GetOrdinal(helper.VT48);
                    if (!dr.IsDBNull(iVT48)) entity.VT48 = dr.GetDecimal(iVT48);

                    int iVT49 = dr.GetOrdinal(helper.VT49);
                    if (!dr.IsDBNull(iVT49)) entity.VT49 = dr.GetDecimal(iVT49);

                    int iVT50 = dr.GetOrdinal(helper.VT50);
                    if (!dr.IsDBNull(iVT50)) entity.VT50 = dr.GetDecimal(iVT50);

                    int iVT51 = dr.GetOrdinal(helper.VT51);
                    if (!dr.IsDBNull(iVT51)) entity.VT51 = dr.GetDecimal(iVT51);

                    int iVT52 = dr.GetOrdinal(helper.VT52);
                    if (!dr.IsDBNull(iVT52)) entity.VT52 = dr.GetDecimal(iVT52);

                    int iVT53 = dr.GetOrdinal(helper.VT53);
                    if (!dr.IsDBNull(iVT53)) entity.VT53 = dr.GetDecimal(iVT53);

                    int iVT54 = dr.GetOrdinal(helper.VT54);
                    if (!dr.IsDBNull(iVT54)) entity.VT54 = dr.GetDecimal(iVT54);

                    int iVT55 = dr.GetOrdinal(helper.VT55);
                    if (!dr.IsDBNull(iVT55)) entity.VT55 = dr.GetDecimal(iVT55);

                    int iVT56 = dr.GetOrdinal(helper.VT56);
                    if (!dr.IsDBNull(iVT56)) entity.VT56 = dr.GetDecimal(iVT56);

                    int iVT57 = dr.GetOrdinal(helper.VT57);
                    if (!dr.IsDBNull(iVT57)) entity.VT57 = dr.GetDecimal(iVT57);

                    int iVT58 = dr.GetOrdinal(helper.VT58);
                    if (!dr.IsDBNull(iVT58)) entity.VT58 = dr.GetDecimal(iVT58);

                    int iVT59 = dr.GetOrdinal(helper.VT59);
                    if (!dr.IsDBNull(iVT59)) entity.VT59 = dr.GetDecimal(iVT59);

                    int iVT60 = dr.GetOrdinal(helper.VT60);
                    if (!dr.IsDBNull(iVT60)) entity.VT60 = dr.GetDecimal(iVT60);

                    int iVT61 = dr.GetOrdinal(helper.VT61);
                    if (!dr.IsDBNull(iVT61)) entity.VT61 = dr.GetDecimal(iVT61);

                    int iVT62 = dr.GetOrdinal(helper.VT62);
                    if (!dr.IsDBNull(iVT62)) entity.VT62 = dr.GetDecimal(iVT62);

                    int iVT63 = dr.GetOrdinal(helper.VT63);
                    if (!dr.IsDBNull(iVT63)) entity.VT63 = dr.GetDecimal(iVT63);

                    int iVT64 = dr.GetOrdinal(helper.VT64);
                    if (!dr.IsDBNull(iVT64)) entity.VT64 = dr.GetDecimal(iVT64);

                    int iVT65 = dr.GetOrdinal(helper.VT65);
                    if (!dr.IsDBNull(iVT65)) entity.VT65 = dr.GetDecimal(iVT65);

                    int iVT66 = dr.GetOrdinal(helper.VT66);
                    if (!dr.IsDBNull(iVT66)) entity.VT66 = dr.GetDecimal(iVT66);

                    int iVT67 = dr.GetOrdinal(helper.VT67);
                    if (!dr.IsDBNull(iVT67)) entity.VT67 = dr.GetDecimal(iVT67);

                    int iVT68 = dr.GetOrdinal(helper.VT68);
                    if (!dr.IsDBNull(iVT68)) entity.VT68 = dr.GetDecimal(iVT68);

                    int iVT69 = dr.GetOrdinal(helper.VT69);
                    if (!dr.IsDBNull(iVT60)) entity.VT69 = dr.GetDecimal(iVT69);

                    int iVT70 = dr.GetOrdinal(helper.VT70);
                    if (!dr.IsDBNull(iVT70)) entity.VT70 = dr.GetDecimal(iVT70);

                    int iVT71 = dr.GetOrdinal(helper.VT71);
                    if (!dr.IsDBNull(iVT71)) entity.VT71 = dr.GetDecimal(iVT71);

                    int iVT72 = dr.GetOrdinal(helper.VT72);
                    if (!dr.IsDBNull(iVT72)) entity.VT72 = dr.GetDecimal(iVT72);

                    int iVT73 = dr.GetOrdinal(helper.VT73);
                    if (!dr.IsDBNull(iVT73)) entity.VT73 = dr.GetDecimal(iVT73);

                    int iVT74 = dr.GetOrdinal(helper.VT74);
                    if (!dr.IsDBNull(iVT74)) entity.VT74 = dr.GetDecimal(iVT74);

                    int iVT75 = dr.GetOrdinal(helper.VT75);
                    if (!dr.IsDBNull(iVT75)) entity.VT75 = dr.GetDecimal(iVT75);

                    int iVT76 = dr.GetOrdinal(helper.VT76);
                    if (!dr.IsDBNull(iVT76)) entity.VT76 = dr.GetDecimal(iVT76);

                    int iVT77 = dr.GetOrdinal(helper.VT77);
                    if (!dr.IsDBNull(iVT77)) entity.VT77 = dr.GetDecimal(iVT77);

                    int iVT78 = dr.GetOrdinal(helper.VT78);
                    if (!dr.IsDBNull(iVT78)) entity.VT78 = dr.GetDecimal(iVT78);

                    int iVT79 = dr.GetOrdinal(helper.VT79);
                    if (!dr.IsDBNull(iVT79)) entity.VT79 = dr.GetDecimal(iVT79);

                    int iVT80 = dr.GetOrdinal(helper.VT80);
                    if (!dr.IsDBNull(iVT80)) entity.VT80 = dr.GetDecimal(iVT80);

                    int iVT81 = dr.GetOrdinal(helper.VT81);
                    if (!dr.IsDBNull(iVT81)) entity.VT81 = dr.GetDecimal(iVT81);

                    int iVT82 = dr.GetOrdinal(helper.VT82);
                    if (!dr.IsDBNull(iVT82)) entity.VT82 = dr.GetDecimal(iVT82);

                    int iVT83 = dr.GetOrdinal(helper.VT83);
                    if (!dr.IsDBNull(iVT83)) entity.VT83 = dr.GetDecimal(iVT83);

                    int iVT84 = dr.GetOrdinal(helper.VT84);
                    if (!dr.IsDBNull(iVT84)) entity.VT84 = dr.GetDecimal(iVT84);

                    int iVT85 = dr.GetOrdinal(helper.VT85);
                    if (!dr.IsDBNull(iVT85)) entity.VT85 = dr.GetDecimal(iVT85);

                    int iVT86 = dr.GetOrdinal(helper.VT86);
                    if (!dr.IsDBNull(iVT86)) entity.VT86 = dr.GetDecimal(iVT86);

                    int iVT87 = dr.GetOrdinal(helper.VT87);
                    if (!dr.IsDBNull(iVT87)) entity.VT87 = dr.GetDecimal(iVT87);

                    int iVT88 = dr.GetOrdinal(helper.VT88);
                    if (!dr.IsDBNull(iVT88)) entity.VT88 = dr.GetDecimal(iVT88);

                    int iVT89 = dr.GetOrdinal(helper.VT89);
                    if (!dr.IsDBNull(iVT89)) entity.VT89 = dr.GetDecimal(iVT89);

                    int iVT90 = dr.GetOrdinal(helper.VT90);
                    if (!dr.IsDBNull(iVT90)) entity.VT90 = dr.GetDecimal(iVT90);

                    int iVT91 = dr.GetOrdinal(helper.VT91);
                    if (!dr.IsDBNull(iVT91)) entity.VT91 = dr.GetDecimal(iVT91);

                    int iVT92 = dr.GetOrdinal(helper.VT92);
                    if (!dr.IsDBNull(iVT92)) entity.VT92 = dr.GetDecimal(iVT92);

                    int iVT93 = dr.GetOrdinal(helper.VT93);
                    if (!dr.IsDBNull(iVT93)) entity.VT93 = dr.GetDecimal(iVT93);

                    int iVT94 = dr.GetOrdinal(helper.VT94);
                    if (!dr.IsDBNull(iVT94)) entity.VT94 = dr.GetDecimal(iVT94);

                    int iVT95 = dr.GetOrdinal(helper.VT95);
                    if (!dr.IsDBNull(iVT95)) entity.VT95 = dr.GetDecimal(iVT95);

                    int iVT96 = dr.GetOrdinal(helper.VT96);
                    if (!dr.IsDBNull(iVT96)) entity.VT96 = dr.GetDecimal(iVT96);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

    }
}
