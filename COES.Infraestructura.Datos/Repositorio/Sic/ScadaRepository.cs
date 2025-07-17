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
    public class ScadaRepository: RepositoryBase
    {
        ScadaHelper helper = new ScadaHelper();

        public ScadaRepository(string strConn)
            : base(strConn)
        {

        }

        public List<ScadaDTO> ObtenerConsultaScada(string filtro, DateTime fechaInicio, DateTime fechaFin, string fuente)
        {
            List<ScadaDTO> entitys = new List<ScadaDTO>();

            String query = String.Format(helper.SqlGetFromScada, filtro,
                fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));
            
            DbCommand command = dbProvider.GetSqlStringCommand(query);           

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ScadaDTO entity = helper.Create(dr);
                    entity.FUENTE = fuente;
                    entitys.Add(entity);
                }
            }

            return entitys;            
        }

        public List<ScadaDTO> ObtenerConsultaScadaSP7(string filtro, DateTime fechaInicio, DateTime fechaFin, string fuente)
        {
            List<ScadaDTO> entitys = new List<ScadaDTO>();

            String query = String.Format(helper.SqlGetFromScadaSP7, filtro,
                fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ScadaDTO entity = helper.Create(dr);
                    entity.FUENTE = fuente;
                    entitys.Add(entity);
                }
            }

            return entitys;
        }



        public List<ScadaDTO> ObtenerConsultaMedicion(string filtro, DateTime fechaInicio, DateTime fechaFin, 
            int lectcodi, int tipoinfocodi, string fuente)
        {
            List<ScadaDTO> entitys = new List<ScadaDTO>();

            String query = String.Format(helper.SqlGetFromMedicion, filtro,
                fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), 
                lectcodi, tipoinfocodi);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ScadaDTO entity = helper.Create(dr);
                    entity.FUENTE = fuente;
                    entitys.Add(entity);
                }
            }

            return entitys;  
        }

        public List<ScadaDTO> ObtenerConsultaMedDistribucion(string filtro, DateTime fechaInicio, DateTime fechaFin, string fuente)
        {
            try
            {
                List<ScadaDTO> entitys = new List<ScadaDTO>();

                String query = String.Format(helper.SqlGetFromMedicionMedDistribucion, filtro,
                    fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));

                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        ScadaDTO entity = helper.Create(dr);
                        entity.FUENTE = fuente;
                        entitys.Add(entity);
                    }
                }

                return entitys;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public List<ScadaDTO> ObtenerConsultaDemandaULyD(string filtro, DateTime fechaInicio, DateTime fechaFin, string fuente)
        {
            try
            {
                List<ScadaDTO> entitys = new List<ScadaDTO>();

                String query = String.Format(helper.SqlGetFromDemandaULyD, filtro,
                    fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));

                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        ScadaDTO entity = helper.Create(dr);
                        entity.FUENTE = fuente;
                        entitys.Add(entity);
                    }
                }

                return entitys;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        public List<ScadaDTO> ObtenerConsultaMedGeneracion(string filtro, DateTime fechaInicio, DateTime fechaFin, string fuente)
        {
            try
            {
                List<ScadaDTO> entitys = new List<ScadaDTO>();

                String query = String.Format(helper.SqlGetFromMedicionMedGeneracion, filtro,
                    fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));

                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        ScadaDTO entity = helper.Create(dr);
                        entity.FUENTE = fuente;
                        entitys.Add(entity);
                    }
                }

                return entitys;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }



        public PerfilScadaDTO ObtenerPerfil(int id)
        {
            PerfilScadaDTO entity = null;

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);
            dbProvider.AddInParameter(command, helper.PERFCODI, DbType.Int32, id);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = this.helper.CreatePerfil(dr);

                    int iPRRUNOMB = dr.GetOrdinal(this.helper.PRRUNOMB);
                    if (!dr.IsDBNull(iPRRUNOMB)) entity.PRRUNOMB = dr.GetString(iPRRUNOMB);

                    int iPRRUABREV = dr.GetOrdinal(this.helper.PRRUABREV);
                    if (!dr.IsDBNull(iPRRUABREV)) entity.PRRUABREV = dr.GetString(iPRRUABREV);
                }
            }

            return entity;
        }

        public PerfilScadaDTO ObtenerPerfilPorFormula(int idFormula, int agrupacion)
        {
            PerfilScadaDTO entity = null;

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByFormula);
            dbProvider.AddInParameter(command, helper.EJRUCODI, DbType.Int32, idFormula);
            dbProvider.AddInParameter(command, helper.PERFCLASI, DbType.Int32, agrupacion);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = this.helper.CreatePerfil(dr);
                }
            }

            return entity;
        }

        public List<PerfilScadaDetDTO> ObtienePerfilDetalle(int id)
        {
            List<PerfilScadaDetDTO> entitys = new List<PerfilScadaDetDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.PERFCODI, DbType.Int32, id);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(this.helper.CreatePerfilDetalle(dr));
                }
            }

            return entitys;
        }

        public List<PerfilScadaDTO> ListarPerfilesPorUsuario(string username, DateTime inicio, DateTime fin)
        {
            List<PerfilScadaDTO> entitys = new List<PerfilScadaDTO>();

            string query = String.Format(helper.SqlBuscarPerfiles, username, inicio.ToString(ConstantesBase.FormatoFecha),
                fin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PerfilScadaDTO entity = this.helper.CreatePerfil(dr);

                    int iPRRUNOMB = dr.GetOrdinal(this.helper.PRRUNOMB);
                    if (!dr.IsDBNull(iPRRUNOMB)) entity.PRRUNOMB = dr.GetString(iPRRUNOMB);

                    int iPRRUABREV = dr.GetOrdinal(this.helper.PRRUABREV);
                    if (!dr.IsDBNull(iPRRUABREV)) entity.PRRUABREV = dr.GetString(iPRRUABREV);

                    entitys.Add(entity);   
                }
            }

            return entitys;
        }

        public List<PerfilScadaDTO> ObtenerPerfilesExportacion(string username, DateTime fechaInicio, DateTime fechaFin)
        {
            List<PerfilScadaDTO> entitys = new List<PerfilScadaDTO>();

            string query = String.Format(this.helper.SqlObtenerExportacion, username, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);
         
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PerfilScadaDTO entity = this.helper.CreatePerfil(dr);

                    int iPRRUNOMB = dr.GetOrdinal(this.helper.PRRUNOMB);
                    if (!dr.IsDBNull(iPRRUNOMB)) entity.PRRUNOMB = dr.GetString(iPRRUNOMB);

                    int iPRRUABREV = dr.GetOrdinal(this.helper.PRRUABREV);
                    if (!dr.IsDBNull(iPRRUABREV)) entity.PRRUABREV = dr.GetString(iPRRUABREV);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int GrabarPerfil(PerfilScadaDTO entity)
        {            
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);

            int id = 1;

            object max = dbProvider.ExecuteScalar(command);

            if (max != null)
            {
                id = Convert.ToInt32(max);
            }
            command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            
            dbProvider.AddInParameter(command, helper.PERFCODI, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.PERFDESC, DbType.String, entity.PERFDESC);
            dbProvider.AddInParameter(command, helper.FECREGISTRO, DbType.DateTime, entity.FECREGISTRO);
            dbProvider.AddInParameter(command, helper.FECINICIO, DbType.DateTime, entity.FECINICIO);
            dbProvider.AddInParameter(command, helper.FECFIN, DbType.DateTime, entity.FECFIN);            
            dbProvider.AddInParameter(command, helper.LASTUSER, DbType.String, entity.LASTUSER);
            dbProvider.AddInParameter(command, helper.LASTDATE, DbType.DateTime, entity.LASTDATE);
            dbProvider.AddInParameter(command, helper.EJRUCODI, DbType.Int32, entity.EJRUCODI);
            dbProvider.AddInParameter(command, helper.PERFCLASI, DbType.Int32, entity.PERFCLASI);
            dbProvider.AddInParameter(command, helper.PERFORIG, DbType.String, entity.PERFORIG);

            dbProvider.ExecuteNonQuery(command);

            return id;
        }

        public void GrabarPerfilDetalle(ScadaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxIdDetalle);

            int id = 1;

            object max = dbProvider.ExecuteScalar(command);

            if (max != null)
            {
                id = Convert.ToInt32(max);
            }
                
            command = dbProvider.GetSqlStringCommand(helper.SqlSaveDetalle);

            dbProvider.AddInParameter(command, helper.PERFDETCODI, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.PERFCODI, DbType.Int32, entity.PERFCODI);
            dbProvider.AddInParameter(command, helper.PERFCLASI, DbType.Int32, entity.CLASIFICACION);
            dbProvider.AddInParameter(command, helper.H1, DbType.Decimal, entity.H2);
            dbProvider.AddInParameter(command, helper.H2, DbType.Decimal, entity.H4);
            dbProvider.AddInParameter(command, helper.H3, DbType.Decimal, entity.H6);
            dbProvider.AddInParameter(command, helper.H4, DbType.Decimal, entity.H8);
            dbProvider.AddInParameter(command, helper.H5, DbType.Decimal, entity.H10);
            dbProvider.AddInParameter(command, helper.H6, DbType.Decimal, entity.H12);
            dbProvider.AddInParameter(command, helper.H7, DbType.Decimal, entity.H14);
            dbProvider.AddInParameter(command, helper.H8, DbType.Decimal, entity.H16);
            dbProvider.AddInParameter(command, helper.H9, DbType.Decimal, entity.H18);
            dbProvider.AddInParameter(command, helper.H10, DbType.Decimal, entity.H20);
            dbProvider.AddInParameter(command, helper.H11, DbType.Decimal, entity.H22);
            dbProvider.AddInParameter(command, helper.H12, DbType.Decimal, entity.H24);
            dbProvider.AddInParameter(command, helper.H13, DbType.Decimal, entity.H26);
            dbProvider.AddInParameter(command, helper.H14, DbType.Decimal, entity.H28);
            dbProvider.AddInParameter(command, helper.H15, DbType.Decimal, entity.H30);
            dbProvider.AddInParameter(command, helper.H16, DbType.Decimal, entity.H32);
            dbProvider.AddInParameter(command, helper.H17, DbType.Decimal, entity.H34);
            dbProvider.AddInParameter(command, helper.H18, DbType.Decimal, entity.H36);
            dbProvider.AddInParameter(command, helper.H19, DbType.Decimal, entity.H38);
            dbProvider.AddInParameter(command, helper.H20, DbType.Decimal, entity.H40);
            dbProvider.AddInParameter(command, helper.H21, DbType.Decimal, entity.H42);
            dbProvider.AddInParameter(command, helper.H22, DbType.Decimal, entity.H44);
            dbProvider.AddInParameter(command, helper.H23, DbType.Decimal, entity.H46);
            dbProvider.AddInParameter(command, helper.H24, DbType.Decimal, entity.H48);
            dbProvider.AddInParameter(command, helper.H25, DbType.Decimal, entity.H50);
            dbProvider.AddInParameter(command, helper.H26, DbType.Decimal, entity.H52);
            dbProvider.AddInParameter(command, helper.H27, DbType.Decimal, entity.H54);
            dbProvider.AddInParameter(command, helper.H28, DbType.Decimal, entity.H56);
            dbProvider.AddInParameter(command, helper.H29, DbType.Decimal, entity.H58);
            dbProvider.AddInParameter(command, helper.H30, DbType.Decimal, entity.H60);
            dbProvider.AddInParameter(command, helper.H31, DbType.Decimal, entity.H62);
            dbProvider.AddInParameter(command, helper.H32, DbType.Decimal, entity.H64);
            dbProvider.AddInParameter(command, helper.H33, DbType.Decimal, entity.H66);
            dbProvider.AddInParameter(command, helper.H34, DbType.Decimal, entity.H68);
            dbProvider.AddInParameter(command, helper.H35, DbType.Decimal, entity.H70);
            dbProvider.AddInParameter(command, helper.H36, DbType.Decimal, entity.H72);
            dbProvider.AddInParameter(command, helper.H37, DbType.Decimal, entity.H74);
            dbProvider.AddInParameter(command, helper.H38, DbType.Decimal, entity.H76);
            dbProvider.AddInParameter(command, helper.H39, DbType.Decimal, entity.H78);
            dbProvider.AddInParameter(command, helper.H40, DbType.Decimal, entity.H80);
            dbProvider.AddInParameter(command, helper.H41, DbType.Decimal, entity.H82);
            dbProvider.AddInParameter(command, helper.H42, DbType.Decimal, entity.H84);
            dbProvider.AddInParameter(command, helper.H43, DbType.Decimal, entity.H86);
            dbProvider.AddInParameter(command, helper.H44, DbType.Decimal, entity.H88);
            dbProvider.AddInParameter(command, helper.H45, DbType.Decimal, entity.H90);
            dbProvider.AddInParameter(command, helper.H46, DbType.Decimal, entity.H92);
            dbProvider.AddInParameter(command, helper.H47, DbType.Decimal, entity.H94);
            dbProvider.AddInParameter(command, helper.H48, DbType.Decimal, entity.H96);
            dbProvider.AddInParameter(command, helper.TH1, DbType.Decimal, entity.TH2);
            dbProvider.AddInParameter(command, helper.TH2, DbType.Decimal, entity.TH4);
            dbProvider.AddInParameter(command, helper.TH3, DbType.Decimal, entity.TH6);
            dbProvider.AddInParameter(command, helper.TH4, DbType.Decimal, entity.TH8);
            dbProvider.AddInParameter(command, helper.TH5, DbType.Decimal, entity.TH10);
            dbProvider.AddInParameter(command, helper.TH6, DbType.Decimal, entity.TH12);
            dbProvider.AddInParameter(command, helper.TH7, DbType.Decimal, entity.TH14);
            dbProvider.AddInParameter(command, helper.TH8, DbType.Decimal, entity.TH16);
            dbProvider.AddInParameter(command, helper.TH9, DbType.Decimal, entity.TH18);
            dbProvider.AddInParameter(command, helper.TH10, DbType.Decimal, entity.TH20);
            dbProvider.AddInParameter(command, helper.TH11, DbType.Decimal, entity.TH22);
            dbProvider.AddInParameter(command, helper.TH12, DbType.Decimal, entity.TH24);
            dbProvider.AddInParameter(command, helper.TH13, DbType.Decimal, entity.TH26);
            dbProvider.AddInParameter(command, helper.TH14, DbType.Decimal, entity.TH28);
            dbProvider.AddInParameter(command, helper.TH15, DbType.Decimal, entity.TH30);
            dbProvider.AddInParameter(command, helper.TH16, DbType.Decimal, entity.TH32);
            dbProvider.AddInParameter(command, helper.TH17, DbType.Decimal, entity.TH34);
            dbProvider.AddInParameter(command, helper.TH18, DbType.Decimal, entity.TH36);
            dbProvider.AddInParameter(command, helper.TH19, DbType.Decimal, entity.TH38);
            dbProvider.AddInParameter(command, helper.TH20, DbType.Decimal, entity.TH40);
            dbProvider.AddInParameter(command, helper.TH21, DbType.Decimal, entity.TH42);
            dbProvider.AddInParameter(command, helper.TH22, DbType.Decimal, entity.TH44);
            dbProvider.AddInParameter(command, helper.TH23, DbType.Decimal, entity.TH46);
            dbProvider.AddInParameter(command, helper.TH24, DbType.Decimal, entity.TH48);
            dbProvider.AddInParameter(command, helper.TH25, DbType.Decimal, entity.TH50);
            dbProvider.AddInParameter(command, helper.TH26, DbType.Decimal, entity.TH52);
            dbProvider.AddInParameter(command, helper.TH27, DbType.Decimal, entity.TH54);
            dbProvider.AddInParameter(command, helper.TH28, DbType.Decimal, entity.TH56);
            dbProvider.AddInParameter(command, helper.TH29, DbType.Decimal, entity.TH58);
            dbProvider.AddInParameter(command, helper.TH30, DbType.Decimal, entity.TH60);
            dbProvider.AddInParameter(command, helper.TH31, DbType.Decimal, entity.TH62);
            dbProvider.AddInParameter(command, helper.TH32, DbType.Decimal, entity.TH64);
            dbProvider.AddInParameter(command, helper.TH33, DbType.Decimal, entity.TH66);
            dbProvider.AddInParameter(command, helper.TH34, DbType.Decimal, entity.TH68);
            dbProvider.AddInParameter(command, helper.TH35, DbType.Decimal, entity.TH70);
            dbProvider.AddInParameter(command, helper.TH36, DbType.Decimal, entity.TH72);
            dbProvider.AddInParameter(command, helper.TH37, DbType.Decimal, entity.TH74);
            dbProvider.AddInParameter(command, helper.TH38, DbType.Decimal, entity.TH76);
            dbProvider.AddInParameter(command, helper.TH39, DbType.Decimal, entity.TH78);
            dbProvider.AddInParameter(command, helper.TH40, DbType.Decimal, entity.TH80);
            dbProvider.AddInParameter(command, helper.TH41, DbType.Decimal, entity.TH82);
            dbProvider.AddInParameter(command, helper.TH42, DbType.Decimal, entity.TH84);
            dbProvider.AddInParameter(command, helper.TH43, DbType.Decimal, entity.TH86);
            dbProvider.AddInParameter(command, helper.TH44, DbType.Decimal, entity.TH88);
            dbProvider.AddInParameter(command, helper.TH45, DbType.Decimal, entity.TH90);
            dbProvider.AddInParameter(command, helper.TH46, DbType.Decimal, entity.TH92);
            dbProvider.AddInParameter(command, helper.TH47, DbType.Decimal, entity.TH94);
            dbProvider.AddInParameter(command, helper.TH48, DbType.Decimal, entity.TH96);

            dbProvider.ExecuteNonQuery(command);
            
        }

        public object ObtenerValorTiempoRealCongestion(int codScada, DateTime fechaProceso)
        { 
            string sql = string.Format(helper.SqlObtenerCongestion, codScada, fechaProceso.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            return dbProvider.ExecuteScalar(command);                
        }

        public List<ScadaDTO> ObtenerConsultaScadaSP7(string filtro, DateTime fechaInicio, DateTime fechaFin)
        {
            List<ScadaDTO> entitys = new List<ScadaDTO>();

            String query = String.Format(helper.SqlGetFromScadaSP7, filtro,
                fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));

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

    }
}
