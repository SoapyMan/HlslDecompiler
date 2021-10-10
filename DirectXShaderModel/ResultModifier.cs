namespace HlslDecompiler.DirectXShaderModel
{
    // https://msdn.microsoft.com/en-us/library/windows/hardware/ff552738%28v=vs.85%29.aspx
    public enum ResultModifier
    {
        None,
        Saturate = 0x1,
        PartialPrecision = 0x2,
        Centroid = 0x4
    }
}
