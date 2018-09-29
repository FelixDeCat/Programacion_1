public interface ISujeto
{
    void NotificarObservers();
    void Suscribe(IObserver obs);
    void UnSuscribe(IObserver obs);
}