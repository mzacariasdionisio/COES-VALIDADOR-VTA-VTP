using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using COES.MVC.Intranet.Areas.Formulas.Helper;
using COES.MVC.Intranet.Areas.Formulas.Models;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Scada;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.SeguridadServicio;

namespace COES.MVC.Intranet.Areas.Formulas.Controllers
{
    public class FormulaController : Controller
    {
        /// <summary>
        /// Clase para el acceso a datos
        /// </summary>
        PerfilScadaServicio servicio = new PerfilScadaServicio();

        /// <summary>
        /// Contiene las formulas que han sido seleccionadas
        /// </summary>
        public List<MePerfilRuleDTO> ListaFormulas
        {
            get
            {
                return (Session[DatosSesion.ListaFormulas] != null) ?
                    (List<MePerfilRuleDTO>)Session[DatosSesion.ListaFormulas] : new List<MePerfilRuleDTO>();
            }
            set { Session[DatosSesion.ListaFormulas] = value; }
        }        

        /// <summary>
        /// Lista que maneja los datos procesados
        /// </summary>
        public List<ScadaDTO> ListaProceso
        {
            get
            {
                return (Session[DatosSesion.ListaScada] != null) ?
                    (List<ScadaDTO>)Session[DatosSesion.ListaScada] : new List<ScadaDTO>();
            }
            set { Session[DatosSesion.ListaScada] = value; }
        }

        /// <summary>
        /// Obtjeto con los datos del último perfil desarrollado
        /// </summary>
        public PerfilScadaDTO UltimoPerfil
        {
            get
            {
                return (Session[DatosSesion.UltimoPerfil] != null) ?
                    (PerfilScadaDTO)Session[DatosSesion.UltimoPerfil] : new PerfilScadaDTO();
            }
            set { Session[DatosSesion.UltimoPerfil] = value; }
        }

        /// <summary>
        /// Lista con el total de puntos
        /// </summary>
        public List<ScadaDTO> ListaTotal
        {
            get
            {
                return (Session[DatosSesion.ListaTotal] != null) ?
                    (List<ScadaDTO>)Session[DatosSesion.ListaTotal] : new List<ScadaDTO>();
            }
            set { Session[DatosSesion.ListaTotal] = value; }
        }

        /// <summary>
        /// Entidad que contiene los datos promediados
        /// </summary>
        public ScadaDTO Entidad
        {
            get
            {
                return (Session[DatosSesion.EntidadScada] != null) ?
                    (ScadaDTO)Session[DatosSesion.EntidadScada] : new ScadaDTO();
            }
            set { Session[DatosSesion.EntidadScada] = value; }
        }

        /// <summary>
        /// Lista de objetos importados
        /// </summary>
        public List<ScadaDTO> ListaImportado
        {
            get 
            {
                return (Session[DatosSesion.DatosImportados] != null) ?
                    (List<ScadaDTO>)Session[DatosSesion.DatosImportados] : new List<ScadaDTO>();
            }
            set { Session[DatosSesion.DatosImportados] = value; }
        }

        /// <summary>
        /// Lista de objetos grabados
        /// </summary>
        public List<ScadaDTO> ListaGrabado
        {
            get
            {
                return (Session[DatosSesion.ListaGrabado] != null) ?
                    (List<ScadaDTO>)Session[DatosSesion.ListaGrabado] : new List<ScadaDTO>();
            }
            set { Session[DatosSesion.ListaGrabado] = value; }
        }

        /// <summary>
        /// Almacena los datos en banda superior
        /// </summary>
        public PerfilScadaDetDTO GraficoMaximo
        {
            get
            {
                return (Session[DatosSesion.ListaGraficoMaximo] != null) ?
                    (PerfilScadaDetDTO)Session[DatosSesion.ListaGraficoMaximo] : new PerfilScadaDetDTO();
            }
            set { Session[DatosSesion.ListaGraficoMaximo] = value; }
        }

