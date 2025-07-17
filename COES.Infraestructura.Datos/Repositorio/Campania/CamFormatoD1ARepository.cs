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
    public class CamFormatoD1ARepository : RepositoryBase, ICamFormatoD1ARepository
    {
        public CamFormatoD1ARepository(string strConn) : base(strConn) { }

        CamFormatoD1AHelper Helper = new CamFormatoD1AHelper();

        public List<FormatoD1ADTO> GetFormatoD1ACodi(int proyCodi)
        {
            List<FormatoD1ADTO> formatoD1ADTOs = new List<FormatoD1ADTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetFormatoD1ACodi);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, proyCodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FormatoD1ADTO ob = new FormatoD1ADTO();
                    ob.FormatoD1ACodi = !dr.IsDBNull(dr.GetOrdinal("FORMATOD1ACODI")) ? dr.GetInt32(dr.GetOrdinal("FORMATOD1ACODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.TipoCarga = !dr.IsDBNull(dr.GetOrdinal("TIPOCARGA")) ? dr.GetString(dr.GetOrdinal("TIPOCARGA")) : string.Empty;
                    ob.Nombre = !dr.IsDBNull(dr.GetOrdinal("NOMBRE")) ? dr.GetString(dr.GetOrdinal("NOMBRE")) : string.Empty;
                    ob.EmpresaProp = !dr.IsDBNull(dr.GetOrdinal("EMPRESAPROP")) ? dr.GetString(dr.GetOrdinal("EMPRESAPROP")) : string.Empty;
                    ob.Distrito = !dr.IsDBNull(dr.GetOrdinal("DISTRITO")) ? dr.GetString(dr.GetOrdinal("DISTRITO")) : string.Empty;
                    ob.ActDesarrollo = !dr.IsDBNull(dr.GetOrdinal("ACTDESARROLLO")) ? dr.GetString(dr.GetOrdinal("ACTDESARROLLO")) : string.Empty;
                    ob.SituacionAct = !dr.IsDBNull(dr.GetOrdinal("SITUACIONACT")) ? dr.GetString(dr.GetOrdinal("SITUACIONACT")) : string.Empty;
                    ob.Exploracion = !dr.IsDBNull(dr.GetOrdinal("EXPLORACION")) ? dr.GetString(dr.GetOrdinal("EXPLORACION")) : string.Empty;
                    ob.EstudioPreFactibilidad = !dr.IsDBNull(dr.GetOrdinal("ESTUDIOPREFACTIBILIDAD")) ? dr.GetString(dr.GetOrdinal("ESTUDIOPREFACTIBILIDAD")) : string.Empty;
                    ob.EstudioFactibilidad = !dr.IsDBNull(dr.GetOrdinal("ESTUDIOFACTIBILIDAD")) ? dr.GetString(dr.GetOrdinal("ESTUDIOFACTIBILIDAD")) : string.Empty;
                    ob.EstudioImpAmb = !dr.IsDBNull(dr.GetOrdinal("ESTUDIOIMPAMB")) ? dr.GetString(dr.GetOrdinal("ESTUDIOIMPAMB")) : string.Empty;
                    ob.Financiamiento2 = !dr.IsDBNull(dr.GetOrdinal("FINANCIAMIENTO")) ? dr.GetString(dr.GetOrdinal("FINANCIAMIENTO")) : string.Empty;
                    ob.Ingenieria = !dr.IsDBNull(dr.GetOrdinal("INGENIERIA")) ? dr.GetString(dr.GetOrdinal("INGENIERIA")) : string.Empty;
                    ob.Construccion = !dr.IsDBNull(dr.GetOrdinal("CONSTRUCCION")) ? dr.GetString(dr.GetOrdinal("CONSTRUCCION")) : string.Empty;
                    ob.PuestaMarchar = !dr.IsDBNull(dr.GetOrdinal("PUESTAMARCHAR")) ? dr.GetString(dr.GetOrdinal("PUESTAMARCHAR")) : string.Empty;
                    ob.TipoExtraccionMin = !dr.IsDBNull(dr.GetOrdinal("TIPOEXTRACCIONMIN")) ? dr.GetString(dr.GetOrdinal("TIPOEXTRACCIONMIN")) : string.Empty;
                    ob.MetalesExtraer = !dr.IsDBNull(dr.GetOrdinal("METALESEXTRAER")) ? dr.GetString(dr.GetOrdinal("METALESEXTRAER")) : string.Empty;
                    ob.TipoYacimiento = !dr.IsDBNull(dr.GetOrdinal("TIPOYACIMIENTO")) ? dr.GetString(dr.GetOrdinal("TIPOYACIMIENTO")) : string.Empty;
                    ob.VidaUtil = !dr.IsDBNull(dr.GetOrdinal("VIDAUTIL")) ? (int?)dr.GetInt32(dr.GetOrdinal("VIDAUTIL")) : null;
                    ob.Reservas = !dr.IsDBNull(dr.GetOrdinal("RESERVAS")) ? dr.GetString(dr.GetOrdinal("RESERVAS")) : string.Empty;
                    ob.EscalaProduccion = !dr.IsDBNull(dr.GetOrdinal("ESCALAPRODUCCION")) ? dr.GetString(dr.GetOrdinal("ESCALAPRODUCCION")) : string.Empty;
                    ob.PlantaBeneficio = !dr.IsDBNull(dr.GetOrdinal("PLANTABENEFICIO")) ? dr.GetString(dr.GetOrdinal("PLANTABENEFICIO")) : string.Empty;
                    ob.RecuperacionMet = !dr.IsDBNull(dr.GetOrdinal("RECUPERACIONMET")) ? dr.GetString(dr.GetOrdinal("RECUPERACIONMET")) : string.Empty;
                    ob.LeyesConcentrado = !dr.IsDBNull(dr.GetOrdinal("LEYESCONCENTRADO")) ? dr.GetString(dr.GetOrdinal("LEYESCONCENTRADO")) : string.Empty;
                    ob.CapacidadTrata = !dr.IsDBNull(dr.GetOrdinal("CAPACIDADTRATA")) ? dr.GetString(dr.GetOrdinal("CAPACIDADTRATA")) : string.Empty;
                    ob.ProduccionAnual = !dr.IsDBNull(dr.GetOrdinal("PRODUCCIONANUAL")) ? dr.GetString(dr.GetOrdinal("PRODUCCIONANUAL")) : string.Empty;
                    ob.Item = !dr.IsDBNull(dr.GetOrdinal("ITEM")) ? dr.GetString(dr.GetOrdinal("ITEM")) : string.Empty;
                    ob.ToneladaMetrica = !dr.IsDBNull(dr.GetOrdinal("TONELADAMETRICA")) ? dr.GetString(dr.GetOrdinal("TONELADAMETRICA")) : string.Empty;
                    ob.Energia = !dr.IsDBNull(dr.GetOrdinal("ENERGIA")) ? dr.GetString(dr.GetOrdinal("ENERGIA")) : string.Empty;
                    ob.Consumo = !dr.IsDBNull(dr.GetOrdinal("CONSUMO")) ? dr.GetString(dr.GetOrdinal("CONSUMO")) : string.Empty;
                    ob.SubestacionCodi = !dr.IsDBNull(dr.GetOrdinal("SUBESTACIONCODI")) ? dr.GetString(dr.GetOrdinal("SUBESTACIONCODI")) : string.Empty;
                    ob.SubestacionOtros = !dr.IsDBNull(dr.GetOrdinal("SUBESTACIONOTROS")) ? dr.GetString(dr.GetOrdinal("SUBESTACIONOTROS")) : string.Empty;
                    ob.NivelTension = !dr.IsDBNull(dr.GetOrdinal("NIVELTENSION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("NIVELTENSION")) : null;
                    ob.EmpresaSuminicodi = !dr.IsDBNull(dr.GetOrdinal("EMPRESASUMINICODI")) ? dr.GetString(dr.GetOrdinal("EMPRESASUMINICODI")) : string.Empty;
                    ob.EmpresaSuminiOtro = !dr.IsDBNull(dr.GetOrdinal("EMPRESASUMINIOTRO")) ? dr.GetString(dr.GetOrdinal("EMPRESASUMINIOTRO")) : string.Empty;
                    ob.FactorPotencia = !dr.IsDBNull(dr.GetOrdinal("FACTORPOTENCIA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("FACTORPOTENCIA")) : null;
                    ob.Inductivo = !dr.IsDBNull(dr.GetOrdinal("INDUCTIVO")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("INDUCTIVO")) : null;
                    ob.Capacitivo = !dr.IsDBNull(dr.GetOrdinal("CAPACITIVO")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("CAPACITIVO")) : null;
                    ob.PrimeraEtapa = !dr.IsDBNull(dr.GetOrdinal("PRIMERAETAPA")) ? (int?)dr.GetInt32(dr.GetOrdinal("PRIMERAETAPA")) : null;
                    ob.SegundaEtapa = !dr.IsDBNull(dr.GetOrdinal("SEGUNDAETAPA")) ? (int?)dr.GetInt32(dr.GetOrdinal("SEGUNDAETAPA")) : null;
                    ob.Final = !dr.IsDBNull(dr.GetOrdinal("FINAL")) ? (int?)dr.GetInt32(dr.GetOrdinal("FINAL")) : null;
                    ob.CostoProduccion = !dr.IsDBNull(dr.GetOrdinal("COSTOPRODUCCION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("COSTOPRODUCCION")) : null;
                    ob.Metales = !dr.IsDBNull(dr.GetOrdinal("METALES")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("METALES")) : null;
                    ob.Precio = !dr.IsDBNull(dr.GetOrdinal("PRECIO")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("PRECIO")) : null;
                    ob.Financiamiento1 = !dr.IsDBNull(dr.GetOrdinal("FINANCIAMIENTO1")) ? dr.GetString(dr.GetOrdinal("FINANCIAMIENTO1")) : string.Empty;
                    ob.FacFavEjecuProy = !dr.IsDBNull(dr.GetOrdinal("FACFAVEJECUPROY")) ? dr.GetString(dr.GetOrdinal("FACFAVEJECUPROY")) : string.Empty;
                    ob.FactDesEjecuProy = !dr.IsDBNull(dr.GetOrdinal("FACTDESEJECUPROY")) ? dr.GetString(dr.GetOrdinal("FACTDESEJECUPROY")) : string.Empty;
                    ob.Comentarios = !dr.IsDBNull(dr.GetOrdinal("COMENTARIOS")) ? dr.GetString(dr.GetOrdinal("COMENTARIOS")) : string.Empty;
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : string.Empty;
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : string.Empty;
                    ob.FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;

                    formatoD1ADTOs.Add(ob);
                }
            }

            return formatoD1ADTOs;
        }

        public bool SaveFormatoD1A(FormatoD1ADTO formatoD1ADTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveFormatoD1A);
            dbProvider.AddInParameter(dbCommand, "FORMATOD1ACODI", DbType.Int32, formatoD1ADTO.FormatoD1ACodi);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, formatoD1ADTO.ProyCodi);
            dbProvider.AddInParameter(dbCommand, "TIPOCARGA", DbType.String, formatoD1ADTO.TipoCarga);
            dbProvider.AddInParameter(dbCommand, "NOMBRE", DbType.String, formatoD1ADTO.Nombre);
            dbProvider.AddInParameter(dbCommand, "EMPRESAPROP", DbType.String, formatoD1ADTO.EmpresaProp);
            dbProvider.AddInParameter(dbCommand, "DISTRITO", DbType.String, formatoD1ADTO.Distrito);
            dbProvider.AddInParameter(dbCommand, "ACTDESARROLLO", DbType.String, formatoD1ADTO.ActDesarrollo);
            dbProvider.AddInParameter(dbCommand, "SITUACIONACT", DbType.String, formatoD1ADTO.SituacionAct);
            dbProvider.AddInParameter(dbCommand, "EXPLORACION", DbType.String, formatoD1ADTO.Exploracion);
            dbProvider.AddInParameter(dbCommand, "ESTUDIOPREFACTIBILIDAD", DbType.String, formatoD1ADTO.EstudioPreFactibilidad);
            dbProvider.AddInParameter(dbCommand, "ESTUDIOFACTIBILIDAD", DbType.String, formatoD1ADTO.EstudioFactibilidad);
            dbProvider.AddInParameter(dbCommand, "ESTUDIOIMPAMB", DbType.String, formatoD1ADTO.EstudioImpAmb);
            dbProvider.AddInParameter(dbCommand, "FINANCIAMIENTO1", DbType.String, formatoD1ADTO.Financiamiento2);
            dbProvider.AddInParameter(dbCommand, "INGENIERIA", DbType.String, formatoD1ADTO.Ingenieria);
            dbProvider.AddInParameter(dbCommand, "CONSTRUCCION", DbType.String, formatoD1ADTO.Construccion);
            dbProvider.AddInParameter(dbCommand, "PUESTAMARCHAR", DbType.String, formatoD1ADTO.PuestaMarchar);
            dbProvider.AddInParameter(dbCommand, "TIPOEXTRACCIONMIN", DbType.String, formatoD1ADTO.TipoExtraccionMin);
            dbProvider.AddInParameter(dbCommand, "METALESEXTRAER", DbType.String, formatoD1ADTO.MetalesExtraer);
            dbProvider.AddInParameter(dbCommand, "TIPOYACIMIENTO", DbType.String, formatoD1ADTO.TipoYacimiento);
            dbProvider.AddInParameter(dbCommand, "VIDAUTIL", DbType.Int32, formatoD1ADTO.VidaUtil);
            dbProvider.AddInParameter(dbCommand, "RESERVAS", DbType.String, formatoD1ADTO.Reservas);
            dbProvider.AddInParameter(dbCommand, "ESCALAPRODUCCION", DbType.String, formatoD1ADTO.EscalaProduccion);
            dbProvider.AddInParameter(dbCommand, "PLANTABENEFICIO", DbType.String, formatoD1ADTO.PlantaBeneficio);
            dbProvider.AddInParameter(dbCommand, "RECUPERACIONMET", DbType.String, formatoD1ADTO.RecuperacionMet);
            dbProvider.AddInParameter(dbCommand, "LEYESCONCENTRADO", DbType.String, formatoD1ADTO.LeyesConcentrado);
            dbProvider.AddInParameter(dbCommand, "CAPACIDADTRATA", DbType.String, formatoD1ADTO.CapacidadTrata);
            dbProvider.AddInParameter(dbCommand, "PRODUCCIONANUAL", DbType.String, formatoD1ADTO.ProduccionAnual);
            dbProvider.AddInParameter(dbCommand, "ITEM", DbType.String, formatoD1ADTO.Item);
            dbProvider.AddInParameter(dbCommand, "TONELADAMETRICA", DbType.String, formatoD1ADTO.ToneladaMetrica);
            dbProvider.AddInParameter(dbCommand, "ENERGIA", DbType.String, formatoD1ADTO.Energia);
            dbProvider.AddInParameter(dbCommand, "CONSUMO", DbType.String, formatoD1ADTO.Consumo);
            dbProvider.AddInParameter(dbCommand, "SUBESTACIONCODI", DbType.String, formatoD1ADTO.SubestacionCodi);
            dbProvider.AddInParameter(dbCommand, "SUBESTACIONOTROS", DbType.String, formatoD1ADTO.SubestacionOtros);
            dbProvider.AddInParameter(dbCommand, "NIVELTENSION", DbType.Decimal, formatoD1ADTO.NivelTension);
            dbProvider.AddInParameter(dbCommand, "EMPRESASUMINICODI", DbType.String, formatoD1ADTO.EmpresaSuminicodi);
            dbProvider.AddInParameter(dbCommand, "EMPRESASUMINIOTRO", DbType.String, formatoD1ADTO.EmpresaSuminiOtro);
            dbProvider.AddInParameter(dbCommand, "FACTORPOTENCIA", DbType.Decimal, formatoD1ADTO.FactorPotencia);
            dbProvider.AddInParameter(dbCommand, "INDUCTIVO", DbType.Decimal, formatoD1ADTO.Inductivo);
            dbProvider.AddInParameter(dbCommand, "CAPACITIVO", DbType.Decimal, formatoD1ADTO.Capacitivo);
            dbProvider.AddInParameter(dbCommand, "PRIMERAETAPA", DbType.Int32, formatoD1ADTO.PrimeraEtapa);
            dbProvider.AddInParameter(dbCommand, "SEGUNDAETAPA", DbType.Int32, formatoD1ADTO.SegundaEtapa);
            dbProvider.AddInParameter(dbCommand, "FINAL", DbType.Int32, formatoD1ADTO.Final);
            dbProvider.AddInParameter(dbCommand, "COSTOPRODUCCION", DbType.Decimal, formatoD1ADTO.CostoProduccion);
            dbProvider.AddInParameter(dbCommand, "METALES", DbType.Decimal, formatoD1ADTO.Metales);
            dbProvider.AddInParameter(dbCommand, "PRECIO", DbType.Decimal, formatoD1ADTO.Precio);
            dbProvider.AddInParameter(dbCommand, "FINANCIAMIENTO2", DbType.String, formatoD1ADTO.Financiamiento1);
            dbProvider.AddInParameter(dbCommand, "FACFAVEJECUPROY", DbType.String, formatoD1ADTO.FacFavEjecuProy);
            dbProvider.AddInParameter(dbCommand, "FACTDESEJECUPROY", DbType.String, formatoD1ADTO.FactDesEjecuProy);
            dbProvider.AddInParameter(dbCommand, "COMENTARIOS", DbType.String, formatoD1ADTO.Comentarios);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, formatoD1ADTO.UsuCreacion);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, formatoD1ADTO.FecCreacion);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, formatoD1ADTO.IndDel);

            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        public bool DeleteFormatoD1AById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteFormatoD1AById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);

            return true;
        }

        public int GetLastFormatoD1AId()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastFormatoD1AId);
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

        public FormatoD1ADTO GetFormatoD1AById(int id)
        {
            FormatoD1ADTO ob = new FormatoD1ADTO();
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(Helper.SqlGetFormatoD1AById);
            dbProvider.AddInParameter(commandHoja, "PROYCODI", DbType.Int32, id);
            dbProvider.AddInParameter(commandHoja, "IND_DEL", DbType.String, Constantes.IndDel);

            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                if (dr.Read())
                {
                    ob.FormatoD1ACodi = !dr.IsDBNull(dr.GetOrdinal("FORMATOD1ACODI")) ? dr.GetInt32(dr.GetOrdinal("FORMATOD1ACODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.TipoCarga = !dr.IsDBNull(dr.GetOrdinal("TIPOCARGA")) ? dr.GetString(dr.GetOrdinal("TIPOCARGA")) : string.Empty;
                    ob.Nombre = !dr.IsDBNull(dr.GetOrdinal("NOMBRE")) ? dr.GetString(dr.GetOrdinal("NOMBRE")) : string.Empty;
                    ob.EmpresaProp = !dr.IsDBNull(dr.GetOrdinal("EMPRESAPROP")) ? dr.GetString(dr.GetOrdinal("EMPRESAPROP")) : string.Empty;
                    ob.Distrito = !dr.IsDBNull(dr.GetOrdinal("DISTRITO")) ? dr.GetString(dr.GetOrdinal("DISTRITO")) : string.Empty;
                    ob.ActDesarrollo = !dr.IsDBNull(dr.GetOrdinal("ACTDESARROLLO")) ? dr.GetString(dr.GetOrdinal("ACTDESARROLLO")) : string.Empty;
                    ob.SituacionAct = !dr.IsDBNull(dr.GetOrdinal("SITUACIONACT")) ? dr.GetString(dr.GetOrdinal("SITUACIONACT")) : string.Empty;
                    ob.Exploracion = !dr.IsDBNull(dr.GetOrdinal("EXPLORACION")) ? dr.GetString(dr.GetOrdinal("EXPLORACION")) : string.Empty;
                    ob.EstudioPreFactibilidad = !dr.IsDBNull(dr.GetOrdinal("ESTUDIOPREFACTIBILIDAD")) ? dr.GetString(dr.GetOrdinal("ESTUDIOPREFACTIBILIDAD")) : string.Empty;
                    ob.EstudioFactibilidad = !dr.IsDBNull(dr.GetOrdinal("ESTUDIOFACTIBILIDAD")) ? dr.GetString(dr.GetOrdinal("ESTUDIOFACTIBILIDAD")) : string.Empty;
                    ob.EstudioImpAmb = !dr.IsDBNull(dr.GetOrdinal("ESTUDIOIMPAMB")) ? dr.GetString(dr.GetOrdinal("ESTUDIOIMPAMB")) : string.Empty;
                    ob.Financiamiento2 = !dr.IsDBNull(dr.GetOrdinal("FINANCIAMIENTO1")) ? dr.GetString(dr.GetOrdinal("FINANCIAMIENTO1")) : string.Empty;
                    ob.Ingenieria = !dr.IsDBNull(dr.GetOrdinal("INGENIERIA")) ? dr.GetString(dr.GetOrdinal("INGENIERIA")) : string.Empty;
                    ob.Construccion = !dr.IsDBNull(dr.GetOrdinal("CONSTRUCCION")) ? dr.GetString(dr.GetOrdinal("CONSTRUCCION")) : string.Empty;
                    ob.PuestaMarchar = !dr.IsDBNull(dr.GetOrdinal("PUESTAMARCHAR")) ? dr.GetString(dr.GetOrdinal("PUESTAMARCHAR")) : string.Empty;
                    ob.TipoExtraccionMin = !dr.IsDBNull(dr.GetOrdinal("TIPOEXTRACCIONMIN")) ? dr.GetString(dr.GetOrdinal("TIPOEXTRACCIONMIN")) : string.Empty;
                    ob.MetalesExtraer = !dr.IsDBNull(dr.GetOrdinal("METALESEXTRAER")) ? dr.GetString(dr.GetOrdinal("METALESEXTRAER")) : string.Empty;
                    ob.TipoYacimiento = !dr.IsDBNull(dr.GetOrdinal("TIPOYACIMIENTO")) ? dr.GetString(dr.GetOrdinal("TIPOYACIMIENTO")) : string.Empty;
                    ob.VidaUtil = !dr.IsDBNull(dr.GetOrdinal("VIDAUTIL")) ? (int?)dr.GetInt32(dr.GetOrdinal("VIDAUTIL")) : null;
                    ob.Reservas = !dr.IsDBNull(dr.GetOrdinal("RESERVAS")) ? dr.GetString(dr.GetOrdinal("RESERVAS")) : string.Empty;
                    ob.EscalaProduccion = !dr.IsDBNull(dr.GetOrdinal("ESCALAPRODUCCION")) ? dr.GetString(dr.GetOrdinal("ESCALAPRODUCCION")) : string.Empty;
                    ob.PlantaBeneficio = !dr.IsDBNull(dr.GetOrdinal("PLANTABENEFICIO")) ? dr.GetString(dr.GetOrdinal("PLANTABENEFICIO")) : string.Empty;
                    ob.RecuperacionMet = !dr.IsDBNull(dr.GetOrdinal("RECUPERACIONMET")) ? dr.GetString(dr.GetOrdinal("RECUPERACIONMET")) : string.Empty;
                    ob.LeyesConcentrado = !dr.IsDBNull(dr.GetOrdinal("LEYESCONCENTRADO")) ? dr.GetString(dr.GetOrdinal("LEYESCONCENTRADO")) : string.Empty;
                    ob.CapacidadTrata = !dr.IsDBNull(dr.GetOrdinal("CAPACIDADTRATA")) ? dr.GetString(dr.GetOrdinal("CAPACIDADTRATA")) : string.Empty;
                    ob.ProduccionAnual = !dr.IsDBNull(dr.GetOrdinal("PRODUCCIONANUAL")) ? dr.GetString(dr.GetOrdinal("PRODUCCIONANUAL")) : string.Empty;
                    ob.Item = !dr.IsDBNull(dr.GetOrdinal("ITEM")) ? dr.GetString(dr.GetOrdinal("ITEM")) : string.Empty;
                    ob.ToneladaMetrica = !dr.IsDBNull(dr.GetOrdinal("TONELADAMETRICA")) ? dr.GetString(dr.GetOrdinal("TONELADAMETRICA")) : string.Empty;
                    ob.Energia = !dr.IsDBNull(dr.GetOrdinal("ENERGIA")) ? dr.GetString(dr.GetOrdinal("ENERGIA")) : string.Empty;
                    ob.Consumo = !dr.IsDBNull(dr.GetOrdinal("CONSUMO")) ? dr.GetString(dr.GetOrdinal("CONSUMO")) : string.Empty;
                    ob.SubestacionCodi = !dr.IsDBNull(dr.GetOrdinal("SUBESTACIONCODI")) ? dr.GetString(dr.GetOrdinal("SUBESTACIONCODI")) : string.Empty;
                    ob.SubestacionOtros = !dr.IsDBNull(dr.GetOrdinal("SUBESTACIONOTROS")) ? dr.GetString(dr.GetOrdinal("SUBESTACIONOTROS")) : string.Empty;
                    ob.NivelTension = !dr.IsDBNull(dr.GetOrdinal("NIVELTENSION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("NIVELTENSION")) : null;
                    ob.EmpresaSuminicodi = !dr.IsDBNull(dr.GetOrdinal("EMPRESASUMINICODI")) ? dr.GetString(dr.GetOrdinal("EMPRESASUMINICODI")) : string.Empty;
                    ob.EmpresaSuminiOtro = !dr.IsDBNull(dr.GetOrdinal("EMPRESASUMINIOTRO")) ? dr.GetString(dr.GetOrdinal("EMPRESASUMINIOTRO")) : string.Empty;
                    ob.FactorPotencia = !dr.IsDBNull(dr.GetOrdinal("FACTORPOTENCIA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("FACTORPOTENCIA")) : null;
                    ob.Inductivo = !dr.IsDBNull(dr.GetOrdinal("INDUCTIVO")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("INDUCTIVO")) : null;
                    ob.Capacitivo = !dr.IsDBNull(dr.GetOrdinal("CAPACITIVO")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("CAPACITIVO")) : null;
                    ob.PrimeraEtapa = !dr.IsDBNull(dr.GetOrdinal("PRIMERAETAPA")) ? (int?)dr.GetInt32(dr.GetOrdinal("PRIMERAETAPA")) : null;
                    ob.SegundaEtapa = !dr.IsDBNull(dr.GetOrdinal("SEGUNDAETAPA")) ? (int?)dr.GetInt32(dr.GetOrdinal("SEGUNDAETAPA")) : null;
                    ob.Final = !dr.IsDBNull(dr.GetOrdinal("FINAL")) ? (int?)dr.GetInt32(dr.GetOrdinal("FINAL")) : null;
                    ob.CostoProduccion = !dr.IsDBNull(dr.GetOrdinal("COSTOPRODUCCION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("COSTOPRODUCCION")) : null;
                    ob.Metales = !dr.IsDBNull(dr.GetOrdinal("METALES")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("METALES")) : null;
                    ob.Precio = !dr.IsDBNull(dr.GetOrdinal("PRECIO")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("PRECIO")) : null;
                    ob.Financiamiento1 = !dr.IsDBNull(dr.GetOrdinal("FINANCIAMIENTO2")) ? dr.GetString(dr.GetOrdinal("FINANCIAMIENTO2")) : string.Empty;
                    ob.FacFavEjecuProy = !dr.IsDBNull(dr.GetOrdinal("FACFAVEJECUPROY")) ? dr.GetString(dr.GetOrdinal("FACFAVEJECUPROY")) : string.Empty;
                    ob.FactDesEjecuProy = !dr.IsDBNull(dr.GetOrdinal("FACTDESEJECUPROY")) ? dr.GetString(dr.GetOrdinal("FACTDESEJECUPROY")) : string.Empty;
                    ob.Comentarios = !dr.IsDBNull(dr.GetOrdinal("COMENTARIOS")) ? dr.GetString(dr.GetOrdinal("COMENTARIOS")) : string.Empty;
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : string.Empty;
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : string.Empty;
                    ob.FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;

                }
            }

            return ob;
        }

        public List<FormatoD1ADTO> GetFormatoD1AByFilter(string plancodi, string empresa, string estado)
        {
            List<FormatoD1ADTO> oblist = new List<FormatoD1ADTO>();
            string query = $@"
                  SELECT CGB.*, TR.EMPRESANOM, TR.PROYNOMBRE, TR.PROYDESCRIPCION, TP.TIPONOMBRE, TF.TIPOFINOMBRE,TR.PROYCONFIDENCIAL  FROM CAM_FORMATOD1A CGB
                 INNER JOIN CAM_TRNSMPROYECTO TR ON TR.PROYCODI = CGB.PROYCODI
                 INNER JOIN CAM_PLANTRANSMISION PL ON PL.PLANCODI = TR.PLANCODI
                 INNER JOIN CAM_TIPOPROYECTO TP ON TP.TIPOCODI = TR.TIPOCODI
                 LEFT JOIN CAM_TIPOFICHAPROYECTO TF ON TF.TIPOFICODI = TR.TIPOFICODI
                WHERE TR.PERICODI  IN ({plancodi}) AND 
                PL.CODEMPRESA IN ({empresa})  AND 
                CGB.IND_DEL = 0 AND 
                PL.PLANESTADO ='{estado}'
                ORDER BY TR.PERICODI, CGB.PROYCODI,PL.CODEMPRESA, CGB.FORMATOD1ACODI ASC";


            DbCommand commandHoja = dbProvider.GetSqlStringCommand(query);
  

            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                while (dr.Read())
                {
                    FormatoD1ADTO ob = new FormatoD1ADTO();
                    ob.FormatoD1ACodi = !dr.IsDBNull(dr.GetOrdinal("FORMATOD1ACODI")) ? dr.GetInt32(dr.GetOrdinal("FORMATOD1ACODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.TipoCarga = !dr.IsDBNull(dr.GetOrdinal("TIPOCARGA")) ? dr.GetString(dr.GetOrdinal("TIPOCARGA")) : string.Empty;
                    ob.Nombre = !dr.IsDBNull(dr.GetOrdinal("NOMBRE")) ? dr.GetString(dr.GetOrdinal("NOMBRE")) : string.Empty;
                    ob.EmpresaProp = !dr.IsDBNull(dr.GetOrdinal("EMPRESAPROP")) ? dr.GetString(dr.GetOrdinal("EMPRESAPROP")) : string.Empty;
                    ob.Distrito = !dr.IsDBNull(dr.GetOrdinal("DISTRITO")) ? dr.GetString(dr.GetOrdinal("DISTRITO")) : string.Empty;
                    ob.ActDesarrollo = !dr.IsDBNull(dr.GetOrdinal("ACTDESARROLLO")) ? dr.GetString(dr.GetOrdinal("ACTDESARROLLO")) : string.Empty;
                    ob.SituacionAct = !dr.IsDBNull(dr.GetOrdinal("SITUACIONACT")) ? dr.GetString(dr.GetOrdinal("SITUACIONACT")) : string.Empty;
                    ob.Exploracion = !dr.IsDBNull(dr.GetOrdinal("EXPLORACION")) ? dr.GetString(dr.GetOrdinal("EXPLORACION")) : string.Empty;
                    ob.EstudioPreFactibilidad = !dr.IsDBNull(dr.GetOrdinal("ESTUDIOPREFACTIBILIDAD")) ? dr.GetString(dr.GetOrdinal("ESTUDIOPREFACTIBILIDAD")) : string.Empty;
                    ob.EstudioFactibilidad = !dr.IsDBNull(dr.GetOrdinal("ESTUDIOFACTIBILIDAD")) ? dr.GetString(dr.GetOrdinal("ESTUDIOFACTIBILIDAD")) : string.Empty;
                    ob.EstudioImpAmb = !dr.IsDBNull(dr.GetOrdinal("ESTUDIOIMPAMB")) ? dr.GetString(dr.GetOrdinal("ESTUDIOIMPAMB")) : string.Empty;
                    ob.Financiamiento2 = !dr.IsDBNull(dr.GetOrdinal("FINANCIAMIENTO1")) ? dr.GetString(dr.GetOrdinal("FINANCIAMIENTO1")) : string.Empty;
                    ob.Ingenieria = !dr.IsDBNull(dr.GetOrdinal("INGENIERIA")) ? dr.GetString(dr.GetOrdinal("INGENIERIA")) : string.Empty;
                    ob.Construccion = !dr.IsDBNull(dr.GetOrdinal("CONSTRUCCION")) ? dr.GetString(dr.GetOrdinal("CONSTRUCCION")) : string.Empty;
                    ob.PuestaMarchar = !dr.IsDBNull(dr.GetOrdinal("PUESTAMARCHAR")) ? dr.GetString(dr.GetOrdinal("PUESTAMARCHAR")) : string.Empty;
                    ob.TipoExtraccionMin = !dr.IsDBNull(dr.GetOrdinal("TIPOEXTRACCIONMIN")) ? dr.GetString(dr.GetOrdinal("TIPOEXTRACCIONMIN")) : string.Empty;
                    ob.MetalesExtraer = !dr.IsDBNull(dr.GetOrdinal("METALESEXTRAER")) ? dr.GetString(dr.GetOrdinal("METALESEXTRAER")) : string.Empty;
                    ob.TipoYacimiento = !dr.IsDBNull(dr.GetOrdinal("TIPOYACIMIENTO")) ? dr.GetString(dr.GetOrdinal("TIPOYACIMIENTO")) : string.Empty;
                    ob.VidaUtil = !dr.IsDBNull(dr.GetOrdinal("VIDAUTIL")) ? dr.GetInt32(dr.GetOrdinal("VIDAUTIL")) : 0;
                    ob.Reservas = !dr.IsDBNull(dr.GetOrdinal("RESERVAS")) ? dr.GetString(dr.GetOrdinal("RESERVAS")) : string.Empty;
                    ob.EscalaProduccion = !dr.IsDBNull(dr.GetOrdinal("ESCALAPRODUCCION")) ? dr.GetString(dr.GetOrdinal("ESCALAPRODUCCION")) : string.Empty;
                    ob.PlantaBeneficio = !dr.IsDBNull(dr.GetOrdinal("PLANTABENEFICIO")) ? dr.GetString(dr.GetOrdinal("PLANTABENEFICIO")) : string.Empty;
                    ob.RecuperacionMet = !dr.IsDBNull(dr.GetOrdinal("RECUPERACIONMET")) ? dr.GetString(dr.GetOrdinal("RECUPERACIONMET")) : string.Empty;
                    ob.LeyesConcentrado = !dr.IsDBNull(dr.GetOrdinal("LEYESCONCENTRADO")) ? dr.GetString(dr.GetOrdinal("LEYESCONCENTRADO")) : string.Empty;
                    ob.CapacidadTrata = !dr.IsDBNull(dr.GetOrdinal("CAPACIDADTRATA")) ? dr.GetString(dr.GetOrdinal("CAPACIDADTRATA")) : string.Empty;
                    ob.ProduccionAnual = !dr.IsDBNull(dr.GetOrdinal("PRODUCCIONANUAL")) ? dr.GetString(dr.GetOrdinal("PRODUCCIONANUAL")) : string.Empty;
                    ob.Item = !dr.IsDBNull(dr.GetOrdinal("ITEM")) ? dr.GetString(dr.GetOrdinal("ITEM")) : string.Empty;
                    ob.ToneladaMetrica = !dr.IsDBNull(dr.GetOrdinal("TONELADAMETRICA")) ? dr.GetString(dr.GetOrdinal("TONELADAMETRICA")) : string.Empty;
                    ob.Energia = !dr.IsDBNull(dr.GetOrdinal("ENERGIA")) ? dr.GetString(dr.GetOrdinal("ENERGIA")) : string.Empty;
                    ob.Consumo = !dr.IsDBNull(dr.GetOrdinal("CONSUMO")) ? dr.GetString(dr.GetOrdinal("CONSUMO")) : string.Empty;
                    ob.SubestacionCodi = !dr.IsDBNull(dr.GetOrdinal("SUBESTACIONCODI")) ? dr.GetString(dr.GetOrdinal("SUBESTACIONCODI")) : string.Empty;
                    ob.SubestacionOtros = !dr.IsDBNull(dr.GetOrdinal("SUBESTACIONOTROS")) ? dr.GetString(dr.GetOrdinal("SUBESTACIONOTROS")) : string.Empty;
                    ob.NivelTension = !dr.IsDBNull(dr.GetOrdinal("NIVELTENSION")) ? dr.GetDecimal(dr.GetOrdinal("NIVELTENSION")) : 0;
                    ob.EmpresaSuminicodi = !dr.IsDBNull(dr.GetOrdinal("EMPRESASUMINICODI")) ? dr.GetString(dr.GetOrdinal("EMPRESASUMINICODI")) : string.Empty;
                    ob.EmpresaSuminiOtro = !dr.IsDBNull(dr.GetOrdinal("EMPRESASUMINIOTRO")) ? dr.GetString(dr.GetOrdinal("EMPRESASUMINIOTRO")) : string.Empty;
                    ob.FactorPotencia = !dr.IsDBNull(dr.GetOrdinal("FACTORPOTENCIA")) ? dr.GetDecimal(dr.GetOrdinal("FACTORPOTENCIA")) : 0;
                    ob.Inductivo = !dr.IsDBNull(dr.GetOrdinal("INDUCTIVO")) ? dr.GetDecimal(dr.GetOrdinal("INDUCTIVO")) : 0;
                    ob.Capacitivo = !dr.IsDBNull(dr.GetOrdinal("CAPACITIVO")) ? dr.GetDecimal(dr.GetOrdinal("CAPACITIVO")) : 0;
                    ob.PrimeraEtapa = !dr.IsDBNull(dr.GetOrdinal("PRIMERAETAPA")) ? dr.GetInt32(dr.GetOrdinal("PRIMERAETAPA")) : 0;
                    ob.SegundaEtapa = !dr.IsDBNull(dr.GetOrdinal("SEGUNDAETAPA")) ? dr.GetInt32(dr.GetOrdinal("SEGUNDAETAPA")) : 0;
                    ob.Final = !dr.IsDBNull(dr.GetOrdinal("FINAL")) ? dr.GetInt32(dr.GetOrdinal("FINAL")) : 0;
                    ob.CostoProduccion = !dr.IsDBNull(dr.GetOrdinal("COSTOPRODUCCION")) ? dr.GetDecimal(dr.GetOrdinal("COSTOPRODUCCION")) : 0;
                    ob.Metales = !dr.IsDBNull(dr.GetOrdinal("METALES")) ? dr.GetDecimal(dr.GetOrdinal("METALES")) : 0;
                    ob.Precio = !dr.IsDBNull(dr.GetOrdinal("PRECIO")) ? dr.GetDecimal(dr.GetOrdinal("PRECIO")) : 0;
                    ob.Financiamiento1 = !dr.IsDBNull(dr.GetOrdinal("FINANCIAMIENTO2")) ? dr.GetString(dr.GetOrdinal("FINANCIAMIENTO2")) : string.Empty;
                    ob.FacFavEjecuProy = !dr.IsDBNull(dr.GetOrdinal("FACFAVEJECUPROY")) ? dr.GetString(dr.GetOrdinal("FACFAVEJECUPROY")) : string.Empty;
                    ob.FactDesEjecuProy = !dr.IsDBNull(dr.GetOrdinal("FACTDESEJECUPROY")) ? dr.GetString(dr.GetOrdinal("FACTDESEJECUPROY")) : string.Empty;
                    ob.Comentarios = !dr.IsDBNull(dr.GetOrdinal("COMENTARIOS")) ? dr.GetString(dr.GetOrdinal("COMENTARIOS")) : string.Empty;
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : string.Empty;
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : string.Empty;
                    ob.FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                    ob.Empresa = !dr.IsDBNull(dr.GetOrdinal("EMPRESANOM")) ? dr.GetString(dr.GetOrdinal("EMPRESANOM")) : string.Empty;
                    ob.DetalleProyecto = !dr.IsDBNull(dr.GetOrdinal("PROYDESCRIPCION")) ? dr.GetString(dr.GetOrdinal("PROYDESCRIPCION")) : string.Empty;
                    ob.TipoProyecto = !dr.IsDBNull(dr.GetOrdinal("TIPONOMBRE")) ? dr.GetString(dr.GetOrdinal("TIPONOMBRE")) : string.Empty;
                    ob.SubTipoProyecto = !dr.IsDBNull(dr.GetOrdinal("TIPOFINOMBRE")) ? dr.GetString(dr.GetOrdinal("TIPOFINOMBRE")) : string.Empty;
                    ob.Condifencial = dr.IsDBNull(dr.GetOrdinal("PROYCONFIDENCIAL")) ? null : dr.GetString(dr.GetOrdinal("PROYCONFIDENCIAL"));
                    ob.NombreProyecto = !dr.IsDBNull(dr.GetOrdinal("PROYNOMBRE")) ? dr.GetString(dr.GetOrdinal("PROYNOMBRE")) : string.Empty;
                    oblist.Add(ob);
                }
            }

            return oblist;
        }

    }
}
