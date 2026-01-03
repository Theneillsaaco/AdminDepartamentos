namespace AdminDepartamentos.Domain.Entities;

public partial class Pago
{
    public static Pago Create(int? numDeposito, decimal monto, int fechaPagoInDays)
    {
        if (monto <= 0)
            throw new Exception("El monto debe ser mayor a 0");
        
        if (fechaPagoInDays > 31)
            throw new Exception("La Fecha no puede ser superior a 30");
            
        return new Pago
        {
            NumDeposito = numDeposito,
            Monto = monto,
            FechaPagoInDays = fechaPagoInDays
        };
    }

    public void MarkDeleted()
    {
        if (Deleted)
            return;

        Deleted = true;
    }
}