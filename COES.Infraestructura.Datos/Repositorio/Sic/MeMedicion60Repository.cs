using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;
using System.Linq;
using System.IO.Compression;
using COES.Framework.Base.Tools;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla ME_MEDICION60
    /// </summary>
    public class MeMedicion60Repository: RepositoryBase, IMeMedicion60Repository
    {
        public MeMedicion60Repository(string strConn): base(strConn)
        {
        }

        MeMedicion60Helper helper = new MeMedicion60Helper();

        public void Save(MeMedicion60DTO entity, int mes)
        {
            string query = string.Format(helper.SqlSave, "_" + mes);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            dbProvider.AddInParameter(command, helper.H56, DbType.Decimal, entity.H56);
            dbProvider.AddInParameter(command, helper.H57, DbType.Decimal, entity.H57);
            dbProvider.AddInParameter(command, helper.H58, DbType.Decimal, entity.H58);
            dbProvider.AddInParameter(command, helper.H59, DbType.Decimal, entity.H59);
            dbProvider.AddInParameter(command, helper.H0, DbType.Decimal, entity.H0);
            dbProvider.AddInParameter(command, helper.Fechahora, DbType.DateTime, entity.Fechahora);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, entity.Tipoinfocodi);
            dbProvider.AddInParameter(command, helper.H1, DbType.Decimal, entity.H1);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.H2, DbType.Decimal, entity.H2);
            dbProvider.AddInParameter(command, helper.H3, DbType.Decimal, entity.H3);
            dbProvider.AddInParameter(command, helper.H4, DbType.Decimal, entity.H4);
            dbProvider.AddInParameter(command, helper.H5, DbType.Decimal, entity.H5);
            dbProvider.AddInParameter(command, helper.H6, DbType.Decimal, entity.H6);
            dbProvider.AddInParameter(command, helper.H7, DbType.Decimal, entity.H7);
            dbProvider.AddInParameter(command, helper.H8, DbType.Decimal, entity.H8);
            dbProvider.AddInParameter(command, helper.H9, DbType.Decimal, entity.H9);
            dbProvider.AddInParameter(command, helper.H10, DbType.Decimal, entity.H10);
            dbProvider.AddInParameter(command, helper.H11, DbType.Decimal, entity.H11);
            dbProvider.AddInParameter(command, helper.H12, DbType.Decimal, entity.H12);
            dbProvider.AddInParameter(command, helper.H13, DbType.Decimal, entity.H13);
            dbProvider.AddInParameter(command, helper.H14, DbType.Decimal, entity.H14);
            dbProvider.AddInParameter(command, helper.H15, DbType.Decimal, entity.H15);
            dbProvider.AddInParameter(command, helper.H16, DbType.Decimal, entity.H16);
            dbProvider.AddInParameter(command, helper.H17, DbType.Decimal, entity.H17);
            dbProvider.AddInParameter(command, helper.H18, DbType.Decimal, entity.H18);
            dbProvider.AddInParameter(command, helper.H19, DbType.Decimal, entity.H19);
            dbProvider.AddInParameter(command, helper.H20, DbType.Decimal, entity.H20);
            dbProvider.AddInParameter(command, helper.H21, DbType.Decimal, entity.H21);
            dbProvider.AddInParameter(command, helper.H22, DbType.Decimal, entity.H22);
            dbProvider.AddInParameter(command, helper.H23, DbType.Decimal, entity.H23);
            dbProvider.AddInParameter(command, helper.H24, DbType.Decimal, entity.H24);
            dbProvider.AddInParameter(command, helper.H25, DbType.Decimal, entity.H25);
            dbProvider.AddInParameter(command, helper.H26, DbType.Decimal, entity.H26);
            dbProvider.AddInParameter(command, helper.H27, DbType.Decimal, entity.H27);
            dbProvider.AddInParameter(command, helper.H28, DbType.Decimal, entity.H28);
            dbProvider.AddInParameter(command, helper.H29, DbType.Decimal, entity.H29);
            dbProvider.AddInParameter(command, helper.H30, DbType.Decimal, entity.H30);
            dbProvider.AddInParameter(command, helper.H31, DbType.Decimal, entity.H31);
            dbProvider.AddInParameter(command, helper.H32, DbType.Decimal, entity.H32);
            dbProvider.AddInParameter(command, helper.H33, DbType.Decimal, entity.H33);
            dbProvider.AddInParameter(command, helper.H34, DbType.Decimal, entity.H34);
            dbProvider.AddInParameter(command, helper.H35, DbType.Decimal, entity.H35);
            dbProvider.AddInParameter(command, helper.H36, DbType.Decimal, entity.H36);
            dbProvider.AddInParameter(command, helper.H37, DbType.Decimal, entity.H37);
            dbProvider.AddInParameter(command, helper.H38, DbType.Decimal, entity.H38);
            dbProvider.AddInParameter(command, helper.H39, DbType.Decimal, entity.H39);
            dbProvider.AddInParameter(command, helper.H40, DbType.Decimal, entity.H40);
            dbProvider.AddInParameter(command, helper.H41, DbType.Decimal, entity.H41);
            dbProvider.AddInParameter(command, helper.H42, DbType.Decimal, entity.H42);
            dbProvider.AddInParameter(command, helper.H43, DbType.Decimal, entity.H43);
            dbProvider.AddInParameter(command, helper.H44, DbType.Decimal, entity.H44);
            dbProvider.AddInParameter(command, helper.H45, DbType.Decimal, entity.H45);
            dbProvider.AddInParameter(command, helper.H46, DbType.Decimal, entity.H46);
            dbProvider.AddInParameter(command, helper.H47, DbType.Decimal, entity.H47);
            dbProvider.AddInParameter(command, helper.H48, DbType.Decimal, entity.H48);
            dbProvider.AddInParameter(command, helper.H49, DbType.Decimal, entity.H49);
            dbProvider.AddInParameter(command, helper.H50, DbType.Decimal, entity.H50);
            dbProvider.AddInParameter(command, helper.H51, DbType.Decimal, entity.H51);
            dbProvider.AddInParameter(command, helper.H52, DbType.Decimal, entity.H52);
            dbProvider.AddInParameter(command, helper.H53, DbType.Decimal, entity.H53);
            dbProvider.AddInParameter(command, helper.H54, DbType.Decimal, entity.H54);
            dbProvider.AddInParameter(command, helper.H55, DbType.Decimal, entity.H55);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(MeMedicion60DTO entity, int mes)
        {
            string query = string.Format(helper.SqlUpdate, "_" + mes);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            dbProvider.AddInParameter(command, helper.H56, DbType.Decimal, entity.H56);
            dbProvider.AddInParameter(command, helper.H57, DbType.Decimal, entity.H57);
            dbProvider.AddInParameter(command, helper.H58, DbType.Decimal, entity.H58);
            dbProvider.AddInParameter(command, helper.H59, DbType.Decimal, entity.H59);
            dbProvider.AddInParameter(command, helper.H0, DbType.Decimal, entity.H0);
            dbProvider.AddInParameter(command, helper.Fechahora, DbType.DateTime, entity.Fechahora);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, entity.Tipoinfocodi);
            dbProvider.AddInParameter(command, helper.H1, DbType.Decimal, entity.H1);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.H2, DbType.Decimal, entity.H2);
            dbProvider.AddInParameter(command, helper.H3, DbType.Decimal, entity.H3);
            dbProvider.AddInParameter(command, helper.H4, DbType.Decimal, entity.H4);
            dbProvider.AddInParameter(command, helper.H5, DbType.Decimal, entity.H5);
            dbProvider.AddInParameter(command, helper.H6, DbType.Decimal, entity.H6);
            dbProvider.AddInParameter(command, helper.H7, DbType.Decimal, entity.H7);
            dbProvider.AddInParameter(command, helper.H8, DbType.Decimal, entity.H8);
            dbProvider.AddInParameter(command, helper.H9, DbType.Decimal, entity.H9);
            dbProvider.AddInParameter(command, helper.H10, DbType.Decimal, entity.H10);
            dbProvider.AddInParameter(command, helper.H11, DbType.Decimal, entity.H11);
            dbProvider.AddInParameter(command, helper.H12, DbType.Decimal, entity.H12);
            dbProvider.AddInParameter(command, helper.H13, DbType.Decimal, entity.H13);
            dbProvider.AddInParameter(command, helper.H14, DbType.Decimal, entity.H14);
            dbProvider.AddInParameter(command, helper.H15, DbType.Decimal, entity.H15);
            dbProvider.AddInParameter(command, helper.H16, DbType.Decimal, entity.H16);
            dbProvider.AddInParameter(command, helper.H17, DbType.Decimal, entity.H17);
            dbProvider.AddInParameter(command, helper.H18, DbType.Decimal, entity.H18);
            dbProvider.AddInParameter(command, helper.H19, DbType.Decimal, entity.H19);
            dbProvider.AddInParameter(command, helper.H20, DbType.Decimal, entity.H20);
            dbProvider.AddInParameter(command, helper.H21, DbType.Decimal, entity.H21);
            dbProvider.AddInParameter(command, helper.H22, DbType.Decimal, entity.H22);
            dbProvider.AddInParameter(command, helper.H23, DbType.Decimal, entity.H23);
            dbProvider.AddInParameter(command, helper.H24, DbType.Decimal, entity.H24);
            dbProvider.AddInParameter(command, helper.H25, DbType.Decimal, entity.H25);
            dbProvider.AddInParameter(command, helper.H26, DbType.Decimal, entity.H26);
            dbProvider.AddInParameter(command, helper.H27, DbType.Decimal, entity.H27);
            dbProvider.AddInParameter(command, helper.H28, DbType.Decimal, entity.H28);
            dbProvider.AddInParameter(command, helper.H29, DbType.Decimal, entity.H29);
            dbProvider.AddInParameter(command, helper.H30, DbType.Decimal, entity.H30);
            dbProvider.AddInParameter(command, helper.H31, DbType.Decimal, entity.H31);
            dbProvider.AddInParameter(command, helper.H32, DbType.Decimal, entity.H32);
            dbProvider.AddInParameter(command, helper.H33, DbType.Decimal, entity.H33);
            dbProvider.AddInParameter(command, helper.H34, DbType.Decimal, entity.H34);
            dbProvider.AddInParameter(command, helper.H35, DbType.Decimal, entity.H35);
            dbProvider.AddInParameter(command, helper.H36, DbType.Decimal, entity.H36);
            dbProvider.AddInParameter(command, helper.H37, DbType.Decimal, entity.H37);
            dbProvider.AddInParameter(command, helper.H38, DbType.Decimal, entity.H38);
            dbProvider.AddInParameter(command, helper.H39, DbType.Decimal, entity.H39);
            dbProvider.AddInParameter(command, helper.H40, DbType.Decimal, entity.H40);
            dbProvider.AddInParameter(command, helper.H41, DbType.Decimal, entity.H41);
            dbProvider.AddInParameter(command, helper.H42, DbType.Decimal, entity.H42);
            dbProvider.AddInParameter(command, helper.H43, DbType.Decimal, entity.H43);
            dbProvider.AddInParameter(command, helper.H44, DbType.Decimal, entity.H44);
            dbProvider.AddInParameter(command, helper.H45, DbType.Decimal, entity.H45);
            dbProvider.AddInParameter(command, helper.H46, DbType.Decimal, entity.H46);
            dbProvider.AddInParameter(command, helper.H47, DbType.Decimal, entity.H47);
            dbProvider.AddInParameter(command, helper.H48, DbType.Decimal, entity.H48);
            dbProvider.AddInParameter(command, helper.H49, DbType.Decimal, entity.H49);
            dbProvider.AddInParameter(command, helper.H50, DbType.Decimal, entity.H50);
            dbProvider.AddInParameter(command, helper.H51, DbType.Decimal, entity.H51);
            dbProvider.AddInParameter(command, helper.H52, DbType.Decimal, entity.H52);
            dbProvider.AddInParameter(command, helper.H53, DbType.Decimal, entity.H53);
            dbProvider.AddInParameter(command, helper.H54, DbType.Decimal, entity.H54);
            dbProvider.AddInParameter(command, helper.H55, DbType.Decimal, entity.H55);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(DateTime fechahora, int tipoinfocodi, int ptomedicodi, int mes)
        {
            string query = string.Format(helper.SqlDelete, "_" + mes);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            dbProvider.AddInParameter(command, helper.Fechahora, DbType.DateTime, fechahora);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, tipoinfocodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, ptomedicodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public MeMedicion60DTO GetById(DateTime fechahora, int tipoinfocodi, int ptomedicodi, int mes)
        {
            string query = string.Format(helper.SqlGetById, "_" + mes);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            dbProvider.AddInParameter(command, helper.Fechahora, DbType.DateTime, fechahora);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, tipoinfocodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, ptomedicodi);
            MeMedicion60DTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MeMedicion60DTO> List(int mes)
        {
            List<MeMedicion60DTO> entitys = new List<MeMedicion60DTO>();
            string query = string.Format(helper.SqlList, "_" + mes);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<MeMedicion60DTO> GetByCriteria(int mes)
        {
            List<MeMedicion60DTO> entitys = new List<MeMedicion60DTO>();
            string query = string.Format(helper.SqlGetByCriteria, "_" + mes);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<int> VerificarCarga(DateTime fecha)
        {
            List<int> puntos = new List<int>();
            string query = String.Format(helper.SqlVerificarCarga, fecha.ToString(ConstantesBase.FormatoFecha), "_" + fecha.Month);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int iPtoMediCodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtoMediCodi))
                    {
                        int punto = Convert.ToInt32(dr.GetValue(iPtoMediCodi));
                        puntos.Add(punto);
                    }
                }
            }

            return puntos;
        }

        public List<MeMedicion60DTO> DescargarEnvio(List<int> codigos, DateTime fecha)
        {
            List<MeMedicion60DTO> entitys = new List<MeMedicion60DTO>();

            string where = string.Join(",", codigos);

            string query = String.Format(helper.SqlConsultaDescarga, where, fecha.Year, fecha.Month.ToString("D2"), fecha.Day.ToString("D2"), "_" + fecha.Month);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion60DTO entity = new MeMedicion60DTO();

                    int iPtoMediCodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtoMediCodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtoMediCodi));

                    int iTipoInfoCodi = dr.GetOrdinal(helper.Tipoinfocodi);
                    if (!dr.IsDBNull(iTipoInfoCodi)) entity.Tipoinfocodi = Convert.ToInt32(dr.GetValue(iTipoInfoCodi));

                    int iFechaHora = dr.GetOrdinal(helper.Fechahora);
                    if (!dr.IsDBNull(iFechaHora)) entity.Fechahora = dr.GetDateTime(iFechaHora);

                    for (int i = 0; i < 60; i++)
                    {
                        int indice = dr.GetOrdinal("H" + i);
                        if (!dr.IsDBNull(indice))
                        {
                            entity.GetType().GetProperty("H" + i).SetValue(entity, Convert.ToDecimal(dr.GetValue(indice)));
                        }
                    }

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<int> ObtenerValorencero(DateTime fecha, int tipoinfocodi)
        {
            List<int> ids = new List<int>();
            string query = String.Format(helper.SqlObtenerPotencia, fecha.ToString(ConstantesBase.FormatoFecha), tipoinfocodi, "_" + fecha.Month);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ids.Add(Convert.ToInt32(dr.GetValue(dr.GetOrdinal(helper.Ptomedicodi))));
                }
            }

            return ids;
        }

        public List<RegistrorpfDTO> ObtenerRango(string ptomedicodi, DateTime fechaInicio, DateTime fechaFin)
        {
            List<RegistrorpfDTO> entitys = new List<RegistrorpfDTO>();

            string query = String.Format(helper.SqlObtenerRango, ptomedicodi, fechaInicio.ToString(ConstantesBase.FormatoFechaExtendido), fechaFin.ToString(ConstantesBase.FormatoFechaExtendido), "_" + fechaInicio.Month);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RegistrorpfDTO entity = new RegistrorpfDTO();

                    int iPtoMediCodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtoMediCodi)) entity.PTOMEDICODI = dr.GetInt32(iPtoMediCodi);

                    int iTipoInfoCodi = dr.GetOrdinal(helper.Tipoinfocodi);
                    if (!dr.IsDBNull(iTipoInfoCodi)) entity.TIPOINFOCODI = dr.GetInt32(iTipoInfoCodi);

                    int iFechaHora = dr.GetOrdinal(helper.Fechahora);
                    if (!dr.IsDBNull(iFechaHora)) entity.FECHAHORA = dr.GetDateTime(iFechaHora);

                    int iSegundo = dr.GetOrdinal(helper.SEGUNDO);
                    if (!dr.IsDBNull(iSegundo)) entity.SEGUNDO = Convert.ToInt32(dr.GetString(iSegundo).Replace("H", ""));

                    int iValor = dr.GetOrdinal(helper.Valor);
                    if (!dr.IsDBNull(iValor)) entity.VALOR = dr.GetDecimal(iValor);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeMedicion60DTO> ObtenerDatosComparacionRango(DateTime fechaConsulta, string ptomedicodi, int resolucion, string mes)
        {
            List<MeMedicion60DTO> entitys = new List<MeMedicion60DTO>();
            string query = string.Empty;
            switch (resolucion)
            {
                case 15:
                    query = String.Format(helper.SqlObtenerDatosComparacionRangoResolucionCuartoHora, fechaConsulta.ToString(ConstantesBase.FormatoFecha), fechaConsulta.AddDays(1).ToString(ConstantesBase.FormatoFecha), ptomedicodi, mes);
                    break;
                case 30:
                    query = String.Format(helper.SqlObtenerDatosComparacionRango, fechaConsulta.ToString(ConstantesBase.FormatoFecha), fechaConsulta.AddDays(1).ToString(ConstantesBase.FormatoFecha), ptomedicodi, mes);
                    break;
                case 60:
                    query = String.Format(helper.SqlObtenerDatosComparacionRangoResolucionHora, fechaConsulta.ToString(ConstantesBase.FormatoFecha), fechaConsulta.AddDays(1).ToString(ConstantesBase.FormatoFecha), ptomedicodi, mes);
                    break;
            }

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion60DTO entity = new MeMedicion60DTO();

                    int iFechaHora = dr.GetOrdinal(helper.Fechahora);
                    if (!dr.IsDBNull(iFechaHora)) entity.Fechahora = dr.GetDateTime(iFechaHora);

                    int iH0 = dr.GetOrdinal(helper.H0);
                    if (!dr.IsDBNull(iH0)) entity.H0 = dr.GetDecimal(iH0);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<RegistrorpfDTO> ObtenerPotenciasMaximas(DateTime fechaConsulta)
        {
            List<RegistrorpfDTO> entitys = new List<RegistrorpfDTO>();

            string query = String.Format(helper.SqlObtenerPotenciasMaximas, fechaConsulta.ToString(ConstantesBase.FormatoFechaExtendido), "_" + fechaConsulta.Month);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int ptoMedicodi = 0;

                    int iPtoMediCodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtoMediCodi)) ptoMedicodi = dr.GetInt32(iPtoMediCodi);

                    List<decimal> list = new List<decimal>();
                    
                    for (int i = 0; i < 60; i++)
                    {
                        int indice = dr.GetOrdinal(helper.SEGUNDO + i);
                        if (!dr.IsDBNull(indice))
                        {
                            list.Add(Convert.ToDecimal(dr.GetValue(indice)));
                        }
                    }

                    RegistrorpfDTO entity = new RegistrorpfDTO();

                    decimal min = Decimal.MaxValue;
                    decimal max = Decimal.MinValue;
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i] < min) { min = list[i]; }
                        if (list[i] > max) { max = list[i]; }
                    }
                    entity.PTOMEDICODI = ptoMedicodi;
                    entity.POTENCIA = max;

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<ReporteEnvioDTO> ObtenerReporte(DateTime fechaConsulta)
        {
            List<ReporteEnvioDTO> entitys = new List<ReporteEnvioDTO>();

            string query = String.Format(helper.SqlObtenerReporte, fechaConsulta.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ReporteEnvioDTO entity = new ReporteEnvioDTO();

                    int iFecha = dr.GetOrdinal("Fecha");
                    if (!dr.IsDBNull(iFecha)) entity.Fecha = dr.GetDateTime(iFecha);

                    int iPtoMediCodi = dr.GetOrdinal("PtoMediCodi");
                    if (!dr.IsDBNull(iPtoMediCodi)) entity.PtoMediCodi = dr.GetInt32(iPtoMediCodi);

                    int iFechaCarga = dr.GetOrdinal("FechaCarga");
                    if (!dr.IsDBNull(iFechaCarga)) entity.FechaCarga = dr.GetDateTime(iFechaCarga);

                    int iIndConsistencia = dr.GetOrdinal("IndConsistencia");
                    if (!dr.IsDBNull(iIndConsistencia)) entity.IndConsistencia = dr.GetString(iIndConsistencia);

                    int iValConsistencia = dr.GetOrdinal("ValConsistencia");
                    if (!dr.IsDBNull(iValConsistencia)) entity.ValConsistencia = dr.GetDecimal(iValConsistencia);

                    int iEstadoOperativo = dr.GetOrdinal("EstadoOperativo");
                    if (!dr.IsDBNull(iEstadoOperativo)) entity.EstadoOperativo = dr.GetString(iEstadoOperativo);

                    int iEstadoInformacion = dr.GetOrdinal("EstadoInformacion");
                    if (!dr.IsDBNull(iEstadoInformacion)) entity.EstadoInformacion = dr.GetString(iEstadoInformacion);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void EliminarReporte(DateTime fecha)
        {
            string query = string.Format(helper.SqlEliminarReporte, fecha.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            dbProvider.ExecuteNonQuery(command);
        }

        public void GrabarReporte(ReporteEnvioDTO item)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGrabarReporte);

            dbProvider.AddInParameter(command, "Fecha", DbType.DateTime, item.Fecha);
            dbProvider.AddInParameter(command, "PtoMediCodi", DbType.Decimal, item.PtoMediCodi);
            dbProvider.AddInParameter(command, "FechaCarga", DbType.DateTime, item.FechaCarga);
            dbProvider.AddInParameter(command, "IndConsistencia", DbType.String, item.IndConsistencia);
            dbProvider.AddInParameter(command, "ValConsistencia", DbType.Decimal, item.ValConsistencia);
            dbProvider.AddInParameter(command, "EstadoOperativo", DbType.String, item.EstadoOperativo);
            dbProvider.AddInParameter(command, "EstadoInformacion", DbType.String, item.EstadoInformacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public LogEnvioMedicionDTO ObtenerLogPorPuntoFecha(int idPto, DateTime fecha)
        {
            LogEnvioMedicionDTO entity = null;

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGrabarReporte);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new LogEnvioMedicionDTO();

                    int iLogCodi = dr.GetOrdinal("LogCodi");
                    if (!dr.IsDBNull(iLogCodi)) entity.LogCodi = Convert.ToInt32(dr.GetValue(iLogCodi));

                    int iEmprCodi = dr.GetOrdinal("EmprCodi");
                    if (!dr.IsDBNull(iEmprCodi)) entity.EmprCodi = Convert.ToInt32(dr.GetValue(iEmprCodi));

                    int iLastUser = dr.GetOrdinal("LastUser");
                    if (!dr.IsDBNull(iLastUser)) entity.LastUser = dr.GetString(iLastUser);

                    int iLastDate = dr.GetOrdinal("LastDate");
                    if (!dr.IsDBNull(iLastDate)) entity.LastDate = dr.GetDateTime(iLastDate);

                    int iFilNomb = dr.GetOrdinal("FileNomb");
                    if (!dr.IsDBNull(iFilNomb)) entity.FilNomb = dr.GetString(iFilNomb);

                    int iLogDesc = dr.GetOrdinal("LogDesc");
                    if (!dr.IsDBNull(iLogDesc)) entity.LogDesc = dr.GetString(iLogDesc);
                }
            }

            return entity;
        }

        public void GrabarLogReporte(string descrip)
        {
            string query = string.Format(helper.SqlGrabarLogReporte, descrip);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<MeMedicion60DTO> ListaMedicionesTmp(DateTime fechaInicial, DateTime fechaFinal, int idPto, int tipoinfocodi)
        {
            List<MeMedicion60DTO> entitys = new List<MeMedicion60DTO>();
            string query = string.Format(helper.SqlListaMedicionesTmp, fechaInicial.ToString(ConstantesBase.FormatoFechaHora)
                , fechaInicial.ToString(ConstantesBase.FormatoFechaHora), idPto, tipoinfocodi, "_" + fechaInicial.Month);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iSegundo = dr.GetOrdinal(helper.SEGUNDO);
                    if (!dr.IsDBNull(iSegundo)) entity.SEGUNDO = dr.GetInt32(iSegundo);

                    int iValor = dr.GetOrdinal(helper.Valor);
                    if (!dr.IsDBNull(iValor)) entity.VALOR = dr.GetDecimal(iValor);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void GrabarDatosRpf(List<MeMedicion60DTO> entitys, DateTime fechaCarga, int mes)
        {
            dbProvider.AddColumnMapping(helper.Fechahora, DbType.DateTime);
            dbProvider.AddColumnMapping(helper.Tipoinfocodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Ptomedicodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.H0, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H1, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H2, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H3, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H4, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H5, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H6, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H7, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H8, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H9, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H10, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H11, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H12, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H13, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H14, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H15, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H16, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H17, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H18, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H19, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H20, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H21, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H22, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H23, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H24, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H25, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H26, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H27, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H28, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H29, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H30, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H31, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H32, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H33, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H34, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H35, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H36, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H37, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H38, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H39, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H40, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H41, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H42, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H43, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H44, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H45, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H46, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H47, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H48, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H49, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H50, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H51, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H52, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H53, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H54, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H55, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H56, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H57, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H58, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H59, DbType.Decimal);

            dbProvider.BulkInsert<MeMedicion60DTO>(entitys, helper.TableName + (mes == 0 ? "" : "_" + mes));
        }

        public int GrabarLogEnvio(LogEnvioMedicionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxIdLogReporteEnvio);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlGrabarLogReporteEnvio);

            dbProvider.AddInParameter(command, "logcodi", DbType.Int32, id);
            dbProvider.AddInParameter(command, "emprcodi", DbType.Int32, entity.EmprCodi);
            dbProvider.AddInParameter(command, "lastuser", DbType.String, entity.LastUser);
            dbProvider.AddInParameter(command, "lastdate", DbType.DateTime, entity.LastDate);
            dbProvider.AddInParameter(command, "filnomb", DbType.String, entity.FilNomb);
            dbProvider.AddInParameter(command, "logdesc", DbType.String, entity.LogDesc);
            dbProvider.AddInParameter(command, "ptomedicodi", DbType.Int32, entity.PtoMediCodi);
            dbProvider.AddInParameter(command, "fecha", DbType.DateTime, entity.Fecha);

            return dbProvider.ExecuteNonQuery(command);
        }

        public int EliminarCargaRpf(string ptomedicodi, DateTime fecha1, DateTime fecha2, int mes, string tipoinfocodi)
        {
            string query = string.Format(helper.SqlEliminarCargaRpf, ptomedicodi, fecha1.ToString(ConstantesBase.FormatoFecha), fecha2.ToString(ConstantesBase.FormatoFecha), (mes == 0 ? "" : "_" + mes), tipoinfocodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            return dbProvider.ExecuteNonQuery(command);
        }

        public List<MeMedicion60DTO> BuscarDatosRpf(DateTime fechaini, DateTime fechafin, int ptomedicodi, int idtipodato)
        {
            List<MeMedicion60DTO> entitys = new List<MeMedicion60DTO>();

            string query = string.Format(helper.SqlBuscarDatosRpf, fechaini.ToString(ConstantesBase.FormatoFechaHora), fechafin.ToString(ConstantesBase.FormatoFechaHora), ptomedicodi, idtipodato, "_" + fechaini.Month);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion60DTO entity = new MeMedicion60DTO();

                    int iPtoMediCodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtoMediCodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtoMediCodi));

                    int iTipoInfoCodi = dr.GetOrdinal(helper.Tipoinfocodi);
                    if (!dr.IsDBNull(iTipoInfoCodi)) entity.Tipoinfocodi = Convert.ToInt32(dr.GetValue(iTipoInfoCodi));

                    int iFechaHora = dr.GetOrdinal(helper.Fechahora);
                    if (!dr.IsDBNull(iFechaHora)) entity.Fechahora = dr.GetDateTime(iFechaHora);

                    for (int i = 0; i < 60; i++)
                    {
                        int indice = dr.GetOrdinal(helper.SEGUNDO + i);
                        if (!dr.IsDBNull(indice))
                        {
                            entity.GetType().GetProperty(helper.SEGUNDO + i).SetValue(entity, Convert.ToDecimal(dr.GetValue(indice)));
                        }
                    }

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #region Generar FileZip Pr21

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFinal"></param>
        /// <returns></returns>
        public int ObtenerGenerarFileZip(DateTime fechaInicio, DateTime fechaFinal, string path, List<int> codigos)
        {
            int[] tipos = { 1, 6 };

            int ts = (int)fechaFinal.Subtract(fechaInicio).TotalDays;

            for (int i = 0; i < ts; i++)
            {
                DateTime fechaProceso = fechaInicio.AddDays(i);

                //This creates our list of files to be added
                List<string> filesToArchive = new List<string>();

                #region Seteo

//                int[] codigos = { 1,2,3,5,6,11,12,13,14,15,16,17,18,19,30,31,32,33,34,35,36,37,38,39,40,41,45,46,47,48,49,53,54,55,56,57,58,59,60,61,62,63,64,65,66,67,68,70,71,72,73,74,75,76,77,78,79,
//80,98,279,283,284,286,287,341,342,343,346,347,348,349,351,357,358,359,360,361,362,364,365,366,369,370,371,374,375,376,383,384,398,399,400,402,403,412,413,414,420,421,422,423,426,429,431,434,435,436,
//437,438,440,443,444,447,448,449,450,451,452,455,456,461,463,466,467,471,476,477,478,479,480,481,486,487,488,489,490,491,759,768,769,778,779,782,783,784,785,794,797,798,799,918,919,921,922,992,993,994,
//2129,2130,2132,2133,2134,2136,2137,2153,2158,2173,2174,2176,2177,2179,2180,30624,30625,30626,30627,30702,30703,30707,30708,30709,30710,30711,30712,30714,30716,30717,30718,30719,30721,30722,30723,30724,
//42012,43376,43409,44233,44649,44662,44667,44671,44672,44673,44752,44757,44761,44762,44763,44777,44778,44779,44872,44873 };

                #endregion

                //path = "F:\\Descarga\\";
                FileServer.CreateFolder(path, fechaProceso.Year.ToString(), string.Empty);
                FileServer.CreateFolder(path + fechaProceso.Year, fechaProceso.Month.ToString(), string.Empty);

                foreach (int id in codigos)
                {
                    foreach (int tipo in tipos)
                    {
                        List<string> list = this.DescargarDatos(fechaProceso, fechaProceso, tipo, id).ToList();

                        string nombre = "DATOS";
                        if (tipo != -1)
                        {
                            nombre = (tipo == 1) ? "POTENCIA" : "FRECUENCIA";
                        }

                        string fileName = path + fechaProceso.Year + "/" + fechaProceso.Month + "/" + id + "_" + nombre + "_" + fechaProceso.ToString("dd.MM.yyyy") + ".csv";

                        if (System.IO.File.Exists(fileName))
                        {
                            System.IO.File.Delete(fileName);
                        }

                        if (list.Count > 0)
                        {
                            using (var outFile = System.IO.File.CreateText(fileName))
                            {
                                outFile.WriteLine("PTOMEDICODI,TIPOINFOCODI,FECHAHORA,H0,H1,H2,H3,H4,H5,H6,H7,H8,H9,H10,H11,H12,H13,H14,H15,H16,H17,H18,H19,H20,H21,H22,H23,H24,H25,H26,H27,H28,H29,H30,H31,H32,H33,H34,H35,H36,H37,H38,H39,H40,H41,H42,H43,H44,H45,H46,H47,H48,H49,H50,H51,H52,H53,H54,H55,H56,H57,H58,H59");

                                foreach (string item in list)
                                {
                                    outFile.WriteLine(item);
                                }
                            }

                            filesToArchive.Add(fileName);
                        }
                    }
                }

                if (codigos.Count > 0)
                {
                    Compression.AddToArchive(path + fechaProceso.Year + "/" + fechaProceso.Month + "/" + "DATOS_" + fechaProceso.ToString("dd.MM.yyyy") + ".zip",
                        filesToArchive,
                        Compression.ArchiveAction.Replace,
                        Compression.Overwrite.IfNewer,
                        CompressionLevel.Optimal);
                }

                foreach (string file in filesToArchive)
                {
                    if (System.IO.File.Exists(file))
                    {
                        System.IO.File.Delete(file);
                    }
                }
            }

            return 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="tipo"></param>
        /// <param name="ptomedicodi"></param>
        /// <returns></returns>
        public List<string> DescargarDatos(DateTime fechaInicio, DateTime fechaFinal, int tipo, int ptomedicodi)
        {
            List<string> resultado = new List<string>();
            string query = string.Format(helper.SqlDescargarDatos, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFinal.AddDays(1).ToString(ConstantesBase.FormatoFecha), tipo, ptomedicodi, fechaFinal.Month);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                string[] columnNames = GetColumnNames(dr).ToArray();
                int numFields = columnNames.Length;
                while (dr.Read())
                {
                    string[] columnValues =
                        Enumerable.Range(0, numFields)
                                    .Select(i => dr.GetValue(i).ToString())
                                    .Select(field => string.Concat("\"", field.Replace("\"", "\"\""), "\""))
                                    .ToArray();
                    resultado.Add(string.Join(",", columnValues));
                }
            }
            return resultado;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private IEnumerable<string> GetColumnNames(IDataReader reader)
        {
            foreach (DataRow row in reader.GetSchemaTable().Rows)
            {
                yield return (string)row["ColumnName"];
            }
        }

        #endregion
    }
}
