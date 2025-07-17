using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.TransfPotencia.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.Transferencias
{
    public class CodigoRetiroGeneradoAppServicio
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<CodigoRetiroGeneradoDTO> ListarCodigosGeneradoVTPExtranet()
        {
            return FactoryTransferencia.GetCodigoRetiroGeneradoRepository().ListarCodigosGeneradoVTPExtranet();
        }

        /// <summary>
        /// Permite generar el archivo de exportación de la tabla VTP_PEAJE_EGRESO
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <param name="pegrcodi">Código del Peaje Egreso</param>
        /// <param name="formato">Excel=1 / PDF=2 / Word=3</param>
        /// <param name="pathFile">Rutal del archivo de descarga</param>
        /// <param name="pathLogo">Ruta del Logo del COES</param>
        /// <returns></returns>
        public string GenerarFormatoPeajeEgresoExtranet(int pericodi, int recpotcodi, int pegrcodi, int formato, string pathFile, string pathLogo)
        {
            BarraAppServicio servicioBarra = new BarraAppServicio();
            EmpresaAppServicio servicioEmpresa = new EmpresaAppServicio();

            string fileName = string.Empty;
            VtpRecalculoPotenciaDTO EntidadRecalculoPotencia = this.GetByIdVtpRecalculoPotenciaView(pericodi, recpotcodi);
            List<CodigoRetiroGeneradoDTO> ListaCodigoRetiroGenerado = this.ListarCodigosGeneradoVTPExtranet();
            List<BarraDTO> ListaBarras = servicioBarra.ListVista();
            List<COES.Dominio.DTO.Transferencias.EmpresaDTO> ListaEmpresas = servicioEmpresa.ListEmpresas();
            if (formato == 1)
            {
                fileName = "ReportePeajeEgresoExtranet.xlsx";
                ExcelDocument.GenerarFormatoPeajeEgresoExtranet(pathFile + fileName, EntidadRecalculoPotencia, ListaCodigoRetiroGenerado, ListaEmpresas, ListaBarras);
            }

            return fileName;
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VTP_RECALCULO_POTENCIA
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <returns>VtpRecalculoPotenciaDTO</returns>
        public VtpRecalculoPotenciaDTO GetByIdVtpRecalculoPotenciaView(int pericodi, int recpotcodi)
        {
            return FactoryTransferencia.GetVtpRecalculoPotenciaRepository().GetByIdView(pericodi, recpotcodi);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="barrcodisum"></param>
        /// <param name="genemprcodi"></param>
        /// <param name="cliemprcodi"></param>
        /// <returns></returns>
        public List<CodigoRetiroGeneradoDTO> ListarCodigosVTPByEmpBar(int barrcodisum, int genemprcodi, int cliemprcodi)
        {
            return FactoryTransferencia.GetCodigoRetiroGeneradoRepository().ListarCodigosVTPByEmpBar(barrcodisum, genemprcodi, cliemprcodi);
        }

        public CodigoRetiroGeneradoDTO GetByCodigoVTP(string codigovtp)
        {
            return FactoryTransferencia.GetCodigoRetiroGeneradoRepository().GetByCodigoVTP(codigovtp);
        }
    }
}
