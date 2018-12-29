namespace SCG.TurboSprite
{
    // Possibly move a Sprite by updating its X and/or Y properties. If the Sprite has been moved,
    // its NotifyMoved() method should be invoked.
    public interface IMover
    {
        void MoveSprite(Sprite sprite);
    }
}