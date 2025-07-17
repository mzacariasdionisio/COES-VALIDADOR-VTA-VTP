using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Scada;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;
using System.IO;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    public class EventoRepository : RepositoryBase
    {

        public EventoRepository(string strConn)
            : base(strConn)
        {
        }

        EventoHelper helper = new EventoHelper();

        public List<EventoDTO> BuscarEventosPortal(int? idFallaCier, DateTime fechaInicio, DateTime fechaFin,
           string version, string turno, int? idTipoEmpresa, int? idEmpresa, int? idTipoEquipo,
            string indInterrupcion, int nroPagina, int nroFilas)
        {
            String query = String.Format(helper.SqlGetByCriteriaPortal, idFallaCier, idEmpresa, idTipoEquipo, version,
                fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), nroPagina,
                nroFilas, turno, idTipoEmpresa, indInterrupcion);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<EventoDTO> entitys = new List<EventoDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EventoDTO entity = helper.Create(dr);

                    int iFAMNOMB = dr.GetOrdinal(helper.FAMNOMB);
                    if (!dr.IsDBNull(iFAMNOMB)) entity.FAMNOMB = dr.GetString(iFAMNOMB);

                    int iENERGIAINTERRUMPIDA = dr.GetOrdinal(helper.ENERGIAINTERRUMPIDA);
                    if (!dr.IsDBNull(iENERGIAINTERRUMPIDA)) entity.ENERGIAINTERRUMPIDA = dr.GetDecimal(iENERGIAINTERRUMPIDA);

                    int iINTERRUPCIONMW = dr.GetOrdinal(helper.INTERRUPCIONMW);
                    if (!dr.IsDBNull(iINTERRUPCIONMW)) entity.INTERRUPCIONMW = dr.GetDecimal(iINTERRUPCIONMW);

                    int iDISMINUCIONMW = dr.GetOrdinal(helper.DISMINUCIONMW);
                    if (!dr.IsDBNull(iDISMINUCIONMW)) entity.DISMINUCIONMW = dr.GetDecimal(iDISMINUCIONMW);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ObtenerNroRegistrosPortal(int? idTipoEvento, DateTime fechaInicio, DateTime fechaFin,
           string version, string turno, int? idTipoEmpresa, int? idEmpresa, int? idTipoEquipo, string indInterrupcion)
        {

            String query = String.Format(helper.SqlTotalRecordsPortal, idTipoEvento, idEmpresa, idTipoEquipo, version,
                    fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha),
                    turno, idTipoEmpresa, indInterrupcion);

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);
            return 0;
        }

        public List<AnalisisFallaDTO> ObtenerAnalisisFallaCompleto(DateTime fecha)
        {
            List<AnalisisFallaDTO> entitys = new List<AnalisisFallaDTO>();
            try
            {
                string query = string.Format(helper.SqlObtenerAnalisisFallaCompleto, fecha.ToString(ConstantesBase.FormatoFecha));
                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        AnalisisFallaDTO entity = new AnalisisFallaDTO();


                        int iAFECODI = dr.GetOrdinal("AFECODI");
                        int iEVENCODI = dr.GetOrdinal("EVENCODI");
                        int iAFEANIO = dr.GetOrdinal("AFEANIO");
                        int iAFECORR = dr.GetOrdinal("AFECORR");
                        int iAFERMC = dr.GetOrdinal("AFERMC");
                        int iAFEERACMF = dr.GetOrdinal("AFEERACMF");
                        int iAFERACMT = dr.GetOrdinal("AFERACMT");
                        int iAFEEDAGSF = dr.GetOrdinal("AFEEDAGSF");
                        int iAFECITFECHANOMINAL = dr.GetOrdinal("AFECITFECHANOMINAL");
                        int iAFECITFECHAELAB = dr.GetOrdinal("AFECITFECHAELAB");
                        int iAFEREUFECHANOMINAL = dr.GetOrdinal("AFEREUFECHANOMINAL");
                        int iAFEREUFECHAPROG = dr.GetOrdinal("AFEREUFECHAPROG");
                        int iAFEREUHORAPROG = dr.GetOrdinal("AFEREUHORAPROG");
                        int IAFECONVTIPOREUNION = dr.GetOrdinal("AFECONVTIPOREUNION");
                        int IAevenasunto = dr.GetOrdinal("EVENASUNTO");
                        int ICODIGO = dr.GetOrdinal("CODIGO");
                        int IFECHA_EVENTO = dr.GetOrdinal("FECHA_EVENTO");
                        int IEVENINI = dr.GetOrdinal("EVENINI");

                        if (!dr.IsDBNull(iAFECODI)) entity.AFECODI = dr.GetInt32(iAFECODI);
                        if (!dr.IsDBNull(iEVENCODI)) entity.EVENCODI = Convert.ToInt32(dr.GetDecimal(iEVENCODI));
                        if (!dr.IsDBNull(iAFEANIO)) entity.AFEANIO = dr.GetInt32(iAFEANIO);
                        if (!dr.IsDBNull(iAFECORR)) entity.AFECORR = dr.GetInt32(iAFECORR);
                        if (!dr.IsDBNull(iAFERMC)) entity.AFERMC = dr.GetString(iAFERMC);
                        if (!dr.IsDBNull(iAFEERACMF)) entity.AFEERACMF = dr.GetString(iAFEERACMF);
                        if (!dr.IsDBNull(iAFERACMT)) entity.AFERACMT = dr.GetString(iAFERACMT);
                        if (!dr.IsDBNull(iAFEEDAGSF)) entity.AFEEDAGSF = dr.GetString(iAFEEDAGSF);
                        if (!dr.IsDBNull(iAFECITFECHANOMINAL)) entity.AFECITFECHANOMINAL = dr.GetDateTime(iAFECITFECHANOMINAL);
                        if (!dr.IsDBNull(iAFECITFECHAELAB)) entity.AFECITFECHAELAB = dr.GetDateTime(iAFECITFECHAELAB);
                        if (!dr.IsDBNull(iAFEREUFECHANOMINAL)) entity.AFEREUFECHANOMINAL = dr.GetDateTime(iAFEREUFECHANOMINAL);
                        if (!dr.IsDBNull(iAFEREUFECHAPROG)) entity.AFEREUFECHAPROG = dr.GetDateTime(iAFEREUFECHAPROG);
                        if (!dr.IsDBNull(iAFEREUHORAPROG)) entity.AFEREUHORAPROG = dr.GetString(iAFEREUHORAPROG);
                        if (!dr.IsDBNull(IAFECONVTIPOREUNION)) entity.AFECONVTIPOREUNION = dr.GetString(IAFECONVTIPOREUNION);
                        if (!dr.IsDBNull(IAevenasunto)) entity.EVENASUNTO = dr.GetString(IAevenasunto);
                        if (!dr.IsDBNull(ICODIGO)) entity.CODIGO = dr.GetString(ICODIGO);
                        if (!dr.IsDBNull(IFECHA_EVENTO)) entity.FECHA_EVENTO = dr.GetString(IFECHA_EVENTO);
                        if (!dr.IsDBNull(IEVENINI)) entity.EVENINI = dr.GetDateTime(IEVENINI);

                        entitys.Add(entity);
                    }
                }
            }
            catch (Exception ex)
            {
                entitys = null;
            }
            return entitys;
        }

        public List<AnalisisFallaDTO> ObtenerAnalisisFallaCompleto()
        {
            List<AnalisisFallaDTO> entitys = new List<AnalisisFallaDTO>();
            try
            {
                string query = string.Format(helper.SqlObtenerAnalisisFallaCompleto2);
                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        AnalisisFallaDTO entity = new AnalisisFallaDTO();


                        int iAFECODI = dr.GetOrdinal("AFECODI");
                        int iEVENCODI = dr.GetOrdinal("EVENCODI");
                        int iAFEANIO = dr.GetOrdinal("AFEANIO");
                        int iAFECORR = dr.GetOrdinal("AFECORR");
                        int iAFERMC = dr.GetOrdinal("AFERMC");
                        int iAFEERACMF = dr.GetOrdinal("AFEERACMF");
                        int iAFERACMT = dr.GetOrdinal("AFERACMT");
                        int iAFEEDAGSF = dr.GetOrdinal("AFEEDAGSF");
                        int iAFECITFECHANOMINAL = dr.GetOrdinal("AFECITFECHANOMINAL");
                        int iAFECITFECHAELAB = dr.GetOrdinal("AFECITFECHAELAB");
                        int iAFEREUFECHANOMINAL = dr.GetOrdinal("AFEREUFECHANOMINAL");
                        int iAFEREUFECHAPROG = dr.GetOrdinal("AFEREUFECHAPROG");
                        int iAFEREUHORAPROG = dr.GetOrdinal("AFEREUHORAPROG");
                        int IAFECONVTIPOREUNION = dr.GetOrdinal("AFECONVTIPOREUNION");
                        int IAevenasunto = dr.GetOrdinal("EVENASUNTO");
                        int ICODIGO = dr.GetOrdinal("CODIGO");
                        int IFECHA_EVENTO = dr.GetOrdinal("FECHA_EVENTO");

                        if (!dr.IsDBNull(iAFECODI)) entity.AFECODI = dr.GetInt32(iAFECODI);
                        if (!dr.IsDBNull(iEVENCODI)) entity.EVENCODI = Convert.ToInt32(dr.GetDecimal(iEVENCODI));
                        if (!dr.IsDBNull(iAFEANIO)) entity.AFEANIO = dr.GetInt32(iAFEANIO);
                        if (!dr.IsDBNull(iAFECORR)) entity.AFECORR = dr.GetInt32(iAFECORR);
                        if (!dr.IsDBNull(iAFERMC)) entity.AFERMC = dr.GetString(iAFERMC);
                        if (!dr.IsDBNull(iAFEERACMF)) entity.AFEERACMF = dr.GetString(iAFEERACMF);
                        if (!dr.IsDBNull(iAFERACMT)) entity.AFERACMT = dr.GetString(iAFERACMT);
                        if (!dr.IsDBNull(iAFEEDAGSF)) entity.AFEEDAGSF = dr.GetString(iAFEEDAGSF);
                        if (!dr.IsDBNull(iAFECITFECHANOMINAL)) entity.AFECITFECHANOMINAL = dr.GetDateTime(iAFECITFECHANOMINAL);
                        if (!dr.IsDBNull(iAFECITFECHAELAB)) entity.AFECITFECHAELAB = dr.GetDateTime(iAFECITFECHAELAB);
                        if (!dr.IsDBNull(iAFEREUFECHANOMINAL)) entity.AFEREUFECHANOMINAL = dr.GetDateTime(iAFEREUFECHANOMINAL);
                        if (!dr.IsDBNull(iAFEREUFECHAPROG)) entity.AFEREUFECHAPROG = dr.GetDateTime(iAFEREUFECHAPROG);
                        if (!dr.IsDBNull(iAFEREUHORAPROG)) entity.AFEREUHORAPROG = dr.GetString(iAFEREUHORAPROG);
                        if (!dr.IsDBNull(IAFECONVTIPOREUNION)) entity.AFECONVTIPOREUNION = dr.GetString(IAFECONVTIPOREUNION);
                        if (!dr.IsDBNull(IAevenasunto)) entity.EVENASUNTO = dr.GetString(IAevenasunto);
                        if (!dr.IsDBNull(ICODIGO)) entity.CODIGO = dr.GetString(ICODIGO);
                        if (!dr.IsDBNull(IFECHA_EVENTO)) entity.FECHA_EVENTO = dr.GetString(IFECHA_EVENTO);

                        entitys.Add(entity);
                    }
                }
            }
            catch (Exception ex)
            {
                entitys = null;
            }
            return entitys;
        }

        public AnalisisFallaDTO ObtenerAnalisisFalla2(int id)
        {
            AnalisisFallaDTO entity = new AnalisisFallaDTO();
            try
            {
                string query = string.Format(helper.SqlObtenerAnalisisFalla2, id);
                DbCommand command = dbProvider.GetSqlStringCommand(query);


                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        int iAFECODI = dr.GetOrdinal("AFECODI");
                        int iEVENCODI = dr.GetOrdinal("EVENCODI");
                        int iAFEANIO = dr.GetOrdinal("AFEANIO");
                        int iAFECORR = dr.GetOrdinal("AFECORR");
                        int iAFERMC = dr.GetOrdinal("AFERMC");
                        int iAFEERACMF = dr.GetOrdinal("AFEERACMF");
                        int iAFERACMT = dr.GetOrdinal("AFERACMT");
                        int iAFEEDAGSF = dr.GetOrdinal("AFEEDAGSF");
                        int iAFECITFECHANOMINAL = dr.GetOrdinal("AFECITFECHANOMINAL");
                        int iAFECITFECHAELAB = dr.GetOrdinal("AFECITFECHAELAB");
                        int iAFEREUFECHANOMINAL = dr.GetOrdinal("AFEREUFECHANOMINAL");
                        int iAFEREUFECHAPROG = dr.GetOrdinal("AFEREUFECHAPROG");
                        int iAFEREUHORAPROG = dr.GetOrdinal("AFEREUHORAPROG");
                        int IAFECONVTIPOREUNION = dr.GetOrdinal("AFECONVTIPOREUNION");
                        int IAevenasunto = dr.GetOrdinal("EVENASUNTO");
                        int ICODIGO = dr.GetOrdinal("CODIGO");
                        int IFECHA_EVENTO = dr.GetOrdinal("FECHA_EVENTO");

                        if (!dr.IsDBNull(iAFECODI)) entity.AFECODI = dr.GetInt32(iAFECODI);
                        if (!dr.IsDBNull(iEVENCODI)) entity.EVENCODI = Convert.ToInt32(dr.GetDecimal(iEVENCODI));
                        if (!dr.IsDBNull(iAFEANIO)) entity.AFEANIO = dr.GetInt32(iAFEANIO);
                        if (!dr.IsDBNull(iAFECORR)) entity.AFECORR = dr.GetInt32(iAFECORR);
                        if (!dr.IsDBNull(iAFERMC)) entity.AFERMC = dr.GetString(iAFERMC);
                        if (!dr.IsDBNull(iAFEERACMF)) entity.AFEERACMF = dr.GetString(iAFEERACMF);
                        if (!dr.IsDBNull(iAFERACMT)) entity.AFERACMT = dr.GetString(iAFERACMT);
                        if (!dr.IsDBNull(iAFEEDAGSF)) entity.AFEEDAGSF = dr.GetString(iAFEEDAGSF);
                        if (!dr.IsDBNull(iAFECITFECHANOMINAL)) entity.AFECITFECHANOMINAL = dr.GetDateTime(iAFECITFECHANOMINAL);
                        if (!dr.IsDBNull(iAFECITFECHAELAB)) entity.AFECITFECHAELAB = dr.GetDateTime(iAFECITFECHAELAB);
                        if (!dr.IsDBNull(iAFEREUFECHANOMINAL)) entity.AFEREUFECHANOMINAL = dr.GetDateTime(iAFEREUFECHANOMINAL);
                        if (!dr.IsDBNull(iAFEREUFECHAPROG)) entity.AFEREUFECHAPROG = dr.GetDateTime(iAFEREUFECHAPROG);
                        if (!dr.IsDBNull(iAFEREUHORAPROG)) entity.AFEREUHORAPROG = dr.GetString(iAFEREUHORAPROG);
                        if (!dr.IsDBNull(IAFECONVTIPOREUNION)) entity.AFECONVTIPOREUNION = dr.GetString(IAFECONVTIPOREUNION);
                        if (!dr.IsDBNull(IAevenasunto)) entity.EVENASUNTO = dr.GetString(IAevenasunto);
                        if (!dr.IsDBNull(ICODIGO)) entity.CODIGO = dr.GetString(ICODIGO);
                        if (!dr.IsDBNull(IFECHA_EVENTO)) entity.FECHA_EVENTO = dr.GetString(IFECHA_EVENTO);

                    }
                }
            }
            catch (Exception ex)
            {
                entity = null;
            }
            return entity;
        }


        public List<EventoDTO> ExportarEventosPortal(int? idFallaCier, DateTime fechaInicio, DateTime fechaFin,
           string version, string turno, int? idTipoEmpresa, int? idEmpresa, int? idTipoEquipo,
            string indInterrupcion)
        {
            String query = String.Format(helper.SqlExportarEventosPortal, idFallaCier, idEmpresa, idTipoEquipo, version,
                fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha),
                turno, idTipoEmpresa, indInterrupcion);

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<EventoDTO> entitys = new List<EventoDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EventoDTO entity = new EventoDTO();

                    int iEMPRNOMB = dr.GetOrdinal(helper.EMPRNOMB);
                    if (!dr.IsDBNull(iEMPRNOMB)) entity.EMPRNOMB = dr.GetString(iEMPRNOMB);

                    int iEQUIABREV = dr.GetOrdinal(helper.EQUIABREV);
                    if (!dr.IsDBNull(iEQUIABREV)) entity.EQUIABREV = dr.GetString(iEQUIABREV);

                    int iTAREAABREV = dr.GetOrdinal(helper.TAREAABREV);
                    if (!dr.IsDBNull(iTAREAABREV)) entity.TAREAABREV = dr.GetString(iTAREAABREV);

                    int iAREANOMB = dr.GetOrdinal(helper.AREANOMB);
                    if (!dr.IsDBNull(iAREANOMB)) entity.AREANOMB = dr.GetString(iAREANOMB);

                    int iEVENINI = dr.GetOrdinal(helper.EVENINI);
                    if (!dr.IsDBNull(iEVENINI)) entity.EVENINI = dr.GetDateTime(iEVENINI);

                    int iEVENFIN = dr.GetOrdinal(helper.EVENFIN);
                    if (!dr.IsDBNull(iEVENFIN)) entity.EVENFIN = dr.GetDateTime(iEVENFIN);

                    int iEVENASUNTO = dr.GetOrdinal(helper.EVENASUNTO);
                    if (!dr.IsDBNull(iEVENASUNTO)) entity.EVENASUNTO = dr.GetString(iEVENASUNTO);

                    int iINTERRUPCIONMW = dr.GetOrdinal(helper.INTERRUPCIONMW);
                    if (!dr.IsDBNull(iINTERRUPCIONMW)) entity.INTERRUPCIONMW = dr.GetDecimal(iINTERRUPCIONMW);

                    int iCAUSAEVENABREV = dr.GetOrdinal(helper.CAUSAEVENABREV);
                    if (!dr.IsDBNull(iCAUSAEVENABREV)) entity.CAUSAEVENABREV = dr.GetString(iCAUSAEVENABREV);

                    int iENERGIAINTERRUMPIDA = dr.GetOrdinal(helper.ENERGIAINTERRUMPIDA);
                    if (!dr.IsDBNull(iENERGIAINTERRUMPIDA)) entity.ENERGIAINTERRUMPIDA = dr.GetDecimal(iENERGIAINTERRUMPIDA);

                    int iEQUITENSION = dr.GetOrdinal(helper.EQUITENSION);
                    if (!dr.IsDBNull(iEQUITENSION)) entity.EQUITENSION = dr.GetDecimal(iEQUITENSION);

                    int iFAMNOMB = dr.GetOrdinal(helper.FAMNOMB);
                    if (!dr.IsDBNull(iFAMNOMB)) entity.FAMNOMB = dr.GetString(iFAMNOMB);

                    int iTIPOEMPRDESC = dr.GetOrdinal(helper.TIPOEMPRDESC);
                    if (!dr.IsDBNull(iTIPOEMPRDESC)) entity.TIPOEMPRDESC = dr.GetString(iTIPOEMPRDESC);

                    int iDISMINUCIONMW = dr.GetOrdinal(helper.DISMINUCIONMW);
                    if (!dr.IsDBNull(iDISMINUCIONMW)) entity.DISMINUCIONMW = dr.GetDecimal(iDISMINUCIONMW);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }


        public List<EventoDTO> BuscarEventos(int? idTipoEvento, DateTime fechaInicio, DateTime fechaFin,
           string version, string turno, int? idTipoEmpresa, int? idEmpresa, int? idTipoEquipo,
            string indInterrupcion, int nroPagina, int nroFilas, string campo, string orden, string areaOperativa, int todosaseg)
        {
            String query = String.Format(helper.SqlGetByCriteria, idTipoEvento, idEmpresa, idTipoEquipo, version,
                fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha),
                nroPagina, nroFilas, turno, idTipoEmpresa, indInterrupcion,
                campo, orden, areaOperativa, todosaseg);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<EventoDTO> entitys = new List<EventoDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iFAMNOMB = dr.GetOrdinal(helper.FAMNOMB);
                    if (!dr.IsDBNull(iFAMNOMB)) entity.FAMNOMB = dr.GetString(iFAMNOMB);

                    int iCausaevencodi = dr.GetOrdinal(helper.Causaevencodi);
                    if (!dr.IsDBNull(iCausaevencodi)) entity.Causaevencodi = dr.GetInt32(iCausaevencodi);

                    int iENERGIAINTERRUMPIDA = dr.GetOrdinal(helper.ENERGIAINTERRUMPIDA);
                    if (!dr.IsDBNull(iENERGIAINTERRUMPIDA)) entity.ENERGIAINTERRUMPIDA = dr.GetDecimal(iENERGIAINTERRUMPIDA);

                    #region Mejoras CTAF
                    int iCodigoCtaf = dr.GetOrdinal(helper.CodigoCtaf);
                    if (!dr.IsDBNull(iCodigoCtaf)) entity.CodigoCtaf = dr.GetString(iCodigoCtaf);

                    int iEvenctaf = dr.GetOrdinal(helper.EVENCTAF);
                    if (!dr.IsDBNull(iEvenctaf)) entity.EVENCTAF = dr.GetString(iEvenctaf);
                    #endregion

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EventoDTO> ExportarEventos(string idTipoEvento, DateTime fechaInicio, DateTime fechaFin,
   string version, string turno, int? idTipoEmpresa, string idEmpresa, string idTipoEquipo,
    string indInterrupcion, string areaOperativa)
        {
            String query = String.Format(helper.SqlExportarEventos, idTipoEvento, idEmpresa, idTipoEquipo, version,
                fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha),
                turno, idTipoEmpresa, indInterrupcion, areaOperativa);

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<EventoDTO> entitys = new List<EventoDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EventoDTO entity = new EventoDTO();

                    int iEMPRNOMB = dr.GetOrdinal(helper.EMPRNOMB);
                    if (!dr.IsDBNull(iEMPRNOMB)) entity.EMPRNOMB = dr.GetString(iEMPRNOMB);

                    int iTAREAABREV = dr.GetOrdinal(helper.TAREAABREV);
                    if (!dr.IsDBNull(iTAREAABREV)) entity.TAREAABREV = dr.GetString(iTAREAABREV);

                    int iAREANOMB = dr.GetOrdinal(helper.AREANOMB);
                    if (!dr.IsDBNull(iAREANOMB)) entity.AREANOMB = dr.GetString(iAREANOMB);

                    int iEQUIABREV = dr.GetOrdinal(helper.EQUIABREV);
                    if (!dr.IsDBNull(iEQUIABREV)) entity.EQUIABREV = dr.GetString(iEQUIABREV);

                    int iFamNomb = dr.GetOrdinal(helper.FAMNOMB);
                    if (!dr.IsDBNull(iFamNomb)) entity.FAMNOMB = dr.GetString(iFamNomb);

                    int iFamAbrev = dr.GetOrdinal(helper.FAMABREV);
                    if (!dr.IsDBNull(iFamAbrev)) entity.FAMABREV = dr.GetString(iFamAbrev);

                    int iFamcodi = dr.GetOrdinal(helper.FAMCODI);
                    if (!dr.IsDBNull(iFamcodi)) entity.FAMCODI = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iEVENINI = dr.GetOrdinal(helper.EVENINI);
                    if (!dr.IsDBNull(iEVENINI)) entity.EVENINI = dr.GetDateTime(iEVENINI);

                    int iEVENFIN = dr.GetOrdinal(helper.EVENFIN);
                    if (!dr.IsDBNull(iEVENFIN)) entity.EVENFIN = dr.GetDateTime(iEVENFIN);

                    int iEMPRABREV = dr.GetOrdinal(helper.EMPRABREV);
                    if (!dr.IsDBNull(iEMPRABREV)) entity.EMPRABREV = dr.GetString(iEMPRABREV);

                    int iEVENASUNTO = dr.GetOrdinal(helper.EVENASUNTO);
                    if (!dr.IsDBNull(iEVENASUNTO)) entity.EVENASUNTO = dr.GetString(iEVENASUNTO);

                    int iEVENINTERRUP = dr.GetOrdinal(helper.EVENINTERRUP);
                    if (!dr.IsDBNull(iEVENINTERRUP)) entity.EVENINTERRUP = dr.GetString(iEVENINTERRUP);

                    int iTIPOEVENABREV = dr.GetOrdinal(helper.TIPOEVENABREV);
                    if (!dr.IsDBNull(iTIPOEVENABREV)) entity.TIPOEVENABREV = dr.GetString(iTIPOEVENABREV);

                    int iTIPOEVENCODI = dr.GetOrdinal(helper.TIPOEVENCODI);
                    if (!dr.IsDBNull(iTIPOEVENCODI)) entity.TIPOEVENCODI = Convert.ToInt32(dr.GetValue(iTIPOEVENCODI));

                    int iCAUSAEVENABREV = dr.GetOrdinal(helper.CAUSAEVENABREV);
                    if (!dr.IsDBNull(iCAUSAEVENABREV)) entity.CAUSAEVENABREV = dr.GetString(iCAUSAEVENABREV);

                    int iENERGIAINTERRUMPIDA = dr.GetOrdinal(helper.ENERGIAINTERRUMPIDA);
                    if (!dr.IsDBNull(iENERGIAINTERRUMPIDA)) entity.ENERGIAINTERRUMPIDA = dr.GetDecimal(iENERGIAINTERRUMPIDA);

                    int iTIPOREGISTRO = dr.GetOrdinal(helper.TIPOREGISTRO);
                    if (!dr.IsDBNull(iTIPOREGISTRO)) entity.TIPOREGISTRO = dr.GetString(iTIPOREGISTRO);

                    int iVALTIPOREGISTRO = dr.GetOrdinal(helper.VALTIPOREGISTRO);
                    if (!dr.IsDBNull(iVALTIPOREGISTRO)) entity.VALTIPOREGISTRO = dr.GetString(iVALTIPOREGISTRO);

                    int iEQUITENSION = dr.GetOrdinal(helper.EQUITENSION);
                    if (!dr.IsDBNull(iEQUITENSION)) entity.EQUITENSION = dr.GetDecimal(iEQUITENSION);

                    int iTIPOEMPRNOMB = dr.GetOrdinal(helper.TIPOEMPRNOMB);
                    if (!dr.IsDBNull(iTIPOEMPRNOMB)) entity.TIPOEMPRNOMB = dr.GetString(iTIPOEMPRNOMB);

                    int iEVENPRELIMINAR = dr.GetOrdinal(helper.EVENPRELIMINAR);
                    if (!dr.IsDBNull(iEVENPRELIMINAR)) entity.EVENPRELIMINAR = dr.GetString(iEVENPRELIMINAR);

                    int iSUBCAUSAABREV = dr.GetOrdinal(helper.SUBCAUSAABREV);
                    if (!dr.IsDBNull(iSUBCAUSAABREV)) entity.SUBCAUSAABREV = dr.GetString(iSUBCAUSAABREV);

                    int iEVENMWGENDESCON = dr.GetOrdinal(helper.EVENMWGENDESCON);
                    if (!dr.IsDBNull(iEVENMWGENDESCON)) entity.EVENMWGENDESCON = dr.GetDecimal(iEVENMWGENDESCON);

                    int iEVENGENDESCON = dr.GetOrdinal(helper.EVENGENDESCON);
                    if (!dr.IsDBNull(iEVENGENDESCON)) entity.EVENGENDESCON = dr.GetString(iEVENGENDESCON);

                    int iEMPRCODI = dr.GetOrdinal(helper.EMPRCODI);
                    if (!dr.IsDBNull(iEMPRCODI)) entity.EMPRCODI = Convert.ToInt32(dr.GetValue(iEMPRCODI));

                    int iAREACODI = dr.GetOrdinal(helper.AREACODI);
                    if (!dr.IsDBNull(iAREACODI)) entity.AREACODI = Convert.ToInt32(dr.GetValue(iAREACODI));

                    int iEVENCODI = dr.GetOrdinal(helper.EVENCODI);
                    if (!dr.IsDBNull(iEVENCODI)) entity.EVENCODI = Convert.ToInt32(dr.GetValue(iEVENCODI));

                    int iEVENCOMENTARIOS = dr.GetOrdinal(helper.EVENCOMENTARIOS);
                    if (!dr.IsDBNull(iEVENCOMENTARIOS)) entity.EVENCOMENTARIOS = dr.GetString(iEVENCOMENTARIOS);

                    int iAREADESC = dr.GetOrdinal(helper.AREADESC);
                    if (!dr.IsDBNull(iAREADESC)) entity.AREADESC = dr.GetString(iAREADESC);

                    int iEVENDESC = dr.GetOrdinal(helper.EVENDESC);
                    if (!dr.IsDBNull(iEVENDESC)) entity.EVENDESC = dr.GetString(iEVENDESC);

                    int iMWINTERRUMPIDOS = dr.GetOrdinal(helper.MWINTERRUMPIDOS);
                    if (!dr.IsDBNull(iMWINTERRUMPIDOS)) entity.MWINTERRUMPIDOS = dr.GetDecimal(iMWINTERRUMPIDOS);

                    int iEquiCodi = dr.GetOrdinal(helper.EQUICODI);
                    if (!dr.IsDBNull(iEquiCodi)) entity.EQUICODI = Convert.ToInt16(dr.GetValue(iEquiCodi));

                    //int iINTERRMANUALR = dr.GetOrdinal(helper.Interrmanualr);
                    //if (!dr.IsDBNull(iINTERRMANUALR)) entity.Interrmanualr = dr.GetString(iINTERRMANUALR);

                    //int iINTERRRACMF = dr.GetOrdinal(helper.Interrracmf);
                    //if (!dr.IsDBNull(iINTERRRACMF)) entity.Interrracmf = dr.GetString(iINTERRRACMF);

                    decimal suma = 0;

                    if (entity.TIPOREGISTRO == "D" && entity.EVENPRELIMINAR != "S")
                    {
                        if (!string.IsNullOrEmpty(entity.VALTIPOREGISTRO))
                        {
                            string[] split = entity.VALTIPOREGISTRO.Split(',');

                            if (split.Length == 8)
                            {
                                if (split[0] == "S") suma = suma + decimal.Parse(split[1]);
                                if (split[2] == "S") suma = suma + decimal.Parse(split[3]);
                                if (split[4] == "S") suma = suma + decimal.Parse(split[5]);
                                if (split[6] == "S") suma = suma + decimal.Parse(split[7]);

                                entity.EVENMWINDISP = suma;
                            }
                        }
                    }

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EventoDTO> ExportarEventosDetallado(int? idTipoEvento, DateTime fechaInicio, DateTime fechaFin,
           string version, string turno, int? idTipoEmpresa, int? idEmpresa, int? idTipoEquipo,
            string indInterrupcion, string areaoperativa)
        {
            String query = String.Format(helper.SqlExportarEventosDetallado, idTipoEvento, idEmpresa, idTipoEquipo, version,
                fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha),
                turno, idTipoEmpresa, indInterrupcion, areaoperativa);

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<EventoDTO> entitys = new List<EventoDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EventoDTO entity = new EventoDTO();

                    int iEMPRNOMB = dr.GetOrdinal(helper.EMPRNOMB);
                    if (!dr.IsDBNull(iEMPRNOMB)) entity.EMPRNOMB = dr.GetString(iEMPRNOMB);

                    int iTAREAABREV = dr.GetOrdinal(helper.TAREAABREV);
                    if (!dr.IsDBNull(iTAREAABREV)) entity.TAREAABREV = dr.GetString(iTAREAABREV);

                    int iAREANOMB = dr.GetOrdinal(helper.AREANOMB);
                    if (!dr.IsDBNull(iAREANOMB)) entity.AREANOMB = dr.GetString(iAREANOMB);

                    int iEQUIABREV = dr.GetOrdinal(helper.EQUIABREV);
                    if (!dr.IsDBNull(iEQUIABREV)) entity.EQUIABREV = dr.GetString(iEQUIABREV);

                    int iFamNomb = dr.GetOrdinal(helper.FAMNOMB);
                    if (!dr.IsDBNull(iFamNomb)) entity.FAMNOMB = dr.GetString(iFamNomb);

                    int iFAMABREV = dr.GetOrdinal(helper.FAMABREV);
                    if (!dr.IsDBNull(iFAMABREV)) entity.FAMABREV = dr.GetString(iFAMABREV);

                    int iEVENINI = dr.GetOrdinal(helper.EVENINI);
                    if (!dr.IsDBNull(iEVENINI)) entity.EVENINI = dr.GetDateTime(iEVENINI);

                    int iEVENFIN = dr.GetOrdinal(helper.EVENFIN);
                    if (!dr.IsDBNull(iEVENFIN)) entity.EVENFIN = dr.GetDateTime(iEVENFIN);

                    int iEquiCodi = dr.GetOrdinal(helper.EQUICODI);
                    if (!dr.IsDBNull(iEquiCodi)) entity.EQUICODI = Convert.ToInt16(dr.GetValue(iEquiCodi));

                    int iEquicodiInvolucrado = dr.GetOrdinal(helper.EquicodiInvolucrado);
                    if (!dr.IsDBNull(iEquicodiInvolucrado)) entity.EquicodiInvolucrado = Convert.ToInt16(dr.GetValue(iEquicodiInvolucrado));

                    int iEMPRABREV = dr.GetOrdinal(helper.EMPRABREV);
                    if (!dr.IsDBNull(iEMPRABREV)) entity.EMPRABREV = dr.GetString(iEMPRABREV);

                    int iEVENASUNTO = dr.GetOrdinal(helper.EVENASUNTO);
                    if (!dr.IsDBNull(iEVENASUNTO)) entity.EVENASUNTO = dr.GetString(iEVENASUNTO);

                    int iEVENINTERRUP = dr.GetOrdinal(helper.EVENINTERRUP);
                    if (!dr.IsDBNull(iEVENINTERRUP)) entity.EVENINTERRUP = dr.GetString(iEVENINTERRUP);

                    int iTIPOEVENABREV = dr.GetOrdinal(helper.TIPOEVENABREV);
                    if (!dr.IsDBNull(iTIPOEVENABREV)) entity.TIPOEVENABREV = dr.GetString(iTIPOEVENABREV);

                    int iCAUSAEVENABREV = dr.GetOrdinal(helper.CAUSAEVENABREV);
                    if (!dr.IsDBNull(iCAUSAEVENABREV)) entity.CAUSAEVENABREV = dr.GetString(iCAUSAEVENABREV);

                    int iENERGIAINTERRUMPIDA = dr.GetOrdinal(helper.ENERGIAINTERRUMPIDA);
                    if (!dr.IsDBNull(iENERGIAINTERRUMPIDA)) entity.ENERGIAINTERRUMPIDA = dr.GetDecimal(iENERGIAINTERRUMPIDA);

                    int iTIPOREGISTRO = dr.GetOrdinal(helper.TIPOREGISTRO);
                    if (!dr.IsDBNull(iTIPOREGISTRO)) entity.TIPOREGISTRO = dr.GetString(iTIPOREGISTRO);

                    int iVALTIPOREGISTRO = dr.GetOrdinal(helper.VALTIPOREGISTRO);
                    if (!dr.IsDBNull(iVALTIPOREGISTRO)) entity.VALTIPOREGISTRO = dr.GetString(iVALTIPOREGISTRO);

                    int iEQUITENSION = dr.GetOrdinal(helper.EQUITENSION);
                    if (!dr.IsDBNull(iEQUITENSION)) entity.EQUITENSION = dr.GetDecimal(iEQUITENSION);

                    int iTIPOEMPRNOMB = dr.GetOrdinal(helper.TIPOEMPRNOMB);
                    if (!dr.IsDBNull(iTIPOEMPRNOMB)) entity.TIPOEMPRNOMB = dr.GetString(iTIPOEMPRNOMB);

                    int iEVENPRELIMINAR = dr.GetOrdinal(helper.EVENPRELIMINAR);
                    if (!dr.IsDBNull(iEVENPRELIMINAR)) entity.EVENPRELIMINAR = dr.GetString(iEVENPRELIMINAR);

                    int iEVENDESC = dr.GetOrdinal(helper.EVENDESC);
                    if (!dr.IsDBNull(iEVENDESC)) entity.EVENDESC = dr.GetString(iEVENDESC);

                    int iEVENMWGENDESCON = dr.GetOrdinal(helper.EVENMWGENDESCON);
                    if (!dr.IsDBNull(iEVENMWGENDESCON)) entity.EVENMWGENDESCON = dr.GetDecimal(iEVENMWGENDESCON);

                    int iEVENGENDESCON = dr.GetOrdinal(helper.EVENGENDESCON);
                    if (!dr.IsDBNull(iEVENGENDESCON)) entity.EVENGENDESCON = dr.GetString(iEVENGENDESCON);

                    decimal suma = 0;

                    if (entity.TIPOREGISTRO == "D" && entity.EVENPRELIMINAR != "S")
                    {
                        if (!string.IsNullOrEmpty(entity.VALTIPOREGISTRO))
                        {
                            string[] split = entity.VALTIPOREGISTRO.Split(',');

                            if (split.Length == 8)
                            {
                                if (split[0] == "S") suma = suma + decimal.Parse(split[1]);
                                if (split[2] == "S") suma = suma + decimal.Parse(split[3]);
                                if (split[4] == "S") suma = suma + decimal.Parse(split[5]);
                                if (split[6] == "S") suma = suma + decimal.Parse(split[7]);

                                entity.EVENMWINDISP = suma;
                            }
                        }
                    }


                    int iEventipofalla = dr.GetOrdinal(helper.Eventipofalla);
                    if (!dr.IsDBNull(iEventipofalla)) entity.Eventipofalla = dr.GetString(iEventipofalla);

                    int iEventipofallafase = dr.GetOrdinal(helper.Eventipofallafase);
                    if (!dr.IsDBNull(iEventipofallafase)) entity.Eventipofallafase = dr.GetString(iEventipofallafase);

                    int iInterrmwde = dr.GetOrdinal(helper.Interrmwde);
                    if (!dr.IsDBNull(iInterrmwde)) entity.Interrmwde = dr.GetDecimal(iInterrmwde);

                    int iInterrmwa = dr.GetOrdinal(helper.Interrmwa);
                    if (!dr.IsDBNull(iInterrmwa)) entity.Interrmwa = dr.GetDecimal(iInterrmwa);

                    int iInterrminu = dr.GetOrdinal(helper.Interrminu);
                    if (!dr.IsDBNull(iInterrminu)) entity.Interrminu = dr.GetDecimal(iInterrminu);

                    int iInterrmw = dr.GetOrdinal(helper.Interrmw);
                    if (!dr.IsDBNull(iInterrmw)) entity.Interrmw = dr.GetDecimal(iInterrmw);

                    int iInterrdesc = dr.GetOrdinal(helper.Interrdesc);
                    if (!dr.IsDBNull(iInterrdesc)) entity.Interrdesc = dr.GetString(iInterrdesc);

                    int iInterrnivel = dr.GetOrdinal(helper.Interrnivel);
                    if (!dr.IsDBNull(iInterrnivel)) entity.Interrnivel = dr.GetString(iInterrnivel);

                    int iInterrracmf = dr.GetOrdinal(helper.Interrracmf);
                    if (!dr.IsDBNull(iInterrracmf)) entity.Interrracmf = dr.GetString(iInterrracmf);

                    int iInterrmfetapadesc = dr.GetOrdinal(helper.Interrmfetapadesc);
                    if (!dr.IsDBNull(iInterrmfetapadesc)) entity.Interrmfetapadesc = dr.GetString(iInterrmfetapadesc);

                    int iInterrmanualr = dr.GetOrdinal(helper.Interrmanualr);
                    if (!dr.IsDBNull(iInterrmanualr)) entity.Interrmanualr = dr.GetString(iInterrmanualr);

                    int iPtointerrupnomb = dr.GetOrdinal(helper.Ptointerrupnomb);
                    if (!dr.IsDBNull(iPtointerrupnomb)) entity.Ptointerrupnomb = dr.GetString(iPtointerrupnomb);

                    int iPtoentrenomb = dr.GetOrdinal(helper.Ptoentrenomb);
                    if (!dr.IsDBNull(iPtoentrenomb)) entity.Ptoentrenomb = dr.GetString(iPtoentrenomb);

                    int iClientenomb = dr.GetOrdinal(helper.Clientenomb);
                    if (!dr.IsDBNull(iClientenomb)) entity.Clientenomb = dr.GetString(iClientenomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ObtenerNroRegistros(int? idTipoEvento, DateTime fechaInicio, DateTime fechaFin,
           string version, string turno, int? idTipoEmpresa, int? idEmpresa, int? idTipoEquipo, string indInterrupcion, string areaOperativa,
           int todosaseg)
        {

            String query = String.Format(helper.SqlTotalRecords, idTipoEvento, idEmpresa, idTipoEquipo, version,
                    fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha),
                    turno, idTipoEmpresa, indInterrupcion, areaOperativa, todosaseg);

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);

            return 0;
        }

        public EventoDTO ObtenerEvento(int idEvento)
        {
            EventoDTO entity = null;

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerEvento);
            dbProvider.AddInParameter(command, helper.EVENCODI, DbType.Int32, idEvento);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new EventoDTO();

                    int iEVENCODI = dr.GetOrdinal(helper.EVENCODI);
                    int iEMPRCODIRESPON = dr.GetOrdinal(helper.EMPRCODIRESPON);
                    int iEQUICODI = dr.GetOrdinal(helper.EQUICODI);
                    int iEVENCLASECODI = dr.GetOrdinal(helper.EVENCLASECODI);
                    int iEMPRCODI = dr.GetOrdinal(helper.EMPRCODI);
                    int iTIPOEVENCODI = dr.GetOrdinal(helper.TIPOEVENCODI);
                    int iEVENINI = dr.GetOrdinal(helper.EVENINI);
                    int iEVENMWINDISP = dr.GetOrdinal(helper.EVENMWINDISP);
                    int iEVENFIN = dr.GetOrdinal(helper.EVENFIN);
                    int iSUBCAUSACODI = dr.GetOrdinal(helper.SUBCAUSACODI);
                    int iEVENASUNTO = dr.GetOrdinal(helper.EVENASUNTO);
                    int iEVENPADRE = dr.GetOrdinal(helper.EVENPADRE);
                    int iEVENINTERRUP = dr.GetOrdinal(helper.EVENINTERRUP);
                    int iLASTUSER = dr.GetOrdinal(helper.LASTUSER);
                    int iLASTDATE = dr.GetOrdinal(helper.LASTDATE);
                    int iEVENPREINI = dr.GetOrdinal(helper.EVENPREINI);
                    int iEVENPOSTFIN = dr.GetOrdinal(helper.EVENPOSTFIN);
                    int iEVENDESC = dr.GetOrdinal(helper.EVENDESC);
                    int iEVENTENSION = dr.GetOrdinal(helper.EVENTENSION);
                    int iEVENAOPERA = dr.GetOrdinal(helper.EVENAOPERA);
                    int iEVENPRELIMINAR = dr.GetOrdinal(helper.EVENPRELIMINAR);
                    int iEVENRELEVANTE = dr.GetOrdinal(helper.EVENRELEVANTE);
                    int iEVENCTAF = dr.GetOrdinal(helper.EVENCTAF);
                    int iEVENINFFALLA = dr.GetOrdinal(helper.EVENINFFALLA);
                    int iEVENINFFALLAN2 = dr.GetOrdinal(helper.EVENINFFALLAN2);
                    int iDELETED = dr.GetOrdinal(helper.DELETED);
                    int iEVENTIPOFALLA = dr.GetOrdinal(helper.EVENTIPOFALLA);
                    int iEVENTIPOFALLAFASE = dr.GetOrdinal(helper.EVENTIPOFALLAFASE);
                    int iSMSENVIADO = dr.GetOrdinal(helper.SMSENVIADO);
                    int iSMSENVIAR = dr.GetOrdinal(helper.SMSENVIAR);
                    int iEVENACTUACION = dr.GetOrdinal(helper.EVENACTUACION);
                    int iEQUINOMB = dr.GetOrdinal(helper.EQUINOMB);
                    int iEVENCOMENTARIOS = dr.GetOrdinal(helper.EVENCOMENTARIOS);
                    int iEVENPERTURBACION = dr.GetOrdinal(helper.EVENPERTURBACION);
                    int iEVENMWGENDESCON = dr.GetOrdinal(helper.EVENMWGENDESCON);
                    int iEVENGENDESCON = dr.GetOrdinal(helper.EVENGENDESCON);

                    if (!dr.IsDBNull(iEVENCODI)) entity.EVENCODI = Convert.ToInt32(dr.GetValue(iEVENCODI));
                    if (!dr.IsDBNull(iEMPRCODIRESPON)) entity.EMPRCODIRESPON = Convert.ToInt16(dr.GetValue(iEMPRCODIRESPON));
                    if (!dr.IsDBNull(iEQUICODI)) entity.EQUICODI = Convert.ToInt32(dr.GetValue(iEQUICODI));
                    if (!dr.IsDBNull(iEVENCLASECODI)) entity.EVENCLASECODI = Convert.ToInt16(dr.GetValue(iEVENCLASECODI));
                    if (!dr.IsDBNull(iEMPRCODI)) entity.EMPRCODI = Convert.ToInt32(dr.GetValue(iEMPRCODI));
                    if (!dr.IsDBNull(iTIPOEVENCODI)) entity.TIPOEVENCODI = Convert.ToInt32(dr.GetValue(iTIPOEVENCODI));
                    if (!dr.IsDBNull(iEVENINI)) entity.EVENINI = dr.GetDateTime(iEVENINI);
                    if (!dr.IsDBNull(iEVENMWINDISP)) entity.EVENMWINDISP = dr.GetDecimal(iEVENMWINDISP);
                    if (!dr.IsDBNull(iEVENFIN)) entity.EVENFIN = dr.GetDateTime(iEVENFIN);
                    if (!dr.IsDBNull(iSUBCAUSACODI)) entity.SUBCAUSACODI = Convert.ToInt32(dr.GetValue(iSUBCAUSACODI));
                    if (!dr.IsDBNull(iEVENASUNTO)) entity.EVENASUNTO = dr.GetString(iEVENASUNTO);
                    if (!dr.IsDBNull(iEVENPADRE)) entity.EVENPADRE = Convert.ToInt32(dr.GetValue(iEVENPADRE));
                    if (!dr.IsDBNull(iEVENINTERRUP)) entity.EVENINTERRUP = dr.GetString(iEVENINTERRUP);
                    if (!dr.IsDBNull(iLASTUSER)) entity.LASTUSER = dr.GetString(iLASTUSER);
                    if (!dr.IsDBNull(iLASTDATE)) entity.LASTDATE = dr.GetDateTime(iLASTDATE);
                    if (!dr.IsDBNull(iEVENPREINI)) entity.EVENPREINI = dr.GetDateTime(iEVENPREINI);
                    if (!dr.IsDBNull(iEVENPOSTFIN)) entity.EVENPOSTFIN = dr.GetDateTime(iEVENPOSTFIN);
                    if (!dr.IsDBNull(iEVENDESC)) entity.EVENDESC = dr.GetString(iEVENDESC);
                    if (!dr.IsDBNull(iEVENTENSION)) entity.EVENTENSION = dr.GetDecimal(iEVENTENSION);
                    if (!dr.IsDBNull(iEVENAOPERA)) entity.EVENAOPERA = dr.GetString(iEVENAOPERA);
                    if (!dr.IsDBNull(iEVENPRELIMINAR)) entity.EVENPRELIMINAR = dr.GetString(iEVENPRELIMINAR);
                    if (!dr.IsDBNull(iEVENRELEVANTE)) entity.EVENRELEVANTE = Convert.ToInt32(dr.GetValue(iEVENRELEVANTE));
                    if (!dr.IsDBNull(iEVENCTAF)) entity.EVENCTAF = dr.GetString(iEVENCTAF);
                    if (!dr.IsDBNull(iEVENINFFALLA)) entity.EVENINFFALLA = dr.GetString(iEVENINFFALLA);
                    if (!dr.IsDBNull(iEVENINFFALLAN2)) entity.EVENINFFALLAN2 = dr.GetString(iEVENINFFALLAN2);
                    if (!dr.IsDBNull(iDELETED)) entity.DELETED = dr.GetString(iDELETED);
                    if (!dr.IsDBNull(iEVENTIPOFALLA)) entity.EVENTIPOFALLA = dr.GetString(iEVENTIPOFALLA);
                    if (!dr.IsDBNull(iEVENTIPOFALLAFASE)) entity.EVENTIPOFALLAFASE = dr.GetString(iEVENTIPOFALLAFASE);
                    if (!dr.IsDBNull(iSMSENVIADO)) entity.SMSENVIADO = dr.GetString(iSMSENVIADO);
                    if (!dr.IsDBNull(iSMSENVIAR)) entity.SMSENVIAR = dr.GetString(iSMSENVIAR);
                    if (!dr.IsDBNull(iEVENACTUACION)) entity.EVENACTUACION = dr.GetString(iEVENACTUACION);
                    if (!dr.IsDBNull(iEQUINOMB)) entity.EQUIABREV = dr.GetString(iEQUINOMB);
                    if (!dr.IsDBNull(iEVENCOMENTARIOS)) entity.EVENCOMENTARIOS = dr.GetString(iEVENCOMENTARIOS);
                    if (!dr.IsDBNull(iEVENPERTURBACION)) entity.EVENPERTURBACION = dr.GetString(iEVENPERTURBACION);
                    if (!dr.IsDBNull(iEVENMWGENDESCON)) entity.EVENMWGENDESCON = dr.GetDecimal(iEVENMWGENDESCON);
                    if (!dr.IsDBNull(iEVENGENDESCON)) entity.EVENGENDESCON = dr.GetString(iEVENGENDESCON);

                    int iTIPOEVENABREV = dr.GetOrdinal(helper.TIPOEVENABREV);
                    if (!dr.IsDBNull(iTIPOEVENABREV)) entity.TIPOEVENABREV = dr.GetString(iTIPOEVENABREV);
                    int iCAUSAEVENABREV = dr.GetOrdinal(helper.CAUSAEVENABREV);
                    if (!dr.IsDBNull(iCAUSAEVENABREV)) entity.CAUSAEVENABREV = dr.GetString(iCAUSAEVENABREV);
                    int iSUBCAUSAABREV = dr.GetOrdinal(helper.SUBCAUSAABREV);
                    if (!dr.IsDBNull(iSUBCAUSAABREV)) entity.SUBCAUSAABREV = dr.GetString(iSUBCAUSAABREV);
                    
                    int iEVENDESCCTAF = dr.GetOrdinal(helper.Evendescctaf);
                    if (!dr.IsDBNull(iEVENDESCCTAF)) entity.EVENDESCCTAF = dr.GetString(iEVENDESCCTAF);
                }
            }

            return entity;
        }

        public EventoDTO ObtenerResumenEvento(int idEvento)
        {
            EventoDTO entity = null;

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerResumenEvento);
            dbProvider.AddInParameter(command, helper.EVENCODI, DbType.Int32, idEvento);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new EventoDTO();

                    int iEVENCODI = dr.GetOrdinal(helper.EVENCODI);
                    int iEVENINI = dr.GetOrdinal(helper.EVENINI);
                    int iEMPRNOMB = dr.GetOrdinal(helper.EMPRNOMB);
                    int iEQUIABREV = dr.GetOrdinal(helper.EQUINOMB);
                    int iEVENASUNTO = dr.GetOrdinal(helper.EVENASUNTO);
                    int iEVENDESC = dr.GetOrdinal(helper.EVENDESC);
                    int iTIPOEVENCODI = dr.GetOrdinal(helper.TIPOEVENCODI);
                    int iEVENPERTURBACION = dr.GetOrdinal(helper.EVENPERTURBACION);

                    if (!dr.IsDBNull(iEVENCODI)) entity.EVENCODI = Convert.ToInt32(dr.GetValue(iEVENCODI));
                    if (!dr.IsDBNull(iEVENINI)) entity.EVENINI = dr.GetDateTime(iEVENINI);
                    if (!dr.IsDBNull(iEMPRNOMB)) entity.EMPRNOMB = dr.GetString(iEMPRNOMB);
                    if (!dr.IsDBNull(iEQUIABREV)) entity.EQUIABREV = dr.GetString(iEQUIABREV);
                    if (!dr.IsDBNull(iEVENASUNTO)) entity.EVENASUNTO = dr.GetString(iEVENASUNTO);
                    if (!dr.IsDBNull(iEVENDESC)) entity.EVENDESC = dr.GetString(iEVENDESC);
                    if (!dr.IsDBNull(iTIPOEVENCODI)) entity.TIPOEVENCODI = Convert.ToInt32(dr.GetValue(iTIPOEVENCODI));
                    if (!dr.IsDBNull(iEVENPERTURBACION)) entity.EVENPERTURBACION = dr.GetString(iEVENPERTURBACION);
                }
            }

            return entity;
        }

        public List<AreaDTO> ObtenerAreaPorEmpresa(int? idEmpresa, string idFamilia)
        {
            List<AreaDTO> entitys = new List<AreaDTO>();

            if (idFamilia == "") idFamilia = "0";
            String query = String.Format(helper.SqlObtenerAreaPorEmpresa, idEmpresa, idFamilia);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    AreaDTO entity = new AreaDTO();

                    int iAREACODI = dr.GetOrdinal(helper.AREACODI);
                    int iAREANOMB = dr.GetOrdinal(helper.AREANOMB);
                    int iTAREAABREV = dr.GetOrdinal(helper.TAREAABREV);

                    if (!dr.IsDBNull(iAREACODI)) entity.AREACODI = Convert.ToInt16(dr.GetValue(iAREACODI));
                    if (!dr.IsDBNull(iAREANOMB)) entity.AREANOMB = dr.GetString(iAREANOMB);
                    if (!dr.IsDBNull(iTAREAABREV)) entity.TAREAABREV = dr.GetString(iTAREAABREV);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EquipoDTO> BuscarEquipoEvento(string idEmpresa, int? idArea, string idFamilia, string filtro, int nroPagina, int nroFilas)
        {
            String query = String.Format(helper.SqlBusquedaEquipoEvento, idEmpresa, idFamilia, idArea, filtro, nroPagina, nroFilas);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<EquipoDTO> entitys = new List<EquipoDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EquipoDTO entity = new EquipoDTO();

                    int iEmprNomb = dr.GetOrdinal(helper.EMPRNOMB);
                    if (!dr.IsDBNull(iEmprNomb)) entity.EMPRENOMB = dr.GetString(iEmprNomb);

                    int iAreaNomb = dr.GetOrdinal(helper.AREANOMB);
                    if (!dr.IsDBNull(iAreaNomb)) entity.AREANOMB = dr.GetString(iAreaNomb);

                    int iFamAbrev = dr.GetOrdinal(helper.FAMABREV);
                    if (!dr.IsDBNull(iFamAbrev)) entity.FAMABREV = dr.GetString(iFamAbrev);

                    int iEquiAbrev = dr.GetOrdinal(helper.EQUIABREV);
                    if (!dr.IsDBNull(iEquiAbrev)) entity.EQUIABREV = dr.GetString(iEquiAbrev);

                    int iTareaAbrev = dr.GetOrdinal(helper.TAREAABREV);
                    if (!dr.IsDBNull(iTareaAbrev)) entity.TAREAABREV = dr.GetString(iTareaAbrev);

                    int iEquiCodi = dr.GetOrdinal(helper.EQUICODI);
                    if (!dr.IsDBNull(iEquiCodi)) entity.EQUICODI = Convert.ToInt16(dr.GetValue(iEquiCodi));

                    int iEmprCodi = dr.GetOrdinal(helper.EMPRCODI);
                    if (!dr.IsDBNull(iEmprCodi)) entity.EMPRCODI = Convert.ToInt16(dr.GetValue(iEmprCodi));

                    int iEquiTension = dr.GetOrdinal(helper.EQUITENSION);
                    if (!dr.IsDBNull(iEquiTension)) entity.EQUITENSION = Convert.ToInt16(dr.GetValue(iEquiTension));

                    try
                    {
                        int iAREACODI = dr.GetOrdinal(helper.AREACODI);
                        if (!dr.IsDBNull(iAREACODI)) entity.AREACODI = dr.GetInt16(iAREACODI);

                        int iFAMCODI = dr.GetOrdinal(helper.FAMCODI);
                        if (!dr.IsDBNull(iFAMCODI)) entity.FAMCODI = dr.GetInt16(iFAMCODI);
                    }
                    catch { };

                    int iGrupotipocogen = dr.GetOrdinal(this.helper.Grupotipocogen);
                    if (!dr.IsDBNull(iGrupotipocogen)) entity.Grupotipocogen = dr.GetString(iGrupotipocogen);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EquipoDTO> BuscarEquipoEventoExtranet(string idEmpresa, int? idArea, string idFamilia, string filtro, int nroPagina, int nroFilas, int tipoEmprcodi)
        {
            String cad = "";
            if (tipoEmprcodi == 4 || tipoEmprcodi == 2)
                cad = " AND CASE WHEN (equipo.famcodi IN (8, 9, 10, 7,6,1,17,24,47) AND equipo.equitension > 100) THEN 'A' WHEN equipo.famcodi IN (8, 9, 10,7,6,1,17,24,47) THEN CASE WHEN equipo.equicodi in (711,11301,1495,11346,1397,10498,10496,10500,11551,12990,8058,330,299) THEN 'A' ELSE 'C' END ELSE 'B' END IN ('A', 'B') ";
            String query = String.Format(helper.SqlBusquedaEquipoEventoExtranet, idEmpresa, idFamilia, idArea, filtro, nroPagina, nroFilas, cad);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<EquipoDTO> entitys = new List<EquipoDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EquipoDTO entity = new EquipoDTO();

                    int iEmprNomb = dr.GetOrdinal(helper.EMPRNOMB);
                    if (!dr.IsDBNull(iEmprNomb)) entity.EMPRENOMB = dr.GetString(iEmprNomb);

                    int iAreaCodi = dr.GetOrdinal(helper.AREACODI);
                    if (!dr.IsDBNull(iAreaCodi)) entity.AREACODI = Convert.ToInt16(dr.GetValue(iAreaCodi));

                    int iAreaNomb = dr.GetOrdinal(helper.AREANOMB);
                    if (!dr.IsDBNull(iAreaNomb)) entity.AREANOMB = dr.GetString(iAreaNomb);

                    int iFamcodi = dr.GetOrdinal(helper.FAMCODI);
                    if (!dr.IsDBNull(iFamcodi)) entity.FAMCODI = Convert.ToInt16(dr.GetValue(iFamcodi));

                    int iFamAbrev = dr.GetOrdinal(helper.FAMABREV);
                    if (!dr.IsDBNull(iFamAbrev)) entity.FAMABREV = dr.GetString(iFamAbrev);

                    int iEquiAbrev = dr.GetOrdinal(helper.EQUIABREV);
                    if (!dr.IsDBNull(iEquiAbrev)) entity.EQUIABREV = dr.GetString(iEquiAbrev);

                    int iTareaAbrev = dr.GetOrdinal(helper.TAREAABREV);
                    if (!dr.IsDBNull(iTareaAbrev)) entity.TAREAABREV = dr.GetString(iTareaAbrev);

                    int iEquiCodi = dr.GetOrdinal(helper.EQUICODI);
                    if (!dr.IsDBNull(iEquiCodi)) entity.EQUICODI = Convert.ToInt16(dr.GetValue(iEquiCodi));

                    int iEmprCodi = dr.GetOrdinal(helper.EMPRCODI);
                    if (!dr.IsDBNull(iEmprCodi)) entity.EMPRCODI = Convert.ToInt16(dr.GetValue(iEmprCodi));

                    int iEquiTension = dr.GetOrdinal(helper.EQUITENSION);
                    if (!dr.IsDBNull(iEquiTension)) entity.EQUITENSION = Convert.ToInt16(dr.GetValue(iEquiTension));

                    int iGrupotipocogen = dr.GetOrdinal(this.helper.Grupotipocogen);
                    if (!dr.IsDBNull(iGrupotipocogen)) entity.Grupotipocogen = dr.GetString(iGrupotipocogen);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EquipoDTO> BuscarEquipoEventoNoPermitidos()
        {
            String cad = "";
            String query = String.Format(helper.SqlBusquedaEquipoNoPermitidos);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<EquipoDTO> entitys = new List<EquipoDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EquipoDTO entity = new EquipoDTO();

                    int iEmprNomb = dr.GetOrdinal(helper.EMPRNOMB);
                    if (!dr.IsDBNull(iEmprNomb)) entity.EMPRENOMB = dr.GetString(iEmprNomb);
                    
                    int iEquiAbrev = dr.GetOrdinal(helper.EQUIABREV);
                    if (!dr.IsDBNull(iEquiAbrev)) entity.EQUIABREV = dr.GetString(iEquiAbrev);

                    int iEquiCodi = dr.GetOrdinal(helper.EQUICODI);
                    if (!dr.IsDBNull(iEquiCodi)) entity.EQUICODI = Convert.ToInt16(dr.GetValue(iEquiCodi));

                    int iEmprCodi = dr.GetOrdinal(helper.EMPRCODI);
                    if (!dr.IsDBNull(iEmprCodi)) entity.EMPRCODI = Convert.ToInt16(dr.GetValue(iEmprCodi));

                    int iEquiTension = dr.GetOrdinal(helper.EQUITENSION);
                    if (!dr.IsDBNull(iEquiTension)) entity.EQUITENSION = Convert.ToInt16(dr.GetValue(iEquiTension));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EquipoDTO> BuscarEquipoEventoIntervenciones(string idEmpresa, int? idArea, string idFamilia, string filtro, int nroPagina, int nroFilas)
        {
            String query = String.Format(helper.SqlBusquedaEquipoEventoIntervenciones, idEmpresa, idFamilia, idArea, filtro, nroPagina, nroFilas);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<EquipoDTO> entitys = new List<EquipoDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EquipoDTO entity = new EquipoDTO();

                    int iEmprNomb = dr.GetOrdinal(helper.EMPRNOMB);
                    if (!dr.IsDBNull(iEmprNomb)) entity.EMPRENOMB = dr.GetString(iEmprNomb);

                    int iAreaNomb = dr.GetOrdinal(helper.AREANOMB);
                    if (!dr.IsDBNull(iAreaNomb)) entity.AREANOMB = dr.GetString(iAreaNomb);

                    int iFamAbrev = dr.GetOrdinal(helper.FAMABREV);
                    if (!dr.IsDBNull(iFamAbrev)) entity.FAMABREV = dr.GetString(iFamAbrev);

                    int iEquiAbrev = dr.GetOrdinal(helper.EQUIABREV);
                    if (!dr.IsDBNull(iEquiAbrev)) entity.EQUIABREV = dr.GetString(iEquiAbrev);

                    int iTareaAbrev = dr.GetOrdinal(helper.TAREAABREV);
                    if (!dr.IsDBNull(iTareaAbrev)) entity.TAREAABREV = dr.GetString(iTareaAbrev);

                    int iEquiCodi = dr.GetOrdinal(helper.EQUICODI);
                    if (!dr.IsDBNull(iEquiCodi)) entity.EQUICODI = Convert.ToInt16(dr.GetValue(iEquiCodi));

                    int iEmprCodi = dr.GetOrdinal(helper.EMPRCODI);
                    if (!dr.IsDBNull(iEmprCodi)) entity.EMPRCODI = Convert.ToInt16(dr.GetValue(iEmprCodi));

                    int iAREACODI = dr.GetOrdinal(helper.AREACODI);
                    if (!dr.IsDBNull(iAREACODI)) entity.AREACODI = dr.GetInt16(iAREACODI);

                    int iFAMCODI = dr.GetOrdinal(helper.FAMCODI);
                    if (!dr.IsDBNull(iFAMCODI)) entity.FAMCODI = dr.GetInt16(iFAMCODI);

                    int iGrupotipocogen = dr.GetOrdinal(this.helper.Grupotipocogen);
                    if (!dr.IsDBNull(iGrupotipocogen)) entity.Grupotipocogen = dr.GetString(iGrupotipocogen);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ObtenerNroFilasBusquedaEquipo(string idEmpresa, int? idArea, string idFamilia, string filtro)
        {
            String query = String.Format(helper.SqlTotalRecordsEquipo, idEmpresa, idFamilia, idArea, filtro);

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);

            return 0;
        }

        public int ObtenerNroFilasBusquedaEquipoExtranet(string idEmpresa, int? idArea, string idFamilia, string filtro, int tipoEmpr)
        {
            String cad = "";
            if (tipoEmpr == 4 || tipoEmpr == 2) cad = " AND CASE WHEN (equipo.famcodi IN (8, 9, 10,7,6,1,17,24,47) AND equipo.equitension > 100) THEN 'A' WHEN equipo.famcodi IN (8, 9, 10,7,6,1,17,24,47) THEN CASE WHEN equipo.equicodi in (711,11301,1495,11346,1397,10498,10496,10500,11551,12990,8058,330,299) THEN 'A' ELSE 'C' END ELSE 'B' END IN ('A', 'B') ";
            String query = String.Format(helper.SqlTotalRecordsEquipoExtranet, idEmpresa, idFamilia, idArea, filtro, cad);

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);

            return 0;
        }

        public List<EmpresaDTO> ListarEmpresas()
        {
            List<EmpresaDTO> entitys = new List<EmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarEmpresas);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EmpresaDTO entity = new EmpresaDTO();

                    int iEmprNomb = dr.GetOrdinal(helper.EMPRNOMB);
                    if (!dr.IsDBNull(iEmprNomb)) entity.EMPRNOMB = dr.GetString(iEmprNomb);

                    int iRazSocial = dr.GetOrdinal(helper.RAZSOCIAL);
                    if (!dr.IsDBNull(iRazSocial)) entity.EMPRRAZSOCIAL= dr.GetString(iRazSocial);

                    int iEmprCodi = dr.GetOrdinal(helper.EMPRCODI);
                    if (!dr.IsDBNull(iEmprCodi)) entity.EMPRCODI = Convert.ToInt16(dr.GetValue(iEmprCodi));

                    int iScadaCodi = dr.GetOrdinal(helper.SCADACODI);
                    if (!dr.IsDBNull(iScadaCodi)) entity.SCADACODI = Convert.ToInt32(dr.GetValue(iScadaCodi));

                    int iEmprEstado = dr.GetOrdinal(helper.EMPRESTADO);
                    if (!dr.IsDBNull(iEmprEstado)) entity.EMPRESTADO = dr.GetString(iEmprEstado);


                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EmpresaDTO> ListarEmpresasPorTipo(int idTipoEmpresa)
        {
            List<EmpresaDTO> entitys = new List<EmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarEmpresasPorTipo);
            dbProvider.AddInParameter(command, helper.TIPOEMPRCODI, DbType.Int32, idTipoEmpresa);


            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EmpresaDTO entity = new EmpresaDTO();

                    int iEmprNomb = dr.GetOrdinal(helper.EMPRNOMB);
                    if (!dr.IsDBNull(iEmprNomb)) entity.EMPRNOMB = dr.GetString(iEmprNomb);

                    int iEmprCodi = dr.GetOrdinal(helper.EMPRCODI);
                    if (!dr.IsDBNull(iEmprCodi)) entity.EMPRCODI = Convert.ToInt16(dr.GetValue(iEmprCodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<FamiliaDTO> ListarFamilias()
        {
            List<FamiliaDTO> entitys = new List<FamiliaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarFamilias);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FamiliaDTO entity = new FamiliaDTO();

                    int iFamNomb = dr.GetOrdinal(helper.FAMNOMB);
                    if (!dr.IsDBNull(iFamNomb)) entity.FAMNOMB = dr.GetString(iFamNomb);

                    int iFamCodi = dr.GetOrdinal(helper.FAMCODI);
                    if (!dr.IsDBNull(iFamCodi)) entity.FAMCODI = Convert.ToInt16(dr.GetValue(iFamCodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void ActualizarInformePerturbacion(string estado, int idEvento)
        {
            try
            {
                string query = string.Format(helper.SqlActualizarInformePerturbacion, estado, idEvento);
                DbCommand command = dbProvider.GetSqlStringCommand(query);
                dbProvider.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Listar equipos actualizados para intervenciones
        /// </summary>
        /// <returns></returns>
        public List<EquipoDTO> BuscarEquiposIntervencionesActualizados()
        {
            List<EquipoDTO> entitys = new List<EquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlBusquedaEquiposIntervencionesActualizados);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EquipoDTO entity = new EquipoDTO();

                    int iEquiCodi = dr.GetOrdinal(helper.EQUICODI);
                    if (!dr.IsDBNull(iEquiCodi)) entity.EQUICODI = Convert.ToInt16(dr.GetValue(iEquiCodi));

                    int iEmprCodi = dr.GetOrdinal(helper.EMPRCODI);
                    if (!dr.IsDBNull(iEmprCodi)) entity.EMPRCODI = Convert.ToInt16(dr.GetValue(iEmprCodi));

                    int iAREACODI = dr.GetOrdinal(helper.AREACODI);
                    if (!dr.IsDBNull(iAREACODI)) entity.AREACODI = dr.GetInt16(iAREACODI);

                    int iFAMCODI = dr.GetOrdinal(helper.FAMCODI);
                    if (!dr.IsDBNull(iFAMCODI)) entity.FAMCODI = dr.GetInt16(iFAMCODI);

                    int iEquiAbrev = dr.GetOrdinal(helper.EQUIABREV);
                    if (!dr.IsDBNull(iEquiAbrev)) entity.EQUIABREV = dr.GetString(iEquiAbrev);

                    int iEquiNomb = dr.GetOrdinal(helper.EQUINOMB);
                    if (!dr.IsDBNull(iEquiNomb)) entity.EQUINOMB = dr.GetString(iEquiNomb);

                    int iAreaNomb = dr.GetOrdinal(helper.AREANOMB);
                    if (!dr.IsDBNull(iAreaNomb)) entity.AREANOMB = dr.GetString(iAreaNomb);

                    int iTareaAbrev = dr.GetOrdinal(helper.TAREAABREV);
                    if (!dr.IsDBNull(iTareaAbrev)) entity.TAREAABREV = dr.GetString(iTareaAbrev);

                    int iEQUITENSION = dr.GetOrdinal(helper.EQUITENSION);
                    if (!dr.IsDBNull(iEQUITENSION)) entity.EQUITENSION = dr.GetDecimal(iEQUITENSION);

                    int iEQUIPOT = dr.GetOrdinal(helper.EQUIPOT);
                    if (!dr.IsDBNull(iEQUIPOT)) entity.EQUIPOT = dr.GetDecimal(iEQUIPOT);

                    int iEmprNomb = dr.GetOrdinal(helper.EMPRNOMB);
                    if (!dr.IsDBNull(iEmprNomb)) entity.EMPRENOMB = dr.GetString(iEmprNomb);

                    int iFamAbrev = dr.GetOrdinal(helper.FAMABREV);
                    if (!dr.IsDBNull(iFamAbrev)) entity.FAMABREV = dr.GetString(iFamAbrev);

                    int iGrupotipocogen = dr.GetOrdinal(this.helper.Grupotipocogen);
                    if (!dr.IsDBNull(iGrupotipocogen)) entity.Grupotipocogen = dr.GetString(iGrupotipocogen);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #region FIT - SGOCOES func A
        public List<SiEmpresaDTO> ObtenerEmpresasInvolucrada()
        {

            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
            try
            {
                string query = helper.SqlObtenerEmpresasInvolucrada;
                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        SiEmpresaDTO entity = new SiEmpresaDTO();

                        int iEmprCodi = dr.GetOrdinal(helper.EMPRCODI);
                        if (!dr.IsDBNull(iEmprCodi)) entity.Emprcodi = dr.GetInt32(iEmprCodi);

                        int iEmprNomb = dr.GetOrdinal(helper.EMPRNOMB);
                        if (!dr.IsDBNull(iEmprNomb)) entity.Emprnomb = dr.GetString(iEmprNomb);

                        entitys.Add(entity);
                    }
                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex); ;
            }
            return entitys;

        }

        public List<SiEmpresaDTO> ObtenerEmpresasRecomendacion()
        {

            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
            try
            {
                string query = helper.SqlObtenerEmpresasRecomendacion;
                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        SiEmpresaDTO entity = new SiEmpresaDTO();

                        int iEmprCodi = dr.GetOrdinal(helper.EMPRCODI);
                        if (!dr.IsDBNull(iEmprCodi)) entity.Emprcodi = dr.GetInt32(iEmprCodi);

                        int iEmprNomb = dr.GetOrdinal(helper.EMPRNOMB);
                        if (!dr.IsDBNull(iEmprNomb)) entity.Emprnomb = dr.GetString(iEmprNomb);

                        entitys.Add(entity);
                    }
                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex); ;
            }
            return entitys;
        }
        public List<SiEmpresaDTO> ObtenerEmpresasObservacion()
        {

            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
            try
            {
                string query = helper.SqlObtenerEmpresasObservacion;
                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        SiEmpresaDTO entity = new SiEmpresaDTO();

                        int iEmprCodi = dr.GetOrdinal(helper.EMPRCODI);
                        if (!dr.IsDBNull(iEmprCodi)) entity.Emprcodi = dr.GetInt32(iEmprCodi);

                        int iEmprNomb = dr.GetOrdinal(helper.EMPRNOMB);
                        if (!dr.IsDBNull(iEmprNomb)) entity.Emprnomb = dr.GetString(iEmprNomb);

                        entitys.Add(entity);
                    }
                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex); ;
            }
            return entitys;
        }

        public List<SiEmpresaDTO> ObtenerEmpresaPropietaria()
        {

            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
            try
            {
                string query = helper.SqlObtenerEmpresaPropietaria;
                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        SiEmpresaDTO entity = new SiEmpresaDTO();

                        int iEmprCodi = dr.GetOrdinal(helper.EMPRCODI);
                        if (!dr.IsDBNull(iEmprCodi)) entity.Emprcodi = dr.GetInt32(iEmprCodi);

                        int iEmprNomb = dr.GetOrdinal(helper.EMPRNOMB);
                        if (!dr.IsDBNull(iEmprNomb)) entity.Emprnomb = dr.GetString(iEmprNomb);

                        entitys.Add(entity);
                    }
                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex); ;
            }
            return entitys;
        }
        public List<EventoDTO> ObtenerTipoEquipo()
        {

            List<EventoDTO> entitys = new List<EventoDTO>();
            try
            {
                string query = helper.SqlObtenerTipoEquipo;
                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        EventoDTO entity = new EventoDTO();

                        int iFAMCODI = dr.GetOrdinal(helper.FAMCODI);
                        if (!dr.IsDBNull(iFAMCODI)) entity.FAMCODI = dr.GetInt32(iFAMCODI);

                        int iFAMNOMB = dr.GetOrdinal(helper.FAMNOMB);
                        if (!dr.IsDBNull(iFAMNOMB)) entity.FAMNOMB = dr.GetString(iFAMNOMB);

                        entitys.Add(entity);
                    }
                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex); ;
            }
            return entitys;
        }

        public List<EventoDTO> ConsultarAnalisisFallas(EventoDTO oEventoDTO)
        {

            List<EventoDTO> entitys = new List<EventoDTO>();
            try
            {
                string query = string.Format(helper.SqlConsultarAnalisisFallas,
                    oEventoDTO.DI, //0
                    oEventoDTO.DF, //1
                    oEventoDTO.EmpresaPropietaria,  //2
                    oEventoDTO.TipoEquipo, //3
                    oEventoDTO.RNC, //4
                    oEventoDTO.ERACMF, //5
                    oEventoDTO.ERACMT, //6
                    oEventoDTO.EDAGSF, //7
                    oEventoDTO.FuerzaMayor, //8 
                    oEventoDTO.Anulado, //9
                    oEventoDTO.Estado, //10
                    oEventoDTO.Impugnacion, //11 
                    oEventoDTO.EmpresaInvolucrada, //12 
                    oEventoDTO.TipoReunion); //13
                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        EventoDTO entity = new EventoDTO();

                        int iAFECODI = dr.GetOrdinal("AFECODI");
                        int iCODIGO = dr.GetOrdinal("CODIGO");
                        int iNOMBRE_EVENTO = dr.GetOrdinal("NOMBRE_EVENTO");
                        int iINTERRUMPIDO = dr.GetOrdinal("INTERRUMPIDO");
                        int iFECHA_EVENTO = dr.GetOrdinal("FECHA_EVENTO");
                        int iFECHA_REUNION = dr.GetOrdinal("FECHA_REUNION");
                        int iFECHA_INFORME = dr.GetOrdinal("FECHA_INFORME");
                        int iREVISADO_DJR = dr.GetOrdinal("REVISADO_DJR");
                        int iREVISADO_DO = dr.GetOrdinal("REVISADO_DO");
                        int iPUBLICADO = dr.GetOrdinal("PUBLICADO");
                        int iESTADO = dr.GetOrdinal("ESTADO");
                        int iIMPUG = dr.GetOrdinal("IMPUG");
                        int iRESPONSABLE = dr.GetOrdinal("RESPONSABLE");
                        int iINF_TECNICO = dr.GetOrdinal("INF_TECNICO");
                        int iAFEFZAMAYOR = dr.GetOrdinal("AFEFZAMAYOR");
                        int iEVENCODI = dr.GetOrdinal("EVENCODI");

                        if (!dr.IsDBNull(iAFECODI)) entity.AFECODI = dr.GetInt32(iAFECODI);
                        if (!dr.IsDBNull(iCODIGO)) entity.CODIGO = dr.GetString(iCODIGO);
                        if (!dr.IsDBNull(iNOMBRE_EVENTO)) entity.NOMBRE_EVENTO = dr.GetString(iNOMBRE_EVENTO);
                        if (!dr.IsDBNull(iINTERRUMPIDO)) entity.INTERRUMPIDO = dr.GetString(iINTERRUMPIDO);
                        if (!dr.IsDBNull(iFECHA_EVENTO)) entity.FECHA_EVENTO = dr.GetString(iFECHA_EVENTO);
                        if (!dr.IsDBNull(iFECHA_REUNION)) entity.FECHA_REUNION = dr.GetString(iFECHA_REUNION);
                        if (!dr.IsDBNull(iFECHA_INFORME)) entity.FECHA_INFORME = dr.GetString(iFECHA_INFORME);
                        if (!dr.IsDBNull(iREVISADO_DJR)) entity.REVISADO_DJR = dr.GetString(iREVISADO_DJR);
                        if (!dr.IsDBNull(iREVISADO_DO)) entity.REVISADO_DO = dr.GetString(iREVISADO_DO);
                        if (!dr.IsDBNull(iPUBLICADO)) entity.PUBLICADO = dr.GetString(iPUBLICADO);
                        if (!dr.IsDBNull(iESTADO)) entity.ESTADO = dr.GetString(iESTADO);
                        if (!dr.IsDBNull(iIMPUG)) entity.IMPUG = dr.GetString(iIMPUG);
                        if (!dr.IsDBNull(iRESPONSABLE)) entity.RESPONSABLE = dr.GetString(iRESPONSABLE);
                        if (!dr.IsDBNull(iINF_TECNICO)) entity.INF_TECNICO = dr.GetString(iINF_TECNICO);
                        if (!dr.IsDBNull(iAFEFZAMAYOR)) entity.AFEFZAMAYOR = dr.GetString(iAFEFZAMAYOR);
                        if (!dr.IsDBNull(iEVENCODI)) entity.EVENCODI = dr.GetInt32(iEVENCODI);

                        entitys.Add(entity);
                    }
                }
            }
            catch (Exception ex)
            {
                entitys = null;
            }
            return entitys;
        }
       
        public List<SiEmpresaDTO> BuscarEmpresa(string nombreempresa)
        {

            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
            try
            {
                string query = string.Format(helper.SqlBuscarEmpresa, nombreempresa);
                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        SiEmpresaDTO entity = new SiEmpresaDTO();

                        int iEMPRCODI = dr.GetOrdinal("EMPRCODI");
                        int iEMPRNOMB = dr.GetOrdinal("EMPRNOMB");

                        if (!dr.IsDBNull(iEMPRCODI)) entity.Emprcodi = dr.GetInt32(iEMPRCODI);
                        if (!dr.IsDBNull(iEMPRNOMB)) entity.Emprnomb = dr.GetString(iEMPRNOMB);
                        entitys.Add(entity);
                    }
                }
            }
            catch (Exception ex)
            {
                entitys = null;
            }
            return entitys;
        }
        public List<SiEmpresaDTO> ObtenerEmpresa()
        {

            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
            try
            {
                string query = string.Format(helper.SqlObtenerEmpresa);
                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        SiEmpresaDTO entity = new SiEmpresaDTO();

                        int iEMPRCODI = dr.GetOrdinal("EMPRCODI");
                        int iEMPRNOMB = dr.GetOrdinal("EMPRNOMB");

                        if (!dr.IsDBNull(iEMPRCODI)) entity.Emprcodi = dr.GetInt32(iEMPRCODI);
                        if (!dr.IsDBNull(iEMPRNOMB)) entity.Emprnomb = dr.GetString(iEMPRNOMB);
                        entitys.Add(entity);
                    }
                }
            }
            catch (Exception ex)
            {
                entitys = null;
            }
            return entitys;
        }
        public List<SiEmpresaDTO> BuscarEmpresaPropietaria(string nombreempresa)
        {

            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
            try
            {
                string query = string.Format(helper.SqlBuscarEmpresaPropietaria, nombreempresa);
                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        SiEmpresaDTO entity = new SiEmpresaDTO();

                        int iEMPRCODI = dr.GetOrdinal("EMPRCODI");
                        int iEMPRNOMB = dr.GetOrdinal("EMPRNOMB");

                        if (!dr.IsDBNull(iEMPRCODI)) entity.Emprcodi = dr.GetInt32(iEMPRCODI);
                        if (!dr.IsDBNull(iEMPRNOMB)) entity.Emprnomb = dr.GetString(iEMPRNOMB);
                        entitys.Add(entity);
                    }
                }
            }
            catch (Exception ex)
            {
                entitys = null;
            }
            return entitys;
        }

        public List<string> ObtenerResponsableEvento()
        {

            List<string> entitys = new List<string>();
            try
            {
                string query = helper.SqlObtenerResponsableEvento;
                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        string entity = "";
                        int iNOMBRE = dr.GetOrdinal("NOMBRE");
                        if (!dr.IsDBNull(iNOMBRE)) entity = dr.GetString(iNOMBRE);
                        entitys.Add(entity);
                    }
                }
            }
            catch (Exception ex)
            {
                entitys = null;
            }
            return entitys;
        }

        public List<ReunionResponsableDTO> ObtenerReunionResponsable()
        {

            List<ReunionResponsableDTO> entitys = new List<ReunionResponsableDTO>();
            
            try
            {
                string query = helper.SqlObtenerReunionResponsable;
                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        ReunionResponsableDTO entity = new ReunionResponsableDTO();
                        int iRESPCOD = dr.GetOrdinal("CODIGO");
                        int iRESPNAME = dr.GetOrdinal("NOMBRE");
                        if (!dr.IsDBNull(iRESPCOD)) entity.RESPCOD = dr.GetInt32(iRESPCOD);
                        if (!dr.IsDBNull(iRESPNAME)) entity.RESPNAME = dr.GetString(iRESPNAME);
                        
                        //int iNOMBRE = dr.GetOrdinal("NOMBRE");
                        //if (!dr.IsDBNull(iNOMBRE)) entity = dr.GetString(iNOMBRE);
                        entitys.Add(entity);
                    }
                }
            }
            catch (Exception ex)
            {
                entitys = null;
            }
            return entitys;
        }

        public List<string> ObtenerCodigosEventosPorFechaNominal(string fecha)
        {
            var codigos = new List<string>();

            string query = string.Format(helper.SqlObtenerCodigosEventosPorFechaNominal, fecha);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int codigoIndex = dr.GetOrdinal("CODIGO");
                    codigos.Add(dr.GetString(codigoIndex));
                }
            }

            return codigos;
        }

        public List<string> ObtenerCodigosEventosPorFechaReunion(string fecha)
        {
            var codigos = new List<string>();

            string query = string.Format(helper.SqlObtenerCodigosEventosPorFechaReunion, fecha);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int codigoIndex = dr.GetOrdinal("CODIGO");
                    codigos.Add(dr.GetString(codigoIndex));
                }
            }

            return codigos;
        }

        public List<string> ObtenerCodigosEventosPorFechaElaboracionInformeTecnico(string fecha)
        {
            var codigos = new List<string>();

            string query = string.Format(helper.SqlObtenerCodigosEventosPorFechaElaboracionInformeTecnico, fecha);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int codigoIndex = dr.GetOrdinal("CODIGO");
                    codigos.Add(dr.GetString(codigoIndex));
                }
            }

            return codigos;
        }

        public List<ReporteSemanalItemDTO> ObtenerCodigosEventosPorFechaNominalSemanal(string fechaInicio, string fechaFin)
        {
            var registros = new List<ReporteSemanalItemDTO>();

            string query = string.Format(helper.SqlObtenerCodigosEventosPorFechaNominalSemanal, fechaInicio, fechaFin);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int afecodiIndex = dr.GetOrdinal("AFECODI");
                    int codigoIndex = dr.GetOrdinal("CODIGO");
                    int fechaIndex = dr.GetOrdinal("FECHA");

                    registros.Add(new ReporteSemanalItemDTO()
                    {
                        CODIGO = dr.GetString(codigoIndex),
                        AFECODI = dr.GetInt32(afecodiIndex),
                        FECHA = dr.GetString(fechaIndex)
                    });
                }
            }

            return registros;
        }

        public List<ReporteSemanalItemDTO> ObtenerCodigosEventosPorFechaReunionSemanal(string fechaInicio, string fechaFin)
        {
            var registros = new List<ReporteSemanalItemDTO>();

            string query = string.Format(helper.SqlObtenerCodigosEventosPorFechaReunionSemanal, fechaInicio, fechaFin);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int afecodiIndex = dr.GetOrdinal("AFECODI");
                    int codigoIndex = dr.GetOrdinal("CODIGO");
                    int fechaIndex = dr.GetOrdinal("FECHA");

                    registros.Add(new ReporteSemanalItemDTO()
                    {
                        CODIGO = dr.GetString(codigoIndex),
                        AFECODI = dr.GetInt32(afecodiIndex),
                        FECHA = dr.GetString(fechaIndex)
                    });
                }
            }

            return registros;
        }

        public List<ReporteSemanalItemDTO> ObtenerCodigosEventosPorFechaElaboracionInformeTecnicoSemanal(string fechaInicio, string fechaFin)
        {
            var registros = new List<ReporteSemanalItemDTO>();

            string query = string.Format(helper.SqlObtenerCodigosEventosPorFechaElaboracionInformeTecnicoSemanal, fechaInicio, fechaFin);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int afecodiIndex = dr.GetOrdinal("AFECODI");
                    int codigoIndex = dr.GetOrdinal("CODIGO");
                    int fechaIndex = dr.GetOrdinal("FECHA");

                    registros.Add(new ReporteSemanalItemDTO()
                    {
                        CODIGO = dr.GetString(codigoIndex),
                        AFECODI = dr.GetInt32(afecodiIndex),
                        FECHA = dr.GetString(fechaIndex)
                    });
                }
            }

            return registros;
        }

        public AnalisisFallaDTO ObtenerAnalisisFalla(int id)
        {

            AnalisisFallaDTO entity = new AnalisisFallaDTO();
            try
            {
                string query = string.Format(helper.SqlObtenerAnalisisFalla, id);
                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        int iAFECODI = dr.GetOrdinal("AFECODI");
                        int iEVENCODI = dr.GetOrdinal("EVENCODI");
                        int iAFEANIO = dr.GetOrdinal("AFEANIO");
                        int iAFECORR = dr.GetOrdinal("AFECORR");
                        int iAFERMC = dr.GetOrdinal("AFERMC");
                        int iAFEERACMF = dr.GetOrdinal("AFEERACMF");
                        int iAFERACMT = dr.GetOrdinal("AFERACMT");
                        int iAFEEDAGSF = dr.GetOrdinal("AFEEDAGSF");
                        int iAFERESPONSABLE = dr.GetOrdinal("AFERESPONSABLE");
                        int iAFECITFECHANOMINAL = dr.GetOrdinal("AFECITFECHANOMINAL");
                        int iAFECITFECHAELAB = dr.GetOrdinal("AFECITFECHAELAB");
                        int iAFEREUFECHANOMINAL = dr.GetOrdinal("AFEREUFECHANOMINAL");
                        int iAFEREUFECHAPROG = dr.GetOrdinal("AFEREUFECHAPROG");
                        int iAFEREUHORAPROG = dr.GetOrdinal("AFEREUHORAPROG");
                        int iAFECONVTIPOREUNION = dr.GetOrdinal("AFECONVTIPOREUNION");
                        int iAFERCTAEOBSERVACION = dr.GetOrdinal("AFERCTAEOBSERVACION");
                        int iAFEITFECHANOMINAL = dr.GetOrdinal("AFEITFECHANOMINAL");
                        int iAFEITFECHAELAB = dr.GetOrdinal("AFEITFECHAELAB");
                        int iAFEITRDJRESTADO = dr.GetOrdinal("AFEITRDJRESTADO");
                        int iAFEITRDJRFECHAENVIO = dr.GetOrdinal("AFEITRDJRFECHAENVIO");
                        int iAFEITRDJRFECHARECEP = dr.GetOrdinal("AFEITRDJRFECHARECEP");
                        int iAFEITRDOESTADO = dr.GetOrdinal("AFEITRDOESTADO");
                        int iAFEITRDOFECHAENVIO = dr.GetOrdinal("AFEITRDOFECHAENVIO");
                        int iAFEITRDOFECHARECEP = dr.GetOrdinal("AFEITRDOFECHARECEP");
                        int iLASTUSER = dr.GetOrdinal("LASTUSER");
                        int iLASTDATE = dr.GetOrdinal("LASTDATE");
                        int iAFEIMPUGNA = dr.GetOrdinal("AFEIMPUGNA");
                        int iAFERCTAEACTAFECHA = dr.GetOrdinal("AFERCTAEACTAFECHA");
                        int iAFERCTAEINFORMEFECHA = dr.GetOrdinal("AFERCTAEINFORMEFECHA");
                        int iAFECONVCITACIONFECHA = dr.GetOrdinal("AFECONVCITACIONFECHA");
                        int iAFEITPITFFECHASIST = dr.GetOrdinal("AFEITPITFFECHASIST");
                        int iAFEITPDECISFFECHASIST = dr.GetOrdinal("AFEITPDECISFFECHASIST");
                        int iAFECOMPFECHA = dr.GetOrdinal("AFECOMPFECHA");
                        int iAFECOMPFECHASIST = dr.GetOrdinal("AFECOMPFECHASIST");
                        int iAFEITDECFECHANOMINAL = dr.GetOrdinal("AFEITDECFECHANOMINAL");
                        int iAFEITDECFECHAELAB = dr.GetOrdinal("AFEITDECFECHAELAB");
                        int iAFEFZAFECHASIST = dr.GetOrdinal("AFEFZAFECHASIST");
                        int iAFEFZADECISFECHASIST = dr.GetOrdinal("AFEFZADECISFECHASIST");
                        int iAFEITPITFFECHA = dr.GetOrdinal("AFEITPITFFECHA");
                        int iAFEEMPRESPNINGUNA = dr.GetOrdinal("AFEEMPRESPNINGUNA");
                        int iAFEEMPCOMPNINGUNA = dr.GetOrdinal("AFEEMPCOMPNINGUNA");
                        int iAFEESTADO = dr.GetOrdinal("AFEESTADO");
                        int iAFEESTADOMOTIVO = dr.GetOrdinal("AFEESTADOMOTIVO");
                        int iAFEFZAMAYOR = dr.GetOrdinal("AFEFZAMAYOR");
                        int iCODIGO = dr.GetOrdinal("CODIGO");
                        int iAFSALA = dr.GetOrdinal("AFSALA");
                        int iAFEREUHORINI = dr.GetOrdinal("AFEREUHORINI");
                        int iAFEREUHORFIN = dr.GetOrdinal("AFEREUHORFIN");
                        int iAFELIMATENCOMEN = dr.GetOrdinal("AFELIMATENCOMEN");

                        if (!dr.IsDBNull(iAFECODI)) entity.AFECODI = dr.GetInt32(iAFECODI);
                        if (!dr.IsDBNull(iEVENCODI)) entity.EVENCODI = Convert.ToInt32(dr.GetDecimal(iEVENCODI));
                        if (!dr.IsDBNull(iAFEANIO)) entity.AFEANIO = dr.GetInt32(iAFEANIO);
                        if (!dr.IsDBNull(iAFECORR)) entity.AFECORR = dr.GetInt32(iAFECORR);
                        if (!dr.IsDBNull(iAFERMC)) entity.AFERMC = dr.GetString(iAFERMC);
                        if (!dr.IsDBNull(iAFEERACMF)) entity.AFEERACMF = dr.GetString(iAFEERACMF);
                        if (!dr.IsDBNull(iAFERACMT)) entity.AFERACMT = dr.GetString(iAFERACMT);
                        if (!dr.IsDBNull(iAFEEDAGSF)) entity.AFEEDAGSF = dr.GetString(iAFEEDAGSF);
                        if (!dr.IsDBNull(iAFERESPONSABLE)) entity.AFERESPONSABLE = dr.GetString(iAFERESPONSABLE);
                        if (!dr.IsDBNull(iAFECITFECHANOMINAL)) entity.AFECITFECHANOMINAL = dr.GetDateTime(iAFECITFECHANOMINAL);
                        if (!dr.IsDBNull(iAFECITFECHAELAB)) entity.AFECITFECHAELAB = dr.GetDateTime(iAFECITFECHAELAB);
                        if (!dr.IsDBNull(iAFEREUFECHANOMINAL)) entity.AFEREUFECHANOMINAL = dr.GetDateTime(iAFEREUFECHANOMINAL);
                        if (!dr.IsDBNull(iAFEREUFECHAPROG)) entity.AFEREUFECHAPROG = dr.GetDateTime(iAFEREUFECHAPROG);
                        if (!dr.IsDBNull(iAFEREUHORAPROG)) entity.AFEREUHORAPROG = dr.GetString(iAFEREUHORAPROG);
                        if (!dr.IsDBNull(iAFECONVTIPOREUNION)) entity.AFECONVTIPOREUNION = dr.GetString(iAFECONVTIPOREUNION);
                        if (!dr.IsDBNull(iAFERCTAEOBSERVACION)) entity.AFERCTAEOBSERVACION = dr.GetString(iAFERCTAEOBSERVACION);
                        if (!dr.IsDBNull(iAFEITFECHANOMINAL)) entity.AFEITFECHANOMINAL = dr.GetDateTime(iAFEITFECHANOMINAL);
                        if (!dr.IsDBNull(iAFEITFECHAELAB)) entity.AFEITFECHAELAB = dr.GetDateTime(iAFEITFECHAELAB);
                        if (!dr.IsDBNull(iAFEITRDJRESTADO)) entity.AFEITRDJRESTADO = dr.GetString(iAFEITRDJRESTADO);
                        if (!dr.IsDBNull(iAFEITRDJRFECHAENVIO)) entity.AFEITRDJRFECHAENVIO = dr.GetDateTime(iAFEITRDJRFECHAENVIO);
                        if (!dr.IsDBNull(iAFEITRDJRFECHARECEP)) entity.AFEITRDJRFECHARECEP = dr.GetDateTime(iAFEITRDJRFECHARECEP);
                        if (!dr.IsDBNull(iAFEITRDOESTADO)) entity.AFEITRDOESTADO = dr.GetString(iAFEITRDOESTADO);
                        if (!dr.IsDBNull(iAFEITRDOFECHAENVIO)) entity.AFEITRDOFECHAENVIO = dr.GetDateTime(iAFEITRDOFECHAENVIO);
                        if (!dr.IsDBNull(iAFEITRDOFECHARECEP)) entity.AFEITRDOFECHARECEP = dr.GetDateTime(iAFEITRDOFECHARECEP);
                        if (!dr.IsDBNull(iLASTUSER)) entity.LASTUSER = dr.GetString(iLASTUSER);
                        if (!dr.IsDBNull(iLASTDATE)) entity.LASTDATE = dr.GetDateTime(iLASTDATE);
                        if (!dr.IsDBNull(iAFEIMPUGNA)) entity.AFEIMPUGNA = dr.GetString(iAFEIMPUGNA);
                        if (!dr.IsDBNull(iAFERCTAEACTAFECHA)) entity.AFERCTAEACTAFECHA = dr.GetDateTime(iAFERCTAEACTAFECHA);
                        if (!dr.IsDBNull(iAFERCTAEINFORMEFECHA)) entity.AFERCTAEINFORMEFECHA = dr.GetDateTime(iAFERCTAEINFORMEFECHA);
                        if (!dr.IsDBNull(iAFECONVCITACIONFECHA)) entity.AFECONVCITACIONFECHA = dr.GetDateTime(iAFECONVCITACIONFECHA);
                        if (!dr.IsDBNull(iAFEITPITFFECHASIST)) entity.AFEITPITFFECHASIST = dr.GetDateTime(iAFEITPITFFECHASIST);
                        if (!dr.IsDBNull(iAFEITPDECISFFECHASIST)) entity.AFEITPDECISFFECHASIST = dr.GetDateTime(iAFEITPDECISFFECHASIST);
                        if (!dr.IsDBNull(iAFECOMPFECHA)) entity.AFECOMPFECHA = dr.GetDateTime(iAFECOMPFECHA);
                        if (!dr.IsDBNull(iAFECOMPFECHASIST)) entity.AFECOMPFECHASIST = dr.GetDateTime(iAFECOMPFECHASIST);
                        if (!dr.IsDBNull(iAFEITDECFECHANOMINAL)) entity.AFEITDECFECHANOMINAL = dr.GetDateTime(iAFEITDECFECHANOMINAL);
                        if (!dr.IsDBNull(iAFEITDECFECHAELAB)) entity.AFEITDECFECHAELAB = dr.GetDateTime(iAFEITDECFECHAELAB);
                        if (!dr.IsDBNull(iAFEFZAFECHASIST)) entity.AFEFZAFECHASIST = dr.GetDateTime(iAFEFZAFECHASIST);
                        if (!dr.IsDBNull(iAFEFZADECISFECHASIST)) entity.AFEFZADECISFECHASIST = dr.GetDateTime(iAFEFZADECISFECHASIST);
                        if (!dr.IsDBNull(iAFEITPITFFECHA)) entity.AFEITPITFFECHA = dr.GetDateTime(iAFEITPITFFECHA);
                        if (!dr.IsDBNull(iAFEEMPRESPNINGUNA)) entity.AFEEMPRESPNINGUNA = dr.GetString(iAFEEMPRESPNINGUNA);
                        if (!dr.IsDBNull(iAFEEMPCOMPNINGUNA)) entity.AFEEMPCOMPNINGUNA = dr.GetString(iAFEEMPCOMPNINGUNA);
                        if (!dr.IsDBNull(iAFEESTADO)) entity.AFEESTADO = dr.GetString(iAFEESTADO);
                        if (!dr.IsDBNull(iAFEESTADOMOTIVO)) entity.AFEESTADOMOTIVO = dr.GetString(iAFEESTADOMOTIVO);
                        if (!dr.IsDBNull(iAFEFZAMAYOR)) entity.AFEFZAMAYOR = dr.GetString(iAFEFZAMAYOR);
                        if (!dr.IsDBNull(iCODIGO)) entity.CodigoEvento = dr.GetString(iCODIGO);
                        if (!dr.IsDBNull(iAFSALA)) entity.AFSALA = dr.GetInt32(iAFSALA);
                        if (!dr.IsDBNull(iAFEREUHORINI)) entity.AFEREUHORINI = dr.GetString(iAFEREUHORINI);
                        if (!dr.IsDBNull(iAFEREUHORFIN)) entity.AFEREUHORFIN = dr.GetString(iAFEREUHORFIN);
                        if (!dr.IsDBNull(iAFELIMATENCOMEN)) entity.AFELIMATENCOMEN = dr.GetDateTime(iAFELIMATENCOMEN);
                    }
                }
            }
            catch (Exception ex)
            {
                entity = null;
            }
            return entity;
        }

        public EquipoDTO ObtenerEquipoPorEvento(int id)
        {

            EquipoDTO entity = new EquipoDTO();
            try
            {
                string query = string.Format(helper.SqlObtenerEquipoPorEvento, id);
                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        int iEQUIABREV = dr.GetOrdinal("EQUIABREV");
                        int iAREANOMB = dr.GetOrdinal("AREANOMB");
                        int iEMPRNOMB = dr.GetOrdinal("EMPRNOMB");
                        int iDISMINUIDO = dr.GetOrdinal("DISMINUIDO");
                        int iINTERRUMPIDO = dr.GetOrdinal("INTERRUMPIDO");

                        if (!dr.IsDBNull(iEQUIABREV)) entity.EQUIABREV = dr.GetString(iEQUIABREV);
                        if (!dr.IsDBNull(iAREANOMB)) entity.AREANOMB = dr.GetString(iAREANOMB);
                        if (!dr.IsDBNull(iEMPRNOMB)) entity.EMPRENOMB = dr.GetString(iEMPRNOMB);
                        if (!dr.IsDBNull(iDISMINUIDO)) entity.DISMINUIDO = dr.GetDecimal(iDISMINUIDO);
                        if (!dr.IsDBNull(iINTERRUMPIDO)) entity.INTERRUMPIDO = dr.GetDecimal(iINTERRUMPIDO);

                    }
                }
            }
            catch (Exception ex)
            {
                entity = null;
            }
            return entity;
        }
        public List<EmpresaInvolucradaDTO> ObtenerEmpresasInvolucrada(int id)
        {
            List<EmpresaInvolucradaDTO> entitys = new List<EmpresaInvolucradaDTO>();
            try
            {
                string query = string.Format(helper.SqlObtenerEmpresaInvolucrada, id);
                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        EmpresaInvolucradaDTO entity = new EmpresaInvolucradaDTO();

                        int iEMPRCODI = dr.GetOrdinal("EMPRCODI");
                        int iAFIVERSION = dr.GetOrdinal("AFIVERSION");
                        int iEMPRNOMB = dr.GetOrdinal("EMPRNOMB");
                        int iVERSION = dr.GetOrdinal("VERSION");
                        int iCUMPLIMIENTO = dr.GetOrdinal("CUMPLIMIENTO");
                        int iAFIEXTENSION = dr.GetOrdinal("AFIEXTENSION");
                        int iAFIMENSAJE = dr.GetOrdinal("AFIMENSAJE");
                        int iAFIFECHAINF = dr.GetOrdinal("AFIFECHAINF");
                        int iLASTUSER = dr.GetOrdinal("LASTUSER");
                        int iLASTDATE = dr.GetOrdinal("LASTDATE");

                        if (!dr.IsDBNull(iEMPRCODI)) entity.EMPRCODI = dr.GetInt32(iEMPRCODI);
                        if (!dr.IsDBNull(iAFIVERSION)) entity.AFIVERSION = dr.GetString(iAFIVERSION);
                        if (!dr.IsDBNull(iEMPRNOMB)) entity.EMPRNOMB = dr.GetString(iEMPRNOMB);
                        if (!dr.IsDBNull(iVERSION)) entity.VERSION = dr.GetString(iVERSION);
                        if (!dr.IsDBNull(iCUMPLIMIENTO)) entity.CUMPLIMIENTO = dr.GetString(iCUMPLIMIENTO);
                        if (!dr.IsDBNull(iAFIEXTENSION)) entity.AFIEXTENSION = dr.GetString(iAFIEXTENSION);
                        if (!dr.IsDBNull(iAFIMENSAJE)) entity.AFIMENSAJE = dr.GetString(iAFIMENSAJE);
                        if (!dr.IsDBNull(iAFIFECHAINF)) entity.AFIFECHAINF = dr.GetDateTime(iAFIFECHAINF);
                        if (!dr.IsDBNull(iLASTUSER)) entity.LASTUSER = dr.GetString(iLASTUSER);
                        if (!dr.IsDBNull(iLASTDATE)) entity.LASTDATE = dr.GetDateTime(iLASTDATE);


                        entitys.Add(entity);
                    }
                }
            }
            catch (Exception ex)
            {
                entitys = null;
            }
            return entitys;
        }

        public List<EmpresaInvolucradaDTO> ObtenerEmpresasInvolucradaReunion(int id)
        {
            List<EmpresaInvolucradaDTO> entitys = new List<EmpresaInvolucradaDTO>();
            try
            {
                string query = string.Format(helper.SqlObtenerEmpresaInvolucradaReunion, id);
                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        EmpresaInvolucradaDTO entity = new EmpresaInvolucradaDTO();

                        int iEMPRCODI = dr.GetOrdinal("EMPRCODI");
                        int iEMPRNOMB = dr.GetOrdinal("EMPRNOMB");
                        int iLASTUSER = dr.GetOrdinal("LASTUSER");
                        int iLASTDATE = dr.GetOrdinal("LASTDATE");

                        if (!dr.IsDBNull(iEMPRCODI)) entity.EMPRCODI = dr.GetInt32(iEMPRCODI);
                        if (!dr.IsDBNull(iEMPRNOMB)) entity.EMPRNOMB = dr.GetString(iEMPRNOMB);
                        if (!dr.IsDBNull(iLASTUSER)) entity.LASTUSER = dr.GetString(iLASTUSER);
                        if (!dr.IsDBNull(iLASTDATE)) entity.LASTDATE = dr.GetDateTime(iLASTDATE);

                        entitys.Add(entity);
                    }
                }
            }
            catch (Exception ex)
            {
                entitys = null;
            }
            return entitys;
        }
        
       public List<SecuenciaEventoDTO> ObtenerListaSecuenciaEventos(int id)
        {
            List<SecuenciaEventoDTO> entitys = new List<SecuenciaEventoDTO>();
            try
            {
                string query = string.Format(helper.SqlObtenerListaSecuenciaEvento, id);
                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        SecuenciaEventoDTO entity = new SecuenciaEventoDTO();

                        int iEVESECUENCIACODI = dr.GetOrdinal("EVESECUENCIACODI");
                        int iEVENCODI = dr.GetOrdinal("EVENCODI");
                        int iEVESECFECHA = dr.GetOrdinal("EVESFECHA");
                        int iEVESECHORA = dr.GetOrdinal("EVESECHORA");
                        int iEVESECSECC = dr.GetOrdinal("EVESECSECC");
                        int iEVESECDESCRIPCION = dr.GetOrdinal("EVESECDESCRIPCION");
                        int iEVESECINCMANIOB = dr.GetOrdinal("EVESECINCMANIOB");
                        int iLASTDATE = dr.GetOrdinal("LASTDATE");
                        int iLASTUSER = dr.GetOrdinal("LASTUSER");

                        if (!dr.IsDBNull(iEVESECUENCIACODI)) entity.EVESECUENCIACODI = dr.GetInt32(iEVESECUENCIACODI);
                        if (!dr.IsDBNull(iEVENCODI)) entity.EVENCODI = dr.GetInt32(iEVENCODI);
                        if (!dr.IsDBNull(iEVESECFECHA)) entity.EVESECFECHA = dr.GetString(iEVESECFECHA);
                        if (!dr.IsDBNull(iEVESECHORA)) entity.EVESECHORA = dr.GetString(iEVESECHORA);
                        if (!dr.IsDBNull(iEVESECSECC)) entity.EVESECSECC = dr.GetString(iEVESECSECC);
                        if (!dr.IsDBNull(iEVESECDESCRIPCION)) entity.EVESECDESCRIPCION = dr.GetString(iEVESECDESCRIPCION);
                        if (!dr.IsDBNull(iEVESECINCMANIOB)) entity.EVESECINCMANIOBVALOR = dr.GetString(iEVESECINCMANIOB);
                        entity.EVESECINCMANIOB = entity.EVESECINCMANIOBVALOR == "1" ? true: false;
                        if (!dr.IsDBNull(iLASTDATE)) entity.LASTDATE = dr.GetDateTime(iLASTDATE);
                        if (!dr.IsDBNull(iLASTUSER)) entity.LASTUSER = dr.GetString(iLASTUSER);
                        entitys.Add(entity);
                    }
                }
            }
            catch (Exception ex)
            {
                entitys = null;
            }
            return entitys;
        }
        public List<ReunionResponsableDTO> ObtenerListaReunionResponsable(int id)
        {
            List<ReunionResponsableDTO> entitys = new List<ReunionResponsableDTO>();
            try
            {
                string query = string.Format(helper.SqlObtenerListaReunionResponsable, id);
                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        ReunionResponsableDTO entity = new ReunionResponsableDTO();

                        int iNOMBRE = dr.GetOrdinal("NOMBRE");
                        int iRESEVENCODI = dr.GetOrdinal("RESEVENCODI");
                        int iEVERESPONCODI = dr.GetOrdinal("EVERESPONCODI");
                        int iEVENCODI = dr.GetOrdinal("EVENCODI");
                        int iEVEPARTICIPANTE = dr.GetOrdinal("EVEPARTICIPANTE");
                        int iEMPRCODI = dr.GetOrdinal("EMPRCODI");
                        int iREPRUTAFIRMA = dr.GetOrdinal("REPRUTAFIRMA");

                        if (!dr.IsDBNull(iNOMBRE)) entity.RESPNAME = dr.GetString(iNOMBRE);
                        if (!dr.IsDBNull(iRESEVENCODI)) entity.RESEVENCODI = dr.GetInt32(iRESEVENCODI);
                        if (!dr.IsDBNull(iEVERESPONCODI)) entity.EVERESPONCODI = dr.GetInt32(iEVERESPONCODI);
                        if (!dr.IsDBNull(iEVENCODI)) entity.EVENCODI = dr.GetInt32(iEVENCODI);
                        if (!dr.IsDBNull(iEVEPARTICIPANTE)) entity.EVEPARTICIPANTE = dr.GetString(iEVEPARTICIPANTE);
                        if (!dr.IsDBNull(iEMPRCODI)) entity.EMPRCODI = dr.GetInt32(iEMPRCODI);
                        if (!dr.IsDBNull(iREPRUTAFIRMA)) entity.REPRUTAFIRMA = dr.GetString(iREPRUTAFIRMA);

                        entitys.Add(entity);
                    }
                }
            }
            catch (Exception ex)
            {
                entitys = null;
            }
            return entitys;
        }

        public List<EmpresaRecomendacionDTO> ObtenerEmpresaRecomendacionInformeTecnico(int id)
        {
            List<EmpresaRecomendacionDTO> entitys = new List<EmpresaRecomendacionDTO>();
            try
            {
                string query = string.Format(helper.SqlObtenerEmpresaRecomendacionInformeTecnico, id);
                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        EmpresaRecomendacionDTO entity = new EmpresaRecomendacionDTO();

                        int iEMPRCODI = dr.GetOrdinal("EMPRCODI");
                        int iAFRCORR = dr.GetOrdinal("AFRCORR");
                        int iEMPRNOMB = dr.GetOrdinal("EMPRNOMB");
                        int iAFRRECOMEND = dr.GetOrdinal("AFRRECOMEND");
                        int iLASTUSER = dr.GetOrdinal("LASTUSER");
                        int iLASTDATE = dr.GetOrdinal("LASTDATE");
                        int iAFRREC = dr.GetOrdinal("AFRREC");
                        int iEVENRCMCTAF = dr.GetOrdinal("EVENRCMCTAF");
                        int iAFECODI = dr.GetOrdinal("AFECODI");

                        if (!dr.IsDBNull(iEMPRCODI)) entity.EMPRCODI = dr.GetInt32(iEMPRCODI);
                        if (!dr.IsDBNull(iAFRCORR)) entity.AFRCORR = dr.GetInt32(iAFRCORR);
                        if (!dr.IsDBNull(iEMPRNOMB)) entity.EMPRNOMB = dr.GetString(iEMPRNOMB);
                        if (!dr.IsDBNull(iAFRRECOMEND)) entity.AFRRECOMEND = dr.GetString(iAFRRECOMEND);
                        if (!dr.IsDBNull(iLASTUSER)) entity.LASTUSER = dr.GetString(iLASTUSER);
                        if (!dr.IsDBNull(iLASTDATE)) entity.LASTDATE = dr.GetDateTime(iLASTDATE);
                        if (!dr.IsDBNull(iAFRREC)) entity.AFRREC = dr.GetInt32(iAFRREC);
                        if (!dr.IsDBNull(iEVENRCMCTAF)) entity.EVENRCMCTAF = dr.GetString(iEVENRCMCTAF);
                        if (!dr.IsDBNull(iAFECODI)) entity.AFECODI = dr.GetInt32(iAFECODI);

                        entitys.Add(entity);
                    }
                }
            }
            catch (Exception ex)
            {
                entitys = null;
            }
            return entitys;
        }
        public List<EmpresaObservacionDTO> ObtenerEmpresaObservacionInformeTecnico(int id)
        {
            List<EmpresaObservacionDTO> entitys = new List<EmpresaObservacionDTO>();
            try
            {
                string query = string.Format(helper.SqlObtenerEmpresaObservacionInformeTecnico, id);
                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        EmpresaObservacionDTO entity = new EmpresaObservacionDTO();

                        int iEMPRCODI = dr.GetOrdinal("EMPRCODI");
                        int iAFOCORR = dr.GetOrdinal("AFOCORR");
                        int iEMPRNOMB = dr.GetOrdinal("EMPRNOMB");
                        int iAFOOBSERVAC = dr.GetOrdinal("AFOOBSERVAC");
                        int iLASTUSER = dr.GetOrdinal("LASTUSER");
                        int iLASTDATE = dr.GetOrdinal("LASTDATE");
                        int iAFECODI = dr.GetOrdinal("AFECODI");
                        int iAFOOBS = dr.GetOrdinal("AFOOBS");

                        if (!dr.IsDBNull(iEMPRCODI)) entity.EMPRCODI = Convert.ToInt32(dr.GetDecimal(iEMPRCODI));
                        if (!dr.IsDBNull(iAFOCORR)) entity.AFOCORR = Convert.ToInt32(dr.GetDecimal(iAFOCORR));
                        if (!dr.IsDBNull(iEMPRNOMB)) entity.EMPRNOMB = dr.GetString(iEMPRNOMB);
                        if (!dr.IsDBNull(iAFOOBSERVAC)) entity.AFOOBSERVAC = dr.GetString(iAFOOBSERVAC);
                        if (!dr.IsDBNull(iLASTUSER)) entity.LASTUSER = dr.GetString(iLASTUSER);
                        if (!dr.IsDBNull(iLASTDATE)) entity.LASTDATE = dr.GetDateTime(iLASTDATE);
                        if (!dr.IsDBNull(iAFECODI)) entity.AFECODI = Convert.ToInt32(dr.GetDecimal(iAFECODI));
                        if (!dr.IsDBNull(iAFOOBS)) entity.AFOOBS = Convert.ToInt32(dr.GetDecimal(iAFOOBS));

                        entitys.Add(entity);
                    }
                }
            }
            catch (Exception ex)
            {
                entitys = null;
            }
            return entitys;
        }
        public List<EmpresaFuerzaMayorDTO> ObtenerEmpresaFuerzaMayorInformeTecnico(int id)
        {
            List<EmpresaFuerzaMayorDTO> entitys = new List<EmpresaFuerzaMayorDTO>();
            try
            {
                string query = string.Format(helper.SqlObtenerEmpresaFuerzaMayorInformeTecnico, id);
                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        EmpresaFuerzaMayorDTO entity = new EmpresaFuerzaMayorDTO();

                        int iRECLAMOCODI = dr.GetOrdinal("RECLAMOCODI");
                        int iRPTACODI = dr.GetOrdinal("RPTACODI");
                        int iEMPRCODI = dr.GetOrdinal("EMPRCODI");
                        int iEMPRNOMB = dr.GetOrdinal("EMPRNOMB");
                        int iFECHA = dr.GetOrdinal("FECHA");
                        int iLASTUSER = dr.GetOrdinal("LASTUSER");
                        int iLASTDATE = dr.GetOrdinal("LASTDATE");
                        int iFECHA_COES = dr.GetOrdinal("FECHA_COES");
                        int iLASTUSER_COES = dr.GetOrdinal("LASTUSER_COES");
                        int iLASTDATE_COES = dr.GetOrdinal("LASTDATE_COES");

                        if (!dr.IsDBNull(iRECLAMOCODI)) entity.RECLAMOCODI = dr.GetInt32(iRECLAMOCODI);
                        if (!dr.IsDBNull(iRPTACODI)) entity.RPTACODI = Convert.ToInt32(dr.GetDecimal(iRPTACODI));
                        if (!dr.IsDBNull(iEMPRCODI)) entity.EMPRCODI = Convert.ToInt32(dr.GetDecimal(iEMPRCODI));
                        if (!dr.IsDBNull(iEMPRNOMB)) entity.EMPRNOMB = dr.GetString(iEMPRNOMB);
                        if (!dr.IsDBNull(iFECHA)) entity.FECHA = dr.GetString(iFECHA);
                        if (!dr.IsDBNull(iLASTUSER)) entity.LASTUSER = dr.GetString(iLASTUSER);
                        if (!dr.IsDBNull(iLASTDATE)) entity.LASTDATE = dr.GetString(iLASTDATE);
                        if (!dr.IsDBNull(iFECHA_COES)) entity.FECHA_COES = dr.GetString(iFECHA_COES);
                        if (!dr.IsDBNull(iLASTUSER_COES)) entity.LASTUSER_COES = dr.GetString(iLASTUSER_COES);
                        if (!dr.IsDBNull(iLASTDATE_COES)) entity.LASTDATE_COES = dr.GetString(iLASTDATE_COES);

                        entitys.Add(entity);
                    }
                }
            }
            catch (Exception ex)
            {
                entitys = null;
            }
            return entitys;
        }
        public List<EmpresaResponsableDTO> ObtenerEmpresaResponsableCompensacion(int id)
        {
            List<EmpresaResponsableDTO> entitys = new List<EmpresaResponsableDTO>();
            try
            {
                string query = string.Format(helper.SqlObtenerEmpresaResponsableCompensacion, id);
                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        EmpresaResponsableDTO entity = new EmpresaResponsableDTO();

                        int iEMPRCODI = dr.GetOrdinal("EMPRCODI");
                        int iEMPRNOMB = dr.GetOrdinal("EMPRNOMB");
                        int iLASTUSER = dr.GetOrdinal("LASTUSER");
                        int iLASTDATE = dr.GetOrdinal("LASTDATE");
                        int iAFIPORCENTAJE = dr.GetOrdinal("AFIPORCENTAJE");
                        int iAFIDECIDE = dr.GetOrdinal("AFIDECIDE");
                        int iAFIMENSAJE = dr.GetOrdinal("AFIMENSAJE");

                        if (!dr.IsDBNull(iEMPRCODI)) entity.EMPRCODI = dr.GetInt32(iEMPRCODI);
                        if (!dr.IsDBNull(iEMPRNOMB)) entity.EMPRNOMB = dr.GetString(iEMPRNOMB);
                        if (!dr.IsDBNull(iLASTUSER)) entity.LASTUSER = dr.GetString(iLASTUSER);
                        if (!dr.IsDBNull(iLASTDATE)) entity.LASTDATE = dr.GetDateTime(iLASTDATE);
                        if (!dr.IsDBNull(iAFIPORCENTAJE)) entity.AFIPORCENTAJE = dr.GetDecimal(iAFIPORCENTAJE);
                        if (!dr.IsDBNull(iAFIDECIDE)) entity.AFIDECIDE = Convert.ToInt32(dr.GetDecimal(iAFIDECIDE));
                        if (!dr.IsDBNull(iAFIMENSAJE)) entity.AFIMENSAJE = dr.GetString(iAFIMENSAJE);

                        entitys.Add(entity);
                    }
                }
            }
            catch (Exception ex)
            {
                entitys = null;
            }
            return entitys;
        }
        public List<EmpresaResponsableDTO> ObtenerEmpresaCompensadaCompensacion(int id)
        {
            List<EmpresaResponsableDTO> entitys = new List<EmpresaResponsableDTO>();
            try
            {
                string query = string.Format(helper.SqlObtenerEmpresaCompensadaCompensacion, id);
                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        EmpresaResponsableDTO entity = new EmpresaResponsableDTO();

                        int iEMPRCODI = dr.GetOrdinal("EMPRCODI");
                        int iEMPRNOMB = dr.GetOrdinal("EMPRNOMB");
                        int iLASTUSER = dr.GetOrdinal("LASTUSER");
                        int iLASTDATE = dr.GetOrdinal("LASTDATE");
                        int iAFIPORCENTAJE = dr.GetOrdinal("AFIPORCENTAJE");
                        int iAFIDECIDE = dr.GetOrdinal("AFIDECIDE");
                        int iAFIMENSAJE = dr.GetOrdinal("AFIMENSAJE");

                        if (!dr.IsDBNull(iEMPRCODI)) entity.EMPRCODI = dr.GetInt32(iEMPRCODI);
                        if (!dr.IsDBNull(iEMPRNOMB)) entity.EMPRNOMB = dr.GetString(iEMPRNOMB);
                        if (!dr.IsDBNull(iLASTUSER)) entity.LASTUSER = dr.GetString(iLASTUSER);
                        if (!dr.IsDBNull(iLASTDATE)) entity.LASTDATE = dr.GetDateTime(iLASTDATE);
                        if (!dr.IsDBNull(iAFIPORCENTAJE)) entity.AFIPORCENTAJE = dr.GetDecimal(iAFIPORCENTAJE);
                        if (!dr.IsDBNull(iAFIDECIDE)) entity.AFIDECIDE = Convert.ToInt32(dr.GetDecimal(iAFIDECIDE));
                        if (!dr.IsDBNull(iAFIMENSAJE)) entity.AFIMENSAJE = dr.GetString(iAFIMENSAJE);

                        entitys.Add(entity);
                    }
                }
            }
            catch (Exception ex)
            {
                entitys = null;
            }
            return entitys;
        }
        public List<ReclamoDTO> ObtenerReclamoReconsideracionReconsideracion(int id)
        {
            List<ReclamoDTO> entitys = new List<ReclamoDTO>();
            try
            {
                string query = string.Format(helper.SqlObtenerReclamoReconsideracionReconsideracion, id);
                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        ReclamoDTO entity = new ReclamoDTO();

                        int iRECLAMOCODI = dr.GetOrdinal("RECLAMOCODI");
                        int iRPTACODI = dr.GetOrdinal("RPTACODI");
                        int iEMPRCODI = dr.GetOrdinal("EMPRCODI");
                        int iEMPRNOMB = dr.GetOrdinal("EMPRNOMB");
                        int iFECHA = dr.GetOrdinal("FECHA");
                        int iLASTUSER = dr.GetOrdinal("LASTUSER");
                        int iLASTDATE = dr.GetOrdinal("LASTDATE");
                        int iFECHA_COES = dr.GetOrdinal("FECHA_COES");
                        int iLASTUSER_COES = dr.GetOrdinal("LASTUSER_COES");
                        int iLASTDATE_COES = dr.GetOrdinal("LASTDATE_COES");

                        if (!dr.IsDBNull(iRECLAMOCODI)) entity.RECLAMOCODI = dr.GetInt32(iRECLAMOCODI);
                        if (!dr.IsDBNull(iRPTACODI)) entity.RPTACODI = Convert.ToInt32(dr.GetDecimal(iRPTACODI));
                        if (!dr.IsDBNull(iEMPRCODI)) entity.EMPRCODI = Convert.ToInt32(dr.GetDecimal(iEMPRCODI));
                        if (!dr.IsDBNull(iEMPRNOMB)) entity.EMPRNOMB = dr.GetString(iEMPRNOMB);
                        if (!dr.IsDBNull(iFECHA)) entity.FECHA = dr.GetString(iFECHA);
                        if (!dr.IsDBNull(iLASTUSER)) entity.LASTUSER = dr.GetString(iLASTUSER);
                        if (!dr.IsDBNull(iLASTDATE)) entity.LASTDATE = dr.GetString(iLASTDATE);
                        if (!dr.IsDBNull(iFECHA_COES)) entity.FECHA_COES = dr.GetString(iFECHA_COES);
                        if (!dr.IsDBNull(iLASTUSER_COES)) entity.LASTUSER_COES = dr.GetString(iLASTUSER_COES);
                        if (!dr.IsDBNull(iLASTDATE_COES)) entity.LASTDATE_COES = dr.GetString(iLASTDATE_COES);

                        entitys.Add(entity);
                    }
                }
            }
            catch (Exception ex)
            {
                entitys = null;
            }
            return entitys;
        }
        public List<ReclamoDTO> ObtenerReclamoApelacionReconsideracion(int id)
        {
            List<ReclamoDTO> entitys = new List<ReclamoDTO>();
            try
            {
                string query = string.Format(helper.SqlObtenerReclamoApelacionReconsideracion, id);
                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        ReclamoDTO entity = new ReclamoDTO();

                        int iRECLAMOCODI = dr.GetOrdinal("RECLAMOCODI");
                        int iRPTACODI = dr.GetOrdinal("RPTACODI");
                        int iEMPRCODI = dr.GetOrdinal("EMPRCODI");
                        int iEMPRNOMB = dr.GetOrdinal("EMPRNOMB");
                        int iFECHA = dr.GetOrdinal("FECHA");
                        int iLASTUSER = dr.GetOrdinal("LASTUSER");
                        int iLASTDATE = dr.GetOrdinal("LASTDATE");
                        int iFECHA_COES = dr.GetOrdinal("FECHA_COES");
                        int iLASTUSER_COES = dr.GetOrdinal("LASTUSER_COES");
                        int iLASTDATE_COES = dr.GetOrdinal("LASTDATE_COES");

                        if (!dr.IsDBNull(iRECLAMOCODI)) entity.RECLAMOCODI = dr.GetInt32(iRECLAMOCODI);
                        if (!dr.IsDBNull(iRPTACODI)) entity.RPTACODI = Convert.ToInt32(dr.GetDecimal(iRPTACODI));
                        if (!dr.IsDBNull(iEMPRCODI)) entity.EMPRCODI = Convert.ToInt32(dr.GetDecimal(iEMPRCODI));
                        if (!dr.IsDBNull(iEMPRNOMB)) entity.EMPRNOMB = dr.GetString(iEMPRNOMB);
                        if (!dr.IsDBNull(iFECHA)) entity.FECHA = dr.GetString(iFECHA);
                        if (!dr.IsDBNull(iLASTUSER)) entity.LASTUSER = dr.GetString(iLASTUSER);
                        if (!dr.IsDBNull(iLASTDATE)) entity.LASTDATE = dr.GetString(iLASTDATE);
                        if (!dr.IsDBNull(iFECHA_COES)) entity.FECHA_COES = dr.GetString(iFECHA_COES);
                        if (!dr.IsDBNull(iLASTUSER_COES)) entity.LASTUSER_COES = dr.GetString(iLASTUSER_COES);
                        if (!dr.IsDBNull(iLASTDATE_COES)) entity.LASTDATE_COES = dr.GetString(iLASTDATE_COES);

                        entitys.Add(entity);
                    }
                }
            }
            catch (Exception ex)
            {
                entitys = null;
            }
            return entitys;
        }
        public List<ReclamoDTO> ObtenerReclamoArbitrajeReconsideracion(int id)
        {
            List<ReclamoDTO> entitys = new List<ReclamoDTO>();
            try
            {
                string query = string.Format(helper.SqlObtenerReclamoArbitrajeReconsideracion, id);
                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        ReclamoDTO entity = new ReclamoDTO();

                        int iRECLAMOCODI = dr.GetOrdinal("RECLAMOCODI");
                        int iRPTACODI = dr.GetOrdinal("RPTACODI");
                        int iEMPRCODI = dr.GetOrdinal("EMPRCODI");
                        int iEMPRNOMB = dr.GetOrdinal("EMPRNOMB");
                        int iFECHA = dr.GetOrdinal("FECHA");
                        int iLASTUSER = dr.GetOrdinal("LASTUSER");
                        int iLASTDATE = dr.GetOrdinal("LASTDATE");
                        int iFECHA_COES = dr.GetOrdinal("FECHA_COES");
                        int iLASTUSER_COES = dr.GetOrdinal("LASTUSER_COES");
                        int iLASTDATE_COES = dr.GetOrdinal("LASTDATE_COES");

                        if (!dr.IsDBNull(iRECLAMOCODI)) entity.RECLAMOCODI = dr.GetInt32(iRECLAMOCODI);
                        if (!dr.IsDBNull(iRPTACODI)) entity.RPTACODI = Convert.ToInt32(dr.GetDecimal(iRPTACODI));
                        if (!dr.IsDBNull(iEMPRCODI)) entity.EMPRCODI = Convert.ToInt32(dr.GetDecimal(iEMPRCODI));
                        if (!dr.IsDBNull(iEMPRNOMB)) entity.EMPRNOMB = dr.GetString(iEMPRNOMB);
                        if (!dr.IsDBNull(iFECHA)) entity.FECHA = dr.GetString(iFECHA);
                        if (!dr.IsDBNull(iLASTUSER)) entity.LASTUSER = dr.GetString(iLASTUSER);
                        if (!dr.IsDBNull(iLASTDATE)) entity.LASTDATE = dr.GetString(iLASTDATE);
                        if (!dr.IsDBNull(iFECHA_COES)) entity.FECHA_COES = dr.GetString(iFECHA_COES);
                        if (!dr.IsDBNull(iLASTUSER_COES)) entity.LASTUSER_COES = dr.GetString(iLASTUSER_COES);
                        if (!dr.IsDBNull(iLASTDATE_COES)) entity.LASTDATE_COES = dr.GetString(iLASTDATE_COES);

                        entitys.Add(entity);
                    }
                }
            }
            catch (Exception ex)
            {
                entitys = null;
            }
            return entitys;
        }

        public bool InsertarSecuenciaEvento(SecuenciaEventoDTO entity)
        {
            int result = 0;
            string query = helper.SqlObtenerMaxIdSecuenciaEvento;
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            string MaxId = dbProvider.ExecuteScalar(command).ToString();
            int MaxIdSecuenciaEvento = Int32.Parse(MaxId) + 1;
            string queryInsert = string.Format(helper.SqlInsertarSecuenciaEvento, MaxIdSecuenciaEvento, entity.EVENCODI, entity.EVESECHORA, entity.EVESECSECC, entity.EVESECDESCRIPCION, entity.EVESECINCMANIOB ? 1 : 0, entity.LASTDATEstr, entity.LASTUSER, entity.EVESECFECHA);
            command = dbProvider.GetSqlStringCommand(queryInsert);
            result = dbProvider.ExecuteNonQuery(command);
            return result > 0;
        }

        public bool InsertarEmpresaInvolucrada(EmpresaInvolucradaDTO entity)
        {
            int result = 0;
            string query = string.Format(helper.SqlInsertarEmpresaInvolucrada, entity.AFECODI, entity.EMPRCODI, entity.AFIVERSION, entity.CUMPLIMIENTO, entity.AFIMENSAJE, entity.AFIEXTENSION, entity.AFIFECHAINFEVE, entity.AFIPUBLICA, entity.LASTUSER, entity.LASTDATEstr);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            result = dbProvider.ExecuteNonQuery(command);
            return result > 0;
        }

        public bool InsertarReunionResponsable(ReunionResponsableDTO entity)
        {
            int result = 0;
            string query = helper.SqlObtenerMaxIdReunionResponsable;
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            int MaxId = int.Parse(dbProvider.ExecuteScalar(command).ToString());

            command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlInsertarReunionResponsable, MaxId, entity.RESPCOD, entity.EVENCODI, entity.EVEPARTICIPANTE, entity.EMPRCODI));
            result = dbProvider.ExecuteNonQuery(command);
            return result > 0;
        }
        
        public List<EmpresaRecomendacionDTO> ConsultarRecomendacion(EmpresaRecomendacionDTO obj)
        {
            List<EmpresaRecomendacionDTO> entitys = new List<EmpresaRecomendacionDTO>();
            try
            {
                string query = string.Format(helper.SqlRecomendacionesConsultar, obj.FechaInicio, obj.FechaFin, obj.EMPRCODI, obj.ESTADO, obj.IMPORTANTE);
                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        EmpresaRecomendacionDTO entity = new EmpresaRecomendacionDTO();

                        int iAFRREC = dr.GetOrdinal("AFRREC");
                        int iAFECODI = dr.GetOrdinal("AFECODI");
                        int iCODIGO = dr.GetOrdinal("CODIGO");
                        int iEMPRNOMB = dr.GetOrdinal("EMPRNOMB");
                        int iRECOMENDACION = dr.GetOrdinal("RECOMENDACION");
                        int iESTADO = dr.GetOrdinal("ESTADO");
                        int iRESPUESTA = dr.GetOrdinal("RESPUESTA");
                        int iOBSERVACION = dr.GetOrdinal("OBSERVACION");
                        int iACCIONFINAL = dr.GetOrdinal("ACCION_FINAL");
                        int iINDIMPORTANTE = dr.GetOrdinal("INDIMPORTANTE");
                        int iNROREGRESPUESTA = dr.GetOrdinal("NROREGRESPUESTA");


                        if (!dr.IsDBNull(iAFRREC)) entity.AFRREC = dr.GetInt32(iAFRREC);
                        if (!dr.IsDBNull(iAFECODI)) entity.AFECODI = dr.GetInt32(iAFECODI);
                        if (!dr.IsDBNull(iCODIGO)) entity.CODIGO = dr.GetString(iCODIGO);
                        if (!dr.IsDBNull(iEMPRNOMB)) entity.EMPRNOMB = dr.GetString(iEMPRNOMB);
                        if (!dr.IsDBNull(iRECOMENDACION)) entity.RECOMENDACION = dr.GetString(iRECOMENDACION);
                        if (!dr.IsDBNull(iESTADO)) entity.ESTADO = dr.GetString(iESTADO);
                        if (!dr.IsDBNull(iRESPUESTA)) entity.RESPUESTA = dr.GetString(iRESPUESTA);
                        if (!dr.IsDBNull(iOBSERVACION)) entity.OBSERVACION = dr.GetString(iOBSERVACION);
                        if (!dr.IsDBNull(iACCIONFINAL)) entity.ACCIONFINAL = dr.GetString(iACCIONFINAL);
                        if (!dr.IsDBNull(iINDIMPORTANTE)) entity.INDIMPORTANTE = dr.GetString(iINDIMPORTANTE);
                        if (!dr.IsDBNull(iNROREGRESPUESTA)) entity.NROREGRESPUESTA = dr.GetString(iNROREGRESPUESTA);


                        entitys.Add(entity);
                    }
                }
            }
            catch (Exception ex)
            {
                entitys = null;
            }
            return entitys;
        }

        public List<EmpresaObservacionDTO> ConsultarObservacion(EmpresaObservacionDTO obj)
        {
            List<EmpresaObservacionDTO> entitys = new List<EmpresaObservacionDTO>();
            try
            {
                string query = string.Format(helper.SqlObservacionConsultar, obj.EMPRCODI, obj.FechaInicio, obj.FechaFin);
                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        EmpresaObservacionDTO entity = new EmpresaObservacionDTO();

                        int iAFOOBS = dr.GetOrdinal("AFOOBS");
                        int iAFECODI = dr.GetOrdinal("AFECODI");
                        int iAFOCORR = dr.GetOrdinal("AFOCORR");
                        int iEMPRCODI = dr.GetOrdinal("EMPRCODI");
                        int iAFOOBSERVAC = dr.GetOrdinal("AFOOBSERVAC");
                        int iLASTUSER = dr.GetOrdinal("LASTUSER");
                        int iLASTDATE = dr.GetOrdinal("LASTDATE");
                        int iEMPRNOMB = dr.GetOrdinal("EMPRNOMB");
                        int iEVENTO = dr.GetOrdinal("EVENTO");


                        if (!dr.IsDBNull(iAFOOBS)) entity.AFOOBS = dr.GetInt32(iAFOOBS);
                        if (!dr.IsDBNull(iAFECODI)) entity.AFECODI = dr.GetInt32(iAFECODI);
                        if (!dr.IsDBNull(iAFOCORR)) entity.AFOCORR = dr.GetInt32(iAFOCORR);
                        if (!dr.IsDBNull(iEMPRCODI)) entity.EMPRCODI = dr.GetInt32(iEMPRCODI);
                        if (!dr.IsDBNull(iAFOOBSERVAC)) entity.AFOOBSERVAC = dr.GetString(iAFOOBSERVAC);
                        if (!dr.IsDBNull(iLASTUSER)) entity.LASTUSER = dr.GetString(iLASTUSER);
                        if (!dr.IsDBNull(iLASTDATE)) entity.LASTDATE = dr.GetDateTime(iLASTDATE);
                        if (!dr.IsDBNull(iEMPRNOMB)) entity.EMPRNOMB = dr.GetString(iEMPRNOMB);
                        if (!dr.IsDBNull(iEVENTO)) entity.EVENTO = dr.GetString(iEVENTO);


                        entitys.Add(entity);
                    }
                }
            }
            catch (Exception ex)
            {
                entitys = null;
            }
            return entitys;
        }

        public bool EliminarEmpresaInvolucrada(EmpresaInvolucradaDTO entity)
        {
            int result = 0;
            string query = string.Format(helper.SqlEliminarEmpresaInvolucrada, entity.AFECODI, entity.EMPRCODI, entity.AFIVERSION);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            result = dbProvider.ExecuteNonQuery(command);
            return result > 0;
        }

        public bool EliminarSecuenciaEvento(int id)
        {
            int result = 0;
            string query = string.Format(helper.SqlEliminarSecuenciaEvento, id);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            result = dbProvider.ExecuteNonQuery(command);
            return result > 0;
        }

        public bool EliminarEmpresaInvolucradaReunion(EmpresaInvolucradaDTO entity)
        {
            int result = 0;
            string query = string.Format(helper.SqlEliminarEmpresaInvolucradaReunion, entity.AFECODI, entity.EMPRCODI);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            result = dbProvider.ExecuteNonQuery(command);
            return result > 0;
        }

        public bool EliminarReunionResponsable(ReunionResponsableDTO entity)
        {
            int result = 0;
            string query = string.Format(helper.SqlEliminarReunionResponsable, entity.EVENCODI, entity.RESEVENCODI);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            result = dbProvider.ExecuteNonQuery(command);
            return result > 0;
        }

        public bool EliminarAsistenteResponsable(ReunionResponsableDTO entity)
        {
            int result = 0;
            string query = string.Format(helper.SqlEliminarAsistenteResponsable, entity.EVENCODI, entity.EMPRCODI);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            result = dbProvider.ExecuteNonQuery(command);
            return result > 0;
        }
        

        public bool ActualizarFechaConvocatoriaCitacionReunion(int afecodi, string valor)
        {
            int result = 0;
            string query = string.Format(helper.SqlActualizarFechaConvocatoriaCitacionReunion, valor, afecodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            result = dbProvider.ExecuteNonQuery(command);
            return result > 0;
        }
        public bool ActualizarFechaActaReunion(int afecodi, string valor)
        {
            int result = 0;
            string query = string.Format(helper.SqlActualizarFechaActaReunion, valor, afecodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            result = dbProvider.ExecuteNonQuery(command);
            return result > 0;
        }
        public bool ActualizarFechaInformeCTAFReunion(int afecodi, string valor)
        {
            int result = 0;
            string query = string.Format(helper.SqlActualizarFechaInformeCTAFReunion, valor, afecodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            result = dbProvider.ExecuteNonQuery(command);
            return result > 0;
        }
        public bool ExisteRecomendacionInformeTecnico(EmpresaRecomendacionDTO entity)
        {
            int result = 0;
            string query = string.Format(helper.SqlExisteRecomendacionInformeTecnico, entity.AFECODI, entity.EMPRCODI, entity.AFRCORR, entity.AFRRECOMEND);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            result = int.Parse(dbProvider.ExecuteScalar(command).ToString());
            return result > 0;
        }

        public bool ExisteEmpresaResponsableCompensacion(EmpresaResponsableDTO entity)
        {
            int result = 0;
            string query = string.Format(helper.SqlExisteEmpresaResponsableCompensacion, entity.AFECODI, entity.EMPRCODI, entity.AFIVERSION, entity.AFIDECIDE);

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            result = int.Parse(dbProvider.ExecuteScalar(command).ToString());
            return result > 0;
        }

        public bool ExisteEmpresaCompensadaCompensacion(EmpresaResponsableDTO entity)
        {
            int result = 0;
            string query = string.Format(helper.SqlExisteEmpresaCompensadaCompensacion, entity.AFECODI, entity.EMPRCODI, entity.AFIVERSION);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            result = int.Parse(dbProvider.ExecuteScalar(command).ToString());
            return result > 0;
        }
        public bool ExisteResponsableReunion(ReunionResponsableDTO entity)
        {
            int result = 0;
            string query = string.Format(helper.SqlExisteReunionResponsable, entity.RESPCOD, entity.EVENCODI);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            result = int.Parse(dbProvider.ExecuteScalar(command).ToString());
            return result > 0;
        }
        public bool ExisteAsistenteResponsable(ReunionResponsableDTO entity)
        {
            int result = 0;
            string query = string.Format(helper.SqlExisteAsistenteResponsable, entity.EMPRCODI, entity.EVENCODI, entity.EVEPARTICIPANTE);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            result = int.Parse(dbProvider.ExecuteScalar(command).ToString());
            return result > 0;
        }

        public bool InsertarRecomendacionInformeTecnico(EmpresaRecomendacionDTO entity)
        {
            int result = 0;
            string query = helper.SqlObtenerMaxIdRecomendacionInformeTecnico;
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            int MaxId = int.Parse(dbProvider.ExecuteScalar(command).ToString()) + 1;

            command = dbProvider.GetSqlStringCommand(string.Format(helper.InsertarRecomendacionInformeTecnicoMaximoCorrelativo, entity.AFECODI, entity.EMPRCODI));
            int MaxCorrelativo = int.Parse(dbProvider.ExecuteScalar(command).ToString()) + 1;

            command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlInsertarRecomendacionInformeTecnico, MaxId, entity.AFECODI, entity.EMPRCODI, MaxCorrelativo, entity.AFRRECOMEND, entity.LASTUSER, entity.LASTDATEReg, entity.IDEQUIPO, entity.IDSUBESTACION));
            result = dbProvider.ExecuteNonQuery(command);
            return result > 0;
        }


        public bool EliminarRecomendacionInformeTecnico(EmpresaRecomendacionDTO entity)
        {
            int result = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlEliminarRecomendacionInformeTecnico, entity.AFECODI, entity.EMPRCODI, entity.AFRCORR));
            result = dbProvider.ExecuteNonQuery(command);
            return result > 0;
        }

        public bool InsertarObservacionInformeTecnico(EmpresaObservacionDTO entity)
        {
            int result = 0;
            string query = helper.SqlObtenerMaxIdObservacionInformeTecnico;
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            int MaxId = int.Parse(dbProvider.ExecuteScalar(command).ToString()) + 1;
            command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlInsertarObservacionInformeTecnico, MaxId, entity.AFECODI, entity.EMPRCODI, entity.AFOCORR, entity.AFOOBSERVAC, entity.LASTUSER, entity.LASTDATEReg));
            result = dbProvider.ExecuteNonQuery(command);
            return result > 0;
        }
        public bool EliminarObservacionInformeTecnico(int AFOOBS)
        {
            int result = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlEliminarObservacionInformeTecnico, AFOOBS));
            result = dbProvider.ExecuteNonQuery(command);
            return result > 0;
        }
        public bool ActualizarPublicacionInformeTecnicoAnualInformeTecnico(int afecodi, string AFEITPITFFECHA, string AFEITPITFFECHASIST)
        {
            int result = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlActualizarPublicacionInformeTecnicoAnualInformeTecnico, AFEITPITFFECHA, AFEITPITFFECHASIST, afecodi));
            result = dbProvider.ExecuteNonQuery(command);
            return result > 0;
        }
        public bool ActualizarPublicacionDesicionEventoInformeTecnico(int afecodi, string AFEITPDECISFFECHASIST)
        {
            int result = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlActualizarPublicacionDesicionEventoInformeTecnico, AFEITPDECISFFECHASIST, afecodi));
            result = dbProvider.ExecuteNonQuery(command);
            return result > 0;
        }

        public int InsertarFuerzaMayorInformeTecnico(EmpresaFuerzaMayorDTO entity)
        {

            string query = helper.SqlObtenerMaxIdReclamoInformeTecnico;
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            int MaxId = int.Parse(dbProvider.ExecuteScalar(command).ToString()) + 1;
            command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlInsertarFuerzaMayorInformeTecnico, MaxId, entity.AFECODI, entity.TIPCODI, entity.EMPRCODI, entity.AFREMPFECHA, entity.AFREMPPUBLICALASTUSER, entity.AFREMPPUBLICALASTDATE));
            dbProvider.ExecuteNonQuery(command);
            return MaxId;
        }
        public int ActualizarFuerzaMayorInformeTecnico(EmpresaFuerzaMayorDTO entity)
        {
            string query = helper.SqlObtenerMaxIdReclamoRespuestaInformeTecnico;
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            int MaxId = int.Parse(dbProvider.ExecuteScalar(command).ToString()) + 1;

            if (entity.FECHA_COES == "NULL")
            {
                MaxId = -1;
            }

            command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlActualizarFuerzaMayorInformeTecnico, entity.RECLAMOCODI, entity.FECHA_COES, entity.AFRCOESPUBLICALASTUSER, MaxId, entity.AFRCOESPUBLICALASTDATE));
            dbProvider.ExecuteNonQuery(command);
            return MaxId;
        }
        public bool EliminarFuerzaMayorInformeTecnico(int RECLAMOCODI)
        {
            int result = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlEliminarFuerzaMayorInformeTecnico, RECLAMOCODI));
            result = dbProvider.ExecuteNonQuery(command);
            return result > 0;
        }

        public bool InsertarEmpresaResponsableCompensacion(EmpresaResponsableDTO entity)
        {
            int resul = -1;
            string query = helper.SqlObtenerMaxIdReclamoInformeTecnico;
            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlInsertarEmpresaResponsableCompensacion, entity.AFECODI, entity.EMPRCODI, entity.AFIVERSION, entity.AFIFECHAINF, entity.AFIPUBLICA, entity.LASTUSER, entity.LASTDATEReg, entity.AFIPORCENTAJE, entity.AFIDECIDE, entity.AFIMENSAJE));
            resul = dbProvider.ExecuteNonQuery(command);
            return resul > 0;
        }
        public bool EliminarEmpresaResponsableCompensacion(EmpresaResponsableDTO entity)
        {
            int resul = -1;
            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlEliminarEmpresaResponsableCompensacion, entity.AFECODI, entity.EMPRCODI, entity.AFIVERSION, entity.AFIDECIDE));
            resul = dbProvider.ExecuteNonQuery(command);
            return resul > 0;
        }

        public bool ActualizarInformeCompensaciones(int afecodi, string AFECOMPFECHASIST, string AFECOMPFECHA)
        {
            int result = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlActualizarInformeCompensaciones, AFECOMPFECHASIST, AFECOMPFECHA, afecodi));
            result = dbProvider.ExecuteNonQuery(command);
            return result > 0;
        }

        public int InsertarReclamoRecApe(ReclamoDTO entity)
        {
            string query = helper.SqlObtenerMaxIdReclamoInformeTecnico;
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            int MaxId = int.Parse(dbProvider.ExecuteScalar(command).ToString()) + 1;
            command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlInsertarReclamoRecApe, MaxId, entity.AFECODI, entity.TIPCODI, entity.EMPRCODI, entity.AFREMPFECHA, entity.AFREMPPUBLICALASTUSER, entity.AFREMPPUBLICALASTDATE));
            dbProvider.ExecuteNonQuery(command);
            return MaxId;
        }
        public int ActualizarReclamoRecApe(ReclamoDTO entity)
        {
            string query = helper.SqlObtenerMaxIdReclamoRespuestaInformeTecnico;
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            int MaxId = int.Parse(dbProvider.ExecuteScalar(command).ToString()) + 1;

            if (entity.RPTACODI == -1)
            {
                MaxId = -1;
            }

            command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlActualizarReclamoRecApe, entity.RECLAMOCODI, entity.FECHA_COES, entity.AFRCOESPUBLICALASTUSER, MaxId, entity.AFRCOESPUBLICALASTDATE));
            dbProvider.ExecuteNonQuery(command);
            return MaxId;
        }

        public bool EliminarReclamoRecApe(int reclamocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlEliminarReclamoRecApe, reclamocodi));
            int result = dbProvider.ExecuteNonQuery(command);
            return result > 0;
        }
        public bool ActualizarEvento(AnalisisFallaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlActualizarEvento, entity.AFECODI, entity.AFECORR, entity.AFERMC, entity.AFEERACMF, entity.AFERACMT, entity.AFEEDAGSF,
                entity.AFERESPONSABLE, entity.AFECITFECHAELABU, entity.AFEREUFECHAPROGU, entity.AFEREUHORAPROG, entity.AFECONVTIPOREUNION,
                entity.AFERCTAEOBSERVACION, entity.AFEITFECHAELABU, entity.AFEITRDJRFECHAENVIOU, entity.AFEITRDJRFECHARECEPU,
                entity.AFEITRDOFECHAENVIOU, entity.AFEITRDOFECHARECEPU,//16
                entity.AFEITRDJRESTADO, entity.AFEITRDOESTADO, entity.LASTUSER, entity.LASTDATEstr,//20
                entity.AFEIMPUGNA, entity.AFEFZAMAYOR, entity.AFEITDECFECHANOMINALU, entity.AFEITDECFECHAELABU,
                entity.AFEEMPRESPNINGUNA, entity.AFEEMPCOMPNINGUNA, entity.AFEESTADO, entity.AFEESTADOMOTIVO, entity.AFEITPITFFECHAFecha));
            int result = dbProvider.ExecuteNonQuery(command);
            return result > 0;
        }


        public AnalisisFallaDTO ObtenerMedidasAdoptadas(int id)
        {

            AnalisisFallaDTO entity = new AnalisisFallaDTO();
            try
            {
                string query = string.Format(helper.SqlObtenerMedidasAdoptadas, id);
                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        int iafeanio = dr.GetOrdinal("afeanio");
                        int iafecorr = dr.GetOrdinal("afecorr");
                        int iEVENASUNTO = dr.GetOrdinal("EVENASUNTO");
                        int iEMPRNOMB = dr.GetOrdinal("EMPRNOMB");
                        int iAFRRECOMEND = dr.GetOrdinal("AFRRECOMEND");
                        int iAFRPUBLICAFECHA = dr.GetOrdinal("AFRPUBLICAFECHA");
                        int iLASTDATE = dr.GetOrdinal("LASTDATE");
                        int iAFRMEDADOPFECHA = dr.GetOrdinal("AFRMEDADOPFECHA");
                        int iAFRMEDADOPMEDIDA = dr.GetOrdinal("AFRMEDADOPMEDIDA");
                        int iAFRMEDADOPNIVCUMP = dr.GetOrdinal("AFRMEDADOPNIVCUMP");
                        int iLASTUSERRPTA = dr.GetOrdinal("LASTUSERRPTA");
                        int iLASTDATERPTA = dr.GetOrdinal("LASTDATERPTA");
                        int iINDIMPORTANTE = dr.GetOrdinal("INDIMPORTANTE");
                        int iNROREGRESPUESTA = dr.GetOrdinal("NROREGRESPUESTA");


                        if (!dr.IsDBNull(iafeanio)) entity.AFEANIO = dr.GetInt32(iafeanio);
                        if (!dr.IsDBNull(iafecorr)) entity.AFECORR = dr.GetInt32(iafecorr);
                        if (!dr.IsDBNull(iEVENASUNTO)) entity.EVENASUNTO = dr.GetString(iEVENASUNTO);
                        if (!dr.IsDBNull(iEMPRNOMB)) entity.EMPRNOMB = dr.GetString(iEMPRNOMB);
                        if (!dr.IsDBNull(iAFRRECOMEND)) entity.AFRRECOMEND = dr.GetString(iAFRRECOMEND);
                        if (!dr.IsDBNull(iAFRPUBLICAFECHA)) entity.AFRPUBLICAFECHA = dr.GetDateTime(iAFRPUBLICAFECHA);

                        if (!dr.IsDBNull(iLASTDATE)) entity.LASTDATE = dr.GetDateTime(iLASTDATE);
                        if (!dr.IsDBNull(iAFRMEDADOPFECHA)) entity.AFRMEDADOPFECHA = dr.GetDateTime(iAFRMEDADOPFECHA);
                        if (!dr.IsDBNull(iAFRMEDADOPMEDIDA)) entity.AFRMEDADOPMEDIDA = dr.GetString(iAFRMEDADOPMEDIDA);
                        if (!dr.IsDBNull(iAFRMEDADOPNIVCUMP)) entity.AFRMEDADOPNIVCUMP = dr.GetString(iAFRMEDADOPNIVCUMP);

                        if (!dr.IsDBNull(iLASTUSERRPTA)) entity.LASTUSERRPTA = dr.GetString(iLASTUSERRPTA);
                        if (!dr.IsDBNull(iLASTDATERPTA)) entity.LASTDATERPTA = dr.GetDateTime(iLASTDATERPTA);
                        if (!dr.IsDBNull(iINDIMPORTANTE)) entity.INDIMPORTANTE = dr.GetString(iINDIMPORTANTE);
                        if (!dr.IsDBNull(iNROREGRESPUESTA)) entity.NROREGRESPUESTA = dr.GetString(iNROREGRESPUESTA);




                    }
                }
            }
            catch (Exception ex)
            {
                entity = null;
            }
            return entity;
        }

        public bool ActualizarRecomendacionMA(AnalisisFallaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlActualizarRecomendacionMA, entity.INDIMPORTANTE, entity.NROREGRESPUESTA, entity.AFRREC));
            int result = dbProvider.ExecuteNonQuery(command);
            return result > 0;
        }
        public bool ActualizarRecomendacionMAG(AnalisisFallaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlActualizarRecomendacionMAG, entity.AFRMEDADOPFECHAU, entity.AFRMEDADOPMEDIDA, entity.AFRMEDADOPNIVCUMP, entity.LASTUSERRPTA, entity.LASTDATEstr, entity.AFRREC));
            int result = dbProvider.ExecuteNonQuery(command);
            return result > 0;
        }
        public bool ActualizarCartaRecomendacionCOES(AnalisisFallaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlActualizarCartaRecomendacionCOES, entity.LASTDATEstr, entity.AFRPUBLICAFECHAU, entity.LASTUSER, entity.AFRREC));
            int result = dbProvider.ExecuteNonQuery(command);
            return result > 0;
        }

        public bool ActualizarCartaRespuesta(AnalisisFallaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlActualizarCartaRespuesta, entity.AFRMEDADOPFECHAU, entity.AFRMEDADOPMEDIDA, entity.AFRMEDADOPNIVCUMP, entity.LASTUSER, entity.LASTDATEU, entity.AFRREC));
            int result = dbProvider.ExecuteNonQuery(command);
            return result > 0;
        }

        public List<InformeCTAFDTO> ObtenerCTAFINFORMEREPORTE(int id)
        {
            List<InformeCTAFDTO> entitys = new List<InformeCTAFDTO>();

            try
            {
                string query = string.Format(helper.SqlObtenerCTAFINFORMEREPORTE, id);
                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    InformeCTAFDTO entity = null;

                    int irownum = dr.GetOrdinal("rownum");
                    int iEMPRABREV = dr.GetOrdinal("EMPRABREV");
                    int inombre = dr.GetOrdinal("nombre");
                    int ifecha = dr.GetOrdinal("fecha");

                    while (dr.Read())
                    {

                        entity = new InformeCTAFDTO();

                        if (!dr.IsDBNull(irownum)) entity.RowNumber = dr.GetInt32(irownum);
                        if (!dr.IsDBNull(iEMPRABREV)) { entity.EMPRABREV = dr.GetString(iEMPRABREV); } else { entity.EMPRABREV = ""; };
                        if (!dr.IsDBNull(inombre)) { entity.NOMBRE = dr.GetString(inombre); } else { entity.NOMBRE = ""; };
                        if (!dr.IsDBNull(ifecha)) { entity.FECHA = dr.GetString(ifecha); } else { entity.FECHA = ""; };
                        entitys.Add(entity);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return entitys;
        }
        public List<SecuenciaCTAFDTO> ObtenerSecuenciaEventoREPORTE(int id)
        {
            List<SecuenciaCTAFDTO> entitys = new List<SecuenciaCTAFDTO>();

            try
            {
                string query = string.Format(helper.SqlObtenerSecuenciaEventoREPORTE, id);
                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    SecuenciaCTAFDTO entity = null;

                    int iSEFHORA1 = dr.GetOrdinal("SEFHORA1");
                    int id1 = dr.GetOrdinal("d1");
                    int iSEDESCRIP = dr.GetOrdinal("SEDESCRIP");

                    while (dr.Read())
                    {

                        entity = new SecuenciaCTAFDTO();

                        if (!dr.IsDBNull(iSEFHORA1)) { entity.SEFHORA1 = dr.GetString(iSEFHORA1); } else { entity.SEFHORA1 = ""; };
                        if (!dr.IsDBNull(id1)) { entity.D1 = dr.GetString(id1); } else { entity.D1 = ""; };
                        if (!dr.IsDBNull(iSEDESCRIP)) { entity.SEDESCRIP = dr.GetString(iSEDESCRIP); } else { entity.SEDESCRIP = ""; };

                        entitys.Add(entity);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return entitys;
        }

        public List<SecuenciaCTAFDTO> ObtenerSecuenciaEventoREPORTEv2(int id)
        {
            List<SecuenciaCTAFDTO> entitys = new List<SecuenciaCTAFDTO>();

            try
            {
                string query = string.Format(helper.SqlObtenerSecuenciaEventoREPORTEv2, id);
                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    SecuenciaCTAFDTO entity = null;

                    int iSEFHORA1 = dr.GetOrdinal("EVESECHORA");
                    int id1 = dr.GetOrdinal("EVESECSECC");
                    int iSEDESCRIP = dr.GetOrdinal("EVESECDESCRIPCION");

                    while (dr.Read())
                    {

                        entity = new SecuenciaCTAFDTO();

                        if (!dr.IsDBNull(iSEFHORA1)) { entity.SEFHORA1 = dr.GetString(iSEFHORA1); } else { entity.SEFHORA1 = ""; };
                        if (!dr.IsDBNull(id1)) { entity.D1 = dr.GetString(id1); } else { entity.D1 = ""; };
                        if (!dr.IsDBNull(iSEDESCRIP)) { entity.SEDESCRIP = dr.GetString(iSEDESCRIP); } else { entity.SEDESCRIP = ""; };

                        entitys.Add(entity);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return entitys;
        }

        public List<SecuenciaCTAFDTO> ObtenerSecuenciaEventoREPORTEv3(int id)
        {
            List<SecuenciaCTAFDTO> entitys = new List<SecuenciaCTAFDTO>();

            try
            {
                string query = string.Format(helper.SqlObtenerSecuenciaEventoREPORTEv3, id);
                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    SecuenciaCTAFDTO entity = null;

                    int iSEFHORA1 = dr.GetOrdinal("EVESECHORA");
                    int id1 = dr.GetOrdinal("EVESECSECC");
                    int iSEDESCRIP = dr.GetOrdinal("EVESECDESCRIPCION");
                    int iEVESFECHA = dr.GetOrdinal("EVESFECHA");

                    while (dr.Read())
                    {

                        entity = new SecuenciaCTAFDTO();

                        if (!dr.IsDBNull(iSEFHORA1)) { entity.SEFHORA1 = dr.GetString(iSEFHORA1); } else { entity.SEFHORA1 = ""; };
                        if (!dr.IsDBNull(id1)) { entity.D1 = dr.GetString(id1); } else { entity.D1 = ""; };
                        if (!dr.IsDBNull(iSEDESCRIP)) { entity.SEDESCRIP = dr.GetString(iSEDESCRIP); } else { entity.SEDESCRIP = ""; };
                        if (!dr.IsDBNull(iEVESFECHA)) { entity.EVESFECHA = dr.GetString(iEVESFECHA); } else { entity.EVESFECHA = ""; };
                        entitys.Add(entity);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return entitys;
        }
        
        public List<SenalizacionCTAFDTO> ObtenerSenalizacionREPORTE(int id)
        {
            List<SenalizacionCTAFDTO> entitys = new List<SenalizacionCTAFDTO>();

            try
            {
                string query = string.Format(helper.SqlObtenerSenalizacionREPORTE, id);
                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    SenalizacionCTAFDTO entity = null;

                    int iAREANOMB = dr.GetOrdinal("AREANOMB");
                    int iEQUIABREV = dr.GetOrdinal("EQUIABREV");
                    int iPRNOMBP = dr.GetOrdinal("PRNOMBP");
                    int iPRSENALIZ = dr.GetOrdinal("PRSENALIZ");
                    int iPSINTERRUPT = dr.GetOrdinal("PSINTERRUPT");
                    int iPRAC = dr.GetOrdinal("PRAC");


                    while (dr.Read())
                    {

                        entity = new SenalizacionCTAFDTO();

                        if (!dr.IsDBNull(iAREANOMB)) { entity.AREANOMB = dr.GetString(iAREANOMB); } else { entity.AREANOMB = ""; };
                        if (!dr.IsDBNull(iEQUIABREV)) { entity.EQUIABREV = dr.GetString(iEQUIABREV); } else { entity.EQUIABREV = ""; };
                        if (!dr.IsDBNull(iPRNOMBP)) { entity.PRNOMBP = dr.GetString(iPRNOMBP); } else { entity.PRNOMBP = ""; };
                        if (!dr.IsDBNull(iPRSENALIZ)) { entity.PRSENALIZ = dr.GetString(iPRSENALIZ); } else { entity.PRSENALIZ = ""; };
                        if (!dr.IsDBNull(iPSINTERRUPT)) { entity.PSINTERRUPT = dr.GetString(iPSINTERRUPT); } else { entity.PSINTERRUPT = ""; };
                        if (!dr.IsDBNull(iPRAC)) { entity.PRAC = dr.GetString(iPRAC); } else { entity.PRAC = ""; };

                        entitys.Add(entity);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return entitys;
        }
        public List<SuministroCTAFDTO> ObtenerSuministroREPORTE(int id)
        {
            List<SuministroCTAFDTO> entitys = new List<SuministroCTAFDTO>();

            try
            {
                string query = string.Format(helper.SqlObtenerSuministroREPORTE, id);
                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    SuministroCTAFDTO entity = null;

                    int iptointerrupnomb = dr.GetOrdinal("ptointerrupnomb");
                    int iareanomb = dr.GetOrdinal("areanomb");
                    int iinterrmw = dr.GetOrdinal("interrmw");
                    int ihini = dr.GetOrdinal("hini");
                    int ihfin = dr.GetOrdinal("hfin");
                    int iround = dr.GetOrdinal("round");


                    while (dr.Read())
                    {

                        entity = new SuministroCTAFDTO();

                        if (!dr.IsDBNull(iptointerrupnomb)) { entity.ptointerrupnomb = dr.GetString(iptointerrupnomb); } else { entity.ptointerrupnomb = ""; };
                        if (!dr.IsDBNull(iareanomb)) { entity.areanomb = dr.GetString(iareanomb); } else { entity.areanomb = ""; };
                        if (!dr.IsDBNull(iinterrmw)) { entity.interrmw = dr.GetString(iinterrmw); } else { entity.interrmw = ""; };
                        if (!dr.IsDBNull(ihini)) { entity.hini = dr.GetString(ihini); } else { entity.hini = ""; };
                        if (!dr.IsDBNull(ihfin)) { entity.hfin = dr.GetString(ihfin); } else { entity.hfin = ""; };
                        if (!dr.IsDBNull(iround)) entity.round = dr.GetInt32(iround);


                        entitys.Add(entity);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return entitys;
        }

        public List<EventoDTO> ObtenerEventoCitacion(int id)
        {
            List<EventoDTO> entitys = new List<EventoDTO>();

            try
            {
                string query = string.Format(helper.SqlObtenerEventoCitacion, id);
                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    EventoDTO entity = null;

                    int iCODIGO = dr.GetOrdinal("CODIGO");
                    int ifecha = dr.GetOrdinal("fecha");
                    int ievenasunto = dr.GetOrdinal("evenasunto");
                    int iempresas = dr.GetOrdinal("empresas");
                    int iTipo = dr.GetOrdinal("Tipo");


                    while (dr.Read())
                    {

                        entity = new EventoDTO();

                        if (!dr.IsDBNull(iCODIGO)) { entity.CODIGO = dr.GetString(iCODIGO); } else { entity.CODIGO = ""; };
                        if (!dr.IsDBNull(ifecha)) { entity.FECHA_EVENTO = dr.GetString(ifecha); } else { entity.FECHA_EVENTO = ""; };
                        if (!dr.IsDBNull(ievenasunto)) { entity.EVENASUNTO = dr.GetString(ievenasunto); } else { entity.EVENASUNTO = ""; };
                        if (!dr.IsDBNull(iempresas)) { entity.EmpresaInvolucrada = dr.GetString(iempresas); } else { entity.EmpresaInvolucrada = ""; };
                        if (!dr.IsDBNull(iTipo)) { entity.EVENTIPOFALLA = dr.GetString(iTipo); } else { entity.EVENTIPOFALLA = ""; };


                        entitys.Add(entity);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return entitys;
        }

        public List<SiDirectorioDTO> ObtenerListadoDirectores()
        {

            List<SiDirectorioDTO> entitys = new List<SiDirectorioDTO>();
            try
            {
                string query = helper.SqlObtenerListaDirectores;
                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    SiDirectorioDTO dirNiguno = new SiDirectorioDTO();
                    dirNiguno.DirectorioCodigo = 0;
                    dirNiguno.DirectorioNombre = "-SELECCIONE-";
                    entitys.Add(dirNiguno);

                    while (dr.Read())
                    {
                        SiDirectorioDTO entity = new SiDirectorioDTO();

                        int iCodigoDirector = dr.GetOrdinal(helper.CodigoDirector);
                        if (!dr.IsDBNull(iCodigoDirector)) entity.DirectorioCodigo = dr.GetInt32(iCodigoDirector);

                        int iNombreDirector = dr.GetOrdinal(helper.NombreCompletoDirector);
                        if (!dr.IsDBNull(iNombreDirector)) entity.DirectorioNombre = dr.GetString(iNombreDirector);

                        entitys.Add(entity);
                    }
                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex); ;
            }
            return entitys;
        }

        public List<SiResponsableDTO> ObtenerListadoResponsables(string Estado, string NombreApellidos)
        {

            List<SiResponsableDTO> entitys = new List<SiResponsableDTO>();
            try
            {
                string query = string.Format(helper.SqlObtenerListaResponsables, Estado, NombreApellidos);
                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        SiResponsableDTO entity = new SiResponsableDTO();

                        int iCodigoResponsable = dr.GetOrdinal(helper.CodigoResponsable);
                        if (!dr.IsDBNull(iCodigoResponsable)) entity.CodigoResponsable = dr.GetInt32(iCodigoResponsable);

                        int iCodigoDirector = dr.GetOrdinal(helper.CodigoDirector);
                        if (!dr.IsDBNull(iCodigoDirector)) entity.CodigoDirector = dr.GetInt32(iCodigoDirector);

                        int iNombreCompleto = dr.GetOrdinal(helper.NombreCompletoResponsable);
                        if (!dr.IsDBNull(iNombreCompleto)) entity.NombreCompleto = dr.GetString(iNombreCompleto);

                        int iEstado = dr.GetOrdinal(helper.EstadoResponsable);
                        if (!dr.IsDBNull(iEstado)) entity.Estado = dr.GetString(iEstado);

                        int iNombreArchivoFirma = dr.GetOrdinal(helper.NombreArchivoFirma);
                        if (!dr.IsDBNull(iNombreArchivoFirma)) entity.NombreArchivoFirma = dr.GetString(iNombreArchivoFirma);

                        int iRepfirma = dr.GetOrdinal(helper.Repfirma);
                        if (!dr.IsDBNull(iRepfirma)) entity.Repfirma = dr.GetString(iRepfirma);

                        entitys.Add(entity);
                    }
                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex); ;
            }
            return entitys;
        }

        public int GuardarNuevoResponsable(SiResponsableDTO responsableDTO)
        {
            try
            {
                DbCommand commandMax = dbProvider.GetSqlStringCommand(helper.SqlObtenerMaxIdResponsable);
                object resultMax = dbProvider.ExecuteScalar(commandMax);
                int MaxIdResponsable = 1;
                if (resultMax != null) MaxIdResponsable = Convert.ToInt32(resultMax);

                string query = string.Format(helper.SqlGuardarNuevoResponsable,
                    MaxIdResponsable,
                    responsableDTO.CodigoDirector,
                    responsableDTO.Estado,
                    responsableDTO.NombreArchivoFirma,
                    responsableDTO.Repfirma
                    );
                DbCommand command = dbProvider.GetSqlStringCommand(query);
                object result = dbProvider.ExecuteNonQuery(command);
                return MaxIdResponsable;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex); ;
            }
        }

        public int GuardarEditarResponsable(SiResponsableDTO responsableDTO)
        {
            try
            {
                string query = string.Format(helper.SqlGuardarEditarResponsable,
                    responsableDTO.CodigoResponsable,
                    responsableDTO.Estado,
                    responsableDTO.NombreArchivoFirma,
                    responsableDTO.Repfirma
                    );
                DbCommand command = dbProvider.GetSqlStringCommand(query);
                object result = dbProvider.ExecuteNonQuery(command);
                return 1;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex); ;
            }
        }

        public bool EliminarResponsable(int codigoResponsable)
        {
            try
            {
                string query = string.Format(helper.SqlEliminarResponsable, codigoResponsable);
                DbCommand command = dbProvider.GetSqlStringCommand(query);

                object result = dbProvider.ExecuteScalar(command);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex); ;
            }
        }

        public SiResponsableDTO ObtenerResponsable(int codigoResponsable)
        {
            try
            {
                SiResponsableDTO entity = new SiResponsableDTO();
                string query = string.Format(helper.SqlObtenerResponsable, codigoResponsable);
                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        int iCodigoResponsable = dr.GetOrdinal(helper.CodigoResponsable);
                        if (!dr.IsDBNull(iCodigoResponsable)) entity.CodigoResponsable = dr.GetInt32(iCodigoResponsable);

                        int iCodigoDirector = dr.GetOrdinal(helper.CodigoDirector);
                        if (!dr.IsDBNull(iCodigoDirector)) entity.CodigoDirector = dr.GetInt32(iCodigoDirector);

                        int iNombreCompleto = dr.GetOrdinal(helper.NombreCompletoResponsable);
                        if (!dr.IsDBNull(iNombreCompleto)) entity.NombreCompleto = dr.GetString(iNombreCompleto);

                        int iEstado = dr.GetOrdinal(helper.EstadoEditarResponsable);
                        if (!dr.IsDBNull(iEstado)) entity.Estado = dr.GetString(iEstado);

                        int iNombreArchivoFirma = dr.GetOrdinal(helper.NombreArchivoFirma);
                        if (!dr.IsDBNull(iNombreArchivoFirma)) entity.NombreArchivoFirma = dr.GetString(iNombreArchivoFirma);

                        int iRepfirma = dr.GetOrdinal(helper.Repfirma);
                        if (!dr.IsDBNull(iRepfirma)) entity.Repfirma = dr.GetString(iRepfirma);
                    }
                }
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex); ;
            }
            
        }

        public bool ExisteIEI(EmpresaInvolucradaDTO entity)
        {
            int result = 0;
            string query = string.Format(helper.SqlExisteIEI, entity.AFECODI, entity.EMPRCODI, entity.AFIVERSION);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            result = int.Parse(dbProvider.ExecuteScalar(command).ToString());
            return result > 0;
        }

        #endregion

        #region FIT - SGOCOES func A - Soporte
        public int ValidaCorrelativo(AnalisisFallaDTO entity)
        {

            string query = string.Format(helper.ValidaCorrelativo, entity.AFEANIO, entity.AFECORR);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            return int.Parse(dbProvider.ExecuteScalar(command).ToString());

        }

        public int ValidaRespuestaCOES(int afecodi, int tipcodi, int rptacodi)
        {
            string query = string.Format(helper.ValidaRespuestaCOES, afecodi, tipcodi, rptacodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            return int.Parse(dbProvider.ExecuteScalar(command).ToString());
        }

        public int ValidaRespuestaCOES1(int afecodi, int tipcodi, int rptacodi, int emprcodi)
        {
            string query = string.Format(helper.ValidaRespuestaCOES1, afecodi, tipcodi, rptacodi, emprcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            return int.Parse(dbProvider.ExecuteScalar(command).ToString());
        }


        public bool EliminarEmpresaCompensadaCompensacion(EmpresaResponsableDTO entity)
        {
            int resul = -1;
            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.EliminarEmpresaCompensadaCompensacion, entity.AFECODI, entity.EMPRCODI, entity.AFIVERSION));
            resul = dbProvider.ExecuteNonQuery(command);
            return resul > 0;
        }
        #endregion

        #region SIOSEIN2


        public List<EventoDTO> ObtenerEventosFallas(DateTime fechaInicio, DateTime fechaFin, string indInterrupcion, string evenpreliminar)
        {
            String query = string.Format(helper.SqlObtenerEventosFallas, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha), indInterrupcion, evenpreliminar);

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<EventoDTO> entitys = new List<EventoDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EventoDTO entity = new EventoDTO();

                    int iEMPRNOMB = dr.GetOrdinal(helper.EMPRNOMB);
                    if (!dr.IsDBNull(iEMPRNOMB)) entity.EMPRNOMB = dr.GetString(iEMPRNOMB);

                    int iTAREAABREV = dr.GetOrdinal(helper.TAREAABREV);
                    if (!dr.IsDBNull(iTAREAABREV)) entity.TAREAABREV = dr.GetString(iTAREAABREV);

                    int iAREANOMB = dr.GetOrdinal(helper.AREANOMB);
                    if (!dr.IsDBNull(iAREANOMB)) entity.AREANOMB = dr.GetString(iAREANOMB);

                    int iEQUIABREV = dr.GetOrdinal(helper.EQUIABREV);
                    if (!dr.IsDBNull(iEQUIABREV)) entity.EQUIABREV = dr.GetString(iEQUIABREV);

                    int iEVENINI = dr.GetOrdinal(helper.EVENINI);
                    if (!dr.IsDBNull(iEVENINI)) entity.EVENINI = dr.GetDateTime(iEVENINI);

                    int iEVENFIN = dr.GetOrdinal(helper.EVENFIN);
                    if (!dr.IsDBNull(iEVENFIN)) entity.EVENFIN = dr.GetDateTime(iEVENFIN);

                    int iEMPRABREV = dr.GetOrdinal(helper.EMPRABREV);
                    if (!dr.IsDBNull(iEMPRABREV)) entity.EMPRABREV = dr.GetString(iEMPRABREV);

                    int iEVENASUNTO = dr.GetOrdinal(helper.EVENASUNTO);
                    if (!dr.IsDBNull(iEVENASUNTO)) entity.EVENASUNTO = dr.GetString(iEVENASUNTO);

                    int iEVENINTERRUP = dr.GetOrdinal(helper.EVENINTERRUP);
                    if (!dr.IsDBNull(iEVENINTERRUP)) entity.EVENINTERRUP = dr.GetString(iEVENINTERRUP);

                    int iTIPOEVENCODI = dr.GetOrdinal(helper.TIPOEVENCODI);
                    if (!dr.IsDBNull(iTIPOEVENCODI)) entity.TIPOEVENCODI = Convert.ToInt32(dr.GetValue(iTIPOEVENCODI));

                    int iTIPOEVENABREV = dr.GetOrdinal(helper.TIPOEVENABREV);
                    if (!dr.IsDBNull(iTIPOEVENABREV)) entity.TIPOEVENABREV = dr.GetString(iTIPOEVENABREV);

                    int iCAUSAEVENABREV = dr.GetOrdinal(helper.CAUSAEVENABREV);
                    if (!dr.IsDBNull(iCAUSAEVENABREV)) entity.CAUSAEVENABREV = dr.GetString(iCAUSAEVENABREV);

                    int iENERGIAINTERRUMPIDA = dr.GetOrdinal(helper.ENERGIAINTERRUMPIDA);
                    if (!dr.IsDBNull(iENERGIAINTERRUMPIDA)) entity.ENERGIAINTERRUMPIDA = dr.GetDecimal(iENERGIAINTERRUMPIDA);

                    int iTIPOREGISTRO = dr.GetOrdinal(helper.TIPOREGISTRO);
                    if (!dr.IsDBNull(iTIPOREGISTRO)) entity.TIPOREGISTRO = dr.GetString(iTIPOREGISTRO);

                    int iVALTIPOREGISTRO = dr.GetOrdinal(helper.VALTIPOREGISTRO);
                    if (!dr.IsDBNull(iVALTIPOREGISTRO)) entity.VALTIPOREGISTRO = dr.GetString(iVALTIPOREGISTRO);

                    int iEQUITENSION = dr.GetOrdinal(helper.EQUITENSION);
                    if (!dr.IsDBNull(iEQUITENSION)) entity.EQUITENSION = dr.GetDecimal(iEQUITENSION);

                    int iTIPOEMPRNOMB = dr.GetOrdinal(helper.TIPOEMPRNOMB);
                    if (!dr.IsDBNull(iTIPOEMPRNOMB)) entity.TIPOEMPRNOMB = dr.GetString(iTIPOEMPRNOMB);

                    int iEVENPRELIMINAR = dr.GetOrdinal(helper.EVENPRELIMINAR);
                    if (!dr.IsDBNull(iEVENPRELIMINAR)) entity.EVENPRELIMINAR = dr.GetString(iEVENPRELIMINAR);

                    int iEVENMWGENDESCON = dr.GetOrdinal(helper.EVENMWGENDESCON);
                    if (!dr.IsDBNull(iEVENMWGENDESCON)) entity.EVENMWGENDESCON = dr.GetDecimal(iEVENMWGENDESCON);

                    int iEVENGENDESCON = dr.GetOrdinal(helper.EVENGENDESCON);
                    if (!dr.IsDBNull(iEVENGENDESCON)) entity.EVENGENDESCON = dr.GetString(iEVENGENDESCON);

                    int iEMPRCODI = dr.GetOrdinal(helper.EMPRCODI);
                    if (!dr.IsDBNull(iEMPRCODI)) entity.EMPRCODI = Convert.ToInt32(dr.GetValue(iEMPRCODI));

                    int iAREACODI = dr.GetOrdinal(helper.AREACODI);
                    if (!dr.IsDBNull(iAREACODI)) entity.AREACODI = Convert.ToInt32(dr.GetValue(iAREACODI));

                    int iEVENCODI = dr.GetOrdinal(helper.EVENCODI);
                    if (!dr.IsDBNull(iEVENCODI)) entity.EVENCODI = Convert.ToInt32(dr.GetValue(iEVENCODI));

                    int iEVENCOMENTARIOS = dr.GetOrdinal(helper.EVENCOMENTARIOS);
                    if (!dr.IsDBNull(iEVENCOMENTARIOS)) entity.EVENCOMENTARIOS = dr.GetString(iEVENCOMENTARIOS);

                    int iAREADESC = dr.GetOrdinal(helper.AREADESC);
                    if (!dr.IsDBNull(iAREADESC)) entity.AREADESC = dr.GetString(iAREADESC);

                    int iEVENDESC = dr.GetOrdinal(helper.EVENDESC);
                    if (!dr.IsDBNull(iEVENDESC)) entity.EVENDESC = dr.GetString(iEVENDESC);

                    int iMWINTERRUMPIDOS = dr.GetOrdinal(helper.MWINTERRUMPIDOS);
                    if (!dr.IsDBNull(iMWINTERRUMPIDOS)) entity.MWINTERRUMPIDOS = dr.GetDecimal(iMWINTERRUMPIDOS);

                    //int iINTERRMANUALR = dr.GetOrdinal(helper.Interrmanualr);
                    //if (!dr.IsDBNull(iINTERRMANUALR)) entity.Interrmanualr = dr.GetString(iINTERRMANUALR);

                    //int iINTERRRACMF = dr.GetOrdinal(helper.Interrracmf);
                    //if (!dr.IsDBNull(iINTERRRACMF)) entity.Interrracmf = dr.GetString(iINTERRRACMF);

                    //int iInterrnivel = dr.GetOrdinal(helper.Interrnivel);
                    //if (!dr.IsDBNull(iInterrnivel)) entity.Interrnivel = dr.GetString(iInterrnivel);

                    int iCausaevencodi = dr.GetOrdinal(helper.Causaevencodi);
                    if (!dr.IsDBNull(iCausaevencodi)) entity.Causaevencodi = Convert.ToInt32(dr.GetValue(iCausaevencodi));

                    int iCausaevendesc = dr.GetOrdinal(helper.Causaevendesc);
                    if (!dr.IsDBNull(iCausaevendesc)) entity.Causaevendesc = dr.GetString(iCausaevendesc);

                    int iFAMCODI = dr.GetOrdinal(helper.FAMCODI);
                    if (!dr.IsDBNull(iFAMCODI)) entity.FAMCODI = Convert.ToInt32(dr.GetValue(iFAMCODI));

                    int iFAMNOMB = dr.GetOrdinal(helper.FAMNOMB);
                    if (!dr.IsDBNull(iFAMNOMB)) entity.FAMNOMB = dr.GetString(iFAMNOMB);

                    int iFAMABREV = dr.GetOrdinal(helper.FAMABREV);
                    if (!dr.IsDBNull(iFAMABREV)) entity.FAMABREV = dr.GetString(iFAMABREV);

                    decimal suma = 0;

                    if (entity.TIPOREGISTRO == "D" && entity.EVENPRELIMINAR != "S")
                    {
                        if (!string.IsNullOrEmpty(entity.VALTIPOREGISTRO))
                        {
                            string[] split = entity.VALTIPOREGISTRO.Split(',');

                            if (split.Length == 8)
                            {
                                if (split[0] == "S") suma = suma + decimal.Parse(split[1]);
                                if (split[2] == "S") suma = suma + decimal.Parse(split[3]);
                                if (split[4] == "S") suma = suma + decimal.Parse(split[5]);
                                if (split[6] == "S") suma = suma + decimal.Parse(split[7]);

                                entity.EVENMWINDISP = suma;
                            }
                        }
                    }

                    entitys.Add(entity);
                }
            }

            return entitys;

        }

        #endregion

        #region Aplicativo Extranet CTAF

        /// <summary>
        /// Devuelve la lista de interrupciones de suministros aplicando los filtros indicados
        /// </summary>
        /// <param name="oEventoDTO"></param>
        /// <returns></returns>
        public List<EventoDTO> ConsultarInterrupcionesSuministros(EventoDTO oEventoDTO)
        {
            List<EventoDTO> entitys = new List<EventoDTO>();

            string query = string.Format(helper.SqlConsultarInterrupcionSuministros,
                oEventoDTO.EmpresaPropietaria,  //1
                oEventoDTO.EmpresaInvolucrada, //2
                oEventoDTO.TipoEquipo, //3
                oEventoDTO.Estado, //4
                oEventoDTO.DI, //5
                oEventoDTO.DF, //6                                        
                oEventoDTO.RNC, //7
                oEventoDTO.ERACMF, //8
                oEventoDTO.ERACMT, //9
                oEventoDTO.EDAGSF, //10                  
                oEventoDTO.EveSinDatosReportados, //11
                oEventoDTO.ListaEmprcodi ,//12
                oEventoDTO.EVENASUNTO //12
               );
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EventoDTO entity = new EventoDTO();

                    int iAFECODI = dr.GetOrdinal("AFECODI");
                    int iEVENCODI = dr.GetOrdinal("EVENCODI");
                    int iCODIGO = dr.GetOrdinal("CODIGO");
                    int iNOMBRE_EVENTO = dr.GetOrdinal("NOMBRE_EVENTO");
                    int iINTERRUMPIDO = dr.GetOrdinal("INTERRUMPIDO");
                    int iERACMF = dr.GetOrdinal("ERACMF");
                    int iFECHA_EVENTO = dr.GetOrdinal("FECHA_EVENTO");
                    int IFECHA_INTERRUPCION = dr.GetOrdinal("FECHA_INTERRUPCION");
                    int iPLAZO_ENVIO = dr.GetOrdinal("PLAZO_ENVIO");
                    int IAfeplazofechaampl = dr.GetOrdinal("AFEPLAZOFECHAAMPL");
                    int iReportado = dr.GetOrdinal("REPORTADO");
                    int iAfecorr = dr.GetOrdinal("AFECORR");
                    int iAfeanio = dr.GetOrdinal("AFEANIO");

                    if (!dr.IsDBNull(iAFECODI)) entity.AFECODI = dr.GetInt32(iAFECODI);
                    if (!dr.IsDBNull(iEVENCODI)) entity.EVENCODI = dr.GetInt32(iEVENCODI);
                    if (!dr.IsDBNull(iCODIGO)) entity.CODIGO = dr.GetString(iCODIGO);
                    if (!dr.IsDBNull(iNOMBRE_EVENTO)) entity.NOMBRE_EVENTO = dr.GetString(iNOMBRE_EVENTO);
                    if (!dr.IsDBNull(iINTERRUMPIDO)) entity.INTERRUMPIDO = dr.GetString(iINTERRUMPIDO);
                    if (!dr.IsDBNull(iERACMF)) entity.ERACMF = dr.GetString(iERACMF);
                    if (!dr.IsDBNull(iFECHA_EVENTO)) entity.FechaEvento = dr.GetDateTime(iFECHA_EVENTO);
                    if (!dr.IsDBNull(IFECHA_INTERRUPCION)) entity.Afefechainterr = dr.GetDateTime(IFECHA_INTERRUPCION);
                    if (!dr.IsDBNull(iPLAZO_ENVIO)) entity.Afeplazofecha = dr.GetDateTime(iPLAZO_ENVIO);
                    if (!dr.IsDBNull(IAfeplazofechaampl)) entity.Afeplazofechaampl = dr.GetDateTime(IAfeplazofechaampl);

                    int iEVENINI = dr.GetOrdinal(helper.EVENINI);
                    if (!dr.IsDBNull(iEVENINI)) entity.EVENINI = dr.GetDateTime(iEVENINI);

                    int iEVENFIN = dr.GetOrdinal(helper.EVENFIN);
                    if (!dr.IsDBNull(iEVENFIN)) entity.EVENFIN = dr.GetDateTime(iEVENFIN);

                    int iEVENASUNTO = dr.GetOrdinal(helper.EVENASUNTO);
                    if (!dr.IsDBNull(iEVENASUNTO)) entity.EVENASUNTO = dr.GetString(iEVENASUNTO);

                    if (!dr.IsDBNull(iReportado)) entity.Reportado = dr.GetString(iReportado);

                    if (!dr.IsDBNull(iAfecorr)) entity.Afecorr = dr.GetInt32(iAfecorr).ToString();
                    if (!dr.IsDBNull(iAfeanio)) entity.Afeanio = dr.GetInt32(iAfeanio).ToString();

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        /// <summary>
        /// Devuelve el Evento de Interrupción
        /// </summary>
        /// <param name="afecodi"></param>
        /// <returns></returns>
        public EventoDTO ObtenerInterrupcion(int afecodi)
        {
            EventoDTO entity = null;

            string query = string.Format(helper.SqlObtenerInterrupcionSuministro, afecodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new EventoDTO();

                    int iAFECODI = dr.GetOrdinal("AFECODI");
                    int iEVENCODI = dr.GetOrdinal("EVENCODI");
                    int iCODIGO = dr.GetOrdinal("CODIGO");
                    int iNOMBRE_EVENTO = dr.GetOrdinal("NOMBRE_EVENTO");
                    int iINTERRUMPIDO = dr.GetOrdinal("INTERRUMPIDO");
                    int iERACMF = dr.GetOrdinal("ERACMF");
                    int iFECHA_EVENTO = dr.GetOrdinal("FECHA_EVENTO");
                    int IFECHA_INTERRUPCION = dr.GetOrdinal("FECHA_INTERRUPCION");
                    int iPLAZO_ENVIO = dr.GetOrdinal("PLAZO_ENVIO");
                    int IAfeplazofechaampl = dr.GetOrdinal("AFEPLAZOFECHAAMPL");
                    int iEVENDESC = dr.GetOrdinal("EVENDESC");
                    int iAFECORR = dr.GetOrdinal("AFECORR");
                    int iAFEANIO = dr.GetOrdinal("AFEANIO");
      
                    if (!dr.IsDBNull(iAFECODI)) entity.AFECODI = dr.GetInt32(iAFECODI);
                    if (!dr.IsDBNull(iEVENCODI)) entity.EVENCODI = dr.GetInt32(iEVENCODI);
                    if (!dr.IsDBNull(iCODIGO)) entity.CODIGO = dr.GetString(iCODIGO);
                    if (!dr.IsDBNull(iNOMBRE_EVENTO)) entity.NOMBRE_EVENTO = dr.GetString(iNOMBRE_EVENTO);
                    if (!dr.IsDBNull(iINTERRUMPIDO)) entity.INTERRUMPIDO = dr.GetString(iINTERRUMPIDO);
                    if (!dr.IsDBNull(iERACMF)) entity.ERACMF = dr.GetString(iERACMF);
                    if (!dr.IsDBNull(iFECHA_EVENTO)) entity.FechaEvento = dr.GetDateTime(iFECHA_EVENTO);
                    if (!dr.IsDBNull(IFECHA_INTERRUPCION)) entity.Afefechainterr = dr.GetDateTime(IFECHA_INTERRUPCION);
                    if (!dr.IsDBNull(iPLAZO_ENVIO)) entity.Afeplazofecha = dr.GetDateTime(iPLAZO_ENVIO);
                    if (!dr.IsDBNull(IAfeplazofechaampl)) entity.Afeplazofechaampl = dr.GetDateTime(IAfeplazofechaampl);
                    if (!dr.IsDBNull(iEVENDESC)) entity.EVENDESC = dr.GetString(iEVENDESC);

                    int iEVENINI = dr.GetOrdinal(helper.EVENINI);
                    if (!dr.IsDBNull(iEVENINI)) entity.EVENINI = dr.GetDateTime(iEVENINI);

                    int iEVENFIN = dr.GetOrdinal(helper.EVENFIN);
                    if (!dr.IsDBNull(iEVENFIN)) entity.EVENFIN = dr.GetDateTime(iEVENFIN);

                    int iEVENASUNTO = dr.GetOrdinal(helper.EVENASUNTO);
                    if (!dr.IsDBNull(iEVENASUNTO)) entity.EVENASUNTO = dr.GetString(iEVENASUNTO);
              
                    if (!dr.IsDBNull(iAFECORR)) entity.Afecorr = dr.GetInt32(iAFECORR).ToString();
                    if (!dr.IsDBNull(iAFEANIO)) entity.Afeanio = dr.GetInt32(iAFEANIO).ToString();

                }
            }
            return entity;
        }

        /// <summary>
        /// Devuelve el Evento de Interrupción
        /// </summary>
        /// <param name="afecodi"></param>
        /// <returns></returns>
        public List<EventoDTO> ObtenerInterrupcionCTAF(string anio,string correlativo)
        {
            EventoDTO entity = null;

            string query = string.Format(helper.SqlObtenerInterrupcionCTAF, anio,correlativo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<EventoDTO> entitys = new List<EventoDTO>();
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new EventoDTO();

                    int iAFECODI = dr.GetOrdinal("AFECODI");
                    int iEVENCODI = dr.GetOrdinal("EVENCODI");
                    int iCODIGO = dr.GetOrdinal("CODIGO");
                    int iNOMBRE_EVENTO = dr.GetOrdinal("NOMBRE_EVENTO");
                    int iINTERRUMPIDO = dr.GetOrdinal("INTERRUMPIDO");
                    int iERACMF = dr.GetOrdinal("ERACMF");
                    int iFECHA_EVENTO = dr.GetOrdinal("FECHA_EVENTO");
                    int IFECHA_INTERRUPCION = dr.GetOrdinal("FECHA_INTERRUPCION");
                    int iPLAZO_ENVIO = dr.GetOrdinal("PLAZO_ENVIO");
                    int IAfeplazofechaampl = dr.GetOrdinal("AFEPLAZOFECHAAMPL");
                    int iEVENDESC = dr.GetOrdinal("EVENDESC");
                    int iAFECORR = dr.GetOrdinal("AFECORR");
                    int iAFEANIO = dr.GetOrdinal("AFEANIO");

                    if (!dr.IsDBNull(iAFECODI)) entity.AFECODI = dr.GetInt32(iAFECODI);
                    if (!dr.IsDBNull(iEVENCODI)) entity.EVENCODI = dr.GetInt32(iEVENCODI);
                    if (!dr.IsDBNull(iCODIGO)) entity.CODIGO = dr.GetString(iCODIGO);
                    if (!dr.IsDBNull(iNOMBRE_EVENTO)) entity.NOMBRE_EVENTO = dr.GetString(iNOMBRE_EVENTO);
                    if (!dr.IsDBNull(iINTERRUMPIDO)) entity.INTERRUMPIDO = dr.GetString(iINTERRUMPIDO);
                    if (!dr.IsDBNull(iERACMF)) entity.ERACMF = dr.GetString(iERACMF);
                    if (!dr.IsDBNull(iFECHA_EVENTO)) entity.FechaEvento = dr.GetDateTime(iFECHA_EVENTO);
                    if (!dr.IsDBNull(IFECHA_INTERRUPCION)) entity.Afefechainterr = dr.GetDateTime(IFECHA_INTERRUPCION);
                    if (!dr.IsDBNull(iPLAZO_ENVIO)) entity.Afeplazofecha = dr.GetDateTime(iPLAZO_ENVIO);
                    if (!dr.IsDBNull(IAfeplazofechaampl)) entity.Afeplazofechaampl = dr.GetDateTime(IAfeplazofechaampl);
                    if (!dr.IsDBNull(iEVENDESC)) entity.EVENDESC = dr.GetString(iEVENDESC);

                    int iEVENINI = dr.GetOrdinal(helper.EVENINI);
                    if (!dr.IsDBNull(iEVENINI)) entity.EVENINI = dr.GetDateTime(iEVENINI);

                    int iEVENFIN = dr.GetOrdinal(helper.EVENFIN);
                    if (!dr.IsDBNull(iEVENFIN)) entity.EVENFIN = dr.GetDateTime(iEVENFIN);

                    int iEVENASUNTO = dr.GetOrdinal(helper.EVENASUNTO);
                    if (!dr.IsDBNull(iEVENASUNTO)) entity.EVENASUNTO = dr.GetString(iEVENASUNTO);

                    if (!dr.IsDBNull(iAFECORR)) entity.Afecorr = dr.GetInt32(iAFECORR).ToString();
                    if (!dr.IsDBNull(iAFEANIO)) entity.Afeanio = dr.GetInt32(iAFEANIO).ToString();

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public EventoDTO ObtenerInterrupcionByEvencodi(int evencodi)
        {
            EventoDTO entity = null;

            string query = string.Format(helper.SqlObtenerInterrupcionSuministroByEvencodi, evencodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new EventoDTO();

                    int iAFECODI = dr.GetOrdinal("AFECODI");
                    int iEVENCODI = dr.GetOrdinal("EVENCODI");
                    int iCODIGO = dr.GetOrdinal("CODIGO");
                    int iNOMBRE_EVENTO = dr.GetOrdinal("NOMBRE_EVENTO");
                    int iINTERRUMPIDO = dr.GetOrdinal("INTERRUMPIDO");
                    int iERACMF = dr.GetOrdinal("ERACMF");
                    int iFECHA_EVENTO = dr.GetOrdinal("FECHA_EVENTO");
                    int IFECHA_INTERRUPCION = dr.GetOrdinal("FECHA_INTERRUPCION");
                    int iPLAZO_ENVIO = dr.GetOrdinal("PLAZO_ENVIO");
                    int IAfeplazofechaampl = dr.GetOrdinal("AFEPLAZOFECHAAMPL");
                    int iEVENDESC = dr.GetOrdinal("EVENDESC");

                    if (!dr.IsDBNull(iAFECODI)) entity.AFECODI = dr.GetInt32(iAFECODI);
                    if (!dr.IsDBNull(iEVENCODI)) entity.EVENCODI = dr.GetInt32(iEVENCODI);
                    if (!dr.IsDBNull(iCODIGO)) entity.CODIGO = dr.GetString(iCODIGO);
                    if (!dr.IsDBNull(iNOMBRE_EVENTO)) entity.NOMBRE_EVENTO = dr.GetString(iNOMBRE_EVENTO);
                    if (!dr.IsDBNull(iINTERRUMPIDO)) entity.INTERRUMPIDO = dr.GetString(iINTERRUMPIDO);
                    if (!dr.IsDBNull(iERACMF)) entity.ERACMF = dr.GetString(iERACMF);
                    if (!dr.IsDBNull(iFECHA_EVENTO)) entity.FechaEvento = dr.GetDateTime(iFECHA_EVENTO);
                    if (!dr.IsDBNull(IFECHA_INTERRUPCION)) entity.Afefechainterr = dr.GetDateTime(IFECHA_INTERRUPCION);
                    if (!dr.IsDBNull(iPLAZO_ENVIO)) entity.Afeplazofecha = dr.GetDateTime(iPLAZO_ENVIO);
                    if (!dr.IsDBNull(IAfeplazofechaampl)) entity.Afeplazofechaampl = dr.GetDateTime(IAfeplazofechaampl);
                    if (!dr.IsDBNull(iEVENDESC)) entity.EVENDESC = dr.GetString(iEVENDESC);

                    int iEVENINI = dr.GetOrdinal(helper.EVENINI);
                    if (!dr.IsDBNull(iEVENINI)) entity.EVENINI = dr.GetDateTime(iEVENINI);

                    int iEVENFIN = dr.GetOrdinal(helper.EVENFIN);
                    if (!dr.IsDBNull(iEVENFIN)) entity.EVENFIN = dr.GetDateTime(iEVENFIN);

                    int iEVENASUNTO = dr.GetOrdinal(helper.EVENASUNTO);
                    if (!dr.IsDBNull(iEVENASUNTO)) entity.EVENASUNTO = dr.GetString(iEVENASUNTO);
                }
            }
            return entity;
        }

        public int EditarAfEvento(EventoDTO oEventoDTO)
        {
            int id = -1;
            DbCommand command;

            command = dbProvider.GetSqlStringCommand(helper.SqlEditarAfEvento);

            dbProvider.AddInParameter(command, helper.Afefechainterr, DbType.DateTime, oEventoDTO.Afefechainterr);
            dbProvider.AddInParameter(command, helper.Afeplazofecha, DbType.DateTime, oEventoDTO.Afeplazofecha);
            dbProvider.AddInParameter(command, helper.Afeplazofechaampl, DbType.DateTime, oEventoDTO.Afeplazofechaampl);
            dbProvider.AddInParameter(command, helper.Afeplazousumodificacion, DbType.String, oEventoDTO.Afeplazousumodificacion);
            dbProvider.AddInParameter(command, helper.Afeplazofecmodificacion, DbType.DateTime, oEventoDTO.Afeplazofecmodificacion);
            dbProvider.AddInParameter(command, helper.AFECODI, DbType.Int32, oEventoDTO.AFECODI);

            id = dbProvider.ExecuteNonQuery(command);

            command = dbProvider.GetSqlStringCommand(helper.SqlEditarEventoAf);

            dbProvider.AddInParameter(command, helper.EVENINI, DbType.DateTime, oEventoDTO.EVENINI);
            dbProvider.AddInParameter(command, helper.EVENCODI, DbType.Int32, oEventoDTO.EVENCODI);

            id = dbProvider.ExecuteNonQuery(command);

            return id;
        }


        public List<EventoDTO> ListarInterrupcionPorEventoSCO(EventoDTO dto)
        {
            EventoDTO entity = null;
            List<EventoDTO> listEntity = new List<EventoDTO>();

            string query = string.Format(helper.SqlListarInterrupcionPorEventoSCO, dto.Afeanio,dto.Afecorr);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new EventoDTO();

                    int iAfecodi = dr.GetOrdinal(helper.Afecodi);
                    int iEvencodi = dr.GetOrdinal(helper.EVENCODI);
                    int iEvenini = dr.GetOrdinal(helper.EVENINI);
                    int iAfefechainterr= dr.GetOrdinal(helper.Afefechainterr);
                    int iAfeeracmf = dr.GetOrdinal(helper.Afeeracmf);

                    if (!dr.IsDBNull(iAfecodi)) entity.AFECODI = dr.GetInt32(iAfecodi);
                    if (!dr.IsDBNull(iEvencodi)) entity.EVENCODI = dr.GetInt32(iEvencodi);
                    if (!dr.IsDBNull(iEvenini)) entity.EVENINI = dr.GetDateTime(iEvenini);
                    if (!dr.IsDBNull(iAfefechainterr)) entity.Afefechainterr = dr.GetDateTime(iAfefechainterr);
                    if (!dr.IsDBNull(iAfeeracmf)) entity.Afeeracmf = dr.GetString(iAfeeracmf);

                    listEntity.Add(entity);
                }
               
            }

            return listEntity;
        }

        public List<SiSenializacionDTO> ListarSenializacionesProteccion(int CodigoEvento)
        {
            SiSenializacionDTO entity = null;
            List<SiSenializacionDTO> listEntity = new List<SiSenializacionDTO>();

            string query = string.Format(helper.SqlListarSenializacionesProteccion, CodigoEvento);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new SiSenializacionDTO();

                    int iCodigoSenializacion = dr.GetOrdinal(helper.CodigoSenializacion);
                    int iCodigoEvento = dr.GetOrdinal(helper.CodigoEvento);
                    int iSubEstacion = dr.GetOrdinal(helper.SubEstacion);
                    int iEquipo = dr.GetOrdinal(helper.Equipo);
                    int iCodigo = dr.GetOrdinal(helper.Codigo);
                    int iSenializaciones = dr.GetOrdinal(helper.Senializaciones);
                    int iInterruptor = dr.GetOrdinal(helper.Interruptor);
                    int iCodigoAC = dr.GetOrdinal(helper.CodigoAC);

                    if (!dr.IsDBNull(iCodigoSenializacion)) entity.CodigoSenializacion = dr.GetInt32(iCodigoSenializacion);
                    if (!dr.IsDBNull(iCodigoEvento)) entity.CodigoEvento = dr.GetInt32(iCodigoEvento);
                    if (!dr.IsDBNull(iSubEstacion)) entity.SubEstacion = dr.GetString(iSubEstacion);
                    if (!dr.IsDBNull(iEquipo)) entity.Equipo = dr.GetString(iEquipo);
                    if (!dr.IsDBNull(iCodigo)) entity.Codigo = dr.GetString(iCodigo);
                    if (!dr.IsDBNull(iSenializaciones)) entity.Senializaciones = dr.GetString(iSenializaciones);
                    if (!dr.IsDBNull(iInterruptor)) entity.Interruptor = dr.GetString(iInterruptor);
                    if (!dr.IsDBNull(iCodigoAC)) entity.CodigoAC = dr.GetString(iCodigoAC);

                    listEntity.Add(entity);
                }

            }

            return listEntity;
        }

        public int GrabarSenializacionProteccion(SiSenializacionDTO objSiSenializacionDTO)
        {
            try
            {
                string queryMax = helper.SqlObtenerMaxIdSenializacionesProteccion;
                DbCommand commandMax = dbProvider.GetSqlStringCommand(queryMax);
                int MaxIdSenializacion = int.Parse(dbProvider.ExecuteScalar(commandMax).ToString()) + 1;
                string query = string.Format(helper.SqlGrabarSenializacionesProteccion,
                    MaxIdSenializacion,
                    objSiSenializacionDTO.CodigoEvento,
                    objSiSenializacionDTO.SubEstacion,
                    objSiSenializacionDTO.Equipo,
                    objSiSenializacionDTO.Codigo,
                    objSiSenializacionDTO.Senializaciones,
                    objSiSenializacionDTO.Interruptor,
                    objSiSenializacionDTO.CodigoAC,
                    objSiSenializacionDTO.UsuarioCreacion
                    );
                DbCommand command = dbProvider.GetSqlStringCommand(query);
                object result = dbProvider.ExecuteNonQuery(command);
                return MaxIdSenializacion;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex); ;
            }
        }

        public int EliminarSenializacionProteccion(int codigoEvento)
        {
            try
            {
                string query = string.Format(helper.SqlEliminarSenializacionProteccion, codigoEvento);
                DbCommand command = dbProvider.GetSqlStringCommand(query);
                object result = dbProvider.ExecuteNonQuery(command);
                return codigoEvento;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex); ;
            }
        }


        public List<SiSenializacionDTO> ListarSenializacionesProteccionAgrupado(int CodigoEvento)
        {
            SiSenializacionDTO entity = null;
            List<SiSenializacionDTO> listEntity = new List<SiSenializacionDTO>();

            string query = string.Format(helper.SqlListarSenializacionesProteccionAgrupado, CodigoEvento);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new SiSenializacionDTO();

                    int iSubEstacion = dr.GetOrdinal(helper.SubEstacion);
                    int iEquipo = dr.GetOrdinal(helper.Equipo);
                    int iCodigo = dr.GetOrdinal(helper.Codigo);
                    int iSenializaciones = dr.GetOrdinal(helper.Senializaciones);
                    int iInterruptor = dr.GetOrdinal(helper.Interruptor);
                    int iCodigoAC = dr.GetOrdinal(helper.CodigoAC);

                    if (!dr.IsDBNull(iSubEstacion)) entity.SubEstacion = dr.GetString(iSubEstacion);
                    if (!dr.IsDBNull(iEquipo)) entity.Equipo = dr.GetString(iEquipo);
                    if (!dr.IsDBNull(iCodigo)) entity.Codigo = dr.GetString(iCodigo);
                    if (!dr.IsDBNull(iSenializaciones)) entity.Senializaciones = dr.GetString(iSenializaciones);
                    if (!dr.IsDBNull(iInterruptor)) entity.Interruptor = dr.GetString(iInterruptor);
                    if (!dr.IsDBNull(iCodigoAC)) entity.CodigoAC = dr.GetString(iCodigoAC);

                    listEntity.Add(entity);
                }

            }

            return listEntity;
        }

        #endregion
        #region Mejoras CTAF
        public List<EventoDTO> LstEventosSco(string anio, string correlativo)
        {
            String query = String.Format(helper.SqlLstEventosSco, Convert.ToInt32(anio), Convert.ToInt32(correlativo));
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<EventoDTO> entitys = new List<EventoDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EventoDTO entity = new EventoDTO();

                    int iTIPOEVENABREV = dr.GetOrdinal(helper.TIPOEVENABREV);
                    if (!dr.IsDBNull(iTIPOEVENABREV)) entity.TIPOEVENABREV = dr.GetString(iTIPOEVENABREV);

                    int iEMPRNOMB = dr.GetOrdinal(helper.EMPRNOMB);
                    if (!dr.IsDBNull(iEMPRNOMB)) entity.EMPRNOMB = dr.GetString(iEMPRNOMB);

                    int iAREADESC = dr.GetOrdinal(helper.AREADESC);
                    if (!dr.IsDBNull(iAREADESC)) entity.AREADESC = dr.GetString(iAREADESC);

                    int iFAMABREV = dr.GetOrdinal(helper.FAMABREV);
                    if (!dr.IsDBNull(iFAMABREV)) entity.FAMABREV = dr.GetString(iFAMABREV);

                    int iEQUIABREV = dr.GetOrdinal(helper.EQUIABREV);
                    if (!dr.IsDBNull(iEQUIABREV)) entity.EQUIABREV = dr.GetString(iEQUIABREV);

                    int iEVENINI = dr.GetOrdinal(helper.EVENINI);
                    if (!dr.IsDBNull(iEVENINI)) entity.EVENINI = dr.GetDateTime(iEVENINI);

                    int iCAUSAEVENABREV = dr.GetOrdinal(helper.CAUSAEVENABREV);
                    if (!dr.IsDBNull(iCAUSAEVENABREV)) entity.CAUSAEVENABREV = dr.GetString(iCAUSAEVENABREV);

                    int iSUBCAUSAABREV = dr.GetOrdinal(helper.SUBCAUSAABREV);
                    if (!dr.IsDBNull(iSUBCAUSAABREV)) entity.SUBCAUSAABREV = dr.GetString(iSUBCAUSAABREV);

                    int iEVENINTERRUP = dr.GetOrdinal(helper.EVENINTERRUP);
                    if (!dr.IsDBNull(iEVENINTERRUP)) entity.EVENINTERRUP = dr.GetString(iEVENINTERRUP);

                    int iEVENRELEVANTE = dr.GetOrdinal(helper.EVENRELEVANTE);
                    if (!dr.IsDBNull(iEVENRELEVANTE)) entity.EVENRELEVANTE = dr.GetInt32(iEVENRELEVANTE);

                    int iEVENASUNTO = dr.GetOrdinal(helper.EVENASUNTO);
                    if (!dr.IsDBNull(iEVENASUNTO)) entity.EVENASUNTO = dr.GetString(iEVENASUNTO);

                    int iEVENDESC = dr.GetOrdinal(helper.EVENDESC);
                    if (!dr.IsDBNull(iEVENDESC)) entity.EVENDESC = dr.GetString(iEVENDESC);

                    int iEVENFIN = dr.GetOrdinal(helper.EVENFIN);
                    if (!dr.IsDBNull(iEVENFIN)) entity.EVENFIN = dr.GetDateTime(iEVENFIN);

                    int iLASTUSER = dr.GetOrdinal(helper.LASTUSER);
                    if (!dr.IsDBNull(iLASTUSER)) entity.LASTUSER = dr.GetString(iLASTUSER);

                    int iLASTDATE = dr.GetOrdinal(helper.LASTDATE);
                    if (!dr.IsDBNull(iLASTDATE)) entity.LASTDATE = dr.GetDateTime(iLASTDATE);

                    int iEVENCODI = dr.GetOrdinal(helper.EVENCODI);
                    if (!dr.IsDBNull(iEVENCODI)) entity.EVENCODI = dr.GetInt32(iEVENCODI);

                    int iAFECODI = dr.GetOrdinal(helper.AFECODI);
                    if (!dr.IsDBNull(iAFECODI)) entity.AFECODI = dr.GetInt32(iAFECODI);

                    int iEVENDESCCTAF = dr.GetOrdinal(helper.EVENDESCCTAF);
                    if (!dr.IsDBNull(iEVENDESCCTAF)) entity.EVENDESCCTAF = dr.GetString(iEVENDESCCTAF);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        public List<EventoDTO> ListarEventosSCO(AnalisisFallaDTO dto)
        {
            EventoDTO entity = null;
            List<EventoDTO> listEntity = new List<EventoDTO>();

            string query = string.Format(helper.SqlListarEventosSCO, dto.AFEANIO, dto.AFECORR);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new EventoDTO();

                    int iEvenini = dr.GetOrdinal(helper.EVENINI);
                    int iEvenasunto = dr.GetOrdinal(helper.EVENASUNTO);
                    int iAfecodi = dr.GetOrdinal(helper.Afecodi);
                    int iEvencodi = dr.GetOrdinal(helper.EVENCODI);

                    if (!dr.IsDBNull(iEvenini)) entity.EVENINI = dr.GetDateTime(iEvenini);
                    if (!dr.IsDBNull(iEvenasunto)) entity.EVENASUNTO = dr.GetString(iEvenasunto);
                    if (!dr.IsDBNull(iAfecodi)) entity.AFECODI = dr.GetInt32(iAfecodi);
                    if (!dr.IsDBNull(iEvencodi)) entity.EVENCODI = dr.GetInt32(iEvencodi);

                    listEntity.Add(entity);
                }

            }

            return listEntity;
        }
        public void ActualizarCodEvento(int evencodi,int afecorr)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlActualizarCodEvento, evencodi, afecorr));
            dbProvider.ExecuteNonQuery(command);
        }
        public void ActualizarEventoxAfecodi(AnalisisFallaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlActualizarEventoxAfecodi, entity.AFECODISCO, entity.AFERMC,entity.AFEERACMF,
                entity.AFERACMT,entity.AFEEDAGSF,entity.AFERESPONSABLE,entity.AFEESTADO,entity.AFEESTADOMOTIVO));
            dbProvider.ExecuteNonQuery(command);
        }
        public bool ActualizarEventoAF(AnalisisFallaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlActualizarEventoAF, entity.AFECODI,
                entity.AFECITFECHAELABU, entity.AFEREUFECHAPROGU, entity.AFEREUHORAPROG, entity.AFECONVTIPOREUNION,
                entity.AFERCTAEOBSERVACION, entity.AFEITFECHAELABU, entity.AFEITRDJRFECHAENVIOU, entity.AFEITRDJRFECHARECEPU,
                entity.AFEITRDOFECHAENVIOU, entity.AFEITRDOFECHARECEPU,//16
                entity.AFEITRDJRESTADO, entity.AFEITRDOESTADO, entity.LASTUSER, entity.LASTDATEstr,//20
                entity.AFEIMPUGNA, entity.AFEFZAMAYOR, entity.AFEITDECFECHANOMINALU, entity.AFEITDECFECHAELABU,
                entity.AFEEMPRESPNINGUNA, entity.AFEEMPCOMPNINGUNA, entity.AFEITPITFFECHAFecha, entity.AFSALA, entity.AFEREUHORINI, entity.AFEREUHORFIN,entity.AFELIMATENCOMENU));
            int result = dbProvider.ExecuteNonQuery(command);
            return result > 0;
        }
        public int ObtieneCantInformesCtaf(int afecodi, int emprcodi, string afiversion)
        {
            String sql = String.Format(this.helper.SqlObtieneCantInformesCtaf, afecodi, emprcodi, afiversion);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                return Convert.ToInt32(result);
            }

            return 0;
        }
        public EventoDTO EventoAsociadoCtaf(int evencodi)
        {
            EventoDTO entity = new EventoDTO();
            String sql = String.Format(this.helper.SqlListadoEventoDTOAsoCtaf, evencodi);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int iAFECODI = dr.GetOrdinal("AFECODI");
                    int iCODIGO = dr.GetOrdinal("CODIGO");
                    int iNOMBRE_EVENTO = dr.GetOrdinal("NOMBRE_EVENTO");
                    int iINTERRUMPIDO = dr.GetOrdinal("INTERRUMPIDO");
                    int iFECHA_EVENTO = dr.GetOrdinal("FECHA_EVENTO");
                    int iFECHA_REUNION = dr.GetOrdinal("FECHA_REUNION");
                    int iFECHA_INFORME = dr.GetOrdinal("FECHA_INFORME");
                    int iREVISADO_DJR = dr.GetOrdinal("REVISADO_DJR");
                    int iREVISADO_DO = dr.GetOrdinal("REVISADO_DO");
                    int iPUBLICADO = dr.GetOrdinal("PUBLICADO");
                    int iESTADO = dr.GetOrdinal("ESTADO");
                    int iIMPUG = dr.GetOrdinal("IMPUG");
                    int iRESPONSABLE = dr.GetOrdinal("RESPONSABLE");
                    int iINF_TECNICO = dr.GetOrdinal("INF_TECNICO");
                    int iAFEFZAMAYOR = dr.GetOrdinal("AFEFZAMAYOR");
                    int iEVENCODI = dr.GetOrdinal("EVENCODI");
                    int iAFEERACMF = dr.GetOrdinal("AFEERACMF");

                        

                    if (!dr.IsDBNull(iAFECODI)) entity.AFECODI = dr.GetInt32(iAFECODI);
                    if (!dr.IsDBNull(iCODIGO)) entity.CODIGO = dr.GetString(iCODIGO);
                    if (!dr.IsDBNull(iNOMBRE_EVENTO)) entity.NOMBRE_EVENTO = dr.GetString(iNOMBRE_EVENTO);
                    if (!dr.IsDBNull(iINTERRUMPIDO)) entity.INTERRUMPIDO = dr.GetString(iINTERRUMPIDO);
                    if (!dr.IsDBNull(iFECHA_EVENTO)) entity.FECHA_EVENTO = dr.GetString(iFECHA_EVENTO);
                    if (!dr.IsDBNull(iFECHA_REUNION)) entity.FECHA_REUNION = dr.GetString(iFECHA_REUNION);
                    if (!dr.IsDBNull(iFECHA_INFORME)) entity.FECHA_INFORME = dr.GetString(iFECHA_INFORME);
                    if (!dr.IsDBNull(iREVISADO_DJR)) entity.REVISADO_DJR = dr.GetString(iREVISADO_DJR);
                    if (!dr.IsDBNull(iREVISADO_DO)) entity.REVISADO_DO = dr.GetString(iREVISADO_DO);
                    if (!dr.IsDBNull(iPUBLICADO)) entity.PUBLICADO = dr.GetString(iPUBLICADO);
                    if (!dr.IsDBNull(iESTADO)) entity.ESTADO = dr.GetString(iESTADO);
                    if (!dr.IsDBNull(iIMPUG)) entity.IMPUG = dr.GetString(iIMPUG);
                    if (!dr.IsDBNull(iRESPONSABLE)) entity.RESPONSABLE = dr.GetString(iRESPONSABLE);
                    if (!dr.IsDBNull(iINF_TECNICO)) entity.INF_TECNICO = dr.GetString(iINF_TECNICO);
                    if (!dr.IsDBNull(iAFEFZAMAYOR)) entity.AFEFZAMAYOR = dr.GetString(iAFEFZAMAYOR);
                    if (!dr.IsDBNull(iEVENCODI)) entity.EVENCODI = dr.GetInt32(iEVENCODI);
                    if (!dr.IsDBNull(iAFEERACMF)) entity.Afeeracmf = dr.GetString(iAFEERACMF);
                }
            }

            return entity;
        }
        public EventoDTO InterrupcionAsoCtaf(int evencodi)
        {
            EventoDTO entity = new EventoDTO();
            String sql = String.Format(this.helper.SqlInterrupcionAsoCtaf, evencodi);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int iAFECODI = dr.GetOrdinal("AFECODI");
                    int iEVENCODI = dr.GetOrdinal("EVENCODI");
                    int iCODIGO = dr.GetOrdinal("CODIGO");
                    int iNOMBRE_EVENTO = dr.GetOrdinal("NOMBRE_EVENTO");
                    int iINTERRUMPIDO = dr.GetOrdinal("INTERRUMPIDO");
                    int iERACMF = dr.GetOrdinal("ERACMF");
                    int iFECHA_EVENTO = dr.GetOrdinal("FECHA_EVENTO");
                    int IFECHA_INTERRUPCION = dr.GetOrdinal("FECHA_INTERRUPCION");
                    int iPLAZO_ENVIO = dr.GetOrdinal("PLAZO_ENVIO");
                    int IAfeplazofechaampl = dr.GetOrdinal("AFEPLAZOFECHAAMPL");
                    int iReportado = dr.GetOrdinal("REPORTADO");
                    int iAfecorr = dr.GetOrdinal("AFECORR");
                    int iAfeanio = dr.GetOrdinal("AFEANIO");

                    if (!dr.IsDBNull(iAFECODI)) entity.AFECODI = dr.GetInt32(iAFECODI);
                    if (!dr.IsDBNull(iEVENCODI)) entity.EVENCODI = dr.GetInt32(iEVENCODI);
                    if (!dr.IsDBNull(iCODIGO)) entity.CODIGO = dr.GetString(iCODIGO);
                    if (!dr.IsDBNull(iNOMBRE_EVENTO)) entity.NOMBRE_EVENTO = dr.GetString(iNOMBRE_EVENTO);
                    if (!dr.IsDBNull(iINTERRUMPIDO)) entity.INTERRUMPIDO = dr.GetString(iINTERRUMPIDO);
                    if (!dr.IsDBNull(iERACMF)) entity.ERACMF = dr.GetString(iERACMF);
                    if (!dr.IsDBNull(iFECHA_EVENTO)) entity.FechaEvento = dr.GetDateTime(iFECHA_EVENTO);
                    if (!dr.IsDBNull(IFECHA_INTERRUPCION)) entity.Afefechainterr = dr.GetDateTime(IFECHA_INTERRUPCION);
                    if (!dr.IsDBNull(iPLAZO_ENVIO)) entity.Afeplazofecha = dr.GetDateTime(iPLAZO_ENVIO);
                    if (!dr.IsDBNull(IAfeplazofechaampl)) entity.Afeplazofechaampl = dr.GetDateTime(IAfeplazofechaampl);

                    int iEVENINI = dr.GetOrdinal(helper.EVENINI);
                    if (!dr.IsDBNull(iEVENINI)) entity.EVENINI = dr.GetDateTime(iEVENINI);

                    int iEVENFIN = dr.GetOrdinal(helper.EVENFIN);
                    if (!dr.IsDBNull(iEVENFIN)) entity.EVENFIN = dr.GetDateTime(iEVENFIN);

                    int iEVENASUNTO = dr.GetOrdinal(helper.EVENASUNTO);
                    if (!dr.IsDBNull(iEVENASUNTO)) entity.EVENASUNTO = dr.GetString(iEVENASUNTO);

                    if (!dr.IsDBNull(iReportado)) entity.Reportado = dr.GetString(iReportado);

                    if (!dr.IsDBNull(iAfecorr)) entity.Afecorr = dr.GetInt32(iAfecorr).ToString();
                    if (!dr.IsDBNull(iAfeanio)) entity.Afeanio = dr.GetInt32(iAfeanio).ToString();
                }
            }

            return entity;
        }
        public List<EmpresaInvolucradaDTO> ObtenerEmpresasInvolucradaxEvencodi(int evencodi)
        {
            List<EmpresaInvolucradaDTO> entitys = new List<EmpresaInvolucradaDTO>();
            try
            {
                string query = string.Format(helper.SqlObtenerEmpresaInvolucradaxEvencodi, evencodi);
                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        EmpresaInvolucradaDTO entity = new EmpresaInvolucradaDTO();

                        int iAFECODI = dr.GetOrdinal("AFECODI");
                        int iEMPRCODI = dr.GetOrdinal("EMPRCODI");
                        int iAFIVERSION = dr.GetOrdinal("AFIVERSION");
                        int iEMPRNOMB = dr.GetOrdinal("EMPRNOMB");
                        int iVERSION = dr.GetOrdinal("VERSION");
                        int iCUMPLIMIENTO = dr.GetOrdinal("CUMPLIMIENTO");
                        int iAFIEXTENSION = dr.GetOrdinal("AFIEXTENSION");
                        int iAFIMENSAJE = dr.GetOrdinal("AFIMENSAJE");
                        int iAFIFECHAINF = dr.GetOrdinal("AFIFECHAINF");
                        int iLASTUSER = dr.GetOrdinal("LASTUSER");
                        int iLASTDATE = dr.GetOrdinal("LASTDATE");

                        if (!dr.IsDBNull(iAFECODI)) entity.AFECODI = dr.GetInt32(iAFECODI);
                        if (!dr.IsDBNull(iEMPRCODI)) entity.EMPRCODI = dr.GetInt32(iEMPRCODI);
                        if (!dr.IsDBNull(iAFIVERSION)) entity.AFIVERSION = dr.GetValue(iAFIVERSION).ToString();
                        if (!dr.IsDBNull(iEMPRNOMB)) entity.EMPRNOMB = dr.GetString(iEMPRNOMB);
                        if (!dr.IsDBNull(iVERSION)) entity.VERSION = dr.GetString(iVERSION);
                        if (!dr.IsDBNull(iCUMPLIMIENTO)) entity.CUMPLIMIENTO = dr.GetString(iCUMPLIMIENTO);
                        if (!dr.IsDBNull(iAFIEXTENSION)) entity.AFIEXTENSION = dr.GetString(iAFIEXTENSION);
                        if (!dr.IsDBNull(iAFIMENSAJE)) entity.AFIMENSAJE = dr.GetString(iAFIMENSAJE);
                        if (!dr.IsDBNull(iAFIFECHAINF)) entity.AFIFECHAINF = dr.GetDateTime(iAFIFECHAINF);
                        if (!dr.IsDBNull(iLASTUSER)) entity.LASTUSER = dr.GetString(iLASTUSER);
                        if (!dr.IsDBNull(iLASTDATE)) entity.LASTDATE = dr.GetDateTime(iLASTDATE);


                        entitys.Add(entity);
                    }
                }
            }
            catch (Exception ex)
            {
                entitys = null;
            }
            return entitys;
        }
        public AnalisisFallaDTO ObtenerAnalisisFallaxEvento(int evencodi)
        {

            AnalisisFallaDTO entity = new AnalisisFallaDTO();
            try
            {
                string query = string.Format(helper.SqlObtenerAnalisisFallaxEvento, evencodi);
                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        int iAFECODI = dr.GetOrdinal("AFECODI");
                        int iEVENCODI = dr.GetOrdinal("EVENCODI");
                        int iAFEANIO = dr.GetOrdinal("AFEANIO");
                        int iAFECORR = dr.GetOrdinal("AFECORR");                      
                        int iCODIGO = dr.GetOrdinal("CODIGO");
                        int iEVENINI = dr.GetOrdinal("EVENINI");
                        int iEVENASUNTO = dr.GetOrdinal("EVENASUNTO");
                        int iEMPRNOMB = dr.GetOrdinal("EMPRNOMB");
                        int iLASTUSER = dr.GetOrdinal("LASTUSER"); 
                        int iLASTDATE = dr.GetOrdinal("LASTDATE");

                        if (!dr.IsDBNull(iAFECODI)) entity.AFECODI = dr.GetInt32(iAFECODI);
                        if (!dr.IsDBNull(iEVENCODI)) entity.EVENCODI = Convert.ToInt32(dr.GetDecimal(iEVENCODI));
                        if (!dr.IsDBNull(iAFEANIO)) entity.AFEANIO = dr.GetInt32(iAFEANIO);
                        if (!dr.IsDBNull(iAFECORR)) entity.AFECORR = dr.GetInt32(iAFECORR);
                        if (!dr.IsDBNull(iCODIGO)) entity.CODIGO = dr.GetString(iCODIGO);
                        if (!dr.IsDBNull(iEVENINI)) entity.EVENINI = dr.GetDateTime(iEVENINI);
                        if (!dr.IsDBNull(iEVENASUNTO)) entity.EVENASUNTO = dr.GetString(iEVENASUNTO);
                        if (!dr.IsDBNull(iEMPRNOMB)) entity.EMPRNOMB = dr.GetString(iEMPRNOMB);
                        if (!dr.IsDBNull(iLASTUSER)) entity.LASTUSER = dr.GetString(iLASTUSER);
                        if (!dr.IsDBNull(iLASTDATE)) entity.LASTDATE = dr.GetDateTime(iLASTDATE);
                    }
                }
            }
            catch (Exception ex)
            {
                entity = null;
            }
            return entity;
        }
        public bool ActualizarRecomendacionAO(int afrrec, string evenrcmctaf)
        {
            int result = 0;
            string query = string.Format(helper.SqlActualizarRecomendacionAO, afrrec, evenrcmctaf);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            result = dbProvider.ExecuteNonQuery(command);
            return result > 0;
        }
        public EmpresaRecomendacionDTO ObtenerEmpresaRecomendacion(int afrrec)
        {
            EmpresaRecomendacionDTO entity = new EmpresaRecomendacionDTO();
            try
            {
                string query = string.Format(helper.SqlObtenerEmpresaRecomendacion, afrrec);
                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        int iAFRREC = dr.GetOrdinal("AFRREC");
                        int iAFECODI = dr.GetOrdinal("AFECODI");
                        int iEVENCODI = dr.GetOrdinal("EVENCODI");                        
                        int iAFRCORR = dr.GetOrdinal("AFRCORR");
                        int iEMPRCODI = dr.GetOrdinal("EMPRCODI");
                        int iAFRRECOMEND = dr.GetOrdinal("AFRRECOMEND");
                        int iNROREGRESPUESTA = dr.GetOrdinal("NROREGRESPUESTA");
                        int iEVENRCMCTAF = dr.GetOrdinal("EVENRCMCTAF");
                        int iLASTDATE = dr.GetOrdinal("LASTDATE");
                        int iEVENASUNTO = dr.GetOrdinal("EVENASUNTO");
                        int iIDEQUIPO = dr.GetOrdinal("IDEQUIPO");
                        int iIDSUBESTACION = dr.GetOrdinal("IDSUBESTACION");

                        if (!dr.IsDBNull(iAFRREC)) entity.AFRREC = dr.GetInt32(iAFRREC);
                        if (!dr.IsDBNull(iAFECODI)) entity.AFECODI = dr.GetInt32(iAFECODI);
                        if (!dr.IsDBNull(iEVENCODI)) entity.EVENCODI = dr.GetInt32(iEVENCODI);
                        if (!dr.IsDBNull(iAFRCORR)) entity.AFRCORR = dr.GetInt32(iAFRCORR);
                        if (!dr.IsDBNull(iEMPRCODI)) entity.EMPRCODI = dr.GetInt32(iEMPRCODI);
                        if (!dr.IsDBNull(iAFRRECOMEND)) entity.AFRRECOMEND = dr.GetString(iAFRRECOMEND);
                        if (!dr.IsDBNull(iNROREGRESPUESTA)) entity.NROREGRESPUESTA = dr.GetString(iNROREGRESPUESTA);
                        if (!dr.IsDBNull(iEVENRCMCTAF)) entity.EVENRCMCTAF = dr.GetString(iEVENRCMCTAF);
                        if (!dr.IsDBNull(iLASTDATE)) entity.LASTDATE = dr.GetDateTime(iLASTDATE);
                        if (!dr.IsDBNull(iEVENASUNTO)) entity.EVENASUNTO = dr.GetString(iEVENASUNTO);
                        if (!dr.IsDBNull(iIDEQUIPO)) entity.IDEQUIPO = dr.GetInt32(iIDEQUIPO);
                        if (!dr.IsDBNull(iIDSUBESTACION)) entity.IDSUBESTACION = dr.GetInt32(iIDSUBESTACION);
                    }
                }
            }
            catch (Exception ex)
            {
                entity = null;
            }
            return entity;
        }
        public void ActualizarEventoAO(int evencodi, string evenrcmctaf)
        {
            string query = string.Format(helper.SqlActualizarEventoAO, evencodi, evenrcmctaf);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(command);
        }
        #endregion

        #region Informes SGI

        public List<EventoDTO> ObtenerDetalleEventosInformeSemanal(DateTime fechaInicio, DateTime fechaFin)
        {
            String query = String.Format(helper.SqlObtenerDetalleEventosInformeSemanal, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<EventoDTO> entitys = new List<EventoDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EventoDTO entity = new EventoDTO();

                    int iEVENCODI = dr.GetOrdinal(helper.EVENCODI);
                    if (!dr.IsDBNull(iEVENCODI)) entity.EVENCODI = Convert.ToInt32(dr.GetValue(iEVENCODI));

                    int iEVENASUNTO = dr.GetOrdinal(helper.EVENASUNTO);
                    if (!dr.IsDBNull(iEVENASUNTO)) entity.EVENASUNTO = dr.GetString(iEVENASUNTO);

                    int iSUBCAUSADESC = dr.GetOrdinal(helper.SUBCAUSAABREV);
                    if (!dr.IsDBNull(iSUBCAUSADESC)) entity.SUBCAUSAABREV = dr.GetString(iSUBCAUSADESC);

                    int iEVENFIN = dr.GetOrdinal(helper.EVENFIN);
                    if (!dr.IsDBNull(iEVENFIN)) entity.EVENFIN = dr.GetDateTime(iEVENFIN);

                    int iDURACION = dr.GetOrdinal(helper.Interrminu);
                    if (!dr.IsDBNull(iDURACION)) entity.Interrminu = dr.GetDecimal(iDURACION);

                    int iENERGIA = dr.GetOrdinal(helper.ENERGIAINTERRUMPIDA);
                    if (!dr.IsDBNull(iENERGIA)) entity.ENERGIAINTERRUMPIDA = dr.GetDecimal(iENERGIA);

                    int iFAMNOMB = dr.GetOrdinal(helper.FAMNOMB);
                    if (!dr.IsDBNull(iFAMNOMB)) entity.FAMNOMB = dr.GetString(iFAMNOMB);

                    int iEMPRNOMB = dr.GetOrdinal(helper.EMPRNOMB);
                    if (!dr.IsDBNull(iEMPRNOMB)) entity.EMPRNOMB = dr.GetString(iEMPRNOMB);

                    int iEQUINOMB = dr.GetOrdinal(helper.EQUINOMB);
                    if (!dr.IsDBNull(iEQUINOMB)) entity.EQUIABREV = dr.GetString(iEQUINOMB);
                    
                    int iEVENINI = dr.GetOrdinal(helper.EVENINI);
                    if (!dr.IsDBNull(iEVENINI)) entity.EVENINI = dr.GetDateTime(iEVENINI);
                                        
                    int iEVENDESC = dr.GetOrdinal(helper.EVENDESC);
                    if (!dr.IsDBNull(iEVENDESC)) entity.EVENDESC = dr.GetString(iEVENDESC);

                    int iINTERRMW = dr.GetOrdinal(helper.Interrmw);
                    if (!dr.IsDBNull(iINTERRMW)) entity.INTERRMW = dr.GetDecimal(iINTERRMW);

                    int iBAJOMW = dr.GetOrdinal(helper.BAJOMW);
                    if (!dr.IsDBNull(iBAJOMW)) entity.BAJOMW = dr.GetDecimal(iBAJOMW);

                    int iAREADESC = dr.GetOrdinal(helper.AREADESC);
                    if (!dr.IsDBNull(iAREADESC)) entity.AREADESC = dr.GetString(iAREADESC);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #endregion
        public void ActualizarDesEventoAF(int evencodi, string evendescctaf)
        {
            string query = string.Format(helper.SqlUpdateDesEventoAF, evencodi, evendescctaf);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(command);
        }

        public SiDirectorioDTO ObtenerDirectorio(int dircodi)
        {
            try
            {
                SiDirectorioDTO entity = new SiDirectorioDTO();
                string query = string.Format(helper.SqlObtenerDirector, dircodi);
                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        int iCodigoDirector = dr.GetOrdinal(helper.CodigoDirector);
                        if (!dr.IsDBNull(iCodigoDirector)) entity.DirectorioCodigo = dr.GetInt32(iCodigoDirector);

                        int iNombreCompleto = dr.GetOrdinal(helper.NombreCompletoDirector);
                        if (!dr.IsDBNull(iNombreCompleto)) entity.DirectorioNombre = dr.GetString(iNombreCompleto);

                        int iDircorreo = dr.GetOrdinal(helper.Dircorreo);
                        if (!dr.IsDBNull(iDircorreo)) entity.Dircorreo = dr.GetString(iDircorreo);

                        int iDirestado = dr.GetOrdinal(helper.Direstado);
                        if (!dr.IsDBNull(iDirestado)) entity.iDirestado = dr.GetString(iDirestado);
                    }
                }
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex); ;
            }

        }

        public List<EventoDTO> ConsultarAnalisisFallasAnio(int anio)
        {
            List<EventoDTO> entitys = new List<EventoDTO>();
            try
            {
                string query = string.Format(helper.SqlConsultarAnalisisFallasxAnio, anio);
                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        EventoDTO entity = new EventoDTO();

                        int iAFECODI = dr.GetOrdinal("AFECODI");
                        int iCODIGO = dr.GetOrdinal("CODIGO");
                        int iFECHA_EVENTO = dr.GetOrdinal("FECHA_EVENTO");
                        int iEVENCODI = dr.GetOrdinal("EVENCODI");
                        int iAFEREUFECHAPROG = dr.GetOrdinal("AFEREUFECHAPROG");
                        int iAFEITDECFECHAELAB = dr.GetOrdinal("AFEITDECFECHAELAB");

                        if (!dr.IsDBNull(iAFECODI)) entity.AFECODI = dr.GetInt32(iAFECODI);
                        if (!dr.IsDBNull(iCODIGO)) entity.CODIGO = dr.GetString(iCODIGO);
                        if (!dr.IsDBNull(iFECHA_EVENTO)) entity.FECHA_EVENTO = dr.GetString(iFECHA_EVENTO);
                        if (!dr.IsDBNull(iEVENCODI)) entity.EVENCODI = dr.GetInt32(iEVENCODI);
                        if (!dr.IsDBNull(iAFEREUFECHAPROG)) entity.AFEREUFECHAPROG = dr.GetDateTime(iAFEREUFECHAPROG);
                        if (!dr.IsDBNull(iAFEITDECFECHAELAB)) entity.AFEITDECFECHAELAB = dr.GetDateTime(iAFEITDECFECHAELAB);

                        entitys.Add(entity);
                    }
                }
            }
            catch (Exception ex)
            {
                entitys = null;
            }
            return entitys;
        }

        public List<AnalisisFallaDTO> ListarAnalisisFallaxEvento(int evencodi)
        {

            List<AnalisisFallaDTO> entitys = new List<AnalisisFallaDTO>();
            try
            {
                string query = string.Format(helper.SqlListarAnalisisFallaxEvento, evencodi);
                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        AnalisisFallaDTO entity = new AnalisisFallaDTO();
                        int iAFECODI = dr.GetOrdinal("AFECODI");
                        int iEVENCODI = dr.GetOrdinal("EVENCODI");
                        int iAFEANIO = dr.GetOrdinal("AFEANIO");
                        int iAFECORR = dr.GetOrdinal("AFECORR");
                        int iCODIGO = dr.GetOrdinal("CODIGO");
                        int iEVENINI = dr.GetOrdinal("EVENINI");

                        if (!dr.IsDBNull(iAFECODI)) entity.AFECODI = dr.GetInt32(iAFECODI);
                        if (!dr.IsDBNull(iEVENCODI)) entity.EVENCODI = Convert.ToInt32(dr.GetDecimal(iEVENCODI));
                        if (!dr.IsDBNull(iAFEANIO)) entity.AFEANIO = dr.GetInt32(iAFEANIO);
                        if (!dr.IsDBNull(iAFECORR)) entity.AFECORR = dr.GetInt32(iAFECORR);
                        if (!dr.IsDBNull(iCODIGO)) entity.CODIGO = dr.GetString(iCODIGO);
                        if (!dr.IsDBNull(iEVENINI)) entity.EVENINI = dr.GetDateTime(iEVENINI);

                        entitys.Add(entity);
                    }
                }
            }
            catch (Exception ex)
            {
                entitys = null;
            }
            return entitys;
        }
    }
}

