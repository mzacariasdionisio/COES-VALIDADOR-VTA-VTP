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
    public class IeodCuadroRepository : RepositoryBase
    {
        IeodCuadroHelper helper = new IeodCuadroHelper();

        public IeodCuadroRepository(string strConn)
            : base(strConn)
        {
        }

        public void GrabarDatos(List<IeodCuadroDTO> entitys, DateTime fecha)
        {


            if (entitys.Count > 0)
            {
                string query = String.Format(helper.SqlDelete, fecha.ToString(ConstantesBase.FormatoFecha));
                DbCommand command = dbProvider.GetSqlStringCommand(query);
                dbProvider.ExecuteNonQuery(command);

                command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
                object result = dbProvider.ExecuteScalar(command);
                int id = 1;

                if (result != null)
                    id = Convert.ToInt32(result);

                foreach (IeodCuadroDTO entity in entitys)
                {
                    command.Parameters.Clear();
                    command = dbProvider.GetSqlStringCommand(helper.SqlSave);

                    dbProvider.AddInParameter(command, helper.ICCODI, DbType.Int32, id);
                    dbProvider.AddInParameter(command, helper.EQUICODI, DbType.Int32, entity.EQUICODI);
                    dbProvider.AddInParameter(command, helper.SUBCAUSACODI, DbType.Int32, entity.SUBCAUSACODI);
                    dbProvider.AddInParameter(command, helper.ICHORINI, DbType.DateTime, entity.ICHORINI);
                    dbProvider.AddInParameter(command, helper.ICHORFIN, DbType.DateTime, entity.ICHORFIN);
                    dbProvider.AddInParameter(command, helper.ICCHECK1, DbType.String, entity.ICCHECK1);
                    dbProvider.AddInParameter(command, helper.ICVALOR1, DbType.Decimal, entity.ICVALOR1);
                    dbProvider.AddInParameter(command, helper.LASTUSER, DbType.String, entity.LASTUSER);
                    dbProvider.AddInParameter(command, helper.LASTDATE, DbType.DateTime, entity.LASTDATE);
                    dbProvider.AddInParameter(command, helper.EVENCLASECODI, DbType.Int32, entity.EVENCLASECODI);
                    dbProvider.AddInParameter(command, helper.ICCHECK2, DbType.String, entity.ICCHECK2);

                    dbProvider.ExecuteNonQuery(command);

                    id++;
                }

                command.Parameters.Clear();
                command = dbProvider.GetSqlStringCommand(helper.SqlUpdateCounter);
                dbProvider.AddInParameter(command, helper.MAXCOUNT, DbType.Int32, id);

                dbProvider.ExecuteNonQuery(command);

            }

        }

        public int Save(IeodCuadroDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;

            if (result != null)
                id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            dbProvider.AddInParameter(command, helper.ICCODI, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.EQUICODI, DbType.Int32, entity.EQUICODI);
            dbProvider.AddInParameter(command, helper.SUBCAUSACODI, DbType.Int32, entity.SUBCAUSACODI);
            dbProvider.AddInParameter(command, helper.ICHORINI, DbType.DateTime, entity.ICHORINI);
            dbProvider.AddInParameter(command, helper.ICHORFIN, DbType.DateTime, entity.ICHORFIN);
            dbProvider.AddInParameter(command, helper.ICDESCRIP1, DbType.String, entity.ICDESCRIP1);
            dbProvider.AddInParameter(command, helper.ICCHECK1, DbType.String, entity.ICCHECK1);
            dbProvider.AddInParameter(command, helper.ICVALOR1, DbType.Decimal, entity.ICVALOR1);
            dbProvider.AddInParameter(command, helper.LASTUSER, DbType.String, entity.LASTUSER);
            dbProvider.AddInParameter(command, helper.LASTDATE, DbType.DateTime, entity.LASTDATE);
            dbProvider.AddInParameter(command, helper.EVENCLASECODI, DbType.Int32, entity.EVENCLASECODI);
            dbProvider.AddInParameter(command, helper.ICCHECK2, DbType.String, entity.ICCHECK2);
            dbProvider.AddInParameter(command, helper.Icnombarchenvio, DbType.String, entity.Icnombarchenvio);
            dbProvider.AddInParameter(command, helper.Icnombarchfisico, DbType.String, entity.Icnombarchfisico);
            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public List<IeodCuadroDTO> GetByCriteria(DateTime fechaInicio, DateTime fechaFin)
        {
            List<IeodCuadroDTO> entitys = new List<IeodCuadroDTO>();

            string query = String.Format(helper.SqlGetByCriteria,
                fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    IeodCuadroDTO entity = this.helper.Create(dr);


                    int iEmprNomb = dr.GetOrdinal(helper.EMPRNOMB);
                    if (!dr.IsDBNull(iEmprNomb)) entity.EMPRENOMB = dr.GetString(iEmprNomb);

                    int iAreaNomb = dr.GetOrdinal(helper.AREANOMB);
                    if (!dr.IsDBNull(iAreaNomb)) entity.AREANOMB = dr.GetString(iAreaNomb);

                    int iFamAbrev = dr.GetOrdinal(helper.FAMABREV);
                    if (!dr.IsDBNull(iFamAbrev)) entity.FAMABREV = dr.GetString(iFamAbrev);

                    int iEquiAbrev = dr.GetOrdinal(helper.EQUIABREV);
                    if (!dr.IsDBNull(iEquiAbrev)) entity.EQUIABREV = dr.GetString(iEquiAbrev);

                    int iTareaAbrev = dr.GetOrdinal(helper.TAREAABREV);
                    if (!dr.IsDBNull(iTareaAbrev)) entity.TAREAABREV = dr.GetString(iTareaAbrev);

                    int iEmprCodi = dr.GetOrdinal(helper.EMPRCODI);
                    if (!dr.IsDBNull(iEmprCodi)) entity.EMPRCODI = Convert.ToInt16(dr.GetValue(iEmprCodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<IeodCuadroDTO> ObtenerReporte(DateTime fechaInicio, DateTime fechaFin)
        {
            List<IeodCuadroDTO> entitys = new List<IeodCuadroDTO>();

            string query = String.Format(helper.SqlObtenerReporte,
                fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {

                    IeodCuadroDTO entity = new IeodCuadroDTO();

                    int iRUS = dr.GetOrdinal(this.helper.RUS);
                    int iSUBCAUSACODI = dr.GetOrdinal(this.helper.SUBCAUSACODI);
                    int iICHORINI = dr.GetOrdinal(this.helper.ICHORINI);
                    int iICHORFIN = dr.GetOrdinal(this.helper.ICHORFIN);
                    int iICVALOR1 = dr.GetOrdinal(this.helper.ICVALOR1);
                    int iHORA = dr.GetOrdinal(this.helper.HORA);

                    if (!dr.IsDBNull(iRUS)) entity.RUS = dr.GetString(iRUS);
                    if (!dr.IsDBNull(iSUBCAUSACODI)) entity.SUBCAUSACODI = Convert.ToInt32(dr.GetValue(iSUBCAUSACODI));
                    if (!dr.IsDBNull(iICHORINI)) entity.ICHORINI = dr.GetDateTime(iICHORINI);
                    if (!dr.IsDBNull(iICHORFIN)) entity.ICHORFIN = dr.GetDateTime(iICHORFIN);
                    if (!dr.IsDBNull(iICVALOR1)) entity.ICVALOR1 = dr.GetDecimal(iICVALOR1);
                    if (!dr.IsDBNull(iHORA)) entity.HORA = dr.GetString(iHORA);

                    if (entity.SUBCAUSACODI == 318)
                    {
                        entity.TIPO = "MANUAL";
                    }

                    if (entity.SUBCAUSACODI == 319)
                    {
                        entity.TIPO = "AUTOMÁTICO";
                    }

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<IeodCuadroDTO> GetConfiguracionEmpresa()
        {
            List<IeodCuadroDTO> entitys = new List<IeodCuadroDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlConfiguracionEquipo);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    IeodCuadroDTO entity = new IeodCuadroDTO();

                    int iEmprNomb = dr.GetOrdinal(helper.EMPRNOMB);
                    if (!dr.IsDBNull(iEmprNomb)) entity.EMPRENOMB = dr.GetString(iEmprNomb);

                    int iAreaNomb = dr.GetOrdinal(helper.AREANOMB);
                    if (!dr.IsDBNull(iAreaNomb)) entity.AREANOMB = dr.GetString(iAreaNomb);

                    int iFamAbrev = dr.GetOrdinal(helper.FAMABREV);
                    if (!dr.IsDBNull(iFamAbrev)) entity.FAMABREV = dr.GetString(iFamAbrev);

                    int iEquiCodi = dr.GetOrdinal(helper.EQUICODI);
                    if (!dr.IsDBNull(iEquiCodi)) entity.EQUICODI = Convert.ToInt32(dr.GetValue(iEquiCodi));

                    int iEquiAbrev = dr.GetOrdinal(helper.EQUIABREV);
                    if (!dr.IsDBNull(iEquiAbrev)) entity.EQUIABREV = dr.GetString(iEquiAbrev);

                    int iTareaAbrev = dr.GetOrdinal(helper.TAREAABREV);
                    if (!dr.IsDBNull(iTareaAbrev)) entity.TAREAABREV = dr.GetString(iTareaAbrev);

                    int iEmprCodi = dr.GetOrdinal(helper.EMPRCODI);
                    if (!dr.IsDBNull(iEmprCodi)) entity.EMPRCODI = Convert.ToInt16(dr.GetValue(iEmprCodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<IeodCuadroDTO> ValidarExistenciaRegistro(int equiCodi, DateTime horaInicio, DateTime horaFin)
        {
            List<IeodCuadroDTO> entitys = new List<IeodCuadroDTO>();

            string query = String.Format(helper.SqlValidarExistencia, equiCodi, horaInicio.ToString(ConstantesBase.FormatoFechaExtendido),
                horaFin.ToString(ConstantesBase.FormatoFechaExtendido));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    IeodCuadroDTO entity = new IeodCuadroDTO();

                    int iEquiCodi = dr.GetOrdinal(helper.EQUICODI);
                    if (!dr.IsDBNull(iEquiCodi)) entity.EQUICODI = Convert.ToInt32(dr.GetValue(iEquiCodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int Get_fallaacumuladaSEIN(DateTime dtFecha, string ls_dato)
        {
            int li_count = 0;
            string ls_periodo = dtFecha.ToString("yyyyMMdd HH:mm:ss").Substring(0, 6);
            string strCommand = string.Format(helper.SqlGetFallaAcumuladaSein, ls_dato, ls_periodo);
            DbCommand command = dbProvider.GetSqlStringCommand(strCommand);
            var entitys = new DataTable();
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                entitys.Load(dr);
            }
            if (entitys.Rows.Count > 0) //Si existe, seguimos
            {
                DataRow drow = entitys.Rows[0];
                string ls_evaluar = Convert.ToString(drow[ls_dato]);
                if (ls_evaluar == "")
                    li_count = 0;
                else
                    li_count = Convert.ToInt16(drow[ls_dato]);
            }
            return li_count;
        }


        public List<IeodCuadroDTO> ListarIeodCuadroxEmpresa(DateTime fechaIni, DateTime fechaFin, int subcausaCodi, int emprcodi)
        {
            List<IeodCuadroDTO> entitys = new List<IeodCuadroDTO>();

            string query = String.Format(helper.SqlListarIeodCuadroxEmpresa,
                fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), subcausaCodi, emprcodi);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    IeodCuadroDTO entity = this.helper.Create(dr);


                    int iEmprNomb = dr.GetOrdinal(helper.EMPRNOMB);
                    if (!dr.IsDBNull(iEmprNomb)) entity.EMPRENOMB = dr.GetString(iEmprNomb);

                    int iAreaNomb = dr.GetOrdinal(helper.AREANOMB);
                    if (!dr.IsDBNull(iAreaNomb)) entity.AREANOMB = dr.GetString(iAreaNomb);

                    int iFamAbrev = dr.GetOrdinal(helper.FAMABREV);
                    if (!dr.IsDBNull(iFamAbrev)) entity.FAMABREV = dr.GetString(iFamAbrev);

                    int iEquiAbrev = dr.GetOrdinal(helper.EQUIABREV);
                    if (!dr.IsDBNull(iEquiAbrev)) entity.EQUIABREV = dr.GetString(iEquiAbrev);

                    int iEquinomb = dr.GetOrdinal(helper.EQUINOMB);
                    if (!dr.IsDBNull(iEquinomb)) entity.EQUINOMB = dr.GetString(iEquinomb);

                    int iTareaAbrev = dr.GetOrdinal(helper.TAREAABREV);
                    if (!dr.IsDBNull(iTareaAbrev)) entity.TAREAABREV = dr.GetString(iTareaAbrev);

                    int iEmprCodi = dr.GetOrdinal(helper.EMPRCODI);
                    if (!dr.IsDBNull(iEmprCodi)) entity.EMPRCODI = Convert.ToInt16(dr.GetValue(iEmprCodi));

                    int iSubcausadesc = dr.GetOrdinal(helper.SUBCAUSADESC);
                    if (!dr.IsDBNull(iSubcausadesc)) entity.SUBCAUSADESC = dr.GetString(iSubcausadesc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public IeodCuadroDTO GetById(int iccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.ICCODI, DbType.Int32, iccodi);
            IeodCuadroDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }
            return entity;
        }

        public void Delete(int iccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete2);

            dbProvider.AddInParameter(command, helper.ICCODI, DbType.Int32, iccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(IeodCuadroDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.EQUICODI, DbType.Int32, entity.EQUICODI);
            dbProvider.AddInParameter(command, helper.SUBCAUSACODI, DbType.Int32, entity.SUBCAUSACODI);
            dbProvider.AddInParameter(command, helper.ICHORINI, DbType.DateTime, entity.ICHORINI);
            dbProvider.AddInParameter(command, helper.ICHORFIN, DbType.DateTime, entity.ICHORFIN);
            dbProvider.AddInParameter(command, helper.ICDESCRIP1, DbType.String, entity.ICDESCRIP1);
            dbProvider.AddInParameter(command, helper.ICCHECK1, DbType.String, entity.ICCHECK1);
            dbProvider.AddInParameter(command, helper.ICVALOR1, DbType.Int32, entity.ICVALOR1);
            dbProvider.AddInParameter(command, helper.LASTUSER, DbType.String, entity.LASTUSER);
            dbProvider.AddInParameter(command, helper.LASTDATE, DbType.DateTime, entity.LASTDATE);
            dbProvider.AddInParameter(command, helper.EVENCLASECODI, DbType.Int32, entity.EVENCLASECODI);
            dbProvider.AddInParameter(command, helper.ICCHECK2, DbType.String, entity.ICCHECK2);
            dbProvider.AddInParameter(command, helper.Icnombarchenvio, DbType.String, entity.Icnombarchenvio);
            dbProvider.AddInParameter(command, helper.Icnombarchfisico, DbType.String, entity.Icnombarchfisico);
            dbProvider.AddInParameter(command, helper.ICCODI, DbType.Int32, entity.ICCODI);
            dbProvider.ExecuteNonQuery(command);
        }

        public List<IeodCuadroDTO> GetCriteriaxPKCodis(string pkCodis)
        {
            List<IeodCuadroDTO> entitys = new List<IeodCuadroDTO>();

            string query = string.Format(helper.SqlGetCriteriaxPKCodis, pkCodis);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    IeodCuadroDTO entity = this.helper.Create(dr);

                    int iEmprNomb = dr.GetOrdinal(helper.EMPRNOMB);
                    if (!dr.IsDBNull(iEmprNomb)) entity.EMPRENOMB = dr.GetString(iEmprNomb);

                    int iAreaNomb = dr.GetOrdinal(helper.AREANOMB);
                    if (!dr.IsDBNull(iAreaNomb)) entity.AREANOMB = dr.GetString(iAreaNomb);

                    int iFamAbrev = dr.GetOrdinal(helper.FAMABREV);
                    if (!dr.IsDBNull(iFamAbrev)) entity.FAMABREV = dr.GetString(iFamAbrev);

                    int iEquiAbrev = dr.GetOrdinal(helper.EQUIABREV);
                    if (!dr.IsDBNull(iEquiAbrev)) entity.EQUIABREV = dr.GetString(iEquiAbrev);

                    int iEquinomb = dr.GetOrdinal(helper.EQUINOMB);
                    if (!dr.IsDBNull(iEquinomb)) entity.EQUINOMB = dr.GetString(iEquinomb);

                    int iTareaAbrev = dr.GetOrdinal(helper.TAREAABREV);
                    if (!dr.IsDBNull(iTareaAbrev)) entity.TAREAABREV = dr.GetString(iTareaAbrev);

                    int iEmprCodi = dr.GetOrdinal(helper.EMPRCODI);
                    if (!dr.IsDBNull(iEmprCodi)) entity.EMPRCODI = Convert.ToInt16(dr.GetValue(iEmprCodi));

                    int iSubcausadesc = dr.GetOrdinal(helper.SUBCAUSADESC);
                    if (!dr.IsDBNull(iSubcausadesc)) entity.SUBCAUSADESC = dr.GetString(iSubcausadesc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void BorradoLogico(int iccodi)
        {
            string stQuery = string.Format(helper.SqlBorradoLogico, iccodi);
            DbCommand command = dbProvider.GetSqlStringCommand(stQuery);
            dbProvider.ExecuteNonQuery(command);
        }
    }
}
