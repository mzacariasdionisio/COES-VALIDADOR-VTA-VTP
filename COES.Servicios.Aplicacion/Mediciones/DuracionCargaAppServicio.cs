using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using System.Linq;
using log4net;
using System.Text;
using System.IO;
using OfficeOpenXml;
using System.Net;
using OfficeOpenXml.Drawing;
using System.Globalization;
using OfficeOpenXml.Style;
using System.Drawing;
using COES.Servicios.Aplicacion.IEOD;

namespace COES.Servicios.Aplicacion.Mediciones
{
    public class DuracionCargaAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ConsultaMedidoresAppServicio));

        /// <summary>
        /// Lista los tipos de generación
        /// </summary>
        /// <returns></returns>
        public List<SiTipogeneracionDTO> ListaTipoGeneracion()
        {
            return FactorySic.GetSiTipogeneracionRepository().GetByCriteria().Where(x => x.Tgenercodi != -1 && x.Tgenercodi != 0 && x.Tgenercodi != 5).ToList();
        }

        /// <summary>
        /// Permite listar los tipos de empresas
        /// </summary>
        /// <returns></returns>
        public List<SiTipoempresaDTO> ListaTipoEmpresas()
        {
            return FactorySic.GetSiTipoempresaRepository().List();
        }

        /// <summary>
        /// Lista las fuentes de energía
        /// </summary>
        /// <returns></returns>
        public List<SiFuenteenergiaDTO> ListaFuenteEnergia()
        {
            return FactorySic.GetSiFuenteenergiaRepository().GetByCriteria().Where(x => x.Fenergcodi != -1 && x.Fenergcodi != 0).ToList();
        }

        /// <summary>
        /// Permite listar las empresas
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListaEmpresa()
        {
            return FactorySic.GetSiEmpresaRepository().ObtenerEmpresasSEIN();
        }

        /// <summary>
        /// Permite obtener las empresa por tipo
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ObteneEmpresasPorTipo(string tiposEmpresa)
        {
            if (string.IsNullOrEmpty(tiposEmpresa)) tiposEmpresa = ConstantesAppServicio.ParametroDefecto;
            return (new IEODAppServicio()).ListarEmpresasTienenCentralGenxTipoEmpresa(tiposEmpresa);
        }

        /// <summary>
        /// Muestra el reporte de máxima demanda HFP y HP
        /// </summary>
        /// <param name="fechaini"></param>
        /// <param name="fechafin"></param>
        /// <param name="tiposEmpresa"></param>
        /// <param name="empresas"></param>
        /// <param name="tiposGeneracion"></param>
        /// <param name="central"></param>
        /// <returns></returns>
        public List<MeMedicion96DTO> ObtenerDiagramaCarga(DateTime fechaInicio, DateTime fechaFin, string tiposEmpresa, string empresas,
            string tiposGeneracion, int central)
        {
            if (tiposEmpresa != ConstantesAppServicio.ParametroDefecto && empresas == ConstantesAppServicio.ParametroDefecto)
            {
                List<int> idsEmpresas = this.ObteneEmpresasPorTipo(tiposEmpresa).Select(x => x.Emprcodi).ToList();
                empresas = string.Join<int>(ConstantesAppServicio.CaracterComa.ToString(), idsEmpresas);
            }

            //List<MeMedicion96DTO> list = FactorySic.GetMeMedicion96Repository().ObtenerMaximaDemandaPorRecursoEnergetico(fechaInicio, fechaFin,
            //    empresas, tiposGeneracion, central).OrderBy(x => x.Medifecha).ToList();

            ReporteMedidoresAppServicio servMedidores = new ReporteMedidoresAppServicio();
            string fuentesEnergia = "-1";
            int EstadoTodos = -1;
            List<MeMedicion96DTO> list = servMedidores.ListaDataMDGeneracionConsolidado(fechaInicio, fechaFin, central, tiposGeneracion, empresas, EstadoTodos, fuentesEnergia, true);

            #region Fuente Generacion

            var listFuenteGeneracion = (from t in list
                                        group t by new { t.Medifecha, t.Fenergcodi, t.Fenergnomb }
                                            into destino
                                            select new MeMedicion96DTO()
                                            {
                                                Medifecha = destino.Key.Medifecha,
                                                Fenergcodi = destino.Key.Fenergcodi,
                                                Fenergnomb = destino.Key.Fenergnomb,
                                                H1 = destino.Sum(t => t.H1),
                                                H2 = destino.Sum(t => t.H2),
                                                H3 = destino.Sum(t => t.H3),
                                                H4 = destino.Sum(t => t.H4),
                                                H5 = destino.Sum(t => t.H5),
                                                H6 = destino.Sum(t => t.H6),
                                                H7 = destino.Sum(t => t.H7),
                                                H8 = destino.Sum(t => t.H8),
                                                H9 = destino.Sum(t => t.H9),
                                                H10 = destino.Sum(t => t.H10),

                                                H11 = destino.Sum(t => t.H11),
                                                H12 = destino.Sum(t => t.H12),
                                                H13 = destino.Sum(t => t.H13),
                                                H14 = destino.Sum(t => t.H14),
                                                H15 = destino.Sum(t => t.H15),
                                                H16 = destino.Sum(t => t.H16),
                                                H17 = destino.Sum(t => t.H17),
                                                H18 = destino.Sum(t => t.H18),
                                                H19 = destino.Sum(t => t.H19),
                                                H20 = destino.Sum(t => t.H20),

                                                H21 = destino.Sum(t => t.H21),
                                                H22 = destino.Sum(t => t.H22),
                                                H23 = destino.Sum(t => t.H23),
                                                H24 = destino.Sum(t => t.H24),
                                                H25 = destino.Sum(t => t.H25),
                                                H26 = destino.Sum(t => t.H26),
                                                H27 = destino.Sum(t => t.H27),
                                                H28 = destino.Sum(t => t.H28),
                                                H29 = destino.Sum(t => t.H29),
                                                H30 = destino.Sum(t => t.H30),

                                                H31 = destino.Sum(t => t.H31),
                                                H32 = destino.Sum(t => t.H32),
                                                H33 = destino.Sum(t => t.H33),
                                                H34 = destino.Sum(t => t.H34),
                                                H35 = destino.Sum(t => t.H35),
                                                H36 = destino.Sum(t => t.H36),
                                                H37 = destino.Sum(t => t.H37),
                                                H38 = destino.Sum(t => t.H38),
                                                H39 = destino.Sum(t => t.H39),
                                                H40 = destino.Sum(t => t.H40),

                                                H41 = destino.Sum(t => t.H41),
                                                H42 = destino.Sum(t => t.H42),
                                                H43 = destino.Sum(t => t.H43),
                                                H44 = destino.Sum(t => t.H44),
                                                H45 = destino.Sum(t => t.H45),
                                                H46 = destino.Sum(t => t.H46),
                                                H47 = destino.Sum(t => t.H47),
                                                H48 = destino.Sum(t => t.H48),
                                                H49 = destino.Sum(t => t.H49),
                                                H50 = destino.Sum(t => t.H50),


                                                H51 = destino.Sum(t => t.H51),
                                                H52 = destino.Sum(t => t.H52),
                                                H53 = destino.Sum(t => t.H53),
                                                H54 = destino.Sum(t => t.H54),
                                                H55 = destino.Sum(t => t.H55),
                                                H56 = destino.Sum(t => t.H56),
                                                H57 = destino.Sum(t => t.H57),
                                                H58 = destino.Sum(t => t.H58),
                                                H59 = destino.Sum(t => t.H59),
                                                H60 = destino.Sum(t => t.H60),

                                                H61 = destino.Sum(t => t.H61),
                                                H62 = destino.Sum(t => t.H62),
                                                H63 = destino.Sum(t => t.H63),
                                                H64 = destino.Sum(t => t.H64),
                                                H65 = destino.Sum(t => t.H65),
                                                H66 = destino.Sum(t => t.H66),
                                                H67 = destino.Sum(t => t.H67),
                                                H68 = destino.Sum(t => t.H68),
                                                H69 = destino.Sum(t => t.H69),
                                                H70 = destino.Sum(t => t.H70),

                                                H71 = destino.Sum(t => t.H71),
                                                H72 = destino.Sum(t => t.H72),
                                                H73 = destino.Sum(t => t.H73),
                                                H74 = destino.Sum(t => t.H74),
                                                H75 = destino.Sum(t => t.H75),
                                                H76 = destino.Sum(t => t.H76),
                                                H77 = destino.Sum(t => t.H77),
                                                H78 = destino.Sum(t => t.H78),
                                                H79 = destino.Sum(t => t.H79),
                                                H80 = destino.Sum(t => t.H80),

                                                H81 = destino.Sum(t => t.H81),
                                                H82 = destino.Sum(t => t.H82),
                                                H83 = destino.Sum(t => t.H83),
                                                H84 = destino.Sum(t => t.H84),
                                                H85 = destino.Sum(t => t.H85),
                                                H86 = destino.Sum(t => t.H86),
                                                H87 = destino.Sum(t => t.H87),
                                                H88 = destino.Sum(t => t.H88),
                                                H89 = destino.Sum(t => t.H89),
                                                H90 = destino.Sum(t => t.H90),

                                                H91 = destino.Sum(t => t.H91),
                                                H92 = destino.Sum(t => t.H92),
                                                H93 = destino.Sum(t => t.H93),
                                                H94 = destino.Sum(t => t.H94),
                                                H95 = destino.Sum(t => t.H95),
                                                H96 = destino.Sum(t => t.H96)

                                            }).ToList();

            #endregion

            return listFuenteGeneracion;
        }
    }
}