        /// <summary>
        /// Almacena los datos de la banda inferior
        /// </summary>
        public PerfilScadaDetDTO GraficoMinimo
        {
            get
            {
                return (Session[DatosSesion.ListaGraficoMinimo] != null) ?
                    (PerfilScadaDetDTO)Session[DatosSesion.ListaGraficoMinimo] : new PerfilScadaDetDTO();
            }
            set { Session[DatosSesion.ListaGraficoMinimo] = value; }
        }


        /// <summary>
        /// Nombre de la plantilla a exportar
        /// </summary>
        public string NombreFormatoImportacion
        {
            get
            {
                return (Session[DatosSesion.NombrePlantillaImportacion] != null) ?
                    Session[DatosSesion.NombrePlantillaImportacion].ToString() : string.Empty;
            }
            set { Session[DatosSesion.NombrePlantillaImportacion] = value; }
        }

        /// <summary>
        /// Carga inicial del formulario
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            FormulaModel model = new FormulaModel();            
            model.FechaDesde = DateTime.Now.AddDays(-30).ToString(Constantes.FormatoFecha);
            model.FechaHasta = DateTime.Now.ToString(Constantes.FormatoFecha);

            return View(model);
        }

       
                
        /// <summary>
        /// Prrocesa los datos
        /// </summary>
        /// <param name="formulas"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="agrupacion"></param>
        /// <param name="fuente"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Calcular(string formulas, string fechaInicio, string fechaFin, int agrupacion, string fuente)
        {
            try
            {

                List<MePerfilRuleDTO> listaFormulas = this.ListaFormulas.Where(x => x.Prrucodi == int.Parse(formulas)).ToList();
                DateTime inicio = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fin = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                this.ListaTotal = this.servicio.ObtenerDatosProceso(listaFormulas, inicio, fin, fuente);

                PerfilScadaDTO perfil = this.servicio.ObtenerUltimoPerfil(int.Parse(formulas), agrupacion);
                this.UltimoPerfil = perfil;

                this.GraficoMaximo = null;
                this.GraficoMinimo = null;
                this.ListaImportado = null;

                this.Agrupacion();

                if (perfil != null)
                {
                    return Json(2);
                }
                {
                    return Json(1);
                }
            }
            catch
            {
                return Json(0);
            }
        }

        /// <summary>
        /// Permite mostrar las formulas por fuente
        /// </summary>
        /// <param name="fuente"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Formula(string areaOperativa, string indUsuario)
        {
            FormulaModel model = new FormulaModel();

            int areaCode = -2;
            string username = string.Empty;

            if (Session[DatosSesion.SesionUsuario] != null)
            {
                UserDTO usuario = (UserDTO)Session[DatosSesion.SesionUsuario];

                if (usuario.AreaCode != null)
                {
                    areaCode = (int)usuario.AreaCode;
                }

                if (indUsuario == 1.ToString()) 
                {
                    username = usuario.UserLogin;
                }
            }

            if (string.IsNullOrEmpty(areaOperativa)) areaOperativa = (-1).ToString();


            if (!string.IsNullOrEmpty(username))
            {
                List<MePerfilRuleDTO> formulas = this.servicio.GetByCriteriaMePerfilRules(areaCode, areaOperativa).
                    Where(x => ((!string.IsNullOrEmpty(x.Prrufirstuser)) ? x.Prrufirstuser.ToLower() : string.Empty) == username.ToLower()).ToList();
                model.ListaFormulas = formulas;
                this.ListaFormulas = formulas;
            }
            else
            {
                List<MePerfilRuleDTO> formulas = this.servicio.GetByCriteriaMePerfilRules(areaCode, areaOperativa);
                model.ListaFormulas = formulas;
                this.ListaFormulas = formulas;
            }
            

            return PartialView(model);
        }

