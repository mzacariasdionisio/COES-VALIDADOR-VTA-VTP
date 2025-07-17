using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.CortoPlazo.Helper;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using log4net;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;

namespace COES.Servicios.Aplicacion.CortoPlazo
{
    public class McpAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(McpAppServicio));
        
        #region tabla CP_FUENTEGAMS

        public CpFuentegamsDTO GetByIdCpFuentesgams(int indgsm)
        {
            return FactorySic.GetCpFuentegamsRepository().GetById(indgsm);
        }

        #endregion

        #region Métodos Tabla EQ_CIRCUITO

        /// <summary>
        /// Obtiene circuito por su equicodi de la tabla EQ_CIRCUITO
        /// </summary>
        public EqCircuitoDTO ObtenerCircuitoPorEquicodi(int equicodi)
        {
            return FactorySic.GetEqCircuitoRepository().GetByEquicodi(equicodi);
        }

        /// <summary>
        /// Devuelve la lista con los cuircuitos deacuerdo a los circodis
        /// </summary>
        public List<EqCircuitoDTO> ListarCircuitosPorCircodi(string lstCircodis)
        {
            return FactorySic.GetEqCircuitoRepository().ObtenerListaCircuitos(lstCircodis);
        }

        /// <summary>
        /// Inserta un registro de la tabla EQ_CIRCUITO
        /// </summary>
        public int SaveEqCircuito(EqCircuitoDTO entity)
        {
            try
            {
                return FactorySic.GetEqCircuitoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla EQ_CIRCUITO
        /// </summary>
        public void UpdateEqCircuito(EqCircuitoDTO entity)
        {
            try
            {
                FactorySic.GetEqCircuitoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EQ_CIRCUITO
        /// </summary>
        public void DeleteEqCircuito(int circodi, string username)
        {
            try
            {
                FactorySic.GetEqCircuitoRepository().Delete(circodi);
                FactorySic.GetEqCircuitoRepository().Delete_UpdateAuditoria(circodi, username);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla EQ_CIRCUITO
        /// </summary>
        public EqCircuitoDTO GetByIdEqCircuito(int circodi)
        {
            return FactorySic.GetEqCircuitoRepository().GetById(circodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EQ_CIRCUITO
        /// </summary>
        public List<EqCircuitoDTO> ListEqCircuitos()
        {
            return FactorySic.GetEqCircuitoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla EqCircuito
        /// </summary>
        public List<EqCircuitoDTO> GetByCriteriaEqCircuitos(string emprcodi, string listaEquicodi, int estado)
        {
            if (!string.IsNullOrEmpty(emprcodi))
            {
                var lista = FactorySic.GetEqCircuitoRepository().GetByCriteria(emprcodi, listaEquicodi, estado).OrderBy(x => x.Emprnomb).ThenBy(x => x.Equinomb).ThenByDescending(x => x.Circestado).ToList();

                foreach (var reg in lista)
                {
                    reg.UltimaModificacionFechaDesc = reg.Circusumodificacion != null ? reg.Circfecmodificacion.Value.ToString(ConstantesAppServicio.FormatoFechaFull2) : reg.Circfeccreacion.Value.ToString(ConstantesAppServicio.FormatoFechaFull2);
                    reg.UltimaModificacionUsuarioDesc = reg.Circusumodificacion != null ? reg.Circusumodificacion : reg.Circusucreacion;
                    reg.Emprnomb = reg.Emprnomb != null ? reg.Emprnomb.Trim() : string.Empty;
                    reg.Equinomb = reg.Equinomb != null ? reg.Equinomb.Trim() : string.Empty;
                    reg.CircestadoDesc = reg.Circestado == ConstantesMcp.CircuitoEstadoActivo ? "Activo" : "Baja";
                }

                return lista;
            }

            return new List<EqCircuitoDTO>();
        }

        #endregion

        #region Métodos Tabla EQ_CIRCUITO_DET

        /// <summary>
        /// Retorna la lista de CircuitosDet que tiene un equipo (equicodi)
        /// </summary>
        /// <param name="equicodi"></param>
        /// <returns></returns>
        private List<EqCircuitoDetDTO> ObtenerListaDependientes(int equicodi)
        {
            return FactorySic.GetEqCircuitoDetRepository().GetDependientesAgrupados(equicodi);
        }

        /// <summary>
        /// Inserta un registro de la tabla EQ_CIRCUITO_DET
        /// </summary>
        public void SaveEqCircuitoDet(EqCircuitoDetDTO entity)
        {
            try
            {
                FactorySic.GetEqCircuitoDetRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla EQ_CIRCUITO_DET
        /// </summary>
        public void UpdateEqCircuitoDet(EqCircuitoDetDTO entity)
        {
            try
            {
                FactorySic.GetEqCircuitoDetRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EQ_CIRCUITO_DET
        /// </summary>
        public void DeleteEqCircuitoDet(int circdtcodi)
        {
            try
            {
                FactorySic.GetEqCircuitoDetRepository().Delete(circdtcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla EQ_CIRCUITO_DET
        /// </summary>
        public EqCircuitoDetDTO GetByIdEqCircuitoDet(int circdtcodi)
        {
            return FactorySic.GetEqCircuitoDetRepository().GetById(circdtcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EQ_CIRCUITO_DET
        /// </summary>
        public List<EqCircuitoDetDTO> ListEqCircuitoDets()
        {
            return FactorySic.GetEqCircuitoDetRepository().List();
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EQ_CIRCUITO_DET
        /// </summary>
        public List<EqCircuitoDetDTO> ObtenerCircuitoDetPorCircodi(string lstCircodis)
        {
            return FactorySic.GetEqCircuitoDetRepository().ObtenerListaCircuitosDet(lstCircodis);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla EqCircuitoDet
        /// </summary>
        public List<EqCircuitoDetDTO> GetByCriteriaEqCircuitoDets(int circodi, string listaEquicodi, int estado)
        {
            var lista = FactorySic.GetEqCircuitoDetRepository().GetByCriteria(circodi, listaEquicodi, estado).OrderBy(x => x.Circdtagrup).ToList();

            foreach (var reg in lista)
            {
                reg.TipoDet = reg.Circodihijo != null ? ConstantesMcp.TipoHijoCircuito : ConstantesMcp.TipoHijoEquipo;
                reg.Nombre = reg.Circodihijo != null ? reg.Circnombhijo : reg.Equinombhijo;
                reg.Empresa = reg.Circodihijo != null ? reg.Emprnombcirchijo : reg.Emprnombequihijo;

                reg.Nombre = reg.Nombre != null ? reg.Nombre.Trim() : string.Empty;
                reg.Empresa = reg.Empresa != null ? reg.Empresa.Trim() : string.Empty;

                reg.FechaVigencia = reg.Circdtfecvigencia.ToString(ConstantesAppServicio.FormatoFecha);
                reg.UltimaModificacionFechaDesc = reg.Circdtusumodificacion != null ? reg.Circdtfecmodificacion.Value.ToString(ConstantesAppServicio.FormatoFechaFull2) : reg.Circdtfeccreacion.Value.ToString(ConstantesAppServicio.FormatoFechaFull2);
                reg.UltimaModificacionUsuarioDesc = reg.Circdtusumodificacion != null ? reg.Circdtusumodificacion : reg.Circdtusucreacion;
                reg.UltimaModificacionUsuarioDesc = reg.UltimaModificacionUsuarioDesc != null ? reg.UltimaModificacionUsuarioDesc : string.Empty;
            }

            return lista;
        }

        #endregion

        #region Metodos tabla EVE_MANTTO

        public List<EveManttoDTO> ListManttoEquipoFecha(string idsEquipo, int evenClaseCodi, DateTime dfecha)
        {

            return FactorySic.GetEveManttoRepository().ObtenerManttoEquipo(idsEquipo, evenClaseCodi, dfecha, dfecha.AddDays(1));
        }
        #endregion

        #region Dependencia de Equipos 

        /// <summary>
        /// Registrar / actualizar circuito y su detalle
        /// </summary>
        /// <param name="regCircuito"></param>
        /// <param name="listaDetalleUser"></param>
        /// <param name="usuario"></param>
        public void RegistrarCircuito(EqCircuitoDTO regCircuito, List<EqCircuitoDetDTO> listaDetalleUser, string usuario, out bool HayBucle)
        {
            DateTime fecha = DateTime.Now;

            if (regCircuito.Circodi > 0)
            {
                EqCircuitoDTO regCircuitoBD = this.GetByIdEqCircuito(regCircuito.Circodi);

                if (regCircuito.Circnomb != regCircuitoBD.Circnomb)
                {
                    regCircuitoBD.Circnomb = regCircuito.Circnomb;
                    regCircuitoBD.Circfecmodificacion = fecha;
                    regCircuitoBD.Circusumodificacion = usuario;

                    this.UpdateEqCircuito(regCircuitoBD);
                }
            }
            else
            {
                regCircuito.Circestado = ConstantesMcp.CircuitoEstadoActivo;
                regCircuito.Circfeccreacion = DateTime.Now;
                regCircuito.Circusucreacion = usuario;

                regCircuito.Circodi = this.SaveEqCircuito(regCircuito);
            }

            //BD
            List<int> listaDetcodi = listaDetalleUser.Select(x => x.Circdtcodi).ToList();
            List<EqCircuitoDetDTO> listaDetBD = this.GetByCriteriaEqCircuitoDets(regCircuito.Circodi, ConstantesAppServicio.ParametroDefecto, ConstantesMcp.CircuitoEstadoTodos);
            var listaSelect = listaDetBD.Where(x => listaDetcodi.Contains(x.Circdtcodi)).ToList();
            var listaNoSelect = listaDetBD.Where(x => !listaDetcodi.Contains(x.Circdtcodi)).ToList();

            //Actualizar
            foreach (var select in listaSelect)
            {
                var regMemoria = listaDetalleUser.Find(x => x.Circdtcodi == select.Circdtcodi);
                var fechaV = DateTime.ParseExact(regMemoria.FechaVigencia + " 00:00:00", ConstantesAppServicio.FormatoFechaFull2, CultureInfo.InvariantCulture);
                regMemoria.Circdtfecvigencia = fechaV;
                if (select.Circdtestado != ConstantesMcp.CircuitoEstadoActivo || select.Circdtagrup != regMemoria.Circdtagrup || select.Circdtfecvigencia != regMemoria.Circdtfecvigencia)
                {
                    select.Circdtagrup = regMemoria.Circdtagrup;
                    //select.Circdtestado = ConstantesMcp.CircuitoEstadoActivo;
                    select.Circdtfecmodificacion = fecha;
                    select.Circdtusumodificacion = usuario;
                    var fechaActualizada = DateTime.ParseExact(regMemoria.FechaVigencia + " 00:00:00", ConstantesAppServicio.FormatoFechaFull2, CultureInfo.InvariantCulture);
                    select.Circdtfecvigencia = fechaActualizada;
                    select.Circdtestado = regMemoria.Circdtestado;

                    this.UpdateEqCircuitoDet(select);
                }
            }

            //Eliminar
            foreach (var noselect in listaNoSelect)
            {
                noselect.Circdtestado = ConstantesMcp.CircuitoEstadoInactivo;
                noselect.Circdtfecmodificacion = fecha;
                noselect.Circdtusumodificacion = usuario;

                this.UpdateEqCircuitoDet(noselect);
            }

            //////////////////////////////////////////////////////////////////////////////////////
            //Nuevo
            var listaRegistrados = listaDetBD.Select(x => x.Circdtcodi).ToList();
            var listaNuevo = listaDetalleUser.Where(x => !listaRegistrados.Contains(x.Circdtcodi)).ToList(); //los nuevo vienen con id negativo
            DateTime fecVigenciaRegistro = DateTime.Now;
            foreach (var equicodi in listaNuevo)
            {
                EqCircuitoDetDTO reg = new EqCircuitoDetDTO();
                reg.Circodi = regCircuito.Circodi;
                reg.Circdtfeccreacion = fecha;
                reg.Circdtusucreacion = usuario;
                reg.Circdtestado = ConstantesMcp.CircuitoEstadoActivo;
                reg.Equicodihijo = equicodi.Equicodihijo > 0 ? equicodi.Equicodihijo : null;
                reg.Circodihijo = equicodi.Circodihijo > 0 ? equicodi.Circodihijo : null;
                reg.Circdtagrup = equicodi.Circdtagrup;
                DateTime fecV = DateTime.ParseExact(equicodi.FechaVigencia + " 00:00:00", ConstantesAppServicio.FormatoFechaFull2, CultureInfo.InvariantCulture);
                reg.Circdtfecvigencia = fecV;
                fecVigenciaRegistro = fecV;
                this.SaveEqCircuitoDet(reg);
            }

            //Verificar si existen bucles en el circuito
            HayBucle = VerificarBuclesEnRed(regCircuito.Circodi, fecVigenciaRegistro);

        }

        /// <summary>
        /// Indica si el circuito presentado presenta bucles o no
        /// </summary>
        /// <returns></returns>
        private bool VerificarBuclesEnRed(int miCircodi, DateTime fechaVigencia)
        {
            bool HayBucles = false;
            List<int> lstCircodisUsados = new List<int>();
            List<int> ListaCircodisEnRed = new List<int>();
            List<EqCircuitoDetDTO> listaCircuitosDetBD = ListEqCircuitoDets();
            ListaCircodisEnRed = ObtenerCircuitosEnRed(miCircodi, fechaVigencia, ref lstCircodisUsados, ref HayBucles, listaCircuitosDetBD);
            return HayBucles;
        }

        /// <summary>
        /// Devuelve un registro MeMedicion48 con la disponibilidad de un equipo cada 30minutos (null: disponible    -    1: en mantenimiento)
        /// </summary>
        /// <param name="idDelEquipo"></param>
        /// <param name="matrizMantenimento"></param>
        /// <returns></returns>
        public MeMedicion48DTO ObtenerDisponibilidadPorEquipo(int idDelEquipo, List<MeMedicion48DTO> matrizMantenimento, DateTime fechaVigencia, List<EqCircuitoDTO> lstCircuitosBD, List<EqCircuitoDetDTO> lstCircuitosDetBD, ref List<int> listaMantenimientosDentro)
        {
            MeMedicion48DTO vectorDisp = new MeMedicion48DTO();

            vectorDisp.Equicodi = idDelEquipo;

            //Ingreso los valores para los Hx, según su disponibilidad
            for (int i = 1; i <= 48; i++)
            {
                List<int> lstCircodisUsados = new List<int>();
                List<int> lista1 = new List<int>();
                int caminosDisponibles2 = 0;
                int caminosDisponibles = EncontrarNumeroRutasDisponibles(idDelEquipo, i, matrizMantenimento, fechaVigencia, ref lstCircodisUsados, lstCircuitosBD, lstCircuitosDetBD, ref listaMantenimientosDentro, ref lista1);

                #region Mezcla de formas de Agrupacion
                //si encuentra circuitos dentro del circuito
                if (lista1.Any())
                {
                    var lstNegativos = lista1.Where(x => x == -1).ToList();
                    int val = MultiplicarLista(lista1);
                    if (lstNegativos.Any()) //hubo un  "-1" dentro, quiere decir que hay circuito dentro del circuito
                    {
                        caminosDisponibles2 = -val;
                        caminosDisponibles = caminosDisponibles2;
                    }
                }
                #endregion



                /* ESTADO = NULL : Sin mantenimiento, hay disponibilidad */
                /* ESTADO = 1 : Hay mantenimiento, no hay disponibilidad */
                decimal? estado = -1;
                if (caminosDisponibles > 0 || caminosDisponibles == -1) // existen rutas disponibles en la red para "idDelEquipo"
                {
                    estado = null;
                }
                else //no hay rutas disponibles, probablemente hubo mantenimiento (caminosDisponibles=0)
                {
                    estado = 1;
                }
                vectorDisp.GetType().GetProperty("H" + i.ToString()).SetValue(vectorDisp, estado);
            }

            return vectorDisp;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lista"></param>
        /// <returns></returns>
        private int MultiplicarLista(List<int> lista)
        {
            int val = -1;
            if (lista.Any())
            {
                val = 1;
                foreach (var item in lista)
                {
                    val = val * item;
                }
            }
            return val;
        }

        /// <summary>
        /// Devuelve la lista de circuitos (circodis), ACTIVOS Y que existen hasta FECHAVIGENCIA, dentro de un circuito (circodi), 
        /// </summary>
        /// <param name="circodi"></param>
        /// <param name="lstCircodisUsados"></param>
        /// <returns></returns>
        public List<int> ObtenerCircuitosEnRed(int circodi, DateTime fechaVigencia, ref List<int> lstCircodisUsados, ref bool HayBucles, List<EqCircuitoDetDTO> lstCircuitosDetBD)
        {
            lstCircodisUsados.Add(circodi);
            List<int> listaCircodis = new List<int>();
            List<int> listaVacia = new List<int>();

            /**List<EqCircuitoDetDTO> listaCircuitosDetGeneral = ObtenerCircuitoDetPorCircodi(circodi.ToString());**/
            List<EqCircuitoDetDTO> listaCircuitosDetGeneral = lstCircuitosDetBD.Where(x => x.Circodi == circodi).ToList();

            //filtramos la lista, solo activos y con fechaVigencia menores 
            List<EqCircuitoDetDTO> listaCircuitosDet = listaCircuitosDetGeneral.Where(x => x.Circdtfecvigencia <= fechaVigencia && x.Circdtestado == 1).ToList();

            if (listaCircuitosDet.Any())
            {
                foreach (var circuitoDet in listaCircuitosDet)
                {
                    if (circuitoDet.Circodihijo != null)
                    {

                        var circodiYaUsado = lstCircodisUsados.Where(x => x == circuitoDet.Circodihijo.Value);
                        if (circodiYaUsado.Count() > 0) //Hay bucles, retorna lista vacios
                        {
                            //return null;
                            HayBucles = true;
                        }
                        else
                        {
                            listaCircodis.Add(circuitoDet.Circodihijo.Value);
                            var circuitosHijo = ObtenerCircuitosEnRed(circuitoDet.Circodihijo.Value, fechaVigencia, ref lstCircodisUsados, ref HayBucles, lstCircuitosDetBD);
                            if (circuitosHijo == null) //hubo bucle
                            {
                                //return null;
                                HayBucles = true;
                            }
                            else
                            {
                                listaCircodis.AddRange(ObtenerCircuitosEnRed(circuitoDet.Circodihijo.Value, fechaVigencia, ref lstCircodisUsados, ref HayBucles, lstCircuitosDetBD));
                            }

                        }

                    }
                }
            }
            return listaCircodis;
        }

        /// <summary>
        /// Devuelve los equicodis de todos los equipos (ACTIVOS y que existan hasta FECHAVIGENCIA) involucrados en la red (incluye el equipo inicio)
        /// </summary>
        /// <param name="equicodi"></param>
        /// <returns></returns>
        public List<int> ObtenerEquiposEnCircuito(int equicodi, DateTime fechaVigencia, List<EqCircuitoDTO> lstCircuitosBD, List<EqCircuitoDetDTO> lstCircuitosDetBD)
        {
            List<int> listita = new List<int>();
            List<int> ListaCircodisEnRed = new List<int>();
            var circuito = lstCircuitosBD.Where(x => x.Equicodi == equicodi).ToList();

            bool HayBucles = false;

            if (circuito.Count() > 0)
            {
                int circodi = circuito.First().Circodi;
                List<int> lstCircodisUsados = new List<int>();
                ListaCircodisEnRed = ObtenerCircuitosEnRed(circodi, fechaVigencia, ref lstCircodisUsados, ref HayBucles, lstCircuitosDetBD);
                if (HayBucles == true) //presenta bucles
                {
                    //return null;
                }
                ListaCircodisEnRed = ListaCircodisEnRed.Distinct().ToList();
                ListaCircodisEnRed.Add(circodi); //agregamos la del generador

                /*hallamos los equicodis delos circuitos ACTIVOS dentro de red, circuitos existentes hasta dicha fechaVigencia*/
                List<EqCircuitoDetDTO> listaCircuitosDetGeneral = lstCircuitosDetBD.Where(x => ListaCircodisEnRed.Contains(x.Circodi)).ToList();
                //filtramos la lista, solo activos y con fechaVigencia menores 
                List<EqCircuitoDetDTO> listaCircuitosDet = listaCircuitosDetGeneral.Where(x => x.Circdtfecvigencia <= fechaVigencia && x.Circdtestado == 1).ToList();

                var circuitos = listaCircuitosDet.Select(x => x.Equicodihijo).Distinct().ToList();
                var circuitosSinNulos = circuitos.Where(x => x != null).ToList();
                var circuitosDela = circuitos.Where(x => x != null).ToList();
                List<int> equiposCircuitos1 = circuitosSinNulos.Select(x => x.Value).Distinct().ToList();

                //agregamos los equicodis de los circuitos cabecera
                List<EqCircuitoDTO> listaCircuitosCabecera = lstCircuitosBD.Where(x => ListaCircodisEnRed.Contains(x.Circodi)).ToList();
                var equiposCircuitos2 = listaCircuitosCabecera.Select(x => x.Equicodi).Distinct().ToList();

                //unimos ambas listas y quitamos duplicidad
                equiposCircuitos1.AddRange(equiposCircuitos2);
                listita = equiposCircuitos1;
            }
            listita.Add(equicodi); //agrego cabecera (caso circuito == null)
            listita = listita.Distinct().OrderBy(x => x).ToList();
            return listita;
        }

        /// <summary>
        /// Encuentra el numero de rutas disponibles (sin mantenimientos, que esten ACTIVOS y que dichos equipos existan hasta FECHAVIGENCIA) desde un equipo generador
        /// </summary>
        /// <param name="equicodi"></param>
        /// <param name="posHx"></param>
        /// <param name="matrizDisponibilidades"></param>
        /// <param name="fechaVigencia"></param>
        /// <param name="lstCircodisUsados"></param>
        /// <param name="lstCircuitosBD"></param>
        /// <param name="lstCircuitosDetBD"></param>
        /// <param name="listaMantenimientosDentro">Si hay mantenimiento, guarda el manttoCodi respectivo</param> 
        /// <param name="lista"> Guarda los caminnos encontrados en cada nivel para el metodo que usa Coes para designar circdtagrup</param>
        /// <returns></returns>
        public int EncontrarNumeroRutasDisponibles(int equicodi, int posHx, List<MeMedicion48DTO> matrizDisponibilidades, DateTime fechaVigencia, ref List<int> lstCircodisUsados, List<EqCircuitoDTO> lstCircuitosBD, List<EqCircuitoDetDTO> lstCircuitosDetBD, ref List<int> listaMantenimientosDentro, ref List<int> lista)
        {
            int numCaminos = 0;
            List<int> numResultados = new List<int>();

            int estadoEquipoCabeza = VerificarEstado(equicodi, posHx, matrizDisponibilidades, ref listaMantenimientosDentro);

            if (estadoEquipoCabeza != 0)
            {
                // Obtenemos la lista de circuitosDet que son parte del equipo 
                List<EqCircuitoDetDTO> equiposDebajoGeneral = new List<EqCircuitoDetDTO>();
                List<EqCircuitoDTO> lstCircuitosIgualEquicodi = lstCircuitosBD.Where(x => x.Equicodi == equicodi).ToList();
                List<int> lstCircodisIgualEquicodi = lstCircuitosIgualEquicodi.Select(x => x.Circodi).Distinct().ToList();
                equiposDebajoGeneral = lstCircuitosDetBD.Where(x => lstCircodisIgualEquicodi.Contains(x.Circodi)).ToList();

                //filtrar solo  equipos ACTIVOS y que existan hasta FECHAVIGENCIA
                List<EqCircuitoDetDTO> equiposDebajo = equiposDebajoGeneral.Where(x => x.Circdtfecvigencia <= fechaVigencia && x.Circdtestado == 1).ToList();

                int resultadoParcial = 0;
                int resultadoParcialAntiguo = 1;
                if (equiposDebajo.Any())
                {
                    /*obtener lista segun su agrupacion*/
                    List<int> lstAgrupaciones = equiposDebajo.Select(x => x.Circdtagrup).Distinct().OrderBy(x => x).ToList();

                    foreach (int numNivel in lstAgrupaciones)
                    {
                        bool purosHijos = false;
                        //Equipos con el mismo numero de AGRUPACIÓN
                        List<EqCircuitoDetDTO> equiposNivel = equiposDebajo.Where(x => x.Circdtagrup == numNivel).ToList();

                        //vemos si alguno de esta listadeAGRUPADOS tiene un circuito (si no tiene ningun circuito, solo tendrá equicodis)
                        List<EqCircuitoDetDTO> equiposConCircuitos = equiposNivel.Where(x => x.Circodihijo != null).ToList();
                        int numCircuitosDentroAgrupacion = equiposConCircuitos.Count();

                        #region Recorrido
                        if (numCircuitosDentroAgrupacion > 0) //La agrupacion con nivel "numNivel" tiene algun circodihijos dentro
                        {
                            int numEquihijosDentroAgrupacion = equiposNivel.Count() - numCircuitosDentroAgrupacion;

                            if (numEquihijosDentroAgrupacion == 1)
                            {
                                int valor1 = 1;
                                int valor2 = 1;
                                int resultado = 0;

                                if (numEquihijosDentroAgrupacion > 1)
                                {
                                    valor2 = 0;
                                }
                                foreach (EqCircuitoDetDTO eqCircDet in equiposNivel)
                                {
                                    lstCircodisUsados.Add(eqCircDet.Circodi);
                                    if (eqCircDet.Circodihijo != null)
                                    {
                                        var circodiYaUsado = lstCircodisUsados.Where(x => x == eqCircDet.Circodihijo.Value);
                                        if (circodiYaUsado.Count() > 0) //Hay bucles, retorna lista vacios
                                        {
                                            //return null;
                                            var HayBucles = true;
                                            //valor2 = 0; //no debe agregar mas caminos
                                        }
                                        else
                                        {
                                            int equicodiCircuitoHijo = lstCircuitosBD.Find(x => x.Circodi == eqCircDet.Circodihijo.Value).Equicodi;
                                            List<int> lis = new List<int>();
                                            valor1 = EncontrarNumeroRutasDisponibles(equicodiCircuitoHijo, posHx, matrizDisponibilidades, fechaVigencia, ref lstCircodisUsados, lstCircuitosBD, lstCircuitosDetBD, ref listaMantenimientosDentro, ref lis);

                                        }
                                    }
                                    else
                                    {
                                        if (eqCircDet.Equicodihijo != null)
                                        {
                                            valor2 = VerificarEstado(eqCircDet.Equicodihijo.Value, posHx, matrizDisponibilidades, ref listaMantenimientosDentro);
                                        }
                                    }
                                }

                                resultado = valor1 * valor2;
                                //resultadoParcial = resultadoParcial + resultado;
                                resultadoParcial = resultado;
                            }
                            else
                            {

                                if (numEquihijosDentroAgrupacion > 1)
                                {
                                    int valor1 = 1;
                                    int valor2 = 0;
                                    int resultado = 0;

                                    if (numEquihijosDentroAgrupacion > 1)
                                    {
                                        valor2 = 0;
                                    }
                                    foreach (EqCircuitoDetDTO eqCircDet in equiposNivel)
                                    {
                                        lstCircodisUsados.Add(eqCircDet.Circodi);
                                        if (eqCircDet.Circodihijo != null)
                                        {
                                            var circodiYaUsado = lstCircodisUsados.Where(x => x == eqCircDet.Circodihijo.Value);
                                            if (circodiYaUsado.Count() > 0) //Hay bucles, retorna lista vacios
                                            {
                                                //return null;
                                                var HayBucles = true;
                                                //valor2 = 0; //no debe agregar mas caminos
                                            }
                                            else
                                            {
                                                int equicodiCircuitoHijo = lstCircuitosBD.Find(x => x.Circodi == eqCircDet.Circodihijo.Value).Equicodi;
                                                List<int> lis = new List<int>();
                                                valor1 = EncontrarNumeroRutasDisponibles(equicodiCircuitoHijo, posHx, matrizDisponibilidades, fechaVigencia, ref lstCircodisUsados, lstCircuitosBD, lstCircuitosDetBD, ref listaMantenimientosDentro, ref lis);

                                            }
                                        }
                                        else
                                        {
                                            if (eqCircDet.Equicodihijo != null)
                                            {
                                                valor2 = valor2 + VerificarEstado(eqCircDet.Equicodihijo.Value, posHx, matrizDisponibilidades, ref listaMantenimientosDentro);
                                            }
                                        }
                                    }

                                    resultado = valor1 + valor2;
                                    //resultadoParcial = resultadoParcial + resultado;
                                    resultadoParcial = resultado;
                                    numResultados.Add(-1); //para saber que hay circuito dentro del circuito
                                    numResultados.Add(resultadoParcial);

                                }
                            }



                        }
                        else //La agrupacion con nivel "numNivel" solo tiene equicodisHijos 
                        {
                            purosHijos = true;
                            int valor1 = 1;
                            int resultado = 0;
                            foreach (EqCircuitoDetDTO eqCircDet in equiposNivel)
                            {
                                valor1 = VerificarEstado(eqCircDet.Equicodihijo.Value, posHx, matrizDisponibilidades, ref listaMantenimientosDentro);
                                resultado = resultado + valor1;
                            }
                            resultadoParcialAntiguo = resultadoParcialAntiguo * resultado;

                        }


                        if (purosHijos)
                        {
                            resultadoParcial = resultadoParcialAntiguo;
                            numResultados.Add(resultadoParcial);
                        }

                        #endregion

                        //pasa al sigte nivel


                    }

                    //pasaron todos los niveles
                }
                else //no tiene hijos, es unico
                {
                    resultadoParcial = -1;
                    resultadoParcialAntiguo = -1;
                }

                numCaminos = resultadoParcial;
                lista = numResultados;

            }

            return numCaminos;

        }

        /// <summary>
        /// Verifica si el equipo (equicodi) esta en mantinimiento "0" en la posHx de su vectorDisponibilidad
        /// </summary>
        /// <param name="equicodi"></param>
        /// <param name="posHx"></param>
        /// <param name="matrizDisponibilidades"></param>
        /// <returns></returns>
        public int VerificarEstado(int equicodi, int posHx, List<MeMedicion48DTO> matrizDisponibilidades, ref List<int> listaMantenimientosDentro) // 1: EQUIPO ACTIVO     0: EQUIPO EN MANTENIMIENTO
        {
            int estado = 1;

            if (matrizDisponibilidades.Any())
            {
                List<MeMedicion48DTO> vectoresDisponibilidadesEquipo = matrizDisponibilidades.Where(x => x.Equicodi == equicodi).ToList(); //si existe, solo sera uno

                if (vectoresDisponibilidadesEquipo.Any())
                {
                    MeMedicion48DTO vectorDisponibilidad = vectoresDisponibilidadesEquipo.First();

                    //obtenemos el valor "estado" para la (posicion = posHx) del vector
                    //valorEstado contiene el manttoCodi del mantenimiento, si no tiene mantenimiento es NULL 
                    decimal? valorEstado = (decimal?)vectorDisponibilidad.GetType().GetProperty("H" + posHx.ToString()).GetValue(vectorDisponibilidad, null);

                    if (valorEstado == null) // No existe mantenimiento para equipo, esta disponible dicho equipo
                    {
                        estado = 1;
                    }
                    else
                    {
                        //si valorEstado = manttoCodi > 0, existe mantenimiento
                        if (valorEstado > 0) // Hay mantenimiento para el equipo, dicho equipo no esta disponible (estado = 0)
                        {
                            estado = 0;

                            //Agregamos el manttoCodi a la lista (usada para mostrar los mantenimientos totales)
                            listaMantenimientosDentro.Add(Decimal.ToInt32(valorEstado.Value));
                        }
                    }
                }
            }
            return estado;

        }

        /// <summary>
        /// Genera la matriz con los mantenimientos de todos los equipos pasados por parametros
        /// </summary>
        /// <param name="equiposEnLaRed"></param>
        /// <param name="listaMantenimientoAux"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> ObtenerMantenimientosEquiposDependientes(List<int> equiposEnLaRed, List<EveManttoDTO> listaMantenimientoAux)
        {
            List<MeMedicion48DTO> matrizMantenimento = new List<MeMedicion48DTO>(); // Matriz de horas de mantenimeinto de todos los equipos conectados al equipo principal
            MeMedicion48DTO entity;
            List<EveManttoDTO> mantos;

            // buscamos mantenimiento de los equipos asociados
            foreach (var obj in equiposEnLaRed)
            {
                entity = new MeMedicion48DTO();
                entity.Equicodi = obj;
                mantos = listaMantenimientoAux.Where(x => x.Equicodi == obj).ToList();
                foreach (var obj2 in mantos) // buscamos todos los mantenimientos para el equipo asociado
                {
                    int diaini = ((DateTime)obj2.Evenini).Day;
                    int diafin = ((DateTime)obj2.Evenfin).Day;
                    var horainicio = ((DateTime)obj2.Evenini).Hour;
                    var horafin = ((DateTime)obj2.Evenfin).Hour;
                    var mininicio = ((DateTime)obj2.Evenini).Minute;
                    var minfin = ((DateTime)obj2.Evenfin).Minute;

                    int posDia = diaini < diafin ? 1 : 0;
                    int salto = mininicio > 30 ? 2 : 1;
                    var posIni = 2 * horainicio + salto;
                    salto = minfin > 30 ? 1 : 0;
                    var posFin = 2 * horafin + salto;
                    if (horafin == 0)
                    {
                        if (posDia == 1)
                        {
                            horafin = 23;
                            minfin = 59;
                            posFin = 48;
                        } else posIni = 0;
                    }
                    for (int indice = posIni; indice <= posFin; indice++)
                    {
                        if (indice > 0)
                        {
                            //enviamos el manttoCodi si hay mantenimeinto, si obj2.Manttocodi != null entonces funcion VerificarEstado = 0;
                            entity.GetType().GetProperty("H" + (indice).ToString()).SetValue(entity, (decimal?)obj2.Manttocodi); // 1: hora encendida
                            //entity.GetType().GetProperty("H" + (indice).ToString()).SetValue(entity, (decimal?)1); // 1: hora encendida
                        }

                    }
                }
                matrizMantenimento.Add(entity);
            }
            return matrizMantenimento;
        }



        /// <summary>
        /// Asignar codigo de circuito a los equipos
        /// </summary>
        /// <param name="listaEq"></param>
        public void AsignarCircuitoToEquipos(List<EqEquipoDTO> listaEq)
        {
            List<int> listaEquicodi = listaEq.Where(x => x.Equicodi > 0).Select(x => x.Equicodi).ToList();
            if (listaEquicodi.Any())
            {
                List<EqCircuitoDTO> listaCirc = this.GetByCriteriaEqCircuitos(ConstantesAppServicio.ParametroDefecto, string.Join(",", listaEquicodi), ConstantesMcp.CircuitoEstadoActivo);
                foreach (var reg in listaEq)
                {
                    EqCircuitoDTO circ = listaCirc.Find(x => x.Equicodi == reg.Equicodi);
                    reg.Circodi = circ != null ? circ.Circodi : 0;
                    reg.Circnomb = circ != null ? circ.Circnomb : string.Empty;
                }
            }
        }
        #endregion

        #region Metodos Auxiliares

        /// <summary>
        /// Permite obtener un registro de la tabla CP_TOPOLOGIA final por fecha
        /// </summary>
        public CpTopologiaDTO ObtenerTopologiaFinalPorFecha(DateTime topfecha, string toptipo, int periodo)
        {
            CpTopologiaDTO result = null;

            CpTopologiaDTO entity = FactorySic.GetCpTopologiaRepository().GetByFechaTopfinal(topfecha, toptipo);

            List<CpReprogramaDTO> listaReprogramas = FactorySic.GetCpReprogramaRepository().
               GetByCriteria(entity.Topcodi).OrderByDescending(x => x.Reprogorden).ToList();

            List<CpReprogramaDTO> listaOficiales = listaReprogramas.Where(x => x.Tophorareprog <= periodo && !string.IsNullOrEmpty(x.Topouserdespacho)).
               OrderByDescending(x => x.Reprogorden).ToList();

            if (listaOficiales.Count() > 0)
            {
                result = FactorySic.GetCpTopologiaRepository().GetById((int)listaOficiales[0].Topcodi2);
                result.HoraReprograma = listaOficiales[0].Tophorareprog;
            }
            //else if (listaReprogramas.Count() > 0)
            //{
            //    result = FactorySic.GetCpTopologiaRepository().GetById((int)listaReprogramas[0].Topcodi2);
            //}
            else
            {
                result = entity;
                result.HoraReprograma = 1;
            }

            return result;
        }


        /// <summary>
        /// Permite obtener la topologia por id escenario
        /// </summary>
        /// <param name="idEsceneario"></param>
        /// <returns></returns>
        public CpTopologiaDTO ObtenerPorTopologiaPorId(int idEsceneario)
        {
            return FactorySic.GetCptopologiaRepository().GetById(idEsceneario);
        }


        public List<CpRecursoDTO> ObtenerListaRelacionBarraCentral(CpTopologiaDTO topologiaFinal)
        {
            var lstBarraCentral = new List<CpRecursoDTO>();
            if (topologiaFinal != default(CpTopologiaDTO))
            {
                lstBarraCentral = FactorySic.GetCpRecursoRepository().ObtenerListaRelacionBarraCentral(topologiaFinal.Topcodi);
            }

            return lstBarraCentral;
        }

        public List<CpRecursoDTO> ObtenerVolMinMaxDeEmbalseCentralPorTopologiaFinal(CpTopologiaDTO topologiaFinal)
        {
            var lstRecursoPropiedades = new List<CpRecursoDTO>();
            if (topologiaFinal != default(CpTopologiaDTO))
            {
                lstRecursoPropiedades = ObtenerCpRecursoPorTopologiaYCategoria(topologiaFinal.Topcodi, $"{ConstantesCortoPlazo.PropcodiVolumenMinimo},{ConstantesCortoPlazo.PropcodiVolumenMaximo}");
            }

            return lstRecursoPropiedades;
        }

        /// <summary>
        /// Obtener listado cp_medicion48 por topologia, srestcodi y reprograma
        /// </summary>
        /// <param name="topologiaFinal"></param>
        /// <param name="fecha"></param>
        /// <param name="srestcodi"></param>
        /// <param name="conReprograma"></param>
        /// <returns></returns>
        public List<CpMedicion48DTO> ObtenerCpMedicion48(CpTopologiaDTO topologiaFinal, DateTime fecha, string srestcodi, bool conReprograma = false)
        {
            var lstDataConReprograma = new List<CpMedicion48DTO>();
            if (topologiaFinal != null)
            {
                var lstDataProgramado = GetByCriteriaCpMedicion48s(topologiaFinal.Topcodi.ToString(), fecha, srestcodi);//Data Programado

                if (conReprograma)
                {
                    List<CpReprogramaDTO> listaReprogramados = FactorySic.GetCpReprogramaRepository().GetByCriteria(topologiaFinal.Topcodi);
                    if (listaReprogramados.Any())
                    {
                        var lstTopcodiReprog = listaReprogramados.Select(x => x.Topcodi2);//Topologias reprogramados
                        var lstDataReprogramado = GetByCriteriaCpMedicion48s(string.Join(",", lstTopcodiReprog), fecha, srestcodi);

                        List<CpTopologiaDTO> lstTopologiasRangoHoras = ObtenerTopologiasConRangoHoras(topologiaFinal, listaReprogramados);

                        lstDataConReprograma = ObtenerProgramadoActualizadoConReprograma(lstTopologiasRangoHoras, lstDataProgramado, lstDataReprogramado);
                    }
                    else
                    {
                        lstDataConReprograma = lstDataProgramado;
                    }
                }
                else
                {
                    lstDataConReprograma = lstDataProgramado;
                }
            }
            return lstDataConReprograma;
        }


        public List<CpMedicion48DTO> ProcesarDataXML(CpTopologiaDTO topologiaFinal, DateTime fecha, string srestcodi, bool conReprograma = false)
        {

            if (topologiaFinal != null)
            {
                var lstDataProgramado = GetByCriteriaCpMedicion48s(topologiaFinal.Topcodi.ToString(), fecha, srestcodi);//Data Programado

                if (conReprograma)
                {
                    List<CpReprogramaDTO> listaReprogramados = FactorySic.GetCpReprogramaRepository().GetByCriteria(topologiaFinal.Topcodi).
                        OrderBy(x => x.Reprogorden).ToList();

                    if (listaReprogramados.Any())
                    {
                        foreach (CpReprogramaDTO reprog in listaReprogramados)
                        {
                            var lstDataReprogramado = GetByCriteriaCpMedicion48s(reprog.Topcodi2.ToString(), fecha, srestcodi);

                            ObtenerProgramadoActualizadoConReprogramaXML(reprog.Tophorareprog, ref lstDataProgramado, lstDataReprogramado);
                        }
                    }
                }

                return lstDataProgramado;
            }

            return null;
        }



        private List<CpMedicion48DTO> ObtenerProgramadoActualizadoConReprogramaXML(int horaInicio, ref List<CpMedicion48DTO> lstCpMedicion48, List<CpMedicion48DTO> lstCpMedicion48Reprog)
        {
            if (horaInicio == 0) horaInicio = 1;

            foreach (var prog in lstCpMedicion48)
            {
                var med48 = lstCpMedicion48Reprog.Find(x => x.Recurcodi == prog.Recurcodi && x.Srestcodi == prog.Srestcodi);

                for (int i = horaInicio; i <= 48; i++)
                {
                    try
                    {
                        var valueHx = (decimal?)med48.GetType().GetProperty("H" + i).GetValue(med48, null);
                        prog.GetType().GetProperty("H" + i).SetValue(prog, valueHx);
                    }
                    catch (Exception ex)
                    {
                        //ex.
                    }
                }
            }

            return lstCpMedicion48;
        }


        /// <summary>
        /// Permite listar todos los registros de la tabla CP_PROPRECURSO
        /// </summary>
        public List<CpRecursoDTO> ObtenerCpRecursoPorTopologiaYCategoria(int topcodi, string propcodi = ConstantesAppServicio.ParametroDefecto)
        {
            return FactorySic.GetCpRecursoRepository().ObtenerPorTopologiaYCategoria(topcodi, propcodi);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla CpMedicion48
        /// </summary>
        public List<CpMedicion48DTO> GetByCriteriaCpMedicion48s(string topcodi, DateTime medifecha, string srestcodi)
        {
            return FactorySic.GetCpMedicion48Repository().GetByCriteria(topcodi, medifecha, srestcodi);
        }

        /// <summary>
        /// Permite obtener lista de rango de horas de las topologias programadas y reprogramadas
        /// </summary>
        /// <param name="topologiaFinal"></param>
        /// <param name="listaReprogramados"></param>
        /// <returns></returns>
        private List<CpTopologiaDTO> ObtenerTopologiasConRangoHoras(CpTopologiaDTO topologiaFinal, List<CpReprogramaDTO> listaReprogramados)
        {
            topologiaFinal.Toppadre = true;

            List<CpTopologiaDTO> lstTopologiaHorasInicio = new List<CpTopologiaDTO>() { topologiaFinal };

            if (listaReprogramados.Any())
            {
                var lstHorasInicioReprog = listaReprogramados.OrderBy(x => x.Reprogorden).Select(x => new CpTopologiaDTO { Topcodi = x.Topcodi2.Value, Topiniciohora = x.Topiniciohora }).ToList();

                int index = 0;
                foreach (var horasInicioReprog in lstHorasInicioReprog)
                {
                    lstTopologiaHorasInicio[index].Topfinhora = horasInicioReprog.Topiniciohora - 1;
                    index++;

                    if (lstHorasInicioReprog.Count == index) horasInicioReprog.Topfinhora = 48;

                    lstTopologiaHorasInicio.Add(horasInicioReprog);
                }
            }
            else
            {
                topologiaFinal.Topfinhora = 48;
            }

            return lstTopologiaHorasInicio;
        }



        /// <summary>
        /// Permite actualizar lista de cp_medicion48 con informacion del reprogramado
        /// </summary>
        /// <param name="lstTopologiasHorasInicio"></param>
        /// <param name="lstCpMedicion48"></param>
        /// <param name="lstCpMedicion48Reprog"></param>
        /// <returns></returns>
        private List<CpMedicion48DTO> ObtenerProgramadoActualizadoConReprograma(List<CpTopologiaDTO> lstTopologiasHorasInicio, List<CpMedicion48DTO> lstCpMedicion48, List<CpMedicion48DTO> lstCpMedicion48Reprog)
        {
            var lstTopologHoraInicioReprog = lstTopologiasHorasInicio.Where(x => x.Toppadre = false);

            foreach (var hTop in lstTopologHoraInicioReprog)
            {
                var lstMed48Topolog = lstCpMedicion48Reprog.Where(x => x.Topcodi == hTop.Topcodi).ToList();
                if (!lstMed48Topolog.Any()) continue;

                foreach (var medicion48 in lstCpMedicion48)
                {
                    var med48 = lstMed48Topolog.Find(x => x.Recurcodi == medicion48.Recurcodi && x.Srestcodi == medicion48.Srestcodi);
                    if (med48 == null) continue;

                    for (int index = hTop.Topiniciohora; index <= hTop.Topfinhora; index++)
                    {
                        var valueHx = (decimal?)med48.GetType().GetProperty($"H{index}").GetValue(med48, null);
                        med48.GetType().GetProperty($"H{index}").SetValue(med48, valueHx);
                    }
                }
            }

            return lstCpMedicion48;
        }

        /// <summary>
        /// Permite obtener valor de media hora por recurso
        /// </summary>
        /// <param name="lstCpMedicion48"></param>
        /// <param name="hx"></param>
        /// <param name="recurcodi"></param>
        /// <returns></returns>
        public double? ObtenerValorHxPorRecurso(List<CpMedicion48DTO> lstCpMedicion48, int hx, int? recurcodi)
        {
            CpMedicion48DTO volEmbalse = lstCpMedicion48.Find(x => x.Recurcodi == recurcodi);

            if (volEmbalse != null)
            {
                object result = volEmbalse.GetType().GetProperty($"H{hx}").GetValue(volEmbalse, null);

                if (result != null)
                {
                    return Convert.ToDouble(result);
                }
                return 0;
            }
            return 0;
        }

        /// <summary>
        /// Permite obtener el listado de topologias
        /// </summary>
        /// <param name="fechaProceso"></param>
        /// <returns></returns>
        public List<CpTopologiaDTO> ObtenerEscencariosPorDia(DateTime fechaProceso)
        {
            return FactorySic.GetCpTopologiaRepository().ObtenerEscenariosPorDia(fechaProceso);
        }

        public List<CpTopologiaDTO> ObtenerEscenariosPorDiaConsulta(DateTime fechaProceso, int tipo)
        {
            return FactorySic.GetCpTopologiaRepository().ObtenerEscenariosPorDiaConsulta(fechaProceso, tipo);
        }

        /// <summary>
        /// Permite obtener los datos del yupana
        /// </summary>
        /// <param name="idEscenario"></param>
        /// <param name="tipoInformacion"></param>
        /// <returns></returns>
        public List<CpMedicion48DTO> ObtenerDatosEscenario(int idEscenario, DateTime fecha, int tipoInformacion)
        {
            CpTopologiaDTO topologia = FactorySic.GetCpTopologiaRepository().GetById(idEscenario);

            List<CpMedicion48DTO> list = FactorySic.GetCpMedicion48Repository().ObtenerDatosModelo(idEscenario.ToString(),
                fecha, tipoInformacion.ToString(), topologia.Topsinrsf).OrderBy(x => x.Recurnombre).ToList();

            return list;
        }

        public List<CpTopologiaDTO> ObtenerTipoRestricciones()
        {
            return FactorySic.GetCpTopologiaRepository().ObtenerTipoRestriccion();
        }

        #endregion

        #region Horas Programadas

        /// <summary>
        /// //obtener los rangos de bloques amarillos yupana que no se cruzan con los ejecutados de horas de operación
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="listaHorasOperacion"></param>
        /// <param name="listaModosOperacion"></param>
        /// <param name="listaHorasProgramadas"></param>
        /// <param name="listaModoProgramada"></param>
        public void ObtenerHorasProgramadas(DateTime fechaCI, List<EveHoraoperacionDTO> listaHorasOperacion,
                                                List<PrGrupoDTO> listaModosOperacion, 
                                                out List<HorasProgramadasDTO> listaHorasProgramadas, out List<PrGrupoDTO> listaModoProgramada)
        {
            listaHorasProgramadas = new List<HorasProgramadasDTO>();
            listaModoProgramada = new List<PrGrupoDTO>();

            PrGrupoDTO modo;

            var fecha = fechaCI.Date;
            int hLineaVerde = COES.Servicios.Aplicacion.Helper.Util.GetPosicionHoraInicial48(fechaCI)[0];
            List<CpTopologiaDTO> topologias = this.ObtenerTopologias(fecha);

            List<CpMedicion48DTO> listData;

            int hojaTotalGeter = 62; //restriccion yupana Potencia Térmica

            //un modo de operacion puede estar en varios yupana
            listData = this.ObtenerDataReporte(topologias, hojaTotalGeter, fecha);

            //listado unico de modos
            List<CpMedicion48DTO> listaRecurso = listData.GroupBy(x => x.Recurcodi).Select(x => new CpMedicion48DTO()
            {
                Recurnombre = x.First().Recurnombre,
                Recurcodisicoes = x.First().Recurcodisicoes,
                Recurcodi = x.Key
            }).ToList();

            //caso especial 1: No considerar los modos de operación de la central Oquendo
            List<int> lgrupocodiCTOquendo = new List<int>() { 3427, 3424, 352, 298, 3418 };
            listaRecurso = listaRecurso.Where(x => !lgrupocodiCTOquendo.Contains(x.Recurcodisicoes)).ToList();

            //caso especial 2: Los modos de operación de fuego directo cambiarlos por los modos sin fuego directo 
            foreach (var objRec in listaRecurso)
            {
                if (objRec.Recurcodisicoes == 291) 
                    objRec.Recurcodisicoes = 288; //cambiar VENTCC34GASFD por VENTCC34GAS
                if (objRec.Recurcodisicoes == 289) 
                    objRec.Recurcodisicoes = 286; //cambiar VENTCC3GASFD por VENTCC3GAS
                if (objRec.Recurcodisicoes == 290) 
                    objRec.Recurcodisicoes = 287; //cambiar VENTCC4GASFD por VENTCC4GAS
            }

            //solo considerar los recursos yupana que tienen relacion con modo de operación
            listaRecurso = listaRecurso.Where(p => p.Recurcodisicoes > 0).ToList();
            listaRecurso = listaRecurso.GroupBy(x => x.Recurcodisicoes).Select(x => x.First()).ToList();

            //a cada modo setearle sus valores numericos (cada modo es igual al excel exportado Programación/YUPANA/Consulta de Datos – Diario/)
            foreach (CpTopologiaDTO topologia in topologias)
            {
                List<CpMedicion48DTO> listDataXTop = listData.Where(x => x.Topcodi == topologia.Topcodi).OrderBy(x => x.Recurnombre).ToList();

                foreach (var objRec in listaRecurso)
                {
                    CpMedicion48DTO objDataXTopyRec = listDataXTop.Find(x => x.Recurcodi == objRec.Recurcodi);
                    if (objDataXTopyRec != null)
                    {
                        for (int i = 1; i <= 48; i++)
                        {
                            decimal? valorH = (decimal?)objDataXTopyRec.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(objDataXTopyRec);

                            if (i>= hLineaVerde && i >= topologia.HoraReprograma)
                            {
                                objRec.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).SetValue(objRec, valorH);
                            }
                        }
                    }
                }
            }

            //a los datos yupana del día, setearle un cero cuando se cruza con la hora de operación
            foreach (CpMedicion48DTO objRec in listaRecurso)
            {
                var listaHoXRecurso = listaHorasOperacion.Where(x => x.Grupocodi == objRec.Recurcodisicoes).ToList();
                foreach (var hoEjec in listaHoXRecurso)
                {
                    HorasOperacionAppServicio.GetHoraIniFin48HoVsYupana(fecha, hoEjec.Hophorini.Value, hoEjec.Hophorfin.Value, out int hi, out int hf);
                    
                    for (int i = hi; i <= hf; i++)
                    {
                        objRec.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).SetValue(objRec, 0.0m);
                    }
                }
            }

            //obtener rangos. Lógica similar al de bloques de EMS (método ListarHorasOperacionUnidadesNoRegistrados)   
            foreach (CpMedicion48DTO item in listaRecurso)
            {
                modo = listaModosOperacion.SingleOrDefault(p => p.Grupocodi == item.Recurcodisicoes);
                if (modo != null)
                {
                    var listaHorasProgramadasXModo = new List<HorasProgramadasDTO>();

                    bool flagTieneYupana = false;
                    int contadorBloque30min = 0;
                    HorasProgramadasDTO regNuevo = null;
                    DateTime? fini = null, ffin = null;

                    for (var z = 1; z <= 48; z++)
                    {
                        if (z == 48 && item.Recurcodisicoes == 13604)
                        { }

                        DateTime fi = fecha.Date.AddMinutes(z * 30);

                        decimal? valor = (decimal?)item.GetType().GetProperty(ConstantesAppServicio.CaracterH + z.ToString()).GetValue(item, null);
                        flagTieneYupana = valor.GetValueOrDefault(0) > 0;

                        if (flagTieneYupana)
                        {
                            if (contadorBloque30min == 0)
                            {
                                regNuevo = new HorasProgramadasDTO();
                                regNuevo.Grupocodi = item.Recurcodisicoes;
                                regNuevo.Gruponom = item.Recurnombre;
                                fini = fecha.Date.AddMinutes((z - 1) * 30);
                                ffin = fecha.Date.AddMinutes((z) * 30);
                            }
                            else
                            {
                                ffin = fecha.Date.AddMinutes((z) * 30);
                            }
                            contadorBloque30min++;
                        }

                        if (!flagTieneYupana && regNuevo != null) //no hay scada pero si hop
                        {
                            // Si el fin es media noche solo debe considerarse hasta 23:58
                            if (ffin.Value == ffin.Value.Date)
                            {
                                ffin = ffin.Value.AddMinutes(-2);
                            }

                            regNuevo.FechaInicio = fini.Value;
                            regNuevo.FechaFin = ffin.Value;
                            regNuevo.HoraInicio = regNuevo.FechaInicio.ToString("yyyy-MM-dd HH:mm:ss");
                            regNuevo.HoraFin = regNuevo.FechaFin.ToString("yyyy-MM-dd HH:mm:ss");

                            listaHorasProgramadasXModo.Add(regNuevo);

                            fini = null;
                            ffin = null;
                            contadorBloque30min = 0;
                            regNuevo = null;
                        }
                    }

                    if (flagTieneYupana && regNuevo != null) //no hay scada pero si hop, cuando no se cierra las fechas
                    {
                        // Si el fin es media noche solo debe considerarse hasta 23:58
                        if (ffin.Value == ffin.Value.Date)
                        {
                            ffin = ffin.Value.AddMinutes(-2);
                        }

                        regNuevo.FechaInicio = fini.Value;
                        regNuevo.FechaFin = ffin.Value;
                        regNuevo.HoraInicio = regNuevo.FechaInicio.ToString("yyyy-MM-dd HH:mm:ss");

                        regNuevo.HoraFin = regNuevo.FechaFin.ToString("yyyy-MM-dd HH:mm:ss");

                        listaHorasProgramadasXModo.Add(regNuevo);
                    }

                    //agregar modos yupana con valores numericos
                    if (listaHorasProgramadasXModo.Any())
                    {
                        listaModoProgramada.Add(modo);
                        listaHorasProgramadas.AddRange(listaHorasProgramadasXModo);
                    }
                }
            }

            //ordenamiento de modos programados
            listaModoProgramada = listaModoProgramada.OrderBy(x => x.Emprnomb).ThenBy(x => x.Central).ThenBy(x => x.Grupoabrev).ToList();
        }

        #endregion

        #region Reporte Resumen YUPANA

        /// <summary>
        /// Permite generar el reporte resumen de datos YUPANA
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="path"></param>
        /// <param name="file"></param>
        public int ReporteResumen(DateTime fecha, string path, string filename)
        {
            try
            {
                string file = path + filename;
                FileInfo newFile = new FileInfo(file);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(file);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    List<CpTopologiaDTO> topologias = this.ObtenerTopologias(fecha);
                    List<ReporteResumenYupana> hojas = this.ObtenerHojas();

                    foreach (ReporteResumenYupana hoja in hojas)
                    {
                        List<CpMedicion48DTO> listData = this.ObtenerDataReporte(topologias, hoja.TipoInformacion, fecha);

                        ExcelWorksheet wsTotal = xlPackage.Workbook.Worksheets.Add(hoja.HojaTotal);
                        wsTotal = xlPackage.Workbook.Worksheets[hoja.HojaTotal];

                        int col = 2;
                        int index = 1;
                        wsTotal.Cells[index, col].Value = "RECURSO";
                        index++;
                        for (int i = 1; i <= 48; i++)
                        {
                            string periodo = fecha.AddMinutes(i * 30).ToString(ConstantesAppServicio.FormatoHora);
                            wsTotal.Cells[index, 2].Value = periodo;
                            index++;
                        }

                        foreach (CpTopologiaDTO topologia in topologias)
                        {
                            List<CpMedicion48DTO> list = listData.Where(x => x.Topcodi == topologia.Topcodi).OrderBy(x => x.Recurnombre).ToList();
                            this.GenerarHojaReporte(fecha, xlPackage, hoja.NombreHoja + topologia.Topnombre, list, wsTotal, topologia);
                        }

                    }

                    xlPackage.Save();
                }

                return 1;
            }
            catch (Exception)
            {
                return -1;
            }
        }


        private void GenerarHojaReporte(DateTime fecha, ExcelPackage xlPackage, string nameWS, List<CpMedicion48DTO> list, ExcelWorksheet wsTotal,
            CpTopologiaDTO topologia)
        {
            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];             
            int col = 2;
            int index = 1;
            ws.Cells[index, col].Value = "RECURSO";
            index++;
            for (int i = 1; i <= 48; i++)
            {
                string periodo = fecha.AddMinutes(i * 30).ToString(ConstantesAppServicio.FormatoHora);
                ws.Cells[index, 2].Value = periodo;
                index++;
            }
            col++;
            index = 1;
            foreach (CpMedicion48DTO item in list)
            {
                ws.Cells[index, col].Value = item.Recurnombre;
                wsTotal.Cells[index, col].Value = item.Recurnombre;

                for (int i = 1; i <= 48; i++)
                {
                    index++;
                    object valor = item.GetType().GetProperty("H" + i).GetValue(item);
                    ws.Cells[index, col].Value = (valor != null) ? valor : null;

                    if(i >= topologia.HoraReprograma)
                    {
                        wsTotal.Cells[index, col].Value = (valor != null) ? valor : null;
                    }
                }

                ExcelRange rg = wsTotal.Cells[topologia.HoraReprograma + 1, col, 49, col];
                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(topologia.Color)); 

                index = 1;
                col++;
            }

            
        }

        /// <summary>
        /// Permite preparar la data para el reporte
        /// </summary>
        /// <param name="topologias"></param>
        /// <param name="tipoInformacion"></param>
        /// <param name="fecha"></param>
        public List<CpMedicion48DTO> ObtenerDataReporte(List<CpTopologiaDTO> topologias, int tipoInformacion, DateTime fecha)
        {
            List<CpMedicion48DTO> result = new List<CpMedicion48DTO>();
            foreach (CpTopologiaDTO topologia in topologias)
            {
                List<CpMedicion48DTO> list = FactorySic.GetCpMedicion48Repository().ObtenerDatosModelo(topologia.Topcodi.ToString(),
                    fecha, tipoInformacion.ToString(), topologia.Topsinrsf);
                result.AddRange(list);               
            }

            var listRecursos = result.Select(x => new { Recurcodi = x.Recurcodi, Recurnomb = x.Recurnombre, Recurcodisicoes = x.Recurcodisicoes }).Distinct().ToList();

            foreach(CpTopologiaDTO topologia in topologias)
            {
                List<CpMedicion48DTO> list = result.Where(x => x.Topcodi == topologia.Topcodi).ToList();
                var subList = listRecursos.Where(x => !list.Any(y => x.Recurcodi == y.Recurcodi)).ToList();

                foreach(var itemList in subList)
                {
                    CpMedicion48DTO itemMedicion = new CpMedicion48DTO();
                    itemMedicion.Topcodi = topologia.Topcodi;
                    itemMedicion.Recurcodi = itemList.Recurcodi;
                    itemMedicion.Recurnombre = itemList.Recurnomb;
                    itemMedicion.Recurcodisicoes = itemList.Recurcodisicoes;
                    result.Add(itemMedicion);
                }
            }

            return result;
        }

        /// <summary>
        /// Permite obtener los programas y reprogramas
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<CpTopologiaDTO> ObtenerTopologias(DateTime fecha)
        {
            List<CpTopologiaDTO> result = new List<CpTopologiaDTO>();
            CpTopologiaDTO entity = FactorySic.GetCpTopologiaRepository().GetByFechaTopfinal(fecha, ConstantesCortoPlazo.TopologiaDiario);

            if (entity != null)
            {
                entity.Topnombre = "PDO";
                entity.Color = this.ObtenerColorReprograma(0);
                entity.HoraReprograma = 1;
                result.Add(entity);
                List<CpReprogramaDTO> listaReprogramas = FactorySic.GetCpReprogramaRepository().GetByCriteria(entity.Topcodi).OrderBy(x => x.Reprogorden).ToList();

                int index = 0;
                foreach (CpReprogramaDTO item in listaReprogramas)
                {
                    int topCodi = (int)item.Topcodi2;
                    CpTopologiaDTO entityReprograma = FactorySic.GetCptopologiaRepository().GetById(topCodi);
                    entityReprograma.Topnombre = (Convert.ToChar(65 + index)).ToString();
                    entityReprograma.Color = this.ObtenerColorReprograma(index + 1);
                    entityReprograma.HoraReprograma = item.Tophorareprog;
                    index++;
                    result.Add(entityReprograma);
                }
            }

            return result;
        }

        /// <summary>
        /// Permite otbener la estructura de hojas de reporte
        /// </summary>
        /// <returns></returns>
        public List<ReporteResumenYupana> ObtenerHojas()
        {
            List<ReporteResumenYupana> result = new List<ReporteResumenYupana>();
            result.Add(new ReporteResumenYupana { TipoInformacion = 73, NombreHoja = "CMg", HojaTotal = "Total_Reprog" });
            result.Add(new ReporteResumenYupana { TipoInformacion = 62, NombreHoja = "GerTer", HojaTotal = "Total_Geter" });
            result.Add(new ReporteResumenYupana { TipoInformacion = 72, NombreHoja = "CMgLinea", HojaTotal = "Total_Conges" });
            result.Add(new ReporteResumenYupana { TipoInformacion = 33, NombreHoja = "YupanaCmgFI", HojaTotal = "Total_FI" });
            result.Add(new ReporteResumenYupana { TipoInformacion = 34, NombreHoja = "YupanaCmgFS", HojaTotal = "Total_FS" });
            return result;
        }
        /// <summary>
        /// Permite obtener los colores de los reprogramas
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string ObtenerColorReprograma(int index)
        {
            string[] colores = { "#F3F4F8", "#0AC053", "#FE0000", "#FFDD00", "#00B7E1", "#7551CB", "#E4EA32", "#F48921", "#018273" };
            string color = colores[index];
            return color;
        }
         


        #endregion
    }
}
