/*****************************************************************************************
* Fecha de Creación: 29-05-2014
* Creado por: COES SINAC
* Descripción: Clase padre de las entidades de la aplicación
*****************************************************************************************/


using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Framework.Base.Core
{
    public class ColumnMaper
    {
        public string Nombre { get; set; }
        public DbType DbTipo { get; set; }
    }
}