        /// <summary>
        /// Permite mostrar los datos procesados
        /// </summary>
        /// <param name="agrupacion"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Datos(int agrupacion)
        {
            FormulaModel model = new FormulaModel();
            List<ScadaDTO> lista = this.ListaTotal.Where(x => x.CLASIFICACION == agrupacion).ToList();

            int indice = 0;
            foreach (ScadaDTO item in lista)
            {
                item.INDICE = indice;
                indice++;
            }

            model.ListaScada = lista;
            this.ListaProceso = lista;
            this.Entidad = new ScadaDTO();
            return View(model);
        }

        /// <summary>
        /// Generar el json para generar el gráfico
        /// </summary>
        /// <param name="agrupacion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Grafico(int agrupacion, int formula)
        {            
            List<ScadaDTO> items = this.ListaProceso.Where(x => x.INDICADOR == Constantes.SI).ToList();
            ScadaDTO entidad = this.Entidad;

            StringBuilder str = new StringBuilder();
            StringBuilder strColor = new StringBuilder();
        
            str.Append(Constantes.AperturaSerie);
            strColor.Append(Constantes.AperturaSerie); 

            int index = 0;
            string txt = 0.ToString();

            #region Datos del grafico

            foreach (ScadaDTO item in items)
            {
                str.Append(Constantes.AperturaSerie);

                for (int i = 1; i <= 48; i++)
                {                    
                    object valor = item.GetType().GetProperty("H" + (i * 2).ToString()).GetValue(item, null);
                    if(valor!=null)
                        txt = Convert.ToDecimal(valor).ToString();                    
                    else txt = 0.ToString();
                    str.Append(txt);                                      
                    str.Append((i < 48) ? "," : "");
                }

                str.Append(Constantes.CierreSerie);
                str.Append(",");
                strColor.Append("\"" + Tools.GetColor(item.INDICE) + "\",");

                index++;
            }

            str.Append(Constantes.AperturaSerie);

            for (int i = 1; i <= 48; i++)
            {
                object valor = entidad.GetType().GetProperty("H" + (i * 2).ToString()).GetValue(entidad, null);
                object ajuste = entidad.GetType().GetProperty("TH" + (i * 2).ToString()).GetValue(entidad, null);
                decimal suma = 0;

                if (valor != null)
                {
                    suma = suma + Convert.ToDecimal(valor);
                    if (ajuste != null)
                    {
                        suma = suma + Convert.ToDecimal(ajuste);
                    }
                    txt = suma.ToString();
                }
                else txt = 0.ToString();
                str.Append(txt);
                str.Append((i < 48) ? "," : "");
            }

            strColor.Append("\"#000000\"");
            str.Append(Constantes.CierreSerie);

            #endregion

            #region Graficos del último perfil

            if (this.UltimoPerfil != null)
            {
                if (this.UltimoPerfil.LISTAITEMS != null)
                {
                    PerfilScadaDetDTO detalle = this.UltimoPerfil.LISTAITEMS.Where(x => x.PERFCLASI == agrupacion).FirstOrDefault();
                    if (detalle != null)
                    {
                        str.Append(",");
                        str.Append(Constantes.AperturaSerie);

                        for (int i = 1; i <= 48; i++)
                        {
                            object valor = detalle.GetType().GetProperty("H" + (i).ToString()).GetValue(detalle, null);
                            object ajuste = detalle.GetType().GetProperty("TH" + (i).ToString()).GetValue(detalle, null);
                            decimal suma = 0;

                            if (valor != null)
                            {
                                suma = suma + Convert.ToDecimal(valor);
                                if (ajuste != null)
                                {
                                    suma = suma + Convert.ToDecimal(ajuste);
                                }
                                txt = suma.ToString();
                            }
                            else txt = 0.ToString();
                            str.Append(txt);
                            str.Append((i < 48) ? "," : "");
                        }

                        str.Append(Constantes.CierreSerie);
                        strColor.Append(",");
                        strColor.Append("\"#FF0000\"");
                    }


                    if (this.GraficoMaximo != null)
                    {
                        PerfilScadaDetDTO maximo = this.GraficoMaximo;

                        if (maximo.PERFCLASI == agrupacion)
                        {
                            str.Append(",");
                            str.Append(Constantes.AperturaSerie);

                            for (int i = 1; i <= 48; i++)
                            {
                                object valor = maximo.GetType().GetProperty("H" + (i).ToString()).GetValue(maximo, null);                               
                                decimal suma = 0;
                                if (valor != null)
                                {
                                    suma = suma + Convert.ToDecimal(valor);                                    
                                    txt = suma.ToString();
                                }
                                else txt = 0.ToString();
                                str.Append(txt);
                                str.Append((i < 48) ? "," : "");
                            }

                            str.Append(Constantes.CierreSerie);
                            strColor.Append(",");
                            strColor.Append("\"#FF0000\"");
                        }
                    }

                    if (this.GraficoMinimo != null)
                    {
                        PerfilScadaDetDTO minimo = this.GraficoMinimo;

                        if (minimo.PERFCLASI == agrupacion)
                        {
                            str.Append(",");
                            str.Append(Constantes.AperturaSerie);

                            for (int i = 1; i <= 48; i++)
                            {
                                object valor = minimo.GetType().GetProperty("H" + (i).ToString()).GetValue(minimo, null);
                                decimal suma = 0;
                                if (valor != null)
                                {
                                    suma = suma + Convert.ToDecimal(valor);
                                    txt = suma.ToString();
                                }
                                else txt = 0.ToString();
                                str.Append(txt);
                                str.Append((i < 48) ? "," : "");
                            }

                            str.Append(Constantes.CierreSerie);
                            strColor.Append(",");
                            strColor.Append("\"#FF0000\"");
                        }
                    }

                }
            }

            #endregion

            #region Importado

            if (this.ListaImportado != null)
            {
                ScadaDTO importado = this.ListaImportado.Where(x => x.CLASIFICACION == agrupacion && x.PRRUCODI == formula).FirstOrDefault();

                if (importado != null)
                {
                    str.Append(",");
                    str.Append(Constantes.AperturaSerie);

                    for (int i = 1; i <= 48; i++)
                    {
                        object valor = importado.GetType().GetProperty("H" + (i * 2).ToString()).GetValue(importado, null);
                        object ajuste = importado.GetType().GetProperty("TH" + (i * 2).ToString()).GetValue(importado, null);
                        decimal suma = 0;

                        if (valor != null)
                        {
                            suma = suma + Convert.ToDecimal(valor);
                            if (ajuste != null)
                            {
                                suma = suma + Convert.ToDecimal(ajuste);
                            }
                            txt = suma.ToString();
                        }
                        else txt = 0.ToString();
                        str.Append(txt);
                        str.Append((i < 48) ? "," : "");
                    }

                    str.Append(Constantes.CierreSerie);
                    strColor.Append(",");
                    strColor.Append("\"#23B14D\"");
                }
            }

            #endregion

            strColor.Append(Constantes.CierreSerie);    

            StringBuilder categoria = new StringBuilder();
            categoria.Append(Constantes.AperturaSerie);
            for (int i = 1; i <= 48; i++)
            {
                categoria.Append("\"" + Tools.ObtenerHoraMedicion(i) + "\"" + ((i < 48) ? "," : ""));
            }
            categoria.Append(Constantes.CierreSerie);

            str.Append(Constantes.CierreSerie);

            Ploteo ploteo = new Ploteo();
            ploteo.Series = str.ToString();
            ploteo.Categoria = categoria.ToString();
            ploteo.Colores = strColor.ToString();
            ploteo.Cantidad = items.Count;

            return Json(ploteo);
        }

