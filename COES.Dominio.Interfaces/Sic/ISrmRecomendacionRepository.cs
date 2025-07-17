using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SRM_RECOMENDACION
    /// </summary>
    public interface ISrmRecomendacionRepository
    {
        int Save(SrmRecomendacionDTO entity);
        void Update(SrmRecomendacionDTO entity);
        void Delete(int srmreccodi);
        SrmRecomendacionDTO GetById(int srmreccodi);
        List<SrmRecomendacionDTO> List();
        List<SrmRecomendacionDTO> GetByCriteria();
        int SaveSrmRecomendacionId(SrmRecomendacionDTO entity);
        List<SrmRecomendacionDTO> BuscarOperaciones(int evenCodi,int equiCodi,int srmcrtcodi,int srmstdcodi,int usercode,DateTime srmrecFecharecomend,DateTime srmrecFechavencim,int nroPage, int pageSize);
        int ObtenerNroFilas(int evenCodi,int equiCodi,int srmcrtcodi,int srmstdcodi,int usercode,DateTime srmrecFecharecomend,DateTime srmrecFechavencim);
        List<SrmRecomendacionDTO> BuscarOperaciones(DateTime fechaInicio, DateTime fechaFin, int famcodi, string equiAbrev, int tipoEmpresa, int emprcodi, string detRecomend, int estado,
            int criticidad, int nroPage, int pageSize);
        int ObtenerNroFilas(DateTime fechaInicio, DateTime fechaFin, int famcodi, string equiAbrev, int tipoEmpresa, int emprcodi, string detRecomend, int estado, int criticidad);
        List<SrmRecomendacionDTO> BuscarOperaciones(DateTime fechaInicio, DateTime fechaFin, int famcodi, string equiAbrev, int tipoEmpresa, int emprcodi, int nroPage, int pageSize);
        int ObtenerNroFilas(DateTime fechaInicio, DateTime fechaFin, int famcodi, string equiAbrev, int tipoEmpresa, int emprcodi);
        List<SrmRecomendacionDTO> BuscarOperaciones(int evenCodi, string activo, int nroPage, int pageSize);
        List<SrmRecomendacionDTO> BuscarOperacionesReporteListado(DateTime fechaInicio, DateTime fechaFin, int famcodi, string equiAbrev, int tipoEmpresa, int emprcodi, int estado,
            int criticidad, string recomendacion, int usercode, int nroPage, int pageSize);
        int ObtenerNroFilas(DateTime fechaInicio, DateTime fechaFin, int famcodi, string equiAbrev, int tipoEmpresa, int emprcodi, int estado,
            int criticidad, string recomendacion, int usercode);
        List<SrmRecomendacionDTO> BuscarOperacionesEmpresaCriticidad(DateTime fechaInicio, DateTime fechaFin, int famcodi, string equiAbrev, int tipoEmpresa, int emprcodi, int estado,
            int criticidad, string recomendacion, int usercode);
        List<SrmRecomendacionDTO> BuscarOperacionesEmpresaEstado(DateTime fechaInicio, DateTime fechaFin, int famcodi, string equiAbrev, int tipoEmpresa, int emprcodi, int estado,
            int criticidad, string recomendacion, int usercode);
        List<SrmRecomendacionDTO> BuscarOperacionesTipoEquipoCriticidad(DateTime fechaInicio, DateTime fechaFin, int famcodi, string equiAbrev, int tipoEmpresa, int emprcodi, int estado,
        int criticidad, string recomendacion, int usercode);
            List<SrmRecomendacionDTO> BuscarOperacionesTipoEquipoEstado(DateTime fechaInicio, DateTime fechaFin, int famcodi, string equiAbrev, int tipoEmpresa, int emprcodi, int estado,
        int criticidad, string recomendacion, int usercode);
            List<SrmRecomendacionDTO> BuscarOperacionesEstado(DateTime fechaInicio, DateTime fechaFin, int famcodi, string equiAbrev, int tipoEmpresa, int emprcodi, int estado,
        int criticidad, string recomendacion, int usercode);
            List<SrmRecomendacionDTO> BuscarOperacionesEstadoCriticidad(DateTime fechaInicio, DateTime fechaFin, int famcodi, string equiAbrev, int tipoEmpresa, int emprcodi, int estado,
        int criticidad, string recomendacion, int usercode);
            List<SrmRecomendacionDTO> BuscarOperacionesCriticidad(DateTime fechaInicio, DateTime fechaFin, int famcodi, string equiAbrev, int tipoEmpresa, int emprcodi, int estado,
        int criticidad, string recomendacion, int usercode);
        int ObtenerNroFilas(int evenCodi, string activo);
        List<SrmRecomendacionDTO> BuscarOperacionesAlarma(DateTime fecha, int reporteDiaVencimiento, int repeticionAlarma);
        int ObtenerNroRecomendacionCtaf(int Afrrec);
        List<SrmRecomendacionDTO> BuscarOperacionesCtaf(DateTime fechaInicio, DateTime fechaFin, int famcodi, string equiAbrev, int tipoEmpresa, int emprcodi, int nroPage, int pageSize);
        List<SrmRecomendacionDTO> ListadoRecomendacionesEventosCtaf(int evencodi);
        SrmRecomendacionDTO GetByIdxAfrrec(int afrrec);
        int ValidaRecomendacionxEventoEstado(int evenCodi, int srmstdcodi);
        int ObtenerRecomendacionEvento(int Evencodi, int Equicodi, int Srmcrtcodi, int Srmstdcodi);
    }
}
