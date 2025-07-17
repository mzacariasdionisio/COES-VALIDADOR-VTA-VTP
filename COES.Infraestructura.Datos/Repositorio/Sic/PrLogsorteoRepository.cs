using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla PR_LOGSORTEO
    /// </summary>
    public class PrLogsorteoRepository: RepositoryBase, IPrLogsorteoRepository
    {
        public PrLogsorteoRepository(string strConn): base(strConn)
        {
        }

        PrLogsorteoHelper helper = new PrLogsorteoHelper();

        

        public void Update(PrLogsorteoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Logusuario, DbType.String, entity.Logusuario);
            dbProvider.AddInParameter(command, helper.Logdescrip, DbType.String, entity.Logdescrip);
            dbProvider.AddInParameter(command, helper.Logtipo, DbType.String, entity.Logtipo);
            dbProvider.AddInParameter(command, helper.Logcoordinador, DbType.String, entity.Logcoordinador);
            dbProvider.AddInParameter(command, helper.Logdocoes, DbType.String, entity.Logdocoes);
            dbProvider.AddInParameter(command, helper.Logfecha, DbType.DateTime, entity.Logfecha);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(DateTime logfecha)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Logfecha, DbType.DateTime, logfecha);

            dbProvider.ExecuteNonQuery(command);
        }

        public PrLogsorteoDTO GetById(DateTime logfecha)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Logfecha, DbType.DateTime, logfecha);
            PrLogsorteoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PrLogsorteoDTO> List()
        {
            List<PrLogsorteoDTO> entitys = new List<PrLogsorteoDTO>();
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

        public List<PrLogsorteoDTO> GetByCriteria()
        {
            List<PrLogsorteoDTO> entitys = new List<PrLogsorteoDTO>();

            //string sql = string.Format(helper.SqlGetByCriteria, fecha.ToString(ConstantesBase.FormatoFecha));

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


        public List<PrLogsorteoDTO> ObtenerSituacionUnidades(DateTime fechaInicio, DateTime fechaFin)
        {
            List<PrLogsorteoDTO> entitys = new List<PrLogsorteoDTO>();

            string sql = string.Format(helper.SqlObtenerSituacionUnidades, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrLogsorteoDTO entity = new PrLogsorteoDTO();

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iFechaini = dr.GetOrdinal(helper.Fechaini);
                    if (!dr.IsDBNull(iFechaini)) entity.Fechaini = dr.GetDateTime(iFechaini);

                    int iFechafin = dr.GetOrdinal(helper.Fechafin);
                    if (!dr.IsDBNull(iFechafin)) entity.Fechafin = dr.GetDateTime(iFechafin);

                    int iSubcausadesc = dr.GetOrdinal(helper.Subcausadesc);
                    if (!dr.IsDBNull(iSubcausadesc)) entity.Subcausadesc = dr.GetString(iSubcausadesc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrLogsorteoDTO> ObtenerMantenimientos(string clase, int idClase, DateTime fechaInicio, DateTime fechaFin)
        {
            List<PrLogsorteoDTO> entitys = new List<PrLogsorteoDTO>();

            string sql = string.Format(helper.SqlObtenerMantenimientos, clase, idClase, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrLogsorteoDTO entity = new PrLogsorteoDTO();

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iFechaini = dr.GetOrdinal(helper.Fechaini);
                    if (!dr.IsDBNull(iFechaini)) entity.Fechaini = dr.GetDateTime(iFechaini);

                    int iFechafin = dr.GetOrdinal(helper.Fechafin);
                    if (!dr.IsDBNull(iFechafin)) entity.Fechafin = dr.GetDateTime(iFechafin);

                    int iEvenclase = dr.GetOrdinal(helper.Evenclase);
                    if (!dr.IsDBNull(iEvenclase)) entity.Evenclase = dr.GetString(iEvenclase);

                    int iEvendescrip = dr.GetOrdinal(helper.Evendescrip);
                    if (!dr.IsDBNull(iEvendescrip)) entity.Evendescrip = dr.GetString(iEvendescrip);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ConteoCorreoTipo(string Tipo, DateTime Fecha)
        {
            String sql = String.Format(helper.TotalConteoTipo,Tipo, Fecha.ToString(ConstantesBase.FormatoFecha));            
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);

            return 0;
        }

        public int TotalConteoTipo(string Tipo, DateTime Fecha)
        {
            String sql = String.Format(helper.TotalConteoTipo, Tipo, Fecha.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);

            return 0;
        }

        public int ConteoBalotaNegra(DateTime Fecha)
        {
            String sql = String.Format(helper.TotalBalotaNegra, Fecha.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);

            return 0;
        }

        public string EquipoPrueba(DateTime Fecha)
        {
            String sql = String.Format(helper.EquipoPrueba, Fecha.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object nombre = dbProvider.ExecuteScalar(command);

            if (nombre != null) return Convert.ToString(nombre);

            return "";
        }

        public int EquicodiPrueba(DateTime Fecha)
        {
            String sql = String.Format(helper.EquicodiPrueba, Fecha.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);

            return -1;
        }

        #region MigracionSGOCOES-GrupoB
        public List<PrLogsorteoDTO> ListaLogSorteo(DateTime fecIni)
        {
            List<PrLogsorteoDTO> entitys = new List<PrLogsorteoDTO>();
            String sql = String.Format(helper.SqlListaLogSorteo, fecIni.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }
        #endregion

        #region FIT SGOCOES func A

        public bool DeleteEquipo(DateTime logfecha)
        {
            var delete = true;
            try
            {
                string sql = string.Format(helper.SqlDeleteEquipo, logfecha);
                DbCommand command = dbProvider.GetSqlStringCommand(sql);
                dbProvider.ExecuteNonQuery(command);
            }
            catch
            {
                delete = false;
            }
            return delete;
        }

        public List<PrLogsorteoDTO> GetByCriteria(DateTime fecha)
        {
            List<PrLogsorteoDTO> entitys = new List<PrLogsorteoDTO>();
            var fecha1 = fecha.ToString(ConstantesBase.FormatoFecha);
            string sql = string.Format(helper.SqlGetByCriteriaHistorico, fecha1);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }


        public int Save(PrLogsorteoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Logusuario, DbType.String, entity.Logusuario);
            dbProvider.AddInParameter(command, helper.Logfecha, DbType.DateTime, entity.Logfecha);
            dbProvider.AddInParameter(command, helper.Logdescrip, DbType.String, entity.Logdescrip);
            dbProvider.AddInParameter(command, helper.Logtipo, DbType.String, entity.Logtipo);
            dbProvider.AddInParameter(command, helper.Logcoordinador, DbType.String, entity.Logcoordinador);
            dbProvider.AddInParameter(command, helper.Logdocoes, DbType.String, entity.Logdocoes);

            return dbProvider.ExecuteNonQuery(command);
        }

        public List<PrLogsorteoDTO> eq_equipo()
        {
            List<PrLogsorteoDTO> entitys = new List<PrLogsorteoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.Sqleq_equipo);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateEQ(dr));
                }
            }

            return entitys;
        }

        public List<PrLogsorteoDTO> eq_central()
        {
            List<PrLogsorteoDTO> entitys = new List<PrLogsorteoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.Sqleq_central);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateEQ(dr));
                }
            }

            return entitys;
        }

        public List<PrLogsorteoDTO> eve_mantto()
        {
            var fecha = DateTime.Now;
            List<PrLogsorteoDTO> entitys = new List<PrLogsorteoDTO>();

            //string sql = string.Format(helper.Sqleve_mantto, fecha.ToString(ConstantesBase.FormatoFecha));

            #region Movisoft - Ticket 19434
            string sql = string.Format(helper.Sqleve_mantto, fecha.ToString(ConstantesBase.FormatoFecha), fecha.AddDays(1).ToString(ConstantesBase.FormatoFecha), fecha.ToString(ConstantesBase.FormatoFecha), fecha.AddDays(1).ToString(ConstantesBase.FormatoFecha));
            #endregion

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateEve(dr));
                }
            }

            return entitys;
        }

        public List<PrLogsorteoDTO> eve_indisponibilidad()
        {
            var fecha = DateTime.Now;
            List<PrLogsorteoDTO> entitys = new List<PrLogsorteoDTO>();
            string sql = string.Format(helper.Sqleve_indisponibilidad, fecha.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateInd(dr));
                }
            }

            return entitys;
        }

        public List<PrLogsorteoDTO> eve_horaoperacion()
        {
            var fecha = DateTime.Now;
            List<PrLogsorteoDTO> entitys = new List<PrLogsorteoDTO>();
            string sql = string.Format(helper.Sqleve_horaoperacion, fecha.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrLogsorteoDTO entity = helper.CreateOpe(dr);

                    int iGrupocodi = dr.GetOrdinal(helper.GRUPOCODI);
                    if (!dr.IsDBNull(iGrupocodi)) entity.GRUPOCODI = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrLogsorteoDTO> eve_pruebaunidad()
        {
            var fecha = DateTime.Now;
            List<PrLogsorteoDTO> entitys = new List<PrLogsorteoDTO>();
            string sql = string.Format(helper.Sqleve_pruebaunidad, fecha.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateUnd(dr));
                }
            }

            return entitys;
        }

        public List<Prequipos_validosDTO> equipos_validos(int i_codigo)
        {
            List<Prequipos_validosDTO> entitys = new List<Prequipos_validosDTO>();
            string sql = string.Format(helper.Sqlequipos_validos, i_codigo);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateEqv(dr));
                }
            }

            return entitys;
        }

        public List<Prequipos_validosDTO> eve_mantto_calderos(int i_codigo, DateTime Today, DateTime tomorrow, DateTime mediodia)
        {
            List<Prequipos_validosDTO> entitys = new List<Prequipos_validosDTO>();
            string sql = string.Format(helper.Sqleve_mantto_calderos, i_codigo, Today, tomorrow, mediodia);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateEqv(dr));
                }
            }

            return entitys;
        }

        public int InsertPrSorteo(int equicodi, DateTime fecha, string prueba, int emprcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlInsertPrSorteo);

            dbProvider.AddInParameter(command, helper.Equicodi, DbType.String, equicodi);
            dbProvider.AddInParameter(command, helper.fecha, DbType.DateTime, fecha);
            dbProvider.AddInParameter(command, helper.prueba, DbType.String, prueba);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);

            return dbProvider.ExecuteNonQuery(command);
        }

        public int TotalConteoTipoXEQ(DateTime Fecha)
        {

            String sql = String.Format(helper.SqlTotalConteoTipoXEQ, Fecha.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);

            return 0;
        }

        public int DiasFaltantes(DateTime Fecha)
        {

            String sql = String.Format(helper.SqlDiasFaltantes, Fecha.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);

            return 0;
        }

        #endregion
    }
}
