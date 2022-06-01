using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.Validaciones
{
    public class PesoArchivoValidacion : ValidationAttribute
    {
        private readonly int pesoMaximoenMB;

        public PesoArchivoValidacion(int pesoMaximoenMB)
        {
            this.pesoMaximoenMB = pesoMaximoenMB;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            IFormFile formFile = value as IFormFile;

            if (formFile == null)
            {
                return ValidationResult.Success;
            }

            if(formFile.Length > pesoMaximoenMB * 1024 * 1024)
            {
                return new ValidationResult($"El peso del archivo no debe ser mayor a {pesoMaximoenMB}mb");
            }

            return ValidationResult.Success;

        }
    }
}
