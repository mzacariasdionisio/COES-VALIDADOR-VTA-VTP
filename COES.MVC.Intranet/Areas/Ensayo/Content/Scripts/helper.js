function getImage(ext)
{
    var image;
    switch (ext)
    {
        case "xlsx":
        case "xls":
            image = "../../Content/images/excel.png";
            break;
        case "pdf":
            image = "../../Content/images/pdf.png";
            break;
        case "doc":
        case "docx":
            image = "../../Content/images/doc.png";
            break;
        case "jpg":
            image = "../../Content/images/jpg.png";
            break;
    }
    return image;
}