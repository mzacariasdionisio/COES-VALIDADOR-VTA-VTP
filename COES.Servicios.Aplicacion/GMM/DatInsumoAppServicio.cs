using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.GMM
{
    public class DatInsumoAppServicio
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(IncumplimientoAppServicio));

        /// <summary>
        /// Actualiza un registro de la tabla GMM_DATINSUMO tipo 'SC'
        /// Servicios Complementarios por cada empresa agente
        /// </summary>
        public bool UpsertDatos(GmmDatInsumoDTO entity, int tipo, bool eliminar = false)
        {
            bool respuesta = true;
            try
            {
                switch (tipo)
                {
                    case 1:
                        FactorySic.GetGmmDatInsumoRepository().UpsertDatosInsumoTipoSC(entity);
                        break;
                    case 2:
                        FactorySic.GetGmmDatInsumoRepository().UpsertDatosEntregas(entity);
                        break;
                    case 3:
                        FactorySic.GetGmmDatInsumoRepository().UpsertDatosInflexibilidad(entity);
                        break;
                    case 4:
                        FactorySic.GetGmmDatInsumoRepository().UpsertDatosRecaudacion(entity);
                        break;
                    case 5:
                        var lista = FactorySic.GetGmmDatInsumoRepository().ListaDatosCalculo(entity, "MRESERVA");
                        if (lista.Count > 0 && !eliminar)
                            respuesta = FactorySic.GetGmmDatInsumoRepository().ActualizarUpsertDatosCalculo(entity, "MRESERVA");
                        else if (lista.Count > 0 && eliminar)
                            respuesta = FactorySic.GetGmmDatInsumoRepository().EliminarUpsertDatosCalculo(entity, "MRESERVA");
                        else if (!eliminar)
                            respuesta = FactorySic.GetGmmDatInsumoRepository().UpsertDatosCalculo(entity, "MRESERVA");
                        break;
                    case 6:
                        var lista2 = FactorySic.GetGmmDatInsumoRepository().ListaDatosCalculo(entity, "SSCC");
                        if (lista2.Count > 0 && !eliminar)
                            respuesta = FactorySic.GetGmmDatInsumoRepository().ActualizarUpsertDatosCalculo(entity, "SSCC");
                        else if (lista2.Count > 0 && eliminar)
                            respuesta = FactorySic.GetGmmDatInsumoRepository().EliminarUpsertDatosCalculo(entity, "SSCC");
                        else if (!eliminar)
                            respuesta = FactorySic.GetGmmDatInsumoRepository().UpsertDatosCalculo(entity, "SSCC");
                        break;
                    case 7:
                        var lista3 = FactorySic.GetGmmDatInsumoRepository().ListaDatosCalculo(entity, "TINFLEX");
                        if (lista3.Count > 0 && !eliminar)
                            respuesta = FactorySic.GetGmmDatInsumoRepository().ActualizarUpsertDatosCalculo(entity, "TINFLEX");
                        else if (lista3.Count > 0 && eliminar)
                            respuesta = FactorySic.GetGmmDatInsumoRepository().EliminarUpsertDatosCalculo(entity, "TINFLEX");
                        else if (!eliminar)
                            respuesta = FactorySic.GetGmmDatInsumoRepository().UpsertDatosCalculo(entity, "TINFLEX");
                        break;
                    case 8:
                        var lista4 = FactorySic.GetGmmDatInsumoRepository().ListaDatosCalculo(entity, "TEXCESO");
                        if (lista4.Count > 0 && !eliminar)
                            respuesta = FactorySic.GetGmmDatInsumoRepository().ActualizarUpsertDatosCalculo(entity, "TEXCESO");
                        else if (lista4.Count > 0 && eliminar)
                            respuesta = FactorySic.GetGmmDatInsumoRepository().EliminarUpsertDatosCalculo(entity, "TEXCESO");
                        else if (!eliminar)
                            respuesta = FactorySic.GetGmmDatInsumoRepository().UpsertDatosCalculo(entity, "TEXCESO");
                        break;
                    default:
                        break;
                }
                return respuesta;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        public List<GmmDatInsumoDTO> ListarDatosInsumoTipoSC(int anio, string mes)
        {
            return FactorySic.GetGmmDatInsumoRepository().ListarDatosInsumoTipoSC(anio, mes);
        }

        public List<GmmDatInsumoDTO> ListarDatosEntregas(int anio, string mes)
        {
            return FactorySic.GetGmmDatInsumoRepository().ListarDatosEntregas(anio, mes);
        }

        public List<GmmDatInsumoDTO> ListarDatosInflexibilidades(int anio, string mes)
        {
            return FactorySic.GetGmmDatInsumoRepository().ListarDatosInflexibilidad(anio, mes);
        }
        public List<GmmDatInsumoDTO> ListarDatosReacudaciones(int anio, string mes)
        {
            return FactorySic.GetGmmDatInsumoRepository().ListarDatosRecaudacion(anio, mes);
        }
        public List<GmmDatInsumoDTO> ListadoInsumos(int anio, string mes)
        {
            return FactorySic.GetGmmDatInsumoRepository().ListadoInsumos(anio, mes);
        }

        public bool ConsultaParticipanteExistente(int empgcodi)
        {
          return  FactorySic.GetGmmDatInsumoRepository().ConsultaParticipanteExistente(empgcodi);
        }
        public string ConsultaEstadoPeriodo(int anio, string mes)
        {
            return FactorySic.GetGmmDatInsumoRepository().ConsultaEstadoPeriodo(anio,mes);
        }



    }
}
