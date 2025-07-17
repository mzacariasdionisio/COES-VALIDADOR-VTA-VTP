using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Evaluacion.Helper;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Proteccion.Helper;
using log4net;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace COES.Servicios.Aplicacion.Evaluacion
{
    /// <summary>
    /// Clases con métodos del módulo Evaluación
    /// </summary>
    public class TransformadorAppServicio : AppServicioBase
    {

        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(TransformadorAppServicio));

        CalculosAppServicio calculo = new CalculosAppServicio();

        #region GESPROTEC
        /// <summary>
        /// Devuelve el listado de los transformadores
        /// </summary>
        /// <returns></returns>
        public List<EprEquipoDTO> ListaBuscarTransformador(int tipo, string codigoId, string codigo, int ubicacion,
            int empresa, string estado)
        {
            return FactorySic.GetEprEquipoRepository().ListaBuscarTransformador(tipo, codigoId, codigo, ubicacion,
            empresa, estado);
        }

        /// <summary>
        /// Devuelve el listado de los tipos
        /// </summary>
        /// <returns></returns>
        public List<EqFamiliaDTO> ListaTransformadores()
        {
            return FactorySic.GetEprEquipoRepository().ListaTransformadores();
        }

        /// <summary>
        /// Permite registrar el Transformador
        /// </summary>
        /// <returns></returns>
        public string RegistrarTransformador(EprEquipoDTO equipo)
        {
            return FactorySic.GetEprEquipoRepository().RegistrarTransformador(equipo);
        }

        /// <summary>
        /// Permite obtener la lista inicial del Transformador
        /// </summary>
        /// <returns></returns>
        public List<EprEquipoDTO> ListaTransformador(string codigoId, string codigo, int tipo, int subestacion,
            int empresa, int area, string estado, string tension, int incluirCalcular)
        {
            var listaTransformador = FactorySic.GetEprEquipoRepository().ListaTransformador(codigoId, codigo, tipo, subestacion,
            empresa, area, estado, tension);

           if(incluirCalcular > 0)
            {
                var diccionarioPorFamcodi = listaTransformador
                    .GroupBy(e => e.Famcodi) 
                    .ToDictionary(
                        grupo => grupo.Key, 
                        grupo => grupo.Select(e => e.Equicodi.Value).ToList() 
                    );

                var diccionarioFormulasDosDevanados = new Dictionary<int, List<EprCalculosDTO>>();

                if (diccionarioPorFamcodi.ContainsKey(ConstantesEvaluacionAppServicio.FAMCODI_TRANSFORMADOR_DOS_DEVANADOS))
                {
                    var listaCodigosDosDevanados = string.Join(",", diccionarioPorFamcodi[ConstantesEvaluacionAppServicio.FAMCODI_TRANSFORMADOR_DOS_DEVANADOS].Distinct());
                    var listaFormulasDosDevanados = FactorySic.GetEprCalculosRepository().ListCalculosFormulasTransformadoDosDevanadosMasivo(listaCodigosDosDevanados, ConstantesEvaluacionAppServicio.FAMCODI_TRANSFORMADOR_DOS_DEVANADOS);

                    foreach (var item in diccionarioPorFamcodi[ConstantesEvaluacionAppServicio.FAMCODI_TRANSFORMADOR_DOS_DEVANADOS].Distinct())
                    {
                        diccionarioFormulasDosDevanados[item] = listaFormulasDosDevanados.Where(p => p.Equicodi == item).ToList();
                    }
                }


                var diccionarioFormulasTresDevanados = new Dictionary<int, List<EprCalculosDTO>>();

                if (diccionarioPorFamcodi.ContainsKey(ConstantesEvaluacionAppServicio.FAMCODI_TRANSFORMADOR_TRES_DEVANADOS))
                {
                    var listaCodigosTresDevanados = string.Join(",", diccionarioPorFamcodi[ConstantesEvaluacionAppServicio.FAMCODI_TRANSFORMADOR_TRES_DEVANADOS].Distinct());
                    var listaFormulasTresDevanaTres = FactorySic.GetEprCalculosRepository().ListCalculosFormulasTransformadoTresDevanadosMasivo(listaCodigosTresDevanados, ConstantesEvaluacionAppServicio.FAMCODI_TRANSFORMADOR_TRES_DEVANADOS);

                    foreach (var item in diccionarioPorFamcodi[ConstantesEvaluacionAppServicio.FAMCODI_TRANSFORMADOR_TRES_DEVANADOS].Distinct())
                    {
                        diccionarioFormulasTresDevanados[item] = listaFormulasTresDevanaTres.Where(p => p.Equicodi == item).ToList();
                    }
                }


                var diccionarioFormulasCuatroDevanados = new Dictionary<int, List<EprCalculosDTO>>();

                if (diccionarioPorFamcodi.ContainsKey(ConstantesEvaluacionAppServicio.FAMCODI_TRANSFORMADOR_CUATRO_DEVANADOS))
                {
                    var listaCodigosCuatroDevanados = string.Join(",", diccionarioPorFamcodi[ConstantesEvaluacionAppServicio.FAMCODI_TRANSFORMADOR_CUATRO_DEVANADOS].Distinct());
                    var listaFormulasCuatroDevanaCuatro = FactorySic.GetEprCalculosRepository().ListCalculosFormulasTransformadoCuatroDevanadosMasivo(listaCodigosCuatroDevanados, ConstantesEvaluacionAppServicio.FAMCODI_TRANSFORMADOR_CUATRO_DEVANADOS);


                    foreach (var item in diccionarioPorFamcodi[ConstantesEvaluacionAppServicio.FAMCODI_TRANSFORMADOR_CUATRO_DEVANADOS].Distinct())
                    {
                        diccionarioFormulasCuatroDevanados[item] = listaFormulasCuatroDevanaCuatro.Where(p => p.Equicodi == item).ToList();
                    }
                }
                   

                foreach (var item in listaTransformador)
                {                 

                    switch (item.Famcodi)
                    {
                        case ConstantesEvaluacionAppServicio.FAMCODI_TRANSFORMADOR_DOS_DEVANADOS:
                            {
                                item.D1CapacidadA = "";
                                item.D2CapacidadA = "";                              

                                item.D1CapacidadMva = "";
                                item.D2CapacidadMva = "";                               

                                item.D1FactorLimitanteFinal = "";
                                item.D2FactorLimitanteFinal = "";                              

                                var calculosEquipo = diccionarioFormulasDosDevanados[item.Equicodi.Value];
                                n_calc.EvaluarFormulas(calculosEquipo);

                                #region Devanado 1

                                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D1_CAPACIDAD_A && p.Estado == 1))
                                {
                                    item.D1CapacidadA = EvaluacionHelper.RedondearValor(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D1_CAPACIDAD_A && p.Estado == 1).Valor, 2);

                                }

                                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D1_CAPACIDAD_MVA && p.Estado == 1))
                                {
                                    item.D1CapacidadMva = EvaluacionHelper.RedondearValor(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D1_CAPACIDAD_MVA && p.Estado == 1).Valor, 2);

                                }

                                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D1_FACTOR_LIMITANTE_FINAL && p.Estado == 1))
                                {
                                    item.D1FactorLimitanteFinal = calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D1_FACTOR_LIMITANTE_FINAL && p.Estado == 1).Valor.ToString();

                                }

                                #endregion

                                #region Devanado 2


                                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D2_CAPACIDAD_A && p.Estado == 1))
                                {
                                    item.D2CapacidadA = EvaluacionHelper.RedondearValor(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D2_CAPACIDAD_A && p.Estado == 1).Valor, 2);

                                }

                                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D2_CAPACIDAD_MVA && p.Estado == 1))
                                {
                                    item.D2CapacidadMva = EvaluacionHelper.RedondearValor(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D2_CAPACIDAD_MVA && p.Estado == 1).Valor, 2);

                                }

                                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D2_FACTOR_LIMITANTE_FINAL && p.Estado == 1))
                                {
                                    item.D2FactorLimitanteFinal = calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D2_FACTOR_LIMITANTE_FINAL && p.Estado == 1).Valor.ToString();

                                }

                                #endregion

                                break;
                            }
                        case ConstantesEvaluacionAppServicio.FAMCODI_TRANSFORMADOR_TRES_DEVANADOS:
                            {
                                item.D1CapacidadA = "";
                                item.D2CapacidadA = "";
                                item.D3CapacidadA = "";                              

                                item.D1CapacidadMva = "";
                                item.D2CapacidadMva = "";
                                item.D3CapacidadMva = "";                               

                                item.D1FactorLimitanteFinal = "";
                                item.D2FactorLimitanteFinal = "";
                                item.D3FactorLimitanteFinal = "";                               

                                var calculosEquipo = diccionarioFormulasTresDevanados[item.Equicodi.Value];
                                n_calc.EvaluarFormulas(calculosEquipo);

                                #region Devanado 1

                                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D1_CAPACIDAD_A && p.Estado == 1))
                                {
                                    item.D1CapacidadA = EvaluacionHelper.RedondearValor(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D1_CAPACIDAD_A && p.Estado == 1).Valor, 2);

                                }

                                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D1_CAPACIDAD_MVA && p.Estado == 1))
                                {
                                    item.D1CapacidadMva = EvaluacionHelper.RedondearValor(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D1_CAPACIDAD_MVA && p.Estado == 1).Valor, 2);

                                }

                                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D1_FACTOR_LIMITANTE_FINAL && p.Estado == 1))
                                {
                                    item.D1FactorLimitanteFinal = calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D1_FACTOR_LIMITANTE_FINAL && p.Estado == 1).Valor.ToString();

                                }

                                #endregion

                                #region Devanado 2


                                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D2_CAPACIDAD_A && p.Estado == 1))
                                {
                                    item.D2CapacidadA = EvaluacionHelper.RedondearValor(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D2_CAPACIDAD_A && p.Estado == 1).Valor, 2);

                                }

                                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D2_CAPACIDAD_MVA && p.Estado == 1))
                                {
                                    item.D2CapacidadMva = EvaluacionHelper.RedondearValor(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D2_CAPACIDAD_MVA && p.Estado == 1).Valor, 2);

                                }

                                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D2_FACTOR_LIMITANTE_FINAL && p.Estado == 1))
                                {
                                    item.D2FactorLimitanteFinal = calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D2_FACTOR_LIMITANTE_FINAL && p.Estado == 1).Valor.ToString();

                                }

                                #endregion

                                #region Devanado 3


                                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D3_CAPACIDAD_A && p.Estado == 1))
                                {
                                    item.D3CapacidadA = EvaluacionHelper.RedondearValor(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D3_CAPACIDAD_A && p.Estado == 1).Valor, 2);

                                }

                                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D3_CAPACIDAD_MVA && p.Estado == 1))
                                {
                                    item.D3CapacidadMva = EvaluacionHelper.RedondearValor(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D3_CAPACIDAD_MVA && p.Estado == 1).Valor, 2);

                                }

                                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D3_FACTOR_LIMITANTE_FINAL && p.Estado == 1))
                                {
                                    item.D3FactorLimitanteFinal = calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D3_FACTOR_LIMITANTE_FINAL && p.Estado == 1).Valor.ToString();

                                }

                                #endregion

                                break;
                            }
                        case ConstantesEvaluacionAppServicio.FAMCODI_TRANSFORMADOR_CUATRO_DEVANADOS:
                            {
                                item.D1CapacidadA = "";
                                item.D2CapacidadA = "";
                                item.D3CapacidadA = "";
                                item.D4CapacidadA = "";

                                item.D1CapacidadMva = "";
                                item.D2CapacidadMva = "";
                                item.D3CapacidadMva = "";
                                item.D4CapacidadMva = "";

                                item.D1FactorLimitanteFinal = "";
                                item.D2FactorLimitanteFinal = "";
                                item.D3FactorLimitanteFinal = "";
                                item.D4FactorLimitanteFinal = "";

                                var calculosEquipo = diccionarioFormulasCuatroDevanados[item.Equicodi.Value];
                                n_calc.EvaluarFormulas(calculosEquipo);

                                #region Devanado 1

                                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D1_CAPACIDAD_A && p.Estado == 1))
                                {
                                    item.D1CapacidadA = EvaluacionHelper.RedondearValor(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D1_CAPACIDAD_A && p.Estado == 1).Valor, 2);
                                }                              
                               
                                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D1_CAPACIDAD_MVA && p.Estado == 1))
                                {
                                    item.D1CapacidadMva = EvaluacionHelper.RedondearValor(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D1_CAPACIDAD_MVA && p.Estado == 1).Valor, 2);
                                   
                                }
                               
                                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D1_FACTOR_LIMITANTE_FINAL && p.Estado == 1))
                                {
                                    item.D1FactorLimitanteFinal = calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D1_FACTOR_LIMITANTE_FINAL && p.Estado == 1).Valor.ToString();
                                    
                                }                               

                                #endregion

                                #region Devanado 2


                                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D2_CAPACIDAD_A && p.Estado == 1))
                                {
                                    item.D2CapacidadA = EvaluacionHelper.RedondearValor(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D2_CAPACIDAD_A && p.Estado == 1).Valor, 2);
                                  
                                }
                               
                                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D2_CAPACIDAD_MVA && p.Estado == 1))
                                {
                                    item.D2CapacidadMva = EvaluacionHelper.RedondearValor(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D2_CAPACIDAD_MVA && p.Estado == 1).Valor, 2);
                                   
                                }
                                
                                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D2_FACTOR_LIMITANTE_FINAL && p.Estado == 1))
                                {
                                    item.D2FactorLimitanteFinal = calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D2_FACTOR_LIMITANTE_FINAL && p.Estado == 1).Valor.ToString();
                                   
                                }                               

                                #endregion

                                #region Devanado 3


                                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D3_CAPACIDAD_A && p.Estado == 1))
                                {
                                    item.D3CapacidadA = EvaluacionHelper.RedondearValor(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D3_CAPACIDAD_A && p.Estado == 1).Valor, 2);
                                   
                                }
                               
                                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D3_CAPACIDAD_MVA && p.Estado == 1))
                                {
                                    item.D3CapacidadMva = EvaluacionHelper.RedondearValor(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D3_CAPACIDAD_MVA && p.Estado == 1).Valor, 2);
                                   
                                }                               

                                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D3_FACTOR_LIMITANTE_FINAL && p.Estado == 1))
                                {
                                    item.D3FactorLimitanteFinal = calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D3_FACTOR_LIMITANTE_FINAL && p.Estado == 1).Valor.ToString();
                                   
                                }                               

                                #endregion

                                #region Devanado 4


                                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D4_CAPACIDAD_A && p.Estado == 1))
                                {
                                    item.D4CapacidadA = EvaluacionHelper.RedondearValor(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D4_CAPACIDAD_A && p.Estado == 1).Valor, 2);
                                   
                                }                              

                                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D4_CAPACIDAD_MVA && p.Estado == 1))
                                {
                                    item.D4CapacidadMva = EvaluacionHelper.RedondearValor(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D4_CAPACIDAD_MVA && p.Estado == 1).Valor, 2);
                                    
                                }

                                if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D4_FACTOR_LIMITANTE_FINAL && p.Estado == 1))
                                {
                                    item.D4FactorLimitanteFinal = calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D4_FACTOR_LIMITANTE_FINAL && p.Estado == 1).Valor.ToString();
                                  
                                }
                             

                                #endregion

                                break;
                            }


                    }
                }
            }
            

            return listaTransformador;
        }

        /// <summary>
        /// Permite obtener el detalle del transformador por id
        /// </summary>
        /// <returns></returns>
        public EprEquipoDTO ObtenerTransformadorPorId(int codigoId)
        {
            return FactorySic.GetEprEquipoRepository().ObtenerTransformadorPorId(codigoId);
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
                hojas.Add(ConstantesEvaluacionAppServicio.HojaTransformador);              
                hojas.Add(ConstantesEvaluacionAppServicio.HojaCelda);              
                hojas.Add(ConstantesProteccionAppServicio.HojaProyecto);

                string file = path + fileName;

                FileInfo fi = new FileInfo(file);
                // Revisar si existe
                if (!fi.Exists)
                {
                    throw new Exception("Archivo " + fileName + " No existe");
                }

                using (ExcelPackage xlPackage = new ExcelPackage(fi))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets[ConstantesProteccionAppServicio.HojaPlantillaExcel];

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
                case ConstantesEvaluacionAppServicio.HojaTransformador:
                    //obtener empresas
                    var listaempresas = FactorySic.GetEqEquipoRepository().ListarMaestroEquiposTransformador();
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
        public string ValidarCargaMasivaTransformador(EprCargaMasivaTransformadorDTO entity)
        {
            return FactorySic.GetEprCargaMasivaRepository().ValidarCargaMasivaTransformador(entity);
        }

        /// <summary>
        /// Metodo para grabar cada registro del srchivo de carga
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string SaveCargaMasivaTransformador(EprCargaMasivaTransformadorDTO entity)
        {
            return FactorySic.GetEprCargaMasivaRepository().SaveCargaMasivaTransformador(entity);
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
        /// <param name="tipo"></param>
        /// <param name="idsubestacion"></param>
        /// <param name="idarea"></param>
        ///  /// <param name="tension"></param>
        /// <exception cref="Exception"></exception>
        public void GenerarExcelExportar(string path, string fileName, string equicodi, string codigo, string emprcodi,
            string equiestado, string tipo, string idsubestacion, string idarea, string tension)
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
                                var lista = FactorySic.GetEprCargaMasivaRepository().ListTransformadoresEvaluacionReporte(equicodi, codigo, emprcodi, equiestado,
                                    tipo, idsubestacion, idarea, ConstantesEvaluacionAppServicio.CodigoAreaNorte.ToString(), tension);
                                GenerarFileExcelHojaExportar(xlPackage, item, lista);

                                break;
                            case ConstantesEvaluacionAppServicio.HojaCentro:
                                //obtener Ubicaciones
                                var listaCentro = FactorySic.GetEprCargaMasivaRepository().ListTransformadoresEvaluacionReporte(equicodi, codigo, emprcodi, equiestado,
                                    tipo, idsubestacion, idarea, ConstantesEvaluacionAppServicio.CodigoAreaCentro.ToString(), tension);
                                GenerarFileExcelHojaExportar(xlPackage, item, listaCentro);

                                break;
                            case ConstantesEvaluacionAppServicio.HojaSur:
                                //obtener familias
                                var listaSur = FactorySic.GetEprCargaMasivaRepository().ListTransformadoresEvaluacionReporte(equicodi, codigo, emprcodi, equiestado,
                                    tipo, idsubestacion, idarea, ConstantesEvaluacionAppServicio.CodigoAreaSur.ToString(), tension);
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

        private void GenerarFileExcelHojaExportar(ExcelPackage xlPackage, string hoja, List<EprEquipoTransformadoresReporteDTO> listaTransformador)
        {
            ExcelWorksheet ws = xlPackage.Workbook.Worksheets[hoja];

            if (listaTransformador.Count == 0) return;

            int index = 7;
            var nombreUsuario = "";

            var diccionarioPorFamcodi = listaTransformador
                    .GroupBy(e => e.Famcodi)
                    .ToDictionary(
                        grupo => grupo.Key,
                        grupo => grupo.Select(e => Convert.ToInt32(e.Codigo_Id)).ToList()
                    );

            var diccionarioFormulasDosDevanados = new Dictionary<int, List<EprCalculosDTO>>();

            if (diccionarioPorFamcodi.ContainsKey(ConstantesEvaluacionAppServicio.FAMCODI_TRANSFORMADOR_DOS_DEVANADOS))
            {
                var listaCodigosDosDevanados = string.Join(",", diccionarioPorFamcodi[ConstantesEvaluacionAppServicio.FAMCODI_TRANSFORMADOR_DOS_DEVANADOS].Distinct());
                var listaFormulasDosDevanados = FactorySic.GetEprCalculosRepository().ListCalculosFormulasTransformadoDosDevanadosMasivo(listaCodigosDosDevanados, ConstantesEvaluacionAppServicio.FAMCODI_TRANSFORMADOR_DOS_DEVANADOS);

                foreach (var item in diccionarioPorFamcodi[ConstantesEvaluacionAppServicio.FAMCODI_TRANSFORMADOR_DOS_DEVANADOS].Distinct())
                {
                    diccionarioFormulasDosDevanados[item] = listaFormulasDosDevanados.Where(p => p.Equicodi == item).ToList();
                }
            }


            var diccionarioFormulasTresDevanados = new Dictionary<int, List<EprCalculosDTO>>();

            if (diccionarioPorFamcodi.ContainsKey(ConstantesEvaluacionAppServicio.FAMCODI_TRANSFORMADOR_TRES_DEVANADOS))
            {
                var listaCodigosTresDevanados = string.Join(",", diccionarioPorFamcodi[ConstantesEvaluacionAppServicio.FAMCODI_TRANSFORMADOR_TRES_DEVANADOS].Distinct());
                var listaFormulasTresDevanaTres = FactorySic.GetEprCalculosRepository().ListCalculosFormulasTransformadoTresDevanadosMasivo(listaCodigosTresDevanados, ConstantesEvaluacionAppServicio.FAMCODI_TRANSFORMADOR_TRES_DEVANADOS);

                foreach (var item in diccionarioPorFamcodi[ConstantesEvaluacionAppServicio.FAMCODI_TRANSFORMADOR_TRES_DEVANADOS].Distinct())
                {
                    diccionarioFormulasTresDevanados[item] = listaFormulasTresDevanaTres.Where(p => p.Equicodi == item).ToList();
                }
            }


            var diccionarioFormulasCuatroDevanados = new Dictionary<int, List<EprCalculosDTO>>();

            if (diccionarioPorFamcodi.ContainsKey(ConstantesEvaluacionAppServicio.FAMCODI_TRANSFORMADOR_CUATRO_DEVANADOS))
            {
                var listaCodigosCuatroDevanados = string.Join(",", diccionarioPorFamcodi[ConstantesEvaluacionAppServicio.FAMCODI_TRANSFORMADOR_CUATRO_DEVANADOS].Distinct());
                var listaFormulasCuatroDevanaCuatro = FactorySic.GetEprCalculosRepository().ListCalculosFormulasTransformadoCuatroDevanadosMasivo(listaCodigosCuatroDevanados, ConstantesEvaluacionAppServicio.FAMCODI_TRANSFORMADOR_CUATRO_DEVANADOS);


                foreach (var item in diccionarioPorFamcodi[ConstantesEvaluacionAppServicio.FAMCODI_TRANSFORMADOR_CUATRO_DEVANADOS].Distinct())
                {
                    diccionarioFormulasCuatroDevanados[item] = listaFormulasCuatroDevanaCuatro.Where(p => p.Equicodi == item).ToList();
                }
            }

            foreach (var item in listaTransformador)
            {             
                switch (Convert.ToInt32(item.Famcodi))
                {
                    case ConstantesEvaluacionAppServicio.FAMCODI_TRANSFORMADOR_DOS_DEVANADOS:
                        {
                            var calculosEquipo = diccionarioFormulasDosDevanados[Convert.ToInt32(item.Codigo_Id)];
                            n_calc.EvaluarFormulas(calculosEquipo);

                            item.D1_Capacidad_Mva = "";
                            item.D1_Capacidad_A = "";
                            item.D1_Factor_Limitante_Calc = "";
                            item.D1_Factor_Limitante_Final = "";

                            item.D2_Capacidad_Mva = "";
                            item.D2_Capacidad_A = "";
                            item.D2_Factor_Limitante_Calc = "";
                            item.D2_Factor_Limitante_Final = "";

                            #region Devanado 1

                            if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D1_CAPACIDAD_MVA && p.Estado == 1))
                            {
                                item.D1_Capacidad_Mva = Convert.ToDouble(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D1_CAPACIDAD_MVA && p.Estado == 1).Valor).ToString("N2");
                            }

                            if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D1_CAPACIDAD_A && p.Estado == 1))
                            {
                                item.D1_Capacidad_A = Convert.ToDouble(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D1_CAPACIDAD_A && p.Estado == 1).Valor).ToString("N2");
                            }                        

                            if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D1_FACTOR_LIMITANTE_CALC && p.Estado == 1))
                            {
                                item.D1_Factor_Limitante_Calc = calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D1_FACTOR_LIMITANTE_CALC && p.Estado == 1).Valor.ToString();
                            }

                            if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D1_FACTOR_LIMITANTE_FINAL && p.Estado == 1))
                            {
                                item.D1_Factor_Limitante_Final = calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D1_FACTOR_LIMITANTE_FINAL && p.Estado == 1).Valor.ToString();
                            }


                            #endregion

                            #region Devanado 2

                            if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D2_CAPACIDAD_MVA && p.Estado == 1))
                            {
                                item.D2_Capacidad_Mva = Convert.ToDouble(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D2_CAPACIDAD_MVA && p.Estado == 1).Valor).ToString("N2");
                            }

                            if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D2_CAPACIDAD_A && p.Estado == 1))
                            {
                                item.D2_Capacidad_A = Convert.ToDouble(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D2_CAPACIDAD_A && p.Estado == 1).Valor).ToString("N2");
                            }                      


                            if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D2_FACTOR_LIMITANTE_CALC && p.Estado == 1))
                            {
                                item.D2_Factor_Limitante_Calc = calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D2_FACTOR_LIMITANTE_CALC && p.Estado == 1).Valor.ToString();
                            }

                            if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D2_FACTOR_LIMITANTE_FINAL && p.Estado == 1))
                            {
                                item.D2_Factor_Limitante_Final = calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D2_FACTOR_LIMITANTE_FINAL && p.Estado == 1).Valor.ToString();
                            }

                            #endregion

                            break;
                        }
                    case ConstantesEvaluacionAppServicio.FAMCODI_TRANSFORMADOR_TRES_DEVANADOS:
                        {
                            var calculosEquipo = diccionarioFormulasTresDevanados[Convert.ToInt32(item.Codigo_Id)];
                            n_calc.EvaluarFormulas(calculosEquipo);

                            item.D1_Capacidad_Mva = "";
                            item.D1_Capacidad_A = "";
                            item.D1_Factor_Limitante_Calc = "";
                            item.D1_Factor_Limitante_Final = "";

                            item.D2_Capacidad_Mva = "";
                            item.D2_Capacidad_A = "";
                            item.D2_Factor_Limitante_Calc = "";
                            item.D2_Factor_Limitante_Final = "";

                            item.D3_Capacidad_Mva = "";
                            item.D3_Capacidad_A = "";
                            item.D3_Factor_Limitante_Calc = "";
                            item.D3_Factor_Limitante_Final = "";

                            #region Devanado 1

                            if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D1_CAPACIDAD_MVA && p.Estado == 1))
                            {
                                item.D1_Capacidad_Mva = Convert.ToDouble(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D1_CAPACIDAD_MVA && p.Estado == 1).Valor).ToString("N2");
                            }

                            if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D1_CAPACIDAD_A && p.Estado == 1))
                            {
                                item.D1_Capacidad_A = Convert.ToDouble(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D1_CAPACIDAD_A && p.Estado == 1).Valor).ToString("N2");
                            }
                        

                            if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D1_FACTOR_LIMITANTE_CALC && p.Estado == 1))
                            {
                                item.D1_Factor_Limitante_Calc = calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D1_FACTOR_LIMITANTE_CALC && p.Estado == 1).Valor.ToString();
                            }

                            if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D1_FACTOR_LIMITANTE_FINAL && p.Estado == 1))
                            {
                                item.D1_Factor_Limitante_Final = calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D1_FACTOR_LIMITANTE_FINAL && p.Estado == 1).Valor.ToString();
                            }


                            #endregion

                            #region Devanado 2

                            if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D2_CAPACIDAD_MVA && p.Estado == 1))
                            {
                                item.D2_Capacidad_Mva = Convert.ToDouble(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D2_CAPACIDAD_MVA && p.Estado == 1).Valor).ToString("N2");
                            }

                            if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D2_CAPACIDAD_A && p.Estado == 1))
                            {
                                item.D2_Capacidad_A = Convert.ToDouble(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D2_CAPACIDAD_A && p.Estado == 1).Valor).ToString("N2");
                            }                                                  


                            if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D2_FACTOR_LIMITANTE_CALC && p.Estado == 1))
                            {
                                item.D2_Factor_Limitante_Calc = calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D2_FACTOR_LIMITANTE_CALC && p.Estado == 1).Valor.ToString();
                            }

                            if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D2_FACTOR_LIMITANTE_FINAL && p.Estado == 1))
                            {
                                item.D2_Factor_Limitante_Final = calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D2_FACTOR_LIMITANTE_FINAL && p.Estado == 1).Valor.ToString();
                            }

                            #endregion

                            #region Devanado 3

                            if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D3_CAPACIDAD_MVA && p.Estado == 1))
                            {
                                item.D3_Capacidad_Mva = Convert.ToDouble(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D3_CAPACIDAD_MVA && p.Estado == 1).Valor).ToString("N2");
                            }

                            if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D3_CAPACIDAD_A && p.Estado == 1))
                            {
                                item.D3_Capacidad_A = Convert.ToDouble(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D3_CAPACIDAD_A && p.Estado == 1).Valor).ToString("N2");
                            }                         


                            if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D3_FACTOR_LIMITANTE_CALC && p.Estado == 1))
                            {
                                item.D3_Factor_Limitante_Calc = calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D3_FACTOR_LIMITANTE_CALC && p.Estado == 1).Valor.ToString();
                            }

                            if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D3_FACTOR_LIMITANTE_FINAL && p.Estado == 1))
                            {
                                item.D3_Factor_Limitante_Final = calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D3_FACTOR_LIMITANTE_FINAL && p.Estado == 1).Valor.ToString();
                            }

                            #endregion

                            break;
                        }
                    case ConstantesEvaluacionAppServicio.FAMCODI_TRANSFORMADOR_CUATRO_DEVANADOS:
                        {
                            var calculosEquipo = diccionarioFormulasCuatroDevanados[Convert.ToInt32(item.Codigo_Id)];
                            n_calc.EvaluarFormulas(calculosEquipo);

                            item.D1_Capacidad_Mva = "";
                            item.D1_Capacidad_A = "";
                            item.D1_Factor_Limitante_Calc = "";
                            item.D1_Factor_Limitante_Final = "";

                            item.D2_Capacidad_Mva = "";
                            item.D2_Capacidad_A = "";
                            item.D2_Factor_Limitante_Calc = "";
                            item.D2_Factor_Limitante_Final = "";

                            item.D3_Capacidad_Mva = "";
                            item.D3_Capacidad_A = "";
                            item.D3_Factor_Limitante_Calc = "";
                            item.D3_Factor_Limitante_Final = "";

                            item.D4_Capacidad_Mva = "";
                            item.D4_Capacidad_A = "";
                            item.D4_Factor_Limitante_Calc = "";
                            item.D4_Factor_Limitante_Final = "";

                            #region Devanado 1

                            if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D1_CAPACIDAD_MVA && p.Estado == 1))
                            {
                                item.D1_Capacidad_Mva = Convert.ToDouble(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D1_CAPACIDAD_MVA && p.Estado == 1).Valor).ToString("N2");
                            }

                            if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D1_CAPACIDAD_A && p.Estado == 1))
                            {
                                item.D1_Capacidad_A = Convert.ToDouble(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D1_CAPACIDAD_A && p.Estado == 1).Valor).ToString("N2");
                            }


                            if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D1_FACTOR_LIMITANTE_CALC && p.Estado == 1))
                            {
                                item.D1_Factor_Limitante_Calc = calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D1_FACTOR_LIMITANTE_CALC && p.Estado == 1).Valor.ToString();
                            }

                            if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D1_FACTOR_LIMITANTE_FINAL && p.Estado == 1))
                            {
                                item.D1_Factor_Limitante_Final = calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D1_FACTOR_LIMITANTE_FINAL && p.Estado == 1).Valor.ToString();
                            }


                            #endregion

                            #region Devanado 2

                            if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D2_CAPACIDAD_MVA && p.Estado == 1))
                            {
                                item.D2_Capacidad_Mva = Convert.ToDouble(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D2_CAPACIDAD_MVA && p.Estado == 1).Valor).ToString("N2");
                            }

                            if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D2_CAPACIDAD_A && p.Estado == 1))
                            {
                                item.D2_Capacidad_A = Convert.ToDouble(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D2_CAPACIDAD_A && p.Estado == 1).Valor).ToString("N2");
                            }


                            if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D2_FACTOR_LIMITANTE_CALC && p.Estado == 1))
                            {
                                item.D2_Factor_Limitante_Calc = calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D2_FACTOR_LIMITANTE_CALC && p.Estado == 1).Valor.ToString();
                            }

                            if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D2_FACTOR_LIMITANTE_FINAL && p.Estado == 1))
                            {
                                item.D2_Factor_Limitante_Final = calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D2_FACTOR_LIMITANTE_FINAL && p.Estado == 1).Valor.ToString();
                            }

                            #endregion

                            #region Devanado 3

                            if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D3_CAPACIDAD_MVA && p.Estado == 1))
                            {
                                item.D3_Capacidad_Mva = Convert.ToDouble(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D3_CAPACIDAD_MVA && p.Estado == 1).Valor).ToString("N2");
                            }

                            if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D3_CAPACIDAD_A && p.Estado == 1))
                            {
                                item.D3_Capacidad_A = Convert.ToDouble(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D3_CAPACIDAD_A && p.Estado == 1).Valor).ToString("N2");
                            }                        


                            if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D3_FACTOR_LIMITANTE_CALC && p.Estado == 1))
                            {
                                item.D3_Factor_Limitante_Calc = calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D3_FACTOR_LIMITANTE_CALC && p.Estado == 1).Valor.ToString();
                            }

                            if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D3_FACTOR_LIMITANTE_FINAL && p.Estado == 1))
                            {
                                item.D3_Factor_Limitante_Final = calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D3_FACTOR_LIMITANTE_FINAL && p.Estado == 1).Valor.ToString();
                            }

                            #endregion

                            #region Devanado 4

                            if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D4_CAPACIDAD_MVA && p.Estado == 1))
                            {
                                item.D4_Capacidad_Mva = Convert.ToDouble(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D4_CAPACIDAD_MVA && p.Estado == 1).Valor).ToString("N2");
                            }

                            if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D4_CAPACIDAD_A && p.Estado == 1))
                            {
                                item.D4_Capacidad_A = Convert.ToDouble(calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D4_CAPACIDAD_A && p.Estado == 1).Valor).ToString("N2");
                            }
                                                     

                            if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D4_FACTOR_LIMITANTE_CALC && p.Estado == 1))
                            {
                                item.D4_Factor_Limitante_Calc = calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D4_FACTOR_LIMITANTE_CALC && p.Estado == 1).Valor.ToString();
                            }

                            if (calculosEquipo.Any(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D4_FACTOR_LIMITANTE_FINAL && p.Estado == 1))
                            {
                                item.D4_Factor_Limitante_Final = calculosEquipo.Find(p => p.Identificador.ToUpper() == ConstantesEvaluacionAppServicio.NOMBRE_D4_FACTOR_LIMITANTE_FINAL && p.Estado == 1).Valor.ToString();
                            }

                            #endregion

                            break;
                        }
                }

              

                ws.Cells[index, 1].Value = item.Codigo_Id;
                ws.Cells[index, 2].Value = item.Codigo;
                ws.Cells[index, 3].Value = item.Ubicacion;
                ws.Cells[index, 4].Value = item.Area;
                ws.Cells[index, 5].Value = item.Empresa;

                ws.Cells[index, 6].Value = item.D1_Id_Celda;
                ws.Cells[index, 7].Value = item.D1_Codigo_Celda;
                ws.Cells[index, 8].Value = item.D1_Ubicacion_Celda;
                ws.Cells[index, 9].Value = item.D1_Tension;
                if (!string.IsNullOrEmpty(item.D1_Tension_Coment)) ws.Cells[index, 9].AddComment(item.D1_Tension_Coment, nombreUsuario);
                ws.Cells[index, 10].Value = item.D1_Capacidad_Onan_Mva;
                if (!string.IsNullOrEmpty(item.D1_Capacidad_Onan_Mva_Coment)) ws.Cells[index, 10].AddComment(item.D1_Capacidad_Onan_Mva_Coment, nombreUsuario);
                ws.Cells[index, 11].Value = item.D1_Capacidad_Onaf_Mva;
                if (!string.IsNullOrEmpty(item.D1_Capacidad_Onaf_Mva_Coment)) ws.Cells[index, 11].AddComment(item.D1_Capacidad_Onaf_Mva_Coment, nombreUsuario);
                ws.Cells[index, 12].Value = item.D1_Capacidad_Mva;
                if (!string.IsNullOrEmpty(item.D1_Capacidad_Mva_Coment)) ws.Cells[index, 12].AddComment(item.D1_Capacidad_Mva_Coment, nombreUsuario);
                ws.Cells[index, 13].Value = item.D1_Capacidad_A;
                if (!string.IsNullOrEmpty(item.D1_Capacidad_A_Coment)) ws.Cells[index, 13].AddComment(item.D1_Capacidad_A_Coment, nombreUsuario);
                ws.Cells[index, 14].Value = item.D1_Posicion_Nucleo_Tc;
                ws.Cells[index, 15].Value = item.D1_Pick_Up;
                ws.Cells[index, 16].Value = item.D1_Factor_Limitante_Calc;
                if (!string.IsNullOrEmpty(item.D1_Factor_Limitante_Calc_Coment)) ws.Cells[index, 16].AddComment(item.D1_Factor_Limitante_Calc_Coment, nombreUsuario);
                ws.Cells[index, 17].Value = item.D1_Factor_Limitante_Final;
                if (!string.IsNullOrEmpty(item.D1_Factor_Limitante_Final_Coment)) ws.Cells[index, 17].AddComment(item.D1_Factor_Limitante_Final_Coment, nombreUsuario);

                ws.Cells[index, 18].Value = item.D2_Id_Celda;
                ws.Cells[index, 19].Value = item.D2_Codigo_Celda;
                ws.Cells[index, 20].Value = item.D2_Ubicacion_Celda;
                ws.Cells[index, 21].Value = item.D2_Tension;
                if (!string.IsNullOrEmpty(item.D2_Tension_Coment)) ws.Cells[index, 21].AddComment(item.D2_Tension_Coment, nombreUsuario);
                ws.Cells[index, 22].Value = item.D2_Capacidad_Onan_Mva;
                if (!string.IsNullOrEmpty(item.D2_Capacidad_Onan_Mva_Coment)) ws.Cells[index, 22].AddComment(item.D2_Capacidad_Onan_Mva_Coment, nombreUsuario);
                ws.Cells[index, 23].Value = item.D2_Capacidad_Onaf_Mva;
                if (!string.IsNullOrEmpty(item.D2_Capacidad_Onaf_Mva_Coment)) ws.Cells[index, 23].AddComment(item.D2_Capacidad_Onaf_Mva_Coment, nombreUsuario);
                ws.Cells[index, 24].Value = item.D2_Capacidad_Mva;
                if (!string.IsNullOrEmpty(item.D2_Capacidad_Mva_Coment)) ws.Cells[index, 24].AddComment(item.D2_Capacidad_Mva_Coment, nombreUsuario);
                ws.Cells[index, 25].Value = item.D2_Capacidad_A;
                if (!string.IsNullOrEmpty(item.D2_Capacidad_A_Coment)) ws.Cells[index, 25].AddComment(item.D2_Capacidad_A_Coment, nombreUsuario);
                ws.Cells[index, 26].Value = item.D2_Posicion_Nucleo_Tc;
                ws.Cells[index, 27].Value = item.D2_Pick_Up;
                ws.Cells[index, 28].Value = item.D2_Factor_Limitante_Calc;
                if (!string.IsNullOrEmpty(item.D2_Factor_Limitante_Calc_Coment)) ws.Cells[index, 28].AddComment(item.D2_Factor_Limitante_Calc_Coment, nombreUsuario);
                ws.Cells[index, 29].Value = item.D2_Factor_Limitante_Final;
                if (!string.IsNullOrEmpty(item.D2_Factor_Limitante_Final_Coment)) ws.Cells[index, 29].AddComment(item.D2_Factor_Limitante_Final_Coment, nombreUsuario);

                ws.Cells[index, 30].Value = item.D3_Id_Celda;
                ws.Cells[index, 31].Value = item.D3_Codigo_Celda;
                ws.Cells[index, 32].Value = item.D3_Ubicacion_Celda;
                ws.Cells[index, 33].Value = item.D3_Tension;
                if (!string.IsNullOrEmpty(item.D3_Tension_Coment)) ws.Cells[index, 33].AddComment(item.D3_Tension_Coment, nombreUsuario);
                ws.Cells[index, 34].Value = item.D3_Capacidad_Onan_Mva;
                if (!string.IsNullOrEmpty(item.D3_Capacidad_Onan_Mva_Coment)) ws.Cells[index, 34].AddComment(item.D3_Capacidad_Onan_Mva_Coment, nombreUsuario);
                ws.Cells[index, 35].Value = item.D3_Capacidad_Onaf_Mva;
                if (!string.IsNullOrEmpty(item.D3_Capacidad_Onaf_Mva_Coment)) ws.Cells[index, 35].AddComment(item.D3_Capacidad_Onaf_Mva_Coment, nombreUsuario);
                ws.Cells[index, 36].Value = item.D3_Capacidad_Mva;               
                if (!string.IsNullOrEmpty(item.D3_Capacidad_Mva_Coment)) ws.Cells[index, 36].AddComment(item.D3_Capacidad_Mva_Coment, nombreUsuario);
                ws.Cells[index, 37].Value = item.D3_Capacidad_A;
                if (!string.IsNullOrEmpty(item.D3_Capacidad_A_Coment)) ws.Cells[index, 37].AddComment(item.D3_Capacidad_A_Coment, nombreUsuario);
                ws.Cells[index, 38].Value = item.D3_Posicion_Nucleo_Tc;
                ws.Cells[index, 39].Value = item.D3_Pick_Up;
                ws.Cells[index, 40].Value = item.D3_Factor_Limitante_Calc;
                if (!string.IsNullOrEmpty(item.D3_Factor_Limitante_Calc_Coment)) ws.Cells[index, 40].AddComment(item.D3_Factor_Limitante_Calc_Coment, nombreUsuario);
                ws.Cells[index, 41].Value = item.D3_Factor_Limitante_Final;
                if (!string.IsNullOrEmpty(item.D3_Factor_Limitante_Final_Coment)) ws.Cells[index, 41].AddComment(item.D3_Factor_Limitante_Final_Coment, nombreUsuario);

                ws.Cells[index, 42].Value = item.D4_Id_Celda;
                ws.Cells[index, 43].Value = item.D4_Codigo_Celda;
                ws.Cells[index, 44].Value = item.D4_Ubicacion_Celda;
                ws.Cells[index, 45].Value = item.D4_Tension;
                if (!string.IsNullOrEmpty(item.D4_Tension_Coment)) ws.Cells[index, 45].AddComment(item.D4_Tension_Coment, nombreUsuario);
                ws.Cells[index, 46].Value = item.D4_Capacidad_Onan_Mva;
                if (!string.IsNullOrEmpty(item.D4_Capacidad_Onan_Mva_Coment)) ws.Cells[index, 46].AddComment(item.D4_Capacidad_Onan_Mva_Coment, nombreUsuario);
                ws.Cells[index, 47].Value = item.D4_Capacidad_Onaf_Mva;
                if (!string.IsNullOrEmpty(item.D4_Capacidad_Onaf_Mva_Coment)) ws.Cells[index, 47].AddComment(item.D4_Capacidad_Onaf_Mva_Coment, nombreUsuario);
                ws.Cells[index, 48].Value = item.D4_Capacidad_Mva;
                if (!string.IsNullOrEmpty(item.D4_Capacidad_Mva_Coment)) ws.Cells[index, 48].AddComment(item.D4_Capacidad_Mva_Coment, nombreUsuario);
                ws.Cells[index, 49].Value = item.D4_Capacidad_A;
                if (!string.IsNullOrEmpty(item.D4_Capacidad_A_Coment)) ws.Cells[index, 49].AddComment(item.D4_Capacidad_A_Coment, nombreUsuario);
                ws.Cells[index, 50].Value = item.D4_Posicion_Nucleo_Tc;
                ws.Cells[index, 51].Value = item.D4_Pick_Up;
                ws.Cells[index, 52].Value = item.D4_Factor_Limitante_Calc;
                if (!string.IsNullOrEmpty(item.D4_Factor_Limitante_Calc_Coment)) ws.Cells[index, 52].AddComment(item.D4_Factor_Limitante_Calc_Coment, nombreUsuario);
                ws.Cells[index, 53].Value = item.D4_Factor_Limitante_Final;
                if (!string.IsNullOrEmpty(item.D4_Factor_Limitante_Final_Coment)) ws.Cells[index, 53].AddComment(item.D4_Factor_Limitante_Final_Coment, nombreUsuario);

                ws.Cells[index, 54].Value = item.Observaciones;
                ws.Cells[index, 55].Value = item.Motivo;
                ws.Cells[index, 56].Value = item.Usuario_Auditoria;
                ws.Cells[index, 57].Value = item.Fecha_Modificacion;

                index++;
            }

        }


        #endregion
    }
}
