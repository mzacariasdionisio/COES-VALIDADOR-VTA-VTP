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
namespace COES.Infraestructura.Datos.Repositorio.Campania
{
    public class CamRegHojaARepository: RepositoryBase, ICamRegHojaARepository
    {

        public CamRegHojaARepository(string strConn) : base(strConn){}

        CamRegHojaAHelper Helper = new CamRegHojaAHelper();

        public List<RegHojaADTO> GetRegHojaAProyCodi(int proyCodi)
        {
            List<RegHojaADTO> regHojaADTOs = new List<RegHojaADTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetRegHojaAProyCodi);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.String, proyCodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RegHojaADTO ob = new RegHojaADTO();
                    ob.Fichaacodi = !dr.IsDBNull(dr.GetOrdinal("FICHAACODI")) ? dr.GetInt32(dr.GetOrdinal("FICHAACODI")) : 0;
                    ob.Proycodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Centralnombre = !dr.IsDBNull(dr.GetOrdinal("CENTRALNOMBRE")) ? dr.GetString(dr.GetOrdinal("CENTRALNOMBRE")) : string.Empty;
                    ob.Distrito = !dr.IsDBNull(dr.GetOrdinal("DISTRITO")) ? dr.GetString(dr.GetOrdinal("DISTRITO")) : string.Empty;
                    ob.Cuenca = !dr.IsDBNull(dr.GetOrdinal("CUENCA")) ? dr.GetString(dr.GetOrdinal("CUENCA")) : string.Empty;
                    ob.Rio = !dr.IsDBNull(dr.GetOrdinal("RIO")) ? dr.GetString(dr.GetOrdinal("RIO")) : string.Empty;
                    ob.Propietario = !dr.IsDBNull(dr.GetOrdinal("PROPIETARIO")) ? dr.GetString(dr.GetOrdinal("PROPIETARIO")) : string.Empty;
                    ob.Sociooperador = !dr.IsDBNull(dr.GetOrdinal("SOCIOOPERADOR")) ? dr.GetString(dr.GetOrdinal("SOCIOOPERADOR")) : string.Empty;
                    ob.Socioinversionista = !dr.IsDBNull(dr.GetOrdinal("SOCIOINVERSIONISTA")) ? dr.GetString(dr.GetOrdinal("SOCIOINVERSIONISTA")) : string.Empty;
                    ob.Concesiontemporal = !dr.IsDBNull(dr.GetOrdinal("CONCESIONTEMPORAL")) ? dr.GetString(dr.GetOrdinal("CONCESIONTEMPORAL")) : string.Empty;
                    ob.Fechaconcesiontemporal = !dr.IsDBNull(dr.GetOrdinal("FECHACONCESIONTEMPORAL")) ? dr.GetDateTime(dr.GetOrdinal("FECHACONCESIONTEMPORAL")) : (DateTime?)null;
                    ob.Tipoconcesionactual = !dr.IsDBNull(dr.GetOrdinal("TIPOCONCESIONACTUAL")) ? dr.GetString(dr.GetOrdinal("TIPOCONCESIONACTUAL")) : string.Empty;
                    ob.Fechaconcesionactual = !dr.IsDBNull(dr.GetOrdinal("FECHACONCESIONACTUAL")) ? dr.GetDateTime(dr.GetOrdinal("FECHACONCESIONACTUAL")) : (DateTime?)null;
                    ob.Nombreestacion = !dr.IsDBNull(dr.GetOrdinal("NOMBREESTACION")) ? dr.GetString(dr.GetOrdinal("NOMBREESTACION")) : string.Empty;
                    ob.Numestacion = !dr.IsDBNull(dr.GetOrdinal("NUMESTACION")) ? dr.GetString(dr.GetOrdinal("NUMESTACION")) : string.Empty;
                    ob.Periodohistorica = !dr.IsDBNull(dr.GetOrdinal("PERIODOHISTORICA")) ? dr.GetString(dr.GetOrdinal("PERIODOHISTORICA")) : string.Empty;
                    ob.Periodonaturalizada = !dr.IsDBNull(dr.GetOrdinal("PERIODONATURALIZADA")) ? dr.GetString(dr.GetOrdinal("PERIODONATURALIZADA")) : string.Empty;
                    ob.Demandaagua = !dr.IsDBNull(dr.GetOrdinal("DEMANDAAGUA")) ? dr.GetString(dr.GetOrdinal("DEMANDAAGUA")) : string.Empty;
                    ob.Estudiogeologico = !dr.IsDBNull(dr.GetOrdinal("ESTUDIOGEOLOGICO")) ? dr.GetString(dr.GetOrdinal("ESTUDIOGEOLOGICO")) : string.Empty;
                    ob.Perfodiamantinas = !dr.IsDBNull(dr.GetOrdinal("PERFODIAMANTINAS")) ? dr.GetString(dr.GetOrdinal("PERFODIAMANTINAS")) : string.Empty;
                    ob.Numcalicatas = !dr.IsDBNull(dr.GetOrdinal("NUMCALICATAS")) ? dr.GetString(dr.GetOrdinal("NUMCALICATAS")) : string.Empty;
                    ob.EstudioTopografico = !dr.IsDBNull(dr.GetOrdinal("ESTUDIOTOPOGRAFICO")) ? dr.GetString(dr.GetOrdinal("ESTUDIOTOPOGRAFICO")) : string.Empty;
                    ob.Levantamientotopografico = !dr.IsDBNull(dr.GetOrdinal("LEVANTAMIENTOTOPOGRAFICO")) ? dr.GetString(dr.GetOrdinal("LEVANTAMIENTOTOPOGRAFICO")) : string.Empty;
                    ob.Alturabruta = !dr.IsDBNull(dr.GetOrdinal("ALTURABRUTA")) ? dr.GetString(dr.GetOrdinal("ALTURABRUTA")) : string.Empty;
                    ob.Alturaneta = !dr.IsDBNull(dr.GetOrdinal("ALTURANETA")) ? dr.GetString(dr.GetOrdinal("ALTURANETA")) : string.Empty;
                    ob.Caudaldiseno = !dr.IsDBNull(dr.GetOrdinal("CAUDALDISENO")) ? dr.GetString(dr.GetOrdinal("CAUDALDISENO")) : string.Empty;
                    ob.Potenciainstalada = !dr.IsDBNull(dr.GetOrdinal("POTENCIAINSTALADA")) ? dr.GetString(dr.GetOrdinal("POTENCIAINSTALADA")) : string.Empty;
                    ob.Conduccionlongitud = !dr.IsDBNull(dr.GetOrdinal("CONDUCCIONLONGITUD")) ? dr.GetString(dr.GetOrdinal("CONDUCCIONLONGITUD")) : string.Empty;
                    ob.Tunelarea = !dr.IsDBNull(dr.GetOrdinal("TUNELAREA")) ? dr.GetString(dr.GetOrdinal("TUNELAREA")) : string.Empty;
                    ob.Tuneltipo = !dr.IsDBNull(dr.GetOrdinal("TUNELTIPO")) ? dr.GetString(dr.GetOrdinal("TUNELTIPO")) : string.Empty;
                    ob.Tuberialongitud = !dr.IsDBNull(dr.GetOrdinal("TUBERIALONGITUD")) ? dr.GetString(dr.GetOrdinal("TUBERIALONGITUD")) : string.Empty;
                    ob.Tuberiadiametro = !dr.IsDBNull(dr.GetOrdinal("TUBERIADIAMETRO")) ? dr.GetString(dr.GetOrdinal("TUBERIADIAMETRO")) : string.Empty;
                    ob.Tuberiatipo = !dr.IsDBNull(dr.GetOrdinal("TUBERIATIPO")) ? dr.GetString(dr.GetOrdinal("TUBERIATIPO")) : string.Empty;
                    ob.Maquinatipo = !dr.IsDBNull(dr.GetOrdinal("MAQUINATIPO")) ? dr.GetString(dr.GetOrdinal("MAQUINATIPO")) : string.Empty;
                    ob.Maquinaaltitud = !dr.IsDBNull(dr.GetOrdinal("MAQUINAALTITUD")) ? dr.GetString(dr.GetOrdinal("MAQUINAALTITUD")) : string.Empty;
                    ob.Regestacionalvbruto = !dr.IsDBNull(dr.GetOrdinal("REGESTACIONALVBRUTO")) ? dr.GetString(dr.GetOrdinal("REGESTACIONALVBRUTO")) : string.Empty;
                    ob.Regestacionalvutil = !dr.IsDBNull(dr.GetOrdinal("REGESTACIONALVUTIL")) ? dr.GetString(dr.GetOrdinal("REGESTACIONALVUTIL")) : string.Empty;
                    ob.Regestacionalhpresa = !dr.IsDBNull(dr.GetOrdinal("REGESTACIONALHPRESA")) ? dr.GetString(dr.GetOrdinal("REGESTACIONALHPRESA")) : string.Empty;
                    ob.Reghorariavutil = !dr.IsDBNull(dr.GetOrdinal("REGHORARIAVUTIL")) ? dr.GetString(dr.GetOrdinal("REGHORARIAVUTIL")) : string.Empty;
                    ob.Reghorariahpresa = !dr.IsDBNull(dr.GetOrdinal("REGHORARIAHPRESA")) ? dr.GetString(dr.GetOrdinal("REGHORARIAHPRESA")) : string.Empty;
                    ob.Reghorariaubicacion = !dr.IsDBNull(dr.GetOrdinal("REGHORARIAUBICACION")) ? dr.GetString(dr.GetOrdinal("REGHORARIAUBICACION")) : string.Empty;
                    ob.Energhorapunta = !dr.IsDBNull(dr.GetOrdinal("ENERGHORAPUNTA")) ? dr.GetString(dr.GetOrdinal("ENERGHORAPUNTA")) : string.Empty;
                    ob.Energfuerapunta = !dr.IsDBNull(dr.GetOrdinal("ENERGFUERAPUNTA")) ? dr.GetString(dr.GetOrdinal("ENERGFUERAPUNTA")) : string.Empty;
                    ob.Tipoturbina = !dr.IsDBNull(dr.GetOrdinal("TIPOTURBINA")) ? dr.GetString(dr.GetOrdinal("TIPOTURBINA")) : string.Empty;
                    ob.Velnomrotacion = !dr.IsDBNull(dr.GetOrdinal("VELNOMROTACION")) ? dr.GetString(dr.GetOrdinal("VELNOMROTACION")) : string.Empty;
                    ob.Potturbina = !dr.IsDBNull(dr.GetOrdinal("POTTURBINA")) ? dr.GetString(dr.GetOrdinal("POTTURBINA")) : string.Empty;
                    ob.Numturbinas = !dr.IsDBNull(dr.GetOrdinal("NUMTURBINAS")) ? dr.GetString(dr.GetOrdinal("NUMTURBINAS")) : string.Empty;
                    ob.Potgenerador = !dr.IsDBNull(dr.GetOrdinal("POTGENERADOR")) ? dr.GetString(dr.GetOrdinal("POTGENERADOR")) : string.Empty;
                    ob.Numgeneradores = !dr.IsDBNull(dr.GetOrdinal("NUMGENERADORES")) ? dr.GetString(dr.GetOrdinal("NUMGENERADORES")) : string.Empty;
                    ob.Tensiongeneracion = !dr.IsDBNull(dr.GetOrdinal("TENSIONGENERACION")) ? dr.GetString(dr.GetOrdinal("TENSIONGENERACION")) : string.Empty;
                    ob.Rendimientogenerador = !dr.IsDBNull(dr.GetOrdinal("RENDIMIENTOGENERADOR")) ? dr.GetString(dr.GetOrdinal("RENDIMIENTOGENERADOR")) : string.Empty;
                    ob.Tensionkv = !dr.IsDBNull(dr.GetOrdinal("TENSIONKV")) ? dr.GetString(dr.GetOrdinal("TENSIONKV")) : string.Empty;
                    ob.Longitudkm = !dr.IsDBNull(dr.GetOrdinal("LONGITUDKM")) ? dr.GetString(dr.GetOrdinal("LONGITUDKM")) : string.Empty;
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
                    regHojaADTOs.Add(ob);
                }
            }

            return regHojaADTOs;
        }

        public bool SaveRegHojaA(RegHojaADTO regHojaADTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveRegHojaA);
            dbProvider.AddInParameter(dbCommand, "FICHAACODI", DbType.Int32, ObtenerValorOrDefault(regHojaADTO.Fichaacodi, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, ObtenerValorOrDefault(regHojaADTO.Proycodi, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "CENTRALNOMBRE", DbType.String, ObtenerValorOrDefault(regHojaADTO.Centralnombre, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "DISTRITO", DbType.String, ObtenerValorOrDefault(regHojaADTO.Distrito, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "CUENCA", DbType.String, ObtenerValorOrDefault(regHojaADTO.Cuenca, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "RIO", DbType.String, ObtenerValorOrDefault(regHojaADTO.Rio, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "PROPIETARIO", DbType.String, ObtenerValorOrDefault(regHojaADTO.Propietario, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "SOCIOOPERADOR", DbType.String, ObtenerValorOrDefault(regHojaADTO.Sociooperador, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "SOCIOINVERSIONISTA", DbType.String, ObtenerValorOrDefault(regHojaADTO.Socioinversionista, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "CONCESIONTEMPORAL", DbType.String, ObtenerValorOrDefault(regHojaADTO.Concesiontemporal, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "TIPOCONCESIONACTUAL", DbType.String, ObtenerValorOrDefault(regHojaADTO.Tipoconcesionactual, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "NOMBREESTACION", DbType.String, ObtenerValorOrDefault(regHojaADTO.Nombreestacion, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "NUMESTACION", DbType.String, ObtenerValorOrDefault(regHojaADTO.Numestacion, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "PERIODOHISTORICA", DbType.String, ObtenerValorOrDefault(regHojaADTO.Periodohistorica, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "PERIODONATURALIZADA", DbType.String, ObtenerValorOrDefault(regHojaADTO.Periodonaturalizada, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "DEMANDAAGUA", DbType.String, ObtenerValorOrDefault(regHojaADTO.Demandaagua, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "ESTUDIOGEOLOGICO", DbType.String, ObtenerValorOrDefault(regHojaADTO.Estudiogeologico, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "PERFODIAMANTINAS", DbType.Decimal, ObtenerValorOrDefault(regHojaADTO.Perfodiamantinas, typeof(decimal)));
            dbProvider.AddInParameter(dbCommand, "NUMCALICATAS", DbType.String, ObtenerValorOrDefault(regHojaADTO.Numcalicatas, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "ESTUDIOTOPOGRAFICO", DbType.String, ObtenerValorOrDefault(regHojaADTO.EstudioTopografico, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "LEVANTAMIENTOTOPOGRAFICO", DbType.Decimal, ObtenerValorOrDefault(regHojaADTO.Levantamientotopografico, typeof(decimal)));
            dbProvider.AddInParameter(dbCommand, "ALTURABRUTA", DbType.Decimal, ObtenerValorOrDefault(regHojaADTO.Alturabruta, typeof(decimal)));
            dbProvider.AddInParameter(dbCommand, "ALTURANETA", DbType.Decimal, ObtenerValorOrDefault(regHojaADTO.Alturaneta, typeof(decimal)));
            dbProvider.AddInParameter(dbCommand, "CAUDALDISENO", DbType.Decimal, ObtenerValorOrDefault(regHojaADTO.Caudaldiseno, typeof(decimal)));
            dbProvider.AddInParameter(dbCommand, "POTENCIAINSTALADA", DbType.Decimal, ObtenerValorOrDefault(regHojaADTO.Potenciainstalada, typeof(decimal)));
            dbProvider.AddInParameter(dbCommand, "CONDUCCIONLONGITUD", DbType.Decimal, ObtenerValorOrDefault(regHojaADTO.Conduccionlongitud, typeof(decimal)));
            dbProvider.AddInParameter(dbCommand, "TUNELAREA", DbType.Decimal, ObtenerValorOrDefault(regHojaADTO.Tunelarea, typeof(decimal)));
            dbProvider.AddInParameter(dbCommand, "TUNELTIPO", DbType.String, ObtenerValorOrDefault(regHojaADTO.Tuneltipo, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "TUBERIALONGITUD", DbType.Decimal, ObtenerValorOrDefault(regHojaADTO.Tuberialongitud, typeof(decimal)));
            dbProvider.AddInParameter(dbCommand, "TUBERIADIAMETRO", DbType.Decimal, ObtenerValorOrDefault(regHojaADTO.Tuberiadiametro, typeof(decimal)));
            dbProvider.AddInParameter(dbCommand, "TUBERIATIPO", DbType.String, ObtenerValorOrDefault(regHojaADTO.Tuberiatipo, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "MAQUINATIPO", DbType.String, ObtenerValorOrDefault(regHojaADTO.Maquinatipo, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "MAQUINAALTITUD", DbType.Decimal, ObtenerValorOrDefault(regHojaADTO.Maquinaaltitud, typeof(decimal)));
            dbProvider.AddInParameter(dbCommand, "REGESTACIONALVBRUTO", DbType.Decimal, ObtenerValorOrDefault(regHojaADTO.Regestacionalvbruto, typeof(decimal)));
            dbProvider.AddInParameter(dbCommand, "REGESTACIONALVUTIL", DbType.Decimal, ObtenerValorOrDefault(regHojaADTO.Regestacionalvutil, typeof(decimal)));
            dbProvider.AddInParameter(dbCommand, "REGESTACIONALHPRESA", DbType.Decimal, ObtenerValorOrDefault(regHojaADTO.Regestacionalhpresa, typeof(decimal)));
            dbProvider.AddInParameter(dbCommand, "REGHORARIAVUTIL", DbType.Decimal, ObtenerValorOrDefault(regHojaADTO.Reghorariavutil, typeof(decimal)));
            dbProvider.AddInParameter(dbCommand, "REGHORARIAHPRESA", DbType.Decimal, ObtenerValorOrDefault(regHojaADTO.Reghorariahpresa, typeof(decimal)));
            dbProvider.AddInParameter(dbCommand, "REGHORARIAUBICACION", DbType.String, ObtenerValorOrDefault(regHojaADTO.Reghorariaubicacion, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "ENERGHORAPUNTA", DbType.Decimal, ObtenerValorOrDefault(regHojaADTO.Energhorapunta, typeof(decimal)));
            dbProvider.AddInParameter(dbCommand, "ENERGFUERAPUNTA", DbType.Decimal, ObtenerValorOrDefault(regHojaADTO.Energfuerapunta, typeof(decimal)));
            dbProvider.AddInParameter(dbCommand, "TIPOTURBINA", DbType.String, ObtenerValorOrDefault(regHojaADTO.Tipoturbina, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "VELNOMROTACION", DbType.Decimal, ObtenerValorOrDefault(regHojaADTO.Velnomrotacion, typeof(decimal)));
            dbProvider.AddInParameter(dbCommand, "POTTURBINA", DbType.Decimal, ObtenerValorOrDefault(regHojaADTO.Potturbina, typeof(decimal)));
            dbProvider.AddInParameter(dbCommand, "NUMTURBINAS", DbType.Decimal, ObtenerValorOrDefault(regHojaADTO.Numturbinas, typeof(decimal)));
            dbProvider.AddInParameter(dbCommand, "POTGENERADOR", DbType.Decimal, ObtenerValorOrDefault(regHojaADTO.Potgenerador, typeof(decimal)));
            dbProvider.AddInParameter(dbCommand, "NUMGENERADORES", DbType.Decimal, ObtenerValorOrDefault(regHojaADTO.Numgeneradores, typeof(decimal)));
            dbProvider.AddInParameter(dbCommand, "TENSIONGENERACION", DbType.Decimal, ObtenerValorOrDefault(regHojaADTO.Tensiongeneracion, typeof(decimal)));
            dbProvider.AddInParameter(dbCommand, "RENDIMIENTOGENERADOR", DbType.Decimal, ObtenerValorOrDefault(regHojaADTO.Rendimientogenerador, typeof(decimal)));
            dbProvider.AddInParameter(dbCommand, "TENSIONKV", DbType.Decimal, ObtenerValorOrDefault(regHojaADTO.Tensionkv, typeof(decimal)));
            dbProvider.AddInParameter(dbCommand, "LONGITUDKM", DbType.Decimal, ObtenerValorOrDefault(regHojaADTO.Longitudkm, typeof(decimal)));
            dbProvider.AddInParameter(dbCommand, "NUMTERNAS", DbType.String, ObtenerValorOrDefault(regHojaADTO.Numternas, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "NOMBRESUBESTACION", DbType.String, ObtenerValorOrDefault(regHojaADTO.Nombresubestacion, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "PERFIL", DbType.String, ObtenerValorOrDefault(regHojaADTO.Perfil, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "PREFACTIBILIDAD", DbType.String, ObtenerValorOrDefault(regHojaADTO.Prefactibilidad, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "FACTIBILIDAD", DbType.String, ObtenerValorOrDefault(regHojaADTO.Factibilidad, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "ESTUDIODEFINITIVO", DbType.String, ObtenerValorOrDefault(regHojaADTO.Estudiodefinitivo, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "EIA", DbType.String, ObtenerValorOrDefault(regHojaADTO.Eia, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "FECHAINICIOCONSTRUCCION", DbType.String, regHojaADTO.Fechainicioconstruccion);
            dbProvider.AddInParameter(dbCommand, "PERIODOCONSTRUCCION", DbType.Decimal, regHojaADTO.Periodoconstruccion);
            dbProvider.AddInParameter(dbCommand, "FECHAOPERACIONCOMERCIAL", DbType.String, regHojaADTO.Fechaoperacioncomercial);
            dbProvider.AddInParameter(dbCommand, "COMENTARIOS", DbType.String, ObtenerValorOrDefault(regHojaADTO.Comentarios, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, ObtenerValorOrDefault(regHojaADTO.Usucreacion, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "FECHACONCESIONTEMPORAL", DbType.DateTime, regHojaADTO.Fechaconcesiontemporal);
            dbProvider.AddInParameter(dbCommand, "FECHACONCESIONACTUAL", DbType.DateTime, regHojaADTO.Fechaconcesionactual);
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        public bool DeleteRegHojaAById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteRegHojaAById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);
            return true;
        }

        public int GetLastRegHojaAId()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastRegHojaAId);
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

        public RegHojaADTO GetRegHojaAById(int id)
        {
            RegHojaADTO ob = new RegHojaADTO();
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(Helper.SqlGetRegHojaAById);
            dbProvider.AddInParameter(commandHoja, "PROYCODI", DbType.Int32, id);
            dbProvider.AddInParameter(commandHoja, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.ExecuteNonQuery(commandHoja);
            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                if (dr.Read())
                {
                    ob.Fichaacodi = !dr.IsDBNull(dr.GetOrdinal("FICHAACODI")) ? dr.GetInt32(dr.GetOrdinal("FICHAACODI")) : 0;
                    ob.Proycodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Centralnombre = !dr.IsDBNull(dr.GetOrdinal("CENTRALNOMBRE")) ? dr.GetString(dr.GetOrdinal("CENTRALNOMBRE")) : string.Empty;
                    ob.Distrito = !dr.IsDBNull(dr.GetOrdinal("DISTRITO")) ? dr.GetString(dr.GetOrdinal("DISTRITO")) : string.Empty;
                    ob.Cuenca = !dr.IsDBNull(dr.GetOrdinal("CUENCA")) ? dr.GetString(dr.GetOrdinal("CUENCA")) : string.Empty;
                    ob.Rio = !dr.IsDBNull(dr.GetOrdinal("RIO")) ? dr.GetString(dr.GetOrdinal("RIO")) : string.Empty;
                    ob.Propietario = !dr.IsDBNull(dr.GetOrdinal("PROPIETARIO")) ? dr.GetString(dr.GetOrdinal("PROPIETARIO")) : string.Empty;
                    ob.Sociooperador = !dr.IsDBNull(dr.GetOrdinal("SOCIOOPERADOR")) ? dr.GetString(dr.GetOrdinal("SOCIOOPERADOR")) : string.Empty;
                    ob.Socioinversionista = !dr.IsDBNull(dr.GetOrdinal("SOCIOINVERSIONISTA")) ? dr.GetString(dr.GetOrdinal("SOCIOINVERSIONISTA")) : string.Empty;
                    ob.Concesiontemporal = !dr.IsDBNull(dr.GetOrdinal("CONCESIONTEMPORAL")) ? dr.GetString(dr.GetOrdinal("CONCESIONTEMPORAL")) : string.Empty;
                    ob.Fechaconcesiontemporal = !dr.IsDBNull(dr.GetOrdinal("FECHACONCESIONTEMPORAL")) ? dr.GetDateTime(dr.GetOrdinal("FECHACONCESIONTEMPORAL")) : (DateTime?)null;
                    ob.Tipoconcesionactual = !dr.IsDBNull(dr.GetOrdinal("TIPOCONCESIONACTUAL")) ? dr.GetString(dr.GetOrdinal("TIPOCONCESIONACTUAL")) : string.Empty;
                    ob.Fechaconcesionactual = !dr.IsDBNull(dr.GetOrdinal("FECHACONCESIONACTUAL")) ? dr.GetDateTime(dr.GetOrdinal("FECHACONCESIONACTUAL")) : (DateTime?)null;
                    ob.Nombreestacion = !dr.IsDBNull(dr.GetOrdinal("NOMBREESTACION")) ? dr.GetString(dr.GetOrdinal("NOMBREESTACION")) : string.Empty;
                    ob.Numestacion = !dr.IsDBNull(dr.GetOrdinal("NUMESTACION")) ? dr.GetString(dr.GetOrdinal("NUMESTACION")) : string.Empty;
                    ob.Periodohistorica = !dr.IsDBNull(dr.GetOrdinal("PERIODOHISTORICA")) ? dr.GetString(dr.GetOrdinal("PERIODOHISTORICA")) : string.Empty;
                    ob.Periodonaturalizada = !dr.IsDBNull(dr.GetOrdinal("PERIODONATURALIZADA")) ? dr.GetString(dr.GetOrdinal("PERIODONATURALIZADA")) : string.Empty;
                    ob.Demandaagua = !dr.IsDBNull(dr.GetOrdinal("DEMANDAAGUA")) ? dr.GetString(dr.GetOrdinal("DEMANDAAGUA")) : string.Empty;
                    ob.Estudiogeologico = !dr.IsDBNull(dr.GetOrdinal("ESTUDIOGEOLOGICO")) ? dr.GetString(dr.GetOrdinal("ESTUDIOGEOLOGICO")) : string.Empty;
                    ob.Perfodiamantinas = !dr.IsDBNull(dr.GetOrdinal("PERFODIAMANTINAS")) ? dr.GetString(dr.GetOrdinal("PERFODIAMANTINAS")) : string.Empty;
                    ob.Numcalicatas = !dr.IsDBNull(dr.GetOrdinal("NUMCALICATAS")) ? dr.GetString(dr.GetOrdinal("NUMCALICATAS")) : string.Empty;
                    ob.EstudioTopografico = !dr.IsDBNull(dr.GetOrdinal("ESTUDIOTOPOGRAFICO")) ? dr.GetString(dr.GetOrdinal("ESTUDIOTOPOGRAFICO")) : string.Empty;
                    ob.Levantamientotopografico = !dr.IsDBNull(dr.GetOrdinal("LEVANTAMIENTOTOPOGRAFICO")) ? dr.GetString(dr.GetOrdinal("LEVANTAMIENTOTOPOGRAFICO")) : string.Empty;
                    ob.Alturabruta = !dr.IsDBNull(dr.GetOrdinal("ALTURABRUTA")) ? dr.GetString(dr.GetOrdinal("ALTURABRUTA")) : string.Empty;
                    ob.Alturaneta = !dr.IsDBNull(dr.GetOrdinal("ALTURANETA")) ? dr.GetString(dr.GetOrdinal("ALTURANETA")) : string.Empty;
                    ob.Caudaldiseno = !dr.IsDBNull(dr.GetOrdinal("CAUDALDISENO")) ? dr.GetString(dr.GetOrdinal("CAUDALDISENO")) : string.Empty;
                    ob.Potenciainstalada = !dr.IsDBNull(dr.GetOrdinal("POTENCIAINSTALADA")) ? dr.GetString(dr.GetOrdinal("POTENCIAINSTALADA")) : string.Empty;
                    ob.Conduccionlongitud = !dr.IsDBNull(dr.GetOrdinal("CONDUCCIONLONGITUD")) ? dr.GetString(dr.GetOrdinal("CONDUCCIONLONGITUD")) : string.Empty;
                    ob.Tunelarea = !dr.IsDBNull(dr.GetOrdinal("TUNELAREA")) ? dr.GetString(dr.GetOrdinal("TUNELAREA")) : string.Empty;
                    ob.Tuneltipo = !dr.IsDBNull(dr.GetOrdinal("TUNELTIPO")) ? dr.GetString(dr.GetOrdinal("TUNELTIPO")) : string.Empty;
                    ob.Tuberialongitud = !dr.IsDBNull(dr.GetOrdinal("TUBERIALONGITUD")) ? dr.GetString(dr.GetOrdinal("TUBERIALONGITUD")) : string.Empty;
                    ob.Tuberiadiametro = !dr.IsDBNull(dr.GetOrdinal("TUBERIADIAMETRO")) ? dr.GetString(dr.GetOrdinal("TUBERIADIAMETRO")) : string.Empty;
                    ob.Tuberiatipo = !dr.IsDBNull(dr.GetOrdinal("TUBERIATIPO")) ? dr.GetString(dr.GetOrdinal("TUBERIATIPO")) : string.Empty;
                    ob.Maquinatipo = !dr.IsDBNull(dr.GetOrdinal("MAQUINATIPO")) ? dr.GetString(dr.GetOrdinal("MAQUINATIPO")) : string.Empty;
                    ob.Maquinaaltitud = !dr.IsDBNull(dr.GetOrdinal("MAQUINAALTITUD")) ? dr.GetString(dr.GetOrdinal("MAQUINAALTITUD")) : string.Empty;
                    ob.Regestacionalvbruto = !dr.IsDBNull(dr.GetOrdinal("REGESTACIONALVBRUTO")) ? dr.GetString(dr.GetOrdinal("REGESTACIONALVBRUTO")) : string.Empty;
                    ob.Regestacionalvutil = !dr.IsDBNull(dr.GetOrdinal("REGESTACIONALVUTIL")) ? dr.GetString(dr.GetOrdinal("REGESTACIONALVUTIL")) : string.Empty;
                    ob.Regestacionalhpresa = !dr.IsDBNull(dr.GetOrdinal("REGESTACIONALHPRESA")) ? dr.GetString(dr.GetOrdinal("REGESTACIONALHPRESA")) : string.Empty;
                    ob.Reghorariavutil = !dr.IsDBNull(dr.GetOrdinal("REGHORARIAVUTIL")) ? dr.GetString(dr.GetOrdinal("REGHORARIAVUTIL")) : string.Empty;
                    ob.Reghorariahpresa = !dr.IsDBNull(dr.GetOrdinal("REGHORARIAHPRESA")) ? dr.GetString(dr.GetOrdinal("REGHORARIAHPRESA")) : string.Empty;
                    ob.Reghorariaubicacion = !dr.IsDBNull(dr.GetOrdinal("REGHORARIAUBICACION")) ? dr.GetString(dr.GetOrdinal("REGHORARIAUBICACION")) : string.Empty;
                    ob.Energhorapunta = !dr.IsDBNull(dr.GetOrdinal("ENERGHORAPUNTA")) ? dr.GetString(dr.GetOrdinal("ENERGHORAPUNTA")) : string.Empty;
                    ob.Energfuerapunta = !dr.IsDBNull(dr.GetOrdinal("ENERGFUERAPUNTA")) ? dr.GetString(dr.GetOrdinal("ENERGFUERAPUNTA")) : string.Empty;
                    ob.Tipoturbina = !dr.IsDBNull(dr.GetOrdinal("TIPOTURBINA")) ? dr.GetString(dr.GetOrdinal("TIPOTURBINA")) : string.Empty;
                    ob.Velnomrotacion = !dr.IsDBNull(dr.GetOrdinal("VELNOMROTACION")) ? dr.GetString(dr.GetOrdinal("VELNOMROTACION")) : string.Empty;
                    ob.Potturbina = !dr.IsDBNull(dr.GetOrdinal("POTTURBINA")) ? dr.GetString(dr.GetOrdinal("POTTURBINA")) : string.Empty;
                    ob.Numturbinas = !dr.IsDBNull(dr.GetOrdinal("NUMTURBINAS")) ? dr.GetString(dr.GetOrdinal("NUMTURBINAS")) : string.Empty;
                    ob.Potgenerador = !dr.IsDBNull(dr.GetOrdinal("POTGENERADOR")) ? dr.GetString(dr.GetOrdinal("POTGENERADOR")) : string.Empty;
                    ob.Numgeneradores = !dr.IsDBNull(dr.GetOrdinal("NUMGENERADORES")) ? dr.GetString(dr.GetOrdinal("NUMGENERADORES")) : string.Empty;
                    ob.Tensiongeneracion = !dr.IsDBNull(dr.GetOrdinal("TENSIONGENERACION")) ? dr.GetString(dr.GetOrdinal("TENSIONGENERACION")) : string.Empty;
                    ob.Rendimientogenerador = !dr.IsDBNull(dr.GetOrdinal("RENDIMIENTOGENERADOR")) ? dr.GetString(dr.GetOrdinal("RENDIMIENTOGENERADOR")) : string.Empty;
                    ob.Tensionkv = !dr.IsDBNull(dr.GetOrdinal("TENSIONKV")) ? dr.GetString(dr.GetOrdinal("TENSIONKV")) : string.Empty;
                    ob.Longitudkm = !dr.IsDBNull(dr.GetOrdinal("LONGITUDKM")) ? dr.GetString(dr.GetOrdinal("LONGITUDKM")) : string.Empty;
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

        public bool UpdateRegHojaA(RegHojaADTO regHojaADTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlUpdateRegHojaA);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, regHojaADTO.Proycodi);
            dbProvider.AddInParameter(dbCommand, "CENTRALNOMBRE", DbType.String, regHojaADTO.Centralnombre);
            dbProvider.AddInParameter(dbCommand, "DISTRITO", DbType.String, regHojaADTO.Distrito);
            dbProvider.AddInParameter(dbCommand, "CUENCA", DbType.String, regHojaADTO.Cuenca);
            dbProvider.AddInParameter(dbCommand, "RIO", DbType.String, regHojaADTO.Rio);
            dbProvider.AddInParameter(dbCommand, "PROPIETARIO", DbType.String, regHojaADTO.Propietario);
            dbProvider.AddInParameter(dbCommand, "SOCIOOPERADOR", DbType.String, regHojaADTO.Sociooperador);
            dbProvider.AddInParameter(dbCommand, "SOCIOINVERSIONISTA", DbType.String, regHojaADTO.Socioinversionista);
            dbProvider.AddInParameter(dbCommand, "CONCESIONTEMPORAL", DbType.String, regHojaADTO.Concesiontemporal);
            dbProvider.AddInParameter(dbCommand, "FECHACONCESIONTEMPORAL", DbType.DateTime, regHojaADTO.Fechaconcesiontemporal);
            dbProvider.AddInParameter(dbCommand, "TIPOCONCESIONACTUAL", DbType.String, regHojaADTO.Tipoconcesionactual);
            dbProvider.AddInParameter(dbCommand, "FECHACONCESIONACTUAL", DbType.DateTime, regHojaADTO.Fechaconcesionactual);
            dbProvider.AddInParameter(dbCommand, "NOMBREESTACION", DbType.String, regHojaADTO.Nombreestacion);
            dbProvider.AddInParameter(dbCommand, "NUMESTACION", DbType.String, regHojaADTO.Numestacion);
            dbProvider.AddInParameter(dbCommand, "PERIODOHISTORICA", DbType.String, regHojaADTO.Periodohistorica);
            dbProvider.AddInParameter(dbCommand, "PERIODONATURALIZADA", DbType.String, regHojaADTO.Periodonaturalizada);
            dbProvider.AddInParameter(dbCommand, "DEMANDAAGUA", DbType.String, regHojaADTO.Demandaagua);
            dbProvider.AddInParameter(dbCommand, "ESTUDIOGEOLOGICO", DbType.String, regHojaADTO.Estudiogeologico);
            dbProvider.AddInParameter(dbCommand, "PERFODIAMANTINAS", DbType.String, regHojaADTO.Perfodiamantinas);
            dbProvider.AddInParameter(dbCommand, "NUMCALICATAS", DbType.String, regHojaADTO.Numcalicatas);
            dbProvider.AddInParameter(dbCommand, "ESTUDIOTOPOGRAFICO", DbType.String, regHojaADTO.EstudioTopografico);
            dbProvider.AddInParameter(dbCommand, "LEVANTAMIENTOTOPOGRAFICO", DbType.String, regHojaADTO.Levantamientotopografico);
            dbProvider.AddInParameter(dbCommand, "ALTURABRUTA", DbType.String, regHojaADTO.Alturabruta);
            dbProvider.AddInParameter(dbCommand, "ALTURANETA", DbType.String, regHojaADTO.Alturaneta);
            dbProvider.AddInParameter(dbCommand, "CAUDALDISENO", DbType.String, regHojaADTO.Caudaldiseno);
            dbProvider.AddInParameter(dbCommand, "POTENCIAINSTALADA", DbType.String, regHojaADTO.Potenciainstalada);
            dbProvider.AddInParameter(dbCommand, "CONDUCCIONLONGITUD", DbType.String, regHojaADTO.Conduccionlongitud);
            dbProvider.AddInParameter(dbCommand, "TUNELAREA", DbType.String, regHojaADTO.Tunelarea);
            dbProvider.AddInParameter(dbCommand, "TUNELTIPO", DbType.String, regHojaADTO.Tuneltipo);
            dbProvider.AddInParameter(dbCommand, "TUBERIALONGITUD", DbType.String, regHojaADTO.Tuberialongitud);
            dbProvider.AddInParameter(dbCommand, "TUBERIADIAMETRO", DbType.String, regHojaADTO.Tuberiadiametro);
            dbProvider.AddInParameter(dbCommand, "TUBERIATIPO", DbType.String, regHojaADTO.Tuberiatipo);
            dbProvider.AddInParameter(dbCommand, "MAQUINATIPO", DbType.String, regHojaADTO.Maquinatipo);
            dbProvider.AddInParameter(dbCommand, "MAQUINAALTITUD", DbType.String, regHojaADTO.Maquinaaltitud);
            dbProvider.AddInParameter(dbCommand, "REGESTACIONALVBRUTO", DbType.String, regHojaADTO.Regestacionalvbruto);
            dbProvider.AddInParameter(dbCommand, "REGESTACIONALVUTIL", DbType.String, regHojaADTO.Regestacionalvutil);
            dbProvider.AddInParameter(dbCommand, "REGESTACIONALHPRESA", DbType.String, regHojaADTO.Regestacionalhpresa);
            dbProvider.AddInParameter(dbCommand, "REGHORARIAVUTIL", DbType.String, regHojaADTO.Reghorariavutil);
            dbProvider.AddInParameter(dbCommand, "REGHORARIAHPRESA", DbType.String, regHojaADTO.Reghorariahpresa);
            dbProvider.AddInParameter(dbCommand, "REGHORARIAUBICACION", DbType.String, regHojaADTO.Reghorariaubicacion);
            dbProvider.AddInParameter(dbCommand, "ENERGHORAPUNTA", DbType.String, regHojaADTO.Energhorapunta);
            dbProvider.AddInParameter(dbCommand, "ENERGFUERAPUNTA", DbType.String, regHojaADTO.Energfuerapunta);
            dbProvider.AddInParameter(dbCommand, "TIPOTURBINA", DbType.String, regHojaADTO.Tipoturbina);
            dbProvider.AddInParameter(dbCommand, "VELNOMROTACION", DbType.String, regHojaADTO.Velnomrotacion);
            dbProvider.AddInParameter(dbCommand, "POTTURBINA", DbType.String, regHojaADTO.Potturbina);
            dbProvider.AddInParameter(dbCommand, "NUMTURBINAS", DbType.String, regHojaADTO.Numturbinas);
            dbProvider.AddInParameter(dbCommand, "POTGENERADOR", DbType.String, regHojaADTO.Potgenerador);
            dbProvider.AddInParameter(dbCommand, "NUMGENERADORES", DbType.String, regHojaADTO.Numgeneradores);
            dbProvider.AddInParameter(dbCommand, "TENSIONGENERACION", DbType.String, regHojaADTO.Tensiongeneracion);
            dbProvider.AddInParameter(dbCommand, "RENDIMIENTOGENERADOR", DbType.String, regHojaADTO.Rendimientogenerador);
            dbProvider.AddInParameter(dbCommand, "TENSIONKV", DbType.String, regHojaADTO.Tensionkv);
            dbProvider.AddInParameter(dbCommand, "LONGITUDKM", DbType.String, regHojaADTO.Longitudkm);
            dbProvider.AddInParameter(dbCommand, "NUMTERNAS", DbType.String, regHojaADTO.Numternas);
            dbProvider.AddInParameter(dbCommand, "NOMBRESUBESTACION", DbType.String, regHojaADTO.Nombresubestacion);
            dbProvider.AddInParameter(dbCommand, "PERFIL", DbType.String, regHojaADTO.Perfil);
            dbProvider.AddInParameter(dbCommand, "PREFACTIBILIDAD", DbType.String, regHojaADTO.Prefactibilidad);
            dbProvider.AddInParameter(dbCommand, "FACTIBILIDAD", DbType.String, regHojaADTO.Factibilidad);
            dbProvider.AddInParameter(dbCommand, "ESTUDIODEFINITIVO", DbType.String, regHojaADTO.Estudiodefinitivo);
            dbProvider.AddInParameter(dbCommand, "EIA", DbType.String, regHojaADTO.Eia);
            dbProvider.AddInParameter(dbCommand, "FECHAINICIOCONSTRUCCION", DbType.DateTime, regHojaADTO.Fechainicioconstruccion);
            dbProvider.AddInParameter(dbCommand, "PERIODOCONSTRUCCION", DbType.String, regHojaADTO.Periodoconstruccion);
            dbProvider.AddInParameter(dbCommand, "FECHAOPERACIONCOMERCIAL", DbType.DateTime, regHojaADTO.Fechaoperacioncomercial);
            dbProvider.AddInParameter(dbCommand, "COMENTARIOS", DbType.String, regHojaADTO.Comentarios);
            dbProvider.AddInParameter(dbCommand, "USU_MODIFICACION", DbType.String, regHojaADTO.Usumodificacion);
            dbProvider.AddInParameter(dbCommand, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "FICHAACODI", DbType.Int32, regHojaADTO.Fichaacodi);
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


        public List<RegHojaADTO> GetRegHojaAByFilter(string plancodi, string empresa, string estado)
        {
            List<RegHojaADTO> listOb = new List<RegHojaADTO>();
            string query = $@"
                  SELECT CGB.*, TR.EMPRESANOM, TR.PROYNOMBRE, TR.PROYDESCRIPCION, TP.TIPONOMBRE, TF.TIPOFINOMBRE,TR.PROYCONFIDENCIAL  FROM CAM_REGHOJAA CGB
                 INNER JOIN CAM_TRNSMPROYECTO TR ON TR.PROYCODI = CGB.PROYCODI
                 INNER JOIN CAM_PLANTRANSMISION PL ON PL.PLANCODI = TR.PLANCODI
                 INNER JOIN CAM_TIPOPROYECTO TP ON TP.TIPOCODI = TR.TIPOCODI
                 LEFT JOIN CAM_TIPOFICHAPROYECTO TF ON TF.TIPOFICODI = TR.TIPOFICODI
                WHERE TR.PERICODI  IN ({plancodi}) AND 
                PL.CODEMPRESA IN ({empresa})  AND 
                CGB.IND_DEL = 0 AND 
                PL.PLANESTADO ='{estado}'
                ORDER BY TR.PERICODI, CGB.PROYCODI,PL.CODEMPRESA, CGB.FICHAACODI ASC";
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(commandHoja);
            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                while (dr.Read())
                {
                    RegHojaADTO ob = new RegHojaADTO();
                    ob.Fichaacodi = !dr.IsDBNull(dr.GetOrdinal("FICHAACODI")) ? dr.GetInt32(dr.GetOrdinal("FICHAACODI")) : 0;
                    ob.Proycodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Centralnombre = !dr.IsDBNull(dr.GetOrdinal("CENTRALNOMBRE")) ? dr.GetString(dr.GetOrdinal("CENTRALNOMBRE")) : string.Empty;
                    ob.Distrito = !dr.IsDBNull(dr.GetOrdinal("DISTRITO")) ? dr.GetString(dr.GetOrdinal("DISTRITO")) : string.Empty;
                    ob.Cuenca = !dr.IsDBNull(dr.GetOrdinal("CUENCA")) ? dr.GetString(dr.GetOrdinal("CUENCA")) : string.Empty;
                    ob.Rio = !dr.IsDBNull(dr.GetOrdinal("RIO")) ? dr.GetString(dr.GetOrdinal("RIO")) : string.Empty;
                    ob.Propietario = !dr.IsDBNull(dr.GetOrdinal("PROPIETARIO")) ? dr.GetString(dr.GetOrdinal("PROPIETARIO")) : string.Empty;
                    ob.Sociooperador = !dr.IsDBNull(dr.GetOrdinal("SOCIOOPERADOR")) ? dr.GetString(dr.GetOrdinal("SOCIOOPERADOR")) : string.Empty;
                    ob.Socioinversionista = !dr.IsDBNull(dr.GetOrdinal("SOCIOINVERSIONISTA")) ? dr.GetString(dr.GetOrdinal("SOCIOINVERSIONISTA")) : string.Empty;
                    ob.Concesiontemporal = !dr.IsDBNull(dr.GetOrdinal("CONCESIONTEMPORAL")) ? dr.GetString(dr.GetOrdinal("CONCESIONTEMPORAL")) : string.Empty;
                    ob.Fechaconcesiontemporal = !dr.IsDBNull(dr.GetOrdinal("FECHACONCESIONTEMPORAL")) ? dr.GetDateTime(dr.GetOrdinal("FECHACONCESIONTEMPORAL")) : (DateTime?)null;
                    ob.Tipoconcesionactual = !dr.IsDBNull(dr.GetOrdinal("TIPOCONCESIONACTUAL")) ? dr.GetString(dr.GetOrdinal("TIPOCONCESIONACTUAL")) : string.Empty;
                    ob.Fechaconcesionactual = !dr.IsDBNull(dr.GetOrdinal("FECHACONCESIONACTUAL")) ? dr.GetDateTime(dr.GetOrdinal("FECHACONCESIONACTUAL")) : (DateTime?)null;
                    ob.Nombreestacion = !dr.IsDBNull(dr.GetOrdinal("NOMBREESTACION")) ? dr.GetString(dr.GetOrdinal("NOMBREESTACION")) : string.Empty;
                    ob.Numestacion = !dr.IsDBNull(dr.GetOrdinal("NUMESTACION")) ? dr.GetString(dr.GetOrdinal("NUMESTACION")) : string.Empty;
                    ob.Periodohistorica = !dr.IsDBNull(dr.GetOrdinal("PERIODOHISTORICA")) ? dr.GetString(dr.GetOrdinal("PERIODOHISTORICA")) : string.Empty;
                    ob.Periodonaturalizada = !dr.IsDBNull(dr.GetOrdinal("PERIODONATURALIZADA")) ? dr.GetString(dr.GetOrdinal("PERIODONATURALIZADA")) : string.Empty;
                    ob.Demandaagua = !dr.IsDBNull(dr.GetOrdinal("DEMANDAAGUA")) ? dr.GetString(dr.GetOrdinal("DEMANDAAGUA")) : string.Empty;
                    ob.Estudiogeologico = !dr.IsDBNull(dr.GetOrdinal("ESTUDIOGEOLOGICO")) ? dr.GetString(dr.GetOrdinal("ESTUDIOGEOLOGICO")) : string.Empty;
                    ob.Perfodiamantinas = !dr.IsDBNull(dr.GetOrdinal("PERFODIAMANTINAS")) ? dr.GetString(dr.GetOrdinal("PERFODIAMANTINAS")) : string.Empty;
                    ob.Numcalicatas = !dr.IsDBNull(dr.GetOrdinal("NUMCALICATAS")) ? dr.GetString(dr.GetOrdinal("NUMCALICATAS")) : string.Empty;
                    ob.EstudioTopografico = !dr.IsDBNull(dr.GetOrdinal("ESTUDIOTOPOGRAFICO")) ? dr.GetString(dr.GetOrdinal("ESTUDIOTOPOGRAFICO")) : string.Empty;
                    ob.Levantamientotopografico = !dr.IsDBNull(dr.GetOrdinal("LEVANTAMIENTOTOPOGRAFICO")) ? dr.GetString(dr.GetOrdinal("LEVANTAMIENTOTOPOGRAFICO")) : string.Empty;
                    ob.Alturabruta = !dr.IsDBNull(dr.GetOrdinal("ALTURABRUTA")) ? dr.GetString(dr.GetOrdinal("ALTURABRUTA")) : string.Empty;
                    ob.Alturaneta = !dr.IsDBNull(dr.GetOrdinal("ALTURANETA")) ? dr.GetString(dr.GetOrdinal("ALTURANETA")) : string.Empty;
                    ob.Caudaldiseno = !dr.IsDBNull(dr.GetOrdinal("CAUDALDISENO")) ? dr.GetString(dr.GetOrdinal("CAUDALDISENO")) : string.Empty;
                    ob.Potenciainstalada = !dr.IsDBNull(dr.GetOrdinal("POTENCIAINSTALADA")) ? dr.GetString(dr.GetOrdinal("POTENCIAINSTALADA")) : string.Empty;
                    ob.Conduccionlongitud = !dr.IsDBNull(dr.GetOrdinal("CONDUCCIONLONGITUD")) ? dr.GetString(dr.GetOrdinal("CONDUCCIONLONGITUD")) : string.Empty;
                    ob.Tunelarea = !dr.IsDBNull(dr.GetOrdinal("TUNELAREA")) ? dr.GetString(dr.GetOrdinal("TUNELAREA")) : string.Empty;
                    ob.Tuneltipo = !dr.IsDBNull(dr.GetOrdinal("TUNELTIPO")) ? dr.GetString(dr.GetOrdinal("TUNELTIPO")) : string.Empty;
                    ob.Tuberialongitud = !dr.IsDBNull(dr.GetOrdinal("TUBERIALONGITUD")) ? dr.GetString(dr.GetOrdinal("TUBERIALONGITUD")) : string.Empty;
                    ob.Tuberiadiametro = !dr.IsDBNull(dr.GetOrdinal("TUBERIADIAMETRO")) ? dr.GetString(dr.GetOrdinal("TUBERIADIAMETRO")) : string.Empty;
                    ob.Tuberiatipo = !dr.IsDBNull(dr.GetOrdinal("TUBERIATIPO")) ? dr.GetString(dr.GetOrdinal("TUBERIATIPO")) : string.Empty;
                    ob.Maquinatipo = !dr.IsDBNull(dr.GetOrdinal("MAQUINATIPO")) ? dr.GetString(dr.GetOrdinal("MAQUINATIPO")) : string.Empty;
                    ob.Maquinaaltitud = !dr.IsDBNull(dr.GetOrdinal("MAQUINAALTITUD")) ? dr.GetString(dr.GetOrdinal("MAQUINAALTITUD")) : string.Empty;
                    ob.Regestacionalvbruto = !dr.IsDBNull(dr.GetOrdinal("REGESTACIONALVBRUTO")) ? dr.GetString(dr.GetOrdinal("REGESTACIONALVBRUTO")) : string.Empty;
                    ob.Regestacionalvutil = !dr.IsDBNull(dr.GetOrdinal("REGESTACIONALVUTIL")) ? dr.GetString(dr.GetOrdinal("REGESTACIONALVUTIL")) : string.Empty;
                    ob.Regestacionalhpresa = !dr.IsDBNull(dr.GetOrdinal("REGESTACIONALHPRESA")) ? dr.GetString(dr.GetOrdinal("REGESTACIONALHPRESA")) : string.Empty;
                    ob.Reghorariavutil = !dr.IsDBNull(dr.GetOrdinal("REGHORARIAVUTIL")) ? dr.GetString(dr.GetOrdinal("REGHORARIAVUTIL")) : string.Empty;
                    ob.Reghorariahpresa = !dr.IsDBNull(dr.GetOrdinal("REGHORARIAHPRESA")) ? dr.GetString(dr.GetOrdinal("REGHORARIAHPRESA")) : string.Empty;
                    ob.Reghorariaubicacion = !dr.IsDBNull(dr.GetOrdinal("REGHORARIAUBICACION")) ? dr.GetString(dr.GetOrdinal("REGHORARIAUBICACION")) : string.Empty;
                    ob.Energhorapunta = !dr.IsDBNull(dr.GetOrdinal("ENERGHORAPUNTA")) ? dr.GetString(dr.GetOrdinal("ENERGHORAPUNTA")) : string.Empty;
                    ob.Energfuerapunta = !dr.IsDBNull(dr.GetOrdinal("ENERGFUERAPUNTA")) ? dr.GetString(dr.GetOrdinal("ENERGFUERAPUNTA")) : string.Empty;
                    ob.Tipoturbina = !dr.IsDBNull(dr.GetOrdinal("TIPOTURBINA")) ? dr.GetString(dr.GetOrdinal("TIPOTURBINA")) : string.Empty;
                    ob.Velnomrotacion = !dr.IsDBNull(dr.GetOrdinal("VELNOMROTACION")) ? dr.GetString(dr.GetOrdinal("VELNOMROTACION")) : string.Empty;
                    ob.Potturbina = !dr.IsDBNull(dr.GetOrdinal("POTTURBINA")) ? dr.GetString(dr.GetOrdinal("POTTURBINA")) : string.Empty;
                    ob.Numturbinas = !dr.IsDBNull(dr.GetOrdinal("NUMTURBINAS")) ? dr.GetString(dr.GetOrdinal("NUMTURBINAS")) : string.Empty;
                    ob.Potgenerador = !dr.IsDBNull(dr.GetOrdinal("POTGENERADOR")) ? dr.GetString(dr.GetOrdinal("POTGENERADOR")) : string.Empty;
                    ob.Numgeneradores = !dr.IsDBNull(dr.GetOrdinal("NUMGENERADORES")) ? dr.GetString(dr.GetOrdinal("NUMGENERADORES")) : string.Empty;
                    ob.Tensiongeneracion = !dr.IsDBNull(dr.GetOrdinal("TENSIONGENERACION")) ? dr.GetString(dr.GetOrdinal("TENSIONGENERACION")) : string.Empty;
                    ob.Rendimientogenerador = !dr.IsDBNull(dr.GetOrdinal("RENDIMIENTOGENERADOR")) ? dr.GetString(dr.GetOrdinal("RENDIMIENTOGENERADOR")) : string.Empty;
                    ob.Tensionkv = !dr.IsDBNull(dr.GetOrdinal("TENSIONKV")) ? dr.GetString(dr.GetOrdinal("TENSIONKV")) : string.Empty;
                    ob.Longitudkm = !dr.IsDBNull(dr.GetOrdinal("LONGITUDKM")) ? dr.GetString(dr.GetOrdinal("LONGITUDKM")) : string.Empty;
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
                    listOb.Add(ob);
                }

            }
            return listOb;
        }


    }
}
