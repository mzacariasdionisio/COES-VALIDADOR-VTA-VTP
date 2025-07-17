using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.Transferencias
{
    public class CentralGeneracionAppServicio : AppServicioBase
    {
        /// <summary>
        /// Permite listar todas las centarles de generación de la vista vw_eq_central_generacion
        /// </summary>
        /// <returns>Lista CentralGeneracionDTO</returns>
        public List<CentralGeneracionDTO> ListCentralGeneracion()
        {
            return FactoryTransferencia.GetCentralGeneracionRepository().List();
        }

        /// <summary>
        /// Permite listar todas las centrales de generación de la vista vw_eq_central_generacion interceptado con la tabla trn_codigo_entrega
        /// </summary>
        /// <returns>Lista CentralGeneracionDTO</returns>
        public List<CentralGeneracionDTO> ListaInterCodEnt()
        {
            return FactoryTransferencia.GetCentralGeneracionRepository().ListaInterCodEnt();
        }

        #region PrimasRER.2023
        /// <summary>
        /// Permite listar todas las centrales de generación de la vista vw_eq_central_generacion interceptado con la tabla trn_codigo_entrega segun su emprcodi
        /// </summary>
        /// <returns>Lista CentralGeneracionDTO</returns>
        public List<CentralGeneracionDTO> ListaCentralByEmpresa(int emprcodi)
        {
            return FactoryTransferencia.GetCentralGeneracionRepository().ListaCentralByEmpresa(emprcodi);
        }

        /// <summary>
        /// Permite listar todas las centrales/Unidad de la vista vw_eq_central_generacion segun su emprcodi
        /// </summary>
        /// <returns>Lista CentralGeneracionDTO</returns>
        public List<CentralGeneracionDTO> ListaCentralUnidadByEmpresa(int emprcodi)
        {
            return FactoryTransferencia.GetCentralGeneracionRepository().ListaCentralUnidadByEmpresa(emprcodi);
        }
        #endregion
        /// <summary>
        /// Permite listar todas las centrales de generación de la vista vw_eq_equipo_trn_coinfb interceptado con la tabla trn_codigo_infobase
        /// </summary>
        /// <returns>Lista CentralGeneracionDTO</returns>
        public List<CentralGeneracionDTO> ListaInterCodInfoBase()
        {
            return FactoryTransferencia.GetCentralGeneracionRepository().ListaInterCodInfoBase();
        }

        /// <summary>
        /// Permite listar todas las centarles de generación de la vista vw_eq_equipo_trn_coinfb
        /// </summary>
        /// <returns>Lista CentralGeneracionDTO</returns>
        public List<CentralGeneracionDTO> ListCentralGeneracionInfoBase()
        {
            return FactoryTransferencia.GetCentralGeneracionRepository().ListInfoBase();
        }

        /// <summary>
        /// Permite obtener la central generacion segun el nombre  de la vista vw_eq_equipo_trn_coinfb
        /// </summary>
        /// <param name="CentGeneNomb">Nombre de la empresa</param>
        /// <returns>Lista de CentralGeneracionDTO</returns>
        public CentralGeneracionDTO GetByCentGeneNomb(string CentGeneNomb)
        {
            return FactoryTransferencia.GetCentralGeneracionRepository().GetByCentGeneNombre(CentGeneNomb);
        }

        /// <summary>
        /// Permite listar todas las unidades de la vista vw_eq_central_generacion
        /// </summary>
        /// <returns>Lista CentralGeneracionDTO</returns>
        public List<CentralGeneracionDTO> ListUnidad()
        {
            return FactoryTransferencia.GetCentralGeneracionRepository().Unidad();
        }

        public List<CentralGeneracionDTO> ListUnidadCentral(int equicodicen)
        {
            return FactoryTransferencia.GetCentralGeneracionRepository().UnidadCentral(equicodicen);
        }

        /// <summary>
        /// Permite obtener la lista de empresas mas el nombre de la central generacion: IdCentral, Empresa.Central
        /// </summary>
        /// <returns>Lista de CentralGeneracionDTO</returns>
        public List<CentralGeneracionDTO> ListEmpresaCentralGeneracion()
        {
            return FactoryTransferencia.GetCentralGeneracionRepository().ListEmpresaCentralGeneracion();
        }

        /// <summary>
        /// Permite obtener la central generacion segun el nombre  y la version de recalculo en Energias Netas
        /// </summary>
        /// <param name="CentGeneNomb">Nombre de la empresa</param>
        /// <param name="strecacodi">Versión de recalculo</param>
        /// <returns>Lista de CentralGeneracionDTO</returns>
        public CentralGeneracionDTO GetByCentGeneNombVsEN(string CentGeneNomb, int strecacodi)
        {
            return FactoryTransferencia.GetCentralGeneracionRepository().GetByCentGeneNombVsEN(CentGeneNomb, strecacodi);
        }

        /// <summary>
        /// Permite obtener la central generacion o termoelectrica segun el nombre
        /// </summary>
        /// <param name="CentGeneNomb">Nombre de la empresa</param>
        /// <returns>CentralGeneracionDTO</returns>
        public CentralGeneracionDTO GetByCentGeneTermoelectricaNombre(string CentGeneNomb)
        {
            return FactoryTransferencia.GetCentralGeneracionRepository().GetByCentGeneTermoelectricaNombre(CentGeneNomb);
        }


        public EqEquipoDTO GetByCentGeneNombreEquipo(string CentGeneNomb, int vcrecacodi)
        {
            return FactoryTransferencia.GetCentralGeneracionRepository().GetByCentGeneNombreEquipo(CentGeneNomb, vcrecacodi);
        }

        public EqEquipoDTO GetByCentGeneUniNombreEquipo(string CentGeneNomb, int equicodi, int vcrecacodi)
        {
            return FactoryTransferencia.GetCentralGeneracionRepository().GetByCentGeneUniNombreEquipo(CentGeneNomb, equicodi, vcrecacodi);
        }

        public EqEquipoDTO GetByCentGeneNombreEquipoCenUni(string CentGeneNomb, int vcrecacodi)
        {
            return FactoryTransferencia.GetCentralGeneracionRepository().GetByCentGeneNombreEquipoCenUni(CentGeneNomb, vcrecacodi);
        }
    }
}
