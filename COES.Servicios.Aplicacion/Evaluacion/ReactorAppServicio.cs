using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using COES.Servicios.Aplicacion.Evaluacion.Helper;

namespace COES.Servicios.Aplicacion.Evaluacion
{
    /// <summary>
    /// Clases con métodos del módulo Evaluación
    /// </summary>
    public class ReactorAppServicio : AppServicioBase
    {

        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ReactorAppServicio));


        #region GESPROTEC
        /// <summary>
        /// Devuelve el listado de las celdas de acoplamiento
        /// </summary>
        /// <returns></returns>
        public List<EprEquipoDTO> ListaBuscarReactor(string codigoId, string codigo, int ubicacion,
            int empresa, string estado)
        {
            return FactorySic.GetEprEquipoRepository().ListaBuscarReactor(codigoId, codigo, ubicacion,
            empresa, estado);
        }

        /// <summary>
        /// Permite registrar el Reactor
        /// </summary>
        /// <returns></returns>
        public string RegistrarReactor(EprEquipoDTO equipo)
        {
            return FactorySic.GetEprEquipoRepository().RegistrarReactor(equipo);
        }

        /// <summary>
        /// Permite obtener la lista inicial del formulario Reactor
        /// </summary>
        /// <returns></returns>
        public List<EprEquipoDTO> ListaReactor(string codigoId, string codigo, int ubicacion,
            int empresa, int area, string estado, int incluirCalcular)
        {
            var listaReactor = FactorySic.GetEprEquipoRepository().ListaReactor(codigoId, codigo, ubicacion,
            empresa, area, estado);

            if(incluirCalcular > 0)
            {
                var listaCodigos = string.Join(",", listaReactor.Select(p => p.Equicodi.Value));
                var listaFormulas = FactorySic.GetEprCalculosRepository().ListCalculosFormulasReactorMasivo(listaCodigos, ConstantesEvaluacionAppServicio.FAMCODI_REACTOR);

                var diccionarioFormulas = new Dictionary<int, List<EprCalculosDTO>>();

                foreach (var item in listaReactor.Select(p => p.Equicodi.Value).Distinct())
                {
                    diccionarioFormulas[item] = listaFormulas.Where(p => p.Equicodi == item).ToList();
                }

                foreach (var item in listaReactor)
                {                 

                    var calculosEquipo = diccionarioFormulas[item.Equicodi.Value];
                    n_calc.EvaluarFormulas(calculosEquipo);

                    item.FactorLimitanteFinal = string.Empty;

                    #region Asignacion Datos Calculados

                    if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NombreFactorLimitanteFinal && p.Estado == 1))
                    {
                        item.FactorLimitanteFinal = calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NombreFactorLimitanteFinal).Valor.ToString();
                    }                   

                    #endregion
                }
            }

            return listaReactor;
        }

        /// <summary>
        /// Permite obtener el detalle del reactor por id
        /// </summary>
        /// <returns></returns>
        public EprEquipoDTO ObtenerReactorPorId(int codigoId)
        {
            return FactorySic.GetEprEquipoRepository().ObtenerReactorPorId(codigoId);
        }

        /// <summary>
        /// Generar Excel de reporte plantilla
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <exception cref="Exception"></exception>
        public void GenerarExcelPlantilla(string path, string fileName)
        {
            try
            {
                List<string> hojas = new List<string>();
                hojas.Add(ConstantesEvaluacionAppServicio.HojaReactor);
                hojas.Add(ConstantesEvaluacionAppServicio.HojaCelda);              
                hojas.Add(ConstantesEvaluacionAppServicio.HojaProyecto);

                string file = path + fileName;

                FileInfo fi = new FileInfo(file);
                // Revisar si existe
                if (!fi.Exists)
                {
                    throw new Exception("Archivo " + fileName + " No existe");
                }

                using (ExcelPackage xlPackage = new ExcelPackage(fi))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets[ConstantesEvaluacionAppServicio.HojaPlantillaExcel];

                    xlPackage.Save();

                    foreach (var item in hojas)
                    {
                        GenerarFileExcelHoja(xlPackage, item);
                        xlPackage.Save();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        private void GenerarFileExcelHoja(ExcelPackage xlPackage, string hoja)
        {
            ExcelWorksheet ws = xlPackage.Workbook.Worksheets[hoja];
           
            switch (hoja)
            {
                case ConstantesEvaluacionAppServicio.HojaReactor:
                    //obtener empresas
                    var listaempresas = FactorySic.GetEqEquipoRepository().ListarMaestroEquiposReactor();
                    var listaFormatoEmpresa = listaempresas.Select(x => new { x.Emprabrev, x.Areanomb, x.Equinomb, x.Emprnomb });

                    ws.Cells[2, 1].LoadFromCollection(listaFormatoEmpresa, false);
                    
                    break;
                case ConstantesEvaluacionAppServicio.HojaCelda:
                    //obtener familias
                    var listaFamilias = FactorySic.GetEqEquipoRepository().ListarMaestroCeldaProteccion();

                    var listaFormatoCelda = listaFamilias.Select(x => new { x.Emprabrev, x.Areanomb, x.Equinomb, x.Emprnomb });

                    ws.Cells[2, 1].LoadFromCollection(listaFormatoCelda, false);
                  
                    break;              

               
                case ConstantesEvaluacionAppServicio.HojaProyecto:
                    //obtener Subestación
                    var listaMaestroProyecto = FactorySic.GetEprProyectoActEqpRepository().ListMaestroProyecto();

                    var listaFormatoProyecto = listaMaestroProyecto.Select(x => new { x.Epproydescripcion, x.Epproynemotecnico, x.Epproynomb, x.Emprnomb, x.Epproyfecregistro });
                    ws.Cells[2, 1].LoadFromCollection(listaFormatoProyecto, false);
                   
                    break;
            }

        }

        /// <summary>
        /// Metodo para validar información de cada registro del archivo de carga
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string ValidarCargaMasivaReactor(EprCargaMasivaLineaDTO entity)
        {
            return FactorySic.GetEprCargaMasivaRepository().ValidarCargaMasivaReactor(entity);
        }

        /// <summary>
        /// Metodo para grabar cada registro del srchivo de carga
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string SaveCargaMasivaReactor(EprCargaMasivaLineaDTO entity)
        {
            return FactorySic.GetEprCargaMasivaRepository().SaveCargaMasivaReactor(entity);
        }

        /// <summary>
        /// Generar Excel de reporte plantilla
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <param name="equicodi"></param>
        /// <param name="codigo"></param>
        /// <param name="emprcodi"></param>
        /// <param name="equiestado"></param>
        /// <param name="idsubestacion"></param>       
        /// <param name="idarea"></param>      
        /// <exception cref="Exception"></exception>
        public void GenerarExcelExportar(string path, string fileName, string equicodi, string codigo, string emprcodi,
            string equiestado, string idsubestacion, string idarea)
        {
            try
            {
                List<string> hojas = new List<string>();
                hojas.Add(ConstantesEvaluacionAppServicio.HojaNorte);
                hojas.Add(ConstantesEvaluacionAppServicio.HojaCentro);
                hojas.Add(ConstantesEvaluacionAppServicio.HojaSur);

                string file = path + fileName;

                FileInfo fi = new FileInfo(file);
                // Revisar si existe
                if (!fi.Exists)
                {
                    throw new Exception("Archivo " + fileName + " No existe");
                }

                using (ExcelPackage xlPackage = new ExcelPackage(fi))
                {

                    foreach (var item in hojas)
                    {

                        switch (item)
                        {
                            case ConstantesEvaluacionAppServicio.HojaNorte:
                                //obtener empresas
                                var lista = FactorySic.GetEprCargaMasivaRepository().ListReactorEvaluacionReporte(equicodi, codigo, emprcodi, equiestado,
                                    idsubestacion,  idarea, ConstantesEvaluacionAppServicio.CodigoAreaNorte.ToString());
                                GenerarFileExcelHojaExportar(xlPackage, item, lista);

                                break;
                            case ConstantesEvaluacionAppServicio.HojaCentro:
                                //obtener Ubicaciones
                                var listaCentro = FactorySic.GetEprCargaMasivaRepository().ListReactorEvaluacionReporte(equicodi, codigo, emprcodi, equiestado,
                                    idsubestacion, idarea, ConstantesEvaluacionAppServicio.CodigoAreaCentro.ToString());
                                GenerarFileExcelHojaExportar(xlPackage, item, listaCentro);

                                break;
                            case ConstantesEvaluacionAppServicio.HojaSur:
                                //obtener familias
                                var listaSur = FactorySic.GetEprCargaMasivaRepository().ListReactorEvaluacionReporte(equicodi, codigo, emprcodi, equiestado,
                                    idsubestacion, idarea, ConstantesEvaluacionAppServicio.CodigoAreaSur.ToString());
                                GenerarFileExcelHojaExportar(xlPackage, item, listaSur);

                                break;
                        }

                        xlPackage.Save();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        private void GenerarFileExcelHojaExportar(ExcelPackage xlPackage, string hoja, List<EprEquipoReactorReporteDTO> listaReactor)
        {
            ExcelWorksheet ws = xlPackage.Workbook.Worksheets[hoja];

            if (listaReactor.Count == 0) return;

            int index = 7;
            var nombreUsuario = "";

            var listaCodigos = string.Join(",", listaReactor.Select(p => Convert.ToInt32(p.Codigo_Id)));
            var listaFormulas = FactorySic.GetEprCalculosRepository().ListCalculosFormulasReactorMasivo(listaCodigos, ConstantesEvaluacionAppServicio.FAMCODI_REACTOR);

            var diccionarioFormulas = new Dictionary<int, List<EprCalculosDTO>>();

            foreach (var item in listaReactor.Select(p => Convert.ToInt32(p.Codigo_Id)).Distinct())
            {
                diccionarioFormulas[item] = listaFormulas.Where(p => p.Equicodi == item).ToList();
            }

            foreach (var item in listaReactor)
            {

                var calculosEquipo = diccionarioFormulas[Convert.ToInt32(item.Codigo_Id)];
                n_calc.EvaluarFormulas(calculosEquipo);

                #region Asignacion Datos Calculados

                item.Capacidad_Transmision_A = string.Empty;
                item.Capacidad_Transmision_Mvar = string.Empty;
                item.Factor_Limitante_Calc = string.Empty;
                item.Factor_Limitante_Final = string.Empty;

                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NombreCapacidadTransmisionA && p.Estado == 1))
                {
                    item.Capacidad_Transmision_A = Convert.ToDouble(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NombreCapacidadTransmisionA && p.Estado == 1).Valor).ToString("N2");
                }               

                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NombreCapacidadTransmisionMvar && p.Estado == 1))
                {
                    item.Capacidad_Transmision_Mvar = Convert.ToDouble(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NombreCapacidadTransmisionMvar && p.Estado == 1).Valor).ToString("N2");
                }               

                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NombreFactorLimitanteCalc && p.Estado == 1))
                {
                    item.Factor_Limitante_Calc = calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NombreFactorLimitanteCalc && p.Estado == 1).Valor.ToString();
                }               

                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NombreFactorLimitanteFinal && p.Estado == 1))
                {
                    item.Factor_Limitante_Final = calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NombreFactorLimitanteFinal && p.Estado == 1).Valor.ToString();
                }               

                #endregion

                ws.Cells[index, 1].Value = item.Codigo_Id;
                ws.Cells[index, 2].Value = item.Codigo;
                ws.Cells[index, 3].Value = item.Ubicacion;
                ws.Cells[index, 4].Value = item.Area;
                ws.Cells[index, 5].Value = item.Empresa;                       
                               
                ws.Cells[index, 6].Value = item.Id_Celda_1;
                ws.Cells[index, 7].Value = item.Nombre_Celda_1;
                ws.Cells[index, 8].Value = item.Ubicacion_Celda_1;
                ws.Cells[index, 9].Value = item.Posicion_Nucleo_Tc_Celda_1;
                ws.Cells[index, 10].Value = item.Pick_Up_Celda_1;
                ws.Cells[index, 11].Value = item.Id_Celda_2;
                ws.Cells[index, 12].Value = item.Nombre_Celda_2;
                ws.Cells[index, 13].Value = item.Ubicacion_Celda_2;
                ws.Cells[index, 14].Value = item.Posicion_Nucleo_Tc_Celda_2;
                ws.Cells[index, 15].Value = item.Pick_Up_Celda_2;

                ws.Cells[index, 16].Value = item.Tension;
                if (!string.IsNullOrEmpty(item.Tension_Coment)) ws.Cells[index, 16].AddComment(item.Tension_Coment, nombreUsuario);               
                ws.Cells[index, 17].Value = item.Capacidad_Mvar;
                if (!string.IsNullOrEmpty(item.Capacidad_Mvar_Coment)) ws.Cells[index, 17].AddComment(item.Capacidad_Mvar_Coment, nombreUsuario);
                ws.Cells[index, 18].Value = item.Capacidad_A;
                if (!string.IsNullOrEmpty(item.Capacidad_A_Coment)) ws.Cells[index, 18].AddComment(item.Capacidad_A_Coment, nombreUsuario);

                ws.Cells[index, 19].Value = item.Capacidad_Transmision_A;
                if (!string.IsNullOrEmpty(item.Capacidad_Transmision_A_Coment)) ws.Cells[index, 19].AddComment(item.Capacidad_Transmision_A_Coment, nombreUsuario);
                ws.Cells[index, 20].Value = item.Capacidad_Transmision_Mvar;
                if (!string.IsNullOrEmpty(item.Capacidad_Transmision_Mvar_Coment)) ws.Cells[index, 20].AddComment(item.Capacidad_Transmision_Mvar_Coment, nombreUsuario);
                               
                ws.Cells[index, 21].Value = item.Factor_Limitante_Calc;
                if (!string.IsNullOrEmpty(item.Factor_Limitante_Calc_Coment)) ws.Cells[index, 21].AddComment(item.Factor_Limitante_Calc_Coment, nombreUsuario);
                ws.Cells[index, 22].Value = item.Factor_Limitante_Final;
                if (!string.IsNullOrEmpty(item.Factor_Limitante_Final_Coment)) ws.Cells[index, 22].AddComment(item.Factor_Limitante_Final_Coment, nombreUsuario);

                ws.Cells[index, 23].Value = item.Observaciones;
                ws.Cells[index, 24].Value = item.Motivo;
                ws.Cells[index, 25].Value = item.Usuario_Auditoria;
                ws.Cells[index, 26].Value = item.Fecha_Modificacion;

                index++;
            }

        }


        #endregion
    }
}
