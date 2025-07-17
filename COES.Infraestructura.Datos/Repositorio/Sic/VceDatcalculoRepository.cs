using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase que contiene las operaciones con la base de datos
    /// </summary>
    public class VceDatcalculoRepository : RepositoryBase, IVceDatcalculoRepository
    {
        public VceDatcalculoRepository(string strConn) : base(strConn)
        {
        }

        VceDatcalculoHelper helper = new VceDatcalculoHelper();

        public void Save(VceDatcalculoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.CrdcgcmarrDol, DbType.Decimal, entity.Crdcgcmarr_dol);
            dbProvider.AddInParameter(command, helper.CrdcgcmarrSol, DbType.Decimal, entity.Crdcgcmarr_sol);
            dbProvider.AddInParameter(command, helper.Crdcgccbefparrampa, DbType.Decimal, entity.Crdcgccbefparrampa);
            dbProvider.AddInParameter(command, helper.Crdcgccbefpar, DbType.Decimal, entity.Crdcgccbefpar);
            dbProvider.AddInParameter(command, helper.Crdcgccbefarrtoma, DbType.Decimal, entity.Crdcgccbefarrtoma);
            dbProvider.AddInParameter(command, helper.Crdcgccbefarr, DbType.Decimal, entity.Crdcgccbefarr);
            dbProvider.AddInParameter(command, helper.Crdcgpotmin, DbType.Decimal, entity.Crdcgpotmin);
            dbProvider.AddInParameter(command, helper.Crdcgconcompp4, DbType.Decimal, entity.Crdcgconcompp4);
            dbProvider.AddInParameter(command, helper.Crdcgpotpar4, DbType.Decimal, entity.Crdcgpotpar4);
            dbProvider.AddInParameter(command, helper.Crdcgconcompp3, DbType.Decimal, entity.Crdcgconcompp3);
            dbProvider.AddInParameter(command, helper.Crdcgpotpar3, DbType.Decimal, entity.Crdcgpotpar3);
            dbProvider.AddInParameter(command, helper.Crdcgconcompp2, DbType.Decimal, entity.Crdcgconcompp2);
            dbProvider.AddInParameter(command, helper.Crdcgpotpar2, DbType.Decimal, entity.Crdcgpotpar2);
            dbProvider.AddInParameter(command, helper.Crdcgconcompp1, DbType.Decimal, entity.Crdcgconcompp1);
            dbProvider.AddInParameter(command, helper.Crdcgpotpar1, DbType.Decimal, entity.Crdcgpotpar1);
            dbProvider.AddInParameter(command, helper.Crdcgccpotefe, DbType.Decimal, entity.Crdcgccpotefe);
            dbProvider.AddInParameter(command, helper.Crdcgpotefe, DbType.Decimal, entity.Crdcgpotefe);
            dbProvider.AddInParameter(command, helper.Crdcgnumarrpar, DbType.Decimal, entity.Crdcgnumarrpar);
            dbProvider.AddInParameter(command, helper.Crdcgprecioaplicunid, DbType.String, entity.Crdcgprecioaplicunid);
            dbProvider.AddInParameter(command, helper.Crdcgprecioaplic, DbType.Decimal, entity.Crdcgprecioaplic);
            dbProvider.AddInParameter(command, helper.Crdcgprecombunid, DbType.String, entity.Crdcgprecombunid);
            dbProvider.AddInParameter(command, helper.Crdcgprecomb, DbType.Decimal, entity.Crdcgprecomb);
            dbProvider.AddInParameter(command, helper.Crdcgcvncsol, DbType.Decimal, entity.Crdcgcvncsol);
            dbProvider.AddInParameter(command, helper.Crdcgcvncdol, DbType.Decimal, entity.Crdcgcvncdol);
            dbProvider.AddInParameter(command, helper.Crdcgtratquim, DbType.Decimal, entity.Crdcgtratquim);
            dbProvider.AddInParameter(command, helper.Crdcgtratmec, DbType.Decimal, entity.Crdcgtratmec);
            dbProvider.AddInParameter(command, helper.Crdcgtranspor, DbType.Decimal, entity.Crdcgtranspor);
            dbProvider.AddInParameter(command, helper.Crdcglhv, DbType.Decimal, entity.Crdcglhv);
            dbProvider.AddInParameter(command, helper.Crdcgtipcom, DbType.String, entity.Crdcgtipcom);
            dbProvider.AddInParameter(command, helper.Crdcgfecmod, DbType.DateTime, entity.Crdcgfecmod);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Pecacodi, DbType.Int32, entity.PecaCodi);
            dbProvider.AddInParameter(command, helper.Crdcgdiasfinanc, DbType.Int32, entity.Crdcgdiasfinanc);
            dbProvider.AddInParameter(command, helper.Crdcgtiempo, DbType.Decimal, entity.Crdcgtiempo);
            dbProvider.AddInParameter(command, helper.Crdcgenergia, DbType.Decimal, entity.Crdcgenergia);
            dbProvider.AddInParameter(command, helper.Crdcgconsiderapotnom, DbType.Int32, entity.Crdcgconsiderapotnom);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, entity.Barrcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(VceDatcalculoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.CrdcgcmarrDol, DbType.Decimal, entity.Crdcgcmarr_dol);
            dbProvider.AddInParameter(command, helper.CrdcgcmarrSol, DbType.Decimal, entity.Crdcgcmarr_sol);
            dbProvider.AddInParameter(command, helper.Crdcgccbefparrampa, DbType.Decimal, entity.Crdcgccbefparrampa);
            dbProvider.AddInParameter(command, helper.Crdcgccbefpar, DbType.Decimal, entity.Crdcgccbefpar);
            dbProvider.AddInParameter(command, helper.Crdcgccbefarrtoma, DbType.Decimal, entity.Crdcgccbefarrtoma);
            dbProvider.AddInParameter(command, helper.Crdcgccbefarr, DbType.Decimal, entity.Crdcgccbefarr);
            dbProvider.AddInParameter(command, helper.Crdcgpotmin, DbType.Decimal, entity.Crdcgpotmin);
            dbProvider.AddInParameter(command, helper.Crdcgconcompp4, DbType.Decimal, entity.Crdcgconcompp4);
            dbProvider.AddInParameter(command, helper.Crdcgpotpar4, DbType.Decimal, entity.Crdcgpotpar4);
            dbProvider.AddInParameter(command, helper.Crdcgconcompp3, DbType.Decimal, entity.Crdcgconcompp3);
            dbProvider.AddInParameter(command, helper.Crdcgpotpar3, DbType.Decimal, entity.Crdcgpotpar3);
            dbProvider.AddInParameter(command, helper.Crdcgconcompp2, DbType.Decimal, entity.Crdcgconcompp2);
            dbProvider.AddInParameter(command, helper.Crdcgpotpar2, DbType.Decimal, entity.Crdcgpotpar2);
            dbProvider.AddInParameter(command, helper.Crdcgconcompp1, DbType.Decimal, entity.Crdcgconcompp1);
            dbProvider.AddInParameter(command, helper.Crdcgpotpar1, DbType.Decimal, entity.Crdcgpotpar1);
            dbProvider.AddInParameter(command, helper.Crdcgccpotefe, DbType.Decimal, entity.Crdcgccpotefe);
            dbProvider.AddInParameter(command, helper.Crdcgpotefe, DbType.Decimal, entity.Crdcgpotefe);
            dbProvider.AddInParameter(command, helper.Crdcgnumarrpar, DbType.Decimal, entity.Crdcgnumarrpar);
            dbProvider.AddInParameter(command, helper.Crdcgprecioaplicunid, DbType.String, entity.Crdcgprecioaplicunid);
            dbProvider.AddInParameter(command, helper.Crdcgprecioaplic, DbType.Decimal, entity.Crdcgprecioaplic);
            dbProvider.AddInParameter(command, helper.Crdcgprecombunid, DbType.String, entity.Crdcgprecombunid);
            dbProvider.AddInParameter(command, helper.Crdcgprecomb, DbType.Decimal, entity.Crdcgprecomb);
            dbProvider.AddInParameter(command, helper.Crdcgcvncsol, DbType.Decimal, entity.Crdcgcvncsol);
            dbProvider.AddInParameter(command, helper.Crdcgcvncdol, DbType.Decimal, entity.Crdcgcvncdol);
            dbProvider.AddInParameter(command, helper.Crdcgtratquim, DbType.Decimal, entity.Crdcgtratquim);
            dbProvider.AddInParameter(command, helper.Crdcgtratmec, DbType.Decimal, entity.Crdcgtratmec);
            dbProvider.AddInParameter(command, helper.Crdcgtranspor, DbType.Decimal, entity.Crdcgtranspor);
            dbProvider.AddInParameter(command, helper.Crdcglhv, DbType.Decimal, entity.Crdcglhv);
            dbProvider.AddInParameter(command, helper.Crdcgtipcom, DbType.String, entity.Crdcgtipcom);
            dbProvider.AddInParameter(command, helper.Crdcgdiasfinanc, DbType.Int32, entity.Crdcgdiasfinanc);
            dbProvider.AddInParameter(command, helper.Crdcgtiempo, DbType.Decimal, entity.Crdcgtiempo);
            dbProvider.AddInParameter(command, helper.Crdcgenergia, DbType.Decimal, entity.Crdcgenergia);
            dbProvider.AddInParameter(command, helper.Crdcgconsiderapotnom, DbType.Int32, entity.Crdcgconsiderapotnom);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, entity.Barrcodi);
            //- compensaciones.HDT - Inicio 03/03/2017: Cambio para atender el requerimiento.
            dbProvider.AddInParameter(command, helper.Crdcgconspotefearr, DbType.Decimal, entity.Crdcgconspotefearr);
            dbProvider.AddInParameter(command, helper.Crdcgconspotefepar, DbType.Decimal, entity.Crdcgconspotefepar);
            dbProvider.AddInParameter(command, helper.Crdcgprecioaplicxarr, DbType.Decimal, entity.Crdcgprecioaplicxarr);
            dbProvider.AddInParameter(command, helper.Crdcgprecioaplicxpar, DbType.Decimal, entity.Crdcgprecioaplicxpar);
            dbProvider.AddInParameter(command, helper.Crdcgprecioaplicxincgen, DbType.Decimal, entity.Crdcgprecioaplicxincgen);
            dbProvider.AddInParameter(command, helper.Crdcgprecioaplicxdisgen, DbType.Decimal, entity.Crdcgprecioaplicxdisgen);
            //- HDT Fin
            dbProvider.AddInParameter(command, helper.Crdcgfecmod, DbType.DateTime, entity.Crdcgfecmod);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Pecacodi, DbType.Int32, entity.PecaCodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(DateTime crdcgfecmod, int grupocodi, int pecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Crdcgfecmod, DbType.DateTime, crdcgfecmod);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);
            dbProvider.AddInParameter(command, helper.Pecacodi, DbType.Int32, pecacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public VceDatcalculoDTO GetById(DateTime crdcgfecmod, int grupocodi, int pecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Crdcgfecmod, DbType.DateTime, crdcgfecmod);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);
            dbProvider.AddInParameter(command, helper.Pecacodi, DbType.Int32, pecacodi);
            VceDatcalculoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VceDatcalculoDTO> List()
        {
            List<VceDatcalculoDTO> entitys = new List<VceDatcalculoDTO>();
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

        public List<VceDatcalculoDTO> GetByCriteria()
        {
            List<VceDatcalculoDTO> entitys = new List<VceDatcalculoDTO>();
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

        //NETC
        //DSH 01-06-2017 : Se cambio por requerimiento
        public List<VceDatcalculoDTO> ListByFiltro(int pecacodi, string empresa, string central, string grupo, string modo)
        {
            string condicion = "";

            condicion = condicion + " DC.pecacodi = " + pecacodi;

            //- compensaciones.HDT - Inicio 27/02/2017: Se ha incluido el filtro por empresa.
            //- Originalmente este código no estaba incluido porque faltaba el JOIN con la tabla SI_EMPRESA.
            //- Ahora ya se ha incluido en el archivo VceDatcalculoSQL.xml.
            if (!empresa.Equals("") && empresa != null)
            {
                condicion = condicion + " AND EMP.EMPRCODI = " + empresa;
            }
            //- HDT Fin

            if (!central.Equals("") && central != null)
            {
                condicion = condicion + " AND CG.EQUICODI = " + central;
            }

            if (!grupo.Equals("") && grupo != null)
            {
                condicion = condicion + " AND GG.GRUPOCODI = " + grupo;
            }

            if (!modo.Equals("") && modo != null)
            {
                condicion = condicion + " AND MO.GRUPOCODI = " + modo;
            }

            List<VceDatcalculoDTO> entitys = new List<VceDatcalculoDTO>();
            string queryString = string.Format(helper.SqlListByFiltro, condicion);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VceDatcalculoDTO entity = new VceDatcalculoDTO();

                    int ipecacodi = dr.GetOrdinal(helper.Pecacodi);
                    if (!dr.IsDBNull(ipecacodi)) entity.PecaCodi = dr.GetInt32(ipecacodi);

                    int iGrupoCodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupoCodi)) entity.Grupocodi = dr.GetInt32(iGrupoCodi);

                    int iGrupoNomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGrupoNomb)) entity.Gruponomb = dr.GetString(iGrupoNomb);

                    int iFEnergNomb = dr.GetOrdinal(helper.Fenergnomb);
                    if (!dr.IsDBNull(iFEnergNomb)) entity.Fenergnomb = dr.GetString(iFEnergNomb);

                    int iCrdcgTipCom = dr.GetOrdinal(helper.Crdcgtipcom);
                    if (!dr.IsDBNull(iCrdcgTipCom)) entity.Crdcgtipcom = dr.GetString(iCrdcgTipCom);

                    int iBarraDiaPer = dr.GetOrdinal(helper.Barradiaper);
                    if (!dr.IsDBNull(iBarraDiaPer)) entity.Barradiaper = dr.GetString(iBarraDiaPer);

                    int iConsiderarPotNominal = dr.GetOrdinal(helper.Considerarpotnominal);
                    if (!dr.IsDBNull(iConsiderarPotNominal)) entity.Considerarpotnominal = dr.GetString(iConsiderarPotNominal);

                    int iCrdcgLhv = dr.GetOrdinal(helper.Crdcglhv);
                    if (!dr.IsDBNull(iCrdcgLhv)) entity.Crdcglhv = dr.GetDecimal(iCrdcgLhv);

                    int iCrdcgTranspor = dr.GetOrdinal(helper.Crdcgtranspor);
                    if (!dr.IsDBNull(iCrdcgTranspor)) entity.Crdcgtranspor = dr.GetDecimal(iCrdcgTranspor);

                    int iCrdcgTratMec = dr.GetOrdinal(helper.Crdcgtratmec);
                    if (!dr.IsDBNull(iCrdcgTratMec)) entity.Crdcgtratmec = dr.GetDecimal(iCrdcgTratMec);

                    int iCrdcgTratQuim = dr.GetOrdinal(helper.Crdcgtratquim);
                    if (!dr.IsDBNull(iCrdcgTratQuim)) entity.Crdcgtratquim = dr.GetDecimal(iCrdcgTratQuim);

                    int iCrdcgDiaCosFin = dr.GetOrdinal(helper.Crdcgdiasfinanc);
                    if (!dr.IsDBNull(iCrdcgDiaCosFin)) entity.Crdcgdiasfinanc = dr.GetInt32(iCrdcgDiaCosFin);

                    int iCrdcgCVNCDol = dr.GetOrdinal(helper.Crdcgcvncdol);
                    if (!dr.IsDBNull(iCrdcgCVNCDol)) entity.Crdcgcvncdol = dr.GetDecimal(iCrdcgCVNCDol);

                    int iCrdcgCVNCSol = dr.GetOrdinal(helper.Crdcgcvncsol);
                    if (!dr.IsDBNull(iCrdcgCVNCSol)) entity.Crdcgcvncsol = dr.GetDecimal(iCrdcgCVNCSol);

                    int iCrdcgPreComb = dr.GetOrdinal(helper.Crdcgprecomb);
                    if (!dr.IsDBNull(iCrdcgPreComb)) entity.Crdcgprecomb = dr.GetDecimal(iCrdcgPreComb);

                    int iCrdcgpPreCombUnid = dr.GetOrdinal(helper.Crdcgprecombunid);
                    if (!dr.IsDBNull(iCrdcgpPreCombUnid)) entity.Crdcgprecombunid = dr.GetString(iCrdcgpPreCombUnid);

                    int iVceDCMEnergia = dr.GetOrdinal(helper.VceDCMEnergia);
                    if (!dr.IsDBNull(iVceDCMEnergia)) entity.Vcedcmenergia = dr.GetDecimal(iVceDCMEnergia);

                    int iVceDCMTiempo = dr.GetOrdinal(helper.VceDCMTiempo);
                    if (!dr.IsDBNull(iVceDCMTiempo)) entity.Vcedcmtiempo = dr.GetDecimal(iVceDCMTiempo);

                    int iCrdcgNumArrPar = dr.GetOrdinal(helper.Crdcgnumarrpar);
                    if (!dr.IsDBNull(iCrdcgNumArrPar)) entity.Crdcgnumarrpar = dr.GetDecimal(iCrdcgNumArrPar);

                    int iCrdcgPotEfe = dr.GetOrdinal(helper.Crdcgpotefe);
                    if (!dr.IsDBNull(iCrdcgPotEfe)) entity.Crdcgpotefe = dr.GetDecimal(iCrdcgPotEfe);

                    int iCrdcgCCPotEfe = dr.GetOrdinal(helper.Crdcgccpotefe);
                    if (!dr.IsDBNull(iCrdcgCCPotEfe)) entity.Crdcgccpotefe = dr.GetDecimal(iCrdcgCCPotEfe);

                    int iCrdcgPotPar1 = dr.GetOrdinal(helper.Crdcgpotpar1);
                    if (!dr.IsDBNull(iCrdcgPotPar1)) entity.Crdcgpotpar1 = dr.GetDecimal(iCrdcgPotPar1);

                    int iCrdcgConCompp1 = dr.GetOrdinal(helper.Crdcgconcompp1);
                    if (!dr.IsDBNull(iCrdcgConCompp1)) entity.Crdcgconcompp1 = dr.GetDecimal(iCrdcgConCompp1);

                    int iCrdcgPotPar2 = dr.GetOrdinal(helper.Crdcgpotpar2);
                    if (!dr.IsDBNull(iCrdcgPotPar2)) entity.Crdcgpotpar2 = dr.GetDecimal(iCrdcgPotPar2);

                    int iCrdcgConCompp2 = dr.GetOrdinal(helper.Crdcgconcompp2);
                    if (!dr.IsDBNull(iCrdcgConCompp2)) entity.Crdcgconcompp2 = dr.GetDecimal(iCrdcgConCompp2);

                    int iCrdcgPotPar3 = dr.GetOrdinal(helper.Crdcgpotpar3);
                    if (!dr.IsDBNull(iCrdcgPotPar3)) entity.Crdcgpotpar3 = dr.GetDecimal(iCrdcgPotPar3);

                    int iCrdcgConCompp3 = dr.GetOrdinal(helper.Crdcgconcompp3);
                    if (!dr.IsDBNull(iCrdcgConCompp3)) entity.Crdcgconcompp3 = dr.GetDecimal(iCrdcgConCompp3);

                    int iCrdcgPotPar4 = dr.GetOrdinal(helper.Crdcgpotpar4);
                    if (!dr.IsDBNull(iCrdcgPotPar4)) entity.Crdcgpotpar4 = dr.GetDecimal(iCrdcgPotPar4);

                    int iCrdcgConCompp4 = dr.GetOrdinal(helper.Crdcgconcompp4);
                    if (!dr.IsDBNull(iCrdcgConCompp4)) entity.Crdcgconcompp4 = dr.GetDecimal(iCrdcgConCompp4);

                    int iCrdcgPotMin = dr.GetOrdinal(helper.Crdcgpotmin);
                    if (!dr.IsDBNull(iCrdcgPotMin)) entity.Crdcgpotmin = dr.GetDecimal(iCrdcgPotMin);

                    int iEdit = dr.GetOrdinal(helper.Edit);
                    if (!dr.IsDBNull(iEdit)) entity.Edit = dr.GetInt32(iEdit);

                    //- compensaciones.HDT - Inicio 27/02/2017: Cambio para atender el requerimiento.
                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);
                    //- HDT Fin


                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public VceDatcalculoDTO GetByIdGrupo(int pecacodi, int grupocodi)
        {
            string queryString = string.Format(helper.SqlGetByIdGrupo, pecacodi, grupocodi);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            VceDatcalculoDTO entity = new VceDatcalculoDTO();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    int ipecacodi = dr.GetOrdinal(helper.Pecacodi);
                    if (!dr.IsDBNull(ipecacodi)) entity.PecaCodi = dr.GetInt32(ipecacodi);

                    int iBarrCodi = dr.GetOrdinal(helper.Barrcodi);
                    if (!dr.IsDBNull(iBarrCodi)) entity.Barrcodi = dr.GetInt32(iBarrCodi);

                    int iConsiderarPotNominal = dr.GetOrdinal(helper.Considerarpotnominal);
                    if (!dr.IsDBNull(iConsiderarPotNominal)) entity.Considerarpotnominal = dr.GetString(iConsiderarPotNominal);

                    int iVceDCMEnergia = dr.GetOrdinal(helper.VceDCMEnergia);
                    if (!dr.IsDBNull(iVceDCMEnergia)) entity.Vcedcmenergia = dr.GetDecimal(iVceDCMEnergia);

                    int iVceDCMTiempo = dr.GetOrdinal(helper.VceDCMTiempo);
                    if (!dr.IsDBNull(iVceDCMTiempo)) entity.Vcedcmtiempo = dr.GetDecimal(iVceDCMTiempo);

                    int iGrupoNomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGrupoNomb)) entity.Gruponomb = dr.GetString(iGrupoNomb);

                    int iPeriodo = dr.GetOrdinal(helper.Periodo);
                    if (!dr.IsDBNull(iPeriodo)) entity.Periodo = dr.GetString(iPeriodo);

                }
            }
            return entity;
        }

        public void UpdateCalculo(VceDatcalculoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateCalculo);

            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, entity.Barrcodi);
            dbProvider.AddInParameter(command, helper.VceDCMConsideraPotNom, DbType.String, entity.Vcedcmconsiderapotnom);
            dbProvider.AddInParameter(command, helper.VceDCMEnergia, DbType.Decimal, entity.Vcedcmenergia);
            dbProvider.AddInParameter(command, helper.VceDCMTiempo, DbType.Decimal, entity.Vcedcmtiempo);
            dbProvider.AddInParameter(command, helper.Pecacodi, DbType.Int32, entity.PecaCodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.ExecuteNonQuery(command);
        }

        //- compensaciones.HDT - 03/03/2017: Cambio para atender el requerimiento. 
        public void ConfigurarParametroCalculo(int pecacodi, ref string perianiomes, ref string pecatipocambio)
        {
            string queryString = string.Format(helper.SqlGetTipoCambioFecha, pecacodi);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    perianiomes = dr["PERIANIOMES"].ToString();
                    pecatipocambio = dr["PECATIPOCAMBIO"].ToString();
                }
            }
        }

        //- compensaciones.HDT - 03/03/2017: Cambio para atender el requerimiento. 
        public void PoblarRegistroSinCalculos(int pecacodi, string perianiomes)
        {
            string queryString = string.Empty;
            DbCommand command = null;

            queryString = string.Format(helper.SqlInsertRegistros, pecacodi, perianiomes);
            command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);
        }

        //- compensaciones.HDT - 03/03/2017: Cambio para atender el requerimiento. 
        public List<VceDatcalculoDTO> ObtenerRegistroSinCalculos(int pecacodi)
        {
            string queryString = string.Empty;
            DbCommand command = null;

            queryString = string.Format(helper.SqlListVceDatCalculoPorPeriodo, pecacodi);
            command = dbProvider.GetSqlStringCommand(queryString);

            List<VceDatcalculoDTO> lVceDtacalculoDTO = new List<VceDatcalculoDTO>();
            VceDatcalculoDTO vceDatacalculoDTO = null;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    vceDatacalculoDTO = helper.Create(dr);
                    lVceDtacalculoDTO.Add(vceDatacalculoDTO);
                }
            }

            return lVceDtacalculoDTO;
        }

        //- compensaciones.HDT - 16/03/2017: Cambio para atender el requerimiento. 
        public List<VceDatcalculoDTO> ObtenerRegistroSinCalculosPotenciaEfectica(int pecacodi)
        {
            string queryString = string.Empty;
            DbCommand command = null;

            queryString = string.Format(helper.SqlListVceDatCalculoPorPeriodo, pecacodi);
            command = dbProvider.GetSqlStringCommand(queryString);

            List<VceDatcalculoDTO> lVceDtacalculoDTO = new List<VceDatcalculoDTO>();
            VceDatcalculoDTO vceDatacalculoDTO = null;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    vceDatacalculoDTO = helper.Create(dr);
                    lVceDtacalculoDTO.Add(vceDatacalculoDTO);
                }
            }

            return lVceDtacalculoDTO;
        }

        //- compensaciones.HDT - 03/03/2017: Cambio para atender el requerimiento. 
        public List<DateTime> ObtenerDistintasFechasModificacion(int pecacodi)
        {
            string queryString = string.Empty;
            DbCommand command = null;

            List<DateTime> lFechasModificacion = new List<DateTime>();

            queryString = string.Format(helper.SqlListDistinctFecVceDatCalculoPorPeriodo, pecacodi);
            command = dbProvider.GetSqlStringCommand(queryString);
            DateTime fechaModificacion;
            int iCrdcgfecmod;

            string consultaDistintosGrupos = string.Empty;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    iCrdcgfecmod = dr.GetOrdinal("CRDCGFECMOD");
                    fechaModificacion = dr.GetDateTime(iCrdcgfecmod);
                    lFechasModificacion.Add(fechaModificacion);
                }
            }

            return lFechasModificacion;
        }

        //- compensaciones.HDT - 16/03/2017: Cambio para atender el requerimiento. 
        public List<DateTime> ObtenerDistintasFechasModificacionPotenciaEfectiva(int pecacodi)
        {
            string queryString = string.Empty;
            DbCommand command = null;

            List<DateTime> lFechasModificacion = new List<DateTime>();

            queryString = string.Format(helper.SqlListDistinctFecVceDatCalculoPorPeriodoPotenciaEfectiva, pecacodi);
            command = dbProvider.GetSqlStringCommand(queryString);
            DateTime fechaModificacion;
            int iCrdcgfecmod;

            string consultaDistintosGrupos = string.Empty;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    iCrdcgfecmod = dr.GetOrdinal("CRDCGFECMOD");
                    fechaModificacion = dr.GetDateTime(iCrdcgfecmod);
                    lFechasModificacion.Add(fechaModificacion);
                }
            }

            return lFechasModificacion;
        }

        //- compensaciones.HDT - 03/03/2017: Cambio para atender el requerimiento. 
        public List<int> ObtenerDistintosIdGrupo(int pecacodi, int fenergcodi, string Cfgdccondsql)
        {
            string queryString = string.Empty;
            DbCommand command = null;

            string whereAddDistintosGrupos = string.Empty;
            if (fenergcodi != 0)
            {
                whereAddDistintosGrupos = "AND GRUPOCODI IN (SELECT GRUPOCODI FROM PR_GRUPO WHERE FENERGCODI = " + fenergcodi.ToString() + ")";
            }
            else
            {   
                whereAddDistintosGrupos = "AND 1 = 1";
            }
            if (!string.IsNullOrEmpty(Cfgdccondsql))
            {
                whereAddDistintosGrupos = whereAddDistintosGrupos + Cfgdccondsql;
            }

            queryString = string.Format(helper.SqlListDistinctIdGrupo, pecacodi, whereAddDistintosGrupos);
            command = dbProvider.GetSqlStringCommand(queryString);

            List<int> lIdGruposDistintos = new List<int>();
            int iGrupocodi = 0;
            int grupoCodi = 0;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    iGrupocodi = dr.GetOrdinal("GRUPOCODI");
                    grupoCodi = dr.GetInt32(iGrupocodi);
                    lIdGruposDistintos.Add(grupoCodi);
                }
            }

            return lIdGruposDistintos;
        }

        //- compensaciones.HDT - 16/03/2017: Cambio para atender el requerimiento. 
        public List<int> ObtenerDistintosIdGrupo(int pecacodi)
        {
            string queryString = string.Empty;
            DbCommand command = null;

            string whereAddDistintosGrupos = string.Empty;

            queryString = string.Format(helper.SqlListDistinctIdGrupoPotEfectiva, pecacodi);
            command = dbProvider.GetSqlStringCommand(queryString);

            List<int> lIdGruposDistintos = new List<int>();
            int iGrupocodi = 0;
            int grupoCodi = 0;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    iGrupocodi = dr.GetOrdinal("GRUPOCODI");
                    grupoCodi = dr.GetInt32(iGrupocodi);
                    lIdGruposDistintos.Add(grupoCodi);
                }
            }

            return lIdGruposDistintos;
        }
        

        //- compensaciones.HDT - 03/03/2017: Cambio para atender el requerimiento. 
        public void SaveCalculo(int pecacodi, List<VceCfgDatCalculoDTO> lVceCfgDatCalculoDTO)
        {

            string queryString = string.Empty;
            DbCommand command = null;

            //- 2. Leer la data de DatCalculo y llevarlo a a Lista de DTOs: ListaDtoVCE_DATCALCULO SELECT * FROM VCE_DATCALCULO WHERE pecacodi = 22;
           

            //- 3. Se obtiene la distintas fechas de modificación.
            queryString = string.Format(helper.SqlListDistinctFecVceDatCalculoPorPeriodo, pecacodi);
            command = dbProvider.GetSqlStringCommand(queryString);
            DateTime fechaModificacion;
            int iCrdcgfecmod;

            string queryCampo = string.Empty;
            DbCommand commandCampo = null;

            string consultaDistintosGrupos = string.Empty;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    iCrdcgfecmod = dr.GetOrdinal("CRDCGFECMOD");
                    fechaModificacion = dr.GetDateTime(iCrdcgfecmod);

                    //- 3.1. Se obtienen la configuración de los campos.

                    queryCampo = string.Format(helper.SqlListDistinctFecVceDatCalculoPorPeriodo, pecacodi);
                    commandCampo = dbProvider.GetSqlStringCommand(queryCampo);

                    foreach (VceCfgDatCalculoDTO vceCfgDatCalculoDTO in lVceCfgDatCalculoDTO)
                    {
                        if (vceCfgDatCalculoDTO.Fenergcodi != 0)
                        {
                            //- Incluir cláusala IN
                        }
                        else
                        {
                            //- No Incluir cláusala IN

                        }
                    }

                }
            }

        }

         //- compensaciones.HDT - 03/03/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Actualiza los datos necesarios pos cálculo efectuado. 
        /// </summary>
        /// <param name="pecacodi"></param>
        /// <param name="pecatipocambio"></param>
        public void ActualizarDatosPosCalculo(int pecacodi, string pecatipocambio)
        {
            string queryString = string.Empty;
            DbCommand command = null;

            //Actualizamos los arranques y paradas por modo de operacion
            queryString = string.Format(helper.SqlUpdateArrPar, pecacodi);
            command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);

            //Actualizamos los datos particulares (se ha comentado porque se realizará de otra forma)
            /*
            queryString = string.Format(helper.SqlUpdateConsComb, pecacodi, pecatipocambio);
            command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);
             */
        }

        //- alpha.HDT - Inicio 01/10/2016: : Cambio para atender el requerimiento.
        //- Remover en proxima revision: SI

