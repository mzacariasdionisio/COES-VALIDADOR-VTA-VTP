using System;
using System.Collections.Generic;
using System.Linq;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using COES.Servicios.Aplicacion.Eventos.Helper;
using System.Configuration;
using COES.Servicios.Aplicacion.General.Helper;
using COES.Dominio.DTO.ReportesFrecuencia;
using COES.Servicios.Aplicacion.Intervenciones.Helper;

namespace COES.Servicios.Aplicacion.Correo
{
    /// <summary>
    /// Clases con métodos del módulo Correo
    /// </summary>
    public class CorreoAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(CorreoAppServicio));

        #region Métodos Tabla SI_CORREO

        /// <summary>
        /// Inserta un registro de la tabla SI_CORREO
        /// </summary>
        public void SaveSiCorreo(SiCorreoDTO entity)
        {
            try
            {
                FactorySic.GetSiCorreoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                //Logger.Error(Constantes.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla SI_CORREO
        /// </summary>
        public void UpdateSiCorreo(SiCorreoDTO entity)
        {
            try
            {
                FactorySic.GetSiCorreoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                //Logger.Error(Constantes.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla SI_CORREO
        /// </summary>
        public void DeleteSiCorreo(int corrcodi)
        {
            try
            {
                FactorySic.GetSiCorreoRepository().Delete(corrcodi);
            }
            catch (Exception ex)
            {
                //Logger.Error(Constantes.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SI_CORREO
        /// </summary>
        public SiCorreoDTO GetByIdSiCorreo(int corrcodi)
        {
            return FactorySic.GetSiCorreoRepository().GetById(corrcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SI_CORREO
        /// </summary>
        public List<SiCorreoDTO> ListSiCorreos()
        {
            return FactorySic.GetSiCorreoRepository().List();
        }

        #endregion

        #region Métodos Tabla SI_CORREO_ARCHIVO

        /// <summary>
        /// Inserta un registro de la tabla SI_CORREO_ARCHIVO
        /// </summary>
        public void SaveSiCorreoArchivo(SiCorreoArchivoDTO entity)
        {
            try
            {
                FactorySic.GetSiCorreoArchivoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla SI_CORREO_ARCHIVO
        /// </summary>
        public void UpdateSiCorreoArchivo(SiCorreoArchivoDTO entity)
        {
            try
            {
                FactorySic.GetSiCorreoArchivoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla SI_CORREO_ARCHIVO
        /// </summary>
        public void DeleteSiCorreoArchivo(int earchcodi)
        {
            try
            {
                FactorySic.GetSiCorreoArchivoRepository().Delete(earchcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SI_CORREO_ARCHIVO
        /// </summary>
        public SiCorreoArchivoDTO GetByIdSiCorreoArchivo(int earchcodi)
        {
            return FactorySic.GetSiCorreoArchivoRepository().GetById(earchcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SI_CORREO_ARCHIVO
        /// </summary>
        public List<SiCorreoArchivoDTO> ListSiCorreoArchivos()
        {
            return FactorySic.GetSiCorreoArchivoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla SiCorreoArchivo
        /// </summary>
        public List<SiCorreoArchivoDTO> GetByCriteriaSiCorreoArchivos()
        {
            return FactorySic.GetSiCorreoArchivoRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla SI_PLANTILLACORREO

        /// <summary>
        /// Inserta un registro de la tabla SI_PLANTILLACORREO
        /// </summary>
        public void SaveSiPlantillacorreo(SiPlantillacorreoDTO entity)
        {
            try
            {
                FactorySic.GetSiPlantillacorreoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                //Logger.Error(Constantes.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla SI_PLANTILLACORREO
        /// </summary>
        public void UpdateSiPlantillacorreo(SiPlantillacorreoDTO entity)
        {
            try
            {
                FactorySic.GetSiPlantillacorreoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                //Logger.Error(Constantes.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite actualizar los datos de las plantilla de correo
        /// </summary>
        /// <param name="entity"></param>
        public void ActualizarPlantilla(SiPlantillacorreoDTO entity)
        {
            try
            {
                FactorySic.GetSiPlantillacorreoRepository().ActualizarPlantilla(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla SI_PLANTILLACORREO
        /// </summary>
        public void DeleteSiPlantillacorreo(int plantcodi)
        {
            try
            {
                FactorySic.GetSiPlantillacorreoRepository().Delete(plantcodi);
            }
            catch (Exception ex)
            {
                //Logger.Error(Constantes.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SI_PLANTILLACORREO
        /// </summary>
        public SiPlantillacorreoDTO GetByIdSiPlantillacorreo(int plantcodi)
        {
            return FactorySic.GetSiPlantillacorreoRepository().GetById(plantcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SI_PLANTILLACORREO
        /// </summary>
        public List<SiPlantillacorreoDTO> ListSiPlantillacorreos()
        {
            return FactorySic.GetSiPlantillacorreoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla SiPlantillacorreo
        /// </summary>
        public List<SiPlantillacorreoDTO> GetByCriteriaSiPlantillacorreos()
        {
            return FactorySic.GetSiPlantillacorreoRepository().GetByCriteria();
        }

        /// <summary>
        /// Permite obtener la plantilla correspondiente a un módulo
        /// </summary>
        /// <param name="idTipoPlantilla"></param>
        /// <param name="idModulo"></param>
        /// <returns></returns>
        public SiPlantillacorreoDTO ObtenerPlantillaPorModulo(int idTipoPlantilla, int idModulo)
        {
            return FactorySic.GetSiPlantillacorreoRepository().ObtenerPlantillaPorModulo(idTipoPlantilla, idModulo);
        }


        #endregion

        #region Métodos Tabla SI_TIPOPLANTILLACORREO


        /// <summary>
        /// Actualiza un registro de la tabla SI_TIPOPLANTILLACORREO
        /// </summary>
        public void UpdateSiTipoplantillacorreo(SiTipoplantillacorreoDTO entity)
        {
            try
            {
                FactorySic.GetSiTipoplantillacorreoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                //Logger.Error(Constantes.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla SI_TIPOPLANTILLACORREO
        /// </summary>
        public void DeleteSiTipoplantillacorreo(int tpcorrcodi)
        {
            try
            {
                FactorySic.GetSiTipoplantillacorreoRepository().Delete(tpcorrcodi);
            }
            catch (Exception ex)
            {
                //Logger.Error(Constantes.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SI_TIPOPLANTILLACORREO
        /// </summary>
        public SiTipoplantillacorreoDTO GetByIdSiTipoplantillacorreo(int tpcorrcodi)
        {
            return FactorySic.GetSiTipoplantillacorreoRepository().GetById(tpcorrcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SI_TIPOPLANTILLACORREO
        /// </summary>
        public List<SiTipoplantillacorreoDTO> ListSiTipoplantillacorreos()
        {
            return FactorySic.GetSiTipoplantillacorreoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla SiTipoplantillacorreo
        /// </summary>
        public List<SiTipoplantillacorreoDTO> GetByCriteriaSiTipoplantillacorreos()
        {
            return FactorySic.GetSiTipoplantillacorreoRepository().GetByCriteria();
        }


        #endregion

        /// <summary>
        /// Envío de correo alerta del centro de control
        /// </summary>
        public void EnviarCorreoAlmacenamientoCombustibleCco()
        {
            try
            {
                var oPlantilla = FactorySic.GetSiPlantillacorreoRepository().GetById(2);
                var lsCorreos = oPlantilla.Planticorreos.Split(';').ToList();
                var lsCorreoscc = new List<string>();
                var lsCorreosBcc = oPlantilla.PlanticorreosBcc.Split(';').ToList();
                Base.Tools.Util.SendEmail(lsCorreos, lsCorreoscc, lsCorreosBcc, oPlantilla.Plantasunto, oPlantilla.Plantcontenido, oPlantilla.PlanticorreoFrom);

                var oCorreo = new SiCorreoDTO
                {
                    Corrasunto = oPlantilla.Plantasunto,
                    Corrcontenido = oPlantilla.Plantcontenido,
                    Corrfechaenvio = DateTime.Now,
                    Corrto = oPlantilla.Planticorreos,
                    Plantcodi = oPlantilla.Plantcodi
                };
                // Envio correo Engie
                FactorySic.GetSiCorreoRepository().Save(oCorreo);
                var oPlantillaEngie = FactorySic.GetSiPlantillacorreoRepository().GetById(58);
                var lsCorreosEngie = oPlantillaEngie.Planticorreos.Split(';').ToList();
                var lsCorreosccEngie = new List<string>();
                var lsCorreosBccEngie = oPlantillaEngie.PlanticorreosBcc.Split(';').ToList();
                Base.Tools.Util.SendEmail(lsCorreosEngie, lsCorreosccEngie, lsCorreosBccEngie, oPlantillaEngie.Plantasunto, oPlantillaEngie.Plantcontenido, oPlantillaEngie.PlanticorreoFrom);

                var oCorreoEngie = new SiCorreoDTO
                {
                    Corrasunto = oPlantillaEngie.Plantasunto,
                    Corrcontenido = oPlantillaEngie.Plantcontenido,
                    Corrfechaenvio = DateTime.Now,
                    Corrto = oPlantillaEngie.Planticorreos,
                    Plantcodi = oPlantillaEngie.Plantcodi
                };

                FactorySic.GetSiCorreoRepository().Save(oCorreoEngie);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }

        }

        /// <summary>
        /// Envío de correo alerta del centro de control
        /// </summary>
        public void EnviarCorreoAnalisisFallaAlertaCitacion(string eventoCodigo, string fechaFormatoPeru, int codigoCorreo)
        {
            var oPlantilla = FactorySic.GetSiPlantillacorreoRepository().GetById(codigoCorreo);

            var lsCorreos = oPlantilla.Planticorreos != null ? oPlantilla.Planticorreos.Split(';').ToList() : new List<string>();
            var lsCorreoscc = oPlantilla.PlanticorreosCc != null ? oPlantilla.PlanticorreosCc.Split(';').ToList() : new List<string>();
            var lsCorreosBcc = oPlantilla.PlanticorreosBcc != null ? oPlantilla.PlanticorreosBcc.Split(';').ToList() : new List<string>();

            string htmlContent = string.Format(oPlantilla.Plantcontenido, eventoCodigo, fechaFormatoPeru);
            string footer = GetFooterCorreoAlertas();

            htmlContent = string.Concat(htmlContent, footer);

            string asunto = string.Format(oPlantilla.Plantasunto, eventoCodigo);

            Base.Tools.Util.SendEmail(
                lsCorreos,
                lsCorreoscc,
                lsCorreosBcc,
                asunto,
                htmlContent,
                oPlantilla.PlanticorreoFrom
            );
        }

        /// <summary>
        /// Envío de correo alerta elaboracion informe Ctaf
        /// </summary>
        public void EnviarCorreoAlertaElaboracionInformeCtaf(string eventoCodigo, string fechaFormatoPeru, int codigoCorreo)
        {
            var oPlantilla = FactorySic.GetSiPlantillacorreoRepository().GetById(codigoCorreo);

            var lsCorreos = oPlantilla.Planticorreos != null ? oPlantilla.Planticorreos.Split(';').ToList() : new List<string>();
            var lsCorreoscc = oPlantilla.PlanticorreosCc != null ? oPlantilla.PlanticorreosCc.Split(';').ToList() : new List<string>();
            var lsCorreosBcc = oPlantilla.PlanticorreosBcc != null ? oPlantilla.PlanticorreosBcc.Split(';').ToList() : new List<string>();

            string htmlContent = string.Format(oPlantilla.Plantcontenido, eventoCodigo, fechaFormatoPeru);
            string footer = GetFooterCorreoAlertas();

            htmlContent = string.Concat(htmlContent, footer);

            string asunto = string.Format(oPlantilla.Plantasunto, eventoCodigo);

            Base.Tools.Util.SendEmail(
                lsCorreos,
                lsCorreoscc,
                lsCorreosBcc,
                asunto,
                htmlContent,
                oPlantilla.PlanticorreoFrom
            );
        }

        /// <summary>
        /// Envío de correo alerta elaboracion informe Ctaf mas dosi dias habiles
        /// </summary>
        public void EnviarCorreoAlertaElaboracionInformeCtafMasDosDiasHabiles(string eventoCodigo, string fechaFormatoPeru, int codigoCorreo)
        {
            var oPlantilla = FactorySic.GetSiPlantillacorreoRepository().GetById(codigoCorreo);

            var lsCorreos = oPlantilla.Planticorreos != null ? oPlantilla.Planticorreos.Split(';').ToList() : new List<string>();
            var lsCorreoscc = oPlantilla.PlanticorreosCc != null ? oPlantilla.PlanticorreosCc.Split(';').ToList() : new List<string>();
            var lsCorreosBcc = oPlantilla.PlanticorreosBcc != null ? oPlantilla.PlanticorreosBcc.Split(';').ToList() : new List<string>();

            string htmlContent = string.Format(oPlantilla.Plantcontenido, eventoCodigo, fechaFormatoPeru);
            string footer = GetFooterCorreoAlertas();

            htmlContent = string.Concat(htmlContent, footer);

            string asunto = string.Format(oPlantilla.Plantasunto, eventoCodigo);

            Base.Tools.Util.SendEmail(
                lsCorreos,
                lsCorreoscc,
                lsCorreosBcc,
                asunto,
                htmlContent,
                oPlantilla.PlanticorreoFrom
            );
        }

        /// <summary>
        /// Envío de correo alerta elaboracion informe Ctaf
        /// </summary>
        public void EnviarCorreoAlertaElaboracionInformeTecnico(string eventoCodigo, string fechaFormatoPeru, int codigoCorreo)
        {
            var oPlantilla = FactorySic.GetSiPlantillacorreoRepository().GetById(codigoCorreo);

            var lsCorreos = oPlantilla.Planticorreos != null ? oPlantilla.Planticorreos.Split(';').ToList() : new List<string>();
            var lsCorreoscc = oPlantilla.PlanticorreosCc != null ? oPlantilla.PlanticorreosCc.Split(';').ToList() : new List<string>();
            var lsCorreosBcc = oPlantilla.PlanticorreosBcc != null ? oPlantilla.PlanticorreosBcc.Split(';').ToList() : new List<string>();

            string htmlContent = string.Format(oPlantilla.Plantcontenido, eventoCodigo, fechaFormatoPeru);
            string footer = GetFooterCorreoAlertas();

            htmlContent = string.Concat(htmlContent, footer);

            string asunto = string.Format(oPlantilla.Plantasunto, eventoCodigo);

            Base.Tools.Util.SendEmail(
                lsCorreos,
                lsCorreoscc,
                lsCorreosBcc,
                asunto,
                htmlContent,
                oPlantilla.PlanticorreoFrom
            );
        }

        /// <summary>
        /// Envío de correo alerta elaboracion informe Ctaf
        /// </summary>
        public void EnviarCorreoAlertaDatosFrecuencia(string eventoCodigo, string fechaFormatoPeru, int codigoCorreo)
        {
            var oPlantilla = FactorySic.GetSiPlantillacorreoRepository().GetById(codigoCorreo);

            var lsCorreos = oPlantilla.Planticorreos.Split(';').ToList();
            var lsCorreoscc = new List<string>();
            var lsCorreosBcc = new List<string>();

            string htmlContent = string.Format(oPlantilla.Plantcontenido, eventoCodigo, fechaFormatoPeru);
            string footer = GetFooterCorreoAlertas();

            htmlContent = string.Concat(htmlContent, footer);

            string asunto = string.Format(oPlantilla.Plantasunto, eventoCodigo);

            Base.Tools.Util.SendEmail(
                lsCorreos,
                lsCorreoscc,
                lsCorreosBcc,
                asunto,
                htmlContent,
                oPlantilla.PlanticorreoFrom
            );
        }

        /// <summary>
        /// Envío de correo alerta elaboracion informe Ctaf mas dosi dias habiles
        /// </summary>
        public void EnviarCorreoAlertaElaboracionInformeTecnicoMasDiasHabiles(string eventoCodigo, string fechaFormatoPeru, int codigoCorreo)
        {
            var oPlantilla = FactorySic.GetSiPlantillacorreoRepository().GetById(codigoCorreo);

            var lsCorreos = oPlantilla.Planticorreos != null ? oPlantilla.Planticorreos.Split(';').ToList() : new List<string>();
            var lsCorreoscc = oPlantilla.PlanticorreosCc != null ? oPlantilla.PlanticorreosCc.Split(';').ToList() : new List<string>();
            var lsCorreosBcc = oPlantilla.PlanticorreosBcc != null ? oPlantilla.PlanticorreosBcc.Split(';').ToList() : new List<string>();

            string htmlContent = string.Format(oPlantilla.Plantcontenido, eventoCodigo, fechaFormatoPeru);
            string footer = GetFooterCorreoAlertas();

            htmlContent = string.Concat(htmlContent, footer);

            string asunto = string.Format(oPlantilla.Plantasunto, eventoCodigo);

            Base.Tools.Util.SendEmail(
                lsCorreos,
                lsCorreoscc,
                lsCorreosBcc,
                asunto,
                htmlContent,
                oPlantilla.PlanticorreoFrom
            );
        }

        /// <summary>
        /// Envío de correo alerta elaboracion informe semanal
        /// </summary>
        public void EnviarCorreoAlertaElaboracionInformeTecnicoSemanal(List<ReporteSemanalItemDTO> codigosNominales, List<ReporteSemanalItemDTO> codigosCtaf, List<ReporteSemanalItemDTO> codigosElaboracionTecnico, int codigoCorreo)
        {
            var oPlantilla = FactorySic.GetSiPlantillacorreoRepository().GetById(codigoCorreo);

            var lsCorreos = oPlantilla.Planticorreos != null ? oPlantilla.Planticorreos.Split(';').ToList() : new List<string>();
            var lsCorreoscc = oPlantilla.PlanticorreosCc != null ? oPlantilla.PlanticorreosCc.Split(';').ToList() : new List<string>();
            var lsCorreosBcc = oPlantilla.PlanticorreosBcc != null ? oPlantilla.PlanticorreosBcc.Split(';').ToList() : new List<string>();

            string table = "<table border='0' cellspacing='0' width='70%' style='text-align: center; margin: auto; padding-top: 20px;'><tbody>";

            if (codigosNominales.Count != 0)
            {
                table = string.Concat(table, "<tr><td colspan='2' style='border-width: 1px; border-style: solid;'>CITACIONES</td></tr>");
            }

            foreach (var codigoNominal in codigosNominales)
            {
                table = string.Concat(table, $"<tr><td style='border-width: 1px; border-style: solid;'>{codigoNominal.CODIGO}</td><td style='border-width: 1px; border-style: solid;'>{codigoNominal.FECHA}</td></tr>");
            }

            if (codigosCtaf.Count != 0)
            {
                table = string.Concat(table, "<tr><td colspan='2' style='border-width: 1px; border-style: solid;'>REUNIÓN CTAF</td></tr>");
            }

            foreach (var codigoCtaf in codigosCtaf)
            {
                table = string.Concat(table, $"<tr><td style='border-width: 1px; border-style: solid;'>{codigoCtaf.CODIGO}</td><td style='border-width: 1px; border-style: solid;'>{codigoCtaf.FECHA}</td></tr>");
            }

            if (codigosElaboracionTecnico.Count != 0)
            {
                table = string.Concat(table, "<tr><td colspan='2' style='border-width: 1px; border-style: solid;'>DECISIÓN E INFORME TÉCNICO</td></tr>");
            }

            foreach (var codigoElaboracionTecnico in codigosElaboracionTecnico)
            {
                table = string.Concat(table, $"<tr><td style='border-width: 1px; border-style: solid;'>{codigoElaboracionTecnico.CODIGO}</td><td style='border-width: 1px; border-style: solid;'>{codigoElaboracionTecnico.FECHA}</td></tr>");
            }

            table = string.Concat(table, "</tbody></table>");

            string footer = GetFooterCorreoAlertas();

            string htmlContent = string.Concat(oPlantilla.Plantcontenido, table, footer);

            Base.Tools.Util.SendEmail(
                lsCorreos,
                lsCorreoscc,
                lsCorreosBcc,
                oPlantilla.Plantasunto,
                htmlContent,
                oPlantilla.PlanticorreoFrom
            );
        }

        /// <summary>
        /// Permite realizar el envio de correos con archivos adjuntos
        /// </summary>
        /// <param name="From"></param>
        /// <param name="To"></param>
        /// <param name="CC"></param>
        /// <param name="BCC"></param>
        /// <param name="Asunto"></param>
        /// <param name="Contenido"></param>
        /// <param name="Plantcodi"></param>
        /// <param name="path"></param>
        /// <param name="files"></param>
        public void EnviarCorreo(string From, string To, string CC, string BCC, string Asunto, string Contenido, int? Plantcodi, string path, List<string> files)
        {
            if (From == null) From = "";
            if (To == null) To = "";
            if (CC == null) CC = "";
            if (BCC == null) BCC = "";

            var CorreoTo = To.Split(';').ToList();
            var CorreoCC = CC.Split(';').ToList();
            var CorreoBCC = BCC.Split(';').ToList();

            List<string> listFiles = new List<string>();
            foreach (string file in files)
            {
                if (!string.IsNullOrEmpty(file))
                {
                    if (!listFiles.Contains(path + file))
                        listFiles.Add(path + file);
                }
            }

            Base.Tools.Util.SendEmail(CorreoTo, CorreoCC, CorreoBCC, Asunto, Contenido, From, listFiles);

            var oCorreo = new SiCorreoDTO
            {
                Corrasunto = Asunto,
                Corrcontenido = Contenido,
                Corrfechaenvio = DateTime.Now,
                Corrto = To,
                Corrcc = CC,
                Corrbcc = BCC,
                Corrfrom = From,
                Plantcodi = Plantcodi
            };

            FactorySic.GetSiCorreoRepository().Save(oCorreo);
        }

        /// <summary>
        /// Footer de los correos
        /// </summary>
        /// <returns></returns>
        public static string GetFooterCorreo()
        {
            return CorreoAppServicio.GetFooterCorreoRemitente(string.Empty, string.Empty);
        }

        /// <summary>
        /// Footer de los correos alertas
        /// </summary>
        /// <returns></returns>
        public static string GetFooterCorreoAlertas()
        {
            return CorreoAppServicio.GetFooterAlertasCorreoRemitente(string.Empty, string.Empty);
        }

        /// <summary>
        /// Footer de los correos del CTAF
        /// </summary>
        /// <returns></returns>
        public static string GetFooterCorreoCTAF()
        {
            return CorreoAppServicio.GetFooterCorreoRemitenteCTAF(string.Empty, string.Empty);
        }

        /// <summary>
        /// Footer con remitente y anexo
        /// </summary>
        /// <param name="remitente"></param>
        /// <param name="anexo"></param>
        /// <returns></returns>
        public static string GetFooterCorreoRemitente(string remitente, string anexo)
        {
            /*str.Append("Atentamente,<br><br>");
            str.Append("<b>COES SINAC</b><br>");
            str.Append("Calle Manuel Roaud y Paz Soldan 364. San Isidro, Lima - Perú. Teléfono: (511) 611-8585");
            */

            string html = string.Format(@"
                <div>
                   <p class='MsoNormal'><u></u>&nbsp;<u></u></p>
                   <p class='MsoNormal'>Atentamente,<u></u><u></u></p>
                   <p class='MsoNormal'>
		                <img width='74' height='58' style='width:.7708in;height:.6041in' src='{0}' alt='Logotipo' class='CToWUd'><u></u><u></u></p>
	  
                   <p class='MsoNormal'><span style='font-size:11.0pt;font-weight: bold;'>{1}
                      <u></u><u></u></span>
                   </p>
   
                   <p class='MsoNormal'><span style='font-size:8.0pt'><u></u><u></u></span></p>
   
                   <p class='MsoNormal'><b><span style='font-size:8.0pt;color:#0077a5'>D:</span></b><span style='font-size:8.0pt'> Av. Los Conquistadores N° 1144, San Isidro, Lima - Perú
                      <u></u><u></u></span>
                   </p>
   
                   <p class='MsoNormal'><b><span style='font-size:8.0pt;color:#0077a5'>T:</span></b><span style='font-size:8.0pt;color:#0077a5'>
                      </span><span style='font-size:8.0pt'>+51 611 8585 {2}<u></u><u></u></span>
                   </p>
   
                   <p class='MsoNormal'><b><span style='font-size:8.0pt;color:#0077a5'>W:</span></b><span style='font-size:8.0pt;color:#0077a5'>
                      </span><a href='http://www.coes.org.pe' target='_blank'><span style='font-size:8.0pt;color:#0563c1'>www.coes.org.pe</span></a><span style='font-size:8.0pt'>
                      <u></u><u></u></span>
                   </p>
                </div>
            ", ConstantesAppServicio.LogoCoesEmail, remitente, anexo);

            return html;
        }

        /// <summary>
        /// Obtener la lista de correos para los ambientes de Prueba y Producción
        /// </summary>
        /// <param name="sCorreos"></param>
        /// <param name="flagIncluirCorreoConsultorBCC"></param>
        /// <param name="flagIncluirWebappBCC"></param>
        /// <returns></returns>
        public static List<string> ListarCorreosValidoSegunAmbiente(string sCorreos, bool flagIncluirCorreoConsultorBCC = false, bool flagIncluirWebappBCC = false)
        {
            List<string> listaCorreoFinal = new List<string>();

            //web.config
            string valorKeyFlagEnviarNotificacionACOES = ConfigurationManager.AppSettings[NotificacionAplicativo.KeyNotificacionFlagEnviarACOES];
            string valorKeyFlagEnviarNotificacionAAgente = ConfigurationManager.AppSettings[NotificacionAplicativo.KeyNotificacionFlagEnviarAAgente];
            string valorKeyFlagEnviarNotificacionAConsultor = ConfigurationManager.AppSettings[NotificacionAplicativo.KeyNotificacionFlagEnviarCCAdicional];
            string valorKeyListaEmailConsultor = ConfigurationManager.AppSettings[NotificacionAplicativo.KeyNotificacionListaEmailCCAdicional];
            string valorKeyMailFrom = ConfigurationManager.AppSettings[NotificacionAplicativo.KeyMailFrom];

            //input correos 
            List<string> listaCorreo = ListarCorreoValido(sCorreos);
            if (flagIncluirWebappBCC && !string.IsNullOrEmpty(valorKeyMailFrom))
            {
                listaCorreo.Add(valorKeyMailFrom); //en la copia oculta siempre debe estar webapp@coes.org.pe
            }

            //diferenciar correos
            List<string> listaCorreoAgente = listaCorreo.Where(x => !x.ToLower().Contains("@coes.org.pe")).ToList();
            List<string> listaCorreoCOES = listaCorreo.Where(x => x.ToLower().Contains("@coes.org.pe")).ToList();
            List<string> listaCorreoConsultor = ListarCorreoValido(valorKeyListaEmailConsultor);

            if (valorKeyFlagEnviarNotificacionACOES != "S") //si el valor es N o no existe el key en el web.config 
            {
                //modificar correos de COES para pruebas / preprod para agregarles un prefijo y sufijo
                //listaCorreoCOES = listaCorreoCOES.Select(x => "t1234_" + x + "_4321t").ToList();
                listaCorreoCOES = listaCorreoCOES.Select(x => "t1234_" + x ).ToList();
            }
            if (valorKeyFlagEnviarNotificacionAAgente != "S") //si el valor es N o no existe el key en el web.config 
            {
                //modificar correos de agentes para pruebas / preprod para agregarles un prefijo y sufijo
                //listaCorreoAgente = listaCorreoAgente.Select(x => "t1234_" + x + "_4321t").ToList();
                listaCorreoAgente = listaCorreoAgente.Select(x => "t1234_" + x ).ToList();
            }

            //Obtener listado final
            listaCorreoFinal.AddRange(listaCorreoAgente);
            listaCorreoFinal.AddRange(listaCorreoCOES);
            if (flagIncluirCorreoConsultorBCC && valorKeyFlagEnviarNotificacionAConsultor == "S")
            {
                listaCorreoFinal.AddRange(listaCorreoConsultor);
            }

            return listaCorreoFinal.Distinct().ToList();
        }

        /// <summary>
        /// Texto del asunto segun ambientes de Prueba y Producción
        /// </summary>
        /// <param name="asunto"></param>
        /// <returns></returns>
        public static string GetTextoAsuntoSegunAmbiente(string asunto)
        {
            string valorKeyPrefijoAsuntoPrueba = ConfigurationManager.AppSettings[NotificacionAplicativo.KeyNotificacionPrefijoAsunto];

            return string.IsNullOrEmpty(valorKeyPrefijoAsuntoPrueba) ? asunto : valorKeyPrefijoAsuntoPrueba + asunto;
        }

        private static List<string> ListarCorreoValido(string sCorreos)
        {
            return (sCorreos ?? "").ToLower().Split(';').Select(x => x = x.Trim()).Where(x => x != "").Distinct().ToList();
        }

        public static string GetTextoSinVariable(string texto, Dictionary<string, string> mapa)
        {
            texto = (texto ?? "").Trim();
            foreach (KeyValuePair<string, string> entry in mapa)
            {
                texto = texto.Replace(entry.Key, entry.Value);
            }

            return texto;
        }

        /// <summary>
        /// Footer con remitente y anexo
        /// </summary>
        /// <param name="remitente"></param>
        /// <param name="anexo"></param>
        /// <returns></returns>
        public static string GetFooterAlertasCorreoRemitente(string remitente, string anexo)
        {
            string html = string.Format(@"
                <div>
                   <p class='MsoNormal'><u></u>&nbsp;<u></u></p>
                   <p class='MsoNormal'><strong>Saludos,</strong><u></u><u></u></p>
                   <div class='MsoNormal'><img width='74' height='58' style='width:.7708in;height:.6041in' src='{0}' alt='Logotipo' class='CToWUd'><u></u><u></u></div>
                   <div class='MsoNormal'><span style='font-size:12px'><b>Comité Técnico de Análisis de Fallas</b></span></div>
                   <div class='MsoNormal'><span style='font-size:12px'>Sub Dirección de Evalación</span></div>
                   <div style='width: 30px; padding: 1px; background: #2fa4d4'></div>
                   <div class='MsoNormal'><b><span style='font-size:8.0pt;color:#0077a5'>D:</span></b><span style='font-size:8.0pt'> Av. Los Conquistadores N° 1144, San Isidro, Lima - Perú</span></div>
                   <div class='MsoNormal'><b><span style='font-size:8.0pt;color:#0077a5'>T:</span></b><span style='font-size:8.0pt;color:#0077a5'></span><span style='font-size:8.0pt'>+51 611 8585 {2}</span></div>
                </div>
            ", ConstantesAppServicio.LogoCoesEmail, remitente, anexo);

            return html;
        }

        /// <summary>
        /// Footer con remitente y anexo
        /// </summary>
        /// <param name="remitente"></param>
        /// <param name="anexo"></param>
        /// <returns></returns>
        public static string GetFooterCorreoRemitenteCTAF(string remitente, string anexo)
        {
            /*str.Append("Atentamente,<br><br>");
            str.Append("<b>COES SINAC</b><br>");
            str.Append("Calle Manuel Roaud y Paz Soldan 364. San Isidro, Lima - Perú. Teléfono: (511) 611-8585");
            */

            string html = string.Format(@"
                <div>
                   <p class='MsoNormal'>
		                <img width='74' height='58' style='width:.7708in;height:.6041in' src='{0}' alt='Logotipo' class='CToWUd'><u></u><u></u></p>
  
                   <p  style='font-weight: bold;font-size:12.0pt;margin: 0 auto;text-indent: 0cm;'>Comité Técnico de Análisis de Fallas</p>
                   <p  style='font-weight: bold;font-size:8.0pt;margin: 0 auto;text-indent: 0cm;'>Sub Dirección de Evaluación</p>  
                   <p  style='margin: 0 auto;text-indent: 0cm;'><b><span style='font-size:8.0pt;color:#0077a5'>D:</span></b><span style='font-size:8.0pt'> Av. Los Conquistadores N° 1144, San Isidro, Lima - Perú</span></p>
                   <p  style='margin: 0 auto;text-indent: 0cm;'><b><span style='font-size:8.0pt;color:#0077a5'>T:</span></b><span style='font-size:8.0pt;color:#0077a5'>
                      </span><span style='font-size:8.0pt'>+51 611 8585 {2}</span>
                   </p>
                   <p  style='margin: 0 auto;text-indent: 0cm;'><b><span style='font-size:8.0pt;color:#0077a5'>W:</span></b><span style='font-size:8.0pt;color:#0077a5'>
                      </span><a href='http://www.coes.org.pe' target='_blank'><span style='font-size:8.0pt;color:#0563c1'>www.coes.org.pe</span></a><span style='font-size:8.0pt'></span>
                   </p>
                </div>
            ", ConstantesAppServicio.LogoCoesEmail, remitente, anexo);

            return html;
        }

        /// <summary>
        /// Envío de correo alerta Desviacion Frecuencias
        /// </summary>
        public void EnviarCorreoAlertaDesviacionFrecuencias(int codigoCorreo, List<InformacionFrecuenciaDTO> entities)
        {
            //Generar Tupla de Variable y valor
            var mapaVariable = new Dictionary<string, string>();
            mapaVariable[ConstantesIntervencionesAppServicio.VariableFechaReporte] = DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy");


            var oPlantilla = FactorySic.GetSiPlantillacorreoRepository().GetById(codigoCorreo);

            var lsCorreos = oPlantilla.Planticorreos.Split(';').ToList();
            var lsCorreoscc = oPlantilla.PlanticorreosCc.Split(';').ToList();
            var lsCorreosBcc = oPlantilla.PlanticorreosBcc.Split(';').ToList();

            string htmlContent = CorreoAppServicio.GetTextoSinVariable(oPlantilla.Plantcontenido, mapaVariable);

            string htmlReporte = @"
                <style>
table, th, td {
  border: 1px solid black;
  border-collapse: collapse;
}
                </style>
                <table cellpadding='0' cellspacing='0'>
                <thead>
                    <tr>
                        <th>#</th>
                        <th>Fecha Hora</th>
                        <th>Equipo GPS</th>
                        <th>Frecuencia</ th >
                        <th>Equipo GPS - Comp.</th>
                        <th>Frecuencia Comp.</th>
                        <th>Dif.</th>
                    </tr>
                </thead>
            ";

            if (entities.Count > 0)
            {
                int contador = 1;
                foreach (var item in entities)
                {
                    htmlReporte = htmlReporte + "<tr><td>" + contador.ToString() + "</td><td>" + item.FechaHora.ToString() + "</td><td>" + item.GPSNombre.ToString() + "</td><td>" + item.Frecuencia.ToString() + "</td><td>" + item.GPSNombreComparar.ToString() + "</td><td>" + item.FrecuenciaComparar.ToString() + "</td><td>" + item.FrecuenciaDiferencia.ToString("#.##0") + "</td></tr>";
                    contador++;
                }
            }
            htmlReporte = htmlReporte + "</table>";

            mapaVariable[ConstantesIntervencionesAppServicio.VariableReporte] = htmlReporte.ToString();
            htmlContent = CorreoAppServicio.GetTextoSinVariable(htmlContent, mapaVariable);

            string footer = GetFooterCorreoAlertas();

            htmlContent = string.Concat(htmlContent, footer);

            string asunto = CorreoAppServicio.GetTextoSinVariable(oPlantilla.Plantasunto, mapaVariable);

            Base.Tools.Util.SendEmail(
                lsCorreos,
                lsCorreoscc,
                lsCorreosBcc,
                asunto,
                htmlContent,
                oPlantilla.PlanticorreoFrom
            );
        }

        /// <summary>
        /// Envío de correo alerta Eventos Frecuencias
        /// </summary>
        public void EnviarCorreoAlertaReporteSegFaltantes(int codigoCorreo, List<EquipoGPSDTO> listaGPS, List<ReporteSegundosFaltantesDTO> listaReporte)
        {
            //Generar Tupla de Variable y valor
            var mapaVariable = new Dictionary<string, string>();
            mapaVariable[ConstantesIntervencionesAppServicio.VariableFechaReporte] = DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy");

            var oPlantilla = FactorySic.GetSiPlantillacorreoRepository().GetById(codigoCorreo);

            var lsCorreos = oPlantilla.Planticorreos.Split(';').ToList();
            var lsCorreoscc = oPlantilla.PlanticorreosCc.Split(';').ToList();
            var lsCorreosBcc = oPlantilla.PlanticorreosBcc.Split(';').ToList();

            string htmlContent = CorreoAppServicio.GetTextoSinVariable(oPlantilla.Plantcontenido, mapaVariable);

            string htmlReporte = @"
                <style>
table, th, td {
  border: 1px solid black;
  border-collapse: collapse;
}
                </style>
                <table cellpadding='0' cellspacing='0'>
                <thead>
                    <tr>
                        <th>#</th>
                        <th>Equipo GPS</th>
                        <th>Total</ th >
                    </tr>
                </thead>
            ";

            if (listaGPS.Count > 0)
            {
                int contador = 1;
                foreach (var item in listaGPS)
                {
                    var auxEquipo = listaReporte.Where(x => x.GPSCodi == item.GPSCodi).ToList();
                    var difSeg = 0;
                    if (auxEquipo.Count > 0)
                    {
                        var elemento = auxEquipo.First();
                        difSeg = 86400 - elemento.Num;
                    }
                    else
                    {
                        difSeg = 86400;
                    }

                    htmlReporte = htmlReporte + "<tr><td>" + contador.ToString() + "</td><td>" + item.NombreEquipo.ToString() + "</td><td>" + difSeg.ToString() + "</td></tr>";
                    contador++;
                }
            }
            htmlReporte = htmlReporte + "</table>";

            mapaVariable[ConstantesIntervencionesAppServicio.VariableReporte] = htmlReporte.ToString();
            htmlContent = CorreoAppServicio.GetTextoSinVariable(htmlContent, mapaVariable);

            string footer = GetFooterCorreoAlertas();

            htmlContent = string.Concat(htmlContent, footer);

            string asunto = CorreoAppServicio.GetTextoSinVariable(oPlantilla.Plantasunto, mapaVariable);

            Base.Tools.Util.SendEmail(
                lsCorreos,
                lsCorreoscc,
                lsCorreosBcc,
                asunto,
                htmlContent,
                oPlantilla.PlanticorreoFrom
            );
        }

        /// <summary>
        /// Envío de correo alerta Reporte Segundos Faltantes
        /// </summary>
        public void EnviarCorreoAlertaEventosFrecuencias(int codigoCorreo, List<InformacionFrecuenciaDTO> entities)
        {
            //Generar Tupla de Variable y valor
            var mapaVariable = new Dictionary<string, string>();
            mapaVariable[ConstantesIntervencionesAppServicio.VariableFechaReporte] = DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy");

            var oPlantilla = FactorySic.GetSiPlantillacorreoRepository().GetById(codigoCorreo);

            var lsCorreos = oPlantilla.Planticorreos.Split(';').ToList();
            var lsCorreoscc = oPlantilla.PlanticorreosCc.Split(';').ToList();
            var lsCorreosBcc = oPlantilla.PlanticorreosBcc.Split(';').ToList();

            string htmlContent = CorreoAppServicio.GetTextoSinVariable(oPlantilla.Plantcontenido, mapaVariable);

            string htmlReporte = @"
                <style>
table, th, td {
  border: 1px solid black;
  border-collapse: collapse;
}
                </style>
                <table cellpadding='0' cellspacing='0'>
                <thead>
                    <tr>
                        <th>#</th>
                        <th>Fecha Hora</th>
                        <th>Equipo GPS</th>
                        <th>Frecuencia</ th >
                    </tr>
                </thead>
            ";

            if (entities.Count > 0)
            {
                int contador = 1;
                foreach (var item in entities)
                {
                    htmlReporte = htmlReporte + "<tr><td>" + contador.ToString() + "</td><td>" + item.FechaHora.ToString() + "</td><td>" + item.GPSNombre.ToString() + "</td><td>" + item.Frecuencia.ToString() + "</td></tr>";
                    contador++;
                }
            }
            htmlReporte = htmlReporte + "</table>";

            mapaVariable[ConstantesIntervencionesAppServicio.VariableReporte] = htmlReporte.ToString();
            htmlContent = CorreoAppServicio.GetTextoSinVariable(htmlContent, mapaVariable);

            string footer = GetFooterCorreoAlertas();

            htmlContent = string.Concat(htmlContent, footer);

            string asunto = CorreoAppServicio.GetTextoSinVariable(oPlantilla.Plantasunto, mapaVariable);

            Base.Tools.Util.SendEmail(
                lsCorreos,
                lsCorreoscc,
                lsCorreosBcc,
                asunto,
                htmlContent,
                oPlantilla.PlanticorreoFrom
            );
        }


    }
}
