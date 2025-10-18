public interface IFacing
{
    int FacingSign { get; }          // 1 = вправо, -1 = влево
    void UpdateFacing(float xInput); // обновить направление по вводу
}
