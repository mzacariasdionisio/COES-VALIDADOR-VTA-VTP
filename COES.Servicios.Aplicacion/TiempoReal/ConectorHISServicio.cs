using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using System.Linq;
//using log4net;
using COES.Dominio.DTO.Scada;
using System.Collections;

namespace COES.Servicios.Aplicacion.TiempoReal
{
    public class ConectorHISServicio : IDisposable
    {
        void IDisposable.Dispose() { }


        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        //private static readonly ILog Logger = LogManager.GetLogger(typeof(ConectorHISServicio));

        public ConectorHISServicio()
        {
            //log4net.Config.XmlConfigurator.Configure();
        }

        public void Dispose()
        {
            //GC.SuppressFinalize(this);
        }


        /// <summary>
        /// Obtención de datos de Siemens SP7 
        /// </summary>
        /// <param name="FechaInicio">Fecha Hora inicial</param>
        /// <param name="FechaFin">Fecha Hora final</param>
        /// <param name="Path">Path de consulta de datos. Ejemplo: '/StaRosNu/60      /C-L708  /I       /MvMoment'</param>
        /// <param name="TipoTabla">A: Tabla analógica, D: Tabla digital</param>
        /// <returns>Listado de datos de acuerdo a parámetros solicitados</returns>
        public List<DatosSP7DTO> ObtenerDatosSP7(DateTime FechaInicio, DateTime FechaFin, string Path, string TipoTabla)
        {
            string tabla = "AnalogsRaw";

            List<DatosSP7DTO> datosSP7 = new List<DatosSP7DTO>();

            switch (TipoTabla)
            {
                case "A":
                    tabla = "AnalogsRaw";
                    break;
                case "D":
                    tabla = "DigitalsRaw";
                    break;
                default:
                    return datosSP7;
            }


            DateTime from = FechaInicio; //.AddHours(5);//.ToUniversalTime();
            DateTime to = FechaFin; //.AddHours(5); //.ToUniversalTime();
            string consultaDisponibilidad = "";

            DateTime tiempoAperturaConexion = DateTime.Now;

            consultaDisponibilidad += ",Conexion," + DateTime.Now.Subtract(tiempoAperturaConexion).TotalSeconds + ",s.,";
            string RegistrosRecuperados = "";
            DateTime tiempoConsulta = DateTime.Now;

            try
            {
                datosSP7 = ListadoDatosSp7(tabla, from, to, Path);
                RegistrosRecuperados += ",Reg," + datosSP7.Count + ",Proceso,OK";
            }
            catch
            {
                RegistrosRecuperados += ",Reg," + datosSP7.Count + ",Proceso,ERROR";
            }

            consultaDisponibilidad += "Consulta," + DateTime.Now.Subtract(tiempoConsulta).TotalSeconds + ",s.,";
            string consulta = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + consultaDisponibilidad + "FINI," +
                              from.ToString("yyyy-MM-dd HH:mm:ss") + ",FFIN," + to.ToString("yyyy-MM-dd HH:mm:ss") +
                              ",Path," + Path + RegistrosRecuperados;

            return datosSP7;
        }

        /// <summary>
        /// Relación Canalcodi - Path Siemens
        /// </summary>
        /// <param name="Canalcodi">Código de canal</param>
        /// <param name="CanalTabla">Tabla</param>
        private void ObtenerEquivalenciaCanalPath(string Canalcodi, ref Hashtable CanalTabla)
        {
            List<TrCanalSp7DTO> list = FactoryScada.GetTrCanalSp7Repository().GetByIds(Canalcodi);

            foreach (TrCanalSp7DTO canal in list)
            {
                string canalcodiBD = canal.Canalcodi.ToString();
                string path = canal.Canalnomb;

                CanalTabla[canalcodiBD] = path;
            }
        }

        /// <summary>
        /// Relación Path Siemens - Canalcodi
        /// </summary>
        /// <param name="Canalnomb"></param>
        /// <param name="CanalCanalcodi"></param>
        private void ObtenerEquivalenciaPathCanal(string Canalnomb, ref Hashtable CanalCanalcodi)
        {
            List<TrCanalSp7DTO> list = FactoryScada.GetTrCanalSp7Repository().GetByCanalnomb(Canalnomb);

            foreach (TrCanalSp7DTO canal in list)
            {
                string canalcodiBD = canal.Canalcodi.ToString();
                string path = canal.Canalnomb;

                CanalCanalcodi[path] = canalcodiBD;
            }

            list.Clear();
        }

