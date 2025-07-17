// =======================================================================================
//
// (c) COES Sinac. Todos los derechos reservados. 
//
// Autor: Henry Manuel Díaz Tuesta
// Acronimo: HDT
// Requerimiento: alpha
//
// Fecha creacion: 10/04/2017
// Descripcion: Archivo para la atencion del requerimiento.
//
// Historial de cambios:
// 
// Correlativo	Fecha		Requerimiento		Comentario
//
// =======================================================================================

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla IIO_LOG_IMPORTACION
    /// </summary>
    public class IioLogImportacionRepository : RepositoryBase, IIioLogImportacionRepository
    {
        private readonly IioLogImportacionHelper helper = new IioLogImportacionHelper();

        public IioLogImportacionRepository(string strConn)
            : base(strConn)
        {
            
        }
        public IioLogImportacionDTO GetById(int uLogCodi)
        {
            throw new NotImplementedException();
        }

        public List<IioLogImportacionDTO> List()
        {
            throw new NotImplementedException();
        }

        public int Save(IioLogImportacionDTO oIioLogImportacionDTO)
        {
            throw new NotImplementedException();
        }

        //- alpha.HDT - Inicio 10/04/2017: Cambio para atender el requerimiento.
        public List<IioLogImportacionIncidenteDTO> GetDuplicadosConfiguracionCOES()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDuplicadosConfiguracionCOES);

            List<IioLogImportacionIncidenteDTO> 
                lIioLogImportacionIncidenteDTO = new List<IioLogImportacionIncidenteDTO>();

            IioLogImportacionIncidenteDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new IioLogImportacionIncidenteDTO();

                    int iMensaje = dr.GetOrdinal(helper.Mensaje);
                    if (!dr.IsDBNull(iMensaje)) entity.Mensaje = dr.GetString(iMensaje);

                    lIioLogImportacionIncidenteDTO.Add(entity);

                }
            }

            return lIioLogImportacionIncidenteDTO;
        }

        public List<IioLogImportacionIncidenteDTO> GetIncidentesSinPuntoMedicionCOES()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlIncidentesSinPuntoMedicionCOES);

            List<IioLogImportacionIncidenteDTO>
                lIioLogImportacionIncidenteDTO = new List<IioLogImportacionIncidenteDTO>();

            IioLogImportacionIncidenteDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new IioLogImportacionIncidenteDTO();

                    int iMensaje = dr.GetOrdinal(helper.Mensaje);
                    if (!dr.IsDBNull(iMensaje)) entity.Mensaje = dr.GetString(iMensaje);

                    lIioLogImportacionIncidenteDTO.Add(entity);

                }
            }

            return lIioLogImportacionIncidenteDTO;
        }

        public int GetCorrelativoDisponibleLogImportacionSicli()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int correlativo = 1;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    int iCorrelativo = dr.GetOrdinal(helper.Correlativo);
                    if (!dr.IsDBNull(iCorrelativo)) correlativo = dr.GetInt32(iCorrelativo);
                }
            }

            return correlativo;
        }

        public void EliminarIncidenciasImportacionSicli(int Rcimcodi, string periodo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteByRcimcodi);
            dbProvider.AddInParameter(command, helper.Rcimcodi, DbType.Int32, Rcimcodi);
            dbProvider.AddInParameter(command, helper.Psiclicodi, DbType.String, periodo);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<IioLogImportacionDTO> GetIncidenciasImportacion(int rcImCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetIncidenciasImportacion);
            dbProvider.AddInParameter(command, helper.Rcimcodi, DbType.Int32, rcImCodi);

            List<IioLogImportacionDTO>
                lIioLogImportacionDTO = new List<IioLogImportacionDTO>();

            IioLogImportacionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);

                    lIioLogImportacionDTO.Add(entity);

                }
            }

            return lIioLogImportacionDTO;
        }

        public void SaveIioLogImportacion(IioLogImportacionDTO oIioLogImportacionDTO)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            dbProvider.AddInParameter(command, helper.Ulogcodi, DbType.Int32, oIioLogImportacionDTO.UlogCodi);
            dbProvider.AddInParameter(command, helper.Psiclicodi, DbType.String, oIioLogImportacionDTO.PsicliCodi);
            dbProvider.AddInParameter(command, helper.Ulogusucreacion, DbType.String, oIioLogImportacionDTO.UlogUsuCreacion);
            dbProvider.AddInParameter(command, helper.Ulogfeccreacion, DbType.DateTime, oIioLogImportacionDTO.UlogFecCreacion);
            dbProvider.AddInParameter(command, helper.Ulogproceso, DbType.String, oIioLogImportacionDTO.UlogProceso);
            dbProvider.AddInParameter(command, helper.Ulogtablaafectada, DbType.String, oIioLogImportacionDTO.UlogTablaAfectada);
            dbProvider.AddInParameter(command, helper.Ulognroregistrosafectados, DbType.Int32, oIioLogImportacionDTO.UlogNroRegistrosAfectados);
            dbProvider.AddInParameter(command, helper.Ulogmensaje, DbType.String, oIioLogImportacionDTO.UlogMensaje);
            dbProvider.AddInParameter(command, helper.Rcimcodi, DbType.Int32, oIioLogImportacionDTO.RcimCodi);
            dbProvider.AddInParameter(command, helper.Ulogtablacoes, DbType.String, oIioLogImportacionDTO.UlogTablaCOES);
            dbProvider.AddInParameter(command, helper.Ulogidregistrocoes, DbType.String, oIioLogImportacionDTO.UlogIdRegistroCOES);
            dbProvider.AddInParameter(command, helper.Ulogtipoincidencia, DbType.String, oIioLogImportacionDTO.UlogTipoIncidencia);

            dbProvider.ExecuteNonQuery(command);
        }

        //- alpha.HDT - 26/04/2017: Cambio para atender el requerimiento. 
        public List<IioTabla04DTO> GetDatosTabla04(string periodo, string empresasIn, string fechaDia)
        {
            string extensionWhere = " 1 = 1";

            if (empresasIn != string.Empty){
                extensionWhere = " SUMI.EMPRCODOSINERGMIN IN (" + empresasIn + ")";
            }
            if (fechaDia != string.Empty)
            {
                extensionWhere = extensionWhere + string.Format(" AND TO_CHAR(ME.MEDIFECHA,'YYYYMMDD') = '{0}' ", fechaDia);
            }

            string stQuery = string.Format(helper.SqlReporteTabla04
                                         , periodo
                                         , extensionWhere);

            DbCommand command = dbProvider.GetSqlStringCommand(stQuery);

            List<IioTabla04DTO> lIioTabla04DTO = new List<IioTabla04DTO>();

            IioTabla04DTO entidad = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entidad = new IioTabla04DTO();

                    int iIdSuministrador = dr.GetOrdinal(helper.IdSuministrador);
                    if (!dr.IsDBNull(iIdSuministrador)) entidad.IdSuministrador = dr.GetInt32(iIdSuministrador);

                    int iCodSuministradorSicli = dr.GetOrdinal(helper.CodSuministradorSicli);
                    if (!dr.IsDBNull(iCodSuministradorSicli)) entidad.CodSuministradorSicli = dr.GetString(iCodSuministradorSicli);

                    int iSuministradorSicli = dr.GetOrdinal(helper.SuministradorSicli);
                    if (!dr.IsDBNull(iSuministradorSicli)) entidad.SuministradorSicli = dr.GetString(iSuministradorSicli);

                    int iRuc = dr.GetOrdinal(helper.Ruc);
                    if (!dr.IsDBNull(iRuc)) entidad.Ruc = dr.GetString(iRuc);

                    int iUsuarioLibre = dr.GetOrdinal(helper.UsuarioLibre);
                    if (!dr.IsDBNull(iUsuarioLibre)) entidad.UsuarioLibre = dr.GetString(iUsuarioLibre);

                    int iCodSuministro = dr.GetOrdinal(helper.CodSuministro);
                    if (!dr.IsDBNull(iCodSuministro)) entidad.CodSuministro = dr.GetString(iCodSuministro);

                    int iNombrePtoMedicion = dr.GetOrdinal(helper.NombrePtoMedicion);
                    if (!dr.IsDBNull(iNombrePtoMedicion)) entidad.NombrePtoMedicion = dr.GetString(iNombrePtoMedicion);

                    int iFechaMedicion = dr.GetOrdinal(helper.FechaMedicion);
                    if (!dr.IsDBNull(iFechaMedicion)) entidad.FechaMedicion = dr.GetDateTime(iFechaMedicion);

                    int iH1 = dr.GetOrdinal(helper.H1);
                    if (!dr.IsDBNull(iH1)) entidad.H1 = dr.GetDecimal(iH1);

                    int iH2 = dr.GetOrdinal(helper.H2);
                    if (!dr.IsDBNull(iH2)) entidad.H2 = dr.GetDecimal(iH2);

                    int iH3 = dr.GetOrdinal(helper.H3);
                    if (!dr.IsDBNull(iH3)) entidad.H3 = dr.GetDecimal(iH3);

                    int iH4 = dr.GetOrdinal(helper.H4);
                    if (!dr.IsDBNull(iH4)) entidad.H4 = dr.GetDecimal(iH4);

                    int iH5 = dr.GetOrdinal(helper.H5);
                    if (!dr.IsDBNull(iH5)) entidad.H5 = dr.GetDecimal(iH5);

                    int iH6 = dr.GetOrdinal(helper.H6);
                    if (!dr.IsDBNull(iH6)) entidad.H6 = dr.GetDecimal(iH6);

                    int iH7 = dr.GetOrdinal(helper.H7);
                    if (!dr.IsDBNull(iH7)) entidad.H7 = dr.GetDecimal(iH7);

                    int iH8 = dr.GetOrdinal(helper.H8);
                    if (!dr.IsDBNull(iH8)) entidad.H8 = dr.GetDecimal(iH8);

                    int iH9 = dr.GetOrdinal(helper.H9);
                    if (!dr.IsDBNull(iH9)) entidad.H9 = dr.GetDecimal(iH9);

                    int iH10 = dr.GetOrdinal(helper.H10);
                    if (!dr.IsDBNull(iH10)) entidad.H10 = dr.GetDecimal(iH10);

                    int iH11 = dr.GetOrdinal(helper.H11);
                    if (!dr.IsDBNull(iH11)) entidad.H11 = dr.GetDecimal(iH11);

                    int iH12 = dr.GetOrdinal(helper.H12);
                    if (!dr.IsDBNull(iH12)) entidad.H12 = dr.GetDecimal(iH12);

                    int iH13 = dr.GetOrdinal(helper.H13);
                    if (!dr.IsDBNull(iH13)) entidad.H13 = dr.GetDecimal(iH13);

                    int iH14 = dr.GetOrdinal(helper.H14);
                    if (!dr.IsDBNull(iH14)) entidad.H14 = dr.GetDecimal(iH14);

                    int iH15 = dr.GetOrdinal(helper.H15);
                    if (!dr.IsDBNull(iH15)) entidad.H15 = dr.GetDecimal(iH15);

                    int iH16 = dr.GetOrdinal(helper.H16);
                    if (!dr.IsDBNull(iH16)) entidad.H16 = dr.GetDecimal(iH16);

                    int iH17 = dr.GetOrdinal(helper.H17);
                    if (!dr.IsDBNull(iH17)) entidad.H17 = dr.GetDecimal(iH17);

                    int iH18 = dr.GetOrdinal(helper.H18);
                    if (!dr.IsDBNull(iH18)) entidad.H18 = dr.GetDecimal(iH18);

                    int iH19 = dr.GetOrdinal(helper.H19);
                    if (!dr.IsDBNull(iH19)) entidad.H19 = dr.GetDecimal(iH19);

                    int iH20 = dr.GetOrdinal(helper.H20);
                    if (!dr.IsDBNull(iH20)) entidad.H20 = dr.GetDecimal(iH20);

                    int iH21 = dr.GetOrdinal(helper.H21);
                    if (!dr.IsDBNull(iH21)) entidad.H21 = dr.GetDecimal(iH21);

                    int iH22 = dr.GetOrdinal(helper.H22);
                    if (!dr.IsDBNull(iH22)) entidad.H22 = dr.GetDecimal(iH22);

                    int iH23 = dr.GetOrdinal(helper.H23);
                    if (!dr.IsDBNull(iH23)) entidad.H23 = dr.GetDecimal(iH23);

                    int iH24 = dr.GetOrdinal(helper.H24);
                    if (!dr.IsDBNull(iH24)) entidad.H24 = dr.GetDecimal(iH24);

                    int iH25 = dr.GetOrdinal(helper.H25);
                    if (!dr.IsDBNull(iH25)) entidad.H25 = dr.GetDecimal(iH25);

                    int iH26 = dr.GetOrdinal(helper.H26);
                    if (!dr.IsDBNull(iH26)) entidad.H26 = dr.GetDecimal(iH26);

                    int iH27 = dr.GetOrdinal(helper.H27);
                    if (!dr.IsDBNull(iH27)) entidad.H27 = dr.GetDecimal(iH27);

                    int iH28 = dr.GetOrdinal(helper.H28);
                    if (!dr.IsDBNull(iH28)) entidad.H28 = dr.GetDecimal(iH28);

                    int iH29 = dr.GetOrdinal(helper.H29);
                    if (!dr.IsDBNull(iH29)) entidad.H29 = dr.GetDecimal(iH29);

                    int iH30 = dr.GetOrdinal(helper.H30);
                    if (!dr.IsDBNull(iH30)) entidad.H30 = dr.GetDecimal(iH30);

                    int iH31 = dr.GetOrdinal(helper.H31);
                    if (!dr.IsDBNull(iH31)) entidad.H31 = dr.GetDecimal(iH31);

                    int iH32 = dr.GetOrdinal(helper.H32);
                    if (!dr.IsDBNull(iH32)) entidad.H32 = dr.GetDecimal(iH32);

                    int iH33 = dr.GetOrdinal(helper.H33);
                    if (!dr.IsDBNull(iH33)) entidad.H33 = dr.GetDecimal(iH33);

                    int iH34 = dr.GetOrdinal(helper.H34);
                    if (!dr.IsDBNull(iH34)) entidad.H34 = dr.GetDecimal(iH34);

                    int iH35 = dr.GetOrdinal(helper.H35);
                    if (!dr.IsDBNull(iH35)) entidad.H35 = dr.GetDecimal(iH35);

                    int iH36 = dr.GetOrdinal(helper.H36);
                    if (!dr.IsDBNull(iH36)) entidad.H36 = dr.GetDecimal(iH36);

                    int iH37 = dr.GetOrdinal(helper.H37);
                    if (!dr.IsDBNull(iH37)) entidad.H37 = dr.GetDecimal(iH37);

                    int iH38 = dr.GetOrdinal(helper.H38);
                    if (!dr.IsDBNull(iH38)) entidad.H38 = dr.GetDecimal(iH38);

                    int iH39 = dr.GetOrdinal(helper.H39);
                    if (!dr.IsDBNull(iH39)) entidad.H39 = dr.GetDecimal(iH39);

                    int iH40 = dr.GetOrdinal(helper.H40);
                    if (!dr.IsDBNull(iH40)) entidad.H40 = dr.GetDecimal(iH40);

                    int iH41 = dr.GetOrdinal(helper.H41);
                    if (!dr.IsDBNull(iH41)) entidad.H41 = dr.GetDecimal(iH41);

                    int iH42 = dr.GetOrdinal(helper.H42);
                    if (!dr.IsDBNull(iH42)) entidad.H42 = dr.GetDecimal(iH42);

                    int iH43 = dr.GetOrdinal(helper.H43);
                    if (!dr.IsDBNull(iH43)) entidad.H43 = dr.GetDecimal(iH43);

                    int iH44 = dr.GetOrdinal(helper.H44);
                    if (!dr.IsDBNull(iH44)) entidad.H44 = dr.GetDecimal(iH44);

                    int iH45 = dr.GetOrdinal(helper.H45);
                    if (!dr.IsDBNull(iH45)) entidad.H45 = dr.GetDecimal(iH45);

                    int iH46 = dr.GetOrdinal(helper.H46);
                    if (!dr.IsDBNull(iH46)) entidad.H46 = dr.GetDecimal(iH46);

                    int iH47 = dr.GetOrdinal(helper.H47);
                    if (!dr.IsDBNull(iH47)) entidad.H47 = dr.GetDecimal(iH47);

                    int iH48 = dr.GetOrdinal(helper.H48);
                    if (!dr.IsDBNull(iH48)) entidad.H48 = dr.GetDecimal(iH48);

                    int iH49 = dr.GetOrdinal(helper.H49);
                    if (!dr.IsDBNull(iH49)) entidad.H49 = dr.GetDecimal(iH49);

                    int iH50 = dr.GetOrdinal(helper.H50);
                    if (!dr.IsDBNull(iH50)) entidad.H50 = dr.GetDecimal(iH50);

                    int iH51 = dr.GetOrdinal(helper.H51);
                    if (!dr.IsDBNull(iH51)) entidad.H51 = dr.GetDecimal(iH51);

                    int iH52 = dr.GetOrdinal(helper.H52);
                    if (!dr.IsDBNull(iH52)) entidad.H52 = dr.GetDecimal(iH52);

                    int iH53 = dr.GetOrdinal(helper.H53);
                    if (!dr.IsDBNull(iH53)) entidad.H53 = dr.GetDecimal(iH53);

                    int iH54 = dr.GetOrdinal(helper.H54);
                    if (!dr.IsDBNull(iH54)) entidad.H54 = dr.GetDecimal(iH54);

                    int iH55 = dr.GetOrdinal(helper.H55);
                    if (!dr.IsDBNull(iH55)) entidad.H55 = dr.GetDecimal(iH55);

                    int iH56 = dr.GetOrdinal(helper.H56);
                    if (!dr.IsDBNull(iH56)) entidad.H56 = dr.GetDecimal(iH56);

                    int iH57 = dr.GetOrdinal(helper.H57);
                    if (!dr.IsDBNull(iH57)) entidad.H57 = dr.GetDecimal(iH57);

                    int iH58 = dr.GetOrdinal(helper.H58);
                    if (!dr.IsDBNull(iH58)) entidad.H58 = dr.GetDecimal(iH58);

                    int iH59 = dr.GetOrdinal(helper.H59);
                    if (!dr.IsDBNull(iH59)) entidad.H59 = dr.GetDecimal(iH59);

                    int iH60 = dr.GetOrdinal(helper.H60);
                    if (!dr.IsDBNull(iH60)) entidad.H60 = dr.GetDecimal(iH60);

                    int iH61 = dr.GetOrdinal(helper.H61);
                    if (!dr.IsDBNull(iH61)) entidad.H61 = dr.GetDecimal(iH61);

                    int iH62 = dr.GetOrdinal(helper.H62);
                    if (!dr.IsDBNull(iH62)) entidad.H62 = dr.GetDecimal(iH62);

                    int iH63 = dr.GetOrdinal(helper.H63);
                    if (!dr.IsDBNull(iH63)) entidad.H63 = dr.GetDecimal(iH63);

                    int iH64 = dr.GetOrdinal(helper.H64);
                    if (!dr.IsDBNull(iH64)) entidad.H64 = dr.GetDecimal(iH64);

                    int iH65 = dr.GetOrdinal(helper.H65);
                    if (!dr.IsDBNull(iH65)) entidad.H65 = dr.GetDecimal(iH65);

                    int iH66 = dr.GetOrdinal(helper.H66);
                    if (!dr.IsDBNull(iH66)) entidad.H66 = dr.GetDecimal(iH66);

                    int iH67 = dr.GetOrdinal(helper.H67);
                    if (!dr.IsDBNull(iH67)) entidad.H67 = dr.GetDecimal(iH67);

                    int iH68 = dr.GetOrdinal(helper.H68);
                    if (!dr.IsDBNull(iH68)) entidad.H68 = dr.GetDecimal(iH68);

                    int iH69 = dr.GetOrdinal(helper.H69);
                    if (!dr.IsDBNull(iH69)) entidad.H69 = dr.GetDecimal(iH69);

                    int iH70 = dr.GetOrdinal(helper.H70);
                    if (!dr.IsDBNull(iH70)) entidad.H70 = dr.GetDecimal(iH70);

                    int iH71 = dr.GetOrdinal(helper.H71);
                    if (!dr.IsDBNull(iH71)) entidad.H71 = dr.GetDecimal(iH71);

                    int iH72 = dr.GetOrdinal(helper.H72);
                    if (!dr.IsDBNull(iH72)) entidad.H72 = dr.GetDecimal(iH72);

                    int iH73 = dr.GetOrdinal(helper.H73);
                    if (!dr.IsDBNull(iH73)) entidad.H73 = dr.GetDecimal(iH73);

                    int iH74 = dr.GetOrdinal(helper.H74);
                    if (!dr.IsDBNull(iH74)) entidad.H74 = dr.GetDecimal(iH74);

                    int iH75 = dr.GetOrdinal(helper.H75);
                    if (!dr.IsDBNull(iH75)) entidad.H75 = dr.GetDecimal(iH75);

                    int iH76 = dr.GetOrdinal(helper.H76);
                    if (!dr.IsDBNull(iH76)) entidad.H76 = dr.GetDecimal(iH76);

                    int iH77 = dr.GetOrdinal(helper.H77);
                    if (!dr.IsDBNull(iH77)) entidad.H77 = dr.GetDecimal(iH77);

                    int iH78 = dr.GetOrdinal(helper.H78);
                    if (!dr.IsDBNull(iH78)) entidad.H78 = dr.GetDecimal(iH78);

                    int iH79 = dr.GetOrdinal(helper.H79);
                    if (!dr.IsDBNull(iH79)) entidad.H79 = dr.GetDecimal(iH79);

                    int iH80 = dr.GetOrdinal(helper.H80);
                    if (!dr.IsDBNull(iH80)) entidad.H80 = dr.GetDecimal(iH80);

                    int iH81 = dr.GetOrdinal(helper.H81);
                    if (!dr.IsDBNull(iH81)) entidad.H81 = dr.GetDecimal(iH81);

                    int iH82 = dr.GetOrdinal(helper.H82);
                    if (!dr.IsDBNull(iH82)) entidad.H82 = dr.GetDecimal(iH82);

                    int iH83 = dr.GetOrdinal(helper.H83);
                    if (!dr.IsDBNull(iH83)) entidad.H83 = dr.GetDecimal(iH83);

                    int iH84 = dr.GetOrdinal(helper.H84);
                    if (!dr.IsDBNull(iH84)) entidad.H84 = dr.GetDecimal(iH84);

                    int iH85 = dr.GetOrdinal(helper.H85);
                    if (!dr.IsDBNull(iH85)) entidad.H85 = dr.GetDecimal(iH85);

                    int iH86 = dr.GetOrdinal(helper.H86);
                    if (!dr.IsDBNull(iH86)) entidad.H86 = dr.GetDecimal(iH86);

                    int iH87 = dr.GetOrdinal(helper.H87);
                    if (!dr.IsDBNull(iH87)) entidad.H87 = dr.GetDecimal(iH87);

                    int iH88 = dr.GetOrdinal(helper.H88);
                    if (!dr.IsDBNull(iH88)) entidad.H88 = dr.GetDecimal(iH88);

                    int iH89 = dr.GetOrdinal(helper.H89);
                    if (!dr.IsDBNull(iH89)) entidad.H89 = dr.GetDecimal(iH89);

                    int iH90 = dr.GetOrdinal(helper.H90);
                    if (!dr.IsDBNull(iH90)) entidad.H90 = dr.GetDecimal(iH90);

                    int iH91 = dr.GetOrdinal(helper.H91);
                    if (!dr.IsDBNull(iH91)) entidad.H91 = dr.GetDecimal(iH91);

                    int iH92 = dr.GetOrdinal(helper.H92);
                    if (!dr.IsDBNull(iH92)) entidad.H92 = dr.GetDecimal(iH92);

                    int iH93 = dr.GetOrdinal(helper.H93);
                    if (!dr.IsDBNull(iH93)) entidad.H93 = dr.GetDecimal(iH93);

                    int iH94 = dr.GetOrdinal(helper.H94);
                    if (!dr.IsDBNull(iH94)) entidad.H94 = dr.GetDecimal(iH94);

                    int iH95 = dr.GetOrdinal(helper.H95);
                    if (!dr.IsDBNull(iH95)) entidad.H95 = dr.GetDecimal(iH95);

                    int iH96 = dr.GetOrdinal(helper.H96);
                    if (!dr.IsDBNull(iH96)) entidad.H96 = dr.GetDecimal(iH96);

                    int iCiiu = dr.GetOrdinal(helper.CIIU);
                    if (!dr.IsDBNull(iCiiu)) entidad.Ciiu = dr.GetString(iCiiu);

                    lIioTabla04DTO.Add(entidad);
                }
            }

            return lIioTabla04DTO;
        }

        //- alpha.HDT - 26/04/2017: Cambio para atender el requerimiento. 
        public List<IioTabla05DTO> GetDatosTabla05(string periodo, string empresasIn)
        {
            string extensionWhere = " 1 = 1";

            if (empresasIn != string.Empty)
            {
                extensionWhere = "EM.EMPRCODOSINERGMIN IN (" + empresasIn + ")";
            }

            string stQuery = string.Format(helper.SqlReporteTabla05
                                         , periodo
                                         , extensionWhere);
            
            DbCommand command = dbProvider.GetSqlStringCommand(stQuery);

            List<IioTabla05DTO> lIioTabla05DTO = new List<IioTabla05DTO>();

            IioTabla05DTO entidad = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entidad = new IioTabla05DTO();

                    int iIdSuministrador = dr.GetOrdinal(helper.IdSuministrador);
                    if (!dr.IsDBNull(iIdSuministrador)) entidad.IdSuministrador = dr.GetInt32(iIdSuministrador);

                    int iCodSuministradorSicli = dr.GetOrdinal(helper.CodSuministradorSicli);
                    if (!dr.IsDBNull(iCodSuministradorSicli)) entidad.CodSuministradorSicli = dr.GetString(iCodSuministradorSicli);

                    int iSuministradorSicli = dr.GetOrdinal(helper.SuministradorSicli);
                    if (!dr.IsDBNull(iSuministradorSicli)) entidad.SuministradorSicli = dr.GetString(iSuministradorSicli);

                    int iRuc = dr.GetOrdinal(helper.Ruc);
                    if (!dr.IsDBNull(iRuc)) entidad.Ruc = dr.GetString(iRuc);

                    int iUsuarioLibre = dr.GetOrdinal(helper.UsuarioLibre);
                    if (!dr.IsDBNull(iUsuarioLibre)) entidad.UsuarioLibre = dr.GetString(iUsuarioLibre);

                    int iCodUsuarioLibre = dr.GetOrdinal(helper.CodUsuarioLibre);
                    if (!dr.IsDBNull(iCodUsuarioLibre)) entidad.CodUsuarioLibre = dr.GetString(iCodUsuarioLibre);

                    int iBarraSuministro = dr.GetOrdinal(helper.BarraSuministro);
                    if (!dr.IsDBNull(iBarraSuministro)) entidad.BarraSuministro = dr.GetString(iBarraSuministro);

                    int iAreaDemanda = dr.GetOrdinal(helper.AreaDemanda);
                    if (!dr.IsDBNull(iAreaDemanda)) entidad.AreaDemanda = dr.GetInt32(iAreaDemanda);

                    int iPagaVad = dr.GetOrdinal(helper.PagaVad);
                    if (!dr.IsDBNull(iPagaVad)) entidad.PagaVad = dr.GetString(iPagaVad);

                    int iConsumoEnergiaHp = dr.GetOrdinal(helper.ConsumoEnergiaHp);
                    if (!dr.IsDBNull(iConsumoEnergiaHp)) entidad.ConsumoEnergiaHp = dr.GetDecimal(iConsumoEnergiaHp);

                    int iConsumoEnergiaHfp = dr.GetOrdinal(helper.ConsumoEnergiaHfp);
                    if (!dr.IsDBNull(iConsumoEnergiaHfp)) entidad.ConsumoEnergiaHfp = dr.GetDecimal(iConsumoEnergiaHfp);

                    int iMaximaDemandaHp = dr.GetOrdinal(helper.MaximaDemandaHp);
                    if (!dr.IsDBNull(iMaximaDemandaHp)) entidad.MaximaDemandaHp = dr.GetDecimal(iMaximaDemandaHp);

                    int iMaximaDemandaHfp = dr.GetOrdinal(helper.MaximaDemandaHfp);
                    if (!dr.IsDBNull(iMaximaDemandaHfp)) entidad.MaximaDemandaHfp = dr.GetDecimal(iMaximaDemandaHfp);

                    lIioTabla05DTO.Add(entidad);
                }
            }

            return lIioTabla05DTO;
        }

        //- alpha.HDT - 12/07/2017: Cambio para atender el requerimiento. 
        public List<IioLogImportacionDTO> GetIncidenciasImportacionSuministro(string periodo, string empresasIn)
        {
            string consulta = string.Format(helper.SqlGetIncidenciasImportacionSuministro
                                          , periodo
                                          , empresasIn);

            DbCommand command = dbProvider.GetSqlStringCommand(consulta);


            List<IioLogImportacionDTO>
                lIioLogImportacionDTO = new List<IioLogImportacionDTO>();

            IioLogImportacionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iUlogTablaCOES = dr.GetOrdinal(helper.Ulogtablacoes);
                    if (!dr.IsDBNull(iUlogTablaCOES)) entity.UlogTablaCOES = dr.GetString(iUlogTablaCOES);

                    int iUlogidregistrocoes = dr.GetOrdinal(helper.Ulogidregistrocoes);
                    if (!dr.IsDBNull(iUlogidregistrocoes))
                    {
                        entity.UlogIdRegistroCOES = dr.GetString(iUlogidregistrocoes);
                        entity.Suministro = entity.UlogIdRegistroCOES;
                    }

                    lIioLogImportacionDTO.Add(entity);

                }
            }

            return lIioLogImportacionDTO;
        }

        //- alpha.HDT - 22/07/2017: Cambio para atender el requerimiento. 
        public void Delete(int ulogCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
            dbProvider.AddInParameter(command, helper.Ulogcodi, DbType.Int32, ulogCodi);

            dbProvider.ExecuteNonQuery(command);
        }

        #region Demanda DPO - Iteracion 2
        public List<IioTabla04DTO> ListMedidorDemandaSicli(string cargas, string inicio, string fin, int tipo)
        {
            List<IioTabla04DTO> lIioTabla04DTO = new List<IioTabla04DTO>();
            string query = string.Format(helper.SqlListaMedidorDemandaSicli
                                         , cargas, inicio, fin, tipo);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    IioTabla04DTO entidad = new IioTabla04DTO();

                    int iIdSuministrador = dr.GetOrdinal(helper.IdSuministrador);
                    if (!dr.IsDBNull(iIdSuministrador)) entidad.IdSuministrador = dr.GetInt32(iIdSuministrador);

                    int iCodSuministradorSicli = dr.GetOrdinal(helper.CodSuministradorSicli);
                    if (!dr.IsDBNull(iCodSuministradorSicli)) entidad.CodSuministradorSicli = dr.GetString(iCodSuministradorSicli);

                    int iSuministradorSicli = dr.GetOrdinal(helper.SuministradorSicli);
                    if (!dr.IsDBNull(iSuministradorSicli)) entidad.SuministradorSicli = dr.GetString(iSuministradorSicli);

                    int iRuc = dr.GetOrdinal(helper.Ruc);
                    if (!dr.IsDBNull(iRuc)) entidad.Ruc = dr.GetString(iRuc);

                    int iUsuarioLibre = dr.GetOrdinal(helper.UsuarioLibre);
                    if (!dr.IsDBNull(iUsuarioLibre)) entidad.UsuarioLibre = dr.GetString(iUsuarioLibre);

                    int iCodSuministro = dr.GetOrdinal(helper.CodSuministro);
                    if (!dr.IsDBNull(iCodSuministro)) entidad.CodSuministro = dr.GetString(iCodSuministro);

                    int iNombrePtoMedicion = dr.GetOrdinal(helper.NombrePtoMedicion);
                    if (!dr.IsDBNull(iNombrePtoMedicion)) entidad.NombrePtoMedicion = dr.GetString(iNombrePtoMedicion);

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entidad.Ptomedicodi = dr.GetInt32(iPtomedicodi);

                    int iFechaMedicion = dr.GetOrdinal(helper.FechaMedicion);
                    if (!dr.IsDBNull(iFechaMedicion)) entidad.FechaMedicion = dr.GetDateTime(iFechaMedicion);

                    int iMeditotal = dr.GetOrdinal(helper.Meditotal);
                    if (!dr.IsDBNull(iMeditotal)) entidad.Meditotal = dr.GetDecimal(iMeditotal);

                    int iH1 = dr.GetOrdinal(helper.H1);
                    if (!dr.IsDBNull(iH1)) entidad.H1 = dr.GetDecimal(iH1);

                    int iH2 = dr.GetOrdinal(helper.H2);
                    if (!dr.IsDBNull(iH2)) entidad.H2 = dr.GetDecimal(iH2);

                    int iH3 = dr.GetOrdinal(helper.H3);
                    if (!dr.IsDBNull(iH3)) entidad.H3 = dr.GetDecimal(iH3);

                    int iH4 = dr.GetOrdinal(helper.H4);
                    if (!dr.IsDBNull(iH4)) entidad.H4 = dr.GetDecimal(iH4);

                    int iH5 = dr.GetOrdinal(helper.H5);
                    if (!dr.IsDBNull(iH5)) entidad.H5 = dr.GetDecimal(iH5);

                    int iH6 = dr.GetOrdinal(helper.H6);
                    if (!dr.IsDBNull(iH6)) entidad.H6 = dr.GetDecimal(iH6);

                    int iH7 = dr.GetOrdinal(helper.H7);
                    if (!dr.IsDBNull(iH7)) entidad.H7 = dr.GetDecimal(iH7);

                    int iH8 = dr.GetOrdinal(helper.H8);
                    if (!dr.IsDBNull(iH8)) entidad.H8 = dr.GetDecimal(iH8);

                    int iH9 = dr.GetOrdinal(helper.H9);
                    if (!dr.IsDBNull(iH9)) entidad.H9 = dr.GetDecimal(iH9);

                    int iH10 = dr.GetOrdinal(helper.H10);
                    if (!dr.IsDBNull(iH10)) entidad.H10 = dr.GetDecimal(iH10);

                    int iH11 = dr.GetOrdinal(helper.H11);
                    if (!dr.IsDBNull(iH11)) entidad.H11 = dr.GetDecimal(iH11);

                    int iH12 = dr.GetOrdinal(helper.H12);
                    if (!dr.IsDBNull(iH12)) entidad.H12 = dr.GetDecimal(iH12);

                    int iH13 = dr.GetOrdinal(helper.H13);
                    if (!dr.IsDBNull(iH13)) entidad.H13 = dr.GetDecimal(iH13);

                    int iH14 = dr.GetOrdinal(helper.H14);
                    if (!dr.IsDBNull(iH14)) entidad.H14 = dr.GetDecimal(iH14);

                    int iH15 = dr.GetOrdinal(helper.H15);
                    if (!dr.IsDBNull(iH15)) entidad.H15 = dr.GetDecimal(iH15);

                    int iH16 = dr.GetOrdinal(helper.H16);
                    if (!dr.IsDBNull(iH16)) entidad.H16 = dr.GetDecimal(iH16);

                    int iH17 = dr.GetOrdinal(helper.H17);
                    if (!dr.IsDBNull(iH17)) entidad.H17 = dr.GetDecimal(iH17);

                    int iH18 = dr.GetOrdinal(helper.H18);
                    if (!dr.IsDBNull(iH18)) entidad.H18 = dr.GetDecimal(iH18);

                    int iH19 = dr.GetOrdinal(helper.H19);
                    if (!dr.IsDBNull(iH19)) entidad.H19 = dr.GetDecimal(iH19);

                    int iH20 = dr.GetOrdinal(helper.H20);
                    if (!dr.IsDBNull(iH20)) entidad.H20 = dr.GetDecimal(iH20);

                    int iH21 = dr.GetOrdinal(helper.H21);
                    if (!dr.IsDBNull(iH21)) entidad.H21 = dr.GetDecimal(iH21);

                    int iH22 = dr.GetOrdinal(helper.H22);
                    if (!dr.IsDBNull(iH22)) entidad.H22 = dr.GetDecimal(iH22);

                    int iH23 = dr.GetOrdinal(helper.H23);
                    if (!dr.IsDBNull(iH23)) entidad.H23 = dr.GetDecimal(iH23);

                    int iH24 = dr.GetOrdinal(helper.H24);
                    if (!dr.IsDBNull(iH24)) entidad.H24 = dr.GetDecimal(iH24);

                    int iH25 = dr.GetOrdinal(helper.H25);
                    if (!dr.IsDBNull(iH25)) entidad.H25 = dr.GetDecimal(iH25);

                    int iH26 = dr.GetOrdinal(helper.H26);
                    if (!dr.IsDBNull(iH26)) entidad.H26 = dr.GetDecimal(iH26);

                    int iH27 = dr.GetOrdinal(helper.H27);
                    if (!dr.IsDBNull(iH27)) entidad.H27 = dr.GetDecimal(iH27);

                    int iH28 = dr.GetOrdinal(helper.H28);
                    if (!dr.IsDBNull(iH28)) entidad.H28 = dr.GetDecimal(iH28);

                    int iH29 = dr.GetOrdinal(helper.H29);
                    if (!dr.IsDBNull(iH29)) entidad.H29 = dr.GetDecimal(iH29);

                    int iH30 = dr.GetOrdinal(helper.H30);
                    if (!dr.IsDBNull(iH30)) entidad.H30 = dr.GetDecimal(iH30);

                    int iH31 = dr.GetOrdinal(helper.H31);
                    if (!dr.IsDBNull(iH31)) entidad.H31 = dr.GetDecimal(iH31);

                    int iH32 = dr.GetOrdinal(helper.H32);
                    if (!dr.IsDBNull(iH32)) entidad.H32 = dr.GetDecimal(iH32);

                    int iH33 = dr.GetOrdinal(helper.H33);
                    if (!dr.IsDBNull(iH33)) entidad.H33 = dr.GetDecimal(iH33);

                    int iH34 = dr.GetOrdinal(helper.H34);
                    if (!dr.IsDBNull(iH34)) entidad.H34 = dr.GetDecimal(iH34);

                    int iH35 = dr.GetOrdinal(helper.H35);
                    if (!dr.IsDBNull(iH35)) entidad.H35 = dr.GetDecimal(iH35);

                    int iH36 = dr.GetOrdinal(helper.H36);
                    if (!dr.IsDBNull(iH36)) entidad.H36 = dr.GetDecimal(iH36);

                    int iH37 = dr.GetOrdinal(helper.H37);
                    if (!dr.IsDBNull(iH37)) entidad.H37 = dr.GetDecimal(iH37);

                    int iH38 = dr.GetOrdinal(helper.H38);
                    if (!dr.IsDBNull(iH38)) entidad.H38 = dr.GetDecimal(iH38);

                    int iH39 = dr.GetOrdinal(helper.H39);
                    if (!dr.IsDBNull(iH39)) entidad.H39 = dr.GetDecimal(iH39);

                    int iH40 = dr.GetOrdinal(helper.H40);
                    if (!dr.IsDBNull(iH40)) entidad.H40 = dr.GetDecimal(iH40);

                    int iH41 = dr.GetOrdinal(helper.H41);
                    if (!dr.IsDBNull(iH41)) entidad.H41 = dr.GetDecimal(iH41);

                    int iH42 = dr.GetOrdinal(helper.H42);
                    if (!dr.IsDBNull(iH42)) entidad.H42 = dr.GetDecimal(iH42);

                    int iH43 = dr.GetOrdinal(helper.H43);
                    if (!dr.IsDBNull(iH43)) entidad.H43 = dr.GetDecimal(iH43);

                    int iH44 = dr.GetOrdinal(helper.H44);
                    if (!dr.IsDBNull(iH44)) entidad.H44 = dr.GetDecimal(iH44);

                    int iH45 = dr.GetOrdinal(helper.H45);
                    if (!dr.IsDBNull(iH45)) entidad.H45 = dr.GetDecimal(iH45);

                    int iH46 = dr.GetOrdinal(helper.H46);
                    if (!dr.IsDBNull(iH46)) entidad.H46 = dr.GetDecimal(iH46);

                    int iH47 = dr.GetOrdinal(helper.H47);
                    if (!dr.IsDBNull(iH47)) entidad.H47 = dr.GetDecimal(iH47);

                    int iH48 = dr.GetOrdinal(helper.H48);
                    if (!dr.IsDBNull(iH48)) entidad.H48 = dr.GetDecimal(iH48);

                    int iH49 = dr.GetOrdinal(helper.H49);
                    if (!dr.IsDBNull(iH49)) entidad.H49 = dr.GetDecimal(iH49);

                    int iH50 = dr.GetOrdinal(helper.H50);
                    if (!dr.IsDBNull(iH50)) entidad.H50 = dr.GetDecimal(iH50);

                    int iH51 = dr.GetOrdinal(helper.H51);
                    if (!dr.IsDBNull(iH51)) entidad.H51 = dr.GetDecimal(iH51);

                    int iH52 = dr.GetOrdinal(helper.H52);
                    if (!dr.IsDBNull(iH52)) entidad.H52 = dr.GetDecimal(iH52);

                    int iH53 = dr.GetOrdinal(helper.H53);
                    if (!dr.IsDBNull(iH53)) entidad.H53 = dr.GetDecimal(iH53);

                    int iH54 = dr.GetOrdinal(helper.H54);
                    if (!dr.IsDBNull(iH54)) entidad.H54 = dr.GetDecimal(iH54);

                    int iH55 = dr.GetOrdinal(helper.H55);
                    if (!dr.IsDBNull(iH55)) entidad.H55 = dr.GetDecimal(iH55);

                    int iH56 = dr.GetOrdinal(helper.H56);
                    if (!dr.IsDBNull(iH56)) entidad.H56 = dr.GetDecimal(iH56);

                    int iH57 = dr.GetOrdinal(helper.H57);
                    if (!dr.IsDBNull(iH57)) entidad.H57 = dr.GetDecimal(iH57);

                    int iH58 = dr.GetOrdinal(helper.H58);
                    if (!dr.IsDBNull(iH58)) entidad.H58 = dr.GetDecimal(iH58);

                    int iH59 = dr.GetOrdinal(helper.H59);
                    if (!dr.IsDBNull(iH59)) entidad.H59 = dr.GetDecimal(iH59);

                    int iH60 = dr.GetOrdinal(helper.H60);
                    if (!dr.IsDBNull(iH60)) entidad.H60 = dr.GetDecimal(iH60);

                    int iH61 = dr.GetOrdinal(helper.H61);
                    if (!dr.IsDBNull(iH61)) entidad.H61 = dr.GetDecimal(iH61);

                    int iH62 = dr.GetOrdinal(helper.H62);
                    if (!dr.IsDBNull(iH62)) entidad.H62 = dr.GetDecimal(iH62);

                    int iH63 = dr.GetOrdinal(helper.H63);
                    if (!dr.IsDBNull(iH63)) entidad.H63 = dr.GetDecimal(iH63);

                    int iH64 = dr.GetOrdinal(helper.H64);
                    if (!dr.IsDBNull(iH64)) entidad.H64 = dr.GetDecimal(iH64);

                    int iH65 = dr.GetOrdinal(helper.H65);
                    if (!dr.IsDBNull(iH65)) entidad.H65 = dr.GetDecimal(iH65);

                    int iH66 = dr.GetOrdinal(helper.H66);
                    if (!dr.IsDBNull(iH66)) entidad.H66 = dr.GetDecimal(iH66);

                    int iH67 = dr.GetOrdinal(helper.H67);
                    if (!dr.IsDBNull(iH67)) entidad.H67 = dr.GetDecimal(iH67);

                    int iH68 = dr.GetOrdinal(helper.H68);
                    if (!dr.IsDBNull(iH68)) entidad.H68 = dr.GetDecimal(iH68);

                    int iH69 = dr.GetOrdinal(helper.H69);
                    if (!dr.IsDBNull(iH69)) entidad.H69 = dr.GetDecimal(iH69);

                    int iH70 = dr.GetOrdinal(helper.H70);
                    if (!dr.IsDBNull(iH70)) entidad.H70 = dr.GetDecimal(iH70);

                    int iH71 = dr.GetOrdinal(helper.H71);
                    if (!dr.IsDBNull(iH71)) entidad.H71 = dr.GetDecimal(iH71);

                    int iH72 = dr.GetOrdinal(helper.H72);
                    if (!dr.IsDBNull(iH72)) entidad.H72 = dr.GetDecimal(iH72);

                    int iH73 = dr.GetOrdinal(helper.H73);
                    if (!dr.IsDBNull(iH73)) entidad.H73 = dr.GetDecimal(iH73);

                    int iH74 = dr.GetOrdinal(helper.H74);
                    if (!dr.IsDBNull(iH74)) entidad.H74 = dr.GetDecimal(iH74);

                    int iH75 = dr.GetOrdinal(helper.H75);
                    if (!dr.IsDBNull(iH75)) entidad.H75 = dr.GetDecimal(iH75);

                    int iH76 = dr.GetOrdinal(helper.H76);
                    if (!dr.IsDBNull(iH76)) entidad.H76 = dr.GetDecimal(iH76);

                    int iH77 = dr.GetOrdinal(helper.H77);
                    if (!dr.IsDBNull(iH77)) entidad.H77 = dr.GetDecimal(iH77);

                    int iH78 = dr.GetOrdinal(helper.H78);
                    if (!dr.IsDBNull(iH78)) entidad.H78 = dr.GetDecimal(iH78);

                    int iH79 = dr.GetOrdinal(helper.H79);
                    if (!dr.IsDBNull(iH79)) entidad.H79 = dr.GetDecimal(iH79);

                    int iH80 = dr.GetOrdinal(helper.H80);
                    if (!dr.IsDBNull(iH80)) entidad.H80 = dr.GetDecimal(iH80);

                    int iH81 = dr.GetOrdinal(helper.H81);
                    if (!dr.IsDBNull(iH81)) entidad.H81 = dr.GetDecimal(iH81);

                    int iH82 = dr.GetOrdinal(helper.H82);
                    if (!dr.IsDBNull(iH82)) entidad.H82 = dr.GetDecimal(iH82);

                    int iH83 = dr.GetOrdinal(helper.H83);
                    if (!dr.IsDBNull(iH83)) entidad.H83 = dr.GetDecimal(iH83);

                    int iH84 = dr.GetOrdinal(helper.H84);
                    if (!dr.IsDBNull(iH84)) entidad.H84 = dr.GetDecimal(iH84);

                    int iH85 = dr.GetOrdinal(helper.H85);
                    if (!dr.IsDBNull(iH85)) entidad.H85 = dr.GetDecimal(iH85);

                    int iH86 = dr.GetOrdinal(helper.H86);
                    if (!dr.IsDBNull(iH86)) entidad.H86 = dr.GetDecimal(iH86);

                    int iH87 = dr.GetOrdinal(helper.H87);
                    if (!dr.IsDBNull(iH87)) entidad.H87 = dr.GetDecimal(iH87);

                    int iH88 = dr.GetOrdinal(helper.H88);
                    if (!dr.IsDBNull(iH88)) entidad.H88 = dr.GetDecimal(iH88);

                    int iH89 = dr.GetOrdinal(helper.H89);
                    if (!dr.IsDBNull(iH89)) entidad.H89 = dr.GetDecimal(iH89);

                    int iH90 = dr.GetOrdinal(helper.H90);
                    if (!dr.IsDBNull(iH90)) entidad.H90 = dr.GetDecimal(iH90);

                    int iH91 = dr.GetOrdinal(helper.H91);
                    if (!dr.IsDBNull(iH91)) entidad.H91 = dr.GetDecimal(iH91);

                    int iH92 = dr.GetOrdinal(helper.H92);
                    if (!dr.IsDBNull(iH92)) entidad.H92 = dr.GetDecimal(iH92);

                    int iH93 = dr.GetOrdinal(helper.H93);
                    if (!dr.IsDBNull(iH93)) entidad.H93 = dr.GetDecimal(iH93);

                    int iH94 = dr.GetOrdinal(helper.H94);
                    if (!dr.IsDBNull(iH94)) entidad.H94 = dr.GetDecimal(iH94);

                    int iH95 = dr.GetOrdinal(helper.H95);
                    if (!dr.IsDBNull(iH95)) entidad.H95 = dr.GetDecimal(iH95);

                    int iH96 = dr.GetOrdinal(helper.H96);
                    if (!dr.IsDBNull(iH96)) entidad.H96 = dr.GetDecimal(iH96);

                    int iCiiu = dr.GetOrdinal(helper.CIIU);
                    if (!dr.IsDBNull(iCiiu)) entidad.Ciiu = dr.GetString(iCiiu);

                    lIioTabla04DTO.Add(entidad);
                }
            }

            return lIioTabla04DTO;
        }

        public List<IioTabla04DTO> ListGroupByMonthYear(string anio, string mes, string cargas, string tipo)
        {
            List<IioTabla04DTO> lIioTabla04DTO = new List<IioTabla04DTO>();
            string query = string.Format(helper.SqlListGroupByMonthYear
                                         , anio, mes, cargas, tipo);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    IioTabla04DTO entidad = new IioTabla04DTO();

                    int iFechaMedicion = dr.GetOrdinal(helper.FechaMedicion);
                    if (!dr.IsDBNull(iFechaMedicion)) entidad.FechaMedicion = dr.GetDateTime(iFechaMedicion);

                    int iH1 = dr.GetOrdinal(helper.H1);
                    if (!dr.IsDBNull(iH1)) entidad.H1 = dr.GetDecimal(iH1);

                    int iH2 = dr.GetOrdinal(helper.H2);
                    if (!dr.IsDBNull(iH2)) entidad.H2 = dr.GetDecimal(iH2);

                    int iH3 = dr.GetOrdinal(helper.H3);
                    if (!dr.IsDBNull(iH3)) entidad.H3 = dr.GetDecimal(iH3);

                    int iH4 = dr.GetOrdinal(helper.H4);
                    if (!dr.IsDBNull(iH4)) entidad.H4 = dr.GetDecimal(iH4);

                    int iH5 = dr.GetOrdinal(helper.H5);
                    if (!dr.IsDBNull(iH5)) entidad.H5 = dr.GetDecimal(iH5);

                    int iH6 = dr.GetOrdinal(helper.H6);
                    if (!dr.IsDBNull(iH6)) entidad.H6 = dr.GetDecimal(iH6);

                    int iH7 = dr.GetOrdinal(helper.H7);
                    if (!dr.IsDBNull(iH7)) entidad.H7 = dr.GetDecimal(iH7);

                    int iH8 = dr.GetOrdinal(helper.H8);
                    if (!dr.IsDBNull(iH8)) entidad.H8 = dr.GetDecimal(iH8);

                    int iH9 = dr.GetOrdinal(helper.H9);
                    if (!dr.IsDBNull(iH9)) entidad.H9 = dr.GetDecimal(iH9);

                    int iH10 = dr.GetOrdinal(helper.H10);
                    if (!dr.IsDBNull(iH10)) entidad.H10 = dr.GetDecimal(iH10);

                    int iH11 = dr.GetOrdinal(helper.H11);
                    if (!dr.IsDBNull(iH11)) entidad.H11 = dr.GetDecimal(iH11);

                    int iH12 = dr.GetOrdinal(helper.H12);
                    if (!dr.IsDBNull(iH12)) entidad.H12 = dr.GetDecimal(iH12);

                    int iH13 = dr.GetOrdinal(helper.H13);
                    if (!dr.IsDBNull(iH13)) entidad.H13 = dr.GetDecimal(iH13);

                    int iH14 = dr.GetOrdinal(helper.H14);
                    if (!dr.IsDBNull(iH14)) entidad.H14 = dr.GetDecimal(iH14);

                    int iH15 = dr.GetOrdinal(helper.H15);
                    if (!dr.IsDBNull(iH15)) entidad.H15 = dr.GetDecimal(iH15);

                    int iH16 = dr.GetOrdinal(helper.H16);
                    if (!dr.IsDBNull(iH16)) entidad.H16 = dr.GetDecimal(iH16);

                    int iH17 = dr.GetOrdinal(helper.H17);
                    if (!dr.IsDBNull(iH17)) entidad.H17 = dr.GetDecimal(iH17);

                    int iH18 = dr.GetOrdinal(helper.H18);
                    if (!dr.IsDBNull(iH18)) entidad.H18 = dr.GetDecimal(iH18);

                    int iH19 = dr.GetOrdinal(helper.H19);
                    if (!dr.IsDBNull(iH19)) entidad.H19 = dr.GetDecimal(iH19);

                    int iH20 = dr.GetOrdinal(helper.H20);
                    if (!dr.IsDBNull(iH20)) entidad.H20 = dr.GetDecimal(iH20);

                    int iH21 = dr.GetOrdinal(helper.H21);
                    if (!dr.IsDBNull(iH21)) entidad.H21 = dr.GetDecimal(iH21);

                    int iH22 = dr.GetOrdinal(helper.H22);
                    if (!dr.IsDBNull(iH22)) entidad.H22 = dr.GetDecimal(iH22);

                    int iH23 = dr.GetOrdinal(helper.H23);
                    if (!dr.IsDBNull(iH23)) entidad.H23 = dr.GetDecimal(iH23);

                    int iH24 = dr.GetOrdinal(helper.H24);
                    if (!dr.IsDBNull(iH24)) entidad.H24 = dr.GetDecimal(iH24);

                    int iH25 = dr.GetOrdinal(helper.H25);
                    if (!dr.IsDBNull(iH25)) entidad.H25 = dr.GetDecimal(iH25);

                    int iH26 = dr.GetOrdinal(helper.H26);
                    if (!dr.IsDBNull(iH26)) entidad.H26 = dr.GetDecimal(iH26);

                    int iH27 = dr.GetOrdinal(helper.H27);
                    if (!dr.IsDBNull(iH27)) entidad.H27 = dr.GetDecimal(iH27);

                    int iH28 = dr.GetOrdinal(helper.H28);
                    if (!dr.IsDBNull(iH28)) entidad.H28 = dr.GetDecimal(iH28);

                    int iH29 = dr.GetOrdinal(helper.H29);
                    if (!dr.IsDBNull(iH29)) entidad.H29 = dr.GetDecimal(iH29);

                    int iH30 = dr.GetOrdinal(helper.H30);
                    if (!dr.IsDBNull(iH30)) entidad.H30 = dr.GetDecimal(iH30);

                    int iH31 = dr.GetOrdinal(helper.H31);
                    if (!dr.IsDBNull(iH31)) entidad.H31 = dr.GetDecimal(iH31);

                    int iH32 = dr.GetOrdinal(helper.H32);
                    if (!dr.IsDBNull(iH32)) entidad.H32 = dr.GetDecimal(iH32);

                    int iH33 = dr.GetOrdinal(helper.H33);
                    if (!dr.IsDBNull(iH33)) entidad.H33 = dr.GetDecimal(iH33);

                    int iH34 = dr.GetOrdinal(helper.H34);
                    if (!dr.IsDBNull(iH34)) entidad.H34 = dr.GetDecimal(iH34);

                    int iH35 = dr.GetOrdinal(helper.H35);
                    if (!dr.IsDBNull(iH35)) entidad.H35 = dr.GetDecimal(iH35);

                    int iH36 = dr.GetOrdinal(helper.H36);
                    if (!dr.IsDBNull(iH36)) entidad.H36 = dr.GetDecimal(iH36);

                    int iH37 = dr.GetOrdinal(helper.H37);
                    if (!dr.IsDBNull(iH37)) entidad.H37 = dr.GetDecimal(iH37);

                    int iH38 = dr.GetOrdinal(helper.H38);
                    if (!dr.IsDBNull(iH38)) entidad.H38 = dr.GetDecimal(iH38);

                    int iH39 = dr.GetOrdinal(helper.H39);
                    if (!dr.IsDBNull(iH39)) entidad.H39 = dr.GetDecimal(iH39);

                    int iH40 = dr.GetOrdinal(helper.H40);
                    if (!dr.IsDBNull(iH40)) entidad.H40 = dr.GetDecimal(iH40);

                    int iH41 = dr.GetOrdinal(helper.H41);
                    if (!dr.IsDBNull(iH41)) entidad.H41 = dr.GetDecimal(iH41);

                    int iH42 = dr.GetOrdinal(helper.H42);
                    if (!dr.IsDBNull(iH42)) entidad.H42 = dr.GetDecimal(iH42);

                    int iH43 = dr.GetOrdinal(helper.H43);
                    if (!dr.IsDBNull(iH43)) entidad.H43 = dr.GetDecimal(iH43);

                    int iH44 = dr.GetOrdinal(helper.H44);
                    if (!dr.IsDBNull(iH44)) entidad.H44 = dr.GetDecimal(iH44);

                    int iH45 = dr.GetOrdinal(helper.H45);
                    if (!dr.IsDBNull(iH45)) entidad.H45 = dr.GetDecimal(iH45);

                    int iH46 = dr.GetOrdinal(helper.H46);
                    if (!dr.IsDBNull(iH46)) entidad.H46 = dr.GetDecimal(iH46);

                    int iH47 = dr.GetOrdinal(helper.H47);
                    if (!dr.IsDBNull(iH47)) entidad.H47 = dr.GetDecimal(iH47);

                    int iH48 = dr.GetOrdinal(helper.H48);
                    if (!dr.IsDBNull(iH48)) entidad.H48 = dr.GetDecimal(iH48);

                    int iH49 = dr.GetOrdinal(helper.H49);
                    if (!dr.IsDBNull(iH49)) entidad.H49 = dr.GetDecimal(iH49);

                    int iH50 = dr.GetOrdinal(helper.H50);
                    if (!dr.IsDBNull(iH50)) entidad.H50 = dr.GetDecimal(iH50);

                    int iH51 = dr.GetOrdinal(helper.H51);
                    if (!dr.IsDBNull(iH51)) entidad.H51 = dr.GetDecimal(iH51);

                    int iH52 = dr.GetOrdinal(helper.H52);
                    if (!dr.IsDBNull(iH52)) entidad.H52 = dr.GetDecimal(iH52);

                    int iH53 = dr.GetOrdinal(helper.H53);
                    if (!dr.IsDBNull(iH53)) entidad.H53 = dr.GetDecimal(iH53);

                    int iH54 = dr.GetOrdinal(helper.H54);
                    if (!dr.IsDBNull(iH54)) entidad.H54 = dr.GetDecimal(iH54);

                    int iH55 = dr.GetOrdinal(helper.H55);
                    if (!dr.IsDBNull(iH55)) entidad.H55 = dr.GetDecimal(iH55);

                    int iH56 = dr.GetOrdinal(helper.H56);
                    if (!dr.IsDBNull(iH56)) entidad.H56 = dr.GetDecimal(iH56);

                    int iH57 = dr.GetOrdinal(helper.H57);
                    if (!dr.IsDBNull(iH57)) entidad.H57 = dr.GetDecimal(iH57);

                    int iH58 = dr.GetOrdinal(helper.H58);
                    if (!dr.IsDBNull(iH58)) entidad.H58 = dr.GetDecimal(iH58);

                    int iH59 = dr.GetOrdinal(helper.H59);
                    if (!dr.IsDBNull(iH59)) entidad.H59 = dr.GetDecimal(iH59);

                    int iH60 = dr.GetOrdinal(helper.H60);
                    if (!dr.IsDBNull(iH60)) entidad.H60 = dr.GetDecimal(iH60);

                    int iH61 = dr.GetOrdinal(helper.H61);
                    if (!dr.IsDBNull(iH61)) entidad.H61 = dr.GetDecimal(iH61);

                    int iH62 = dr.GetOrdinal(helper.H62);
                    if (!dr.IsDBNull(iH62)) entidad.H62 = dr.GetDecimal(iH62);

                    int iH63 = dr.GetOrdinal(helper.H63);
                    if (!dr.IsDBNull(iH63)) entidad.H63 = dr.GetDecimal(iH63);

                    int iH64 = dr.GetOrdinal(helper.H64);
                    if (!dr.IsDBNull(iH64)) entidad.H64 = dr.GetDecimal(iH64);

                    int iH65 = dr.GetOrdinal(helper.H65);
                    if (!dr.IsDBNull(iH65)) entidad.H65 = dr.GetDecimal(iH65);

                    int iH66 = dr.GetOrdinal(helper.H66);
                    if (!dr.IsDBNull(iH66)) entidad.H66 = dr.GetDecimal(iH66);

                    int iH67 = dr.GetOrdinal(helper.H67);
                    if (!dr.IsDBNull(iH67)) entidad.H67 = dr.GetDecimal(iH67);

                    int iH68 = dr.GetOrdinal(helper.H68);
                    if (!dr.IsDBNull(iH68)) entidad.H68 = dr.GetDecimal(iH68);

                    int iH69 = dr.GetOrdinal(helper.H69);
                    if (!dr.IsDBNull(iH69)) entidad.H69 = dr.GetDecimal(iH69);

                    int iH70 = dr.GetOrdinal(helper.H70);
                    if (!dr.IsDBNull(iH70)) entidad.H70 = dr.GetDecimal(iH70);

                    int iH71 = dr.GetOrdinal(helper.H71);
                    if (!dr.IsDBNull(iH71)) entidad.H71 = dr.GetDecimal(iH71);

                    int iH72 = dr.GetOrdinal(helper.H72);
                    if (!dr.IsDBNull(iH72)) entidad.H72 = dr.GetDecimal(iH72);

                    int iH73 = dr.GetOrdinal(helper.H73);
                    if (!dr.IsDBNull(iH73)) entidad.H73 = dr.GetDecimal(iH73);

                    int iH74 = dr.GetOrdinal(helper.H74);
                    if (!dr.IsDBNull(iH74)) entidad.H74 = dr.GetDecimal(iH74);

                    int iH75 = dr.GetOrdinal(helper.H75);
                    if (!dr.IsDBNull(iH75)) entidad.H75 = dr.GetDecimal(iH75);

                    int iH76 = dr.GetOrdinal(helper.H76);
                    if (!dr.IsDBNull(iH76)) entidad.H76 = dr.GetDecimal(iH76);

                    int iH77 = dr.GetOrdinal(helper.H77);
                    if (!dr.IsDBNull(iH77)) entidad.H77 = dr.GetDecimal(iH77);

                    int iH78 = dr.GetOrdinal(helper.H78);
                    if (!dr.IsDBNull(iH78)) entidad.H78 = dr.GetDecimal(iH78);

                    int iH79 = dr.GetOrdinal(helper.H79);
                    if (!dr.IsDBNull(iH79)) entidad.H79 = dr.GetDecimal(iH79);

                    int iH80 = dr.GetOrdinal(helper.H80);
                    if (!dr.IsDBNull(iH80)) entidad.H80 = dr.GetDecimal(iH80);

                    int iH81 = dr.GetOrdinal(helper.H81);
                    if (!dr.IsDBNull(iH81)) entidad.H81 = dr.GetDecimal(iH81);

                    int iH82 = dr.GetOrdinal(helper.H82);
                    if (!dr.IsDBNull(iH82)) entidad.H82 = dr.GetDecimal(iH82);

                    int iH83 = dr.GetOrdinal(helper.H83);
                    if (!dr.IsDBNull(iH83)) entidad.H83 = dr.GetDecimal(iH83);

                    int iH84 = dr.GetOrdinal(helper.H84);
                    if (!dr.IsDBNull(iH84)) entidad.H84 = dr.GetDecimal(iH84);

                    int iH85 = dr.GetOrdinal(helper.H85);
                    if (!dr.IsDBNull(iH85)) entidad.H85 = dr.GetDecimal(iH85);

                    int iH86 = dr.GetOrdinal(helper.H86);
                    if (!dr.IsDBNull(iH86)) entidad.H86 = dr.GetDecimal(iH86);

                    int iH87 = dr.GetOrdinal(helper.H87);
                    if (!dr.IsDBNull(iH87)) entidad.H87 = dr.GetDecimal(iH87);

                    int iH88 = dr.GetOrdinal(helper.H88);
                    if (!dr.IsDBNull(iH88)) entidad.H88 = dr.GetDecimal(iH88);

                    int iH89 = dr.GetOrdinal(helper.H89);
                    if (!dr.IsDBNull(iH89)) entidad.H89 = dr.GetDecimal(iH89);

                    int iH90 = dr.GetOrdinal(helper.H90);
                    if (!dr.IsDBNull(iH90)) entidad.H90 = dr.GetDecimal(iH90);

                    int iH91 = dr.GetOrdinal(helper.H91);
                    if (!dr.IsDBNull(iH91)) entidad.H91 = dr.GetDecimal(iH91);

                    int iH92 = dr.GetOrdinal(helper.H92);
                    if (!dr.IsDBNull(iH92)) entidad.H92 = dr.GetDecimal(iH92);

                    int iH93 = dr.GetOrdinal(helper.H93);
                    if (!dr.IsDBNull(iH93)) entidad.H93 = dr.GetDecimal(iH93);

                    int iH94 = dr.GetOrdinal(helper.H94);
                    if (!dr.IsDBNull(iH94)) entidad.H94 = dr.GetDecimal(iH94);

                    int iH95 = dr.GetOrdinal(helper.H95);
                    if (!dr.IsDBNull(iH95)) entidad.H95 = dr.GetDecimal(iH95);

                    int iH96 = dr.GetOrdinal(helper.H96);
                    if (!dr.IsDBNull(iH96)) entidad.H96 = dr.GetDecimal(iH96);

                    lIioTabla04DTO.Add(entidad);
                }
            }

            return lIioTabla04DTO;
        }

        public List<IioTabla04DTO> ListDatosSICLI(int anio, string mes, string cargas, string tipo)
        {
            List<IioTabla04DTO> lIioTabla04DTO = new List<IioTabla04DTO>();
            string query = string.Format(helper.SqlListDatosSICLI
                                         , anio, mes, cargas, tipo);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    IioTabla04DTO entidad = new IioTabla04DTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entidad.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iFechaMedicion = dr.GetOrdinal(helper.FechaMedicion);
                    if (!dr.IsDBNull(iFechaMedicion)) entidad.FechaMedicion = dr.GetDateTime(iFechaMedicion);

                    int iH1 = dr.GetOrdinal(helper.H1);
                    if (!dr.IsDBNull(iH1)) entidad.H1 = dr.GetDecimal(iH1);

                    int iH2 = dr.GetOrdinal(helper.H2);
                    if (!dr.IsDBNull(iH2)) entidad.H2 = dr.GetDecimal(iH2);

                    int iH3 = dr.GetOrdinal(helper.H3);
                    if (!dr.IsDBNull(iH3)) entidad.H3 = dr.GetDecimal(iH3);

                    int iH4 = dr.GetOrdinal(helper.H4);
                    if (!dr.IsDBNull(iH4)) entidad.H4 = dr.GetDecimal(iH4);

                    int iH5 = dr.GetOrdinal(helper.H5);
                    if (!dr.IsDBNull(iH5)) entidad.H5 = dr.GetDecimal(iH5);

                    int iH6 = dr.GetOrdinal(helper.H6);
                    if (!dr.IsDBNull(iH6)) entidad.H6 = dr.GetDecimal(iH6);

                    int iH7 = dr.GetOrdinal(helper.H7);
                    if (!dr.IsDBNull(iH7)) entidad.H7 = dr.GetDecimal(iH7);

                    int iH8 = dr.GetOrdinal(helper.H8);
                    if (!dr.IsDBNull(iH8)) entidad.H8 = dr.GetDecimal(iH8);

                    int iH9 = dr.GetOrdinal(helper.H9);
                    if (!dr.IsDBNull(iH9)) entidad.H9 = dr.GetDecimal(iH9);

                    int iH10 = dr.GetOrdinal(helper.H10);
                    if (!dr.IsDBNull(iH10)) entidad.H10 = dr.GetDecimal(iH10);

                    int iH11 = dr.GetOrdinal(helper.H11);
                    if (!dr.IsDBNull(iH11)) entidad.H11 = dr.GetDecimal(iH11);

                    int iH12 = dr.GetOrdinal(helper.H12);
                    if (!dr.IsDBNull(iH12)) entidad.H12 = dr.GetDecimal(iH12);

                    int iH13 = dr.GetOrdinal(helper.H13);
                    if (!dr.IsDBNull(iH13)) entidad.H13 = dr.GetDecimal(iH13);

                    int iH14 = dr.GetOrdinal(helper.H14);
                    if (!dr.IsDBNull(iH14)) entidad.H14 = dr.GetDecimal(iH14);

                    int iH15 = dr.GetOrdinal(helper.H15);
                    if (!dr.IsDBNull(iH15)) entidad.H15 = dr.GetDecimal(iH15);

                    int iH16 = dr.GetOrdinal(helper.H16);
                    if (!dr.IsDBNull(iH16)) entidad.H16 = dr.GetDecimal(iH16);

                    int iH17 = dr.GetOrdinal(helper.H17);
                    if (!dr.IsDBNull(iH17)) entidad.H17 = dr.GetDecimal(iH17);

                    int iH18 = dr.GetOrdinal(helper.H18);
                    if (!dr.IsDBNull(iH18)) entidad.H18 = dr.GetDecimal(iH18);

                    int iH19 = dr.GetOrdinal(helper.H19);
                    if (!dr.IsDBNull(iH19)) entidad.H19 = dr.GetDecimal(iH19);

                    int iH20 = dr.GetOrdinal(helper.H20);
                    if (!dr.IsDBNull(iH20)) entidad.H20 = dr.GetDecimal(iH20);

                    int iH21 = dr.GetOrdinal(helper.H21);
                    if (!dr.IsDBNull(iH21)) entidad.H21 = dr.GetDecimal(iH21);

                    int iH22 = dr.GetOrdinal(helper.H22);
                    if (!dr.IsDBNull(iH22)) entidad.H22 = dr.GetDecimal(iH22);

                    int iH23 = dr.GetOrdinal(helper.H23);
                    if (!dr.IsDBNull(iH23)) entidad.H23 = dr.GetDecimal(iH23);

                    int iH24 = dr.GetOrdinal(helper.H24);
                    if (!dr.IsDBNull(iH24)) entidad.H24 = dr.GetDecimal(iH24);

                    int iH25 = dr.GetOrdinal(helper.H25);
                    if (!dr.IsDBNull(iH25)) entidad.H25 = dr.GetDecimal(iH25);

                    int iH26 = dr.GetOrdinal(helper.H26);
                    if (!dr.IsDBNull(iH26)) entidad.H26 = dr.GetDecimal(iH26);

                    int iH27 = dr.GetOrdinal(helper.H27);
                    if (!dr.IsDBNull(iH27)) entidad.H27 = dr.GetDecimal(iH27);

                    int iH28 = dr.GetOrdinal(helper.H28);
                    if (!dr.IsDBNull(iH28)) entidad.H28 = dr.GetDecimal(iH28);

                    int iH29 = dr.GetOrdinal(helper.H29);
                    if (!dr.IsDBNull(iH29)) entidad.H29 = dr.GetDecimal(iH29);

                    int iH30 = dr.GetOrdinal(helper.H30);
                    if (!dr.IsDBNull(iH30)) entidad.H30 = dr.GetDecimal(iH30);

                    int iH31 = dr.GetOrdinal(helper.H31);
                    if (!dr.IsDBNull(iH31)) entidad.H31 = dr.GetDecimal(iH31);

                    int iH32 = dr.GetOrdinal(helper.H32);
                    if (!dr.IsDBNull(iH32)) entidad.H32 = dr.GetDecimal(iH32);

                    int iH33 = dr.GetOrdinal(helper.H33);
                    if (!dr.IsDBNull(iH33)) entidad.H33 = dr.GetDecimal(iH33);

                    int iH34 = dr.GetOrdinal(helper.H34);
                    if (!dr.IsDBNull(iH34)) entidad.H34 = dr.GetDecimal(iH34);

                    int iH35 = dr.GetOrdinal(helper.H35);
                    if (!dr.IsDBNull(iH35)) entidad.H35 = dr.GetDecimal(iH35);

                    int iH36 = dr.GetOrdinal(helper.H36);
                    if (!dr.IsDBNull(iH36)) entidad.H36 = dr.GetDecimal(iH36);

                    int iH37 = dr.GetOrdinal(helper.H37);
                    if (!dr.IsDBNull(iH37)) entidad.H37 = dr.GetDecimal(iH37);

                    int iH38 = dr.GetOrdinal(helper.H38);
                    if (!dr.IsDBNull(iH38)) entidad.H38 = dr.GetDecimal(iH38);

                    int iH39 = dr.GetOrdinal(helper.H39);
                    if (!dr.IsDBNull(iH39)) entidad.H39 = dr.GetDecimal(iH39);

                    int iH40 = dr.GetOrdinal(helper.H40);
                    if (!dr.IsDBNull(iH40)) entidad.H40 = dr.GetDecimal(iH40);

                    int iH41 = dr.GetOrdinal(helper.H41);
                    if (!dr.IsDBNull(iH41)) entidad.H41 = dr.GetDecimal(iH41);

                    int iH42 = dr.GetOrdinal(helper.H42);
                    if (!dr.IsDBNull(iH42)) entidad.H42 = dr.GetDecimal(iH42);

                    int iH43 = dr.GetOrdinal(helper.H43);
                    if (!dr.IsDBNull(iH43)) entidad.H43 = dr.GetDecimal(iH43);

                    int iH44 = dr.GetOrdinal(helper.H44);
                    if (!dr.IsDBNull(iH44)) entidad.H44 = dr.GetDecimal(iH44);

                    int iH45 = dr.GetOrdinal(helper.H45);
                    if (!dr.IsDBNull(iH45)) entidad.H45 = dr.GetDecimal(iH45);

                    int iH46 = dr.GetOrdinal(helper.H46);
                    if (!dr.IsDBNull(iH46)) entidad.H46 = dr.GetDecimal(iH46);

                    int iH47 = dr.GetOrdinal(helper.H47);
                    if (!dr.IsDBNull(iH47)) entidad.H47 = dr.GetDecimal(iH47);

                    int iH48 = dr.GetOrdinal(helper.H48);
                    if (!dr.IsDBNull(iH48)) entidad.H48 = dr.GetDecimal(iH48);

                    int iH49 = dr.GetOrdinal(helper.H49);
                    if (!dr.IsDBNull(iH49)) entidad.H49 = dr.GetDecimal(iH49);

                    int iH50 = dr.GetOrdinal(helper.H50);
                    if (!dr.IsDBNull(iH50)) entidad.H50 = dr.GetDecimal(iH50);

                    int iH51 = dr.GetOrdinal(helper.H51);
                    if (!dr.IsDBNull(iH51)) entidad.H51 = dr.GetDecimal(iH51);

                    int iH52 = dr.GetOrdinal(helper.H52);
                    if (!dr.IsDBNull(iH52)) entidad.H52 = dr.GetDecimal(iH52);

                    int iH53 = dr.GetOrdinal(helper.H53);
                    if (!dr.IsDBNull(iH53)) entidad.H53 = dr.GetDecimal(iH53);

                    int iH54 = dr.GetOrdinal(helper.H54);
                    if (!dr.IsDBNull(iH54)) entidad.H54 = dr.GetDecimal(iH54);

                    int iH55 = dr.GetOrdinal(helper.H55);
                    if (!dr.IsDBNull(iH55)) entidad.H55 = dr.GetDecimal(iH55);

                    int iH56 = dr.GetOrdinal(helper.H56);
                    if (!dr.IsDBNull(iH56)) entidad.H56 = dr.GetDecimal(iH56);

                    int iH57 = dr.GetOrdinal(helper.H57);
                    if (!dr.IsDBNull(iH57)) entidad.H57 = dr.GetDecimal(iH57);

                    int iH58 = dr.GetOrdinal(helper.H58);
                    if (!dr.IsDBNull(iH58)) entidad.H58 = dr.GetDecimal(iH58);

                    int iH59 = dr.GetOrdinal(helper.H59);
                    if (!dr.IsDBNull(iH59)) entidad.H59 = dr.GetDecimal(iH59);

                    int iH60 = dr.GetOrdinal(helper.H60);
                    if (!dr.IsDBNull(iH60)) entidad.H60 = dr.GetDecimal(iH60);

                    int iH61 = dr.GetOrdinal(helper.H61);
                    if (!dr.IsDBNull(iH61)) entidad.H61 = dr.GetDecimal(iH61);

                    int iH62 = dr.GetOrdinal(helper.H62);
                    if (!dr.IsDBNull(iH62)) entidad.H62 = dr.GetDecimal(iH62);

                    int iH63 = dr.GetOrdinal(helper.H63);
                    if (!dr.IsDBNull(iH63)) entidad.H63 = dr.GetDecimal(iH63);

                    int iH64 = dr.GetOrdinal(helper.H64);
                    if (!dr.IsDBNull(iH64)) entidad.H64 = dr.GetDecimal(iH64);

                    int iH65 = dr.GetOrdinal(helper.H65);
                    if (!dr.IsDBNull(iH65)) entidad.H65 = dr.GetDecimal(iH65);

                    int iH66 = dr.GetOrdinal(helper.H66);
                    if (!dr.IsDBNull(iH66)) entidad.H66 = dr.GetDecimal(iH66);

                    int iH67 = dr.GetOrdinal(helper.H67);
                    if (!dr.IsDBNull(iH67)) entidad.H67 = dr.GetDecimal(iH67);

                    int iH68 = dr.GetOrdinal(helper.H68);
                    if (!dr.IsDBNull(iH68)) entidad.H68 = dr.GetDecimal(iH68);

                    int iH69 = dr.GetOrdinal(helper.H69);
                    if (!dr.IsDBNull(iH69)) entidad.H69 = dr.GetDecimal(iH69);

                    int iH70 = dr.GetOrdinal(helper.H70);
                    if (!dr.IsDBNull(iH70)) entidad.H70 = dr.GetDecimal(iH70);

                    int iH71 = dr.GetOrdinal(helper.H71);
                    if (!dr.IsDBNull(iH71)) entidad.H71 = dr.GetDecimal(iH71);

                    int iH72 = dr.GetOrdinal(helper.H72);
                    if (!dr.IsDBNull(iH72)) entidad.H72 = dr.GetDecimal(iH72);

                    int iH73 = dr.GetOrdinal(helper.H73);
                    if (!dr.IsDBNull(iH73)) entidad.H73 = dr.GetDecimal(iH73);

                    int iH74 = dr.GetOrdinal(helper.H74);
                    if (!dr.IsDBNull(iH74)) entidad.H74 = dr.GetDecimal(iH74);

                    int iH75 = dr.GetOrdinal(helper.H75);
                    if (!dr.IsDBNull(iH75)) entidad.H75 = dr.GetDecimal(iH75);

                    int iH76 = dr.GetOrdinal(helper.H76);
                    if (!dr.IsDBNull(iH76)) entidad.H76 = dr.GetDecimal(iH76);

                    int iH77 = dr.GetOrdinal(helper.H77);
                    if (!dr.IsDBNull(iH77)) entidad.H77 = dr.GetDecimal(iH77);

                    int iH78 = dr.GetOrdinal(helper.H78);
                    if (!dr.IsDBNull(iH78)) entidad.H78 = dr.GetDecimal(iH78);

                    int iH79 = dr.GetOrdinal(helper.H79);
                    if (!dr.IsDBNull(iH79)) entidad.H79 = dr.GetDecimal(iH79);

                    int iH80 = dr.GetOrdinal(helper.H80);
                    if (!dr.IsDBNull(iH80)) entidad.H80 = dr.GetDecimal(iH80);

                    int iH81 = dr.GetOrdinal(helper.H81);
                    if (!dr.IsDBNull(iH81)) entidad.H81 = dr.GetDecimal(iH81);

                    int iH82 = dr.GetOrdinal(helper.H82);
                    if (!dr.IsDBNull(iH82)) entidad.H82 = dr.GetDecimal(iH82);

                    int iH83 = dr.GetOrdinal(helper.H83);
                    if (!dr.IsDBNull(iH83)) entidad.H83 = dr.GetDecimal(iH83);

                    int iH84 = dr.GetOrdinal(helper.H84);
                    if (!dr.IsDBNull(iH84)) entidad.H84 = dr.GetDecimal(iH84);

                    int iH85 = dr.GetOrdinal(helper.H85);
                    if (!dr.IsDBNull(iH85)) entidad.H85 = dr.GetDecimal(iH85);

                    int iH86 = dr.GetOrdinal(helper.H86);
                    if (!dr.IsDBNull(iH86)) entidad.H86 = dr.GetDecimal(iH86);

                    int iH87 = dr.GetOrdinal(helper.H87);
                    if (!dr.IsDBNull(iH87)) entidad.H87 = dr.GetDecimal(iH87);

                    int iH88 = dr.GetOrdinal(helper.H88);
                    if (!dr.IsDBNull(iH88)) entidad.H88 = dr.GetDecimal(iH88);

                    int iH89 = dr.GetOrdinal(helper.H89);
                    if (!dr.IsDBNull(iH89)) entidad.H89 = dr.GetDecimal(iH89);

                    int iH90 = dr.GetOrdinal(helper.H90);
                    if (!dr.IsDBNull(iH90)) entidad.H90 = dr.GetDecimal(iH90);

                    int iH91 = dr.GetOrdinal(helper.H91);
                    if (!dr.IsDBNull(iH91)) entidad.H91 = dr.GetDecimal(iH91);

                    int iH92 = dr.GetOrdinal(helper.H92);
                    if (!dr.IsDBNull(iH92)) entidad.H92 = dr.GetDecimal(iH92);

                    int iH93 = dr.GetOrdinal(helper.H93);
                    if (!dr.IsDBNull(iH93)) entidad.H93 = dr.GetDecimal(iH93);

                    int iH94 = dr.GetOrdinal(helper.H94);
                    if (!dr.IsDBNull(iH94)) entidad.H94 = dr.GetDecimal(iH94);

                    int iH95 = dr.GetOrdinal(helper.H95);
                    if (!dr.IsDBNull(iH95)) entidad.H95 = dr.GetDecimal(iH95);

                    int iH96 = dr.GetOrdinal(helper.H96);
                    if (!dr.IsDBNull(iH96)) entidad.H96 = dr.GetDecimal(iH96);

                    lIioTabla04DTO.Add(entidad);
                }
            }

            return lIioTabla04DTO;
        }

        public List<IioTabla04DTO> ListSicliByDateRange(string codigo, string inicio, string fin, string tipo)
        {
            List<IioTabla04DTO> lIioTabla04DTO = new List<IioTabla04DTO>();
            string query = string.Format(helper.SqlListSicliByDateRange
                                         , codigo, inicio, fin, tipo);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    IioTabla04DTO entidad = new IioTabla04DTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entidad.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iFechaMedicion = dr.GetOrdinal(helper.FechaMedicion);
                    if (!dr.IsDBNull(iFechaMedicion)) entidad.FechaMedicion = dr.GetDateTime(iFechaMedicion);

                    int iH1 = dr.GetOrdinal(helper.H1);
                    if (!dr.IsDBNull(iH1)) entidad.H1 = dr.GetDecimal(iH1);

                    int iH2 = dr.GetOrdinal(helper.H2);
                    if (!dr.IsDBNull(iH2)) entidad.H2 = dr.GetDecimal(iH2);

                    int iH3 = dr.GetOrdinal(helper.H3);
                    if (!dr.IsDBNull(iH3)) entidad.H3 = dr.GetDecimal(iH3);

                    int iH4 = dr.GetOrdinal(helper.H4);
                    if (!dr.IsDBNull(iH4)) entidad.H4 = dr.GetDecimal(iH4);

                    int iH5 = dr.GetOrdinal(helper.H5);
                    if (!dr.IsDBNull(iH5)) entidad.H5 = dr.GetDecimal(iH5);

                    int iH6 = dr.GetOrdinal(helper.H6);
                    if (!dr.IsDBNull(iH6)) entidad.H6 = dr.GetDecimal(iH6);

                    int iH7 = dr.GetOrdinal(helper.H7);
                    if (!dr.IsDBNull(iH7)) entidad.H7 = dr.GetDecimal(iH7);

                    int iH8 = dr.GetOrdinal(helper.H8);
                    if (!dr.IsDBNull(iH8)) entidad.H8 = dr.GetDecimal(iH8);

                    int iH9 = dr.GetOrdinal(helper.H9);
                    if (!dr.IsDBNull(iH9)) entidad.H9 = dr.GetDecimal(iH9);

                    int iH10 = dr.GetOrdinal(helper.H10);
                    if (!dr.IsDBNull(iH10)) entidad.H10 = dr.GetDecimal(iH10);

                    int iH11 = dr.GetOrdinal(helper.H11);
                    if (!dr.IsDBNull(iH11)) entidad.H11 = dr.GetDecimal(iH11);

                    int iH12 = dr.GetOrdinal(helper.H12);
                    if (!dr.IsDBNull(iH12)) entidad.H12 = dr.GetDecimal(iH12);

                    int iH13 = dr.GetOrdinal(helper.H13);
                    if (!dr.IsDBNull(iH13)) entidad.H13 = dr.GetDecimal(iH13);

                    int iH14 = dr.GetOrdinal(helper.H14);
                    if (!dr.IsDBNull(iH14)) entidad.H14 = dr.GetDecimal(iH14);

                    int iH15 = dr.GetOrdinal(helper.H15);
                    if (!dr.IsDBNull(iH15)) entidad.H15 = dr.GetDecimal(iH15);

                    int iH16 = dr.GetOrdinal(helper.H16);
                    if (!dr.IsDBNull(iH16)) entidad.H16 = dr.GetDecimal(iH16);

                    int iH17 = dr.GetOrdinal(helper.H17);
                    if (!dr.IsDBNull(iH17)) entidad.H17 = dr.GetDecimal(iH17);

                    int iH18 = dr.GetOrdinal(helper.H18);
                    if (!dr.IsDBNull(iH18)) entidad.H18 = dr.GetDecimal(iH18);

                    int iH19 = dr.GetOrdinal(helper.H19);
                    if (!dr.IsDBNull(iH19)) entidad.H19 = dr.GetDecimal(iH19);

                    int iH20 = dr.GetOrdinal(helper.H20);
                    if (!dr.IsDBNull(iH20)) entidad.H20 = dr.GetDecimal(iH20);

                    int iH21 = dr.GetOrdinal(helper.H21);
                    if (!dr.IsDBNull(iH21)) entidad.H21 = dr.GetDecimal(iH21);

                    int iH22 = dr.GetOrdinal(helper.H22);
                    if (!dr.IsDBNull(iH22)) entidad.H22 = dr.GetDecimal(iH22);

                    int iH23 = dr.GetOrdinal(helper.H23);
                    if (!dr.IsDBNull(iH23)) entidad.H23 = dr.GetDecimal(iH23);

                    int iH24 = dr.GetOrdinal(helper.H24);
                    if (!dr.IsDBNull(iH24)) entidad.H24 = dr.GetDecimal(iH24);

                    int iH25 = dr.GetOrdinal(helper.H25);
                    if (!dr.IsDBNull(iH25)) entidad.H25 = dr.GetDecimal(iH25);

                    int iH26 = dr.GetOrdinal(helper.H26);
                    if (!dr.IsDBNull(iH26)) entidad.H26 = dr.GetDecimal(iH26);

                    int iH27 = dr.GetOrdinal(helper.H27);
                    if (!dr.IsDBNull(iH27)) entidad.H27 = dr.GetDecimal(iH27);

                    int iH28 = dr.GetOrdinal(helper.H28);
                    if (!dr.IsDBNull(iH28)) entidad.H28 = dr.GetDecimal(iH28);

                    int iH29 = dr.GetOrdinal(helper.H29);
                    if (!dr.IsDBNull(iH29)) entidad.H29 = dr.GetDecimal(iH29);

                    int iH30 = dr.GetOrdinal(helper.H30);
                    if (!dr.IsDBNull(iH30)) entidad.H30 = dr.GetDecimal(iH30);

                    int iH31 = dr.GetOrdinal(helper.H31);
                    if (!dr.IsDBNull(iH31)) entidad.H31 = dr.GetDecimal(iH31);

                    int iH32 = dr.GetOrdinal(helper.H32);
                    if (!dr.IsDBNull(iH32)) entidad.H32 = dr.GetDecimal(iH32);

                    int iH33 = dr.GetOrdinal(helper.H33);
                    if (!dr.IsDBNull(iH33)) entidad.H33 = dr.GetDecimal(iH33);

                    int iH34 = dr.GetOrdinal(helper.H34);
                    if (!dr.IsDBNull(iH34)) entidad.H34 = dr.GetDecimal(iH34);

                    int iH35 = dr.GetOrdinal(helper.H35);
                    if (!dr.IsDBNull(iH35)) entidad.H35 = dr.GetDecimal(iH35);

                    int iH36 = dr.GetOrdinal(helper.H36);
                    if (!dr.IsDBNull(iH36)) entidad.H36 = dr.GetDecimal(iH36);

                    int iH37 = dr.GetOrdinal(helper.H37);
                    if (!dr.IsDBNull(iH37)) entidad.H37 = dr.GetDecimal(iH37);

                    int iH38 = dr.GetOrdinal(helper.H38);
                    if (!dr.IsDBNull(iH38)) entidad.H38 = dr.GetDecimal(iH38);

                    int iH39 = dr.GetOrdinal(helper.H39);
                    if (!dr.IsDBNull(iH39)) entidad.H39 = dr.GetDecimal(iH39);

                    int iH40 = dr.GetOrdinal(helper.H40);
                    if (!dr.IsDBNull(iH40)) entidad.H40 = dr.GetDecimal(iH40);

                    int iH41 = dr.GetOrdinal(helper.H41);
                    if (!dr.IsDBNull(iH41)) entidad.H41 = dr.GetDecimal(iH41);

                    int iH42 = dr.GetOrdinal(helper.H42);
                    if (!dr.IsDBNull(iH42)) entidad.H42 = dr.GetDecimal(iH42);

                    int iH43 = dr.GetOrdinal(helper.H43);
                    if (!dr.IsDBNull(iH43)) entidad.H43 = dr.GetDecimal(iH43);

                    int iH44 = dr.GetOrdinal(helper.H44);
                    if (!dr.IsDBNull(iH44)) entidad.H44 = dr.GetDecimal(iH44);

                    int iH45 = dr.GetOrdinal(helper.H45);
                    if (!dr.IsDBNull(iH45)) entidad.H45 = dr.GetDecimal(iH45);

                    int iH46 = dr.GetOrdinal(helper.H46);
                    if (!dr.IsDBNull(iH46)) entidad.H46 = dr.GetDecimal(iH46);

                    int iH47 = dr.GetOrdinal(helper.H47);
                    if (!dr.IsDBNull(iH47)) entidad.H47 = dr.GetDecimal(iH47);

                    int iH48 = dr.GetOrdinal(helper.H48);
                    if (!dr.IsDBNull(iH48)) entidad.H48 = dr.GetDecimal(iH48);

                    int iH49 = dr.GetOrdinal(helper.H49);
                    if (!dr.IsDBNull(iH49)) entidad.H49 = dr.GetDecimal(iH49);

                    int iH50 = dr.GetOrdinal(helper.H50);
                    if (!dr.IsDBNull(iH50)) entidad.H50 = dr.GetDecimal(iH50);

                    int iH51 = dr.GetOrdinal(helper.H51);
                    if (!dr.IsDBNull(iH51)) entidad.H51 = dr.GetDecimal(iH51);

                    int iH52 = dr.GetOrdinal(helper.H52);
                    if (!dr.IsDBNull(iH52)) entidad.H52 = dr.GetDecimal(iH52);

                    int iH53 = dr.GetOrdinal(helper.H53);
                    if (!dr.IsDBNull(iH53)) entidad.H53 = dr.GetDecimal(iH53);

                    int iH54 = dr.GetOrdinal(helper.H54);
                    if (!dr.IsDBNull(iH54)) entidad.H54 = dr.GetDecimal(iH54);

                    int iH55 = dr.GetOrdinal(helper.H55);
                    if (!dr.IsDBNull(iH55)) entidad.H55 = dr.GetDecimal(iH55);

                    int iH56 = dr.GetOrdinal(helper.H56);
                    if (!dr.IsDBNull(iH56)) entidad.H56 = dr.GetDecimal(iH56);

                    int iH57 = dr.GetOrdinal(helper.H57);
                    if (!dr.IsDBNull(iH57)) entidad.H57 = dr.GetDecimal(iH57);

                    int iH58 = dr.GetOrdinal(helper.H58);
                    if (!dr.IsDBNull(iH58)) entidad.H58 = dr.GetDecimal(iH58);

                    int iH59 = dr.GetOrdinal(helper.H59);
                    if (!dr.IsDBNull(iH59)) entidad.H59 = dr.GetDecimal(iH59);

                    int iH60 = dr.GetOrdinal(helper.H60);
                    if (!dr.IsDBNull(iH60)) entidad.H60 = dr.GetDecimal(iH60);

                    int iH61 = dr.GetOrdinal(helper.H61);
                    if (!dr.IsDBNull(iH61)) entidad.H61 = dr.GetDecimal(iH61);

                    int iH62 = dr.GetOrdinal(helper.H62);
                    if (!dr.IsDBNull(iH62)) entidad.H62 = dr.GetDecimal(iH62);

                    int iH63 = dr.GetOrdinal(helper.H63);
                    if (!dr.IsDBNull(iH63)) entidad.H63 = dr.GetDecimal(iH63);

                    int iH64 = dr.GetOrdinal(helper.H64);
                    if (!dr.IsDBNull(iH64)) entidad.H64 = dr.GetDecimal(iH64);

                    int iH65 = dr.GetOrdinal(helper.H65);
                    if (!dr.IsDBNull(iH65)) entidad.H65 = dr.GetDecimal(iH65);

                    int iH66 = dr.GetOrdinal(helper.H66);
                    if (!dr.IsDBNull(iH66)) entidad.H66 = dr.GetDecimal(iH66);

                    int iH67 = dr.GetOrdinal(helper.H67);
                    if (!dr.IsDBNull(iH67)) entidad.H67 = dr.GetDecimal(iH67);

                    int iH68 = dr.GetOrdinal(helper.H68);
                    if (!dr.IsDBNull(iH68)) entidad.H68 = dr.GetDecimal(iH68);

                    int iH69 = dr.GetOrdinal(helper.H69);
                    if (!dr.IsDBNull(iH69)) entidad.H69 = dr.GetDecimal(iH69);

                    int iH70 = dr.GetOrdinal(helper.H70);
                    if (!dr.IsDBNull(iH70)) entidad.H70 = dr.GetDecimal(iH70);

                    int iH71 = dr.GetOrdinal(helper.H71);
                    if (!dr.IsDBNull(iH71)) entidad.H71 = dr.GetDecimal(iH71);

                    int iH72 = dr.GetOrdinal(helper.H72);
                    if (!dr.IsDBNull(iH72)) entidad.H72 = dr.GetDecimal(iH72);

                    int iH73 = dr.GetOrdinal(helper.H73);
                    if (!dr.IsDBNull(iH73)) entidad.H73 = dr.GetDecimal(iH73);

                    int iH74 = dr.GetOrdinal(helper.H74);
                    if (!dr.IsDBNull(iH74)) entidad.H74 = dr.GetDecimal(iH74);

                    int iH75 = dr.GetOrdinal(helper.H75);
                    if (!dr.IsDBNull(iH75)) entidad.H75 = dr.GetDecimal(iH75);

                    int iH76 = dr.GetOrdinal(helper.H76);
                    if (!dr.IsDBNull(iH76)) entidad.H76 = dr.GetDecimal(iH76);

                    int iH77 = dr.GetOrdinal(helper.H77);
                    if (!dr.IsDBNull(iH77)) entidad.H77 = dr.GetDecimal(iH77);

                    int iH78 = dr.GetOrdinal(helper.H78);
                    if (!dr.IsDBNull(iH78)) entidad.H78 = dr.GetDecimal(iH78);

                    int iH79 = dr.GetOrdinal(helper.H79);
                    if (!dr.IsDBNull(iH79)) entidad.H79 = dr.GetDecimal(iH79);

                    int iH80 = dr.GetOrdinal(helper.H80);
                    if (!dr.IsDBNull(iH80)) entidad.H80 = dr.GetDecimal(iH80);

                    int iH81 = dr.GetOrdinal(helper.H81);
                    if (!dr.IsDBNull(iH81)) entidad.H81 = dr.GetDecimal(iH81);

                    int iH82 = dr.GetOrdinal(helper.H82);
                    if (!dr.IsDBNull(iH82)) entidad.H82 = dr.GetDecimal(iH82);

                    int iH83 = dr.GetOrdinal(helper.H83);
                    if (!dr.IsDBNull(iH83)) entidad.H83 = dr.GetDecimal(iH83);

                    int iH84 = dr.GetOrdinal(helper.H84);
                    if (!dr.IsDBNull(iH84)) entidad.H84 = dr.GetDecimal(iH84);

                    int iH85 = dr.GetOrdinal(helper.H85);
                    if (!dr.IsDBNull(iH85)) entidad.H85 = dr.GetDecimal(iH85);

                    int iH86 = dr.GetOrdinal(helper.H86);
                    if (!dr.IsDBNull(iH86)) entidad.H86 = dr.GetDecimal(iH86);

                    int iH87 = dr.GetOrdinal(helper.H87);
                    if (!dr.IsDBNull(iH87)) entidad.H87 = dr.GetDecimal(iH87);

                    int iH88 = dr.GetOrdinal(helper.H88);
                    if (!dr.IsDBNull(iH88)) entidad.H88 = dr.GetDecimal(iH88);

                    int iH89 = dr.GetOrdinal(helper.H89);
                    if (!dr.IsDBNull(iH89)) entidad.H89 = dr.GetDecimal(iH89);

                    int iH90 = dr.GetOrdinal(helper.H90);
                    if (!dr.IsDBNull(iH90)) entidad.H90 = dr.GetDecimal(iH90);

                    int iH91 = dr.GetOrdinal(helper.H91);
                    if (!dr.IsDBNull(iH91)) entidad.H91 = dr.GetDecimal(iH91);

                    int iH92 = dr.GetOrdinal(helper.H92);
                    if (!dr.IsDBNull(iH92)) entidad.H92 = dr.GetDecimal(iH92);

                    int iH93 = dr.GetOrdinal(helper.H93);
                    if (!dr.IsDBNull(iH93)) entidad.H93 = dr.GetDecimal(iH93);

                    int iH94 = dr.GetOrdinal(helper.H94);
                    if (!dr.IsDBNull(iH94)) entidad.H94 = dr.GetDecimal(iH94);

                    int iH95 = dr.GetOrdinal(helper.H95);
                    if (!dr.IsDBNull(iH95)) entidad.H95 = dr.GetDecimal(iH95);

                    int iH96 = dr.GetOrdinal(helper.H96);
                    if (!dr.IsDBNull(iH96)) entidad.H96 = dr.GetDecimal(iH96);

                    lIioTabla04DTO.Add(entidad);
                }
            }

            return lIioTabla04DTO;
        }
        #endregion

    }
}