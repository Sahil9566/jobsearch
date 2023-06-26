using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IRepository
{
    public interface IImageStorageService
    {
        Task<string> UploadImageAsync(Stream imageStream, string imageName);
        Task<bool> DeleteImageAsync(string imageUrl);
    }
}
