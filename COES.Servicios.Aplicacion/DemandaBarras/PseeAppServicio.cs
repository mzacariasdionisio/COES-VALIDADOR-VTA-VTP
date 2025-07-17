using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.CortoPlazo.Helper;
using COES.Servicios.Aplicacion.DemandaBarras.Helper;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.DemandaBarras
{
    public class PseeAppServicio
    {
        /// <summary>
        /// Instancia de la clase helper
        /// </summary>
        FileHelper fileHelper = new FileHelper();

        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(PseeAppServicio));

        #region Configuracion

        /// <summary>
        /// Permite listar las empresas de la relación
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarEmpresasRelacion()
        {
            return FactorySic.GetEqRelacionRepository().ListarEmpresas(ConstantesDemandaBarras.FuenteDemanda);
        }

        /// <summary>
        /// Permite listar las empresas del sein
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarEmpresasSein()
        {
            return FactorySic.GetSiEmpresaRepository().ObtenerEmpresasSEIN();
        }

        /// <summary>
        /// Permite listar las familias de equipos
        /// </summary>
        /// <returns></returns>
        public List<EqFamiliaDTO> ListarFamilias()
        {
            return FactorySic.GetEqFamiliaRepository().List();
        }

        /// <summary>
        /// Permite obtener los equipos generadores por empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public List<EqEquipoDTO> ListarEquiposPorEmpresa(int emprcodi, int famcodi)
        {
            return (new FormatoMedicionAppServicio()).ObtenerEquiposPorFamilia(emprcodi, famcodi);
        }

        /// <summary>
        /// Inserta un registro de la tabla EQ_RELACION
        /// </summary>
        public int SaveEqRelacion(EqRelacionDTO entity)
        {
            try
            {
                entity.Indfuente = ConstantesDemandaBarras.FuenteDemanda;
                int resultado = 1;
                if (entity.Relacioncodi == 0)
                {
                    int count = FactorySic.GetEqRelacionRepository().ObtenerPorEquipo((int)entity.Equicodi, ConstantesDemandaBarras.FuenteDemanda);

                    if (count == 0)
                    {
                        FactorySic.GetEqRelacionRepository().Save(entity);
                    }
                    else
                    {
                        resultado = 2;
                    }
                }
                else
                {
                    FactorySic.GetEqRelacionRepository().Update(entity);
                }
                
                int contador = FactorySic.GetMePtomedicionRepository().VerificarExistencia((int)entity.Equicodi, 
                    ConstantesDemandaBarras.OrigenLectura);

                if (contador == 0)
                {
                    EqEquipoDTO equipo = FactorySic.GetEqEquipoRepository().GetById((int)entity.Equicodi);
                    MePtomedicionDTO ptoMedicion = new MePtomedicionDTO
                    {
                        Origlectcodi = ConstantesDemandaBarras.OrigenLectura,
                        Ptomedielenomb = equipo.Equiabrev,
                        Ptomedidesc = equipo.Equinomb,
                        Orden = -1,
                        Codref = -1,
                        Equicodi = equipo.Equicodi,
                        Grupocodi = -1,
                        Emprcodi = equipo.Emprcodi,
                        Tipoinfocodi = 15,
                        Ptomediestado = ConstantesAppServicio.Activo,
                        Lastdate = DateTime.Now
                    };

                    FactorySic.GetMePtomedicionRepository().Save(ptoMedicion);
                }

                return resultado;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EQ_RELACION
        /// </summary>
        public void DeleteEqRelacion(int relacioncodi)
        {
            try
            {
                FactorySic.GetEqRelacionRepository().Delete(relacioncodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla EQ_RELACION
        /// </summary>
        public EqRelacionDTO GetByIdEqRelacion(int relacioncodi)
        {
            return FactorySic.GetEqRelacionRepository().GetById(relacioncodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EQ_RELACION
        /// </summary>
        public List<EqRelacionDTO> ListEqRelacions()
        {
            return FactorySic.GetEqRelacionRepository().List(ConstantesDemandaBarras.FuenteDemanda);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla EqRelacion
        /// </summary>
        public List<EqRelacionDTO> GetByCriteriaEqRelacions(int idEmpresa, string estado)
        {
            return FactorySic.GetEqRelacionRepository().GetByCriteria(idEmpresa, estado, ConstantesDemandaBarras.FuenteDemanda);
        }

        #endregion

        #region Lectura

        /// <summary>
        /// Realiza el proceso de obtencion de datos
        /// </summary>
        public void Proceso()
        {
            try
            {
                //string ruta = ConfigurationManager.AppSettings[ConstantesDemandaBarras.RutaPSSE];
                //string archivo = ConfigurationManager.AppSettings[ConstantesDemandaBarras.ArchivoPSSE];

                //// Obtenemos la ruta del archivo
                //string filename = ruta + archivo;

                //DateTime fechaProceso = DateTime.Now;

                //// Obtenemos los datos de las barras
                //List<NombreCodigoBarra> barras = this.fileHelper.ObtenerBarras(filename);

                //// Obtenemos los datos de demmanda
                //List<string[]> datosDemanda = this.fileHelper.ObtenerDatosDemanda(filename);

                //// Obtenemos las relaciones con equipo y ptomedicion
                //List<EqRelacionDTO> configuracion = this.ObtenerRelacion(barras, datosDemanda);

                //// Grabamos en la tabla de mediciones
                //foreach (EqRelacionDTO item in configuracion)
                //{
                //    if (item.Ptomedicodi != null)
                //    {
                //        this.GrabarMedicion((int)item.Ptomedicodi, ConstantesDemandaBarras.TipoInfoActiva, fechaProceso, item.PotenciaActiva);
                //        this.GrabarMedicion((int)item.Ptomedicodi, ConstantesDemandaBarras.TipoInfoReactiva, fechaProceso, item.PotenciaReactiva);
                //    }
                //}
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Grabar datos las mediciones cada 15 minutos
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="tipoInformacion"></param>
        private void GrabarMedicion(int ptomedicion, int tipoInformacion, DateTime fechaProceso, decimal? valor)
        {
            DateTime fechaConsulta = fechaProceso;

            int minuto = fechaProceso.Hour * 60 + fechaProceso.Minute;
            int indice = 0;

            if (fechaProceso.Hour == 0 && fechaProceso.Minute == 0)
            {
                fechaConsulta = fechaProceso.AddDays(-1);
                indice = 96;
            }
            else 
            {
                indice = minuto / 15;
            }

            int anio = fechaConsulta.Year;
            int mes = fechaConsulta.Month;
            int dia = fechaConsulta.Day;

            DateTime fecha = new DateTime(anio, mes, dia);

            MeMedicion96DTO medicion = FactorySic.GetMeMedicion96Repository().GetById(ConstantesDemandaBarras.Lectura, fecha,
                tipoInformacion, ptomedicion);

            MeMedicion96DTO entidad = new MeMedicion96DTO();
            entidad.Lectcodi = ConstantesDemandaBarras.Lectura;
            entidad.Medifecha = fecha;
            entidad.Tipoinfocodi = tipoInformacion;
            entidad.Ptomedicodi = ptomedicion;
            entidad.Meditotal = 0;

            if (medicion == null)
            {
                FactorySic.GetMeMedicion96Repository().Save(entidad);
            }

            if (valor != null)
                FactorySic.GetMeMedicion96Repository().ActualizarMedicion(ConstantesDemandaBarras.Lectura, fecha, tipoInformacion,
                    ptomedicion, indice, valor);
        }

        /// <summary>
        /// Permite obtener la relacion de equipamiento
        /// </summary>
        /// <returns></returns>
        private List<EqRelacionDTO> ObtenerRelacion(List<NombreCodigoBarra> list, List<string[]> datosDemanda)
        {
            List<EqRelacionDTO> relacion = FactorySic.GetEqRelacionRepository().ObtenerConfiguracionProcesoDemanda(
                ConstantesDemandaBarras.FuenteDemanda, ConstantesDemandaBarras.OrigenLectura);

            foreach (EqRelacionDTO entity in relacion)
            {
                string tension = string.Empty;
                decimal? potenciaActiva = null;
                decimal? potenciaReactiva = null;
                //entity.Codbarra = this.fileHelper.ObtenerCodigoBarra(entity.Nombarra, list, out tension);
                //this.fileHelper.ObtenerPotenciaDemandaBarras(entity.Codbarra, datosDemanda, out potenciaActiva, out potenciaReactiva);
                entity.PotenciaActiva = potenciaActiva;
                entity.PotenciaReactiva = potenciaReactiva;
                entity.Tension = tension;
            }

            return relacion;
        }

        #endregion

    }
}
