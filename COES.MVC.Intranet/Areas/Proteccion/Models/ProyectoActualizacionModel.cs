using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Models;
namespace COES.MVC.Intranet.Areas.Proteccion.Models
{
    public class ProyectoActualizacionModel
    {
        public List<SiAreaDTO> ListaArea { get; set; }
        public string Nombre { get; set; }
        public string FechaDesde { get; set; }
        public string FechaHasta { get; set; }
        public List<EstadoModel> ListaEstadoFlag { get; set; }
        public List<List<string>> listaProyectos { get; set; }
        public List<SiEmpresaDTO> listaTitular { get; set; }
        public int idArea { get; set; }
        public int idFamilia { get; set; }

        public List<SiTipoempresaDTO> listaTipoEmpresa { get; set; }

        public List<EprPropCatalogoDataDTO> listaEstado { get; set; }

    }

    public class ListadoProyectoModel
    {
        public List<EprProyectoActEqpDTO> ListaProyecto { get; set; }
    }

    public class ProyectoActualizacionEditarModel
    {
        public List<SiAreaDTO> ListaArea { get; set; }
        public List<EstadoModel> ListaEstadoFlag { get; set; }
        public List<SiEmpresaDTO> listaTitular { get; set; }
        public int idArea { get; set; }
        public int idFamilia { get; set; }
        public int Codigo { get; set; }
        public string  CodigoNemoTecnico { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string FechaRegistro { get; set; }

        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }

        public string Eppproyflgtieneeo { get; set; }

    }

    public class EquipamientoModificadoModel
    {
        public string Codigo { get; set; }
        public string MemoTecnico { get; set; }
        public string Motivo { get; set; }
        public string FechaModificacion { get; set; }
        public string FechaCreacion { get; set; }
    }

    public class ListadoEqModificadoModel
    {
        public List<EprEquipoDTO> ListaEqModificado { get; set; }
    }

}