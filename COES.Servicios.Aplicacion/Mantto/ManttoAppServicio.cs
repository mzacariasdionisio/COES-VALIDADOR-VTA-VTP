using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.Mantto
{
    public class ManttoAppServicio : AppServicioBase
    {
        /// <summary>
        /// Permite buscar equipos segun los criterios especificados
        /// </summary>
        /// <param name="idEvento"></param>
        /// <returns></returns>
        public List<ManRegistroDTO> BuscarManRegistroEvento(int idEvento, int nroPagina, int nroFilas)
        {
            return FactorySic.GetManRegistroRepository().BuscarManRegistro(idEvento, nroPagina, nroFilas);
        }

        /// <summary>
        /// Permite obtener el nro de items del resultado de la busqueda de equipos
        /// </summary>
        /// <param name="idEvento"></param>
        /// <returns></returns>
        public int ObtenerNroFilasBusquedaEquipo(int? idEvento)
        {
            return FactorySic.GetManRegistroRepository().ObtenerNroFilasManRegistroxTipo(idEvento);
        }

        public List<ManManttoDTO> ListManttoPorEquipoFecha(int equicodi, DateTime fecha)
        {
            return FactorySic.GetManManttoRepository().ListManttoPorEquipoFecha(equicodi, fecha);
        }
        public List<ManManttoDTO> ConsultarManttoURS(int iGrupoCodi, DateTime fechaRegistro)
        {
            var mantenimientosResult = new List<ManManttoDTO>();
            var oGrupo = Factory.FactorySic.GetPrGrupoRepository().GetById(iGrupoCodi);
            List<EqEquipoDTO> lsEquipos = new List<EqEquipoDTO>();
            lsEquipos = Factory.FactorySic.GetEqEquipoRepository().ListarGeneradoresTermicosPorModoOperacion(iGrupoCodi);
            foreach (var oEquipo in lsEquipos)
            {
                var lsMantto = FactorySic.GetManManttoRepository().ListManttoPorEquipoFecha(oEquipo.Equicodi, fechaRegistro);
                if (lsMantto != null && lsMantto.Count > 0)
                    mantenimientosResult.AddRange(lsMantto);
            }
            return mantenimientosResult;
        }
    }
}
