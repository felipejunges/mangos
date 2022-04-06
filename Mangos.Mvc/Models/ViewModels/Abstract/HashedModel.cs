using Mangos.Dominio.Utils;

namespace Mangos.Mvc.Models.ViewModels.Abstract
{
    public abstract class HashedModel
    {
        public int Id { get; set; }

        public int GrupoId { get; set; }

        public string? Hash { get; set; }

        private string GenerateValidationData()
        {
            return $"{this.Id.ToString().PadLeft(12, 'x')}_{GrupoId.ToString().PadLeft(12, 'y')}_mang0x_no_bug!";
        }

        public void SetValidationHash()
        {
            this.Hash = Cripto.BCryptGenerate(this.GenerateValidationData(), 10);
        }

        public bool CheckValidationHash()
        {
            if (Hash is null)
                return false;

            return Cripto.BCryptCheck(this.GenerateValidationData(), Hash);
        }
    }
}