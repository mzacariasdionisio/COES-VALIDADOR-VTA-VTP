using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EQ_RELACION
    /// </summary>
    public interface IEqRelacionRepository
    {
        int Save(EqRelacionDTO entity);
        void Update(EqRelacionDTO entity);
        void Delete(int relacioncodi);
        EqRelacionDTO GetById(int relacioncodi);
        List<EqRelacionDTO> List(string fuente);
        List<EqRelacionDTO> GetByCriteria(int idEmpresa, string estado, string fuente);
        List<SiEmpresaDTO> ListarEmpresas(string fente);
        int ObtenerPorEquipo(int equicodi, string fuente);
        List<EqEquipoDTO> ObtenerEquiposRelacion(int idEmpresa);        
        List<EqRelacionDTO> ListHidraulico(string fuente);
        List<EqRelacionDTO> ObtenerConfiguracionProceso(string fuente, string famcodis = "-1");
        List<EqRelacionDTO> ObtenerContadorGrupo();
        List<int> ObtenerModosOperacion(DateTime fecha);
        List<int> ObtenerModosOperacionEspeciales();
        List<int> ObtenerUnidadesEnOperacion(DateTime fechaProceso, int idModoOperacion);
        List<EqRelacionDTO> ObtenerModosOperacionLimiteTransmision(DateTime fecha);
        List<EqRelacionDTO> ObtenerCalificacionUnidades(DateTime fecha);
        List<EqRelacionDTO> ObtenerRestriccionOperativa(DateTime fecha);
        int ObtenerModoOperacionUnidad(int grupoCodi);
        List<EqRelacionDTO> ObtenerConfiguracionProcesoDemanda(string fuente, int origenlectura);
        List<EqRelacionDTO> ObtenerPropiedadHidraulicos();
        List<EqRelacionDTO> ObtenerPropiedadHidraulicosCentral();
        List<EqRelacionDTO> ObtenerPropiedadesConfiguracion(int indicador);
        int ObtenerNroUnidades(int grupocodi);

        List<SiEmpresaDTO> ListarEmpresasReservaRotante();
        int ObtenerPorEquipoReservaRotante(int equicodi);
        List<EqRelacionDTO> GetByCriteriaReservaRotante(int idEmpresa, string estado, int idGrupo);
        int SaveReservaRotante(EqRelacionDTO entity);
        void UpdateReservaRotante(EqRelacionDTO entity);
        List<EqRelacionDTO> ObtenerConfiguracionReservaRotante();
        List<EqRelacionDTO> ObtenerListadoReservaRotante(int idEmpresa, string estado);

        #region Mejoras CMgN

        List<EqRelacionDTO> ObtenerUnidadComparativoCM();

        #endregion
    }
}
