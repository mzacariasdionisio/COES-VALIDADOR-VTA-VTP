using COES.Base.Tools;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.CortoPlazo.Helper;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.CortoPlazo
{
    public class ObtenerPropiedades
    {
        public void Procesar()
        {

            string textoAll = string.Empty;
            List<PrGrupoDTO> listGrupo = FactorySic.GetPrGrupoRepository().ListaModosOperacionActivos();



            foreach (PrGrupoDTO item in listGrupo)
            {
                #region Obteniendo las propiedades de las térmicas

                PrGrupoDTO padre = FactorySic.GetPrGrupoRepository().GetById((int)item.Grupopadre);
                string idsGrupos = item.Grupocodi + ConstantesAppServicio.CaracterComa.ToString() + item.Grupopadre +
                                   ConstantesAppServicio.CaracterComa.ToString() + padre.Grupopadre;

                List<PrGrupodatDTO> listFormulas = new List<PrGrupodatDTO>();
                listFormulas.AddRange(FactorySic.GetPrGrupodatRepository().ObtenerParametroGeneral(DateTime.Now));
                listFormulas.AddRange(FactorySic.GetPrGrupodatRepository().ObtenerParametroModoOperacion(idsGrupos, DateTime.Now));

                // Declaramos el objeto parameter para calcular los valores
                n_parameter parameter = new n_parameter();
                foreach (PrGrupodatDTO itemConcepto in listFormulas)
                {
                    if (!string.IsNullOrEmpty(itemConcepto.Concepabrev) && !string.IsNullOrEmpty(itemConcepto.Formuladat))
                        parameter.SetData(itemConcepto.Concepabrev, itemConcepto.Formuladat);
                }

                // Con esto sacamos las propiedades para el modo de operación como son curva de consumo, costo combustible, CV OYM
                double costoCombustible = parameter.GetEvaluate(ConstantesCortoPlazo.PropCostoCombustible);
                double costoVariableOM = parameter.GetEvaluate(ConstantesCortoPlazo.PropCostoVariable);
                double potMaxima = parameter.GetEvaluate(ConstantesCortoPlazo.PropPotenciaMaxima); //- Pe
                double potMinima = parameter.GetEvaluate(ConstantesCortoPlazo.PropPotenciaMinima);  //- Pmin
                double factorConversion = parameter.GetEvaluate(ConstantesCortoPlazo.PropFactorConversion); //PCI_SI

                string formulacostoCombustible = listFormulas.Where(x => x.Concepabrev == ConstantesCortoPlazo.PropCostoCombustible).FirstOrDefault().Formuladat;
                string formulacostoCombustibleUni = listFormulas.Where(x => x.Concepabrev == ConstantesCortoPlazo.PropCostoCombustible).FirstOrDefault().ConcepUni;
                string formulacostoVariableOM = listFormulas.Where(x => x.Concepabrev == ConstantesCortoPlazo.PropCostoVariable).FirstOrDefault().Formuladat;
                string formulacostoVariableOMUni = listFormulas.Where(x => x.Concepabrev == ConstantesCortoPlazo.PropCostoVariable).FirstOrDefault().ConcepUni;

                string factorConversionFormula = (listFormulas.Where(x => x.Concepabrev == ConstantesCortoPlazo.PropFactorConversion).Count() > 0) ? listFormulas.Where(x => x.Concepabrev == ConstantesCortoPlazo.PropFactorConversion).FirstOrDefault().Formuladat : string.Empty;
                string factorConversionFormulaUni = (listFormulas.Where(x => x.Concepabrev == ConstantesCortoPlazo.PropFactorConversion).Count() > 0) ? listFormulas.Where(x => x.Concepabrev == ConstantesCortoPlazo.PropFactorConversion).FirstOrDefault().ConcepUni : string.Empty;

                string formulapotMaxima = listFormulas.Where(x => x.Concepabrev == ConstantesCortoPlazo.PropPotenciaMaxima).FirstOrDefault().Formuladat;
                string formulapotMinima = listFormulas.Where(x => x.Concepabrev == ConstantesCortoPlazo.PropPotenciaMinima).FirstOrDefault().Formuladat;

                List<CoordenadaConsumo> curvaEnsayoList = new List<CoordenadaConsumo>();
                List<CoordenadaConsumo> curvaSPRList = new List<CoordenadaConsumo>();
                //string tipoCurva = listFormulas.Where(x => x.Concepabrev == ConstantesCortoPlazo.PropIndicadorCurva).FirstOrDefault().Formuladat; //- CMGNTC

                //if (tipoCurva == 0.ToString())
                //{
                string curvaEnsayo = listFormulas.Where(x => x.Concepabrev == ConstantesCortoPlazo.PropCurvaAjustadaSPR).FirstOrDefault().Formuladat; //- CoordConsumComb

                string[] strPuntos = curvaEnsayo.Split(ConstantesAppServicio.CaracterNumeral);

                foreach (string strPunto in strPuntos)
                {
                    string[] strCorrdenada = strPunto.Split(ConstantesAppServicio.CaracterPorcentaje);

                    if (strCorrdenada.Length == 2)
                    {
                        decimal x = 0;
                        decimal y = 0;

                        if (decimal.TryParse(strCorrdenada[0], out x) && decimal.TryParse(strCorrdenada[1], out y))
                        {
                            curvaEnsayoList.Add(new CoordenadaConsumo
                            {
                                Potencia = x,
                                Consumo = y
                            });
                        }
                    }
                }
                //}
                //else
                //{
                int[] categoriasCurva = { 14, 175, 176, 177, 178, 179, 180, 181, 182, 183 };
                List<PrGrupodatDTO> puntos = listFormulas.Where(x => categoriasCurva.Any(y => x.Concepcodi == y)).
                    Select(x => new PrGrupodatDTO { Concepcodi = x.Concepcodi, Formuladat = x.Formuladat }).ToList();

                curvaSPRList = UtilCortoPlazo.ObtenerCurvaConsumo(puntos);
                //}

                //- Obteniendo velocidad de toma y reduccion de carga
                //- double velocidadDescarga = parameter.GetEvaluate(ConstantesCortoPlazo.PropVelocidadDescarga);
                //- double velocidadCarga = parameter.GetEvaluate(ConstantesCortoPlazo.PropVelocidadCarga);
                //string velocidadToma = listFormulas.Where(x => x.Concepabrev == ConstantesCortoPlazo.PropVelocidadCarga).FirstOrDefault().Formuladat;
                //string velocidadReduccion = listFormulas.Where(x => x.Concepabrev == ConstantesCortoPlazo.PropVelocidadDescarga).FirstOrDefault().Formuladat;
                //bool existeVelocidaCarga = false;
                //bool existeVelocidaDescarga = false;
                //double velocidadCarga = this.ObtenerPropiedadVelocidad(velocidadToma, out existeVelocidaCarga);
                //double velocidadDescarga = this.ObtenerPropiedadVelocidad(velocidadReduccion, out existeVelocidaDescarga);

                //- Llenado propiedades

                //entity.CostoCombustible = costoCombustible;
                //entity.CostoVariableOYM = costoVariableOM;
                //entity.PotenciaMaxima = potMaxima;
                //entity.PotenciaMinima = potMinima;
                //entity.VelocidadCarga = velocidadCarga;
                //entity.VelocidadDescarga = velocidadDescarga;     
                //entity.ListaCurva = curva;

                string texto = item.Grupocodi + "," + item.Gruponomb + "," +
                    formulacostoCombustibleUni + "," +
                    formulacostoCombustible + "," + costoCombustible + "," +
                    formulacostoVariableOMUni + "," +
                    formulacostoVariableOM + "," + costoVariableOM + "," +
                    factorConversionFormulaUni + "," +
                    factorConversionFormula + "," + factorConversion + ",";


                string curensayo = "";
                foreach (CoordenadaConsumo coor in curvaEnsayoList)
                {
                    curensayo = curensayo + "(" + coor.Potencia + ";" + coor.Consumo + ");";
                }
                string curspr = "";
                foreach (CoordenadaConsumo coor in curvaSPRList)
                {
                    curspr = curspr + "(" + coor.Potencia + ";" + coor.Consumo + ");";
                }

                texto = texto + curensayo + "," + curspr;

                textoAll += texto + "\n";
                #endregion

            }

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"D:\Descarga\termicos.txt"))
            {
                file.Write(textoAll);
            }

            //#region Obteniendo las propiedades de las hidráulicas

            ////textoAll = string.Empty;

            //////- Obteniendo las propiedades necesarias de las unidades hidraulicas
            ////List<EqRelacionDTO> propHidraulicas = FactorySic.GetEqRelacionRepository().ObtenerPropiedadHidraulicos();
            //////- Obteniendo las propiedades necesarias de las unidades hidraulicas
            ////List<EqRelacionDTO> propHidraulicasCentral = FactorySic.GetEqRelacionRepository().ObtenerPropiedadHidraulicosCentral();
            ////List<EqEquipoDTO> genHidraulicos = FactorySic.GetEqEquipoRepository().ListarEquiposPorFamilia(2);

            ////foreach (EqEquipoDTO entity in genHidraulicos)
            ////{
            ////    //- Jalando los valores de las propiedades
            ////    EqRelacionDTO propPotMax = propHidraulicas.Where(x => x.Equicodi == entity.Equicodi && x.Propcodi == ConstantesCortoPlazo.IdPropPotMaxH).FirstOrDefault();
            ////    EqRelacionDTO propPotMin = propHidraulicas.Where(x => x.Equicodi == entity.Equicodi && x.Propcodi == ConstantesCortoPlazo.IdPropPotMinH).FirstOrDefault();
            ////    EqRelacionDTO propVelCarga = propHidraulicas.Where(x => x.Equicodi == entity.Equicodi && x.Propcodi == ConstantesCortoPlazo.IdPropVelCargaH).FirstOrDefault();
            ////    EqRelacionDTO propVelDescarga = propHidraulicas.Where(x => x.Equicodi == entity.Equicodi && x.Propcodi == ConstantesCortoPlazo.IdPropVelDescargaH).FirstOrDefault();
            ////    //EqRelacionDTO propVelDescarga = propHidraulicas.Where(x => x.Equicodi == entity.Equicodi && x.Propcodi == ConstantesCortoPlazo.IdPropVelDescargaH).FirstOrDefault();
                
            ////    //obtener capacidad de regulacion con equipadre
            ////    EqRelacionDTO propCapacRegulacion = propHidraulicasCentral.Where(x => x.Equicodi == entity.Equipadre && x.Propcodi == ConstantesCortoPlazo.IdPropCapacRegulacion).FirstOrDefault();
                
            ////    //- Definiendo las variables y indicadores de existencia de las propiedades evaluadas

            ////    bool flagPotMaxH = false;
            ////    bool flagPotMinH = false;
            ////    bool flagVelCargaH = false;
            ////    bool flagVelDescargaH = false;

            ////    double potMaxH = this.ObtenerPropiedadHidraulico(propPotMax, out flagPotMaxH);
            ////    double potMinH = this.ObtenerPropiedadHidraulico(propPotMin, out flagPotMinH);
            ////    double velCargaH = this.ObtenerPropiedadHidraulico(propVelCarga, out flagVelCargaH);
            ////    double velDescargaH = this.ObtenerPropiedadHidraulico(propVelDescarga, out flagVelDescargaH);

            ////    //- Llenando a la estructura los valores e indicadores cargados         
            ////    //entity.PotenciaMaxima = potMaxH;
            ////    //entity.PotenciaMinima = potMinH;
            ////    //entity.VelocidadCarga = velCargaH;
            ////    //entity.VelocidadDescarga = velDescargaH;

            ////    //// pasar valor de capacidad de regulacion en base el equipadre 
            ////    //if (propCapacRegulacion != null)
            ////    //    entity.CapacidadRegulacion = propCapacRegulacion.Propiedad;


            ////    string texto = entity.Equicodi + "," + entity.Equinomb + "," +
            ////       potMaxH + "," +
            ////       potMinH + "," +
            ////       velCargaH + "," +
            ////       velDescargaH + ",";

            ////    if (propCapacRegulacion != null)
            ////        texto += propCapacRegulacion.Propiedad;
            ////    else {
            ////        texto += "";
            ////    }

            ////    textoAll += texto + "\n";
            ////}


            ////using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"D:\Descarga\hidro.txt"))
            ////{
            ////    file.Write(textoAll);
            ////}
            //#endregion

        }

        /// <summary>
        /// Permite obtener el valor de una propiedad
        /// </summary>
        /// <returns></returns>
        private double ObtenerPropiedadVelocidad(string valor, out bool existePropiedad)
        {
            existePropiedad = false;
            double velocidad = 0;

            if (!string.IsNullOrEmpty(valor))
            {
                //- Verificamos que no existan los caracteres / o -

                if (valor.Contains('/'))
                {
                    string[] component = valor.Split('/');

                    if (component.Length > 0)
                    {
                        existePropiedad = true;

                        if (!double.TryParse(component[component.Length - 1], out velocidad))
                        {
                            existePropiedad = false;
                        }
                    }
                    else
                    {
                        existePropiedad = false;
                    }
                }
                else if (valor.Contains('-'))
                {
                    string[] component = valor.Split('-');

                    if (component.Length > 0)
                    {
                        existePropiedad = true;

                        if (!double.TryParse(component[component.Length - 1], out velocidad))
                        {
                            existePropiedad = false;
                        }
                    }
                    else
                    {
                        existePropiedad = false;
                    }
                }
                else
                {
                    existePropiedad = true;
                    if (!double.TryParse(valor, out velocidad))
                    {
                        existePropiedad = false;
                    }
                }
            }

            return velocidad;
        }

        /// <summary>
        /// Permite evaluar la formula de las unidades hidraulicas
        /// </summary>
        /// <param name="item"></param>
        /// <param name="existePropiedad"></param>
        /// <returns></returns>
        private double ObtenerPropiedadHidraulico(EqRelacionDTO item, out bool existePropiedad)
        {
            existePropiedad = false;
            double propiedad = 0;

            if (item != null)
            {
                if (!string.IsNullOrEmpty(item.Propiedad))
                {
                    if (double.TryParse(item.Propiedad, out propiedad))
                    {
                        existePropiedad = true;
                    }
                }
            }

            return propiedad;
        }
    }
}
