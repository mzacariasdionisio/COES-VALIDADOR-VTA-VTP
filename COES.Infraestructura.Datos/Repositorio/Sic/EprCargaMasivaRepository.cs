using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Framework.Base.Tools;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla EPR_PROYECTO_ACTEQP
    /// </summary>
    public class EprCargaMasivaRepository : RepositoryBase, IEprCargaMasivaRepository
    {
        public EprCargaMasivaRepository(string strConn) : base(strConn)
        {
        }

        EprCargaMasivaHelper helper = new EprCargaMasivaHelper();

        public int Save(EprCargaMasivaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Epcamacodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Epcamatipuso, DbType.Int32, entity.Epcamatipuso);
            dbProvider.AddInParameter(command, helper.Epcamafeccarga, DbType.DateTime, entity.Epcamafeccarga.Date);
            dbProvider.AddInParameter(command, helper.Epcamatotalregistro, DbType.Int32, entity.Epcamatotalregistro);
            dbProvider.AddInParameter(command, helper.Epcamausucreacion, DbType.String, entity.Epcamausucreacion);
           
            dbProvider.ExecuteNonQuery(command);
            return id;
        }

      
        public EprCargaMasivaDTO GetById(int epequicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Epcamacodi, DbType.Int32, epequicodi);
            EprCargaMasivaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<EprCargaMasivaDTO> ListarCargaMasivaFiltro(int tipoUso, string usuario, string fecIni, string fecFin)
        {
            List<EprCargaMasivaDTO> entitys = new List<EprCargaMasivaDTO>();

            string condicion = "";          

            if (!string.IsNullOrEmpty(usuario))
            {
                condicion = condicion + " AND UPPER(EPCAMAUSUCREACION) LIKE '%" + usuario.ToUpper() + "%'";
            }

            if (fecIni != null && !fecIni.Equals(""))
            {
                condicion = condicion + " AND TRUNC(EPCAMAFECCARGA) >= TO_DATE('" + fecIni + "' , 'DD/MM/YYYY') ";
            }

            if (fecFin != null && !fecFin.Equals(""))
            {
                condicion = condicion + " AND TRUNC(EPCAMAFECCARGA) <= TO_DATE('" + fecFin + "' , 'DD/MM/YYYY') ";
            }

            string queryString = string.Format(helper.SqlListFiltroCargaMasiva, condicion);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.AddInParameter(command, helper.Epcamatipuso, DbType.Int32, tipoUso);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EprCargaMasivaDTO entity = new EprCargaMasivaDTO();

                    int iRccarecodi = dr.GetOrdinal(helper.Epcamacodi);
                    if (!dr.IsDBNull(iRccarecodi)) entity.Epcamacodi = Convert.ToInt32(dr.GetValue(iRccarecodi));

                    int iEmprcodi = dr.GetOrdinal(helper.Epcamatipuso);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Epcamatipuso = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEpcamafeccarga = dr.GetOrdinal(helper.Epcamafeccarga);
                    if (!dr.IsDBNull(iEpcamafeccarga)) entity.Epcamafeccarga = dr.GetDateTime(iEpcamafeccarga);

                    int iEpcamatotalregistro = dr.GetOrdinal(helper.Epcamatotalregistro);
                    if (!dr.IsDBNull(iEpcamatotalregistro)) entity.Epcamatotalregistro = Convert.ToInt32(dr.GetValue(iEpcamatotalregistro));

                    int iEpcamatipusoNombre = dr.GetOrdinal(helper.Epcamatipusonombre);
                    if (!dr.IsDBNull(iEpcamatipusoNombre)) entity.Epcamatipusonombre = dr.GetString(iEpcamatipusoNombre);

                    int iRccarefecharecepcion = dr.GetOrdinal(helper.Epcamafeccreacion);
                    if (!dr.IsDBNull(iRccarefecharecepcion)) entity.Epcamafeccreacion = dr.GetDateTime(iRccarefecharecepcion);

                    int iRccareestado = dr.GetOrdinal(helper.Epcamausucreacion);
                    if (!dr.IsDBNull(iRccareestado)) entity.Epcamausucreacion = dr.GetString(iRccareestado);
                                     
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public string ValidarProteccionesUsoGeneral(EprCargaMasivaDetalleDTO entity)
        {           

            var valores = new List<string>
            {
                "'" + entity.CodigoEmpresa + "'",
                "'" + entity.CodigoCelda + "'",
                "'" + entity.CodigoRele + "'",
                "'" + entity.NombreRele + "'",
                "'" + entity.Fecha + "'",
                "'" + entity.Estado + "'",
                "'" + entity.NivelTension + "'",
                "'" + entity.SistemaRele + "'",
                "'" + entity.Fabricante + "'",
                "'" + entity.Modelo + "'",
                "'" + entity.RTC_Ip + "'",
                "'" + entity.RTC_Is + "'",
                "'" + entity.RTT_Vp + "'",
                "'" + entity.RTT_Vs + "'",
                "'" + entity.ProteccionesCoordinables + "'",
                "'" + entity.SincActivo + "'",
                "'" + entity.SincInterruptor + "'",
                "'" + entity.SincTension + "'",
                "'" + entity.SincAngulo + "'",
                "'" + entity.SincFrecuencia + "'",
                "'" + entity.SobretActivo + "'",
                "'" + entity.SobretInterruptor + "'",
                "'" + entity.SobretTension + "'",
                "'" + entity.SobretAngulo + "'",
                "'" + entity.SobretFrecuencia + "'",
                "'" + entity.SobreCorrienteActivo + "'",
                "'" + entity.SobreCorrienteActivoDelta + "'",
                "'" + entity.PmuActivo + "'",
                "'" + entity.PmuAccion + "'",
                "'" + entity.Proyecto + "'"
            };
           

            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlValidarProteccionesUsoGeneral, valores.ToArray()));
            object result = dbProvider.ExecuteScalar(command);
            string res = "";
            if (result != null) res = Convert.ToString(result);
          
            return res;
        }

        public string SaveProteccionesUsoGeneral(EprCargaMasivaDetalleDTO entity)
        {

            var valores = new List<string>
            {
                "'" + entity.CodigoEmpresa + "'",
                "'" + entity.CodigoCelda + "'",
                "'" + entity.CodigoRele + "'",
                "'" + entity.NombreRele + "'",
                "'" + entity.Fecha + "'",
                "'" + entity.Estado + "'",
                "'" + entity.NivelTension + "'",
                "'" + entity.SistemaRele + "'",
                "'" + entity.Fabricante + "'",
                "'" + entity.Modelo + "'",
                "'" + entity.RTC_Ip + "'",
                "'" + entity.RTC_Is + "'",
                "'" + entity.RTT_Vp + "'",
                "'" + entity.RTT_Vs + "'",
                "'" + entity.ProteccionesCoordinables + "'",
                "'" + entity.SincActivo + "'",
                "'" + entity.SincInterruptor + "'",
                "'" + entity.SincTension + "'",
                "'" + entity.SincAngulo + "'",
                "'" + entity.SincFrecuencia + "'",
                "'" + entity.SobretActivo + "'",
                "'" + entity.SobretInterruptor + "'",
                "'" + entity.SobretTension + "'",
                "'" + entity.SobretAngulo + "'",
                "'" + entity.SobretFrecuencia + "'",
                "'" + entity.SobreCorrienteActivo + "'",
                "'" + entity.SobreCorrienteActivoDelta + "'",
                "'" + entity.PmuActivo + "'",
                "'" + entity.PmuAccion + "'",
                "'" + entity.Proyecto + "'",
                "'" + entity.NombreUsuario + "'"
            };


            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlSaveProteccionesUsoGeneral, valores.ToArray()));
            object result = dbProvider.ExecuteScalar(command);
            string res = "";
            if (result != null) res = Convert.ToString(result);

            return res;
        }

        public string ValidarProteccionesMandoSincronizado(EprCargaMasivaDetalleDTO entity)
        {

            var valores = new List<string>
            {
                "'" + entity.CodigoEmpresa + "'",
                "'" + entity.CodigoCelda + "'",
                "'" + entity.CodigoRele + "'",
                "'" + entity.NombreRele + "'",
                "'" + entity.Fecha + "'",
                "'" + entity.Estado + "'",
                "'" + entity.NivelTension + "'",
                "'" + entity.SistemaRele + "'",
                "'" + entity.Fabricante + "'",
                "'" + entity.Modelo + "'",
                "'" + entity.InterruptorComanda + "'",
                "'" + entity.Mando + "'",          
                "'" + entity.Proyecto + "'"
            };


            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlValidarProteccionesMandoSincronizado, valores.ToArray()));
            object result = dbProvider.ExecuteScalar(command);
            string res = "";
            if (result != null) res = Convert.ToString(result);

            return res;
        }

        public string SaveProteccionesMandoSincronizado(EprCargaMasivaDetalleDTO entity)
        {

            var valores = new List<string>
            {
                "'" + entity.CodigoEmpresa + "'",
                "'" + entity.CodigoCelda + "'",
                "'" + entity.CodigoRele + "'",
                "'" + entity.NombreRele + "'",
                "'" + entity.Fecha + "'",
                "'" + entity.Estado + "'",
                "'" + entity.NivelTension + "'",
                "'" + entity.SistemaRele + "'",
                "'" + entity.Fabricante + "'",
                "'" + entity.Modelo + "'",
                "'" + entity.InterruptorComanda + "'",
                "'" + entity.Mando + "'",
                "'" + entity.Proyecto + "'",
                "'" + entity.NombreUsuario + "'"
            };


            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlSaveProteccionesMandoSincronizado, valores.ToArray()));
            object result = dbProvider.ExecuteScalar(command);
            string res = "";
            if (result != null) res = Convert.ToString(result);

            return res;
        }


        public string ValidarProteccionesTorsional(EprCargaMasivaDetalleDTO entity)
        {

            var valores = new List<string>
            {
                "'" + entity.CodigoEmpresa + "'",
                "'" + entity.CodigoCelda + "'",
                "'" + entity.CodigoRele + "'",
                "'" + entity.NombreRele + "'",
                "'" + entity.Fecha + "'",
                "'" + entity.Estado + "'",
                "'" + entity.NivelTension + "'",
                "'" + entity.SistemaRele + "'",
                "'" + entity.Fabricante + "'",
                "'" + entity.Modelo + "'",
                "'" + entity.MedidaMitigacion + "'",
                "'" + entity.Implementado + "'",
                "'" + entity.Proyecto + "'"
            };


            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlValidarProteccionesTorsional, valores.ToArray()));
            object result = dbProvider.ExecuteScalar(command);
            string res = "";
            if (result != null) res = Convert.ToString(result);

            return res;
        }

        public string SaveProteccionesTorsional(EprCargaMasivaDetalleDTO entity)
        {

            var valores = new List<string>
            {
                "'" + entity.CodigoEmpresa + "'",
                "'" + entity.CodigoCelda + "'",
                "'" + entity.CodigoRele + "'",
                "'" + entity.NombreRele + "'",
                "'" + entity.Fecha + "'",
                "'" + entity.Estado + "'",
                "'" + entity.NivelTension + "'",
                "'" + entity.SistemaRele + "'",
                "'" + entity.Fabricante + "'",
                "'" + entity.Modelo + "'",
                "'" + entity.MedidaMitigacion + "'",
                "'" + entity.Implementado + "'",
                "'" + entity.Proyecto + "'",
                "'" + entity.NombreUsuario + "'"
            };


            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlSaveProteccionesTorsional, valores.ToArray()));
            object result = dbProvider.ExecuteScalar(command);
            string res = "";
            if (result != null) res = Convert.ToString(result);

            return res;
        }

        public string ValidarProteccionesPmu(EprCargaMasivaDetalleDTO entity)
        {

            var valores = new List<string>
            {
                "'" + entity.CodigoEmpresa + "'",
                "'" + entity.CodigoCelda + "'",
                "'" + entity.CodigoRele + "'",
                "'" + entity.NombreRele + "'",
                "'" + entity.Fecha + "'",
                "'" + entity.Estado + "'",
                "'" + entity.NivelTension + "'",
                "'" + entity.SistemaRele + "'",
                "'" + entity.Fabricante + "'",
                "'" + entity.Modelo + "'",
                "'" + entity.PmuAccion + "'",
                "'" + entity.Implementado + "'",
                "'" + entity.Proyecto + "'"
            };


            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlValidarProteccionesPmu, valores.ToArray()));
            object result = dbProvider.ExecuteScalar(command);
            string res = "";
            if (result != null) res = Convert.ToString(result);

            return res;
        }

        public string SaveProteccionesPmu(EprCargaMasivaDetalleDTO entity)
        {

            var valores = new List<string>
            {
                "'" + entity.CodigoEmpresa + "'",
                "'" + entity.CodigoCelda + "'",
                "'" + entity.CodigoRele + "'",
                "'" + entity.NombreRele + "'",
                "'" + entity.Fecha + "'",
                "'" + entity.Estado + "'",
                "'" + entity.NivelTension + "'",
                "'" + entity.SistemaRele + "'",
                "'" + entity.Fabricante + "'",
                "'" + entity.Modelo + "'",
                "'" + entity.PmuAccion + "'",
                "'" + entity.Implementado + "'",
                "'" + entity.Proyecto + "'",
                "'" + entity.NombreUsuario + "'"
            };


            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlSaveProteccionesPmu, valores.ToArray()));
            object result = dbProvider.ExecuteScalar(command);
            string res = "";
            if (result != null) res = Convert.ToString(result);

            return res;
        }

        #region GESPROTECT - 2050206

        public string ValidarCargaMasivaLinea(EprCargaMasivaLineaDTO entity)
        {

            var valores = new List<string>
            {
                "'" + entity.Codigo + "'",
                "'" + entity.Ubicacion + "'",
                "'" + entity.CapacidadA + "'",
                "'" + entity.Celda + "'",
                "'" + entity.Celda2 + "'",
                "'" + entity.BancoCondensador + "'",
                "'" + entity.BancoCapacidadA + "'",
                "'" + entity.BancoCapacidadMVAr + "'",
                "'" + entity.CapacTransCond1Porcen + "'",
                "'" + entity.CapacTransCond1Min + "'",
                "'" + entity.CapacTransCond2Porcen + "'",
                "'" + entity.CapacTransCond2Min + "'",
                "'" + entity.LimiteSegCoes + "'",
                "'" + entity.Observaciones + "'",
                "'" + entity.Motivo + "'"
            };


            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlValidarProteccionesLinea, valores.ToArray()));
            object result = dbProvider.ExecuteScalar(command);
            string res = "";
            if (result != null) res = Convert.ToString(result);

            return res;
        }

        public string SaveCargaMasivaLinea(EprCargaMasivaLineaDTO entity)
        {

            var valores = new List<string>
            {
                "'" + entity.Codigo + "'",
                "'" + entity.ComentarioCodigo + "'",
                "'" + entity.Ubicacion + "'",
                "'" + entity.ComentarioUbicacion + "'",
                "'" + entity.CapacidadA + "'",
                "'" + entity.ComentarioCapacidadA + "'",
                "'" + entity.Celda + "'",
                 "'" + entity.ComentarioCelda + "'",
                "'" + entity.Celda2 + "'",
                 "'" + entity.ComentarioCelda2 + "'",
                "'" + entity.BancoCondensador + "'",
                 "'" + entity.ComentarioBancoCondensador + "'",
                "'" + entity.BancoCapacidadA + "'",
                 "'" + entity.ComentarioBancoCapacidadA + "'",
                "'" + entity.BancoCapacidadMVAr + "'",
                 "'" + entity.ComentarioBancoCapacidadMVAr + "'",
                "'" + entity.CapacTransCond1Porcen + "'",
                "'" + entity.ComentarioCapacTransCond1Porcen + "'",
                "'" + entity.CapacTransCond1Min + "'",
                 "'" + entity.ComentarioCapacTransCond1Min + "'",
                "'" + entity.CapacTransCond2Porcen + "'",
                 "'" + entity.ComentarioCapacTransCond2Porcen + "'",
                "'" + entity.CapacTransCond2Min + "'",
                 "'" + entity.ComentarioCapacTransCond2Min + "'",
                "'" + entity.LimiteSegCoes + "'",
                 "'" + entity.ComentarioLimiteSegCoes + "'",
                "'" + entity.Observaciones + "'",
                 "'" + entity.ComentarioObservaciones + "'",
                "'" + entity.Motivo + "'",
                  "'" + entity.ComentarioMotivo + "'",
                "'" + entity.NombreUsuario + "'"
            };


            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlSaveProteccionesLinea, valores.ToArray()));
            object result = dbProvider.ExecuteScalar(command);
            string res = "";
            if (result != null) res = Convert.ToString(result);

            return res;
        }

        public string ValidarCargaMasivaReactor(EprCargaMasivaLineaDTO entity)
        {

            var valores = new List<string>
            {
                "'" + entity.Codigo + "'",         
                "'" + entity.Celda + "'",
                "'" + entity.Celda2 + "'",
                 "'" + entity.CapacidadMvar + "'",
                 "'" + entity.CapacidadA + "'",              
                "'" + entity.Observaciones + "'",
                "'" + entity.Motivo + "'"
            };


            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlValidarProteccionesReactor, valores.ToArray()));
            object result = dbProvider.ExecuteScalar(command);
            string res = "";
            if (result != null) res = Convert.ToString(result);

            return res;
        }

        public string SaveCargaMasivaReactor(EprCargaMasivaLineaDTO entity)
        {

            var valores = new List<string>
            {
                "'" + entity.Codigo + "'",
                "'" + entity.ComentarioCodigo + "'",            
                "'" + entity.Celda + "'",
                 "'" + entity.ComentarioCelda + "'",
                "'" + entity.Celda2 + "'",
                 "'" + entity.ComentarioCelda2 + "'",
                   "'" + entity.CapacidadMvar + "'",
                "'" + entity.ComentarioCapacidadMvar + "'",
                   "'" + entity.CapacidadA + "'",
                "'" + entity.ComentarioCapacidadA + "'",
                "'" + entity.Observaciones + "'",
                 "'" + entity.ComentarioObservaciones + "'",
                "'" + entity.Motivo + "'",
                  "'" + entity.ComentarioMotivo + "'",
                "'" + entity.NombreUsuario + "'"
            };


            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlSaveProteccionesReactor, valores.ToArray()));
            object result = dbProvider.ExecuteScalar(command);
            string res = "";
            if (result != null) res = Convert.ToString(result);

            return res;
        }

        public string ValidarCargaMasivaCeldaAcoplamiento(EprCargaMasivaCeldaAcoplamientoDTO entity)
        {

            var valores = new List<string>
            {
                "'" + entity.Codigo + "'",
                "'" + entity.CodigoInterruptorAcoplamiento + "'",
                "'" + entity.CapacidadInterruptorAcoplamiento + "'",                
                "'" + entity.Observaciones + "'",
                "'" + entity.Motivo + "'"
            };


            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlValidarProteccionesCeldaAcoplamiento, valores.ToArray()));
            object result = dbProvider.ExecuteScalar(command);
            string res = "";
            if (result != null) res = Convert.ToString(result);

            return res;
        }

        public string SaveCargaMasivaCeldaAcoplamiento(EprCargaMasivaCeldaAcoplamientoDTO entity)
        {

            var valores = new List<string>
            {
                "'" + entity.Codigo + "'",
                "'" + entity.ComentarioCodigo + "'",
                "'" + entity.CodigoInterruptorAcoplamiento + "'",
                 "'" + entity.ComentarioCodigoInterruptorAcoplamiento + "'",
                "'" + entity.CapacidadInterruptorAcoplamiento + "'",
                 "'" + entity.ComentarioCapacidadInterruptorAcoplamiento + "'",               
                "'" + entity.Observaciones + "'",
                 "'" + entity.ComentarioObservaciones + "'",
                "'" + entity.Motivo + "'",
                  "'" + entity.ComentarioMotivo + "'",
                "'" + entity.NombreUsuario + "'"
            };


            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlSaveProteccionesCeldaAcoplamiento, valores.ToArray()));
            object result = dbProvider.ExecuteScalar(command);
            string res = "";
            if (result != null) res = Convert.ToString(result);

            return res;
        }

        public string ValidarCargaMasivaTransformador(EprCargaMasivaTransformadorDTO entity)
        {

            var valores = new List<string>
            {
                "'" + entity.Codigo + "'",
                "'" + entity.DevanadoCodigo + "'",
                "'" + entity.DevanadoCapacidadONAN + "'",
                "'" + entity.DevanadoCapacidadONAF + "'",
                "'" + entity.DevanadoDosCodigo + "'",
                "'" + entity.DevanadoDosCapacidadONAN + "'",
                "'" + entity.DevanadoDosCapacidadONAF + "'",
                 "'" + entity.DevanadoTresCodigo + "'",
                "'" + entity.DevanadoTresCapacidadONAN + "'",
                "'" + entity.DevanadoTresCapacidadONAF + "'",
                 "'" + entity.DevanadoCuatroCodigo + "'",
                "'" + entity.DevanadoCuatroCapacidadONAN + "'",
                "'" + entity.DevanadoCuatroCapacidadONAF + "'",             
                "'" + entity.Observaciones + "'",
                "'" + entity.Motivo + "'"
            };


            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlValidarProteccionesTransformador, valores.ToArray()));
            object result = dbProvider.ExecuteScalar(command);
            string res = "";
            if (result != null) res = Convert.ToString(result);

            return res;
        }

        public string SaveCargaMasivaTransformador(EprCargaMasivaTransformadorDTO entity)
        {

            var valores = new List<string>
            {
                "'" + entity.Codigo + "'",
                "'" + entity.ComentarioCodigo + "'",
                "'" + entity.DevanadoCodigo + "'",
                "'" + entity.ComentarioDevanadoCodigo+ "'",
                "'" + entity.DevanadoCapacidadONAN + "'",
                "'" + entity.ComentarioDevanadoCapacidadONAN + "'",
                "'" + entity.DevanadoCapacidadONAF + "'",
                 "'" + entity.ComentarioDevanadoCapacidadONAF + "'",
                "'" + entity.DevanadoDosCodigo + "'",
                 "'" + entity.ComentarioDevanadoDosCodigo + "'",
                "'" + entity.DevanadoDosCapacidadONAN + "'",
                 "'" + entity.ComentarioDevanadoDosCapacidadONAN+ "'",
                 "'" + entity.DevanadoDosCapacidadONAF + "'",
                 "'" + entity.ComentarioDevanadoDosCapacidadONAF+ "'",             
                 "'" + entity.DevanadoTresCodigo + "'",
                 "'" + entity.ComentarioDevanadoTresCodigo + "'",
                "'" + entity.DevanadoTresCapacidadONAN + "'",
                 "'" + entity.ComentarioDevanadoTresCapacidadONAN+ "'",
                 "'" + entity.DevanadoTresCapacidadONAF + "'",
                 "'" + entity.ComentarioDevanadoTresCapacidadONAF+ "'",               
                 "'" + entity.DevanadoCuatroCodigo + "'",
                 "'" + entity.ComentarioDevanadoCuatroCodigo + "'",
                "'" + entity.DevanadoCuatroCapacidadONAN + "'",
                 "'" + entity.ComentarioDevanadoCuatroCapacidadONAN+ "'",
                 "'" + entity.DevanadoCuatroCapacidadONAF + "'",
                 "'" + entity.ComentarioDevanadoCuatroCapacidadONAF+ "'",              
                "'" + entity.Observaciones + "'",
                 "'" + entity.ComentarioObservaciones + "'",
                "'" + entity.Motivo + "'",
                  "'" + entity.ComentarioMotivo + "'",
                "'" + entity.NombreUsuario + "'"
            };


            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlSaveProteccionesTransformador, valores.ToArray()));
            object result = dbProvider.ExecuteScalar(command);
            string res = "";
            if (result != null) res = Convert.ToString(result);

            return res;
        }

        #region Exportacion de Datos

        public List<EprEquipoReporteDTO> ListLineaEvaluacionReporte(string equicodi, string codigo, string emprcodi, string equiestado,
            string idsuestacion1, string idsuestacion2, string idarea, string idAreaReporte, string tension)
        {
            List<EprEquipoReporteDTO> entitys = new List<EprEquipoReporteDTO>();

            var consulta = string.Format(helper.ListLineaEvaluacionReporte, new string[] { equicodi, codigo, emprcodi, equiestado, idsuestacion1, idsuestacion2, idarea, idAreaReporte, tension });

            DbCommand command = dbProvider.GetSqlStringCommand(consulta);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EprEquipoReporteDTO entity = new EprEquipoReporteDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Codigo_Id);
                    if (!dr.IsDBNull(iEquicodi)) entity.Codigo_Id = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iCodigo = dr.GetOrdinal(helper.Codigo);
                    if (!dr.IsDBNull(iCodigo)) entity.Codigo = Convert.ToString(dr.GetValue(iCodigo));

                    int iEmprNomb = dr.GetOrdinal(helper.Ubicacion);
                    if (!dr.IsDBNull(iEmprNomb)) entity.Ubicacion = Convert.ToString(dr.GetValue(iEmprNomb));

                    int iEquiAbrev = dr.GetOrdinal(helper.Empresa);
                    if (!dr.IsDBNull(iEquiAbrev)) entity.Empresa = Convert.ToString(dr.GetValue(iEquiAbrev));

                    int iArea = dr.GetOrdinal(helper.Area);
                    if (!dr.IsDBNull(iArea)) entity.Area = Convert.ToString(dr.GetValue(iArea));

                    int iLongitudReporte = dr.GetOrdinal(helper.Longitud_Reporte);
                    if (!dr.IsDBNull(iLongitudReporte)) entity.Longitud = Convert.ToString(dr.GetValue(iLongitudReporte));

                    int iLongitudComent = dr.GetOrdinal(helper.Longitud_Coment);
                    if (!dr.IsDBNull(iLongitudComent)) entity.Longitud_Coment = Convert.ToString(dr.GetValue(iLongitudComent));

                    int iTension = dr.GetOrdinal(helper.Tension_Reporte);
                    if (!dr.IsDBNull(iTension)) entity.Tension = Convert.ToString(dr.GetValue(iTension));

                    int iTensionC = dr.GetOrdinal(helper.Tension_Coment);
                    if (!dr.IsDBNull(iTensionC)) entity.Tension_Coment = Convert.ToString(dr.GetValue(iTensionC));

                    int iCapacidadA = dr.GetOrdinal(helper.Capacidad_A);
                    if (!dr.IsDBNull(iCapacidadA)) entity.Capacidad_A = Convert.ToString(dr.GetValue(iCapacidadA));

                    int iCapacidadAC = dr.GetOrdinal(helper.Capacidad_A_Coment);
                    if (!dr.IsDBNull(iCapacidadAC)) entity.Capacidad_A_Coment = Convert.ToString(dr.GetValue(iCapacidadAC));

                    int iCapacidadMva = dr.GetOrdinal(helper.Capacidad_Mva);
                    if (!dr.IsDBNull(iCapacidadMva)) entity.Capacidad_Mva = Convert.ToString(dr.GetValue(iCapacidadMva));

                    int iIdCelda1 = dr.GetOrdinal(helper.Id_Celda_1);
                    if (!dr.IsDBNull(iIdCelda1)) entity.Id_Celda_1 = Convert.ToString(dr.GetValue(iIdCelda1));

                    int iNombreCelda1 = dr.GetOrdinal(helper.Nombre_Celda_1);
                    if (!dr.IsDBNull(iNombreCelda1)) entity.Nombre_Celda_1 = Convert.ToString(dr.GetValue(iNombreCelda1));

                    int iUbicacionCelda1 = dr.GetOrdinal(helper.Ubicacion_Celda_1);
                    if (!dr.IsDBNull(iUbicacionCelda1)) entity.Ubicacion_Celda_1 = Convert.ToString(dr.GetValue(iUbicacionCelda1));

                    int iPosicionNucleo1 = dr.GetOrdinal(helper.Posicion_Nucleo_Tc_Celda_1);
                    if (!dr.IsDBNull(iPosicionNucleo1)) entity.Posicion_Nucleo_Tc_Celda_1 = Convert.ToString(dr.GetValue(iPosicionNucleo1));

                    int iPickupCelda1 = dr.GetOrdinal(helper.Pick_Up_Celda_1);
                    if (!dr.IsDBNull(iPickupCelda1)) entity.Pick_Up_Celda_1 = Convert.ToString(dr.GetValue(iPickupCelda1));

                    int iIdCelda2 = dr.GetOrdinal(helper.Id_Celda_2);
                    if (!dr.IsDBNull(iIdCelda2)) entity.Id_Celda_2 = Convert.ToString(dr.GetValue(iIdCelda2));

                    int iNombreCelda2 = dr.GetOrdinal(helper.Nombre_Celda_2);
                    if (!dr.IsDBNull(iNombreCelda2)) entity.Nombre_Celda_2 = Convert.ToString(dr.GetValue(iNombreCelda2));

                    int iUbicacionCelda2 = dr.GetOrdinal(helper.Ubicacion_Celda_2);
                    if (!dr.IsDBNull(iUbicacionCelda2)) entity.Ubicacion_Celda_2 = Convert.ToString(dr.GetValue(iUbicacionCelda2));

                    int iPosicionNucleo2 = dr.GetOrdinal(helper.Posicion_Nucleo_Tc_Celda_2);
                    if (!dr.IsDBNull(iPosicionNucleo2)) entity.Posicion_Nucleo_Tc_Celda_2 = Convert.ToString(dr.GetValue(iPosicionNucleo2));

                    int iPickupCelda2 = dr.GetOrdinal(helper.Pick_Up_Celda_2);
                    if (!dr.IsDBNull(iPickupCelda2)) entity.Pick_Up_Celda_2 = Convert.ToString(dr.GetValue(iPickupCelda2));

                    int iIdBancoCondensador = dr.GetOrdinal(helper.Id_Banco_Condensador);
                    if (!dr.IsDBNull(iIdBancoCondensador)) entity.Id_Banco_Condensador = Convert.ToString(dr.GetValue(iIdBancoCondensador));

                    int iNombreBancoCondensador = dr.GetOrdinal(helper.Nombre_Banco_Condensador);
                    if (!dr.IsDBNull(iNombreBancoCondensador)) entity.Nombre_Banco_Condensador = Convert.ToString(dr.GetValue(iNombreBancoCondensador));

                    int iUbicacionBancoCondensador = dr.GetOrdinal(helper.Ubicacion_Banco_Condensador);
                    if (!dr.IsDBNull(iUbicacionBancoCondensador)) entity.Ubicacion_Banco_Condensador = Convert.ToString(dr.GetValue(iUbicacionBancoCondensador));

                    int iCapacidadABanco= dr.GetOrdinal(helper.Capacidad_A_Banco);
                    if (!dr.IsDBNull(iCapacidadABanco)) entity.Capacidad_A_Banco = Convert.ToString(dr.GetValue(iCapacidadABanco));

                    int iCapacidadMvarBanco = dr.GetOrdinal(helper.Capacidad_Mvar_Banco);
                    if (!dr.IsDBNull(iCapacidadMvarBanco)) entity.Capacidad_Mvar_Banco = Convert.ToString(dr.GetValue(iCapacidadMvarBanco));

                    int iCapacTransCond1Porcen = dr.GetOrdinal(helper.Capac_Trans_Cond_1_Porcen);
                    if (!dr.IsDBNull(iCapacTransCond1Porcen)) entity.Capac_Trans_Cond_1_Porcen = Convert.ToString(dr.GetValue(iCapacTransCond1Porcen));

                    int iCapacTransCond1PorcenC = dr.GetOrdinal(helper.Capac_Trans_Cond_1_Porcen_Coment);
                    if (!dr.IsDBNull(iCapacTransCond1PorcenC)) entity.Capac_Trans_Cond_1_Porcen_Coment = Convert.ToString(dr.GetValue(iCapacTransCond1PorcenC));

                    int iCapacTransCond1Min = dr.GetOrdinal(helper.Capac_Trans_Cond_1_Min);
                    if (!dr.IsDBNull(iCapacTransCond1Min)) entity.Capac_Trans_Cond_1_Min = Convert.ToString(dr.GetValue(iCapacTransCond1Min));

                    int iCapacTransCond1MinC = dr.GetOrdinal(helper.Capac_Trans_Cond_1_Min_Coment);
                    if (!dr.IsDBNull(iCapacTransCond1MinC)) entity.Capac_Trans_Cond_1_Min_Coment = Convert.ToString(dr.GetValue(iCapacTransCond1MinC));

                    int iCapacTransCorr1A = dr.GetOrdinal(helper.Capac_Trans_Corr_1_A);
                    if (!dr.IsDBNull(iCapacTransCorr1A)) entity.Capac_Trans_Corr_1_A = Convert.ToString(dr.GetValue(iCapacTransCorr1A));

                    int iCapacTransCorr1AC = dr.GetOrdinal(helper.Capac_Trans_Corr_1_A_Coment);
                    if (!dr.IsDBNull(iCapacTransCorr1AC)) entity.Capac_Trans_Corr_1_A_Coment = Convert.ToString(dr.GetValue(iCapacTransCorr1AC));

                    int iCapacTransCond2Porcen = dr.GetOrdinal(helper.Capac_Trans_Cond_2_Porcen);
                    if (!dr.IsDBNull(iCapacTransCond2Porcen)) entity.Capac_Trans_Cond_2_Porcen = Convert.ToString(dr.GetValue(iCapacTransCond2Porcen));

                    int iCapacTransCond2PorcenC = dr.GetOrdinal(helper.Capac_Trans_Cond_2_Porcen_Coment);
                    if (!dr.IsDBNull(iCapacTransCond2PorcenC)) entity.Capac_Trans_Cond_2_Porcen_Coment = Convert.ToString(dr.GetValue(iCapacTransCond2PorcenC));

                    int iCapacTransCond2Min = dr.GetOrdinal(helper.Capac_Trans_Cond_2_Min);
                    if (!dr.IsDBNull(iCapacTransCond2Min)) entity.Capac_Trans_Cond_2_Min = Convert.ToString(dr.GetValue(iCapacTransCond2Min));

                    int iCapacTransCond2MinC = dr.GetOrdinal(helper.Capac_Trans_Cond_2_Min_Coment);
                    if (!dr.IsDBNull(iCapacTransCond2MinC)) entity.Capac_Trans_Cond_2_Min_Coment = Convert.ToString(dr.GetValue(iCapacTransCond2MinC));

                    int iCapacTransCorr2A = dr.GetOrdinal(helper.Capac_Trans_Corr_2_A);
                    if (!dr.IsDBNull(iCapacTransCorr2A)) entity.Capac_Trans_Corr_2_A = Convert.ToString(dr.GetValue(iCapacTransCorr2A));

                    int iCapacTransCorr2AC = dr.GetOrdinal(helper.Capac_Trans_Corr_2_A_Coment);
                    if (!dr.IsDBNull(iCapacTransCorr2AC)) entity.Capac_Trans_Corr_2_A_Coment = Convert.ToString(dr.GetValue(iCapacTransCorr2AC));

                    int iCapacTrans = dr.GetOrdinal(helper.Capacidad_Transmision_A);
                    if (!dr.IsDBNull(iCapacTrans)) entity.Capacidad_Transmision_A = Convert.ToString(dr.GetValue(iCapacTrans));

                    int iCapacTransC = dr.GetOrdinal(helper.Capacidad_Transmision_A_Coment);
                    if (!dr.IsDBNull(iCapacTransC)) entity.Capacidad_Transmision_A_Coment = Convert.ToString(dr.GetValue(iCapacTransC));

                    int iCapacTransMva = dr.GetOrdinal(helper.Capacidad_Transmision_Mva);
                    if (!dr.IsDBNull(iCapacTransMva)) entity.Capacidad_Transmision_Mva = Convert.ToString(dr.GetValue(iCapacTransMva));

                    int iCapacTransMvaC = dr.GetOrdinal(helper.Capacidad_Transmision_Mva_Coment);
                    if (!dr.IsDBNull(iCapacTransMvaC)) entity.Capacidad_Transmision_Mva_Coment = Convert.ToString(dr.GetValue(iCapacTransMvaC));

                    int iLimite = dr.GetOrdinal(helper.Limite_Seg_Coes);
                    if (!dr.IsDBNull(iLimite)) entity.Limite_Seg_Coes = Convert.ToString(dr.GetValue(iLimite));

                    int iLimiteC = dr.GetOrdinal(helper.Limite_Seg_Coes_Coment);
                    if (!dr.IsDBNull(iLimiteC)) entity.Limite_Seg_Coes_Coment = Convert.ToString(dr.GetValue(iLimiteC));

                    int iFactorLimitanteCalc = dr.GetOrdinal(helper.Factor_Limitante_Calc);
                    if (!dr.IsDBNull(iFactorLimitanteCalc)) entity.Factor_Limitante_Calc = Convert.ToString(dr.GetValue(iFactorLimitanteCalc));

                    int iFactorLimitanteCalcC = dr.GetOrdinal(helper.Factor_Limitante_Calc_Coment);
                    if (!dr.IsDBNull(iFactorLimitanteCalcC)) entity.Factor_Limitante_Calc_Coment = Convert.ToString(dr.GetValue(iFactorLimitanteCalcC));

                    int iFactorLimitanteFinal = dr.GetOrdinal(helper.Factor_Limitante_Final);
                    if (!dr.IsDBNull(iFactorLimitanteFinal)) entity.Factor_Limitante_Final = Convert.ToString(dr.GetValue(iFactorLimitanteFinal));

                    int iFactorLimitanteFinalC = dr.GetOrdinal(helper.Factor_Limitante_Final_Coment);
                    if (!dr.IsDBNull(iFactorLimitanteFinalC)) entity.Factor_Limitante_Final_Coment = Convert.ToString(dr.GetValue(iFactorLimitanteFinalC));

                    int iObservaciones = dr.GetOrdinal(helper.Observaciones_Reporte);
                    if (!dr.IsDBNull(iObservaciones)) entity.Observaciones = Convert.ToString(dr.GetValue(iObservaciones));

                    int iFechamodificacionstr = dr.GetOrdinal(helper.Fecha_Modificacion);
                    if (!dr.IsDBNull(iFechamodificacionstr)) entity.Fecha_Modificacion = Convert.ToString(dr.GetValue(iFechamodificacionstr));

                    int iUsuarioAuditoria = dr.GetOrdinal(helper.Usuario_Auditoria);
                    if (!dr.IsDBNull(iUsuarioAuditoria)) entity.Usuario_Auditoria = Convert.ToString(dr.GetValue(iUsuarioAuditoria));

                    int iMotivo = dr.GetOrdinal(helper.Motivo);
                    if (!dr.IsDBNull(iMotivo)) entity.Motivo = Convert.ToString(dr.GetValue(iMotivo));

                    int iCapacidadABancoComent = dr.GetOrdinal(helper.Capacidad_A_Banco_Coment);
                    if (!dr.IsDBNull(iCapacidadABancoComent)) entity.CapacidadABancoComent = Convert.ToString(dr.GetValue(iCapacidadABancoComent));

                    int iCapacidadMvarBancoComent = dr.GetOrdinal(helper.Capacidad_Mvar_Banco_Coment);
                    if (!dr.IsDBNull(iCapacidadMvarBancoComent)) entity.CapacidadMvarBancoComent = Convert.ToString(dr.GetValue(iCapacidadMvarBancoComent));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<EprEquipoReactorReporteDTO> ListReactorEvaluacionReporte(string equicodi, string codigo, string emprcodi, string equiestado,
            string idsubestacion, string idarea, string idAreaReporte)
        {
            List<EprEquipoReactorReporteDTO> entitys = new List<EprEquipoReactorReporteDTO>();

            var consulta = string.Format(helper.ListReactorEvaluacionReporte, new string[] { equicodi, codigo, emprcodi, equiestado, idsubestacion, idarea, idAreaReporte });

            DbCommand command = dbProvider.GetSqlStringCommand(consulta);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EprEquipoReactorReporteDTO entity = new EprEquipoReactorReporteDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Codigo_Id);
                    if (!dr.IsDBNull(iEquicodi)) entity.Codigo_Id = Convert.ToString(dr.GetValue(iEquicodi));

                    int iCodigo = dr.GetOrdinal(helper.Codigo);
                    if (!dr.IsDBNull(iCodigo)) entity.Codigo = Convert.ToString(dr.GetValue(iCodigo));

                    int iEmprNomb = dr.GetOrdinal(helper.Ubicacion);
                    if (!dr.IsDBNull(iEmprNomb)) entity.Ubicacion = Convert.ToString(dr.GetValue(iEmprNomb));

                    int iEquiAbrev = dr.GetOrdinal(helper.Empresa);
                    if (!dr.IsDBNull(iEquiAbrev)) entity.Empresa = Convert.ToString(dr.GetValue(iEquiAbrev));

                    int iArea = dr.GetOrdinal(helper.Area);
                    if (!dr.IsDBNull(iArea)) entity.Area = Convert.ToString(dr.GetValue(iArea));                 

                    int iTension = dr.GetOrdinal(helper.Tension_Reporte);
                    if (!dr.IsDBNull(iTension)) entity.Tension = Convert.ToString(dr.GetValue(iTension));

                    int iTensionC = dr.GetOrdinal(helper.Tension_Coment);
                    if (!dr.IsDBNull(iTensionC)) entity.Tension_Coment = Convert.ToString(dr.GetValue(iTensionC));

                    int iCapacidadA = dr.GetOrdinal(helper.Capacidad_A);
                    if (!dr.IsDBNull(iCapacidadA)) entity.Capacidad_A = Convert.ToString(dr.GetValue(iCapacidadA));

                    int iCapacidadAC = dr.GetOrdinal(helper.Capacidad_A_Coment);
                    if (!dr.IsDBNull(iCapacidadAC)) entity.Capacidad_A_Coment = Convert.ToString(dr.GetValue(iCapacidadAC));

                    int iCapacidadMva = dr.GetOrdinal(helper.Capacidad_Mvar);
                    if (!dr.IsDBNull(iCapacidadMva)) entity.Capacidad_Mvar = Convert.ToString(dr.GetValue(iCapacidadMva));

                    int iCapacidadMvaC = dr.GetOrdinal(helper.Capacidad_Mvar_Coment);
                    if (!dr.IsDBNull(iCapacidadMvaC)) entity.Capacidad_Mvar_Coment = Convert.ToString(dr.GetValue(iCapacidadMvaC));

                    int iIdCelda1 = dr.GetOrdinal(helper.Id_Celda_1);
                    if (!dr.IsDBNull(iIdCelda1)) entity.Id_Celda_1 = Convert.ToString(dr.GetValue(iIdCelda1));

                    int iNombreCelda1 = dr.GetOrdinal(helper.Nombre_Celda_1);
                    if (!dr.IsDBNull(iNombreCelda1)) entity.Nombre_Celda_1 = Convert.ToString(dr.GetValue(iNombreCelda1));

                    int iUbicacionCelda1 = dr.GetOrdinal(helper.Ubicacion_Celda_1);
                    if (!dr.IsDBNull(iUbicacionCelda1)) entity.Ubicacion_Celda_1 = Convert.ToString(dr.GetValue(iUbicacionCelda1));

                    int iPosicionNucleo1 = dr.GetOrdinal(helper.Posicion_Nucleo_Tc_Celda_1);
                    if (!dr.IsDBNull(iPosicionNucleo1)) entity.Posicion_Nucleo_Tc_Celda_1 = Convert.ToString(dr.GetValue(iPosicionNucleo1));

                    int iPickupCelda1 = dr.GetOrdinal(helper.Pick_Up_Celda_1);
                    if (!dr.IsDBNull(iPickupCelda1)) entity.Pick_Up_Celda_1 = Convert.ToString(dr.GetValue(iPickupCelda1));

                    int iIdCelda2 = dr.GetOrdinal(helper.Id_Celda_2);
                    if (!dr.IsDBNull(iIdCelda2)) entity.Id_Celda_2 = Convert.ToString(dr.GetValue(iIdCelda2));

                    int iNombreCelda2 = dr.GetOrdinal(helper.Nombre_Celda_2);
                    if (!dr.IsDBNull(iNombreCelda2)) entity.Nombre_Celda_2 = Convert.ToString(dr.GetValue(iNombreCelda2));

                    int iUbicacionCelda2 = dr.GetOrdinal(helper.Ubicacion_Celda_2);
                    if (!dr.IsDBNull(iUbicacionCelda2)) entity.Ubicacion_Celda_2 = Convert.ToString(dr.GetValue(iUbicacionCelda2));

                    int iPosicionNucleo2 = dr.GetOrdinal(helper.Posicion_Nucleo_Tc_Celda_2);
                    if (!dr.IsDBNull(iPosicionNucleo2)) entity.Posicion_Nucleo_Tc_Celda_2 = Convert.ToString(dr.GetValue(iPosicionNucleo2));

                    int iPickupCelda2 = dr.GetOrdinal(helper.Pick_Up_Celda_2);
                    if (!dr.IsDBNull(iPickupCelda2)) entity.Pick_Up_Celda_2 = Convert.ToString(dr.GetValue(iPickupCelda2));                   

                    int iCapacTrans = dr.GetOrdinal(helper.Capacidad_Transmision_A);
                    if (!dr.IsDBNull(iCapacTrans)) entity.Capacidad_Transmision_A = Convert.ToString(dr.GetValue(iCapacTrans));

                    int iCapacTransC = dr.GetOrdinal(helper.Capacidad_Transmision_A_Coment);
                    if (!dr.IsDBNull(iCapacTransC)) entity.Capacidad_Transmision_A_Coment = Convert.ToString(dr.GetValue(iCapacTransC));

                    int iCapacTransMva = dr.GetOrdinal(helper.Capacidad_Transmision_Mvar);
                    if (!dr.IsDBNull(iCapacTransMva)) entity.Capacidad_Transmision_Mvar = Convert.ToString(dr.GetValue(iCapacTransMva));

                    int iCapacTransMvaC = dr.GetOrdinal(helper.Capacidad_Transmision_Mvar_Coment);
                    if (!dr.IsDBNull(iCapacTransMvaC)) entity.Capacidad_Transmision_Mvar_Coment = Convert.ToString(dr.GetValue(iCapacTransMvaC));                  

                    int iFactorLimitanteCalc = dr.GetOrdinal(helper.Factor_Limitante_Calc);
                    if (!dr.IsDBNull(iFactorLimitanteCalc)) entity.Factor_Limitante_Calc = Convert.ToString(dr.GetValue(iFactorLimitanteCalc));

                    int iFactorLimitanteCalcC = dr.GetOrdinal(helper.Factor_Limitante_Calc_Coment);
                    if (!dr.IsDBNull(iFactorLimitanteCalcC)) entity.Factor_Limitante_Calc_Coment = Convert.ToString(dr.GetValue(iFactorLimitanteCalcC));

                    int iFactorLimitanteFinal = dr.GetOrdinal(helper.Factor_Limitante_Final);
                    if (!dr.IsDBNull(iFactorLimitanteFinal)) entity.Factor_Limitante_Final = Convert.ToString(dr.GetValue(iFactorLimitanteFinal));

                    int iFactorLimitanteFinalC = dr.GetOrdinal(helper.Factor_Limitante_Final_Coment);
                    if (!dr.IsDBNull(iFactorLimitanteFinalC)) entity.Factor_Limitante_Final_Coment = Convert.ToString(dr.GetValue(iFactorLimitanteFinalC));

                    int iObservaciones = dr.GetOrdinal(helper.Observaciones_Reporte);
                    if (!dr.IsDBNull(iObservaciones)) entity.Observaciones = Convert.ToString(dr.GetValue(iObservaciones));

                    int iFechamodificacionstr = dr.GetOrdinal(helper.Fecha_Modificacion);
                    if (!dr.IsDBNull(iFechamodificacionstr)) entity.Fecha_Modificacion = Convert.ToString(dr.GetValue(iFechamodificacionstr));

                    int iUsuarioAuditoria = dr.GetOrdinal(helper.Usuario_Auditoria);
                    if (!dr.IsDBNull(iUsuarioAuditoria)) entity.Usuario_Auditoria = Convert.ToString(dr.GetValue(iUsuarioAuditoria));

                    int iMotivo = dr.GetOrdinal(helper.Motivo);
                    if (!dr.IsDBNull(iMotivo)) entity.Motivo = Convert.ToString(dr.GetValue(iMotivo));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<EprEquipoCeldaAcoplamientoReporteDTO> ListCeldaAcoplamientoReporte(string equicodi, string codigo, string emprcodi, string equiestado,
           string idsubestacion, string idarea, string idAreaReporte, string tension)
        {
            List<EprEquipoCeldaAcoplamientoReporteDTO> entitys = new List<EprEquipoCeldaAcoplamientoReporteDTO>();

            var consulta = string.Format(helper.ListCeldasAcoplamientoReporte, new string[] { equicodi, codigo, emprcodi, equiestado, idsubestacion, idarea, idAreaReporte, tension });

           
            DbCommand command = dbProvider.GetSqlStringCommand(consulta);           

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EprEquipoCeldaAcoplamientoReporteDTO entity = new EprEquipoCeldaAcoplamientoReporteDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Codigo_Id);
                    if (!dr.IsDBNull(iEquicodi)) entity.Codigo_Id = Convert.ToString(dr.GetValue(iEquicodi));

                    int iCodigo = dr.GetOrdinal(helper.Nombre);
                    if (!dr.IsDBNull(iCodigo)) entity.Nombre = Convert.ToString(dr.GetValue(iCodigo));

                    int iEmprNomb = dr.GetOrdinal(helper.Ubicacion);
                    if (!dr.IsDBNull(iEmprNomb)) entity.Ubicacion = Convert.ToString(dr.GetValue(iEmprNomb));                  

                    int iArea = dr.GetOrdinal(helper.Area);
                    if (!dr.IsDBNull(iArea)) entity.Area = Convert.ToString(dr.GetValue(iArea));

                    int iPosicionNucleo1 = dr.GetOrdinal(helper.Posicion_Nucleo_Tc);
                    if (!dr.IsDBNull(iPosicionNucleo1)) entity.Posicion_Nucleo_Tc = Convert.ToString(dr.GetValue(iPosicionNucleo1));

                    int iPickupCelda1 = dr.GetOrdinal(helper.Pick_Up);
                    if (!dr.IsDBNull(iPickupCelda1)) entity.Pick_Up = Convert.ToString(dr.GetValue(iPickupCelda1));

                    int iCapacidadAC = dr.GetOrdinal(helper.Codigo_Id_Interruptor);
                    if (!dr.IsDBNull(iCapacidadAC)) entity.Codigo_Id_Interruptor = Convert.ToString(dr.GetValue(iCapacidadAC));

                    int iCapacidadMva = dr.GetOrdinal(helper.Nombre_Interruptor);
                    if (!dr.IsDBNull(iCapacidadMva)) entity.Nombre_Interruptor = Convert.ToString(dr.GetValue(iCapacidadMva));

                    int iCapacidadMvaC = dr.GetOrdinal(helper.Ubicacion_Interruptor);
                    if (!dr.IsDBNull(iCapacidadMvaC)) entity.Ubicacion_Interruptor = Convert.ToString(dr.GetValue(iCapacidadMvaC));

                    int iIdCelda1 = dr.GetOrdinal(helper.Empresa_Interruptor);
                    if (!dr.IsDBNull(iIdCelda1)) entity.Empresa_Interruptor = Convert.ToString(dr.GetValue(iIdCelda1));

                    int iNombreCelda1 = dr.GetOrdinal(helper.Tension_Interruptor);
                    if (!dr.IsDBNull(iNombreCelda1)) entity.Tension_Interruptor = Convert.ToString(dr.GetValue(iNombreCelda1));

                    int iUbicacionCelda1 = dr.GetOrdinal(helper.Capacidad_A_Interruptor);
                    if (!dr.IsDBNull(iUbicacionCelda1)) entity.Capacidad_A_Interruptor = Convert.ToString(dr.GetValue(iUbicacionCelda1));                   

                    int iIdCelda2 = dr.GetOrdinal(helper.Capacidad_A_Interruptor_Coment);
                    if (!dr.IsDBNull(iIdCelda2)) entity.Capacidad_A_Interruptor_Coment = Convert.ToString(dr.GetValue(iIdCelda2));

                    int iNombreCelda2 = dr.GetOrdinal(helper.Capacidad_Mva_Interruptor);
                    if (!dr.IsDBNull(iNombreCelda2)) entity.Capacidad_Mva_Interruptor = Convert.ToString(dr.GetValue(iNombreCelda2));

                    int iUbicacionCelda2 = dr.GetOrdinal(helper.Capacidad_Mva_Interruptor_Coment);
                    if (!dr.IsDBNull(iUbicacionCelda2)) entity.Capacidad_Mva_Interruptor_Coment = Convert.ToString(dr.GetValue(iUbicacionCelda2));
                 
                    int iCapacTrans = dr.GetOrdinal(helper.Capacidad_Transmision_A);
                    if (!dr.IsDBNull(iCapacTrans)) entity.Capacidad_Transmision_A = Convert.ToString(dr.GetValue(iCapacTrans));

                    int iCapacTransC = dr.GetOrdinal(helper.Capacidad_Transmision_A_Coment);
                    if (!dr.IsDBNull(iCapacTransC)) entity.Capacidad_Transmision_A_Coment = Convert.ToString(dr.GetValue(iCapacTransC));

                    int iCapacTransMva = dr.GetOrdinal(helper.Capacidad_Transmision_Mva);
                    if (!dr.IsDBNull(iCapacTransMva)) entity.Capacidad_Transmision_Mva = Convert.ToString(dr.GetValue(iCapacTransMva));

                    int iCapacTransMvaC = dr.GetOrdinal(helper.Capacidad_Transmision_Mva_Coment);
                    if (!dr.IsDBNull(iCapacTransMvaC)) entity.Capacidad_Transmision_Mva_Coment = Convert.ToString(dr.GetValue(iCapacTransMvaC));

                    int iFactorLimitanteCalc = dr.GetOrdinal(helper.Factor_Limitante_Calc);
                    if (!dr.IsDBNull(iFactorLimitanteCalc)) entity.Factor_Limitante_Calc = Convert.ToString(dr.GetValue(iFactorLimitanteCalc));

                    int iFactorLimitanteCalcC = dr.GetOrdinal(helper.Factor_Limitante_Calc_Coment);
                    if (!dr.IsDBNull(iFactorLimitanteCalcC)) entity.Factor_Limitante_Calc_Coment = Convert.ToString(dr.GetValue(iFactorLimitanteCalcC));

                    int iFactorLimitanteFinal = dr.GetOrdinal(helper.Factor_Limitante_Final);
                    if (!dr.IsDBNull(iFactorLimitanteFinal)) entity.Factor_Limitante_Final = Convert.ToString(dr.GetValue(iFactorLimitanteFinal));

                    int iFactorLimitanteFinalC = dr.GetOrdinal(helper.Factor_Limitante_Final_Coment);
                    if (!dr.IsDBNull(iFactorLimitanteFinalC)) entity.Factor_Limitante_Final_Coment = Convert.ToString(dr.GetValue(iFactorLimitanteFinalC));

                    int iObservaciones = dr.GetOrdinal(helper.Observaciones_Reporte);
                    if (!dr.IsDBNull(iObservaciones)) entity.Observaciones = Convert.ToString(dr.GetValue(iObservaciones));

                    int iFechamodificacionstr = dr.GetOrdinal(helper.Fecha_Modificacion);
                    if (!dr.IsDBNull(iFechamodificacionstr)) entity.Fecha_Modificacion = Convert.ToString(dr.GetValue(iFechamodificacionstr));

                    int iUsuarioAuditoria = dr.GetOrdinal(helper.Usuario_Auditoria);
                    if (!dr.IsDBNull(iUsuarioAuditoria)) entity.Usuario_Auditoria = Convert.ToString(dr.GetValue(iUsuarioAuditoria));

                    int iMotivo = dr.GetOrdinal(helper.Motivo);
                    if (!dr.IsDBNull(iMotivo)) entity.Motivo = Convert.ToString(dr.GetValue(iMotivo));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<EprEquipoTransformadoresReporteDTO> ListTransformadoresEvaluacionReporte(string equicodi, string codigo, string emprcodi, string equiestado,
           string tipo, string idsubestacion, string idarea, string idAreaReporte, string tension)
        {
            List<EprEquipoTransformadoresReporteDTO> entitys = new List<EprEquipoTransformadoresReporteDTO>();

            var consulta = string.Format(helper.ListTransformadoresReporte, new string[] { equicodi, codigo, emprcodi, equiestado, tipo, idsubestacion, idarea, idAreaReporte, tension });

            DbCommand command = dbProvider.GetSqlStringCommand(consulta);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EprEquipoTransformadoresReporteDTO entity = new EprEquipoTransformadoresReporteDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Codigo_Id);
                    if (!dr.IsDBNull(iEquicodi)) entity.Codigo_Id = Convert.ToString(dr.GetValue(iEquicodi));

                    int iCodigo = dr.GetOrdinal(helper.Codigo);
                    if (!dr.IsDBNull(iCodigo)) entity.Codigo = Convert.ToString(dr.GetValue(iCodigo));

                    int iEmprNomb = dr.GetOrdinal(helper.Ubicacion);
                    if (!dr.IsDBNull(iEmprNomb)) entity.Ubicacion = Convert.ToString(dr.GetValue(iEmprNomb));

                    int iEquiAbrev = dr.GetOrdinal(helper.Empresa);
                    if (!dr.IsDBNull(iEquiAbrev)) entity.Empresa = Convert.ToString(dr.GetValue(iEquiAbrev));

                    int iArea = dr.GetOrdinal(helper.Area);
                    if (!dr.IsDBNull(iArea)) entity.Area = Convert.ToString(dr.GetValue(iArea));

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    #region Celda 1

                    int iD1_Id_Celda = dr.GetOrdinal(helper.D1_Id_Celda);
                    if (!dr.IsDBNull(iD1_Id_Celda)) entity.D1_Id_Celda = Convert.ToString(dr.GetValue(iD1_Id_Celda));

                    int iD1_Codigo_Celda = dr.GetOrdinal(helper.D1_Codigo_Celda);
                    if (!dr.IsDBNull(iD1_Codigo_Celda)) entity.D1_Codigo_Celda = Convert.ToString(dr.GetValue(iD1_Codigo_Celda));

                    int iD1_Ubicacion_Celda = dr.GetOrdinal(helper.D1_Ubicacion_Celda);
                    if (!dr.IsDBNull(iD1_Ubicacion_Celda)) entity.D1_Ubicacion_Celda = Convert.ToString(dr.GetValue(iD1_Ubicacion_Celda));

                    int iD1_Tension = dr.GetOrdinal(helper.D1_Tension);
                    if (!dr.IsDBNull(iD1_Tension)) entity.D1_Tension = Convert.ToString(dr.GetValue(iD1_Tension));

                    int iD1_Tension_Coment = dr.GetOrdinal(helper.D1_Tension_Coment);
                    if (!dr.IsDBNull(iD1_Tension_Coment)) entity.D1_Tension_Coment = Convert.ToString(dr.GetValue(iD1_Tension_Coment));

                    int iD1_Capacidad_Onan_Mva = dr.GetOrdinal(helper.D1_Capacidad_Onan_Mva);
                    if (!dr.IsDBNull(iD1_Capacidad_Onan_Mva)) entity.D1_Capacidad_Onan_Mva = Convert.ToString(dr.GetValue(iD1_Capacidad_Onan_Mva));

                    int iD1_Capacidad_Onan_Mva_Coment = dr.GetOrdinal(helper.D1_Capacidad_Onan_Mva_Coment);
                    if (!dr.IsDBNull(iD1_Capacidad_Onan_Mva_Coment)) entity.D1_Capacidad_Onan_Mva_Coment = Convert.ToString(dr.GetValue(iD1_Capacidad_Onan_Mva_Coment));

                    int iD1_Capacidad_Onaf_Mva = dr.GetOrdinal(helper.D1_Capacidad_Onaf_Mva);
                    if (!dr.IsDBNull(iD1_Capacidad_Onaf_Mva)) entity.D1_Capacidad_Onaf_Mva = Convert.ToString(dr.GetValue(iD1_Capacidad_Onaf_Mva));

                    int iD1_Capacidad_Onaf_Mva_Coment = dr.GetOrdinal(helper.D1_Capacidad_Onaf_Mva_Coment);
                    if (!dr.IsDBNull(iD1_Capacidad_Onaf_Mva_Coment)) entity.D1_Capacidad_Onaf_Mva_Coment = Convert.ToString(dr.GetValue(iD1_Capacidad_Onaf_Mva_Coment));

                    int iD1_Capacidad_Mva = dr.GetOrdinal(helper.D1_Capacidad_Mva);
                    if (!dr.IsDBNull(iD1_Capacidad_Mva)) entity.D1_Capacidad_Mva = Convert.ToString(dr.GetValue(iD1_Capacidad_Mva));

                    int iD1_Capacidad_Mva_Coment = dr.GetOrdinal(helper.D1_Capacidad_Mva_Coment);
                    if (!dr.IsDBNull(iD1_Capacidad_Mva_Coment)) entity.D1_Capacidad_Mva_Coment = Convert.ToString(dr.GetValue(iD1_Capacidad_Mva_Coment));

                    int iD1_Capacidad_A = dr.GetOrdinal(helper.D1_Capacidad_A);
                    if (!dr.IsDBNull(iD1_Capacidad_A)) entity.D1_Capacidad_A = Convert.ToString(dr.GetValue(iD1_Capacidad_A));

                    int iD1_Capacidad_A_Coment = dr.GetOrdinal(helper.D1_Capacidad_A_Coment);
                    if (!dr.IsDBNull(iD1_Capacidad_A_Coment)) entity.D1_Capacidad_A_Coment = Convert.ToString(dr.GetValue(iD1_Capacidad_A_Coment));

                    int iD1_Posicion_Nucleo_Tc = dr.GetOrdinal(helper.D1_Posicion_Nucleo_Tc);
                    if (!dr.IsDBNull(iD1_Posicion_Nucleo_Tc)) entity.D1_Posicion_Nucleo_Tc = Convert.ToString(dr.GetValue(iD1_Posicion_Nucleo_Tc));

                    int iD1_Pick_Up = dr.GetOrdinal(helper.D1_Pick_Up);
                    if (!dr.IsDBNull(iD1_Pick_Up)) entity.D1_Pick_Up = Convert.ToString(dr.GetValue(iD1_Pick_Up));

                    int iD1_Factor_Limitante_Calc = dr.GetOrdinal(helper.D1_Factor_Limitante_Calc);
                    if (!dr.IsDBNull(iD1_Factor_Limitante_Calc)) entity.D1_Factor_Limitante_Calc = Convert.ToString(dr.GetValue(iD1_Factor_Limitante_Calc));

                    int iD1_Factor_Limitante_Calc_Coment = dr.GetOrdinal(helper.D1_Factor_Limitante_Calc_Coment);
                    if (!dr.IsDBNull(iD1_Factor_Limitante_Calc_Coment)) entity.D1_Factor_Limitante_Calc_Coment = Convert.ToString(dr.GetValue(iD1_Factor_Limitante_Calc_Coment));

                    int iD1_Factor_Limitante_Final = dr.GetOrdinal(helper.D1_Factor_Limitante_Final);
                    if (!dr.IsDBNull(iD1_Factor_Limitante_Final)) entity.D1_Factor_Limitante_Final = Convert.ToString(dr.GetValue(iD1_Factor_Limitante_Final));

                    int iD1_Factor_Limitante_Final_Coment = dr.GetOrdinal(helper.D1_Factor_Limitante_Final_Coment);
                    if (!dr.IsDBNull(iD1_Factor_Limitante_Final_Coment)) entity.D1_Factor_Limitante_Final_Coment = Convert.ToString(dr.GetValue(iD1_Factor_Limitante_Final_Coment));


                    #endregion

                    #region Celda 2

                    int iD2_Id_Celda = dr.GetOrdinal(helper.D2_Id_Celda);
                    if (!dr.IsDBNull(iD2_Id_Celda)) entity.D2_Id_Celda = Convert.ToString(dr.GetValue(iD2_Id_Celda));

                    int iD2_Codigo_Celda = dr.GetOrdinal(helper.D2_Codigo_Celda);
                    if (!dr.IsDBNull(iD2_Codigo_Celda)) entity.D2_Codigo_Celda = Convert.ToString(dr.GetValue(iD2_Codigo_Celda));

                    int iD2_Ubicacion_Celda = dr.GetOrdinal(helper.D2_Ubicacion_Celda);
                    if (!dr.IsDBNull(iD2_Ubicacion_Celda)) entity.D2_Ubicacion_Celda = Convert.ToString(dr.GetValue(iD2_Ubicacion_Celda));

                    int iD2_Tension = dr.GetOrdinal(helper.D2_Tension);
                    if (!dr.IsDBNull(iD2_Tension)) entity.D2_Tension = Convert.ToString(dr.GetValue(iD2_Tension));

                    int iD2_Tension_Coment = dr.GetOrdinal(helper.D2_Tension_Coment);
                    if (!dr.IsDBNull(iD2_Tension_Coment)) entity.D2_Tension_Coment = Convert.ToString(dr.GetValue(iD2_Tension_Coment));

                    int iD2_Capacidad_Onan_Mva = dr.GetOrdinal(helper.D2_Capacidad_Onan_Mva);
                    if (!dr.IsDBNull(iD2_Capacidad_Onan_Mva)) entity.D2_Capacidad_Onan_Mva = Convert.ToString(dr.GetValue(iD2_Capacidad_Onan_Mva));

                    int iD2_Capacidad_Onan_Mva_Coment = dr.GetOrdinal(helper.D2_Capacidad_Onan_Mva_Coment);
                    if (!dr.IsDBNull(iD2_Capacidad_Onan_Mva_Coment)) entity.D2_Capacidad_Onan_Mva_Coment = Convert.ToString(dr.GetValue(iD2_Capacidad_Onan_Mva_Coment));

                    int iD2_Capacidad_Onaf_Mva = dr.GetOrdinal(helper.D2_Capacidad_Onaf_Mva);
                    if (!dr.IsDBNull(iD2_Capacidad_Onaf_Mva)) entity.D2_Capacidad_Onaf_Mva = Convert.ToString(dr.GetValue(iD2_Capacidad_Onaf_Mva));

                    int iD2_Capacidad_Onaf_Mva_Coment = dr.GetOrdinal(helper.D2_Capacidad_Onaf_Mva_Coment);
                    if (!dr.IsDBNull(iD2_Capacidad_Onaf_Mva_Coment)) entity.D2_Capacidad_Onaf_Mva_Coment = Convert.ToString(dr.GetValue(iD2_Capacidad_Onaf_Mva_Coment));

                    int iD2_Capacidad_Mva = dr.GetOrdinal(helper.D2_Capacidad_Mva);
                    if (!dr.IsDBNull(iD2_Capacidad_Mva)) entity.D2_Capacidad_Mva = Convert.ToString(dr.GetValue(iD2_Capacidad_Mva));

                    int iD2_Capacidad_Mva_Coment = dr.GetOrdinal(helper.D2_Capacidad_Mva_Coment);
                    if (!dr.IsDBNull(iD2_Capacidad_Mva_Coment)) entity.D2_Capacidad_Mva_Coment = Convert.ToString(dr.GetValue(iD2_Capacidad_Mva_Coment));

                    int iD2_Capacidad_A = dr.GetOrdinal(helper.D2_Capacidad_A);
                    if (!dr.IsDBNull(iD2_Capacidad_A)) entity.D2_Capacidad_A = Convert.ToString(dr.GetValue(iD2_Capacidad_A));

                    int iD2_Capacidad_A_Coment = dr.GetOrdinal(helper.D2_Capacidad_A_Coment);
                    if (!dr.IsDBNull(iD2_Capacidad_A_Coment)) entity.D2_Capacidad_A_Coment = Convert.ToString(dr.GetValue(iD2_Capacidad_A_Coment));

                    int iD2_Posicion_Nucleo_Tc = dr.GetOrdinal(helper.D2_Posicion_Nucleo_Tc);
                    if (!dr.IsDBNull(iD2_Posicion_Nucleo_Tc)) entity.D2_Posicion_Nucleo_Tc = Convert.ToString(dr.GetValue(iD2_Posicion_Nucleo_Tc));

                    int iD2_Pick_Up = dr.GetOrdinal(helper.D2_Pick_Up);
                    if (!dr.IsDBNull(iD2_Pick_Up)) entity.D2_Pick_Up = Convert.ToString(dr.GetValue(iD2_Pick_Up));

                    int iD2_Factor_Limitante_Calc = dr.GetOrdinal(helper.D2_Factor_Limitante_Calc);
                    if (!dr.IsDBNull(iD2_Factor_Limitante_Calc)) entity.D2_Factor_Limitante_Calc = Convert.ToString(dr.GetValue(iD2_Factor_Limitante_Calc));

                    int iD2_Factor_Limitante_Calc_Coment = dr.GetOrdinal(helper.D2_Factor_Limitante_Calc_Coment);
                    if (!dr.IsDBNull(iD2_Factor_Limitante_Calc_Coment)) entity.D2_Factor_Limitante_Calc_Coment = Convert.ToString(dr.GetValue(iD2_Factor_Limitante_Calc_Coment));

                    int iD2_Factor_Limitante_Final = dr.GetOrdinal(helper.D2_Factor_Limitante_Final);
                    if (!dr.IsDBNull(iD2_Factor_Limitante_Final)) entity.D2_Factor_Limitante_Final = Convert.ToString(dr.GetValue(iD2_Factor_Limitante_Final));

                    int iD2_Factor_Limitante_Final_Coment = dr.GetOrdinal(helper.D2_Factor_Limitante_Final_Coment);
                    if (!dr.IsDBNull(iD2_Factor_Limitante_Final_Coment)) entity.D2_Factor_Limitante_Final_Coment = Convert.ToString(dr.GetValue(iD2_Factor_Limitante_Final_Coment));


                    #endregion

                    #region Celda 3

                    int iD3_Id_Celda = dr.GetOrdinal(helper.D3_Id_Celda);
                    if (!dr.IsDBNull(iD3_Id_Celda)) entity.D3_Id_Celda = Convert.ToString(dr.GetValue(iD3_Id_Celda));

                    int iD3_Codigo_Celda = dr.GetOrdinal(helper.D3_Codigo_Celda);
                    if (!dr.IsDBNull(iD3_Codigo_Celda)) entity.D3_Codigo_Celda = Convert.ToString(dr.GetValue(iD3_Codigo_Celda));

                    int iD3_Ubicacion_Celda = dr.GetOrdinal(helper.D3_Ubicacion_Celda);
                    if (!dr.IsDBNull(iD3_Ubicacion_Celda)) entity.D3_Ubicacion_Celda = Convert.ToString(dr.GetValue(iD3_Ubicacion_Celda));

                    int iD3_Tension = dr.GetOrdinal(helper.D3_Tension);
                    if (!dr.IsDBNull(iD3_Tension)) entity.D3_Tension = Convert.ToString(dr.GetValue(iD3_Tension));

                    int iD3_Tension_Coment = dr.GetOrdinal(helper.D3_Tension_Coment);
                    if (!dr.IsDBNull(iD3_Tension_Coment)) entity.D3_Tension_Coment = Convert.ToString(dr.GetValue(iD3_Tension_Coment));

                    int iD3_Capacidad_Onan_Mva = dr.GetOrdinal(helper.D3_Capacidad_Onan_Mva);
                    if (!dr.IsDBNull(iD3_Capacidad_Onan_Mva)) entity.D3_Capacidad_Onan_Mva = Convert.ToString(dr.GetValue(iD3_Capacidad_Onan_Mva));

                    int iD3_Capacidad_Onan_Mva_Coment = dr.GetOrdinal(helper.D3_Capacidad_Onan_Mva_Coment);
                    if (!dr.IsDBNull(iD3_Capacidad_Onan_Mva_Coment)) entity.D3_Capacidad_Onan_Mva_Coment = Convert.ToString(dr.GetValue(iD3_Capacidad_Onan_Mva_Coment));

                    int iD3_Capacidad_Onaf_Mva = dr.GetOrdinal(helper.D3_Capacidad_Onaf_Mva);
                    if (!dr.IsDBNull(iD3_Capacidad_Onaf_Mva)) entity.D3_Capacidad_Onaf_Mva = Convert.ToString(dr.GetValue(iD3_Capacidad_Onaf_Mva));

                    int iD3_Capacidad_Onaf_Mva_Coment = dr.GetOrdinal(helper.D3_Capacidad_Onaf_Mva_Coment);
                    if (!dr.IsDBNull(iD3_Capacidad_Onaf_Mva_Coment)) entity.D3_Capacidad_Onaf_Mva_Coment = Convert.ToString(dr.GetValue(iD3_Capacidad_Onaf_Mva_Coment));

                    int iD3_Capacidad_Mva = dr.GetOrdinal(helper.D3_Capacidad_Mva);
                    if (!dr.IsDBNull(iD3_Capacidad_Mva)) entity.D3_Capacidad_Mva = Convert.ToString(dr.GetValue(iD3_Capacidad_Mva));

                    int iD3_Capacidad_Mva_Coment = dr.GetOrdinal(helper.D3_Capacidad_Mva_Coment);
                    if (!dr.IsDBNull(iD3_Capacidad_Mva_Coment)) entity.D3_Capacidad_Mva_Coment = Convert.ToString(dr.GetValue(iD3_Capacidad_Mva_Coment));

                    int iD3_Capacidad_A = dr.GetOrdinal(helper.D3_Capacidad_A);
                    if (!dr.IsDBNull(iD3_Capacidad_A)) entity.D3_Capacidad_A = Convert.ToString(dr.GetValue(iD3_Capacidad_A));

                    int iD3_Capacidad_A_Coment = dr.GetOrdinal(helper.D3_Capacidad_A_Coment);
                    if (!dr.IsDBNull(iD3_Capacidad_A_Coment)) entity.D3_Capacidad_A_Coment = Convert.ToString(dr.GetValue(iD3_Capacidad_A_Coment));

                    int iD3_Posicion_Nucleo_Tc = dr.GetOrdinal(helper.D3_Posicion_Nucleo_Tc);
                    if (!dr.IsDBNull(iD3_Posicion_Nucleo_Tc)) entity.D3_Posicion_Nucleo_Tc = Convert.ToString(dr.GetValue(iD3_Posicion_Nucleo_Tc));

                    int iD3_Pick_Up = dr.GetOrdinal(helper.D3_Pick_Up);
                    if (!dr.IsDBNull(iD3_Pick_Up)) entity.D3_Pick_Up = Convert.ToString(dr.GetValue(iD3_Pick_Up));

                    int iD3_Factor_Limitante_Calc = dr.GetOrdinal(helper.D3_Factor_Limitante_Calc);
                    if (!dr.IsDBNull(iD3_Factor_Limitante_Calc)) entity.D3_Factor_Limitante_Calc = Convert.ToString(dr.GetValue(iD3_Factor_Limitante_Calc));

                    int iD3_Factor_Limitante_Calc_Coment = dr.GetOrdinal(helper.D3_Factor_Limitante_Calc_Coment);
                    if (!dr.IsDBNull(iD3_Factor_Limitante_Calc_Coment)) entity.D3_Factor_Limitante_Calc_Coment = Convert.ToString(dr.GetValue(iD3_Factor_Limitante_Calc_Coment));

                    int iD3_Factor_Limitante_Final = dr.GetOrdinal(helper.D3_Factor_Limitante_Final);
                    if (!dr.IsDBNull(iD3_Factor_Limitante_Final)) entity.D3_Factor_Limitante_Final = Convert.ToString(dr.GetValue(iD3_Factor_Limitante_Final));

                    int iD3_Factor_Limitante_Final_Coment = dr.GetOrdinal(helper.D3_Factor_Limitante_Final_Coment);
                    if (!dr.IsDBNull(iD3_Factor_Limitante_Final_Coment)) entity.D3_Factor_Limitante_Final_Coment = Convert.ToString(dr.GetValue(iD3_Factor_Limitante_Final_Coment));


                    #endregion

                    #region Celda 4

                    int iD4_Id_Celda = dr.GetOrdinal(helper.D4_Id_Celda);
                    if (!dr.IsDBNull(iD4_Id_Celda)) entity.D4_Id_Celda = Convert.ToString(dr.GetValue(iD4_Id_Celda));

                    int iD4_Codigo_Celda = dr.GetOrdinal(helper.D4_Codigo_Celda);
                    if (!dr.IsDBNull(iD4_Codigo_Celda)) entity.D4_Codigo_Celda = Convert.ToString(dr.GetValue(iD4_Codigo_Celda));

                    int iD4_Ubicacion_Celda = dr.GetOrdinal(helper.D4_Ubicacion_Celda);
                    if (!dr.IsDBNull(iD4_Ubicacion_Celda)) entity.D4_Ubicacion_Celda = Convert.ToString(dr.GetValue(iD4_Ubicacion_Celda));

                    int iD4_Tension = dr.GetOrdinal(helper.D4_Tension);
                    if (!dr.IsDBNull(iD4_Tension)) entity.D4_Tension = Convert.ToString(dr.GetValue(iD4_Tension));

                    int iD4_Tension_Coment = dr.GetOrdinal(helper.D4_Tension_Coment);
                    if (!dr.IsDBNull(iD4_Tension_Coment)) entity.D4_Tension_Coment = Convert.ToString(dr.GetValue(iD4_Tension_Coment));

                    int iD4_Capacidad_Onan_Mva = dr.GetOrdinal(helper.D4_Capacidad_Onan_Mva);
                    if (!dr.IsDBNull(iD4_Capacidad_Onan_Mva)) entity.D4_Capacidad_Onan_Mva = Convert.ToString(dr.GetValue(iD4_Capacidad_Onan_Mva));

                    int iD4_Capacidad_Onan_Mva_Coment = dr.GetOrdinal(helper.D4_Capacidad_Onan_Mva_Coment);
                    if (!dr.IsDBNull(iD4_Capacidad_Onan_Mva_Coment)) entity.D4_Capacidad_Onan_Mva_Coment = Convert.ToString(dr.GetValue(iD4_Capacidad_Onan_Mva_Coment));

                    int iD4_Capacidad_Onaf_Mva = dr.GetOrdinal(helper.D4_Capacidad_Onaf_Mva);
                    if (!dr.IsDBNull(iD4_Capacidad_Onaf_Mva)) entity.D4_Capacidad_Onaf_Mva = Convert.ToString(dr.GetValue(iD4_Capacidad_Onaf_Mva));

                    int iD4_Capacidad_Onaf_Mva_Coment = dr.GetOrdinal(helper.D4_Capacidad_Onaf_Mva_Coment);
                    if (!dr.IsDBNull(iD4_Capacidad_Onaf_Mva_Coment)) entity.D4_Capacidad_Onaf_Mva_Coment = Convert.ToString(dr.GetValue(iD4_Capacidad_Onaf_Mva_Coment));

                    int iD4_Capacidad_Mva = dr.GetOrdinal(helper.D4_Capacidad_Mva);
                    if (!dr.IsDBNull(iD4_Capacidad_Mva)) entity.D4_Capacidad_Mva = Convert.ToString(dr.GetValue(iD4_Capacidad_Mva));

                    int iD4_Capacidad_Mva_Coment = dr.GetOrdinal(helper.D4_Capacidad_Mva_Coment);
                    if (!dr.IsDBNull(iD4_Capacidad_Mva_Coment)) entity.D4_Capacidad_Mva_Coment = Convert.ToString(dr.GetValue(iD4_Capacidad_Mva_Coment));

                    int iD4_Capacidad_A = dr.GetOrdinal(helper.D4_Capacidad_A);
                    if (!dr.IsDBNull(iD4_Capacidad_A)) entity.D4_Capacidad_A = Convert.ToString(dr.GetValue(iD4_Capacidad_A));

                    int iD4_Capacidad_A_Coment = dr.GetOrdinal(helper.D4_Capacidad_A_Coment);
                    if (!dr.IsDBNull(iD4_Capacidad_A_Coment)) entity.D4_Capacidad_A_Coment = Convert.ToString(dr.GetValue(iD4_Capacidad_A_Coment));

                    int iD4_Posicion_Nucleo_Tc = dr.GetOrdinal(helper.D4_Posicion_Nucleo_Tc);
                    if (!dr.IsDBNull(iD4_Posicion_Nucleo_Tc)) entity.D4_Posicion_Nucleo_Tc = Convert.ToString(dr.GetValue(iD4_Posicion_Nucleo_Tc));

                    int iD4_Pick_Up = dr.GetOrdinal(helper.D4_Pick_Up);
                    if (!dr.IsDBNull(iD4_Pick_Up)) entity.D4_Pick_Up = Convert.ToString(dr.GetValue(iD4_Pick_Up));

                    int iD4_Factor_Limitante_Calc = dr.GetOrdinal(helper.D4_Factor_Limitante_Calc);
                    if (!dr.IsDBNull(iD4_Factor_Limitante_Calc)) entity.D4_Factor_Limitante_Calc = Convert.ToString(dr.GetValue(iD4_Factor_Limitante_Calc));

                    int iD4_Factor_Limitante_Calc_Coment = dr.GetOrdinal(helper.D4_Factor_Limitante_Calc_Coment);
                    if (!dr.IsDBNull(iD4_Factor_Limitante_Calc_Coment)) entity.D4_Factor_Limitante_Calc_Coment = Convert.ToString(dr.GetValue(iD4_Factor_Limitante_Calc_Coment));

                    int iD4_Factor_Limitante_Final = dr.GetOrdinal(helper.D4_Factor_Limitante_Final);
                    if (!dr.IsDBNull(iD4_Factor_Limitante_Final)) entity.D4_Factor_Limitante_Final = Convert.ToString(dr.GetValue(iD4_Factor_Limitante_Final));

                    int iD4_Factor_Limitante_Final_Coment = dr.GetOrdinal(helper.D4_Factor_Limitante_Final_Coment);
                    if (!dr.IsDBNull(iD4_Factor_Limitante_Final_Coment)) entity.D4_Factor_Limitante_Final_Coment = Convert.ToString(dr.GetValue(iD4_Factor_Limitante_Final_Coment));


                    #endregion


                    int iObservaciones = dr.GetOrdinal(helper.Observaciones_Reporte);
                    if (!dr.IsDBNull(iObservaciones)) entity.Observaciones = Convert.ToString(dr.GetValue(iObservaciones));

                    int iFechamodificacionstr = dr.GetOrdinal(helper.Fecha_Modificacion);
                    if (!dr.IsDBNull(iFechamodificacionstr)) entity.Fecha_Modificacion = Convert.ToString(dr.GetValue(iFechamodificacionstr));

                    int iUsuarioAuditoria = dr.GetOrdinal(helper.Usuario_Auditoria);
                    if (!dr.IsDBNull(iUsuarioAuditoria)) entity.Usuario_Auditoria = Convert.ToString(dr.GetValue(iUsuarioAuditoria));

                    int iMotivo = dr.GetOrdinal(helper.Motivo);
                    if (!dr.IsDBNull(iMotivo)) entity.Motivo = Convert.ToString(dr.GetValue(iMotivo));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }


        #endregion


        #endregion
    }
}
