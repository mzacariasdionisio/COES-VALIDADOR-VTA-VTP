using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.YupanaContinuo.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.YupanaContinuo.Models
{
    public class CondicionTermicaModel
    {
        public string Fecha { get; set; }
        public List<GenericoDTO> ListaHora { get; set; }

        public bool TienePermisoAdmin { get; set; }

        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }
        public string Reporte { get; set; }
        public string HtmlList { get; set; }

        #region Periodo Forzado
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public List<EqEquipoDTO> ListaCentrales { get; set; }

        public int IdPos { get; set; }
        public int IdEmpresa { get; set; }
        public int IdTipoCentral { get; set; }
        public string ActFiltroCtral { get; set; }
        public int IdCentralSelect { get; set; }
        public string HoraIni { get; set; }
        public string HoraFin { get; set; }
        public string EtiquetaFiltro { get; set; }

        public int TipoVistaCoordinador { get; set; }
        public int ValorAlertaXSI { get { return ConstantesYupanaContinuo.AlertaXSI; } }
        public string ValorParamIdEmpresaSeleccione { get { return ConstantesYupanaContinuo.ParamEmpresaSeleccione; } }
        public string ValorParamIdCentralSeleccione { get { return ConstantesYupanaContinuo.ParamCentralSeleccione; } }

        public bool AccionNuevo { get; internal set; }
        public int IdEquipo { get; internal set; }
        public int IdEquipoOrIdModo { get; internal set; }
        public List<SelectListItem> SelectListItem { get; internal set; }
        public List<SelectListItem> SelectListItem2 { get; internal set; }
        public int IdGrupoModo { get; internal set; }
        public string ValorParamIdModoSeleccione { get { return ConstantesYupanaContinuo.ParamModoSeleccione; } }

        public CpForzadoDetDTO ForzadoDet { get; internal set; }
        public CpForzadoCabDTO ForzadoCab { get; internal set; }

        #endregion

    }
}