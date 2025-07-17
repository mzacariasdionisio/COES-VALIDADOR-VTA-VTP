using COES.Framework.Base.Tools;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Resarcimientos.Models
{
    public class RechazoCargaListModel
    {
        public List<RntRegPuntoEntregaDTO> ListTable { get; set; }
    }


    public class PeriodosModel
    {
        public List<RntPeriodoDTO> ListaPeriodo { get; set; }

        public int Periodocodi { get; set; }
        public string Perdestado { get; set; }
        public int? Perdanio { get; set; }
        public string Perdnombre { get; set; }
        public string Perdusuariocreacion { get; set; }
        public DateTime? Perdfechacreacion { get; set; }
        public string Perdusuarioupdate { get; set; }
        public DateTime? Perdfechaupdate { get; set; }
        public string Perdsemestre { get; set; }
        public string Estado
        {
            get
            {
                string strReturn = "";

                switch (this.Perdestado)
                {
                    case "0":
                        strReturn = "Habilitado";
                        break;
                    case "1":
                        strReturn = "Bloqueado";
                        break;
                }

                return strReturn;
            }
        }
        public RntPeriodoDTO ListaComboTodos
        {
            get
            {
                RntPeriodoDTO a = new RntPeriodoDTO();
                a.PeriodoCodi = 0;
                a.PerdNombre = "(TODOS)";
                return a;
            }
        }
        public RntPeriodoDTO ListaComboSeleccionar
        {
            get
            {
                RntPeriodoDTO a = new RntPeriodoDTO();
                a.PeriodoCodi = 0;
                a.PerdNombre = "(TODOS)";
                return a;
            }
        }

    }

    public class ConfiguracionModel
    {

        public List<RntConfiguracionDTO> ListaConfiguracion { get; set; }
        public RntConfiguracionDTO ListaComboSeleccionar
        {
            get
            {
                RntConfiguracionDTO a = new RntConfiguracionDTO();
                a.ConfigCodi = 0;
                a.ConfParametro = "(SELECCIONE)";
                return a;
            }
        }
    }

    public class PEntregaModel
    {
        public List<EqAreaDTO> ListaArea { get; set; }

        public List<RntRegPuntoEntregaDTO> ListBarras { get; set; }
        public int Areacodi { get; set; }
        public string Areanomb { get; set; }

        public RntRegPuntoEntregaDTO ListaComboSeleccionar
        {
            get
            {
                RntRegPuntoEntregaDTO a = new RntRegPuntoEntregaDTO();
                a.Barrcodi = 0;
                a.BarrNombre = "(TODOS)";
                return a;
            }
        }
    }

    public class EmpresasGeneradorasModel
    {
        public List<SiEmpresaDTO> ListaEmpresasGeneradoras { get; set; }
        public SiEmpresaDTO ListaComboTodos
        {
            get
            {
                SiEmpresaDTO a = new SiEmpresaDTO();
                a.Emprcodi = 0;
                a.Emprnomb = "(TODOS)";
                return a;
            }
        }
    }

    public class ConfiguraionModel
    {
        public string Confatributo { get; set; }
        public string Confparametro { get; set; }
        public string Confvalor { get; set; }
        public int Configcodi { get; set; }
    }


    #region Métodos PuntoEntrega

    public class RegistroPuntoEntregaModel
    {
        public List<EmpresasResponsablesModel> ListEmpresasResponsablesModel { get; set; }
        public System.Int32 RPEKEY { get; set; }
        public System.Int32 RPEEMPRESAGENERADORA { get; set; }
        public System.String RPEEMPRESAGENERADORANOMBRE { get; set; }
        public System.Int32 RPECLIENTE { get; set; }
        public System.String RPECLIENTENOMBRE { get; set; }
        public System.Int32 RPENIVELTENSION { get; set; }
        public System.String RPENIVELTENSIONNOMB { get; set; }
        public System.DateTime RPEFECHAINICIO { get; set; }
        public System.DateTime RPEFECHAFIN { get; set; }
        public System.Double RPECOMPENSACION { get; set; }
        public System.String RPECAUSAINTERRUPCION { get; set; }
        public System.String RPETRAMFUERMAYOR { get; set; }
        public System.Int32 RPEESTADO { get; set; }
        public System.Int32 RPEPUNTOENTREGA { get; set; }
        public System.String RPEPUNTOENTREGANOMBRE { get; set; }
        public System.Int32 RPEPERDCODI { get; set; }
        public System.String RPEPERDNOMB { get; set; }
        public System.Int32 RPETIPOINTCODI { get; set; }
        public System.String RPETIPOINTNOMBRE { get; set; }
        public System.String RPEUSUARIOCREACION { get; set; }
        public System.DateTime RPEFECHACREACION { get; set; }
        public System.String RPEUSUARIOUPDATE { get; set; }
        public System.DateTime RPEFECHAUPDATE { get; set; }
        public System.String RPEDURACION { get; set; }
        public System.String RPEBARRNOMBRE { get; set; }

        public bool IndicadorPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public int NroPagina { get; set; }

        public string BackgroundColor
        {
            get
            {
                string strReturn = "";

                switch (this.RPEESTADO)
                {
                    case 1:
                        strReturn = "#E6F584";
                        break;
                    case 2:
                        strReturn = "#FFFFFF";
                        break;
                    case 3:
                        strReturn = "#BFDFFF";
                        break;
                    case 4:
                        strReturn = "#E9E9E9";
                        break;
                    case 5:
                        strReturn = "#EBEBD8";
                        break;
                }

                return strReturn;
            }
        }
        public string FontColor
        {
            get
            {
                string strReturn = "";

                switch ("1")
                {
                    case "1":
                        strReturn = "#0000FF";
                        break;
                    //case "2":
                    //    strReturn = "#348741";
                    //    break;
                    //case "3":
                    //    strReturn = "#FF5C26";
                    //    break;
                    //case "4":
                    //    strReturn = "#E90314";
                    //    break;
                }

                return strReturn;
            }
        }
    }

    public class UsuarioModel
    {
        public int UsuarioCodi { get; set; }
        public string UsuarioVal { get; set; }
        public string UsuarioNombre { get; set; }
    }

    public class EmpresasResponsablesModel
    {
        public List<SiEmpresaDTO> ListaEmpresasResponsables { get; set; }
        public int Empgencodi { get; set; }
        public string Emprpenombre { get; set; }
        public decimal? Regporcentaje { get; set; }
        public string Peeusuariocreacion { get; set; }
        public DateTime? Peefechacreacion { get; set; }
        public string Peeusuarioupdate { get; set; }
        public DateTime? Peefechaupdate { get; set; }
        public int Regpuntoentcodi { get; set; }
        public int Emprcodi { get; set; }

        public SiEmpresaDTO ListaComboSeleccionar
        {
            get
            {
                SiEmpresaDTO a = new SiEmpresaDTO();
                a.Emprcodi = 0;
                a.Emprnomb = "";
                return a;
            }
        }
    }

    #endregion

    #region Métodos RechazoCarga

    public class RegistroRechazoCargaModel
    {
        public System.Int32 RRCKEY { get; set; }
        public System.Int32 RRCEMPRESAGENERADORA { get; set; }
        public System.String RRCEMPRESAGENERADORANOMBRE { get; set; }
        public System.Int32 RRCCLIENTE { get; set; }
        public System.String RRCCLIENTENOMBRE { get; set; }
        public System.Int32 RRCNIVELTENSION { get; set; }
        public System.String RRCNIVELTENSIONNOMB { get; set; }
        public System.DateTime RRCFECHAINICIO { get; set; }
        public System.DateTime RRCFECHAFIN { get; set; }
        public System.String RRCSUBESTACIONDSTRB { get; set; }
        public System.Decimal RRCNIVELTENSIONSED { get; set; }
        public System.String RRCCODIALIMENTADOR { get; set; }
        public System.Decimal RRCENERGIAENS { get; set; }
        public System.String RRCDURACION { get; set; }
        public System.Int32 RRCNRCF { get; set; }
        public System.Decimal RRCEF { get; set; }
        public System.Decimal RRCCOMPENSACION { get; set; }
        public System.Int32 RRCESTADO { get; set; }
        public System.Int32 RRCPUNTOENTREGA { get; set; }
        public System.String RRCPUNTOENTREGANOMBRE { get; set; }
        public System.Int32 RRCPERDCODI { get; set; }
        public System.String RPEPERDNOMB { get; set; }
        public System.Int32 RRCEVENCODI { get; set; }
        public System.String RRCEVENCODINOMB { get; set; }
        public System.String RRCUSUARIOCREACION { get; set; }
        public System.DateTime RRCFECHACREACION { get; set; }
        public System.String RRCUSUARIOUPDATE { get; set; }
        public System.DateTime RRCFECHAUPDATE { get; set; }

        public bool IndicadorPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public int NroPagina { get; set; }

        public string BackgroundColor
        {
            get
            {
                string strReturn = "";

                switch (this.RRCESTADO)
                {
                    case 1:
                        strReturn = "#E6F584";
                        break;
                    case 2:
                        strReturn = "#FFFFFF";
                        break;
                    case 3:
                        strReturn = "#BFDFFF";
                        break;
                    case 4:
                        strReturn = "#E9E9E9";
                        break;
                    case 5:
                        strReturn = "#EBEBD8";
                        break;
                }

                return strReturn;
            }
        }
        public string FontColor
        {
            get
            {
                string strReturn = "";

                switch ("1")
                {
                    case "1":
                        strReturn = "#0000FF";
                        break;
                    //case "2":
                    //    strReturn = "#348741";
                    //    break;
                    //case "3":
                    //    strReturn = "#FF5C26";
                    //    break;
                    //case "4":
                    //    strReturn = "#E90314";
                    //    break;
                }

                return strReturn;
            }
        }
    }

    public class ClienteModel
    {
        public List<SiEmpresaDTO> ListaClientes { get; set; }
        public int Emprcodi { get; set; }
        public string Emprnomb { get; set; }

        public static ClienteModel ListaComboTodos
        {
            get
            {
                ClienteModel a = new ClienteModel();
                a.Emprcodi = 0;
                a.Emprnomb = "(TODOS)";
                return a;
            }
        }
        public SiEmpresaDTO ListaComboSeleccionar
        {
            get
            {
                SiEmpresaDTO a = new SiEmpresaDTO();
                a.Emprcodi = 0;
                a.Emprnomb = "(TODOS)";
                return a;
            }
        }
    }



    #endregion

}