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
    public class GmmDatCalculoRepository : RepositoryBase, IGmmDatCalculoRepository
    {
        public GmmDatCalculoRepository(string strConn)
           : base(strConn)
        {

        }

        GmmDatCalculoHelper helper = new GmmDatCalculoHelper();

        public void UpsertDatCalculo(GmmDatCalculoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpsertDatos);
            command = dbProvider.GetSqlStringCommand(helper.SqlUpsertDatos);

            //dbProvider.AddInParameter(command, helper.DCALCODI, DbType.Int32, entity.DCALCODI);
            dbProvider.AddInParameter(command, helper.TINSCODI, DbType.String, entity.TINSCODI);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, entity.PERICODI);
            dbProvider.AddInParameter(command, helper.EMPGCODI, DbType.Int32, entity.EMPGCODI);
            dbProvider.AddInParameter(command, helper.DCALVALOR, DbType.Decimal, entity.DCALVALOR);
            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, entity.EMPRCODI);
            dbProvider.AddInParameter(command, helper.BARRCODI, DbType.Int32, entity.BARRCODI);
            dbProvider.AddInParameter(command, helper.PTOMEDICODI, DbType.Int32, entity.PTOMEDICODI);
            dbProvider.AddInParameter(command, helper.TPTOMEDICODI, DbType.Int32, entity.TPTOMEDICODI);
            dbProvider.AddInParameter(command, helper.TIPOINFOCODI, DbType.Int32, entity.TIPOINFOCODI);
            dbProvider.AddInParameter(command, helper.MEDIFECHA, DbType.String, Convert.ToDateTime(entity.MEDIFECHA).ToString("yyyy-MM-dd"));
            dbProvider.AddInParameter(command, helper.LECTCODI, DbType.Int32, entity.LECTCODI);
            dbProvider.AddInParameter(command, helper.DCALANIO, DbType.Int32, entity.DCALANIO);
            dbProvider.AddInParameter(command, helper.DCALMES, DbType.Int32, entity.DCALMES);
            dbProvider.AddInParameter(command, helper.DCALUSUCREACION, DbType.String, entity.DCALUSUCREACION);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Deletevalores(GmmEmpresaDTO agente)
        {
            // Borrar energía para la empresa y el periodo
            string sqlDelete = string.Format(helper.SqlDeleteValoresEnergia, agente.EmprcodiCal, agente.PericodiCal);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlDelete);
            dbProvider.ExecuteNonQuery(command);
            // Borrar demanda para la empresa y el periodo
            sqlDelete = string.Format(helper.SqlDeleteValoresDemanda, agente.EmprcodiCal, agente.PericodiCal);
            command = dbProvider.GetSqlStringCommand(sqlDelete);
            dbProvider.ExecuteNonQuery(command);
        }
        public void DeletevaloresEntrega(GmmEmpresaDTO agente)
        {
            // Borrar energía para la empresa y el periodo
            string sqlDelete = string.Format(helper.SqlDeleteValoresEnergiaEntrega, agente.EmprcodiCal, agente.PericodiCal);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlDelete);
            dbProvider.ExecuteNonQuery(command);
        }

        public List<GmmDatCalculoDTO> ListarValores1Originales(GmmDatCalculoDTO datDC)
        {
            List<GmmDatCalculoDTO> entities = new List<GmmDatCalculoDTO>();
            int mes = datDC.DCALMES; int anio = datDC.DCALANIO;
            String per0 = anio.ToString() + (mes - 1).ToString();
            String per1 = anio.ToString() + (mes).ToString();
            String per2 = anio.ToString() + (mes + 1).ToString();
            String per3 = anio.ToString() + (mes + 2).ToString();
            if (mes >= 11)
            {
                switch (mes)
                {
                    case 11:
                        per3 = (anio + 1).ToString() + (1).ToString();
                        break;
                    case 12:
                        per2 = (anio + 1).ToString() + (1).ToString();
                        per3 = (anio + 1).ToString() + (2).ToString();
                        break;
                }

            }
            if (mes == 1)
            {
                per0 = (anio + -1).ToString() + (12).ToString();
            }

            int formato = datDC.formatoDemandaTrimestral;
            if (datDC.EmpgPrimerMes != true)
            {
                //per1 = "0";
                formato = datDC.formatoDemandaMensual;
            }

            string queryString = string.Format(helper.SqlGetValorDC, formato, datDC.EMPRCODI, datDC.PTOMEDICODI, per0, per1, per2, per3);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entities.Add(helper.CreateListaValores1Originales(dr));
                }
            }

            return entities;
        }

        public void SetPPO(int anio, int mes, int pericodi, String user)
        {
            string queryString = string.Format(helper.SqlSetPPO, anio, mes, user,
                pericodi, anio, mes, user);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);
        }

        public void SetValoresAdicionales(int anio, int mes, int pericodi, String user,
            decimal tipoCambio, decimal margenReserva, decimal totalInflex, decimal totalExceso)
        {
            string queryString = string.Format(helper.SqlSetValoresAdicionales, anio, mes,
                pericodi, user, "TCAMBIO", tipoCambio);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);

            queryString = string.Format(helper.SqlSetValoresAdicionales, anio, mes,
                pericodi, user, "MRESERVA", margenReserva);

            command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);

            queryString = string.Format(helper.SqlSetValoresAdicionales, anio, mes,
               pericodi, user, "TINFLEX", totalInflex);

            command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);

            queryString = string.Format(helper.SqlSetValoresAdicionales, anio, mes,
               pericodi, user, "TEXCESO", totalExceso);

            command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);
        }

        public void SetValorTipoCambio(int anio, int mes, int pericodi, String user)
        {

            int mes0 = mes -1; int anio0 = anio;
            if (mes == 1)
            {
                mes0 = 12;
                anio0 = anio - 1;
            }

            String stc = "(select pecatipocambio from vce_periodo_calculo where pericodi = (select pericodi from  trn_periodo where perianio = " + anio0 + " and perimes = " + mes0 + ") and pecanombre = 'Mensual')";


            string queryString = string.Format(helper.SqlSetValoresAdicionales, anio0, mes0,
                pericodi, user, "TCAMBIO", stc);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);
        }


        public void SetPFR(int anio, int mes, int pericodi, String user, int emprcodi)
        {
            int anio2 = anio; int mes2 = mes - 1;
            if (mes == 1) { mes2 = 12; anio2 = anio - 1; };

            string queryString = string.Format(helper.SqlSetPFR,
               anio, mes, pericodi, emprcodi, user, anio2, mes2);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);
        }
        public void SetLVTEA(int anio, int mes, int pericodi, String user, int emprcodi)
        {
            int anio2 = anio; int mes2 = mes - 1;
            if (mes == 1) { mes2 = 12; anio2 = anio - 1;}

            string queryString = string.Format(helper.SqlSetLVTEA, anio2, mes2, pericodi, emprcodi, user);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);
        }

        public void SetMEEN10(int anio, int mes, int pericodi, String user, int emprcodi)
        {
            string periodo = anio.ToString() + mes.ToString();
            string queryString = string.Format(helper.SqlSetMEEN10, 
                anio, mes, pericodi, emprcodi, user, periodo);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);
        }


        public void SetVTP(int anio, int mes, int pericodi, String user, int emprcodi)
        {
            int anio2 = anio; int mes2 = mes - 1;
            if (mes == 1) { mes2 = 12; anio2 = anio - 1; };

            string queryString = string.Format(helper.SqlSetVTP,
               anio, mes, pericodi, emprcodi, user, anio2, mes2);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);
        }

        public void SetPREG(int anio, int mes, int pericodi, String user, int emprcodi)
        {
            int anio2 = anio; int mes2 = mes - 1;
            if (mes == 1) { mes2 = 12; anio2 = anio - 1; }
            string queryString = string.Format(helper.SqlSetPREG, anio, mes,
            pericodi, emprcodi, user, anio2, mes2);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);
        }

        public void SetDemandaCOES(int anio, int mes, int pericodi, String user)
        {
            int anio2 = anio; int anio3 = anio; int mes2 = mes + 1; int mes3 = mes + 2;
            if (mes == 11) { mes3 = 1; anio3 = anio + 1; }
            if (mes == 12) { mes2 = 1; mes3 = 2; anio2 = anio + 1; anio3 = anio + 1; }

            // Día 1 y dia2 de cada mes
            string mesdia1 = ""; string mesdia30 = "";
            DateTime oPrimerDiaDelMes = new DateTime(anio, mes, 1);
            DateTime oUltimoDiaDelMes = oPrimerDiaDelMes.AddMonths(1).AddDays(-1);
            mesdia1 = oPrimerDiaDelMes.ToString("d/M/yyyy");
            mesdia30 = oUltimoDiaDelMes.ToString("d/M/yyyy");

            string mes2dia1 = ""; string mes2dia30 = "";
            DateTime oPrimerDiaDelMes2 = new DateTime(anio2, mes2, 1);
            DateTime oUltimoDiaDelMes2 = oPrimerDiaDelMes2.AddMonths(1).AddDays(-1);
            mes2dia1 = oPrimerDiaDelMes2.ToString("d/M/yyyy");
            mes2dia30 = oUltimoDiaDelMes2.ToString("d/M/yyyy");

            string mes3dia1 = ""; string mes3dia30 = "";
            DateTime oPrimerDiaDelMes3 = new DateTime(anio3, mes3, 1);
            DateTime oUltimoDiaDelMes3 = oPrimerDiaDelMes3.AddMonths(1).AddDays(-1);
            mes3dia1 = oPrimerDiaDelMes3.ToString("d/M/yyyy");
            mes3dia30 = oUltimoDiaDelMes3.ToString("d/M/yyyy");
            
            string periodo = anio.ToString() + mes.ToString();
            string queryString = string.Format(helper.SqlSetDemandaCOES, anio, mes,
                pericodi, periodo, user, anio, mes, mesdia1, mesdia30);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);

            periodo = anio2.ToString() + (mes2).ToString();
            queryString = string.Format(helper.SqlSetDemandaCOES, anio2, mes2,
                pericodi, periodo, user, anio, mes, mes2dia1, mes2dia30);

            command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);

            periodo = anio3.ToString() + (mes3).ToString();
            queryString = string.Format(helper.SqlSetDemandaCOES, anio3, mes3,
                pericodi, periodo, user, anio, mes, mes3dia1, mes3dia30);

            command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);
        }

        public void SetVDIO10(int anio, int mes, int pericodi, string user, int Emprcodi)
        {
            string desde = anio.ToString() + "-" + mes.ToString("00") + "-01";
            string hasta = anio.ToString() + "-" + mes.ToString("00") + "-10";
            string queryString = null;
            DbCommand command = null;

            // Servicios Complememtarios
            queryString = string.Format(helper.SqlSetVDSC10, anio, mes,
               pericodi, desde, hasta, Emprcodi, user);

            command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);

            // Inflexibilidad Operativa
            queryString = string.Format(helper.SqlSetVDIO10, anio, mes,
                pericodi, desde, hasta, Emprcodi, user);

            command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);
        }

        public void SetVDER10(int anio, int mes, int pericodi, string user, int Emprcodi)
        {
            string desde = anio.ToString() + "-" + mes.ToString("00") + "-01";
            string hasta = anio.ToString() + "-" + mes.ToString("00") + "-10";

            string queryString = string.Format(helper.SqlSetVDER10, anio, mes,
                pericodi, desde, hasta, Emprcodi, user);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);
        }
        public void SetMPE(int anio, int mes, int pericodi, string user, int Emprcodi)
        {

            int anio2 = anio; int mes2 = mes - 1;
            if (mes == 1) { mes2 = 12; anio2 = anio - 1;}
            string queryString = null;
            DbCommand command = null;

            queryString = string.Format(helper.SqlSetMPE, anio, mes,
            pericodi, Emprcodi, user, anio2, mes2);
            command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);

        }
        public void SetENRG10(int anio, int mes, int pericodi, string user, int Emprcodi)
        {
            string periodo = anio.ToString() + mes.ToString();
            string queryString = string.Format(helper.SqlDelENRG10,
                anio, mes, pericodi, Emprcodi);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);

            queryString = string.Format(helper.SqlSetENRG10,
                anio, mes, pericodi,
                periodo, Emprcodi, user);

            command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);

        }
        public void SetENRG11(int anio, int mes, int pericodi, string user, int Emprcodi)
        {
            int anio2 = anio; int mes2 = mes + 1; int anio3 = anio; int mes3 = mes + 2;
            if (mes == 11) { anio3 = anio + 1; mes3 = 1; }
            if (mes == 12) { mes2 = 1; anio2 = anio + 1; anio3 = anio2; mes3 = 2; }

            string periodo = anio.ToString() + mes.ToString();
            string periodo2 = anio2.ToString() + mes2.ToString();
            string periodo3 = anio3.ToString() + mes3.ToString();

            string queryString = string.Format(helper.SqlDelENRG11,
                anio, mes, pericodi, Emprcodi);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);

            periodo = anio.ToString() + mes.ToString();
            queryString = string.Format(helper.SqlSetENRG11, anio, mes,
                pericodi, periodo, Emprcodi, user);
            command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);

            queryString = string.Format(helper.SqlDelENRGN1,
                pericodi, Emprcodi);
            command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);

            // Energia prevista a retirar de cada mes
            queryString = string.Format(helper.SqlSetENRGN1, anio, mes,
                pericodi, periodo, Emprcodi, user);
            command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);

            periodo = anio2.ToString() + mes2.ToString();
            queryString = string.Format(helper.SqlSetENRGN1, anio2, mes2,
                pericodi, periodo2, Emprcodi, user);
            command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);

            periodo = anio2.ToString() + mes2.ToString();
            queryString = string.Format(helper.SqlSetENRGN1, anio3, mes3,
                pericodi, periodo3, Emprcodi, user);
            command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);

        }

        public void SetPerimsjpaso1(int pericodi, string user)
        {
            string fechaProceso = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

            string queryString = string.Format(helper.SqlSetPerimsjpaso1, fechaProceso, user, pericodi);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);
        }

        public void SetPerimsjpaso2(int pericodi, string user)
        {
            string fechaProceso = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

            string queryString = string.Format(helper.SqlSetPerimsjpaso2, fechaProceso, user, pericodi);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);
        }

        public void SetPerimsjpaso3(int pericodi, string user)
        {
            string fechaProceso = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

            string queryString = string.Format(helper.SqlSetPerimsjpaso3, fechaProceso, user, pericodi);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);
        }

        public void SetPeriEstado(int pericodi, string periestado, string user)
        {
            string fechaProceso = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            string estado = "";
            if (periestado.Equals("A"))
                estado = "Abierto";
            else
                estado = "Cerrado";

            string queryString = string.Format(helper.SqlSetPeriEstado, fechaProceso, periestado, user, pericodi, estado);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);
        }

        public void SetGarantiaEnergia(int anio, int mes, int pericodi, bool primerMes, int Emprcodi, int Empgcodi, String user)
        {

            string queryString = null;
            DbCommand command = null;
            // Limpiar registros de cálculo para la empresa
            queryString = string.Format(helper.SqlCleanGarantia,
                   pericodi, Empgcodi);
            command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command); 

            int mes2 = mes + 1;
            if (mes == 12) mes2 = 1;

            if (primerMes) {
                queryString = string.Format(helper.SqlSetGarantiaEnergia1mes,
                    pericodi, Emprcodi, Empgcodi, user);
                command = dbProvider.GetSqlStringCommand(queryString);
                dbProvider.ExecuteNonQuery(command);
                // Completar calculo
                queryString = string.Format(helper.SqlSetGarantiaEnergia1mesComp,
                   pericodi, Empgcodi, user);
                command = dbProvider.GetSqlStringCommand(queryString);
                dbProvider.ExecuteNonQuery(command);
            }
            else {
                queryString = string.Format(helper.SqlSetGarantiaEnergia, 
                pericodi, Emprcodi, Empgcodi, mes, mes2);
                command = dbProvider.GetSqlStringCommand(queryString);
                dbProvider.ExecuteNonQuery(command);
                // Completar calculo
                queryString = string.Format(helper.SqlSetGarantiaEnergiaComp,
                  pericodi, Empgcodi, user);
                command = dbProvider.GetSqlStringCommand(queryString);
                dbProvider.ExecuteNonQuery(command);

            }

        }
        public void SetGarantiaPotenciaPeaje(int anio, int mes, int pericodi, bool primerMes, int Emprcodi, int Empgcodi, String user)
        {
            string queryString = null;
            DbCommand command = null;
            // Limpiar registros de cálculo para la empresa
            queryString = string.Format(helper.SqlCleanPotencia,
                   pericodi, Empgcodi);
            command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);
            // mESES 1 , 2, 3
            int anio2 = anio; int anio3 = anio; int mes2 = mes + 1; int mes3 = mes + 2;
            if (mes == 11) { mes3 = 1; anio3 = anio + 1; }
            if (mes == 12) { mes2 = 1; mes3 = 2; anio2 = anio + 1; anio3 = anio + 1; }

            if (primerMes)
            {
                queryString = string.Format(helper.SqlSetGarantiaPotenciaPeaje1mes,
                pericodi, Emprcodi, Empgcodi, anio, mes, anio2, mes2, anio3, mes3);
                command = dbProvider.GetSqlStringCommand(queryString);
                dbProvider.ExecuteNonQuery(command);
                // Totalizar
                queryString = string.Format(helper.SqlSetGarantiaPotenciaPeaje1mesComp,
                pericodi, Empgcodi, user);
                command = dbProvider.GetSqlStringCommand(queryString);
                dbProvider.ExecuteNonQuery(command);

            }
            else
            {
                queryString = string.Format(helper.SqlSetGarantiaPotenciaPeaje,
                 pericodi, Emprcodi, Empgcodi, anio, mes, anio2, mes2);
                command = dbProvider.GetSqlStringCommand(queryString);
                dbProvider.ExecuteNonQuery(command);
                // Totalizar
                queryString = string.Format(helper.SqlSetGarantiaPotenciaPeajeComp,
                pericodi, Empgcodi, user);
                command = dbProvider.GetSqlStringCommand(queryString);
                dbProvider.ExecuteNonQuery(command);

            }

        }
        public void SetGarantiaServiciosComp(int anio, int mes, int pericodi, bool primerMes, int Emprcodi, int Empgcodi, String user)
        {

            string queryString = null;
            DbCommand command = null;
            int anio2 = anio; int mes2 = mes + 1; int anio0 = anio; int mes0 = mes - 1;
            int anio3 = anio; int mes3 = mes + 2; 


            if (mes == 1) { mes0 = 12; anio0 = anio - 1; }
            if (mes == 12) { mes2 = 1; anio2 = anio + 1; mes3 = 2; anio3 = anio + 1; }

            int diasMes = DateTime.DaysInMonth(anio, mes);


            // Limpiar registros de cálculo para la empresa
            queryString = string.Format(helper.SqlCleanServicioComp,
                   pericodi, Empgcodi);
            command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);

            if (primerMes)
            {
                //queryString = string.Format(helper.SqlSetGarantiaServiciosComp1mes, anio, mes,
                //pericodi, Emprcodi, user);
                queryString = string.Format(helper.SqlSetGarantiaServiciosComp1mes,
                pericodi, Emprcodi, Empgcodi, anio, mes, anio2, mes2, anio3, mes3);
                command = dbProvider.GetSqlStringCommand(queryString);
                dbProvider.ExecuteNonQuery(command);
                // Totalizar
                queryString = string.Format(helper.SqlSetGarantiaServiciosCompComp,
                pericodi, Empgcodi, user);
                command = dbProvider.GetSqlStringCommand(queryString);
                dbProvider.ExecuteNonQuery(command);
            }
            else
            {
                queryString = string.Format(helper.SqlSetGarantiaServiciosComp, 
                pericodi, Emprcodi, Empgcodi, anio0, mes0, anio2, mes2, anio, mes, diasMes);
                command = dbProvider.GetSqlStringCommand(queryString);
                dbProvider.ExecuteNonQuery(command);
                // Totalizar
                queryString = string.Format(helper.SqlSetGarantiaServiciosCompComp,
                pericodi, Empgcodi, user);
                command = dbProvider.GetSqlStringCommand(queryString);
                dbProvider.ExecuteNonQuery(command);
            }

        }

        public void SetGarantiaEnergiaReactiva(int anio, int mes, int pericodi, bool primerMes, int Emprcodi, int Empgcodi, String user)
        {
            int anio2 = anio; int anio3 = anio; int mes2 = mes + 1; int mes3 = mes + 2;
            int mes0 = mes - 1; int anio0 = anio;
            if (mes == 11) { mes2 = 12; mes3 = 1; anio3 = anio + 1; }
            if (mes == 12) { mes2 = 1; mes3 = 2; anio2 = anio + 1; anio3 = anio + 1; }

            if (mes == 1) { mes0 = 12; anio0 = anio - 1;}

            string queryString = null;
            DbCommand command = null;

            int diasMes = DateTime.DaysInMonth(anio, mes);

            // Limpiar registros de cálculo para la empresa
            queryString = string.Format(helper.SqlCleanEnergiaReactiva,
                   pericodi, Empgcodi);
            command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);

            if (primerMes)
            {
                queryString = string.Format(helper.SqlSetGarantiaEnergiaReactiva1mes,
                pericodi, Emprcodi, Empgcodi, anio, mes, anio2, mes2, anio3, mes3);
                command = dbProvider.GetSqlStringCommand(queryString);
                dbProvider.ExecuteNonQuery(command);
                // Totalizar
                queryString = string.Format(helper.SqlSetGarantiaEnergiaReactiva1mesComp,
                pericodi, Empgcodi, user);
                command = dbProvider.GetSqlStringCommand(queryString);
                dbProvider.ExecuteNonQuery(command);
            }
            else
            {
                queryString = string.Format(helper.SqlSetGarantiaEnergiaReactiva,
                pericodi, Emprcodi, Empgcodi, anio0, mes0, anio2, mes2, anio, mes, diasMes);
                command = dbProvider.GetSqlStringCommand(queryString);
                dbProvider.ExecuteNonQuery(command);
                // Totalizar
                queryString = string.Format(helper.SqlSetGarantiaEnergiaReactivaComp,
                pericodi, Empgcodi, user);
                command = dbProvider.GetSqlStringCommand(queryString);
                dbProvider.ExecuteNonQuery(command);
            }

        }

        public void SetGarantiainflexibilidadOpe(int anio, int mes, int pericodi, bool primerMes, int Emprcodi, int Empgcodi, String user)
        {

            int anio2 = anio; int anio3 = anio; int mes2 = mes + 1; int mes3 = mes + 2;
            int mes0 = mes - 1; int anio0 = anio;
            if (mes == 11) { mes3 = 1; anio3 = anio + 1; }
            if (mes == 12) { mes2 = 1; mes3 = 2; anio2 = anio + 1; anio3 = anio + 1; }

            if (mes == 1) { mes0 = 12; anio0 = anio - 1; }

            string queryString = null;
            DbCommand command = null;

            int diasMes = DateTime.DaysInMonth(anio, mes);

            // Limpiar registros de cálculo para la empresa
            queryString = string.Format(helper.SqlCleaninflexibilidadOpe,
                   pericodi, Empgcodi);
            command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);

            if (primerMes)
            {
                queryString = string.Format(helper.SqlSetGarantiainflexibilidadOpe1mes,
                pericodi, Emprcodi, Empgcodi, anio, mes, anio2, mes2, anio3, mes3);
                command = dbProvider.GetSqlStringCommand(queryString);
                dbProvider.ExecuteNonQuery(command);
                // Totalizar
                queryString = string.Format(helper.SqlSetGarantiainflexibilidadOpe1mesComp,
                pericodi, Empgcodi, user);
                command = dbProvider.GetSqlStringCommand(queryString);
                dbProvider.ExecuteNonQuery(command);
            }
            else
            {
                queryString = string.Format(helper.SqlSetGarantiainflexibilidadOpe,
                pericodi, Emprcodi, Empgcodi, anio0, mes0, anio2, mes2, anio, mes, diasMes);
                command = dbProvider.GetSqlStringCommand(queryString);
                dbProvider.ExecuteNonQuery(command);
                // Totalizar
                queryString = string.Format(helper.SqlSetGarantiainflexibilidadOpeComp,
                pericodi, Empgcodi, user);
                command = dbProvider.GetSqlStringCommand(queryString);
                dbProvider.ExecuteNonQuery(command);
            }

            
        }
        //
        public List<GmmDatCalculoDTO> listarRptEnergia(int anio, int mes)
        {
            List<GmmDatCalculoDTO> entities = new List<GmmDatCalculoDTO>();
            string queryString = string.Format(helper.SqllistarRptEnergia, anio, mes);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    GmmDatCalculoDTO gmmRpt = helper.CreateListaRptEnergia(dr);
                    entities.Add(gmmRpt);
                }
            }
            return entities;
        }

        public List<GmmDatCalculoDTO> listarRptInsumo(int anio, int mes)
        {
            List<GmmDatCalculoDTO> entities = new List<GmmDatCalculoDTO>();
            string queryString = string.Format(helper.SqllistarRptInsumo, anio, mes);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    GmmDatCalculoDTO gmmRpt = helper.CreateListaRptInsumo(dr);
                    entities.Add(gmmRpt);
                }
            }
            return entities;
        }

        public List<GmmDatCalculoDTO> listarRpt1(int anio, int mes)
        {
            string sPeriodo = anio.ToString();
            if (mes < 10) sPeriodo += "0" + mes.ToString();
            else
                sPeriodo += mes.ToString();

            List<GmmDatCalculoDTO> entities = new List<GmmDatCalculoDTO>();
            string queryString = string.Format(helper.SqllistarRpt1, anio, mes, sPeriodo);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    GmmDatCalculoDTO gmmRpt = helper.CreateListaRpt1(dr);
                    entities.Add(gmmRpt);
                }
            }
            return entities;
        }

        public List<GmmDatCalculoDTO> listarRpt2(int anio, int mes)
        {
            List<GmmDatCalculoDTO> entities = new List<GmmDatCalculoDTO>();
            string queryString = string.Format(helper.SqllistarRpt2, anio, mes);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    GmmDatCalculoDTO gmmRpt = helper.CreateListaRpt2(dr);
                    entities.Add(gmmRpt);

                }
            }
            return entities;
        }

        public List<GmmDatCalculoDTO> listarRpt3(int anio, int mes)
        {
            List<GmmDatCalculoDTO> entities = new List<GmmDatCalculoDTO>();
            string queryString = string.Format(helper.SqllistarRpt3, anio, mes);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    GmmDatCalculoDTO gmmRpt = helper.CreateListaRpt3(dr);
                    entities.Add(gmmRpt);

                }
            }
            return entities;
        }
        public List<GmmDatCalculoDTO> listarRpt4(int anio, int mes)
        {
            List<GmmDatCalculoDTO> entities = new List<GmmDatCalculoDTO>();
            string queryString = string.Format(helper.SqllistarRpt4, anio, mes);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    GmmDatCalculoDTO gmmRpt = helper.CreateListaRpt4(dr);
                    entities.Add(gmmRpt);

                }
            }
            return entities;
        }
        public List<GmmDatCalculoDTO> listarRpt5(int anio, int mes)
        {
            List<GmmDatCalculoDTO> entities = new List<GmmDatCalculoDTO>();
            string queryString = string.Format(helper.SqllistarRpt5, anio, mes);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    GmmDatCalculoDTO gmmRpt = helper.CreateListaRpt5(dr);
                    entities.Add(gmmRpt);

                }
            }
            return entities;
        }
        public List<GmmDatCalculoDTO> listarRpt6(int anio, int mes)
        {
            List<GmmDatCalculoDTO> entities = new List<GmmDatCalculoDTO>();
            string queryString = string.Format(helper.SqllistarRpt6, anio, mes);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    GmmDatCalculoDTO gmmRpt = helper.CreateListaRpt6(dr);
                    entities.Add(gmmRpt);

                }
            }
            return entities;
        }

        public void SetEntPre(int anio, int mes, int pericodi, string user, int emprcodi)
        {
            int anio2 = anio; int mes2 = mes + 1; int anio3 = anio; int mes3 = mes + 2;
            if (mes == 11) { anio3 = anio + 1; mes3 = 1; }
            if (mes == 12) { mes2 = 1; anio2 = anio + 1; anio3 = anio2; mes3 = 2; }

            string periodo = anio.ToString() + mes.ToString();
            string periodo2 = anio2.ToString() + mes2.ToString();
            string periodo3 = anio3.ToString() + mes3.ToString();

            string queryString = string.Format(helper.SqlDelENTPRE,
                anio, mes, pericodi, emprcodi);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);

            periodo = anio.ToString() + mes.ToString();
            
            // Energia prevista a entregar de cada mes
            queryString = string.Format(helper.SqlSetENTPRE, anio, mes,
                pericodi, periodo, emprcodi, user);
            command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);

            periodo = anio2.ToString() + mes2.ToString();
            queryString = string.Format(helper.SqlSetENTPRE, anio2, mes2,
                pericodi, periodo2, emprcodi, user);
            command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);

            periodo = anio2.ToString() + mes2.ToString();
            queryString = string.Format(helper.SqlSetENTPRE, anio3, mes3,
                pericodi, periodo3, emprcodi, user);
            command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);
        }
    }
}