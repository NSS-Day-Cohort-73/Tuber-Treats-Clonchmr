namespace TuberTreats.Models.DTOs;

public class TuberDriverDTO 
{
    public int ID { get; set; }
    public string Name { get; set; }
    public List<TubeOrderDTO> TuberDeliveries { get; set; }
}