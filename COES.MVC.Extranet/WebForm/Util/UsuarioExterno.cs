using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSIC2010.Util
{
    public class UsuarioExterno
    {
        private int code;
        private EstadoXModulo statusXModulo;
        private string status;
        private string name;
        private string surName;
        private int codeEmpresa;
        private string email;
        private string phone;
        private string motivoContacto;
        private string areaName;
        private string areaLaboral;
        private string cargoUsuario;

        public int Codigo
        {
            get
            {
                return code;
            }
            set
            {
                code = value;
            }
        }

        public EstadoXModulo Estado
        {
            get
            {
                return statusXModulo;
            }
            set
            {
                statusXModulo = value;
            }
        }
        
        public string Nombre
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        public string Apellido
        {
            get
            {
                return surName;
            }
            set
            {
                surName = value;
            }
        }

        public string Cargo
        {
            get
            {
                return cargoUsuario;
            }
            set
            {
                cargoUsuario = value;
            }
        }

        public int Empresa
        {
            get
            {
                return codeEmpresa;
            }
            set
            {
                codeEmpresa = value;
            }
        }

        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                email = value;
            }
        }

        public string Telefono
        {
            get
            {
                return phone;
            }
            set
            {
                phone = value;
            }
        }

        public string AreaNombre
        {
            get
            {
                return areaName;
            }
            set
            {
                areaName = value;
            }
        }

        public string AreaLaboral
        {
            get
            {
                return areaLaboral;
            }
            set
            {
                areaLaboral = value;
            }
        }

        public string MotivoContacto
        {
            get
            {
                return motivoContacto;
            }
            set
            {
                motivoContacto = value;
            }
        }
    }

    public class EstadoXModulo
    {
        private int _codigoModulo;
        private string _descripcionEstado;


        public string DescripcionEstado
        {
            get
            {
                return _descripcionEstado;
            }
            set
            {
                _descripcionEstado = value;
            }
        }

        public int CodigoModulo
        {
            get
            {
                return _codigoModulo;
            }
            set
            {
                _codigoModulo = value;
            }
        }

        public EstadoXModulo()
        { }

        public EstadoXModulo(int codigoModulo, string descripcionEstado)
        {
            _codigoModulo = codigoModulo;
            _descripcionEstado = descripcionEstado;
        }

    }
}