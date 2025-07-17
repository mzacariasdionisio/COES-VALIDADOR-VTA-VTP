using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a las formulas para calculos
    /// </summary>
    public interface IEprCalculosRepository
    {
       
        List<EprCalculosDTO> ListCalculosFormulasLinea(EprEquipoDTO equipo, int flgOrigen);
        List<EprCalculosDTO> ListCalculosFormulasReactor(EprEquipoDTO equipo, int flgOrigen);
        List<EprCalculosDTO> ListCalculosFormulasCelda(EprEquipoDTO equipo, int flgOrigen);

        List<EprCalculosDTO> ListCalculosTransformadorDosDevanados(EprEquipoDTO equipo);
        List<EprCalculosDTO> ListCalculosTransformadorTresDevanados(EprEquipoDTO equipo);
        List<EprCalculosDTO> ListCalculosTransformadorCuatroDevanados(EprEquipoDTO equipo);

        double? EvaluarCeldaPosicionNucleo(int equicodi);
        double? EvaluarCeldaPickUp(int equicodi);
        double? EvaluarPropiedadEquipo(int equicodi, string tipoPropiedad);
        double? EvaluarTensionEquipo(int equicodi);

        List<EprPropCatalogoDataDTO> ListFunciones();
        List<EqPropiedadDTO> ListPropiedades(int famcodi);

        List<EprCalculosDTO> ListValidarFormulas(int famcodi, string formula);

        List<EprCalculosDTO> ListCalculosFormulasLineaMasivo(string listaCodigosEquipo, int famcodi);

        List<EprCalculosDTO> ListCalculosFormulasReactorMasivo(string listaCodigosEquipo, int famcodi);

        List<EprCalculosDTO> ListCalculosFormulasCeldaMasivo(string listaCodigosEquipo, int famcodi);

        List<EprCalculosDTO> ListCalculosFormulasTransformadoDosDevanadosMasivo(string listaCodigosEquipo, int famcodi);

        List<EprCalculosDTO> ListCalculosFormulasTransformadoTresDevanadosMasivo(string listaCodigosEquipo, int famcodi);

        List<EprCalculosDTO> ListCalculosFormulasTransformadoCuatroDevanadosMasivo(string listaCodigosEquipo, int famcodi);

        List<EprCalculosDTO> ListCalculosFormulasInterruptor(EprEquipoDTO equipo);

    }
}
