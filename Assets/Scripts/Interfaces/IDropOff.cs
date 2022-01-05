public interface IDropOff
{
    Resource Resource { get; set; }

    void Deliver(int amount);
}
