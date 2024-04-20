using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sistema.Provaiders;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace Sistema.Helpers{
    public class HelperUploadFiles{
        private PathProvaider _pathProvaider;

        public HelperUploadFiles(PathProvaider pathProvaider){
            _pathProvaider = pathProvaider;
        }

        public async Task <String> UploadFileAsync(IFormFile formFile, string nombreImagen, Folders folder){
            string path = _pathProvaider.MapPath(nombreImagen, folder);

            using (Stream stream = new FileStream(path, FileMode.Create)){
                await formFile.CopyToAsync(stream);
            }

            return path;
        }
        
        

    }
}