namespace DevToys.Tools.Tools.EncodersDecoders.JsonWebToken;

internal enum JsonWebTokenGridRows
{
    Settings,
    SubContainer
}

internal enum GridColumns
{
    Stretch,
    Left,
    Right
}

internal enum JsonWebTokenExpirationGridRow
{
    Content
}

internal enum JsonWebTokenExpirationGridColumn
{
    Year,
    Month,
    Day,
    Hour,
    Minute
}

internal enum JsonWebTokenDecodeGridRow
{
    Settings,
    Token,
    TokenPayload
}

internal enum JsonWebTokenDecodeRow
{
    Payload
}

internal enum JsonWebTokenDecodeColumns
{
    Left,
    Right
}

internal enum JsonWebTokenEncoderGridRows
{
    Settings,
    Token,
    TokenPayload,
    TokenSignature
}
