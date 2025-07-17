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
    public class TransferenciaRentaCongestionRepository : RepositoryBase, ITransferenciaRentaCongestionRepository
    {
        public TransferenciaRentaCongestionRepository(string strConn)
            : base(strConn)
        {
        }

        TransferenciaRentaCongestionHelper helper = new TransferenciaRentaCongestionHelper();


        public List<TransferenciaRecalculoDTO> ListPeriodosRentaCongestion(int pericodi, int recacodi, int regIni, int regFin)
        {
            List<TransferenciaRecalculoDTO> entitys = new List<TransferenciaRecalculoDTO>();

            var consulta = helper.SqlListPeriodosRentaCongestion;
            var consultaWhere = string.Empty;

            if (pericodi > 0)
            {
                consultaWhere = string.Format(" WHERE RE.PERICODI = {0}  AND RE.RECACODI = {1} ", pericodi, recacodi);
            }

            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(consulta, consultaWhere, regFin, regIni));                   

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TransferenciaRecalculoDTO entity = new TransferenciaRecalculoDTO();

                    int iPERICODI = dr.GetOrdinal(helper.PERICODI);
                    if (!dr.IsDBNull(iPERICODI)) entity.PeriCodi = dr.GetInt32(iPERICODI);

                    int iRECACODI = dr.GetOrdinal(helper.RECACODI);
                    if (!dr.IsDBNull(iRECACODI)) entity.RecaCodi = dr.GetInt32(iRECACODI);

                    int iPERINOMBRE = dr.GetOrdinal(helper.PERINOMBRE);
                    if (!dr.IsDBNull(iPERINOMBRE)) entity.PeriNombre = dr.GetString(iPERINOMBRE);

                    int iRECANOMBRE = dr.GetOrdinal(helper.RECANOMBRE);
                    if (!dr.IsDBNull(iRECANOMBRE)) entity.RecaNombre = dr.GetString(iRECANOMBRE);

                    int iPERIUSERNAME = dr.GetOrdinal(helper.PERIUSERNAME);
                    if (!dr.IsDBNull(iPERIUSERNAME)) entity.PeriUserName = dr.GetString(iPERIUSERNAME);

                    int iRECAUSERNAME = dr.GetOrdinal(helper.RECAUSERNAME);
                    if (!dr.IsDBNull(iRECAUSERNAME)) entity.RecaUserName = dr.GetString(iRECAUSERNAME);

                    int iRECAESTADO = dr.GetOrdinal(helper.RECAESTADO);
                    if (!dr.IsDBNull(iRECAESTADO)) entity.RecaEstado = dr.GetString(iRECAESTADO);

                    int iPERIFECINS = dr.GetOrdinal(helper.PERIFECINS);
                    if (!dr.IsDBNull(iPERIFECINS)) entity.PeriFecIns = dr.GetDateTime(iPERIFECINS);

                    int iRECAFECACT = dr.GetOrdinal(helper.RECAFECACT);
                    if (!dr.IsDBNull(iRECAFECACT)) entity.RecaFecAct = dr.GetDateTime(iRECAFECACT);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #region Proceso Calculo Rentas Congestion

        public void CalcularRentasCongestionPeriodo(int pericodi, int recacodi, string nombreUsuario, int diasPeriodo)
        {
            var resCongestion = DeleteDatosRentaCongestion(pericodi, recacodi);
            var resEntrega = DeleteDatosEntrega(pericodi, recacodi);
            var resRetiro = DeleteDatosRetiro(pericodi, recacodi);
            var resPeriodo = DeleteDatosPeriodo(pericodi, recacodi);

            var perioaniomes = GetPeriodoMes(pericodi);
            var rcentdcodi = GetMaximoEntregaId();
            var rccretdcodi = GetMaximoRetiroId();
            var rcrpercodi = GetMaximoPeriodoId();

            var insertRegistrosEntrega = SaveDetalleEntrega(pericodi, recacodi, rcentdcodi, perioaniomes, diasPeriodo);
            var insertRegistrosRetiro = SaveDetalleRetiro(pericodi, recacodi, rccretdcodi, perioaniomes, diasPeriodo);
            var insertRegistrosPeriodo = SaveRentaPeriodo(pericodi, recacodi, rcrpercodi, nombreUsuario);
            var insertRegistrosCongestion = SaveRentaCongestionRetiro(pericodi, recacodi, nombreUsuario);
        }

        public int DeleteDatosRentaCongestion(int pericodi, int recacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlDeleteDatosRentaCongestion, pericodi, recacodi));

            //dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            //dbProvider.AddInParameter(command, helper.RECACODI, DbType.Int32, recacodi);           

            //command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlDeleteDatosRentaCongestion, pericodi, recacodi));

            var res = dbProvider.ExecuteNonQuery(command);

            return res;

            
        }
        public int DeleteDatosEntrega(int pericodi, int recacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlDeleteDatosRentaEntrega, pericodi, recacodi));
            var res = dbProvider.ExecuteNonQuery(command);

            return res;
        }
        public int DeleteDatosRetiro(int pericodi, int recacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlDeleteDatosRentaRetiro, pericodi, recacodi));
            var res = dbProvider.ExecuteNonQuery(command);

            return res;
        }
        public int DeleteDatosPeriodo(int pericodi, int recacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlDeleteDatosRentaPeriodo, pericodi, recacodi));
            var res = dbProvider.ExecuteNonQuery(command);

            return res;
        }

        public int DeleteDatosReparto(int pericodi, int recacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlDeleteDatosRentaReparto, pericodi, recacodi));
            var res = dbProvider.ExecuteNonQuery(command);

            return res;
        }

        public int DeleteDatosingresocompensacion(int pericodi, int recacodi, int cabcomcodi)
        {
            var consulta = string.Format(helper.SqlDeleteDatosIngresoCompensacion, pericodi, recacodi, cabcomcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(consulta);
            var res = dbProvider.ExecuteNonQuery(command);

            return res;
        }
        public int GetPeriodoMes(int pericodi)
        {
            string sqlQuery = string.Format(helper.SqlGetPeriodoMes, pericodi);

            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            var cant = 0;
            var resultado = dbProvider.ExecuteScalar(command);
            if (resultado != System.DBNull.Value)
            {
                cant = Int32.Parse(resultado.ToString());
            }
            return cant;
        }

        public int GetMaximoEntregaId()
        {
            int valor = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaximoEntregaId);
            //dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            //dbProvider.AddInParameter(command, helper.RECACODI, DbType.Int32, recacodi);

            var resultado = dbProvider.ExecuteScalar(command);
            if (resultado != System.DBNull.Value)
            {
                valor = Int32.Parse(resultado.ToString());
            }

            return valor;
        }
        public long GetMaximoRetiroId()
        {
            long valor = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaximoRetiroId);
            //dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            //dbProvider.AddInParameter(command, helper.RECACODI, DbType.Int32, recacodi);

            var resultado = dbProvider.ExecuteScalar(command);
            if (resultado != System.DBNull.Value)
            {
                valor = Int64.Parse(resultado.ToString());
            }

            return valor;
        }
        public int GetMaximoPeriodoId()
        {
            int valor = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaximoPeriodoId);
            //dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            //dbProvider.AddInParameter(command, helper.RECACODI, DbType.Int32, recacodi);

            var resultado = dbProvider.ExecuteScalar(command);
            if (resultado != System.DBNull.Value)
            {
                valor = Int32.Parse(resultado.ToString());
            }

            return valor;
        }

        public int GetMaximoIngresoCompensacionId()
        {
            int valor = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaximoIngresoCompensacionId);
            //dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            //dbProvider.AddInParameter(command, helper.RECACODI, DbType.Int32, recacodi);

            var resultado = dbProvider.ExecuteScalar(command);
            if (resultado != System.DBNull.Value)
            {
                valor = Int32.Parse(resultado.ToString());
            }

            return valor;
        }

        public int GetMaximoCabeceraCompensacionId(int pericodi)
        {
            int valor = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaximoCabeceraCompensacionId);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            //dbProvider.AddInParameter(command, helper.RECACODI, DbType.Int32, recacodi);

            var resultado = dbProvider.ExecuteScalar(command);
            if (resultado != System.DBNull.Value)
            {
                valor = Int32.Parse(resultado.ToString());
            }

            return valor;
        }

        public int GetMaximoRepartoId()
        {
            int valor = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaximoRepartoId);
            //dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            //dbProvider.AddInParameter(command, helper.RECACODI, DbType.Int32, recacodi);

            var resultado = dbProvider.ExecuteScalar(command);
            if (resultado != System.DBNull.Value)
            {
                valor = Int32.Parse(resultado.ToString());
            }

            return valor;
        }

        public List<int> GetPeriodoVersionReparto(int pericodi, int recacodi)
        {
            List<int> resultado = new List<int>();
            var consulta = string.Format(helper.SqlGetPeriodoVersionReparto, pericodi, recacodi);
            DbCommand command = dbProvider.GetSqlStringCommand(consulta);
            //dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            //dbProvider.AddInParameter(command, helper.RECACODI, DbType.Int32, recacodi);
            int cant = 0;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    int iQregistros = dr.GetOrdinal("PERICODI");
                    if (!dr.IsDBNull(iQregistros))
                    {
                        resultado.Add(dr.GetInt32(iQregistros));
                    }
                    else
                    {
                        resultado.Add(0);
                    }

                    iQregistros = dr.GetOrdinal("INGRSCVERSION");
                    if (!dr.IsDBNull(iQregistros))
                    {
                        resultado.Add(dr.GetInt32(iQregistros));
                    }
                    else
                    {
                        resultado.Add(0);
                    }
                }
            }
            return resultado;
        }

        public decimal GetTotalReparto(int pericodi, int recacodi)
        {
            decimal valor = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetTotalReparto);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.RECACODI, DbType.Int32, recacodi);

            var resultado = dbProvider.ExecuteScalar(command);
            if (resultado != System.DBNull.Value)
            {
                valor = decimal.Parse(resultado.ToString());
            }

            return valor;
        }

        public int SaveDetalleReparto(int rcrrndcodi,int pericodi, int recacodi, decimal rentaTotal, 
            string nombreUsuario,int porc_pericodi, int porc_version)
        {
            var lista = new object[] { rcrrndcodi, pericodi, recacodi, rentaTotal, nombreUsuario };
            var sqlQuery = string.Format(helper.SqlSaveDetalleReparto, lista);

            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            dbProvider.AddInParameter(command, "PORC_PERICODI", DbType.Int32, porc_pericodi);
            dbProvider.AddInParameter(command, "PORC_VERSION", DbType.Int32, porc_version);
            //dbProvider.AddInParameter(command, helper.PERIANIOMES, DbType.Int32, perioaniomes);


            var res = dbProvider.ExecuteNonQuery(command);

            return res;
        }

        public int SaveDetalleEntrega(int pericodi, int recacodi, int rcentdcodi, int perioaniomes, int diasPeriodo)
        {
            var sqlQuery = string.Format(helper.SqlSaveDetalleEntrega, rcentdcodi, perioaniomes, diasPeriodo);

            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.RECACODI, DbType.Int32, recacodi);
            //dbProvider.AddInParameter(command, helper.PERIANIOMES, DbType.Int32, perioaniomes);


            var res = dbProvider.ExecuteNonQuery(command);

            return res;
        }

        public int SaveDetalleRetiro(int pericodi, int recacodi, long rcretdcodi, int perioaniomes, int diasPeriodo)
        {
            var sqlQuery = string.Format(helper.SqlSaveDetalleRetiro, rcretdcodi, perioaniomes, diasPeriodo);

            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.RECACODI, DbType.Int32, recacodi);
            //dbProvider.AddInParameter(command, helper.PERIANIOMES, DbType.Int32, perioaniomes);


            var res = dbProvider.ExecuteNonQuery(command);

            return res;
        }

        public int SaveRentaPeriodo(int pericodi, int recacodi, int rcrpercodi, string nombreUsuario)
        {
            var sqlQuery = string.Format(helper.SqlSaveRentaPeriodo, rcrpercodi);

            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.RECACODI, DbType.Int32, recacodi);
            dbProvider.AddInParameter(command, "USUARIO", DbType.String, nombreUsuario);


            var res = dbProvider.ExecuteNonQuery(command);

            return res;
        }

        public int SaveRentaCongestionRetiro(int pericodi, int recacodi, string nombreUsuario)
        {
            var sqlQuery = string.Format(helper.SqlSaveRentaCongestionRetiro, nombreUsuario, pericodi, recacodi);

            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            //dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            //dbProvider.AddInParameter(command, helper.RECACODI, DbType.Int32, recacodi);
            //dbProvider.AddInParameter(command, "USUARIO", DbType.String, nombreUsuario);

            var res = dbProvider.ExecuteNonQuery(command);

            return res;
        }

        public int SaveRentaCongestionIngresoCompensacion(int pericodi, int recacodi, int ingcomcodi, int cabcomcodi, string nombreUsuario)
        {
            var lista = new object[] { ingcomcodi, cabcomcodi, pericodi, recacodi, nombreUsuario };
            var sqlQuery = string.Format(helper.SqlSaveRentaCongestionIngresoCompensacion, lista);

            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            //dbProvider.AddInParameter(command, "PORC_PERICODI", DbType.Int32, porc_pericodi);
            //dbProvider.AddInParameter(command, "PORC_VERSION", DbType.Int32, porc_version);
            //dbProvider.AddInParameter(command, helper.PERIANIOMES, DbType.Int32, perioaniomes);


            var res = dbProvider.ExecuteNonQuery(command);

            return res;
        }

        #endregion
        public decimal GetTotalRentaCongestion(int pericodi, int recacodi)
        {
            decimal valorRenta = decimal.Zero;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetTotalRentaCongestion);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.RECACODI, DbType.Int32, recacodi);

            var resultado = dbProvider.ExecuteScalar(command);
            if (resultado != System.DBNull.Value)
            {
                valorRenta = decimal.Parse(resultado.ToString());
            }

            return valorRenta;
        }

        public decimal GetTotalRentaNoAsignada(int pericodi, int recacodi)
        {
            decimal valorRenta = decimal.Zero;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetTotalRentaNoAsignada);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.RECACODI, DbType.Int32, recacodi);

            var resultado = dbProvider.ExecuteScalar(command);
            if (resultado != System.DBNull.Value)
            {
                valorRenta = decimal.Parse(resultado.ToString());
            }
            

            return valorRenta;
        }

        //Metodo Principal
        public List<TransferenciaRentaCongestionDTO> ListRentaCongestion(int pericodi, int recacodi)
        {
            List<TransferenciaRentaCongestionDTO> entitys = new List<TransferenciaRentaCongestionDTO>();

            string consulta = string.Format(helper.SqlListRentaCongestion, pericodi, recacodi);
            DbCommand command = dbProvider.GetSqlStringCommand(consulta);
            //dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            //dbProvider.AddInParameter(command, helper.RECACODI, DbType.Int32, recacodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = new TransferenciaRentaCongestionDTO();

                    int iEMPRNOMB = dr.GetOrdinal(helper.EMPRNOMB);
                    if (!dr.IsDBNull(iEMPRNOMB)) entity.EmprNombre = dr.GetString(iEMPRNOMB);

                    int iRCRENCRENTA = dr.GetOrdinal(helper.RCRENCRENTA);
                    if (!dr.IsDBNull(iRCRENCRENTA)) entity.Rcrencrenta = dr.GetDecimal(iRCRENCRENTA);

                    int iREPARTO = dr.GetOrdinal(helper.REPARTO);
                    if (!dr.IsDBNull(iREPARTO)) entity.Reparto = dr.GetDecimal(iREPARTO);

                    int iRENTATOTAL = dr.GetOrdinal(helper.RENTATOTAL);
                    if (!dr.IsDBNull(iRENTATOTAL)) entity.RentaTotal = dr.GetDecimal(iRENTATOTAL);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<TransferenciaRentaCongestionDTO> ListRentaCongestionDetalle(int pericodi, int recacodi)
        {
            List<TransferenciaRentaCongestionDTO> entitys = new List<TransferenciaRentaCongestionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListRentaCongestionDetalle);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.RECACODI, DbType.Int32, recacodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = new TransferenciaRentaCongestionDTO();

                    int iEMPRNOMB = dr.GetOrdinal(helper.EMPRNOMB);
                    if (!dr.IsDBNull(iEMPRNOMB)) entity.EmprNombre = dr.GetString(iEMPRNOMB);

                    int iEMPRNOMBRECLIENTE = dr.GetOrdinal(helper.EMPRNOMBRECLIENTE);
                    if (!dr.IsDBNull(iEMPRNOMBRECLIENTE)) entity.EmprNombreCliente = dr.GetString(iEMPRNOMBRECLIENTE);

                    int iBARRBARRATRANSFERENCIA = dr.GetOrdinal(helper.BARRBARRATRANSFERENCIA);
                    if (!dr.IsDBNull(iBARRBARRATRANSFERENCIA)) entity.BarrBarraTransferencia = dr.GetString(iBARRBARRATRANSFERENCIA);

                    int iTRETCODIGO = dr.GetOrdinal(helper.TRETCODIGO);
                    if (!dr.IsDBNull(iTRETCODIGO)) entity.TretCodigo = dr.GetString(iTRETCODIGO);

                    int iLICITACION = dr.GetOrdinal(helper.LICITACION);
                    if (!dr.IsDBNull(iLICITACION)) entity.Licitacion = dr.GetDecimal(iLICITACION);

                    int iBILATERAL = dr.GetOrdinal(helper.BILATERAL);
                    if (!dr.IsDBNull(iBILATERAL)) entity.Bilateral = dr.GetDecimal(iBILATERAL);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public int ListPeriodosRentaCongestionCount()
        {          

            DbCommand command = dbProvider.GetSqlStringCommand(this.helper.SqlPeriodosRentaCongestionCount);
            int cant = 0;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    int iQregistros = dr.GetOrdinal(helper.Qregistros);
                    cant = Convert.ToInt32(dr.GetValue(iQregistros));
                }
            }
            return cant;
        }

        public int GetTotalPorcentajes(int pericodi, int recacodi)
        {
            //string sqlQuery = string.Format(helper.SqlListTotalPorcentajes, pericodi);

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListTotalPorcentajes);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.RECACODI, DbType.Int32, recacodi);
            var cant = 0;
            var resultado = dbProvider.ExecuteScalar(command);
            if (resultado != System.DBNull.Value)
            {
                cant = Int32.Parse(resultado.ToString());
            }
            return cant;
        }

        public List<TransferenciaRentaCongestionDTO> ListErroresBarras(int pericodi, int recacodi, int perianiomes)
        {
            var sqlConsulta = string.Format(helper.SqlListErroresBarras, pericodi, recacodi, perianiomes);
            List<TransferenciaRentaCongestionDTO> entitys = new List<TransferenciaRentaCongestionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlConsulta);
            //dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            //dbProvider.AddInParameter(command, helper.RECACODI, DbType.Int32, recacodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = new TransferenciaRentaCongestionDTO();                                       

                    int iBARRBARRATRANSFERENCIA = dr.GetOrdinal(helper.BARRBARRATRANSFERENCIA);
                    if (!dr.IsDBNull(iBARRBARRATRANSFERENCIA)) entity.BarrBarraTransferencia = dr.GetString(iBARRBARRATRANSFERENCIA);

                    int iOBSERVACION = dr.GetOrdinal(helper.OBSERVACION);
                    if (!dr.IsDBNull(iOBSERVACION)) entity.Observacion = dr.GetString(iOBSERVACION);

                    int iFECHAOBSERVACION = dr.GetOrdinal(helper.FECHAOBSERVACION);
                    if (!dr.IsDBNull(iFECHAOBSERVACION)) entity.Fechaobservacion = dr.GetString(iFECHAOBSERVACION);                    

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public IDataReader ListCostosMarginales(int pericodi, int recacodi, int perianiomes)
        {
            List<TransferenciaRentaCongestionDTO> entitys = new List<TransferenciaRentaCongestionDTO>();
            var queryString = string.Format(helper.SqlListCostosMarginales, perianiomes, pericodi, recacodi);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            //dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            //dbProvider.AddInParameter(command, helper.RECACODI, DbType.Int32, recacodi);

           
            //DbCommand command = dbProvider.GetSqlStringCommand(sql);
            IDataReader reader = dbProvider.ExecuteReader(command);

            return reader;
        }

        public IDataReader ListEntregasRetiros(int pericodi, int recacodi,int perianiomes, int ultimoDia)
        {
            List<TransferenciaRentaCongestionDTO> entitys = new List<TransferenciaRentaCongestionDTO>();
            var queryString = string.Format(helper.SqlListEntregasRetiros, perianiomes, ultimoDia);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.RECACODI, DbType.Int32, recacodi);


            //DbCommand command = dbProvider.GetSqlStringCommand(sql);
            IDataReader reader = dbProvider.ExecuteReader(command);

            return reader;
        }

        public IDataReader ListCostosMarginalesPorBarra(int pericodi, int recacodi, int perianiomes)
        {
            List<TransferenciaRentaCongestionDTO> entitys = new List<TransferenciaRentaCongestionDTO>();
            var queryString = string.Format(helper.SqlListCostosMarginalesPorBarra, perianiomes, pericodi, recacodi);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            //dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            //dbProvider.AddInParameter(command, helper.RECACODI, DbType.Int32, recacodi);


            //DbCommand command = dbProvider.GetSqlStringCommand(sql);
            IDataReader reader = dbProvider.ExecuteReader(command);

            return reader;
        }

        public IDataReader ListTotalRegistrosCostosMarginales(int pericodi, int recacodi)
        {
            List<TransferenciaRentaCongestionDTO> entitys = new List<TransferenciaRentaCongestionDTO>();
            var queryString = helper.SqlListTotalRegistrosCostosMarginales;
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.RECACODI, DbType.Int32, recacodi);


            //DbCommand command = dbProvider.GetSqlStringCommand(sql);
            IDataReader reader = dbProvider.ExecuteReader(command);

            return reader;
        }

        public int DeleteDatosRcgCostoMarginalCab(int pericodi, int recacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteCostoMarginalCab);

            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.RECACODI, DbType.Int32, recacodi);           

            var res = dbProvider.ExecuteNonQuery(command);

            return res;


        }

        public int DeleteDatosRcgCostoMarginalDet(int rccmgccodi)
        {
            //DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlDeleteDatosRentaCongestion, pericodi, recacodi));
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteDatosRcgCostoMarginalDet);

            dbProvider.AddInParameter(command, helper.RCCMGCCODI, DbType.Int32, rccmgccodi);
            //dbProvider.AddInParameter(command, helper.RECACODI, DbType.Int32, recacodi);           

            //command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlDeleteDatosRentaCongestion, pericodi, recacodi));

            var res = dbProvider.ExecuteNonQuery(command);

            return res;


        }

        public int GetMaximoCostoMarginalDetalleId()
        {
            int valor = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaximoCostoMarginalDetalleId);
            //dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            //dbProvider.AddInParameter(command, helper.RECACODI, DbType.Int32, recacodi);

            var resultado = dbProvider.ExecuteScalar(command);
            if (resultado != System.DBNull.Value)
            {
                valor = Int32.Parse(resultado.ToString());
            }

            return valor;
        }

        public int SaveCostoMarginalDetalleSEV(int rccmgdcodi, int rccmgccodi, int perioaniomes)
        {
            var sqlQuery = string.Format(helper.SqlSaveCostoMarginalDetSEV, rccmgdcodi, rccmgccodi, perioaniomes);

            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            //dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            //dbProvider.AddInParameter(command, helper.RECACODI, DbType.Int32, recacodi);
            //dbProvider.AddInParameter(command, helper.PERIANIOMES, DbType.Int32, perioaniomes);


            var res = dbProvider.ExecuteNonQuery(command);

            return res;
        }

        
        public int SaveCostoMarginalDetalleCalculoAnterior(int rccmgdcodi, int rccmgccodi, int pericodi, int recacodiAnterior)
        {
            object[] listParametros = new object[]{ rccmgdcodi, rccmgccodi, pericodi, recacodiAnterior };

            var sqlQuery = string.Format(helper.SqlSaveCostoMarginalDetCalculoAnterior, listParametros);

            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            //dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            //dbProvider.AddInParameter(command, helper.RECACODI, DbType.Int32, recacodi);
            //dbProvider.AddInParameter(command, "USUARIO", DbType.String, nombreUsuario);

            var res = dbProvider.ExecuteNonQuery(command);

            return res;
        }

        public int ValidaCostoMarginal(int pericodi, int recacodi)
        {
            //string sqlQuery = string.Format(helper.SqlListTotalPorcentajes, pericodi);

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlValidarCostoMarginal);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.RECACODI, DbType.Int32, recacodi);
            var cant = 0;
            var resultado = dbProvider.ExecuteScalar(command);
            if (resultado != System.DBNull.Value)
            {
                cant = Int32.Parse(resultado.ToString());
            }
            return cant;
        }

        #region ASSETEC 202210 - Ajustar intervalos de 15 y 45 minutos.
        public void AjustarRGCEntregasRetiros(int rcentdcodi, long rccretdcodi, string sTrncmafecha, string sRcEntRetHora)
        {
            //dTrncmafecha > dRcEntRetHora
            //ENTREGAS
            object[] listParamEnt = new object[] { sRcEntRetHora, sTrncmafecha, rcentdcodi };
            var sqlQuery = string.Format(helper.SqlAjustarRGCEntregas, listParamEnt);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            dbProvider.ExecuteNonQuery(command);
            //RETIROS
            object[] listParamRet = new object[] { sRcEntRetHora, sTrncmafecha, rccretdcodi };
            sqlQuery = string.Format(helper.SqlAjustarRGCRetiros, listParamRet);
            command = dbProvider.GetSqlStringCommand(sqlQuery);
            dbProvider.ExecuteNonQuery(command);
        }

        public List<TransferenciaEntregaDTO> listRGCEntregas(int rcentdcodi)
        {
            List<TransferenciaEntregaDTO> entitys = new List<TransferenciaEntregaDTO>();
            string query = string.Format(helper.SqlListRGCEntregas, rcentdcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TransferenciaEntregaDTO entity = new TransferenciaEntregaDTO();

                    int iTentcodi = dr.GetOrdinal(helper.TENTCCODI);
                    if (!dr.IsDBNull(iTentcodi)) entity.TranEntrCodi = Convert.ToInt32(dr.GetValue(iTentcodi)); 

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<TransferenciaRetiroDTO> listRGCRetiros(long rccretdcodi)
        {
            List<TransferenciaRetiroDTO> entitys = new List<TransferenciaRetiroDTO>();
            string query = string.Format(helper.SqlListRGCRetiros, rccretdcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TransferenciaRetiroDTO entity = new TransferenciaRetiroDTO();

                    int iTretcodi = dr.GetOrdinal(helper.TRETCCODI);
                    if (!dr.IsDBNull(iTretcodi)) entity.TranRetiCodi = Convert.ToInt32(dr.GetValue(iTretcodi));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public void AjustarRGCEntregasDiaAnterior(int tentcodi, int pericodianterior, int versionanterior, string sTrncmafecha, string sRcEntRetHora)
        {
            //dTrncmafecha > dRcEntRetHora
            //ENTREGAS
            object[] listParamEnt = new object[] { tentcodi, pericodianterior, versionanterior, sRcEntRetHora, sTrncmafecha};
            var sqlQuery = string.Format(helper.SqlAjustarRGCEntregasDiaAterior, listParamEnt);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            dbProvider.ExecuteNonQuery(command);
        }

        public void AjustarRGCRetirosDiaAnterior(int tretcodi, int pericodianterior, int versionanterior, string sTrncmafecha, string sRcEntRetHora)
        {
            //RETIROS
            object[] listParamEnt = new object[] { tretcodi, pericodianterior, versionanterior, sRcEntRetHora, sTrncmafecha };
            var sqlQuery = string.Format(helper.SqlAjustarRGCRetirosDiaAterior, listParamEnt);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            dbProvider.ExecuteNonQuery(command);
        }

        public void UpdateRGCEntregasRetiros(int rcentdcodi, long rccretdcodi)
        {
            //ENTREGAS
            var sqlQuery = string.Format(helper.SqlUpdateRGCEntregas, rcentdcodi);
            DbCommand commandEntrega = dbProvider.GetSqlStringCommand(sqlQuery);
            dbProvider.ExecuteNonQuery(commandEntrega);
            //RETIROS
            sqlQuery = string.Format(helper.SqlUpdateRGCRetiros, rccretdcodi);
            DbCommand commandRetiro = dbProvider.GetSqlStringCommand(sqlQuery);
            dbProvider.ExecuteNonQuery(commandRetiro);
        }
        #endregion
    }

}
