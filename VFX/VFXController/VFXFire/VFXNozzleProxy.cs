public class VFXNozzleProxy : VFXInteractor
{
    public FireType interactibleFireType;
    public float damage;
    public override void ApplyValueTo(VFXTraits traits)
    {
        target = traits;
        if(target is not VFXFireTraits fireTraits || !fireTraits.IsValidate(interactibleFireType)) return;
        
        interactOnStart = fireTraits.IsReceivedFloat(damage);
    }
}
