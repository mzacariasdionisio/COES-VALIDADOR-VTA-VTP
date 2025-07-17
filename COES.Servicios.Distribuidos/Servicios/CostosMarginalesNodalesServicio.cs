using System;
using System.Collections.Generic;
using System.ServiceModel;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Distribuidos.Contratos;
using COES.Servicios.Aplicacion.OperacionesVarias;
using System.Globalization;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using COES.Servicios.Distribuidos.Models;
using COES.Servicios.Aplicacion.Eventos.Helper;
using COES.Servicios.Aplicacion.CortoPlazo;

namespace COES.Servicios.Distribuidos.Servicios
{
    /// <summary>
    /// Implementa los contratos de los servicios
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.PerSession)]
    public class CostosMarginalesNodalesServicio : ICostosMarginalesNodales
    {
        string FormatoFecha = "dd/MM/yyyy";
        CortoPlazoAppServicio servicio = new CortoPlazoAppServicio();

        public CostoMarginalNodalModel ObtenerRegistros(int correlativo)
        {
            CostoMarginalNodalModel model = new CostoMarginalNodalModel();
            model.Listado = this.servicio.ObtenerDatosCostoMarginalCorrida(correlativo);
            model.ListadoGeneracionEms = servicio.ObtenerGeneracionEmsPorCorrelativo(correlativo);
            model.ListaRestricciones = servicio.ObtenerRestriccionesPorCorrida(correlativo);
            model.VersionPrograma = servicio.ObtenerVersionPrograma(correlativo);
            return model;
        }

        public CostoMarginalNodalModel ObetenerListadoEjecuciones(string fecha)
        {
            CostoMarginalNodalModel model = new CostoMarginalNodalModel();
            DateTime fechaConsulta = DateTime.ParseExact(fecha, this.FormatoFecha, CultureInfo.InvariantCulture);
            model.Listado = this.servicio.ObtenerResultadoCostosMarginales(fechaConsulta, 0);
            return model;
        }

        public List<CpTopologiaDTO> ObtenerEscenariosPorDia(string fecha)
        {

            DateTime fechaProceso = DateTime.ParseExact(fecha, this.FormatoFecha, CultureInfo.InvariantCulture);
            return (new McpAppServicio()).ObtenerEscencariosPorDia(fechaProceso);
        }

    }


}
