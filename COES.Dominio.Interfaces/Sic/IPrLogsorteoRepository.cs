using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PR_LOGSORTEO
    /// </summary>
    public interface IPrLogsorteoRepository
    {
        void Update(PrLogsorteoDTO entity);
        void Delete(DateTime logfecha);
        PrLogsorteoDTO GetById(DateTime logfecha);
        List<PrLogsorteoDTO> List();
        List<PrLogsorteoDTO> GetByCriteria();        
        List<PrLogsorteoDTO> ObtenerSituacionUnidades(DateTime fechaInicio, DateTime fechaFin);
        List<PrLogsorteoDTO> ObtenerMantenimientos(string clase, int idClase, DateTime fechaInicio, DateTime fechaFin);

        int ConteoCorreoTipo(string Tipo, DateTime Fecha);
        int ConteoBalotaNegra(DateTime Fecha);
        string EquipoPrueba(DateTime Fecha);
        int EquicodiPrueba(DateTime Fecha);

      
        #region MigracionSGOCOES-GrupoB
        List<PrLogsorteoDTO> ListaLogSorteo(DateTime fecIni);
        #endregion

        #region FIT SGOCOES func A
        int Save(PrLogsorteoDTO entity);
        List<PrLogsorteoDTO> eq_equipo();
        List<PrLogsorteoDTO> eq_central();
        List<PrLogsorteoDTO> eve_mantto();
        List<PrLogsorteoDTO> eve_indisponibilidad();
        List<PrLogsorteoDTO> eve_horaoperacion();
        List<PrLogsorteoDTO> eve_pruebaunidad();
        List<Prequipos_validosDTO> equipos_validos(int i_codigo);
        List<Prequipos_validosDTO> eve_mantto_calderos(int i_codigo, DateTime Today, DateTime tomorrow, DateTime mediodia);
        int TotalConteoTipo(string tipo, DateTime fecha);
        int InsertPrSorteo(int equicodi, DateTime fecha, string prueba, int emprcodi);
        bool DeleteEquipo(DateTime logfecha);
        List<PrLogsorteoDTO> GetByCriteria(DateTime fecha);
        int TotalConteoTipoXEQ(DateTime Fecha);
        int DiasFaltantes(DateTime Fecha);
        #endregion

    }
}
