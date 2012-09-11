using PhotosProxy.PhotosServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotosProxy.SapoServices
{
    public sealed class CreateImageResult
    {
        private readonly Image _img;
        private readonly string _responseResult;

        public CreateImageResult(Image img, string responseResult)
        {
            _img = img;
            _responseResult = responseResult;
        }

        public Image Img
        {
            get { return _img; }
        }

        public string ResponseResult
        {
            get { return _responseResult; }
        }
    }
}
