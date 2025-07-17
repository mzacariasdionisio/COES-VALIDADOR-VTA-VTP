using GAMS;
using COES.Framework.Base.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Servicios.Aplicacion.PotenciaFirme;

namespace COES.Servicios.Aplicacion.PotenciaFirmeRemunerable.Helper
{
    public class EjecucionGams
    {
        /// <summary>
        /// Realiza la ejecuacion GAMS
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="parametros">string parametros = @"D:\Gams\Casos\201610191840.DAT "</param>
        /// <param name="salida">string salida = @"D:\Gams\Salida\salida6.csv"</param>
        public static int Ejecutar(out GAMSJob modelo, DateTime fechaProceso, string path, string codigoGams1)
        {
            int resultado = -1;
            string salida = ConstantesPotenciaFirmeRemunerable.ArchivoSalidaGams;
            try
            {
                //- Declaraando los objetos para integracion con GAMS
                string XsystemDirectory = @"C:\GAMS\34";
                GAMSWorkspace ws = new GAMSWorkspace(workingDirectory: path, XsystemDirectory, DebugLevel.Off);                
                GAMSDatabase db = ws.AddDatabase();

                using (GAMSOptions opt = ws.AddOptions())
                {
                    //- Ejecutando el modelo

                    modelo = ws.AddJobFromString(codigoGams1);
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
        public static string ObtenerModelo()
        {
            #region Model

            string result = @"

*******************************************************************************************
* Flujo de carga optimo AC
* Desarrollado por el COMITE DE OPERACIÓN ECONÓMICA DEL SISTEMA (COES)
* Lima - Perú
* Derechos reservados
*******************************************************************************************
option nlp=snopt;
option lp=cplex;

$INCLUDE OPS_ENTRADA.DAT 
file salida2  /_opf.csv/;
$include    inicializa12.gms

EQUATIONS

* Para OPF
  Total,Operacion,PenalizaP,PenalizaV,PenalizaC,
  BalanceP(i), BalanceQ(i),
  deltaang1(l,i,j), deltaang2(l,i,j),
  FlujoP1(l), FlujoP2(l),  FlujoQ1(l), FlujoQ2(l),
  DeltaC1(l), DeltaC2(l),  DeltaCC1(c),DeltaCC2(c),  DeltaV1(i), DeltaV2(i),
  Pgmax(t),   Pgmin(t) ,   Qgmax(t),   Qgmin(t) ,
  CompRmin(cr), CompRmax(cr),eCentral(t,t2);

* Funcion objetivo minimizar costo
  Total..        costo    =e= costo_op + costo_penP +  costo_penC + costo_penV;

  Operacion..    costo_op=e=   sum{t$Conecg(t), PgCV(t)*Pg(t)}  ;
  PenalizaP..    costo_penP=e= sum{i,CPenalizaP*DeltaP[i]};
  PenalizaC..    costo_penC=e= sum{l$(TipoC(l)=0), CPenalizaC0*DeltaC(l)} +sum{l$(TipoC(l)=1), CPenalizaC1*DeltaC(l)} +sum{l$(TipoC(l)=2), CPenalizaC2*DeltaC(l)*DeltaC(l)}
                              +sum{c$(TipoCC(c)=0),CPenalizaC0*DeltaCC(c)}+sum{c$(TipoCC(c)=1),CPenalizaC1*DeltaCC(c)}+sum{c$(TipoCC(c)=2),CPenalizaC2*DeltaCC(c)*DeltaCC(c)} ;
  PenalizaV..    costo_penV=e= sum{i$(TipoV(i)=0), CPenalizaV0*DeltaV(i)} +sum{i$(TipoV(i)=1), CPenalizaV1*DeltaV(i)} +sum{i$(TipoV(i)=2), CPenalizaV2*DeltaV(i)*DeltaV(i)} ;

* Ecuaciones de balance de potencia por barra
  BalanceP[i]..  sum{(l,j)$(Conecl(l) and FBus(l,i,j)),FP1(l)  } + sum{(l,j)$(Conecl(l) and FBus(l,j,i)),FP2(l)}
                 =e=0*DeltaP[i]$(Pholgura=1) + sum{ (t)$(PgBus(t,i) and Conecg(t)),Pg[t] } - DemP(i) + sqr(V(i))*ShuntR(i);
  BalanceQ[i]..  sum{(l,j)$(Conecl(l) and FBus(l,i,j)),FQ1(l)  } + sum{(l,j)$(Conecl(l) and FBus(l,j,i)),FQ2(l)}
                 =e=sqr(V(i))* sum(cr$(ConecQCompR(cr) and CRBus(cr,i)),QCompR(cr))   +sum{ (t)$(PgBus(t,i) and Conecg(t)),Qg[t] } - DemQ(i) + sqr(V(i))*ShuntI(i);

* Ecuaciones de Flujo activa de envio y recepcion por enlace
   FlujoP1(l)$Conecl(l)..  FP1[l] =e= sum{ (i,j)$FBus(l,i,j)  , sqr(V(i)/tap1(l))*EnlG[l]-(V(i)*V(j)/(tap1(l)*tap2(l)))*( EnlG(l)*cos(Ang(i)-Ang(j)) + EnlB(l)*sin(Ang(i)-Ang(j))  )+sqr(V(i)/tap1(l))*0.5*LossG[l]};
   FlujoP2(l)$Conecl(l)..  FP2[l] =e= sum{ (i,j)$FBus(l,i,j)  , sqr(V(j)/tap2(l))*EnlG[l]-(V(i)*V(j)/(tap1(l)*tap2(l)))*( EnlG(l)*cos(Ang(i)-Ang(j)) - EnlB(l)*sin(Ang(i)-Ang(j))  )+sqr(V(j)/tap2(l))*0.5*LossG[l]};
   FlujoQ1(l)$Conecl(l)..  FQ1[l] =e= sum{ (i,j)$FBus(l,i,j)  ,-sqr(V(i)/tap1(l))*EnlB[l]-(V(i)*V(j)/(tap1(l)*tap2(l)))*( EnlG(l)*sin(Ang(i)-Ang(j)) - EnlB(l)*cos(Ang(i)-Ang(j))  )+sqr(V(i)/tap1(l))*0.5*LossB[l]};
   FlujoQ2(l)$Conecl(l)..  FQ2[l] =e= sum{ (i,j)$FBus(l,i,j)  ,-sqr(V(j)/tap2(l))*EnlB[l]-(V(i)*V(j)/(tap1(l)*tap2(l)))*(-EnlG(l)*sin(Ang(i)-Ang(j)) - EnlB(l)*cos(Ang(i)-Ang(j))  )+sqr(V(j)/tap2(l))*0.5*LossB[l]};

   Pgmax(t)$Conecg(t) ..  Pg(t) =l=  Pmax(t) ;
   Pgmin(t)$Conecg(t) .. -Pg(t) =l= -Pmin(t);

   Qgmax(t)$Conecg(t) ..  Qg(t) =l=  Qmax(t) ;
   Qgmin(t)$Conecg(t) .. -Qg(t) =l= -Qmin(t);

   CompRmax(cr)$ConecQCompR(cr)..   QCompR(cr) =l=  QCompRmax(cr);
   CompRmin(cr)$ConecQCompR(cr)..  -QCompR(cr) =l= -QCompRmin(cr);

   deltaang1(l,i,j)$(Conecl(l) and FBus(l,i,j))..   Ang(i) - Ang(j) =l= 0.33*pi;
   deltaang2(l,i,j)$(Conecl(l) and FBus(l,j,i))..   Ang(i) - Ang(j) =l= 0.33*pi;

* Desviaciones sobre valores maximos
  DeltaV1(i)..             V(i) - DeltaV(i) =l= Vmax(i) ;
  DeltaV2(i)..            -V(i) - DeltaV(i) =l=-Vmin(i) ;

  DeltaC1(l)$Congl(l)..   (FP1(l)*LadoC2(l)+ FP2(l)*(1-LadoC2(l))) - DeltaC(l) =l= Flmax2(l);
  DeltaC2(l)$Congl(l)..  -(FP1(l)*LadoC2(l)+ FP2(l)*(1-LadoC2(l))) - DeltaC(l) =l= Flmax2(l);
  DeltaCC1[c]$Congc(c)..  sum(l$(Conecl(l) and CONGRuta(c,l)), FP1(l)*LadoCC2(c)+ FP2(l)*(1-LadoCC2(c)) ) - DeltaCC(c) =l= Fcmax2(c);
  DeltaCC2[c]$Congc(c).. -sum(l$(Conecl(l) and CONGRuta(c,l)), FP1(l)*LadoCC2(c)+ FP2(l)*(1-LadoCC2(c)) ) - DeltaCC(c) =l= Fcmax2(c);
  eCentral(t,t2)$( (central(t)>0) and (central(t)=central(t2))  and (ord(t)<ord(t2))).. Qg(t)*Qg(t2)=g=0;
MODEL OPF   /Total,Operacion,
             PenalizaP,PenalizaV,PenalizaC,
             BalanceP, BalanceQ,
             deltaang1, deltaang2,
             FlujoP1, FlujoP2,  FlujoQ1, FlujoQ2,
             Pgmax, Pgmin, Qgmax, Qgmin,
             CompRmax, CompRmin,
             DeltaV1, DeltaV2, DeltaC1,DeltaC2, DeltaCC1,DeltaCC2,
             eCentral
             /;

  Solve OPF using nlp minimzing costo;

***************************************************************************************************************
* Reporte de generación y flujos en congestión
***************************************************************************************************************
salida2.pc=5;
put salida2;
put /'Potencia Generada','','MW'/;
loop(t,put t.tl,put t.te(t),put (Pg.l(t)*PBase):8:3/);
parameter ncl, ncc;
ncl=0;ncc=0;
loop(l$Congl(l),ncl=ncl+1);loop(c$Congc(c),ncc=ncc+1);
if(ncl>0,
    put /'Congestion Simple','','Envio(1)','MW','Limite MW','Envio'/;
    loop(l$Congl(l),put l.tl,put l.te(l);
       put LadoC(l):1:0;
        ValFlujoL(l)=((FP1.l(l)*LadoC(l)- FP2.l(l)*(1-LadoC(l)))*PBase);
        put (ValFlujoL(l)):8:3;
       put (Flmax(l)*PBase):8:3,LadoC(l) /);
);
if(ncc>0,
   put /'Congestion Compuesta','','MW','Limite MW','Envio'/;
   loop(c$Congc(c),  put c.tl,put c.te(c);
      ValFlujoc(c)=sum{l$(CONGRuta(c,l)),  (FP1.l(l)*LadoCC(c)- FP2.l(l)*(1-LadoCC(c)))   }*PBase;
       put (ValFlujoc(c)):8:3  ;
       put (Fcmax(c)*PBase):8:3,
       put LadoCC(c)  /);
);
    execute_unload 'sol', Pg.l,ValFLujoL,ValFlujoC,V.l;
            ";

            #endregion

            return result;
        }

        public static string ObtenerModeloSecundario()
        {

            string result = @"
*******************************************************************************************
* Flujo de carga optimo AC
* Desarrollado por el COMITE DE OPERACIÓN ECONÓMICA DEL SISTEMA (COES)
* Lima - Perú
* Derechos reservados
*******************************************************************************************
Alias (NB,i), (NB,j), (NB,k), (NB,z),(NB,m);
Alias (GT,t), (GT,t2)  ;
Alias (ENL,l),(CONG,c);

set MAXN /1*2000/;
Alias (m2,MAXN),(n2,MAXN);

SCALAR
 Pholgura     /1/

 Pbase        /100.0/
 CPenalizaP   /1e4/

PARAMETER
* Generales
  ncentral
* Para unidades de generacion
  Conecg(t),PgCV(t),Pmax(t),Pmin(t),Qmin(t),Qmax(t), Ref(t) , Forzada(t),RefOrden
* Para enlaces
  Conecl(l), EnlR(l),EnlX(l),EnlG(l),  EnlB(l),LossLP(l),LossG(l),LossB(l),Tap1(l),Tap2(l),PerdPT(i),PerdQT(i)
  Congl(l),LadoC(l), Flmax(l), TipoC(l), ValFlujoL(l)
  Congc(c),LadoCC(c),Fcmax(c), TipoCC(c),ValFlujoC(c)
* Para barras
  DemP(i), DemQ(i), ShuntR(i),ShuntI(i)
  Vmin(i), Vmax(i) ,TipoV(i)
* Para compensacion reactiva
  ConecQCompR(cr),QCompRmin(cr),QCompRmax(cr)

***********************************************
* Para otros calculos
***********************************************
  G(i,j),B(i,j)
  LossLFi(i),LossTotal
  LossC(c),LadoC2(l),LadoCC2(c),Flmax2(l),Fcmax2(c)
  escentral,central(t),central1(t)
;

VARIABLES

  costo

  costo_op,costo_penP,costo_penQ,costo_penV,costo_penC
  Ang(i)                                                                                        ,
  Qg(t),QCompR(cr)

  FP1(l),FP2(l),FQ1(l),FQ2(l)
  positive variable Pg(t),V(i)

* Penalizacion sobre valores que salen fuera de rango
  positive variable DeltaP(i),DeltaV(i),DeltaC(l),DeltaCC(c)


;

********************************************
* Datos
********************************************
* Barras
  Ang.lo(i)=-pi;  Ang.up(i)= pi;
  V.l(i)=1; Ang.l(i)=0;
  DemP(i) =Demanda(i,'Pc')/PBase;
  DemQ(i) =Demanda(i,'Qc')/PBase;
  ShuntR(i)=Demanda(i,'ShuntR')/PBase;
  ShuntI(i)=Demanda(i,'ShuntI')/PBase;

* Unidades Generacion
  PgCV(t)=PgData(t,'CI1') ;
  Conecg(t)=PgData(t,'Conec');
  Pg.l(t)=PgData(t,'Pgen')/PBase;     Qg.l(t)=PgData(t,'Qgen')/PBase;
  Qmin(t)= PgData( t,'Qmin')/PBase;   Qmax(t)= PgData( t,'Qmax')/PBase;
  Ref(t)=PgData(t,'Ref');
*  Forzada(t)=PgData(t,'Forzada');    variap(t)=1$(not forzada(t));
  Qmin(t)$(Ref(t))=-inf;  Qmax(t)$(Ref(t))=inf;
  loop(t,  loop(j$(PgBus(t,j) and Ref(t)),RefOrden=ord(j)) ) ;
* poner como angulo de referencia a la generacion a la indicada como referencia
  Ang.fx(i)$(ord(i) =RefOrden) =0;

* Compensacion reactiva dinamica
  QCompR.l(cr) =CRData(cr,'Q')/PBase;
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

* Generacion
  Pmin(t)= PgData( t,'Pmin')/PBase;   Pmax(t)= PgData( t,'Pmax')/PBase;
  DeltaP.lo(i)=0;  DeltaP.up(i)=inf;  DeltaP.l(i)= 0;

* Enlaces
  Congl(l)$Conecl(l)= FData(l,'Cong');  LadoC(l)=FData(l,'Envio');   Flmax(l)=FData(l,'Pmax')/PBase;   TipoC(l)$Congl(l )=FData(l,'Tipo');  DeltaC.fx(l)$(TipoC(l)=3)=0;
  Congc(c)=CONGMax(c,'Cong');           LadoCC(c)=CONGMax(c,'Envio');Fcmax(c)=CONGMAX(c,'Pmax')/PBase; TipoCC(c)$Congc(c)=CONGMAX(c,'Tipo');DeltaCC.fx(c)$(TipoCC(c)=3)=0;
  LadoC2(l)=LadoC(l); LadoCC2(c)=LadoCC(c); Flmax2(l)=Flmax(l); Fcmax2(c)=Fcmax(c);

***************************************************************************************************************
* Identificar unidades pertenecientes a una misma central para que la reactiva no se genere y absorva a la vez
***************************************************************************************************************
  ncentral=0; central(t)=0;
  loop((t,i)$PgBus(t,i),
        loop(t2$((ord(t)<ord(t2)) and central(t2)=0 and PgBus(t2,i) ) ,
                      if(central(t)=0,
                            ncentral=ncentral+1;
                            central(t)=ncentral;
                        );
                      central(t2)=central(t);
             );
      );
";

            return result;
        }
    }
}
