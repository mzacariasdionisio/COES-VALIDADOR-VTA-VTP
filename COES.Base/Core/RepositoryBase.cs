/*****************************************************************************************
* Fecha de Creación: 29-05-2014
* Creado por: COES SINAC
* Descripción: Clase padre de las clases repositorio
*****************************************************************************************/

using System;
using System.Text;
using COES.Base.DataHelper;

namespace COES.Base.Core
{
    public abstract class RepositoryBase
    {
        public DbProvider dbProvider;

        public RepositoryBase(string strConn)
        {
            dbProvider = new DbProvider(strConn);
        }

        public RepositoryBase()
        { 
        
        }
        
    }

}
