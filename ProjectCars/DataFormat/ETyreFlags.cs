namespace ProjectCars.DataFormat
{
    public enum ETyreFlags
    {
        TyreAttached = (1 << 0),
        TyreInflated = (1 << 1),
        TyreIsOnGround = (1 << 2),
    }
}