using COES.Base.Core;
using COES.Dominio.DTO.ReportesFrecuencia;
using COES.Dominio.Interfaces.ReportesFrecuencia;
using COES.Infraestructura.Datos.Helper.ReportesFrecuencia;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Repositorio.ReportesFrecuencia
{
    public class ReporteFrecuenciaRepository : RepositoryBase, IReporteFrecuenciaRepository
    {
        public ReporteFrecuenciaRepository(string strConn) : base(strConn)
        {
        }
        ReporteFrecuenciaHelper helper = new ReporteFrecuenciaHelper();

        public List<ReporteFrecuenciaDescargaDTO> ObtenerFrecuencias(ReporteFrecuenciaParam param)
        {
            List<ReporteFrecuenciaDescargaDTO> lista = new List<ReporteFrecuenciaDescargaDTO>();
            try
            {
                var query = string.Format(helper.SqlGetFrecuencias, param.IdGPS,
                                            param.FechaInicial.ToString("dd/MM/yyyy HH:mm:ss"),
                                            param.FechaFinal.ToString("dd/MM/yyyy HH:mm:ss"));

                DbCommand command = dbProvider.GetSqlStringCommand(query);
                //dbProvider.AddInParameter(command, "GPSCODI", DbType.Int32, param.IdGPS);
                //dbProvider.AddInParameter(command, "FECHAINI", DbType.DateTime, param.FechaInicial);
                //dbProvider.AddInParameter(command, "FECHAFIN", DbType.DateTime, param.FechaFinal);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        lista.Add(helper.Create(dr));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lista;
        }

        public List<ReporteFrecuenciaDescargaDTO> ObtenerFrecuenciasMinuto(ReporteFrecuenciaParam param)
        {
            List<ReporteFrecuenciaDescargaDTO> lista = new List<ReporteFrecuenciaDescargaDTO>();
            try
            {
                var query = string.Format(helper.SqlGetFrecuenciasMinuto, param.IdGPS,
                                            param.FechaInicial.ToString("dd/MM/yyyy HH:mm:ss"),
                                            param.FechaFinal.ToString("dd/MM/yyyy HH:mm:ss"));

                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        lista.Add(helper.CreateFrecMinuto(dr));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lista;
        }

        public List<int> ObtenerGPSPorRangoFechas(ReporteFrecuenciaParam param)
        {
            List<int> Ids = new List<int>();
            try
            {
                //var query = string.Format(helper.SqlGetGPSPorRangoFechas, param.FechaInicial.ToString("dd/MM/yyyy HH:mm:ss"),param.FechaFinal.ToString("dd/MM/yyyy HH:mm:ss"));
                var cadena = @"SELECT GPSCODI FROM sic.F_LECTURA WHERE FECHAHORA BETWEEN to_date('{0}', 'DD/MM/YYYY HH24:MI:SS') AND to_date('{1}', 'DD/MM/YYYY HH24:MI:SS') GROUP BY GPSCODI;";
                var query = string.Format(cadena, param.FechaInicial.ToString("dd/MM/yyyy HH:mm:ss"), param.FechaFinal.ToString("dd/MM/yyyy HH:mm:ss"));
                DbCommand command = dbProvider.GetSqlStringCommand(query);
                //dbProvider.AddInParameter(command, "FECHAINI", DbType.DateTime, param.FechaInicial);
                //dbProvider.AddInParameter(command, "FECHAFIN", DbType.DateTime, param.FechaFinal);

                //dbProvider.AddInParameter(command, "FECHAINI", DbType.String, param.FechaInicial.ToString("dd/MM/yyyy HH:mm:ss"));
                //dbProvider.AddInParameter(command, "FECHAFIN", DbType.String, param.FechaFinal.ToString("dd/MM/yyyy HH:mm:ss"));

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        Ids.Add(Convert.ToInt32(dr["GPSCODI"]));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Ids;
        }

        public object ObtenerSQL(string query, string Tipo)
        {
            object Retorno = null;
            try
            {
                DbCommand command = dbProvider.GetSqlStringCommand(query);

                switch (Tipo)
                {
                    case "O":
                        Retorno = dbProvider.ExecuteScalar(command);
                        break;
                    default:
                        Retorno = new DataSet();
                        dbProvider.LoadDataSet(command, (DataSet)Retorno, Tipo);
                        break;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Retorno;
        }

        public List<EquipoGPSDTO> ObtenerGPSs(ReporteFrecuenciaParam param)
        {
            List<EquipoGPSDTO> lista = new List<EquipoGPSDTO>();
            try
            {
                var query = string.Format(helper.SqlGetGPSs,
                                            param.FechaInicial.ToString("dd/MM/yyyy HH:mm:ss"),
                                            param.FechaFinal.ToString("dd/MM/yyyy HH:mm:ss"),
                                            param.IdGPS,
                                            param.IndOficial);

                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        lista.Add(helper.CreateGPS(dr));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lista;
        }

        public DataSet Reportes(int idGPS, string gps, DateTime fechaIni, DateTime fechaFin, string Tipo)
        {
            DataSet Retorno = null;
            try
            {
                string query;
                if (Tipo == "Frec")
                {
                    query = string.Format(helper.SqlFrecuencia,
                        fechaIni.ToString("dd/MM/yyyy HH:mm"),
                        fechaFin.ToString("dd/MM/yyyy HH:mm"), idGPS, 
                        fechaIni.ToString("dd/MM/yyyy HH:mm:ss"),
                        fechaFin.ToString("dd/MM/yyyy HH:mm:ss"));
                }
                else if (Tipo == "Ocur")
                {
                    query = string.Format(helper.SqOcurrencias,
                                fechaIni.ToString("dd/MM/yyyy HH:mm"),
                                fechaFin.ToString("dd/MM/yyyy HH:mm"), idGPS);
                }
                else
                {
                    query = string.Format(helper.SqlFrecuenciaMin,
                                fechaIni.ToString("dd/MM/yyyy HH:mm"),
                                fechaFin.ToString("dd/MM/yyyy HH:mm"), idGPS, Tipo);
                }

                Retorno = new DataSet();
                DbCommand command = dbProvider.GetSqlStringCommand(query);
                dbProvider.LoadDataSet(command, (DataSet)Retorno, Tipo);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Retorno;
        }

        public int Indicadores(int idGPS, string gps, DateTime fecha)
        {
            int Retorno = 0;
            try
            {
                var query = string.Format(helper.SqlIndicadores, fecha.ToString("yyyy-MM-dd"), idGPS);
                DbCommand command = dbProvider.GetSqlStringCommand(query);
                Retorno = dbProvider.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Retorno;
        }

        public System.Data.DataSet TransgresionMensual(string inicio, string fin)
        {
            DataSet Retorno = null;
            try
            {
                string query;
                query = string.Format(helper.SqlTransgresionMensual, inicio, fin);
                Retorno = new DataSet();
                DbCommand command = dbProvider.GetSqlStringCommand(query);
                dbProvider.LoadDataSet(command, (DataSet)Retorno, "R");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Retorno;
        }

    }
}
