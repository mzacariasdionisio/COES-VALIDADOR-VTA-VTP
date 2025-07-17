using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.Mantto;
using COES.Servicios.Distribuidos.Contratos;

namespace COES.Servicios.Distribuidos.Servicios
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.PerSession)]
    public class UrsServicio : IUrsServicio
    {
        DespachoAppServicio servicio = new DespachoAppServicio();
        ManttoAppServicio servicioMantto = new ManttoAppServicio();
        EquipamientoAppServicio servicioEquipamiento = new EquipamientoAppServicio();

        public List<Dominio.DTO.Sic.PrGrupoConceptoDato> ListarParametrosGenerales()
        {
            return servicio.ObtenerParametrosGeneralesUrs();
        }

        public List<Dominio.DTO.Sic.PrGrupoConceptoDato> ObtenerDatosMO(int iGrupoCodi, DateTime fechaRegistro)
        {
            return servicio.ObtenerDatosMO_URS(iGrupoCodi, fechaRegistro);
        }


        public List<Dominio.DTO.Sic.ManManttoDTO> ConsultarManttoURS(int iGrupoCodi, DateTime fechaRegistro)
        {
            var mantenimientosResult = new List<Dominio.DTO.Sic.ManManttoDTO>();
            var lsEquipos = servicioEquipamiento.ListarGeneradoresTermicosPorModoOperacion(iGrupoCodi);
            foreach (var oEquipo in lsEquipos)
            {
                var lsMantto = servicioMantto.ListManttoPorEquipoFecha(oEquipo.Equicodi, fechaRegistro);
                if(lsMantto!=null && lsMantto.Count>0)
                    mantenimientosResult.AddRange(lsMantto);
            }
            return mantenimientosResult;
        }
    }
}