//        public void SaveCalculo(int pecacodi)
//        {
//            //Obtenemos los parametros (PERIANIOMES, PERITIPOCAMBIO)
//            string queryString = string.Format(helper.SqlGetTipoCambioFecha, pecacodi);
//            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

//            String perianiomes = "";
//            String pecatipocambio = "";
            
//            using (IDataReader dr = dbProvider.ExecuteReader(command))
//            {
//                while (dr.Read())
//                {
//                    perianiomes = dr["PERIANIOMES"].ToString();
//                    pecatipocambio = dr["PERITIPOCAMBIO"].ToString();
//                }
//            }

//            //Insertamos los registros sin los calculos
//            queryString = string.Format(helper.SqlInsertRegistros, pecacodi, perianiomes);
//            command = dbProvider.GetSqlStringCommand(queryString);
//            dbProvider.ExecuteNonQuery(command);

//            //Reemplazamos el nuevo método

//#warning HDT.26.02.2017 - Inicio:  Esta es la implementación que falta completar.

//            //1. Actualizar el tipo de combustible: CFGDCTIPOVAL ='F'

//            //2. Leer la data de DatCalculo y llevarlo a a Lista de DTOs: ListaDtoVCE_DATCALCULO SELECT * FROM VCE_DATCALCULO WHERE pecacodi = 22;
            