        /// <summary>
        /// Valores del promedio
        /// </summary>
        /// <param name="agrupacion"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Valores(int agrupacion)
        {
            FormulaModel model = new FormulaModel();                        
            model.Entidad = this.ObtenerPromedio(agrupacion);            
            return View(model);
        }

        /// <summary>
        /// Permite agregar o quitar un item
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="indicador"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddRemoveItem(string fecha, string indicador)
        {
            try
            {
                ScadaDTO item = this.ListaProceso.Where(x => x.MEDIFECHA.ToString(Constantes.FormatoFecha) == fecha).FirstOrDefault();
                if (item != null)
                {
                    if (indicador == Constantes.NO)
                        item.INDICADOR = Constantes.NO;
                    else
                        item.INDICADOR = Constantes.SI;
                }
                return Json(1);
            }
            catch
            {
                return Json(0);
            }
        }

        /// <summary>
        /// Permite obtener el promedio de datos
        /// </summary>
        /// <param name="agrupacion"></param>
        /// <returns></returns>
        private ScadaDTO ObtenerPromedio(int agrupacion)
        {
            List<ScadaDTO> items = this.ListaProceso.Where(x => x.INDICADOR == Constantes.SI).ToList();
            ScadaDTO entity = this.Entidad;
                        
            bool flag = false;
            PerfilScadaDetDTO minimo = this.GraficoMinimo;
            PerfilScadaDetDTO maximo = this.GraficoMaximo;
            if (this.UltimoPerfil != null)
            {
                if (this.UltimoPerfil.LISTAITEMS != null)
                {
                    if (this.UltimoPerfil.PERFCLASI == agrupacion)
                    {
                        if (minimo != null && maximo != null)
                        {
                            if (maximo.PERFCLASI == agrupacion && minimo.PERFCLASI == agrupacion)
                            {
                                flag = true;
                            }
                        }
                    }
                }
            }

            List<decimal> valores = new List<decimal>();

            for (int i = 1; i <= 48; i++)
            {
                decimal suma = 0;
                int contador = 0;

                foreach (ScadaDTO item in items)
                {
                    object objeto = item.GetType().GetProperty("H" + (i * 2).ToString()).GetValue(item, null);
                    decimal valor = 0;
                    if (objeto != null)
                    {
                        valor = Convert.ToDecimal(objeto);                        
                    }

                    if (flag)
                    {
                        decimal valorMaximo = (decimal)maximo.GetType().GetProperty("H" + i).GetValue(maximo, null);
                        decimal valorMinimo = (decimal)minimo.GetType().GetProperty("H" + i).GetValue(minimo, null);

                        if (valor >= valorMinimo && valor <= valorMaximo)
                        {
                            suma = suma + valor;
                            contador = contador + 1;
                        }
                    }
                    else
                    {
                        suma = suma + valor;
                        contador = contador + 1;
                    }
                }

                if (contador != 0)
                {
                    entity.GetType().GetProperty("H" + (i * 2).ToString()).SetValue(entity, suma / contador);
                    valores.Add(suma / contador);
                }
                else 
                {
                    decimal? cero = 0;
                    entity.GetType().GetProperty("H" + (i * 2).ToString()).SetValue(entity, cero);
                    //valores.Add(0);
                }
            }

            entity.MEDIANA = Tools.GetMediana(valores);
            entity.MAXIMA = Tools.GetMaximo(valores);
            entity.MINIMA = Tools.GetMinimo(valores);

            this.Entidad = entity;
            this.ActualizarListado(entity, agrupacion);
            return entity;        
        }        

