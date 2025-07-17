using COES.Base.Core;
using COES.Dominio.DTO.Campania;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Correo;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Transferencias;
using DocumentFormat.OpenXml.Office2010.Excel;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.Campanias
{
    public class CampaniasAppService : AppServicioBase
    {


        public CampaniasAppService()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        /// <summary>
        /// Instanciamiento de Log4net
        /// </summary>  
        private readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        CorreoAppServicio servCorreo = new CorreoAppServicio();

        public List<PeriodoDTO> ListPeriodos()
        {
            return FactoryCampania.ObtenerCamPeriodoDao().GetPeriodos();
        }

        public List<TipoProyectoDTO> ListTipoProyecto()
        {
            return FactoryCampania.ObtenerCamTipoproyectoDao().GetTipoProyecto();
        }

        public List<PeriodoDTO> ListPeriodosByAnioAndEstado(int anio, string estado)
        {
            return FactoryCampania.ObtenerCamPeriodoDao().GetPeriodosByAnioAndEstado(anio, estado);
        }

        public List<PeriodoDTO> ListPeriodosByAnio(int anio)
        {
            return FactoryCampania.ObtenerCamPeriodoDao().GetPeriodosByAnio(anio);
        }

        public bool SavePeriodo(PeriodoDTO per)
        {
            return FactoryCampania.ObtenerCamPeriodoDao().SavePeriodo(per);
        }

        public bool SaveFicha(List<DetallePeriodoDTO> detallePro)
        {
            bool result = false;
            foreach (DetallePeriodoDTO ficha in detallePro)
            {
                FactoryCampania.ObtenerCamDetallePeriodoDao().SaveDetalle(ficha);
                result = true;
            }
            return result;
        }

        public int GetPeriodoId()
        {
            return FactoryCampania.ObtenerCamPeriodoDao().GetPeriodoId();
        }
        public int GetFichaId()
        {
            return FactoryCampania.ObtenerCamDetallePeriodoDao().GetDetalleId();
        }

        public bool DeletePeriodoById(int pericodi, string usuario)
        {
            return FactoryCampania.ObtenerCamPeriodoDao().DeletePeriodoById(pericodi, usuario);
        }

        public bool DeleteFichaById(int pericodi, string usuario)
        {
            return FactoryCampania.ObtenerCamDetallePeriodoDao().DeleteDetalleById(pericodi, usuario);
        }

        public PeriodoDTO GetPeriodoDTOById(int pericodi)
        {
            return FactoryCampania.ObtenerCamPeriodoDao().GetPeriodoById(pericodi);
        }

        public int GetPeriodoByDate(DateTime perifechaini, DateTime perifechafin, int id)
        {
            return FactoryCampania.ObtenerCamPeriodoDao().GetPeriodoByDate(perifechaini,perifechafin,id);
        }

        public bool UpdatePeriodo(PeriodoDTO per)
        {
            return FactoryCampania.ObtenerCamPeriodoDao().UpdatePeriodo(per);
        }
        public List<CatalogoDTO> GetCatalogoXdesc(string descortaCat)
        {
            return FactoryCampania.ObtenerCamCatalogoDao().GetCatalogoXdesc(descortaCat);
        }

        public List<DataCatalogoDTO> ListParametria(int catcodi)
        {
            return FactoryCampania.ObtenerCamDatacatalogoDao().GetParametria(catcodi);
        }

        public List<DataCatalogoDTO> ListParametriaAll()
        {
            return FactoryCampania.ObtenerCamDatacatalogoDao().GetParametriaAll();
        }

        public List<DepartamentoDTO> GetDepartamentos()
        {
            return FactoryCampania.ObtenerCamDepartamentoDao().GetDepartamento();
        }

        public List<ProvinciaDTO> GetProvincias(string id)
        {
            return FactoryCampania.ObtenerCamProvinciaDao().GetListProvByDepId(id);
        }

        public List<DistritoDTO> GetDistrito(string id)
        {
            return FactoryCampania.ObtenerCamDistritoDao().GetListDistByProvDepId(id);
        }

        public UbicacionDTO GetUbicacionByDistrito(string id)
        {
            return FactoryCampania.ObtenerCamDistritoDao().GetUbicacionByProvDepId(id);
        }

        public List<RegHojaADTO> GetRegHojaAProyCodi(int proyCodi)
        {
            return FactoryCampania.ObtenerCamRegHojaADao().GetRegHojaAProyCodi(proyCodi);
        }

        public bool SaveRegHojaA(RegHojaADTO regHojaADTO)
        {
            return FactoryCampania.ObtenerCamRegHojaADao().SaveRegHojaA(regHojaADTO);
        }

        public bool DeleteRegHojaAById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCamRegHojaADao().DeleteRegHojaAById(id, usuario);
        }

        public int GetLastRegHojaAId()
        {
            return FactoryCampania.ObtenerCamRegHojaADao().GetLastRegHojaAId();
        }

        public RegHojaADTO GetRegHojaAById(int id)
        {
            return FactoryCampania.ObtenerCamRegHojaADao().GetRegHojaAById(id);
        }

        public bool UpdateRegHojaA(RegHojaADTO regHojaADTO)
        {
            return FactoryCampania.ObtenerCamRegHojaADao().UpdateRegHojaA(regHojaADTO);
        }

        public List<RegHojaADTO> GetRegHojaAByFilter(string plancodi, string empresa, string estado)
        {
            return FactoryCampania.ObtenerCamRegHojaADao().GetRegHojaAByFilter(plancodi, empresa, estado);
        }

        public List<RegHojaBDTO> GetRegHojaBByFilter(string plancodi, string empresa, string estado)
        {
            return FactoryCampania.ObtenerCamRegHojaBDao().GetRegHojaBByFilter(plancodi, empresa, estado);
        }

        public List<DetRegHojaCDTO> GetRegHojaCByFilter(string plancodi, string empresa, string estado)
        {
            return FactoryCampania.ObtenerCamRegHojaCDao().GetRegHojaCByFilter(plancodi, empresa, estado);
        }

        public List<TransmisionProyectoDTO> GetTransmisionProyecto(int id)
        {
            return FactoryCampania.ObtenerCamTransmisionProyectoDao().GetTransmisionProyecto(id);
        }

        public bool SaveTransmisionProyecto(TransmisionProyectoDTO transmisionProy)
        {
            return FactoryCampania.ObtenerCamTransmisionProyectoDao().SaveTransmisionProyecto(transmisionProy);
        }

        public bool DeleteTransmisionProyectoById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCamTransmisionProyectoDao().DeleteTransmisionProyectoById(id, usuario);
        }

        public int GetLastTransmisionProyectoId()
        {
            return FactoryCampania.ObtenerCamTransmisionProyectoDao().GetLastTransmisionProyectoId();
        }

        public TransmisionProyectoDTO GetTransmisionProyectoById(int id)
        {
            return FactoryCampania.ObtenerCamTransmisionProyectoDao().GetTransmisionProyectoById(id);
        }

        public bool UpdateTransmisionProyecto(TransmisionProyectoDTO transmisionProy)
        {
            return FactoryCampania.ObtenerCamTransmisionProyectoDao().UpdateTransmisionProyecto(transmisionProy);
        }

        public List<TransmisionProyectoDTO> GetTransmisionProyectoByPeriodo(int id)
        {
            return FactoryCampania.ObtenerCamTransmisionProyectoDao().GetTransmisionProyectoByPeriodo(id);
        }

         public bool UpdateProyEstadoById(int id, string proyestado)
        {
            return FactoryCampania.ObtenerCamTransmisionProyectoDao().UpdateProyEstadoById(id, proyestado);
        }

         public bool UpdateProyEstadoByIdProy(int id, string proyestado, string proyestadoini)
        {
            return FactoryCampania.ObtenerCamTransmisionProyectoDao().UpdateProyEstadoByIdProy(id, proyestado, proyestadoini);
        }

        public bool UpdateProyFechaEnvioObsById(int id)
        {
            return FactoryCampania.ObtenerCamTransmisionProyectoDao().UpdateProyFechaEnvioObsById(id);
        }


        public List<TransmisionProyectoDTO> GetTransmisionProyectoFilter(string pericodi, string empresa, string estado)
        {
            return FactoryCampania.ObtenerCamTransmisionProyectoDao().GetTransmisionProyectoByPeriodoFilter(pericodi, empresa, estado);
        }


        public string ObtenerPathArchivosCampianas()
        {
            string pathBase = ConfigurationManager.AppSettings[ConstantesCampania.RutaBaseCampania].ToString();
            return pathBase + ConstantesCampania.FolderCampanias;
        }

        public List<RegHojaBDTO> GetRegHojaBProyCodi(int proyCodi)
        {
            return FactoryCampania.ObtenerCamRegHojaBDao().GetRegHojaBProyCodi(proyCodi);
        }

        public bool SaveRegHojaB(RegHojaBDTO RegHojaBDTO)
        {
            return FactoryCampania.ObtenerCamRegHojaBDao().SaveRegHojaB(RegHojaBDTO);
        }

        public bool DeleteRegHojaBById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCamRegHojaBDao().DeleteRegHojaBById(id, usuario);
        }

        public int GetLastRegHojaBId()
        {
            return FactoryCampania.ObtenerCamRegHojaBDao().GetLastRegHojaBId();
        }

        public RegHojaBDTO GetRegHojaBById(int id)
        {
            return FactoryCampania.ObtenerCamRegHojaBDao().GetRegHojaBById(id);
        }

        public bool UpdateRegHojaB(RegHojaBDTO RegHojaBDTO)
        {
            return FactoryCampania.ObtenerCamRegHojaBDao().UpdateRegHojaB(RegHojaBDTO);
        }

        public List<RegHojaCDTO> GetRegHojaCProyCodi(int proyCodi)
        {
            return FactoryCampania.ObtenerCamRegHojaCDao().GetRegHojaCProyCodi(proyCodi);
        }

        public bool SaveRegHojaC(RegHojaCDTO regHojaCDTO)
        {
            return FactoryCampania.ObtenerCamRegHojaCDao().SaveRegHojaC(regHojaCDTO);
        }

        public bool DeleteRegHojaCById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCamRegHojaCDao().DeleteRegHojaCById(id, usuario);
        }

        public int GetLastRegHojaCId()
        {
            return FactoryCampania.ObtenerCamRegHojaCDao().GetLastRegHojaCId();
        }

        public RegHojaCDTO GetRegHojaCById(int id)
        {
            return FactoryCampania.ObtenerCamRegHojaCDao().GetRegHojaCById(id);
        }

        public bool UpdateRegHojaC(RegHojaCDTO regHojaCDTO)
        {
            return FactoryCampania.ObtenerCamRegHojaCDao().UpdateRegHojaC(regHojaCDTO);
        }

        public List<DetRegHojaCDTO> GetDetRegHojaCFichaCCodi(int fichaCCodi)
        {
            return FactoryCampania.ObtenerCamDetRegHojaCDao().GetDetRegHojaCFichaCCodi(fichaCCodi);
        }

        public bool SaveDetRegHojaC(DetRegHojaCDTO detRegHojaCDTO)
        {
            return FactoryCampania.ObtenerCamDetRegHojaCDao().SaveDetRegHojaC(detRegHojaCDTO);
        }

        public bool DeleteDetRegHojaCById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCamDetRegHojaCDao().DeleteDetRegHojaCById(id, usuario);
        }

        public int GetLastDetRegHojaCId()
        {
            return FactoryCampania.ObtenerCamDetRegHojaCDao().GetLastDetRegHojaCId();
        }

        public DetRegHojaCDTO GetDetRegHojaCById(int id)
        {
            return FactoryCampania.ObtenerCamDetRegHojaCDao().GetDetRegHojaCById(id);
        }

        public bool UpdateDetRegHojaC(DetRegHojaCDTO detRegHojaCDTO)
        {
            return FactoryCampania.ObtenerCamDetRegHojaCDao().UpdateDetRegHojaC(detRegHojaCDTO);
        }

        public List<ArchivoInfoDTO> GetArchivoInfoProyCodi(int proyCodi, int secccodi)
        {
            return FactoryCampania.ObtenerCamArchivoInfoDao().GetArchivoInfoProyCodi(proyCodi,secccodi);
        }

        public List<ArchivoInfoDTO> GetArchivoInfoByProyCodi(int proyCodi)
        {
            return FactoryCampania.ObtenerCamArchivoInfoDao().GetArchivoInfoByProyCodi(proyCodi);
        }

        public bool UpdateArchivoInfoByProyCodi(int proyCodi,string ruta)
        {
            return FactoryCampania.ObtenerCamArchivoInfoDao().UpdateArchivoInfoByProyCodi(proyCodi,ruta);
        }
        public ArchivoInfoDTO GetArchivoInfoNombreGenerado(string nombre)
        {
            return FactoryCampania.ObtenerCamArchivoInfoDao().GetArchivoInfoNombreArchivo(nombre);
        }

        public bool SaveArchivoInfo(ArchivoInfoDTO ArchivoInfo)
        {
            return FactoryCampania.ObtenerCamArchivoInfoDao().SaveArchivoInfo(ArchivoInfo);
        }

        public bool DeleteArchivoInfoById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCamArchivoInfoDao().DeleteArchivoInfoById(id, usuario);
        }

        public int GetLastArchivoInfoId()
        {
            return FactoryCampania.ObtenerCamArchivoInfoDao().GetLastArchivoInfoId();
        }

        public ArchivoInfoDTO GetArchivoInfoById(int id)
        {
            return FactoryCampania.ObtenerCamArchivoInfoDao().GetArchivoInfoById(id);
        }

        public bool UpdateArchivoInfo(ArchivoInfoDTO ArchivoInfoDTO)
        {
            return FactoryCampania.ObtenerCamArchivoInfoDao().UpdateArchivoInfo(ArchivoInfoDTO);
        }

        public List<ArchivoInfoDTO> GetArchivoInfoProyCodiNom(int proyCodi, int secccodi, string nombre)
        {
            return FactoryCampania.ObtenerCamArchivoInfoDao().GetArchivoInfoProyCodiNom(proyCodi,secccodi,nombre);
        }

        public List<SeccionHojasDTO> GetSeccionHojaByHojaCod(int hojaCodi)
        {
            return FactoryCampania.ObtenerCamSeccionHojaDao().GetSeccionHojaByHojaCod(hojaCodi);
        }

        public List<RegHojaDDTO> GetRegHojaDProyCodi(int proyCodi)
        {
            return FactoryCampania.ObtenerCamRegHojaDDao().GetRegHojaDProyCodi(proyCodi);
        }

        public bool SaveRegHojaD(RegHojaDDTO RegHojaDTO)
        {
            return FactoryCampania.ObtenerCamRegHojaDDao().SaveRegHojaD(RegHojaDTO);
        }

        public bool DeleteRegHojaDById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCamRegHojaDDao().DeleteRegHojaDById(id, usuario);
        }
        public bool DeleteRegHojaDById2(int id, string usuario)
        {
            return FactoryCampania.ObtenerCamRegHojaDDao().DeleteRegHojaDById2(id, usuario);
        }

        public int GetLastRegHojaDId()
        {
            return FactoryCampania.ObtenerCamRegHojaDDao().GetLastRegHojaDId();
        }

        public List<RegHojaDDTO> GetRegHojaDById(int id)
        {
            return FactoryCampania.ObtenerCamRegHojaDDao().GetRegHojaDById(id);
        }

        public List<RegHojaDDTO> GetRegHojaDByFilter(string plancodi, string empresa, string estado)
        {
            return FactoryCampania.ObtenerCamRegHojaDDao().GetRegHojaDByFilter(plancodi, empresa, estado);
        }


        public bool UpdateRegHojaD(RegHojaDDTO RegHojaDDTO)
        {
            return FactoryCampania.ObtenerCamRegHojaDDao().UpdateRegHojaD(RegHojaDDTO);
        }

        public List<PlanTransmisionDTO> GetPlanTransmision()
        {
            return FactoryCampania.ObtenerCamPlanTransmisionDao().GetPlanTransmision();
        }

        public bool SavePlanTransmision(PlanTransmisionDTO transmisionProy)
        {
            return FactoryCampania.ObtenerCamPlanTransmisionDao().SavePlanTransmision(transmisionProy);
        }

        public bool DeletePlanTransmisionById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCamPlanTransmisionDao().DeletePlanTransmisionById(id, usuario);
        }

        public int GetLastPlanTransmisionId()
        {
            return FactoryCampania.ObtenerCamPlanTransmisionDao().GetLastPlanTransmisionId();
        }

        public PlanTransmisionDTO GetPlanTransmisionById(int id)
        {
            return FactoryCampania.ObtenerCamPlanTransmisionDao().GetPlanTransmisionById(id);
        }

         public List<PlanTransmisionDTO> GetPlanTransmisionByEstado(string empresa, string estado, string periodo, string vigente, string fueraplazo, string estadoExcl)
        {
            return FactoryCampania.ObtenerCamPlanTransmisionDao().GetPlanTransmisionByEstado(empresa, estado, periodo, vigente, fueraplazo, estadoExcl);
        }

        public List<PlanTransmisionDTO> GetPlanTransmisionByEstadoEmpresa(string empresa, string estado, string periodo, string vigente, string fueraplazo, string estadoExcl)
        {
            return FactoryCampania.ObtenerCamPlanTransmisionDao().GetPlanTransmisionByEstadoEmpresa(empresa, estado, periodo, vigente, fueraplazo, estadoExcl);
        }

        public bool UpdatePlanTransmision(PlanTransmisionDTO transmisionProy)
        {
            return FactoryCampania.ObtenerCamPlanTransmisionDao().UpdatePlanTransmision(transmisionProy);
        }
        public List<PlanTransmisionDTO> GetPlanTransmisionByFilters(int id)
        {
            return FactoryCampania.ObtenerCamPlanTransmisionDao().GetPlanTransmisionByFilters(id);
        }
        public bool DesactivatePlanById(int id)
        {
            return FactoryCampania.ObtenerCamPlanTransmisionDao().DesactivatePlanById(id, Vigencia.Desactivado);
        }
        public bool ActivatePlanById(int id)
        {
            return FactoryCampania.ObtenerCamPlanTransmisionDao().ActivatePlanById(id, Vigencia.Activado);
        }

        public bool UpdatePlanEstadoById(int id, string planestado)
        {
            return FactoryCampania.ObtenerCamPlanTransmisionDao().UpdatePlanEstadoById(id, planestado);
        }

        public bool UpdatePlanEstadoEnviarById(int id, string planestado, string correo)
        {
            return FactoryCampania.ObtenerCamPlanTransmisionDao().UpdatePlanEstadoEnviarById(id, planestado, correo);
        }

        public List<PlanTransmisionDTO> GetPlanTransmisionByEstadoVigente(string empresa, string estado, string periodo, string tipoproyecto, string subtipoproyecto, string observados, string estadoExcl)
        {
            return FactoryCampania.ObtenerCamPlanTransmisionDao().GetPlanTransmisionByEstadoVigente(empresa, estado, periodo, tipoproyecto, subtipoproyecto, observados, estadoExcl);
        }

        public List<PlanTransmisionDTO> GetPlanTransmisionByVigente(string empresa, string estado, string periodo, string estadoExcl)
        {
            return FactoryCampania.ObtenerCamPlanTransmisionDao().GetPlanTransmisionByVigente(empresa, estado, periodo, estadoExcl);
        }

        public bool UpdateProyRegById(int id)
        {
            return FactoryCampania.ObtenerCamPlanTransmisionDao().UpdateProyRegById(id);
        }

        public List<DetRegHojaDDTO> GetDetRegHojaDFichaCCodi(String proyCodi)
        {
            return FactoryCampania.ObtenerCamDetRegHojaDDao().GetDetRegHojaDFichaCCodi(proyCodi);
        }

        public bool SaveDetRegHojaD(DetRegHojaDDTO DetRegHojaDTO)
        {
            return FactoryCampania.ObtenerCamDetRegHojaDDao().SaveDetRegHojaD(DetRegHojaDTO);
        }

        public bool DeleteDetRegHojaDById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCamDetRegHojaDDao().DeleteDetRegHojaDById(id, usuario);
        }

        public int GetLastDetRegHojaDId()
        {
            return FactoryCampania.ObtenerCamDetRegHojaDDao().GetLastDetRegHojaDId();
        }

        public DetRegHojaDDTO GetDetRegHojaDById(string id)
        {
            return FactoryCampania.ObtenerCamDetRegHojaDDao().GetDetRegHojaDById(id);
        }

        public bool UpdateDetRegHojaD(DetRegHojaDDTO DetRegHojaDDTO)
        {
            return FactoryCampania.ObtenerCamDetRegHojaDDao().UpdateDetRegHojaD(DetRegHojaDDTO);
        }

        public List<RegHojaCCTTADTO> GetRegHojaCCTTAProyCodi(int proyCodi)
        {
            return FactoryCampania.ObtenerCamRegHojaCCTTADao().GetRegHojaCCTTAProyCodi(proyCodi);
        }

        public bool SaveRegHojaCCTTA(RegHojaCCTTADTO regHojaCCTTADTO)
        {
            return FactoryCampania.ObtenerCamRegHojaCCTTADao().SaveRegHojaCCTTA(regHojaCCTTADTO);
        }

        public bool DeleteRegHojaCCTTAById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCamRegHojaCCTTADao().DeleteRegHojaCCTTAById(id, usuario);
        }

        public int GetLastRegHojaCCTTAId()
        {
            return FactoryCampania.ObtenerCamRegHojaCCTTADao().GetLastRegHojaCCTTAId();
        }

        public RegHojaCCTTADTO GetRegHojaCCTTAById(int id)
        {
            return FactoryCampania.ObtenerCamRegHojaCCTTADao().GetRegHojaCCTTAById(id);
        }

        public List<RegHojaCCTTADTO> GetRegHojaCCTTAByFilter(string plancodi, string empresa, string estado)
        {
            return FactoryCampania.ObtenerCamRegHojaCCTTADao().GetRegHojaCCTTAByFilter(plancodi, empresa, estado);
        }


        public bool UpdateRegHojaCCTTA(RegHojaCCTTADTO regHojaCCTTADTO)
        {
            return FactoryCampania.ObtenerCamRegHojaCCTTADao().UpdateRegHojaCCTTA(regHojaCCTTADTO);
        }

        public List<RegHojaCCTTBDTO> GetRegHojaCCTTBProyCodi(int proyCodi)
        {
            return FactoryCampania.ObtenerCamRegHojaCCTTBDao().GetRegHojaCCTTBProyCodi(proyCodi);
        }

        public bool SaveRegHojaCCTTB(RegHojaCCTTBDTO RegHojaCCTTBDTO)
        {
            return FactoryCampania.ObtenerCamRegHojaCCTTBDao().SaveRegHojaCCTTB(RegHojaCCTTBDTO);
        }

        public bool DeleteRegHojaCCTTBById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCamRegHojaCCTTBDao().DeleteRegHojaCCTTBById(id, usuario);
        }

        public int GetLastRegHojaCCTTBId()
        {
            return FactoryCampania.ObtenerCamRegHojaCCTTBDao().GetLastRegHojaCCTTBId();
        }

        public RegHojaCCTTBDTO GetRegHojaCCTTBById(int id)
        {
            return FactoryCampania.ObtenerCamRegHojaCCTTBDao().GetRegHojaCCTTBById(id);
        }

        public List<RegHojaCCTTBDTO> GetRegHojaCCTTBByFilter(string plancodi, string empresa, string estado)
        {
            return FactoryCampania.ObtenerCamRegHojaCCTTBDao().GetRegHojaCCTTBByFilter(plancodi, empresa, estado);
        }

        public bool UpdateRegHojaCCTTB(RegHojaCCTTBDTO RegHojaCCTTBDTO)
        {
            return FactoryCampania.ObtenerCamRegHojaCCTTBDao().UpdateRegHojaCCTTB(RegHojaCCTTBDTO);
        }

        public List<RegHojaCCTTCDTO> GetRegHojaCCTTCProyCodi(int proyCodi)
        {
            return FactoryCampania.ObtenerCamRegHojaCCTTCDao().GetRegHojaCCTTCProyCodi(proyCodi);
        }

        public bool SaveRegHojaCCTTC(RegHojaCCTTCDTO regHojaCCTTCDTO)
        {
            return FactoryCampania.ObtenerCamRegHojaCCTTCDao().SaveRegHojaCCTTC(regHojaCCTTCDTO);
        }

        public bool DeleteRegHojaCCTTCById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCamRegHojaCCTTCDao().DeleteRegHojaCCTTCById(id, usuario);
        }

        public int GetLastRegHojaCCTTCId()
        {
            return FactoryCampania.ObtenerCamRegHojaCCTTCDao().GetLastRegHojaCCTTCId();
        }

        public RegHojaCCTTCDTO GetRegHojaCCTTCById(int id)
        {
            return FactoryCampania.ObtenerCamRegHojaCCTTCDao().GetRegHojaCCTTCById(id);
        }

        public bool UpdateRegHojaCCTTC(RegHojaCCTTCDTO regHojaCCTTCDTO)
        {
            return FactoryCampania.ObtenerCamRegHojaCCTTCDao().UpdateRegHojaCCTTC(regHojaCCTTCDTO);
        }

        public List<Det1RegHojaCCTTCDTO> GetRegHojaCCTTCByFilter(string plancodi, string empresa, string estado)
        {
            return FactoryCampania.ObtenerCamRegHojaCCTTCDao().GetRegHojaCCTTCByFilter(plancodi, empresa, estado);
        }

        public List<Det2RegHojaCCTTCDTO> GetRegHojaCCTTC2ByFilter(string plancodi, string empresa, string estado)
        {
            return FactoryCampania.ObtenerCamRegHojaCCTTCDao().GetRegHojaCCTTC2ByFilter(plancodi, empresa, estado);
        }

        public List<Det1RegHojaCCTTCDTO> GetDet1RegHojaCCTTCCentralCodi(int fichaCCodi)
        {
            return FactoryCampania.ObtenerCamDet1RegHojaCCTTCDao().GetDet1RegHojaCCTTCCentralCodi(fichaCCodi);
        }

        public bool SaveDet1RegHojaCCTTC(Det1RegHojaCCTTCDTO detRegHojaCCTTCDTO)
        {
            return FactoryCampania.ObtenerCamDet1RegHojaCCTTCDao().SaveDet1RegHojaCCTTC(detRegHojaCCTTCDTO);
        }

        public bool DeleteDet1RegHojaCCTTCById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCamDet1RegHojaCCTTCDao().DeleteDet1RegHojaCCTTCById(id, usuario);
        }

        public int GetLastDet1RegHojaCCTTCId()
        {
            return FactoryCampania.ObtenerCamDet1RegHojaCCTTCDao().GetLastDet1RegHojaCCTTCId();
        }

        public Det1RegHojaCCTTCDTO GetDet1RegHojaCCTTCById(int id)
        {
            return FactoryCampania.ObtenerCamDet1RegHojaCCTTCDao().GetDet1RegHojaCCTTCById(id);
        }

        public bool UpdateDet1RegHojaCCTTC(Det1RegHojaCCTTCDTO detRegHojaCCTTCDTO)
        {
            return FactoryCampania.ObtenerCamDet1RegHojaCCTTCDao().UpdateDet1RegHojaCCTTC(detRegHojaCCTTCDTO);
        }

        public List<Det2RegHojaCCTTCDTO> GetDet2RegHojaCCTTCCentralCodi(int fichaCCodi)
        {
            return FactoryCampania.ObtenerCamDet2RegHojaCCTTCDao().GetDet2RegHojaCCTTCCentralCodi(fichaCCodi);
        }

        public bool SaveDet2RegHojaCCTTC(Det2RegHojaCCTTCDTO detRegHojaCCTTCDTO)
        {
            return FactoryCampania.ObtenerCamDet2RegHojaCCTTCDao().SaveDet2RegHojaCCTTC(detRegHojaCCTTCDTO);
        }

        public bool DeleteDet2RegHojaCCTTCById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCamDet2RegHojaCCTTCDao().DeleteDet2RegHojaCCTTCById(id, usuario);
        }

        public int GetLastDet2RegHojaCCTTCId()
        {
            return FactoryCampania.ObtenerCamDet2RegHojaCCTTCDao().GetLastDet2RegHojaCCTTCId();
        }

        public Det2RegHojaCCTTCDTO GetDet2RegHojaCCTTCById(int id)
        {
            return FactoryCampania.ObtenerCamDet2RegHojaCCTTCDao().GetDet2RegHojaCCTTCById(id);
        }

        public bool UpdateDet2RegHojaCCTTC(Det2RegHojaCCTTCDTO detRegHojaCCTTCDTO)
        {
            return FactoryCampania.ObtenerCamDet2RegHojaCCTTCDao().UpdateDet2RegHojaCCTTC(detRegHojaCCTTCDTO);
        }

        public List<int> GetDetalleHojaByPericodi(int pericodi, string inddel)
        {
            return FactoryCampania.ObtenerCamDetallePeriodoDao().GetDetalleHojaByPericodi(pericodi, inddel);
        }

        public List<SolHojaADTO> GetSolHojaAProyCodi(int proyCodi)
        {
            return FactoryCampania.ObtenerCamSolHojaADao().GetSolHojaAProyCodi(proyCodi);
        }

        public bool SaveSolHojaA(SolHojaADTO SolHojaADTO)
        {
            return FactoryCampania.ObtenerCamSolHojaADao().SaveSolHojaA(SolHojaADTO);
        }

        public bool DeleteSolHojaAById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCamSolHojaADao().DeleteSolHojaAById(id, usuario);
        }

        public int GetLastSolHojaAId()
        {
            return FactoryCampania.ObtenerCamSolHojaADao().GetLastSolHojaAId();
        }

        public SolHojaADTO GetSolHojaAById(int id)
        {
            return FactoryCampania.ObtenerCamSolHojaADao().GetSolHojaAById(id);
        }

        public List<SolHojaADTO> GetSolHojaAByFilter(string plancodi, string empresa, string estado)
        {
            return FactoryCampania.ObtenerCamSolHojaADao().GetSolHojaAByFilter(plancodi, empresa, estado);
        }

        public bool UpdateSolHojaA(SolHojaADTO SolHojaADTO)
        {
            return FactoryCampania.ObtenerCamSolHojaADao().UpdateSolHojaA(SolHojaADTO);
        }

        public List<SolHojaBDTO> GetSolHojaBProyCodi(int proyCodi)
        {
            return FactoryCampania.ObtenerCamSolHojaBDao().GetSolHojaBProyCodi(proyCodi);
        }

        public bool SaveSolHojaB(SolHojaBDTO SolHojaBDTO)
        {
            return FactoryCampania.ObtenerCamSolHojaBDao().SaveSolHojaB(SolHojaBDTO);
        }

        public bool DeleteSolHojaBById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCamSolHojaBDao().DeleteSolHojaBById(id, usuario);
        }

        public int GetLastSolHojaBId()
        {
            return FactoryCampania.ObtenerCamSolHojaBDao().GetLastSolHojaBId();
        }

        public SolHojaBDTO GetSolHojaBById(int id)
        {
            return FactoryCampania.ObtenerCamSolHojaBDao().GetSolHojaBById(id);
        }

        public List<SolHojaBDTO> GetSolHojaBByFilter(string plancodi, string empresa, string estado)
        {
            return FactoryCampania.ObtenerCamSolHojaBDao().GetSolHojaBByFilter(plancodi, empresa, estado);
        }

        public bool UpdateSolHojaB(SolHojaBDTO SolHojaBDTO)
        {
            return FactoryCampania.ObtenerCamSolHojaBDao().UpdateSolHojaB(SolHojaBDTO);
        }

        public List<RegHojaASubestDTO> GetRegHojaASubestProyCodi(int proyCodi)
        {
            return FactoryCampania.ObtenerCamRegHojaASubestDao().GetRegHojaASubestProyCodi(proyCodi);
        }

        public bool SaveRegHojaASubest(RegHojaASubestDTO regHojaASubestDTO)
        {
            return FactoryCampania.ObtenerCamRegHojaASubestDao().SaveRegHojaASubest(regHojaASubestDTO);
        }

        public bool DeleteRegHojaASubestById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCamRegHojaASubestDao().DeleteRegHojaASubestById(id, usuario);
        }

        public int GetLastRegHojaASubestId()
        {
            return FactoryCampania.ObtenerCamRegHojaASubestDao().GetLastRegHojaASubestId();
        }

        public RegHojaASubestDTO GetRegHojaASubestById(int id)
        {
            return FactoryCampania.ObtenerCamRegHojaASubestDao().GetRegHojaASubestById(id);
        }

        public bool UpdateRegHojaASubest(RegHojaASubestDTO regHojaASubestDTO)
        {
            return FactoryCampania.ObtenerCamRegHojaASubestDao().UpdateRegHojaASubest(regHojaASubestDTO);
        }

        public List<DetRegHojaASubestDTO> GetDetRegHojaASubestFichaCCodi(int fichaCCodi)
        {
            return FactoryCampania.ObtenerCamDetRegHojaASubestDao().GetDetRegHojaASubestFichaCCodi(fichaCCodi);
        }

        public bool SaveDetRegHojaASubest(DetRegHojaASubestDTO detRegHojaASubestDTO)
        {
            return FactoryCampania.ObtenerCamDetRegHojaASubestDao().SaveDetRegHojaASubest(detRegHojaASubestDTO);
        }

        public bool DeleteDetRegHojaASubestById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCamDetRegHojaASubestDao().DeleteDetRegHojaASubestById(id, usuario);
        }

        public int GetLastDetRegHojaASubestId()
        {
            return FactoryCampania.ObtenerCamDetRegHojaASubestDao().GetLastDetRegHojaASubestId();
        }

        public DetRegHojaASubestDTO GetDetRegHojaASubestById(int id)
        {
            return FactoryCampania.ObtenerCamDetRegHojaASubestDao().GetDetRegHojaASubestById(id);
        }

        public bool UpdateDetRegHojaASubest(DetRegHojaASubestDTO detRegHojaASubestDTO)
        {
            return FactoryCampania.ObtenerCamDetRegHojaASubestDao().UpdateDetRegHojaASubest(detRegHojaASubestDTO);
        }
        public List<RegHojaEolADTO> GetRegHojaEolACodi(int proyCodi)
        {
            return FactoryCampania.ObtenerCamRegHojaEolADao().GetRegHojaEolACodi(proyCodi);
        }

        public bool SaveRegHojaEolA(RegHojaEolADTO regHojaEolADTO)
        {
            return FactoryCampania.ObtenerCamRegHojaEolADao().SaveRegHojaEolA(regHojaEolADTO);
        }

        public bool DeleteRegHojaEolAById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCamRegHojaEolADao().DeleteRegHojaEolAById(id, usuario);
        }

        public int GetLastRegHojaEolAId()
        {
            return FactoryCampania.ObtenerCamRegHojaEolADao().GetLastRegHojaEolAId();
        }

        public RegHojaEolADTO GetRegHojaEolAById(int id)
        {
            return FactoryCampania.ObtenerCamRegHojaEolADao().GetRegHojaEolAById(id);
        }

        public List<RegHojaEolADTO> GetRegHojaEolAByFilter(string plancodi, string empresa, string estado)
        {
            return FactoryCampania.ObtenerCamRegHojaEolADao().GetRegHojaEolAByFilter(plancodi, empresa, estado);
        }

        public bool UpdateRegHojaEolA(RegHojaEolADTO regHojaEolADTO)
        {
            return FactoryCampania.ObtenerCamRegHojaEolADao().UpdateRegHojaEolA(regHojaEolADTO);
        }

        public List<RegHojaEolADetDTO> GetRegHojaEolADetCodi(int proyCodi)
        {
            return FactoryCampania.ObtenerCamRegHojaEolADetDao().GetRegHojaEolADetCodi(proyCodi);
        }

        public bool SaveRegHojaEolADet(RegHojaEolADetDTO regHojaEolADetDTO)
        {
            return FactoryCampania.ObtenerCamRegHojaEolADetDao().SaveRegHojaEolADet(regHojaEolADetDTO);
        }

        public bool DeleteRegHojaEolADetById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCamRegHojaEolADetDao().DeleteRegHojaEolADetById(id, usuario);
        }

        public int GetLastRegHojaEolADetId()
        {
            return FactoryCampania.ObtenerCamRegHojaEolADetDao().GetLastRegHojaEolADetId();
        }

        public RegHojaEolADetDTO GetRegHojaEolADetById(int id)
        {
            return FactoryCampania.ObtenerCamRegHojaEolADetDao().GetRegHojaEolADetById(id);
        }

        public List<RegHojaEolBDTO> GetRegHojaEolBCodi(int proyCodi)
        {
            return FactoryCampania.ObtenerCamRegHojaEolBDao().GetRegHojaEolBCodi(proyCodi);
        }

        public bool SaveRegHojaEolB(RegHojaEolBDTO regHojaEolBDTO)
        {
            return FactoryCampania.ObtenerCamRegHojaEolBDao().SaveRegHojaEolB(regHojaEolBDTO);
        }

        public bool DeleteRegHojaEolBById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCamRegHojaEolBDao().DeleteRegHojaEolBById(id, usuario);
        }

        public int GetLastRegHojaEolBId()
        {
            return FactoryCampania.ObtenerCamRegHojaEolBDao().GetLastRegHojaEolBId();
        }

        public List<RegHojaEolBDTO> GetRegHojaEolBByFilter(string plancodi, string empresa, string estado)
        {
            return FactoryCampania.ObtenerCamRegHojaEolBDao().GetRegHojaEolBByFilter(plancodi, empresa, estado);
        }

        public RegHojaEolBDTO GetRegHojaEolBById(int id)
        {
            return FactoryCampania.ObtenerCamRegHojaEolBDao().GetRegHojaEolBById(id);
        }


        public bool UpdateRegHojaEolB(RegHojaEolBDTO regHojaEolBDTO)
        {
            return FactoryCampania.ObtenerCamRegHojaEolBDao().UpdateRegHojaEolB(regHojaEolBDTO);
        }

        public List<RegHojaEolCDTO> GetRegHojaEolCCodi(int proyCodi)
        {
            return FactoryCampania.ObtenerCamRegHojaEolCDao().GetRegHojaEolCCodi(proyCodi);
        }

        public bool SaveRegHojaEolC(RegHojaEolCDTO regHojaEolCDTO)
        {
            return FactoryCampania.ObtenerCamRegHojaEolCDao().SaveRegHojaEolC(regHojaEolCDTO);
        }

        public bool DeleteRegHojaEolCById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCamRegHojaEolCDao().DeleteRegHojaEolCById(id, usuario);
        }

        public int GetLastRegHojaEolCId()
        {
            return FactoryCampania.ObtenerCamRegHojaEolCDao().GetLastRegHojaEolCId();
        }

        public RegHojaEolCDTO GetRegHojaEolCById(int id)
        {
            return FactoryCampania.ObtenerCamRegHojaEolCDao().GetRegHojaEolCById(id);
        }

        public bool UpdateRegHojaEolC(RegHojaEolCDTO regHojaEolCDTO)
        {
            return FactoryCampania.ObtenerCamRegHojaEolCDao().UpdateRegHojaEolC(regHojaEolCDTO);
        }

        public List<DetRegHojaEolCDTO> GetRegHojaEolCByFilter(string plancodi, string empresa, string estado)
        {
            return FactoryCampania.ObtenerCamRegHojaEolCDao().GetRegHojaEolCByFilter(plancodi, empresa, estado);
        }

        public List<DetRegHojaEolCDTO> GetDetRegHojaEolCCodi(int id)
        {
            return FactoryCampania.ObtenerDetCamRegHojaEolCDao().GetDetRegHojaEolCCodi(id);
        }

        public bool SaveDetRegHojaEolC(DetRegHojaEolCDTO detRegHojaEolCDTO)
        {
            return FactoryCampania.ObtenerDetCamRegHojaEolCDao().SaveDetRegHojaEolC(detRegHojaEolCDTO);
        }

        public bool DeleteDetRegHojaEolCById(int id, string usuario)
        {
            return FactoryCampania.ObtenerDetCamRegHojaEolCDao().DeleteDetRegHojaEolCById(id, usuario);
        }

        public int GetLastDetRegHojaEolCId()
        {
            return FactoryCampania.ObtenerDetCamRegHojaEolCDao().GetLastDetRegHojaEolCId();
        }

        public DetRegHojaEolCDTO GetDetRegHojaEolCById(int id)
        {
            return FactoryCampania.ObtenerDetCamRegHojaEolCDao().GetDetRegHojaEolCById(id);
        }

        public bool UpdateDetRegHojaEolC(DetRegHojaEolCDTO detRegHojaEolCDTO)
        {
            return FactoryCampania.ObtenerDetCamRegHojaEolCDao().UpdateDetRegHojaEolC(detRegHojaEolCDTO);
        }



        public List<SolHojaCDTO> GetSolHojaCProyCodi(int proyCodi)
        {
            return FactoryCampania.ObtenerCamSolHojaCDao().GetSolHojaCProyCodi(proyCodi);
        }

        public bool SaveSolHojaC(SolHojaCDTO SolHojaCDTO)
        {
            return FactoryCampania.ObtenerCamSolHojaCDao().SaveSolHojaC(SolHojaCDTO);
        }

        public bool DeleteSolHojaCById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCamSolHojaCDao().DeleteSolHojaCById(id, usuario);
        }

        public int GetLastSolHojaCId()
        {
            return FactoryCampania.ObtenerCamSolHojaCDao().GetLastSolHojaCId();
        }

        public SolHojaCDTO GetSolHojaCById(int id)
        {
            return FactoryCampania.ObtenerCamSolHojaCDao().GetSolHojaCById(id);
        }

        public bool UpdateSolHojaC(SolHojaCDTO SolHojaCDTO)
        {
            return FactoryCampania.ObtenerCamSolHojaCDao().UpdateSolHojaC(SolHojaCDTO);
        }

        public List<DetSolHojaCDTO> GetSolHojaCByFilter(string plancodi, string empresa, string estado)
        {
            return FactoryCampania.ObtenerCamSolHojaCDao().GetSolHojaCByFilter(plancodi, empresa, estado);
        }

        public List<DetSolHojaCDTO> GetDetSolHojaCCodi(int proyCodi)
        {
            return FactoryCampania.ObtenerCamDetSolHojaCDao().GetDetSolHojaCCodi(proyCodi);
        }

        public bool SaveDetSolHojaC(DetSolHojaCDTO DetSolHojaCDTO)
        {
            return FactoryCampania.ObtenerCamDetSolHojaCDao().SaveDetSolHojaC(DetSolHojaCDTO);
        }

        public bool DeleteDetSolHojaCById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCamDetSolHojaCDao().DeleteDetSolHojaCById(id, usuario);
        }

        public int GetLastDetSolHojaCId()
        {
            return FactoryCampania.ObtenerCamDetSolHojaCDao().GetLastDetSolHojaCId();
        }

        public DetSolHojaCDTO GetDetSolHojaCById(int id)
        {
            return FactoryCampania.ObtenerCamDetSolHojaCDao().GetDetSolHojaCById(id);
        }

        public bool UpdateDetSolHojaC(DetSolHojaCDTO DetSolHojaCDTO)
        {
            return FactoryCampania.ObtenerCamDetSolHojaCDao().UpdateDetSolHojaC(DetSolHojaCDTO);
        }


        public List<BioHojaADTO> GetBioHojaAProyCodi(int proyCodi)
        {
            return FactoryCampania.ObtenerCamBioHojaADao().GetBioHojaAProyCodi(proyCodi);
        }

        public bool SaveBioHojaA(BioHojaADTO BioHojaADTO)
        {
            return FactoryCampania.ObtenerCamBioHojaADao().SaveBioHojaA(BioHojaADTO);
        }

        public bool DeleteBioHojaAById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCamBioHojaADao().DeleteBioHojaAById(id, usuario);
        }

        public int GetLastBioHojaAId()
        {
            return FactoryCampania.ObtenerCamBioHojaADao().GetLastBioHojaAId();
        }

        public BioHojaADTO GetBioHojaAById(int id)
        {
            return FactoryCampania.ObtenerCamBioHojaADao().GetBioHojaAById(id);
        }

        public List<BioHojaADTO> GetBioHojaAByFilter(string plancodi, string empresa, string estado)
        {
            return FactoryCampania.ObtenerCamBioHojaADao().GetBioHojaAByFilter(plancodi, empresa, estado);
        }


        public bool UpdateBioHojaA(BioHojaADTO BioHojaADTO)
        {
            return FactoryCampania.ObtenerCamBioHojaADao().UpdateBioHojaA(BioHojaADTO);
        }



        public List<BioHojaBDTO> GetBioHojaBProyCodi(int proyCodi)
        {
            return FactoryCampania.ObtenerCamBioHojaBDao().GetBioHojaBProyCodi(proyCodi);
        }

        public bool SaveBioHojaB(BioHojaBDTO BioHojaBDTO)
        {
            return FactoryCampania.ObtenerCamBioHojaBDao().SaveBioHojaB(BioHojaBDTO);
        }

        public bool DeleteBioHojaBById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCamBioHojaBDao().DeleteBioHojaBById(id, usuario);
        }

        public int GetLastBioHojaBId()
        {
            return FactoryCampania.ObtenerCamBioHojaBDao().GetLastBioHojaBId();
        }

        public BioHojaBDTO GetBioHojaBById(int id)
        {
            return FactoryCampania.ObtenerCamBioHojaBDao().GetBioHojaBById(id);
        }

        public List<BioHojaBDTO> GetBioHojaBByFilter(string plancodi, string empresa, string estado)
        {
            return FactoryCampania.ObtenerCamBioHojaBDao().GetBioHojaBByFilter(plancodi, empresa, estado);
        }

        public bool UpdateBioHojaB(BioHojaBDTO BioHojaBDTO)
        {
            return FactoryCampania.ObtenerCamBioHojaBDao().UpdateBioHojaB(BioHojaBDTO);
        }


        public List<BioHojaCDTO> GetBioHojaCProyCodi(int proyCodi)
        {
            return FactoryCampania.ObtenerCamBioHojaCDao().GetBioHojaCProyCodi(proyCodi);
        }



        public bool SaveBioHojaC(BioHojaCDTO BioHojaCDTO)
        {
            return FactoryCampania.ObtenerCamBioHojaCDao().SaveBioHojaC(BioHojaCDTO);
        }

        public bool DeleteBioHojaCById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCamBioHojaCDao().DeleteBioHojaCById(id, usuario);
        }

        public int GetLastBioHojaCId()
        {
            return FactoryCampania.ObtenerCamBioHojaCDao().GetLastBioHojaCId();
        }

        public BioHojaCDTO GetBioHojaCById(int id)
        {
            return FactoryCampania.ObtenerCamBioHojaCDao().GetBioHojaCById(id);
        }

        public List<DetBioHojaCDTO> GetBioHojaCByFilter(string plancodi, string empresa, string estado)
        {
            return FactoryCampania.ObtenerCamBioHojaCDao().GetBioHojaCByFilter(plancodi, empresa, estado);
        }

        public bool UpdateBioHojaC(BioHojaCDTO BioHojaCDTO)
        {
            return FactoryCampania.ObtenerCamBioHojaCDao().UpdateBioHojaC(BioHojaCDTO);
        }


        public List<DetBioHojaCDTO> GetDetBioHojaCCodi(int proycodi)
        {
            return FactoryCampania.ObtenerCamDetBioHojaCDao().GetDetBioHojaCCodi(proycodi);
        }

        public bool SaveDetBioHojaC(DetBioHojaCDTO DetBioHojaCDTO)
        {
            return FactoryCampania.ObtenerCamDetBioHojaCDao().SaveDetBioHojaC(DetBioHojaCDTO);
        }

        public bool DeleteDetBioHojaCById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCamDetBioHojaCDao().DeleteDetBioHojaCById(id, usuario);
        }

        public int GetLastDetBioHojaCId()
        {
            return FactoryCampania.ObtenerCamDetBioHojaCDao().GetLastDetBioHojaCId();
        }

        public DetBioHojaCDTO GetDetBioHojaCById(int id)
        {
            return FactoryCampania.ObtenerCamDetBioHojaCDao().GetDetBioHojaCById(id);
        }

        public bool UpdateDetBioHojaC(DetBioHojaCDTO DetBioHojaCDTO)
        {
            return FactoryCampania.ObtenerCamDetBioHojaCDao().UpdateDetBioHojaC(DetBioHojaCDTO);
        }

        /* E-01 E01*/

        public List<ITCFE01DTO> GetRegITCFE01ProyCodi(int proyCodi)
        {
            return FactoryCampania.ObtenerITCE01().GetRegITCFE01ProyCodi(proyCodi);
        }

        public bool SaveRegITCFE01(ITCFE01DTO itcFE01)
        {
            return FactoryCampania.ObtenerITCE01().SaveRegITCFE01(itcFE01);
        }

        public bool DeleteRegITCFE01ById(int id, string usuario)
        {
            return FactoryCampania.ObtenerITCE01().DeleteRegITCFE01ById(id, usuario);
        }

        public int GetLastRegITCFE01Id()
        {
            return FactoryCampania.ObtenerITCE01().GetLastRegITCFE01Id();
        }

        public ITCFE01DTO GetRegITCFE01ById(int id)
        {
            return FactoryCampania.ObtenerITCE01().GetRegITCFE01ById(id);
        }

        public bool UpdateRegITCFE01(ITCFE01DTO itcFE01)
        {
            return FactoryCampania.ObtenerITCE01().UpdateRegITCFE01(itcFE01);
        }





        public List<Itcdf104DTO> GetItcdf104Codi(int proyCodi)
        {
            return FactoryCampania.ObtenerItcdf104Dao().GetItcdf104Codi(proyCodi);
        }

        public bool SaveItcdf104(Itcdf104DTO itcdf104DTO)
        {
            return FactoryCampania.ObtenerItcdf104Dao().SaveItcdf104(itcdf104DTO);
        }

        public bool DeleteItcdf104ById(int id, string usuario)
        {
            return FactoryCampania.ObtenerItcdf104Dao().DeleteItcdf104ById(id, usuario);
        }

        public int GetLastItcdf104Id()
        {
            return FactoryCampania.ObtenerItcdf104Dao().GetLastItcdf104Id();
        }

        public List<Itcdf104DTO> GetItcdf104ById(int id)
        {
            return FactoryCampania.ObtenerItcdf104Dao().GetItcdf104ById(id);
        }

        public bool UpdateItcdf104(Itcdf104DTO itcdf104DTO)
        {
            return FactoryCampania.ObtenerItcdf104Dao().UpdateItcdf104(itcdf104DTO);
        }

        public List<Itcdf104DTO> GetItcdf104ByFilter(string plancodi, string empresa, string estado)
        {
            return FactoryCampania.ObtenerItcdf104Dao().GetItcdf104ByFilter(plancodi, empresa, estado);
        }

        /**/

        public List<Itcdf108DTO> GetItcdf108Codi(int proyCodi)
        {
            return FactoryCampania.ObtenerItcdf108Dao().GetItcdf108Codi(proyCodi);
        }

        public bool SaveItcdf108(Itcdf108DTO itcdf108DTO)
        {
            return FactoryCampania.ObtenerItcdf108Dao().SaveItcdf108(itcdf108DTO);
        }

        public bool DeleteItcdf108ById(int id, string usuario)
        {
            return FactoryCampania.ObtenerItcdf108Dao().DeleteItcdf108ById(id, usuario);
        }

        public int GetLastItcdf108Id()
        {
            return FactoryCampania.ObtenerItcdf108Dao().GetLastItcdf108Id();
        }

        public List<Itcdf108DTO> GetItcdf108ById(int id)
        {
            return FactoryCampania.ObtenerItcdf108Dao().GetItcdf108ById(id);
        }

        public List<Itcdf108DTO> GetItcdf108ByFilter(string plancodi, string empresa, string estado)
        {
            return FactoryCampania.ObtenerItcdf108Dao().GetItcdf108ByFilter(plancodi, empresa,estado);
        }

        public bool UpdateItcdf108(Itcdf108DTO itcdf108DTO)
        {
            return FactoryCampania.ObtenerItcdf108Dao().UpdateItcdf108(itcdf108DTO);
        }

        /**/

        public List<Itcdf110DTO> GetItcdf110Codi(int proyCodi)
        {
            return FactoryCampania.ObtenerItcdf110Dao().GetItcdf110Codi(proyCodi);
        }

        public bool SaveItcdf110(Itcdf110DTO itcdf110DTO)
        {
            return FactoryCampania.ObtenerItcdf110Dao().SaveItcdf110(itcdf110DTO);
        }

        public bool DeleteItcdf110ById(int id, string usuario)
        {
            return FactoryCampania.ObtenerItcdf110Dao().DeleteItcdf110ById(id, usuario);
        }

        public int GetLastItcdf110Id()
        {
            return FactoryCampania.ObtenerItcdf110Dao().GetLastItcdf110Id();
        }

        public List<Itcdf110DTO> GetItcdf110ById(int id)
        {
            return FactoryCampania.ObtenerItcdf110Dao().GetItcdf110ById(id);
        }

        public List<Itcdf110DTO> GetItcdf110ByFilter(string plancodi, string empresa, string estado)
        {
            return FactoryCampania.ObtenerItcdf110Dao().GetItcdf110ByFilter(plancodi, empresa,estado);
        }

        public bool UpdateItcdf110(Itcdf110DTO itcdf110DTO)
        {
            return FactoryCampania.ObtenerItcdf110Dao().UpdateItcdf110(itcdf110DTO);
        }

        /**/

        public List<Itcdf116DTO> GetItcdf116Codi(int proyCodi)
        {
            return FactoryCampania.ObtenerItcdf116Dao().GetItcdf116Codi(proyCodi);
        }

        public bool SaveItcdf116(Itcdf116DTO itcdf116DTO)
        {
            return FactoryCampania.ObtenerItcdf116Dao().SaveItcdf116(itcdf116DTO);
        }

        public bool DeleteItcdf116ById(int id, string usuario)
        {
            return FactoryCampania.ObtenerItcdf116Dao().DeleteItcdf116ById(id, usuario);
        }

        public int GetLastItcdf116Id()
        {
            return FactoryCampania.ObtenerItcdf116Dao().GetLastItcdf116Id();
        }

        public List<Itcdf116DTO> GetItcdf116ById(int id)
        {
            return FactoryCampania.ObtenerItcdf116Dao().GetItcdf116ById(id);
        }

        public List<Itcdf116DTO> GetItcdf116ByFilter(string plancodi, string empresa, string estado)
        {
            return FactoryCampania.ObtenerItcdf116Dao().GetItcdf116ByFilter(plancodi, empresa, estado);
        }

        public bool UpdateItcdf116(Itcdf116DTO itcdf116DTO)
        {
            return FactoryCampania.ObtenerItcdf116Dao().UpdateItcdf116(itcdf116DTO);
        }

        /**/

        public List<Itcdf121DTO> GetItcdf121Codi(int proyCodi)
        {
            return FactoryCampania.ObtenerItcdf121Dao().GetItcdf121Codi(proyCodi);
        }

        public bool SaveItcdf121(Itcdf121DTO itcdf121DTO)
        {
            return FactoryCampania.ObtenerItcdf121Dao().SaveItcdf121(itcdf121DTO);
        }

        public bool DeleteItcdf121ById(int id, string usuario)
        {
            return FactoryCampania.ObtenerItcdf121Dao().DeleteItcdf121ById(id, usuario);
        }

        public int GetLastItcdf121Id()
        {
            return FactoryCampania.ObtenerItcdf121Dao().GetLastItcdf121Id();
        }

        public List<Itcdf121DTO> GetItcdf121ById(int id)
        {
            return FactoryCampania.ObtenerItcdf121Dao().GetItcdf121ById(id);
        }

        public List<Itcdf121DTO> GetItcdf121ByFilter(string plancodi, string empresa, string estado)
        {
            return FactoryCampania.ObtenerItcdf121Dao().GetItcdf121ByFilter(plancodi, empresa, estado);
        }

        public bool UpdateItcdf121(Itcdf121DTO itcdf121DTO)
        {
            return FactoryCampania.ObtenerItcdf121Dao().UpdateItcdf121(itcdf121DTO);
        }

        /**/

        public List<Itcdf123DTO> GetItcdf123Codi(int proyCodi)
        {
            return FactoryCampania.ObtenerItcdf123Dao().GetItcdf123Codi(proyCodi);
        }

        public bool SaveItcdf123(Itcdf123DTO itcdf123DTO)
        {
            return FactoryCampania.ObtenerItcdf123Dao().SaveItcdf123(itcdf123DTO);
        }

        public bool DeleteItcdf123ById(int id, string usuario)
        {
            return FactoryCampania.ObtenerItcdf123Dao().DeleteItcdf123ById(id, usuario);
        }

        public int GetLastItcdf123Id()
        {
            return FactoryCampania.ObtenerItcdf123Dao().GetLastItcdf123Id();
        }

        public List<Itcdf123DTO> GetItcdf123ById(int id)
        {
            return FactoryCampania.ObtenerItcdf123Dao().GetItcdf123ById(id);
        }

        public List<Itcdf123DTO> GetItcdf123ByFilter(string plancodi, string empresa, string estado)
        {
            return FactoryCampania.ObtenerItcdf123Dao().GetItcdf123ByFilter(plancodi, empresa, estado);
        }


        public bool UpdateItcdf123(Itcdf123DTO itcdf123DTO)
        {
            return FactoryCampania.ObtenerItcdf123Dao().UpdateItcdf123(itcdf123DTO);
        }
        /**/

        public List<Itcdfp011DTO> GetItcdfp011Codi(int proyCodi)
        {
            return FactoryCampania.ObtenerItcdfp011Dao().GetItcdfp011Codi(proyCodi);
        }

        public bool SaveItcdfp011(Itcdfp011DTO itcdfp011DTO)
        {
            return FactoryCampania.ObtenerItcdfp011Dao().SaveItcdfp011(itcdfp011DTO);
        }

        public bool DeleteItcdfp011ById(int id, string usuario)
        {
            return FactoryCampania.ObtenerItcdfp011Dao().DeleteItcdfp011ById(id, usuario);
        }

        public int GetLastItcdfp011Id()
        {
            return FactoryCampania.ObtenerItcdfp011Dao().GetLastItcdfp011Id();
        }

        public Itcdfp011DTO GetItcdfp011ById(int id)
        {
            return FactoryCampania.ObtenerItcdfp011Dao().GetItcdfp011ById(id);
        }

        public bool UpdateItcdfp011(Itcdfp011DTO itcdfp011DTO)
        {
            return FactoryCampania.ObtenerItcdfp011Dao().UpdateItcdfp011(itcdfp011DTO);
        }
        /**/
        public List<Itcdfp012DTO> GetItcdfp012Codi(int proyCodi)
        {
            return FactoryCampania.ObtenerItcdfp012Dao().GetItcdfp012Codi(proyCodi);
        }

        public bool SaveItcdfp012(Itcdfp012DTO Itcdfp012DTO)
        {
            return FactoryCampania.ObtenerItcdfp012Dao().SaveItcdfp012(Itcdfp012DTO);
        }

        public bool DeleteItcdfp012ById(int id, string usuario)
        {
            return FactoryCampania.ObtenerItcdfp012Dao().DeleteItcdfp012ById(id, usuario);
        }

        public int GetLastItcdfp012Id()
        {
            return FactoryCampania.ObtenerItcdfp012Dao().GetLastItcdfp012Id();
        }

        public List<Itcdfp012DTO> GetItcdfp012ById(int id)
        {
            return FactoryCampania.ObtenerItcdfp012Dao().GetItcdfp012ById(id);
        }

        public List<Itcdfp012DTO> GetItcdfp012ByFilter(string plancodi, string empresa, string estado)
        {
            return FactoryCampania.ObtenerItcdfp012Dao().GetItcdfp012ByFilter(plancodi, empresa, estado);
        }

        public bool UpdateItcdfp012(Itcdfp012DTO Itcdfp012DTO)
        {
            return FactoryCampania.ObtenerItcdfp012Dao().UpdateItcdfp012(Itcdfp012DTO);
        }
        /**/

        public List<Itcdfp013DTO> GetItcdfp013Codi(int proyCodi)
        {
            return FactoryCampania.ObtenerItcdfp013Dao().GetItcdfp013Codi(proyCodi);
        }

        public bool SaveItcdfp013(Itcdfp013DTO Itcdfp013DTO)
        {
            return FactoryCampania.ObtenerItcdfp013Dao().SaveItcdfp013(Itcdfp013DTO);
        }

        public bool DeleteItcdfp013ById(int id, string usuario)
        {
            return FactoryCampania.ObtenerItcdfp013Dao().DeleteItcdfp013ById(id, usuario);
        }

        public int GetLastItcdfp013Id()
        {
            return FactoryCampania.ObtenerItcdfp013Dao().GetLastItcdfp013Id();
        }

        public List<Itcdfp013DTO> GetItcdfp013ById(int id)
        {
            return FactoryCampania.ObtenerItcdfp013Dao().GetItcdfp013ById(id);
        }

        public List<Itcdfp013DTO> GetItcdfp013ByFilter(string plancodi, string empresa, string estado)
        {
            return FactoryCampania.ObtenerItcdfp013Dao().GetItcdfp013ByFilter(plancodi, empresa, estado);
        }

        public bool UpdateItcdfp013(Itcdfp013DTO Itcdfp013DTO)
        {
            return FactoryCampania.ObtenerItcdfp013Dao().UpdateItcdfp013(Itcdfp013DTO);
        }

        /**/

        /**/

        public List<Itcdf110DetDTO> GetItcdf110DetCodi(int proycodi)
        {
            return FactoryCampania.ObtenerItcdf110DetDao().GetItcdf110DetCodi(proycodi);
        }

        public bool SaveItcdf110Det(Itcdf110DetDTO itcdf110DetDTO)
        {
            return FactoryCampania.ObtenerItcdf110DetDao().SaveItcdf110Det(itcdf110DetDTO);
        }

        public bool DeleteItcdf110DetById(int id, string usuario)
        {
            return FactoryCampania.ObtenerItcdf110DetDao().DeleteItcdf110DetById(id, usuario);
        }

        public int GetLastItcdf110DetId()
        {
            return FactoryCampania.ObtenerItcdf110DetDao().GetLastItcdf110DetId();
        }

        public Itcdf110DetDTO GetItcdf110DetById(int id)
        {
            return FactoryCampania.ObtenerItcdf110DetDao().GetItcdf110DetById(id);
        }

        public bool UpdateItcdf110Det(Itcdf110DetDTO itcdf110DetDTO)
        {
            return FactoryCampania.ObtenerItcdf110DetDao().UpdateItcdf110Det(itcdf110DetDTO);
        }

        /**/

        public List<Itcdf116DetDTO> GetItcdf116DetCodi(int proycodi)
        {
            return FactoryCampania.ObtenerItcdf116DetDao().GetItcdf116DetCodi(proycodi);
        }

        public bool SaveItcdf116Det(Itcdf116DetDTO itcdf116DetDTO)
        {
            return FactoryCampania.ObtenerItcdf116DetDao().SaveItcdf116Det(itcdf116DetDTO);
        }

        public bool DeleteItcdf116DetById(int id, string usuario)
        {
            return FactoryCampania.ObtenerItcdf116DetDao().DeleteItcdf116DetById(id, usuario);
        }

        public int GetLastItcdf116DetId()
        {
            return FactoryCampania.ObtenerItcdf116DetDao().GetLastItcdf116DetId();
        }

        public Itcdf116DetDTO GetItcdf116DetById(int id)
        {
            return FactoryCampania.ObtenerItcdf116DetDao().GetItcdf116DetById(id);
        }

        public bool UpdateItcdf116Det(Itcdf116DetDTO itcdf116DetDTO)
        {
            return FactoryCampania.ObtenerItcdf116DetDao().UpdateItcdf116Det(itcdf116DetDTO);
        }

        /**/

        public List<Itcdf121DetDTO> GetItcdf121DetCodi(int proycodi)
        {
            return FactoryCampania.ObtenerItcdf121DetDao().GetItcdf121DetCodi(proycodi);
        }

        public bool SaveItcdf121Det(Itcdf121DetDTO itcdf121DetDTO)
        {
            return FactoryCampania.ObtenerItcdf121DetDao().SaveItcdf121Det(itcdf121DetDTO);
        }

        public bool DeleteItcdf121DetById(int id, string usuario)
        {
            return FactoryCampania.ObtenerItcdf121DetDao().DeleteItcdf121DetById(id, usuario);
        }

        public int GetLastItcdf121DetId()
        {
            return FactoryCampania.ObtenerItcdf121DetDao().GetLastItcdf121DetId();
        }

        public Itcdf121DetDTO GetItcdf121DetById(int id)
        {
            return FactoryCampania.ObtenerItcdf121DetDao().GetItcdf121DetById(id);
        }

        public bool UpdateItcdf121Det(Itcdf121DetDTO itcdf121DetDTO)
        {
            return FactoryCampania.ObtenerItcdf121DetDao().UpdateItcdf121Det(itcdf121DetDTO);
        }

        /**/

        public List<Itcdfp011DetDTO> GetItcdfp011DetCodi(int proycodi)
        {
            return FactoryCampania.ObtenerItcdfp011DetDao().GetItcdfp011DetCodi(proycodi);
        }

        public bool SaveItcdfp011Det(List<Itcdfp011DetDTO> itcdfp011DetDTOs, int itcdfp011Codi, string usuCreacion)
        {
            return FactoryCampania.ObtenerItcdfp011DetDao().SaveItcdfp011Det(itcdfp011DetDTOs, itcdfp011Codi, usuCreacion);
        }

        public List<Itcdfp011DTO> GetItcdfp011ByFilter(string plancodi, string empresa, string estado)
        {
            return FactoryCampania.ObtenerItcdfp011Dao().GetItcdfp011ByFilter(plancodi, empresa, estado);
        }


        public bool DeleteItcdfp011DetById(int id, string usuario)
        {
            return FactoryCampania.ObtenerItcdfp011DetDao().DeleteItcdfp011DetById(id, usuario);
        }

        public int GetLastItcdfp011DetId()
        {
            return FactoryCampania.ObtenerItcdfp011DetDao().GetLastItcdfp011DetId();
        }

        public List<Itcdfp011DetDTO> GetItcdfp011DetById(int id)
        {
            return FactoryCampania.ObtenerItcdfp011DetDao().GetItcdfp011DetById(id);
        }

        public List<Itcdfp011DetDTO> GetItcdfp011DetByIdPag(int id, int offset, int pageSize)
        {
            return FactoryCampania.ObtenerItcdfp011DetDao().GetItcdfp011DetByIdPag(id, offset, pageSize);
        }

        public bool GetCloneItcdfp011DetById(int id, int newId)
        {
            return FactoryCampania.ObtenerItcdfp011DetDao().GetCloneItcdfp011DetById(id, newId);
        }

        public bool UpdateItcdfp011Det(Itcdfp011DetDTO itcdfp011DetDTO)
        {
            return FactoryCampania.ObtenerItcdfp011DetDao().UpdateItcdfp011Det(itcdfp011DetDTO);
        }

        /**/

        public List<Itcdfp013DetDTO> GetItcdfp013DetCodi(int proycodi)
        {
            return FactoryCampania.ObtenerItcdfp013DetDao().GetItcdfp013DetCodi(proycodi);
        }

        public bool SaveItcdfp013Det(Itcdfp013DetDTO itcdfp013DetDTO)
        {
            return FactoryCampania.ObtenerItcdfp013DetDao().SaveItcdfp013Det(itcdfp013DetDTO);
        }

        public bool DeleteItcdfp013DetById(int id, string usuario)
        {
            return FactoryCampania.ObtenerItcdfp013DetDao().DeleteItcdfp013DetById(id, usuario);
        }

        public int GetLastItcdfp013DetId()
        {
            return FactoryCampania.ObtenerItcdfp013DetDao().GetLastItcdfp013DetId();
        }

        public Itcdfp013DetDTO GetItcdfp013DetById(int id)
        {
            return FactoryCampania.ObtenerItcdfp013DetDao().GetItcdfp013DetById(id);
        }

        public bool UpdateItcdfp013Det(Itcdfp013DetDTO itcdfp013DetDTO)
        {
            return FactoryCampania.ObtenerItcdfp013DetDao().UpdateItcdfp013Det(itcdfp013DetDTO);
        }

        
        // Para CAM_ITCPRM1
        public List<ItcPrm1Dto> GetItcprm1Codi(int proyCodi)
        {
            return FactoryCampania.ObtenerItcprm1Dao().GetItcPrm1ByProyCodi(proyCodi);
        }

        public bool SaveItcprm1(ItcPrm1Dto itcprm1DTO)
        {
            return FactoryCampania.ObtenerItcprm1Dao().SaveItcPrm1(itcprm1DTO);
        }

        public bool DeleteItcprm1ById(int id, string usuario)
        {
            return FactoryCampania.ObtenerItcprm1Dao().DeleteItcPrm1ById(id, usuario);
        }

        public int GetLastItcprm1Id()
        {
            return FactoryCampania.ObtenerItcprm1Dao().GetLastItcPrm1Id();
        }

        public List<ItcPrm1Dto> GetItcprm1ById(int id)
        {
            return FactoryCampania.ObtenerItcprm1Dao().GetItcPrm1ById(id);
        }

        public List<ItcPrm1Dto> GetItcprm1ByFilter(string plancodi, string empresa, string estado)
        {
            return FactoryCampania.ObtenerItcprm1Dao().GetItcPrm1ByFilter(plancodi, empresa, estado);
        }


        public bool UpdateItcprm1(ItcPrm1Dto itcprm1DTO)
        {
            return FactoryCampania.ObtenerItcprm1Dao().UpdateItcPrm1(itcprm1DTO);
        }

        // Para CAM_ITCPRM2
        public List<ItcPrm2Dto> GetItcprm2Codi(int proyCodi)
        {
            return FactoryCampania.ObtenerItcprm2Dao().GetItcPrm2Codi(proyCodi);
        }

        public bool SaveItcprm2(ItcPrm2Dto itcprm2DTO)
        {
            return FactoryCampania.ObtenerItcprm2Dao().SaveItcPrm2(itcprm2DTO);
        }

        public bool DeleteItcprm2ById(int id, string usuario)
        {
            return FactoryCampania.ObtenerItcprm2Dao().DeleteItcPrm2ById(id, usuario);
        }

        public int GetLastItcprm2Id()
        {
            return FactoryCampania.ObtenerItcprm2Dao().GetLastItcPrm2Id();
        }

        public List<ItcPrm2Dto> GetItcprm2ById(int id)
        {
            return FactoryCampania.ObtenerItcprm2Dao().GetItcPrm2ById(id);
        }

        public List<ItcPrm2Dto> GetItcprm2ByFilter(string plancodi, string empresa, string estado)
        {
            return FactoryCampania.ObtenerItcprm2Dao().GetItcPrm2ByFilter(plancodi, empresa, estado);
        }


        public bool UpdateItcprm2(ItcPrm2Dto itcprm2DTO)
        {
            return FactoryCampania.ObtenerItcprm2Dao().UpdateItcPrm2(itcprm2DTO);
        }

        // Para CAM_ITCRED1
        public List<ItcRed1Dto> GetItcred1Codi(int proyCodi)
        {
            return FactoryCampania.ObtenerItcred1Dao().GetItcRed1ByProyCodi(proyCodi);
        }

        public bool SaveItcred1(ItcRed1Dto itcred1DTO)
        {
            return FactoryCampania.ObtenerItcred1Dao().SaveItcRed1(itcred1DTO);
        }

        public bool DeleteItcred1ById(int id, string usuario)
        {
            return FactoryCampania.ObtenerItcred1Dao().DeleteItcRed1ById(id, usuario);
        }

        public int GetLastItcred1Id()
        {
            return FactoryCampania.ObtenerItcred1Dao().GetLastItcRed1Id();
        }

        public List<ItcRed1Dto> GetItcred1ById(int id)
        {
            return FactoryCampania.ObtenerItcred1Dao().GetItcRed1ById(id);
        }

        public List<ItcRed1Dto> GetItcred1ByFilter(string plancodi, string empresa, string estado)
        {
            return FactoryCampania.ObtenerItcred1Dao().GetItcRed1ByFilter(plancodi, empresa, estado);
        }


        public bool UpdateItcred1(ItcRed1Dto itcred1DTO)
        {
            return FactoryCampania.ObtenerItcred1Dao().UpdateItcRed1(itcred1DTO);
        }

        // Para CAM_ITCRED2
        public List<ItcRed2Dto> GetItcred2Codi(int proyCodi)
        {
            return FactoryCampania.ObtenerItcred2Dao().GetCamItcRed2ByProyCodi(proyCodi);
        }

        public bool SaveItcred2(ItcRed2Dto itcred2DTO)
        {
            return FactoryCampania.ObtenerItcred2Dao().SaveCamItcRed2(itcred2DTO);
        }

        public bool DeleteItcred2ById(int id, string usuario)
        {
            return FactoryCampania.ObtenerItcred2Dao().DeleteCamItcRed2ById(id, usuario);
        }

        public int GetLastItcred2Id()
        {
            return FactoryCampania.ObtenerItcred2Dao().GetLastCamItcRed2Id();
        }

        public List<ItcRed2Dto> GetItcred2ById(int id)
        {
            return FactoryCampania.ObtenerItcred2Dao().GetCamItcRed2ById(id);
        }

        public List<ItcRed2Dto> GetItcred2ByFilter(string plancodi, string empresa, string estado)
        {
            return FactoryCampania.ObtenerItcred2Dao().GetCamItcRed2ByFilter(plancodi, empresa, estado);
        }

        public bool UpdateItcred2(ItcRed2Dto itcred2DTO)
        {
            return FactoryCampania.ObtenerItcred2Dao().UpdateCamItcRed2(itcred2DTO);
        }

        // Para CAM_ITCRED3
        public List<ItcRed3Dto> GetItcred3Codi(int proyCodi)
        {
            return FactoryCampania.ObtenerItcred3Dao().GetItcRed3ByProyCodi(proyCodi);
        }

        public bool SaveItcred3(ItcRed3Dto itcred3DTO)
        {
            return FactoryCampania.ObtenerItcred3Dao().SaveItcRed3(itcred3DTO);
        }

        public bool DeleteItcred3ById(int id, string usuario)
        {
            return FactoryCampania.ObtenerItcred3Dao().DeleteItcRed3ById(id, usuario);
        }

        public int GetLastItcred3Id()
        {
            return FactoryCampania.ObtenerItcred3Dao().GetLastItcRed3Id();
        }

        public List<ItcRed3Dto> GetItcred3ById(int id)
        {
            return FactoryCampania.ObtenerItcred3Dao().GetItcRed3ById(id);
        }

        public List<ItcRed3Dto> GetItcred3ByFilter(string plancodi, string empresa, string estado)
        {
            return FactoryCampania.ObtenerItcred3Dao().GetItcRed3ByFilter(plancodi, empresa, estado);
        }


        public bool UpdateItcred3(ItcRed3Dto itcred3DTO)
        {
            return FactoryCampania.ObtenerItcred3Dao().UpdateItcRed3(itcred3DTO);
        }

        // Para CAM_ITCRED4
        public List<ItcRed4Dto> GetItcred4Codi(int proyCodi)
        {
            return FactoryCampania.ObtenerItcred4Dao().GetItcRed4ByProyCodi(proyCodi);
        }

        public bool SaveItcred4(ItcRed4Dto itcred4DTO)
        {
            return FactoryCampania.ObtenerItcred4Dao().SaveItcRed4(itcred4DTO);
        }

        public bool DeleteItcred4ById(int id, string usuario)
        {
            return FactoryCampania.ObtenerItcred4Dao().DeleteItcRed4ById(id, usuario);
        }

        public int GetLastItcred4Id()
        {
            return FactoryCampania.ObtenerItcred4Dao().GetLastItcRed4Id();
        }

        public List<ItcRed4Dto> GetItcred4ById(int id)
        {
            return FactoryCampania.ObtenerItcred4Dao().GetItcRed4ById(id);
        }

        public List<ItcRed4Dto> GetItcred4ByFilter(string plancodi, string empresa, string estado)
        {
            return FactoryCampania.ObtenerItcred4Dao().GetItcRed4ByFilter(plancodi, empresa, estado);
        }

        public bool UpdateItcred4(ItcRed4Dto itcred4DTO)
        {
            return FactoryCampania.ObtenerItcred4Dao().UpdateItcRed4(itcred4DTO);
        }

        // Para CAM_ITCRED5
        public List<ItcRed5Dto> GetItcred5Codi(int proyCodi)
        {
            return FactoryCampania.ObtenerItcred5Dao().GetItcRed5ByProyCodi(proyCodi);
        }

        public bool SaveItcred5(ItcRed5Dto itcred5DTO)
        {
            return FactoryCampania.ObtenerItcred5Dao().SaveItcRed5(itcred5DTO);
        }

        public bool DeleteItcred5ById(int id, string usuario)
        {
            return FactoryCampania.ObtenerItcred5Dao().DeleteItcRed5ById(id, usuario);
        }

        public int GetLastItcred5Id()
        {
            return FactoryCampania.ObtenerItcred5Dao().GetLastItcRed5Id();
        }

        public List<ItcRed5Dto> GetItcred5ById(int id)
        {
            return FactoryCampania.ObtenerItcred5Dao().GetItcRed5ById(id);
        }

        public List<ItcRed5Dto> GetItcred5ByFilter(string plancodi, string empresa, string estado)
        {
            return FactoryCampania.ObtenerItcred5Dao().GetItcRed5ByFilter(plancodi, empresa, estado);
        }


        public bool UpdateItcred5(ItcRed5Dto itcred5DTO)
        {
            return FactoryCampania.ObtenerItcred5Dao().UpdateItcRed5(itcred5DTO);
        }
        /**/
        public List<CCGDADTO> GetCcgdaByProyCodi(int proyCodi)
        {
            return FactoryCampania.ObtenerCcgdaDao().GetCamCCGDA(proyCodi);
        }

        public bool SaveCcgda(CCGDADTO ccgdaDTO)
        {
            return FactoryCampania.ObtenerCcgdaDao().SaveCamCCGDA(ccgdaDTO);
        }

        public bool DeleteCcgdaById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCcgdaDao().DeleteCamCCGDAById(id, usuario);
        }

        public int GetLastCcgdaId()
        {
            return FactoryCampania.ObtenerCcgdaDao().GetLastCamCCGDAId();
        }

        public CCGDADTO GetCcgdaById(int id)
        {
            return FactoryCampania.ObtenerCcgdaDao().GetCamCCGDAById(id);
        }

        public List<CCGDADTO> GetCamCCGDAByFilter(string plancodi, string empresa, string estado)
        {
            return FactoryCampania.ObtenerCcgdaDao().GetCamCCGDAByFilter(plancodi, empresa, estado);
        }


        public bool UpdateCcgda(CCGDADTO ccgdaDTO)
        {
            return FactoryCampania.ObtenerCcgdaDao().UpdateCamCCGDA(ccgdaDTO);
        }

        /**/
        public List<CCGDBDTO> GetCcgdbByProyCodi(int proyCodi)
        {
            return FactoryCampania.ObtenerCcgdbDao().GetCamCCGDB(proyCodi);
        }

        public bool SaveCcgdb(CCGDBDTO ccgdbDTO)
        {
            return FactoryCampania.ObtenerCcgdbDao().SaveCamCCGDB(ccgdbDTO);
        }

        public bool DeleteCcgdbById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCcgdbDao().DeleteCamCCGDBById(id, usuario);
        }

        public int GetLastCcgdbId()
        {
            return FactoryCampania.ObtenerCcgdbDao().GetLastCamCCGDBCodi();
        }

        public List<CCGDBDTO> GetCcgdbById(int id)
        {
            return FactoryCampania.ObtenerCcgdbDao().GetCamCCGDBById(id);
        }

        public List<CCGDBDTO> GetCcgdbByFilter(string plancodi, string empresa, string estado)
        {
            return FactoryCampania.ObtenerCcgdbDao().GetCamCCGDBByFilter(plancodi, empresa, estado);
        }


        public bool UpdateCcgdb(CCGDBDTO ccgdbDTO)
        {
            return FactoryCampania.ObtenerCcgdbDao().UpdateCamCCGDB(ccgdbDTO);
        }

        /**/
        public List<CCGDCOptDTO> GetCcgdcOptByProyCodi(int proyCodi)
        {
            return FactoryCampania.ObtenerCcgdcOptDao().GetCamCCGDC(proyCodi);
        }

        public bool SaveCcgdcOpt(CCGDCOptDTO ccgdcDTO)
        {
            return FactoryCampania.ObtenerCcgdcOptDao().SaveCamCCGDC(ccgdcDTO);
        }

        public bool DeleteCcgdcOptById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCcgdcOptDao().DeleteCamCCGDCById(id, usuario);
        }

        public int GetLastCcgdcOptId()
        {
            return FactoryCampania.ObtenerCcgdcOptDao().GetLastCamCCGDCCodi();
        }

        public List<CCGDCOptDTO> GetCcgdcOptById(int id)
        {
            return FactoryCampania.ObtenerCcgdcOptDao().GetCamCCGDCById(id);
        }

        public List<CCGDCOptDTO> GetCamCCGDCOptByFilter(string plancodi, string empresa, string estado)
        {
            return FactoryCampania.ObtenerCcgdcOptDao().GetCamCCGDCByFilter(plancodi, empresa, estado);
        }

        public bool UpdateCcgdcOpt(CCGDCOptDTO ccgdcDTO)
        {
            return FactoryCampania.ObtenerCcgdcOptDao().UpdateCamCCGDC(ccgdcDTO);
        }

        public List<CCGDCDTO> GetCamCCGDCByFilter(string plancodi, string empresa, string estado)
        {
            return FactoryCampania.ObtenerCcgdbDao().GetCamCCGDCByFilter(plancodi, empresa, estado);
        }

        /**/

        /**/
        public List<CCGDCPesDTO> GetCcgdcPesByProyCodi(int proyCodi)
        {
            return FactoryCampania.ObtenerCcgdcPesDao().GetCamCCGDC(proyCodi);
        }

        public bool SaveCcgdcPes(CCGDCPesDTO ccgdcDTO)
        {
            return FactoryCampania.ObtenerCcgdcPesDao().SaveCamCCGDC(ccgdcDTO);
        }

        public bool DeleteCcgdcPesById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCcgdcPesDao().DeleteCamCCGDCById(id, usuario);
        }

        public int GetLastCcgdcPesId()
        {
            return FactoryCampania.ObtenerCcgdcPesDao().GetLastCamCCGDCCodi();
        }

        public List<CCGDCPesDTO> GetCcgdcPesById(int id)
        {
            return FactoryCampania.ObtenerCcgdcPesDao().GetCamCCGDCById(id);
        }

        public List<CCGDCPesDTO> GetCamCCGDCPesByFilter(string plancodi, string empresa, string estado)
        {
            return FactoryCampania.ObtenerCcgdcPesDao().GetCamCCGDCByFilter(plancodi, empresa, estado);
        }

        public bool UpdateCcgdcPes(CCGDCPesDTO ccgdcDTO)
        {
            return FactoryCampania.ObtenerCcgdcPesDao().UpdateCamCCGDC(ccgdcDTO);
        }
        /**/


        public List<CCGDDDTO> GetCcgddByProyCodi(int proyCodi)
        {
            return FactoryCampania.ObtenerCcgddDao().GetCamCCGDD(proyCodi);
        }

        public bool SaveCcgdd(CCGDDDTO ccgddDTO)
        {
            return FactoryCampania.ObtenerCcgddDao().SaveCamCCGDD(ccgddDTO);
        }

        public bool DeleteCcgddById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCcgddDao().DeleteCamCCGDDById(id, usuario);
        }

        public int GetLastCcgddId()
        {
            return FactoryCampania.ObtenerCcgddDao().GetLastCamCCGDDCodi();
        }

        public List<CCGDDDTO> GetCcgddById(int id)
        {
            return FactoryCampania.ObtenerCcgddDao().GetCamCCGDDById(id);
        }

        public List<CCGDDDTO> GetCamCCGDDByFilter(string plancodi, string empresa, string estado)
        {
            return FactoryCampania.ObtenerCcgddDao().GetCamCCGDDByFilter(plancodi, empresa, estado);
        }

        public bool UpdateCcgdd(CCGDDDTO ccgddDTO)
        {
            return FactoryCampania.ObtenerCcgddDao().UpdateCamCCGDD(ccgddDTO);
        }
        /**/
        public List<CCGDEDTO> GetCcgdeByProyCodi(int proyCodi)
        {
            return FactoryCampania.ObtenerCcgdeDao().GetCamCCGDE(proyCodi);
        }

        public bool SaveCcgde(CCGDEDTO ccgdeDTO)
        {
            return FactoryCampania.ObtenerCcgdeDao().SaveCamCCGDE(ccgdeDTO);
        }

        public bool DeleteCcgdeById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCcgdeDao().DeleteCamCCGDEById(id, usuario);
        }

        public int GetLastCcgdeId()
        {
            return FactoryCampania.ObtenerCcgdeDao().GetLastCamCCGDECodi();
        }

        public CCGDEDTO GetCcgdeById(int id)
        {
            return FactoryCampania.ObtenerCcgdeDao().GetCamCCGDEById(id);
        }

        public List<CCGDEDTO> GetCamCCGDEByFilter(string plancodi, string empresa, string estado)
        {
            return FactoryCampania.ObtenerCcgdeDao().GetCamCCGDEByFilter(plancodi, empresa, estado);
        }

        public bool UpdateCcgde(CCGDEDTO ccgdeDTO)
        {
            return FactoryCampania.ObtenerCcgdeDao().UpdateCamCCGDE(ccgdeDTO);
        }
        /**/
        public List<CCGDFDTO> GetCcgdfByProyCodi(int proyCodi)
        {
            return FactoryCampania.ObtenerCcgdfDao().GetCamCCGDF(proyCodi);
        }

        public bool SaveCcgdf(CCGDFDTO ccgdfDTO)
        {
            return FactoryCampania.ObtenerCcgdfDao().SaveCamCCGDF(ccgdfDTO);
        }

        public bool DeleteCcgdfById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCcgdfDao().DeleteCamCCGDFById(id, usuario);
        }

        public int GetLastCcgdfId()
        {
            return FactoryCampania.ObtenerCcgdfDao().GetLastCamCCGDFCodi();
        }

        public List<CCGDFDTO> GetCcgdfById(int id)
        {
            return FactoryCampania.ObtenerCcgdfDao().GetCamCCGDFById(id);
        }

        public List<CCGDFDTO> GetCcgdfByFilter(string plancodi, string empresa, string estado)
        {
            return FactoryCampania.ObtenerCcgdfDao().GetCamCCGDFByFilter(plancodi,empresa, estado);
        }

        public bool UpdateCcgdf(CCGDFDTO ccgdfDTO)
        {
            return FactoryCampania.ObtenerCcgdfDao().UpdateCamCCGDF(ccgdfDTO);
        }

        /**/
        public List<FormatoD1ADTO> GetFormatoD1AByProyCodi(int proyCodi)
        {
            return FactoryCampania.ObtenerFormatoD1ADao().GetFormatoD1ACodi(proyCodi);
        }

        public bool SaveFormatoD1A(FormatoD1ADTO formatoD1ADTO)
        {
            return FactoryCampania.ObtenerFormatoD1ADao().SaveFormatoD1A(formatoD1ADTO);
        }

        public bool DeleteFormatoD1AById(int id, string usuario)
        {
            return FactoryCampania.ObtenerFormatoD1ADao().DeleteFormatoD1AById(id, usuario);
        }

        public int GetLastFormatoD1AId()
        {
            return FactoryCampania.ObtenerFormatoD1ADao().GetLastFormatoD1AId();
        }

        public FormatoD1ADTO GetFormatoD1AById(int id)
        {
            return FactoryCampania.ObtenerFormatoD1ADao().GetFormatoD1AById(id);
        }

        public List<FormatoD1ADTO> GetFormatoD1AByFilter(string plancodi, string empresa, string estado)
        {
            return FactoryCampania.ObtenerFormatoD1ADao().GetFormatoD1AByFilter(plancodi, empresa, estado);
        }

        /**/
        public List<FormatoD1ADet1DTO> GetFormatoD1ADET1ByProyCodi(int proyCodi)
        {
            return FactoryCampania.ObtenerFormatoD1ADet1Dao().GetFormatoD1ADET1Codi(proyCodi);
        }

        public bool SaveFormatoD1ADET1(FormatoD1ADet1DTO formatoD1ADET1DTO)
        {
            return FactoryCampania.ObtenerFormatoD1ADet1Dao().SaveFormatoD1ADET1(formatoD1ADET1DTO);
        }

        public bool DeleteFormatoD1ADET1ById(int id, string usuario)
        {
            return FactoryCampania.ObtenerFormatoD1ADet1Dao().DeleteFormatoD1ADET1ById(id, usuario);
        }

        public int GetLastFormatoD1ADET1Id()
        {
            return FactoryCampania.ObtenerFormatoD1ADet1Dao().GetLastFormatoD1ADET1Id();
        }

        public FormatoD1ADet1DTO GetFormatoD1ADET1ById(int id)
        {
            return FactoryCampania.ObtenerFormatoD1ADet1Dao().GetFormatoD1ADET1ById(id);
        }
        /**/
        public List<FormatoD1ADet2DTO> GetFormatoD1ADET2ByProyCodi(int proyCodi)
        {
            return FactoryCampania.ObtenerFormatoD1ADet2Dao().GetFormatoD1ADET2Codi(proyCodi);
        }

        public bool SaveFormatoD1ADET2(FormatoD1ADet2DTO formatoD1ADET2DTO)
        {
            return FactoryCampania.ObtenerFormatoD1ADet2Dao().SaveFormatoD1ADET2(formatoD1ADET2DTO);
        }

        public bool DeleteFormatoD1ADET2ById(int id, string usuario)
        {
            return FactoryCampania.ObtenerFormatoD1ADet2Dao().DeleteFormatoD1ADET2ById(id, usuario);
        }

        public int GetLastFormatoD1ADET2Id()
        {
            return FactoryCampania.ObtenerFormatoD1ADet2Dao().GetLastFormatoD1ADET2Id();
        }

        public FormatoD1ADet2DTO GetFormatoD1ADET2ById(int id)
        {
            return FactoryCampania.ObtenerFormatoD1ADet2Dao().GetFormatoD1ADET2ById(id);
        }

        /**/
        public List<FormatoD1ADet3DTO> GetFormatoD1ADET3ByProyCodi(int proyCodi)
        {
            return FactoryCampania.ObtenerFormatoD1ADet3Dao().GetFormatoD1ADET3Codi(proyCodi);
        }

        public bool SaveFormatoD1ADET3(FormatoD1ADet3DTO formatoD1ADET3DTO)
        {
            return FactoryCampania.ObtenerFormatoD1ADet3Dao().SaveFormatoD1ADET3(formatoD1ADET3DTO);
        }

        public bool DeleteFormatoD1ADET3ById(int id, string usuario)
        {
            return FactoryCampania.ObtenerFormatoD1ADet3Dao().DeleteFormatoD1ADET3ById(id, usuario);
        }

        public int GetLastFormatoD1ADET3Id()
        {
            return FactoryCampania.ObtenerFormatoD1ADet3Dao().GetLastFormatoD1ADET3Id();
        }

        public FormatoD1ADet3DTO GetFormatoD1ADET3ById(int id)
        {
            return FactoryCampania.ObtenerFormatoD1ADet3Dao().GetFormatoD1ADET3ById(id);
        }
        /**/
        public List<FormatoD1ADet4DTO> GetFormatoD1ADET4ByProyCodi(int formatoD1ACodi)
        {
            return FactoryCampania.ObtenerFormatoD1ADet4Dao().GetFormatoD1ADET4Codi(formatoD1ACodi);
        }

        public bool SaveFormatoD1ADET4(FormatoD1ADet4DTO formatoD1ADET4DTO)
        {
            return FactoryCampania.ObtenerFormatoD1ADet4Dao().SaveFormatoD1ADET4(formatoD1ADET4DTO);
        }

        public bool DeleteFormatoD1ADET4ById(int id, string usuario)
        {
            return FactoryCampania.ObtenerFormatoD1ADet4Dao().DeleteFormatoD1ADET4ById(id, usuario);
        }

        public int GetLastFormatoD1ADET4Id()
        {
            return FactoryCampania.ObtenerFormatoD1ADet4Dao().GetLastFormatoD1ADET4Id();
        }

        public FormatoD1ADet4DTO GetFormatoD1ADET4ById(int id)
        {
            return FactoryCampania.ObtenerFormatoD1ADet4Dao().GetFormatoD1ADET4ById(id);
        }
        /**/
        public List<FormatoD1ADet5DTO> GetFormatoD1ADET5ByProyCodi(int formatoD1ACodi)
        {
            return FactoryCampania.ObtenerFormatoD1ADet5Dao().GetFormatoD1ADET5Codi(formatoD1ACodi);
        }

        public bool SaveFormatoD1ADET5(FormatoD1ADet5DTO formatoD1ADET5DTO)
        {
            return FactoryCampania.ObtenerFormatoD1ADet5Dao().SaveFormatoD1ADET5(formatoD1ADET5DTO);
        }

        public bool DeleteFormatoD1ADET5ById(int id, string usuario)
        {
            return FactoryCampania.ObtenerFormatoD1ADet5Dao().DeleteFormatoD1ADET5ById(id, usuario);
        }

        public int GetLastFormatoD1ADET5Id()
        {
            return FactoryCampania.ObtenerFormatoD1ADet5Dao().GetLastFormatoD1ADET5Id();
        }

        public FormatoD1ADet5DTO GetFormatoD1ADET5ById(int id)
        {
            return FactoryCampania.ObtenerFormatoD1ADet5Dao().GetFormatoD1ADET5ById(id);
        }
        /**/
        public List<FormatoD1BDTO> GetFormatoD1BByProyCodi(int proyCodi)
        {
            return FactoryCampania.ObtenerFormatoD1BDao().GetFormatoD1BCodi(proyCodi);
        }

        public bool SaveFormatoD1B(FormatoD1BDTO formatoD1BDTO)
        {
            return FactoryCampania.ObtenerFormatoD1BDao().SaveFormatoD1B(formatoD1BDTO);
        }

        public bool DeleteFormatoD1BById(int id, string usuario)
        {
            return FactoryCampania.ObtenerFormatoD1BDao().DeleteFormatoD1BById(id, usuario);
        }

        public int GetLastFormatoD1BId()
        {
            return FactoryCampania.ObtenerFormatoD1BDao().GetLastFormatoD1BId();
        }

        public FormatoD1BDTO GetFormatoD1BById(int id)
        {
            return FactoryCampania.ObtenerFormatoD1BDao().GetFormatoD1BById(id);
        }


        public List<FormatoD1BDTO> GetFormatoD1BByFilter(string plancodi, string empresa, string estado)
        {
            return FactoryCampania.ObtenerFormatoD1BDao().GetFormatoD1BByFilter(plancodi, empresa, estado);
        }

        public List<FormatoD1BDetDTO> GetFormatoD1BDetByCodi(int formatoD1BCodi)
        {
            return FactoryCampania.ObtenerFormatoD1BDetDao().GetFormatoD1BDETCodi(formatoD1BCodi);
        }

        public List<FormatoD1CDTO> GetFormatoD1CByFilter(string plancodi, string empresa, string estado)
        {
            return FactoryCampania.ObtenerFormatoD1CDao().GetFormatoD1CByFilter(plancodi, empresa, estado);
        }


        public List<FormatoD1DDTO> GetFormatoD1DByFilter(string plancodi, string empresa, string estado)
        {
            return FactoryCampania.ObtenerFormatoD1DDao().GetFormatoD1DByFilter(plancodi, empresa, estado);
        }

        public bool SaveFormatoD1BDet(FormatoD1BDetDTO formatoD1BDetDTO)
        {
            return FactoryCampania.ObtenerFormatoD1BDetDao().SaveFormatoD1BDET(formatoD1BDetDTO);
        }

        public bool DeleteFormatoD1BDetById(int id, string usuario)
        {
            return FactoryCampania.ObtenerFormatoD1BDetDao().DeleteFormatoD1BDETById(id, usuario);
        }

        public int GetLastFormatoD1BDetId()
        {
            return FactoryCampania.ObtenerFormatoD1BDetDao().GetLastFormatoD1BDETId();
        }

        public FormatoD1BDetDTO GetFormatoD1BDetById(int id)
        {
            return FactoryCampania.ObtenerFormatoD1BDetDao().GetFormatoD1BDETById(id);
        }
        /**/
        public List<FormatoD1CDTO> GetFormatoD1CByProyCodi(int proyCodi)
        {
            return FactoryCampania.ObtenerFormatoD1CDao().GetFormatoD1CCodi(proyCodi);
        }

        public bool SaveFormatoD1C(FormatoD1CDTO formatoD1CDTO)
        {
            return FactoryCampania.ObtenerFormatoD1CDao().SaveFormatoD1C(formatoD1CDTO);
        }

        public bool DeleteFormatoD1CById(int id, string usuario)
        {
            return FactoryCampania.ObtenerFormatoD1CDao().DeleteFormatoD1CById(id, usuario);
        }

        public int GetLastFormatoD1CId()
        {
            return FactoryCampania.ObtenerFormatoD1CDao().GetLastFormatoD1CId();
        }

        public FormatoD1CDTO GetFormatoD1CById(int id)
        {
            return FactoryCampania.ObtenerFormatoD1CDao().GetFormatoD1CById(id);
        }
        /**/
        public List<FormatoD1CDetDTO> GetFormatoD1CDetCCodi(int formatoD1CCodi)
        {
            return FactoryCampania.ObtenerFormatoD1CDetDao().GetFormatoD1CDETCodi(formatoD1CCodi);
        }

        public bool SaveFormatoD1CDet(FormatoD1CDetDTO formatoD1CDetDTO)
        {
            return FactoryCampania.ObtenerFormatoD1CDetDao().SaveFormatoD1CDET(formatoD1CDetDTO);
        }

        public bool DeleteFormatoD1CDetById(int id, string usuario)
        {
            return FactoryCampania.ObtenerFormatoD1CDetDao().DeleteFormatoD1CDETById(id, usuario);
        }

        public int GetLastFormatoD1CDetId()
        {
            return FactoryCampania.ObtenerFormatoD1CDetDao().GetLastFormatoD1CDETId();
        }

        public FormatoD1CDetDTO GetFormatoD1CDetById(int id)
        {
            return FactoryCampania.ObtenerFormatoD1CDetDao().GetFormatoD1CDETById(id);
        }
        /**/
        public List<FormatoD1DDTO> GetFormatoD1DByProyCodi(int proyCodi)
        {
            return FactoryCampania.ObtenerFormatoD1DDao().GetFormatoD1DCodi(proyCodi);
        }

        public bool SaveFormatoD1D(FormatoD1DDTO formatoD1DDTO)
        {
            return FactoryCampania.ObtenerFormatoD1DDao().SaveFormatoD1D(formatoD1DDTO);
        }

        public bool DeleteFormatoD1DById(int id, string usuario)
        {
            return FactoryCampania.ObtenerFormatoD1DDao().DeleteFormatoD1DById(id, usuario);
        }

        public int GetLastFormatoD1DId()
        {
            return FactoryCampania.ObtenerFormatoD1DDao().GetLastFormatoD1DId();
        }

        public List<FormatoD1DDTO> GetFormatoD1DById(int id)
        {
            return FactoryCampania.ObtenerFormatoD1DDao().GetFormatoD1DById(id);
        }

        /**/
        public List<CuestionarioH2VADTO> GetCuestionarioH2VAByProyCodi(int proyCodi)
        {
            return FactoryCampania.ObtenerCuestionarioH2VADao().GetCuestionarioH2VACodi(proyCodi);
        }

        public bool SaveCuestionarioH2VA(CuestionarioH2VADTO cuestionarioH2VADTO)
        {
            return FactoryCampania.ObtenerCuestionarioH2VADao().SaveCuestionarioH2VA(cuestionarioH2VADTO);
        }

        public bool DeleteCuestionarioH2VAById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCuestionarioH2VADao().DeleteCuestionarioH2VAById(id, usuario);
        }

        public int GetLastCuestionarioH2VAId()
        {
            return FactoryCampania.ObtenerCuestionarioH2VADao().GetLastCuestionarioH2VAId();
        }

        public CuestionarioH2VADTO GetCuestionarioH2VAById(int id)
        {
            return FactoryCampania.ObtenerCuestionarioH2VADao().GetCuestionarioH2VAById(id);
        }
        public List<CuestionarioH2VADTO> GetFormatoH2VAByFilter(string plancodi, string empresa, string estado)
        {
            return FactoryCampania.ObtenerCuestionarioH2VADao().GetFormatoH2VAByFilter(plancodi, empresa, estado);
        }
        public List<CuestionarioH2VBDTO> GetFormatoH2VBByFilter(string plancodi, string empresa, string estado)
        {
            return FactoryCampania.ObtenerCuestionarioH2VBDao().GetFormatoH2VBByFilter(plancodi, empresa, estado);
        }

        public List<CuestionarioH2VCDTO> GetFormatoH2VCByFilter(string plancodi, string empresa, string estado)
        {
            return FactoryCampania.ObtenerCuestionarioH2VCDao().GetFormatoH2VCByFilter(plancodi, empresa, estado);
        }

        public List<CuestionarioH2VEDTO> GetFormatoH2VEByFilter(string plancodi, string empresa, string estado)
        {
            return FactoryCampania.ObtenerCuestionarioH2VEDao().GetFormatoH2VEByFilter(plancodi, empresa, estado);
        }

        public List<CuestionarioH2VFDTO> GetFormatoH2VFByFilter(string plancodi, string empresa, string estado)
        {
            return FactoryCampania.ObtenerCuestionarioH2VFDao().GetFormatoH2VFByFilter(plancodi, empresa, estado);
        }

        /**/
        public List<CuestionarioH2VADet1DTO> GetCuestionarioH2VADet1ByProyCodi(int proyCodi)
        {
            return FactoryCampania.ObtenerCuestionarioH2VADet1Dao().GetCuestionarioH2VADet1Codi(proyCodi);
        }

        public bool SaveCuestionarioH2VADet1(CuestionarioH2VADet1DTO cuestionarioH2VADet1DTO)
        {
            return FactoryCampania.ObtenerCuestionarioH2VADet1Dao().SaveCuestionarioH2VADet1(cuestionarioH2VADet1DTO);
        }

        public bool DeleteCuestionarioH2VADet1ById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCuestionarioH2VADet1Dao().DeleteCuestionarioH2VADet1ById(id, usuario);
        }

        public int GetLastCuestionarioH2VADet1Id()
        {
            return FactoryCampania.ObtenerCuestionarioH2VADet1Dao().GetLastCuestionarioH2VADet1Id();
        }

        public List<CuestionarioH2VADet1DTO> GetCuestionarioH2VADet1ById(int id)
        {
            return FactoryCampania.ObtenerCuestionarioH2VADet1Dao().GetCuestionarioH2VADet1ById(id);
        }
        /**/
        public List<CuestionarioH2VADet2DTO> GetCuestionarioH2VADet2ByProyCodi(int proyCodi)
        {
            return FactoryCampania.ObtenerCuestionarioH2VADet2Dao().GetCuestionarioH2VADet2Codi(proyCodi);
        }

        public bool SaveCuestionarioH2VADet2(CuestionarioH2VADet2DTO cuestionarioH2VADet2DTO)
        {
            return FactoryCampania.ObtenerCuestionarioH2VADet2Dao().SaveCuestionarioH2VADet2(cuestionarioH2VADet2DTO);
        }

        public bool DeleteCuestionarioH2VADet2ById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCuestionarioH2VADet2Dao().DeleteCuestionarioH2VADet2ById(id, usuario);
        }

        public int GetLastCuestionarioH2VADet2Id()
        {
            return FactoryCampania.ObtenerCuestionarioH2VADet2Dao().GetLastCuestionarioH2VADet2Id();
        }

        public List<CuestionarioH2VADet2DTO> GetCuestionarioH2VADet2ById(int id)
        {
            return FactoryCampania.ObtenerCuestionarioH2VADet2Dao().GetCuestionarioH2VADet2ById(id);
        }
        /**/
        public List<CuestionarioH2VBDTO> GetCuestionarioH2VBByProyCodi(int proyCodi)
        {
            return FactoryCampania.ObtenerCuestionarioH2VBDao().GetCuestionarioH2VBCodi(proyCodi);
        }

        public bool SaveCuestionarioH2VB(CuestionarioH2VBDTO cuestionarioH2VBDTO)
        {
            return FactoryCampania.ObtenerCuestionarioH2VBDao().SaveCuestionarioH2VB(cuestionarioH2VBDTO);
        }

        public bool DeleteCuestionarioH2VBById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCuestionarioH2VBDao().DeleteCuestionarioH2VBById(id, usuario);
        }

        public int GetLastCuestionarioH2VBId()
        {
            return FactoryCampania.ObtenerCuestionarioH2VBDao().GetLastCuestionarioH2VBId();
        }

        public CuestionarioH2VBDTO GetCuestionarioH2VBById(int id)
        {
            return FactoryCampania.ObtenerCuestionarioH2VBDao().GetCuestionarioH2VBById(id);
        }
        /**/
        public List<CuestionarioH2VCDTO> GetCuestionarioH2VCByProyCodi(int proyCodi)
        {
            return FactoryCampania.ObtenerCuestionarioH2VCDao().GetCuestionarioH2VCCodi(proyCodi);
        }

        public bool SaveCuestionarioH2VC(CuestionarioH2VCDTO cuestionarioH2VCDTO)
        {
            return FactoryCampania.ObtenerCuestionarioH2VCDao().SaveCuestionarioH2VC(cuestionarioH2VCDTO);
        }

        public bool DeleteCuestionarioH2VCById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCuestionarioH2VCDao().DeleteCuestionarioH2VCById(id, usuario);
        }

        public int GetLastCuestionarioH2VCId()
        {
            return FactoryCampania.ObtenerCuestionarioH2VCDao().GetLastCuestionarioH2VCId();
        }

        public List<CuestionarioH2VCDTO> GetCuestionarioH2VCById(int id)
        {
            return FactoryCampania.ObtenerCuestionarioH2VCDao().GetCuestionarioH2VCById(id);
        }
        /**/
        public List<CuestionarioH2VEDTO> GetCuestionarioH2VEByProyCodi(int proyCodi)
        {
            return FactoryCampania.ObtenerCuestionarioH2VEDao().GetCuestionarioH2VECodi(proyCodi);
        }

        public bool SaveCuestionarioH2VE(CuestionarioH2VEDTO cuestionarioH2VEDTO)
        {
            return FactoryCampania.ObtenerCuestionarioH2VEDao().SaveCuestionarioH2VE(cuestionarioH2VEDTO);
        }

        public bool DeleteCuestionarioH2VEById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCuestionarioH2VEDao().DeleteCuestionarioH2VEById(id, usuario);
        }

        public int GetLastCuestionarioH2VEId()
        {
            return FactoryCampania.ObtenerCuestionarioH2VEDao().GetLastCuestionarioH2VEId();
        }

        public List<CuestionarioH2VEDTO> GetCuestionarioH2VEById(int id)
        {
            return FactoryCampania.ObtenerCuestionarioH2VEDao().GetCuestionarioH2VEById(id);
        }
        /**/
        public List<CuestionarioH2VFDTO> GetCuestionarioH2VFByProyCodi(int proyCodi)
        {
            return FactoryCampania.ObtenerCuestionarioH2VFDao().GetCuestionarioH2VFCodi(proyCodi);
        }

        public bool SaveCuestionarioH2VF(CuestionarioH2VFDTO cuestionarioH2VFDTO)
        {
            return FactoryCampania.ObtenerCuestionarioH2VFDao().SaveCuestionarioH2VF(cuestionarioH2VFDTO);
        }

        public bool DeleteCuestionarioH2VFById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCuestionarioH2VFDao().DeleteCuestionarioH2VFById(id, usuario);
        }

        public int GetLastCuestionarioH2VFId()
        {
            return FactoryCampania.ObtenerCuestionarioH2VFDao().GetLastCuestionarioH2VFId();
        }

        public CuestionarioH2VFDTO GetCuestionarioH2VFById(int id)
        {
            return FactoryCampania.ObtenerCuestionarioH2VFDao().GetCuestionarioH2VFById(id);
        }


        /**/
        public List<CuestionarioH2VGDTO> GetCuestionarioH2VGByProyCodi(int proyCodi)
        {
            return FactoryCampania.ObtenerCuestionarioH2VGDao().GetCamCuestionarioH2VG(proyCodi);
        }

        public bool SaveCuestionarioH2VG(CuestionarioH2VGDTO cuestionarioH2VGDTO)
        {
            return FactoryCampania.ObtenerCuestionarioH2VGDao().SaveCamCuestionarioH2VG(cuestionarioH2VGDTO);
        }

        public bool DeleteCuestionarioH2VGById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCuestionarioH2VGDao().DeleteCamCuestionarioH2VGById(id, usuario);
        }

        public int GetLastCuestionarioH2VGId()
        {
            return FactoryCampania.ObtenerCuestionarioH2VGDao().GetLastCamCuestionarioH2VGCodi();
        }

        public List<CuestionarioH2VGDTO> GetCuestionarioH2VGById(int id)
        {
            return FactoryCampania.ObtenerCuestionarioH2VGDao().GetCamCuestionarioH2VGById(id);
        }

        public List<CroFicha1DTO> GetCroFicha1ProyCodi(int proyCodi)
        {
            return FactoryCampania.ObtenerCamCroFicha1Dao().GetCroFicha1ProyCodi(proyCodi);
        }

        public bool SaveCroFicha1(CroFicha1DTO CroFicha1DTO)
        {
            return FactoryCampania.ObtenerCamCroFicha1Dao().SaveCroFicha1(CroFicha1DTO);
        }

        public bool DeleteCroFicha1ById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCamCroFicha1Dao().DeleteCroFicha1ById(id, usuario);
        }

        public int GetLastCroFicha1Id()
        {
            return FactoryCampania.ObtenerCamCroFicha1Dao().GetLastCroFicha1Id();
        }

        public CroFicha1DTO GetCroFicha1ById(int id)
        {
            return FactoryCampania.ObtenerCamCroFicha1Dao().GetCroFicha1ById(id);
        }

        public bool UpdateCroFicha1(CroFicha1DTO CroFicha1DTO)
        {
            return FactoryCampania.ObtenerCamCroFicha1Dao().UpdateCroFicha1(CroFicha1DTO);
        }

        public List<CroFicha1DetDTO> GetCroFicha1DetCodi(int proyCodi)
        {
            return FactoryCampania.ObtenerCamCroFicha1DetDao().GetCroFicha1DetCodi(proyCodi);
        }

        public bool SaveCroFicha1Det(CroFicha1DetDTO CroFicha1DetDTO)
        {
            return FactoryCampania.ObtenerCamCroFicha1DetDao().SaveCroFicha1Det(CroFicha1DetDTO);
        }

        public bool DeleteCroFicha1DetById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCamCroFicha1DetDao().DeleteCroFicha1DetById(id, usuario);
        }

        public int GetLastCroFicha1DetId()
        {
            return FactoryCampania.ObtenerCamCroFicha1DetDao().GetLastCroFicha1DetId();
        }

        public CroFicha1DetDTO GetCroFicha1DetById(int id)
        {
            return FactoryCampania.ObtenerCamCroFicha1DetDao().GetCroFicha1DetById(id);
        }

        public bool UpdateCroFicha1Det(CroFicha1DetDTO CroFicha1DetDTO)
        {
            return FactoryCampania.ObtenerCamCroFicha1DetDao().UpdateCroFicha1Det(CroFicha1DetDTO);
        }

        public List<CroFicha1DetDTO> GetT3CronoFicha1ByFilter(string plancodi, string empresa, string estado)
        {
            return FactoryCampania.ObtenerCamCroFicha1Dao().GetCroFicha1ByFilter(plancodi, empresa, estado);
        }

        /**/
        public List<LineasFichaADTO> GetLineasFichaACodi(int proyCodi)
        {
            return FactoryCampania.ObtenerCamLineasFichaADao().GetLineasFichaACodi(proyCodi);
        }

        public bool SaveLineasFichaA(LineasFichaADTO lineasFichaADTO)
        {
            return FactoryCampania.ObtenerCamLineasFichaADao().SaveLineasFichaA(lineasFichaADTO);
        }

        public bool DeleteLineasFichaAById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCamLineasFichaADao().DeleteLineasFichaAById(id, usuario);
        }

        public int GetLastLineasFichaAId()
        {
            return FactoryCampania.ObtenerCamLineasFichaADao().GetLastLineasFichaAId();
        }

        public LineasFichaADTO GetLineasFichaAById(int id)
        {
            return FactoryCampania.ObtenerCamLineasFichaADao().GetLineasFichaAById(id);
        }

        public List<LineasFichaADTO> GetLineasFichaAByFilter(string plancodi, string empresa, string estado)
        {
            return FactoryCampania.ObtenerCamLineasFichaADao().GetLineasFichaAByFilter(plancodi, empresa, estado);
        }

        public List<LineasFichaATramoDTO> GetLineasFichaATramoByFilter(string plancodi, string empresa, string estado)
        {
            return FactoryCampania.ObtenerCamLineasFichaADao().GetLineasFichaATramoByFilter(plancodi, empresa, estado);
        }
        /**/
        /**/
        public List<LineasFichaADet1DTO> GetLineasFichaADet1Codi(int proyCodi)
        {
            return FactoryCampania.ObtenerCamLineasFichaADet1Dao().GetLineasFichaADet1Codi(proyCodi);
        }

        public bool SaveLineasFichaADet1(LineasFichaADet1DTO LineasFichaADet1DTO)
        {
            return FactoryCampania.ObtenerCamLineasFichaADet1Dao().SaveLineasFichaADet1(LineasFichaADet1DTO);
        }

        public bool DeleteLineasFichaADet1ById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCamLineasFichaADet1Dao().DeleteLineasFichaADet1ById(id, usuario);
        }

        public int GetLastLineasFichaADet1Id()
        {
            return FactoryCampania.ObtenerCamLineasFichaADet1Dao().GetLastLineasFichaADet1Id();
        }

        public LineasFichaADet1DTO GetLineasFichaADet1ById(int id)
        {
            return FactoryCampania.ObtenerCamLineasFichaADet1Dao().GetLineasFichaADet1ById(id);
        }
        /**/
        /**/
        public List<LineasFichaADet2DTO> GetLineasFichaADet2Codi(int proyCodi)
        {
            return FactoryCampania.ObtenerCamLineasFichaADet2Dao().GetLineasFichaADet2Codi(proyCodi);
        }

        public bool SaveLineasFichaADet2(LineasFichaADet2DTO LineasFichaADet2DTO)
        {
            return FactoryCampania.ObtenerCamLineasFichaADet2Dao().SaveLineasFichaADet2(LineasFichaADet2DTO);
        }

        public bool DeleteLineasFichaADet2ById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCamLineasFichaADet2Dao().DeleteLineasFichaADet2ById(id, usuario);
        }

        public int GetLastLineasFichaADet2Id()
        {
            return FactoryCampania.ObtenerCamLineasFichaADet2Dao().GetLastLineasFichaADet2Codi();
        }

        public LineasFichaADet2DTO GetLineasFichaADet2ById(int id)
        {
            return FactoryCampania.ObtenerCamLineasFichaADet2Dao().GetLineasFichaADet2ById(id);
        }
        /**/
        public List<LineasFichaBDTO> GetLineasFichaBCodi(int proyCodi)
        {
            return FactoryCampania.ObtenerCamLineasFichaBDao().GetLineasFichaB(proyCodi);
        }

        public bool SaveLineasFichaB(LineasFichaBDTO lineasFichaBDTO)
        {
            return FactoryCampania.ObtenerCamLineasFichaBDao().SaveLineasFichaB(lineasFichaBDTO);
        }

        public bool DeleteLineasFichaBById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCamLineasFichaBDao().DeleteLineasFichaBById(id, usuario);
        }

        public int GetLastLineasFichaBId()
        {
            return FactoryCampania.ObtenerCamLineasFichaBDao().GetLastLineasFichaBCodi();
        }

        public LineasFichaBDTO GetLineasFichaBById(int id)
        {
            return FactoryCampania.ObtenerCamLineasFichaBDao().GetLineasFichaBById(id);
        }

        public List<LineasFichaBDetDTO> GetLineasFichaBByFilter(string plancodi, string empresa, string estado)
        {
            return FactoryCampania.ObtenerCamLineasFichaBDao().GetLineasFichaBByFilter(plancodi, empresa, estado);
        }

        /**/
        public List<LineasFichaBDetDTO> GetLineasFichaBDetCodi(int proyCodi)
        {
            return FactoryCampania.ObtenerCamLineasFichaBDetDao().GetLineasFichaBDet(proyCodi);
        }

        public bool SaveLineasFichaBDet(LineasFichaBDetDTO lineasFichaBDetDTO)
        {
            return FactoryCampania.ObtenerCamLineasFichaBDetDao().SaveLineasFichaBDet(lineasFichaBDetDTO);
        }

        public bool DeleteLineasFichaBDetById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCamLineasFichaBDetDao().DeleteLineasFichaBDetById(id, usuario);
        }

        public int GetLastLineasFichaBDetId()
        {
            return FactoryCampania.ObtenerCamLineasFichaBDetDao().GetLastLineasFichaBDetCodi();
        }

        public LineasFichaBDetDTO GetLineasFichaBDetById(int id)
        {
            return FactoryCampania.ObtenerCamLineasFichaBDetDao().GetLineasFichaBDetById(id);
        }

        public List<SubestFicha1DTO> GetSubestFicha1(int proyCodi)
        {
            return FactoryCampania.ObtenerCamSubestFicha1Dao().GetSubestFicha1(proyCodi);
        }

        public bool SaveSubestFicha1(SubestFicha1DTO subestFicha1)
        {
            return FactoryCampania.ObtenerCamSubestFicha1Dao().SaveSubestFicha1(subestFicha1);
        }

        public bool DeleteSubestFicha1ById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCamSubestFicha1Dao().DeleteSubestFicha1ById(id, usuario);
        }

        public int GetLastSubestFicha1Id()
        {
            return FactoryCampania.ObtenerCamSubestFicha1Dao().GetLastSubestFicha1Id();
        }

        public SubestFicha1DTO GetSubestFicha1ById(int id)
        {
            return FactoryCampania.ObtenerCamSubestFicha1Dao().GetSubestFicha1ById(id);
        }

        public List<SubestFicha1Det1DTO> GetSubestFicha1Det1(int proyCodi)
        {
            return FactoryCampania.ObtenerCamSubestFicha1Det1Dao().GetSubestFicha1Det1(proyCodi);
        }

        public bool SaveSubestFicha1Det1(SubestFicha1Det1DTO SubestFicha1Det1)
        {
            return FactoryCampania.ObtenerCamSubestFicha1Det1Dao().SaveSubestFicha1Det1(SubestFicha1Det1);
        }

        public bool DeleteSubestFicha1Det1ById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCamSubestFicha1Det1Dao().DeleteSubestFicha1Det1ById(id, usuario);
        }

        public int GetLastSubestFicha1Det1Id()
        {
            return FactoryCampania.ObtenerCamSubestFicha1Det1Dao().GetLastSubestFicha1Det1Id();
        }

        public List<SubestFicha1Det1DTO> GetSubestFicha1Det1ById(int id)
        {
            return FactoryCampania.ObtenerCamSubestFicha1Det1Dao().GetSubestFicha1Det1ById(id);
        }

        public List<SubestFicha1Det2DTO> GetSubestFicha1Det2(int proyCodi)
        {
            return FactoryCampania.ObtenerCamSubestFicha1Det2Dao().GetSubestFicha1Det2(proyCodi);
        }

        public bool SaveSubestFicha1Det2(SubestFicha1Det2DTO SubestFicha1Det2)
        {
            return FactoryCampania.ObtenerCamSubestFicha1Det2Dao().SaveSubestFicha1Det2(SubestFicha1Det2);
        }

        public bool DeleteSubestFicha1Det2ById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCamSubestFicha1Det2Dao().DeleteSubestFicha1Det2ById(id, usuario);
        }

        public int GetLastSubestFicha1Det2Id()
        {
            return FactoryCampania.ObtenerCamSubestFicha1Det2Dao().GetLastSubestFicha1Det2Id();
        }

        public List<SubestFicha1Det2DTO> GetSubestFicha1Det2ById(int id)
        {
            return FactoryCampania.ObtenerCamSubestFicha1Det2Dao().GetSubestFicha1Det2ById(id);
        }

        public List<SubestFicha1Det3DTO> GetSubestFicha1Det3(int proyCodi)
        {
            return FactoryCampania.ObtenerCamSubestFicha1Det3Dao().GetSubestFicha1Det3(proyCodi);
        }

        public bool SaveSubestFicha1Det3(SubestFicha1Det3DTO SubestFicha1Det3)
        {
            return FactoryCampania.ObtenerCamSubestFicha1Det3Dao().SaveSubestFicha1Det3(SubestFicha1Det3);
        }

        public bool DeleteSubestFicha1Det3ById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCamSubestFicha1Det3Dao().DeleteSubestFicha1Det3ById(id, usuario);
        }

        public int GetLastSubestFicha1Det3Id()
        {
            return FactoryCampania.ObtenerCamSubestFicha1Det3Dao().GetLastSubestFicha1Det3Id();
        }

        public List<SubestFicha1Det3DTO> GetSubestFicha1Det3ById(int id)
        {
            return FactoryCampania.ObtenerCamSubestFicha1Det3Dao().GetSubestFicha1Det3ById(id);
        }




        public void EnviarCorreoNotificacion(EnvioDto envioDto, int plantcodi)
        {
            SiPlantillacorreoDTO plantilla = servCorreo.GetByIdSiPlantillacorreo(plantcodi);
            Dictionary<string, string> mapaVariable = LlenarVariablesCorreos(envioDto);
            try
            {
                string from = CorreoAppServicio.GetTextoSinVariable(plantilla.PlanticorreoFrom, mapaVariable);
                string to = CorreoAppServicio.GetTextoSinVariable(plantilla.Planticorreos, mapaVariable);
                string cc = CorreoAppServicio.GetTextoSinVariable(plantilla.PlanticorreosCc, mapaVariable);
                string bcc = CorreoAppServicio.GetTextoSinVariable(plantilla.PlanticorreosBcc, mapaVariable);
                string asunto = CorreoAppServicio.GetTextoSinVariable(plantilla.Plantasunto, mapaVariable);
                string contenido = CorreoAppServicio.GetTextoSinVariable(plantilla.Plantcontenido, mapaVariable);

                //List<string> listaTo = CorreoAppServicio.ListarCorreosValidoSegunAmbiente(to, false);
                //List<string> listaCC = CorreoAppServicio.ListarCorreosValidoSegunAmbiente(cc, false);
                //List<string> listaBCC = CorreoAppServicio.ListarCorreosValidoSegunAmbiente(bcc, true);
                //to = string.Join(";", listaTo);
                Logger.Info("mailto: " + to);
                Logger.Info("asunto: " + asunto);
                Logger.Info("contenido: " + contenido);

                string asuntoSendEmail = CorreoAppServicio.GetTextoAsuntoSegunAmbiente(asunto);
                COES.Base.Tools.Util.SendEmail(to, envioDto.Correos, asunto, contenido);

                //var correo = new SiCorreoDTO();
                //correo.Corrasunto = asunto;
                //correo.Corrcontenido = contenido;
                //correo.Corrfechaenvio = fechaRegistro;
                //correo.Corrfechaperiodo = regEnvio.Ftenvfecsolicitud;
                //correo.Corrfrom = from;
                //correo.Corrto = to;
                //correo.Corrcc = cc;
                //correo.Emprcodi = regEnvio.Emprcodi;
                //correo.Enviocodi = regEnvio.Ftenvcodi;
                //correo.Plantcodi = plantcodi;

                //servCorreo.SaveSiCorreo(correo);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
            }
        }

        public Dictionary<string, string> LlenarVariablesCorreos(EnvioDto envioDto)
        {
            Dictionary<string, string> lstSalida = new Dictionary<string, string>();
            lstSalida.Add(ConstantesCampania.VAR_CODIGO_TRANSMISION, Convert.ToString(envioDto.PlanTransmision.Plancodi));
            lstSalida.Add(ConstantesCampania.VAR_NOMBRE_EMPRESA, envioDto.PlanTransmision.Nomempresa);
            var ListaTrsmProyecto = "<ul>";
            foreach (var proyecto in envioDto.ListaTrsmProyecto)
            {
                ListaTrsmProyecto += "<li style='font-size:12px;margin-bottom:8px'><div>";
                ListaTrsmProyecto += "Proyecto de " + proyecto.TipoNombre;
                if (!string.IsNullOrEmpty(proyecto.TipofiNom))
                {
                    ListaTrsmProyecto += " " + proyecto.TipofiNom;
                }

                ListaTrsmProyecto += ": " + proyecto.Proynombre;
                ListaTrsmProyecto += "</div></li>";
            }
            ListaTrsmProyecto += "</ul>";
            lstSalida.Add(ConstantesCampania.VAR_LISTA_PROYECTOS, ListaTrsmProyecto);
            lstSalida.Add(ConstantesCampania.VAR_FECHA_ENVIO, DateTime.Now.ToString("dd/MM/yyyy"));
            lstSalida.Add(ConstantesCampania.VAR_NOMBRE_PERIODO, envioDto.PlanTransmision.PeriNombre);
            lstSalida.Add(ConstantesCampania.VAR_COMENTARIOS, envioDto.Comentarios);
            return lstSalida;
        }

        public void EnviarCorreoNotificacionRevision(EnvioDto envioDto, int plantcodi)
        {
            SiPlantillacorreoDTO plantilla = servCorreo.GetByIdSiPlantillacorreo(plantcodi);
            Dictionary<string, string> mapaVariable = LlenarVariablesCorreosRevision(envioDto);
            try
            {
                string from = CorreoAppServicio.GetTextoSinVariable(plantilla.PlanticorreoFrom, mapaVariable);
                string to = CorreoAppServicio.GetTextoSinVariable(plantilla.Planticorreos, mapaVariable);
                string cc = CorreoAppServicio.GetTextoSinVariable(plantilla.PlanticorreosCc, mapaVariable);
                string bcc = CorreoAppServicio.GetTextoSinVariable(plantilla.PlanticorreosBcc, mapaVariable);
                string asunto = CorreoAppServicio.GetTextoSinVariable(plantilla.Plantasunto, mapaVariable);
                string contenido = CorreoAppServicio.GetTextoSinVariable(plantilla.Plantcontenido, mapaVariable);

                //List<string> listaTo = CorreoAppServicio.ListarCorreosValidoSegunAmbiente(to, false);
                //List<string> listaCC = CorreoAppServicio.ListarCorreosValidoSegunAmbiente(cc, false);
                //List<string> listaBCC = CorreoAppServicio.ListarCorreosValidoSegunAmbiente(bcc, true);
                Logger.Info("mailto: " + to);
                Logger.Info("asunto: " + asunto);
                Logger.Info("contenido: " + contenido);

                string asuntoSendEmail = CorreoAppServicio.GetTextoAsuntoSegunAmbiente(asunto);
                COES.Base.Tools.Util.SendEmail(envioDto.Correos, cc, asunto, contenido);

                //var correo = new SiCorreoDTO();
                //correo.Corrasunto = asunto;
                //correo.Corrcontenido = contenido;
                //correo.Corrfechaenvio = fechaRegistro;
                //correo.Corrfechaperiodo = regEnvio.Ftenvfecsolicitud;
                //correo.Corrfrom = from;
                //correo.Corrto = to;
                //correo.Corrcc = cc;
                //correo.Emprcodi = regEnvio.Emprcodi;
                //correo.Enviocodi = regEnvio.Ftenvcodi;
                //correo.Plantcodi = plantcodi;

                //servCorreo.SaveSiCorreo(correo);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
            }
        }

        public Dictionary<string, string> LlenarVariablesCorreosRevision(EnvioDto envioDto)
        {
            Dictionary<string, string> lstSalida = new Dictionary<string, string>();
            lstSalida.Add(ConstantesCampania.VAR_NOMBRE_TRANSMISION, envioDto.PlanTransmision.PeriNombre);
            lstSalida.Add(ConstantesCampania.VAR_NOMBRE_EMPRESA, envioDto.PlanTransmision.Nomempresa);
            lstSalida.Add(ConstantesCampania.VAR_COMENTARIOS, envioDto.Comentarios);
            return lstSalida;
        }

        public List<T2SubestFicha1DTO> GetT2SubestFicha1(int proyCodi)
        {
            return FactoryCampania.ObtenerCamT2SubestFicha1Dao().GetT2SubestFicha1(proyCodi);
        }

        public bool SaveT2SubestFicha1(T2SubestFicha1DTO subestFicha1)
        {
            return FactoryCampania.ObtenerCamT2SubestFicha1Dao().SaveT2SubestFicha1(subestFicha1);
        }

        public bool DeleteT2SubestFicha1ById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCamT2SubestFicha1Dao().DeleteT2SubestFicha1ById(id, usuario);
        }

        public int GetLastT2SubestFicha1Id()
        {
            return FactoryCampania.ObtenerCamT2SubestFicha1Dao().GetLastT2SubestFicha1Id();
        }

        public T2SubestFicha1DTO GetT2SubestFicha1ById(int id)
        {
            return FactoryCampania.ObtenerCamT2SubestFicha1Dao().GetT2SubestFicha1ById(id);
        }

        public List<T2SubestFicha1Det1DTO> GetT2SubestFicha1Det1(int proyCodi)
        {
            return FactoryCampania.ObtenerCamT2SubestFicha1Det1Dao().GetT2SubestFicha1Det1(proyCodi);
        }

        public bool SaveT2SubestFicha1Det1(T2SubestFicha1Det1DTO T2SubestFicha1Det1)
        {
            return FactoryCampania.ObtenerCamT2SubestFicha1Det1Dao().SaveT2SubestFicha1Det1(T2SubestFicha1Det1);
        }

        public bool DeleteT2SubestFicha1Det1ById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCamT2SubestFicha1Det1Dao().DeleteT2SubestFicha1Det1ById(id, usuario);
        }

        public int GetLastT2SubestFicha1Det1Id()
        {
            return FactoryCampania.ObtenerCamT2SubestFicha1Det1Dao().GetLastT2SubestFicha1Det1Id();
        }

        public List<T2SubestFicha1Det1DTO> GetT2SubestFicha1Det1ById(int id)
        {
            return FactoryCampania.ObtenerCamT2SubestFicha1Det1Dao().GetT2SubestFicha1Det1ById(id);
        }

        public List<T2SubestFicha1Det2DTO> GetT2SubestFicha1Det2(int proyCodi)
        {
            return FactoryCampania.ObtenerCamT2SubestFicha1Det2Dao().GetT2SubestFicha1Det2(proyCodi);
        }

        public bool SaveT2SubestFicha1Det2(T2SubestFicha1Det2DTO T2SubestFicha1Det2)
        {
            return FactoryCampania.ObtenerCamT2SubestFicha1Det2Dao().SaveT2SubestFicha1Det2(T2SubestFicha1Det2);
        }

        public bool DeleteT2SubestFicha1Det2ById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCamT2SubestFicha1Det2Dao().DeleteT2SubestFicha1Det2ById(id, usuario);
        }

        public int GetLastT2SubestFicha1Det2Id()
        {
            return FactoryCampania.ObtenerCamT2SubestFicha1Det2Dao().GetLastT2SubestFicha1Det2Id();
        }

        public List<T2SubestFicha1Det2DTO> GetT2SubestFicha1Det2ById(int id)
        {
            return FactoryCampania.ObtenerCamT2SubestFicha1Det2Dao().GetT2SubestFicha1Det2ById(id);
        }

        public List<T2SubestFicha1Det3DTO> GetT2SubestFicha1Det3(int proyCodi)
        {
            return FactoryCampania.ObtenerCamT2SubestFicha1Det3Dao().GetT2SubestFicha1Det3(proyCodi);
        }

        public bool SaveT2SubestFicha1Det3(T2SubestFicha1Det3DTO T2SubestFicha1Det3)
        {
            return FactoryCampania.ObtenerCamT2SubestFicha1Det3Dao().SaveT2SubestFicha1Det3(T2SubestFicha1Det3);
        }

        public bool DeleteT2SubestFicha1Det3ById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCamT2SubestFicha1Det3Dao().DeleteT2SubestFicha1Det3ById(id, usuario);
        }

        public int GetLastT2SubestFicha1Det3Id()
        {
            return FactoryCampania.ObtenerCamT2SubestFicha1Det3Dao().GetLastT2SubestFicha1Det3Id();
        }

        public List<T2SubestFicha1Det3DTO> GetT2SubestFicha1Det3ById(int id)
        {
            return FactoryCampania.ObtenerCamT2SubestFicha1Det3Dao().GetT2SubestFicha1Det3ById(id);
        }

        public List<T2SubestFicha1DTO> GetT2SubFicha1ByFilter(string plancodi, string empresa, string estado)
        {
            return FactoryCampania.ObtenerCamT2SubestFicha1Dao().GetT2SubFicha1ByFilter(plancodi, empresa, estado);
        }

        public List<T2SubestFicha1TransDTO> GetT2SubFicha1TransByFilter(string plancodi, string empresa, string estado)
        {
            return FactoryCampania.ObtenerCamT2SubestFicha1Dao().GetT2SubFicha1TransByFilter(plancodi, empresa, estado);
        }

        public List<T2SubestFicha1EquiDTO> GetT2SubFicha1EquiByFilter(string plancodi, string empresa, string estado)
        {
            return FactoryCampania.ObtenerCamT2SubestFicha1Dao().GetT2SubFicha1EquiByFilter(plancodi, empresa, estado);
        }

        /**/
        public List<T1LinFichaADTO> GetT1LineasFichaACodi(int proyCodi)
        {
            return FactoryCampania.ObtenerCamT1LineasFichaADao().GetLineasFichaACodi(proyCodi);
        }

        public bool SaveT1LineasFichaA(T1LinFichaADTO lineasFichaADTO)
        {
            return FactoryCampania.ObtenerCamT1LineasFichaADao().SaveLineasFichaA(lineasFichaADTO);
        }

        public bool DeleteT1LineasFichaAById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCamT1LineasFichaADao().DeleteLineasFichaAById(id, usuario);
        }

        public int GetLastT1LineasFichaAId()
        {
            return FactoryCampania.ObtenerCamT1LineasFichaADao().GetLastLineasFichaAId();
        }

        public T1LinFichaADTO GetLineasT1FichaAById(int id)
        {
            return FactoryCampania.ObtenerCamT1LineasFichaADao().GetLineasFichaAById(id);
        }
        
        public List<T1LinFichaADet1DTO> GetT1LineasFichaADet1Codi(int proyCodi)
        {
            return FactoryCampania.ObtenerCamT1LineasFichaADet1Dao().GetLineasFichaADet1Codi(proyCodi);
        }

        public bool SaveT1LineasFichaADet1(T1LinFichaADet1DTO LineasFichaADet1DTO)
        {
            return FactoryCampania.ObtenerCamT1LineasFichaADet1Dao().SaveLineasFichaADet1(LineasFichaADet1DTO);
        }

        public bool DeleteLineasT1FichaADet1ById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCamT1LineasFichaADet1Dao().DeleteLineasFichaADet1ById(id, usuario);
        }

        public int GetLastT1LineasFichaADet1Id()
        {
            return FactoryCampania.ObtenerCamT1LineasFichaADet1Dao().GetLastLineasFichaADet1Id();
        }

        public T1LinFichaADet1DTO GetLineasT1FichaADet1ById(int id)
        {
            return FactoryCampania.ObtenerCamT1LineasFichaADet1Dao().GetLineasFichaADet1ById(id);
        }
        public List<T1LinFichaADet2DTO> GetT1LineasFichaADet2Codi(int proyCodi)
        {
            return FactoryCampania.ObtenerCamT1LineasFichaADet2Dao().GetLineasFichaADet2Codi(proyCodi);
        }

        public bool SaveT1LineasFichaADet2(T1LinFichaADet2DTO LineasFichaADet2DTO)
        {
            return FactoryCampania.ObtenerCamT1LineasFichaADet2Dao().SaveLineasFichaADet2(LineasFichaADet2DTO);
        }

        public bool DeleteT1LineasFichaADet2ById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCamT1LineasFichaADet2Dao().DeleteLineasFichaADet2ById(id, usuario);
        }

        public int GetLastT1LineasFichaADet2Id()
        {
            return FactoryCampania.ObtenerCamT1LineasFichaADet2Dao().GetLastLineasFichaADet2Id();
        }

        public T1LinFichaADet2DTO GetT1LineasFichaADet2ById(int id)
        {
            return FactoryCampania.ObtenerCamT1LineasFichaADet2Dao().GetLineasFichaADet2ById(id);
        }

        public string GetDirectory()
        {
            return ConfigurationManager.AppSettings["LocalDirectory"];
        }

        public Stream DownloadToStream(string sourceBlobName)
        {
                Stream stream = new MemoryStream();
   ;

                if (File.Exists(sourceBlobName))
                {
                    stream = File.OpenRead(sourceBlobName);
                }

                return stream;
        }

        public List<DataSubestacionDTO> ListParamSubestacion()
        {
            return FactoryCampania.ObtenerCamDatacatalogoDao().GetParamSubestacion();
        }

        public List<ObservacionDTO> GetObservacionByProyCodi(int proyCodi)
        {
            return FactoryCampania.ObtenerCamObservacionDao().GetObservacionByProyCodi(proyCodi);
        }
        public int SaveObservacion(ObservacionDTO observacionDTO)
        {
            return FactoryCampania.ObtenerCamObservacionDao().SaveObservacion(observacionDTO);
        }
        public bool DeleteObservacionById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCamObservacionDao().DeleteObservacionById(id, usuario);
        }
        public int GetLastObservacionId()
        {
            return FactoryCampania.ObtenerCamObservacionDao().GetLastObservacionId();
        }
        public ObservacionDTO GetObservacionById(int id)
        {
            return FactoryCampania.ObtenerCamObservacionDao().GetObservacionById(id);
        }
        public bool UpdateObservacion(ObservacionDTO observacionDTO)
        {
            return FactoryCampania.ObtenerCamObservacionDao().UpdateObservacion(observacionDTO);
        }
         public bool EnviarObservacionByProyecto(int idProyecto)
        {
            return FactoryCampania.ObtenerCamObservacionDao().EnviarObservacionByProyecto(idProyecto);
        }
        public List<RespuestaObsDTO> GetRespuestaObsByObs(int observacion)
        {
            return FactoryCampania.ObtenerCamRespuestaObsDao().GetRespuestaObsByObs(observacion);
        }

         public bool UpdateObservacionByProyectoAbs(int idProyecto, string estado)
        {
            return FactoryCampania.ObtenerCamObservacionDao().UpdateObservacionByProyecto(idProyecto, estado);
        }

        public bool SaveRespuestaObs(RespuestaObsDTO respuestaObsDTO)
        {
            return FactoryCampania.ObtenerCamRespuestaObsDao().SaveRespuestaObs(respuestaObsDTO);
        }
        public bool DeleteRespuestaObsById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCamRespuestaObsDao().DeleteRespuestaObsById(id, usuario);
        }
        public int GetLastRespuestaObsId()
        {
            return FactoryCampania.ObtenerCamRespuestaObsDao().GetLastRespuestaObsId();
        }
        public RespuestaObsDTO GetRespuestaObsById(int id)
        {
            return FactoryCampania.ObtenerCamRespuestaObsDao().GetRespuestaObsById(id);
        }
        public bool UpdateRespuestaObs(RespuestaObsDTO respuestaObsDTO)
        {
            return FactoryCampania.ObtenerCamRespuestaObsDao().UpdateRespuestaObs(respuestaObsDTO);
        }

        public List<ArchivoObsDTO> GetArchivoObsByObsId(int observacion, string tipo)
        {
            return FactoryCampania.ObtenerCamArchivoObsDao().GetArchivoObsByObsId(observacion, tipo);
        }

        public bool SaveArchivoObs(ArchivoObsDTO archivoObsDTO)
        {
            return FactoryCampania.ObtenerCamArchivoObsDao().SaveArchivoObs(archivoObsDTO);
        }
        public bool DeleteArchivoObsById(int id, string usuario)
        {
            return FactoryCampania.ObtenerCamArchivoObsDao().DeleteArchivoObsById(id, usuario);
        }
        public int GetLastArchivoObsId()
        {
            return FactoryCampania.ObtenerCamArchivoObsDao().GetLastArchivoObsId();
        }
        public ArchivoObsDTO GetArchivoObsById(int id)
        {
            return FactoryCampania.ObtenerCamArchivoObsDao().GetArchivoObsById(id);
        }
        public ArchivoObsDTO GetArchivoObsNombreArchivo(string nombre)
        {
            return FactoryCampania.ObtenerCamArchivoObsDao().GetArchivoObsNombreArchivo(nombre);
        }
        public bool UpdateArchivoObs(ArchivoObsDTO archivoObsDTO)
        {
            return FactoryCampania.ObtenerCamArchivoObsDao().UpdateArchivoObs(archivoObsDTO);
        }
        public List<ArchivoObsDTO> GetArchivoObsProyCodiNom(int observacionId, string tipo, string nombre)
        {
            return FactoryCampania.ObtenerCamArchivoObsDao().GetArchivoObsProyCodiNom(observacionId, tipo, nombre);
        }

         public bool GetObservacionByPlanCodi(int planCodi)
        {
            return FactoryCampania.ObtenerCamObservacionDao().GetObservacionByPlanCodi(planCodi);
        }

        public void EnviarCorreoNotificacionObservacion(EnvioDto envioDto, int plantcodi, bool sendClient)
        {
            SiPlantillacorreoDTO plantilla = servCorreo.GetByIdSiPlantillacorreo(plantcodi);
            Dictionary<string, string> mapaVariable = LlenarVariablesCorreosObservacion(envioDto);
            try
            {
                string from = CorreoAppServicio.GetTextoSinVariable(plantilla.PlanticorreoFrom, mapaVariable);
                string to = CorreoAppServicio.GetTextoSinVariable(plantilla.Planticorreos, mapaVariable);
                string cc = CorreoAppServicio.GetTextoSinVariable(plantilla.PlanticorreosCc, mapaVariable);
                string bcc = CorreoAppServicio.GetTextoSinVariable(plantilla.PlanticorreosBcc, mapaVariable);
                string asunto = CorreoAppServicio.GetTextoSinVariable(plantilla.Plantasunto, mapaVariable);
                string contenido = CorreoAppServicio.GetTextoSinVariable(plantilla.Plantcontenido, mapaVariable);

                Logger.Info("mailto: " + to);
                Logger.Info("asunto: " + asunto);
                Logger.Info("contenido: " + contenido);

                string asuntoSendEmail = CorreoAppServicio.GetTextoAsuntoSegunAmbiente(asunto);
                if(sendClient) COES.Base.Tools.Util.SendEmail(envioDto.Correos, cc, asunto, contenido);
                else COES.Base.Tools.Util.SendEmail(to, envioDto.Correos, asunto, contenido);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
            }
        }

        public Dictionary<string, string> LlenarVariablesCorreosObservacion(EnvioDto envioDto)
        {
            Dictionary<string, string> lstSalida = new Dictionary<string, string>();
            if(envioDto.TransmisionProyectoDTO != null){
                lstSalida.Add(ConstantesCampania.VAR_NOMBRE_PROYECTO, envioDto.TransmisionProyectoDTO.Proynombre);
                lstSalida.Add(ConstantesCampania.VAR_NOMBRE_TRANSMISION, envioDto.TransmisionProyectoDTO.NomPeri);
                lstSalida.Add(ConstantesCampania.VAR_NOMBRE_EMPRESA, envioDto.TransmisionProyectoDTO.EmpresaNom);
            } else {
                lstSalida.Add(ConstantesCampania.VAR_NOMBRE_TRANSMISION, envioDto.PlanTransmision.PeriNombre);
                lstSalida.Add(ConstantesCampania.VAR_NOMBRE_EMPRESA, envioDto.PlanTransmision.Nomempresa);
            }
            
            return lstSalida;
        }

    }
}
