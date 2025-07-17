using COES.Base.Core;
using COES.Dominio.DTO.Campania;
using COES.Dominio.Interfaces.Campania;
using COES.Infraestructura.Datos.Helper;
using COES.Infraestructura.Datos.Helper.Campania;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Campania
{
    public class CamCuestionarioH2VFRepository : RepositoryBase, ICamCuestionarioH2VFRepository
    {
        public CamCuestionarioH2VFRepository(string strConn) : base(strConn) { }

        CamCuestionarioH2VFHelper Helper = new CamCuestionarioH2VFHelper();

        public List<CuestionarioH2VFDTO> GetCuestionarioH2VFCodi(int proyCodi)
        {
            List<CuestionarioH2VFDTO> cuestionarios = new List<CuestionarioH2VFDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetCuestionarioH2VFCodi);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, proyCodi);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CuestionarioH2VFDTO cuestionario = new CuestionarioH2VFDTO();

                    cuestionario.H2vFCodi = !dr.IsDBNull(dr.GetOrdinal("H2VFCODI")) ? dr.GetInt32(dr.GetOrdinal("H2VFCODI")) : 0;
                    cuestionario.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    cuestionario.EstudioFactibilidad = !dr.IsDBNull(dr.GetOrdinal("ESTUDIOFACTIBILIDAD")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("ESTUDIOFACTIBILIDAD")) : null;
                    cuestionario.InvestigacionesCampo = !dr.IsDBNull(dr.GetOrdinal("INVESTIGACIONESCAMPO")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("INVESTIGACIONESCAMPO")) : null;
                    cuestionario.GestionesFinancieras = !dr.IsDBNull(dr.GetOrdinal("GESTIONESFINANCIERAS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("GESTIONESFINANCIERAS")) : null;
                    cuestionario.DisenosPermisos = !dr.IsDBNull(dr.GetOrdinal("DISENOSPERMISOS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("DISENOSPERMISOS")) : null;
                    cuestionario.ObrasCiviles = !dr.IsDBNull(dr.GetOrdinal("OBRASCIVILES")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("OBRASCIVILES")) : null;
                    cuestionario.Equipamiento = !dr.IsDBNull(dr.GetOrdinal("EQUIPAMIENTO")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("EQUIPAMIENTO")) : null;
                    cuestionario.LineaTransmision = !dr.IsDBNull(dr.GetOrdinal("LINEATRANSMISION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("LINEATRANSMISION")) : null;
                    cuestionario.Administracion = !dr.IsDBNull(dr.GetOrdinal("ADMINISTRACION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("ADMINISTRACION")) : null;
                    cuestionario.Aduanas = !dr.IsDBNull(dr.GetOrdinal("ADUANAS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("ADUANAS")) : null;
                    cuestionario.Supervision = !dr.IsDBNull(dr.GetOrdinal("SUPERVISION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("SUPERVISION")) : null;
                    cuestionario.GastosGestion = !dr.IsDBNull(dr.GetOrdinal("GASTOSGESTION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("GASTOSGESTION")) : null;
                    cuestionario.Imprevistos = !dr.IsDBNull(dr.GetOrdinal("IMPREVISTOS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("IMPREVISTOS")) : null;
                    cuestionario.Igv = !dr.IsDBNull(dr.GetOrdinal("IGV")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("IGV")) : null;
                    cuestionario.OtrosGastos = !dr.IsDBNull(dr.GetOrdinal("OTROSGASTOS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("OTROSGASTOS")) : null;
                    cuestionario.InversionTotalSinIgv = !dr.IsDBNull(dr.GetOrdinal("INVERSIONTOTALSINIGV")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("INVERSIONTOTALSINIGV")) : null;
                    cuestionario.InversionTotalConIgv = !dr.IsDBNull(dr.GetOrdinal("INVERSIONTOTALCONIGV")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("INVERSIONTOTALCONIGV")) : null;
                    cuestionario.FinanciamientoTipo = !dr.IsDBNull(dr.GetOrdinal("FINANCIAMIENTOTIPO")) ? dr.GetString(dr.GetOrdinal("FINANCIAMIENTOTIPO")) : string.Empty;
                    cuestionario.FinanciamientoEstado = !dr.IsDBNull(dr.GetOrdinal("FINANCIAMIENTOESTADO")) ? dr.GetString(dr.GetOrdinal("FINANCIAMIENTOESTADO")) : string.Empty;
                    cuestionario.PorcentajeFinanciado = !dr.IsDBNull(dr.GetOrdinal("PORCENTAJEFINANCIADO")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("PORCENTAJEFINANCIADO")) : null;
                    cuestionario.ConcesionDefinitiva = !dr.IsDBNull(dr.GetOrdinal("CONCESIONDEFINITIVA")) ? dr.GetString(dr.GetOrdinal("CONCESIONDEFINITIVA")) : string.Empty;
                    cuestionario.VentaEnergia = !dr.IsDBNull(dr.GetOrdinal("VENTAENERGIA")) ? dr.GetString(dr.GetOrdinal("VENTAENERGIA")) : string.Empty;
                    cuestionario.EjecucionObra = !dr.IsDBNull(dr.GetOrdinal("EJECUCIONOBRA")) ? dr.GetString(dr.GetOrdinal("EJECUCIONOBRA")) : string.Empty;
                    cuestionario.ContratosFinancieros = !dr.IsDBNull(dr.GetOrdinal("CONTRATOSFINANCIEROS")) ? dr.GetString(dr.GetOrdinal("CONTRATOSFINANCIEROS")) : string.Empty;
                    cuestionario.Observaciones = !dr.IsDBNull(dr.GetOrdinal("OBSERVACIONES")) ? dr.GetString(dr.GetOrdinal("OBSERVACIONES")) : string.Empty;
                    cuestionario.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : string.Empty;
                    cuestionario.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    cuestionario.UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : string.Empty;
                    cuestionario.FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    cuestionario.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;

                    cuestionarios.Add(cuestionario);
                }

            }
            return cuestionarios;
        }

        public CuestionarioH2VFDTO GetCuestionarioH2VFById(int h2vfCodi)
        {
            CuestionarioH2VFDTO cuestionario = new CuestionarioH2VFDTO();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetCuestionarioH2VFById);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, h2vfCodi);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    cuestionario = new CuestionarioH2VFDTO
                    {
                        H2vFCodi = !dr.IsDBNull(dr.GetOrdinal("H2VFCODI")) ? dr.GetInt32(dr.GetOrdinal("H2VFCODI")) : 0,
                        ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0,
                        EstudioFactibilidad = !dr.IsDBNull(dr.GetOrdinal("ESTUDIOFACTIBILIDAD")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("ESTUDIOFACTIBILIDAD")) : null,
                        InvestigacionesCampo = !dr.IsDBNull(dr.GetOrdinal("INVESTIGACIONESCAMPO")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("INVESTIGACIONESCAMPO")) : null,
                        GestionesFinancieras = !dr.IsDBNull(dr.GetOrdinal("GESTIONESFINANCIERAS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("GESTIONESFINANCIERAS")) : null,
                        DisenosPermisos = !dr.IsDBNull(dr.GetOrdinal("DISENOSPERMISOS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("DISENOSPERMISOS")) : null,
                        ObrasCiviles = !dr.IsDBNull(dr.GetOrdinal("OBRASCIVILES")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("OBRASCIVILES")) : null,
                        Equipamiento = !dr.IsDBNull(dr.GetOrdinal("EQUIPAMIENTO")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("EQUIPAMIENTO")) : null,
                        LineaTransmision = !dr.IsDBNull(dr.GetOrdinal("LINEATRANSMISION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("LINEATRANSMISION")) : null,
                        Administracion = !dr.IsDBNull(dr.GetOrdinal("ADMINISTRACION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("ADMINISTRACION")) : null,
                        Aduanas = !dr.IsDBNull(dr.GetOrdinal("ADUANAS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("ADUANAS")) : null,
                        Supervision = !dr.IsDBNull(dr.GetOrdinal("SUPERVISION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("SUPERVISION")) : null,
                        GastosGestion = !dr.IsDBNull(dr.GetOrdinal("GASTOSGESTION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("GASTOSGESTION")) : null,
                        Imprevistos = !dr.IsDBNull(dr.GetOrdinal("IMPREVISTOS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("IMPREVISTOS")) : null,
                        Igv = !dr.IsDBNull(dr.GetOrdinal("IGV")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("IGV")) : null,
                        OtrosGastos = !dr.IsDBNull(dr.GetOrdinal("OTROSGASTOS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("OTROSGASTOS")) : null,
                        InversionTotalSinIgv = !dr.IsDBNull(dr.GetOrdinal("INVERSIONTOTALSINIGV")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("INVERSIONTOTALSINIGV")) : null,
                        InversionTotalConIgv = !dr.IsDBNull(dr.GetOrdinal("INVERSIONTOTALCONIGV")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("INVERSIONTOTALCONIGV")) : null,
                        FinanciamientoTipo = !dr.IsDBNull(dr.GetOrdinal("FINANCIAMIENTOTIPO")) ? dr.GetString(dr.GetOrdinal("FINANCIAMIENTOTIPO")) : string.Empty,
                        FinanciamientoEstado = !dr.IsDBNull(dr.GetOrdinal("FINANCIAMIENTOESTADO")) ? dr.GetString(dr.GetOrdinal("FINANCIAMIENTOESTADO")) : string.Empty,
                        PorcentajeFinanciado = !dr.IsDBNull(dr.GetOrdinal("PORCENTAJEFINANCIADO")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("PORCENTAJEFINANCIADO")) : null,
                        ConcesionDefinitiva = !dr.IsDBNull(dr.GetOrdinal("CONCESIONDEFINITIVA")) ? dr.GetString(dr.GetOrdinal("CONCESIONDEFINITIVA")) : string.Empty,
                        VentaEnergia = !dr.IsDBNull(dr.GetOrdinal("VENTAENERGIA")) ? dr.GetString(dr.GetOrdinal("VENTAENERGIA")) : string.Empty,
                        EjecucionObra = !dr.IsDBNull(dr.GetOrdinal("EJECUCIONOBRA")) ? dr.GetString(dr.GetOrdinal("EJECUCIONOBRA")) : string.Empty,
                        ContratosFinancieros = !dr.IsDBNull(dr.GetOrdinal("CONTRATOSFINANCIEROS")) ? dr.GetString(dr.GetOrdinal("CONTRATOSFINANCIEROS")) : string.Empty,
                        Observaciones = !dr.IsDBNull(dr.GetOrdinal("OBSERVACIONES")) ? dr.GetString(dr.GetOrdinal("OBSERVACIONES")) : string.Empty,
                        UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : string.Empty,
                        FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue,
                        UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : string.Empty,
                        FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue,
                        IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty
                    };

                }
            }
            return cuestionario;
        }

        public int GetLastCuestionarioH2VFId()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastCuestionarioH2VFId);
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
        public bool SaveCuestionarioH2VF(CuestionarioH2VFDTO cuestionario)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveCuestionarioH2VF);

            dbProvider.AddInParameter(dbCommand, "H2VFCODI", DbType.Int32, cuestionario.H2vFCodi);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, cuestionario.ProyCodi);
            dbProvider.AddInParameter(dbCommand, "ESTUDIOFACTIBILIDAD", DbType.Decimal, cuestionario.EstudioFactibilidad ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "INVESTIGACIONESCAMPO", DbType.Decimal, cuestionario.InvestigacionesCampo ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "GESTIONESFINANCIERAS", DbType.Decimal, cuestionario.GestionesFinancieras ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "DISENOSPERMISOS", DbType.Decimal, cuestionario.DisenosPermisos ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "OBRASCIVILES", DbType.Decimal, cuestionario.ObrasCiviles ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "EQUIPAMIENTO", DbType.Decimal, cuestionario.Equipamiento ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "LINEATRANSMISION", DbType.Decimal, cuestionario.LineaTransmision ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "ADMINISTRACION", DbType.Decimal, cuestionario.Administracion ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "ADUANAS", DbType.Decimal, cuestionario.Aduanas ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "SUPERVISION", DbType.Decimal, cuestionario.Supervision ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "GASTOSGESTION", DbType.Decimal, cuestionario.GastosGestion ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "IMPREVISTOS", DbType.Decimal, cuestionario.Imprevistos ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "IGV", DbType.Decimal, cuestionario.Igv ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "OTROSGASTOS", DbType.Decimal, cuestionario.OtrosGastos ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "INVERSIONTOTALSINIGV", DbType.Decimal, cuestionario.InversionTotalSinIgv ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "INVERSIONTOTALCONIGV", DbType.Decimal, cuestionario.InversionTotalConIgv ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "FINANCIAMIENTOTIPO", DbType.String, cuestionario.FinanciamientoTipo ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "FINANCIAMIENTOESTADO", DbType.String, cuestionario.FinanciamientoEstado ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "PORCENTAJEFINANCIADO", DbType.Decimal, cuestionario.PorcentajeFinanciado ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "CONCESIONDEFINITIVA", DbType.String, cuestionario.ConcesionDefinitiva ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "VENTAENERGIA", DbType.String, cuestionario.VentaEnergia ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "EJECUCIONOBRA", DbType.String, cuestionario.EjecucionObra ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "CONTRATOSFINANCIEROS", DbType.String, cuestionario.ContratosFinancieros ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "OBSERVACIONES", DbType.String, cuestionario.Observaciones ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, cuestionario.UsuCreacion ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, cuestionario.FecCreacion != DateTime.MinValue ? cuestionario.FecCreacion : (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, cuestionario.IndDel ?? (object)DBNull.Value);

            try
            {
                dbProvider.ExecuteNonQuery(dbCommand);
                return true;
            }
            catch (Exception ex)
            {
                // Log exception
                return false;
            }
        }

        public bool DeleteCuestionarioH2VFById(int h2vfCodi, string usuario)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlDeleteCuestionarioH2VFById);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(dbCommand, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(dbCommand, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, h2vfCodi);
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }


        public List<CuestionarioH2VFDTO> GetFormatoH2VFByFilter(string plancodi, string empresa, string estado)
        {
            List<CuestionarioH2VFDTO> oblist = new List<CuestionarioH2VFDTO>();

            string query = $@"
        SELECT CGB.*, TR.EMPRESANOM, TR.PROYNOMBRE, TR.PROYDESCRIPCION, TP.TIPONOMBRE, TF.TIPOFINOMBRE,TR.PROYCONFIDENCIAL  
        FROM cam_cuestionarioh2vf CGB
        INNER JOIN CAM_TRNSMPROYECTO TR ON TR.PROYCODI = CGB.PROYCODI
        INNER JOIN CAM_PLANTRANSMISION PL ON PL.PLANCODI = TR.PLANCODI
        INNER JOIN CAM_TIPOPROYECTO TP ON TP.TIPOCODI = TR.TIPOCODI
        LEFT JOIN CAM_TIPOFICHAPROYECTO TF ON TF.TIPOFICODI = TR.TIPOFICODI
        WHERE TR.PERICODI IN ({plancodi})
          AND PL.CODEMPRESA IN ({empresa})
          AND CGB.IND_DEL = 0
          AND PL.PLANESTADO = '{estado}'
        ORDER BY TR.PERICODI, CGB.PROYCODI, PL.CODEMPRESA, CGB.H2VFCODI ASC";

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CuestionarioH2VFDTO ob = new CuestionarioH2VFDTO();

                    ob.H2vFCodi = !dr.IsDBNull(dr.GetOrdinal("H2VFCODI")) ? dr.GetInt32(dr.GetOrdinal("H2VFCODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.EstudioFactibilidad = !dr.IsDBNull(dr.GetOrdinal("ESTUDIOFACTIBILIDAD")) ? dr.GetDecimal(dr.GetOrdinal("ESTUDIOFACTIBILIDAD")) : (decimal?)null;
                    ob.InvestigacionesCampo = !dr.IsDBNull(dr.GetOrdinal("INVESTIGACIONESCAMPO")) ? dr.GetDecimal(dr.GetOrdinal("INVESTIGACIONESCAMPO")) : (decimal?)null;
                    ob.GestionesFinancieras = !dr.IsDBNull(dr.GetOrdinal("GESTIONESFINANCIERAS")) ? dr.GetDecimal(dr.GetOrdinal("GESTIONESFINANCIERAS")) : (decimal?)null;
                    ob.DisenosPermisos = !dr.IsDBNull(dr.GetOrdinal("DISENOSPERMISOS")) ? dr.GetDecimal(dr.GetOrdinal("DISENOSPERMISOS")) : (decimal?)null;
                    ob.ObrasCiviles = !dr.IsDBNull(dr.GetOrdinal("OBRASCIVILES")) ? dr.GetDecimal(dr.GetOrdinal("OBRASCIVILES")) : (decimal?)null;
                    ob.Equipamiento = !dr.IsDBNull(dr.GetOrdinal("EQUIPAMIENTO")) ? dr.GetDecimal(dr.GetOrdinal("EQUIPAMIENTO")) : (decimal?)null;
                    ob.LineaTransmision = !dr.IsDBNull(dr.GetOrdinal("LINEATRANSMISION")) ? dr.GetDecimal(dr.GetOrdinal("LINEATRANSMISION")) : (decimal?)null;
                    ob.Administracion = !dr.IsDBNull(dr.GetOrdinal("ADMINISTRACION")) ? dr.GetDecimal(dr.GetOrdinal("ADMINISTRACION")) : (decimal?)null;
                    ob.Aduanas = !dr.IsDBNull(dr.GetOrdinal("ADUANAS")) ? dr.GetDecimal(dr.GetOrdinal("ADUANAS")) : (decimal?)null;
                    ob.Supervision = !dr.IsDBNull(dr.GetOrdinal("SUPERVISION")) ? dr.GetDecimal(dr.GetOrdinal("SUPERVISION")) : (decimal?)null;
                    ob.GastosGestion = !dr.IsDBNull(dr.GetOrdinal("GASTOSGESTION")) ? dr.GetDecimal(dr.GetOrdinal("GASTOSGESTION")) : (decimal?)null;
                    ob.Imprevistos = !dr.IsDBNull(dr.GetOrdinal("IMPREVISTOS")) ? dr.GetDecimal(dr.GetOrdinal("IMPREVISTOS")) : (decimal?)null;
                    ob.Igv = !dr.IsDBNull(dr.GetOrdinal("IGV")) ? dr.GetDecimal(dr.GetOrdinal("IGV")) : (decimal?)null;
                    ob.OtrosGastos = !dr.IsDBNull(dr.GetOrdinal("OTROSGASTOS")) ? dr.GetDecimal(dr.GetOrdinal("OTROSGASTOS")) : (decimal?)null;
                    ob.InversionTotalSinIgv = !dr.IsDBNull(dr.GetOrdinal("INVERSIONTOTALSINIGV")) ? dr.GetDecimal(dr.GetOrdinal("INVERSIONTOTALSINIGV")) : (decimal?)null;
                    ob.InversionTotalConIgv = !dr.IsDBNull(dr.GetOrdinal("INVERSIONTOTALCONIGV")) ? dr.GetDecimal(dr.GetOrdinal("INVERSIONTOTALCONIGV")) : (decimal?)null;
                    ob.FinanciamientoTipo = !dr.IsDBNull(dr.GetOrdinal("FINANCIAMIENTOTIPO")) ? dr.GetString(dr.GetOrdinal("FINANCIAMIENTOTIPO")) : "";
                    ob.FinanciamientoEstado = !dr.IsDBNull(dr.GetOrdinal("FINANCIAMIENTOESTADO")) ? dr.GetString(dr.GetOrdinal("FINANCIAMIENTOESTADO")) : "";
                    ob.PorcentajeFinanciado = !dr.IsDBNull(dr.GetOrdinal("PORCENTAJEFINANCIADO")) ? dr.GetDecimal(dr.GetOrdinal("PORCENTAJEFINANCIADO")) : (decimal?)null;
                    ob.ConcesionDefinitiva = !dr.IsDBNull(dr.GetOrdinal("CONCESIONDEFINITIVA")) ? dr.GetString(dr.GetOrdinal("CONCESIONDEFINITIVA")) : "";
                    ob.VentaEnergia = !dr.IsDBNull(dr.GetOrdinal("VENTAENERGIA")) ? dr.GetString(dr.GetOrdinal("VENTAENERGIA")) : "";
                    ob.EjecucionObra = !dr.IsDBNull(dr.GetOrdinal("EJECUCIONOBRA")) ? dr.GetString(dr.GetOrdinal("EJECUCIONOBRA")) : "";
                    ob.ContratosFinancieros = !dr.IsDBNull(dr.GetOrdinal("CONTRATOSFINANCIEROS")) ? dr.GetString(dr.GetOrdinal("CONTRATOSFINANCIEROS")) : "";
                    ob.Observaciones = !dr.IsDBNull(dr.GetOrdinal("OBSERVACIONES")) ? dr.GetString(dr.GetOrdinal("OBSERVACIONES")) : "";
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : "";
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : "";
                    ob.FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : "";
                    ob.Empresa = !dr.IsDBNull(dr.GetOrdinal("EMPRESANOM")) ? dr.GetString(dr.GetOrdinal("EMPRESANOM")) : "";
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
