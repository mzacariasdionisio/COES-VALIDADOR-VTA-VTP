using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;
using System.Data;
using System.Globalization;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    public class DpoEstimadorRawRepository : RepositoryBase, IDpoEstimadorRawRepository
    {
        public DpoEstimadorRawRepository(string strConn) : base(strConn)
        {
        }

        DpoEstimadorRawHelper helper = new DpoEstimadorRawHelper();

        public void Save(DpoEstimadorRawTmpDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Prnvarcodi, DbType.Int32, entity.Prnvarcodi);
            dbProvider.AddInParameter(command, helper.Dporawfuente, DbType.Int32, entity.Dporawfuente);
            dbProvider.AddInParameter(command, helper.Dporawtipomedi, DbType.Int32, entity.Dporawtipomedi);
            dbProvider.AddInParameter(command, helper.Dporawfecha, DbType.DateTime, entity.Dporawfecha);
            dbProvider.AddInParameter(command, helper.Dporawvalor, DbType.Decimal, entity.Dporawvalor);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(DpoEstimadorRawTmpDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Prnvarcodi, DbType.Int32, entity.Prnvarcodi);
            dbProvider.AddInParameter(command, helper.Dporawfuente, DbType.Int32, entity.Dporawfuente);
            dbProvider.AddInParameter(command, helper.Dporawtipomedi, DbType.Int32, entity.Dporawtipomedi);
            dbProvider.AddInParameter(command, helper.Dporawfecha, DbType.DateTime, entity.Dporawfecha);
            dbProvider.AddInParameter(command, helper.Dporawvalor, DbType.Decimal, entity.Dporawvalor);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int ptomedicodi, int dpovarcodi, int dporawfuente, int dporawtipomedi, DateTime dporawfecha)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, ptomedicodi);
            dbProvider.AddInParameter(command, helper.Prnvarcodi, DbType.Int32, dpovarcodi);
            dbProvider.AddInParameter(command, helper.Dporawfuente, DbType.Int32, dporawfuente);
            dbProvider.AddInParameter(command, helper.Dporawtipomedi, DbType.Int32, dporawtipomedi);
            dbProvider.AddInParameter(command, helper.Dporawfecha, DbType.DateTime, dporawfecha);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<DpoEstimadorRawTmpDTO> List()
        {
            List<DpoEstimadorRawTmpDTO> entitys = new List<DpoEstimadorRawTmpDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DpoEstimadorRawTmpDTO entity = new DpoEstimadorRawTmpDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iDpovarcodi = dr.GetOrdinal(helper.Prnvarcodi);
                    if (!dr.IsDBNull(iDpovarcodi)) entity.Prnvarcodi = Convert.ToInt32(dr.GetValue(iDpovarcodi));

                    int iDporawfuente = dr.GetOrdinal(helper.Dporawfuente);
                    if (!dr.IsDBNull(iDporawfuente)) entity.Dporawfuente = Convert.ToInt32(dr.GetValue(iDporawfuente));

                    int iDporawtipomedi = dr.GetOrdinal(helper.Dporawtipomedi);
                    if (!dr.IsDBNull(iDporawtipomedi)) entity.Dporawtipomedi = Convert.ToInt32(dr.GetValue(iDporawtipomedi));

                    int iDporawfecha = dr.GetOrdinal(helper.Dporawfecha);
                    if (!dr.IsDBNull(iDporawfecha)) entity.Dporawfecha = dr.GetDateTime(iDporawfecha);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public DpoEstimadorRawTmpDTO GetById(int ptomedicodi, int dpovarcodi, int dporawfuente, int dporawtipomedi, DateTime dporawfecha)
        {
            DpoEstimadorRawTmpDTO entity = new DpoEstimadorRawTmpDTO();

            string fecha = dporawfecha.ToString("dd/MM/yyyy");
            string query = string.Format(helper.SqlGetById, ptomedicodi, dpovarcodi, dporawfuente, dporawtipomedi, fecha);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }



        public void BulkInsert(List<DpoEstimadorRawTmpDTO> entitys, string nombreTabla)
        {
            dbProvider.AddColumnMapping(helper.Ptomedicodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Prnvarcodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Dporawfuente, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Dporawtipomedi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Dporawfecha, DbType.DateTime);
            dbProvider.AddColumnMapping(helper.Dporawvalor, DbType.Decimal);

            dbProvider.BulkInsert<DpoEstimadorRawTmpDTO>(entitys, nombreTabla);
        }

        public void TruncateTablaTemporal(string nombreTabla)
        {
            string query = string.Format(helper.SqlTruncateTablaTemporal, nombreTabla);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteRawsHistoricos(DateTime dporawfecha)
        {
            string fecha = dporawfecha.ToString("dd/MM/yyyy"); // Ejemp. 30/01/2023
            string query = string.Format(helper.SqlDeleteRawsHistoricos, fecha);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            dbProvider.ExecuteNonQuery(command);
        }


        // ------------------------------------------------------------------------------------------------------------------
        // Metodos para el proceso automatico de los archivos Raw
        // ------------------------------------------------------------------------------------------------------------------
        public List<DpoEstimadorRawTmpDTO> ListFileRawTmp()
        {
            List<DpoEstimadorRawTmpDTO> entitys = new List<DpoEstimadorRawTmpDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListFileRawTmp);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DpoEstimadorRawTmpDTO entity = new DpoEstimadorRawTmpDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iDpovarcodi = dr.GetOrdinal(helper.Prnvarcodi);
                    if (!dr.IsDBNull(iDpovarcodi)) entity.Prnvarcodi = Convert.ToInt32(dr.GetValue(iDpovarcodi));

                    int iDporawfuente = dr.GetOrdinal(helper.Dporawfuente);
                    if (!dr.IsDBNull(iDporawfuente)) entity.Dporawfuente = Convert.ToInt32(dr.GetValue(iDporawfuente));

                    int iDporawtipomedi = dr.GetOrdinal(helper.Dporawtipomedi);
                    if (!dr.IsDBNull(iDporawtipomedi)) entity.Dporawtipomedi = Convert.ToInt32(dr.GetValue(iDporawtipomedi));

                    int iDporawfecha = dr.GetOrdinal(helper.Dporawfecha);
                    if (!dr.IsDBNull(iDporawfecha)) entity.Dporawfecha = dr.GetDateTime(iDporawfecha);

                    int iDporawvalor = dr.GetOrdinal(helper.Dporawvalor);
                    if (!dr.IsDBNull(iDporawvalor)) entity.Dporawvalor = dr.GetDecimal(iDporawvalor);


                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void MigrarRawsProcesadosHora(string sufAnioMes, string fecha, string hora, DateTime dFecha60, string sTabla)
        {
            string sDpoRawFecha = fecha.Substring(0, 10);
            string sFecha60 = dFecha60.ToString("dd/MM/yyyy HH:mm");
            string query = string.Format(helper.SqlMigrarRawsProcesadosHora, sufAnioMes, fecha, hora, sDpoRawFecha, sFecha60, sTabla);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            dbProvider.ExecuteNonQuery(command);
        }

        public void InsertLogRaw(string nomArchivoRaw, DateTime fecArchivoRaw, string tipoProceso)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlInsertLogRaw);
            dbProvider.AddInParameter(command, helper.Nomarchivoraw, DbType.String, nomArchivoRaw.Trim());
            dbProvider.AddInParameter(command, helper.Fechaarchivoraw, DbType.DateTime, fecArchivoRaw);
            dbProvider.AddInParameter(command, helper.Tipo, DbType.String, tipoProceso.Trim());

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteLogRaw(string nomArchivoRaw, DateTime fecArchivoRaw)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteLogRaw);
            dbProvider.AddInParameter(command, helper.Nomarchivoraw, DbType.String, nomArchivoRaw.Trim());
            dbProvider.AddInParameter(command, helper.Fechaarchivoraw, DbType.DateTime, fecArchivoRaw);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<DpoEstimadorRawLogDTO> ListFilesLogRaw(string tipoProceso, DateTime dporawfecha)
        {
            List<DpoEstimadorRawLogDTO> entitys = new List<DpoEstimadorRawLogDTO>();

            string fecha = dporawfecha.ToString("dd/MM/yyyy");
            string query = string.Format(helper.SqlListFilesLogRaw, tipoProceso, fecha);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DpoEstimadorRawLogDTO entity = new DpoEstimadorRawLogDTO();

                    int iNomarchivoraw = dr.GetOrdinal(helper.Nomarchivoraw);
                    if (!dr.IsDBNull(iNomarchivoraw)) entity.NomArchivoRaw = dr.GetString(iNomarchivoraw);

                    int iFechaarchivoraw = dr.GetOrdinal(helper.Fechaarchivoraw);
                    if (!dr.IsDBNull(iFechaarchivoraw)) entity.FechaArchivoRaw = dr.GetDateTime(iFechaarchivoraw);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<DpoEstimadorRawLogDTO> ListFilesLogRawHora(string tipoProceso, DateTime dporawfechainicio, DateTime dporawfechafinal)
        {
            List<DpoEstimadorRawLogDTO> entitys = new List<DpoEstimadorRawLogDTO>();

            string fechaInicio = dporawfechainicio.ToString("dd/MM/yyyy HH:mm");
            string fechaFinal = dporawfechafinal.ToString("dd/MM/yyyy HH:mm");
            string query = string.Format(helper.SqlListFilesLogRawHora, tipoProceso, fechaInicio, fechaFinal);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DpoEstimadorRawLogDTO entity = new DpoEstimadorRawLogDTO();

                    int iNomarchivoraw = dr.GetOrdinal(helper.Nomarchivoraw);
                    if (!dr.IsDBNull(iNomarchivoraw)) entity.NomArchivoRaw = dr.GetString(iNomarchivoraw);

                    int iFechaarchivoraw = dr.GetOrdinal(helper.Fechaarchivoraw);
                    if (!dr.IsDBNull(iFechaarchivoraw)) entity.FechaArchivoRaw = dr.GetDateTime(iFechaarchivoraw);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public DpoEstimadorRawTmpDTO GetFileLogRaw(string nomarchivoraw, DateTime dporawfecha)
        {
            DpoEstimadorRawTmpDTO entity = null;

            string fecha = dporawfecha.ToString("dd/MM/yyyy HH:mm");
            string query = string.Format(helper.SqlGetFileLogRaw, nomarchivoraw, fecha);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<DpoEstimadorRawTmpDTO> GetByIdFileRaw(int ptomedicodi, int dpovarcodi, int dporawfuente, int dporawtipomedi, DateTime dporawfecha)
        {
            List<DpoEstimadorRawTmpDTO> entitys = new List<DpoEstimadorRawTmpDTO>();

            string fecha = dporawfecha.ToString("dd/MM/yyyy HH:mm");
            string query = string.Format(helper.SqlGetByIdFileRaw, ptomedicodi, dpovarcodi, dporawfuente, dporawtipomedi, fecha);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DpoEstimadorRawTmpDTO entity = new DpoEstimadorRawTmpDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iDpovarcodi = dr.GetOrdinal(helper.Prnvarcodi);
                    if (!dr.IsDBNull(iDpovarcodi)) entity.Prnvarcodi = Convert.ToInt32(dr.GetValue(iDpovarcodi));

                    int iDporawfuente = dr.GetOrdinal(helper.Dporawfuente);
                    if (!dr.IsDBNull(iDporawfuente)) entity.Dporawfuente = Convert.ToInt32(dr.GetValue(iDporawfuente));

                    int iDporawtipomedi = dr.GetOrdinal(helper.Dporawtipomedi);
                    if (!dr.IsDBNull(iDporawtipomedi)) entity.Dporawtipomedi = Convert.ToInt32(dr.GetValue(iDporawtipomedi));

                    int iDporawfecha = dr.GetOrdinal(helper.Dporawfecha);
                    if (!dr.IsDBNull(iDporawfecha)) entity.Dporawfecha = dr.GetDateTime(iDporawfecha);

                    int iDporawvalor = dr.GetOrdinal(helper.Dporawvalor);
                    if (!dr.IsDBNull(iDporawvalor)) entity.Dporawvalor = dr.GetDecimal(iDporawvalor);


                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void UpdateRawsProcesadosHora(string sufAnioMes, string campoHn)
        {
            string query = string.Format(helper.SqlUpdateRawsProcesadosHora, sufAnioMes, campoHn);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteRawsProcesados(int ptomedicodi, int dpovarcodi, int dporawfuente, int dporawtipomedi, DateTime dporawfecha)
        {
            string fecha = dporawfecha.ToString("dd/MM/yyyy HH:mm");
            string query = string.Format(helper.SqlDeleteRawsProcesados, ptomedicodi, dpovarcodi, dporawfuente, dporawtipomedi, fecha);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            dbProvider.ExecuteNonQuery(command);
        }

        public void InsertFileRaw(string nomArchivoRaw, DateTime fecArchivoRaw, string tipoProceso, string flag)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlInsertFileRaw);

            dbProvider.AddInParameter(command, helper.Nomarchivoraw, DbType.String, nomArchivoRaw.Trim());
            dbProvider.AddInParameter(command, helper.Fechaarchivoraw, DbType.DateTime, fecArchivoRaw);
            dbProvider.AddInParameter(command, helper.Tipo, DbType.String, tipoProceso.Trim());
            dbProvider.AddInParameter(command, helper.Flag, DbType.String, flag);

            dbProvider.ExecuteNonQuery(command);
        }

        public void UpdateFileRaw(string nomArchivoRaw, DateTime fecArchivoRaw, string tipoProceso, string flag)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateFileRaw);

            dbProvider.AddInParameter(command, helper.Tipo, DbType.String, tipoProceso.Trim());
            dbProvider.AddInParameter(command, helper.Flag, DbType.String, flag);
            dbProvider.AddInParameter(command, helper.Nomarchivoraw, DbType.String, nomArchivoRaw.Trim());
            dbProvider.AddInParameter(command, helper.Fechaarchivoraw, DbType.DateTime, fecArchivoRaw);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<DpoEstimadorRawFilesDTO> ListFilesRaw(int tipoProceso, DateTime dporawfecha)
        {
            string fechaInicio = dporawfecha.AddMinutes(1).ToString("dd/MM/yyyy HH:mm"); // Intervalo de incio
            string fechaFin = dporawfecha.AddDays(1).ToString("dd/MM/yyyy HH:mm"); // Intervalo final
            List<DpoEstimadorRawFilesDTO> entitys = new List<DpoEstimadorRawFilesDTO>();

            string query = string.Format(helper.SqlListFilesRaw, tipoProceso, fechaInicio, fechaFin);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DpoEstimadorRawFilesDTO entity = new DpoEstimadorRawFilesDTO();

                    int iNomarchivoraw = dr.GetOrdinal(helper.Nomarchivoraw);
                    if (!dr.IsDBNull(iNomarchivoraw)) entity.NomArchivoRaw = dr.GetString(iNomarchivoraw);

                    int iFechaarchivoraw = dr.GetOrdinal(helper.Fechaarchivoraw);
                    if (!dr.IsDBNull(iFechaarchivoraw)) entity.FechaArchivoRaw = dr.GetDateTime(iFechaarchivoraw);

                    int iTipo = dr.GetOrdinal(helper.Tipo);
                    if (!dr.IsDBNull(iTipo)) entity.Tipo = dr.GetString(iTipo);

                    int iFlag = dr.GetOrdinal(helper.Flag);
                    if (!dr.IsDBNull(iFlag)) entity.Flag = dr.GetString(iFlag);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        //ASSETEC: 20240610
        public DpoEstimadorRawFilesDTO GetFileRaw(string nomarchivoraw)
        {
            DpoEstimadorRawFilesDTO entity = null;

            string query = string.Format(helper.SqlGetFileRaw, nomarchivoraw);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new DpoEstimadorRawFilesDTO();

                    int iNomarchivoraw = dr.GetOrdinal(helper.Nomarchivoraw);
                    if (!dr.IsDBNull(iNomarchivoraw)) entity.NomArchivoRaw = dr.GetString(iNomarchivoraw);

                    int iFechaarchivoraw = dr.GetOrdinal(helper.Fechaarchivoraw);
                    if (!dr.IsDBNull(iFechaarchivoraw)) entity.FechaArchivoRaw = dr.GetDateTime(iFechaarchivoraw);

                    int iTipo = dr.GetOrdinal(helper.Tipo);
                    if (!dr.IsDBNull(iTipo)) entity.Tipo = dr.GetString(iTipo);

                    int iFlag = dr.GetOrdinal(helper.Flag);
                    if (!dr.IsDBNull(iFlag)) entity.Flag = dr.GetString(iFlag);
                }
            }

            return entity;
        }

        // ------------------------------------------------------------------------------------------------------------------
        // Metodos para el proceso automatico de costo marginales cada 30 minuto de los archivos Raw
        // ------------------------------------------------------------------------------------------------------------------
        public void UpdateRawsProcesados30Minutos(string sufAnioMes, string sFechaProceso)
        {
            string query = string.Format(helper.SqlUpdateRawsProcesados30Minutos, sufAnioMes, sFechaProceso);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteREstimadorRawFileByDiaProceso(DateTime fechaHoraInicio, DateTime fechaHoraFin, int tipo)
        {
            string fechaInicio = fechaHoraInicio.ToString("dd/MM/yyyy HH:mm"); // Intervalo de incio
            string fechaFin = fechaHoraFin.ToString("dd/MM/yyyy HH:mm"); // Intervalo final
            string query = string.Format(helper.SqlDeleteREstimadorRawFileByDiaProceso, tipo, fechaInicio, fechaFin);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteREstimadorRawLogByDiaProceso(DateTime fechaHoraInicio, DateTime fechaHoraFin, int tipo)
        {
            string fechaInicio = fechaHoraInicio.ToString("dd/MM/yyyy HH:mm"); // Intervalo de incio
            string fechaFin = fechaHoraFin.ToString("dd/MM/yyyy HH:mm"); // Intervalo final
            string query = string.Format(helper.SqlDeleteREstimadorRawLogByDiaProceso, tipo, fechaInicio, fechaFin);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteREstimadorRawTemporalByDiaProceso(DateTime fechaHoraInicio, DateTime fechaHoraFin)
        {
            string fechaInicio = fechaHoraInicio.ToString("dd/MM/yyyy HH:mm"); // Intervalo de incio
            string fechaFin = fechaHoraFin.ToString("dd/MM/yyyy HH:mm"); // Intervalo final
            string query = string.Format(helper.SqlDeleteREstimadorRawTemporalByDiaProceso, fechaInicio, fechaFin);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteREstimadorRawCmTemporalByDiaProceso(DateTime fechaHoraInicio, DateTime fechaHoraFin)
        {
            string fechaInicio = fechaHoraInicio.ToString("dd/MM/yyyy HH:mm"); // Intervalo de incio
            string fechaFin = fechaHoraFin.ToString("dd/MM/yyyy HH:mm"); // Intervalo final
            string query = string.Format(helper.SqlDeleteREstimadorRawCmTemporalByDiaProceso, fechaInicio, fechaFin);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteREstimadorRawByDiaProceso(string sufAnioMes, DateTime fechaHoraInicio, DateTime fechaHoraFin)
        {
            string fechaInicio = fechaHoraInicio.ToString("dd/MM/yyyy HH:mm"); // Intervalo de incio
            string fechaFin = fechaHoraFin.ToString("dd/MM/yyyy HH:mm"); // Intervalo final
            string query = string.Format(helper.SqlDeleteREstimadorRawByDiaProceso, sufAnioMes, fechaInicio, fechaFin);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteREstimadorRawFileByNomArchivo(string sNomArchivo, int tipo)
        {
            string query = string.Format(helper.SqlDeleteREstimadorRawFileByNomArchivo, sNomArchivo, tipo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            dbProvider.ExecuteNonQuery(command);
        }

        public void UpdateRawsProcesados60Minutos(string sufAnioMes, string sFechaProceso, string sFechaSiguiente)
        {
            string query = string.Format(helper.SqlUpdateRawsProcesados60Minutos, sufAnioMes, sFechaProceso, sFechaSiguiente);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            dbProvider.ExecuteNonQuery(command);
        }

        // ------------------------------------------------------------------------------------------------------------------
        // Metodos para el proceso manual de los archivos Raw
        // ------------------------------------------------------------------------------------------------------------------
        public List<DpoEstimadorRawManualDTO> ListFileRawManual()
        {
            List<DpoEstimadorRawManualDTO> entitys = new List<DpoEstimadorRawManualDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListEstimadorRawManual);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DpoEstimadorRawManualDTO entity = new DpoEstimadorRawManualDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iDpovarcodi = dr.GetOrdinal(helper.Prnvarcodi);
                    if (!dr.IsDBNull(iDpovarcodi)) entity.Prnvarcodi = Convert.ToInt32(dr.GetValue(iDpovarcodi));

                    int iDporawfuente = dr.GetOrdinal(helper.Dporawfuente);
                    if (!dr.IsDBNull(iDporawfuente)) entity.Dporawfuente = Convert.ToInt32(dr.GetValue(iDporawfuente));

                    int iDporawtipomedi = dr.GetOrdinal(helper.Dporawtipomedi);
                    if (!dr.IsDBNull(iDporawtipomedi)) entity.Dporawtipomedi = Convert.ToInt32(dr.GetValue(iDporawtipomedi));

                    int iDporawfecha = dr.GetOrdinal(helper.Dporawfecha);
                    if (!dr.IsDBNull(iDporawfecha)) entity.Dporawfecha = dr.GetDateTime(iDporawfecha);

                    int iDporawvalor = dr.GetOrdinal(helper.Dporawvalor);
                    if (!dr.IsDBNull(iDporawvalor)) entity.Dporawvalor = dr.GetDecimal(iDporawvalor);


                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<DpoEstimadorRawManualDTO> ListEstimadorRawManualHora()
        {
            List<DpoEstimadorRawManualDTO> entitys = new List<DpoEstimadorRawManualDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListEstimadorRawManualHora);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DpoEstimadorRawManualDTO entity = new DpoEstimadorRawManualDTO();

                    int iHora = dr.GetOrdinal(helper.Hora);
                    if (!dr.IsDBNull(iHora)) entity.Hora = dr.GetString(iHora);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<DpoEstimadorRawManualDTO> ListEstimadorRawManualMinuto()
        {
            List<DpoEstimadorRawManualDTO> entitys = new List<DpoEstimadorRawManualDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListEstimadorRawManualMinuto);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DpoEstimadorRawManualDTO entity = new DpoEstimadorRawManualDTO();

                    int iMinuto = dr.GetOrdinal(helper.Minuto);
                    if (!dr.IsDBNull(iMinuto)) entity.Minuto = dr.GetString(iMinuto);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void UpdateRawsProcesadosHoraManual(string sufAnioMes, string campoHn, DateTime fechaTemp, DateTime fecha)
        {
            string fechaHoraTemp = fechaTemp.ToString("dd/MM/yyyy HH:mm");
            string fechaHora = fecha.ToString("dd/MM/yyyy HH:mm");
            string query = string.Format(helper.SqlUpdateRawManual, sufAnioMes, fechaHoraTemp, fechaHora, campoHn);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            dbProvider.ExecuteNonQuery(command);
        }

        // ------------------------------------------------------------------------------------------------------------------
        // Metodos para reportes exportables a excel
        // ------------------------------------------------------------------------------------------------------------------
        public List<DpoEstimadorRawFilesDTO> ListFilesRawPorMinuto()
        {
            List<DpoEstimadorRawFilesDTO> entitys = new List<DpoEstimadorRawFilesDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListFilesRawPorMinuto);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DpoEstimadorRawFilesDTO entity = new DpoEstimadorRawFilesDTO();

                    int iNomarchivoraw = dr.GetOrdinal(helper.Nomarchivoraw);
                    if (!dr.IsDBNull(iNomarchivoraw)) entity.NomArchivoRaw = dr.GetString(iNomarchivoraw);

                    int iFechaarchivoraw = dr.GetOrdinal(helper.Fechaarchivoraw);
                    if (!dr.IsDBNull(iFechaarchivoraw)) entity.FechaArchivoRaw = dr.GetDateTime(iFechaarchivoraw);

                    int iTipo = dr.GetOrdinal(helper.Tipo);
                    if (!dr.IsDBNull(iTipo)) entity.Tipo = dr.GetString(iTipo);

                    int iFlag = dr.GetOrdinal(helper.Flag);
                    if (!dr.IsDBNull(iFlag)) entity.Flag = dr.GetString(iFlag);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<DpoEstimadorRawFilesDTO> ListFilesRawIeod()
        {
            List<DpoEstimadorRawFilesDTO> entitys = new List<DpoEstimadorRawFilesDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListFilesRawIeod);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DpoEstimadorRawFilesDTO entity = new DpoEstimadorRawFilesDTO();

                    int iNomarchivoraw = dr.GetOrdinal(helper.Nomarchivoraw);
                    if (!dr.IsDBNull(iNomarchivoraw)) entity.NomArchivoRaw = dr.GetString(iNomarchivoraw);

                    int iFechaarchivoraw = dr.GetOrdinal(helper.Fechaarchivoraw);
                    if (!dr.IsDBNull(iFechaarchivoraw)) entity.FechaArchivoRaw = dr.GetDateTime(iFechaarchivoraw);

                    int iTipo = dr.GetOrdinal(helper.Tipo);
                    if (!dr.IsDBNull(iTipo)) entity.Tipo = dr.GetString(iTipo);

                    int iFlag = dr.GetOrdinal(helper.Flag);
                    if (!dr.IsDBNull(iFlag)) entity.Flag = dr.GetString(iFlag);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<DpoEstimadorRawDTO> ObtenerPorRangoFuente(
            int dporawfuente,
            string fecini,
            string fecfin)
        {
            List<DpoEstimadorRawDTO> entitys = new List<DpoEstimadorRawDTO>();
            string query = string.Format(helper.SqlObtenerPorRangoFuente,
                dporawfuente, fecini,
                fecfin);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DpoEstimadorRawDTO entity = helper.CreateEntity(dr);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<DpoEstimadorRawDTO> DatosPorPtoMedicion(
            string nombreTabla, int dporawfuente,
            int ptomedicion)
        {
            List<DpoEstimadorRawDTO> entitys = new List<DpoEstimadorRawDTO>();
            string query = string.Format(helper.SqlDatosPorPtoMedicion,
                nombreTabla, dporawfuente,
                ptomedicion);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DpoEstimadorRawDTO entity = helper.CreateEntity(dr);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public string UpdateTMP()
        {
            string resultado = "OK";
            try
            {
                string sIndex = "idx_dpo_estimadorraw_tmp";
                string query = string.Format(helper.SqlUpdateTMP, sIndex);
                DbCommand command = dbProvider.GetSqlStringCommand(query);
                dbProvider.ExecuteNonQuery(command);

                sIndex = "idx_estimrawtmp_fech";
                query = string.Format(helper.SqlUpdateTMP, sIndex);
                command = dbProvider.GetSqlStringCommand(query);
                dbProvider.ExecuteNonQuery(command);
            }
            catch (Exception e)
            {
                resultado = e.Message;

            }
            return resultado;
        }
        public string UpdateRAW(DateTime dporawfecha)
        {
            string resultado = "OK";
            try
            {
                string sSufijoTabla = dporawfecha.ToString("yyyyMM");
                string sIndex = "idx_dporawfuente_" + sSufijoTabla;
                string query = string.Format(helper.SqlUpdateTMP, sIndex);
                DbCommand command = dbProvider.GetSqlStringCommand(query);
                dbProvider.ExecuteNonQuery(command);

                sIndex = "idx_dporawfecha_" + sSufijoTabla;
                query = string.Format(helper.SqlUpdateTMP, sIndex);
                command = dbProvider.GetSqlStringCommand(query);
                dbProvider.ExecuteNonQuery(command);

                sIndex = "idx_dpoestraw_" + sSufijoTabla;
                query = string.Format(helper.SqlUpdateTMP, sIndex);
                command = dbProvider.GetSqlStringCommand(query);
                dbProvider.ExecuteNonQuery(command);
            }
            catch (Exception e)
            {
                resultado = e.Message;

            }
            return resultado;
        }

        public string DeleteEstimadorRawTemporal()
        {
            string resultado = "OK";
            try
            {
                //TruncateTablaTemporal("dpo_estimadorraw_tmp");
                TruncateTablaTemporal("dpo_estimadorraw_manual");
                TruncateTablaTemporal("dpo_estimadorraw_cmtmp");
            }
            catch (Exception e)
            {
                resultado = e.Message;

            }
            return resultado;
        }
        public DpoEstimadorRawFilesDTO ObtenerUltimoEstimadorRawFiles()
        {
            DpoEstimadorRawFilesDTO entity = null;

            string query = string.Format(helper.SqlObtenerUltimoEstimadorRawFiles);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new DpoEstimadorRawFilesDTO();

                    int iNomarchivoraw = dr.GetOrdinal(helper.Nomarchivoraw);
                    if (!dr.IsDBNull(iNomarchivoraw)) entity.NomArchivoRaw = dr.GetString(iNomarchivoraw);

                    int iFechaarchivoraw = dr.GetOrdinal(helper.Fechaarchivoraw);
                    if (!dr.IsDBNull(iFechaarchivoraw)) entity.FechaArchivoRaw = dr.GetDateTime(iFechaarchivoraw);

                    int iTipo = dr.GetOrdinal(helper.Tipo);
                    if (!dr.IsDBNull(iTipo)) entity.Tipo = dr.GetString(iTipo);

                    int iFlag = dr.GetOrdinal(helper.Flag);
                    if (!dr.IsDBNull(iFlag)) entity.Flag = dr.GetString(iFlag);
                }
            }

            return entity;
        }

        public DpoEstimadorRawFilesDTO ObtenerEstimadorRawFilesFlag()
        {
            DpoEstimadorRawFilesDTO entity = null;

            string query = string.Format(helper.SqlObtenerEstimadorRawFilesFlag);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new DpoEstimadorRawFilesDTO();

                    int iNomarchivoraw = dr.GetOrdinal(helper.Nomarchivoraw);
                    if (!dr.IsDBNull(iNomarchivoraw)) entity.NomArchivoRaw = dr.GetString(iNomarchivoraw);

                    int iFechaarchivoraw = dr.GetOrdinal(helper.Fechaarchivoraw);
                    if (!dr.IsDBNull(iFechaarchivoraw)) entity.FechaArchivoRaw = dr.GetDateTime(iFechaarchivoraw);

                    int iTipo = dr.GetOrdinal(helper.Tipo);
                    if (!dr.IsDBNull(iTipo)) entity.Tipo = dr.GetString(iTipo);

                    int iFlag = dr.GetOrdinal(helper.Flag);
                    if (!dr.IsDBNull(iFlag)) entity.Flag = dr.GetString(iFlag);
                }
            }

            return entity;
        }

        public List<DpoEstimadorRawDTO> verificarFaltantesDia(DateTime dporawfecha)
        {

            List<DpoEstimadorRawDTO> entitys = new List<DpoEstimadorRawDTO>();
            //dporawfecha = DD/MM/YYYY
            string sSufijoTabla = dporawfecha.ToString("yyyyMM");
            string sFecha = dporawfecha.ToString("dd/MM/yyyy");
            string query = string.Format(helper.SqlVerificarFaltantesDia, sFecha, sSufijoTabla);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DpoEstimadorRawDTO entity = new DpoEstimadorRawDTO();

                    int iDporawfecha = dr.GetOrdinal(helper.Dporawfecha);
                    if (!dr.IsDBNull(iDporawfecha)) entity.Dporawfecha = dr.GetDateTime(iDporawfecha);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<DpoEstimadorRawDTO> ObtenerDemActivaPorDia(
           string nombreTabla, int dporawfuente, string ptomedicodi,
           string prnvarcodi, string dporawfecha)
        {
            List<DpoEstimadorRawDTO> entitys = new List<DpoEstimadorRawDTO>();

            DateTime fecFin = DateTime.ParseExact(
                dporawfecha, ConstantesBase.FormatoFechaBase,
                CultureInfo.InvariantCulture).AddDays(1);
            string strFecha = fecFin.ToString(ConstantesBase.FormatoFechaBase);

            string query = string.Format(helper.SqlObtenerDemActivaPorDia,
                nombreTabla, dporawfuente, ptomedicodi, prnvarcodi,
                dporawfecha, strFecha);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            try
            {
                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        DpoEstimadorRawDTO entity = new DpoEstimadorRawDTO();

                        int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                        if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                        int iIeod = dr.GetOrdinal(helper.Ieod);
                        if (!dr.IsDBNull(iPtomedicodi)) entity.Ieod = Convert.ToInt32(dr.GetValue(iIeod));

                        int iDpovarcodi = dr.GetOrdinal(helper.Prnvarcodi);
                        if (!dr.IsDBNull(iDpovarcodi)) entity.Prnvarcodi = Convert.ToInt32(dr.GetValue(iDpovarcodi));

                        int iDporawfuente = dr.GetOrdinal(helper.Dporawfuente);
                        if (!dr.IsDBNull(iDporawfuente)) entity.Dporawfuente = Convert.ToInt32(dr.GetValue(iDporawfuente));

                        int iDporawtipomedi = dr.GetOrdinal(helper.Dporawtipomedi);
                        if (!dr.IsDBNull(iDporawtipomedi)) entity.Dporawtipomedi = Convert.ToInt32(dr.GetValue(iDporawtipomedi));

                        int iDporawfecha = dr.GetOrdinal(helper.Dporawfecha);
                        if (!dr.IsDBNull(iDporawfecha)) entity.Dporawfecha = dr.GetDateTime(iDporawfecha);

                        int ih1 = dr.GetOrdinal(helper.Dporawvalorh1);
                        if (!dr.IsDBNull(ih1)) entity.Dporawvalorh1 = dr.GetDecimal(ih1);

                        int ih2 = dr.GetOrdinal(helper.Dporawvalorh2);
                        if (!dr.IsDBNull(ih2)) entity.Dporawvalorh2 = dr.GetDecimal(ih2);

                        int ih3 = dr.GetOrdinal(helper.Dporawvalorh3);
                        if (!dr.IsDBNull(ih3)) entity.Dporawvalorh3 = dr.GetDecimal(ih3);

                        int ih4 = dr.GetOrdinal(helper.Dporawvalorh4);
                        if (!dr.IsDBNull(ih4)) entity.Dporawvalorh4 = dr.GetDecimal(ih4);

                        int ih5 = dr.GetOrdinal(helper.Dporawvalorh5);
                        if (!dr.IsDBNull(ih5)) entity.Dporawvalorh5 = dr.GetDecimal(ih5);

                        int ih6 = dr.GetOrdinal(helper.Dporawvalorh6);
                        if (!dr.IsDBNull(ih6)) entity.Dporawvalorh6 = dr.GetDecimal(ih6);

                        int ih7 = dr.GetOrdinal(helper.Dporawvalorh7);
                        if (!dr.IsDBNull(ih7)) entity.Dporawvalorh7 = dr.GetDecimal(ih7);

                        int ih8 = dr.GetOrdinal(helper.Dporawvalorh8);
                        if (!dr.IsDBNull(ih8)) entity.Dporawvalorh8 = dr.GetDecimal(ih8);

                        int ih9 = dr.GetOrdinal(helper.Dporawvalorh9);
                        if (!dr.IsDBNull(ih9)) entity.Dporawvalorh9 = dr.GetDecimal(ih9);

                        int ih10 = dr.GetOrdinal(helper.Dporawvalorh10);
                        if (!dr.IsDBNull(ih10)) entity.Dporawvalorh10 = dr.GetDecimal(ih10);

                        int ih11 = dr.GetOrdinal(helper.Dporawvalorh11);
                        if (!dr.IsDBNull(ih11)) entity.Dporawvalorh11 = dr.GetDecimal(ih11);

                        int ih12 = dr.GetOrdinal(helper.Dporawvalorh12);
                        if (!dr.IsDBNull(ih12)) entity.Dporawvalorh12 = dr.GetDecimal(ih12);

                        int ih13 = dr.GetOrdinal(helper.Dporawvalorh13);
                        if (!dr.IsDBNull(ih13)) entity.Dporawvalorh13 = dr.GetDecimal(ih13);

                        int ih14 = dr.GetOrdinal(helper.Dporawvalorh14);
                        if (!dr.IsDBNull(ih14)) entity.Dporawvalorh14 = dr.GetDecimal(ih14);

                        int ih15 = dr.GetOrdinal(helper.Dporawvalorh15);
                        if (!dr.IsDBNull(ih15)) entity.Dporawvalorh15 = dr.GetDecimal(ih15);

                        int ih16 = dr.GetOrdinal(helper.Dporawvalorh16);
                        if (!dr.IsDBNull(ih16)) entity.Dporawvalorh16 = dr.GetDecimal(ih16);

                        int ih17 = dr.GetOrdinal(helper.Dporawvalorh17);
                        if (!dr.IsDBNull(ih17)) entity.Dporawvalorh17 = dr.GetDecimal(ih17);

                        int ih18 = dr.GetOrdinal(helper.Dporawvalorh18);
                        if (!dr.IsDBNull(ih18)) entity.Dporawvalorh18 = dr.GetDecimal(ih18);

                        int ih19 = dr.GetOrdinal(helper.Dporawvalorh19);
                        if (!dr.IsDBNull(ih19)) entity.Dporawvalorh19 = dr.GetDecimal(ih19);

                        int ih20 = dr.GetOrdinal(helper.Dporawvalorh20);
                        if (!dr.IsDBNull(ih20)) entity.Dporawvalorh20 = dr.GetDecimal(ih20);

                        int ih21 = dr.GetOrdinal(helper.Dporawvalorh21);
                        if (!dr.IsDBNull(ih21)) entity.Dporawvalorh21 = dr.GetDecimal(ih21);

                        int ih22 = dr.GetOrdinal(helper.Dporawvalorh22);
                        if (!dr.IsDBNull(ih22)) entity.Dporawvalorh22 = dr.GetDecimal(ih22);

                        int ih23 = dr.GetOrdinal(helper.Dporawvalorh23);
                        if (!dr.IsDBNull(ih23)) entity.Dporawvalorh23 = dr.GetDecimal(ih23);

                        int ih24 = dr.GetOrdinal(helper.Dporawvalorh24);
                        if (!dr.IsDBNull(ih24)) entity.Dporawvalorh24 = dr.GetDecimal(ih24);

                        int ih25 = dr.GetOrdinal(helper.Dporawvalorh25);
                        if (!dr.IsDBNull(ih25)) entity.Dporawvalorh25 = dr.GetDecimal(ih25);

                        int ih26 = dr.GetOrdinal(helper.Dporawvalorh26);
                        if (!dr.IsDBNull(ih26)) entity.Dporawvalorh26 = dr.GetDecimal(ih26);

                        int ih27 = dr.GetOrdinal(helper.Dporawvalorh27);
                        if (!dr.IsDBNull(ih27)) entity.Dporawvalorh27 = dr.GetDecimal(ih27);

                        int ih28 = dr.GetOrdinal(helper.Dporawvalorh28);
                        if (!dr.IsDBNull(ih28)) entity.Dporawvalorh28 = dr.GetDecimal(ih28);

                        int ih29 = dr.GetOrdinal(helper.Dporawvalorh29);
                        if (!dr.IsDBNull(ih29)) entity.Dporawvalorh29 = dr.GetDecimal(ih29);

                        int ih30 = dr.GetOrdinal(helper.Dporawvalorh30);
                        if (!dr.IsDBNull(ih30)) entity.Dporawvalorh30 = dr.GetDecimal(ih30);

                        int ih31 = dr.GetOrdinal(helper.Dporawvalorh31);
                        if (!dr.IsDBNull(ih31)) entity.Dporawvalorh31 = dr.GetDecimal(ih31);

                        int ih32 = dr.GetOrdinal(helper.Dporawvalorh32);
                        if (!dr.IsDBNull(ih32)) entity.Dporawvalorh32 = dr.GetDecimal(ih32);

                        int ih33 = dr.GetOrdinal(helper.Dporawvalorh33);
                        if (!dr.IsDBNull(ih33)) entity.Dporawvalorh33 = dr.GetDecimal(ih33);

                        int ih34 = dr.GetOrdinal(helper.Dporawvalorh34);
                        if (!dr.IsDBNull(ih34)) entity.Dporawvalorh34 = dr.GetDecimal(ih34);

                        int ih35 = dr.GetOrdinal(helper.Dporawvalorh35);
                        if (!dr.IsDBNull(ih35)) entity.Dporawvalorh35 = dr.GetDecimal(ih35);

                        int ih36 = dr.GetOrdinal(helper.Dporawvalorh36);
                        if (!dr.IsDBNull(ih36)) entity.Dporawvalorh36 = dr.GetDecimal(ih36);

                        int ih37 = dr.GetOrdinal(helper.Dporawvalorh37);
                        if (!dr.IsDBNull(ih37)) entity.Dporawvalorh37 = dr.GetDecimal(ih37);

                        int ih38 = dr.GetOrdinal(helper.Dporawvalorh38);
                        if (!dr.IsDBNull(ih38)) entity.Dporawvalorh38 = dr.GetDecimal(ih38);

                        int ih39 = dr.GetOrdinal(helper.Dporawvalorh39);
                        if (!dr.IsDBNull(ih39)) entity.Dporawvalorh39 = dr.GetDecimal(ih39);

                        int ih40 = dr.GetOrdinal(helper.Dporawvalorh40);
                        if (!dr.IsDBNull(ih40)) entity.Dporawvalorh40 = dr.GetDecimal(ih40);

                        int ih41 = dr.GetOrdinal(helper.Dporawvalorh41);
                        if (!dr.IsDBNull(ih41)) entity.Dporawvalorh41 = dr.GetDecimal(ih41);

                        int ih42 = dr.GetOrdinal(helper.Dporawvalorh42);
                        if (!dr.IsDBNull(ih42)) entity.Dporawvalorh42 = dr.GetDecimal(ih42);

                        int ih43 = dr.GetOrdinal(helper.Dporawvalorh43);
                        if (!dr.IsDBNull(ih43)) entity.Dporawvalorh43 = dr.GetDecimal(ih43);

                        int ih44 = dr.GetOrdinal(helper.Dporawvalorh44);
                        if (!dr.IsDBNull(ih44)) entity.Dporawvalorh44 = dr.GetDecimal(ih44);

                        int ih45 = dr.GetOrdinal(helper.Dporawvalorh45);
                        if (!dr.IsDBNull(ih45)) entity.Dporawvalorh45 = dr.GetDecimal(ih45);

                        int ih46 = dr.GetOrdinal(helper.Dporawvalorh46);
                        if (!dr.IsDBNull(ih46)) entity.Dporawvalorh46 = dr.GetDecimal(ih46);

                        int ih47 = dr.GetOrdinal(helper.Dporawvalorh47);
                        if (!dr.IsDBNull(ih47)) entity.Dporawvalorh47 = dr.GetDecimal(ih47);

                        int ih48 = dr.GetOrdinal(helper.Dporawvalorh48);
                        if (!dr.IsDBNull(ih48)) entity.Dporawvalorh48 = dr.GetDecimal(ih48);

                        int ih49 = dr.GetOrdinal(helper.Dporawvalorh49);
                        if (!dr.IsDBNull(ih49)) entity.Dporawvalorh49 = dr.GetDecimal(ih49);

                        int ih50 = dr.GetOrdinal(helper.Dporawvalorh50);
                        if (!dr.IsDBNull(ih50)) entity.Dporawvalorh50 = dr.GetDecimal(ih50);

                        int ih51 = dr.GetOrdinal(helper.Dporawvalorh51);
                        if (!dr.IsDBNull(ih51)) entity.Dporawvalorh51 = dr.GetDecimal(ih51);

                        int ih52 = dr.GetOrdinal(helper.Dporawvalorh52);
                        if (!dr.IsDBNull(ih52)) entity.Dporawvalorh52 = dr.GetDecimal(ih52);

                        int ih53 = dr.GetOrdinal(helper.Dporawvalorh53);
                        if (!dr.IsDBNull(ih53)) entity.Dporawvalorh53 = dr.GetDecimal(ih53);

                        int ih54 = dr.GetOrdinal(helper.Dporawvalorh54);
                        if (!dr.IsDBNull(ih54)) entity.Dporawvalorh54 = dr.GetDecimal(ih54);

                        int ih55 = dr.GetOrdinal(helper.Dporawvalorh55);
                        if (!dr.IsDBNull(ih55)) entity.Dporawvalorh55 = dr.GetDecimal(ih55);

                        int ih56 = dr.GetOrdinal(helper.Dporawvalorh56);
                        if (!dr.IsDBNull(ih56)) entity.Dporawvalorh56 = dr.GetDecimal(ih56);

                        int ih57 = dr.GetOrdinal(helper.Dporawvalorh57);
                        if (!dr.IsDBNull(ih57)) entity.Dporawvalorh57 = dr.GetDecimal(ih57);

                        int ih58 = dr.GetOrdinal(helper.Dporawvalorh58);
                        if (!dr.IsDBNull(ih58)) entity.Dporawvalorh58 = dr.GetDecimal(ih58);

                        int ih59 = dr.GetOrdinal(helper.Dporawvalorh59);
                        if (!dr.IsDBNull(ih59)) entity.Dporawvalorh59 = dr.GetDecimal(ih59);

                        int ih60 = dr.GetOrdinal(helper.Dporawvalorh60);
                        if (!dr.IsDBNull(ih60)) entity.Dporawvalorh60 = dr.GetDecimal(ih60);

                        entitys.Add(entity);
                    }
                }
            }
            catch (Exception ex)
            {
                return entitys;
            }

            return entitys;
        }

        public List<DpoEstimadorRawDTO> ReporteDatosEstimadorxHora(
            string nomTabla, string fecIni, string fecFin,
            int tipo)
        {
            List<DpoEstimadorRawDTO> entitys = new List<DpoEstimadorRawDTO>();
            string query = string.Format(helper.SqlReporteDatosEstimadorxHora,
                nomTabla, fecIni, fecFin, tipo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            try
            {
                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        DpoEstimadorRawDTO entity = new DpoEstimadorRawDTO();

                        int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                        if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                        int iPrnvarnom = dr.GetOrdinal(helper.Prnvarnom);
                        if (!dr.IsDBNull(iPrnvarnom)) entity.Prnvarnom = dr.GetString(iPrnvarnom);

                        int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                        if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                        int iPrnvarcodi = dr.GetOrdinal(helper.Prnvarcodi);
                        if (!dr.IsDBNull(iPrnvarcodi)) entity.Prnvarcodi = Convert.ToInt32(dr.GetValue(iPrnvarcodi));

                        int iDporawfuente = dr.GetOrdinal(helper.Dporawfuente);
                        if (!dr.IsDBNull(iDporawfuente)) entity.Dporawfuente = Convert.ToInt32(dr.GetValue(iDporawfuente));

                        int iDporawtipomedi = dr.GetOrdinal(helper.Dporawtipomedi);
                        if (!dr.IsDBNull(iDporawtipomedi)) entity.Dporawtipomedi = Convert.ToInt32(dr.GetValue(iDporawtipomedi));

                        int iDporawfecha = dr.GetOrdinal(helper.Dporawfecha);
                        if (!dr.IsDBNull(iDporawfecha)) entity.Dporawfecha = dr.GetDateTime(iDporawfecha);

                        for (int i = 1; i <= 60; i++)
                        {
                            int iOrdinal = dr.GetOrdinal($"DPORAWVALORH{i}");

                            if (!dr.IsDBNull(iOrdinal))
                                entity.GetType().GetProperty($"Dporawvalorh{i}")
                                    .SetValue(entity, dr.GetDecimal(iOrdinal));
                        }

                        entitys.Add(entity);
                    }
                }
            }
            catch (Exception ex)
            {
                return entitys;
            }

            return entitys;
        }
    }
}
