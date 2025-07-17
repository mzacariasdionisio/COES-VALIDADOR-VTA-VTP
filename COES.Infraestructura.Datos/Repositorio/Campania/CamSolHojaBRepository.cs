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
    public class CamSolHojaBRepository : RepositoryBase, ICamSolHojaBRepository
    {

        public CamSolHojaBRepository(string strConn) : base(strConn) { }

        CamSolHojaBHelper Helper = new CamSolHojaBHelper();

        public List<SolHojaBDTO> GetSolHojaBProyCodi(int proyCodi)
        {
            List<SolHojaBDTO> solHojaBDTOs = new List<SolHojaBDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetSolHojaBProyCodi);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, proyCodi); // Asumiendo que PROYCODI es de tipo int

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SolHojaBDTO ob = new SolHojaBDTO();
                    ob.SolhojabCodi = !dr.IsDBNull(dr.GetOrdinal("SOLHOJABCODI")) ? dr.GetInt32(dr.GetOrdinal("SOLHOJABCODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Estudiofactibilidad = !dr.IsDBNull(dr.GetOrdinal("ESTUDIOFACTIBILIDAD")) ? dr.GetDecimal(dr.GetOrdinal("ESTUDIOFACTIBILIDAD")) : 0;
                    ob.Investigacionescampo = !dr.IsDBNull(dr.GetOrdinal("INVESTIGACIONESCAMPO")) ? dr.GetDecimal(dr.GetOrdinal("INVESTIGACIONESCAMPO")) : 0;
                    ob.Gestionesfinancieras = !dr.IsDBNull(dr.GetOrdinal("GESTIONESFINANCIERAS")) ? dr.GetDecimal(dr.GetOrdinal("GESTIONESFINANCIERAS")) : 0;
                    ob.Disenospermisos = !dr.IsDBNull(dr.GetOrdinal("DISENOSPERMISOS")) ? dr.GetDecimal(dr.GetOrdinal("DISENOSPERMISOS")) : 0;
                    ob.Obrasciviles = !dr.IsDBNull(dr.GetOrdinal("OBRASCIVILES")) ? dr.GetDecimal(dr.GetOrdinal("OBRASCIVILES")) : 0;
                    ob.Equipamiento = !dr.IsDBNull(dr.GetOrdinal("EQUIPAMIENTO")) ? dr.GetDecimal(dr.GetOrdinal("EQUIPAMIENTO")) : 0;
                    ob.Lineatransmision = !dr.IsDBNull(dr.GetOrdinal("LINEATRANSMISION")) ? dr.GetDecimal(dr.GetOrdinal("LINEATRANSMISION")) : 0;
                    ob.Administracion = !dr.IsDBNull(dr.GetOrdinal("ADMINISTRACION")) ? dr.GetDecimal(dr.GetOrdinal("ADMINISTRACION")) : 0;
                    ob.Aduanas = !dr.IsDBNull(dr.GetOrdinal("ADUANAS")) ? dr.GetDecimal(dr.GetOrdinal("ADUANAS")) : 0;
                    ob.Supervision = !dr.IsDBNull(dr.GetOrdinal("SUPERVISION")) ? dr.GetDecimal(dr.GetOrdinal("SUPERVISION")) : 0;
                    ob.Gastosgestion = !dr.IsDBNull(dr.GetOrdinal("GASTOSGESTION")) ? dr.GetDecimal(dr.GetOrdinal("GASTOSGESTION")) : 0;
                    ob.Imprevistos = !dr.IsDBNull(dr.GetOrdinal("IMPREVISTOS")) ? dr.GetDecimal(dr.GetOrdinal("IMPREVISTOS")) : 0;
                    ob.Igv = !dr.IsDBNull(dr.GetOrdinal("IGV")) ? dr.GetDecimal(dr.GetOrdinal("IGV")) : 0;
                    ob.Otrosgastos = !dr.IsDBNull(dr.GetOrdinal("OTROSGASTOS")) ? dr.GetDecimal(dr.GetOrdinal("OTROSGASTOS")) : 0;
                    ob.Inversiontotalsinigv = !dr.IsDBNull(dr.GetOrdinal("INVERSIONTOTALSINIGV")) ? dr.GetDecimal(dr.GetOrdinal("INVERSIONTOTALSINIGV")) : 0;
                    ob.Inversiontotalconigv = !dr.IsDBNull(dr.GetOrdinal("INVERSIONTOTALCONIGV")) ? dr.GetDecimal(dr.GetOrdinal("INVERSIONTOTALCONIGV")) : 0;
                    ob.Financiamientotipo = !dr.IsDBNull(dr.GetOrdinal("FINANCIAMIENTOTIPO")) ? dr.GetString(dr.GetOrdinal("FINANCIAMIENTOTIPO")) : string.Empty;
                    ob.Financiamientoestado = !dr.IsDBNull(dr.GetOrdinal("FINANCIAMIENTOESTADO")) ? dr.GetString(dr.GetOrdinal("FINANCIAMIENTOESTADO")) : string.Empty;
                    ob.Porcentajefinanciado = !dr.IsDBNull(dr.GetOrdinal("PORCENTAJEFINANCIADO")) ? dr.GetDecimal(dr.GetOrdinal("PORCENTAJEFINANCIADO")) : 0;
                    ob.Concesiondefinitiva = !dr.IsDBNull(dr.GetOrdinal("CONCESIONDEFINITIVA")) ? dr.GetString(dr.GetOrdinal("CONCESIONDEFINITIVA")) : string.Empty;
                    ob.Ventaenergia = !dr.IsDBNull(dr.GetOrdinal("VENTAENERGIA")) ? dr.GetString(dr.GetOrdinal("VENTAENERGIA")) : string.Empty;
                    ob.Ejecucionobra = !dr.IsDBNull(dr.GetOrdinal("EJECUCIONOBRA")) ? dr.GetString(dr.GetOrdinal("EJECUCIONOBRA")) : string.Empty;
                    ob.Contratosfinancieros = !dr.IsDBNull(dr.GetOrdinal("CONTRATOSFINANCIEROS")) ? dr.GetString(dr.GetOrdinal("CONTRATOSFINANCIEROS")) : string.Empty;
                    ob.Observaciones = !dr.IsDBNull(dr.GetOrdinal("OBSERVACIONES")) ? dr.GetString(dr.GetOrdinal("OBSERVACIONES")) : string.Empty;
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : string.Empty;
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                    solHojaBDTOs.Add(ob);
                }
            }

            return solHojaBDTOs;
        }



        public bool SaveSolHojaB(SolHojaBDTO solHojaBDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveSolHojaB);
            dbProvider.AddInParameter(dbCommand, "SOLHOJABCODI", DbType.Int32, ObtenerValorOrDefault(solHojaBDTO.SolhojabCodi, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, ObtenerValorOrDefault(solHojaBDTO.ProyCodi, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "ESTUDIOFACTIBILIDAD", DbType.Decimal, solHojaBDTO.Estudiofactibilidad ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "INVESTIGACIONESCAMPO", DbType.Decimal, solHojaBDTO.Investigacionescampo ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "GESTIONESFINANCIERAS", DbType.Decimal, solHojaBDTO.Gestionesfinancieras ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "DISENOSPERMISOS", DbType.Decimal, solHojaBDTO.Disenospermisos ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "OBRASCIVILES", DbType.Decimal, solHojaBDTO.Obrasciviles ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "EQUIPAMIENTO", DbType.Decimal, solHojaBDTO.Equipamiento ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "LINEATRANSMISION", DbType.Decimal, solHojaBDTO.Lineatransmision ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "ADMINISTRACION", DbType.Decimal, solHojaBDTO.Administracion ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "ADUANAS", DbType.Decimal, solHojaBDTO.Aduanas ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "SUPERVISION", DbType.Decimal, solHojaBDTO.Supervision ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "GASTOSGESTION", DbType.Decimal, solHojaBDTO.Gastosgestion ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "IMPREVISTOS", DbType.Decimal, solHojaBDTO.Imprevistos ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "IGV", DbType.Decimal, solHojaBDTO.Igv ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "OTROSGASTOS", DbType.Decimal, solHojaBDTO.Otrosgastos ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "INVERSIONTOTALSINIGV", DbType.Decimal, solHojaBDTO.Inversiontotalsinigv ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "INVERSIONTOTALCONIGV", DbType.Decimal, solHojaBDTO.Inversiontotalconigv ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "FINANCIAMIENTOTIPO", DbType.String, solHojaBDTO.Financiamientotipo);
            dbProvider.AddInParameter(dbCommand, "FINANCIAMIENTOESTADO", DbType.String, solHojaBDTO.Financiamientoestado);
            dbProvider.AddInParameter(dbCommand, "PORCENTAJEFINANCIADO", DbType.Decimal, solHojaBDTO.Porcentajefinanciado ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "CONCESIONDEFINITIVA", DbType.String, solHojaBDTO.Concesiondefinitiva);
            dbProvider.AddInParameter(dbCommand, "VENTAENERGIA", DbType.String, solHojaBDTO.Ventaenergia);
            dbProvider.AddInParameter(dbCommand, "EJECUCIONOBRA", DbType.String, solHojaBDTO.Ejecucionobra);
            dbProvider.AddInParameter(dbCommand, "CONTRATOSFINANCIEROS", DbType.String, solHojaBDTO.Contratosfinancieros);
            dbProvider.AddInParameter(dbCommand, "OBSERVACIONES", DbType.String, solHojaBDTO.Observaciones);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, solHojaBDTO.UsuCreacion);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.ExecuteNonQuery(dbCommand);

            return true;
        }

        public bool DeleteSolHojaBById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteSolHojaBById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);
            return true;
        }

        public int GetLastSolHojaBId()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastSolHojaBId);
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

        public SolHojaBDTO GetSolHojaBById(int id)
        {
            SolHojaBDTO ob = new SolHojaBDTO();
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(Helper.SqlGetSolHojaBById);
            dbProvider.AddInParameter(commandHoja, "PROYCODI", DbType.Int32, id);
            dbProvider.AddInParameter(commandHoja, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.ExecuteNonQuery(commandHoja);
            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                if (dr.Read())
                {
                    ob.SolhojabCodi = !dr.IsDBNull(dr.GetOrdinal("SOLHOJABCODI")) ? dr.GetInt32(dr.GetOrdinal("SOLHOJABCODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Estudiofactibilidad = !dr.IsDBNull(dr.GetOrdinal("ESTUDIOFACTIBILIDAD")) ? dr.GetDecimal(dr.GetOrdinal("ESTUDIOFACTIBILIDAD")) : (decimal?)null;
                    ob.Investigacionescampo = !dr.IsDBNull(dr.GetOrdinal("INVESTIGACIONESCAMPO")) ? dr.GetDecimal(dr.GetOrdinal("INVESTIGACIONESCAMPO")) : (decimal?)null;
                    ob.Gestionesfinancieras = !dr.IsDBNull(dr.GetOrdinal("GESTIONESFINANCIERAS")) ? dr.GetDecimal(dr.GetOrdinal("GESTIONESFINANCIERAS")) : (decimal?)null;
                    ob.Disenospermisos = !dr.IsDBNull(dr.GetOrdinal("DISENOSPERMISOS")) ? dr.GetDecimal(dr.GetOrdinal("DISENOSPERMISOS")) : (decimal?)null;
                    ob.Obrasciviles = !dr.IsDBNull(dr.GetOrdinal("OBRASCIVILES")) ? dr.GetDecimal(dr.GetOrdinal("OBRASCIVILES")) : (decimal?)null;
                    ob.Equipamiento = !dr.IsDBNull(dr.GetOrdinal("EQUIPAMIENTO")) ? dr.GetDecimal(dr.GetOrdinal("EQUIPAMIENTO")) : (decimal?)null;
                    ob.Lineatransmision = !dr.IsDBNull(dr.GetOrdinal("LINEATRANSMISION")) ? dr.GetDecimal(dr.GetOrdinal("LINEATRANSMISION")) : (decimal?)null;
                    ob.Administracion = !dr.IsDBNull(dr.GetOrdinal("ADMINISTRACION")) ? dr.GetDecimal(dr.GetOrdinal("ADMINISTRACION")) : (decimal?)null;
                    ob.Aduanas = !dr.IsDBNull(dr.GetOrdinal("ADUANAS")) ? dr.GetDecimal(dr.GetOrdinal("ADUANAS")) : (decimal?)null;
                    ob.Supervision = !dr.IsDBNull(dr.GetOrdinal("SUPERVISION")) ? dr.GetDecimal(dr.GetOrdinal("SUPERVISION")) : (decimal?)null;
                    ob.Gastosgestion = !dr.IsDBNull(dr.GetOrdinal("GASTOSGESTION")) ? dr.GetDecimal(dr.GetOrdinal("GASTOSGESTION")) : (decimal?)null;
                    ob.Imprevistos = !dr.IsDBNull(dr.GetOrdinal("IMPREVISTOS")) ? dr.GetDecimal(dr.GetOrdinal("IMPREVISTOS")) : (decimal?)null;
                    ob.Igv = !dr.IsDBNull(dr.GetOrdinal("IGV")) ? dr.GetDecimal(dr.GetOrdinal("IGV")) : (decimal?)null;
                    ob.Otrosgastos = !dr.IsDBNull(dr.GetOrdinal("OTROSGASTOS")) ? dr.GetDecimal(dr.GetOrdinal("OTROSGASTOS")) : (decimal?)null;
                    ob.Inversiontotalsinigv = !dr.IsDBNull(dr.GetOrdinal("INVERSIONTOTALSINIGV")) ? dr.GetDecimal(dr.GetOrdinal("INVERSIONTOTALSINIGV")) : (decimal?)null;
                    ob.Inversiontotalconigv = !dr.IsDBNull(dr.GetOrdinal("INVERSIONTOTALCONIGV")) ? dr.GetDecimal(dr.GetOrdinal("INVERSIONTOTALCONIGV")) : (decimal?)null;
                    ob.Financiamientotipo = !dr.IsDBNull(dr.GetOrdinal("FINANCIAMIENTOTIPO")) ? dr.GetString(dr.GetOrdinal("FINANCIAMIENTOTIPO")) : string.Empty;
                    ob.Financiamientoestado = !dr.IsDBNull(dr.GetOrdinal("FINANCIAMIENTOESTADO")) ? dr.GetString(dr.GetOrdinal("FINANCIAMIENTOESTADO")) : string.Empty;
                    ob.Porcentajefinanciado = !dr.IsDBNull(dr.GetOrdinal("PORCENTAJEFINANCIADO")) ? dr.GetDecimal(dr.GetOrdinal("PORCENTAJEFINANCIADO")) : (decimal?)null;
                    ob.Concesiondefinitiva = !dr.IsDBNull(dr.GetOrdinal("CONCESIONDEFINITIVA")) ? dr.GetString(dr.GetOrdinal("CONCESIONDEFINITIVA")) : string.Empty;
                    ob.Ventaenergia = !dr.IsDBNull(dr.GetOrdinal("VENTAENERGIA")) ? dr.GetString(dr.GetOrdinal("VENTAENERGIA")) : string.Empty;
                    ob.Ejecucionobra = !dr.IsDBNull(dr.GetOrdinal("EJECUCIONOBRA")) ? dr.GetString(dr.GetOrdinal("EJECUCIONOBRA")) : string.Empty;
                    ob.Contratosfinancieros = !dr.IsDBNull(dr.GetOrdinal("CONTRATOSFINANCIEROS")) ? dr.GetString(dr.GetOrdinal("CONTRATOSFINANCIEROS")) : string.Empty;
                    ob.Observaciones = !dr.IsDBNull(dr.GetOrdinal("OBSERVACIONES")) ? dr.GetString(dr.GetOrdinal("OBSERVACIONES")) : string.Empty;
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : string.Empty;
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                }

            }
            return ob;
        }

        public bool UpdateSolHojaB(SolHojaBDTO solHojaBDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlUpdateSolHojaB);
            
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, ObtenerValorOrDefault(solHojaBDTO.ProyCodi, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "ESTUDIOFACTIBILIDAD", DbType.Decimal, solHojaBDTO.Estudiofactibilidad);
            dbProvider.AddInParameter(dbCommand, "INVESTIGACIONESCAMPO", DbType.Decimal, solHojaBDTO.Investigacionescampo);
            dbProvider.AddInParameter(dbCommand, "GESTIONESFINANCIERAS", DbType.Decimal, solHojaBDTO.Gestionesfinancieras);
            dbProvider.AddInParameter(dbCommand, "DISEÑOSPERMISOS", DbType.Decimal, solHojaBDTO.Disenospermisos);
            dbProvider.AddInParameter(dbCommand, "OBRASCIVILES", DbType.Decimal, solHojaBDTO.Obrasciviles);
            dbProvider.AddInParameter(dbCommand, "EQUIPAMIENTO", DbType.Decimal, solHojaBDTO.Equipamiento);
            dbProvider.AddInParameter(dbCommand, "LINEATRANSMISION", DbType.Decimal, solHojaBDTO.Lineatransmision);
            dbProvider.AddInParameter(dbCommand, "ADMINISTRACION", DbType.Decimal, solHojaBDTO.Administracion);
            dbProvider.AddInParameter(dbCommand, "ADUANAS", DbType.Decimal, solHojaBDTO.Aduanas);
            dbProvider.AddInParameter(dbCommand, "SUPERVISION", DbType.Decimal, solHojaBDTO.Supervision);
            dbProvider.AddInParameter(dbCommand, "GASTOSGESTION", DbType.Int32, solHojaBDTO.Gastosgestion);
            dbProvider.AddInParameter(dbCommand, "IMPREVISTOS", DbType.Decimal, solHojaBDTO.Imprevistos);
            dbProvider.AddInParameter(dbCommand, "IGV", DbType.Decimal, solHojaBDTO.Igv);
            dbProvider.AddInParameter(dbCommand, "OTROSGASTOS", DbType.Decimal, solHojaBDTO.Otrosgastos);
            dbProvider.AddInParameter(dbCommand, "INVERSIONTOTALSINIGV", DbType.Decimal, solHojaBDTO.Inversiontotalsinigv);
            dbProvider.AddInParameter(dbCommand, "INVERSIONTOTALCONIGV", DbType.Decimal, solHojaBDTO.Inversiontotalconigv);
            dbProvider.AddInParameter(dbCommand, "FINANCIAMIENTOTIPO", DbType.String, solHojaBDTO.Financiamientotipo);
            dbProvider.AddInParameter(dbCommand, "FINANCIAMIENTOESTADO", DbType.String, solHojaBDTO.Financiamientoestado);
            dbProvider.AddInParameter(dbCommand, "PORCENTAJEFINANCIADO", DbType.Decimal, solHojaBDTO.Porcentajefinanciado);
            dbProvider.AddInParameter(dbCommand, "CONCESIONDEFINITIVA", DbType.DateTime, solHojaBDTO.Concesiondefinitiva);
            dbProvider.AddInParameter(dbCommand, "VENTAENERGIA", DbType.DateTime, solHojaBDTO.Ventaenergia);
            dbProvider.AddInParameter(dbCommand, "EJECUCIONOBRA", DbType.DateTime, solHojaBDTO.Ejecucionobra);
            dbProvider.AddInParameter(dbCommand, "CONTRATOSFINANCIEROS", DbType.DateTime, solHojaBDTO.Contratosfinancieros);
            dbProvider.AddInParameter(dbCommand, "OBSERVACIONES", DbType.String, solHojaBDTO.Observaciones);
            dbProvider.AddInParameter(dbCommand, "USU_MODIFICACION", DbType.String, solHojaBDTO.UsuModificacion);
            dbProvider.AddInParameter(dbCommand, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "SOLHOJABCODI", DbType.Int32, ObtenerValorOrDefault(solHojaBDTO.SolhojabCodi, typeof(int)));
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

        public List<SolHojaBDTO> GetSolHojaBByFilter(string plancodi, string empresa, string estado)
        {
            List<SolHojaBDTO> oblist = new List<SolHojaBDTO>();
            string query = $@"
                SELECT CGB.*, TR.EMPRESANOM, TR.PROYNOMBRE, TR.PROYDESCRIPCION, TP.TIPONOMBRE, TF.TIPOFINOMBRE, TR.PROYCONFIDENCIAL  FROM CAM_SOLHOJAB CGB
                INNER JOIN CAM_TRNSMPROYECTO TR ON TR.PROYCODI = CGB.PROYCODI
                INNER JOIN CAM_PLANTRANSMISION PL ON PL.PLANCODI = TR.PLANCODI
                INNER JOIN CAM_TIPOPROYECTO TP ON TP.TIPOCODI = TR.TIPOCODI
                LEFT JOIN CAM_TIPOFICHAPROYECTO TF ON TF.TIPOFICODI = TR.TIPOFICODI
                WHERE TR.PERICODI  IN ({plancodi}) AND 
                PL.CODEMPRESA IN ({empresa})  AND 
                CGB.IND_DEL = 0 AND 
                PL.PLANESTADO ='{estado}'
                ORDER BY TR.PERICODI, CGB.PROYCODI,PL.CODEMPRESA, CGB.SOLHOJABCODI ASC";
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(query);

            dbProvider.ExecuteNonQuery(commandHoja);
            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                while (dr.Read())
                {
                    SolHojaBDTO ob = new SolHojaBDTO();
                    ob.SolhojabCodi = !dr.IsDBNull(dr.GetOrdinal("SOLHOJABCODI")) ? dr.GetInt32(dr.GetOrdinal("SOLHOJABCODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Estudiofactibilidad = !dr.IsDBNull(dr.GetOrdinal("ESTUDIOFACTIBILIDAD")) ? dr.GetDecimal(dr.GetOrdinal("ESTUDIOFACTIBILIDAD")) : (decimal?)null;
                    ob.Investigacionescampo = !dr.IsDBNull(dr.GetOrdinal("INVESTIGACIONESCAMPO")) ? dr.GetDecimal(dr.GetOrdinal("INVESTIGACIONESCAMPO")) : (decimal?)null;
                    ob.Gestionesfinancieras = !dr.IsDBNull(dr.GetOrdinal("GESTIONESFINANCIERAS")) ? dr.GetDecimal(dr.GetOrdinal("GESTIONESFINANCIERAS")) : (decimal?)null;
                    ob.Disenospermisos = !dr.IsDBNull(dr.GetOrdinal("DISENOSPERMISOS")) ? dr.GetDecimal(dr.GetOrdinal("DISENOSPERMISOS")) : (decimal?)null;
                    ob.Obrasciviles = !dr.IsDBNull(dr.GetOrdinal("OBRASCIVILES")) ? dr.GetDecimal(dr.GetOrdinal("OBRASCIVILES")) : (decimal?)null;
                    ob.Equipamiento = !dr.IsDBNull(dr.GetOrdinal("EQUIPAMIENTO")) ? dr.GetDecimal(dr.GetOrdinal("EQUIPAMIENTO")) : (decimal?)null;
                    ob.Lineatransmision = !dr.IsDBNull(dr.GetOrdinal("LINEATRANSMISION")) ? dr.GetDecimal(dr.GetOrdinal("LINEATRANSMISION")) : (decimal?)null;
                    ob.Administracion = !dr.IsDBNull(dr.GetOrdinal("ADMINISTRACION")) ? dr.GetDecimal(dr.GetOrdinal("ADMINISTRACION")) : (decimal?)null;
                    ob.Aduanas = !dr.IsDBNull(dr.GetOrdinal("ADUANAS")) ? dr.GetDecimal(dr.GetOrdinal("ADUANAS")) : (decimal?)null;
                    ob.Supervision = !dr.IsDBNull(dr.GetOrdinal("SUPERVISION")) ? dr.GetDecimal(dr.GetOrdinal("SUPERVISION")) : (decimal?)null;
                    ob.Gastosgestion = !dr.IsDBNull(dr.GetOrdinal("GASTOSGESTION")) ? dr.GetDecimal(dr.GetOrdinal("GASTOSGESTION")) : (decimal?)null;
                    ob.Imprevistos = !dr.IsDBNull(dr.GetOrdinal("IMPREVISTOS")) ? dr.GetDecimal(dr.GetOrdinal("IMPREVISTOS")) : (decimal?)null;
                    ob.Igv = !dr.IsDBNull(dr.GetOrdinal("IGV")) ? dr.GetDecimal(dr.GetOrdinal("IGV")) : (decimal?)null;
                    ob.Otrosgastos = !dr.IsDBNull(dr.GetOrdinal("OTROSGASTOS")) ? dr.GetDecimal(dr.GetOrdinal("OTROSGASTOS")) : (decimal?)null;
                    ob.Inversiontotalsinigv = !dr.IsDBNull(dr.GetOrdinal("INVERSIONTOTALSINIGV")) ? dr.GetDecimal(dr.GetOrdinal("INVERSIONTOTALSINIGV")) : (decimal?)null;
                    ob.Inversiontotalconigv = !dr.IsDBNull(dr.GetOrdinal("INVERSIONTOTALCONIGV")) ? dr.GetDecimal(dr.GetOrdinal("INVERSIONTOTALCONIGV")) : (decimal?)null;
                    ob.Financiamientotipo = !dr.IsDBNull(dr.GetOrdinal("FINANCIAMIENTOTIPO")) ? dr.GetString(dr.GetOrdinal("FINANCIAMIENTOTIPO")) : string.Empty;
                    ob.Financiamientoestado = !dr.IsDBNull(dr.GetOrdinal("FINANCIAMIENTOESTADO")) ? dr.GetString(dr.GetOrdinal("FINANCIAMIENTOESTADO")) : string.Empty;
                    ob.Porcentajefinanciado = !dr.IsDBNull(dr.GetOrdinal("PORCENTAJEFINANCIADO")) ? dr.GetDecimal(dr.GetOrdinal("PORCENTAJEFINANCIADO")) : (decimal?)null;
                    ob.Concesiondefinitiva = !dr.IsDBNull(dr.GetOrdinal("CONCESIONDEFINITIVA")) ? dr.GetString(dr.GetOrdinal("CONCESIONDEFINITIVA")) : string.Empty;
                    ob.Ventaenergia = !dr.IsDBNull(dr.GetOrdinal("VENTAENERGIA")) ? dr.GetString(dr.GetOrdinal("VENTAENERGIA")) : string.Empty;
                    ob.Ejecucionobra = !dr.IsDBNull(dr.GetOrdinal("EJECUCIONOBRA")) ? dr.GetString(dr.GetOrdinal("EJECUCIONOBRA")) : string.Empty;
                    ob.Contratosfinancieros = !dr.IsDBNull(dr.GetOrdinal("CONTRATOSFINANCIEROS")) ? dr.GetString(dr.GetOrdinal("CONTRATOSFINANCIEROS")) : string.Empty;
                    ob.Observaciones = !dr.IsDBNull(dr.GetOrdinal("OBSERVACIONES")) ? dr.GetString(dr.GetOrdinal("OBSERVACIONES")) : string.Empty;
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : string.Empty;
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                    ob.Empresa = dr.IsDBNull(dr.GetOrdinal("EMPRESANOM")) ? null : dr.GetString(dr.GetOrdinal("EMPRESANOM"));
                    ob.NombreProyecto = dr.IsDBNull(dr.GetOrdinal("PROYNOMBRE")) ? null : dr.GetString(dr.GetOrdinal("PROYNOMBRE"));
                    ob.DetalleProyecto = dr.IsDBNull(dr.GetOrdinal("PROYDESCRIPCION")) ? null : dr.GetString(dr.GetOrdinal("PROYDESCRIPCION"));
                    ob.TipoProyecto = dr.IsDBNull(dr.GetOrdinal("TIPONOMBRE")) ? null : dr.GetString(dr.GetOrdinal("TIPONOMBRE"));
                    ob.SubTipoProyecto = dr.IsDBNull(dr.GetOrdinal("TIPOFINOMBRE")) ? null : dr.GetString(dr.GetOrdinal("TIPOFINOMBRE"));
                    oblist.Add(ob);
                }

            }
            return oblist;
        }




    }
}
