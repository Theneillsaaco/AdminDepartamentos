namespace AdminDepartamentos.Domain.Entities;

public partial class UnidadHabitacional
{
    public void UpdateInfo(string name, string tipo, string lightCode)
    {
        Name = name;
        Tipo = tipo;
        LightCode = lightCode;
    }
    
    public void AssignInquilino(int inquilinoId)
    {
        if (Deleted)
            throw new Exception("La unidad habitacional fue eliminada");
        
        if (Occupied)
            throw new Exception("La unidad habitacional ya esta ocupada");
        
        IdInquilinoActual = inquilinoId;
    }
    
    public void Release()
    {
        if (!Occupied)
            return;
        
        IdInquilinoActual = null;
    }

    public void MarkDeleted()
    {
        if (Deleted)
            return;
        
        Release();
        Deleted = true;
    }
}