        /// <summary>
        /// Obtener históricos basado en canalcodi (separados por coma si son varios) 
        /// f_get_dato_hist (basado en Canalcodi)
        /// </summary>
        /// <param name="FechaInicio">Fecha-hora inicial</param>
        /// <param name="FechaFin">Fecha-hora final</param>
        /// <param name="Canalcodi">Canalcodi separados por coma</param>
        /// <returns></returns>
        public List<DatosSP7DTO> ObtenerDatosHistoricos(DateTime FechaInicio, DateTime FechaFin, string Canalcodi,
            string TipoTabla)
        {
            Hashtable canal = new Hashtable();
            List<DatosSP7DTO> valores = new List<DatosSP7DTO>();

            try
            {
                ObtenerEquivalenciaCanalPath(Canalcodi, ref canal);

                foreach (DictionaryEntry datos in canal)
                {
                    List<DatosSP7DTO> data1 = ObtenerDatosSP7(FechaInicio, FechaFin, datos.Value.ToString(), TipoTabla);

                    //canalcodi, Fecha-hora sistema, Fecha-hora de señal, Valor, Calidad
                    foreach (DatosSP7DTO d in data1)
                    {
                        int canalcodi = Convert.ToInt32(datos.Key);
                        DateTime fecha = d.Fecha;
                        DateTime fechaSistema = d.FechaSistema;
                        decimal valor = d.Valor;
                        int calidad = d.Calidad;

                        /*
                        DatosSP7DTO dat = new DatosSP7DTO
                        {
                            Canalcodi = canalcodi,
                            Fecha = fecha,
                            FechaSistema = fechaSistema,
                            Valor = valor,
                            Calidad = calidad
                        };*/

                        DatosSP7DTO dat = new DatosSP7DTO(canalcodi, fecha, fechaSistema, valor, calidad);

                        valores.Add(dat);

                        /*
                        valores.Add(new DatosSP7DTO
                        {
                            Canalcodi = Convert.ToInt32(datos.Key),
                            Fecha = d.Fecha,
                            FechaSistema = d.FechaSistema,
                            Valor = d.Valor,
                            Calidad = d.Calidad
                        });
                        */
                    }
                }
            }
            catch (Exception e1)
            {

            }

            try
            {
                canal.Clear();
            }
            catch { }

            return valores;
        }

        /// <summary>
        /// Permite obtener el los datos históricos basados en Path
        /// f_get_dato_hist (basado en Path)
        /// </summary>
        /// <param name="FechaInicio">Fecha inicial</param>
        /// <param name="FechaFin">Fecha final</param>
        /// <param name="Path">Path (Siemens)</param>
        /// <param name="TipoTabla">Tipo de tabla</param>
        /// <returns></returns>
        public List<DatosSP7DTO> ObtenerDatosHistoricosPath(DateTime FechaInicio, DateTime FechaFin, string Path,
            string TipoTabla)
        {

            Hashtable canal = new Hashtable();
            List<DatosSP7DTO> valores = new List<DatosSP7DTO>();

            try
            {
                ObtenerEquivalenciaPathCanal(Path, ref canal);
                List<DatosSP7DTO> data1 = ObtenerDatosSP7(FechaInicio, FechaFin, Path, "A");

                //canalcodi, Fecha-hora sistema, Fecha-hora de señal, Valor, Calidad
                foreach (DatosSP7DTO d in data1)
                {
                    int canalcodi = Convert.ToInt32(canal[d.Path]);
                    DateTime fecha = d.Fecha;
                    DateTime fechaSistema = d.FechaSistema;
                    decimal valor = d.Valor;
                    int calidad = d.Calidad;

                    /*
                    DatosSP7DTO dat = new DatosSP7DTO
                    {
                        Canalcodi = canalcodi,
                        Fecha = fecha,
                        FechaSistema = fechaSistema,
                        Valor = valor,
                        Calidad = calidad
                    };
                    */

                    DatosSP7DTO dat = new DatosSP7DTO(canalcodi, fecha, fechaSistema, valor, calidad);

                    valores.Add(dat);


                    /*
                    valores.Add(new DatosSP7DTO
                    {
                        Canalcodi = Convert.ToInt32(canal[d.Path]),
                        Fecha = d.Fecha,
                        FechaSistema = d.FechaSistema,
                        Valor = d.Valor,
                        Calidad = d.Calidad

                    });
                    */
                }

            }
            catch (Exception e1)
            {
            }

            try
            {
                canal.Clear();
            }
            catch { }

            return valores;
        }

