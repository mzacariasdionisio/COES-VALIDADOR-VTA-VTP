using System;
using System.Reflection;

namespace COES.WEBAPI.MedidoresGeneracion.Areas.HelpPage.ModelDescriptions
{
    public interface IModelDocumentationProvider
    {
        string GetDocumentation(MemberInfo member);

        string GetDocumentation(Type type);
    }
}