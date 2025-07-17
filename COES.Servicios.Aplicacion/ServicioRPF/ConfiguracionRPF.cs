using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Factory;

namespace COES.Servicios.Aplicacion.ServicioRPF
{
    /// <summary>
    /// Constantes para el modulo del servicio rpf
    /// </summary>
    public class ConstantesServicioRPF
    {
        public const string GpsSanJuan = "SAN JUAN";
        public const string SI = "S";
        public const string NO = "N";
        public const string Consistente = "CONSISTENTE";
        public const string Inconsistente = "INCONSISTENTE";
        public const string OPERO = "OPERÓ";
        public const string CERO = "CERO";
        public const string OK = "OK";
    }

    /// <summary>
    /// Datos configurables para el RPF
    /// </summary>
    public struct ValoresRPF
    {
        /// <summary>
        /// Parámetros para la evaluación estado normal
        /// </summary>
        public const int Estatismo = 1;
        public const int RPrimaria = 2;
        public const int FrecuenciaNominal = 3;
        public const int PorcentajeEvaluacion = 4;
        public const int VariacionPotencia = 5;
        public const int VariacionFrecuencia = 6;
        public const int NumeroIntentos = 7;
        public const int MinimoExlusion = 8;
        public const int TamanioBanda = 9;
        public const int NumeroDatosNormal = 16;
        
        /// <summary>
        /// Parámetros para la evaluación ante fallas
        /// </summary>
        public const int PorcentajeCumplimientoFalla = 10;
        public const int FrecuenciaNominalFalla = 11;
        public const int EstatismoFalla = 12;
        public const int RPrimariaFalla = 13;
        public const int FrecuenciaUmbralFalla = 14;
        public const int SegundosPosteriores = 15;
        public const int SegundosFrecuenciaFalla = 20;

        /// <summary>
        /// Parámetros para el reporte de consistencias
        /// </summary>
        public const int PorcentajePercentil = 17;
        public const int ValorPercentil = 18;
        public const int PotenciaPercentil = 19;

        /// <summary>
        /// Parámetros para la evaluación en estado normal alternativo
        /// </summary>
        public const int ConstanteDAlt = 21;
        public const int FrecuenciaNomimalAlt = 22;
        public const int PorcentajeBalanceAlt = 23;
        public const int FrecuenciaLimiteMaxAlt = 24;
        public const int FrecuenciaLimiteMinAlt = 25;
        public const int VariacionPotenciaAlt = 26;
        public const int NumeroIntentosAlt = 27;
        public const int CantidadMinSinRangoAlt = 28;
        public const int NroDatosAlt = 29;
        
        /// <summary>
        /// Tipo de parámetros
        /// </summary>
        public const string AnalisisNormal = "N";
        public const string AnalisisFalla = "F";
        public const string Consultas = "C";
        public const string AnalisisAlternativo = "A";
        public const string TipoAlfanumerico = "A";
    }

    /// <summary>
    /// Permite obtener los valores configurables
    /// </summary>
    public class ConfiguracionRPF
    {
        /// <summary>
        /// Permite obtener un valor de configuracion especifico
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public double ObtenerParametro(int id)
        {
            ParametroRpfDTO entity = FactorySic.ObtenerParametroRpfDao().GetById(id);

            if (!string.IsNullOrEmpty(entity.PARAMVALUE)) 
            {
                return double.Parse(entity.PARAMVALUE);
            }

            return 0;            
        }

        /// <summary>
        /// Permite listar los parametros
        /// </summary>
        /// <param name="modulo"></param>
        /// <returns></returns>
        public List<ParametroRpfDTO> ListarParametros(string modulo)
        {
            return FactorySic.ObtenerParametroRpfDao().GetByCriteria(modulo);
        }

        /// <summary>
        /// Permite listar el histórico de un parámetro
        /// </summary>
        /// <param name="idParametro"></param>
        /// <returns></returns>
        public List<ParametroDetRpfDTO> ListaHistoricoParametro(int idParametro)
        {
            return FactorySic.ObtenerParametroRpfDao().ListarHistoricoParametro(idParametro);
        }

        /// <summary>
        /// Permite grabar los parametros utilizados
        /// </summary>
        /// <param name="entitys"></param>
        public void GrabarParametro(List<ParametroRpfDTO> entitys, string modulo)
        {
            try
            {
                //FactorySic.ObtenerParametroRpfDao().Detele(modulo);

                foreach(ParametroRpfDTO item in entitys)
                {
                    item.PARAMMODULO = modulo;
                    item.PARAMTIPO = ValoresRPF.TipoAlfanumerico;
                }

                FactorySic.ObtenerParametroRpfDao().Save(entitys);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite actualizar el valor de un parametro
        /// </summary>
        /// <param name="id"></param>
        /// <param name="valor"></param>
        public void ActualizaParametro(int id, decimal valor)
        {
            try
            {
                FactorySic.ObtenerParametroRpfDao().ActualizarParametro(id, valor);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        /// <summary>
        /// Permite grabar el historico de un parametro
        /// </summary>
        /// <param name="paramCodi"></param>
        /// <param name="valor"></param>
        /// <param name="fechaVigencia"></param>
        /// <param name="usuario"></param>
        public void GrabarHistoricoParametro(int paramCodi, decimal valor, DateTime fechaVigencia, string usuario) 
        {
            try
            {
                ParametroDetRpfDTO entity = new ParametroDetRpfDTO();
                entity.Paramdate = DateTime.Now;
                entity.Paramrpfcodi = paramCodi;
                entity.Paramusuario = usuario;
                entity.Paramvalor = valor;
                entity.Paramvigencia = fechaVigencia;

                FactorySic.ObtenerParametroRpfDao().GrabarHistorico(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener el filtro de la evaluación
        /// </summary>
        /// <returns></returns>
        public double ObtenerFiltroEvaluacion()
        {
            return Math.Max(59.85, 60 - 60 * this.ObtenerParametro(ValoresRPF.Estatismo) * this.ObtenerParametro(ValoresRPF.RPrimaria) / 10000);
        }

        /// <summary>
        /// Permite obtener el valor de la reserva asignada
        /// </summary>
        /// <param name="potenciaMax"></param>
        /// <param name="frec30Seg"></param>
        /// <param name="frecFalla"></param>
        /// <param name="potenciaGenerada"></param>
        /// <returns></returns>
        public decimal ObtenerValorRA(decimal potenciaMax, decimal frec30Seg, decimal frecFalla, decimal potenciaGenerada)
        {
            decimal estatismo = (decimal)(new ConfiguracionRPF()).ObtenerParametro(ValoresRPF.EstatismoFalla);
            decimal rPrimaria = (decimal)(new ConfiguracionRPF()).ObtenerParametro(ValoresRPF.RPrimariaFalla);
            decimal frecNominal = (decimal)(new ConfiguracionRPF()).ObtenerParametro(ValoresRPF.FrecuenciaNominalFalla);
            decimal ra = 0;
            decimal frecUmbral = frecNominal - frecNominal * estatismo * rPrimaria / 10000;
            
            decimal raEntregar = 0;
            if (frecFalla > frecUmbral)
            {
                raEntregar = (frecFalla - frecUmbral) * (100) / (frecNominal * estatismo);
            }

            decimal newRaEntregar = Math.Min(raEntregar, rPrimaria/100);

            decimal calculo = Math.Abs(potenciaMax * (frecFalla - frec30Seg) * 100 / (frecFalla * estatismo));

            if (calculo > potenciaGenerada * newRaEntregar) ra = potenciaGenerada * newRaEntregar;
            else ra = calculo;

            return ra;
        }
    }   

}
