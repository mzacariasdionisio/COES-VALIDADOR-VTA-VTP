using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EPR_EQUIPO
    /// </summary>
    public interface IEprEquipoRepository
    {
        int Save(EprEquipoDTO entity);
        void Update(EprEquipoDTO entity);
        void Delete_UpdateAuditoria(EprEquipoDTO entity);
        EprEquipoDTO GetById(int epequicodi);
        List<EprEquipoDTO> ListArbol(int Idzona, string Ubicacion);
        List<EprEquipoDTO> ListCelda(int areacodi);
        string SaveRele(EprEquipoDTO equipo);
        List<EprEquipoDTO> ListEquipoProtGrilla(int equicodi, int nivel, string celda, string rele, int idArea, string nombSubestacion);
        List<EprEquipoDTO> ReporteEquipoProtGrilla(int equicodi, int nivel, string celda, string rele, int idArea, string nombSubestacion);
        List<EprEquipoDTO> ReporteEquipoProtGrillaProyecto(int epproycodi);
        List<EprEquipoDTO> ArchivoZipHistarialCambio(int areacodi, int zonacodi);
        EprEquipoDTO GetByIdEquipoProtec(int equicodi);
        string UpdateRele(EprEquipoDTO equipo);
        EprEquipoDTO GetByIdCelda(int equicodi);
        List<EprEquipoDTO> ListLineaTiempo(int equicodi);
        List<EprEquipoDTO> ListInterruptor(int areacodi);
        List<EprEquipoDTO> ListEquipamientoModificado(int epproycodi);
        EprEquipoDTO GetCantidadEquipoSGOCOESEliminar(int equicodi);
        EprEquipoDTO GetDetalleArbolEquipoProteccion(int equicodi, int nivel);
        List<EprEquipoDTO> ListaReleSincronismo(string codigoId, string codigo, int subestacion, int celda,
            int empresa, int area, string estado);

        List<EprEquipoDTO> ListaBuscarCeldaAcoplamiento(string codigoId, string codigo, int ubicacion,
            int empresa, string estado);

        string RegistrarLinea(EprEquipoDTO equipo);




        #region GESTPROTEC Evaluación
        List<EprEquipoDTO> ListCeldaEvaluacion(string idUbicacion);
        List<EprEquipoDTO> ListBancoEvaluacion();
        EprEquipoDTO GetIdLineaIncluir(int equicodi);
        List<EprEquipoDTO> ListLineaEvaluacionPrincipal(string equicodi, string codigo, string emprcodi, string equiestado, string idsuestacion1, string idsuestacion2, string idarea, string tension);
        #endregion

        List<EprEquipoDTO> ListaBuscarReactor(string codigoId, string codigo, int ubicacion,
            int empresa, string estado);

        List<EprEquipoDTO> ListaBuscarTransformador(int tipo, string codigoId, string codigo, int ubicacion,
            int empresa, string estado);

        List<EqFamiliaDTO> ListaTransformadores();

        List<EprEquipoDTO> ListaTransversalConsultarEquipo(string codigoId);
        EprEquipoDTO ObtenerTransversalHistorialActualizacion(string codigoId);
        List<EprEquipoDTO> ListaTransversalActualizaciones(string codigoId);
        List<EprEquipoDTO> ListaTransversalPropiedadesActualizadas(string codigoId, string proyId);

        string RegistrarReactor(EprEquipoDTO equipo);
        string RegistrarTransformador(EprEquipoDTO equipo);
        List<EprEquipoDTO> ListaReactor(string codigoId, string codigo, int ubicacion,
            int empresa, int area, string estado);

        EprEquipoDTO ObtenerReactorPorId(int codigoId);

        List<EprEquipoDTO> ListaCeldaAcoplamiento(string codigoId, string codigo, int ubicacion,
            int empresa, int area, string estado, string tension);

        EprEquipoDTO ObtenerCeldaAcoplamientoPorId(int codigoId);

        List<EprEquipoDTO> ListaReleSobretension(string codigoId, string codigo, int subestacion, int celda,
            int empresa, int area, string estado);

        List<EprEquipoDTO> ListaReleMandoSincronizado(string codigoId, string codigo, int subestacion, int celda,
            int empresa, int area, string estado);

        List<EprEquipoDTO> ListaReleTorcional(string codigoId, string codigo, int subestacion, int celda,
            int empresa, int area, string estado);

        List<EprEquipoDTO> ListaRelePMU(string codigoId, string codigo, int subestacion, int celda,
            int empresa, int area, string estado);
        List<EprEquipoDTO> ListaTransformador(string codigoId, string codigo, int tipo, int subestacion,
            int empresa, int area, string estado, string tension);

        EprEquipoDTO ObtenerTransformadorPorId(int codigoId);

        string RegistrarCeldaAcoplamiento(EprEquipoDTO equipo);
        EprEquipoDTO ObtenerEquipoPorId(string codigoId);
        List<EprEquipoDTO> ListaInterruptorPorAreacodi(string id);

        EprEquipoDTO ObtenerCabeceraEquipoPorId(int codigoId);
        List<EprEquipoDTO> ListaReporteLimiteCapacidad(int revision, string descripcion);

        EprEquipoDTO ObtenerReporteLimiteCapacidadPorId(int id);

        int GuardarReporteLimiteCapacidad(EprEquipoDTO entity);
        void ActualizarReporteLimiteCapacidad(EprEquipoDTO entity);
        void EliminarReporteLimiteCapacidad(EprEquipoDTO entity);
        string ExcluirEquipoProtecciones(EprEquipoDTO equipo);
        void AgregarEliminarArchivoReporteLimiteCapacidad(EprEquipoDTO entity);

        EprEquipoDTO ObtenerDatoCelda(int equicodi);

        string ObtenerFechaReportePorId(int id);

        #region Exportacion Datos Reles

        List<EprEquipoDTO> ListaExportarReleSincronismo(string codigoId, string codigo, int subestacion, int celda,
          int empresa, int area, string estado);

        List<EprEquipoDTO> ListaExportarReleSobreTension(string codigoId, string codigo, int subestacion, int celda,
         int empresa, int area, string estado);

        List<EprEquipoDTO> ListaExportarReleMandoSincronizado(string codigoId, string codigo, int subestacion, int celda,
        int empresa, int area, string estado);

        List<EprEquipoDTO> ListaExportarReleTorsional(string codigoId, string codigo, int subestacion, int celda,
       int empresa, int area, string estado);

        List<EprEquipoDTO> ListaExportarRelePmu(string codigoId, string codigo, int subestacion, int celda,
       int empresa, int area, string estado);


        #endregion

    }
}
