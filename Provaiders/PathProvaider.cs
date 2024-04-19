using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Sistema.Provaiders{
    public enum Folders {
     Images = 1, Documents = 2, Temp = 3
    }

    public class PathProvaider{
        private readonly IWebHostEnvironment _hostEnvironment;

        public PathProvaider(IWebHostEnvironment hostEnvironment){
            _hostEnvironment = hostEnvironment;
        }

        public string MapPath(string fileName, Folders folder){
            string carpeta = "";

            if(folder == Folders.Images){
                carpeta = "images";
            }
        
            else if(folder == Folders.Documents){
                carpeta = "documents";
            }

            string path = Path.Combine(_hostEnvironment.WebRootPath, carpeta, fileName);

            if(folder == Folders.Temp){
                path = Path.Combine(Path.GetTempPath(), fileName);
            }

            return path;
        }

         public List<string> GetFilesList(string folderPath)
        {
            List<string> filesList = new List<string>();

            try
            {
                string webRootPath = _hostEnvironment.WebRootPath;
                string folderFullPath = Path.Combine(webRootPath, folderPath);

                if (Directory.Exists(folderFullPath))
                {
                    string[] files = Directory.GetFiles(folderFullPath);
                    foreach (string file in files)
                    {
                        filesList.Add(Path.GetFileName(file));
                    }
                }
                else
                {
                    Console.WriteLine($"El directorio {folderPath} no existe.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener la lista de archivos: {ex.Message}");
            }

            return filesList;
        }

        public void DeleteFile(string fileName)
        {
            try
            {
                string webRootPath = _hostEnvironment.WebRootPath;
                string filePath = Path.Combine(webRootPath, fileName);
                Console.WriteLine("ruta: " + filePath);
                Console.WriteLine("ruta2: " + webRootPath);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    Console.WriteLine($"El archivo {fileName} ha sido eliminado correctamente.");
                }
                else
                {
                    Console.WriteLine($"El archivo {fileName} no existe.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar el archivo {fileName}: {ex.Message}");
            }
        }
    }
}