        /// <summary>
        /// Permite hacer modificaciones sobre el promedio
        /// </summary>
        /// <param name="indicador"></param>
        /// <param name="indice"></param>
        /// <param name="valor"></param>
        /// <param name="agrupacion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AplicarTunnig(string indicador, int indice, decimal valor, int agrupacion)
        {
            try
            {
                List<int> horas = new List<int>();
                //  2     4     6     8    10     12    ...94    96
                //  1     2     3     4    5      6        47    48
                //00:30 01:00 01:30 02:30 03:00 03:30     23:30  00:00

                if (indicador == Constantes.SI)
                {
                    if (indice == 1)
                    {
                        for (int i = 1; i < 16; i++)
                        {
                            horas.Add(i);
                        }
                        for (int i = 47; i <= 48; i++)
                        {
                            horas.Add(i);
                        }                        
                    }
                    if (indice == 2)
                    {                        
                        for (int i = 16; i < 36; i++)
                        {
                            horas.Add(i);
                        }
                    }
                    if (indice == 3)
                    {                        
                        for (int i = 36; i <= 46; i++)
                        {
                            horas.Add(i);
                        }
                    }
                }
                else
                {
                    horas.Add(indice);
                }

                ScadaDTO entidad = this.Entidad;

                for (int k = 0; k < horas.Count; k++)
                {
                    entidad.GetType().GetProperty("TH" + (horas[k] * 2).ToString()).SetValue(entidad, valor);
                }

                this.Entidad = entidad;
                this.ActualizarListado(entidad, agrupacion);

                return Json(1);
            }
            catch
            {
                return Json(0);
            }
        }
               
