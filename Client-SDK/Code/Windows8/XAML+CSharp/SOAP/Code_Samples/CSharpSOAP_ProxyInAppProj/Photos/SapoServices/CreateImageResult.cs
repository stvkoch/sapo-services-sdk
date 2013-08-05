using Photos.PhotosReference;

namespace Photos.SapoServices
{
    public class CreateImageResult
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
