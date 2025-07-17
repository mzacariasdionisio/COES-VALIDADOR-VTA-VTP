using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.Eventos
{
    public class ResponsableAppServicio : AppServicioBase
    {

        /// <summary>
        /// Obtener Listao de Directores
        /// </summary>
        /// <returns></returns>
        public List<SiDirectorioDTO> ObtenerListaDirectorio()
        {
            return FactorySic.ObtenerEventoDao().ObtenerListadoDirectores();
        }

        /// <summary>
        /// Obtener Listao de Directores
        /// </summary>
        /// <returns></returns>
        public List<SiResponsableDTO> ObtenerListaResponsables(string Estado, string NombreApellido)
        {
            return FactorySic.ObtenerEventoDao().ObtenerListadoResponsables(Estado, NombreApellido);
        }

        /// <summary>
        /// Grabar Nuevo Responsable
        /// </summary>
        /// <returns></returns>
        public int GuardarNuevoResponsable(SiResponsableDTO responsableDTO)
        {
            return FactorySic.ObtenerEventoDao().GuardarNuevoResponsable(responsableDTO);
        }

        /// <summary>
        /// Grabar Editar Responsable
        /// </summary>
        /// <returns></returns>
        public int GuardarEditarResponsable(SiResponsableDTO responsableDTO)
        {
            return FactorySic.ObtenerEventoDao().GuardarEditarResponsable(responsableDTO);
        }

        /// <summary>
        /// Eliminar un Responsable
        /// </summary>
        /// <returns></returns>
        public bool EliminarResponsable(int CodigoResponsable,string path,string nombreArchivo)
        {
            bool eliminado= FactorySic.ObtenerEventoDao().EliminarResponsable(CodigoResponsable);

            //if (eliminado)
            //{
            //    FileServer.DeleteBlob(nombreArchivo, path);
            //}

            return eliminado;
        }

        /// <summary>
        /// Obtener un Responsable por su código
        /// </summary>
        /// <returns></returns>
        public SiResponsableDTO ObtenerResponsable(int CodigoResponsable)
        {
            return FactorySic.ObtenerEventoDao().ObtenerResponsable(CodigoResponsable);
        }

        /// <summary>
        /// Obtener Directores
        /// </summary>
        /// <returns></returns>
        public SiDirectorioDTO ObtenerDirectorio(int dircodi)
        {
            return FactorySic.ObtenerEventoDao().ObtenerDirectorio(dircodi);
        }
    }
}