        /// <summary>
        /// Realiza la agrupación de datos
        /// </summary>
        public void Agrupacion()
        {           
            List<ScadaDTO> lista = this.ListaTotal;
            this.ListaGrabado = null;
            List<ScadaDTO> resultado = new List<ScadaDTO>();

            for (int i = 1; i <= 4; i++)
            {
                List<ScadaDTO> items = lista.Where(x => x.CLASIFICACION == i).ToList();

                ScadaDTO entity = new ScadaDTO();
                int contador = items.Count;
               
                for (int j = 1; j <= 48; j++)
                {
                    decimal valor = 0;

                    foreach (ScadaDTO item in items)
                    {
                        object objeto = item.GetType().GetProperty("H" + (j * 2).ToString()).GetValue(item, null);
                        if (objeto != null)
                        {
                            valor = valor + Convert.ToDecimal(objeto);
                        }
                    }

                    if (contador != 0)
                    {
                        entity.GetType().GetProperty("H" + (j * 2).ToString()).SetValue(entity, valor / contador);                      
                    }
                }
                entity.CLASIFICACION = i;

                resultado.Add(entity);
            }

            this.ListaGrabado = resultado;
        }

        /// <summary>
        /// Permite actualizar los valore de la lista
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="agrupacion"></param>
        public void ActualizarListado(ScadaDTO entity, int agrupacion)
        {
            List<ScadaDTO> lista = this.ListaGrabado;

            ScadaDTO item = lista.Where(x => x.CLASIFICACION == agrupacion).First();

            if (item != null)
            {
                object valor = null, tvalor = null;
                for (int i = 2; i <= 96; i += 2)
                {
                    valor = entity.GetType().GetProperty("H" + i).GetValue(entity, null);
                    tvalor = entity.GetType().GetProperty("TH" + i).GetValue(entity, null);

                    if (valor != null)
                        item.GetType().GetProperty("H" + i).SetValue(item, valor);

                    if (tvalor != null)
                        item.GetType().GetProperty("TH" + i).SetValue(item, tvalor);
                }
            }

            this.ListaGrabado = lista;
        }

