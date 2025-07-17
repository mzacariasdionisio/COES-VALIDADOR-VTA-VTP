using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.General;
using GAMS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.CortoPlazo.Helper
{
    public class EjecucionGams
    {
        #region Movisoft - Ticket 18685
        /// <summary>
        /// Realiza la ejecuacion GAMS
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="parametros">string parametros = @"D:\Gams\Casos\201610191840.DAT "</param>
        /// <param name="salida">string salida = @"D:\Gams\Salida\salida6.csv"</param>
        public static int Ejecutar(DateTime fechaProceso, string path, string parametros, string salida, string salida2, string workspace)
        {
            int resultado = -1;

            double pendiente = (double)(new ParametroAppServicio()).ObtenerValorParametro(ConstantesCortoPlazo.IdParametroPendiente, fechaProceso);
            if (pendiente == 0)
                pendiente = 0.95;

            try
            {
                //- Declaraando los objetos para integracion con GAMS

                #region Ticket - 18685
                string XsystemDirectory = @"C:\GAMS\34";
                string pathWorkspace = ConfigurationManager.AppSettings["PathWorkspaceCM"] + workspace;
                GAMSWorkspace ws = new GAMSWorkspace(workingDirectory: pathWorkspace, XsystemDirectory, DebugLevel.Off);
                //GAMSWorkspace ws = new GAMSWorkspace();
                #endregion

                GAMSDatabase db = ws.AddDatabase();

                using (GAMSOptions opt = ws.AddOptions())
                {
                    //- Ejecutando el modelo

                    GAMSJob modelo = ws.AddJobFromString(ObtenerModelo(path + parametros, path + salida, path + salida2, pendiente.ToString()));
                    modelo.Run(opt, db);
                }

                //- Leyendo el archivo de resultaodo
                if (FileServer.VerificarExistenciaFile(string.Empty, salida, path))
                {
                    resultado = 1;
                }
                else
                {
                    //- No se ejecuto GAMS
                    resultado = -2;
                }
            }
            catch (Exception ex)
            {
                //- Ha ocurrido un error
                resultado = -1;
                throw new Exception(ex.Message, ex);
            }

            return resultado;
        }

        #endregion

        /// <summary>
        /// Permite ejecutar el modelo GAMS Alternativo
        /// </summary>
        /// <param name="fechaProceso"></param>
        /// <param name="path"></param>
        /// <param name="parametros"></param>
        /// <param name="salida"></param>
        /// <returns></returns>
        public static int EjecutarAlternativo(DateTime fechaProceso, string path, string parametros, string salida, string salida2)
        {
            int resultado = -1;

            try
            {
                //- Declaraando los objetos para integracion con GAMS    
                string XsystemDirectory = @"C:\GAMS\34";
                GAMSWorkspace ws = new GAMSWorkspace(null, XsystemDirectory);

                GAMSDatabase db = ws.AddDatabase();

                using (GAMSOptions opt = ws.AddOptions())
                {
                    //- Ejecutando el modelo

                    GAMSJob modelo = ws.AddJobFromString(ObtenerModeloAlternativo(path + parametros, path + salida, path + salida2));
                    modelo.Run(opt, db);
                }

                //- Leyendo el archivo de resultaodo
                if (FileServer.VerificarExistenciaFile(string.Empty, salida, path))
                {
                    resultado = 1;
                }
                else
                {
                    //- No se ejecuto GAMS
                    resultado = -2;
                }
            }
            catch (Exception ex)
            {
                //- Ha ocurrido un error
                resultado = -1;
                throw new Exception(ex.Message, ex);
            }

            return resultado;
        }

        /// <summary>
        /// Permite ejecutar el modelo GAMS Alternativo
        /// </summary>
        /// <param name="fechaProceso"></param>
        /// <param name="path"></param>
        /// <param name="parametros"></param>
        /// <param name="salida"></param>
        /// <returns></returns>
        public static int EjecutarModeloAC(DateTime fechaProceso, string path, string parametros, string salida, string salida2, string salida3)
        {
            int resultado = -1;

            try
            {
                //- Declaraando los objetos para integracion con GAMS
                string XsystemDirectory = @"C:\GAMS\34";
                GAMSWorkspace ws = new GAMSWorkspace(null, XsystemDirectory);

                GAMSDatabase db = ws.AddDatabase();

                using (GAMSOptions opt = ws.AddOptions())
                {
                    //- Ejecutando el modelo

                    GAMSJob modelo = ws.AddJobFromString(ObtenerModeloAC(path + parametros, path + salida, path + salida2, path + salida3));
                    modelo.Run(opt, db);
                }

                //- Leyendo el archivo de resultaodo
                if (FileServer.VerificarExistenciaFile(string.Empty, salida, path))
                {
                    resultado = 1;
                }
                else
                {
                    //- No se ejecuto GAMS
                    resultado = -2;
                }
            }
            catch (Exception ex)
            {
                //- Ha ocurrido un error
                resultado = -1;
                throw new Exception(ex.Message, ex);
            }

            return resultado;
        }


        /// <summary>
        /// Obtiene el texto del modelo GAMS
        /// </summary>
        /// <param name="parametros"></param>
        /// <param name="salida"></param>
        /// <returns></returns>
        public static string ObtenerModelo(string parametros, string salida, string salida2, string pendiente)
        {
            #region Model

            string result = @"
option qcp=cplex;
option nlp=knitro;

$INCLUDE " + parametros + @" 
file salida /" + salida + @"/;
file salida2 /" + salida2 + @"/;

Alias (NB,i), (NB,j), (NB,k);
Alias (GT,t), (GH,h);
Alias (ENL,l),(CONG,c);
Alias (CONG_AS_ARRIBA,ra),(CONG_AS_ABAJO,rb);

SCALAR

 Pbase /100.0/
 PI /3.1416/
 Pendiente /" + pendiente + @"/
 CRacionamiento /1000/
 Tolerancia /0.05/
 Variacion  /1/ 
 Variacion2 /0.15/;

PARAMETER
* Generales
  CostoFijo,esta,  error  ,iter ,msg1,msg2
* Para enlaces
  EnlG(l),
  EnlB(l),  LossTF(l), LossLF(l),PerdT(i), LossTotalL
* Para barras
  Dem(i), Falla(i),Falla1(i),Falla2(i), FallaTotal
* Captura de resultados
  Phdc(h),Phdirec(h),Ptdc(t),Ptdirec(t),  F0(l),  LossLF0(l),  costo0,  LossTotalL0 ,costo0
                 F1(l) , cmg0(i)
                 Ptmin(t),Ptmax(t),Phmin(h), Phmax(h)
* Para shift factors
  LF(i),    LFx(i)
  SF(l,i),  SFc(c,i), SFx(l,i), SFcx(c,i)
* Para shift factors  (Regiones de seguridad)
  SFra(ra,i),  SFrb(rb,i)
  SFrax(ra,i),  SFrbx(rb,i)

  D(i)
  W(i)
  Offset

* Para costos marginales descompuestos
  Tau,  CMGenergia(i),  CMGcongestion(i),  CMGsf(i)
* Para calculo Shift Factors
  ybus(i,j),  zbus(i,j)
* Para detectar si obtuvo congestion
  congestion(l),  congestionc(c)
  congestionra(ra), congestionrb(rb)
  ValETotal, ValRTotal
  ValEEnergia,ValREnergia
  ValECong , ValRCong ,ValRCongPositivo
* Para determinar compensaciones
  CV(t), Compensa(t)
  p1, p2, p1max
;
VARIABLES
  costo, F(l),  Ang(i)
  positive variable Ph(h), Pt(t), Pt1(t), Pt2(t), RacP(i);
  positive variable Loss;
  positive variable LossLFx(l);
* Costo que ya de por si se esta incurriendo por tener a las unidades prendidas
  CostoFijo = sum{t,PtData( t,'Costo1') } ;
* Calculos previos de admitancias y perdidas transversales
  EnlB[l]   = -FData(l,'X0')/ (FData(l,'R0')*FData(l,'R0') +FData(l,'X0')*FData(l,'X0'));
  EnlG[l]   =  FData(l,'R0')/ (FData(l,'R0')*FData(l,'R0') +FData(l,'X0')*FData(l,'X0') );
  LossTF[l] =  FData(l,'G0')*Pbase;
  PerdT[i] =  sum{ (l,j)$(FBus(l,i,j)), 0.5*LossTF[l]}+sum{ (l,j)$(FBus(l,j,i)), 0.5*LossTF[l]};

  Falla(i)=0;
  Falla1(i)=0;
  Falla2(i)=0;
  Pt.up(t)=PtData( t,'Pmax');   Pt.lo(t)=PtData( t,'Pmin'); Pt.l(t)=PtData(t,'Pgen');
  Ph.up(h)=PhData( h,'Pmax');   Ph.lo(h)=PhData( h,'Pmin'); Ph.l(h)=PhData(h,'Pgen');
  Pt1.up(t)=PtData( t,'Pmax1')-PtData( t,'Pmin');  Pt1.lo(t)=0;   Pt1.l(t)=Pt.l(t)-PtData( t,'Pmin');
  Pt2.up(t)=PtData( t,'Pmax2')-PtData( t,'Pmax1'); Pt2.lo(t)=0;   Pt2.l(t)=0;

  Pt.up(t)= PtData( t,'Pmax')$(PtData( t,'Pmax')<=PtData(t,'Pgen')*(1+Variacion)) + (PtData(t,'Pgen')*(1+Variacion))$(PtData( t,'Pmax')>PtData(t,'Pgen')*(1+Variacion));
  Pt.lo(t)= PtData( t,'Pmin')$(PtData( t,'Pmin')>=PtData(t,'Pgen')*(1-Variacion)) + (PtData(t,'Pgen')*(1-Variacion))$(PtData( t,'Pmin')<PtData(t,'Pgen')*(1-Variacion));
  Ph.up(h)= PhData( h,'Pmax')$(PhData( h,'Pmax')<=PhData(h,'Pgen')*(1+Variacion)) + (PhData(h,'Pgen')*(1+Variacion))$(PhData( h,'Pmax')>PhData(h,'Pgen')*(1+Variacion));
  Ph.lo(h)= PhData( h,'Pmin')$(PhData( h,'Pmin')>=PhData(h,'Pgen')*(1-Variacion)) + (PhData(h,'Pgen')*(1-Variacion))$(PhData( h,'Pmin')<PhData(h,'Pgen')*(1-Variacion));
  Ptmin(t)=Pt.lo(t); Ptmax(t)=Pt.up(t); Phmin(h)=Ph.lo(h); Phmax(h)=Ph.up(h);

  RacP.up(i)=inf;   RacP.lo(i)=0;    RacP.l(i)= 0;
  Ang.up(i)= pi;    Ang.lo(i)=-pi;
  esta=0; loop(h, if(esta=0,  loop(j$(PhBus(h,j)),Ang.fx[j]=0; ); esta=1;); );
  Dem(i)=Demanda(i,'Pc')+PerdT(i) ;

  display Ang.l,EnlB;
***********************************************************************************
*  Shift Factors (SF):  Incremento de flujo en Enlace L ante incrementos de generacion en barra I
***********************************************************************************
    ybus(i,j)=0;
    loop(l, loop((i,j)$FBus(l,i,j),ybus[i,j]=ybus[i,j]+EnlB[l];ybus[j,i]=ybus[j,i]+EnlB[l];ybus[i,i]=ybus[i,i]-EnlB[l];ybus[j,j]=ybus[j,j]-EnlB[l]));
    esta=0; loop(h, if(esta=0,  loop(k$(PhBus(h,k)),ybus(k,i)=0;ybus(i,k)=0;ybus(k,k)=1e20;);  esta=1;  ) );


    execute_unload 'zbus.gdx',i,zbus;
    execute_unload 'ybus.gdx',i,ybus;
    execute 'invert.exe ybus.gdx i ybus zbus.gdx zbus';
    execute_load 'zbus.gdx',zbus;

    display ybus,zbus;

    esta=0; loop(h, if(esta=0,  loop(k$(PhBus(h,k)),zbus(k,k)=0;);  esta=1;  ) );

    loop(k, loop(l,  SFx[l,k]= EnlB[l]*sum{ (i,j)$FBus(l,i,j)  ,-(zbus[i,k] - zbus[j,k])} );
            loop(c $CONGMax(c,'Cong'),               SFcx[c,k]=sum{l$(CONGRuta(c,l) ),   SFx[l,k]} );
            loop(ra $CONGmax_AS_ARRIBA(ra,'Cong'), SFrax[ra,k]=sum{l$(CONGRuta_AS_ARRIBA_ENL(ra,l) ),   SFx[l,k]} );                                                                             );
            loop(rb $CONGmax_AS_ABAJO(rb,'Cong'), SFrbx[rb,k]=sum{l$(CONGRuta_AS_ABAJO_ENL(rb,l) ),   SFx[l,k]} );





*  Inicializar angulos Ang(i)
    loop(i,    Ang.l(i)=  0.01*sum{k, -zbus(i,k)* (sum{ (t)$(PtBus(t,k)),Pt.l[t]} +   sum{ (h)$(PhBus(h,k)),Ph.l[h]}  - Dem(k) )  } );
display Ang.l;

    Ang.l(i)= 0;

parameter SFJOSE;
SFJOSE(l)= sum(k,SFx(l,k));
display SFx,Ang.l,SFJOSE;

positive variables
phup, phdw;

EQUATIONS
*** Comunes para DC y SF
  Operacion,
  PotTer[t],TerFor[t], HidFor[h],
  EnlCongMax[l],  EnlCongMin[l],  CongesMax[c],   CongesMin[c],
  EnlCongMax2[l], EnlCongMin2[l], CongesMax2[c],  CongesMin2[c],


***** Ecuaciones de regiones de seguridad
  Region1CongArribaMax[ra], Region1CongArribaMin[ra],
  Region1CongAbajoMax[rb], Region1CongAbajoMin[rb],
  Region1CongArribaMax2[ra], Region1CongArribaMin2[ra],
  Region1CongAbajoMax2[rb], Region1CongAbajoMin2[rb],

*** Para metodo DC iterativo y con perdidas cuadraticas
  Flujo[l],  Perdidas[l], Balance1[i],Balance2[i]
*** Para metodo con SF
  EcBalance, EcEnergia, FlujoSF[l]
  EcPerdidas
;

* Funcion objetivo minimizar costo
Operacion..
  costo =e=
     sum{i,CRacionamiento*RacP[i] }
   + sum{t, PtData(t,'CI1')*Pt1(t)}
   + sum{t, PtData(t,'CI2')*Pt2(t)}
   + sum{h, PhData(h,'CI')*( Pendiente + 0.5*(1-Pendiente)* (Ph(h)/(PhData(h,'Pgen'))) )*Ph(h) }

*   + sum{h, PhData(h,'CI')*( Ph(h) + 0.5*(1-Pendiente)* (Ph(h)-PhData(h,'Pgen'))*(Ph(h)-PhData(h,'Pgen')) /(PhData(h,'Pgen'))  ) }

*+ sum{h, PhData(h,'CI')*Ph(h)*(Pendiente + 0.5*(1-Pendiente)* ( (Ph(h)/PhData(h,'Pgen')) $(Ph(h) ge PhData(h,'Pgen'))+(2-(Ph(h)/PhData(h,'Pgen')))$(Ph(h) lt PhData(h,'Pgen')) )  )}
*   + sum(h,PhData(h,'CI')*PhData(h,'Pgen')) + sum( h,PhData(h,'CI')*(1+0.5*(1-Pendiente)*(phup(h)/PhData(h,'Pgen')) )*phup(h))+ sum( h,PhData(h,'CI')*(1+0.5*(1-Pendiente)*(phdw(h)/PhData(h,'Pgen')) )*phdw(h))
     ;
* Para hidraulicas el Costo Incremental  = CV*(Pendiente + (1-Pendiente) * (H / HidGen) )
* por tanto como Costo Total para FObj   = CV*Pendiente*P + CV*((1-Pendiente)/2) * (H**2 / HidGen)


** Ecuaciones comunes para flujo DC y Shift Factors
PotTer[t]..                          Pt[t]=e=  PtData( t,'Pmin')+Pt1[t]+Pt2[t];
TerFor[t]$(PtData(t,'Forzada'))..    Pt[t]=e=  PtData(t,'Pgen');
HidFor[h]$(PhData(h,'Forzada'))..    Ph[h]=e=  PhData(h,'Pgen');

EnlCongMax[l]$(FData(l,'Cong'))..    F[l] =l=  FData(l,'Pmax')-0.5*LossTF[l]-0.5*LossLF[l];
EnlCongMin[l]$(FData(l,'Cong'))..   -F[l] =l=  FData(l,'Pmax')-0.5*LossTF[l]-0.5*LossLF[l];
CongesMax [c]$(CONGMax(c,'Cong'))..  sum(l$(CONGRuta(c,l)), F[l]+0.5*LossTF[l]+0.5*LossLF[l])  =l=  CONGMAX(c,'Pmax');
CongesMin [c]$(CONGMax(c,'Cong'))..  sum(l$(CONGRuta(c,l)),-F[l]+0.5*LossTF[l]+0.5*LossLF[l])  =l=  CONGMAX(c,'Pmax');

EnlCongMax2[l]$(FData(l,'Cong'))..    F[l] =l=  FData(l,'Pmax')-0.5*LossTF[l]-0.5*LossLFx[l];
EnlCongMin2[l]$(FData(l,'Cong'))..   -F[l] =l=  FData(l,'Pmax')-0.5*LossTF[l]-0.5*LossLFx[l];
CongesMax2 [c]$(CONGMax(c,'Cong'))..  sum(l$(CONGRuta(c,l)), F[l]+0.5*LossTF[l]+0.5*LossLFx[l])  =l=  CONGMAX(c,'Pmax');
CongesMin2 [c]$(CONGMax(c,'Cong'))..  sum(l$(CONGRuta(c,l)),-F[l]+0.5*LossTF[l]+0.5*LossLFx[l])  =l=  CONGMAX(c,'Pmax');


**************Restricciones por regiones de seguridad*********************

*Hacia arriba
Region1CongArribaMax[ra]$(CONGmax_AS_ARRIBA(ra,'Cong'))..    CONGmax_AS_ARRIBA(ra,'m')*sum{l$(CONGRuta_AS_ARRIBA_ENL(ra,l)), F[l]+0.5*LossTF[l]+0.5*LossLF[l]}
                                                    +CONGmax_AS_ARRIBA(ra,'b')=l=sum{h$(CONGRuta_AS_ARRIBA_GH(ra,h)),Ph[h]};

***Region1CongArribaMin[ra]$(CONGmax_AS_ARRIBA(ra,'Cong'))..    CONGmax_AS_ARRIBA(ra,'m')*sum{l$(CONGRuta_AS_ARRIBA_ENL(ra,l)), -F[l]+0.5*LossTF[l]+0.5*LossLF[l]}
***                                                    +CONGmax_AS_ARRIBA(ra,'b')=l=sum{h$(CONGRuta_AS_ARRIBA_GH(ra,h)),Ph[h]};


*Hacia abajo
Region1CongAbajoMax[rb]$(CONGmax_AS_ABAJO(rb,'Cong'))..    CONGmax_AS_ABAJO(rb,'m')*sum{l$(CONGRuta_AS_ABAJO_ENL(rb,l)), F[l]+0.5*LossTF[l]+0.5*LossLF[l]}
                                                    +CONGmax_AS_ABAJO(rb,'b')=g=sum{h$(CONGRuta_AS_ABAJO_GH(rb,h)),Ph[h]};

***Region1CongAbajoMin[rb]$(CONGmax_AS_ABAJO(rb,'Cong'))..    CONGmax_AS_ABAJO(rb,'m')*sum{l$(CONGRuta_AS_ABAJO_ENL(rb,l)), -F[l]+0.5*LossTF[l]+0.5*LossLF[l]}
***                                                    +CONGmax_AS_ABAJO(rb,'b')=g=sum{h$(CONGRuta_AS_ABAJO_GH(rb,h)),Ph[h]};


*Hacia arriba2
Region1CongArribaMax2[ra]$(CONGmax_AS_ARRIBA(ra,'Cong'))..    CONGmax_AS_ARRIBA(ra,'m')*sum{l$(CONGRuta_AS_ARRIBA_ENL(ra,l)), F[l]+0.5*LossTF[l]+0.5*LossLFx[l]}
                                                    +CONGmax_AS_ARRIBA(ra,'b')=l=sum{h$(CONGRuta_AS_ARRIBA_GH(ra,h)),Ph[h]};

***Region1CongArribaMin2[ra]$(CONGmax_AS_ARRIBA(ra,'Cong'))..    CONGmax_AS_ARRIBA(ra,'m')*sum{l$(CONGRuta_AS_ARRIBA_ENL(ra,l)), -F[l]+0.5*LossTF[l]+0.5*LossLFx[l]}
***                                                    +CONGmax_AS_ARRIBA(ra,'b')=l=sum{h$(CONGRuta_AS_ARRIBA_GH(ra,h)),Ph[h]};

*Hacia abajo2
Region1CongAbajoMax2[rb]$(CONGmax_AS_ABAJO(rb,'Cong'))..    CONGmax_AS_ABAJO(rb,'m')*sum{l$(CONGRuta_AS_ABAJO_ENL(rb,l)), F[l]+0.5*LossTF[l]+0.5*LossLFx[l]}
                                                    +CONGmax_AS_ABAJO(rb,'b')=g=sum{h$(CONGRuta_AS_ABAJO_GH(rb,h)),Ph[h]};

***Region1CongAbajoMin2[rb]$(CONGmax_AS_ABAJO(rb,'Cong'))..    CONGmax_AS_ABAJO(rb,'m')*sum{l$(CONGRuta_AS_ABAJO_ENL(rb,l)), -F[l]+0.5*LossTF[l]+0.5*LossLFx[l]}
***                                                    +CONGmax_AS_ABAJO(rb,'b')=g=sum{h$(CONGRuta_AS_ABAJO_GH(rb,h)),Ph[h]};

**************************************************************************



Flujo[l]..      F[l]/Pbase/EnlB[l]        =e= sum{ (i,j)$FBus(l,i,j)  ,(Ang[i] - Ang[j])};

** Ecuaciones para flujo DC calculo iterativo
Balance1[i]..   RacP[i] +sum{ (t)$(PtBus(t,i)),Pt[t] }+sum{ (h)$(PhBus(h,i)),Ph[h] }
              -sum{ (l,j)$(FBus(l,i,j)),F[l]*( 1+LossLF(l)/(2*F0(l)) ) }
              +sum{ (l,j)$(FBus(l,j,i)),F[l]*( 1-LossLF(l)/(2*F0(l)) ) }
              =e=   Dem(i);

** Ecuaciones para flujo DC perdidas cuadraticas
Balance2[i]..   RacP[i]+sum{ (t)$(PtBus(t,i)),Pt[t] }+sum{ (h)$(PhBus(h,i)),Ph[h] }
                -sum{ (l,j)$(FBus(l,i,j)),F[l]+0.5*LossLFx(l)  }
                +sum{ (l,j)$(FBus(l,j,i)),F[l]-0.5*LossLFx(l)  }
                =e=   Dem(i)-Falla[i] ;
Perdidas[l]..   LossLFx[l]   =e=  FData(l,'R0')/Pbase * F(l)*F(l);
*Perdidas[l]..   LossLFx[l]   =e=  2*EnlG[l]* sum((i,j)$FBus(l,i,j),(1-cos(Ang[i] - Ang[j])) );


** Ecuaciones para calculo de flujo con shift factors
EcBalance..     sum{i,( sum{t$(PtBus(t,i)),Pt(t)} + sum{h$(PhBus(h,i)), Ph(h)} - Dem(i))} -Loss =e= 0;
EcEnergia..     Loss -  sum{i, LF(i)*( sum{t$(PtBus(t,i)),Pt(t)} + sum{h$(PhBus(h,i)), Ph(h)} - Dem(i)) } + Offset =e= 0;
FlujoSF[l]..    F[l] =e= sum{ i, SF[l,i]*( sum{ (t)$(PtBus(t,i)), Pt(t) } + sum{ (h)$(PhBus (h,i)), Ph(h) } - Dem(i) - D(i)*Loss)} ;



MODEL DCIter    /Operacion,Flujo,Balance1,                PotTer,TerFor,HidFor,EnlCongMax,EnlCongMin,CongesMax,CongesMin,Region1CongArribaMax,Region1CongAbajoMax/;
MODEL DCDirec   /Operacion,Flujo,Balance2,Perdidas,       PotTer,TerFor,HidFor,EnlCongMax2,EnlCongMin2,CongesMax2,CongesMin2,Region1CongArribaMax2,Region1CongAbajoMax2/;
MODEL LmpSF     /Operacion,EcBalance, EcEnergia, FlujoSF ,PotTer,TerFor,HidFor,EnlCongMax,EnlCongMin,CongesMax,CongesMin,Region1CongArribaMax,Region1CongAbajoMax/;

*DCIter.OptFile=1;

**************************************************************************************************
*  Flujo DC con pérdidas iterativas: aproximación para Flujo DC pérdidas cuadráticas
**************************************************************************************************
    RacP.up(i)=inf;   RacP.lo(i)=0;   Falla(i)=0;

*   inicializacion de datos considerando los datos de scada
    costo0=1;
    Pt.l(t)=PtData(t,'Pgen'); Ph.l(h)=PhData(h,'Pgen');  Ang.l(i)=0;
    SF(l,i)=SFx(l,i);
    F.l[l] = sum{ i, SF[l,i]*( sum{ (t)$(PtBus(t,i)), Pt.l(t) } + sum{ (h)$(PhBus (h,i)), Ph.l(h) } - Dem(i))} ;
    LossLF[l] =  FData(l,'R0') * F.l(l)*F.l(l)/PBase;    LossTotalL=sum{(l),LossLF[l]};
    F0(l)=F.l(l)$(F.l(l) ne 0) + 1$(F.l(l) eq 0);

display LossLF,F0,Ang.l;

    repeat
            Solve DCIter using qcp minimzing costo;
            display  DCIter.modelstat,Balance1.m;
             ABORT$(DCIter.solvestat <> 1) 'ERROR FASE1: MODELO NO COMPLETO NORMALMENTE (TIEMPO O ITERACIONES)';

            LossLF[l] =  FData(l,'R0') * F.l(l)*F.l(l)/PBase; LossTotalL=sum{(l),LossLF[l]};
            F0(l)=F.l(l)$(F.l(l) ne 0) + 1$(F.l(l) eq 0);
            error=(costo.l-costo0)/costo0;
            costo0=costo.l;
    until (error <Tolerancia);

    Phdc(h)=Ph.l(h);  Ptdc(t)=Pt.l(t);
    LossLFx.l[l]=LossLF(l);
    display  RacP.l,LossLFx.l;
***********************************************************************************
*  Flujo metodo DC con perdidas cuadráticas
***********************************************************************************
     Solve DCDirec using nlp minimizing costo;
     ABORT$(DCDirec.modelstat <> 2) 'ERROR FASE2: SOLUCION NO FACTIBLE)';

     display  RacP.l,LossLFx.l,Balance2.m;

*   Eliminar generacion por holgura en caso hubiera
*    Falla[i]=RacP.l[i]*1.001; RacP.fx(i)=0; FallaTotal=sum{(i),Falla[i]};

*   Eliminar generacion por holgura en caso hubiera. Se incluye e-3 al parámetro falla, en caso el error fuera menor a e-3.

    Falla[i]=RacP.l[i]*1.01;Falla1[i]=RacP.l[i]*1.01; Falla2[i]=Falla1[i]-RacP.l[i];RacP.fx(i)=0; FallaTotal=sum{(i),Falla1[i]};
    loop(i$((Falla2[i] lt 0.001)and(Falla2[i]gt 0)),Falla[i]=(Falla1[i]+0.01));
    loop(i$(Falla2[i] ge 0.001),Falla[i]=Falla1[i]);


*    Falla[i]=RacP.l[i]*1.001+0.001$(RacP.l[i]*0.001<0.001 and RacP.l[i]>0)  ;RacP.fx(i)=0; FallaTotal=sum{(i),Falla[i]};

display Falla;

*    DCDirec.optfile=1;

    if ((FallaTotal gt 0),  SOLVE DCDirec USING nlp MINIMIZING costo;);

   display DCDirec.Modelstat;
   display DCDirec.Solvestat,Balance2.m;

   ABORT$(DCDirec.modelstat <> 2) 'ERROR FASE3: SOLUCION NO FACTIBLE)';
*ABORT$(DCDirec.modelstat<>1 or DCDirec.modelstat<>2 or DCDirec.modelstat<>7) 'ERROR FASE3: SOLUCION NO FACTIBLE)';
*not (msg1<>1 or msg1<>2 or msg1<>7)
   ABORT$(DCDirec.solvestat <> 1) 'ERROR FASE3: MODELO NO COMPLETO NORMALMENTE (TIEMPO O ITERACIONES)';
*   ABORT$(DCDirec.solvestat <> 4);


    LossTotalL=sum{(l),LossLFx.l[l]};

    D[i]= (sum{(l,j)$(FBus(l,i,j)), 0.5*LossLFx.l[l]}+ sum{(l,j)$(FBus(l,j,i)), 0.5*LossLFx.l[l]})/LossTotalL;

*   Capturar valores resultados de flujo DC cuadratico
    LossLF0[l]=LossLFx.l[l]; LossTotalL0=LossTotalL; F0(l)=F.l(l);   costo0=costo.l;   F0(l)=F.l(l);
    Phdirec(h)=Ph.l(h);   Ptdirec(t)=Pt.l(t);
    cmg0(i)=Balance2.m(i);

display Falla,RacP.l,LossTotalL;

***********************************************************************************
*  Flujo metodo Shift Factors
***********************************************************************************
*   Eliminar generacion por holgura en caso hubiera
    Dem(i)=Demanda(i,'Pc')+PerdT(i) - Falla[i];
    LossLF(l)=LossLFx.l(l);
    Loss.l=LossTotalL;
    W(i)=D(i);
    SF(l,i)=SFx(l,i)-sum{(j),W(j)*(SFx[l,j])} ;
    SFc(c,i)=SFcx(c,i)-sum{(j),W(j)*(SFcx[c,j])};
**********************Region de seguridad**********************
    SFra(ra,i)=SFrax(ra,i)-sum{(j),W(j)*(SFrax[ra,j])} ;
    SFrb(rb,i)=SFrbx(rb,i)-sum{(j),W(j)*(SFrbx[rb,j])} ;
***************************************************************
    LFx(i)= sum(l, 2*FData(l,'R0') *SFx(l,i) *F.l(l)/Pbase  );
    LF(i)=(LFx(i)-sum{j,W[j]*LFx[j]})/(1-sum{j,W[j]*LFx[j]});



    Offset = - LossTotalL +  sum{i, LF(i)*( sum{t$(PtBus(t,i)),Pt.l(t)} + sum{h$(PhBus(h,i)), Ph.l(h)} - Dem(i)) };



  loop( t$(not(PtData(t,'Forzada'))),  Pt.up(t)= PtData( t,'Pmax')$(PtData( t,'Pmax')<=Ptdirec(t)*(1+Variacion2)) + (Ptdirec(t)*(1+Variacion2))$(PtData( t,'Pmax')>Ptdirec(t)*(1+Variacion2)));
  loop( t$(not(PtData(t,'Forzada'))),  Pt.lo(t)= PtData( t,'Pmin')$(PtData( t,'Pmin')>=Ptdirec(t)*(1-Variacion2)) + (Ptdirec(t)*(1-Variacion2))$(PtData( t,'Pmin')<Ptdirec(t)*(1-Variacion2)));
  loop( h$(not(PhData(h,'Forzada'))),  Ph.up(h)= PhData( h,'Pmax')$(PhData( h,'Pmax')<=Phdirec(h)*(1+Variacion2)) + (Phdirec(h)*(1+Variacion2))$(PhData( h,'Pmax')>Phdirec(h)*(1+Variacion2)));
  loop( h$(not(PhData(h,'Forzada'))),  Ph.lo(h)= PhData( h,'Pmin')$(PhData( h,'Pmin')>=Phdirec(h)*(1-Variacion2)) + (Phdirec(h)*(1-Variacion2))$(PhData( h,'Pmin')<Phdirec(h)*(1-Variacion2)));

*   option nlp=snopt;
    option nlp=ipopt;

    Solve LmpSF using nlp minimzing costo;

parameter SFJOSE2,SFJOSE3;
SFJOSE2(l)= sum(i,SF(l,i));
SFJOSE3(l)=SF(l,'B077');
display SFJOSE2,LF,SFJOSE3;

parameter Ecenergy;
Ecenergy= Loss.l -  sum{i, LF(i)*( sum{t$(PtBus(t,i)),Pt.l(t)} + sum{h$(PhBus(h,i)), Ph.l(h)} - Dem(i)) } + Offset;


    display offset,EcBalance.m,Ecenergy;

    display DCDirec.Modelstat;
    display DCDirec.Solvestat;

    msg1=LmpSF.modelstat;
    ABORT$( not (msg1<>1 or msg1<>2 or msg1<>7) ) 'ERROR FASE4: SOLUCION NO FACTIBLE)';

    msg2=LmpSF.Solvestat;
    ABORT$( not (msg2 = 1 or (msg1=7 and msg2=4))) 'ERROR FASE4: MODELO NO COMPLETO NORMALMENTE (TIEMPO O ITERACIONES)';


***********************************************************************************
*  Determinar componentes de costos marginales
***********************************************************************************
   congestion(l)=0; congestionc(c)=0; congestionra(ra)=0; congestionrb(rb)=0;
   loop(l$(FData(l,'Cong'))  , congestion(l)=1$(EnlCongMax.m(l)- EnlCongMin.m(l))<>0);
   loop(c$(CONGMax(c,'Cong')), congestionc(c)=1$(CongesMax.m(c) - CongesMin.m(c))<>0);
   loop(ra$(CONGmax_AS_ARRIBA(ra,'Cong')), congestionra(ra)=1$(Region1CongArribaMax.m(ra))<>0);
   loop(rb$(CONGmax_AS_ABAJO(rb,'Cong')), congestionrb(rb)=1$(Region1CongAbajoMax.m(rb))<>0);

   Tau = EcEnergia.m;
   CMGenergia(i)=Tau*(1-LF(i));
   CMGcongestion(i)= sum{ (l)$(FData(l,'Cong'))  , (EnlCongMax.m(l)- EnlCongMin.m(l))*(SF [l,i]- sum{j,D(j)*(SF [l,j])}) }
                   + sum{ (c)$(CONGMax(c,'Cong')), (CongesMax.m(c) - CongesMin.m(c))*(SFc[c,i]- sum{j,D(j)*(SFc[c,j])}) }
                   + sum{ (ra)$(CONGmax_AS_ARRIBA(ra,'Cong')) ,(Region1CongArribaMax.m(ra))*(SFra[ra,i]*CONGmax_AS_ARRIBA(ra,'m')-sum{j,D(j)*(SFra[ra,j])})}
                   + sum{ (rb)$(CONGmax_AS_ABAJO(rb,'Cong')) , (Region1CongAbajoMax.m(rb))*(-SFrb[rb,i]*CONGmax_AS_ABAJO(rb,'m')+sum{j,D(j)*(SFrb[rb,j])})};

   CMGsf(i)= CMGenergia(i)+CMGcongestion(i);
   display Tau;
   display EcBalance.m;


   display CMGsf,CMGenergia,CMGcongestion,Falla,RacP.l;
***********************************************************************************
*  Valorizar  y reportes
***********************************************************************************
*$INCLUDE \\coes.org.pe\Areas\str\COSTOMARGINAL\GAMS\modelo5\valorizaciones.gms

***********************************************************************************
*  Determinar valorizaciones
***********************************************************************************
   ValETotal   = sum{i, CMGsf(i)*( sum{ (t)$(PtBus(t,i)),PtData(t,'Pgen')} +sum{ (h)$(PhBus(h,i)),PhData(h,'Pgen')}) };
   ValRTotal   = sum{i, CMGsf(i)*( Demanda(i,'Pc') )  };
   ValEEnergia = sum{i, CMGenergia(i)*( sum{ (t)$(PtBus(t,i)),PtData(t,'Pgen')} +sum{ (h)$(PhBus(h,i)),PhData(h,'Pgen')}) };
   ValREnergia = sum{i, CMGenergia(i)*( Demanda(i,'Pc') )  };
   ValECong    = sum{i, CMGcongestion(i)*( sum{ (t)$(PtBus(t,i)),PtData(t,'Pgen')} +sum{ (h)$(PhBus(h,i)),PhData(h,'Pgen')})  };
   ValRCong    = sum{i, CMGcongestion(i)*( Demanda(i,'Pc'))  };   ValRCongPositivo   = sum{i$(CMGcongestion(i) gt 0),CMGcongestion(i)*( Demanda(i,'Pc') )  };

   loop(t, if(PtData(t,'Pgen') ne 0,
              p2=0;  p1=PtData(t,'Pgen')- PtData( t,'Pmin'); p1max=PtData( t,'Pmax1')- PtData( t,'Pmin');
              if (p1>p1max, p2=p1-p1max; p1=p1max;);
              CV(t)=(PtData( t,'Costo1')+ PtData(t,'CI1')*p1+ PtData(t,'CI2')*p2)/PtData(t,'Pgen'))  );
   loop((t,i)$(PtBus(t,i)), if (  (CV(t)-CMGsf(i) gt 0), Compensa(t)=PtData(t,'Pgen')*0.5*(CV(t)-CMgsf(i))   ));

************

*$INCLUDE \\coes.org.pe\Areas\str\COSTOMARGINAL\GAMS\modelo5\salida1.gms

salida.pc=5;
put salida;

put 'Costo Total',(costo.l+CostoFijo):10:4/;
put /'Congestion'/;
put '----------'/;
put 'Enlace','Limite','Envio','Recepcion','Congestion'/;
loop(l$(FData(l,'Cong')),put l.te(l),FData(l,'Pmax'):8:3;
          put (F.l(l)+0.5*LossLFx.l(l)+0.5*LossTF[l]):8:3;
          put (F.l(l)-0.5*LossLFx.l(l)-0.5*LossTF[l]):8:3;
          put congestion(l)/);

put /'Ruta en congestion'/;
put '------------------'/;
put 'Barras 1','Limite','Envio','Recepcion','Congestion'/;
loop(c$(CONGMAX(c,'Cong')),put c.te(c), CONGMAX(c,'Pmax'):8:3;
          put (sum{l$(CONGRuta(c,l) ),  F.l[l]+0.5*LossLFx.l[l]+0.5*LossTF[l]}):8:3;
          put (sum{l$(CONGRuta(c,l) ),  F.l[l]-0.5*LossLFx.l[l]-0.5*LossTF[l]}):8:3
          put congestionc(c)/);
********************************************************************************************************************

put /'Congestion Region de Seguridad (arriba)'/;
put '------------------'/;
put 'Barras 1','Limite','Envio','Recepcion','Congestion','GenLimite','Generacion'/;
loop(ra$(CONGmax_AS_ARRIBA(ra,'Cong')),put ra.te(ra), CONGmax_AS_ARRIBA(ra,'FlujoT'):8:3;
          put (sum{l$(CONGRuta_AS_ARRIBA_ENL(ra,l) ),  F.l[l]+0.5*LossLFx.l[l]+0.5*LossTF[l]}):8:3;
          put (sum{l$(CONGRuta_AS_ARRIBA_ENL(ra,l) ),  F.l[l]-0.5*LossLFx.l[l]-0.5*LossTF[l]}):8:3
          put congestionra(ra)
          put CONGmax_AS_ARRIBA(ra,'GenT'):8:3
          put (sum{h$(CONGRuta_AS_ARRIBA_GH(ra,h) ), Ph.l[h]}):8:3/);
********************************************************************************************************************
********************************************************************************************************************

put /'Congestion Region de Seguridad (abajo)'/;
put '------------------'/;
put 'Barras 1','Limite','Envio','Recepcion','Congestion','GenLimite','Generacion'/;
loop(rb$(CONGmax_AS_ABAJO(rb,'Cong')),put rb.te(rb), CONGmax_AS_ABAJO(rb,'FlujoT'):8:3;
          put (sum{l$(CONGRuta_AS_ABAJO_ENL(rb,l) ),  F.l[l]+0.5*LossLFx.l[l]+0.5*LossTF[l]}):8:3;
          put (sum{l$(CONGRuta_AS_ABAJO_ENL(rb,l) ),  F.l[l]-0.5*LossLFx.l[l]-0.5*LossTF[l]}):8:3
          put congestionrb(rb)
          put CONGmax_AS_ABAJO(rb,'GenT'):8:3
          put (sum{h$(CONGRuta_AS_ABAJO_GH(rb,h) ), Ph.l[h]}):8:3/);
********************************************************************************************************************


put /'Descomposicion Costos Marginales'/;
put '--------------------------------'/;
put 'Barra','Energia','Congestion','Total' /;
loop(i$(   sum{ (l,j)$FBus(l,j,i),FData(l,'Sist')} + sum{ (l,j)$FBus(l,i,j),FData(l,'Sist')}    ),
*loop(i,
           put i.te(i);
           put CMGenergia(i):15:6;
           put CMGcongestion(i):15:6;
           put CMGsf(i):15:6;
           put /;);

**************** ADICIONAL

*$INCLUDE \\coes.org.pe\Areas\str\COSTOMARGINAL\GAMS\modelo5\salida2.gms

salida2.pc=5;
put salida2;
put /'Congestion'/;
put '----------'/;
put 'Enlace','Limite','Envio','Recepcion','Congestion'/;
loop(l$(FData(l,'Cong')),put l.te(l),FData(l,'Pmax'):8:3;
          put (F.l(l)+0.5*LossLFx.l(l)+0.5*LossTF[l]):8:3;
          put (F.l(l)-0.5*LossLFx.l(l)-0.5*LossTF[l]):8:3;
          put congestion(l)/);

put ''/;

put /'Ruta en congestion'/;
put '------------------'/;
put 'Barras 1','Limite','Envio','Recepcion','Congestion'/;
loop(c$(CONGMAX(c,'Cong')),put c.te(c), CONGMAX(c,'Pmax'):8:3;
          put (sum{l$(CONGRuta(c,l) ),  F.l[l]+0.5*LossLFx.l[l]+0.5*LossTF[l]}):8:3;
          put (sum{l$(CONGRuta(c,l) ),  F.l[l]-0.5*LossLFx.l[l]-0.5*LossTF[l]}):8:3
          put congestionc(c)/);

********************************************************************************************************************
put /'Congestion Region de Seguridad (arriba)'/;
put '------------------'/;
put 'Barras 1','Limite','Envio','Recepcion','Congestion','GenLimite','Generacion'/;
loop(ra$(CONGmax_AS_ARRIBA(ra,'Cong')),put ra.te(ra), CONGmax_AS_ARRIBA(ra,'FlujoT'):8:3;
          put (sum{l$(CONGRuta_AS_ARRIBA_ENL(ra,l) ),  F.l[l]+0.5*LossLFx.l[l]+0.5*LossTF[l]}):8:3;
          put (sum{l$(CONGRuta_AS_ARRIBA_ENL(ra,l) ),  F.l[l]-0.5*LossLFx.l[l]-0.5*LossTF[l]}):8:3
          put congestionra(ra)
          put CONGmax_AS_ARRIBA(ra,'GenT'):8:3
          put (sum{h$(CONGRuta_AS_ARRIBA_GH(ra,h) ), Ph.l[h]}):8:3/);

********************************************************************************************************************
********************************************************************************************************************

put /'Congestion Region de Seguridad (abajo)'/;
put '------------------'/;
put 'Barras 1','Limite','Envio','Recepcion','Congestion','GenLimite','Generacion'/;
loop(rb$(CONGmax_AS_ABAJO(rb,'Cong')),put rb.te(rb), CONGmax_AS_ABAJO(rb,'FlujoT'):8:3;
          put (sum{l$(CONGRuta_AS_ABAJO_ENL(rb,l) ),  F.l[l]+0.5*LossLFx.l[l]+0.5*LossTF[l]}):8:3;
          put (sum{l$(CONGRuta_AS_ABAJO_ENL(rb,l) ),  F.l[l]-0.5*LossLFx.l[l]-0.5*LossTF[l]}):8:3
          put congestionrb(rb)
          put CONGmax_AS_ABAJO(rb,'GenT'):8:3
          put (sum{h$(CONGRuta_AS_ABAJO_GH(rb,h) ), Ph.l[h]}):8:3/);

********************************************************************************************************************


put ''/;

put  /'Resumen'/;
put  '-------'/;
put 'Generacion Termica',   (sum{t, Pt.l(t)}):8:3/;
put 'Generacion Hidraulica',   (sum{h, Ph.l(h)}):8:3/;
put 'Demanda Total',   (sum{i,Demanda(i,'Pc')}):8:3/;
put 'Falla',   (sum{i,Falla(i)}):8:7/;
put 'Perdidas Longitudinales',   Loss.l:8:3/;
put 'Perdidas Transversales',   (sum{i,PerdT (i)}):8:3/;
put 'racionamiento',   (sum{i,RacP.l(i)}):8:7/;
put ''/;

put 'GENERACION NO FORZADA'/;
put '---------------------'/;
put ''/;

put '','Scada','Ph dc-lin''Ph dc-cuad','Ph SF','Dif.','Dif.','Pmin','Pmax','CI scada','CI dc-cuad','Cmg dc-cuad','CI SF','Cmg SF','Cmg Energia','Cmg Cong','LF','CImlf SF','','Pmax-PhSF','PhSF-Pmin','CMg Energia - perdidas'/;
put '','A','B''C','D','D-C','D-A'/;
loop( h$(not(PhData(h,'Forzada'))),put h.te(h),
       PhData(h,'Pgen'):8:3,
       Phdc(h):8:3,
       Phdirec(h):8:3,
       Ph.l[h]:8:3,
       (Ph.l[h]-Phdirec(h)):8:3,
       (Ph.l[h]-PhData(h,'Pgen')):8:3,
       Phmin(h):8:3,
       Phmax(h):8:3,
       PhData(h,'CI'):8:4,
       (PhData(h,'CI')*(Pendiente+(1-Pendiente)*(Phdirec[h]/(PhData(h,'Pgen'))))):8:4,
       (sum{i$(PhBus(h,i)), cmg0(i)}):12:4,
       (PhData(h,'CI')*(Pendiente+(1-Pendiente)*(Ph.l[h]/(PhData(h,'Pgen'))))):8:4,
       (sum{i$(PhBus(h,i)), CMGsf(i)}):12:4,
       (sum{i$(PhBus(h,i)), CMGenergia(i)}):12:4,
       (sum{i$(PhBus(h,i)), CMGcongestion(i)}):12:4,
       (sum{i$(PhBus(h,i)), LF(i)}):12:8,
       (sum{i$(PhBus(h,i)), (1/(1-LF(i)))*PhData(h,'CI')}):8:4,
       '',
       (Phmax(h)-Ph.l[h]):8:3,
       (Ph.l[h]-Phmin(h)):8:3,
       (sum{i$(PhBus(h,i)), CMGenergia(i)/(1- LF(i))}):12:4
        /);
put ''/;

put '','Scada','Pt dc-lin','Pt dc-cuad','PtSF','Dif.','Dif.','Pmin','Pmax','CI1','CI2','Cmg SF','Cmg Energia','Cmg Cong','LF','CI1mlf SF','CI2mlf SF','','Pmax-PtSF','PtSF-Pmin','CMg Energia - perdidas'/;
put '','A','B''C','D','D-C','D-A'/;
loop( t,put t.te(t),
       PtData(t,'Pgen'):8:3,
       Ptdc(t):8:3,
       Ptdirec(t):8:3,
       Pt.l[t]:8:3,
       (Pt.l[t]-Ptdirec(t)):8:3,
       (Pt.l[t]-PtData(t,'Pgen')):8:3,
       Ptmin(t):8:3,
       Ptmax(t):8:3,
       PtData(t,'CI1'):8:4,
       PtData(t,'CI2'):8:4,
       (sum{i$(PtBus(t,i)), CMGsf(i)}):12:4,
       (sum{i$(PtBus(t,i)), CMGenergia(i)}):12:4,
       (sum{i$(PtBus(t,i)), CMGcongestion(i)}):12:4,
       (sum{i$(PtBus(t,i)), LF(i)}):12:8,
       (sum{i$(PtBus(t,i)), 1/(1-LF(i))*PtData(t,'CI1')}):8:4,
       (sum{i$(PtBus(t,i)), 1/(1-LF(i))*PtData(t,'CI2')}):8:4,
       '',
       (Ptmax(t)-Pt.l[t]):8:3,
       (Pt.l[t]-Ptmin(t)):8:3,
       (sum{i$(PtBus(t,i)), CMGenergia(i)/(1-LF(i))}):12:4
        /);
put ''/;

put /'Generacion Termica'/;
put  '------------------'/;
put 'Termica','Pg MW','Scada','Calificacion'/;
loop(t,put t.te(t), Pt.l[t]:8:3,  PtData(t,'Pgen'):8:3, PtData(t,'Calif'):3:0/);
put ''/;

put /'Generacion Hidraulica'/;
put  '---------------------'/;
put 'Hidraulica','Ph MW','Scada'/;
loop( h,put h.te(h),Ph.l[h]:8:3,PhData(h,'Pgen'):8:3 /);
put ''/;

put  /'Falla'/;
put  '-------'/;
put 'Barra','Falla por barra'/;
loop( i$(Falla(i)),put i.te(i),Falla(i):8:7 /);
put ''/;

put  /'Racionamiento por barra'/;
put  '-------'/;
put 'Barra','Racionamiento por barra'/;
loop( i$(RacP.l(i)),put i.te(i),RacP.l(i):8:7 /);
put ''/;

put /'Valorizacion'/;
put '--------------'/;
put ' ','Entrega','Retiro','Retiro-Entrega'/;
put 'Total',ValETotal:10:2, ValRTotal:10:2,(ValRTotal-ValETotal):10:2/
put 'Energia',ValEEnergia:10:2, ValREnergia:10:2,(ValREnergia-ValEEnergia):10:2/
put 'Congestion', ValECong:10:2, ValRCong:10:2,(ValRCong-ValECong):10:2/
put 'Congestion Positiva',' ',ValRCongPositivo:10:2/
put ''/;

put /'Compensaciones Termicas'/;
put  '---------------------'/;
put 'Termica','Calificacion','Pg MW','CV','CMg','Compensacion'/;
loop(t,put t.te(t);
       put PtData(t,'Calif'):3:0;
       put PtData(t,'Pgen');
       put CV(t);
       put (       sum{i$(PtBus(t,i)),CMGsf(i)}         );
       put Compensa(t)/);

*Reporte de flujo y pérdidas finales

parameter LossFinal;
LossFinal(l) =FData(l,'R0')/Pbase*F.l[l]*F.l[l]

put /'Flujos'/;
put '------------------'/;
put '','MWe','MWr','MW perd'/;

loop(l,put l.te(l), (F.l[l]+LossFinal(l)*0.5):8:3, (F.l[l]-LossFinal(l)*0.5),LossFinal(l):8:3/);
            ";

            #endregion

            return result;
        }

        /// <summary>
        /// Permite obtener el modelo con nueva formulacion
        /// </summary>
        /// <param name="parametros"></param>
        /// <param name="salida"></param>
        /// <returns></returns>
        public static string ObtenerModeloAlternativo(string parametros, string salida, string salida2)
        {
            #region Modelo

            return @"            

option qcp=cplex;
option nlp=ipopt;

$INCLUDE " + parametros + @" 
file salida /" + salida + @"/;
file salida2 /" + salida2 + @"/;

**********************************************
* Definicion de variables
**********************************************
Alias (NB,i), (NB,j), (NB,k);
Alias (GT,t,t2), (GH,h,h2);
Alias (ENL,l),(CONG,c);

SCALAR

 Pbase /100.0/
 PI /3.1416/
 CRacionamiento /1000/
 Pendiente /1.00/
 deltaFL /0.01/;

PARAMETER
* Generales
  CostoFijo
* Para enlaces
  EnlG(l),  EnlB(l), LossLF(l), LossTF(l), PerdT(i), LossTotalL
* Para barras
  Dem(i), Falla(i), FallaTotal
* Limites
  Ptmin(t),Ptmax(t),Phmin(h), Phmax(h)  ,Ptgen(t),Phgen(h)
* Sensibilidades
  LF(i),LFx(i), SF(l,i),  SFc(c,i), SFx(l,i), SFcx(c,i),LFh(h),LFt(t)
* Distribucion
  D(i),  W(i)
* Balance
  Offset
* Costos para flujo de carga simple
  CT(t),CH(h)
* Para costos marginales descompuestos
  Tau,  CMGenergia(i),  CMGcongestion(i),  CMGtotal(i)
* Para calculo Shift Factors
  ybus(i,j),  zbus(i,j)
* Para determinar valorizaciones
  ValETotal, ValRTotal,  ValEEnergia,ValREnergia, ValECong , ValRCong ,ValRCongPositivo
* Para determinar compensaciones
  CV(t), Compensa(t)
  p1, p2, p1max

* auxiliares
  esta,potgrande
  Phdc(h),Ptdc(t),Fdc(l)
  Flmax(l),Fcmax(c)
  Phmin(h),Phmax(h),Ptmin(t),Ptmax(t)
  variah(h),variat(t)
  Ptaux(t)
;
VARIABLES
  costoF,costo,F(l), Ang(i)
  dPh(h),dPt(t)
  positive variable PhF(h),PtF(t),Ph(h), Pt(t),RacP(i), Pt1(t), Pt2(t);
  positive variable Loss, LossLFx(l) ;


* Costo que ya de por si se esta incurriendo por tener a las unidades prendidas
  CostoFijo = sum{t,PtData( t,'Costo1') } ;
* Calculos previos de admitancias y perdidas transversales
  EnlB[l]   = -FData(l,'X0')/ (FData(l,'R0')*FData(l,'R0') +FData(l,'X0')*FData(l,'X0'));
  EnlG[l]   =  FData(l,'R0')/ (FData(l,'R0')*FData(l,'R0') +FData(l,'X0')*FData(l,'X0') );
  LossTF[l] =  FData(l,'G0')*Pbase;
  PerdT[i] =  sum{ (l,j)$(FBus(l,i,j)), 0.5*LossTF[l]}+sum{ (l,j)$(FBus(l,j,i)), 0.5*LossTF[l]};

  Falla(i)=0;
  Pt.up(t)=PtData( t,'Pmax');   Pt.lo(t)=PtData( t,'Pmin'); Pt.l(t)=PtData(t,'Pgen');
  Ph.up(h)=PhData( h,'Pmax');   Ph.lo(h)=PhData( h,'Pmin'); Ph.l(h)=PhData(h,'Pgen');
  Pt1.up(t)=PtData( t,'Pmax1')-PtData( t,'Pmin');  Pt1.lo(t)=0;   Pt1.l(t)=Pt.l(t)-PtData( t,'Pmin');
  Pt2.up(t)=PtData( t,'Pmax2')-PtData( t,'Pmax1'); Pt2.lo(t)=0;   Pt2.l(t)=0;

  Ptmin(t)=Pt.lo(t); Ptmax(t)=Pt.up(t); Phmin(h)=Ph.lo(h); Phmax(h)=Ph.up(h);
  RacP.up(i)=inf;   RacP.lo(i)=0;    RacP.l(i)= 0;
  Ang.up(i)= pi;    Ang.lo(i)=-pi;   Ang.l(i)= 0;
  esta=0; loop(h, if(esta=0,  loop(j$(PhBus(h,j)),Ang.fx[j]=0; ); esta=1;); );
  Dem(i)=Demanda(i,'Pc')+PerdT(i) ;

  variat(t)=0;   variat(t)=1$(not (PtData(t,'Forzada')));
  variah(h)=0;   variah(h)=1$(not (PhData(h,'Forzada')));
  Ptgen(t)=PtData(t,'Pgen');  Phgen(h)= PhData(h,'Pgen');

**********************************************
* Calculo de Shift Factors
**********************************************
    ybus(i,j)=0;
    loop(l, loop((i,j)$FBus(l,i,j),ybus[i,j]=ybus[i,j]+EnlB[l];ybus[j,i]=ybus[j,i]+EnlB[l];ybus[i,i]=ybus[i,i]-EnlB[l];ybus[j,j]=ybus[j,j]-EnlB[l]));
    esta=0; loop(h, if(esta=0,  loop(k$(PhBus(h,k)),ybus(k,i)=0;ybus(i,k)=0;ybus(k,k)=1e20;);  esta=1;  ) );

    execute_unload 'zbus.gdx',i,zbus;
    execute_unload 'ybus.gdx',i,ybus;
    execute 'invert.exe ybus.gdx i ybus zbus.gdx zbus';
    execute_load 'zbus.gdx',zbus;

    esta=0; loop(h, if(esta=0,  loop(k$(PhBus(h,k)),zbus(k,k)=0;);  esta=1;  ) );

    loop(k, loop(l,  SFx[l,k]= EnlB[l]*sum{ (i,j)$FBus(l,i,j)  ,-(zbus[i,k] - zbus[j,k])} );
            loop(c $CONGMax(c,'Cong'), SFcx[c,k]=sum{l$(CONGRuta(c,l) ),   SFx[l,k]} ); );
    Ang.l(i)= 0;


**********************************************
* Ecuaciones
**********************************************
EQUATIONS
* Para flujo de  carga
  OperacionF,  Flujo,  Balance,  Perdidas
*** Comunes para DC y SF
  Operacion,
  PotTer[t],TerFor[t], HidFor[h],
  EnlCongMax[l],  EnlCongMin[l],  CongesMax[c],   CongesMin[c],
*** Para metodo DC iterativo y con perdidas cuadraticas
  Flujo[l],  Perdidas[l]
*** Para metodo con SF
  EcBalance, EcEnergia, FlujoSF[l]
  EcPerdidas
  EcGruposh,EcGrupost  ,EcGruposht
  DifPh,DifPt
;


* Flujo de carga
OperacionF..   costoF =e=  sum{i,CRacionamiento*RacP[i] }+ sum{t, CT(t)*PtF(t)} + sum{h, CH(h)*PhF(h)} ;
Flujo[l]..      F[l]/Pbase/EnlB[l]        =e= sum{ (i,j)$FBus(l,i,j)  ,(Ang[i] - Ang[j])};
Balance[i]..   RacP[i] +sum{ (t)$(PtBus(t,i)),PtF[t] }+sum{ (h)$(PhBus(h,i)),PhF[h] }
                      -sum{ (l,j)$(FBus(l,i,j)),F[l]+0.5*LossLFx(l) }  +sum{ (l,j)$(FBus(l,j,i)),F[l]-0.5*LossLFx(l) }  =e=   Dem(i) ;
Perdidas(l)..  LossLFx(l) =e= FData(l,'R0') * F.l(l)*F.l(l)/PBase;

Operacion..
  costo =e=
     sum{i,CRacionamiento*RacP[i] }
   + sum{t, PtData(t,'CI1')*Pt1(t)} + sum{t, PtData(t,'CI2')*Pt2(t)}
   + sum{h, PhData(h,'CI')*( Pendiente + 0.5*(1-Pendiente)* (Ph(h)/(PhData(h,'Pgen'))) )*Ph(h) }     ;
* Para hidraulicas el Costo Incremental  = CV*(Pendiente + (1-Pendiente) * (H / HidGen) )
* por tanto como Costo Total para FObj   = CV*Pendiente*P + CV*((1-Pendiente)/2) * (H**2 / HidGen)

** Ecuaciones comunes para flujo DC y Shift Factors
PotTer[t]..                          Pt[t]=e=  PtData( t,'Pmin')+Pt1[t]+Pt2[t];
TerFor[t]$( not variat(t))..         Pt[t]=e=  Ptgen(t);
HidFor[h]$( not variah(h))..         Ph[h]=e=  Phgen(h);
EnlCongMax[l]$(FData(l,'Cong'))..    F[l] =l=  Flmax(l)-0.5*LossTF[l]-0.5*LossLF[l];
EnlCongMin[l]$(FData(l,'Cong'))..   -F[l] =l=  Flmax(l)-0.5*LossTF[l]-0.5*LossLF[l];
CongesMax [c]$(CONGMax(c,'Cong'))..  sum(l$(CONGRuta(c,l)), F[l]+0.5*LossTF[l]+0.5*LossLF[l])  =l=  Fcmax(c);
CongesMin [c]$(CONGMax(c,'Cong'))..  sum(l$(CONGRuta(c,l)),-F[l]+0.5*LossTF[l]+0.5*LossLF[l])  =l=  Fcmax(c);

** Ecuaciones para calculo de flujo con shift factors
EcBalance..     sum{i,( sum{t$(PtBus(t,i)),Pt(t)} + sum{h$(PhBus(h,i)), Ph(h)} - Dem(i))} -Loss =e= 0;
EcEnergia..     Loss -  sum{i, LF(i)*( sum{t$(PtBus(t,i)),Pt(t)} + sum{h$(PhBus(h,i)), Ph(h)} - Dem(i)) } + Offset =e= 0;
FlujoSF[l]..    F[l] =e= sum{ i, SF[l,i]*( sum{ (t)$(PtBus(t,i)), Pt(t) } + sum{ (h)$(PhBus (h,i)), Ph(h) } - Dem(i) - D(i)*Loss)} ;

* variacion de unidades en paralelo en un mismo sentido
DifPh(h)$(variah(h))..      dPh(h)=e=Ph(h)-Phdc(h);
DifPt(t)$(variat(t))..      dPt(t)=e=Pt(t)-Ptdc(t);
EcGruposh(i,h,h2)$( PhBus(h,i) and PhBus(h2,i) and (PhData(h,'CI')=PhData(h2,'CI')) and (LFh(h)=LFh(h2)) and variah(h) and variah(h2) and (ord(h)<ord(h2))).. dPh(h)*dPh(h2)=g=0;
EcGrupost(i,t,t2)$( PtBus(t,i) and PtBus(t2,i) and (PtData(t,'CI1')=PtData(t2,'CI1')) and (LFt(t)=LFt(t2)) and variat(t) and variat(t2) and (ord(t)<ord(t2))).. dPt(t)*dPt(t2)=g=0;
EcGruposht(i,t,h)$( PtBus(t,i) and PhBus(h,i)  and (CT(t)=CH(h))  and (LFt(t)=LFh(h))  and variah(h) and variat(t)  ).. dPt(t)*dPh(h)=g=0;


MODEL FlujoCarga /OperacionF,Flujo,Balance,Perdidas/;
MODEL LmpSF      /Operacion,EcBalance, EcEnergia, FlujoSF ,PotTer,TerFor,HidFor,EnlCongMax,EnlCongMin,CongesMax,CongesMin,EcGruposh,EcGrupost,EcGruposht,DifPh,DifPt/;

*************************************************************************
* FLujo de carga
*************************************************************************
     CH(h)=0; CT(t)=0;
     PtF.fx(t)= Ptgen(t);  PhF.fx(h)= Phgen(h);
     RacP.up(i)=inf;   RacP.lo(i)=0;   Falla(i)=0;

*  Aproximacion de flujos y perdidas
     F.l[l] = sum{ i, SFx[l,i]*( sum{ (t)$(PtBus(t,i)), PtF.l(t) } + sum{ (h)$(PhBus (h,i)), PhF.l(h) } - Dem(i))} ;
     LossLFx.l[l] =  FData(l,'R0') * F.l(l)*F.l(l)/PBase;
*  buscar termica mas grande no forzada
     Ptaux(t)=PtF.l(t);
     potgrande=smax(t,Ptaux(t)) ;
     esta=0; loop(t, if(esta=0 and potgrande=Ptaux(t),  CT(t)=1; PtF.lo(t)=-inf; PtF.up(t)=inf; esta=1;   ) );
     Solve FlujoCarga using lp minimzing costoF;



* Se asignan limites para la unidad ajustada, en caso se ha salido de los limites
*    Se hace que solo la termica mas grande tenga alternativa para marginar.  Ello hasta que se habilite la calificacion de A PLENA CARGA
     variat(t)=0;
     esta=0; loop(t, if(esta=0 and potgrande=Ptaux(t), Ptgen(t)=PtF.l(t);variat(t)=1; esta=1;));


     Pt.up(t)=Ptmax(t)$(Ptmax(t)>=PtF.l(t))+PtF.l(t)$(Ptmax(t)<PtF.l(t));
     Pt.lo(t)=Ptmin(t)$(Ptmin(t)<=PtF.l(t))+PtF.l(t)$(Ptmin(t)>PtF.l(t));
     Ptmax(t)=Pt.up(t);  Ptmin(t)=Pt.lo(t);
     Pt1.up(t)=PtData( t,'Pmax1')-Ptmin(t);
     Pt2.up(t)=Ptmax(t)-PtData( t,'Pmax1');
     Pt1.l(t)=Pt.l(t)-Ptmin(t);   Pt2.l(t)=0;
     Pt1.l(t)$(Pt.l(t)>PtData(t,'Pmax1'))=Pt1.up(t);
     Pt2.l(t)$(Pt.l(t)>PtData(t,'Pmax1'))=Pt.l(t)-PtData(t,'Pmax1');

    Falla[i]=RacP.l[i];    RacP.fx(i)=0;   FallaTotal=sum{(i),Falla[i]};
    Dem(i)=Demanda(i,'Pc')+PerdT(i) -Falla[i];
    Ph.l(h)=PhF.l(h); Pt.l(t)=PtF.l(t);
    LossTotalL=sum{(l),LossLFx.l[l]};
    Loss.l=LossTotalL;   LossLF(l)=LossLFx.l(l);

    Phdc(h)=PhF.l(h); Ptdc(t)=PtF.l(t);  Fdc(l)=F.l(l);

***********************************************************************************
*  Flujo metodo Shift Factors
***********************************************************************************
    D[i]= (sum{(l,j)$(FBus(l,i,j)), 0.5*LossLFx.l[l]}+ sum{(l,j)$(FBus(l,j,i)), 0.5*LossLFx.l[l]})/LossTotalL;
    W(i)=D(i);
    SF(l,i)=SFx(l,i)-sum{(j),W(j)*(SFx[l,j])} ;
    SFc(c,i)=SFcx(c,i)-sum{(j),W(j)*(SFcx[c,j])} ;
    LFx(i)= sum(l, 2*FData(l,'R0') *SFx(l,i) *F.l(l)/Pbase  );
    LF(i)=(LFx(i)-sum{j,W[j]*LFx[j]})/(1-sum{j,W[j]*LFx[j]});
    loop((h,i)$(PhBus(h,i)),LFh(h)=LF(i)); loop((t,i)$(PtBus(t,i)),LFt(t)=LF(i));

    Offset = - LossTotalL +  sum{i, LF(i)*( sum{t$(PtBus(t,i)),Pt.l(t)} + sum{h$(PhBus(h,i)), Ph.l(h)} - Dem(i)) };

    Flmax(l)$(FData(l,'Cong'))  = F.l[l]$(F.l(l)>0) - F.l(l)$(F.l(l)<0) + 0.5*LossTF[l]+0.5*LossLFx.l[l];
    Fcmax[c]$(CONGMax(c,'Cong'))= sum(l$(CONGRuta(c,l)),  F.l[l]$(F.l(l)>0) - F.l(l)$(F.l(l)<0)  +0.5*LossTF[l]+0.5*LossLFx.l[l]) ;
    F.up(l)=Fdc(l)+deltaFL;F.lo(l)=Fdc(l)-deltaFL;



    Solve LmpSF using nlp minimzing costo;
***********************************************************************************
*  Determinar componentes de costos marginales
***********************************************************************************
   Tau = EcEnergia.m;
   CMGenergia(i)=Tau*(1-LF(i));
   CMGcongestion(i)= sum{ (l)  , (-FlujoSF.m[l])*(SF [l,i]- sum{j,D(j)*(SF [l,j])}) }  ;
   CMGtotal(i)= CMGenergia(i)+CMGcongestion(i);


***********************************************************************************
*  Reporte
***********************************************************************************
salida.pc=5;
put salida;
put /'Congestion'/;
put '----------'/;
put 'Enlace','Refencia','Envío','Recepción'/;
loop(l$(FData(l,'Cong')),put l.te(l),FData(l,'Pmax'):8:3;
          put (F.l(l)+0.5*LossLFx.l(l)+0.5*LossTF[l]):8:3;
          put (F.l(l)-0.5*LossLFx.l(l)-0.5*LossTF[l]):8:3/);

put /'Ruta en congestion'/;
put '------------------'/;
put 'Barras 1','Referencia','Envío','Recepción'/;
loop(c$(CONGMAX(c,'Cong')),put c.te(c), CONGMAX(c,'Pmax'):8:3;
          put (sum{l$(CONGRuta(c,l) ),  F.l[l]+0.5*LossLFx.l[l]+0.5*LossTF[l]}):8:3,
          put (sum{l$(CONGRuta(c,l) ),  F.l[l]-0.5*LossLFx.l[l]-0.5*LossTF[l]}):8:3/);


put /'Descomposicion Costos Marginales'/;
put '--------------------------------'/;
put 'Barra','Energia','Congestion','Total'/;
loop(i$(   sum{ (l,j)$FBus(l,j,i),FData(l,'Sist')} + sum{ (l,j)$FBus(l,i,j),FData(l,'Sist')}    ),
           put i.te(i),CMGenergia(i):15:6,CMGcongestion(i):15:6,CMGtotal(i):15:6 /;);


*$include \\coes.org.pe\areas\str\COSTOMARGINAL\GAMS\modelos4\salida2.gms
salida2.pc=5;
put salida2;
put /'Congestion'/;
put '----------'/;
put 'Enlace','Límite','Envío','Recepción'/;
loop(l$(FData(l,'Cong')),put l.te(l),FData(l,'Pmax'):8:3;
          put (F.l(l)+0.5*LossLFx.l(l)+0.5*LossTF[l]):8:3;
          put (F.l(l)-0.5*LossLFx.l(l)-0.5*LossTF[l]):8:3;
          put LossLFx.l(l);
          put LossTF(l)/);

put /'Ruta en congestion'/;
put '------------------'/;
put 'Barras 1','Límite','Envío','Recepción'/;
loop(c$(CONGMAX(c,'Cong')),put c.te(c), CONGMAX(c,'Pmax'):8:3;
          put (sum{l$(CONGRuta(c,l) ),  F.l[l]+0.5*LossLFx.l[l]+0.5*LossTF[l]}):8:3,
          put (sum{l$(CONGRuta(c,l) ),  F.l[l]-0.5*LossLFx.l[l]-0.5*LossTF[l]}):8:3/);

put 'GENERACION NO FORZADA'/;
put '---------------------'/;
put '','Scada','Ph SF','Dif.','Pmin','Pmax','CI scada','Cmg Energia','Cmg Cong','Cmg Total'/;
put '','A','B''B-A'/;
loop( (h,i)$( (not(PhData(h,'Forzada'))) and (PhBus(h,i))),
       put h.te(h),
       PhData(h,'Pgen'):8:3,
       Ph.l[h]:8:3,
       (Ph.l[h]-PhData(h,'Pgen')):8:3,
       Phmin(h):8:3,
       Phmax(h):8:3,
       PhData(h,'CI'):8:4,
       CMGenergia(i):12:4,
       CMGcongestion(i):12:4,
       CMGtotal(i):12:4/);


put '','Scada','PtSF','Dif.','Pmin','Pmax','CI','Cmg Total','Cmg Energia','Cmg Cong'/;
put '','A','B''B-A'/;
loop( (t,i)$( (not(PtData(t,'Forzada'))) and (PtBus(t,i))),
       put t.te(t),
       PtData(t,'Pgen'):8:3,
       Pt.l[t]:8:3,
       (Pt.l[t]-PtData(t,'Pgen')):8:3,
       Ptmin(t):8:3,
       Ptmax(t):8:3,
       PtData(t,'CI1'):8:4,
       CMGenergia(i):12:4,
       CMGcongestion(i):12:4,
       CMGtotal(i):12:4/);

put /'Generación Térmica'/;
put  '------------------'/;
put 'Térmica','Pg MW','Scada','Calificación'/;
loop(t,put t.te(t), Pt.l[t]:8:3,  PtData(t,'Pgen'):8:3, PtData(t,'Calif'):1:0/);


put /'Generación Hidraúlica'/;
put  '---------------------'/;
put 'Hidraulica','Ph MW','Scada'/;
loop( h,put h.te(h),Ph.l[h]:8:3,PhData(h,'Pgen'):8:3 /);


put  /'Resumen'/;
put  '-------'/;
put 'Generacion Termica',   (sum{t, Pt.l(t)}):8:3/;
put 'Generacion Hidraulica',   (sum{h, Ph.l(h)}):8:3/;
put 'Demanda Total',   (sum{i,Demanda(i,'Pc')}):8:3/;
put 'Falla',   (sum{i,Falla(i)}):8:3/;
put 'Perdidas Longitudinales',   Loss.l:8:3/;
put 'Perdidas Transversales',   (sum{i,PerdT (i)}):8:3/;

put /'Enlaces congestionados'/;
put 'Enlace','Flujo','Dual'/;
loop(l$(abs(FlujoSF.m[l])>0.1),put l.te(l), F.l(l):8:3, (-FlujoSF.m[l]):8:3 /);



            ";

            #endregion 
        }

        /// <summary>
        /// Permite obtener el modelo en GAMS
        /// </summary>
        /// <param name="parametros"></param>
        /// <param name="salida"></param>
        /// <param name="salida1"></param>
        /// <param name="salida2"></param>
        /// <returns></returns>
        public static string ObtenerModeloAC(string parametros, string salida, string salida1, string salida2)
        {

            string modelo = @"
*******************************************************************************************
*Descomposición CMG por sensibilidades, distribución de barra slack según pérdidas
*******************************************************************************************
*option qcp = cplex;
option nlp = snopt;
option lp = cplex;

$INCLUDE " + parametros + @" 
file salida1 /" + salida + @"/;
file salida2 /" + salida1 + @"/;
file salida3 /" + salida2 + @"/;

Alias (NB,i), (NB,j), (NB,k), (NB,z),(NB,m);
Alias (GT,t), (GT,t2)  ;
Alias (ENL,l),(CONG,c);

set MAXN /1*2000/;
Alias (m2,MAXN),(n2,MAXN);

SCALAR
 ModeloDC     /0/
 AplicaBound   /0/
 BoundCMG      /0.5/

 Pholgura     /1/
 Qholgura     /1/

 Pbase        /100.0/
 CPenalizaP   /1e4/
 CPenalizaQ   /1e2/
 CPenalizaQ0   /0/
 CPenalizaQ1   /1e3/
 CPenalizaQ2   /1e4/
 CPenalizaV0  /0/
 CPenalizaV1  /1e2/
 CPenalizaV2  /1e4/
 CPenalizaC0  /0/
 CPenalizaC1  /1e3/
 CPenalizaC2  /1e5/
 delta        /1e-3/
PARAMETER
* Generales
  reporte
  esta ,ncentral
* Para unidades de generacion\
  Conecg(t),PgCI(t),Pmax(t),Pmin(t),Qmin(t),Qmax(t),TipoQ(t), Ref(t) , Forzada(t),RefOrden
* Para enlaces
  Conecl(l), EnlR(l),EnlX(l),EnlG(l),  EnlB(l),LossLP(l),LossG(l),LossB(l),Tap1(l),Tap2(l),PerdPT(i),PerdQT(i)
  Congl(l),LadoC(l), Flmax(l), TipoC(l)   ,Congl0(l)
  Congc(c),LadoCC(c),Fcmax(c), TipoCC(c)  ,Congc0(c)
  PerdFP(l),PerdFQ(l)
* Para barras
  DemP(i), DemQ(i), ShuntR(i),ShuntI(i)
  Vmin(i), Vmax(i) ,TipoV(i) ,Vgen(i)
  Racp(i), Racq(i)
* Para compensacion reactiva
  ConecQCompR(cr),QCompRmin(cr),QCompRmax(cr)
* Para penalizaciones
  holgurav(i),cholgurav
  holgural(l),cholguralC
  holgurac(c)
  holguraq(t),cholguraq


***********************************************
* Para otros calculos
***********************************************
* Potencias inyectada-carga
  P(i),Q(i)
* Para calculo matriz admitancia
  G(i,j),B(i,j)
* Jacobiano
  JH(i,i),JN(i,i),JJ(i,i),JL(i,i)
  LFp(i), dFdP(l,i), dFcdP(c,i),dAngdP(i,i),dVdP(i,i), dang2(i),dv2(i) ,dang0(i),dv0(i)
  LFq(i), dFdQ(l,i), dFcdQ(c,i),dAngdQ(i,i),dVdQ(i,i), dang3(i),dV3(i)
  LFtotal(i),dFdPtotal(l,i),dFcdPtotal(c,i),dQdP(i,i)
  LossLFi(i),LossTotal
  LossC(c),LadoC2(l),LadoCC2(c),Flmax2(l),Fcmax2(c) ,FC1(c)
  D(i)

 CMGtotal(i),CMGcongestion(i),CMGenergia(i),CMGperdidas(i),CMGenergia0 ,CMG(i)
 CMGq(i),CMGqt(t),CMGv(i),CMGl(l),CMGc(c)
 val,val2,conta,contb,nbarras
 EnCongestion(l)

 JAC(MAXN,MAXN) ,invJAC(MAXN,MAXN)

 dPmax(t),dPmin(t),dQmax(t),dQmin(t)
 variap(t),variaq(t),variaqbus(i)
 escentral,central(t),central1(t)
bbp(i),bbq(i)

;

VARIABLES

  costo

  costo_op,costo_penP,costo_penQ,costo_penV,costo_penC
  Ang(i)                                                                                        ,
  Qg(t),QCompR(cr) ,Qbus(i)

  FP1(l),FP2(l),FQ1(l),FQ2(l)
  FMed(l) , FMedC(c)
  PSlack

  FObj
  dPg(t)
  positive variable Pg(t),V(i)

* Penalizacion sobre valores que salen fuera de rango
  positive variable DeltaP(i),DeltaP2(i),DeltaV(i),DeltaQ(i),DeltaQ2(i),DeltaC(l),DeltaCC(c),DeltaD(i)



;

********************************************
* Datos
********************************************
* Barras
  Ang.lo(i)=-pi;  Ang.up(i)= pi;
  V.l(i)=1;   V.l(i)=Demanda(i,'V');
  Ang.l(i)=0; Ang.l(i)=Demanda(i,'Angulo')*pi/180;
  DemP(i) =Demanda(i,'Pc')/PBase;
  DemQ(i) =Demanda(i,'Qc')/PBase;
  ShuntR(i)=Demanda(i,'ShuntR')/PBase;
  ShuntI(i)=Demanda(i,'ShuntI')/PBase;

* Unidades Generacion
  PgCI(t)=PgData(t,'CI1') ;
  Conecg(t)=PgData(t,'Conec');
  Pg.l(t)=PgData(t,'Pgen')/PBase;     Qg.l(t)=PgData(t,'Qgen')/PBase;
  Qmin(t)= PgData( t,'Qmin')/PBase;   Qmax(t)= PgData( t,'Qmax')/PBase;
  Ref(t)=PgData(t,'Ref');
  Forzada(t)=PgData(t,'Forzada');
  Qmin(t)$(Ref(t))=-inf;  Qmax(t)$(Ref(t))=inf;
*  esta=0;loop(t,  loop(j$(PgBus(t,j) and Ref(t)),if(esta=0,RefOrden=ord(j);esta=1) ) );
  loop(t,  loop(j$(PgBus(t,j) and Ref(t)),RefOrden=ord(j)) ) ;
* poner como angulo de referencia a la generacion a la indicada como referencia
  Ang.fx(i)$(ord(i) =RefOrden) =0;

* Compensacion reactiva dinamica
  QCompR.l(cr)=CRData(cr, 'Q')/PBase;
  QCompRmin(cr)=CRData(cr,'Qmin')/PBase;
  QCompRmax(cr)=CRData(cr,'Qmax')/PBase;
  ConecQCompR(cr)= CRData(cr,'Conec');

* Enlaces
  Conecl(l)= FData(l,'Conec') ;
  EnlR(l)=   FData(l,'R0'); EnlX(l)=   FData(l,'X0');

  EnlG(l)= 0;    EnlG[l]$(Conecl(l) and (EnlR(l)<>0 or EnlX(l)<>0)) =  EnlR(l)/ (sqr(EnlR(l)) +sqr(EnlX(l)));
  EnlB(l)=-1e10; EnlB[l]$(Conecl(l) and (EnlR(l)<>0 or EnlX(l)<>0)) = -EnlX(l)/ (sqr(EnlR(l)) +sqr(EnlX(l)));

  LossG[l]$Conecl(l) =  FData(l,'G0');
  LossB[l]$Conecl(l) =  FData(l,'B0');

  Tap1(l)$Conecl(l)   =  FData(l,'Tap1');
  Tap2(l)$Conecl(l)   =  FData(l,'Tap2');
********************************************
* Para OPF-AC
********************************************
* Barras
  Vmin(i)=Tension(i,'Vmin') ;   Vmax(i)=Tension(i,'Vmax');TipoV(i)=Tension(i,'Tipo');DeltaV.fx(i)$(TipoV(i)=3)=0;
  Vmax(i)$(V.l(i)>Vmax(i))=V.l(i);
  Vmin(i)$(V.l(i)<Vmin(i))=V.l(i);
* Generacion
  Pmin(t)= PgData( t,'Pmin')/PBase;   Pmax(t)= PgData( t,'Pmax')/PBase;
  Forzada(t)$(Pmin(t)=Pmax(t))=yes;
*  loop(i$(Vmax(i)=0 and Vmin(i)=0), Vmin(i)=0.80;  Vmax(i)=1.2; TipoV(i)=2; );

  DeltaP.lo(i)=0;  DeltaP.up(i)=inf;  DeltaP.l(i)= 0;
  Racp(i)=0  ;

* Enlaces
  Congl(l)$Conecl(l)= FData(l,'Cong');  LadoC(l)=FData(l,'Envio');   Flmax(l)=FData(l,'Pmax')/PBase;   TipoC(l)$Congl(l )=FData(l,'Tipo');  DeltaC.fx(l)$(TipoC(l)=3)=0;
  Congc(c)=CONGMax(c,'Cong');           LadoCC(c)=CONGMax(c,'Envio');Fcmax(c)=CONGMAX(c,'Pmax')/PBase; TipoCC(c)$Congc(c)=CONGMAX(c,'Tipo');DeltaCC.fx(c)$(TipoCC(c)=3)=0;
  LadoC2(l)=LadoC(l); LadoCC2(c)=LadoCC(c); Flmax2(l)=Flmax(l); Fcmax2(c)=Fcmax(c);

* Identificar todos los enlaces vinculados a congestion, para calcularle sus SF
  EnCongestion(l)$(Congl(l))=1;
  loop((c,l)$(CONGRuta(c,l) and Congc(c)), EnCongestion(l)=1; );


********************************************
* Valores iniciales
********************************************
  costo.l=0;
  costo_op.l=sum{t$Conecg(t), PgCI(t)*Pg.l(t)};
  costo_penP.l=0;
  cholguraq=0;cholgurav=0; cholguralC=0;   holguraq(t)=0; holgurav(i)=0;  holgural(l)=0;    holgurac(c)=0;
  FP1.l[l] = sum{ (i,j)$FBus(l,i,j)  , sqr(V.l(i)/tap1(l))*EnlG[l]-(V.l(i)*V.l(j))/(tap1(l)*tap2(l))*( EnlG(l)*cos(Ang.l(i)-Ang.l(j)) + EnlB(l)*sin(Ang.l(i)-Ang.l(j))  ) +sqr(V.l(i)/tap1(l))*0.5*LossG[l]};
  FP2.l[l] = sum{ (i,j)$FBus(l,i,j)  , sqr(V.l(j)/tap2(l))*EnlG[l]-(V.l(i)*V.l(j))/(tap1(l)*tap2(l))*( EnlG(l)*cos(Ang.l(i)-Ang.l(j)) - EnlB(l)*sin(Ang.l(i)-Ang.l(j))  ) +sqr(V.l(j)/tap2(l))*0.5*LossG[l]};
  FQ1.l[l] = sum{ (i,j)$FBus(l,i,j)  ,-sqr(V.l(i)/tap1(l))*EnlB[l]-(V.l(i)*V.l(j))/(tap1(l)*tap2(l))*( EnlG(l)*sin(Ang.l(i)-Ang.l(j)) - EnlB(l)*cos(Ang.l(i)-Ang.l(j))  ) +sqr(V.l(i)/tap1(l))*0.5*LossB[l]};
  FQ2.l[l] = sum{ (i,j)$FBus(l,i,j)  ,-sqr(V.l(j)/tap2(l))*EnlB[l]-(V.l(i)*V.l(j))/(tap1(l)*tap2(l))*(-EnlG(l)*sin(Ang.l(i)-Ang.l(j)) - EnlB(l)*cos(Ang.l(i)-Ang.l(j))  ) +sqr(V.l(j)/tap2(l))*0.5*LossB[l]};


************************************************************
* Buscar las que unidades o shunt que pueden variar P y Q
************************************************************
   nbarras=card(i);

  variaq(t)=0; variaq(t)$(Conecg(t) and (not Forzada(t)))=1$(Qg.l(t)+delta<Qmax(t) and Qg.l(t)-delta>Qmin(t));
  variap(t)=1$(not forzada(t));
  loop(t$variap(t),dPmax(t)=Pmax(t)-Pg.l(t); dPmin(t)=Pmin(t)-Pg.l(t));
  if (AplicaBound=1,
          loop(t$variap(t),
                if(dPmax(t)>BoundCMG, dPmax(t)=BoundCMG);
                if(dPmin(t)<-BoundCMG,dPmin(t)=-BoundCMG);
               )
      );

  dQmax(t)=Qmax(t)-Qg.l(t);  dQmin(t)=Qmin(t)-Qg.l(t);
  variaqbus(i)=0; loop((t,i)$PgBus(t,i),variaqbus(i)=variaqbus(i)+variaq(t));
  loop((cr,i),
          if(CRBus(cr,i) and (ConecQCompR(cr)) and (QCompR.l(cr)+delta<QCompR.up(cr) ) and (QCompR.l(cr)-delta>QCompR.lo(cr)),
             variaqbus(i)=1);
      );

************************************************************
* Identificar la primera unidad de una central
************************************************************
  ncentral=0; central(t)=0; central1(t)=0
  loop(t$(variap(t) and PgCI(t)>0),
           loop(t2$(variap(t2) and PgCI(t2)=PgCI(t) and (ord(t)<ord(t2)) and central(t2)=0 ) ,
                      if(central(t)=0,
                            ncentral=ncentral+1;
                            central(t)=ncentral;
                            central1(t)=1;
                        );
                      central(t2)=central(t);
             );
      );

   costo_penQ.l=0;
   costo_penV.l=0;
   costo_penC.l=0;
   DeltaQ.l(i)=0;

SCALAR
 PorcentajeVariaPlim /10/
 PorcentajeVariaCI   /5/

parameter
 dFlujol(l),dFlujoc(c);

VARIABLES
 varvar
 positive variable dPg2(t),dPg3(t);

EQUATIONS
  eFObj,
  eBalanceP,
  eFlujoCongMax(l),
  eFlujoConcMax(c),
  ePmax(t),ePmin(t) ,eCentral;

  eFObj..                       FObj    =e=  sum{t$variap(t), PgCI(t)*dPg(t)}
                                            +sum(t$variap(t), PgCI(t)*(1+0.5*PorcentajeVariaCI/PorcentajeVariaPlim*dPg2(t)/Pmax(t))*dPg2(t) )
                                            +sum(t$variap(t), PgCI(t)*(1+0.5*PorcentajeVariaCI/PorcentajeVariaPlim*dPg3(t)/Pmin(t))*dPg3(t) )     ;
  eBalanceP..                   sum(i, (1-LFtotal(i))* (sum( t$(PgBus(t,i) and variap(t)),  dPg(t)+dPg2(t)-dPg3(t) )) )  =e=0.01;
  eFlujoCongMax(l)$Congl(l)..   sum((t,i)$(PgBus(t,i) and variap(t)),dFdPtotal(l,i)*(dPg(t)+dPg2(t)-dPg3(t)))  =e= 0;
  eFlujoConcMax(c)$Congc(c)..   sum((t,i)$(PgBus(t,i) and variap(t)),dFcdPtotal(c,i)*(dPg(t)+dPg2(t)-dPg3(t))) =e= 0;
  ePmax(t)$variap(t)..          dPg(t)=l= dPmax(t);
  ePmin(t)$variap(t)..         -dPg(t)=l=-dPmin(t);
  eCentral(t,t2)$( (central(t)>0) and (central(t)=central(t2))  and (ord(t)<ord(t2))).. dPg(t)*dPg(t2)=g=0;




MODEL CMGCP /eFObj, eBalanceP,
  eFlujoCongMax,
  eFlujoConcMax,
  ePmax,ePmin,eCentral /;


*****************************************************************
* Calculo de sensibilidades    LFtotal, dFdPtotal y dFcdPtotal
*   datos necesarios: * Pg, Qg, V, Ang, Racp, QCompR
*****************************************************************



* Calculo de pérdidas por enlace
  LossLP(l) = sum{ (i,j)$FBus(l,i,j)  , EnlG(l)*(sqr(V.l(i)/tap1(l))+sqr(V.l(j)))-2*(V.l(i)*V.l(j)/tap1(l))*EnlG(l)*cos(Ang.l(i)-Ang.l(j)) };
  LossC[c]$Congc(c)= sum(l$(Conecl(l) and CONGRuta(c,l)), LossLP(l));
  PerdPT[i]=0;  PerdPT[i] =  sum{ (l,j)$(Conecl(l) and (FBus(l,i,j) or FBus(l,j,i))), 0.5*LossG[l]};
  PerdQT[i]=0;  PerdQT[i] =  sum{ (l,j)$(Conecl(l) and (FBus(l,i,j) or FBus(l,j,i))), 0.5*LossB[l]};

* Distribucion de pérdidas por barra
  LossLFi(i)=sum{(l,j)$(FBus(l,i,j)), 0.5*LossLP[l]}+ sum{(l,j)$(FBus(l,j,i)), 0.5*LossLP[l]} + sqr(V.l(i))*PerdPT(i);
  LossTotal=sum{i,LossLFi(i)};

  D[i]= LossLFi(i)/LossTotal;

* Matriz de admitancia
  if(ModeloDC=0,
                   G(i,j)=0; B(i,j)=0;
                   loop(l$Conecl(l), loop((i,j)$FBus(l,i,j),G[i,i]=G[i,i]+EnlG[l]/sqr(tap1(l));G[j,j]=G[j,j]+EnlG[l]/sqr(tap2(l));G[i,j]=G[i,j]-EnlG[l]/(tap1(l)*tap2(l));G[j,i]=G[j,i]-EnlG[l]/(tap1(l)*tap2(l))));
                   loop(l$Conecl(l), loop((i,j)$FBus(l,i,j),B[i,i]=B[i,i]+EnlB[l]/sqr(tap1(l));B[j,j]=B[j,j]+EnlB[l]/sqr(tap2(l));B[i,j]=B[i,j]-EnlB[l]/(tap1(l)*tap2(l));B[j,i]=B[j,i]-EnlB[l]/(tap1(l)*tap2(l))));
                   G(i,i)=G(i,i)+PerdPT(i)-ShuntR(i);
                   B(i,i)=B(i,i)+PerdQT(i)-ShuntI(i)-sum(cr$(CRBus(cr,i) and ConecQCompR(cr)), QCompR.l(cr)  );
             else
                   G(i,j)=0; B(i,j)=0;
                   loop(l$Conecl(l), loop((i,j)$FBus(l,i,j),G[i,i]=G[i,i]+EnlG[l];G[j,j]=G[j,j]+EnlG[l];G[i,j]=G[i,j]-EnlG[l];G[j,i]=G[j,i]-EnlG[l]));
                   loop(l$Conecl(l), loop((i,j)$FBus(l,i,j),B[i,i]=B[i,i]+EnlB[l];B[j,j]=B[j,j]+EnlB[l];B[i,j]=B[i,j]-EnlB[l];B[j,i]=B[j,i]-EnlB[l]));
         );

* Calculo de Jacobiano

  if (ModeloDC=0,
           P(i) = sum{ (t)$(PgBus(t,i) and Conecg(t)),Pg.l[t] } - DemP(i);
           Q(i) = sum{ (t)$(PgBus(t,i) and Conecg(t)),Qg.l[t] } - DemQ(i) ;

           JH(i,j)= V.l(i)*V.l(j)*(G(i,j)*sin(Ang.l(i)-Ang.l(j))-B(i,j)*cos(Ang.l(i)-Ang.l(j)));
           JH(i,i)= -sum(j$(ord(i)<>ord(j)),JH(i,j));

           JJ(i,j)= -V.l(i)*V.l(j)*(G(i,j)*cos(Ang.l(i)-Ang.l(j)) + B(i,j)*sin(Ang.l(i)-Ang.l(j)));
           JJ(i,i)= -sum(j$(ord(i)<>ord(j)),JJ(i,j));

           JN(i,j)= -JJ(i,j)/V.l(j) ;
           JN(i,i)=  P(i)/V.l(i) + G(i,i)*V.l(i);

           JL(i,j)=  JH(i,j)/V.l(j);
           JL(i,i) = Q(i)/V.l(i) - V.l(i)*B(i,i);


      else
           JH(i,j)= -B(i,j); JH(i,i)= -B(i,i); JN(i,j)= 0; JJ(i,j)= 0; JL(i,j)= 0; JL(i,i) =1;
      );


*****************************************************
parameter vec(i,m2);

* Crear Jacobiano completo e invertirla
  loop((i,m2)$(ord(i)=ord(m2)),vec(i,m2)=1);
  JAC(m2,m2)=1;
  loop((i,j)$(JH(i,j)<>0), loop(m2$(vec(i,m2)), loop(n2$(vec(j,n2)), JAC(m2,n2)=JH(i,j) )));
  loop((i,j)$(JN(i,j)<>0), loop(m2$(vec(i,m2)), loop(n2$(vec(j,n2)), JAC(m2,n2+nbarras)=JN(i,j))));
  loop((i,j)$(JJ(i,j)<>0), loop(m2$(vec(i,m2)), loop(n2$(vec(j,n2)), JAC(m2+nbarras,n2)=JJ(i,j))));
  loop((i,j)$(JL(i,j)<>0), loop(m2$(vec(i,m2)), loop(n2$(vec(j,n2)), JAC(m2+nbarras,n2+nbarras)=JL(i,j))));

* Poner en la referencia JH(ref,j)=0, JN(ref,j)=0, JH(j,ref)=0, JJ(j,ref)=0, JH(ref,ref)=1e20,
  JH(i,j)$(ord(i)=RefOrden)=0; JN(i,j)$(ord(i)=RefOrden)=0; JH(j,i)$(ord(i)=RefOrden)=0; JJ(j,i)$(ord(i)=RefOrden)=0;
  JH(i,i)$(ord(i)=RefOrden)=1e20;

* Artificio para tener solucion factible, debido a que se busca Angi - Angj, se pone Ang Ref=0
  JAC(m2,n2)$(ord(m2)=RefOrden) =0;  JAC(m2,n2)$(ord(n2)=RefOrden) =0; JAC(m2,m2)$(ord(m2)=refOrden) =1e20;

* Se elimina donde hay Qgen variable
  if(ModeloDC=0,
       loop(i$(variaqbus(i)<>0) , loop(m2$(vec(i,m2)), loop(n2,JAC(m2+nbarras,n2)=0; JAC(n2,m2+nbarras)=0; JAC(m2+nbarras,m2+nbarras)=1e20;)) ));

  execute_unload 'invJAC.gdx',m2,invJAC;
  execute_unload 'JAC.gdx',m2,JAC;
  execute 'invert.exe JAC.gdx m2 JAC invJAC.gdx invJAC';
  execute_load 'invJAC.gdx',invJAC;

 loop((m2,n2)$(ord(m2)<=nbarras and ord(n2)<=nbarras and invJAC(m2,n2)<>0),
      loop(i$(vec(i,m2)),  loop(j$(vec(j,n2)), dAngdP(i,j)=invJAC(m2,n2) ) )  );
 loop((m2,n2)$(ord(m2)<=nbarras and ord(n2)<=nbarras and invJAC(m2+nbarras,n2)<>0),
      loop(i$(vec(i,m2)),  loop(j$(vec(j,n2)), dVdP(i,j)=invJAC(m2+nbarras,n2))) );
 loop((m2,n2)$(ord(m2)<=nbarras and ord(n2)<=nbarras and invJAC(m2+nbarras,n2+nbarras)<>0),
      loop(i$(vec(i,m2)),  loop(j$(vec(j,n2)), dVdQ(i,j)=invJAC(m2+nbarras,n2+nbarras))) );
 loop((m2,n2)$(ord(m2)<=nbarras and ord(n2)<=nbarras and invJAC(m2,n2+nbarras)<>0),
      loop(i$(vec(i,m2)),  loop(j$(vec(j,n2)), dAngdQ(i,j)=invJAC(m2,n2+nbarras))) );

* Conversión de dAng y dV con una distribucion D
  dang0(i)= sum(j, D(j)*dAngdP(i,j) );
  dv0(i)  = sum(j, D(j)*dVdP(i,j) );
  loop(k, dAngdP(i,k)=1*dAngdP(i,k) - dang0(i); dVdP(i,k)  =1*dVdP(i,k)   - dv0(i);      );

  dang0(i)= sum(j, D(j)*dAngdQ(i,j) );
  dv0(i)  = sum(j, D(j)*dVdQ(i,j) );
  loop(k, dAngdQ(i,k)=1*dAngdQ(i,k) - dang0(i); dVdQ(i,k)  =1*dVdQ(i,k)   - dv0(i);      );


* Calculo de Pérdidas marginales y Shift Factor
  loop(k,

      dang2(i)=dAngdP(i,k); dv2(i)=dVdP(i,k);
      dang3(i)=dAngdQ(i,k); dv3(i)=dVdQ(i,k);

*     Shift Factor dFlujo/dPk, se calcula en el medio
*        fpm=  (sqr(Vi/tap1)-sqr(Vj/tap2))*( g/2+g/4) -Vi*Vj/(tap1*tap2)*b*sin(Angi-Angj) )
*        fqm=  (sqr(Vi/tap1)-sqr(Vj/tap2))*(-b/2+b/4) -Vi*Vj/(tap1*tap2)*g*sin(Angi-Angj) )

         if(ModeloDC=0,
                 dFdP(l,k)$(Conecl(l) and EnCongestion(l))= sum( (i,j)$FBus(l,i,j)  ,
                        -V.l(i)*V.l(j)/(tap1(l)*tap2(l))*EnlB(l)*cos(Ang.l(i)-Ang.l(j))*(dang2(i)-dang2(j))
                        +( V.l(i)/sqr(tap1(l))*(EnlG(l)+LossG(l)/2)-V.l(j)/(tap1(l)*tap2(l))*EnlB(l)*sin(Ang.l(i)-Ang.l(j)))*dv2(i)
                        +(-V.l(j)/sqr(tap2(l))*(EnlG(l)+LossG(l)/2)-V.l(i)/(tap1(l)*tap2(l))*EnlB(l)*sin(Ang.l(i)-Ang.l(j)))*dv2(j)
                       );
                 dFdQ(l,k)$(Conecl(l) and EnCongestion(l))= sum( (i,j)$FBus(l,i,j)  ,
                        -V.l(i)*V.l(j)/(tap1(l)*tap2(l))*EnlB(l)*cos(Ang.l(i)-Ang.l(j))*(dang3(i)-dang3(j))
                        +( V.l(i)/sqr(tap1(l))*(EnlG(l)+LossG(l)/2)-V.l(j)/(tap1(l)*tap2(l))*EnlB(l)*sin(Ang.l(i)-Ang.l(j)))*dv3(i)
                        +(-V.l(j)/sqr(tap2(l))*(EnlG(l)+LossG(l)/2)-V.l(i)/(tap1(l)*tap2(l))*EnlB(l)*sin(Ang.l(i)-Ang.l(j)))*dv3(j) );
          else
                 dFdP(l,k)$(Conecl(l) and EnCongestion(l))= sum( (i,j)$FBus(l,i,j)  , -EnlB(l)*(dAng2(i)-dAng2(j)) );
                 dFdQ(l,k)=0;
          );
*     Factor de pérdidas marginales
*        loss = (sqr(Vi/tap1)+sqr(Vj/tap2))*(g+lossg/2)-2*Vi*Vj/(tap1*tap2)*g*cos(angi-angj)
         if(ModeloDC=0,
                 LFp(k)=sum( (l,i,j)$(FBus(l,i,j) and Conecl(l)),
                        2*V.l(i)*V.l(j)/(tap1(l)*tap2(l))*EnlG(l)*sin(Ang.l(i)-Ang.l(j))*(dang2(i)-dang2(j))
                       +(2*V.l(i)/sqr(tap1(l))*(EnlG(l)+LossG(l)/2) - 2*V.l(j)/(tap1(l)*tap2(l))*EnlG(l)*cos(Ang.l(i)-Ang.l(j))   )*dV2(i)
                       +(2*V.l(j)/sqr(tap2(l))*(EnlG(l)+LossG(l)/2) - 2*V.l(i)/(tap1(l)*tap2(l))*EnlG(l)*cos(Ang.l(i)-Ang.l(j))   )*dV2(j)  );

                 LFq(k)=sum( (l,i,j)$(FBus(l,i,j) and Conecl(l)),
                        2*V.l(i)*V.l(j)/(tap1(l)*tap2(l))*EnlG(l)*sin(Ang.l(i)-Ang.l(j))*(dang3(i)-dang3(j))
                       +(2*V.l(i)/sqr(tap1(l))*(EnlG(l)+LossG(l)/2) - 2*V.l(j)/(tap1(l)*tap2(l))*EnlG(l)*cos(Ang.l(i)-Ang.l(j))   )*dV3(i)
                       +(2*V.l(j)/sqr(tap2(l))*(EnlG(l)+LossG(l)/2) - 2*V.l(i)/(tap1(l)*tap2(l))*EnlG(l)*cos(Ang.l(i)-Ang.l(j))   )*dV3(j)  );
         else
                 LFp(k)=sum((l,i,j)$(FBus(l,i,j) and Conecl(l)),  2*EnlR(l)*dFdP(l,k)*FP1.l(l));
                 LFq(k)=0;
             );
      );

  display dFdP,LFp;

  loop(c $CONGMax(c,'Cong'), dFcdP[c,k]=sum{l$(CONGRuta(c,l) ),   dFdP[l,k]} );
  loop(c $CONGMax(c,'Cong'), dFcdQ[c,k]=sum{l$(CONGRuta(c,l) ),   dFdQ[l,k]} );

  dQdP(i,j)$variaqbus(i)=sum(k, JJ(i,k)*dAngdP(k,j)+JL(i,k)*dVdP(k,j));


  LFtotal(i) = LFp(i)+sum(j,LFq(j)*dQdP(j,i));
  dFdPtotal(l,i)= dFdP(l,i)+sum(j,dFdQ(l,j)*dQdP(j,i));

  loop(c $CONGMax(c,'Cong'), dFcdPtotal[c,k]=sum{l$(CONGRuta(c,l) ),   dFdPtotal[l,k]} );


  dPg2.up(t)=0; dPg2.up(t)=(PorcentajeVariaPlim/100)*Pmax(t);
  dPg3.up(t)=0; dPg3.up(t)=(PorcentajeVariaPlim/100)*Pmin(t);


  Solve CMGCP using nlp minimzing Fobj;

  CMGl(l)         =  eFlujoCongMax.m(l);
  CMGc(c)         =  eFlujoConcMax.m(c);
  CMGEnergia0     =  eBalanceP.m;
  CMGenergia(i)   =  CMGenergia0;
  CMGperdidas(i)  = -CMGenergia0*LFtotal(i);
  CMGcongestion(i)= sum( (l)$Congl(l), CMGl(l) *(dFdPtotal[l,i])) + sum{ (c)$Congc(c), CMGc(c)*(dFcdPtotal[c,i]) };

  CMG(i)=CMGenergia0+CMGperdidas(i)+CMGcongestion(i);

  dFlujol(l)$Congl(l)=sum((t,i)$(PgBus(t,i)),dFdPtotal(l,i)*(dPg.l(t)+dPg2.l(t)-dPg3.l(t)));
  dFlujoc(c)$Congc(c)=sum((t,i)$(PgBus(t,i)),dFcdPtotal(c,i)*(dPg.l(t)+dPg2.l(t)-dPg3.l(t)));


salida1.pc=5;

put salida1;

put /'Congestion Simple'/;
put '------------------'/;
put '','Envio(1)','MW','Envio'/;
loop(l$Congl(l),put l.te(l);
       put (Flmax(l)*PBase):8:3,LadoC(l) /
     );

put /'Congestion Compuesta'/;
put '------------------'/;
put '','MW','Envio'/;
loop(c$Congc(c),  put c.te(c);
       put (Fcmax(c)*PBase):8:3,
       put LadoCC(c)  /);


put /'Descomposicion Costos Marginaless'/;
put '------------------'/;
put 'Barra','Energia','Congestion','Total'/;
loop(i,put i.te(i),put (CMGenergia(i)+CMGperdidas(i)):11:6;  put CMGcongestion(i):11:6,put CMG(i):11:6/);


salida2.pc=5;

put salida2;

put 'Costo Total',     (costo.l):10:3/;
put 'Costo Operacion', (costo_op.l):10:3/;
put 'Costo Pen.P',     (costo_penP.l):10:3/;
put 'Costo Pen.Q',     cholguraq:10:4/;
put 'Costo Pen.V',     cholgurav:10:3/;
put 'Costo Pen.C',     cholguralC:10:4/;
put ''/;
put 'Generacion (MW):'; put  (PBase*sum{t,Pg.l(t)}):10:5/;
put 'Generacion Holgura(MW):'; put ( PBase*sum{i,DeltaP.l(i)}):10:5/;
put 'Demanda (MW):'; put ( PBase*sum{i,DemP(i)}):10:5/;
put 'Perdidas (MW):'; put ( (PBase*sum{t,Pg.l(t)}+PBase*sum{i,DeltaP.l(i)} - PBase*sum{i,DemP(i)})):10:5/;
put 'Perdidas Longt (MW):'; put (PBase*sum{l,LossLP(l)} ):10:5/;
put 'Perdidas Trans (MW):'; put (PBase*sum{i,sqr(V.l(i))*(PerdPT(i)-ShuntR(i))} ):10:5/;


put /'Costos Marginales'/;
put '------------------'/;
put '','Energia','Perdidas','Congestion','Total'/;
loop(i,put i.te(i),put CMGenergia(i):9:4; put CMGperdidas(i):9:4; put CMGcongestion(i):9:4,put CMG(i):9:4,put (1-LFtotal(i)):10:7/);

put /'Potencia Generada'/;
put '------------------'/;
put '','MW','MVAR'/;
loop(t,put t.te(t),put (Pg.l(t)*PBase):8:3;put (Qg.l(t)*PBase):8:3/);

put /'Tension Barras'/;
put '------------------'/;
put '','tension (pu)','Grados'/;
loop(i,put i.te(i);put V.l(i):8:5;put (Ang.l(i)*180/pi):8:4/);


put /'Flujos'/;
put '------------------'/;
put '','MW e','MW r','MVAR e','MVAR r','MW perd','MVAR perd'/;
loop(l,put l.te(l);
       put (FP1.l(l)$Conecl(l)*PBase):8:3;put (-FP2.l(l)$Conecl(l)*PBase):8:3;
       put (FQ1.l(l)$Conecl(l)*PBase):8:3;put (-FQ2.l(l)$Conecl(l)*PBase):8:3;
       put ((FP1.l(l)$Conecl(l)+FP2.l(l)$Conecl(l))*PBase):8:3;
       put ((FQ1.l(l)$Conecl(l)+FQ2.l(l)$Conecl(l))*PBase):8:3/;   )    ;


put /'Shunt '/;
put '------------------'/;
put '','MW shunt','MVAR shunt'/;
loop(i$(ShuntR(i)<>0 or ShuntI(i)<>0 ),put i.te(i);put (ShuntR(i)*sqr(V.l(i))*PBase):12:3;put (ShuntI(i)*sqr(V.l(i))*PBase):12:3/);

put /'Compensacion Reactiva dinamica'/;
put '------------------'/;
put '','MVAR'/;
loop((i,cr)$(CRBus(cr,i)),put cr.te(cr);put (sqr(V.l(i))*QCompR.l(cr)*PBase):8:3/);


put /'Congestion Simple'/;
put '------------------'/;
put '','Envio(1)','MW','Limite MW','Envio'/;
loop(l$Congl(l),put l.te(l);
       put LadoC(l):1:0;
       put ((FP1.l(l)*LadoC(l)- FP2.l(l)*(1-LadoC(l)))*PBase):8:3;
       put (Flmax(l)*PBase):8:3,LadoC(l) /
     );

put /'Congestion Compuesta'/;
put '------------------'/;
put '','Envio(1)','MW','Limite MW','Envio'/;
loop(c$Congc(c),  put c.te(c),put LadoCC(c):1:0;
       put (sum{l$(CONGRuta(c,l)),  (FP1.l(l)*LadoCC(c)- FP2.l(l)*(1-LadoCC(c)))   }*PBase):8:3  ;
       put (Fcmax(c)*PBase):8:3,
       put LadoCC(c)  /);


salida3.pc=5;

put salida3;
put /'Congestion Simple'/;
put '------------------'/;
put '','MW','Envio','Delta'/;
loop(l$Congl(l),put l.te(l);
       put (Flmax(l)*PBase):8:3,LadoC(l),(dFlujol(l)*PBase):8:3 /
     );

put /'Congestion Compuesta'/;
put '------------------'/;
put '','MW','Envio','Delta'/;
loop(c$Congc(c),  put c.te(c);
       put (Fcmax(c)*PBase):8:3,
       put LadoCC(c),(dFlujoc(c)*PBase):8:3  /);

put /'---- CMG mínimo y máximo -----'/;
put smin(i,CMG(i)):11:6, smax(i,CMG(i)):11:6/;

put /'---- Variacion de generacion activa por centrales -----'/;
put '','Pg MW','Pg final MW','dPg MW'/;
loop((t)$(variap(t) and central1(t)=1),
      val  =sum(t2$(variap(t) and central(t2)=central(t)), Pg.l(t2));
      val2 =sum(t2$(variap(t) and central(t2)=central(t)), dPg.l(t2)+dPg2.l(t2)-dPg3.l(t2));
      if (val2<>0, put t.te(t); put (val*PBase):8:3, put ((val+val2)*PBase):8:3,put (val2*PBase):8:3/);
    );
loop((t)$(variap(t) and central(t)=0),
      if (dPg.l(t)<>0, put t.te(t); put (Pg.l(t)*PBase):8:3,put ((Pg.l(t)+dPg.l(t))*PBase):8:3, put (dPg.l(t)*PBase):8:3/);
    );

put /'---- Variacion de generacion activa por unidades -----'/;
put '','CI','CMg','dPg MW','dPg MW min','dPg MW max','','dPgExtra MW','dPgLadoMin MW','dPgLadoMax MW','FPmg'/;
loop((t,i)$(PgBus(t,i)  and variap(t) and (dPg.l(t)<>0 or dPg2.l(t)>0 or dPg3.l(t)>0)  ),
      put t.te(t),put PgCI(t):8:4,put CMG(i):8:4,
      put (dPg.l(t)*PBase):8:3,
      put (dPmin(t)*PBase):8:3,put (dPmax(t)*PBase):8:3,
      if(  (dPmin(t)<dPg.l(t)-delta and dPg.l(t)+delta<dPmax(t)),put 'Var'  );
      if(  (dPmin(t)>=dPg.l(t)-delta or  dPg.l(t)+delta>=dPmax(t)),put ''  );
      put ((dPg2.l(t)-dPg3.l(t))*PBase):8:3,
      put (-dPg3.up(t)*PBase):8:3,put (dPg2.up(t)*PBase):8:3,
      put (1-LFtotal(i)):10:6,

      put ''/);
put 'Suma:';
put (PBase*sum((t,i)$(PgBus(t,i) ) , (dPg.l(t)+dPg2.l(t)-dPg3.l(t))*(1-LFtotal(i)))):8:4/;

put /'---- Sensibilidades -----'/;
put '','CI','CMg','dPg MW','dPgExtra MW','dFdP'/;
loop((l,t,i)$(Congl(l) and variap(t) and PgBus(t,i) ),
      put t.te(t),put PgCI(t):8:4,put CMG(i):8:4,put (dPg.l(t)*PBase):8:3, put ((dPg2.l(t)-dPg3.l(t))*PBase):8:3,
      put (dFdPtotal(l,i)):10:6,
      put ''/);

put /'---- Variacion de generación reactiva por unidades -----'/;
put '','dQg MVAR','dQg MVAR min','dQg MVAR max'/;
loop((t,i)$(PgBus(t,i) and variaq(t)),
                val=sum( (t2,j)$PgBus(t2,j),  dQdP(i,j)*dPg.l(t2)   );
                put t.te(t),put (val*PBase):8:4,put (dQmin(t)*PBase):8:4,put (dQmax(t)*PBase):8:4/;
             );



  bbp(i)= sum{(l,j)$(Conecl(l) and FBus(l,i,j)),FP1.l(l)  } + sum{(l,j)$(Conecl(l) and FBus(l,j,i)),FP2.l(l)}
         - sum{ (t)$(PgBus(t,i) and Conecg(t)),Pg.l[t] }
         + DemP(i)
         - sqr(V.l(i))*ShuntR(i)  ;
  bbq(i)= sum{(l,j)$(Conecl(l) and FBus(l,i,j)),FQ1.l(l)  } + sum{(l,j)$(Conecl(l) and FBus(l,j,i)),FQ2.l(l)}
         - sqr(V.l(i))*sum(cr$   (ConecQCompR(cr) and CRBus(cr,i))   ,QCompR.l(cr) )
         - sum{ (t)$(PgBus(t,i) and Conecg(t)),Qg.l[t] }
         + DemQ(i)
         - sqr(V.l(i))*ShuntI(i) ;




put /'---- Desviaciones máximas y mínimas MW -----'/;
val=smax(i,bbp(i));
loop(i,  if(val=bbp(i), put i.te(i); put (val*PBase):8:3/));
val=smin(i,bbp(i));
loop(i,  if(val=bbp(i), put i.te(i); put (val*PBase):8:3/));
put /'---- Desviaciones máxima y mínimas MVAR -----'/;
val=smax(i,bbq(i));
loop(i,  if(val=bbq(i), put i.te(i); put (val*PBase):8:3/));
val=smin(i,bbq(i));
loop(i,  if(val=bbq(i), put i.te(i); put (val*PBase):8:3/));


";

            return modelo;

        }
    }
}
