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
    public class VceCompBajaeficRepository : RepositoryBase, IVceCompBajaeficRepository
    {
        public VceCompBajaeficRepository(string strConn)
            : base(strConn)
        {
        }

        VceCompBajaeficHelper helper = new VceCompBajaeficHelper();

        public void Save(VceCompBajaeficDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Crcbetipocalc, DbType.String, entity.Crcbetipocalc);
            dbProvider.AddInParameter(command, helper.Crcbecompensacion, DbType.Decimal, entity.Crcbecompensacion);
            dbProvider.AddInParameter(command, helper.Crcbecvt, DbType.Decimal, entity.Crcbecvt);
            dbProvider.AddInParameter(command, helper.Crcbecvnc, DbType.Decimal, entity.Crcbecvnc);
            dbProvider.AddInParameter(command, helper.Crcbecvc, DbType.Decimal, entity.Crcbecvc);
            dbProvider.AddInParameter(command, helper.Crcbeconsumo, DbType.Decimal, entity.Crcbeconsumo);
            dbProvider.AddInParameter(command, helper.Crcbepotencia, DbType.Decimal, entity.Crcbepotencia);
            dbProvider.AddInParameter(command, helper.Crcbehorfin, DbType.DateTime, entity.Crcbehorfin);
            dbProvider.AddInParameter(command, helper.Crcbehorini, DbType.DateTime, entity.Crcbehorini);
            dbProvider.AddInParameter(command, helper.Subcausacodi, DbType.Int32, entity.Subcausacodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Pecacodi, DbType.Int32, entity.PecaCodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(VceCompBajaeficDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Crcbetipocalc, DbType.String, entity.Crcbetipocalc);
            dbProvider.AddInParameter(command, helper.Crcbecompensacion, DbType.Decimal, entity.Crcbecompensacion);
            dbProvider.AddInParameter(command, helper.Crcbecvt, DbType.Decimal, entity.Crcbecvt);
            dbProvider.AddInParameter(command, helper.Crcbecvnc, DbType.Decimal, entity.Crcbecvnc);
            dbProvider.AddInParameter(command, helper.Crcbecvc, DbType.Decimal, entity.Crcbecvc);
            dbProvider.AddInParameter(command, helper.Crcbeconsumo, DbType.Decimal, entity.Crcbeconsumo);
            dbProvider.AddInParameter(command, helper.Crcbepotencia, DbType.Decimal, entity.Crcbepotencia);
            dbProvider.AddInParameter(command, helper.Crcbehorfin, DbType.DateTime, entity.Crcbehorfin);
            dbProvider.AddInParameter(command, helper.Crcbehorini, DbType.DateTime, entity.Crcbehorini);
            dbProvider.AddInParameter(command, helper.Subcausacodi, DbType.Int32, entity.Subcausacodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Pecacodi, DbType.Int32, entity.PecaCodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(DateTime crcbehorfin, DateTime crcbehorini, int subcausacodi, int grupocodi, int pecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Crcbehorfin, DbType.DateTime, crcbehorfin);
            dbProvider.AddInParameter(command, helper.Crcbehorini, DbType.DateTime, crcbehorini);
            dbProvider.AddInParameter(command, helper.Subcausacodi, DbType.Int32, subcausacodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);
            dbProvider.AddInParameter(command, helper.Pecacodi, DbType.Int32, pecacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public VceCompBajaeficDTO GetById(DateTime crcbehorfin, DateTime crcbehorini, int subcausacodi, int grupocodi, int pecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Crcbehorfin, DbType.DateTime, crcbehorfin);
            dbProvider.AddInParameter(command, helper.Crcbehorini, DbType.DateTime, crcbehorini);
            dbProvider.AddInParameter(command, helper.Subcausacodi, DbType.Int32, subcausacodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);
            dbProvider.AddInParameter(command, helper.Pecacodi, DbType.Int32, pecacodi);
            VceCompBajaeficDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VceCompBajaeficDTO> List()
        {
            List<VceCompBajaeficDTO> entitys = new List<VceCompBajaeficDTO>();
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

        public List<VceCompBajaeficDTO> GetByCriteria()
        {
            List<VceCompBajaeficDTO> entitys = new List<VceCompBajaeficDTO>();
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

        public List<VceCompBajaeficDTO> ListCompensacionesRegulares(int pecacodi, string empresa, string central, string grupo, string modo, string tipo, string fechaini, string fechafin, string tipocalculo)
        {
            string condicion = "";

            //DSH 30-06-2017 : Se actualizo por requerimiento
            //condicion = condicion + " AND pecacodi = " + pecacodi;

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

            if (!tipo.Equals("") && tipo != null)
            {
                condicion = condicion + " AND BE.SUBCAUSACODI = " + tipo;
            }

            if (!fechaini.Equals("") && fechaini != null)
            {
                condicion = condicion + " AND TRUNC(BE.CRCBEHORINI) >= TO_DATE('" + fechaini + "','DD/MM/YYYY') ";
            }

            if (!fechafin.Equals("") && fechafin != null)
            {
                condicion = condicion + " AND TRUNC(BE.CRCBEHORINI) <= TO_DATE('" + fechafin + "','DD/MM/YYYY') ";
            }

            if (!tipocalculo.Equals("") && tipocalculo != null)
            {
                condicion = condicion + " AND BE.CRCBETIPOCALC = '" + tipocalculo + "'";
            }
            List<VceCompBajaeficDTO> entitys = new List<VceCompBajaeficDTO>();
            string queryString = string.Format(helper.SqlListCompensacionesRegulares, pecacodi, condicion);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VceCompBajaeficDTO entity = new VceCompBajaeficDTO();

                    int iEmprNomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprNomb)) entity.Emprnomb = dr.GetString(iEmprNomb);

                    int iSubCausaDesc = dr.GetOrdinal(helper.Subcausadesc);
                    if (!dr.IsDBNull(iSubCausaDesc)) entity.Subcausadesc = dr.GetString(iSubCausaDesc);

                    int iGrupoNomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGrupoNomb)) entity.Gruponomb = dr.GetString(iGrupoNomb);

                    int iCrcbeHorIni = dr.GetOrdinal(helper.Crcbehorini);
                    if (!dr.IsDBNull(iCrcbeHorIni)) entity.Crcbehorini = dr.GetDateTime(iCrcbeHorIni);

                    int iCrcbeHorFin = dr.GetOrdinal(helper.Crcbehorfin);
                    if (!dr.IsDBNull(iCrcbeHorFin)) entity.Crcbehorfin = dr.GetDateTime(iCrcbeHorFin);

                    int iCrcbePotencia = dr.GetOrdinal(helper.Crcbepotencia);
                    if (!dr.IsDBNull(iCrcbePotencia)) entity.Crcbepotencia = dr.GetDecimal(iCrcbePotencia);

                    int iCrcbeConsumo = dr.GetOrdinal(helper.Crcbeconsumo);
                    if (!dr.IsDBNull(iCrcbeConsumo)) entity.Crcbeconsumo = dr.GetDecimal(iCrcbeConsumo);

                    int iCrcbeCvc = dr.GetOrdinal(helper.Crcbecvc);
                    if (!dr.IsDBNull(iCrcbeCvc)) entity.Crcbecvc = dr.GetDecimal(iCrcbeCvc);

                    int iCrcbeCvnc = dr.GetOrdinal(helper.Crcbecvnc);
                    if (!dr.IsDBNull(iCrcbeCvnc)) entity.Crcbecvnc = dr.GetDecimal(iCrcbeCvnc);

                    int iCrcbeCvt = dr.GetOrdinal(helper.Crcbecvt);
                    if (!dr.IsDBNull(iCrcbeCvt)) entity.Crcbecvt = dr.GetDecimal(iCrcbeCvt);

                    int iCrcbeCompensacion = dr.GetOrdinal(helper.Crcbecompensacion);
                    if (!dr.IsDBNull(iCrcbeCompensacion)) entity.Crcbecompensacion = dr.GetDecimal(iCrcbeCompensacion);

                    int iGrupoCodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupoCodi)) entity.Grupocodi = dr.GetInt32(iGrupoCodi);

                    int iSubcausaCodi = dr.GetOrdinal(helper.Subcausacodi);
                    if (!dr.IsDBNull(iSubcausaCodi)) entity.Subcausacodi = dr.GetInt32(iSubcausaCodi);

                    int iPecaCodi = dr.GetOrdinal(helper.Pecacodi);
                    if (!dr.IsDBNull(iPecaCodi)) entity.PecaCodi = dr.GetInt32(iPecaCodi);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public void ProcesarCompensacionRegular(int pecacodi)
        {
            //Obtenemos los valores del Periodo
            string queryPeriodo = string.Format(helper.SqlGetPeriodoCompensacion, pecacodi);
            DbCommand commandPeriodo = dbProvider.GetSqlStringCommand(queryPeriodo);

            decimal tc = 0;
            DateTime fecha = new DateTime();

            using (IDataReader drp = dbProvider.ExecuteReader(commandPeriodo))
            {
                while (drp.Read())
                {
                    tc = drp.GetDecimal(drp.GetOrdinal("PECATIPOCAMBIO"));
                    fecha = new DateTime(drp.GetInt32(drp.GetOrdinal("PERIANIO")), drp.GetInt32(drp.GetOrdinal("PERIMES")), 1);
                }
            }

            //ELIMINAMOS EL REGISTRO VCE_COMP_BAJAEFIC
            string queryDelete = "DELETE FROM VCE_COMP_BAJAEFIC WHERE pecacodi = " + pecacodi + " AND CRCBETIPOCALC IS NULL OR CRCBETIPOCALC <> 'M'";
            DbCommand commanDelete = dbProvider.GetSqlStringCommand(queryDelete);
            dbProvider.ExecuteNonQuery(commanDelete);

            //Variables a usar
            int calificacion = 0;
            int calificacionAnt = 0;
            float potencia = 0;
            float consumo = 0;
            float energia = 0;
            int intervalos = 0;
            float cvc = 0;
            float cvnc = 0;
            float cvt = 0;
            DateTime horIni = new DateTime();
            DateTime horFin = new DateTime();
            DateTime fechacontrol = Convert.ToDateTime("01/01/2000");
            DateTime fechaControlAnterior = Convert.ToDateTime("01/01/2000");

            float compensa = 0;

            //Obtenemos la lista de modos
            string queryModo = string.Format(helper.SqlListModoOperacion, pecacodi);
            DbCommand commandModo = dbProvider.GetSqlStringCommand(queryModo);

            //Obtenemos lista de mediciones
            using (IDataReader drm = dbProvider.ExecuteReader(commandModo))
            {
                while (drm.Read())
                {
                    horIni = new DateTime();
                    horFin = new DateTime();

                    int grupocodi = 0;

                    int iGrupoCodi = drm.GetOrdinal("GRUPOCODI");
                    if (!drm.IsDBNull(iGrupoCodi)) grupocodi = drm.GetInt32(iGrupoCodi);

                    if (grupocodi == 241)
                    {
                        Console.WriteLine("Valor grupocodi");
                    }

                    string queryMedicion = string.Format(helper.SqlListMedicion, pecacodi, grupocodi);
                    DbCommand commandMedicion = dbProvider.GetSqlStringCommand(queryMedicion);

                    using (IDataReader drr = dbProvider.ExecuteReader(commandMedicion))
                    {
                        while (drr.Read())
                        {
                            if (!drr.IsDBNull(drr.GetOrdinal("SUBCAUSACODI")))
                            {
                                calificacion = Convert.ToInt32(drr.GetValue(drr.GetOrdinal("SUBCAUSACODI")));
                            }
                            else
                            {
                                calificacion = 0;
                            }

                            // asignar hora de control
                            if (!drr.IsDBNull(drr.GetOrdinal("CRDETHORA")))
                            {
                                fechacontrol = drr.GetDateTime(drr.GetOrdinal("CRDETHORA"));
                                string strfecha = fechacontrol.ToString("dd/MM/yyyy");
                            }

                            //Cambio de calificación
                            //if (calificacionAnt != calificacion)
                            if ((calificacionAnt != calificacion) || (calificacion == 320 && fechaControlAnterior.ToString("dd/MM/yyyy") != "01/01/2000" && fechaControlAnterior.ToString("dd/MM/yyyy") != fechacontrol.ToString("dd/MM/yyyy")))
                            {
                                //Cerramos periodo anterior
                                if (calificacionAnt != 0)
                                {
                                    //Console.WriteLine("LLEGAMOS");

                                    // DSH 22/08/2017 : asgnar fecha para obtener datos de calculo
                                    int iFecha = drr.GetOrdinal("CRDETHORA");
                                    if (!drr.IsDBNull(iFecha))
                                    {
                                        fecha = drr.GetDateTime(iFecha);
                                        //horFinAnt = drr.GetDateTime(iFecha);
                                    }


                                    string queryCal = string.Format(helper.SqlGetDatCalculo, pecacodi, grupocodi, fecha.ToString("dd/MM/yyyy"));
                                    DbCommand commandCal = dbProvider.GetSqlStringCommand(queryCal);

                                    VceDatcalculoDTO entity = null;

                                    using (IDataReader dr = dbProvider.ExecuteReader(commandCal))
                                    {
                                        while (dr.Read())
                                        {
                                            entity = new VceDatcalculoDTO();

                                            int iCrdcgConsideraPotNom = dr.GetOrdinal("CRDCGCONSIDERAPOTNOM");
                                            if (!dr.IsDBNull(iCrdcgConsideraPotNom)) entity.Crdcgconsiderapotnom = dr.GetInt32(iCrdcgConsideraPotNom);

                                            int iCrdcgPotEfe = dr.GetOrdinal("CRDCGPOTEFE");
                                            if (!dr.IsDBNull(iCrdcgPotEfe)) entity.Crdcgpotefe = dr.GetDecimal(iCrdcgPotEfe);

                                            int iCrdcgCCPotEfe = dr.GetOrdinal("CRDCGCCPOTEFE");
                                            if (!dr.IsDBNull(iCrdcgCCPotEfe)) entity.Crdcgccpotefe = dr.GetDecimal(iCrdcgCCPotEfe);

                                            int iCrdcgPotMin = dr.GetOrdinal("CRDCGPOTMIN");
                                            if (!dr.IsDBNull(iCrdcgPotMin)) entity.Crdcgpotmin = dr.GetDecimal(iCrdcgPotMin);

                                            int iCrdcgPotPar1 = dr.GetOrdinal("CRDCGPOTPAR1");
                                            if (!dr.IsDBNull(iCrdcgPotPar1)) entity.Crdcgpotpar1 = dr.GetDecimal(iCrdcgPotPar1);

                                            int iCrdcgConCompp1 = dr.GetOrdinal("CRDCGCONCOMPP1");
                                            if (!dr.IsDBNull(iCrdcgConCompp1)) entity.Crdcgconcompp1 = dr.GetDecimal(iCrdcgConCompp1);

                                            int iCrdcgPotPar2 = dr.GetOrdinal("CRDCGPOTPAR2");
                                            if (!dr.IsDBNull(iCrdcgPotPar2)) entity.Crdcgpotpar2 = dr.GetDecimal(iCrdcgPotPar2);

                                            int iCrdcgConCompp2 = dr.GetOrdinal("CRDCGCONCOMPP2");
                                            if (!dr.IsDBNull(iCrdcgConCompp2)) entity.Crdcgconcompp2 = dr.GetDecimal(iCrdcgConCompp2);

                                            int iCrdcgPotPar3 = dr.GetOrdinal("CRDCGPOTPAR3");
                                            if (!dr.IsDBNull(iCrdcgPotPar3)) entity.Crdcgpotpar3 = dr.GetDecimal(iCrdcgPotPar3);

                                            int iCrdcgConCompp3 = dr.GetOrdinal("CRDCGCONCOMPP3");
                                            if (!dr.IsDBNull(iCrdcgConCompp3)) entity.Crdcgconcompp3 = dr.GetDecimal(iCrdcgConCompp3);

                                            int iCrdcgPotPar4 = dr.GetOrdinal("CRDCGPOTPAR4");
                                            if (!dr.IsDBNull(iCrdcgPotPar4)) entity.Crdcgpotpar4 = dr.GetDecimal(iCrdcgPotPar4);

                                            int iCrdcgConCompp4 = dr.GetOrdinal("CRDCGCONCOMPP4");
                                            if (!dr.IsDBNull(iCrdcgConCompp4)) entity.Crdcgconcompp4 = dr.GetDecimal(iCrdcgConCompp4);

                                            int iCrdcgPrecioAplic = dr.GetOrdinal("CRDCGPRECIOAPLIC");
                                            if (!dr.IsDBNull(iCrdcgPrecioAplic)) entity.Crdcgprecioaplic = dr.GetDecimal(iCrdcgPrecioAplic);

                                            int iCrdcgCVNCDol = dr.GetOrdinal("CRDCGCVNCDOL");
                                            if (!dr.IsDBNull(iCrdcgCVNCDol)) entity.Crdcgcvncdol = dr.GetDecimal(iCrdcgCVNCDol);

                                        }
                                    }

                                    if (entity != null)
                                    {
                                        if (entity.Crdcgconsiderapotnom == 1)
                                        {
                                            potencia = (float)entity.Crdcgpotefe;
                                            consumo = (float)entity.Crdcgccpotefe;
                                        }
                                        else
                                        {
                                            double div = 0.25;

                                            potencia = energia / (float)(intervalos * div);
                                            if (potencia < (float)entity.Crdcgpotmin)
                                            {
                                                potencia = (float)entity.Crdcgpotmin;
                                            }

                                            /*
                                            string query = "SELECT VCE_COMPENSACION_PKG.FNC_CALC_CONSUMO_COMB(" + potencia + ",";
                                            query = query + entity.Crdcgpotefe + ",";
                                            query = query + entity.Crdcgccpotefe + ",";
                                            query = query + entity.Crdcgpotpar1 + ",";
                                            query = query + entity.Crdcgconcompp1 + ",";
                                            query = query + entity.Crdcgpotpar2 + ",";
                                            query = query + entity.Crdcgconcompp2 + ",";
                                            query = query + entity.Crdcgpotpar3 + ",";
                                            query = query + entity.Crdcgconcompp3 + ",";
                                            query = query + entity.Crdcgpotpar4 + ",";
                                            query = query + entity.Crdcgconcompp4;
                                            query = query + ") AS VALOR FROM DUAL";
                                            */
                                            // DSH 09-06-2017  - Cambio segun requerimiento
                                            consumo = CalculaConsumoComb(potencia, entity.Crdcgpotefe, entity.Crdcgccpotefe, entity.Crdcgpotpar1, entity.Crdcgconcompp1, entity.Crdcgpotpar2, entity.Crdcgconcompp2,
                                                                              entity.Crdcgpotpar3, entity.Crdcgconcompp3, entity.Crdcgpotpar4, entity.Crdcgconcompp4);
                                        }

                                        /*string qCVC = "SELECT VCE_COMPENSACION_PKG.FNC_CALC_CVC(" + pecacodi + ",";
                                        qCVC = qCVC + grupocodi + ",";
                                        qCVC = qCVC + consumo + ",";
                                        qCVC = qCVC + potencia + ",";
                                        qCVC = qCVC + entity.Crdcgprecioaplic + ",";
                                        qCVC = qCVC + tc;
                                        qCVC = qCVC + ") AS VALOR FROM DUAL";
                                        */
                                        // DSH 13-06-2017  - Cambio seegun requerimiento 
                                        cvc = calculaCvc(pecacodi, grupocodi, consumo, potencia, entity.Crdcgprecioaplic, tc);

                                        cvnc = (float)(entity.Crdcgcvncdol * tc);
                                        cvt = cvc + cvnc;

                                        //ACTUALIZAMOS LOS PARÁMETROS DEL DETALLE DE COMPENSACIÓN
                                        string queryDet = "UPDATE VCE_COMP_REGULAR_DET SET CRDETCVTBAJAEFIC = " + cvt + ", CRDETCOMPENSACION = CASE WHEN " + cvt + " > CRDETCMG THEN (" + cvt + "-CRDETCMG)*CRDETVALOR*1000 ELSE 0 END";
                                        queryDet = queryDet + " WHERE pecacodi = " + pecacodi + " AND GRUPOCODI = " + grupocodi + " AND SUBCAUSACODI = " + calificacionAnt + " AND CRDETHORA BETWEEN TO_DATE('" + horIni.ToString("dd/MM/yyyy HH:mm") + "','DD/MM/YYYY HH24:MI') AND TO_DATE('" + horFin.ToString("dd/MM/yyyy HH:mm") + "','DD/MM/YYYY HH24:MI')";

                                        DbCommand commanDet = dbProvider.GetSqlStringCommand(queryDet);
                                        dbProvider.ExecuteNonQuery(commanDet);

                                        //CALCULAMOS LA COMPENSACION
                                        string qCom = "SELECT SUM(CRDETCOMPENSACION) AS VALOR FROM VCE_COMP_REGULAR_DET WHERE pecacodi = " + pecacodi + " AND GRUPOCODI = " + grupocodi + " AND SUBCAUSACODI = " + calificacionAnt + " AND CRDETHORA BETWEEN TO_DATE('" + horIni.ToString("dd/MM/yyyy HH:mm") + "','DD/MM/YYYY HH24:MI') AND TO_DATE('" + horFin.ToString("dd/MM/yyyy HH:mm") + "','DD/MM/YYYY HH24:MI')";
                                        DbCommand commandCom = dbProvider.GetSqlStringCommand(qCom);

                                        using (IDataReader dr = dbProvider.ExecuteReader(commandCom))
                                        {
                                            while (dr.Read())
                                            {
                                                int iValor = dr.GetOrdinal("VALOR");
                                                if (!dr.IsDBNull(iValor)) compensa = (float)dr.GetDecimal(iValor);
                                            }
                                        }

                                        //REGISTRAMOS EL PERIODO
                                        string queryPer = "INSERT INTO VCE_COMP_BAJAEFIC(pecacodi,GRUPOCODI,SUBCAUSACODI,CRCBEHORINI,CRCBEHORFIN,CRCBEPOTENCIA,CRCBECONSUMO,CRCBECVC,CRCBECVNC,CRCBECVT,CRCBECOMPENSACION)";
                                        queryPer = queryPer + " VALUES (" + pecacodi + "," + grupocodi + "," + calificacionAnt + ",TO_DATE('" + horIni.ToString("dd/MM/yyyy HH:mm") + "','DD/MM/YYYY HH24:MI'),TO_DATE('" + horFin.ToString("dd/MM/yyyy HH:mm") + "','DD/MM/YYYY HH24:MI')," + potencia + "," + consumo + "," + cvc + "," + cvnc + "," + cvt + "," + compensa + ")";

                                        DbCommand commanPer = dbProvider.GetSqlStringCommand(queryPer);
                                        dbProvider.ExecuteNonQuery(commanPer);
                                    }
                                } // fin de condicion (calificacionAnt != 0)

                                int iHorIni = drr.GetOrdinal("CRDETHORA");
                                if (!drr.IsDBNull(iHorIni)) horIni = drr.GetDateTime(iHorIni);

                                intervalos = 1;

                                int iEnergia = drr.GetOrdinal("CRDETVALOR");
                                if (!drr.IsDBNull(iEnergia))
                                {
                                    energia = (float)drr.GetDecimal(iEnergia);
                                }
                                else
                                {
                                    energia = 0;
                                }

                                int iHorFin = drr.GetOrdinal("CRDETHORA");
                                if (!drr.IsDBNull(iHorFin)) horFin = drr.GetDateTime(iHorFin);
                            } // fin de condicion (calificacionAnt != calificacion)
                            else
                            {
                                intervalos++;

                                int iEnergia = drr.GetOrdinal("CRDETVALOR");
                                if (!drr.IsDBNull(iEnergia))
                                {
                                    energia = energia + (float)drr.GetDecimal(iEnergia);
                                }
                                else
                                {
                                    energia = energia + 0;
                                }

                                int iHorFin = drr.GetOrdinal("CRDETHORA");
                                if (!drr.IsDBNull(iHorFin)) horFin = drr.GetDateTime(iHorFin);

                            }
                            calificacionAnt = calificacion;
                            fechaControlAnterior = horFin;
                        } // fin de lectura de bucle de medicion
                    }
                }
            }
        }

        private float CalculaConsumoComb(float v_potencia, decimal? v_p1, decimal? v_con1, decimal? v_p2, decimal? v_con2, 
                                         decimal? v_p3, decimal? v_con3, decimal? v_p4, decimal? v_con4, decimal? v_p5, decimal? v_con5)
        {
            float vresultado;
            float? vconsumo = null;
            float vpotencia = v_potencia;
            float vp1 = (float) (v_p1 ?? 0);
            float vcon1 = (float)(v_con1 ?? 0);
            float vp2 = (float)(v_p2 ?? 0);
            float vcon2 = (float)(v_con2 ?? 0);
            float vp3 = (float)(v_p3 ?? 0);
            float vcon3 = (float)(v_con3 ?? 0);
            float vp4 = (float)(v_p4 ?? 0);
            float vcon4 = (float)(v_con4 ?? 0);
            float vp5 = (float)(v_p5 ?? 0);
            float vcon5 = (float)(v_con5 ?? 0);

            if (vp2 > 0 && (vp2 < vpotencia) && (vpotencia <= vp1)){
                vconsumo = vcon2 + (vpotencia - vp2) * (vcon1 - vcon2) / (vp1 - vp2);
            }

            if (vp3 > 0 && (vp3 < vpotencia) && (vpotencia <= vp2))
            {
                vconsumo = vcon3 + (vpotencia - vp3) * (vcon2 - vcon3) / (vp2 - vp3);
            }

            if (vp4 > 0 && (vp4 < vpotencia) && (vpotencia <= vp3))
            {
                vconsumo = vcon4 + (vpotencia - vp4) * (vcon3 - vcon4) / (vp3 - vp4);
            }

            if (vp5 > 0 && (vp5 <= vpotencia) && (vpotencia <= vp4))
            {
                vconsumo = vcon5 + (vpotencia - vp5) * (vcon4 - vcon5) / (vp4 - vp5);
            }

            // para los valores que estan fuera del intervalo
            if (vconsumo.Equals(null))
            {
                if (v_p5 > 0) vconsumo = vcon5 + (vpotencia - vp5) * (vcon4 - vcon5) / (vp4 - vp5);
                else if (vp4 > 0) vconsumo = vcon4 + (vpotencia - vp4) * (vcon3 - vcon4) / (vp3 - vp4);
                else if (vp3 > 0) vconsumo = vcon3 + (vpotencia - vp3) * (vcon2 - vcon3) / (vp2 - vp3);
                else if (vp2 > 0) vconsumo = vcon2 + (vpotencia - vp2) * (vcon1 - vcon2) / (vp1 - vp2);

            }
            vresultado = vconsumo ?? 0;
            return vresultado;
        }
      
        private float calculaCvc(int pecacodi, int grupocodi, float consumo, float potencia, decimal? precioaplica, decimal tc)
        {
            float vcvc = 0;
            float vprecioaplica = (float)(precioaplica ?? 0);
            float vlhv = 0;
            int venergcodi=0;
            

            string qCom = "SELECT GR.FENERGCODI,DC.CRDCGLHV FROM PR_GRUPO GR JOIN VCE_DATCALCULO DC ON GR.GRUPOCODI = DC.GRUPOCODI";
            qCom = qCom + " WHERE DC.PECACODI = " + pecacodi + " AND DC.GRUPOCODI = "+ grupocodi + " AND TO_CHAR(DC.CRDCGFECMOD,'DD')='01'";
            DbCommand commandCom = dbProvider.GetSqlStringCommand(qCom);

            using (IDataReader dr = dbProvider.ExecuteReader(commandCom))
            {
                while (dr.Read())
                {
                    int iValor = dr.GetOrdinal("FENERGCODI");
                    if (!dr.IsDBNull(iValor)) venergcodi = dr.GetInt32(iValor);

                    iValor = dr.GetOrdinal("CRDCGLHV");
                    if (!dr.IsDBNull(iValor)) vlhv = (float)dr.GetDecimal(iValor);
                }
            }

            // calculo CVC
            if (potencia != 0)
            {
                // diesel y residual
                if (venergcodi == 3 || venergcodi == 4) vcvc = consumo * vprecioaplica / (potencia * 1000);
                // carbon
                else if(venergcodi == 5) vcvc = consumo * vprecioaplica / (potencia * 1000);
                // gas
                else if(venergcodi == 2) vcvc = consumo * vprecioaplica  * (vlhv / 1000) / (potencia * 1000000);
                else vcvc = 0;              
        
            }

            return vcvc;

        }


        public void DeleteCompensacionManual(int pecacodi)
        {
            string queryString = string.Format(helper.SqlDeleteCompensacionManual, pecacodi);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);
        }
        public void DeleteByVersion(int pecacodi)
        {
            string queryString = string.Format(helper.SqlDeleteByVersion, pecacodi);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteByVersionTipoCalculoAutomatico(int pecacodi)
        {
            string queryString = string.Format(helper.SqlDeleteByVersionTipoCalculoAutomatico, pecacodi);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);
        }

        public void SaveByEntity(VceCompBajaeficDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSaveManual);

            dbProvider.AddInParameter(command, helper.Pecacodi, DbType.Int32, entity.PecaCodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Subcausacodi, DbType.Int32, entity.Subcausacodi);
            dbProvider.AddInParameter(command, helper.Crcbehorini, DbType.DateTime, entity.Crcbehorini);
            dbProvider.AddInParameter(command, helper.Crcbehorfin, DbType.DateTime, entity.Crcbehorfin);
            dbProvider.AddInParameter(command, helper.Crcbepotencia, DbType.Decimal, entity.Crcbepotencia);
            dbProvider.AddInParameter(command, helper.Crcbeconsumo, DbType.Decimal, entity.Crcbeconsumo);
            dbProvider.AddInParameter(command, helper.Crcbecvc, DbType.Decimal, entity.Crcbecvc);
            dbProvider.AddInParameter(command, helper.Crcbecvnc, DbType.Decimal, entity.Crcbecvnc);
            dbProvider.AddInParameter(command, helper.Crcbecvt, DbType.Decimal, entity.Crcbecvt);
            dbProvider.AddInParameter(command, helper.Crcbecompensacion, DbType.Decimal, entity.Crcbecompensacion);
            dbProvider.AddInParameter(command, helper.Crcbetipocalc, DbType.String, entity.Crcbetipocalc);

            dbProvider.ExecuteNonQuery(command);
        }

        public void SaveFromOtherVersion(int pecacodiDestino, int pecacodiOrigen)
        {
            string queryString = string.Format(helper.SqlSaveFromOtherVersion, pecacodiDestino, pecacodiOrigen);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);
        }

        public List<VceCompBajaeficDTO> ListCompensacionOperacionInflexibilidad(int pecacodi)
        {
            List<VceCompBajaeficDTO> entitys = new List<VceCompBajaeficDTO>();

            string sql = String.Format(helper.SqlListCompensacionOperacionInflexibilidad, pecacodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VceCompBajaeficDTO entity = new VceCompBajaeficDTO();

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iMinimacarga = dr.GetOrdinal(helper.Minimacarga);
                    if (!dr.IsDBNull(iMinimacarga)) entity.Minimacarga = dr.GetDecimal(iMinimacarga);

                    int iPruebapr25 = dr.GetOrdinal(helper.Pruebapr25);
                    if (!dr.IsDBNull(iPruebapr25)) entity.Pruebapr25 = dr.GetDecimal(iPruebapr25);

                    int iPagocuenta = dr.GetOrdinal(helper.Pagocuenta);
                    if (!dr.IsDBNull(iPagocuenta)) entity.Pagocuenta = dr.GetDecimal(iPagocuenta);

                    int iTotalmodo = dr.GetOrdinal(helper.Totalmodo);
                    if (!dr.IsDBNull(iTotalmodo)) entity.Totalmodo = dr.GetDecimal(iTotalmodo);

                    int iTotalemp = dr.GetOrdinal(helper.Totalemp);
                    if (!dr.IsDBNull(iTotalemp)) entity.Totalemp = dr.GetDecimal(iTotalemp);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<VceCompBajaeficDTO> ListCompensacionOperacionSeguridad(int pecacodi)
        {
            List<VceCompBajaeficDTO> entitys = new List<VceCompBajaeficDTO>();

            string sql = String.Format(helper.SqlListCompensacionOperacionSeguridad, pecacodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VceCompBajaeficDTO entity = new VceCompBajaeficDTO();

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iSeguridad = dr.GetOrdinal(helper.Seguridad);
                    if (!dr.IsDBNull(iSeguridad)) entity.Seguridad = dr.GetDecimal(iSeguridad);

                    int iTotalemp = dr.GetOrdinal(helper.Totalemp);
                    if (!dr.IsDBNull(iTotalemp)) entity.Totalemp = dr.GetDecimal(iTotalemp);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }
        public List<VceCompBajaeficDTO> ListCompensacionOperacionRSF(int pecacodi)
        {
            List<VceCompBajaeficDTO> entitys = new List<VceCompBajaeficDTO>();

            string sql = String.Format(helper.SqlListCompensacionOperacionRSF, pecacodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VceCompBajaeficDTO entity = new VceCompBajaeficDTO();

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iRsf = dr.GetOrdinal(helper.Rsf);
                    if (!dr.IsDBNull(iRsf)) entity.Rsf = dr.GetDecimal(iRsf);

                    int iReservaesp = dr.GetOrdinal(helper.Reservaesp);
                    if (!dr.IsDBNull(iReservaesp)) entity.Reservaesp = dr.GetDecimal(iReservaesp);

                    int iTotalemp = dr.GetOrdinal(helper.Totalemp);
                    if (!dr.IsDBNull(iTotalemp)) entity.Totalemp = dr.GetDecimal(iTotalemp);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<VceCompBajaeficDTO> ListCompensacionRegulacionTension(int pecacodi)
        {
            List<VceCompBajaeficDTO> entitys = new List<VceCompBajaeficDTO>();

            string sql = String.Format(helper.SqlListCompensacionRegulacionTension, pecacodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VceCompBajaeficDTO entity = new VceCompBajaeficDTO();

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iTension = dr.GetOrdinal(helper.Tension);
                    if (!dr.IsDBNull(iTension)) entity.Tension = dr.GetDecimal(iTension);

                    int iTotalemp = dr.GetOrdinal(helper.Totalemp);
                    if (!dr.IsDBNull(iTotalemp)) entity.Totalemp = dr.GetDecimal(iTotalemp);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public IDataReader ListCompensacionOperacionMME(int pecacodi, List<EveSubcausaeventoDTO> subCausaEvento)
        {
            List<VceCompBajaeficDTO> entitys = new List<VceCompBajaeficDTO>();

            string columnasEvento = "";

            foreach (var subCausa in subCausaEvento)
            {
                columnasEvento += string.Format("SUM(CASE WHEN CE.SUBCAUSACODI = {0} THEN CE.CRDETCOMPENSACION END) AS \"C{0}\",", subCausa.Subcausacodi);
            }

            string sql = String.Format(helper.SqlListCompensacionOperacionMME, pecacodi, columnasEvento);


            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            IDataReader reader = dbProvider.ExecuteReader(command);
            
            return reader;
        }

        public IDataReader ListCompensacionDiarioMME(int pecacodi, List<PrGrupoDTO> listaGrupo)
        {
            List<VceCompBajaeficDTO> entitys = new List<VceCompBajaeficDTO>();

            string columnasGrupo = "";

            foreach (var grupo in listaGrupo)
            {
                columnasGrupo += string.Format(",SUM(CASE WHEN CT.GRUPOCODI = {0} THEN CT.CRDETCOMPENSACION ELSE 0 END) AS C{0}", grupo.Grupocodi);
            }

            string sql = string.Format(helper.SqlListCompensacionDiarioMME, pecacodi, columnasGrupo);


            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            IDataReader reader = dbProvider.ExecuteReader(command);

            return reader;
        }
    }
}
