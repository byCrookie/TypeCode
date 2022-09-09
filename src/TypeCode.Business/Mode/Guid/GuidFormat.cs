using Framework.Extensions.Enum;

namespace TypeCode.Business.Mode.Guid;

public enum GuidFormat
{
    [EnumName("N")]
    [EnumDescription("digits - 00000000000000000000000000000000")]
    [EnumKey("N")]
    N,
    [EnumName("D")]
    [EnumDescription("hyphens - 00000000-0000-0000-0000-000000000000")]
    [EnumKey("D")]
    D,
    [EnumName("B")]
    [EnumDescription("braces - {00000000-0000-0000-0000-000000000000}")]
    [EnumKey("B")]
    B,
    [EnumName("P")]
    [EnumDescription("parentheses - (00000000-0000-0000-0000-000000000000)")]
    [EnumKey("P")]
    P,
    [EnumName("X")]
    [EnumDescription("hexadecimal - {0x00000000,0x0000,0x0000,{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00}}")]
    [EnumKey("X")]
    X
}