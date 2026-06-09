using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaceAgro.DotNetApi.Models
{
    [Table("TB_TALHAO")]
    public class Talhao
    {
        [Key]
        [Column("ID_TALHAO")]
        public int Id { get; set; }

        [Column("NOME_TALHAO")]
        public string Nome { get; set; } = string.Empty;

        [Column("CULTURA")]
        public string Cultura { get; set; } = string.Empty;

        [Column("AREA_HECTARES")]
        public double AreaHectares { get; set; }

        [Column("LATITUDE")]
        public double Latitude { get; set; }

        [Column("LONGITUDE")]
        public double Longitude { get; set; }

        [Column("ID_PRODUTOR")]
        public int IdProdutor { get; set; }

        public ICollection<LeituraSensor> Leituras { get; set; } = new List<LeituraSensor>();
    }
}
