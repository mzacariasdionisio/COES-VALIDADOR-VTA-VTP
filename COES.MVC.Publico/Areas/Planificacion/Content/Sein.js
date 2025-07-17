var controlador = siteRoot;
$(document).ready(function () {

    $("#divLineamientos").click(function () {
        document.location.href = controlador + "browser/download?url=Planificaci%C3%B3n%2FNuevos%20Proyectos%2FConexi%C3%B3n%20de%20Instalaciones%2FLineamientos%20para%20la%20elaboraci%C3%B3n%20de%20Diagramas%20Unifilares_vf.docx";
    });

    $("#divElaboracion").click(function () {
        window.open(controlador + "Operacion/Estudios/OperacionSEIN?path=Operaci%C3%B3n%2FEstudios%2FOperacion%20del%20SEIN%2FProcedimientos%20de%20Maniobra%2F1_FORMATO%20Y%20GU%C3%8DA%20PARA%20ELABORACI%C3%93N%20DE%20PROCEDIMIENTOS%20DE%20MANIOBRA%2F");

    });

    $("#divModelo").click(function () {
        window.open(controlador + "Planificacion/NuevosProyectos/OperacionComercial?path=Planificaci%C3%B3n%2FNuevos%20Proyectos%2FOperaci%C3%B3n%20Comercial%20e%20Integraci%C3%B3n%2FFORMATOS%2F");
    });

    $("#divLista").click(function () {
        window.location.href = controlador + "browser/download?url=Planificaci%C3%B3n%2FNuevos%20Proyectos%2FConexi%C3%B3n%20de%20Instalaciones%2FCONEXI%C3%93N%20DE%20INSTALACIONES%202015%20-%202021.xlsx";
    });

});

