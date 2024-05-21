using AdminDepartamentos.Domain.Models;

namespace AdminDepartamentos.Server.Models
{
    public class PagoModel
    {
        public PagoModel() { }

        public PagoModel(PagoDomainModel pagoDomainModel)
        {
            this.IdPago = pagoDomainModel.IdPago;
            this.IdInquilino = pagoDomainModel.IdInquilino;
            this.InquilinoFirstName = pagoDomainModel.InquilinoFirstName;
            this.InquilinoLastName = pagoDomainModel.InquilinoLastName;
            this.NumDeposito = pagoDomainModel.NumDeposito;
            this.Monto = pagoDomainModel.Monto;
            this.FechaPago = pagoDomainModel.FechaPago;
            this.Retrasado = pagoDomainModel.Retrasado;
        }

        public int IdPago { get; set; }

        public int IdInquilino { get; set; }

        public string InquilinoFirstName { get; set; }

        public string InquilinoLastName { get; set; }

        public int? NumDeposito { get; set; }

        public decimal Monto { get; set; }

        public DateOnly FechaPago { get; set; }

        public bool Retrasado { get; set; }
    }
}