        /// <summary>
        /// Permite grabar los datos del perfil generado
        /// </summary>
        /// <param name="comentario"></param>
        /// <param name="fecInicio"></param>
        /// <param name="fecFin"></param>
        /// <param name="idFormula"></param>
        /// <param name="agrupacion"></param>
        /// <param name="fuente"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarPerfil(string comentario, string fecInicio, string fecFin, int idFormula, int agrupacion, string fuente)
        {
            try
            {
                PerfilScadaDTO entity = new PerfilScadaDTO();

                entity.FECINICIO = DateTime.ParseExact(fecInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                entity.FECFIN = DateTime.ParseExact(fecFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                entity.LASTUSER = User.Identity.Name;
                entity.PERFDESC = comentario;
                entity.LASTDATE = DateTime.Now;
                entity.FECREGISTRO = DateTime.Now;
                entity.EJRUCODI = idFormula;
                entity.PERFCLASI = agrupacion;
                entity.PERFORIG = fuente;

                List<ScadaDTO> lista = this.ListaGrabado.Where(x => x.CLASIFICACION == agrupacion).ToList();

                if (lista.Count > 0)
                {
                    entity.LISTADETALLE = lista;

                    int id = this.servicio.GrabarPerfilScada(entity);

                    return Json(id);
                }
                else
                {
                    Json(-1);
                }
                
                return Json(-2);
            }
            catch
            {
                return Json(0);
            }
        }

        /// <summary>
        /// Permite mostrar el detalle del perfil grabado
        /// </summary>
        /// <returns></returns>
        public ActionResult Detalle()
        {
            FormulaModel model = new FormulaModel();

            int id = int.Parse(Request[RequestParameter.PerfilId]);
            PerfilScadaDTO entity = this.servicio.ObtenerPerfil(id);

            model.PerfilScada = entity;
            model.IdPerfil = id;

            return View(model);
        }

        /// <summary>
        /// Permite listar los perfiles generados
        /// </summary>
        /// <returns></returns>
        public ActionResult Lista()
        {
            FormulaModel model = new FormulaModel();
            model.FechaDesde = DateTime.Now.AddDays(-1 * DateTime.Now.Day).ToString(Constantes.FormatoFecha);
            model.FechaHasta = DateTime.Now.AddDays(1).ToString(Constantes.FormatoFecha);

            return View(model);
        }

        /// <summary>
        /// Permite mostrar la grilla de perfiles
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="fuente"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Grid(string fechaInicio, string fechaFin)
        {
            FormulaModel model = new FormulaModel();

            DateTime inicio = DateTime.Now.AddDays(-1 * DateTime.Now.Day);
            DateTime fin = DateTime.Now.AddDays(1);

            if (!string.IsNullOrEmpty(fechaInicio))
            {
                inicio = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            if (!string.IsNullOrEmpty(fechaFin))
            {
                fin = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            model.Listado = this.servicio.ListarPerfilesPorUsuario(User.Identity.Name, inicio, fin);          

            return View(model);
        }

        /// <summary>
        /// Permite generar el archivo de reporte
        /// </summary>
        /// <param name="fuente"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarArchivo(string fechaInicio, string fechaFin)
        {
            int result = 1;
            try
            {
                DateTime inicio = DateTime.Now.AddDays(-1 * DateTime.Now.Day);
                DateTime fin = DateTime.Now.AddDays(1);

                if (!string.IsNullOrEmpty(fechaInicio))
                {
                    inicio = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }
                if (!string.IsNullOrEmpty(fechaFin))
                {
                    fin = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }
                int areaCode = (int)((COES.MVC.Intranet.SeguridadServicio.UserDTO)Session[DatosSesion.SesionUsuario]).AreaCode;
                List<PerfilScadaDTO> list = this.servicio.ObtenerPerfilesExportacion(User.Identity.Name, inicio, fin);
                ExcelDocument.GenerarExcelPerfilScada(list);                
                result = 1;
            }
            catch
            {
                result = -1;
            }

            return Json(result);
        }

        /// <summary>
        /// Permite abrir el archivo generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult Exportar()
        {
            string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.ReportePerfiles] + Constantes.NombreReportePerfilScadaExcel;
            return File(fullPath, Constantes.AppExcel, Constantes.NombreReportePerfilScadaExcel);
        }

        /// <summary>
        /// Permite generar el formato de importacion
        /// </summary>
        /// <param name="fuente"></param>
        /// <param name="subEstacion"></param>
        /// <param name="area"></param>
        /// <param name="agrupacion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarFormato(string subEstacion, string area, string agrupacion, int formula)
        {
            int result = 1;
            try
            {
                ExcelDocument.GenerarPlantillaImportacion(subEstacion, area, agrupacion, formula);
                this.NombreFormatoImportacion = subEstacion + "-" + area + "-" + agrupacion + ".xlsx";
                result = 1;
            }
            catch
            {
                result = -1;
            }

            return Json(result);
        }

        /// <summary>
        /// Permite descargar el formato de importacion
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarFormato()
        {
            string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.ReportePerfiles] + Constantes.NombreFormatoImportacionPerfilExcel;
            return File(fullPath, Constantes.AppExcel, this.NombreFormatoImportacion);
        }

        /// <summary>
        /// Permite cargar el archivo de potencia
        /// </summary>
        /// <returns></returns>
        public ActionResult Upload()
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga;

                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    string fileName = path + Constantes.ArchivoImportacionPerfiles;

                    if (System.IO.File.Exists(fileName))
                    {
                        System.IO.File.Delete(fileName);
                    }

                    file.SaveAs(fileName);
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Permite realizar la importación
        /// </summary>
        /// <param name="clasificacion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Importar(int clasificacion, int formula)
        {
            try
            {
                int idFormula = 0;
                string path = AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga + Constantes.ArchivoImportacionPerfiles;
                ScadaDTO importado = ExcelDocument.ImportarDatos(path, out idFormula);

                if (idFormula == formula)
                {
                    importado.CLASIFICACION = clasificacion;
                    importado.PRRUCODI = formula;

                    List<ScadaDTO> list = this.ListaImportado;
                    ScadaDTO entity = list.Where(x => x.CLASIFICACION == clasificacion && x.PRRUCODI == formula).FirstOrDefault();

                    if (entity != null)
                    {
                        list.Remove(entity);
                    }

                    list.Add(importado);

                    this.ListaImportado = list;
                    return Json(1);
                }
                else 
                {
                    return Json(0);
                }
               
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Quita los datos importados
        /// </summary>
        /// <param name="clasificacion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult QuitarImportado(int clasificacion, int formula)
        {
            try
            {
                List<ScadaDTO> list = this.ListaImportado;
                ScadaDTO entity = list.Where(x => x.CLASIFICACION == clasificacion && x.PRRUCODI == formula).FirstOrDefault();

                if (entity != null)
                {
                    list.Remove(entity);
                    this.ListaImportado = list;
                    return Json(1);
                }
                else
                {
                    return Json(0);
                }                
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite graficar la banda
        /// </summary>
        /// <param name="clasificacion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AjustarBanda(int clasificacion, decimal porcentaje)
        {
            try
            {
                if (this.UltimoPerfil != null)
                {
                    if (this.UltimoPerfil.LISTAITEMS != null)
                    {
                        if (this.UltimoPerfil.PERFCLASI == clasificacion)
                        {
                            PerfilScadaDTO entity = this.UltimoPerfil;
                            PerfilScadaDetDTO itemMaximo = new PerfilScadaDetDTO();
                            PerfilScadaDetDTO itemMinimo = new PerfilScadaDetDTO();

                            foreach (PerfilScadaDetDTO item in entity.LISTAITEMS)
                            {
                                for (int i = 1; i <= 48; i++)
                                {
                                    decimal valor = (decimal)item.GetType().GetProperty("H" + i).GetValue(item);
                                    object th = item.GetType().GetProperty("TH" + i).GetValue(item);
                                    valor = (th != null) ? valor + (decimal)th : valor;

                                    itemMaximo.GetType().GetProperty("H" + i).SetValue(itemMaximo, valor * (1 + porcentaje / 100));
                                    itemMinimo.GetType().GetProperty("H" + i).SetValue(itemMinimo, valor * (1 - porcentaje / 100));
                                }

                                itemMinimo.PERFCLASI = clasificacion;
                                itemMaximo.PERFCLASI = clasificacion;

                                break;
                            }

                            this.GraficoMaximo = itemMaximo;
                            this.GraficoMinimo = itemMinimo;

                            return Json(1);
                        }
                    }
                }

                return Json(0);
            }
            catch
            {
                return Json(-1);
            }
        }
    }

    /// <summary>
    /// Clase que permite manejar los datos del gráfico
    /// </summary>
    public class Ploteo
    {
        public string Series { get; set; }
        public string Categoria { get; set; }
        public string Colores { get; set; }
        public int Cantidad { get; set; }
    }
}
