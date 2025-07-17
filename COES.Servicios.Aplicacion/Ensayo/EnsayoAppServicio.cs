using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.Ensayo.Helper;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace COES.Servicios.Aplicacion.ensayo
{
    /// <summary>
    /// Clases con métodos del módulo ensayo
    /// </summary>
    public class EnsayoAppServicio : AppServicioBase
    {
        public HorasOperacionAppServicio servHO = new HorasOperacionAppServicio();

        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(EnsayoAppServicio));

        #region Métodos Tabla EN_ENSAYO

        /// <summary>
        /// Guarda un ensayo proveniente de Extranet
        /// </summary>
        public string GuardarEnsayoNuevo(ref EnEnsayoDTO ensayo, int idEmpresa, int idCentral, List<EnEnsayomodoDTO> listaMO, string usuario)
        {
            var res = "-1";
            int codiensayo; //codigo del ensayo a guardar  
            EnEnsayomodoDTO ensayoModo = new EnEnsayomodoDTO();
            EnEnsayounidadDTO ensayoUnidad = new EnEnsayounidadDTO();
            EnEnsayomodequiDTO ensayoModoEqui = new EnEnsayomodequiDTO();


            // guardamos el ensayo 
            ensayo.Emprcodi = idEmpresa;//empresa
            ensayo.Equicodi = idCentral;//central
            //ensayo.Equinomb = equinomb;//nombre CT
            ensayo.Ensayofecha = DateTime.Now;
            ensayo.Usercodi = usuario;
            ensayo.Estadocodi = EstadoEnsayo.Solicitado;
            ensayo.Ensayomodoper = "nousado";
            codiensayo = SaveEnEnsayo(ensayo);
            ensayo.Ensayocodi = codiensayo;

            int codiensayomodo;

            //ahora guardamos los MO
            foreach (var regenmo in listaMO)
            {
                //List<EnEnsayomodequiDTO> unidadesDelMO = regenmo.ListaUnidades;

                ensayoModo.Grupocodi = regenmo.Grupocodi;//ModoOperacion
                ensayoModo.Ensayocodi = ensayo.Ensayocodi;
                codiensayomodo = SaveEnEnsayomodo(ensayoModo);
                ensayoModo.Enmodocodi = codiensayomodo;

                int codiensayomodequi;
                foreach (var unidad in regenmo.ListaUnidades)
                {
                    // if (regenmo.Enmodocodi == unidad.Enmodocodi) //coloca los MO  con sus respectivos unidades
                    //{
                    if (unidad.Equicodi == -1)
                    { //si es un no especial

                        List<PrGrupoDTO> ListaUnidXModoOP = this.servHO.ListarUnidadesWithModoOperacionXCentralYEmpresa(idCentral, idEmpresa.ToString()).Where(x => x.Grupocodi == regenmo.Grupocodi).ToList();
                        foreach (var unidad1 in ListaUnidXModoOP)
                        {
                            ensayoModoEqui.Equicodi = unidad1.Equicodi;//Unidad
                            //ensayoModoEqui.Enmodocodi = ensayoModo.Ensayocodi;
                            ensayoModoEqui.Enmodocodi = ensayoModo.Enmodocodi;
                            codiensayomodequi = SaveEnEnsayomodequi(ensayoModoEqui);
                            ensayoModoEqui.Enmoeqcodi = codiensayomodequi;
                        }
                    }
                    else
                    {  // si es especial
                        ensayoModoEqui.Equicodi = unidad.Equicodi;//Unidad
                        //ensayoModoEqui.Enmodocodi = ensayoModo.Ensayocodi;
                        ensayoModoEqui.Enmodocodi = ensayoModo.Enmodocodi;
                        codiensayomodequi = SaveEnEnsayomodequi(ensayoModoEqui);
                        ensayoModoEqui.Enmoeqcodi = codiensayomodequi;
                    }

                    //}

                }
            }
            //Llena la tabla de EnsayoUnidad con las unidades filtradas
            List<EqEquipoDTO> listaUnidadesParaEnsayo = ListarUnidadesxEnsayo(ensayo.Ensayocodi);
            int codiensayounidad;
            foreach (var unidadG in listaUnidadesParaEnsayo)
            {
                ensayoUnidad.Ensayocodi = ensayo.Ensayocodi;
                ensayoUnidad.Equicodi = unidadG.Equicodi;
                codiensayounidad = SaveEnEnsayounidad(ensayoUnidad);
                ensayoUnidad.Enunidadcodi = codiensayounidad;
            }

            var lstformatos = ListEnEnsayoFormatosEmpresaCentral(idEmpresa, idCentral);
            //guardar formatos si existen previos a la misma empresa y central
            if (lstformatos != null)
            {
                foreach (var reg in lstformatos)
                {
                    if (reg.Formatocodi != 2 && reg.Formatocodi != 10)
                    {
                        /*reg.Ensayocodi = codiensayo;
                        reg.Ensformtestado = EstadoFormato.Enviado;
                        reg.Lastdate = DateTime.Now;
                        reg.Lastuser = usuario;
                        SaveEnEnsayoformato(reg);*/
                    }
                }
            }
            res = "1";
            return res;
        }

        /// <summary>
        /// Guardar Ensayo y Formatos provenientes de Intranet
        /// </summary>
        public string GrabarEnsayosYFormatos(int codensayo, List<string> codiUnidad, int emprcodi, string nomUser, string mo, List<EnEnsayoformatoDTO> lstformatos)
        {
            var resultado = "";
            EnEnsayoDTO ensayo = new EnEnsayoDTO();
            if (codensayo == 0) // si no existe el ensayo
            {
                foreach (string sUnidad in codiUnidad)
                {
                    // guardamos el ensayo 
                    ensayo.Emprcodi = emprcodi;
                    ensayo.Equicodi = int.Parse(sUnidad);// model.Equicodi;
                    ensayo.Ensayofecha = DateTime.Now;
                    ensayo.Usercodi = nomUser;
                    ensayo.Estadocodi = EstadoEnsayo.Solicitado;
                    ensayo.Ensayomodoper = mo;
                    codensayo = SaveEnEnsayo(ensayo);
                    ensayo.Ensayocodi = codensayo;

                    //guardar formatos si existen previos a la misma empresa y central
                    if (lstformatos != null)
                    {
                        foreach (var reg in lstformatos)
                        {
                            if (reg.Formatocodi != 2 && reg.Formatocodi != 10)
                            {
                                reg.Ensayocodi = codensayo;
                                reg.Ensformtestado = EstadoFormato.Enviado;
                                reg.Lastdate = DateTime.Now;
                                reg.Lastuser = nomUser;
                                SaveEnEnsayoformato(reg);
                            }
                        }
                    }
                }
                resultado = "graboEnsayo";
            }
            //si todo se guardo bien y el email tiene formato correcto, notificamos al controlador
            if (resultado.Equals("graboEnsayo") && (ensayo.Usercodi.IndexOf(ConstantesAppServicio.CaracterArroba) != -1))
            {
                resultado = "aptoParaEnviarEmail";
            }
            return resultado;
        }

        /// <summary>
        /// Inserta un registro de la tabla EN_ENSAYO
        /// </summary>
        public int SaveEnEnsayo(EnEnsayoDTO entity)
        {
            int codigo = 0;
            try
            {
                codigo = FactorySic.GetEnEnsayoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return codigo;
        }
        /// <summary>
        /// Actualiza un registro de la tabla EN_ENSAYO
        /// </summary>
        public void UpdateEnEnsayo(EnEnsayoDTO entity)
        {
            try
            {
                FactorySic.GetEnEnsayoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro columna ensayo de la tabla EN_ENSAYO
        /// </summary>       
        public void ActualizaEstadoEnsayo(int icodiensayo, DateTime dfechaEvento, int iCodEstado, DateTime lastdate, string lastuser)
        {
            try
            {
                FactorySic.GetEnEnsayoRepository().UpdateEstadoEnsayo(icodiensayo, dfechaEvento, iCodEstado, lastdate, lastuser);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EN_ENSAYO
        /// </summary>
        public void DeleteEnEnsayo()
        {
            try
            {
                FactorySic.GetEnEnsayoRepository().Delete();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla EN_ENSAYO
        /// </summary>
        public EnEnsayoDTO GetByIdEnEnsayo(int id)
        {
            return FactorySic.GetEnEnsayoRepository().GetById(id);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EN_ENSAYO
        /// </summary>
        public List<EnEnsayoDTO> ListEnEnsayos()
        {
            return FactorySic.GetEnEnsayoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla EnEnsayo
        /// </summary>
        public List<EnEnsayoDTO> GetByCriteriaEnEnsayos()
        {
            return FactorySic.GetEnEnsayoRepository().GetByCriteria();
        }

        /// <summary>
        /// Permite listar los ensayos segun los filtros indicados
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="equicodi"></param>
        /// <param name="estados"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="nroPaginas"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<EnEnsayoDTO> ListaDetalleCbEnsayosFiltro(string emprcodi, string equicodi, string estados, DateTime fechaInicio, DateTime fechaFin, int nroPaginas, int pageSize)
        {
            return FactorySic.GetEnEnsayoRepository().ListaDetalleFiltro(emprcodi, equicodi, estados, fechaInicio, fechaFin, nroPaginas, pageSize);
        }

        /// <summary>
        ///Permite listar los ensayos segun los filtros indicados para formato Excel
        /// </summary>     
        public List<EnEnsayoDTO> ListaDetalleCbEnsayosFiltroXls(string emprcodi, string equicodi, string estados, DateTime fechaInicio, DateTime fechaFin)
        {
            var listaData = FactorySic.GetEnEnsayoRepository().ListaDetalleFiltroXls(emprcodi, equicodi, estados, fechaInicio, fechaFin);
            List<EnEnsayoDTO> lista = listaData.GroupBy(x => new { x.Ensayocodi, x.Emprnomb, x.Equinomb, x.Ensayofecha, x.Estadonombre })
                .Select(x => new EnEnsayoDTO() { Ensayocodi = x.Key.Ensayocodi, Emprnomb = x.Key.Emprnomb, Equinomb = x.Key.Equinomb, Ensayofecha = x.Key.Ensayofecha, Estadonombre = x.Key.Estadonombre }).ToList();

            foreach (var reg in lista)
            {
                var listaDataXEnsayo = listaData.Where(x => x.Ensayocodi == reg.Ensayocodi).ToList();
                List<string> listaModo = listaDataXEnsayo.Select(x => x.Ensayomodoper).Distinct().OrderBy(x => x).ToList();
                List<string> listaUnidad = listaDataXEnsayo.Select(x => x.Unidadnomb).Distinct().OrderBy(x => x).ToList();

                reg.Ensayomodoper = string.Join(",", listaModo);
                reg.Unidadnomb = string.Join(",", listaUnidad);
            }

            return lista;
        }
        /// <summary>
        /// permite contar la antidad de ensayos
        /// </summary>       
        public int GetTotalEnsayo(string empresas, string centrales, string estados, DateTime fechaInicio, DateTime fechaFin)
        {
            if (centrales.Equals(""))
            {
                centrales = "-1";
            }
            if (empresas.Equals(""))
            {
                empresas = "-1";
            }
            return FactorySic.GetEnEnsayoRepository().ObtenerTotalListaEnsayo(empresas, centrales, estados, fechaInicio, fechaFin);
        }

        /// <summary>
        /// Inserta un registro de la tabla EN_ENSAYOMASTER
        /// </summary>
        public void SaveEnsayoMaster(int codensayo, int maxCodIngreso, DateTime lastDate, string lastUser)
        {
            try
            {
                FactorySic.GetEnEnsayoRepository().SaveEnsMaster(maxCodIngreso, codensayo, lastDate, lastUser);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// obtiene el maximo codigo del campo "codiingreso" de la tabla EN_ENSAYOMASTER
        /// </summary>       
        public int GetMaxIdEnsayoMaster()
        {
            return FactorySic.GetEnEnsayoRepository().GetMaxIdEnsMaster();
        }


        #endregion

        #region Métodos Tabla EN_ENSAYOFORMATO

        /// <summary>
        /// Inserta un registro de la tabla EN_ENSAYOFORMATO
        /// </summary>
        public void SaveEnEnsayoformato(EnEnsayoformatoDTO entity)
        {
            try
            {
                FactorySic.GetEnEnsayoformatoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla EN_ENSAYOFORMATO
        /// </summary>
        public void UpdateEnEnsayoformato(EnEnsayoformatoDTO entity)
        {
            try
            {
                FactorySic.GetEnEnsayoformatoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro estado de la tabla EN_ENSAYOFORMATO
        /// <param name="ensformtestado"></param>
        /// <param name="enunidadcodi"></param>
        /// <param name="formatocodi"></param>
        /// </summary>
        public void UpdateEnEnsayoformatoEstado(int ensformtestado, int enunidadcodi, int formatocodi)
        {
            try
            {
                FactorySic.GetEnEnsayoformatoRepository().UpdateEstado(ensformtestado, enunidadcodi, formatocodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EN_ENSAYOFORMATO
        /// </summary>
        public void DeleteEnEnsayoformato()
        {
            try
            {
                FactorySic.GetEnEnsayoformatoRepository().Delete();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EN_ENSAYOFORMATO
        /// <param name="formatocodi"></param>
        /// <param name="enunidadcodi"></param>
        /// </summary>
        public void DeleteEnEnsayoformato(int formatocodi, int enunidadcodi)
        {
            try
            {
                FactorySic.GetEnEnsayoformatoRepository().Delete(formatocodi, enunidadcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla EN_ENSAYOFORMATO
        /// <param name="formatocodi"></param>
        /// <param name="enunidadcodi"></param>
        /// </summary>
        public EnEnsayoformatoDTO GetByIdEnEnsayoformato(int formatocodi, int enunidadcodi)
        {
            return FactorySic.GetEnEnsayoformatoRepository().GetById(formatocodi, enunidadcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EN_ENSAYOFORMATO
        /// </summary>
        public List<EnEnsayoformatoDTO> ListEnEnsayoformatos()
        {
            return FactorySic.GetEnEnsayoformatoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla EnEnsayoformato
        /// </summary>
        public List<EnEnsayoformatoDTO> GetByCriteriaEnEnsayoformatos()
        {
            return FactorySic.GetEnEnsayoformatoRepository().GetByCriteria();
        }


        /// <summary>
        /// Lista todos los formatos por el ensayo indicado, agrupados y ordenados por la unidad generadora
        /// </summary>
        /// <param name="ensayocodi"></param>
        /// <returns></returns>
        public List<EnEnsayoformatoDTO> ListFormatoXEnsayoConColorFila(int ensayocodi)
        {
            List<EnEnsayoformatoDTO> lista = ListFormatoXEnsayo(ensayocodi);
            List<EnEnsayoformatoDTO> listaOrdenada = lista.OrderBy(x => x.Equinomb).ThenBy(x => x.Formatodesc).ToList(); //lista ordenada por unidad y formato            
            string color1 = "#ffffff";
            string color2 = "#E5E8E8";
            string colortemp = "";
            Boolean nocambio = true;
            if (listaOrdenada.Count > 0)
            {
                var ecodiAnterior = listaOrdenada[0].Equicodi;
                foreach (var fila in listaOrdenada)
                {
                    if (fila.Equicodi.Equals(ecodiAnterior) && nocambio)
                    {
                        fila.Colorfila = color1;
                    }
                    else
                    {
                        fila.Colorfila = color2;
                        colortemp = color1;
                        color1 = color2;
                        color2 = colortemp;
                    }
                    ecodiAnterior = fila.Equicodi;
                }
            }
            return listaOrdenada;
        }

        /// <summary>
        /// Lista todos los formatos por el ensayo indicado
        /// </summary>
        /// <param name="ensayocodi"></param>
        /// <returns></returns>
        public List<EnEnsayoformatoDTO> ListFormatoXEnsayo(int ensayocodi)
        {
            return FactorySic.GetEnEnsayoformatoRepository().ListaFormatoXEnsayo(ensayocodi);
        }


        /// <summary>
        /// Lista todos los formatos de la unidad y el ensayo indicado
        /// </summary>
        /// <param name="ensayocodi"></param>
        /// /// <param name="equicodi"></param>
        /// <returns></returns>
        public List<EnEnsayoformatoDTO> ListFormatoXEnsayo(int ensayocodi, int equicodi)
        {
            return FactorySic.GetEnEnsayoformatoRepository().ListaFormatoXEnsayo(ensayocodi, equicodi);
        }

        /// <summary>
        /// Permite listar todos los ultimos formatos de la tabla EN_ENSAYOFORMATO de una emresa y una central
        /// </summary>
        public List<EnEnsayoformatoDTO> ListEnEnsayoFormatosEmpresaCentral(int emprcodi, int equicodi)
        {
            return FactorySic.GetEnEnsayoformatoRepository().ListaFormatoXEnsayoEmpresaCtral(emprcodi, equicodi);
        }

        /// <summary>
        /// Devuelve la lista de unidades con sus formatos (si los tuviese)
        /// </summary>
        /// <returns></returns>
        public List<EnEnsayoformatoDTO> ListarUnidadesConFormatos(int idEnsayo)
        {
            return FactorySic.GetEnEnsayoformatoRepository().ListarUnidadesConFormatos(idEnsayo);
        }

        /// <summary>
        /// Devuelve el nombre del archivo segun el nro del formato
        /// </summary>
        /// <returns></returns>
        public string ObtenerNombreDelArchivo(int nroFormato, int idensayo, string fecha, string extension)
        {
            string nomb = "";
            string nombreArch = "";
            int numBuscado = nroFormato + 7; // número de formatos padre = 7
            EnFormatoDTO formato = new EnFormatoDTO();
            formato = this.GetByIdEnFormato(numBuscado);
            nombreArch = formato.Formatoprefijo;
            nomb = nombreArch + "_" + idensayo + "_" + nroFormato + "_" + fecha + "." + extension;

            return nomb;
        }

        #endregion

        #region Métodos Tabla EN_ENSAYOUNIDAD

        /// <summary>
        /// Inserta un registro de la tabla EN_ENSAYOUNIDAD
        /// </summary>
        public int SaveEnEnsayounidad(EnEnsayounidadDTO entity)
        {
            int codigo = 0;
            try
            {
                codigo = FactorySic.GetEnEnsayounidadRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return codigo;
        }

        /// <summary>
        /// Actualiza un registro de la tabla EN_ENSAYOUNIDAD
        /// </summary>
        public void UpdateEnEnsayounidad(EnEnsayounidadDTO entity)
        {
            try
            {
                FactorySic.GetEnEnsayounidadRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EN_ENSAYOUNIDAD
        /// </summary>
        public void DeleteEnEnsayounidad(int enunidadcodi)
        {
            try
            {
                FactorySic.GetEnEnsayounidadRepository().Delete(enunidadcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla EN_ENSAYOUNIDAD
        /// </summary>
        public EnEnsayounidadDTO GetByIdEnEnsayounidad(int enunidadcodi)
        {
            return FactorySic.GetEnEnsayounidadRepository().GetById(enunidadcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EN_ENSAYOUNIDAD
        /// </summary>
        public List<EnEnsayounidadDTO> ListEnEnsayounidads()
        {
            return FactorySic.GetEnEnsayounidadRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla EnEnsayounidad
        /// </summary>
        public List<EnEnsayounidadDTO> GetByCriteriaEnEnsayounidads()
        {
            return FactorySic.GetEnEnsayounidadRepository().GetByCriteria();
        }

        /// <summary>
        /// Permite obtener el ensayo de cierta unidad de ensayo
        /// </summary>
        public EnEnsayounidadDTO ObtenerEnsayoUnidad(int idensayo, int equicodi)
        {
            return FactorySic.GetEnEnsayounidadRepository().GetEnsayoUnidad(idensayo, equicodi);
        }

        /// <summary>
        /// Permite obtener los ensayosUnidad de un ensayo
        /// <param name="idEnsayo"></param>
        /// </summary>
        public List<EnEnsayounidadDTO> ObtenerUnidadesXEnsayo(int idensayo)
        {
            return FactorySic.GetEnEnsayounidadRepository().GetUnidadesXEnsayo(idensayo);
        }



        #endregion

        #region Métodos Tabla EN_FORMATO

        /// <summary>
        /// Inserta un registro de la tabla EN_FORMATO
        /// </summary>
        public void SaveEnFormato(EnFormatoDTO entity)
        {
            try
            {
                FactorySic.GetEnFormatoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla EN_FORMATO
        /// </summary>
        public void UpdateEnFormato(EnFormatoDTO entity)
        {
            try
            {
                FactorySic.GetEnFormatoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EN_FORMATO
        /// </summary>
        public void DeleteEnFormato(int formatocodi)
        {
            try
            {
                FactorySic.GetEnFormatoRepository().Delete(formatocodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla EN_FORMATO
        /// </summary>
        public EnFormatoDTO GetByIdEnFormato(int formatocodi)
        {
            return FactorySic.GetEnFormatoRepository().GetById(formatocodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EN_FORMATO
        /// </summary>
        public List<EnFormatoDTO> ListEnFormatos()
        {
            return FactorySic.GetEnFormatoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla EnFormato
        /// </summary>
        public List<EnFormatoDTO> GetByCriteriaEnFormatos()
        {
            return FactorySic.GetEnFormatoRepository().GetByCriteria();
        }


        /// <summary>
        /// Permite listar los formatos 
        /// /// </summary>
        /// <param name="valor"> -1 -> todos, 0 -> Solo los sin padre</param>
        /// </summary>
        public List<EnFormatoDTO> ListarFormatosActuales(int valor)
        {
            if (valor == 0)
                return FactorySic.GetEnFormatoRepository().ListarFormatosActuales();
            else
            {
                return FactorySic.GetEnFormatoRepository().ListarFormatosActualesTodos();
            }

        }

        /// <summary>
        /// Permite listar los 'formatos hijos' de un padre
        /// </summary>
        public List<EnFormatoDTO> ListarFormatosPorPadre(int idPadre)
        {
            return FactorySic.GetEnFormatoRepository().ListarFormatosPorPadre(idPadre);
        }

        /// <summary>
        /// Permite listar todos los formatos con una lista de sus hijos
        /// /// </summary>        
        public List<EnFormatoDTO> ListaFormatosConSusHijos()
        {
            List<EnFormatoDTO> listita = new List<EnFormatoDTO>();
            List<EnFormatoDTO> listaFormatos = new List<EnFormatoDTO>();

            listaFormatos = ListarFormatosActuales(-1);

            foreach (var formato in listaFormatos)
            {
                if (formato.Formatopadre == null)  //si  son formatos padre
                {
                    EnFormatoDTO formatoTemp = new EnFormatoDTO();
                    formatoTemp.Formatocodi = formato.Formatocodi;
                    formatoTemp.Formatodesc = formato.Formatodesc;
                    formatoTemp.Formatonumero = formato.Formatonumero;
                    formatoTemp.Formatoestado = formato.Formatoestado;
                    List<EnFormatoDTO> listaHijos = ListarFormatosPorPadre(formato.Formatocodi);
                    formatoTemp.ListaFormato = new List<EnFormatoDTO>();
                    foreach (var formatoHijo in listaHijos)
                    {
                        formatoTemp.ListaFormato.Add(formatoHijo);
                    }
                    listita.Add(formatoTemp);
                }
            }
            return listita;

        }



        /// <summary>
        /// Devuelve el total de formatos activos
        /// </summary>
        /// <returns></returns>
        public List<EnFormatoDTO> ObtenerNumeroFormatosActivos()
        {
            return FactorySic.GetEnFormatoRepository().ListarFormatosActivos();
        }

        #endregion

        #region Métodos Tabla Si_Empresa
        /// <summary>
        /// Obtiene la lista de empresas generadoras
        /// </summary>
        public List<SiEmpresaDTO> ObtenerEmpresasSEIN()
        {
            return FactorySic.GetSiEmpresaRepository().ObtenerEmpresasSEIN();
        }

        public List<SiEmpresaDTO> ObtenerEmpresasXUsuario(string userlogin)
        {
            return FactorySic.GetSiEmpresaRepository().GetByUser(userlogin);
        }

        /// <summary>
        /// Permite listar las empresas
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarEmpresasxTipoEquipos(string tipoEquipos)
        {
            return FactorySic.GetSiEmpresaRepository().ListarEmpresasxTipoEquipos(tipoEquipos, ConstantesAppServicio.ParametroDefecto);
        }

        #endregion

        #region Métodos TabLa eq_equipo

        /// <summary>
        /// Lista Centrales por Empresa 
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <returns></returns>
        public List<EqEquipoDTO> ListEqEquipoEmpresaGEN2(string emprcodi)
        {
            List<EqEquipoDTO> listaVacia = new List<EqEquipoDTO>();
            if (emprcodi != null && !emprcodi.Equals(""))
            {
                return FactorySic.GetEqEquipoRepository().ListarCentralesXEmpresaGEN2(emprcodi);
            }
            else
                return listaVacia;
        }

        /// <summary>
        /// Lista de centrales filtrados por empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public List<EqEquipoDTO> ListarCentralesXEmpresaGener(int idEmpresa)
        {
            return FactorySic.GetEqEquipoRepository().CentralesXEmpresaHorasOperacion(idEmpresa);
        }

        /// <summary>
        /// Lista de Modos de operacion por Central y Empresa
        /// </summary>
        /// <param name="idCentral"></param>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public List<PrGrupoDTO> ListarModoOperacionXCentralYEmpresa(int idCentral, int idEmpresa)
        {
            return servHO.ListarModoOperacionXCentralYEmpresa(idCentral, idEmpresa);
        }

        /// <summary>
        /// Lista equipos por ensayo
        /// </summary>
        /// <param name="equicodi"></param>
        /// <returns></returns>
        public List<EqEquipoDTO> ListEqEquipoEnsayo(string equicodi)
        {
            return FactorySic.GetEqEquipoRepository().ListarEquiposEnsayo(equicodi);
        }

        /// <summary>
        /// Lista las unidades para el ensayo (Filtradas)
        /// </summary>
        /// <param name="idEnsayo"></param>
        /// <returns></returns>
        public List<EqEquipoDTO> ListarUnidadesxEnsayo(int idEnsayo)
        {
            return FactorySic.GetEqEquipoRepository().ListarUnidadesxEnsayo(idEnsayo);
        }

        /// <summary>
        /// Obtengo un equipo por su id
        /// </summary>
        /// <param name="idEquipo"></param>
        /// <returns></returns>
        public EqEquipoDTO GetByIdEquipo(int idEquipo)
        {
            return FactorySic.GetEqEquipoRepository().GetByIdEquipo(idEquipo);
        }


        #endregion

        #region Métodos Tabla EN_ENSAYOMODEQUI

        /// <summary>
        /// Inserta un registro de la tabla EN_ENSAYOMODEQUI
        /// </summary>
        public int SaveEnEnsayomodequi(EnEnsayomodequiDTO entity)
        {
            int codigo = 0;
            try
            {
                codigo = FactorySic.GetEnEnsayomodequiRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return codigo;
        }

        /// <summary>
        /// Actualiza un registro de la tabla EN_ENSAYOMODEQUI
        /// </summary>
        public void UpdateEnEnsayomodequi(EnEnsayomodequiDTO entity)
        {
            try
            {
                FactorySic.GetEnEnsayomodequiRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EN_ENSAYOMODEQUI
        /// </summary>
        public void DeleteEnEnsayomodequi(int enmoeqcodi)
        {
            try
            {
                FactorySic.GetEnEnsayomodequiRepository().Delete(enmoeqcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla EN_ENSAYOMODEQUI
        /// </summary>
        public EnEnsayomodequiDTO GetByIdEnEnsayomodequi(int enmoeqcodi)
        {
            return FactorySic.GetEnEnsayomodequiRepository().GetById(enmoeqcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EN_ENSAYOMODEQUI
        /// </summary>
        public List<EnEnsayomodequiDTO> ListEnEnsayomodequis()
        {
            return FactorySic.GetEnEnsayomodequiRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla EnEnsayomodequi
        /// </summary>
        public List<EnEnsayomodequiDTO> GetByCriteriaEnEnsayomodequis()
        {
            return FactorySic.GetEnEnsayomodequiRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla EN_ENSAYOMODO

        /// <summary>
        /// Inserta un registro de la tabla EN_ENSAYOMODO
        /// </summary>
        public int SaveEnEnsayomodo(EnEnsayomodoDTO entity)
        {
            int codigo = 0;
            try
            {
                codigo = FactorySic.GetEnEnsayomodoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return codigo;
        }

        /// <summary>
        /// Actualiza un registro de la tabla EN_ENSAYOMODO
        /// </summary>
        public void UpdateEnEnsayomodo(EnEnsayomodoDTO entity)
        {
            try
            {
                FactorySic.GetEnEnsayomodoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EN_ENSAYOMODO
        /// </summary>
        public void DeleteEnEnsayomodo(int enmodocodi)
        {
            try
            {
                FactorySic.GetEnEnsayomodoRepository().Delete(enmodocodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla EN_ENSAYOMODO
        /// </summary>
        public EnEnsayomodoDTO GetByIdEnEnsayomodo(int enmodocodi)
        {
            return FactorySic.GetEnEnsayomodoRepository().GetById(enmodocodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EN_ENSAYOMODO
        /// </summary>
        public List<EnEnsayomodoDTO> ListEnEnsayomodos()
        {
            return FactorySic.GetEnEnsayomodoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla EnEnsayomodo
        /// </summary>
        public List<EnEnsayomodoDTO> GetByCriteriaEnEnsayomodos()
        {
            return FactorySic.GetEnEnsayomodoRepository().GetByCriteria();
        }

        /// <summary>
        /// Permite listar los ensayosmodo
        /// </summary>
        /// <returns></returns>
        public List<EnEnsayomodoDTO> ListarEnsayosModo(int idEnsayo)
        {
            return FactorySic.GetEnEnsayomodoRepository().ListarEnsayosModo(idEnsayo);
        }
        #endregion

        #region Métodos Tabla EN_ESTADOS

        /// <summary>
        /// Inserta un registro de la tabla EN_ESTADOS
        /// </summary>
        public void SaveEnEstados(EnEstadoDTO entity)
        {
            try
            {
                FactorySic.GetEnEstadosRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla EN_ESTADOS
        /// </summary>
        public void UpdateEnEstados(EnEstadoDTO entity)
        {
            try
            {
                FactorySic.GetEnEstadosRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EN_ESTADOS
        /// </summary>
        public void DeleteEnEstados()
        {
            try
            {
                FactorySic.GetEnEstadosRepository().Delete();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla EN_ESTADOS
        /// </summary>
        public EnEstadoDTO GetByIdEnEstados()
        {
            return FactorySic.GetEnEstadosRepository().GetById();
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EN_ESTADOS
        /// </summary>
        public List<EnEstadoDTO> ListEnEstadoss()
        {
            return FactorySic.GetEnEstadosRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla EnEstados
        /// </summary>
        public List<EnEstadoDTO> GetByCriteriaEnEstadoss()
        {
            return FactorySic.GetEnEstadosRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla EN_ESTENSAYO

        /// <summary>
        /// Inserta un registro de la tabla EN_ESTENSAYO
        /// </summary>
        public void SaveEnEstensayo(EnEstensayoDTO entity)
        {
            try
            {
                FactorySic.GetEnEstensayoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla EN_ESTENSAYO
        /// </summary>
        public void UpdateEnEstensayo(EnEstensayoDTO entity)
        {
            try
            {
                FactorySic.GetEnEstensayoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EN_ESTENSAYO
        /// </summary>
        public void DeleteEnEstensayo()
        {
            try
            {
                FactorySic.GetEnEstensayoRepository().Delete();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla EN_ESTENSAYO
        /// </summary>
        public EnEstensayoDTO GetByIdEnEstensayo()
        {
            return FactorySic.GetEnEstensayoRepository().GetById();
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EN_ESTENSAYO
        /// </summary>
        public List<EnEstensayoDTO> ListEnEstensayos()
        {
            return FactorySic.GetEnEstensayoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla EnEstensayo
        /// </summary>
        public List<EnEstensayoDTO> GetByCriteriaEnEstensayos()
        {
            return FactorySic.GetEnEstensayoRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla EN_ESTFORMATO

        /// <summary>
        /// Inserta un registro de la tabla EN_ESTFORMATO
        /// </summary>
        public int SaveEnEstformato(EnEstformatoDTO entity)
        {
            int codigo = 0;
            try
            {
                codigo = FactorySic.GetEnEstformatoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return codigo;
        }

        /// <summary>
        /// Actualiza un registro de la tabla EN_ESTFORMATO
        /// </summary>
        public void UpdateEnEstformato(EnEstformatoDTO entity)
        {
            try
            {
                FactorySic.GetEnEstformatoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }


        /// <summary>
        /// Elimina un registro de la tabla EN_ESTFORMATO
        /// </summary>
        public void DeleteEnEstformato(int estfmtcodi)
        {
            try
            {
                FactorySic.GetEnEstformatoRepository().Delete(estfmtcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }


        /// <summary>
        /// Permite obtener un registro de la tabla EN_ESTFORMATO
        /// </summary>
        public EnEstformatoDTO GetByIdEnEstformato(int estfmtcodi)
        {
            return FactorySic.GetEnEstformatoRepository().GetById(estfmtcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EN_ESTFORMATO
        /// </summary>
        public List<EnEstformatoDTO> ListEnEstformatos()
        {
            return FactorySic.GetEnEstformatoRepository().List();
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EN_ESTFORMATO de un Ensayo
        /// </summary>
        public List<EnEstformatoDTO> ListEnEstformatosEnsayo(int enunidadcodi, int iformatocodi)
        {
            return FactorySic.GetEnEstformatoRepository().ListFormatoXEstado(enunidadcodi, iformatocodi);
        }


        /// <summary>
        /// Permite realizar búsquedas en la tabla EnEstformato
        /// </summary>
        public List<EnEstformatoDTO> GetByCriteriaEnEstformatos()
        {
            return FactorySic.GetEnEstformatoRepository().GetByCriteria();
        }


        public void ActualizarFormato(EnEnsayoformatoDTO ArchEnvio, int formatocodi, int enunidad, string sNombreArchivo, string sNombreArchivoEnsayo, string user, string path, int iEstadoFormatoHistorial, int idensayo)
        {
            ArchEnvio.Formatocodi = formatocodi;
            ArchEnvio.Enunidadcodi = enunidad;
            ArchEnvio.Ensformatnomblogico = sNombreArchivo;
            ArchEnvio.Ensformatnombfisico = sNombreArchivoEnsayo;
            ArchEnvio.Ensformtestado = EstadoFormato.Enviado;
            ArchEnvio.Lastdate = DateTime.Now;
            ArchEnvio.Lastuser = user;

            //cambia el archivo del formato            
            FileServer.DeleteBlob(sNombreArchivo, path);

            // verificamos si existe un archivo enviado para el ensayo
            var ensayoFormato = GetByIdEnEnsayoformato(formatocodi, enunidad);
            if (ensayoFormato == null)
                SaveEnEnsayoformato(ArchEnvio);
            else
            {
                ArchEnvio.Ensformtestado = EstadoFormato.Corregido;
                ArchEnvio.Lastdate = DateTime.Now;
                ArchEnvio.Lastuser = user;
                UpdateEnEnsayoformato(ArchEnvio);
                iEstadoFormatoHistorial = EstadoFormato.Corregido;

                //actualizamos el estado del ensayo a "Autorizado" si esta en estado "Aprobado"
                var ensayo = GetByIdEnEnsayo(idensayo);
                if (ensayo.Estadocodi == EstadoEnsayo.Aprobado)
                {
                    DateTime lastDate = DateTime.Now;
                    string lastUser = user;
                    ActualizaEstadoEnsayo(idensayo, ensayo.Ensayofechaevento, EstadoEnsayo.Autorizado, lastDate, lastUser);
                }

            }
            //guardamos en el historial de formatos estado table EN_ESTFORMATO
            EnEstformatoDTO entity = new EnEstformatoDTO();
            entity.Enunidadcodi = enunidad;
            entity.Formatocodi = formatocodi;
            entity.Estadocodi = iEstadoFormatoHistorial;
            entity.Estfmtlastdate = DateTime.Now;
            entity.Estfmtlastuser = user;
            SaveEnEstformato(entity);
        }

        #endregion

        /// <summary>
        /// Devuelve una lista con los modos de operacion de  cierto ensayo
        /// </summary>
        public List<PrGrupoDTO> ListarMOXensayo(int idEnsayo)
        {
            return FactorySic.GetPrGrupoRepository().ListarMOXensayo(idEnsayo);
        }

        /// <summary>
        /// Nombre del archivo que se guardará en el sistema
        /// </summary>
        /// <param name="idensayo"></param>
        /// <param name="formatocodi"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        public string GetNombreArchivoFormato(int idensayo, int formatocodi, string extension)
        {
            int nroFormato = formatocodi;
            string fecha = DateTime.Now.ToString("yyyyMMddhhmm");
            string sNombreArchivo = "ArchFormato01" + "_" + idensayo + "_" + nroFormato + "_" + fecha + "." + extension;

            return sNombreArchivo;
        }


        /// <summary>
        /// Devuelve la ruta donde se encuentran los archivos
        /// </summary>
        public string ObtenerRutaDeArchivos()
        {
            string ruta = "";
            ruta = ConfigurationManager.AppSettings[RutaDirectorioEnsayo.Repositorio].ToString();
            return ruta;
        }

        /// <summary>
        /// Devuelve el tipo de archivo 
        /// </summary>
        /// <param name="nameFile">Archivo a consultar</param>
        /// <returns></returns>
        public string DevolvertipoArchivo(string nameFile)
        {
            string archTipo = "";

            if (nameFile.Contains(".DOC"))
            {
                archTipo = "application/vnd.ms-word";
            }
            if (nameFile.Contains(".DOCX"))
            {
                archTipo = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            }
            else if (nameFile.Contains(".PDF"))
            {
                archTipo = "application/pdf";
            }
            else if (nameFile.Contains(".XML"))
            {
                archTipo = "application/XML";
            }
            else if (nameFile.Contains(".XLS"))
            {
                archTipo = "application/vnd.ms-excel";
            }
            else if (nameFile.Contains(".XLSX"))
            {
                archTipo = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            }
            else if (nameFile.Contains(".CVS"))
            {
                archTipo = "application/CSV";
            }
            else if (nameFile.Contains(".ZIP"))
            {
                archTipo = "application/zip";
            }
            else if (nameFile.Contains(".RAR"))
            {
                archTipo = "application/x-rar-compressed";
            }
            else if (nameFile.Contains(".JPG"))
            {
                archTipo = "image/jpeg";
            }
            else if (nameFile.Contains(".GIF"))
            {
                archTipo = "image/gif";
            }

            return archTipo;
        }


    }
}
