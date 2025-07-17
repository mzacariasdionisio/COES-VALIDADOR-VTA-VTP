using COES.Base.Core;
using COES.Dominio.DTO.Campania;
using COES.Dominio.Interfaces.Campania;
using COES.Infraestructura.Datos.Helper;
using COES.Infraestructura.Datos.Helper.Campania;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace COES.Infraestructura.Datos.Repositorio.Campania
{
    public class CamRegHojaEolARepository : RepositoryBase, ICamRegHojaEolARepository
    {
        public CamRegHojaEolARepository(string strConn) : base(strConn) { }

        CamRegHojaEolAHelper Helper = new CamRegHojaEolAHelper();

        public List<RegHojaEolADTO> GetRegHojaEolACodi(int id)
        {
            List<RegHojaEolADTO> RegHojaEolADTOs = new List<RegHojaEolADTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetRegHojaEolACodi);
            dbProvider.AddInParameter(command, "CENTRALACODI", DbType.String, id);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RegHojaEolADTO ob = new RegHojaEolADTO();

                    ob.CentralACodi = !dr.IsDBNull(dr.GetOrdinal("CENTRALACODI")) ? dr.GetInt32(dr.GetOrdinal("CENTRALACODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.CentralNombre = !dr.IsDBNull(dr.GetOrdinal("CENTRALNOMBRE")) ? dr.GetString(dr.GetOrdinal("CENTRALNOMBRE")) : string.Empty;
                    ob.Distrito = !dr.IsDBNull(dr.GetOrdinal("DISTRITO")) ? dr.GetString(dr.GetOrdinal("DISTRITO")) : string.Empty;
                    ob.Propietario = !dr.IsDBNull(dr.GetOrdinal("PROPIETARIO")) ? dr.GetString(dr.GetOrdinal("PROPIETARIO")) : string.Empty;
                    ob.OtroPropietario = !dr.IsDBNull(dr.GetOrdinal("OTROPROPIETARIO")) ? dr.GetString(dr.GetOrdinal("OTROPROPIETARIO")) : string.Empty;
                    ob.SocioOperador = !dr.IsDBNull(dr.GetOrdinal("SOCIOOPERADOR")) ? dr.GetString(dr.GetOrdinal("SOCIOOPERADOR")) : string.Empty;
                    ob.SocioInversionista = !dr.IsDBNull(dr.GetOrdinal("SOCIOINVERSIONISTA")) ? dr.GetString(dr.GetOrdinal("SOCIOINVERSIONISTA")) : string.Empty;
                    ob.ConcesionTemporal = !dr.IsDBNull(dr.GetOrdinal("CONCESIONTEMPORAL")) ? dr.GetString(dr.GetOrdinal("CONCESIONTEMPORAL")) : string.Empty;
                    ob.FechaConcesionTemporal = !dr.IsDBNull(dr.GetOrdinal("FECHACONCESIONTEMPORAL")) ? dr.GetDateTime(dr.GetOrdinal("FECHACONCESIONTEMPORAL")) : (DateTime?)null;
                    ob.TipoConcesionActual = !dr.IsDBNull(dr.GetOrdinal("TIPOCONCESIONACTUAL")) ? dr.GetString(dr.GetOrdinal("TIPOCONCESIONACTUAL")) : string.Empty;
                    ob.FechaConcesionActual = !dr.IsDBNull(dr.GetOrdinal("FECHACONCESIONACTUAL")) ? dr.GetDateTime(dr.GetOrdinal("FECHACONCESIONACTUAL")) : (DateTime?)null;
                    ob.NombreEstacionMet = !dr.IsDBNull(dr.GetOrdinal("NOMBREESTACIONMET")) ? dr.GetString(dr.GetOrdinal("NOMBREESTACIONMET")) : string.Empty;
                    ob.NumEstacionMet = !dr.IsDBNull(dr.GetOrdinal("NUMESTACIONMET")) ? (int?)dr.GetInt32(dr.GetOrdinal("NUMESTACIONMET")) : null;
                    ob.SerieVelViento = !dr.IsDBNull(dr.GetOrdinal("SERIEVELVIENTO")) ? dr.GetString(dr.GetOrdinal("SERIEVELVIENTO")) : string.Empty;
                    ob.PeriodoDisAnio = !dr.IsDBNull(dr.GetOrdinal("PERIODODISANIO")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("PERIODODISANIO")) : null;
                    ob.EstudioGeologico = !dr.IsDBNull(dr.GetOrdinal("ESTUDIOGEOLOGICO")) ? dr.GetString(dr.GetOrdinal("ESTUDIOGEOLOGICO")) : string.Empty;
                    ob.PerfoDiamantinas = !dr.IsDBNull(dr.GetOrdinal("PERFODIAMANTINAS")) ? (int?)dr.GetInt32(dr.GetOrdinal("PERFODIAMANTINAS")) : null;
                    ob.NumCalicatas =     !dr.IsDBNull(dr.GetOrdinal("NUMCALICATAS")) ? (int?)dr.GetInt32(dr.GetOrdinal("NUMCALICATAS")) : null;
                    ob.EstudioTopografico = !dr.IsDBNull(dr.GetOrdinal("ESTUDIOTOPOGRAFICO")) ? dr.GetString(dr.GetOrdinal("ESTUDIOTOPOGRAFICO")) : string.Empty;
                    ob.LevantamientoTopografico = !dr.IsDBNull(dr.GetOrdinal("LEVANTAMIENTOTOPOGRAFICO")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("LEVANTAMIENTOTOPOGRAFICO")) : null;
                    ob.PotenciaInstalada = !dr.IsDBNull(dr.GetOrdinal("POTENCIAINSTALADA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("POTENCIAINSTALADA")) : null;
                    ob.VelVientoInstalada = !dr.IsDBNull(dr.GetOrdinal("VELVIENTOINSTALADA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("VELVIENTOINSTALADA")) : null;
                    ob.HorPotNominal = !dr.IsDBNull(dr.GetOrdinal("HORPOTNOMINAL")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("HORPOTNOMINAL")) : null;
                    ob.VelDesconexion = !dr.IsDBNull(dr.GetOrdinal("VELDESCONEXION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("VELDESCONEXION")) : null;
                    ob.VelConexion = !dr.IsDBNull(dr.GetOrdinal("VELCONEXION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("VELCONEXION")) : null;
                    ob.EfiDescargaBat = !dr.IsDBNull(dr.GetOrdinal("EFIDESCARGABAT")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("EFIDESCARGABAT")) : null;
                    ob.TiempoMaxRegulacion = !dr.IsDBNull(dr.GetOrdinal("TIEMPMAXREGULACION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("TIEMPMAXREGULACION")) : null;
                    ob.RampaCargDescarg = !dr.IsDBNull(dr.GetOrdinal("RAMPCARGDESCARG")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("RAMPCARGDESCARG")) : null;
                    ob.TensionKv = !dr.IsDBNull(dr.GetOrdinal("TENSIONKV")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("TENSIONKV")) : null;
                    ob.LongitudKm = !dr.IsDBNull(dr.GetOrdinal("LONGITUDKM")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("LONGITUDKM")) : null;
                    ob.NumTernas = !dr.IsDBNull(dr.GetOrdinal("NUMTERNAS")) ? (int?)dr.GetInt32(dr.GetOrdinal("NUMTERNAS")) : null;
                    ob.NombreSubestacion = !dr.IsDBNull(dr.GetOrdinal("NOMBRESUBESTACION")) ? dr.GetString(dr.GetOrdinal("NOMBRESUBESTACION")) : string.Empty;
                    ob.NombreSubOtro = !dr.IsDBNull(dr.GetOrdinal("NOMBRESUBOTRO")) ? dr.GetString(dr.GetOrdinal("NOMBRESUBOTRO")) : string.Empty;
                    ob.Perfil = !dr.IsDBNull(dr.GetOrdinal("PERFIL")) ? dr.GetString(dr.GetOrdinal("PERFIL")) : string.Empty;
                    ob.Prefactibilidad = !dr.IsDBNull(dr.GetOrdinal("PREFACTIBILIDAD")) ? dr.GetString(dr.GetOrdinal("PREFACTIBILIDAD")) : string.Empty;
                    ob.Factibilidad = !dr.IsDBNull(dr.GetOrdinal("FACTIBILIDAD")) ? dr.GetString(dr.GetOrdinal("FACTIBILIDAD")) : string.Empty;
                    ob.EstudioDefinitivo = !dr.IsDBNull(dr.GetOrdinal("ESTUDIODEFINITIVO")) ? dr.GetString(dr.GetOrdinal("ESTUDIODEFINITIVO")) : string.Empty;
                    ob.Eia = !dr.IsDBNull(dr.GetOrdinal("EIA")) ? dr.GetString(dr.GetOrdinal("EIA")) : string.Empty;
                    ob.FechaInicioConstruccion = !dr.IsDBNull(dr.GetOrdinal("FECHAINICIOCONSTRUCCION")) ? dr.GetString(dr.GetOrdinal("FECHAINICIOCONSTRUCCION")) : string.Empty;
                    ob.PeriodoConstruccion = !dr.IsDBNull(dr.GetOrdinal("PERIODOCONSTRUCCION")) ? (int?)dr.GetInt32(dr.GetOrdinal("PERIODOCONSTRUCCION")) : null;
                    ob.FechaOperacionComercial = !dr.IsDBNull(dr.GetOrdinal("FECHAOPERACIONCOMERCIAL")) ? dr.GetString(dr.GetOrdinal("FECHAOPERACIONCOMERCIAL")) : string.Empty;
                    ob.Comentarios = !dr.IsDBNull(dr.GetOrdinal("COMENTARIOS")) ? dr.GetString(dr.GetOrdinal("COMENTARIOS")) : string.Empty;
                    ob.UsuCreacion = dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? null : dr.GetString(dr.GetOrdinal("USU_CREACION"));
                    ob.FecCreacion = dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? (DateTime?)null : dr.GetDateTime(dr.GetOrdinal("FEC_CREACION"));
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("INDDEL")) ? dr.GetString(dr.GetOrdinal("INDDEL")) : string.Empty;

                    RegHojaEolADTOs.Add(ob);
                }

            }

            return RegHojaEolADTOs;
        }

        public bool SaveRegHojaEolA(RegHojaEolADTO regHojaEolADTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveRegHojaEolA);

            dbProvider.AddInParameter(dbCommand, "CENTRALACODI", DbType.Int32, ObtenerValorOrDefault(regHojaEolADTO.CentralACodi, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, ObtenerValorOrDefault(regHojaEolADTO.ProyCodi, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "CENTRALNOMBRE", DbType.String, ObtenerValorOrDefault(regHojaEolADTO.CentralNombre, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "DISTRITO", DbType.String, ObtenerValorOrDefault(regHojaEolADTO.Distrito, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "PROPIETARIO", DbType.String, ObtenerValorOrDefault(regHojaEolADTO.Propietario, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "OTROPROPIETARIO", DbType.String, ObtenerValorOrDefault(regHojaEolADTO.OtroPropietario, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "SOCIOOPERADOR", DbType.String, ObtenerValorOrDefault(regHojaEolADTO.SocioOperador, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "SOCIOINVERSIONISTA", DbType.String, ObtenerValorOrDefault(regHojaEolADTO.SocioInversionista, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "CONCESIONTEMPORAL", DbType.String, ObtenerValorOrDefault(regHojaEolADTO.ConcesionTemporal, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "FECHACONCESIONTEMPORAL", DbType.DateTime, regHojaEolADTO.FechaConcesionTemporal);
            dbProvider.AddInParameter(dbCommand, "TIPOCONCESIONACTUAL", DbType.String, ObtenerValorOrDefault(regHojaEolADTO.TipoConcesionActual, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "FECHACONCESIONACTUAL", DbType.DateTime, regHojaEolADTO.FechaConcesionActual);
            dbProvider.AddInParameter(dbCommand, "NOMBREESTACIONMET", DbType.String, ObtenerValorOrDefault(regHojaEolADTO.NombreEstacionMet, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "NUMESTACIONMET", DbType.Int32, ObtenerValorOrDefault(regHojaEolADTO.NumEstacionMet, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "SERIEVELVIENTO", DbType.String, ObtenerValorOrDefault(regHojaEolADTO.SerieVelViento, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "PERIODODISANIO", DbType.Decimal, ObtenerValorOrDefault(regHojaEolADTO.PeriodoDisAnio, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "ESTUDIOGEOLOGICO", DbType.String, ObtenerValorOrDefault(regHojaEolADTO.EstudioGeologico, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "PERFODIAMANTINAS", DbType.Decimal, ObtenerValorOrDefault(regHojaEolADTO.PerfoDiamantinas, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "NUMCALICATAS", DbType.Int32, ObtenerValorOrDefault(regHojaEolADTO.NumCalicatas, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "ESTUDIOTOPOGRAFICO", DbType.String, ObtenerValorOrDefault(regHojaEolADTO.EstudioTopografico, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "LEVANTAMIENTOTOPOGRAFICO", DbType.Decimal, ObtenerValorOrDefault(regHojaEolADTO.LevantamientoTopografico, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "POTENCIAINSTALADA", DbType.Decimal, ObtenerValorOrDefault(regHojaEolADTO.PotenciaInstalada, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "VELVIENTOINSTALADA", DbType.Decimal, ObtenerValorOrDefault(regHojaEolADTO.VelVientoInstalada, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "HORPOTNOMINAL", DbType.Decimal, ObtenerValorOrDefault(regHojaEolADTO.HorPotNominal, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "VELDESCONEXION", DbType.Decimal, ObtenerValorOrDefault(regHojaEolADTO.VelDesconexion, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "VELCONEXION", DbType.Decimal, ObtenerValorOrDefault(regHojaEolADTO.VelConexion, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "TIPOCONTRCENTRAL", DbType.String, ObtenerValorOrDefault(regHojaEolADTO.TipoContrCentral, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "RANGOVELTURBINA", DbType.String, ObtenerValorOrDefault(regHojaEolADTO.RangoVelTurbina, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "TIPOTURBINA", DbType.String, ObtenerValorOrDefault(regHojaEolADTO.TipoTurbina, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "ENERGIAANUAL", DbType.Decimal, ObtenerValorOrDefault(regHojaEolADTO.EnergiaAnual, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "TIPOPARQELICO", DbType.String, ObtenerValorOrDefault(regHojaEolADTO.TipoParqEolico, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "TIPOTECGENERADOR", DbType.String, ObtenerValorOrDefault(regHojaEolADTO.TipoTecGenerador, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "NUMPALTURBINA", DbType.Decimal, ObtenerValorOrDefault(regHojaEolADTO.NumPalTurbina, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "DIAROTOR", DbType.Decimal, ObtenerValorOrDefault(regHojaEolADTO.DiaRotor, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "LONGPALA", DbType.Decimal, ObtenerValorOrDefault(regHojaEolADTO.LongPala, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "ALTURATORRE", DbType.Decimal, ObtenerValorOrDefault(regHojaEolADTO.AlturaTorre, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "POTNOMGENERADOR", DbType.Decimal, ObtenerValorOrDefault(regHojaEolADTO.PotNomGenerador, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "NUMUNIDADES", DbType.Decimal, ObtenerValorOrDefault(regHojaEolADTO.NumUnidades, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "NUMPOLOS", DbType.Decimal, ObtenerValorOrDefault(regHojaEolADTO.NumPolos, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "TENSIONGENERACION", DbType.Decimal, ObtenerValorOrDefault(regHojaEolADTO.TensionGeneracion, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "BESS", DbType.String, ObtenerValorOrDefault(regHojaEolADTO.Bess, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "ENERGIAMAXBAT", DbType.Decimal, ObtenerValorOrDefault(regHojaEolADTO.EnergiaMaxBat, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "POTENCIAMAXBAT", DbType.Decimal, ObtenerValorOrDefault(regHojaEolADTO.PotenciaMaxBat, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "EFICARGABAT", DbType.Decimal, ObtenerValorOrDefault(regHojaEolADTO.EfiCargaBat, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "EFIDESCARGABAT", DbType.Decimal, ObtenerValorOrDefault(regHojaEolADTO.EfiDescargaBat, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "TIEMPMAXREGULACION", DbType.Decimal, ObtenerValorOrDefault(regHojaEolADTO.TiempoMaxRegulacion, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "RAMPCARGDESCARG", DbType.Decimal, ObtenerValorOrDefault(regHojaEolADTO.RampaCargDescarg, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "TENSIONKV", DbType.Decimal, ObtenerValorOrDefault(regHojaEolADTO.TensionKv, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "LONGITUDKM", DbType.Decimal, ObtenerValorOrDefault(regHojaEolADTO.LongitudKm, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "NUMTERNAS", DbType.Int32, ObtenerValorOrDefault(regHojaEolADTO.NumTernas, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "NOMBRESUBESTACION", DbType.String, ObtenerValorOrDefault(regHojaEolADTO.NombreSubestacion, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "NOMBRESUBOTRO", DbType.String, ObtenerValorOrDefault(regHojaEolADTO.NombreSubOtro, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "PERFIL", DbType.String, ObtenerValorOrDefault(regHojaEolADTO.Perfil, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "PREFACTIBILIDAD", DbType.String, ObtenerValorOrDefault(regHojaEolADTO.Prefactibilidad, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "FACTIBILIDAD", DbType.String, ObtenerValorOrDefault(regHojaEolADTO.Factibilidad, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "ESTUDIODEFINITIVO", DbType.String, ObtenerValorOrDefault(regHojaEolADTO.EstudioDefinitivo, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "EIA", DbType.String, ObtenerValorOrDefault(regHojaEolADTO.Eia, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "FECHAINICIOCONSTRUCCION", DbType.String, ObtenerValorOrDefault(regHojaEolADTO.FechaInicioConstruccion, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "PERIODOCONSTRUCCION", DbType.Int32, ObtenerValorOrDefault(regHojaEolADTO.PeriodoConstruccion, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "FECHAOPERACIONCOMERCIAL", DbType.String, ObtenerValorOrDefault(regHojaEolADTO.FechaOperacionComercial, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "COMENTARIOS", DbType.String, ObtenerValorOrDefault(regHojaEolADTO.Comentarios, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, ObtenerValorOrDefault(regHojaEolADTO.UsuCreacion, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, ObtenerValorOrDefault(regHojaEolADTO.FecCreacion, typeof(DateTime?)));
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, ObtenerValorOrDefault(Constantes.IndDel, typeof(string)));

            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        public bool DeleteRegHojaEolAById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteRegHojaEolAById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);
            return true;
        }

        public int GetLastRegHojaEolAId()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastRegHojaEolAId);
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

        public RegHojaEolADTO GetRegHojaEolAById(int id)
        {
            RegHojaEolADTO ob = new RegHojaEolADTO();
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(Helper.SqlGetRegHojaEolAById);
            dbProvider.AddInParameter(commandHoja, "PROYCODI", DbType.Int32, id);
            dbProvider.AddInParameter(commandHoja, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.ExecuteNonQuery(commandHoja);
            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                if (dr.Read())
                {
                    ob.CentralACodi = !dr.IsDBNull(dr.GetOrdinal("CENTRALACODI")) ? dr.GetInt32(dr.GetOrdinal("CENTRALACODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.CentralNombre = !dr.IsDBNull(dr.GetOrdinal("CENTRALNOMBRE")) ? dr.GetString(dr.GetOrdinal("CENTRALNOMBRE")) : string.Empty;
                    ob.Distrito = !dr.IsDBNull(dr.GetOrdinal("DISTRITO")) ? dr.GetString(dr.GetOrdinal("DISTRITO")) : string.Empty;
                    ob.Propietario = !dr.IsDBNull(dr.GetOrdinal("PROPIETARIO")) ? dr.GetString(dr.GetOrdinal("PROPIETARIO")) : string.Empty;
                    ob.OtroPropietario = !dr.IsDBNull(dr.GetOrdinal("OTROPROPIETARIO")) ? dr.GetString(dr.GetOrdinal("OTROPROPIETARIO")) : string.Empty;
                    ob.SocioOperador = !dr.IsDBNull(dr.GetOrdinal("SOCIOOPERADOR")) ? dr.GetString(dr.GetOrdinal("SOCIOOPERADOR")) : string.Empty;
                    ob.SocioInversionista = !dr.IsDBNull(dr.GetOrdinal("SOCIOINVERSIONISTA")) ? dr.GetString(dr.GetOrdinal("SOCIOINVERSIONISTA")) : string.Empty;
                    ob.ConcesionTemporal = !dr.IsDBNull(dr.GetOrdinal("CONCESIONTEMPORAL")) ? dr.GetString(dr.GetOrdinal("CONCESIONTEMPORAL")) : string.Empty;
                    ob.FechaConcesionTemporal = !dr.IsDBNull(dr.GetOrdinal("FECHACONCESIONTEMPORAL")) ? dr.GetDateTime(dr.GetOrdinal("FECHACONCESIONTEMPORAL")) : (DateTime?)null;
                    ob.TipoConcesionActual = !dr.IsDBNull(dr.GetOrdinal("TIPOCONCESIONACTUAL")) ? dr.GetString(dr.GetOrdinal("TIPOCONCESIONACTUAL")) : string.Empty;
                    ob.FechaConcesionActual = !dr.IsDBNull(dr.GetOrdinal("FECHACONCESIONACTUAL")) ? dr.GetDateTime(dr.GetOrdinal("FECHACONCESIONACTUAL")) : (DateTime?)null;
                    ob.NombreEstacionMet = !dr.IsDBNull(dr.GetOrdinal("NOMBREESTACIONMET")) ? dr.GetString(dr.GetOrdinal("NOMBREESTACIONMET")) : string.Empty;
                    ob.NumEstacionMet = !dr.IsDBNull(dr.GetOrdinal("NUMESTACIONMET")) ? (int?)dr.GetInt32(dr.GetOrdinal("NUMESTACIONMET")) : null;
                    ob.SerieVelViento = !dr.IsDBNull(dr.GetOrdinal("SERIEVELVIENTO")) ? dr.GetString(dr.GetOrdinal("SERIEVELVIENTO")) : string.Empty;
                    ob.PeriodoDisAnio = !dr.IsDBNull(dr.GetOrdinal("PERIODODISANIO")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("PERIODODISANIO")) : null;
                    ob.EstudioGeologico = !dr.IsDBNull(dr.GetOrdinal("ESTUDIOGEOLOGICO")) ? dr.GetString(dr.GetOrdinal("ESTUDIOGEOLOGICO")) : string.Empty;
                    ob.PerfoDiamantinas = !dr.IsDBNull(dr.GetOrdinal("PERFODIAMANTINAS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("PERFODIAMANTINAS")) : null;
                    ob.NumCalicatas = !dr.IsDBNull(dr.GetOrdinal("NUMCALICATAS")) ? (int?)dr.GetInt32(dr.GetOrdinal("NUMCALICATAS")) : null;
                    ob.EstudioTopografico = !dr.IsDBNull(dr.GetOrdinal("ESTUDIOTOPOGRAFICO")) ? dr.GetString(dr.GetOrdinal("ESTUDIOTOPOGRAFICO")) : string.Empty;
                    ob.LevantamientoTopografico = !dr.IsDBNull(dr.GetOrdinal("LEVANTAMIENTOTOPOGRAFICO")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("LEVANTAMIENTOTOPOGRAFICO")) : null;
                    ob.PotenciaInstalada = !dr.IsDBNull(dr.GetOrdinal("POTENCIAINSTALADA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("POTENCIAINSTALADA")) : null;
                    ob.VelVientoInstalada = !dr.IsDBNull(dr.GetOrdinal("VELVIENTOINSTALADA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("VELVIENTOINSTALADA")) : null;
                    ob.HorPotNominal = !dr.IsDBNull(dr.GetOrdinal("HORPOTNOMINAL")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("HORPOTNOMINAL")) : null;
                    ob.VelDesconexion = !dr.IsDBNull(dr.GetOrdinal("VELDESCONEXION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("VELDESCONEXION")) : null;
                    ob.VelConexion = !dr.IsDBNull(dr.GetOrdinal("VELCONEXION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("VELCONEXION")) : null;
                    ob.TipoContrCentral = !dr.IsDBNull(dr.GetOrdinal("TIPOCONTRCENTRAL")) ? dr.GetString(dr.GetOrdinal("TIPOCONTRCENTRAL")) : string.Empty;
                    ob.RangoVelTurbina = !dr.IsDBNull(dr.GetOrdinal("RANGOVELTURBINA")) ? dr.GetString(dr.GetOrdinal("RANGOVELTURBINA")) : string.Empty;
                    ob.TipoTurbina = !dr.IsDBNull(dr.GetOrdinal("TIPOTURBINA")) ? dr.GetString(dr.GetOrdinal("TIPOTURBINA")) : string.Empty;
                    ob.EnergiaAnual = !dr.IsDBNull(dr.GetOrdinal("ENERGIAANUAL")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("ENERGIAANUAL")) : null;
                    ob.TipoParqEolico = !dr.IsDBNull(dr.GetOrdinal("TIPOPARQELICO")) ? dr.GetString(dr.GetOrdinal("TIPOPARQELICO")) : string.Empty;
                    ob.TipoTecGenerador = !dr.IsDBNull(dr.GetOrdinal("TIPOTECGENERADOR")) ? dr.GetString(dr.GetOrdinal("TIPOTECGENERADOR")) : string.Empty;
                    ob.NumPalTurbina = !dr.IsDBNull(dr.GetOrdinal("NUMPALTURBINA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("NUMPALTURBINA")) : null;
                    ob.DiaRotor = !dr.IsDBNull(dr.GetOrdinal("DIAROTOR")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("DIAROTOR")) : null;
                    ob.LongPala = !dr.IsDBNull(dr.GetOrdinal("LONGPALA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("LONGPALA")) : null;
                    ob.AlturaTorre = !dr.IsDBNull(dr.GetOrdinal("ALTURATORRE")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("ALTURATORRE")) : null;
                    ob.PotNomGenerador = !dr.IsDBNull(dr.GetOrdinal("POTNOMGENERADOR")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("POTNOMGENERADOR")) : null;
                    ob.NumUnidades = !dr.IsDBNull(dr.GetOrdinal("NUMUNIDADES")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("NUMUNIDADES")) : null;
                    ob.NumPolos = !dr.IsDBNull(dr.GetOrdinal("NUMPOLOS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("NUMPOLOS")) : null;
                    ob.TensionGeneracion = !dr.IsDBNull(dr.GetOrdinal("TENSIONGENERACION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("TENSIONGENERACION")) : null;
                    ob.Bess = !dr.IsDBNull(dr.GetOrdinal("BESS")) ? dr.GetString(dr.GetOrdinal("BESS")) : string.Empty;
                    ob.EnergiaMaxBat = !dr.IsDBNull(dr.GetOrdinal("ENERGIAMAXBAT")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("ENERGIAMAXBAT")) : null;
                    ob.PotenciaMaxBat = !dr.IsDBNull(dr.GetOrdinal("POTENCIAMAXBAT")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("POTENCIAMAXBAT")) : null;
                    ob.EfiCargaBat = !dr.IsDBNull(dr.GetOrdinal("EFICARGABAT")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("EFICARGABAT")) : null;
                    ob.EfiDescargaBat = !dr.IsDBNull(dr.GetOrdinal("EFIDESCARGABAT")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("EFIDESCARGABAT")) : null;
                    ob.TiempoMaxRegulacion = !dr.IsDBNull(dr.GetOrdinal("TIEMPMAXREGULACION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("TIEMPMAXREGULACION")) : null;
                    ob.RampaCargDescarg = !dr.IsDBNull(dr.GetOrdinal("RAMPCARGDESCARG")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("RAMPCARGDESCARG")) : null;
                    ob.TensionKv = !dr.IsDBNull(dr.GetOrdinal("TENSIONKV")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("TENSIONKV")) : null;
                    ob.LongitudKm = !dr.IsDBNull(dr.GetOrdinal("LONGITUDKM")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("LONGITUDKM")) : null;
                    ob.NumTernas = !dr.IsDBNull(dr.GetOrdinal("NUMTERNAS")) ? (int?)dr.GetInt32(dr.GetOrdinal("NUMTERNAS")) : null;
                    ob.NombreSubestacion = !dr.IsDBNull(dr.GetOrdinal("NOMBRESUBESTACION")) ? dr.GetString(dr.GetOrdinal("NOMBRESUBESTACION")) : string.Empty;
                    ob.NombreSubOtro = !dr.IsDBNull(dr.GetOrdinal("NOMBRESUBOTRO")) ? dr.GetString(dr.GetOrdinal("NOMBRESUBOTRO")) : string.Empty;
                    ob.Perfil = !dr.IsDBNull(dr.GetOrdinal("PERFIL")) ? dr.GetString(dr.GetOrdinal("PERFIL")) : string.Empty;
                    ob.Prefactibilidad = !dr.IsDBNull(dr.GetOrdinal("PREFACTIBILIDAD")) ? dr.GetString(dr.GetOrdinal("PREFACTIBILIDAD")) : string.Empty;
                    ob.Factibilidad = !dr.IsDBNull(dr.GetOrdinal("FACTIBILIDAD")) ? dr.GetString(dr.GetOrdinal("FACTIBILIDAD")) : string.Empty;
                    ob.EstudioDefinitivo = !dr.IsDBNull(dr.GetOrdinal("ESTUDIODEFINITIVO")) ? dr.GetString(dr.GetOrdinal("ESTUDIODEFINITIVO")) : string.Empty;
                    ob.Eia = !dr.IsDBNull(dr.GetOrdinal("EIA")) ? dr.GetString(dr.GetOrdinal("EIA")) : string.Empty;
                    ob.FechaInicioConstruccion = !dr.IsDBNull(dr.GetOrdinal("FECHAINICIOCONSTRUCCION")) ? dr.GetString(dr.GetOrdinal("FECHAINICIOCONSTRUCCION")) : string.Empty;
                    ob.PeriodoConstruccion = !dr.IsDBNull(dr.GetOrdinal("PERIODOCONSTRUCCION")) ? (int?)dr.GetInt32(dr.GetOrdinal("PERIODOCONSTRUCCION")) : null;
                    ob.FechaOperacionComercial = !dr.IsDBNull(dr.GetOrdinal("FECHAOPERACIONCOMERCIAL")) ? dr.GetString(dr.GetOrdinal("FECHAOPERACIONCOMERCIAL")) : string.Empty;
                    ob.Comentarios = !dr.IsDBNull(dr.GetOrdinal("COMENTARIOS")) ? dr.GetString(dr.GetOrdinal("COMENTARIOS")) : string.Empty;
                    ob.UsuCreacion = dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? null : dr.GetString(dr.GetOrdinal("USU_CREACION"));
                    ob.FecCreacion = dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? (DateTime?)null : dr.GetDateTime(dr.GetOrdinal("FEC_CREACION"));
                    ob.IndDel = dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? null : dr.GetString(dr.GetOrdinal("IND_DEL"));
                }

            }
            return ob;
        }

        public bool UpdateRegHojaEolA(RegHojaEolADTO regHojaEolADTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlUpdateRegHojaEolA);
            
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, regHojaEolADTO.ProyCodi);
            dbProvider.AddInParameter(dbCommand, "CENTRALNOMBRE", DbType.String, regHojaEolADTO.CentralNombre);
            dbProvider.AddInParameter(dbCommand, "DISTRITO", DbType.String, regHojaEolADTO.Distrito);
            dbProvider.AddInParameter(dbCommand, "PROPIETARIO", DbType.String, regHojaEolADTO.Propietario);
            dbProvider.AddInParameter(dbCommand, "OTROPROPIETARIO", DbType.String, regHojaEolADTO.OtroPropietario);
            dbProvider.AddInParameter(dbCommand, "SOCIOOPERADOR", DbType.String, regHojaEolADTO.SocioOperador);
            dbProvider.AddInParameter(dbCommand, "SOCIOINVERSIONISTA", DbType.String, regHojaEolADTO.SocioInversionista);
            dbProvider.AddInParameter(dbCommand, "CONCESIONTEMPORAL", DbType.String, regHojaEolADTO.ConcesionTemporal);
            dbProvider.AddInParameter(dbCommand, "FECHACONCESIONTEMPORAL", DbType.DateTime, regHojaEolADTO.FechaConcesionTemporal);
            dbProvider.AddInParameter(dbCommand, "TIPOCONCESIONACTUAL", DbType.String, regHojaEolADTO.TipoConcesionActual);
            dbProvider.AddInParameter(dbCommand, "FECHACONCESIONACTUAL", DbType.DateTime, regHojaEolADTO.FechaConcesionActual);
            dbProvider.AddInParameter(dbCommand, "NOMBREESTACION", DbType.String, regHojaEolADTO.NombreEstacionMet);
            dbProvider.AddInParameter(dbCommand, "NUMESTACION", DbType.Int32, regHojaEolADTO.NumEstacionMet);
            dbProvider.AddInParameter(dbCommand, "SERIEVELVIENTO", DbType.String, regHojaEolADTO.SerieVelViento);
            dbProvider.AddInParameter(dbCommand, "PERIODODISANIO", DbType.Int32, regHojaEolADTO.PeriodoDisAnio);
            dbProvider.AddInParameter(dbCommand, "ESTUDIOGEOLOGICO", DbType.String, regHojaEolADTO.EstudioGeologico);
            dbProvider.AddInParameter(dbCommand, "PERFODIAMANTINAS", DbType.Int32, regHojaEolADTO.PerfoDiamantinas);
            dbProvider.AddInParameter(dbCommand, "NUMCALICATAS", DbType.Int32, regHojaEolADTO.NumCalicatas);
            dbProvider.AddInParameter(dbCommand, "ESTUDIOTOPOGRAFICO", DbType.String, regHojaEolADTO.EstudioTopografico);
            dbProvider.AddInParameter(dbCommand, "LEVANTAMIENTOTOPOGRAFICO", DbType.Int32, regHojaEolADTO.LevantamientoTopografico);
            dbProvider.AddInParameter(dbCommand, "POTENCIAINSTALADA", DbType.Int32, regHojaEolADTO.PotenciaInstalada);
            dbProvider.AddInParameter(dbCommand, "VELVIENTOINSTALADA", DbType.Int32, regHojaEolADTO.VelVientoInstalada);
            dbProvider.AddInParameter(dbCommand, "HORPOTNOMINAL", DbType.Int32, regHojaEolADTO.HorPotNominal);
            dbProvider.AddInParameter(dbCommand, "VELDESCONEXION", DbType.Int32, regHojaEolADTO.VelDesconexion);
            dbProvider.AddInParameter(dbCommand, "VELCONEXION", DbType.Int32, regHojaEolADTO.VelConexion);
            dbProvider.AddInParameter(dbCommand, "TIPOCONTRCENTRAL", DbType.String, regHojaEolADTO.TipoContrCentral);
            dbProvider.AddInParameter(dbCommand, "RANGOVELTURBINA", DbType.String, regHojaEolADTO.RangoVelTurbina);
            dbProvider.AddInParameter(dbCommand, "TIPOTURBINA", DbType.String, regHojaEolADTO.TipoTurbina);
            dbProvider.AddInParameter(dbCommand, "ENERGIAANUAL", DbType.Int32, regHojaEolADTO.EnergiaAnual);
            dbProvider.AddInParameter(dbCommand, "TIPOPARQELICO", DbType.String, regHojaEolADTO.TipoParqEolico);
            dbProvider.AddInParameter(dbCommand, "TIPOTECGENERADOR", DbType.String, regHojaEolADTO.TipoTecGenerador);
            dbProvider.AddInParameter(dbCommand, "NUMPALTURBINA", DbType.Int32, regHojaEolADTO.NumPalTurbina);
            dbProvider.AddInParameter(dbCommand, "DIAROTOR", DbType.Int32, regHojaEolADTO.DiaRotor);
            dbProvider.AddInParameter(dbCommand, "LONGPALA", DbType.Int32, regHojaEolADTO.LongPala);
            dbProvider.AddInParameter(dbCommand, "ALTURATORRE", DbType.Int32, regHojaEolADTO.AlturaTorre);
            dbProvider.AddInParameter(dbCommand, "POTNOMGENERADOR", DbType.Int32, regHojaEolADTO.PotNomGenerador);
            dbProvider.AddInParameter(dbCommand, "NUMUNIDADES", DbType.Int32, regHojaEolADTO.NumUnidades);
            dbProvider.AddInParameter(dbCommand, "NUMPOLOS", DbType.Int32, regHojaEolADTO.NumPolos);
            dbProvider.AddInParameter(dbCommand, "TENSIONGENERACION", DbType.Int32, regHojaEolADTO.TensionGeneracion);
            dbProvider.AddInParameter(dbCommand, "BESS", DbType.String, regHojaEolADTO.Bess);
            dbProvider.AddInParameter(dbCommand, "ENERGIAMAXBAT", DbType.Int32, regHojaEolADTO.EnergiaMaxBat);
            dbProvider.AddInParameter(dbCommand, "POTENCIAMAXBAT", DbType.Int32, regHojaEolADTO.PotenciaMaxBat);
            dbProvider.AddInParameter(dbCommand, "EFICARGABAT", DbType.Int32, regHojaEolADTO.EfiCargaBat);
            dbProvider.AddInParameter(dbCommand, "EFIDESCARGABAT", DbType.Int32, regHojaEolADTO.EfiDescargaBat);
            dbProvider.AddInParameter(dbCommand, "TIEMPMAXREGULACION", DbType.Int32, regHojaEolADTO.TiempoMaxRegulacion);
            dbProvider.AddInParameter(dbCommand, "RAMPCARGDESCARG", DbType.Int32, regHojaEolADTO.RampaCargDescarg);
            dbProvider.AddInParameter(dbCommand, "TENSIONKV", DbType.Int32, regHojaEolADTO.TensionKv);
            dbProvider.AddInParameter(dbCommand, "LONGITUDKM", DbType.Int32, regHojaEolADTO.LongitudKm);
            dbProvider.AddInParameter(dbCommand, "NUMTERNAS", DbType.Int32, regHojaEolADTO.NumTernas);
            dbProvider.AddInParameter(dbCommand, "NOMBRESUBESTACION", DbType.String, regHojaEolADTO.NombreSubestacion);
            dbProvider.AddInParameter(dbCommand, "NOMBRESUBOTRO", DbType.String, regHojaEolADTO.NombreSubOtro);
            dbProvider.AddInParameter(dbCommand, "PERFIL", DbType.String, regHojaEolADTO.Perfil);
            dbProvider.AddInParameter(dbCommand, "PREFACTIBILIDAD", DbType.String, regHojaEolADTO.Prefactibilidad);
            dbProvider.AddInParameter(dbCommand, "FACTIBILIDAD", DbType.String, regHojaEolADTO.Factibilidad);
            dbProvider.AddInParameter(dbCommand, "ESTUDIODEFINITIVO", DbType.String, regHojaEolADTO.EstudioDefinitivo);
            dbProvider.AddInParameter(dbCommand, "EIA", DbType.String, regHojaEolADTO.Eia);
            dbProvider.AddInParameter(dbCommand, "FECHAINICIOCONSTRUCCION", DbType.String, regHojaEolADTO.FechaInicioConstruccion);
            dbProvider.AddInParameter(dbCommand, "PERIODOCONSTRUCCION", DbType.Int32, regHojaEolADTO.PeriodoConstruccion);
            dbProvider.AddInParameter(dbCommand, "FECHAOPERACIONCOMERCIAL", DbType.String, regHojaEolADTO.FechaOperacionComercial);
            dbProvider.AddInParameter(dbCommand, "COMENTARIOS", DbType.String, regHojaEolADTO.Comentarios);
            dbProvider.AddInParameter(dbCommand, "USU_MODIFICACION", DbType.String, regHojaEolADTO.UsuModificacion);
            dbProvider.AddInParameter(dbCommand, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "CENTRALCODI", DbType.Int32, regHojaEolADTO.CentralACodi);
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        object ObtenerValorOrDefault(object valor, Type tipo)
        {
            DateTime fechaMinimaValida = DateTime.Now;
            if (valor == null || (valor is DateTime && (DateTime)valor == DateTime.MinValue))
            {
                if (tipo == typeof(int) || tipo == typeof(int?))
                {
                    return valor ?? DBNull.Value;
                }
                else if (tipo == typeof(string))
                {
                    return "";
                }
                else if (tipo == typeof(DateTime) || tipo == typeof(DateTime?))
                {
                    return fechaMinimaValida;
                }
            }
            return valor;
        }


        public List<RegHojaEolADTO> GetRegHojaEolAByFilter(string plancodi, string empresa, string estado)
        {
            List<RegHojaEolADTO> oblist = new List<RegHojaEolADTO>();
            string query = $@"
                SELECT CGB.*, TR.EMPRESANOM, TR.PROYNOMBRE, TR.PROYDESCRIPCION, TP.TIPONOMBRE, TF.TIPOFINOMBRE, TR.PROYCONFIDENCIAL  FROM CAM_CENEOLIHOJAA CGB
                INNER JOIN CAM_TRNSMPROYECTO TR ON TR.PROYCODI = CGB.PROYCODI
                INNER JOIN CAM_PLANTRANSMISION PL ON PL.PLANCODI = TR.PLANCODI
                INNER JOIN CAM_TIPOPROYECTO TP ON TP.TIPOCODI = TR.TIPOCODI
                LEFT JOIN CAM_TIPOFICHAPROYECTO TF ON TF.TIPOFICODI = TR.TIPOFICODI
                WHERE TR.PERICODI  IN ({plancodi}) AND 
                PL.CODEMPRESA IN ({empresa})  AND 
                CGB.IND_DEL = 0 AND 
                PL.PLANESTADO ='{estado}'
                ORDER BY TR.PERICODI, CGB.PROYCODI,PL.CODEMPRESA, CGB.CENTRALACODI ASC";

            DbCommand commandHoja = dbProvider.GetSqlStringCommand(query);
            //dbProvider.ExecuteNonQuery(commandHoja);
            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                while (dr.Read())
                {
                    RegHojaEolADTO ob = new RegHojaEolADTO();
                    ob.CentralACodi = !dr.IsDBNull(dr.GetOrdinal("CENTRALACODI")) ? dr.GetInt32(dr.GetOrdinal("CENTRALACODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.CentralNombre = !dr.IsDBNull(dr.GetOrdinal("CENTRALNOMBRE")) ? dr.GetString(dr.GetOrdinal("CENTRALNOMBRE")) : string.Empty;
                    ob.Distrito = !dr.IsDBNull(dr.GetOrdinal("DISTRITO")) ? dr.GetString(dr.GetOrdinal("DISTRITO")) : string.Empty;
                    ob.Propietario = !dr.IsDBNull(dr.GetOrdinal("PROPIETARIO")) ? dr.GetString(dr.GetOrdinal("PROPIETARIO")) : string.Empty;
                    ob.OtroPropietario = !dr.IsDBNull(dr.GetOrdinal("OTROPROPIETARIO")) ? dr.GetString(dr.GetOrdinal("OTROPROPIETARIO")) : string.Empty;
                    ob.SocioOperador = !dr.IsDBNull(dr.GetOrdinal("SOCIOOPERADOR")) ? dr.GetString(dr.GetOrdinal("SOCIOOPERADOR")) : string.Empty;
                    ob.SocioInversionista = !dr.IsDBNull(dr.GetOrdinal("SOCIOINVERSIONISTA")) ? dr.GetString(dr.GetOrdinal("SOCIOINVERSIONISTA")) : string.Empty;
                    ob.ConcesionTemporal = !dr.IsDBNull(dr.GetOrdinal("CONCESIONTEMPORAL")) ? dr.GetString(dr.GetOrdinal("CONCESIONTEMPORAL")) : string.Empty;
                    ob.FechaConcesionTemporal = !dr.IsDBNull(dr.GetOrdinal("FECHACONCESIONTEMPORAL")) ? dr.GetDateTime(dr.GetOrdinal("FECHACONCESIONTEMPORAL")) : (DateTime?)null;
                    ob.TipoConcesionActual = !dr.IsDBNull(dr.GetOrdinal("TIPOCONCESIONACTUAL")) ? dr.GetString(dr.GetOrdinal("TIPOCONCESIONACTUAL")) : string.Empty;
                    ob.FechaConcesionActual = !dr.IsDBNull(dr.GetOrdinal("FECHACONCESIONACTUAL")) ? dr.GetDateTime(dr.GetOrdinal("FECHACONCESIONACTUAL")) : (DateTime?)null;
                    ob.NombreEstacionMet = !dr.IsDBNull(dr.GetOrdinal("NOMBREESTACIONMET")) ? dr.GetString(dr.GetOrdinal("NOMBREESTACIONMET")) : string.Empty;
                    ob.NumEstacionMet = !dr.IsDBNull(dr.GetOrdinal("NUMESTACIONMET")) ? dr.GetInt32(dr.GetOrdinal("NUMESTACIONMET")) : 0;
                    ob.SerieVelViento = !dr.IsDBNull(dr.GetOrdinal("SERIEVELVIENTO")) ? dr.GetString(dr.GetOrdinal("SERIEVELVIENTO")) : string.Empty;
                    ob.PeriodoDisAnio = !dr.IsDBNull(dr.GetOrdinal("PERIODODISANIO")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("PERIODODISANIO")) : null;
                    ob.EstudioGeologico = !dr.IsDBNull(dr.GetOrdinal("ESTUDIOGEOLOGICO")) ? dr.GetString(dr.GetOrdinal("ESTUDIOGEOLOGICO")) : string.Empty;
                    ob.PerfoDiamantinas = !dr.IsDBNull(dr.GetOrdinal("PERFODIAMANTINAS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("PERFODIAMANTINAS")) : null;
                    ob.NumCalicatas = !dr.IsDBNull(dr.GetOrdinal("NUMCALICATAS")) ? dr.GetInt32(dr.GetOrdinal("NUMCALICATAS")) : 0;
                    ob.EstudioTopografico = !dr.IsDBNull(dr.GetOrdinal("ESTUDIOTOPOGRAFICO")) ? dr.GetString(dr.GetOrdinal("ESTUDIOTOPOGRAFICO")) : string.Empty;
                    ob.LevantamientoTopografico = !dr.IsDBNull(dr.GetOrdinal("LEVANTAMIENTOTOPOGRAFICO")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("LEVANTAMIENTOTOPOGRAFICO")) : null;
                    ob.PotenciaInstalada = !dr.IsDBNull(dr.GetOrdinal("POTENCIAINSTALADA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("POTENCIAINSTALADA")) : null;
                    ob.VelVientoInstalada = !dr.IsDBNull(dr.GetOrdinal("VELVIENTOINSTALADA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("VELVIENTOINSTALADA")) : null;
                    ob.HorPotNominal = !dr.IsDBNull(dr.GetOrdinal("HORPOTNOMINAL")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("HORPOTNOMINAL")) : null;
                    ob.VelDesconexion = !dr.IsDBNull(dr.GetOrdinal("VELDESCONEXION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("VELDESCONEXION")) : null;
                    ob.VelConexion = !dr.IsDBNull(dr.GetOrdinal("VELCONEXION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("VELCONEXION")) : null;
                    ob.TipoContrCentral = !dr.IsDBNull(dr.GetOrdinal("TIPOCONTRCENTRAL")) ? dr.GetString(dr.GetOrdinal("TIPOCONTRCENTRAL")) : string.Empty;
                    ob.RangoVelTurbina = !dr.IsDBNull(dr.GetOrdinal("RANGOVELTURBINA")) ? dr.GetString(dr.GetOrdinal("RANGOVELTURBINA")) : string.Empty;
                    ob.TipoTurbina = !dr.IsDBNull(dr.GetOrdinal("TIPOTURBINA")) ? dr.GetString(dr.GetOrdinal("TIPOTURBINA")) : string.Empty;
                    ob.EnergiaAnual = !dr.IsDBNull(dr.GetOrdinal("ENERGIAANUAL")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("ENERGIAANUAL")) : null;
                    ob.TipoParqEolico = !dr.IsDBNull(dr.GetOrdinal("TIPOPARQELICO")) ? dr.GetString(dr.GetOrdinal("TIPOPARQELICO")) : string.Empty;
                    ob.TipoTecGenerador = !dr.IsDBNull(dr.GetOrdinal("TIPOTECGENERADOR")) ? dr.GetString(dr.GetOrdinal("TIPOTECGENERADOR")) : string.Empty;
                    ob.NumPalTurbina = !dr.IsDBNull(dr.GetOrdinal("NUMPALTURBINA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("NUMPALTURBINA")) : null;
                    ob.DiaRotor = !dr.IsDBNull(dr.GetOrdinal("DIAROTOR")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("DIAROTOR")) : null;
                    ob.LongPala = !dr.IsDBNull(dr.GetOrdinal("LONGPALA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("LONGPALA")) : null;
                    ob.AlturaTorre = !dr.IsDBNull(dr.GetOrdinal("ALTURATORRE")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("ALTURATORRE")) : null;
                    ob.PotNomGenerador = !dr.IsDBNull(dr.GetOrdinal("POTNOMGENERADOR")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("POTNOMGENERADOR")) : null;
                    ob.NumUnidades = !dr.IsDBNull(dr.GetOrdinal("NUMUNIDADES")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("NUMUNIDADES")) : null;
                    ob.NumPolos = !dr.IsDBNull(dr.GetOrdinal("NUMPOLOS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("NUMPOLOS")) : null;
                    ob.TensionGeneracion = !dr.IsDBNull(dr.GetOrdinal("TENSIONGENERACION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("TENSIONGENERACION")) : null;
                    ob.Bess = !dr.IsDBNull(dr.GetOrdinal("BESS")) ? dr.GetString(dr.GetOrdinal("BESS")) : string.Empty;
                    ob.EnergiaMaxBat = !dr.IsDBNull(dr.GetOrdinal("ENERGIAMAXBAT")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("ENERGIAMAXBAT")) : null;
                    ob.PotenciaMaxBat = !dr.IsDBNull(dr.GetOrdinal("POTENCIAMAXBAT")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("POTENCIAMAXBAT")) : null;
                    ob.EfiCargaBat = !dr.IsDBNull(dr.GetOrdinal("EFICARGABAT")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("EFICARGABAT")) : null;
                    ob.EfiDescargaBat = !dr.IsDBNull(dr.GetOrdinal("EFIDESCARGABAT")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("EFIDESCARGABAT")) : null;
                    ob.TiempoMaxRegulacion = !dr.IsDBNull(dr.GetOrdinal("TIEMPMAXREGULACION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("TIEMPMAXREGULACION")) : null;
                    ob.RampaCargDescarg = !dr.IsDBNull(dr.GetOrdinal("RAMPCARGDESCARG")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("RAMPCARGDESCARG")) : null;
                    ob.TensionKv = !dr.IsDBNull(dr.GetOrdinal("TENSIONKV")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("TENSIONKV")) : null;
                    ob.LongitudKm = !dr.IsDBNull(dr.GetOrdinal("LONGITUDKM")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("LONGITUDKM")) : null;
                    ob.NumTernas = !dr.IsDBNull(dr.GetOrdinal("NUMTERNAS")) ? dr.GetInt32(dr.GetOrdinal("NUMTERNAS")) : 0;
                    ob.NombreSubestacion = !dr.IsDBNull(dr.GetOrdinal("NOMBRESUBESTACION")) ? dr.GetString(dr.GetOrdinal("NOMBRESUBESTACION")) : string.Empty;
                    ob.NombreSubOtro = !dr.IsDBNull(dr.GetOrdinal("NOMBRESUBOTRO")) ? dr.GetString(dr.GetOrdinal("NOMBRESUBOTRO")) : string.Empty;
                    ob.Perfil = !dr.IsDBNull(dr.GetOrdinal("PERFIL")) ? dr.GetString(dr.GetOrdinal("PERFIL")) : string.Empty;
                    ob.Prefactibilidad = !dr.IsDBNull(dr.GetOrdinal("PREFACTIBILIDAD")) ? dr.GetString(dr.GetOrdinal("PREFACTIBILIDAD")) : string.Empty;
                    ob.Factibilidad = !dr.IsDBNull(dr.GetOrdinal("FACTIBILIDAD")) ? dr.GetString(dr.GetOrdinal("FACTIBILIDAD")) : string.Empty;
                    ob.EstudioDefinitivo = !dr.IsDBNull(dr.GetOrdinal("ESTUDIODEFINITIVO")) ? dr.GetString(dr.GetOrdinal("ESTUDIODEFINITIVO")) : string.Empty;
                    ob.Eia = !dr.IsDBNull(dr.GetOrdinal("EIA")) ? dr.GetString(dr.GetOrdinal("EIA")) : string.Empty;
                    ob.FechaInicioConstruccion = !dr.IsDBNull(dr.GetOrdinal("FECHAINICIOCONSTRUCCION")) ? dr.GetString(dr.GetOrdinal("FECHAINICIOCONSTRUCCION")) : string.Empty;
                    ob.PeriodoConstruccion = !dr.IsDBNull(dr.GetOrdinal("PERIODOCONSTRUCCION")) ? dr.GetInt32(dr.GetOrdinal("PERIODOCONSTRUCCION")) : 0;
                    ob.FechaOperacionComercial = !dr.IsDBNull(dr.GetOrdinal("FECHAOPERACIONCOMERCIAL")) ? dr.GetString(dr.GetOrdinal("FECHAOPERACIONCOMERCIAL")) : string.Empty;
                    ob.Comentarios = !dr.IsDBNull(dr.GetOrdinal("COMENTARIOS")) ? dr.GetString(dr.GetOrdinal("COMENTARIOS")) : string.Empty;
                    ob.UsuCreacion = dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? null : dr.GetString(dr.GetOrdinal("USU_CREACION"));
                    ob.FecCreacion = dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? (DateTime?)null : dr.GetDateTime(dr.GetOrdinal("FEC_CREACION"));
                    ob.Empresa = dr.IsDBNull(dr.GetOrdinal("EMPRESANOM")) ? null : dr.GetString(dr.GetOrdinal("EMPRESANOM"));
                    ob.NombreProyecto = dr.IsDBNull(dr.GetOrdinal("PROYNOMBRE")) ? null : dr.GetString(dr.GetOrdinal("PROYNOMBRE"));
                    ob.DetalleProyecto = dr.IsDBNull(dr.GetOrdinal("PROYDESCRIPCION")) ? null : dr.GetString(dr.GetOrdinal("PROYDESCRIPCION"));
                    ob.TipoProyecto = dr.IsDBNull(dr.GetOrdinal("TIPONOMBRE")) ? null : dr.GetString(dr.GetOrdinal("TIPONOMBRE"));
                    ob.SubTipoProyecto = dr.IsDBNull(dr.GetOrdinal("TIPOFINOMBRE")) ? null : dr.GetString(dr.GetOrdinal("TIPOFINOMBRE"));
                    ob.Confidencial = dr.IsDBNull(dr.GetOrdinal("PROYCONFIDENCIAL")) ? null : dr.GetString(dr.GetOrdinal("PROYCONFIDENCIAL"));
                    oblist.Add(ob);
                }

            }
            return oblist;
        }


    }


}
