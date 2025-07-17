using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.ServicioRPF
{
    public class ComparativoAppServicio : AppServicioBase
    {
        /// <summary>
        /// Permite obtener las empresas
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ObtenerEmpresas()
        {
            return FactorySic.GetMePtorelacionRepository().ObtenerEmpresas();
        }

        /// <summary>
        /// Permite obtener las centrales relacionadas
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public List<EqEquipoDTO> ObtenerCentrales(int idEmpresa, DateTime fechaPeriodo)
        {
            return FactorySic.GetMePtorelacionRepository().ListarCentrales(idEmpresa, fechaPeriodo);
        }

        #region Equivalencias entre puntos de medicion

        /// <summary>
        /// Permite obtener el listado de equivalencias
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idCentral"></param>
        /// <returns></returns>
        public string ObtenerRelacion(int? idEmpresa, int? idCentral, string ruta)
        {
            if (idEmpresa == null) idEmpresa = -1;
            if (idCentral == null) idCentral = -1;

            StringBuilder str = new StringBuilder();

            str.Append("<table class='pretty tabla-icono'>");
            str.Append("    <thead>");
            str.Append("        <tr>");
            str.Append("            <th>Empresa</th>");
            str.Append("            <th>Central</th>");
            str.Append("            <th style='background-color:#61c13a'>Ptos. Servicio RPF</th>");
            str.Append("            <th style='background-color: rgb(239, 164, 8)'>Ptos. Despacho Ejecutado</th>");
            str.Append("            <th>Editar</th>");
            str.Append("        </tr>");
            str.Append("    </thead>");
            str.Append("    <tbody>");

            List<MePtorelacionDTO> list = FactorySic.GetMePtorelacionRepository().GetByCriteria(idEmpresa, idCentral);

            var empresas = list.Select(x => new { x.Emprcodi, x.Emprnomb }).Distinct().ToList();
            int index = 0;
            foreach (var empresa in empresas)
            {
                var centrales = list.Where(x => x.Emprcodi == empresa.Emprcodi).Select(x => new { x.Centralcodi, x.Centralnomb }).Distinct().ToList();

                int indice = 0;
                foreach (var central in centrales)
                {
                    var puntos = list.Where(x => x.Centralcodi == central.Centralcodi).ToList();
                    var puntosRpf = puntos.Where(x => x.Origlectcodi == 1).ToList();
                    var puntosDespacho = puntos.Where(x => x.Origlectcodi == 2).ToList();

                    var liRpf = "";
                    foreach (var puntoRpf in puntosRpf)
                    {
                        liRpf = liRpf + "<li>" + puntoRpf.Ptomedicodi + " " + puntoRpf.Equinomb + "</li>";
                    }
                    //liRpf = liRpf + "</ul>";

                    var liDespacho = "";
                    foreach (var puntoDespacho in puntosDespacho)
                    {
                        liDespacho = liDespacho + "<li>" + puntoDespacho.Ptomedicodi + " " + puntoDespacho.Equinomb + "</li>";
                    }
                    //liDespacho = liDespacho + "</ul>";

                    string style = "style='background-color: #f2f5f7'";
                    if (index % 2 == 0) style = "";

                    str.AppendFormat("        <tr>");
                    if (indice == 0)
                    {
                        str.AppendFormat("            <td rowspan='{1}' {2}>{0}</td>", empresa.Emprnomb, centrales.Count,style);
                    }
                    str.AppendFormat("            <td {1}>{0}</td>", central.Centralnomb, style);
                    str.AppendFormat("            <td {1}>{0}</td>", liRpf, style);
                    str.AppendFormat("            <td {1}>{0}</td>", liDespacho, style);
                    str.AppendFormat("            <td {1}><a href='JavaScript:editarRelacion({0})'><img src='{2}Content/Images/btn-edit.png' alt='' /></a></td>", central.Centralcodi, style, ruta);

                    str.Append("        </tr>");

                    indice++;
                    index++;
                }
            }

            str.Append("    </tbody>");
            str.Append("</table>");

            return str.ToString();
        }

        /// <summary>
        /// Permite obtener las relaciones de una central
        /// </summary>
        /// <param name="idCentral"></param>
        /// <returns></returns>
        public List<MePtorelacionDTO> ObtenerConfiguracion(int idCentral, out EqEquipoDTO equipo, out SiEmpresaDTO empresa)
        {
            equipo = FactorySic.GetEqEquipoRepository().GetById(idCentral);
            empresa = FactorySic.GetSiEmpresaRepository().GetById((int)equipo.Emprcodi);
            return FactorySic.GetMePtorelacionRepository().List(idCentral);
        }

        /// <summary>
        /// Permite grabar la relaciones de puntos de medicion
        /// </summary>
        /// <param name="idCentral"></param>
        /// <param name="idsRpf"></param>
        /// <param name="idsDespacho"></param>
        public void GrabarEquivalencia(int idCentral, string idsRpf, string idsDespacho, string usuario)
        {
            try
            {
                FactorySic.GetMePtorelacionRepository().Delete(idCentral);
                this.GrabarPunto(idCentral, idsRpf, 1, usuario);
                this.GrabarPunto(idCentral, idsDespacho, 2, usuario);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Graba los puntos de la relacion
        /// </summary>
        /// <param name="idCentral"></param>
        /// <param name="cadena"></param>
        /// <param name="origlectcodi"></param>
        /// <param name="usuario"></param>
        private void GrabarPunto(int idCentral, string cadena, int origlectcodi, string usuario)
        {
            if (!string.IsNullOrEmpty(cadena))
            {
                List<int> list = cadena.Split(ConstantesAppServicio.CaracterComa).Select(int.Parse).ToList();

                foreach (int id in list)
                {
                    MePtorelacionDTO entity = new MePtorelacionDTO
                    {
                        Equicodi = idCentral,
                        Ptorelpunto1 = (origlectcodi == 1) ? id : -1,
                        Ptorelpunto2 = (origlectcodi == 2) ? id : -1,
                        Lastuser = usuario,
                        Lastdate = DateTime.Now
                    };

                    FactorySic.GetMePtorelacionRepository().Save(entity);
                }
            }
        }

        /// <summary>
        /// Permite obtener los puntos de medición
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idCentral"></param>
        /// <returns></returns>
        public void ObtenerPuntosMedicion(int? idEmpresa, int? idCentral, out List<int> puntosRpf, out List<int> puntosDespacho, DateTime fechaPeriodo)
        {
            if (idEmpresa == null) idEmpresa = -1;
            if (idCentral == null) idCentral = -1;

            List<MePtorelacionDTO> list = FactorySic.GetMePtorelacionRepository().ObtenerPuntosRelacion(idEmpresa, idCentral, fechaPeriodo);

            puntosRpf = list.Where(x => x.Origlectcodi == 1).Select(x => (int)x.Ptomedicodi).Distinct().ToList();
            puntosDespacho = list.Where(x => x.Origlectcodi == 2).Select(x => (int)x.Ptomedicodi).Distinct().ToList();
        }

        /// <summary>
        /// Permite obtener los puntos de medición del servicio RPF
        /// </summary>
        /// <returns></returns>
        public List<MePtorelacionDTO> ObtenerPuntosRPF(DateTime fechaPeriodo)
        {
            return FactorySic.GetMePtorelacionRepository().ObtenerPuntosRPF(fechaPeriodo);
        }

        #endregion

        /// <summary>
        /// Permite obtener los datos del despacho ejecutado
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="puntosMedicion"></param>
        /// <returns></returns>
        public MeMedicion48DTO ObtenerDatosDespacho(DateTime fecha, string puntosMedicion)
        {
            return FactorySic.GetMePtorelacionRepository().ObtenerDatosDespacho(fecha, puntosMedicion);            
        }
    }
}
