using System.ComponentModel.DataAnnotations;
using COES.Dominio.DTO.Sic;

namespace COES.MVC.Intranet.Areas.IntercambioOsinergmin.Models.Maestros
{
    public class DetalleEntidadModel
    {
        [Display(Name = "Código")]
        public string Codigo { get; set; }
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }
        [Display(Name = "Código Osinergmin")]
        [StringLength(6, ErrorMessage = "El resumen breve no puede tener más de 6 caracteres.")]
        [Required(ErrorMessage = "El código de Osinergmin es requerido.")]
        public string CodigoOsinergmin { get; set; }
        [Display(Name = "Entidad")]
        public string EntidadDescripcion { get; set; }
        public string Estado { get; set; }
        public int EntidadCodigo { get; set; }

        /// <summary>
        /// Segun el detalle DTO, obtiene el viewmodel equivalente
        /// </summary>
        /// <param name="entidad">Entidad a transformar</param>
        /// <returns></returns>
        public static DetalleEntidadModel Create(EntidadListadoDTO entidad)
        {
            return new DetalleEntidadModel
            {
                Descripcion = entidad.Descripcion,
                EntidadCodigo = entidad.EntidadCodigo,
                Codigo = entidad.Codigo,
                CodigoOsinergmin = entidad.CodigoOsinergmin,
                EntidadDescripcion = entidad.EntidadDescricion,
                Estado = entidad.Estado
            };
        }

        public static EntidadListadoDTO ToDTO(DetalleEntidadModel model)
        {
            return new EntidadListadoDTO
            {
                Descripcion = model.Descripcion,
                Codigo = model.Codigo,
                CodigoOsinergmin = model.CodigoOsinergmin,
                EntidadCodigo = model.EntidadCodigo,
                EntidadDescricion = model.EntidadDescripcion,
                Estado = model.Estado
            };
        }
    }
}