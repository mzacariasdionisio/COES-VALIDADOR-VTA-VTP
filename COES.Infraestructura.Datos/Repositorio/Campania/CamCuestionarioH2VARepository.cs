using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Base.Core;
using COES.Dominio.DTO.Campania;
using COES.Dominio.Interfaces.Campania;
using COES.Infraestructura.Datos.Helper;
using System.Linq;
using COES.Infraestructura.Datos.Helper.Campania;

namespace COES.Infraestructura.Datos.Repositorio.Campania
{
    public class CamCuestionarioH2VARepository : RepositoryBase, ICamCuestionarioH2VARepository
    {
        public CamCuestionarioH2VARepository(string strConn) : base(strConn) { }

        CamCuestionarioH2VAHelper Helper = new CamCuestionarioH2VAHelper();

        public List<CuestionarioH2VADTO> GetCuestionarioH2VACodi(int proyectoId)
        {
            List<CuestionarioH2VADTO> cuestionarios = new List<CuestionarioH2VADTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetCuestionarioH2VACodi);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, proyectoId);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.Int32, Constantes.IndDel);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CuestionarioH2VADTO cuestionario = new CuestionarioH2VADTO();
                    cuestionario.H2vaCodi = !dr.IsDBNull(dr.GetOrdinal("H2VACODI")) ? dr.GetInt32(dr.GetOrdinal("H2VACODI")) : 0;
                    cuestionario.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    cuestionario.ProyNp = !dr.IsDBNull(dr.GetOrdinal("PROYNP")) ? dr.GetString(dr.GetOrdinal("PROYNP")) : "";
                    cuestionario.SocioOperador = !dr.IsDBNull(dr.GetOrdinal("SOCIOOPERADOR")) ? dr.GetString(dr.GetOrdinal("SOCIOOPERADOR")) : "";
                    cuestionario.SocioInversionista = !dr.IsDBNull(dr.GetOrdinal("SOCIOINVERSIONISTA")) ? dr.GetString(dr.GetOrdinal("SOCIOINVERSIONISTA")) : "";
                    cuestionario.Distrito = !dr.IsDBNull(dr.GetOrdinal("DISTRITO")) ? dr.GetString(dr.GetOrdinal("DISTRITO")) : "";
                    cuestionario.ActDesarrollar = !dr.IsDBNull(dr.GetOrdinal("ACTDESARROLLAR")) ? dr.GetString(dr.GetOrdinal("ACTDESARROLLAR")) : "";
                    cuestionario.SituacionAct = !dr.IsDBNull(dr.GetOrdinal("SITUACIONACT")) ? dr.GetString(dr.GetOrdinal("SITUACIONACT")) : "";
                    cuestionario.TipoElectrolizador = !dr.IsDBNull(dr.GetOrdinal("TIPOELECTROLIZADOR")) ? dr.GetString(dr.GetOrdinal("TIPOELECTROLIZADOR")) : "";
                    cuestionario.OtroElectrolizador = !dr.IsDBNull(dr.GetOrdinal("OTROELECTROLIZADOR")) ? dr.GetString(dr.GetOrdinal("OTROELECTROLIZADOR")) : "";
                    cuestionario.VidaUtil = !dr.IsDBNull(dr.GetOrdinal("VIDAUTIL")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("VIDAUTIL")) : null;
                    cuestionario.ProduccionAnual = !dr.IsDBNull(dr.GetOrdinal("PRODUCCIONANUAL")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("PRODUCCIONANUAL")) : null;
                    cuestionario.ObjetivoProyecto = !dr.IsDBNull(dr.GetOrdinal("OBJETIVOPROYECTO")) ? dr.GetString(dr.GetOrdinal("OBJETIVOPROYECTO")) : "";
                    cuestionario.OtroObjetivo = !dr.IsDBNull(dr.GetOrdinal("OTROOBJETIVO")) ? dr.GetString(dr.GetOrdinal("OTROOBJETIVO")) : "";
                    cuestionario.UsoEsperadoHidro = !dr.IsDBNull(dr.GetOrdinal("USOESPERADOHIDRO")) ? dr.GetString(dr.GetOrdinal("USOESPERADOHIDRO")) : "";
                    cuestionario.OtroUsoEsperadoHidro = !dr.IsDBNull(dr.GetOrdinal("OTROUSESPERADOHIDRO")) ? dr.GetString(dr.GetOrdinal("OTROUSESPERADOHIDRO")) : "";
                    cuestionario.MetodoTransH2 = !dr.IsDBNull(dr.GetOrdinal("METODOTRANSH2")) ? dr.GetString(dr.GetOrdinal("METODOTRANSH2")) : "";
                    cuestionario.OtroMetodoTransH2 = !dr.IsDBNull(dr.GetOrdinal("OTROMETODOTRANSH2")) ? dr.GetString(dr.GetOrdinal("OTROMETODOTRANSH2")) : "";
                    cuestionario.PoderCalorifico = !dr.IsDBNull(dr.GetOrdinal("PODERCALORIFICO")) ? dr.GetString(dr.GetOrdinal("PODERCALORIFICO")) : "";
                    cuestionario.SubEstacionSein = !dr.IsDBNull(dr.GetOrdinal("SUBESTACIONSEIN")) ? dr.GetString(dr.GetOrdinal("SUBESTACIONSEIN")) : "";
                    cuestionario.NivelTension = !dr.IsDBNull(dr.GetOrdinal("NIVELTENSION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("NIVELTENSION")) : null;
                    cuestionario.TipoSuministro = !dr.IsDBNull(dr.GetOrdinal("TIPOSUMINISTRO")) ? dr.GetString(dr.GetOrdinal("TIPOSUMINISTRO")) : "";
                    cuestionario.OtroSuministro = !dr.IsDBNull(dr.GetOrdinal("OTROSUMINISTRO")) ? dr.GetString(dr.GetOrdinal("OTROSUMINISTRO")) : "";
                    cuestionario.PrimeraEtapa = !dr.IsDBNull(dr.GetOrdinal("PRIMERAETAPA")) ? (int?)dr.GetInt32(dr.GetOrdinal("PRIMERAETAPA")) : null;
                    cuestionario.SegundaEtapa = !dr.IsDBNull(dr.GetOrdinal("SEGUNDAETAPA")) ? (int?)dr.GetInt32(dr.GetOrdinal("SEGUNDAETAPA")) : null;
                    cuestionario.Final = !dr.IsDBNull(dr.GetOrdinal("FINAL")) ? (int?)dr.GetInt32(dr.GetOrdinal("FINAL")) : null;
                    cuestionario.CostoProduccion = !dr.IsDBNull(dr.GetOrdinal("COSTOPRODUCCION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("COSTOPRODUCCION")) : null;
                    cuestionario.PrecioVenta = !dr.IsDBNull(dr.GetOrdinal("PRECIOVENTA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("PRECIOVENTA")) : null;
                    cuestionario.Financiamiento = !dr.IsDBNull(dr.GetOrdinal("FINANCIAMIENTO")) ? dr.GetString(dr.GetOrdinal("FINANCIAMIENTO")) : "";
                    cuestionario.FactFavorecenProy = !dr.IsDBNull(dr.GetOrdinal("FACTFAVORECENPROY")) ? dr.GetString(dr.GetOrdinal("FACTFAVORECENPROY")) : "";
                    cuestionario.FactDesfavorecenProy = !dr.IsDBNull(dr.GetOrdinal("FACTDESFAVORECENPROY")) ? dr.GetString(dr.GetOrdinal("FACTDESFAVORECENPROY")) : "";
                    cuestionario.Comentarios = !dr.IsDBNull(dr.GetOrdinal("COMENTARIOS")) ? dr.GetString(dr.GetOrdinal("COMENTARIOS")) : "";
                    cuestionarios.Add(cuestionario);
                }
            }
            return cuestionarios;
        }

        public bool SaveCuestionarioH2VA(CuestionarioH2VADTO cuestionario)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveCuestionarioH2VA);
            dbProvider.AddInParameter(dbCommand, "H2VACODI", DbType.Int32, cuestionario.H2vaCodi);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, cuestionario.ProyCodi);
            dbProvider.AddInParameter(dbCommand, "PROYNP", DbType.String, cuestionario.ProyNp);
            dbProvider.AddInParameter(dbCommand, "SOCIOOPERADOR", DbType.String, cuestionario.SocioOperador);
            dbProvider.AddInParameter(dbCommand, "SOCIOINVERSIONISTA", DbType.String, cuestionario.SocioInversionista);
            dbProvider.AddInParameter(dbCommand, "DISTRITO", DbType.String, cuestionario.Distrito);
            dbProvider.AddInParameter(dbCommand, "ACTDESARROLLAR", DbType.String, cuestionario.ActDesarrollar);
            dbProvider.AddInParameter(dbCommand, "SITUACIONACT", DbType.String, cuestionario.SituacionAct);
            dbProvider.AddInParameter(dbCommand, "TIPOELECTROLIZADOR", DbType.String, cuestionario.TipoElectrolizador);
            dbProvider.AddInParameter(dbCommand, "OTROELECTROLIZADOR", DbType.String, cuestionario.OtroElectrolizador);
            dbProvider.AddInParameter(dbCommand, "VIDAUTIL", DbType.Decimal, cuestionario.VidaUtil);
            dbProvider.AddInParameter(dbCommand, "PRODUCCIONANUAL", DbType.Decimal, cuestionario.ProduccionAnual);
            dbProvider.AddInParameter(dbCommand, "OBJETIVOPROYECTO", DbType.String, cuestionario.ObjetivoProyecto);
            dbProvider.AddInParameter(dbCommand, "OTROOBJETIVO", DbType.String, cuestionario.OtroObjetivo);
            dbProvider.AddInParameter(dbCommand, "USOESPERADOHIDRO", DbType.String, cuestionario.UsoEsperadoHidro);
            dbProvider.AddInParameter(dbCommand, "OTROUSESPERADOHIDRO", DbType.String, cuestionario.OtroUsoEsperadoHidro);
            dbProvider.AddInParameter(dbCommand, "METODOTRANSH2", DbType.String, cuestionario.MetodoTransH2);
            dbProvider.AddInParameter(dbCommand, "OTROMETODOTRANSH2", DbType.String, cuestionario.OtroMetodoTransH2);
            dbProvider.AddInParameter(dbCommand, "PODERCALORIFICO", DbType.String, cuestionario.PoderCalorifico);
            dbProvider.AddInParameter(dbCommand, "SUBESTACIONSEIN", DbType.String, cuestionario.SubEstacionSein);
            dbProvider.AddInParameter(dbCommand, "NIVELTENSION", DbType.Decimal, cuestionario.NivelTension);
            dbProvider.AddInParameter(dbCommand, "TIPOSUMINISTRO", DbType.String, cuestionario.TipoSuministro);
            dbProvider.AddInParameter(dbCommand, "OTROSUMINISTRO", DbType.String, cuestionario.OtroSuministro);
            dbProvider.AddInParameter(dbCommand, "PRIMERAETAPA", DbType.Int32, (object)cuestionario.PrimeraEtapa ?? DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "SEGUNDAETAPA", DbType.Int32, (object)cuestionario.SegundaEtapa ?? DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "FINAL", DbType.Int32, (object)cuestionario.Final ?? DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "COSTOPRODUCCION", DbType.Decimal, cuestionario.CostoProduccion);
            dbProvider.AddInParameter(dbCommand, "PRECIOVENTA", DbType.Decimal, cuestionario.PrecioVenta);
            dbProvider.AddInParameter(dbCommand, "FINANCIAMIENTO", DbType.String, cuestionario.Financiamiento);
            dbProvider.AddInParameter(dbCommand, "FACTFAVORECENPROY", DbType.String, cuestionario.FactFavorecenProy);
            dbProvider.AddInParameter(dbCommand, "FACTDESFAVORECENPROY", DbType.String, cuestionario.FactDesfavorecenProy);
            dbProvider.AddInParameter(dbCommand, "COMENTARIOS", DbType.String, cuestionario.Comentarios);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, cuestionario.UsuCreacion);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, cuestionario.IndDel);

            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        public bool DeleteCuestionarioH2VAById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteCuestionarioH2VAById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);
            return true;
        }

        public int GetLastCuestionarioH2VAId()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastCuestionarioH2VAId);
            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                count = Convert.ToInt32(result) + 1;
            }
            else
            {
                count = 1;
            }
            return count;
        }

        public CuestionarioH2VADTO GetCuestionarioH2VAById(int id)
        {
            CuestionarioH2VADTO cuestionario = new CuestionarioH2VADTO();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetCuestionarioH2VAById);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    cuestionario.H2vaCodi = !dr.IsDBNull(dr.GetOrdinal("H2VACODI")) ? dr.GetInt32(dr.GetOrdinal("H2VACODI")) : 0;
                    cuestionario.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    cuestionario.ProyNp = !dr.IsDBNull(dr.GetOrdinal("PROYNP")) ? dr.GetString(dr.GetOrdinal("PROYNP")) : "";
                    cuestionario.SocioOperador = !dr.IsDBNull(dr.GetOrdinal("SOCIOOPERADOR")) ? dr.GetString(dr.GetOrdinal("SOCIOOPERADOR")) : "";
                    cuestionario.SocioInversionista = !dr.IsDBNull(dr.GetOrdinal("SOCIOINVERSIONISTA")) ? dr.GetString(dr.GetOrdinal("SOCIOINVERSIONISTA")) : "";
                    cuestionario.Distrito = !dr.IsDBNull(dr.GetOrdinal("DISTRITO")) ? dr.GetString(dr.GetOrdinal("DISTRITO")) : "";
                    cuestionario.ActDesarrollar = !dr.IsDBNull(dr.GetOrdinal("ACTDESARROLLAR")) ? dr.GetString(dr.GetOrdinal("ACTDESARROLLAR")) : "";
                    cuestionario.SituacionAct = !dr.IsDBNull(dr.GetOrdinal("SITUACIONACT")) ? dr.GetString(dr.GetOrdinal("SITUACIONACT")) : "";
                    cuestionario.TipoElectrolizador = !dr.IsDBNull(dr.GetOrdinal("TIPOELECTROLIZADOR")) ? dr.GetString(dr.GetOrdinal("TIPOELECTROLIZADOR")) : "";
                    cuestionario.OtroElectrolizador = !dr.IsDBNull(dr.GetOrdinal("OTROELECTROLIZADOR")) ? dr.GetString(dr.GetOrdinal("OTROELECTROLIZADOR")) : "";
                    cuestionario.VidaUtil = !dr.IsDBNull(dr.GetOrdinal("VIDAUTIL")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("VIDAUTIL")) : null;
                    cuestionario.ProduccionAnual = !dr.IsDBNull(dr.GetOrdinal("PRODUCCIONANUAL")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("PRODUCCIONANUAL")) : null;
                    cuestionario.ObjetivoProyecto = !dr.IsDBNull(dr.GetOrdinal("OBJETIVOPROYECTO")) ? dr.GetString(dr.GetOrdinal("OBJETIVOPROYECTO")) : "";
                    cuestionario.OtroObjetivo = !dr.IsDBNull(dr.GetOrdinal("OTROOBJETIVO")) ? dr.GetString(dr.GetOrdinal("OTROOBJETIVO")) : "";
                    cuestionario.UsoEsperadoHidro = !dr.IsDBNull(dr.GetOrdinal("USOESPERADOHIDRO")) ? dr.GetString(dr.GetOrdinal("USOESPERADOHIDRO")) : "";
                    cuestionario.OtroUsoEsperadoHidro = !dr.IsDBNull(dr.GetOrdinal("OTROUSESPERADOHIDRO")) ? dr.GetString(dr.GetOrdinal("OTROUSESPERADOHIDRO")) : "";
                    cuestionario.MetodoTransH2 = !dr.IsDBNull(dr.GetOrdinal("METODOTRANSH2")) ? dr.GetString(dr.GetOrdinal("METODOTRANSH2")) : "";
                    cuestionario.OtroMetodoTransH2 = !dr.IsDBNull(dr.GetOrdinal("OTROMETODOTRANSH2")) ? dr.GetString(dr.GetOrdinal("OTROMETODOTRANSH2")) : "";
                    cuestionario.PoderCalorifico = !dr.IsDBNull(dr.GetOrdinal("PODERCALORIFICO")) ? dr.GetString(dr.GetOrdinal("PODERCALORIFICO")) : "";
                    cuestionario.SubEstacionSein = !dr.IsDBNull(dr.GetOrdinal("SUBESTACIONSEIN")) ? dr.GetString(dr.GetOrdinal("SUBESTACIONSEIN")) : "";
                    cuestionario.NivelTension = !dr.IsDBNull(dr.GetOrdinal("NIVELTENSION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("NIVELTENSION")) : null;
                    cuestionario.TipoSuministro = !dr.IsDBNull(dr.GetOrdinal("TIPOSUMINISTRO")) ? dr.GetString(dr.GetOrdinal("TIPOSUMINISTRO")) : "";
                    cuestionario.OtroSuministro = !dr.IsDBNull(dr.GetOrdinal("OTROSUMINISTRO")) ? dr.GetString(dr.GetOrdinal("OTROSUMINISTRO")) : "";
                    cuestionario.PrimeraEtapa = !dr.IsDBNull(dr.GetOrdinal("PRIMERAETAPA")) ? (int?)dr.GetInt32(dr.GetOrdinal("PRIMERAETAPA")) : null;
                    cuestionario.SegundaEtapa = !dr.IsDBNull(dr.GetOrdinal("SEGUNDAETAPA")) ? (int?)dr.GetInt32(dr.GetOrdinal("SEGUNDAETAPA")) : null;
                    cuestionario.Final = !dr.IsDBNull(dr.GetOrdinal("FINAL")) ? (int?)dr.GetInt32(dr.GetOrdinal("FINAL")) : null;
                    cuestionario.CostoProduccion = !dr.IsDBNull(dr.GetOrdinal("COSTOPRODUCCION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("COSTOPRODUCCION")) : null;
                    cuestionario.PrecioVenta = !dr.IsDBNull(dr.GetOrdinal("PRECIOVENTA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("PRECIOVENTA")) : null;
                    cuestionario.Financiamiento = !dr.IsDBNull(dr.GetOrdinal("FINANCIAMIENTO")) ? dr.GetString(dr.GetOrdinal("FINANCIAMIENTO")) : "";
                    cuestionario.FactFavorecenProy = !dr.IsDBNull(dr.GetOrdinal("FACTFAVORECENPROY")) ? dr.GetString(dr.GetOrdinal("FACTFAVORECENPROY")) : "";
                    cuestionario.FactDesfavorecenProy = !dr.IsDBNull(dr.GetOrdinal("FACTDESFAVORECENPROY")) ? dr.GetString(dr.GetOrdinal("FACTDESFAVORECENPROY")) : "";
                    cuestionario.Comentarios = !dr.IsDBNull(dr.GetOrdinal("COMENTARIOS")) ? dr.GetString(dr.GetOrdinal("COMENTARIOS")) : "";
                    cuestionario.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : "";
                    cuestionario.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    cuestionario.UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : "";
                    cuestionario.FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    cuestionario.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : "";

                }
            }
            return cuestionario;
        }

        public List<CuestionarioH2VADTO> GetFormatoH2VAByFilter(string plancodi, string empresa, string estado)
        {
            List<CuestionarioH2VADTO> oblist = new List<CuestionarioH2VADTO>();
            string query = $@"
                 SELECT CGB.*, TR.EMPRESANOM, TR.PROYNOMBRE, TR.PROYDESCRIPCION, TP.TIPONOMBRE, TF.TIPOFINOMBRE,TR.PROYCONFIDENCIAL  
                 FROM cam_cuestionarioh2va CGB
                 INNER JOIN CAM_TRNSMPROYECTO TR ON TR.PROYCODI = CGB.PROYCODI
                 INNER JOIN CAM_PLANTRANSMISION PL ON PL.PLANCODI = TR.PLANCODI
                 INNER JOIN CAM_TIPOPROYECTO TP ON TP.TIPOCODI = TR.TIPOCODI
                 LEFT JOIN CAM_TIPOFICHAPROYECTO TF ON TF.TIPOFICODI = TR.TIPOFICODI
                    WHERE TR.PERICODI  IN ({plancodi}) AND 
                    PL.CODEMPRESA IN ({empresa})  AND 
                    CGB.IND_DEL = 0 AND 
                    PL.PLANESTADO ='{estado}'
                    ORDER BY TR.PERICODI, CGB.PROYCODI,PL.CODEMPRESA, CGB.H2VACODI ASC";
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CuestionarioH2VADTO ob = new CuestionarioH2VADTO();
                    ob.H2vaCodi = !dr.IsDBNull(dr.GetOrdinal("H2VACODI")) ? dr.GetInt32(dr.GetOrdinal("H2VACODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.ProyNp = !dr.IsDBNull(dr.GetOrdinal("PROYNP")) ? dr.GetString(dr.GetOrdinal("PROYNP")) : "";
                    ob.SocioOperador = !dr.IsDBNull(dr.GetOrdinal("SOCIOOPERADOR")) ? dr.GetString(dr.GetOrdinal("SOCIOOPERADOR")) : "";
                    ob.SocioInversionista = !dr.IsDBNull(dr.GetOrdinal("SOCIOINVERSIONISTA")) ? dr.GetString(dr.GetOrdinal("SOCIOINVERSIONISTA")) : "";
                    ob.Distrito = !dr.IsDBNull(dr.GetOrdinal("DISTRITO")) ? dr.GetString(dr.GetOrdinal("DISTRITO")) : "";
                    ob.ActDesarrollar = !dr.IsDBNull(dr.GetOrdinal("ACTDESARROLLAR")) ? dr.GetString(dr.GetOrdinal("ACTDESARROLLAR")) : "";
                    ob.SituacionAct = !dr.IsDBNull(dr.GetOrdinal("SITUACIONACT")) ? dr.GetString(dr.GetOrdinal("SITUACIONACT")) : "";
                    ob.TipoElectrolizador = !dr.IsDBNull(dr.GetOrdinal("TIPOELECTROLIZADOR")) ? dr.GetString(dr.GetOrdinal("TIPOELECTROLIZADOR")) : "";
                    ob.OtroElectrolizador = !dr.IsDBNull(dr.GetOrdinal("OTROELECTROLIZADOR")) ? dr.GetString(dr.GetOrdinal("OTROELECTROLIZADOR")) : "";
                    ob.VidaUtil = !dr.IsDBNull(dr.GetOrdinal("VIDAUTIL")) ? dr.GetDecimal(dr.GetOrdinal("VIDAUTIL")) : (Decimal?)null;
                    ob.ProduccionAnual = !dr.IsDBNull(dr.GetOrdinal("PRODUCCIONANUAL")) ? dr.GetDecimal(dr.GetOrdinal("PRODUCCIONANUAL")) : (Decimal?)null;
                    ob.ObjetivoProyecto = !dr.IsDBNull(dr.GetOrdinal("OBJETIVOPROYECTO")) ? dr.GetString(dr.GetOrdinal("OBJETIVOPROYECTO")) : "";
                    ob.OtroObjetivo = !dr.IsDBNull(dr.GetOrdinal("OTROOBJETIVO")) ? dr.GetString(dr.GetOrdinal("OTROOBJETIVO")) : "";
                    ob.UsoEsperadoHidro = !dr.IsDBNull(dr.GetOrdinal("USOESPERADOHIDRO")) ? dr.GetString(dr.GetOrdinal("USOESPERADOHIDRO")) : "";
                    ob.OtroUsoEsperadoHidro = !dr.IsDBNull(dr.GetOrdinal("OTROUSESPERADOHIDRO")) ? dr.GetString(dr.GetOrdinal("OTROUSESPERADOHIDRO")) : "";
                    ob.MetodoTransH2 = !dr.IsDBNull(dr.GetOrdinal("METODOTRANSH2")) ? dr.GetString(dr.GetOrdinal("METODOTRANSH2")) : "";
                    ob.OtroMetodoTransH2 = !dr.IsDBNull(dr.GetOrdinal("OTROMETODOTRANSH2")) ? dr.GetString(dr.GetOrdinal("OTROMETODOTRANSH2")) : "";
                    ob.PoderCalorifico = !dr.IsDBNull(dr.GetOrdinal("PODERCALORIFICO")) ? dr.GetString(dr.GetOrdinal("PODERCALORIFICO")) : "";
                    ob.SubEstacionSein = !dr.IsDBNull(dr.GetOrdinal("SUBESTACIONSEIN")) ? dr.GetString(dr.GetOrdinal("SUBESTACIONSEIN")) : "";
                    ob.NivelTension = !dr.IsDBNull(dr.GetOrdinal("NIVELTENSION")) ? dr.GetDecimal(dr.GetOrdinal("NIVELTENSION")) : (Decimal?)null;
                    ob.TipoSuministro = !dr.IsDBNull(dr.GetOrdinal("TIPOSUMINISTRO")) ? dr.GetString(dr.GetOrdinal("TIPOSUMINISTRO")) : "";
                    ob.OtroSuministro = !dr.IsDBNull(dr.GetOrdinal("OTROSUMINISTRO")) ? dr.GetString(dr.GetOrdinal("OTROSUMINISTRO")) : "";
                    ob.PrimeraEtapa = !dr.IsDBNull(dr.GetOrdinal("PRIMERAETAPA")) ? dr.GetInt32(dr.GetOrdinal("PRIMERAETAPA")) : (int?)null;
                    ob.SegundaEtapa = !dr.IsDBNull(dr.GetOrdinal("SEGUNDAETAPA")) ? dr.GetInt32(dr.GetOrdinal("SEGUNDAETAPA")) : (int?)null;
                    ob.Final = !dr.IsDBNull(dr.GetOrdinal("FINAL")) ? dr.GetInt32(dr.GetOrdinal("FINAL")) : (int?)null;
                    ob.CostoProduccion = !dr.IsDBNull(dr.GetOrdinal("COSTOPRODUCCION")) ? dr.GetDecimal(dr.GetOrdinal("COSTOPRODUCCION")) : (Decimal?)null;
                    ob.PrecioVenta = !dr.IsDBNull(dr.GetOrdinal("PRECIOVENTA")) ? dr.GetDecimal(dr.GetOrdinal("PRECIOVENTA")) : (Decimal?)null;
                    ob.Financiamiento = !dr.IsDBNull(dr.GetOrdinal("FINANCIAMIENTO")) ? dr.GetString(dr.GetOrdinal("FINANCIAMIENTO")) : "";
                    ob.FactFavorecenProy = !dr.IsDBNull(dr.GetOrdinal("FACTFAVORECENPROY")) ? dr.GetString(dr.GetOrdinal("FACTFAVORECENPROY")) : "";
                    ob.FactDesfavorecenProy = !dr.IsDBNull(dr.GetOrdinal("FACTDESFAVORECENPROY")) ? dr.GetString(dr.GetOrdinal("FACTDESFAVORECENPROY")) : "";
                    ob.Comentarios = !dr.IsDBNull(dr.GetOrdinal("COMENTARIOS")) ? dr.GetString(dr.GetOrdinal("COMENTARIOS")) : "";
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : "";
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : "";
                    ob.FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : "";
                    ob.Empresa = !dr.IsDBNull(dr.GetOrdinal("EMPRESANOM")) ? dr.GetString(dr.GetOrdinal("EMPRESANOM")) : string.Empty;
                    // Nuevos campos
                    ob.NombreProyecto = !dr.IsDBNull(dr.GetOrdinal("PROYNOMBRE")) ? dr.GetString(dr.GetOrdinal("PROYNOMBRE")) : "";
                    ob.DetalleProyecto = !dr.IsDBNull(dr.GetOrdinal("PROYDESCRIPCION")) ? dr.GetString(dr.GetOrdinal("PROYDESCRIPCION")) : "";
                    ob.TipoProyecto = !dr.IsDBNull(dr.GetOrdinal("TIPONOMBRE")) ? dr.GetString(dr.GetOrdinal("TIPONOMBRE")) : "";
                    ob.Confidencial = !dr.IsDBNull(dr.GetOrdinal("PROYCONFIDENCIAL")) ? dr.GetString(dr.GetOrdinal("PROYCONFIDENCIAL")) : "";

                    oblist.Add(ob);
                }
            }
            return oblist;
        }

    }
}