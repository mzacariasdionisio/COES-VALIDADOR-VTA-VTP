using COES.Base.Core;
using COES.Dominio.DTO.Campania;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Campania;
using COES.Framework.Base.Tools;
using COES.Infraestructura.Datos.Helper;
using COES.Infraestructura.Datos.Helper.Campania;
using COES.Infraestructura.Datos.Resources;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace COES.Infraestructura.Datos.Repositorio.Campania
{
    public class CamRegHojaCCTTARepository: RepositoryBase, ICamRegHojaCCTTARepository
    {

        public CamRegHojaCCTTARepository(string strConn) : base(strConn){}

        CamRegHojaCCTTAHelper Helper = new CamRegHojaCCTTAHelper();

        public List<RegHojaCCTTADTO> GetRegHojaCCTTAProyCodi(int proyCodi)
        {
            List<RegHojaCCTTADTO> regHojaCCTTADTOs = new List<RegHojaCCTTADTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetRegHojaCCTTAProyCodi);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.String, proyCodi);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RegHojaCCTTADTO ob = new RegHojaCCTTADTO();
                    ob.Centralcodi = !dr.IsDBNull(dr.GetOrdinal("CENTRALCODI")) ? dr.GetInt32(dr.GetOrdinal("CENTRALCODI")) : 0;
                    ob.Proycodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Centralnombre = !dr.IsDBNull(dr.GetOrdinal("CENTRALNOMBRE")) ? dr.GetString(dr.GetOrdinal("CENTRALNOMBRE")) : string.Empty;
                    ob.Distrito = !dr.IsDBNull(dr.GetOrdinal("DISTRITO")) ? dr.GetString(dr.GetOrdinal("DISTRITO")) : string.Empty;
                    ob.Propietario = !dr.IsDBNull(dr.GetOrdinal("PROPIETARIO")) ? dr.GetString(dr.GetOrdinal("PROPIETARIO")) : string.Empty;
                    ob.Sociooperador = !dr.IsDBNull(dr.GetOrdinal("SOCIOOPERADOR")) ? dr.GetString(dr.GetOrdinal("SOCIOOPERADOR")) : string.Empty;
                    ob.Socioinversionista = !dr.IsDBNull(dr.GetOrdinal("SOCIOINVERSIONISTA")) ? dr.GetString(dr.GetOrdinal("SOCIOINVERSIONISTA")) : string.Empty;
                    ob.Tipoconcesionactual = !dr.IsDBNull(dr.GetOrdinal("TIPOCONCESIONACTUAL")) ? dr.GetString(dr.GetOrdinal("TIPOCONCESIONACTUAL")) : string.Empty;
                    ob.Fechaconcesionactual = !dr.IsDBNull(dr.GetOrdinal("FECHACONCESIONACTUAL")) ? dr.GetDateTime(dr.GetOrdinal("FECHACONCESIONACTUAL")) : (DateTime?)null;
                    ob.Potenciainstalada = !dr.IsDBNull(dr.GetOrdinal("POTENCIAINSTALADA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("POTENCIAINSTALADA")) : null;
                    ob.Potenciamaxima = !dr.IsDBNull(dr.GetOrdinal("POTENCIAMAXIMA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("POTENCIAMAXIMA")) : null;
                    ob.Potenciaminima = !dr.IsDBNull(dr.GetOrdinal("POTENCIAMINIMA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("POTENCIAMINIMA")) : null;
                    ob.Combustibletipo = !dr.IsDBNull(dr.GetOrdinal("COMBUSTIBLETIPO")) ? dr.GetString(dr.GetOrdinal("COMBUSTIBLETIPO")) : string.Empty;
                    ob.CombustibletipoOtro = !dr.IsDBNull(dr.GetOrdinal("COMBUSTIBLETIPOOTRO")) ?dr.GetString(dr.GetOrdinal("COMBUSTIBLETIPOOTRO")) : string.Empty;
                    ob.Podercalorificoinferior = !dr.IsDBNull(dr.GetOrdinal("PODERCALORIFICOINFERIOR")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("PODERCALORIFICOINFERIOR")) : null;
                    ob.Undpci = !dr.IsDBNull(dr.GetOrdinal("UNDPCI")) ? dr.GetString(dr.GetOrdinal("UNDPCI")) : string.Empty;
                    ob.Podercalorificosuperior = !dr.IsDBNull(dr.GetOrdinal("PODERCALORIFICOSUPERIOR")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("PODERCALORIFICOSUPERIOR")) : null;
                    ob.Undpcs = !dr.IsDBNull(dr.GetOrdinal("UNDPCS")) ? dr.GetString(dr.GetOrdinal("UNDPCS")) : string.Empty;
                    ob.Costocombustible = !dr.IsDBNull(dr.GetOrdinal("COSTOCOMBUSTIBLE")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("COSTOCOMBUSTIBLE")) : null;
                    ob.Undcomb = !dr.IsDBNull(dr.GetOrdinal("UNDCOMB")) ? dr.GetString(dr.GetOrdinal("UNDCOMB")) : string.Empty;
                    ob.Costotratamientocombustible = !dr.IsDBNull(dr.GetOrdinal("COSTOTRATAMIENTOCOMBUSTIBLE")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("COSTOTRATAMIENTOCOMBUSTIBLE")) : null;
                    ob.Undtrtcomb = !dr.IsDBNull(dr.GetOrdinal("UNDTRTCOMB")) ? dr.GetString(dr.GetOrdinal("UNDTRTCOMB")) : string.Empty;
                    ob.Costotransportecombustible = !dr.IsDBNull(dr.GetOrdinal("COSTOTRANSPORTECOMBUSTIBLE")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("COSTOTRANSPORTECOMBUSTIBLE")) : null;
                    ob.Undtrnspcomb = !dr.IsDBNull(dr.GetOrdinal("UNDTRNSPCOMB")) ? dr.GetString(dr.GetOrdinal("UNDTRNSPCOMB")) : string.Empty;
                    ob.Costovariablenocombustible = !dr.IsDBNull(dr.GetOrdinal("COSTOVARIABLENOCOMBUSTIBLE")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("COSTOVARIABLENOCOMBUSTIBLE")) : null;
                    ob.Undvarncmb = !dr.IsDBNull(dr.GetOrdinal("UNDVARNCMB")) ? dr.GetString(dr.GetOrdinal("UNDVARNCMB")) : string.Empty;
                    ob.Costoinversioninicial = !dr.IsDBNull(dr.GetOrdinal("COSTOINVERSIONINICIAL")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("COSTOINVERSIONINICIAL")) : null;
                    ob.Undinvinic = !dr.IsDBNull(dr.GetOrdinal("UNDINVINIC")) ? dr.GetString(dr.GetOrdinal("UNDINVINIC")) : string.Empty;
                    ob.Rendimientoplantacondicion = !dr.IsDBNull(dr.GetOrdinal("RENDIMIENTOPLANTACONDICION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("RENDIMIENTOPLANTACONDICION")) : null;
                    ob.Undrendcnd = !dr.IsDBNull(dr.GetOrdinal("UNDRENDCND")) ? dr.GetString(dr.GetOrdinal("UNDRENDCND")) : string.Empty;
                    ob.Consespificacondicion = !dr.IsDBNull(dr.GetOrdinal("CONSESPIFICACONDICION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("CONSESPIFICACONDICION")) : null;
                    ob.Undconscp = !dr.IsDBNull(dr.GetOrdinal("UNDCONSCP")) ? dr.GetString(dr.GetOrdinal("UNDCONSCP")) : string.Empty;
                    ob.Tipomotortermico = !dr.IsDBNull(dr.GetOrdinal("TIPOMOTORTERMICO")) ? dr.GetString(dr.GetOrdinal("TIPOMOTORTERMICO")) : string.Empty;
                    ob.Velnomrotacion = !dr.IsDBNull(dr.GetOrdinal("VELNOMROTACION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("VELNOMROTACION")) : null;
                    ob.Potmotortermico = !dr.IsDBNull(dr.GetOrdinal("POTMOTORTERMICO")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("POTMOTORTERMICO")) : null;
                    ob.Nummotorestermicos = !dr.IsDBNull(dr.GetOrdinal("NUMMOTORESTERMICOS")) ? dr.GetString(dr.GetOrdinal("NUMMOTORESTERMICOS")) : string.Empty;
                    ob.Potgenerador = !dr.IsDBNull(dr.GetOrdinal("POTGENERADOR")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("POTGENERADOR")) : null;
                    ob.Numgeneradores = !dr.IsDBNull(dr.GetOrdinal("NUMGENERADORES")) ? dr.GetString(dr.GetOrdinal("NUMGENERADORES")) : string.Empty;
                    ob.Tensiongeneracion = !dr.IsDBNull(dr.GetOrdinal("TENSIONGENERACION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("TENSIONGENERACION")) : null;
                    ob.Rendimientogenerador = !dr.IsDBNull(dr.GetOrdinal("RENDIMIENTOGENERADOR")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("RENDIMIENTOGENERADOR")) : null;
                    ob.Tensionkv = !dr.IsDBNull(dr.GetOrdinal("TENSIONKV")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("TENSIONKV")) : null;
                    ob.Longitudkm = !dr.IsDBNull(dr.GetOrdinal("LONGITUDKM")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("LONGITUDKM")) : null;
                    ob.Numternas = !dr.IsDBNull(dr.GetOrdinal("NUMTERNAS")) ? dr.GetString(dr.GetOrdinal("NUMTERNAS")) : string.Empty;
                    ob.Nombresubestacion = !dr.IsDBNull(dr.GetOrdinal("NOMBRESUBESTACION")) ? dr.GetString(dr.GetOrdinal("NOMBRESUBESTACION")) : string.Empty;
                    ob.Perfil = !dr.IsDBNull(dr.GetOrdinal("PERFIL")) ? dr.GetString(dr.GetOrdinal("PERFIL")) : string.Empty;
                    ob.Prefactibilidad = !dr.IsDBNull(dr.GetOrdinal("PREFACTIBILIDAD")) ? dr.GetString(dr.GetOrdinal("PREFACTIBILIDAD")) : string.Empty;
                    ob.Factibilidad = !dr.IsDBNull(dr.GetOrdinal("FACTIBILIDAD")) ? dr.GetString(dr.GetOrdinal("FACTIBILIDAD")) : string.Empty;
                    ob.Estudiodefinitivo = !dr.IsDBNull(dr.GetOrdinal("ESTUDIODEFINITIVO")) ? dr.GetString(dr.GetOrdinal("ESTUDIODEFINITIVO")) : string.Empty;
                    ob.Eia = !dr.IsDBNull(dr.GetOrdinal("EIA")) ? dr.GetString(dr.GetOrdinal("EIA")) : string.Empty;
                    ob.Fechainicioconstruccion = !dr.IsDBNull(dr.GetOrdinal("FECHAINICIOCONSTRUCCION")) ? dr.GetString(dr.GetOrdinal("FECHAINICIOCONSTRUCCION")) : string.Empty;
                    ob.Periodoconstruccion = !dr.IsDBNull(dr.GetOrdinal("PERIODOCONSTRUCCION")) ? dr.GetString(dr.GetOrdinal("PERIODOCONSTRUCCION")) : string.Empty;
                    ob.Fechaoperacioncomercial = !dr.IsDBNull(dr.GetOrdinal("FECHAOPERACIONCOMERCIAL")) ? dr.GetString(dr.GetOrdinal("FECHAOPERACIONCOMERCIAL")) : string.Empty;
                    ob.Comentarios = !dr.IsDBNull(dr.GetOrdinal("COMENTARIOS")) ? dr.GetString(dr.GetOrdinal("COMENTARIOS")) : string.Empty;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                    regHojaCCTTADTOs.Add(ob);
                }
            }

            return regHojaCCTTADTOs;
        }

        public bool SaveRegHojaCCTTA(RegHojaCCTTADTO regHojaCCTTADTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveRegHojaCCTTA);
            dbProvider.AddInParameter(dbCommand, "CENTRALCODI", DbType.Int32, ObtenerValorOrDefault(regHojaCCTTADTO.Centralcodi, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, ObtenerValorOrDefault(regHojaCCTTADTO.Proycodi, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "CENTRALNOMBRE", DbType.String, ObtenerValorOrDefault(regHojaCCTTADTO.Centralnombre, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "DISTRITO", DbType.String, ObtenerValorOrDefault(regHojaCCTTADTO.Distrito, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "PROPIETARIO", DbType.String, ObtenerValorOrDefault(regHojaCCTTADTO.Propietario, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "SOCIOOPERADOR", DbType.String, ObtenerValorOrDefault(regHojaCCTTADTO.Sociooperador, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "SOCIOINVERSIONISTA", DbType.String, ObtenerValorOrDefault(regHojaCCTTADTO.Socioinversionista, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "TIPOCONCESIONACTUAL", DbType.String, ObtenerValorOrDefault(regHojaCCTTADTO.Tipoconcesionactual, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "POTENCIAINSTALADA", DbType.Decimal, ObtenerValorOrDefault(regHojaCCTTADTO.Potenciainstalada, typeof(decimal)));
            dbProvider.AddInParameter(dbCommand, "POTENCIAMAXIMA", DbType.Decimal, ObtenerValorOrDefault(regHojaCCTTADTO.Potenciamaxima, typeof(decimal)));
            dbProvider.AddInParameter(dbCommand, "POTENCIAMINIMA", DbType.Decimal, ObtenerValorOrDefault(regHojaCCTTADTO.Potenciaminima, typeof(decimal)));
            dbProvider.AddInParameter(dbCommand, "COMBUSTIBLETIPO", DbType.String, ObtenerValorOrDefault(regHojaCCTTADTO.Combustibletipo, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "COMBUSTIBLETIPOOTRO", DbType.String, ObtenerValorOrDefault(regHojaCCTTADTO.CombustibletipoOtro, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "PODERCALORIFICOINFERIOR", DbType.Decimal, ObtenerValorOrDefault(regHojaCCTTADTO.Podercalorificoinferior, typeof(decimal)));
            dbProvider.AddInParameter(dbCommand, "UNDPCI", DbType.String, ObtenerValorOrDefault(regHojaCCTTADTO.Undpci, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "PODERCALORIFICOSUPERIOR", DbType.Decimal, ObtenerValorOrDefault(regHojaCCTTADTO.Podercalorificosuperior, typeof(decimal)));
            dbProvider.AddInParameter(dbCommand, "UNDPCS", DbType.String, ObtenerValorOrDefault(regHojaCCTTADTO.Undpcs, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "COSTOCOMBUSTIBLE", DbType.Decimal, ObtenerValorOrDefault(regHojaCCTTADTO.Costocombustible, typeof(decimal)));
            dbProvider.AddInParameter(dbCommand, "UNDCOMB", DbType.String, ObtenerValorOrDefault(regHojaCCTTADTO.Undcomb, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "COSTOTRATAMIENTOCOMBUSTIBLE", DbType.Decimal, ObtenerValorOrDefault(regHojaCCTTADTO.Costotratamientocombustible, typeof(decimal)));
            dbProvider.AddInParameter(dbCommand, "UNDTRTCOMB", DbType.String, ObtenerValorOrDefault(regHojaCCTTADTO.Undtrtcomb, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "COSTOTRANSPORTECOMBUSTIBLE", DbType.Decimal, ObtenerValorOrDefault(regHojaCCTTADTO.Costotransportecombustible, typeof(decimal)));
            dbProvider.AddInParameter(dbCommand, "UNDTRNSPCOMB", DbType.String, ObtenerValorOrDefault(regHojaCCTTADTO.Undtrnspcomb, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "COSTOVARIABLENOCOMBUSTIBLE", DbType.Decimal, ObtenerValorOrDefault(regHojaCCTTADTO.Costovariablenocombustible, typeof(decimal)));
            dbProvider.AddInParameter(dbCommand, "UNDVARNCMB", DbType.String, ObtenerValorOrDefault(regHojaCCTTADTO.Undvarncmb, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "COSTOINVERSIONINICIAL", DbType.Decimal, ObtenerValorOrDefault(regHojaCCTTADTO.Costoinversioninicial, typeof(decimal)));
            dbProvider.AddInParameter(dbCommand, "UNDINVINIC", DbType.String, ObtenerValorOrDefault(regHojaCCTTADTO.Undinvinic, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "RENDIMIENTOPLANTACONDICION", DbType.Decimal, ObtenerValorOrDefault(regHojaCCTTADTO.Rendimientoplantacondicion, typeof(decimal)));
            dbProvider.AddInParameter(dbCommand, "UNDRENDCND", DbType.String, ObtenerValorOrDefault(regHojaCCTTADTO.Undrendcnd, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "CONSESPIFICACONDICION", DbType.Decimal, ObtenerValorOrDefault(regHojaCCTTADTO.Consespificacondicion, typeof(decimal)));
            dbProvider.AddInParameter(dbCommand, "UNDCONSCP", DbType.String, ObtenerValorOrDefault(regHojaCCTTADTO.Undconscp, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "TIPOMOTORTERMICO", DbType.String, ObtenerValorOrDefault(regHojaCCTTADTO.Tipomotortermico, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "VELNOMROTACION", DbType.Decimal, ObtenerValorOrDefault(regHojaCCTTADTO.Velnomrotacion, typeof(decimal)));
            dbProvider.AddInParameter(dbCommand, "POTMOTORTERMICO", DbType.Decimal, ObtenerValorOrDefault(regHojaCCTTADTO.Potmotortermico, typeof(decimal)));
            dbProvider.AddInParameter(dbCommand, "NUMMOTORESTERMICOS", DbType.String, ObtenerValorOrDefault(regHojaCCTTADTO.Nummotorestermicos, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "POTGENERADOR", DbType.Decimal, ObtenerValorOrDefault(regHojaCCTTADTO.Potgenerador, typeof(decimal)));
            dbProvider.AddInParameter(dbCommand, "NUMGENERADORES", DbType.String, ObtenerValorOrDefault(regHojaCCTTADTO.Numgeneradores, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "TENSIONGENERACION", DbType.Decimal, ObtenerValorOrDefault(regHojaCCTTADTO.Tensiongeneracion, typeof(decimal)));
            dbProvider.AddInParameter(dbCommand, "RENDIMIENTOGENERADOR", DbType.Decimal, ObtenerValorOrDefault(regHojaCCTTADTO.Rendimientogenerador, typeof(decimal)));
            dbProvider.AddInParameter(dbCommand, "TENSIONKV", DbType.Decimal, ObtenerValorOrDefault(regHojaCCTTADTO.Tensionkv, typeof(decimal)));
            dbProvider.AddInParameter(dbCommand, "LONGITUDKM", DbType.Decimal, ObtenerValorOrDefault(regHojaCCTTADTO.Longitudkm, typeof(decimal)));
            dbProvider.AddInParameter(dbCommand, "NUMTERNAS", DbType.String, ObtenerValorOrDefault(regHojaCCTTADTO.Numternas, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "NOMBRESUBESTACION", DbType.String, ObtenerValorOrDefault(regHojaCCTTADTO.Nombresubestacion, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "PERFIL", DbType.String, ObtenerValorOrDefault(regHojaCCTTADTO.Perfil, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "PREFACTIBILIDAD", DbType.String, ObtenerValorOrDefault(regHojaCCTTADTO.Prefactibilidad, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "FACTIBILIDAD", DbType.String, ObtenerValorOrDefault(regHojaCCTTADTO.Factibilidad, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "ESTUDIODEFINITIVO", DbType.String, ObtenerValorOrDefault(regHojaCCTTADTO.Estudiodefinitivo, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "EIA", DbType.String, ObtenerValorOrDefault(regHojaCCTTADTO.Eia, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "FECHAINICIOCONSTRUCCION", DbType.String, regHojaCCTTADTO.Fechainicioconstruccion);
            dbProvider.AddInParameter(dbCommand, "PERIODOCONSTRUCCION", DbType.String, regHojaCCTTADTO.Periodoconstruccion);
            dbProvider.AddInParameter(dbCommand, "FECHAOPERACIONCOMERCIAL", DbType.String, regHojaCCTTADTO.Fechaoperacioncomercial);
            dbProvider.AddInParameter(dbCommand, "COMENTARIOS", DbType.String, ObtenerValorOrDefault(regHojaCCTTADTO.Comentarios, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, ObtenerValorOrDefault(regHojaCCTTADTO.Usucreacion, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "FECHACONCESIONACTUAL", DbType.DateTime, regHojaCCTTADTO.Fechaconcesionactual);
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        public bool DeleteRegHojaCCTTAById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteRegHojaCCTTAById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);
            return true;
        }

        public int GetLastRegHojaCCTTAId()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastRegHojaCCTTAId);
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

        public RegHojaCCTTADTO GetRegHojaCCTTAById(int id)
        {
            RegHojaCCTTADTO ob = new RegHojaCCTTADTO();
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(Helper.SqlGetRegHojaCCTTAById);
            dbProvider.AddInParameter(commandHoja, "PROYCODI", DbType.Int32, id);
            dbProvider.AddInParameter(commandHoja, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.ExecuteNonQuery(commandHoja);
            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                if (dr.Read())
                {
                    ob.Centralcodi =    !dr.IsDBNull(dr.GetOrdinal("CENTRALCODI")) ? dr.GetInt32(dr.GetOrdinal("CENTRALCODI")) : 0;
                    ob.Proycodi =       !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Centralnombre =  !dr.IsDBNull(dr.GetOrdinal("CENTRALNOMBRE")) ? dr.GetString(dr.GetOrdinal("CENTRALNOMBRE")) : string.Empty;
                    ob.Distrito =       !dr.IsDBNull(dr.GetOrdinal("DISTRITO")) ? dr.GetString(dr.GetOrdinal("DISTRITO")) : string.Empty;
                    ob.Propietario =    !dr.IsDBNull(dr.GetOrdinal("PROPIETARIO")) ? dr.GetString(dr.GetOrdinal("PROPIETARIO")) : string.Empty;
                    ob.Sociooperador =  !dr.IsDBNull(dr.GetOrdinal("SOCIOOPERADOR")) ? dr.GetString(dr.GetOrdinal("SOCIOOPERADOR")) : string.Empty;
                    ob.Socioinversionista = !dr.IsDBNull(dr.GetOrdinal("SOCIOINVERSIONISTA")) ? dr.GetString(dr.GetOrdinal("SOCIOINVERSIONISTA")) : string.Empty;
                    ob.Tipoconcesionactual =!dr.IsDBNull(dr.GetOrdinal("TIPOCONCESIONACTUAL")) ? dr.GetString(dr.GetOrdinal("TIPOCONCESIONACTUAL")) : string.Empty;
                    ob.Fechaconcesionactual = !dr.IsDBNull(dr.GetOrdinal("FECHACONCESIONACTUAL")) ? dr.GetDateTime(dr.GetOrdinal("FECHACONCESIONACTUAL")) : (DateTime?)null;
                    ob.Potenciainstalada =  !dr.IsDBNull(dr.GetOrdinal("POTENCIAINSTALADA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("POTENCIAINSTALADA")) : null;
                    ob.Potenciamaxima =     !dr.IsDBNull(dr.GetOrdinal("POTENCIAMAXIMA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("POTENCIAMAXIMA")) : null;
                    ob.Potenciaminima =     !dr.IsDBNull(dr.GetOrdinal("POTENCIAMINIMA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("POTENCIAMINIMA")) : null;
                    ob.Combustibletipo =    !dr.IsDBNull(dr.GetOrdinal("COMBUSTIBLETIPO")) ? dr.GetString(dr.GetOrdinal("COMBUSTIBLETIPO")) : string.Empty;
                    ob.CombustibletipoOtro =!dr.IsDBNull(dr.GetOrdinal("COMBUSTIBLETIPOOTRO")) ? dr.GetString(dr.GetOrdinal("COMBUSTIBLETIPOOTRO")) : string.Empty;
                    ob.Podercalorificoinferior = !dr.IsDBNull(dr.GetOrdinal("PODERCALORIFICOINFERIOR")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("PODERCALORIFICOINFERIOR")) : null;
                    ob.Undpci =             !dr.IsDBNull(dr.GetOrdinal("UNDPCI")) ? dr.GetString(dr.GetOrdinal("UNDPCI")) : string.Empty;
                    ob.Podercalorificosuperior = !dr.IsDBNull(dr.GetOrdinal("PODERCALORIFICOSUPERIOR")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("PODERCALORIFICOSUPERIOR")) : null;
                    ob.Undpcs =             !dr.IsDBNull(dr.GetOrdinal("UNDPCS")) ? dr.GetString(dr.GetOrdinal("UNDPCS")) : string.Empty;
                    ob.Costocombustible =   !dr.IsDBNull(dr.GetOrdinal("COSTOCOMBUSTIBLE")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("COSTOCOMBUSTIBLE")) : null;
                    ob.Undcomb =            !dr.IsDBNull(dr.GetOrdinal("UNDCOMB")) ? dr.GetString(dr.GetOrdinal("UNDCOMB")) : string.Empty;
                    ob.Costotratamientocombustible = !dr.IsDBNull(dr.GetOrdinal("COSTOTRATAMIENTOCOMBUSTIBLE")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("COSTOTRATAMIENTOCOMBUSTIBLE")) : null;
                    ob.Undtrtcomb =         !dr.IsDBNull(dr.GetOrdinal("UNDTRTCOMB")) ? dr.GetString(dr.GetOrdinal("UNDTRTCOMB")) : string.Empty;
                    ob.Costotransportecombustible = !dr.IsDBNull(dr.GetOrdinal("COSTOTRANSPORTECOMBUSTIBLE")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("COSTOTRANSPORTECOMBUSTIBLE")) : null;
                    ob.Potenciainstalada =  !dr.IsDBNull(dr.GetOrdinal("POTENCIAINSTALADA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("POTENCIAINSTALADA")) : null;
                    ob.Undtrnspcomb =       !dr.IsDBNull(dr.GetOrdinal("UNDTRNSPCOMB")) ? dr.GetString(dr.GetOrdinal("UNDTRNSPCOMB")) : string.Empty;
                    ob.Costovariablenocombustible = !dr.IsDBNull(dr.GetOrdinal("COSTOVARIABLENOCOMBUSTIBLE")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("COSTOVARIABLENOCOMBUSTIBLE")) : null;
                    ob.Undvarncmb =         !dr.IsDBNull(dr.GetOrdinal("UNDVARNCMB")) ? dr.GetString(dr.GetOrdinal("UNDVARNCMB")) : string.Empty;
                    ob.Costoinversioninicial = !dr.IsDBNull(dr.GetOrdinal("COSTOINVERSIONINICIAL")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("COSTOINVERSIONINICIAL")) : null;
                    ob.Undinvinic = !dr.IsDBNull(dr.GetOrdinal("UNDINVINIC")) ? dr.GetString(dr.GetOrdinal("UNDINVINIC")) : string.Empty;
                    ob.Rendimientoplantacondicion = !dr.IsDBNull(dr.GetOrdinal("RENDIMIENTOPLANTACONDICION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("RENDIMIENTOPLANTACONDICION")) : null;
                    ob.Undrendcnd = !dr.IsDBNull(dr.GetOrdinal("UNDRENDCND")) ? dr.GetString(dr.GetOrdinal("UNDRENDCND")) : string.Empty;
                    ob.Consespificacondicion = !dr.IsDBNull(dr.GetOrdinal("CONSESPIFICACONDICION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("CONSESPIFICACONDICION")) : null;
                    ob.Undconscp = !dr.IsDBNull(dr.GetOrdinal("UNDCONSCP")) ? dr.GetString(dr.GetOrdinal("UNDCONSCP")) : string.Empty;
                    ob.Tipomotortermico = !dr.IsDBNull(dr.GetOrdinal("TIPOMOTORTERMICO")) ? dr.GetString(dr.GetOrdinal("TIPOMOTORTERMICO")) : string.Empty;
                    ob.Velnomrotacion = !dr.IsDBNull(dr.GetOrdinal("VELNOMROTACION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("VELNOMROTACION")) : null;
                    ob.Potmotortermico = !dr.IsDBNull(dr.GetOrdinal("POTMOTORTERMICO")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("POTMOTORTERMICO")) : null;
                    ob.Nummotorestermicos = !dr.IsDBNull(dr.GetOrdinal("NUMMOTORESTERMICOS")) ? dr.GetString(dr.GetOrdinal("NUMMOTORESTERMICOS")) : string.Empty;
                    ob.Potgenerador = !dr.IsDBNull(dr.GetOrdinal("POTGENERADOR")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("POTGENERADOR")) : null;
                    ob.Numgeneradores = !dr.IsDBNull(dr.GetOrdinal("NUMGENERADORES")) ? dr.GetString(dr.GetOrdinal("NUMGENERADORES")) : string.Empty;
                    ob.Tensiongeneracion = !dr.IsDBNull(dr.GetOrdinal("TENSIONGENERACION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("TENSIONGENERACION")) : null;
                    ob.Rendimientogenerador = !dr.IsDBNull(dr.GetOrdinal("RENDIMIENTOGENERADOR")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("RENDIMIENTOGENERADOR")) : null;
                    ob.Tensionkv = !dr.IsDBNull(dr.GetOrdinal("TENSIONKV")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("TENSIONKV")) : null;
                    ob.Longitudkm = !dr.IsDBNull(dr.GetOrdinal("LONGITUDKM")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("LONGITUDKM")) : null;
                    ob.Numternas = !dr.IsDBNull(dr.GetOrdinal("NUMTERNAS")) ? dr.GetString(dr.GetOrdinal("NUMTERNAS")) : string.Empty;
                    ob.Nombresubestacion = !dr.IsDBNull(dr.GetOrdinal("NOMBRESUBESTACION")) ? dr.GetString(dr.GetOrdinal("NOMBRESUBESTACION")) : string.Empty;
                    ob.Perfil = !dr.IsDBNull(dr.GetOrdinal("PERFIL")) ? dr.GetString(dr.GetOrdinal("PERFIL")) : string.Empty;
                    ob.Prefactibilidad = !dr.IsDBNull(dr.GetOrdinal("PREFACTIBILIDAD")) ? dr.GetString(dr.GetOrdinal("PREFACTIBILIDAD")) : string.Empty;
                    ob.Factibilidad = !dr.IsDBNull(dr.GetOrdinal("FACTIBILIDAD")) ? dr.GetString(dr.GetOrdinal("FACTIBILIDAD")) : string.Empty;
                    ob.Estudiodefinitivo = !dr.IsDBNull(dr.GetOrdinal("ESTUDIODEFINITIVO")) ? dr.GetString(dr.GetOrdinal("ESTUDIODEFINITIVO")) : string.Empty;
                    ob.Eia = !dr.IsDBNull(dr.GetOrdinal("EIA")) ? dr.GetString(dr.GetOrdinal("EIA")) : string.Empty;
                    ob.Fechainicioconstruccion = !dr.IsDBNull(dr.GetOrdinal("FECHAINICIOCONSTRUCCION")) ? dr.GetString(dr.GetOrdinal("FECHAINICIOCONSTRUCCION")) : string.Empty;
                    ob.Periodoconstruccion = !dr.IsDBNull(dr.GetOrdinal("PERIODOCONSTRUCCION")) ? dr.GetString(dr.GetOrdinal("PERIODOCONSTRUCCION")) : string.Empty;
                    ob.Fechaoperacioncomercial = !dr.IsDBNull(dr.GetOrdinal("FECHAOPERACIONCOMERCIAL")) ? dr.GetString(dr.GetOrdinal("FECHAOPERACIONCOMERCIAL")) : string.Empty;
                    ob.Comentarios = !dr.IsDBNull(dr.GetOrdinal("COMENTARIOS")) ? dr.GetString(dr.GetOrdinal("COMENTARIOS")) : string.Empty;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;

                }

            }
            return ob;
        }
          public List<RegHojaCCTTADTO> GetRegHojaCCTTAByFilter(string plancodi, string empresa, string estado)
        {
            List<RegHojaCCTTADTO> oblist = new List<RegHojaCCTTADTO>();
            string query = $@"
                SELECT CGB.*, TR.EMPRESANOM, TR.PROYNOMBRE, TR.PROYDESCRIPCION, TP.TIPONOMBRE, TF.TIPOFINOMBRE,TR.PROYCONFIDENCIAL  FROM CAM_CENTERMOHOJAA CGB
                INNER JOIN CAM_TRNSMPROYECTO TR ON TR.PROYCODI = CGB.PROYCODI
                INNER JOIN CAM_PLANTRANSMISION PL ON PL.PLANCODI = TR.PLANCODI
                INNER JOIN CAM_TIPOPROYECTO TP ON TP.TIPOCODI = TR.TIPOCODI
                LEFT JOIN CAM_TIPOFICHAPROYECTO TF ON TF.TIPOFICODI = TR.TIPOFICODI
                WHERE TR.PERICODI  IN ({plancodi}) AND 
                PL.CODEMPRESA IN ({empresa})  AND 
                CGB.IND_DEL = 0 AND 
                PL.PLANESTADO ='{estado}'
                ORDER BY TR.PERICODI, CGB.PROYCODI,PL.CODEMPRESA, CGB.CENTRALCODI ASC";
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(commandHoja);
            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                while (dr.Read())
                {
                    RegHojaCCTTADTO ob = new RegHojaCCTTADTO();
                    ob.Centralcodi = !dr.IsDBNull(dr.GetOrdinal("CENTRALCODI")) ? dr.GetInt32(dr.GetOrdinal("CENTRALCODI")) : 0;
                    ob.Proycodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Centralnombre = !dr.IsDBNull(dr.GetOrdinal("CENTRALNOMBRE")) ? dr.GetString(dr.GetOrdinal("CENTRALNOMBRE")) : string.Empty;
                    ob.Distrito = !dr.IsDBNull(dr.GetOrdinal("DISTRITO")) ? dr.GetString(dr.GetOrdinal("DISTRITO")) : string.Empty;
                    ob.Propietario = !dr.IsDBNull(dr.GetOrdinal("PROPIETARIO")) ? dr.GetString(dr.GetOrdinal("PROPIETARIO")) : string.Empty;
                    ob.Sociooperador = !dr.IsDBNull(dr.GetOrdinal("SOCIOOPERADOR")) ? dr.GetString(dr.GetOrdinal("SOCIOOPERADOR")) : string.Empty;
                    ob.Socioinversionista = !dr.IsDBNull(dr.GetOrdinal("SOCIOINVERSIONISTA")) ? dr.GetString(dr.GetOrdinal("SOCIOINVERSIONISTA")) : string.Empty;
                    ob.Tipoconcesionactual = !dr.IsDBNull(dr.GetOrdinal("TIPOCONCESIONACTUAL")) ? dr.GetString(dr.GetOrdinal("TIPOCONCESIONACTUAL")) : string.Empty;
                    ob.Fechaconcesionactual = !dr.IsDBNull(dr.GetOrdinal("FECHACONCESIONACTUAL")) ? dr.GetDateTime(dr.GetOrdinal("FECHACONCESIONACTUAL")) : (DateTime?)null;
                    ob.Potenciainstalada = !dr.IsDBNull(dr.GetOrdinal("POTENCIAINSTALADA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("POTENCIAINSTALADA")) : null;
                    ob.Potenciamaxima = !dr.IsDBNull(dr.GetOrdinal("POTENCIAMAXIMA")) ? dr.GetDecimal(dr.GetOrdinal("POTENCIAMAXIMA")) : 0;
                    ob.Potenciaminima = !dr.IsDBNull(dr.GetOrdinal("POTENCIAMINIMA")) ? dr.GetDecimal(dr.GetOrdinal("POTENCIAMINIMA")) : 0;
                    ob.Combustibletipo = !dr.IsDBNull(dr.GetOrdinal("COMBUSTIBLETIPO")) ? dr.GetString(dr.GetOrdinal("COMBUSTIBLETIPO")) : string.Empty;
                    ob.CombustibletipoOtro = !dr.IsDBNull(dr.GetOrdinal("COMBUSTIBLETIPOOTRO")) ? dr.GetString(dr.GetOrdinal("COMBUSTIBLETIPOOTRO")) : string.Empty;
                    ob.Podercalorificoinferior = !dr.IsDBNull(dr.GetOrdinal("PODERCALORIFICOINFERIOR")) ? dr.GetDecimal(dr.GetOrdinal("PODERCALORIFICOINFERIOR")) : 0;
                    ob.Undpci = !dr.IsDBNull(dr.GetOrdinal("UNDPCI")) ? dr.GetString(dr.GetOrdinal("UNDPCI")) : string.Empty;
                    ob.Podercalorificosuperior = !dr.IsDBNull(dr.GetOrdinal("PODERCALORIFICOSUPERIOR")) ? dr.GetDecimal(dr.GetOrdinal("PODERCALORIFICOSUPERIOR")) : 0;
                    ob.Undpcs = !dr.IsDBNull(dr.GetOrdinal("UNDPCS")) ? dr.GetString(dr.GetOrdinal("UNDPCS")) : string.Empty;
                    ob.Costocombustible = !dr.IsDBNull(dr.GetOrdinal("COSTOCOMBUSTIBLE")) ? dr.GetDecimal(dr.GetOrdinal("COSTOCOMBUSTIBLE")) :0;
                    ob.Undcomb = !dr.IsDBNull(dr.GetOrdinal("UNDCOMB")) ? dr.GetString(dr.GetOrdinal("UNDCOMB")) : string.Empty;
                    ob.Costotratamientocombustible = !dr.IsDBNull(dr.GetOrdinal("COSTOTRATAMIENTOCOMBUSTIBLE")) ? dr.GetDecimal(dr.GetOrdinal("COSTOTRATAMIENTOCOMBUSTIBLE")) : 0;
                    ob.Undtrtcomb = !dr.IsDBNull(dr.GetOrdinal("UNDTRTCOMB")) ? dr.GetString(dr.GetOrdinal("UNDTRTCOMB")) : string.Empty;
                    ob.Costotransportecombustible = !dr.IsDBNull(dr.GetOrdinal("COSTOTRANSPORTECOMBUSTIBLE")) ? dr.GetDecimal(dr.GetOrdinal("COSTOTRANSPORTECOMBUSTIBLE")) : 0;
                    ob.Potenciainstalada = !dr.IsDBNull(dr.GetOrdinal("POTENCIAINSTALADA")) ? dr.GetDecimal(dr.GetOrdinal("POTENCIAINSTALADA")) : 0;
                    ob.Undtrnspcomb = !dr.IsDBNull(dr.GetOrdinal("UNDTRNSPCOMB")) ? dr.GetString(dr.GetOrdinal("UNDTRNSPCOMB")) : string.Empty;
                    ob.Costovariablenocombustible = !dr.IsDBNull(dr.GetOrdinal("COSTOVARIABLENOCOMBUSTIBLE")) ? dr.GetDecimal(dr.GetOrdinal("COSTOVARIABLENOCOMBUSTIBLE")) : 0;
                    ob.Undvarncmb = !dr.IsDBNull(dr.GetOrdinal("UNDVARNCMB")) ? dr.GetString(dr.GetOrdinal("UNDVARNCMB")) : string.Empty;
                    ob.Costoinversioninicial = !dr.IsDBNull(dr.GetOrdinal("COSTOINVERSIONINICIAL")) ? dr.GetDecimal(dr.GetOrdinal("COSTOINVERSIONINICIAL")) : 0;
                    ob.Undinvinic = !dr.IsDBNull(dr.GetOrdinal("UNDINVINIC")) ? dr.GetString(dr.GetOrdinal("UNDINVINIC")) : string.Empty;
                    ob.Rendimientoplantacondicion = !dr.IsDBNull(dr.GetOrdinal("RENDIMIENTOPLANTACONDICION")) ? dr.GetDecimal(dr.GetOrdinal("RENDIMIENTOPLANTACONDICION")) : 0;
                    ob.Undrendcnd = !dr.IsDBNull(dr.GetOrdinal("UNDRENDCND")) ? dr.GetString(dr.GetOrdinal("UNDRENDCND")) : string.Empty;
                    ob.Consespificacondicion = !dr.IsDBNull(dr.GetOrdinal("CONSESPIFICACONDICION")) ? dr.GetDecimal(dr.GetOrdinal("CONSESPIFICACONDICION")) : 0;
                    ob.Undconscp = !dr.IsDBNull(dr.GetOrdinal("UNDCONSCP")) ? dr.GetString(dr.GetOrdinal("UNDCONSCP")) : string.Empty;
                    ob.Tipomotortermico = !dr.IsDBNull(dr.GetOrdinal("TIPOMOTORTERMICO")) ? dr.GetString(dr.GetOrdinal("TIPOMOTORTERMICO")) : string.Empty;
                    ob.Velnomrotacion = !dr.IsDBNull(dr.GetOrdinal("VELNOMROTACION")) ? dr.GetDecimal(dr.GetOrdinal("VELNOMROTACION")) : 0;
                    ob.Potmotortermico = !dr.IsDBNull(dr.GetOrdinal("POTMOTORTERMICO")) ? dr.GetDecimal(dr.GetOrdinal("POTMOTORTERMICO")) : 0;
                    ob.Nummotorestermicos = !dr.IsDBNull(dr.GetOrdinal("NUMMOTORESTERMICOS")) ? dr.GetString(dr.GetOrdinal("NUMMOTORESTERMICOS")) : string.Empty;
                    ob.Potgenerador = !dr.IsDBNull(dr.GetOrdinal("POTGENERADOR")) ? dr.GetDecimal(dr.GetOrdinal("POTGENERADOR")) : 0;
                    ob.Numgeneradores = !dr.IsDBNull(dr.GetOrdinal("NUMGENERADORES")) ? dr.GetString(dr.GetOrdinal("NUMGENERADORES")) : string.Empty;
                    ob.Tensiongeneracion = !dr.IsDBNull(dr.GetOrdinal("TENSIONGENERACION")) ? dr.GetDecimal(dr.GetOrdinal("TENSIONGENERACION")) : 0;
                    ob.Rendimientogenerador = !dr.IsDBNull(dr.GetOrdinal("RENDIMIENTOGENERADOR")) ? dr.GetDecimal(dr.GetOrdinal("RENDIMIENTOGENERADOR")) : 0;
                    ob.Tensionkv = !dr.IsDBNull(dr.GetOrdinal("TENSIONKV")) ? dr.GetDecimal(dr.GetOrdinal("TENSIONKV")) : 0;
                    ob.Longitudkm = !dr.IsDBNull(dr.GetOrdinal("LONGITUDKM")) ? dr.GetDecimal(dr.GetOrdinal("LONGITUDKM")) : 0;
                    ob.Numternas = !dr.IsDBNull(dr.GetOrdinal("NUMTERNAS")) ? dr.GetString(dr.GetOrdinal("NUMTERNAS")) : string.Empty;
                    ob.Nombresubestacion = !dr.IsDBNull(dr.GetOrdinal("NOMBRESUBESTACION")) ? dr.GetString(dr.GetOrdinal("NOMBRESUBESTACION")) : string.Empty;
                    ob.Perfil = !dr.IsDBNull(dr.GetOrdinal("PERFIL")) ? dr.GetString(dr.GetOrdinal("PERFIL")) : string.Empty;
                    ob.Prefactibilidad = !dr.IsDBNull(dr.GetOrdinal("PREFACTIBILIDAD")) ? dr.GetString(dr.GetOrdinal("PREFACTIBILIDAD")) : string.Empty;
                    ob.Factibilidad = !dr.IsDBNull(dr.GetOrdinal("FACTIBILIDAD")) ? dr.GetString(dr.GetOrdinal("FACTIBILIDAD")) : string.Empty;
                    ob.Estudiodefinitivo = !dr.IsDBNull(dr.GetOrdinal("ESTUDIODEFINITIVO")) ? dr.GetString(dr.GetOrdinal("ESTUDIODEFINITIVO")) : string.Empty;
                    ob.Eia = !dr.IsDBNull(dr.GetOrdinal("EIA")) ? dr.GetString(dr.GetOrdinal("EIA")) : string.Empty;
                    ob.Fechainicioconstruccion = !dr.IsDBNull(dr.GetOrdinal("FECHAINICIOCONSTRUCCION")) ? dr.GetString(dr.GetOrdinal("FECHAINICIOCONSTRUCCION")) : string.Empty;
                    ob.Periodoconstruccion = !dr.IsDBNull(dr.GetOrdinal("PERIODOCONSTRUCCION")) ? dr.GetString(dr.GetOrdinal("PERIODOCONSTRUCCION")) : string.Empty;
                    ob.Fechaoperacioncomercial = !dr.IsDBNull(dr.GetOrdinal("FECHAOPERACIONCOMERCIAL")) ? dr.GetString(dr.GetOrdinal("FECHAOPERACIONCOMERCIAL")) : string.Empty;
                    ob.Comentarios = !dr.IsDBNull(dr.GetOrdinal("COMENTARIOS")) ? dr.GetString(dr.GetOrdinal("COMENTARIOS")) : string.Empty;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                    ob.Empresa = !dr.IsDBNull(dr.GetOrdinal("EMPRESANOM")) ? dr.GetString(dr.GetOrdinal("EMPRESANOM")) : string.Empty;
                    ob.NombreProyecto = !dr.IsDBNull(dr.GetOrdinal("PROYNOMBRE")) ? dr.GetString(dr.GetOrdinal("PROYNOMBRE")) : string.Empty;
                    ob.DetalleProyecto = !dr.IsDBNull(dr.GetOrdinal("PROYDESCRIPCION")) ? dr.GetString(dr.GetOrdinal("PROYDESCRIPCION")) : string.Empty;
                    ob.TipoProyecto = !dr.IsDBNull(dr.GetOrdinal("TIPONOMBRE")) ? dr.GetString(dr.GetOrdinal("TIPONOMBRE")) : string.Empty;
                    ob.SubTipoProyecto = !dr.IsDBNull(dr.GetOrdinal("TIPOFINOMBRE")) ? dr.GetString(dr.GetOrdinal("TIPOFINOMBRE")) : string.Empty;
                    ob.Condifencial = dr.IsDBNull(dr.GetOrdinal("PROYCONFIDENCIAL")) ? null : dr.GetString(dr.GetOrdinal("PROYCONFIDENCIAL"));
                    oblist.Add(ob);
                }

            }
            return oblist;
        }

        public bool UpdateRegHojaCCTTA(RegHojaCCTTADTO regHojaCCTTADTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlUpdateRegHojaCCTTA);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, regHojaCCTTADTO.Proycodi);
            dbProvider.AddInParameter(dbCommand, "CENTRALNOMBRE", DbType.String, regHojaCCTTADTO.Centralnombre);
            dbProvider.AddInParameter(dbCommand, "DISTRITO", DbType.String, regHojaCCTTADTO.Distrito);
            dbProvider.AddInParameter(dbCommand, "PROPIETARIO", DbType.String, regHojaCCTTADTO.Propietario);
            dbProvider.AddInParameter(dbCommand, "SOCIOOPERADOR", DbType.String, regHojaCCTTADTO.Sociooperador);
            dbProvider.AddInParameter(dbCommand, "SOCIOINVERSIONISTA", DbType.String, regHojaCCTTADTO.Socioinversionista);
            dbProvider.AddInParameter(dbCommand, "TIPOCONCESIONACTUAL", DbType.String, regHojaCCTTADTO.Tipoconcesionactual);
            dbProvider.AddInParameter(dbCommand, "FECHACONCESIONACTUAL", DbType.DateTime, regHojaCCTTADTO.Fechaconcesionactual);
            dbProvider.AddInParameter(dbCommand, "POTENCIAINSTALADA", DbType.String, regHojaCCTTADTO.Potenciainstalada);
            dbProvider.AddInParameter(dbCommand, "POTENCIAMAXIMA", DbType.String, regHojaCCTTADTO.Potenciamaxima);
            dbProvider.AddInParameter(dbCommand, "POTENCIAMINIMA", DbType.String, regHojaCCTTADTO.Potenciaminima);
            dbProvider.AddInParameter(dbCommand, "COMBUSTIBLETIPO", DbType.String, regHojaCCTTADTO.Combustibletipo);
            dbProvider.AddInParameter(dbCommand, "COMBUSTIBLETIPOOTRO", DbType.String, regHojaCCTTADTO.CombustibletipoOtro);
            dbProvider.AddInParameter(dbCommand, "PODERCALORIFICOINFERIOR", DbType.String, regHojaCCTTADTO.Podercalorificoinferior);
            dbProvider.AddInParameter(dbCommand, "UNDPCI", DbType.String, regHojaCCTTADTO.Undpci);
            dbProvider.AddInParameter(dbCommand, "PODERCALORIFICOSUPERIOR", DbType.String, regHojaCCTTADTO.Podercalorificosuperior);
            dbProvider.AddInParameter(dbCommand, "UNDPCS", DbType.String, regHojaCCTTADTO.Undpcs);
            dbProvider.AddInParameter(dbCommand, "COSTOCOMBUSTIBLE", DbType.String, regHojaCCTTADTO.Costocombustible);
            dbProvider.AddInParameter(dbCommand, "UNDCOMB", DbType.String, regHojaCCTTADTO.Undcomb);
            dbProvider.AddInParameter(dbCommand, "COSTOTRATAMIENTOCOMBUSTIBLE", DbType.String, regHojaCCTTADTO.Costotratamientocombustible);
            dbProvider.AddInParameter(dbCommand, "UNDTRTCOMB", DbType.String, regHojaCCTTADTO.Undtrtcomb);
            dbProvider.AddInParameter(dbCommand, "COSTOTRANSPORTECOMBUSTIBLE", DbType.String, regHojaCCTTADTO.Costotransportecombustible);
            dbProvider.AddInParameter(dbCommand, "UNDTRNSPCOMB", DbType.String, regHojaCCTTADTO.Undtrnspcomb);
            dbProvider.AddInParameter(dbCommand, "COSTOVARIABLENOCOMBUSTIBLE", DbType.String, regHojaCCTTADTO.Costovariablenocombustible);
            dbProvider.AddInParameter(dbCommand, "UNDVARNCMB", DbType.String, regHojaCCTTADTO.Undvarncmb);
            dbProvider.AddInParameter(dbCommand, "COSTOINVERSIONINICIAL", DbType.String, regHojaCCTTADTO.Costoinversioninicial);
            dbProvider.AddInParameter(dbCommand, "UNDINVINIC", DbType.String, regHojaCCTTADTO.Undinvinic);
            dbProvider.AddInParameter(dbCommand, "RENDIMIENTOPLANTACONDICION", DbType.String, regHojaCCTTADTO.Rendimientoplantacondicion);
            dbProvider.AddInParameter(dbCommand, "UNDRENDCND", DbType.String, regHojaCCTTADTO.Undrendcnd);
            dbProvider.AddInParameter(dbCommand, "CONSESPIFICACONDICION", DbType.String, regHojaCCTTADTO.Consespificacondicion);
            dbProvider.AddInParameter(dbCommand, "UNDCONSCP", DbType.String, regHojaCCTTADTO.Undconscp);
            dbProvider.AddInParameter(dbCommand, "TIPOMOTORTERMICO", DbType.String, regHojaCCTTADTO.Tipomotortermico);
            dbProvider.AddInParameter(dbCommand, "VELNOMROTACION", DbType.String, regHojaCCTTADTO.Velnomrotacion);
            dbProvider.AddInParameter(dbCommand, "POTMOTORTERMICO", DbType.String, regHojaCCTTADTO.Potmotortermico);
            dbProvider.AddInParameter(dbCommand, "NUMMOTORESTERMICOS", DbType.String, regHojaCCTTADTO.Nummotorestermicos);
            dbProvider.AddInParameter(dbCommand, "POTGENERADOR", DbType.String, regHojaCCTTADTO.Potgenerador);
            dbProvider.AddInParameter(dbCommand, "NUMGENERADORES", DbType.String, regHojaCCTTADTO.Numgeneradores);
            dbProvider.AddInParameter(dbCommand, "TENSIONGENERACION", DbType.String, regHojaCCTTADTO.Tensiongeneracion);
            dbProvider.AddInParameter(dbCommand, "RENDIMIENTOGENERADOR", DbType.String, regHojaCCTTADTO.Rendimientogenerador);
            dbProvider.AddInParameter(dbCommand, "TENSIONKV", DbType.String, regHojaCCTTADTO.Tensionkv);
            dbProvider.AddInParameter(dbCommand, "LONGITUDKM", DbType.String, regHojaCCTTADTO.Longitudkm);
            dbProvider.AddInParameter(dbCommand, "NUMTERNAS", DbType.String, regHojaCCTTADTO.Numternas);
            dbProvider.AddInParameter(dbCommand, "NOMBRESUBESTACION", DbType.String, regHojaCCTTADTO.Nombresubestacion);
            dbProvider.AddInParameter(dbCommand, "PERFIL", DbType.String, regHojaCCTTADTO.Perfil);
            dbProvider.AddInParameter(dbCommand, "PREFACTIBILIDAD", DbType.String, regHojaCCTTADTO.Prefactibilidad);
            dbProvider.AddInParameter(dbCommand, "FACTIBILIDAD", DbType.String, regHojaCCTTADTO.Factibilidad);
            dbProvider.AddInParameter(dbCommand, "ESTUDIODEFINITIVO", DbType.String, regHojaCCTTADTO.Estudiodefinitivo);
            dbProvider.AddInParameter(dbCommand, "EIA", DbType.String, regHojaCCTTADTO.Eia);
            dbProvider.AddInParameter(dbCommand, "FECHAINICIOCONSTRUCCION", DbType.DateTime, regHojaCCTTADTO.Fechainicioconstruccion);
            dbProvider.AddInParameter(dbCommand, "PERIODOCONSTRUCCION", DbType.String, regHojaCCTTADTO.Periodoconstruccion);
            dbProvider.AddInParameter(dbCommand, "FECHAOPERACIONCOMERCIAL", DbType.DateTime, regHojaCCTTADTO.Fechaoperacioncomercial);
            dbProvider.AddInParameter(dbCommand, "COMENTARIOS", DbType.String, regHojaCCTTADTO.Comentarios);
            dbProvider.AddInParameter(dbCommand, "USU_MODIFICACION", DbType.String, regHojaCCTTADTO.Usumodificacion);
            dbProvider.AddInParameter(dbCommand, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "CENTRALCODI", DbType.Int32, regHojaCCTTADTO.Centralcodi);
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
                else if (tipo == typeof(decimal) || tipo == typeof(decimal?))
                {
                    return null;
                }
                else if (tipo == typeof(DateTime) || tipo == typeof(DateTime?))
                {
                    return fechaMinimaValida;
                }
            }
            return valor;
        }

    }
}
