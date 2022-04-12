using MgAPI.Business.JSONModels;
using MgAPI.Business.Services.Interfaces;
using MgAPI.Data;
using MgAPI.Data.Entities;
using MgAPI.Services.Authorization;
using MgAPI.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MgAPI.Data.Repositories;

namespace MgAPI.Business.Services
{
    public class WebFileService : IWebFileService
    {
        private WebFileRepository _fileRepository;
        private PostRepository _postRepository;

        public WebFileService(WebFileRepository fileRepository, PostRepository postRepository)
        {
            _fileRepository = fileRepository;
            _postRepository = postRepository;
        }

        public IEnumerable<WebFile> GetAll()
        {
            return _fileRepository.ReadAll();
        }

        public WebFile GetById(string id)
        {
            var file = _fileRepository.Read(id);
            if (file == null) throw new KeyNotFoundException("File not found");
            return file;
        }

        public WebFile Create(CreateWebFileRequest model)
        {
            byte[] fileToUpload = File.ReadAllBytes(model.Localpath);
            string fileName = model.Localpath.Split(@"\").Last();
            string extension = model.Localpath.Split(".").Last();
            string serverFileName = "file" + _fileRepository.ReadAll().Count() + "." + extension;                     
            string serverPath = @"..\MgAPI.Data\Files\" + serverFileName;

            Post post = _postRepository.Read(model.PostID);
            WebFile file = new WebFile
            {
                ID = Guid.NewGuid().ToString(),
                Post = post,
                Path = serverPath,
                Name = fileName,
                NameInServer = serverFileName,
                Extension = extension,
            };

            File.WriteAllBytes(serverPath, fileToUpload);

            _fileRepository.Create(file);
            post.Files.Add(file);

            return file;
        }

        public void Delete(string id)
        {
            WebFile fileToDelete = _fileRepository.Read(x => x.ID == id);
            if (fileToDelete == null) throw new KeyNotFoundException("File not found");
            File.Delete(fileToDelete.Path);

            _fileRepository.Delete(fileToDelete.ID);
        }
    }
}
