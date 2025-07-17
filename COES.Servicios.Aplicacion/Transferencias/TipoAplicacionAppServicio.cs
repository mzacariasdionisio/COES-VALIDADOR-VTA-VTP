using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Indisponibilidades;
using COES.Servicios.Aplicacion.SIOSEIN;
using COES.Servicios.Aplicacion.Transferencias;
using COES.Servicios.Aplicacion.TransfPotencia.Helper;
using log4net;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.Transferencias
{
    public class TipoAplicacionAppServicio: AppServicioBase
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(VariacionEmpresaAppServicio));
        /// <summary>
        /// Permite obtener la variacion de empresa por tipo de comparación

        /// <summary>
        /// Permite obtener una lista de empresas con sus respectivos porcentajes de variacion
        /// </summary>
        /// <param name="varemptipocomp">Tipo de comp</param>
        /// <returns>Lista de ValorTransferenciaDTO</returns> 
        public List<VtpTipoAplicacionDTO> ListTipoAplicacion()
        {
            return FactoryTransferencia.GetVtpTipoAplicacionRepository().ListTipoAplicacion();
        }
    }
}
