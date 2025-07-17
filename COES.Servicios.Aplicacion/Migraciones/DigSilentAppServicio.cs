using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Migraciones.Helper;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.IEOD;
using System.IO;
using COES.Framework.Base.Tools;

namespace COES.Servicios.Aplicacion.Migraciones
{
    public class DigSilentAppServicio
    {
        /// <summary>
        /// Proceso digsilent
        /// </summary>
        /// <param name="fechaPeriodo"></param>
        /// <param name="program"></param>
        /// <param name="rdchk"></param>
        /// <param name="bloq"></param>
        /// <param name="fuenteReprog"></param>
        /// <param name="topcodiYupana"></param>
        /// <param name="resultado"></param>
        /// <param name="comentario"></param>
        /// <param name="configuracion"></param>
        /// <param name="validacionDuplicadoForeignKey"></param>
        public void ProcesarDIgSILENT(DateTime fechaPeriodo, string program, int rdchk, string bloq, int fuenteReprog, int topcodiYupana
            , out string resultado, out string comentario
            , out string configuracion, out string validacionDuplicadoForeignKey)
        {
            List<EqEquirelDTO> ListaEquirel = new List<EqEquirelDTO>();
            List<MeMedicion24DTO> ListaGeneracionOpera = new List<MeMedicion24DTO>();
            List<MeMedicion24DTO> ListaGeneracionNoOpera = new List<MeMedicion24DTO>();
            List<MeMedicion24DTO> ListaDemanda = new List<MeMedicion24DTO>();
            List<EqEquipoDTO> ListaTrafo3d = new List<EqEquipoDTO>();
            List<EqEquipoDTO> ListaTrafo2d = new List<EqEquipoDTO>();
            List<EqEquipoDTO> ListaSvc = new List<EqEquipoDTO>();
            List<EqEquipoDTO> ListaLineas = new List<EqEquipoDTO>();
            List<EveManttoDTO> ListaManttos = new List<EveManttoDTO>();

            configuracion = string.Empty;
            validacionDuplicadoForeignKey = string.Empty;

            int siparcodiDigSdemanda = ConstantesMigraciones.SiparcodiDigsilentDemanda;
            string tiporelcodi = ConstantesMigraciones.TiporelcodiDigsilent, evenclasecodi = string.Empty;
            string propcodiDigsilentLinea = ConstantesMigraciones.PropcodiDigsilentLinea, propcodiDigsilentSvc = ConstantesMigraciones.PropcodiDigsilentSvc;
            string propcodiDigsilentTrafo2d = ConstantesMigraciones.PropcodiDigsilentTrafo2d, propcodiDigsilentTrafo3d = ConstantesMigraciones.PropcodiDigsilentTrafo3d;
            SiParametroDTO siparametros = GetByCriteriaSiparametro(siparcodiDigSdemanda);
            string propcodiDigsilentDemanda = (siparametros != null ? siparametros.Sipardescripcion : ConstantesMigraciones.PropcodiDigsilentDemanda);

            string famcodiLinea = ConstantesAppServicio.FamcodiLinea, famcodiSvc = ConstantesAppServicio.FamcodiSvc;
            string famcodiTran2d = ConstantesAppServicio.Famcoditrafo2d, famcodiTran3d = ConstantesAppServicio.Famcoditrafo3d;
            string famcodiDemanda = ConstantesAppServicio.FamcodiDemanda;

            int lectcodiOrigr = int.Parse(program);

            evenclasecodi = GetEvenclasecodiByLectcodi(lectcodiOrigr).ToString();

            ListaEquirel = GetListaEquirelDigsilent(tiporelcodi);//Equirel
            ListaManttos = GetListaManttosDigsilent(evenclasecodi, fechaPeriodo);//Manttos
            if (rdchk == 0 || rdchk == 1)
            {
                ListaLineas = GetListaLineasDigsilent(propcodiDigsilentLinea, famcodiLinea); //Lineas
            }
            if (rdchk == 0 || rdchk == 2)
            {
                List<MeMedicion24DTO> listaM24Digsilent = GetListaGeneracionDIgSILENT(program, fuenteReprog, topcodiYupana, fechaPeriodo);
                ListaGeneracionNoOpera = listaM24Digsilent.Where(x => x.Meditotal == null).ToList();//Generacion No Opera
                ListaGeneracionOpera = listaM24Digsilent.Where(x => x.Meditotal != null).ToList(); //Generacion Opera

                configuracion = ReporteConfiguracionUnidadesOperaHtml(listaM24Digsilent);
                validacionDuplicadoForeignKey = VerificarDuplicadosForeignKey(listaM24Digsilent);
            }
            if (rdchk == 0 || rdchk == 3)
            {
                ListaTrafo2d = GetListaLineasDigsilent(propcodiDigsilentTrafo2d, famcodiTran2d);//Transformador 2d
                ListaTrafo3d = GetListaLineasDigsilent(propcodiDigsilentTrafo3d, famcodiTran3d);//Transformador 3d
            }
            if (rdchk == 0 || rdchk == 4)
            {
                ListaSvc = GetListaLineasDigsilent(propcodiDigsilentSvc, famcodiSvc); //Svc
            }
            if (rdchk == 0 || rdchk == 5)
            {
                ListaDemanda = GetListaDemandaDigsilent(propcodiDigsilentDemanda, famcodiDemanda, fechaPeriodo); //Demanda
            }

            //Procesar Horas
            StringBuilder result_ = new StringBuilder();
            StringBuilder coment_ = new StringBuilder();

            result_.AppendFormat("dole/dbupd/fkey<br>");
            result_.AppendFormat("cls/out<br>");
            result_.AppendFormat("ac/de all<br>");
            result_.AppendFormat("ac {0}.IntPrj<br>", this.GetFechaDigsilent(fechaPeriodo));
            result_.AppendFormat("cd {0} <br>", this.GetFechaDigsilent(fechaPeriodo));

            List<string> horas = bloq.Split(',').ToList();

            foreach (var h in horas)
            {

                result_.Append("ac " + h + "h.IntCase<br>");
                if (rdchk == 0 || rdchk == 1)
                {
                    #region lineas
                    List<EqEquipoDTO> lineas_ = new List<EqEquipoDTO>();

                    foreach (var l in ListaLineas)
                    {
                        if (l.Equicodi == 1073)
                        {

                        }
                        lineas_.AddRange(ListaDigSilentEqequipo((int.Parse(h) - 1), l, ListaManttos, ListaEquirel));
                    }

                    result_.Append("echo Cargando datos de disponibilidad de lineas...<br>set/def obj=ElmLne var=outserv<br>");
                    result_.Append(ArchivoDigsilentHtml(lineas_.OrderBy(x => x.Correlativo).ToList()));
                    #endregion
                }

                if (rdchk == 0 || rdchk == 2)
                {
                    #region generadores no operan
                    result_.Append("echo Cargando datos de generacion...<br>");
                    result_.Append("set/def obj=ElmGenstat var=pgini,outserv<br>");
                    result_.Append("set/def obj=ElmSym var=pgini,outserv<br>");

                    foreach (var d in ListaGeneracionNoOpera)
                    {
                        if (string.IsNullOrEmpty(d.Digsilent))
                            coment_.Append(d.Gruponomb + " " + d.Equiabrev + " Definicion Digsilent no existe<br>");
                        else
                            result_.Append("set/fkey obj=" + d.Digsilent + " val=0,1<br>");
                    }
                    #endregion

                    #region generadores operan

                    foreach (var agrupGrupoDespacho in ListaGeneracionOpera.GroupBy(x => x.Grupocodi))
                    {
                        int grupocodi = agrupGrupoDespacho.Key;
                        List<MeMedicion24DTO> listaGeneracionOperaXGrupo = agrupGrupoDespacho.ToList();

                        if (grupocodi == 961)
                        { }

                        try
                        {
                            this.ListaDigSilentPrgrupo(grupocodi, int.Parse(h) - 1, ref result_, ref coment_, listaGeneracionOperaXGrupo, ListaManttos);
                        }
                        catch (Exception ex)
                        { }

                    }
                    #endregion
                }

                if (rdchk == 0 || rdchk == 3)
                {
                    #region trafo 2d y 3d
                    List<EqEquipoDTO> trafo2d_ = new List<EqEquipoDTO>();
                    List<EqEquipoDTO> trafo3d_ = new List<EqEquipoDTO>();

                    //2D
                    result_.Append("echo Cargando datos de disponibilidad de transformadores...<br>set/def obj=ElmTr2 var=outserv<br>");
                    foreach (var l in ListaTrafo2d)
                    {
                        trafo2d_.AddRange(ListaDigSilentEqequipo((int.Parse(h) - 1), l, ListaManttos, ListaEquirel));
                    }
                    result_.Append(ArchivoDigsilentHtml(trafo2d_.OrderBy(x => x.Correlativo).ToList()));

                    //3D
                    result_.Append("set/def obj=ElmTr3 var=outserv<br>");
                    foreach (var l in ListaTrafo3d)
                    {
                        trafo3d_.AddRange(ListaDigSilentEqequipo((int.Parse(h) - 1), l, ListaManttos, ListaEquirel));
                    }
                    result_.Append(ArchivoDigsilentHtml(trafo3d_.OrderBy(x => x.Correlativo).ToList()));
                    #endregion
                }

                if (rdchk == 0 || rdchk == 4)
                {
                    #region svc
                    List<EqEquipoDTO> svc_ = new List<EqEquipoDTO>();

                    foreach (var l in ListaSvc)
                    {
                        svc_.AddRange(ListaDigSilentEqequipo(int.Parse(h), l, ListaManttos, ListaEquirel));
                    }

                    result_.Append("echo Cargando datos de disponibilidad de SVC...<br>set/def obj=ElmSvs var=outserv<br>");
                    result_.Append(ArchivoDigsilentHtml(svc_.OrderBy(x => x.Correlativo).ToList()));
                    #endregion
                }

                if (rdchk == 0 || rdchk == 5)
                {
                    #region demanda
                    decimal MW = 0, MVAR = 0;

                    result_.Append("echo Cargando datos de Demanda...<br>set/def obj=ElmFeeder var=i_scalepf,Qset,outserv<br>");

                    var listaEquipo = ListaDemanda.GroupBy(x => new { x.Equicodi, x.Digsilent }).Select(x => new { x.Key.Equicodi, x.Key.Digsilent }).ToList();

                    for (int m = 0; m < listaEquipo.Count; m++)
                    {
                        string digsilent_ = listaEquipo[m].Digsilent;
                        var det = ListaDemanda.Where(x => x.Equicodi == listaEquipo[m].Equicodi).ToList();
                        var regMW = det.Where(x => x.Tipoinfocodi == ConstantesAppServicio.TipoinfocodiMW).FirstOrDefault();
                        var regMVAR = det.Where(x => x.Tipoinfocodi == ConstantesAppServicio.TipoinfocodiMVAR).FirstOrDefault();

                        MW = regMW != null ? ((decimal?)regMW.GetType().GetProperty("H" + int.Parse(h).ToString()).GetValue(regMW, null)).GetValueOrDefault(0) : 0;
                        MVAR = regMVAR != null ? ((decimal?)regMVAR.GetType().GetProperty("H" + int.Parse(h).ToString()).GetValue(regMVAR, null)).GetValueOrDefault(0) : 0;
                        int valor = 0;
                        if (MW > 0 && MVAR != 0)
                        {
                            valor = 0;
                        }
                        else
                        {
                            if (MW == 0 && MVAR == 0)
                            {
                                valor = 1;
                            }
                            else
                            {
                                valor = 0;
                            }
                        }

                        result_.Append("set/fkey obj=" + digsilent_ + " val=3," + MW + ",1," + MVAR + "," + valor + "<br>");
                    }

                    if (ListaDemanda.Count == 0)
                    {
                        coment_.Append("Datos de demanda NO fueron cargados. Se obtienen de Demanda Típica<br>");
                    }

                    #endregion
                }
            }

            resultado = result_.ToString();
            comentario = coment_.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="siparcodiDigSdemanda"></param>
        /// <returns></returns>
        public SiParametroDTO GetByCriteriaSiparametro(int siparcodiDigSdemanda)
        {
            return FactorySic.GetSiParametroRepository().GetById(siparcodiDigSdemanda);
        }
        /// <summary>
        /// Obtener el evenclase segun la lectura
        /// </summary>
        /// <param name="lectcodiOrig"></param>
        /// <returns></returns>
        public static int GetEvenclasecodiByLectcodi(int lectcodiOrig)
        {
            int evenclasecodi = 1;

            string lectcodi = lectcodiOrig + string.Empty;
            switch (lectcodi)
            {
                case ConstantesAppServicio.LectcodiEjecutadoHisto: //6
                case ConstantesAppServicio.LectcodiEjecutado: //93
                case ConstantesAppServicio.LectcodiReprogDiario: //5
                    evenclasecodi = ConstantesAppServicio.EvenclasecodiEjecutado;
                    break;
                case ConstantesAppServicio.LectcodiProgDiario: //4
                case ConstantesAppServicio.LectcodiAjusteDiario: //7
                    evenclasecodi = ConstantesAppServicio.EvenclasecodiProgDiario;
                    break;
                case ConstantesAppServicio.LectcodiProgSemanal: //3
                    evenclasecodi = ConstantesAppServicio.EvenclasecodiProgSemanal;
                    break;
            }

            return evenclasecodi;
        }
        /// <summary>
        /// Get informacion equirel digsilent
        /// </summary>
        /// <returns></returns>
        public List<EqEquirelDTO> GetListaEquirelDigsilent(string tiporelcodi)
        {
            return FactorySic.GetEqEquirelRepository().GetByCriteria(int.Parse(ConstantesAppServicio.ParametroDefecto), tiporelcodi);
        }
        /// <summary>
        /// Get informacion mantenimientos digsilent
        /// </summary>
        /// <param name="evenclasecodi"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<EveManttoDTO> GetListaManttosDigsilent(string evenclasecodi, DateTime fecha)
        {
            return FactorySic.GetEveManttoRepository().ListaManttosDigsilent(evenclasecodi, fecha);
        }
        /// <summary>
        /// Get informacion lineas digsilent
        /// </summary>
        /// <param name="propcodi"></param>
        /// <param name="famcodi"></param>
        /// <returns></returns>
        public List<EqEquipoDTO> GetListaLineasDigsilent(string propcodi, string famcodi)
        {
            return FactorySic.GetEqEquipoRepository().ListaLineasDigsilent(propcodi, famcodi);
        }
        /// <summary>
        /// Get informacion generacion que opero
        /// </summary>
        /// <param name="lectcodi"></param>
        /// <param name="tipoFuenteReprog"></param>
        /// <param name="topcodiYupana"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<MeMedicion24DTO> GetListaGeneracionDIgSILENT(string lectcodi, int tipoFuenteReprog, int topcodiYupana, DateTime fecha)
        {
            //obtener configuración Digsilent, Potencia máxima, potencia mínima, potencia efectiva
            var lista = FactorySic.GetMeMedicion24Repository().ListaGeneracionDIgSILENT(fecha);

            //Obtener data de medición
            bool reprogAprobado = true;
            List<MeMedicion48DTO> listaDespachoYupanaReprog = new List<MeMedicion48DTO>(), listaDespachoYupanaReprogHidroXEq = new List<MeMedicion48DTO>();

            //Reprog Yupana y preliminar
            if (ConstantesAppServicio.LectcodiReprogDiario == lectcodi && tipoFuenteReprog == 2 && topcodiYupana > 0)
            {
                listaDespachoYupanaReprog = DespachoYupana(fecha, fecha, topcodiYupana, string.Empty, ConstantesBase.IdlectDespachoReprog, out listaDespachoYupanaReprogHidroXEq);

                listaDespachoYupanaReprog = listaDespachoYupanaReprog.Where(x => x.Meditotal != 0).ToList();
                listaDespachoYupanaReprogHidroXEq = listaDespachoYupanaReprogHidroXEq.Where(x => x.Meditotal != 0).ToList();

                reprogAprobado = false;
            }

            List<MeMedicion48DTO> listaMe48 = reprogAprobado ? GetObtenerHistoricoMedicion48(Int32.Parse(lectcodi), fecha, fecha) : listaDespachoYupanaReprog;
            List<MeMedicion48DTO> listaMe48HidroXGen = reprogAprobado ? new List<MeMedicion48DTO>() : listaDespachoYupanaReprogHidroXEq;
            listaMe48 = listaMe48.Where(x => x.Tipoinfocodi == ConstantesAppServicio.TipoinfocodiMW).ToList();

            //Volver a calcular el meditotal
            int grupocodiFicticio = 100000; //el ficticio es para que no se distribuya en generadores que sí tienen despacho x equipo
            foreach (var m24 in lista)
            {
                decimal meditotal = 0;

                //buscar en medicion48 si existe el equipo
                var regm48 = listaMe48.Find(x => x.Ptomedicodi == m24.Ptomedicodi);
                bool existePtoXgen = listaMe48HidroXGen.Find(x => x.Ptomedicodi == m24.Ptomedicodi) != null; //si existe algun equipo distribuido en yupana

                MeMedicion48DTO reg48bd = regm48;
                if (existePtoXgen)
                {
                    //obtener datos de medias horas
                    reg48bd = listaMe48HidroXGen.Find(x => x.Ptomedicodi == m24.Ptomedicodi && x.Equicodi == m24.Equicodi);

                    //asignar nuevo codigo al grupocodi
                    m24.Grupocodi = grupocodiFicticio;
                    grupocodiFicticio++;
                }

                if (reg48bd != null)
                {
                    for (int h = 1; h <= 24; h++)
                    {
                        //El valor del H de medicion48 es el mismo para todos los equipos del grupo despacho
                        decimal valorH = ((decimal?)reg48bd.GetType().GetProperty(ConstantesAppServicio.CaracterH + h * 2).GetValue(reg48bd, null)).GetValueOrDefault(0);
                        m24.GetType().GetProperty(ConstantesAppServicio.CaracterH + (h).ToString()).SetValue(m24, valorH);

                        meditotal += valorH;
                    }
                }

                if (meditotal > 0)
                    m24.Meditotal = meditotal;
            }

            //Completar información RE-10650
            lista = this.CompletarDigSilentEolicaSolares(lista);

            lista = lista.Where(x => ConstantesHorasOperacion.IdTipoEolica != x.Famcodi && ConstantesHorasOperacion.IdTipoSolar != x.Famcodi).ToList();

            //Formatear data
            foreach (var reg in lista)
            {
                reg.Digsilent = !string.IsNullOrEmpty(reg.Digsilent) ? reg.Digsilent.Trim() : string.Empty;
                reg.FechapropequiMinDesc = reg.FechapropequiMin != null ? reg.FechapropequiMin.Value.ToString(ConstantesAppServicio.FormatoFecha) : string.Empty;
                reg.FechapropequiPotefecDesc = reg.FechapropequiPotefec != null ? reg.FechapropequiPotefec.Value.ToString(ConstantesAppServicio.FormatoFecha) : string.Empty;
            }

            return lista;
        }
        /// <summary>
        /// Devuelve una lista de resultados de Yupana en MeMedicion48
        /// </summary>
        /// <param name="fechaini"></param>
        /// <param name="fechafin"></param>
        /// <param name="topcodi"></param>
        /// <param name="usuario"></param>
        /// <param name="lectcodi"></param>
        /// <param name="listaDespachoHidroXEq"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> DespachoYupana(DateTime fechaini, DateTime fechafin, int topcodi, string usuario, int lectcodi, out List<MeMedicion48DTO> listaDespachoHidroXEq)
        {
            List<MeMedicion48DTO> lista = new List<MeMedicion48DTO>();
            string equiestado = "'A','F','P','B'", ptomediestado = "'A'", grupoactivo = "'S','N'", emprestado = "A";
            List<PrGrupoDTO> listaGrupo = new List<PrGrupoDTO>();
            List<MePtomedicionDTO> listaPtoDespacho30min = new List<MePtomedicionDTO>();
            var AllListaGrupo = FactorySic.GetPrGrupoRepository().ListarAllGrupoGeneracion(fechaini, grupoactivo, emprestado);

            var listaPtoDespacho = FactorySic.GetMePtomedicionRepository().GetByCriteria(ConstantesAppServicio.ParametroDefecto, "2", ConstantesAppServicio.ParametroDefecto);
            listaPtoDespacho30min.AddRange(listaPtoDespacho.Where(x => x.Grupoactivo == "S").ToList());

            listaGrupo.AddRange(AllListaGrupo.Where(x => x.Grupoactivo == "S").ToList());
            var listaOrder = listaGrupo;
            listaGrupo = new List<PrGrupoDTO>();
            listaGrupo.AddRange(listaOrder);
            listaGrupo.AddRange(this.FillCabeceraCdispatchHistorico().Where(x => x.Equiabrev != "").ToList());

            //Convertir Lista Pr_Grupo a Ptomedicion
            var lpto = listaGrupo.Where(x => x.Emprcodi == 9992).Select(x => x.Ptomedicodi).ToArray();
            var lptoCm = listaGrupo.Where(x => x.Emprcodi == 9993).Select(x => x.Ptomedicodi).ToArray();
            var lptos = String.Join(", ", lpto);
            var listaPtos = FactorySic.GetMePtomedicionRepository().List(lptos, ConstantesAppServicio.ParametroDefecto);
            var lptoCms = String.Join(", ", lptoCm);
            var listaPtosCm = FactorySic.GetMePtomedicionRepository().List(lptoCms, ConstantesAppServicio.ParametroDefecto);

            var listaFlujo = ListaFlujosMcp(topcodi, listaPtos, fechaini, fechafin, usuario, lectcodi);
            CompletarTotalMeMecicion48(ref listaFlujo);
            lista.AddRange(listaFlujo);

            var listaCostoMarginal = ListaCMarginalMcp(topcodi, listaPtosCm, fechaini, fechafin, usuario, lectcodi);
            CompletarTotalMeMecicion48(ref listaCostoMarginal);
            lista.AddRange(listaCostoMarginal);

            List<MeMedicion48DTO> listaMe48 = ListaHidroMcp(topcodi, usuario, lectcodi, out listaDespachoHidroXEq).Where(x => x.Medifecha >= fechaini && x.Medifecha <= fechafin).ToList();
            CompletarTotalMeMecicion48(ref listaMe48);
            lista.AddRange(listaMe48);
            listaDespachoHidroXEq = listaDespachoHidroXEq.Where(x => x.Medifecha >= fechaini && x.Medifecha <= fechafin).ToList();

            var ltermicas = ListaTermicaMcp(topcodi, usuario, lectcodi).Where(x => x.Medifecha >= fechaini && x.Medifecha <= fechafin).ToList();
            var ltermicasCC = ListaTermicasCCMcp(topcodi, fechaini, fechafin, usuario, lectcodi);
            UnirTotalTermicas(ref ltermicas, ltermicasCC);
            CompletarTotalMeMecicion48(ref ltermicas);
            lista.AddRange(ltermicas);

            var lrer = ListaPlantasRerMcp(topcodi, ConstantesBase.SRES_GENER_RER.ToString(), usuario, lectcodi).Where(x => x.Medifecha >= fechaini && x.Medifecha <= fechafin).ToList();
            CompletarTotalMeMecicion48(ref lrer);
            lista.AddRange(lrer);

            var ltotalSein = ListaTotalSein(topcodi, usuario, lectcodi).Where(x => x.Medifecha >= fechaini && x.Medifecha <= fechafin).ToList();
            CompletarTotalMeMecicion48(ref ltotalSein);
            lista.AddRange(ltotalSein);

            return lista;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<PrGrupoDTO> FillCabeceraCdispatchHistorico()
        {
            List<PrGrupoDTO> lista = new List<PrGrupoDTO>();
            List<PrGrupoDTO> listaTempFl = new List<PrGrupoDTO>();
            List<PrGrupoDTO> listaTempCmg = new List<PrGrupoDTO>();

            lista.Add(this.FillMeptomedicion(1, ConstantesMigraciones.Totalcogeneracion));
            lista.Add(this.FillMeptomedicion(2, ConstantesMigraciones.Totalrenov));
            lista.Add(this.FillMeptomedicion(3, ConstantesMigraciones.Generacioncoes));

            lista.Add(this.FillMeptomedicion(1, ConstantesMigraciones.Demandancp));
            lista.Add(this.FillMeptomedicion(2, ConstantesMigraciones.Gtotal));
            lista.Add(this.FillMeptomedicion(3, ConstantesMigraciones.Perdidasncp));
            lista.Add(this.FillMeptomedicion(4, ConstantesMigraciones.Deficitncp));
            lista.Add(this.FillMeptomedicion(5, ConstantesMigraciones.Demandacentroncp));

            listaTempFl.Add(this.FillMeptomedicion(1, ConstantesMigraciones.FlCentroSur));
            listaTempFl.Add(this.FillMeptomedicion(2, ConstantesMigraciones.FlChilcaCarapongo));
            listaTempFl.Add(this.FillMeptomedicion(3, ConstantesMigraciones.FlCarapongoCarabayllo));
            listaTempFl.Add(this.FillMeptomedicion(4, ConstantesMigraciones.FlChimboteTrujillo));
            listaTempFl.Add(this.FillMeptomedicion(5, ConstantesMigraciones.FlTrujilloNinia));
            listaTempFl.Add(this.FillMeptomedicion(6, ConstantesMigraciones.FlColcabambaPoroma));
            listaTempFl.Add(this.FillMeptomedicion(7, ConstantesMigraciones.FlChilcaPoroma));
            listaTempFl.Add(this.FillMeptomedicion(8, ConstantesMigraciones.FlPoromaYarabamba));
            listaTempFl.Add(this.FillMeptomedicion(9, ConstantesMigraciones.FlPoromaOconia));
            listaTempFl.Add(this.FillMeptomedicion(10, ConstantesMigraciones.FlYarabambaMontalvo));
            listaTempFl.Add(this.FillMeptomedicion(11, ConstantesMigraciones.FlHuancavelicaIndependencia));
            listaTempFl.Add(this.FillMeptomedicion(12, ConstantesMigraciones.FlChilcaSanJuan));
            listaTempFl.Add(this.FillMeptomedicion(13, ConstantesMigraciones.Flcotasoca));
            listaTempFl.Add(this.FillMeptomedicion(14, ConstantesMigraciones.Flchimbtruj));
            listaTempFl.Add(this.FillMeptomedicion(15, ConstantesMigraciones.Flsepanuchimb));
            listaTempFl.Add(this.FillMeptomedicion(16, ConstantesMigraciones.Flpomacochasjuan));
            listaTempFl.Add(this.FillMeptomedicion(17, ConstantesMigraciones.Fltrujguadalupe));
            listaTempFl.Add(this.FillMeptomedicion(18, ConstantesMigraciones.Flarmicota));
            listaTempFl.Add(this.FillMeptomedicion(19, ConstantesMigraciones.Floconsjose));
            listaTempFl.Add(this.FillMeptomedicion(20, ConstantesMigraciones.Flsjosemonta));
            listaTempFl.Add(this.FillMeptomedicion(21, ConstantesMigraciones.Fltinnvasoca));
            listaTempFl.Add(this.FillMeptomedicion(22, ConstantesMigraciones.Flsocamoque));
            listaTempFl.Add(this.FillMeptomedicion(23, ConstantesMigraciones.Flcarabchimb));
            listaTempFl.Add(this.FillMeptomedicion(24, ConstantesMigraciones.Flguadareque));
            listaTempFl.Add(this.FillMeptomedicion(25, ConstantesMigraciones.Flninapiura));
            listaTempFl.Add(this.FillMeptomedicion(26, ConstantesMigraciones.Flcarabchilca));
            listaTempFl.Add(this.FillMeptomedicion(27, ConstantesMigraciones.Flsjuanindus));
            listaTempFl.Add(this.FillMeptomedicion(28, ConstantesMigraciones.Flchilcaplani));
            listaTempFl.Add(this.FillMeptomedicion(29, ConstantesMigraciones.Flplanicarab));
            listaTempFl.Add(this.FillMeptomedicion(30, ConstantesMigraciones.Flventachav));
            listaTempFl.Add(this.FillMeptomedicion(31, ConstantesMigraciones.Flventazapa));
            listaTempFl.Add(this.FillMeptomedicion(32, ConstantesMigraciones.Flhuanzcarab));

            //>>>>>>>>>>>>>>>>>>>>Flujos de Potencia>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            var listaPuntosFlujos = FactorySic.GetMePtomedicionRepository().List(ConstantesAppServicio.ParametroDefecto, "2").Where(x => x.Tipoinfocodi == ConstantesAppServicio.TipoinfocodiFlujo).ToList();
            var listaPtosReporteFl = FactorySic.GetMeReporptomedRepository().GetByCriteria(ConstantesMigraciones.ReporteCdispatchFl, -1).OrderBy(x => x.Repptoorden).ToList();

            //si el punto existe en la lista temporal se saca el equinomb y gruponomb
            AgregarListaEQNombres(listaPuntosFlujos, listaTempFl, ref lista, ConstantesMigraciones.ReporteCdispatchFl);

            //si de la lista de puntos del reporte, alguno no existe en la lista final, entonces se agrega
            foreach (var pto in listaPtosReporteFl)
            {
                if (lista.Find(x => x.Ptomedicodi == pto.Ptomedicodi) == null)
                {
                    string cadena = string.Empty;
                    string[] words = { "9992", pto.Ptomedidesc.Trim(), pto.Ptomedicodi.ToString(), "8", "F0", "FLUJO EN LINEAS", pto.Equiabrev };
                    cadena = String.Join(",", words);

                    lista.Add(this.FillMeptomedicion(9999999, cadena));
                }
            }

            listaTempCmg.Add(this.FillMeptomedicion(1, ConstantesMigraciones.Cmgsein_ncp));
            listaTempCmg.Add(this.FillMeptomedicion(2, ConstantesMigraciones.Cmgstarosancp));
            listaTempCmg.Add(this.FillMeptomedicion(3, ConstantesMigraciones.Cmgtrujillo_ncp));
            listaTempCmg.Add(this.FillMeptomedicion(4, ConstantesMigraciones.Cmgsocabaya_ncp));
            //>>>>>>>>>>>>>>>>>>>>>>Costos Marginales>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            var listaPuntosCmg = FactorySic.GetMePtomedicionRepository().List(ConstantesAppServicio.ParametroDefecto, "2").Where(x => x.Tipoinfocodi == ConstantesAppServicio.TipoinfocodiDolar).ToList();
            var listaPtosReporteCmg = FactorySic.GetMeReporptomedRepository().GetByCriteria(ConstantesMigraciones.ReporteCdispatchCmg, -1).OrderBy(x => x.Repptoorden).ToList();

            //si el punto existe en la lista temporal se saca el equinomb y gruponomb
            AgregarListaEQNombres(listaPuntosCmg, listaTempCmg, ref lista, ConstantesMigraciones.ReporteCdispatchCmg);

            //si de la lista de puntos del reporte, alguno no existe en la lista final, entonces se agrega
            foreach (var pto in listaPtosReporteCmg)
            {
                if (lista.Find(x => x.Ptomedicodi == pto.Ptomedicodi) == null)
                {
                    string cadena = string.Empty;
                    string desc = pto.Ptomedidesc.Replace("_", " ").Trim();
                    desc = desc + "(US$/MWh)";
                    string[] words = { "9993", desc, pto.Ptomedicodi.ToString(), "21", "F2", "COSTOS MARGINALES", pto.Equiabrev };
                    cadena = String.Join(",", words);

                    lista.Add(this.FillMeptomedicion(9999999, cadena));
                }
            }

            lista.Add(this.FillMeptomedicion(1, ConstantesMigraciones.Niveld));

            lista.Add(this.FillMeptomedicion(1, ConstantesMigraciones.Escenario));
            lista.Add(this.FillMeptomedicion(2, ConstantesMigraciones.Cmgxmwh_sr));
            lista.Add(this.FillMeptomedicion(3, ConstantesMigraciones.Cmgxmwh_srideal));
            lista.Add(this.FillMeptomedicion(4, ConstantesMigraciones.Unidadmarginal));
            lista.Add(this.FillMeptomedicion(5, ConstantesMigraciones.Unidadmarginalideal));

            lista.Add(this.FillMeptomedicion(1, ConstantesMigraciones.Reservarotante));
            lista.Add(this.FillMeptomedicion(2, ConstantesMigraciones.Reservasec));
            lista.Add(this.FillMeptomedicion(3, ConstantesMigraciones.Reservafria));
            lista.Add(this.FillMeptomedicion(4, ConstantesMigraciones.Reservafriatermica));
            lista.Add(this.FillMeptomedicion(5, ConstantesMigraciones.Reservafriahidraulica));
            lista.Add(this.FillMeptomedicion(6, ConstantesMigraciones.ReservaEficiente));
            lista.Add(this.FillMeptomedicion(7, ConstantesMigraciones.ReservaEficienteGas));
            lista.Add(this.FillMeptomedicion(8, ConstantesMigraciones.ReservaEficienteCarbon));

            lista.Add(this.FillMeptomedicion(1, ConstantesMigraciones.Ntablachaca));

            lista.Add(this.FillMeptomedicion(1, ConstantesMigraciones.Demandasein));
            lista.Add(this.FillMeptomedicion(2, ConstantesMigraciones.Demandacentro));
            lista.Add(this.FillMeptomedicion(3, ConstantesMigraciones.Demandanorte));
            lista.Add(this.FillMeptomedicion(4, ConstantesMigraciones.Demandasur));
            lista.Add(this.FillMeptomedicion(5, ConstantesMigraciones.Demandaela));
            lista.Add(this.FillMeptomedicion(6, ConstantesMigraciones.Demandaecuador));
            lista.Add(this.FillMeptomedicion(7, ConstantesMigraciones.Demandacoes));

            //Agregar los puntos de medición de Hidrología, Flujos que no han sido registrados lineas arriba
            //if (ls_grupoabrev != null && li_tipoinfocodi > 0 && !H_Pr.ContainsKey(ls_grupoabrev) && li_tipoinfocodi > 0)
            List<MePtomedicionDTO> listaPto = FactorySic.GetMePtomedicionRepository().List(ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.OriglectcodiDespachomediahora.ToString())
                .Where(x => x.Tipoinfocodi > 0 && x.Grupocodi.GetValueOrDefault(0) <= 0).ToList();
            listaPto = listaPto.Where(x => !lista.Any(y => y.Ptomedicodi == x.Ptomedicodi)).ToList();
            listaPto = listaPto.Where(x => !lista.Any(y => ((y.Gruponomb) ?? string.Empty).Replace(" ", "").ToUpper().Trim() == ((x.Ptomedielenomb) ?? string.Empty).ToUpper().Trim())).ToList();

            foreach (var reg in listaPto)
            {
                PrGrupoDTO obj = new PrGrupoDTO()
                {
                    Gruponomb = reg.Ptomedidesc,
                    Ptomedicodi = reg.Ptomedicodi,
                    Tipoinfocodi = reg.Tipoinfocodi.Value,
                    Osicodi = reg.Osicodi,
                    Equinomb = reg.Equinomb,
                    Equiabrev = reg.Equiabrev,
                    Emprnomb = reg.Emprnomb,
                    Emprcodi = reg.Emprcodi,
                    Emprorden = 9999999 + 1,
                    Grupoorden = 9999999 + 1
                };

                lista.Add(obj);
            }

            return lista;
        }
        /// <summary>
        /// </summary>
        /// <param name="ordenGrupo"></param>
        /// <param name="cadena"></param>
        /// <returns></returns>
        private PrGrupoDTO FillMeptomedicion(int ordenGrupo, string cadena)
        {
            var arr = cadena.Split(',');
            string equiabrev_ = string.Empty;
            if (arr.Length > 7)
            {
                for (int z = 6; z < arr.Length; z++) { equiabrev_ += arr[z] + ","; }
                equiabrev_ = equiabrev_.Substring(0, equiabrev_.Length - 1);
            }
            else { equiabrev_ = arr[6]; }
            //string equiabrev_ = (arr.Length > 7 ? string.Join(",",)+ arr[6] + "," + arr[7] : arr[6]);

            int emprorden = 9999999 + 1;
            //setear orden
            switch (arr[5] ?? "")
            {
                case "GENERACION":
                    emprorden += 100;
                    break;

                case "SALIDAS NCP":
                case "SALIDAS YUPANA":
                    emprorden += 200;
                    break;

                case "FLUJO EN LINEAS":
                    emprorden += 300;
                    break;

                case "COSTOS MARGINALES":
                    emprorden += 400;
                    break;

                case "RESERVA":
                    emprorden += 500;
                    break;

                case "R":
                    emprorden += 600;
                    break;

                case "DEMANDA POR AREAS":
                    emprorden += 700;
                    break;
            }

            return new PrGrupoDTO()
            {
                Gruponomb = arr[1],
                GruponombWeb = arr[1],
                Ptomedicodi = int.Parse(arr[2]),
                Tipoinfocodi = int.Parse(arr[3]),
                Osicodi = arr[4],
                Equinomb = arr[5],
                Equiabrev = equiabrev_,
                Grupointegrante = ConstantesAppServicio.SI,
                Emprnomb = arr[5],
                Emprcodi = int.Parse(arr[0]),
                Grupoorden = ordenGrupo,
                Emprorden = emprorden
            };
        }
        /// <summary>
        /// método que obtiene el equinombre 
        /// </summary>
        /// <param name="listaPuntos"></param>
        /// <param name="listaTemporal"></param>
        /// <param name="lista"></param>
        /// <param name="tipo"></param>
        private void AgregarListaEQNombres(List<MePtomedicionDTO> listaPuntos, List<PrGrupoDTO> listaTemporal, ref List<PrGrupoDTO> lista, int tipo)
        {
            if (tipo == ConstantesMigraciones.ReporteCdispatchFl)
            {
                foreach (var ptoFlujos in listaPuntos)
                {
                    var ptoTemporal = listaTemporal.Find(x => x.Ptomedicodi == ptoFlujos.Ptomedicodi);
                    if (ptoTemporal != null)
                    {
                        ptoFlujos.Equinomb = ptoTemporal.Equinomb;
                        ptoFlujos.Gruponomb = ptoTemporal.Gruponomb;
                        ptoFlujos.Equiabrev = ptoTemporal.Equiabrev;
                    }
                    string cadena = string.Empty;
                    ptoFlujos.Equiabrev = ptoFlujos.Equiabrev == "0" || ptoFlujos.Equiabrev == null ? string.Empty : ptoFlujos.Equiabrev;
                    string[] words = { "9992", ptoFlujos.Ptomedidesc.Trim(), ptoFlujos.Ptomedicodi.ToString(), "8", "F0", "FLUJO EN LINEAS", ptoFlujos.Equiabrev };
                    cadena = String.Join(",", words);

                    lista.Add(this.FillMeptomedicion(9999999, cadena));
                }
            }
            if (tipo == ConstantesMigraciones.ReporteCdispatchCmg)
            {
                foreach (var ptoCmg in listaPuntos)
                {
                    var ptoTemporal = listaTemporal.Find(x => x.Ptomedicodi == ptoCmg.Ptomedicodi);
                    if (ptoTemporal != null)
                    {
                        ptoCmg.Equinomb = ptoTemporal.Equinomb;
                        ptoCmg.Gruponomb = ptoTemporal.Gruponomb;
                        ptoCmg.Equiabrev = ptoTemporal.Equiabrev;
                    }
                    string cadena = string.Empty;
                    ptoCmg.Equiabrev = ptoCmg.Equiabrev == "0" || ptoCmg.Equiabrev == null ? string.Empty : ptoCmg.Equiabrev;
                    string desc = ptoCmg.Ptomedidesc.Replace("_", " ").Trim();
                    desc = desc + "(US$/MWh)";
                    string[] words = { "9993", desc, ptoCmg.Ptomedicodi.ToString(), "21", "F2", "COSTOS MARGINALES", ptoCmg.Equiabrev };
                    cadena = String.Join(",", words);

                    lista.Add(this.FillMeptomedicion(9999999, cadena));
                }
            }
        }
        /// <summary>
        /// Lista de Flujos
        /// </summary>
        /// <param name="topcodi"></param>
        /// <param name="lPuntos"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="usuario"></param>
        /// <param name="lectcodi"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> ListaFlujosMcp(int topcodi, List<MePtomedicionDTO> lPuntos, DateTime fechaInicio, DateTime fechaFin, string usuario, int lectcodi)
        {
            List<MeMedicion48DTO> listaDespacho = new List<MeMedicion48DTO>();
            short idFlujo = ConstantesBase.SresFlujos;
            var listaFlujo = FactorySic.GetCpMedicion48Repository().ObtieneRegistrosToDespacho(topcodi, idFlujo, ConstantesAppServicio.OriglectcodiFlujos).Where(x => x.Medifecha >= fechaInicio && x.Medifecha <= fechaFin).ToList();

            List<MeMedicion48DTO> lista = new List<MeMedicion48DTO>();
            foreach (var regCalc in lPuntos)
            {
                var listaPunto = this.GetListaCalculadaM48Recursivo(listaFlujo, fechaInicio, fechaFin, regCalc.Ptomedicodi, lectcodi, usuario);
                listaDespacho.AddRange(listaPunto);
            }

            return listaDespacho;
        }
        /// <summary>
        /// Funcion recursiva que devuelve la data de un punto calculado
        /// </summary>
        /// <param name="listaFlujo"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="lectcodi"></param>
        /// <param name="pto"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> GetListaCalculadaM48Recursivo(List<CpMedicion48DTO> listaFlujo, DateTime fechaInicio, DateTime fechaFin, int pto, int lectcodi, string usuario)
        {
            List<MeMedicion48DTO> lista = new List<MeMedicion48DTO>();
            var allPtos = FactorySic.GetMeRelacionptoRepository().GetByCriteria(pto.ToString(), "-1");
            decimal? valor, valorAcum;
            bool considera = false;
            //Data de los puntos de medicion
            for (var fecha = fechaInicio; fecha <= fechaFin; fecha = fecha.AddDays(1))
            {
                considera = false;
                MeMedicion48DTO flujo = new MeMedicion48DTO();
                foreach (var regMed in allPtos)
                {
                    flujo.Ptomedicodi = pto;
                    flujo.Lectcodi = lectcodi;
                    flujo.Tipoinfocodi = ConstantesAppServicio.TipoinfocodiFlujo;
                    flujo.Medifecha = fecha;
                    flujo.Lastuser = usuario;
                    flujo.Lastdate = DateTime.Now;

                    var listaDataXPtoMedicionTmp = listaFlujo.Where(x => x.Ptomedicodi == regMed.Ptomedicodi2 && x.Medifecha == fecha);
                    decimal factor = (regMed.Relptofactor != null) ? (decimal)regMed.Relptofactor : 1;
                    foreach (var tmp in listaDataXPtoMedicionTmp)
                    {
                        considera = true;
                        //CAlcular Hs para registro reg
                        for (int i = 1; i <= 48; i++)
                        {
                            valor = (decimal?)tmp.GetType().GetProperty("H" + i.ToString()).GetValue(tmp, null);
                            valorAcum = (decimal?)flujo.GetType().GetProperty("H" + i.ToString()).GetValue(flujo, null);
                            if (valor != null)
                            {
                                if (valorAcum != null)
                                {
                                    valorAcum += factor * valor;
                                }
                                else
                                {
                                    valorAcum = factor * valor;
                                }
                            }
                            flujo.GetType().GetProperty("H" + i.ToString()).SetValue(flujo, valorAcum);
                        }
                    }
                }
                if (considera)
                {
                    lista.Add(flujo);
                }
            }
            return lista;
        }
        /// <summary>
        /// Completa el campo Meditotal
        /// </summary>
        /// <param name="lista"></param>
        public void CompletarTotalMeMecicion48(ref List<MeMedicion48DTO> lista)
        {
            decimal total = 0;
            decimal? valor;
            foreach (var reg in lista)
            {
                total = 0;
                for (int i = 1; i <= 48; i++)
                {
                    valor = (decimal?)reg.GetType().GetProperty("H" + i.ToString()).GetValue(reg, null);
                    total += (decimal)((valor != null) ? valor : 0);
                }
                reg.Meditotal = total;
            }

        }
        /// <summary>
        /// Lista de Costo Marginales
        /// </summary>
        /// <param name="topcodi"></param>
        /// <param name="lPuntos"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="usuario"></param>
        /// <param name="lectcodi"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> ListaCMarginalMcp(int topcodi, List<MePtomedicionDTO> lPuntos, DateTime fechaInicio, DateTime fechaFin, string usuario, int lectcodi)
        {
            List<MeMedicion48DTO> listaDespacho = new List<MeMedicion48DTO>();
            decimal? valor;
            short idFCmar = ConstantesBase.SresCostoMarginalBarra;
            var listaCmar = FactorySic.GetCpMedicion48Repository().ObtieneRegistrosToBarra(topcodi, idFCmar, ConstantesAppServicio.OriglectcodiDespachomediahora).Where(x => x.Medifecha >= fechaInicio && x.Medifecha <= fechaFin).ToList();

            List<MeMedicion48DTO> lista = new List<MeMedicion48DTO>();
            var listaFinal = listaCmar.Where(x => lPuntos.Any(y => y.Ptomedicodi == x.Ptomedicodi)).ToList();
            foreach (var regCalc in listaFinal)
            {
                MeMedicion48DTO reg = new MeMedicion48DTO();
                reg.Ptomedicodi = regCalc.Ptomedicodi;
                reg.Medifecha = regCalc.Medifecha;
                reg.Tipoinfocodi = ConstantesAppServicio.TipoinfocodiDolares;
                reg.Lectcodi = lectcodi;
                reg.Lastuser = usuario;
                for (var i = 1; i <= 48; i++)
                {
                    valor = (decimal?)regCalc.GetType().GetProperty("H" + i.ToString()).GetValue(regCalc, null);
                    reg.GetType().GetProperty("H" + i.ToString()).SetValue(reg, valor);
                }
                listaDespacho.Add(reg);
            }

            return listaDespacho;
        }
        /// <summary>
        /// Lista de Despacho Hidro
        /// </summary>
        /// <param name="topcodi"></param>
        /// <param name="usuario"></param>
        /// <param name="lectcodi"></param>
        /// <param name="listaDespachoXEq"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> ListaHidroMcp(int topcodi, string usuario, int lectcodi, out List<MeMedicion48DTO> listaDespachoXEq)
        {
            List<MeMedicion48DTO> listaDespacho = new List<MeMedicion48DTO>();
            listaDespachoXEq = new List<MeMedicion48DTO>();

            MeMedicion48DTO regdespacho;
            decimal? total, valor, valorParcial;
            short idPotHidro = ConstantesBase.SresPotHidro;
            var listaHidro = FactorySic.GetCpMedicion48Repository().ObtieneRegistrosPHToDespacho(topcodi, idPotHidro, ConstantesBase.IdOrigLectDespacho);

            foreach (var reg in listaHidro)
            {

                total = null;
                bool nuevoreg = false;

                //agrupar por punto de medición 
                regdespacho = listaDespacho.Find(x => x.Ptomedicodi == reg.Ptomedicodi && x.Lectcodi == lectcodi && x.Medifecha == reg.Medifecha);
                if (regdespacho == null)
                {
                    regdespacho = new MeMedicion48DTO();
                    nuevoreg = true;
                }
                for (int i = 1; i <= 48; i++)
                {
                    valor = (decimal?)reg.GetType().GetProperty("H" + i.ToString()).GetValue(reg, null);
                    if (!nuevoreg)
                    {
                        valorParcial = (decimal?)regdespacho.GetType().GetProperty("H" + i.ToString()).GetValue(regdespacho, null);
                        if (valorParcial != null)
                        {
                            valor = (valor == null) ? valorParcial : valor + valorParcial;
                        }
                    }
                    if (valor != null)
                    {
                        regdespacho.GetType().GetProperty("H" + i.ToString()).SetValue(regdespacho, valor);
                        if (total == null)
                            total = valor;
                        else
                            total += valor;
                    }
                }

                if (nuevoreg)
                {
                    regdespacho.Meditotal = total;
                    regdespacho.Medifecha = reg.Medifecha;
                    regdespacho.Ptomedicodi = reg.Ptomedicodi;
                    regdespacho.Tipoinfocodi = 1;
                    regdespacho.Lastuser = usuario;
                    regdespacho.Lastdate = DateTime.Now;
                    regdespacho.Lectcodi = lectcodi;
                    listaDespacho.Add(regdespacho);
                }

                //la planta hidráulica es un generador de CH
                if (reg.Recurfamsic == 2)
                {
                    regdespacho = new MeMedicion48DTO();
                    total = null;

                    for (int i = 1; i <= 48; i++)
                    {
                        valor = (decimal?)reg.GetType().GetProperty("H" + i.ToString()).GetValue(reg, null);
                        if (valor != null)
                        {
                            regdespacho.GetType().GetProperty("H" + i.ToString()).SetValue(regdespacho, valor);
                            if (total == null)
                                total = valor;
                            else
                                total += valor;
                        }
                    }

                    regdespacho.Meditotal = total;
                    regdespacho.Medifecha = reg.Medifecha;
                    regdespacho.Ptomedicodi = reg.Ptomedicodi;
                    regdespacho.Equicodi = reg.Recurcodisicoes;
                    regdespacho.Tipoinfocodi = 1;
                    regdespacho.Lastuser = usuario;
                    regdespacho.Lastdate = DateTime.Now;
                    regdespacho.Lectcodi = lectcodi;

                    listaDespachoXEq.Add(regdespacho);
                }
            }

            return listaDespacho;
        }
        /// <summary>
        /// Devuelve Lista de Termicas de Yupana en MeMedicion48
        /// </summary>
        /// <param name="topcodi"></param>
        /// <param name="usuario"></param>
        /// <param name="lectcodi"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> ListaTermicaMcp(int topcodi, string usuario, int lectcodi)
        {
            List<MeMedicion48DTO> listaDespacho = new List<MeMedicion48DTO>();
            MeMedicion48DTO regdespacho;
            decimal? total, valor;
            short idPotTermica = ConstantesBase.SresPotTermica;

            var listaTermica1 = FactorySic.GetCpMedicion48Repository().ObtieneRegistrosToDespachoPTermica1(topcodi, idPotTermica);
            foreach (var reg in listaTermica1)
            {
                regdespacho = new MeMedicion48DTO();
                total = null;
                for (int i = 1; i <= 48; i++)
                {
                    valor = (decimal?)reg.GetType().GetProperty("H" + i.ToString()).GetValue(reg, null);
                    if (valor != null)
                    {
                        regdespacho.GetType().GetProperty("H" + i.ToString()).SetValue(regdespacho, valor);
                        if (total == null)
                            total = valor;
                        else
                            total += valor;
                    }
                }
                regdespacho.Meditotal = total;
                regdespacho.Medifecha = reg.Medifecha;
                regdespacho.Ptomedicodi = reg.Ptomedicodi;
                regdespacho.Tipoinfocodi = 1;
                regdespacho.Lastuser = usuario;
                regdespacho.Lastdate = DateTime.Now;
                regdespacho.Lectcodi = lectcodi;
                listaDespacho.Add(regdespacho);
            }
            return listaDespacho;
        }
        /// <summary>
        /// Obtiene Termica de Ciclo Combiana de Yupana en MeMedicion48
        /// </summary>
        /// <param name="topcodi"></param>
        /// <param name="fechaini"></param>
        /// <param name="fechafin"></param>
        /// <param name="usuario"></param>
        /// <param name="lectcodi"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> ListaTermicasCCMcp(int topcodi, DateTime fechaini, DateTime fechafin, string usuario, int lectcodi)
        {
            List<MeMedicion48DTO> listaDespacho = new List<MeMedicion48DTO>();
            short idPotTermica = ConstantesBase.SresPotTermica;

            var listaTermica2 = FactorySic.GetCpMedicion48Repository().ObtieneRegistrosToDespachoPTermica2(topcodi, idPotTermica);
            listaTermica2 = listaTermica2.Where(x => x.Medifecha >= fechaini && x.Medifecha <= fechafin).ToList();
            var listaCentral = listaTermica2.Where(x => x.Ptomedicodi == ConstantesBase.IdPtoFenixTv).ToList();
            listaDespacho = ActualizarDespachoFenix(listaCentral, lectcodi, fechaini, fechafin, usuario);

            listaCentral = listaTermica2.Where(x => x.Ptomedicodi == ConstantesBase.IdPtoKallpaTv).ToList();
            var lkallpa = ActualizarDespachoKallpa(listaCentral, lectcodi, fechaini, fechafin, usuario);

            listaCentral = listaTermica2.Where(x => x.Ptomedicodi == ConstantesBase.IdPtoVentanillaTv).ToList();
            var lVentanilla = ActualizarDespachoVentanilla(listaCentral, lectcodi, fechaini, fechafin, usuario);

            listaCentral = listaTermica2.Where(x => x.Ptomedicodi == ConstantesBase.IdPtoChilca1Tv).ToList();
            var lChila1 = ActualizarDespachoChica1(listaCentral, lectcodi, fechaini, fechafin, usuario);

            listaCentral = listaTermica2.Where(x => x.Ptomedicodi == ConstantesBase.IdPtoChilca2Tv).ToList();
            var lChilca2 = ActualizarDespachoChica2(listaCentral, lectcodi, fechaini, fechafin, usuario);

            listaCentral = listaTermica2.Where(x => x.Ptomedicodi == ConstantesBase.IdPtoOllerosTv).ToList();
            var lOlleros = ActualizarDespachoOlleros(listaCentral, lectcodi, fechaini, fechafin, usuario);

            listaCentral = listaTermica2.Where(x => x.Ptomedicodi == ConstantesBase.IdPtoLasFloresTv).ToList();
            var lLasFlores = ActualizarDespachoLasFlores(listaCentral, lectcodi, fechaini, fechafin, usuario);

            listaDespacho = listaDespacho.Union(lkallpa).ToList();
            listaDespacho = listaDespacho.Union(lVentanilla).ToList();
            listaDespacho = listaDespacho.Union(lChila1).ToList();
            listaDespacho = listaDespacho.Union(lChilca2).ToList();
            listaDespacho = listaDespacho.Union(lOlleros).ToList();
            listaDespacho = listaDespacho.Union(lLasFlores).ToList();

            return listaDespacho;
        }
        /// <summary>
        /// Despacho Olleros
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="lectcodi"></param>
        /// <param name="fechaini"></param>
        /// <param name="fechafin"></param>
        /// <param name="usuario"></param>
        public List<MeMedicion48DTO> ActualizarDespachoOlleros(List<CpMedicion48DTO> lista, int lectcodi, DateTime fechaini, DateTime fechafin, string usuario)
        {
            MeMedicion48DTO regdespacho, regdespachotv;
            List<MeMedicion48DTO> listaDespacho = new List<MeMedicion48DTO>();
            for (var f = fechaini; f <= fechafin; f = f.AddDays(1))
            {

                regdespacho = new MeMedicion48DTO(); // Tg1
                regdespacho.Medifecha = f;
                regdespacho.Ptomedicodi = ConstantesBase.IdPtoOllerosTg1;
                regdespacho.Tipoinfocodi = 1;
                regdespacho.Lastuser = usuario;
                regdespacho.Lastdate = DateTime.Now;
                regdespacho.Lectcodi = lectcodi;
                listaDespacho.Add(regdespacho);

                regdespachotv = new MeMedicion48DTO(); // Tv
                regdespachotv.Medifecha = f;
                regdespachotv.Ptomedicodi = ConstantesBase.IdPtoOllerosTv;
                regdespachotv.Tipoinfocodi = 1;
                regdespachotv.Lastuser = usuario;
                regdespachotv.Lastdate = DateTime.Now;
                regdespachotv.Lectcodi = lectcodi;
                listaDespacho.Add(regdespachotv);

            }
            foreach (var reg in lista)
            {
                switch (reg.Grupocodi)
                {
                    case ConstantesBase.IdOllerosCC:
                        DistribuyeModoAGrupos(ref listaDespacho, reg, usuario, lectcodi, ConstantesBase.IdPtoOllerosTg1, ConstantesBase.IdPtoOllerosTv);
                        break;
                }
            }
            return listaDespacho;
        }
        /// <summary>
        /// Despacho Las Flores
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="lectcodi"></param>
        /// <param name="fechaini"></param>
        /// <param name="fechafin"></param>
        /// <param name="usuario"></param>
        public List<MeMedicion48DTO> ActualizarDespachoLasFlores(List<CpMedicion48DTO> lista, int lectcodi, DateTime fechaini, DateTime fechafin, string usuario)
        {
            MeMedicion48DTO regdespacho, regdespachotv;
            List<MeMedicion48DTO> listaDespacho = new List<MeMedicion48DTO>();
            for (var f = fechaini; f <= fechafin; f = f.AddDays(1))
            {

                regdespacho = new MeMedicion48DTO(); // Tg1
                regdespacho.Medifecha = f;
                regdespacho.Ptomedicodi = ConstantesBase.IdPtoLasFloresTg1;
                regdespacho.Tipoinfocodi = 1;
                regdespacho.Lastuser = usuario;
                regdespacho.Lastdate = DateTime.Now;
                regdespacho.Lectcodi = lectcodi;
                listaDespacho.Add(regdespacho);

                regdespachotv = new MeMedicion48DTO(); // Tv
                regdespachotv.Medifecha = f;
                regdespachotv.Ptomedicodi = ConstantesBase.IdPtoLasFloresTv;
                regdespachotv.Tipoinfocodi = 1;
                regdespachotv.Lastuser = usuario;
                regdespachotv.Lastdate = DateTime.Now;
                regdespachotv.Lectcodi = lectcodi;
                listaDespacho.Add(regdespachotv);

            }
            foreach (var reg in lista)
            {
                switch (reg.Grupocodi)
                {
                    case ConstantesBase.IdLasFloresCC:
                        DistribuyeModoAGrupos(ref listaDespacho, reg, usuario, lectcodi, ConstantesBase.IdPtoLasFloresTg1, ConstantesBase.IdPtoLasFloresTv);
                        break;
                }
            }
            return listaDespacho;
        }
        /// <summary>
        /// Despacho Fenix
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="lectcodi"></param>
        /// <param name="fechaini"></param>
        /// <param name="fechafin"></param>
        /// <param name="usuario"></param>
        public List<MeMedicion48DTO> ActualizarDespachoFenix(List<CpMedicion48DTO> lista, int lectcodi, DateTime fechaini, DateTime fechafin, string usuario)
        {
            MeMedicion48DTO regdespacho, regdespachotg2, regdespachotv;
            List<MeMedicion48DTO> listaDespacho = new List<MeMedicion48DTO>();
            decimal? total, total2, totaltv, valor, x6;
            for (var f = fechaini; f <= fechafin; f = f.AddDays(1))
            {

                regdespacho = new MeMedicion48DTO(); // Tg1
                regdespacho.Medifecha = f;
                regdespacho.Ptomedicodi = ConstantesBase.IdPtoFenixTg1;
                regdespacho.Tipoinfocodi = 1;
                regdespacho.Lastuser = usuario;
                regdespacho.Lastdate = DateTime.Now;
                regdespacho.Lectcodi = lectcodi;
                listaDespacho.Add(regdespacho);

                regdespachotg2 = new MeMedicion48DTO(); // Tg2
                regdespachotg2.Medifecha = f;
                regdespachotg2.Ptomedicodi = ConstantesBase.IdPtoFenixTg2;
                regdespachotg2.Tipoinfocodi = 1;
                regdespachotg2.Lastuser = usuario;
                regdespachotg2.Lastdate = DateTime.Now;
                regdespachotg2.Lectcodi = lectcodi;
                listaDespacho.Add(regdespachotg2);

                regdespachotv = new MeMedicion48DTO(); // Tv
                regdespachotv.Medifecha = f;
                regdespachotv.Ptomedicodi = ConstantesBase.IdPtoFenixTv;
                regdespachotv.Tipoinfocodi = 1;
                regdespachotv.Lastuser = usuario;
                regdespachotv.Lastdate = DateTime.Now;
                regdespachotv.Lectcodi = lectcodi;
                listaDespacho.Add(regdespachotv);

            }
            foreach (var reg in lista)
            {

                total = null;
                totaltv = null;
                total2 = null;
                switch (reg.Grupocodi)
                {
                    case ConstantesBase.IdFenixCCTg1:
                        DistribuyeModoAGrupos(ref listaDespacho, reg, usuario, lectcodi, ConstantesBase.IdPtoFenixTg1, ConstantesBase.IdPtoFenixTv);
                        break;
                    case ConstantesBase.IdFenixCCTg2:
                        DistribuyeModoAGrupos(ref listaDespacho, reg, usuario, lectcodi, ConstantesBase.IdPtoFenixTg2, ConstantesBase.IdPtoFenixTv);
                        break;
                    case ConstantesBase.IdFenixCCTg12:
                        regdespacho = listaDespacho.Find(x => x.Ptomedicodi == ConstantesBase.IdPtoFenixTg1 && x.Medifecha == reg.Medifecha);//new MeMedicion48DTO(); // Tg1
                        regdespachotg2 = listaDespacho.Find(x => x.Ptomedicodi == ConstantesBase.IdPtoFenixTg2 && x.Medifecha == reg.Medifecha);//new MeMedicion48DTO(); // Tg2
                        regdespachotv = listaDespacho.Find(x => x.Ptomedicodi == ConstantesBase.IdPtoFenixTv && x.Medifecha == reg.Medifecha);//new MeMedicion48DTO(); // Tv

                        decimal valorAnt = 0, valorAnt2 = 0, valorAnt3 = 0;
                        for (int i = 1; i <= 48; i++)
                        {
                            valor = (decimal?)reg.GetType().GetProperty("H" + i.ToString()).GetValue(reg, null);
                            if (valor != null)
                                if (valor != 0)
                                {
                                    if (valor > 510)
                                        x6 = 170;
                                    else
                                    {
                                        if (valor > 330 && valor < 265)
                                            x6 = 90;
                                        else
                                            x6 = valor / 3;
                                    }
                                    var valorReg = regdespacho.GetType().GetProperty("H" + i.ToString()).GetValue(regdespacho, null);
                                    if (valorReg != null)
                                    {
                                        valorAnt = (decimal)valorReg;
                                    }
                                    regdespacho.GetType().GetProperty("H" + i.ToString()).SetValue(regdespacho, valorAnt + x6);

                                    valorReg = regdespachotg2.GetType().GetProperty("H" + i.ToString()).GetValue(regdespachotg2, null);
                                    if (valorReg != null)
                                    {
                                        valorAnt2 = (decimal)valorReg;
                                    }
                                    regdespachotg2.GetType().GetProperty("H" + i.ToString()).SetValue(regdespachotg2, valorAnt2 + x6);

                                    valorReg = regdespachotv.GetType().GetProperty("H" + i.ToString()).GetValue(regdespachotv, null);
                                    if (valorReg != null)
                                    {
                                        valorAnt3 = (decimal)valorReg;
                                    }
                                    regdespachotv.GetType().GetProperty("H" + i.ToString()).SetValue(regdespachotv, valorAnt3 + valor - 2 * x6);
                                    if (total == null)
                                        total = x6;
                                    else
                                        total += valorAnt + x6;
                                    if (total2 == null)
                                        total2 = x6;
                                    else
                                        total2 += valorAnt2 + x6;

                                    if (totaltv == null)
                                        totaltv = valor - 2 * x6;
                                    else
                                        totaltv += valorAnt + valor - 2 * x6;
                                }
                        }
                        if (total != null)
                            regdespacho.Meditotal = total;
                        regdespachotg2.Meditotal = total;
                        if (totaltv != null)
                            regdespachotv.Meditotal = totaltv;

                        break;
                }
            }
            return listaDespacho;
        }

        /// <summary>
        /// Despacho Kallpa
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="lectcodi"></param>
        /// <param name="fechaini"></param>
        /// <param name="fechafin"></param>
        /// <param name="usuario"></param>
        public List<MeMedicion48DTO> ActualizarDespachoKallpa(List<CpMedicion48DTO> lista, int lectcodi, DateTime fechaini, DateTime fechafin, string usuario)
        {
            List<MeMedicion48DTO> listaDespacho = new List<MeMedicion48DTO>();
            MeMedicion48DTO regdespacho, regdespachotg2, regdespachotg3, regdespachotv;
            for (var f = fechaini; f <= fechafin; f = f.AddDays(1))
            {
                regdespacho = new MeMedicion48DTO(); // Tg1
                regdespacho.Medifecha = f;
                regdespacho.Ptomedicodi = ConstantesBase.IdPtoKallpaTg1;
                regdespacho.Tipoinfocodi = 1;
                regdespacho.Lastuser = usuario;
                regdespacho.Lastdate = DateTime.Now;
                regdespacho.Lectcodi = lectcodi;
                listaDespacho.Add(regdespacho);

                regdespachotg2 = new MeMedicion48DTO(); // Tg2
                regdespachotg2.Medifecha = f;
                regdespachotg2.Ptomedicodi = ConstantesBase.IdPtoKallpaTg2;
                regdespachotg2.Tipoinfocodi = 1;
                regdespachotg2.Lastuser = usuario;
                regdespachotg2.Lastdate = DateTime.Now;
                regdespachotg2.Lectcodi = lectcodi;
                listaDespacho.Add(regdespachotg2);

                regdespachotg3 = new MeMedicion48DTO(); // Tg3
                regdespachotg3.Medifecha = f;
                regdespachotg3.Ptomedicodi = ConstantesBase.IdPtoKallpaTg3;
                regdespachotg3.Tipoinfocodi = 1;
                regdespachotg3.Lastuser = usuario;
                regdespachotg3.Lastdate = DateTime.Now;
                regdespachotg3.Lectcodi = lectcodi;
                listaDespacho.Add(regdespachotg3);

                regdespachotv = new MeMedicion48DTO(); // Tv
                regdespachotv.Medifecha = f;
                regdespachotv.Ptomedicodi = ConstantesBase.IdPtoKallpaTv;
                regdespachotv.Tipoinfocodi = 1;
                regdespachotv.Lastuser = usuario;
                regdespachotv.Lastdate = DateTime.Now;
                regdespachotv.Lectcodi = lectcodi;
                listaDespacho.Add(regdespachotv);
            }

            foreach (var reg in lista)
            {
                switch (reg.Grupocodi)
                {
                    case ConstantesBase.IdKallpaCCTg1:
                        DistribuyeModoAGrupos(ref listaDespacho, reg, usuario, lectcodi, ConstantesBase.IdPtoKallpaTg1, ConstantesBase.IdPtoKallpaTv);
                        break;
                    case ConstantesBase.IdKallpaCCTg2:
                        DistribuyeModoAGrupos(ref listaDespacho, reg, usuario, lectcodi, ConstantesBase.IdPtoKallpaTg2, ConstantesBase.IdPtoKallpaTv);
                        break;
                    case ConstantesBase.IdKallpaCCTg3:
                        DistribuyeModoAGrupos(ref listaDespacho, reg, usuario, lectcodi, ConstantesBase.IdPtoKallpaTg3, ConstantesBase.IdPtoKallpaTv);
                        break;
                    case ConstantesBase.IdKallpaCCTg12:
                        DistribuyeModoAGrupos(ref listaDespacho, reg, usuario, lectcodi, ConstantesBase.IdPtoKallpaTg1, ConstantesBase.IdPtoKallpaTg2, ConstantesBase.IdPtoKallpaTv);
                        break;
                    case ConstantesBase.IdKallpaCCTg23:
                        DistribuyeModoAGrupos(ref listaDespacho, reg, usuario, lectcodi, ConstantesBase.IdPtoKallpaTg2, ConstantesBase.IdPtoKallpaTg3, ConstantesBase.IdPtoKallpaTv);
                        break;
                    case ConstantesBase.IdKallpaCCTg31:
                        DistribuyeModoAGrupos(ref listaDespacho, reg, usuario, lectcodi, ConstantesBase.IdPtoKallpaTg1, ConstantesBase.IdPtoKallpaTg3, ConstantesBase.IdPtoKallpaTv);
                        break;
                    case ConstantesBase.IdKallpaCCTg123:
                        DistribuyeModoAGrupos(ref listaDespacho, reg, usuario, lectcodi, ConstantesBase.IdPtoKallpaTg1, ConstantesBase.IdPtoKallpaTg2, ConstantesBase.IdPtoKallpaTg3, ConstantesBase.IdPtoKallpaTv);
                        break;

                }
            }
            return listaDespacho;
        }

        /// <summary>
        /// Despacho Ventanilla
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="lectcodi"></param>
        /// <param name="fechaini"></param>
        /// <param name="fechafin"></param>
        /// <param name="usuario"></param>
        public List<MeMedicion48DTO> ActualizarDespachoVentanilla(List<CpMedicion48DTO> lista, int lectcodi, DateTime fechaini, DateTime fechafin, string usuario)
        {
            List<MeMedicion48DTO> listaDespacho = new List<MeMedicion48DTO>();
            MeMedicion48DTO regdespacho, regdespachotg2, regdespachotv;
            for (var f = fechaini; f <= fechafin; f = f.AddDays(1))
            {

                regdespacho = new MeMedicion48DTO(); // Tg3
                regdespacho.Medifecha = f;
                regdespacho.Ptomedicodi = ConstantesBase.IdPtoVentanillaTg3;
                regdespacho.Tipoinfocodi = 1;
                regdespacho.Lastuser = usuario;
                regdespacho.Lastdate = DateTime.Now;
                regdespacho.Lectcodi = lectcodi;
                listaDespacho.Add(regdespacho);

                regdespachotg2 = new MeMedicion48DTO(); // Tg4
                regdespachotg2.Medifecha = f;
                regdespachotg2.Ptomedicodi = ConstantesBase.IdPtoVentanillaTg4;
                regdespachotg2.Tipoinfocodi = 1;
                regdespachotg2.Lastuser = usuario;
                regdespachotg2.Lastdate = DateTime.Now;
                regdespachotg2.Lectcodi = lectcodi;
                listaDespacho.Add(regdespachotg2);

                regdespachotv = new MeMedicion48DTO(); // Tv
                regdespachotv.Medifecha = f;
                regdespachotv.Ptomedicodi = ConstantesBase.IdPtoVentanillaTv;
                regdespachotv.Tipoinfocodi = 1;
                regdespachotv.Lastuser = usuario;
                regdespachotv.Lastdate = DateTime.Now;
                regdespachotv.Lectcodi = lectcodi;
                listaDespacho.Add(regdespachotv);

            }
            foreach (var reg in lista)
            {
                switch (reg.Grupocodi)
                {
                    case ConstantesBase.IdVentanillaCCTg3:
                    case ConstantesBase.IdVentanillaCCTg3FD:
                        DistribuyeModoAGrupos(ref listaDespacho, reg, usuario, lectcodi, ConstantesBase.IdPtoVentanillaTg3, ConstantesBase.IdPtoVentanillaTv);
                        break;
                    case ConstantesBase.IdVentanillaCCTg4:
                    case ConstantesBase.IdVentanillaCCTg4FD:
                        DistribuyeModoAGrupos(ref listaDespacho, reg, usuario, lectcodi, ConstantesBase.IdPtoVentanillaTg4, ConstantesBase.IdPtoVentanillaTv);
                        break;
                    case ConstantesBase.IdVentanillaCCTg34:
                    case ConstantesBase.IdVentanillaCCTg34FD:
                        DistribuyeModoAGrupos(ref listaDespacho, reg, usuario, lectcodi, ConstantesBase.IdPtoVentanillaTg3, ConstantesBase.IdPtoVentanillaTg4, ConstantesBase.IdPtoVentanillaTv);
                        break;

                }
            }
            return listaDespacho;
        }
        /// <summary>
        /// Actualiza despacho chilca 1
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="lectcodi"></param>
        /// <param name="fechaini"></param>
        /// <param name="fechafin"></param>
        /// <param name="usuario"></param>
        public List<MeMedicion48DTO> ActualizarDespachoChica1(List<CpMedicion48DTO> lista, int lectcodi, DateTime fechaini, DateTime fechafin, string usuario)
        {
            List<MeMedicion48DTO> listaDespacho = new List<MeMedicion48DTO>();
            MeMedicion48DTO regdespacho, regdespachotg2, regdespachotg3, regdespachotv;
            for (var f = fechaini; f <= fechafin; f = f.AddDays(1))
            {

                regdespacho = new MeMedicion48DTO(); // Tg1
                regdespacho.Medifecha = f;
                regdespacho.Ptomedicodi = ConstantesBase.IdPtoChilca1Tg1;
                regdespacho.Tipoinfocodi = 1;
                regdespacho.Lastuser = usuario;
                regdespacho.Lastdate = DateTime.Now;
                regdespacho.Lectcodi = lectcodi;
                listaDespacho.Add(regdespacho);

                regdespachotg2 = new MeMedicion48DTO(); // Tg2
                regdespachotg2.Medifecha = f;
                regdespachotg2.Ptomedicodi = ConstantesBase.IdPtoChilca1Tg2;
                regdespachotg2.Tipoinfocodi = 1;
                regdespachotg2.Lastuser = usuario;
                regdespachotg2.Lastdate = DateTime.Now;
                regdespachotg2.Lectcodi = lectcodi;
                listaDespacho.Add(regdespachotg2);

                regdespachotg3 = new MeMedicion48DTO(); // Tg3
                regdespachotg3.Medifecha = f;
                regdespachotg3.Ptomedicodi = ConstantesBase.IdPtoChilca1Tg3;
                regdespachotg3.Tipoinfocodi = 1;
                regdespachotg3.Lastuser = usuario;
                regdespachotg3.Lastdate = DateTime.Now;
                regdespachotg3.Lectcodi = lectcodi;
                listaDespacho.Add(regdespachotg3);

                regdespachotv = new MeMedicion48DTO(); // Tv
                regdespachotv.Medifecha = f;
                regdespachotv.Ptomedicodi = ConstantesBase.IdPtoChilca1Tv;
                regdespachotv.Tipoinfocodi = 1;
                regdespachotv.Lastuser = usuario;
                regdespachotv.Lastdate = DateTime.Now;
                regdespachotv.Lectcodi = lectcodi;
                listaDespacho.Add(regdespachotv);

            }
            foreach (var reg in lista)
            {
                switch (reg.Grupocodi)
                {
                    case ConstantesBase.IdChilca1CCTg1:
                        DistribuyeModoAGrupos(ref listaDespacho, reg, usuario, lectcodi, ConstantesBase.IdPtoChilca1Tg1, ConstantesBase.IdPtoChilca1Tv);
                        break;
                    case ConstantesBase.IdChilca1CCTg2:
                        DistribuyeModoAGrupos(ref listaDespacho, reg, usuario, lectcodi, ConstantesBase.IdPtoChilca1Tg2, ConstantesBase.IdPtoChilca1Tv);
                        break;
                    case ConstantesBase.IdChilca1CCTg3:
                        DistribuyeModoAGrupos(ref listaDespacho, reg, usuario, lectcodi, ConstantesBase.IdPtoChilca1Tg3, ConstantesBase.IdPtoChilca1Tv);
                        break;
                    case ConstantesBase.IdChilca1CCTg12:
                        DistribuyeModoAGrupos(ref listaDespacho, reg, usuario, lectcodi, ConstantesBase.IdPtoChilca1Tg1, ConstantesBase.IdPtoChilca1Tg2, ConstantesBase.IdPtoChilca1Tv);
                        break;
                    case ConstantesBase.IdChilca1CCTg23:
                        DistribuyeModoAGrupos(ref listaDespacho, reg, usuario, lectcodi, ConstantesBase.IdPtoChilca1Tg2, ConstantesBase.IdPtoChilca1Tg3, ConstantesBase.IdPtoChilca1Tv);
                        break;
                    case ConstantesBase.IdChilca1CCTg31:
                        DistribuyeModoAGrupos(ref listaDespacho, reg, usuario, lectcodi, ConstantesBase.IdPtoChilca1Tg1, ConstantesBase.IdPtoChilca1Tg3, ConstantesBase.IdPtoChilca1Tv);
                        break;
                    case ConstantesBase.IdChilca1CCTg123:
                        DistribuyeModoAGrupos(ref listaDespacho, reg, usuario, lectcodi, ConstantesBase.IdPtoChilca1Tg1, ConstantesBase.IdPtoChilca1Tg2, ConstantesBase.IdPtoChilca1Tg3, ConstantesBase.IdPtoChilca1Tv);
                        break;

                }
            }
            return listaDespacho;
        }
        /// <summary>
        /// Actualiza despacho chilca 2
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="lectcodi"></param>
        /// <param name="fechaini"></param>
        /// <param name="fechafin"></param>
        /// <param name="usuario"></param>
        public List<MeMedicion48DTO> ActualizarDespachoChica2(List<CpMedicion48DTO> lista, int lectcodi, DateTime fechaini, DateTime fechafin, string usuario)
        {
            List<MeMedicion48DTO> listaDespacho = new List<MeMedicion48DTO>();
            MeMedicion48DTO regdespacho, regdespachotv;
            for (var f = fechaini; f <= fechafin; f = f.AddDays(1))
            {
                regdespacho = new MeMedicion48DTO(); // Tg4
                regdespacho.Medifecha = f;
                regdespacho.Ptomedicodi = ConstantesBase.IdPtoChilca2Tg4;
                regdespacho.Tipoinfocodi = 1;
                regdespacho.Lastuser = usuario;
                regdespacho.Lastdate = DateTime.Now;
                regdespacho.Lectcodi = lectcodi;
                listaDespacho.Add(regdespacho);

                regdespachotv = new MeMedicion48DTO(); // Tv
                regdespachotv.Medifecha = f;
                regdespachotv.Ptomedicodi = ConstantesBase.IdPtoChilca2Tv;
                regdespachotv.Tipoinfocodi = 1;
                regdespachotv.Lastuser = usuario;
                regdespachotv.Lastdate = DateTime.Now;
                regdespachotv.Lectcodi = lectcodi;
                listaDespacho.Add(regdespachotv);
            }
            foreach (var reg in lista)
            {
                switch (reg.Grupocodi)
                {
                    case ConstantesBase.IdChilca2CCTg4:
                        DistribuyeModoAGrupos(ref listaDespacho, reg, usuario, lectcodi, ConstantesBase.IdPtoChilca2Tg4, ConstantesBase.IdPtoChilca2Tv);
                        break;
                }
            }
            return listaDespacho;
        }
        /// <summary>
        /// Distribuye la potencia del modo de operacion entre una Tg y La Tv
        /// </summary>
        /// <param name="listaDespacho"></param>
        /// <param name="registro"></param>
        /// <param name="usuario"></param>
        /// <param name="lectcodi"></param>
        /// <param name="ptoTg1"></param>
        /// <param name="ptoTv"></param>
        public void DistribuyeModoAGrupos(ref List<MeMedicion48DTO> listaDespacho, CpMedicion48DTO registro, string usuario, int lectcodi, int ptoTg1, int ptoTv)
        {
            MeMedicion48DTO regdespacho, regdespachotv;
            decimal? total = null, totaltv = null, valor;
            regdespacho = listaDespacho.Find(x => x.Ptomedicodi == ptoTg1 && x.Medifecha == registro.Medifecha); // Tg1
            regdespachotv = listaDespacho.Find(x => x.Ptomedicodi == ptoTv && x.Medifecha == registro.Medifecha); // Tv
            decimal valorAnt = 0, valorAnt2 = 0;
            for (int i = 1; i <= 48; i++)
            {
                valor = (decimal?)registro.GetType().GetProperty("H" + i.ToString()).GetValue(registro, null);
                if (valor != null)
                    if (valor != 0)
                    {
                        var valorReg = regdespacho.GetType().GetProperty("H" + i.ToString()).GetValue(regdespacho, null);
                        if (valorReg != null)
                        {
                            valorAnt = (decimal)valorReg;
                        }
                        regdespacho.GetType().GetProperty("H" + i.ToString()).SetValue(regdespacho, valorAnt + 2 * valor / 3);
                        valorReg = regdespachotv.GetType().GetProperty("H" + i.ToString()).GetValue(regdespachotv, null);
                        if (valorReg != null)
                        {
                            valorAnt2 = (decimal)valorReg;
                        }
                        regdespachotv.GetType().GetProperty("H" + i.ToString()).SetValue(regdespachotv, valorAnt2 + valor / 3);
                        if (total == null)
                            total = 2 * valor / 3;
                        else
                            total += valorAnt + 2 * valor / 3;
                        if (totaltv == null)
                            totaltv = valor / 3;
                        else
                            totaltv += valorAnt2 + valor / 3;
                    }
            }
            if (total != null)
                regdespacho.Meditotal = total;
            if (totaltv != null)
                regdespachotv.Meditotal = totaltv;
        }
        /// <summary>
        /// Distribuye la potencia del modo de operacion entre una Tg y La Tv
        /// </summary>
        /// <param name="listaDespacho"></param>
        /// <param name="registro"></param>
        /// <param name="usuario"></param>
        /// <param name="lectcodi"></param>
        /// <param name="ptoTg1"></param>
        /// <param name="ptoTg2"></param>
        /// <param name="ptoTv"></param>
        public void DistribuyeModoAGrupos(ref List<MeMedicion48DTO> listaDespacho, CpMedicion48DTO registro, string usuario, int lectcodi, int ptoTg1, int ptoTg2, int ptoTv)
        {
            MeMedicion48DTO regdespacho, regdespachotg2, regdespachotv;
            decimal? total = null, total2 = null, totaltv = null, valor;
            decimal valorAnt = 0, valorAnt2 = 0, valorAnt3 = 0;
            regdespacho = listaDespacho.Find(x => x.Ptomedicodi == ptoTg1 && x.Medifecha == registro.Medifecha); // new MeMedicion48DTO(); // Tg1
            regdespachotg2 = listaDespacho.Find(x => x.Ptomedicodi == ptoTg2 && x.Medifecha == registro.Medifecha); // new MeMedicion48DTO(); // Tg2
            regdespachotv = listaDespacho.Find(x => x.Ptomedicodi == ptoTv && x.Medifecha == registro.Medifecha); // new MeMedicion48DTO(); // Tv

            for (int i = 1; i <= 48; i++)
            {
                valor = (decimal?)registro.GetType().GetProperty("H" + i.ToString()).GetValue(registro, null);
                if (valor != null)
                    if (valor != 0)
                    {
                        var valorReg = regdespacho.GetType().GetProperty("H" + i.ToString()).GetValue(regdespacho, null);
                        if (valorReg != null)
                        {
                            valorAnt = (decimal)valorReg;
                        }
                        regdespacho.GetType().GetProperty("H" + i.ToString()).SetValue(regdespacho, valorAnt + 1 * valor / 3);
                        valorReg = regdespachotg2.GetType().GetProperty("H" + i.ToString()).GetValue(regdespachotg2, null);
                        if (valorReg != null)
                        {
                            valorAnt2 = (decimal)valorReg;
                        }
                        regdespachotg2.GetType().GetProperty("H" + i.ToString()).SetValue(regdespachotg2, valorAnt2 + 1 * valor / 3);
                        valorReg = regdespachotv.GetType().GetProperty("H" + i.ToString()).GetValue(regdespachotv, null);
                        if (valorReg != null)
                        {
                            valorAnt3 = (decimal)valorReg;
                        }
                        regdespachotv.GetType().GetProperty("H" + i.ToString()).SetValue(regdespachotv, valorAnt3 + 1 * valor / 3);
                        if (total == null)
                            total = 1 * valor / 3;
                        else
                            total += valorAnt + 1 * valor / 3;
                        if (total2 == null)
                            total2 = 1 * valor / 3;
                        else
                            total2 += valorAnt2 + 1 * valor / 3;
                        if (totaltv == null)
                            totaltv = valor / 3;
                        else
                            totaltv += valorAnt3 + valor / 3;

                    }
            }
            if (total != null)
                regdespacho.Meditotal = total;
            if (total2 != null)
                regdespachotg2.Meditotal = total2;
            if (totaltv != null)
                regdespachotv.Meditotal = totaltv;
        }
        /// <summary>
        /// Distribuye la potencia del modo de operacion entre una Tg y La Tv
        /// </summary>
        /// <param name="listaDespacho"></param>
        /// <param name="registro"></param>
        /// <param name="usuario"></param>
        /// <param name="lectcodi"></param>
        /// <param name="ptoTg1"></param>
        /// <param name="ptoTg2"></param>
        /// <param name="ptoTg3"></param>
        /// <param name="ptoTv"></param>
        public void DistribuyeModoAGrupos(ref List<MeMedicion48DTO> listaDespacho, CpMedicion48DTO registro, string usuario, int lectcodi, int ptoTg1, int ptoTg2, int ptoTg3, int ptoTv)
        {
            MeMedicion48DTO regdespacho, regdespachotg2, regdespachotg3, regdespachotv;
            decimal? total = null, total2 = null, total3 = null, totaltv = null, valor;
            decimal valorAnt = 0, valorAnt2 = 0, valorAnt3 = 0, valorAnt4 = 0;
            regdespacho = listaDespacho.Find(x => x.Ptomedicodi == ptoTg1 && x.Medifecha == registro.Medifecha); //new MeMedicion48DTO(); // Tg1
            regdespachotg2 = listaDespacho.Find(x => x.Ptomedicodi == ptoTg2 && x.Medifecha == registro.Medifecha); //new MeMedicion48DTO(); // Tg2
            regdespachotg3 = listaDespacho.Find(x => x.Ptomedicodi == ptoTg3 && x.Medifecha == registro.Medifecha); //new MeMedicion48DTO(); // Tg3
            regdespachotv = listaDespacho.Find(x => x.Ptomedicodi == ptoTv && x.Medifecha == registro.Medifecha);//new MeMedicion48DTO(); // Tv

            for (int i = 1; i <= 48; i++)
            {
                valor = (decimal?)registro.GetType().GetProperty("H" + i.ToString()).GetValue(registro, null);
                if (valor != null)
                    if (valor != 0)
                    {
                        var valorReg = regdespacho.GetType().GetProperty("H" + i.ToString()).GetValue(regdespacho, null);
                        if (valorReg != null)
                        {
                            valorAnt = (decimal)valorReg;
                        }
                        regdespacho.GetType().GetProperty("H" + i.ToString()).SetValue(regdespacho, valorAnt + 2 * valor / 9);
                        valorReg = regdespachotg2.GetType().GetProperty("H" + i.ToString()).GetValue(regdespachotg2, null);
                        if (valorReg != null)
                        {
                            valorAnt2 = (decimal)valorReg;
                        }
                        regdespachotg2.GetType().GetProperty("H" + i.ToString()).SetValue(regdespachotg2, valorAnt2 + 2 * valor / 9);
                        valorReg = regdespachotg3.GetType().GetProperty("H" + i.ToString()).GetValue(regdespachotg3, null);
                        if (valorReg != null)
                        {
                            valorAnt3 = (decimal)valorReg;
                        }
                        regdespachotg3.GetType().GetProperty("H" + i.ToString()).SetValue(regdespachotg3, valorAnt3 + 2 * valor / 9);
                        valorReg = regdespachotv.GetType().GetProperty("H" + i.ToString()).GetValue(regdespachotv, null);
                        if (valorReg != null)
                        {
                            valorAnt4 = (decimal)valorReg;
                        }
                        regdespachotv.GetType().GetProperty("H" + i.ToString()).SetValue(regdespachotv, valorAnt4 + 1 * valor / 3);
                        if (total == null)
                            total = 2 * valor / 9;
                        else
                            total += valorAnt + 2 * valor / 9;
                        if (total2 == null)
                            total2 = 2 * valor / 9;
                        else
                            total2 += valorAnt2 + 2 * valor / 9;
                        if (total3 == null)
                            total3 = 2 * valor / 9;
                        else
                            total3 += valorAnt3 + 2 * valor / 9;
                        if (totaltv == null)
                            totaltv = valor / 3;
                        else
                            totaltv += valorAnt4 + valor / 3;

                    }
            }
            if (total != null)
                regdespacho.Meditotal = total;
            if (total2 != null)
                regdespachotg2.Meditotal = total2;
            if (total3 != null)
                regdespachotg3.Meditotal = total3;
            if (totaltv != null)
                regdespachotv.Meditotal = totaltv;
        }
        /// <summary>
        /// Une Termicas con Termicas CicloCombinado
        /// </summary>
        /// <param name="lista1"></param>
        /// <param name="lista2"></param>
        public void UnirTotalTermicas(ref List<MeMedicion48DTO> lista1, List<MeMedicion48DTO> lista2)
        {
            decimal? valor, valor1;
            decimal valTot;

            foreach (var reg in lista2)
            {
                var find = lista1.Find(x => x.Ptomedicodi == reg.Ptomedicodi && x.Medifecha == reg.Medifecha);
                if (find == null)
                {
                    lista1.Add(reg);
                }
                else
                {
                    for (var i = 1; i <= 48; i++)
                    {
                        valor = (decimal?)reg.GetType().GetProperty("H" + i.ToString()).GetValue(reg, null);
                        valor1 = (decimal?)find.GetType().GetProperty("H" + i.ToString()).GetValue(find, null);
                        valTot = (valor == null) ? ((valor1 == null) ? 0 : (decimal)valor1) : ((valor1 == null) ? (decimal)valor : (decimal)valor + (decimal)valor1);
                        find.GetType().GetProperty("H" + i.ToString()).SetValue(find, valTot);

                    }
                }
            }
            return;
        }
        /// <summary>
        /// Obtiene Plantas Rer de Yupana en memedicion48
        /// </summary>
        /// <param name="topcodi"></param>
        /// <param name="srestriccodi"></param>
        /// <param name="usuario"></param>
        /// <param name="lectcodi"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> ListaPlantasRerMcp(int topcodi, string srestriccodi, string usuario, int lectcodi)
        {
            List<MeMedicion48DTO> listaDespacho = new List<MeMedicion48DTO>();
            MeMedicion48DTO regdespacho;
            decimal? total, valor;
            var listaplanta = FactorySic.GetCpMedicion48Repository().ObtieneRegistrosToDespachoRerPrGrupo(topcodi, srestriccodi);
            foreach (var reg in listaplanta)
            {
                regdespacho = new MeMedicion48DTO();
                total = null;
                for (int i = 1; i <= 48; i++)
                {
                    valor = (decimal?)reg.GetType().GetProperty("H" + i.ToString()).GetValue(reg, null);
                    if (valor != null)
                    {
                        regdespacho.GetType().GetProperty("H" + i.ToString()).SetValue(regdespacho, valor);
                        if (total == null)
                            total = valor;
                        else
                            total += valor;
                    }
                }
                regdespacho.Meditotal = total;
                regdespacho.Medifecha = reg.Medifecha;
                regdespacho.Ptomedicodi = reg.Ptomedicodi;
                regdespacho.Tipoinfocodi = 1;
                regdespacho.Lastuser = usuario;
                regdespacho.Lastdate = DateTime.Now;
                regdespacho.Lectcodi = lectcodi;
                listaDespacho.Add(regdespacho);
            }
            return listaDespacho;
        }
        /// <summary>
        /// Obtiene Total de Sein, tal como Demanda, Perdidas,deficit y generacion Sein en MeMedicion48
        /// </summary>
        /// <param name="topcodi"></param>
        /// <param name="usuario"></param>
        /// <param name="lectcodi"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> ListaTotalSein(int topcodi, string usuario, int lectcodi)
        {
            List<MeMedicion48DTO> listaDespacho = new List<MeMedicion48DTO>();
            MeMedicion48DTO regdespacho;
            decimal? total, valor;

            string srestric = ConstantesBase.SresGeneracionSEIN.ToString() + "," + ConstantesBase.SresDemandaSEIN.ToString() + "," +
                ConstantesBase.SresPerdidasSEIN.ToString() + "," + ConstantesBase.SresDeficitSEIN.ToString();
            var lista = FactorySic.GetCpMedicion48Repository().GetByCriteriaRecurso(topcodi, srestric.ToString(), 0);
            foreach (var reg in lista)
            {
                regdespacho = new MeMedicion48DTO();
                total = null;
                for (int i = 1; i <= 48; i++)
                {
                    valor = (decimal?)reg.GetType().GetProperty("H" + i.ToString()).GetValue(reg, null);
                    if (valor != null)
                    {
                        regdespacho.GetType().GetProperty("H" + i.ToString()).SetValue(regdespacho, valor);
                        if (total == null)
                            total = valor;
                        else
                            total += valor;
                    }
                }
                regdespacho.Meditotal = total;
                regdespacho.Medifecha = reg.Medifecha;
                switch (reg.Srestcodi)
                {
                    case ConstantesBase.SresGeneracionSEIN:
                        regdespacho.Ptomedicodi = ConstantesBase.PtoGeneracionSEIN;
                        break;
                    case ConstantesBase.SresDemandaSEIN:
                        regdespacho.Ptomedicodi = ConstantesBase.PtoDemandaSEIN;
                        break;
                    case ConstantesBase.SresPerdidasSEIN:
                        regdespacho.Ptomedicodi = ConstantesBase.PtoPerdidasSEIN;
                        break;
                    case ConstantesBase.SresDeficitSEIN:
                        regdespacho.Ptomedicodi = ConstantesBase.PtoDeficitSEIN;
                        break;
                }

                regdespacho.Tipoinfocodi = ConstantesBase.TipoInfoMWDemanda;
                regdespacho.Lastuser = usuario;
                regdespacho.Lastdate = DateTime.Now;
                regdespacho.Lectcodi = lectcodi;
                listaDespacho.Add(regdespacho);
            }
            return listaDespacho;
        }
        /// <summary>
        /// Obtener el listado de medicion48 segun lectura
        /// </summary>
        /// <param name="lectcodi"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> GetObtenerHistoricoMedicion48(int lectcodi, DateTime fechaIni, DateTime fechaFin)
        {
            List<MeMedicion48DTO> lista48 = FactorySic.GetMeMedicion48Repository().ListarMeMedicion48ByFiltro(lectcodi.ToString(), fechaIni, fechaFin, ConstantesAppServicio.ParametroDefecto);
            return lista48;
        }
        /// <summary>
        /// Completar Eólica y Solares
        /// </summary>
        /// <param name="lista"></param>
        /// <returns></returns>
        public List<MeMedicion24DTO> CompletarDigSilentEolicaSolares(List<MeMedicion24DTO> lista)
        {
            List<MeMedicion24DTO> listaFinal = new List<MeMedicion24DTO>();
            List<MeMedicion24DTO> listaConfUnidadesFicticas = this.ListarEquiposEolicoSolarRE10650();

            foreach (var reg in lista)
            {
                if (ConstantesHorasOperacion.IdTipoEolica == reg.Famcodi
                    || ConstantesHorasOperacion.IdTipoSolar == reg.Famcodi)
                {
                    if (reg.Equicodi == 14426 || reg.Equicodi == 18074)
                    { }
                    var listaUnidadFict = listaConfUnidadesFicticas.Where(x => x.Equipadre == reg.Equicodi).ToList();
                    decimal potenciaTotalMVA = listaUnidadFict.Sum(x => x.NumUnidadesXGrupo * x.MVAxUnidad.GetValueOrDefault(0));

                    var listaDataXEq = new List<MeMedicion24DTO>();
                    foreach (var regEq in listaUnidadFict)
                    {
                        listaDataXEq.Add(this.CopiarDataDigsilent24(reg, regEq, potenciaTotalMVA));
                    }

                    listaFinal.AddRange(listaDataXEq);
                }
                else
                {
                    listaFinal.Add(reg);
                }
            }

            return listaFinal;
        }
        /// <summary>
        /// Listar las unidades para las unidades eólicas y solares
        /// </summary>
        /// <returns></returns>
        public List<MeMedicion24DTO> ListarEquiposEolicoSolarRE10650()
        {
            MeMedicion24DTO obj = null;
            List<MeMedicion24DTO> l = new List<MeMedicion24DTO>();

            //TALARA_E
            obj = new MeMedicion24DTO() { };
            obj.Equipadre = 14426;
            obj.Digsilent = "PE_Talara";
            obj.NumUnidadesXGrupo = 17;
            obj.MVAxUnidad = 2.0m;
            l.Add(obj);

            //CUPISNIQUE_E
            obj = new MeMedicion24DTO() { };
            obj.Equipadre = 14407;
            obj.Digsilent = "PE_Cupisnique";
            obj.NumUnidadesXGrupo = 45;
            obj.MVAxUnidad = 2.0m;
            l.Add(obj);

            //WAYRA1_E
            obj = new MeMedicion24DTO() { };
            obj.Equipadre = 18306;
            obj.Digsilent = "CE_Wayra I";
            obj.NumUnidadesXGrupo = 42;
            obj.MVAxUnidad = 3.9m;
            l.Add(obj);

            //MAJES_S
            obj = new MeMedicion24DTO() { };
            obj.Equipadre = 13402;
            obj.Digsilent = "PS_Majes1";
            obj.NumUnidadesXGrupo = 16;
            obj.MVAxUnidad = 0.625m;
            l.Add(obj);

            obj = new MeMedicion24DTO() { };
            obj.Equipadre = 13402;
            obj.Digsilent = "PS_Majes2";
            obj.NumUnidadesXGrupo = 16;
            obj.MVAxUnidad = 0.625m;
            l.Add(obj);

            //REPARTICIO_S
            obj = new MeMedicion24DTO() { };
            obj.Equipadre = 13399;
            obj.Digsilent = "PS_Reparticion1";
            obj.NumUnidadesXGrupo = 16;
            obj.MVAxUnidad = 0.625m;
            l.Add(obj);

            obj = new MeMedicion24DTO() { };
            obj.Equipadre = 13399;
            obj.Digsilent = "PS_Reparticion2";
            obj.NumUnidadesXGrupo = 16;
            obj.MVAxUnidad = 0.625m;
            l.Add(obj);

            //TACNA_S
            obj = new MeMedicion24DTO() { };
            obj.Equipadre = 13503;
            obj.Digsilent = "PS_Tacna1";
            obj.NumUnidadesXGrupo = 16;
            obj.MVAxUnidad = 0.625m;
            l.Add(obj);

            obj = new MeMedicion24DTO() { };
            obj.Equipadre = 13503;
            obj.Digsilent = "PS_Tacna2";
            obj.NumUnidadesXGrupo = 16;
            obj.MVAxUnidad = 0.625m;
            l.Add(obj);

            //PANAMERICA_S
            obj = new MeMedicion24DTO() { };
            obj.Equipadre = 13533;
            obj.Digsilent = "PS_Panamericana 2";
            obj.NumUnidadesXGrupo = 16;
            obj.MVAxUnidad = 0.625m;
            l.Add(obj);

            obj = new MeMedicion24DTO() { };
            obj.Equipadre = 13533;
            obj.Digsilent = "PS_Panamericana1";
            obj.NumUnidadesXGrupo = 16;
            obj.MVAxUnidad = 0.625m;
            l.Add(obj);

            //MOQUEGUA_S
            obj = new MeMedicion24DTO() { };
            obj.Equipadre = 14762;
            obj.Digsilent = "PS_MoqueguaFV 1";
            obj.NumUnidadesXGrupo = 10;
            obj.MVAxUnidad = 0.8m;
            l.Add(obj);

            obj = new MeMedicion24DTO() { };
            obj.Equipadre = 14762;
            obj.Digsilent = "PS_MoqueguaFV 2";
            obj.NumUnidadesXGrupo = 10;
            obj.MVAxUnidad = 0.8m;
            l.Add(obj);

            //INTIPAMPA_S
            obj = new MeMedicion24DTO() { };
            obj.Equipadre = 17796;
            obj.Digsilent = "CS_Intipampa1";
            obj.NumUnidadesXGrupo = 9;
            obj.MVAxUnidad = 2.5m;
            l.Add(obj);

            obj = new MeMedicion24DTO() { };
            obj.Equipadre = 17796;
            obj.Digsilent = "CS_Intipampa2";
            obj.NumUnidadesXGrupo = 9;
            obj.MVAxUnidad = 2.5m;
            l.Add(obj);

            //RUBI_S
            obj = new MeMedicion24DTO() { };
            obj.Equipadre = 18074;
            obj.Digsilent = "CS_Rubi1";
            obj.NumUnidadesXGrupo = 40;
            obj.MVAxUnidad = 1.025m;
            l.Add(obj);

            obj = new MeMedicion24DTO() { };
            obj.Equipadre = 18074;
            obj.Digsilent = "CS_Rubi2";
            obj.NumUnidadesXGrupo = 40;
            obj.MVAxUnidad = 1.025m;
            l.Add(obj);

            obj = new MeMedicion24DTO() { };
            obj.Equipadre = 18074;
            obj.Digsilent = "CS_Rubi3";
            obj.NumUnidadesXGrupo = 42;
            obj.MVAxUnidad = 1.025m;
            l.Add(obj);

            obj = new MeMedicion24DTO() { };
            obj.Equipadre = 18074;
            obj.Digsilent = "CS_Rubi4";
            obj.NumUnidadesXGrupo = 42;
            obj.MVAxUnidad = 1.025m;
            l.Add(obj);

            //MARCONA_E
            obj = new MeMedicion24DTO() { };
            obj.Equipadre = 14160;
            obj.Digsilent = "PE_Marcona1";
            obj.NumUnidadesXGrupo = 8;
            obj.MVAxUnidad = 3.15m;
            l.Add(obj);

            obj = new MeMedicion24DTO() { };
            obj.Equipadre = 14160;
            obj.Digsilent = "PE_Marcona2";
            obj.NumUnidadesXGrupo = 3;
            obj.MVAxUnidad = 2.3m;
            l.Add(obj);

            //TRES_HERMANA
            obj = new MeMedicion24DTO() { };
            obj.Equipadre = 15160;
            obj.Digsilent = "PE_3 Hermanas 1";
            obj.NumUnidadesXGrupo = 25;
            obj.MVAxUnidad = 3.15m;
            l.Add(obj);

            obj = new MeMedicion24DTO() { };
            obj.Equipadre = 15160;
            obj.Digsilent = "PE_3 Hermanas 2";
            obj.NumUnidadesXGrupo = 8;
            obj.MVAxUnidad = 2.3m;
            l.Add(obj);


            //////////////////////////////////////////////////////
            ///Asignar un equicodi ficticio

            int maxEquicodi = -10000;
            foreach (var reg in l)
            {
                reg.PotenciaTotalMVA = reg.NumUnidadesXGrupo * reg.MVAxUnidad;
                reg.Equicodi = maxEquicodi;
                maxEquicodi--;
            }

            return l;
        }

        /// <summary>
        /// Copiar informacion de la central a sus unidades
        /// </summary>
        /// <param name="m24"></param>
        /// <param name="regEquipo"></param>
        /// <param name="totalMVACentral"></param>
        /// <returns></returns>
        private MeMedicion24DTO CopiarDataDigsilent24(MeMedicion24DTO m24, MeMedicion24DTO regEquipo, decimal totalMVACentral)
        {
            MeMedicion24DTO reg = new MeMedicion24DTO();
            reg.Medifecha = m24.Medifecha;
            reg.Ptomedicodi = m24.Ptomedicodi;
            reg.Emprcodi = m24.Emprcodi;
            reg.Lectcodi = m24.Lectcodi;
            reg.Tipoinfocodi = m24.Tipoinfocodi;
            for (int h = 1; h <= 24; h++)
            {
                decimal valorCentral = ((decimal?)m24.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(m24, null)).GetValueOrDefault(0);

                decimal mwXUnidadGenerador = valorCentral * regEquipo.PotenciaTotalMVA.GetValueOrDefault(0) / totalMVACentral;
                //Para el archivo DLE (Potencia entre el número de unidades de cada grupo)
                decimal valorUnidadGenerador = mwXUnidadGenerador / regEquipo.NumUnidadesXGrupo;

                reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + (h).ToString()).SetValue(reg, valorUnidadGenerador);
            }
            reg.Meditotal = m24.Meditotal;

            reg.Grupocodi = m24.Grupocodi;
            reg.Gruponomb = m24.Gruponomb;
            reg.Grupoabrev = m24.Grupoabrev;
            reg.Grupotipo = m24.Grupotipo;
            reg.Equicodi = regEquipo.Equicodi;
            reg.Equipadre = regEquipo.Equipadre;
            reg.Central = m24.Central;
            reg.Famcodi = m24.Famcodi == ConstantesHorasOperacion.IdTipoEolica ? ConstantesHorasOperacion.IdGeneradorEolica : ConstantesHorasOperacion.IdGeneradorSolar;
            reg.Equiabrev = "--";
            reg.Equinomb = "--";
            reg.Minimo = 0; //actualmente las centrales eolicas y solares no tienen una propiedad mínima
            reg.PotenciaEfectiva = regEquipo.PotenciaTotalMVA;
            reg.PotenciaEfectiva = Math.Round(reg.PotenciaEfectiva.GetValueOrDefault(0), 3);
            reg.FechapropequiMin = null;
            reg.FechapropequiPotefec = m24.FechapropequiPotefec;
            reg.Digsilent = regEquipo.Digsilent;
            reg.Emprnomb = m24.Emprnomb;

            return reg;
        }
        /// <summary>
        /// reporte
        /// </summary>
        /// <param name="listaGeneracionOpera"></param>
        /// <returns></returns>
        public string ReporteConfiguracionUnidadesOperaHtml(List<MeMedicion24DTO> listaGeneracionOpera)
        {
            var listaData = listaGeneracionOpera;
            if (listaGeneracionOpera.Count == 0) return string.Empty;

            StringBuilder str = new StringBuilder();

            //
            str.Append("<table class='pretty tabla-adicional' border='0' cellspacing='0' width='auto' id='tablaConfOpera'>");

            str.Append("<thead>");
            #region cabecera
            str.Append("<tr>");
            str.Append("<th colspan='8' style='width: 600px;'>Equipo</th>");
            str.Append("<th colspan='2' style='width: 220px; background: #ba80d8 !important'>Potencia mínima</th>");
            str.Append("<th colspan='2' style='width: 220px; background: #8829b9 !important' >Potencia máxima</th>");
            str.Append("<th colspan='1' style='width: 200px; background: #17b6bd !important'>DIgSILENT</th>");
            str.Append("</tr>");

            str.Append("<tr>");

            str.Append("<th style='width: 100px'>Empresa</th>");
            str.Append("<th style='width:  20px'>Cód. grupo<br>despacho</th>");
            str.Append("<th style='width:  70px'>Abrev. grupo</th>");
            str.Append("<th style='width:  20px'>Operó grupo<br>despacho</th>");
            str.Append("<th style='width:  20px'>Cód. pto<br>despacho</th>");
            str.Append("<th style='width: 100px'>Central</th>");
            str.Append("<th style='width:  20px'>Cód. eq</th>");
            str.Append("<th style='width:  70px'>Abrev. eq</th>");

            str.Append("<th style='width:  70px; background: #ba80d8 !important'>Fecha Vigencia</th>");
            str.Append("<th style='width:  40px; background: #ba80d8 !important'>Valor</th>");

            str.Append("<th style='width:  70px; background: #8829b9 !important'>Fecha Vigencia</th>");
            str.Append("<th style='width:  40px; background: #8829b9 !important'>Valor</th>");

            str.Append("<th style='width: 100px; background: #17b6bd !important;'>Foreign Key</th>");

            str.Append("</tr>");
            #endregion
            str.Append("</thead>");

            str.Append("<tbody>");
            #region cuerpo

            foreach (var reg in listaData)
            {
                str.Append("<tr>");
                str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.Emprnomb);
                str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.Grupocodi);
                str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.Grupoabrev);
                str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.Meditotal != null ? "<b>" + ConstantesAppServicio.SIDesc + "</b>" : ConstantesAppServicio.NODesc);
                str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.Ptomedicodi);
                str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.Central);
                str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.Equicodi > 0 ? reg.Equicodi + "" : "--");
                str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.Equiabrev);

                str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.FechapropequiMinDesc);
                str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.Minimo);

                str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.FechapropequiPotefecDesc);
                str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.PotenciaEfectiva);

                str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.Digsilent);

                str.Append("</tr>");
            }

            #endregion
            str.Append("</tbody>");

            str.Append("</table>");

            return str.ToString();
        }

        /// <summary>
        /// Obtener valores duplicados de Foreign Key
        /// </summary>
        /// <param name="listaM24Digsilent"></param>
        /// <returns></returns>
        public string VerificarDuplicadosForeignKey(List<MeMedicion24DTO> listaM24Digsilent)
        {
            string msj = string.Empty;

            var listaDuplicados = listaM24Digsilent.Where(x => x.Digsilent != null && x.Digsilent.Length > 0)
                .GroupBy(x => new { x.Digsilent })
                .Where(x => x.Count() >= 2)
                .Select(x => new { x.Key.Digsilent, Total = x.Count(), Desc = x.Key.Digsilent + "(" + x.Count() + ")" })
                .OrderBy(x => x.Digsilent).ToList();

            var listaSinDigsilent = listaM24Digsilent.Where(x => x.Digsilent == null || x.Digsilent.Length == 0).ToList();
            if (listaSinDigsilent.Any())
                msj += string.Format("Existe(n) {0} equipo(s) sin DIgSILENT Foreign Key", listaSinDigsilent.Count()) + ".";
            if (listaDuplicados.Any())
                msj += (msj.Length > 0 ? "<br>" : string.Empty) + "Existen varios equipos que tiene el mismo DIgSILENT Foreign Key: " + string.Join(", ", listaDuplicados.Select(x => x.Desc).ToList()) + ". ";

            return msj;
        }
        /// <summary>
        /// Lista demanda
        /// </summary>
        /// <param name="propcodi"></param>
        /// <param name="famcodi"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<MeMedicion24DTO> GetListaDemandaDigsilent(string propcodi, string famcodi, DateTime fecha)
        {
            return FactorySic.GetMeMedicion24Repository().ListaDemandaDigsilent(propcodi, famcodi, fecha);
        }
        /// <summary>
        /// Obtener fecha en formato digsilent
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        private string GetFechaDigsilent(DateTime fecha)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

            DateTimeFormatInfo mfi = new DateTimeFormatInfo();
            string strMonthName = mfi.GetMonthName(fecha.Month).ToString(); //August
            string shortMonthName = strMonthName.Substring(0, 3); //Aug

            string strfecha = fecha.ToString("dd");
            strfecha += textInfo.ToTitleCase(shortMonthName.ToLower());
            strfecha += fecha.ToString("yyyy");

            return strfecha;
        }
        /// <summary>
        /// Lista DigSilent tabla Eqequipo
        /// </summary>
        /// <param name="h">bloque</param>
        /// <param name="l">EqEquipoDTO</param>
        /// <param name="ListaManttos"></param>
        /// <param name="ListaEquirel"></param>
        /// <returns></returns>
        private List<EqEquipoDTO> ListaDigSilentEqequipo(int h, EqEquipoDTO l, List<EveManttoDTO> ListaManttos, List<EqEquirelDTO> ListaEquirel)
        {
            List<EqEquipoDTO> lista_ = new List<EqEquipoDTO>();

            string valor_ = VerificaChkLineas(h, l.Equicodi, ListaManttos);
            if (valor_ == "1")
            {
                l.Formuladat = valor_;
                l.Correlativo = h;
                lista_.Add(l);
            }
            else
            {
                var dat = ListaEquirel.Find(x => x.Equicodi1 == l.Equicodi);
                if (dat != null)
                {
                    valor_ = VerificaChkLineas(h, dat.Equicodi2, ListaManttos);
                    lista_.Add(new EqEquipoDTO()
                    {
                        Equicodi = dat.Equicodi2,
                        Valor = l.Valor,
                        Correlativo = h,
                        Formuladat = valor_
                    });
                }
                else
                {
                    dat = ListaEquirel.Find(x => x.Equicodi2 == l.Equicodi);
                    if (dat != null)
                    {
                        valor_ = VerificaChkLineas(h, dat.Equicodi1, ListaManttos);
                        lista_.Add(new EqEquipoDTO()
                        {
                            Equicodi = dat.Equicodi1,
                            Valor = l.Valor,
                            Correlativo = h,
                            Formuladat = valor_
                        });
                    }
                    else
                    {
                        l.Formuladat = valor_;
                        l.Correlativo = h;
                        lista_.Add(l);
                    }
                }
            }

            return lista_;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="h"></param>
        /// <param name="equicodi_"></param>
        /// <param name="ListaManttos"></param>
        /// <returns></returns>
        private string VerificaChkLineas(int h, int equicodi_, List<EveManttoDTO> ListaManttos)
        {
            DateTime evenini_, evenfin_;
            DateTime horaini_, horafin_;
            foreach (var d in ListaManttos)
            {
                var equicodi = Convert.ToInt32(d.Equicodi);

                if (equicodi_ == equicodi)
                {
                    evenini_ = d.Evenini.Value;
                    evenfin_ = d.Evenfin.Value;

                    if (h == 23)
                    {
                        horaini_ = Convert.ToDateTime(evenini_.ToString("yyyy-MM-dd") + " " + h + ":00:00");
                        horafin_ = evenini_.AddDays(1);
                    }
                    else
                    {
                        horaini_ = Convert.ToDateTime(evenini_.ToString("yyyy-MM-dd") + " " + h + ":00:00");
                        horafin_ = Convert.ToDateTime(evenini_.ToString("yyyy-MM-dd") + " " + (h + 1) + ":00:00");
                    }

                    if (((evenini_ < horaini_) && (horaini_ < evenfin_)) || ((evenini_ < horafin_) && (horafin_ < evenfin_)))
                    {
                        return "1";
                    }
                }
            }

            return "0";
        }
        /// <summary>
        /// Txt Digsilent Html
        /// </summary>
        /// <param name="listaEqequipo"></param>
        /// <returns></returns>
        public string ArchivoDigsilentHtml(List<EqEquipoDTO> listaEqequipo)
        {
            StringBuilder srtHtml = new StringBuilder();

            foreach (var d in listaEqequipo)
            {
                srtHtml.Append("set/fkey obj=" + d.Valor + " val=" + d.Formuladat + "<br>");
            }

            return srtHtml.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="grupo_old"></param>
        /// <param name="filaini"></param>
        /// <param name="filafin"></param>
        /// <param name="bloq"></param>
        private void ListaDigSilentPrgrupo(int grupocodi, int bloq, ref StringBuilder strArchivoDigsilent, ref StringBuilder strObservacion, List<MeMedicion24DTO> listaM24, List<EveManttoDTO> listaManttos)
        {
            int famcodi = listaM24.First().Famcodi;
            int totalElementos = listaM24.Count();

            bool[] arr_Mantto = new bool[totalElementos];
            double cociente = 1.0;
            //if (grupo_old == 405) {cociente = 20.0;} //trujillo norte

            //indisponibilidad por gr
            for (int i = 0; i < totalElementos; i++)
            {
                if (!arr_Mantto[i])
                {
                    if (GetManttoEquipo(bloq, listaM24[i].Equicodi, listaM24[i].Equipadre, listaManttos))
                    {
                        arr_Mantto[i] = true;
                    }
                }
            }

            //reporta equipos sin datos completos
            bool procesagrupo = true;

            for (int i = 0; i < totalElementos; i++)
            {
                if (string.IsNullOrEmpty(listaM24[i].Digsilent))
                {
                    strObservacion.Append(listaM24[i].Gruponomb.Trim() + " " + listaM24[i].Equiabrev + " Definicion Digsilent no existe<br>");
                }

                if (listaM24[i].Minimo == null)
                {
                    strObservacion.Append(listaM24[i].Gruponomb.Trim() + " " + listaM24[i].Equiabrev + " Potencia minima no existe<br>");
                }

                if (listaM24[i].PotenciaEfectiva == null)
                {
                    strObservacion.Append(listaM24[i].Gruponomb.Trim() + " " + listaM24[i].Equiabrev + " Potencia efectiva no existe<br>");
                    strObservacion.Append("No se proceso " + listaM24[i].Gruponomb.Trim() + ". No tiene todas su P.Efectivas<br>");
                    // procesagrupo = false;
                }
                else
                {
                    if (listaM24[i].PotenciaEfectiva == 0)
                    {
                        strObservacion.Append("No se proceso " + listaM24[i].Gruponomb.Trim() + ". No tiene todas su P.Efectivas<br>");
                        //     procesagrupo = false;
                    }
                }
            }

            //reporta eq en mantenimiento
            //Los equipos que no tienen manto irán a la repartición de potencia
            List<MeMedicion24DTO> listaM24SinMantto = new List<MeMedicion24DTO>();
            for (int i = 0; i < totalElementos; i++)
            {
                if (arr_Mantto[i] || string.IsNullOrEmpty(listaM24[i].Digsilent))
                {
                    if (!string.IsNullOrEmpty(listaM24[i].Digsilent))
                    {
                        strArchivoDigsilent.Append("set/fkey obj=" + listaM24[i].Digsilent + " val=0,1<br>");
                    }

                    //si no tiene ForeignKey no se procesará
                }
                else
                {
                    listaM24SinMantto.Add(listaM24[i]);
                }
            }
            totalElementos = listaM24SinMantto.Count();

            //reparto
            if (procesagrupo && totalElementos > 0)
            {
                if (ConstantesHorasOperacion.IdTipoEolica != famcodi && ConstantesHorasOperacion.IdTipoSolar != famcodi
                    && ConstantesHorasOperacion.IdGeneradorEolica != famcodi && ConstantesHorasOperacion.IdGeneradorSolar != famcodi)
                    this.RepartirPotencia(bloq, cociente, ref strArchivoDigsilent, ref strObservacion, listaM24SinMantto);
                else
                    this.RepartirMVA(bloq, cociente, ref strArchivoDigsilent, ref strObservacion, listaM24SinMantto);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="h"></param>
        /// <param name="equicodi"></param>
        /// <param name="equipadre"></param>
        /// <param name="listaManttos"></param>
        /// <returns></returns>
        private bool GetManttoEquipo(int h, int equicodi, int equipadre, List<EveManttoDTO> listaManttos)
        {
            DateTime _evenini, _evenfin;
            DateTime _horaini, _horafin;

            List<EveManttoDTO> subLista = listaManttos.Where(x => x.Equicodi == equicodi || x.Equicodi == equipadre).ToList();

            foreach (var d in subLista)
            {
                _evenini = Convert.ToDateTime(d.Evenini);
                _evenfin = Convert.ToDateTime(d.Evenfin);

                if (h == 23)
                {
                    _horaini = Convert.ToDateTime(_evenini.ToString("yyyy-MM-dd") + " " + h.ToString() + ":00:00");
                    _horafin = _evenini.AddDays(1);
                }
                else
                {
                    _horaini = Convert.ToDateTime(_evenini.ToString("yyyy-MM-dd") + " " + h.ToString() + ":00:00");
                    _horafin = Convert.ToDateTime(_evenini.ToString("yyyy-MM-dd") + " " + (h + 1).ToString() + ":00:00");
                }

                if (((_evenini < _horaini) && (_horaini < _evenfin)) || ((_evenini < _horafin) && (_horafin < _evenfin)))
                {
                    return true;
                }
            }

            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bloq"></param>
        /// <param name="cociente"></param>
        /// <param name="strArchivoDigsilent"></param>
        /// <param name="strObservacion"></param>
        /// <param name="listaM24"></param>
        private void RepartirPotencia(int bloq, double cociente, ref StringBuilder strArchivoDigsilent, ref StringBuilder strObservacion, List<MeMedicion24DTO> listaM24)
        {
            MeMedicion24DTO regPrimerGen = listaM24[0];
            //El valor del H de medicion48 es el mismo para todos los equipos del grupo despacho
            decimal? va_ = (decimal?)regPrimerGen.GetType().GetProperty("H" + (bloq + 1).ToString()).GetValue(regPrimerGen, null);
            double valorH = Convert.ToDouble(va_);
            int nro_elem = listaM24.Count();

            if (nro_elem == 1)
            {
                if (va_ != null)
                {
                    if (valorH == 0)
                    {
                        strArchivoDigsilent.Append("set/fkey obj=" + regPrimerGen.Digsilent + " val=0,1<br>");
                    }
                    else
                    {
                        strArchivoDigsilent.Append("set/fkey obj=" + regPrimerGen.Digsilent + " val=" + Math.Round(valorH / cociente, 3).ToString() + ",0<br>");
                    }
                }
                else
                {
                    strArchivoDigsilent.Append("set/fkey obj=" + regPrimerGen.Digsilent + " val=" + Math.Round(valorH / cociente, 3).ToString() + ",0<br>");
                }
                return;
            }


            //sum Pmin
            double sumPmin = 0;//suma potencia minima
            double sumPef = 0;//suma potencia efectiva
            double sumRango = 0;

            for (int i = 0; i < nro_elem; i++)
            {
                if (listaM24[i].Minimo == null || listaM24[i].PotenciaEfectiva == null)
                    return;
                double pmin = Convert.ToDouble(listaM24[i].Minimo);
                double pmax = Convert.ToDouble(listaM24[i].PotenciaEfectiva);

                sumPmin += pmin;
                sumPef += pmax;
                sumRango += (pmax - pmin);
            }


            if (sumPmin > Convert.ToDouble(va_))
            {
                //parar a la unidad de menor potencia efectiva y/o en mantto
                strArchivoDigsilent.Append("set/fkey obj=" + listaM24[0].Digsilent + " val=0,1<br>");

                int nuevoNroElem = nro_elem - 1;
                if (nuevoNroElem > 0)
                {
                    List<MeMedicion24DTO> nuevaListaGeneracionOpera = listaM24.GetRange(nuevoNroElem >= 1 ? 1 : 0, nuevoNroElem);
                    this.RepartirPotencia(bloq, cociente, ref strArchivoDigsilent, ref strObservacion, nuevaListaGeneracionOpera);
                }
            }
            else
            {
                //reparto en ff. de potencia efectiva
                double prop_ = 0;
                double peff_ = 0;

                prop_ = valorH;

                if (sumPmin >= 0) //2009-10-26
                {
                    prop_ -= sumPmin;

                    for (int i = 0; i < nro_elem; i++)
                    {
                        double _valor1 = Convert.ToDouble(listaM24[i].Minimo);
                        double _valor2 = Convert.ToDouble(listaM24[i].PotenciaEfectiva);

                        peff_ = _valor1 + (valorH - sumPmin) * (_valor2 - _valor1) / (sumRango * 1.0);

                        if (peff_ == 0)
                        {
                            strArchivoDigsilent.Append("set/fkey obj=" + listaM24[i].Digsilent + " val=0,1<br>");
                        }
                        else
                        {
                            strArchivoDigsilent.Append("set/fkey obj=" + listaM24[i].Digsilent + " val=" + Math.Round(peff_ / cociente, 3).ToString() + ",0<br>");
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Función para repartir Potencia Total por unidad (MVA) de las Centrales solares y eólicas
        /// </summary>
        /// <param name="bloq"></param>
        /// <param name="cociente"></param>
        /// <param name="strArchivoDigsilent"></param>
        /// <param name="strObservacion"></param>
        /// <param name="listaM24"></param>
        private void RepartirMVA(int bloq, double cociente, ref StringBuilder strArchivoDigsilent, ref StringBuilder strObservacion, List<MeMedicion24DTO> listaM24)
        {
            //El valor del H de medicion48 es el mismo para todos los equipos del grupo despacho
            int nro_elem = listaM24.Count();

            foreach (var reg in listaM24)
            {
                decimal? va_ = (decimal?)reg.GetType().GetProperty("H" + (bloq + 1).ToString()).GetValue(reg, null);
                double valorH = Convert.ToDouble(va_);

                if (va_ != null)
                {
                    if (valorH == 0)
                    {
                        strArchivoDigsilent.Append("set/fkey obj=" + reg.Digsilent + " val=0,1<br>");
                    }
                    else
                    {
                        strArchivoDigsilent.Append("set/fkey obj=" + reg.Digsilent + " val=" + Math.Round(valorH, 3).ToString() + ",0<br>");
                    }
                }
                else
                {
                    strArchivoDigsilent.Append("set/fkey obj=" + reg.Digsilent + " val=" + Math.Round(valorH, 3).ToString() + ",0<br>");
                }
            }
        }
        /// <summary>
        /// Get informacion me_lectura BD
        /// </summary>
        /// <param name="lectcodi"></param>
        /// <returns></returns>
        public List<MeLecturaDTO> GetByCriteriaMeLectura(string lectcodi)
        {
            return FactorySic.GetMeLecturaRepository().GetByCriteria(lectcodi);
        }
        /// <summary>
        /// Valida campos de proceso Digsilent
        /// </summary>
        /// <param name="program"></param>
        /// <param name="fecha"></param>
        /// <param name="rdchk"></param>
        /// <param name="bloq"></param>
        /// <param name="fuente"></param>
        /// <param name="topcodiYupana"></param>
        /// <returns></returns>
        public MigracionesResult ValidaCampos(string program, string fecha, int rdchk, string bloq, int fuente, int topcodiYupana)
        {
            MigracionesResult modelo = new MigracionesResult();
           
            var ListaTipoProg = GetByCriteriaMeLectura(program);//lectcodi
            DateTime _fecha;
            modelo.nRegistros = 1;

            if (bloq == "")
            {
                modelo.nRegistros = -1;
                modelo.Mensaje = "Debe al menos seleccionar un bloque de Horario";
                return modelo;
            }
            else if (!DateTime.TryParseExact(fecha, ConstantesMigraciones.FormatoFecha, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fecha))
            {
                modelo.nRegistros = -1;
                modelo.Mensaje = "El formato de fecha ingresado es incorrecto.";
                return modelo;
            }

            if (!ConstantesAppServicio.TipoFuente.Contains(fuente.ToString()))
            {
                modelo.nRegistros = -1;
                modelo.Mensaje = "El código de tipo de fuente ingresado es incorrecto.";
                return modelo;
            }

            if (ListaTipoProg.Count == 0)
            {
                modelo.nRegistros = -1;
                modelo.Mensaje = "El código de Tipo de Programación ingresado es incorrecto.";
                return modelo;
            }

            if (topcodiYupana <= 0)
            {
                modelo.nRegistros = -1;
                modelo.Mensaje = "El código Yupana ingresado es incorrecto.";
                return modelo;
            }

            if (!ConstantesAppServicio.TipoRadio.Contains(rdchk.ToString()))
            {
                modelo.nRegistros = -1;
                modelo.Mensaje = "El código ingresado es incorrecto.";
                return modelo;
            }

            return modelo;
        }
        /// <summary>
        /// Proceso de crear el archivo digsilent .dle
        /// </summary>
        /// <param name="texto_"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public MigracionesResult SaveDigSilent(string texto_, string fecha)
        {
            MigracionesResult modelo = new MigracionesResult();
            try
            {
                string ruta = ConstantesAppServicio.FileSystemMigraciones;
                DateTime dtfecha = DateTime.ParseExact(fecha, ConstantesBase.FormatoFechaBase, CultureInfo.InvariantCulture);
                string nameFile = ConstantesMigraciones.FileDigsilente + "_" + dtfecha.ToString("yyyyMMdd") + ConstantesAppServicio.ExtensionDle;

                GenerarArchivoDigSilent(dtfecha, ruta + nameFile, texto_);

                modelo.Resultado = nameFile;
                modelo.nRegistros = 1;
            }
            catch (Exception ex)
            {
                modelo.nRegistros = -1;
                modelo.Mensaje = ex.Message;
                modelo.Detalle = ex.StackTrace;
            }

            return modelo;
        }
        /// <summary>
        /// Generador de archivo .dle
        /// </summary>
        /// <param name="f_"></param>
        /// <param name="file"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public void GenerarArchivoDigSilent(DateTime f_, string file, string tags)
        {
            try
            {
                FileInfo newFile = new FileInfo(file);
                FileServer.CreateFolder(null, null, ConstantesAppServicio.FileSystemMigraciones);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(file);
                }

                try
                {
                    string[] separador = new string[] { "<br>" };
                    if (!newFile.Exists)
                    {
                        using (StreamWriter sw = newFile.CreateText())
                        {
                            var texto_ = tags.Split(separador, StringSplitOptions.RemoveEmptyEntries).ToList();
                            foreach (var d in texto_)
                            {
                                sw.WriteLine(d);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new ArgumentException(ex.Message, ex);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message, ex);
            }
        }
    }
}
