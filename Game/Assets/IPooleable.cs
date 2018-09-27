public interface IPooleable<T>
{
    void Activate();
    void Deactivate();
    void SetOwner(T ent);
}