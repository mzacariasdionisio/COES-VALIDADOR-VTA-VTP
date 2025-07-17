using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Factory;
using System.Collections.Generic;
using System;
using COES.Servicios.Aplicacion.Equipamiento;
using System.Linq;

namespace COES.Servicios.Aplicacion.IEOD
{
    /// <summary>
    /// 
    /// </summary>
    public class ConfiguracionCentralAppServicio : AppServicioBase
    {
        IEODAppServicio servIeod = new IEODAppServicio();
        EquipamientoAppServicio servEquip = new EquipamientoAppServicio();

        /// <summary>
        /// Listado de la interfaz de central de color
        /// </summary>
        /// <returns></returns>
        public List<EqEquipoDTO> ListarCentralColorProp(int iEmpresa, int iFamilia, int iTipoEmpresa, int iEquipo,
            string sEstado, string nombre, int nroPagina, int nroFilas)
        {
            var lstCentales = servEquip.ListaEquipamientoPaginado(iEmpresa, iFamilia, iTipoEmpresa, iEquipo, sEstado, nombre, nroPagina, nroFilas);
            var fecha = DateTime.Today.Date;
            var listaValProiedad = new List<EqPropequiDTO>();
            if (lstCentales.Any())
            {
                var listaEquipos = lstCentales.Select(x => x.Equicodi).Distinct();
                listaValProiedad = FactorySic.GetEqPropequiRepository()
                    .ListarValoresVigentesPropiedades(fecha, string.Join(",", listaEquipos), "-1", "-1",
                    ConstantesIEOD.IDPropiedadColor + "", string.Empty, "-1");
            }

            foreach (var central in lstCentales)
            {
                if (central.B2 == null || central.B2 == "")
                {
                    var config = listaValProiedad.FirstOrDefault(x => x.Equicodi == central.Equicodi);
                    if (config != null)
                    {
                        central.B2 = config.Valor;
                    }
                    else
                    {
                        central.B2 = ConstantesIEOD.PropiedadColorDefault;
                    }
                }
            }

            return lstCentales;
        }


        /// <summary>
        /// Listado de empresas termicas
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarEmpresasTermicas()
        {
            return this.servIeod.ListarEmpresasxTipoEquipos(ConstantesIEOD.IDFamiliaTermica + "");
        }



    }
}
