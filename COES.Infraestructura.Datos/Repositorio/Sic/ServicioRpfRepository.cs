using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Scada;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    public class ServicioRpfRepository: RepositoryBase
    {
        public ServicioRpfRepository(string strConn)
            : base(strConn)
        {

        }

        ServicioRpfHelper helper = new ServicioRpfHelper();

        public List<ServicioRpfDTO> GetByCriteria(DateTime fechaPeriodo)
        {
            List<ServicioRpfDTO> entitys = new List<ServicioRpfDTO>();
            string sql = string.Format(helper.SqlGetByCriteria, fechaPeriodo.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ServicioRpfDTO entity = this.helper.Create(dr);

                    int iEmprNomb = dr.GetOrdinal(this.helper.EMPRNOMB);
                    if (!dr.IsDBNull(iEmprNomb)) entity.EMPRNOMB = dr.GetString(iEmprNomb);

                    int iEquiNomb = dr.GetOrdinal(this.helper.EQUINOMB);
                    if (!dr.IsDBNull(iEquiNomb)) entity.EQUINOMB = dr.GetString(iEquiNomb);

                    int iEquiAbrev = dr.GetOrdinal(this.helper.EQUIABREV);
                    if (!dr.IsDBNull(iEquiAbrev)) entity.EQUIABREV = dr.GetString(iEquiAbrev);

                    int iFamCodi = dr.GetOrdinal(this.helper.FAMCODI);
                    if (!dr.IsDBNull(iFamCodi)) entity.FAMCODI = Convert.ToInt32(dr.GetValue(iFamCodi));

                    int iEquiCodi = dr.GetOrdinal(this.helper.EQUICODI);
                    if (!dr.IsDBNull(iEquiCodi)) entity.EQUICODI = Convert.ToInt32(dr.GetValue(iEquiCodi));

                    if (entity.RPFCODI == 28 || entity.RPFCODI == 144)
                    {
                        entity.EQUINOMB = "C.T. STA.ROSA";
                    }

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public decimal? ObtenerReservaPrimaria(DateTime fecha)
        {
            string query = String.Format(helper.SqlObtenerReservaPrimaria, fecha.ToString(ConstantesBase.FormatoFechaExtendido));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    DateTime date = DateTime.Now;
                    decimal valor = 0;

                    int iFecRegistro = dr.GetOrdinal(this.helper.FECHA);
                    if (!dr.IsDBNull(iFecRegistro)) date = dr.GetDateTime(iFecRegistro);

                    int iValor = dr.GetOrdinal(this.helper.VALOR);
                    if (!dr.IsDBNull(iValor)) valor = dr.GetDecimal(iValor);

                    TimeSpan ts = date.Subtract(fecha);

                    if (Math.Abs(ts.TotalSeconds) > 20)
                    {
                        return null;
                    }
                    else 
                    {
                        return valor;
                    }
                }
            }

            return null;
        }

        public List<decimal> ObtenerFrecuenciasSanJuan(DateTime fecha)
        {
            string query = String.Format(helper.SqlObtenerFrecuenciasSanJuan, fecha.ToString(ConstantesBase.FormatoFechaHora),
                fecha.AddMinutes(-1).ToString(ConstantesBase.FormatoFechaHora));

            int segundo = fecha.Second;

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<decimal> resultado = new List<decimal>();
            List<decimal> valores = new List<decimal>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    for (int i = 59; i >= 0; i--)
                    {
                        decimal valor = 0;
                        int indice = dr.GetOrdinal(helper.SEGUNDO + i.ToString());
                        if (!dr.IsDBNull(indice)) valor = dr.GetDecimal(indice);
                        valores.Add(valor);
                    }
                }
            }

            valores.Reverse();

            if (valores.Count == 120)
            {
                int indice = 60 + segundo - 10;

                for (int i = indice; i < indice + 10; i++)
                {
                    resultado.Add(valores[i]);
                }
            }

            return resultado;
        }

        public List<decimal> ObtenerFrecuenciasComparacion(DateTime fecha)
        {
            string query = String.Format(helper.SqlObtenerFrecuenciasConsistencia, fecha.ToString(ConstantesBase.FormatoFecha));                   

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<decimal> valores = new List<decimal>();
            
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    for (int i = 0; i <= 59; i++)
                    {
                        decimal valor = 0;
                        int indice = dr.GetOrdinal(helper.SEGUNDO + i.ToString());
                        if (!dr.IsDBNull(indice)) valor = dr.GetDecimal(indice);
                        valores.Add(valor);                      
                    }                  
                }
            }

            return valores;
        }


        public List<decimal> ObtenerFrecuenciaSanJuanTotal(DateTime fecha)
        {
            int segundos = 600; //modificamos esta linea
            string query = String.Format(helper.SqlObtenerFrecuenciaSanJuanTotal, fecha.ToString(ConstantesBase.FormatoFechaHora),
                    fecha.AddSeconds(segundos).ToString(ConstantesBase.FormatoFechaHora));

            int segundo = fecha.Second;

            DbCommand command = dbProvider.GetSqlStringCommand(query);          
            List<decimal> valores = new List<decimal>();

            bool flag = true;
            bool exit = false;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    for (int i = 0; i <= 59; i++)
                    {
                        if (flag == true)
                        {
                            if (i >= segundo)
                            {
                                decimal valor = 0;
                                int indice = dr.GetOrdinal(helper.SEGUNDO + i.ToString());
                                if (!dr.IsDBNull(indice)) valor = dr.GetDecimal(indice);
                                valores.Add(valor);

                                if (valores.Count == segundos) 
                                {
                                    exit = true;
                                    break;
                                }
                            }
                        }
                        else 
                        {
                            decimal valor = 0;
                            int indice = dr.GetOrdinal(helper.SEGUNDO + i.ToString());
                            if (!dr.IsDBNull(indice)) valor = dr.GetDecimal(indice);
                            valores.Add(valor);

                            if (valores.Count == segundos)
                            {
                                exit = true;
                                break;
                            }
                        }
                    }

                    if (exit)
                        break;

                    flag = false;
                }
            }

            return valores;
        }

        public int ValidarExistenciaHoraOperacion(int equicodi, DateTime fecha)
        {
            string query = String.Format(helper.SqlVerificarHoraOperacion, equicodi, fecha.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                return Convert.ToInt32(result);
            }

            return 0;
        }

        public List<ServicioGps> ObtenerGPS(DateTime fecha)
        {
            List<ServicioGps> entitys = new List<ServicioGps>();
            string query = string.Format(helper.SqlObtenerGPS, fecha.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ServicioGps entity = new ServicioGps();

                    int iGpsCodi = dr.GetOrdinal(helper.GPSCODI);
                    if (!dr.IsDBNull(iGpsCodi)) entity.GpsCodi = Convert.ToInt32(dr.GetValue(iGpsCodi));

                    int iGpsNomb = dr.GetOrdinal(helper.GPSNOMBRE);
                    if (!dr.IsDBNull(iGpsNomb)) entity.GpsNombre = dr.GetString(iGpsNomb);

                    int iCantidad = dr.GetOrdinal(helper.CANTIDAD);
                    if (!dr.IsDBNull(iCantidad)) entity.Cantidad = Convert.ToInt32(dr.GetValue(iCantidad));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        
        public List<ServicioGps> ObtenerConsultaFrecuencia(DateTime fecha, int gpsCodi)
        {
            string query = String.Format(helper.SqlObtenerConsultaFrecuencia, fecha.ToString(ConstantesBase.FormatoFecha), gpsCodi);

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<ServicioGps> entitys = new List<ServicioGps>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DateTime hora = DateTime.Now;
                    int iFecha = dr.GetOrdinal(helper.FECHA);
                    if (!dr.IsDBNull(iFecha)) hora = dr.GetDateTime(iFecha);

                    for (int i = 0; i <= 59; i++)
                    {
                        ServicioGps entity = new ServicioGps();
                        entity.Fecha = hora.AddSeconds(i);

                        decimal valor = 0;
                        int indice = dr.GetOrdinal(helper.SEGUNDO + i.ToString());
                        if (!dr.IsDBNull(indice)) valor = dr.GetDecimal(indice);

                        entity.Frecuencia = valor;
                        entitys.Add(entity);
                    }
                }
            }

            return entitys;
        }


        public void ReemplazarFrecuencias(DateTime fecha, int gpsOrigen, int gpsDestino)
        {
            try
            {
                string query = String.Format(helper.SqlEliminarFrecuenciaGPS, fecha.ToString(ConstantesBase.FormatoFecha), gpsDestino);
                DbCommand command = dbProvider.GetSqlStringCommand(query);
                dbProvider.ExecuteNonQuery(command);

                query = String.Format(helper.SqlReemplazarFrecuencias, fecha.ToString(ConstantesBase.FormatoFecha), gpsOrigen, gpsDestino);
                command = dbProvider.GetSqlStringCommand(query);
                dbProvider.ExecuteNonQuery(command);
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message, ex);
            }
        }
        
        public int VerificarFrecuenciaSanJuan(DateTime fecha)
        {
            string query = String.Format(helper.SqlVerificarFrecuenciaSanJuan, fecha.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                return Convert.ToInt32(result);
            }

            return 0;
        }

        public void CompletarFrecuenciaSanJuan(DateTime fecha, int gpsCodi)
        {
            string query = String.Format(helper.SqlCompletarFrecuenciaSanJuan, fecha.ToString(ConstantesBase.FormatoFecha), gpsCodi);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            dbProvider.ExecuteNonQuery(command);
        }

        public decimal? ObtenerValorActualizar(DateTime fecha, int gpsCodi)
        {
            string query = String.Format(helper.SqlObtenerValorActualizar, fecha.ToString(ConstantesBase.FormatoFechaHora), gpsCodi, fecha.Second);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                if (!string.IsNullOrEmpty(result.ToString()))
                {
                    return Convert.ToDecimal(result);
                }
            }

            return null;
        }

        public void ActualizarValorFrecuencia(DateTime fecha, decimal valor)
        {
            string query = String.Format(helper.SqlActualizarValorFrecuencia, fecha.ToString(ConstantesBase.FormatoFechaHora), fecha.Second, valor);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            dbProvider.ExecuteNonQuery(command);            
        }
    }
}
