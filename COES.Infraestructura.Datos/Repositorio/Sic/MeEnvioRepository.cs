using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;
using COES.Framework.Base.Tools;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla ME_ENVIO
    /// </summary>
    public class MeEnvioRepository : RepositoryBase, IMeEnvioRepository
    {
        private string strConexion;
         MeEnvioHelper helper = new MeEnvioHelper();

        public MeEnvioRepository(string strConn)
            : base(strConn)
        {
            strConexion = strConn;
        }

        public IDbConnection BeginConnection()
        {
            Database db = DatabaseFactory.CreateDatabase(strConexion);
            IDbConnection conn = db.CreateConnection();
            conn.Open();
            return conn;
        }

        public DbTransaction StartTransaction(IDbConnection conn)
        {
            return (DbTransaction)conn.BeginTransaction();
        }

        // ---------------------------------------- 04-04-2019 -----------------------------------------
        // Se recargo la función corrigiendo la logica de generación del correlativo
        // ---------------------------------------------------------------------------------------------
        public int Save(MeEnvioDTO entity, IDbConnection conn, DbTransaction tran, int correlativoEnvio)
        {
            int id = -1;

            #region AUTO GENERA EL CORRELATIVO
            // -------------------------- 04-04-2019--------------------------------------------------
            // Se cajausto la logica de generación del correlativo
            // ---------------------------------------------------------------------------------------
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);

            if (correlativoEnvio != -1)
            {
                id = correlativoEnvio + 1;
            }
            else
            {
                object result = dbProvider.ExecuteScalar(command);
                if (result != null)
                {
                    id = Convert.ToInt32(result);
                }
            }
            // ---------------------------------------------------------------------------------------

            // ---------------------------------- 04-04-2019------------------------------------------
            // Se comento esta lineas porque no estan funcionando bien
            // ---------------------------------------------------------------------------------------
            //DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            //object result = dbProvider.ExecuteScalar(command);

            //if (result != null)
            //{
            //    id = Convert.ToInt32(result);
            //}
            // ---------------------------------------------------------------------------------------
            #endregion

            DbCommand command2 = (DbCommand)conn.CreateCommand();
            command2.CommandText = helper.SqlSave;
            command2.Transaction = tran;
            command2.Connection = (DbConnection)conn;

            IDbDataParameter param = command2.CreateParameter();
            param.ParameterName = helper.Enviocodi;
            param.Value = id;
            command2.Parameters.Add(param);

            param = command2.CreateParameter(); param.ParameterName = helper.Enviofecha; param.Value = entity.Enviofecha; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Estenvcodi; param.Value = entity.Estenvcodi; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Archcodi; param.Value = entity.Archcodi; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Envioplazo; param.Value = entity.Envioplazo; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Userlogin; param.Value = entity.Userlogin; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Lastuser; param.Value = entity.Lastuser; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Lastdate; param.Value = entity.Lastdate; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Emprcodi; param.Value = entity.Emprcodi; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Enviofechaperiodo; param.Value = entity.Enviofechaperiodo; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Enviofechaini; param.Value = entity.Enviofechaini; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Enviofechafin; param.Value = entity.Enviofechafin; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Formatcodi; param.Value = entity.Formatcodi; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Cfgenvcodi; param.Value = entity.Cfgenvcodi; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Modcodi; param.Value = entity.Modcodi; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Fdatcodi; param.Value = entity.Fdatcodi; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Envionumbloques; param.Value = entity.Envionumbloques; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Envioorigen; param.Value = entity.Envioorigen; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Enviofechaplazoini; param.Value = entity.Enviofechaplazoini; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Enviofechaplazofin; param.Value = entity.Enviofechaplazofin; command2.Parameters.Add(param);

            param = command2.CreateParameter(); param.ParameterName = helper.Enviodesc; param.Value = entity.Enviodesc; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Enviobloquehora; param.Value = entity.Enviobloquehora; command2.Parameters.Add(param);

            command2.ExecuteNonQuery();

            return id;
        }

        public int Save(MeEnvioDTO entity, IDbConnection conn, IDbTransaction tran)
        {
            int id = -1;

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            DbCommand command2 = (DbCommand)conn.CreateCommand();

            command2.CommandText = helper.SqlSave;
            command2.Transaction = (DbTransaction)tran;
            command2.Connection = (DbConnection)conn;

            IDbDataParameter param = command2.CreateParameter();
            param.ParameterName = helper.Enviocodi;
            param.Value = id;
            command2.Parameters.Add(param);

            param = command2.CreateParameter(); param.ParameterName = helper.Enviofecha; param.Value = entity.Enviofecha; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Estenvcodi; param.Value = entity.Estenvcodi; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Archcodi; param.Value = entity.Archcodi; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Envioplazo; param.Value = entity.Envioplazo; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Userlogin; param.Value = entity.Userlogin; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Lastuser; param.Value = entity.Lastuser; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Lastdate; param.Value = entity.Lastdate; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Emprcodi; param.Value = entity.Emprcodi; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Enviofechaperiodo; param.Value = entity.Enviofechaperiodo; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Enviofechaini; param.Value = entity.Enviofechaini; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Enviofechafin; param.Value = entity.Enviofechafin; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Formatcodi; param.Value = entity.Formatcodi; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Cfgenvcodi; param.Value = entity.Cfgenvcodi; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Modcodi; param.Value = entity.Modcodi; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Fdatcodi; param.Value = entity.Fdatcodi; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Envionumbloques; param.Value = entity.Envionumbloques; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Envioorigen; param.Value = entity.Envioorigen; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Enviofechaplazoini; param.Value = entity.Enviofechaplazoini; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Enviofechaplazofin; param.Value = entity.Enviofechaplazofin; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Enviodesc; param.Value = entity.Enviodesc; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Enviobloquehora; param.Value = entity.Enviobloquehora; command2.Parameters.Add(param);
            command2.ExecuteNonQuery();

            return id;
        }

        public int Save(MeEnvioDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);
            command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            dbProvider.AddInParameter(command, helper.Enviocodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Enviofecha, DbType.DateTime, entity.Enviofecha);
            dbProvider.AddInParameter(command, helper.Estenvcodi, DbType.Int32, entity.Estenvcodi);
            dbProvider.AddInParameter(command, helper.Archcodi, DbType.Int32, entity.Archcodi);
            dbProvider.AddInParameter(command, helper.Envioplazo, DbType.String, entity.Envioplazo);
            dbProvider.AddInParameter(command, helper.Userlogin, DbType.String, entity.Userlogin);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Enviofechaperiodo, DbType.DateTime, entity.Enviofechaperiodo);
            dbProvider.AddInParameter(command, helper.Enviofechaini, DbType.DateTime, entity.Enviofechaini);
            dbProvider.AddInParameter(command, helper.Enviofechafin, DbType.DateTime, entity.Enviofechafin);
            dbProvider.AddInParameter(command, helper.Formatcodi, DbType.Int32, entity.Formatcodi);
            dbProvider.AddInParameter(command, helper.Cfgenvcodi, DbType.Int32, entity.Cfgenvcodi);
            dbProvider.AddInParameter(command, helper.Modcodi, DbType.Int32, entity.Modcodi);
            dbProvider.AddInParameter(command, helper.Fdatcodi, DbType.Int32, entity.Fdatcodi);
            dbProvider.AddInParameter(command, helper.Envionumbloques, DbType.Int32, entity.Envionumbloques);
            dbProvider.AddInParameter(command, helper.Envioorigen, DbType.Int32, entity.Envioorigen);
            dbProvider.AddInParameter(command, helper.Enviofechaplazoini, DbType.DateTime, entity.Enviofechaplazoini);
            dbProvider.AddInParameter(command, helper.Enviofechaplazofin, DbType.DateTime, entity.Enviofechaplazofin);
            dbProvider.AddInParameter(command, helper.Enviodesc, DbType.String, entity.Enviodesc);
            dbProvider.AddInParameter(command, helper.Enviobloquehora, DbType.Int32, entity.Enviobloquehora);

            dbProvider.ExecuteNonQuery(command);

            return id;
        }

        public void Update(MeEnvioDTO entity)
        {
            string stQuery = string.Format(helper.SqlUpdate, entity.Enviocodi, entity.Estenvcodi, entity.Cfgenvcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(stQuery);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Update1(MeEnvioDTO entity)
        {
            string stQuery = string.Format(helper.SqlUpdate1, entity.Enviocodi, entity.Estenvcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(stQuery);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Update2(MeEnvioDTO entity)
        {
            string stQuery = string.Format(helper.SqlUpdate2, entity.Enviocodi, entity.Estenvcodi, ((DateTime)entity.Lastdate).ToString(ConstantesBase.FormatoFechaExtendido));
            DbCommand command = dbProvider.GetSqlStringCommand(stQuery);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Update3(MeEnvioDTO entity)
        {
            string stQuery = string.Format(helper.SqlUpdate3, entity.Enviocodi, entity.Enviodesc);
            DbCommand command = dbProvider.GetSqlStringCommand(stQuery);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);


            dbProvider.ExecuteNonQuery(command);
        }

        public MeEnvioDTO GetById(int idEnvio)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);
            dbProvider.AddInParameter(command, helper.Enviocodi, DbType.Int32, idEnvio);
            MeEnvioDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iEnviodesc = dr.GetOrdinal(helper.Enviodesc);
                    if (!dr.IsDBNull(iEnviodesc)) entity.Enviodesc = dr.GetString(iEnviodesc);
                }
            }

            return entity;
        }

        public List<MeEnvioDTO> List()
        {
            List<MeEnvioDTO> entitys = new List<MeEnvioDTO>();
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

        public List<MeEnvioDTO> GetByCriteria(int idEmpresa, int idFormato, DateTime fecha)
        {
            string sqlQuery = string.Format(helper.SqlGetByCriteria, idEmpresa, idFormato, fecha.ToString(ConstantesBase.FormatoFecha));
            List<MeEnvioDTO> entitys = new List<MeEnvioDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<MeEnvioDTO> GetByCriteriaRango(int idEmpresa, int idFormato, DateTime fechaIni, DateTime fechaFin)
        {
            string sqlQuery = string.Format(helper.SqlGetByCriteriaRango, idEmpresa, idFormato, fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));
            List<MeEnvioDTO> entitys = new List<MeEnvioDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<MeEnvioDTO> GetListaMultiple(string idsEmpresa, string idsLectura, string idsFormato, string idsEstado, DateTime fechaIni, DateTime fechaFin, int nroPaginas, int pageSize)
        {
            string sqlQuery = string.Format(helper.SqlGetListaMultiple, idsEmpresa, idsLectura, idsFormato, idsEstado,
                fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), nroPaginas, pageSize);
            List<MeEnvioDTO> entitys = new List<MeEnvioDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeEnvioDTO entity = helper.Create(dr);
                    int iFormatnombre = dr.GetOrdinal(helper.Formatnombre);
                    if (!dr.IsDBNull(iFormatnombre)) entity.Formatnombre = dr.GetString(iFormatnombre);
                    int iLectnomb = dr.GetOrdinal(helper.Lectnomb);
                    if (!dr.IsDBNull(iLectnomb)) entity.Lectnomb = dr.GetString(iLectnomb);
                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);
                    int iEmprruc = dr.GetOrdinal(helper.Emprruc);
                    if (!dr.IsDBNull(iEmprruc)) entity.RucEmpresa = dr.GetString(iEmprruc);
                    int iUsername = dr.GetOrdinal(helper.Username);
                    if (!dr.IsDBNull(iUsername)) entity.Username = dr.GetString(iUsername);
                    int iUsertlf = dr.GetOrdinal(helper.Usertlf);
                    if (!dr.IsDBNull(iUsertlf)) entity.Usertlf = dr.GetString(iUsertlf);
                    int iEstenvnombre = dr.GetOrdinal(helper.Estenvnombre);
                    if (!dr.IsDBNull(iEstenvnombre)) entity.Estenvnombre = dr.GetString(iEstenvnombre);
                    int periodo = 0;
                    string fechaPeriodo = string.Empty;
                    int iFormatperiodo = dr.GetOrdinal(helper.Formatperiodo);
                    if (!dr.IsDBNull(iFormatperiodo)) periodo = Convert.ToInt32(dr.GetValue(iFormatperiodo));
                    switch (periodo)
                    {
                        case 1:
                            entity.Periodo = "Diario";
                            entity.FechaPeriodo = ((DateTime)entity.Enviofechaperiodo).ToString("dd/MM/yyyy");
                            break;
                        case 2:
                            entity.Periodo = "Semanal";
                            entity.FechaPeriodo = entity.Enviofechaperiodo.Value.Year.ToString() + " Sem: " + EPDate.f_numerosemana((DateTime)entity.Enviofechaperiodo).ToString();
                            break;
                        case 3:
                            entity.Periodo = "Mensual";
                            entity.FechaPeriodo = entity.Enviofechaperiodo.Value.Year.ToString() + " " + EPDate.f_NombreMesCorto(entity.Enviofechaperiodo.Value.Month).ToString();
                            break;
                        case 4:
                            entity.Periodo = "Anual";
                            entity.FechaPeriodo = entity.Enviofechaperiodo.Value.Year.ToString();
                            break;
                        case 5:
                            entity.Periodo = "Mensual x Semana";
                            entity.FechaPeriodo = entity.Enviofechaperiodo.Value.Year.ToString() + " " + EPDate.f_NombreMesCorto(entity.Enviofechaperiodo.Value.Month).ToString();
                            break;
                        default:
                            entity.Periodo = "No Definido";
                            break;
                    }

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<MeEnvioDTO> GetListaMultipleXLS(string idsEmpresa, string idsLectura, string idsFormato, string idsEstado, DateTime fechaIni, DateTime fechaFin)
        {
            string sqlQuery = string.Format(helper.SqlGetListaMultipleXLS, idsEmpresa, idsLectura, idsFormato, idsEstado,
                fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));
            List<MeEnvioDTO> entitys = new List<MeEnvioDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeEnvioDTO entity = helper.Create(dr);
                    int iFormatnombre = dr.GetOrdinal(helper.Formatnombre);
                    if (!dr.IsDBNull(iFormatnombre)) entity.Formatnombre = dr.GetString(iFormatnombre);
                    int iLectnomb = dr.GetOrdinal(helper.Lectnomb);
                    if (!dr.IsDBNull(iLectnomb)) entity.Lectnomb = dr.GetString(iLectnomb);
                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);
                    int iEmprruc = dr.GetOrdinal(helper.Emprruc);
                    if (!dr.IsDBNull(iEmprruc)) entity.RucEmpresa = dr.GetString(iEmprruc);
                    int iUsername = dr.GetOrdinal(helper.Username);
                    if (!dr.IsDBNull(iUsername)) entity.Username = dr.GetString(iUsername);
                    int iUsertlf = dr.GetOrdinal(helper.Usertlf);
                    if (!dr.IsDBNull(iUsertlf)) entity.Usertlf = dr.GetString(iUsertlf);
                    int iEstenvnombre = dr.GetOrdinal(helper.Estenvnombre);
                    if (!dr.IsDBNull(iEstenvnombre)) entity.Estenvnombre = dr.GetString(iEstenvnombre);
                    int periodo = 0;
                    string fechaPeriodo = string.Empty;
                    int iFormatperiodo = dr.GetOrdinal(helper.Formatperiodo);
                    if (!dr.IsDBNull(iFormatperiodo)) periodo = Convert.ToInt32(dr.GetValue(iFormatperiodo));

                    int iEnviodesc = dr.GetOrdinal(helper.Enviodesc);
                    if (!dr.IsDBNull(iEnviodesc)) entity.Enviodesc = dr.GetString(iEnviodesc);

                    switch (periodo)
                    {
                        case 1:
                            entity.Periodo = "Diario";
                            entity.FechaPeriodo = ((DateTime)entity.Enviofechaperiodo).ToString("dd/MM/yyyy");
                            break;
                        case 2:
                            entity.Periodo = "Semanal";
                            entity.FechaPeriodo = entity.Enviofechaperiodo.Value.Year.ToString() + " Sem: " + EPDate.f_numerosemana((DateTime)entity.Enviofechaperiodo).ToString();
                            break;
                        case 3:
                            entity.Periodo = "Mensual";
                            entity.FechaPeriodo = entity.Enviofechaperiodo.Value.Year.ToString() + " " + EPDate.f_NombreMesCorto(entity.Enviofechaperiodo.Value.Month).ToString();
                            break;
                        case 4:
                            entity.Periodo = "Anual";
                            entity.FechaPeriodo = entity.Enviofechaperiodo.Value.Year.ToString();
                            break;
                        case 5:
                            entity.Periodo = "Mensual x Semana";
                            entity.FechaPeriodo = entity.Enviofechaperiodo.Value.Year.ToString() + " " + EPDate.f_NombreMesCorto(entity.Enviofechaperiodo.Value.Month).ToString();
                            break;
                        default:
                            entity.Periodo = "No Definido";
                            break;
                    }

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public int TotalListaMultiple(string idsEmpresa, string idsLectura, string idsFormato, string idsEstado, DateTime fechaIni, DateTime fechaFin)
        {
            string sqlTotal = string.Format(helper.SqlTotalListaMultiple, idsEmpresa, idsLectura, idsFormato, idsEstado,
               fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sqlTotal);
            object result = dbProvider.ExecuteScalar(command);
            int total = 0;
            if (result != null) total = Convert.ToInt32(result);
            return total;
        }

        public List<MeEnvioDTO> ObtenerReporteEnvioCumplimiento(string empresas, int formatCodi, DateTime fechaInicio, DateTime fechaFin)
        {
            List<MeEnvioDTO> entitys = new List<MeEnvioDTO>();

            string query = String.Format(helper.SqlObtenerReporteEnvioCumplimiento, formatCodi, empresas,
                fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeEnvioDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEstenvnombre = dr.GetOrdinal(helper.Estenvnombre);
                    if (!dr.IsDBNull(iEstenvnombre)) entity.Estenvnombre = dr.GetString(iEstenvnombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int GetMaxIdEnvioFormato(int idFormato, int idEmpresa)
        {
            string sqlQuery = string.Format(helper.SqlGetByMaxEnvioFormato, idFormato, idEmpresa);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            object result = dbProvider.ExecuteScalar(command);
            int id = 0;
            string idS = result.ToString();
            if (!string.IsNullOrEmpty(idS)) id = Convert.ToInt32(result);
            return id;
        }

        public List<MeEnvioDTO> GetByCriteriaRangoFecha(int idEmpresa, int idFormato, DateTime fechaini, DateTime fechafin)
        {
            string sqlQuery = string.Format(helper.SqlGetByCriteriaRangoFecha, idEmpresa, idFormato, fechaini.ToString(ConstantesBase.FormatoFecha),
               fechaini.ToString(ConstantesBase.FormatoFecha));
            List<MeEnvioDTO> entitys = new List<MeEnvioDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<MeEnvioDTO> ObtenerReporteCumplimiento(string empresas, int formatCodi, DateTime fechaInicio, DateTime fechaFin)
        {
            List<MeEnvioDTO> entitys = new List<MeEnvioDTO>();

            string query = String.Format(helper.SqlObtenerReporteCumplimiento, formatCodi,
                fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), empresas);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeEnvioDTO entity = helper.Create(dr);
                    int iFormatperiodo = dr.GetOrdinal(helper.Formatperiodo);
                    if (!dr.IsDBNull(iFormatperiodo)) entity.Formatperiodo = Convert.ToInt32(dr.GetValue(iFormatperiodo));
                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeEnvioDTO> ObtenerReporteCumplimientoXBloqueHorario(string empresas, string formatCodis, DateTime fechaInicio, DateTime fechaFin)
        {
            List<MeEnvioDTO> entitys = new List<MeEnvioDTO>();

            string query = String.Format(helper.SqlObtenerReporteCumplimientoXBloqueHorario, formatCodis,
                fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), empresas);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeEnvioDTO entity = helper.Create(dr);
                    int iFormatperiodo = dr.GetOrdinal(helper.Formatperiodo);
                    if (!dr.IsDBNull(iFormatperiodo)) entity.Formatperiodo = Convert.ToInt32(dr.GetValue(iFormatperiodo));
                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int GetByMaxEnvioFormatoPeriodo(int idFormato, int idEmpresa, DateTime periodo)
        {
            string sqlQuery = string.Format(helper.SqlGetByMaxEnvioFormatoPeriodo, idFormato, idEmpresa, periodo.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            object result = dbProvider.ExecuteScalar(command);
            int id = 0;
            string idS = result.ToString();
            if (!string.IsNullOrEmpty(idS)) id = Convert.ToInt32(result);
            return id;
        }


        public List<MeEnvioDTO> ObtenerListaEnvioActual(int idEmpresa, string periodo)
        {
            string sqlQuery = string.Format(helper.SqlObtenerListaEnvioActual, idEmpresa, periodo);
            List<MeEnvioDTO> entitys = new List<MeEnvioDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeEnvioDTO entity = new MeEnvioDTO();

                    int iEnvioCodiAct = dr.GetOrdinal(helper.EnviocodiAct);
                    if (!dr.IsDBNull(iEnvioCodiAct)) entity.EnvioCodiAct = Convert.ToInt32(dr.GetValue(iEnvioCodiAct));

                    int iEnvioCodiAnt = dr.GetOrdinal(helper.EnviocodiAnt);
                    if (!dr.IsDBNull(iEnvioCodiAnt)) entity.EnvioCodiAnt = Convert.ToInt32(dr.GetValue(iEnvioCodiAnt));

                    int iEnvioFechaPeriodoAct = dr.GetOrdinal(helper.EnviofechaperiodoAct);
                    if (!dr.IsDBNull(iEnvioFechaPeriodoAct)) entity.EnvioFechaPeriodoAct = dr.GetDateTime(iEnvioFechaPeriodoAct);

                    int iEnvioFechaPeriodoAnt = dr.GetOrdinal(helper.EnviofechaperiodoAnt);
                    if (!dr.IsDBNull(iEnvioFechaPeriodoAnt)) entity.EnvioFechaPeriodoAnt = dr.GetDateTime(iEnvioFechaPeriodoAnt);

                    int iEnvioFechaIniAct = dr.GetOrdinal(helper.EnviofechainiAct);
                    if (!dr.IsDBNull(iEnvioFechaIniAct)) entity.EnvioFechaIniAct = dr.GetDateTime(iEnvioFechaIniAct);

                    int iEnvioFechaFinAct = dr.GetOrdinal(helper.EnviofechafinAct);
                    if (!dr.IsDBNull(iEnvioFechaFinAct)) entity.EnvioFechaFinAct = dr.GetDateTime(iEnvioFechaFinAct);

                    int iEnvioFechaIniAnt = dr.GetOrdinal(helper.EnviofechainiAnt);
                    if (!dr.IsDBNull(iEnvioFechaIniAnt)) entity.EnvioFechaIniAnt = dr.GetDateTime(iEnvioFechaIniAnt);

                    int iEnvioFechaFinAnt = dr.GetOrdinal(helper.EnviofechafinAnt);
                    if (!dr.IsDBNull(iEnvioFechaFinAnt)) entity.EnvioFechaFinAnt = dr.GetDateTime(iEnvioFechaFinAnt);

                    entitys.Add(entity);

                }
            }

            return entitys;
        }

        public List<MeEnvioDTO> ObtenerListaPeriodoReporte(string fecha)
        {
            string sqlQuery = string.Format(this.helper.SqlObtenerListaPeriodoReporte, fecha);
            List<MeEnvioDTO> entitys = new List<MeEnvioDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeEnvioDTO entity = new MeEnvioDTO();

                    int iIniRemision = dr.GetOrdinal(helper.IniRemision);
                    if (!dr.IsDBNull(iIniRemision)) entity.IniRemision = dr.GetDateTime(iIniRemision);

                    int iPeriodo = dr.GetOrdinal(helper.Periodo);
                    if (!dr.IsDBNull(iPeriodo)) entity.Periodo = dr.GetString(iPeriodo);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public IDataReader GetListReporteCumplimiento(int formato, string empresas, string tipos, string fechaIni, string fechaFin, string cumpli, string ulcoes, string abreviatura,
            string origen)
        {
            string qEmpresa = "AND 1 = 1";
            string qTipoEmpresa = "AND 1 = 1";
            string qCumplimiento = "";
            string qUlCoes = "";
            string qAbreviatura = "AND 1 = 1";
            if (empresas != "" && empresas != null)
            {
                qEmpresa = " AND empr.EMPRCODI IN (" + empresas + ") ";
            }
            if (tipos != "")
            {
                qTipoEmpresa = " AND empr.TIPOEMPRCODI = " + tipos + " ";
            }
            if (!string.IsNullOrEmpty(abreviatura))
            {
                qAbreviatura = "AND equi.EQUIABREV LIKE '%" + abreviatura + "%'";
            }
            if (cumpli != "")
            {
                qCumplimiento = " where inte.CUMPLIMIENTO IN (" + cumpli + ")";
            }
            if (ulcoes != "0")
            {
                qUlCoes = " AND empr.EMPRESTREGINT = 'A' AND empr.emprcoes = 'S' ";
            }

            var fecInicio = DateTime.ParseExact(fechaIni, "dd/MM/yyyy", null);
            var fecFin = DateTime.ParseExact(fechaFin, "dd/MM/yyyy", null);

            var meses = Math.Abs((fecInicio.Month - fecFin.Month) + 12 * (fecInicio.Year - fecFin.Year));
            var sqlPeriodo = "";

            for (int i = 0; i <= meses; i++)
            {
                sqlPeriodo = sqlPeriodo + string.Format(@",MAX(CASE WHEN PERIODO='{0}' THEN CUMPLIMIENTO END) ""{1}"" ", fecInicio.AddMonths(i).ToString("MM yyyy"),
                    fecInicio.AddMonths(i).ToString("MMyyyy"));
            }

            string sqlQuery = string.Format(this.helper.SqlListReporteCumplimiento, formato, qEmpresa, qTipoEmpresa, fechaIni, fechaFin, qCumplimiento, qUlCoes,
                sqlPeriodo, qAbreviatura);
            //List<MeEnvioDTO> entitys = new List<MeEnvioDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            IDataReader dr = dbProvider.ExecuteReader(command);

            return dr;
            //using (dr)
            //{
            //    while (dr.Read())
            //    {
            //        MeEnvioDTO entity = new MeEnvioDTO();
            //        int iItem = dr.GetOrdinal(helper.Item);
            //        if (!dr.IsDBNull(iItem)) entity.Item = Convert.ToInt32(dr.GetValue(iItem));

            //        int iPeriodo = dr.GetOrdinal(helper.Periodo);
            //        if (!dr.IsDBNull(iPeriodo)) entity.Periodo = dr.GetString(iPeriodo);

            //        int iCumplimiento = dr.GetOrdinal(helper.Cumplimiento);
            //        if (!dr.IsDBNull(iCumplimiento)) entity.Cumplimiento = dr.GetString(iCumplimiento);

            //        int iTipoEmpresa = dr.GetOrdinal(helper.TipoEmpresa);
            //        if (!dr.IsDBNull(iTipoEmpresa)) entity.TipoEmpresa = dr.GetString(iTipoEmpresa);

            //        int iRucEmpresa = dr.GetOrdinal(helper.RucEmpresa);
            //        if (!dr.IsDBNull(iRucEmpresa)) entity.RucEmpresa = dr.GetString(iRucEmpresa);

            //        int iNombreEmpresa = dr.GetOrdinal(helper.NombreEmpresa);
            //        if (!dr.IsDBNull(iNombreEmpresa)) entity.NombreEmpresa = dr.GetString(iNombreEmpresa);

            //        int iNroEnvios = dr.GetOrdinal(helper.NroEnvios);
            //        if (!dr.IsDBNull(iNroEnvios)) entity.NroEnvios = Convert.ToInt32(dr.GetValue(iNroEnvios));

            //        int iFechaPrimerEnvio = dr.GetOrdinal(helper.FechaPrimerEnvio);
            //        if (!dr.IsDBNull(iFechaPrimerEnvio)) entity.FechaPrimerEnvio = dr.GetDateTime(iFechaPrimerEnvio);

            //        int iFechaUltimoEnvio = dr.GetOrdinal(helper.FechaUltimoEnvio);
            //        if (!dr.IsDBNull(iFechaUltimoEnvio)) entity.FechaUltimoEnvio = dr.GetDateTime(iFechaUltimoEnvio);

            //        int iIniRemision = dr.GetOrdinal(helper.IniRemision);
            //        if (!dr.IsDBNull(iIniRemision)) entity.IniRemision = dr.GetDateTime(iIniRemision);

            //        int iFinRemision = dr.GetOrdinal(helper.FinRemision);
            //        if (!dr.IsDBNull(iFinRemision)) entity.FinRemision = dr.GetDateTime(iFinRemision);

            //        int iIniPeriodo = dr.GetOrdinal(helper.IniPeriodo);
            //        if (!dr.IsDBNull(iIniPeriodo)) entity.IniPeriodo = dr.GetDateTime(iIniPeriodo);
            //        entitys.Add(entity);
            //    }
            //}

            //return entitys;
        }

        public List<MeEnvioDTO> ObtenerEnvioXModulo(int modcodi, int emprcodi, DateTime fecha)
        {
            string sqlQuery = string.Format(helper.SqlObtenerEnvioXModulo, modcodi, fecha.ToString(ConstantesBase.FormatoFecha), emprcodi);
            List<MeEnvioDTO> entitys = new List<MeEnvioDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<MeEnvioDTO> GetListaReporteCumplimientoHidrologia(string sAreas, string sEmpresas, DateTime fechaIni, DateTime fechaFin)
        {
            string sqlQuery = string.Format(this.helper.SqlListaReporteCumplimientoHidrologia, sAreas, sEmpresas,
                fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));
            List<MeEnvioDTO> entitys = new List<MeEnvioDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeEnvioDTO entity = new MeEnvioDTO();
                    //area
                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);
                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    //empresa
                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = dr.GetInt32(iAreacodi);
                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    //lectura
                    int iLectcodi = dr.GetOrdinal(helper.Lectcodi);
                    if (!dr.IsDBNull(iLectcodi)) entity.Lectcodi = dr.GetInt32(iLectcodi);
                    int iLectnomb = dr.GetOrdinal(helper.Lectnomb);
                    if (!dr.IsDBNull(iLectnomb)) entity.Lectnomb = dr.GetString(iLectnomb);
                    int iLectnro = dr.GetOrdinal(helper.Lectnro);
                    if (!dr.IsDBNull(iLectnro)) entity.Lectnro = dr.GetInt32(iLectnro);
                    int iLectperiodo = dr.GetOrdinal(helper.Lectperiodo);
                    if (!dr.IsDBNull(iLectperiodo)) entity.Lectperiodo = dr.GetInt32(iLectperiodo);
                    int iLecttipo = dr.GetOrdinal(helper.Lecttipo);
                    if (!dr.IsDBNull(iLecttipo)) entity.Lecttipo = Convert.ToInt32(dr.GetValue(iLecttipo));

                    //envio
                    int iEnviocodi = dr.GetOrdinal(helper.Enviocodi);
                    if (!dr.IsDBNull(iEnviocodi)) entity.Enviocodi = Convert.ToInt32(dr.GetValue(iEnviocodi));

                    int iEnviofecha = dr.GetOrdinal(helper.Enviofecha);
                    if (!dr.IsDBNull(iEnviofecha)) entity.Enviofecha = dr.GetDateTime(iEnviofecha);
                    int iEnvioplazo = dr.GetOrdinal(helper.Envioplazo);
                    if (!dr.IsDBNull(iEnvioplazo)) entity.Envioplazo = dr.GetString(iEnvioplazo);
                    int iEnviofechaperiodo = dr.GetOrdinal(helper.Enviofechaperiodo);
                    if (!dr.IsDBNull(iEnviofechaperiodo)) entity.Enviofechaperiodo = dr.GetDateTime(iEnviofechaperiodo);
                    int iEnviofechaini = dr.GetOrdinal(helper.Enviofechaini);
                    if (!dr.IsDBNull(iEnviofechaini)) entity.Enviofechaini = dr.GetDateTime(iEnviofechaini);
                    int iEnviofechafin = dr.GetOrdinal(helper.Enviofechafin);
                    if (!dr.IsDBNull(iEnviofechafin)) entity.Enviofechafin = dr.GetDateTime(iEnviofechafin);

                    //formato
                    int iFormatcodi = dr.GetOrdinal(helper.Formatcodi);
                    if (!dr.IsDBNull(iFormatcodi)) entity.Formatcodi = Convert.ToInt32(dr.GetValue(iFormatcodi));
                    int iFormatnombre = dr.GetOrdinal(helper.Formatnombre);
                    if (!dr.IsDBNull(iFormatnombre)) entity.Formatnombre = dr.GetString(iFormatnombre);
                    int iFormatperiodo = dr.GetOrdinal(helper.Formatperiodo);
                    if (!dr.IsDBNull(iFormatperiodo)) entity.Formatperiodo = Convert.ToInt32(dr.GetValue(iFormatperiodo));

                    int iFormatresolucion = dr.GetOrdinal(helper.Formatresolucion);
                    if (!dr.IsDBNull(iFormatresolucion)) entity.Formatresolucion = Convert.ToInt32(dr.GetValue(iFormatresolucion));
                    int iFormathorizonte = dr.GetOrdinal(helper.Formathorizonte);
                    if (!dr.IsDBNull(iFormathorizonte)) entity.Formathorizonte = Convert.ToInt32(dr.GetValue(iFormathorizonte));
                    int iFormatdiaplazo = dr.GetOrdinal(helper.Formatdiaplazo);
                    if (!dr.IsDBNull(iFormatdiaplazo)) entity.Formatdiaplazo = Convert.ToInt32(dr.GetValue(iFormatdiaplazo));
                    int iFormatminplazo = dr.GetOrdinal(helper.Formatminplazo);
                    if (!dr.IsDBNull(iFormatminplazo)) entity.Formatminplazo = Convert.ToInt32(dr.GetValue(iFormatminplazo));

                    //equipo
                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = dr.GetInt32(iEquicodi);
                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    //Punto de medicion
                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = dr.GetInt32(iPtomedicodi);
                    int iTipoinfocodi = dr.GetOrdinal(helper.Tipoinfocodi);
                    if (!dr.IsDBNull(iTipoinfocodi)) entity.Tipoinfocodi = dr.GetInt32(iTipoinfocodi);
                    int iTipoInfoabrev = dr.GetOrdinal(helper.TipoInfoabrev);
                    if (!dr.IsDBNull(iTipoInfoabrev)) entity.TipoInfoabrev = dr.GetString(iTipoInfoabrev);
                    int iPtomedibarranomb = dr.GetOrdinal(helper.Ptomedibarranomb);
                    if (!dr.IsDBNull(iPtomedibarranomb)) entity.Ptomedibarranomb = dr.GetString(iPtomedibarranomb);
                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    //Inicio  Plazo
                    int iFormatmesplazo = dr.GetOrdinal(helper.Formatmesplazo);
                    if (!dr.IsDBNull(iFormatmesplazo)) entity.Formatmesplazo = Convert.ToInt32(dr.GetValue(iFormatmesplazo));
                    //Fin plazo
                    int iFormatmesfinplazo = dr.GetOrdinal(helper.Formatmesfinplazo);
                    if (!dr.IsDBNull(iFormatmesfinplazo)) entity.Formatmesfinplazo = Convert.ToInt32(dr.GetValue(iFormatmesfinplazo));

                    int iFormatdiafinplazo = dr.GetOrdinal(helper.Formatdiafinplazo);
                    if (!dr.IsDBNull(iFormatdiafinplazo)) entity.Formatdiafinplazo = Convert.ToInt32(dr.GetValue(iFormatdiafinplazo));

                    int iFormatminfinplazo = dr.GetOrdinal(helper.Formatminfinplazo);
                    if (!dr.IsDBNull(iFormatminfinplazo)) entity.Formatminfinplazo = Convert.ToInt32(dr.GetValue(iFormatminfinplazo));
                    // Fuera plazo

                    int iFormatmesfinfueraplazo = dr.GetOrdinal(helper.Formatmesfinfueraplazo);
                    if (!dr.IsDBNull(iFormatmesfinfueraplazo)) entity.Formatmesfinfueraplazo = Convert.ToInt32(dr.GetValue(iFormatmesfinfueraplazo));

                    int iFormatdiafinfueraplazo = dr.GetOrdinal(helper.Formatdiafinfueraplazo);
                    if (!dr.IsDBNull(iFormatdiafinfueraplazo)) entity.Formatdiafinfueraplazo = Convert.ToInt32(dr.GetValue(iFormatdiafinfueraplazo));

                    int iFormatminfinfueraplazo = dr.GetOrdinal(helper.Formatminfinfueraplazo);
                    if (!dr.IsDBNull(iFormatminfinfueraplazo)) entity.Formatminfinfueraplazo = Convert.ToInt32(dr.GetValue(iFormatminfinfueraplazo));

                    //FomatoCheckPlazoPunto
                    int iFormatcheckplazopunto = dr.GetOrdinal(helper.Formatcheckplazopunto);
                    if (!dr.IsDBNull(iFormatcheckplazopunto)) entity.Formatcheckplazopunto = Convert.ToInt32(dr.GetValue(iFormatcheckplazopunto));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeEnvioDTO> ListaMeEnvioByFdat(int fdatcodi, DateTime fecha)
        {
            string sqlQuery = string.Format(helper.SqlListaMeEnvioByFdat, fdatcodi, fecha.ToString(ConstantesBase.FormatoFecha));
            List<MeEnvioDTO> entitys = new List<MeEnvioDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iEnviodesc = dr.GetOrdinal(helper.Enviodesc);
                    if (!dr.IsDBNull(iEnviodesc)) entity.Enviodesc = dr.GetString(iEnviodesc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #region ASSETEC - CAMBIOS 24/07/2018
        public void EliminarFisico(int idEnvio, IDbConnection conn, DbTransaction tran)
        {

            DbCommand commandEliminarEnvio = (DbCommand)conn.CreateCommand();
            commandEliminarEnvio.CommandText = helper.SqlEliminarEnvioFisicoPorId;
            commandEliminarEnvio.Transaction = tran;
            commandEliminarEnvio.Connection = (DbConnection)conn;

            commandEliminarEnvio.Parameters.Add(dbProvider.CreateParameter(commandEliminarEnvio, helper.Enviocodi, DbType.Int32, idEnvio));

            commandEliminarEnvio.ExecuteNonQuery();

        }
        #endregion

        #region Aplicativo Extranet CTAF

        public List<MeEnvioDTO> ObtenerEnvioInterrupSuministro(int emprcodi, int afecodi, int fdatcodi)
        {
            List<MeEnvioDTO> entitys = new List<MeEnvioDTO>();

            string query = string.Format(helper.SqlObtenerEnvioInterrupSuministro, afecodi, emprcodi, fdatcodi);
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

        #endregion

        #region Mejoras RDO
        public List<MeEnvioDTO> GetByCriteriaCaudalVolumen(int idFormato, DateTime fecha)
        {
            string sqlQuery = string.Format(helper.GetByCriteriaCaudalVolumen, idFormato, fecha.ToString(ConstantesBase.FormatoFecha));
            List<MeEnvioDTO> entitys = new List<MeEnvioDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public void SaveHorario(int idEnvio, int horario)
        {
            string stQuery = string.Format(helper.SqlSaveHorario, idEnvio, horario);
            DbCommand command = dbProvider.GetSqlStringCommand(stQuery);
            dbProvider.ExecuteNonQuery(command);
        }
        public List<MeEnvioDTO> GetByCriteriaxHorario(int idEmpresa, int idFormato, DateTime fecha, int horario)
        {
            string sqlQuery = string.Format(helper.SqlGetByCriteriaxHorario, idEmpresa, idFormato, fecha.ToString(ConstantesBase.FormatoFecha), horario);
            List<MeEnvioDTO> entitys = new List<MeEnvioDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);
                    //entitys.Add(helper.Create(dr));
                    int iHorarioCodi = dr.GetOrdinal(helper.HorarioCodi);
                    if (!dr.IsDBNull(iHorarioCodi)) entity.HorarioCodi = dr.GetInt32(iHorarioCodi);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion
        #region Mejoras RDO-II
        //public List<MeEnvioDTO> GetByCriteriaMeEnviosUltimoEjecutado(int idEmpresa, int idFormato, DateTime fecha,int horario)
        //{
        //    string sqlQuery = string.Format(helper.SqlGetByCriteriaMeEnviosUltimoEjecutado, idEmpresa, idFormato, fecha.ToString(ConstantesBase.FormatoFecha), horario);
        //    List<MeEnvioDTO> entitys = new List<MeEnvioDTO>();
        //    DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

        //    using (IDataReader dr = dbProvider.ExecuteReader(command))
        //    {
        //        while (dr.Read())
        //        {
        //            var entity = helper.Create(dr);
        //            //entitys.Add(helper.Create(dr));
        //            int iHorarioCodi = dr.GetOrdinal(helper.HorarioCodi);
        //            if (!dr.IsDBNull(iHorarioCodi)) entity.HorarioCodi = dr.GetInt32(iHorarioCodi);
        //            entitys.Add(entity);
        //        }
        //    }

        //    return entitys;
        //}
        #endregion

        #region Mejora CTAF
        public List<MeEnvioDTO> ListaEnviosPorEvento(MeEnvioDTO entity)
        {
            string fallaN1= entity.TipoFalla == "1" ? "S": "N";
            string fallaN2= entity.TipoFalla == "2" ? "S": "N";
            string sqlQuery = string.Format(helper.SqlListaEnviosPorEvento, entity.Periodo, entity.EtapaInforme, fallaN1, fallaN2);
            List<MeEnvioDTO> entitys = new List<MeEnvioDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeEnvioDTO entityMeEnvio = new MeEnvioDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entityMeEnvio.Emprcodi = dr.GetInt32(iEmprcodi);
                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entityMeEnvio.Emprnomb = dr.GetString(iEmprnomb);
                    int iEvenfecha = dr.GetOrdinal(helper.Evenini);
                    if (!dr.IsDBNull(iEvenfecha)) entityMeEnvio.Evenini = dr.GetDateTime(iEvenfecha);
                    int iEvenasunto = dr.GetOrdinal(helper.Evenasunto);
                    if (!dr.IsDBNull(iEvenasunto)) entityMeEnvio.Evenasunto = dr.GetString(iEvenasunto);
                    int iEnviocodi = dr.GetOrdinal(helper.Enviocodi);
                    if (!dr.IsDBNull(iEnviocodi)) entityMeEnvio.Enviocodi = dr.GetInt32(iEnviocodi);
                    int iEvencodi = dr.GetOrdinal(helper.Evencodi);
                    if (!dr.IsDBNull(iEvencodi)) entityMeEnvio.Evencodi = dr.GetInt32(iEvencodi);
                    int iEnvioplazo = dr.GetOrdinal(helper.Envioplazo);
                    if (!dr.IsDBNull(iEnvioplazo)) entityMeEnvio.Envioplazo = dr.GetString(iEnvioplazo);
                    entitys.Add(entityMeEnvio);
                }
            }
       
            return entitys;
        }

        public List<MeEnvioDTO> ListaInformeEnvios(int idEvento)
        {
            string sqlQuery = string.Format(helper.SqlListaInformeEnvios, idEvento);
            List<MeEnvioDTO> entitys = new List<MeEnvioDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeEnvioDTO entityMeEnvio = new MeEnvioDTO();

                    int iEnviofecha = dr.GetOrdinal(helper.Enviofecha);
                    if (!dr.IsDBNull(iEnviofecha)) entityMeEnvio.Enviofecha = dr.GetDateTime(iEnviofecha);
                    int iEnvevencodi = dr.GetOrdinal(helper.Envevencodi);
                    if (!dr.IsDBNull(iEnvevencodi)) entityMeEnvio.Envevencodi = dr.GetInt32(iEnvevencodi);
                    int iEnviousuario = dr.GetOrdinal(helper.Userlogin);
                    if (!dr.IsDBNull(iEnviousuario)) entityMeEnvio.Userlogin = dr.GetString(iEnviousuario);
                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entityMeEnvio.Emprnomb = dr.GetString(iEmprnomb);
                    int iTipoInforme = dr.GetOrdinal(helper.TipoInforme);
                    if (!dr.IsDBNull(iTipoInforme)) entityMeEnvio.TipoInforme = dr.GetString(iTipoInforme);
                    int iEveinfrutaarchivo = dr.GetOrdinal(helper.Eveinfrutaarchivo);
                    if (!dr.IsDBNull(iEveinfrutaarchivo)) entityMeEnvio.Eveinfrutaarchivo = dr.GetString(iEveinfrutaarchivo);
                    entitys.Add(entityMeEnvio);
                }
            }
            return entitys;
        }


        public List<MeEnvioDTO> ListaInformeEnviosLog(MeEnvioDTO entity)
        {
            string fallaN1 = entity.TipoFalla == "1" ? "S" : "N";
            string fallaN2 = entity.TipoFalla == "2" ? "S" : "N";
            string sqlQuery = string.Format(helper.SqlListaInformeEnviosLog, entity.FechaIni,entity.FechaFin, fallaN1, fallaN2);
            List<MeEnvioDTO> entitys = new List<MeEnvioDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeEnvioDTO entityMeEnvio = new MeEnvioDTO();
                    int iEnvevencodi = dr.GetOrdinal(helper.Envevencodi);
                    if (!dr.IsDBNull(iEnvevencodi)) entityMeEnvio.Envevencodi = dr.GetInt32(iEnvevencodi);
                    int iEvenini = dr.GetOrdinal(helper.Evenini);
                    if (!dr.IsDBNull(iEvenini)) entityMeEnvio.Evenini = dr.GetDateTime(iEvenini);
                    int iEvenasunto = dr.GetOrdinal(helper.Evenasunto);
                    if (!dr.IsDBNull(iEvenasunto)) entityMeEnvio.Evenasunto = dr.GetString(iEvenasunto);
                    int iEnviofecha = dr.GetOrdinal(helper.Enviofecha);
                    if (!dr.IsDBNull(iEnviofecha)) entityMeEnvio.Enviofecha = dr.GetDateTime(iEnviofecha);                 
                    int iEnviousuario = dr.GetOrdinal(helper.Userlogin);
                    if (!dr.IsDBNull(iEnviousuario)) entityMeEnvio.Userlogin = dr.GetString(iEnviousuario);
                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entityMeEnvio.Emprnomb = dr.GetString(iEmprnomb);
                    int iTipoInforme = dr.GetOrdinal(helper.TipoInforme);
                    if (!dr.IsDBNull(iTipoInforme)) entityMeEnvio.TipoInforme = dr.GetString(iTipoInforme);
                    int iEveinfrutaarchivo = dr.GetOrdinal(helper.Eveinfrutaarchivo);
                    if (!dr.IsDBNull(iEveinfrutaarchivo)) entityMeEnvio.Eveinfrutaarchivo = dr.GetString(iEveinfrutaarchivo);
                    entitys.Add(entityMeEnvio);
                }
            }
            return entitys;
        }
        #endregion
    }
}
