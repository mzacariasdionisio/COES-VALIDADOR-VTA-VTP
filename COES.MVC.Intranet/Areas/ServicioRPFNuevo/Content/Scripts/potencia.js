function loadInfoFile(fileName, fileSize)
{
    $('#fileInfo').html(fileName + " (" + fileSize + ")");  
}

function loadValidacionFile(mensaje)
{
    $('#fileInfo').html(mensaje);  
}

function mostrarProgreso(porcentaje)
{
    $('#progreso').text(porcentaje + "%");
}
