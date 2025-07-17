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
    public class CamCCGDERepository : RepositoryBase, ICamCCGDERepository
    {
        public CamCCGDERepository(string strConn) : base(strConn) { }

        CamCCGDEHelper Helper = new CamCCGDEHelper();

        public List<CCGDEDTO> GetCamCCGDE(int proyCodi)
        {
            List<CCGDEDTO> ccgdeDTOs = new List<CCGDEDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetCamCCGDE);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, proyCodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CCGDEDTO ob = new CCGDEDTO
                    {
                        CcgdeCodi = !dr.IsDBNull(dr.GetOrdinal("CCGDECODI")) ? dr.GetInt32(dr.GetOrdinal("CCGDECODI")) : 0,
                        ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0,
                        Estudiofactibilidad = !dr.IsDBNull(dr.GetOrdinal("ESTUDIOFACTIBILIDAD")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("ESTUDIOFACTIBILIDAD")) : null,
                        Investigacionescampo = !dr.IsDBNull(dr.GetOrdinal("INVESTIGACIONESCAMPO")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("INVESTIGACIONESCAMPO")) : null,
                        Gestionesfinancieras = !dr.IsDBNull(dr.GetOrdinal("GESTIONESFINANCIERAS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("GESTIONESFINANCIERAS")) : null,
                        Disenospermisos = !dr.IsDBNull(dr.GetOrdinal("DISENOSPERMISOS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("DISENOSPERMISOS")) : null,
                        Obrasciviles = !dr.IsDBNull(dr.GetOrdinal("OBRASCIVILES")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("OBRASCIVILES")) : null,
                        Equipamiento = !dr.IsDBNull(dr.GetOrdinal("EQUIPAMIENTO")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("EQUIPAMIENTO")) : null,
                        Lineatransmision = !dr.IsDBNull(dr.GetOrdinal("LINEATRANSMISION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("LINEATRANSMISION")) : null,
                        Administracion = !dr.IsDBNull(dr.GetOrdinal("ADMINISTRACION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("ADMINISTRACION")) : null,
                        Aduanas = !dr.IsDBNull(dr.GetOrdinal("ADUANAS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("ADUANAS")) : null,
                        Supervision = !dr.IsDBNull(dr.GetOrdinal("SUPERVISION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("SUPERVISION")) : null,
                        Gastosgestion = !dr.IsDBNull(dr.GetOrdinal("GASTOSGESTION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("GASTOSGESTION")) : null,
                        Imprevistos = !dr.IsDBNull(dr.GetOrdinal("IMPREVISTOS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("IMPREVISTOS")) : null,
                        Igv = !dr.IsDBNull(dr.GetOrdinal("IGV")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("IGV")) : null,
                        Otrosgastos = !dr.IsDBNull(dr.GetOrdinal("OTROSGASTOS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("OTROSGASTOS")) : null,
                        Inversiontotalsinigv = !dr.IsDBNull(dr.GetOrdinal("INVERSIONTOTALSINIGV")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("INVERSIONTOTALSINIGV")) : null,
                        Inversiontotalconigv = !dr.IsDBNull(dr.GetOrdinal("INVERSIONTOTALCONIGV")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("INVERSIONTOTALCONIGV")) : null,
                        Financiamientotipo = !dr.IsDBNull(dr.GetOrdinal("FINANCIAMIENTOTIPO")) ? dr.GetString(dr.GetOrdinal("FINANCIAMIENTOTIPO")) : "",
                        Financiamientoestado = !dr.IsDBNull(dr.GetOrdinal("FINANCIAMIENTOESTADO")) ? dr.GetString(dr.GetOrdinal("FINANCIAMIENTOESTADO")) : "",
                        Porcentajefinanciado = !dr.IsDBNull(dr.GetOrdinal("PORCENTAJEFINANCIADO")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("PORCENTAJEFINANCIADO")) : null,
                        Concesiondefinitiva = !dr.IsDBNull(dr.GetOrdinal("CONCESIONDEFINITIVA")) ? dr.GetString(dr.GetOrdinal("CONCESIONDEFINITIVA")) : "",
                        Ventaenergia = !dr.IsDBNull(dr.GetOrdinal("VENTAENERGIA")) ? dr.GetString(dr.GetOrdinal("VENTAENERGIA")) : "",
                        Ejecucionobra = !dr.IsDBNull(dr.GetOrdinal("EJECUCIONOBRA")) ? dr.GetString(dr.GetOrdinal("EJECUCIONOBRA")) : "",
                        Contratosfinancieros = !dr.IsDBNull(dr.GetOrdinal("CONTRATOSFINANCIEROS")) ? dr.GetString(dr.GetOrdinal("CONTRATOSFINANCIEROS")) : "",
                        Observaciones = !dr.IsDBNull(dr.GetOrdinal("OBSERVACIONES")) ? dr.GetString(dr.GetOrdinal("OBSERVACIONES")) : "",
                        UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : "",
                        FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue,
                        UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : "",
                        FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue,
                        IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : "",

                    };
                    ccgdeDTOs.Add(ob);
                }
            }
            return ccgdeDTOs;
        }

        public bool SaveCamCCGDE(CCGDEDTO ccgdeDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveCamCCGDE);
            dbProvider.AddInParameter(dbCommand, "CCGDECODI", DbType.Int32, ccgdeDTO.CcgdeCodi);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, ccgdeDTO.ProyCodi);
            dbProvider.AddInParameter(dbCommand, "ESTUDIOFACTIBILIDAD", DbType.Decimal, (object)ccgdeDTO.Estudiofactibilidad ?? DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "INVESTIGACIONESCAMPO", DbType.Decimal, (object)ccgdeDTO.Investigacionescampo ?? DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "GESTIONESFINANCIERAS", DbType.Decimal, (object)ccgdeDTO.Gestionesfinancieras ?? DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "DISENOSPERMISOS", DbType.Decimal, (object)ccgdeDTO.Disenospermisos ?? DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "OBRASCIVILES", DbType.Decimal, (object)ccgdeDTO.Obrasciviles ?? DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "EQUIPAMIENTO", DbType.Decimal, (object)ccgdeDTO.Equipamiento ?? DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "LINEATRANSMISION", DbType.Decimal, (object)ccgdeDTO.Lineatransmision ?? DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "ADMINISTRACION", DbType.Decimal, (object)ccgdeDTO.Administracion ?? DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "ADUANAS", DbType.Decimal, (object)ccgdeDTO.Aduanas ?? DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "SUPERVISION", DbType.Decimal, (object)ccgdeDTO.Supervision ?? DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "GASTOSGESTION", DbType.Decimal, (object)ccgdeDTO.Gastosgestion ?? DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "IMPREVISTOS", DbType.Decimal, (object)ccgdeDTO.Imprevistos ?? DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "IGV", DbType.Decimal, (object)ccgdeDTO.Igv ?? DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "OTROSGASTOS", DbType.Decimal, (object)ccgdeDTO.Otrosgastos ?? DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "INVERSIONTOTALSINIGV", DbType.Decimal, (object)ccgdeDTO.Inversiontotalsinigv ?? DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "INVERSIONTOTALCONIGV", DbType.Decimal, (object)ccgdeDTO.Inversiontotalconigv ?? DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "FINANCIAMIENTOTIPO", DbType.String, ccgdeDTO.Financiamientotipo);
            dbProvider.AddInParameter(dbCommand, "FINANCIAMIENTOESTADO", DbType.String, ccgdeDTO.Financiamientoestado);
            dbProvider.AddInParameter(dbCommand, "PORCENTAJEFINANCIADO", DbType.Decimal, (object)ccgdeDTO.Porcentajefinanciado ?? DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "CONCESIONDEFINITIVA", DbType.String, ccgdeDTO.Concesiondefinitiva);
            dbProvider.AddInParameter(dbCommand, "VENTAENERGIA", DbType.String, ccgdeDTO.Ventaenergia);
            dbProvider.AddInParameter(dbCommand, "EJECUCIONOBRA", DbType.String, ccgdeDTO.Ejecucionobra);
            dbProvider.AddInParameter(dbCommand, "CONTRATOSFINANCIEROS", DbType.String, ccgdeDTO.Contratosfinancieros);
            dbProvider.AddInParameter(dbCommand, "OBSERVACIONES", DbType.String, ccgdeDTO.Observaciones);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, ccgdeDTO.UsuCreacion);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        public bool DeleteCamCCGDEById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteCamCCGDEById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);
            return true;
        }

        public int GetLastCamCCGDECodi()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastCamCCGDECodi);
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
        public CCGDEDTO GetCamCCGDEById(int id)
        {
            CCGDEDTO ob = new CCGDEDTO();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetCamCCGDEById);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    ob.CcgdeCodi = !dr.IsDBNull(dr.GetOrdinal("CCGDECODI")) ? dr.GetInt32(dr.GetOrdinal("CCGDECODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Estudiofactibilidad = !dr.IsDBNull(dr.GetOrdinal("ESTUDIOFACTIBILIDAD")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("ESTUDIOFACTIBILIDAD")) : null;
                    ob.Investigacionescampo = !dr.IsDBNull(dr.GetOrdinal("INVESTIGACIONESCAMPO")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("INVESTIGACIONESCAMPO")) : null;
                    ob.Gestionesfinancieras = !dr.IsDBNull(dr.GetOrdinal("GESTIONESFINANCIERAS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("GESTIONESFINANCIERAS")) : null;
                    ob.Disenospermisos = !dr.IsDBNull(dr.GetOrdinal("DISENOSPERMISOS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("DISENOSPERMISOS")) : null;
                    ob.Obrasciviles = !dr.IsDBNull(dr.GetOrdinal("OBRASCIVILES")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("OBRASCIVILES")) : null;
                    ob.Equipamiento = !dr.IsDBNull(dr.GetOrdinal("EQUIPAMIENTO")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("EQUIPAMIENTO")) : null;
                    ob.Lineatransmision = !dr.IsDBNull(dr.GetOrdinal("LINEATRANSMISION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("LINEATRANSMISION")) : null;
                    ob.Administracion = !dr.IsDBNull(dr.GetOrdinal("ADMINISTRACION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("ADMINISTRACION")) : null;
                    ob.Aduanas = !dr.IsDBNull(dr.GetOrdinal("ADUANAS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("ADUANAS")) : null;
                    ob.Supervision = !dr.IsDBNull(dr.GetOrdinal("SUPERVISION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("SUPERVISION")) : null;
                    ob.Gastosgestion = !dr.IsDBNull(dr.GetOrdinal("GASTOSGESTION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("GASTOSGESTION")) : null;
                    ob.Imprevistos = !dr.IsDBNull(dr.GetOrdinal("IMPREVISTOS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("IMPREVISTOS")) : null;
                    ob.Igv = !dr.IsDBNull(dr.GetOrdinal("IGV")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("IGV")) : null;
                    ob.Otrosgastos = !dr.IsDBNull(dr.GetOrdinal("OTROSGASTOS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("OTROSGASTOS")) : null;
                    ob.Inversiontotalsinigv = !dr.IsDBNull(dr.GetOrdinal("INVERSIONTOTALSINIGV")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("INVERSIONTOTALSINIGV")) : null;
                    ob.Inversiontotalconigv = !dr.IsDBNull(dr.GetOrdinal("INVERSIONTOTALCONIGV")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("INVERSIONTOTALCONIGV")) : null;
                    ob.Financiamientotipo = !dr.IsDBNull(dr.GetOrdinal("FINANCIAMIENTOTIPO")) ? dr.GetString(dr.GetOrdinal("FINANCIAMIENTOTIPO")) : "";
                    ob.Financiamientoestado = !dr.IsDBNull(dr.GetOrdinal("FINANCIAMIENTOESTADO")) ? dr.GetString(dr.GetOrdinal("FINANCIAMIENTOESTADO")) : "";
                    ob.Porcentajefinanciado = !dr.IsDBNull(dr.GetOrdinal("PORCENTAJEFINANCIADO")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("PORCENTAJEFINANCIADO")) : null;
                    ob.Concesiondefinitiva = !dr.IsDBNull(dr.GetOrdinal("CONCESIONDEFINITIVA")) ? dr.GetString(dr.GetOrdinal("CONCESIONDEFINITIVA")) : "";
                    ob.Ventaenergia = !dr.IsDBNull(dr.GetOrdinal("VENTAENERGIA")) ? dr.GetString(dr.GetOrdinal("VENTAENERGIA")) : "";
                    ob.Ejecucionobra = !dr.IsDBNull(dr.GetOrdinal("EJECUCIONOBRA")) ? dr.GetString(dr.GetOrdinal("EJECUCIONOBRA")) : "";
                    ob.Contratosfinancieros = !dr.IsDBNull(dr.GetOrdinal("CONTRATOSFINANCIEROS")) ? dr.GetString(dr.GetOrdinal("CONTRATOSFINANCIEROS")) : "";
                    ob.Observaciones = !dr.IsDBNull(dr.GetOrdinal("OBSERVACIONES")) ? dr.GetString(dr.GetOrdinal("OBSERVACIONES")) : "";
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : "";
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : "";
                    ob.FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : "";
                }
            }
            return ob;
        }

        public bool UpdateCamCCGDE(CCGDEDTO ccgdeDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlUpdateCamCCGDE);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, ccgdeDTO.ProyCodi);
            dbProvider.AddInParameter(dbCommand, "ESTUDIOFACTIBILIDAD", DbType.Decimal, ccgdeDTO.Estudiofactibilidad);
            dbProvider.AddInParameter(dbCommand, "INVESTIGACIONESCAMPO", DbType.Decimal, ccgdeDTO.Investigacionescampo);
            dbProvider.AddInParameter(dbCommand, "GESTIONESFINANCIERAS", DbType.Decimal, ccgdeDTO.Gestionesfinancieras);
            dbProvider.AddInParameter(dbCommand, "DISENOSPERMISOS", DbType.Decimal, ccgdeDTO.Disenospermisos);
            dbProvider.AddInParameter(dbCommand, "OBRASCIVILES", DbType.Decimal, ccgdeDTO.Obrasciviles);
            dbProvider.AddInParameter(dbCommand, "EQUIPAMIENTO", DbType.Decimal, ccgdeDTO.Equipamiento);
            dbProvider.AddInParameter(dbCommand, "LINEATRANSMISION", DbType.Decimal, ccgdeDTO.Lineatransmision);
            dbProvider.AddInParameter(dbCommand, "ADMINISTRACION", DbType.Decimal, ccgdeDTO.Administracion);
            dbProvider.AddInParameter(dbCommand, "ADUANAS", DbType.Decimal, ccgdeDTO.Aduanas);
            dbProvider.AddInParameter(dbCommand, "SUPERVISION", DbType.Decimal, ccgdeDTO.Supervision);
            dbProvider.AddInParameter(dbCommand, "GASTOSGESTION", DbType.Decimal, ccgdeDTO.Gastosgestion);
            dbProvider.AddInParameter(dbCommand, "IMPREVISTOS", DbType.Decimal, ccgdeDTO.Imprevistos);
            dbProvider.AddInParameter(dbCommand, "IGV", DbType.Decimal, ccgdeDTO.Igv);
            dbProvider.AddInParameter(dbCommand, "OTROSGASTOS", DbType.Decimal, ccgdeDTO.Otrosgastos);
            dbProvider.AddInParameter(dbCommand, "INVERSIONTOTALSINIGV", DbType.Decimal, ccgdeDTO.Inversiontotalsinigv);
            dbProvider.AddInParameter(dbCommand, "INVERSIONTOTALCONIGV", DbType.Decimal, ccgdeDTO.Inversiontotalconigv);
            dbProvider.AddInParameter(dbCommand, "FINANCIAMIENTOTIPO", DbType.String, ccgdeDTO.Financiamientotipo);
            dbProvider.AddInParameter(dbCommand, "FINANCIAMIENTOESTADO", DbType.String, ccgdeDTO.Financiamientoestado);
            dbProvider.AddInParameter(dbCommand, "PORCENTAJEFINANCIADO", DbType.Decimal, ccgdeDTO.Porcentajefinanciado);
            dbProvider.AddInParameter(dbCommand, "CONCESIONDEFINITIVA", DbType.String, ccgdeDTO.Concesiondefinitiva);
            dbProvider.AddInParameter(dbCommand, "VENTAENERGIA", DbType.String, ccgdeDTO.Ventaenergia);
            dbProvider.AddInParameter(dbCommand, "EJECUCIONOBRA", DbType.String, ccgdeDTO.Ejecucionobra);
            dbProvider.AddInParameter(dbCommand, "CONTRATOSFINANCIEROS", DbType.String, ccgdeDTO.Contratosfinancieros);
            dbProvider.AddInParameter(dbCommand, "OBSERVACIONES", DbType.String, ccgdeDTO.Observaciones);
            dbProvider.AddInParameter(dbCommand, "USU_MODIFICACION", DbType.String, ccgdeDTO.UsuModificacion);
            dbProvider.AddInParameter(dbCommand, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "CCGDECODI", DbType.Int32, ccgdeDTO.CcgdeCodi);
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        public List<CCGDEDTO> GetCamCCGDEByFilter(string plancodi, string empresa, string estado)
        {
            List<CCGDEDTO> oblist = new List<CCGDEDTO>();
            string query = $@"
                SELECT CGB.*, TR.EMPRESANOM, TR.PROYNOMBRE, TR.PROYDESCRIPCION, TP.TIPONOMBRE, TF.TIPOFINOMBRE,TR.PROYCONFIDENCIAL  FROM CAM_CCGDE CGB
                INNER JOIN CAM_TRNSMPROYECTO TR ON TR.PROYCODI = CGB.PROYCODI
                INNER JOIN CAM_PLANTRANSMISION PL ON PL.PLANCODI = TR.PLANCODI
                INNER JOIN CAM_TIPOPROYECTO TP ON TP.TIPOCODI = TR.TIPOCODI
                LEFT JOIN CAM_TIPOFICHAPROYECTO TF ON TF.TIPOFICODI = TR.TIPOFICODI
                WHERE TR.PERICODI  IN ({plancodi}) AND 
                PL.CODEMPRESA IN ({empresa})  AND 
                CGB.IND_DEL = 0 AND 
                PL.PLANESTADO ='{estado}'
                ORDER BY TR.PERICODI, CGB.PROYCODI,PL.CODEMPRESA, CGB.CCGDECODI ASC";

            DbCommand command = dbProvider.GetSqlStringCommand(query);


            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CCGDEDTO ob = new CCGDEDTO();
                    ob.CcgdeCodi = !dr.IsDBNull(dr.GetOrdinal("CCGDECODI")) ? dr.GetInt32(dr.GetOrdinal("CCGDECODI")) : 0;
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
                    ob.Financiamientotipo = !dr.IsDBNull(dr.GetOrdinal("FINANCIAMIENTOTIPO")) ? dr.GetString(dr.GetOrdinal("FINANCIAMIENTOTIPO")) : "";
                    ob.Financiamientoestado = !dr.IsDBNull(dr.GetOrdinal("FINANCIAMIENTOESTADO")) ? dr.GetString(dr.GetOrdinal("FINANCIAMIENTOESTADO")) : "";
                    ob.Porcentajefinanciado = !dr.IsDBNull(dr.GetOrdinal("PORCENTAJEFINANCIADO")) ? dr.GetDecimal(dr.GetOrdinal("PORCENTAJEFINANCIADO")) : 0;
                    ob.Concesiondefinitiva = !dr.IsDBNull(dr.GetOrdinal("CONCESIONDEFINITIVA")) ? dr.GetString(dr.GetOrdinal("CONCESIONDEFINITIVA")) : "";
                    ob.Ventaenergia = !dr.IsDBNull(dr.GetOrdinal("VENTAENERGIA")) ? dr.GetString(dr.GetOrdinal("VENTAENERGIA")) : "";
                    ob.Ejecucionobra = !dr.IsDBNull(dr.GetOrdinal("EJECUCIONOBRA")) ? dr.GetString(dr.GetOrdinal("EJECUCIONOBRA")) : "";
                    ob.Contratosfinancieros = !dr.IsDBNull(dr.GetOrdinal("CONTRATOSFINANCIEROS")) ? dr.GetString(dr.GetOrdinal("CONTRATOSFINANCIEROS")) : "";
                    ob.Observaciones = !dr.IsDBNull(dr.GetOrdinal("OBSERVACIONES")) ? dr.GetString(dr.GetOrdinal("OBSERVACIONES")) : "";
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : "";
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : "";
                    ob.FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : "";
                    ob.Empresa = !dr.IsDBNull(dr.GetOrdinal("EMPRESANOM")) ? dr.GetString(dr.GetOrdinal("EMPRESANOM")) : string.Empty;
                    ob.NombreProyecto = !dr.IsDBNull(dr.GetOrdinal("PROYNOMBRE")) ? dr.GetString(dr.GetOrdinal("PROYNOMBRE")) : string.Empty;
                    ob.DetalleProyecto = !dr.IsDBNull(dr.GetOrdinal("PROYDESCRIPCION")) ? dr.GetString(dr.GetOrdinal("PROYDESCRIPCION")) : string.Empty;
                    ob.TipoProyecto = !dr.IsDBNull(dr.GetOrdinal("TIPONOMBRE")) ? dr.GetString(dr.GetOrdinal("TIPONOMBRE")) : string.Empty;
                    ob.SubTipoProyecto = !dr.IsDBNull(dr.GetOrdinal("TIPOFINOMBRE")) ? dr.GetString(dr.GetOrdinal("TIPOFINOMBRE")) : string.Empty;
                    ob.Confidencial = dr.IsDBNull(dr.GetOrdinal("PROYCONFIDENCIAL")) ? null : dr.GetString(dr.GetOrdinal("PROYCONFIDENCIAL"));
                    oblist.Add(ob);
                }
            }
            return oblist;
        }
    }

}
