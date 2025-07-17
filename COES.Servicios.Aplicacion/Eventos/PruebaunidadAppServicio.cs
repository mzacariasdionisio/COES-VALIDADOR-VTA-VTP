using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Eventos.Helper;
using System.Linq;

namespace COES.Servicios.Aplicacion.Pruebaunidad
{
    /// <summary>
    /// Clases con métodos del módulo Eventos
    /// </summary>
    public class PruebaunidadAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(PruebaunidadAppServicio));
        FormatoMedicionAppServicio servFormato = new FormatoMedicionAppServicio();

        #region Métodos Tabla EVE_PRUEBAUNIDAD

        /// <summary>
        /// Inserta un registro de la tabla EVE_PRUEBAUNIDAD
        /// </summary>
        public void SaveEvePruebaunidad(EvePruebaunidadDTO entity)
        {
            try
            {
                FactorySic.GetEvePruebaunidadRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla EVE_PRUEBAUNIDAD
        /// </summary>
        public void UpdateEvePruebaunidad(EvePruebaunidadDTO entity)
        {
            try
            {
                FactorySic.GetEvePruebaunidadRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EVE_PRUEBAUNIDAD
        /// </summary>
        public void DeleteEvePruebaunidad(int prundcodi)
        {
            try
            {
                FactorySic.GetEvePruebaunidadRepository().Delete(prundcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla EVE_PRUEBAUNIDAD
        /// </summary>
        public EvePruebaunidadDTO GetByIdEvePruebaunidad(int prundcodi)
        {
            return FactorySic.GetEvePruebaunidadRepository().GetById(prundcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EVE_PRUEBAUNIDAD
        /// </summary>
        public List<EvePruebaunidadDTO> ListEvePruebaunidads()
        {
            return FactorySic.GetEvePruebaunidadRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla EvePruebaunidad
        /// </summary>
        public List<EvePruebaunidadDTO> GetByCriteriaEvePruebaunidads(DateTime prundFechaIni, DateTime prundFechaFin)
        {
            return FactorySic.GetEvePruebaunidadRepository().GetByCriteria(prundFechaIni, prundFechaFin);
        }

        /// <summary>
        /// Graba los datos de la tabla EVE_PRUEBAUNIDAD
        /// </summary>
        public int SaveEvePruebaunidadId(EvePruebaunidadDTO entity)
        {
            return FactorySic.GetEvePruebaunidadRepository().SaveEvePruebaunidadId(entity);
        }

        /// <summary>
        /// Busca las operaciones de acuerdo a filtro de la tabla EVE_PRUEBAUNIDAD
        /// </summary>
        public List<EvePruebaunidadDTO> BuscarOperaciones(string estado, DateTime prundFechaIni, DateTime prundFechaFin, int nroPage, int pageSize)
        {
            return FactorySic.GetEvePruebaunidadRepository().BuscarOperaciones(estado, prundFechaIni, prundFechaFin, nroPage, pageSize);
        }

        /// <summary>
        /// Obtiene el numero de filas de acuerdo a filtro de la tabla EVE_PRUEBAUNIDAD
        /// </summary>
        public int ObtenerNroFilas(string estado, DateTime prundFechaIni, DateTime prundFechaFin)
        {
            return FactorySic.GetEvePruebaunidadRepository().ObtenerNroFilas(estado, prundFechaIni, prundFechaFin);
        }

        /// <summary>
        /// Obtiene la unidad sorteada
        /// </summary>
        /// <param name="prundFecha">Fecha</param>
        /// <returns></returns>
        public EqEquipoDTO ObtenerUnidadSorteada(DateTime prundFecha)
        {
            return FactorySic.GetEvePruebaunidadRepository().ObtenerUnidadSorteada(prundFecha);
        }

        /// <summary>
        /// Obtiene la unidad sorteada habilitada(Una unidad puede estar habilitada más de un día para ingreso de medidores)
        /// </summary>
        /// <param name="prundFecha">Fecha</param>
        /// <returns></returns>
        public List<EqEquipoDTO> ObtenerUnidadSorteadaHabilitada(DateTime prundFecha)
        {
            return FactorySic.GetEvePruebaunidadRepository().ObtenerUnidadSorteadaHabilitada(prundFecha);
        }

        /// <summary>
        /// Permite obtener el valor de parametro por modo
        /// </summary>
        /// <param name="grupocodi">Código de grupo</param>
        /// <param name="concepcodi">Código de concepto</param>
        /// <param name="fecha">Fecha</param>
        /// <returns></returns>
        public decimal ObtenerValorParametroGrupo(int grupocodi, int concepcodi, DateTime fecha)
        {
            return FactorySic.GetPrGrupodatRepository().GetValorModoOperacion(grupocodi, concepcodi, fecha);

        }

        /// <summary>
        /// Permite obtener el valor de parametro por equipo
        /// </summary>
        /// <param name="grupocodi">Código de grupo</param>
        /// <param name="concepcodi">Código de concepto</param>
        /// <param name="fecha">Fecha</param>
        /// <returns></returns>
        public decimal ObtenerValorParametroEquipo(int equicodi, int concepcodi, DateTime fecha)
        {

            return FactorySic.GetPrGrupoEquipoValRepository().GetValorPropiedadDetalleEquipo(equicodi, concepcodi, fecha);

        }

        /// <summary>
        /// Permite obtener el punto de medición de la unidad sorteada
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="origlectcodi"></param>
        /// <returns></returns>
        public int ObtenerPtomedicionSorteo(DateTime fecha, int origlectcodi)
        {
            return FactorySic.GetMePtomedicionRepository().ObtenerPtomedicionSorteo(fecha, origlectcodi);
        }

        /// <summary>
        /// Permite obtener los datos 15" relacionados a la unidad sorteada
        /// </summary>
        /// <param name="idTipoInformacion">Código de tipo de información</param>
        /// <param name="idPtoMedicion">Código de punto de medición</param>
        /// <param name="idLectura">Código de lectura</param>
        /// <param name="fechaInicio">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <returns></returns>
        public List<MeMedicion96DTO> GetByCriteriaMedicion96(int idTipoInformacion, int idPtoMedicion, int idLectura, DateTime fechaInicio,
                DateTime fechaFin)
        {
            return FactorySic.GetMeMedicion96Repository().GetByCriteria(idTipoInformacion, idPtoMedicion, idLectura, fechaInicio, fechaFin);
        }

        /// <summary>
        /// Permite eliminar los datos 15" relacionados a la unidad sorteada
        /// </summary>
        /// <param name="ptomedicodi">Código de punto de medición</param>
        /// <param name="tipoinfocodi">Tipo de información</param>
        /// <param name="fechaInicio">Fecha inicial</param>
        /// <param name="fechaFinal">fecha final</param>
        /// <param name="lectcodi96">Código de lectura</param>
        public void Delete(int ptomedicodi, int tipoinfocodi, DateTime fechaInicio, DateTime fechaFinal, int lectcodi96)
        {
            FactorySic.GetMeMedicion96Repository().Delete(ptomedicodi, tipoinfocodi, fechaInicio, fechaFinal, lectcodi96);
        }



        #endregion

        public List<MeMedicion96DTO> GetDataFormato96Pruebaunidad(List<MeMedicion96DTO> listaOld, int idEmpresa, MeFormatoDTO formato, int idEnvio, int idUltimoEnvio)
        {
            List<MeMedicion96DTO> lista96 = listaOld.Count > 0 ? listaOld : FactorySic.GetMeMedicion96Repository().GetEnvioArchivo(formato.Formatcodi, formato.FechaInicio, formato.FechaFin);
            if (idEnvio != 0)
            {
                var lista = (new FormatoMedicionAppServicio()).GetAllCambioEnvio(formato.Formatcodi, formato.FechaInicio, formato.FechaFin, idEnvio, idEmpresa);
                if (lista.Count > 0)
                {
                    foreach (var reg in lista)
                    {
                        var find = lista96.Find(x => x.Ptomedicodi == reg.Ptomedicodi && x.Tipoinfocodi == reg.Tipoinfocodi &&
                             x.Tipoptomedicodi == reg.Tipoptomedicodi && x.Medifecha == reg.Cambenvfecha);
                        if (find != null)
                        {
                            var fila = reg.Cambenvdatos.Split(',');
                            for (var i = 0; i < 96; i++)
                            {
                                decimal dato;
                                decimal? numero = null;
                                if (decimal.TryParse(fila[i], out dato))
                                    numero = dato;
                                find.GetType().GetProperty("H" + (i + 1).ToString()).SetValue(find, numero);
                            }
                        }
                    }
                }
            }
            else
            {
                if (formato.Formatsecundario != 0 && lista96.Count == 0)
                {
                    //lista48 = FactorySic.GetMeMedicion48Repository().GetDataFormatoSecundario(formato.Formatcodi, idEmpresa, formato.FechaInicio, formato.FechaFin);
                }
            }
            return lista96;
        }

        /// <summary>
        /// Permite grabar los datos cargados
        /// </summary>
        /// <param name="entitys"></param>
        public void GrabarValoresCargados96Pruebaunidad(List<MeMedicion96DTO> entitys, string usuario, int idEnvio, int idEmpresa, MeFormatoDTO formato)
        {
            try
            {
                int count = 0;
                //Traer Ultimos Valores                
                var lista = FormatoMedicionAppServicio.Convert96DTO(GetDataFormatoPruebaunidad(idEmpresa, formato, 0, 0));
                if (lista.Count > 0) // Verificar si hay cambios en el envio
                {
                    var listaCambio = new List<MeCambioenvioDTO>();
                    var listaOrigen = new List<MeCambioenvioDTO>();


                    foreach (var reg in entitys)
                    {
                        var regAnt = lista.Find(x => x.Medifecha == reg.Medifecha && x.Ptomedicodi == reg.Ptomedicodi &&
                            x.Lectcodi == reg.Lectcodi);


                        if (regAnt != null)
                        {
                            List<string> filaValores = new List<string>();
                            List<string> filaValoresOrigen = new List<string>();
                            List<string> filaCambios = new List<string>();
                            for (int i = 1; i <= 96; i++)
                            {
                                decimal? valorOrigen = (decimal?)regAnt.GetType().GetProperty("H" + i.ToString()).GetValue(regAnt, null);
                                decimal? valorModificado = (decimal?)reg.GetType().GetProperty("H" + i.ToString()).GetValue(reg, null);
                                if (valorModificado != null)
                                    filaValores.Add(valorModificado.ToString());
                                else
                                    filaValores.Add("");
                                if (valorOrigen != null)
                                    filaValoresOrigen.Add(valorOrigen.ToString());
                                else
                                    filaValoresOrigen.Add("");
                                if (valorOrigen != valorModificado)//&& valorOrigen != null && valorModificado != null)
                                {
                                    if (count <= 100)
                                    {
                                        filaCambios.Add(i.ToString());
                                        count++;
                                    }
                                }
                            }


                            if (filaCambios.Count > 0)
                            {
                                MeCambioenvioDTO cambio = new MeCambioenvioDTO();
                                cambio.Cambenvdatos = String.Join(",", filaValores);
                                cambio.Cambenvcolvar = String.Join(",", filaCambios);
                                cambio.Cambenvfecha = (DateTime)reg.Medifecha;
                                cambio.Enviocodi = idEnvio;
                                //cambio.Formatcodi = formato.Formatcodi;
                                cambio.Ptomedicodi = reg.Ptomedicodi;
                                cambio.Tipoinfocodi = reg.Tipoinfocodi;
                                cambio.Lastuser = usuario;
                                cambio.Lastdate = DateTime.Now;
                                cambio.Camenviocodi = 0; //nuevo
                                listaCambio.Add(cambio);
                                /// Si no ha habido cambio se graba el registro original
                                if ((new FormatoMedicionAppServicio()).ListMeCambioenvios(reg.Ptomedicodi, reg.Tipoinfocodi, formato.Formatcodi, (DateTime)reg.Medifecha).Count == 0)
                                {
                                    int idEnvioPrevio = 0;
                                    var listAux = (new FormatoMedicionAppServicio()).GetByCriteriaMeEnvios(idEmpresa, formato.Formatcodi, formato.FechaInicio);
                                    if (listAux.Count > 0)
                                        idEnvioPrevio = listAux.Min(x => x.Enviocodi);
                                    MeCambioenvioDTO origen = new MeCambioenvioDTO();
                                    origen.Cambenvdatos = String.Join(",", filaValoresOrigen);
                                    origen.Cambenvcolvar = "";
                                    origen.Cambenvfecha = (DateTime)reg.Medifecha;
                                    origen.Enviocodi = idEnvioPrevio;
                                    //origen.Formatcodi = formato.Formatcodi;
                                    origen.Ptomedicodi = reg.Ptomedicodi;
                                    origen.Tipoinfocodi = reg.Tipoinfocodi;
                                    origen.Lastuser = usuario;
                                    origen.Lastdate = DateTime.Now;
                                    origen.Camenviocodi = 0; //nuevo
                                    listaOrigen.Add(origen);
                                }
                            }
                        }
                    }
                    if (listaCambio.Count > 0)
                    {//Grabar Cambios
                        (new FormatoMedicionAppServicio()).GrabarCambios(listaCambio);
                        (new FormatoMedicionAppServicio()).GrabarCambios(listaOrigen);
                        //si es primer reenvio grabar valores origen
                    }
                }

                foreach (MeMedicion96DTO entity in entitys)
                {
                    Delete(entity.Ptomedicodi, entity.Tipoinfocodi, formato.FechaInicio, formato.FechaFin, entity.Lectcodi);

                    FactorySic.GetMeMedicion96Repository().Save(entity);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Se obtiene los datos enviados de un formato de la BD.
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="formato"></param>
        /// <param name="idEnvio"></param>
        /// <returns></returns>
        public List<Object> GetDataFormatoPruebaunidad(int idEmpresa, MeFormatoDTO formato, int idEnvio, int idUltimoEnvio)
        {
            List<Object> listaGenerica = new List<Object>();

            switch (formato.Formatresolucion)
            {
                case ParametrosFormato.ResolucionCuartoHora:
                    List<MeMedicion96DTO> lista96 = FactorySic.GetMeMedicion96Repository().GetEnvioArchivo(formato.Formatcodi, formato.FechaInicio, formato.FechaFin);
                    if (idEnvio != 0)
                    {
                        var lista = (new FormatoMedicionAppServicio()).GetAllCambioEnvio(formato.Formatcodi, formato.FechaInicio, formato.FechaFin, idEnvio, idEmpresa);
                        if (lista.Count > 0)
                        {
                            foreach (var reg in lista)
                            {
                                var find = lista96.Find(x => x.Ptomedicodi == reg.Ptomedicodi && x.Tipoinfocodi == reg.Tipoinfocodi &&
                                    x.Medifecha == reg.Cambenvfecha);
                                if (find != null)
                                {
                                    var fila = reg.Cambenvdatos.Split(',');
                                    for (var i = 0; i < 96; i++)
                                    {
                                        decimal dato;
                                        decimal? numero = null;
                                        if (decimal.TryParse(fila[i], out dato))
                                            numero = dato;
                                        find.GetType().GetProperty("H" + (i + 1).ToString()).SetValue(find, numero);
                                    }
                                }
                            }
                        }
                    }

                    foreach (var reg in lista96)
                    {
                        listaGenerica.Add(reg);
                    }
                    break;
            }
            return listaGenerica;
        }

    }
}
