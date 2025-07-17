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
    public class CamSolHojaARepository : RepositoryBase, ICamSolHojaARepository
    {

        public CamSolHojaARepository(string strConn) : base(strConn) { }

        CamSolHojaAHelper Helper = new CamSolHojaAHelper();

        public List<SolHojaADTO> GetSolHojaAProyCodi(int proyCodi)
        {
            List<SolHojaADTO> solHojaADTOs = new List<SolHojaADTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetSolHojaAProyCodi);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.String, proyCodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SolHojaADTO ob = new SolHojaADTO();
                    ob.Solhojaacodi = !dr.IsDBNull(dr.GetOrdinal("SOLHOJAACODI")) ? dr.GetInt32(dr.GetOrdinal("SOLHOJAACODI")) : 0;
                    ob.Proycodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Centralnombre = dr.IsDBNull(dr.GetOrdinal("CENTRALNOMBRE")) ? null : dr.GetString(dr.GetOrdinal("CENTRALNOMBRE"));
                    ob.Distrito = dr.IsDBNull(dr.GetOrdinal("DISTRITO")) ? null : dr.GetString(dr.GetOrdinal("DISTRITO"));
                    ob.Propietario = dr.IsDBNull(dr.GetOrdinal("PROPIETARIO")) ? null : dr.GetString(dr.GetOrdinal("PROPIETARIO"));
                    ob.Otro = dr.IsDBNull(dr.GetOrdinal("OTRO")) ? null : dr.GetString(dr.GetOrdinal("OTRO"));
                    ob.Sociooperador = dr.IsDBNull(dr.GetOrdinal("SOCIOOPERADOR")) ? null : dr.GetString(dr.GetOrdinal("SOCIOOPERADOR"));
                    ob.Socioinversionista = dr.IsDBNull(dr.GetOrdinal("SOCIOINVERSIONISTA")) ? null : dr.GetString(dr.GetOrdinal("SOCIOINVERSIONISTA"));
                    ob.Concesiontemporal = dr.IsDBNull(dr.GetOrdinal("CONCESIONTEMPORAL")) ? null : dr.GetString(dr.GetOrdinal("CONCESIONTEMPORAL"));
                    ob.Tipoconcesionact = dr.IsDBNull(dr.GetOrdinal("TIPOCONCESIONACT")) ? null : dr.GetString(dr.GetOrdinal("TIPOCONCESIONACT"));
                    ob.Fechaconcesiontem = dr.IsDBNull(dr.GetOrdinal("FECHACONCESIONTEM")) ? (DateTime?)null : dr.GetDateTime(dr.GetOrdinal("FECHACONCESIONTEM"));
                    ob.Fechaconcesionact = dr.IsDBNull(dr.GetOrdinal("FECHACONCESIONACT")) ? (DateTime?)null : dr.GetDateTime(dr.GetOrdinal("FECHACONCESIONACT"));
                    ob.Nomestacion = dr.IsDBNull(dr.GetOrdinal("NOMESTACION")) ? null : dr.GetString(dr.GetOrdinal("NOMESTACION"));
                    ob.Serieradiacion = dr.IsDBNull(dr.GetOrdinal("SERIERADIACION")) ? null : dr.GetString(dr.GetOrdinal("SERIERADIACION"));
                    ob.Potinstnom = !dr.IsDBNull(dr.GetOrdinal("POTINSTNOM")) ? dr.GetDecimal(dr.GetOrdinal("POTINSTNOM")) : (decimal?)null;
                    ob.Ntotalmodfv = !dr.IsDBNull(dr.GetOrdinal("NTOTALMODFV")) ? dr.GetDecimal(dr.GetOrdinal("NTOTALMODFV")) : (decimal?)null;
                    ob.Horutilequ = !dr.IsDBNull(dr.GetOrdinal("HORUTILEQU")) ? dr.GetDecimal(dr.GetOrdinal("HORUTILEQU")) : (decimal?)null;
                    ob.Eneestanual = !dr.IsDBNull(dr.GetOrdinal("ENEESTANUAL")) ? dr.GetDecimal(dr.GetOrdinal("ENEESTANUAL")) : (decimal?)null;
                    ob.Facplantaact = !dr.IsDBNull(dr.GetOrdinal("FACPLANTAACT")) ? dr.GetDecimal(dr.GetOrdinal("FACPLANTAACT")) : (decimal?)null;
                    ob.Tecnologia = dr.IsDBNull(dr.GetOrdinal("TECNOLOGIA")) ? null : dr.GetString(dr.GetOrdinal("TECNOLOGIA"));
                    ob.Potenciapico = !dr.IsDBNull(dr.GetOrdinal("POTENCIAPICO")) ? dr.GetDecimal(dr.GetOrdinal("POTENCIAPICO")) : (decimal?)null;
                    ob.Nivelradsol = !dr.IsDBNull(dr.GetOrdinal("NIVELRADSOL")) ? dr.GetDecimal(dr.GetOrdinal("NIVELRADSOL")) : (decimal?)null;
                    ob.Seguidorsol = dr.IsDBNull(dr.GetOrdinal("SEGUIDORSOL")) ? null : dr.GetString(dr.GetOrdinal("SEGUIDORSOL"));
                    ob.Volpunmax = !dr.IsDBNull(dr.GetOrdinal("VOLPUNMAX")) ? dr.GetDecimal(dr.GetOrdinal("VOLPUNMAX")) : (decimal?)null;
                    ob.Intpunmax = !dr.IsDBNull(dr.GetOrdinal("INTPUNMAX")) ? dr.GetDecimal(dr.GetOrdinal("INTPUNMAX")) : (decimal?)null;
                    ob.Modelo = dr.IsDBNull(dr.GetOrdinal("MODELO")) ? null : dr.GetString(dr.GetOrdinal("MODELO"));
                    ob.Entpotmax = !dr.IsDBNull(dr.GetOrdinal("ENTPOTMAX")) ? dr.GetDecimal(dr.GetOrdinal("ENTPOTMAX")) : (decimal?)null;
                    ob.Salpotmax = !dr.IsDBNull(dr.GetOrdinal("SALPOTMAX")) ? dr.GetDecimal(dr.GetOrdinal("SALPOTMAX")) : (decimal?)null;
                    ob.Siscontro = dr.IsDBNull(dr.GetOrdinal("SISCONTRO")) ? null : dr.GetString(dr.GetOrdinal("SISCONTRO"));
                    ob.Baterias = dr.IsDBNull(dr.GetOrdinal("BATERIAS")) ? null : dr.GetString(dr.GetOrdinal("BATERIAS"));
                    ob.Enemaxbat = !dr.IsDBNull(dr.GetOrdinal("ENEMAXBAT")) ? dr.GetDecimal(dr.GetOrdinal("ENEMAXBAT")) : (decimal?)null;
                    ob.Potmaxbat = !dr.IsDBNull(dr.GetOrdinal("POTMAXBAT")) ? dr.GetDecimal(dr.GetOrdinal("POTMAXBAT")) : (decimal?)null;
                    ob.Eficargamax = !dr.IsDBNull(dr.GetOrdinal("EFICARGAMAX")) ? dr.GetDecimal(dr.GetOrdinal("EFICARGAMAX")) : (decimal?)null;
                    ob.Efidesbat = !dr.IsDBNull(dr.GetOrdinal("EFIDESBAT")) ? dr.GetDecimal(dr.GetOrdinal("EFIDESBAT")) : (decimal?)null;
                    ob.Timmaxreg = !dr.IsDBNull(dr.GetOrdinal("TIMMAXREG")) ? dr.GetDecimal(dr.GetOrdinal("TIMMAXREG")) : (decimal?)null;
                    ob.Rampascardes = !dr.IsDBNull(dr.GetOrdinal("RAMPASCARDES")) ? dr.GetDecimal(dr.GetOrdinal("RAMPASCARDES")) : (decimal?)null;
                    ob.Tension = !dr.IsDBNull(dr.GetOrdinal("TENSION")) ? dr.GetDecimal(dr.GetOrdinal("TENSION")) : (decimal?)null;
                    ob.Longitud = !dr.IsDBNull(dr.GetOrdinal("LONGITUD")) ? dr.GetDecimal(dr.GetOrdinal("LONGITUD")) : (decimal?)null;
                    ob.Numternas = !dr.IsDBNull(dr.GetOrdinal("NUMTERNAS")) ? dr.GetDecimal(dr.GetOrdinal("NUMTERNAS")) : (decimal?)null;
                    ob.Nombsubest = dr.IsDBNull(dr.GetOrdinal("NOMSUBEST")) ? null : dr.GetString(dr.GetOrdinal("NOMSUBEST"));
                    ob.Perfil = dr.IsDBNull(dr.GetOrdinal("PERFIL")) ? null : dr.GetString(dr.GetOrdinal("PERFIL"));
                    ob.Prefact = dr.IsDBNull(dr.GetOrdinal("PREFACT")) ? null : dr.GetString(dr.GetOrdinal("PREFACT"));
                    ob.Factibilidad = dr.IsDBNull(dr.GetOrdinal("FACTIBILIDAD")) ? null : dr.GetString(dr.GetOrdinal("FACTIBILIDAD"));
                    ob.Estdefinitivo = dr.IsDBNull(dr.GetOrdinal("ESTDEFINITIVO")) ? null : dr.GetString(dr.GetOrdinal("ESTDEFINITIVO"));
                    ob.Eia = dr.IsDBNull(dr.GetOrdinal("EIA")) ? null : dr.GetString(dr.GetOrdinal("EIA"));
                    ob.Fecinicioconst = dr.IsDBNull(dr.GetOrdinal("FECINICIOCONST")) ? null : dr.GetString(dr.GetOrdinal("FECINICIOCONST"));
                    ob.Perconstruccion = dr.IsDBNull(dr.GetOrdinal("PERCONSTRUCCION")) ? null : dr.GetString(dr.GetOrdinal("PERCONSTRUCCION"));
                    ob.Fecoperacioncom = dr.IsDBNull(dr.GetOrdinal("FECOPERACIONCOM")) ? null : dr.GetString(dr.GetOrdinal("FECOPERACIONCOM"));
                    ob.Comentarios = dr.IsDBNull(dr.GetOrdinal("COMENTARIOS")) ? null : dr.GetString(dr.GetOrdinal("COMENTARIOS"));
                    ob.UsuCreacion = dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? null : dr.GetString(dr.GetOrdinal("USU_CREACION"));
                    ob.FecCreacion = dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? (DateTime?)null : dr.GetDateTime(dr.GetOrdinal("FEC_CREACION"));
                    ob.IndDel = dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? null : dr.GetString(dr.GetOrdinal("IND_DEL"));
                    solHojaADTOs.Add(ob);
                }
            }

            return solHojaADTOs;
        }


        public bool SaveSolHojaA(SolHojaADTO solHojaADTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveSolHojaA);
            dbProvider.AddInParameter(dbCommand, "SOLHOJAACODI", DbType.Int32, ObtenerValorOrDefault(solHojaADTO.Solhojaacodi, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, ObtenerValorOrDefault(solHojaADTO.Proycodi, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "CENTRALNOMBRE", DbType.String, ObtenerValorOrDefault(solHojaADTO.Centralnombre, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "DISTRITO", DbType.String, ObtenerValorOrDefault(solHojaADTO.Distrito, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "PROPIETARIO", DbType.String, ObtenerValorOrDefault(solHojaADTO.Propietario, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "OTRO", DbType.String, ObtenerValorOrDefault(solHojaADTO.Otro, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "SOCIOOPERADOR", DbType.String, ObtenerValorOrDefault(solHojaADTO.Sociooperador, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "SOCIOINVERSIONISTA", DbType.String, ObtenerValorOrDefault(solHojaADTO.Socioinversionista, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "CONCESIONTEMPORAL", DbType.String, ObtenerValorOrDefault(solHojaADTO.Concesiontemporal, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "TIPOCONCESIONACT", DbType.String, ObtenerValorOrDefault(solHojaADTO.Tipoconcesionact, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "FECHACONCESIONTEM", DbType.Date, solHojaADTO.Fechaconcesiontem);
            dbProvider.AddInParameter(dbCommand, "FECHACONCESIONACT", DbType.Date, solHojaADTO.Fechaconcesionact);
            dbProvider.AddInParameter(dbCommand, "NOMESTACION", DbType.String, ObtenerValorOrDefault(solHojaADTO.Nomestacion, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "SERIERADIACION", DbType.String, ObtenerValorOrDefault(solHojaADTO.Serieradiacion, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "POTINSTNOM", DbType.Decimal, ObtenerValorOrDefault(solHojaADTO.Potinstnom, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "NTOTALMODFV", DbType.Decimal, ObtenerValorOrDefault(solHojaADTO.Ntotalmodfv, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "HORUTILEQU", DbType.Decimal, ObtenerValorOrDefault(solHojaADTO.Horutilequ, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "ENEESTANUAL", DbType.Decimal, ObtenerValorOrDefault(solHojaADTO.Eneestanual, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "FACPLANTAACT", DbType.Decimal, ObtenerValorOrDefault(solHojaADTO.Facplantaact, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "TECNOLOGIA", DbType.String, ObtenerValorOrDefault(solHojaADTO.Tecnologia, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "POTENCIAPICO", DbType.Decimal, ObtenerValorOrDefault(solHojaADTO.Potenciapico, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "NIVELRADSOL", DbType.Decimal, ObtenerValorOrDefault(solHojaADTO.Nivelradsol, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "SEGUIDORSOL", DbType.String, ObtenerValorOrDefault(solHojaADTO.Seguidorsol, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "VOLPUNMAX", DbType.Decimal, ObtenerValorOrDefault(solHojaADTO.Volpunmax, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "INTPUNMAX", DbType.Decimal, ObtenerValorOrDefault(solHojaADTO.Intpunmax, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "MODELO", DbType.String, ObtenerValorOrDefault(solHojaADTO.Modelo, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "ENTPOTMAX", DbType.Decimal, ObtenerValorOrDefault(solHojaADTO.Entpotmax, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "SALPOTMAX", DbType.Decimal, ObtenerValorOrDefault(solHojaADTO.Salpotmax, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "SISCONTRO", DbType.String, ObtenerValorOrDefault(solHojaADTO.Siscontro, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "BATERIAS", DbType.String, ObtenerValorOrDefault(solHojaADTO.Baterias, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "ENEMAXBAT", DbType.Decimal, ObtenerValorOrDefault(solHojaADTO.Enemaxbat, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "POTMAXBAT", DbType.Decimal, ObtenerValorOrDefault(solHojaADTO.Potmaxbat, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "EFICARGAMAX", DbType.Decimal, ObtenerValorOrDefault(solHojaADTO.Eficargamax, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "EFIDESBAT", DbType.Decimal, ObtenerValorOrDefault(solHojaADTO.Efidesbat, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "TIMMAXREG", DbType.Decimal, ObtenerValorOrDefault(solHojaADTO.Timmaxreg, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "RAMPASCARDES", DbType.Decimal, ObtenerValorOrDefault(solHojaADTO.Rampascardes, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "TENSION", DbType.Decimal, ObtenerValorOrDefault(solHojaADTO.Tension, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "LONGITUD", DbType.Decimal, ObtenerValorOrDefault(solHojaADTO.Longitud, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "NUMTERNAS", DbType.Decimal, ObtenerValorOrDefault(solHojaADTO.Numternas, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "NOMSUBEST", DbType.String, ObtenerValorOrDefault(solHojaADTO.Nombsubest, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "PERFIL", DbType.String, ObtenerValorOrDefault(solHojaADTO.Perfil, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "PREFACT", DbType.String, ObtenerValorOrDefault(solHojaADTO.Prefact, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "FACTIBILIDAD", DbType.String, ObtenerValorOrDefault(solHojaADTO.Factibilidad, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "ESTDEFINITIVO", DbType.String, ObtenerValorOrDefault(solHojaADTO.Estdefinitivo, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "EIA", DbType.String, ObtenerValorOrDefault(solHojaADTO.Eia, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "FECINICIOCONST", DbType.String, ObtenerValorOrDefault(solHojaADTO.Fecinicioconst, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "PERCONSTRUCCION", DbType.String, ObtenerValorOrDefault(solHojaADTO.Perconstruccion, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "FECOPERACIONCOM", DbType.String, ObtenerValorOrDefault(solHojaADTO.Fecoperacioncom, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "COMENTARIOS", DbType.String, ObtenerValorOrDefault(solHojaADTO.Comentarios, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, ObtenerValorOrDefault(solHojaADTO.UsuCreacion, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.Date, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, Constantes.IndDel);

            dbProvider.ExecuteNonQuery(dbCommand);

            return true;
        }

        public bool DeleteSolHojaAById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteSolHojaAById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);
            return true;
        }

        public int GetLastSolHojaAId()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastSolHojaAId);
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

        public SolHojaADTO GetSolHojaAById(int id)
        {
            SolHojaADTO ob = new SolHojaADTO();
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(Helper.SqlGetSolHojaAById);
            dbProvider.AddInParameter(commandHoja, "PROYCODI", DbType.Int32, id);
            dbProvider.AddInParameter(commandHoja, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.ExecuteNonQuery(commandHoja);
            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                if (dr.Read())
                {
                    ob.Solhojaacodi = !dr.IsDBNull(dr.GetOrdinal("SOLHOJAACODI")) ? dr.GetInt32(dr.GetOrdinal("SOLHOJAACODI")) : 0;
                    ob.Proycodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Centralnombre = dr.IsDBNull(dr.GetOrdinal("CENTRALNOMBRE")) ? null : dr.GetString(dr.GetOrdinal("CENTRALNOMBRE"));
                    ob.Distrito = dr.IsDBNull(dr.GetOrdinal("DISTRITO")) ? null : dr.GetString(dr.GetOrdinal("DISTRITO"));
                    ob.Propietario = dr.IsDBNull(dr.GetOrdinal("PROPIETARIO")) ? null : dr.GetString(dr.GetOrdinal("PROPIETARIO"));
                    ob.Otro = dr.IsDBNull(dr.GetOrdinal("OTRO")) ? null : dr.GetString(dr.GetOrdinal("OTRO"));
                    ob.Sociooperador = dr.IsDBNull(dr.GetOrdinal("SOCIOOPERADOR")) ? null : dr.GetString(dr.GetOrdinal("SOCIOOPERADOR"));
                    ob.Socioinversionista = dr.IsDBNull(dr.GetOrdinal("SOCIOINVERSIONISTA")) ? null : dr.GetString(dr.GetOrdinal("SOCIOINVERSIONISTA"));
                    ob.Concesiontemporal = dr.IsDBNull(dr.GetOrdinal("CONCESIONTEMPORAL")) ? null : dr.GetString(dr.GetOrdinal("CONCESIONTEMPORAL"));
                    ob.Tipoconcesionact = dr.IsDBNull(dr.GetOrdinal("TIPOCONCESIONACT")) ? null : dr.GetString(dr.GetOrdinal("TIPOCONCESIONACT"));
                    ob.Fechaconcesiontem = dr.IsDBNull(dr.GetOrdinal("FECHACONCESIONTEM")) ? (DateTime?)null : dr.GetDateTime(dr.GetOrdinal("FECHACONCESIONTEM"));
                    ob.Fechaconcesionact = dr.IsDBNull(dr.GetOrdinal("FECHACONCESIONACT")) ? (DateTime?)null : dr.GetDateTime(dr.GetOrdinal("FECHACONCESIONACT"));
                    ob.Nomestacion = dr.IsDBNull(dr.GetOrdinal("NOMESTACION")) ? null : dr.GetString(dr.GetOrdinal("NOMESTACION"));
                    ob.Serieradiacion = dr.IsDBNull(dr.GetOrdinal("SERIERADIACION")) ? null : dr.GetString(dr.GetOrdinal("SERIERADIACION"));
                    ob.Potinstnom = !dr.IsDBNull(dr.GetOrdinal("POTINSTNOM")) ? dr.GetDecimal(dr.GetOrdinal("POTINSTNOM")) : (decimal?)null;
                    ob.Ntotalmodfv = !dr.IsDBNull(dr.GetOrdinal("NTOTALMODFV")) ? dr.GetDecimal(dr.GetOrdinal("NTOTALMODFV")) : (decimal?)null;
                    ob.Horutilequ = !dr.IsDBNull(dr.GetOrdinal("HORUTILEQU")) ? dr.GetDecimal(dr.GetOrdinal("HORUTILEQU")) : (decimal?)null;
                    ob.Eneestanual = !dr.IsDBNull(dr.GetOrdinal("ENEESTANUAL")) ? dr.GetDecimal(dr.GetOrdinal("ENEESTANUAL")) : (decimal?)null;
                    ob.Facplantaact = !dr.IsDBNull(dr.GetOrdinal("FACPLANTAACT")) ? dr.GetDecimal(dr.GetOrdinal("FACPLANTAACT")) : (decimal?)null;
                    ob.Tecnologia = dr.IsDBNull(dr.GetOrdinal("TECNOLOGIA")) ? null : dr.GetString(dr.GetOrdinal("TECNOLOGIA"));
                    ob.Potenciapico = !dr.IsDBNull(dr.GetOrdinal("POTENCIAPICO")) ? dr.GetDecimal(dr.GetOrdinal("POTENCIAPICO")) : (decimal?)null;
                    ob.Nivelradsol = !dr.IsDBNull(dr.GetOrdinal("NIVELRADSOL")) ? dr.GetDecimal(dr.GetOrdinal("NIVELRADSOL")) : (decimal?)null;
                    ob.Seguidorsol = dr.IsDBNull(dr.GetOrdinal("SEGUIDORSOL")) ? null : dr.GetString(dr.GetOrdinal("SEGUIDORSOL"));
                    ob.Volpunmax = !dr.IsDBNull(dr.GetOrdinal("VOLPUNMAX")) ? dr.GetDecimal(dr.GetOrdinal("VOLPUNMAX")) : (decimal?)null;
                    ob.Intpunmax = !dr.IsDBNull(dr.GetOrdinal("INTPUNMAX")) ? dr.GetDecimal(dr.GetOrdinal("INTPUNMAX")) : (decimal?)null;
                    ob.Modelo = dr.IsDBNull(dr.GetOrdinal("MODELO")) ? null : dr.GetString(dr.GetOrdinal("MODELO"));
                    ob.Entpotmax = !dr.IsDBNull(dr.GetOrdinal("ENTPOTMAX")) ? dr.GetDecimal(dr.GetOrdinal("ENTPOTMAX")) : (decimal?)null;
                    ob.Salpotmax = !dr.IsDBNull(dr.GetOrdinal("SALPOTMAX")) ? dr.GetDecimal(dr.GetOrdinal("SALPOTMAX")) : (decimal?)null;
                    ob.Siscontro = dr.IsDBNull(dr.GetOrdinal("SISCONTRO")) ? null : dr.GetString(dr.GetOrdinal("SISCONTRO"));
                    ob.Baterias = dr.IsDBNull(dr.GetOrdinal("BATERIAS")) ? null : dr.GetString(dr.GetOrdinal("BATERIAS"));
                    ob.Enemaxbat = !dr.IsDBNull(dr.GetOrdinal("ENEMAXBAT")) ? dr.GetDecimal(dr.GetOrdinal("ENEMAXBAT")) : (decimal?)null;
                    ob.Potmaxbat = !dr.IsDBNull(dr.GetOrdinal("POTMAXBAT")) ? dr.GetDecimal(dr.GetOrdinal("POTMAXBAT")) : (decimal?)null;
                    ob.Eficargamax = !dr.IsDBNull(dr.GetOrdinal("EFICARGAMAX")) ? dr.GetDecimal(dr.GetOrdinal("EFICARGAMAX")) : (decimal?)null;
                    ob.Efidesbat = !dr.IsDBNull(dr.GetOrdinal("EFIDESBAT")) ? dr.GetDecimal(dr.GetOrdinal("EFIDESBAT")) : (decimal?)null;
                    ob.Timmaxreg = !dr.IsDBNull(dr.GetOrdinal("TIMMAXREG")) ? dr.GetDecimal(dr.GetOrdinal("TIMMAXREG")) : (decimal?)null;
                    ob.Rampascardes = !dr.IsDBNull(dr.GetOrdinal("RAMPASCARDES")) ? dr.GetDecimal(dr.GetOrdinal("RAMPASCARDES")) : (decimal?)null;
                    ob.Tension = !dr.IsDBNull(dr.GetOrdinal("TENSION")) ? dr.GetDecimal(dr.GetOrdinal("TENSION")) : (decimal?)null;
                    ob.Longitud = !dr.IsDBNull(dr.GetOrdinal("LONGITUD")) ? dr.GetDecimal(dr.GetOrdinal("LONGITUD")) : (decimal?)null;
                    ob.Numternas = !dr.IsDBNull(dr.GetOrdinal("NUMTERNAS")) ? dr.GetDecimal(dr.GetOrdinal("NUMTERNAS")) : (decimal?)null;
                    ob.Nombsubest = dr.IsDBNull(dr.GetOrdinal("NOMSUBEST")) ? null : dr.GetString(dr.GetOrdinal("NOMSUBEST"));
                    ob.Perfil = dr.IsDBNull(dr.GetOrdinal("PERFIL")) ? null : dr.GetString(dr.GetOrdinal("PERFIL"));
                    ob.Prefact = dr.IsDBNull(dr.GetOrdinal("PREFACT")) ? null : dr.GetString(dr.GetOrdinal("PREFACT"));
                    ob.Factibilidad = dr.IsDBNull(dr.GetOrdinal("FACTIBILIDAD")) ? null : dr.GetString(dr.GetOrdinal("FACTIBILIDAD"));
                    ob.Estdefinitivo = dr.IsDBNull(dr.GetOrdinal("ESTDEFINITIVO")) ? null : dr.GetString(dr.GetOrdinal("ESTDEFINITIVO"));
                    ob.Eia = dr.IsDBNull(dr.GetOrdinal("EIA")) ? null : dr.GetString(dr.GetOrdinal("EIA"));
                    ob.Fecinicioconst = dr.IsDBNull(dr.GetOrdinal("FECINICIOCONST")) ? null : dr.GetString(dr.GetOrdinal("FECINICIOCONST"));
                    ob.Perconstruccion = dr.IsDBNull(dr.GetOrdinal("PERCONSTRUCCION")) ? null : dr.GetString(dr.GetOrdinal("PERCONSTRUCCION"));
                    ob.Fecoperacioncom = dr.IsDBNull(dr.GetOrdinal("FECOPERACIONCOM")) ? null : dr.GetString(dr.GetOrdinal("FECOPERACIONCOM"));
                    ob.Comentarios = dr.IsDBNull(dr.GetOrdinal("COMENTARIOS")) ? null : dr.GetString(dr.GetOrdinal("COMENTARIOS"));
                    ob.UsuCreacion = dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? null : dr.GetString(dr.GetOrdinal("USU_CREACION"));
                    ob.FecCreacion = dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? (DateTime?)null : dr.GetDateTime(dr.GetOrdinal("FEC_CREACION"));
                    ob.IndDel = dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? null : dr.GetString(dr.GetOrdinal("IND_DEL"));
                }

            }
            return ob;
        }

        public bool UpdateSolHojaA(SolHojaADTO solHojaADTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlUpdateSolHojaA);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, ObtenerValorOrDefault(solHojaADTO.Proycodi, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "CENTRALNOMBRE", DbType.String, ObtenerValorOrDefault(solHojaADTO.Centralnombre, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "DISTRITO", DbType.String, ObtenerValorOrDefault(solHojaADTO.Distrito, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "PROPIETARIO", DbType.String, ObtenerValorOrDefault(solHojaADTO.Propietario, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "OTRO", DbType.String, ObtenerValorOrDefault(solHojaADTO.Otro, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "SOCIOOPERADOR", DbType.String, ObtenerValorOrDefault(solHojaADTO.Sociooperador, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "SOCIOINVERSIONISTA", DbType.String, ObtenerValorOrDefault(solHojaADTO.Socioinversionista, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "CONCESIONTEMPORAL", DbType.String, ObtenerValorOrDefault(solHojaADTO.Concesiontemporal, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "TIPOCONCESIONACT", DbType.String, ObtenerValorOrDefault(solHojaADTO.Tipoconcesionact, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "FECHACONCESIONTEM", DbType.Date, solHojaADTO.Fechaconcesiontem);
            dbProvider.AddInParameter(dbCommand, "FECHACONCESIONACT", DbType.Date, solHojaADTO.Fechaconcesionact);
            dbProvider.AddInParameter(dbCommand, "NOMESTACION", DbType.String, ObtenerValorOrDefault(solHojaADTO.Nomestacion, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "SERIERADIACION", DbType.String, ObtenerValorOrDefault(solHojaADTO.Serieradiacion, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "POTINSTNOM", DbType.Decimal, ObtenerValorOrDefault(solHojaADTO.Potinstnom, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "NTOTALMODFV", DbType.Decimal, ObtenerValorOrDefault(solHojaADTO.Ntotalmodfv, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "HORUTILEQU", DbType.Decimal, ObtenerValorOrDefault(solHojaADTO.Horutilequ, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "ENEESTANUAL", DbType.Decimal, ObtenerValorOrDefault(solHojaADTO.Eneestanual, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "FACPLANTAACT", DbType.Decimal, ObtenerValorOrDefault(solHojaADTO.Facplantaact, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "TECNOLOGIA", DbType.String, ObtenerValorOrDefault(solHojaADTO.Tecnologia, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "POTENCIAPICO", DbType.Decimal, ObtenerValorOrDefault(solHojaADTO.Potenciapico, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "NIVELRADSOL", DbType.Decimal, ObtenerValorOrDefault(solHojaADTO.Nivelradsol, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "SEGUIDORSOL", DbType.String, ObtenerValorOrDefault(solHojaADTO.Seguidorsol, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "VOLPUNMAX", DbType.Decimal, ObtenerValorOrDefault(solHojaADTO.Volpunmax, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "INTPUNMAX", DbType.Decimal, ObtenerValorOrDefault(solHojaADTO.Intpunmax, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "MODELO", DbType.String, ObtenerValorOrDefault(solHojaADTO.Modelo, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "ENTPOTMAX", DbType.Decimal, ObtenerValorOrDefault(solHojaADTO.Entpotmax, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "SALPOTMAX", DbType.Decimal, ObtenerValorOrDefault(solHojaADTO.Salpotmax, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "SISCONTRO", DbType.String, ObtenerValorOrDefault(solHojaADTO.Siscontro, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "BATERIAS", DbType.String, ObtenerValorOrDefault(solHojaADTO.Baterias, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "ENEMAXBAT", DbType.Decimal, ObtenerValorOrDefault(solHojaADTO.Enemaxbat, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "POTMAXBAT", DbType.Decimal, ObtenerValorOrDefault(solHojaADTO.Potmaxbat, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "EFICARGAMAX", DbType.Decimal, ObtenerValorOrDefault(solHojaADTO.Eficargamax, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "EFIDESBAT", DbType.Decimal, ObtenerValorOrDefault(solHojaADTO.Efidesbat, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "TIMMAXREG", DbType.Decimal, ObtenerValorOrDefault(solHojaADTO.Timmaxreg, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "RAMPASCARDES", DbType.Decimal, ObtenerValorOrDefault(solHojaADTO.Rampascardes, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "TENSION", DbType.Decimal, ObtenerValorOrDefault(solHojaADTO.Tension, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "LONGITUD", DbType.Decimal, ObtenerValorOrDefault(solHojaADTO.Longitud, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "NUMTERNAS", DbType.Decimal, ObtenerValorOrDefault(solHojaADTO.Numternas, typeof(decimal?)));
            dbProvider.AddInParameter(dbCommand, "NOMSUBEST", DbType.String, ObtenerValorOrDefault(solHojaADTO.Nombsubest, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "PERFIL", DbType.String, ObtenerValorOrDefault(solHojaADTO.Perfil, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "PREFACT", DbType.String, ObtenerValorOrDefault(solHojaADTO.Prefact, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "FACTIBILIDAD", DbType.String, ObtenerValorOrDefault(solHojaADTO.Factibilidad, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "ESTDEFINITIVO", DbType.String, ObtenerValorOrDefault(solHojaADTO.Estdefinitivo, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "EIA", DbType.String, ObtenerValorOrDefault(solHojaADTO.Eia, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "FECINICIOCONST", DbType.String, ObtenerValorOrDefault(solHojaADTO.Fecinicioconst, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "PERCONSTRUCCION", DbType.String, ObtenerValorOrDefault(solHojaADTO.Perconstruccion, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "FECOPERACIONCOM", DbType.String, ObtenerValorOrDefault(solHojaADTO.Fecoperacioncom, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "COMENTARIOS", DbType.String, ObtenerValorOrDefault(solHojaADTO.Comentarios, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "USU_MODIFICACION", DbType.String, ObtenerValorOrDefault(solHojaADTO.UsuModificacion, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "FEC_MODIFICACION", DbType.Date, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.AddInParameter(dbCommand, "SOLHOJAACODI", DbType.Int32, ObtenerValorOrDefault(solHojaADTO.Solhojaacodi, typeof(int)));
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
                    return 0;
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


        public List<SolHojaADTO> GetSolHojaAByFilter(string plancodi, string empresa, string estado)
        {
            List<SolHojaADTO> oblist = new List<SolHojaADTO>();
            string query = $@"
                SELECT CGB.*, TR.EMPRESANOM, TR.PROYNOMBRE, TR.PROYDESCRIPCION, TP.TIPONOMBRE, TF.TIPOFINOMBRE, TR.PROYCONFIDENCIAL  FROM CAM_SOLHOJAA CGB
                INNER JOIN CAM_TRNSMPROYECTO TR ON TR.PROYCODI = CGB.PROYCODI
                INNER JOIN CAM_PLANTRANSMISION PL ON PL.PLANCODI = TR.PLANCODI
                INNER JOIN CAM_TIPOPROYECTO TP ON TP.TIPOCODI = TR.TIPOCODI
                LEFT JOIN CAM_TIPOFICHAPROYECTO TF ON TF.TIPOFICODI = TR.TIPOFICODI
                WHERE TR.PERICODI  IN ({plancodi}) AND 
                PL.CODEMPRESA IN ({empresa})  AND 
                CGB.IND_DEL = 0 AND 
                PL.PLANESTADO ='{estado}'
                ORDER BY TR.PERICODI, CGB.PROYCODI,PL.CODEMPRESA, CGB.SOLHOJAACODI ASC";
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(commandHoja);
            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                while (dr.Read())
                {
                    SolHojaADTO ob = new SolHojaADTO();
                    ob.Solhojaacodi = !dr.IsDBNull(dr.GetOrdinal("SOLHOJAACODI")) ? dr.GetInt32(dr.GetOrdinal("SOLHOJAACODI")) : 0;
                    ob.Proycodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Centralnombre = dr.IsDBNull(dr.GetOrdinal("CENTRALNOMBRE")) ? null : dr.GetString(dr.GetOrdinal("CENTRALNOMBRE"));
                    ob.Distrito = dr.IsDBNull(dr.GetOrdinal("DISTRITO")) ? null : dr.GetString(dr.GetOrdinal("DISTRITO"));
                    ob.Propietario = dr.IsDBNull(dr.GetOrdinal("PROPIETARIO")) ? null : dr.GetString(dr.GetOrdinal("PROPIETARIO"));
                    ob.Otro = dr.IsDBNull(dr.GetOrdinal("OTRO")) ? null : dr.GetString(dr.GetOrdinal("OTRO"));
                    ob.Sociooperador = dr.IsDBNull(dr.GetOrdinal("SOCIOOPERADOR")) ? null : dr.GetString(dr.GetOrdinal("SOCIOOPERADOR"));
                    ob.Socioinversionista = dr.IsDBNull(dr.GetOrdinal("SOCIOINVERSIONISTA")) ? null : dr.GetString(dr.GetOrdinal("SOCIOINVERSIONISTA"));
                    ob.Concesiontemporal = dr.IsDBNull(dr.GetOrdinal("CONCESIONTEMPORAL")) ? null : dr.GetString(dr.GetOrdinal("CONCESIONTEMPORAL"));
                    ob.Tipoconcesionact = dr.IsDBNull(dr.GetOrdinal("TIPOCONCESIONACT")) ? null : dr.GetString(dr.GetOrdinal("TIPOCONCESIONACT"));
                    ob.Fechaconcesiontem = dr.IsDBNull(dr.GetOrdinal("FECHACONCESIONTEM")) ? (DateTime?)null : dr.GetDateTime(dr.GetOrdinal("FECHACONCESIONTEM"));
                    ob.Fechaconcesionact = dr.IsDBNull(dr.GetOrdinal("FECHACONCESIONACT")) ? (DateTime?)null : dr.GetDateTime(dr.GetOrdinal("FECHACONCESIONACT"));
                    ob.Nomestacion = dr.IsDBNull(dr.GetOrdinal("NOMESTACION")) ? null : dr.GetString(dr.GetOrdinal("NOMESTACION"));
                    ob.Serieradiacion = dr.IsDBNull(dr.GetOrdinal("SERIERADIACION")) ? null : dr.GetString(dr.GetOrdinal("SERIERADIACION"));
                    ob.Potinstnom = !dr.IsDBNull(dr.GetOrdinal("POTINSTNOM")) ? dr.GetDecimal(dr.GetOrdinal("POTINSTNOM")) : (decimal?)null;
                    ob.Ntotalmodfv = !dr.IsDBNull(dr.GetOrdinal("NTOTALMODFV")) ? dr.GetDecimal(dr.GetOrdinal("NTOTALMODFV")) : (decimal?)null;
                    ob.Horutilequ = !dr.IsDBNull(dr.GetOrdinal("HORUTILEQU")) ? dr.GetDecimal(dr.GetOrdinal("HORUTILEQU")) : (decimal?)null;
                    ob.Eneestanual = !dr.IsDBNull(dr.GetOrdinal("ENEESTANUAL")) ? dr.GetDecimal(dr.GetOrdinal("ENEESTANUAL")) : (decimal?)null;
                    ob.Facplantaact = !dr.IsDBNull(dr.GetOrdinal("FACPLANTAACT")) ? dr.GetDecimal(dr.GetOrdinal("FACPLANTAACT")) : (decimal?)null;
                    ob.Tecnologia = dr.IsDBNull(dr.GetOrdinal("TECNOLOGIA")) ? null : dr.GetString(dr.GetOrdinal("TECNOLOGIA"));
                    ob.Potenciapico = !dr.IsDBNull(dr.GetOrdinal("POTENCIAPICO")) ? dr.GetDecimal(dr.GetOrdinal("POTENCIAPICO")) : (decimal?)null;
                    ob.Nivelradsol = !dr.IsDBNull(dr.GetOrdinal("NIVELRADSOL")) ? dr.GetDecimal(dr.GetOrdinal("NIVELRADSOL")) : (decimal?)null;
                    ob.Seguidorsol = dr.IsDBNull(dr.GetOrdinal("SEGUIDORSOL")) ? null : dr.GetString(dr.GetOrdinal("SEGUIDORSOL"));
                    ob.Volpunmax = !dr.IsDBNull(dr.GetOrdinal("VOLPUNMAX")) ? dr.GetDecimal(dr.GetOrdinal("VOLPUNMAX")) : (decimal?)null;
                    ob.Intpunmax = !dr.IsDBNull(dr.GetOrdinal("INTPUNMAX")) ? dr.GetDecimal(dr.GetOrdinal("INTPUNMAX")) : (decimal?)null;
                    ob.Modelo = dr.IsDBNull(dr.GetOrdinal("MODELO")) ? null : dr.GetString(dr.GetOrdinal("MODELO"));
                    ob.Entpotmax = !dr.IsDBNull(dr.GetOrdinal("ENTPOTMAX")) ? dr.GetDecimal(dr.GetOrdinal("ENTPOTMAX")) : (decimal?)null;
                    ob.Salpotmax = !dr.IsDBNull(dr.GetOrdinal("SALPOTMAX")) ? dr.GetDecimal(dr.GetOrdinal("SALPOTMAX")) : (decimal?)null;
                    ob.Siscontro = dr.IsDBNull(dr.GetOrdinal("SISCONTRO")) ? null : dr.GetString(dr.GetOrdinal("SISCONTRO"));
                    ob.Baterias = dr.IsDBNull(dr.GetOrdinal("BATERIAS")) ? null : dr.GetString(dr.GetOrdinal("BATERIAS"));
                    ob.Enemaxbat = !dr.IsDBNull(dr.GetOrdinal("ENEMAXBAT")) ? dr.GetDecimal(dr.GetOrdinal("ENEMAXBAT")) : (decimal?)null;
                    ob.Potmaxbat = !dr.IsDBNull(dr.GetOrdinal("POTMAXBAT")) ? dr.GetDecimal(dr.GetOrdinal("POTMAXBAT")) : (decimal?)null;
                    ob.Eficargamax = !dr.IsDBNull(dr.GetOrdinal("EFICARGAMAX")) ? dr.GetDecimal(dr.GetOrdinal("EFICARGAMAX")) : (decimal?)null;
                    ob.Efidesbat = !dr.IsDBNull(dr.GetOrdinal("EFIDESBAT")) ? dr.GetDecimal(dr.GetOrdinal("EFIDESBAT")) : (decimal?)null;
                    ob.Timmaxreg = !dr.IsDBNull(dr.GetOrdinal("TIMMAXREG")) ? dr.GetDecimal(dr.GetOrdinal("TIMMAXREG")) : (decimal?)null;
                    ob.Rampascardes = !dr.IsDBNull(dr.GetOrdinal("RAMPASCARDES")) ? dr.GetDecimal(dr.GetOrdinal("RAMPASCARDES")) : (decimal?)null;
                    ob.Tension = !dr.IsDBNull(dr.GetOrdinal("TENSION")) ? dr.GetDecimal(dr.GetOrdinal("TENSION")) : (decimal?)null;
                    ob.Longitud = !dr.IsDBNull(dr.GetOrdinal("LONGITUD")) ? dr.GetDecimal(dr.GetOrdinal("LONGITUD")) : (decimal?)null;
                    ob.Numternas = !dr.IsDBNull(dr.GetOrdinal("NUMTERNAS")) ? dr.GetDecimal(dr.GetOrdinal("NUMTERNAS")) : (decimal?)null;
                    ob.Nombsubest = dr.IsDBNull(dr.GetOrdinal("NOMSUBEST")) ? null : dr.GetString(dr.GetOrdinal("NOMSUBEST"));
                    ob.Perfil = dr.IsDBNull(dr.GetOrdinal("PERFIL")) ? null : dr.GetString(dr.GetOrdinal("PERFIL"));
                    ob.Prefact = dr.IsDBNull(dr.GetOrdinal("PREFACT")) ? null : dr.GetString(dr.GetOrdinal("PREFACT"));
                    ob.Factibilidad = dr.IsDBNull(dr.GetOrdinal("FACTIBILIDAD")) ? null : dr.GetString(dr.GetOrdinal("FACTIBILIDAD"));
                    ob.Estdefinitivo = dr.IsDBNull(dr.GetOrdinal("ESTDEFINITIVO")) ? null : dr.GetString(dr.GetOrdinal("ESTDEFINITIVO"));
                    ob.Eia = dr.IsDBNull(dr.GetOrdinal("EIA")) ? null : dr.GetString(dr.GetOrdinal("EIA"));
                    ob.Fecinicioconst = dr.IsDBNull(dr.GetOrdinal("FECINICIOCONST")) ? null : dr.GetString(dr.GetOrdinal("FECINICIOCONST"));
                    ob.Perconstruccion = dr.IsDBNull(dr.GetOrdinal("PERCONSTRUCCION")) ? null : dr.GetString(dr.GetOrdinal("PERCONSTRUCCION"));
                    ob.Fecoperacioncom = dr.IsDBNull(dr.GetOrdinal("FECOPERACIONCOM")) ? null : dr.GetString(dr.GetOrdinal("FECOPERACIONCOM"));
                    ob.Comentarios = dr.IsDBNull(dr.GetOrdinal("COMENTARIOS")) ? null : dr.GetString(dr.GetOrdinal("COMENTARIOS"));
                    ob.UsuCreacion = dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? null : dr.GetString(dr.GetOrdinal("USU_CREACION"));
                    ob.FecCreacion = dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? (DateTime?)null : dr.GetDateTime(dr.GetOrdinal("FEC_CREACION"));
                    ob.IndDel = dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? null : dr.GetString(dr.GetOrdinal("IND_DEL"));
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
