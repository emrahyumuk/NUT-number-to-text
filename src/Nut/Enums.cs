namespace Nut
{
    public enum GenderGroup
    {
        None,
        Feminine,
        Masculine,

        /// <summary>
        /// Needed by the Slavic languages: евро and пени are neuter in Bulgarian, песо in
        /// Russian and Ukrainian. Appended so the existing values keep their numbers.
        /// </summary>
        Neuter
    }
}
