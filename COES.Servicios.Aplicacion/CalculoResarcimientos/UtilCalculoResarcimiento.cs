using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Helper;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.CalculoResarcimientos
{
    public class UtilCalculoResarcimiento
    {
        public List<int> IdsInterrupcion = new List<int>();
        public List<int> IdsRechazoCarga = new List<int>();
        /// <summary>
        /// Permite validar lo datos del evento
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="index"></param>
        /// <param name="empresa"></param>
        /// <param name="porcentaje"></param>
        /// <param name="flag"></param>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static List<string> ValidarEvento(List<ReEmpresaDTO> empresas, int index, string empresa, 
            string porcentaje, out bool flag, out decimal? valor, out int? idEmpresa, int nroResponsable)
        {
            List<string> errores = new List<string>();
            valor = null;
            decimal valPorc = 0;
            idEmpresa = null;
            flag = true;

            if (!string.IsNullOrEmpty(empresa) && !string.IsNullOrEmpty(porcentaje))
            {
                ReEmpresaDTO entityEmpresa = empresas.Where(x => x.Emprnomb.Trim() == empresa.Trim()).FirstOrDefault();
                if (entityEmpresa == null)
                {
                    errores.Add("El responsable " + nroResponsable + " de la fila " + index + " no existe en la base de datos.");
                    flag = false;
                }
                else
                {
                    idEmpresa = entityEmpresa.Emprcodi;
                }

                if (!decimal.TryParse(porcentaje, out valPorc))
                {
                    errores.Add("El porcentaje del responsable " + nroResponsable +  " de la fila " + index + " no tiene formato numérico.");
                    flag = false;
                }
                else
                {
                    valor = valPorc;
                }
            }
            else
            {
                if ((string.IsNullOrEmpty(empresa) && !string.IsNullOrEmpty(porcentaje)) ||
                        (!string.IsNullOrEmpty(empresa) && string.IsNullOrEmpty(porcentaje)))
                {
                    errores.Add("Debe ingresar tanto el responsable " + nroResponsable + " como su porcentaje en la fila " + index);
                    flag = false;
                }
            }

            return errores;

        }

        /// <summary>
        /// Permite obtener la estructura de la plantilla de interrupciones
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<string> ObtenerEstructuraPlantillaInterrupcuones(string path)
        {
            List<string> result = new List<string>();
            FileInfo template = new FileInfo(path + ConstantesCalculoResarcimiento.PlantillaInterrupcion);

            using (ExcelPackage xlPackage = new ExcelPackage(template))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets["Interrupciones"];

                for (int i = 1; i <= 32; i++)
                {
                    result.Add((ws.Cells[1, i].Value != null) ? ws.Cells[1, i].Value.ToString() : string.Empty);
                }

            }
            return result;
        }

        /// <summary>
        /// Permite obtener la estructura de la plantilla de interrupciones
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<string> ObtenerEstructuraPlantillaInterrupcionesInsumo(string path)
        {
            List<string> result = new List<string>();
            FileInfo template = new FileInfo(path + ConstantesCalculoResarcimiento.PlantillaCargaInterrupcionInsumo);

            using (ExcelPackage xlPackage = new ExcelPackage(template))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets["Interrupciones"];

                for (int i = 1; i <= 24; i++)
                {
                    result.Add((ws.Cells[1, i].Value != null) ? ws.Cells[1, i].Value.ToString() : string.Empty);
                }

            }
            return result;
        }

        /// <summary>
        /// Permite obtener la estructura de la plantilla de interrupciones
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<string> ObtenerEstructuraPlantillaRespuestas(string path)
        {
            List<string> result = new List<string>();
            FileInfo template = new FileInfo(path + ConstantesCalculoResarcimiento.PlantillaRespuesta);

            using (ExcelPackage xlPackage = new ExcelPackage(template))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets["Interrupciones"];

                for (int i = 1; i <= 72; i++)
                {
                    result.Add((ws.Cells[1, i].Value != null) ? ws.Cells[1, i].Value.ToString() : string.Empty);
                }

            }
            return result;
        }

        /// <summary>
        /// Permite obtener la estructura de la plantilla de interrupciones
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<string> ObtenerEstructuraPlantillaRechazoCarga(string path)
        {
            List<string> result = new List<string>();
            FileInfo template = new FileInfo(path + ConstantesCalculoResarcimiento.PlantillaRechazoCarga);

            using (ExcelPackage xlPackage = new ExcelPackage(template))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets["RechazoCarga"];

                for (int i = 1; i <= 15; i++)
                {
                    result.Add((ws.Cells[1, i].Value != null) ? ws.Cells[1, i].Value.ToString() : string.Empty);
                }

            }
            return result;
        }

        /// <summary>
        /// Permite obtener la estructura de la plantilla de interrupciones
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<string> ObtenerEstructuraPlantillaObservacion(string path)
        {
            List<string> result = new List<string>();
            FileInfo template = new FileInfo(path + ConstantesCalculoResarcimiento.PlantillaObservacion);

            using (ExcelPackage xlPackage = new ExcelPackage(template))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets["Interrupciones"];

                for (int i = 1; i <= 36; i++)
                {
                    result.Add((ws.Cells[1, i].Value != null) ? ws.Cells[1, i].Value.ToString() : string.Empty);
                }

            }
            return result;
        }


        /// <summary>
        /// Permite obtener la estructura de la plantilla de interrupciones
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<string> ObtenerEstructuraPlantillaConsolidadoInterrupcuones(string path)
        {
            List<string> result = new List<string>();
            FileInfo template = new FileInfo(path + ConstantesCalculoResarcimiento.PlantillaConsolidadoInterrupcion);

            using (ExcelPackage xlPackage = new ExcelPackage(template))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets["Interrupciones"];

                for (int i = 1; i <= 74; i++)
                {
                    result.Add((ws.Cells[1, i].Value != null) ? ws.Cells[1, i].Value.ToString() : string.Empty);
                }

            }
            return result;
        }

        /// <summary>
        /// Permite obtener la estructura de la plantilla de interrupciones
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<string> ObtenerEstructuraPlantillaConsolidadoRechazoCarga(string path)
        {
            List<string> result = new List<string>();
            FileInfo template = new FileInfo(path + ConstantesCalculoResarcimiento.PlantilaaConsolidadoRechazoCarga);

            using (ExcelPackage xlPackage = new ExcelPackage(template))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets["Interrupciones"];

                for (int i = 1; i <= 31; i++)
                {
                    result.Add((ws.Cells[1, i].Value != null) ? ws.Cells[1, i].Value.ToString() : string.Empty);
                }

            }
            return result;
        }

        /// <summary>
        /// Permite comparar los registros de interrupciones
        /// </summary>
        /// <param name="nuevo"></param>
        /// <param name="anterior"></param>
        /// <returns></returns>
        public static bool CompararRegistroInterrupcion(ReInterrupcionSuministroDTO nuevo, ReInterrupcionSuministroDTO anterior)
        {
            if (
                nuevo.Reintcorrelativo != anterior.Reintcorrelativo ||
                nuevo.Reinttipcliente != anterior.Reinttipcliente ||
                nuevo.Reintcliente != anterior.Reintcliente ||
                nuevo.Repentcodi != anterior.Repentcodi ||
                nuevo.Reintptoentrega != anterior.Reintptoentrega ||
                //nuevo.Reintnrosuministro != anterior.Reintnrosuministro ||
                (((string.IsNullOrEmpty(nuevo.Reintnrosuministro)) ? "" : nuevo.Reintnrosuministro) != (string.IsNullOrEmpty(anterior.Reintnrosuministro) ? "" : anterior.Reintnrosuministro)) ||
                nuevo.Rentcodi != anterior.Rentcodi ||
                nuevo.Reintaplicacionnumeral != anterior.Reintaplicacionnumeral ||
                (nuevo.Reintenergiasemestral - anterior.Reintenergiasemestral != 0) ||
                nuevo.Reintinctolerancia != anterior.Reintinctolerancia ||
                nuevo.Retintcodi != anterior.Retintcodi ||
                nuevo.Recintcodi != anterior.Recintcodi ||
                nuevo.Reintni != anterior.Reintni ||
                nuevo.Reintki != anterior.Reintki ||
                nuevo.Reintfejeinicio != anterior.Reintfejeinicio ||
                nuevo.Reintfejefin != anterior.Reintfejefin ||
                nuevo.Reintfproginicio != anterior.Reintfproginicio ||
                nuevo.Reintfprogfin != anterior.Reintfprogfin ||
                nuevo.Emprcodi1 != anterior.Emprcodi1 ||
                nuevo.Porcentaje1 != anterior.Porcentaje1 ||
                nuevo.Emprcodi2 != anterior.Emprcodi2 ||
                nuevo.Porcentaje2 != anterior.Porcentaje2 ||
                nuevo.Emprcodi3 != anterior.Emprcodi3 ||
                nuevo.Porcentaje3 != anterior.Porcentaje3 ||
                nuevo.Emprcodi4 != anterior.Emprcodi4 ||
                nuevo.Porcentaje4 != anterior.Porcentaje4 ||
                nuevo.Emprcodi5 != anterior.Emprcodi5 ||
                nuevo.Porcentaje5 != anterior.Porcentaje5 ||
                (((string.IsNullOrEmpty(nuevo.Reintcausaresumida))?"": nuevo.Reintcausaresumida) != (string.IsNullOrEmpty(anterior.Reintcausaresumida)?"": anterior.Reintcausaresumida)) ||
                nuevo.Reinteie != anterior.Reinteie ||
                nuevo.Reintresarcimiento != anterior.Reintresarcimiento ||
                nuevo.Reintevidencia != anterior.Reintevidencia
                )
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// Permite comparar los registros de interrupciones
        /// </summary>
        /// <param name="nuevo"></param>
        /// <param name="anterior"></param>
        /// <returns></returns>
        public static bool CompararRegistroInterrupcionRegistro(ReInterrupcionSuministroDTO nuevo, ReInterrupcionSuministroDTO anterior)
        {
            if (
                nuevo.Reintcorrelativo != anterior.Reintcorrelativo ||
                nuevo.Reinttipcliente != anterior.Reinttipcliente ||
                nuevo.Reintcliente != anterior.Reintcliente ||
                nuevo.Repentcodi != anterior.Repentcodi ||
                nuevo.Reintptoentrega != anterior.Reintptoentrega ||
                //nuevo.Reintnrosuministro != anterior.Reintnrosuministro ||
                 (((string.IsNullOrEmpty(nuevo.Reintnrosuministro)) ? "" : nuevo.Reintnrosuministro) != (string.IsNullOrEmpty(anterior.Reintnrosuministro) ? "" : anterior.Reintnrosuministro)) ||
                nuevo.Rentcodi != anterior.Rentcodi ||
                nuevo.Reintaplicacionnumeral != anterior.Reintaplicacionnumeral ||
                //nuevo.Reintenergiasemestral != anterior.Reintenergiasemestral ||
                nuevo.Reintinctolerancia != anterior.Reintinctolerancia ||
                nuevo.Retintcodi != anterior.Retintcodi ||
                nuevo.Recintcodi != anterior.Recintcodi ||
                nuevo.Reintni != anterior.Reintni ||
                nuevo.Reintki != anterior.Reintki ||
                nuevo.Reintfejeinicio != anterior.Reintfejeinicio ||
                nuevo.Reintfejefin != anterior.Reintfejefin ||
                nuevo.Reintfproginicio != anterior.Reintfproginicio ||
                nuevo.Reintfprogfin != anterior.Reintfprogfin ||
                nuevo.Emprcodi1 != anterior.Emprcodi1 ||
                nuevo.Porcentaje1 != anterior.Porcentaje1 ||
                nuevo.Emprcodi2 != anterior.Emprcodi2 ||
                nuevo.Porcentaje2 != anterior.Porcentaje2 ||
                nuevo.Emprcodi3 != anterior.Emprcodi3 ||
                nuevo.Porcentaje3 != anterior.Porcentaje3 ||
                nuevo.Emprcodi4 != anterior.Emprcodi4 ||
                nuevo.Porcentaje4 != anterior.Porcentaje4 ||
                nuevo.Emprcodi5 != anterior.Emprcodi5 ||
                nuevo.Porcentaje5 != anterior.Porcentaje5 ||
                 (((string.IsNullOrEmpty(nuevo.Reintcausaresumida)) ? "" : nuevo.Reintcausaresumida) != (string.IsNullOrEmpty(anterior.Reintcausaresumida) ? "" : anterior.Reintcausaresumida)) ||
                //nuevo.Reinteie != anterior.Reinteie ||
                //nuevo.Reintresarcimiento != anterior.Reintresarcimiento ||
                nuevo.Reintevidencia != anterior.Reintevidencia
                )
            {
                return false;
            }
            return true;
        }

        /// Valida si existen cambios a nivel de conformidad de suministrador
        /// </summary>
        /// <param name="det1"></param>
        /// <param name="det2"></param>
        /// <param name="det3"></param>
        /// <param name="det4"></param>
        /// <param name="det5"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool CompararConformidadSuministrador(ReInterrupcionSuministroDetDTO det1,
            ReInterrupcionSuministroDetDTO det2,
            ReInterrupcionSuministroDetDTO det3,
            ReInterrupcionSuministroDetDTO det4,
            ReInterrupcionSuministroDetDTO det5, string[] data)
        {

            string[] original = new string[5];
            string[] final = new string[5];

            original[0] = (det1 != null) ? (!string.IsNullOrEmpty(det1.Reintdconformidadsumioriginal)) ? det1.Reintdconformidadsumioriginal : string.Empty : string.Empty;
            original[1] = (det2 != null) ? (!string.IsNullOrEmpty(det2.Reintdconformidadsumioriginal)) ? det2.Reintdconformidadsumioriginal : string.Empty : string.Empty;
            original[2] = (det3 != null) ? (!string.IsNullOrEmpty(det3.Reintdconformidadsumioriginal)) ? det3.Reintdconformidadsumioriginal : string.Empty : string.Empty;
            original[3] = (det4 != null) ? (!string.IsNullOrEmpty(det4.Reintdconformidadsumioriginal)) ? det4.Reintdconformidadsumioriginal : string.Empty : string.Empty;
            original[4] = (det5 != null) ? (!string.IsNullOrEmpty(det5.Reintdconformidadsumioriginal)) ? det5.Reintdconformidadsumioriginal : string.Empty : string.Empty;

            final[0] = data[38];
            final[1] = data[38 + 1 * 8];
            final[2] = data[38 + 2 * 8];
            final[3] = data[38 + 3 * 8];
            final[4] = data[38 + 4 * 8];

            for (int i = 0; i < 5; i++)
            {
                if (original[i] != final[i] && final[i] == ConstantesAppServicio.SI)
                {                    
                    return false;
                }
            }

            return true;
        }


        /// <summary>
        /// Permite comparar los registros de interrupciones
        /// </summary>
        /// <param name="nuevo"></param>
        /// <param name="anterior"></param>
        /// <returns></returns>
        public static bool CompararRegistroRechazoCarga(ReRechazoCargaDTO nuevo, ReRechazoCargaDTO anterior)
        {
            if (
                nuevo.Rerccorrelativo != anterior.Rerccorrelativo ||
                nuevo.Rerctipcliente != anterior.Rerctipcliente ||
                nuevo.Rerccliente != anterior.Rerccliente ||
                nuevo.Repentcodi != anterior.Repentcodi ||
                //nuevo.Rercptoentrega != anterior.Rercptoentrega ||
                (((string.IsNullOrEmpty(nuevo.Rercptoentrega)) ? "" : nuevo.Rercptoentrega) != (string.IsNullOrEmpty(anterior.Rercptoentrega) ? "" : anterior.Rercptoentrega)) ||
                nuevo.Rercalimentadorsed != anterior.Rercalimentadorsed ||
                //nuevo.Rercenst != anterior.Rercenst ||
                (nuevo.Rercenst - anterior.Rercenst != 0) ||
                nuevo.Reevecodi != anterior.Reevecodi ||
                //nuevo.Rerccomentario != anterior.Rerccomentario ||
                (((string.IsNullOrEmpty(nuevo.Rerccomentario)) ? "" : nuevo.Rerccomentario) != (string.IsNullOrEmpty(anterior.Rerccomentario) ? "" : anterior.Rerccomentario)) ||
                nuevo.Rerctejecinicio != anterior.Rerctejecinicio ||
                nuevo.Rerctejecfin != anterior.Rerctejecfin ||
                //nuevo.Rercpk != anterior.Rercpk ||
                (nuevo.Rercpk - anterior.Rercpk != 0) ||
                nuevo.Rerccompensable != anterior.Rerccompensable ||
                //nuevo.Rercens != anterior.Rercens ||
                (nuevo.Rercens - anterior.Rercens != 0) ||
                //nuevo.Rercresarcimiento != anterior.Rercresarcimiento ||
                (nuevo.Rercresarcimiento - anterior.Rercresarcimiento != 0)
                )
            {
                return false;
            }
            return true;
        }

        public static void setaerDatosRespuesta(ReInterrupcionSuministroDetDTO entity1, ReInterrupcionSuministroDetDTO entity2)
        {
            if (entity2 != null)
            {
                entity1.Reintdconformidadresp = entity2.Reintdconformidadresp;
                entity1.Reintdobservacionresp = entity2.Reintdobservacionresp;
                entity1.Reintddetalleresp = entity2.Reintddetalleresp;
                entity1.Reintdcomentarioresp = entity2.Reintdcomentarioresp;
                entity1.Reintdevidenciaresp = entity2.Reintdevidenciaresp;
                entity1.Reintdconformidadsumi = entity2.Reintdconformidadsumi;
                entity1.Reintdcomentariosumi = entity2.Reintdcomentariosumi;
                entity1.Reintdevidenciasumi = entity2.Reintdevidenciasumi;
            }
        }

        /// <summary>
        /// Permite obtener los tipos de observaciones
        /// </summary>
        /// <returns></returns>
        public static List<TipoObservacion> ListarTiposObservacion()
        {
            List<TipoObservacion> entitys = new List<TipoObservacion>();
            entitys.Add(new TipoObservacion { Id = "1", Texto = "En trámite de FM o FM Fundada" });
            entitys.Add(new TipoObservacion { Id = "2", Texto = "Exonerado por Expansión o Reforzamiento" });
            entitys.Add(new TipoObservacion { Id = "3", Texto = "No se tiene registro de interrupción" });
            entitys.Add(new TipoObservacion { Id = "4", Texto = "Campo observado Nivel de Tensión" });
            entitys.Add(new TipoObservacion { Id = "5", Texto = "Campo observado Causa o Tipo" });
            entitys.Add(new TipoObservacion { Id = "6", Texto = "Campo observado fecha y hora" });
            entitys.Add(new TipoObservacion { Id = "7", Texto = "Campo observado responsable o porcentaje de responsabilidad" });
            entitys.Add(new TipoObservacion { Id = "8", Texto = "Incremento de Tolerancias Sector Típico de Distribución" });
            entitys.Add(new TipoObservacion { Id = "9", Texto = "Energía semestral observada" });
            entitys.Add(new TipoObservacion { Id = "10", Texto = "Otras Observaciones" });

            return entitys;
        }

        /// <summary>
        /// Permite obtener la data
        /// </summary>
        /// <param name="det1"></param>
        /// <param name="det2"></param>
        /// <param name="det3"></param>
        /// <param name="det4"></param>
        /// <param name="det5"></param>
        /// <returns></returns>
        public static string[] ObtenerCamposObservacion(ReInterrupcionSuministroDetDTO det1,
            ReInterrupcionSuministroDetDTO det2,
            ReInterrupcionSuministroDetDTO det3,
            ReInterrupcionSuministroDetDTO det4,
            ReInterrupcionSuministroDetDTO det5,
            List<TipoObservacion> listaTipoObservacion
            )
        {
            List<string> arreglo = new List<string>();
            arreglo.AddRange(ObtenerArregloDetalle(det1, listaTipoObservacion));
            arreglo.AddRange(ObtenerArregloDetalle(det2, listaTipoObservacion));
            arreglo.AddRange(ObtenerArregloDetalle(det3, listaTipoObservacion));
            arreglo.AddRange(ObtenerArregloDetalle(det4, listaTipoObservacion));
            arreglo.AddRange(ObtenerArregloDetalle(det5, listaTipoObservacion));
            return arreglo.ToArray();
        }

        /// <summary>
        /// Permite obtener los datos de observaciones y respuestas
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static string[] ObtenerArregloDetalle(ReInterrupcionSuministroDetDTO entity,
            List<TipoObservacion> listaTipoObservacion)
        {
            string[] arreglo = new string[8];

            if(entity != null)
            {
                arreglo[0] = (!string.IsNullOrEmpty(entity.Reintdconformidadresp)) ?
                    (entity.Reintdconformidadresp == ConstantesAppServicio.SI) ?
                    ConstantesCalculoResarcimiento.TextoSi : ConstantesCalculoResarcimiento.TextoNo : string.Empty;

                if (!string.IsNullOrEmpty(entity.Reintdobservacionresp))
                {
                    List<int> idsObservaciones = entity.Reintdobservacionresp.Split(',').Select(int.Parse).ToList();
                    List<string> textosObservaciones = listaTipoObservacion.Where(x => idsObservaciones.Any(y => int.Parse(x.Id) == y)).Select(x => x.Texto).ToList();
                    arreglo[1] = string.Join(",", textosObservaciones);
                }
                else
                {
                    arreglo[1] = string.Empty;
                }                   


                arreglo[2] = (!string.IsNullOrEmpty(entity.Reintddetalleresp))?entity.Reintddetalleresp: string.Empty;
                arreglo[3] = (!string.IsNullOrEmpty(entity.Reintdcomentarioresp)) ? entity.Reintdcomentarioresp : string.Empty;
                arreglo[4] = (!string.IsNullOrEmpty(entity.Reintdevidenciaresp)) ? entity.Reintdevidenciaresp : string.Empty;
                arreglo[5] = (!string.IsNullOrEmpty(entity.Reintdconformidadsumi)) ? entity.Reintdconformidadsumi : string.Empty;
                arreglo[6] = (!string.IsNullOrEmpty(entity.Reintdcomentariosumi)) ? entity.Reintdcomentariosumi : string.Empty;
                arreglo[7] = (!string.IsNullOrEmpty(entity.Reintdevidenciasumi)) ? entity.Reintdevidenciasumi : string.Empty;
            }
            else
            {
                for(int i = 0; i<8; i++)
                {
                    arreglo[i] = string.Empty;
                }
            }

            return arreglo;
        }

        /// <summary>
        /// Permite obtener la grilla de consolidado de interrupciones
        /// </summary>
        /// <param name="entitys"></param>
        /// <param name="conformidadResponsable"></param>
        /// <param name="conformidadSuministrador"></param>
        /// <param name="buscar"></param>
        /// <returns></returns>
        public static string[][] ObtenerConsolidadoInterrupciones(List<ReInterrupcionSuministroDTO> entitys, string conformidadResponsable,
            string conformidadSuministrador, string buscar)
        {
            List<string[]> result = new List<string[]>();
            List<int> ids = entitys.Select(x => x.Reintcodi).Distinct().ToList();
            List<TipoObservacion> listaTipos = UtilCalculoResarcimiento.ListarTiposObservacion();


            foreach (int id in ids)
            {
                List<ReInterrupcionSuministroDTO> detalle = entitys.Where(x => x.Reintcodi == id).OrderBy(x => x.OrdenDetalle).ToList();

                //- Filtramos conformidad suministrador y responsable
                int contador = detalle.Where(x => (x.Reintdconformidadresp == conformidadResponsable || string.IsNullOrEmpty(conformidadResponsable))
                && (x.Reintdconformidadsumi == conformidadSuministrador || string.IsNullOrEmpty(conformidadSuministrador))).Count();
                
                if (contador > 0)
                {
                    string[] data = new string[86];
                    data[0] = detalle[0].Reintcodi.ToString() + "-" + detalle[0].Reintestado + "-" + detalle[0].Reintreftrimestral; //- Id
                    data[1] = string.Empty; //- Opciones
                    data[2] = detalle[0].Emprnomb; //- Suministrador
                    data[3] = detalle[0].Reintcorrelativo.ToString(); //- Correlativo
                    data[4] = detalle[0].Reinttipcliente; //- Tipo de cliente
                    data[5] = detalle[0].Cliente; //- Cliente
                    data[6] = detalle[0].Reintptoentrega; //- Pto de entrega
                    data[7] = (detalle[0].Reintnrosuministro != null) ? detalle[0].Reintnrosuministro.ToString() : String.Empty; //- Nro suministro
                    data[8] = detalle[0].NivelTension; //- Nivel de tension
                    data[9] = detalle[0].Reintaplicacionnumeral.ToString(); //- Aplicacion literal
                    data[10] = (detalle[0].Reintenergiasemestral != null) ? detalle[0].Reintenergiasemestral.ToString() : string.Empty; //- Energia semestral
                    data[11] = detalle[0].Reintinctolerancia; //- Incremento de tolerancias
                    data[12] = detalle[0].TipoInterrupcion; //- Tipo de interrupcion
                    data[13] = detalle[0].CausaInterrupcion; //- Causa de interrupcion
                    data[14] = detalle[0].Reintni.ToString(); //- Indicador NI
                    data[15] = detalle[0].Reintki.ToString(); //- Indicador KI
                    data[16] = ((DateTime)detalle[0].Reintfejeinicio).ToString(ConstantesAppServicio.FormatoFechaFull2); //- Ejecutado inicio
                    data[17] = ((DateTime)detalle[0].Reintfejefin).ToString(ConstantesAppServicio.FormatoFechaFull2); //- Ejecutado fin
                    data[18] = (detalle[0].Reintfproginicio != null) ?
                        ((DateTime)detalle[0].Reintfproginicio).ToString(ConstantesAppServicio.FormatoFechaFull2) : string.Empty; //- Programado inicio
                    data[19] = (detalle[0].Reintfprogfin != null) ?
                        ((DateTime)detalle[0].Reintfprogfin).ToString(ConstantesAppServicio.FormatoFechaFull2) : string.Empty; //- Programado fin

                    for (int k = 1; k <= 5; k++)
                    {
                        ReInterrupcionSuministroDTO itemDetalle = detalle.Where(x => x.OrdenDetalle == k).FirstOrDefault();
                        if (itemDetalle != null)
                        {
                            data[20 + (k - 1) * 4] = itemDetalle.Responsable; //- Responsable
                            data[21 + (k - 1) * 4] = itemDetalle.Reintdorcentaje.ToString(); // Porcentaje
                            data[22 + (k - 1) * 4] = (itemDetalle.Reintddispocision != ConstantesAppServicio.SI)? "N":"S";
                            data[23 + (k - 1) * 4] = (itemDetalle.Reintdcompceo != ConstantesAppServicio.SI) ? "N" : "S";
                            // aplicaicon ntcse
                            // compensacion 0
                        }
                        else
                        {
                            data[20 + (k - 1) * 4] = string.Empty;
                            data[21 + (k - 1) * 4] = string.Empty;
                            data[22 + (k - 1) * 4] = string.Empty;
                            data[23 + (k - 1) * 4] = string.Empty;
                            //
                            //
                        }
                    }

                    data[40] = (detalle[0].Reintcausaresumida != null) ? detalle[0].Reintcausaresumida : string.Empty; //- Causa resumida
                    data[41] = detalle[0].Reinteie.ToString(); //- Ei/E
                    data[42] = detalle[0].Reintresarcimiento.ToString(); //- Resarcimiento
                    data[43] = (detalle[0].Reintevidencia != null) ? detalle[0].Reintevidencia.ToString() : string.Empty; //- Evidencia

                    for (int k = 1; k <= 5; k++)
                    {
                        ReInterrupcionSuministroDTO itemDetalle = detalle.Where(x => x.OrdenDetalle == k).FirstOrDefault();
                        if (itemDetalle != null)
                        {
                            if (itemDetalle.Reintdconformidadresp == ConstantesAppServicio.NO)
                                itemDetalle.Reintdconformidadresp = "No";
                            if (itemDetalle.Reintdconformidadresp == ConstantesAppServicio.SI)
                                itemDetalle.Reintdconformidadresp = "SI";

                            if (itemDetalle.Reintdconformidadsumi == ConstantesAppServicio.NO)
                                itemDetalle.Reintdconformidadsumi = "No";
                            if (itemDetalle.Reintdconformidadsumi == ConstantesAppServicio.SI)
                                itemDetalle.Reintdconformidadsumi = "SI";

                            data[44 + (k - 1) * 8] = (itemDetalle.Reintdconformidadresp!=null)?itemDetalle.Reintdconformidadresp:string.Empty; //- Conformidad responsable
                            data[45 + (k - 1) * 8] = (itemDetalle.Reintdobservacionresp != null) ? itemDetalle.Reintdobservacionresp : string.Empty; //- Observacion responsable
                            data[46 + (k - 1) * 8] = (itemDetalle.Reintddetalleresp != null) ? itemDetalle.Reintddetalleresp : string.Empty; //- Detalle campo observado responsable
                            data[47 + (k - 1) * 8] = (itemDetalle.Reintdcomentarioresp != null) ? itemDetalle.Reintdcomentarioresp : string.Empty; //- Comentario responsable
                            data[48 + (k - 1) * 8] = (itemDetalle.Reintdevidenciaresp != null) ? itemDetalle.Reintdevidenciaresp : string.Empty; //- Evidencia responsable
                            data[49 + (k - 1) * 8] = (itemDetalle.Reintdconformidadsumi != null) ? itemDetalle.Reintdconformidadsumi : string.Empty; //- Conformidad suministrador
                            data[50 + (k - 1) * 8] = (itemDetalle.Reintdcomentariosumi != null) ? itemDetalle.Reintdcomentariosumi : string.Empty; //- Comentario suministrador
                            data[51 + (k - 1) * 8] = (itemDetalle.Reintdevidenciasumi != null) ? itemDetalle.Reintdevidenciasumi : string.Empty; //- Evidencia suministrador
                        }
                        else
                        {
                            data[44 + (k - 1) * 8] = string.Empty;
                            data[45 + (k - 1) * 8] = string.Empty;
                            data[46 + (k - 1) * 8] = string.Empty;
                            data[47 + (k - 1) * 8] = string.Empty;
                            data[48 + (k - 1) * 8] = string.Empty;
                            data[49 + (k - 1) * 8] = string.Empty;
                            data[50 + (k - 1) * 8] = string.Empty;
                            data[51 + (k - 1) * 8] = string.Empty;
                        }
                    }

                    data[84] = detalle[0].Reintdescontroversia; //- Decisión controveria
                    data[85] = detalle[0].Reintcomentario; //- Comentarios

                    if (!string.IsNullOrEmpty(buscar))
                    {
                        int count = data.Where(x => x!= null && x.ToLower().Contains(buscar)).Count();
                        if (count > 0) result.Add(data);
                    }
                    else result.Add(data);
                }
            }

            return result.ToArray();
        }


        public static string[][] ObtenerConsolidadoInterrupcionesComparativo(List<ReInterrupcionSuministroDTO> entitys, string conformidadResponsable,
            string conformidadSuministrador, string buscar)
        {
            List<string[]> result = new List<string[]>();
            List<int> ids = entitys.Select(x => x.Reintcodi).Distinct().ToList();
            List<TipoObservacion> listaTipos = UtilCalculoResarcimiento.ListarTiposObservacion();


            foreach (int id in ids)
            {
                List<ReInterrupcionSuministroDTO> detalle = entitys.Where(x => x.Reintcodi == id).OrderBy(x => x.OrdenDetalle).ToList();

                //- Filtramos conformidad suministrador y responsable
                int contador = detalle.Where(x => (x.Reintdconformidadresp == conformidadResponsable || string.IsNullOrEmpty(conformidadResponsable))
                && (x.Reintdconformidadsumi == conformidadSuministrador || string.IsNullOrEmpty(conformidadSuministrador))).Count();



                if (contador > 0)
                {
                    string[] data = new string[76];
                    data[0] = detalle[0].Reintcodi.ToString() + "-" + detalle[0].Reintestado + "-" + detalle[0].Reintreftrimestral; //- Id
                    data[1] = string.Empty; //- Opciones
                    data[2] = detalle[0].Emprnomb; //- Suministrador
                    data[3] = detalle[0].Reintcorrelativo.ToString(); //- Correlativo
                    data[4] = detalle[0].Reinttipcliente; //- Tipo de cliente
                    data[5] = detalle[0].Cliente; //- Cliente
                    data[6] = detalle[0].Reintptoentrega; //- Pto de entrega
                    data[7] = (detalle[0].Reintnrosuministro != null) ? detalle[0].Reintnrosuministro.ToString() : String.Empty; //- Nro suministro
                    data[8] = detalle[0].NivelTension; //- Nivel de tension
                    data[9] = detalle[0].Reintaplicacionnumeral.ToString(); //- Aplicacion literal
                    data[10] = (detalle[0].Reintenergiasemestral != null) ? detalle[0].Reintenergiasemestral.ToString() : string.Empty; //- Energia semestral
                    data[11] = detalle[0].Reintinctolerancia; //- Incremento de tolerancias
                    data[12] = detalle[0].TipoInterrupcion; //- Tipo de interrupcion
                    data[13] = detalle[0].CausaInterrupcion; //- Causa de interrupcion
                    data[14] = detalle[0].Reintni.ToString(); //- Indicador NI
                    data[15] = detalle[0].Reintki.ToString(); //- Indicador KI
                    data[16] = ((DateTime)detalle[0].Reintfejeinicio).ToString(ConstantesAppServicio.FormatoFechaFull2); //- Ejecutado inicio
                    data[17] = ((DateTime)detalle[0].Reintfejefin).ToString(ConstantesAppServicio.FormatoFechaFull2); //- Ejecutado fin
                    data[18] = (detalle[0].Reintfproginicio != null) ?
                        ((DateTime)detalle[0].Reintfproginicio).ToString(ConstantesAppServicio.FormatoFechaFull2) : string.Empty; //- Programado inicio
                    data[19] = (detalle[0].Reintfprogfin != null) ?
                        ((DateTime)detalle[0].Reintfprogfin).ToString(ConstantesAppServicio.FormatoFechaFull2) : string.Empty; //- Programado fin

                    for (int k = 1; k <= 5; k++)
                    {
                        ReInterrupcionSuministroDTO itemDetalle = detalle.Where(x => x.OrdenDetalle == k).FirstOrDefault();
                        if (itemDetalle != null)
                        {
                            data[20 + (k - 1) * 2] = itemDetalle.Responsable; //- Responsable
                            data[21 + (k - 1) * 2] = itemDetalle.Reintdorcentaje.ToString(); // Porcentaje
                        }
                        else
                        {
                            data[20 + (k - 1) * 2] = string.Empty;
                            data[21 + (k - 1) * 2] = string.Empty;
                        }
                    }

                    data[30] = (detalle[0].Reintcausaresumida != null) ? detalle[0].Reintcausaresumida : string.Empty; //- Causa resumida
                    data[31] = detalle[0].Reinteie.ToString(); //- Ei/E
                    data[32] = detalle[0].Reintresarcimiento.ToString(); //- Resarcimiento
                    data[33] = detalle[0].Reintevidencia; //- Evidencia

                    for (int k = 1; k <= 5; k++)
                    {
                        ReInterrupcionSuministroDTO itemDetalle = detalle.Where(x => x.OrdenDetalle == k).FirstOrDefault();
                        if (itemDetalle != null)
                        {
                            if (itemDetalle.Reintdconformidadresp == ConstantesAppServicio.NO)
                                itemDetalle.Reintdconformidadresp = "No";
                            if (itemDetalle.Reintdconformidadresp == ConstantesAppServicio.SI)
                                itemDetalle.Reintdconformidadresp = "SI";

                            if (itemDetalle.Reintdconformidadsumi == ConstantesAppServicio.NO)
                                itemDetalle.Reintdconformidadsumi = "No";
                            if (itemDetalle.Reintdconformidadsumi == ConstantesAppServicio.SI)
                                itemDetalle.Reintdconformidadsumi = "SI";

                            if (!string.IsNullOrEmpty(itemDetalle.Reintdobservacionresp))
                            {
                                List<int> idsObservaciones = itemDetalle.Reintdobservacionresp.Split(',').Select(int.Parse).ToList();
                                List<string> textosObservaciones = listaTipos.Where(x => idsObservaciones.Any(y => int.Parse(x.Id) == y)).Select(x => x.Texto).ToList();
                                itemDetalle.Reintdobservacionresp = string.Join(",", textosObservaciones);
                            }
                            else
                            {
                                itemDetalle.Reintdobservacionresp = string.Empty;
                            }

                            data[34 + (k - 1) * 8] = itemDetalle.Reintdconformidadresp; //- Conformidad responsable
                            data[35 + (k - 1) * 8] = itemDetalle.Reintdobservacionresp; //- Observacion responsable
                            data[36 + (k - 1) * 8] = itemDetalle.Reintddetalleresp; //- Detalle campo observado responsable
                            data[37 + (k - 1) * 8] = itemDetalle.Reintdcomentarioresp; //- Comentario responsable
                            data[38 + (k - 1) * 8] = itemDetalle.Reintdevidenciaresp; //- Evidencia responsable
                            data[39 + (k - 1) * 8] = itemDetalle.Reintdconformidadsumi; //- Conformidad suministrador
                            data[40 + (k - 1) * 8] = itemDetalle.Reintdcomentariosumi; //- Comentario suministrador
                            data[41 + (k - 1) * 8] = itemDetalle.Reintdevidenciasumi; //- Evidencia suministrador
                        }
                        else
                        {
                            data[34 + (k - 1) * 8] = string.Empty;
                            data[35 + (k - 1) * 8] = string.Empty;
                            data[36 + (k - 1) * 8] = string.Empty;
                            data[37 + (k - 1) * 8] = string.Empty;
                            data[38 + (k - 1) * 8] = string.Empty;
                            data[39 + (k - 1) * 8] = string.Empty;
                            data[40 + (k - 1) * 8] = string.Empty;
                            data[41 + (k - 1) * 8] = string.Empty;
                        }
                    }

                    data[74] = detalle[0].Reintdescontroversia; //- Decisión controveria
                    data[75] = detalle[0].Reintcomentario; //- Comentarios

                    if (!string.IsNullOrEmpty(buscar))
                    {
                        int count = data.Where(x => x != null && x.ToLower().Contains(buscar)).Count();
                        if (count > 0) result.Add(data);
                    }
                    else result.Add(data);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Permite obtener la grilla de consolidado de interrupciones
        /// </summary>
        /// <param name="entitys"></param>
        /// <param name="conformidadResponsable"></param>
        /// <param name="conformidadSuministrador"></param>
        /// <param name="buscar"></param>
        /// <returns></returns>
        public static string[][] ObtenerTrazabilidadInterrupcion(List<ReInterrupcionSuministroDTO> entitys)
        {
            List<string[]> result = new List<string[]>();
            List<int> ids = entitys.Select(x => x.Reintcodi).Distinct().ToList();

            foreach (int id in ids)
            {
                List<ReInterrupcionSuministroDTO> detalle = entitys.Where(x => x.Reintcodi == id).OrderBy(x => x.OrdenDetalle).ToList();

                //- Filtramos conformidad suministrador y responsable
                string[] data = new string[65];

                data[0] = detalle[0].Reintusucreacion; //- usuario
                data[1] = (detalle[0].Reintfeccreacion != null)? ((DateTime)detalle[0].Reintfeccreacion).ToString(ConstantesAppServicio.FormatoFechaFull2): string.Empty; //- Fecha
                data[2] = detalle[0].Emprnomb; //- Suministrador
                data[3] = detalle[0].Reintcorrelativo.ToString(); //- Correlativo
                data[4] = detalle[0].Reinttipcliente; //- Tipo de cliente
                data[5] = detalle[0].Cliente; //- Cliente
                data[6] = detalle[0].Reintptoentrega; //- Pto de entrega
                data[7] = (detalle[0].Reintnrosuministro != null) ? detalle[0].Reintnrosuministro.ToString() : String.Empty; //- Nro suministro
                data[8] = detalle[0].NivelTension; //- Nivel de tension
                data[9] = detalle[0].Reintaplicacionnumeral.ToString(); //- Aplicacion literal
                data[10] = (detalle[0].Reintenergiasemestral != null) ? detalle[0].Reintenergiasemestral.ToString() : string.Empty; //- Energia semestral
                data[11] = detalle[0].Reintinctolerancia; //- Incremento de tolerancias
                data[12] = detalle[0].TipoInterrupcion; //- Tipo de interrupcion
                data[13] = detalle[0].CausaInterrupcion; //- Causa de interrupcion
                data[14] = detalle[0].Reintni.ToString(); //- Indicador NI
                data[15] = detalle[0].Reintki.ToString(); //- Indicador KI
                data[16] = ((DateTime)detalle[0].Reintfejeinicio).ToString(ConstantesAppServicio.FormatoFechaFull2); //- Ejecutado inicio
                data[17] = ((DateTime)detalle[0].Reintfejefin).ToString(ConstantesAppServicio.FormatoFechaFull2); //- Ejecutado fin
                data[18] = (detalle[0].Reintfproginicio != null) ?
                    ((DateTime)detalle[0].Reintfproginicio).ToString(ConstantesAppServicio.FormatoFechaFull2) : string.Empty; //- Programado inicio
                data[19] = (detalle[0].Reintfprogfin != null) ?
                    ((DateTime)detalle[0].Reintfprogfin).ToString(ConstantesAppServicio.FormatoFechaFull2) : string.Empty; //- Programado fin

                for (int k = 1; k <= 5; k++)
                {
                    ReInterrupcionSuministroDTO itemDetalle = detalle.Where(x => x.OrdenDetalle == k).FirstOrDefault();
                    if (itemDetalle != null)
                    {
                        data[20 + (k - 1) * 2] = itemDetalle.Responsable; //- Responsable
                        data[21 + (k - 1) * 2] = itemDetalle.Reintdorcentaje.ToString(); // Porcentaje
                    }
                    else
                    {
                        data[20 + (k - 1) * 2] = string.Empty;
                        data[21 + (k - 1) * 2] = string.Empty;
                    }
                }

                data[30] = (detalle[0].Reintcausaresumida != null) ? detalle[0].Reintcausaresumida : string.Empty; //- Causa resumida
                data[31] = detalle[0].Reinteie.ToString(); //- Ei/E
                data[32] = detalle[0].Reintresarcimiento.ToString(); //- Resarcimiento               

                for (int k = 1; k <= 5; k++)
                {
                    ReInterrupcionSuministroDTO itemDetalle = detalle.Where(x => x.OrdenDetalle == k).FirstOrDefault();
                    if (itemDetalle != null)
                    {
                        data[33 + (k - 1) * 6] = (itemDetalle.Reintdconformidadresp != null) ? itemDetalle.Reintdconformidadresp : string.Empty; //- Conformidad responsable
                        data[34 + (k - 1) * 6] = (itemDetalle.Reintdobservacionresp != null) ? itemDetalle.Reintdobservacionresp : string.Empty;  //- Observacion responsable
                        data[35 + (k - 1) * 6] = (itemDetalle.Reintddetalleresp != null) ? itemDetalle.Reintddetalleresp : string.Empty;  //- Detalle campo observado responsable
                        data[36 + (k - 1) * 6] = (itemDetalle.Reintdcomentarioresp != null) ? itemDetalle.Reintdcomentarioresp : string.Empty;  //- Comentario responsable                       
                        data[37 + (k - 1) * 6] = (itemDetalle.Reintdconformidadsumi != null) ? itemDetalle.Reintdconformidadsumi : string.Empty;  //- Conformidad suministrador
                        data[38 + (k - 1) * 6] = (itemDetalle.Reintdcomentariosumi != null) ? itemDetalle.Reintdcomentariosumi : string.Empty;  //- Comentario suministrador                       
                    }
                    else
                    {
                        data[33 + (k - 1) * 6] = string.Empty;
                        data[34 + (k - 1) * 6] = string.Empty;
                        data[35 + (k - 1) * 6] = string.Empty;
                        data[36 + (k - 1) * 6] = string.Empty;
                        data[37 + (k - 1) * 6] = string.Empty;
                        data[38 + (k - 1) * 6] = string.Empty;                        
                    }
                }

                data[63] = detalle[0].Reintdescontroversia; //- Decisión controveria
                data[64] = detalle[0].Reintcomentario; //- Comentarios

                result.Add(data);

            }

            return result.ToArray();
        }

        /// <summary>
        /// Permite obtener la data de las interrupciones de rechazo de carga
        /// </summary>
        /// <param name="entitys"></param>
        /// <param name="buscar"></param>
        /// <returns></returns>
        public static string[][] ObtenerConsolidadoRechazoCarga(List<ReRechazoCargaDTO> entitys, string buscar)
        {
            List<string[]> result = new List<string[]>();

            foreach(ReRechazoCargaDTO entity in entitys)
            {
                string[] data = new string[32];
                data[0] = entity.Rerccodi.ToString() + "-" + entity.Rercestado; //- Id
                data[1] = string.Empty;                //- Eliminar
                data[2] = entity.Suministrador;
                data[3] = entity.Rerccorrelativo.ToString(); //- Correlativo
                data[4] = entity.Rerctipcliente; //- Tipo de cliente
                data[5] = entity.Cliente; //- Cliente
                data[6] = entity.Rercptoentrega; //- Punto de entrega
                data[7] = entity.Rercalimentadorsed; //- Alimentador SED
                data[8] = entity.Rercenst.ToString(); //- Enst
                data[9] = entity.Evento; //- Evento COES
                data[10] = (entity.Rerccomentario != null) ? entity.Rerccomentario : string.Empty; //- Comentario              
                data[11] = ((DateTime)entity.Rerctejecinicio).ToString(ConstantesAppServicio.FormatoFechaFull2); //- Ejecutado inicio
                data[12] = ((DateTime)entity.Rerctejecfin).ToString(ConstantesAppServicio.FormatoFechaFull2); //- Ejecutado fin
                data[13] = entity.Rercpk.ToString(); //- Pk
                data[14] = entity.Rerccompensable; //- Compensable
                data[15] = entity.Rercens.ToString(); //- ENS
                data[16] = entity.Rercresarcimiento.ToString(); //- Resarcimiento

                //- Corregir aquí
                data[17] = (entity.Rercresponsable1 != null) ? entity.Rercresponsable1.ToString() : string.Empty;
                data[18] = entity.Rercporcentaje1.ToString();
                data[19] = (!string.IsNullOrEmpty(entity.Rercresponsable1))? (entity.Rercdisposicion1 != ConstantesAppServicio.SI) ? "N" : "S": string.Empty;

                data[20] = (entity.Rercresponsable2 != null) ? entity.Rercresponsable2.ToString() : string.Empty;
                data[21] = entity.Rercporcentaje2.ToString();
                data[22] = (!string.IsNullOrEmpty(entity.Rercresponsable2)) ? (entity.Rercdisposicion2 != ConstantesAppServicio.SI) ? "N" : "S" : string.Empty;

                data[23] = (entity.Rercresponsable3 != null) ? entity.Rercresponsable3.ToString() : string.Empty;
                data[24] = entity.Rercporcentaje3.ToString();
                data[25] = (!string.IsNullOrEmpty(entity.Rercresponsable3)) ? (entity.Rercdisposicion3 != ConstantesAppServicio.SI) ? "N" : "S" : string.Empty;

                data[26] = (entity.Rercresponsable4 != null) ? entity.Rercresponsable4.ToString() : string.Empty;
                data[27] = entity.Rercporcentaje4.ToString();
                data[28] = (!string.IsNullOrEmpty(entity.Rercresponsable4)) ? (entity.Rercdisposicion4 != ConstantesAppServicio.SI) ? "N" : "S" : string.Empty;

                data[29] = (entity.Rercresponsable5 != null) ? entity.Rercresponsable5.ToString() : string.Empty;
                data[30] = entity.Rercporcentaje5.ToString();
                data[31] = (!string.IsNullOrEmpty(entity.Rercresponsable5)) ? (entity.Rercdisposicion5 != ConstantesAppServicio.SI) ? "N" : "S" : string.Empty;


                if (!string.IsNullOrEmpty(buscar))
                {
                    int count = data.Where(x => x != null && x.ToLower().Contains(buscar)).Count();
                    if (count > 0) result.Add(data);
                }
                else result.Add(data);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Obtiene los datos de la trazabilidad de rechazo de carga
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public static string[][] ObtenerTrazabilidadRechazoCarga(List<ReRechazoCargaDTO> entitys)
        {
            List<string[]> result = new List<string[]>();

            foreach (ReRechazoCargaDTO entity in entitys)
            {
                string[] data = new string[17];
                data[0] = entity.Rercusucreacion;
                data[1] = (entity.Rercfeccreacion != null)?((DateTime)entity.Rercfeccreacion).ToString(ConstantesAppServicio.FormatoFechaFull2):string.Empty;                //- Eliminar
                data[2] = entity.Suministrador;
                data[3] = entity.Rerccorrelativo.ToString(); //- Correlativo
                data[4] = entity.Rerctipcliente; //- Tipo de cliente
                data[5] = entity.Cliente; //- Cliente
                data[6] = entity.Rercptoentrega; //- Punto de entrega
                data[7] = entity.Rercalimentadorsed; //- Alimentador SED
                data[8] = entity.Rercenst.ToString(); //- Enst
                data[9] = entity.Evento; //- Evento COES
                data[10] = (entity.Rerccomentario != null) ? entity.Rerccomentario : string.Empty; //- Comentario              
                data[11] = ((DateTime)entity.Rerctejecinicio).ToString(ConstantesAppServicio.FormatoFechaFull2); //- Ejecutado inicio
                data[12] = ((DateTime)entity.Rerctejecfin).ToString(ConstantesAppServicio.FormatoFechaFull2); //- Ejecutado fin
                data[13] = entity.Rercpk.ToString(); //- Pk
                data[14] = entity.Rerccompensable; //- Compensable
                data[15] = entity.Rercens.ToString(); //- ENS
                data[16] = entity.Rercresarcimiento.ToString(); //- Resarcimiento
                result.Add(data);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Permite generar el excel de consolidado de interrupciones
        /// </summary>
        /// <param name="path"></param>
        /// <param name="file"></param>
        /// <param name="plantilla"></param>
        /// <param name="result"></param>
        public static void GenerarExcelConsolidadoInterrupcion(string path, string file, string plantilla,string[][] result)
        {
            try
            {
                FileInfo template = new FileInfo(path + plantilla);
                FileInfo newFile = new FileInfo(path + file);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path + file);
                }
                using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                {                    
                    ExcelWorksheet wsInterrupcion = xlPackage.Workbook.Worksheets["Interrupciones"];
                    ExcelRange rg = null;
                    int[] cols = { 1, 43, 48, 56, 64, 72, 80, 51, 59, 67, 75, 83 };
                    int[] colssino = { 22, 23, 26, 27, 30, 31, 34, 35, 38, 39 };
                    int index = 5;
                    foreach (string[] data in result)
                    {
                        int col = 1;
                        for(int k = 0; k< data.Length; k++)
                        {
                            if (!cols.Contains(k))
                            {
                                if (colssino.Contains(k))
                                {
                                    if (!string.IsNullOrEmpty(data[k]))
                                    {
                                        wsInterrupcion.Cells[index, col].Value = (data[k] == "S") ? "Si" : "No";
                                    }
                                }
                                else
                                {
                                    wsInterrupcion.Cells[index, col].Value = data[k];
                                }


                                col++;
                            }                            
                        }

                        string estado = data[0].Split(ConstantesAppServicio.CaracterGuion)[1];
                        if(estado == ConstantesAppServicio.Baja)
                        {
                            rg = wsInterrupcion.Cells[index, 1, index, 74];
                            rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FFB0B0"));
                        }
                        
                        string copia = data[0].Split(ConstantesAppServicio.CaracterGuion)[2];

                        if (copia == ConstantesAppServicio.SI)
                        {
                            rg = wsInterrupcion.Cells[index, 1, index, 74];
                            rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FFFF00"));
                        }

                        index++;
                    }

                    rg = wsInterrupcion.Cells[5, 1, index - 1, 74];
                    rg.Style.Font.Size = 9;
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                    xlPackage.Save();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);           
            }
        }

        /// <summary>
        /// Permite generar el excel de consolidado de interrupciones
        /// </summary>
        /// <param name="path"></param>
        /// <param name="file"></param>
        /// <param name="plantilla"></param>
        /// <param name="result"></param>
        public static void GenerarExcelConsolidadoRechazoCarga(string path, string file, string plantilla, string[][] result)
        {
            try
            {
                FileInfo template = new FileInfo(path + plantilla);
                FileInfo newFile = new FileInfo(path + file);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path + file);
                }
                using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                {
                    ExcelWorksheet wsInterrupcion = xlPackage.Workbook.Worksheets["Interrupciones"];                   
                    int index = 3;
                    ExcelRange rg = null;
                    int[] colssino = { 19, 22, 25, 28, 31 };
                    foreach (string[] data in result)
                    {
                        int col = 1;
                        for (int k = 0; k < data.Length; k++)
                        {
                            if (k != 1)
                            {
                                if (colssino.Contains(k))
                                {
                                    if (!string.IsNullOrEmpty(data[k]))
                                    {
                                        wsInterrupcion.Cells[index, col].Value = (data[k] == "S") ? "Si" : "No";
                                    }
                                }
                                else
                                {
                                    wsInterrupcion.Cells[index, col].Value = data[k];
                                }
                                col++;
                            }
                        }

                        string estado = data[0].Split(ConstantesAppServicio.CaracterGuion)[1];
                        if (estado == ConstantesAppServicio.Baja)
                        {
                            rg = wsInterrupcion.Cells[index, 1, index, 31];
                            rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FFB0B0"));
                        }

                        index++;
                    }

                    rg = wsInterrupcion.Cells[3, 1, index - 1, 31];
                    rg.Style.Font.Size = 9;
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                    xlPackage.Save();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener los ids recursivamente
        /// </summary>
        /// <param name="blobcodi"></param>
        public void ObtenerIdInterrupcionRecursivo(int id, List<ReInterrupcionSuministroDTO> list)
        {
            this.IdsInterrupcion.Add(id);
            ReInterrupcionSuministroDTO entity = list.Where(x => x.Reintpadre == id).FirstOrDefault();

            if(entity != null)
            {
                ObtenerIdInterrupcionRecursivo(entity.Reintcodi, list);
            }
        }

        /// <summary>
        /// Permite obtener los ids recursivamente
        /// </summary>
        /// <param name="blobcodi"></param>
        public void ObtenerIdRechazoCargaRecursivo(int id, List<ReRechazoCargaDTO> list)
        {
            this.IdsRechazoCarga.Add(id);
            ReRechazoCargaDTO entity = list.Where(x => x.Rercpadre == id).FirstOrDefault();

            if (entity != null)
            {
                ObtenerIdRechazoCargaRecursivo(entity.Rerccodi, list);
            }
        }

        /// <summary>
        /// Permite obtener las fechas
        /// </summary>
        /// <param name="entityEvento"></param>
        /// <param name="rangoInicio"></param>
        /// <param name="rangoFin"></param>
        public static void ObtenerRangoFechasMedicion(ReEventoProductoDTO entityEvento, out DateTime rangoInicio, out DateTime rangoFin)
        {
            DateTime fechaInicio = (DateTime)entityEvento.Reevprfecinicio;
            DateTime fechaFin = (DateTime)entityEvento.Reevprfecfin;
            DateTime inicio = DateTime.Now;
            DateTime fin = DateTime.Now;


            if (fechaInicio.Minute == 0 || fechaInicio.Minute == 15 || fechaInicio.Minute == 30 || fechaInicio.Minute == 45)
            {
                inicio = fechaInicio;
            }
            else
            {
                if (fechaInicio.Minute > 0 && fechaInicio.Minute < 15)
                {
                    inicio = new DateTime(fechaInicio.Year, fechaInicio.Month, fechaInicio.Day).AddHours(fechaInicio.Hour).AddMinutes(15);
                }
                if (fechaInicio.Minute > 15 && fechaInicio.Minute < 30)
                {
                    inicio = new DateTime(fechaInicio.Year, fechaInicio.Month, fechaInicio.Day).AddHours(fechaInicio.Hour).AddMinutes(30);
                }
                if (fechaInicio.Minute > 30 && fechaInicio.Minute < 45)
                {
                    inicio = new DateTime(fechaInicio.Year, fechaInicio.Month, fechaInicio.Day).AddHours(fechaInicio.Hour).AddMinutes(45);
                }
                if (fechaInicio.Minute > 45 && fechaInicio.Minute <= 59)
                {
                    inicio = new DateTime(fechaInicio.Year, fechaInicio.Month, fechaInicio.Day).AddHours(fechaInicio.Hour + 1);
                }
            }


            if (fechaFin.Minute == 0 || fechaFin.Minute == 15 || fechaFin.Minute == 30 || fechaFin.Minute == 45)
            {
                fin = fechaFin;
            }
            else
            {
                if (fechaFin.Minute > 0 && fechaFin.Minute < 15)
                {
                    fin = new DateTime(fechaFin.Year, fechaFin.Month, fechaFin.Day).AddHours(fechaFin.Hour);
                }
                if (fechaFin.Minute > 15 && fechaFin.Minute < 30)
                {
                    fin = new DateTime(fechaFin.Year, fechaFin.Month, fechaFin.Day).AddHours(fechaFin.Hour).AddMinutes(15);
                }
                if (fechaFin.Minute > 30 && fechaFin.Minute < 45)
                {
                    fin = new DateTime(fechaFin.Year, fechaFin.Month, fechaFin.Day).AddHours(fechaFin.Hour).AddMinutes(30);
                }
                if (fechaFin.Minute > 45 && fechaFin.Minute <= 59)
                {
                    fin = new DateTime(fechaFin.Year, fechaFin.Month, fechaFin.Day).AddHours(fechaFin.Hour).AddMinutes(45);
                }
            }

            rangoInicio = inicio;
            rangoFin = fin;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public static List<ReInterrupcionSuministroDTO> ClonarListaInterrupcion(List<ReInterrupcionSuministroDTO> entitys)
        {
            List<ReInterrupcionSuministroDTO> result = new List<ReInterrupcionSuministroDTO>();

            foreach (ReInterrupcionSuministroDTO entity in entitys)
            {
                ReInterrupcionSuministroDTO subItem = new ReInterrupcionSuministroDTO();

                subItem.Reintcodi = entity.Reintcodi;
                subItem.Emprnomb = entity.Emprnomb;
                subItem.Reintestado = entity.Reintestado;
                subItem.Reintcorrelativo = entity.Reintcorrelativo;
                subItem.Reinttipcliente = entity.Reinttipcliente;
                subItem.Cliente = entity.Cliente;
                subItem.Reintptoentrega = entity.Reintptoentrega;
                subItem.Reintnrosuministro = entity.Reintnrosuministro;
                subItem.NivelTension = entity.NivelTension;
                subItem.Reintaplicacionnumeral = entity.Reintaplicacionnumeral;
                subItem.Reintenergiasemestral = entity.Reintenergiasemestral;
                subItem.Reintinctolerancia = entity.Reintinctolerancia;
                subItem.TipoInterrupcion = entity.TipoInterrupcion;
                subItem.CausaInterrupcion = entity.CausaInterrupcion;
                subItem.Reintni = entity.Reintni;
                subItem.Reintki = entity.Reintki;
                subItem.Reintfejeinicio = entity.Reintfejeinicio;
                subItem.Reintfejefin = entity.Reintfejefin;
                subItem.Reintfproginicio = entity.Reintfproginicio;
                subItem.Reintfprogfin = entity.Reintfprogfin;
                subItem.Reintcausaresumida = entity.Reintcausaresumida;
                subItem.Reinteie = entity.Reinteie;
                subItem.Reintresarcimiento = entity.Reintresarcimiento;
                subItem.Reintevidencia = entity.Reintevidencia;
                subItem.Reintdescontroversia = entity.Reintdescontroversia;
                subItem.Reintcomentario = entity.Reintcomentario;
                subItem.Reintusucreacion = entity.Reintusucreacion;
                subItem.Reintfeccreacion = entity.Reintfeccreacion;
                subItem.OrdenDetalle = entity.OrdenDetalle;
                subItem.Responsable = entity.Responsable;
                subItem.Reintdorcentaje = entity.Reintdorcentaje;
                subItem.Reintdcodi = entity.Reintdcodi;
                subItem.Reintdconformidadresp = entity.Reintdconformidadresp;
                subItem.Reintdobservacionresp = entity.Reintdobservacionresp;
                subItem.Reintddetalleresp = entity.Reintddetalleresp;
                subItem.Reintdcomentarioresp = entity.Reintdcomentarioresp;
                subItem.Reintdevidenciaresp = entity.Reintdevidenciaresp;
                subItem.Reintdconformidadsumi = entity.Reintdconformidadsumi;
                subItem.Reintdcomentariosumi = entity.Reintdcomentariosumi;
                subItem.Reintdevidenciasumi = entity.Reintdevidenciasumi;
                subItem.Reintreftrimestral = entity.Reintreftrimestral;

                result.Add(subItem);
            }

            return result;
        }
    }

    /// <summary>
    /// Clase para manejo de grilla de interrupciones
    /// </summary>
    public class EstructuraInterrupcion
    {
        public string[][] Data { get; set; }
        public List<ReEmpresaDTO> ListaCliente { get; set; }
        public List<ReEmpresaDTO> ListaEmpresa { get; set; }
        public List<RePtoentregaPeriodoDTO> ListaPuntoEntrega { get; set; }
        public List<ReTipoInterrupcionDTO> ListaTipoInterrupcion { get; set; }
        public List<ReCausaInterrupcionDTO> ListaCausaInterrupcion { get; set; }
        public List<ReNivelTensionDTO> ListaNivelTension { get; set; }
        public int Result { get; set; }
        public string[][] Indicadores { get; set; }
        public string PlazoEnvio { get; set; }
        public string PlazoEnergia { get; set; }
        public string PlazoRespuesta { get; set; }
        public string PlazoObservacion { get; set; }
        public List<TipoObservacion> ListaTiposObservacion { get; set; }
        public bool ConDatos { get; set; }
        public bool Grabar { get; set; }
    }

    public class EstructuraRechazoCarga
    {
        public string[][] Data { get; set; }
        public List<ReEmpresaDTO> ListaCliente { get; set; }       
        public List<RePtoentregaPeriodoDTO> ListaPuntoEntrega { get; set; }
        public List<ReEventoPeriodoDTO> ListaEvento { get; set; }
        public string PlazoEnvio { get; set; }
        public int Result { get; set; }
        public bool ConDatos { get; set; }
        public bool Grabar { get; set; }
    }

    public class EstructuraConsolidado
    {
        public List<ReEventoPeriodoDTO> ListaEvento { get; set; }
        public List<RePtoentregaPeriodoDTO> ListaPuntoEntrega { get; set; }
        public List<ReEmpresaDTO> ListaSuministrador { get; set; }
        public List<ReEmpresaDTO> ListaResponsables { get; set; }
        public List<ReCausaInterrupcionDTO> ListaCausaInterrupcion { get; set; }
        public string[][] Data { get; set; }
        public int MuestraReporteComparativo { get; set; }
        public bool Grabar { get; set; }
    }

    public class TipoObservacion
    {
        public string Id { get; set; }
        public string Texto { get; set; }
    }

    public class TipoError
    { 
        public int Fila { get; set; }
        public string Mensaje { get; set; }
    }

    public class MotivoAnulacion
    { 
        public string Fecha { get; set; }
        public string Usuario { get; set; }
        public string Motivo { get; set; }
    }

    public class  EstructuraGrabado 
    {
        public int Result { get; set; }
        public string[][] Data { get; set; }
        public List<TipoError> ListaMensaje { get; set; }
    }

    public class EstructuraMedicion
    {
        public string[][] Data { get; set; }
        public string Resarcimiento { get; set; }
        public double FactorCompensacionUnitaria { get; set; }
        public int Result { get; set; }
        public List<string> Validaciones { get; set; }
        public decimal Tension { get; set; }
    }

    public class EstructuraIngresoTransmision
    {
        public int Plazo { get; set; }
        public int Existe { get; set; }
        public ReIngresoTransmisionDTO Entidad { get; set; }
    }

    public class EstructuraNotificacion
    {
        public int IdNuevo { get; set; }
        public int IdAnterior { get; set; }
        public int TipoCambio { get; set; }
        public ReInterrupcionSuministroDetDTO Det1 { get; set; }
        public ReInterrupcionSuministroDetDTO Det2 { get; set; }
        public ReInterrupcionSuministroDetDTO Det3 { get; set; }
        public ReInterrupcionSuministroDetDTO Det4 { get; set; }
        public ReInterrupcionSuministroDetDTO Det5 { get; set; }
    }

    public class EstructuraGrabadoRespuesta
    {
        public ReInterrupcionSuministroDTO Entidad { get; set; }
        public ReInterrupcionSuministroDTO Original { get; set; }
        public ReInterrupcionSuministroDetDTO Det1 { get; set; }
        public ReInterrupcionSuministroDetDTO Det2 { get; set; }
        public ReInterrupcionSuministroDetDTO Det3 { get; set; }
        public ReInterrupcionSuministroDetDTO Det4 { get; set; }
        public ReInterrupcionSuministroDetDTO Det5 { get; set; }
        public bool Grabar { get; set; }
        public bool FlagActualizacionRespuesta { get; set; }
    }

    /// <summary>
    /// Clase para manejo de grilla de interrupciones
    /// </summary>
    public class EstructuraInterrupcionInsumo
    {
        public string[][] Data { get; set; }
        public List<ReEmpresaDTO> ListaCliente { get; set; }
        public List<ReEmpresaDTO> ListaEmpresa { get; set; }
        public List<RePtoentregaPeriodoDTO> ListaPuntoEntrega { get; set; }
        public List<ReTipoInterrupcionDTO> ListaTipoInterrupcion { get; set; }
        public List<ReCausaInterrupcionDTO> ListaCausaInterrupcion { get; set; }       
        public List<ReEmpresaDTO> ListaSuministradores { get; set; }
        public int Result { get; set; }
        public bool Grabar { get; set; }
    }

    public class EstructuraCargaFile
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public int Fila { get; set; }
        public List<EstructuraCargaFileItem> ListaItems { get; set; }
    }

    public class EstructuraCargaFileItem
    {
        public int Orden { get; set; }
        public int Indicador { get; set; }
        public string FileName { get; set; }
    }

    public class EstructuraErroresInterrupcion
    {
        public int Fila { get; set; }
        public string Columna { get; set; }
        public string Mensaje { get; set; }
    }

    public class EstructuraArchivoSustento
    {
        public string Extension { get; set; }
        public string FileName { get; set; }
    }
}
