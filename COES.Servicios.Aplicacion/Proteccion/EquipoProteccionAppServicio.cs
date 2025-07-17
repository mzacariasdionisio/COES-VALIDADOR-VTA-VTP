using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Factory;
using log4net;
using System;
using System.Collections.Generic;

namespace COES.Servicios.Aplicacion.Equipamiento
{
    /// <summary>
    /// Clases con métodos del módulo Equipamiento
    /// </summary>
    public class EquipoProteccionAppServicio : AppServicioBase
    {
        FichaTecnicaAppServicio servFictec = new FichaTecnicaAppServicio();

        /// <summary>
        /// Propiedades de las coordenadas
        /// </summary>
        private const int PropiedadCoorX = 1814;
        private const int PropiedadCoorY = 1815;


        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ProyectoActualizacionAppServicio));


        #region GESPROTEC-20241031
        /// <summary>
        /// Devuelve el listado de equipos COES
        /// </summary>
        /// <returns></returns>
        public List<EprEquipoDTO> ListArbol(int Idzona, string Ubicacion)
        {
            return FactorySic.GetEprEquipoRepository().ListArbol(Idzona, Ubicacion);
        }

        public List<EprAreaDTO> ListSubEstacion()
        {
            return FactorySic.GetEprAreaRepository().ListSubEstacion();
        }

        public List<EprProyectoActEqpDTO> ListProyectoProyectoActualizacion(int equicodi)
        {
            return FactorySic.GetEprProyectoActEqpRepository().ListProyectoProyectoActualizacion(equicodi);
        }

        public List<EprPropCatalogoDataDTO> ListPropCatalogoData(int Eqcatccodi)
        {
            return FactorySic.GetEprPropCatalogoDataRepository().List(Eqcatccodi);
        }

        public List<EprEquipoDTO> ListCelda(int areacodi)
        {
            return FactorySic.GetEprEquipoRepository().ListCelda(areacodi);
        }

        public List<EprEquipoDTO> ListCeldaEvaluacion(string idUbicacion)
        {
            return FactorySic.GetEprEquipoRepository().ListCeldaEvaluacion(idUbicacion);
        }

        public List<EprEquipoDTO> ListBancoEvaluacion()
        {
            return FactorySic.GetEprEquipoRepository().ListBancoEvaluacion();
        }

        public string SaveRele(EprEquipoDTO equipo)
        {
            return FactorySic.GetEprEquipoRepository().SaveRele(equipo);
        }

        public List<EprEquipoDTO> ListEquipoProtGrilla(int equicodi, int nivel, string celda, string rele, int idArea, string nombSubestacion)
        {
            return FactorySic.GetEprEquipoRepository().ListEquipoProtGrilla(equicodi, nivel, celda, rele, idArea, nombSubestacion);
        }

        public List<EprEquipoDTO> ReporteEquipoProtGrilla(int equicodi, int nivel, string celda, string rele, int idArea, string nombSubestacion)
        {
            return FactorySic.GetEprEquipoRepository().ReporteEquipoProtGrilla(equicodi, nivel, celda, rele, idArea, nombSubestacion);
        }

        public List<EprEquipoDTO> ReporteEquipoProtGrillaProyecto(int epproycodi)
        {
            return FactorySic.GetEprEquipoRepository().ReporteEquipoProtGrillaProyecto(epproycodi);
        }

        public List<EprEquipoDTO> ArchivoZipHistarialCambio(int areacodi, int zonacodi)
        {
            return FactorySic.GetEprEquipoRepository().ArchivoZipHistarialCambio(areacodi, zonacodi);
        }
        
        public EprEquipoDTO GetByIdEquipoProtec(int equicodi)
        {
            return FactorySic.GetEprEquipoRepository().GetByIdEquipoProtec(equicodi);
        }
        public string UpdateRele(EprEquipoDTO equipo)
        {
            return FactorySic.GetEprEquipoRepository().UpdateRele(equipo);
        }
        public EprEquipoDTO GetByIdCelda(int equicodi)
        {
            return FactorySic.GetEprEquipoRepository().GetByIdCelda(equicodi);
        }

        public List<EprEquipoDTO> ListLineaTiempo(int equicodi)
        {
            return FactorySic.GetEprEquipoRepository().ListLineaTiempo(equicodi);
        }
        public List<EprEquipoDTO> ListInterruptor(int areacodi)
        {
            return FactorySic.GetEprEquipoRepository().ListInterruptor(areacodi);
        }

        /// <summary>
        /// Nombre del archivo que se guardará en el sistema
        /// </summary>
        /// <param name="equicodi"></param>
        /// <param name="subEstacion"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        public string GetNombreArchivoFormato(string equicodi,string subEstacion, string extension, string nombre)
        {
            string sNombreArchivo = "";
            string fecha = DateTime.Now.ToString("yyyyMMddhhmm");

            sNombreArchivo = nombre + "_" + fecha + "." + extension;

            return sNombreArchivo;
        }


        public EqPropequiDTO GetIdCambioEstado(int equicodi)
        {
            return FactorySic.GetEqPropequiRepository().GetIdCambioEstado(equicodi);
        }

        public string SaveCambioEstado(EqPropequiDTO entity)
        {
            return FactorySic.GetEqPropequiRepository().SaveCambioEstado(entity);
        }

        public void UpdateCambioEstado(EqPropequiDTO entity)
        {
            FactorySic.GetEqPropequiRepository().UpdateCambioEstado(entity);
        }

        public EprEquipoDTO GetDetalleArbolEquipoProteccion(int equicodi, int nivel)
        {
            return FactorySic.GetEprEquipoRepository().GetDetalleArbolEquipoProteccion(equicodi, nivel);
        }

        public EprEquipoDTO ObtenerDatoCelda(int equicodi)
        {
            return FactorySic.GetEprEquipoRepository().ObtenerDatoCelda(equicodi);
        }


        #endregion
    }
}
