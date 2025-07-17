using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso para carga masiva para modulos Proteccion y Evaluacion de GESPROTECT
    /// </summary>
    public interface IEprCargaMasivaRepository
    {
        int Save(EprCargaMasivaDTO entity);

        EprCargaMasivaDTO GetById(int epcamacodi);      

        List<EprCargaMasivaDTO> ListarCargaMasivaFiltro(int tipoUso, string usuario, string fecIni, string fecFin);

        string ValidarProteccionesUsoGeneral(EprCargaMasivaDetalleDTO entity);

        string SaveProteccionesUsoGeneral(EprCargaMasivaDetalleDTO entity);

        string ValidarProteccionesMandoSincronizado(EprCargaMasivaDetalleDTO entity);

        string SaveProteccionesMandoSincronizado(EprCargaMasivaDetalleDTO entity);
        string ValidarProteccionesTorsional(EprCargaMasivaDetalleDTO entity);

        string SaveProteccionesTorsional(EprCargaMasivaDetalleDTO entity);
        string ValidarProteccionesPmu(EprCargaMasivaDetalleDTO entity);

        string SaveProteccionesPmu(EprCargaMasivaDetalleDTO entity);

        #region GESPROTECT - 20250206
        string ValidarCargaMasivaLinea(EprCargaMasivaLineaDTO entity);

        string SaveCargaMasivaLinea(EprCargaMasivaLineaDTO entity);


        string ValidarCargaMasivaReactor(EprCargaMasivaLineaDTO entity);

        string SaveCargaMasivaReactor(EprCargaMasivaLineaDTO entity);

        string ValidarCargaMasivaCeldaAcoplamiento(EprCargaMasivaCeldaAcoplamientoDTO entity);

        string SaveCargaMasivaCeldaAcoplamiento(EprCargaMasivaCeldaAcoplamientoDTO entity);

        string ValidarCargaMasivaTransformador(EprCargaMasivaTransformadorDTO entity);

        string SaveCargaMasivaTransformador(EprCargaMasivaTransformadorDTO entity);

        #endregion

        #region GESPROTECT Exportacion Datos

        List<EprEquipoReporteDTO> ListLineaEvaluacionReporte(string equicodi, string codigo, string emprcodi, string equiestado,
            string idsuestacion1, string idsuestacion2, string idarea, string idAreaReporte, string tension);

        List<EprEquipoReactorReporteDTO> ListReactorEvaluacionReporte(string equicodi, string codigo, string emprcodi, string equiestado,
          string idsubestacion, string idarea, string idAreaReporte);

        List<EprEquipoCeldaAcoplamientoReporteDTO> ListCeldaAcoplamientoReporte(string equicodi, string codigo, string emprcodi, string equiestado,
         string idsubestacion, string idarea, string idAreaReporte, string tension);

        List<EprEquipoTransformadoresReporteDTO> ListTransformadoresEvaluacionReporte(string equicodi, string codigo, string emprcodi, string equiestado,
         string tipo, string idsubestacion, string idarea, string idAreaReporte, string tension);
        #endregion
    }
}
