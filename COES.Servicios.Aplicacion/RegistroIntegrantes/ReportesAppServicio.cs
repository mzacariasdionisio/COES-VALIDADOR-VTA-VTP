using System;
using System.Linq;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using COES.Servicios.Aplicacion.Factory;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Configuration;
using System.Globalization;

namespace COES.Servicios.Aplicacion.RegistroIntegrantes
{
    public class ReportesAppServicio : AppServicioBase
    {

        /// <summary>
        /// Devuelve un listado de empresas segun el tipo
        /// </summary>
        /// <param name="tipoempresa">Tipo de empresa</param>
        /// <param name="nroPage">nro de pagina</param>
        /// <param name="pageSize">tamaño de pagina</param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarIntegrantes(string tipoempresa,
            string nombre,
            int Page,
            int PageSize)
        {
            List<SiEmpresaDTO> lista = new List<SiEmpresaDTO>();

            lista = FactorySic.GetSiEmpresaRIRepository().ListarIntegrantesporTipo(tipoempresa, nombre, Page, PageSize);

            return lista;
        }

        /// <summary>
        /// Devuelve total registros de empresas segun el tipo
        /// </summary>
        /// <param name="tipoempresa">Tipo de empresa</param>
        /// <returns>Numero de registros</returns>
        public int ObtenerTotalRegListarIntegrantes(string tipoempresa, string nombre)
        {
            return FactorySic.GetSiEmpresaRIRepository().ObtenerTotalListarIntegrantesporTipo(tipoempresa, nombre);
        }

