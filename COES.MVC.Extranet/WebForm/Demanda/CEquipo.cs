using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSIC2010.Demanda
{
    public class CEquipo
    {

        public CEquipo()
        { }

        public CEquipo(int pi_codigoEquipo, string ps_nombreEquipo, string ps_abrevEquipo, int pi_familiaEquipo, double pd_tensionEquipo, int pi_codigoArea, int pi_codigoEmpresa)
        {
            _li_equicodi = pi_codigoEquipo;
            _ls_equinomb = ps_nombreEquipo;
            _ls_equiabrev = ps_abrevEquipo;
            _li_famcodi = pi_familiaEquipo;
            _ld_equitension = pd_tensionEquipo;
            _li_areacodi = pi_codigoArea;
            _li_emprcodi = pi_codigoEmpresa;
        }

        private int _li_equicodi;
        private string _ls_equinomb;
        private string _ls_equiabrev;
        private int _li_famcodi;
        private double _ld_equitension;
        private int _li_areacodi;
        private int _li_emprcodi;

        public int Codigo
        {
            get { return _li_equicodi; }
            set { _li_equicodi = value; }
        }

        public string Nombre
        {
            get { return _ls_equinomb; }
            set { _ls_equinomb = value; }
        }

        public string Abreviatura
        {
            get { return _ls_equiabrev; }
            set { _ls_equiabrev = value; }
        }

        public int Familia
        {
            get { return _li_famcodi; }
            set { _li_famcodi = value; }
        }

        public double Tension
        {
            get { return _ld_equitension; }
            set { _ld_equitension = value; }
        }

        public int Area
        {
            get { return _li_areacodi; }
            set { _li_areacodi = value; }
        }

        public int Empresa
        {
            get { return _li_emprcodi; }
            set { _li_emprcodi = value; }
        }

    }
}