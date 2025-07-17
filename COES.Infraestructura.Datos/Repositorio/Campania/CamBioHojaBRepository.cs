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
    public class CamBioHojaBRepository : RepositoryBase, ICamBioHojaBRepository
    {

        public CamBioHojaBRepository(string strConn) : base(strConn) { }

        CamBioHojaBHelper Helper = new CamBioHojaBHelper();

        public List<BioHojaBDTO> GetBioHojaBProyCodi(int proyCodi)
        {
            List<BioHojaBDTO> bioHojaBDTOs = new List<BioHojaBDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetBioHojaBProyCodi);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, proyCodi); // Asumiendo que PROYCODI es de tipo int

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    BioHojaBDTO ob = new BioHojaBDTO();
                    ob.BiohojabCodi = !dr.IsDBNull(dr.GetOrdinal("BIOHOJABCODI")) ? dr.GetInt32(dr.GetOrdinal("BIOHOJABCODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Estudiofactibilidad = !dr.IsDBNull(dr.GetOrdinal("ESTUDIOFACTIBILIDAD")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("ESTUDIOFACTIBILIDAD")) : null;
                    ob.Investigacionescampo = !dr.IsDBNull(dr.GetOrdinal("INVESTIGACIONESCAMPO")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("INVESTIGACIONESCAMPO")) : null;
                    ob.Gestionesfinancieras = !dr.IsDBNull(dr.GetOrdinal("GESTIONESFINANCIERAS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("GESTIONESFINANCIERAS")) : null;
                    ob.Disenospermisos = !dr.IsDBNull(dr.GetOrdinal("DISENOSPERMISOS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("DISENOSPERMISOS")) : null;
                    ob.Obrasciviles = !dr.IsDBNull(dr.GetOrdinal("OBRASCIVILES")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("OBRASCIVILES")) : null;
                    ob.Equipamiento = !dr.IsDBNull(dr.GetOrdinal("EQUIPAMIENTO")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("EQUIPAMIENTO")) : null;
                    ob.Lineatransmision = !dr.IsDBNull(dr.GetOrdinal("LINEATRANSMISION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("LINEATRANSMISION")) : null;
                    ob.Obrasregulacion = !dr.IsDBNull(dr.GetOrdinal("OBRASREGULACION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("OBRASREGULACION")) : null;
                    ob.Administracion = !dr.IsDBNull(dr.GetOrdinal("ADMINISTRACION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("ADMINISTRACION")) : null;
                    ob.Aduanas = !dr.IsDBNull(dr.GetOrdinal("ADUANAS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("ADUANAS")) : null;
                    ob.Supervision = !dr.IsDBNull(dr.GetOrdinal("SUPERVISION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("SUPERVISION")) : null;
                    ob.Gastosgestion = !dr.IsDBNull(dr.GetOrdinal("GASTOSGESTION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("GASTOSGESTION")) : null;
                    ob.Imprevistos = !dr.IsDBNull(dr.GetOrdinal("IMPREVISTOS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("IMPREVISTOS")) : null;
                    ob.Igv = !dr.IsDBNull(dr.GetOrdinal("IGV")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("IGV")) : null;
                    ob.Otrosgastos = !dr.IsDBNull(dr.GetOrdinal("OTROSGASTOS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("OTROSGASTOS")) : null;
                    ob.Inversiontotalsinigv = !dr.IsDBNull(dr.GetOrdinal("INVERSIONTOTALSINIGV")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("INVERSIONTOTALSINIGV")) : null;
                    ob.Inversiontotalconigv = !dr.IsDBNull(dr.GetOrdinal("INVERSIONTOTALCONIGV")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("INVERSIONTOTALCONIGV")) : null;
                    ob.Financiamientotipo = !dr.IsDBNull(dr.GetOrdinal("FINANCIAMIENTOTIPO")) ? dr.GetString(dr.GetOrdinal("FINANCIAMIENTOTIPO")) : string.Empty;
                    ob.Financiamientoestado = !dr.IsDBNull(dr.GetOrdinal("FINANCIAMIENTOESTADO")) ? dr.GetString(dr.GetOrdinal("FINANCIAMIENTOESTADO")) : string.Empty;
                    ob.Porcentajefinanciado = !dr.IsDBNull(dr.GetOrdinal("PORCENTAJEFINANCIADO")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("PORCENTAJEFINANCIADO")) : null;
                    ob.Concesiondefinitiva = !dr.IsDBNull(dr.GetOrdinal("CONCESIONDEFINITIVA")) ? dr.GetString(dr.GetOrdinal("CONCESIONDEFINITIVA")) : string.Empty;
                    ob.Ventaenergia = !dr.IsDBNull(dr.GetOrdinal("VENTAENERGIA")) ? dr.GetString(dr.GetOrdinal("VENTAENERGIA")) : string.Empty;
                    ob.Ejecucionobra = !dr.IsDBNull(dr.GetOrdinal("EJECUCIONOBRA")) ? dr.GetString(dr.GetOrdinal("EJECUCIONOBRA")) : string.Empty;
                    ob.Contratosfinancieros = !dr.IsDBNull(dr.GetOrdinal("CONTRATOSFINANCIEROS")) ? dr.GetString(dr.GetOrdinal("CONTRATOSFINANCIEROS")) : string.Empty;
                    ob.Observaciones = !dr.IsDBNull(dr.GetOrdinal("OBSERVACIONES")) ? dr.GetString(dr.GetOrdinal("OBSERVACIONES")) : string.Empty;
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : string.Empty;
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                    bioHojaBDTOs.Add(ob);
                }
            }

            return bioHojaBDTOs;
        }


        public bool SaveBioHojaB(BioHojaBDTO bioHojaBDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveBioHojaB);
            dbProvider.AddInParameter(dbCommand, "BIOHOJABCODI", DbType.Int32, bioHojaBDTO.BiohojabCodi);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, bioHojaBDTO.ProyCodi);
            dbProvider.AddInParameter(dbCommand, "ESTUDIOFACTIBILIDAD", DbType.Decimal, bioHojaBDTO.Estudiofactibilidad ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "INVESTIGACIONESCAMPO", DbType.Decimal, bioHojaBDTO.Investigacionescampo ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "GESTIONESFINANCIERAS", DbType.Decimal, bioHojaBDTO.Gestionesfinancieras ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "DISENOSPERMISOS", DbType.Decimal, bioHojaBDTO.Disenospermisos ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "OBRASCIVILES", DbType.Decimal, bioHojaBDTO.Obrasciviles ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "EQUIPAMIENTO", DbType.Decimal, bioHojaBDTO.Equipamiento ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "LINEATRANSMISION", DbType.Decimal, bioHojaBDTO.Lineatransmision ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "OBRASREGULACION", DbType.Decimal, bioHojaBDTO.Obrasregulacion ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "ADMINISTRACION", DbType.Decimal, bioHojaBDTO.Administracion ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "ADUANAS", DbType.Decimal, bioHojaBDTO.Aduanas ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "SUPERVISION", DbType.Decimal, bioHojaBDTO.Supervision ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "GASTOSGESTION", DbType.Decimal, bioHojaBDTO.Gastosgestion ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "IMPREVISTOS", DbType.Decimal, bioHojaBDTO.Imprevistos ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "IGV", DbType.Decimal, bioHojaBDTO.Igv ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "OTROSGASTOS", DbType.Int32, bioHojaBDTO.Otrosgastos); // No se modifica
            dbProvider.AddInParameter(dbCommand, "INVERSIONTOTALSINIGV", DbType.Decimal, bioHojaBDTO.Inversiontotalsinigv ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "INVERSIONTOTALCONIGV", DbType.Decimal, bioHojaBDTO.Inversiontotalconigv ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "FINANCIAMIENTOTIPO", DbType.String, bioHojaBDTO.Financiamientotipo);
            dbProvider.AddInParameter(dbCommand, "FINANCIAMIENTOESTADO", DbType.String, bioHojaBDTO.Financiamientoestado);
            dbProvider.AddInParameter(dbCommand, "PORCENTAJEFINANCIADO", DbType.Decimal, bioHojaBDTO.Porcentajefinanciado ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "CONCESIONDEFINITIVA", DbType.String, bioHojaBDTO.Concesiondefinitiva);
            dbProvider.AddInParameter(dbCommand, "VENTAENERGIA", DbType.String, bioHojaBDTO.Ventaenergia);
            dbProvider.AddInParameter(dbCommand, "EJECUCIONOBRA", DbType.String, bioHojaBDTO.Ejecucionobra);
            dbProvider.AddInParameter(dbCommand, "CONTRATOSFINANCIEROS", DbType.String, bioHojaBDTO.Contratosfinancieros);
            dbProvider.AddInParameter(dbCommand, "OBSERVACIONES", DbType.String, bioHojaBDTO.Observaciones);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, bioHojaBDTO.UsuCreacion);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, Constantes.IndDel);

            dbProvider.ExecuteNonQuery(dbCommand);

            return true;
        }

        public bool DeleteBioHojaBById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteBioHojaBById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);
            return true;
        }

        public int GetLastBioHojaBId()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastBioHojaBId);
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

        public BioHojaBDTO GetBioHojaBById(int id)
        {
            BioHojaBDTO ob = new BioHojaBDTO();
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(Helper.SqlGetBioHojaBById);
            dbProvider.AddInParameter(commandHoja, "PROYCODI", DbType.Int32, id);
            dbProvider.AddInParameter(commandHoja, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.ExecuteNonQuery(commandHoja);
            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                if (dr.Read())
                {
                    ob.BiohojabCodi = !dr.IsDBNull(dr.GetOrdinal("BIOHOJABCODI")) ? dr.GetInt32(dr.GetOrdinal("BIOHOJABCODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Estudiofactibilidad = !dr.IsDBNull(dr.GetOrdinal("ESTUDIOFACTIBILIDAD")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("ESTUDIOFACTIBILIDAD")) : null;
                    ob.Investigacionescampo = !dr.IsDBNull(dr.GetOrdinal("INVESTIGACIONESCAMPO")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("INVESTIGACIONESCAMPO")) : null;
                    ob.Gestionesfinancieras = !dr.IsDBNull(dr.GetOrdinal("GESTIONESFINANCIERAS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("GESTIONESFINANCIERAS")) : null;
                    ob.Disenospermisos = !dr.IsDBNull(dr.GetOrdinal("DISENOSPERMISOS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("DISENOSPERMISOS")) : null;
                    ob.Obrasciviles = !dr.IsDBNull(dr.GetOrdinal("OBRASCIVILES")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("OBRASCIVILES")) : null;
                    ob.Equipamiento = !dr.IsDBNull(dr.GetOrdinal("EQUIPAMIENTO")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("EQUIPAMIENTO")) : null;
                    ob.Lineatransmision = !dr.IsDBNull(dr.GetOrdinal("LINEATRANSMISION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("LINEATRANSMISION")) : null;
                    ob.Obrasregulacion = !dr.IsDBNull(dr.GetOrdinal("OBRASREGULACION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("OBRASREGULACION")) : null;
                    ob.Administracion = !dr.IsDBNull(dr.GetOrdinal("ADMINISTRACION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("ADMINISTRACION")) : null;
                    ob.Aduanas = !dr.IsDBNull(dr.GetOrdinal("ADUANAS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("ADUANAS")) : null;
                    ob.Supervision = !dr.IsDBNull(dr.GetOrdinal("SUPERVISION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("SUPERVISION")) : null;
                    ob.Gastosgestion = !dr.IsDBNull(dr.GetOrdinal("GASTOSGESTION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("GASTOSGESTION")) : null;
                    ob.Imprevistos = !dr.IsDBNull(dr.GetOrdinal("IMPREVISTOS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("IMPREVISTOS")) : null;
                    ob.Igv = !dr.IsDBNull(dr.GetOrdinal("IGV")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("IGV")) : null;
                    ob.Otrosgastos = !dr.IsDBNull(dr.GetOrdinal("OTROSGASTOS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("OTROSGASTOS")) : null;
                    ob.Inversiontotalsinigv = !dr.IsDBNull(dr.GetOrdinal("INVERSIONTOTALSINIGV")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("INVERSIONTOTALSINIGV")) : null;
                    ob.Inversiontotalconigv = !dr.IsDBNull(dr.GetOrdinal("INVERSIONTOTALCONIGV")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("INVERSIONTOTALCONIGV")) : null;
                    ob.Financiamientotipo = !dr.IsDBNull(dr.GetOrdinal("FINANCIAMIENTOTIPO")) ? dr.GetString(dr.GetOrdinal("FINANCIAMIENTOTIPO")) : string.Empty;
                    ob.Financiamientoestado = !dr.IsDBNull(dr.GetOrdinal("FINANCIAMIENTOESTADO")) ? dr.GetString(dr.GetOrdinal("FINANCIAMIENTOESTADO")) : string.Empty;
                    ob.Porcentajefinanciado = !dr.IsDBNull(dr.GetOrdinal("PORCENTAJEFINANCIADO")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("PORCENTAJEFINANCIADO")) : null;
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

        public bool UpdateBioHojaB(BioHojaBDTO bioHojaBDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlUpdateBioHojaB);

            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, ObtenerValorOrDefault(bioHojaBDTO.ProyCodi, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "ESTUDIOFACTIBILIDAD", DbType.Decimal, bioHojaBDTO.Estudiofactibilidad);
            dbProvider.AddInParameter(dbCommand, "INVESTIGACIONESCAMPO", DbType.Decimal, bioHojaBDTO.Investigacionescampo);
            dbProvider.AddInParameter(dbCommand, "GESTIONESFINANCIERAS", DbType.Decimal, bioHojaBDTO.Gestionesfinancieras);
            dbProvider.AddInParameter(dbCommand, "DISEÑOSPERMISOS", DbType.Decimal, bioHojaBDTO.Disenospermisos);
            dbProvider.AddInParameter(dbCommand, "OBRASCIVILES", DbType.Decimal, bioHojaBDTO.Obrasciviles);
            dbProvider.AddInParameter(dbCommand, "EQUIPAMIENTO", DbType.Decimal, bioHojaBDTO.Equipamiento);
            dbProvider.AddInParameter(dbCommand, "LINEATRANSMISION", DbType.Decimal, bioHojaBDTO.Lineatransmision);
            dbProvider.AddInParameter(dbCommand, "ADMINISTRACION", DbType.Decimal, bioHojaBDTO.Administracion);
            dbProvider.AddInParameter(dbCommand, "ADUANAS", DbType.Decimal, bioHojaBDTO.Aduanas);
            dbProvider.AddInParameter(dbCommand, "SUPERVISION", DbType.Decimal, bioHojaBDTO.Supervision);
            dbProvider.AddInParameter(dbCommand, "GASTOSGESTION", DbType.Decimal, bioHojaBDTO.Gastosgestion);
            dbProvider.AddInParameter(dbCommand, "IMPREVISTOS", DbType.Decimal, bioHojaBDTO.Imprevistos);
            dbProvider.AddInParameter(dbCommand, "IGV", DbType.Decimal, bioHojaBDTO.Igv);
            dbProvider.AddInParameter(dbCommand, "OTROSGASTOS", DbType.Int32, bioHojaBDTO.Otrosgastos);
            dbProvider.AddInParameter(dbCommand, "INVERSIONTOTALSINIGV", DbType.Decimal, bioHojaBDTO.Inversiontotalsinigv);
            dbProvider.AddInParameter(dbCommand, "INVERSIONTOTALCONIGV", DbType.Decimal, bioHojaBDTO.Inversiontotalconigv);
            dbProvider.AddInParameter(dbCommand, "FINANCIAMIENTOTIPO", DbType.String, bioHojaBDTO.Financiamientotipo);
            dbProvider.AddInParameter(dbCommand, "FINANCIAMIENTOESTADO", DbType.String, bioHojaBDTO.Financiamientoestado);
            dbProvider.AddInParameter(dbCommand, "PORCENTAJEFINANCIADO", DbType.Decimal, bioHojaBDTO.Porcentajefinanciado);
            dbProvider.AddInParameter(dbCommand, "CONCESIONDEFINITIVA", DbType.DateTime, bioHojaBDTO.Concesiondefinitiva);
            dbProvider.AddInParameter(dbCommand, "VENTAENERGIA", DbType.DateTime, bioHojaBDTO.Ventaenergia);
            dbProvider.AddInParameter(dbCommand, "EJECUCIONOBRA", DbType.DateTime, bioHojaBDTO.Ejecucionobra);
            dbProvider.AddInParameter(dbCommand, "CONTRATOSFINANCIEROS", DbType.DateTime, bioHojaBDTO.Contratosfinancieros);
            dbProvider.AddInParameter(dbCommand, "OBSERVACIONES", DbType.String, bioHojaBDTO.Observaciones);
            dbProvider.AddInParameter(dbCommand, "USU_MODIFICACION", DbType.String, bioHojaBDTO.UsuModificacion);
            dbProvider.AddInParameter(dbCommand, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "BIOHOJABCODI", DbType.Int32, bioHojaBDTO.BiohojabCodi);
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

        public List<BioHojaBDTO> GetBioHojaBByFilter(string plancodi, string empresa, string estado)
        {
            List<BioHojaBDTO> oblist = new List<BioHojaBDTO>();
            string query = $@"
                SELECT CGB.*, TR.EMPRESANOM, TR.PROYNOMBRE, TR.PROYDESCRIPCION, TP.TIPONOMBRE, TF.TIPOFINOMBRE,TR.PROYCONFIDENCIAL  FROM CAM_BIOHOJAB CGB
                INNER JOIN CAM_TRNSMPROYECTO TR ON TR.PROYCODI = CGB.PROYCODI
                INNER JOIN CAM_PLANTRANSMISION PL ON PL.PLANCODI = TR.PLANCODI
                INNER JOIN CAM_TIPOPROYECTO TP ON TP.TIPOCODI = TR.TIPOCODI
                LEFT JOIN CAM_TIPOFICHAPROYECTO TF ON TF.TIPOFICODI = TR.TIPOFICODI
                WHERE TR.PERICODI  IN ({plancodi}) AND 
                PL.CODEMPRESA IN ({empresa})  AND 
                CGB.IND_DEL = 0 AND 
                PL.PLANESTADO ='{estado}'
                ORDER BY TR.PERICODI, CGB.PROYCODI,PL.CODEMPRESA, CGB.BIOHOJABCODI ASC";
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(commandHoja);
            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                while (dr.Read())
                {
                    BioHojaBDTO ob = new BioHojaBDTO();
                    ob.BiohojabCodi = !dr.IsDBNull(dr.GetOrdinal("BIOHOJABCODI")) ? dr.GetInt32(dr.GetOrdinal("BIOHOJABCODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Estudiofactibilidad = !dr.IsDBNull(dr.GetOrdinal("ESTUDIOFACTIBILIDAD")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("ESTUDIOFACTIBILIDAD")) : null;
                    ob.Investigacionescampo = !dr.IsDBNull(dr.GetOrdinal("INVESTIGACIONESCAMPO")) ? dr.GetDecimal(dr.GetOrdinal("INVESTIGACIONESCAMPO")) : 0;
                    ob.Gestionesfinancieras = !dr.IsDBNull(dr.GetOrdinal("GESTIONESFINANCIERAS")) ? dr.GetDecimal(dr.GetOrdinal("GESTIONESFINANCIERAS")) : 0;
                    ob.Disenospermisos = !dr.IsDBNull(dr.GetOrdinal("DISENOSPERMISOS")) ? dr.GetDecimal(dr.GetOrdinal("DISENOSPERMISOS")) : 0;
                    ob.Obrasciviles = !dr.IsDBNull(dr.GetOrdinal("OBRASCIVILES")) ? dr.GetDecimal(dr.GetOrdinal("OBRASCIVILES")) : 0;
                    ob.Equipamiento = !dr.IsDBNull(dr.GetOrdinal("EQUIPAMIENTO")) ? dr.GetDecimal(dr.GetOrdinal("EQUIPAMIENTO")) : 0;
                    ob.Lineatransmision = !dr.IsDBNull(dr.GetOrdinal("LINEATRANSMISION")) ? dr.GetDecimal(dr.GetOrdinal("LINEATRANSMISION")) : 0;
                    ob.Obrasregulacion = !dr.IsDBNull(dr.GetOrdinal("OBRASREGULACION")) ? dr.GetDecimal(dr.GetOrdinal("OBRASREGULACION")) : 0;
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
                    ob.Empresa = !dr.IsDBNull(dr.GetOrdinal("EMPRESANOM")) ? dr.GetString(dr.GetOrdinal("EMPRESANOM")) : string.Empty;
                    ob.NombreProyecto = !dr.IsDBNull(dr.GetOrdinal("PROYNOMBRE")) ? dr.GetString(dr.GetOrdinal("PROYNOMBRE")) : string.Empty;
                    ob.TipoProyecto = !dr.IsDBNull(dr.GetOrdinal("TIPONOMBRE")) ? dr.GetString(dr.GetOrdinal("TIPONOMBRE")) : string.Empty;
                    ob.SubTipoProyecto = !dr.IsDBNull(dr.GetOrdinal("TIPOFINOMBRE")) ? dr.GetString(dr.GetOrdinal("TIPOFINOMBRE")) : string.Empty;
                    oblist.Add(ob);
                }

            }
            return oblist;
        }




    }
}
