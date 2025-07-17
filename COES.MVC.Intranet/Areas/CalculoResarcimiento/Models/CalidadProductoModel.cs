using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.CalculoResarcimiento.Models
{
    public class CalidadProductoModel
    {
        public int Anio { get; set; }
        public int Mes { get; set; }
        public List<ReEventoProductoDTO> ListaEventos { get; set; }
        public ReEventoProductoDTO Entidad { get; set; }
        public List<ReEmpresaDTO> ListaEmpresa { get; set; }
        public List<ReEmpresaDTO> ListaSuministradores { get; set; }
        public List<ReEmpresaDTO> ListaSuministrador { get; set; }
        public List<int> ListaAnio { get; set; }

        public bool TienePermisoAdmin { get; set; }
        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }
        public SiCorreoDTO Correo { get; set; }

        public int AnioEvento { get; set; }
        public int MesEvento { get; set; }
        public string FechaInicial { get; set; }
        public string FechaFinal { get; set; }
        public string PuntoEntrega { get; set; }
        public decimal Tension { get; set; }
        public int Empresa1 { get; set; }
        public int? Empresa2 { get; set; }
        public int? Empresa3 { get; set; }
        public decimal Porcentaje1 { get; set; }
        public decimal? Porcentaje2 { get; set; }
        public decimal? Porcentaje3 { get; set; }
        public string Comentario { get; set; }
        public string Acceso { get; set; }
        public int CodigoEvento { get; set; }
        public string Empresas { get; set; }
        public bool Grabar { get; set; }

    }
}
