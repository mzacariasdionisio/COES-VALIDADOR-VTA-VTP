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
    public class CamBioHojaARepository : RepositoryBase, ICamBioHojaARepository
    {

        public CamBioHojaARepository(string strConn) : base(strConn) { }

        CamBioHojaAHelper Helper = new CamBioHojaAHelper();

        public List<BioHojaADTO> GetBioHojaAProyCodi(int proyCodi)
        {
            List<BioHojaADTO> bioHojaADTOs = new List<BioHojaADTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetBioHojaAProyCodi);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.String, proyCodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    BioHojaADTO ob = new BioHojaADTO();
                    ob.BiohojaaCodi = !dr.IsDBNull(dr.GetOrdinal("BIOHOJAACODI")) ? dr.GetInt32(dr.GetOrdinal("BIOHOJAACODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.CentralNombre = dr.IsDBNull(dr.GetOrdinal("CENTRALNOMBRE")) ? null : dr.GetString(dr.GetOrdinal("CENTRALNOMBRE"));
                    ob.Distrito = dr.IsDBNull(dr.GetOrdinal("DISTRITO")) ? null : dr.GetString(dr.GetOrdinal("DISTRITO"));
                    ob.Propietario = dr.IsDBNull(dr.GetOrdinal("PROPIETARIO")) ? null : dr.GetString(dr.GetOrdinal("PROPIETARIO"));
                    ob.Otro = dr.IsDBNull(dr.GetOrdinal("OTRO")) ? null : dr.GetString(dr.GetOrdinal("OTRO"));
                    ob.SocioOperador = dr.IsDBNull(dr.GetOrdinal("SOCIOOPERADOR")) ? null : dr.GetString(dr.GetOrdinal("SOCIOOPERADOR"));
                    ob.SocioInversionista = dr.IsDBNull(dr.GetOrdinal("SOCIOINVERSIONISTA")) ? null : dr.GetString(dr.GetOrdinal("SOCIOINVERSIONISTA"));
                    ob.ConTemporal = dr.IsDBNull(dr.GetOrdinal("CONTEMPORAL")) ? null : dr.GetString(dr.GetOrdinal("CONTEMPORAL"));
                    ob.FecAdjudicacionTemp = dr.IsDBNull(dr.GetOrdinal("FECADJUDICACIONTEMP")) ? (DateTime?)null : dr.GetDateTime(dr.GetOrdinal("FECADJUDICACIONTEMP"));
                    ob.TipoConActual = dr.IsDBNull(dr.GetOrdinal("TIPOCONACTUAL")) ? null : dr.GetString(dr.GetOrdinal("TIPOCONACTUAL"));
                    ob.FecAdjudicacionAct = dr.IsDBNull(dr.GetOrdinal("FECADJUDICACIONACT")) ? (DateTime?)null : dr.GetDateTime(dr.GetOrdinal("FECADJUDICACIONACT"));
                    ob.PotInstalada = !dr.IsDBNull(dr.GetOrdinal("POTINSTALADA")) ? dr.GetDecimal(dr.GetOrdinal("POTINSTALADA")) : (decimal?)null;
                    ob.TipoNomComb = dr.IsDBNull(dr.GetOrdinal("TIPONOMCOMB")) ? null : dr.GetString(dr.GetOrdinal("TIPONOMCOMB"));
                    ob.OtroComb = dr.IsDBNull(dr.GetOrdinal("OTROCOMB")) ? null : dr.GetString(dr.GetOrdinal("OTROCOMB"));
                    ob.PotMaxima = !dr.IsDBNull(dr.GetOrdinal("POTMAXIMA")) ? dr.GetDecimal(dr.GetOrdinal("POTMAXIMA")) : (decimal?)null;
                    ob.PoderCalorInf = !dr.IsDBNull(dr.GetOrdinal("PODERCALORINF")) ? dr.GetDecimal(dr.GetOrdinal("PODERCALORINF")) : (decimal?)null;
                    ob.CombPoderCalorInf = dr.IsDBNull(dr.GetOrdinal("COMBPODERCALORINF")) ? null : dr.GetString(dr.GetOrdinal("COMBPODERCALORINF"));
                    ob.PotMinima = !dr.IsDBNull(dr.GetOrdinal("POTMINIMA")) ? dr.GetDecimal(dr.GetOrdinal("POTMINIMA")) : (decimal?)null;
                    ob.PoderCalorSup = !dr.IsDBNull(dr.GetOrdinal("PODERCALORSUP")) ? dr.GetDecimal(dr.GetOrdinal("PODERCALORSUP")) : (decimal?)null;
                    ob.CombPoderCalorSup = dr.IsDBNull(dr.GetOrdinal("COMBPODERCALORSUP")) ? null : dr.GetString(dr.GetOrdinal("COMBPODERCALORSUP"));
                    ob.CostCombustible = !dr.IsDBNull(dr.GetOrdinal("COSTCOMBUSTIBLE")) ? dr.GetDecimal(dr.GetOrdinal("COSTCOMBUSTIBLE")) : (decimal?)null;
                    ob.CombCostoCombustible = dr.IsDBNull(dr.GetOrdinal("COMBCOSTOCOMBUSTIBLE")) ? null : dr.GetString(dr.GetOrdinal("COMBCOSTOCOMBUSTIBLE"));
                    ob.CostTratamiento = !dr.IsDBNull(dr.GetOrdinal("COSTTRATAMIENTO")) ? dr.GetDecimal(dr.GetOrdinal("COSTTRATAMIENTO")) : (decimal?)null;
                    ob.CombCostTratamiento = dr.IsDBNull(dr.GetOrdinal("COMBCOSTTRAMIENTO")) ? null : dr.GetString(dr.GetOrdinal("COMBCOSTTRAMIENTO"));
                    ob.CostTransporte = !dr.IsDBNull(dr.GetOrdinal("COSTTRANSPORTE")) ? dr.GetDecimal(dr.GetOrdinal("COSTTRANSPORTE")) : (decimal?)null;
                    ob.CombCostTransporte = dr.IsDBNull(dr.GetOrdinal("COMBCOSTTRANSPORTE")) ? null : dr.GetString(dr.GetOrdinal("COMBCOSTTRANSPORTE"));
                    ob.CostoVariableNoComb = !dr.IsDBNull(dr.GetOrdinal("COSTOVARIABLENOCOMB")) ? dr.GetDecimal(dr.GetOrdinal("COSTOVARIABLENOCOMB")) : (decimal?)null;
                    ob.CombCostoVariableNoComb = dr.IsDBNull(dr.GetOrdinal("COMBCOSTOVARIABLENOCOMB")) ? null : dr.GetString(dr.GetOrdinal("COMBCOSTOVARIABLENOCOMB"));
                    ob.CostInversion = !dr.IsDBNull(dr.GetOrdinal("COSTINVERSION")) ? dr.GetDecimal(dr.GetOrdinal("COSTINVERSION")) : (decimal?)null;
                    ob.CombCostoInversion = dr.IsDBNull(dr.GetOrdinal("COMBCOSTOINVERSION")) ? null : dr.GetString(dr.GetOrdinal("COMBCOSTOINVERSION"));
                    ob.RendPlanta = !dr.IsDBNull(dr.GetOrdinal("RENDPLANTA")) ? dr.GetDecimal(dr.GetOrdinal("RENDPLANTA")) : (decimal?)null;
                    ob.CombRendPlanta = dr.IsDBNull(dr.GetOrdinal("COMBRENDPLANTA")) ? null : dr.GetString(dr.GetOrdinal("COMBRENDPLANTA"));
                    ob.ConsEspec = !dr.IsDBNull(dr.GetOrdinal("CONSESPEC")) ? dr.GetDecimal(dr.GetOrdinal("CONSESPEC")) : (decimal?)null;
                    ob.CombConsEspec = dr.IsDBNull(dr.GetOrdinal("COMBCONSESPEC")) ? null : dr.GetString(dr.GetOrdinal("COMBCONSESPEC"));
                    ob.TipoMotorTer = dr.IsDBNull(dr.GetOrdinal("TIPOMOTORTER")) ? null : dr.GetString(dr.GetOrdinal("TIPOMOTORTER"));
                    ob.VelNomRotacion = !dr.IsDBNull(dr.GetOrdinal("VELNOMROTACION")) ? dr.GetDecimal(dr.GetOrdinal("VELNOMROTACION")) : (decimal?)null;
                    ob.PotEjeMotorTer = !dr.IsDBNull(dr.GetOrdinal("POTEJEMOTORTER")) ? dr.GetDecimal(dr.GetOrdinal("POTEJEMOTORTER")) : (decimal?)null;
                    ob.NumMotoresTer = !dr.IsDBNull(dr.GetOrdinal("NUMMOTORESTER")) ? dr.GetDecimal(dr.GetOrdinal("NUMMOTORESTER")) : (decimal?)null;
                    ob.PotNomGenerador = !dr.IsDBNull(dr.GetOrdinal("POTNOMGENERADOR")) ? dr.GetDecimal(dr.GetOrdinal("POTNOMGENERADOR")) : (decimal?)null;
                    ob.NumGeneradores = !dr.IsDBNull(dr.GetOrdinal("NUMGENERADORES")) ? dr.GetDecimal(dr.GetOrdinal("NUMGENERADORES")) : (decimal?)null;
                    ob.TipoGenerador = !dr.IsDBNull(dr.GetOrdinal("TIPOGENERADOR")) ? dr.GetDecimal(dr.GetOrdinal("TIPOGENERADOR")) : (decimal?)null;
                    ob.TenGeneracion = !dr.IsDBNull(dr.GetOrdinal("TENGENERACION")) ? dr.GetDecimal(dr.GetOrdinal("TENGENERACION")) : (decimal?)null;
                    ob.Tension = !dr.IsDBNull(dr.GetOrdinal("TENSION")) ? dr.GetDecimal(dr.GetOrdinal("TENSION")) : (decimal?)null;
                    ob.Longitud = !dr.IsDBNull(dr.GetOrdinal("LONGITUD")) ? dr.GetDecimal(dr.GetOrdinal("LONGITUD")) : (decimal?)null;
                    ob.NumTernas = !dr.IsDBNull(dr.GetOrdinal("NUMTERNAS")) ? dr.GetDecimal(dr.GetOrdinal("NUMTERNAS")) : (decimal?)null;
                    ob.NomSubEstacion = dr.IsDBNull(dr.GetOrdinal("NOMSUBESTACION")) ? null : dr.GetString(dr.GetOrdinal("NOMSUBESTACION"));
                    ob.OtroSubEstacion = dr.IsDBNull(dr.GetOrdinal("OTROSUBESTACION")) ? null : dr.GetString(dr.GetOrdinal("OTROSUBESTACION"));
                    ob.Perfil = dr.IsDBNull(dr.GetOrdinal("PERFIL")) ? null : dr.GetString(dr.GetOrdinal("PERFIL"));
                    ob.Prefactibilidad = dr.IsDBNull(dr.GetOrdinal("PREFACTIBILIDAD")) ? null : dr.GetString(dr.GetOrdinal("PREFACTIBILIDAD"));
                    ob.Factibilidad = dr.IsDBNull(dr.GetOrdinal("FACTIBILIDAD")) ? null : dr.GetString(dr.GetOrdinal("FACTIBILIDAD"));
                    ob.EstDefinitivo = dr.IsDBNull(dr.GetOrdinal("ESTDEFINITIVO")) ? null : dr.GetString(dr.GetOrdinal("ESTDEFINITIVO"));
                    ob.Eia = dr.IsDBNull(dr.GetOrdinal("EIA")) ? null : dr.GetString(dr.GetOrdinal("EIA"));
                    ob.FecInicioConst = dr.IsDBNull(dr.GetOrdinal("FECINICIOCONST")) ? null : dr.GetString(dr.GetOrdinal("FECINICIOCONST"));
                    ob.PeriodoConst = dr.IsDBNull(dr.GetOrdinal("PERIODOCONST")) ? null : dr.GetString(dr.GetOrdinal("PERIODOCONST"));
                    ob.FecOperacionComer = dr.IsDBNull(dr.GetOrdinal("FECOPERACIONCOMER")) ? null : dr.GetString(dr.GetOrdinal("FECOPERACIONCOMER"));
                    ob.Comentarios = dr.IsDBNull(dr.GetOrdinal("COMENTARIOS")) ? null : dr.GetString(dr.GetOrdinal("COMENTARIOS"));
                    ob.UsuCreacion = dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? null : dr.GetString(dr.GetOrdinal("USU_CREACION"));
                    ob.FecCreacion = dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? DateTime.MinValue : dr.GetDateTime(dr.GetOrdinal("FEC_CREACION"));
                    ob.IndDel = dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? null : dr.GetString(dr.GetOrdinal("IND_DEL"));
                    bioHojaADTOs.Add(ob);
                }
            }

            return bioHojaADTOs;
        }


        public bool SaveBioHojaA(BioHojaADTO bioHojaADTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveBioHojaA);
            dbProvider.AddInParameter(dbCommand, "BIOHOJAACODI", DbType.Int32, bioHojaADTO.BiohojaaCodi);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, bioHojaADTO.ProyCodi);
            dbProvider.AddInParameter(dbCommand, "CENTRALNOMBRE", DbType.String, bioHojaADTO.CentralNombre);
            dbProvider.AddInParameter(dbCommand, "DISTRITO", DbType.String, bioHojaADTO.Distrito);
            dbProvider.AddInParameter(dbCommand, "PROPIETARIO", DbType.String, bioHojaADTO.Propietario);
            dbProvider.AddInParameter(dbCommand, "OTRO", DbType.String, bioHojaADTO.Otro);
            dbProvider.AddInParameter(dbCommand, "SOCIOOPERADOR", DbType.String, bioHojaADTO.SocioOperador);
            dbProvider.AddInParameter(dbCommand, "SOCIOINVERSIONISTA", DbType.String, bioHojaADTO.SocioInversionista);
            dbProvider.AddInParameter(dbCommand, "CONTEMPORAL", DbType.String, bioHojaADTO.ConTemporal);
            dbProvider.AddInParameter(dbCommand, "FECADJUDICACIONTEMP", DbType.DateTime, bioHojaADTO.FecAdjudicacionTemp);
            dbProvider.AddInParameter(dbCommand, "TIPOCONACTUAL", DbType.String, bioHojaADTO.TipoConActual);
            dbProvider.AddInParameter(dbCommand, "FECADJUDICACIONACT", DbType.DateTime, bioHojaADTO.FecAdjudicacionAct);
            dbProvider.AddInParameter(dbCommand, "POTINSTALADA", DbType.Decimal, bioHojaADTO.PotInstalada ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "TIPONOMCOMB", DbType.String, bioHojaADTO.TipoNomComb);
            dbProvider.AddInParameter(dbCommand, "OTROCOMB", DbType.String, bioHojaADTO.OtroComb);
            dbProvider.AddInParameter(dbCommand, "POTMAXIMA", DbType.Decimal, bioHojaADTO.PotMaxima ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "PODERCALORINF", DbType.Decimal, bioHojaADTO.PoderCalorInf ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "COMBPODERCALORINF", DbType.String, bioHojaADTO.CombPoderCalorInf);
            dbProvider.AddInParameter(dbCommand, "POTMINIMA", DbType.Decimal, bioHojaADTO.PotMinima ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "PODERCALORSUP", DbType.Decimal, bioHojaADTO.PoderCalorSup ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "COMBPODERCALORSUP", DbType.String, bioHojaADTO.CombPoderCalorSup);
            dbProvider.AddInParameter(dbCommand, "COSTCOMBUSTIBLE", DbType.Decimal, bioHojaADTO.CostCombustible ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "COMBCOSTOCOMBUSTIBLE", DbType.String, bioHojaADTO.CombCostoCombustible);
            dbProvider.AddInParameter(dbCommand, "COSTTRATAMIENTO", DbType.Decimal, bioHojaADTO.CostTratamiento ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "COMBCOSTTRAMIENTO", DbType.String, bioHojaADTO.CombCostTratamiento);
            dbProvider.AddInParameter(dbCommand, "COSTTRANSPORTE", DbType.Decimal, bioHojaADTO.CostTransporte ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "COMBCOSTTRANSPORTE", DbType.String, bioHojaADTO.CombCostTransporte);
            dbProvider.AddInParameter(dbCommand, "COSTOVARIABLENOCOMB", DbType.Decimal, bioHojaADTO.CostoVariableNoComb ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "COMBCOSTOVARIABLENOCOMB", DbType.String, bioHojaADTO.CombCostoVariableNoComb);
            dbProvider.AddInParameter(dbCommand, "COSTINVERSION", DbType.Decimal, bioHojaADTO.CostInversion ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "COMBCOSTOINVERSION", DbType.String, bioHojaADTO.CombCostoInversion);
            dbProvider.AddInParameter(dbCommand, "RENDPLANTA", DbType.Decimal, bioHojaADTO.RendPlanta ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "COMBRENDPLANTA", DbType.String, bioHojaADTO.CombRendPlanta);
            dbProvider.AddInParameter(dbCommand, "CONSESPEC", DbType.Decimal, bioHojaADTO.ConsEspec ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "COMBCONSESPEC", DbType.String, bioHojaADTO.CombConsEspec);
            dbProvider.AddInParameter(dbCommand, "TIPOMOTORTER", DbType.String, bioHojaADTO.TipoMotorTer);
            dbProvider.AddInParameter(dbCommand, "VELNOMROTACION", DbType.Decimal, bioHojaADTO.VelNomRotacion ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "POTEJEMOTORTER", DbType.Decimal, bioHojaADTO.PotEjeMotorTer ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "NUMMOTORESTER", DbType.Decimal, bioHojaADTO.NumMotoresTer ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "POTNOMGENERADOR", DbType.Decimal, bioHojaADTO.PotNomGenerador ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "NUMGENERADORES", DbType.Decimal, bioHojaADTO.NumGeneradores ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "TIPOGENERADOR", DbType.Decimal, bioHojaADTO.TipoGenerador ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "TENGENERACION", DbType.Decimal, bioHojaADTO.TenGeneracion ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "TENSION", DbType.Decimal, bioHojaADTO.Tension ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "LONGITUD", DbType.Decimal, bioHojaADTO.Longitud ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "NUMTERNAS", DbType.Decimal, bioHojaADTO.NumTernas ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "NOMSUBESTACION", DbType.String, bioHojaADTO.NomSubEstacion);
            dbProvider.AddInParameter(dbCommand, "OTROSUBESTACION", DbType.String, bioHojaADTO.OtroSubEstacion);
            dbProvider.AddInParameter(dbCommand, "PERFIL", DbType.String, bioHojaADTO.Perfil);
            dbProvider.AddInParameter(dbCommand, "PREFACTIBILIDAD", DbType.String, bioHojaADTO.Prefactibilidad);
            dbProvider.AddInParameter(dbCommand, "FACTIBILIDAD", DbType.String, bioHojaADTO.Factibilidad);
            dbProvider.AddInParameter(dbCommand, "ESTDEFINITIVO", DbType.String, bioHojaADTO.EstDefinitivo);
            dbProvider.AddInParameter(dbCommand, "EIA", DbType.String, bioHojaADTO.Eia);
            dbProvider.AddInParameter(dbCommand, "FECINICIOCONST", DbType.String, bioHojaADTO.FecInicioConst);
            dbProvider.AddInParameter(dbCommand, "PERIODOCONST", DbType.String, bioHojaADTO.PeriodoConst);
            dbProvider.AddInParameter(dbCommand, "FECOPERACIONCOMER", DbType.String, bioHojaADTO.FecOperacionComer);
            dbProvider.AddInParameter(dbCommand, "COMENTARIOS", DbType.String, bioHojaADTO.Comentarios);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, bioHojaADTO.UsuCreacion);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, bioHojaADTO.FecCreacion);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, bioHojaADTO.IndDel);

            dbProvider.ExecuteNonQuery(dbCommand);

            return true;
        }

        public bool DeleteBioHojaAById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteBioHojaAById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);
            return true;
        }

        public int GetLastBioHojaAId()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastBioHojaAId);
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

        public BioHojaADTO GetBioHojaAById(int id)
        {
            BioHojaADTO ob = new BioHojaADTO();
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(Helper.SqlGetBioHojaAById);
            dbProvider.AddInParameter(commandHoja, "PROYCODI", DbType.Int32, id);
            dbProvider.AddInParameter(commandHoja, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.ExecuteNonQuery(commandHoja);
            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                if (dr.Read())
                {
                    ob.BiohojaaCodi = !dr.IsDBNull(dr.GetOrdinal("BIOHOJAACODI")) ? dr.GetInt32(dr.GetOrdinal("BIOHOJAACODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.CentralNombre = dr.IsDBNull(dr.GetOrdinal("CENTRALNOMBRE")) ? null : dr.GetString(dr.GetOrdinal("CENTRALNOMBRE"));
                    ob.Distrito = dr.IsDBNull(dr.GetOrdinal("DISTRITO")) ? null : dr.GetString(dr.GetOrdinal("DISTRITO"));
                    ob.Propietario = dr.IsDBNull(dr.GetOrdinal("PROPIETARIO")) ? null : dr.GetString(dr.GetOrdinal("PROPIETARIO"));
                    ob.Otro = dr.IsDBNull(dr.GetOrdinal("OTRO")) ? null : dr.GetString(dr.GetOrdinal("OTRO"));
                    ob.SocioOperador = dr.IsDBNull(dr.GetOrdinal("SOCIOOPERADOR")) ? null : dr.GetString(dr.GetOrdinal("SOCIOOPERADOR"));
                    ob.SocioInversionista = dr.IsDBNull(dr.GetOrdinal("SOCIOINVERSIONISTA")) ? null : dr.GetString(dr.GetOrdinal("SOCIOINVERSIONISTA"));
                    ob.ConTemporal = dr.IsDBNull(dr.GetOrdinal("CONTEMPORAL")) ? null : dr.GetString(dr.GetOrdinal("CONTEMPORAL"));
                    ob.FecAdjudicacionTemp = dr.IsDBNull(dr.GetOrdinal("FECADJUDICACIONTEMP")) ? (DateTime?)null : dr.GetDateTime(dr.GetOrdinal("FECADJUDICACIONTEMP"));
                    ob.TipoConActual = dr.IsDBNull(dr.GetOrdinal("TIPOCONACTUAL")) ? null : dr.GetString(dr.GetOrdinal("TIPOCONACTUAL"));
                    ob.FecAdjudicacionAct = dr.IsDBNull(dr.GetOrdinal("FECADJUDICACIONACT")) ? (DateTime?)null : dr.GetDateTime(dr.GetOrdinal("FECADJUDICACIONACT"));
                    ob.PotInstalada = !dr.IsDBNull(dr.GetOrdinal("POTINSTALADA")) ? dr.GetDecimal(dr.GetOrdinal("POTINSTALADA")) : (decimal?)null;
                    ob.TipoNomComb = dr.IsDBNull(dr.GetOrdinal("TIPONOMCOMB")) ? null : dr.GetString(dr.GetOrdinal("TIPONOMCOMB"));
                    ob.OtroComb = dr.IsDBNull(dr.GetOrdinal("OTROCOMB")) ? null : dr.GetString(dr.GetOrdinal("OTROCOMB"));
                    ob.PotMaxima = !dr.IsDBNull(dr.GetOrdinal("POTMAXIMA")) ? dr.GetDecimal(dr.GetOrdinal("POTMAXIMA")) : (decimal?)null;
                    ob.PoderCalorInf = !dr.IsDBNull(dr.GetOrdinal("PODERCALORINF")) ? dr.GetDecimal(dr.GetOrdinal("PODERCALORINF")) : (decimal?)null;
                    ob.CombPoderCalorInf = dr.IsDBNull(dr.GetOrdinal("COMBPODERCALORINF")) ? null : dr.GetString(dr.GetOrdinal("COMBPODERCALORINF"));
                    ob.PotMinima = !dr.IsDBNull(dr.GetOrdinal("POTMINIMA")) ? dr.GetDecimal(dr.GetOrdinal("POTMINIMA")) : (decimal?)null;
                    ob.PoderCalorSup = !dr.IsDBNull(dr.GetOrdinal("PODERCALORSUP")) ? dr.GetDecimal(dr.GetOrdinal("PODERCALORSUP")) : (decimal?)null;
                    ob.CombPoderCalorSup = dr.IsDBNull(dr.GetOrdinal("COMBPODERCALORSUP")) ? null : dr.GetString(dr.GetOrdinal("COMBPODERCALORSUP"));
                    ob.CostCombustible = !dr.IsDBNull(dr.GetOrdinal("COSTCOMBUSTIBLE")) ? dr.GetDecimal(dr.GetOrdinal("COSTCOMBUSTIBLE")) : (decimal?)null;
                    ob.CombCostoCombustible = dr.IsDBNull(dr.GetOrdinal("COMBCOSTOCOMBUSTIBLE")) ? null : dr.GetString(dr.GetOrdinal("COMBCOSTOCOMBUSTIBLE"));
                    ob.CostTratamiento = !dr.IsDBNull(dr.GetOrdinal("COSTTRATAMIENTO")) ? dr.GetDecimal(dr.GetOrdinal("COSTTRATAMIENTO")) : (decimal?)null;
                    ob.CombCostTratamiento = dr.IsDBNull(dr.GetOrdinal("COMBCOSTTRAMIENTO")) ? null : dr.GetString(dr.GetOrdinal("COMBCOSTTRAMIENTO"));
                    ob.CostTransporte = !dr.IsDBNull(dr.GetOrdinal("COSTTRANSPORTE")) ? dr.GetDecimal(dr.GetOrdinal("COSTTRANSPORTE")) : (decimal?)null;
                    ob.CombCostTransporte = dr.IsDBNull(dr.GetOrdinal("COMBCOSTTRANSPORTE")) ? null : dr.GetString(dr.GetOrdinal("COMBCOSTTRANSPORTE"));
                    ob.CostoVariableNoComb = !dr.IsDBNull(dr.GetOrdinal("COSTOVARIABLENOCOMB")) ? dr.GetDecimal(dr.GetOrdinal("COSTOVARIABLENOCOMB")) : (decimal?)null;
                    ob.CombCostoVariableNoComb = dr.IsDBNull(dr.GetOrdinal("COMBCOSTOVARIABLENOCOMB")) ? null : dr.GetString(dr.GetOrdinal("COMBCOSTOVARIABLENOCOMB"));
                    ob.CostInversion = !dr.IsDBNull(dr.GetOrdinal("COSTINVERSION")) ? dr.GetDecimal(dr.GetOrdinal("COSTINVERSION")) : (decimal?)null;
                    ob.CombCostoInversion = dr.IsDBNull(dr.GetOrdinal("COMBCOSTOINVERSION")) ? null : dr.GetString(dr.GetOrdinal("COMBCOSTOINVERSION"));
                    ob.RendPlanta = !dr.IsDBNull(dr.GetOrdinal("RENDPLANTA")) ? dr.GetDecimal(dr.GetOrdinal("RENDPLANTA")) : (decimal?)null;
                    ob.CombRendPlanta = dr.IsDBNull(dr.GetOrdinal("COMBRENDPLANTA")) ? null : dr.GetString(dr.GetOrdinal("COMBRENDPLANTA"));
                    ob.ConsEspec = !dr.IsDBNull(dr.GetOrdinal("CONSESPEC")) ? dr.GetDecimal(dr.GetOrdinal("CONSESPEC")) : (decimal?)null;
                    ob.CombConsEspec = dr.IsDBNull(dr.GetOrdinal("COMBCONSESPEC")) ? null : dr.GetString(dr.GetOrdinal("COMBCONSESPEC"));
                    ob.TipoMotorTer = dr.IsDBNull(dr.GetOrdinal("TIPOMOTORTER")) ? null : dr.GetString(dr.GetOrdinal("TIPOMOTORTER"));
                    ob.VelNomRotacion = !dr.IsDBNull(dr.GetOrdinal("VELNOMROTACION")) ? dr.GetDecimal(dr.GetOrdinal("VELNOMROTACION")) : (decimal?)null;
                    ob.PotEjeMotorTer = !dr.IsDBNull(dr.GetOrdinal("POTEJEMOTORTER")) ? dr.GetDecimal(dr.GetOrdinal("POTEJEMOTORTER")) : (decimal?)null;
                    ob.NumMotoresTer = !dr.IsDBNull(dr.GetOrdinal("NUMMOTORESTER")) ? dr.GetDecimal(dr.GetOrdinal("NUMMOTORESTER")) : (decimal?)null;
                    ob.PotNomGenerador = !dr.IsDBNull(dr.GetOrdinal("POTNOMGENERADOR")) ? dr.GetDecimal(dr.GetOrdinal("POTNOMGENERADOR")) : (decimal?)null;
                    ob.NumGeneradores = !dr.IsDBNull(dr.GetOrdinal("NUMGENERADORES")) ? dr.GetDecimal(dr.GetOrdinal("NUMGENERADORES")) : (decimal?)null;
                    ob.TipoGenerador = !dr.IsDBNull(dr.GetOrdinal("TIPOGENERADOR")) ? dr.GetDecimal(dr.GetOrdinal("TIPOGENERADOR")) : (decimal?)null;
                    ob.TenGeneracion = !dr.IsDBNull(dr.GetOrdinal("TENGENERACION")) ? dr.GetDecimal(dr.GetOrdinal("TENGENERACION")) : (decimal?)null;
                    ob.Tension = !dr.IsDBNull(dr.GetOrdinal("TENSION")) ? dr.GetDecimal(dr.GetOrdinal("TENSION")) : (decimal?)null;
                    ob.Longitud = !dr.IsDBNull(dr.GetOrdinal("LONGITUD")) ? dr.GetDecimal(dr.GetOrdinal("LONGITUD")) : (decimal?)null;
                    ob.NumTernas = !dr.IsDBNull(dr.GetOrdinal("NUMTERNAS")) ? dr.GetDecimal(dr.GetOrdinal("NUMTERNAS")) : (decimal?)null;
                    ob.NomSubEstacion = dr.IsDBNull(dr.GetOrdinal("NOMSUBESTACION")) ? null : dr.GetString(dr.GetOrdinal("NOMSUBESTACION"));
                    ob.OtroSubEstacion = dr.IsDBNull(dr.GetOrdinal("OTROSUBESTACION")) ? null : dr.GetString(dr.GetOrdinal("OTROSUBESTACION"));
                    ob.Perfil = dr.IsDBNull(dr.GetOrdinal("PERFIL")) ? null : dr.GetString(dr.GetOrdinal("PERFIL"));
                    ob.Prefactibilidad = dr.IsDBNull(dr.GetOrdinal("PREFACTIBILIDAD")) ? null : dr.GetString(dr.GetOrdinal("PREFACTIBILIDAD"));
                    ob.Factibilidad = dr.IsDBNull(dr.GetOrdinal("FACTIBILIDAD")) ? null : dr.GetString(dr.GetOrdinal("FACTIBILIDAD"));
                    ob.EstDefinitivo = dr.IsDBNull(dr.GetOrdinal("ESTDEFINITIVO")) ? null : dr.GetString(dr.GetOrdinal("ESTDEFINITIVO"));
                    ob.Eia = dr.IsDBNull(dr.GetOrdinal("EIA")) ? null : dr.GetString(dr.GetOrdinal("EIA"));
                    ob.FecInicioConst = dr.IsDBNull(dr.GetOrdinal("FECINICIOCONST")) ? null : dr.GetString(dr.GetOrdinal("FECINICIOCONST"));
                    ob.PeriodoConst = dr.IsDBNull(dr.GetOrdinal("PERIODOCONST")) ? null : dr.GetString(dr.GetOrdinal("PERIODOCONST"));
                    ob.FecOperacionComer = dr.IsDBNull(dr.GetOrdinal("FECOPERACIONCOMER")) ? null : dr.GetString(dr.GetOrdinal("FECOPERACIONCOMER"));
                    ob.Comentarios = dr.IsDBNull(dr.GetOrdinal("COMENTARIOS")) ? null : dr.GetString(dr.GetOrdinal("COMENTARIOS"));
                    ob.UsuCreacion = dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? null : dr.GetString(dr.GetOrdinal("USU_CREACION"));
                    ob.FecCreacion = dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? DateTime.MinValue : dr.GetDateTime(dr.GetOrdinal("FEC_CREACION"));
                    ob.IndDel = dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? null : dr.GetString(dr.GetOrdinal("IND_DEL"));
                }
            }
            return ob;
        }

        public bool UpdateBioHojaA(BioHojaADTO bioHojaADTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlUpdateBioHojaA);
            
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, ObtenerValorOrDefault(bioHojaADTO.ProyCodi, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "CENTRALNOMBRE", DbType.String, ObtenerValorOrDefault(bioHojaADTO.CentralNombre, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "DISTRITO", DbType.String, ObtenerValorOrDefault(bioHojaADTO.Distrito, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "PROPIETARIO", DbType.String, ObtenerValorOrDefault(bioHojaADTO.Propietario, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "OTRO", DbType.String, ObtenerValorOrDefault(bioHojaADTO.Otro, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "SOCIOOPERADOR", DbType.String, ObtenerValorOrDefault(bioHojaADTO.SocioOperador, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "SOCIOINVERSIONISTA", DbType.String, ObtenerValorOrDefault(bioHojaADTO.SocioInversionista, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "CONTEMPORAL", DbType.String, ObtenerValorOrDefault(bioHojaADTO.ConTemporal, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "FECADJUDICACIONTEMP", DbType.DateTime, ObtenerValorOrDefault(bioHojaADTO.FecAdjudicacionTemp, typeof(DateTime?)));
            dbProvider.AddInParameter(dbCommand, "TIPOCONACTUAL", DbType.String, ObtenerValorOrDefault(bioHojaADTO.TipoConActual, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "FECADJUDICACIONACT", DbType.DateTime, ObtenerValorOrDefault(bioHojaADTO.FecAdjudicacionAct, typeof(DateTime?)));
            dbProvider.AddInParameter(dbCommand, "POTINSTALADA", DbType.Int32, ObtenerValorOrDefault(bioHojaADTO.PotInstalada, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "TIPONOMCOMB", DbType.String, ObtenerValorOrDefault(bioHojaADTO.TipoNomComb, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "OTROCOMB", DbType.String, ObtenerValorOrDefault(bioHojaADTO.OtroComb, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "POTMAXIMA", DbType.Int32, ObtenerValorOrDefault(bioHojaADTO.PotMaxima, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "PODERCALORINF", DbType.Int32, ObtenerValorOrDefault(bioHojaADTO.PoderCalorInf, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "COMBPODERCALORINF", DbType.String, ObtenerValorOrDefault(bioHojaADTO.CombPoderCalorInf, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "POTMINIMA", DbType.Int32, ObtenerValorOrDefault(bioHojaADTO.PotMinima, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "PODERCALORSUP", DbType.Int32, ObtenerValorOrDefault(bioHojaADTO.PoderCalorSup, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "COMBPODERCALORSUP", DbType.String, ObtenerValorOrDefault(bioHojaADTO.CombPoderCalorSup, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "COSTCOMBUSTIBLE", DbType.Int32, ObtenerValorOrDefault(bioHojaADTO.CostCombustible, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "COMBCOSTOCOMBUSTIBLE", DbType.String, ObtenerValorOrDefault(bioHojaADTO.CombCostoCombustible, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "COSTTRATAMIENTO", DbType.Int32, ObtenerValorOrDefault(bioHojaADTO.CostTratamiento, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "COMBCOSTTRAMIENTO", DbType.String, ObtenerValorOrDefault(bioHojaADTO.CombCostTratamiento, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "COSTTRANSPORTE", DbType.Int32, ObtenerValorOrDefault(bioHojaADTO.CostTransporte, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "COMBCOSTTRANSPORTE", DbType.String, ObtenerValorOrDefault(bioHojaADTO.CombCostTransporte, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "COSTOVARIABLENOCOMB", DbType.Int32, ObtenerValorOrDefault(bioHojaADTO.CostoVariableNoComb, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "COMBCOSTOVARIABLENOCOMB", DbType.String, ObtenerValorOrDefault(bioHojaADTO.CombCostoVariableNoComb, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "COSTINVERSION", DbType.Int32, ObtenerValorOrDefault(bioHojaADTO.CostInversion, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "COMBCOSTOINVERSION", DbType.String, ObtenerValorOrDefault(bioHojaADTO.CombCostoInversion, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "RENDPLANTA", DbType.Int32, ObtenerValorOrDefault(bioHojaADTO.RendPlanta, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "COMBRENDPLANTA", DbType.String, ObtenerValorOrDefault(bioHojaADTO.CombRendPlanta, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "CONSESPEC", DbType.Int32, ObtenerValorOrDefault(bioHojaADTO.ConsEspec, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "COMBCONSESPEC", DbType.String, ObtenerValorOrDefault(bioHojaADTO.CombConsEspec, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "TIPOMOTORTER", DbType.String, ObtenerValorOrDefault(bioHojaADTO.TipoMotorTer, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "VELNOMROTACION", DbType.Int32, ObtenerValorOrDefault(bioHojaADTO.VelNomRotacion, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "POTEJEMOTORTER", DbType.Int32, ObtenerValorOrDefault(bioHojaADTO.PotEjeMotorTer, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "NUMMOTORESTER", DbType.Int32, ObtenerValorOrDefault(bioHojaADTO.NumMotoresTer, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "POTNOMGENERADOR", DbType.Int32, ObtenerValorOrDefault(bioHojaADTO.PotNomGenerador, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "NUMGENERADORES", DbType.Int32, ObtenerValorOrDefault(bioHojaADTO.NumGeneradores, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "TIPOGENERADOR", DbType.Decimal, ObtenerValorOrDefault(bioHojaADTO.TipoGenerador, typeof(Decimal)));
            dbProvider.AddInParameter(dbCommand, "TENGENERACION", DbType.Decimal, ObtenerValorOrDefault(bioHojaADTO.TenGeneracion, typeof(Decimal)));
            dbProvider.AddInParameter(dbCommand, "TENSION", DbType.Int32, ObtenerValorOrDefault(bioHojaADTO.Tension, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "LONGITUD", DbType.Int32, ObtenerValorOrDefault(bioHojaADTO.Longitud, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "NUMTERNAS", DbType.Int32, ObtenerValorOrDefault(bioHojaADTO.NumTernas, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "NOMSUBESTACION", DbType.String, ObtenerValorOrDefault(bioHojaADTO.NomSubEstacion, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "OTROSUBESTACION", DbType.String, ObtenerValorOrDefault(bioHojaADTO.OtroSubEstacion, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "PERFIL", DbType.String, ObtenerValorOrDefault(bioHojaADTO.Perfil, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "PREFACTIBILIDAD", DbType.String, ObtenerValorOrDefault(bioHojaADTO.Prefactibilidad, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "FACTIBILIDAD", DbType.String, ObtenerValorOrDefault(bioHojaADTO.Factibilidad, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "ESTDEFINITIVO", DbType.String, ObtenerValorOrDefault(bioHojaADTO.EstDefinitivo, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "EIA", DbType.String, ObtenerValorOrDefault(bioHojaADTO.Eia, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "FECINICIOCONST", DbType.String, ObtenerValorOrDefault(bioHojaADTO.FecInicioConst, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "PERIODOCONST", DbType.String, ObtenerValorOrDefault(bioHojaADTO.PeriodoConst, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "FECOPERACIONCOMER", DbType.String, ObtenerValorOrDefault(bioHojaADTO.FecOperacionComer, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "COMENTARIOS", DbType.String, ObtenerValorOrDefault(bioHojaADTO.Comentarios, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "USUMODIFICACION", DbType.String, ObtenerValorOrDefault(bioHojaADTO.UsuModificacion, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "FECMODIFICACION", DbType.DateTime, ObtenerValorOrDefault(bioHojaADTO.FecModificacion, typeof(DateTime?)));
            dbProvider.AddInParameter(dbCommand, "BIOHOJAACODI", DbType.Int32, ObtenerValorOrDefault(bioHojaADTO.BiohojaaCodi, typeof(int)));
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        object ObtenerValorOrDefault(object valor, Type tipo)
        {
            DateTime fechaMinimaValida = DateTime.Now;
            if (valor == null || (valor is DateTime && (DateTime)valor == DateTime.MinValue))
            {
                if (tipo == typeof(int) || tipo == typeof(int?) || tipo == typeof(Decimal) || tipo == typeof(Decimal?))
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


        public List<BioHojaADTO> GetBioHojaAByFilter(string plancodi, string empresa, string estado)
        {
            List<BioHojaADTO> oblist = new List<BioHojaADTO>();
            string query = $@"
                SELECT CGB.*, TR.EMPRESANOM, TR.PROYNOMBRE, TR.PROYDESCRIPCION, TP.TIPONOMBRE, TF.TIPOFINOMBRE,TR.PROYCONFIDENCIAL  FROM CAM_BIOHOJAA CGB
                INNER JOIN CAM_TRNSMPROYECTO TR ON TR.PROYCODI = CGB.PROYCODI
                INNER JOIN CAM_PLANTRANSMISION PL ON PL.PLANCODI = TR.PLANCODI
                INNER JOIN CAM_TIPOPROYECTO TP ON TP.TIPOCODI = TR.TIPOCODI
                LEFT JOIN CAM_TIPOFICHAPROYECTO TF ON TF.TIPOFICODI = TR.TIPOFICODI
                WHERE TR.PERICODI  IN ({plancodi}) AND 
                PL.CODEMPRESA IN ({empresa})  AND 
                CGB.IND_DEL = 0 AND 
                PL.PLANESTADO ='{estado}'
                ORDER BY TR.PERICODI, CGB.PROYCODI,PL.CODEMPRESA, CGB.BIOHOJAACODI ASC";
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(commandHoja);
            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                while (dr.Read())
                {
                    BioHojaADTO ob = new BioHojaADTO();
                    ob.BiohojaaCodi = !dr.IsDBNull(dr.GetOrdinal("BIOHOJAACODI")) ? dr.GetInt32(dr.GetOrdinal("BIOHOJAACODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.CentralNombre = dr.IsDBNull(dr.GetOrdinal("CENTRALNOMBRE")) ? null : dr.GetString(dr.GetOrdinal("CENTRALNOMBRE"));
                    ob.Distrito = dr.IsDBNull(dr.GetOrdinal("DISTRITO")) ? null : dr.GetString(dr.GetOrdinal("DISTRITO"));
                    ob.Propietario = dr.IsDBNull(dr.GetOrdinal("PROPIETARIO")) ? null : dr.GetString(dr.GetOrdinal("PROPIETARIO"));
                    ob.Otro = dr.IsDBNull(dr.GetOrdinal("OTRO")) ? null : dr.GetString(dr.GetOrdinal("OTRO"));
                    ob.SocioOperador = dr.IsDBNull(dr.GetOrdinal("SOCIOOPERADOR")) ? null : dr.GetString(dr.GetOrdinal("SOCIOOPERADOR"));
                    ob.SocioInversionista = dr.IsDBNull(dr.GetOrdinal("SOCIOINVERSIONISTA")) ? null : dr.GetString(dr.GetOrdinal("SOCIOINVERSIONISTA"));
                    ob.ConTemporal = dr.IsDBNull(dr.GetOrdinal("CONTEMPORAL")) ? null : dr.GetString(dr.GetOrdinal("CONTEMPORAL"));
                    ob.FecAdjudicacionTemp = dr.IsDBNull(dr.GetOrdinal("FECADJUDICACIONTEMP")) ? (DateTime?)null : dr.GetDateTime(dr.GetOrdinal("FECADJUDICACIONTEMP"));
                    ob.TipoConActual = dr.IsDBNull(dr.GetOrdinal("TIPOCONACTUAL")) ? null : dr.GetString(dr.GetOrdinal("TIPOCONACTUAL"));
                    ob.FecAdjudicacionAct = dr.IsDBNull(dr.GetOrdinal("FECADJUDICACIONACT")) ? (DateTime?)null : dr.GetDateTime(dr.GetOrdinal("FECADJUDICACIONACT"));
                    ob.PotInstalada = !dr.IsDBNull(dr.GetOrdinal("POTINSTALADA")) ? dr.GetDecimal(dr.GetOrdinal("POTINSTALADA")) : (decimal?)null;
                    ob.TipoNomComb = dr.IsDBNull(dr.GetOrdinal("TIPONOMCOMB")) ? null : dr.GetString(dr.GetOrdinal("TIPONOMCOMB"));
                    ob.OtroComb = dr.IsDBNull(dr.GetOrdinal("OTROCOMB")) ? null : dr.GetString(dr.GetOrdinal("OTROCOMB"));
                    ob.PotMaxima = !dr.IsDBNull(dr.GetOrdinal("POTMAXIMA")) ? dr.GetDecimal(dr.GetOrdinal("POTMAXIMA")) : (decimal?)null;
                    ob.PoderCalorInf = !dr.IsDBNull(dr.GetOrdinal("PODERCALORINF")) ? dr.GetDecimal(dr.GetOrdinal("PODERCALORINF")) : (decimal?)null;
                    ob.CombPoderCalorInf = dr.IsDBNull(dr.GetOrdinal("COMBPODERCALORINF")) ? null : dr.GetString(dr.GetOrdinal("COMBPODERCALORINF"));
                    ob.PotMinima = !dr.IsDBNull(dr.GetOrdinal("POTMINIMA")) ? dr.GetDecimal(dr.GetOrdinal("POTMINIMA")) : (decimal?)null;
                    ob.PoderCalorSup = !dr.IsDBNull(dr.GetOrdinal("PODERCALORSUP")) ? dr.GetDecimal(dr.GetOrdinal("PODERCALORSUP")) : (decimal?)null;
                    ob.CombPoderCalorSup = dr.IsDBNull(dr.GetOrdinal("COMBPODERCALORSUP")) ? null : dr.GetString(dr.GetOrdinal("COMBPODERCALORSUP"));
                    ob.CostCombustible = !dr.IsDBNull(dr.GetOrdinal("COSTCOMBUSTIBLE")) ? dr.GetDecimal(dr.GetOrdinal("COSTCOMBUSTIBLE")) : (decimal?)null;
                    ob.CombCostoCombustible = dr.IsDBNull(dr.GetOrdinal("COMBCOSTOCOMBUSTIBLE")) ? null : dr.GetString(dr.GetOrdinal("COMBCOSTOCOMBUSTIBLE"));
                    ob.CostTratamiento = !dr.IsDBNull(dr.GetOrdinal("COSTTRATAMIENTO")) ? dr.GetDecimal(dr.GetOrdinal("COSTTRATAMIENTO")) : (decimal?)null;
                    ob.CombCostTratamiento = dr.IsDBNull(dr.GetOrdinal("COMBCOSTTRAMIENTO")) ? null : dr.GetString(dr.GetOrdinal("COMBCOSTTRAMIENTO"));
                    ob.CostTransporte = !dr.IsDBNull(dr.GetOrdinal("COSTTRANSPORTE")) ? dr.GetDecimal(dr.GetOrdinal("COSTTRANSPORTE")) : (decimal?)null;
                    ob.CombCostTransporte = dr.IsDBNull(dr.GetOrdinal("COMBCOSTTRANSPORTE")) ? null : dr.GetString(dr.GetOrdinal("COMBCOSTTRANSPORTE"));
                    ob.CostoVariableNoComb = !dr.IsDBNull(dr.GetOrdinal("COSTOVARIABLENOCOMB")) ? dr.GetDecimal(dr.GetOrdinal("COSTOVARIABLENOCOMB")) : (decimal?)null;
                    ob.CombCostoVariableNoComb = dr.IsDBNull(dr.GetOrdinal("COMBCOSTOVARIABLENOCOMB")) ? null : dr.GetString(dr.GetOrdinal("COMBCOSTOVARIABLENOCOMB"));
                    ob.CostInversion = !dr.IsDBNull(dr.GetOrdinal("COSTINVERSION")) ? dr.GetDecimal(dr.GetOrdinal("COSTINVERSION")) : (decimal?)null;
                    ob.CombCostoInversion = dr.IsDBNull(dr.GetOrdinal("COMBCOSTOINVERSION")) ? null : dr.GetString(dr.GetOrdinal("COMBCOSTOINVERSION"));
                    ob.RendPlanta = !dr.IsDBNull(dr.GetOrdinal("RENDPLANTA")) ? dr.GetDecimal(dr.GetOrdinal("RENDPLANTA")) : (decimal?)null;
                    ob.CombRendPlanta = dr.IsDBNull(dr.GetOrdinal("COMBRENDPLANTA")) ? null : dr.GetString(dr.GetOrdinal("COMBRENDPLANTA"));
                    ob.ConsEspec = !dr.IsDBNull(dr.GetOrdinal("CONSESPEC")) ? dr.GetDecimal(dr.GetOrdinal("CONSESPEC")) : (decimal?)null;
                    ob.CombConsEspec = dr.IsDBNull(dr.GetOrdinal("COMBCONSESPEC")) ? null : dr.GetString(dr.GetOrdinal("COMBCONSESPEC"));
                    ob.TipoMotorTer = dr.IsDBNull(dr.GetOrdinal("TIPOMOTORTER")) ? null : dr.GetString(dr.GetOrdinal("TIPOMOTORTER"));
                    ob.VelNomRotacion = !dr.IsDBNull(dr.GetOrdinal("VELNOMROTACION")) ? dr.GetDecimal(dr.GetOrdinal("VELNOMROTACION")) : (decimal?)null;
                    ob.PotEjeMotorTer = !dr.IsDBNull(dr.GetOrdinal("POTEJEMOTORTER")) ? dr.GetDecimal(dr.GetOrdinal("POTEJEMOTORTER")) : (decimal?)null;
                    ob.NumMotoresTer = !dr.IsDBNull(dr.GetOrdinal("NUMMOTORESTER")) ? dr.GetDecimal(dr.GetOrdinal("NUMMOTORESTER")) : (decimal?)null;
                    ob.PotNomGenerador = !dr.IsDBNull(dr.GetOrdinal("POTNOMGENERADOR")) ? dr.GetDecimal(dr.GetOrdinal("POTNOMGENERADOR")) : (decimal?)null;
                    ob.NumGeneradores = !dr.IsDBNull(dr.GetOrdinal("NUMGENERADORES")) ? dr.GetDecimal(dr.GetOrdinal("NUMGENERADORES")) : (decimal?)null;
                    ob.TipoGenerador = !dr.IsDBNull(dr.GetOrdinal("TIPOGENERADOR")) ? dr.GetDecimal(dr.GetOrdinal("TIPOGENERADOR")) : (decimal?)null;
                    ob.TenGeneracion = !dr.IsDBNull(dr.GetOrdinal("TENGENERACION")) ? dr.GetDecimal(dr.GetOrdinal("TENGENERACION")) : (decimal?)null;
                    ob.Tension = !dr.IsDBNull(dr.GetOrdinal("TENSION")) ? dr.GetDecimal(dr.GetOrdinal("TENSION")) : (decimal?)null;
                    ob.Longitud = !dr.IsDBNull(dr.GetOrdinal("LONGITUD")) ? dr.GetDecimal(dr.GetOrdinal("LONGITUD")) : (decimal?)null;
                    ob.NumTernas = !dr.IsDBNull(dr.GetOrdinal("NUMTERNAS")) ? dr.GetDecimal(dr.GetOrdinal("NUMTERNAS")) : (decimal?)null;
                    ob.NomSubEstacion = dr.IsDBNull(dr.GetOrdinal("NOMSUBESTACION")) ? null : dr.GetString(dr.GetOrdinal("NOMSUBESTACION"));
                    ob.OtroSubEstacion = dr.IsDBNull(dr.GetOrdinal("OTROSUBESTACION")) ? null : dr.GetString(dr.GetOrdinal("OTROSUBESTACION"));
                    ob.Perfil = dr.IsDBNull(dr.GetOrdinal("PERFIL")) ? null : dr.GetString(dr.GetOrdinal("PERFIL"));
                    ob.Prefactibilidad = dr.IsDBNull(dr.GetOrdinal("PREFACTIBILIDAD")) ? null : dr.GetString(dr.GetOrdinal("PREFACTIBILIDAD"));
                    ob.Factibilidad = dr.IsDBNull(dr.GetOrdinal("FACTIBILIDAD")) ? null : dr.GetString(dr.GetOrdinal("FACTIBILIDAD"));
                    ob.EstDefinitivo = dr.IsDBNull(dr.GetOrdinal("ESTDEFINITIVO")) ? null : dr.GetString(dr.GetOrdinal("ESTDEFINITIVO"));
                    ob.Eia = dr.IsDBNull(dr.GetOrdinal("EIA")) ? null : dr.GetString(dr.GetOrdinal("EIA"));
                    ob.FecInicioConst = dr.IsDBNull(dr.GetOrdinal("FECINICIOCONST")) ? null : dr.GetString(dr.GetOrdinal("FECINICIOCONST"));
                    ob.PeriodoConst = dr.IsDBNull(dr.GetOrdinal("PERIODOCONST")) ? null : dr.GetString(dr.GetOrdinal("PERIODOCONST"));
                    ob.FecOperacionComer = dr.IsDBNull(dr.GetOrdinal("FECOPERACIONCOMER")) ? null : dr.GetString(dr.GetOrdinal("FECOPERACIONCOMER"));
                    ob.Comentarios = dr.IsDBNull(dr.GetOrdinal("COMENTARIOS")) ? null : dr.GetString(dr.GetOrdinal("COMENTARIOS"));
                    ob.UsuCreacion = dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? null : dr.GetString(dr.GetOrdinal("USU_CREACION"));
                    ob.FecCreacion = dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? DateTime.MinValue : dr.GetDateTime(dr.GetOrdinal("FEC_CREACION"));
                    ob.IndDel = dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? null : dr.GetString(dr.GetOrdinal("IND_DEL"));
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



    }
}
