using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.CortoPlazo.Helper
{
    /// <summary>
    /// Clase para obtener los codigos y nombres de las barras
    /// </summary>
    public class NombreCodigoBarra
    {
        public string CodBarra { get; set; }
        public string NombBarra { get; set; }
        public string Tension { get; set; }
        public string NomCargaZ { get; set; }
        public string DesCargaZ { get; set; }
        public double VoltajePU { get; set; }    //V
        public double Angulo { get; set; }     //T       
        public double barraPc { get; set; }     //demanda total       
        public double barraQc { get; set; }
        public double barraShunt { get; set; }  //carga del tipo shunt

        public double barraShuntR { get; set; }
        public double barraShuntI { get; set; }
        public double TipoTension { get; set; }
        public int sis { get; set; }  //sistema aislado
        public bool conect { get; set; }  //conectado
        public int Id { get; set; } //id numerico
        public bool Indicador { get; set; }
        
    }

    /// <summary>
    /// Nombre codigo linea
    /// </summary>
    public class NombreCodigoLinea
    {
        public string CodBarra1 { get; set; }
        public string CodBarra2 { get; set; }
        public string NombLinea { get; set; }
        public double Rps { get; set; }
        public double Xps { get; set; }
        public double Bsh { get; set; }
        public double GshP { get; set; }
        public double BshP { get; set; }
        public double GshS { get; set; }
        public double BshS { get; set; }
        public int BitEstado { get; set; }
        public double VoltajePU1 { get; set; }    //V
        public double Angulo1 { get; set; }     //T   
        public double VoltajePU2 { get; set; }    //V
        public double Angulo2 { get; set; }     //T     
        public double Pot { get; set; }     //Potencia
        public double L { get; set; }

        //-- Cambio por nuevo TNA --//
        public string Nombretna { get; set; }
    }

    /// <summary>
    /// Linea EMS
    /// </summary>
    public class LineaEms
    {
        public double Rps;
        public double Xps;
        public double Bsh;
        public double Gshp;
        public double Bshp;
        public double Gshs;
        public double Bshs;
        public int BitEstado;
        public double Tension1;
        public double Angulo1;
        public double Tension2;
        public double Angulo2;
        double AnguloRadianes1;
        double AnguloRadianes2;
        public Complex Zps;
        public Complex Yps;
        public Complex BshP;
        public Complex YshpP;
        public Complex YshpS;
        public Complex VpP;
        public Complex VsP;
        public Complex IpP;
        public Complex SpP;
        public double PP;
        public double B;
        public double QP;
        public Complex IsP;
        public Complex SsP;
        public double Ps;
        public double Qs;
        public bool EstadoGeneral = true;
        public NombreCodigoBarra Barra1 { get; set; }
        public NombreCodigoBarra Barra2 { get; set; }
        //nuevos parámetros
        public string IdBarra1;
        public string IdBarra2;
        public string LinOp;
        public decimal LinL;
        public int LinIslin;
        public string Orden;
        public double LinPot;
        public int Id { get; set; } //id numerico
        public double Tap1 { get; set; }
        public double Tap2 { get; set; }
        public string NombreTna { get; set; }
        //---------------
        public LineaEms(string idBarra1, string IdBarra2, int con, decimal linL, int linIslin, string orden, double rps, double xps, double bsh, double gshp, double bshp, double gshs,
        double bshs, int bitEstado, double tension1, double angulo1, double tension2, double angulo2, double linPot, double tap1, double tap2, string nombreTna)
        {
            try
            {
                //nuevos parámetros
                this.IdBarra1 = idBarra1;
                this.IdBarra2 = IdBarra2;
                this.NombreTna = nombreTna;
                //parametro 14
                this.LinOp = (con == 1) ? "yes" : "no ";
                this.LinL = linL;
                this.LinIslin = linIslin;

                //parametro 4
                this.Orden = orden;
                //---------------

                this.Rps = rps;
                this.Xps = xps;
                this.Bsh = bsh;
                this.Gshp = gshp;
                this.Gshs = gshs;

                this.Bshs = bshs;
                this.Bshp = bshp;

                this.B = bshs + bshp - bsh;

                this.BitEstado = bitEstado;
                this.Tension1 = tension1;
                this.Angulo1 = angulo1;
                this.Tension2 = tension2;

                this.Angulo2 = angulo2;

                this.LinPot = linPot;
                this.Tap1 = tap1;
                this.Tap2 = tap2;

                //cálculos
                AnguloRadianes1 = angulo1 * Math.PI / 180;
                AnguloRadianes2 = angulo2 * Math.PI / 180;

                //Zps : [v2]
                Zps = new Complex(rps, xps);

                //Yps : [w2]
                Yps = Complex.Pow(Zps, -1);

                //bsh': [x2]
                BshP = new Complex(0, bsh / 2);

                //Ysh_p': [y2]
                YshpP = new Complex(gshp, bshp);

                //Ysh_s': [z2]
                YshpS = new Complex(gshs, bshs);

                //Vp': [ad2]
                VpP = new Complex(tension1 * Math.Cos(AnguloRadianes1), tension1 * Math.Sin(AnguloRadianes1));// Vp.Cos(Tp)+jVp.Sen(Tp)

                //Vs': Vs.Cos(Ts)+jVs.Sen(Ts): [ak2]
                VsP = new Complex(tension2 * Math.Cos(AnguloRadianes2), tension2 * Math.Sin(AnguloRadianes2));// Vp.Cos(Tp)+jVp.Sen(Tp)

                //Ip': Vp'.(Yps'+Bsh'+Ysh')-Vs'.Yps': [ae2]
                IpP = (bitEstado == 0 ?
                    new Complex(0, 0) :
                    Complex.Add(Complex.Multiply(VpP, Complex.Add(Complex.Add(Yps, BshP), YshpP)), Complex.Multiply(Complex.Multiply(new Complex(-1, 0), VsP), Yps)));

                //Sp': Vp'Ip'*.100: [af2]
                SpP = Complex.Multiply(Complex.Multiply(VpP, Complex.Conjugate(IpP)), new Complex(100, 0));

                //Pp: Real(Sp'): [ag2]
                PP = SpP.Real;

                //Qp: Imag(Sp'): [ah2]
                QP = SpP.Imaginary;

                //Is': Vs'.(Yps'+Bsh'+Ysh')-Vp'.Yps': [al2]
                IsP = (bitEstado == 0 ?
                    new Complex(0, 0) :
                    Complex.Add(Complex.Multiply(VsP, Complex.Add(Complex.Add(Yps, BshP), YshpS)), Complex.Multiply(Complex.Multiply(new Complex(-1, 0), VpP), Yps)));

                //Ss': Vs'Is'*.100: [am2]
                SsP = Complex.Multiply(Complex.Multiply(VsP, Complex.Conjugate(IsP)), new Complex(100, 0));

                //Ps
                Ps = SsP.Real;

                //Qs
                Qs = SsP.Imaginary;

            }
            catch
            {
                EstadoGeneral = false;
            }

        }
    }

    /// <summary>
    /// Trafos EMS
    /// </summary>
    public class TrafoEms
    {
        public double[,] DatoTrafo;
        public double NivelTension1;
        public double NivelTension2;
        public double Tension1;
        public double Angulo1;
        public double Tension2;
        public double Angulo2;
        public double Tension3;
        public double Angulo3;
       

        public Complex VpP;
        public Complex Z1P;
        public Complex Y1;
        public Complex VsP;
        public Complex Z2P;
        public Complex Y2;
        public Complex Nt;
        public Complex Z3P;
        public Complex Y3;
        public Complex Ym1;
        public Complex Y12;
        public Complex Y23;
        public Complex Y13;
        public Complex Vp;
        public Complex Vs;
        public Complex Vt;
        public Complex Ip;
        public Complex Sp;
        public double Pp;
        public double Qp;
        public Complex Is0;
        public Complex Ss;
        public double Ps;
        public double Qs;
        public Complex It;
        public Complex St;
        public double Pt;
        public double Qt;

        public bool EstadoGeneral = true;

        //nuevos parámetros
        public string IdBarra1;
        public string IdBarra2;
        public string IdBarra3;
        public string Orden;
        public string NombreTna;
        public double G;
        public double B;
        public string Op;
        public NombreCodigoBarra Barra1 { get; set; }
        public NombreCodigoBarra Barra2 { get; set; }
        public NombreCodigoBarra Barra3 { get; set; }


        public double Vf1;
        public double Angf1;
        public double G1;
        public double B1;
        public double R1;
        public double R2;
        public double R3;
        public double X1;
        public double X2;
        public double X3;
        public double Tap1;
        public double Tap2;
        public double Tap3;
        public double Pot1;
        public double Pot2;
        public double Pot3;

        //---------------


        public TrafoEms(double[,] datoTrafo, string nombre4, double nivelTension1, double nivelTension2, double tension1,
        double angulo1, double tension2, double angulo2, double tension3, double angulo3, double nivelTension3, string nombreTna)
        {
            try
            {
                //nuevos parámetros
                this.IdBarra1 = datoTrafo[0, 0].ToString();
                this.IdBarra2 = datoTrafo[0, 1].ToString();
                this.IdBarra3 = datoTrafo[0, 2].ToString();
                this.Orden = nombre4;
                this.NombreTna = nombreTna;
                this.G = datoTrafo[0, 7];
                this.B = datoTrafo[0, 8];
                Op = (datoTrafo[0, 11] == 1) ? "yes" : "no ";
                //---------------

                //- Codigo agregado

                double cw = datoTrafo[0, 4];              

                double r1 = 0;
                double x1 = 0;
                double g1 = 0;
                double b1 = 0;
                double tap1 = 0;
                double tap2 = 0;
                double tap3 = 0;
                double pot = 0;

                //- Trafo 2D
                if (datoTrafo[0, 2] == 0)
                {
                    r1 = datoTrafo[1, 0];
                    x1 = datoTrafo[1, 1];
                    g1 = this.G / 3;
                    b1 = this.B / 3;
                    pot = datoTrafo[1, 2];

                    if (cw == 1)
                    {
                        tap1 = datoTrafo[2, 0];
                        tap2 = datoTrafo[3, 0];
                    }
                    else if (cw == 2)
                    {
                        tap1 = nivelTension1 / datoTrafo[2, 0];
                        tap2 = nivelTension2 / datoTrafo[3, 0];
                    }
                    else if (cw == 3)
                    {
                        tap1 = datoTrafo[2, 1] / datoTrafo[2, 0];
                        tap2 = datoTrafo[3, 1] / datoTrafo[3, 0];
                    }

                    this.R1 = r1;
                    this.X1 = x1;
                    this.G1 = g1;
                    this.B1 = -1 * b1;
                    this.Pot1 = pot;
                    this.Tap1 = tap1;
                    this.Tap2 = tap2;


                }
                //- Trafo 3D
                else 
                {
                    double vf = datoTrafo[1, 9];
                    double angf = datoTrafo[1, 10];

                    if (Math.Abs(vf - tension1) < 0.001 && Math.Abs(angf - angulo1) < 0.001)
                    {
                        vf = tension1;
                        angf = angulo1;
                    }
                    if (Math.Abs(vf - tension2) < 0.001 && Math.Abs(angf - angulo2) < 0.001)
                    {
                        vf = tension2;
                        angf = angulo2;
                    }
                    if (Math.Abs(vf - tension3) < 0.001 && Math.Abs(angf - angulo3) < 0.001)
                    {
                        vf = tension3;
                        angf = angulo3;
                    }

                    if(cw == 1)
                    {
                        tap1 = datoTrafo[2, 0];
                        tap2 = datoTrafo[3, 0];
                        tap3 = datoTrafo[4, 0];
                    }
                    else if (cw == 2)
                    {
                        tap1 = nivelTension1 / datoTrafo[2, 0];
                        tap2 = nivelTension2 / datoTrafo[3, 0];
                        tap3 = nivelTension3 / datoTrafo[4, 0];
                    }
                    else if (cw == 3)
                    {
                        tap1 = datoTrafo[2, 1] / datoTrafo[2, 0];
                        tap2 = datoTrafo[3, 1] / datoTrafo[3, 0];
                        tap3 = datoTrafo[4, 1] / datoTrafo[4, 0];
                    }

                    double r12 = datoTrafo[1, 0];
                    double x12 = datoTrafo[1, 1];
                    double r23 = datoTrafo[1, 3];
                    double x23 = datoTrafo[1, 4];
                    double r31 = datoTrafo[1, 6];
                    double x31 = datoTrafo[1, 7];

                    r1 = (r12 + r31 - r23) / 2;
                    double r2 = (r12 + r23 - r31) / 2;
                    double r3 = (r23 + r31 - r12) / 2;
                    x1 = (x12 + x31 - x23) / 2;
                    double x2 = (x12 + x23 - x31) / 2;
                    double x3 = (x23 + x31 - x12) / 2;

                    g1 = r1 / (r1 * r1 + x1 * x1);
                    double g2 = r2 / (r2 * r2 + x2 * x2);
                    double g3 = r3 / (r3 * r3 + x3 * x3);
                    b1 = -x1 / (r1 * r1 + x1 * x1);
                    double b2 = -x2 / (r2 * r2 + x2 * x2);
                    double b3 = -x3 / (r3 * r3 + x3 * x3);

                    double v1 = tension1;
                    double v2 = tension2;
                    double v3 = tension3;
                    double ang1 = angulo1 * 3.14159 / 180;
                    double ang2 = angulo2 * 3.14159 / 180;
                    double ang3 = angulo3 * 3.14159 / 180;
                    double angfx = angf * 3.14159 / 180;

                    double A1 = g1 + g2 + g3 + this.G / 2;
                    double A2 = -b1 - b2 - b3 - this.B / 2;
                    double alfa1 = -v1 * g1 / tap1 * Math.Cos(ang1) - v2 * g2 / tap2 * Math.Cos(ang2) - v3 * g3 / tap3 * Math.Cos(ang3) + v1 * b1 / tap1 * Math.Sin(ang1) + v2 * b2 / tap2 * Math.Sin(ang2) + v3 * b3 / tap3 * Math.Sin(ang3);
                    double alfa2 = -v1 * g1 / tap1 * Math.Sin(ang1) - v2 * g2 / tap2 * Math.Sin(ang2) - v3 * g3 / tap3 * Math.Sin(ang3) - v1 * b1 / tap1 * Math.Cos(ang1) - v2 * b2 / tap2 * Math.Cos(ang2) - v3 * b3 / tap3 * Math.Cos(ang3);
                    angfx = Math.Atan((alfa1 * A2 + alfa2 * A1) / (alfa1 * A1 - alfa2 * A2));
                    vf = alfa2 / A2 * Math.Cos(angfx) - alfa1 / A2 * Math.Sin(angfx); // Preguntar a percy
                    angfx = angfx * 180 / 3.14159;

                    //
                    this.Vf1 = vf;
                    this.Angf1 = angfx;
                    this.G1 = this.G / 3;
                    this.B1 = -1 * this.B / 3;
                    this.R1 = r1;
                    this.R2 = r2;
                    this.R3 = r3;
                    this.X1 = x1;
                    this.X2 = x2;
                    this.X3 = x3;
                    this.Tap1 = tap1;
                    this.Tap2 = tap2;
                    this.Tap3 = tap3;
                    this.Pot1 = datoTrafo[1, 2];
                    this.Pot2 = datoTrafo[1, 5];
                    this.Pot3 = datoTrafo[1, 8];
                }

                //- Fin codigo agregado

                this.DatoTrafo = datoTrafo;
                this.NivelTension1 = nivelTension1;
                this.NivelTension2 = nivelTension2;
                this.Tension1 = tension1;
                this.Angulo1 = angulo1;
                this.Tension2 = tension2;
                this.Angulo2 = angulo2;
                this.Tension3 = tension3;
                this.Angulo3 = angulo3;       

                //Vp' = Vp.Cos(Tp)+j.Vp.Sen(Tp): [x2]
                VpP = new Complex(datoTrafo[2, 0] * Math.Cos(datoTrafo[2, 2] * Math.PI / 180), datoTrafo[2, 0] * Math.Sin(datoTrafo[2, 2] * Math.PI / 180));

                //Z1'=r1+jx1: 0.5(Zps'+Zpt'-Zst'): [y2]
                Z1P = new Complex(0.5 * (datoTrafo[1, 0] - datoTrafo[1, 3] + datoTrafo[1, 6]),
                    0.5 * (datoTrafo[1, 1] - datoTrafo[1, 4] + datoTrafo[1, 7]));

                //Y1 =1/Z1': [z2]
                Y1 = Complex.Pow(Z1P, -1);

                //Vs': [aa2]
                VsP = new Complex(datoTrafo[3, 0] * Math.Cos(datoTrafo[3, 2] * Math.PI / 180), datoTrafo[3, 0] * Math.Sin(datoTrafo[3, 2] * Math.PI / 180));

                //Z2'=r2+jx2: [ab2]
                Z2P = new Complex(0.5 * (datoTrafo[1, 0] + datoTrafo[1, 3] - datoTrafo[1, 6]), 0.5 * (datoTrafo[1, 1] + datoTrafo[1, 4] - datoTrafo[1, 7]));

                //Y2: [ac2]
                Y2 = Complex.Pow(Z2P, -1);

                //Nt(PU): [ad2]
                Nt = new Complex(
                    (datoTrafo[0, 2] == 0 ? 1 : datoTrafo[4, 0]) * Math.Cos(datoTrafo[4, 2] * Math.PI / 180),
                    (datoTrafo[0, 2] == 0 ? 0 : datoTrafo[4, 0]) * Math.Sin(datoTrafo[4, 2] * Math.PI / 180));

                //Z3'=r3+jx3: [ae2]
                Z3P =
                    datoTrafo[0, 2] == 0 ? new Complex(0, 0) :
                    new Complex(0.5 * (-datoTrafo[1, 0] + datoTrafo[1, 3] + datoTrafo[1, 6]), 0.5 * (-datoTrafo[1, 1] + datoTrafo[1, 4] + datoTrafo[1, 7]));

                //Y3: [af2]
                Y3 =
                    (datoTrafo[0, 2] == 0 ? new Complex(0, 0) : Complex.Pow(Z3P, -1));

                //Ym1: [ag2]
                //Ym1 =
                //    Complex.Multiply(
                //    new Complex(datoTrafo[0, 7], datoTrafo[0, 8]),
                //    new Complex(
                //    ((datoTrafo[0, 2] == 0) || (datoTrafo[3, 1] == datoTrafo[4, 1])) ?
                //    Math.Pow(nivelTension1 / datoTrafo[2, 1], 2) :
                //    Math.Pow(nivelTension2 / datoTrafo[3, 1], 2), 0));

                Ym1 = new Complex(datoTrafo[0, 7], datoTrafo[0, 8]);

                //Y12: [ah2]
                Y12 =
                    Complex.Divide(Complex.Multiply(Y1, Y2), Complex.Add(Complex.Add(Y1, Y2), Y3));

                //Y23: [ai2]
                Y23 =
                    Complex.Divide(Complex.Multiply(Y2, Y3), Complex.Add(Complex.Add(Y1, Y2), Y3));

                //Y13: [aj2]
                Y13 =
                    Complex.Divide(Complex.Multiply(Y1, Y3), Complex.Add(Complex.Add(Y1, Y2), Y3));

                //Vp: [an2]
                Vp = new Complex(tension1 * Math.Cos(angulo1 * Math.PI / 180), tension1 * Math.Sin(angulo1 * Math.PI / 180));


                //Vs: [AU3]
                Vs =
                    new Complex(tension2 * Math.Cos(angulo2 * Math.PI / 180), tension2 * Math.Sin(angulo2 * Math.PI / 180));


                //Vt: [BB3]
                Vt =
                    datoTrafo[0, 2] == 0 ?
                    new Complex(0, 0) :
                    new Complex(tension3 * Math.Cos(angulo3 * Math.PI / 180), tension3 * Math.Sin(angulo3 * Math.PI / 180));



                //Ip: [ao2]          

                Complex ipAux = (datoTrafo[0, 2] == 0 || datoTrafo[3, 1] == datoTrafo[4, 1]) ? Complex.Multiply(Ym1, new Complex(Math.Pow(datoTrafo[2, 0], 2), 0)) : new Complex(0, 0);
                Complex ipAux2 = Complex.Add(Complex.Add(Y12, Y13), ipAux);
                Complex ipAux3 = Complex.Divide((Complex.Multiply(Vp, ipAux2)), Complex.Multiply(Complex.Conjugate(VpP), VpP));
                Complex ipAux4 = Complex.Divide((Complex.Multiply(Vs, Y12)), Complex.Multiply(Complex.Conjugate(VpP), VsP));
                Complex ipAux5 = Complex.Divide((Complex.Multiply(Vt, Y13)), Complex.Multiply(Complex.Conjugate(VpP), Nt));
                Complex ipAux6 = Complex.Multiply(new Complex(-1, 0), ipAux5);
                Complex ipAux7 = Complex.Multiply(new Complex(-1, 0), ipAux4);

                Ip =
                datoTrafo[0, 11] == 0 ?
                new Complex(0, 0) :
                Complex.Add(Complex.Add(ipAux3, ipAux7), ipAux6);


                //Sp [ap2]
                Sp = Complex.Multiply(Complex.Multiply(Vp, Complex.Conjugate(Ip)), new Complex(100, 0));

                //Pp [aq2]
                Pp = Math.Round(Sp.Real, 1);

                //Qp [ar2]
                Qp = Math.Round(Sp.Imaginary, 1);



                //Is [av2]
                Complex is0Aux1 = (datoTrafo[0, 2] == 0 || datoTrafo[3, 1] == datoTrafo[4, 1]) ?
                        new Complex(0, 0) : Complex.Multiply(Ym1, new Complex(Math.Pow(datoTrafo[3, 0], 2), 0));

                Complex is0Aux2 = Complex.Divide(Complex.Multiply(Vp, Y12), Complex.Multiply(Complex.Conjugate(VsP), VpP));
                Complex is0Aux3 = Complex.Multiply(Vs, Complex.Add(Complex.Add(Y12, Y23), is0Aux1));
                Complex is0Aux4 = Complex.Divide(is0Aux3, Complex.Multiply(Complex.Conjugate(VsP), VsP));
                Complex is0Aux5 = Complex.Divide((Complex.Multiply(Vt, Y23)), Complex.Multiply(Complex.Conjugate(VsP), Nt));

                Is0 =
                    datoTrafo[0, 11] == 0 ? new Complex(0, 0) : Complex.Add(Complex.Add(Complex.Multiply(new Complex(-1, 0), is0Aux2), is0Aux4),Complex.Multiply(new Complex(-1, 0), is0Aux5));

                //Ss [aw2]
                Ss = Complex.Multiply(Complex.Multiply(Vs, Complex.Conjugate(Is0)), new Complex(100, 0));

                //Ps [ax2]
                Ps = Math.Round(Ss.Real, 1);

                //Qs [ay2]
                Qs = Math.Round(Ss.Imaginary, 1);

                //It [bc2]
                Complex itAux1 = Complex.Multiply(new Complex(-1, 0), Complex.Divide((Complex.Multiply(Vp, Y13)),
                    Complex.Multiply(Complex.Conjugate(Nt), VpP)));

                Complex itAux2 = Complex.Multiply(new Complex(-1, 0), Complex.Divide((Complex.Multiply(Vs, Y23)),
                    Complex.Multiply(Complex.Conjugate(Nt), VsP)));

                It =
                    datoTrafo[0, 11] == 0 ? new Complex(0, 0) : Complex.Add(Complex.Add(itAux1, itAux2), Complex.Divide((Complex.Multiply(Vt, Complex.Add(Y13, Y23))), Complex.Multiply(Complex.Conjugate(Nt), Nt)));

                //St [bd2]
                St =
                    Complex.Multiply(Complex.Multiply(Vt, Complex.Conjugate(It)), new Complex(100, 0));

                //Pt [be2]: =ROUNDDOWN(IMREAL(BD3),1)
                Pt = St.Real; 

                //Qt [bf2]
                Qt = St.Imaginary;

            }
            catch
            {
                EstadoGeneral = false;
            }
        }

    }

    /// <summary>
    /// Clase para obtener los codigos y nombres de las cargas
    /// </summary>
    public class Carga
    {
        public string CodCarga { get; set; }
        public double Carga1 { get; set; } //elemento 6 del raw PC
        public double Carga2 { get; set; } //elemento 6 del raw QC
        public int Conn { get; set; }
        public string NomBarra { get; set; }

    }

    /// <summary>
    /// Clase para obtener los codigos y nombres de los shunt
    /// </summary>
    public class Shunt
    {
        public string CodShunt { get; set; }
        public double Carga { get; set; } //elemento 5 del raw
        public double CargaAdicional { get; set; } //elemento 4 del raw
        public int Conn { get; set; }
    }

    /// <summary>
    /// Estructura para leer los datos de la parte de generacion del .raw
    /// </summary>
    public class Generator
    {
        public string Codbarra { get; set; }
        public string ID { get; set; }
        public double Qmax { get; set; }
        public double Qmin { get; set; }
        public double Qg { get; set; }
        public double Pg { get; set; }
        public int Ope { get; set; }
        public int Traslado { get; set; }
    }

    /// <summary>
    /// Clave para el manejo de switched shunt
    /// </summary>
    public class SwitchedShunt
    {
        public int Conn { get; set; }
        public string Codigo { get; set; }
        public int N { get; set; }
        public double Val { get; set; }
        public double Max { get; set; }
        public double Min { get; set; }
        public int Correlativo { get; set; }
        public int BarraID { get; set; }
    }


    /// <summary>
    /// Clase para manejar 
    /// </summary>
    public class RelacionCongestion
    {
        public int Periodo { get; set; }
        public string CodigoNCP { get; set; }
        public int PeriodoInicio { get; set; }
        public int PeriodoFin { get; set; }
        public string Indicador { get; set; }
    }

    /// <summary>
    /// Clase para pintar los datos generados
    /// </summary>
    public class RegistroGenerado
    {
        public decimal Costo1 { get; set; }
        public string BarraNombre { get; set; }
        public int BarraID { get; set; }
        public string Nombretna { get; set; }
        public int BarraID2 { get; set; }
        public string GenerID { get; set; }
        public string Tipo { get; set; }
        public string Pmax { get; set; }
        public string Pmin { get; set; }
        public int IndOpe { get; set; }
        public string IndNcv { get; set; }
        public string Pmax1 { get; set; }
        public string Ci1 { get; set; }
        public string Pmax2 { get; set; }
        public string Ci2 { get; set; }
        public string Pmax3 { get; set; }
        public string Ci3 { get; set; }
        public string Pmax4 { get; set; }
        public string Ci4 { get; set; }
        public string Pmax5 { get; set; }
        public string Ci5 { get; set; }
        public string Tension { get; set; }
        public string Tension2 { get; set; }
        public decimal PGen { get; set; }
        public decimal Potencia { get; set; }
        public decimal PotenciaMinima { get; set; }
        public decimal PotenciaMaxima { get; set; }
        public int? Ccombcodi { get; set; }
        public EqRelacionDTO Relacion { get; set; }
        public int Cod { get; set; }
        public int Correlativo { get; set; }
        public bool EsCicloCombinado { get; set; }
        public int NroCicloCombinado { get; set; }
        public int IdModoOperacion { get; set; }
        public int IdCalificacion { get; set; }

        //- Campos adicionales para cc
        public double PotGeneradaCC { get; set; }
        public double VelocidadCargaCC { get; set; }
        public double VelocidadDescargaCC { get; set; }
        public double PotenciaMaximaCC { get; set; }
        public double PotenciaMinimaCC { get; set; }
        public bool ExistenciaRsf { get; set; }
        public decimal ValorRsf { get; set; }
        public double Qmin { get; set; }
        public double Qmax { get; set; }
        public double Qg { get; set; }
        public double Pg { get; set; }
        public string NombreBarra { get; set; }
        public bool Conectado { get; set; }
        public int IdBarra { get; set; }
        public string Referencia { get; set; }
        public string GenFor { get; set; }
        public int BarraSis { get; set; }
        public string GenCalificacion { get; set; }

        public bool IndLimTrans { get; set; }
        public double CostoIncremental { get; set; }
        public string Indnoforzada { get; set; }

        #region Mejoras CMgN
        public int ModoOperacion { get; set; }
        public int? Equicodi { get; set; }

        public int IndRegistro { get; set; }
        #endregion

        #region Ticket_6245
        public string IndCmgh { get; set; }
        #endregion
    }

    /// <summary>
    /// Permite mapear el resultado GAMS
    /// </summary>
    public class ResultadoGams
    {
        public string Nombarra { get; set; }
        public decimal Energia { get; set; }
        public decimal Congestion { get; set; }
        public decimal Total { get; set; }
    }


    /// <summary>
    /// Clase para manejar los modos de operacion
    /// </summary>
    public class RelacionModoOperacion
    {
        public int Periodo { get; set; }
        public string CodigoNCP { get; set; }
        public int PeriodoInicio { get; set; }
        public int PeriodoFin { get; set; }
        public string Indicador { get; set; }
    }

    /// <summary>
    /// Clase para manejo de resultados
    /// </summary>
    public class ResultadoProcesoMasivo
    {
        public string FechaProceso { get; set; }
        public bool Resultado { get; set; }
        public string Mensaje { get; set; }
    }

    /// <summary>
    /// Clase para enviar datos del resultado de la validacion
    /// </summary>
    public class ResultadoValidacion
    {
        public bool Indicador { get; set; }
        public bool IndicadorPSSE { get; set; }
        public string ValidacionPSSE { get; set; }
        public bool IndicadorNCP { get; set; }
        public string ValidacionNCP { get; set; }
        public bool IndicadorRSF { get; set; }
        public string ValidacionRSF { get; set; }
        public bool IndicadorNegativo { get; set; }
        public string ValidacionNegativo { get; set; }
        public bool IndicadorMO { get; set; }
        public string ValidacionMO { get; set; }
        public bool IndicadorEMS { get; set; }
        public string ValidacionEMS { get; set; }
        public List<ResultadoValidacionItem> ListaModosOperacion { get; set; }
        public List<ResultadoValidacionItem> ListaOperacionEMS { get; set; }
        public bool IndicadorGeneracionNegativa { get; set; }
        public string ValidacionGeneracionNegativa { get; set; }
        public bool IndicadorComparativoRAW { get; set; }
        public string ValidacionCompartivoRAW { get; set; }

        public bool IndicadorMaximoCM { get; set; }
        public string ValidacionMaximoCM { get; set; }
    }

    /// <summary>
    /// Detalle de la validacion del proceso
    /// </summary>
    public class ResultadoValidacionItem
    {
        public string BarraNombre { get; set; }
        public string GenerID { get; set; }
        public string Tension { get; set; }
        public decimal Potencia { get; set; }
        public int IndOpe { get; set; }
    }


    #region FIT - VALORIZACION

    /// <summary>
    /// Permite mapear el resultado GAMS
    /// </summary>
    public class ResultadoGamsAnalisis
    {
        public ResultadoResumen Resumen { get; set; }
        public List<ResultadoCompensacionesTermicas> ListaCompensacionTermica { get; set; }
        public List<ResultadoGeneracionTermica> ListaGeneracionTermica { get; set; }
        public List<ResultadoGeneracionHidraulica> ListaGeneracionHidraulica { get; set; }

        public List<ResultadoCongestion> ListaCongestion { get; set; }
        public List<ResultadoCongestion> ListaCongestionConjunta { get; set; }
        public List<ResultadoCongestion> ListaCongestionRegionArriba { get; set; }
        public List<ResultadoCongestion> ListaCongestionRegionAbajo { get; set; }
    }

    /// <summary>
    /// Permite mapear el resultado GAMS
    /// </summary>
    public class ResultadoResumen
    {
        public decimal GeneracionTermica { get; set; }
        public decimal GeneracionHidraulica { get; set; }
        public decimal GeneracionDemandaTotal { get; set; }
    }


    /// <summary>
    /// Permite mapear el resultado GAMS
    /// </summary>
    public class ResultadoCompensacionesTermicas
    {
        public string Termica { get; set; }
        public string Calificacion { get; set; }
        public decimal Compensacion { get; set; }
        public int CodigoEquipo { get; set; }

    }

    /// <summary>
    /// Permite mapear el resultado GAMS
    /// </summary>
    public class ResultadoGeneracionTermica
    {
        public string Termica { get; set; }
        public int CodigoEquipo { get; set; }
        public decimal PgMW { get; set; }

    }

    /// <summary>
    /// Permite mapear el resultado GAMS
    /// </summary>
    public class ResultadoGeneracionHidraulica
    {
        public string Hidraulica { get; set; }
        public int CodigoEquipo { get; set; }
        public decimal PhMW { get; set; }

    }

    public class ResultadoCongestion
    {
        public string NombreCongestion { get; set; }
        public decimal Limite { get; set; }
        public decimal Envio { get; set; }
        public decimal Recepcion { get; set; }
        public decimal Congestion { get; set; }
        public decimal GenLimite { get; set; }
        public decimal Generacion { get; set; }
        public int Tipo { get; set; }
    }

    #endregion
}
