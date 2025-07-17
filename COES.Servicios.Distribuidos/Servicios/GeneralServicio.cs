using System;
using System.Collections.Generic;
using System.ServiceModel;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Distribuidos.Contratos;
using COES.Servicios.Aplicacion.Demo;

namespace COES.Servicios.Distribuidos.Servicios
{
    /// <summary>
    /// Implementa los contratos de los servicios
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.PerSession)]
    public class GeneralServicio : IGeneralServicio
    {        
        //public List<EmpresaDTO> BuscarEmpresas(string nombre) 
        //{
        //    return (new GeneralAppServicio()).BuscarEmpresas(nombre);
        //}

        //public EmpresaDTO GetByIdEmpresa(int idEmpresa)
        //{
        //    return (new GeneralAppServicio()).GetByIdEmpresa(idEmpresa);
        //}

        public List<SiPruebaDTO> BuscarPorNombre(string nombre)
        {
            return (new DemoAppServicio()).BuscarPorNombre(nombre);
        }

        public List<DocDiaEspDTO> ListDocDiaEsps()
        {
            return (new GeneralAppServicio()).ListDocDiaEsps();
        }

        public bool EsFeriado(DateTime adt_fecha)
        {
            return (new GeneralAppServicio()).EsFeriado(adt_fecha);
        }
        public CpFuentegamsDTO GetByIdCpFuentesgams(int indgsm)
        {
            return (new Aplicacion.CortoPlazo.McpAppServicio()).GetByIdCpFuentesgams(indgsm);
        }

        public int EnviarNotificacionTramiteVirtual(int idExpediente, int tipo)
        {
            return (new TramiteVirtualAppServicio()).EnviarNotificacionPortalTramite(idExpediente, tipo);
        }



    }
}