        /// <summary>
        /// Devuelve un listado de representantes segun criterio
        /// </summary>
        /// <param name="tipoempresa">Tipo de empresa</param>
        /// <param name="nombre">nombre</param>
        /// <param name="tiporepresentante">Tipo representante</param>
        /// <param name="tiporepresentantecontacto">Tipo representante, contacto</param>
        /// <param name="estado">estado</param>
        /// <param name="fecha">Fecha Vigencia Poder</param>
        /// <param name="nroPage">nro de pagina</param>
        /// <param name="pageSize">tamaño de pagina</param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarRepresentanteLegal(string tipoempresa,
            string nombre,
            string tiporepresentante,
            string tiporepresentantecontacto,
            string estado,
            DateTime fecha,
            int Page,
            int PageSize)
        {
            List<SiEmpresaDTO> lista = new List<SiEmpresaDTO>();

            lista = FactorySic.GetSiEmpresaRIRepository().ListarporTipoNombreRepresentante(tipoempresa, nombre, tiporepresentante, tiporepresentantecontacto, estado, fecha, Page, PageSize);

            return lista;
        }

        /// <summary>
        /// Devuelve total registros de empresas segun el tipo
        /// </summary>
        /// <param name="tipoempresa">Tipo de empresa</param>
        /// <param name="nombre">nombre</param>
        /// <param name="tiporepresentante">Tipo representante</param>
        /// <param name="tiporepresentantecontacto">Tipo representantecontacto</param>
        /// <param name="estado">Estado</param>
        /// <param name="fecha">Fecha Vigencia Poder</param>
        /// <returns>Numero de registros</returns>
        public int ObtenerTotalRegListarRepresentanteLegal(string tipoempresa,
            string nombre,
            string tiporepresentante,
            string tiporepresentantecontacto,
            string estado,
            DateTime fecha)
        {
            return FactorySic.GetSiEmpresaRIRepository().ObtenerTotalListarporTipoNombreRepresentante(tipoempresa, nombre, tiporepresentante, tiporepresentantecontacto, estado, fecha);
        }

        /// <summary>
        /// Devuelve un listado de empresas segun el tipo
        /// </summary>
        /// <param name="fechaInicio">fecha inicio</param>
        /// <param name="fechaFin">fecha fin</param>
        /// <param name="tipoempresa">Tipo de empresa</param>
        /// <param name="nombre">nombre</param>
        /// <param name="tipomodalidad">Tipo modalidad</param>
        /// <param name="nroPage">nro de pagina</param>
        /// <param name="pageSize">tamaño de pagina</param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarEmpresas(DateTime fechaInicio,
            DateTime fechaFin,
            string tipoempresa,
            string nombre,
            string tipomodalidad,
            int Page,
            int PageSize)
        {
            List<SiEmpresaDTO> lista = new List<SiEmpresaDTO>();

            lista = FactorySic.GetSiEmpresaRIRepository().ListarEmpresas(fechaInicio,
                fechaFin,
                tipoempresa,
                nombre,
                tipomodalidad,
                Page,
                PageSize);

            return lista;
        }

        /// <summary>
        /// Devuelve total registros de empresas segun el tipo
        /// </summary>
        /// <param name="fechaInicio">fecha inicio</param>
        /// <param name="fechaFin">fecha fin</param>
        /// <param name="tipoempresa">Tipo de empresa</param>
        /// <param name="nombre">nombre</param>
        /// <param name="tipomodalidad">Tipo modalidad</param>
        /// <returns>Numero de registros</returns>
        public int ObtenerTotalRegListarEmpresas(DateTime fechaInicio,
            DateTime fechaFin,
            string tipoempresa,
            string nombre,
            string tipomodalidad)
        {
            return FactorySic.GetSiEmpresaRIRepository().ObtenerTotalRegListarEmpresas(fechaInicio,
                fechaFin,
                tipoempresa,
                nombre,
                tipomodalidad);
        }

        /// <summary>
        /// Devuelve total registros para exportar a Excel
        /// </summary>
        /// <param name="fechaInicio">fecha inicio</param>
        /// <param name="fechaFin">fecha fin</param>
        /// <param name="tipoempresa">Tipo de empresa</param>
        /// <param name="nombre">nombre</param>
        /// <param name="tipomodalidad">Tipo modalidad</param>
        public List<SiEmpresaDTO> ListarEmpresasFiltroXls(DateTime fechaInicio,
            DateTime fechaFin,
            string tipoempresa,
            string nombre,
            string tipomodalidad)
        {
            List<SiEmpresaDTO> lista = new List<SiEmpresaDTO>();

            lista = FactorySic.GetSiEmpresaRIRepository().ListarEmpresasFiltroXls(fechaInicio,
                fechaFin,
                tipoempresa,
                nombre,
                tipomodalidad);

            return lista;
        }

        /// <summary>
        /// Devuelve un listado de empresas segun el tipo
        /// </summary>
        /// <param name="fechaInicio">fecha inicio</param>
        /// <param name="fechaFin">fecha fin</param>
        /// <param name="tipoempresa">Tipo de empresa</param>
        /// <param name="tiposolicitud">Tipo solicitud</param>
        /// <param name="nroPage">nro de pagina</param>
        /// <param name="pageSize">tamaño de pagina</param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarEvolucionEmpresas(DateTime fechaInicio,
            DateTime fechaFin,
            string tipoempresa,
            string tiposolicitud,
            int Page,
            int PageSize)
        {
            List<SiEmpresaDTO> lista = new List<SiEmpresaDTO>();

            lista = FactorySic.GetSiEmpresaRIRepository().ListarEvolucionEmpresas(fechaInicio,
                fechaFin,
                tipoempresa,
                tiposolicitud,
                Page,
                PageSize);

            return lista;
        }

        /// <summary>
        /// Devuelve total registros de empresas segun el tipo
        /// </summary>
        /// <param name="fechaInicio">fecha inicio</param>
        /// <param name="fechaFin">fecha fin</param>
        /// <param name="tipoempresa">Tipo de empresa</param>
        /// <param name="tiposolicitud">Tipo solicitud</param>
        /// <returns>Numero de registros</returns>
        public int ObtenerTotalRegListarEvolucionEmpresas(DateTime fechaInicio,
            DateTime fechaFin,
            string tipoempresa,
            string tiposolicitud)
        {
            return FactorySic.GetSiEmpresaRIRepository().ObtenerTotalRegListarEvolucionEmpresas(fechaInicio,
                fechaFin,
                tipoempresa,
                tiposolicitud);
        }

        /// <summary>
        /// Devuelve la lista de empresas para exportar a Excel
        /// </summary>
        /// <param name="fechaInicio">fecha inicio</param>
        /// <param name="fechaFin">fecha fin</param>
        /// <param name="tipoempresa">Tipo de empresa</param>
        /// <param name="tiposolicitud">Tipo solicitud</param>
        /// <returns>Numero de registros</returns>
        public List<SiEmpresaDTO> ListarEvolucionEmpresasFiltroXls(DateTime fechaInicio,
            DateTime fechaFin,
            string tipoempresa,
            string tiposolicitud)
        {
            List<SiEmpresaDTO> lista = new List<SiEmpresaDTO>();

            lista = FactorySic.GetSiEmpresaRIRepository().ListarEvolucionEmpresasFiltroXls(fechaInicio,
                fechaFin,
                tipoempresa,
                tiposolicitud);

            return lista;
        }


        /// <summary>
        /// Devuelve un listado de representantes segun criterio
        /// </summary>
        /// <param name="tipoempresa">Tipo de empresa</param>
        /// <param name="nombre">nombre</param>
        /// <param name="tiporepresentante">Tipo representante</param>
        /// <param name="tiporepresentantecontacto">Tipo representante, contacto</param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarRepresentanteLegalFiltroXls(string tipoempresa,
            string nombre,
            string tiporepresentante,
            string tiporepresentantecontacto,
            string estado,
            DateTime fecha)
        {
            List<SiEmpresaDTO> lista = new List<SiEmpresaDTO>();

            lista = FactorySic.GetSiEmpresaRIRepository().ListarporTipoNombreRepresentanteFiltroXls(tipoempresa, nombre, tiporepresentante, tiporepresentantecontacto, estado, fecha);

            return lista;
        }

        #region Reporte Historico de solicitudes

        /// <summary>
        /// Devuelve un listado de historico de solicitudes
        /// </summary>
        /// <param name="fechaInicio">fecha inicio</param>
        /// <param name="fechaFin">fecha fin</param>
        /// <param name="tipoempresa">Tipo de empresa</param>
        /// <param name="tiposolicitud">Tipo solicitud</param>
        /// <param name="nroPage">nro de pagina</param>
        /// <param name="pageSize">tamaño de pagina</param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarHistoricoSolicitudes(DateTime fechaInicio,
            DateTime fechaFin,
            string tipoempresa,
            string tiposolicitud,
            string empresa,
            int Page,
            int PageSize)
        {
            List<SiEmpresaDTO> lista = new List<SiEmpresaDTO>();

            lista = FactorySic.GetSiEmpresaRIRepository().ListarHistoricoSolicitudes(fechaInicio,
                fechaFin,
                tipoempresa,
                tiposolicitud,
                empresa,
                Page,
                PageSize);

            return lista;
        }

        /// <summary>
        /// Devuelve total registros de historico de solicitudes
        /// </summary>
        /// <param name="fechaInicio">fecha inicio</param>
        /// <param name="fechaFin">fecha fin</param>
        /// <param name="tipoempresa">Tipo de empresa</param>
        /// <param name="tiposolicitud">Tipo solicitud</param>
        /// <returns>Numero de registros</returns>
        public int ObtenerTotalRegListarHistoricoSolicitudes(DateTime fechaInicio,
            DateTime fechaFin,
            string tipoempresa,
            string tiposolicitud,
            string empresa)
        {
            return FactorySic.GetSiEmpresaRIRepository().ObtenerTotalRegListarHistoricoSolicitudes(fechaInicio,
                fechaFin,
                tipoempresa,
                tiposolicitud,
                empresa);
        }

        /// <summary>
        /// Devuelve la lista de historico de solicitudes para exportar a Excel
        /// </summary>
        /// <param name="fechaInicio">fecha inicio</param>
        /// <param name="fechaFin">fecha fin</param>
        /// <param name="tipoempresa">Tipo de empresa</param>
        /// <param name="tiposolicitud">Tipo solicitud</param>
        /// <returns>Numero de registros</returns>
        public List<SiEmpresaDTO> ListarHistoricoSolicitudesFiltroXls(DateTime fechaInicio,
            DateTime fechaFin,
            string tipoempresa,
            string tiposolicitud,
            string empresa)
        {
            List<SiEmpresaDTO> lista = new List<SiEmpresaDTO>();

            lista = FactorySic.GetSiEmpresaRIRepository().ListarHistoricoSolicitudesFiltroXls(fechaInicio,
                fechaFin,
                tipoempresa,
                tiposolicitud,
                empresa);

            return lista;
        }

        #endregion

        #region Reporte Historico de revisiones

        /// <summary>
        /// Devuelve un listado de historico de revisiones
        /// </summary>
        /// <param name="fechaInicio">fecha inicio</param>
        /// <param name="fechaFin">fecha fin</param>
        /// <param name="tipoempresa">Tipo de empresa</param>
        /// <param name="tiporevision">Tipo revision</param>
        /// <param name="empresa">empresa</param>
        /// <param name="nroPage">nro de pagina</param>
        /// <param name="pageSize">tamaño de pagina</param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarHistoricoRevisiones(DateTime fechaInicio,
            DateTime fechaFin,
            string tipoempresa,
            string tiporevision,
            string empresa,
            int Page,
            int PageSize)
        {
            List<SiEmpresaDTO> lista = new List<SiEmpresaDTO>();

            lista = FactorySic.GetSiEmpresaRIRepository().ListarHistoricoRevisiones(fechaInicio,
                fechaFin,
                tipoempresa,
                tiporevision,
                empresa,
                Page,
                PageSize);

            return lista;
        }

        /// <summary>
        /// Devuelve total registros de historico de revisiones
        /// </summary>
        /// <param name="fechaInicio">fecha inicio</param>
        /// <param name="fechaFin">fecha fin</param>
        /// <param name="tipoempresa">Tipo de empresa</param>
        /// <param name="tiporevision">Tipo revision</param>
        /// <param name="empresa">Empresa</param>
        /// <returns>Numero de registros</returns>
        public int ObtenerTotalRegListarHistoricoRevisiones(DateTime fechaInicio,
        DateTime fechaFin,
        string tipoempresa,
        string tiporevision,
        string empresa)
        {
            return FactorySic.GetSiEmpresaRIRepository().ObtenerTotalRegListarHistoricoRevisiones(fechaInicio,
                fechaFin,
                tipoempresa,
                tiporevision,
                empresa);
        }

        /// <summary>
        /// Devuelve la lista de historico de revisiones para exportar a Excel
        /// </summary>
        /// <param name="fechaInicio">fecha inicio</param>
        /// <param name="fechaFin">fecha fin</param>
        /// <param name="tipoempresa">Tipo de empresa</param>
        /// <param name="tiporevision">Tipo revision</param>
        /// <param name="empresa">Empresa</param>
        /// <returns>Numero de registros</returns>
        public List<SiEmpresaDTO> ListarHistoricoRevisionesFiltroXls(DateTime fechaInicio,
        DateTime fechaFin,
        string tipoempresa,
        string tiporevision,
        string empresa)
        {
            List<SiEmpresaDTO> lista = new List<SiEmpresaDTO>();

            lista = FactorySic.GetSiEmpresaRIRepository().ListarHistoricoRevisionesFiltroXls(fechaInicio,
                fechaFin,
                tipoempresa,
                tiporevision,
                empresa);

            return lista;
        }



        #endregion

        #region Reporte Historico de Modificaciones

        /// <summary>
        /// Devuelve un listado de historico de modificaciones
        /// </summary>
        /// <param name="fechaInicio">fecha inicio</param>
        /// <param name="fechaFin">fecha fin</param>
        /// <param name="tipoempresa">Tipo de empresa</param>
        /// <param name="tiposolicitud">Tipo solicitud</param>
        /// <param name="empresa">empresa</param>
        /// <param name="nroPage">nro de pagina</param>
        /// <param name="pageSize">tamaño de pagina</param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarHistoricoModificaciones(DateTime fechaInicio,
            DateTime fechaFin,
            string tipoempresa,
            string tiposolicitud,
            string empresa,
            int Page,
            int PageSize)
        {
            List<SiEmpresaDTO> lista = new List<SiEmpresaDTO>();

            lista = FactorySic.GetSiEmpresaRIRepository().ListarHistoricoModificaciones(fechaInicio,
                fechaFin,
                tipoempresa,
                tiposolicitud,
                empresa,
                Page,
                PageSize);

            return lista;
        }

        /// <summary>
        /// Devuelve total registros de historico de Modificaciones
        /// </summary>
        /// <param name="fechaInicio">fecha inicio</param>
        /// <param name="fechaFin">fecha fin</param>
        /// <param name="tipoempresa">Tipo de empresa</param>
        /// <param name="tiposolicitud">Tipo solicitud</param>
        /// <param name="empresa">empresa</param>
        /// <returns>Numero de registros</returns>
        public int ObtenerTotalRegListarHistoricoModificaciones(DateTime fechaInicio,
        DateTime fechaFin,
        string tipoempresa,
        string tiposolicitud,
        string empresa)
        {
            return FactorySic.GetSiEmpresaRIRepository().ObtenerTotalRegListarHistoricoModificaciones(fechaInicio,
                fechaFin,
                tipoempresa,
                tiposolicitud,
                empresa);
        }

        /// <summary>
        /// Devuelve la lista de historico de Modificaciones para exportar a Excel
        /// </summary>
        /// <param name="fechaInicio">fecha inicio</param>
        /// <param name="fechaFin">fecha fin</param>
        /// <param name="tipoempresa">Tipo de empresa</param>
        /// <param name="tiposolicitud">Tipo solicitud</param>
        /// <param name="empresa">empresa</param>
        /// <returns>Numero de registros</returns>
        public List<SiEmpresaDTO> ListarHistoricoModificacionesFiltroXls(DateTime fechaInicio,
        DateTime fechaFin,
        string tipoempresa,
        string tiposolicitud,
        string empresa)
        {
            List<SiEmpresaDTO> lista = new List<SiEmpresaDTO>();

            lista = FactorySic.GetSiEmpresaRIRepository().ListarHistoricoModificacionesFiltroXls(fechaInicio,
                fechaFin,
                tipoempresa,
                tiposolicitud,
                empresa);

            return lista;
        }



        #endregion

        #region Reporte Tiempos Proceso

        /// <summary>
        /// Devuelve un listado de los tiempos del proceso
        /// </summary>
        /// <param name="fechaInicio">fecha inicio</param>
        /// <param name="fechaFin">fecha fin</param>
        /// <param name="tipoempresa">Tipo de empresa</param>
        /// <param name="tiporevision">Tipo solicitud</param>
        /// <param name="nroPage">nro de pagina</param>
        /// <param name="pageSize">tamaño de pagina</param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarTiemposProceso(DateTime fechaInicio,
            DateTime fechaFin,
            string tipoempresa,
            string tiporevision,
            int Page,
            int PageSize)
        {
            List<SiEmpresaDTO> lista = new List<SiEmpresaDTO>();

            lista = FactorySic.GetSiEmpresaRIRepository().ListarTiempoProceso(fechaInicio,
                fechaFin,
                tipoempresa,
                tiporevision,
                Page,
                PageSize);

            return lista;
        }

        /// <summary>
        /// Devuelve total registros de los tiempos del proceso
        /// </summary>
        /// <param name="fechaInicio">fecha inicio</param>
        /// <param name="fechaFin">fecha fin</param>
        /// <param name="tipoempresa">Tipo de empresa</param>
        /// <param name="tiporevision">Tipo revision</param>
        /// <returns>Numero de registros</returns>
        public int ObtenerTotalRegListarTiemposProceso(DateTime fechaInicio,
            DateTime fechaFin,
            string tipoempresa,
            string tiporevision)
        {
            return FactorySic.GetSiEmpresaRIRepository().ObtenerTotalRegListarTiempoProceso(fechaInicio,
                fechaFin,
                tipoempresa,
                tiporevision);
        }

        /// <summary>
        /// Devuelve la lista de tiempos de proceso
        /// </summary>
        /// <param name="fechaInicio">fecha inicio</param>
        /// <param name="fechaFin">fecha fin</param>
        /// <param name="tipoempresa">Tipo de empresa</param>
        /// <param name="tiporevision">Tipo revision</param>
        /// <returns>Numero de registros</returns>
        public List<SiEmpresaDTO> ListarTiemposProcesoFiltroXls(DateTime fechaInicio,
            DateTime fechaFin,
            string tipoempresa,
            string tiporevision)
        {
            List<SiEmpresaDTO> lista = new List<SiEmpresaDTO>();

            lista = FactorySic.GetSiEmpresaRIRepository().ListarTiempoProcesoFiltroXls(fechaInicio,
                fechaFin,
                tipoempresa,
                tiporevision);

            return lista;
        }



        #endregion

        #region Funciones

        /// <summary>
        /// Exportar Constancia de reigstro de integrante en PDF
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public byte[] ExportarPDF(int id, out int nroConstancia)
        {
            byte[] bytes = new byte[0];

            //try
            //{

            RegistroIntegrantesAppServicio oServicio = new RegistroIntegrantesAppServicio();


            SiEmpresaDTO oEmpresa = oServicio.GetEmpresaByIdConRevision(id);

            nroConstancia = oEmpresa.Emprnroconstancia;

            var Representante = oServicio.GetRepresentantesByEmprcodi(id);
            var TipoComportamiento = oServicio.ListSiTipoComportamientoByEmprcodi(id);
            var RepresentanteLegal = Representante.FindAll(x => x.Rptetipo == ConstantesRegistroIntegrantes.RepresentanteTipoLegal);
            var PersonaContacto = Representante.Find(x => x.Rptetipo == ConstantesRegistroIntegrantes.RepresentanteTipoContacto);
            var ResponsableTramite = Representante.Find(x => x.Rptetipo == ConstantesRegistroIntegrantes.RepresentanteTipoResponsableTramite);
            var TipoComportamientoPrincipal = TipoComportamiento.Find(x => x.Tipoprincipal == ConstantesRegistroIntegrantes.TipoComportamientoPrincipalSi);

            bool existeTipoComportamiento = true;
            if (TipoComportamientoPrincipal == null)
                existeTipoComportamiento = false;

            var TituloEmpresa = ConfigurationManager.AppSettings["riTituloEmpresa"].ToString();
            var TituloTipoIntegrante = ConfigurationManager.AppSettings["riTituloTipoIntegrante"].ToString();
            var TituloRepresentanteLegal = ConfigurationManager.AppSettings["riTituloRepresentanteLegal"].ToString();
            var TituloPersonaContacto = ConfigurationManager.AppSettings["riTituloPersonaContacto"].ToString();
            var TituloPersonaResponsable = ConfigurationManager.AppSettings["riTituloPersonaResponsable"].ToString();

            using (MemoryStream ms = new MemoryStream())
            {
                Document document = new Document(iTextSharp.text.PageSize.A4, 50f, 50f, 50f, 50f);  // A4.Rotate PARA HORIZONTAL
                PdfWriter writer = PdfWriter.GetInstance(document, ms);

                document.Open();

                PdfContentByte pcb = writer.DirectContent;

                var Titulo = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 8, BaseColor.WHITE);
                var SubTitulo = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 8, 2);

                var Smaller = FontFactory.GetFont(FontFactory.HELVETICA, 6);
                var SmallFontBold = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 7);
                var SmallFont = FontFactory.GetFont(FontFactory.HELVETICA, 7);
                var SmallFontWhite = FontFactory.GetFont(FontFactory.HELVETICA, 7);
                var SmallFontBoldWhite = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 7);
                var RegularFontBold = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);

                var MediumFontBold = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14);
                var MediumFont = FontFactory.GetFont(FontFactory.HELVETICA, 9);
                var LargeFontBold = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 32);
                var LargeFont = FontFactory.GetFont(FontFactory.HELVETICA, 32);

                MediumFont.SetColor(0, 0, 0);


                //Tabla Principal
                PdfPTable tblContent = new PdfPTable(1);
                tblContent.WidthPercentage = 100;

                //Logo

                PdfPTable table = new PdfPTable(10);
                table.TotalWidth = 500f;
                table.LockedWidth = true;
                float[] widths = new float[] { 2f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f };
                table.SetWidths(widths);
                table.HorizontalAlignment = Element.ALIGN_LEFT;
                table.SpacingBefore = 5f;
                table.SpacingAfter = 0f;

                string pathLogo = ConfigurationManager.AppSettings["RutaLogo"].ToString() + "logocoes.png";
                iTextSharp.text.Image imgLogo = iTextSharp.text.Image.GetInstance(pathLogo);
                imgLogo.ScalePercent(0.5f);
                imgLogo.Alignment = Element.ALIGN_CENTER;
                PdfPCell cell = new PdfPCell(imgLogo, true);
                cell.FixedHeight = 30f;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.VerticalAlignment = Element.ALIGN_CENTER;
                cell.Border = Rectangle.NO_BORDER;
                cell.Colspan = 10;
                table.AddCell(cell);

                // Titulo del Documento
                PdfPCell cTitulo = new PdfPCell(new Paragraph("Revisión de documentos – Sistema Registro de Integrantes", MediumFontBold));
                cTitulo.HorizontalAlignment = Rectangle.ALIGN_RIGHT;

                PdfPCell cNro = new PdfPCell(new Paragraph(string.Format("NRO: {0}", oEmpresa.Emprnroconstancia), MediumFontBold));
                cNro.HorizontalAlignment = Rectangle.ALIGN_RIGHT;

                PdfPCell cSubTitulo = new PdfPCell(new Paragraph(string.Format("Fecha : {0}", ((DateTime)oEmpresa.Fecregistro).ToString("dd/MM/yyyy hh:mm tt")), SmallFontBold));
                cSubTitulo.HorizontalAlignment = Rectangle.ALIGN_RIGHT;

                // Datos de Empresa
                PdfPTable tblDatosEmpresa = new PdfPTable(2);
                tblDatosEmpresa.WidthPercentage = 100;

                PdfPCell cDatosEmpresaTitulo = new PdfPCell(new Paragraph("I) DATOS DE EMPRESA:", Titulo));
                cDatosEmpresaTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                cDatosEmpresaTitulo.Colspan = 2;
                cDatosEmpresaTitulo.BackgroundColor = new BaseColor(98, 101, 103);
                tblDatosEmpresa.AddCell(cDatosEmpresaTitulo);

                PdfPCell cDatosEmpresaSubTitulo = new PdfPCell(new Paragraph(TituloEmpresa, SubTitulo));
                cDatosEmpresaSubTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                cDatosEmpresaSubTitulo.Colspan = 2;
                cDatosEmpresaSubTitulo.BackgroundColor = new BaseColor(240, 240, 240);
                tblDatosEmpresa.AddCell(cDatosEmpresaSubTitulo);

                PdfPCell cDatosEmpresaRucTitulo = new PdfPCell(new Paragraph("Ruc: ", SmallFontBold));
                cDatosEmpresaRucTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                tblDatosEmpresa.AddCell(cDatosEmpresaRucTitulo);

                PdfPCell cDatosEmpresaRuc = new PdfPCell(new Paragraph(oEmpresa.Emprruc, SmallFont));
                cDatosEmpresaRuc.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                tblDatosEmpresa.AddCell(cDatosEmpresaRuc);

                PdfPCell cDatosEmpresaNombreComercialTitulo = new PdfPCell(new Paragraph("Nombre Comercial: ", SmallFontBold));
                cDatosEmpresaNombreComercialTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                tblDatosEmpresa.AddCell(cDatosEmpresaNombreComercialTitulo);

                PdfPCell cDatosEmpresaNombreComercial = new PdfPCell(new Paragraph(oEmpresa.Emprnombrecomercial, SmallFont));
                cDatosEmpresaNombreComercial.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                tblDatosEmpresa.AddCell(cDatosEmpresaNombreComercial);

                PdfPCell cDatosEmpresaRazonSocialTitulo = new PdfPCell(new Paragraph("Denomicacion o Razon Social: ", SmallFontBold));
                cDatosEmpresaRazonSocialTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                tblDatosEmpresa.AddCell(cDatosEmpresaRazonSocialTitulo);

                PdfPCell cDatosEmpresaRazonSocial = new PdfPCell(new Paragraph(oEmpresa.Emprrazsocial, SmallFont));
                cDatosEmpresaRazonSocial.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                tblDatosEmpresa.AddCell(cDatosEmpresaRazonSocial);

                PdfPCell cDatosEmpresaDomicilioLegalTitulo = new PdfPCell(new Paragraph("Domicilio Legal: ", SmallFontBold));
                cDatosEmpresaDomicilioLegalTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                tblDatosEmpresa.AddCell(cDatosEmpresaDomicilioLegalTitulo);

                PdfPCell cDatosEmpresaDomicilioLegal = new PdfPCell(new Paragraph(oEmpresa.Emprdomiciliolegal, SmallFont));
                cDatosEmpresaDomicilioLegalTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                tblDatosEmpresa.AddCell(cDatosEmpresaDomicilioLegal);

                PdfPCell cDatosEmpresaSiglaTitulo = new PdfPCell(new Paragraph("Sigla: ", SmallFontBold));
                cDatosEmpresaSiglaTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                tblDatosEmpresa.AddCell(cDatosEmpresaSiglaTitulo);

                PdfPCell cDatosEmpresaSigla = new PdfPCell(new Paragraph(oEmpresa.Emprsigla, SmallFont));
                cDatosEmpresaSigla.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                tblDatosEmpresa.AddCell(cDatosEmpresaSigla);

                PdfPCell cDatosEmpresaNumeroPartidaTitulo = new PdfPCell(new Paragraph("Número de Partida Registral: ", SmallFontBold));
                cDatosEmpresaNumeroPartidaTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                tblDatosEmpresa.AddCell(cDatosEmpresaNumeroPartidaTitulo);

                PdfPCell cDatosEmpresaNumeroPartida = new PdfPCell(new Paragraph(oEmpresa.Emprnumpartidareg, SmallFont));
                cDatosEmpresaNumeroPartida.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                tblDatosEmpresa.AddCell(cDatosEmpresaNumeroPartida);

                PdfPCell cDatosEmpresaTelefonoTitulo = new PdfPCell(new Paragraph("Teléfono: ", SmallFontBold));
                cDatosEmpresaTelefonoTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                tblDatosEmpresa.AddCell(cDatosEmpresaTelefonoTitulo);

                PdfPCell cDatosEmpresaTelefono = new PdfPCell(new Paragraph(oEmpresa.Emprtelefono, SmallFont));
                cDatosEmpresaTelefono.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                tblDatosEmpresa.AddCell(cDatosEmpresaTelefono);

                PdfPCell cDatosEmpresaFaxTitulo = new PdfPCell(new Paragraph("Fax: ", SmallFontBold));
                cDatosEmpresaFaxTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                tblDatosEmpresa.AddCell(cDatosEmpresaFaxTitulo);

                PdfPCell cDatosEmpresaFax = new PdfPCell(new Paragraph(oEmpresa.Emprfax, SmallFont));
                cDatosEmpresaFax.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                tblDatosEmpresa.AddCell(cDatosEmpresaFax);

                PdfPCell cDatosEmpresaPaginaWebTitulo = new PdfPCell(new Paragraph("Página web: ", SmallFontBold));
                cDatosEmpresaPaginaWebTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                tblDatosEmpresa.AddCell(cDatosEmpresaPaginaWebTitulo);

                PdfPCell cDatosEmpresaPaginaWeb = new PdfPCell(new Paragraph(oEmpresa.Emprpagweb, SmallFont));
                cDatosEmpresaPaginaWeb.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                tblDatosEmpresa.AddCell(cDatosEmpresaPaginaWeb);

                // Tipo Integrante
                PdfPTable tblTipoIntegrante = new PdfPTable(2);
                tblTipoIntegrante.WidthPercentage = 100;

                PdfPCell cTipoIntregranteTitulo = new PdfPCell(new Paragraph("II) TIPO INTEGRANTE:", Titulo));
                cTipoIntregranteTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                cTipoIntregranteTitulo.Colspan = 2;
                cTipoIntregranteTitulo.BackgroundColor = new BaseColor(98, 101, 103);
                tblTipoIntegrante.AddCell(cTipoIntregranteTitulo);

                PdfPCell cTipoIntregranteSubTitulo = new PdfPCell(new Paragraph(TituloTipoIntegrante, SubTitulo));
                cTipoIntregranteSubTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                cTipoIntregranteSubTitulo.Colspan = 2;
                cTipoIntregranteSubTitulo.BackgroundColor = new BaseColor(240, 240, 240);
                tblTipoIntegrante.AddCell(cTipoIntregranteSubTitulo);

                PdfPCell cTipoComportamientoTitulo = new PdfPCell(new Paragraph("Tipo: ", SmallFontBold));
                cTipoComportamientoTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                tblTipoIntegrante.AddCell(cTipoComportamientoTitulo);

                PdfPCell cTipoComportamiento = new PdfPCell(new Paragraph(existeTipoComportamiento ? TipoComportamientoPrincipal.Tipotipagente : "", SmallFont));
                cTipoComportamiento.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                tblTipoIntegrante.AddCell(cTipoComportamiento);

                if (existeTipoComportamiento)
                {
                    if (TipoComportamientoPrincipal.Tipoemprcodi == ConstantesRegistroIntegrantes.TipoComportamientoGeneradorCodigo)
                    {
                        PdfPCell cTipoComportamientoDSTitulo = new PdfPCell(new Paragraph("Documento Sustentatorio: ", SmallFontBold));
                        cTipoComportamientoDSTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                        tblTipoIntegrante.AddCell(cTipoComportamientoDSTitulo);

                        PdfPCell cTipoComportamientoDS = new PdfPCell(new Paragraph(TipoComportamientoPrincipal.Tipodocsustentatorio, SmallFont));
                        cTipoComportamientoDS.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                        tblTipoIntegrante.AddCell(cTipoComportamientoDS);

                        PdfPCell cTipoComportamientoPITitulo = new PdfPCell(new Paragraph("Potencia Instalada: ", SmallFontBold));
                        cTipoComportamientoPITitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                        tblTipoIntegrante.AddCell(cTipoComportamientoPITitulo);

                        PdfPCell cTipoComportamientoPI = new PdfPCell(new Paragraph(TipoComportamientoPrincipal.Tipopotenciainstalada, SmallFont));
                        cTipoComportamientoPI.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                        tblTipoIntegrante.AddCell(cTipoComportamientoPI);

                        PdfPCell cTipoComportamientoNCTitulo = new PdfPCell(new Paragraph("Número de Centrales: ", SmallFontBold));
                        cTipoComportamientoNCTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                        tblTipoIntegrante.AddCell(cTipoComportamientoNCTitulo);

                        PdfPCell cTipoComportamientoNC = new PdfPCell(new Paragraph(TipoComportamientoPrincipal.Tiponrocentrales, SmallFont));
                        cTipoComportamientoNC.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                        tblTipoIntegrante.AddCell(cTipoComportamientoNC);

                    }
                    else if (TipoComportamientoPrincipal.Tipoemprcodi == ConstantesRegistroIntegrantes.TipoComportamientoTrasmisorCodigo)
                    {
                        PdfPCell cTipoComportamientoDSTitulo = new PdfPCell(new Paragraph("Documento Sustentatorio: ", SmallFontBold));
                        cTipoComportamientoDSTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                        tblTipoIntegrante.AddCell(cTipoComportamientoDSTitulo);

                        PdfPCell cTipoComportamientoDS = new PdfPCell(new Paragraph(TipoComportamientoPrincipal.Tipodocsustentatorio, SmallFont));
                        cTipoComportamientoDS.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                        tblTipoIntegrante.AddCell(cTipoComportamientoDS);

                        PdfPCell cTipoComportamientoLTTitulo = new PdfPCell(new Paragraph("Línea de Transmisión: ", SmallFontBold));
                        cTipoComportamientoLTTitulo.HorizontalAlignment = Rectangle.ALIGN_MIDDLE;
                        cTipoComportamientoLTTitulo.Colspan = 2;
                        tblTipoIntegrante.AddCell(cTipoComportamientoLTTitulo);

                        PdfPCell cTipoComportamiento500KVTitulo = new PdfPCell(new Paragraph("500 kV: ", SmallFontBold));
                        cTipoComportamiento500KVTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                        tblTipoIntegrante.AddCell(cTipoComportamiento500KVTitulo);

                        PdfPCell cTipoComportamiento500KV = new PdfPCell(new Paragraph(TipoComportamientoPrincipal.Tipolineatrans500km, SmallFont));
                        cTipoComportamiento500KV.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                        tblTipoIntegrante.AddCell(cTipoComportamiento500KV);

                        PdfPCell cTipoComportamiento200KVTitulo = new PdfPCell(new Paragraph("200 kV: ", SmallFontBold));
                        cTipoComportamiento200KVTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                        tblTipoIntegrante.AddCell(cTipoComportamiento200KVTitulo);

                        PdfPCell cTipoComportamiento200KV = new PdfPCell(new Paragraph(TipoComportamientoPrincipal.Tipolineatrans220km, SmallFont));
                        cTipoComportamiento200KV.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                        tblTipoIntegrante.AddCell(cTipoComportamiento200KV);

                        PdfPCell cTipoComportamiento138KVTitulo = new PdfPCell(new Paragraph("138 kV: ", SmallFontBold));
                        cTipoComportamiento138KVTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                        tblTipoIntegrante.AddCell(cTipoComportamiento138KVTitulo);

                        PdfPCell cTipoComportamiento138KV = new PdfPCell(new Paragraph(TipoComportamientoPrincipal.Tipolineatrans138km, SmallFont));
                        cTipoComportamiento138KV.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                        tblTipoIntegrante.AddCell(cTipoComportamiento138KV);

                        PdfPCell cTipoComportamientoTLTTitulo = new PdfPCell(new Paragraph("Total Líneas de transmisión: ", SmallFontBold));
                        cTipoComportamientoTLTTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                        tblTipoIntegrante.AddCell(cTipoComportamientoTLTTitulo);

                        PdfPCell cTipoComportamientoTLT = new PdfPCell(new Paragraph(TipoComportamientoPrincipal.Tipototallineastransmision, SmallFont));
                        cTipoComportamientoTLT.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                        tblTipoIntegrante.AddCell(cTipoComportamientoTLT);

                    }
                    else if (TipoComportamientoPrincipal.Tipoemprcodi == ConstantesRegistroIntegrantes.TipoComportamientoDistribuidorCodigo)
                    {
                        PdfPCell cTipoComportamientoDSTitulo = new PdfPCell(new Paragraph("Documento Sustentatorio: ", SmallFontBold));
                        cTipoComportamientoDSTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                        tblTipoIntegrante.AddCell(cTipoComportamientoDSTitulo);

                        PdfPCell cTipoComportamientoDS = new PdfPCell(new Paragraph(TipoComportamientoPrincipal.Tipodocsustentatorio, SmallFont));
                        cTipoComportamientoDS.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                        tblTipoIntegrante.AddCell(cTipoComportamientoDS);

                        PdfPCell cTipoComportamientoMDCATitulo = new PdfPCell(new Paragraph("Máxima demanda coincidente anual : ", SmallFontBold));
                        cTipoComportamientoMDCATitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                        tblTipoIntegrante.AddCell(cTipoComportamientoMDCATitulo);

                        PdfPCell cTipoComportamientoMDCA = new PdfPCell(new Paragraph(TipoComportamientoPrincipal.Tipomaxdemandacoincidente, SmallFont));
                        cTipoComportamientoMDCA.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                        tblTipoIntegrante.AddCell(cTipoComportamientoMDCA);

                    }
                    else if (TipoComportamientoPrincipal.Tipoemprcodi == ConstantesRegistroIntegrantes.TipoComportamientoUsuarioLibreCodigo)
                    {
                        PdfPCell cTipoComportamientoDSTitulo = new PdfPCell(new Paragraph("Documento Sustentatorio: ", SmallFontBold));
                        cTipoComportamientoDSTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                        tblTipoIntegrante.AddCell(cTipoComportamientoDSTitulo);

                        PdfPCell cTipoComportamientoDS = new PdfPCell(new Paragraph(TipoComportamientoPrincipal.Tipodocsustentatorio, SmallFont));
                        cTipoComportamientoDS.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                        tblTipoIntegrante.AddCell(cTipoComportamientoDS);

                        PdfPCell cTipoComportamientoMDCTitulo = new PdfPCell(new Paragraph("Máxima demanda contratada (MW): ", SmallFontBold));
                        cTipoComportamientoMDCTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                        tblTipoIntegrante.AddCell(cTipoComportamientoMDCTitulo);

                        PdfPCell cTipoComportamientoMDC = new PdfPCell(new Paragraph(TipoComportamientoPrincipal.Tipomaxdemandacontratada, SmallFont));
                        cTipoComportamientoMDC.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                        tblTipoIntegrante.AddCell(cTipoComportamientoMDC);

                        PdfPCell cTipoComportamientoNSTitulo = new PdfPCell(new Paragraph("Número de Suministrador: ", SmallFontBold));
                        cTipoComportamientoNSTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                        tblTipoIntegrante.AddCell(cTipoComportamientoNSTitulo);

                        PdfPCell cTipoComportamientoNS = new PdfPCell(new Paragraph(TipoComportamientoPrincipal.Tiponumsuministrador, SmallFont));
                        cTipoComportamientoNS.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                        tblTipoIntegrante.AddCell(cTipoComportamientoNS);

                    }
                }

                PdfPTable tblRepresentante = new PdfPTable(2);
                tblRepresentante.WidthPercentage = 100;

                PdfPCell cRepresentanteLegalTitulo = new PdfPCell(new Paragraph("III) REPRESENTANTE LEGAL:", Titulo));
                cRepresentanteLegalTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                cRepresentanteLegalTitulo.Colspan = 2;
                cRepresentanteLegalTitulo.BackgroundColor = new BaseColor(98, 101, 103);
                tblRepresentante.AddCell(cRepresentanteLegalTitulo);

                PdfPCell cRepresentanteLegalSubTitulo = new PdfPCell(new Paragraph(TituloRepresentanteLegal, SubTitulo));
                cRepresentanteLegalSubTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                cRepresentanteLegalSubTitulo.Colspan = 2;
                cRepresentanteLegalSubTitulo.BackgroundColor = new BaseColor(240, 240, 240);
                tblRepresentante.AddCell(cRepresentanteLegalSubTitulo);

                foreach (var item in RepresentanteLegal)
                {
                    string Tipo = "";

                    if (item.Rptetiprepresentantelegal == ConstantesRegistroIntegrantes.RepresentanteLegalTipoTitular)
                    {
                        Tipo = ConstantesRegistroIntegrantes.RepresentanteLegalTitular;
                    }
                    else
                    {
                        Tipo = ConstantesRegistroIntegrantes.RepresentanteLegalAlterno;
                    }

                    PdfPCell cRepresentanteTipoTitulo = new PdfPCell(new Paragraph("Tipo: ", SmallFontBold));
                    cRepresentanteTipoTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    tblRepresentante.AddCell(cRepresentanteTipoTitulo);

                    PdfPCell cRepresentanteTipo = new PdfPCell(new Paragraph(Tipo, SmallFont));
                    cRepresentanteTipo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    tblRepresentante.AddCell(cRepresentanteTipo);

                    PdfPCell cRepresentanteDNITitulo = new PdfPCell(new Paragraph("DNI / Carné de Extranjeria: ", SmallFontBold));
                    cRepresentanteDNITitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    tblRepresentante.AddCell(cRepresentanteDNITitulo);

                    PdfPCell cRepresentanteDNI = new PdfPCell(new Paragraph(item.Rptedocidentidad, SmallFont));
                    cRepresentanteDNI.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    tblRepresentante.AddCell(cRepresentanteDNI);

                    PdfPCell cRepresentanteNombreTitulo = new PdfPCell(new Paragraph("Nombre: ", SmallFontBold));
                    cRepresentanteNombreTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    tblRepresentante.AddCell(cRepresentanteNombreTitulo);

                    PdfPCell cRepresentanteNombre = new PdfPCell(new Paragraph(item.Rptenombres, SmallFont));
                    cRepresentanteNombre.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    tblRepresentante.AddCell(cRepresentanteNombre);

                    PdfPCell cRepresentanteApellidoTitulo = new PdfPCell(new Paragraph("Apellido: ", SmallFontBold));
                    cRepresentanteApellidoTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    tblRepresentante.AddCell(cRepresentanteApellidoTitulo);

                    PdfPCell cRepresentanteApellido = new PdfPCell(new Paragraph(item.Rpteapellidos, SmallFont));
                    cRepresentanteApellido.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    tblRepresentante.AddCell(cRepresentanteApellido);

                    PdfPCell cRepresentanteCETitulo = new PdfPCell(new Paragraph("Cargo en la Empresa: ", SmallFontBold));
                    cRepresentanteCETitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    tblRepresentante.AddCell(cRepresentanteCETitulo);

                    PdfPCell cRepresentanteCE = new PdfPCell(new Paragraph(item.Rptecargoempresa, SmallFont));
                    cRepresentanteCE.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    tblRepresentante.AddCell(cRepresentanteCE);

                    PdfPCell cRepresentanteTeleTitulo = new PdfPCell(new Paragraph("Teléfono: ", SmallFontBold));
                    cRepresentanteTeleTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    tblRepresentante.AddCell(cRepresentanteTeleTitulo);

                    PdfPCell cRepresentanteTele = new PdfPCell(new Paragraph(item.Rptetelefono, SmallFont));
                    cRepresentanteTele.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    tblRepresentante.AddCell(cRepresentanteTele);

                    PdfPCell cRepresentanteTeleMovilTitulo = new PdfPCell(new Paragraph("Teléfono Movil: ", SmallFontBold));
                    cRepresentanteTeleMovilTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    tblRepresentante.AddCell(cRepresentanteTeleMovilTitulo);

                    PdfPCell cRepresentanteTeleMovil = new PdfPCell(new Paragraph(item.Rptetelfmovil, SmallFont));
                    cRepresentanteTeleMovil.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    tblRepresentante.AddCell(cRepresentanteTeleMovil);

                    PdfPCell cRepresentanteCorreoTitulo = new PdfPCell(new Paragraph("Correo Electrónico: ", SmallFontBold));
                    cRepresentanteCorreoTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    tblRepresentante.AddCell(cRepresentanteCorreoTitulo);

                    PdfPCell cRepresentanteCorreo = new PdfPCell(new Paragraph(item.Rptecorreoelectronico, SmallFont));
                    cRepresentanteCorreo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    tblRepresentante.AddCell(cRepresentanteCorreo);

                    tblRepresentante.AddCell(new Paragraph(""));
                    tblRepresentante.AddCell(new Paragraph(""));

                }

                //Contacto
                PdfPTable tblContacto = new PdfPTable(2);
                tblContacto.WidthPercentage = 100;

                PdfPCell cContactoTitulo = new PdfPCell(new Paragraph("IV) PERSONA CONTACTO:", Titulo));
                cContactoTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                cContactoTitulo.Colspan = 2;
                cContactoTitulo.BackgroundColor = new BaseColor(98, 101, 103);
                tblContacto.AddCell(cContactoTitulo);

                PdfPCell cContactoSubTitulo = new PdfPCell(new Paragraph(TituloPersonaContacto, SubTitulo));
                cContactoSubTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                cContactoSubTitulo.Colspan = 2;
                cContactoSubTitulo.BackgroundColor = new BaseColor(240, 240, 240);
                tblContacto.AddCell(cContactoSubTitulo);


                if (PersonaContacto != null)
                {
                    PdfPCell cContactoNombreTitulo = new PdfPCell(new Paragraph("Nombre: ", SmallFontBold));
                    cContactoNombreTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    tblContacto.AddCell(cContactoNombreTitulo);

                    PdfPCell cContactoNombre = new PdfPCell(new Paragraph(PersonaContacto.Rptenombres, SmallFont));
                    cContactoNombre.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    tblContacto.AddCell(cContactoNombre);

                    PdfPCell cContactoApellidoTitulo = new PdfPCell(new Paragraph("Apellido: ", SmallFontBold));
                    cContactoApellidoTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    tblContacto.AddCell(cContactoApellidoTitulo);

                    PdfPCell cContactoApellido = new PdfPCell(new Paragraph(PersonaContacto.Rpteapellidos, SmallFont));
                    cContactoApellido.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    tblContacto.AddCell(cContactoApellido);

                    PdfPCell cContactoCETitulo = new PdfPCell(new Paragraph("Cargo en la Empresa: ", SmallFontBold));
                    cContactoCETitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    tblContacto.AddCell(cContactoCETitulo);

                    PdfPCell cContactoCE = new PdfPCell(new Paragraph(PersonaContacto.Rptecargoempresa, SmallFont));
                    cContactoCE.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    tblContacto.AddCell(cContactoCE);

                    PdfPCell cContactoTeleTitulo = new PdfPCell(new Paragraph("Teléfono: ", SmallFontBold));
                    cContactoTeleTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    tblContacto.AddCell(cContactoTeleTitulo);

                    PdfPCell cContactoTele = new PdfPCell(new Paragraph(PersonaContacto.Rptetelefono, SmallFont));
                    cContactoTele.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    tblContacto.AddCell(cContactoTele);

                    PdfPCell cContactoTeleMovilTitulo = new PdfPCell(new Paragraph("Teléfono Movil: ", SmallFontBold));
                    cContactoTeleMovilTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    tblContacto.AddCell(cContactoTeleMovilTitulo);

                    PdfPCell cContactoTeleMovil = new PdfPCell(new Paragraph(PersonaContacto.Rptetelfmovil, SmallFont));
                    cContactoTeleMovil.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    tblContacto.AddCell(cContactoTeleMovil);

                    PdfPCell cContactoCorreoTitulo = new PdfPCell(new Paragraph("Correo Electrónico: ", SmallFontBold));
                    cContactoCorreoTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    tblContacto.AddCell(cContactoCorreoTitulo);

                    PdfPCell cContactoCorreo = new PdfPCell(new Paragraph(PersonaContacto.Rptecorreoelectronico, SmallFont));
                    cContactoCorreo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    tblContacto.AddCell(cContactoCorreo);

                }



                //Reponsable Tramite
                PdfPTable tblResponsable = new PdfPTable(2);
                tblResponsable.WidthPercentage = 100;


                if (ResponsableTramite != null)
                {
                    PdfPCell cResponsableTitulo = new PdfPCell(new Paragraph("V) PERSONA RESPONSABLE: ", Titulo));
                    cResponsableTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    cResponsableTitulo.Colspan = 2;
                    cResponsableTitulo.BackgroundColor = new BaseColor(98, 101, 103);
                    tblResponsable.AddCell(cResponsableTitulo);

                    PdfPCell cResponsableSubTitulo = new PdfPCell(new Paragraph(TituloPersonaResponsable, SubTitulo));
                    cResponsableSubTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    cResponsableSubTitulo.Colspan = 2;
                    cResponsableSubTitulo.BackgroundColor = new BaseColor(240, 240, 240);
                    tblResponsable.AddCell(cResponsableSubTitulo);

                    PdfPCell cResponsableNombreTitulo = new PdfPCell(new Paragraph("Nombre: ", SmallFontBold));
                    cResponsableNombreTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    tblResponsable.AddCell(cResponsableNombreTitulo);

                    PdfPCell cResponsableNombre = new PdfPCell(new Paragraph(ResponsableTramite.Rptenombres, SmallFont));
                    cResponsableNombre.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    tblResponsable.AddCell(cResponsableNombre);

                    PdfPCell cResponsableApellidoTitulo = new PdfPCell(new Paragraph("Apellido: ", SmallFontBold));
                    cResponsableApellidoTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    tblResponsable.AddCell(cResponsableApellidoTitulo);

                    PdfPCell cResponsableApellido = new PdfPCell(new Paragraph(ResponsableTramite.Rpteapellidos, SmallFont));
                    cResponsableApellido.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    tblResponsable.AddCell(cResponsableApellido);

                    PdfPCell cResponsableCETitulo = new PdfPCell(new Paragraph("Cargo en la Empresa: ", SmallFontBold));
                    cResponsableCETitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    tblResponsable.AddCell(cResponsableCETitulo);

                    PdfPCell cResponsableCE = new PdfPCell(new Paragraph(ResponsableTramite.Rptecargoempresa, SmallFont));
                    cResponsableCE.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    tblResponsable.AddCell(cResponsableCE);

                    PdfPCell cResponsableTeleTitulo = new PdfPCell(new Paragraph("Teléfono: ", SmallFontBold));
                    cResponsableTeleTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    tblResponsable.AddCell(cResponsableTeleTitulo);

                    PdfPCell cResponsableTele = new PdfPCell(new Paragraph(ResponsableTramite.Rptetelefono, SmallFont));
                    cResponsableTele.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    tblResponsable.AddCell(cResponsableTele);

                    PdfPCell cResponsableTeleMovilTitulo = new PdfPCell(new Paragraph("Teléfono Movil: ", SmallFontBold));
                    cResponsableTeleMovilTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    tblResponsable.AddCell(cResponsableTeleMovilTitulo);

                    PdfPCell cResponsableTeleMovil = new PdfPCell(new Paragraph(ResponsableTramite.Rptetelfmovil, SmallFont));
                    cResponsableTeleMovil.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    tblResponsable.AddCell(cResponsableTeleMovil);

                    PdfPCell cResponsableCorreoTitulo = new PdfPCell(new Paragraph("Correo Electrónico: ", SmallFontBold));
                    cResponsableCorreoTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    tblResponsable.AddCell(cResponsableCorreoTitulo);

                    PdfPCell cResponsableCorreo = new PdfPCell(new Paragraph(ResponsableTramite.Rptecorreoelectronico, SmallFont));
                    cResponsableCorreo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    tblResponsable.AddCell(cResponsableCorreo);

                }


                tblContent.AddCell(table);

                tblContent.AddCell(cTitulo);
                tblContent.AddCell(cNro);
                tblContent.AddCell(cSubTitulo);
                tblContent.AddCell(tblDatosEmpresa);
                tblContent.AddCell(tblTipoIntegrante);
                tblContent.AddCell(tblRepresentante);
                tblContent.AddCell(tblContacto);
                tblContent.AddCell(tblResponsable);

                tblContent.DefaultCell.Border = Rectangle.NO_BORDER;

                document.Add(tblContent);

                document.Close();
                writer.Close();

                bytes = ms.GetBuffer();
            }

            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.Message);

            //}

            return bytes;//File(bytes, "application/pdf", FileName + ".pdf");
        }

        /// <summary>
        /// Exportar Constancia de reigstro de integrante en PDF
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public byte[] ExportarRegistroPDF(int id, out int nroRegistro)
        {
            byte[] bytes = new byte[0];

            try
            {

                RegistroIntegrantesAppServicio oServicio = new RegistroIntegrantesAppServicio();


                SiEmpresaDTO oEmpresa = oServicio.GetEmpresaByIdConRevision(id);

                nroRegistro = oEmpresa.Emprnroregistro;

                var Representante = oServicio.GetRepresentantesByEmprcodi(id);
                var TipoComportamiento = oServicio.ListSiTipoComportamientoByEmprcodi(id);
                var RepresentanteLegalTitular = Representante.FindAll(x => x.Rptetipo == ConstantesRegistroIntegrantes.RepresentanteTipoLegal && x.Rptetiprepresentantelegal == ConstantesRegistroIntegrantes.RepresentanteLegalTipoTitular).OrderByDescending(x=>x.Rptefeccreacion);
                var RepresentanteLegalAlterno = Representante.FindAll(x => x.Rptetipo == ConstantesRegistroIntegrantes.RepresentanteTipoLegal && x.Rptetiprepresentantelegal == ConstantesRegistroIntegrantes.RepresentanteLegalTipoAlterno);
                var PersonaContacto = Representante.FindAll(x => x.Rptetipo == ConstantesRegistroIntegrantes.RepresentanteTipoContacto);
                var ResponsableTramite = Representante.Find(x => x.Rptetipo == ConstantesRegistroIntegrantes.RepresentanteTipoResponsableTramite);
                var TipoComportamientoPrincipal = TipoComportamiento.Find(x => x.Tipoprincipal == ConstantesRegistroIntegrantes.TipoComportamientoPrincipalSi);

                bool existeTipoComportamiento = true;
                if (TipoComportamientoPrincipal == null)
                    existeTipoComportamiento = false;

                using (MemoryStream ms = new MemoryStream())
                {
                    Document document = new Document(iTextSharp.text.PageSize.A4, 50f, 50f, 50f, 50f);  // A4.Rotate PARA HORIZONTAL
                    PdfWriter writer = PdfWriter.GetInstance(document, ms);

                    document.Open();

                    PdfContentByte pcb = writer.DirectContent;

                    var Titulo = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 8, BaseColor.WHITE);

                    var Smaller = FontFactory.GetFont(FontFactory.HELVETICA, 6);
                    var SmallFontBold = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 7);
                    var SmallFont = FontFactory.GetFont(FontFactory.HELVETICA, 7);
                    var SmallFontWhite = FontFactory.GetFont(FontFactory.HELVETICA, 7);
                    var SmallFontBoldWhite = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 7);
                    var RegularFontBold = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);

                    var MediumFontBold = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
                    var MediumFont = FontFactory.GetFont(FontFactory.HELVETICA, 9);
                    var LargeFontBold = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 32);
                    var LargeFont = FontFactory.GetFont(FontFactory.HELVETICA, 32);

                    MediumFont.SetColor(0, 0, 0);


                    //Tabla Principal
                    PdfPTable tblContent = new PdfPTable(1);
                    tblContent.WidthPercentage = 100;

                    //Logo

                    PdfPTable table = new PdfPTable(10);
                    table.TotalWidth = 500f;
                    table.LockedWidth = true;
                    float[] widths = new float[] { 2f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f };
                    table.SetWidths(widths);
                    table.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.SpacingBefore = 5f;
                    table.SpacingAfter = 0f;

                    string pathLogo = ConfigurationManager.AppSettings["RutaLogo"].ToString() + "logocoes.png";
                    iTextSharp.text.Image imgLogo = iTextSharp.text.Image.GetInstance(pathLogo);
                    imgLogo.ScalePercent(0.5f);
                    imgLogo.Alignment = Element.ALIGN_LEFT;
                    PdfPCell cell = new PdfPCell(imgLogo, true);
                    cell.FixedHeight = 30f;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.VerticalAlignment = Element.ALIGN_CENTER;
                    cell.Border = Rectangle.NO_BORDER;
                    cell.Colspan = 10;
                    table.AddCell(cell);

                    // Titulo del Documento

                    PdfPCell cTitulo = new PdfPCell(new Paragraph("FORMULARIO DE INSCRIPCIÓN EN EL REGISTRO DE COES", MediumFontBold));
                    cTitulo.HorizontalAlignment = Rectangle.ALIGN_RIGHT;

                    PdfPCell cSubTitulo = new PdfPCell(new Paragraph(string.Format("Nro : {0}", oEmpresa.Emprnroregistro), MediumFontBold));
                    cSubTitulo.HorizontalAlignment = Rectangle.ALIGN_RIGHT;

                    // Datos de Empresa
                    PdfPTable tblDatosEmpresa = new PdfPTable(2);
                    tblDatosEmpresa.WidthPercentage = 100;



                    PdfPCell cDatosEmpresaTitulo = new PdfPCell(new Paragraph("DATOS DE IDENTIFICACIÓN", Titulo));
                    cDatosEmpresaTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    cDatosEmpresaTitulo.Colspan = 2;
                    cDatosEmpresaTitulo.BackgroundColor = new BaseColor(98, 101, 103);
                    tblDatosEmpresa.AddCell(cDatosEmpresaTitulo);



                    PdfPCell cDatosEmpresaRazonSocialTitulo = new PdfPCell(new Paragraph("Denomicacion o Razon Social: ", SmallFontBold));
                    cDatosEmpresaRazonSocialTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    tblDatosEmpresa.AddCell(cDatosEmpresaRazonSocialTitulo);

                    PdfPCell cDatosEmpresaRazonSocial = new PdfPCell(new Paragraph(oEmpresa.Emprrazsocial, SmallFont));
                    cDatosEmpresaRazonSocial.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    tblDatosEmpresa.AddCell(cDatosEmpresaRazonSocial);

                    PdfPCell cDatosEmpresaNombreComercialTitulo = new PdfPCell(new Paragraph("Nombre Comercial: ", SmallFontBold));
                    cDatosEmpresaNombreComercialTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    tblDatosEmpresa.AddCell(cDatosEmpresaNombreComercialTitulo);

                    PdfPCell cDatosEmpresaNombreComercial = new PdfPCell(new Paragraph(oEmpresa.Emprnombrecomercial, SmallFont));
                    cDatosEmpresaNombreComercial.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    tblDatosEmpresa.AddCell(cDatosEmpresaNombreComercial);

                    PdfPCell cDatosEmpresaRucTitulo = new PdfPCell(new Paragraph("Número de R.U.C: ", SmallFontBold));
                    cDatosEmpresaRucTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    tblDatosEmpresa.AddCell(cDatosEmpresaRucTitulo);

                    PdfPCell cDatosEmpresaRuc = new PdfPCell(new Paragraph(oEmpresa.Emprruc, SmallFont));
                    cDatosEmpresaRuc.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    tblDatosEmpresa.AddCell(cDatosEmpresaRuc);

                    PdfPCell cDatosEmpresaNumeroPartidaTitulo = new PdfPCell(new Paragraph("Número de Partida Registral: ", SmallFontBold));
                    cDatosEmpresaNumeroPartidaTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    tblDatosEmpresa.AddCell(cDatosEmpresaNumeroPartidaTitulo);

                    PdfPCell cDatosEmpresaNumeroPartida = new PdfPCell(new Paragraph(oEmpresa.Emprnumpartidareg, SmallFont));
                    cDatosEmpresaNumeroPartida.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    tblDatosEmpresa.AddCell(cDatosEmpresaNumeroPartida);

                    PdfPCell cDatosEmpresaDomicilioLegalTitulo = new PdfPCell(new Paragraph("Domicilio Legal: ", SmallFontBold));
                    cDatosEmpresaDomicilioLegalTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    tblDatosEmpresa.AddCell(cDatosEmpresaDomicilioLegalTitulo);

                    PdfPCell cDatosEmpresaDomicilioLegal = new PdfPCell(new Paragraph(oEmpresa.Emprdomiciliolegal, SmallFont));
                    cDatosEmpresaDomicilioLegalTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    tblDatosEmpresa.AddCell(cDatosEmpresaDomicilioLegal);

                    PdfPCell cDatosEmpresaTelefonoTitulo = new PdfPCell(new Paragraph("Teléfono: ", SmallFontBold));
                    cDatosEmpresaTelefonoTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    tblDatosEmpresa.AddCell(cDatosEmpresaTelefonoTitulo);

                    PdfPCell cDatosEmpresaTelefono = new PdfPCell(new Paragraph(oEmpresa.Emprtelefono, SmallFont));
                    cDatosEmpresaTelefono.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    tblDatosEmpresa.AddCell(cDatosEmpresaTelefono);

                    PdfPCell cDatosEmpresaFaxTitulo = new PdfPCell(new Paragraph("Fax: ", SmallFontBold));
                    cDatosEmpresaFaxTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    tblDatosEmpresa.AddCell(cDatosEmpresaFaxTitulo);

                    PdfPCell cDatosEmpresaFax = new PdfPCell(new Paragraph(oEmpresa.Emprfax, SmallFont));
                    cDatosEmpresaFax.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    tblDatosEmpresa.AddCell(cDatosEmpresaFax);

                    PdfPCell cDatosEmpresaPaginaWebTitulo = new PdfPCell(new Paragraph("Portal de internet: ", SmallFontBold));
                    cDatosEmpresaPaginaWebTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    tblDatosEmpresa.AddCell(cDatosEmpresaPaginaWebTitulo);

                    PdfPCell cDatosEmpresaPaginaWeb = new PdfPCell(new Paragraph(oEmpresa.Emprpagweb, SmallFont));
                    cDatosEmpresaPaginaWeb.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    tblDatosEmpresa.AddCell(cDatosEmpresaPaginaWeb);



                    PdfPTable tblRepresentante = new PdfPTable(2);
                    tblRepresentante.WidthPercentage = 100;

                    PdfPCell cRepresentanteLegalSubTitulo = new PdfPCell(new Paragraph("DATOS DE REPRESENTACIÓN", Titulo));
                    cRepresentanteLegalSubTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    cRepresentanteLegalSubTitulo.Colspan = 2;
                    cRepresentanteLegalSubTitulo.BackgroundColor = new BaseColor(98, 101, 103);
                    tblRepresentante.AddCell(cRepresentanteLegalSubTitulo);

                    int contador = 0;

                    //REPRESENTANTE LEGAL TITULAR
                    foreach (var item in RepresentanteLegalTitular)
                    {
                        contador++;

                        PdfPCell cRepresentanteNombreTitulo = new PdfPCell(new Paragraph("Representante legal (" + contador + "): ", SmallFontBold));
                        cRepresentanteNombreTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                        tblRepresentante.AddCell(cRepresentanteNombreTitulo);

                        PdfPCell cRepresentanteNombre = new PdfPCell(new Paragraph(item.Rptenombres + " " + item.Rpteapellidos, SmallFont));
                        cRepresentanteNombre.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                        tblRepresentante.AddCell(cRepresentanteNombre);

                        PdfPCell cRepresentanteCETitulo = new PdfPCell(new Paragraph("Cargo en la Empresa (" + contador + "): ", SmallFontBold));
                        cRepresentanteCETitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                        tblRepresentante.AddCell(cRepresentanteCETitulo);

                        PdfPCell cRepresentanteCE = new PdfPCell(new Paragraph(item.Rptecargoempresa, SmallFont));
                        cRepresentanteCE.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                        tblRepresentante.AddCell(cRepresentanteCE);

                        PdfPCell cRepresentanteTeleTitulo = new PdfPCell(new Paragraph("Teléfono (" + contador + "): ", SmallFontBold));
                        cRepresentanteTeleTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                        tblRepresentante.AddCell(cRepresentanteTeleTitulo);

                        PdfPCell cRepresentanteTele = new PdfPCell(new Paragraph(item.Rptetelefono, SmallFont));
                        cRepresentanteTele.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                        tblRepresentante.AddCell(cRepresentanteTele);

                        PdfPCell cRepresentanteCorreoTitulo = new PdfPCell(new Paragraph("Correo Electrónico (" + contador + "): ", SmallFontBold));
                        cRepresentanteCorreoTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                        tblRepresentante.AddCell(cRepresentanteCorreoTitulo);

                        PdfPCell cRepresentanteCorreo = new PdfPCell(new Paragraph(item.Rptecorreoelectronico, SmallFont));
                        cRepresentanteCorreo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                        tblRepresentante.AddCell(cRepresentanteCorreo);

                        tblRepresentante.AddCell(new Paragraph(""));
                        tblRepresentante.AddCell(new Paragraph(""));
                        
                        break;

                    }

                    //REPRESENTANTES LEGAL ALTERNO
                    foreach (var item in RepresentanteLegalAlterno)
                    {
                        contador++;

                        PdfPCell cRepresentanteNombreTitulo = new PdfPCell(new Paragraph("Representante legal (" + contador + "): ", SmallFontBold));
                        cRepresentanteNombreTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                        tblRepresentante.AddCell(cRepresentanteNombreTitulo);

                        PdfPCell cRepresentanteNombre = new PdfPCell(new Paragraph(item.Rptenombres + " " + item.Rpteapellidos, SmallFont));
                        cRepresentanteNombre.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                        tblRepresentante.AddCell(cRepresentanteNombre);

                        PdfPCell cRepresentanteCETitulo = new PdfPCell(new Paragraph("Cargo en la Empresa (" + contador + "): ", SmallFontBold));
                        cRepresentanteCETitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                        tblRepresentante.AddCell(cRepresentanteCETitulo);

                        PdfPCell cRepresentanteCE = new PdfPCell(new Paragraph(item.Rptecargoempresa, SmallFont));
                        cRepresentanteCE.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                        tblRepresentante.AddCell(cRepresentanteCE);

                        PdfPCell cRepresentanteTeleTitulo = new PdfPCell(new Paragraph("Teléfono (" + contador + "): ", SmallFontBold));
                        cRepresentanteTeleTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                        tblRepresentante.AddCell(cRepresentanteTeleTitulo);

                        PdfPCell cRepresentanteTele = new PdfPCell(new Paragraph(item.Rptetelefono, SmallFont));
                        cRepresentanteTele.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                        tblRepresentante.AddCell(cRepresentanteTele);

                        PdfPCell cRepresentanteCorreoTitulo = new PdfPCell(new Paragraph("Correo Electrónico (" + contador + "): ", SmallFontBold));
                        cRepresentanteCorreoTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                        tblRepresentante.AddCell(cRepresentanteCorreoTitulo);

                        PdfPCell cRepresentanteCorreo = new PdfPCell(new Paragraph(item.Rptecorreoelectronico, SmallFont));
                        cRepresentanteCorreo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                        tblRepresentante.AddCell(cRepresentanteCorreo);

                        tblRepresentante.AddCell(new Paragraph(""));
                        tblRepresentante.AddCell(new Paragraph(""));

                    }                    

                    //Contacto
                    PdfPTable tblContacto = new PdfPTable(2);
                    tblContacto.WidthPercentage = 100;

                    PdfPCell cContactoTitulo = new PdfPCell(new Paragraph("PERSONA CONTACTO:", Titulo));
                    cContactoTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    cContactoTitulo.Colspan = 2;
                    cContactoTitulo.BackgroundColor = new BaseColor(98, 101, 103);
                    tblContacto.AddCell(cContactoTitulo);

                    contador = 0;
                    foreach (var item in PersonaContacto)
                    {
                        contador++;
                        PdfPCell cContactoNombreTitulo = new PdfPCell(new Paragraph("Persona de contacto (:" + contador + "); ", SmallFontBold));
                        cContactoNombreTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                        tblContacto.AddCell(cContactoNombreTitulo);

                        PdfPCell cContactoNombre = new PdfPCell(new Paragraph(item.Rptenombres + " " + item.Rpteapellidos, SmallFont));
                        cContactoNombre.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                        tblContacto.AddCell(cContactoNombre);

                        PdfPCell cContactoCETitulo = new PdfPCell(new Paragraph("Cargo en la Empresa (" + contador + "): ", SmallFontBold));
                        cContactoCETitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                        tblContacto.AddCell(cContactoCETitulo);

                        PdfPCell cContactoCE = new PdfPCell(new Paragraph(item.Rptecargoempresa, SmallFont));
                        cContactoCE.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                        tblContacto.AddCell(cContactoCE);

                        PdfPCell cContactoTeleTitulo = new PdfPCell(new Paragraph("Teléfono (" + contador + "): ", SmallFontBold));
                        cContactoTeleTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                        tblContacto.AddCell(cContactoTeleTitulo);

                        PdfPCell cContactoTele = new PdfPCell(new Paragraph(item.Rptetelefono, SmallFont));
                        cContactoTele.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                        tblContacto.AddCell(cContactoTele);

                        PdfPCell cContactoTeleMovilTitulo = new PdfPCell(new Paragraph("Teléfono móvil (" + contador + "): ", SmallFontBold));
                        cContactoTeleMovilTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                        tblContacto.AddCell(cContactoTeleMovilTitulo);

                        PdfPCell cContactoTeleMovil = new PdfPCell(new Paragraph(item.Rptetelfmovil, SmallFont));
                        cContactoTeleMovil.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                        tblContacto.AddCell(cContactoTeleMovil);

                        PdfPCell cContactoCorreoTitulo = new PdfPCell(new Paragraph("Correo Electrónico (" + contador + "): ", SmallFontBold));
                        cContactoCorreoTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                        tblContacto.AddCell(cContactoCorreoTitulo);

                        PdfPCell cContactoCorreo = new PdfPCell(new Paragraph(item.Rptecorreoelectronico, SmallFont));
                        cContactoCorreo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                        tblContacto.AddCell(cContactoCorreo);

                        tblContacto.AddCell(new Paragraph(""));
                        tblContacto.AddCell(new Paragraph(""));

                    }



                    // Tipo Integrante
                    PdfPTable tblTipoIntegrante = new PdfPTable(2);
                    tblTipoIntegrante.WidthPercentage = 100;

                    PdfPCell cTipoIntregranteSubTitulo = new PdfPCell(new Paragraph("INFORMACIÓN SOBRE EL AGENTE", Titulo));
                    cTipoIntregranteSubTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    cTipoIntregranteSubTitulo.Colspan = 2;
                    cTipoIntregranteSubTitulo.BackgroundColor = new BaseColor(98, 101, 103);
                    tblTipoIntegrante.AddCell(cTipoIntregranteSubTitulo);

                    PdfPCell cTipoComportamientoTitulo = new PdfPCell(new Paragraph("Tipo Agente: ", SmallFontBold));
                    cTipoComportamientoTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    tblTipoIntegrante.AddCell(cTipoComportamientoTitulo);

                    PdfPCell cTipoComportamiento = new PdfPCell(new Paragraph(existeTipoComportamiento ? TipoComportamientoPrincipal.Tipotipagente : "", SmallFont));
                    cTipoComportamiento.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    tblTipoIntegrante.AddCell(cTipoComportamiento);

                    PdfPCell cTipoComportamientoDSTitulo = new PdfPCell(new Paragraph("Documento Sustentatorio: ", SmallFontBold));
                    cTipoComportamientoDSTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    tblTipoIntegrante.AddCell(cTipoComportamientoDSTitulo);

                    PdfPCell cTipoComportamientoDS = new PdfPCell(new Paragraph(existeTipoComportamiento ? TipoComportamientoPrincipal.Tipodocsustentatorio : "", SmallFont));
                    cTipoComportamientoDS.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    tblTipoIntegrante.AddCell(cTipoComportamientoDS);


                    // Tipo Integrante
                    PdfPTable tblInformacionTecnica = new PdfPTable(2);
                    tblInformacionTecnica.WidthPercentage = 100;

                    PdfPCell cTipoInformacionTecnicaSubTitulo = new PdfPCell(new Paragraph("INFORMACIÓN TÉCNICA", Titulo));
                    cTipoInformacionTecnicaSubTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    cTipoInformacionTecnicaSubTitulo.Colspan = 2;
                    cTipoInformacionTecnicaSubTitulo.BackgroundColor = new BaseColor(98, 101, 103);
                    tblInformacionTecnica.AddCell(cTipoInformacionTecnicaSubTitulo);

                    if (existeTipoComportamiento)
                    {
                        if (TipoComportamientoPrincipal.Tipoemprcodi == ConstantesRegistroIntegrantes.TipoComportamientoGeneradorCodigo)
                        {

                            PdfPCell cTipoComportamientoPITitulo = new PdfPCell(new Paragraph("Potencia Instalada (MW): ", SmallFontBold));
                            cTipoComportamientoPITitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                            tblInformacionTecnica.AddCell(cTipoComportamientoPITitulo);

                            PdfPCell cTipoComportamientoPI = new PdfPCell(new Paragraph(TipoComportamientoPrincipal.Tipopotenciainstalada, SmallFont));
                            cTipoComportamientoPI.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                            tblInformacionTecnica.AddCell(cTipoComportamientoPI);

                            PdfPCell cTipoComportamientoNCTitulo = new PdfPCell(new Paragraph("Número de Centrales: ", SmallFontBold));
                            cTipoComportamientoNCTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                            tblInformacionTecnica.AddCell(cTipoComportamientoNCTitulo);

                            PdfPCell cTipoComportamientoNC = new PdfPCell(new Paragraph(TipoComportamientoPrincipal.Tiponrocentrales, SmallFont));
                            cTipoComportamientoNC.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                            tblInformacionTecnica.AddCell(cTipoComportamientoNC);

                        }
                        else if (TipoComportamientoPrincipal.Tipoemprcodi == ConstantesRegistroIntegrantes.TipoComportamientoTrasmisorCodigo)
                        {

                            PdfPCell cTipoComportamientoLTTitulo = new PdfPCell(new Paragraph("Línea de Transmisión: ", SmallFontBold));
                            cTipoComportamientoLTTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                            cTipoComportamientoLTTitulo.Colspan = 2;
                            tblInformacionTecnica.AddCell(cTipoComportamientoLTTitulo);

                            PdfPCell cTipoComportamiento500KVTitulo = new PdfPCell(new Paragraph("500 kV: ", SmallFontBold));
                            cTipoComportamiento500KVTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                            tblInformacionTecnica.AddCell(cTipoComportamiento500KVTitulo);

                            PdfPCell cTipoComportamiento500KV = new PdfPCell(new Paragraph(TipoComportamientoPrincipal.Tipolineatrans500km, SmallFont));
                            cTipoComportamiento500KV.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                            tblInformacionTecnica.AddCell(cTipoComportamiento500KV);

                            PdfPCell cTipoComportamiento200KVTitulo = new PdfPCell(new Paragraph("200 kV: ", SmallFontBold));
                            cTipoComportamiento200KVTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                            tblInformacionTecnica.AddCell(cTipoComportamiento200KVTitulo);

                            PdfPCell cTipoComportamiento200KV = new PdfPCell(new Paragraph(TipoComportamientoPrincipal.Tipolineatrans220km, SmallFont));
                            cTipoComportamiento200KV.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                            tblInformacionTecnica.AddCell(cTipoComportamiento200KV);

                            PdfPCell cTipoComportamiento138KVTitulo = new PdfPCell(new Paragraph("138 kV: ", SmallFontBold));
                            cTipoComportamiento138KVTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                            tblInformacionTecnica.AddCell(cTipoComportamiento138KVTitulo);

                            PdfPCell cTipoComportamiento138KV = new PdfPCell(new Paragraph(TipoComportamientoPrincipal.Tipolineatrans138km, SmallFont));
                            cTipoComportamiento138KV.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                            tblInformacionTecnica.AddCell(cTipoComportamiento138KV);

                            PdfPCell cTipoComportamientoTLTTitulo = new PdfPCell(new Paragraph("Total Líneas de transmisión: ", SmallFontBold));
                            cTipoComportamientoTLTTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                            tblInformacionTecnica.AddCell(cTipoComportamientoTLTTitulo);

                            PdfPCell cTipoComportamientoTLT = new PdfPCell(new Paragraph(TipoComportamientoPrincipal.Tipototallineastransmision, SmallFont));
                            cTipoComportamientoTLT.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                            tblInformacionTecnica.AddCell(cTipoComportamientoTLT);

                        }
                        else if (TipoComportamientoPrincipal.Tipoemprcodi == ConstantesRegistroIntegrantes.TipoComportamientoDistribuidorCodigo)
                        {
                            PdfPCell cTipoComportamientoMDCATitulo = new PdfPCell(new Paragraph("Máxima demanda coincidente anual : ", SmallFontBold));
                            cTipoComportamientoMDCATitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                            tblInformacionTecnica.AddCell(cTipoComportamientoMDCATitulo);

                            PdfPCell cTipoComportamientoMDCA = new PdfPCell(new Paragraph(TipoComportamientoPrincipal.Tipomaxdemandacoincidente, SmallFont));
                            cTipoComportamientoMDCA.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                            tblInformacionTecnica.AddCell(cTipoComportamientoMDCA);

                        }
                        else if (TipoComportamientoPrincipal.Tipoemprcodi == ConstantesRegistroIntegrantes.TipoComportamientoUsuarioLibreCodigo)
                        {
                            PdfPCell cTipoComportamientoMDCTitulo = new PdfPCell(new Paragraph("Máxima demanda contratada (MW): ", SmallFontBold));
                            cTipoComportamientoMDCTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                            tblInformacionTecnica.AddCell(cTipoComportamientoMDCTitulo);

                            PdfPCell cTipoComportamientoMDC = new PdfPCell(new Paragraph(TipoComportamientoPrincipal.Tipomaxdemandacontratada, SmallFont));
                            cTipoComportamientoMDC.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                            tblInformacionTecnica.AddCell(cTipoComportamientoMDC);

                            PdfPCell cTipoComportamientoNSTitulo = new PdfPCell(new Paragraph("Número de Suministrador: ", SmallFontBold));
                            cTipoComportamientoNSTitulo.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                            tblInformacionTecnica.AddCell(cTipoComportamientoNSTitulo);

                            PdfPCell cTipoComportamientoNS = new PdfPCell(new Paragraph(TipoComportamientoPrincipal.Tiponumsuministrador, SmallFont));
                            cTipoComportamientoNS.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                            tblInformacionTecnica.AddCell(cTipoComportamientoNS);

                        }

                    }

                    tblContent.AddCell(table);

                    tblContent.AddCell(cTitulo);
                    tblContent.AddCell(cSubTitulo);

                    tblContent.AddCell(tblDatosEmpresa);
                    tblContent.AddCell(tblRepresentante);
                    tblContent.AddCell(tblContacto);
                    tblContent.AddCell(tblTipoIntegrante);
                    tblContent.AddCell(tblInformacionTecnica);

                    tblContent.DefaultCell.Border = Rectangle.NO_BORDER;

                    document.Add(tblContent);

                    Paragraph fechas;
                    Paragraph nombres;
                    Paragraph saltoDeLinea1;
                    if (oEmpresa.SoliFecModificacion.HasValue)
                    {
                        fechas = new Paragraph(string.Format("{0}                                                            {1}", oEmpresa.EmprfechainscripcionR != null ? ((DateTime)oEmpresa.EmprfechainscripcionR).ToString("dd/MM/yyyy") : ((DateTime)oEmpresa.Fecregistro).ToString("dd/MM/yyyy"), oEmpresa.SoliFecModificacion.Value.ToString("dd/MM/yyyy")));
                        saltoDeLinea1 = new Paragraph("");
                        nombres = new Paragraph("Fecha Inscripción                                                 Fecha Actualización");
                    }
                    else
                    {
                        fechas = new Paragraph(string.Format("{0}", oEmpresa.EmprfechainscripcionR != null ? ((DateTime)oEmpresa.EmprfechainscripcionR).ToString("dd/MM/yyyy") : ((DateTime)oEmpresa.Fecregistro).ToString("dd/MM/yyyy")));
                        saltoDeLinea1 = new Paragraph("");
                        nombres = new Paragraph("Fecha Inscripción");
                    }

                    document.Add(fechas);
                    document.Add(saltoDeLinea1);
                    document.Add(nombres);


                    document.Close();
                    writer.Close();

                    bytes = ms.GetBuffer();
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return bytes;//File(bytes, "application/pdf", FileName + ".pdf");
        }

        #endregion

        /// <summary>
        /// Devuelve un listado de empresas segun el tipo
        /// </summary>        
        /// <param name="tipoempresa">Tipo de empresa</param>
        /// <param name="tiposolicitud">Tipo solicitud</param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarEmpresasPublico(
            string tipoempresa,
            string tiposolicitud)
        {
            List<SiEmpresaDTO> lista = new List<SiEmpresaDTO>();

            lista = FactorySic.GetSiEmpresaRIRepository().ListarEmpresasPublico(
                tipoempresa,
                tiposolicitud);

            return lista;
        }



    }
}