//            //3. Procesar la información.

//            queryString = string.Format(helper.SqlListGrupo);
//            command = dbProvider.GetSqlStringCommand(queryString);

//            String vFenergcodi = "";
//            String vCfgdccondsql = "";

//            using (IDataReader dr = dbProvider.ExecuteReader(command))
//            {
//                while (dr.Read())
//                {
//                    int vSec = 0;

//                    vFenergcodi = dr["FENERGCODI"].ToString();
//                    vCfgdccondsql = dr["CFGDCCONDSQL"].ToString();

//                    string queryStringSql = string.Format(helper.SqlListCampo, vFenergcodi, vCfgdccondsql.Replace("'", "''"));
//                    DbCommand commandSql = dbProvider.GetSqlStringCommand(queryStringSql);

//                    string sql = "UPDATE VCE_DATCALCULO SET ";
//                    using (IDataReader drC = dbProvider.ExecuteReader(commandSql))
//                    {
//                        while (drC.Read())
//                        {
//                            if (vSec > 0)
//                            {
//                                sql = sql + ",";
//                            }
//                            if (drC["CFGDCTIPOVAL"].ToString().Equals("V"))
//                            {
//                                sql = sql + " " + drC["CFGDCCAMPONOMB"].ToString() + " = ROUND(VCE_COMPENSACION_PKG.FNC_OBTIENE_VAL_CONCEPTO(GRUPOCODI," + drC["CONCEPCODI"].ToString() + ",CRDCGFECMOD," + pecatipocambio + "),10)";
//                            }
//                            else if (drC["CFGDCTIPOVAL"].ToString().Equals("F"))
//                            {
//                                sql = sql + " " + drC["CFGDCCAMPONOMB"].ToString() + " = SUBSTR(VCE_COMPENSACION_PKG.FNC_OBTIENE_FORMULA_BASE(GRUPOCODI," + drC["CONCEPCODI"].ToString() + ",CRDCGFECMOD),1,30)";
//                            }
//                            else if (drC["CFGDCTIPOVAL"].ToString().Equals("U"))
//                            {
//                                sql = sql + " " + drC["CFGDCCAMPONOMB"].ToString() + " = SUBSTR((SELECT MAX(CONCEPUNID) FROM PR_CONCEPTO WHERE CONCEPCODI = " + drC["CONCEPCODI"].ToString() + "),1,20)";
//                            }

