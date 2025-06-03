public class PlayerConditions
{
    public bool IsCollidingBelow { get; set; }
    public bool IsCollidingAbove { get; set; }
    public bool IsCollidingLeft { get; set; }
    public bool IsCollidingRight { get; set; }
    public bool IsFalling { get; set; }
    public bool IsJumping { get; set; }
    public bool IsDashing { get; set; }

    /// <summary>True when we’ve hit a walkable slope in HorizontalCollision</summary>
    public bool IsOnSlope { get; set; }
    /// <summary>The angle (in degrees) of that slope</summary>
    public float SlopeAngle { get; set; }

    public void Reset()
    {
        IsCollidingAbove = false;
        IsCollidingBelow = false;
        IsCollidingRight = false;
        IsCollidingLeft = false;
        IsFalling = false;
        IsOnSlope = false;
        SlopeAngle = 0f;
    }
}
