using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Core;
using COES.Servicios.Aplicacion.Intervenciones.Helper;
using System;
using System.Collections.Generic;

namespace COES.MVC.Intranet.Areas.Intervenciones.Models
{
    public class FactorF1F2Model
    {
        public List<GraficoWeb> Graficos { get; set; }

        //permisos
        public bool AccionNuevo { get; set; }
        public bool AccionEditar { get; set; }
        public bool AccionEliminar { get; set; }
        public bool AccionGrabar { get; set; }


        //Almacenar lista de versiones de la tabla IN_FACTOR_VERSION
        public List<InFactorVersionDTO> ListInFactorVersion { get; set; }

        public List<SiEmpresaDTO> ListaEmpresa { get; set; }
        public List<EqAreaDTO> ListaUbicacion { get; set; }
        public List<EqEquipoDTO> ListaEquipo { get; set; }
        public List<InFactorVersionMmayorDTO> ListInFactorVersionMmayor { get; set; }

        //propiedades de fechas
        public string Mes { get; set; }
        public int NumMes { get; set; }
        public int NumAnio { get; set; }
        public int Version { get; set; }
        public string Infverflagfinal { get; set; }
        public int Hoja { get; set; }
        public string VersionDesc { get; set; }

        //manejo de errores
        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }
        public string Reporte { get; set; }


        // -------------------------------------------------------------------------------------
        // Lista para combos.
        // -------------------------------------------------------------------------------------
        public List<SiEmpresaDTO> ListaCboEmpresa { get; set; }
        public string Emprcodi { get; set; }

        //cruzadas
        public int Infvercodi { get; set; }
        public string InfverF1 { get; set; }
        public string InfverF2 { get; set; }
        public string Infverfechaperiodo { get; set; }
        public IntervencionGridExcel GridExcel { get; internal set; }
    }

    public class FactorF1F2MmayorModel
    {
        //permisos
        public bool AccionNuevo { get; set; }
        public bool AccionEditar { get; set; }
        public bool AccionEliminar { get; set; }
        public bool AccionGrabar { get; set; }

        public InFactorVersionMmayorDTO Entidad { get; set; }

        //propiedades de fechas
        public string Mes { get; set; }
        public int Version { get; set; }
        public string VersionDesc { get; set; }

        //manejo de errores
        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }
        public string Reporte { get; set; }
    }
}