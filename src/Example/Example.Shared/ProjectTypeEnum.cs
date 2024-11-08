namespace Example.Shared;

public enum ProjectTypeEnum
{
    Packaging,
    Creative,
    Administrative
}

public static class ProjectTypeToStr
{
    public static string FrendlyName(this ProjectTypeEnum typeEnum)
        => typeEnum switch
           {
               ProjectTypeEnum.Packaging      => "Packaging",
               ProjectTypeEnum.Creative       => "Creative",
               ProjectTypeEnum.Administrative => "Administrative",
               _ => throw new ArgumentOutOfRangeException(nameof(typeEnum), typeEnum, null)
           };
}