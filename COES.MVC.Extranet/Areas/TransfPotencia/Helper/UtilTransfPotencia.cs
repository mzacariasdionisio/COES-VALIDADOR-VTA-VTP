using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Extranet.Areas.TransfPotencia.Helper
{
    public class UtilTransfPotencia
    {
        //Lista de empresas relacionados al usuario
        public static List<SeguridadServicio.EmpresaDTO> ObtenerEmpresasPorUsuario(string usuario)
        {
            SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();
            List<SeguridadServicio.EmpresaDTO> list = new List<SeguridadServicio.EmpresaDTO>();

            if ( usuario == "raul.castro" || usuario == "lvasquez" || usuario == "ppajan" || usuario == "maritza.arapa")
            {
                list = seguridad.ListarEmpresas().ToList().Where(x => x.TIPOEMPRCODI == 3 || x.EMPRCODI == 11772 || x.EMPRCODI == 13).OrderBy(x => x.EMPRNOMB).ToList();
            }
            else
            {
                list = seguridad.ObtenerEmpresasPorUsuario(usuario).ToList();
            }
            foreach (SeguridadServicio.EmpresaDTO item in list)
            {
                item.EMPRNOMB = item.EMPRNOMB.Trim();
            }
            return list;
        }

        /// <summary>
        /// Valida que la información ingresada sea un numero valido, caso contrario devuelve cero
        /// /// </summary>
        /// <param name="sValor">Cadena de texto</param>
        public static decimal ValidarNumero(string sValor)
        {
            decimal dNumero;
            if (!sValor.Equals("") && (Decimal.TryParse(sValor, out dNumero)))
            {
                return dNumero;
            }
            else
            {
                return 0;
            }
        }
    }
}