using System;

[Serializable]
public class DistToElement<T>
{
    public T element;
    public float weight;

    public T Element { get { return element; } }
    public float Weight { get { return weight; } }

    public DistToElement()
    {
    }

    public DistToElement(T element, float weight)
    {
        this.element = element;
        this.weight = weight;
    }
}