        public List<DatosSP7DTO> ObtenerDatosHistoricos15Min(DateTime FechaInicio, DateTime FechaFin,
       bool IncluirDigital)
        {
            IncluirDigital = false;

            //List<DatosSP7DTO> ValoresDigital = new List<DatosSP7DTO>();

            //configuracion General
            int elementos = (IncluirDigital ? 2 : 1);

            string[][] diccionario = new string[elementos][];
            string[] tabla = new string[elementos];
            string[] path = new string[elementos];
            Hashtable[] canal = new Hashtable[elementos];
            Hashtable canalAnalogico = new Hashtable();
            Hashtable canalDigital = new Hashtable();


            List<DatosSP7DTO> valoresAnalogicoTotal = new List<DatosSP7DTO>();
            //ValoresDigital = new List<DatosSP7DTO>();


            try
            {

                #region diccionario Analogico

                #region Configuracion Analogica

                string tablaAnalogico = "AnalogsRaw";
                string pathAnalogico = "%/%/%/MvMoment%";
                ObtenerEquivalenciaPathCanal(pathAnalogico, ref canalAnalogico);

                #region antiguo


                string[] diccionarioAnalogico = new string[]
                {
                    "3",
                    "5",
                    "Ab",
                    "Ac",
                    "Ag",
                    "Al",
                    "Am",
                    "An",
                    "Ar",
                    "As",
                    "At",
                    "Au",
                    "Ay",
                    "Az",
                    "Bo",
                    "Bal",
                    "Bam",
                    "Bar",
                    "Campana",
                    "CE",
                    "CHA",
                    "CHCah",
                    "CHCal",
                    "CHCanonP/13.8",
                    
                    //"CHCanonP/13.8    /C",
                    //"CHCanonP/13.8    / ",
                    //"CHCanonP/13.8    /U",
                    //"CHCanonP/13.8    /G",
                    
                    
                    //"CHCanonP/13.8    /G%/P",
                    //"CHCanonP/13.8    /G%/Q",
                    //"CHCanonP/13.8    /G%/Frequ",
                    //"CHCanonP/13.8    /G%/I",
                    //"CHCanonP/13.8    /G%/V",
                    
                    "CHCanonP/ ",
                    "CHCanonP/6",
                    "CHCarh",
                    "CHCarp",
                    "CHCeAgui",
                    "CHChan",
                    "CHCharc1",
                    "CHCharc2",
                    "CHCharc3",
                    "CHCharc5",
                    "CHCharc4",
                    "CHCharc6",
                    "CHChag",
                    "CHChi",
                    "CHChe",
                    "CHCu",
                    "CHG",
                    "CHHui",
                    "CHHuas",
                    "CHHuam",
                    "CHHuan",
                    "CHL",
                    "CHMo",
                    "CHMar",
                    "CHMat",
                    "CHMan",
                    "CHMac",
                    "CHMal",
                    "CHO",
                    "CHPa",
                    "CHPl",
                    "CHPi",
                    "CHPo",
                    "CHQ",
                    "CHRe",
                    "CHRu",
                    "CHSt",
                    "CHSa",
                    "CHYu",
                    "CHYau",
                    "CHYar",
                    "CHYan",
                    "CS",
                    "CTA",
                    "CTChic",
                    "CTChim",
                    "CTChili",
                    "CTChilc1/18",
                    "CTChilc2/13.8",
                    "CTChilc1/16",
                    "CTE",
                    "CTF",
                    "CTIn",
                    "CTIll",
                    "CTIlo1",
                    "CTIlo4",
                    "CTIlo2",
                    "CTK",
                    "CTM",
                    "CTO",
                    "CTPi",
                    "CTPB",
                    "CTPt",
                    "CTPu",
                    "CTPa",
                    "CTR",
                    "CTS",
                    "CTT",
                    "CTV",
                    "Cac",
                    "Cah",
                    "Cajama",
                    "CajamN",
                    "Callah",
                    "Callal",
                    "Cama",
                    "CampA",
                    "Campa",
                    "Can",
                    "Cari",
                    "Carh",
                    "Cara",
                    "Carp",
                    "Cas",
                    "Cem",
                    "Cerr",
                    "CerA",
                    "Chal",
                    "Chavarri/1",
                    "Chavarri/6",
                    "Chavarri/2",
                    "Char",
                    "Chan",
                    "Che",
                    "ChiclayO/2",
                    "ChiclayO/1",
                    "ChiclayO/0",
                    "ChiclayO/6",
                    "Chilca  /6",
                    "Chilca  /2",
                    "ChilcaNu/5",
                    "Chilca1 /2",
                    "Chilina /33",
                    
                    //"Chilina /33      /C-ACP3",
                    //"Chilina /33      /C-TV3",
                    //"Chilina /33      /C-L300",
                    //"Chilina /33      /C-L305",
                    //"Chilina /33      /C-L306",
                    //"Chilina /33      /BARRA",
                    //"Chilina /33      /CH-T32",
                    //"Chilina /33      /C-L310",
                    //"Chilina /33      /HTR32",
                    
                    "Chill",
                    "Chimbot1",
                    "ChimbotS",
                    "Chimbot2",
                    "ChimboN",
                    "Cob",
                    "Com",
                    "Converti/33",
                    "Converti/138",
                    "Cond",
                    "Cono",
                    "Conc",
                    "Cons",
                    "Cot",
                    "Cu",
                    "D",
                    "Es",
                    "El",
                    "Et",
                    "Ex",
                    "Fo",
                    "Fl",
                    "Fe",
                    "Fr",
                    "Guadalup/2",
                    "Guadalup/6",
                    "Guadalup/1",
                    "Glor",
                    "He",
                    "Hi",
                    "Huacho",
                    "Huachi",
                    "Hualla",
                    "Hualma",
                    "Huanuc",
                    "Huanca",
                    "Huanta",
                    "Huanza",
                    "Huar",
                    "Huay",
                    "Ic",
                    "Ill",
                    "Ilo1",
                    "Ilo2",
                    "Ilo4",
                    "Ilo3",
                    "IloE",
                    "Indu",
                    "Independ/2",
                    "Independ/1",
                    "Independ/6",
                    "J",
                    "K",
                    "LaR",
                    "LaQ",
                    "LaN",
                    "Ll",
                    "Li",
                    "Lo",
                    "Lu",
                    "Macha",
                    "Machu",
                    "Maj",
                    "Mal",
                    "Marcona /1",
                    "Marcona /6",
                    "Marcona /2",
                    "Maz",
                    "Me",
                    "Mil",
                    "Min",
                    "Mirado",
                    "Mirone",
                    "Mon",
                    "Mor",
                    "Mol",
                    "Mod",
                    "Moquegua/1",
                    "Moquegua/2",
                    "N",
                    "Oc",
                    "Ox",
                    "Or",
                    "Ol",
                    "Oq",
                    "PC",
                    "PaI",
                    "Pac",
                    "Pai",
                    "Parac",
                    "Paragsh1",
                    "Paragsh2/2",
                    "Paragsh2/1",
                    "Paramo",
                    "ParamE",
                    "Pari",
                    "Pe",
                    "PiuraOes/6",
                    "PiuraOes/1",
                    "PiuraOes/2",
                    "PiuraOes/0",
                    "PiuraC",
                    "Pie",
                    "Pis",
                    "Pl",
                    "Por",
                    "Pom",
                    "Pt",
                    "Pus",
                    "Pun",
                    "Pue",
                    "Puq",
                    "Puc",
                    "Q",
                    "Ru",
                    "Re",
                    "SE",
                    "Sal",
                    "SanC",
                    "SanG",
                    "SanJo",
                    "SanJuan /6",
                    "SanJuan /0",
                    "SanJuan /1",
                    "SanJuan /2",
                    "SanL",
                    "SanM",
                    "SanN",
                    "SanR",
                    "Sant",
                    "Socabaya/1",
                    "Socabaya/2",
                    "Socabaya/3",
                    "Socabaya/0",
                    "StTe",
                    "StaA",
                    "StaC",
                    "StaI",
                    "StaM",
                    "StaRosNu/6",
                    "StaRosNu/2",
                    "StaRosAn",
                    "Su",
                    "Tam",
                    "Tal",
                    "Tac",
                    "Ting",
                    "Tintaya",
                    "TintayN",
                    "Tie",
                    "Tor",
                    "Toq",
                    "Toc",
                    "Trujil",
                    "TrujiNor/1",
                    "TrujiNor/2",
                    "Ventan",
                    "Vizcar",
                    "Y",
                    "Za",
                    "Zo"
                };


                #endregion

                /*
                string[] diccionarioAnalogico = new string[]
                {
                    "Campana",//SE LA CAMPANA                           
                    "CE",//SE Cupisnique (SE Talara)                            
                    "CHA",//CH ARICOTA 2                            
                    "CHCah",//CH CAHUA                                
                    "CHCal",//CH CALLAHUANCA                          
                    "CHCanonP/ ",
                    "CHCarh",//CH CARHUAQUERO                                              
                    "CHCeAgui",//CH Cerro del Aguila                     
                    "CHChan",//CH Chancay                              
                    "CHCharc1",//CH CHARCANI I                           
                    "CHCharc2",//CH CHARCANI II                          
                    "CHCharc3",//CH CHARCANI III                         
                    "CHCharc5",//CH CHARCANI V                           
                    "CHCharc4",//CH CHARCANI IV                          
                    "CHCharc6",//CH CHARCANI VI                          
                    "CHChag",//CH Chaglla                              
                    "CHChi",//CH CHIMAY                               
                    "CHChe",//CH CHEVES                               
                    "CHCu",//CH CURUMUY                              
                    "CHG",//CH GALLITO CIEGO                        
                    "CHHui",//CH HUINCO                               
                    "CHHuas",//CH HUASAHUASI II                        
                    "CHHuam",//CH HUAMPANI                             
                    "CHHuan",//CH HUANZA                               
                    "CHMo",//CH MOYOPAMPA                            
                    "CHMar",//CH MARANON                              
                    "CHMat",//CH MATUCANA                             
                    "CHMan",//CH MANTARO                              
                    "CHMac",//CH MACHUPICCHU II                       
                    "CHMal",//CH MALPASO                              
                    "CHO",//CH OROYA                                
                    "CHPa",//CH PACHACHACA                           
                    "CHPl",//CH EL PLATANAL                          
                    "CHPi",//CH LAS PIZARRAS                         
                    "CHQ",//SE QUITARACSA I                         
                    "CHRu",//CH Rucuy                                
                    "CHSt",//CH Santa Teresa                         
                    "CHYau",//CH YAUPI                                
                    "CHYan",//CH YANAPAMPA                            
                    "CS",//CS TACNA SOLAR                          
                    "CTA",//CT AGUAYTIA                             
                    "CTChili",//CT CHILINA                              
                    "CTE",//CT PLANTA ETANOL                        
                    "CTF",//CT LAS FLORES                           
                    "CTIn",//CT INDEPENDENCIA                                            
                    "CTIlo1",//CT ILO 1                                                    
                    "CTIlo2",//CT ILO 2                                
                    "CTK",//CT KALLPA                               
                    "CTM",//CT MOLLENDO                             
                    "CTO",//CT STO. DOMINGO DE OLLEROS              
                    "CTPB",//CT Puerto Bravo                         
                    "CTPa",//CT PARAMONGA                            
                    "CTR",//CT Recka                                
                    "CTV",//CT VENTANILLA                           
                    "PC"//PCH Chaglla                             
                };
                */


                diccionario[0] = diccionarioAnalogico;
                tabla[0] = tablaAnalogico;
                path[0] = pathAnalogico;
                canal[0] = canalAnalogico;

                #endregion

                #region Configuracion Digital

                if (IncluirDigital)
                {
                    string tablaDigital = "DigitalsRaw";
                    string pathDigital = "%/%/%/Status%";
                    ObtenerEquivalenciaPathCanal(pathDigital, ref canalDigital);

                    #region diccionario Digital

                    string[] diccionarioDigital = new string[]
                    {
                        "Ab", "Ac", "Ag", "Al", "Am", "An", "Ar", "As", "At", "Au", "Ay", "Az",
                        "Bal", "Bam", "Bar", "Bo",
                        "Cac", "Cah", "Caj", "Cal", "Cam", "Can", "Cas", "Cara", "Carh", "Cari", "Carp", "CHA", "CHCa",
                        "CHCe", "CHCh", "CHCu", "CHG", "CHH",
                        "CHL", "CHM", "CHO", "CHP", "CHQ", "CHR", "CHS", "CHY", "Che",
                        "Chic", "ChimboN", "ChimbotS", "Chimbot1/1", "Chimbot1/2", "Chimbot2", "Chilca  /2",
                        "Chilca  /6", "ChilcaN", "Chilca1",
                        "Chili", "Chill", "Chal", "Chavarri/1", "Chavarri/2", "Chavarri/4", "Chavarri/6",
                        "Cob", "Com", "Con", "Cot",
                        "CTA", "CTC", "CTE", "CTF", "CTI", "CTK", "CTM", "CTO", "CTP", "CTR", "CTS", "CTT", "CTV",
                        "CE", "Ce", "CS", "Cu",
                        "D",
                        "E",
                        "F",
                        "G",
                        "He", "Hi", "Huac", "Hual", "Huan", "Huar", "Huay",
                        "Ic", "Ill", "IloE", "Ilo1", "Ilo2", "Ilo3", "Ilo4", "Inde", "Indu",
                        "J",
                        "K",
                        "La", "Li", "Ll", "Lo", "Lu",
                        "Mac", "Maj", "Mal", "Mar", "Maz", "Me", "Mil", "Min", "Mir", "Mod", "Mol", "Mon", "Moq", "Mor",
                        "N",
                        "Oc", "Ol", "Or", "Ox",
                        "Pac", "PaI", "Pai", "Parac", "Parag", "Param", "Pari", "PC", "Pe", "Pi", "Pl", "Pom", "Por",
                        "Pt", "Pu", "Q",
                        "R",
                        "Sal", "SanC", "SanG", "SanL", "SanM", "SanN", "SanR", "Sant", "SanJo", "SanJuan /2",
                        "SanJuan /6", "SE", "Si", "Socabaya/1", "Socabaya/2", "Socabaya/3",
                        "StT", "StaA", "StaC", "StaI", "StaM", "StaRosA", "StaRosNu/2", "StaRosNu/6", "Su",
                        "Ta", "Tie", "Ting", "Tint", "To", "Trujil", "TrujiNor/1", "TrujiNor/2",
                        "V",
                        "Y",
                        "Z",
                        "3",
                        "5"
                    };

                    #endregion

                    diccionario[1] = diccionarioDigital;
                    tabla[1] = tablaDigital;
                    path[1] = pathDigital;
                    canal[1] = canalDigital;

                }

                #endregion

                DateTime to = FechaFin;
                //string consultaDisponibilidad = "";

                DateTime fromAnalogico = FechaInicio;
                DateTime fromDigital = fromAnalogico.AddMinutes(-15);

                {
                    DateTime tiempoAperturaConexion = DateTime.Now;

                    /*consultaDisponibilidad += ",Conexion," + DateTime.Now.Subtract(tiempoAperturaConexion).TotalSeconds +
                                              ",s.,";*/


                    int registrosRecuperadosTotal = 0;
                    DateTime tiempoConsulta2 = DateTime.Now;

                    for (int i = 0; i < elementos; i++)
                    {
                        string[] diccionarioItem = diccionario[i];
                        string tablaItem = tabla[i];
                        string pathItem = path[i];
                        Hashtable canalItem = canal[i];
                        DateTime from;

                        if (i == 0)
                        {
                            from = fromAnalogico;
                        }
                        else
                        {
                            from = fromDigital;
                        }

                        //--- 1


                        foreach (string ssee in diccionarioItem)
                        {
                            Hashtable canalValorDigital = new Hashtable();
                            DateTime tiempoConsulta = DateTime.Now;
                            string pathDiccionario = "/" + ssee + (ssee.IndexOf("/") > 0 ? "" : "%/") + pathItem;
                            //string registrosRecuperados = "";

                            try
                            {
                                int cuenta = 0;

                                if (i == 0)
                                {
                                    List<DatosSP7DTO> valoresAnalogico;

                                    try
                                    {
                                        valoresAnalogico =
                                        ListadoDatosSp7(tablaItem, from, to, pathDiccionario).ToList();
                                    }
                                    catch
                                    {
                                        valoresAnalogico = new List<DatosSP7DTO>();
                                    }

                                    foreach (DatosSP7DTO item in valoresAnalogico)
                                    {
                                        /*
                                        item.Canalcodi = Convert.ToInt32(canalItem[item.Path]);
                                        item.TipoPunto = "A";
                                        valoresAnalogicoTotal.Add(item);*/


                                        int canalcodi = Convert.ToInt32(canalItem[item.Path]);
                                        DateTime fecha = item.Fecha;
                                        DateTime fechaSistema = item.FechaSistema;
                                        decimal valor = item.Valor;
                                        int calidad = item.Calidad;
                                        string tipopunto = "A";

                                        /*
                                        DatosSP7DTO dat = new DatosSP7DTO
                                        {
                                            Canalcodi = canalcodi,
                                            Fecha = fecha,
                                            FechaSistema = fechaSistema,
                                            Valor = valor,
                                            Calidad = calidad,
                                            TipoPunto = tipopunto
                                        };*/

                                        //si no existe la actualización ingresa el punto
                                        int contadorPunto = valoresAnalogicoTotal.Where(x => x.Canalcodi == canalcodi).Count();

                                        if (contadorPunto == 0)
                                        {
                                            DatosSP7DTO dat = new DatosSP7DTO(canalcodi, fecha, fechaSistema, valor, calidad, tipopunto);
                                            valoresAnalogicoTotal.Add(dat);
                                        }

                                        /*
                                        valoresAnalogicoTotal.Add(new DatosSP7DTO
                                        {
                                            Canalcodi = Convert.ToInt32(canalItem[item.Path]),
                                            Fecha = item.Fecha,
                                            FechaSistema = item.FechaSistema,
                                            Valor = item.Valor,
                                            Calidad = item.Calidad,
                                            TipoPunto = "A"
                                        });*/

                                        cuenta++;
                                    }

                                    valoresAnalogico.Clear();


                                }
                                else
                                {
                                    List<DatosSP7DTO> registrosDigital;

                                    try
                                    {
                                        registrosDigital =
                                        ListadoDatosSp7(tablaItem, from, to, pathDiccionario).ToList();
                                    }
                                    catch
                                    {
                                        registrosDigital = new List<DatosSP7DTO>();
                                    }

                                    //digitales
                                    foreach (DatosSP7DTO item in registrosDigital)
                                    {
                                        int canalcodiDigital = Convert.ToInt32(item.Path);
                                        item.Canalcodi = canalcodiDigital;
                                        item.TipoPunto = "D";

                                        if (!canalValorDigital.Contains(canalcodiDigital))
                                        {
                                            canalValorDigital[canalcodiDigital] = item;
                                        }
                                        else
                                        {
                                            DatosSP7DTO ultimoDatoDigital =
                                                (DatosSP7DTO)canalValorDigital[canalcodiDigital];

                                            if (item.FechaSistema > ultimoDatoDigital.FechaSistema)
                                            {
                                                canalValorDigital[canalcodiDigital] = item;
                                            }
                                        }

                                        cuenta++;
                                    }

                                    registrosDigital.Clear();

                                }

                                try
                                {
                                    //creando datos historicos
                                    if (i != 0)
                                    {
                                        foreach (DictionaryEntry entry in canalValorDigital)
                                        {
                                            //ValoresDigital.Add((DatosSP7DTO)entry.Value);  
                                            DatosSP7DTO datoDigital = (DatosSP7DTO)entry.Value;
                                            int contadorPunto = valoresAnalogicoTotal.Where(x => x.Canalcodi == datoDigital.Canalcodi).Count();

                                            if (contadorPunto == 0)
                                            {
                                                valoresAnalogicoTotal.Add((DatosSP7DTO)entry.Value);
                                            }

                                        }

                                    }

                                    /*registrosRecuperados += ",Reg," + cuenta + ",Proceso " + tablaItem + ",OK";
                                    registrosRecuperadosTotal += cuenta;*/
                                }
                                catch
                                {
                                    /*registrosRecuperados += ",Reg," + cuenta + ",Proceso " + tablaItem + ",ERROR";
                                    registrosRecuperadosTotal += cuenta;*/
                                }

                            }
                            catch (Exception ExceptionSiemens)
                            {

                                /*registrosRecuperados += ",Reg," + "0" + ",Proceso " + tablaItem + ",ERROR Siemens" +
                                                        ExceptionSiemens.ToString();*/

                                /*string consultaError = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ",Error en Path " +
                                                       tablaItem + "," + pathDiccionario + ",FINI," +
                                                       from.ToString("yyyy-MM-dd HH:mm:ss") + ",FFIN," +
                                                       to.ToString("yyyy-MM-dd HH:mm:ss");*/
                            }

                            canalValorDigital.Clear();
                            /*consultaDisponibilidad += "Consulta," + DateTime.Now.Subtract(tiempoConsulta).TotalSeconds +
                                                      ",s.,";*/

                            /*string consulta = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + consultaDisponibilidad +
                                              "FINI," + from.ToString("yyyy-MM-dd HH:mm:ss") + ",FFIN," +
                                              to.ToString("yyyy-MM-dd HH:mm:ss") + ",Path," + pathDiccionario +
                                              registrosRecuperados;*/

                            //consultaDisponibilidad = ",Conexion," + "ABIERTA" + ",s.,";
                        }

                        //--- 1 FIN

                        //limpiando registros
                        Array.Clear(diccionarioItem, 0, diccionarioItem.Length);
                        canalItem.Clear();
                        diccionarioItem = null;
                        canalItem = null;
                    }

                    /*consultaDisponibilidad = "Tiempo total de Proceso," +
                                             DateTime.Now.Subtract(tiempoConsulta2).TotalSeconds + ",s.,";*/

                    /*string consultaTotal = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "," + consultaDisponibilidad +
                                           ",FFIN," + to.ToString("yyyy-MM-dd HH:mm:ss") + ",Total Registros," +
                                           registrosRecuperadosTotal;*/
                }

                #endregion

                Array.Clear(diccionarioAnalogico, 0, diccionarioAnalogico.Length);


            }
            catch (Exception e1)
            {
                //Logger.Error(ConstantesAppServicio.LogError, e1);
            }


            //limpiando registros
            for (int i = 0; i < elementos; i++)
            {
                Array.Clear(diccionario[i], 0, diccionario[i].Length);
                canal[i].Clear();

            }

            try
            {
                Array.Clear(tabla, 0, tabla.Length);
                Array.Clear(path, 0, path.Length);
                canalAnalogico.Clear();
                canalDigital.Clear();




                diccionario = null;
                tabla = null;
                path = null;
                canal = null;
                canalAnalogico = null;
                canalDigital = null;
            }
            catch
            {

            }


            return valoresAnalogicoTotal;
        }

        #region Métodos Tabla DATOS_SP7

        /// <summary>
        /// Permite obtener el listado de datos HIS
        /// </summary>
        /// <param name="fechaInicial">fecha incial</param>
        /// <param name="fechaFinal">fecha final</param>
        /// <param name="path">path Siemens</param>
        /// <returns>Lista de DatosSP7DTO</returns>
        public List<DatosSP7DTO> ListadoDatosSp7(string tabla, DateTime fechaInicial, DateTime fechaFinal, string path)
        {
            List<DatosSP7DTO> entitys;

            try
            {
                //return FactoryScada.GetDatosSp7Repository().ObtenerListadoSp7(tabla, fechaInicial, fechaFinal, path);
                entitys = FactoryScada.GetDatosSp7Repository().ObtenerListadoSp7(tabla, fechaInicial, fechaFinal, path);

            }
            catch (Exception ex)
            {
                //Logger.Error(ConstantesAppServicio.LogError, ex);
                //throw new Exception(ex.Message, ex);
                //return new List<DatosSP7DTO>();
                entitys = new List<DatosSP7DTO>();
            }

            return entitys;
        }


        #endregion

        public string canalcodi { get; set; }
    }
}
