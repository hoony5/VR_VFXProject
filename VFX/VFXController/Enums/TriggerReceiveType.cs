public enum TriggerReceiveType
{
    Ignore,
    /// <summary>
    /// call default trigger event
    /// </summary>
    NormalTrigger,
    /// <summary>
    /// call raycast hit event
    /// </summary>
    Raycast,
    /// <summary>
    /// call both event
    /// </summary>
    Both
}