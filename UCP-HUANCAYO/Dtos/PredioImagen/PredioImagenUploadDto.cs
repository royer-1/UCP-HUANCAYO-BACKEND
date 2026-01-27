namespace UCP_HUANCAYO.Dtos.PredioImagen
{
    public class PredioImagenUploadDto
    {
        public Guid IdPredio { get; set; }
        public IFormFile File { get; set; }
    }
}
