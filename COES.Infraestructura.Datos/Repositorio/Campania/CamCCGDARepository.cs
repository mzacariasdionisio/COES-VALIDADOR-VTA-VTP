using COES.Base.Core;
using COES.Dominio.DTO.Campania;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Campania;
using COES.Infraestructura.Datos.Helper;
using COES.Infraestructura.Datos.Helper.Campania;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Campania
{
    public class CamCCGDARepository : RepositoryBase, ICamCCGDARepository
    {
        public CamCCGDARepository(string strConn) : base(strConn) { }

        CamCCGDAHelper Helper = new CamCCGDAHelper();

        public List<CCGDADTO> GetCamCCGDA(int proyCodi)
        {
            List<CCGDADTO> ccgdaDTOs = new List<CCGDADTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetCCGDA);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, proyCodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CCGDADTO ob = new CCGDADTO();
                    ob.CcgdaCodi = !dr.IsDBNull(dr.GetOrdinal("CCGDACODI")) ? dr.GetInt32(dr.GetOrdinal("CCGDACODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.NombreUnidad = !dr.IsDBNull(dr.GetOrdinal("NOMBREUNIDAD")) ? dr.GetString(dr.GetOrdinal("NOMBREUNIDAD")) : "";
                    ob.DistritoCodi = !dr.IsDBNull(dr.GetOrdinal("DISTRITOCODI")) ? dr.GetString(dr.GetOrdinal("DISTRITOCODI")) : "";
                    ob.NombreDistribuidor = !dr.IsDBNull(dr.GetOrdinal("NOMBREDISTRIBUIDOR")) ? dr.GetString(dr.GetOrdinal("NOMBREDISTRIBUIDOR")) : "";
                    ob.Propietario = !dr.IsDBNull(dr.GetOrdinal("PROPIETARIO")) ? dr.GetString(dr.GetOrdinal("PROPIETARIO")) : "";
                    ob.SocioOperador = !dr.IsDBNull(dr.GetOrdinal("SOCIOOPERADOR")) ? dr.GetString(dr.GetOrdinal("SOCIOOPERADOR")) : "";
                    ob.SocioInversionista = !dr.IsDBNull(dr.GetOrdinal("SOCIOINVERSIONISTA")) ? dr.GetString(dr.GetOrdinal("SOCIOINVERSIONISTA")) : "";
                    ob.ObjetivoProyecto = !dr.IsDBNull(dr.GetOrdinal("OBJETIVOPROYECTO")) ? dr.GetString(dr.GetOrdinal("OBJETIVOPROYECTO")) : "";
                    ob.OtroObjetivo = !dr.IsDBNull(dr.GetOrdinal("OTROOBJETIVO")) ? dr.GetString(dr.GetOrdinal("OTROOBJETIVO")) : "";
                    ob.IncluidoPlanTrans = !dr.IsDBNull(dr.GetOrdinal("INCLUIDOPLANTRANS")) ? dr.GetString(dr.GetOrdinal("INCLUIDOPLANTRANS")) : "";
                    ob.EstadoOperacion = !dr.IsDBNull(dr.GetOrdinal("ESTADOOPERACION")) ? dr.GetString(dr.GetOrdinal("ESTADOOPERACION")) : "";
                    ob.CargaRedDistribucion = !dr.IsDBNull(dr.GetOrdinal("CARGAREDDISTRIBUICION")) ? dr.GetString(dr.GetOrdinal("CARGAREDDISTRIBUICION")) : "";
                    ob.ConexionTemporal = !dr.IsDBNull(dr.GetOrdinal("CONEXIONTEMPORAL")) ? dr.GetString(dr.GetOrdinal("CONEXIONTEMPORAL")) : "";
                    ob.TipoTecnologia = !dr.IsDBNull(dr.GetOrdinal("TIPOTECNOLOGIA")) ? dr.GetString(dr.GetOrdinal("TIPOTECNOLOGIA")) : "";
                    ob.FechaAdjudicactem = !dr.IsDBNull(dr.GetOrdinal("FECADJUDICACTEM")) ? dr.GetDateTime(dr.GetOrdinal("FECADJUDICACTEM")) : (DateTime?)null;
                    ob.FechaAdjutitulo = !dr.IsDBNull(dr.GetOrdinal("FECADJUTITULO")) ? dr.GetDateTime(dr.GetOrdinal("FECADJUTITULO")) : (DateTime?)null;
                    ob.Perfil = !dr.IsDBNull(dr.GetOrdinal("PERFIL")) ? dr.GetString(dr.GetOrdinal("PERFIL")) : "";
                    ob.Prefactibilidad = !dr.IsDBNull(dr.GetOrdinal("PREFACTIBILIDAD")) ? dr.GetString(dr.GetOrdinal("PREFACTIBILIDAD")) : "";
                    ob.Factibilidad = !dr.IsDBNull(dr.GetOrdinal("FACTIBILIDAD")) ? dr.GetString(dr.GetOrdinal("FACTIBILIDAD")) : "";
                    ob.EstDefinitivo = !dr.IsDBNull(dr.GetOrdinal("ESTDEFINITIVO")) ? dr.GetString(dr.GetOrdinal("ESTDEFINITIVO")) : "";
                    ob.Eia = !dr.IsDBNull(dr.GetOrdinal("EIA")) ? dr.GetString(dr.GetOrdinal("EIA")) : "";
                    ob.FechaInicioConst = !dr.IsDBNull(dr.GetOrdinal("FECINICIOCONST")) ? dr.GetString(dr.GetOrdinal("FECINICIOCONST")) : "";
                    ob.PeriodoConst = !dr.IsDBNull(dr.GetOrdinal("PERIODOCONST")) ? dr.GetString(dr.GetOrdinal("PERIODOCONST")) : "";
                    ob.FechaOpeComercial = !dr.IsDBNull(dr.GetOrdinal("FECHAOPECOMERCIAL")) ? dr.GetString(dr.GetOrdinal("FECHAOPECOMERCIAL")) : "";
                    ob.PotInstalada = !dr.IsDBNull(dr.GetOrdinal("POTINSTALADA")) ? dr.GetString(dr.GetOrdinal("POTINSTALADA")) : "";
                    ob.RecursoUsada = !dr.IsDBNull(dr.GetOrdinal("RECURSOUSADA")) ? dr.GetString(dr.GetOrdinal("RECURSOUSADA")) : "";
                    ob.Tecnologia = !dr.IsDBNull(dr.GetOrdinal("TECNOLOGIA")) ? dr.GetString(dr.GetOrdinal("TECNOLOGIA")) : "";
                    ob.TecOtro = !dr.IsDBNull(dr.GetOrdinal("TECOTRO")) ? dr.GetString(dr.GetOrdinal("TECOTRO")) : "";
                    ob.BarraConexion = !dr.IsDBNull(dr.GetOrdinal("BARRACONEXION")) ? dr.GetString(dr.GetOrdinal("BARRACONEXION")) : "";
                    ob.NivelTension = !dr.IsDBNull(dr.GetOrdinal("NIVELTENSION")) ? dr.GetString(dr.GetOrdinal("NIVELTENSION")) : "";
                    ob.NombreProyectoGD = !dr.IsDBNull(dr.GetOrdinal("NOMBREPROYECTOGD")) ? dr.GetString(dr.GetOrdinal("NOMBREPROYECTOGD")) : "";
                    ob.IncluidoPlanTransGD = !dr.IsDBNull(dr.GetOrdinal("INCLUIDOPLANTRANSGD")) ? dr.GetString(dr.GetOrdinal("INCLUIDOPLANTRANSGD")) : "";
                    ob.DistritoGDCodi = !dr.IsDBNull(dr.GetOrdinal("DISTRITOGDCODI")) ? dr.GetString(dr.GetOrdinal("DISTRITOGDCODI")) : "";
                    ob.NomDistribuidorGD = !dr.IsDBNull(dr.GetOrdinal("NOMDISTRIBUIDORGD")) ? dr.GetString(dr.GetOrdinal("NOMDISTRIBUIDORGD")) : "";
                    ob.PropietarioGD = !dr.IsDBNull(dr.GetOrdinal("PROPIETARIOGD")) ? dr.GetString(dr.GetOrdinal("PROPIETARIOGD")) : "";
                    ob.SocioOperadorGD = !dr.IsDBNull(dr.GetOrdinal("SOCIOOPERADORGD")) ? dr.GetString(dr.GetOrdinal("SOCIOOPERADORGD")) : "";
                    ob.SocioInversionistaGD = !dr.IsDBNull(dr.GetOrdinal("SOCIOINVERSIONISTAGD")) ? dr.GetString(dr.GetOrdinal("SOCIOINVERSIONISTAGD")) : "";
                    ob.EstadoOperacionGD = !dr.IsDBNull(dr.GetOrdinal("ESTADOOPERACIONGD")) ? dr.GetString(dr.GetOrdinal("ESTADOOPERACIONGD")) : "";
                    ob.CargaRedDistribucionGD = !dr.IsDBNull(dr.GetOrdinal("CARGAREDDISTRIBUICIONGD")) ? dr.GetString(dr.GetOrdinal("CARGAREDDISTRIBUICIONGD")) : "";
                    ob.BarraConexionGD = !dr.IsDBNull(dr.GetOrdinal("BARRACONEXIONGD")) ? dr.GetString(dr.GetOrdinal("BARRACONEXIONGD")) : "";
                    ob.NivelTensionGD = !dr.IsDBNull(dr.GetOrdinal("NIVELTENSIONGD")) ? dr.GetString(dr.GetOrdinal("NIVELTENSIONGD")) : "";
                    ob.Comentarios = !dr.IsDBNull(dr.GetOrdinal("COMENTARIOS")) ? dr.GetString(dr.GetOrdinal("COMENTARIOS")) : "";
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : "";
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : "";
                    ob.FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : "";

                    ccgdaDTOs.Add(ob);
                }
            }

            return ccgdaDTOs;
        }

        public bool SaveCamCCGDA(CCGDADTO ccgdaDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveCCGDA);
            dbProvider.AddInParameter(dbCommand, "CCGDACODI", DbType.Int32, ccgdaDTO.CcgdaCodi);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, ccgdaDTO.ProyCodi);
            dbProvider.AddInParameter(dbCommand, "NOMBREUNIDAD", DbType.String, ccgdaDTO.NombreUnidad);
            dbProvider.AddInParameter(dbCommand, "DISTRITOCODI", DbType.String, ccgdaDTO.DistritoCodi);
            dbProvider.AddInParameter(dbCommand, "NOMBREDISTRIBUIDOR", DbType.String, ccgdaDTO.NombreDistribuidor);
            dbProvider.AddInParameter(dbCommand, "PROPIETARIO", DbType.String, ccgdaDTO.Propietario);
            dbProvider.AddInParameter(dbCommand, "SOCIOOPERADOR", DbType.String, ccgdaDTO.SocioOperador);
            dbProvider.AddInParameter(dbCommand, "SOCIOINVERSIONISTA", DbType.String, ccgdaDTO.SocioInversionista);
            dbProvider.AddInParameter(dbCommand, "OBJETIVOPROYECTO", DbType.String, ccgdaDTO.ObjetivoProyecto);
            dbProvider.AddInParameter(dbCommand, "OTROOBJETIVO", DbType.String, ccgdaDTO.OtroObjetivo);
            dbProvider.AddInParameter(dbCommand, "INCLUIDOPLANTRANS", DbType.String, ccgdaDTO.IncluidoPlanTrans);
            dbProvider.AddInParameter(dbCommand, "ESTADOOPERACION", DbType.String, ccgdaDTO.EstadoOperacion);
            dbProvider.AddInParameter(dbCommand, "CARGAREDDISTRIBUICION", DbType.String, ccgdaDTO.CargaRedDistribucion);
            dbProvider.AddInParameter(dbCommand, "CONEXIONTEMPORAL", DbType.String, ccgdaDTO.ConexionTemporal);
            dbProvider.AddInParameter(dbCommand, "TIPOTECNOLOGIA", DbType.String, ccgdaDTO.TipoTecnologia);
            dbProvider.AddInParameter(dbCommand, "FECADJUDICACTEM", DbType.DateTime, ccgdaDTO.FechaAdjudicactem);
            dbProvider.AddInParameter(dbCommand, "FECADJUTITULO", DbType.DateTime, ccgdaDTO.FechaAdjutitulo);
            dbProvider.AddInParameter(dbCommand, "PERFIL", DbType.String, ccgdaDTO.Perfil);
            dbProvider.AddInParameter(dbCommand, "PREFACTIBILIDAD", DbType.String, ccgdaDTO.Prefactibilidad);
            dbProvider.AddInParameter(dbCommand, "FACTIBILIDAD", DbType.String, ccgdaDTO.Factibilidad);
            dbProvider.AddInParameter(dbCommand, "ESTDEFINITIVO", DbType.String, ccgdaDTO.EstDefinitivo);
            dbProvider.AddInParameter(dbCommand, "EIA", DbType.String, ccgdaDTO.Eia);
            dbProvider.AddInParameter(dbCommand, "FECINICIOCONST", DbType.String, ccgdaDTO.FechaInicioConst);
            dbProvider.AddInParameter(dbCommand, "PERIODOCONST", DbType.String, ccgdaDTO.PeriodoConst);
            dbProvider.AddInParameter(dbCommand, "FECHAOPECOMERCIAL", DbType.String, ccgdaDTO.FechaOpeComercial);
            dbProvider.AddInParameter(dbCommand, "POTINSTALADA", DbType.String, ccgdaDTO.PotInstalada);
            dbProvider.AddInParameter(dbCommand, "RECURSOUSADA", DbType.String, ccgdaDTO.RecursoUsada);
            dbProvider.AddInParameter(dbCommand, "TECNOLOGIA", DbType.String, ccgdaDTO.Tecnologia);
            dbProvider.AddInParameter(dbCommand, "TECOTRO", DbType.String, ccgdaDTO.TecOtro);
            dbProvider.AddInParameter(dbCommand, "BARRACONEXION", DbType.String, ccgdaDTO.BarraConexion);
            dbProvider.AddInParameter(dbCommand, "NIVELTENSION", DbType.String, ccgdaDTO.NivelTension);
            dbProvider.AddInParameter(dbCommand, "NOMBREPROYECTOGD", DbType.String, ccgdaDTO.NombreProyectoGD);
            dbProvider.AddInParameter(dbCommand, "INCLUIDOPLANTRANSGD", DbType.String, ccgdaDTO.IncluidoPlanTransGD);
            dbProvider.AddInParameter(dbCommand, "DISTRITOGDCODI", DbType.String, ccgdaDTO.DistritoGDCodi);
            dbProvider.AddInParameter(dbCommand, "NOMDISTRIBUIDORGD", DbType.String, ccgdaDTO.NomDistribuidorGD);
            dbProvider.AddInParameter(dbCommand, "PROPIETARIOGD", DbType.String, ccgdaDTO.PropietarioGD);
            dbProvider.AddInParameter(dbCommand, "SOCIOOPERADORGD", DbType.String, ccgdaDTO.SocioOperadorGD);
            dbProvider.AddInParameter(dbCommand, "SOCIOINVERSIONISTAGD", DbType.String, ccgdaDTO.SocioInversionistaGD);
            dbProvider.AddInParameter(dbCommand, "ESTADOOPERACIONGD", DbType.String, ccgdaDTO.EstadoOperacionGD);
            dbProvider.AddInParameter(dbCommand, "CARGAREDDISTRIBUICIONGD", DbType.String, ccgdaDTO.CargaRedDistribucionGD);
            dbProvider.AddInParameter(dbCommand, "BARRACONEXIONGD", DbType.String, ccgdaDTO.BarraConexionGD);
            dbProvider.AddInParameter(dbCommand, "NIVELTENSIONGD", DbType.String, ccgdaDTO.NivelTensionGD);
            dbProvider.AddInParameter(dbCommand, "COMENTARIOS", DbType.String, ccgdaDTO.Comentarios);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, ccgdaDTO.UsuCreacion);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, ccgdaDTO.FecCreacion);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, ccgdaDTO.IndDel);

            dbProvider.ExecuteNonQuery(dbCommand);

            return true;
        }

        public bool DeleteCamCCGDAById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteCCGDAById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);

            return true;
        }

        public int GetLastCamCCGDAId()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastCCGDAById);
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

        public CCGDADTO GetCamCCGDAById(int id)
        {
            CCGDADTO ob = new CCGDADTO();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetCcgdaById);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    ob.CcgdaCodi = !dr.IsDBNull(dr.GetOrdinal("CCGDACODI")) ? dr.GetInt32(dr.GetOrdinal("CCGDACODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.NombreUnidad = !dr.IsDBNull(dr.GetOrdinal("NOMBREUNIDAD")) ? dr.GetString(dr.GetOrdinal("NOMBREUNIDAD")) : "";
                    ob.DistritoCodi = !dr.IsDBNull(dr.GetOrdinal("DISTRITOCODI")) ? dr.GetString(dr.GetOrdinal("DISTRITOCODI")) : "";
                    ob.NombreDistribuidor = !dr.IsDBNull(dr.GetOrdinal("NOMBREDISTRIBUIDOR")) ? dr.GetString(dr.GetOrdinal("NOMBREDISTRIBUIDOR")) : "";
                    ob.Propietario = !dr.IsDBNull(dr.GetOrdinal("PROPIETARIO")) ? dr.GetString(dr.GetOrdinal("PROPIETARIO")) : "";
                    ob.SocioOperador = !dr.IsDBNull(dr.GetOrdinal("SOCIOOPERADOR")) ? dr.GetString(dr.GetOrdinal("SOCIOOPERADOR")) : "";
                    ob.SocioInversionista = !dr.IsDBNull(dr.GetOrdinal("SOCIOINVERSIONISTA")) ? dr.GetString(dr.GetOrdinal("SOCIOINVERSIONISTA")) : "";
                    ob.ObjetivoProyecto = !dr.IsDBNull(dr.GetOrdinal("OBJETIVOPROYECTO")) ? dr.GetString(dr.GetOrdinal("OBJETIVOPROYECTO")) : "";
                    ob.OtroObjetivo = !dr.IsDBNull(dr.GetOrdinal("OTROOBJETIVO")) ? dr.GetString(dr.GetOrdinal("OTROOBJETIVO")) : "";
                    ob.IncluidoPlanTrans = !dr.IsDBNull(dr.GetOrdinal("INCLUIDOPLANTRANS")) ? dr.GetString(dr.GetOrdinal("INCLUIDOPLANTRANS")) : "";
                    ob.EstadoOperacion = !dr.IsDBNull(dr.GetOrdinal("ESTADOOPERACION")) ? dr.GetString(dr.GetOrdinal("ESTADOOPERACION")) : "";
                    ob.CargaRedDistribucion = !dr.IsDBNull(dr.GetOrdinal("CARGAREDDISTRIBUICION")) ? dr.GetString(dr.GetOrdinal("CARGAREDDISTRIBUICION")) : "";
                    ob.ConexionTemporal = !dr.IsDBNull(dr.GetOrdinal("CONEXIONTEMPORAL")) ? dr.GetString(dr.GetOrdinal("CONEXIONTEMPORAL")) : "";
                    ob.TipoTecnologia = !dr.IsDBNull(dr.GetOrdinal("TIPOTECNOLOGIA")) ? dr.GetString(dr.GetOrdinal("TIPOTECNOLOGIA")) : "";
                    ob.FechaAdjudicactem = !dr.IsDBNull(dr.GetOrdinal("FECADJUDICACTEM")) ? dr.GetDateTime(dr.GetOrdinal("FECADJUDICACTEM")) : (DateTime?)null;
                    ob.FechaAdjutitulo = !dr.IsDBNull(dr.GetOrdinal("FECADJUTITULO")) ? dr.GetDateTime(dr.GetOrdinal("FECADJUTITULO")) : (DateTime?)null;
                    ob.Perfil = !dr.IsDBNull(dr.GetOrdinal("PERFIL")) ? dr.GetString(dr.GetOrdinal("PERFIL")) : "";
                    ob.Prefactibilidad = !dr.IsDBNull(dr.GetOrdinal("PREFACTIBILIDAD")) ? dr.GetString(dr.GetOrdinal("PREFACTIBILIDAD")) : "";
                    ob.Factibilidad = !dr.IsDBNull(dr.GetOrdinal("FACTIBILIDAD")) ? dr.GetString(dr.GetOrdinal("FACTIBILIDAD")) : "";
                    ob.EstDefinitivo = !dr.IsDBNull(dr.GetOrdinal("ESTDEFINITIVO")) ? dr.GetString(dr.GetOrdinal("ESTDEFINITIVO")) : "";
                    ob.Eia = !dr.IsDBNull(dr.GetOrdinal("EIA")) ? dr.GetString(dr.GetOrdinal("EIA")) : "";
                    ob.FechaInicioConst = !dr.IsDBNull(dr.GetOrdinal("FECINICIOCONST")) ? dr.GetString(dr.GetOrdinal("FECINICIOCONST")) : "";
                    ob.PeriodoConst = !dr.IsDBNull(dr.GetOrdinal("PERIODOCONST")) ? dr.GetString(dr.GetOrdinal("PERIODOCONST")) : "";
                    ob.FechaOpeComercial = !dr.IsDBNull(dr.GetOrdinal("FECHAOPECOMERCIAL")) ? dr.GetString(dr.GetOrdinal("FECHAOPECOMERCIAL")) : "";
                    ob.PotInstalada = !dr.IsDBNull(dr.GetOrdinal("POTINSTALADA")) ? dr.GetString(dr.GetOrdinal("POTINSTALADA")) : "";
                    ob.RecursoUsada = !dr.IsDBNull(dr.GetOrdinal("RECURSOUSADA")) ? dr.GetString(dr.GetOrdinal("RECURSOUSADA")) : "";
                    ob.Tecnologia = !dr.IsDBNull(dr.GetOrdinal("TECNOLOGIA")) ? dr.GetString(dr.GetOrdinal("TECNOLOGIA")) : "";
                    ob.TecOtro = !dr.IsDBNull(dr.GetOrdinal("TECOTRO")) ? dr.GetString(dr.GetOrdinal("TECOTRO")) : "";
                    ob.BarraConexion = !dr.IsDBNull(dr.GetOrdinal("BARRACONEXION")) ? dr.GetString(dr.GetOrdinal("BARRACONEXION")) : "";
                    ob.NivelTension = !dr.IsDBNull(dr.GetOrdinal("NIVELTENSION")) ? dr.GetString(dr.GetOrdinal("NIVELTENSION")) : "";
                    ob.IncluidoPlanTransGD = !dr.IsDBNull(dr.GetOrdinal("INCLUIDOPLANTRANSGD")) ? dr.GetString(dr.GetOrdinal("INCLUIDOPLANTRANSGD")) : "";
                    ob.NombreProyectoGD = !dr.IsDBNull(dr.GetOrdinal("NOMBREPROYECTOGD")) ? dr.GetString(dr.GetOrdinal("NOMBREPROYECTOGD")) : "";
                    ob.DistritoGDCodi = !dr.IsDBNull(dr.GetOrdinal("DISTRITOGDCODI")) ? dr.GetString(dr.GetOrdinal("DISTRITOGDCODI")) : "";
                    ob.NomDistribuidorGD = !dr.IsDBNull(dr.GetOrdinal("NOMDISTRIBUIDORGD")) ? dr.GetString(dr.GetOrdinal("NOMDISTRIBUIDORGD")) : "";
                    ob.PropietarioGD = !dr.IsDBNull(dr.GetOrdinal("PROPIETARIOGD")) ? dr.GetString(dr.GetOrdinal("PROPIETARIOGD")) : "";
                    ob.SocioOperadorGD = !dr.IsDBNull(dr.GetOrdinal("SOCIOOPERADORGD")) ? dr.GetString(dr.GetOrdinal("SOCIOOPERADORGD")) : "";
                    ob.SocioInversionistaGD = !dr.IsDBNull(dr.GetOrdinal("SOCIOINVERSIONISTAGD")) ? dr.GetString(dr.GetOrdinal("SOCIOINVERSIONISTAGD")) : "";
                    ob.EstadoOperacionGD = !dr.IsDBNull(dr.GetOrdinal("ESTADOOPERACIONGD")) ? dr.GetString(dr.GetOrdinal("ESTADOOPERACIONGD")) : "";
                    ob.CargaRedDistribucionGD = !dr.IsDBNull(dr.GetOrdinal("CARGAREDDISTRIBUICIONGD")) ? dr.GetString(dr.GetOrdinal("CARGAREDDISTRIBUICIONGD")) : "";
                    ob.BarraConexionGD = !dr.IsDBNull(dr.GetOrdinal("BARRACONEXIONGD")) ? dr.GetString(dr.GetOrdinal("BARRACONEXIONGD")) : "";
                    ob.NivelTensionGD = !dr.IsDBNull(dr.GetOrdinal("NIVELTENSIONGD")) ? dr.GetString(dr.GetOrdinal("NIVELTENSIONGD")) : "";
                    ob.Comentarios = !dr.IsDBNull(dr.GetOrdinal("COMENTARIOS")) ? dr.GetString(dr.GetOrdinal("COMENTARIOS")) : "";
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : "";
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : "";
                    ob.FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : "";
                }
            }
            return ob;
        }

        public bool UpdateCamCCGDA(CCGDADTO ccgdaDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlUpdateCCGDA);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, ccgdaDTO.ProyCodi);
            dbProvider.AddInParameter(dbCommand, "NOMBREUNIDAD", DbType.String, ccgdaDTO.NombreUnidad);
            dbProvider.AddInParameter(dbCommand, "DISTRITOCODI", DbType.String, ccgdaDTO.DistritoCodi);
            dbProvider.AddInParameter(dbCommand, "NOMBREDISTRIBUIDOR", DbType.String, ccgdaDTO.NombreDistribuidor);
            dbProvider.AddInParameter(dbCommand, "PROPIETARIO", DbType.String, ccgdaDTO.Propietario);
            dbProvider.AddInParameter(dbCommand, "SOCIOOPERADOR", DbType.String, ccgdaDTO.SocioOperador);
            dbProvider.AddInParameter(dbCommand, "SOCIOINVERSIONISTA", DbType.String, ccgdaDTO.SocioInversionista);
            dbProvider.AddInParameter(dbCommand, "OBJETIVOPROYECTO", DbType.String, ccgdaDTO.ObjetivoProyecto);
            dbProvider.AddInParameter(dbCommand, "OTROOBJETIVO", DbType.String, ccgdaDTO.OtroObjetivo);
            dbProvider.AddInParameter(dbCommand, "INCLUIDOPLANTRANS", DbType.String, ccgdaDTO.IncluidoPlanTrans);
            dbProvider.AddInParameter(dbCommand, "ESTADOOPERACION", DbType.String, ccgdaDTO.EstadoOperacion);
            dbProvider.AddInParameter(dbCommand, "CARGAREDDISTRIBUCION", DbType.String, ccgdaDTO.CargaRedDistribucion);
            dbProvider.AddInParameter(dbCommand, "CONEXIONTEMPORAL", DbType.String, ccgdaDTO.ConexionTemporal);
            dbProvider.AddInParameter(dbCommand, "TIPOTECNOLOGIA", DbType.String, ccgdaDTO.TipoTecnologia);
            dbProvider.AddInParameter(dbCommand, "FECADJUDICACTEM", DbType.DateTime, ccgdaDTO.FechaAdjudicactem);
            dbProvider.AddInParameter(dbCommand, "FECADJUTITULO", DbType.DateTime, ccgdaDTO.FechaAdjutitulo);
            dbProvider.AddInParameter(dbCommand, "PERFIL", DbType.String, ccgdaDTO.Perfil);
            dbProvider.AddInParameter(dbCommand, "PREFACTIBILIDAD", DbType.String, ccgdaDTO.Prefactibilidad);
            dbProvider.AddInParameter(dbCommand, "FACTIBILIDAD", DbType.String, ccgdaDTO.Factibilidad);
            dbProvider.AddInParameter(dbCommand, "ESTDEFINITIVO", DbType.String, ccgdaDTO.EstDefinitivo);
            dbProvider.AddInParameter(dbCommand, "EIA", DbType.String, ccgdaDTO.Eia);
            dbProvider.AddInParameter(dbCommand, "FECINICIOCONST", DbType.String, ccgdaDTO.FechaInicioConst);
            dbProvider.AddInParameter(dbCommand, "PERIODOCONST", DbType.String, ccgdaDTO.PeriodoConst);
            dbProvider.AddInParameter(dbCommand, "FECHAOPECOMERCIAL", DbType.String, ccgdaDTO.FechaOpeComercial);
            dbProvider.AddInParameter(dbCommand, "POTINSTALADA", DbType.String, ccgdaDTO.PotInstalada);
            dbProvider.AddInParameter(dbCommand, "RECURSOUSADA", DbType.String, ccgdaDTO.RecursoUsada);
            dbProvider.AddInParameter(dbCommand, "TECNOLOGIA", DbType.String, ccgdaDTO.Tecnologia);
            dbProvider.AddInParameter(dbCommand, "TECOTRO", DbType.String, ccgdaDTO.TecOtro);
            dbProvider.AddInParameter(dbCommand, "BARRACONEXION", DbType.String, ccgdaDTO.BarraConexion);
            dbProvider.AddInParameter(dbCommand, "NIVELTENSION", DbType.String, ccgdaDTO.NivelTension);
            dbProvider.AddInParameter(dbCommand, "NOMBREPROYECTOGD", DbType.String, ccgdaDTO.NombreProyectoGD);
            dbProvider.AddInParameter(dbCommand, "INCLUIDOPLANTRANSGD", DbType.String, ccgdaDTO.IncluidoPlanTransGD);
            dbProvider.AddInParameter(dbCommand, "DISTRITOGDCODI", DbType.String, ccgdaDTO.DistritoGDCodi);
            dbProvider.AddInParameter(dbCommand, "NOMDISTRIBUIDORGD", DbType.String, ccgdaDTO.NomDistribuidorGD);
            dbProvider.AddInParameter(dbCommand, "PROPIETARIOGD", DbType.String, ccgdaDTO.PropietarioGD);
            dbProvider.AddInParameter(dbCommand, "SOCIOOPERADORGD", DbType.String, ccgdaDTO.SocioOperadorGD);
            dbProvider.AddInParameter(dbCommand, "SOCIOINVERSIONISTAGD", DbType.String, ccgdaDTO.SocioInversionistaGD);
            dbProvider.AddInParameter(dbCommand, "ESTADOOPERACIONGD", DbType.String, ccgdaDTO.EstadoOperacionGD);
            dbProvider.AddInParameter(dbCommand, "CARGAREDDISTRIBUICIONGD", DbType.String, ccgdaDTO.CargaRedDistribucionGD);
            dbProvider.AddInParameter(dbCommand, "BARRACONEXIONGD", DbType.String, ccgdaDTO.BarraConexionGD);
            dbProvider.AddInParameter(dbCommand, "NIVELTENSIONGD", DbType.String, ccgdaDTO.NivelTensionGD);
            dbProvider.AddInParameter(dbCommand, "COMENTARIOS", DbType.String, ccgdaDTO.Comentarios);
            dbProvider.AddInParameter(dbCommand, "USU_MODIFICACION", DbType.String, ccgdaDTO.UsuModificacion);
            dbProvider.AddInParameter(dbCommand, "FEC_MODIFICACION", DbType.DateTime, ccgdaDTO.FecModificacion);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, ccgdaDTO.IndDel);
            dbProvider.AddInParameter(dbCommand, "CCGDACODI", DbType.Int32, ccgdaDTO.CcgdaCodi);

            dbProvider.ExecuteNonQuery(dbCommand);

            return true;
        }


        public List<CCGDADTO> GetCamCCGDAByFilter(string plancodi, string empresa, string estado )
        {
            List <CCGDADTO> listob = new List<CCGDADTO>();

            string query = $@"
                SELECT CGB.*, TR.EMPRESANOM, TR.PROYNOMBRE, TR.PROYDESCRIPCION, TP.TIPONOMBRE, TF.TIPOFINOMBRE,TR.PROYCONFIDENCIAL  FROM CAM_CCGDA CGB
                INNER JOIN CAM_TRNSMPROYECTO TR ON TR.PROYCODI = CGB.PROYCODI
                INNER JOIN CAM_PLANTRANSMISION PL ON PL.PLANCODI = TR.PLANCODI
                INNER JOIN CAM_TIPOPROYECTO TP ON TP.TIPOCODI = TR.TIPOCODI
                LEFT JOIN CAM_TIPOFICHAPROYECTO TF ON TF.TIPOFICODI = TR.TIPOFICODI
                WHERE TR.PERICODI  IN ({plancodi}) AND 
                PL.CODEMPRESA IN ({empresa})  AND 
                CGB.IND_DEL = 0 AND 
                PL.PLANESTADO ='{estado}'
                ORDER BY TR.PERICODI, CGB.PROYCODI,PL.CODEMPRESA, CGB.CCGDACODI ASC";

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CCGDADTO ob = new CCGDADTO();
                    ob.CcgdaCodi = !dr.IsDBNull(dr.GetOrdinal("CCGDACODI")) ? dr.GetInt32(dr.GetOrdinal("CCGDACODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.NombreUnidad = !dr.IsDBNull(dr.GetOrdinal("NOMBREUNIDAD")) ? dr.GetString(dr.GetOrdinal("NOMBREUNIDAD")) : "";
                    ob.DistritoCodi = !dr.IsDBNull(dr.GetOrdinal("DISTRITOCODI")) ? dr.GetString(dr.GetOrdinal("DISTRITOCODI")) : "";
                    ob.NombreDistribuidor = !dr.IsDBNull(dr.GetOrdinal("NOMBREDISTRIBUIDOR")) ? dr.GetString(dr.GetOrdinal("NOMBREDISTRIBUIDOR")) : "";
                    ob.Propietario = !dr.IsDBNull(dr.GetOrdinal("PROPIETARIO")) ? dr.GetString(dr.GetOrdinal("PROPIETARIO")) : "";
                    ob.SocioOperador = !dr.IsDBNull(dr.GetOrdinal("SOCIOOPERADOR")) ? dr.GetString(dr.GetOrdinal("SOCIOOPERADOR")) : "";
                    ob.SocioInversionista = !dr.IsDBNull(dr.GetOrdinal("SOCIOINVERSIONISTA")) ? dr.GetString(dr.GetOrdinal("SOCIOINVERSIONISTA")) : "";
                    ob.ObjetivoProyecto = !dr.IsDBNull(dr.GetOrdinal("OBJETIVOPROYECTO")) ? dr.GetString(dr.GetOrdinal("OBJETIVOPROYECTO")) : "";
                    ob.OtroObjetivo = !dr.IsDBNull(dr.GetOrdinal("OTROOBJETIVO")) ? dr.GetString(dr.GetOrdinal("OTROOBJETIVO")) : "";
                    ob.IncluidoPlanTrans = !dr.IsDBNull(dr.GetOrdinal("INCLUIDOPLANTRANS")) ? dr.GetString(dr.GetOrdinal("INCLUIDOPLANTRANS")) : "";
                    ob.EstadoOperacion = !dr.IsDBNull(dr.GetOrdinal("ESTADOOPERACION")) ? dr.GetString(dr.GetOrdinal("ESTADOOPERACION")) : "";
                    ob.CargaRedDistribucion = !dr.IsDBNull(dr.GetOrdinal("CARGAREDDISTRIBUICION")) ? dr.GetString(dr.GetOrdinal("CARGAREDDISTRIBUICION")) : "";
                    ob.ConexionTemporal = !dr.IsDBNull(dr.GetOrdinal("CONEXIONTEMPORAL")) ? dr.GetString(dr.GetOrdinal("CONEXIONTEMPORAL")) : "";
                    ob.TipoTecnologia = !dr.IsDBNull(dr.GetOrdinal("TIPOTECNOLOGIA")) ? dr.GetString(dr.GetOrdinal("TIPOTECNOLOGIA")) : "";
                    ob.FechaAdjudicactem = !dr.IsDBNull(dr.GetOrdinal("FECADJUDICACTEM")) ? dr.GetDateTime(dr.GetOrdinal("FECADJUDICACTEM")) : (DateTime?)null;
                    ob.FechaAdjutitulo = !dr.IsDBNull(dr.GetOrdinal("FECADJUTITULO")) ? dr.GetDateTime(dr.GetOrdinal("FECADJUTITULO")) : (DateTime?)null;
                    ob.Perfil = !dr.IsDBNull(dr.GetOrdinal("PERFIL")) ? dr.GetString(dr.GetOrdinal("PERFIL")) : "";
                    ob.Prefactibilidad = !dr.IsDBNull(dr.GetOrdinal("PREFACTIBILIDAD")) ? dr.GetString(dr.GetOrdinal("PREFACTIBILIDAD")) : "";
                    ob.Factibilidad = !dr.IsDBNull(dr.GetOrdinal("FACTIBILIDAD")) ? dr.GetString(dr.GetOrdinal("FACTIBILIDAD")) : "";
                    ob.EstDefinitivo = !dr.IsDBNull(dr.GetOrdinal("ESTDEFINITIVO")) ? dr.GetString(dr.GetOrdinal("ESTDEFINITIVO")) : "";
                    ob.Eia = !dr.IsDBNull(dr.GetOrdinal("EIA")) ? dr.GetString(dr.GetOrdinal("EIA")) : "";
                    ob.FechaInicioConst = !dr.IsDBNull(dr.GetOrdinal("FECINICIOCONST")) ? dr.GetString(dr.GetOrdinal("FECINICIOCONST")) : "";
                    ob.PeriodoConst = !dr.IsDBNull(dr.GetOrdinal("PERIODOCONST")) ? dr.GetString(dr.GetOrdinal("PERIODOCONST")) : "";
                    ob.FechaOpeComercial = !dr.IsDBNull(dr.GetOrdinal("FECHAOPECOMERCIAL")) ? dr.GetString(dr.GetOrdinal("FECHAOPECOMERCIAL")) : "";
                    ob.PotInstalada = !dr.IsDBNull(dr.GetOrdinal("POTINSTALADA")) ? dr.GetString(dr.GetOrdinal("POTINSTALADA")) : "";
                    ob.RecursoUsada = !dr.IsDBNull(dr.GetOrdinal("RECURSOUSADA")) ? dr.GetString(dr.GetOrdinal("RECURSOUSADA")) : "";
                    ob.Tecnologia = !dr.IsDBNull(dr.GetOrdinal("TECNOLOGIA")) ? dr.GetString(dr.GetOrdinal("TECNOLOGIA")) : "";
                    ob.TecOtro = !dr.IsDBNull(dr.GetOrdinal("TECOTRO")) ? dr.GetString(dr.GetOrdinal("TECOTRO")) : "";
                    ob.BarraConexion = !dr.IsDBNull(dr.GetOrdinal("BARRACONEXION")) ? dr.GetString(dr.GetOrdinal("BARRACONEXION")) : "";
                    ob.NivelTension = !dr.IsDBNull(dr.GetOrdinal("NIVELTENSION")) ? dr.GetString(dr.GetOrdinal("NIVELTENSION")) : "";
                    ob.IncluidoPlanTransGD = !dr.IsDBNull(dr.GetOrdinal("INCLUIDOPLANTRANSGD")) ? dr.GetString(dr.GetOrdinal("INCLUIDOPLANTRANSGD")) : "";
                    ob.NombreProyectoGD = !dr.IsDBNull(dr.GetOrdinal("NOMBREPROYECTOGD")) ? dr.GetString(dr.GetOrdinal("NOMBREPROYECTOGD")) : "";
                    ob.DistritoGDCodi = !dr.IsDBNull(dr.GetOrdinal("DISTRITOGDCODI")) ? dr.GetString(dr.GetOrdinal("DISTRITOGDCODI")) : "";
                    ob.NomDistribuidorGD = !dr.IsDBNull(dr.GetOrdinal("NOMDISTRIBUIDORGD")) ? dr.GetString(dr.GetOrdinal("NOMDISTRIBUIDORGD")) : "";
                    ob.PropietarioGD = !dr.IsDBNull(dr.GetOrdinal("PROPIETARIOGD")) ? dr.GetString(dr.GetOrdinal("PROPIETARIOGD")) : "";
                    ob.SocioOperadorGD = !dr.IsDBNull(dr.GetOrdinal("SOCIOOPERADORGD")) ? dr.GetString(dr.GetOrdinal("SOCIOOPERADORGD")) : "";
                    ob.SocioInversionistaGD = !dr.IsDBNull(dr.GetOrdinal("SOCIOINVERSIONISTAGD")) ? dr.GetString(dr.GetOrdinal("SOCIOINVERSIONISTAGD")) : "";
                    ob.EstadoOperacionGD = !dr.IsDBNull(dr.GetOrdinal("ESTADOOPERACIONGD")) ? dr.GetString(dr.GetOrdinal("ESTADOOPERACIONGD")) : "";
                    ob.CargaRedDistribucionGD = !dr.IsDBNull(dr.GetOrdinal("CARGAREDDISTRIBUICIONGD")) ? dr.GetString(dr.GetOrdinal("CARGAREDDISTRIBUICIONGD")) : "";
                    ob.BarraConexionGD = !dr.IsDBNull(dr.GetOrdinal("BARRACONEXIONGD")) ? dr.GetString(dr.GetOrdinal("BARRACONEXIONGD")) : "";
                    ob.NivelTensionGD = !dr.IsDBNull(dr.GetOrdinal("NIVELTENSIONGD")) ? dr.GetString(dr.GetOrdinal("NIVELTENSIONGD")) : "";
                    ob.Comentarios = !dr.IsDBNull(dr.GetOrdinal("COMENTARIOS")) ? dr.GetString(dr.GetOrdinal("COMENTARIOS")) : "";
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : "";
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : "";
                    ob.FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : "";
                    ob.Empresa = dr.IsDBNull(dr.GetOrdinal("EMPRESANOM")) ? null : dr.GetString(dr.GetOrdinal("EMPRESANOM"));
                    ob.NombreProyecto = dr.IsDBNull(dr.GetOrdinal("PROYNOMBRE")) ? null : dr.GetString(dr.GetOrdinal("PROYNOMBRE"));
                    ob.DetalleProyecto = dr.IsDBNull(dr.GetOrdinal("PROYDESCRIPCION")) ? null : dr.GetString(dr.GetOrdinal("PROYDESCRIPCION"));
                    ob.TipoProyecto = dr.IsDBNull(dr.GetOrdinal("TIPONOMBRE")) ? null : dr.GetString(dr.GetOrdinal("TIPONOMBRE"));
                    ob.SubTipoProyecto = dr.IsDBNull(dr.GetOrdinal("TIPOFINOMBRE")) ? null : dr.GetString(dr.GetOrdinal("TIPOFINOMBRE"));
                    ob.Confidencial = dr.IsDBNull(dr.GetOrdinal("PROYCONFIDENCIAL")) ? null : dr.GetString(dr.GetOrdinal("PROYCONFIDENCIAL"));
                    listob.Add(ob);
                }
            }
            return listob;
        }



    }
}