//                            vSec++;
//                        }
//                    }
//                    sql = sql + " WHERE pecacodi = " + pecacodi;

//                    if (!dr["FENERGCODI"].ToString().Equals("0"))
//                    {
//                        sql = sql + " AND GRUPOCODI IN (SELECT GRUPOCODI FROM PR_GRUPO WHERE FENERGCODI = " + dr["FENERGCODI"].ToString() + " )";
//                    }

//                    sql = sql + " " + vCfgdccondsql;

//                    DbCommand commandFinal = dbProvider.GetSqlStringCommand(sql);
//                    dbProvider.ExecuteNonQuery(commandFinal);
//                }
//            }

//#warning HDT Fin

//            //Actualizamos los arranques y paradas por modo de operacion
//            queryString = string.Format(helper.SqlUpdateArrPar, pecacodi);
//            command = dbProvider.GetSqlStringCommand(queryString);
//            dbProvider.ExecuteNonQuery(command);

//            //Actualizamos los datos particulares
//            queryString = string.Format(helper.SqlUpdateConsComb, pecacodi, pecatipocambio);
//            command = dbProvider.GetSqlStringCommand(queryString);
//            dbProvider.ExecuteNonQuery(command);
//        }

        //- HDT Fin

        public void DeleteCalculo(int pecacodi)
        {
            string queryString = string.Format(helper.SqlDeleteCalculo, pecacodi);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);
        }

        public IDataReader ListCompensacionArrPar(int pecacodi, string empresa, string central, string grupo, string modo)
        {
            string querySession = "ALTER SESSION SET NLS_DATE_LANGUAGE = 'SPANISH'";
            DbCommand command = dbProvider.GetSqlStringCommand(querySession);
            dbProvider.ExecuteNonQuery(command);

            string queryStringParametros = string.Format(helper.SqlGetParametrosGenerales, pecacodi);
            DbCommand commandParametros = dbProvider.GetSqlStringCommand(queryStringParametros);

            String fechaIniPeriodo = "";
            String fechaFinPeriodo = "";
            String pecaTipoCambio = "";

            using (IDataReader dr = dbProvider.ExecuteReader(commandParametros))
            {
                while (dr.Read())
                {
                    fechaIniPeriodo = dr["FECHAINI"].ToString();
                    fechaFinPeriodo = dr["FECHAFIN"].ToString();
                    pecaTipoCambio = dr["PECATIPOCAMBIO"].ToString();
                }
            }

            //Cursor Fechas [ListCursorFechas]
            string queryStringFechas = string.Format(helper.SqlListCursorFechas, pecacodi);
            DbCommand commandFechas = dbProvider.GetSqlStringCommand(queryStringFechas);

            //Cursor Grupos [ListCursorGrupos]
            string queryStringGrupos = string.Format(helper.SqlListCursorGrupos, fechaIniPeriodo, fechaFinPeriodo);
            DbCommand commandGrupos = dbProvider.GetSqlStringCommand(queryStringGrupos);


            string queryString = "SELECT DC.GRUPOCODI, (CASE WHEN EQ.EMPRCODI IS NULL  THEN '_NO DEFINIDO' ELSE EMP.EMPRNOMB END) as \"Empresa\", TRIM(MO.GRUPONOMB) AS \"Modo Operación\"";
            queryString = queryString + ",MAX(CRDCGCCBEFARR) \"CC.Arran\",MAX(CRDCGCCBEFARRTOMA) \"CC.Toma\",MAX(CRDCGCONSPOTEFEARR) \"Cons. PE\"";
            queryString = queryString + ",MAX(CRDCGCCBEFPAR) \"CC.Parada\",MAX(CRDCGCCBEFPARRAMPA) \"Rampa Desc.\",MAX(CRDCGCONSPOTEFEPAR) \"Cons. PE Par\"";
            queryString = queryString + ",MAX(CRDCGPRECIOAPLICUNID) AS \"Unidad\"";

            using (IDataReader dr = dbProvider.ExecuteReader(commandFechas))
            {
                while (dr.Read())
                {
                    queryString = queryString + ",MAX(CASE TO_CHAR(DC.CRDCGFECMOD,'DD') WHEN '" + dr["DIA"].ToString() + "' THEN CRDCGPRECIOAPLIC END) AS \"" + dr["TITULO"].ToString() + "\"";
                }
            }

            using (IDataReader dr = dbProvider.ExecuteReader(commandFechas))
            {
                while (dr.Read())
                {
                    queryString = queryString + ",MAX(CASE TO_CHAR(DC.CRDCGFECMOD,'DD') WHEN '" + dr["DIA"].ToString() + "' THEN CRDCGPRECIOAPLICXARR END) AS \"" + dr["TITULO"].ToString() + "\"";
                }
            }

            using (IDataReader dr = dbProvider.ExecuteReader(commandFechas))
            {
                while (dr.Read())
                {
                    queryString = queryString + ",MAX(CASE TO_CHAR(DC.CRDCGFECMOD,'DD') WHEN '" + dr["DIA"].ToString() + "' THEN CRDCGPRECIOAPLICXPAR END) AS \"" + dr["TITULO"].ToString() + "\"";
                }
            }

            using (IDataReader dr = dbProvider.ExecuteReader(commandGrupos))
            {
                while (dr.Read())
                {
                    queryString = queryString + ",MAX(CASE WHEN cab.APGCFCCODI='" + dr["APGCFCCODI"].ToString() + "' THEN apgcabccbef END) AS \"" + dr["APGCFCNOMBRE"].ToString() + "\""; 
                }
            }

            using (IDataReader dr = dbProvider.ExecuteReader(commandFechas))
            {
                while (dr.Read())
                {
                    queryString = queryString + ",MAX(CASE TO_CHAR(DC.CRDCGFECMOD,'DD') WHEN '" + dr["DIA"].ToString() + "' THEN CRDCGCMARR_SOL END) AS \"" + dr["TITULO"].ToString() + "\"";
                }
            }

            using (IDataReader dr = dbProvider.ExecuteReader(commandGrupos))
            {
                while (dr.Read())
                {
                    queryString = queryString + ",MAX(CASE WHEN cab.APGCFCCODI='" + dr["APGCFCCODI"].ToString() + "' THEN apgcabccmarr END) AS \"" + dr["APGCFCNOMBRE"].ToString() + "\"";
                }
            }

            using (IDataReader dr = dbProvider.ExecuteReader(commandGrupos))
            {
                while (dr.Read())
                {

                    using (IDataReader drFec1 = dbProvider.ExecuteReader(commandFechas))
                    {
                        while (drFec1.Read())
                        {
                            queryString = queryString + ",MAX(CASE WHEN DET.APGCFCCODI='" + dr["APGCFCCODI"].ToString() + "' AND TO_CHAR(DET.APGDETFECINIPER,'DD') = '" + drFec1["DIA"].ToString() + "' THEN APGDETNUMARR END) AS \"" + drFec1["TITULO"].ToString() + "\"";
                        }
                    }

                    using (IDataReader drFec2 = dbProvider.ExecuteReader(commandFechas))
                    {
                        while (drFec2.Read())
                        {
                            queryString = queryString + ",MAX(CASE WHEN DET.APGCFCCODI='" + dr["APGCFCCODI"].ToString() + "' AND TO_CHAR(DET.APGDETFECINIPER,'DD') = '" + drFec2["DIA"].ToString() + "' THEN APGDETNUMPAR END) AS \"" + drFec2["TITULO"].ToString() + "\"";
                        }
                    }
                }
            }

            // DSH 19-04-2017 aplicando cambios 
            using (IDataReader dr = dbProvider.ExecuteReader(commandFechas))
            {
                while (dr.Read())
                {
                    queryString = queryString + ",MAX(CASE TO_CHAR(DC.CRDCGFECMOD,'DD') WHEN '" + dr["DIA"].ToString() + "' THEN CRDCGPRECIOAPLICXINCGEN END) AS \"" + dr["TITULO"].ToString() + "\"";
                }
            }

            using (IDataReader dr = dbProvider.ExecuteReader(commandFechas))
            {
                while (dr.Read())
                {
                    queryString = queryString + ",MAX(CASE TO_CHAR(DC.CRDCGFECMOD,'DD') WHEN '" + dr["DIA"].ToString() + "' THEN CRDCGPRECIOAPLICXDISGEN END) AS \"" + dr["TITULO"].ToString() + "\"";
                }
            }

            using (IDataReader dr = dbProvider.ExecuteReader(commandFechas))
            {
                while (dr.Read())
                {
                    queryString = queryString + ",MAX(CASE TO_CHAR(DET.APGDETFECINIPER,'DD') WHEN '" + dr["DIA"].ToString() + "' THEN APGDETNUMINC END) AS \"" + dr["TITULO"].ToString() + "\"";
                }
            }

            using (IDataReader dr = dbProvider.ExecuteReader(commandFechas))
            {
                while (dr.Read())
                {
                    queryString = queryString + ",MAX(CASE TO_CHAR(DET.APGDETFECINIPER,'DD') WHEN '" + dr["DIA"].ToString() + "' THEN APGDETNUMDIS END) AS \"" + dr["TITULO"].ToString() + "\"";
                }
            }

            using (IDataReader dr = dbProvider.ExecuteReader(commandGrupos))
            {
                while (dr.Read())
                {
                    queryString = queryString + ",MAX(CASE WHEN cab.APGCFCCODI='" + dr["APGCFCCODI"].ToString() + "' THEN apgcabccadic END) AS \"" + dr["APGCFCNOMBRE"].ToString() + "\"";
                }
            }

            
            // DSH 30-06-2017 : cambios por requerimiento
               
            string condicion = "";
            
            if (!empresa.Equals("") && empresa != null)
            {
                condicion = condicion + " AND EMP.EMPRCODI = " + empresa;
            }

            if (!central.Equals("") && central != null)
            {
                condicion = condicion + " AND CG.EQUICODI = " + central;
            }

            if (!grupo.Equals("") && grupo != null)
            {
                condicion = condicion + " AND GG.GRUPOCODI = " + grupo;
            }

            if (!modo.Equals("") && modo != null)
            {
                condicion = condicion + " AND MO.GRUPOCODI = " + modo;
            }

            /*
            queryString = queryString + " FROM PR_GRUPO GR JOIN VCE_DATCALCULO DC ON GR.GRUPOCODI = DC.GRUPOCODI ";
            queryString = queryString + " LEFT JOIN SI_EMPRESA EMP ON GR.EMPRCODI = EMP.EMPRCODI";
            queryString = queryString + " LEFT JOIN VCE_ARRPAR_GRUPO_CAB CAB ON DC.pecacodi = CAB.pecacodi AND DC.GRUPOCODI = CAB.GRUPOCODI";
            queryString = queryString + " LEFT JOIN VCE_ARRPAR_GRUPO_DET DET ON DC.pecacodi = DET.pecacodi AND DC.GRUPOCODI = DET.GRUPOCODI";
            queryString = queryString + " WHERE GR.CATECODI = 2 AND GR.GRUPOACTIVO = 'S' AND DC.pecacodi = " + pecacodi + condicion;
            queryString = queryString + " GROUP BY EMP.EMPRNOMB,DC.GRUPOCODI,GR.GRUPONOMB";
             */
            queryString = queryString + " FROM VCE_DATCALCULO DC";
            queryString = queryString + " JOIN PR_GRUPO MO ON DC.GRUPOCODI = MO.GRUPOCODI";
            queryString = queryString + " JOIN PR_GRUPO GG ON MO.GRUPOPADRE = GG.GRUPOCODI";
           // queryString = queryString + " JOIN PR_GRUPO CG ON GG.GRUPOPADRE = CG.GRUPOCODI";
            queryString = queryString + " LEFT JOIN";
            queryString = queryString + " (SELECT GRUPOCODI, EMPRCODI, MAX(EQUIPADRE) EQUIPADRE";
            queryString = queryString + " FROM EQ_EQUIPO ";
            queryString = queryString + " WHERE FAMCODI IN (2,3) AND NVL(GRUPOCODI,-1) != -1  AND EQUIESTADO = 'A'";
            queryString = queryString + " GROUP BY GRUPOCODI, EMPRCODI";
            queryString = queryString + " UNION";
            queryString = queryString + " SELECT GRUPOCODI, EMPRCODI, MAX(EQUIPADRE) EQUIPADRE";
            queryString = queryString + " FROM EQ_EQUIPO";
            queryString = queryString + " WHERE FAMCODI IN (2,3) AND NVL(GRUPOCODI,-1) != -1  AND EQUIESTADO = 'B'";
            queryString = queryString + " AND GRUPOCODI NOT IN (SELECT DISTINCT GRUPOCODI";
            queryString = queryString + " FROM EQ_EQUIPO";
            queryString = queryString + " WHERE FAMCODI IN (2,3) AND NVL(GRUPOCODI,-1) != -1";
            queryString = queryString + " AND EQUIESTADO = 'A' )";
            queryString = queryString + " GROUP BY GRUPOCODI, EMPRCODI";
            queryString = queryString + " )EQ ON MO.GRUPOPADRE =EQ.GRUPOCODI";
            queryString = queryString + " LEFT JOIN EQ_EQUIPO CG ON  EQ.EQUIPADRE = CG.EQUICODI";
            queryString = queryString + " LEFT JOIN SI_EMPRESA EMP ON EQ.EMPRCODI = EMP.EMPRCODI";
            queryString = queryString + " LEFT JOIN VCE_ARRPAR_GRUPO_CAB CAB ON DC.pecacodi = CAB.pecacodi AND DC.GRUPOCODI = CAB.GRUPOCODI";
            queryString = queryString + " LEFT JOIN VCE_ARRPAR_GRUPO_DET DET ON DC.pecacodi = DET.pecacodi AND DC.GRUPOCODI = DET.GRUPOCODI";
            queryString = queryString + " WHERE DC.pecacodi = " + pecacodi + " AND MO.CATECODI = 2 AND MO.GRUPOACTIVO = 'S' " + condicion;
            queryString = queryString + " GROUP BY (CASE WHEN EQ.EMPRCODI IS NULL  THEN '_NO DEFINIDO' ELSE EMP.EMPRNOMB END),DC.GRUPOCODI,MO.GRUPONOMB";
            //queryString = queryString + " ORDER BY (CASE WHEN EQ.EMPRCODI IS NULL  THEN '_NO DEFINIDO' ELSE EMP.EMPRNOMB END),MO.GRUPONOMB";

            command = dbProvider.GetSqlStringCommand(queryString);
            IDataReader drList = dbProvider.ExecuteReader(command);
            return drList;
        }


        public IDataReader ListCabCompensacionArrPar(int pecacodi)
        {
            string querySession = "ALTER SESSION SET NLS_DATE_LANGUAGE = 'SPANISH'";
            DbCommand command = dbProvider.GetSqlStringCommand(querySession);
            dbProvider.ExecuteNonQuery(command);

            string queryStringParametros = string.Format(helper.SqlGetParametrosGenerales, pecacodi);
            DbCommand commandParametros = dbProvider.GetSqlStringCommand(queryStringParametros);

            String fechaIniPeriodo = "";
            String fechaFinPeriodo = "";
            String pecaTipoCambio = "";

            int nroFechas = 0;
            int nroGrupos = 0;

            using (IDataReader dr = dbProvider.ExecuteReader(commandParametros))
            {
                while (dr.Read())
                {
                    fechaIniPeriodo = dr["FECHAINI"].ToString();
                    fechaFinPeriodo = dr["FECHAFIN"].ToString();
                    pecaTipoCambio = dr["PECATIPOCAMBIO"].ToString();
                }
            }

            //Cursor Fechas [ListCursorFechas]
            string queryStringFechas = string.Format(helper.SqlListCursorFechas, pecacodi);
            DbCommand commandFechas = dbProvider.GetSqlStringCommand(queryStringFechas);

            //Cursor Grupos [ListCursorGrupos]
            string queryStringGrupos = string.Format(helper.SqlListCursorGrupos, fechaIniPeriodo, fechaFinPeriodo);
            DbCommand commandGrupos = dbProvider.GetSqlStringCommand(queryStringGrupos);

            string queryCab = "SELECT ' ' AS TITULO,3 AS COLSPAN FROM DUAL";
            queryCab = queryCab + " UNION ALL SELECT 'Consumo de Combustible para el CCbefa (GAL, GJ)',3 AS COLSPAN FROM DUAL";
            queryCab = queryCab + " UNION ALL SELECT 'Consumo de Combustible para el CCbefp (GAL, GJ)',3 AS COLSPAN FROM DUAL";
            queryCab = queryCab + " UNION ALL SELECT ' ',1 AS COLSPAN FROM DUAL";

            using (IDataReader dr = dbProvider.ExecuteReader(commandFechas))
            {
                while (dr.Read())
                {
                    nroFechas++;
                }
            }

            queryCab = queryCab + " UNION ALL SELECT 'COSTO DE COMBUSTIBLE (S//GAL)',"+ nroFechas +" FROM DUAL";
            queryCab = queryCab + " UNION ALL SELECT 'COSTO POR ARRANQUE (S//Evento)'," + nroFechas + " FROM DUAL";
            queryCab = queryCab + " UNION ALL SELECT 'COSTO POR PARADA (S//Evento)'," + nroFechas + " FROM DUAL";

            using (IDataReader dr = dbProvider.ExecuteReader(commandGrupos))
            {
                while (dr.Read())
                {
                    nroGrupos++;
                }
            }

            queryCab = queryCab + " UNION ALL SELECT 'Ccbef (S/.)'," + nroGrupos + " FROM DUAL";
            queryCab = queryCab + " UNION ALL SELECT 'CMarr (S/.)'," + nroFechas + " FROM DUAL";
            //queryCab = queryCab + " UNION ALL SELECT 'CMarr (S/.)'," + nroGrupos + " FROM DUAL";
            queryCab = queryCab + " UNION ALL SELECT 'CMarr total del mes (S/.)'," + nroGrupos + " FROM DUAL";


            using (IDataReader dr = dbProvider.ExecuteReader(commandGrupos))
            {
                while (dr.Read())
                {
                    queryCab = queryCab + " UNION ALL SELECT 'Número de arranques (" + dr["APGCFCNOMBRE"].ToString() + ")'," + nroFechas + " FROM DUAL";

                    queryCab = queryCab + " UNION ALL SELECT 'Número de paradas (" + dr["APGCFCNOMBRE"].ToString() + ")'," + nroFechas + " FROM DUAL";
                }
            }

            // DSH 19-04-2017 : cambios por requerimiento
            queryCab = queryCab + " UNION ALL SELECT 'Costo combustible adicional en incremento de generación (S//Evento)'," + nroFechas + " FROM DUAL";
            queryCab = queryCab + " UNION ALL SELECT 'Costo combustible adicional en disminución de generación (S//Evento)'," + nroFechas + " FROM DUAL";
            queryCab = queryCab + " UNION ALL SELECT 'Número de incrementos de generación'," + nroFechas + " FROM DUAL";
            queryCab = queryCab + " UNION ALL SELECT 'Número de reducciones de generación'," + nroFechas + " FROM DUAL";
            queryCab = queryCab + " UNION ALL SELECT 'CCCadic (S/.)'," + nroGrupos + " FROM DUAL";

            command = dbProvider.GetSqlStringCommand(queryCab);
            IDataReader drList = dbProvider.ExecuteReader(command);
            return drList;
        }

        //- compensaciones.JDEL - 05/03/2017: Cambio para atender el requerimiento. 
        public void SaveArranquesParadas(int pecacodi)
        {

            string queryString = string.Empty;

            //--INICIALIZAMOS LA TABLA
            string queryDelete = "DELETE FROM VCE_ARRPAR_GRUPO_DET WHERE pecacodi = " + pecacodi;
            DbCommand commanDelete = dbProvider.GetSqlStringCommand(queryDelete);
            dbProvider.ExecuteNonQuery(commanDelete);

            queryDelete = "DELETE FROM VCE_ARRPAR_GRUPO_CAB WHERE pecacodi = " + pecacodi;
            commanDelete = dbProvider.GetSqlStringCommand(queryDelete);
            dbProvider.ExecuteNonQuery(commanDelete);

            string queryStringParametros = string.Format(helper.SqlGetParametrosGenerales, pecacodi);
            DbCommand commandParametros = dbProvider.GetSqlStringCommand(queryStringParametros);

            String fechaIniPeriodo = "";
            String fechaFinPeriodo = "";
            String pecaTipoCambio = "";

            using (IDataReader dr = dbProvider.ExecuteReader(commandParametros))
            {
                while (dr.Read())
                {
                    fechaIniPeriodo = dr["FECHAINI"].ToString();
                    fechaFinPeriodo = dr["FECHAFIN"].ToString();
                    pecaTipoCambio = dr["PECATIPOCAMBIO"].ToString();
                }
            }

            //--Generamos el detalle de periodos    
            // DSH 05-06-2017 : se cambio por requerimiento
            string queryDetalle = "INSERT INTO VCE_ARRPAR_GRUPO_DET (pecacodi,GRUPOCODI,APGDETFECINIPER,APGCFCCODI,APGDETPRECIOAPLICXARR,APGDETPRECIOAPLICXPAR)";
            queryDetalle = queryDetalle + "SELECT " + pecacodi + ",GRUPOCODI,CRDCGFECMOD,GR.APGCFCCODI,CRDCGPRECIOAPLICXARR,CRDCGPRECIOAPLICXPAR";
            queryDetalle = queryDetalle + " FROM VCE_DATCALCULO DC, (SELECT DISTINCT APGCFCCODI FROM VCE_ARRPAR_GRUPO_CFGDET WHERE APGCFDFECALTA <= TO_DATE('" + fechaIniPeriodo + "','DD/MM/YYYY') AND (APGCFDFECBAJA IS NULL OR APGCFDFECBAJA > TO_DATE('" + fechaFinPeriodo + "','DD/MM/YYYY'))) GR ";
            queryDetalle = queryDetalle + " WHERE DC.pecacodi = " + pecacodi;

            DbCommand commanDetalle = dbProvider.GetSqlStringCommand(queryDetalle);
            dbProvider.ExecuteNonQuery(commanDetalle);

            //--Procesamos los modos de Operación
            string queryCurModoOperacion = string.Format(helper.SqlListCurModoOperacion, fechaFinPeriodo, pecacodi);
            DbCommand commandProc = dbProvider.GetSqlStringCommand(queryCurModoOperacion);

            using (IDataReader dr = dbProvider.ExecuteReader(commandProc))
            {
                while (dr.Read())
                {
                    //--Actualizamos los arranques y paradas 
                    string queryStringArrPar = "SELECT NVL(SUM(CASE WHEN HOPCOMPORDARRQ='S' THEN 1 ELSE 0 END),0) AS NUM_ARR , NVL(SUM(CASE WHEN HOPCOMPORDPARD='S' THEN 1 ELSE 0 END),0) AS NUM_PAR ";
                    queryStringArrPar = queryStringArrPar + " FROM EVE_HORAOPERACION";
                    queryStringArrPar = queryStringArrPar + " WHERE GRUPOCODI = " + dr["GRUPOCODI"].ToString();
                    queryStringArrPar = queryStringArrPar + " AND SUBCAUSACODI IN (SELECT SUBCAUSACODI FROM VCE_ARRPAR_GRUPO_CFGDET WHERE APGCFCCODI = '" + dr["APGCFCCODI"].ToString() + "' AND APGCFDFECALTA <= TO_DATE('" + fechaIniPeriodo + "','DD/MM/YYYY') AND (APGCFDFECBAJA IS NULL OR APGCFDFECBAJA > TO_DATE('" + fechaFinPeriodo + "','DD/MM/YYYY')))";
                    queryStringArrPar = queryStringArrPar + " AND HOPHORINI >= TO_DATE('" + dr["APGDETFECINIPER"] + "','DD/MM/YYYY') AND HOPHORINI < TO_DATE('" + dr["APGDETFECFINPER"] + "','DD/MM/YYYY')";

                    DbCommand commandArrPar = dbProvider.GetSqlStringCommand(queryStringArrPar);

                    String vNumArr = "";
                    String vNumPar = "";
                    using (IDataReader dr1 = dbProvider.ExecuteReader(commandArrPar))
                    {
                        while (dr1.Read())
                        {
                            vNumArr = dr1["NUM_ARR"].ToString();
                            vNumPar = dr1["NUM_PAR"].ToString();
                        }
                    }

                    queryString = "UPDATE VCE_ARRPAR_GRUPO_DET  SET APGDETNUMARR = " + vNumArr + ", APGDETNUMPAR = " + vNumPar;
                    queryString = queryString + " WHERE pecacodi = " + pecacodi + " AND GRUPOCODI = " + dr["GRUPOCODI"] + " AND APGCFCCODI = '" + dr["APGCFCCODI"] + "'";
                    queryString = queryString + " AND APGDETFECINIPER = TO_DATE('" + dr["APGDETFECINIPER"] + "','DD/MM/YYYY')";

                    DbCommand command = dbProvider.GetSqlStringCommand(queryString);
                    dbProvider.ExecuteNonQuery(command);

                }
            }

            queryString = "INSERT INTO VCE_ARRPAR_GRUPO_CAB (pecacodi,GRUPOCODI,APGCFCCODI,APGCABCCBEF,APGCABCCMARR)";
            queryString = queryString + " SELECT APD.pecacodi,APD.GRUPOCODI,APD.APGCFCCODI,SUM(NVL(APD.APGDETNUMARR*APD.APGDETPRECIOAPLICXARR,0) + NVL(APD.APGDETNUMPAR*APD.APGDETPRECIOAPLICXPAR,0))";
            queryString = queryString + " ,SUM(NVL(DC.CRDCGCMARR_SOL*APD.APGDETNUMARR,0) + NVL(DC.CRDCGCMARR_SOL*APD.APGDETNUMPAR,0))/2  ";
            queryString = queryString + " FROM VCE_ARRPAR_GRUPO_DET APD JOIN VCE_DATCALCULO DC ON APD.pecacodi = DC.pecacodi AND APD.GRUPOCODI = DC.GRUPOCODI AND APD.APGDETFECINIPER = DC.CRDCGFECMOD";
            queryString = queryString + " WHERE APD.pecacodi = " + pecacodi;
            queryString = queryString + " GROUP BY APD.pecacodi,APD.GRUPOCODI,APD.APGCFCCODI";

            DbCommand commandInsert = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(commandInsert);

        }


        //- compensaciones.HDT - 16/03/2017: Cambio para atender el requerimiento. 
        public void ActualizarTipoCombustible(int pecacodi, string fechaModificacion)
        {
            string queryString = string.Format(helper.SqlActualizarTipoCombustible, pecacodi, fechaModificacion);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);
        }

        public void SaveFromOtherVersion(int pecacodiDestino, int pecacodiOrigen)
        {
            string queryString = string.Format(helper.SqlSaveFromOtherVersion, pecacodiDestino, pecacodiOrigen);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);
        }
    }
}
