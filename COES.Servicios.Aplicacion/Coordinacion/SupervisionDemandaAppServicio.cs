using System;
using System.Collections.Generic;
using System.Linq;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System.Configuration;
using COES.Framework.Base.Tools;
using COES.Base.Tools;
using System.Net;
using COES.Servicios.Aplicacion.CortoPlazo.Helper;
using COES.Servicios.Aplicacion.CortoPlazo;
using COES.Dominio.DTO.Scada;
using System.Text;
using COES.Servicios.Aplicacion.General;

namespace COES.Servicios.Aplicacion.Coordinacion
{
    public class SupervisionDemandaAppServicio
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(SupervisionDemandaAppServicio));

        #region Relaciones

        /// <summary>
        /// Permite listar las empresas de la relación
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarEmpresasRelacion()
        {
            return FactorySic.GetEqRelacionRepository().ListarEmpresasReservaRotante();
        }
        /// <summary>
        /// 
        /// Permite obtener las empresas generadoras
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarEmpresasGeneradoras()
        {
            return FactorySic.GetSiEmpresaRepository().ObtenerEmpresasSEIN().Where(x => x.Tipoemprcodi == 3 || x.Emprcodi == 13 || x.Emprcodi == 67).OrderBy(x => x.Emprnomb).ToList();
        }

        /// <summary>
        /// Permite obtener los equipos generadores por empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public List<EqEquipoDTO> ListarEquiposPorEmpresa(int idEmpresa)
        {
            return FactorySic.GetEqRelacionRepository().ObtenerEquiposRelacion(idEmpresa);
        }

        /// <summary>
        /// Inserta un registro de la tabla EQ_RELACION
        /// </summary>
        public int SaveEqRelacion(EqRelacionDTO entity)
        {
            try
            {
                //entity.Indfuente = ConstantesCortoPlazo.FuenteGeneracion;
                int resultado = 1;
                entity.Lastdate = DateTime.Now;
                if (entity.Relacioncodi == 0)
                {
                    int count = FactorySic.GetEqRelacionRepository().ObtenerPorEquipoReservaRotante((int)entity.Equicodi);

                    if (count == 0)
                    {
                        FactorySic.GetEqRelacionRepository().SaveReservaRotante(entity);
                    }
                    else
                    {
                        resultado = 2;
                    }
                }
                else
                {
                    FactorySic.GetEqRelacionRepository().UpdateReservaRotante(entity);
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
        /// Permite realizar búsquedas en la tabla EqRelacion
        /// </summary>
        public List<EqRelacionDTO> GetByCriteriaEqRelacions(int idEmpresa, string estado)
        {
            return FactorySic.GetEqRelacionRepository().ObtenerListadoReservaRotante(idEmpresa, estado);
        }

        #endregion

        #region Generación EMS

        /// <summary>
        /// Permite leer los datos de generación EMS
        /// </summary>
        public void ObtenerGeneracionEMS()
        {
            try
            {
                DateTime fechaProceso = DateTime.Now;
                string filePsse = ConstantesCortoPlazo.ArchivoPSSEGen;
                string pathTrabajo = string.Empty;
                string pathRaiz = ConfigurationManager.AppSettings[ConstantesCortoPlazo.PathGeneracionEMS];
                bool indicadorPsse = FileHelper.ObtenerArchivoRawGeneracion(filePsse, pathTrabajo, pathRaiz, fechaProceso);

                if (indicadorPsse)
                {
                    //- Obtenemos la configuracion de los nombres y códigos de las barras
                    List<NombreCodigoBarra> relacionBarra = FileHelper.ObtenerBarras(pathTrabajo + filePsse, pathRaiz);

                    //- Cargado datos de generacion del PSSE y NCP
                    List<string[]> datosEPPS = FileHelper.ObtenerDatosEPPS(pathTrabajo + filePsse, pathRaiz);

                    List<EqRelacionDTO> list = (new CostoMarginalAppServicio()).ObtenerConfiguracionGeneracionEMS(relacionBarra, datosEPPS);

                    List<EqRelacionDTO> restricciones = FactorySic.GetEqRelacionRepository().ObtenerRestriccionOperativa(fechaProceso);

                    //- Lista los modos de operación activos en la hora
                    List<int> modosOperacion = FactorySic.GetEqRelacionRepository().ObtenerModosOperacion(fechaProceso);

                    //- Listado de formulas generales
                    List<PrGrupodatDTO> formulasGeneral = FactorySic.GetPrGrupodatRepository().ObtenerParametroGeneral(fechaProceso);

                    //- Listado de unidades de Reserva Secundaria
                    List<EveRsfdetalleDTO> listadoRsf = FactorySic.GetEveRsfdetalleRepository().ObtenerUnidadesRSF(fechaProceso);

                    n_parameter parameterGeneral = new n_parameter();
                    foreach (PrGrupodatDTO itemConcepto in formulasGeneral)
                    {
                        if (!string.IsNullOrEmpty(itemConcepto.Concepabrev) && !string.IsNullOrEmpty(itemConcepto.Formuladat))
                            parameterGeneral.SetData(itemConcepto.Concepabrev, itemConcepto.Formuladat);
                    }

                    double tipoCambio = parameterGeneral.GetEvaluate(ConstantesCortoPlazo.PropTipoCambio);

                                       
                    //- Grabado de datos
                    foreach (EqRelacionDTO item in list)
                    {
                        item.IndEspecial = true;
                        if (item.IndTipo == ConstantesCortoPlazo.TipoTermica)
                        {
                            if (item.Indcoes == ConstantesAppServicio.SI)
                            {
                                int idModoOperacion = 0;

                                //- Buscamos el modo de operación en las horas de operación
                                if (!string.IsNullOrEmpty(item.Modosoperacion))
                                {
                                    List<int> modos = item.Modosoperacion.Split(ConstantesAppServicio.CaracterComa).Select(int.Parse).ToList();
                                    foreach (int modo in modos)
                                    {
                                        if (modosOperacion.Where(x => x == modo).Count() > 0)
                                        {
                                            idModoOperacion = modo;
                                            item.IndModoOperacion = ConstantesCortoPlazo.ModoOperacionHO;
                                            break;
                                        }
                                    }
                                }

                                if (idModoOperacion == 0)
                                {
                                    item.IndModoOperacion = ConstantesCortoPlazo.ModoOperacionNoExiste;
                                }
                                else
                                {
                                    #region Obteniendo las propiedades de las térmicas

                                    string idsGrupos = idModoOperacion + ConstantesAppServicio.CaracterComa.ToString() + item.Grupocodi +
                                                       ConstantesAppServicio.CaracterComa.ToString() + item.Grupopadre;
                                    List<PrGrupodatDTO> listFormulas = new List<PrGrupodatDTO>();
                                    listFormulas.AddRange(formulasGeneral);
                                    listFormulas.AddRange(FactorySic.GetPrGrupodatRepository().ObtenerParametroModoOperacion(idsGrupos, fechaProceso));

                                    //- Declaramos el objeto parameter para calcular los valores
                                    n_parameter parameter = new n_parameter();
                                    foreach (PrGrupodatDTO itemConcepto in listFormulas)
                                    {
                                        if (!string.IsNullOrEmpty(itemConcepto.Concepabrev) && !string.IsNullOrEmpty(itemConcepto.Formuladat))
                                            parameter.SetData(itemConcepto.Concepabrev, itemConcepto.Formuladat);
                                    }

                                    //- Con esto sacamos las propiedades para el modo de operación como son curva de consumo, costo combustible, CV OYM

                                    double potMaxima = parameter.GetEvaluate(ConstantesCortoPlazo.PropPotenciaMaxima); //- Pmax                             
                                    double potEfectiva = parameter.GetEvaluate(ConstantesCortoPlazo.PropPotenciaEfectiva); //- Pe
                                    double factorConversion = parameter.GetEvaluate(ConstantesCortoPlazo.PropFactorConversion); //PCI_SI

                                    List<CoordenadaConsumo> curva = new List<CoordenadaConsumo>();

                                    string indCentral = (listFormulas.Where(x => x.Concepabrev == ConstantesCortoPlazo.PropIndCentral).Count() > 0) ?
                                        listFormulas.Where(x => x.Concepabrev == ConstantesCortoPlazo.PropIndCentral).FirstOrDefault().Formuladat : string.Empty; //;                              

                                    int nroUnidades = 1;
                                    if (indCentral == ConstantesAppServicio.SI)
                                    {
                                        nroUnidades = FactorySic.GetEqRelacionRepository().ObtenerNroUnidades(item.Grupocodi);
                                        if (nroUnidades == 0) nroUnidades = 1;
                                    }

                                    //- Obteniendo velocidad de toma y reduccion de carga
                                    //- double velocidadDescarga = parameter.GetEvaluate(ConstantesCortoPlazo.PropVelocidadDescarga);
                                    //- double velocidadCarga = parameter.GetEvaluate(ConstantesCortoPlazo.PropVelocidadCarga);
                                    string velocidadToma = listFormulas.Where(x => x.Concepabrev == ConstantesCortoPlazo.PropVelocidadCarga).FirstOrDefault().Formuladat;
                                    string velocidadReduccion = listFormulas.Where(x => x.Concepabrev == ConstantesCortoPlazo.PropVelocidadDescarga).FirstOrDefault().Formuladat;
                                    bool existeVelocidaCarga = false;
                                    bool existeVelocidaDescarga = false;
                                    double velocidadCarga = (new CostoMarginalAppServicio()).ObtenerPropiedadVelocidad(velocidadToma, out existeVelocidaCarga);
                                    double velocidadDescarga = (new CostoMarginalAppServicio()).ObtenerPropiedadVelocidad(velocidadReduccion, out existeVelocidaDescarga);

                                    item.PotenciaMaxima = potEfectiva / nroUnidades;
                                    item.VelocidadCarga = velocidadCarga;
                                    item.VelocidadDescarga = velocidadDescarga;


                                    //- Region completado por RSF

                                    #endregion
                                    item.IndEspecial = false;
                                }

                                List<EqRelacionDTO> listRestriccion = restricciones.Where(x => x.Equicodi == item.Equicodi).ToList();
                                List<RestriccionUnidad> ListaRestricciones = new List<RestriccionUnidad>();

                                foreach (EqRelacionDTO itemRestriccion in listRestriccion)
                                {
                                    if (itemRestriccion.Subcausacodi == COES.Servicios.Aplicacion.Eventos.Helper.ConstantesOperacionesVarias.SubcausacodiPotenciaFija)
                                    {
                                        ListaRestricciones.Add(new RestriccionUnidad
                                        {
                                            Tipo = ConstantesCortoPlazo.RestriccionPotFija,
                                            Valor = itemRestriccion.Valor
                                        });
                                    }
                                    else if (itemRestriccion.Subcausacodi == COES.Servicios.Aplicacion.Eventos.Helper.ConstantesOperacionesVarias.SubcausacodiPotenciaMax)
                                    {
                                        ListaRestricciones.Add(new RestriccionUnidad
                                        {
                                            Tipo = ConstantesCortoPlazo.RestriccionPotMaxima,
                                            Valor = itemRestriccion.Valor
                                        });
                                    }
                                    else if (itemRestriccion.Subcausacodi == COES.Servicios.Aplicacion.Eventos.Helper.ConstantesOperacionesVarias.SubcausacodiPotenciaMin)
                                    {
                                        ListaRestricciones.Add(new RestriccionUnidad
                                        {
                                            Tipo = ConstantesCortoPlazo.RestriccionPotMinima,
                                            Valor = itemRestriccion.Valor
                                        });
                                    }
                                    else if (itemRestriccion.Subcausacodi == COES.Servicios.Aplicacion.Eventos.Helper.ConstantesOperacionesVarias.SubcausacodiPlenacarga)
                                    {
                                        ListaRestricciones.Add(new RestriccionUnidad
                                        {
                                            Tipo = ConstantesCortoPlazo.RestriccionPlenaCarga,
                                            Valor = itemRestriccion.Valor
                                        });
                                    }
                                }

                                item.ListaRestriccion = ListaRestricciones;

                                //- Verificamos si tiene restriccion RSF
                                EveRsfdetalleDTO rsfDetalle = listadoRsf.Where(x => x.Equicodi == item.Equicodi).FirstOrDefault();

                                if (rsfDetalle != null)
                                {
                                    item.ExistenciaRsf = true;
                                    item.ValorRsf = (decimal)rsfDetalle.Rsfdetvalaut;
                                    item.PadreRsf = rsfDetalle.Equipadre;
                                }

                               
                            }                            
                        }
                    }

                    //- Calculamos las valores RSF para cada equipo
                    List<int> equispadre = list.Where(x => x.ExistenciaRsf).Select(x => x.PadreRsf).Distinct().ToList();

                    foreach (int equipadre in equispadre)
                    {
                        List<EqRelacionDTO> elementos = list.Where(x => x.ExistenciaRsf && x.PadreRsf == equipadre && x.IndOperacion == 1.ToString()).ToList();

                        foreach (EqRelacionDTO itemRSF in elementos)
                        {
                            itemRSF.CantidadRsf = elementos.Count();
                        }
                    }

                    double minutos = (double)(new ParametroAppServicio()).ObtenerValorParametro(ConstantesCortoPlazo.IdParametroMinutos, fechaProceso);
                    if (minutos == 0)
                        minutos = 10;

                    double rpf = (double)(new ParametroAppServicio()).ObtenerValorParametro(ConstantesCortoPlazo.IdParametroRpf, fechaProceso);
                    if (rpf == 0)
                        rpf = 0.024;

                    double variacionPotencia = (double)(new ParametroAppServicio()).ObtenerValorParametro(ConstantesCortoPlazo.IdParametroVariacionPotencia, fechaProceso);

                    foreach (EqRelacionDTO item in list)
                    {
                        if (!item.IndEspecial)
                        {
                            double rsf = 0;
                            if (item.ExistenciaRsf)
                            {
                                if (item.CantidadRsf > 0)
                                    rsf = (double)(item.ValorRsf / item.CantidadRsf);
                            }

                            decimal potenciaMaxima = (decimal)(Math.Min((double)item.PotGenerada + minutos * (double)item.VelocidadCarga, (item.PotenciaMaxima - rsf) / (1 + rpf)));
                            potenciaMaxima = Math.Max(potenciaMaxima, (decimal)item.PotGenerada);


                            if (variacionPotencia >= 0 && variacionPotencia <= 100)
                            {
                                item.PotenciaMaxima = Math.Min(item.PotenciaMaxima, ((double)item.PotGenerada) * ((double)(1 + (decimal)variacionPotencia / 100)));
                            }
                        }
                        else 
                        {
                            item.PotenciaMaxima = (double)item.PotGenerada;
                        }

                        EmsGeneracionDTO entity = new EmsGeneracionDTO
                        {
                            Equicodi = item.Equicodi ?? -1,
                            Emsgenfecha = fechaProceso,
                            Emsgenvalor = (decimal?)item.PotGenerada,
                            Emsgenoperativo = item.IndOperacion,
                            Emsgenfeccreacion = DateTime.Now,
                            Emsgenpotmax = (decimal)item.PotenciaMaxima,                            
                            Emsgenusucreacion = "procesoems"
                        };

                        FactorySic.GetEmsGeneracionRepository().Save(entity);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        /// <summary>
        /// Permite obtener los grupos para la supervision de la demanda
        /// </summary>
        /// <returns></returns>
        public List<EqRelacionDTO> ObtenerGruposSupervisionDemanda()
        {
            List<EqRelacionDTO> entitys = FactorySic.GetEqRelacionRepository().GetByCriteriaReservaRotante(-1, ConstantesAppServicio.Activo, -1);
            return entitys;
        }

        /// <summary>
        /// Permite obtener los datos para la generación
        /// </summary>
        /// <param name="tipo">0: PSS/ODMS, 1: SCADA SP7</param>
        public List<EqRelacionDTO> ObtenerDatosGeneracion(int tipo, int idGrupo)
        {
            DateTime fecha = DateTime.Now;

            //- Lista de todos los agrupamientos (pr_grupo)
            List<EqRelacionDTO> entitys = FactorySic.GetEqRelacionRepository().GetByCriteriaReservaRotante(-1, ConstantesAppServicio.Activo, idGrupo);

            //- datos del programa o reprograma diario
            List<SupDemandaDato> programado = this.ObtenerDatosProgramado(fecha);
            List<SupDemandaDato> list = new List<SupDemandaDato>();

            if (tipo == 0)
            {
                //- Obtención de la generación del EMS
                list = this.ObtenerDatosEMS(fecha);
            }
            else if (tipo == 1)
            {
                //- Obtención de la generación del SCADA
                list = this.ObtenerDatosSCADA(fecha);
            }

            //- Completamos los elementos con los valores correctos
            foreach (EqRelacionDTO item in entitys)
            {
                //- Leemos los datos del ems o scada por cada grupo
                List<SupDemandaDato> subList = list.Where(x => x.Grupocodi == item.Grupocodi).ToList();

                //- En caso no exista datos en la fuente origen se obtiene del programado
                if (subList.Count() == 0)
                {
                    item.ListaGeneracion = programado.Where(x => x.Grupocodi == item.Grupocodi).OrderBy(x => x.Indice).ToList();
                    item.IndCompletado = 1;
                }
                else
                {
                    item.ListaGeneracion = subList.OrderBy(x => x.Indice).ToList();
                    item.IndCompletado = 0;
                }
            }

            return entitys;
        }

        /// <summary>
        /// Permite obtener los datos para la grilla
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="idGrupo"></param>
        /// <param name="tipos"></param>
        /// <returns></returns>
        public string[][] ObtenerGrillaDatosSupDemanda(int tipo, int idGrupo, out int[] indices, int tipoGeneracion)
        {
            DateTime fecha = new DateTime(DateTime.Now.Year, 1, 1);
            string[][] datos = new string[97][];
            List<EqRelacionDTO> entitys = new List<EqRelacionDTO>();

            if (tipoGeneracion == -1)
            {
                entitys = this.ObtenerDatosGeneracion(tipo, idGrupo);
            }
            else
            {
                entitys = this.ObtenerDatosGeneracion(tipo, idGrupo).Where(x => x.Tgenercodi == tipoGeneracion).ToList();
            }



            indices = new int[entitys.Count + 1];

            for (int i = 0; i <= 96; i++)
            {
                datos[i] = new string[entitys.Count + 1];
                if (i > 0)
                    datos[i][0] = fecha.AddMinutes((i) * 15).ToString("HH:mm");
            }

            datos[0][0] = "HORA";

            int column = 1;
            foreach (EqRelacionDTO entity in entitys)
            {
                datos[0][column] = entity.Gruponomb;
                indices[column] = entity.IndCompletado;

                for (int i = 1; i <= 96; i++)
                {
                    if (entity.ListaGeneracion.Count > i)
                        datos[i][column] = entity.ListaGeneracion[i - 1].Valor.ToString();
                }

                column++;
            }

            return datos;
        }

        /// <summary>
        /// Permite obtener los datos para la grilla
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="idGrupo"></param>
        /// <param name="tipos"></param>
        /// <returns></returns>
        public string[][] ObtenerGrillaDatosSupDemandaExportado(int tipo, int idGrupo, int tipoGeneracion)
        {
            DateTime fecha = new DateTime(DateTime.Now.Year, 1, 1);
            string[][] datos = new string[99][];
            List<EqRelacionDTO> entitys = new List<EqRelacionDTO>();

            if (tipoGeneracion == -1)
            {
                entitys = this.ObtenerDatosGeneracion(tipo, idGrupo);
            }
            else
            {
                entitys = this.ObtenerDatosGeneracion(tipo, idGrupo).Where(x => x.Tgenercodi == tipoGeneracion).ToList();
            }

            datos[0] = new string[entitys.Count + 1];
            datos[1] = new string[entitys.Count + 1];
            datos[2] = new string[entitys.Count + 1];

            for (int i = 1; i <= 96; i++)
            {
                datos[2 + i] = new string[entitys.Count + 1];
                datos[2 + i][0] = fecha.AddMinutes((i) * 15).ToString("HH:mm");
            }

            datos[0][0] = "HORA";

            int column = 1;
            foreach (EqRelacionDTO entity in entitys)
            {
                datos[0][column] = entity.Gruponomb.Trim();
                datos[1][column] = entity.Tgenernomb;

                if (entity.IndCompletado == 1)
                    datos[2][column] = "PROGRAMADO";
                else
                    datos[2][column] = "EJECUTADO";

                for (int i = 1; i <= 96; i++)
                {
                    if (entity.ListaGeneracion.Count > i)
                        datos[i + 2][column] = entity.ListaGeneracion[i - 1].Valor.ToString();
                }

                column++;
            }

            return datos;
        }

        /// <summary>
        /// Permite obtener los datos del EMS
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<SupDemandaDato> ObtenerDatosEMS(DateTime fecha)
        {
            List<SupDemandaDato> entitys = new List<SupDemandaDato>();
            List<EmsGeneracionDTO> entitysEMS = FactorySic.GetEmsGeneracionRepository().ObtenerDatosSupervisionDemanda(fecha);
            List<int> idsGrupo = entitysEMS.Select(x => x.Grupocodi).Distinct().ToList();

            foreach (int idGrupo in idsGrupo)
            {
                List<EmsGeneracionDTO> list = entitysEMS.Where(x => x.Grupocodi == idGrupo).ToList();

                for (int i = 1; i <= 96; i++)
                {
                    SupDemandaDato entity = new SupDemandaDato();
                    entity.Grupocodi = idGrupo;
                    //entity.Fecha = fecha.AddMinutes(15 * i);
                    entity.Valor = ((list.Where(x => x.Indice == i)).Count() > 0) ? list.Where(x => x.Indice == i).Sum(x => x.Emsgenvalor) : null;
                    entity.PotenciaMaxima = ((list.Where(x => x.Indice == i)).Count() > 0) ? list.Where(x => x.Indice == i).Sum(x => x.Emsgenpotmax) : null;
                    entity.Tipo = 0;
                    entity.Indice = i;
                    

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        /// <summary>
        /// Permite obtener los datos de SCADA
        /// </summary>
        /// <returns></returns>
        public List<SupDemandaDato> ObtenerDatosSCADA(DateTime fecha)
        {
            List<SupDemandaDato> entitys = new List<SupDemandaDato>();
            List<MeScadaSp7DTO> entitysSCADA = FactoryScada.GetMeScadaSp7Repository().ObtenerDatosSupervisionDemanda(fecha);
            int hora = Convert.ToInt32(Math.Floor(((double)(DateTime.Now.Hour * 60) + DateTime.Now.Minute) / 15));

            foreach (MeScadaSp7DTO item in entitysSCADA)
            {
                for (int i = 1; i <= 96; i++)
                {
                    SupDemandaDato entity = new SupDemandaDato();
                    entity.Grupocodi = item.Grupocodi;                    
                    entity.Valor = (decimal?)(item.GetType().GetProperty("H" + i).GetValue(item, null));

                    if (i > 1)
                    {
                        if (entity.Valor == null)
                        {
                            decimal? valor2 = (decimal?)(item.GetType().GetProperty("H" + (i - 1)).GetValue(item, null));
                            entity.Valor = valor2;

                            if (i == hora)
                            {
                                if (i > 2)
                                {
                                    decimal? valor3 = (decimal?)(item.GetType().GetProperty("H" + (i - 2)).GetValue(item, null));
                                    entity.Valor = valor3;
                                }
                            }
                        }
                    }

                    if (item.Tgenercodi == 3)
                    {
                        if (entity.Valor < 0)
                        {
                            entity.Valor = 0;
                        }
                    }


                    entity.Tipo = 1;
                    entity.Indice = i;
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        /// <summary>
        /// Permite obtener los datos del programado
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<SupDemandaDato> ObtenerDatosProgramado(DateTime fecha)
        {
            List<SupDemandaDato> entitys = new List<SupDemandaDato>();
            List<MeMedicion48DTO> list = FactorySic.GetMeMedicion48Repository().ObtenerProgramaReProgramaDia(fecha, 5);

            if(list == null)
            {
                list = FactorySic.GetMeMedicion48Repository().ObtenerProgramaReProgramaDia(fecha, 4);
            }
            else if(list.Count == 0)
            {
                list = FactorySic.GetMeMedicion48Repository().ObtenerProgramaReProgramaDia(fecha, 4);
            }

            foreach (MeMedicion48DTO item in list)
            {
                for (int i = 1; i <= 48; i++)
                {
                    SupDemandaDato entity = new SupDemandaDato();
                    entity.Grupocodi = item.Grupocodi;
                    //entity.Fecha = fecha.AddMinutes(30 * i - 15);
                    entity.Valor = (decimal)(item.GetType().GetProperty("H" + i).GetValue(item, null));
                    entity.Tipo = 3;
                    entity.Indice = (i - 1) * 2 + 1;
                    entitys.Add(entity);

                    SupDemandaDato entityAdicional = new SupDemandaDato();
                    entityAdicional.Grupocodi = item.Grupocodi;
                    //entityAdicional.Fecha = fecha.AddMinutes(30 * i);
                    entityAdicional.Valor = entity.Valor;
                    entityAdicional.Tipo = 3;
                    entityAdicional.Indice = (i - 1) * 2 + 2;
                    entitys.Add(entityAdicional);
                }
            }

            return entitys;
        }

        /// <summary>
        /// Permite obtener los datos para el reporte gráfico
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public SupDemandaGrafico ObtenerDatosGrafico(int tipo)
        {
            SupDemandaGrafico result = new SupDemandaGrafico();

            result.IndicadorHora = Convert.ToInt32(Math.Floor(((double)(DateTime.Now.Hour * 60) + DateTime.Now.Minute) / 15));

            //- Creamos las instalaciones
            List<SupDemandaGraficoDatos> graficoTotal = new List<SupDemandaGraficoDatos>();
            List<SupDemandaGraficoDatos> graficoSolar = new List<SupDemandaGraficoDatos>();
            List<SupDemandaGraficoDatos> graficoEolico = new List<SupDemandaGraficoDatos>();
            List<SupDemandaGraficoDatos> graficoReservaRotante = new List<SupDemandaGraficoDatos>();

            //- Obtenemos los datos de la generación de EMS o SCADA según sea el caso
            List<EqRelacionDTO> generacionEjecutada = this.ObtenerDatosGeneracion(tipo, -1);

            //- Programado 4: programado, 5: reprogramado, 6: ejecutado
            List<MeMedicion48DTO> generacionPrograma = FactorySic.GetMeMedicion48Repository().ObtenerDemandaPortalWebTipo(5, DateTime.Now, DateTime.Now);
            if (generacionPrograma == null || generacionPrograma.Count == 0)
                generacionPrograma = FactorySic.GetMeMedicion48Repository().ObtenerDemandaPortalWebTipo(4, DateTime.Now, DateTime.Now);

            //- Obtenemos el ejecutado del dia anterior
            List<MeMedicion48DTO> ejecutadoDiaAnterior = FactorySic.GetMeMedicion48Repository().ObtenerDemandaPortalWebTipo(6, DateTime.Now, DateTime.Now);

            //- Obtener el ejecutado del día tipico dependiendo el dia
            DateTime fechaDiaTipico = DateTime.Now;
            if (DateTime.Now.DayOfWeek == DayOfWeek.Monday ||
                DateTime.Now.DayOfWeek == DayOfWeek.Saturday ||
                DateTime.Now.DayOfWeek == DayOfWeek.Sunday) fechaDiaTipico = DateTime.Now.AddDays(-7);
            else if (DateTime.Now.DayOfWeek == DayOfWeek.Tuesday) fechaDiaTipico = DateTime.Now.AddDays(-4);
            else if (DateTime.Now.DayOfWeek == DayOfWeek.Wednesday ||
                     DateTime.Now.DayOfWeek == DayOfWeek.Thursday ||
                     DateTime.Now.DayOfWeek == DayOfWeek.Friday) fechaDiaTipico = DateTime.Now.AddDays(-1);
            List<MeMedicion48DTO> ejecutadoDiaTipico = FactorySic.GetMeMedicion48Repository().ObtenerDemandaPortalWebTipo(6, fechaDiaTipico, fechaDiaTipico);

            //- Se debe armar las estructuras con todos los datos

            //- Colocamos los datos del ejecutado en tiempo real
            for (int i = 1; i <= 96; i++)
            {
                decimal totalGeneral = 0;
                decimal totalSolar = 0;
                decimal totalEolico = 0;
                decimal reservaRotante = 0;                

                foreach (EqRelacionDTO relacion in generacionEjecutada)
                {
                    if (relacion.ListaGeneracion.Count() > i)
                    {
                        decimal valor = (relacion.ListaGeneracion[i - 1].Valor != null) ? (decimal)relacion.ListaGeneracion[i - 1].Valor : 0;
                        decimal potenciaMaxima = (relacion.ListaGeneracion[i - 1].PotenciaMaxima != null) ? (decimal)relacion.ListaGeneracion[i - 1].PotenciaMaxima : 0;

                        reservaRotante = reservaRotante + Math.Abs(potenciaMaxima - valor);
                        totalGeneral = totalGeneral + valor;
                        if (relacion.Tgenercodi == 3)
                            totalSolar = totalSolar + valor;
                        if (relacion.Tgenercodi == 4)
                            totalEolico = totalEolico + valor;
                    }
                }

                SupDemandaGraficoDatos entityTotal = new SupDemandaGraficoDatos();
                entityTotal.Indice = i;
                entityTotal.Ejecutado = totalGeneral;
                graficoTotal.Add(entityTotal);

                SupDemandaGraficoDatos entitySolar = new SupDemandaGraficoDatos();
                entitySolar.Indice = i;
                entitySolar.Ejecutado = totalSolar;
                graficoSolar.Add(entitySolar);

                SupDemandaGraficoDatos entityEolico = new SupDemandaGraficoDatos();
                entityEolico.Indice = i;
                entityEolico.Ejecutado = totalEolico;
                graficoEolico.Add(entityEolico);

                SupDemandaGraficoDatos entityReservaRotante = new SupDemandaGraficoDatos();
                entityReservaRotante.Indice = i;
                entityReservaRotante.ReservaRotante = reservaRotante;
                graficoReservaRotante.Add(entityReservaRotante);
            }

            //- Obtener los datos del programado y ejecutado del dia tipico y dia anterior
            for (int i = 1; i <= 48; i++)
            {
                decimal totalGeneral = 0;
                decimal totalSolar = 0;
                decimal totalEolico = 0;

                foreach (MeMedicion48DTO item in generacionPrograma)
                {
                    totalGeneral = totalGeneral + (decimal)(item.GetType().GetProperty("H" + i).GetValue(item, null));
                    if (item.Tgenercodi == 3)
                        totalSolar = totalSolar + (decimal)(item.GetType().GetProperty("H" + i).GetValue(item, null));
                    if (item.Tgenercodi == 4)
                        totalEolico = totalEolico + (decimal)(item.GetType().GetProperty("H" + i).GetValue(item, null));
                }

                graficoTotal[(i - 1) * 2].Programado = totalGeneral;
                graficoTotal[(i - 1) * 2 + 1].Programado = totalGeneral;
                graficoSolar[(i - 1) * 2].Programado = totalSolar;
                graficoSolar[(i - 1) * 2 + 1].Programado = totalSolar;
                graficoEolico[(i - 1) * 2].Programado = totalEolico;
                graficoEolico[(i - 1) * 2 + 1].Programado = totalEolico;

                //- Datos para el dia tipico
                totalGeneral = 0;
                foreach (MeMedicion48DTO item in ejecutadoDiaTipico)
                {
                    totalGeneral = totalGeneral + (decimal)(item.GetType().GetProperty("H" + i).GetValue(item, null));
                }
                graficoTotal[(i - 1) * 2].DemandaDiaTipico = totalGeneral;
                graficoTotal[(i - 1) * 2 + 1].DemandaDiaTipico = totalGeneral;

                totalSolar = 0;
                totalEolico = 0;
                foreach (MeMedicion48DTO item in ejecutadoDiaAnterior)
                {
                    if (item.Tgenercodi == 3)
                        totalSolar = totalSolar + (decimal)(item.GetType().GetProperty("H" + i).GetValue(item, null));
                    if (item.Tgenercodi == 4)
                        totalEolico = totalEolico + (decimal)(item.GetType().GetProperty("H" + i).GetValue(item, null));
                }

                graficoSolar[(i - 1) * 2].DemandaDiaAnterior = totalSolar;
                graficoSolar[(i - 1) * 2 + 1].DemandaDiaAnterior = totalSolar;
                graficoEolico[(i - 1) * 2].DemandaDiaAnterior = totalEolico;
                graficoEolico[(i - 1) * 2 + 1].DemandaDiaAnterior = totalEolico;
            }

            result.ListaTotal = graficoTotal;
            result.ListaSolar = graficoSolar;
            result.ListaEolica = graficoEolico;
            result.ListaReservaRotante = graficoReservaRotante;


            result.Indicador1 = graficoTotal[result.IndicadorHora].Programado - graficoTotal[result.IndicadorHora - 1].Programado +
                                Math.Abs(graficoSolar[result.IndicadorHora].Programado - graficoSolar[result.IndicadorHora - 1].Programado) +
                                Math.Abs(graficoEolico[result.IndicadorHora].Programado - graficoEolico[result.IndicadorHora - 1].Programado);
            result.Indicador2 = graficoTotal[result.IndicadorHora - 1].Ejecutado - graficoTotal[result.IndicadorHora - 1].Programado;
            result.Indicador3 = graficoReservaRotante[result.IndicadorHora - 1].ReservaRotante;
            result.Indicador4 = graficoEolico[result.IndicadorHora - 1].Ejecutado - graficoEolico[result.IndicadorHora - 1].Programado;
            result.Indicador5 = graficoSolar[result.IndicadorHora - 1].Ejecutado - graficoSolar[result.IndicadorHora - 1].Programado;

            return result;
        }
    }
}
