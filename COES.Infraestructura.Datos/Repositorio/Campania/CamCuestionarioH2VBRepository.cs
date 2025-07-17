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
    public class CamCuestionarioH2VBRepository : RepositoryBase, ICamCuestionarioH2VBRepository
    {
        public CamCuestionarioH2VBRepository(string strConn) : base(strConn) { }

        CamCuestionarioH2VBHelper Helper = new CamCuestionarioH2VBHelper();

        public List<CuestionarioH2VBDTO> GetCuestionarioH2VBCodi(int proyCodi)
        {
            List<CuestionarioH2VBDTO> cuestionarios = new List<CuestionarioH2VBDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetCuestionarioH2VBCodi);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, proyCodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CuestionarioH2VBDTO cuestionario = new CuestionarioH2VBDTO();
                    cuestionario.H2vbCodi = !dr.IsDBNull(dr.GetOrdinal("H2VBCODI")) ? dr.GetInt32(dr.GetOrdinal("H2VBCODI")) : 0;
                    cuestionario.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    cuestionario.NombreUnidad = !dr.IsDBNull(dr.GetOrdinal("NOMBREUNIDAD")) ? dr.GetString(dr.GetOrdinal("NOMBREUNIDAD")) : "";
                    cuestionario.Distrito = !dr.IsDBNull(dr.GetOrdinal("DISTRITO")) ? dr.GetString(dr.GetOrdinal("DISTRITO")) : "";
                    cuestionario.Propietario = !dr.IsDBNull(dr.GetOrdinal("PROPIETARIO")) ? dr.GetString(dr.GetOrdinal("PROPIETARIO")) : "";
                    cuestionario.SocioOperador = !dr.IsDBNull(dr.GetOrdinal("SOCIOOPERADOR")) ? dr.GetString(dr.GetOrdinal("SOCIOOPERADOR")) : "";
                    cuestionario.SocioInversionista = !dr.IsDBNull(dr.GetOrdinal("SOCIOINVERSIONISTA")) ? dr.GetString(dr.GetOrdinal("SOCIOINVERSIONISTA")) : "";
                    cuestionario.IncluidoPlanTrans = !dr.IsDBNull(dr.GetOrdinal("INCLUIDOPLANTRANS")) ? dr.GetString(dr.GetOrdinal("INCLUIDOPLANTRANS")) : "";
                    cuestionario.ConcesionTemporal = !dr.IsDBNull(dr.GetOrdinal("CONCESIONTEMPORAL")) ? dr.GetString(dr.GetOrdinal("CONCESIONTEMPORAL")) : "";
                    cuestionario.TipoElectrolizador = !dr.IsDBNull(dr.GetOrdinal("TIPOELECTROLIZADOR")) ? dr.GetString(dr.GetOrdinal("TIPOELECTROLIZADOR")) : "";
                    cuestionario.FechaConcesionTemporal = !dr.IsDBNull(dr.GetOrdinal("FECHACONCESIONTEMPORAL")) ? dr.GetDateTime(dr.GetOrdinal("FECHACONCESIONTEMPORAL")) : (DateTime?)null;
                    cuestionario.FechaTituloHabilitante = !dr.IsDBNull(dr.GetOrdinal("FECHATITULOHABILITANTE")) ? dr.GetDateTime(dr.GetOrdinal("FECHATITULOHABILITANTE")) : (DateTime?)null;
                    cuestionario.Perfil = !dr.IsDBNull(dr.GetOrdinal("PERFIL")) ? dr.GetString(dr.GetOrdinal("PERFIL")) : "";
                    cuestionario.Prefactibilidad = !dr.IsDBNull(dr.GetOrdinal("PREFACTIBILIDAD")) ? dr.GetString(dr.GetOrdinal("PREFACTIBILIDAD")) : "";
                    cuestionario.Factibilidad = !dr.IsDBNull(dr.GetOrdinal("FACTIBILIDAD")) ? dr.GetString(dr.GetOrdinal("FACTIBILIDAD")) : "";
                    cuestionario.EstudioDefinitivo = !dr.IsDBNull(dr.GetOrdinal("ESTUDIODEFINITIVO")) ? dr.GetString(dr.GetOrdinal("ESTUDIODEFINITIVO")) : "";
                    cuestionario.EIA = !dr.IsDBNull(dr.GetOrdinal("EIA")) ? dr.GetString(dr.GetOrdinal("EIA")) : "";
                    cuestionario.FechaInicioConstruccion = !dr.IsDBNull(dr.GetOrdinal("FECHAINICIOCONSTRUCCION")) ? dr.GetString(dr.GetOrdinal("FECHAINICIOCONSTRUCCION")) : "";
                    cuestionario.PeriodoConstruccion = !dr.IsDBNull(dr.GetOrdinal("PERIODOCONSTRUCCION")) ? dr.GetString(dr.GetOrdinal("PERIODOCONSTRUCCION")) : "";
                    cuestionario.FechaOperacionComercial = !dr.IsDBNull(dr.GetOrdinal("FECHAOPERACIONCOMERCIAL")) ? dr.GetString(dr.GetOrdinal("FECHAOPERACIONCOMERCIAL")) : "";
                    cuestionario.PotenciaInstalada = !dr.IsDBNull(dr.GetOrdinal("POTENCIAINSTALADA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("POTENCIAINSTALADA")) : null;

                    cuestionario.RecursoUsado = !dr.IsDBNull(dr.GetOrdinal("RECURSOUSADO")) ? dr.GetString(dr.GetOrdinal("RECURSOUSADO")) : "";
                    cuestionario.Tecnologia = !dr.IsDBNull(dr.GetOrdinal("TECNOLOGIA")) ? dr.GetString(dr.GetOrdinal("TECNOLOGIA")) : "";
                    cuestionario.OtroTecnologia = !dr.IsDBNull(dr.GetOrdinal("OTROTECNOLOGIA")) ? dr.GetString(dr.GetOrdinal("OTROTECNOLOGIA")) : "";
                    cuestionario.BarraConexion = !dr.IsDBNull(dr.GetOrdinal("BARRACONEXION")) ? dr.GetString(dr.GetOrdinal("BARRACONEXION")) : "";
                    cuestionario.NivelTension = !dr.IsDBNull(dr.GetOrdinal("NIVELTENSION")) ? dr.GetString(dr.GetOrdinal("NIVELTENSION")) : "";
                    cuestionario.Comentarios = !dr.IsDBNull(dr.GetOrdinal("COMENTARIOS")) ? dr.GetString(dr.GetOrdinal("COMENTARIOS")) : "";
                    cuestionario.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : "";
                    cuestionario.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    cuestionario.UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : "";
                    cuestionario.FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    cuestionario.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : "";


                    cuestionarios.Add(cuestionario);
                }
            }
            return cuestionarios;
        }

        public bool SaveCuestionarioH2VB(CuestionarioH2VBDTO cuestionario)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveCuestionarioH2VB);
            dbProvider.AddInParameter(dbCommand, "H2VBCODI", DbType.Int32, cuestionario.H2vbCodi);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, cuestionario.ProyCodi);
            dbProvider.AddInParameter(dbCommand, "NOMBREUNIDAD", DbType.String, cuestionario.NombreUnidad);
            dbProvider.AddInParameter(dbCommand, "DISTRITO", DbType.String, cuestionario.Distrito);
            dbProvider.AddInParameter(dbCommand, "PROPIETARIO", DbType.String, cuestionario.Propietario);
            dbProvider.AddInParameter(dbCommand, "SOCIOOPERADOR", DbType.String, cuestionario.SocioOperador);
            dbProvider.AddInParameter(dbCommand, "SOCIOINVERSIONISTA", DbType.String, cuestionario.SocioInversionista);
            dbProvider.AddInParameter(dbCommand, "INCLUIDOPLANTRANS", DbType.String, cuestionario.IncluidoPlanTrans);
            dbProvider.AddInParameter(dbCommand, "CONCESIONTEMPORAL", DbType.String, cuestionario.ConcesionTemporal);
            dbProvider.AddInParameter(dbCommand, "TIPOELECTROLIZADOR", DbType.String, cuestionario.TipoElectrolizador);
            dbProvider.AddInParameter(dbCommand, "FECHACONCESIONTEMPORAL", DbType.DateTime, cuestionario.FechaConcesionTemporal);
            dbProvider.AddInParameter(dbCommand, "FECHATITULOHABILITANTE", DbType.DateTime, cuestionario.FechaTituloHabilitante);
            dbProvider.AddInParameter(dbCommand, "PERFIL", DbType.String, cuestionario.Perfil);
            dbProvider.AddInParameter(dbCommand, "PREFACTIBILIDAD", DbType.String, cuestionario.Prefactibilidad);
            dbProvider.AddInParameter(dbCommand, "FACTIBILIDAD", DbType.String, cuestionario.Factibilidad);
            dbProvider.AddInParameter(dbCommand, "ESTUDIODEFINITIVO", DbType.String, cuestionario.EstudioDefinitivo);
            dbProvider.AddInParameter(dbCommand, "EIA", DbType.String, cuestionario.EIA);
            dbProvider.AddInParameter(dbCommand, "FECHAINICIOCONSTRUCCION", DbType.String, cuestionario.FechaInicioConstruccion);
            dbProvider.AddInParameter(dbCommand, "PERIODOCONSTRUCCION", DbType.String, cuestionario.PeriodoConstruccion);
            dbProvider.AddInParameter(dbCommand, "FECHAOPERACIONCOMERCIAL", DbType.String, cuestionario.FechaOperacionComercial);
            dbProvider.AddInParameter(dbCommand, "POTENCIAINSTALADA", DbType.Decimal, cuestionario.PotenciaInstalada);
            dbProvider.AddInParameter(dbCommand, "RECURSOUSADO", DbType.String, cuestionario.RecursoUsado);
            dbProvider.AddInParameter(dbCommand, "TECNOLOGIA", DbType.String, cuestionario.Tecnologia);
            dbProvider.AddInParameter(dbCommand, "OTROTECNOLOGIA", DbType.String, cuestionario.OtroTecnologia);
            dbProvider.AddInParameter(dbCommand, "BARRACONEXION", DbType.String, cuestionario.BarraConexion);
            dbProvider.AddInParameter(dbCommand, "NIVELTENSION", DbType.String, cuestionario.NivelTension);
            dbProvider.AddInParameter(dbCommand, "COMENTARIOS", DbType.String, cuestionario.Comentarios);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, cuestionario.UsuCreacion);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, cuestionario.FecCreacion);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        public bool DeleteCuestionarioH2VBById(int h2vbCodi, string usuario)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlDeleteCuestionarioH2VBById);
            
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(dbCommand, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(dbCommand, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, h2vbCodi);
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        public int GetLastCuestionarioH2VBId()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastCuestionarioH2VBId);
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

        public CuestionarioH2VBDTO GetCuestionarioH2VBById(int h2vbCodi)
        {
            CuestionarioH2VBDTO cuestionario = new CuestionarioH2VBDTO();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetCuestionarioH2VBById);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, h2vbCodi);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    cuestionario = new CuestionarioH2VBDTO();
                    cuestionario.H2vbCodi = !dr.IsDBNull(dr.GetOrdinal("H2VBCODI")) ? dr.GetInt32(dr.GetOrdinal("H2VBCODI")) : 0;
                    cuestionario.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    cuestionario.NombreUnidad = !dr.IsDBNull(dr.GetOrdinal("NOMBREUNIDAD")) ? dr.GetString(dr.GetOrdinal("NOMBREUNIDAD")) : "";
                    cuestionario.Distrito = !dr.IsDBNull(dr.GetOrdinal("DISTRITO")) ? dr.GetString(dr.GetOrdinal("DISTRITO")) : "";
                    cuestionario.Propietario = !dr.IsDBNull(dr.GetOrdinal("PROPIETARIO")) ? dr.GetString(dr.GetOrdinal("PROPIETARIO")) : "";
                    cuestionario.SocioOperador = !dr.IsDBNull(dr.GetOrdinal("SOCIOOPERADOR")) ? dr.GetString(dr.GetOrdinal("SOCIOOPERADOR")) : "";
                    cuestionario.SocioInversionista = !dr.IsDBNull(dr.GetOrdinal("SOCIOINVERSIONISTA")) ? dr.GetString(dr.GetOrdinal("SOCIOINVERSIONISTA")) : "";
                    cuestionario.IncluidoPlanTrans = !dr.IsDBNull(dr.GetOrdinal("INCLUIDOPLANTRANS")) ? dr.GetString(dr.GetOrdinal("INCLUIDOPLANTRANS")) : "";
                    cuestionario.ConcesionTemporal = !dr.IsDBNull(dr.GetOrdinal("CONCESIONTEMPORAL")) ? dr.GetString(dr.GetOrdinal("CONCESIONTEMPORAL")) : "";
                    cuestionario.TipoElectrolizador = !dr.IsDBNull(dr.GetOrdinal("TIPOELECTROLIZADOR")) ? dr.GetString(dr.GetOrdinal("TIPOELECTROLIZADOR")) : "";
                    cuestionario.FechaConcesionTemporal = !dr.IsDBNull(dr.GetOrdinal("FECHACONCESIONTEMPORAL")) ? dr.GetDateTime(dr.GetOrdinal("FECHACONCESIONTEMPORAL")) : (DateTime?)null;
                    cuestionario.FechaTituloHabilitante = !dr.IsDBNull(dr.GetOrdinal("FECHATITULOHABILITANTE")) ? dr.GetDateTime(dr.GetOrdinal("FECHATITULOHABILITANTE")) : (DateTime?)null;
                    cuestionario.Perfil = !dr.IsDBNull(dr.GetOrdinal("PERFIL")) ? dr.GetString(dr.GetOrdinal("PERFIL")) : "";
                    cuestionario.Prefactibilidad = !dr.IsDBNull(dr.GetOrdinal("PREFACTIBILIDAD")) ? dr.GetString(dr.GetOrdinal("PREFACTIBILIDAD")) : "";
                    cuestionario.Factibilidad = !dr.IsDBNull(dr.GetOrdinal("FACTIBILIDAD")) ? dr.GetString(dr.GetOrdinal("FACTIBILIDAD")) : "";
                    cuestionario.EstudioDefinitivo = !dr.IsDBNull(dr.GetOrdinal("ESTUDIODEFINITIVO")) ? dr.GetString(dr.GetOrdinal("ESTUDIODEFINITIVO")) : "";
                    cuestionario.EIA = !dr.IsDBNull(dr.GetOrdinal("EIA")) ? dr.GetString(dr.GetOrdinal("EIA")) : "";
                    cuestionario.FechaInicioConstruccion = !dr.IsDBNull(dr.GetOrdinal("FECHAINICIOCONSTRUCCION")) ? dr.GetString(dr.GetOrdinal("FECHAINICIOCONSTRUCCION")) : "";
                    cuestionario.PeriodoConstruccion = !dr.IsDBNull(dr.GetOrdinal("PERIODOCONSTRUCCION")) ? dr.GetString(dr.GetOrdinal("PERIODOCONSTRUCCION")) : "";
                    cuestionario.FechaOperacionComercial = !dr.IsDBNull(dr.GetOrdinal("FECHAOPERACIONCOMERCIAL")) ? dr.GetString(dr.GetOrdinal("FECHAOPERACIONCOMERCIAL")) : "";
                    cuestionario.PotenciaInstalada = !dr.IsDBNull(dr.GetOrdinal("POTENCIAINSTALADA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("POTENCIAINSTALADA")) : null;
                    cuestionario.RecursoUsado = !dr.IsDBNull(dr.GetOrdinal("RECURSOUSADO")) ? dr.GetString(dr.GetOrdinal("RECURSOUSADO")) : "";
                    cuestionario.Tecnologia = !dr.IsDBNull(dr.GetOrdinal("TECNOLOGIA")) ? dr.GetString(dr.GetOrdinal("TECNOLOGIA")) : "";
                    cuestionario.OtroTecnologia = !dr.IsDBNull(dr.GetOrdinal("OTROTECNOLOGIA")) ? dr.GetString(dr.GetOrdinal("OTROTECNOLOGIA")) : "";
                    cuestionario.BarraConexion = !dr.IsDBNull(dr.GetOrdinal("BARRACONEXION")) ? dr.GetString(dr.GetOrdinal("BARRACONEXION")) : "";
                    cuestionario.NivelTension = !dr.IsDBNull(dr.GetOrdinal("NIVELTENSION")) ? dr.GetString(dr.GetOrdinal("NIVELTENSION")) : "";
                    cuestionario.Comentarios = !dr.IsDBNull(dr.GetOrdinal("COMENTARIOS")) ? dr.GetString(dr.GetOrdinal("COMENTARIOS")) : "";
                    cuestionario.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : "";
                    cuestionario.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    cuestionario.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : "";
                }
            }
            return cuestionario;
        }

        public List<CuestionarioH2VBDTO> GetFormatoH2VBByFilter(string plancodi, string empresa, string estado)
        {
            List<CuestionarioH2VBDTO> oblist = new List<CuestionarioH2VBDTO>();
            string query = $@"
        SELECT CGB.*, TR.EMPRESANOM, TR.PROYNOMBRE, TR.PROYDESCRIPCION, TP.TIPONOMBRE, TF.TIPOFINOMBRE,TR.PROYCONFIDENCIAL  
        FROM cam_cuestionarioh2vb CGB
        INNER JOIN CAM_TRNSMPROYECTO TR ON TR.PROYCODI = CGB.PROYCODI
        INNER JOIN CAM_PLANTRANSMISION PL ON PL.PLANCODI = TR.PLANCODI
        INNER JOIN CAM_TIPOPROYECTO TP ON TP.TIPOCODI = TR.TIPOCODI
        LEFT JOIN CAM_TIPOFICHAPROYECTO TF ON TF.TIPOFICODI = TR.TIPOFICODI
        WHERE TR.PERICODI IN ({plancodi})
          AND PL.CODEMPRESA IN ({empresa})
          AND CGB.IND_DEL = 0
          AND PL.PLANESTADO = '{estado}'
        ORDER BY TR.PERICODI, CGB.PROYCODI, PL.CODEMPRESA, CGB.H2VBCODI ASC";

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CuestionarioH2VBDTO ob = new CuestionarioH2VBDTO();

                    ob.H2vbCodi = !dr.IsDBNull(dr.GetOrdinal("H2VBCODI")) ? dr.GetInt32(dr.GetOrdinal("H2VBCODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.NombreUnidad = !dr.IsDBNull(dr.GetOrdinal("NOMBREUNIDAD")) ? dr.GetString(dr.GetOrdinal("NOMBREUNIDAD")) : "";
                    ob.Distrito = !dr.IsDBNull(dr.GetOrdinal("DISTRITO")) ? dr.GetString(dr.GetOrdinal("DISTRITO")) : "";
                    ob.Propietario = !dr.IsDBNull(dr.GetOrdinal("PROPIETARIO")) ? dr.GetString(dr.GetOrdinal("PROPIETARIO")) : "";
                    ob.SocioOperador = !dr.IsDBNull(dr.GetOrdinal("SOCIOOPERADOR")) ? dr.GetString(dr.GetOrdinal("SOCIOOPERADOR")) : "";
                    ob.SocioInversionista = !dr.IsDBNull(dr.GetOrdinal("SOCIOINVERSIONISTA")) ? dr.GetString(dr.GetOrdinal("SOCIOINVERSIONISTA")) : "";
                    ob.IncluidoPlanTrans = !dr.IsDBNull(dr.GetOrdinal("INCLUIDOPLANTRANS")) ? dr.GetString(dr.GetOrdinal("INCLUIDOPLANTRANS")) : "";
                    ob.ConcesionTemporal = !dr.IsDBNull(dr.GetOrdinal("CONCESIONTEMPORAL")) ? dr.GetString(dr.GetOrdinal("CONCESIONTEMPORAL")) : "";
                    ob.TipoElectrolizador = !dr.IsDBNull(dr.GetOrdinal("TIPOELECTROLIZADOR")) ? dr.GetString(dr.GetOrdinal("TIPOELECTROLIZADOR")) : "";
                    ob.FechaConcesionTemporal = !dr.IsDBNull(dr.GetOrdinal("FECHACONCESIONTEMPORAL")) ? dr.GetDateTime(dr.GetOrdinal("FECHACONCESIONTEMPORAL")) : (DateTime?)null;
                    ob.FechaTituloHabilitante = !dr.IsDBNull(dr.GetOrdinal("FECHATITULOHABILITANTE")) ? dr.GetDateTime(dr.GetOrdinal("FECHATITULOHABILITANTE")) : (DateTime?)null;
                    ob.Perfil = !dr.IsDBNull(dr.GetOrdinal("PERFIL")) ? dr.GetString(dr.GetOrdinal("PERFIL")) : "";
                    ob.Prefactibilidad = !dr.IsDBNull(dr.GetOrdinal("PREFACTIBILIDAD")) ? dr.GetString(dr.GetOrdinal("PREFACTIBILIDAD")) : "";
                    ob.Factibilidad = !dr.IsDBNull(dr.GetOrdinal("FACTIBILIDAD")) ? dr.GetString(dr.GetOrdinal("FACTIBILIDAD")) : "";
                    ob.EstudioDefinitivo = !dr.IsDBNull(dr.GetOrdinal("ESTUDIODEFINITIVO")) ? dr.GetString(dr.GetOrdinal("ESTUDIODEFINITIVO")) : "";
                    ob.EIA = !dr.IsDBNull(dr.GetOrdinal("EIA")) ? dr.GetString(dr.GetOrdinal("EIA")) : "";
                    ob.FechaInicioConstruccion = !dr.IsDBNull(dr.GetOrdinal("FECHAINICIOCONSTRUCCION")) ? dr.GetString(dr.GetOrdinal("FECHAINICIOCONSTRUCCION")) : "";
                    ob.PeriodoConstruccion = !dr.IsDBNull(dr.GetOrdinal("PERIODOCONSTRUCCION")) ? dr.GetString(dr.GetOrdinal("PERIODOCONSTRUCCION")) : "";
                    ob.FechaOperacionComercial = !dr.IsDBNull(dr.GetOrdinal("FECHAOPERACIONCOMERCIAL")) ? dr.GetString(dr.GetOrdinal("FECHAOPERACIONCOMERCIAL")) : "";
                    ob.PotenciaInstalada = !dr.IsDBNull(dr.GetOrdinal("POTENCIAINSTALADA")) ? dr.GetDecimal(dr.GetOrdinal("POTENCIAINSTALADA")) : (Decimal?)null;
                    ob.RecursoUsado = !dr.IsDBNull(dr.GetOrdinal("RECURSOUSADO")) ? dr.GetString(dr.GetOrdinal("RECURSOUSADO")) : "";
                    ob.Tecnologia = !dr.IsDBNull(dr.GetOrdinal("TECNOLOGIA")) ? dr.GetString(dr.GetOrdinal("TECNOLOGIA")) : "";
                    ob.OtroTecnologia = !dr.IsDBNull(dr.GetOrdinal("OTROTECNOLOGIA")) ? dr.GetString(dr.GetOrdinal("OTROTECNOLOGIA")) : "";
                    ob.BarraConexion = !dr.IsDBNull(dr.GetOrdinal("BARRACONEXION")) ? dr.GetString(dr.GetOrdinal("BARRACONEXION")) : "";
                    ob.NivelTension = !dr.IsDBNull(dr.GetOrdinal("NIVELTENSION")) ? dr.GetString(dr.GetOrdinal("NIVELTENSION")) : "";
                    ob.Comentarios = !dr.IsDBNull(dr.GetOrdinal("COMENTARIOS")) ? dr.GetString(dr.GetOrdinal("COMENTARIOS")) : "";
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : "";
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : "";
                    ob.FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : "";
                    ob.Empresa = !dr.IsDBNull(dr.GetOrdinal("EMPRESANOM")) ? dr.GetString(dr.GetOrdinal("EMPRESANOM")) : string.Empty;
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