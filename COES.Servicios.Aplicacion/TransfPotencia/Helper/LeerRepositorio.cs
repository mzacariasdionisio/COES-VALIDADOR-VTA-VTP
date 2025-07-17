using COES.Dominio.DTO.Enum;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.TransfPotencia.Helper
{
    /// <summary>
    /// Clase, que permite leer todos lso archivos de repositorios staticos en el sistema
    /// </summary>
    public class LeerRepositorio
    {

        /// <summary>
        ///   Obtiene la carga inicial de los codigos vtp con su potencias cargadas del primer mes de ingreso produccion. EN este ejemplo se usa febrero
        /// </summary>
        /// <returns></returns>
        public List<VtpPeajeEgresoDetalleDTO> ListaCodigosPotencias()
        {
            string path = new EnumDbRepositorio(dbTypeReport.consolidadoCodigoPotencia).path;
            List<VtpPeajeEgresoDetalleDTO> resultado = new List<VtpPeajeEgresoDetalleDTO>();
            DataTable tbl = ExcelDocument.GetDataTableFromExcel(path);
            for (int i = 0; i < tbl.Rows.Count; i++)
            { 
                DataRow item = tbl.Rows[i];
                VtpPeajeEgresoDetalleDTO entidad = new VtpPeajeEgresoDetalleDTO();
                entidad.Codcncodivtp = item[0].ToString();
                entidad.Pegrdpreciopote = Convert.ToDecimal(item[6].ToString());
                entidad.Pegrdpotecoincidente = Convert.ToDecimal(item[7].ToString());
                entidad.Pegrdpoteegreso = Convert.ToDecimal(item[7].ToString());
                entidad.Pegrdpotedeclarada = Convert.ToDecimal(item[9].ToString());
                entidad.Pegrdpeajeunitario = Convert.ToDecimal(item[10].ToString());
                entidad.Pegrdfacperdida = null;
                entidad.Pegrdcalidad = item[11].ToString();
                resultado.Add(entidad);
            }
            return resultado;
        }


    }
}
